Imports ION.data
Imports ION.Logging
Imports System.Security
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports System.Data



Partial Class MonitoringCenter_ChkPingStatus
    Inherits System.Web.UI.Page


    Public Shared dsALTY As New DataSet

    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private arrSearchText As New ArrayList
    Private Shared dvSearch As New DataView
    Shared mintID As Integer
    Shared mintstatus As String
    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer
    Dim dvtemp As DataView
    Private mdvPingMonitor As New DataView
    Dim intdomain As Integer
#Region "User Variables"

    Dim rowvalue As Integer
    Dim insertedBy As String
    Dim insertedOn As String
    Dim systemBy As String

    Private Shared mTextBox() As TextBox
    Protected Shared mdvtable As DataView = New DataView
    Private Shared arColWidth As New ArrayList
    Private arColumnName As New ArrayList
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared intCol As Integer

    Dim mshFlag As Short

    Private intRequestID As Integer


#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCSS(Me.Page, "cpnlPing")
        cpnlError.Visible = False
        ' javascript function added with controls check numeric value and decimal 

        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 

        '''paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlPing:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 15
        End If
        txtPageSize.Text = mintPageSize
        ''textbox objectname and reportdropdown list visible acc to selected value of object type
        '******************************  
        Try
            If Not (Page.IsPostBack) Then
                cpnlPing.Text = "Ping Monitor [" & WSSSearch.SearchCompNameID(HttpContext.Current.Session("PropCAComp")).ExtraValue & "]"
                'set paging index count from zero
                CurrentPg.Text = _currentPageNumber.ToString()
                'GetCompany()
                'DdlDomain.Items.Clear()
                'DefineGridColumnData()
                DefineGridColumnData()
                'call domain
                GetDomain()

                cpnlPing.State = CustomControls.Web.PanelState.Collapsed
                'cpnldatabase enabled fales
                cpnlPing.Enabled = False
                CpnlPingSearch.State = CustomControls.Web.PanelState.Collapsed
                'cpnldatabase enabled fales
                CpnlPingSearch.Enabled = False

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
                            If SaveRequest() = True Then
                                'if save reguest true then msg pnl show msg
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)

                            End If
                        Case "Close"
                            Response.Redirect("Configuration.aspx", False)

                        Case "Delete"
                            'Call DeleteBGRequest function
                            If DeletePingRequest() = True Then
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

        Catch ex As Exception
        End Try
    End Sub
    Private Function GetDomain()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011  where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
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
            CreateLog("Ping", "GetDomain-266", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString)
        End Try
    End Function
    Private Function FormatGrid()
        Try
            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrPing.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrPing.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrPing.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("Ping", "FormatGrid-331", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Function
    Private Function DefineGridColumnData()
        'define width of columns
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(20)

        arrColWidth.Add(110) 'Machine   
        arrColWidth.Add(100) 'start date
        arrColWidth.Add(100) 'End Date
        arrColWidth.Add(80) 'Time

        arrColWidth.Add(100) 'alert

        arrColWidth.Add(50) 'status


        arrTextboxName.Clear()
        arrTextboxName.Add("TxtMachine_s")
        arrTextboxName.Add("TxtstartDate_s")
        arrTextboxName.Add("TxtEnddate_s")
        arrTextboxName.Add("TxtTime_s")
        arrTextboxName.Add("TxtAlert_s")
        arrTextboxName.Add("Txtstatus_s")


        arrColumnName.Clear()
        arrColumnName.Add("RQ_VC150_CAT3") 'Machine
        arrColumnName.Add("RQ_VC100_Request_Date") 'satrt date
        arrColumnName.Add("EndDate") 'End date
        arrColumnName.Add("RQ_VC150_CAT4") 'Time
        arrColumnName.Add("AM_VC20_Code") 'Alert 

        arrColumnName.Add("RQ_CH2_STATUS") 'status
        arrColumnName.Add("RQ_NU9_SQID_PK")

    End Function
    Private Function FillMachineDropDown(ByVal Domain As Integer)
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            ' This function will fetch all the Machine against domain
            sqstr = "select MM_VC150_Machine_Name  from t170012 where MM_NU9_DID_FK=" & Domain & " and  MM_CH1_IsEnable='E' "
            If SQL.Search("T170012 ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                'if sql.search true then fill Machine dropdown
                'dvSearch = dsTemp.Tables(0).DefaultView
                DdlMachine.DataSource = dsTemp.Tables(0)
                'Machine name
                DdlMachine.DataTextField = "MM_VC150_Machine_Name"
                'save as machine id
                'DDLMachine_DiskF.DataValueField = "MM_NU9_MID"
                DdlMachine.DataBind()
                DdlMachine.Items.Insert(0, New ListItem("Select", "0"))

                ddlMachine1.DataSource = dsTemp.Tables(0)
                'Machine name
                ddlMachine1.DataTextField = "MM_VC150_Machine_Name"
                'save as machine id
                'DDLMachine_DiskF.DataValueField = "MM_NU9_MID"
                ddlMachine1.DataBind()
                ddlMachine1.Items.Insert(0, New ListItem("Select", "0"))
            Else
                'if sql.search false then msg pnl show msg
                lstError.Items.Clear()
                lstError.Items.Add("Sorry no Machine available for selected Domain")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("Ping", "FillDiskMachineDropdown-554", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Function

    Private Sub BindGrid(ByVal DomainName As String)

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            dgrPing.PageSize = mintPageSize ' set the grid page size
            ' This function will fetch data from t130022  against process and a company
            sqstr = "select RQ_VC150_CAT3, convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as rq_vc100_request_date ,convert(varchar,convert(datetime,rq_vc100_request_date,101),101)as Enddate ,RQ_VC150_CAT4,b.AM_VC20_Code,RQ_CH2_STATUS,RQ_NU9_SQID_PK from t130022 ,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK  and rq_vc150_cat2='" & DomainName & "' and  RQ_NU9_PROCESSID='10020016'and  RQ_NU9_ClientID_FK=" & Session("PropCAComp")

            If SQL.Search("T130022", "Basicmonitoring", "BindDiskGrid", sqstr, dsTemp, "", "") = True Then
                'if sql search is true then we bind grid
                'put value of dataset to dataview
                mdvPingMonitor = dsTemp.Tables("T130022").DefaultView
                'filterdataview
                GetFilteredDataView(mdvPingMonitor, GetRowFilter)
                'Datagrid fetch data from dataview
                dgrPing.DataSource = mdvPingMonitor.Table
                'Paging
                If (mintPageSize) * (dgrPing.CurrentPageIndex) > mdvPingMonitor.Table.Rows.Count - 1 Then
                    dgrPing.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                'bind grid
                dgrPing.DataBind()
            Else
                'if sql search is false then dummy row of columns shown in datagrid
                mdvPingMonitor = dsTemp.Tables("T130022").DefaultView
                dgrPing.DataSource = dsTemp.Tables(0)
                dgrPing.DataBind()

            End If

            Dim intRows As Integer = mdvPingMonitor.Table.Rows.Count
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
            CreateLog("Ping", "bindGrid-394", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Function SaveRequest() As Boolean
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
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020016'"
            'reader
            sqRDR = SQL.Search("Ping", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
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

            Dim intMax As Integer = SQL.Search("PingMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            'define column name
            arColName.Add("RQ_NU9_SQID_PK")
            arColName.Add("RQ_NU9_PROCESSID")
            arColName.Add("RQ_VC150_CAT1")
            arColName.Add("RQ_VC150_CAT2") 'Domain
            arColName.Add("RQ_VC150_CAT3") 'Machine 

            arColName.Add("RQ_VC150_CAT4") 'Time

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
            dtStart = dtStartDate.CalendarDate

            dtEnd = dtEndDate.CalendarDate

            'calculate diff of enddate and start date
            intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)

            Dim dtStDate1 As String
            Dim dtStDate As DateTime
            Dim dtENDate As DateTime
            'hold start date ,hours and min
            dtStDate = CDate(CDate(dtStartDate.CalendarDate).ToShortDateString & " " & ddlHours.SelectedValue & ":" & DdlMins.SelectedValue)
            Dim dttime As DateTime = CDate(ddlHours.SelectedValue & ":" & DdlMins.SelectedValue)
            dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString

            'hold end date,hours and min
            dtENDate = CDate(CDate(dtEndDate.CalendarDate).ToShortDateString & " " & ddlHours.SelectedValue & ":" & DdlMins.SelectedValue)
            While dtStDate <= dtENDate
                'dtENDate.AddDays(+1)
                arRowData.Clear()
                arRowData.Add(intMax)
                arRowData.Add("10020016")
                arRowData.Add("Ping")
                arRowData.Add(Ddldomain.SelectedItem.Text)
                arRowData.Add(DdlMachine.SelectedItem.Text)

                arRowData.Add(IIf(dtStDate.TimeOfDay.Hours < 10, "0" & dtStDate.TimeOfDay.Hours, dtStDate.TimeOfDay.Hours) & IIf(dtStDate.TimeOfDay.Minutes < 10, "0" & dtStDate.TimeOfDay.Minutes, dtStDate.TimeOfDay.Minutes) & "00")

                arRowData.Add(dtStDate1)
                arRowData.Add(DDLAlert.SelectedValue)
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
                dtStDate = dtStDate.AddMinutes(DdlMin_F.SelectedValue)
                dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString

                'dtStDate = dtStDate.AddMinutes(dmin_f)
            End While
            'multisave
            Multi.Save()

            DdlMachine.SelectedIndex = 0
            dtStartDate.CalendarDate = ""
            dtEndDate.CalendarDate = ""
            ddlHours.SelectedIndex = 0
            DdlMins.SelectedIndex = 0
            DDLAlert.SelectedIndex = 0
            DDLFrequency.SelectedIndex = 0
            DdlMin_F.SelectedIndex = 0
            Ddldomain.SelectedIndex = 0
            DdlMachine.Items.Clear()
            'Multi.Dispose()

            Return True
        Catch ex As Exception
            CreateLog("Ping", "SaveRequest-1213", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        Dim currentDate As Date
        currentDate = Date.Now.Today


        If DdlMachine.SelectedValue.Equals("0") Then
            lstError.Items.Add("Select Machine name...")
            shFlag = 1
        End If

        If Ddldomain.SelectedValue.Equals("0") Then
            lstError.Items.Add("Select Domain name...")
            shFlag = 1
        End If

        If DDLAlert.SelectedValue.Equals("0") Then
            lstError.Items.Add("Select alert type ...")
            shFlag = 1
        End If

        If DDLFrequency.SelectedValue.Equals("0") And DdlMin_F.SelectedValue.Equals("0") Then
            lstError.Items.Add("Frequency cannot be less then 0...")
            shFlag = 1
        End If

        If dtStartDate.CalendarDate = "" Then
            lstError.Items.Add("Start Date cannot be blank...")
            shFlag = 1
            If dtEndDate.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
            Else
                If CDate(dtStartDate.CalendarDate) > CDate(dtEndDate.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        Else
            If dtEndDate.CalendarDate = "" Then
                lstError.Items.Add("End Date cannot be blank...")
                shFlag = 1
                If CDate(dtStartDate.CalendarDate) < currentDate Then
                    lstError.Items.Add("Start Date cannot be Less than Current date...")
                End If
            Else

                If CDate(dtStartDate.CalendarDate) > CDate(dtEndDate.CalendarDate) Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        End If

        'if shFlag=1 means any textbox empty 
        If shFlag = 1 Then
            'message panel display msg
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            'return False
            Return False
        Else
            'If Shflag=0 no text box empty
            'return True
            Return True
        End If

    End Function
    Private Function GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlPing:dgrPing:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("Ping", "GetsearchText-1045", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            For inti As Integer = 0 To mdvPingMonitor.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If mdvPingMonitor.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf mdvPingMonitor.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("Ping", "GetRowFilter-1085", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Function
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
            CreateLog("Ping", "FillAlertTypeDropDown-289", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

    Private Sub Ddldomain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ddldomain.SelectedIndexChanged
        cpnlPing.State = CustomControls.Web.PanelState.Expanded
        'cpnldatabase enabled fales
        cpnlPing.Enabled = True
        CpnlPingSearch.State = CustomControls.Web.PanelState.Expanded
        'cpnldatabase enabled fales
        CpnlPingSearch.Enabled = True


        DdlMachine.Items.Clear()
        FillMachineDropDown(Ddldomain.SelectedValue)

        FillAlertTypeDropDown()

        'fill ddlalert Disk dropdown list
        DDLAlert.DataSource = dsALTY.Tables(0)
        'textfield as Alert
        DDLAlert.DataTextField = "AM_VC20_Code"
        'value save as ID
        DDLAlert.DataValueField = "AM_NU9_AID_PK"
        DDLAlert.DataBind()
        DDLAlert.Items.Insert(0, New ListItem("Select", "0"))
        BindGrid(Ddldomain.SelectedItem.Text)
        Session("DomainName") = Ddldomain.SelectedItem.Text
    End Sub

    Private Sub dgrPing_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(dvtemp) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To mdvPingMonitor.Table.Columns.Count - 2
                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                    Next
                End If
            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlPingr:dgrPing:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("PingMonitor", "dgrPing_ItemDataBound-785", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgrPing.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindGrid(Session("DomainName"))

    End Sub

    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgrPing.CurrentPageIndex > 0) Then
            dgrPing.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindGrid(Session("DomainName"))

    End Sub

    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgrPing.CurrentPageIndex < (dgrPing.PageCount - 1)) Then
            dgrPing.CurrentPageIndex += 1

            If dgrPing.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgrPing.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindGrid(Session("DomainName"))

    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgrPing.CurrentPageIndex = (dgrPing.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindGrid(Session("DomainName"))

    End Sub
    Private Function DeletePingRequest() As Boolean
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
            CreateLog("Ping", "DeletePingRequest-851", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try

    End Function

    Private Function ReadGrid(ByVal sqid As ArrayList)
        Try
            Dim gridrow As DataGridItem

            For Each gridrow In dgrPing.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    'add Sqid of checked Cheeckboxes
                    sqid.Add(gridrow.Cells(0).Text)
                End If
            Next
        Catch ex As Exception
        End Try
    End Function

    Private Function CheckPingRequest()
        Try
            Dim intRows As Integer
            Dim strSearchRecords As String = "select DM_VC150_DomainName,MM_VC150_Machine_Name,rs_vc100_response_DateTime from T130023,T170011,t170012 where RS_NU9_PROCESSID=10020016 and DM_NU9_DID_PK= MM_NU9_DID_FK and MM_NU9_MID=RS_VC150_CAT1  and MM_NU9_DID_FK=RS_VC150_CAT2"

            SQL.DBTracing = False

            If DDLMACHINE1.SelectedItem.Text.Equals("") = False And (DateSelector1.CalendarDate.Equals("") = False And DateSelector2.CalendarDate.Equals("") = False) Then
                strSearchRecords &= " where RS_VC150_CAT1='" & DDLMACHINE1.SelectedValue & "' and rs_vc100_response_DateTime '" & DateSelector1.CalendarDate & "' and '" & DateSelector2.CalendarDate & "'"
            ElseIf DDLMACHINE1.SelectedItem.Text.Equals("") = False And (DateSelector1.CalendarDate.Equals("") = True And DateSelector2.CalendarDate.Equals("") = True) Then
                strSearchRecords &= " where RS_VC150_CAT1='" & DDLMACHINE1.SelectedValue & "'"
            ElseIf DDLMACHINE1.SelectedItem.Text.Equals("") = False And (DateSelector1.CalendarDate.Equals("") = False And DateSelector2.CalendarDate.Equals("") = True) Then
                strSearchRecords &= " where RS_VC150_CAT1='" & DDLMACHINE1.SelectedValue & "' and rs_vc100_response_DateTime '" & DateSelector1.CalendarDate & "' and '" & Now.ToShortDateString & "'"
            ElseIf DDLMACHINE1.SelectedItem.Text.Equals("") = True And (DateSelector1.CalendarDate.Equals("") = False And DateSelector2.CalendarDate.Equals("") = False) Then
                strSearchRecords &= " where rs_vc100_response_DateTime '" & DateSelector1.CalendarDate & "' and '" & DateSelector2.CalendarDate & "'"
            End If

            Dim dsReqResult As New DataSet

            If SQL.Search(" T130023", "ChkPingStatus", "CheckPingRequest", strSearchRecords, dsReqResult, "", "") = True Then
                For inti As Integer = 0 To dsReqResult.Tables(0).Rows.Count - 1
                    If dsReqResult.Tables(0).Rows(inti).Item(3) = "NE" Then
                        dsReqResult.Tables(0).Rows(inti).Item(3) = "<refresh><img src='../Images/GreenAlert.gif' align='middle'></refresh>"
                    Else
                        dsReqResult.Tables(0).Rows(inti).Item(3) = "<refresh><img src='../Images/RedAlert.gif' align='middle'></refresh>"
                    End If

                Next
                dsReqResult.AcceptChanges()
                grdPing.DataSource = dsReqResult
                grdPing.DataBind()
            Else
                grdPing.DataSource = dsReqResult.Tables(0)
                grdPing.DataBind()
                lstError.Items.Clear()
                lstError.Items.Add("No Records available for this machine...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            End If

        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy Please Try Later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
        End Try
    End Function

    Private Sub dgrPing_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrPing.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(mdvPingMonitor) = True Then
                    Exit Sub
                Else

                    For inti As Integer = 0 To mdvPingMonitor.Table.Columns.Count - 2
                        'checking value of status if it is not P
                        If CType(e.Item.Cells(5).FindControl("lblStatus"), Label).Text.Trim <> "P" Then
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
                        CType(txt, TextBox).Text = Request.Form("cpnlPing:dgrPing:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("Pingmonitoring", "dgrPing_ItemDataBound-980", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Function ValidatePingRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        Dim currentDate As Date
        currentDate = Date.Now.Today

        If DDLMACHINE1.SelectedItem.Text.Equals("") Then
            If DateSelector1.CalendarDate = "" Then
                If DateSelector2.CalendarDate = "" Then
                Else
                    lstError.Items.Add("Start Date Cannot be  blank...")
                    shFlag = 1
                End If
            End If
        End If

        If DDLMACHINE1.SelectedItem.Text.Equals("") = False Then
            If DateSelector1.CalendarDate = "" Then
                If DateSelector2.CalendarDate = "" Then
                Else
                    lstError.Items.Add("Start Date Cannot be  blank...")
                    shFlag = 1
                End If
            End If
        End If

        If DateSelector1.CalendarDate.Trim.Equals("") = False And DateSelector2.CalendarDate.Trim.Equals("") = False Then
            If CDate(DateSelector1.CalendarDate) > CDate(DateSelector2.CalendarDate) Then
                If DateSelector2.CalendarDate.Equals("") = False Then
                    lstError.Items.Add("End Date cannot be less than Start Date...")
                    shFlag = 1
                End If
            End If
        ElseIf DateSelector1.CalendarDate.Trim.Equals("") = True And DDLMACHINE1.SelectedItem.Text.Equals("") = True Then
            shFlag = 1
            lstError.Items.Add("Please enter start date...")
        ElseIf DateSelector1.CalendarDate.Trim.Equals("") = True And DDLMACHINE1.SelectedItem.Text.Equals("") = False And DateSelector2.CalendarDate.Trim.Equals("") = False Then
            shFlag = 1
            lstError.Items.Add("Please enter start date...")

        ElseIf DDLMACHINE1.SelectedItem.Text.Equals("") = True Then
            shFlag = 1
            lstError.Items.Add("Please enter Machine name...")
        ElseIf DDLMACHINE1.SelectedItem.Text.Trim.ToUpper.Equals("SELECT") = True And DateSelector2.CalendarDate.Trim.Equals("") = True And DateSelector1.CalendarDate.Trim.Equals("") = True Then
            shFlag = 1
            lstError.Items.Add("Please enter some search criteria...")
        ElseIf DateSelector2.CalendarDate.Trim.Equals("") = True Then
            DateSelector2.CalendarDate = Now.Today
        Else
            shFlag = 0
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

    Private Sub imgShow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgShow.Click
        If ValidatePingRequest() = False Then
            Exit Sub
        End If
        If DDLMACHINE1.SelectedValue.Equals("0") Then
            DDLMACHINE1.SelectedItem.Text = ""
        End If
        CheckPingRequest()
    End Sub

End Class
