#Region "Purpose"
'************************************************************************************************************
' Page                 :- User Search
' Purpose              :- It will display list of users
' Tables used          :- T010011, T070042,T070031
' Date				Author						Modification Date					Description
' 13/03/06			Amandeep/jaswinder			-------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
#End Region
#Region "Session Used"
'Session("PageSize") replace with   ViewState("PageSize")
'Session("SortOrder") replace with ViewState("SortOrder")
'Session("SortWay") replace with ViewState("SortWay")
'ViewState("PropShowAllUser")
'Session("SUserID") Replace with ViewState("User_SearchUserID")
'Session("PropRootDir") Fill on Home page
#End Region

#Region "NameSpace"
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports ION.Logging
#End Region

Partial Class Security_User_Search
    Inherits System.Web.UI.Page
    Protected WithEvents Toolbar1 As Microsoft.Web.UI.WebControls.Toolbar
    Protected WithEvents Toolbar2 As Microsoft.Web.UI.WebControls.Toolbar
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents LblErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button5 As System.Web.UI.WebControls.Button
    Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton

#Region "global level declaration"


    ' Holding text boxes created above UDC Type Grid
    Dim mtxtUDCTypeQuery As TextBox()

    Private Shared arColWidth As New ArrayList
    Dim flage As Integer
    Dim mdvtable As New DataView
    Dim marTextbox() As TextBox
    Private Shared mTextBox() As TextBox
    Dim mintColumns As Integer
    Dim mshFlag As Short
    Dim Expanded2 As New PlaceHolder
    Dim ii As WebControls.Unit
    Dim rowvalue As Integer
    Dim shF As Short
    Dim flg As Short

    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer
    Public mintPageSize As Integer
    Protected _currentPageNumber As Int32 = 1

#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(Session.Timeout) * 60) & ";Login.aspx"" />")
        'Response.CacheControl = "no-cache"
        'Response.AddHeader("Pragma", "no-cache")
        'Response.Expires = -1
        If Not IsPostBack Then
            ViewState("PropShowAllUser") = "0"
            'Session("SortOrder") = Nothing
            ViewState("SortOrder") = Nothing
            ''Session("SortWay") = 0
            ViewState("SortWay") = 0
        Else
        End If

        If Not IsNothing(ViewState("PropShowAllUser")) Then
            If ViewState("PropShowAllUser") = "0" Then
                imgShowAll.ToolTip = "Show All"
            Else
                imgShowAll.ToolTip = "Hide Disabled"
            End If
        End If
        imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
        imgShowAll.Attributes.Add("Onclick", "return SaveEdit('ShowAll');")
        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenAdno = Request.Form("txthiddenAdno")

        'paging
        '******************************************
        mintPageSize = Val(Request.Form("txtPageSize"))


        If IsPostBack = False Then
            txtCSS(Me.Page)
            If ChkPageView() = True Then
                ''txtPageSize.Text = Session("PageSize")
                ''mintPageSize = Session("PageSize")
                txtPageSize.Text = ViewState("PageSize")
                mintPageSize = ViewState("PageSize")
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = 20
                    txtPageSize.Text = mintPageSize
                    ''Session("PageSize") = mintPageSize
                    ViewState("PageSize") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                    ''Session("PageSize") = mintPageSize
                    SavePageSize()
                End If
            End If
        Else
            ''If Session("PageSize") = mintPageSize Then
            If ViewState("PageSize") = mintPageSize Then
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    ''mintPageSize = Session("PageSize")
                    ''txtPageSize.Text = Session("PageSize")
                    mintPageSize = ViewState("PageSize")
                    txtPageSize.Text = ViewState("PageSize")
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                    ''Session("PageSize") = mintPageSize
                End If

                SavePageSize()
            End If
        End If
        '******************************************

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        If Request.Form("txthidden") = "" Then
        Else
            ''Session("SUserID") = Request.Form("txthidden")
            ViewState("User_SearchUserID") = Request.Form("txthidden")
            'Not used in the page
            ''Session("SUser") = Request.Form("txthiddenUser")
            ''Session("SRole") = Request.Form("txthiddenRole")
            ''Session("SCompany") = Request.Form("txthiddenCompany")

        End If

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "ShowAll"
                        If Not IsNothing(ViewState("PropShowAllUser")) Then
                            If ViewState("PropShowAllUser") = "1" Then
                                ViewState("PropShowAllUser") = "0"
                            Else
                                ViewState("PropShowAllUser") = "1"
                            End If
                        Else
                            ViewState("PropShowAllUser") = "1"
                        End If
                    Case "Edit"
                        '  Response.Redirect("UserManage.aspx?ID=-1&ScrID=258&UserID=" + ViewState("User_SearchUserID"), False)
                    Case "Add"
                        ''Session("SUserID") = ""
                        ViewState("User_SearchUserID") = ""
                        ' Response.Redirect("UserManage.aspx?ID=1&ScrID=258", False)
                    Case "Delete"
                        ''If Session("SUserID") <> "" Then
                        If ViewState("User_SearchUserID") <> "" Then
                            ''If WSSDelete.DeleteUserProfile(Val(Session("SUserID"))) = True Then
                            If WSSDelete.DeleteUserProfile(Val(ViewState("User_SearchUserID"))) = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            Else
                                lstError.Items.Clear()
                                lstError.Items.Add("You cannot delete this record as it has been used in other modules...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            End If
                        End If

                    Case "Logout"
                        LogoutWSS()
                End Select

            Catch ex As Exception
                Dim str As String = ex.ToString
            End Try
        End If


        If Not IsNothing(ViewState("PropShowAllUser")) Then
            If ViewState("PropShowAllUser") = "0" Then
                imgShowAll.ToolTip = "Show All"
            Else
                imgShowAll.ToolTip = "Hide Disabled"
            End If
        End If

        If Not IsPostBack Then

            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T060011"
                SQL.DBTracing = False

                '**************
                ' Dim arSetColumnName As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "

                GrdAddSerach.PageSize = mintPageSize ' set the grid page size

                arColumnName.Clear()
                arCol.Clear()
                arSetColumnName.Clear()
                arColWidth.Clear()

                ' SQL.DBTable = "T030212"
                sqrdView = SQL.Search("User_search", "load-179", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T060011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
                If blnView = True Then
                    While sqrdView.Read

                        If sqrdView.Item("UV_VC50_COL_Value") = "UM_DT8_To_date" Then
                            strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101) ValidUpTo" & ","
                        ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC4_Status_Code_FK" Then
                            strSelect &= "case UM_VC4_Status_Code_FK when 'ENB' then 'Enabled' when 'DNB' then 'Disabled' end " & ","
                        ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC4_User_Type_FK" Then
                            strSelect &= "case UM_VC4_User_Type_FK when 'INTR' then 'Internal' when 'EXTR' then 'External' end " & ","
                        Else
                            strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        End If
                        arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                        arColWidth.Add(sqrdView.Item("UV_VC10_Col_width")) 'adding columns widthe in arraylist

                    End While

                    sqrdView.Close()
                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                    strSelect &= " from T060011,T010011,T010011 UName where T010011.CI_NU8_Address_Number=T060011.UM_IN4_Company_AB_ID  AND UName.CI_NU8_Address_Number=T060011.UM_IN4_Address_no_fk  "

                    strSelect &= " AND UM_IN4_Company_AB_ID in (" & GetCompanySubQuery() & ") "

                    If ViewState("PropShowAllUser") = "0" Then
                        strSelect &= "AND UM_VC4_Status_Code_FK='ENB' "
                    End If

                    ' SQL.DBTable = "T060011"
                    If SQL.Search("T060011", "User_search", "load-192", strSelect, dsDefault, "sachin", "Prashar") = True Then


                    End If
                    'change the datagrid header columns name 
                    For inti As Integer = 0 To dsDefault.Tables("T060011").Columns.Count - 1
                        dsDefault.Tables("T060011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                    mdvtable.Table = dsDefault.Tables("T060011")
                End If
                '**************

                Dim htDateCols As New Hashtable
                htDateCols.Add("ValidUpTo", 2)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)


                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke

                If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If

                GrdAddSerach.DataBind()


                ''paging count
                Dim intRows As Integer = mdvtable.Table.Rows.Count
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

                'create textbox at run time at head of the datagrid        
                CreateTextBox()
                CurrentPg.Text = _currentPageNumber.ToString()

            Catch ex As Exception
                CreateLog("AB_Search", "Load-195", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            End Try
        Else

            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form(arCol.Item(i)))
            Next
            '**************************************
            'fill data in datagrid on load on post back event
            FillView()
            CreateTextBox()

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage

                        Case "Search"
                            Call BtnGrdSearch_Click(Me, New EventArgs)

                    End Select

                Catch ex As Exception
                    CreateLog("AB_Search", "Load-218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
                End Try
            End If


            If ChechkValidityforSearch(arrtextvalue) = True Then
                Call BtnGrdSearch_Click(Me, New EventArgs)
            End If

            ''If IsNothing(Session("SortOrder")) = False Then
            If IsNothing(ViewState("SortOrder")) = False Then
                SortGRDDuplicate()
            End If


        End If


        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        'End of Security Block

    End Sub

#End Region

#Region "fill View"

    Private Sub FillView()
        Dim intViewID As Integer = 0
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        ' Dim strConnection As String = SQL.GetConncetionString("strConnectionString")
        Dim arcolName As New ArrayList
        GrdAddSerach.PageSize = mintPageSize ' set the grid page size

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        ' SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String

            sqrdView = SQL.Search("user_search-280", "FillView-280", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T060011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                Dim dsFromView As New DataSet

                arColumnName.Clear()
                arCol.Clear()
                arColWidth.Clear()

                While sqrdView.Read

                    If sqrdView.Item("UV_VC50_COL_Value") = "UM_DT8_To_date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101) ValidUpTo" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC4_Status_Code_FK" Then
                        strSelect &= "case UM_VC4_Status_Code_FK when 'ENB' then 'Enabled' when 'DNB' then 'Disabled' end " & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC4_User_Type_FK" Then
                        strSelect &= "case UM_VC4_User_Type_FK when 'INTR' then 'Internal' when 'EXTR' then 'External' end " & ","
                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If
                    'strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_Width"))

                End While

                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                ' strSelect &= " from T060011,T010011 where T010011.CI_NU8_Address_Number=T060011.UM_IN4_Company_AB_ID "
                strSelect &= " from T060011,T010011,T010011 UName where T010011.CI_NU8_Address_Number=T060011.UM_IN4_Company_AB_ID  AND UName.CI_NU8_Address_Number=T060011.UM_IN4_Address_no_fk  "

                strSelect &= " AND UM_IN4_Company_AB_ID in (" & GetCompanySubQuery() & ") "

                If ViewState("PropShowAllUser") = "0" Then
                    strSelect &= " AND UM_VC4_Status_Code_FK='ENB' "
                End If

                If IsNothing(strUnsortQuery) = False Then
                    strUnsortQuery = strUnsortQuery.TrimEnd
                    strUnsortQuery = strUnsortQuery.Remove(Len(strUnsortQuery) - 1, 1)
                    If IsNothing(strOrderQuery) = True Then
                        strSelect &= strUnsortQuery
                    Else
                        strSelect &= strOrderQuery & " " & strUnsortQuery
                    End If
                Else
                    If strOrderQuery.Equals(" order by ") = False Then
                        strOrderQuery = strOrderQuery.TrimEnd
                        strOrderQuery = strOrderQuery.Remove(Len(strOrderQuery) - 1, 1)
                        strSelect &= strOrderQuery
                    End If
                End If

                If SQL.Search("T060011", "", "", strSelect, dsFromView, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                    Next
                    rowvalue = 0
                    mdvtable.Table = dsFromView.Tables(0)
                    Dim htDateCols As New Hashtable
                    htDateCols.Add("ValidUpTo", 2)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    ' GrdAddSerach.Columns.Clear()
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If
                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If
                    GrdAddSerach.DataBind()
                    ''paging count
                    Dim intRows As Integer = mdvtable.Table.Rows.Count
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
                Else
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("AB_Search", "FillView-293", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Sub

#End Region

#Region "Fill data into datagrid and change the header name according the database"

    Private Sub FillGrid_data()
        'Dim intViewID As Integer = ddlstview.SelectedValue

        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim strConnection As String
        Dim dsDefault As DataSet
        Dim arSetColumnName As ArrayList

        'call connection 
        strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        Try
            dsDefault = New DataSet
            arSetColumnName = New ArrayList
            ' pass table name
            ' SQL.DBTable = "T030212"

            'query on database
            sqrdView = SQL.Search("user_Search", "FillGrid_data-375", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T060011'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'fill the value and name into the diffrent arrays
                While sqrdView.Read
                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
                sqrdView.Close()

                'build the query
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T060011"

                ' SQL.DBTable = "T060011"

                'changing the dataset columns header title from database 
                If SQL.Search("T060011", "user_Search", "FillGrid_data-375", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T060011").Columns.Count - 1
                        dsDefault.Tables("T060011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If

                arSetColumnName.Clear()
                ' GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T060011")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

                'call formating function for format datagrid according to database size
                '   GetColumns()

            End If
        Catch ex As Exception
            CreateLog("AB_Search", "FillGrid_Data-357", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "fill data into the dropdown from view table "

    '''' fill value into the dropdown name and id of the field view table

    Private Sub GetView()

        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T030201"
            SQL.DBTracing = False

            sqrdView = SQL.Search("User_Search", "GetView-430", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T060011'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then

            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("AB_Search", "GetView-389", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Sub CreateTextBox()

        Try
            Dim _textbox As TextBox
            Dim intii As Integer

            arColumns.Clear()
            'fill the columns count into the array from mdvtable view
            intCol = arSetColumnName.Count  'mdvtable.Table.Columns.Count

            If Not IsPostBack Then
                ReDim mTextBox(intCol)
            End If

            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value - 2
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(arSetColumnName.Item(intii))
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arSetColumnName.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"

                    If arrtextvalue.Count <> arSetColumnName.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15"" ></asp:TextBox>"))
                    _textbox.ID = arSetColumnName(intii)

                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next

        Catch ex As Exception
            CreateLog("AB_Search", "CreateTextBox-458", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region


#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid(ByVal ColumnName As ArrayList)
        Dim intI As Integer

        Try
            GrdAddSerach.AutoGenerateColumns = False
            ' chkgridwidth()

            For intI = 0 To ColumnName.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                GrdAddSerach.Columns.Add(Bound_Column)
            Next
            ColumnName.Clear()

        Catch ex As Exception
            CreateLog("AB_Search", "FormatGrid-562", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            If IsNothing(mdvtable.Table) = False Then

                For intI As Integer = 0 To arColumns.Count - 1
                    If Not mTextBox(intI).Text.Trim.Equals("") Then
                        strSearch = mTextBox(intI).Text
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") Then
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                                If IsDate(strSearch) = False Then
                                    'fill own date if some body fill wrong data in date filled 
                                    strSearch = "12/12/1825"
                                End If
                            End If
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") = True Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Then
                                'strSearch = CInt(strSearch)
                                strSearch = strSearch.Replace("*", "")
                                If IsNumeric(strSearch) = False Then
                                    'fill own data if somebody fill wrong data in numaric field
                                    strSearch = "-101"
                                End If
                            End If
                            ' mdvTable.Table.Columns(intI).DataType.FullName = System.DateTime
                            strSearch = strSearch.Replace("*", "")
                            strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            strSearch = mTextBox(intI).Text.Trim
                            strSearch = GetSearchString(strSearch)
                            'strSearch = strSearch.Replace("*", "%")
                            If strSearch.Contains("*") = True Then
                                strSearch = strSearch.Replace("*", "%")
                            Else
                                strSearch &= "%"
                            End If
                            strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                        End If
                    End If
                Next
                If (strRowFilterString Is Nothing) Then
                    shF = 1
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.DataSource = mdvtable
                    GrdAddSerach.DataBind()
                    '  GetColumns()
                    Exit Sub
                End If

                strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
                mdvtable.RowFilter = strRowFilterString

                ' GrdAddSerach.Columns.Clear()
                GetFilteredDataView(mdvtable, strRowFilterString)
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.AutoGenerateColumns = True

                If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If

                GrdAddSerach.DataBind()

                ''paging count
                Dim intRows As Integer = mdvtable.Table.Rows.Count
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

                'GetColumns()
                If mdvtable.Count = 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Data not found according to your search string... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            End If

        Catch ex As Exception
            CreateLog("AB_Search", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound
        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "')")
                End If
            Next
            rowvalue += 1
        Catch ex As Exception
            CreateLog("AB_Search", "ItemDataBound-720", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            Dim intA As Integer = 0

            For intI = 0 To arColWidth.Count - 1 + 2
                If intI > 1 Then
                    If e.Item.Cells.Count > 1 Then
                        ' e.Item.Cells(intA + 2).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                        e.Item.Cells(intA).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                    End If
                    intA += 1
                ElseIf intI = 0 Then
                    e.Item.Cells(0).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")

                End If
            Next

        Catch ex As Exception

        End Try


    End Sub


    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        GrdAddSerach.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber

        FillView()

        If ChechkValidityforSearch(arrtextvalue) = True Then
            Call BtnGrdSearch_Click(Me, New EventArgs)
        End If

        ''If IsNothing(Session("SortOrder")) = False Then
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If

    End Sub
    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click

        If (GrdAddSerach.CurrentPageIndex > 0) Then
            GrdAddSerach.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If

        FillView()

        If ChechkValidityforSearch(arrtextvalue) = True Then
            Call BtnGrdSearch_Click(Me, New EventArgs)
        End If

        ''If IsNothing(Session("SortOrder")) = False Then
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If

    End Sub
    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (GrdAddSerach.CurrentPageIndex < (GrdAddSerach.PageCount - 1)) Then
            GrdAddSerach.CurrentPageIndex += 1
            If GrdAddSerach.PageCount = Int32.Parse(CurrentPg.Text) Then
                CurrentPg.Text = GrdAddSerach.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber

            End If
        End If

        FillView()

        If ChechkValidityforSearch(arrtextvalue) = True Then
            Call BtnGrdSearch_Click(Me, New EventArgs)
        End If

        ''If IsNothing(Session("SortOrder")) = False Then
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If

    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        GrdAddSerach.CurrentPageIndex = (GrdAddSerach.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber


        FillView()

        If ChechkValidityforSearch(arrtextvalue) = True Then
            Call BtnGrdSearch_Click(Me, New EventArgs)
        End If

        'If IsNothing(Session("SortOrder")) = False Then
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If

    End Sub
    Private Sub SortGRD()
        Try
            If Not IsNothing(mdvtable.Table) Then
                ''If Val(Session("SortWay")) Mod 2 = 0 Then
                If Val(ViewState("SortWay")) Mod 2 = 0 Then
                    ''mdvtable.Sort = Session("SortOrder") & " ASC"
                    mdvtable.Sort = ViewState("SortOrder") & " ASC"
                Else
                    ''mdvtable.Sort = Session("SortOrder") & " DESC"
                    mdvtable.Sort = ViewState("SortOrder") & " DESC"
                End If
                'Session("SortWay") += 1
                ViewState("SortWay") += 1
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                rowvalue = 0
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SortGRDDuplicate()

        Try
            ' If SortWay Mod 2 = 0 Then
            ''If Val(Session("SortWay")) Mod 2 = 0 Then
            If Val(ViewState("SortWay")) Mod 2 = 0 Then
                ''mdvtable.Sort = Session("SortOrder") & " DESC"
                mdvtable.Sort = ViewState("SortOrder") & " DESC"
            Else
                ''mdvtable.Sort = Session("SortOrder") & " ASC"
                mdvtable.Sort = ViewState("SortOrder") & " ASC"
            End If

            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If

            rowvalue = 0

            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.DataBind()
            ' GridRowSelection()
        Catch ex As Exception
        End Try

    End Sub

    Private Sub GrdAddSerach_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddSerach.SortCommand

        ''Session("SortOrder") = e.SortExpression
        ViewState("SortOrder") = e.SortExpression
        SortGRD()

    End Sub
    Private Sub SavePageSize()
        Dim intid = 63
        Dim strCheck As String = SQL.Search("Historicview", "SavePageSize-3406", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "")
        If Not IsNothing(strCheck) Then
            'update
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            arColumnName.Add("PS_NU9_PSize")
            arRowData.Add(Val(ViewState("PageSize")))
            ''arRowData.Add(Val(Session("PageSize")))
            If SQL.Update("T030214", "SavePageSIZE", "update  T030214 set PS_NU9_PSize=" & Val(ViewState("PageSize")) & "  where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid"), SQL.Transaction.Serializable) = True Then
                'Save message
            Else
                'Error message
            End If
        Else
            'save
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("PS_NU9_PSize")
            arColumnName.Add("PS_NU9_ScreenID")
            arColumnName.Add("PS_NU9_RoleID")
            arColumnName.Add("PS_NU9_ComID")
            arColumnName.Add("PS_NU9_UserID") 'Added new field to store user id with view records

            ''arRowData.Add(Val(Session("PageSize")))
            arRowData.Add(Val(ViewState("PageSize")))
            arRowData.Add(intid)
            arRowData.Add(Session("PropRole"))
            arRowData.Add(Session("PropCompanyID"))
            arRowData.Add(Session("PropUserID"))

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Save("T030214", "SaveUserView", "SaveUserView-3436", arColumnName, arRowData) = True Then
                'Save message
            Else
                'Error message
            End If
        End If
    End Sub

    Private Function ChkPageView() As Boolean
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=63 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

            If blnReturn = False Then
                Return False
                Exit Function
            Else
                While sqdrCol.Read
                    ''Session("PageSize") = sqdrCol.Item("PS_NU9_PSize")
                    ViewState("PageSize") = sqdrCol.Item("PS_NU9_PSize")
                End While
                Return True
            End If

            sqdrCol.Close()
            sqdrCol = Nothing

        Catch ex As Exception
            'ddlstview.SelectedValue = 0
            CreateLog("Task_View", "ChkSelectedView-2080", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
End Class
