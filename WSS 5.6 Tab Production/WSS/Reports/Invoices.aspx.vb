'************************************************************************************************************
' Page                 : - Invoices
' Purpose              : - it  will show the call InvoiceAnnexure2 report,crInvoiceList,crRecieptList,                                  crReciept,crInvoiceDatail,crInvoiceActions,crInvoiceActionsDiscount,crInvoice
'                          report (one at a time)depends upon the value of query string passed.
' Date		    		   Author						Modification Date					Description
' 22/05/06				   Atul 					             					        Created
'
' Notes: 
' Code:
'************************************************************************************************************
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

Partial Class Reports_Invoices
    Inherits System.Web.UI.Page
#Region " Page Level Varaibles"

    Dim crDocument As New ReportDocument
    Private objReports As clsReportData
    Public mstrCallNumber As String
    Public mstrCompanyID As String
    'Protected WithEvents ddlInvoiceNo As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents lblInvoice As System.Web.UI.WebControls.Label
    'Protected WithEvents Ok As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cpnlError As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents pnlMsg As System.Web.UI.WebControls.Panel
    Dim strRecordSelectionFormula As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            HIDSCRID.Value = Request.QueryString("ScrID")
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlInvoiceNo.ClientID & "');")
            Ok.Attributes.Add("OnClick", "ShowImg();")
            'imgClose.Attributes.Add("Onclick", "return back('" & Request("ip") & "');")
            Ok.Visible = False
            'Ok.Attributes.Add("OnClick", "ShowImg();")

            Dim txthiddenImage = Request.Form("txthiddenImage")
            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If

            If txthiddenImage = "Logout" Then
                LogoutWSS()
            End If

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "OK"
                        Case "inl"
                            Response.Redirect("../Home.aspx")
                    End Select
                Catch ex As Exception
                End Try
            End If
            show()
            'dtFromDate.LeftPos = 500
            'dtFromDate.TopPos = 30
            'dtToDate.LeftPos = 550
            'dtToDate.TopPos = 30
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub


#Region "Function Show"


    Private Sub show()
        Try

            Select Case Request("ip")

                Case Nothing
                    Response.Redirect("ReportsIndex.aspx")
                Case "ina"
                    'Response.Write("<head><title>" & "ANNEXURES" & "</title></head>")
                    cpnlReport.Text = "ANNEXURES"
                    CpnlCallDetails.Visible = False
                    If Not IsPostBack Then
                        fill_company()
                        If Not Request("CompanyID") Is Nothing Then
                            fill_InvoiceNumber(1, Request("CompanyID"))
                        Else
                            fill_InvoiceNumber(1, HttpContext.Current.Session("PropCompanyID"))
                        End If
                    End If
                    ShowReport(1)
                Case "inl"

                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 589
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block

                    'Response.Write("<head><title>" & "INVOICES" & "</title></head>")
                    cpnlReport.Text = "INVOICE SUMMARY"
                    CpnlCallDetails.Text = "INVOICE SUMMARY"
                    If IsPostBack Then
                        FillAjaxDropDown(ddlInvoiceNo, Request.Form("txtHiddenStatus"), "CpnlCallDetails:" & ddlInvoiceNo.ID, New ListItem("--ALL--", 0))

                    End If
                    If Not IsPostBack Then
                        lblInvoice.Text = "Select Status"
                        fill_company()
                        fill_Status(Session("PropCompanyID"))
                    End If
                    Ok.Visible = True
                    If IsPostBack Then
                        ShowReport(2)
                    End If

                Case "inr"
                    'Response.Write("<head><title>" & "INVOICE RECEIPTS" & "</title></head>")
                    cpnlReport.Text = "INVOICE RECEIPT"
                    CpnlCallDetails.Visible = False
                    If Not IsPostBack Then
                        fill_company()
                        If Not Request("CompanyID") Is Nothing Then
                            fill_InvoiceNumber(1, Request("CompanyID"))
                        Else
                            fill_InvoiceNumber(1, HttpContext.Current.Session("PropCompanyID"))
                        End If
                    End If
                    ShowReport(3)
                Case "inrp"
                    'Response.Write("<head><title>" & "RECEIPT" & "</title></head>")
                    lblHead.Text = "RECEIPT"
                    cpnlReport.Text = "RECEIPT"
                    CpnlCallDetails.Visible = False

                    ShowReport(4)

                Case "inrd"
                    'Response.Write("<head><title>" & "INVOICE DETAILS " & "</title></head>")
                    cpnlReport.Text = "INVOICE DETAILS "
                    CpnlCallDetails.Visible = False
                    If Not IsPostBack Then
                        fill_company()
                        If Not Request("CompanyID") Is Nothing Then
                            fill_InvoiceNumber(1, Request("CompanyID"))
                        Else
                            fill_InvoiceNumber(1, HttpContext.Current.Session("PropCompanyID"))
                        End If
                    End If
                    ShowReport(5)
                Case "inrda"
                    'Response.Write("<head><title>" & "INVOICE DETAILS ( Actions ) " & "</title></head>")
                    cpnlReport.Text = "INVOICE DETAILS ( Actions )"
                    CpnlCallDetails.Visible = False
                    'If Not IsPostBack Then
                    '    fill_company()
                    '    If Not Request("CompanyID") Is Nothing Then
                    '        fill_InvoiceNumber(1, Request("CompanyID"))
                    '    Else
                    '        fill_InvoiceNumber(1, HttpContext.Current.Session("PropCompanyID"))
                    '    End If
                    'End If
                    ShowReport(6)
                Case "inrdis"
                    'Response.Write("<head><title>" & "INVOICE DETAILS ( Discount ) " & "</title></head>")
                    cpnlReport.Text = "INVOICE DETAILS ( Discount )"
                    CpnlCallDetails.Visible = False
                    If Not IsPostBack Then
                        fill_company()
                        If Not Request("CompanyID") Is Nothing Then
                            fill_InvoiceNumber(1, Request("CompanyID"))
                        Else
                            fill_InvoiceNumber(1, HttpContext.Current.Session("PropCompanyID"))
                        End If
                    End If
                    ShowReport(7)
                Case "invs"
                    'Response.Write("<head><title>" & "INVOICE " & "</title></head>")
                    cpnlReport.Text = "INVOICE"
                    CpnlCallDetails.Visible = False
                    ShowReport(8)

                Case Else
                    Response.Redirect("reportsindex.aspx")

            End Select
            If ddlInvoiceNo.SelectedIndex = 0 Then
                Exit Sub

            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Fill Drop Down List Boxes"


    Private Sub fill_InvoiceNumber(ByVal id As Integer, ByVal CompanyID As Integer)
        'go to branch and fill the combo
        'CDDLCallNo.CDDLQuery = "select CM_NU9_Call_No_PK ID, CN_VC20_Call_Status  Status, CM_VC100_Subject Subject from T040011 where CM_NU9_Comp_Id_FK=" & ddlCompany.SelectedValue
        'CDDLCallNo.CDDLUDC = True
        'CDDLCallNo.CDDLFillDropDown(15, True)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If id = 1 Then
                dt = objReports.extractInvoiceNo(2, CompanyID)
                ddlInvoiceNo.DataSource = dt
                ddlInvoiceNo.DataTextField = "InvoiceNumber"
                ddlInvoiceNo.DataValueField = "InvoiceNumber"
                ddlInvoiceNo.DataBind()
                ddlInvoiceNo.Items.Insert(0, "--Select--")
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_Status(ByVal companyID As String)

        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If ddlCompany.SelectedIndex > 0 Then
                dt = objReports.extractInvoiceStatus(companyID)
            Else : ddlInvoiceNo.Enabled = False
            End If

            ddlInvoiceNo.DataSource = dt
            ddlInvoiceNo.DataTextField = "Status"
            ddlInvoiceNo.DataValueField = "Status"
            ddlInvoiceNo.DataBind()
            ddlInvoiceNo.Items.Insert(0, "--All--")
            Dim item
            'For Each item In ddlInvoiceNo.Items()
            '    If item.ToString.ToUpper = "PENDING" Then
            '        ddlInvoiceNo.SelectedValue = "PENDING"
            '        Exit For
            '    End If
            'Next
            ' ddlInvoiceNo.SelectedValue = "PENDING"
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
            ddlCompany.Items.Insert(0, "--Select--")
            If Request("CompanyID") <> Nothing Then
                ddlCompany.SelectedValue = CType(Request("CompanyID"), Integer)
            ElseIf HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

#End Region

#Region "Function ShowReport To Show the Report "


    Private Sub ShowReport(ByVal id As Integer)
        Try

            Select Case id
                Case 1
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crInvoiceAnnexure2.rpt")
                    crDocument.Load(Reportpath)

                Case 2
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crInvoiceList.rpt")
                    crDocument.Load(Reportpath)

                Case 3
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crRecieptList.rpt")
                    crDocument.Load(Reportpath)

                Case 4
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crReciept.rpt")
                    crDocument.Load(Reportpath)

                Case 5
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crInvoiceDatail.rpt")
                    crDocument.Load(Reportpath)

                Case 6
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crInvoiceActions.rpt")
                    crDocument.Load(Reportpath)

                Case 7
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crInvoiceActionsDiscount.rpt")
                    crDocument.Load(Reportpath)

                Case 8
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crInvoice.rpt")
                    crDocument.Load(Reportpath)


            End Select

            ' crvReport.SeparatePages = True



            If id = 1 Or id = 5 Or id = 7 Then

                Dim strRequest As String = Request("InvoiceNumber")
                Dim strCompanyID As String = Request("CompanyID")

                If ddlCompany.SelectedIndex <= 0 Then
                    Exit Sub
                End If
                If ddlInvoiceNo.Items.Count <= 1 Then
                    Exit Sub
                End If
                If ddlInvoiceNo.SelectedIndex <= 0 And strRequest = Nothing Then
                    Exit Sub

                ElseIf ddlInvoiceNo.SelectedIndex > 0 Then

                    ' paramDiscrete.Value = ddlCallNo.SelectedItem.Value.Trim
                    'crDocument.SetParameterValue("InvoiceNo", ddlInvoiceNo.SelectedItem.Value.Trim)
                    ' crDocument.SetParameterValue("CompanyID", ddlCompany.SelectedItem.Value.Trim)

                    strRecordSelectionFormula = "{Command.CompanyID}=" & ddlCompany.SelectedItem.Value.Trim & " and " & "{Command.InvoiceNumber}= " & ddlInvoiceNo.SelectedItem.Text.Trim
                Else
                    strRecordSelectionFormula = "{Command.CompanyID}=" & strCompanyID & " and " & "{Command.InvoiceNumber}= " & strRequest
                End If

            ElseIf id = 2 Then


                If ddlCompany.SelectedIndex <= 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Select Company")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    cpnlError.Visible = False
                End If
                If ddlInvoiceNo.Items.Count <= 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("There is no invoice available for this company")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    cpnlError.Visible = False

                End If
                If ddlInvoiceNo.SelectedIndex <= 0 Then
                    strRecordSelectionFormula = "{Command.CompanyID}=" & ddlCompany.SelectedItem.Value.Trim

                ElseIf ddlInvoiceNo.SelectedIndex > 0 Then
                    strRecordSelectionFormula = "{Command.CompanyID}=" & ddlCompany.SelectedItem.Value.Trim & " and " & "{Command.Status}= " & Chr(34) & ddlInvoiceNo.SelectedItem.Text.Trim & Chr(34)

                    Dim strStatus As String
                End If
            ElseIf id = 3 Then


                Dim strRequest As String = Request("InvoiceNumber")
                Dim strCompanyID As String = Request("CompanyID")
                Dim strTotalAmount As String = Request("TotalAmount")
                Dim crFormula As FormulaFieldDefinition
                crFormula = CType(crDocument.DataDefinition.FormulaFields("TotalInvoiceAmount"), FormulaFieldDefinition)
                crformula.Text = "'" & strTotalAmount & "'"
                If ddlCompany.SelectedIndex <= 0 Then
                    Exit Sub
                End If
                If ddlInvoiceNo.Items.Count <= 1 Then
                    Exit Sub
                End If
                If ddlInvoiceNo.SelectedIndex <= 0 And strRequest = Nothing Then
                    Exit Sub
                ElseIf ddlInvoiceNo.SelectedIndex > 0 Then
                    strRecordSelectionFormula = "{Command.CompanyID}=" & ddlCompany.SelectedItem.Value.Trim & " and " & "{Command.InvoiceNumber}= " & ddlInvoiceNo.SelectedItem.Text.Trim
                Else
                    strRecordSelectionFormula = "{Command.CompanyID}=" & strCompanyID & " and " & "{Command.InvoiceNumber}= " & strRequest
                End If

            ElseIf id = 4 Then
                Dim crFormula As FormulaFieldDefinition
                crFormula = CType(crDocument.DataDefinition.FormulaFields("balance"), FormulaFieldDefinition)
                crFormula.Text = "'" & Request("bal") & "'"
                strRecordSelectionFormula = "{Command.PK}=" & Request("PK").ToString
            ElseIf id = 6 Then
                Dim strRequest As String = Request("InvoiceNumber")
                Dim strCompanyID As String = Request("CompanyID")
                Dim intCallNo As Integer = Request("CallNo")
                Dim intTaskNo As Integer = Request("TaskNo")
                strRecordSelectionFormula = "{Command.CompanyID}=" & strCompanyID & " and " & "{Command.InvoiceNumber}= " & strRequest & " and" & "{Command.CallNo}=" & intCallNo & " and" & "{Command.TaskNo}=" & intTaskNo
            ElseIf id = 8 Then
                Dim strRequest As String = Request("InvoiceNumber")
                Dim strCompanyID As String = Request("CompanyID")
                strRecordSelectionFormula = "{Command.CompanyID}=" & strCompanyID & " and " & "{Command.InvoiceNumber}= " & strRequest

            End If

            crDocument.RecordSelectionFormula = strRecordSelectionFormula
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            crvReport.HasSearchButton = False
            crvReport.SeparatePages = False
            crvReport.EnableDrillDown = False
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


    'Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
    '    Try
    '        If Request("ip").ToString = "ina" Then
    '            If ddlCompany.SelectedIndex = 0 Then
    '                ddlInvoiceNo.Enabled = False
    '            Else
    '                Dim intSelectedValue As Integer = CType(ddlCompany.SelectedItem.Value.Trim, Integer)
    '                fill_InvoiceNumber(1, intSelectedValue)
    '            End If
    '        ElseIf Request("ip").ToString = "inl" Then
    '            If ddlCompany.SelectedIndex = 0 Then
    '                ddlInvoiceNo.Enabled = False
    '            Else : ddlInvoiceNo.Enabled = True
    '                fill_Status(ddlCompany.SelectedValue.Trim)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Dim str As String = ex.Message.ToString
    '    End Try
    'End Sub
#End Region

#End Region

End Class
