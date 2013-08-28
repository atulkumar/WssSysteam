'************************************************************************************************************
' Page                 : - Daily Actions 
' Purpose              : - This page shows the DailyReport of the employees    
' Date		    		   Author						Modification Date					Description
' 27/11/2007		   Suresh Kharod 					             					        Created
'
' Notes: 
' Code:
'************************************************************************************************************
#Region "Refered Assemblies"
Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
#End Region

Partial Class Reports_ReportsPages_DailyActionsNew
    Inherits System.Web.UI.Page
#Region "Page Level Variables "
    Private crDocument As New ReportDocument
    Private objReports As clsDSReports
    Private params As ParameterFieldDefinitions
    Private paramValues As ParameterValues
    Private paramCallNo As ParameterFieldDefinition
    Private paramCustomerID As ParameterFieldDefinition
    Private paramDate As ParameterFieldDefinition
    Private paramDiscrete As ParameterDiscreteValue
    Private paramRangeValue As ParameterRangeValue
    Private formulas As FormulaFieldDefinitions
    Public formulaServerName As FormulaFieldDefinition
    Private strServerName As String
    Public mstrCallNumber As String
#End Region

#Region " Page Load Function"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim txthiddenImage = Request.Form("txthiddenImage")
            HIDSCRID.Value = Request.QueryString("ScrID")
            If Session("PropCompanyType") = "SCM" Then
                ddlCompany.Enabled = True
            Else
                ddlCompany.Enabled = False
            End If
            If txthiddenImage = "Logout" Then
                LogoutWSS()
            End If
            If Not IsPostBack = True Then
                fill_company()
                fill_Project(HttpContext.Current.Session("PropCompanyID"))
                fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                dtFromDate.Text = SetDateFormat(Today)
                dtToDate.Text = SetDateFormat(Today)
            Else
                ShowReport()
            End If

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Attach"
                            Response.Write("<script>window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');</script>")
                        Case "OK"
                    End Select
                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Function Show() "
    Private Sub show()
        Try

            Select Case Request("ip")
                Case Nothing
                    Response.Redirect("ReportsIndex.aspx")
                Case "da"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS:" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        Else
                            ddlProject.Enabled = True
                        End If
                    End If


                    If Not IsPostBack Then
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        Else
                            ddlProject.Enabled = True
                        End If
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    Response.Write("<head><title>" & "DAILY REPORT" & "</title></head>")

                    cpnlReport.Text = "DAILY REPORT"
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "DAILY REPORT"
                    lblHead.Text = "DAILY REPORT"

                    tdLevels.Visible = False
                    If IsPostBack Then
                        'Showreport(4)
                    End If
                Case "da2"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS:" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        Else
                            ddlProject.Enabled = True
                        End If
                    End If


                    If Not IsPostBack Then
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        Else
                            ddlProject.Enabled = True
                        End If
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    Response.Write("<head><title>" & "TASK SUMMARY REPORT" & "</title></head>")

                    cpnlReport.Text = "TASK SUMMARY "
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "TASK SUMMARY "
                    lblHead.Text = "TASK SUMMARY "

                    If IsPostBack Then
                        'Showreport(5)
                    End If
                Case "da3"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS:" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        Else
                            ddlProject.Enabled = True
                        End If
                    End If


                    If Not IsPostBack Then
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        Else
                            ddlProject.Enabled = True
                        End If
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    Response.Write("<head><title>" & "DAILY REPORT" & "</title></head>")

                    cpnlReport.Text = "DAILY REPORT"
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "DAILY REPORT"
                    lblHead.Text = "DAILY REPORT"

                    If IsPostBack Then
                        'Showreport(6)
                    End If
                Case Else
                    Response.Redirect("ReportsIndex.aspx")
            End Select
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region " Fill Drop Down List Boxes"
    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsDSReports
            dt = objReports.extractCompany(2)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            If ddlCompany.Items(0).Text <> "--ALL--" Then
                ddlCompany.Items.Insert(0, New ListItem("--ALL--", 0))
            End If
            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If
            If Request("cid") <> Nothing Then
                ddlCompany.SelectedValue = CInt(Request("cid"))
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
            objReports = New clsDSReports
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
                ddlProject.Items.Insert(0, New ListItem("--ALL--", 0))
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsDSReports
            dt = objReports.extractTaskOwner(companyID, projectID, 2)
            '  dt = objReports.extractCustomer(companyID, projectID, category)
            ddlEmployee.DataSource = dt
            ddlEmployee.DataTextField = "Name"
            ddlEmployee.DataValueField = "AddressNo"
            ddlEmployee.DataBind()
            If ddlEmployee.Items(0).Text <> "--Select--" Then
                ddlEmployee.Items.Insert(0, "--Select--")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub
#End Region

#Region "Function ShowReport()"
    Public Sub ShowReport()

        crDocument = New ReportDocument
        Dim Reportpath As String
        Reportpath = Server.MapPath("DailyActionReportDS.rpt")
        crDocument.Load(Reportpath)

        Dim dsDailyActions As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim strQuery As String
        Dim dtFrom As String
        Dim dtTo As String
        Dim CompID As Integer = Val(ddlCompany.SelectedValue)
        Dim TaskOwnerID As Integer = Val(ddlEmployee.SelectedValue)
        Dim ProjectID As Integer = Val(ddlProject.SelectedValue)
        dtFrom = dtFromDate.Text
        dtTo = dtToDate.Text
        Dim htCols As New Hashtable
        htCols.Add("ActionDate", 1)
        strQuery = "select AM_NU9_Action_Number ActionNo,AM_NU9_Task_Number TaskNo,AM_NU9_Call_Number CallNo,AM_NU9_Comp_ID_FK CompID,AM_VC8_Supp_Owner SuppOwner,AM_VC100_Action_type ActionType,convert(varchar,AM_DT8_Action_Date) as ActionDate,AM_CH1_IsInvoiced IsInvoiced,AM_VC_2000_Description ActionDesc,AM_FL8_Used_Hr UsedHours,a.ci_vc36_name as EmpName,b.ci_vc36_name as CompanyName,TM_NU9_Task_no_PK TaskNumber,TM_VC1000_Subtsk_Desc TaskDesc,TM_NU9_Project_ID as ProjectID ,PR_VC20_Name as ProjectName from ( (((T040031 left outer join t010011 a on a.ci_nu8_address_number=AM_VC8_Supp_Owner )left outer join T010011 b on b.ci_nu8_address_number=AM_NU9_Comp_ID_FK)left outer join T040021 on TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Task_no_PK=AM_NU9_Task_Number )left outer join  T210011 on TM_NU9_Project_ID=PR_NU9_Project_ID_Pk and TM_NU9_Comp_ID_FK=PR_NU9_Comp_ID_FK )  "

        Dim strFilter1 As String = "  where T040031.AM_DT8_Action_Date between '" & dtFrom & "'and'" & dtTo & "' "
        If Val(ddlCompany.SelectedValue) <> 0 Then
            If Val(ddlProject.SelectedValue) <> 0 Then
                If Val(ddlEmployee.SelectedValue) <> 0 Then
                    strFilter1 += "and AM_NU9_Comp_ID_FK=" & CompID & " and T040021.TM_NU9_Project_ID=" & ProjectID & " and  AM_VC8_Supp_Owner=" & TaskOwnerID & ""
                Else
                    strFilter1 += "and AM_NU9_Comp_ID_FK=" & CompID & " and T040021.TM_NU9_Project_ID=" & ProjectID & " "
                End If
            Else
                If Val(ddlEmployee.SelectedValue) <> 0 Then
                    strFilter1 += "and AM_NU9_Comp_ID_FK=" & CompID & "  and  AM_VC8_Supp_Owner=" & TaskOwnerID & ""
                Else
                    strFilter1 += "and AM_NU9_Comp_ID_FK=" & CompID & " "
                End If
            End If
        Else
            If Val(ddlProject.SelectedValue) <> 0 Then
                If Val(ddlEmployee.SelectedValue) <> 0 Then
                    strFilter1 += "and T040021.TM_NU9_Project_ID=" & ProjectID & " and  AM_VC8_Supp_Owner=" & TaskOwnerID & ""
                Else
                    strFilter1 += "and T040021.TM_NU9_Project_ID=" & ProjectID & " "
                End If
            Else
                If Val(ddlEmployee.SelectedValue) <> 0 Then
                    strFilter1 += " and AM_VC8_Supp_Owner=" & TaskOwnerID & ""
                Else
                    strFilter1 += ""
                End If
            End If
        End If
        strQuery += strFilter1

        SQL.Search("DailyActions", "WSSReportsD", "ExtractHours", strQuery, dsDailyActions, "Atul", "Sharma")
        SetDataTableDateFormat(dsDailyActions.Tables(0), htCols)
        crDocument.SetDataSource(dsDailyActions)
        crvReport.EnableDrillDown = False
        crvReport.ReportSource = crDocument
        crvReport.DataBind()
    End Sub

#End Region

#Region "All other Functions "
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
    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("../../home.aspx", False)
    End Sub
    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        ddlProject.Items.Clear()
        ddlEmployee.Items.Clear()
        fill_Project(Val(ddlCompany.SelectedValue))
        fill_employee(Val(ddlCompany.SelectedValue), 0, 2)
    End Sub

    Private Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        ddlEmployee.Items.Clear()
        fill_employee(Val(ddlCompany.SelectedValue), Val(ddlProject.SelectedValue), 2)
    End Sub
#End Region

    Private Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        ShowReport()
    End Sub
End Class
