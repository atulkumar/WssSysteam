'************************************************************************************************************
' Page                 : - Timeregistration
' Purpose              : - it will show the call crTimeRegisteration report,crTimeRegisterationCall,                                    crTimeRegistrationAction,crDailyReport report(one at a time)depends upon the value                           of query string passed .                                                
' Date		    		   Author						Modification Date					Description
' 27/04/06				   Atul 					             					        Created
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

Partial Class Reports_TimeRegistration
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

#Region " Page Load Function"


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim txthiddenImage = Request.Form("txthiddenImage")
            HIDSCRID.Value = Request.QueryString("ScrID")
            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If

            imgOK.Attributes.Add("OnClick", "ShowImg();")
            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlEmployee.ClientID & "');")

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
            'If ddlCallNo.Items.Count = -1 Or ddlCallNo.Items.Count = 0 Then
            'Exit Sub
            'End If
            'mstrCallNumber = CType(ddlCallNo.SelectedItem.Value.Trim, String)
            'HttpContext.Current.Session("PropCallNumber") = CType(mstrCallNumber, Integer)
            'dtFromDate.LeftPos = 500
            'dtFromDate.TopPos = 30
            'dtToDate.LeftPos = 550
            'dtToDate.TopPos = 30
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region "Function Show() "


    Private Sub show()
        Try
            If dtWeekDate.CalendarDate = Nothing Then
                Dim i As Integer
                i = Today.DayOfWeek
                dtWeekDate.CalendarDate = SetDateFormat(Today.AddDays(-i))
            End If
            Select Case Request("ip")
                Case Nothing
                    Response.Redirect("ReportsIndex.aspx")
                Case "tms"
                    dtWeekDate.Visible = False
                    lblDateWeek.Visible = False
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 548
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block




                    'Response.Write("<head><title>" & "TIME REGISTRATION" & "</title></head>")

                    cpnlReport.Text = "TIME REGISTRATION"
                    cpnlRS.Text = "TIME REGISTRATION"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                    End If

                    If Not IsPostBack Then
                        fill_company()
                        'fill_Project(HttpContext.Current.Session("PropCompanyID"))
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If
                    lblHead.Text = "TIME REGISTRATION"
                    If IsPostBack Then
                        Showreport(1)
                    End If

                Case "tms2"
                    'Response.Write("<head><title>" & "TIME REGISTRATION" & "</title></head>")

                    cpnlReport.Text = "TIME REGISTRATION"
                    cpnlRS.Text = "TIME REGISTRATION"

                    If IsPostBack Then
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                    End If

                    If Not IsPostBack Then
                        fill_company()
                        '   fill_Project(ddlCompany.SelectedValue.Trim)
                        fill_employee(ddlCompany.SelectedValue.Trim, 0, 2)
                    End If
                    lblHead.Text = "TIME REGISTRATION"
                    'imgClose.Attributes.Add("Onclick", "return back('" & Request("ip") & "');")
                    Showreport(2)

                Case "tms3"
                    'Response.Write("<head><title>" & "TIME REGISTRATION" & "</title></head>")

                    cpnlReport.Text = "TIME REGISTRATION"
                    cpnlRS.Visible = False
                    cpnlRS.Text = "TIME REGISTRATION"
                    'If Not IsPostBack Then
                    '    fill_company()
                    '    fill_Project(HttpContext.Current.Session("PropCompanyID"))
                    '    fill_employee(HttpContext.Current.Session("PropCompanyID"), 2)
                    'End If
                    lblHead.Text = "TIME REGISTRATION"
                    'imgClose.Visible = False
                    'imgClose.Attributes.Add("Onclick", "return back('" & Request("ip") & "');")
                    Showreport(3)
                Case "tms4"


                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 737
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block
                    If IsPostBack Then
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                    End If


                    If Not IsPostBack Then
                        fill_company()
                        fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                    End If

                    'Response.Write("<head><title>" & "DAILY REPORT" & "</title></head>")

                    cpnlReport.Text = "DAILY REPORT"
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "DAILY REPORT"

                    lblHead.Text = "Daily Report"
                    'imgClose.Visible = False
                    'imgClose.Attributes.Add("Onclick", "return back('" & Request("ip") & "');")
                    'tdWeekDate.Visible = False
                    'tdweekdateTitle.Visible = False
                    ' tdProject1.Visible = False
                    ' tdProject2.Visible = False
                    If IsPostBack Then
                        Showreport(4)
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
                ddlCompany.Items.Insert(0, "--ALL--")
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
    'Private Sub fill_Project(ByVal comapnyID As Integer)
    '    Try

    '        Dim dt As New DataTable
    '        objReports = New clsReportData
    '        If ddlCompany.SelectedIndex = 0 Then
    '            dt = objReports.extractProject(0)
    '        Else
    '            dt = objReports.extractProject(comapnyID)
    '        End If
    '        ddlProject.DataSource = dt
    '        ddlProject.DataTextField = "Name"
    '        ddlProject.DataValueField = "Name"
    '        ddlProject.DataBind()
    '        If ddlProject.Items(0).Text <> "--ALL--" Then
    '            ddlProject.Items.Insert(0, "--ALL--")
    '        End If


    '    Catch ex As Exception
    '        Dim str As String = ex.Message.ToString

    '    Finally
    '        objReports = Nothing
    '    End Try
    'End Sub

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractTaskOwner(companyID, projectID, 2)
            'dt = objReports.extractCustomer(companyID, projectID, category)
            If dt.Rows.Count > 0 Then
                ddlEmployee.DataSource = dt
                ddlEmployee.DataTextField = "Name"
                ddlEmployee.DataValueField = "AddressNo"
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

#Region "Function ShowReport()"

    Private Sub Showreport(ByVal id As Integer)
        Try
            If id = 1 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crTimeRegisteration.rpt")
                crDocument.Load(Reportpath)

            ElseIf id = 2 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crTimeRegisterationCall.rpt")
                crDocument.Load(Reportpath)

            ElseIf id = 3 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crTimeRegistrationAction.rpt")
                crDocument.Load(Reportpath)

            ElseIf id = 4 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crDailyReport.rpt")
                crDocument.Load(Reportpath)

            Else
                Exit Sub
            End If
            ' mstrCallNumber = CType(ddlCallNo.SelectedItem.Value.Trim, String)
            Dim strRecordSelectionFormula As String
            If id = 1 Then
                Dim dtFrom As String
                Dim dtTo As String
                Dim intCallFrom, intCallTo As Integer
                intCallFrom = 0
                intCallTo = 0
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text
                Dim dtToDate1 As DateTime
                If dtFrom = Nothing And dtTo = Nothing Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please Select Date...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    Dim From As String = "01/01/1932"
                    dtFrom = CDate(From)
                    'lstError.Items.Clear()
                    'lstError.Items.Add("Please Select Date...")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    'Exit Sub
                End If
                If dtFrom <> Nothing And dtTo <> Nothing Then
                    If CDate(dtFrom) > CDate(dtTo) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Date From can not be greater than To Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                End If
                If dtFrom <> Nothing Then
                    If CDate(dtFrom) > Date.Now() Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Date From can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                End If
                If dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " {Command.adate} >= " & "#" & dtFrom & "#"
                End If
                If dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " and " & " {Command.adate} <= " & "#" & dtTo & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {Command.adate} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"
                End If

                Dim dateFrom As FormulaFieldDefinition
                Dim dateTo As FormulaFieldDefinition
                dateFrom = crDocument.DataDefinition.FormulaFields("dateFrom")
                dateFrom.Text = Chr(34) & dtFrom & Chr(34)
                dateTo = crDocument.DataDefinition.FormulaFields("dateTo")
                dateTo.Text = Chr(34) & dtTo & Chr(34)
                'Label6.Visible = False
                'ddlProject.Visible = False
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += " and " & "{Command.cid}=" & strCompany


                End If
                'If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                '    Dim strProject As String = ddlProject.SelectedItem.Text.ToString.Trim
                '    strRecordSelectionFormula += "{Command.Name}=" & "'" & strProject & "'" & "and"
                'End If
                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += " and " & "{Command.owner}=" & strEmployee
                    End If
                Else
                    strRecordSelectionFormula += " and " & "{Command.owner}=" & Session("PropUserID")
                End If

                'Dim i As Integer
                'If dtFromDate.CalendarDate = Nothing Then
                '    dtFrom = dtWeekDate.CalendarDate
                'End If
                'If dtToDate.CalendarDate = Nothing Then
                '    dtTo = DateAdd("d", 1, Today)
                'End If

                'If dtFrom = Nothing And dtTo <> Nothing Then
                '    lstError.Items.Clear()
                '    lstError.Items.Add("Select From Date... ")
                '    ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                '    Exit Sub
                'ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                '    If CDate(dtFrom) > Date.Now() Then
                '        lstError.Items.Clear()
                '        lstError.Items.Add("Date From can not be greater than Current Date... ")
                '        ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                '        Exit Sub
                '    Else
                '        dtTo = Date.Now.ToShortDateString
                '        lstError.Items.Clear()
                '    End If

                'Else



                'If dtTo = Nothing And dtFrom = Nothing Then

                '    strRecordSelectionFormula += " {Command.adate} = " & "#" & dtFrom & "#"

                'Else


            End If
            If id = 2 Then
                If Request("cid") <> Nothing And Request("owner") <> Nothing Then
                    If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                        Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                        strRecordSelectionFormula += "{Command.cid}=" & strCompany & " and"
                    Else
                        strRecordSelectionFormula += "{Command.cid}=" & Request("cid") & " and"
                    End If
                    'If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    '    Dim strProject As String = ddlProject.SelectedItem.Text.ToString.Trim
                    '    strRecordSelectionFormula += "{Command.Name}=" & "'" & strProject & "'" & "and"
                    'End If
                    If Session("PropAdmin") = "1" Then
                        If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                            Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                            strRecordSelectionFormula += "{Command.owner}=" & strEmployee & " and"
                        Else
                            strRecordSelectionFormula += "{Command.owner}=" & Request("owner") & " and"
                        End If
                    Else
                        strRecordSelectionFormula += "{Command.owner}=" & Session("PropUserID") & " and"
                    End If
                    Dim dtFrom As String
                    Dim dtTo As String
                    Dim intCallFrom, intCallTo As Integer
                    intCallFrom = 0
                    intCallTo = 0
                    dtFrom = dtFromDate.Text
                    dtTo = dtToDate.Text
                    Dim dtToDate1 As DateTime
                    Dim i As Integer
                    If dtTo = Nothing And dtFrom = Nothing Then
                        If Request("dtFrom") <> Nothing Then
                            dtFrom = Request("dtFrom")
                        Else
                            dtFrom = dtWeekDate.CalendarDate
                        End If
                        If Request("dtTo") <> Nothing Then
                            dtTo = Request("dtTo")
                        Else
                            dtTo = Today
                        End If
                        strRecordSelectionFormula += " {Command.adate} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"
                        ' strRecordSelectionFormula += " {Command.adate} <= " & "#" & DateAdd("d", 1, Today) & "#"
                    ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                        strRecordSelectionFormula += " {Command.adate} <= " & "#" & dtTo & "#"
                    ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                        strRecordSelectionFormula += " {Command.adate} >= " & "#" & dtFrom & "#"
                    ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                        strRecordSelectionFormula += " {Command.adate} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"
                    End If
                    If dtFrom <> Nothing And dtTo <> Nothing Then
                        If DateDiff(DateInterval.Day, CType(dtFrom, Date), CType(dtTo, Date)) < 0 Then
                            lstError.Items.Clear()
                            lstError.Items.Add("To date should be grater than From date ")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        Else
                            cpnlError.Visible = False
                        End If
                    End If
                    Dim dateFrom As FormulaFieldDefinition
                    Dim dateTo As FormulaFieldDefinition
                    dateFrom = crDocument.DataDefinition.FormulaFields("dateFrom")
                    dateFrom.Text = Chr(34) & dtFrom & Chr(34)
                    dateTo = crDocument.DataDefinition.FormulaFields("dateTo")
                    dateTo.Text = Chr(34) & dtTo & Chr(34)
                End If
            End If
            If id = 3 Then
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{T040031.AM_NU9_Comp_Id_FK}=" & strCompany & " and"
                Else
                    strRecordSelectionFormula += "{T040031.AM_NU9_Comp_Id_FK}=" & Request("cid") & " and"
                End If
                'If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                '    Dim strProject As String = ddlProject.SelectedItem.Text.ToString.Trim
                '    strRecordSelectionFormula += "{Command.Name}=" & "'" & strProject & "'" & "and"
                'End If
                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += " {T040031.AM_VC8_Supp_Owner}=" & strEmployee & " and"
                    Else
                        strRecordSelectionFormula += " {T040031.AM_VC8_Supp_Owner}=" & Request("owner") & " and"
                    End If
                Else
                    strRecordSelectionFormula += " {T040031.AM_VC8_Supp_Owner}=" & Session("PropUserID") & " and"
                End If
                strRecordSelectionFormula += " {T040031.AM_NU9_Call_Number}=" & Request("callNo") & " and"
                Dim dtFrom As String
                Dim dtTo As String
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text
                Dim dtToDate1 As DateTime
                Dim i As Integer
                If dtTo = Nothing And dtFrom = Nothing Then
                    If Request("dtFrom") <> Nothing Then
                        dtFrom = Request("dtFrom")
                    Else
                        dtFrom = dtWeekDate.CalendarDate
                    End If
                    If Request("dtTo") <> Nothing Then
                        dtTo = Request("dtTo")       ' DateAdd("d", 1, Request("dtTo"))
                    Else
                        dtTo = Today
                    End If
                    strRecordSelectionFormula += "  {T040031.AM_DT8_Action_Date} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"
                    ' strRecordSelectionFormula += "  {T040031.AM_DT8_Action_Date} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += "  {T040031.AM_DT8_Action_Date} <= " & "#" & dtTo & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += "  {T040031.AM_DT8_Action_Date} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += "  {T040031.AM_DT8_Action_Date} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"
                End If
                If dtFrom <> Nothing And dtTo <> Nothing Then
                    If DateDiff(DateInterval.Day, CType(dtFrom, Date), CType(dtTo, Date)) < 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("To date should be grater than From date ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        cpnlError.Visible = False
                    End If
                End If
            End If
            If id = 4 Then
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                    Dim strCompany As String = ddlCompany.SelectedItem.Value.Trim
                    strRecordSelectionFormula += "{Command_1.AM_NU9_Comp_ID_FK}=" & strCompany & " and"
                End If
                'If ddlProject.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                '    Dim strProject As String = ddlProject.SelectedItem.Text.ToString.Trim
                '    strRecordSelectionFormula += "{Command.Name}=" & "'" & strProject & "'" & "and"
                'End If
                If Session("PropAdmin") = "1" Then
                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += " {Command_1.AM_VC8_Supp_Owner}=" & strEmployee & " and"
                    End If
                Else
                    strRecordSelectionFormula += " {Command_1.AM_VC8_Supp_Owner}=" & Session("PropUserID") & " and"
                End If
                Dim dtFrom As String
                Dim dtTo As String
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text
                Dim dtToDate1 As DateTime
                Dim i As Integer
                If dtTo = Nothing And dtFrom = Nothing Then
                    dtFromDate.Text = Today
                    dtToDate.Text = Today
                    dtFrom = dtFromDate.Text
                    dtTo = dtToDate.Text
                    'strRecordSelectionFormula += "   {Command_1.ActionDate} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                    strRecordSelectionFormula += "   {Command_1.ActionDate} = " & "#" & dtFrom & "#"
                    ' strRecordSelectionFormula += "   {Command_1.ActionDate} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += "   {Command_1.ActionDate} <= " & "#" & dtTo & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += "   {Command_1.ActionDate} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += "   {Command_1.ActionDate} in " & "#" & dtFrom & "#" & "to #" & dtTo & "#"
                    'strRecordSelectionFormula += "   {Command_1.ActionDate} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If
                If dtFrom <> Nothing And dtTo <> Nothing Then
                    If DateDiff(DateInterval.Day, CType(dtFrom, Date), CType(dtTo, Date)) < 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("To date should be grater than From date ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        cpnlError.Visible = False
                    End If
                End If
            End If
            crvReport.EnableDrillDown = False
            crDocument.RecordSelectionFormula = strRecordSelectionFormula
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            clsReport.LogonInformation(crDocument)
            cpnlError.Visible = False
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

    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged

        'Try
        '    If ddlCompany.SelectedIndex <= 0 Then
        '        '  fill_Project(0)
        '        fill_employee(0, 0, 1)

        '    Else
        '        Dim intSelectedValue As Integer = CType(ddlCompany.SelectedItem.Value.Trim, Integer)
        '        '  fill_Project(intSelectedValue)
        '        fill_employee(intSelectedValue, 0, 2)

        '    End If

        'Catch ex As Exception
        '    Dim str As String = ex.Message.ToString
        'End Try



    End Sub
    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
        HttpContext.Current.Session("PropCallNumber") = 0
        HttpContext.Current.Session("PropTaskNumber") = 0
    End Sub
    'Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
    '    If Request("ip") = "tms2" Then
    '        Server.Transfer("TimeRegistration.aspx?ip=tms")
    '    ElseIf Request("ip") = "tms" Or Request("ip") = "tms4" Then
    '        Response.Redirect("../home.aspx", False)
    '    End If

    'End Sub

#End Region

    'Private Sub imgOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
    '    Try
    '        If Request("ip") = "tms4" Then
    '            Showreport(4)
    '        End If
    '    Catch ex As Exception
    '        Dim str As String = ex.Message.ToString
    '    End Try


    'End Sub
End Class
