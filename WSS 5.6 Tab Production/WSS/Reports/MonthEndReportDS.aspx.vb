#Region "Reffered Assemblies"
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

#End Region

Partial Class Reports_MonthEndReportDS
    Inherits System.Web.UI.Page
#Region " Page Level Varaibles"
    Private crDocument As New ReportDocument
    Private objReports As clsReportData
    Public mstrCallNumber As String
    Public mstrTaskNumber As String
    Public mstrCompanyID As String
    Private dtFrom As String
    Private dtTo As String
#End Region
#Region " Page Load Event"
    Private Shared flgSearchClick As Boolean = False
  
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
            crvReport.ToolbarStyle.Width = New Unit("930px")
            'Put user code to initialize the page here
            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                dtToDate.Height = 14
                dtFromDate.Height = 14
                fill_company()
                fill_Suupcompany()
                fill_Project(HttpContext.Current.Session("PropCompanyID"))
                If Fill_CallStatus(HttpContext.Current.Session("PropCompanyID")) = False Then
                    chkStatus.Enabled = False
                End If
                Fill_CallCategory(HttpContext.Current.Session("PropCompanyID"))
                Fill_ActionOwner(HttpContext.Current.Session("PropCompanyID"))
                FillCallType(HttpContext.Current.Session("PropCompanyID")) 'added on 06-07-09
            Else
            End If

            If Not IsPostBack Then
            Else
                show()
            End If
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Logout"
                            LogoutWSS()
                            Exit Sub
                    End Select
                Catch ex As Exception
                    CreateLog("Reports_ProjectDetail", "Page_Load-50", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                End Try
            End If

        Catch ex As Exception
            CreateLog("MonthEndReport", "Page_Load", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region
#Region "Function Show"

    Private Sub show()
        Try
            crvReport.ReportSource = Nothing
            If Request("ip") = Nothing Then
                Response.Redirect("ReportsIndex.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")

                'chkStatus.Attributes.Add("OnClick", "return CheckStatus('" & chkLstStatus.ClientID & "','" & chkStatus.ClientID & "');")
            End If

            If Request("ip").ToString = "ME" Then


                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 950
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If

            End If

            'Response.Write("<head><title>" & "MONTH END REPORT" & "</title></head>")
            lblHead.Text = "Month End Report"
            cpnlReport.Text = "Month End Report"
            cpnlRS.Text = "Month End Report"
            If IsPostBack Then
                If chkLstStatus.SelectedIndex <> -1 Or chkStatus.Checked = True Then
                    'cpnlError.Visible = False
                    ShowReport(1)
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Please select Call Status...")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "show", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region
#Region "FillAllDropDown"
    Private Sub fill_company()
        Try
            If Request("ip").ToString = "ME" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCompany(2)
                ddlCompany.DataSource = dt
                ddlCompany.DataTextField = "Name"
                ddlCompany.DataValueField = "ID"
                ddlCompany.DataBind()
                'ddlCompany.Items.Insert(0, "--ALL--")
                If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                    ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
                End If
                If Session("PropCompanyType") <> "SCM" Then
                    ddlCompany.Enabled = False
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "fill_company", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objReports = Nothing
        End Try
    End Sub
    Private Sub fill_Suupcompany()
        Try
            If Request("ip").ToString = "ME" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCompany(2)
                If dt.Rows.Count > 0 Then
                    ddlSuppComp.DataSource = dt
                    ddlSuppComp.DataTextField = "Name"
                    ddlSuppComp.DataValueField = "ID"
                    ddlSuppComp.DataBind()
                    ViewState("SuupCOM") = dt
                    'ddlCompany.Items.Insert(0, "--ALL--")
                    If ddlSuppComp.Items(0).Text <> "--None--" Then
                        ddlSuppComp.Items.Insert(0, "--None--")
                    End If
                    If ddlSuppComp.Items(0).Text <> "--ALL--" Then
                        ddlSuppComp.Items.Insert(1, "--ALL--")
                    End If
                Else
                    ddlSuppComp.Items.Insert(0, "--ALL--")
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "fill_company", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objReports = Nothing
        End Try
    End Sub
    Private Function Fill_CallStatus(ByVal CompID As Integer) As Boolean
        Try
            If Request("ip").ToString = "ME" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCallStatus(CompID)
                If dt.Rows.Count > 0 Then
                    chkLstStatus.DataSource = dt
                    chkLstStatus.DataTextField = "CallStatus"
                    chkLstStatus.DataBind()
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "Fill_CallStatus", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            objReports = Nothing
        End Try
    End Function
    Private Sub fill_Project(ByVal CompanyID As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractProject(CompanyID)
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
            'cpnlError.Visible = False

        Catch ex As Exception
            CreateLog("MonthEndReport", "fill_Project", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objReports = Nothing 'extractCallStatus
        End Try
    End Sub
    Sub Fill_CallCategory(ByVal CompanyID As Integer)
        Try
            If Request("ip").ToString = "ME" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCallCategory(CompanyID)
                If dt.Rows.Count > 0 Then
                    ddlCategory.Items.Clear()
                    ddlCategory.DataSource = dt
                    ddlCategory.DataTextField = "CallCategory"
                    ddlCategory.DataBind()
                    If ddlCategory.Items(0).Text <> "--ALL--" Then
                        ddlCategory.Items.Insert(0, "--ALL--")
                    End If
                Else
                    ddlCategory.Items.Insert(0, "--ALL--")
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "Fill_CallCategory", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objReports = Nothing
        End Try
    End Sub
    Sub Fill_ActionOwner(ByVal CompanyID As Integer, Optional ByVal ProjectID As Integer = 0)
        Try
            If Request("ip").ToString = "ME" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractActionOwner(CompanyID, ProjectID)
                ddlActionOwner.Items.Clear()
                If dt.Rows.Count > 0 Then
                    ddlActionOwner.DataSource = dt
                    ddlActionOwner.DataTextField = "ActionOwner"
                    ddlActionOwner.DataValueField = "ActionOwnerID"
                    ddlActionOwner.DataBind()
                    If ddlActionOwner.Items(0).Text <> "--ALL--" Then
                        ddlActionOwner.Items.Insert(0, "--ALL--")
                    End If
                Else
                    ddlActionOwner.Items.Insert(0, "--ALL--")
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "Fill_ActionOwner", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objReports = Nothing
        End Try
    End Sub
    'Added by sachin on 6-7-09
    Private Sub FillCallType(ByVal intCompID As Integer)
        Try
            If Request("ip").ToString = "ME" Then
                Dim dtCallType As New DataTable
                objReports = New clsReportData
                dtCallType = objReports.extractCallType(intCompID)
                ddlCallType.Items.Clear()
                If dtCallType.Rows.Count > 0 Then
                    ddlCallType.DataSource = dtCallType
                    ddlCallType.DataTextField = "ID"
                    ddlCallType.DataValueField = "ID"
                    ddlCallType.DataBind()
                    If ddlCallType.Items(0).Text <> "--ALL--" Then
                        ddlCallType.Items.Insert(0, "--ALL--")
                    End If
                Else
                    ddlCallType.Items.Insert(0, "--ALL--")
                End If
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "FillCallType", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objReports = Nothing
        End Try
    End Sub
#End Region
#Region "Function ShowReport To Show the Report "
    Private Sub ShowReport(ByVal id As Integer)
        Try
            If flgSearchClick = True Then
                Dim strActionType As String = "Internal"
                Dim strCompanyType As String = Session("PropCompanyType")
                dtFrom = Request.Form("cpnlRS$dtFromDate$txtDate")
                dtTo = Request.Form("cpnlRS$dtToDate$txtDate")
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
                        lstError.Items.Add("Date From can not be greater than Current Date... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    Else
                        dtTo = Date.Now.ToShortDateString
                        lstError.Items.Clear()
                    End If
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
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
                If id = 1 Then
                    crvReport.Visible = True
                    FillReport()
                End If
                cpnlReport.State = CustomControls.Web.PanelState.Expanded
                crvReport.HasSearchButton = False
                crvReport.HasViewList = False
                crvReport.HasDrillUpButton = False
                crvReport.SeparatePages = True
                crvReport.EnableDrillDown = False
                crvReport.ReportSource = crDocument
                crvReport.DataBind()
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "ShowReport", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Sub FillReport()
        Try
            Dim strFinalSQL As String
            Dim dsME As New DSMonthEndReport
            Dim ds As New DataSet
            Dim strFilter As String = String.Empty
            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text
            If Session("PropAdmin") = "1" Then
                strFinalSQL = "SELECT  TM_NU9_Task_no_PK as TaskNo,TM_VC1000_Subtsk_Desc as TaskDesc,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC8_Call_Type AS CallType,CM_VC200_Work_Priority AS CallPriority,CN_VC20_Call_Status AS CallStatus,CM_VC100_Subject AS CallSubject,SupOwner.UM_VC50_UserID as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_FL8_Used_Hr as UsedHrs,AM_DT8_Action_Date as ActionDate,CustComp.CI_VC36_Name as CustComp FROM T040011,T040021,T040031,T010011 Company,T010011 CustComp,T060011 SupOwner where CM_NU9_CustID_FK=Company.CI_NU8_Address_Number and AM_VC8_Supp_Owner=SupOwner.UM_IN4_Address_No_FK and CM_NU9_Comp_Id_FK=CustComp.CI_NU8_Address_Number and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number) "
                '"SELECT  TM_NU9_Task_no_PK as TaskNo,TM_VC1000_Subtsk_Desc as TaskDesc,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC8_Call_Type AS CallType, CM_VC200_Work_Priority AS CallPriority,CN_VC20_Call_Status AS CallStatus, CM_VC100_Subject AS CallSubject,SupOwner.CI_VC36_name as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_FL8_Used_Hr as UsedHrs,AM_DT8_Action_Date as ActionDate,CustComp.CI_VC36_name as CustComp FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner,t010011 CustComp where  CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number  and  CustComp.CI_NU8_Address_Number=SupOwner.CI_IN4_Business_Relation  and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number)"
                'strFinalSQL = "SELECT  TM_NU9_Task_no_PK as TaskNo,TM_VC1000_Subtsk_Desc as TaskDesc,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC8_Call_Type AS CallType, CM_VC200_Work_Priority AS CallPriority,CN_VC20_Call_Status AS CallStatus, CM_VC100_Subject AS CallSubject,SupOwner.CI_VC36_name as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_FL8_Used_Hr as UsedHrs,AM_DT8_Action_Date as ActionDate,CustComp.CI_VC36_name as CustComp FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner,t010011 CustComp where  CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number  and  CustComp.CI_NU8_Address_Number=SupOwner.CI_IN4_Business_Relation  and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number)" 'OLD
            Else
                'strFinalSQL = "SELECT  TM_NU9_Task_no_PK as TaskNo,TM_VC1000_Subtsk_Desc as TaskDesc,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC8_Call_Type AS CallType, CM_VC200_Work_Priority AS CallPriority,CN_VC20_Call_Status AS CallStatus, CM_VC100_Subject AS CallSubject,SupOwner.CI_VC36_name as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_FL8_Used_Hr as UsedHrs,AM_DT8_Action_Date as ActionDate,CustComp.CI_VC36_name as CustComp FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner,t010011 CustComp where  CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number  and  CustComp.CI_NU8_Address_Number=SupOwner.CI_IN4_Business_Relation  and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number)" 'OLD
                strFinalSQL = "SELECT  TM_NU9_Task_no_PK as TaskNo,TM_VC1000_Subtsk_Desc as TaskDesc,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC8_Call_Type AS CallType,CM_VC200_Work_Priority AS CallPriority,CN_VC20_Call_Status AS CallStatus,CM_VC100_Subject AS CallSubject,SupOwner.UM_VC50_UserID as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_FL8_Used_Hr as UsedHrs,AM_DT8_Action_Date as ActionDate,CustComp.CI_VC36_Name as CustComp FROM T040011,T040021,T040031,T010011 Company,T010011 CustComp,T060011 SupOwner where CM_NU9_CustID_FK=Company.CI_NU8_Address_Number and AM_VC8_Supp_Owner=SupOwner.UM_IN4_Address_No_FK and CM_NU9_Comp_Id_FK=CustComp.CI_NU8_Address_Number and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number) "
            End If
            If ddlCompany.SelectedIndex <> -1 Then
                If ddlCompany.SelectedItem.Value.Trim <> "--ALL--" Then
                    strFilter += " and CM_NU9_Comp_Id_FK=" & ddlCompany.SelectedItem.Value.Trim & ""
                End If
            End If
            If ddlProject.SelectedIndex <> -1 Then
                If ddlProject.SelectedItem.Text.Trim <> "--ALL--" Then
                    strFilter += " and CM_nu9_project_id=" & ddlProject.SelectedItem.Value.Trim & ""
                End If
            End If
            'Added on 06-07-09
            If ddlCallType.SelectedIndex <> -1 Then
                If ddlCallType.SelectedItem.Text.Equals("--ALL--") = False And ddlCallType.SelectedItem.Text <> "--None--" Then
                    strFilter += " and CM_VC8_Call_Type =" & "'" & ddlCallType.SelectedItem.Value.Trim & "'"
                End If
            End If
            If ddlSuppComp.SelectedIndex <> -1 Then
                If ddlSuppComp.SelectedItem.Text.Equals("--ALL--") = False And ddlSuppComp.SelectedItem.Text <> "--None--" Then
                    strFilter += " and  (SELECT CI_IN4_Business_Relation FROM T010011 WHERE CI_NU8_Address_Number=T040031.AM_VC8_Supp_Owner and CI_IN4_Business_Relation <>'SCM' and CI_IN4_Business_Relation <>'CCMP')=" & ddlSuppComp.SelectedValue & ""
                End If
            End If
            If chkStatus.Checked = False Then
                If chkLstStatus.SelectedIndex <> -1 Then
                    Dim strStatus As String = String.Empty
                    Dim intCount As Integer = 0
                    For inti As Integer = 0 To chkLstStatus.Items.Count - 1
                        If chkLstStatus.Items(inti).Selected Then
                            If intCount = 0 Then
                                strStatus = "( " & "'" & chkLstStatus.Items(inti).Text & "'"
                                intCount += 1
                            Else
                                strStatus &= "," & "'" & chkLstStatus.Items(inti).Text & "'"
                            End If
                        End If
                    Next
                    strStatus &= ")"
                    strFilter += " and CN_VC20_Call_Status in " & strStatus
                End If
            End If
            If ddlActionOwner.SelectedIndex <> -1 Then
                If ddlActionOwner.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and AM_VC8_Supp_Owner='" & ddlActionOwner.SelectedItem.Value.Trim & "'"
                End If
            End If
            If ddlCategory.SelectedIndex <> -1 Then
                If ddlCategory.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and CM_VC8_Category='" & ddlCategory.SelectedItem.Text.Trim & "'"
                End If
            End If

            If dtFrom <> Nothing Then
                If IsDate(dtFrom) = True Then
                    strFilter += " and convert(datetime,convert(varchar,am_dt8_action_date,101),101) >='" & CDate(dtFrom) & "'"
                End If
            End If

            If dtTo <> Nothing Then
                If IsDate(dtTo) = True Then
                    strFilter += " and convert(datetime,convert(varchar,am_dt8_action_date,101),101) <='" & CDate(dtTo) & "'"
                End If
            End If
            If ddlActionTime.SelectedIndex <> -1 Then
                If ddlActionTime.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and AM_FL8_Used_Hr<>0"
                End If
            End If
            strFilter += " order by CM_VC200_Work_Priority asc,CM_NU9_Call_No_PK"
            strFinalSQL += strFilter
            SQL.Search("dtMonthEndRpt", "WSSReportsD", "ExtractCallNo", strFinalSQL, dsME, "jagmit", "sidhu")
            'cpnlError.Visible = False
            crDocument = New ReportDocument

            Dim Reportpath As String
            Reportpath = Server.MapPath("crMonthEndReport.rpt")
            crDocument.Load(Reportpath)

            crDocument.SetDataSource(dsME)
        Catch ex As Exception
            CreateLog("MonthEndReport", "FillReport", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region
#Region "Events "
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

    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Try
            flgSearchClick = False
            If ddlCompany.SelectedIndex <> -1 Then
                fill_Project(ddlCompany.SelectedItem.Value)
                Fill_CallCategory(ddlCompany.SelectedItem.Value)
                Fill_CallStatus(ddlCompany.SelectedItem.Value)
                Fill_ActionOwner(ddlCompany.SelectedItem.Value)
                FillCallType(ddlCompany.SelectedItem.Value) 'add on 06-07-09
                If ddlProject.Items.Count < 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("No SubCategory found for selected SubCategory...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    crvReport.Visible = False
                    Exit Sub
                End If
                If ddlCategory.Items.Count < 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("No Category found for selected company...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    crvReport.Visible = False
                    Exit Sub
                End If
                If ddlActionOwner.Items.Count < 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("No Action owner found for selected Company/SubCategory...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    crvReport.Visible = False
                    Exit Sub
                End If
                crvReport.Visible = False
                'cpnlError.Visible = False
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "ddlCompany_SelectedIndexChanged", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub imgOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        Try
            If ddlProject.Items.Count <= 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("No Project data found for selected company ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            flgSearchClick = True
            show()
        Catch ex As Exception
            CreateLog("MonthEndReport", "imgOK_Click", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub ddlProject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        Try
            flgSearchClick = False
            If ddlProject.SelectedIndex <> -1 Then
                If ddlProject.SelectedItem.Text <> "--ALL--" Then
                    Fill_ActionOwner(ddlCompany.SelectedItem.Value, ddlProject.SelectedItem.Value)
                Else
                    Fill_ActionOwner(ddlCompany.SelectedItem.Value)
                End If
                If ddlActionOwner.Items.Count < 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("No Action owner found for selected SubCategory...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    crvReport.Visible = False
                    Exit Sub
                End If
                crvReport.Visible = False
                'cpnlError.Visible = False
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "imgOK_Click", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region
    Private Sub chkStatus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStatus.CheckedChanged
        Try
            flgSearchClick = False
            If chkStatus.Checked = True Then
                chkLstStatus.Enabled = False
                crvReport.Visible = False
                'cpnlError.Visible = False
            Else
                chkLstStatus.Enabled = True
                crvReport.Visible = False
                'cpnlError.Visible = False
            End If
        Catch ex As Exception
            CreateLog("MonthEndReport", "chkStatus_CheckedChanged", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
