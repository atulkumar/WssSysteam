'************************************************************************************************************
' Page                 : - CReports
' Purpose              : - it  will show the call detail Full report,Call summary2,CalltaskDetail,Calldetail
'                          report (one at a time)depends upon the value of query string passed.
' Date		    		   Author						Modification Date					Description
' 18/03/06				   Atul 					             					        Created
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
Partial Class Reports_CReports
    Inherits System.Web.UI.Page

#Region " Page Level Varaibles"

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
    Public mstrTaskNumber As String
    Public mstrCompanyID As String
    Dim strRecordSelectionFormula As String
    Shared mshCall As Short
#End Region

#Region " Page Load Event"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' CDDLCallNo.CDDLPopUpURL = "../Search/Common/PopSearch.aspx"
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

            'Put user code to initialize the page here
            Dim txthiddenImage = Request.Form("txthiddenImage")
            HIDSCRID.Value = Request.QueryString("ScrID")
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
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
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
            'dtFromDate.LeftPos = 500
            'dtFromDate.TopPos = 30
            'dtToDate.LeftPos = 550
            'dtToDate.TopPos = 30
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Function Show"

    Private Sub show()
        Try
            'cpnlError.Visible = False
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

            ElseIf Request("ip").ToString = "cs" Then
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
                    lblHead.Text = "CALL SUMMARY"
                    lblCallNo.Visible = False
                    ddlCallNo.Visible = False
                    lblCompany2.Visible = False
                    ddlCompany2.Visible = False
                    CpnlCallDetails.Visible = False
                    imgAttach.Visible = False
                    cpnlReport.Text = "CALL SUMMARY"
                    cpnlRS.Text = "CALL SUMMARY"

                    If fill_Project(HttpContext.Current.Session("PropCompanyID")) = False Then
                        lstError.Items.Clear()
                        lstError.Items.Add("No Sub Category Found for Selected Company ... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    If fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2) = False Then
                        lstError.Items.Clear()
                        lstError.Items.Add("No Call Data Found for Selected Company ... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                    fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                End If

                If IsPostBack Then
                    If dtFromDate.Text = Nothing And dtToDate.Text = Nothing And ddlCallTo.SelectedIndex = 0 And ddlCallFrom.SelectedIndex = 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Please Select Date or Call Number before Clicking on Search Button...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    If dtFromDate.Text <> Nothing Then
                        If dtFromDate.Text > Date.Now Then
                            lstError.Items.Clear()
                            lstError.Items.Add("From date cannot be greater than today Date ... ")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                    End If
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
                    lblHead.Text = "CALL SUMMARY"
                    lblCallNo.Visible = False
                    ddlCallNo.Visible = False
                    lblCompany2.Visible = False
                    ddlCompany2.Visible = False
                    CpnlCallDetails.Visible = False
                    imgAttach.Visible = False
                    cpnlReport.Text = "CALL SUMMARY"
                    cpnlRS.Text = "CALL SUMMARY"
                    If fill_Project(HttpContext.Current.Session("PropCompanyID")) = False Then
                        lstError.Items.Clear()
                        lstError.Items.Add("No Sub Category Found for Selected Company ... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    If fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2) = False Then
                        lstError.Items.Clear()
                        lstError.Items.Add("No Call Data Found for Selected Company ... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    fill_CallNo(2, HttpContext.Current.Session("PropCompanyID"), 0)
                    fill_CallNo(3, HttpContext.Current.Session("PropCompanyID"), 0)
                End If
                If IsPostBack Then
                    If dtFromDate.Text = Nothing And dtToDate.Text = Nothing And ddlCallTo.SelectedIndex = 0 And ddlCallFrom.SelectedIndex = 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Please Select Date or Call Number before Clicking on Search Button...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    If dtFromDate.Text <> Nothing Then
                        If dtFromDate.Text > Date.Now Then
                            lstError.Items.Clear()
                            lstError.Items.Add("From date cannot be greater than today Date ... ")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                    End If
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
                ddlCallFrom.Items.Insert(0, "--ALL--")
            End If
            If id = 3 Then
                ddlCallTo.Items.Clear()
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
            If Request("ip").ToString = "cs" Or Request("ip").ToString = "cs2" Then
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

    Private Function fill_Project(ByVal comapnyID As Integer) As Boolean
        Try
            If Request("ip").ToString = "cs" Or Request("ip").ToString = "cs2" Then
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
                        ddlProject.Items.Insert(0, "--ALL--")
                    End If
                Else
                    ddlProject.Items.Insert(0, "--ALL--")
                End If
            End If
            Return True
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
            Return False
        Finally
            objReports = Nothing
        End Try
    End Function

    Private Function fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer) As Boolean
        Dim dt As New DataTable
        Try
            objReports = New clsReportData
            dt = objReports.extractCallOwner(companyID, projectID, 1)
            ddlEmployee.DataSource = dt
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
            Return True
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
            objReports = Nothing
            Return False
        Finally
            dt.Dispose()
        End Try
    End Function

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
                    'crDocument = New crCallDetailFull
                Else
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallDetailFull.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crCallDetail
                End If
                'crDocument = New crCallTaskDetail
            ElseIf id = 2 Then
                If Request("ip").ToString = "cs2" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallSummaryAgg.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crCallSummaryAgg
                ElseIf Request("ip").ToString = "cs" Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crCallSummary.rpt")
                    crDocument.Load(Reportpath)
                    'crDocument = New crCallSummary
                End If

                crvReport.SeparatePages = True
            ElseIf id = 3 Then
                '  crDocument = New crCallSummaryAgg
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crCallTaskDetail.rpt")
                crDocument.Load(Reportpath)
                'crDocument = New crCallTaskDetail
                'crvReport.SeparatePages = True
            ElseIf id = 4 Then
                '  crDocument = New crCallSummaryAgg
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crCallDetail.rpt")
                crDocument.Load(Reportpath)
                'crDocument = New crCallDetail
                'crvReport.SeparatePages = True
            End If

            ' mstrCallNumber = CType(ddlCallNo.SelectedItem.Value.Trim, String)

            params = crDocument.DataDefinition.ParameterFields
            paramValues = New ParameterValues

            If id = 1 Or id = 3 Then

                'paramCallNo = params.Item("CallNo")
                'paramValues = paramCallNo.CurrentValues
                'paramDiscrete = New ParameterDiscreteValue



                Dim strRequestCall As String = Request("CallNo")
                Dim strCompanyID As String = Request("CompanyID")


                If ddlCompany2.SelectedIndex <= 0 Then
                    Exit Sub
                End If
                If ddlCallNo.Items.Count <= 1 Then
                    Exit Sub
                End If
                If ddlCallNo.SelectedIndex <= 0 And strRequestCall = Nothing Then
                    Exit Sub

                ElseIf ddlCallNo.SelectedIndex > 0 Or strRequestCall = Nothing Then
                    strRequestCall = Nothing
                    ' paramDiscrete.Value = ddlCallNo.SelectedItem.Value.Trim
                    crDocument.SetParameterValue("CallNo", ddlCallNo.SelectedItem.Value.Trim)
                    crDocument.SetParameterValue("CompanyID", ddlCompany2.SelectedItem.Value.Trim)
                    mstrCallNumber = ddlCallNo.SelectedItem.Text.Trim
                    mstrCompanyID = ddlCompany2.SelectedItem.Text.Trim

                Else
                    ' paramDiscrete.Value = strRequestCall
                    crDocument.SetParameterValue("CallNo", strRequestCall)
                    crDocument.SetParameterValue("CompanyID", strCompanyID)
                    mstrCallNumber = strRequestCall
                    mstrCompanyID = strCompanyID

                End If
                strRecordSelectionFormula = "{T040011.CM_NU9_Call_No_PK}=" & mstrCallNumber & " and  " & "{T040011.CM_NU9_Comp_Id_FK} = " & ddlCompany2.SelectedItem.Value.Trim

                'If strCompanyType <> "SCM" Then
                '    strRecordSelectionFormula = "{T040011.CM_NU9_Call_No_PK}=" & mstrCallNumber & " and  " & "{T040011.CM_NU9_Comp_Id_FK} = " & ddlCompany2.SelectedItem.Value.Trim & " and  " & "{T040031.AM_VC8_ActionType} <> " & Chr(34) & strActionType & Chr(34)

                'End If
                ' if 
                ' paramValues.Add(paramDiscrete)
                ' paramCallNo.ApplyCurrentValues(paramValues)
                ' paramDiscrete = Nothing

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

                'If strCompanyType <> "SCM" Then
                '    strRecordSelectionFormula = "{T040011.CM_NU9_Call_No_PK}=" & strRequestCall & " and  " & "{T040011.CM_NU9_Comp_Id_FK} = " & strCompanyID & " and  " & "{T040021.TM_NU9_Task_no_PK}= " & strRequestTask & " and  " & "{T040031.AM_VC8_ActionType} <> " & Chr(34) & strActionType & Chr(34)
                'Else

                'End If

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
                    strRecordSelectionFormula &= "{T040011.CM_NU9_Comp_Id_FK}=" & strCompany
                End If

                If ddlProject.Items.Count >= 1 And ddlProject.SelectedIndex <> 0 Then
                    Dim strProject As String = ddlProject.SelectedItem.Text.ToString.Trim
                    strRecordSelectionFormula &= " and " & "{Command_3.Name}=" & "'" & strProject & "'" '& "and"
                End If

                If Session("PropAdmin") = 1 Then

                    If ddlEmployee.SelectedIndex <> 0 And ddlEmployee.Items.Count > 1 Then
                        Dim strEmployee As String = ddlEmployee.SelectedItem.Value.Trim
                        strRecordSelectionFormula += " and " & "{T010011_CallOwner.CI_NU8_Address_Number}=" & strEmployee '& " and"
                    End If

                Else
                    strRecordSelectionFormula += " and " & "{T010011_CallOwner.CI_NU8_Address_Number}=" & Session("PropUserID") '& " and"

                End If


                Dim dtFrom As String
                Dim dtTo As String
                Dim intCallFrom, intCallTo As Integer
                intCallFrom = 0
                intCallTo = 0
                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text
                Dim dtToDate1 As DateTime



                If ddlCallFrom.SelectedIndex <> 0 And ddlCallFrom.Items.Count > 1 Then
                    intCallFrom = CType(ddlCallFrom.SelectedItem.Value, Integer)
                End If
                If ddlCallTo.SelectedIndex <> 0 And ddlCallTo.Items.Count > 1 Then
                    intCallTo = CType(ddlCallTo.SelectedItem.Text.Trim, Integer)
                End If

                If intCallFrom = 0 And intCallTo <> 0 Then
                    strRecordSelectionFormula &= " and " & "{T040011.CM_NU9_Call_No_PK} <= " & intCallTo '& " and"
                ElseIf intCallFrom <> 0 And intCallTo = 0 Then
                    strRecordSelectionFormula &= " and " & "{T040011.CM_NU9_Call_No_PK} >= " & intCallFrom '& " and"
                ElseIf intCallFrom <> 0 And intCallTo <> 0 Then
                    strRecordSelectionFormula &= " and " & "{T040011.CM_NU9_Call_No_PK} >= " & intCallFrom & " and" & "{T040011.CM_NU9_Call_No_PK} <= " & intCallTo '& " and"
                End If

                '''  No Default date Required Now
                'Dim i As Integer
                'If dtFromDate.CalendarDate = Nothing Then
                '    i = Today.DayOfWeek
                '    dtFromDate.CalendarDate = SetDateFormat(Today.AddDays(0 - i))
                '    'Else
                '    '    i = CType(dtFromDate.CalendarDate, DateTime).DayOfWeek
                '    '    Dim dt1 As DateTime = dtFromDate.CalendarDate
                '    'dtFromDate.CalendarDate = dt1.AddDays(0 - i)
                'End If


                'If dtToDate.CalendarDate = Nothing Then
                '    i = Today.DayOfWeek
                '    dtToDate.CalendarDate = SetDateFormat(Today.AddDays(6 - i))
                '    'Else
                '    '    i = CType(dtToDate.CalendarDate, DateTime).DayOfWeek
                '    '    Dim dt2 As DateTime = dtToDate.CalendarDate
                '    '    dtToDate.CalendarDate = dt2.AddDays(6 - i)
                'End If

                dtFrom = dtFromDate.Text
                dtTo = dtToDate.Text


                'If dtTo = Nothing And dtFrom = Nothing Then
                '    strRecordSelectionFormula += " {T040011.CM_DT8_Request_Date} <= " & "#" & DateAdd("d", 1, Today) & "#"
                'Else
                If dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " and " & "{T040011.CM_DT8_Request_Date} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
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


                crDocument.RecordSelectionFormula = strRecordSelectionFormula

                showHideContents(2)
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
            'Dim a As BoxObject
            'a = CType(crDocument.ReportDefinition.ReportObjects.Item("Box1"), BoxObject)
            'a.FillColor = Color.White
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

    'Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
    '    If Request("ip").ToString = "ctad" Then

    '        Server.Transfer("CReports.aspx?ip=ctd&CallNo=" & Request("CallNo") & "&CompanyID= " & Request("CompanyID"), False)
    '    Else
    '        Response.Redirect("../home.aspx", False)
    '    End If
    'End Sub


#End Region
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
