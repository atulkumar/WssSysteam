'*******************************************************************
' Page                   :-Security
' Purpose                :- ,                     
' Tables Used            :- 
' Date		    			Author						Modification Date					Description
' 29/11/07				    Sachin 					            					                    Created
'
' Notes: 
' Code:
'*******************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class ImportExport_DocumentView
    Inherits System.Web.UI.Page


#Region "global level declaration"

    ' Holding text boxes created above UDC Type Grid
    Dim mtxtUDCTypeQuery As TextBox()

    Private arColWidth As New ArrayList
    Private arColumnName As New ArrayList
    Private mdvtable As New DataView
    Private Shared mTextBox() As TextBox

    Dim ii As WebControls.Unit
    Dim rowvalue As Integer
    Public mintPageSize As Integer

    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared intCol As Integer

#End Region

#Region "Page Load Code"
    Dim FilePath As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCSS(Me.Page)
        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")

        cpnlErrorPanel.Visible = False

        imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")


        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenAdno = Request.Form("txthiddenAdno")
        FilePath = Request.Form("txtFilePath")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        If Request.Form("txthiddenAdno") = "" Then
        Else
            HttpContext.Current.Session("FileNo") = Request.Form("txthiddenAdno")
        End If

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        ' Response.Redirect("Role_Search.aspx?ScrID=64")
                    Case "Logout"
                        LogoutWSS()
                    Case "Edit"
                        Response.Redirect("Document_Details.aspx", False)
                    Case "Add"
                        HttpContext.Current.Session("FileNo") = Nothing
                        Response.Redirect("Document_Details.aspx", False)
                    Case "Delete"
                        If DeleteRecord() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("File Deleted Successfully ...")
                            'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        End If
                End Select

            Catch ex As Exception
                Dim str As String = ex.ToString
            End Try
        End If

        GetColumns()

        If Not IsPostBack Then
            'chk grid width in database
            FillView()
            'create textbox at run time at head of the datagrid        
            CreateTextBox()
        Else
            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumnName.Count - 1
                arrtextvalue.Add(Request.Form("cpnlGrdView:" & arColumnName.Item(i)))
            Next
            '**************************************
            'fill data in datagrid on load on post back event
            FillView()
            CreateTextBox()

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Search"
                            Call BtnGrdSearch_Click(Me, New EventArgs)
                    End Select
                Catch ex As Exception
                    CreateLog("DocumentView", "Load-218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                End Try
            End If
        End If

        'Security Block
        Dim intID As Int32
        If Not IsPostBack Then
            intID = 953
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block

    End Sub

#End Region

#Region "Delete record"
    Private Function DeleteRecord() As Boolean

        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        Dim strConn As String
        Dim objTrn As SqlTransaction


        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        objConn = New SqlConnection(strConn)
        objConn.Open()
        objTrn = objConn.BeginTransaction(IsolationLevel.RepeatableRead)
        Try
            strQuery = "Delete from T220021 where FS_NU9_File_ID_FK=" & Request.Form("txthiddenAdno")
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .Transaction = objTrn
                .ExecuteNonQuery()
            End With

            strQuery = "Delete from T220011 where FM_NU9_File_ID_PK=" & Request.Form("txthiddenAdno")
            cmdCommand.CommandText = strQuery
            cmdCommand.ExecuteNonQuery()
            objTrn.Commit()

            If System.IO.File.Exists(FilePath) = True Then
                System.IO.File.Delete(FilePath)
            End If

            Return True
        Catch ex As Exception
            objTrn.Rollback()
            Response.Write(ex.Message)
            Return False
        Finally
            objConn.Close()
        End Try

    End Function

    Private Function isUsed() As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        Dim strConn As String
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim obj As Object
        Try
            strQuery = "Select count(*) from T060022 where RA_IN4_Role_ID_FK=" & Request.Form("txthiddenAdno")
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                obj = .ExecuteScalar()
            End With
            If (Not obj Is Nothing) Then
                intcount = Convert.ToInt32(obj)
            End If
            If intcount > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ' DisplayError(ex.Message)
            Response.Write(ex.Message)
            Return False
        Finally
            objConn.Close()

        End Try

    End Function

#End Region

#Region "fill View"

    Private Function FillView()

        Dim sqrdView As SqlDataReader
        Dim strSelect As String
        Dim dsFromView As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try

            strSelect = "select FM_NU9_File_ID_PK as FileID, CI_VC36_Name Company,FM_VC256_FileName as FileName,FM_VC512_File_Description as FileDescription,Description as FileGroup, FM_VC256_File_Size as FileSize, convert(varchar,FM_DT8_DateTime) as UploadedDate, Upl.UM_VC50_UserID as UploadedBy  ,FM_VC256_File_Version as Version,FM_VC512_File_Path as FilePath  from T220011,T010011, T060011 Upl, UDC UdcDesc where FM_NU9_Company_ID_FK=CI_NU8_Address_Number and  Upl.UM_IN4_Address_No_FK=FM_NU9_Uploaded_By and UdcDesc.Name= FM_VC8_File_Group and UdcDesc.UDCType='GRP' and FM_NU9_Company_ID_FK in (" & GetCompanySubQuery() & ")  and FM_NU9_Uploaded_By=" & Session("PropUserID") & " order by fileid desc "


            If SQL.Search("T220011", "", "", strSelect, dsFromView, "", "") = True Then
                mdvtable.Table = dsFromView.Tables(0)

                Dim htDateCols As New Hashtable
                htDateCols.Add("UploadedDate", 1)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)

                GrdAddSerach.DataSource = mdvtable.Table
                'GrdAddSerach.Columns.Clear()
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                rowvalue = 0
                GrdAddSerach.DataBind()

                cpnlGrdView.Enabled = True
                cpnlGrdView.State = CustomControls.Web.PanelState.Expanded
                cpnlGrdView.TitleCSS = "test"
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Files not Uploaded so far ...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                cpnlGrdView.Enabled = False
                cpnlGrdView.State = CustomControls.Web.PanelState.Collapsed
                cpnlGrdView.TitleCSS = "test2"
            End If

        Catch ex As Exception
            CreateLog("DocumentView", "FillView-353", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
        intCol = arColumnName.Count - 1

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

                    If intii > 7 And intii < 9 Then
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    End If


                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arColumnName.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))

                    If intii > 7 And intii < 9 Then
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    End If

                    _textbox.Text = arrtextvalue.Item(intii)

                    strcolid = arColumnName.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arColumnName.Item(intii)
                    mTextBox(intii) = _textbox
                End If

            Next
        Catch ex As Exception
            CreateLog("DocumentView", "CreateTextBox-458", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    ''Private Sub FormatGrid(ByVal ColumnName As ArrayList)
    ''    Dim intI As Integer
    ''    Try
    ''        GrdAddSerach.AutoGenerateColumns = False
    ''        For intI = 0 To ColumnName.Count - 1
    ''            Dim Bound_Column As New BoundColumn
    ''            Dim strWidth As String = arColWidth.Item(intI) & "pt"
    ''            Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
    ''            Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
    ''            Bound_Column.ItemStyle.Wrap = True
    ''            GrdAddSerach.Columns.Add(Bound_Column)
    ''        Next

    ''    Catch ex As Exception
    ''        CreateLog("DocumentView", "FormatGrid-562", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
    ''    End Try
    ''End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()


        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(130)
        arColWidth.Add(250)
        arColWidth.Add(250)
        arColWidth.Add(120)
        arColWidth.Add(50)
        arColWidth.Add(100)
        arColWidth.Add(65)
        arColWidth.Add(40)
        arColWidth.Add(0)



        arColumnName.Add("FileID")
        arColumnName.Add("Company")
        arColumnName.Add("FileName")
        arColumnName.Add("FileDescription")
        arColumnName.Add("FileGroup")
        arColumnName.Add("FileSize")
        arColumnName.Add("UploadedDate")
        arColumnName.Add("UploadedBy")
        arColumnName.Add("Version")
        arColumnName.Add("FilePath")


    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumnName.Count - 2
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text
                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
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
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
                ' GrdAddSerach.Columns.Clear()
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If

                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            'GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.AutoGenerateColumns = True
            GrdAddSerach.DataBind()

        Catch ex As Exception
            CreateLog("AB_Search", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim strID As String
        Dim FilePath As String
        Try

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                FilePath = e.Item.Cells(8).Text().Trim
                e.Item.Attributes.Add("style", "cursor:hand")
                e.Item.Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & e.Item.ItemIndex + 1 & "','" & FilePath & "')")
                e.Item.Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & e.Item.ItemIndex + 1 & "','" & FilePath & "')")
            End If

        Catch ex As Exception
            CreateLog("AB_Search", "ItemDataBound-720", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated

        Try

            Dim intA As Integer = 0

            For intI = 0 To arColWidth.Count - 1
                If intI >= 0 Then
                    If intI = 9 Then
                        e.Item.Cells(intA).Width = System.Web.UI.WebControls.Unit.Parse("0px")
                        e.Item.Cells(intA).Visible = False
                    Else
                        e.Item.Cells(intA).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                    End If
                    intA += 1
                End If
            Next

        Catch ex As Exception
        End Try

    End Sub
End Class
