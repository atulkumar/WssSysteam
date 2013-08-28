'***********************************************************************************************************
' Page                 : - TempInvoices
' Purpose              : - 
' Tables used          : - T080031, T010011
' Date					   Author						Modification Date				Description
' 22/05/06			   Sachin Prashar		            ------------------				Created
'
' Notes: 
' Code:
'***********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
'''''' session used on thi page other than Common session Variables'''
'ViewState("SInvID")
'ViewState("SCompany")

Partial Class AdministrationCenter_Agreement_TempInvoices
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
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        cpnlErrorPanel.Visible = False
        Dim arColName As New ArrayList
        Dim arRowData As New ArrayList
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        End If
        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenValue = Request.Form("txthidden")

        If Request.Form("txthidden") = "" Then
        Else
            ViewState("SInvID") = Request.Form("txthidden")
            ViewState("SCompany") = Request.Form("txthiddenCompany")
        End If

        If txthiddenImage <> "" Then

            Select Case txthiddenImage
                Case "Edit"
                    ViewState("SInvID") = Request.Form("txthidden")
                    ViewState("SCompany") = Request.Form("txthiddenCompany")
                    Response.Redirect("InvoiceDetails.aspx?ID=-1&SInvID=" + ViewState("SInvID") + "&SCompany=" + ViewState("SCompany") + "")
                    Exit Sub
                Case "Add"
                    ViewState("SInvID") = ""
                    ViewState("SCompany") = ""
                    Response.Redirect("InvoiceDetails.aspx?ID=1")
                    Exit Sub
                Case "Logout"
                    LogoutWSS()
            End Select

        End If

        txtCSS(Me.Page)
        If Not IsPostBack Then

            chkgridwidth()

            Try

                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T080011"
                SQL.DBTracing = False

                Dim strSelect As String = "select IM_NU9_Invoice_ID_PK InvNo,IM_NU9_Company_ID_PK CusID,comp.CI_VC36_Name Customer,convert(varchar,IM_DT8_Invoice_From_Date,101) FromDate,convert(varchar,IM_DT8_Invoice_To_Date,101) ToDate,IM_NU9_Invoice_Amount  Amount,convert(varchar,IM_DT8_Invoice_Due_Date,101) DueDate,IM_VC8_Invoice_Status Status,convert(varchar,IM_DT8_Invoice_Status_Date,101) LastUpdate,crt.CI_VC36_Name CreatedBy,convert(varchar,IM_DT8_Invoice_Created_Date,101) CreatedDate from T080031,T010011 comp,T010011 crt where IM_CH1_Invoice_Temp='Y' and comp.CI_NU8_Address_Number=IM_NU9_Company_ID_PK and crt.CI_NU8_Address_Number=IM_NU9_Invoice_Created_By AND IM_NU9_Company_ID_PK IN(" & GetCompanySubQuery() & ")"

                If SQL.Search("T080011", "page", "load", strSelect, dsDefault, "sachin", "Prashar") = True Then

                    mdvtable.Table = dsDefault.Tables("T080011")

                    Dim htDateCols As New Hashtable
                    htDateCols.Add("FromDate", 2)
                    htDateCols.Add("ToDate", 2)
                    htDateCols.Add("DueDate", 2)
                    htDateCols.Add("LastUpdate", 2)
                    htDateCols.Add("CreatedDate", 2)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)


                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    GrdAddSerach.DataBind()
                    FormatGrid()
                    GetColumns()

                    'create textbox at run time at head of the datagrid        

                    CreateTextBox()

                Else

                    lstError.Items.Clear()

                    'cpnlErrorPanel.Text = "Message"
                    'cpnlErrorPanel.Visible = True
                    'cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    lstError.Items.Add(" Invoice not opened so far...  ")
                    '  Image1.ImageUrl = "../../Images/Pok.gif"
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If

            Catch ex As Exception
                CreateLog("TempInvoices", "Load-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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

                    Call BtnGrdSearch_Click(Me, New EventArgs)

                Catch ex As Exception
                    CreateLog("TempInvoices", "Load-262", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try

            End If

        End If

        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 582
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        'End of Security Block

    End Sub


#Region "fill View"

    Private Function FillView()

        Try

            Dim dsDefault As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T080011"
            SQL.DBTracing = False

            Dim strSelect As String = "select IM_NU9_Invoice_ID_PK InvNo,IM_NU9_Company_ID_PK CusID,comp.CI_VC36_Name Customer,convert(varchar,IM_DT8_Invoice_From_Date,101) FromDate,convert(varchar,IM_DT8_Invoice_To_Date,101) ToDate,IM_NU9_Invoice_Amount  Amount,convert(varchar,IM_DT8_Invoice_Due_Date,101) DueDate,IM_VC8_Invoice_Status Status,convert(varchar,IM_DT8_Invoice_Status_Date,101) LastUpdate,crt.CI_VC36_Name CreatedBy,convert(varchar,IM_DT8_Invoice_Created_Date,101) CreatedDate from T080031,T010011 comp,T010011 crt where IM_CH1_Invoice_Temp='Y' and comp.CI_NU8_Address_Number=IM_NU9_Company_ID_PK and crt.CI_NU8_Address_Number=IM_NU9_Invoice_Created_By AND IM_NU9_Company_ID_PK IN (" & GetCompanySubQuery() & ")"

            If SQL.Search("T080011", "TempInvoices", "FillView", strSelect, dsDefault, "sachin", "Prashar") = True Then

                mdvtable.Table = dsDefault.Tables("T080011")

                Dim htDateCols As New Hashtable
                htDateCols.Add("FromDate", 2)
                htDateCols.Add("ToDate", 2)
                htDateCols.Add("DueDate", 2)
                htDateCols.Add("LastUpdate", 2)
                htDateCols.Add("CreatedDate", 2)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)

                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
                FormatGrid()
                GetColumns()

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            End If

        Catch ex As Exception
            CreateLog("TempInvoices", "Load-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

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
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(arCol.Item(intii))
                    If arCol.Item(intii) = "CusID" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=""0"" Visible=""False"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If
                    'Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arCol.Item(intii)
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

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)
                    If arCol.Item(intii) = "CusID" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server""  Width=""0"" Visible=""False"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If
                    'Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("TempInvoices", "CreateTextBox-313", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()


        arColWidth.Add(35)
        arColWidth.Add(2)
        arColWidth.Add(60)
        arColWidth.Add(69)
        arColWidth.Add(69)
        arColWidth.Add(60)
        arColWidth.Add(70)
        arColWidth.Add(60)
        arColWidth.Add(69)
        arColWidth.Add(69)
        arColWidth.Add(69)



        Try
            GrdAddSerach.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1

                If intI = 1 Then
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
            CreateLog("TempInvoices", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()


        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(2)
        arColWidth.Add(60)
        arColWidth.Add(69)
        arColWidth.Add(69)
        arColWidth.Add(60)
        arColWidth.Add(70)
        arColWidth.Add(60)
        arColWidth.Add(69)
        arColWidth.Add(69)
        arColWidth.Add(69)



        arColumnName.Add("InvNo")
        arColumnName.Add("CusID")
        arColumnName.Add("Customer")
        arColumnName.Add("FromDate")
        arColumnName.Add("ToDate")
        arColumnName.Add("Amount")
        arColumnName.Add("DueDate")
        arColumnName.Add("Status")
        arColumnName.Add("LastUpdate")
        arColumnName.Add("CreatedBy")
        arColumnName.Add("CreatedDate")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(2)
        arColWidth.Add(60)
        arColWidth.Add(69)
        arColWidth.Add(69)
        arColWidth.Add(60)
        arColWidth.Add(70)
        arColWidth.Add(60)
        arColWidth.Add(69)
        arColWidth.Add(69)
        arColWidth.Add(69)


        arCol.Add("InvNo")
        arCol.Add("CusID")
        arCol.Add("Customer")
        arCol.Add("FromDate")
        arCol.Add("ToDate")
        arCol.Add("Amount")
        arCol.Add("DueDate")
        arCol.Add("Status")
        arCol.Add("LastUpdate")
        arCol.Add("CreatedBy")
        arCol.Add("CreatedDate")

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
                        strSearch = mTextBox(intI).Text.Trim
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

        GrdAddSerach.Columns.Clear()
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



End Class
