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
Imports System.IO
Imports System.Data

Partial Class ImportExport_List_View
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCSS(Me.Page)
        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        cpnlErrorPanel.Visible = False
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")


        Dim txthiddenImage = Request.Form("txthiddenImage")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString


        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
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
            intID = 966
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block

    End Sub

#End Region



#Region "fill View"

    Private Function FillView()

        Dim sqrdView As SqlDataReader
        Dim strSelect As String
        Dim dsFromView As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try

            strSelect = "select FM_VC256_FileName FileName,  CI_VC36_Name CompanyName, UDC.Description FileGroup, FM_VC512_File_Description Description,   FM_VC256_File_Size FileSize, FM_VC256_File_Version FileVersion, convert(varchar,FM_NU9_Inserted_Date) UploadDate, UM_VC50_UserID UploadedBy, FM_NU9_File_ID_PK,FM_VC512_File_Path as FilePath from T220011, T060011 , T010011, UDC  where  UDC.UDCType='GRP' and UDC.Name=FM_VC8_File_Group and  FM_NU9_Uploaded_By = UM_IN4_Address_No_FK and CI_NU8_Address_Number = FM_NU9_Company_ID_FK  and FM_NU9_File_ID_PK in ( (select FS_NU9_File_ID_FK from T220021 where FS_VC4_Status='ENB' and FS_DT8_Valid_From <= '" & Now.Date & "' and FS_DT8_Valid_UpTo >= '" & Now.Date & "' and FS_VC8_Object_Type='ROLE' and FS_NU9_Object_ID_FK = " & Val(Session("PropRole")) & " ) union (select FS_NU9_File_ID_FK from T220021 where FS_VC4_Status='ENB' and FS_DT8_Valid_From <= '" & Now.Date & "' and  FS_DT8_Valid_UpTo >= '" & Now.Date & "' and FS_VC8_Object_Type='USER' and FS_NU9_Object_ID_FK = " & Val(Session("PropUserID")) & "))"


            If SQL.Search("T220011", "", "", strSelect, dsFromView, "", "") = True Then
                mdvtable.Table = dsFromView.Tables(0)

                Dim htDateCols As New Hashtable
                htDateCols.Add("UploadDate", 2)

                htDateCols.Add("UploadedDate", 1)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)
                GrdAddSerach.DataSource = mdvtable.Table

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

                    If arColumnName.Item(intii) = "Attachment" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " BackColor=""WhiteSmoke"" readonly=""true"" height=""20px"" BorderWidth=""1px"" BorderColor=""Gray""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If


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

                    If strcolid = "Attachment" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server""  Width=" & col1cng & " BackColor=""WhiteSmoke"" height=""20px""  readonly=""true"" BorderWidth=""1px"" BorderColor=""Gray""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If

                    _textbox.ID = arColumnName.Item(intii)
                    mTextBox(intii) = _textbox
                End If

            Next
        Catch ex As Exception
            CreateLog("DocumentView", "CreateTextBox-458", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "get columns from database"

    Private Sub GetColumns()


        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(80)
        arColWidth.Add(200)
        arColWidth.Add(120)
        arColWidth.Add(100)
        arColWidth.Add(300)
        arColWidth.Add(50)
        arColWidth.Add(65)
        arColWidth.Add(100)
        arColWidth.Add(80)
        arColWidth.Add(200)

        arColumnName.Add("Attachment")
        arColumnName.Add("FileName")
        arColumnName.Add("CompanyName")
        arColumnName.Add("FileGroup")
        arColumnName.Add("Description")
        arColumnName.Add("FileSize")
        arColumnName.Add("FileVersion")
        arColumnName.Add("UploadDate")
        arColumnName.Add("UploadedBy")
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
                    If (mdvtable.Table.Columns(mTextBox(intI).ID).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(mTextBox(intI).ID).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(mTextBox(intI).ID).DataType.FullName = "System.DateTime") Then
                        If (mdvtable.Table.Columns(mTextBox(intI).ID).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) Then
                            Else
                                Exit Sub
                            End If
                        End If
                        If (mdvtable.Table.Columns(mTextBox(intI).ID).DataType.FullName = "System.Decimal") = True Then
                        End If
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(mTextBox(intI).ID).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mTextBox(intI).Text.Trim
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(mTextBox(intI).ID).ColumnName & " like " & "'" & strSearch & "' AND "
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
            GrdAddSerach.DataSource = mdvtable

            GrdAddSerach.DataBind()

        Catch ex As Exception
            CreateLog("AB_Search", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                e.Item.Attributes.Add("style", "cursor:hand")
                e.Item.Attributes.Add("onclick", "javascript:KeyCheck('" & e.Item.ItemIndex + 1 & "')")

            End If

        Catch ex As Exception
            CreateLog("AB_Search", "ItemDataBound-720", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region



    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("../Home.aspx", False)
    End Sub

End Class
