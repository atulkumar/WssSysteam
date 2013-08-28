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
Partial Class Reports_ReportsPages_KnowledgeDbRptDS
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
    Dim intCallFrom, intCallTo As Integer
    Dim flgCompanyChanged As Boolean = False
    Shared mshCall As Short
    Dim dtFrom As String
    Dim dtTo As String
#End Region
#Region " Page Load Event"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try


            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            crvReport.ToolbarStyle.Width = New Unit("900px")

            'Put user code to initialize the page here
            Dim txthiddenImage = Request.Form("txthiddenImage")
            HIDSCRID.Value = Request.QueryString("ScrID")
            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                Dim flgError As Boolean = False
                lstError.Items.Clear()
                fill_company()
                fill_Project(HttpContext.Current.Session("PropCompanyID"))
                Fill_CallCategory(HttpContext.Current.Session("PropCompanyID"))
                Fill_TaskType(HttpContext.Current.Session("PropCompanyID"))

             
            Else
                show()
            End If
        Catch ex As Exception
            CreateLog("KnowledgeDbRpt", "Page_Load", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
#End Region
#Region "Function Show"

    Private Sub show()
        Try
            If Request("ip") = Nothing Then
                Response.Redirect("ReportsIndex.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
            End If

            If Request("ip").ToString = "KDR" Then
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 955
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
            End If

            Response.Write("<head><title>" & "KNOWLEDGE DATABASE REPORT" & "</title></head>")
            lblHead.Text = "Knowledge Database Report"
            cpnlReport.Text = "Knowledge Database Report"
            cpnlRS.Text = "Knowledge Database Report"
            If IsPostBack Then
                ShowReport(1)
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region
#Region "FillAllDropDown"

    Private Sub fill_company()
        Try
            If Request("ip").ToString = "KDR" Then
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
    Private Function fill_Project(ByVal CompanyID As Integer) As Boolean
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractProject(CompanyID)
            If dt.Rows.Count > 0 Then
                ddlProject.DataSource = dt
                ddlProject.DataTextField = "Name"
                ddlProject.DataValueField = "ID"
                ddlProject.DataBind()
            End If

            If ddlProject.Items.Count > 0 Then
                If ddlProject.Items(0).Text <> "--ALL--" Then
                    ddlProject.Items.Insert(0, "--ALL--")
                End If
            Else
                ddlProject.Items.Insert(0, "--ALL--")
            End If

            'cpnlError.Visible = False
            Return True

        Catch ex As Exception
            CreateLog("KnowledgeDbRpt", "fill_Project", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False

        Finally
            objReports = Nothing 'extractCallStatus
        End Try
    End Function
    Private Function Fill_CallCategory(ByVal CompanyID As Integer) As Boolean
        Try
            If Request("ip").ToString = "KDR" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractCategoryCode(CompanyID)
                If dt.Rows.Count > 0 Then
                    ddlCatCodeNew.Items.Clear()
                    ddlCatCodeNew.DataSource = dt
                    ddlCatCodeNew.DataTextField = "ID"
                    ddlCatCodeNew.DataBind()
                    If ddlCatCodeNew.Items(0).Text <> "--ALL--" Then
                        ddlCatCodeNew.Items.Insert(0, "--ALL--")
                    End If
                Else
                    ddlCatCodeNew.Items.Insert(0, "--ALL--")
                End If


                Return True
            End If
        Catch ex As Exception
            CreateLog("KnowledgeDbRpt", "Fill_CallCategory", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False

        Finally
            objReports = Nothing
        End Try
    End Function
    Private Function Fill_TaskType(ByVal CompanyID As Integer) As Boolean
        Try
            If Request("ip").ToString = "KDR" Then
                Dim dt As New DataTable
                objReports = New clsReportData
                dt = objReports.extractTaskType(CompanyID)
                If dt.Rows.Count > 0 Then
                    ddlTaskType.Items.Clear()
                    ddlTaskType.DataSource = dt
                    ddlTaskType.DataTextField = "ID"
                    ddlTaskType.DataValueField = "ID"
                    ddlTaskType.DataBind()
                    Dim lstItem As New ListItem("SOLUTION", "SOLUTION")
                    If ddlTaskType.Items.Contains(lstItem) Then
                        ddlTaskType.SelectedValue = lstItem.Value
                    End If
                    If ddlTaskType.Items(0).Text <> "--ALL--" Then
                        ddlTaskType.Items.Insert(0, "--ALL--")
                    End If
                Else
                    ddlTaskType.Items.Insert(0, "--ALL--")
                End If

            End If
            Return True
        Catch ex As Exception
            CreateLog("KnowledgeDbRpt", "Fill_TaskType", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            objReports = Nothing
        End Try
    End Function
#End Region
#Region "Function ShowReport To Show the Report "

    Private Sub ShowReport(ByVal id As Integer)
        Try
            Dim strActionType As String = "Internal"
            Dim strCompanyType As String = Session("PropCompanyType")
            If id = 1 Then
                crvReport.Visible = True
                FillReport()

            End If

            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            'If intPageFlag = 1 Then
            '    crvReport.SeparatePages = True
            'ElseIf intPageFlag = 2 Then
            '    crvReport.SeparatePages = False
            'End If
            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False
            crvReport.SeparatePages = True
            crvReport.EnableDrillDown = False
            'clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Sub FillReport()
        Try
            Dim s As String

            Dim dsKDR As New DSKnowledgeDbRpt
            Dim ds As New DataSet
            Dim strFilter As String = ""

            Dim htCols As New Hashtable
            htCols.Add("ActionDate", 1)

            If Session("PropAdmin") = "1" Then
                s = "SELECT  CM_VC2000_Call_Desc as CallDescription,AM_VC_2000_Description as ActionText,TM_NU9_Task_no_PK as TaskNo,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC100_Subject AS CallSubject,SupOwner.CI_VC36_name as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_DT8_Action_Date as ActionDate FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner where CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number  and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number)"
            Else
                s = "SELECT  CM_VC2000_Call_Desc as CallDescription,AM_VC_2000_Description as ActionText,TM_NU9_Task_no_PK as TaskNo,CM_nu9_project_id as ProjectID,Company.CI_VC36_name as CompName,CM_NU9_Call_No_PK as CallNo, CM_NU9_Comp_Id_FK AS CompId, CM_VC100_Subject AS CallSubject,SupOwner.CI_VC36_name as ActionOwner,AM_NU9_Action_Number as ActionNo,AM_VC8_Supp_Owner as ActOwnerID,AM_DT8_Action_Date as ActionDate FROM T040011,t010011 Company,t040021,t040031,t010011 SupOwner where CM_NU9_Comp_Id_FK=Company.cI_nu8_address_Number  and (CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK) and AM_VC8_Supp_Owner=SupOwner.CI_nu8_address_number and (TM_NU9_Call_No_FK=AM_NU9_Call_Number and TM_NU9_Comp_ID_FK=AM_NU9_Comp_ID_FK and TM_NU9_Task_no_PK=AM_NU9_Task_Number)"
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
            If ddlTaskType.SelectedIndex <> -1 Then
                If ddlTaskType.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and Tm_vc8_Task_type='" & ddlTaskType.SelectedItem.Text.Trim & "'"
                End If
            End If

            If ddlCatCodeNew.SelectedIndex <> -1 Then
                If ddlCatCodeNew.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and CM_VC8_Call_Category='" & ddlCatCodeNew.SelectedItem.Text.Trim & "'"
                End If
            End If



            strFilter += " order by CM_VC200_Work_Priority asc,CM_NU9_Call_No_PK"
            s += strFilter
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("dtKnowledgeDbRpt", "WSSReportsD", "ExtractCallNo", s, dsKDR, "jagmit", "sidhu")
            SetDataTableDateFormat(dsKDR.Tables(0), htCols)
            'cpnlError.Visible = False

            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crKnowledgeDBReportDS.rpt")
            crDocument.Load(Reportpath)


            crDocument.SetDataSource(dsKDR)
        Catch ex As Exception

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
  

    Private Sub imgOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        If ddlProject.Items.Count <= 0 Then
            lstError.Items.Clear()
            lstError.Items.Add("No SubCategory  found for selected company ...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        show()
    End Sub
  
#End Region

    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Try
            If ddlCompany.SelectedIndex <> -1 Then
                fill_Project(ddlCompany.SelectedItem.Value)
                Fill_CallCategory(ddlCompany.SelectedItem.Value)
                Fill_TaskType(ddlCompany.SelectedItem.Value)
                If ddlProject.Items.Count <= 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("No SubCategory  found for selected company ...")
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
                crvReport.Visible = False
            End If
        Catch ex As Exception
            CreateLog("KnowledgeDbRpt", "ddlCompany_SelectedIndexChanged", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
