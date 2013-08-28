Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Partial Class MonitoringCenter_EnvEntry
    Inherits System.Web.UI.Page


    Private dvSearch As New DataView
    Private arrSearchText As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    'Protected WithEvents cpnlEnvEntry As CustomControls.Web.CollapsiblePanel
    Private Shared arrColumnName As New ArrayList
    Public mintPageSize As Integer
    'Protected WithEvents txtDatabase_F As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtOwner_F As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtMachine_F As System.Web.UI.WebControls.TextBox
    Protected _currentPageNumber As Int32 = 1
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        txtCSS(Me.Page, "cpnlEnvEntry")

        '''paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlEnvEntry:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 15
        End If
        txtPageSize.Text = mintPageSize

        Try
            If Not (Page.IsPostBack) Then
                CurrentPg.Text = _currentPageNumber.ToString()
                'DdlDomain.Items.Clear()
                DefineGridColumnData()

                'DdlEnv.Items.Clear()
                GetEnvironment()
                'BINDGrid()


            Else
                Dim txthiddenImage As String = Request.Form("txthiddenImage")
                If txthiddenImage <> "" Then
                    Select Case txthiddenImage
                        Case "Save"
                            'call SAVERecord Function
                            If SAVERecord() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                                cleartextboxes()
                                'BINDGrid()
                                dgrEnvDetail.SelectedIndex = -1
                            End If
                            'Call DeleteRecord Function
                        Case "Delete"
                            If DeleteRecord() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                                BINDGrid()
                            End If
                        Case "Close"
                            Response.Redirect("Configuration.aspx", False)
                    End Select
                End If
            End If
            'call BindGrid Function
            BINDGrid()
            FormatGrid()
        Catch ex As Exception
            CreateLog("ReportEntry", "PageLoad-157", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Function BINDGrid() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim dttemp As New DataTable
            Dim sqstr As String
            'this function fetch data from t1300041 against a company
            sqstr = "select  EV_VC30_Environment_Name,EV_VC50_Database, EV_VC30_Owner,case isnull(EV_VC50_UserID,'')when '' then '' else '*' end as EV_VC50_UserID , case isnull(EV_VC50_Password,'') when '' then '' else '*' end as EV_VC50_Password ,EV_VC100_SystemID,EV_NU9_ID_PK  from T130172 where   EV_NU9_CompanyID=" & Session("PropCAComp")

            If SQL.Search("T130172 ", "ReportEntry", "BINDGrid", sqstr, dsTemp, "", "") = True Then

                dvSearch = dsTemp.Tables(0).DefaultView
                GetFilteredDataView(dvSearch, GetRowFilter)
                'filter dataview
                dgrEnvDetail.DataSource = dvSearch.Table

                'Paging
                If (mintPageSize) * (dgrEnvDetail.CurrentPageIndex) > dvSearch.Table.Rows.Count - 1 Then
                    dgrEnvDetail.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                'bind grid
                dgrEnvDetail.DataBind()
            Else
                dttemp = dsTemp.Tables(0)
                dvSearch = dttemp.DefaultView
                dgrEnvDetail.DataSource = dttemp
                dgrEnvDetail.DataBind()

                'Dim drTemp As DataRow
                'For inti As Integer = 0 To arrColumnName.Count - 1
                '    dttemp.Columns.Add(arrColumnName(inti))
                'Next
                'drTemp = dttemp.NewRow
                'For intI As Integer = 0 To dttemp.Columns.Count - 1
                '    drTemp.Item(inti) = ""
                'Next
                'dttemp.Rows.Add(drTemp)
                'dttemp.AcceptChanges()
                'dvSearch = dttemp.DefaultView ' use for paging
                'dgrEnvDetail.DataSource = dttemp

                'CurrentPg.Text = 0
                ''''End If
                'dgrEnvDetail.DataBind()

            End If
            Dim intRows As Integer = dvSearch.Table.Rows.Count

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

        Catch ex As Exception
            CreateLog("ReportEntry", "BindGrid-369", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtOwner_F.Text.Equals("") Then
            lstError.Items.Add("Owner  cannot be blank...")
            shFlag = 1
        End If

        If txtDatabase_F.Text.Equals("") Then
            lstError.Items.Add("Database  cannot be blank...")
            shFlag = 1
        End If
        If txtMachine_F.Text.Equals("") Then
            lstError.Items.Add("Machine Name cannot be blank...")
            shFlag = 1
        End If
        If txtUserID_F.Text.Equals("") Then
            lstError.Items.Add("User ID cannot be blank...")
            shFlag = 1
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

    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrEnvDetail.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrEnvDetail.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrEnvDetail.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("ReportEntry", "FormatGrid-899", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    Private Function GetEnvironment()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch  ENV against a company
            sqstr = "select name from udc where udctype='PENV' "
            If SQL.Search("udc ", "ReportEntry", "GetEnvironment", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlENV dropdown fill acc to selected company
                DDLEnv_F.DataSource = dsTemp.Tables(0)
                'Env Name
                DDLEnv_F.DataTextField = "name"
                DDLEnv_F.DataBind()
            Else
                'SQL.Search is False ddlENV dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Env  avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetEnvironment-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    Private Function GetSeacrhText()
        Try
            'get the text from search textboxes 
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                strSearch = Request.Form("cpnlEnvEntry:dgrEnvDetail:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("ReportEntry", "GetSeacrhText-394", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function
    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function
            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To dvSearch.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If dvSearch.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf dvSearch.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("ReportEntry", "GetRowFilter-438", LogType.Application, LogSubType.Exception, "", ex.ToString)
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

    Private Sub dgrEnvDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrEnvDetail.ItemDataBound
        Try

            If (e.Item.ItemType = ListItemType.Item Or ListItemType.AlternatingItem) And e.Item.ItemType <> ListItemType.Header Then

                'e.Item.Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                If IsNothing(dvSearch) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To dvSearch.Table.Columns.Count - 2
                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "GridClick(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                    Next
                End If
            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlEnvEntry:dgrEnvDetail:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "dgrBGDailyMonitor_ItemDataBound-798", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Sub

    Private Function DefineGridColumnData()
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(80) 'ENV
        arrColWidth.Add(100) 'OWNER
        arrColWidth.Add(100) 'Machine
        arrColWidth.Add(100) ' UID
        arrColWidth.Add(100) ' PWD
        arrColWidth.Add(100) 'Database

        arrTextboxName.Clear()
        arrTextboxName.Add("txtEnvName")
        arrTextboxName.Add("txtDatabase")
        arrTextboxName.Add("txtOwner")
        arrTextboxName.Add("txtUserID")
        arrTextboxName.Add("txtPassword")
        arrTextboxName.Add("txtMachine")




        arrColumnName.Clear()
        arrColumnName.Add("EV_VC30_Environment_Name") 'Domain
        arrColumnName.Add("EV_VC50_Database") 'Submit REport
        arrColumnName.Add("EV_VC30_Owner") 'Machine Name
        arrColumnName.Add("EV_VC50_UserID") 'Machine UID
        arrColumnName.Add("EV_VC50_Password") 'Machine PWD
        arrColumnName.Add("EV_VC100_SystemID") 'Release

        arrColumnName.Add("EV_NU9_ID_PK")

    End Function
    Private Function DeleteRecord() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'intSeqId hold value of SQID that in txthiddenID which is not visible
            Dim intSeqID As Integer = Request.Form("txthiddenID")

            sqstr = "delete from t130172 where  EV_NU9_ID_PK=" & intSeqID
            If SQL.Delete("EnvEntry", "deleterecod", sqstr, SQL.Transaction.Serializable, "") = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function cleartextboxes()
        'clear textboxe and reset dropdown after save
        txtUserID_F.Text = ""
        txtPassword_F.Attributes.Add("value", txtPassword_F.Text)
        DDLEnv_F.SelectedIndex = 0

        txtPassword_F.Text = ""
        txtOwner_F.Text = ""
        txtDatabase_F.Text = ""
        txtMachine_F.Text = ""



    End Function
    Private Function SAVERecord() As Boolean
        Try

            If ValidateRequest() = False Then
                Exit Function
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            Dim PUID As String
            Dim PPWD As String
            Dim FilePath As String
            Dim strsql As String
            'this query fetch SQID from T130041
            Dim intMax As Integer = SQL.Search("Report in People Soft", "SaveRecord", "select max(EV_NU9_ID_PK) from t130172", "")
            intMax += 1
            'this function check validation before save that all Fields are fill or not if submit report is  N
            arColName.Add("EV_NU9_ID_PK")
            arColName.Add("EV_VC30_Environment_Name") 'ENV
            arColName.Add("EV_VC30_Owner") 'Table owner
            arColName.Add("EV_VC100_SystemID") 'Machine
            arColName.Add("EV_VC50_UserID") 'userid
            arColName.Add("EV_VC50_Password") 'password
            arColName.Add("EV_VC50_Database") 'database
            arColName.Add("EV_NU9_CompanyID") 'company



            arRowData.Add(intMax)
            arRowData.Add(DDLEnv_F.SelectedValue)
            arRowData.Add(txtOwner_F.Text)
            arRowData.Add(txtMachine_F.Text)
            arRowData.Add(Encrypt(txtUserID_F.Text))
            arRowData.Add(Encrypt(txtPassword_F.Text))
            arRowData.Add(txtDatabase_F.Text)
            arRowData.Add(Session("PropCAComp"))



            If SQL.Save("t130172", "ENV", "SaveRecord", arColName, arRowData, "") = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("ReportEntry", "SaveRecord-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        dgrEnvDetail.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BINDGrid()
    End Sub

    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (dgrEnvDetail.CurrentPageIndex > 0) Then
            dgrEnvDetail.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BINDGrid()
    End Sub

    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (dgrEnvDetail.CurrentPageIndex < (dgrEnvDetail.PageCount - 1)) Then
            dgrEnvDetail.CurrentPageIndex += 1

            If dgrEnvDetail.PageCount = CurrentPg.Text Then
                CurrentPg.Text = dgrEnvDetail.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BINDGrid()
    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        dgrEnvDetail.CurrentPageIndex = (dgrEnvDetail.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BINDGrid()
    End Sub


End Class
