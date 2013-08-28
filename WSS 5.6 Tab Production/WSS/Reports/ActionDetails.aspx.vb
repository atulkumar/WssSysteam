
Imports ION.Data
Imports System.Text
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

Partial Class Reports_ActionDetails
    Inherits System.Web.UI.Page

    Private crDocument As New ReportDocument
    Private objReports As clsReportData
    Private dtFrom As String
    Private dtTo As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        '    'Put user code to initialize the page here
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        crvReport.ToolbarStyle.Width = New Unit("950px")
        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage = "Logout" Then
            FormsAuthentication.SignOut()
            Call ClearVariables()
            Response.Redirect("../Login/Login.aspx")
        End If
        If Not IsPostBack Then
            imgOK.Attributes.Add("OnClick", "ShowImg();")
            fill_company()
        Else
            FillReport()
        End If
        'Catch ex As Exception
        '    Dim str As String = ex.Message.ToString
        'End Try
    End Sub
#Region "Fill Drop Down List Boxes"

    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCompany(2)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            ddlCompany.Items.Insert(0, New ListItem("--All--", 0))

            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If

        Catch ex As Exception

            Dim str As String = ex.Message.ToString


        Finally
            objReports = Nothing
        End Try
    End Sub

    Sub FillReport()
        Try
            Dim s As String
            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
            crvReport.ReportSource = Nothing
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
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    dtTo = Date.Now.ToShortDateString
                    lstError.Items.Clear()
                End If

            ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                If CDate(dtFrom) > CDate(dtTo) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than To Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
            Else
                lstError.Items.Clear()
            End If
            crDocument = New ReportDocument
            Dim Reportpath As String

            Reportpath = Server.MapPath("crActionDetail.rpt")
            crDocument.Load(Reportpath)

            Dim ds As New DataSet
            Dim strFilter As String = ""
            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text

            Dim htCols As New Hashtable
            htCols.Add("ActionDate", 2)

            s = " SELECT Company.CI_VC36_name as CompName,PR_VC20_Name as SubCategory, TM_VC8_Task_Type as TaskType,convert(varchar,AM_DT8_Action_Date) as ActionDate,AM_VC_2000_Description  as actiondesc,AM_FL8_Used_Hr as UsedHrs,SupOwner.CI_VC36_name as ActionOwner,TaskCoordinator.CI_VC36_name as Coordinator,PI_VC8_Department as Department ,Manager.CI_VC36_name As Manager, CM_NU9_Comp_Id_FK AS CompId,AM_VC8_Supp_Owner as ActOwnerID FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner,t210011,t010011 TaskSupOwner, t010011 TaskCoordinator,t010011 Manager,T010043 where  CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number and Manager.cI_nu8_address_Number=PI_NU9_Manager and PI_NU8_ADDRESS_NO=AM_VC8_Supp_Owner and TaskCoordinator.CI_nu8_address_number=*CM_NU9_Coordinator   and  (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and  AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and TM_VC8_Supp_Owner=TaskSupOwner.CI_nu8_address_number and  (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and  TM_NU9_Task_no_PK=AM_NU9_Task_Number) and CM_nu9_project_id=PR_NU9_Project_ID_Pk and PR_NU9_Comp_ID_FK=CM_NU9_Comp_Id_FK "

            If ddlCompany.SelectedIndex <> -1 Then
                If ddlCompany.SelectedItem.Value.Trim <> "--ALL--" Then
                    strFilter += " and CM_NU9_Comp_Id_FK=" & ddlCompany.SelectedItem.Value.Trim & ""
                End If
            End If
            If dtFrom <> Nothing Then
                If IsDate(dtFrom) = True Then
                    strFilter += " and convert(datetime,convert(varchar,am_dt8_action_date,101),101) >='" & CDate(dtFrom) & "'"
                End If
            End If

            If dtTo <> Nothing Then
                If IsDate(dtTo) = True Then
                    strFilter += " and convert(datetime,convert(varchar,am_dt8_action_date,101),101) <='" & CDate(dtTo) & "'"
                End If
            End If

            strFilter += " order by CM_VC200_Work_Priority asc,CM_NU9_Call_No_PK"
            s += strFilter
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("DSActionDetails", "WSSReportsD", "ExtractCallNo", s, ds, "Mandeep", "Kaur")
            SetDataTableDateFormat(ds.Tables(0), htCols)
            cpnlError.Visible = False
            crDocument.SetDataSource(ds)

            crvReport.EnableDrillDown = True
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception

        End Try

    End Sub

#Region "Function Show"

    Private Sub show()
        Try
            If Request("ip") = Nothing Then
                Response.Redirect("ReportsIndex.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")

                'chkStatus.Attributes.Add("OnClick", "return CheckStatus('" & chkLstStatus.ClientID & "','" & chkStatus.ClientID & "');")
            End If

            If Request("ip").ToString = "ME" Then


                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 950
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If

            End If

            'Response.Write("<head><title>" & "MONTH END REPORT" & "</title></head>")
            lblHead.Text = "Call Task Action"
            cpnlReport.Text = "Call Task Action"
            cpnlRS.Text = "Call Task Action"

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region
#End Region

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
