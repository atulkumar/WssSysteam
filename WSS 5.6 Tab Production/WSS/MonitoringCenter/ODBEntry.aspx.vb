Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_ODBEntry
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

    Public txthiddenImage As String

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        txtObjectDescription.Attributes.Add("OnKeyPress", " return MaxLength('" & txtObjectDescription.ClientID & "','200');")


        If txtLimit_F.Text <> "" Then
            Session("Limit") = txtLimit_F.Text
        End If
        If txtPercent_F.Text <> "" Then
            Session("Percent") = txtPercent_F.Text
        End If
        If txtAlertType_F.Text <> "" Then
            Session("AlertTypeV") = txtAlertType_F.Text
        End If
        If txtAlertType_FName.Text <> "" Then
            Session("AlertTypeN") = txtAlertType_FName.Text
        End If
        If txtMail_F.Text <> "" Then
            Session("Mail") = txtMail_F.Text
        End If
        txtCSS(Me.Page, "cpnlObjectDeatail")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(Session.Timeout) * 60) & ";Login.aspx"" />")

        If Request.QueryString("FE") = "1" Then
            Session("Limit") = ""
            Session("Percent") = ""
            Session("AlertTypeV") = ""
            Session("AlertTypeN") = ""
            Session("Mail") = ""
            lstError.Items.Clear()
            cpnlError.Visible = True
            cpnlError.Text = "Message"
            lstError.Items.Add("Record saved successfully")
            ImgError.ImageUrl = "../images/Pok.gif"
            MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
        ElseIf Request.QueryString("FE") = "2" Then
            txtLimit_F.Text = Session("Limit")
            txtPercent_F.Text = Session("Percent")
            txtAlertType_F.Text = Session("AlertTypeV")
            txtAlertType_FName.Text = Session("AlertTypeN")
            txtMail_F.Text = Session("Mail")
            If CheckObjectDetail() = True Then
                ValidateObjectDetailInfo()
                cpnlError.Visible = True
                cpnlError.Text = "Error Message"
                ImgError.ImageUrl = "../images/warning.gif"
                MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            End If
        Else
            cpnlError.Visible = False
        End If
        If IsPostBack = False Then
            mstrqID = Request.QueryString("ID")
        End If
        If mstrqID = "-1" Then
            Dim intObjectID As Integer = SQL.Search("", "", "select max(OM_NU9_ID_PK) from T130131")
            intObjectID += 1
            txtObjectID.Text = intObjectID
            dtObjectDetail.CalendarDate = Today
        End If
        Dim intNext As Integer
        intNext = SQL.Search("", "", "select max(OD_NU9_OID_PK) from T130142")
        intNext = intNext + 1
        txtOIDPK_F.Text = intNext
        Dim srtQueryWhere As String

        txthiddenImage = Request.Form("txthiddenImage")
        'If Request.QueryString("FE") = "1" Then
        '    txthiddenImage = "Save"
        'End If
        Dim txthiddenValue = Request.Form("txthidden")
        If Request.Form("txthidden") = "" Then
        Else
            Session("SAddressNumber") = Request.Form("txthidden")
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        Response.Redirect("SearchODBObject.aspx?")
                    Case "Edit"
                        'Response.Redirect("MachineEntry.aspx?ID='" & txthiddenValue & "'")
                    Case "Ok"
                        If SaveObject() = True Then
                            If CheckObjectDetail() = True Then
                                If SaveObjectDetail() = True Then
                                    Response.Redirect("../SearchODBObject.aspx")
                                End If
                            End If
                        End If
                    Case "Save"
                        SaveObject()
                        If CheckObjectDetail() = True Then
                            If SaveObjectDetail() = True Then
                                ClearObjectDetailTextBoxes()
                            End If
                        End If
                    Case "Add"
                        'Response.Redirect("MachineEntry.aspx?ID=-1")
                    Case "Delete"
                        If SQL.Delete("ODBEntry", "Load", "delete from T130142 where OD_NU9_OID_PK=" & txthiddenValue, SQL.Transaction.Serializable) = True Then
                            lstError.Items.Clear()
                            'cpnlError.Visible = True
                            'cpnlError.Text = "Message"
                            lstError.Items.Add("Record Deleted successfully...")
                            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            'ImgError.ImageUrl = "../images/Pok.gif"
                            'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)

                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server is busy please try later...")
                            'cpnlError.Visible = True
                            'cpnlError.Text = "Error Message"
                            'ImgError.ImageUrl = "../images/error_image.gif"
                            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("OBDEntry", "Load-236", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If
        If mstrqID = "-1" Then
            cpnlObjectDeatail.CssClass = "test2"
            cpnlObjectDeatail.State = CustomControls.Web.PanelState.Collapsed
            cpnlObjectDeatail.Enabled = False
        End If
        If SQL.Search("", "", "select * from T130142 where OD_NU9_OID=" & mstrqID) = False Then
            Panel2.Visible = True
            Panel1.Visible = False
            GrdAddSerach.Visible = False
        Else
            Panel1.Visible = True
            Panel2.Visible = False
            GrdAddSerach.Visible = True
        End If
        txtObjectID.ReadOnly = False

        If IsPostBack = False Then
            If mstrqID <> "-1" Then

                txtObjectID.ReadOnly = True
                Dim sqrdr As SqlClient.SqlDataReader
                Dim blnStatus As Boolean
                Try
                    sqrdr = SQL.Search("ODBEntry", "Load", "select * from T130131 where OM_NU9_ID_PK=" & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                    If blnStatus = True Then
                        While sqrdr.Read
                            txtObjectID.Text = IIf(Not IsDBNull(sqrdr("OM_NU9_ID_PK")), sqrdr("OM_NU9_ID_PK"), "")
                            txtObjectType.Text = IIf(Not IsDBNull(sqrdr("OM_VC4_OType")), sqrdr("OM_VC4_OType"), "")
                            txtObjectTypeName.Text = SQL.Search("", "", "select Description from UDC where Name='" & txtObjectType.Text & "'")
                            txtObjectName.Text = IIf(Not IsDBNull(sqrdr("OM_VC250_OName")), sqrdr("OM_VC250_OName"), "")
                            txtObjectDescription.Text = IIf(Not IsDBNull(sqrdr("OM_VC200_ODesc")), sqrdr("OM_VC200_ODesc"), "")
                            txtObjectPath.Text = IIf(Not IsDBNull(sqrdr("OM_VC250_OPath")), sqrdr("OM_VC250_OPath"), "")
                            dtObjectDetail.CalendarDate = IIf(Not IsDBNull(sqrdr("OM_DT8_OCreated")), sqrdr("OM_DT8_OCreated"), "")
                            txtObjectMachineCode.Text = IIf(IsDBNull(sqrdr("OM_IN4_OMCode")) = False, sqrdr("OM_IN4_OMCode"), "")
                            txtObjectMachineCodeName.Text = SQL.Search("", "", "select MM_VC20_MName from T130011 where MM_IN4_MCode=" & txtObjectMachineCode.Text)
                            txtObjectProcessCode.Text = IIf(IsDBNull(sqrdr("OM_NU9_OPID")) = False, sqrdr("OM_NU9_OPID"), "")
                            txtObjectProcessCodeName.Text = SQL.Search("", "", "select PM_VC20_PName from T130031 where PM_IN4_PCode=" & txtObjectProcessCode.Text)
                        End While
                        sqrdr.Close()
                    End If
                Catch ex As Exception
                    CreateLog("OBDEntry", "Load-280", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try
            End If
        End If

        If Not IsPostBack Then
            'chk grid width in database
            chkgridwidth()

            'fill dropdown list data from database
            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T130142"
                SQL.DBTracing = False

                '**************
                ' Dim arSetColumnName As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnView As Boolean
                Dim strSelect As String = "select "

                'SQL.DBTable = "T030212"
                Dim sqlquery As String

                sqrdView = SQL.Search("ODBEntry", "Load-314", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130142' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
                'sqrdView = SQL.Search("", "", srtQueryWhere, SQL.CommandBehaviour.CloseConnection, blnView)

                If blnView = True Then
                    While sqrdView.Read
                        If sqrdView.Item("UV_VC50_COL_Value") = "OD_DT8_ODate" Then
                            strSelect &= "convert(varchar(12)," & sqrdView.Item("UV_VC50_COL_Value") & " ,103) as OD_DT8_ODate ,"
                        Else
                            strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                        End If
                        arOriginalColumnName.Add(sqrdView.Item("UV_VC50_COL_Value"))
                        arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    End While
                    sqrdView.Close()

                    strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                    strSelect &= " from T130142"
                    srtQueryWhere &= " OD_NU9_OID=" & mstrqID
                    If srtQueryWhere <> "" Then
                        strSelect = strSelect & " where " + srtQueryWhere
                    End If
                    ' SQL.DBTable = "T130142"
                    If SQL.Search("T130142", "ODBEntry", "Load", strSelect, dsDefault, "", "") = True Then

                        'change the datagrid header columns name 
                        For inti As Integer = 0 To dsDefault.Tables("T130142").Columns.Count - 1
                            dsDefault.Tables("T130142").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        Next
                    End If
                    mdvtable.Table = dsDefault.Tables("T130142")
                End If
                '**************
                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
                GetColumns()
                'create textbox at run time at head of the datagrid        
                CreateTextBox()
            Catch ex As Exception
                CreateLog("OBDEntry", "Load-345", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlObjectDeatail:" & arCol.Item(i)))
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
                    CreateLog("OBDEntry", "Load-367", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try
            End If

        End If
        If dtObjectDetail_F.CalendarDate = "" Then
            dtObjectDetail_F.CalendarDate = Today
        End If

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
        'SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String

            sqrdView = SQL.Search("ODBEntry", "FillView", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130142' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
                strSelect &= " from T130142"

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

                strSelect &= " where OD_NU9_OID=" & mstrqID
                ' SQL.DBTable = "T130142"

                If SQL.Search("T130142", "ODBEntry", "FillView", strSelect, dsFromView, "", "") = True Then
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
                    GetColumns()
                    CreateTextBox()
                Else
                End If
            Else
                Exit Function
            End If
        Catch ex As Exception
            CreateLog("OBDEntry", "FillView-477", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            '   SQL.DBTable = "T030212"

            'query on database
            sqrdView = SQL.Search("ODBEntry", "FillGrid_data", "select UV_VC50_COL_Value,UV_VC50_COL_Name from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130142'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                'fill the value and name into the diffrent arrays
                While sqrdView.Read
                    strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    arSetColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                End While
                sqrdView.Close()

                'build the query
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T130142"

                ' SQL.DBTable = "T130142"

                'changing the dataset columns header title from database 
                If SQL.Search("T130142", "ODBEntry", "FillGrid_data", strSelect, dsDefault, "", "") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T130142").Columns.Count - 1
                        dsDefault.Tables("T130142").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If

                arSetColumnName.Clear()
                GrdAddSerach.Columns.Clear()
                GrdAddSerach.AutoGenerateColumns = True
                mdvtable.Table = dsDefault.Tables("T130142")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

                'call formating function for format datagrid according to database size
                GetColumns()
            End If
        Catch ex As Exception
            CreateLog("OBDEntry", "FillGrid_Data-540", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

            sqrdView = SQL.Search("ODBEntry", "GetView", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where  UV_VC50_TBL_Name='T130142'", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("OBDEntry", "GetView-567", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            CreateLog("OBDEntry", "CreateTextBox-637", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            CreateLog("OBDEntry", "CreateViewTextBox-697", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            CreateLog("OBDEntry", "FormatGrid-746", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
            'SQL.DBTable = "T030212"
            SQL.DBTracing = False

            sqrdView = SQL.Search("ODBEntry", "GetColums", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130142' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("OBDEntry", "GetColumns-780", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
        ' SQL.DBTable = "T030212"
        SQL.DBTracing = False

        sqrdView = SQL.Search("ODBEntry", "chkgridwidth", "select UV_VC50_COL_Value, UV_VC50_COL_Name, UV_VC10_Col_width from T030212 where UV_IN4_View_ID=0 and UV_VC50_tbl_Name='T130142' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
        SaveObject()

        If CheckObjectDetail() = True Then

            If SaveObjectDetail() = True Then
                ClearObjectDetailTextBoxes()
                Response.Redirect("ODBEntry.aspx?ID=" & mstrqID & "&FE=1")
            Else
                Response.Redirect("ODBEntry.aspx?ID=" & mstrqID & "&FE=2")
            End If
        Else
            Response.Redirect("ODBEntry.aspx?ID=" & mstrqID & "&FE=2")
        End If
        FillView()
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
            CreateLog("OBDEntry", "Click-894", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & strTempName & "', '" & e.Item.Cells(6).Text.Trim & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "', '" & strTempName & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("OBDEntry", "ItemDataBound-924", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

    Public Function SaveObjectDetail() As Boolean
        Try
            If ValidateObjectDetailInfo() = True Then
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T130142"
                SQL.DBTracing = False

                arColumnName.Add("OD_NU9_OID_PK")
                arColumnName.Add("OD_NU9_OID")
                arColumnName.Add("OD_DT8_ODate")
                arColumnName.Add("OD_FL8_OLimit")
                arColumnName.Add("OD_FL8_OPct")
                arColumnName.Add("OD_VC8_OAlertType")
                arColumnName.Add("OD_VC75_OMail")
                arColumnName.Add("OD_CH2_Status")

                arRowData.Add(txtOIDPK_F.Text.Trim)
                arRowData.Add(txtObjectID.Text.Trim)
                arRowData.Add(dtObjectDetail_F.CalendarDate.Trim)
                arRowData.Add(txtLimit_F.Text.Trim)
                arRowData.Add(txtPercent_F.Text.Trim)
                arRowData.Add(txtAlertType_F.Text.Trim)
                arRowData.Add(txtMail_F.Text.Trim)
                arRowData.Add("C")
                If SQL.Save("T130142", "", "", arColumnName, arRowData) = True Then

                    ' Update the status in T130131 table if date is equal or more then the current date and status is not E
                    Dim strOID As String = SQL.Search("ODBEntry", "SaveObjectDetail-983", "select t130131.om_nu9_id_pk from t130131,t130142 where od_dt8_odate >= convert(varchar, getdate(), 101)and om_nu9_id_pk=od_nu9_oid and om_nu9_id_pk=" & txtObjectID.Text.Trim & " and om_ch2_status='e'")

                    If IsNothing(strOID) = False Then
                        If SQL.Update("T130142", "ODBEntry", "SaveObjectDetail-986", "update T130131 set OM_CH2_Status='C' where OM_NU9_ID_PK=" & txtObjectID.Text.Trim & "", SQL.Transaction.Serializable) = True Then

                        Else

                        End If
                    End If

                    mstrqID = txtObjectID.Text.Trim
                    lstError.Items.Clear()
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Message"
                    lstError.Items.Add("Object Detail Record saved successfully...")
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Error Message"
                    'ImgError.ImageUrl = "../images/error_image.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
        Catch ex As Exception
            CreateLog("ODBEntry", "SaveObjectDetail-988", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Public Function SaveObject() As Boolean
        Try
            If ValidateObjectInfo() = True Then
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T130131"
                SQL.DBTracing = False

                If mstrqID = "-1" Then
                    arColumnName.Add("OM_NU9_ID_PK")
                    arColumnName.Add("OM_CH2_Status")
                End If
                arColumnName.Add("OM_VC4_OType")
                arColumnName.Add("OM_VC250_OName")
                arColumnName.Add("OM_VC200_ODesc")
                arColumnName.Add("OM_VC250_OPath")
                arColumnName.Add("OM_DT8_OCreated")
                arColumnName.Add("OM_IN4_OMCode")
                arColumnName.Add("OM_NU9_OPID")


                If mstrqID = "-1" Then
                    arRowData.Add(txtObjectID.Text.Trim)
                    arRowData.Add("C")
                End If
                arRowData.Add(txtObjectType.Text.Trim)
                arRowData.Add(txtObjectName.Text.Trim)
                arRowData.Add(txtObjectDescription.Text.Trim)
                arRowData.Add(txtObjectPath.Text.Trim)
                arRowData.Add(dtObjectDetail.CalendarDate.Trim)
                arRowData.Add(txtObjectMachineCode.Text.Trim)
                arRowData.Add(txtObjectProcessCode.Text.Trim)
                If mstrqID = "-1" Then
                    If SQL.Save("T130131", "ODBEntry", "SaveObject", arColumnName, arRowData) = True Then
                        SaveDBInfo()
                        cpnlObjectDeatail.CssClass = "test"
                        cpnlObjectDeatail.State = CustomControls.Web.PanelState.Expanded
                        cpnlObjectDeatail.Enabled = True
                        mstrqID = txtObjectID.Text.Trim
                        GrdAddSerach.Visible = False
                        Panel2.Visible = True

                        'lstError.Items.Clear()
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Message"
                        lstError.Items.Add("Record saved successfully...")
                        'ImgError.ImageUrl = "../images/Pok.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Server is busy please try later...")
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Error Message"
                        'ImgError.ImageUrl = "../images/error_image.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        Return False
                    End If
                Else

                    If SQL.Update("T130131", "ODBEntry", "SaveObject", "select * from T130131 where OM_NU9_ID_PK=" & mstrqID, arColumnName, arRowData) = True Then
                        mstrqID = txtObjectID.Text.Trim
                        lstError.Items.Clear()
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Message"
                        lstError.Items.Add("Record updated successfully...")
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        'ImgError.ImageUrl = "../images/Pok.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Server is busy please try later...")
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Error Message"
                        'ImgError.ImageUrl = "../images/error_image.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        Return False
                    End If

                End If
            End If

        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            CreateLog("OBDEntry", "SaveObject-1082", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try

    End Function
    Public Function CheckObjectDetail() As Boolean
        If txtAlertType_F.Text = "" And txtLimit_F.Text = "" And txtMail_F.Text = "" And txtPercent_F.Text = "" Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Function ValidateObjectDetailInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtAlertType_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Detail: Alert Type cannot be empty...")
        End If
        If txtLimit_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Detail: Object Limit cannot be empty...")
        End If
        If txtMail_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Detail: Object Mail cannot be empty...")
        End If
        If txtPercent_F.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Detail: Object Percentage cannot be empty...")
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        End If
        lstError.Items.Clear()
        Try
            Dim intRows As Integer

            If SQL.Search("ODBEntry", "ValidateObjectDetailInfo", "select  OD_NU9_OID_PK from T130142 where OD_NU9_OID_PK=" & txtOIDPK_F.Text, intRows) = True Then
                lstError.Items.Add("Object Detail: Invalid Object ID Key")
                shFlag = 1
            End If
            If shFlag = 1 Then
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                'ImgError.ImageUrl = "../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If
            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            CreateLog("OBDEntry", "ValidateObjectDeatilInfo-1145", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try


    End Function

    Public Function ValidateObjectInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If dtObjectDetail.CalendarDate.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Date cannot be empty...")
        End If

        If txtObjectID.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object ID cannot be empty...")
        End If

        If txtObjectMachineCode.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Machine Code cannot be empty...")
        End If

        If txtObjectName.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Name cannot be empty...")
        End If

        'If txtObjectPath.Text.Equals("") Then
        '    shFlag = 1
        '    lstError.Items.Add("Object Path cannot be empty")
        'End If

        If txtObjectProcessCode.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Process Code cannot be empty...")
        End If

        If txtObjectType.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Object Type cannot be empty...")
        End If

        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        End If

        lstError.Items.Clear()
        Try
            If shFlag = 1 Then
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                'ImgError.ImageUrl = "../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                Return False
            End If

            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            CreateLog("OBDEntry", "ValidateObjectDetailInfo-1218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
        Return True
    End Function
    Public Function ClearObjectDetailTextBoxes() As Boolean
        txtAlertType_F.Text = ""
        txtLimit_F.Text = ""
        txtMail_F.Text = ""
        txtPercent_F.Text = ""
    End Function

    'Private Sub SaveFastEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveFastEntry.Click
    '    txthiddenImage = "Save"
    'End Sub

    Private Function SaveDBInfo() As Boolean
        Try
            Dim strPEnv As String
            strPEnv = SQL.Search("ODBEntry", "SaveDBInfo", "select PM_VC20_PENV from T130031 where PM_VC20_PName='OBJINFO'")
            Dim sqRdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            sqRdr = SQL.Search("ODBEntry", "SaveDBInfo", "select * from T130172 where EV_VC30_Environment_Name='" & strPEnv & "'", SQL.CommandBehaviour.SingleRow, blnStatus)
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T130153"
            SQL.DBTracing = False

            arColumnName.Add("UI_NU9_ID_FK")
            arColumnName.Add("UI_VC100_SystemID")
            arColumnName.Add("UI_VC50_UserID")
            arColumnName.Add("UI_VC50_Password")
            arRowData.Add(txtObjectID.Text.Trim)
            While sqRdr.Read
                arRowData.Add(sqRdr("EV_VC100_SystemID"))
                arRowData.Add(sqRdr("EV_VC50_UserID"))
                arRowData.Add(sqRdr("EV_VC50_Password"))
            End While
            sqRdr.Close()
            If SQL.Save("T130153", "ODBEntry", "SaveDBInfo", arColumnName, arRowData) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("OBDEntry", "SaveDBInfo-1263", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
End Class
