Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient



Partial Class MonitoringCenter_DeleteRecords
    Inherits System.Web.UI.Page
    Private mdvProcessMonitor As New DataView
    Public mintPageSize As Integer
    'Protected WithEvents lblrecords As System.Web.UI.WebControls.Label
    'Protected WithEvents TotalRecods As System.Web.UI.WebControls.Label
    'Protected WithEvents Panel7 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    'Protected WithEvents TxtrequestID As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblFreq As System.Web.UI.WebControls.Label
    'Protected WithEvents DDLFrequency As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DdlMin_Dsk As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents dgrDeleteRecord As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents DdlProcess As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cpnlDeleteRecords As CustomControls.Web.CollapsiblePanel
    Protected _currentPageNumber As Int32 = 1

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        txtCSS(Me.Page, "cpnlDeleteRecords")
        cpnlError.Visible = False

        mintPageSize = Val(Request.Form("cpnlDeleteRecords:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 18
        End If
        txtPageSize.Text = mintPageSize

        Try
            If Not (Page.IsPostBack) Then

                CurrentPg.Text = _currentPageNumber.ToString()

                cpnlDeleteRecords.State = CustomControls.Web.PanelState.Collapsed
                'cpnldatabase enabled fales
                cpnlDeleteRecords.Enabled = False
            Else
                Dim txthiddenImage As String = Request.Form("txthiddenImage")
                If txthiddenImage <> "" Then
                    Select Case txthiddenImage
                        Case "Save"

                        Case "Close"
                            Response.Redirect("Configuration.aspx", False)

                        Case "Delete"
                            'Call DeleteBGRequest function
                            If DeleteRequest() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)

                            End If
                        Case "Logout"
                            LogoutWSS()
                    End Select
                End If
            End If

            If IsPostBack = True Then
                BindGrid(DdlProcess.SelectedValue)
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub BindGrid(ByVal Process As String)
        Dim dttemp As New DataTable

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            dgrDeleteRecord.PageSize = mintPageSize ' set the grid page size
            ' This function will fetch data from t130022  against process and a company
            If Process <> "" Then

                Select Case Process

                    Case "10020012"
                        sqstr = " select RQ_NU9_SQID_PK as ID, RQ_VC150_CAT1 as 'Process Type', RQ_VC150_CAT2 as Drive,RQ_VC150_CAT12 as Machine, convert(varchar,convert(datetime,rq_vc100_request_date,101),101) as Startdate ,convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as Enddate ,RQ_VC150_CAT9 as Time,RQ_VC150_CAT4 ='*',RQ_VC150_CAT5 ='*',b.AM_VC20_Code as Alert,RQ_VC150_CAT3 as Limit,RQ_VC150_CAT8 as 'MB/GB',RQ_VC150_CAT6 as Space,RQ_CH2_STATUS as Status,RQ_NU9_SQID_PK  from t130022 ,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK  and RQ_NU9_PROCESSID='10020012'and  RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " and rq_ch2_status like '[C-S]'"

                    Case "10020020"
                        sqstr = " select  RQ_NU9_SQID_PK as ID, RQ_VC150_CAT1 as 'Process Type' ,RQ_VC150_CAT2 as 'DB Name',RQ_VC150_CAT3 as' DB Type',RQ_VC150_CAT12 as Machine,RQ_VC150_CAT4 as StartDate,RQ_VC150_CAT6 as EndDate, RQ_VC150_CAT5 as Time,  RQ_VC150_CAT7 ='*' , RQ_VC150_CAT8 ='*', RQ_VC150_CAT9 as source, b.AM_VC20_Code as Alert,RQ_VC150_CAT10 as Limit,RQ_VC150_CAT11 as 'MB/GB', RQ_CH2_STATUS as Status from T130022,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK and  RQ_NU9_PROCESSID='10020020'  and RQ_VC150_CAT1='DB' and  RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " and rq_ch2_status like '[C-S]'"

                    Case "10020016"
                        sqstr = " select RQ_NU9_SQID_PK as ID, RQ_VC150_CAT1 as 'Process Type' ,RQ_VC150_CAT3 as Domain,RQ_VC150_CAT3 as Machine, convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as StartDate ,convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as Enddate ,RQ_VC150_CAT4 as Time,b.AM_VC20_Code as Alert,RQ_CH2_STATUS as Status from t130022 ,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK   and  RQ_NU9_PROCESSID='10020016'and  RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " and rq_ch2_status like '[C-S]'"
                    Case "10020018"
                        sqstr = " select RQ_NU9_SQID_PK as ID, RQ_VC150_CAT1 as 'Object Type', RQ_VC150_CAT2 as' Object name', RQ_VC150_CAT4 StartDate , rq_vc100_request_date as ReqDate ,RQ_VC150_CAT3 as Reoc,b.AM_VC20_Code as Alert,RQ_VC150_CAT9 as 'Status Check',RQ_VC150_CAT11 as 'Time Check',RQ_VC150_CAT10 as 'Status Alert',RQ_VC150_CAT12 as Machine,RQ_VC150_CAT13 as ENV,RQ_CH2_STATUS as Status,RQ_NU9_SQID_PK from t130022 ,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK   and  RQ_NU9_PROCESSID='10020018'and  RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " and rq_ch2_status like '[C-S]'"

                End Select
                If sqstr = "" Then
                    Exit Sub
                End If

            End If
            If SQL.Search("T130022", "Basicmonitoring", "BindDiskGrid", sqstr, dsTemp, "", "") = True Then
                'if sql search is true then we bind grid
                'put value of dataset to dataview
                mdvProcessMonitor = dsTemp.Tables("T130022").DefaultView
                'filterdataview
                'GetFilteredDataView(mdvProcessMonitor, GetRowFilter)
                'Datagrid fetch data from dataview
                dgrDeleteRecord.DataSource = mdvProcessMonitor.Table
                'Paging()
                If (mintPageSize) * (dgrDeleteRecord.CurrentPageIndex) > mdvProcessMonitor.Table.Rows.Count - 1 Then
                    dgrDeleteRecord.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                'bind grid
                dgrDeleteRecord.DataBind()
                dgrDeleteRecord.Visible = True
            Else
                dttemp = dsTemp.Tables(0)
                'if sql search is false then dummy row of columns shown in datagrid
                mdvProcessMonitor = dttemp.DefaultView
                dgrDeleteRecord.DataSource = dsTemp.Tables(0)
                dgrDeleteRecord.DataBind()
                dgrDeleteRecord.Visible = True
            End If

            Dim intRows As Integer = mdvProcessMonitor.Table.Rows.Count
            'paging
            Dim _totalPages As Double = 1
            Dim _totalrecords As Int32
            If Not Page.IsPostBack Then
                _totalrecords = intRows
                _totalPages = _totalrecords / mintPageSize
                TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                TotalRecods.Text = _totalrecords
            Else
                _totalrecords = intRows
                If CurrentPg.Text = 0 And _totalrecords > 0 Then
                    CurrentPg.Text = 1
                End If
                If _totalrecords = 0 Then
                    CurrentPg.Text = 0
                End If
                _totalPages = _totalrecords / mintPageSize
                TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                _totalPages = Double.Parse(TotalPages.Text)
                TotalRecods.Text = _totalrecords
            End If

        Catch ex As Exception
            CreateLog("BasicMonitoring", "bindDiskGrid-1019", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function

            Dim strRowFilter As String
            If strRowFilter <> Nothing Then
                strRowFilter = strRowFilter.Replace("*", "%")
                If strRowFilter.Substring(strRowFilter.Length - 3, 3) = "and" Then
                    strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3)
                Else
                    strRowFilter = strRowFilter
                End If
            End If
            Return strRowFilter
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetRowFilter-456", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Sub ReadGrid(ByVal sqid As ArrayList)
        Try
            Dim gridrow As DataGridItem

            For Each gridrow In dgrDeleteRecord.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    'add Sqid of checked Cheeckboxes
                    sqid.Add(gridrow.Cells(0).Text)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Function DeleteRequest() As Boolean
        Dim arSQId As New ArrayList
        Dim strSQID As String
        ReadGrid(arSQId) 'Add sqid in Array

        For cnt As Integer = 0 To arSQId.Count - 1
            strSQID &= arSQId(cnt) + ","
        Next
        strSQID = strSQID.Remove((strSQID.Length - 1), 1)

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            'strDeleteSQL hold delete query 
            Dim strDeleteSQL As String
            'this query delete data from database agains sqid
            strDeleteSQL = "delete from T130022 where  RQ_NU9_SQID_PK in (" & strSQID & ")"
            If SQL.Delete("BasicMonitoring", "DeleteBGRequest", strDeleteSQL, SQL.Transaction.ReadCommitted, "") = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("BasicMonitoring", "DeleteBGRRequest-1326", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try

    End Function


    Private Sub DdlProcess_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlProcess.SelectedIndexChanged
        'Session("DomainName") = DdlProcess.SelectedValue
        cpnlDeleteRecords.State = CustomControls.Web.PanelState.Expanded
        'cpnldatabase enabled fales
        cpnlDeleteRecords.Enabled = True
        If DdlProcess.SelectedValue.Equals("0") Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select Process...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            dgrDeleteRecord.Visible = False
            Exit Sub
            'Else
            '    BindGrid(DdlProcess.SelectedValue)
        End If
    End Sub

    Private Sub dgrDeleteRecord_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(mdvProcessMonitor) = True Then
                    Exit Sub
                Else

                    For inti As Integer = 0 To mdvProcessMonitor.Table.Columns.Count - 1
                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                    Next
                End If

            End If
        Catch ex As Exception
            CreateLog("DeleteRecord", " dgrDeleteRecord_ItemDataBound-240", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgrDeleteRecord.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindGrid(DdlProcess.SelectedValue)
    End Sub

    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgrDeleteRecord.CurrentPageIndex > 0) Then
            dgrDeleteRecord.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindGrid(DdlProcess.SelectedValue)
    End Sub

    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgrDeleteRecord.CurrentPageIndex < (dgrDeleteRecord.PageCount - 1)) Then
            dgrDeleteRecord.CurrentPageIndex += 1

            If dgrDeleteRecord.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgrDeleteRecord.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindGrid(DdlProcess.SelectedValue)
    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgrDeleteRecord.CurrentPageIndex = (dgrDeleteRecord.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindGrid(DdlProcess.SelectedValue)
    End Sub

End Class
