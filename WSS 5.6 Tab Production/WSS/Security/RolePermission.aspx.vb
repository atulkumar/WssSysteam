#Region "Purpose"
'*******************************************************************
' Page                   : -Security
' Purpose                :- This screen is used to give access to role for various Menus, screens & controls.                            User will view only those screen, control & menus that have choosen for view                                 through this screen
' Date		    		Author						Modification Date					Description
' 17/03/06				Jagtar 					    06/03/2006        					Created
'
' Notes: 
' Code:
'*******************************************************************
#End Region

#Region "Session Used"
'Session("PropUserID")
'Session("PropUserName")
'Session("RoleCreatedDate") 
'Session("RoleEditStartDate")
'Session("PropRootDir")
'Session("propCompanyType")
'ViewState("RoleKey")
#End Region

#Region "NameSpace"
Imports ION.Data
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports Microsoft.Web.UI.WebControls
Imports System.Text.StringBuilder
Imports System.Drawing
#End Region

Partial Class Security_RolePermission
    Inherits System.Web.UI.Page
    Dim strValue As String
    Dim strConn As String
    Private Shared KeyStatus As Int16
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        Dim a As String
        a = cpnlTaskAction.State
        a = cpnlCallTask.State
        a = cpnlCallView.State
        a = Collapsiblepanel1.State
        If Not IsPostBack Then
            FillNonUDCDropDown(DDLCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from   T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' AND CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ")", True)
            FillNonUDCDropDown(DDLRole, "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name + ' [' + CI_VC36_Name + ']' as RoleName from T070031, T010011 where  CI_NU8_Address_Number=ROM_IN4_Company_ID_FK and ROM_IN4_Company_ID_FK IN (" & GetCompanySubQuery() & ")", True)
            Session("RoleCreatedDate") = ""
            Session("RoleEditStartDate") = ""
        End If
        ViewState("RoleKey") = Request.QueryString("RoleKey")
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        lstError.Items.Clear()
        Dim txthiddenImage = Request.Form("txthiddenImage")
        If txtCheckStatus.Value <> "" Then
            Try
                Select Case txtCheckStatus.Value
                    Case "1"
                        If DoValidate(1, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Menu name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Else
                            SaveMenuAll()
                        End If
                    Case "2"
                        If DoValidate(2, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Screen name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Else
                            SaveScreenAll()
                        End If
                    Case "3"
                        If DoValidate(3, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Control name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(3, 2) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Control name cannot be duplicate...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Else
                            SaveControlAll()
                        End If
                        txtCheckStatus.Value = ""
                End Select
            Catch ex As Exception
                CreateLog("RolePermission", "Load-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Edit"
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If DoValidate(1, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Menu name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(2, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Screen name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(3, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Control name cannot be blank")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(3, 2) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Control name cannot be duplicate...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Else
                            If SaveRoleData() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            End If
                        End If
                    Case "Close"
                        'Response.Redirect("Role_Search.aspx?ScrID=64", False)
                    Case "Logout"
                        LogoutWSS()
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If DoValidate(1, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Menu name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(2, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Screen name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(3, 1) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Control name cannot be blank...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        ElseIf DoValidate(3, 2) = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Control name cannot be duplicate...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Else
                            If SaveRoleData() = True Then
                                'Security Block
                                Dim objCls As New clsSecurityCache
                                If HttpContext.Current.Session("PropUserName") = "" Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("User name does not initialized...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                                Else
                                    objCls.FillCache()
                                End If
                                'End of Security Block
                                Response.Redirect("Role_Search.aspx?ScrID=64", False)
                            End If
                        End If
                End Select
            Catch ex As Exception
                CreateLog("RolePermission", "Load-160", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If

        If Not IsPostBack Then
            KeyStatus = Request.QueryString("ID")
            FillMenuGrid()
            If KeyStatus = 1 Then
                cpnlCallTask.Visible = False
                cpnlTaskAction.Visible = False
                Collapsiblepanel1.Visible = False
                dtStartDate.Text = SetDateFormat(Now, mdlMain.IsTime.DateOnly)
                dtEndDate.Text = SetDateFormat(Now.Date.AddMonths(12), mdlMain.IsTime.DateOnly)
                dtStatusDate.Text = SetDateFormat(Now, mdlMain.IsTime.DateOnly)
            End If
        End If
        If Not IsPostBack() Then
            If KeyStatus = -1 Then
                cpnlCallTask.Visible = True
                cpnlCallTask.Visible = True
                cpnlTaskAction.Visible = True
                ShowValues()
            End If
        End If
        If Not IsPostBack() Then
            txtCSS(Me.Page)
            Session("RoleEditStartDate") = dtStartDate.Text
        End If
        If cboEnabeDisable.SelectedValue = "DSB" Then
            txtRollName.Enabled = False
            txtParentRole.Visible = False
            txtDescription.Enabled = False
            txtAliasName.Enabled = False
        Else
            txtAliasName.Enabled = True
            txtRollName.Enabled = True
            txtParentRole.Visible = True
            txtDescription.Enabled = True
        End If
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        txtRollName.Attributes.Add("OnChange", "CopyText();")

        'Security Block
        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block
        If Session("propCompanyType") = "SCM" Then
            '    ImgCoID.Visible = True
        Else
            '  ImgCoID.Visible = False
            'txtCompanyID.Text = Session("PropCompanyID")
        End If

    End Sub
    Private Function DoValidate(ByVal intMode As Int16, ByVal intValidate As Int16) As Boolean
        Dim gridRow As DataGridItem
        Dim strAliasName As String
        If intValidate = 1 Then
            If intMode = 1 Then
                For Each gridRow In dgMenu.Items
                    strAliasName = CType(gridRow.FindControl("txtAliasMenu"), TextBox).Text
                    If strAliasName = "" Then
                        Return False
                    End If
                Next
                Return True
            ElseIf intMode = 2 Then
                For Each gridRow In dgScreen.Items
                    strAliasName = CType(gridRow.FindControl("txtAliasScreen"), TextBox).Text
                    If strAliasName = "" Then
                        Return False
                    End If
                Next
                Return True
            ElseIf intMode = 3 Then
                For Each gridRow In dgControls.Items
                    strAliasName = CType(gridRow.FindControl("txtAliasControl"), TextBox).Text
                    If strAliasName = "" Then
                        Return False
                    End If
                Next
                Return True
            End If
        ElseIf intValidate = 2 Then
            Dim strAliasName1 As String
            Dim gridRow1 As DataGridItem
            Dim intCount As Int16
            For Each gridRow In dgControls.Items
                intCount = 0
                strAliasName = CType(gridRow.FindControl("txtAliasControl"), TextBox).Text
                For Each gridRow1 In dgControls.Items
                    strAliasName1 = CType(gridRow1.FindControl("txtAliasControl"), TextBox).Text
                    If strAliasName = strAliasName1 Then
                        intCount = intCount + 1
                        If intCount > 1 Then
                            Return False
                        End If
                    End If
                Next
            Next
            Return True
        End If
    End Function
    Private Sub SaveMenuAll()
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim gridRow As DataGridItem
        Dim strAliasName As String
        Dim strVH, strED As String
        Dim intRoleID, intMenuID As Integer
        If cboEnabeDisable.SelectedValue = "DSB" Then
            Exit Sub
        End If

        intRoleID = Request.QueryString("RoleKey")
        ViewState("RoleKey") = Request.QueryString("RoleKey") 'Val(ViewState("RoleKey"))
        objConn = New SqlConnection(strConn)
        objConn.Open()

        Try

            For Each gridRow In dgMenu.Items
                strAliasName = CType(gridRow.FindControl("txtAliasMenu"), TextBox).Text
                If CType(gridRow.FindControl("rdView"), RadioButton).Checked = False And CType(gridRow.FindControl("rdHide"), RadioButton).Checked = False Then
                    strVH = "V"
                Else
                    If CType(gridRow.FindControl("rdView"), RadioButton).Checked = True Then
                        strVH = "V"
                    Else
                        strVH = "H"
                    End If
                End If
                intMenuID = Val(gridRow.Cells(1).Text)
                If IsExisted(intRoleID, intMenuID) = True Then
                    strQuery = "Update T070042 set ROD_VC50_Alias_Name='" & strAliasName & "'," _
                      & "ROD_CH1_View_Hide='" & strVH & "'," & "ROD_CH1_Enable_Disable='Z'," _
                      & "ROD_CH1_isLast_Level='N', ROD_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                      & "ROD_DT8_Last_Modified_Date='" & Now.Date _
                      & "' where ROD_IN4_Object_ID_FK=" & intMenuID _
                       & " and ROD_IN4_Role_ID_FK=" & intRoleID
                Else
                    strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                     & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                     & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                     & intRoleID & "," & intMenuID & ",'" & strAliasName & "','" & strVH _
                     & "','Z','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"

                End If

                With cmdCommand
                    .CommandText = strQuery
                    .CommandType = CommandType.Text
                    .Connection = objConn
                    .ExecuteNonQuery()
                End With
            Next
            txtCheckStatus.Value = ""
        Catch ex As Exception
            CreateLog("RolePermission", "SaveMenuALL-295", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        Finally
            objConn.Close()
        End Try
    End Sub
    Private Sub SaveScreenAll()
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim gridRow As DataGridItem
        Dim strAliasName As String
        Dim strVH, strED As String
        Dim intRoleID, intMenuID As Integer
        If cboEnabeDisable.SelectedValue = "DSB" Then
            Exit Sub
        End If

        intRoleID = Val(ViewState("RoleKey"))
        objConn = New SqlConnection(strConn)
        objConn.Open()

        Try

            For Each gridRow In dgScreen.Items
                strAliasName = CType(gridRow.FindControl("txtAliasScreen"), TextBox).Text
                If CType(gridRow.FindControl("rdView1"), RadioButton).Checked = False And CType(gridRow.FindControl("rdHide1"), RadioButton).Checked = False Then
                    strVH = "V"
                Else
                    If CType(gridRow.FindControl("rdView1"), RadioButton).Checked = True Then
                        strVH = "V"
                    Else
                        strVH = "H"
                    End If
                End If
                intMenuID = Val(gridRow.Cells(1).Text)
                If IsExisted(intRoleID, intMenuID) = True Then
                    strQuery = "Update T070042 set ROD_VC50_Alias_Name='" & strAliasName & "'," _
                      & "ROD_CH1_View_Hide='" & strVH & "'," & "ROD_CH1_Enable_Disable='Z'," _
                      & "ROD_CH1_isLast_Level='N', ROD_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                      & "ROD_DT8_Last_Modified_Date='" & Now.Date _
                      & "' where ROD_IN4_Object_ID_FK=" & intMenuID _
                       & " and ROD_IN4_Role_ID_FK=" & intRoleID
                Else
                    strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                     & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                     & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                     & intRoleID & "," & intMenuID & ",'" & strAliasName & "','" & strVH _
                     & "','Z','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"

                End If

                With cmdCommand
                    .CommandText = strQuery
                    .CommandType = CommandType.Text
                    .Connection = objConn
                    .ExecuteNonQuery()
                End With
            Next
            txtCheckStatus.Value = ""
        Catch ex As Exception
            CreateLog("RolePermission", "SaveScreenAll-366", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objConn.Close()
        End Try
    End Sub
    Private Sub ShowValues()
        Dim DT As New DataTable
        Dim dtChq As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String = String.Empty
        Dim objCommand As SqlCommand
        Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            objCon.Open()
            QueryString = "Select ROM_VC50_Role_Name,ROM_IN4_Company_ID_FK,CI_VC36_Name," _
              & "ROM_IN4_BusinessUnit_ID_FK,ROM_VC50_Descr,ROM_VC50_Alias_Name,ROM_VC50_Parent_Role,ROM_IN4_Copy_From," _
              & "ROM_DT8_Start_Date,ROM_DT8_End_Date,ROM_VC50_Status_Code_FK,ROM_DT8_Status_Date,isnull(ROM_CH1_IsAdminRight,0) as  ROM_CH1_IsAdminRight" _
              & " from T010011, T070031 where ROM_IN4_Role_ID_PK=" & Val(ViewState("RoleKey")) _
              & " and CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK"
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)

            If DT.Rows.Count > 0 Then
                DDLCompany.Enabled = False
                DDLRole.Enabled = False
                txtRollName.ReadOnly = True
                txtRollName.Text = IIf(IsDBNull(DT.Rows(0).Item("ROM_VC50_Role_Name")), "", DT.Rows(0).Item("ROM_VC50_Role_Name"))
                txtAliasName.Text = IIf(IsDBNull(DT.Rows(0).Item("ROM_VC50_Alias_Name")), "", DT.Rows(0).Item("ROM_VC50_Alias_Name"))
                txtParentRole.Value = IIf(IsDBNull(DT.Rows(0).Item("ROM_VC50_Parent_Role")), "", DT.Rows(0).Item("ROM_VC50_Parent_Role"))
                ' txtCompanyID.Text = IIf(IsDBNull(DT.Rows(0).Item("ROM_IN4_Company_ID_FK")), "", DT.Rows(0).Item("ROM_IN4_Company_ID_FK"))
                '   txtCompanyIDName.Text = IIf(IsDBNull(DT.Rows(0).Item("CI_VC36_Name")), "", DT.Rows(0).Item("CI_VC36_Name"))
                DDLCompany.SelectedValue = Val(DT.Rows(0).Item("ROM_IN4_Company_ID_FK"))
                ' DDLRole.SelectedValue = Val(ViewState("RoleKey"))
                txtBusinessUnit.Text = IIf(IsDBNull(DT.Rows(0).Item("ROM_IN4_BusinessUnit_ID_FK")), "", DT.Rows(0).Item("ROM_IN4_BusinessUnit_ID_FK"))
                txtDescription.Text = IIf(IsDBNull(DT.Rows(0).Item("ROM_VC50_Descr")), "", DT.Rows(0).Item("ROM_VC50_Descr"))
                dtStartDate.Text = SetDateFormat(IIf(IsDBNull(DT.Rows(0).Item("ROM_DT8_Start_Date")), "", DT.Rows(0).Item("ROM_DT8_Start_Date")), mdlMain.IsTime.DateOnly)
                chkIsAdminRights.Checked = DT.Rows(0).Item("ROM_CH1_IsAdminRight") ' new addition to set admin role rights
                Session("RoleCreatedDate") = SetDateFormat(IIf(IsDBNull(DT.Rows(0).Item("ROM_DT8_Start_Date")), "", DT.Rows(0).Item("ROM_DT8_Start_Date")), mdlMain.IsTime.DateOnly)
                If DT.Rows(0).Item("ROM_DT8_End_Date") = "01/01/1900" Then
                Else
                    dtEndDate.Text = SetDateFormat(IIf((IsDBNull(DT.Rows(0).Item("ROM_DT8_End_Date")) Or (DT.Rows(0).Item("ROM_DT8_End_Date") = "01/01/1900")), "", DT.Rows(0).Item("ROM_DT8_End_Date")), mdlMain.IsTime.DateOnly)
                End If
                'dtEndDate.CalendarDate = IIf(IsDBNull(DT.Rows(0).Item("ROM_DT8_End_Date")), "", DT.Rows(0).Item("ROM_DT8_End_Date"))
                cboEnabeDisable.SelectedValue = DT.Rows(0).Item("ROM_VC50_Status_Code_FK")
                If IsDBNull(DT.Rows(0).Item("ROM_DT8_Status_Date")) Then
                Else
                    dtStatusDate.Text = SetDateFormat(IIf(IsDBNull(DT.Rows(0).Item("ROM_DT8_Status_Date")), "", DT.Rows(0).Item("ROM_DT8_Status_Date")), mdlMain.IsTime.DateOnly)
                End If
                FillNonUDCDropDown(DDLRole, "select ROM_IN4_Role_ID_PK, ROM_VC50_Role_Name + ' [' + CI_VC36_Name +']'  from T070031,T010011 where ROM_IN4_Company_ID_FK=CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=" & Val(DT.Rows(0).Item("ROM_IN4_Copy_From")), True)
                If DDLRole.Items.Count > 1 Then
                    DDLRole.SelectedIndex = 1
                Else
                    DDLRole.SelectedIndex = 0
                End If
                'DDLRole.SelectedValue = IIf(Val(DT.Rows(0).Item("ROM_IN4_Copy_From")) = 0, "", DT.Rows(0).Item("ROM_IN4_Copy_From"))
            Else

                lstError.Items.Clear()
                lstError.Items.Add("Record not found...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("RolePermission", "ShowValues-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objCon.Close()
        End Try
    End Sub

    Sub myItems_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)

        Try
            'Dim tblStoreName As TableRow
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
                Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
                e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
            Else
                Dim a As String
                If e.Item.ItemType = ListItemType.SelectedItem Then
                    a = e.Item.ID.ToString
                End If
            End If
        Catch ex As Exception
            CreateLog("RolePermission", "my_ITMDB-429", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Function IsExistedRole(ByVal intRoleID As Integer) As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim obj As Object
        Try
            strQuery = "Select count(*) from T070031 where " _
               & " ROM_IN4_Role_ID_PK=" & intRoleID
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
            CreateLog("RolePermission", "isExistedRole-459", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        Finally
            objConn.Close()
        End Try
    End Function

    Private Function GetRoleData()
        Dim dtRoleData As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtRoleData = New DataTable
            Dim sqlQuery As String
            sqlCon = New SqlConnection(SQL.DBConnection)
            sqlQuery = "Select ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
               & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable" _
               & " from T070042 where ROD_IN4_Role_ID_FK=" & DDLRole.SelectedValue

            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtRoleData)
            Return dtRoleData
        Catch ex As Exception
            CreateLog("RolePermission", "GetRoleData-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            sqlCon.Close()
        End Try

    End Function

    Private Sub CopyRole(ByVal intRoleID As Int32)
        Dim dtRoleData As DataTable
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim strIP As String
        Try
            dtRoleData = GetRoleData()
            strIP = GetIP()
            If dtRoleData.Rows.Count > 0 Then
                objConn = New SqlConnection(strConn)
                objConn.Open()
                With cmdCommand
                    .CommandType = CommandType.Text
                    .Connection = objConn
                End With
                For i As Int16 = 0 To dtRoleData.Rows.Count - 1
                    strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                     & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                     & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                     & intRoleID & "," & dtRoleData.Rows(i).Item("ROD_IN4_Object_ID_FK") & ",'" & dtRoleData.Rows(i).Item("ROD_VC50_Alias_Name") _
                     & "','" & dtRoleData.Rows(i).Item("ROD_CH1_View_Hide") & "','" & dtRoleData.Rows(i).Item("ROD_CH1_Enable_Disable") _
                     & "','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & strIP & "')"
                    With cmdCommand
                        .CommandText = strQuery
                        .ExecuteNonQuery()
                    End With
                Next
            End If
        Catch ex As Exception
            CreateLog("RolePermission", "CopyRole-521", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objConn.Close()
        End Try
    End Sub

    Private Function SaveRoleData() As Boolean
        Dim intNextNum, intMRNo As Integer
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As New SqlConnection
        Dim strIsAdminRights As String = String.Empty
        objConn = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)

        lstError.Items.Clear()
        If DDLCompany.SelectedValue = "" Then
            lstError.Items.Clear()
            lstError.Items.Add("Company cannot be blank...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Function
        End If
        'added new code to fill admin rights with role
        If chkIsAdminRights.Checked = True Then
            strIsAdminRights = "1"
        Else
            strIsAdminRights = "0"
        End If

        Try
            If KeyStatus = 1 Then
                If Date.Parse(dtStartDate.Text.Trim) < Now.Date Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Start date cannot be less than current date...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)

                    Exit Function
                End If
            Else
                If Date.Parse(dtStartDate.Text.Trim) < Date.Parse(Session("RoleEditStartDate")) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Start date cannot be less than Previous selected start date " & Session("RoleEditStartDate"))
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            End If


            If (dtEndDate.Text.Trim <> "") Then
                If IsDate(dtEndDate.Text.Trim) Then
                    If Date.Parse(dtEndDate.Text.Trim) < Date.Parse(dtStartDate.Text.Trim) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("End date cannot be less than start date...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Exit Function
                    ElseIf Date.Parse(dtStatusDate.Text) < Date.Parse(dtStartDate.Text.Trim) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Status date cannot be less than start date...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Exit Function
                    End If
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("End date is not valid...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            End If

            If isUsed() = True Then
                If Not String.IsNullOrEmpty(Session("RoleCreatedDate")) Then
                    If CDate(Session("RoleCreatedDate")) = CDate(dtStartDate.Text.Trim) Then
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Role in Use so you cannot change start date...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Exit Function
                    End If
                End If
            End If

            If Date.Parse(dtStatusDate.Text) <= Now.Date Then
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Status must be less than  or equal to current date...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            End If


        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Some Error occured Please Try Later")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Exit Function
        End Try
        If (dtEndDate.Text.Trim <> "") Then
            If KeyStatus = 1 Then
                'Search Role Name
                Dim intRows As Integer
                If SQL.Search("", "", "select * from T070031 where ROM_VC50_Role_Name='" & txtRollName.Text.Trim & "' and ROM_IN4_Company_ID_FK=" & DDLCompany.SelectedValue & "", intRows) = True Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Role Name already exists...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
                intNextNum = clsNextNo.GetNextNo(100, "COM", strConn)
                HttpContext.Current.Session.Remove("RoleKey")
                strQuery = "Insert into T070031(ROM_IN4_Role_ID_PK,ROM_VC50_Role_Name,ROM_IN4_Company_ID_FK," _
                   & "ROM_IN4_BusinessUnit_ID_FK,ROM_VC50_Descr,ROM_VC50_Alias_Name,ROM_VC50_Parent_Role," _
                   & "ROM_IN4_Copy_From,ROM_DT8_Start_Date,ROM_DT8_End_Date,ROM_VC50_Status_Code_FK,ROM_DT8_Status_Date," _
                   & "ROM_SI2_Inserted_By,ROM_DT8_Inserted_Date,ROM_VC10_Inserted_By_System_Code,ROM_CH1_IsAdminRight)values(" _
                   & intNextNum & "," & "'" & txtRollName.Text.Trim.Replace("'", "''") & "'," & Val(DDLCompany.SelectedValue) & "," _
                   & Val(txtBusinessUnit.Text.Trim) & ",'" & txtDescription.Text.Trim.Replace("'", "''") & "','" & txtAliasName.Text.Trim.Replace("'", "''") _
                   & "','" & txtParentRole.Value.Trim & "'," & IIf(DDLRole.SelectedValue = "", 0, DDLRole.SelectedValue) & ",'" & dtStartDate.Text & "','" & dtEndDate.Text _
                   & "','" & cboEnabeDisable.SelectedValue & "','" & dtStatusDate.Text & "'," & HttpContext.Current.Session("PropUserID") & ",'" _
                   & Now.Date & "','" & GetIP() & "','" & strIsAdminRights & "')"

            ElseIf KeyStatus = -1 Then
                If IsExistedRole(Val(ViewState("RoleKey"))) = True Then

                    strQuery = "update T070031 set ROM_VC50_Role_Name='" & txtRollName.Text.Trim.Replace("'", "''") & "'," _
                      & "ROM_IN4_Company_ID_FK=" & Val(DDLCompany.SelectedValue) & "," _
                      & "ROM_IN4_BusinessUnit_ID_FK=" & Val(txtBusinessUnit.Text.Trim) & "," _
                      & "ROM_VC50_Descr='" & txtDescription.Text.Trim.Replace("'", "''") & "'," _
                      & "ROM_VC50_Alias_Name='" & txtAliasName.Text.Trim.Replace("'", "''") & "'," _
                      & "ROM_VC50_Parent_Role='" & txtParentRole.Value.Trim & "'," _
                      & " ROM_DT8_Start_Date='" & dtStartDate.Text & "'," _
                      & "ROM_DT8_End_Date='" & dtEndDate.Text & "'," _
                      & "ROM_VC50_Status_Code_FK='" & cboEnabeDisable.SelectedValue & "'," _
                      & "ROM_DT8_Status_Date='" & dtStatusDate.Text & "'," _
                      & "ROM_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                      & "ROM_DT8_Last_Modified_Date='" & Now.Date & "'," _
                      & "ROM_CH1_IsAdminRight='" & strIsAdminRights & "' where ROM_IN4_Role_ID_PK=" _
                      & Val(ViewState("RoleKey"))
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Record not found...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'lstError.Items.Add("Record not found")
                    Exit Function
                End If
            End If
        Else
            'Dim dbNull1 As DBNull
            If KeyStatus = 1 Then
                intNextNum = clsNextNo.GetNextNo(100, "COM", strConn)
                HttpContext.Current.Session.Remove("RoleKey")
                strQuery = "Insert into T070031(ROM_IN4_Role_ID_PK,ROM_VC50_Role_Name,ROM_IN4_Company_ID_FK," _
                   & "ROM_IN4_BusinessUnit_ID_FK,ROM_VC50_Descr,ROM_VC50_Alias_Name,ROM_VC50_Parent_Role," _
                   & "ROM_DT8_Start_Date,ROM_DT8_End_Date,ROM_VC50_Status_Code_FK,ROM_DT8_Status_Date," _
                   & "ROM_SI2_Inserted_By,ROM_DT8_Inserted_Date,ROM_VC10_Inserted_By_System_Code,ROM_CH1_IsAdminRight)values(" _
                   & intNextNum & "," & "'" & txtRollName.Text.Trim.Replace("'", "''") & "'," & Val(DDLCompany.SelectedValue) & "," _
                   & Val(txtBusinessUnit.Text.Trim) & ",'" & txtDescription.Text.Trim.Replace("'", "''") & "','" & txtAliasName.Text.Trim.Replace("'", "''") _
                   & "','" & txtParentRole.Value.Trim & "','" & dtStartDate.Text & "','" & DBNull.Value _
                   & "','" & cboEnabeDisable.SelectedValue & "','" & dtStatusDate.Text & "'," & HttpContext.Current.Session("PropUserID") & ",'" _
                   & Now.Date & "','" & GetIP() & "','" & strIsAdminRights & "')"

            ElseIf KeyStatus = -1 Then
                If IsExistedRole(Val(ViewState("RoleKey"))) = True Then

                    strQuery = "update T070031 set ROM_VC50_Role_Name='" & txtRollName.Text.Trim.Replace("'", "''") & "'," _
                      & "ROM_IN4_Company_ID_FK=" & Val(DDLCompany.SelectedValue) & "," _
                      & "ROM_IN4_BusinessUnit_ID_FK=" & Val(txtBusinessUnit.Text.Trim) & "," _
                      & "ROM_VC50_Descr='" & txtDescription.Text.Trim.Replace("'", "''") & "'," _
                      & "ROM_VC50_Alias_Name='" & txtAliasName.Text.Trim.Replace("'", "''") & "'," _
                      & "ROM_VC50_Parent_Role='" & txtParentRole.Value.Trim & "'," _
                      & " ROM_DT8_Start_Date='" & dtStartDate.Text & "'," _
                      & "ROM_DT8_End_Date='" & DBNull.Value & "'," _
                      & "ROM_VC50_Status_Code_FK='" & cboEnabeDisable.SelectedValue & "'," _
                      & "ROM_DT8_Status_Date='" & dtStatusDate.Text & "'," _
                      & "ROM_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                      & "ROM_DT8_Last_Modified_Date='" & Now.Date & "'," _
                      & "ROM_CH1_IsAdminRight='" & strIsAdminRights & "' where ROM_IN4_Role_ID_PK=" _
                      & Val(ViewState("RoleKey"))
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Record not found...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    'lstError.Items.Add("Record not found")
                    Exit Function
                End If
            End If

        End If
        Try
            objConn = New SqlConnection(strConn)
            objConn.Open()
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .ExecuteNonQuery()
                DDLCompany.Enabled = False
                DDLRole.Enabled = False
                txtRollName.ReadOnly = True
            End With
            'FillNonUDCDropDown(DDLRole, "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as RoleName from T070031 where  ROM_IN4_Company_ID_FK=" & DDLCompany.SelectedValue, True)
            lstError.Items.Clear()
            If KeyStatus = 1 Then
                If Val(DDLRole.SelectedValue) > 0 Then
                    CopyRole(intNextNum)
                End If
                HttpContext.Current.Session.Add("RoleKey", intNextNum)
                KeyStatus = -1
                cpnlCallTask.Visible = True
                cpnlTaskAction.Visible = True
                Collapsiblepanel1.Visible = True
                FillMenuGrid()
            Else
                SaveMenuAll()
                SaveScreenAll()
                SaveControlAll()
            End If
            'DDLRole.SelectedValue = ""
            DDLCompany.Enabled = False
            Return True
        Catch ex As Exception
            CreateLog("RolePermission", "SaveRoleData-594", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objConn.Close()
        End Try
    End Function

    Private Sub SaveMenu(ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim gridRow As DataGridItem
        Dim strAliasName As String
        Dim strVH, strED As String
        Dim intRoleID, intMenuID As Integer
        intRoleID = Val(ViewState("RoleKey"))

        If cboEnabeDisable.SelectedValue = "DSB" Then
            Exit Sub
        End If

        Try
            'gridRow = dgMenu.Items(dgMenu.SelectedItem.ItemIndex)
            strAliasName = CType(e.Item.FindControl("txtAliasMenu"), TextBox).Text
            If strAliasName = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Screen name cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            If CType(e.Item.FindControl("rdView"), RadioButton).Checked = False And CType(e.Item.FindControl("rdHide"), RadioButton).Checked = False Then
                strVH = "V"
            Else
                If CType(e.Item.FindControl("rdView"), RadioButton).Checked = True Then
                    strVH = "V"
                Else
                    strVH = "H"
                End If
            End If
            intMenuID = Val(e.Item.Cells(1).Text)
            If IsExisted(intRoleID, intMenuID) = True Then
                strQuery = "Update T070042 set ROD_VC50_Alias_Name='" & strAliasName & "'," _
                  & "ROD_CH1_View_Hide='" & strVH & "'," & "ROD_CH1_Enable_Disable='Z'," _
                  & "ROD_CH1_isLast_Level='N', ROD_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                  & "ROD_DT8_Last_Modified_Date='" & Now.Date _
                  & "' where ROD_IN4_Object_ID_FK=" & intMenuID _
                   & " and ROD_IN4_Role_ID_FK=" & intRoleID
            Else

                strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                 & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                 & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                 & intRoleID & "," & intMenuID & ",'" & strAliasName & "','" & strVH _
                 & "','Z','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"


            End If

            objConn = New SqlConnection(strConn)
            objConn.Open()
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            CreateLog("RolePermission", "SaveMenu-654", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            Try
                objConn.Close()
            Catch ex As Exception
                CreateLog("RolePermission", "SaveMenu-939", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try

        End Try
    End Sub

    Private Sub SaveScreen(ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim strAliasName As String
        Dim strVH, strED As String
        Dim intRoleID, intMenuID As Integer
        If cboEnabeDisable.SelectedValue = "DSB" Then
            Exit Sub
        End If

        intRoleID = Val(ViewState("RoleKey"))
        Try
            strAliasName = CType(e.Item.FindControl("txtAliasScreen"), TextBox).Text
            If strAliasName = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Menu name cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

            If CType(e.Item.FindControl("rdView1"), RadioButton).Checked = False And CType(e.Item.FindControl("rdHide1"), RadioButton).Checked = False Then
                strVH = "V"
            Else
                If CType(e.Item.FindControl("rdView1"), RadioButton).Checked = True Then
                    strVH = "V"
                Else
                    strVH = "H"
                End If
            End If
            intMenuID = e.Item.Cells(1).Text
            If IsExisted(intRoleID, intMenuID) = True Then
                strQuery = "Update T070042 set ROD_VC50_Alias_Name='" & strAliasName & "'," _
                  & "ROD_CH1_View_Hide='" & strVH & "'," & "ROD_CH1_Enable_Disable='Z'," _
                  & "ROD_CH1_isLast_Level='N', ROD_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                  & "ROD_DT8_Last_Modified_Date='" & Now.Date _
                  & "' where ROD_IN4_Object_ID_FK=" & intMenuID _
                   & " and ROD_IN4_Role_ID_FK=" & intRoleID
            Else
                strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                & intRoleID & "," & intMenuID & ",'" & strAliasName & "','" & strVH _
                & "','Z','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"

            End If
            objConn = New SqlConnection(strConn)
            objConn.Open()
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            CreateLog("RolePermission", "SaveScreen-708", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            Try
                objConn.Close()
            Catch ex As Exception
                CreateLog("RolePermission", "SaveMenu-654", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End Try
    End Sub

    Private Sub SaveControl(ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim strAliasName, strAliasName1 As String
        Dim strVH, strED As String
        Dim intRoleID, intMenuID As Integer
        Dim intCount As Int16
        Dim gridRow As DataGridItem
        'Dim str As String = Environment.MachineName
        If cboEnabeDisable.SelectedValue = "DSB" Then
            Exit Sub
        End If

        intRoleID = Val(ViewState("RoleKey"))
        Try
            strAliasName = CType(e.Item.FindControl("txtAliasControl"), TextBox).Text
            If strAliasName = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Control name cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If
            For Each gridRow In dgControls.Items
                strAliasName1 = CType(gridRow.FindControl("txtAliasControl"), TextBox).Text
                If strAliasName = strAliasName1 Then
                    intCount = intCount + 1
                    If intCount > 1 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Control name cannot be duplicate...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                End If
            Next

            If CType(e.Item.FindControl("rdView2"), RadioButton).Checked = False And CType(e.Item.FindControl("rdHide2"), RadioButton).Checked = False Then
                strVH = "V"
            Else
                If CType(e.Item.FindControl("rdView2"), RadioButton).Checked = True Then
                    strVH = "V"
                Else
                    strVH = "H"
                End If
            End If
            If CType(e.Item.FindControl("rdEnable"), RadioButton).Checked = False And CType(e.Item.FindControl("rdDisable"), RadioButton).Checked = False Then
                strED = "E"
                'Dim rd As RadioButton
                'rd = CType(e.Item.FindControl("rdView2"), RadioButton)
                'rd.Checked = True
            Else
                If CType(e.Item.FindControl("rdEnable"), RadioButton).Checked = True Then
                    strED = "E"
                Else
                    strED = "D"
                End If
                'Dim rd As RadioButton
                'rd = CType(e.Item.FindControl("rdView2"), RadioButton)
                'rd.Checked = True
            End If


            intMenuID = e.Item.Cells(1).Text
            If IsExisted(intRoleID, intMenuID) = True Then
                strQuery = "Update T070042 set ROD_VC50_Alias_Name='" & strAliasName & "'," _
                  & "ROD_CH1_View_Hide='" & strVH & "'," & "ROD_CH1_Enable_Disable='" & strED _
                  & "',ROD_CH1_isLast_Level='N', ROD_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                  & "ROD_DT8_Last_Modified_Date='" & Now.Date _
                  & "' where ROD_IN4_Object_ID_FK=" & intMenuID _
                   & " and ROD_IN4_Role_ID_FK=" & intRoleID
            Else
                strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                & intRoleID & "," & intMenuID & ",'" & strAliasName & "','" & strVH _
                & "','" & strED & "','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"
            End If
            objConn = New SqlConnection(strConn)
            objConn.Open()
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            CreateLog("RolePermission", "SaveControl-780", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            Try
                objConn.Close()
            Catch ex As Exception
                CreateLog("RolePermission", "SaveControl-1100", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End Try
    End Sub

    Private Function IsExisted(ByVal intRoleID As Integer, ByVal intObjectID As Integer) As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim obj As Object
        Try
            strQuery = "Select count(*) from T070042 where ROD_IN4_Object_ID_FK=" & intObjectID _
               & " and ROD_IN4_Role_ID_FK=" & intRoleID
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
            CreateLog("RolePermission", "IsExisted-812", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            objConn.Close()
        End Try
    End Function

    Private Sub SaveControlAll()
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim gridRow As DataGridItem
        Dim strAliasName As String
        Dim strVH, strED As String
        Dim intRoleID, intMenuID As Integer
        If cboEnabeDisable.SelectedValue = "DSB" Then
            Exit Sub
        End If

        intRoleID = Val(ViewState("RoleKey"))
        objConn = New SqlConnection(strConn)
        objConn.Open()

        Try
            For Each gridRow In dgControls.Items

                strAliasName = CType(gridRow.FindControl("txtAliasControl"), TextBox).Text
                If CType(gridRow.FindControl("rdView2"), RadioButton).Checked = False And CType(gridRow.FindControl("rdHide2"), RadioButton).Checked = False Then
                    CType(gridRow.FindControl("rdView2"), RadioButton).Checked = True
                    strVH = "V"
                Else
                    If CType(gridRow.FindControl("rdView2"), RadioButton).Checked = True Then
                        strVH = "V"
                    Else
                        strVH = "H"
                    End If
                End If
                If CType(gridRow.FindControl("rdEnable"), RadioButton).Checked = False And CType(gridRow.FindControl("rdDisable"), RadioButton).Checked = False Then
                    CType(gridRow.FindControl("rdEnable"), RadioButton).Checked = True
                    strED = "E"
                Else
                    If CType(gridRow.FindControl("rdEnable"), RadioButton).Checked = True Then
                        strED = "E"
                    Else
                        strED = "D"
                    End If
                End If

                intMenuID = gridRow.Cells(1).Text
                If IsExisted(intRoleID, intMenuID) = True Then
                    strQuery = "Update T070042 set ROD_VC50_Alias_Name='" & strAliasName & "'," _
                      & "ROD_CH1_View_Hide='" & strVH & "'," & "ROD_CH1_Enable_Disable='" & strED & "'," _
                      & "ROD_CH1_isLast_Level='N', ROD_IN4_Last_Modified_By=" & HttpContext.Current.Session("PropUserID") & "," _
                      & "ROD_DT8_Last_Modified_Date='" & Now.Date _
                      & "' where ROD_IN4_Object_ID_FK=" & intMenuID _
                       & " and ROD_IN4_Role_ID_FK=" & intRoleID
                Else
                    strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                    & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                    & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                    & intRoleID & "," & intMenuID & ",'" & strAliasName & "','" & strVH _
                    & "','" & strED & "','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"
                End If
                With cmdCommand
                    .CommandText = strQuery
                    .CommandType = CommandType.Text
                    .Connection = objConn
                    .ExecuteNonQuery()
                End With
            Next
        Catch ex As Exception
            CreateLog("RolePermission", "SaveControlAll-883", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objConn.Close()
        End Try
    End Sub

    Private Sub InsertData()
        Dim intNextNum, intMRNo As Integer
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objTrn As SqlTransaction
        Dim objConn As SqlConnection
        intNextNum = clsNextNo.GetNextNo(100, "COM", strConn)
        objConn = New SqlConnection(strConn)
        objConn.Open()
        objTrn = objConn.BeginTransaction(IsolationLevel.RepeatableRead)
        cmdCommand.Transaction = objTrn

        strQuery = "Insert into T070031(ROM_IN4_Role_ID_PK,ROM_VC50_Role_Name,ROM_IN4_Company_ID_FK," _
           & "ROM_IN4_BusinessUnit_ID_FK,ROM_VC50_Descr,ROM_VC50_Alias_Name,ROM_VC50_Parent_Role," _
           & "ROM_DT8_Start_Date,ROM_DT8_End_Date,ROM_VC50_Status_Code_FK,ROM_DT8_Status_Date," _
           & "ROM_SI2_Inserted_By,ROM_DT8_Inserted_Date,ROM_VC10_Inserted_By_System_Code)values(" _
           & intNextNum & "," & "'" & txtRollName.Text.Trim.Replace("'", "''") & "'," & DDLCompany.SelectedValue & "," _
           & txtBusinessUnit.Text.Trim & ",'" & txtDescription.Text.Trim.Replace("'", "''") & "','" & txtAliasName.Text.Trim.Replace("'", "''") _
           & "','" & txtParentRole.Value.Trim & "','" & dtStartDate.Text & "','" & dtEndDate.Text _
           & "','" & cboEnabeDisable.SelectedValue & "','" & dtStatusDate.Text & "'," & HttpContext.Current.Session("PropUserID") & ",'" _
           & Now.Date & "','" & GetIP() & "')"
        Try
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .ExecuteNonQuery()
            End With
            Dim gridRow As DataGridItem
            Dim strAliasName As String
            Dim strVH, strMenuID, strED As String
            For Each gridRow In dgMenu.Items
                strAliasName = CType(gridRow.FindControl("txtAliasMenu"), TextBox).Text
                If CType(gridRow.FindControl("rdView"), RadioButton).Checked = False And CType(gridRow.FindControl("rdHide"), RadioButton).Checked = False Then
                    strVH = "Z"
                Else
                    If CType(gridRow.FindControl("rdView"), RadioButton).Checked = True Then
                        strVH = "V"
                    Else
                        strVH = "H"
                    End If
                End If
                strMenuID = gridRow.Cells(1).Text
                strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                   & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                   & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                   & intNextNum & "," & strMenuID & ",'" & strAliasName & "','" & strVH _
                   & "','Z','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"
                With cmdCommand
                    .CommandText = strQuery
                    .ExecuteNonQuery()
                End With
            Next

            For Each gridRow In dgScreen.Items
                strAliasName = CType(gridRow.FindControl("txtAliasScreen"), TextBox).Text
                If CType(gridRow.FindControl("rdView1"), RadioButton).Checked = False And CType(gridRow.FindControl("rdHide1"), RadioButton).Checked = False Then
                    strVH = "Z"
                Else
                    If CType(gridRow.FindControl("rdView1"), RadioButton).Checked = True Then
                        strVH = "V"
                    Else
                        strVH = "H"
                    End If
                End If
                strMenuID = gridRow.Cells(1).Text
                strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                   & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                   & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                   & intNextNum & "," & strMenuID & ",'" & strAliasName & "','" & strVH _
                   & "','Z','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"
                With cmdCommand
                    .CommandText = strQuery
                    .ExecuteNonQuery()
                End With
            Next

            For Each gridRow In dgControls.Items
                strAliasName = CType(gridRow.FindControl("txtAliasControl"), TextBox).Text
                If CType(gridRow.FindControl("rdView2"), RadioButton).Checked = False And CType(gridRow.FindControl("rdHide2"), RadioButton).Checked = False Then
                    strVH = "Z"
                Else
                    If CType(gridRow.FindControl("rdView2"), RadioButton).Checked = True Then
                        strVH = "V"
                    Else
                        strVH = "H"
                    End If
                End If
                If CType(gridRow.FindControl("rdEnable"), RadioButton).Checked = False And CType(gridRow.FindControl("rdDisable"), RadioButton).Checked = False Then
                    strED = "Z"
                Else
                    If CType(gridRow.FindControl("rdEnable"), RadioButton).Checked = True Then
                        strED = "E"
                    Else
                        strED = "D"
                    End If
                End If

                strMenuID = gridRow.Cells(1).Text
                strQuery = "Insert into T070042(ROD_IN4_Role_ID_FK,ROD_IN4_Object_ID_FK,ROD_VC50_Alias_Name," _
                   & "ROD_CH1_View_Hide,ROD_CH1_Enable_Disable,ROD_CH1_isLast_Level," _
                   & "ROD_SI2_Inserted_By,ROD_DT8_Inserted_Date,ROD_VC10_Inserted_By_System_Code) values(" _
                   & intNextNum & "," & strMenuID & ",'" & strAliasName & "','" & strVH _
                   & "','" & strED & "','N'," & HttpContext.Current.Session("PropUserID") & ",'" & Now.Date & "','" & GetIP() & "')"
                With cmdCommand
                    .CommandText = strQuery
                    .ExecuteNonQuery()
                End With
            Next

            objTrn.Commit()
        Catch ex As Exception
            CreateLog("RolePermission", "InsertData-1001", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            objTrn.Rollback()
        End Try
    End Sub

#Region "MenuGrid"
    Private Function FillMenuGrid()
        Dim dtItemDetail As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtItemDetail = New DataTable
            Dim sqlQuery As String
            Dim row As DataRow
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqlCon = New SqlConnection(SQL.DBConnection)

            'sqlQuery = "Select OBM_IN4_Object_ID_PK, OBM_VC50_Object_Name, OBM_VC50_Alias_Name from T070011 where OBM_VC4_Object_Type_FK='MNU' and OBM_VC4_Status_Code_FK='ENB'"
            sqlQuery = " Select  OBM.OBM_IN4_Object_ID_PK ,OBM.OBM_VC50_Object_Name,OBM.OBM_VC200_Image," _
            & "ROD.ROD_VC50_Alias_Name as OBM_VC50_Alias_Name,OBM_VC50_Alias_Name as alias,ROD.ROD_CH1_View_Hide,ROD.ROD_CH1_Enable_Disable,OBM.OBM_CH1_Mandatory from " _
              & "T070042 ROD, T070011 OBM where OBM.OBM_IN4_Object_ID_PK *= ROD.ROD_IN4_Object_ID_FK " _
              & " and ROD.ROD_IN4_Role_ID_FK =" & Val(ViewState("RoleKey")) & " and OBM.OBM_VC4_Object_Type_FK = 'MNU' and OBM.OBM_VC4_Status_Code_FK='ENB' order by obm.OBM_SI2_Order_By"

            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtItemDetail)
            dgMenu.DataSource = dtItemDetail
            dgMenu.DataBind()
            Dim i As Integer
            For i = 0 To dgMenu.Items.Count - 1
                If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide"))) Then
                    If dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "V" Or dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "Z" Then
                        CType(dgMenu.Items(i).FindControl("rdView"), RadioButton).Checked = True
                    ElseIf dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "H" Then
                        CType(dgMenu.Items(i).FindControl("rdHide"), RadioButton).Checked = True
                    End If
                Else
                    CType(dgMenu.Items(i).FindControl("rdHide"), RadioButton).Checked = True
                End If
                If cboEnabeDisable.SelectedValue = "DSB" Then
                    CType(dgMenu.Items(i).FindControl("rdView"), RadioButton).Enabled = False
                    CType(dgMenu.Items(i).FindControl("rdHide"), RadioButton).Enabled = False
                    CType(dgMenu.Items(i).FindControl("txtAliasMenu"), TextBox).Enabled = False

                Else
                    If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("OBM_CH1_Mandatory"))) Then
                        If dtItemDetail.Rows.Item(i).Item("OBM_CH1_Mandatory") = "Y" Then
                            'dgMenu.Items.Item(i).Enabled = False
                            CType(dgMenu.Items(i).FindControl("txtAliasMenu"), TextBox).Enabled = False
                            CType(dgMenu.Items(i).FindControl("rdView"), RadioButton).Enabled = False
                            CType(dgMenu.Items(i).FindControl("rdHide"), RadioButton).Enabled = False
                            CType(dgMenu.Items(i).FindControl("rdView"), RadioButton).Checked = True
                        End If
                    End If
                End If
                If IsDBNull(dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name")) Then
                    CType(dgMenu.Items(i).FindControl("txtAliasMenu"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("alias")
                ElseIf dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name") = "" Then
                    CType(dgMenu.Items(i).FindControl("txtAliasMenu"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("alias")
                Else
                    CType(dgMenu.Items(i).FindControl("txtAliasMenu"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name")
                End If
            Next
        Catch ex As Exception
            CreateLog("RolePermission", "FillMenuGrid-1065", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Public Shared Sub SetFocus(ByVal control As Control)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "<script language='JavaScript'>" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("<!--" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("function SetFocus()" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("{" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("" & Microsoft.VisualBasic.Chr(9) & "document.")
        Dim p As Control = control.Parent
        While Not (TypeOf p Is System.Web.UI.HtmlControls.HtmlForm)
            p = p.Parent
        End While
        sb.Append(p.ClientID)
        sb.Append("['")
        sb.Append(control.UniqueID)
        sb.Append("'].focus();" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("}" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("window.onload = SetFocus;" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("// -->" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("</script>")
        control.Page.RegisterClientScriptBlock("SetFocus", sb.ToString)
    End Sub

    Private Sub dgMenu_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgMenu.ItemCommand
        cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
        cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
        Collapsiblepanel1.State = CustomControls.Web.PanelState.Collapsed
        Try

            If e.CommandName = "select" Then

                Dim i As Integer = dgMenu.Columns.Count
                For i = 0 To dgMenu.Items.Count - 1
                    If i Mod 2 = 0 Then
                        dgMenu.Items(i).BackColor = Color.WhiteSmoke
                    Else
                        dgMenu.Items(i).BackColor = Color.White
                    End If
                Next

                e.Item.BackColor = Color.LightGray
            End If

            SaveMenu(e)
            'SaveRoleData()
            strValue = Convert.ToString(e.Item.Cells(1).Text)
            dgControls.Visible = False
            ViewState.Add("MNID", strValue)
            FillScreenGrid(CInt(strValue))
            If dgScreen.Items.Count > 0 Then
                dgScreen.Visible = True
                cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
            Else
                dgScreen.Visible = False
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            End If

            Dim objTBox As TextBox
            objTBox = CType(e.Item.FindControl("txtAliasMenu"), TextBox)
            If objTBox.Enabled = True Then
                SetFocus(objTBox)
            End If
        Catch ex As Exception
            CreateLog("RolePermission", "dgMenuICMD-1126", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "ScreenGrid"
    Private Function FillScreenGrid(ByVal PID As Integer)


        Dim dtItemDetail As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtItemDetail = New DataTable
            Dim sqlQuery As String
            Dim row As DataRow

            sqlCon = New SqlConnection(SQL.DBConnection)

            'sqlQuery = "Select OBM_IN4_Object_ID_PK, OBM_VC50_Object_Name, OBM_VC50_Alias_Name from T070011 where OBM_VC4_Object_Type_FK='MNU' and OBM_VC4_Status_Code_FK='ENB'"
            sqlQuery = " Select  OBM.OBM_IN4_Object_ID_PK ,OBM.OBM_VC50_Object_Name, OBM.OBM_VC200_Image," _
               & "ROD.ROD_VC50_Alias_Name as OBM_VC50_Alias_Name,OBM_VC50_Alias_Name as alias, " _
               & "ROD.ROD_CH1_View_Hide,ROD.ROD_CH1_Enable_Disable,OBM.OBM_CH1_Mandatory from " _
              & "T070042 ROD, T070011 OBM where OBM.OBM_IN4_Object_ID_PK *= ROD.ROD_IN4_Object_ID_FK " _
              & " and ROD.ROD_IN4_Role_ID_FK =" & ViewState("RoleKey") & " and (OBM.OBM_VC4_Object_Type_FK = 'SCR' or OBM.OBM_VC4_Object_Type_FK = 'POP')" _
              & " and OBM.OBM_VC4_Status_Code_FK='ENB' and OBM_IN4_Object_PID_FK=" & PID & " order by obm.OBM_SI2_Order_By"
            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtItemDetail)
            dgScreen.DataSource = dtItemDetail
            dgScreen.DataBind()
            Dim i As Integer
            For i = 0 To dgScreen.Items.Count - 1
                If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide"))) Then
                    If dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "V" Or dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "Z" Then
                        CType(dgScreen.Items(i).FindControl("rdView1"), RadioButton).Checked = True
                    ElseIf dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "H" Then
                        CType(dgScreen.Items(i).FindControl("rdHide1"), RadioButton).Checked = True
                    End If
                Else
                    CType(dgScreen.Items(i).FindControl("rdHide1"), RadioButton).Checked = True
                End If
                If cboEnabeDisable.SelectedValue = "DSB" Then
                    CType(dgScreen.Items(i).FindControl("rdView1"), RadioButton).Enabled = False
                    CType(dgScreen.Items(i).FindControl("rdHide1"), RadioButton).Enabled = False
                    CType(dgScreen.Items(i).FindControl("txtAliasScreen"), TextBox).Enabled = False
                Else
                    CType(dgScreen.Items(i).FindControl("rdView1"), RadioButton).Enabled = True
                    CType(dgScreen.Items(i).FindControl("rdHide1"), RadioButton).Enabled = True
                    CType(dgScreen.Items(i).FindControl("txtAliasScreen"), TextBox).Enabled = True

                    If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("OBM_CH1_Mandatory"))) Then
                        If dtItemDetail.Rows.Item(i).Item("OBM_CH1_Mandatory") = "Y" Then
                            'dgScreen.Items.Item(i).Enabled = False
                            CType(dgScreen.Items(i).FindControl("rdView1"), RadioButton).Enabled = False
                            CType(dgScreen.Items(i).FindControl("rdHide1"), RadioButton).Enabled = False
                            CType(dgScreen.Items(i).FindControl("rdView1"), RadioButton).Checked = True
                            CType(dgScreen.Items(i).FindControl("txtAliasScreen"), TextBox).Enabled = False
                        End If
                    End If
                End If
                If IsDBNull(dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name")) Then
                    CType(dgScreen.Items(i).FindControl("txtAliasScreen"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("alias")
                ElseIf dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name") = "" Then
                    CType(dgScreen.Items(i).FindControl("txtAliasScreen"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("alias")
                Else
                    CType(dgScreen.Items(i).FindControl("txtAliasScreen"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name")
                End If
            Next
        Catch ex As Exception
            CreateLog("RolePermission", "FillScrenGridL-1197", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

    Private Sub dgScreen_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgScreen.ItemCommand
        cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
        cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
        'cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
        Try
            If e.CommandName = "select" Then
                Dim i As Integer = dgScreen.Columns.Count

                For i = 0 To dgScreen.Items.Count - 1
                    If i Mod 2 = 0 Then
                        dgScreen.Items(i).BackColor = Color.WhiteSmoke
                    Else
                        dgScreen.Items(i).BackColor = Color.White
                    End If
                Next
                dgControls.Visible = True
            End If
            e.Item.BackColor = Color.LightGray
            strValue = Convert.ToString(e.Item.Cells(1).Text)
            ViewState.Add("SCID", strValue)
            FillControlGrid(CInt(strValue))
            If dgControls.Items.Count > 0 Then
                dgControls.Visible = True
                Collapsiblepanel1.State = CustomControls.Web.PanelState.Expanded
            Else
                dgControls.Visible = False
                Collapsiblepanel1.State = CustomControls.Web.PanelState.Collapsed
            End If
            SaveScreen(e)
            Dim objTBox As TextBox
            objTBox = CType(e.Item.FindControl("txtAliasScreen"), TextBox)
            If objTBox.Enabled = True Then
                SetFocus(objTBox)
            End If
            'SaveRoleData()
        Catch ex As Exception
            CreateLog("RolePermission", "dgScreen-1234", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
#End Region

#Region "ControlGrid"
    Private Function FillControlGrid(ByVal PID As Integer)
        Dim dtItemDetail As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtItemDetail = New DataTable
            Dim sqlQuery As String
            Dim row As DataRow

            sqlCon = New SqlConnection(SQL.DBConnection)

            'sqlQuery = "Select OBM_IN4_Object_ID_PK, OBM_VC50_Object_Name, OBM_VC50_Alias_Name from T070011 where OBM_VC4_Object_Type_FK='MNU' and OBM_VC4_Status_Code_FK='ENB'"
            sqlQuery = " Select  OBM.OBM_VC4_Object_Type_FK, OBM.OBM_IN4_Object_ID_PK ,OBM.OBM_VC50_Object_Name,OBM.OBM_VC200_Image," _
               & "ROD.ROD_VC50_Alias_Name as OBM_VC50_Alias_Name,OBM_VC50_Alias_Name as alias,ROD.ROD_CH1_View_Hide,ROD.ROD_CH1_Enable_Disable,OBM.OBM_CH1_Mandatory from " _
              & "T070042 ROD, T070011 OBM where OBM.OBM_IN4_Object_ID_PK *= ROD.ROD_IN4_Object_ID_FK " _
              & " and ROD.ROD_IN4_Role_ID_FK =" & ViewState("RoleKey") & " and (OBM.OBM_VC4_Object_Type_FK != 'SCR' and OBM.OBM_VC4_Object_Type_FK != 'MNU')" _
              & " and OBM.OBM_VC4_Status_Code_FK='ENB' and OBM_IN4_Object_PID_FK=" & PID & " order by obm.OBM_SI2_Order_By"
            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtItemDetail)
            dgControls.DataSource = dtItemDetail
            dgControls.DataBind()
            Dim i As Integer
            For i = 0 To dgControls.Items.Count - 1

                If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide"))) Then
                    If dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "V" Or dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "Z" Then
                        CType(dgControls.Items(i).FindControl("rdView2"), RadioButton).Checked = True
                    ElseIf dtItemDetail.Rows.Item(i).Item("ROD_CH1_View_Hide") = "H" Then
                        CType(dgControls.Items(i).FindControl("rdHide2"), RadioButton).Checked = True
                    End If
                Else
                    'CType(dgControls.Items(i).FindControl("rdView2"), RadioButton).Checked = True
                    CType(dgControls.Items(i).FindControl("rdHide2"), RadioButton).Checked = True
                End If


                If cboEnabeDisable.SelectedValue = "DSB" Then
                    CType(dgControls.Items(i).FindControl("rdView2"), RadioButton).Enabled = False
                    CType(dgControls.Items(i).FindControl("rdHide2"), RadioButton).Enabled = False
                    CType(dgControls.Items(i).FindControl("txtAliasControl"), TextBox).Enabled = False
                    CType(dgControls.Items(i).FindControl("rdEnable"), RadioButton).Enabled = False
                    CType(dgControls.Items(i).FindControl("rdDisable"), RadioButton).Enabled = False
                Else
                    CType(dgControls.Items(i).FindControl("rdView2"), RadioButton).Enabled = True
                    CType(dgControls.Items(i).FindControl("rdHide2"), RadioButton).Enabled = True
                    CType(dgControls.Items(i).FindControl("txtAliasControl"), TextBox).Enabled = True
                    CType(dgControls.Items(i).FindControl("rdEnable"), RadioButton).Enabled = True
                    CType(dgControls.Items(i).FindControl("rdDisable"), RadioButton).Enabled = True

                    If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("ROD_CH1_Enable_Disable"))) Then
                        If dtItemDetail.Rows.Item(i).Item("ROD_CH1_Enable_Disable") = "E" Or dtItemDetail.Rows.Item(i).Item("ROD_CH1_Enable_Disable") = "Z" Then
                            CType(dgControls.Items(i).FindControl("rdEnable"), RadioButton).Checked = True
                        ElseIf dtItemDetail.Rows.Item(i).Item("ROD_CH1_Enable_Disable") = "D" Then
                            CType(dgControls.Items(i).FindControl("rdDisable"), RadioButton).Checked = True
                        End If
                    Else
                        'CType(dgControls.Items(i).FindControl("rdEnable"), RadioButton).Checked = True
                        CType(dgControls.Items(i).FindControl("rdDisable"), RadioButton).Checked = True
                    End If
                    If Not (IsDBNull(dtItemDetail.Rows.Item(i).Item("OBM_CH1_Mandatory"))) Then
                        If dtItemDetail.Rows.Item(i).Item("OBM_CH1_Mandatory") = "Y" Then
                            CType(dgControls.Items(i).FindControl("rdView2"), RadioButton).Checked = True
                            CType(dgControls.Items(i).FindControl("rdEnable"), RadioButton).Checked = True
                            CType(dgControls.Items(i).FindControl("txtAliasControl"), TextBox).Enabled = False
                            dgControls.Items.Item(i).Enabled = False
                        End If
                    End If
                End If
                If IsDBNull(dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name")) Then
                    CType(dgControls.Items(i).FindControl("txtAliasControl"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("alias")
                ElseIf dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name") = "" Then
                    CType(dgControls.Items(i).FindControl("txtAliasControl"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("alias")
                Else
                    CType(dgControls.Items(i).FindControl("txtAliasControl"), TextBox).Text = dtItemDetail.Rows.Item(i).Item("OBM_VC50_Alias_Name")
                End If

                If CStr(dtItemDetail.Rows(i).Item("OBM_VC4_Object_Type_FK")).ToUpper.Equals("VIW") Then
                    CType(dgControls.Items(i).FindControl("rdDisable"), RadioButton).Enabled = False
                    CType(dgControls.Items(i).FindControl("rdEnable"), RadioButton).Enabled = False
                End If
            Next
        Catch ex As Exception
            CreateLog("RolePermission", "FillControlGrid-1316", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

    Private Sub dgControls_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgControls.ItemCommand
        Try

            If e.CommandName = "select" Then

                Dim i As Integer = dgControls.Columns.Count
                For i = 0 To dgControls.Items.Count - 1
                    If i Mod 2 = 0 Then
                        dgControls.Items(i).BackColor = Color.WhiteSmoke
                    Else
                        dgControls.Items(i).BackColor = Color.White
                    End If
                Next

                e.Item.BackColor = Color.LightGray
            End If
            SaveControl(e)
            Dim objTBox As TextBox
            objTBox = CType(e.Item.FindControl("txtAliasControl"), TextBox)
            If objTBox.Enabled = True Then
                SetFocus(objTBox)
            End If
        Catch ex As Exception
            CreateLog("RolePermission", "dgcontrolsitCMD-1343", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

    End Sub

    Private Sub dgMenu_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgMenu.SelectedIndexChanged

    End Sub

    Private Sub dgScreen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgScreen.SelectedIndexChanged

    End Sub

    Private Sub dgControls_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgControls.SelectedIndexChanged

    End Sub

    Private Function isUsed() As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        Dim strConn As String
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim obj As Object
        Try
            strQuery = "Select count(*) from T060022 where RA_IN4_Role_ID_FK=" & Val(ViewState("RoleKey"))
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
            Response.Write(ex.Message)
            Return False
        Finally
            objConn.Close()

        End Try

    End Function

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class
