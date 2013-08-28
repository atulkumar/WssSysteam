Imports ION.Data
Imports System.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Reports_TimeRegistrationDS
    Inherits System.Web.UI.Page

    Private crDocument As New ReportDocument
    Private objReports As clsReportData
    Private objReportsDS As clsDSReports
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################


            Dim txthiddenImage = Request.Form("txthiddenImage")
            HIDSCRID.Value = Request.QueryString("ScrID")
            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If
            crvReport.ToolbarStyle.Width = New Unit("800px")
            'imgOK.Attributes.Add("OnClick", "ShowImg();")
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
            If Not IsPostBack Then
                fill_company()
                FillEmployee(HttpContext.Current.Session("PropCompanyID"))
            Else
                show()
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub

#Region "Function Show() "
    Private Sub show()
        Try
            Select Case Request("ip")
                Case Nothing
                    'Response.Redirect("ReportsIndex.aspx")
                Case "tms"
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
                    'Response.Write("<head><title>" & "TIME REGISTRATION" & "</title></head>")
                    cpnlReport.Text = "TIME REGISTRATION"
                    cpnlRS.Text = "TIME REGISTRATION"
                    If IsPostBack Then
                    End If

                    If Not IsPostBack Then
                        fill_company()
                        FillEmployee(HttpContext.Current.Session("PropCompanyID"))
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
                    End If
                    If Not IsPostBack Then
                        fill_company()
                        FillEmployee(Val(ddlCompany.SelectedValue))
                    End If
                    lblHead.Text = "TIME REGISTRATION"
                    Showreport(2)

                Case "tms3"
                    'Response.Write("<head><title>" & "TIME REGISTRATION" & "</title></head>")

                    cpnlReport.Text = "TIME REGISTRATION"
                    cpnlRS.Visible = False
                    cpnlRS.Text = "TIME REGISTRATION"
                    lblHead.Text = "TIME REGISTRATION"

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
                    End If
                    If Not IsPostBack Then
                        fill_company()
                        FillEmployee(HttpContext.Current.Session("PropCompanyID"))
                    End If
                    'Response.Write("<head><title>" & "DAILY REPORT" & "</title></head>")
                    cpnlReport.Text = "DAILY REPORT"
                    ' cpnlRS.Visible = False
                    cpnlRS.Text = "DAILY REPORT"
                    lblHead.Text = "Daily Report"
                    If IsPostBack Then
                        Showreport(4)
                    End If
                Case Else
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
            objReportsDS = New clsDSReports
            dt = objReportsDS.extractCompany(2)
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
    Private Sub FillEmployee(ByVal CompanyID As String)
        Try
            Dim dt As New DataTable
            objReportsDS = New clsDSReports
            dt = objReportsDS.extractTaskOwnerTR(companyID)
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
                ddlEmployee.Items.Clear()
                ddlEmployee.Items.Add("--ALL--")
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
            crvReport.ReportSource = Nothing
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("TimeRegReport1.rpt")
            crDocument.Load(Reportpath)
            'crDocument = New TimeRegReport1
            Dim dsTimeReg As New DataSet
            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
            If dtFrom = Nothing And dtTo = Nothing Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Date....")
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                Exit Sub
            ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                Dim From As String = "01/01/1932"
                dtFrom = CDate(From)
            ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                If CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
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
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
            Else
                lstError.Items.Clear()
            End If
            Dim strSQL As String
            If Val(ddlCompany.SelectedValue) = 0 Then
                If Val(ddlEmployee.SelectedValue) = 0 Then
                    strSQL = "select T040031.AM_VC8_Supp_Owner TaskOwnerID,Emp.UM_VC50_UserID TaskOwner, T040031.AM_NU9_Comp_ID_FK CompID,Comp.CI_VC36_Name CompName,AM_FL8_Used_Hr UsedHrs,convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) ActionDate From T040031,T010011 Comp,T060011 Emp where convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) between '" & dtFrom & "' and '" & dtTo & "' and T040031.AM_NU9_Comp_ID_FK=Comp.CI_NU8_Address_Number and T040031.AM_VC8_Supp_Owner=Emp.UM_IN4_Address_No_FK and AM_FL8_Used_Hr<>0  order by TaskOwner,CompName"
                Else
                    strSQL = "select T040031.AM_VC8_Supp_Owner TaskOwnerID,Emp.UM_VC50_UserID TaskOwner, T040031.AM_NU9_Comp_ID_FK CompID,Comp.CI_VC36_Name CompName,AM_FL8_Used_Hr UsedHrs,convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) ActionDate From T040031,T010011 Comp,T060011 Emp where convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) between '" & dtFrom & "' and '" & dtTo & "' and T040031.AM_NU9_Comp_ID_FK=Comp.CI_NU8_Address_Number and T040031.AM_VC8_Supp_Owner=Emp.UM_IN4_Address_No_FK and AM_FL8_Used_Hr<>0  and T040031.AM_VC8_Supp_Owner=" & Val(ddlEmployee.SelectedValue) & " order by TaskOwner,CompName"
                End If
            Else
                If Val(ddlEmployee.SelectedValue) = 0 Then
                    strSQL = "select T040031.AM_VC8_Supp_Owner TaskOwnerID,Emp.UM_VC50_UserID TaskOwner, T040031.AM_NU9_Comp_ID_FK CompID,Comp.CI_VC36_Name CompName,AM_FL8_Used_Hr UsedHrs,convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) ActionDate From T040031,T010011 Comp,T060011 Emp where convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) between '" & dtFrom & "' and '" & dtTo & "' and T040031.AM_NU9_Comp_ID_FK=Comp.CI_NU8_Address_Number and T040031.AM_VC8_Supp_Owner=Emp.UM_IN4_Address_No_FK and AM_FL8_Used_Hr<>0  and T040031.AM_NU9_Comp_ID_FK=" & Val(ddlCompany.SelectedValue) & " order by TaskOwner,CompName"
                Else
                    strSQL = "select T040031.AM_VC8_Supp_Owner TaskOwnerID,Emp.UM_VC50_UserID TaskOwner, T040031.AM_NU9_Comp_ID_FK CompID,Comp.CI_VC36_Name CompName,AM_FL8_Used_Hr UsedHrs,convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) ActionDate From T040031,T010011 Comp,T060011 Emp where convert(Datetime,(convert(varchar,AM_DT8_Action_Date,101))) between '" & dtFrom & "' and '" & dtTo & "' and T040031.AM_NU9_Comp_ID_FK=Comp.CI_NU8_Address_Number and T040031.AM_VC8_Supp_Owner=Emp.UM_IN4_Address_No_FK and AM_FL8_Used_Hr<>0  and T040031.AM_NU9_Comp_ID_FK=" & Val(ddlCompany.SelectedValue) & "   and T040031.AM_VC8_Supp_Owner=" & Val(ddlEmployee.SelectedValue) & " order by TaskOwner,CompName"
                End If

            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim dsWeekDays As New DataSet
            dsWeekDays.Tables.Add("TimeReg1")

            dsWeekDays.Tables(0).Columns.Add("CompName")
            dsWeekDays.Tables(0).Columns.Add("UserName")
            dsWeekDays.Tables(0).Columns.Add("SUN", System.Type.GetType("System.Double"))
            dsWeekDays.Tables(0).Columns.Add("MON", System.Type.GetType("System.Double"))
            dsWeekDays.Tables(0).Columns.Add("TUE", System.Type.GetType("System.Double"))
            dsWeekDays.Tables(0).Columns.Add("WED", System.Type.GetType("System.Double"))
            dsWeekDays.Tables(0).Columns.Add("THR", System.Type.GetType("System.Double"))
            dsWeekDays.Tables(0).Columns.Add("FRI", System.Type.GetType("System.Double"))
            dsWeekDays.Tables(0).Columns.Add("SAT", System.Type.GetType("System.Double"))
            Dim drWeedDays As DataRow
            If SQL.Search("TimeReg1", "", "", strSQL, dsTimeReg, "", "") = True Then
                Dim CompID As Integer = dsTimeReg.Tables("TimeReg1").Rows(0).Item("CompID")
                Dim UserID As Integer = dsTimeReg.Tables("TimeReg1").Rows(0).Item("TaskOwnerID")
                For intI As Integer = 0 To dsTimeReg.Tables("TimeReg1").Rows.Count - 1
                    Dim WeekDate As DateTime
                    WeekDate = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("Actiondate")
                    If dsWeekDays.Tables(0).Rows.Count < 1 Then
                        drWeedDays = dsWeekDays.Tables(0).NewRow
                        drWeedDays.Item("UserName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("TaskOwner")
                        drWeedDays.Item("CompName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("CompName")
                        If WeekDate.DayOfWeek = 0 Then
                            drWeedDays.Item("SUN") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 1 Then
                            drWeedDays.Item("MON") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 2 Then
                            drWeedDays.Item("TUE") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 3 Then
                            drWeedDays.Item("WED") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 4 Then
                            drWeedDays.Item("THR") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 5 Then
                            drWeedDays.Item("FRI") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 6 Then
                            drWeedDays.Item("SAT") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                        End If
                        dsWeekDays.Tables(0).Rows.Add(drWeedDays)
                    Else
                        Dim dtTemp As New DataTable
                        dtTemp = dsWeekDays.Tables(0).Copy
                        For intJ As Integer = 0 To dsWeekDays.Tables(0).Rows.Count - 1
                            If dsWeekDays.Tables(0).Rows(intJ).Item("UserName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("TaskOwner") And dsWeekDays.Tables(0).Rows(intJ).Item("CompName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("CompName") Then
                                'drWeedDays.Item("UserName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("TaskOwner")
                                '   drWeedDays.Item("CompName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("CompName")
                                If WeekDate.DayOfWeek = 0 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("SUN")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("SUN") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim SUN As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("SUN") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("SUN"))
                                        SUN += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("SUN") = SUN
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 1 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("MON")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("MON") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim MON As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("MON") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("MON"))
                                        MON += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("MON") = MON
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 2 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("TUE")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("TUE") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim TUE As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("TUE") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("TUE"))
                                        TUE += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("TUE") = TUE
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 3 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("WED")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("WED") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim WED As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("WED") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("WED"))
                                        WED += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("WED") = WED
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 4 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("THR")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("THR") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim THR As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("THR") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("THR"))
                                        THR += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("THR") = THR
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 5 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("FRI")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("FRI") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim FRI As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("FRI") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("FRI"))
                                        FRI += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("FRI") = FRI
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 6 Then
                                    If IsNothing(dsWeekDays.Tables(0).Rows(intJ).Item("SAT")) = True Then
                                        dsWeekDays.Tables(0).Rows(intJ).Item("SAT") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim SAT As Double = IIf(dsWeekDays.Tables(0).Rows(intJ).Item("SAT") Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intJ).Item("SAT"))
                                        SAT += dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                                        dsWeekDays.Tables(0).Rows(intJ).Item("SAT") = SAT
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                        If CompareDatasets(dtTemp, dsWeekDays.Tables(0)) = False Then
                        Else
                            drWeedDays = dsWeekDays.Tables(0).NewRow
                            drWeedDays.Item("UserName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("TaskOwner")
                            drWeedDays.Item("CompName") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("CompName")
                            If WeekDate.DayOfWeek = 0 Then
                                drWeedDays.Item("SUN") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 1 Then
                                drWeedDays.Item("MON") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 2 Then
                                drWeedDays.Item("TUE") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 3 Then
                                drWeedDays.Item("WED") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 4 Then
                                drWeedDays.Item("THR") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 5 Then
                                drWeedDays.Item("FRI") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 6 Then
                                drWeedDays.Item("SAT") = dsTimeReg.Tables("TimeReg1").Rows(intI).Item("UsedHrs")
                            End If
                            dsWeekDays.Tables(0).Rows.Add(drWeedDays)
                        End If
                    End If
                Next
                For intI As Integer = 0 To dsWeekDays.Tables(0).Columns.Count - 1
                    For intJ As Integer = 0 To dsWeekDays.Tables(0).Rows.Count - 1
                        If dsWeekDays.Tables(0).Rows(intJ).Item(intI) Is DBNull.Value Then
                            dsWeekDays.Tables(0).Rows(intJ).Item(intI) = 0
                        End If
                        'IIf(dsWeekDays.Tables(0).Rows(intj).Item(inti) Is DBNull.Value, 0, dsWeekDays.Tables(0).Rows(intj).Item(inti))
                    Next
                Next
            End If
            crDocument.SetDataSource(dsWeekDays)
            crvReport.EnableDrillDown = False
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            'clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub

#End Region

#Region "Function ShowReportCall()"

    Private Sub ShowreportCall()
        Try
            crvReport.ReportSource = Nothing
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("TimeRegReportDS.rpt")
            crDocument.Load(Reportpath)

            Dim dsTimeRegCall As New DataSet
            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
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

            If dtFrom = Nothing And dtTo = Nothing Then
                If Request("dtFrom") <> Nothing Then
                    dtFrom = Request("dtFrom")
                End If
                If Request("dtTo") <> Nothing Then
                    dtTo = Request("dtTo")
                Else
                    dtTo = Today
                End If
            End If
            Dim strSQL As String
            Dim strFilter As String = ""
            If ddlCompany.SelectedIndex <> -1 Then
                If ddlCompany.SelectedItem.Value.Trim <> "--ALL--" Then
                    strFilter += " and AM_NU9_Comp_ID_FK=" & ddlCompany.SelectedItem.Value.Trim & ""
                End If
            End If
            If ddlEmployee.SelectedIndex <> -1 Then
                If ddlEmployee.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and AM_VC8_Supp_Owner='" & ddlEmployee.SelectedItem.Value.Trim & "'"
                End If
            End If

            strSQL = "select AM_NU9_Comp_ID_FK as CompID,Comp.CI_VC36_Name CompName ,AM_VC8_Supp_Owner as UserID ,T010011.CI_VC36_Name UserName,AM_NU9_Call_Number as CallNo ,PR_NU9_Project_ID_Pk as ProjectID,PR_VC20_Name as ProjectName,convert(datetime,CONVERT(varchar, AM_DT8_Action_Date, 101),101) as ActionDate,AM_FL8_Used_Hr as usedhrs from T040031,T210011,T040011,T010011,T010011 Comp where CM_NU9_call_no_pk=AM_NU9_Call_Number and AM_NU9_Comp_ID_FK=CM_NU9_Comp_Id_FK and AM_NU9_Comp_ID_FK= PR_NU9_Comp_ID_FK and PR_NU9_Project_ID_Pk=CM_NU9_Project_ID and T040031.AM_VC8_Supp_Owner=T010011.CI_NU8_Address_Number and T040031.AM_NU9_Comp_ID_FK=Comp.CI_NU8_Address_Number and T040031.AM_FL8_Used_Hr<>0 and convert(datetime,CONVERT(varchar, AM_DT8_Action_Date, 101),101) Between  '" & dtFrom & "'and'" & dtTo & "'"

            strFilter += " Order By callNo "
            strSQL += strFilter
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim dsWeekDaysCall As New DataSet
            dsWeekDaysCall.Tables.Add("TimeRegCall")

            dsWeekDaysCall.Tables(0).Columns.Add("CompID", System.Type.GetType("System.Int32"))
            dsWeekDaysCall.Tables(0).Columns.Add("CompName")
            dsWeekDaysCall.Tables(0).Columns.Add("UserID", System.Type.GetType("System.Int32"))
            dsWeekDaysCall.Tables(0).Columns.Add("UserName")
            dsWeekDaysCall.Tables(0).Columns.Add("ProjectID", System.Type.GetType("System.Int32"))
            dsWeekDaysCall.Tables(0).Columns.Add("ProjectName")
            dsWeekDaysCall.Tables(0).Columns.Add("CallNo", System.Type.GetType("System.Int32"))
            dsWeekDaysCall.Tables(0).Columns.Add("SUN", System.Type.GetType("System.Double"))
            dsWeekDaysCall.Tables(0).Columns.Add("MON", System.Type.GetType("System.Double"))
            dsWeekDaysCall.Tables(0).Columns.Add("TUE", System.Type.GetType("System.Double"))
            dsWeekDaysCall.Tables(0).Columns.Add("WED", System.Type.GetType("System.Double"))
            dsWeekDaysCall.Tables(0).Columns.Add("THR", System.Type.GetType("System.Double"))
            dsWeekDaysCall.Tables(0).Columns.Add("FRI", System.Type.GetType("System.Double"))
            dsWeekDaysCall.Tables(0).Columns.Add("SAT", System.Type.GetType("System.Double"))
            Dim drWeedDays As DataRow

            If SQL.Search("TimeRegCall", "", "", strSQL, dsTimeRegCall, "", "") = True Then

                Dim CompanyID As Integer = dsTimeRegCall.Tables("TimeRegCall").Rows(0).Item("CompID")
                Dim UserID As Integer = dsTimeRegCall.Tables("TimeRegCall").Rows(0).Item("UserID")
                Dim ProjectID As Integer = dsTimeRegCall.Tables("TimeRegCall").Rows(0).Item("ProjectID")

                For intI As Integer = 0 To dsTimeRegCall.Tables("TimeRegCall").Rows.Count - 1
                    Dim WeekDate As DateTime
                    WeekDate = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("ActionDate")
                    If dsWeekDaysCall.Tables(0).Rows.Count < 1 Then
                        drWeedDays = dsWeekDaysCall.Tables(0).NewRow
                        drWeedDays.Item("CompID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CompID")
                        drWeedDays.Item("CompName") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CompName")
                        drWeedDays.Item("UserID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UserID")
                        drWeedDays.Item("UserName") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UserName")
                        drWeedDays.Item("ProjectID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("ProjectID")
                        drWeedDays.Item("ProjectName") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("ProjectName")
                        drWeedDays.Item("CallNo") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CallNo")
                        If WeekDate.DayOfWeek = 0 Then
                            drWeedDays.Item("SUN") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 1 Then
                            drWeedDays.Item("MON") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 2 Then
                            drWeedDays.Item("TUE") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 3 Then
                            drWeedDays.Item("WED") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 4 Then
                            drWeedDays.Item("THR") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 5 Then
                            drWeedDays.Item("FRI") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        ElseIf WeekDate.DayOfWeek = 6 Then
                            drWeedDays.Item("SAT") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                        End If
                        dsWeekDaysCall.Tables(0).Rows.Add(drWeedDays)
                    Else
                        Dim dtTempCall As New DataTable
                        dtTempCall = dsWeekDaysCall.Tables(0).Copy
                        For intJ As Integer = 0 To dsWeekDaysCall.Tables(0).Rows.Count - 1
                            If dsWeekDaysCall.Tables(0).Rows(intJ).Item("CompID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CompID") And dsWeekDaysCall.Tables(0).Rows(intJ).Item("UserID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UserID") And dsWeekDaysCall.Tables(0).Rows(intJ).Item("ProjectID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("ProjectID") And dsWeekDaysCall.Tables(0).Rows(intJ).Item("CallNo") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CallNo") Then

                                If WeekDate.DayOfWeek = 0 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("SUN")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("SUN") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim SUN As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("SUN") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("SUN"))
                                        SUN += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("SUN") = SUN
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 1 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("MON")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("MON") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim MON As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("MON") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("MON"))
                                        MON += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("MON") = MON
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 2 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("TUE")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("TUE") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim TUE As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("TUE") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("TUE"))
                                        TUE += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("TUE") = TUE
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 3 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("WED")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("WED") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim WED As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("WED") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("WED"))
                                        WED += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("WED") = WED
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 4 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("THR")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("THR") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim THR As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("THR") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("THR"))
                                        THR += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("THR") = THR
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 5 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("FRI")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("FRI") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim FRI As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("FRI") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("FRI"))
                                        FRI += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("FRI") = FRI
                                    End If
                                    Exit For
                                ElseIf WeekDate.DayOfWeek = 6 Then
                                    If IsNothing(dsWeekDaysCall.Tables(0).Rows(intJ).Item("SAT")) = True Then
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("SAT") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                    Else
                                        Dim SAT As Double = IIf(dsWeekDaysCall.Tables(0).Rows(intJ).Item("SAT") Is DBNull.Value, 0, dsWeekDaysCall.Tables(0).Rows(intJ).Item("SAT"))
                                        SAT += dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                                        dsWeekDaysCall.Tables(0).Rows(intJ).Item("SAT") = SAT
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                        If CompareDatasets(dtTempCall, dsWeekDaysCall.Tables(0)) = False Then
                        Else
                            drWeedDays = dsWeekDaysCall.Tables(0).NewRow
                            drWeedDays.Item("CompID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CompID")
                            drWeedDays.Item("CompName") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CompName")
                            drWeedDays.Item("UserID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UserID")
                            drWeedDays.Item("UserName") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UserName")
                            drWeedDays.Item("ProjectID") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("ProjectID")
                            drWeedDays.Item("ProjectName") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("ProjectName")
                            drWeedDays.Item("CallNo") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("CallNo")

                            If WeekDate.DayOfWeek = 0 Then
                                drWeedDays.Item("SUN") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 1 Then
                                drWeedDays.Item("MON") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 2 Then
                                drWeedDays.Item("TUE") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 3 Then
                                drWeedDays.Item("WED") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 4 Then
                                drWeedDays.Item("THR") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 5 Then
                                drWeedDays.Item("FRI") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            ElseIf WeekDate.DayOfWeek = 6 Then
                                drWeedDays.Item("SAT") = dsTimeRegCall.Tables("TimeRegCall").Rows(intI).Item("UsedHrs")
                            End If
                            dsWeekDaysCall.Tables(0).Rows.Add(drWeedDays)
                        End If
                    End If
                Next
                For intI As Integer = 0 To dsWeekDaysCall.Tables(0).Columns.Count - 1
                    For intJ As Integer = 0 To dsWeekDaysCall.Tables(0).Rows.Count - 1
                        If dsWeekDaysCall.Tables(0).Rows(intJ).Item(intI) Is DBNull.Value Then
                            dsWeekDaysCall.Tables(0).Rows(intJ).Item(intI) = 0
                        End If
                    Next
                Next
            End If
            'dsTimeRegCall = dsWeekDaysCall
            crDocument.SetDataSource(dsWeekDaysCall)
            crvReport.EnableDrillDown = False
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            'clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub

#End Region

#Region "Function ShowReportActions()"

    Private Sub ShowreportAction()
        Try
            crvReport.ReportSource = Nothing

            ' crDocument = New crTimeRegActionsDS
            Dim dsTimeRegAction As New DataSet
            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
            If dtFrom = Nothing And dtTo = Nothing Then
                If Request("dtFrom") <> Nothing Then
                    dtFrom = Request("dtFrom")
                End If
                If Request("dtTo") <> Nothing Then
                    dtTo = Request("dtTo")
                Else
                    dtTo = Today
                End If
            End If
            Dim strSQL As String
            Dim CompID As Integer = 8 'Request("CompID")
            Dim TaskOwnerID As Integer = 12 ' Request("ActionOwnerID")
            Dim CallNo As Integer = 1 ' Request("CallNo")
            If CompID <> Nothing And TaskOwnerID <> Nothing Then
                strSQL = "select AM_NU9_Call_Number CallNo,AM_NU9_Task_Number TaskNo,AM_NU9_Comp_ID_FK CompID,T010011.CI_VC36_Name CompName,T040021.TM_NU9_Project_ID ProjectID,T210011.PR_VC20_Name ProjectName,TM_VC1000_Subtsk_Desc TaskDesc,AM_VC_2000_Description ActionDesc,convert(datetime,CONVERT(varchar, AM_DT8_Action_Date, 101),101) as ActionDate,AM_FL8_Used_Hr UsedHrs From T040031,T040021,T210011,T010011 where T040021.TM_NU9_Call_No_FK=T040031.AM_NU9_Call_Number and T040031.AM_NU9_Comp_ID_FK=T010011.CI_NU8_Address_Number and T040021.TM_NU9_Project_ID=T210011.PR_NU9_Project_ID_Pk and T040021.TM_NU9_Comp_ID_FK=T210011.PR_NU9_Comp_ID_FK and T040031.AM_NU9_Comp_ID_FK=T210011.PR_NU9_Comp_ID_FK and T040021.TM_NU9_Task_no_PK=T040031.AM_NU9_Task_Number and T040031.AM_NU9_Call_Number=" & CallNo & " and T040031.AM_NU9_Comp_ID_FK=" & CompID & " and T040031.AM_VC8_Supp_Owner=" & TaskOwnerID & " and convert(datetime,CONVERT(varchar, AM_DT8_Action_Date, 101),101)between '" & dtFrom & "'and'" & dtTo & "' Order by TaskNo,ActionDate"

            End If

            SQL.DBConnection = SQL.GetConncetionString("ConnectionString")
            If SQL.Search("TimeRegActions", "", "", strSQL, dsTimeRegAction, "", "") = True Then
            End If
            dsTimeRegAction.Tables.Add("DateRange")
            dsTimeRegAction.Tables("DateRange").Columns.Add("FromDate", System.Type.GetType("System.DateTime"))
            dsTimeRegAction.Tables("DateRange").Columns.Add("ToDate", System.Type.GetType("System.DateTime"))
            Dim drDate As DataRow
            drDate = dsTimeRegAction.Tables("DateRange").NewRow
            drDate.Item("FromDate") = dtFrom
            drDate.Item("ToDate") = dtTo
            dsTimeRegAction.Tables("DateRange").Rows.Add(drDate)
            dsTimeRegAction.Tables("DateRange").AcceptChanges()

            crDocument.SetDataSource(dsTimeRegAction)
            crvReport.EnableDrillDown = False
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
    

#End Region

    Private Sub imgOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        Showreport(1)
        'ShowreportCall()
        'ShowreportAction()
    End Sub

    Private Function CompareDatasets(ByVal DtFirst As DataTable, ByVal DtSec As DataTable) As Boolean
        Try
            If DtFirst.Columns.Count = DtSec.Columns.Count Then
                If DtFirst.Rows.Count = DtSec.Rows.Count Then
                    For intI As Integer = 0 To DtFirst.Columns.Count - 1
                        For intJ As Integer = 0 To DtFirst.Rows.Count - 1
                            If IIf(DtFirst.Rows(intJ).Item(intI) Is DBNull.Value, 0, DtFirst.Rows(intJ).Item(intI)) = IIf(DtSec.Rows(intJ).Item(intI) Is DBNull.Value, 0, DtSec.Rows(intJ).Item(intI)) Then

                            Else
                                Return False
                            End If
                        Next
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception

        End Try
    End Function
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class



