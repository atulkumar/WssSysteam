#Region "Reffered Assemblies"


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

Partial Class Reports_Reports
    Inherits System.Web.UI.Page
#Region "Page Level Variables "


    Dim crDocument As New ReportDocument
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

#Region "Page Load Function "


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
            crvReport.ReportSource = Nothing
            If Not IsPostBack Then
                objReports = New clsReportData
                Dim dt As New DataTable
                ' imgClose.Attributes.Add("Onclick", "return back('" & Request("ip") & "');")
                dt = objReports.extractCompanyType(HttpContext.Current.Session("PropCompanyID"))
                If dt.Rows(0).Item(0) <> "SCM" Then
                    ddlCompany.Enabled = False
                End If
                HIDSCRID.Value = Request.QueryString("ScrID")
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "','" & ddlCallFrom.ClientID & "','" & ddlCallTo.ClientID & "');")
                ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "','" & ddlCallFrom.ClientID & "','" & ddlCallTo.ClientID & "');")
            End If

            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Attach"
                        Response.Write("<script>window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');</script>")
                    Case "Logout"
                        LogoutWSS()
                    Case "OK"
                End Select
            End If

            If IsPostBack Then
                If dtFromDate.Text = Nothing And dtToDate.Text = Nothing And ddlCallTo.SelectedIndex = 0 And ddlCallFrom.SelectedIndex = 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please Select Date or Call Number before Clicking on Search Button...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                    'cpnlError.Visible = False
                End If
            End If
            If dtFromDate.Text <> Nothing Then
                If dtFromDate.Text > Date.Now Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From date cannot be greater than today Date ... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
            End If
            show()

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Function Show()"


    Private Sub show()
        Try
            Select Case Request("ip")
                Case Nothing
                    Response.Redirect("ReportsIndex.aspx")
                Case "cds"

                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 529
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block




                    'Response.Write("<head><title>" & "CALL DELIVERY STATUS" & "</title></head>")

                    cpnlReport.Text = "CALL DELIVERY STATUS"
                    cpnlRS.Text = "CALL DELIVERY STATUS"


                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlCallFrom, Request.Form("txthiddenCallnos"), "cpnlRS$" & ddlCallFrom.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlCallTo, Request.Form("txthiddenCallnos2"), "cpnlRS$" & ddlCallTo.ID, New ListItem("--ALL--", 0))
                        'If ddlCompany.SelectedIndex = 0 Then
                        '    ddlProject.Enabled = False
                        'End If
                    Else
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                        fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                        fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                    End If

                    lblHead.Text = "CALL DELIVERY STATUS"
                    If IsPostBack Then
                        Showreport(1)
                    End If

                Case "csd"

                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 547
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block


                    'Response.Write("<head><title>" & "CALL STATUS SUMMARY" & "</title></head>")
                    cpnlReport.Text = "CALL STATUS SUMMARY"
                    cpnlRS.Text = "CALL STATUS SUMMARY"
                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS$" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS$" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlCallFrom, Request.Form("txthiddenCallnos"), "cpnlRS$" & ddlCallFrom.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlCallTo, Request.Form("txthiddenCallnos2"), "cpnlRS$" & ddlCallTo.ID, New ListItem("--ALL--", 0))

                    Else
                        fill_company()
                        fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                        fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                        fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                    End If
                    lblHead.Text = "CALL STATUS SUMMARY"
                    If IsPostBack Then
                        Showreport(2)
                    End If

                Case "mds"
                    'Response.Write("<head><title>" & "MONITORING DETAILS" & "</title></head>")
                    cpnlReport.Text = "MONITORING DETAILS"
                    'Showreport(3)
                Case "cdt"
                    'Response.Write("<head><title>" & "CALL STATUS" & "</title></head>")
                    cpnlReport.Text = "CALL STATUS"
                    cpnlRS.Text = "CALL STATUS"
                    If Not IsPostBack Then
                        fill_company()
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                        fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                        fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                    End If
                    lblHead.Text = "CALL STATUS"
                    ' Showreport(4)
                Case "ctas"
                    'Response.Write("<head><title>" & "CALL TASK SUMMARY" & "</title></head>")
                    cpnlReport.Text = "CALL TASK SUMMARY"
                    cpnlRS.Text = "CALL TASK SUMMARY"
                    If Not IsPostBack Then
                        fill_company()
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                        fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                        fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                    End If
                    lblHead.Text = "CALL STATUS"
                    ' Showreport(5)
                Case Else
                    Response.Redirect("ReportsIndex.aspx")
            End Select
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region "Fill Drop down List"


    Private Sub fill_CallNo(ByVal id As Integer, ByVal CompanyID As Integer, ByVal ProjectID As Integer)
        'go to branch and fill the combo
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If id = 2 Then
                dt = objReports.extractCallNo(1, CompanyID, ProjectID)
                ddlCallFrom.DataSource = dt
                ddlCallFrom.DataTextField = "CallNo"
                ddlCallFrom.DataValueField = "CallNo"
                ddlCallFrom.DataBind()
                ddlCallFrom.Items.Insert(0, "--ALL--")
            End If
            If id = 3 Then
                dt = objReports.extractCallNo(1, CompanyID, ProjectID)
                ddlCallTo.DataSource = dt
                ddlCallTo.DataTextField = "CallNo"
                ddlCallTo.DataValueField = "CallNo"
                ddlCallTo.DataBind()
                ddlCallTo.Items.Insert(0, "--ALL--")
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString


        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCompany(1)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            ' ddlCompany.Items.Insert(0, "--ALL--")

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
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCallOwner(companyID, projectID, 1)
            If dt.Rows.Count > 0 Then
                ddlEmployee.DataSource = dt
                ddlEmployee.DataTextField = "Name"
                ddlEmployee.DataValueField = "addressNo"
                ddlEmployee.DataBind()
                If ddlEmployee.Items(0).Text <> "--ALL--" Then
                    ddlEmployee.Items.Insert(0, "--ALL--")
                End If
            Else
                ddlEmployee.Items.Insert(0, "--ALL--")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString


        Finally
            objReports = Nothing
        End Try
    End Sub
#End Region

#Region " Function Showreport"


    Private Sub Showreport(ByVal id As Integer)
        'cpnlError.Visible = False
        Try

            If id = 1 Or id = 2 Or id = 4 Then
                If id = 1 Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallDeliveryStatus.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crCallDeliveryStatus

                ElseIf id = 2 Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallStatusSummary1.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crCallStatusSummary1

                    showHideContents()

                ElseIf id = 4 Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallStatus.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crCallStatus
                End If

                ' mstrCallNumber = CType(ddlCallNo.SelectedItem.Value.Trim, String)

                Dim strRecordSelectionFormula As String

                If ddlCompany.SelectedIndex >= 0 Then ' And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{T010011_Company.CI_NU8_Address_Number}=" & strCompany
                End If

                If ddlProject.SelectedIndex <> 0 And ddlProject.Items.Count > 1 Then
                    Dim strProject As String = ddlProject.SelectedItem.Text.Trim
                    strRecordSelectionFormula += " and " & "{Command_2.Name}=" & "'" & strProject & "'"
                End If

                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += " and " & "{T010011_CallOwner.CI_NU8_Address_Number}=" & strEmployee
                    End If
                Else
                    strRecordSelectionFormula += " and " & "{T010011_CallOwner.CI_NU8_Address_Number}=" & Session("PropUserID")
                End If

                Dim intCallFrom, intCallTo As Integer
                intCallFrom = 0
                intCallTo = 0
                Dim dtToDate1 As DateTime

                If ddlCallFrom.SelectedIndex <> 0 And ddlCallFrom.Items.Count > 1 Then
                    intCallFrom = CType(ddlCallFrom.SelectedItem.Value, Integer)
                End If
                If ddlCallTo.SelectedIndex <> 0 And ddlCallTo.Items.Count > 1 Then
                    intCallTo = CType(ddlCallTo.SelectedItem.Text.Trim, Integer)
                End If
                If intCallFrom <> 0 And intCallTo <> 0 Then
                    If intCallFrom > intCallTo Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Call To should be grater than Call From")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        'cpnlError.Visible = False
                    End If
                End If
                If intCallFrom = 0 And intCallTo <> 0 Then
                    strRecordSelectionFormula += " and " & "{T040011.CM_NU9_Call_No_PK} <= " & intCallTo
                ElseIf intCallFrom <> 0 And intCallTo = 0 Then
                    strRecordSelectionFormula += " and " & "{T040011.CM_NU9_Call_No_PK} >= " & intCallFrom
                ElseIf intCallFrom <> 0 And intCallTo <> 0 Then
                    strRecordSelectionFormula += " and " & "{T040011.CM_NU9_Call_No_PK} >= " & intCallFrom & " and" & "{T040011.CM_NU9_Call_No_PK} <= " & intCallTo
                End If

                Dim dtFrom As String
                Dim dtTo As String
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text
       
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text

                If dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " and " & " {T040011.CM_DT8_Request_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " and " & " {T040011.CM_DT8_Request_Date} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " and " & " {T040011.CM_DT8_Request_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
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

                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                'crDocument.RecordSelectionFormula = "{T040011.CM_DT8_Request_Date} in " & "#03/09/2006#" & " to #03/23/2006#"

            End If

            If id = 3 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crMonitor2.rpt")
                crDocument.Load(Reportpath)
                'crDocument = New crMonitor2
            End If
            HttpContext.Current.Session("PropCallNumber") = mstrCallNumber
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            If id = 4 Then
                crvReport.SeparatePages = True
            Else
                crvReport.SeparatePages = False
            End If

            clsReport.LogonInformation(crDocument)

            crvReport.HasSearchButton = False
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region "All Other Functions"

    Private Sub showHideContents()
        Try
            objReports = New clsReportData
            Dim dt As New DataTable
            dt = objReports.extractCompanyType(HttpContext.Current.Session("PropCompanyID"))
            If dt.Rows(0).Item(0) <> "SCM" Then

                crDocument.ReportDefinition.ReportObjects.Item("Text11").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("Text12").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS1").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("UsedHRS1").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("Text4 ").Left = 10225

            End If

            objReports = Nothing

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub

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
