Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Text
Imports System.Security.Cryptography
Imports System.Data


Partial Class MonitoringCenter_ChkDBStatus
    Inherits System.Web.UI.Page
    Public Shared dsALTY As New DataSet
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private arrSearchText As New ArrayList
    'Private mdvDBMonitor As DataView
    'paging variables
    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer
    'Protected WithEvents txtPageSize As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Ddldomain As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents pg As System.Web.UI.WebControls.Label
    'Protected WithEvents CurrentPg As System.Web.UI.WebControls.Label
    'Protected WithEvents of As System.Web.UI.WebControls.Label
    'Protected WithEvents TotalPages As System.Web.UI.WebControls.Label
    'Protected WithEvents Firstbutton As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Prevbutton As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Nextbutton As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Lastbutton As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Button3 As System.Web.UI.WebControls.Button
    'Protected WithEvents Panel6 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Panel5 As System.Web.UI.WebControls.Panel
    'Protected WithEvents ddlHours_F As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DdlMins_F As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddldb_Type As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents lblrecords As System.Web.UI.WebControls.Label
    'Protected WithEvents TotalRecods As System.Web.UI.WebControls.Label
    'Protected WithEvents Panel7 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    'Protected WithEvents lblFreq As System.Web.UI.WebControls.Label
    'Protected WithEvents DDLFrequency As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DdlMin_DB As System.Web.UI.WebControls.DropDownList
    Dim dvtemp As DataView
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        'Put user code to initialize the page here

        txtCSS(Me.Page, "cpnlDBMonitor")
        cpnlError.Visible = False
        ' javascript function added with controls check numeric value and decimal 
        txtLimit.Attributes.Add("onkeypress", "FloatData('" & txtLimit.ClientID & "');")
        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 

        '''paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlDBMonitor:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 18
        End If
        txtPageSize.Text = mintPageSize
        ''textbox objectname and reportdropdown list visible acc to selected value of object type
        '******************************  

        Try
            If Not (Page.IsPostBack) Then
                cpnlDBMonitor.Text = "Data Base Monitor [" & WSSSearch.SearchCompNameID(HttpContext.Current.Session("PropCAComp")).ExtraValue & "]"
                'set paging index count from zero
                CurrentPg.Text = _currentPageNumber.ToString()
                'GetCompany()
                'DdlDomain.Items.Clear()
                'DefineGridColumnData()
                DefineDBGridColumnData()
                'call domain
                GetDomain()
                'call fillalertTypeDropdown
                'FillAlertTypeDropDown()

                ''***************************
                ''fill ddlalert Database dropdown list
                'DDLAlert_DB.DataSource = dsALTY.Tables(0)
                ''textfield as Alert
                'DDLAlert_DB.DataTextField = "AM_VC20_Code"
                ''Value Save as ID
                'DDLAlert_DB.DataValueField = "AM_NU9_AID_PK"
                'DDLAlert_DB.DataBind()
                '***************************************
                cpnlDBMonitor.State = CustomControls.Web.PanelState.Collapsed
                'cpnldatabase enabled fales
                cpnlDBMonitor.Enabled = False



            Else
                Dim txthiddenImage As String = Request.Form("txthiddenImage")
                If txthiddenImage <> "" Then

                    Select Case txthiddenImage
                        'Case "Ok"
                        '    If UpdateAlertFlow() = True Then
                        '        Response.Write("<script>window.close();</script>")
                        '    End If
                        Case "Save"
                            'Call SaveDiskRequest Function
                            'If SaveDiskRequest() = True Then
                            '    'if save reguest true then msg pnl show msg
                            '    lstError.Items.Clear()
                            '    lstError.Items.Add("Record saved successfully...")
                            '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            'End If
                            If SaveDBRequest() = True Then
                                'if save reguest true then msg pnl show msg
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)

                            End If
                        Case "Close"
                            Response.Redirect("Configuration.aspx", False)

                        Case "Delete"
                            'Call DeleteBGRequest function
                            If DeleteBGRequest() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            End If
                        Case "Logout"
                            LogoutWSS()
                    End Select

                End If

            End If
            'call BindDiskgrid to fill grid
            'bindDiskGrid()
            If IsPostBack = True Then
                BindGrid(Session("DomainName"))
                FormatGrid()
            End If

            'call format grid 
            'FormatGrid()
        Catch ex As Exception
        End Try
    End Sub
    Private Function FillAlertTypeDropDown()
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
    End Function

    Private Sub BindGrid(ByVal domain As String)
        Try
            Dim strSQL As String
            Dim dsTemp As New DataSet
            Dim dtTemp As New DataTable

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            dgrDBMonitor.PageSize = mintPageSize ' set the grid page size

            strSQL = "select RQ_VC150_CAT2, RQ_VC150_CAT3,RQ_VC150_CAT12,RQ_VC150_CAT4,RQ_VC150_CAT6, RQ_VC150_CAT5,  RQ_VC150_CAT7='*' , RQ_VC150_CAT8='*', RQ_VC150_CAT9, b.AM_VC20_Code, " & _
             "RQ_VC150_CAT10,RQ_VC150_CAT11, RQ_CH2_STATUS , RQ_NU9_SQID_PK  from T130022,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK and  RQ_NU9_PROCESSID='10020020'  and RQ_VC150_CAT1='DB' and RQ_VC150_CAT13='" & domain & "' and  RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " order by RQ_VC150_CAT4"
            If SQL.Search("T130022", "DB_Search", "BindGrid", strSQL, dsTemp, "", "") = True Then
                'mdvDBMonitor = dsTemp.Tables("T130032").DefaultView
                dvtemp = dsTemp.Tables("T130022").DefaultView ' use for paging
                ModifyDataView()
                '  mdvBGDailyMonitor.RowFilter = GetRowFilter()
                'filterdataview
                'GetFilteredDataView(dvtemp, GetRowFilter)
                'Datagrid fetch data from dataview
                'dgrDBMonitor.DataSource = dvtemp
                GetFilteredDataView(dvtemp, GetRowFilter)
                dgrDBMonitor.DataSource = dvtemp
                'Paging
                If (mintPageSize) * (dgrDBMonitor.CurrentPageIndex) > dvtemp.Table.Rows.Count - 1 Then
                    dgrDBMonitor.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                dgrDBMonitor.DataBind()
            Else
                'Dummy ROW In grid
                dtTemp = dsTemp.Tables(0)
                dvtemp = dtTemp.DefaultView
                dgrDBMonitor.DataSource = dtTemp
                dgrDBMonitor.DataBind()


            End If

            ''paging count
            Dim intRows As Integer = dvtemp.Table.Rows.Count       'mdvtable.Table.Rows.Count
            'CurrentPage.Text = _currentPageNumber.ToString()
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

            '''

        Catch ex As Exception
            CreateLog("DBMonitor", "BindGrid-368", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub
    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrDBMonitor.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrDBMonitor.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrDBMonitor.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "FormatGrid-133", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    Private Function GetDomain()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            If SQL.Search("T170011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlDomain dropdown fill acc to company
                Ddldomain.DataSource = dsTemp.Tables(0)
                'domain Name
                Ddldomain.DataTextField = "DM_VC150_DomainName"
                'domainId
                Ddldomain.DataValueField = "DM_NU9_DID_PK"
                Ddldomain.DataBind()
                Ddldomain.Items.Insert(0, New ListItem("Select", "0"))
            Else
                'SQL.Search is False Msgpanel show no domain exist for  selected company
                lstError.Items.Clear()
                lstError.Items.Add("No domain avilable for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("DBMonitor", "GetDomain-206", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function
    '****************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           this function fill machine dropdown list 
    '                   pass value of selected domain from dropdown Domain
    '                   table t170012 
    'Modify Date:       ------
    '*****************************************************************************
    Private Function GetMachine(ByVal Domain As Integer)
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select MM_VC150_Machine_Name ,MM_NU9_MID   from t170012 where MM_NU9_DID_FK=" & Domain & " and  MM_CH1_IsEnable='E' "
            If SQL.Search("T170012 ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlMachine dropdown fill acc to selected domain
                DdlMachine.DataSource = dsTemp.Tables(0)
                'Machine Name
                DdlMachine.DataTextField = "MM_VC150_Machine_Name"
                DdlMachine.DataBind()
            Else
                'SQL.Search is False Msgpanel show no Machine exist for  selected Domain
                lstError.Items.Clear()
                lstError.Items.Add("No machine  avilable for selected domain")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("DBMonitor", "GetMachine", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Function

    Private Sub GetDBInfo()

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch  ENV against a company
            sqstr = "select name from udc where udctype='DBT' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "DB", "getDB", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlENV dropdown fill acc to selected company
                ddldb_Type.DataSource = dsTemp.Tables(0)
                'Env Name
                ddldb_Type.DataTextField = "name"
                ddldb_Type.DataBind()
            Else
                'SQL.Search is False ddlENV dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Database type  avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetEnvironment-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Function ModifyDataView()
        Try

            For inti As Integer = 0 To dvtemp.Table.Rows.Count - 1
                'change julian date to show in grid which we fetch from database 

                dvtemp.Table.Rows(inti).Item("RQ_VC150_CAT4") = JulianToDate(dvtemp.Table.Rows(inti).Item("RQ_VC150_CAT4")).ToShortDateString
                'change julian date  to show in grid which we fetch from database
                dvtemp.Table.Rows(inti).Item("RQ_VC150_CAT6") = JulianToDate(dvtemp.Table.Rows(inti).Item("RQ_VC150_CAT6")).ToShortDateString
            Next
            dvtemp.Table.AcceptChanges()
        Catch ex As Exception
            CreateLog("DBMonitor", "ModifyDataView-523", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    Private Function GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlDBMonitor:dgrDBMonitor:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("DBMonitor", "GetSeacrhText-413", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function
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
            For inti As Integer = 0 To dvtemp.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If dvtemp.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf dvtemp.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("DBMonitor", "GetRowFilter-456", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Sub dgrDBMonitor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrDBMonitor.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(dvtemp) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To dvtemp.Table.Columns.Count - 2
                        'checking value of status if it is not P
                        If CType(e.Item.Cells(12).FindControl("lblStatus"), Label).Text.Trim <> "P" Then
                            'checkbox enabled is false
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = False
                        Else
                            'checkbox enabled is True
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = True
                        End If

                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        'Here check value of status if it is P or H

                        'If CType(e.Item.Cells(13).FindControl("lblStatus"), Label).Text.Trim = "P" Or CType(e.Item.Cells(13).FindControl("lblStatus"), Label).Text.Trim = "H" Then
                        '    'open popup window
                        '    e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        'Else
                        '    'message
                        '    e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck56();")
                        'End If
                        'e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55()")
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                        'e.Item.Cells(inti + 2).ToolTip = IIf(IsDBNull(mdvBGDailyMonitor.Table.Rows(e.Item.ItemIndex).Item(inti)) = True, "", mdvBGDailyMonitor.Table.Rows(e.Item.ItemIndex).Item(inti))
                    Next
                End If
            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlDBMonitor:dgrDBMonitor:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("DBMonitor", "dgrDBMonitor_ItemDataBound-501", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub
    Private Function SaveDBRequest() As Boolean
        Try
            If ValidateDBRequest() = False Then
                Exit Function
            End If
            Dim Multi As New SQL.AddMultipleRows
            Dim intDomainName As Integer
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020020'"
            'reader
            sqRDR = SQL.Search("DBMonitor", "SaveDBequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
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
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString

            Dim intMax As Integer = SQL.Search("BasicMonitoring", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            arColName.Add("RQ_NU9_SQID_PK")
            arColName.Add("RQ_NU9_PROCESSID")
            arColName.Add("RQ_VC150_CAT1") 'DB 
            arColName.Add("RQ_VC150_CAT2") 'DB Name
            arColName.Add("RQ_VC150_CAT3") 'DbType
            arColName.Add("RQ_VC150_CAT4") ' start date
            arColName.Add("RQ_VC150_CAT5") 'Time
            arColName.Add("RQ_VC150_CAT6") 'end date
            arColName.Add("RQ_VC150_CAT7") ' uid
            arColName.Add("RQ_VC150_CAT8") 'pwd
            arColName.Add("RQ_VC150_CAT9") ' Source  
            arColName.Add("RQ_VC150_CAT10") ' Limit
            arColName.Add("RQ_VC150_CAT11") 'Mb
            arColName.Add("RQ_VC150_CAT12") 'Machine
            arColName.Add("RQ_VC150_CAT13") 'Domain
            arColName.Add("RQ_NU9_Machine_Code_FK") 'machine code
            arColName.Add("RQ_NU9_ALERT_FK") 'alert


            arColName.Add("RQ_VC100_REQUEST_DATE")
            arColName.Add("RQ_NU9_Domain_FK") 'domain
            arColName.Add("RQ_CH2_STATUS")
            arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

            Dim dtStart As Date
            Dim dtEnd As Date
            Dim intDays As Integer
            'hold start date
            dtStart = dtStartdate_DB.CalendarDate()
            'hold end date
            dtEnd = dtEnddate_DB.CalendarDate
            intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            'Dim arColName As New ArrayList
            'Dim arRowData As New ArrayList

            'Dim intMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            'intMax += 1

            Dim dtStDate1 As String
            'hold start date ,hours and min
            Dim dtStDate As DateTime
            Dim dtENDate As DateTime
            dtStDate = CDate(CDate(dtStartdate_DB.CalendarDate).ToShortDateString & " " & ddlHours_F.SelectedValue & ":" & DdlMins_F.SelectedValue)
            Dim dttime As DateTime = CDate(ddlHours_F.SelectedValue & ":" & DdlMins_F.SelectedValue)
            dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString
            dtENDate = CDate(CDate(dtEnddate_DB.CalendarDate).ToShortDateString & " " & ddlHours_F.SelectedValue & ":" & DdlMins_F.SelectedValue)
            While dtStDate <= dtENDate
                'dtENDate.AddDays(+1)
                arRowData.Clear()
                arRowData.Add(intMax)
                arRowData.Add("10020020")
                arRowData.Add("DB")
                arRowData.Add(TxtDB_Name.Text)
                arRowData.Add(ddldb_Type.SelectedItem.Value)
                arRowData.Add(DateToJulian(dtStDate))

                arRowData.Add(IIf(dtStDate.TimeOfDay.Hours < 10, "0" & dtStDate.TimeOfDay.Hours, dtStDate.TimeOfDay.Hours) & IIf(dtStDate.TimeOfDay.Minutes < 10, "0" & dtStDate.TimeOfDay.Minutes, dtStDate.TimeOfDay.Minutes) & "00")
                arRowData.Add(DateToJulian(dtENDate))
                arRowData.Add(Encrypt(txtUID.Text))
                arRowData.Add(Encrypt(TxtPWD.Text))
                arRowData.Add(TxtSource.Text)
                arRowData.Add(txtLimit.Text)
                arRowData.Add(ddlmb.SelectedValue)
                arRowData.Add(DdlMachine.SelectedItem.Text)
                arRowData.Add(Ddldomain.SelectedItem.Text)
                arRowData.Add(intMachineCode)
                arRowData.Add(DDLAlert_DB.SelectedValue)
                arRowData.Add(dtStDate1)
                arRowData.Add(intDomainName)
                arRowData.Add("P")
                arRowData.Add(Session("PropCAComp"))

                Multi.Add("T130022", arColName, arRowData)
                intMax += 1

                dtStDate = dtStDate.AddHours(DDLFrequency.SelectedValue)
                dtStDate = dtStDate.AddMinutes(DdlMin_DB.SelectedValue)
                dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString
                'dtStDate = dtStDate.AddMinutes(dmin_f)
            End While
            Multi.Save()
            Multi.Dispose()

            TxtDB_Name.Text = ""
            ddldb_Type.Items.Clear()
            txtUID.Text = ""
            TxtPWD.Text = ""
            TxtSource.Text = ""
            txtLimit.Text = ""
            ddlHours_F.SelectedIndex = 0
            DdlMins_F.SelectedIndex = 0
            Ddldomain.SelectedIndex = 0
            DDLFrequency.SelectedIndex = 0
            DdlMins_F.SelectedIndex = 0
            ddlmb.SelectedIndex = 0
            dtStartdate_DB.CalendarDate = ""
            dtEnddate_DB.CalendarDate = ""
            DDLAlert_DB.SelectedIndex = 0
            DdlMachine.SelectedIndex = 0
            DdlMin_DB.SelectedIndex = 0
            DdlMachine.Items.Clear()
            Return True
        Catch ex As Exception
            CreateLog("DBMonitor", "SaveDBrequest-661", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
    Private Function DefineDBGridColumnData()
        'define width of columns
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(20)
        arrColWidth.Add(80) 'Db name
        arrColWidth.Add(73) 'type
        arrColWidth.Add(110) 'Machine Name   
        arrColWidth.Add(80) 'start date
        arrColWidth.Add(80) 'End Date
        arrColWidth.Add(80) 'Time
        arrColWidth.Add(80) 'uid
        arrColWidth.Add(82) 'pwd
        arrColWidth.Add(82) 'Source
        arrColWidth.Add(95) 'alert
        arrColWidth.Add(78) 'Limit
        arrColWidth.Add(78) 'Mb/GB
        arrColWidth.Add(50) 'status


        arrTextboxName.Clear()
        arrTextboxName.Add("TxtDBname_s")
        arrTextboxName.Add("TxtDBType_s")
        arrTextboxName.Add("TxtMachine_s")
        arrTextboxName.Add("TxtstartDate_s")
        arrTextboxName.Add("TxtEnddate_s")
        arrTextboxName.Add("TxtTime_s")

        arrTextboxName.Add("Txtuid_s")
        arrTextboxName.Add("TxtPWD_s")
        arrTextboxName.Add("TxtSource_s")
        arrTextboxName.Add("TxtAlert_s")
        arrTextboxName.Add("TxtLimit_s")
        arrTextboxName.Add("TxtMB_s")
        arrTextboxName.Add("Txtstatus_s")


        arrColumnName.Clear()

        arrColumnName.Add("RQ_VC150_CAT2") 'DB Name
        arrColumnName.Add("RQ_VC150_CAT3") 'Db Type
        arrColumnName.Add("RQ_VC150_CAT12") 'Machine
        arrColumnName.Add("RQ_VC150_CAT4") 'StartDate
        arrColumnName.Add("RQ_VC150_CAT6") 'EndDate
        arrColumnName.Add("RQ_VC150_CAT5") 'Time
        arrColumnName.Add("RQ_VC150_CAT7") 'UID
        arrColumnName.Add("RQ_VC150_CAT8") 'PWD
        arrColumnName.Add("RQ_VC150_CAT9") 'Source
        arrColumnName.Add("AM_VC20_Code") 'Alert
        arrColumnName.Add("RQ_VC150_CAT10") 'Limit
        arrColumnName.Add("RQ_VC150_CAT11") 'Mb
        arrColumnName.Add("RQ_CH2_STATUS") 'status
        arrColumnName.Add("RQ_NU9_SQID_PK")

    End Function

    Private Sub DdlDomain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ddldomain.SelectedIndexChanged
        cpnlDBMonitor.State = CustomControls.Web.PanelState.Expanded
        'cpnldatabase enabled fales
        cpnlDBMonitor.Enabled = True
        GetMachine(Ddldomain.SelectedValue)
        FillAlertTypeDropDown()
        GetDBInfo()
        '***************************
        'fill ddlalert Database dropdown list
        DDLAlert_DB.DataSource = dsALTY.Tables(0)
        'textfield as Alert
        DDLAlert_DB.DataTextField = "AM_VC20_Code"
        'Value Save as ID
        DDLAlert_DB.DataValueField = "AM_NU9_AID_PK"
        DDLAlert_DB.DataBind()

        Session("DomainName") = Ddldomain.SelectedItem.Text
        BindGrid(Session("DomainName"))

    End Sub

    Public Function Encrypt(ByVal Data As String) As String
        Dim shaM As SHA1Managed = New SHA1Managed

        System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        '// Getting the bytes of the encrypted data.//
        Dim bytEncrypt() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        '// Converting the byte into string.//
        Dim strEncrypt As String = System.Convert.ToBase64String(bytEncrypt)
        Encrypt = strEncrypt
    End Function

    Public Function Decrypt(ByVal Data As String) As String
        Dim bytData() As Byte = System.Convert.FromBase64String(Data)
        Dim strData As String = ASCIIEncoding.ASCII.GetString(bytData)
        Decrypt = strData
    End Function

    Private Function ValidateDBRequest() As Boolean

        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        Dim currentDate As Date
        currentDate = Date.Now.Today

        If TxtDB_Name.Text.Equals("") Then
            lstError.Items.Add("DB Name cannot be blank...")
            shFlag = 1
        End If

        If ddldb_Type.SelectedItem.Text.Equals("") Then
            lstError.Items.Add("DB Type cannot be blank...")
            shFlag = 1
        End If


        If txtUID.Text.Equals("") Then
            lstError.Items.Add("UserId cannot be blank...")
            shFlag = 1
        End If


        If DDLFrequency.SelectedValue.Equals("0") And DdlMin_DB.SelectedValue.Equals("0") Then
            lstError.Items.Add("Frequency cannot be less then 0...")
            shFlag = 1
        End If

        If TxtSource.Text.Equals("") Then
            lstError.Items.Add("DB Source cannot be blank...")
            shFlag = 1
        End If

        If txtLimit.Text.Equals("") Then
            lstError.Items.Add("Limit cannot be blank...")
            shFlag = 1
        End If

        If dtStartdate_DB.CalendarDate = "" Then
            lstError.Items.Add("Start Date cannot be blank...")
            shFlag = 1
            If dtEnddate_DB.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
            Else
                If CDate(dtStartdate_DB.CalendarDate) > CDate(dtEnddate_DB.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        Else
            If dtEnddate_DB.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
                If CDate(dtStartdate_DB.CalendarDate) < currentDate Then
                    lstError.Items.Add("Start Date cannot be Less than Current date...")
                End If
            Else
                If CDate(dtStartdate_DB.CalendarDate) > CDate(dtEnddate_DB.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        End If



        If shFlag = 1 Then
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If
    End Function

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

    Private Function ReadGrid(ByVal sqid As ArrayList)
        Try
            Dim gridrow As DataGridItem

            For Each gridrow In dgrDBMonitor.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    'add Sqid of checked Cheeckboxes
                    sqid.Add(gridrow.Cells(0).Text)
                End If
            Next
        Catch ex As Exception
        End Try
    End Function

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgrDBMonitor.CurrentPageIndex = (dgrDBMonitor.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindGrid(Session("DomainName"))

    End Sub

    Private Sub DdlMin_DB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgrDBMonitor.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindGrid(Session("DomainName"))

    End Sub

    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgrDBMonitor.CurrentPageIndex > 0) Then
            dgrDBMonitor.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindGrid(Session("DomainName"))

    End Sub

    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgrDBMonitor.CurrentPageIndex < (dgrDBMonitor.PageCount - 1)) Then
            dgrDBMonitor.CurrentPageIndex += 1

            If dgrDBMonitor.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgrDBMonitor.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindGrid(Session("DomainName"))

    End Sub

End Class
