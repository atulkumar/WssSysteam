#Region "NameSpace"
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports WSSBLL
#End Region
Partial Class Reports_TaskH
    Inherits System.Web.UI.Page
    Private rptDoc As New ReportDocument 'declare object to load Report
    Private txthiddenImage As String 'stored clicked button's cation  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        crvReport.ToolbarStyle.Width = New Unit("1200px")
        crvReport.HasSearchButton = False
        crvReport.HasPrintButton = False
        crvReport.HasExportButton = False
        crvReport.HasDrillUpButton = False
        crvReport.HasViewList = False
        ShowReport()
    End Sub
    Private Sub ShowReport()
        Try
            Dim strReportPath As String 'To store the Report Path
            Dim dtTask As New DataTable
           
            rptDoc = New ReportDocument()
            strReportPath = Server.MapPath("crTaskH.rpt") 'to store Report Path
            rptDoc.Load(strReportPath)
            dtTask = Session("TaskHicry")
            rptDoc.SetDataSource(dtTask)
            crvReport.ReportSource = rptDoc
            crvReport.Visible = True
        Catch ex As Exception
            CreateLog("Reports_Taskh", "ShowReport-37", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
End Class
