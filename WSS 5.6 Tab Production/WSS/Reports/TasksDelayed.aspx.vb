#Region "Reffered Assemblies"
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
#End Region
Partial Class Reports_TasksDelayed
    Inherits System.Web.UI.Page

#Region " Page Level Varaibles"

    Private crDocument As New ReportDocument
    Private objReports As clsReportData
    Public mstrCallNumber As String

#End Region

#Region " Page Load Event"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            'Put user code to initialize the page here


            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            crvReport.ToolbarStyle.Width = New Unit("900px")

            Dim txthiddenImage = Request.Form("txthiddenImage")
            HIDSCRID.Value = Request.QueryString("ScrID")
            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If

            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                fill_company()
            End If
            show()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region "Function Show"

    Private Sub show()
        Try
            crvReport.ReportSource = Nothing

            If Request("ip") = Nothing Then
                Response.Redirect("ReportsIndex.aspx")
            End If

            If Request("ip").ToString = "TDR" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 794
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
            End If

            'Response.Write("<head><title>" & "CALL SUMMARY" & "</title></head>")
            lblHead.Text = "Task Delay Report"
            cpnlReport.Text = "Task Delay Report"
            cpnlRS.Text = "Task Delay Report"
            If IsPostBack Then
                ShowReport(1)
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

    Private Sub fill_company()
        Try
            If Request("ip").ToString = "TDR" Then
                Dim dt As New System.Data.DataTable
                objReports = New clsReportData
                dt = objReports.extractCompany(2)
                ddlCompany.DataSource = dt
                ddlCompany.DataTextField = "Name"
                ddlCompany.DataValueField = "ID"
                ddlCompany.DataBind()
                'ddlCompany.Items.Insert(0, "--ALL--")
                If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                    ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
                End If
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

#Region "Function ShowReport To Show the Report "

    Private Sub ShowReport(ByVal id As Integer)
        Try
            Dim strActionType As String = "Internal"
            Dim strCompanyType As String = Session("PropCompanyType")
            If id = 1 Then
                If Request("ip").ToString = "TDR" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crTaskDelay.rpt")
                    crDocument.Load(Reportpath)

                End If
                Dim dtFrom As String
                Dim dtTo As String
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text

                If dtFrom = Nothing And dtTo = Nothing Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please Select Date....")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    Dim From As String = "01/01/1932"
                    dtFrom = CDate(From)
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    If CDate(dtFrom) > Date.Now() Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Date From can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        dtTo = Date.Now.ToShortDateString
                        lstError.Items.Clear()
                    End If

                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    If CDate(dtFrom) > CDate(dtTo) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Date From can not be greater than To Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    ElseIf CDate(dtFrom) > Date.Now() Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Date From can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                Else
                    lstError.Items.Clear()
                End If
                FillReportTDR()
            End If
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            If intPageFlag = 1 Then
                crvReport.SeparatePages = True
            ElseIf intPageFlag = 2 Then
                crvReport.SeparatePages = False
            End If
            If ddlgroup.SelectedValue = 0 Then
                crDocument.DataDefinition.FormulaFields("GroupFormula").Text = "{TbDelayedTasks.Tm_vc8_supp_owner}"
            Else
                crDocument.DataDefinition.FormulaFields("GroupFormula").Text = "{TbDelayedTasks.priority}"
            End If
            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False
            'cpnlError.Visible = False
            crvReport.SeparatePages = False
            crvReport.EnableDrillDown = False
            'clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "All Other Functions "
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        crDocument.Dispose()

    End Sub
    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
        HttpContext.Current.Session("PropCallNumber") = 0
        HttpContext.Current.Session("PropTaskNumber") = 0
    End Sub



    Private Sub FillReportTDR()
        Dim s As String
        Dim dtFrom As String = dtFromDate.Text
        Dim dtTo As String = dtToDate.Text
        Dim dsCP As New DSDelayedTasks
        Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
        Dim htCols As New Hashtable
        htCols.Add("LastActionDate", 2)
        htCols.Add("Taskdate", 2)
        htCols.Add("Estclosedate", 2)

        If Session("PropAdmin") = "1" Then
            s = "select ci_vc36_name,convert(varchar,(Max(am_dt8_action_date_auto))) as  LastActionDate,DATEDIFF(day ,Max(am_dt8_action_date_auto),getdate()) as NotTouchedFor,t.Compid,t.callno,t.priority,t.tm_nu9_task_no_pk as Task_no,Task_Status,UM_VC50_UserID as Tm_vc8_supp_owner,convert(varchar,Tm_Dt8_Task_date) as Taskdate ,convert(varchar,tm_dt8_est_close_date) as Estclosedate,Delayed from (select cm_nu9_comp_id_fk as compID,cm_nu9_call_no_pk as callno,cm_vc200_work_priority as priority, tm_nu9_task_no_pk,tm_vc50_deve_status as Task_Status,Tm_vc8_supp_owner,Tm_Dt8_Task_date,tm_dt8_est_close_date,isnull(DATEDIFF(day ,tm_dt8_est_close_date,getdate()),0) as Delayed  from t040011,t040021 where CM_DT8_Request_Date >='" + dtFrom + "' and CM_DT8_Request_Date<='" + dtTo + "' and  cm_nu9_comp_id_fk='" & strCompany & "' and cm_nu9_comp_id_fk=tm_nu9_comp_id_fk and cm_nu9_call_no_pk=Tm_nu9_Call_no_Fk and tm_vc50_deve_status<>'closed')as t,t040031,t010011,T060011 where t.CompID='" & strCompany & "' and t.compID=t040031.am_nu9_comp_id_fk and t.callno=t040031.am_nu9_call_number  and UM_IN4_Address_No_FK=Tm_vc8_supp_owner and am_nu9_comp_id_fk = t010011.ci_nu8_address_number group by t.compID,t.callno,t.priority,t.tm_nu9_task_no_pk,t.Task_Status,Tm_vc8_supp_owner,Tm_Dt8_Task_date,tm_dt8_est_close_date, Delayed,ci_vc36_name,UM_VC50_UserID order by t.compID,t.callno"
        Else

            s = "select ci_vc36_name,(Max(am_dt8_action_date_auto))) as  LastActionDate,DATEDIFF(day ,Max(am_dt8_action_date_auto),getdate()) as NotTouchedFor,t.Compid,t.callno,t.priority,t.tm_nu9_task_no_pk as Task_no,Task_Status,UM_VC50_UserID as Tm_vc8_supp_owner,convert(varchar,Tm_Dt8_Task_date) as Taskdate ,convert(varchar,tm_dt8_est_close_date) as Estclosedate,Delayed from (select cm_nu9_comp_id_fk as compID,cm_nu9_call_no_pk as callno,cm_vc200_work_priority as priority, tm_nu9_task_no_pk,tm_vc50_deve_status as Task_Status,Tm_vc8_supp_owner,Tm_Dt8_Task_date,tm_dt8_est_close_date,isnull(DATEDIFF(day ,tm_dt8_est_close_date,getdate()),0) as Delayed  from t040011,t040021 where CM_DT8_Request_Date >='" + dtFrom + "' and CM_DT8_Request_Date<='" + dtTo + "' and  cm_nu9_comp_id_fk='" & strCompany & "' and cm_nu9_comp_id_fk=tm_nu9_comp_id_fk and cm_nu9_call_no_pk=Tm_nu9_Call_no_Fk and tm_vc50_deve_status<>'closed')as t,t040031,t010011,T060011 where Tm_vc8_supp_owner='" & Session("PropUserID") & "' and  t.CompID='" & strCompany & "' and t.compID=t040031.am_nu9_comp_id_fk and t.callno=t040031.am_nu9_call_number  and UM_IN4_Address_No_FK=Tm_vc8_supp_owner and am_nu9_comp_id_fk = t010011.ci_nu8_address_number group by t.compID,t.callno,t.priority,t.tm_nu9_task_no_pk,t.Task_Status,Tm_vc8_supp_owner,Tm_Dt8_Task_date,tm_dt8_est_close_date, Delayed,ci_vc36_name,UM_VC50_UserID order by t.compID,t.callno"
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        SQL.Search("TbDelayedTasks", "WSSReportsD", "ExtractCallNo", s, dsCP, "jagmit", "sidhu")
        SetDataTableDateFormat(dsCP.Tables(0), htCols)

        crDocument = New ReportDocument
        Dim Reportpath As String
        Reportpath = Server.MapPath("crTaskDelay.rpt")
        crDocument.Load(Reportpath)

        crDocument.SetDataSource(dsCP)

    End Sub

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub


#End Region
End Class
