#Region " Refered Assemblies"
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
#End Region
Partial Class Reports_DSCallTaskReport
    Inherits System.Web.UI.Page
#Region "Page Level Variables"
    Private crDocument As New ReportDocument
    Private objReports As clsReportData
    Private strServerName As String
    Public mstrCallNumber As String
    Private dtFrom As String
    Private dtTo As String
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "');")
            ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "');")
            'ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlAssignedBy.ClientID & "','" & ddlEmployee.ClientID & "');")
            'ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlAssignedBy.ClientID & "','" & ddlEmployee.ClientID & "');")
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                fill_company()
                fill_Project(HttpContext.Current.Session("PropCompanyID"))
                fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                'fill_AssignedBy(HttpContext.Current.Session("PropCompanyID"), 0, 2)

            Else
                FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                'FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS$" & ddlAssignedBy.ID, New ListItem("--ALL--", 0))
                FillReport()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
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

    Private Sub fill_Project(ByVal comapnyID As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If ddlCompany.SelectedIndex = 0 Then
                dt = objReports.extractProject(0)
            Else
                dt = objReports.extractProject(comapnyID)
            End If
            ddlProject.DataSource = dt
            ddlProject.DataTextField = "Name"
            ddlProject.DataValueField = "ID"
            ddlProject.DataBind()
            If ddlProject.Items(0).Text <> "--ALL--" Then
                ddlProject.Items.Insert(0, "--ALL--")
            End If
        Catch ex As Exception
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)

        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractTaskOwner(companyID, projectID, 2)
            ddlEmployee.DataSource = dt
            ddlEmployee.DataTextField = "Name"
            ddlEmployee.DataValueField = "AddressNo"
            ddlEmployee.DataBind()
            ddlEmployee.Items.Insert(0, "--ALL--")
            'dt = Nothing

            'dt = New DataTable
            'dt = objReports.extractCallOwner(companyID, projectID, 2)
            'ddlAssignedBy.DataSource = dt
            'ddlAssignedBy.DataTextField = "Name"
            'ddlAssignedBy.DataValueField = "AddressNo"
            'ddlAssignedBy.DataBind()
            'ddlAssignedBy.Items.Insert(0, "--ALL--")

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_AssignedBy(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        'Dim dt As New DataTable
        'objReports = New clsReportData
        'dt = objReports.extractCallOwner(companyID, projectID, category)
        'ddlAssignedBy.DataSource = dt
        'ddlAssignedBy.DataTextField = "Name"
        'ddlAssignedBy.DataValueField = "AddressNo"
        'ddlAssignedBy.DataBind()
        'ddlAssignedBy.Items.Insert(0, "--ALL--")
    End Sub

    Sub FillReport()
        Try
            Dim s As String
            crvReport.ReportSource = Nothing
            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
            If dtFrom = Nothing And dtTo = Nothing Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Date....")
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                Dim From As String = "01/01/1932"
                dtFrom = CDate(From)
            ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                If CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
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
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
            Else
                lstError.Items.Clear()
            End If
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crTaskNew.rpt")
            crDocument.Load(Reportpath)

            Dim DSCallTask As New DataSet
            Dim strFilter As String = ""
            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text

            Dim htCols As New Hashtable
            htCols.Add("ActionDate", 2)

            If Session("PropAdmin") = "1" Then
                s = " select TM_VC8_Task_Type 'TaskType', AM_NU9_Comp_ID_FK 'CMPID',AM_NU9_Call_Number 'CallNo',AM_NU9_Task_Number 'TaskNo',AM_VC8_Supp_Owner 'SuppOwnerID',am_dt8_action_date 'ActionDate',Tm_fl8_est_hr 'estHr',sum(AM_FL8_Used_Hr)UsedHrs,cm_nu9_project_id 'ProjectId', T010011.CI_VC36_Name 'compName',Supp.CI_VC36_Name 'OwnerName' from T040031 ,T040021,t040011,T010011 ,T010011 Supp where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and AM_NU9_Task_Number=TM_NU9_Task_No_PK and  T010011.CI_NU8_Address_Number=AM_NU9_Comp_ID_FK  and  Supp.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Call_Number=TM_NU9_Call_No_FK and AM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and AM_FL8_Used_Hr>0"

            Else
                s = "SELECT CM_NU9_Call_No_PK as CallNo, TM_NU9_Task_no_PK as TaskNo,CM_VC2000_Call_Desc as callDesc,TM_VC50_Deve_status as status,TM_VC1000_Subtsk_Desc as TaskDesc,AM_VC_2000_Description  as actiondesc,convert(varchar,AM_DT8_Action_Date) as ActionDate,AM_FL8_Used_Hr as UsedHrs,PR_VC20_Name as Project,Company.CI_VC36_name as CompName,TaskSupOwner.CI_VC36_name as TaskOwner,SupOwner.CI_VC36_name as ActionOwner, CM_NU9_Comp_Id_FK AS CompId, CM_VC8_Call_Type AS CallType, CM_VC200_Work_Priority AS CallPriority,CN_VC20_Call_Status AS CallStatus, CM_VC100_Subject AS CallSubject,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_VC100_Action_Type as actionType  FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner,t210011,t010011 TaskSupOwner  where  CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number and  (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and  AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and TM_VC8_Supp_Owner=TaskSupOwner.CI_nu8_address_number and  (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and  TM_NU9_Task_no_PK=AM_NU9_Task_Number) and CM_nu9_project_id=PR_NU9_Project_ID_Pk and PR_NU9_Comp_ID_FK=CM_NU9_Comp_Id_FK "

            End If
            If ddlCompany.SelectedIndex <> -1 Then
                If ddlCompany.SelectedItem.Value.Trim <> "--ALL--" Then
                    strFilter += " and AM_NU9_Comp_Id_FK=" & ddlCompany.SelectedItem.Value.Trim & ""
                End If
            End If
            If ddlProject.SelectedIndex <> -1 Then
                If ddlProject.SelectedItem.Text.Trim <> "--ALL--" Then
                    strFilter += " and cm_nu9_project_id=" & ddlProject.SelectedItem.Value.Trim & ""
                End If
            End If

            If ddlEmployee.SelectedIndex <> -1 Then
                If ddlEmployee.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and AM_VC8_Supp_Owner='" & ddlEmployee.SelectedItem.Value.Trim & "'"
                End If
            End If
           

            If dtFrom <> Nothing Then
                If IsDate(dtFrom) = True Then
                    strFilter += " and convert(datetime,convert(varchar,AM_DT8_Action_Date,101),101) >='" & CDate(dtFrom) & "'"
                End If
            End If

            If dtTo <> Nothing Then
                If IsDate(dtTo) = True Then
                    strFilter += " and convert(datetime,convert(varchar,AM_DT8_Action_Date,101),101) <='" & CDate(dtTo) & "'"
                End If
            End If

            strFilter += " group by cm_nu9_project_id,AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_VC8_Supp_Owner,TM_VC8_Task_Type,am_dt8_action_date,Tm_fl8_est_hr,T010011.CI_VC36_Name,Supp.CI_VC36_Name  order by AM_VC8_Supp_Owner "
            s += strFilter
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("element1", "WSSReportsD", "ExtractCallNo", s, DSCallTask, "Mandeep", "Kaur")
            SetDataTableDateFormat(DSCallTask.Tables(0), htCols)
            'cpnlError.Visible = False
            crDocument.SetDataSource(DSCallTask.Tables(0))
            crvReport.EnableDrillDown = True
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            'clsReport.LogonInformation(crDocument)
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
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
#End Region

End Class
