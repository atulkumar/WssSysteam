'************************************************************************************************************
' Page                 : - Call Task action  
' Purpose              : - It will show three diffrent reports(Calltaskaction report,Emp  call detail report,                          Task wise productivity report) depends upon value of query string passed                               
' Date		    			Author						Modification Date					Description
' 03/11/06				    Jagmit 					         					            Created
'
' Notes:   
' Code:
'************************************************************************************************************

#Region " Refered Assemblies"

Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Text
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

#End Region
Partial Class Reports_CallTaskAction
    Inherits System.Web.UI.Page
    Protected WithEvents chkLCallStatus As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents chklTaskStatus As System.Web.UI.WebControls.CheckBoxList


#Region "Page Level Variables"
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

#Region "Page Load Function"


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
            imgOK.Attributes.Add("OnClick", "ShowImg();")
            HIDSCRID.Value = Request.QueryString("ScrID")
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlAssignedBy.ClientID & "','" & ddlEmployee.ClientID & "');")
            ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlAssignedBy.ClientID & "','" & ddlEmployee.ClientID & "');")

            objReports = New clsReportData
            Dim dt As New DataTable
            dt = objReports.extractCompanyType(HttpContext.Current.Session("PropCompanyID"))
            If dt.Rows(0).Item(0) <> "SCM" Then
                ddlCompany.Enabled = False
            End If

            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
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

#Region " Function Show () "


    Private Sub show()
        Try
            crvReport.ReportSource = Nothing
            Select Case Request("ip")
                Case "cta"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 763
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block

                    'Response.Write("<head><title>" & "CALL  TASK ACTION" & "</title></head>")
                    cpnlReport.Text = "CALL TASK ACTION"
                    cpnlRS.Text = "CALL TASK ACTION"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlAssignedBy, Request.Form("txthiddenOwner"), "cpnlRS$" & ddlAssignedBy.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        End If
                    Else
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    If ddlCompany.SelectedIndex = 0 Then
                        ddlProject.Enabled = False
                        ddlEmployee.Enabled = False
                        ddlAssignedBy.Enabled = False
                    Else
                        ddlProject.Enabled = True
                        ddlEmployee.Enabled = True
                        ddlAssignedBy.Enabled = True
                    End If
                    lblHead.Text = "CALL TASK ACTION"

                    If IsPostBack Then
                        Showreport(1)
                    End If
                Case "ed"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 764
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block

                    'Response.Write("<head><title>" & "CALL  TASK ACTION" & "</title></head>")

                    cpnlReport.Text = "EMPLOYEE CALL DETAILS "
                    cpnlRS.Text = " EMPLOYEE CALL DETAILS"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlAssignedBy, Request.Form("txthiddenOwner"), "cpnlRS$" & ddlAssignedBy.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        End If
                    Else
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    If ddlCompany.SelectedIndex = 0 Then
                        ddlProject.Enabled = False
                        ddlEmployee.Enabled = False
                        ddlAssignedBy.Enabled = False
                    Else
                        ddlProject.Enabled = True
                        ddlEmployee.Enabled = True
                        ddlAssignedBy.Enabled = True
                    End If
                    lblHead.Text = "EMPLOYEE CALL DETAILS"

                    ddlAssignedBy.Width = Unit.Point(0)
                    ddlAssignedBy.Height = Unit.Point(0)
                    lblAssignedBy.Visible = False
                    'ddlAssignedBy.Visible = False

                    If IsPostBack Then
                        Showreport(2)
                    End If

                Case "Tr"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 767
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block
                    'Response.Write("<head><title>" & "TASK REPORT" & "</title></head>")

                    cpnlReport.Text = "TASK REPORT"
                    cpnlRS.Text = "TASK REPORT"
                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlAssignedBy, Request.Form("txthiddenOwner"), "cpnlRS$" & ddlAssignedBy.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        End If
                    Else
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    If ddlCompany.SelectedIndex = 0 Then
                        ddlProject.Enabled = False
                        ddlEmployee.Enabled = False
                        ddlAssignedBy.Enabled = False
                    Else
                        ddlProject.Enabled = True
                        ddlEmployee.Enabled = True
                        ddlAssignedBy.Enabled = True
                    End If
                    lblHead.Text = "EMPLOYEE TASK REPORT"

                    ddlAssignedBy.Width = Unit.Point(0)
                    ddlAssignedBy.Height = Unit.Point(0)
                    lblAssignedBy.Visible = False
                    'ddlAssignedBy.Visible = False

                    If IsPostBack Then
                        Showreport(3)
                    End If
                Case Else
                    Response.Redirect("reportsindex.aspx", False)
            End Select
        Catch ex As Exception
            'Dim str As String = ex.Message.ToString
        End Try

    End Sub

#End Region

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
            ddlCompany.Items.Insert(0, New ListItem("--Select--", 0))

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
            dt = objReports.extractProject(comapnyID)
            If dt.Rows.Count > 0 Then
                ddlProject.DataSource = dt
                ddlProject.DataTextField = "Name"
                ddlProject.DataValueField = "ID"
                ddlProject.DataBind()
                If ddlProject.Items(0).Text <> "--ALL--" Then
                    ddlProject.Items.Insert(0, "--ALL--")
                End If
            Else
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
            dt = Nothing

            dt = New DataTable
            dt = objReports.extractCallOwner(companyID, projectID, 2)
            ddlAssignedBy.DataSource = dt
            ddlAssignedBy.DataTextField = "Name"
            ddlAssignedBy.DataValueField = "AddressNo"
            ddlAssignedBy.DataBind()
            ddlAssignedBy.Items.Insert(0, "--ALL--")

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_AssignedBy(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Dim dt As New DataTable
        objReports = New clsReportData
        dt = objReports.extractCallOwner(companyID, projectID, category)
        ddlAssignedBy.DataSource = dt
        ddlAssignedBy.DataTextField = "Name"
        ddlAssignedBy.DataValueField = "AddressNo"
        ddlAssignedBy.DataBind()
        ddlAssignedBy.Items.Insert(0, "--ALL--")
    End Sub

    Private Sub fill_CallTaskStatus(ByVal id As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If id = 1 Then
                dt = objReports.extractCTStatus(1)
                chkLCallStatus.DataSource = dt
                chkLCallStatus.DataTextField = "CallStatus"
                chkLCallStatus.DataValueField = "CallStatus"
                chkLCallStatus.DataBind()
                chkLCallStatus.Items.Insert(0, "ALL")

            ElseIf id = 2 Then
                dt = objReports.extractCTStatus(2)
                chklTaskStatus.DataSource = dt
                chklTaskStatus.DataTextField = "TaskStatus"
                chklTaskStatus.DataValueField = "TaskStatus"
                chklTaskStatus.DataBind()
                chklTaskStatus.Items.Insert(0, "ALL")

            End If


        Catch ex As Exception
            ' Dim str As String = ex.Message.ToString


        Finally
            objReports = Nothing
        End Try
    End Sub


#End Region

#Region "Function ShowReport() "
    Private Sub Showreport(ByVal id As Integer)
        Try
            Select Case id
                Case 1
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallTaskAction2.rpt")
                    crDocument.Load(Reportpath)

                Case 2
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallEmpDetail.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crEmpDetail1

                Case 3
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crTaskReport.rpt")
                    crDocument.Load(Reportpath)
                    clsReport.LogonInformation(crDocument)
            End Select
            Dim strCallStatus As New StringBuilder
            Dim strTaskStatus As New StringBuilder
            strCallStatus.Append("[")
            strTaskStatus.Append("[")
            Dim i, j As Integer
            Dim strCallS As String
            Dim strTaskS As String
            Dim dtToDate1 As DateTime
            Dim dtFrom As String
            Dim dtTo As String
            Dim strRecordSelectionFormula As String
            If id = 3 Then
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{Command_3.AM_NU9_Comp_Id_FK}=" & strCompany & " and"
                End If
            ElseIf id <> 2 Then
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{T040011.CM_NU9_Comp_Id_FK}=" & strCompany & " and"
                End If
            End If
            If id = 1 Then
                If ddlProject.SelectedIndex <> 0 And ddlProject.Items.Count > 1 Then
                    Dim strProject As String = ddlProject.SelectedItem.Text.Trim
                    strRecordSelectionFormula += "{Command_5.Name}=" & "'" & strProject & "'" & " and"
                End If
                If ddlAssignedBy.SelectedIndex <> 0 And ddlAssignedBy.Items.Count > 1 Then
                    Dim strAssignedBy As String = ddlAssignedBy.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{T010011_SOWNER.CI_NU8_Address_Number}=" & strAssignedBy & " and"
                End If
                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += "{T010011_AssignTo.CI_NU8_Address_Number}=" & strEmployee & "                and"
                    End If
                Else
                    strRecordSelectionFormula += "{T010011_AssignTo.CI_NU8_Address_Number}=" & Session("PropUserID") & "                and"
                End If
            End If
            If id = 2 Then
                fillReportEd()
            End If
            If id = 3 Then
                If ddlProject.SelectedIndex <> 0 And ddlProject.Items.Count > 1 Then
                    Dim strProjectID As Integer = ddlProject.SelectedValue.Trim
                    strRecordSelectionFormula += "{Command_3.cm_nu9_project_id}=" & "" & strProjectID & "" & " and"
                End If

                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += "{command_3.AM_VC8_Supp_Owner}=" & strEmployee & "                and"
                    End If
                Else
                    strRecordSelectionFormula += "{command_3.AM_VC8_Supp_Owner}=" & Session("propUserID") & "                and"
                End If
            End If

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

            '*** Check for Task report
            If id = 3 Then
                If dtTo = Nothing And dtFrom = Nothing Then
                    strRecordSelectionFormula += "{Command_3.AM_DT8_Action_Date} in " & "#" & Today & "#" & "to #" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    'dtFrom = Today
                    'strRecordSelectionFormula += " {Command_3.TM_DT8_Task_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                    strRecordSelectionFormula += " {Command_3.AM_DT8_Action_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    'dtTo = Today
                    'strRecordSelectionFormula += " {Command_3.TM_DT8_Action_Date} >= " & "#" & dtFrom & "#"
                    strRecordSelectionFormula += " {Command_3.AM_DT8_Action_Date} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    'strRecordSelectionFormula += " {Command_3.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    strRecordSelectionFormula += " {Command_3.AM_DT8_Action_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If
            Else
                '*** Check for Call Task Action  report ****
                If id = 1 Then
                    If dtTo = Nothing And dtFrom = Nothing Then
                        strRecordSelectionFormula += " {T040031.AM_DT8_Action_Date} in " & "#" & Today & "#" & "to #" & DateAdd("d", 1, Today) & "#"
                    ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                        strRecordSelectionFormula += " {T040031.AM_DT8_Action_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                    ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                        strRecordSelectionFormula += " {T040031.AM_DT8_Action_Date} >= " & "#" & dtFrom & "#"
                    ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                        strRecordSelectionFormula += " {T040031.AM_DT8_Action_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    End If
                End If
            End If
            '*** Check for  Emp Call detail  report ****
            Dim objReports As New clsReportData
            Dim intCompany As Integer = 0
            If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                intCompany = ddlCompany.SelectedItem.Value.Trim
            End If

            If id = 3 Then
                crDocument.DataDefinition.FormulaFields("per").Text = "(Sum ({Command_3.UsedHrs},{Command_3.                TM_VC8_Task_Type})*100 )/Sum ({Command_3.UsedHrs}, {Command_3.AM_VC8_Supp_Owner})"
            End If
            objReports = Nothing
            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False
            crvReport.EnableDrillDown = False
            'cpnlError.Visible = False
            'clsReport.LogonInformation(crDocument)
            crDocument.RecordSelectionFormula = strRecordSelectionFormula
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Public Function extractTaskReport(ByVal dtFrom As String, ByVal dtTo As String) As DataTable
        Dim dsType As New DataSet
        Dim CompanyName, SubCategory, sowner
        If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
            CompanyName = ddlCompany.SelectedItem.Text.Trim
        End If
        If ddlProject.SelectedIndex <> 0 And ddlProject.Items.Count > 1 Then
            SubCategory = ddlProject.SelectedItem.Text.Trim
        End If
        If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
            sowner = ddlEmployee.SelectedValue
        End If

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        '        SQL.DBTable = "T010011"
        SQL.DBTracing = False

        '**task Report****************

        SQL.Search("t010011", "WssreportsId", "ExtractTaskStatus", "select sum(AM_FL8_Used_Hr)UsedHrs from t040031,t040021 where AM_NU9_Task_Number = TM_NU9_Task_No_PK  and AM_NU9_Call_Number=TM_NU9_Call_No_FK and AM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and AM_NU9_Comp_ID_FK=(select CI_Nu8_Address_number from t010011 where ci_vc36_name='" & CompanyName & "' and ci_vc8_address_book_type='com') and AM_VC8_Supp_Owner=(select CI_Nu8_Address_number from t010011 where CI_Nu8_Address_number=" & sowner & " and ci_vc8_address_book_type='em') and tm_nu9_project_id=(select pr_nu9_project_id_pk from t210011 where pr_vc20_name='" & SubCategory & "') and convert(datetime,convert(varchar,t040021.TM_DT8_Task_Date,101),101) >= ('" & CDate(dtFrom) & "') and convert(datetime,convert(varchar,t040021.TM_DT8_Task_Date,101),101) <=('" & CDate(dtTo) & "')", dsType, "jagmit", "sidhu")

        '***Task Report End****
        Return dsType.Tables(0)
    End Function
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

#End Region
    Sub fillReportEd()
        Dim s As String
        Dim dtFrom As String = dtFromDate.Text
        Dim dtTo As String = dtToDate.Text
        Dim ds As New DataSet
        Dim inti As Integer
        inti = 0
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
        Dim htCols As New Hashtable
        htCols.Add("CM_DT8_Request_Date", 1)
        If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlProject.SelectedIndex < 1 And ddlEmployee.SelectedIndex < 1 Then

            If Session("PropAdmin") = "1" Then
                s = "Select CM_NU9_Call_No_PK,CM_NU9_Comp_Id_FK,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CN_VC20_Call_Status,convert(varchar,CM_DT8_Request_Date) CM_DT8_Request_Date,CM_NU9_Call_Owner, PR_NU9_Project_ID_Pk,PR_VC20_Name,PR_NU9_Comp_ID_FK from t040011 left outer join t210011 on  cm_nu9_project_id=PR_NU9_Project_ID_Pk and cm_nu9_comp_id_fk=PR_NU9_Comp_ID_FK where cm_nu9_comp_id_fk =" + ddlCompany.SelectedValue + ";select * from t010011 where  ci_nu8_address_number=" + ddlCompany.SelectedValue + ";select * from t010011;select * from t010011;SELECT  am_nu9_comp_id_fk ,am_nu9_call_number,am_vc8_supp_owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate , CM_NU9_Project_ID as ProjectID,sum(am_fl8_used_hr) as TotalHrs FROM ( T040031 left outer join t040021 on (am_nu9_comp_id_fk=TM_NU9_Comp_ID_FK) and (am_nu9_call_number=TM_NU9_Call_No_FK) and (AM_NU9_Task_Number=TM_NU9_Task_no_PK )),T040011 where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and am_nu9_comp_id_fk=" + ddlCompany.SelectedValue + " and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)>='" + dtFrom + "'  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)<= '" + dtTo + "' group by am_nu9_comp_id_fk,am_nu9_call_number,am_vc8_supp_owner,CM_NU9_Project_ID,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)"
            Else

                s = "Select CM_NU9_Call_No_PK,CM_NU9_Comp_Id_FK,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CN_VC20_Call_Status,convert(varchar,CM_DT8_Request_Date) CM_DT8_Request_Date,CM_NU9_Call_Owner, PR_NU9_Project_ID_Pk,PR_VC20_Name,PR_NU9_Comp_ID_FK from t040011 left outer join t210011 on  cm_nu9_project_id=PR_NU9_Project_ID_Pk and cm_nu9_comp_id_fk=PR_NU9_Comp_ID_FK where cm_nu9_comp_id_fk =" + ddlCompany.SelectedValue + ";select * from t010011 where  ci_nu8_address_number=" + ddlCompany.SelectedValue + ";select * from t010011;select * from t010011;SELECT  am_nu9_comp_id_fk ,am_nu9_call_number,am_vc8_supp_owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate , CM_NU9_Project_ID as ProjectID,sum(am_fl8_used_hr) as TotalHrs FROM ( T040031 left outer join t040021 on (am_nu9_comp_id_fk=TM_NU9_Comp_ID_FK) and (am_nu9_call_number=TM_NU9_Call_No_FK) and (AM_NU9_Task_Number=TM_NU9_Task_no_PK )),T040011 where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and am_nu9_comp_id_fk=" + ddlCompany.SelectedValue + " and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)>='" + dtFrom + "'  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)<= '" + dtTo + "'  and am_vc8_supp_owner = '" & Session("PropUserID") & "'  group by am_nu9_comp_id_fk,am_nu9_call_number,am_vc8_supp_owner,CM_NU9_Project_ID,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)"
            End If
        Else
            If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlProject.SelectedIndex <> 0 And ddlEmployee.SelectedIndex < 1 Then

                If Session("PropAdmin") = "1" Then
                    s = "Select CM_NU9_Call_No_PK,CM_NU9_Comp_Id_FK,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CN_VC20_Call_Status,convert(varchar,CM_DT8_Request_Date) CM_DT8_Request_Date,CM_NU9_Call_Owner, PR_NU9_Project_ID_Pk,PR_VC20_Name,PR_NU9_Comp_ID_FK from t040011 left outer join t210011 on  cm_nu9_project_id=PR_NU9_Project_ID_Pk and cm_nu9_comp_id_fk=PR_NU9_Comp_ID_FK where cm_nu9_comp_id_fk =" + ddlCompany.SelectedValue + " and cm_nu9_project_id = " + ddlProject.SelectedValue + ";select * from t010011 where  ci_nu8_address_number=" + ddlCompany.SelectedValue + ";select * from t010011;select * from t010011;SELECT  am_nu9_comp_id_fk ,am_nu9_call_number,am_vc8_supp_owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate , CM_NU9_Project_ID as ProjectID,sum(am_fl8_used_hr) as TotalHrs FROM ( T040031 left outer join t040021 on (am_nu9_comp_id_fk=TM_NU9_Comp_ID_FK) and (am_nu9_call_number=TM_NU9_Call_No_FK) and (AM_NU9_Task_Number=TM_NU9_Task_no_PK )),t040011 where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and  am_nu9_comp_id_fk=" + ddlCompany.SelectedValue + " and TM_NU9_Project_ID=" + ddlProject.SelectedValue + " and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)>='" + dtFrom + "'  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)<= '" + dtTo + "' group by am_nu9_comp_id_fk,am_nu9_call_number,am_vc8_supp_owner,CM_NU9_Project_ID,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)"
                Else

                    s = "Select CM_NU9_Call_No_PK,CM_NU9_Comp_Id_FK,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CN_VC20_Call_Status,convert(varchar,CM_DT8_Request_Date) CM_DT8_Request_Date,CM_NU9_Call_Owner, PR_NU9_Project_ID_Pk,PR_VC20_Name,PR_NU9_Comp_ID_FK from t040011 left outer join t210011 on  cm_nu9_project_id=PR_NU9_Project_ID_Pk and cm_nu9_comp_id_fk=PR_NU9_Comp_ID_FK where cm_nu9_comp_id_fk =" + ddlCompany.SelectedValue + " and cm_nu9_project_id = " + ddlProject.SelectedValue + ";select * from t010011 where  ci_nu8_address_number=" + ddlCompany.SelectedValue + ";select * from t010011;select * from t010011;SELECT  am_nu9_comp_id_fk ,am_nu9_call_number,am_vc8_supp_owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate , CM_NU9_Project_ID as ProjectID,sum(am_fl8_used_hr) as TotalHrs FROM ( T040031 left outer join t040021 on (am_nu9_comp_id_fk=TM_NU9_Comp_ID_FK) and (am_nu9_call_number=TM_NU9_Call_No_FK) and (AM_NU9_Task_Number=TM_NU9_Task_no_PK )),t040011 where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and  am_nu9_comp_id_fk=" + ddlCompany.SelectedValue + " and TM_NU9_Project_ID=" + ddlProject.SelectedValue + " and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)>='" + dtFrom + "'  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)<= '" + dtTo + "'  and  am_vc8_supp_owner = '" & Session("PropUserID") & "' group by am_nu9_comp_id_fk,am_nu9_call_number,am_vc8_supp_owner,CM_NU9_Project_ID,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)"
                End If
            Else
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlProject.SelectedIndex <> 0 And ddlEmployee.SelectedIndex <> 0 Then

                    s = "Select CM_NU9_Call_No_PK,CM_NU9_Comp_Id_FK,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CN_VC20_Call_Status,convert(varchar,CM_DT8_Request_Date) CM_DT8_Request_Date,CM_NU9_Call_Owner, PR_NU9_Project_ID_Pk,PR_VC20_Name,PR_NU9_Comp_ID_FK from t040011 left outer join t210011 on  cm_nu9_project_id=PR_NU9_Project_ID_Pk and cm_nu9_comp_id_fk=PR_NU9_Comp_ID_FK where cm_nu9_comp_id_fk =" + ddlCompany.SelectedValue + " and cm_nu9_project_id = " + ddlProject.SelectedValue + ";select * from t010011 where  ci_nu8_address_number=" + ddlCompany.SelectedValue + ";select * from t010011;select * from t010011;SELECT  am_nu9_comp_id_fk ,am_nu9_call_number,am_vc8_supp_owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate , CM_NU9_Project_ID as ProjectID,sum(am_fl8_used_hr) as TotalHrs FROM ( T040031 left outer join t040021 on (am_nu9_comp_id_fk=TM_NU9_Comp_ID_FK) and (am_nu9_call_number=TM_NU9_Call_No_FK) and (AM_NU9_Task_Number=TM_NU9_Task_no_PK )) ,T040011 where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and am_nu9_comp_id_fk=" + ddlCompany.SelectedValue + " and am_vc8_supp_owner=" + ddlEmployee.SelectedValue + " and CM_NU9_Project_ID=" + ddlProject.SelectedValue + "  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)>='" + dtFrom + "'  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)<= '" + dtTo + "' group by am_nu9_comp_id_fk,am_nu9_call_number,am_vc8_supp_owner,CM_NU9_Project_ID,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)"

                Else
                    If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlProject.SelectedIndex < 1 And ddlEmployee.SelectedIndex <> 0 Then

                        s = "Select CM_NU9_Call_No_PK,CM_NU9_Comp_Id_FK,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CN_VC20_Call_Status,convert(varchar,CM_DT8_Request_Date) CM_DT8_Request_Date,CM_NU9_Call_Owner, PR_NU9_Project_ID_Pk,PR_VC20_Name,PR_NU9_Comp_ID_FK from t040011 left outer join t210011 on  cm_nu9_project_id=PR_NU9_Project_ID_Pk and cm_nu9_comp_id_fk=PR_NU9_Comp_ID_FK where cm_nu9_comp_id_fk =" + ddlCompany.SelectedValue + ";select * from t010011 where  ci_nu8_address_number=" + ddlCompany.SelectedValue + ";select * from t010011;select * from t010011;SELECT  am_nu9_comp_id_fk ,am_nu9_call_number,am_vc8_supp_owner,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0) as ActionDate , CM_NU9_Project_ID as ProjectID,sum(am_fl8_used_hr) as TotalHrs FROM ( T040031 left outer join t040021 on (am_nu9_comp_id_fk=TM_NU9_Comp_ID_FK) and (am_nu9_call_number=TM_NU9_Call_No_FK) and (AM_NU9_Task_Number=TM_NU9_Task_no_PK )),T040011 where Tm_nu9_call_no_FK=CM_nu9_call_no_pk and TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and  am_nu9_comp_id_fk=" + ddlCompany.SelectedValue + " and am_vc8_supp_owner=" + ddlEmployee.SelectedValue + "  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)>='" + dtFrom + "'  and convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)<= '" + dtTo + "'  group by am_nu9_comp_id_fk,am_nu9_call_number,am_vc8_supp_owner,CM_NU9_Project_ID,convert(datetime,convert(varchar(15),AM_DT8_Action_Date,101),0)"

                    End If
                End If

            End If
        End If

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim ds1 As New Dataset1
        SQL.DBTracing = False
        SQL.Search("Table", "WSSReportsD", "ExtractCallNo", s, ds1, "jagmit", "sidhu")
        SetDataTableDateFormat(ds1.Tables(2), htCols)
        Dim strRecordSelectionFormula As String = Nothing
        crDocument.SetDataSource(ds1)

    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
