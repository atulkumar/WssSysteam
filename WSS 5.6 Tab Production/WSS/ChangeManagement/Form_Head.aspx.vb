'**********************************************************************************************************
' Page                   : - Form_Head
' Purpose                : - Purpose of this form is to list down the forms already made. User can search for                             any form from this screen.
' Tables used            : - T030212, T110011, T010011, T030201, T110044, T110033, T110022, T100011
' Date		    		Author						Modification Date					Description
' 21/03/06				Jaswinder				    ----------------	        		Created
'
''*********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class ChangeManagement_Form_Head
    Inherits System.Web.UI.Page
    '''''''''Session Variables Used on this Page are:::
    'Session("SFormID") 
    'Session("SUser")
    'Session("SRole") 
    'Session("SCompany") 
    'Session("PropUserName")
    'Session("PropUserID")
    'Session("PropCompanyID")
    'Session("PropRootDir")
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
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        'Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'cpnlErrorPanel.Visible = False
        imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
        'imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")



        Dim txthiddenImage = Request.Form("txthiddenImage")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        If Request.Form("txthidden") = "" Then
        Else
            ViewState("SFormID") = Request.Form("txthidden")
            ViewState("SUser") = Request.Form("txthiddenUser")
            ViewState("SRole") = Request.Form("txthiddenRole")
            ViewState("SCompany") = Request.Form("txthiddenCompany")

        End If


        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Edit"
                        'Response.Redirect("form_entry_head.aspx?ScrID=259&ID=0&SFormID" + ViewState("SFormID"), False)
                    Case "Add"
                        ViewState("SFormID") = ""
                        'Response.Redirect("form_entry_head.aspx?ScrID=259&ID=-1", False)

                    Case "Delete"
                        If deleteForm(Request.Form("txthidden")) Then
                            ViewState("SFormID") = ""
                            Response.Redirect("Form_Head.aspx?ScrID=14", False)
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Form has been used, cannot be Deleted...")
                            'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                            ShowMsgPenelNew(pnlMsg, lstError, MSG.msgWarning)
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select

            Catch ex As Exception
                CreateLog("Form_Head", "Load-132", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If


        If Not IsPostBack Then
            txtCSS(Me.Page)
            'chk grid width in database
            chkgridwidth()

            'fill dropdown list data from database
            ' GetView()

            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                'SQL.DBTable = "T110011"
                SQL.DBTracing = False

                '**************
                ' Dim arSetColumnName As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "

                'SQL.DBTable = "T030212"
                sqrdView = SQL.Search("Form_head", "load", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T110011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
                If blnView = True Then
                    While sqrdView.Read

                        If sqrdView.Item("UV_VC50_COL_Value") = "FN_VC100_Inserted_On" Then
                            strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' LastUpdate ,"
                        Else
                            strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        End If

                        arOriginalColumnName.Add(sqrdView.Item("UV_VC50_COL_Value"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    End While
                    sqrdView.Close()

                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                    strSelect &= " from T110011,T010011 where FN_IN4_Company_ID=" & HttpContext.Current.Session("PropCompanyID") & " and CI_NU8_Address_Number=FN_VC100_Inserted_By order by FN_VC100_Form_name"
                    'SQL.DBTable = "T110011"
                    If SQL.Search("T110011", "Form_Head", "load-191", strSelect, dsDefault, "", "") = True Then

                        'change the datagrid header columns name 
                        For inti As Integer = 0 To dsDefault.Tables("T110011").Columns.Count - 1
                            dsDefault.Tables("T110011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        Next
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("No Form created yet, click on plus button to create new form...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        GrdAddSerach.Visible = False
                        Panel1.Visible = False
                    End If
                    mdvtable.Table = dsDefault.Tables("T110011")
                End If
                '**************

                Dim htDateCols As New Hashtable
                htDateCols.Add("LastUpdate", 1)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)


                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()

                If mdvtable.Table.Rows.Count > 0 Then
                    GrdAddSerach.Visible = True
                Else
                    GrdAddSerach.Visible = False
                End If

                GetColumns()

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            Catch ex As Exception
                CreateLog("CM_Form_Head", "Load-198", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
                    CreateLog("CM_Form_Head", "Load-221", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                End Try
            End If

        End If


        'Security Block
        Dim intid As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
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
        Dim arcolName As New ArrayList

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        ' SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "

            sqrdView = SQL.Search("CM_Form_Head", "FillView", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T110011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                Dim dsFromView As New DataSet
                arColumnName.Clear()
                arCol.Clear()
                arColWidth.Clear()

                While sqrdView.Read
                    If sqrdView.Item("UV_VC50_COL_Value") = "FN_VC100_Inserted_On" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + ''  LastUpdate,"
                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If
                    arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_Width"))
                End While

                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)

                strSelect &= " from T110011,T010011 where FN_IN4_Company_ID=" & HttpContext.Current.Session("PropCompanyID") & " and CI_NU8_Address_Number=FN_VC100_Inserted_By order by FN_VC100_Form_name"

                If SQL.Search("T110011", "CM_Form_Head", "FillView", strSelect, dsFromView, "", "") = True Then

                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                    Next

                    mdvtable.Table = dsFromView.Tables(0)

                    Dim htDateCols As New Hashtable
                    htDateCols.Add("LastUpdate", 1)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)

                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.Columns.Clear()
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.DataBind()

                    If mdvtable.Table.Rows.Count > 0 Then
                        GrdAddSerach.Visible = True
                        GetColumns()
                        CreateTextBox()
                    End If
                Else
                    GrdAddSerach.Visible = False
                    lstError.Items.Add("Pending Invoice not Available...")
                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)

                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("CM_Form_Head", "FillView-355", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
            sqrdView = SQL.Search("CM_Form_Head", "FillGrid_Data", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T110011'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'fill the value and name into the diffrent arrays
                While sqrdView.Read
                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
                sqrdView.Close()

                'build the query
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T110011"

                ' SQL.DBTable = "T110011"

                'changing the dataset columns header title from database 
                If SQL.Search("", "CM_Form_Head", "FillGrid_data", strSelect, dsDefault, "", "") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T110011").Columns.Count - 1
                        dsDefault.Tables("T110011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If

                arSetColumnName.Clear()
                GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T110011")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

                'call formating function for format datagrid according to database size
                GetColumns()

            End If
        Catch ex As Exception
            CreateLog("CM_Form_Head", "FillGrid_Data-419", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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

            sqrdView = SQL.Search("CM_Form_Head", "GetView", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T110011'", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("CM_Form_Head", "GetView-451", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
                    col1cng = col1.Value
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(arSetColumnName.Item(intii))
                    If arColWidth.Item(intii) = 0 Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " Visible=""false"" CssClass=SearchTxtBox MaxLength=""45""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""45""></asp:TextBox>"))
                    End If
                    _textbox.ID = arSetColumnName.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else
                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value
                    col1cng = col1cng & "pt"

                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)
                    If col1cng = "0pt" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " Visible=""False"" CssClass=SearchTxtBox MaxLength=""45""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""45""></asp:TextBox>"))
                    End If
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                    _textbox.MaxLength = 100
                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("CM_Form_Head", "CreateTextBox-520", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "create textbox for User View"

    Private Sub CreateViewTextBox()

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
            CreateLog("CM_Form_Head", "CreateVieewTextBox-580", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

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

                If intI = 0 Then
                    Bound_Column.Visible = False
                End If

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                GrdAddSerach.Columns.Add(Bound_Column)
            Next
            ColumnName.Clear()
        Catch ex As Exception
            CreateLog("CM_Form_Head", "FormatGrid-630", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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

            sqrdView = SQL.Search("CM_Form_Head", "GetColums", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T110011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("CM_Form_Head", "GetColumns-663", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        Dim sqrdView As SqlDataReader
        Dim blnView As Boolean
        Dim arCol As New ArrayList

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        sqrdView = SQL.Search("CM_Form_Head", "chkgridwidth", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T110011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
                lstError.Items.Add("Data not found according to your search string... ")
                'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)

            End If

        Catch ex As Exception
            CreateLog("CM_Form_Head", "Click-763", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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
            CreateLog("CM_Form_Head", "ItemDataBound-791", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAddSearch")
        End Try
    End Sub

#End Region

#Region "Deletion"

    Function deleteForm(ByVal formNo As Integer) As Boolean

        Dim retflag, flg As Boolean
        Dim formName As String = getName(formNo)

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBConnection = strConnection
        '  SQL.DBTable = "T060022"
        SQL.DBTracing = False


        If checkFormDetails(formName) Then

        Else
            'Update the Task Table To Set the Form Bit to 0
            'Based on various condtions
            Dim strUpdate As String
            strUpdate = "update T040021 set TM_CH1_Forms='0' where Ltrim(convert(varchar(16),TM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Comp_ID_FK)) in (select Ltrim(convert(varchar(16),TM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Comp_ID_FK)) from T040021,T040011 where TM_NU9_Call_No_FK=CM_NU9_Call_No_PK and CM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and CM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and TM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and TM_CH1_Forms<>'0' and CM_VC8_Call_type + TM_VC8_Task_Type in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name='" & formName & "' and  FT_VC8_Call_type + FT_VC8_Task_Type not in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name<>'" & formName & "' and FT_IN4_Comp_ID=" & Session("PropCompanyID") & ") and FT_IN4_Comp_ID=" & Session("PropCompanyID") & "))"
            If SQL.Update("Form_Entry_Head", "DeleteForm", strUpdate, SQL.Transaction.Serializable) = True Then

            End If
            'Update the Template Task Table To Set the Form Bit to 0
            'Based on various condtions
            strUpdate = "update T050031 set TTM_CH1_Forms='0' where Ltrim(convert(varchar(16),TTM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Comp_ID_FK)) in (select Ltrim(convert(varchar(16),TTM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Comp_ID_FK)) from T050031,T050021 where TTM_NU9_Call_No_FK=TCM_NU9_Call_No_PK and TCM_NU9_CompID_FK=TTM_NU9_Comp_ID_FK and TCM_NU9_CompID_FK=" & Session("PropCompanyID") & " and TTM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and TTM_CH1_Forms<>'0' and TCM_VC8_Call_type + TTM_VC8_Task_Type in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name='" & formName & "' and  FT_VC8_Call_type + FT_VC8_Task_Type not in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name<>'" & formName & "' and FT_IN4_Comp_ID=" & Session("PropCompanyID") & ") and FT_IN4_Comp_ID=" & Session("PropCompanyID") & "))"
            If SQL.Update("Form_Entry_Head", "DeleteForm", strUpdate, SQL.Transaction.Serializable) = True Then

            End If

            'delete field info
            flg = SQL.Delete("CM_Form_Head", "deleteForm", "delete from T110044 where CA_IN4_Form_No=" & formNo, SQL.Transaction.ReadCommitted)

            'delete tabs
            flg = SQL.Delete("CM_Form_Head", "deleteForm", "delete from T110033 where FB_IN4_form_no=" & formNo, SQL.Transaction.ReadCommitted)

            'delete form
            If SQL.Delete("CM_Form_Head", "deleteForm", "delete from T110011 where FN_IN4_form_no=" & formNo, SQL.Transaction.ReadCommitted) Then
                retflag = True
            Else
                retflag = False
            End If

            ' give deletion message id delete called

            If retflag Then
                flg = SQL.Delete("CM_Form_Head", "deleteForm", "delete from T110022 where FT_VC100_form_name='" & formName & "' and FT_IN4_Comp_id=" & Session("PropCompanyID"), SQL.Transaction.ReadCommitted)
            End If

        End If


        Return retflag

    End Function

    Function getName(ByVal formNo) As String
        Try
            Dim strFormID As String = SQL.Search("CM_Form_Head", "getName", "Select FN_VC100_Form_name from T110011 where FN_IN4_form_no=" & formNo)

            Return strFormID

        Catch ex As Exception
            CreateLog("CM_Form_Head", "getName-791", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAddSearch")
        End Try
    End Function

    Function checkFormDetails(ByVal formName As String) As Boolean
        Try
            Dim strFormID As Integer = SQL.Search("CM_Form_Head", "checkFormDetails", "Select FD_IN4_Form_no from T100011 where FD_VC50_Call_form_Name='" & formName & "' and FD_IN4_Comp_id=" & Session("PropCompanyID"))

            If strFormID > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("CM_Form_Head", "checkFormDetails-791", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAddSearch")
        End Try
    End Function

#End Region

  
End Class
