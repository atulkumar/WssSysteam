Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data



Partial Class MonitoringCenter_PackageDeployment
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

        txtCSS(Me.Page)
        arOriginalColumnName.Clear()
        arOriginalColumnName.Add("PH_VC10_PKGNAME")
        arOriginalColumnName.Add("PH_VC30_PKGDSC")
        arOriginalColumnName.Add("PH_VC10_PATHCD")
        arOriginalColumnName.Add("PH_VC2_BLDSTS")
        arOriginalColumnName.Add("PH_VC4_PKGDEPSTS")
        arOriginalColumnName.Add("PH_VC20_BSPCKNAME")
        arOriginalColumnName.Add("PH_NU9_UPMJ")
        arOriginalColumnName.Add("PH_FL8_UPMT")
        arSetColumnName.Clear()
        arSetColumnName.Add("PKGName")
        arSetColumnName.Add("Description")
        arSetColumnName.Add("PATHCD")
        arSetColumnName.Add("BLDSTS")
        arSetColumnName.Add("PKGDEPSTS")
        arSetColumnName.Add("BSPCKNAME")
        arSetColumnName.Add("UPMJ")
        arSetColumnName.Add("UPMT")
        arColumnName.Clear()
        arColumnName.Add("PKGName")
        arColumnName.Add("Description")
        arColumnName.Add("PATHCD")
        arColumnName.Add("BLDSTS")
        arColumnName.Add("PKGDEPSTS")
        arColumnName.Add("BSPCKNAME")
        arColumnName.Add("UPMJ")
        arColumnName.Add("UPMT")
        arColWidth.Clear()
        arColWidth.Add(100)
        arColWidth.Add(200)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)



        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        cpnlErrorPanel.Visible = False

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
                CreateLog("PackageDeployment", "Load-154", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If

        If Not IsPostBack Then

            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '    SQL.DBTable = "T160031"
                SQL.DBTracing = False

                Dim strSelect As String = "select "

                Dim strItem As String
                For inti As Integer = 0 To arOriginalColumnName.Count - 1
                    strSelect &= arOriginalColumnName.Item(inti) & ","
                Next
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T160031"
                If srtQueryWhere <> "" Then
                    strSelect = strSelect & " where " + srtQueryWhere
                End If
                '  SQL.DBTable = "T160031"
                If SQL.Search("T160031", "", "", strSelect, dsDefault, "", "") = True Then

                    'change the datagrid header columns name 
                    For inti As Integer = 0 To dsDefault.Tables("T160031").Columns.Count - 1
                        dsDefault.Tables("T160031").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If
                mdvtable.Table = dsDefault.Tables("T160031")
                'End If
                '**************

                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
                FormatGrid(arColumnName)

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            Catch ex As Exception
                CreateLog("PackageDeployment", "Load-219", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlViewJobs:" & arCol.Item(i)))
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
                    CreateLog("PackageDeployment", "Load-242", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String


            Dim dsFromView As New DataSet



            Dim strItem As String
            For inti As Integer = 0 To arOriginalColumnName.Count - 1
                strSelect &= arOriginalColumnName.Item(inti) & ","
            Next

            strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
            'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
            strSelect &= " from T160031"

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


            'SQL.DBTable = "T160031"

            If SQL.Search("T160031", "PackageDeployment", "FillView", strSelect, dsFromView, "", "") = True Then
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
                FormatGrid(arColumnName)

                CreateTextBox()
            Else
            End If
            'Else
            '    Exit Function
            'End If
        Catch ex As Exception
            CreateLog("PackageDeployment", "Fillview-351", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

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
            CreateLog("PackageDeployment", "CreateTextBox-516", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

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
            CreateLog("PackageDeployment", "FormatGrid-623", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click
        arOriginalColumnName.Clear()
        arOriginalColumnName.Add("PH_VC10_PKGNAME")
        arOriginalColumnName.Add("PH_VC30_PKGDSC")
        arOriginalColumnName.Add("PH_VC10_PATHCD")
        arOriginalColumnName.Add("PH_VC2_BLDSTS")
        arOriginalColumnName.Add("PH_VC4_PKGDEPSTS")
        arOriginalColumnName.Add("PH_VC20_BSPCKNAME")
        arOriginalColumnName.Add("PH_NU9_UPMJ")
        arOriginalColumnName.Add("PH_FL8_UPMT")
        arSetColumnName.Clear()
        arSetColumnName.Add("PKGName")
        arSetColumnName.Add("Description")
        arSetColumnName.Add("PATHCD")
        arSetColumnName.Add("BLDSTS")
        arSetColumnName.Add("PKGDEPSTS")
        arSetColumnName.Add("BSPCKNAME")
        arSetColumnName.Add("UPMJ")
        arSetColumnName.Add("UPMT")
        arColumnName.Clear()
        arColumnName.Add("PKGName")
        arColumnName.Add("Description")
        arColumnName.Add("PATHCD")
        arColumnName.Add("BLDSTS")
        arColumnName.Add("PKGDEPSTS")
        arColumnName.Add("BSPCKNAME")
        arColumnName.Add("UPMJ")
        arColumnName.Add("UPMT")
        arColWidth.Clear()
        arColWidth.Add(100)
        arColWidth.Add(200)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)

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
                FormatGrid(arColumnName)
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.AutoGenerateColumns = True
            GrdAddSerach.DataBind()

            FormatGrid(arColumnName)
        Catch ex As Exception
            CreateLog("PackageDeployment", "Click-757", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "','" & strTempName & "')")
                    'e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "', '" & strTempName & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("PackageDeployment", "ItemDataBound-787", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try
    End Sub

#End Region
End Class
