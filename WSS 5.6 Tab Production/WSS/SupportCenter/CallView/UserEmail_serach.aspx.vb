Imports ION.data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports System.Data

Partial Class SupportCenter_CallView_UserEmail_serach
    Inherits System.Web.UI.Page

    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected Shared mdvtable As DataView = New DataView
    Private Shared mTextBox() As TextBox
    Private Shared arColWidth As New ArrayList
    Private arColumnName As New ArrayList
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared intCol As Integer
    Private mshFlag As Short
    Private Shared arrIDs As New ArrayList
    'Private arrIDs As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Dim rowvalue As Integer


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        txtCSS(Me.Page)
        imgClose.Attributes.Add("onclick", "return closeWindow('Close')")
        Dim strAno As String
        Dim asUserID As New ArrayList
        Dim strAnv As String
        Dim strChk As String
        Dim strEmailCheck As String

        strAno = Request.QueryString("userId")
        If Not IsPostBack Then
            arrIDs.Clear()
            strAnv = Session("UserID")
            strChk = Session("CheckGetSearch")
            strEmailCheck = Session("CommentPage_EmailId")
            Dim ArrList As New ArrayList()
            If strAno.Trim.Equals("") = False Then
                'strAno = strAno.Remove(strAno.Length - 1, 1)
                For inti As Integer = 0 To strAno.Split(",").Length - 1
                    arrIDs.Add(strAno.Split(",")(inti))
                    If strEmailCheck IsNot Nothing Then
                        Dim strArr As String() = Nothing
                        strArr = strEmailCheck.Split(";")
                        For iSplitEmailId As Integer = 0 To strArr.Length - 1
                            arrIDs.Add(strArr(iSplitEmailId))
                            Session("CommentPage_EmailId") = Nothing
                        Next
                    End If
                    'If strEmailCheck IsNot Nothing Then
                    '    ArrList.Add(strEmailCheck.Split(";"))
                    '    For iSplitEmail As Integer = 0 To ArrList.Count - 1
                    '        arrIDs.Add(strEmailCheck.Split(";")(inti))
                    '    Next
                    '    'arrIDs.Add(strEmailCheck.Split(";")(inti))
                    '    Session("CommentPage_EmailId") = Nothing
                    'End If
                Next
            End If
            If strChk IsNot Nothing Then
                For ints As Integer = 0 To strChk.Split(",").Length - 1
                    arrIDs.Add(strChk.Split(",")(ints))
                Next
            End If
            If strAnv IsNot Nothing Then
                For intv As Integer = 0 To strAnv.Split(",").Length - 1
                    If Not arrIDs.Contains(strAnv) Then
                        arrIDs.Add(strAnv.Split(",")(intv))
                    End If
                Next
            End If
        Else
            ReadUserID(asUserID)
            For intv As Integer = 0 To asUserID.Count - 1
                If Not arrIDs.Contains(asUserID(intv)) Then
                    arrIDs.Add(asUserID(intv))
                End If
            Next

        End If
        If IsPostBack Then
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form(arCol.Item(i)))
            Next


        Else

            FillView()
            CreateTextBox()
        End If


    End Sub

#Region "fill View"

    Private Function FillView()
        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String
        Dim strActionType As String = Request.QueryString("ActionType")

        If strActionType = "External" Then
            'strselect = "select UM_IN4_Address_No_FK, CI_VC36_Name, CI_VC28_Email_1 from T060011,T010011 where UM_IN4_Address_No_FK=CI_NU8_Address_Number and  CI_VC8_Status<>'DISA' and (UM_IN4_Company_AB_ID in(select CI_NU8_Address_Number from T010011 where  CI_nu8_Address_Number=" & Session("PropCAComp") & ") or UM_IN4_Company_AB_ID in(select CI_NU8_Address_Number from T010011 where CI_IN4_Business_Relation='SCM')) order by CI_VC36_Name  "
            'strselect = "select UM_IN4_Address_No_FK, CI_VC36_Name, CI_VC28_Email_1 from T060011,T010011 where UM_IN4_Address_No_FK=CI_NU8_Address_Number and  CI_VC8_Status<>'DISA' and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Company_AB_ID in (" & GetCompanySubQuery() & ")  order by CI_VC36_Name"
            If Val(Session("PropCallNumber")) = 0 Then
                'strselect = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']' as Name, T2.CI_VC28_Email_1 EmailID,  T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(Session("PropCompanyID")) & " and UC_BT1_Access=1)  Order By Name"
                strselect = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']' as Name, T2.CI_VC28_Email_1 EmailID,  T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_BT1_Access=1)  Order By Name"

            Else
                strselect = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']' as Name, T2.CI_VC28_Email_1 EmailID,  T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(Session("PropCAComp")) & " and UC_BT1_Access=1)  Order By Name"
            End If
        Else
            'strselect = "select UM_IN4_Address_No_FK, CI_VC36_Name, CI_VC28_Email_1 from T060011,T010011 where UM_IN4_Address_No_FK=CI_NU8_Address_Number and CI_VC8_Status<>'DISA'  and UM_IN4_Company_AB_ID in(select CI_NU8_Address_Number from T010011 where CI_IN4_Business_Relation='SCM') order by CI_VC36_Name  "
            'strselect = "select UM_IN4_Address_No_FK, CI_VC36_Name, CI_VC28_Email_1 from T060011,T010011 where UM_IN4_Address_No_FK=CI_NU8_Address_Number and  CI_VC8_Status<>'DISA' and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Company_AB_ID in(" & Val(Session("PropCAComp")) & ") order by CI_VC36_Name"
            If Val(Session("PropCallNumber")) = 0 Then
                'strselect = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']' as Name, T2.CI_VC28_Email_1 EmailID,  T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and T1.CI_IN4_Business_Relation='SCM' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(Session("PropCompanyID")) & " and UC_BT1_Access=1)  Order By Name"
                strselect = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']' as Name, T2.CI_VC28_Email_1 EmailID,  T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and T1.CI_IN4_Business_Relation='SCM' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_BT1_Access=1)  Order By Name"

            Else
                strselect = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']' as Name, T2.CI_VC28_Email_1 EmailID,  T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and T1.CI_IN4_Business_Relation='SCM' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(Session("PropCAComp")) & " and UC_BT1_Access=1)  Order By Name"
            End If
        End If

        'strselect = "select CI_NU8_Address_Number,CI_VC36_Name,CI_VC28_Email_1 from t010011 a where  a.CI_VC8_Address_Book_Type<>'COM'"
        'SQL.DBTable = "t010011"

        If SQL.Search("t010011", "UserEmail_Search", "FillView-95", strselect, dsFromView, "sachin", "Prashar") Then
            Try
                mdvtable.Table = dsFromView.Tables("t010011")
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                'FormatGrid()
                GetColumns()
                'CreateTextBox()
                Return True

            Catch ex As Exception
                CreateLog("OW Views", "Fill View", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else
            Dim ds As New DataSet
            ds.Tables.Add("Dummy")
            ds.Tables("Dummy").Columns.Add("MachName")
            Dim dtRow As DataRow
            dtRow = ds.Tables("Dummy").NewRow
            dtRow.Item(0) = ""
            mdvtable.Table = ds.Tables("Dummy")
            GrdAddSerach.DataSource = mdvtable.Table
            GrdAddSerach.DataBind()
            FormatGrid()
        End If

    End Function

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()

        arrColWidth.Clear()
        arrColWidth.Add(0) 'SQID
        arrColWidth.Add(30) 'check BOx
        arrColWidth.Add(178)
        arrColWidth.Add(220)
        arrColWidth.Add(120)
        Dim intI As Integer
        Try
            GrdAddSerach.Columns.Clear()
            GrdAddSerach.AutoGenerateColumns = False
            For intI = 1 To arrColWidth.Count - 1

                GrdAddSerach.Columns(intI).ItemStyle.Width = Unit.Pixel(arrColWidth(intI))
                GrdAddSerach.Columns(intI).ItemStyle.Wrap = True
                'Dim Bound_Column As New BoundColumn
                'Bound_Column.ItemStyle.Wrap = True
                'GrdAddSerach.Columns.Add(Bound_Column)
            Next
        Catch ex As Exception
            CreateLog("UserEmail_search", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()
        arColWidth.Clear()
        arColumnName.Clear()
        arColWidth.Add(120)
        arColumnName.Add("MachName")
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

                        col1 = Unit.Parse(10)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    ElseIf (intii = 1) Then
                        col1 = Unit.Parse(130)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"

                    ElseIf (intii = 2) Then

                        col1 = Unit.Parse(160)
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    ElseIf (intii = 3) Then
                        col1 = Unit.Parse(90)
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    End If

                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(mdvtable.Table.Columns(intii).ColumnName())
                    If intii = 0 Then
                        pndgsrch.Controls.Add(Page.ParseControl("<asp:TextBox id=" & rr & " runat=""server"" ReadOnly=""true""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    Else
                        pndgsrch.Controls.Add(Page.ParseControl("<asp:TextBox id=" & rr & " runat=""server""   Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If

                    _textbox.ID = rr
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String

                    If (intii = 0) Then
                        col1 = Unit.Parse(10)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    ElseIf (intii = 1) Then
                        col1 = Unit.Parse(130)
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"

                    ElseIf (intii = 2) Then

                        col1 = Unit.Parse(160)
                        col1cng = col1.Value + 1
                        col1cng = col1cng & "pt"
                    ElseIf (intii = 3) Then
                        col1 = Unit.Parse(90)
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

#End Region

    Private Sub imgOk_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOk.Click

        Dim arMach As New ArrayList
        Dim arUserID As New ArrayList
        Dim arCols As New ArrayList
        Dim arRow As New ArrayList
        Dim DomID, Mid As Integer
        Dim flg As Boolean
        Dim strEmailId As String
        Dim strUserID As String

        ReadGrid(arMach)
        For cnt As Integer = 0 To arMach.Count - 1
            If cnt <> 0 Then
                strEmailId = strEmailId + ";" + arMach(cnt)
            Else
                strEmailId = arMach(cnt)
            End If
        Next

        ReadUserID(arUserID)
        For cnt1 As Integer = 0 To arUserID.Count - 1
            If cnt1 <> 0 Then
                strUserID = strUserID + "," + arUserID(cnt1)
            Else
                strUserID = arUserID(cnt1)
            End If
        Next



        Session("UserID") = ""
        Session("UserID") = strUserID

        Session("SEmailId") = ""
        Session("SEmailId") = strEmailId

        Dim popupScript As String = "<script language='javascript'>" & _
           "self.opener.EmailList('" & strEmailId & "','" & strUserID & "');window.close();" & _
           "</script>"
        Page.RegisterStartupScript("PopupScript", popupScript)

        ' Dim popupScript1 As String = "<script language='javascript'>" & _
        ' "  window.close();" & _
        '"</script>"
        ' Page.RegisterStartupScript("PopupScript1", popupScript1)

    End Sub
    Sub ReadGrid(ByRef dtMach As ArrayList)
        Try
            Dim gridrow As DataGridItem
            Dim drow As DataRow
            Dim strEnb As String
            dtMach.Clear()
            For Each gridrow In GrdAddSerach.Items

                If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                    'If Not MachExists(CType(gridrow.FindControl("lblEmailId"), Label).Text, strEnb) Then
                    dtMach.Add(CType(gridrow.FindControl("lblEmailId"), Label).Text)
                    'End If
                End If
            Next
        Catch ex As Exception
            CreateLog("UserEmail_Search", "imgOk_Click-304", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Sub ReadUserID(ByRef dtUserID As ArrayList)
        Try
            Dim gridrow As DataGridItem
            Dim drow As DataRow
            Dim strEnb As String
            dtUserID.Clear()
            For Each gridrow In GrdAddSerach.Items

                If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                    'If Not MachExists(CType(gridrow.FindControl("lblEmailId"), Label).Text, strEnb) Then
                    dtUserID.Add(gridrow.Cells(0).Text)
                    'End If
                End If
            Next
        Catch ex As Exception
            CreateLog("UserEmail_Search", "imgOk_Click-304", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Sub ReadSearchUserID(ByRef dtUserID As ArrayList)
        Try
            Dim gridrow As DataGridItem
            Dim drow As DataRow
            Dim strEnb As String
            dtUserID.Clear()
            For Each gridrow In GrdAddSerach.Items
                If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                    'If Not MachExists(CType(gridrow.FindControl("lblEmailId"), Label).Text, strEnb) Then
                    dtUserID.Add(gridrow.Cells(0).Text + "#")
                Else
                    dtUserID.Add(gridrow.Cells(0).Text)
                    'End If
                End If
            Next
        Catch ex As Exception
            CreateLog("UserEmail_Search", "imgOk_Click-304", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub


    Private Sub GrdAddSerach_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            CType(e.Item.FindControl("chkReq"), System.Web.UI.WebControls.CheckBox).Attributes.Add("onclick", "CheckBox('" & CType(e.Item.FindControl("chkReq"), System.Web.UI.WebControls.CheckBox).ClientID & "')")

            e.Item.Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & ")")

            If arrIDs.Contains(e.Item.Cells(0).Text) Then

                CType(e.Item.FindControl("chkReq"), CheckBox).Checked = True
                e.Item.Enabled = True
                'e.Item.Enabled = False
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            CType(e.Item.FindControl("chkAll"), System.Web.UI.WebControls.CheckBox).Attributes.Add("onclick", "CheckAll('" & CType(e.Item.FindControl("chkAll"), System.Web.UI.WebControls.CheckBox).ClientID & "')")
        End If

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try

            Dim asMach As New ArrayList
            Dim asUserID As New ArrayList
            Dim strUserChk As String
            Dim strUsrID As String
            Dim strSearches As String
            ReadGrid(asMach)
            For count As Integer = 0 To asMach.Count - 1
                If count <> 0 Then
                    strUserChk = strUserChk + "," + asMach(count)
                Else
                    strUserChk = asMach(count)
                End If
            Next
            ReadSearchUserID(asUserID)
            For count1 As Integer = 0 To asUserID.Count - 1
                If count1 <> 0 Then
                    strUsrID = strUsrID + "," + asUserID(count1)
                Else
                    strUsrID = asUserID(count1)
                End If
            Next
            ' Dim arrSearchUserId As ArrayList
            If arrIDs.ToArray() IsNot Nothing Then
                'If Not IsPostBack Then
                'arrSearchUserId = New ArrayList
                'End If
                If strUsrID IsNot Nothing Then
                    For intv As Integer = 0 To asUserID.Count - 1
                        If asUserID(intv).ToString().Contains("#") Then
                            asUserID(intv) = asUserID(intv).ToString().Replace("#", "")

                            strSearch = strSearches + "," + asUserID(intv)
                            Session.Add("CheckGetSearch", strSearches)
                            If Not Session.Contents(asUserID(intv)) Then
                                'If Not arrIDs.Contains(asUserID(intv)) Then
                                arrIDs.Add(asUserID(intv))
                                'arrSearchUserId = arrIDs


                                ' End If
                            End If
                        Else
                            If arrIDs.Contains(asUserID(intv)) Then
                                arrIDs.Remove(asUserID(intv))
                            End If
                        End If
                    Next
                End If
            End If


            FillView()
            CreateTextBox()
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
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & "%" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then

                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                Exit Sub
            End If
            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)

            ' strRowFilterString = "%" & strSearch
            mdvtable.RowFilter = strRowFilterString
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.DataBind()
            txtHIDCount.Value = GrdAddSerach.Items.Count
            'GetColumns()


            '----------------------------------------


            'Session("SEmailId") = ""
            'Session("SEmailId") = strUserChk
            '----------------------------------------
            'Dim arMach As New ArrayList
            'Dim arUserID As New ArrayList
            'Dim arCols As New ArrayList
            'Dim arRow As New ArrayList
            'Dim DomID, Mid As Integer
            'Dim flg As Boolean
            'Dim strEmailId As String
            'Dim strUserID As String

            'ReadGrid(arMach)
            'For cnt As Integer = 0 To arMach.Count - 1
            '    If cnt <> 0 Then
            '        strEmailId = strEmailId + ";" + arMach(cnt)
            '    Else
            '        strEmailId = arMach(cnt)
            '    End If
            'Next

            'ReadUserID(arUserID)
            'For cnt1 As Integer = 0 To arUserID.Count - 1
            '    If cnt1 <> 0 Then
            '        strUserID = strUserID + "," + arUserID(cnt1)
            '    Else
            '        strUserID = arUserID(cnt1)
            '    End If
            'Next



            'Session("UserID") = ""
            'Session("UserID") = strUserID

            'Session("SEmailId") = ""
            'Session("SEmailId") = strEmailId

            '---------------------------------------------


        Catch ex As Exception
            CreateLog("popSearch1", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
End Class
