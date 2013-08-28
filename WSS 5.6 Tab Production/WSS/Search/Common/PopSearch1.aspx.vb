'************************************************************************************************************
' Page                 :-Pop Search1
' Purpose              :- Purpose of this screen is to search any User defined Code (UDC).   
' Date				Author						Modification Date					Description
' 4/03/06			Jaswinder		           -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports System.Data
Imports ION.Data


Partial Class Search_Common_PopSearch1
    Inherits System.Web.UI.Page
    Protected Shared mdvtable As DataView = New DataView

    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared mTextBox() As TextBox
    Private Shared intCol As Integer




    Dim strQuery As String
    Dim rowvalue As Integer
    Dim strRowFilterString As String
    Dim strSearch As String

    Dim dsdataset As New DataSet
    Dim flagststus As Integer
    Dim mshFlag As Short
    Public intCount As Integer = 0
    Public strNameParam As String
    Public strIdParam As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'Put user code to initialize the page here
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        imgOk.Attributes.Add("Onclick", "return closeWindow('Ok');")
        imgClose.Attributes.Add("Onclick", "return closeWindow('Close');")

        strQuery = Request.QueryString("ID")
        HttpContext.Current.Session("strtbname") = Request.QueryString("tbname")
        strQuery = strQuery.Replace("""", "'")

        If Not IsPostBack Then
            '''''''''''''''''''''''''''UPdated for keyboard scrolling''''''''''''''''''''''
            'Try
            '    Dim sqRDR As SqlClient.SqlDataReader
            '    Dim blnStatus As Boolean
            '    sqRDR = SQL.Search("", "", strQuery, SQL.CommandBehaviour.Default, blnStatus)
            '    If blnStatus = True Then
            '        While sqRDR.Read
            '            strIdParam = strIdParam & sqRDR(0) & "###"
            '            strNameParam = strNameParam & sqRDR(1) & "###"
            '        End While
            '        sqRDR.Close()
            '    End If
            'Catch ex As Exception

            'End Try
            arCol.Clear()


            FillUDCSearchgrd()
            CreateTextBox()


            'grdsearch.Attributes.Add("ONKEYDOWN", "return checkArrows(this, event, '" & strIdParam & "', '" & strNameParam & "', '" & HttpContext.Current.Session("strtbname") & "')")
            'grdsearch.Attributes.Add("onkeypress", "Select('" & HttpContext.Current.Session("strtbname") & "')")


        Else

            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form(arCol.Item(i)))
            Next


            FillUDCSearchgrd()
            CreateTextBox()


        End If
        Call FormatGrid()
    End Sub

    Private Sub FillUDCSearchgrd()
        Dim sqlcon As SqlConnection
        Try
            sqlcon = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)

            If Not dsdataset.Tables("udc") Is Nothing Then
                dsdataset.Tables("udc").Clear()
            End If

            Dim dasqlda As SqlDataAdapter = New SqlDataAdapter(strQuery, sqlcon)
            dasqlda.Fill(dsdataset, "udc")
            mdvtable.Table = dsdataset.Tables("udc")
            grdsearch.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
            'grdsearch.Width = panel1.Width
            grdsearch.DataSource = mdvtable

            grdsearch.AutoGenerateColumns = False

            For intI As Integer = 0 To mdvtable.Table.Columns.Count - 1
                Dim BC As New BoundColumn
                BC.DataField = mdvtable.Table.Columns(intI).ColumnName
                BC.HeaderText = mdvtable.Table.Columns(intI).ColumnName
                grdsearch.Columns.Add(BC)

            Next



            txtHIDCount.Value = mdvtable.Table.Rows.Count '  intCount
            grdsearch.DataBind()
            Call FilteredData()

        Catch ex As Exception
            CreateLog("PopSearch1", "FillUDCSearchgrd", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        Finally
            sqlcon.Close()
        End Try
    End Sub

    Private Function FormatGrid()
        Dim arColWidth As New ArrayList

        arColWidth.Add(0)
        arColWidth.Add(44)
        arColWidth.Add(220)
        arColWidth.Add(78)

        Try
            For inti As Integer = 0 To grdsearch.Columns.Count - 1
                'Dim Bound_Column As New BoundColumn
                grdsearch.Columns(inti).HeaderStyle.Width = System.Web.UI.WebControls.Unit.Point(arColWidth(inti))
                grdsearch.Columns(inti).ItemStyle.Width = System.Web.UI.WebControls.Unit.Point(arColWidth(inti))
                grdsearch.Columns(inti).ItemStyle.Wrap = True
                'grdsearch.Columns.Add(Bound_Column)
            Next
            grdsearch.Width = Unit.Point(352)
        Catch ex As Exception
            CreateLog("PopSearch1", "FormatGrid-174", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    Private Function CreateTextBox()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arColumns.Clear()

        'fill the columns count into the array from mdvtable view


        Try

            intCol = mdvtable.Table.Columns.Count

            If Not IsPostBack Then
                ReDim mTextBox(intCol)
            End If



            For intii = 0 To mdvtable.Table.Columns.Count - 1
                Dim rr As String
                rr = mdvtable.Table.Columns(intii).ColumnName()
                _textbox = New TextBox


                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String


                    If (intii = 0) Then
                        col1 = Unit.Parse(44)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    ElseIf (intii = 1) Then
                        col1 = Unit.Parse(220)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    Else
                        col1 = Unit.Parse(78)
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    End If

                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(mdvtable.Table.Columns(intii).ColumnName())
                    pndgsrch.Controls.Add(Page.ParseControl("<asp:TextBox id=" & rr & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = rr
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String

                    If (intii = 0) Then
                        col1 = Unit.Parse(44)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    ElseIf (intii = 1) Then
                        col1 = Unit.Parse(220)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    Else
                        col1 = Unit.Parse(78)
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    End If

                    _textbox.Text = arrtextvalue.Item(intii)
                    strcolid = arCol.Item(intii)
                    pndgsrch.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arCol.Item(intii)
                    mTextBox(intii) = _textbox

                End If

                mshFlag = 1
                arColumns.Add(_textbox.ID)

            Next
        Catch ex As Exception
            CreateLog("popSearch1", "CreateTextBox-265", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Sub btsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btsearch.Click
        intCount = 0
        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not arrtextvalue(intI).Text.Trim.Equals("") Then
                    strSearch = arrtextvalue(intI).Text
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

                grdsearch.DataSource = mdvtable
                txtHIDCount.Value = mdvtable.Table.Rows.Count '  intCount
                grdsearch.DataBind()
                FormatGrid()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            ''''''''''''''''''''''''''UPdated for keyboard scrolling''''''''''''''''''''''
            Try
                Dim sqRDR As SqlClient.SqlDataReader
                Dim blnStatus As Boolean
                sqRDR = SQL.Search("popsearch1", "btsearch_Click-327", strRowFilterString, SQL.CommandBehaviour.Default, blnStatus)
                If blnStatus = True Then
                    While sqRDR.Read
                        strIdParam = strIdParam & sqRDR(0) & "###"
                        strNameParam = strNameParam & sqRDR(1) & "###"
                    End While
                    sqRDR.Close()
                End If
            Catch ex As Exception
                CreateLog("popSearch1", "Click-336", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try

            grdsearch.DataSource = mdvtable
            txtHIDCount.Value = mdvtable.Table.Rows.Count '  intCount
            grdsearch.DataBind()

            FormatGrid()
        Catch ex As Exception
            CreateLog("AB_Search", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
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
                'shF = 1

                '''''''''''''''''''''''''''UPdated for keyboard scrolling''''''''''''''''''''''
                'Try
                '    Dim sqRDR As SqlClient.SqlDataReader
                '    Dim blnStatus As Boolean



                '    strIdParam = ""
                '    strNameParam = ""
                '    grdsearch.Columns.Clear()
                '    sqRDR = SQL.Search("", "", strQuery, SQL.CommandBehaviour.Default, blnStatus)
                '    While sqRDR.Read
                '        strIdParam = strIdParam & sqRDR(0) & "###"
                '        strNameParam = strNameParam & sqRDR(1) & "###"
                '    End While
                '    sqRDR.Close()

                '    grdsearch.Attributes.Add("ONKEYDOWN", "return checkArrows(this, event, '" & strIdParam & "', '" & strNameParam & "', '" & HttpContext.Current.Session("strtbname") & "')")
                '    grdsearch.Attributes.Add("onkeypress", "Select('" & HttpContext.Current.Session("strtbname") & "')")


                'Catch ex As Exception

                'End Try

                grdsearch.DataSource = mdvtable
                grdsearch.DataBind()
                Call FilteredData()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString


            '''''''''''''''''''''''''''UPdated for keyboard scrolling''''''''''''''''''''''
            'Try
            '    Dim sqRDR As SqlClient.SqlDataReader
            '    Dim blnStatus As Boolean
            '    '  Dim strNewQuery As String

            '    strQuery &= " and  " & strRowFilterString

            '    grdsearch.Columns.Clear()

            '    strIdParam = ""
            '    strNameParam = ""
            '    '   strNewQuery=

            '    sqRDR = SQL.Search("", "", strQuery, SQL.CommandBehaviour.Default, blnStatus)
            '    While sqRDR.Read
            '        strIdParam = strIdParam & sqRDR(0) & "###"
            '        strNameParam = strNameParam & sqRDR(1) & "###"
            '    End While
            '    sqRDR.Close()

            'grdsearch.Attributes.Add("ONKEYDOWN", "return checkArrows(this, event, '" & strIdParam & "', '" & strNameParam & "', '" & HttpContext.Current.Session("strtbname") & "')")
            'grdsearch.Attributes.Add("onkeypress", "Select('" & HttpContext.Current.Session("strtbname") & "')")


            'Catch ex As Exception

            'End Try


            grdsearch.DataSource = mdvtable


            grdsearch.DataBind()
            Call FilteredData()
            txtHIDCount.Value = grdsearch.Items.Count
            'GetColumns()
        Catch ex As Exception
            CreateLog("popSearch1", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
    Private Sub FilteredData()
        Try
            strIdParam = ""
            strNameParam = ""
            If IsPostBack Then
                For inti As Integer = 0 To grdsearch.Items.Count - 1
                    'strIdParam = strIdParam & grdsearch.Items(inti).Cells(0).Text & "###"
                    'strNameParam = strNameParam & grdsearch.Items(inti).Cells(1).Text & "###"
                    strIdParam = strIdParam & grdsearch.Items(inti).Cells(1).Text & "###"
                    strNameParam = strNameParam & grdsearch.Items(inti).Cells(2).Text & "###"
                Next
            Else
                For inti As Integer = 0 To grdsearch.Items.Count - 1
                    strIdParam = strIdParam & grdsearch.Items(inti).Cells(1).Text & "###"
                    strNameParam = strNameParam & grdsearch.Items(inti).Cells(2).Text & "###"
                Next
            End If
            strIdParam = strIdParam.Replace(vbCrLf, "")
            strNameParam = strNameParam.Replace(vbCrLf, "")
            grdsearch.Attributes.Add("ONKEYDOWN", "return checkArrows(this, event, '" & strIdParam & "', '" & strNameParam & "', '" & HttpContext.Current.Session("strtbname") & "')")
            grdsearch.Attributes.Add("onkeypress", "Select('" & HttpContext.Current.Session("strtbname") & "')")

            Dim dgItem As DataGridItem
            For Each dgItem In grdsearch.Items
                If IsPostBack Then
                    dgItem.Attributes.Add("onclick", "KeyCheck('" & dgItem.Cells(0).Text.Trim & "' ,'" & dgItem.Cells(1).Text & "', '" & dgItem.ItemIndex + 1 & "', '" & HttpContext.Current.Session("strtbname") & "', '" & strIdParam & "','" & "" & "', '" & strNameParam & "')")
                Else
                    dgItem.Attributes.Add("onclick", "KeyCheck('" & dgItem.Cells(1).Text.Trim & "' ,'" & dgItem.Cells(2).Text & "', '" & dgItem.ItemIndex + 1 & "', '" & HttpContext.Current.Session("strtbname") & "', '" & strIdParam & "','" & "" & "', '" & strNameParam & "')")
                End If
            Next
        Catch ex As Exception
            CreateLog("popSearch1", "FilteredData-509", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Exit Sub
        End Try
    End Sub
    Private Sub grdsearch_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdsearch.ItemDataBound

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim strID As String = mdvtable.Table.Rows(e.Item.ItemIndex).Item(0)
            Dim strTempName As String = mdvtable.Table.Rows(e.Item.ItemIndex).Item(1)
            e.Item.Attributes.Add("style", "cursor:hand")
            'e.Item.Attributes.Add("onclick", "KeyCheck('" & mdvtable.Table.Rows(e.Item.ItemIndex).Item(0) & "' ,'" & mdvtable.Table.Rows(e.Item.ItemIndex).Item(1) & "', '" & e.Item.ItemIndex + 1 & "', '" & HttpContext.Current.Session("strtbname") & "', '" & strIdParam & "','" & strTempName & "', '" & strNameParam & "')")
            e.Item.Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & HttpContext.Current.Session("strtbname") & "', '" & rowvalue & "','" & strTempName & "')")
            intCount = intCount + 1
        End If
    End Sub
End Class
