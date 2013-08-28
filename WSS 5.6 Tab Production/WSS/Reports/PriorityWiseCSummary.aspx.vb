#Region "Reffered Assemblies"
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

#End Region
Partial Class Reports_PriorityWiseCSummary
    Inherits System.Web.UI.Page
#Region " Page Level Varaibles"

    Private crDocument As ReportDocument
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
    Private strRecordSelectionFormula As String
    Private intCallFrom, intCallTo As Integer
    Shared mshCall As Short
    Private showclosecall As Boolean
#End Region

#Region " Page Load Event"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' CDDLCallNo.CDDLPopUpURL = "../Search/Common/PopSearch.aspx"
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

            If Not IsPostBack Then
                'showclosecall = False
                ViewState("VSclosecall") = "True"
                If Not IsNothing(Request.QueryString("ScrID")) Then
                    HIDSCRID.Value = Request.QueryString("ScrID")
                End If

            End If
            Dim txthiddenImage = Request.Form("txthiddenImage")

            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
                ddlCompany2.Enabled = False
            End If


            imgOK.Attributes.Add("OnClick", "ShowImg();")
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "','" & ddlCallFrom.ClientID & "','" & ddlCallTo.ClientID & "');")
            ddlProject.Attributes.Add("OnChange", "DDLChange(1, '" & ddlCompany.ClientID & "','" & ddlProject.ClientID & "','" & ddlEmployee.ClientID & "','" & ddlCallFrom.ClientID & "','" & ddlCallTo.ClientID & "');")

            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            'imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Attach"
                            '   Response.Write("<script>window.open('../MessageCenter/UploadFiles/                                            ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width                                   =800,height=350,status=yes');</script>")


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

                lstError.Items.Clear()
                lstError.Items.Add("url of this page has been changed . contact your administrator")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub


            ElseIf Request("ip").ToString = "cdf" Then

                lstError.Items.Clear()
                lstError.Items.Add("url of this page has been changed . contact your administrator")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            ElseIf Request("ip").ToString = "ctd" Then

                lstError.Items.Clear()
                lstError.Items.Add("url of this page has been changed . contact your administrator")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            ElseIf Request("ip").ToString = "ctad" Then
                lstError.Items.Clear()
                lstError.Items.Add("url of this page has been changed . contact your administrator")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub

            ElseIf Request("ip").ToString = "PCS" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = HIDSCRID.Value '499
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block


                'Response.Write("<head><title>" & "CALL SUMMARY" & "</title></head>")

                If IsPostBack Then
                    FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS:" & ddlProject.ID, New ListItem("--ALL--", 0))
                    FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                    FillAjaxDropDown(ddlCallFrom, Request.Form("txthiddenCallnos"), "cpnlRS:" & ddlCallFrom.ID, New ListItem("--ALL--", 0))
                    FillAjaxDropDown(ddlCallTo, Request.Form("txthiddenCallnos2"), "cpnlRS:" & ddlCallTo.ID, New ListItem("--ALL--", 0))

                Else
                    fill_company()
                    fillPriority()
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                    fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                    fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                End If

                lblHead.Text = "CALL SUMMARY"
                lblCallNo.Visible = False
                ddlCallNo.Visible = False
                lblCompany2.Visible = False
                ddlCompany2.Visible = False
                CpnlCallDetails.Visible = False
                imgAttach.Visible = False
                cpnlReport.Text = "CALL SUMMARY"
                cpnlRS.Text = "CALL SUMMARY"
                If IsPostBack Then
                    ShowReport(2)
                End If
            ElseIf Request("ip").ToString = "cs2" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 525
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block

                'Response.Write("<head><title>" & "CALL SUMMARY" & "</title></head>")

                If IsPostBack Then
                    FillAjaxDropDown(ddlProject, Request.Form("txthiddenProject"), "cpnlRS:" & ddlProject.ID, New ListItem("--ALL--", 0))
                    FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                    FillAjaxDropDown(ddlCallFrom, Request.Form("txthiddenCallnos"), "cpnlRS:" & ddlCallFrom.ID, New ListItem("--ALL--", 0))
                    FillAjaxDropDown(ddlCallTo, Request.Form("txthiddenCallnos2"), "cpnlRS:" & ddlCallTo.ID, New ListItem("--ALL--", 0))

                Else
                    fill_company()
                    fillPriority()
                    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                    fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                    fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                End If

                lblHead.Text = "CALL SUMMARY"
                lblCallNo.Visible = False
                ddlCallNo.Visible = False
                lblCompany2.Visible = False
                ddlCompany2.Visible = False
                CpnlCallDetails.Visible = False
                imgAttach.Visible = False
                cpnlReport.Text = "CALL SUMMARY"
                cpnlRS.Text = "CALL SUMMARY"
                If IsPostBack Then
                    ShowReport(2)
                End If

            Else
                Response.Redirect("reportsindex.aspx")
            End If
            If ddlCallNo.SelectedIndex = 0 Then
                Exit Sub

            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Fill Drop Down List Boxes"


    Private Sub fill_CallNo(ByVal id As Integer, ByVal CompanyID As Integer, ByVal ProjectID As Integer)
        'go to branch and fill the combo
        'CDDLCallNo.CDDLQuery = "select CM_NU9_Call_No_PK ID, CN_VC20_Call_Status  Status, CM_VC100_Subject Subject from T040011 where CM_NU9_Comp_Id_FK=" & ddlCompany2.SelectedValue
        'CDDLCallNo.CDDLUDC = True
        'CDDLCallNo.CDDLFillDropDown(15, True)
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
                dt = objReports.extractCallNo(1, CompanyID, ProjectID)
                ddlCallFrom.DataSource = dt
                ddlCallFrom.DataTextField = "CallNo"
                ddlCallFrom.DataValueField = "CallNo"
                ddlCallFrom.DataBind()
                ddlCallFrom.Items.Insert(0, New ListItem("--ALL--", 0))
            End If
            If id = 3 Then
                ddlCallTo.Items.Clear()
                dt = objReports.extractCallNo(1, CompanyID, ProjectID)
                ddlCallTo.DataSource = dt
                ddlCallTo.DataTextField = "CallNo"
                ddlCallTo.DataValueField = "CallNo"
                ddlCallTo.DataBind()
                ddlCallTo.Items.Insert(0, New ListItem("--ALL--", 0))
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_company()
        Try
            If Request("ip").ToString = "PCS" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCompany(1)
                ddlCompany.DataSource = dt
                ddlCompany.DataTextField = "Name"
                ddlCompany.DataValueField = "ID"
                ddlCompany.DataBind()
                'ddlCompany.Items.Insert(0, "--ALL--")
                If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                    ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
                End If
            ElseIf Request("ip").ToString = "cd" Or Request("ip").ToString = "cdf" Or Request("ip").ToString = "ctd" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCompany(1)
                ddlCompany2.DataSource = dt
                ddlCompany2.DataTextField = "Name"
                ddlCompany2.DataValueField = "ID"
                ddlCompany2.DataBind()
                ddlCompany2.Items.Insert(0, "--Select--")
                If Request("CompanyID") <> Nothing Then
                    ddlCompany2.SelectedValue = CType(Request("CompanyID"), Integer)
                ElseIf HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                    ddlCompany2.SelectedValue = HttpContext.Current.Session("PropCompanyID")
                End If
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_Project(ByVal comapnyID As Integer)
        Try
            If Request("ip").ToString = "PCS" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                'If ddlCompany.SelectedIndex = 0 Then
                '    dt = objReports.extractProject(0)
                'Else
                dt = objReports.extractProject(comapnyID)
                'End If
                If dt.Rows.Count > 0 Then
                    ddlProject.DataSource = dt
                    ddlProject.DataTextField = "Name"
                    ddlProject.DataValueField = "ID"
                    ddlProject.DataBind()
                    If ddlProject.Items(0).Text <> "--ALL--" Then
                        ddlProject.Items.Insert(0, New ListItem("--ALL--", 0))
                    End If
                Else
                    ddlProject.Items.Insert(0, New ListItem("--ALL--", 0))
                End If

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
            dt = objReports.extractCallOwner(companyID, projectID, 1)
            If dt.Rows.Count > 0 Then
                ddlEmployee.DataSource = dt
                ddlEmployee.DataTextField = "Name"
                ddlEmployee.DataValueField = "AddressNo"
                ddlEmployee.DataBind()
                If ddlEmployee.Items(0).Text <> "--ALL--" Then
                    ddlEmployee.Items.Insert(0, New ListItem("--ALL--", 0))
                End If
            Else
                ddlEmployee.Items.Insert(0, New ListItem("--ALL--", 0))
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
            objReports = Nothing
        Finally
            dt.Dispose()
        End Try
    End Sub
    Sub fillPriority()
        Dim dt As New DataTable
        Try
            objReports = New clsReportData
            dt = objReports.ExtractPriority
            ddlCPriority.DataSource = dt
            ddlCPriority.DataTextField = "priority"
            'ddlCPriority.DataValueField = "priority"
            ddlCPriority.DataBind()
            If ddlCPriority.Items(0).Text <> "--ALL--" Then
                ddlCPriority.Items.Insert(0, New ListItem("--ALL--", 0))
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
            Dim strActionType As String = "Internal"
            Dim strCompanyType As String = Session("PropCompanyType")
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
            ElseIf id = 2 Then
                If Request("ip").ToString = "PCS" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crPriorityCS2.rpt")
                    crDocument.Load(Reportpath)

                End If
                'crvReport.SeparatePages = True
                'params = crDocument.DataDefinition.ParameterFields
                'paramValues = New ParameterValues
                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                ' showHideContents(1)
                'showHideContents(3)
            End If
            If id = 2 Then
                mstrCallNumber = 32
                Dim dtFrom As String
                Dim dtTo As String
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text
                'If dtFromDate.CalendarDate = Nothing Then
                '    intI = Today.DayOfWeek
                '    dtFromDate.CalendarDate = SetDateFormat(Today.AddDays(0 - intI))
                'End If
                'If dtToDate.CalendarDate = Nothing Then
                '    intI = Today.DayOfWeek
                '    dtToDate.CalendarDate = SetDateFormat(Today.AddDays(6 - intI))
                'End If
                'If dtFrom <> Nothing And dtTo <> Nothing Then
                '    If DateDiff(DateInterval.Day, CType(dtFrom, Date), CType(dtTo, Date)) < 0 Then
                '        lstError.Items.Clear()
                '        lstError.Items.Add("To date should be grater than From date ")
                '        ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                '        Exit Sub
                '    Else
                '        cpnlError.Visible = False
                '    End If
                'End If
                If dtFrom = Nothing And dtTo = Nothing And ddlCallFrom.SelectedIndex = 0 And ddlCallTo.SelectedIndex = 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please select DateRange or call Number...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf dtFrom = Nothing And dtTo = Nothing And ddlCallFrom.SelectedIndex <> 0 And ddlCallTo.SelectedIndex = 0 Then
                ElseIf dtFrom = Nothing And dtTo = Nothing And ddlCallFrom.SelectedIndex = 0 And ddlCallTo.SelectedIndex <> 0 Then
                ElseIf dtFrom <> Nothing And dtTo = Nothing And ddlCallFrom.SelectedIndex = 0 And ddlCallTo.SelectedIndex = 0 Then
                    If CDate(dtFrom) > Date.Now() Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Date From can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        dtTo = Date.Now.ToShortDateString
                        lstError.Items.Clear()
                    End If
                ElseIf dtFrom = Nothing And dtTo <> Nothing And ddlCallFrom.SelectedIndex = 0 And ddlCallTo.SelectedIndex = 0 Then
                    Dim From As String = "01/01/1932"
                    dtFrom = CDate(From)
                    'lstError.Items.Clear()
                    'lstError.Items.Add("Please Select From Date... ")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    'Exit Sub
                ElseIf dtFrom <> Nothing And dtTo <> Nothing And ddlCallFrom.SelectedIndex = 0 And ddlCallTo.SelectedIndex = 0 Then
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
                FillReportPCS()
                'showHideContents(2)
            End If
            HttpContext.Current.Session("PropCallNumber") = mstrCallNumber
            'HttpContext.Current.Session("PropCompanyID") = mstrCompanyID
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            If intPageFlag = 1 Then
                crvReport.SeparatePages = True
            ElseIf intPageFlag = 2 Then
                crvReport.SeparatePages = False
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

    Private Sub ddlCompany2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany2.SelectedIndexChanged
        Try
            If ddlCompany2.SelectedIndex = 0 Then
                ddlCallNo.Enabled = False
            Else
                ddlCallNo.Enabled = True
                Dim intSelectedValue As Integer = CType(ddlCompany2.SelectedItem.Value.Trim, Integer)
                fill_CallNo(1, intSelectedValue, 0)
                ddlCallNo.SelectedIndex = 0

            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
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
        HttpContext.Current.Session("PropCallNumber") = 0
        HttpContext.Current.Session("PropTaskNumber") = 0
    End Sub

    Private Sub FillReportPCS()
        Dim s As String
        Dim dtFrom As String = dtFromDate.Text
        Dim dtTo As String = dtToDate.Text
        Dim dsCP As New DSPriorityCS
        Dim dsTemp As New DataSet
        Dim inti As Integer

        inti = 0

        intCallFrom = 0
        intCallTo = 0
        If ddlCallFrom.SelectedIndex <> 0 And ddlCallFrom.Items.Count > 1 Then
            intCallFrom = CType(ddlCallFrom.SelectedItem.Value, Integer)
        End If
        If ddlCallTo.SelectedIndex <> 0 And ddlCallTo.Items.Count > 1 Then
            intCallTo = CType(ddlCallTo.SelectedItem.Text.Trim, Integer)
        End If
        Dim htCols As New Hashtable
        htCols.Add("CM_DT8_Request_Date", 2)
        htCols.Add("CM_Dt8_Call_Close_Date", 2)
        Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim

        s = "(select estimatedHRS=0,CM_NU9_Call_No_PK callno, CI_VC36_Name, UsedHrs=0,UM_VC50_UserID , convert(Varchar,CM_DT8_Request_Date) CM_DT8_Request_Date, convert(varchar,CM_Dt8_Call_Close_Date) CM_Dt8_Call_Close_Date  , PR_VC20_Name,CN_VC20_Call_Status,cm_vc200_work_priority,CM_NU9_Comp_ID_FK  from T210011,T040011,T010011, T060011 where CM_NU9_Project_ID=PR_NU9_Project_ID_PK and CM_NU9_Comp_ID_FK=PR_NU9_Comp_ID_FK and CI_NU8_Address_number=CM_NU9_Comp_ID_FK and UM_IN4_Address_No_FK=CM_NU9_Call_Owner and CM_NU9_Call_No_PK  not in ( select callno from (select SUM(TM_FL8_Est_Hr) AS estimatedHRS, T.callno,CI_VC36_Name, T.UsedHrs ,UM_VC50_UserID,CM_DT8_Request_Date, CM_Dt8_Call_Close_Date  , PR_VC20_Name,CN_VC20_Call_Status from T210011,T040011,T040021,T010011, T060011,(SELECT AM_NU9_Call_Number AS callno,AM_NU9_Comp_ID_FK as CompanyID ,SUM(AM_FL8_Used_Hr) AS UsedHRS FROM T040031,T040021 where AM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and AM_NU9_Call_Number=TM_NU9_Call_No_FK and AM_NU9_Task_Number=TM_NU9_Task_No_PK group by AM_NU9_Call_Number,AM_NU9_Comp_ID_FK)as T where T.callno=TM_NU9_Call_No_FK and T.CompanyID=TM_NU9_Comp_ID_FK and T.callno=CM_NU9_Call_No_PK and T.CompanyID=CM_NU9_Comp_ID_FK and T.CompanyID=PR_NU9_Comp_ID_FK and T040011.CM_NU9_Project_ID=PR_NU9_Project_ID_PK and CI_NU8_Address_number=T.CompanyID and T.CompanyID=UM_IN4_Company_AB_ID and UM_IN4_Address_No_FK=CM_NU9_Call_Owner group by TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,T.callno,CI_VC36_Name,T.UsedHrs,UM_VC50_UserID,CM_DT8_Request_Date,CM_Dt8_Call_Close_Date, CN_VC20_Call_Status ,PR_VC20_Name,cm_vc200_work_priority,CM_NU9_Comp_ID_FK )T1) ) union (select SUM(TM_FL8_Est_Hr) AS estimatedHRS, T.callno,CI_VC36_Name, T.UsedHrs ,UM_VC50_UserID, convert(Varchar,CM_DT8_Request_Date)CM_DT8_Request_Date, convert(varchar,CM_Dt8_Call_Close_Date) CM_Dt8_Call_Close_Date  , PR_VC20_Name,CN_VC20_Call_Status,cm_vc200_work_priority,CM_NU9_Comp_ID_FK  from T210011,T040011,T040021,T010011, T060011,(SELECT AM_NU9_Call_Number AS callno,AM_NU9_Comp_ID_FK as CompanyID ,SUM(AM_FL8_Used_Hr) AS UsedHRS FROM T040031,T040021 where AM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and AM_NU9_Call_Number=TM_NU9_Call_No_FK and AM_NU9_Task_Number=TM_NU9_Task_No_PK group by AM_NU9_Call_Number,AM_NU9_Comp_ID_FK)as T where T.callno=TM_NU9_Call_No_FK and T.CompanyID=TM_NU9_Comp_ID_FK and T.callno=CM_NU9_Call_No_PK and T.CompanyID=CM_NU9_Comp_ID_FK and T.CompanyID=PR_NU9_Comp_ID_FK and T040011.CM_NU9_Project_ID=PR_NU9_Project_ID_PK and CI_NU8_Address_number=T.CompanyID and T.CompanyID=UM_IN4_Company_AB_ID and UM_IN4_Address_No_FK=CM_NU9_Call_Owner group by TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,T.callno,CI_VC36_Name,T.UsedHrs,UM_VC50_UserID,CM_DT8_Request_Date,CM_Dt8_Call_Close_Date, CN_VC20_Call_Status ,PR_VC20_Name,cm_vc200_work_priority,CM_NU9_Comp_ID_FK )"

        Dim strFilter As String = "CI_VC36_Name='" & ddlCompany.SelectedItem.Text & "'"
        If ddlCallFrom.SelectedValue > 0 Then
            strFilter &= " and callno >=" & ddlCallFrom.SelectedValue
        End If
        If ddlCallTo.SelectedValue > 0 Then
            strFilter &= " and callno <=" & ddlCallTo.SelectedValue
        End If

        If ViewState("VSclosecall") = "False" Then
            ViewState("VSclosecall") = "True"
        Else
            ViewState("VSclosecall") = "False"
        End If
        If ViewState("VSclosecall") = "False" Then
            strFilter &= " and CN_VC20_Call_Status<>'closed'"
        End If
        If dtFrom <> "" Then
            strFilter &= " and CM_DT8_Request_Date >= '" & dtFrom & "'"
        End If
        If dtTo <> "" Then
            strFilter &= " and CM_DT8_Request_Date <='" & dtTo & "'"
        End If
        If ddlCPriority.SelectedItem.Text <> "--ALL--" Then
            Dim strPriority As String
            strPriority = ddlCPriority.SelectedItem.Text
            strFilter &= " and cm_vc200_work_priority ='" & strPriority & "'"
        End If
        If ddlEmployee.SelectedValue <> 0 Then
            Dim strCallOwner As String = ddlEmployee.SelectedItem.Text
            strCallOwner = strCallOwner.Remove(strCallOwner.IndexOf("["), strCallOwner.Substring(strCallOwner.IndexOf("[")).Length)
            strFilter &= " and UM_VC50_UserID='" & strCallOwner.Trim & "'"
        End If
        If ddlProject.SelectedValue <> 0 Then
            strFilter &= " and PR_VC20_Name='" & ddlProject.SelectedItem.Text & "'"
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'Dim ds1 As New Dataset1
        SQL.DBTracing = False
        SQL.Search("element1", "WSSReportsD", "ExtractCallNo", s, dsCP, "jagmit", "sidhu")
        SetDataTableDateFormat(dsCP.Tables(0), htCols)
        Dim dvTemp As New DataView
        dvTemp = dsCP.Tables(0).DefaultView
        If strFilter <> "" Then
            GetFilteredDataView(dvTemp, strFilter)
            dsTemp.Tables.Add(dvTemp.Table)

        Else
            dsTemp.Tables.Add(dsCP.Tables(0))
        End If
        dsTemp.AcceptChanges()
        dsTemp.Tables(0).TableName = "element1"
        crDocument.SetDataSource(dsTemp)
        dsCP.Dispose()
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
