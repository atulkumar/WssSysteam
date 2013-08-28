Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_DomainInfo
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
    Private Shared intCol As Integer

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCSS(Me.Page)
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        cpnlErrorPanel.Visible = False
        Dim arColName As New ArrayList
        Dim arRowData As New ArrayList


        'Security Block

        'Dim intID As Int32
        'If Not IsPostBack Then
        '    Dim str As String
        '    str = HttpContext.Current.Session("PropRootDir")
        '    intID = 555
        '    Dim obj As New clsSecurityCache
        '    If obj.ScreenAccess(intID) = False Then
        '        Response.Redirect("../../frm_NoAccess.aspx")
        '    End If
        '    obj.ControlSecurity(Me.Page, intID)
        'End If

        'End of Security Block


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
                        Response.Redirect("CreateDomain.aspx?ID='" & txthiddenValue & "'")
                    Case "Add"
                        Response.Redirect("CreateDomain.aspx?ID=-1")
                    Case "Delete"
                        If SQL.Delete("DomainInfo", "Load", "Delete from T170011 where DM_IN4_DID = " & txthiddenValue, SQL.Transaction.Serializable) = True Then
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

                CreateLog("DomainInfo", "Load-174", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If


        If Not IsPostBack Then

            chkgridwidth()

            Try
                Dim dsDefault As New DataSet
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '                SQL.DBTable = "T170011"
                SQL.DBTracing = False

                Dim strSelect As String = "select DM_IN4_DID as ID, CI_VC36_Name as CompanyName, DM_VC150_DomainName as DomainName, DM_CH1_IsEnable as IsEnable  from T010011, T170011   where DM_IN4_Company_ID_FK = CI_NU8_Address_Number and CI_VC8_Status = 'ENA'"

                If SQL.Search("T170011", "DomainInfo", "Load", strSelect, dsDefault, "", "") = True Then

                    mdvtable.Table = dsDefault.Tables("T170011")

                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    GrdAddSerach.DataBind()
                    FormatGrid()
                    GetColumns()

                    'create textbox at run time at head of the datagrid        

                    CreateTextBox()
                End If

            Catch ex As Exception
                CreateLog("DomainInfo", "Load-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

                    Call BtnGrdSearch_Click(Me, New EventArgs)

                Catch ex As Exception
                    CreateLog("DomainInfo", "Load-262", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try

            End If

        End If

    End Sub


#Region "fill View"

    Private Function FillView()

        Try

            Dim dsDefault As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            '            SQL.DBTable = "T170011"
            SQL.DBTracing = False

            Dim strSelect As String = "select DM_IN4_DID as ID, CI_VC36_Name as CompanyName, DM_VC150_DomainName as DomainName, DM_CH1_IsEnable as IsEnable  from T010011, T170011   where DM_IN4_Company_ID_FK = CI_NU8_Address_Number and CI_VC8_Status = 'ENA'"

            If SQL.Search("T170011", "DomainInfo", "FillView", strSelect, dsDefault, "", "") = True Then

                mdvtable.Table = dsDefault.Tables("T170011")

                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
                FormatGrid()
                GetColumns()

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            End If

        Catch ex As Exception
            CreateLog("DomainInfo", "Load-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
                    arCol.Add(arCol.Item(intii))
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arCol.Item(intii)
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
            CreateLog("DomainInfo", "CreateTextBox-536", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(0)
        arColWidth.Add(150)
        arColWidth.Add(270)
        arColWidth.Add(50)

        Try
            GrdAddSerach.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1

                If intI = 0 Then
                    Dim Bound_Column As New BoundColumn
                    Bound_Column.Visible = False
                    'Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    'Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    'Bound_Column.ItemStyle.Wrap = True
                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    GrdAddSerach.Columns.Add(Bound_Column)
                Else
                    Dim Bound_Column As New BoundColumn
                    Dim strWidth As String = arColWidth.Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True
                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    GrdAddSerach.Columns.Add(Bound_Column)
                End If
            Next

        Catch ex As Exception
            CreateLog("DomainInfo", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Add(0)
        arColWidth.Add(150)
        arColWidth.Add(270)
        arColWidth.Add(50)

        arColumnName.Add("ID")
        arColumnName.Add("CompanyName")
        arColumnName.Add("DomainName")
        arColumnName.Add("IsEnable")


    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Add(0)
        arColWidth.Add(150)
        arColWidth.Add(270)
        arColWidth.Add(50)

        arCol.Add("ID")
        arCol.Add("CompanyName")
        arCol.Add("DomainName")
        arCol.Add("IsEnable")

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

                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"

                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) Then
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
                FormatGrid()
                GetColumns()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.AutoGenerateColumns = True
            GrdAddSerach.DataBind()
            FormatGrid()
            GetColumns()
        Catch ex As Exception
            CreateLog("DomainInfo", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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

        GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    StrComp = e.Item.Cells(1).Text

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "','" & StrComp & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "', '" & StrComp & "')")

                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("DomainInfo", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region


End Class
