Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data
Imports System.Drawing



Partial Class MonitoringCenter_BGDailyMonitor
    Inherits System.Web.UI.Page

    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private arrSearchText As New ArrayList
    Private mdvBGDailyMonitor As DataView
    'paging variables
    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer
    Dim dvtemp As DataView

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page, "cpnlBGDailyMonitor")
        cpnlError.Visible = False
        'check UpperCase Value
        txtObjectName_F.Attributes.Add("onblur", "javascript: this.value=this.value.toUpperCase();")
        'check numaric value 
        txtReoccur_F.Attributes.Add("onkeypress", "NumericOnly();")
        'check numaric value 
        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();")
        '''paging
        '******************************************
        mintPageSize = Request.Form("cpnlBGDailyMonitor:txtPageSize")
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 20
        End If
        txtPageSize.Text = mintPageSize
        ''textbox objectname and reportdropdown list visible acc to selected value of object type
        '******************************  
        If Not (IsPostBack) Then
            'calling javascript function to fill machine Dropdown
            DdlDomain.Attributes.Add("OnChange", "DomainChange('" & DdlDomain.ClientID & "','" & DdlMachine.ClientID & "');")
            DdlMachine.Items.Add(New ListItem("Select", "0"))
        End If

        If IsPostBack Then
            'fill machine dropdown by selected domain on post back
            FillAjaxDropDown(DdlMachine, Request.Form("txtMachineInfo"), "cpnlBGDailyMonitor:" & DdlMachine.ID, New ListItem("Select", "0"))
        End If

        Try
            If Not IsPostBack Then
                cpnlBGDailyMonitor.Text = "Daily Monitor [" & WSSSearch.SearchCompNameID(HttpContext.Current.Session("PropCAComp")).ExtraValue & "]"
                'set paging index count from zero
                CurrentPg.Text = _currentPageNumber.ToString()
                DefineGridColumnData()
                dtStartDate_F.FastEntry = True
                dtEndDate_F.FastEntry = True
                'Filling PeopleSoft Object Type Dropdown
                FillDropDown(DDLObjectType_F, "select name from UDC where UDCType='POBT'", True, True)
                'fill Alert DRopdown
                FillDropDown(DDLAlert_F, "select AM_NU9_AID_PK, AM_VC20_Code from T180011", False, True)
                'fill TimeLimit dropdown 
                For inti As Integer = 0 To 100
                    DDLTimeLimit_F.Items.Add(inti)
                Next
                'ddlreport dropdown visible false
                ddlReport_f.Visible = False
                GetDomain()
                'call getEnvironment Function
                GetEnvironment()
            Else
                Dim strhiddenImage As String
                strhiddenImage = Request.Form("txthiddenImage")

                If strhiddenImage <> "" Then

                    Select Case strhiddenImage
                        Case "Save"
                            'call saveBGrequest Function
                            If SaveBGRequest() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            End If
                            'End If
                        Case "Delete"
                            'call DeleteBGRequest function
                            If DeleteBGRequest() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            End If
                    End Select

                End If
            End If
            'call BindGrid Function
            BindGrid()
            FormatGrid()
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "Load-170", LogType.Application, LogSubType.Exception, "", ex.ToString)
            lstError.Items.Clear()
            lstError.Items.Add("Please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
        End Try
    End Sub
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill domain dropdown acc to selected company
    '                   Table t170011
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetDomain()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            If SQL.Search("T170011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlDomain dropdown fill acc to company
                DdlDomain.DataSource = dsTemp.Tables(0)
                'domain Name
                DdlDomain.DataTextField = "DM_VC150_DomainName"
                'domainId
                DdlDomain.DataValueField = "DM_NU9_DID_PK"
                DdlDomain.DataBind()
                DdlDomain.Items.Insert(0, New ListItem("Select", "0"))
            Else
                'SQL.Search is False Msgpanel show no domain exist for  selected company
                lstError.Items.Clear()
                lstError.Items.Add("No domain avilable for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetDomain-206", LogType.Application, LogSubType.Exception, "", ex.ToString)
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
            sqstr = "select MM_VC150_Machine_Name ,MM_NU9_MID   from t170012 where MM_NU9_DID_FK=" & Domain & ""
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
            CreateLog("BGDailyMonitor", "GetMachine", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill peoplesoft ENV dropdown acc to selected company
    '                   Table UDC
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetEnvironment()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select name from udc where udctype='PENV' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "Report in people Soft", "GetRole", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlENV dropdown fill acc to selected company
                DdlEnv.DataSource = dsTemp.Tables(0)
                'env Name
                DdlEnv.DataTextField = "name"
                DdlEnv.DataBind()
            Else
                'SQL.Search is False Msgpanel show Msg
                lstError.Items.Clear()
                lstError.Items.Add("No Env avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetEnvironment-268", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill datagrid 
    '                   Table t130022
    'Modify Date:       ------
    '***************************************************************************************
    Private Function BindGrid()
        Try
            Dim strSQL As String
            Dim dsTemp As New DataSet
            Dim dtTemp As New DataTable

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            strSQL = "select "
            dgrBGDailyMonitor.PageSize = mintPageSize ' set the grid page size

            For inti As Integer = 0 To arrColumnName.Count - 1
                'if dataobject TYPE is RPT we select alias name from t130041 acc to sqid
                If inti = 1 Then
                    strSQL &= " (case rq_vc150_cat1 when 'RPT' then (select RP_VC50_AliasName from T130041 where RP_NU9_SQID_PK=rq_vc150_cat2) else rq_vc150_cat2 end) rq_vc150_cat2, "
                Else
                    'if dataobject name is  not RPT then object name select  from t130041 
                    strSQL &= arrColumnName(inti) & ", "
                End If
            Next
            strSQL = strSQL.Trim
            strSQL = strSQL.Substring(0, strSQL.Length - 1)
            strSQL &= " from T130022,T180011 where RQ_NU9_ALERT_FK=AM_NU9_AID_PK and  RQ_NU9_PROCESSID='10020018' and RQ_NU9_ClientID_FK=" & Session("PropCAComp")

            If SQL.Search("T130032", "RPA_Search", "BindGrid", strSQL, dsTemp, "", "") = True Then
                mdvBGDailyMonitor = dsTemp.Tables("T130032").DefaultView
                dvtemp = dsTemp.Tables("T130032").DefaultView ' use for paging
                ModifyDataView()
                '  mdvBGDailyMonitor.RowFilter = GetRowFilter()
                'filterdataview
                GetFilteredDataView(mdvBGDailyMonitor, GetRowFilter)
                'Datagrid fetch data from dataview
                dgrBGDailyMonitor.DataSource = mdvBGDailyMonitor.Table
                GetFilteredDataView(dvtemp, GetRowFilter)
                'Paging
                If (mintPageSize) * (dgrBGDailyMonitor.CurrentPageIndex) > dvtemp.Table.Rows.Count - 1 Then
                    dgrBGDailyMonitor.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                dgrBGDailyMonitor.DataBind()
            Else
                'Dummy ROW In grid
                Dim drTemp As DataRow
                For inti As Integer = 0 To arrColumnName.Count - 1
                    dtTemp.Columns.Add(arrColumnName(inti))
                Next
                drTemp = dtTemp.NewRow
                For intI As Integer = 0 To dtTemp.Columns.Count - 1
                    drTemp.Item(inti) = ""
                Next
                dtTemp.Rows.Add(drTemp)
                dtTemp.AcceptChanges()
                dvtemp = dtTemp.DefaultView ' use for paging
                dgrBGDailyMonitor.DataSource = dtTemp
                'dgrBGDailyMonitor.DataSource = dvtemp
                'FormatGrid()
                ''paging
                '''dvtemp.Table = dtTemp
                '''If (mintPageSize) * (dgrBGDailyMonitor.CurrentPageIndex) > dvtemp.Table.Rows.Count - 1 Then
                '''    dgrBGDailyMonitor.CurrentPageIndex = 0
                CurrentPg.Text = 0
                '''End If
                dgrBGDailyMonitor.DataBind()

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
            End If

            '''

        Catch ex As Exception
            CreateLog("BGDailyMonitor", "BindGrid-368", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function format  datagrid 
    '                   Table t130022
    'Modify Date:       ------
    '***************************************************************************************
    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrBGDailyMonitor.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrBGDailyMonitor.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrBGDailyMonitor.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "FormatGrid-133", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       12/11/2006
    'Purpose:           This function is used  to get the text from search textboxes in                         an array list
    'Modify Date:       ------
    '***********************************************************************************
    Private Function GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlBGDailyMonitor:dgrBGDailyMonitor:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetSeacrhText-413", LogType.Application, LogSubType.Exception, "", ex.ToString)
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
            For inti As Integer = 0 To mdvBGDailyMonitor.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If mdvBGDailyMonitor.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf mdvBGDailyMonitor.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("BGDailyMonitor", "GetRowFilter-456", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function
    Private Sub dgrBGDailyMonitor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrBGDailyMonitor.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(mdvBGDailyMonitor) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To mdvBGDailyMonitor.Table.Columns.Count - 2
                        'checking value of status if it is not P
                        If CType(e.Item.Cells(13).FindControl("lblStatus"), Label).Text.Trim <> "P" Then
                            'checkbox enabled is false
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = False
                        Else
                            'checkbox enabled is True
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = True
                        End If

                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        'Here check value of status if it is P or H

                        If CType(e.Item.Cells(13).FindControl("lblStatus"), Label).Text.Trim = "P" Or CType(e.Item.Cells(13).FindControl("lblStatus"), Label).Text.Trim = "H" Then
                            'open popup window
                            e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        Else
                            'message
                            e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck56();")
                        End If
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
                        CType(txt, TextBox).Text = Request.Form("cpnlBGDailyMonitor:dgrBGDailyMonitor:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "dgrBGDailyMonitor_ItemDataBound-501", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       11/01/2007
    'Purpose:           This function is used to bind grid
    'Table              t130022
    'Modify Date:       ------
    '***********************************************************************************
    '
    Private Function ModifyDataView()
        Try

            For inti As Integer = 0 To mdvBGDailyMonitor.Table.Rows.Count - 1
                'change julian date to show in grid which we fetch from database 
                mdvBGDailyMonitor.Table.Rows(inti).Item("RQ_VC150_CAT4") = JulianToDate(mdvBGDailyMonitor.Table.Rows(inti).Item("RQ_VC150_CAT4")).ToShortDateString
                'change julian date  to show in grid which we fetch from database
                mdvBGDailyMonitor.Table.Rows(inti).Item("RQ_VC150_CAT7") = JulianToDate(mdvBGDailyMonitor.Table.Rows(inti).Item("RQ_VC150_CAT7")).ToShortDateString
            Next
            mdvBGDailyMonitor.Table.AcceptChanges()
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "ModifyDataView-523", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to DefineGrid Column Data 
    'Modify Date:       ------
    '***************************************************************************************
    Private Function DefineGridColumnData()
        'define width of columns
        arrColWidth.Clear()
        arrColWidth.Add(0) 'SQID
        arrColWidth.Add(20) 'check BOx
        arrColWidth.Add(50)
        arrColWidth.Add(100)
        arrColWidth.Add(85)
        arrColWidth.Add(85) 'End Date
        arrColWidth.Add(80) 'Time
        arrColWidth.Add(70) 'Reoccur
        arrColWidth.Add(110) 'Alert
        arrColWidth.Add(75) 'Time Limit
        arrColWidth.Add(100) 'Status Check
        arrColWidth.Add(80) 'status alert
        arrColWidth.Add(80) 'Time Check
        arrColWidth.Add(80) 'Machine
        arrColWidth.Add(50) 'Env
        arrColWidth.Add(50) 'Status

        arrTextboxName.Clear()
        arrTextboxName.Add("txtObjectType")
        arrTextboxName.Add("txtObjectName")
        arrTextboxName.Add("txtStartDate")
        arrTextboxName.Add("txtEndDate")
        arrTextboxName.Add("txtTime")
        arrTextboxName.Add("txtReoccur")
        arrTextboxName.Add("txtAlert")
        arrTextboxName.Add("txtTimeLimit")
        arrTextboxName.Add("txtStatusCheck")
        arrTextboxName.Add("txtStatusAlert")
        arrTextboxName.Add("txtTimeCheck")
        arrTextboxName.Add("txtMachine")
        arrTextboxName.Add("txtEnv")
        arrTextboxName.Add("txtStatus")

        arrColumnName.Clear()
        arrColumnName.Add("RQ_VC150_CAT1") 'Object Type
        arrColumnName.Add("RQ_VC150_CAT2") 'Object Name
        arrColumnName.Add("RQ_VC150_CAT4") 'Start Date
        arrColumnName.Add("RQ_VC150_CAT7") 'End Date
        arrColumnName.Add("RQ_VC150_CAT5") 'Time
        arrColumnName.Add("RQ_VC150_CAT3") 'Reoccur
        arrColumnName.Add("AM_VC20_Code") 'Alert
        arrColumnName.Add("RQ_VC150_CAT8") 'Time Limit
        arrColumnName.Add("RQ_VC150_CAT9") 'Status Check
        arrColumnName.Add("RQ_VC150_CAT10") 'Status Alert
        arrColumnName.Add("RQ_VC150_CAT11") 'Time Check
        arrColumnName.Add("RQ_VC150_CAT12") 'Machine
        arrColumnName.Add("RQ_VC150_CAT13") 'Env
        arrColumnName.Add("RQ_CH2_STATUS") 'Status
        arrColumnName.Add("RQ_NU9_SQID_PK")

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill Object Type dropdown 
    'Table              UDC 
    'Modify Date:       ------
    '***************************************************************************************
    Public Function FILLDropDownUDC() As System.Data.DataTable
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        Dim dsTemp As New DataSet
        'sqstr hold SqlQuery
        Dim sqstr As String
        sqstr = "select name from UDC where UDCType='OBTY'"
        ' This function will fetch Object Type where UDCType='OBTY'
        If SQL.Search("UDC", "dataobjentry", "FILLDropDown", sqstr, dsTemp, "", "") = True Then
            Return dsTemp.Tables("udc")
        Else
            Return Nothing
        End If

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill Alert dropdown acc to selected company
    '                   Table t180011
    'Modify Date:       ------
    '***************************************************************************************
    Public Function FILLDropDownAlert() As System.Data.DataTable
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        Dim dsTemp As New DataSet
        Dim sqstr As String
        sqstr = "select * from T180011 "
        ' This function will fetch all the alerts from t180011
        If SQL.Search("T180011", "dataobjentry", "FILLDropDown", sqstr, dsTemp, "", "") = True Then
            Return dsTemp.Tables("T180011")
        Else
            Return Nothing
        End If

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill report dropdown acc to company 
    '                   Table t130041
    'Modify Date:       ------
    '***************************************************************************************
    Private Function FillReportDropDown()
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select RP_VC50_AliasName ,RP_NU9_SQID_PK from t130041 where RP_NU9_CompanyID_FK=" & Session("PropCAComp")
            If SQL.Search("T130041 ", "BGDailyMonitor", "FILLReportDropDown", sqstr, dsTemp, "", "") = True Then
                'dvSearch = dsTemp.Tables(0).DefaultView
                'SQL.Search is true then ddlreport_f dropdown fill acc to selected company
                ddlReport_f.DataSource = dsTemp.Tables(0)
                'Report Name
                ddlReport_f.DataTextField = "RP_VC50_AliasName"
                'Report ID
                ddlReport_f.DataValueField = "RP_NU9_SQID_PK"
                ddlReport_f.DataBind()
            Else
                'SQL.Search is False msg panel show msg
                lstError.Items.Clear()
                lstError.Items.Add("No Report Name Exist For This Company...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If

        Catch ex As Exception
            CreateLog("BGDailyMonitor", "FillReportDropdown-663", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Function

    Public Function FillDropDown(ByVal ddlCustom As DropDownList, ByVal strSQL As String, Optional ByVal UDC As Boolean = False, Optional ByVal OptionalField As Boolean = False)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            If OptionalField = True Then
                ddlCustom.Items.Add(New ListItem("", ""))
            End If
            If blnStatus = True Then
                While sqRDR.Read
                    If UDC = True Then
                        ddlCustom.Items.Add(New ListItem(sqRDR(0), sqRDR(0)))
                    Else
                        ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                    End If
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "FillDropDown", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       12/01/2007
    'Purpose:           This function is used  to Save data
    '                   Table t130022
    'Modify Date:       ------
    '***************************************************************************************
    Private Function SaveBGRequest() As Boolean
        Try
            If ValidateRequest() = False Then
                Exit Function
            End If

            Dim Multi As SQL.AddMultipleRows
            Dim strDomainName As String
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020018'"
            'reader
            sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold domainname
                strDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
                'hold machineCode
                intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
                sqRDR.Close()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Domain is available for this process...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            Dim dtStart As Date
            Dim dtEnd As Date
            Dim intDays As Integer
            'hold start date
            dtStart = dtStartDate_F.CalendarDate()
            'hold end date
            dtEnd = dtEndDate_F.CalendarDate
            intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            Dim intMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            Dim dtStDate1 As String
            'hold start date ,hours and min
            Dim dtStDate As DateTime
            Dim dtENDate As DateTime
            dtStDate = CDate(CDate(dtStartDate_F.CalendarDate).ToShortDateString & " " & DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
            Dim dttime As DateTime = CDate(DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
            dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString
            'dtStDate1 = dtStartDate_F.CalendarDate & " " & dttime.ToShortTimeString
            dtENDate = CDate(CDate(dtEndDate_F.CalendarDate).ToShortDateString & " " & DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
            Dim intROF As Integer

            While dtStDate <= dtENDate
                'define column name
                arColName.Add("RQ_NU9_SQID_PK")
                arColName.Add("RQ_NU9_PROCESSID")
                arColName.Add("RQ_VC150_CAT1") 'ObjectType
                'arColName.Add("RQ_VC150_CAT2") 'ObjectName
                arColName.Add("RQ_VC100_REQUEST_DATE")
                arColName.Add("RQ_NU9_ALERT_FK")
                arColName.Add("RQ_NU9_Domain_FK")
                arColName.Add("RQ_NU9_Machine_Code_FK")
                arColName.Add("RQ_CH2_STATUS")
                'arColName.Add("RQ_VC150_CAT8") 'Time Limit
                arColName.Add("RQ_VC150_CAT9") 'Status Check
                'arColName.Add("RQ_VC150_CAT10") 'Status Alert
                'arColName.Add("RQ_VC150_CAT11") 'Time Check

                'arColName.Add("RQ_VC150_CAT3") 'Reoccur
                arColName.Add("RQ_VC150_CAT4") 'StartDate
                arColName.Add("RQ_VC150_CAT5") 'StartTime
                arColName.Add("RQ_VC150_CAT6") 'EndTime
                arColName.Add("RQ_VC150_CAT7") 'EndDate
                arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

                arRowData.Add(intMax)
                arRowData.Add("10020018")
                arRowData.Add(DDLObjectType_F.SelectedValue)
                'If DDLObjectType_F.SelectedValue.Equals("RPT") Then
                'arRowData.Add(ddlReport_f.SelectedValue)
                'Else
                '    arRowData.Add(txtObjectName_F.Text)
                'End If
                arRowData.Add(dtStDate1)
                arRowData.Add(DDLAlert_F.SelectedValue)
                arRowData.Add(strDomainName)
                arRowData.Add(intMachineCode)
                arRowData.Add("P")
                'arRowData.Add(DDLTimeLimit_F.SelectedValue)
                arRowData.Add(DDLStatusCheck_F.SelectedValue)
                'arRowData.Add(DDLStatusalert_F.SelectedValue)
                'arRowData.Add(DDLTimeCheck_F.SelectedValue)

                ''intROF = (DDLFrequency.SelectedValue * 60) / txtReoccur_F.Text.Trim
                ''arRowData.Add(intROF)
                'arRowData.Add(txtReoccur_F.Text)
                arRowData.Add(DateToJulian(dtStDate))
                arRowData.Add(IIf(dtStDate.TimeOfDay.Hours < 10, "0" & dtStDate.TimeOfDay.Hours, dtStDate.TimeOfDay.Hours) & IIf(dtStDate.TimeOfDay.Minutes < 10, "0" & dtStDate.TimeOfDay.Minutes, dtStDate.TimeOfDay.Minutes) & "00")
                Dim dtTemp As DateTime
                dtTemp = dtStDate.AddHours(-DDLFrequency.SelectedValue)
                dtTemp = dtTemp.AddMinutes(-Ddlmin.SelectedValue)

                arRowData.Add(IIf(dtTemp.TimeOfDay.Hours < 10, "0" & dtTemp.TimeOfDay.Hours, dtTemp.TimeOfDay.Hours) & IIf(dtTemp.TimeOfDay.Minutes < 10, "0" & dtTemp.TimeOfDay.Minutes, dtTemp.TimeOfDay.Minutes) & "00")

                arRowData.Add(DateToJulian(dtTemp.Date))
                arRowData.Add(Session("PropCAComp"))
                'define and save data when Object Typeis RPT
                If DDLObjectType_F.SelectedValue.Equals("RPT") Then
                    arColName.Add("RQ_VC150_CAT2") 'ObjectName
                    arColName.Add("RQ_VC150_CAT8") 'Time Limit
                    arColName.Add("RQ_VC150_CAT10") 'Status Alert
                    arColName.Add("RQ_VC150_CAT11") 'Time Check
                    arColName.Add("RQ_VC150_CAT3") 'Reoccur

                    arRowData.Add(ddlReport_f.SelectedValue)
                    arRowData.Add(DDLTimeLimit_F.SelectedValue)
                    arRowData.Add(DDLStatusalert_F.SelectedValue)
                    arRowData.Add(DDLTimeCheck_F.SelectedValue)
                    arRowData.Add(txtReoccur_F.Text)
                    'define and save data when Object Typeis QUE
                ElseIf DDLObjectType_F.SelectedValue.Equals("QUE") Then
                    arColName.Add("RQ_VC150_CAT2") 'ObjectName
                    arColName.Add("RQ_VC150_CAT8") 'Time Limit
                    arColName.Add("RQ_VC150_CAT10") 'Status Alert
                    arColName.Add("RQ_VC150_CAT11") 'Time Check
                    arColName.Add("RQ_VC150_CAT3") 'Reoccur
                    arColName.Add("RQ_VC150_CAT12") 'Machine Name
                    arColName.Add("RQ_VC150_CAT13") 'EnV

                    arRowData.Add(txtObjectName_F.Text.Trim)
                    arRowData.Add(DDLTimeLimit_F.SelectedValue)
                    arRowData.Add(DDLStatusalert_F.SelectedValue)
                    arRowData.Add(DDLTimeCheck_F.SelectedValue)
                    arRowData.Add(txtReoccur_F.Text)
                    arRowData.Add(DdlMachine.SelectedItem.Text) 'Machine
                    arRowData.Add(DdlEnv.SelectedValue) 'ENV
                    'define and save data when Object Typeis STS
                Else
                    arColName.Add("RQ_VC150_CAT12") 'Machine Name
                    arColName.Add("RQ_VC150_CAT13") 'EnV

                    arRowData.Add(DdlMachine.SelectedItem.Text) 'Machine
                    arRowData.Add(DdlEnv.SelectedValue) 'ENV
                End If
                'add Multiple rows in t130022
                Multi.Add("T130022", arColName, arRowData)
                arColName.Clear()
                'increment int max
                intMax += 1
                dtStDate = dtStDate.AddHours(DDLFrequency.SelectedValue)
                dtStDate = dtStDate.AddMinutes(Ddlmin.SelectedValue)
                dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString
            End While
            Multi.Save()
            ClearFastEntry()
            'DDLTimeLimit_F.SelectedIndex = 0
            'DDLAlert_F.SelectedIndex = 0
            'DDLStatusCheck_F.SelectedIndex = 0
            'DDLStatusalert_F.SelectedIndex = 0
            'DDLTimeCheck_F.SelectedIndex = 0
            'txtObjectName_F.Text = ""
            'txtReoccur_F.Text = ""
            Return True
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "SaveBGRequest-862", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
            Return False
        End Try

    End Function
    '**************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function check all textboxes are fill before save Function 
    'Modify Date:       ------
    '***************************************************************************************

    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        Dim currentDate As Date
        currentDate = Date.Now.Today

        If DDLObjectType_F.SelectedValue.Equals("") Then
            lstError.Items.Add("Object Type cannot be blank...")
            shFlag = 1
        End If

        If DDLObjectType_F.SelectedValue.Equals("QUE") Then
            If txtObjectName_F.Text.Equals("") Then
                lstError.Items.Add("Object Name cannot be blank...")
                shFlag = 1
            End If
        End If

        'If dtStartDate_F.CalendarDate = "" Then
        '    lstError.Items.Add("Start Date cannot be blank...")
        '    shFlag = 1
        'Else
        '    If CDate(dtStartDate_F.CalendarDate) < currentDate Then
        '        lstError.Items.Add("Start Date cannot be Less than Current date...")
        '        shFlag = 1
        '    End If
        'End If

        'If dtEndDate_F.CalendarDate = "" Then
        '    lstError.Items.Add("End Date cannot be blank...")
        '    shFlag = 1
        'End If

        If DDLAlert_F.SelectedValue.Equals("") Then
            lstError.Items.Add("Alert Type cannot be blank...")
            shFlag = 1
        End If

        If DDLObjectType_F.SelectedValue.Equals("RPT") Or DDLObjectType_F.SelectedValue.Equals("QUE") Then
            If txtReoccur_F.Text.Equals("") Then
                lstError.Items.Add("Reoccur frequency cannot be blank...")
                shFlag = 1
            End If
        End If

        If DDLObjectType_F.SelectedValue.Equals("STS") Or DDLObjectType_F.SelectedValue.Equals("QUE") Then
            If DdlMachine.SelectedValue.Equals("0") Then
                lstError.Items.Add("Machine Type cannot be blank...")
                shFlag = 1
            End If
        End If

        If DDLFrequency.SelectedValue.Equals("0") And Ddlmin.SelectedValue.Equals("0") Then
            lstError.Items.Add("Frequency cannot be less then 0...")
            shFlag = 1
        End If

        'If CDate(dtStartDate_F.CalendarDate) > CDate(dtEndDate_F.CalendarDate) Then
        '    lstError.Items.Add("End Date cannot be less than Start Date...")
        '    shFlag = 1
        'End If
        If dtStartDate_F.CalendarDate = "" Then
            lstError.Items.Add("Start Date cannot be blank...")
            shFlag = 1
            If dtEndDate_F.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
            Else
                If CDate(dtStartDate_F.CalendarDate) > CDate(dtEndDate_F.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        Else
            If dtEndDate_F.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
                If CDate(dtStartDate_F.CalendarDate) < currentDate Then
                    lstError.Items.Add("Start Date cannot be Less than Current date...")
                End If
            Else
                If CDate(dtStartDate_F.CalendarDate) > CDate(dtEndDate_F.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        End If


        'if shFlag =1 then any field is empty and msg panel show msg 
        If shFlag = 1 Then
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            ' shFlag =0 then no field is empty 
            Return True
        End If
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       4/12/2006 
    'Purpose:           This function Delete Multiple rows from grid 
    '                   Table t130022
    'Modify Date:       8/01/2006
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
            If SQL.Delete("BGDailyMonitor", "Delete", strDeleteSQL, SQL.Transaction.ReadCommitted, "") = True Then
                Return True
            Else
                Return False
                lstError.Items.Clear()
                lstError.Items.Add("sorry your Requst is not proceesed Now...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "DeleteBGRequest-973", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
            Return False
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       8/01/2007 
    'Purpose:           This function Readchecked row in grid to add SQID  in array  
    '                   Table t130022
    'Modify Date:       -------
    '***************************************************************************************
    Private Function ReadGrid(ByVal sqid As ArrayList)
        Try
            Dim gridrow As DataGridItem

            For Each gridrow In dgrBGDailyMonitor.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    'add Sqid of checked Cheeckboxes
                    sqid.Add(gridrow.Cells(0).Text)
                End If
            Next
        Catch ex As Exception
        End Try
    End Function
    '*****************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function clear fast entry text  after save  and reset dropdown
    'Modify Date:       ------
    '*****************************************************************************************
    Private Function ClearFastEntry()
        Try
            'clear text boxes after Save Function
            txtObjectName_F.Text = ""
            txtReoccur_F.Text = ""
            DDLAlert_F.SelectedIndex = -1
            DDLFrequency.SelectedIndex = -1
            DDLHours_F.SelectedIndex = -1
            DDLMins_F.SelectedIndex = -1
            DDLObjectType_F.SelectedIndex = -1
            DDLStatusalert_F.SelectedIndex = -1
            DDLStatusCheck_F.SelectedIndex = -1
            DDLTimeCheck_F.SelectedIndex = -1
            DDLTimeLimit_F.SelectedIndex = -1
            dtEndDate_F.CalendarDate = ""
            dtStartDate_F.CalendarDate = ""
            DdlMachine.Items.Clear()
            DdlDomain.SelectedIndex = -1
            DdlEnv.SelectedIndex = -1
            Ddlmin.SelectedIndex = -1
            txtObjectName_F.Visible = True
            ddlReport_f.Visible = False
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "ClearFastEntry-1027", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgrBGDailyMonitor.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindGrid()
    End Sub
    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgrBGDailyMonitor.CurrentPageIndex > 0) Then
            dgrBGDailyMonitor.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindGrid()
    End Sub
    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgrBGDailyMonitor.CurrentPageIndex < (dgrBGDailyMonitor.PageCount - 1)) Then
            dgrBGDailyMonitor.CurrentPageIndex += 1

            If dgrBGDailyMonitor.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgrBGDailyMonitor.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindGrid()
    End Sub
    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgrBGDailyMonitor.CurrentPageIndex = (dgrBGDailyMonitor.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindGrid()
    End Sub

    Private Sub DDLObjectType_F_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLObjectType_F.SelectedIndexChanged

        txtObjectName_F.Enabled = True
        txtReoccur_F.Enabled = True
        DDLStatusalert_F.Enabled = True
        DDLTimeLimit_F.Enabled = True
        DDLTimeCheck_F.Enabled = True
        DdlDomain.Enabled = True
        DdlMachine.Enabled = True
        DdlEnv.Enabled = True
        'if object type is RPT then  text boxes enable or disable and color change acc to that
        If DDLObjectType_F.SelectedValue.Equals("RPT") Then
            ddlReport_f.Visible = True
            FillReportDropDown()
            txtObjectName_F.Visible = False
            DDLTimeCheck_F.BackColor = Color.FromArgb(227, 240, 225)
            txtReoccur_F.BackColor = Color.FromArgb(227, 240, 225)
            DDLStatusalert_F.BackColor = Color.FromArgb(227, 240, 225)
            DDLTimeLimit_F.BackColor = Color.FromArgb(227, 240, 225)
            DdlDomain.Enabled = False
            DdlDomain.BackColor = Color.LightGray
            DdlMachine.Enabled = False
            DdlMachine.BackColor = Color.LightGray
            DdlEnv.Enabled = False
            DdlEnv.BackColor = Color.LightGray
            ' if object type is STS then following  text boxes enable or disable and color change acc to that
        ElseIf DDLObjectType_F.SelectedValue.Equals("STS") Then
            ddlReport_f.Visible = False
            txtObjectName_F.Visible = True
            txtObjectName_F.Enabled = False
            txtObjectName_F.BackColor = Color.LightGray
            txtReoccur_F.Enabled = False
            txtReoccur_F.BackColor = Color.LightGray
            DDLStatusalert_F.Enabled = False
            DDLStatusalert_F.BackColor = Color.LightGray
            DDLTimeLimit_F.Enabled = False
            DDLTimeLimit_F.BackColor = Color.LightGray
            DDLTimeCheck_F.Enabled = False
            DDLTimeCheck_F.BackColor = Color.LightGray
            DdlDomain.BackColor = Color.White
            DdlMachine.BackColor = Color.FromArgb(227, 240, 225)
            DdlEnv.BackColor = Color.FromArgb(227, 240, 225)
        Else
            ' if object type is not STS OR RPT  then following  text boxes enable or disable acc to that
            ddlReport_f.Visible = False
            txtObjectName_F.Visible = True
            DDLTimeCheck_F.BackColor = Color.FromArgb(227, 240, 225)
            txtReoccur_F.BackColor = Color.FromArgb(227, 240, 225)
            DDLStatusalert_F.BackColor = Color.FromArgb(227, 240, 225)
            DDLTimeLimit_F.BackColor = Color.FromArgb(227, 240, 225)
            txtObjectName_F.BackColor = Color.FromArgb(227, 240, 225)
            DdlDomain.BackColor = Color.White
            DdlMachine.BackColor = Color.FromArgb(227, 240, 225)
            DdlEnv.BackColor = Color.FromArgb(227, 240, 225)
        End If
    End Sub
    '/****************************************
    'not in use

    'If DDLObjectType_F.SelectedValue.Equals("STS") Then
    '    If SaveSTSRequest() = True Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("Record saved successfully...")
    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
    '    End If
    'ElseIf DDLObjectType_F.SelectedValue.Equals("QUE") Then
    '    If SaveQUERequest() = True Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("Record saved successfully...")
    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
    '    End If
    'Else

    'Private Sub DdlDomain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlDomain.SelectedIndexChanged
    '    If DdlDomain.SelectedIndex.Equals(0) Then
    '        lstError.Items.Add("Select domain")
    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
    '        DdlMachine.Items.Clear()
    '    Else
    '        cpnlError.Visible = False
    '        DdlMachine.Items.Clear()
    '        GetMachine(DdlDomain.SelectedValue)
    '    End If
    'End Sub
    'Private Function ValidateRequest() As Boolean
    '    Dim shFlag As Short
    '    shFlag = 0
    '    lstError.Items.Clear()
    '    Dim currentDate As Date
    '    currentDate = Date.Now.Today

    '    If DDLObjectType_F.SelectedValue.Equals("RPT") Then
    '        If ddlReport_f.SelectedValue.Equals("") Then
    '            lstError.Items.Add("Report Name cannot be blank...")
    '            shFlag = 1
    '        End If
    '    Else
    '        If txtObjectName_F.Text.Equals("") Then
    '            lstError.Items.Add("Object Name cannot be blank...")
    '            shFlag = 1
    '        End If
    '    End If

    '    If dtStartDate_F.CalendarDate = "" Then
    '        lstError.Items.Add("Start Date cannot be blank...")
    '        shFlag = 1
    '    Else
    '        If dtStartDate_F.CalendarDate < currentDate Then
    '            lstError.Items.Add("Start Date cannot be Less than Current date...")
    '            shFlag = 1
    '        End If
    '    End If
    '    If dtEndDate_F.CalendarDate = "" Then
    '        lstError.Items.Add("End Date cannot be blank...")
    '        shFlag = 1
    '    End If

    '    If DDLFrequency.SelectedValue.Equals("0") And Ddlmin.SelectedValue.Equals("0") Then
    '        lstError.Items.Add("Frequency cannot be less then 0...")
    '        shFlag = 1
    '    End If


    '    If DDLObjectType_F.SelectedValue.Equals("") Then
    '        lstError.Items.Add("Object Type cannot be blank...")
    '        shFlag = 1
    '    End If
    '    If txtReoccur_F.Text.Equals("") Then
    '        lstError.Items.Add("Reoccur frequency cannot be blank...")
    '        shFlag = 1
    '    End If
    '    If DDLAlert_F.SelectedValue.Equals("") Then
    '        lstError.Items.Add("Alert Type cannot be blank...")
    '        shFlag = 1
    '    End If
    '    If dtStartDate_F.CalendarDate > dtEndDate_F.CalendarDate Then
    '        lstError.Items.Add("End Date cannot be less than Start Date...")
    '        shFlag = 1
    '    End If
    '    If shFlag = 1 Then
    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
    '        Return False
    '    Else
    '        Return True
    '    End If

    'End Function
    'Private Function SaveQUERequest() As Boolean

    '    Try

    '        If ValidateQUERequest() = False Then
    '            Exit Function
    '        End If
    '        Dim Multi As SQL.AddMultipleRows
    '        Dim strDomainName As String
    '        Dim intMachineCode As Integer

    '        Dim sqRDR As SqlClient.SqlDataReader
    '        Dim blnStatus As Boolean
    '        Dim strSQL As String
    '        strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020018'"
    '        sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
    '        If blnStatus = True Then
    '            sqRDR.Read()
    '            strDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
    '            intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
    '            sqRDR.Close()
    '        Else
    '            lstError.Items.Clear()
    '            lstError.Items.Add("No Domain is available for this process...")
    '            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
    '            Exit Function
    '        End If

    '        Dim dtStart As Date
    '        Dim dtEnd As Date
    '        Dim intDays As Integer
    '        dtStart = dtStartDate_F.CalendarDate()
    '        dtEnd = dtEndDate_F.CalendarDate
    '        intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)


    '        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
    '        Dim arColName As New ArrayList
    '        Dim arRowData As New ArrayList

    '        arColName.Add("RQ_NU9_SQID_PK")
    '        arColName.Add("RQ_NU9_PROCESSID")
    '        arColName.Add("RQ_VC150_CAT1") 'ObjectType
    '        arColName.Add("RQ_VC150_CAT2") 'ObjectName
    '        arColName.Add("RQ_VC100_REQUEST_DATE")
    '        arColName.Add("RQ_NU9_ALERT_FK")

    '        arColName.Add("RQ_NU9_Domain_FK")
    '        arColName.Add("RQ_NU9_Machine_Code_FK")
    '        arColName.Add("RQ_CH2_STATUS")
    '        arColName.Add("RQ_VC150_CAT8") 'Time Limit
    '        arColName.Add("RQ_VC150_CAT9") 'Status Check
    '        arColName.Add("RQ_VC150_CAT10") 'Status Alert
    '        arColName.Add("RQ_VC150_CAT11") 'Time Check
    '        arColName.Add("RQ_VC150_CAT3") 'Reoccur
    '        arColName.Add("RQ_VC150_CAT12") 'Machine
    '        arColName.Add("RQ_VC150_CAT13") 'ENV           
    '        arColName.Add("RQ_VC150_CAT4") 'StartDate
    '        arColName.Add("RQ_VC150_CAT5") 'StartTime
    '        arColName.Add("RQ_VC150_CAT6") 'EndTime
    '        arColName.Add("RQ_VC150_CAT7") 'EndDate
    '        arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

    '        Dim intMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
    '        intMax += 1

    '        Dim dtStDate As DateTime
    '        Dim dtENDate As DateTime
    '        dtStDate = CDate(CDate(dtStartDate_F.CalendarDate).ToShortDateString & " " & DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
    '        dtENDate = CDate(CDate(dtEndDate_F.CalendarDate).ToShortDateString & " " & DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
    '        Dim intROF As Integer

    '        While dtStDate <= dtENDate


    '            arRowData.Add(intMax)
    '            arRowData.Add("10020018")
    '            arRowData.Add(DDLObjectType_F.SelectedValue)

    '            arRowData.Add(txtObjectName_F.Text)

    '            arRowData.Add(dtStDate)
    '            arRowData.Add(DDLAlert_F.SelectedValue)
    '            arRowData.Add(strDomainName)
    '            arRowData.Add(intMachineCode)
    '            arRowData.Add("P")
    '            arRowData.Add(DDLTimeLimit_F.SelectedValue)
    '            arRowData.Add(DDLStatusCheck_F.SelectedValue)
    '            arRowData.Add(DDLStatusalert_F.SelectedValue)
    '            arRowData.Add(DDLTimeCheck_F.SelectedValue)

    '            'intROF = (DDLFrequency.SelectedValue * 60) / txtReoccur_F.Text.Trim
    '            'arRowData.Add(intROF)
    '            arRowData.Add(txtReoccur_F.Text)
    '            arRowData.Add(DdlMachine.SelectedItem.Text) 'Machine
    '            arRowData.Add(DdlEnv.SelectedValue) 'ENV

    '            arRowData.Add(DateToJulian(dtStDate))
    '            arRowData.Add(IIf(dtStDate.TimeOfDay.Hours < 10, "0" & dtStDate.TimeOfDay.Hours, dtStDate.TimeOfDay.Hours) & IIf(dtStDate.TimeOfDay.Minutes < 10, "0" & dtStDate.TimeOfDay.Minutes, dtStDate.TimeOfDay.Minutes) & "00")
    '            Dim dtTemp As DateTime
    '            dtTemp = dtStDate.AddHours(-DDLFrequency.SelectedValue)
    '            dtTemp = dtTemp.AddMinutes(-Ddlmin.SelectedValue)

    '            arRowData.Add(IIf(dtTemp.TimeOfDay.Hours < 10, "0" & dtTemp.TimeOfDay.Hours, dtTemp.TimeOfDay.Hours) & IIf(dtTemp.TimeOfDay.Minutes < 10, "0" & dtTemp.TimeOfDay.Minutes, dtTemp.TimeOfDay.Minutes) & "00")

    '            arRowData.Add(DateToJulian(dtTemp.Date))
    '            arRowData.Add(Session("PropCAComp"))

    '            Multi.Add("T130022", arColName, arRowData)
    '            intMax += 1
    '            dtStDate = dtStDate.AddHours(DDLFrequency.SelectedValue)
    '            dtStDate = dtStDate.AddMinutes(Ddlmin.SelectedValue)
    '        End While
    '        Multi.Save()
    '        ClearFastEntry()
    '        'DDLTimeLimit_F.SelectedIndex = 0
    '        'DDLAlert_F.SelectedIndex = 0
    '        'DDLStatusCheck_F.SelectedIndex = 0
    '        'DDLStatusalert_F.SelectedIndex = 0
    '        'DDLTimeCheck_F.SelectedIndex = 0
    '        'txtObjectName_F.Text = ""
    '        'txtReoccur_F.Text = ""
    '        Return True

    '    Catch ex As Exception

    '        CreateLog("BGDailyMonitor", "SaveBGRequest", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
    '        Return False
    '    End Try

    'End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function check all textboxes are fill before save STSRequest 
    'Modify Date:       ------
    '***************************************************************************************

    'Private Function ValidateSTSRequest() As Boolean
    '    Dim shFlag As Short
    '    shFlag = 0
    '    lstError.Items.Clear()
    '    Dim currentDate As Date
    '    currentDate = Date.Now.Today


    '    If dtStartDate_F.CalendarDate = "" Then
    '        lstError.Items.Add("Start Date cannot be blank...")
    '        shFlag = 1
    '    Else
    '        If dtStartDate_F.CalendarDate < currentDate Then
    '            lstError.Items.Add("Start Date cannot be Less than Current date...")
    '            shFlag = 1
    '        End If
    '    End If


    '    If DDLStatusCheck_F.SelectedValue = "" Then
    '        lstError.Items.Add("Select Status...")
    '        shFlag = 1
    '    End If

    '    If dtEndDate_F.CalendarDate = "" Then
    '        lstError.Items.Add("End Date cannot be blank...")
    '        shFlag = 1
    '    End If
    '    If DDLObjectType_F.SelectedValue.Equals("") Then
    '        lstError.Items.Add("Object Type cannot be blank...")
    '        shFlag = 1
    '    End If

    '    If DdlMachine.SelectedValue.Equals("") Then
    '        lstError.Items.Add("Machine Name cannot be blank...")
    '        shFlag = 1
    '    End If

    '    If DDLFrequency.SelectedValue.Equals("0") And Ddlmin.SelectedValue.Equals("0") Then
    '        lstError.Items.Add("Frequency cannot be less then 0...")
    '        shFlag = 1
    '    End If

    '    If DDLAlert_F.SelectedValue.Equals("") Then
    '        lstError.Items.Add("Alert Type cannot be blank...")
    '        shFlag = 1
    '    End If
    '    If dtStartDate_F.CalendarDate > dtEndDate_F.CalendarDate Then
    '        lstError.Items.Add("End Date cannot be less than Start Date...")
    '        shFlag = 1
    '    End If


    '    'if shFlag =1 then any field is empty and msg panel show msg 
    '    If shFlag = 1 Then
    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
    '        Return False
    '    Else
    '        ' shFlag =0 then no field is empty 
    '        Return True
    '    End If

    'End Function
    '*****************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function save STSrequest in table t130022 
    'Modify Date:       ------
    '*****************************************************************

    'Private Function SaveSTSRequest() As Boolean
    '    Try
    '        If ValidateSTSRequest() = False Then
    '            Exit Function
    '        End If
    '        Dim Multi As SQL.AddMultipleRows
    '        'this string  to hold value of Domain id
    '        Dim strDomainName As String
    '        'intMachineCode used to hold value of Machine id
    '        Dim intMachineCode As Integer

    '        Dim sqRDR As SqlClient.SqlDataReader
    '        Dim blnStatus As Boolean
    '        Dim strSQL As String
    '        'strSQL select domain id and machine id  from t130033 to save these id in t130022 
    '        strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020018'"
    '        sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
    '        If blnStatus = True Then
    '            sqRDR.Read()
    '            strDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
    '            intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
    '            sqRDR.Close()
    '        Else
    '            lstError.Items.Clear()
    '            lstError.Items.Add("No Domain is available for this process...")
    '            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
    '            Exit Function
    '        End If
    '        'dtstart hold  value of start date 
    '        Dim dtStart As Date
    '        'dtEnd hold End date 
    '        Dim dtEnd As Date
    '        Dim intDays As Integer
    '        dtStart = dtStartDate_F.CalendarDate()
    '        dtEnd = dtEndDate_F.CalendarDate
    '        intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)


    '        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
    '        Dim arColName As New ArrayList
    '        Dim arRowData As New ArrayList

    '        arColName.Add("RQ_NU9_SQID_PK")
    '        arColName.Add("RQ_NU9_PROCESSID")
    '        arColName.Add("RQ_VC150_CAT1") 'ObjectType
    '        arColName.Add("RQ_VC150_CAT9") 'Status Check
    '        arColName.Add("RQ_VC100_REQUEST_DATE")
    '        arColName.Add("RQ_NU9_ALERT_FK")
    '        arColName.Add("RQ_NU9_Domain_FK")
    '        arColName.Add("RQ_NU9_Machine_Code_FK")
    '        arColName.Add("RQ_CH2_STATUS")
    '        arColName.Add("RQ_VC150_CAT4") 'StartDate
    '        arColName.Add("RQ_VC150_CAT5") 'StartTime
    '        arColName.Add("RQ_VC150_CAT6") 'EndTime
    '        arColName.Add("RQ_VC150_CAT7") 'EndDate
    '        arColName.Add("RQ_VC150_CAT12") 'Machine Name
    '        arColName.Add("RQ_VC150_CAT13") 'EnV
    '        arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

    '        Dim intMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
    '        intMax += 1
    '        'dtstdate hold value of  date+hours+min
    '        Dim dtStDate As DateTime
    '        Dim dtENDate As DateTime
    '        dtStDate = CDate(CDate(dtStartDate_F.CalendarDate).ToShortDateString & " " & DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
    '        dtENDate = CDate(CDate(dtEndDate_F.CalendarDate).ToShortDateString & " " & DDLHours_F.SelectedValue & ":" & DDLMins_F.SelectedValue)
    '        Dim intROF As Integer


    '        While dtStDate <= dtENDate


    '            arRowData.Add(intMax)
    '            arRowData.Add("10020018")
    '            arRowData.Add(DDLObjectType_F.SelectedValue) 'object type
    '            arRowData.Add(DDLStatusCheck_F.SelectedValue) 'status Check
    '            arRowData.Add(dtStDate) 'Request date
    '            arRowData.Add(DDLAlert_F.SelectedValue) 'Alert
    '            arRowData.Add(strDomainName) 'domain
    '            arRowData.Add(intMachineCode) 'Machine
    '            arRowData.Add("P") 'status


    '            'intROF = (DDLFrequency.SelectedValue * 60) / txtReoccur_F.Text.Trim
    '            'arRowData.Add(intROF)

    '            arRowData.Add(DateToJulian(dtStDate))
    '            arRowData.Add(IIf(dtStDate.TimeOfDay.Hours < 10, "0" & dtStDate.TimeOfDay.Hours, dtStDate.TimeOfDay.Hours) & IIf(dtStDate.TimeOfDay.Minutes < 10, "0" & dtStDate.TimeOfDay.Minutes, dtStDate.TimeOfDay.Minutes) & "00")
    '            Dim dtTemp As DateTime
    '            dtTemp = dtStDate.AddHours(-DDLFrequency.SelectedValue)
    '            dtTemp = dtTemp.AddMinutes(-Ddlmin.SelectedValue)

    '            arRowData.Add(IIf(dtTemp.TimeOfDay.Hours < 10, "0" & dtTemp.TimeOfDay.Hours, dtTemp.TimeOfDay.Hours) & IIf(dtTemp.TimeOfDay.Minutes < 10, "0" & dtTemp.TimeOfDay.Minutes, dtTemp.TimeOfDay.Minutes) & "00")

    '            arRowData.Add(DateToJulian(dtTemp.Date))
    '            arRowData.Add(DdlMachine.SelectedItem.Text) 'Machine
    '            arRowData.Add(DdlEnv.SelectedValue) 'ENV
    '            arRowData.Add(Session("PropCAComp")) 'Company id

    '            Multi.Add("T130022", arColName, arRowData)
    '            intMax += 1
    '            dtStDate = dtStDate.AddHours(DDLFrequency.SelectedValue)
    '            dtStDate = dtStDate.AddMinutes(Ddlmin.SelectedValue)
    '        End While
    '        Multi.Save()
    '        'after save all text boxes and dropdown reset
    '        DDLAlert_F.SelectedIndex = -1
    '        DDLFrequency.SelectedIndex = -1
    '        DDLHours_F.SelectedIndex = -1
    '        DDLMins_F.SelectedIndex = -1
    '        DDLObjectType_F.SelectedIndex = -1
    '        DDLStatusCheck_F.SelectedIndex = -1
    '        dtEndDate_F.CalendarDate = ""
    '        dtStartDate_F.CalendarDate = ""
    '        DdlDomain.SelectedIndex = -1
    '        DdlMachine.Items.Clear()
    '        DdlEnv.SelectedIndex = -1
    '        Ddlmin.SelectedIndex = -1

    '        Return True
    '    Catch ex As Exception
    '        CreateLog("BGDailyMonitor", "SaveBGRequest", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
    '        Return False
    '    End Try

    'End Function



End Class
