Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_ProcessEnvironment
    Inherits System.Web.UI.Page
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
    Public mintPageSize As Integer
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer

#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Request.QueryString("FE") = "1" Then
            lstError.Items.Clear()
            cpnlErrorPanel.Visible = True
            cpnlErrorPanel.Text = "Message"
            lstError.Items.Add("Record saved successfully")
            ImgError.ImageUrl = "../images/Pok.gif"
            MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
        End If

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        txtCSS(Me.Page, "cpnlEnvironment")
        'cpnlErrorPanel.Visible = False

        'get value from call detail form for serach
        '***********************************
        imgSystemID.Attributes.Add("onclick", "OpenMachineIP('cpnlEnvironment_txtSystemID_F');")

        Dim srtQueryWhere As String
        Dim chkand As Short
        chkand = 0



        Dim intIDPK As Integer
        intIDPK = SQL.Search("ProcessEnvironment", "Load", "select max(EV_NU9_ID_PK) from T130172")
        intIDPK = intIDPK + 1
        txtIDPK_F.Text = intIDPK
        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenValue = Request.Form("txthidden")
        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SAddressNumber") = Request.Form("txthidden")
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        Response.Redirect("../Home.aspx")
                    Case "Save"
                        If CheckInfo() = True Then
                            If SaveEnvironmentInfo() = True Then
                                Response.Redirect("ProcessEnvironment.aspx?FE=1")
                            End If
                        End If
                    Case "Delete"
                        If SQL.Delete("ProcessEnvironment", "Load", "delete from T130172 where EV_NU9_ID_PK=" & txthiddenValue, SQL.Transaction.Serializable) = True Then
                            lstError.Items.Clear()
                            'cpnlErrorPanel.Visible = True
                            'cpnlErrorPanel.Text = "Message"
                            lstError.Items.Add("Record Deleted successfully...")
                            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgOK)
                            'ImgError.ImageUrl = "../images/Pok.gif"
                            'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server is busy please try later...")
                            'cpnlErrorPanel.Visible = True
                            'cpnlErrorPanel.Text = "Error Message"
                            'ImgError.ImageUrl = "../images/error_image.gif"
                            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                'cpnlErrorPanel.Visible = True
                'cpnlErrorPanel.Text = "Error Message"
                'ImgError.ImageUrl = "../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)

                CreateLog("ProcessEnv", "Load-164", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        If Not IsPostBack Then

            'chk grid width in database
            chkgridwidth()

            'fill dropdown list data from database
            ' GetView()

            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                'SQL.DBTable = "T130172"
                SQL.DBTracing = False

                '**************
                ' Dim arSetColumnName As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "

                'SQL.DBTable = "T030212"
                Dim sqlquery As String

                sqrdView = SQL.Search("ProcessEnvironment", "Load", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130172' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
                'sqrdView = SQL.Search("", "", srtQueryWhere, SQL.CommandBehaviour.CloseConnection, blnView)

                If blnView = True Then
                    While sqrdView.Read
                        If sqrdView.Item("UV_VC50_COL_Value").Equals("EV_VC50_Password") = False Then strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        arOriginalColumnName.Add(sqrdView.Item("UV_VC50_COL_Value"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    End While
                    sqrdView.Close()

                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                    strSelect &= ",'*' as EV_VC50_Password from T130172"
                    If srtQueryWhere <> "" Then
                        strSelect = strSelect & " where " + srtQueryWhere
                    End If

                    'SQL.DBTable = "T130172"

                    If SQL.Search("T130172", "ProcessEnvironment", "Load", strSelect, dsDefault, "", "") = True Then

                        'change the datagrid header columns name 
                        For inti As Integer = 0 To dsDefault.Tables("T130172").Columns.Count - 1
                            dsDefault.Tables("T130172").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        Next
                    End If
                    mdvtable.Table = dsDefault.Tables("T130172")
                End If
                '**************

                If mdvtable.Count > 0 Then
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    GrdAddSerach.DataBind()
                Else
                    Dim dtTemp As New DataTable
                    Dim dtRow As DataRow
                    dtTemp.Columns.Add("ID")
                    dtTemp.Columns.Add("Environment")
                    dtTemp.Columns.Add("Owner")
                    dtTemp.Columns.Add("SystemID")
                    dtTemp.Columns.Add("UserID")
                    dtTemp.Columns.Add("Password")
                    dtRow = dtTemp.NewRow
                    dtRow.Item(0) = ""

                    GrdAddSerach.DataSource = dtTemp
                    GrdAddSerach.DataBind()
                End If
                GetColumns()

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            Catch ex As Exception
                CreateLog("ProcessEnv", "Load-231", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        Else

            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlEnvironment:" & arCol.Item(i)))
            Next
            '**************************************
            'fill data in datagrid on load on post back event
            FillView()

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage

                        Case "Search"
                            Call BtnGrdSearch_Click(Me, New EventArgs)

                    End Select
                Catch ex As Exception
                    CreateLog("ProcessEnv", "Load-254", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                End Try
            End If

        End If

        ''Security Block

        'Dim intId As Integer

        'If Not IsPostBack Then
        '    Dim str As String
        '    str = HttpContext.Current.Session("PropRootDir")
        '    intId = Request.QueryString("ScrID")
        '    Dim obj As New clsSecurityCache
        '    If obj.ScreenAccess(intId) = False Then
        '        Response.Redirect("../../frm_NoAccess.aspx")
        '    End If
        '    obj.ControlSecurity(Me.Page, intId)
        'End If
        ''End of Security Block
    End Sub

#End Region

#Region "fill View"

    Private Function FillView()
        Dim intViewID As Integer = 0
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim arcolName As New ArrayList

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String

            sqrdView = SQL.Search("ProcessEnvironment", "FillView", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130172' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            'If SQL.Search("", "", strConnection, "select name,view_id from tbl_userview_name", dsView, "tbl_userview_detail") = True Then
            If blnView = True Then
                Dim dsFromView As New DataSet
                arColumnName.Clear()
                arCol.Clear()
                'arColWidth2.Clear()
                arColWidth.Clear()

                While sqrdView.Read
                    If sqrdView.Item("UV_VC50_COL_Value").Equals("EV_VC50_Password") = False Then strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_Width"))
                End While

                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
                strSelect &= ",'*' as EV_VC50_Password from T130172"

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

                'SQL.DBTable = "T130172"

                If SQL.Search("T130172", "ProcessEnvironment", "FillView", strSelect, dsFromView, "", "") = True Then
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                    Next

                    mdvtable.Table = dsFromView.Tables(0)
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.Columns.Clear()

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.DataBind()
                    GetColumns()
                    CreateTextBox()
                Else
                End If
            Else
                Exit Function
            End If
        Catch ex As Exception
            CreateLog("ProcessEnv", "FillView-341", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

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
            'SQL.DBTable = "T030212"

            'query on database
            sqrdView = SQL.Search("ProcessEnvironment", "FillGrid_data", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130172'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'fill the value and name into the diffrent arrays
                While sqrdView.Read
                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
                sqrdView.Close()

                'build the query
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T130172"

                '	SQL.DBTable = "T130172"

                'changing the dataset columns header title from database 
                If SQL.Search("T130172", "ProcessEnvironment", "FillGrid_data", strSelect, dsDefault, "", "") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T130172").Columns.Count - 1
                        dsDefault.Tables("T130172").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If

                arSetColumnName.Clear()
                GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T130172")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

                'call formating function for format datagrid according to database size
                GetColumns()

            End If
        Catch ex As Exception
            CreateLog("ProcessEnv", "FillGrid_Data-405", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
            'SQL.DBTable = "T030201"
            SQL.DBTracing = False

            sqrdView = SQL.Search("ProcessEnvironment", "GetView", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T130172'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then

            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("ProcessEnv", "GetView-433", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Function CreateTextBox()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arColumns.Clear()

        'fill the columns count into the array from mdvtable view
        intCol = mdvtable.Table.Columns.Count

        If Not IsPostBack Then
            ReDim mTextBox(intCol)
        End If

        Try
            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
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

                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox  MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("ProcessEnv", "CreateTextBox-502", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "create textbox for User View"

    Private Function CreateViewTextBox()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arColumns.Clear()

        'fill the columns count into the array from mdvtable view
        intCol = mdvtable.Table.Columns.Count

        If Not IsPostBack Then
            ReDim mTextBox(intCol)
        End If

        Try
            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    arCol.Add(arOriginalColumnName.Item(intii))
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arOriginalColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & "></asp:TextBox>"))
                    _textbox.ID = arOriginalColumnName.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    '_textbox.Text = arrtextvalue.Item(intii)
                    _textbox.Text = ""
                    strcolid = arCol.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " ></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("ProcessEnv", "CreateViewTextBox-562", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "ReSizeGrid"

    Private Sub ReSizeGrid()
        Dim intCount As Integer
        Dim bcl As BoundColumn = New BoundColumn
        Dim count As Integer

        intCount = mdvtable.Table.Columns.Count
        GrdAddSerach.AutoGenerateColumns = False

        For count = 0 To arColumnName.Count - 1
            'we can also bind it to the datatable   
            bcl.HeaderStyle.Width = New System.Web.ui.WebControls.Unit(150)
            GrdAddSerach.Columns.Add(bcl)
        Next

        GrdAddSerach.DataSource = mdvtable.Table
        GrdAddSerach.DataBind()
    End Sub
#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid(ByVal ColumnName As ArrayList)
        Dim intI As Integer

        Try
            GrdAddSerach.AutoGenerateColumns = False
            chkgridwidth()

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
            CreateLog("ProcessEnv", "FormatGrid-612", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region


#Region "get columns from database"

    Private Sub GetColumns()
        'Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            '  SQL.DBTable = "T030212"
            SQL.DBTracing = False

            sqrdView = SQL.Search("ProcessEnvironment", "GetColumns", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130172' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                Dim dsFromView As New DataSet
                arColumnName.Clear()
                arColWidth.Clear()
                While sqrdView.Read
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_width"))
                    arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
            End If
            FormatGrid(arColumnName)
            sqrdView.Close()
        Catch ex As Exception
            CreateLog("ProcessEnv", "GetColumns-646", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        Dim sqrdView As SqlDataReader
        Dim blnView As Boolean
        'Dim intViewID As Integer = ddlstview.SelectedValue
        Dim arCol As New ArrayList

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        '  SQL.DBTable = "T030212"
        SQL.DBTracing = False

        sqrdView = SQL.Search("ProcessEnvironment", "chkgridwidth", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130172' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
        arColWidth.Clear()

        If blnView = True Then
            arColWidth.Clear()
            Dim dsFromView As New DataSet
            While sqrdView.Read
                arColWidth.Add(sqrdView.Item("UV_VC10_Col_width"))
                arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
            End While
            sqrdView.Close()
        End If
    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        If CheckInfo() = True Then
            If SaveEnvironmentInfo() = True Then
                Response.Redirect("ProcessEnvironment.aspx?FE=1")
            End If
        Else
            cpnlErrorPanel.Visible = False
        End If

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text

                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"

                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
                            End If
                        End If

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
                            strSearch = strSearch.Replace("*", "")
                            If IsNumeric(strSearch) = False Then
                                strSearch = "-101"
                            End If
                        End If
                        ' mdvTable.Table.Columns(intI).DataType.FullName = System.DateTime
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mTextBox(intI).Text.Trim
                        strSearch = GetSearchString(strSearch)

                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
                shF = 1
                GrdAddSerach.Columns.Clear()
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If

                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                GetColumns()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.AutoGenerateColumns = True
            GrdAddSerach.DataBind()

            GetColumns()
        Catch ex As Exception
            CreateLog("ProcessEnv", "Click-754", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String

        GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strTempName = e.Item.Cells(1).Text

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & strTempName & "')")
                    ' e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "', '" & strTempName & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheckProcessEnvironmentEdit(" & strID & ", '" & rowvalue & "', 'cpnlEnvironment_GrdAddSerach')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("ProcessEnv", "ItemDataBound-785", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

    Private Function SaveEnvironmentInfo() As Boolean
        Try

            If ValidateEnvironmentInfo() = True Then

                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '     SQL.DBTable = "T130172"
                SQL.DBTracing = False

                arColumnName.Add("EV_NU9_ID_PK")
                arColumnName.Add("EV_VC30_Environment_Name")
                arColumnName.Add("EV_VC30_Owner")
                arColumnName.Add("EV_VC100_SystemID")
                arColumnName.Add("EV_VC50_UserID")
                arColumnName.Add("EV_VC50_Password")

                arRowData.Add(txtIDPK_F.Text.Trim)
                arRowData.Add(txtEnvironmentName_F.Text.Trim)
                arRowData.Add(txtOwner_F.Text.Trim)
                arRowData.Add(txtSystemID_F.Text.Trim)
                arRowData.Add(txtUserID_F.Text.Trim)
                arRowData.Add(txtPassword_F.Text.Trim)

                If SQL.Save("T130172", "ProcessEnvironment", "SaveEnvironmentInfo", arColumnName, arRowData) = True Then
                    lstError.Items.Clear()
                    'cpnlErrorPanel.Visible = True
                    'cpnlErrorPanel.Text = "Message"
                    lstError.Items.Add("Record saved successfully...")
                    ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgOK)
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'cpnlErrorPanel.Visible = True
                    'cpnlErrorPanel.Text = "Error Message"
                    'ImgError.ImageUrl = "../images/error_image.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If

            End If
        Catch ex As Exception
            'lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlErrorPanel.Visible = True
            'cpnlErrorPanel.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEnv", "SaveEnvinfo-785", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try

    End Function
    Private Function ValidateEnvironmentInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtEnvironmentName_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Environment Name  cannot be empty...")
        End If
        If txtOwner_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Database Owner cannot be empty...")
        End If
        If txtSystemID_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("SystemID cannot be empty...")
        End If
        If txtUserID_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("UserID cannot be empty...")
        End If
        If txtUserID_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Password cannot be empty...")
        End If
        If shFlag = 1 Then
            'cpnlErrorPanel.Visible = True
            'cpnlErrorPanel.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False

        End If
        lstError.Items.Clear()
        Try
            Dim introws As Integer

            If SQL.Search("ProcessEnvironment", "ValidateEnvironmentInfo", "select  MM_IN4_MCode from t130011 where MM_VC20_MIP='" & txtSystemID_F.Text & "'", introws) = False Then
                lstError.Items.Add("SystemID Mismatch...")
                shFlag = 1
            End If

            If shFlag = 1 Then
                'cpnlErrorPanel.Visible = True
                'cpnlErrorPanel.Text = "Error Message"
                'ImgError.ImageUrl = "../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If

            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlErrorPanel.Visible = True
            'cpnlErrorPanel.Text = "error message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEnv", "ValidateEnvinfo-905", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function CheckInfo() As Boolean
        If txtEnvironmentName_F.Text.Equals("") And txtOwner_F.Text.Equals("") And txtSystemID_F.Text.Equals("") And txtUserID_F.Text.Equals("") Then
            Return False
        Else
            Return True
        End If
    End Function
End Class
