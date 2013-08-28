Imports ION.Data
Imports System.Data
Imports ION.Logging.EventLogging
'Session Names Used on this Page are :-
'-------                     ---------'
'Session("PropUserName")
'Session("PropUserID")

Partial Class AdministrationCenter_UDC_CallTypeAccess
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        Try

            lstError.Items.Clear()
            'cpnlError.Visible = False
            If Not IsPostBack Then
                lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
                imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")

            End If

            Dim txthiddenImage As String = Request.Form("txthiddenImage")

            If txthiddenImage <> "" Then

                Select Case txthiddenImage
                    Case "Close"
                        Response.Redirect("../../Home.aspx", False)
                    Case "Save"
                        Call SaveCallType()

                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            End If

            Call BindCallTypeGrid()
            Call txtCSS(Me.Page)
            If Not IsPostBack Then

                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(969) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx", False)
                End If
                obj.ControlSecurity(Me.Page, 969)
            End If

        Catch ex As Exception
            CreateLog("CallTypeAccess", "PageLoad-81", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserName"), Session("PropUserID"))
        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

    Private Function BindCallTypeGrid() As Boolean
        Try

            Dim strSQL As String

            strSQL = "select Description CallTypeDesc, isnull(CI_VC36_Name,'') CompName, Company CompID,Name CallType, isnull(CT_BT1_CallEnteryFlag,0) CEFlag, isnull(CT_BT1_CallFastEnteryFlag,0) CFEFlag from UDC, T040103, T010011 where UDCType='Call' and CT_VC8_CallType_FK=*Name and  CI_NU8_Address_Number=*Company "

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            Dim dsCallType As New DataSet
            SQL.Search("T040103", "", "", strSQL, dsCallType, "", "")

            grdCallType.DataSource = dsCallType
            grdCallType.DataBind()

        Catch ex As Exception
            CreateLog("CallTypeAccess", "BindCallTypeGrid-88", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserName"), Session("PropUserID"))
        End Try
    End Function


    Private Function SaveCallType() As Boolean
        Try

            Dim arrColName As New ArrayList
            Dim arrRowData As New ArrayList


            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.Delete("UserManage", "SaveCompanyList", "delete from T040103", SQL.Transaction.Serializable)

            arrColName.Add("CT_NU9_CompID_FK")
            arrColName.Add("CT_VC8_CallType_FK")
            arrColName.Add("CT_BT1_CallEnteryFlag")
            arrColName.Add("CT_BT1_CallFastEnteryFlag")
            arrColName.Add("CT_IN4_InsertedBy_FK")
            arrColName.Add("CT_DT8_InsertedOnDate")
            arrColName.Add("CT_VC50_SystemIP")

            For Each DGI As DataGridItem In grdCallType.Items
                arrRowData = New ArrayList
                arrRowData.Add(Val(DGI.Cells(0).Text.Trim))
                arrRowData.Add(DGI.Cells(1).Text.Trim)
                arrRowData.Add(CType(DGI.FindControl("chkCEAccess"), CheckBox).Checked)
                arrRowData.Add(CType(DGI.FindControl("chkCFEAccess"), CheckBox).Checked)
                arrRowData.Add(Val(Session("PropUserID")))
                arrRowData.Add(Now)
                arrRowData.Add(Request.UserHostAddress)
                SQL.Save("T040103", "", "", arrColName, arrRowData)

            Next

            lstError.Items.Clear()
            lstError.Items.Add("Record Saved Successfully...")
            'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Record Not Saved, Please Try Later...")
            'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            CreateLog("UserManage", "SaveCompanyList-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function



    Private Sub grdCallType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCallType.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            e.Item.Attributes.Add("OnCLick", "KeyCheck('" & e.Item.ItemIndex + 1 & "')")
        End If
    End Sub
End Class
