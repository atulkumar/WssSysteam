#Region "Reffered Assemblies"
Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
#End Region
Imports System.Data

Partial Class DocumentsMgt_FolderPermissions
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOK.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        btnAdd.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        Try
            'Security Block
            Dim intID As Int32
            If Not IsPostBack Then
                intID = 1005
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intID) = False Then
                    Response.Redirect("../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intID)
            End If
            'End of Security Block

            Dim txthiddenvalue = Request.Form("txthiddenImage")
            Dim intFolderID As Integer = Session("FolderID")
            If Not IsPostBack Then
                If intFolderID <> 0 Then
                    txtFolderName.Text = getFolderName(intFolderID)
                End If
            Else
            End If
            If txthiddenvalue <> "" Then
                Select Case txthiddenvalue
                    Case "Save"
                        If gridPermissions.Items.Count <= 0 Then
                            BindPermissionTypeGrid()
                            lstError.Items.Clear()
                            lstError.Items.Add("There is No Role or Company in the List, Please Click on the Add Button to add ....")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            Exit Sub
                        Else
                            If Save() = True Then
                                BindPermissionTypeGrid()
                                lstError.Items.Clear()
                                lstError.Items.Add("Records Saved Successfully")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                Exit Sub
                            End If
                        End If
                    Case "Ok"
                        Call Save()
                        Response.Write("<script>window.close();self.opener.Form1.submit();</script>")
                    Case "Edit"
                        HIDPermID.Value = ViewState("PermID")
                    Case "Close"
                        Response.Write("<script>window.close();self.opener.Form1.submit();</script>")
                End Select
            End If
            BindPermissionsGrid()
            If txthiddenvalue <> "Edit" Then
                If IsPostBack Then
                    ViewState("PermID") = Val(Request.Form("HIDPermID"))
                End If
            End If
            BindPermissionTypeGrid()

        Catch ex As Exception
        End Try
    End Sub
    Private Function getFolderName(ByVal intFolderID As Integer) As String
        Try
            Dim dsfolderID As New DataSet
            Dim strSQL As String
            Dim intFolderName As String
            strSQL = "select FD_VC255_Folder_Name FolderName From T250021 where FD_NU9_Folder_ID_PK=" & intFolderID & ""
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("Call", "WSSReportsD", "ExtractCallNo", strSQL, dsfolderID, "Suresh", "Kharod")
            intFolderName = dsfolderID.Tables(0).Rows(0).Item(0)
            Return intFolderName
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Private Function BindPermissionTypeGrid() As Boolean
        Try
            Dim dsPermType As New DataSet
            Dim strSQL As String

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            strSQL = "select PT_NU9_Permission_Type_ID_PK PermTypeID, PT_VC100_Permission_Name PermissionType,0 as Allow,1 as PDeny From T250031 ; Select PD_NU9_Permission_Type_ID_FK PermissionTypeID,PD_BT1_Is_Allowed Allow From T250042 where PD_NU9_Permission_ID_FK=" & Val(ViewState("PermID")) & ""
            SQL.Search("Call", "WSSReportsD", "ExtractCallNo", strSQL, dsPermType, "Suresh", "Kharod")
            If dsPermType.Tables(1).Rows.Count > 0 Then
                For intI As Integer = 0 To dsPermType.Tables(1).Rows.Count - 1
                    For intJ As Integer = 0 To dsPermType.Tables(0).Rows.Count - 1
                        If dsPermType.Tables(1).Rows(intI).Item("PermissionTypeID") = dsPermType.Tables(0).Rows(intJ).Item("PermTypeID") Then
                            If dsPermType.Tables(1).Rows(intI).Item("Allow") = True Then
                                dsPermType.Tables(0).Rows(intJ).Item("Allow") = dsPermType.Tables(1).Rows(intI).Item("Allow")
                                dsPermType.Tables(0).Rows(intJ).Item("PDeny") = 0
                            End If
                        End If
                    Next
                Next
            End If
            gridPermType.DataSource = dsPermType.Tables(0)
            gridPermType.DataBind()
        Catch ex As Exception

        End Try

    End Function
    Private Function BindPermissionsGrid() As Boolean
        Try
            Dim dsPermissions As New DataSet
            Dim FolderID As Integer
            FolderID = Session("FolderID")
            Dim strSQL As String
            strSQL = "select PM_NU9_Permission_ID_PK PermID, ROM_VC50_Role_Name AssignTo,CI_VC36_Name CompanyName,'../Images/Role11.ico'as RImage From T250041,T070031,T010011 where ROM_IN4_Role_ID_PK=PM_IN4_PermissionTo_Role_ID_FK and ROM_IN4_Company_ID_FK=CI_NU8_Address_Number and PM_NU9_Folder_ID_FK=" & FolderID & " ; select  PM_NU9_Permission_ID_PK PermID,CI_VC36_Name AssignTo,'../Images/comp1.jpg'as RImage From T250041,T010011 where CI_NU8_Address_Number=PM_NU9_PermissionTo_Company_ID_FK  and PM_NU9_Folder_ID_FK=" & FolderID & ""
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("Call", "WSSReportsD", "ExtractCallNo", strSQL, dsPermissions, "Suresh", "Kharod")
            dsPermissions.Tables.Add("Permissions")

            dsPermissions.Tables("Permissions").Columns.Add("PermID", System.Type.GetType("System.Int32"))
            dsPermissions.Tables("Permissions").Columns.Add("Image", System.Type.GetType("System.String"))
            dsPermissions.Tables("Permissions").Columns.Add("AssignTO", System.Type.GetType("System.String"))
            Dim dr As DataRow
            For intI As Integer = 0 To dsPermissions.Tables(0).Rows.Count - 1
                dr = dsPermissions.Tables("Permissions").NewRow
                dr.Item("PermID") = dsPermissions.Tables(0).Rows(intI).Item("PermID")
                dr.Item("AssignTO") = "" & dsPermissions.Tables(0).Rows(intI).Item("AssignTo") & " [ " & dsPermissions.Tables(0).Rows(intI).Item("CompanyName") & " ]"
                dr.Item("Image") = dsPermissions.Tables(0).Rows(intI).Item("RImage")
                dsPermissions.Tables("Permissions").Rows.Add(dr)
                dsPermissions.Tables("Permissions").AcceptChanges()
            Next
            For intI As Integer = 0 To dsPermissions.Tables(1).Rows.Count - 1
                dr = dsPermissions.Tables("Permissions").NewRow
                dr.Item("PermID") = dsPermissions.Tables(1).Rows(intI).Item("PermID")
                dr.Item("AssignTO") = dsPermissions.Tables(1).Rows(intI).Item("AssignTo")
                dr.Item("Image") = dsPermissions.Tables(1).Rows(intI).Item("RImage")
                dsPermissions.Tables("Permissions").Rows.Add(dr)
                dsPermissions.Tables("Permissions").AcceptChanges()
            Next
            gridPermissions.DataSource = dsPermissions.Tables(2)
            gridPermissions.DataBind()

            If gridPermissions.Items.Count > 0 Then
                gridPermissions.SelectedIndex = 0
                ViewState("PermID") = gridPermissions.DataKeys(0)
            End If
        Catch ex As Exception

        End Try

    End Function

    Private Function Save() As Boolean
        Try
            '*******************************************
            'Name:      Save Function To Save Roles and Companies
            'Purpose:  -------
            'Author:    Suresh Kharod
            'Date:        Feb 11, 2008
            'Called by:  Any
            'Calls:     
            'Inputs:     
            'Output:    
            '*******************************************
            If ViewState("PermID") = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Role or Company or Both....")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Exit Function
            End If
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim dtSave As New DataTable
            dtSave.Columns.Add("PermissionID", System.Type.GetType("System.Int32"))
            dtSave.Columns.Add("PermissionTypeID", System.Type.GetType("System.Int32"))
            dtSave.Columns.Add("IsAllowed", System.Type.GetType("System.Int32"))
            Dim dr As DataRow
            Dim HaveAccess As Boolean = False
            For intI As Integer = 0 To gridPermType.Items.Count - 1
                dr = dtSave.NewRow
                dr.Item("PermissionID") = ViewState("PermID")
                dr.Item("PermissionTypeID") = gridPermType.DataKeys.Item(intI)
                dr.Item("IsAllowed") = CType(gridPermType.Items(intI).FindControl("chkAllow"), CheckBox).Checked
                If dr.Item("IsAllowed") = 1 Then
                    HaveAccess = True
                End If
                dtSave.Rows.Add(dr)
                dtSave.AcceptChanges()
            Next
            If dtSave.Rows.Count > 0 Then
                Dim strDelete As String
                Dim strUpdate As String

                If HaveAccess = True Then
                    strUpdate = "Update T250041 set PM_BT1_Have_Access=1 where PM_NU9_Permission_ID_PK=" & ViewState("PermID") & ""
                Else
                    strUpdate = "Update T250041 set PM_BT1_Have_Access=0 where PM_NU9_Permission_ID_PK=" & ViewState("PermID") & ""
                End If
                SQL.Update("FolderPermissions", "FolderPermissions", strUpdate, SQL.Transaction.Serializable, "")
                strDelete = "delete from T250042 where PD_NU9_Permission_ID_FK=" & ViewState("PermID") & ""
                If SQL.Delete("FolderPermissions", " FolderPermissions", strDelete, SQL.Transaction.Serializable, "") = True Then

                    For intI As Integer = 0 To dtSave.Rows.Count - 1
                        Dim arColName As New ArrayList
                        Dim arRowData As New ArrayList
                        'define column name
                        arColName.Add("PD_NU9_Permission_ID_FK") 'Item_Ledger_ID
                        arColName.Add("PD_NU9_Permission_Type_ID_FK") 'Item_Status
                        arColName.Add("PD_BT1_Is_Allowed") 'Assign To Company
                        'arColName.Add("PM_BT1_Have_Access") 'CompanyID
                        arColName.Add("PD_DT8_Created_ON")
                        arColName.Add("PD_DT8_Modify_ON")
                        arColName.Add("PD_VC32_Created_From_IP")
                        arColName.Add("PD_VC32_Modify_From_IP")
                        arColName.Add("PD_NU9_CreatedBy_ID_FK")
                        arColName.Add("PD_NU9_ModifyBy_ID_FK")

                        arRowData.Add(dtSave.Rows(intI).Item(0))
                        arRowData.Add(dtSave.Rows(intI).Item(1))
                        arRowData.Add(dtSave.Rows(intI).Item(2))
                        arRowData.Add(Now)
                        arRowData.Add(Now)
                        arRowData.Add(Request.UserHostAddress)
                        arRowData.Add(Request.UserHostAddress)
                        arRowData.Add(Val(Session("PropUserID")))
                        arRowData.Add(Val(Session("PropUserID")))


                        If SQL.Save("T250042", " ItemTransaction", "SaveRecord", arColName, arRowData, "") = True Then
                        Else
                            Return False
                        End If
                    Next
                End If
            End If
            Return True
        Catch ex As Exception
            CreateLog("FolderPermissions", "Save", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function

    Private Sub gridPermType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles gridPermType.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
                'Modified by atul so that when we ckeck one chkbox the other will automatically uncheck
                'CType(e.Item.Cells(0).FindControl("chkAllow"), CheckBox).Attributes.Add("Onclick", "checkAllow('gridPermType:_ctl" & e.Item.ItemIndex + 2 & ":chkAllow','gridPermType:_ctl" & e.Item.ItemIndex + 2 & ":chkDeny');")
                'CType(e.Item.Cells(0).FindControl("chkDeny"), CheckBox).Attributes.Add("Onclick", "checkDeny('gridPermType:_ctl" & e.Item.ItemIndex + 2 & ":chkAllow','gridPermType:_ctl" & e.Item.ItemIndex + 2 & ":chkDeny');")
                Dim chkAllow As CheckBox = CType(e.Item.Cells(0).FindControl("chkAllow"), CheckBox)
                Dim chkDeny As CheckBox = CType(e.Item.Cells(0).FindControl("chkDeny"), CheckBox)
                CType(e.Item.Cells(0).FindControl("chkAllow"), CheckBox).Attributes.Add("Onclick", "checkAllow('" & chkAllow.ClientID & "','" & chkDeny.ClientID & "');")
                CType(e.Item.Cells(0).FindControl("chkDeny"), CheckBox).Attributes.Add("Onclick", "checkDeny('" & chkAllow.ClientID & "','" & chkDeny.ClientID & "');")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub gridPermissions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles gridPermissions.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
                For intI As Integer = 1 To e.Item.Cells.Count - 1
                    e.Item.Cells(intI).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(intI).Attributes.Add("onmousedown", "javascript:setTimeout(""__doPostBack('gridPermissions$_ctl" & e.Item.ItemIndex + 2 & "$_ctl0','')"",200)")
                    e.Item.Cells(intI).Attributes.Add("OnClick", "KeyCheck('" & gridPermissions.DataKeys(e.Item.ItemIndex) & "')")

                    'Modified by atul to show selected row color
                    '///////////////////////////////////////////
                    Dim selectedrow As String = Val(Request.Form("HIDPermID")).ToString()
                    If selectedrow > 0 Then
                        If selectedrow = gridPermissions.DataKeys(e.Item.ItemIndex).ToString Then
                            e.Item.Cells(intI).BackColor = Drawing.ColorTranslator.FromHtml("#dcdcdc")
                        End If
                    Else
                        If e.Item.ItemIndex = 0 Then
                            e.Item.Cells(intI).BackColor = Drawing.ColorTranslator.FromHtml("#dcdcdc")
                        End If
                    End If
                    '///////////////////////////////////////////
                    
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If RemoveRoleOrCompany() = True Then
            BindPermissionsGrid()
            BindPermissionTypeGrid()
        End If
    End Sub

    Private Function RemoveRoleOrCompany() As Boolean
        Try
            '-- Delete role permissions
            If SQL.Delete("FolderPermissions", "RemoveRoleOrCompany", "Delete from T250042 Where PD_NU9_Permission_ID_FK=" & HIDPermID.Value, SQL.Transaction.ReadCommitted) = True Then
                'delete role if all permissions get deleted successfully
                SQL.Delete("FolderPermissions", "RemoveRoleOrCompany", "Delete from T250041 Where PM_NU9_Permission_ID_PK=" & HIDPermID.Value, SQL.Transaction.ReadCommitted)
            End If
            Return True
        Catch ex As Exception
            Return False
            CreateLog("FolderPermissions", "RemoveRoleOrCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
End Class
