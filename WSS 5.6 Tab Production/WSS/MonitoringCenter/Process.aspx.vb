Imports ION.Net
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_Process
    Inherits System.Web.UI.Page
    Public shFlag As Short
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

        txtProcessCode_F.Attributes.Add("onkeypress", "NumericOnly('cpnlProcessSearch_txtProcessCode_F')")
        imgProcessType.Attributes.Add("onclick", "OpenW(0,'PRTY','cpnlProcessSearch_txtProcessType_F');")
        imgProcessEnv.Attributes.Add("onclick", "OpenProcessEnvironment('cpnlProcessSearch_txtProcessEnv_F');")
        If Request.QueryString("FE") = "1" Then
            lstError.Items.Clear()
            'cpnlErrorPanel.Visible = True
            'cpnlErrorPanel.Text = "Message"
            lstError.Items.Add("Record saved successfully...")
            'ImgError.ImageUrl = "../images/Pok.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgOK)
        Else
            cpnlErrorPanel.Visible = False
        End If

        txtCSS(Me.Page, "cpnlProcessSearch")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")



        Dim srtQueryWhere As String
        Dim chkand As Short
        chkand = 0

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
                    Case "Edit"
                        Response.Redirect("ProcessEntry.aspx?ID='" & txthiddenValue & "'")
                    Case "Save"
                        If CheckInfo() = True Then
                            If SaveProcessInfo() = True Then
                                'Response.Redirect("Process.aspx?FE=1")
                            End If
                        Else
                            cpnlErrorPanel.Visible = False
                        End If
                    Case "Delete"
                        If SQL.Delete("Process", "Load", "delete from T130031 where PM_IN4_PCode=" & txthiddenValue, SQL.Transaction.Serializable) = True Then
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
                ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
                'cpnlErrorPanel.Visible = True
                'cpnlErrorPanel.Text = "Error Message"
                'ImgError.ImageUrl = "../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)

                CreateLog("Process", "Load-167", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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
                'SQL.DBTable = "T130031"
                SQL.DBTracing = False

                '**************
                ' Dim arSetColumnName As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "

                'SQL.DBTable = "T030212"
                Dim sqlquery As String

                sqrdView = SQL.Search("Process", "Load", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130031' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
                'sqrdView = SQL.Search("", "", srtQueryWhere, SQL.CommandBehaviour.CloseConnection, blnView)

                If blnView = True Then
                    While sqrdView.Read
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        arOriginalColumnName.Add(sqrdView.Item("UV_VC50_COL_Value"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    End While
                    sqrdView.Close()

                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                    strSelect &= " from T130031"
                    If srtQueryWhere <> "" Then
                        strSelect = strSelect & " where " + srtQueryWhere
                    End If
                    'SQL.DBTable = "T130031"
                    If SQL.Search("T130031", "Process", "Load", strSelect, dsDefault, "", "") = True Then

                        'change the datagrid header columns name 
                        For inti As Integer = 0 To dsDefault.Tables("T130031").Columns.Count - 1
                            dsDefault.Tables("T130031").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        Next
                    End If
                    mdvtable.Table = dsDefault.Tables("T130031")
                End If
                '**************

                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
                GetColumns()

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            Catch ex As Exception
                CreateLog("Process", "Load-232", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlProcessSearch:" & arCol.Item(i)))
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
                    CreateLog("Process", "Load-255", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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
        ' Dim strConnection As String = SQL.GetConncetionString("strConnectionString")
        Dim arcolName As New ArrayList

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        ' SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String

            sqrdView = SQL.Search("Process", "FillView", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130031' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            'If SQL.Search("", "", strConnection, "select name,view_id from tbl_userview_name", dsView, "tbl_userview_detail") = True Then
            If blnView = True Then
                Dim dsFromView As New DataSet
                arColumnName.Clear()
                arCol.Clear()
                'arColWidth2.Clear()
                arColWidth.Clear()

                While sqrdView.Read

                    ' Check for sort order of the column and if AD value is not unsorted
                    'If sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                    '    strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "

                    '    ' Check for sort order of the column and if AD value is unsorted
                    'ElseIf sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower = "unsorted" Then
                    '    strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & ", "

                    '    ' If sort order of the column =0 and AD value is not unsorted
                    'ElseIf sqrdView.Item("UV_NU9_SO") = 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                    '    strUnsortQuery = sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                    'End If

                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_Width"))

                End While

                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
                strSelect &= " from T130031"

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


                'SQL.DBTable = "T130031"

                If SQL.Search("T130031", "Process", "FillView", strSelect, dsFromView, "", "") = True Then
                    ' DataGrid1.DataSource = dsFromView
                    'dsFromView.Tables(0).Columns(0).ColumnName = ""
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
                    '  DataGrid1.Visible = True
                    ' ReSizeGrid()
                    GetColumns()

                    CreateTextBox()
                Else
                End If
            Else
                Exit Function
            End If
        Catch ex As Exception
            CreateLog("Process", "FillView-364", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            sqrdView = SQL.Search("Process", "FillGrid_data", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130031'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'fill the value and name into the diffrent arrays
                While sqrdView.Read
                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
                sqrdView.Close()

                'build the query
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T130031"

                'SQL.DBTable = "T130031"

                'changing the dataset columns header title from database 
                If SQL.Search("T130031", "Process", "Fillgrid_data", strSelect, dsDefault, "", "") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T130031").Columns.Count - 1
                        dsDefault.Tables("T130031").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If

                arSetColumnName.Clear()
                GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T130031")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

                'call formating function for format datagrid according to database size
                GetColumns()

            End If
        Catch ex As Exception
            CreateLog("Process", "FillGrid_data-428", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

            sqrdView = SQL.Search("Process", "GetView", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T130031'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'ddlstview.DataSource = sqrdView
                'ddlstview.DataTextField = "UV_VC50_View_Name"
                'ddlstview.DataValueField = "UV_IN4_View_ID"
                'ddlstview.DataBind()
                'sqrdView.Close()
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("Process", "GetView-460", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("Process", "CreateTextBox-529", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            CreateLog("Process", "CreateViewTextBox-589", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

#End Region

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
            CreateLog("Process", "FormatGrid-636", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            ' SQL.DBTable = "T030212"
            SQL.DBTracing = False

            sqrdView = SQL.Search("", "", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130031' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("Process", "GetColumns-670", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

        sqrdView = SQL.Search("Process", "chkgridwidth", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130031' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
            If SaveProcessInfo() = True Then
                Response.Redirect("Process.aspx?FE=1")
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
            CreateLog("Process", "Click-777", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdsearch")
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
                    '	e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "', '" & strTempName & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheckProcess(" & strID & ", '" & rowvalue & "', 'cpnlProcessSearch_GrdAddSerach')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("Process", "ItemdataBound-808", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try
    End Sub

#End Region

    Private Function SaveProcessInfo() As Boolean
        Try

            If ValidateProcessInfo() = True Then

                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T130031"
                SQL.DBTracing = False

                arColumnName.Add("PM_VC20_PENV")
                arColumnName.Add("PM_IN4_PCode")
                arColumnName.Add("PM_VC20_PName")
                arColumnName.Add("PM_VC10_PType")
                arColumnName.Add("PM_VC500_PDesc")
                arColumnName.Add("PM_VC1_PAck")
                arColumnName.Add("PM_VC20_UUser")
                arColumnName.Add("PM_NU8_UDate")
                arColumnName.Add("PM_NU6_UTime")
                arColumnName.Add("PM_VC20_UMID")

                arRowData.Add(txtProcessEnv_F.Text.Trim)
                arRowData.Add(txtProcessCode_F.Text.Trim)
                arRowData.Add(txtProcessName_F.Text.Trim)
                arRowData.Add(txtProcessType_F.Text.Trim)
                arRowData.Add(txtDescription_F.Text.Trim)
                If chkAck_F.Checked = True Then
                    arRowData.Add("Y")
                Else
                    arRowData.Add("N")
                End If
                arRowData.Add(HttpContext.Current.Session("PropUserName"))
                arRowData.Add(PropSetDate)
                arRowData.Add(PropSetTime)
                arRowData.Add(Network.GetIPAddress("", "", Network.GetMachineName("", "")))

                If SQL.Save("T130031", "Process", "SaveProcessInfo", arColumnName, arRowData) = True Then
                    mstrqID = txtProcessCode_F.Text.Trim
                    'lstError.Items.Clear()
                    'cpnlErrorPanel.Visible = True
                    'cpnlErrorPanel.Text = "Message"
                    lstError.Items.Add("Record saved successfully...")
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgOK)
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
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy ple4ase try later...")
            'cpnlErrorPanel.Visible = True
            'cpnlErrorPanel.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("Process", "SaveprocessInfo-879", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try

    End Function

    Private Function ValidateProcessInfo() As Boolean
        shFlag = 0
        lstError.Items.Clear()
        If txtProcessCode_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Code cannot be empty...")
        End If
        If txtProcessEnv_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Environment cannot be empty...")
        End If
        If txtProcessName_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Name cannot be empty...")
        End If
        If txtProcessType_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Type cannot be empty...")
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

        Dim arrUDCList As New ArrayList
        arrUDCList.Add("PRTY")
        arrUDCList.Add("PACK")
        CheckUDC(arrUDCList)

        Try
            Dim intRows As Integer
            If mstrqID = "-1" Then
                If SQL.Search("Process", "ValidateProcessInfo", "select  * from t130031 where PM_IN4_PCode=" & txtProcessCode_F.Text.Trim & " and PM_VC20_PENV='" & txtProcessEnv_F.Text.Trim & "'", intRows) = True Then
                    lstError.Items.Add("This Process Already Exists in the given Environment...")
                    shFlag = 1
                End If
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
            'cpnlErrorPanel.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("Process", "ValidateProcessInfo-943", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function

    Public Function CheckUDC(ByVal arrUDC As ArrayList) As Boolean

        Dim intCount As Integer
        Dim intRows As Integer
        For intCount = 0 To arrUDC.Count - 1
            Select Case arrUDC.Item(intCount)
                Case "PRTY"
                    If SQL.Search("Process", "CheckUDC", "select  Name from UDC where UDCType='PRTY' and Name='" & txtProcessType_F.Text.Trim & "'", intRows) = False Then
                        lstError.Items.Add("Process Type Mismatch...")
                        shFlag = 1
                    End If
            End Select
        Next

        If shFlag = 1 Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function CheckInfo() As Boolean
        If txtDescription_F.Text.Equals("") And txtProcessCode_F.Text.Equals("") And txtProcessCode_F.Text.Equals("") And txtProcessEnv_F.Text.Equals("") And txtProcessName_F.Text.Equals("") And txtProcessType_F.Text.Equals("") Then
            Return False
        Else
            Return True
        End If
    End Function
End Class
