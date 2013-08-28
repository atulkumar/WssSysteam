Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Partial Class MonitoringCenter_SystemLogin
    Inherits System.Web.UI.Page

    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer
    Dim dvtemp As DataView
    Private mdvSystemLogin As New DataView
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents Ddldomain As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cpnlDiskEntry As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Private arrSearchText As New ArrayList
    'Protected WithEvents dgSystemLogin As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Dropdownlist1 As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents TxtURL_F As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LblDomain As System.Web.UI.WebControls.Label
    'Protected WithEvents cpnlSystemLogin As CustomControls.Web.CollapsiblePanel
    Private mdvDiskMonitor As New DataView
    Const intProcessid As Integer = 10020019

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        txtCSS(Me.Page, "cpnlSystemLogin")
        cpnlError.Visible = False
        'check numaric value 
        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();")
        '''paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlSystemLogin:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 15
        End If
        txtPageSize.Text = mintPageSize
        ''textbox objectname and reportdropdown list visible acc to selected value of object type
        '******************************  
        If Not (IsPostBack) Then
            'calling javascript function
            Ddldomain.Attributes.Add("OnChange", "DomainChange('" & Ddldomain.ClientID & "','" & DdlMachine_F.ClientID & "');")
            DdlMachine_F.Items.Add(New ListItem("Select", "0"))
        End If

        If IsPostBack Then
            'fill machine dropdown by selected domain without postback
            FillAjaxDropDown(DdlMachine_F, Request.Form("txtMachineInfo"), "cpnlSystemLogin:" & DdlMachine_F.ID, New ListItem("Select", "0"))
        End If

        Try
            If Not (Page.IsPostBack) Then
                'set paging index count from zero
                CurrentPg.Text = _currentPageNumber.ToString()
                'GetCompany()
                'DdlDomain.Items.Clear()
                DefineGridColumnData()
                'call domain
                GetDomain()
                TxtURL_F.Visible = False
                'fill Alert DRopdown
                FillDropDown(DDLalert_F, "select AM_NU9_AID_PK, AM_VC20_Code from T180011", False, True)
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
                            If SaveRequest() = True Then
                                'if save reguest true then msg pnl show msg
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            End If
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
            'call BindDiskgrid to fill grid
            bindGrid()
            'call format grid 
            FormatGrid()
        Catch ex As Exception
        End Try
    End Sub
    '***************************************************************************************
    'Created By:         Mandeep
    'Create Date:       22/01/2007
    'Purpose:             This function is used to fill Alert dropdown acc to selected company
    'Table:                  t180011
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
    'Create Date:      22/01/2007
    'Purpose:            This function is used to fill Alert dropdown acc to selected company
    'Table                  t180011
    'Modify Date:       ------
    '***************************************************************************************
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
            CreateLog("SystemLogin", "FillDropDown", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
    '***************************************************************************************
    'Created By:         Mandeep
    'Create Date:       22/01/2007
    'Purpose:             This function is used to fill domain dropdown acc to selected company
    ' Table:                 t170011
    'Modify Date:        ------
    '***************************************************************************************
    Private Function GetDomain()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            ' This function will fetch all the Domains against a company
            If SQL.Search("T170011", "SystemLogin", "GetDomain", sqstr, dsTemp, "", "") = True Then
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
            CreateLog("SystemLogin", "GetDomain-238", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:       Mandeep
    'Create Date:     21/01/2007
    'Purpose:           This function is used to Format grid
    'Table:                t130022
    'Modify Date:       ------
    '***********************************************************************************
    '
    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgSystemLogin.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgSystemLogin.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgSystemLogin.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("SystemLogin", "FormatGrid-258", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:         Mandeep
    'Create Date:       25/01/2007
    'Purpose:             This function is used to bind grid
    'Modify Date:       27/01/2007
    '***********************************************************************************
    '
    Private Function bindGrid() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            Dim strSQL As String

            dgSystemLogin.PageSize = mintPageSize ' set the grid page size          
            '' This function will fetch data from t130022  against process and a company
            strSQL = "select RQ_VC150_CAT1,RQ_VC150_CAT2, case isnull(RQ_VC150_CAT3,'')when '' then '' else '*' end as RQ_VC150_CAT3 , case isnull(RQ_VC150_CAT4,'') when '' then '' else '*' end as RQ_VC150_CAT4,convert(varchar,convert(datetime,rq_vc100_request_date,101),101) as rq_vc100_request_date  ,convert(varchar,convert(datetime,rq_vc100_request_date,101),101) as Enddate ,convert(varchar,convert(datetime,rq_vc100_request_date,101),108) as RQTime,b.AM_VC20_Code,RQ_CH2_STATUS,RQ_NU9_SQID_PK from t130022 ,T180011 b where RQ_NU9_ALERT_FK=b.AM_NU9_AID_PK and RQ_NU9_PROCESSID=" & intProcessid & " and RQ_NU9_ClientID_FK=" & Session("PropCAComp") & " order by rq_vc100_request_date, RQTime"
            If SQL.Search("T130022", "SystemLogin", "BindGrid", strSQL, dsTemp, "", "") = True Then
                'if sql search is true then we bind grid
                'put value of dataset to dataview
                mdvSystemLogin = dsTemp.Tables("t130022").DefaultView
                'filterdataview
                GetFilteredDataView(mdvSystemLogin, GetRowFilter)
                'Datagrid fetch data from dataview
                dgSystemLogin.DataSource = mdvSystemLogin.Table
                'Paging
                If (mintPageSize) * (dgSystemLogin.CurrentPageIndex) > mdvSystemLogin.Table.Rows.Count - 1 Then
                    dgSystemLogin.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                'bind grid
                dgSystemLogin.DataBind()
            Else
                'if sql search is false then dummy row of columns shown in datagrid
                mdvSystemLogin = dsTemp.Tables("t130022").DefaultView
                dgSystemLogin.DataSource = dsTemp.Tables("t130022")
                dgSystemLogin.DataBind()
            End If

            Dim intRows As Integer = mdvSystemLogin.Table.Rows.Count
            'paging
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
            ''
        Catch ex As Exception
            CreateLog("SysytemLogin", "bindGrid-323", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    '***************************************************************************************
    'Created By:          Mandeep
    'Create Date:        25/01/2007
    'Purpose:              This function is used  to get the text from search textboxes in  an array list
    'Modify Date:         ------
    '***********************************************************************************
    '
    Private Function GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlSystemLogin:dgSystemLogin:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("SysytemLogin", "GetsearchText-349", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       25/01/2007
    'Purpose:           This function is used  to Gets the row filter string
    'Modify Date:       ------
    '***********************************************************************************
    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function
            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To mdvSystemLogin.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If mdvSystemLogin.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf mdvSystemLogin.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("SystemLogin", "GetRowFilter-392", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:      25/01/2007
    'Purpose:            This function is used to chek all textboxes are fill
    'Table:                 t130022
    'Modify Date:      27/01/2007
    '***************************************************************************************
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
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK=" & intProcessid
            'reader
            sqRDR = SQL.Search("SystemLogin", "SaveRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
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

            'machine IP
            Dim strMachineIP As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MM_VC100_Machine_IP from T170012 where MM_NU9_DID_FK=" & Ddldomain.SelectedValue & " and MM_NU9_MID=" & DdlMachine_F.SelectedValue
            'reader
            sqRDR = SQL.Search("SystemLogin", "SaveRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold machineIP
                strMachineIP = sqRDR("MM_VC100_Machine_IP")
                sqRDR.Close()
            End If

            Dim intMax As Integer = SQL.Search("SysytemLogin", "SaveRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            'define column name
            arColName.Add("RQ_NU9_SQID_PK")
            arColName.Add("RQ_NU9_PROCESSID")
            arColName.Add("RQ_VC150_CAT1") 'Req Check
            arColName.Add("RQ_VC150_Cat2") 'MachineIP/URL
            arColName.Add("RQ_VC150_Cat3") 'UID
            arColName.Add("RQ_VC150_Cat4") 'PWD
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
            dtStart = dtStartdate_F.CalendarDate

            If dtEnddate_F.CalendarDate = "" Then
                'end date empty then startdate become enddate
                dtEnd = dtStartdate_F.CalendarDate
            Else
                'hold end date
                dtEnd = dtEnddate_F.CalendarDate
            End If
            'calculate diff of enddate and start date
            intDays = DateDiff(DateInterval.Day, dtStart, dtEnd)

            Dim dtStDate1 As String
            Dim dtStDate As DateTime
            Dim dtENDate As DateTime
            'hold start date ,hours and min
            dtStDate = CDate(CDate(dtStartdate_F.CalendarDate).ToShortDateString & " " & Ddlhours_F.SelectedValue & ":" & DdlMin_F.SelectedValue)
            Dim dttime As DateTime = CDate(Ddlhours_F.SelectedValue & ":" & DdlMin_F.SelectedValue)
            dtStDate1 = dtStartdate_F.CalendarDate & " " & dttime.ToShortTimeString
            'hold end date,hours and min
            dtENDate = CDate(CDate(dtEnddate_F.CalendarDate).ToShortDateString & " " & Ddlhours_F.SelectedValue & ":" & DdlMin_F.SelectedValue)
            While dtStDate <= dtENDate
                'dtENDate.AddDays(+1)
                arRowData.Clear()
                arRowData.Add(intMax)
                arRowData.Add(intProcessid)
                arRowData.Add(DdlReqCheck_F.SelectedValue)
                If DdlReqCheck_F.SelectedValue.Equals("S") Then
                    arRowData.Add(strMachineIP)
                Else
                    arRowData.Add(TxtURL_F.Text)
                End If
                arRowData.Add(Encrypt(TxtPUID_F.Text))
                arRowData.Add(Encrypt(txtPPWD_F.Text))
                arRowData.Add(dtStDate1)
                arRowData.Add(DDLalert_F.SelectedValue)
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
                dtStDate = dtStDate.AddMinutes(DdlMin.SelectedValue)
                dtStDate1 = IIf(Len(dtStDate.Month.ToString) < 2, 0 & dtStDate.Month.ToString, dtStDate.Month) & "/" & IIf(Len(dtStDate.Day.ToString) < 2, 0 & dtStDate.Day, dtStDate.Day) & "/" & dtStDate.Year & " " & dtStDate.ToShortTimeString

                'dtStDate = dtStDate.AddMinutes(dmin_f)
            End While
            'multisave
            Multi.Save()
            resettextboxes()
            'Multi.Dispose()

            Return True
        Catch ex As Exception
            CreateLog("SystemLogin", "SaveRequest-538", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:      29/01/2007
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

        If DdlReqCheck_F.SelectedValue.Equals("S") Then
            If DdlMachine_F.SelectedValue.Equals("0") Then
                lstError.Items.Add("Select Machine name...")
                shFlag = 1
            End If
        Else
            If TxtURL_F.Text.Equals("") Then
                lstError.Items.Add("URL cannot be blank...")
                shFlag = 1
            End If
        End If

        If DDLalert_F.SelectedValue.Equals("") Then
            lstError.Items.Add("Select alert type ...")
            shFlag = 1
        End If

        If DDLFrequency.SelectedValue.Equals("0") And DdlMin.SelectedValue.Equals("0") Then
            lstError.Items.Add("Frequency cannot be less then 0...")
            shFlag = 1
        End If

        If dtStartdate_F.CalendarDate = "" Then
            lstError.Items.Add("Start Date cannot be blank...")
            shFlag = 1
        Else
            If dtStartdate_F.CalendarDate < currentDate Then
                lstError.Items.Add("Start Date cannot be Less than Current date...")
            End If
        End If

        If dtEnddate_F.CalendarDate = "" Then
            lstError.Items.Add("End Date cannot be blank...")
            shFlag = 1
        End If


        If DdlReqCheck_F.SelectedValue.Equals("S") Then
            If TxtPUID_F.Text.Equals("") Then
                lstError.Items.Add("UID cannot be blank...")
                shFlag = 1
            End If
        End If

        If dtStartdate_F.CalendarDate > dtEnddate_F.CalendarDate Then
            lstError.Items.Add("End Date cannot be less than Start Date...")
            shFlag = 1
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
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:      25/01/2007
    'Purpose:           This function is used to DefineGrid Column Data 
    'Modify Date:       ------
    '***************************************************************************************
    Private Function DefineGridColumnData()
        'define width of columns
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(20)
        arrColWidth.Add(90) 'req type
        arrColWidth.Add(160) 'MIP
        arrColWidth.Add(80) 'UID
        arrColWidth.Add(80) 'pwd
        arrColWidth.Add(80) 'startdate
        arrColWidth.Add(80) 'Enddate
        arrColWidth.Add(80) 'Time
        arrColWidth.Add(100) 'alert
        arrColWidth.Add(50) 'Status

        arrTextboxName.Clear()
        arrTextboxName.Add("TxtReqType_s")
        arrTextboxName.Add("TxtMavhineIP_s")
        arrTextboxName.Add("TxtUID_s")
        arrTextboxName.Add("TxtPWD_s")
        arrTextboxName.Add("TxtStartDate_s")
        arrTextboxName.Add("TxtEndDate_s")
        arrTextboxName.Add("txtTime_s")
        arrTextboxName.Add("TxtAlert_s")
        arrTextboxName.Add("txtStatus_s")

        arrColumnName.Clear()
        arrColumnName.Add("RQ_VC150_CAT1") 'req type
        arrColumnName.Add("RQ_VC150_Cat2") 'MIP
        arrColumnName.Add("RQ_VC150_Cat3") 'Machine UID
        arrColumnName.Add("RQ_VC150_Cat4") 'Machine PWD
        arrColumnName.Add("RQ_VC100_REQUEST_DATE") 'startdate
        arrColumnName.Add("Enddate") 'Enddate
        arrColumnName.Add("RQTime") 'Time
        arrColumnName.Add("AM_VC20_Code") 'alert
        arrColumnName.Add("RQ_CH2_STATUS") 'status
        arrColumnName.Add("RQ_NU9_SQID_PK")
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
    Private Sub DdlReqCheck_F_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlReqCheck_F.SelectedIndexChanged
        If DdlReqCheck_F.SelectedValue.Equals("S") Then
            DdlMachine_F.Visible = True
            TxtURL_F.Visible = False
            TxtPUID_F.Enabled = True
            txtPPWD_F.Enabled = True
            Ddldomain.Enabled = True
            Ddldomain.ForeColor = System.Drawing.Color.White
        Else
            TxtURL_F.Visible = True
            DdlMachine_F.Visible = False
            TxtPUID_F.Enabled = False
            txtPPWD_F.Enabled = False
            Ddldomain.Enabled = False
            Ddldomain.ForeColor = System.Drawing.Color.Gray
        End If
    End Sub
    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgSystemLogin.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        bindGrid()

    End Sub
    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgSystemLogin.CurrentPageIndex > 0) Then
            dgSystemLogin.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        bindGrid()


    End Sub
    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgSystemLogin.CurrentPageIndex < (dgSystemLogin.PageCount - 1)) Then
            dgSystemLogin.CurrentPageIndex += 1

            If dgSystemLogin.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgSystemLogin.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        bindGrid()

    End Sub
    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgSystemLogin.CurrentPageIndex = (dgSystemLogin.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        bindGrid()

    End Sub
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:      27/01/2007
    'Purpose:            This function is used to delete multiple Rows from grid
    'Table                  T130022
    'Modify Date:       ------
    '***************************************************************************************
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
            If SQL.Delete("SystemLogin", "DeleteBGRequest", strDeleteSQL, SQL.Transaction.ReadCommitted, "") = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("SystemLogin", "DeleteRequest-759", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:      27/01/2007
    'Purpose:           This function is used to Read Grid and add checked rows's SQID in Array
    'Table:                T170011
    'Modify Date:       ------
    '***************************************************************************************
    Private Function ReadGrid(ByVal sqid As ArrayList)
        Try
            Dim gridrow As DataGridItem

            For Each gridrow In dgSystemLogin.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    'add Sqid of checked Cheeckboxes
                    sqid.Add(gridrow.Cells(0).Text)
                End If
            Next
        Catch ex As Exception
        End Try
    End Function
    Private Sub dgSystemLogin_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgSystemLogin.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(mdvSystemLogin) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To mdvSystemLogin.Table.Columns.Count - 2
                        'checking value of status if it is not P
                        If CType(e.Item.Cells(8).FindControl("lblStatus"), Label).Text.Trim <> "P" Then
                            'checkbox enabled is false
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = False
                        Else
                            'checkbox enabled is True
                            CType(e.Item.FindControl("chkReq1"), CheckBox).Enabled = True
                        End If

                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        'Here check value of status if it is P or H
                        If CType(e.Item.Cells(8).FindControl("lblStatus"), Label).Text.Trim = "P" Or CType(e.Item.Cells(8).FindControl("lblStatus"), Label).Text.Trim = "H" Then
                            'open popup window
                            e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ",1)")
                        Else
                            'message
                            e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck56();")
                        End If
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                        'e.Item.Cells(inti + 2).ToolTip = IIf(IsDBNull(mdvSystemLogin.Table.Rows(e.Item.ItemIndex).Item(inti)) = True, "", mdvSystemLogin.Table.Rows(e.Item.ItemIndex).Item(inti))
                    Next
                End If
                'For inti As Integer = 0 To dvtemp.Table.Columns.Count - 2
                '    e.Item.Cells(inti + 2).ToolTip = IIf(IsDBNull(dvtemp.Table.Rows(e.Item.ItemIndex).Item(inti)) = True, "", dvtemp.Table.Rows(e.Item.ItemIndex).Item(inti))
                'Next

            Else
                'for search
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlSystemLogin:dgSystemLogin:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("systemLogin", "dgSystemLogin_ItemDataBound-825", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub
    Private Function resettextboxes()
        Try
            'clear textboxes of fast entry and reset Dropdown list

            TxtPUID_F.Text = ""
            txtPPWD_F.Text = ""
            txtPPWD_F.Attributes.Add("value", txtPPWD_F.Text)

            'DDLMachine_DiskF.SelectedIndex = -1
            Ddlhours_F.SelectedIndex = 0
            DdlMin.SelectedIndex = 0
            DdlMin_F.SelectedIndex = 0
            DDLFrequency.SelectedIndex = -1
            dtStartdate_F.CalendarDate = ""
            DdlReqCheck_F.SelectedIndex = -1
            TxtURL_F.Text = ""
            DDLalert_F.SelectedIndex = -1
            dtEnddate_F.CalendarDate = ""
            DdlMachine_F.Items.Clear()
            Ddldomain.SelectedIndex = -1

        Catch ex As Exception
            CreateLog("BasicMonitoring", "resettextboxes-1439", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Function

End Class
