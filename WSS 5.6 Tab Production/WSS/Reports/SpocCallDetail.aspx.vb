#Region "NameSpace"
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports WSSBLL
Imports Telerik.Web.UI

#End Region
Partial Class Reports_SpocCallDetail
    Inherits System.Web.UI.Page
#Region "Variables"

    Private objSpoc As New clsSpocWeeklyBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private rptDoc As New ReportDocument 'declare object to load Report
    Private txthiddenImage As String 'stored clicked button's cation  
    Private strCallStatus As String ' = "ALL" 'To store call type for search
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################


        '  crvReport.ToolbarStyle.Width = New Unit("1500px")
        txthiddenImage = Request.Form("txthiddenImage")
        cpnlReport.State = CustomControls.Web.PanelState.Collapsed
        cpnlReport.Enabled = False

        If IsPostBack = True Then
            If ViewState("Flag") = 1 Then
                ShowReport(ViewState("strCallStatus").ToString())
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
#Region "ShowReport"
    ''' <summary>
    ''' This Function is used to show the records according to Filter condition
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowReport(ByVal strStatus As String)
        Try
            ' strCallStatus = strStatus
            Dim shtFlag As Short = 0
            Dim strReportPath As String 'To store the Report Path
            Dim dtSpoc As New DataTable
            Dim strFilter As String = ""
            Dim arrvalues As New ArrayList
            lstError.Items.Clear()
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

            ViewState("Flag") = 1
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            cpnlReport.Enabled = True

        
            dtSpoc = objSpoc.GetCallDetail(dtFrom, dtTo, strStatus)

            grdSearch.DataSource = dtSpoc
            grdSearch.DataBind()
            grdSearch.Visible = True
       
        Catch ex As Exception
            CreateLog("Reports_ProjectDetail", "ShowReport-166", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

    Protected Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        strCallStatus = "ALL"
        ViewState("strCallStatus") = strCallStatus
        ShowReport(ViewState("strCallStatus"))
    End Sub
 

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
    Protected Sub grdSearch_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdSearch.NeedDataSource
        If Not e.IsFromDetailTable Then
            Call ShowReport(ViewState("strCallStatus").ToString())
        End If
    End Sub
    Protected Sub imgExportToExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToExcel.Click
        If grdSearch.Columns.Count <= 0 Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select the search parameters....")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        'grdSearch.PageSize = grdSearch.MasterTableView.VirtualItemCount
        'grdSearch.ExportSettings.IgnorePaging = True
        'grdSearch.ExportSettings.OpenInNewWindow = True
        grdSearch.MasterTableView.ExportToExcel()

    End Sub
    Protected Sub imgExportToPDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToPDF.Click
        strCallStatus = "Closed"
        ViewState("strCallStatus") = strCallStatus
        ShowReport(ViewState("strCallStatus").ToString())
    End Sub
 
End Class
