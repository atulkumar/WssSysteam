#Region "NameSpace"
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports WSSBLL
#End Region
Partial Class Reports_EVADetail
    Inherits System.Web.UI.Page
#Region "Variables"
    Private objEVA As New clsEVABLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private rptDoc As New ReportDocument 'declare object to load Report
    Private txthiddenImage As String 'stored clicked button's cation  
#End Region

#Region "Page_Load"
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
            crvReport.ToolbarStyle.Width = New Unit("800px")
            txthiddenImage = Request.Form("txthiddenImage")
            cpnlReport.State = CustomControls.Web.PanelState.Collapsed
            cpnlReport.Enabled = False
            If IsPostBack = False Then
                FillCompanyName()
                FillProject(ddlCompany.SelectedValue)
                FillCallNo(ddlCompany.SelectedValue, ddlProject.SelectedValue)
                FillTaskNo(ddlCompany.SelectedValue, ddlProject.SelectedValue, 0)
                ddlTaskNo.Enabled = False
            End If

        Catch ex As Exception
            CreateLog("Reports_EVADetail", "Page_Load-81", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "Fill Drop Down List Boxes"
#Region "FillCompany"
    ''' <summary>
    ''' This Function is used to Fill the Company DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillCompanyName()
        Try
            Dim dtComp As New Data.DataTable
            Dim dr As DataRow
            dtComp = objEVA.GetCompany
            ddlCompany.DataSource = dtComp
            dr = dtComp.NewRow
            dr(0) = 0
            dr(1) = "Select"
            dtComp.Rows.InsertAt(dr, 0)
            dtComp.AcceptChanges()
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            ddlCompany.SelectedIndex = 1
        Catch ex As Exception
            CreateLog("Reports_EVADetail", "FillCompanyName-81", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillProject"
    ''' <summary>
    ''' This Function is used to Fill the Project DropDown
    ''' </summary>
    ''' <param name="CompID"></param>
    ''' <remarks></remarks>
    Public Sub FillProject(ByVal CompID As Integer)
        Try
            Dim dtProject As New Data.DataTable
            ddlProject.Items.Clear()
            dtProject = objEVA.GetProject(CompID)
            ddlProject.DataSource = dtProject
            ddlProject.DataTextField = "Name"
            ddlProject.DataValueField = "ID"
            ddlProject.DataBind()
        Catch ex As Exception
            CreateLog("Reports_ProjectDetail", "FillProject-108", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillProject"
    ''' <summary>
    '''  This Function is used to Fill the Task  DropDown
    ''' </summary>
    ''' <param name="CompID"></param>
    ''' <param name="Project"></param>
    ''' <param name="CallNo"></param>
    ''' <remarks></remarks>
    Public Sub FillTaskNo(ByVal CompID As Integer, ByVal Project As Integer, ByVal CallNo As Integer)
        Try
            Dim dtProject As New Data.DataTable
            ddlTaskNo.Items.Clear()
            dtProject = objEVA.GetTask(CompID, Project, CallNo)
            ddlTaskNo.DataSource = dtProject
            ddlTaskNo.DataTextField = "TaskNo"
            ddlTaskNo.DataValueField = "TaskID"
            ddlTaskNo.DataBind()
            ddlTaskNo.Items.Insert(0, "--ALL--")
            If dtProject.Rows.Count > 1 Then
                ddlTaskNo.Enabled = True
            Else
                ddlTaskNo.Enabled = False
            End If
        Catch ex As Exception
            CreateLog("Reports_EVADetail", "FillTaskNo-108", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillProject"
    ''' <summary>
    ''' This Function is used to Fill the Call Number  DropDown
    ''' </summary>
    ''' <param name="CompID"></param>
    ''' <param name="Project"></param>
    ''' <remarks></remarks>
    Public Sub FillCallNo(ByVal CompID As Integer, ByVal Project As Integer)
        Try
            Dim dtProject As New Data.DataTable
            ddlTaskNo.Items.Clear()
            dtProject = objEVA.GetCall(CompID, Project)
            ddlCallTo.DataSource = dtProject
            ddlCallTo.DataTextField = "CallNo"
            ddlCallTo.DataValueField = "CallNo"
            ddlCallTo.DataBind()
            ddlCallTo.Items.Insert(0, "--ALL--")
        Catch ex As Exception
            CreateLog("Reports_EVADetail", "FillCallNo-108", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "Show Report"
    Private Sub ShowReport()
        Try
            Dim shtFlag As Short = 0
            Dim strReportPath As String 'To store the Report Path
            Dim dtEva As New DataTable
            Dim CallNo As Integer
            Dim TaskNo As Integer

            ViewState("Flag") = 1
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            cpnlReport.Enabled = True

            rptDoc = New ReportDocument()
            strReportPath = Server.MapPath("crEVA.rpt") 'to store Report Path
            rptDoc.Load(strReportPath)

            If ddlCallTo.Text.Equals("--ALL--") = True Then
                CallNo = 0
            Else
                CallNo = ddlCallTo.SelectedValue
            End If

            If ddlTaskNo.Text.Equals("--ALL--") = True Then
                TaskNo = 0
            Else
                TaskNo = ddlTaskNo.SelectedValue
            End If
            dtEva = objEVA.GetEvaDetail(ddlCompany.SelectedValue, ddlProject.SelectedValue, CallNo, TaskNo)
            rptDoc.SetDataSource(dtEva)
            crvReport.ReportSource = rptDoc
            crvReport.Visible = True
        Catch ex As Exception
            CreateLog("Reports_EVADetail", "ShowReport-166", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#End Region

#Region "Events"
    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        FillProject(ddlCompany.SelectedValue)
    End Sub

    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        FillCallNo(ddlCompany.SelectedValue, ddlProject.SelectedValue)
        FillTaskNo(ddlCompany.SelectedValue, ddlProject.SelectedValue, 0)
    End Sub

    Protected Sub ddlCallTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCallTo.SelectedIndexChanged
        Dim CallNo As Integer
        If ddlCallTo.Text.Equals("--ALL--") = True Then
            CallNo = 0
        Else
            CallNo = ddlCallTo.SelectedValue
        End If
        FillTaskNo(ddlCompany.SelectedValue, ddlProject.SelectedValue, CallNo)
    End Sub

     
    Protected Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        ShowReport()
    End Sub
#End Region
End Class
