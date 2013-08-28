#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [13/07/09]
' PURPOSE   : [This Report is used to view the project detail according to company and Project 
' TABLES    : [T040011,T040021,T040031,T01011]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports WSSBLL
#End Region
Partial Class Reports_ProjectDetail
    Inherits System.Web.UI.Page

#Region "Variables"
    Private objProject As New ProjectSummary(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private rptDoc As New ReportDocument 'declare object to load Report
    Private txthiddenImage As String 'stored clicked button's cation  
#End Region

#Region "PageLoad"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Else
            If ViewState("Flag") = 1 Then
                ShowReport()
            End If
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
            dtComp = objProject.GetCompany
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
            CreateLog("Reports_ProjectDetail", "FillCompanyName-81", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillProject"
    ''' <summary>
    ''' This Function is used to Fill the Project DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillProject(ByVal CompID As Integer)
        Try
            Dim dtProject As New Data.DataTable
            Dim dr As DataRow
            ddlProject.Items.Clear()
            dtProject = objProject.GetProject(CompID)
            ddlProject.DataSource = dtProject
            dr = dtProject.NewRow

            dr(0) = 0
            dr(1) = "Select"
            dtProject.Rows.InsertAt(dr, 0)
            dtProject.AcceptChanges()
            ddlProject.DataTextField = "Name"
            ddlProject.DataValueField = "ID"
            ddlProject.DataBind()
        Catch ex As Exception
            CreateLog("Reports_ProjectDetail", "FillProject-108", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region
#End Region

#Region "Events"
    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Try
            ViewState("Flag") = 2
            FillProject(ddlCompany.SelectedValue)
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        ShowReport()
    End Sub
#End Region

#Region "ShowReport"
    ''' <summary>
    ''' This Function is used to show the records according to Filter condition
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowReport()
        Try
            Dim shtFlag As Short = 0
            Dim strReportPath As String 'To store the Report Path
            Dim dtRBMPaid As New DataSet
            Dim strFilter As String = ""
            Dim arrvalues As New ArrayList
            lstError.Items.Clear()
            'Check the user select the mandatory Field
            If ddlCompany.SelectedValue.Equals("0") = True Then
                lstError.Items.Add("Please Select Company... ")
                shtFlag = 1
            End If
            If ddlProject.SelectedValue.Equals("0") = True Then
                lstError.Items.Add("Please Select Project... ")
                shtFlag = 1
            End If
            If shtFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

            ViewState("Flag") = 1
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            cpnlReport.Enabled = True

            rptDoc = New ReportDocument()
            strReportPath = Server.MapPath("crProjectDetail.rpt") 'to store Report Path
            rptDoc.Load(strReportPath)
            dtRBMPaid = objProject.GetProject(ddlCompany.SelectedValue, ddlProject.SelectedValue)
            dtRBMPaid.Tables(0).TableName = "dtProjectDetails"
            dtRBMPaid.Tables(1).TableName = "dtProject"
            dtRBMPaid.Tables(2).TableName = "dtCall"
            rptDoc.SetDataSource(dtRBMPaid)
            crvReport.ReportSource = rptDoc
            crvReport.Visible = True
        Catch ex As Exception
            CreateLog("Reports_ProjectDetail", "ShowReport-166", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
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
