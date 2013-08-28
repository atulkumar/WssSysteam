'***********************************************************************************************************
' Page                   : - Template Search
' Purpose                : - It contains all the templates.User can search any template from ita s searching                              facility is there for searching templates. 
' Tables used            : - , T050011, T030212, T010011, T030201

' Date					Author						Modification Date					Description
' 11/03/06				Sachin						-------------------					Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class AdministrationCenter_Template_TemplateSearch
    Inherits System.Web.UI.Page
    Private Shared srtQueryWhere As String
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
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        txtCSS(Me.Page)
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")

            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        End If
        'get value from call detail form for serach
        '***********************************
        Dim calltype As String
        calltype = Request.QueryString("CALLTYPE")
        calltype = calltype.Replace("1_a_2_b_3_c", "&")
        Dim custname As String
        custname = Request.QueryString("CUST")
        Dim temptype As String
        temptype = Request.QueryString("TYPE")
        '*******************************************

        Dim chkand As Short
        chkand = 0
        srtQueryWhere = ""
        If calltype <> "" Then
            srtQueryWhere = srtQueryWhere & "  TL_VC8_Call_type ='" & calltype & "'"
            chkand = 1
        End If
        If custname <> "" Then
            If chkand = 1 Then
                srtQueryWhere = srtQueryWhere & " and TL_NU9_CustID_FK = '" & custname & "'"
            Else
                srtQueryWhere = srtQueryWhere & "TL_NU9_CustID_FK = '" & custname & "'"
            End If
            chkand = 1
        End If
        If temptype <> "" Then
            If chkand = 1 Then
                srtQueryWhere = srtQueryWhere & " and TL_VC8_Tmpl_Type = '" & temptype & "' "
            Else
                srtQueryWhere = srtQueryWhere & " TL_VC8_Tmpl_Type = '" & temptype & "' "
            End If
        End If

        Dim txthiddenImage = Request.Form("txthiddenImage")
        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SAddressNumber") = Request.Form("txthidden")
        End If
        If txthiddenImage <> "" Then
            Try

                Select Case txthiddenImage
                    Case "Edit"
                        If HttpContext.Current.Session("SAddressNumber") <> "" And HttpContext.Current.Session("SAddressNumber") <> "-1" Then
                            Response.Redirect("TemplateDetail.aspx", False)
                        End If
                    Case "Add"
                        HttpContext.Current.Session("SAddressNumber") = "-1"
                        Response.Redirect("TemplateDetail.aspx", False)
                    Case "Logout"
                        LogoutWSS()
                End Select

            Catch ex As Exception
                CreateLog("TemplateSearch", "Load-182", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        If Not IsPostBack Then
            'chk grid width in database
            ChkGridWidth()

            Try
                Dim dsDefault As New DataSet
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "
                sqrdView = SQL.Search("TemplateSearch", "Load-210", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

                If blnView = True Then
                    While sqrdView.Read
                        If sqrdView.Item("UV_VC50_COL_Value") = "TL_NU9_CustID_FK" Then
                            strSelect &= "comp." & "CI_VC36_Name" & ","
                        Else
                            strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        End If
                        arOriginalColumnName.Add(sqrdView.Item("UV_VC50_COL_Value"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    End While

                    sqrdView.Close()
                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                    strSelect &= " from T050011,T010011 comp,T210011 Project "
                    If srtQueryWhere <> "" Then
                        strSelect = strSelect & " where " + srtQueryWhere & " and  T050011.TL_NU9_CustID_FK=comp.CI_NU8_Address_Number and  TL_NU9_ProjectID_FK=Project.PR_NU9_Project_ID_Pk and TL_NU9_CustID_FK=Project.PR_NU9_Comp_ID_FK  "
                    End If
                    If Not Request.QueryString("ProjectID").Equals("") Then
                        strSelect = strSelect & " and TL_NU9_ProjectID_FK=" & Request.QueryString("ProjectID") & " order by TL_NU9_ID_PK desc"
                    Else
                        strSelect = strSelect & " and TL_VC8_Tmpl_Type<>'TAO' order by TL_NU9_ID_PK desc"
                    End If

                    If SQL.Search("T050011", "TemplateSearch", "Load-231", strSelect, dsDefault, "sachin", "Prashar") = True Then
                        'change the datagrid header columns name 
                        For inti As Integer = 0 To dsDefault.Tables("T050011").Columns.Count - 1
                            dsDefault.Tables("T050011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        Next
                        mdvtable.Table = dsDefault.Tables("T050011")
                        GrdAddSerach.DataSource = mdvtable.Table
                        GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                        GrdAddSerach.DataBind()
                        GetColumns()
                    Else
                        GrdAddSerach.Visible = False
                        lstError.Items.Clear()
                        lstError.Items.Add("No Template available...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    End If
                End If
                CreateTextBox()
            Catch ex As Exception
                CreateLog("TemplateSearch", "Load-272", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        Else
            ' fill the textboxes value into the array 
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form(arCol.Item(i)))
            Next
            'fill data in datagrid on load on post back event
            FillView()
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Search"
                            Call BtnGrdSearch_Click(Me, New EventArgs)
                    End Select
                Catch ex As Exception
                    CreateLog("TemplateSearch", "Load-295", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                End Try
            End If

        End If
        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 420
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
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
        SQL.DBTracing = False


        Try

            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String

            sqrdView = SQL.Search("TemplateSearch", "FillView-328", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id ", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                Dim dsFromView As New DataSet
                arColumnName.Clear()
                arCol.Clear()
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
                strSelect &= " from T050011,T010011 comp,T210011 Project  "

                If srtQueryWhere <> "" Then
                    strSelect = strSelect & " where " + srtQueryWhere & " and  T050011.TL_NU9_CustID_FK=comp.CI_NU8_Address_Number and  TL_NU9_ProjectID_FK=Project.PR_NU9_Project_ID_Pk and TL_NU9_CustID_FK=Project.PR_NU9_Comp_ID_FK  "
                End If
                If Not Request.QueryString("ProjectID").Equals("") Then
                    strSelect = strSelect & " and TL_NU9_ProjectID_FK=" & Request.QueryString("ProjectID") & " order by TL_NU9_ID_PK desc"
                Else
                    strSelect = strSelect & " and TL_VC8_Tmpl_Type<>'TAO' order by TL_NU9_ID_PK desc"
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
                If SQL.Search("T050011", "TemplateSearch", "FillView-383", strSelect, dsFromView, "sachin", "Prashar") = True Then
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
                    Panel1.Visible = False
                    GrdAddSerach.Visible = False
                    lstError.Items.Clear()
                    DisplayMessage("No Template available...")
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("TemplateSearch", "FillView-443", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Fill data into datagrid and change the header name according the database"

    Private Sub FillGrid_data()

        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim strConnection As String
        Dim dsDefault As DataSet
        Dim arSetColumnName As ArrayList
        strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        Try
            dsDefault = New DataSet
            arSetColumnName = New ArrayList
            'query on database
            sqrdView = SQL.Search("TemplateSearch", "FillGrid_Data-449", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011'", SQL.CommandBehaviour.CloseConnection, blnView)

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
                'changing the dataset columns header title from database 
                If SQL.Search("T050011", "TemplateSearch", "FillGrid_data-466", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T050011").Columns.Count - 1
                        dsDefault.Tables("T050011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If
                arSetColumnName.Clear()
                GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T050011")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                'call formating function for format datagrid according to database size
                GetColumns()
            End If
        Catch ex As Exception
            CreateLog("TemplateSearch", "FillGrid_Data-497", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
            SQL.DBTracing = False
            sqrdView = SQL.Search("TemplateSearch", "GetView-504", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T050011'", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("TemplateSearch", "GetView-529", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
                    col1cng = col1.Value - 2
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
                    col1cng = col1.Value - 2
                    col1cng = col1cng & "pt"

                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If
                    strcolid = arCol.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("TemplateSearch", "CreateTextBox-598", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

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
                GrdAddSerach.Columns.Add(Bound_Column)
            Next
            ColumnName.Clear()

        Catch ex As Exception
            CreateLog("TemplateSearch", "FormatGrid-705", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()
        'Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strConnection As String =  System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            sqrdView = SQL.Search("TemplateSearch", "GetColumns-708", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("TemplateSearch", "GetColumns-739", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub ChkGridWidth()

        Dim sqrdView As SqlDataReader
        Dim blnView As Boolean
        Dim arCol As New ArrayList
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        sqrdView = SQL.Search("TemplateSearch", "ChkGridWidth-741", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T050011' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
            CreateLog("TemplateSearch", "Click-839", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & strTempName & "','" & e.Item.Cells(2).Text.Trim & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "', '" & strTempName & "','" & e.Item.Cells(2).Text.Trim & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("TemplateSearch", "ItemDataBound-869", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Add(Msg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub
#End Region

End Class
