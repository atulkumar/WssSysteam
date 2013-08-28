'************************************************************************************************************
' Page                 : - Call Details
' Purpose              : - it  will show the call detail Full report,Call summary2,CalltaskDetail,Calldetail
'                          report (one at a time)depends upon the value of query string passed.
' Date		    		   Author						Modification Date					Description
' 05/09/06				   Atul 					             					        Created
'
' Notes: 
' Code:
'************************************************************************************************************

#Region "Reffered Assemblies"
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
#End Region

Partial Class Reports_CallDetails
    Inherits System.Web.UI.Page
    Protected WithEvents ddlCallNo As System.Web.UI.WebControls.DropDownList

#Region " Page Level Varaibles"

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
    Public mstrTaskNumber As String
    Public mstrCompanyID As String
    Public mstrCompanyIDN As String
    Private strRecordSelectionFormula As String
    Private Shared mshCall As Short
    Private Shared intCount As Integer
#End Region

#Region " Page Load Event"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        crvReport.ToolbarStyle.Width = New Unit("900px")

        If Not IsNothing(Request.QueryString("CallNo")) Then
            If Val(Request.QueryString("CallNo")) > 0 Then
            End If
        End If

        Try
            'Put user code to initialize the page here
            HIDSCRID.Value = Request.QueryString("ScrID")
            If Not IsPostBack Then
                txtCallNumber.Attributes.Add("onkeypress", "NumericOnly();")
            End If
            intCount = 2

            'imgClose.Attributes.Add("Onclick", "return back('" & intCount & "');")
            ' imgClose.Attributes.Add("Onclick", "return back(2);")
            imgOK.Attributes.Add("OnClick", "ShowImg();")
            Dim txthiddenImage = Request.Form("txthiddenImage")

            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If
            Dim strCompanyType = Session("PropCompanyType")

            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Attach"
                        Case "CloseCall"
                            If mshCall = 0 Then
                                mshCall = 1
                            Else
                                mshCall = 0
                            End If
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

#Region "Function Show"

    Private Sub show()
        Try
            crvReport.ReportSource = Nothing
            If Request("ip") = Nothing Then

                Response.Redirect("ReportsIndex.aspx")
            ElseIf Request("ip").ToString = "cd" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 500 'HIDSCRID.Value 
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                imgAttach.Attributes.Add("Onclick", "return Attachment('AttachCall');")
                Dim strRequestCall As String = Request("CallNo")
                Dim strCompanyID As String = Request("CompanyID")
                If Not Request("CompanyID") Is Nothing And Not Request("CallNo") Is Nothing Then

                    lblHead.Text = "CALL DETAIL"

                    cpnlReport.Text = "CALL DETAIL"
                    cpnlRS.Text = "CALL DETAIL"

                    imgCloseCall.Visible = False
                    cpnlRS.Visible = False
                    ShowReport(1)
                    Exit Sub
                End If
                If IsPostBack Then

                Else
                    fill_company()
                End If

                lblHead.Text = "CALL DETAIL"
                cpnlReport.Text = "CALL DETAIL"
                cpnlRS.Text = "CALL DETAIL"

                imgCloseCall.Visible = False
                ddlProject.Visible = False
                lblProject.Visible = False
                If Not Request("CompanyID") Is Nothing And Not Request("CallNo") Is Nothing Then
                    ShowReport(1)
                    Exit Sub
                End If
                If IsPostBack Then
                    ShowReport(1)
                    Exit Sub
                End If
            ElseIf Request("ip").ToString = "cdf" Then

                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 775
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                imgAttach.Attributes.Add("Onclick", "return Attachment('AttachCall');")

                If IsPostBack Then
                Else
                    fill_company()
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                End If
                If Not IsPostBack Then
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                End If
                lblHead.Text = "CALL DETAIL (FULL)"

                cpnlReport.Text = "CALL DETAIL (FULL)"
                cpnlRS.Text = "CALL DETAIL (FULL)"

                imgCloseCall.Visible = False
                If Not Request("CompanyID") Is Nothing And Not Request("CallNo") Is Nothing Then
                    ShowReport(1)
                    Exit Sub
                End If
                If IsPostBack Then
                    ShowReport(1)
                    Exit Sub
                End If
            ElseIf Request("ip").ToString = "ctd" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 776
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                imgAttach.Attributes.Add("Onclick", "return Attachment('AttachCall');")
                trDate.Visible = True
                imgAttach.Visible = False
                If IsPostBack Then

                Else
                    fill_company()
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                End If
                If Not IsPostBack Then
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                End If
                lblHead.Text = "CALL/TASK DETAIL"

                cpnlReport.Text = "CALL/TASK DETAIL"
                cpnlRS.Text = "CALL/TASK DETAIL"

                imgCloseCall.Visible = False
                If IsPostBack Then
                    ShowReport(3)
                    Exit Sub
                End If
            ElseIf Request("ip").ToString = "ctad" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 777
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                imgAttach.Attributes.Add("Onclick", "return Attachment('AttachTask');")

                'Response.Write("<head><title>" & "CALL/TASK/ACTION DETAIL" & "</title></head>")
                cpnlRS.State = CustomControls.Web.PanelState.Collapsed
                cpnlRS.Visible = False
                cpnlReport.Text = "CALL/TASK/ACTION DETAIL"

                lblHead.Text = "CALL/TASK/ACTION DETAIL"
                imgCloseCall.Visible = False
                ShowReport(4)

            ElseIf Request("ip").ToString = "cs" Or Request("ip").ToString = "cs2" Then
                'Response.Write("<head><title>" & "CALL SUMMARY" & "</title></head>")

                If IsPostBack Then
                Else
                    fill_company()
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                End If
                If Not IsPostBack Then

                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                End If
                lblHead.Text = "CALL SUMMARY"

                cpnlReport.Text = "CALL SUMMARY"
                cpnlRS.Text = "CALL SUMMARY"
                If IsPostBack Then
                    ShowReport(2)
                End If
            Else
                Response.Redirect("reportsindex.aspx")
            End If

            If ddlCompany.Items.Count <= 0 Then
                ddlCompany.Enabled = False
            End If

            If ddlProject.Items.Count <= 0 Then
                ddlProject.Enabled = False
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Fill Drop Down List Boxes"
    Private Sub fill_CallNo(ByVal id As Integer, ByVal CompanyID As Integer, ByVal ProjectID As Integer)

        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If id = 1 Then
                dt = objReports.extractCallNo(2, CompanyID, ProjectID)
                ddlCallNo.DataSource = dt
                ddlCallNo.DataTextField = "CallNo"
                ddlCallNo.DataValueField = "CallNo"
                ddlCallNo.DataBind()
                ddlCallNo.Items.Insert(0, "--Select--")
                If Not Request("CallNo") Is Nothing Then
                    ddlCallNo.SelectedValue = CInt(Request("CallNo"))
                End If
            End If

            If id = 2 Then
                dt = objReports.extractCallNo(2, CompanyID, ProjectID)
                ddlCallFrom.DataSource = dt
                ddlCallFrom.DataTextField = "CallNo"
                ddlCallFrom.DataValueField = "CallNo"
                ddlCallFrom.DataBind()
                ddlCallFrom.Items.Insert(0, "--Select--")
            End If
            If id = 3 Then
                ddlCallTo.Items.Clear()
                dt = objReports.extractCallNo(2, CompanyID, ProjectID)
                ddlCallTo.DataSource = dt
                ddlCallTo.DataTextField = "CallNo"
                ddlCallTo.DataValueField = "CallNo"
                ddlCallTo.DataBind()
                ddlCallTo.Items.Insert(0, "--Select--")
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
            ddlCompany.Items.Insert(0, New ListItem("--Select--", 0))
            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If

            Dim strRequestCall As String = Request("CallNo")
            Dim strCompanyID As String = Request("CompanyID")
            If strRequestCall <> Nothing And strCompanyID <> Nothing Then
                ddlCompany.SelectedValue = CType(strCompanyID, Integer)
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
            'If ddlCompany.SelectedIndex = 0 Then
            '    dt = objReports.extractProject(0)
            'Else
            dt = objReports.extractProject(comapnyID)
            'End If
            ddlProject.DataSource = dt
            ddlProject.DataTextField = "Name"
            ddlProject.DataValueField = "ID"
            ddlProject.DataBind()
            If ddlProject.Items(0).Text <> "--ALL--" Then
                ddlProject.Items.Insert(0, "--ALL--")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Dim dt As New DataTable
        Try
            objReports = New clsReportData
            dt = objReports.extractCallOwner(companyID, projectID, category)
            ddlEmployee.DataSource = dt
            ddlEmployee.DataTextField = "Name"
            ddlEmployee.DataValueField = "AddressNo"
            ddlEmployee.DataBind()
            If ddlEmployee.Items(0).Text <> "--ALL--" Then
                ddlEmployee.Items.Insert(0, "--ALL--")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
            objReports = Nothing
        Finally
            dt.Dispose()
        End Try
    End Sub

#End Region

#Region "Function ShowReport To Show the Report "
    Private Sub ShowReport(ByVal id As Integer)
        Try
            If ddlCompany.Items.Count <= 0 Then
                ddlCompany.Enabled = False
            End If

            If ddlProject.Items.Count <= 0 Then
                ddlProject.Enabled = False
            End If
            Dim strActionType As String = "Internal"
            Dim strCompanyType = Session("PropCompanyType")
            If id = 1 Then
                If Request("ip").ToString = "cdf" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallDetailFull.rpt")
                    crDocument.Load(Reportpath)

                Else
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallDetail.rpt")
                    crDocument.Load(Reportpath)

                End If
                'crDocument = New crCallTaskDetail
            ElseIf id = 2 Then
                If Request("ip").ToString = "cs2" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallSummaryAgg.rpt")
                    crDocument.Load(Reportpath)

                ElseIf Request("ip").ToString = "cs" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallSummary.rpt")
                    crDocument.Load(Reportpath)

                End If

                crvReport.SeparatePages = True
            ElseIf id = 3 Then
                '  crDocument = New crCallSummaryAgg
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crCallTaskDetail.rpt")
                crDocument.Load(Reportpath)

                'crvReport.SeparatePages = True
            ElseIf id = 4 Then
                '  crDocument = New crCallSummaryAgg
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crCallDetail.rpt")
                crDocument.Load(Reportpath)

                'crvReport.SeparatePages = True
            End If
            ' mstrCallNumber = CType(ddlCallNo.SelectedItem.Value.Trim, String)

            params = crDocument.DataDefinition.ParameterFields
            paramValues = New ParameterValues
            If id = 1 Then
                Dim strRequestCall As String = Request("CallNo")
                Dim strCompanyID As String = Request("CompanyID")
                If strRequestCall <> Nothing And strCompanyID <> Nothing Then
                    crDocument.SetParameterValue("CallNo", strRequestCall)
                    crDocument.SetParameterValue("CompanyID", strCompanyID)
                    mstrCallNumber = strRequestCall
                    mstrCompanyID = WSSSearch.SearchCompNameID(Val(strCompanyID)).ExtraValue
                    mstrCompanyIDN = strCompanyID

                    'cpnlError.Visible = False

                    strRecordSelectionFormula = "{T040011.CM_NU9_Call_No_PK}=" & mstrCallNumber & " and  " & "{T040011.CM_NU9_Comp_Id_FK} = " & mstrCompanyIDN

                    crDocument.RecordSelectionFormula = strRecordSelectionFormula

                    strRequestCall = Nothing
                    If Request("ip").ToString = "cdf" Then
                        showHideContents(4)
                    Else
                        showHideContents(1)
                    End If
                    crvReport.HasSearchButton = False
                    crvReport.HasViewList = False
                    crvReport.HasDrillUpButton = False
                    crvReport.SeparatePages = False
                    crvReport.EnableDrillDown = False
                    clsReport.LogonInformation(crDocument)
                    crvReport.ReportSource = crDocument
                    crvReport.DataBind()
                    Exit Sub
                End If

                If strRequestCall = Nothing And strCompanyID = Nothing Then
                    If ddlCompany.SelectedIndex <= 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Select Company ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        'cpnlError.Visible = False
                    End If
                End If
              
                If txtCallNumber.Text <> "" Then
                    'cpnlError.Visible = False
                End If
                'If ddlCallFrom.SelectedIndex <= 0 And strRequestCall = Nothing Then
                If txtCallNumber.Text = "" And strRequestCall = Nothing Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Enter Call Number ... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                    'ElseIf ddlCallFrom.SelectedIndex > 0 Or strRequestCall = Nothing Then
                ElseIf txtCallNumber.Text <> "" Or strRequestCall = Nothing Then
                    strRequestCall = Nothing
                    ' paramDiscrete.Value = ddlCallNo.SelectedItem.Value.Trim
                    crDocument.SetParameterValue("CallNo", txtCallNumber.Text.Trim)
                    crDocument.SetParameterValue("CompanyID", ddlCompany.SelectedItem.Value.Trim)
                    mstrCallNumber = txtCallNumber.Text.Trim  'ddlCallFrom.SelectedItem.Text.Trim
                    mstrCompanyID = ddlCompany.SelectedItem.Text.Trim
                    mstrCompanyIDN = ddlCompany.SelectedItem.Value.Trim
                Else
                    ' paramDiscrete.Value = strRequestCall
                    crDocument.SetParameterValue("CallNo", strRequestCall)
                    crDocument.SetParameterValue("CompanyID", strCompanyID)
                    mstrCallNumber = strRequestCall
                    mstrCompanyID = WSSSearch.SearchCompNameID(Val(strCompanyID)).ExtraValue
                    mstrCompanyIDN = strCompanyID

                End If
                'cpnlError.Visible = False

                strRecordSelectionFormula = "{T040011.CM_NU9_Call_No_PK}=" & mstrCallNumber & " and  " & "{T040011.CM_NU9_Comp_Id_FK} = " & mstrCompanyIDN

                crDocument.RecordSelectionFormula = strRecordSelectionFormula

                strRequestCall = Nothing
                If Request("ip").ToString = "cdf" Then
                    showHideContents(4)
                Else
                    showHideContents(1)
                End If
            End If

            If id = 3 Then
                Dim strRequestCall As String = Request("CallNo")
                Dim strCompanyID As String = Request("CompanyID")
                If strRequestCall = Nothing And strCompanyID = Nothing Then
                    If ddlCompany.SelectedIndex <= 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Select Company ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        'cpnlError.Visible = False
                    End If
                End If
                strRecordSelectionFormula = "{T040011.CM_NU9_Comp_Id_FK} = " & ddlCompany.SelectedItem.Value.Trim
                If txtCallNumber.Text.Trim <> "" Then
                    strRecordSelectionFormula &= " and" & " {T040011.CM_NU9_Call_No_PK} = " & Val(txtCallNumber.Text.Trim) '& " and "
                End If

                If Session("PropAdmin") <> "1" Then
                    strRecordSelectionFormula &= " and " & " {T040021.TM_VC8_Supp_Owner} = " & Session("PropUserID") '& " and "
                End If
                'cpnlError.Visible = False
                Dim dtFrom As String
                Dim dtTo As String
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text

                If txtCallNumber.Text.Trim = "" And dtFrom = Nothing And dtTo = Nothing Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please select DateRange or call Number...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf txtCallNumber.Text.Trim = "" And dtFrom <> Nothing And dtTo = Nothing Then
                    If CDate(dtFrom) > Date.Now() Then
                        lstError.Items.Clear()
                        lstError.Items.Add("From Date can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        dtTo = Date.Now.ToShortDateString
                        lstError.Items.Clear()
                    End If
                ElseIf txtCallNumber.Text.Trim <> "" And dtFrom <> Nothing And dtTo = Nothing Then
                    If CDate(dtFrom) > Date.Now() Then
                        lstError.Items.Clear()
                        lstError.Items.Add("From Date can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        dtTo = Date.Now.ToShortDateString
                        lstError.Items.Clear()
                    End If
                ElseIf txtCallNumber.Text.Trim = "" And dtFrom <> Nothing And dtTo <> Nothing Then
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
                ElseIf txtCallNumber.Text.Trim <> "" And dtFrom <> Nothing And dtTo <> Nothing Then
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
                End If

                If txtCallNumber.Text.Trim <> "" Then
                    If dtFrom <> Nothing And dtTo <> Nothing Then
                        strRecordSelectionFormula &= " and " & "{T040021.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                        Dim From As String = "01/01/1932"
                        dtFrom = CDate(From)
                        strRecordSelectionFormula &= " and " & "{T040021.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                        'Dim From As String = "01/01/1932"
                        dtFrom = Date.Now.ToShortDateString
                        strRecordSelectionFormula &= " and " & "{T040021.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    End If
                Else
                    If dtFrom <> Nothing And dtTo <> Nothing Then
                        strRecordSelectionFormula &= " and  " & "{T040021.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                        Dim From As String = "01/01/1932"
                        dtFrom = CDate(From)
                        strRecordSelectionFormula &= " and  " & "{T040021.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                        'Dim From As String = "01/01/1932"
                        dtFrom = Date.Now.ToShortDateString
                        strRecordSelectionFormula &= " and " & "{T040021.TM_DT8_Task_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    End If
                End If


                If dtFrom <> Nothing And dtTo <> Nothing Then
                    If DateDiff(DateInterval.Day, CType(dtFrom, Date), CType(dtTo, Date)) < 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("To date should be grater than From date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        'cpnlError.Visible = False
                    End If
                End If

                crDocument.RecordSelectionFormula = strRecordSelectionFormula

                strRequestCall = Nothing
                If Request("ip").ToString = "cdf" Then
                    showHideContents(4)
                Else
                    showHideContents(1)
                End If
            End If
            If id = 4 Then
                Dim strRequestCall As String = Request("CallNo")
                Dim strRequestTask As String = Request("TaskNo")
                Dim strCompanyID As String = Request("CompanyID")
                crDocument.SetParameterValue("CallNo", strRequestCall)
                crDocument.SetParameterValue("CompanyID", strCompanyID)

                mstrCallNumber = CType(strRequestCall, Integer)

                mstrCompanyID = WSSSearch.SearchCompNameID(Val(strCompanyID)).ExtraValue
                mstrTaskNumber = CType(strRequestTask, Integer)

                strRecordSelectionFormula = "{T040011.CM_NU9_Call_No_PK}=" & strRequestCall & " and  " & "{T040011.CM_NU9_Comp_Id_FK} = " & strCompanyID & " and  " & "{T040021.TM_NU9_Task_no_PK}= " & strRequestTask

                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                showHideContents(1)
                showHideContents(3)
            End If


            If id = 2 Then

                mstrCallNumber = 32
                'code to select company 
                If mshCall = 0 Then
                    strRecordSelectionFormula += "{T040011.CN_VC20_Call_Status}<>" & "'CLOSED'" & " and"
                End If

                If ddlCompany.SelectedIndex >= 0 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{T040011.CM_NU9_Comp_Id_FK}=" & strCompany & " and"
                End If

                If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count >= 1 Then
                    Dim strProject As String = ddlProject.SelectedItem.Text.ToString.Trim
                    strRecordSelectionFormula += "{Command_3.Name}=" & "'" & strProject & "'" & "and"
                End If

                If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                    Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{T010011_CallOwner.CI_NU8_Address_Number}=" & strEmployee & " and"
                End If
                Dim dtFrom As String
                Dim dtTo As String
                Dim intCallFrom, intCallTo As Integer
                intCallFrom = 0
                intCallTo = 0
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text

                If ddlCallFrom.SelectedIndex <> 0 And ddlCallFrom.Items.Count > 1 Then
                    intCallFrom = txtCallNumber.Text.Trim  'CType(ddlCallFrom.SelectedItem.Value, Integer)

                End If
                If ddlCallTo.SelectedIndex <> 0 And ddlCallTo.Items.Count > 1 Then
                    intCallTo = CType(ddlCallTo.SelectedItem.Text.Trim, Integer)
                End If

                If intCallFrom = 0 And intCallTo <> 0 Then
                    strRecordSelectionFormula += "{T040011.CM_NU9_Call_No_PK} <= " & intCallTo & " and"
                ElseIf intCallFrom <> 0 And intCallTo = 0 Then
                    strRecordSelectionFormula += "{T040011.CM_NU9_Call_No_PK} >= " & intCallFrom & " and"
                ElseIf intCallFrom <> 0 And intCallTo <> 0 Then
                    strRecordSelectionFormula += "{T040011.CM_NU9_Call_No_PK} >= " & intCallFrom & " and" & "{T040011.CM_NU9_Call_No_PK} <= " & intCallTo & " and"
                End If

                Dim i As Integer
                If dtFromDate.Text = Nothing Then
                    i = Today.DayOfWeek
                    dtFromDate.Text = SetDateFormat(Today.AddDays(0 - i))
                End If

                If dtToDate.Text = Nothing Then
                    i = Today.DayOfWeek
                    dtToDate.Text = SetDateFormat(Today.AddDays(6 - i))
                End If

                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text

                If dtTo = Nothing And dtFrom = Nothing Then
                    strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} >= " & "#" & dtFrom & "#"

                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If

                crDocument.RecordSelectionFormula = strRecordSelectionFormula

                showHideContents(2)
            End If
            HttpContext.Current.Session("PropCallNumber") = mstrCallNumber

            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            If intPageFlag = 1 Then
                crvReport.SeparatePages = True
            ElseIf intPageFlag = 2 Then
                crvReport.SeparatePages = False
            End If

            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False


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

    Private Sub showHideContents(ByVal ID As Integer)
        Try

            objReports = New clsReportData
            Dim dt As New DataTable
            dt = objReports.extractCompanyType(HttpContext.Current.Session("PropCompanyID"))
            If dt.Rows(0).Item(0) <> "SCM" Then

                Select Case ID
                    Case 1
                        crDocument.ReportDefinition.ReportObjects.Item("rtxtUsedHrs").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("rtxtActHrs").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("rtxtEstHrs").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text15").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text16").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text20").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text30").ObjectFormat.EnableSuppress = True

                        crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("AMFL8UsedHr1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS2").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS3").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("TMFL8EstHr1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS2").ObjectFormat.EnableSuppress = True

                        crDocument.ReportDefinition.ReportObjects.Item("Box6").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Box7").Left = 300
                        crDocument.ReportDefinition.Sections.Item("GroupHeaderSection4").SectionFormat.EnableSuppress = True
                        crDocument.ReportDefinition.Sections.Item("GroupFooterSection4").SectionFormat.EnableSuppress = True

                    Case 2

                        crDocument.ReportDefinition.ReportObjects.Item("Text11").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text12").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text4 ").Left = 10225

                    Case 4
                        crDocument.ReportDefinition.ReportObjects.Item("rtxtUsedHrs").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("rtxtActHrs").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("rtxtEstHrs").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text15").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text16").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text20").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Text30").ObjectFormat.EnableSuppress = True

                        crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS3").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS4").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("AMFL8UsedHr1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS2").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("UsedHRS3").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("TMFL8EstHr1").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS2").ObjectFormat.EnableSuppress = True

                        crDocument.ReportDefinition.ReportObjects.Item("Box6").ObjectFormat.EnableSuppress = True
                        crDocument.ReportDefinition.ReportObjects.Item("Box7").Left = 300
                        crDocument.ReportDefinition.Sections.Item("GroupHeaderSection4").SectionFormat.EnableSuppress = True
                        crDocument.ReportDefinition.Sections.Item("GroupFooterSection4").SectionFormat.EnableSuppress = True

                    Case 3
                    Case Else
                        Exit Sub
                End Select
            End If

            If ID = 3 Then
                crDocument.ReportDefinition.ReportObjects.Item("Text20").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("Text30").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("estimatedHRS2").ObjectFormat.EnableSuppress = True

                crDocument.ReportDefinition.ReportObjects.Item("UsedHRS3").ObjectFormat.EnableSuppress = True
                crDocument.ReportDefinition.ReportObjects.Item("Box7").ObjectFormat.EnableSuppress = True
            End If
            
            objReports = Nothing

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub

    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
        Session("PropCallNumber") = 0
    End Sub

    'Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
    '    If Request("ip").ToString = "ctad" Then

    '        Server.Transfer("CReports.aspx?ip=ctd&CallNo=" & Request("CallNo") & "&CompanyID= " & Request("CompanyID"), False)
    '    Else
    '        Response.Redirect("../home.aspx", False)
    '    End If
    'End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
#End Region

End Class
