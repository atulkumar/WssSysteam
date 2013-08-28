Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography
Partial Class MonitoringCenter_BasicMonitoring
    Inherits System.Web.UI.Page
    Dim dvtemp As DataView

#Region " Variables "

    Public Shared dsALTY As New DataSet
    Public mintPageSize As Integer
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private arrSearchText As New ArrayList
    Private Shared mdvping As New DataView
    Private Shared dvSearch As New DataView
    Private mdvDiskMonitor As DataView
    Private intdomain As Integer

    Shared mintID As Integer
    Shared mintstatus As String
    Protected _currentPageNumber As Int32 = 1

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtCSS(Me.Page, "cpnlDiskEntry")
        cpnlError.Visible = False
        ' javascript function added with controls check numeric value and decimal 
        txtLimit_DiskF.Attributes.Add("onkeypress", "FloatData('" & txtLimit_DiskF.ClientID & "');")
        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 

        '''paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlDiskEntry:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 18
        End If
        txtPageSize.Text = mintPageSize

        Try
            If Not (Page.IsPostBack) Then
                cpnlDiskEntry.Text = "Disk Monitor [" & WSSSearch.SearchCompNameID(HttpContext.Current.Session("PropCAComp")).ExtraValue & "]"
                'set paging index count from zero
                CurrentPg.Text = _currentPageNumber.ToString()
                DefineGridColumnData()
                GetDomain()
                cpnlDiskEntry.State = CustomControls.Web.PanelState.Collapsed
                'cpnldatabase enabled fales
                cpnlDiskEntry.Enabled = False

            Else
                Dim txthiddenImage As String = Request.Form("txthiddenImage")
                If txthiddenImage <> "" Then

                    Select Case txthiddenImage

                        Case "Save"
                            'Call SaveDiskRequest Function
                            If SaveDiskRequest() = True Then
                                'if save reguest true then msg pnl show msg
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            End If

                        Case "Delete"
                            'Call DeleteBGRequest function
                            If DeleteBGRequest() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            End If
                        Case "Close"
                            Response.Redirect("Configuration.aspx", False)
                        Case "Logout"
                            LogoutWSS()
                    End Select

                End If

            End If
            'call BindDiskgrid to fill grid
            If IsPostBack = True Then
                BindDiskGrid(Session("DomainName"))
                FormatGrid()
            End If


            'call format grid 

        Catch ex As Exception
        End Try
    End Sub

    Private Sub dgrDiskMonitor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrDiskMonitor.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(mdvDiskMonitor) = True Then
                    Exit Sub
                Else

                    For inti As Integer = 0 To mdvDiskMonitor.Table.Columns.Count - 2
                        'checking value of status if it is not P
                        If CType(e.Item.Cells(11).FindControl("lblStatus"), Label).Text.Trim <> "P" Then
                            'checkbox enabled is false
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = False
                        Else
                            'checkbox enabled is True
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = True
                        End If

                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                    Next
                End If


            Else
                'for search
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlDiskEntry:dgrDiskMonitor:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("Basicmonitoring", "dgrDiskMonitor_ItemDataBound-1485", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Sub Ddldomain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ddldomain.SelectedIndexChanged
        cpnlDiskEntry.State = CustomControls.Web.PanelState.Expanded
        'cpnldatabase enabled fales
        cpnlDiskEntry.Enabled = True
        DDLMachine_DiskF.Items.Clear()
        FillMachineDropDown(Ddldomain.SelectedValue)

        FillAlertTypeDropDown()

        'fill ddlalert Disk dropdown list
        DdlAlert_DiskF.DataSource = dsALTY.Tables(0)
        'textfield as Alert
        DdlAlert_DiskF.DataTextField = "AM_VC20_Code"
        'value save as ID
        DdlAlert_DiskF.DataValueField = "AM_NU9_AID_PK"
        DdlAlert_DiskF.DataBind()
        DdlAlert_DiskF.Items.Insert(0, New ListItem("Select", "0"))
        BindDiskGrid(Ddldomain.SelectedItem.Text)
        Session("DomainName") = Ddldomain.SelectedItem.Text
    End Sub

#Region " Paging Buttons "

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgrDiskMonitor.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindDiskGrid(Session("DomainName"))
    End Sub

    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgrDiskMonitor.CurrentPageIndex > 0) Then
            dgrDiskMonitor.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindDiskGrid(Session("DomainName"))
    End Sub

    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgrDiskMonitor.CurrentPageIndex < (dgrDiskMonitor.PageCount - 1)) Then
            dgrDiskMonitor.CurrentPageIndex += 1

            If dgrDiskMonitor.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgrDiskMonitor.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindDiskGrid(Session("DomainName"))
    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgrDiskMonitor.CurrentPageIndex = (dgrDiskMonitor.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindDiskGrid(Session("DomainName"))
    End Sub

#End Region

#Region " Private SUB and Function "

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill domain dropdown acc to selected company
    '                   Table t170011
    'Modify Date:       ------
    '***************************************************************************************
    Private Sub GetDomain()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            ' This function will fetch all the Domains against a company
            If SQL.Search("T170011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                'sql.Search is true then ddldomain fill with domain
                Ddldomain.DataSource = dsTemp.Tables(0)
                ' Domain Name
                Ddldomain.DataTextField = "DM_VC150_DomainName"
                ' Domain ID
                Ddldomain.DataValueField = "DM_NU9_DID_PK"
                Ddldomain.DataBind()
                Ddldomain.Items.Insert(0, New ListItem("Select", "0"))

            Else
                'if sql search is false then msg panel show msg
                lstError.Items.Clear()
                lstError.Items.Add("Sorry no Domain available for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("Basicmonitoring", "GetDomain-266", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString)
        End Try
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill Alert dropdown acc to selected company
    '                   Table t180011
    'Modify Date:       ------
    '***************************************************************************************
    Private Sub FillAlertTypeDropDown()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            dsALTY.Clear()
            'sqstr hold SqlQuery
            Dim sqstr As String
            ' This function will fetch all the alerts from t180011
            sqstr = "select  AM_NU9_AID_PK, AM_VC20_Code  from T180011 "
            If SQL.Search("T180011", "Basicmonitoring", "FillAlertTypeDropDown", sqstr, dsALTY, "", "") = True Then

            End If
        Catch ex As Exception
            CreateLog("BasicMonitoring", "FillAlertTypeDropDown-289", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       11/01/2007
    'Purpose:           This function is used to Format grid
    'Table              t130022
    'Modify Date:       ------
    '***********************************************************************************
    Private Sub FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrDiskMonitor.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrDiskMonitor.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrDiskMonitor.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("BasicMonitoring", "FormatGrid-331", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to Read Grid and add checked rows's SQID in Array
    '                   Table t170011
    'Modify Date:       ------
    '***************************************************************************************
    Private Sub ReadGrid(ByVal sqid As ArrayList)
        Try
            Dim gridrow As DataGridItem

            For Each gridrow In dgrDiskMonitor.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    'add Sqid of checked Cheeckboxes
                    sqid.Add(gridrow.Cells(0).Text)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DefineGridColumnData()
        'define width of columns
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(20)
        arrColWidth.Add(50) 'Drive
        arrColWidth.Add(110) 'Machine   
        arrColWidth.Add(85) 'start date
        arrColWidth.Add(85) 'End Date
        arrColWidth.Add(80) 'Time
        arrColWidth.Add(80) 'uid
        arrColWidth.Add(80) 'pwd
        arrColWidth.Add(100) 'alert
        arrColWidth.Add(80) 'Limit
        arrColWidth.Add(80) 'Mb/GB
        arrColWidth.Add(80) 'Space
        arrColWidth.Add(50) 'status

        arrTextboxName.Clear()
        arrTextboxName.Add("txtDriveType_DSK")
        arrTextboxName.Add("TxtMachine_dsk")
        arrTextboxName.Add("txtStartDate_Dsk")
        arrTextboxName.Add("txtEndDate_Dsk")
        arrTextboxName.Add("txtTime_Dsk")
        arrTextboxName.Add("TxtUID_Dsk")
        arrTextboxName.Add("TxtPWD_DSK")
        arrTextboxName.Add("TxtAlert_dsk")
        arrTextboxName.Add("txtLimit_DSK")
        arrTextboxName.Add("txtMb_Dsk")
        arrTextboxName.Add("TxtSpace_Dsk")
        arrTextboxName.Add("txtStatus")

        arrColumnName.Clear()
        arrColumnName.Add("RQ_VC150_CAT2") 'Drive
        arrColumnName.Add("RQ_VC150_CAT12") 'Machine
        arrColumnName.Add("RQ_VC100_Request_Date") 'satrt date
        arrColumnName.Add("EndDate") 'End date
        arrColumnName.Add("RQ_VC150_CAT9") 'Time
        arrColumnName.Add("RQ_VC150_CAT4") 'UID
        arrColumnName.Add("RQ_VC150_CAT5") 'PWD
        arrColumnName.Add("AM_VC20_Code") 'Alert
        arrColumnName.Add("RQ_VC150_CAT3") 'Limit
        arrColumnName.Add("RQ_VC150_CAT8") 'Mb
        arrColumnName.Add(" RQ_VC150_CAT6") 'Space
        arrColumnName.Add("RQ_CH2_STATUS") 'status
        arrColumnName.Add("RQ_NU9_SQID_PK")

    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to clear text boxes and reset dropdown after save
    'Modify Date:       ------
    '***************************************************************************************
    Private Sub ResetTextboxes()
        Try
            'clear textboxes of fast entry and reset Dropdown list
            DdlDrive_DiskF.SelectedIndex = 0
            TxtUID_DiskF.Text = ""

            'TxtPWD_DiskF.Attributes.Add("value", TxtPWD_DiskF.Text)
            TxtPWD_DiskF.Text = ""
            txtLimit_DiskF.Text = ""
            'DDLMachine_DiskF.SelectedIndex = -1
            Ddlhours_DiskF.SelectedIndex = 0
            DdlMin_DiskF.SelectedIndex = 0
            DDLFrequency.SelectedIndex = -1
            dtStartdate_DiskF.CalendarDate = ""
            DDLMB_DiskF.SelectedIndex = -1
            DDLSpace_diskF.SelectedIndex = -1
            DdlAlert_DiskF.SelectedIndex = -1
            dtEnddate_DiskF.CalendarDate = ""
            dtStartdate_DiskF.CalendarDate = ""
            DDLMachine_DiskF.Items.Clear()
            Ddldomain.SelectedIndex = -1
        Catch ex As Exception
            CreateLog("BasicMonitoring", "resettextboxes-1439", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Function FillMachineDropDown(ByVal Domain As Integer) As Boolean
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select MM_VC150_Machine_Name ,MM_NU9_MID ,MM_CH1_IsEnable  from t170012 where MM_NU9_DID_FK=" & Domain & " and  MM_CH1_IsEnable='E' "
            If SQL.Search("T170012 ", "BasicMonitoring", "FILLMachineDropDown", sqstr, dsTemp, "", "") = True Then
                'dvSearch = dsTemp.Tables(0).DefaultView

                DDLMachine_DiskF.DataSource = dsTemp.Tables(0)
                DDLMachine_DiskF.DataTextField = "MM_VC150_Machine_Name"
                DDLMachine_DiskF.DataValueField = "MM_NU9_MID"
                DDLMachine_DiskF.DataBind()
                Return True
            Else

                lstError.Items.Clear()
                lstError.Items.Add("Sorry no Machine available for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If

        Catch ex As Exception
            CreateLog("BasicMonitoring", "FillmachineDropdown-506", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       11/01/2007
    'Purpose:           This function is used to fill Machine acc to selected domain
    'Table              t170012
    'Modify Date:       ------
    '***********************************************************************************
    Private Sub FillDiskMachineDropDown(ByVal Domain As Integer)
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            ' This function will fetch all the Machine against domain
            sqstr = "select MM_vc100_machine_ip  from t170012 where MM_NU9_DID_FK=" & Domain & "   "
            If SQL.Search("T170012 ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                'if sql.search true then fill Machine dropdown
                DDLMachine_DiskF.DataSource = dsTemp.Tables(0)
                'Machine name
                DDLMachine_DiskF.DataTextField = "MM_vc100_machine_ip"
                'save as machine id
                DDLMachine_DiskF.DataBind()
                DDLMachine_DiskF.Items.Insert(0, New ListItem("Select", "0"))
            Else
                'if sql.search false then msg pnl show msg
                lstError.Items.Clear()
                lstError.Items.Add("Sorry no Machine available for selected Domain")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("BasicMonitoring", "FillDiskMachineDropdown-554", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       11/01/2007
    'Purpose:           This function is used to bind grid
    'Table              t130022
    'Modify Date:       ------
    '***********************************************************************************
    Private Sub DgDBEntry_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Dim itemType As ListItemType = e.Item.ItemType

        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)

            e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        End If
    End Sub

    Private Function DeleteRecord(ByVal ID As Integer) As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String

            sqstr = "delete from t130022 where  RQ_NU9_SQID_PK=" & ID
            If SQL.Delete("MONITORING", "deleterecod", sqstr, SQL.Transaction.Serializable, "") = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       11/01/2007
    'Purpose:           This function is used to bind grid
    'Table              t130022
    'Modify Date:       ------
    '***********************************************************************************
    Private Sub BindDiskGrid(ByVal DomainName As String)
        Dim dtemp As DataTable
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            dgrDiskMonitor.PageSize = mintPageSize ' set the grid page size
            ' This function will fetch data from t130022  against process and a company
            sqstr = "select RQ_VC150_CAT2,RQ_VC150_CAT12, convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as rq_vc100_request_date ,convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as Enddate ,RQ_VC150_CAT9,RQ_VC150_CAT4='*',RQ_VC150_CAT5='*',b.AM_VC20_Code,RQ_VC150_CAT3,RQ_VC150_CAT8,RQ_VC150_CAT6,RQ_CH2_STATUS,RQ_NU9_SQID_PK from t130022 ,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK  and rq_vc150_cat13='" & DomainName & "' and  RQ_NU9_PROCESSID='10020012'and  RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " order by  rq_vc150_cat2,rq_vc100_request_date "

            If SQL.Search("T130022", "Basicmonitoring", "BindDiskGrid", sqstr, dsTemp, "", "") = True Then
                'if sql search is true then we bind grid
                'put value of dataset to dataview
                mdvDiskMonitor = dsTemp.Tables("T130022").DefaultView
                dvtemp = dsTemp.Tables("T130022").DefaultView ' use for paging
                'filterdataview
                GetFilteredDataView(mdvDiskMonitor, GetRowFilter)
                'Datagrid fetch data from dataview
                dgrDiskMonitor.DataSource = mdvDiskMonitor.Table
                GetFilteredDataView(dvtemp, GetRowFilter)
                'Paging
                If (mintPageSize) * (dgrDiskMonitor.CurrentPageIndex) > dvtemp.Table.Rows.Count - 1 Then
                    dgrDiskMonitor.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                'bind grid
                dgrDiskMonitor.DataBind()
            Else
                dtemp = dsTemp.Tables("T130022")
                dvtemp = dtemp.DefaultView
                'if sql search is false then dummy row of columns shown in datagrid
                'mdvDiskMonitor = dsTemp.Tables("T130022").DefaultView
                dgrDiskMonitor.DataSource = dtemp
                dgrDiskMonitor.DataBind()

            End If

            Dim intRows As Integer = dvtemp.Table.Rows.Count
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

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       12/01/2007
    'Purpose:           This function is used  to get the text from search textboxes in                         an array list
    'Modify Date:       ------
    '***********************************************************************************
    Private Sub GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlDiskEntry:dgrDiskMonitor:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("BasicMonitoring", "GetsearchText-1045", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       12/01/2007
    'Purpose:           This function is used  to Gets the row filter string
    'Modify Date:       ------
    '***********************************************************************************
    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function
            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To mdvDiskMonitor.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If mdvDiskMonitor.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf mdvDiskMonitor.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
                        If IsNumeric(arrSearchText(inti)) = False Then
                            strRowFilter &= " " & arrColumnName(inti) & "=-101019 and"      'change row filter to non existing value
                        Else
                            strRowFilter &= " " & arrColumnName(inti) & "=" & arrSearchText(inti) & " and"
                        End If
                    Else
                        strRowFilter &= " " & arrColumnName(inti) & " like " & arrSearchText(inti) & " and"
                    End If
                End If
            Next

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
            CreateLog("BasicMonitoring", "GetRowFilter-1085", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       12/01/2007
    'Purpose:           This function is used  to Save DiskFast Entry
    '                   Table t130022
    'Modify Date:       ------
    '***************************************************************************************
    Private Function SaveDiskRequest() As Boolean
        Try
            ' call validate request function
            If ValidateRequest() = False Then
                'if return false then exit sub
                Exit Function
            End If

            Dim Multi As New SQL.AddMultipleRows
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim intDomainName As Integer
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020012'"
            'reader
            sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold domainname
                intDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
                'hold machineCode
                intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
                sqRDR.Close()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("This Process is not Registered Please Register it...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            Dim intMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            'define column name
            arColName.Add("RQ_NU9_SQID_PK")
            arColName.Add("RQ_NU9_PROCESSID")
            arColName.Add("RQ_VC150_CAT1")
            arColName.Add("RQ_VC150_CAT2") 'Drive type
            arColName.Add("RQ_VC150_CAT3") 'Limit
            arColName.Add("RQ_VC150_CAT4") 'UID
            arColName.Add("RQ_VC150_CAT5") 'PWD
            arColName.Add("RQ_VC150_CAT6") 'Space  
            arColName.Add("RQ_VC150_CAT12") 'Machine IP
            arColName.Add("RQ_VC150_CAT8") 'MB/GB
            arColName.Add("RQ_VC150_CAT9") 'Time
            arColName.Add("RQ_VC150_CAT13") 'Domain
            arColName.Add("RQ_VC100_REQUEST_DATE") 'Start date
            arColName.Add("RQ_NU9_ALERT_FK") 'alert
            arColName.Add("RQ_NU9_Domain_FK") 'domain
            arColName.Add("RQ_NU9_Machine_Code_FK") 'machine code
            arColName.Add("RQ_CH2_STATUS") 'status
            arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

            Dim dtStart As Date
            Dim dtEnd As Date
            Dim intDays As Integer
            Dim dHours_D As Integer
            Dim dmin_D As Integer
            Dim dhours_F As Integer
            Dim dmin_f As Integer
            Dim dHM_f As String
            'hold start date
            dtStart = dtStartdate_DiskF.CalendarDate
            dtEnd = dtEnddate_DiskF.CalendarDate

            'calculate diff of enddate and start date
            intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)

            Dim dtStDate1 As String
            Dim dtStDate As DateTime
            Dim dtENDate As DateTime
            'hold start date ,hours and min
            dtStDate = CDate(CDate(dtStartdate_DiskF.CalendarDate).ToShortDateString & " " & Ddlhours_DiskF.SelectedValue & ":" & DdlMin_DiskF.SelectedValue)
            Dim dttime As DateTime = CDate(Ddlhours_DiskF.SelectedValue & ":" & DdlMin_DiskF.SelectedValue)
            dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString

            'hold end date,hours and min
            dtENDate = CDate(CDate(dtEnddate_DiskF.CalendarDate).ToShortDateString & " " & Ddlhours_DiskF.SelectedValue & ":" & DdlMin_DiskF.SelectedValue)
            While dtStDate <= dtENDate
                'dtENDate.AddDays(+1)
                arRowData.Clear()
                arRowData.Add(intMax)
                arRowData.Add("10020012")
                arRowData.Add("DSK")
                arRowData.Add(DdlDrive_DiskF.SelectedValue)
                arRowData.Add(txtLimit_DiskF.Text)
                arRowData.Add(Encrypt(TxtUID_DiskF.Text))
                arRowData.Add(Encrypt(TxtPWD_DiskF.Text))
                arRowData.Add(DDLSpace_diskF.SelectedValue)
                arRowData.Add(DDLMachine_DiskF.SelectedItem.Text)
                arRowData.Add(DDLMB_DiskF.SelectedValue)
                arRowData.Add(IIf(dtStDate.TimeOfDay.Hours < 10, "0" & dtStDate.TimeOfDay.Hours, dtStDate.TimeOfDay.Hours) & IIf(dtStDate.TimeOfDay.Minutes < 10, "0" & dtStDate.TimeOfDay.Minutes, dtStDate.TimeOfDay.Minutes) & "00")
                arRowData.Add(Ddldomain.SelectedItem.Text)
                arRowData.Add(dtStDate1)
                arRowData.Add(DdlAlert_DiskF.SelectedValue)
                arRowData.Add(intDomainName)
                arRowData.Add(intMachineCode)
                arRowData.Add("P")
                arRowData.Add(Session("PropCAComp"))
                'add Multiple rows in t130022
                Multi.Add("T130022", arColName, arRowData)
                'increment int max
                intMax += 1
                'dtStDate = dtStDate.AddHours(CType(Gridrow.FindControl("txtFrequency"), TextBox).Text.Trim)
                'add frequency hours in start date
                dtStDate = dtStDate.AddHours(DDLFrequency.SelectedValue)
                'add frequency min to end date
                dtStDate = dtStDate.AddMinutes(DdlMin_Dsk.SelectedValue)
                dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString

                'dtStDate = dtStDate.AddMinutes(dmin_f)
            End While
            'multisave
            Multi.Save()

            'Multi.Dispose()
            ResetTextboxes()
            Return True
        Catch ex As Exception
            CreateLog("BasicMonitoring", "SaveDiskRequest-1213", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to chek all textboxes are fill
    'Table                  t130022
    'Modify Date:       ------
    '***************************************************************************************
    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        Dim currentDate As Date
        currentDate = Date.Now.Today

        If DdlDrive_DiskF.SelectedValue.Equals("") Then
            lstError.Items.Add("Drive Name cannot be blank...")
            shFlag = 1
        End If

        If DDLMachine_DiskF.SelectedValue.Equals("0") Then
            lstError.Items.Add("Select Machine name...")
            shFlag = 1
        End If

        If DdlAlert_DiskF.SelectedValue.Equals("0") Then
            lstError.Items.Add("Select alert type ...")
            shFlag = 1
        End If

        If DDLFrequency.SelectedValue.Equals("0") And DdlMin_Dsk.SelectedValue.Equals("0") Then
            lstError.Items.Add("Frequency cannot be less then 0...")
            shFlag = 1
        End If

        If dtStartdate_DiskF.CalendarDate = "" Then
            lstError.Items.Add("Start Date cannot be blank...")
            shFlag = 1
            If dtEnddate_DiskF.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
            Else
                If CDate(dtStartdate_DiskF.CalendarDate) > CDate(dtEnddate_DiskF.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        Else
            If dtEnddate_DiskF.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
                If CDate(dtStartdate_DiskF.CalendarDate) < currentDate Then
                    lstError.Items.Add("Start Date cannot be Less than Current date...")
                End If
            Else
                If CDate(dtStartdate_DiskF.CalendarDate) > CDate(dtEnddate_DiskF.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        End If

        If TxtUID_DiskF.Text.Equals("") Then
            lstError.Items.Add("UID cannot be blank...")
            shFlag = 1
        End If

        If txtLimit_DiskF.Text.Equals("") Then
            lstError.Items.Add("Limit cannot be blank...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            'message panel display msg
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            'return False
            Return False
        Else
            'return True
            Return True
        End If

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to delete multiple Rows from grid
    '                   Table t130022
    'Modify Date:       ------
    '***************************************************************************************
    Private Function DeleteBGRequest() As Boolean
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

    '*************************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill UID and Pwd acc to selected Domain and Machine
    '                   Table t170012
    'Modify Date:       ------
    '*************************************************************************************************
    Private Function FillMachineDiskUID() As Boolean
        Dim StrSql As String
        Dim SQID As Integer
        Dim subreport As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            'strSql hold SQL Query
            'this function fetch Machine uid and PWD against macine and Domain ID
            StrSql = "select MM_VC500_UID,MM_VC500_PWD from T170012 where MM_NU9_DID_FK=" & Ddldomain.SelectedValue & " AND MM_NU9_MID=" & DDLMachine_DiskF.SelectedValue & " "
            Dim blnStatus As Boolean
            If StrSql = "" Then
                blnStatus = False
            End If
            Dim SqDrReport As SqlDataReader
            SqDrReport = SQL.Search("", "", StrSql, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'reader
                SqDrReport.Read()
                If IsDBNull(SqDrReport.Item("MM_VC500_UID")) = False Then
                    ' reader read value from t170011 and fill text boxes UID and PWD
                    TxtUID_DiskF.Text = Decrypt(SqDrReport.Item("MM_VC500_UID"))
                    TxtPWD_DiskF.Text = Decrypt(SqDrReport.Item("MM_VC500_PWD"))
                    TxtPWD_DiskF.Attributes.Add("value", TxtPWD_DiskF.Text)
                Else
                    'ig dbnull is true 
                    TxtUID_DiskF.Text = ""
                    TxtPWD_DiskF.Text = ""
                    TxtPWD_DiskF.Attributes.Add("value", TxtPWD_DiskF.Text)
                End If
            Else
                TxtUID_DiskF.Text = ""
                TxtPWD_DiskF.Text = ""
                TxtPWD_DiskF.Attributes.Add("value", TxtPWD_DiskF.Text)
            End If

            SqDrReport.Close()
        Catch ex As Exception
        End Try
    End Function

    Private Function Encrypt(ByVal Data As String) As String
        Dim shaM As SHA1Managed = New SHA1Managed
        System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        '// Getting the bytes of the encrypted data.//
        Dim bytEncrypt() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        '// Converting the byte into string.//
        Dim strEncrypt As String = System.Convert.ToBase64String(bytEncrypt)
        Encrypt = strEncrypt
    End Function

    Private Function Decrypt(ByVal Data As String) As String
        Dim bytData() As Byte = System.Convert.FromBase64String(Data)
        Dim strData As String = ASCIIEncoding.ASCII.GetString(bytData)
        Decrypt = strData
    End Function

#End Region

    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("Configuration.aspx", False)
    End Sub
End Class
