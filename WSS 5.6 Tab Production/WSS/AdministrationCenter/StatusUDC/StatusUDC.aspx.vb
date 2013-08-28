'*********************************************************************************************************
' Page                     : - StatusUDC
' Purpose                : - Purpose of this screen is to show status name & its description Filtered by                                  
'                                   selected screen through Dropdown list  & company Selected.  
' Tables used          : - T040081, T010011, T040011, T040021
' Date					    Author				        		Modification Date					Description
' 05/02/06				Harpreet Singh				-------------------					Created
'
' Notes: 
' Code:
'*********************************************************************************************************
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class AdministrationCenter_StatusUDC_StatusUDC
    Inherits System.Web.UI.Page
    Public TXTb As New TextBox
    Public intNextNo As Integer
    Dim txthiddenImage As String
    Dim intRowCount As Integer
    Public dsStatus As DataSet
    Public arRowVal As New ArrayList
    Public arrG As New ArrayList
    Protected WithEvents txtDescription As New System.Web.UI.WebControls.TextBox

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        'Call txtCSS(Me.Page)
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")

            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return SaveEdit('Delete');")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgbtnSearch.Attributes.Add("Onclick", "return SaveEdit('Save');")

            'ddlScreen.Attributes.Add("onchange", "DLLChangeSCR('" & ddlScreen.ClientID & "','" & ddlCompany.ClientID & "');")
            'ddlCompany.Attributes.Add("onchange", "DLLChangeCOM('" & ddlScreen.ClientID & "','" & ddlCompany.ClientID & "');")
        End If
        cpnlError.Visible = False
        If Not IsPostBack Then
            Session("arRowVal") = Nothing
        End If

        txthiddenImage = Request.Form("txthiddenImage")
        If IsPostBack = False Then
            Call FillCompanyDDL()
            Call BindGrid()
        End If
        If Session("gridBindStatus") = 1 Then
            Call BindGrid()
            Session("gridBindStatus") = 0
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Edit"

                    Case "Delete"
                        Call DeleteUDC()
                        BindGrid()
                    Case "Save"
                        If CheckInput() = True Then
                            SaveStatusUDC(GetFooterValues())
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is unable to process your request please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                CreateLog("Task_View", "Load-386", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If
        Call FormatGridColumns()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' -- Security Block
        If Not IsPostBack Then
            Dim intId As Integer
            If 1 = 1 Then 'This is a fake block for executing security because visibility of controls is changing in programming 
                Dim str As String
                str = Session("PropRootDir")
                intId = 541
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intId) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intId)
            End If
        End If
        ' --End of Security Block------------------------------------------------------
    End Sub

    Public Function BindGrid()
        Dim blnStatus As Boolean

        Try
            intNextNo = SQL.Search("StatusUDC", "BindGrid", "select max(SU_NU9_ID_PK) from T040081")
            intNextNo = intNextNo + 1
            dsStatus = New DataSet
            If SQL.Search("T040081", "StatusUDC", "BindGrid-144", "select * from T040081 where (SU_NU9_CompID=0 or SU_NU9_CompID=" & Val(ddlCompany.SelectedValue) & ") and (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=" & Val(ddlScreen.SelectedValue) & ") order by SU_NU9_ID_PK ", dsStatus, "", "") = True Then
                grdStatusUDC.DataSource = dsStatus
                intRowCount = dsStatus.Tables(0).Rows.Count
                grdStatusUDC.DataBind()
                cpnlStatusUDCDetail.State = CustomControls.Web.PanelState.Expanded
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is unable to process your request please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            CreateLog("StatusUDC", "BindGrid-118", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
        End Try

    End Function

    Private Sub FormatGridColumns()
        grdStatusUDC.Columns(0).Visible = False
        grdStatusUDC.Columns(0).HeaderStyle.Width = Unit.Pixel(0)
        grdStatusUDC.Columns(0).FooterStyle.Width = Unit.Pixel(0)
        grdStatusUDC.Columns(0).ItemStyle.Width = Unit.Pixel(0)
        grdStatusUDC.Columns(1).ItemStyle.Width = Unit.Pixel(200)
        grdStatusUDC.Columns(2).ItemStyle.Width = Unit.Pixel(350)
        grdStatusUDC.Columns(3).ItemStyle.Width = Unit.Pixel(100)
        grdStatusUDC.Columns(0).ItemStyle.Wrap = True
        grdStatusUDC.Columns(1).ItemStyle.Wrap = True
        grdStatusUDC.Columns(2).ItemStyle.Wrap = True
        grdStatusUDC.Columns(3).ItemStyle.Wrap = True
        grdStatusUDC.Columns(0).ItemStyle.VerticalAlign = VerticalAlign.Top
        grdStatusUDC.Columns(1).ItemStyle.VerticalAlign = VerticalAlign.Top
        grdStatusUDC.Columns(2).ItemStyle.VerticalAlign = VerticalAlign.Top
        grdStatusUDC.Columns(3).ItemStyle.VerticalAlign = VerticalAlign.Top
    End Sub

    Private Function SaveStatusUDC(ByVal arRowVal As ArrayList) As Boolean
        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            lstError.Items.Clear()
            cpnlError.Text = "Message..."
            lstError.Items.Add("Your Role does not have rights to save Status UDC...")
            ' mshFlag = 0
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            Exit Function
        End If
        'End of Security Block


        Dim arCol As New ArrayList
        Dim arRow As New ArrayList
        intNextNo = SQL.Search("StatusUDC", "BindGrid", "select max(SU_NU9_ID_PK) from T040081")
        intNextNo = intNextNo + 1
        arCol.Add("SU_NU9_ID_PK")
        arCol.Add("SU_VC50_Status_Name")
        arCol.Add("SU_VC500_Status_Description")
        arCol.Add("SU_NU9_CompID")
        arCol.Add("SU_NU9_ScreenID")
        arCol.Add("SU_NU9_Status_Code")

        'arRow.Add(arRowVal.Item(0))
        arRow.Add(intNextNo)
        arRow.Add(Convert.ToString(arRowVal.Item(1)).ToUpper)
        arRow.Add(arRowVal.Item(2))
        arRow.Add(arRowVal.Item(3))
        arRow.Add(arRowVal.Item(4))
        arRow.Add(arRowVal.Item(5))

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            If SQL.Save("T040081", "StatusUDC", "SaveStatusUDC", arCol, arRow) = True Then
                Session("arRowVal") = Nothing
                Call BindGrid()
                lstError.Items.Clear()
                lstError.Items.Add("Record saved successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is unable to process your request please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is unable to process your request please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            CreateLog("StatusUDC", "SaveStatusUDC-118", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
        End Try

    End Function

    Private Sub grdStatusUDC_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdStatusUDC.ItemDataBound
        If e.Item.ItemIndex <> -1 Then

            For inti As Integer = 0 To e.Item.Cells.Count - 1
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    e.Item.Attributes.Add("style", "cursor:hand")
                    'e.Item.Cells(inti).ToolTip = dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item(inti)  'e.Item.Cells(inti).Text
                    e.Item.Attributes.Add("onclick", "KeyCheck('" & e.Item.ItemIndex + 1 & "'," & dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_NU9_Status_Code") & "," & dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_NU9_ID_PK") & ", '" & ddlCompany.SelectedValue & "', '" & ddlScreen.SelectedValue & "','" & ddlCompany.SelectedItem.Text & "','" & ddlScreen.SelectedItem.Text & "','" & dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_VC50_Status_Name") & "')")
                    e.Item.Attributes.Add("ondblclick", "KeyCheck55('" & dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item(0) & "', '" & ddlCompany.SelectedValue & "', '" & ddlScreen.SelectedValue & "','" & ddlCompany.SelectedItem.Text & "','" & ddlScreen.SelectedItem.Text & "')")
                End If
            Next
            e.Item.Cells(0).ToolTip = dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_NU9_ID_PK")
            e.Item.Cells(1).ToolTip = dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_VC50_Status_Name")
            e.Item.Cells(2).ToolTip = dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_VC500_Status_Description")
            e.Item.Cells(3).ToolTip = dsStatus.Tables(0).Rows(e.Item.ItemIndex).Item("SU_NU9_Status_Code")

            If e.Item.ItemIndex >= 0 And e.Item.ItemIndex < 4 Then
                e.Item.BackColor = System.Drawing.Color.FromArgb(215, 227, 243)
            End If
        End If
        Try
            Dim txt As TextBox
            txt = New TextBox
            txt = CType(e.Item.Cells(0).FindControl("txtSNo"), TextBox)
            If TypeOf txt Is TextBox Then
                txt.Text = intNextNo
            End If
            txt = New TextBox
            txt = CType(e.Item.Cells(0).FindControl("txtStatusName"), TextBox)
            If TypeOf txt Is TextBox Then
                If IsNothing(Session("arRowVal")) = False Then
                    txt.Text = CType(Session("arRowVal"), ArrayList).Item(1)
                End If
            End If
            txt = New TextBox
            txt = CType(e.Item.Cells(0).FindControl("txtDescription"), TextBox)
            If TypeOf txt Is TextBox Then
                If IsNothing(Session("arRowVal")) = False Then
                    txt.Text = CType(Session("arRowVal"), ArrayList).Item(2)
                End If
            End If
            txt = New TextBox
            txt = CType(e.Item.Cells(0).FindControl("txtStatusCode"), TextBox)
            If TypeOf txt Is TextBox Then
                If IsNothing(Session("arRowVal")) = False Then
                    txt.Text = CType(Session("arRowVal"), ArrayList).Item(5)
                End If
            End If

            txt = New TextBox
            txt = CType(e.Item.Cells(0).FindControl("txtStatusCode"), TextBox)
            If TypeOf txt Is TextBox Then
                txt.Attributes.Add("onkeypress", "NumericOnly();")
            End If
            txt = New TextBox
            txt = CType(e.Item.Cells(0).FindControl("txtDescription"), TextBox)
            If TypeOf txt Is TextBox Then
                txt.Attributes.Add("onkeypress", "MaxLength('" & txt.ClientID & "', 250);")
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is unable to process your request please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            CreateLog("StatusUDC", "SaveStatusUDC-118", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
        End Try
    End Sub
    Function GetFooterValues() As ArrayList
        Dim str As String
        str = TXTb.Text
        arRowVal = New ArrayList

        'Dim row As  GridViewRow = grdStatusUDC.h
        Dim txtstatusName As New TextBox()
        Dim txtSNo As New TextBox()
        Dim txtDescription As New TextBox()
        Dim txtStatusCode As New TextBox()
        txtSNo = CType(grdStatusUDC.Controls(0).Controls(grdStatusUDC.Controls(0).Controls.Count - 1).FindControl("txtSNo"), TextBox)
        txtstatusName = CType(grdStatusUDC.Controls(0).Controls(grdStatusUDC.Controls(0).Controls.Count - 1).FindControl("txtStatusName"), TextBox)
        txtDescription = CType(grdStatusUDC.Controls(0).Controls(grdStatusUDC.Controls(0).Controls.Count - 1).FindControl("txtDescription"), TextBox)
        txtStatusCode = CType(grdStatusUDC.Controls(0).Controls(grdStatusUDC.Controls(0).Controls.Count - 1).FindControl("txtStatusCode"), TextBox)
        arRowVal.Add(txtSNo.Text)
        arRowVal.Add(txtstatusName.Text)
        arRowVal.Add(txtDescription.Text)
        arRowVal.Add(ddlCompany.SelectedValue)
        arRowVal.Add(ddlScreen.SelectedValue)
        arRowVal.Add(txtStatusCode.Text)
        Session("arRowVal") = arRowVal
        Return arRowVal
    End Function

    Private Sub BtnGrdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click
        ' Dim arRow As New ArrayList
        If CheckInput() = True Then
            SaveStatusUDC(GetFooterValues())
        End If
    End Sub
    Public Function FillCompanyDDL()
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("StatusUDC", "FillCompanyDDL", "select CI_NU8_Address_number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type='COM'  and CI_VC8_Status='ENA' AND CI_NU8_Address_number IN (" & GetCompanySubQuery() & ")", SQL.CommandBehaviour.Default, blnStatus, "")
            If blnStatus = True Then
                ddlCompany.Items.Clear()
                ddlCompany.Items.Add(New ListItem("", "0"))
                While sqRDR.Read
                    ddlCompany.Items.Add(New ListItem(sqRDR("CI_VC36_Name"), sqRDR("CI_NU8_Address_number")))
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is unable to process your request please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            CreateLog("StatusUDC", "FillCompanyDDL-233", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
        End Try
    End Function
    Private Function CheckInput() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        Dim intCount As Integer = 0
        Dim arrTemp As New ArrayList
        arrTemp = GetFooterValues()
        If arrTemp.Item(1) = "" Then
            lstError.Items.Add("UDC Name cannot be blank...")
            shFlag = 1
            intCount += 1
        End If
        If arrTemp.Item(2) = "" Then
            lstError.Items.Add("UDC Description cannot be blank...")
            shFlag = 1
            intCount += 1
        End If
        If arrTemp.Item(5) = "" Then
            lstError.Items.Add("Status Code cannot be blank...")
            shFlag = 1
            intCount += 1
        End If
        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return False
        End If
        Try
            SQL.DBTracing = False
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim dsTemp As New DataSet
            Dim strSQL As String
            If ddlCompany.SelectedValue = 0 Then
                strSQL = "select * from T040081 where (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=" & ddlScreen.SelectedValue & ") and SU_VC50_Status_Name='" & GetFooterValues.Item(1) & "'"
            Else
                strSQL = "select * from T040081 where (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=" & ddlScreen.SelectedValue & ") and (SU_NU9_CompID=0 or SU_NU9_CompID=" & ddlCompany.SelectedValue & ") and SU_VC50_Status_Name='" & GetFooterValues.Item(1) & "'"
            End If
            If SQL.Search("T040081", "StatusUDC", "CheckInput", strSQL, dsTemp, "", "") = True Then
                lstError.Items.Add("This UDC is already used...")
                shFlag = 1
            End If
        Catch ex As Exception
            CreateLog("StatusUDC", "CheckInput-233", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
        End Try
        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        SaveStatusUDC(GetFooterValues())
    End Sub
    Private Function DeleteUDC() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim intRows As Integer = 0
            Dim blnFlag As Boolean = False

            If ddlScreen.SelectedValue = 0 Then
                SQL.Search("StatusUDC", "DeleteUDC", "select distinct(CN_VC20_Call_Status) from T040011 where CN_VC20_Call_Status='" & txthiddenStatusName.Value.Trim & "'", intRows)
                If intRows > 0 Then
                Else
                    SQL.Search("StatusUDC", "DeleteUDC", "select distinct(TM_VC50_Deve_Status) from T040021 where TM_VC50_Deve_Status='" & txthiddenStatusName.Value.Trim & "'", intRows)
                    If intRows > 0 Then
                    Else
                        blnFlag = True
                    End If
                End If
            ElseIf ddlScreen.SelectedValue = 3 Then
                SQL.Search("StatusUDC", "DeleteUDC", "select distinct(CN_VC20_Call_Status) from T040011 where CN_VC20_Call_Status='" & txthiddenStatusName.Value.Trim & "' and CM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue, intRows)
                If intRows > 0 Then
                Else
                    blnFlag = True
                End If
            ElseIf ddlScreen.SelectedValue = 464 Then
                SQL.Search("StatusUDC", "DeleteUDC", "select distinct(TM_VC50_Deve_Status) from T040021 where TM_VC50_Deve_Status='" & txthiddenStatusName.Value.Trim & "' and TM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue, intRows)
                If intRows > 0 Then
                Else
                    blnFlag = True
                End If
            End If
            If blnFlag = True Then
                Dim intID As Integer = Request.Form("txthidden")
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False
                If SQL.Delete("StatusUDC", "DeleteUDC", "delete from T040081 where SU_NU9_ID_PK=" & intID, SQL.Transaction.Serializable) = True Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Record deleted successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is unable to process your request please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("This Status UDC cannot be deleted as it is already used some where...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is unable to process your request please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            CreateLog("StatusUDC", "DeleteUDC-233", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
        End Try
    End Function
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

    Protected Sub ddlScreen_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScreen.SelectedIndexChanged
        BindGrid()
    End Sub

    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        BindGrid()
    End Sub
End Class
