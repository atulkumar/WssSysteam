'**********************************************************************************************************
' Page                 : - Template
' Purpose              : - Purpose of this screen is to show all the available templates
' Tables used          : - T010023, T050011, T050031, T050041, T050051, T050061, T030212, T010011


' Date					Author						Modification Date					Description
' 10/03/06				Sachin						-------------------					Created
'
' Notes: 
' Code:
'**********************************************************************************************************

Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
''''''''''Session Variables used in this Page are''''''''''
'ViewState("SAddressNumber_Template")
Partial Class AdministrationCenter_Template_Template
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

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            'imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            'imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        End If
        Dim txthiddenImage = Request.Form("txthiddenImage")
        If Request.Form("txthidden") = "" Then
        Else
            ViewState("SAddressNumber_Template") = Request.Form("txthidden")
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "Edit"
                        If ViewState("SAddressNumber_Template") <> "" And ViewState("SAddressNumber_Template") <> "-1" Then
                            'Response.Redirect("TemplateDetail.aspx?AddressNo=" + ViewState("SAddressNumber_Template") + "", False)
                        End If

                    Case "Add"
                        ViewState("SAddressNumber_Template") = "-1"
                        'Response.Redirect("TemplateDetail.aspx", False)

                    Case "Delete"
                        If ViewState("SAddressNumber_Template") <> "" And ViewState("SAddressNumber_Template") <> "-1" Then
                            Dim intTemplateID As Integer = ViewState("SAddressNumber_Template")
                            WSSDelete.DeleteTemplate(intTemplateID)
                            lstError.Items.Clear()
                            lstError.Items.Add("Record Deleted successfully...")
                            'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        End If
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select

            Catch ex As Exception
                CreateLog("Template", "Load-160", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If
        txtCSS(Me.Page)
        If Not IsPostBack Then

            'chk grid width in database
            ChkGridWidth()

            'fill dropdown list data from database
            ' GetView()

            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '                SQL.DBTable = "T050011"
                SQL.DBTracing = False

                '**************
                ' Dim arSetColumnName As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "

                '                SQL.DBTable = "T030212"
                sqrdView = SQL.Search("Template", "Load-186", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id ", SQL.CommandBehaviour.CloseConnection, blnView)
                If blnView = True Then

                    arSetColumnName.Clear()
                    arOriginalColumnName.Clear()
                    While sqrdView.Read

                        If sqrdView.Item("UV_VC50_COL_Value") = "TL_NU9_CustID_FK" Then
                            strSelect &= "comp." & "CI_VC36_Name" & ","
                        Else
                            strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        End If
                        ' strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        arOriginalColumnName.Add(sqrdView.Item("UV_VC50_COL_Value"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    End While
                    sqrdView.Close()
                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)

                    strSelect &= " from T050011,T010011 comp,T210011 Project  WHERE T050011.TL_NU9_CustID_FK=comp.CI_NU8_Address_Number  and  TL_NU9_ProjectID_FK=Project.PR_NU9_Project_ID_Pk and TL_NU9_CustID_FK=Project.PR_NU9_Comp_ID_FK  AND TL_NU9_CustID_FK IN (" & GetCompanySubQuery() & ") order by TL_NU9_ID_PK desc"

                    If SQL.Search("T050011", "Template", "Load-221", strSelect, dsDefault, "sachin", "Prashar") = True Then
                        For inti As Integer = 0 To dsDefault.Tables("T050011").Columns.Count - 1
                            dsDefault.Tables("T050011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        Next
                        mdvtable.Table = dsDefault.Tables("T050011")
                        GrdAddSerach.DataSource = mdvtable.Table
                        GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                        If GrdAddSerach.AutoGenerateColumns = False Then
                            GrdAddSerach.AutoGenerateColumns = True
                        End If
                        GrdAddSerach.DataBind()
                        GetColumns()
                    Else
                        Panel1.Visible = False
                        GrdAddSerach.Visible = False
                        lstError.Items.Clear()
                        'lstError.Items.Add("Template not opened so far...")
                        'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                        DisplayMessage("Template not opened so far...")

                    End If
                End If
                '*****************************************
                'create textbox at run time at head of the datagrid        
                CreateTextBox()
            Catch ex As Exception
                CreateLog("Template", "Load-248", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage

                        Case "Search"
                            Call BtnGrdSearch_Click(Me, New EventArgs)

                    End Select
                Catch ex As Exception
                    CreateLog("Template", "Load-218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
                End Try
            End If

        End If

        'Security Block

        Dim intId As Integer

        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            'intId = Request.QueryString("ScrID")
            intId = 43
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If

        'End of Security Block



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
        '        SQL.DBTable = "T030212"
        SQL.DBTracing = False
        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            sqrdView = SQL.Search("Template", "FillView-330", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id ", SQL.CommandBehaviour.CloseConnection, blnView)
            'If SQL.Search("", "", strConnection, "select name,view_id from tbl_userview_name", dsView, "tbl_userview_detail") = True Then
            If blnView = True Then
                Dim dsFromView As New DataSet
                arColumnName.Clear()
                arCol.Clear()
                'arColWidth2.Clear()
                arColWidth.Clear()
                While sqrdView.Read
                    If sqrdView.Item("UV_VC50_COL_Value") = "TL_NU9_CustID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If
                    arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_Width"))
                End While
                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)

                strSelect &= " from T050011,T010011 comp,T210011 Project  WHERE T050011.TL_NU9_CustID_FK=comp.CI_NU8_Address_Number  and  TL_NU9_ProjectID_FK=Project.PR_NU9_Project_ID_Pk and TL_NU9_CustID_FK=Project.PR_NU9_Comp_ID_FK  AND TL_NU9_CustID_FK IN (" & GetCompanySubQuery() & ") order by TL_NU9_ID_PK desc"

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
                '                SQL.DBTable = "T050011"
                If SQL.Search("T050011", "Template", "FillView-385", strSelect, dsFromView, "sachin", "Prashar") = True Then
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
                    Panel1.Visible = False
                    GrdAddSerach.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("Template not opened so far...")
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'DisplayMessage("Template not opened so far...")

                End If
            Else
                Exit Function
            End If

        Catch ex As Exception
            CreateLog("Template", "FillView-293", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
            '            SQL.DBTable = "T030212"

            'query on database
            sqrdView = SQL.Search("Template", "FillGrid_Data-449", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'fill the value and name into the diffrent arrays
                While sqrdView.Read
                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
                sqrdView.Close()

                'build the query
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T050011"

                '                SQL.DBTable = "T050011"

                'changing the dataset columns header title from database 
                If SQL.Search("T050011", "Template", "FillGrid_Data-466", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T050011").Columns.Count - 1
                        dsDefault.Tables("T050011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If

                arSetColumnName.Clear()
                GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T050011")
                GrdAddSerach.DataSource = mdvtable
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                GrdAddSerach.DataBind()

                'call formating function for format datagrid according to database size
                GetColumns()

            End If
        Catch ex As Exception
            CreateLog("Template", "FillGrid_Data-479", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
            '            SQL.DBTable = "T030201"
            SQL.DBTracing = False

            sqrdView = SQL.Search("Template", "GetView-507", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T050011'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then

            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("Template", "GetView-511", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Sub CreateTextBox()

        Dim _textbox As TextBox
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
            CreateLog("Template", "CreateTextBox-580", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub

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
            CreateLog("Template", "CreateViewTextBox-640", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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
            bcl.HeaderStyle.Width = New System.Web.UI.WebControls.Unit(150)
            GrdAddSerach.Columns.Add(bcl)
        Next

        GrdAddSerach.DataSource = mdvtable.Table
        If GrdAddSerach.AutoGenerateColumns = False Then
            GrdAddSerach.AutoGenerateColumns = True
        End If
        GrdAddSerach.DataBind()
    End Sub

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid(ByVal ColumnName As ArrayList)
        Dim intI As Integer

        Try
            GrdAddSerach.AutoGenerateColumns = False
            ChkGridWidth()

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
            CreateLog("Template", "FormatGrid-690", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
            '            SQL.DBTable = "T030212"
            SQL.DBTracing = False

            sqrdView = SQL.Search("Template", "GetColumns-714", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("Template", "GetColumns-724", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub ChkGridWidth()

        Dim sqrdView As SqlDataReader
        Dim blnView As Boolean
        'Dim intViewID As Integer = ddlstview.SelectedValue
        Dim arCol As New ArrayList

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        '		SQL.DBTable = "T030212"
        SQL.DBTracing = False

        sqrdView = SQL.Search("Template", "ChkGridWidth-747", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text
                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
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
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mTextBox(intI).Text.Trim
                        strSearch = GetSearchString(strSearch)
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
            If mdvtable.Count = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Data not found according to your search string...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If

        Catch ex As Exception
            CreateLog("Template", "Click-824", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        GrdAddSerach.Columns.Clear()
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
            CreateLog("Template", "ItemDataBound-851", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub

#End Region

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgError)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Add(Msg)
        'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub
#End Region

    'Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
    '    Try
    '        Response.Redirect("../../Home.aspx", False)
    '    Catch ex As Exception
    '        CreateLog("Template", "imgClose_Click", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
    '    End Try
    'End Sub


End Class
