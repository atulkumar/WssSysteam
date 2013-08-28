Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data


Partial Class MonitoringCenter_RPA_Search
    Inherits System.Web.UI.Page
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private arrSearchText As New ArrayList

    Private mdvRPA As DataView

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        cpnlError.Visible = False
        txtCSS(Me.Page)

        imgEdit.Attributes.Add("onclick", "return SaveEdit('Edit');")
        imgAdd.Attributes.Add("onclick", "return SaveEdit('Add');")
        Try
            If Not IsPostBack Then
                DefineColumnData()
            End If

            If IsPostBack Then
                Dim strhiddenImage As String
                strhiddenImage = Request.Form("txthiddenImage")

                Dim intID As Integer = Val(Request.Form("txthiddenID"))
                Dim intReqID As Integer = Val(Request.Form("txthiddenReqID"))

                If strhiddenImage <> "" Then

                    Select Case strhiddenImage
                        Case "Add"
                            Response.Redirect("RPA.aspx?ID=-1" & "&Status=Add", False)
                        Case "Edit"
                            Response.Redirect("RPA.aspx?ID=" & intID & "&ReqID=" & intReqID & "&Status=" & Request.Form("txthiddenStatus"), False)
                        Case "Logout"
                            LogoutWSS()
                    End Select

                End If
            Else



            End If
            BindGrid()
            FormatGrid()

        Catch ex As Exception
            CreateLog("RPA", "Load-89", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Sub

    'Bind the datagrid
    Private Function BindGrid()
        Try
            Dim strSQL As String
            Dim dsRPA As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T130032"
            SQL.DBTracing = False
            strSQL = "select "
            For inti As Integer = 0 To arrColumnName.Count - 1

                strSQL &= arrColumnName(inti) & ", "

            Next
            strSQL = strSQL.Trim
            strSQL = strSQL.Substring(0, strSQL.Length - 1)
            strSQL &= " from T130022,T130031,t010011 where RQ_NU9_CLIENTID_FK=CI_NU8_Address_Number and RQ_NU9_ProcessID='10020017' and RQ_NU9_ProcessID=PM_nu9_PCode"

            If SQL.Search("T130022", "RPA_Search", "BindGrid", strSQL, dsRPA, "", "") = True Then
                mdvRPA = dsRPA.Tables("T130022").DefaultView
                ModifyDataView()
                mdvRPA.RowFilter = GetRowFilter()
                dgrRPA.DataSource = mdvRPA.Table
                dgrRPA.DataBind()
            Else
                cpnlRPA.State = CustomControls.Web.PanelState.Collapsed
                cpnlRPA.Enabled = False
                lstError.Items.Clear()
                lstError.Items.Add("No RPA request exists...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("RPA", "BindGrid-124", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    'Format the grid
    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrRPA.Columns(inti).HeaderStyle.Width = Unit.Point(arrColWidth(inti))
                dgrRPA.Columns(inti).ItemStyle.Width = Unit.Point(arrColWidth(inti))
                dgrRPA.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("RPA", "FormatGrid-133", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    'get the text from search textboxes in an array list
    Private Function GetSeacrhText()
        Try


            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                strSearch = Request.Form("cpnlRPA:dgrRPA:_ctl1:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    strSearch = GetSearchString(strSearch)
                End If


                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("RPA", "GetSeacrhText-160", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function
    'Gets the row filter string
    Private Function GetRowFilter() As String
        Try

            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To mdvRPA.Table.Columns.Count - 2
                If arrSearchText(inti) <> "" Then
                    If mdvRPA.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                    ElseIf mdvRPA.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
                        If IsNumeric(arrSearchText(inti)) = False Then
                            strRowFilter &= " " & arrColumnName(inti) & "=-101019 and"      'change row filter to non existing value
                        Else
                            strRowFilter &= " " & arrColumnName(inti) & "=" & arrSearchText(inti) & " and"
                        End If
                    Else
                        strRowFilter &= " " & arrColumnName(inti) & " like " & arrSearchText(inti) & " and"
                    End If
                End If
            Next

            If strRowFilter <> Nothing Then
                strRowFilter = strRowFilter.Replace("*", "%")
                If strRowFilter.Substring(strRowFilter.Length - 3, 3) = "and" Then
                    strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3)
                Else
                    strRowFilter = strRowFilter
                End If
            End If
            Return strRowFilter
        Catch ex As Exception
            CreateLog("RPA", "GetRowFilter-195", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    Private Sub dgrRPA_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrRPA.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                For inti As Integer = 0 To mdvRPA.Table.Columns.Count - 2

                    e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                    e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55()")
                    e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                    'e.Item.Cells(inti + 1).ToolTip = mdvRPA.Table.Rows(e.Item.ItemIndex).Item(inti)
                Next
            Else

                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlRPA:dgrRPA:_ctl1:" & arrTextboxName(inti))
                    End If
                Next

            End If
        Catch ex As Exception
            CreateLog("RPA", "dgrRPA_ItemDataBound-222", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Function ModifyDataView()
        Try

            For inti As Integer = 0 To mdvRPA.Table.Rows.Count - 1
                Dim strProcessType As String = mdvRPA.Table.Rows(inti).Item("RQ_VC150_CAT3")
                Dim strProcessStatus As String = mdvRPA.Table.Rows(inti).Item("RQ_CH2_STATUS")

                Select Case strProcessType.Trim.ToUpper
                    Case "AT"
                        mdvRPA.Table.Rows(inti).Item("RQ_VC150_CAT3") = "Automatic"
                    Case "MT"
                        mdvRPA.Table.Rows(inti).Item("RQ_VC150_CAT3") = "Manual"
                End Select

                Select Case strProcessStatus.Trim.ToUpper
                    Case "P"
                        mdvRPA.Table.Rows(inti).Item("RQ_CH2_STATUS") = "Pending"
                    Case "C"
                        mdvRPA.Table.Rows(inti).Item("RQ_CH2_STATUS") = "Complete"
                    Case "S"
                        mdvRPA.Table.Rows(inti).Item("RQ_CH2_STATUS") = "Sent"
                End Select
            Next
            mdvRPA.Table.AcceptChanges()
        Catch ex As Exception
            CreateLog("RPA", "ModifyDataView-230", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    Private Function DefineColumnData()
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(60)
        arrColWidth.Add(120)
        arrColWidth.Add(120)
        arrColWidth.Add(200)
        arrColWidth.Add(200)
        arrColWidth.Add(60)
        arrColWidth.Add(80)
        arrColWidth.Add(70)
        arrColWidth.Add(140)

        arrTextboxName.Clear()
        arrTextboxName.Add("txtRequestID")
        arrTextboxName.Add("txtProcessName")
        arrTextboxName.Add("txtProject")
        arrTextboxName.Add("txtENV")
        arrTextboxName.Add("txtNotes")
        arrTextboxName.Add("txtDate")
        arrTextboxName.Add("txtClient")
        arrTextboxName.Add("txtRequestType")
        arrTextboxName.Add("txtStatus")

        arrColumnName.Clear()
        'arrColumnName.Add("EP_VC50_RequestID")
        'arrColumnName.Add("PM_VC20_PName")
        'arrColumnName.Add("EP_NU9_Process_ID")
        'arrColumnName.Add("EP_VC50_Field1")
        'arrColumnName.Add("EP_VC50_Field2")
        'arrColumnName.Add("EP_VC50_Client")
        'arrColumnName.Add("EP_VC50_Field3")
        'arrColumnName.Add("EP_CH2_Status")
        'arrColumnName.Add("EP_NU9_SEQ_ID")


        arrColumnName.Add("RQ_NU9_REQUEST_ID")
        arrColumnName.Add("PM_VC20_PName")
        arrColumnName.Add("RQ_VC150_CAT1")
        arrColumnName.Add("RQ_VC150_CAT5")
        arrColumnName.Add("RQ_VC150_CAT8")
        arrColumnName.Add("RQ_VC150_CAT2")
        arrColumnName.Add("CI_VC36_Name ")
        arrColumnName.Add("RQ_VC150_CAT3")
        arrColumnName.Add("RQ_CH2_STATUS")
        arrColumnName.Add("RQ_NU9_SQID_PK")
    End Function
End Class
