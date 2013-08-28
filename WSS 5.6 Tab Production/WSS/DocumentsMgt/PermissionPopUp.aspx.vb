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


Partial Class DocumentsMgt_PermissionPopUp
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgOK.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        Dim txthiddenvalue = Request.Form("txthiddenImage")

        'Security Block
        Dim intID As Int32
        If Not IsPostBack Then
            intID = 972
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block

        If Not IsPostBack Then
            getRolesCompanies()
        End If
        If txthiddenvalue <> "" Then
            Select Case txthiddenvalue
                Case "Ok"
                    Call Save()
                    Response.Write("<script>window.close();self.opener.Form1.submit();</script>")
            End Select
        End If
    End Sub
    Private Sub getRolesCompanies()
        Try
            Dim dsPermissions As New DataSet
            Dim FolderID As Integer
            FolderID = Session("FolderID")
            Dim strSQL As String
            If Session("PropCompanyType") = "SCM" Then
                strSQL = "Select * From (Select ROM_IN4_Role_ID_PK RoleID,CI_VC36_Name CompanyName,'Role' as Type, ROM_VC50_Role_Name AssignTo,'../Images/Role11.ico'as RImage From T070031,T010011 Where ROM_IN4_Company_ID_FK IN(select UC_NU9_Comp_ID_FK ID from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1) and ROM_IN4_Company_ID_FK=CI_NU8_Address_Number ) b where RoleID not IN(select PM_IN4_PermissionTo_Role_ID_FK RoleID From T250041 where PM_VC16_Permission_To='Role' and PM_NU9_Folder_ID_FK=" & FolderID & ") ;select * From (select UC_NU9_Comp_ID_FK CompID,CI_VC36_Name as AssignTo,'../Images/comp1.jpg'as RImage,'Company' as Type  from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1) a where CompID Not IN (select PM_NU9_PermissionTo_Company_ID_FK From T250041 where PM_VC16_Permission_To='Company'  and PM_NU9_Folder_ID_FK=" & FolderID & ")"

            ElseIf Session("PropCompanyType") <> "SCM" Then

                strSQL = "(Select * From (Select ROM_IN4_Role_ID_PK RoleID,CI_VC36_Name CompanyName,'Role' as Type, ROM_VC50_Role_Name AssignTo,'../Images/Role11.ico'as RImage From T070031,T010011 Where ROM_IN4_Company_ID_FK IN(select UC_NU9_Comp_ID_FK ID from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1) and ROM_IN4_Company_ID_FK=CI_NU8_Address_Number ) b where RoleID not IN(select PM_IN4_PermissionTo_Role_ID_FK RoleID From T250041 where PM_VC16_Permission_To='Role' and PM_NU9_Folder_ID_FK=" & FolderID & ") )union(Select * From(Select ROM_IN4_Role_ID_PK RoleID,CI_VC36_Name CompanyName,'Role' as Type, ROM_VC50_Role_Name AssignTo,'../Images/Role11.ico'as RImage From T070031,T010011 Where  ROM_IN4_Company_ID_FK=CI_NU8_Address_Number and ROM_IN4_Company_ID_FK IN (Select CI_NU8_Address_Number  CompID From T010011 where CI_VC8_Address_Book_Type='COM' and CI_IN4_Business_Relation='SCM'))c where RoleID not IN (select PM_IN4_PermissionTo_Role_ID_FK RoleID From T250041 where PM_VC16_Permission_To='Role' and PM_NU9_Folder_ID_FK=" & FolderID & ")) ;(select * From (select UC_NU9_Comp_ID_FK CompID,CI_VC36_Name as AssignTo,'../Images/comp1.jpg'as RImage,'Company' as Type  from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & "  and UC_BT1_Access=1) a where CompID Not IN (select PM_NU9_PermissionTo_Company_ID_FK From T250041 where PM_VC16_Permission_To='Company'  and PM_NU9_Folder_ID_FK=" & FolderID & "))union(Select * From (Select CI_NU8_Address_Number  CompID,CI_VC36_Name as AssignTo,'../Images/comp1.jpg'as RImage,'Company' as Type From T010011 where CI_VC8_Address_Book_Type='COM' and CI_IN4_Business_Relation='SCM')d where CompID Not IN (select PM_NU9_PermissionTo_Company_ID_FK From T250041 where PM_VC16_Permission_To='Company'  and PM_NU9_Folder_ID_FK=" & FolderID & "))"

            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("Call", "WSSReportsD", "ExtractCallNo", strSQL, dsPermissions, "Suresh", "Kharod")
            dsPermissions.Tables.Add("Permissions")
            dsPermissions.Tables("Permissions").Columns.Add("DataKey", System.Type.GetType("System.Int32"))
            dsPermissions.Tables("Permissions").Columns.Add("ID", System.Type.GetType("System.Int32"))
            dsPermissions.Tables("Permissions").Columns.Add("Image", System.Type.GetType("System.String"))
            dsPermissions.Tables("Permissions").Columns.Add("AssignTO", System.Type.GetType("System.String"))
            dsPermissions.Tables("Permissions").Columns.Add("Type", System.Type.GetType("System.String"))
            Dim dr As DataRow
            Dim intDataKay As Integer
            For intI As Integer = 0 To dsPermissions.Tables(0).Rows.Count - 1
                dr = dsPermissions.Tables("Permissions").NewRow
                dr.Item("DataKey") = inti + 1
                dr.Item("ID") = dsPermissions.Tables(0).Rows(intI).Item("RoleID")
                dr.Item("AssignTO") = "" & dsPermissions.Tables(0).Rows(intI).Item("AssignTo") & " [ " & dsPermissions.Tables(0).Rows(intI).Item("CompanyName") & " ]"
                dr.Item("Image") = dsPermissions.Tables(0).Rows(intI).Item("RImage")
                dr.Item("Type") = dsPermissions.Tables(0).Rows(intI).Item("Type")
                dsPermissions.Tables("Permissions").Rows.Add(dr)
                dsPermissions.Tables("Permissions").AcceptChanges()
            Next
            If dsPermissions.Tables("Permissions").Rows.Count <> 0 Then
                intDataKay = dsPermissions.Tables("Permissions").Rows.Count
            Else
                intDataKay = 1
            End If
            For intI As Integer = 0 To dsPermissions.Tables(1).Rows.Count - 1
                dr = dsPermissions.Tables("Permissions").NewRow
                dr.Item("DataKey") = intDataKay + inti
                dr.Item("ID") = dsPermissions.Tables(1).Rows(intI).Item("CompID")
                dr.Item("AssignTO") = dsPermissions.Tables(1).Rows(intI).Item("AssignTo")
                dr.Item("Image") = dsPermissions.Tables(1).Rows(intI).Item("RImage")
                dr.Item("Type") = dsPermissions.Tables(1).Rows(intI).Item("Type")
                dsPermissions.Tables("Permissions").Rows.Add(dr)
                dsPermissions.Tables("Permissions").AcceptChanges()
            Next
            gridRoles.DataSource = dsPermissions.Tables(2)
            gridRoles.DataBind()
        Catch ex As Exception

        End Try

    End Sub
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
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString

            Dim dtSave As New DataTable
            dtSave.Columns.Add("FolderID", System.Type.GetType("System.Int32"))
            dtSave.Columns.Add("PermissionType", System.Type.GetType("System.String"))
            dtSave.Columns.Add("PermissionTo", System.Type.GetType("System.Int32"))
            Dim dr As DataRow
            For intI As Integer = 0 To gridRoles.Items.Count - 1
                If CType(gridRoles.Items(intI).FindControl("chkAllow"), CheckBox).Checked Then
                    dr = dtSave.NewRow
                    dr.Item("FolderID") = Session("FolderID")
                    dr.Item("PermissionType") = CType(gridRoles.Items(intI).FindControl("lblType"), Label).Text.Trim
                    dr.Item("PermissionTo") = CType(gridRoles.Items(intI).FindControl("lblID"), Label).Text.Trim
                    dtSave.Rows.Add(dr)
                    dtSave.AcceptChanges()
                End If
            Next
            If dtSave.Rows.Count > 0 Then
                For intI As Integer = 0 To dtSave.Rows.Count - 1
                    Dim arColName As New ArrayList
                    Dim arRowData As New ArrayList
                    'define column name
                    arColName.Add("PM_NU9_Folder_ID_FK") 'Item_Ledger_ID
                    arColName.Add("PM_VC16_Permission_To") 'Item_Status
                    If dtSave.Rows(intI).Item("PermissionType") = "Role" Then
                        arColName.Add("PM_IN4_PermissionTo_Role_ID_FK") 'Assign To Role
                    ElseIf dtSave.Rows(intI).Item("PermissionType") = "Company" Then
                        arColName.Add("PM_NU9_PermissionTo_Company_ID_FK") 'Assign To Company
                    End If
                    arColName.Add("PM_BT1_Have_Access") 'CompanyID
                    arColName.Add("PM_DT8_Created_ON")
                    arColName.Add("PM_DT8_Modify_ON")
                    arColName.Add("PM_VC32_Created_From_IP")
                    arColName.Add("PM_VC32_Modify_From_IP")
                    arColName.Add("PM_NU9_CreatedBy_ID_FK")
                    arColName.Add("PM_NU9_ModifyBy_ID_FK")

                    arRowData.Add(dtSave.Rows(intI).Item(0))
                    arRowData.Add(dtSave.Rows(intI).Item(1))
                    arRowData.Add(dtSave.Rows(intI).Item(2))
                    arRowData.Add(0)
                    arRowData.Add(Now)
                    arRowData.Add(Now)
                    arRowData.Add(Request.UserHostAddress)
                    arRowData.Add(Request.UserHostAddress)
                    arRowData.Add(Val(Session("PropUserID")))
                    arRowData.Add(Val(Session("PropUserID")))
                    If SQL.Save("T250041", "ItemTransaction", "SaveRecord", arColName, arRowData, "") = True Then
                    Else
                        Return False
                    End If
                Next
                Return True
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please Select a record....")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Exit Function
            End If
        Catch ex As Exception
            CreateLog("PermissionPopUp", "Save", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
End Class
