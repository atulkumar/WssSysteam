Imports ION.Data
Imports System.Data
Partial Class SupportCenter_CallView_GetCallsPopup
    Inherits System.Web.UI.Page
    Private arrCols As New ArrayList
    Private dsCalls As New DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        imgClose.Attributes.Add("OnClick", "window.close();")
        imgOk.Attributes.Add("OnClick", "return OK();")

        arrCols.Clear()
        arrCols.Add("txtCallNo")
        arrCols.Add("txtCallSubject")
        arrCols.Add("txtCallType")
        arrCols.Add("txtCompany")
        arrCols.Add("txtProject")
        arrCols.Add("txtRequestedBy")

        If Not IsPostBack Then
            Call BindGrid()
        End If

    End Sub

    Private Function BindGrid() As Boolean
        Try

            Dim strSQL As String
            strSQL = "select CM_NU9_Call_No_PK CallNo, CM_VC100_Subject Subject, CM_VC8_Call_Type CallType, CI_VC36_Name Company,Project.PR_VC20_Name as Project,Req.UM_VC50_UserID as RequestedBy from T040011, T010011,T210011 Project,T060011 Req where CI_NU8_Address_Number=CM_NU9_Comp_Id_FK and  Req.UM_IN4_Address_No_FK=CM_NU9_Call_Owner and CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk  and CM_NU9_Comp_Id_FK=" & Val(Request.QueryString("CompID")) & " and  T040011.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK order by CM_NU9_Call_No_PK DESC"
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("T040011", "", "", strSQL, dsCalls, "", "") = True Then
                Dim strTemp As String = GetFilter()
                If strTemp.Equals("") = False Then
                    dsCalls.Tables(0).DefaultView.RowFilter = strTemp
                End If

                Dim htDescCols As New Hashtable
                htDescCols.Add("Subject", 27)

                HTMLEncodeDecode(mdlMain.Action.Encode, dsCalls.Tables(0).DefaultView, htDescCols)

                grdCalls.DataSource = dsCalls.Tables(0).DefaultView
                grdCalls.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Function
    Private Function GetFilter() As String
        Try
            Dim strFilter As String = ""
            For intI As Integer = 0 To arrCols.Count - 1
                Dim strTemp As String = Request.Form("cpnlCalls$grdCalls$ctl01$" & arrCols(intI))
                If Not IsNothing(strTemp) Then
                    If strTemp.Trim.Equals("") = False Then
                        Select Case dsCalls.Tables(0).Columns(intI).DataType.FullName
                            Case "System.String"
                                strFilter &= dsCalls.Tables(0).Columns(intI).ColumnName & " like '" & strTemp & "' AND "
                            Case Else
                                If IsNumeric(strTemp) = False Then
                                    strTemp = "-9999999999"
                                End If
                                strFilter &= dsCalls.Tables(0).Columns(intI).ColumnName & "=" & strTemp & " AND "
                        End Select
                    End If
                End If
            Next
            If strFilter.Equals("") = False Then
                strFilter = strFilter.Remove(strFilter.Length - 4, 4)
            End If
            strFilter = strFilter.Replace("*", "%")
            Return strFilter
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Sub grdCalls_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCalls.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            e.Item.Attributes.Add("OnClick", "SClick('" & Val(CType(e.Item.Cells(0).FindControl("lblCallNo"), Label).Text.Trim) & "', " & e.Item.ItemIndex + 1 & ")")
            e.Item.Attributes.Add("OnDblClick", "DClick('" & Val(CType(e.Item.Cells(0).FindControl("lblCallNo"), Label).Text.Trim) & "')")
        End If
        If e.Item.ItemType = ListItemType.Header Then
            For intI As Integer = 0 To arrCols.Count - 1
                CType(e.Item.Cells(intI).FindControl(arrCols(intI)), TextBox).Text = Request.Form("cpnlCalls$grdCalls$ctl01$" & arrCols(intI))
            Next
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Call BindGrid()
    End Sub
End Class
