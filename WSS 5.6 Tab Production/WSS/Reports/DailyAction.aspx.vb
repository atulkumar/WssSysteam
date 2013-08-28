'************************************************************************************************************
' Page                 : - Daily Actions 
' Purpose              : - This page shows the DailyReport of the employees    
' Date		    		   Author						Modification Date					Description
' 27/04/06				   Atul 					             					        Created
'
' Notes: 
' Code:
'************************************************************************************************************
#Region "Refered Assemblies"
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Imports CrystalDecisions.Shared
#End Region
Partial Class Reports_DailyAction
    Inherits System.Web.UI.Page
#Region "Page Level Variables "
    Private crDocument As New ReportDocument
    Private objReports As clsReportData
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
        Try

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

            imgOK.Attributes.Add("OnClick", "ShowImg();")
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "');")
            ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "');")

            If Session("PropCompanyType") = "SCM" Then
                ddlCompany.Enabled = True
            Else
                ddlCompany.Enabled = False
            End If

            If txthiddenImage = "Logout" Then
                LogoutWSS()
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

            show()
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

                    'Response.Write("<head><title>" & "DAILY REPORT" & "</title></head>")

                    cpnlReport.Text = "DAILY REPORT"
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "DAILY REPORT"
                    lblHead.Text = "DAILY REPORT"
                    tdLevels.Visible = False
                    If IsPostBack Then
                        Showreport(4)
                    End If
                Case "da2"

                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 863
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block
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

                    'Response.Write("<head><title>" & "TASK SUMMARY REPORT" & "</title></head>")

                    cpnlReport.Text = "TASK SUMMARY "
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "TASK SUMMARY "
                    lblHead.Text = "TASK SUMMARY "

                    If IsPostBack Then
                        Showreport(5)
                    End If
                Case "da3"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
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

                    'Response.Write("<head><title>" & "DAILY REPORT" & "</title></head>")

                    cpnlReport.Text = "DAILY REPORT"
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "DAILY REPORT"
                    lblHead.Text = "DAILY REPORT"
                  
                    If IsPostBack Then
                        Showreport(6)
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
            objReports = New clsReportData
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
            objReports = New clsReportData
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

    Private Sub Showreport(ByVal id As Integer)
        Try

            ' mstrCallNumber = CType(ddlCallNo.SelectedItem.Value.Trim, String)
            Dim strRecordSelectionFormula As String
            If id = 4 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crDailyReport.rpt")
                crDocument.Load(Reportpath)

            ElseIf id = 5 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crTaskSummaryDS.rpt")
                crDocument.Load(Reportpath)





            ElseIf id = 6 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crsTaskSummary.rpt")
                crDocument.Load(Reportpath)

            Else
                Exit Sub
            End If
            Dim dtFrom As String
            Dim dtTo As String

            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text

            Dim dtToDate1 As DateTime
            Dim i As Integer

            If dtTo = Nothing And dtFrom = Nothing Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Date... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            Else
                'cpnlError.Visible = False
            End If
            If dtFrom <> Nothing And dtTo <> Nothing Then
                If DateDiff(DateInterval.Day, CType(dtFrom, Date), CType(dtTo, Date)) < 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("To date should be grater than From date ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    'cpnlError.Visible = False
                End If
            End If
            If id <> 5 Then
                If dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += "   {Command_1.ActionDate} <= " & "#" & dtTo & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += "   {Command_1.ActionDate} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += "   {Command_1.ActionDate} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"

                End If



                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "and" & "{Command_1.AM_NU9_Comp_ID_FK}=" & strCompany
                End If
                If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strProject As String = ddlProject.SelectedItem.Value.Trim
                    strRecordSelectionFormula += " and" & "{Command_1.ProjectID}=" & strProject
                End If

                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += " and" & " {Command_1.AM_VC8_Supp_Owner}=" & strEmployee
                    End If
                Else
                    strRecordSelectionFormula += "and " & " {Command_1.AM_VC8_Supp_Owner}=" & Session("PropUserID")
                End If
            End If
            If id = 5 Then
                Dim dsTask As New DataSet
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                ' Dim strQuery As String = "select AM_NU9_Action_Number,AM_NU9_Task_Number,AM_NU9_Call_Number,AM_NU9_Comp_ID_FK,AM_VC8_Supp_Owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate,AM_FL8_Used_Hr,a.ci_vc36_name as Name,b.ci_vc36_name as CompanyName,TM_NU9_Task_no_PK,TM_NU9_Project_ID as ProjectID ,PR_VC20_Name as ProjectName from ( (((T040031 left outer join t010011 a on a.ci_nu8_address_number=AM_VC8_Supp_Owner)left outer join t010011 b on b.ci_nu8_address_number=AM_NU9_Comp_ID_FK)left outer join t040021 on TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Task_no_PK=AM_NU9_Task_Number)left outer join  t210011 on TM_NU9_Project_ID=PR_NU9_Project_ID_Pk and TM_NU9_Comp_ID_FK=PR_NU9_Comp_ID_FK) where AM_NU9_Comp_ID_FK=" & ddlCompany.SelectedItem.Value.Trim & " and PR_NU9_Project_ID_Pk=" & ddlProject.SelectedItem.Value.Trim & "  and convert(datetime,convert(varchar,AM_DT8_Action_Date,101)) >= '" & dtFrom.ToString & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<= '" & dtTo.ToString & "'  "
                Dim strQuery As String = "select AM_NU9_Action_Number,AM_NU9_Task_Number,AM_NU9_Call_Number,AM_NU9_Comp_ID_FK,AM_VC8_Supp_Owner,AM_VC100_Action_type, AM_DT8_Action_Date as ActionDate,AM_CH1_IsInvoiced,AM_VC_2000_Description,AM_FL8_Used_Hr,a.ci_vc36_name as Name,b.ci_vc36_name as CompanyName,TM_NU9_Task_no_PK,TM_VC50_Deve_status,TM_VC1000_Subtsk_Desc,TM_NU9_Project_ID as ProjectID ,PR_VC20_Name as ProjectName from ( (((T040031 left outer join t010011 a on a.ci_nu8_address_number=AM_VC8_Supp_Owner)left outer join t010011 b on b.ci_nu8_address_number=AM_NU9_Comp_ID_FK) left outer join t040021 on TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Task_no_PK=AM_NU9_Task_Number)left outer join  t210011 on TM_NU9_Project_ID=PR_NU9_Project_ID_Pk and TM_NU9_Comp_ID_FK=PR_NU9_Comp_ID_FK) where "

                If dtFrom <> "" And dtTo = "" Then
                    strQuery += "convert(datetime,convert(varchar,AM_DT8_Action_Date,101)) >= '" & dtFrom.ToString & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<= '" & DateTime.Now.ToString() & "'"
                End If
                If dtFrom = "" And dtTo <> "" Then
                    strQuery += "convert(datetime,convert(varchar,AM_DT8_Action_Date,101)) >= '" & DateTime.Now.ToString() & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<= '" & dtTo.ToString() & "'"
                End If
                If dtFrom <> "" And dtTo <> "" Then
                    strQuery += "convert(datetime,convert(varchar,AM_DT8_Action_Date,101)) >= '" & dtFrom.ToString() & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<= '" & dtTo.ToString() & "'"
                End If
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strQuery += " and" & " AM_NU9_Comp_ID_FK=" & strCompany
                End If
                If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strProject As String = ddlProject.SelectedItem.Value.Trim
                    strQuery += " and" & " PR_NU9_Project_ID_Pk=" & strProject
                End If


                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strQuery += " and" & " AM_VC8_Supp_Owner=" & strEmployee
                    End If
                Else
                    strQuery += "and " & " AM_VC8_Supp_Owner=" & Session("PropUserID")
                End If

                SQL.Search("dsTask", "WSSReportsD", "ExtractHours", strQuery, dsTask, "Atul", "Sharma")
                crDocument.SetDataSource(dsTask)
                crvReport.ReportSource = crDocument
                crvReport.DataBind()
                '

                'Dim strRecordSelectionFormula1 As String = strRecordSelectionFormula.Replace("Command_1", "dsTask")
                'With crDocument.Subreports(0)
                '    .RecordSelectionFormula = strRecordSelectionFormula1
                '    .SetDataSource(dsTask)
                '    .DataDefinition.FormulaFields("Date").Text = "'" & dtFrom.ToString & "  till  " & dtTo.ToString & "'"

                'End With

                Select Case rdlLevels.SelectedIndex
                    Case 0
                        For i = 0 To crDocument.ReportDefinition.Sections.Count - 1
                            If crDocument.ReportDefinition.Sections.Item(i).Name <> "rhs1" Then
                                crDocument.ReportDefinition.Sections(i).SectionFormat.EnableSuppress = True
                            End If
                        Next
                    Case 1

                        crDocument.ReportDefinition.Sections.Item("ghsActionsH").SectionFormat.EnableSuppress = True
                        crDocument.ReportDefinition.Sections.Item("ghsActionsF").SectionFormat.EnableSuppress = True


                End Select
                Exit Sub
            End If
            If id = 6 Then

                Dim dsTask As New DataSet
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                Dim strQuery As String = "select AM_NU9_Action_Number,AM_NU9_Task_Number,AM_NU9_Call_Number,AM_NU9_Comp_ID_FK,AM_VC8_Supp_Owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate,AM_FL8_Used_Hr,a.ci_vc36_name as Name,TM_NU9_Task_no_PK,TM_NU9_Project_ID as ProjectID ,PR_VC20_Name as ProjectName from ( (((T040031 left outer join t010011 a on a.ci_nu8_address_number=AM_VC8_Supp_Owner)left outer join t010011 b on b.ci_nu8_address_number=AM_NU9_Comp_ID_FK)left outer join t040021 on TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Task_no_PK=AM_NU9_Task_Number)left outer join  t210011 on TM_NU9_Project_ID=PR_NU9_Project_ID_Pk and TM_NU9_Comp_ID_FK=PR_NU9_Comp_ID_FK)"
                SQL.Search("dsTask", "WSSReportsD", "ExtractHours", strQuery, dsTask, "Atul", "Sharma")

                'crDocument.Subreports(0).RecordSelectionFormula = strRecordSelectionFormula
                'crDocument.Subreports(0).
                strRecordSelectionFormula = strRecordSelectionFormula.Replace("Command_1", "dsTask")

                crDocument.SetDataSource(dsTask)
                crvReport.EnableDrillDown = False
                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                cpnlReport.State = CustomControls.Web.PanelState.Expanded
                crvReport.ReportSource = crDocument
                crvReport.DataBind()
                Exit Sub

            End If

            crvReport.EnableDrillDown = False
            'If id = 5 Then
            '    crvReport.EnableDrillDown = True
            'End If
            crDocument.RecordSelectionFormula = strRecordSelectionFormula
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            clsReport.LogonInformation(crDocument)

            crvReport.ReportSource = crDocument
            crvReport.DataBind()

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

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
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

#End Region
End Class
