Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class AdministrationCenter_Inventory_Inventory_View
    Inherits System.Web.UI.Page
#Region "global level declaration"

    Dim mtxtUDCTypeQuery As TextBox()
    Private Shared arColWidth As New ArrayList
    Dim flage As Integer
    Dim mdvtable As New DataView
    Dim marTextbox() As TextBox
    Dim arrTextboxId As New ArrayList
    Dim mintColumns As Integer
    Dim mshFlag As Short
    Dim Expanded2 As New PlaceHolder
    Dim ii As WebControls.Unit
    Dim rowvalue As Integer
    Public mintPageSize As Integer
    Dim arrColumnsName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared intCol As Integer

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        cpnlError.Visible = False

        Dim arColName As New ArrayList
        Dim arRowData As New ArrayList
        If Not IsPostBack Then
            txtCSS(Me.Page)
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        End If
        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenValue = Request.Form("txthidden")

        If Request.Form("txthidden") = "" Then
        End If

        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Edit"
                Case "Add"
                    Response.Redirect("Inventory_Detail.aspx")
                    Exit Sub
                Case "Logout"
                    LogoutWSS()
            End Select
        End If


        SetColumnsNameAndWidth()

        If Not IsPostBack Then
            FillView()
            CreateTextBox()
        Else
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
            For i As Integer = 0 To arColWidth.Count - 1
                arrtextvalue.Add(Request.Form("cpnlCallView$" & arrColumnsName.Item(i)))
            Next

            FillView()
            CreateTextBox()
            If txthiddenImage <> "" Then
                Try
                    Call BtnGrdSearch_Click(Me, New EventArgs)
                Catch ex As Exception
                    CreateLog("TempInvoices", "Load-262", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try
            End If
        End If


    End Sub

#Region "fill View"

    Private Function FillView()
        Try
            Dim dsDefault As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strSelect As String = "Select ID, ItemName,ItemCode,Description,PartNo,SerialNo,ItemGroup,Type,Unit,Location,DateOfPurchase from ItemMaster"
            If SQL.Search("ItemMaster", "TempInvoices", "FillView", strSelect, dsDefault, "sachin", "Prashar") = True Then
                mdvtable.Table = dsDefault.Tables("ItemMaster")
                Dim htDateCols As New Hashtable
                htDateCols.Add("DateOfPurchase", 2)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
            End If
        Catch ex As Exception
            CreateLog("TempInvoices", "Load-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

        arrTextboxId.Clear()

        'fill the columns count into the array from mdvtable view
        intCol = mdvtable.Table.Columns.Count

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
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arrColumnsName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arrColumnsName.Item(intii)
                    _textbox.Text = ""
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
                    strcolid = arrColumnsName.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arrColumnsName.Item(intii)
                End If
                arrTextboxId.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("TempInvoices", "CreateTextBox-313", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

#End Region

#Region "check the grid width from database"

    Private Sub SetColumnsNameAndWidth()

        arColWidth.Clear()
        arrColumnsName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(70)
        arColWidth.Add(100)
        arColWidth.Add(150)
        arColWidth.Add(70)
        arColWidth.Add(60)
        arColWidth.Add(70)
        arColWidth.Add(60)
        arColWidth.Add(70)
        arColWidth.Add(70)
        arColWidth.Add(110)


        arrColumnsName.Add("ID")
        arrColumnsName.Add("ItemName")
        arrColumnsName.Add("ItemCode")
        arrColumnsName.Add("Description")
        arrColumnsName.Add("PartNo")
        arrColumnsName.Add("SerialNo")
        arrColumnsName.Add("ItemGroup")
        arrColumnsName.Add("Type")
        arrColumnsName.Add("Unit")
        arrColumnsName.Add("Location")
        arrColumnsName.Add("DateOfPurchase")

    End Sub

#End Region

#Region "Serach Grid Button Click"
    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click
        Dim strRowFilterString As String
        Dim strSearch As String
        rowvalue = 0
        Try
            For intI As Integer = 0 To arColWidth.Count - 1
                If Not arrtextvalue(intI).Equals("") Then
                    strSearch = arrtextvalue(intI)
                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            If IsDate(strSearch) Then
                            Else
                                Exit Sub
                            End If
                        End If
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
                        End If
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = arrtextvalue(intI)
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
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                Exit Sub
            End If
            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString
            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.DataBind()
        Catch ex As Exception
            CreateLog("TempInvoices", "BtnGrdSearch_Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim StrComp As String

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = e.Item.Cells(0).Text
                    StrComp = e.Item.Cells(1).Text

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "','" & StrComp & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "', '" & StrComp & "')")

                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("TempInvoices", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region


    Private Sub ImgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgClose.Click
        Response.Redirect("../../Home.aspx", False)
    End Sub

    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated

        Try

            Dim intA As Integer = 0

            For intI = 0 To arColWidth.Count - 1

                e.Item.Cells(intA).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                intA += 1

            Next

        Catch ex As Exception
        End Try

    End Sub
End Class
