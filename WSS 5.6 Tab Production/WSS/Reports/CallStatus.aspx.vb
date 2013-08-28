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

Partial Class Reports_CallStatus
    Inherits System.Web.UI.Page

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
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            crvReport.ToolbarStyle.Width = New Unit("1000px")

            HIDSCRID.Value = Request.QueryString("ScrID")
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlOwner.ClientID & "','" & ddlAssignedBy.ClientID & "','" & ddlEmployee.ClientID & "');")
            ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlOwner.ClientID & "','" & ddlAssignedBy.ClientID & "','" & ddlEmployee.ClientID & "');")
            imgOK.Attributes.Add("OnClick", "ShowImg();")

            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            Else
                ddlCompany.Enabled = True
            End If

            Dim txthiddenImage = Request.Form("txthiddenImage")

            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Attach"
                        Response.Write("<script>window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');</script>")
                    Case "OK"

                    Case "Logout"
                        LogoutWSS()
                End Select

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

            Select Case Request("ip")
                Case "cst"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 501
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block

                    'Response.Write("<head><title>" & "CALL  STATUS" & "</title></head>")

                    cpnlReport.Text = "CALL  STATUS"
                    cpnlRS.Text = "CALL  STATUS"


                    If IsPostBack Then
                        FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS:" & ddlProject.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlOwner, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlOwner.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlAssignedBy, Request.Form("txthiddenAssignBy"), "cpnlRS:" & ddlAssignedBy.ID, New ListItem("--ALL--", 0))
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                        If ddlCompany.SelectedIndex = 0 Then
                            ddlProject.Enabled = False
                        End If
                    Else
                        fill_company()
                        fill_CallTaskStatus(1)
                        fill_CallTaskStatus(2)
                        If fill_Project(HttpContext.Current.Session("PropCompanyID")) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("No Call Data Found for Selected Company ... ")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If
                    lblHead.Text = "CALL  STATUS"
                    If IsPostBack Then
                        If dtFromDate.Text = Nothing And dtToDate.Text = Nothing Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Please Select Date...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        Else
                            'cpnlError.Visible = False
                        End If
                    End If
                    If IsPostBack Then
                        Showreport(1)
                    End If
                Case Else
                    Response.Redirect("reportsindex.aspx", False)
            End Select
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
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
            ddlCompany.Items.Insert(0, New ListItem("--ALL--", 0))

            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Function fill_Project(ByVal comapnyID As Integer) As Boolean
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractProject(comapnyID)
            ddlProject.DataSource = dt
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
            Return True
        Catch ex As Exception

            Dim str As String = ex.Message.ToString
            Return False
        Finally
            objReports = Nothing
        End Try
    End Function

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Try
            Dim dt As DataTable
            objReports = New clsReportData
            If Session("PropCompanyType") = "SCM" Then
                If Session("PropAdmin") <> "1" Then
                    ddlEmployee.Items.Clear()
                    Dim strUser As String = Session("PropUserName")
                    strUser = strUser.ToUpper
                    ddlEmployee.Items.Insert(0, New ListItem(strUser, Session("PropUserID")))
                Else
                    ddlEmployee.Items.Clear()
                    dt = New DataTable
                    dt = objReports.extractTaskOwner(companyID, projectID, 2)
                    ddlEmployee.DataSource = dt
                    ddlEmployee.DataTextField = "Name"
                    ddlEmployee.DataValueField = "AddressNo"
                    ddlEmployee.DataBind()
                    ddlEmployee.Items.Insert(0, New ListItem("--ALL--", 0))
                    dt = Nothing
                End If
            Else
                ddlEmployee.Items.Clear()
                dt = New DataTable
                dt = objReports.extractTaskOwner(companyID, projectID, 2)
                ddlEmployee.DataSource = dt
                ddlEmployee.DataTextField = "Name"
                ddlEmployee.DataValueField = "AddressNo"
                ddlEmployee.DataBind()
                ddlEmployee.Items.Insert(0, New ListItem("--ALL--", 0))
                dt = Nothing
            End If

            dt = New DataTable
            dt = objReports.extractTaskOwner(companyID, projectID, 1)
            ddlAssignedBy.DataSource = dt
            ddlAssignedBy.DataTextField = "Name"
            ddlAssignedBy.DataValueField = "AddressNo"
            ddlAssignedBy.DataBind()
            ddlAssignedBy.Items.Insert(0, New ListItem("--ALL--", 0))
            dt = Nothing

            dt = New DataTable
            dt = objReports.extractCallOwner(companyID, projectID, 2)
            ddlOwner.DataSource = dt
            ddlOwner.DataTextField = "Name"
            ddlOwner.DataValueField = "AddressNo"
            ddlOwner.DataBind()
            ddlOwner.Items.Insert(0, New ListItem("--ALL--", 0))

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
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
                'chkLCallStatus.Items.Insert(0, "ALL")

            ElseIf id = 2 Then
                dt = objReports.extractCTStatus(2)
                chklTaskStatus.DataSource = dt
                chklTaskStatus.DataTextField = "TaskStatus"
                chklTaskStatus.DataValueField = "TaskStatus"
                chklTaskStatus.DataBind()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

#End Region

#Region "Function ShowReport() "
    Private Sub Showreport(ByVal id As Integer)
        Try
            crvReport.ReportSource = Nothing
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crCallStatus.rpt")
            crDocument.Load(Reportpath)

            Dim strCallStatus As New StringBuilder
            Dim strTaskStatus As New StringBuilder
            strCallStatus.Append("[")
            strTaskStatus.Append("[")
            Dim i, j As Integer
            Dim strCallS As String
            Dim strTaskS As String
            Dim strRecordSelectionFormula As String
            Dim dtToDate1 As DateTime
            Dim dtFrom As String
            Dim dtTo As String
            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text

            If dtFrom = Nothing And dtTo <> Nothing Then
                Dim From As String = "01/01/1932"
                dtFrom = CDate(From)
            End If
            If dtFrom <> Nothing And dtTo = Nothing Then
                If dtFromDate.Text > Date.Now Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From date cannot be greater than today Date ... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    'cpnlError.Visible = False
                End If
                If strRecordSelectionFormula <> Nothing Then
                    strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} >= " & "#" & dtFrom & "#"
                Else
                    strRecordSelectionFormula += "{T040011.CM_DT8_Request_Date} >= " & "#" & dtFrom & "#"
                End If

            ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
            ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
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


            If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                strRecordSelectionFormula += " and" & "{T040011.CM_NU9_Comp_Id_FK}=" & strCompany
            End If
            If ddlProject.SelectedIndex <> 0 And ddlProject.Items.Count > 1 Then
                Dim strProject As String = ddlProject.SelectedItem.Text.Trim
                strRecordSelectionFormula += " and" & "{Command_5.Name}=" & "'" & strProject & "'"
            End If
            If ddlOwner.SelectedIndex <> 0 And ddlOwner.Items.Count > 1 Then
                Dim strOwner As String = ddlOwner.SelectedItem.Value.Trim
                strRecordSelectionFormula += " and" & "{T010011_CallOwner.CI_NU8_Address_Number}=" & strOwner
            End If

            If ddlAssignedBy.SelectedIndex <> 0 And ddlAssignedBy.Items.Count > 1 Then
                Dim strAssignedBy As String = ddlAssignedBy.SelectedItem.Value.Trim
                strRecordSelectionFormula += " and" & "{T010011_SOWNER.CI_NU8_Address_Number}=" & strAssignedBy
            End If

            If Session("PropAdmin") = 1 Then
                If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                    Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                    strRecordSelectionFormula += " and" & "{T010011_AssignedTo.CI_NU8_Address_Number}=" & strEmployee
                End If

            ElseIf Session("PropCompanyType") = "SCM" Then

                strRecordSelectionFormula += " and" & "{T010011_AssignedTo.CI_NU8_Address_Number}=" & Session("PropUserID")

            End If

            If chkLCallStatus.SelectedIndex > -1 Then
                j = 0
                For i = 0 To chkLCallStatus.Items.Count - 1
                    If chkLCallStatus.Items(i).Selected = True Then
                        strCallStatus.Append("'")
                        strCallStatus.Append(chkLCallStatus.Items(i))
                        strCallStatus.Append("'")
                        strCallStatus.Append(",")
                    End If
                Next
                strCallStatus.Remove(strCallStatus.Length - 1, 1)
                strCallStatus.Append("]")
                strCallS = strCallStatus.ToString
                strRecordSelectionFormula += " and" & "{T040011.CN_VC20_Call_Status} in " & strCallS
                strCallStatus = Nothing
            End If

            If chklTaskStatus.SelectedIndex > -1 Then
                j = 0
                For i = 0 To chklTaskStatus.Items.Count - 1
                    If chklTaskStatus.Items(i).Selected = True Then
                        strTaskStatus.Append("'")
                        strTaskStatus.Append(chklTaskStatus.Items(i))
                        strTaskStatus.Append("'")
                        strTaskStatus.Append(",")
                    End If
                Next
                strTaskStatus.Remove(strTaskStatus.Length - 1, 1)
                strTaskStatus.Append("]")
                strTaskS = strTaskStatus.ToString
                strRecordSelectionFormula += " and" & "{T040021.TM_VC50_Deve_status} in " & strTaskS
                strTaskStatus = Nothing
            End If

            Dim objReports As New clsReportData
            Dim intCompany As Integer = 0
            If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                intCompany = ddlCompany.SelectedItem.Value.Trim
            End If

            crDocument.DataDefinition.FormulaFields("f1").Text = objReports.extractCTStatusReport(1, intCompany, "OPEN", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f2").Text = objReports.extractCTStatusReport(1, intCompany, "ASSIGNED", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f3").Text = objReports.extractCTStatusReport(1, intCompany, "PROGRESS", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f4").Text = objReports.extractCTStatusReport(1, intCompany, "PCA", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f7").Text = objReports.extractCTStatusReport(1, intCompany, "N", Today, Today).Rows(0).Item(0)

            crDocument.DataDefinition.FormulaFields("f5").Text = objReports.extractCTStatusReport(2, intCompany, "OPEN", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f6").Text = objReports.extractCTStatusReport(2, intCompany, "PROGRESS", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f9").Text = objReports.extractCTStatusReport(2, intCompany, "ASSIGNED", dtFrom, dtTo).Rows(0).Item(0)
            crDocument.DataDefinition.FormulaFields("f8").Text = objReports.extractCTStatusReport(2, intCompany, "N", Today, Today).Rows(0).Item(0)


            objReports = Nothing
            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False
            crDocument.RecordSelectionFormula = strRecordSelectionFormula

            clsReport.LogonInformation(crDocument)
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

#End Region

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
