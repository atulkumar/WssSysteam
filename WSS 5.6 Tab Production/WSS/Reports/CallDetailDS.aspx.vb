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
Partial Class Reports_CallDetailDS
    Inherits System.Web.UI.Page

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
    Private strRecordSelectionFormula As String
    Private intCallFrom, intCallTo As Integer
    Shared mshCall As Short
#End Region
#Region " Page Load Event"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
            crvReport.ToolbarStyle.Width = New Unit("800px")

            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            If Not IsPostBack Then
                fill_company()
                HIDSCRID.Value = Request.QueryString("ScrID")
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                txtCallNumber.Attributes.Add("onkeypress", "NumericOnly();")
            Else
                If txtCallNumber.Text.Trim <> "" Then
                    show()
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Please Enter Call Number...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region "Function Show"

    Private Sub show()
        Try
            If Request("ip") = Nothing Then
                Response.Redirect("ReportsIndex.aspx")
            End If
            If Request("ip").ToString = "cd" Then
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = HIDSCRID.Value '500
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
            End If
            If Request("ip").ToString = "cdf" Then
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = HIDSCRID.Value
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
            End If
            'Response.Write("<head><title>" & "CALL DETAIL" & "</title></head>")
            lblHead.Text = "Call Detail Report"
            cpnlReport.Text = "Call Detail Report"
            cpnlRS.Text = "Call Detail Report"
            If IsPostBack Then
                If Request.QueryString("ip") = "cdf" Then
                    ShowReport(2)
                End If
                If Request.QueryString("ip") = "cd" Then
                    ShowReport(1)
                End If

            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

    Private Sub fill_company()
        Try
            If Request.QueryString("ip").ToString.ToUpper = "CD" Or Request.QueryString("ip").ToString.ToUpper = "CDF" Then
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
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

#Region "Function ShowReport To Show the Report "


    Private Sub ShowReport(ByVal id As Integer)
        Try
            Dim strActionType As String = "Internal"
            Dim strCompanyType As String = Session("PropCompanyType")
            If id = 1 Then
                FillReportTDR()
            End If

            ''Call Detail Full
            If id = 2 Then
                FillReportCallDetailFull()
            End If
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False
            crvReport.SeparatePages = True
            crvReport.EnableDrillDown = False
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
    End Sub

    Sub FillReportTDR()
        Dim s As String
        Dim dsCD As New DSCallDetail
        Dim ds As New DataSet
        Dim strCompanyID As String
        Dim strCallNumber As String
        strCompanyID = ddlCompany.SelectedItem.Value.Trim
        strCallNumber = txtCallNumber.Text.Trim
        Dim htCols As New Hashtable
        htCols.Add("RequestDate", 1)
        htCols.Add("EstCallCloseDate", 2)
        htCols.Add("CallCloseDate", 2)
        htCols.Add("TaskDate", 1)
        htCols.Add("ActionDate", 2)
        If Session("PropAdmin") = "1" Then
            s = "select CM_NU9_Call_No_PK as CallNo,convert(varchar,CM_DT8_Request_Date) as RequestDate,CM_VC8_Call_Type as CallType,CM_VC2000_Call_Desc Description,Cowner.Ci_vc36_name as CallOwner,CN_VC20_Call_Status as CallStatus,convert(varchar,CM_DT8_Close_Date) as EstCallCloseDate,CM_NU9_Comp_Id_FK as CompanyId,company.CI_VC36_Name as Company,CM_VC50_Reference_Id as Reference,convert(varchar,CM_DT8_Call_Close_Date) CallCloseDate,CM_NU9_Project_ID,PR_VC20_Name as Project,(SELECT SUM(TM_FL8_Est_Hr) AS estimatedHRS FROM T040021 where TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK=" & strCompanyID & ") as EstHRS,(SELECT  SUM(AM_FL8_Used_Hr) AS UsedHRS FROM T040031 where AM_NU9_Call_Number =" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ") UsedHrs from t040011,t210011,t010011 as company,t010011 Cowner where Cowner.CI_NU8_Address_Number=t040011.CM_NU9_Call_Owner and Company.CI_NU8_Address_Number=t040011.CM_NU9_Comp_Id_FK and CM_NU9_Project_Id =t210011.PR_NU9_Project_ID_Pk and t040011.CM_NU9_Comp_Id_FK=t210011.PR_NU9_Comp_ID_FK and (CM_NU9_Call_No_PK=" & strCallNumber & " and t040011.CM_NU9_Comp_Id_FK=" & strCompanyID & ");select TM_NU9_Call_No_FK as CallNo,TM_NU9_Comp_ID_FK as CompanyId,TM_NU9_Task_no_PK as TaskNo,convert(varchar,TM_DT8_Task_Date) as TaskDate,TM_VC50_Deve_status as TaskStatus,TM_VC8_task_type as TaskType,SupportOwner.CI_vc36_name as SuppOwner,TaskOwner.CI_vc36_name as TaskAssignedTo,TM_VC1000_Subtsk_Desc as Description,tm_fl8_est_hr as TaskEstHRS from t040021,t010011 as SupportOwner,t010011 as TaskOwner where TM_NU9_Assign_by=SupportOwner.CI_NU8_Address_Number and TM_VC8_Supp_Owner=TaskOwner.CI_NU8_Address_Number and TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK =" & strCompanyID & ";select AM_NU9_Action_Number as ActionNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Call_Number as CallNo,AM_NU9_Comp_ID_FK as CompId,AM_VC8_ActionType as ActionType,t010011.CI_VC36_Name as ActionOwner,AM_VC_2000_Description as Description,AM_DT8_Action_Date as ActionDate,AM_FL8_Used_Hr as UsedHrs from T040031,t010011 where t010011.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Call_Number=" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ""
        Else
            s = "select CM_NU9_Call_No_PK as CallNo,convert(varchar,CM_DT8_Request_Date) as RequestDate,CM_VC8_Call_Type as CallType,CM_VC2000_Call_Desc Description,Cowner.Ci_vc36_name as CallOwner,CN_VC20_Call_Status as CallStatus,convert(varchar,CM_DT8_Close_Date) as EstCallCloseDate,CM_NU9_Comp_Id_FK as CompanyId,company.CI_VC36_Name as Company,CM_VC50_Reference_Id as Reference,convert(varchar,CM_DT8_Call_Close_Date) CallCloseDate,CM_NU9_Project_ID,PR_VC20_Name as Project,(SELECT SUM(TM_FL8_Est_Hr) AS estimatedHRS FROM T040021 where TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK=" & strCompanyID & ") as EstHRS,(SELECT  SUM(AM_FL8_Used_Hr) AS UsedHRS FROM T040031 where AM_NU9_Call_Number =" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ") UsedHrs from t040011,t210011,t010011 as company,t010011 Cowner where Cowner.CI_NU8_Address_Number=t040011.CM_NU9_Call_Owner and Company.CI_NU8_Address_Number=t040011.CM_NU9_Comp_Id_FK and CM_NU9_Project_Id =t210011.PR_NU9_Project_ID_Pk and t040011.CM_NU9_Comp_Id_FK=t210011.PR_NU9_Comp_ID_FK and (CM_NU9_Call_No_PK=" & strCallNumber & " and t040011.CM_NU9_Comp_Id_FK=" & strCompanyID & ");select TM_NU9_Call_No_FK as CallNo,TM_NU9_Comp_ID_FK as CompanyId,TM_NU9_Task_no_PK as TaskNo,convert(varchar,TM_DT8_Task_Date) as TaskDate,TM_VC50_Deve_status as TaskStatus,TM_VC8_task_type as TaskType,SupportOwner.CI_vc36_name as SuppOwner,TaskOwner.CI_vc36_name as TaskAssignedTo,TM_VC1000_Subtsk_Desc as Description,tm_fl8_est_hr as TaskEstHRS from t040021,t010011 as SupportOwner,t010011 as TaskOwner where TM_NU9_Assign_by=SupportOwner.CI_NU8_Address_Number and TM_VC8_Supp_Owner=TaskOwner.CI_NU8_Address_Number and TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK =" & strCompanyID & ";select AM_NU9_Action_Number as ActionNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Call_Number as CallNo,AM_NU9_Comp_ID_FK as CompId,AM_VC8_ActionType as ActionType,t010011.CI_VC36_Name as ActionOwner,AM_VC_2000_Description as Description,AM_DT8_Action_Date as ActionDate,AM_FL8_Used_Hr as UsedHrs from T040031,t010011 where t010011.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Call_Number=" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ""
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        SQL.Search("Call", "WSSReportsD", "ExtractCallNo", s, dsCD, "jagmit", "sidhu")
        SetDataTableDateFormat(dsCD.Tables(0), htCols)
        SetDataTableDateFormat(dsCD.Tables(1), htCols)
        SetDataTableDateFormat(dsCD.Tables(2), htCols)

        crDocument = New ReportDocument
        Dim Reportpath As String
        Reportpath = Server.MapPath("CallDetailReport.rpt")
        crDocument.Load(Reportpath)


        crDocument.SetDataSource(dsCD)
    End Sub

    Sub FillReportCallDetailFull()
        cpnlReport.Visible = True
        Dim s As String
        Dim dsCD As New DSCallDetail
        Dim ds As New DataSet
        Dim strCompanyID As String
        Dim strCallNumber As String
        strCompanyID = ddlCompany.SelectedItem.Value.Trim
        strCallNumber = txtCallNumber.Text.Trim

        Dim htCols As New Hashtable
        htCols.Add("RequestDate", 1)
        htCols.Add("EstCallCloseDate", 2)
        htCols.Add("CallCloseDate", 2)
        htCols.Add("TaskDate", 1)
        htCols.Add("ActionDate", 2)
        If Session("PropAdmin") = "1" Then
            s = "select CM_NU9_Call_No_PK as CallNo,convert(varchar,CM_DT8_Request_Date) as RequestDate,CM_VC8_Call_Type as CallType,CM_VC2000_Call_Desc Description,Cowner.Ci_vc36_name as CallOwner,CN_VC20_Call_Status as CallStatus,CM_DT8_Close_Date as EstCallCloseDate,CM_NU9_Comp_Id_FK as CompanyId,company.CI_VC36_Name as Company,CM_VC50_Reference_Id as Reference,CM_DT8_Call_Close_Date CallCloseDate,CM_NU9_Project_ID,PR_VC20_Name as Project,(SELECT SUM(TM_FL8_Est_Hr) AS estimatedHRS FROM T040021 where TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK=" & strCompanyID & ") as EstHRS,(SELECT  SUM(AM_FL8_Used_Hr) AS UsedHRS FROM T040031 where AM_NU9_Call_Number =" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ") UsedHrs from t040011,t210011,t010011 as company,t010011 Cowner where Cowner.CI_NU8_Address_Number=t040011.CM_NU9_Call_Owner and Company.CI_NU8_Address_Number=t040011.CM_NU9_Comp_Id_FK and CM_NU9_Project_Id =t210011.PR_NU9_Project_ID_Pk and t040011.CM_NU9_Comp_Id_FK=t210011.PR_NU9_Comp_ID_FK and (CM_NU9_Call_No_PK=" & strCallNumber & " and t040011.CM_NU9_Comp_Id_FK=" & strCompanyID & ");select TM_NU9_Call_No_FK as CallNo,TM_NU9_Comp_ID_FK as CompanyId,TM_NU9_Task_no_PK as TaskNo,TM_DT8_Task_Date as TaskDate,TM_VC50_Deve_status as TaskStatus,TM_VC8_task_type as TaskType,SupportOwner.CI_vc36_name as SuppOwner,TaskOwner.CI_vc36_name as TaskAssignedTo,TM_VC1000_Subtsk_Desc as Description,tm_fl8_est_hr as TaskEstHRS from t040021,t010011 as SupportOwner,t010011 as TaskOwner where TM_NU9_Assign_by=SupportOwner.CI_NU8_Address_Number and TM_VC8_Supp_Owner=TaskOwner.CI_NU8_Address_Number and TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK =" & strCompanyID & ";select AM_NU9_Action_Number as ActionNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Call_Number as CallNo,AM_NU9_Comp_ID_FK as CompId,AM_VC8_ActionType as ActionType,t010011.CI_VC36_Name as ActionOwner,AM_VC_2000_Description as Description,AM_DT8_Action_Date as ActionDate,AM_FL8_Used_Hr as UsedHrs from T040031,t010011 where t010011.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Call_Number=" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ";SELECT CM_NU9_CompId_Fk as CompId, CM_NU9_Call_Number as CallNo, CM_NU9_Task_Number as TaskNo, CM_NU9_Action_Number as ActionNo,CM_VC2_Flag as Flag,cm_nu9_ab_number, CM_VC256_Comments as CallComments,CI_VC36_Name as CommentsBy, CI_NU8_Address_Number as AddressNo FROM   T040061,t010011 where cm_nu9_ab_number=CI_NU8_Address_Number and CM_NU9_CompId_Fk=" & ddlCompany.SelectedItem.Value & " and CM_NU9_Call_Number=" & txtCallNumber.Text.Trim & " and CM_VC2_Flag='C';SELECT T040051.VH_NU9_CompId_Fk as CompanyId, T040051.VH_NU9_Call_Number as CallNo, T040051.VH_NU9_Task_Number as TaskNo,T040051.VH_NU9_Action_Number as ActionNo,T040051.VH_IN4_Level as Level, T040051.VH_NU9_Address_Book_Number AS Owner,T040051.VH_VC255_File_Name as FileName, T040051.VH_VC255_File_Path as FilePath,T010011.CI_NU8_Address_Number as AddressNo,T010011.CI_VC36_Name FROM  T040051 INNER JOIN T010011 ON T040051.VH_NU9_Address_Book_Number = T010011.CI_NU8_Address_Number and T040051.VH_NU9_CompId_Fk =" & ddlCompany.SelectedItem.Value & " and T040051.VH_NU9_Call_Number=" & txtCallNumber.Text & " and T040051.VH_IN4_Level=1 and T040051.VH_VC4_Active_Status='ENB';SELECT CM_NU9_CompId_Fk as CompId, CM_NU9_Call_Number as CallNo, CM_NU9_Task_Number as TaskNo, CM_NU9_Action_Number as ActionNo,CM_VC2_Flag as Flag,cm_nu9_ab_number, CM_VC256_Comments as TaskComments,CI_VC36_Name as CommentsBy, CI_NU8_Address_Number as AddressNo FROM   T040061,t010011 where cm_nu9_ab_number=CI_NU8_Address_Number and CM_NU9_CompId_Fk=" & ddlCompany.SelectedItem.Value & " and CM_NU9_Call_Number=" & txtCallNumber.Text.Trim & " and CM_VC2_Flag='T';SELECT T040051.VH_NU9_CompId_Fk as CompanyId, T040051.VH_NU9_Call_Number as CallNo, T040051.VH_NU9_Task_Number as TaskNo,T040051.VH_NU9_Action_Number as ActionNo,T040051.VH_IN4_Level as Level, T040051.VH_NU9_Address_Book_Number AS Owner,T040051.VH_VC255_File_Name as FileName, T040051.VH_VC255_File_Path as FilePath,T010011.CI_NU8_Address_Number as AddressNo,T010011.CI_VC36_Name FROM  T040051 INNER JOIN T010011 ON T040051.VH_NU9_Address_Book_Number = T010011.CI_NU8_Address_Number and T040051.VH_NU9_CompId_Fk =" & ddlCompany.SelectedItem.Value & " and T040051.VH_NU9_Call_Number=" & txtCallNumber.Text & " and T040051.VH_IN4_Level=2 and T040051.VH_VC4_Active_Status='ENB';SELECT CM_NU9_CompId_Fk as CompId, CM_NU9_Call_Number as CallNo, CM_NU9_Task_Number as TaskNo, CM_NU9_Action_Number as ActionNo,CM_VC2_Flag as Flag,cm_nu9_ab_number, CM_VC256_Comments as ActionComments,CI_VC36_Name as CommentsBy, CI_NU8_Address_Number as AddressNo FROM   T040061,t010011 where cm_nu9_ab_number=CI_NU8_Address_Number and CM_NU9_CompId_Fk=" & ddlCompany.SelectedItem.Value & " and CM_NU9_Call_Number=" & txtCallNumber.Text.Trim & " and CM_VC2_Flag='A';SELECT T040051.VH_NU9_CompId_Fk as CompanyId, T040051.VH_NU9_Call_Number as CallNo, T040051.VH_NU9_Task_Number as TaskNo,T040051.VH_NU9_Action_Number as ActionNo,T040051.VH_IN4_Level as Level, T040051.VH_NU9_Address_Book_Number AS Owner,T040051.VH_VC255_File_Name as FileName, T040051.VH_VC255_File_Path as FilePath,T010011.CI_NU8_Address_Number as AddressNo,T010011.CI_VC36_Name FROM  T040051 INNER JOIN T010011 ON T040051.VH_NU9_Address_Book_Number = T010011.CI_NU8_Address_Number and T040051.VH_NU9_CompId_Fk =" & ddlCompany.SelectedItem.Value & " and T040051.VH_NU9_Call_Number=" & txtCallNumber.Text & " and T040051.VH_IN4_Level=3 and T040051.VH_VC4_Active_Status='ENB'"
        Else
            s = "select CM_NU9_Call_No_PK as CallNo,CM_DT8_Request_Date as RequestDate,CM_VC8_Call_Type as CallType,CM_VC2000_Call_Desc Description,Cowner.Ci_vc36_name as CallOwner,CN_VC20_Call_Status as CallStatus,CM_DT8_Close_Date as EstCallCloseDate,CM_NU9_Comp_Id_FK as CompanyId,company.CI_VC36_Name as Company,CM_VC50_Reference_Id as Reference,CM_DT8_Call_Close_Date CallCloseDate,CM_NU9_Project_ID,PR_VC20_Name as Project,(SELECT SUM(TM_FL8_Est_Hr) AS estimatedHRS FROM T040021 where TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK=" & strCompanyID & ") as EstHRS,(SELECT  SUM(AM_FL8_Used_Hr) AS UsedHRS FROM T040031 where AM_NU9_Call_Number =" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ") UsedHrs from t040011,t210011,t010011 as company,t010011 Cowner where Cowner.CI_NU8_Address_Number=t040011.CM_NU9_Call_Owner and Company.CI_NU8_Address_Number=t040011.CM_NU9_Comp_Id_FK and CM_NU9_Project_Id =t210011.PR_NU9_Project_ID_Pk and t040011.CM_NU9_Comp_Id_FK=t210011.PR_NU9_Comp_ID_FK and (CM_NU9_Call_No_PK=" & strCallNumber & " and t040011.CM_NU9_Comp_Id_FK=" & strCompanyID & ");select TM_NU9_Call_No_FK as CallNo,TM_NU9_Comp_ID_FK as CompanyId,TM_NU9_Task_no_PK as TaskNo,TM_DT8_Task_Date as TaskDate,TM_VC50_Deve_status as TaskStatus,TM_VC8_task_type as TaskType,SupportOwner.CI_vc36_name as SuppOwner,TaskOwner.CI_vc36_name as TaskAssignedTo,TM_VC1000_Subtsk_Desc as Description,tm_fl8_est_hr as TaskEstHRS from t040021,t010011 as SupportOwner,t010011 as TaskOwner where TM_NU9_Assign_by=SupportOwner.CI_NU8_Address_Number and TM_VC8_Supp_Owner=TaskOwner.CI_NU8_Address_Number and TM_NU9_Call_No_FK=" & strCallNumber & " and TM_NU9_Comp_ID_FK =" & strCompanyID & ";select AM_NU9_Action_Number as ActionNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Call_Number as CallNo,AM_NU9_Comp_ID_FK as CompId,AM_VC8_ActionType as ActionType,t010011.CI_VC36_Name as ActionOwner,AM_VC_2000_Description as Description,AM_DT8_Action_Date as ActionDate,AM_FL8_Used_Hr as UsedHrs from T040031,t010011 where t010011.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Call_Number=" & strCallNumber & " and AM_NU9_Comp_ID_FK=" & strCompanyID & ";SELECT CM_NU9_CompId_Fk as CompId, CM_NU9_Call_Number as CallNo, CM_NU9_Task_Number as TaskNo, CM_NU9_Action_Number as ActionNo,CM_VC2_Flag as Flag,cm_nu9_ab_number, CM_VC256_Comments as CallComments,CI_VC36_Name as CommentsBy, CI_NU8_Address_Number as AddressNo FROM   T040061,t010011 where cm_nu9_ab_number=CI_NU8_Address_Number and CM_NU9_CompId_Fk=" & ddlCompany.SelectedItem.Value & " and CM_NU9_Call_Number=" & txtCallNumber.Text.Trim & " and CM_VC2_Flag='C';SELECT T040051.VH_NU9_CompId_Fk as CompanyId, T040051.VH_NU9_Call_Number as CallNo, T040051.VH_NU9_Task_Number as TaskNo,T040051.VH_NU9_Action_Number as ActionNo,T040051.VH_IN4_Level as Level, T040051.VH_NU9_Address_Book_Number AS Owner,T040051.VH_VC255_File_Name as FileName, T040051.VH_VC255_File_Path as FilePath,T010011.CI_NU8_Address_Number as AddressNo,T010011.CI_VC36_Name FROM  T040051 INNER JOIN T010011 ON T040051.VH_NU9_Address_Book_Number = T010011.CI_NU8_Address_Number and T040051.VH_NU9_CompId_Fk =" & ddlCompany.SelectedItem.Value & " and T040051.VH_NU9_Call_Number=" & txtCallNumber.Text & " and T040051.VH_IN4_Level=1 and T040051.VH_VC4_Active_Status='ENB';SELECT CM_NU9_CompId_Fk as CompId, CM_NU9_Call_Number as CallNo, CM_NU9_Task_Number as TaskNo, CM_NU9_Action_Number as ActionNo,CM_VC2_Flag as Flag,cm_nu9_ab_number, CM_VC256_Comments as TaskComments,CI_VC36_Name as CommentsBy, CI_NU8_Address_Number as AddressNo FROM   T040061,t010011 where cm_nu9_ab_number=CI_NU8_Address_Number and CM_NU9_CompId_Fk=" & ddlCompany.SelectedItem.Value & " and CM_NU9_Call_Number=" & txtCallNumber.Text.Trim & " and CM_VC2_Flag='T';SELECT T040051.VH_NU9_CompId_Fk as CompanyId, T040051.VH_NU9_Call_Number as CallNo, T040051.VH_NU9_Task_Number as TaskNo,T040051.VH_NU9_Action_Number as ActionNo,T040051.VH_IN4_Level as Level, T040051.VH_NU9_Address_Book_Number AS Owner,T040051.VH_VC255_File_Name as FileName, T040051.VH_VC255_File_Path as FilePath,T010011.CI_NU8_Address_Number as AddressNo,T010011.CI_VC36_Name FROM  T040051 INNER JOIN T010011 ON T040051.VH_NU9_Address_Book_Number = T010011.CI_NU8_Address_Number and T040051.VH_NU9_CompId_Fk =" & ddlCompany.SelectedItem.Value & " and T040051.VH_NU9_Call_Number=" & txtCallNumber.Text & " and T040051.VH_IN4_Level=2 and T040051.VH_VC4_Active_Status='ENB';SELECT CM_NU9_CompId_Fk as CompId, CM_NU9_Call_Number as CallNo, CM_NU9_Task_Number as TaskNo, CM_NU9_Action_Number as ActionNo,CM_VC2_Flag as Flag,cm_nu9_ab_number, CM_VC256_Comments as ActionComments,CI_VC36_Name as CommentsBy, CI_NU8_Address_Number as AddressNo FROM   T040061,t010011 where cm_nu9_ab_number=CI_NU8_Address_Number and CM_NU9_CompId_Fk=" & ddlCompany.SelectedItem.Value & " and CM_NU9_Call_Number=" & txtCallNumber.Text.Trim & " and CM_VC2_Flag='A';SELECT T040051.VH_NU9_CompId_Fk as CompanyId, T040051.VH_NU9_Call_Number as CallNo, T040051.VH_NU9_Task_Number as TaskNo,T040051.VH_NU9_Action_Number as ActionNo,T040051.VH_IN4_Level as Level, T040051.VH_NU9_Address_Book_Number AS Owner,T040051.VH_VC255_File_Name as FileName, T040051.VH_VC255_File_Path as FilePath,T010011.CI_NU8_Address_Number as AddressNo,T010011.CI_VC36_Name FROM  T040051 INNER JOIN T010011 ON T040051.VH_NU9_Address_Book_Number = T010011.CI_NU8_Address_Number and T040051.VH_NU9_CompId_Fk =" & ddlCompany.SelectedItem.Value & " and T040051.VH_NU9_Call_Number=" & txtCallNumber.Text & " and T040051.VH_IN4_Level=3 and T040051.VH_VC4_Active_Status='ENB'"
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        SQL.Search("Call", "WSSReportsD", "ExtractCallNo", s, dsCD, "jagmit", "sidhu")
        SetDataTableDateFormat(dsCD.Tables(0), htCols)
        SetDataTableDateFormat(dsCD.Tables(1), htCols)
        SetDataTableDateFormat(dsCD.Tables(2), htCols)

        crDocument = New ReportDocument
        Dim Reportpath As String
        Reportpath = Server.MapPath("CallFullDetailDS.rpt")
        crDocument.Load(Reportpath)

        crDocument.SetDataSource(dsCD)
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
