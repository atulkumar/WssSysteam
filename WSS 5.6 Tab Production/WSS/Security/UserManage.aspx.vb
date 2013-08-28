'************************************************************************************************************
' Page                 :- User Management
' Purpose              :- This screen is used to set the userid & password with address book name & also to                            assign roles to address book. 

' Tables used          :- T010011, T070042,T070031
' Date				Author						Modification Date					Description
' 11/03/06		Amandeep/jaswinder			-------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
#Region "Session Used"
'Session("PropUserID")
'Session("PropUserName")
'Session("PropCompanyID")
'Session("SUserID")replace  with ViewState("User_SearchUserID")
'Session("PropRootDir")
'Session("strtbname")
#End Region

#Region "NameSpace"
Imports ION.Data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
#End Region

Partial Class Security_UserManage
    Inherits System.Web.UI.Page

#Region "Varibles"
    Private mintAddressKey As Integer ' Will keep Contact key and used to store contact key in session
    Protected Shared mdvtable As DataView = New DataView
    Dim rowvalue As Integer
    Shared mintID As Integer
    Private Shared Flag As Boolean
    Private insertedBy As String
    Private insertedOn As String
    Private systemBy As String
    Private RoleId As Integer
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")


        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        insertedBy = Session("PropUserID")
        insertedOn = Now.ToShortDateString
        systemBy = GetIP()
        txtAssBy.Text = Session("PropUserName")
        ''''''commented to takeoff password expiry textbox'''''
        txtAssignDate.Text = SetDateFormat(Now, mdlMain.IsTime.DateOnly)
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")


        lstError.Items.Clear()
        If Not IsPostBack Then
            txtCSS(Me.Page, "cpnlCallTask")
            FillNonUDCDropDown(DDLCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' AND CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ")")

            DDLCompany.SelectedValue = Session("PropCompanyID")
            '---------------------Addres numner CDDL
            CDDLUserID.CDDLQuery = "select T010011.CI_NU8_Address_Number as ID,T010011.CI_VC36_Name as UserName,CI_VC36_Name as Company from T010011 where CI_VC8_Address_Book_Type<>'COM' and CI_VC8_Status='ENA' and CI_IN4_Business_Relation='" & DDLCompany.SelectedValue & "' and  T010011.CI_NU8_Address_Number not in(select t060011.UM_IN4_Address_No_FK from T060011)"

            CDDLUserID.CDDLUDC = False
            CDDLUserID.CDDLType = CustomDDL.DDLType.NonFastEntry
            CDDLUserID.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"

            CDDLUserID.CDDLFillDropDown(10, False)
            '-----------------------------------------
        End If

        If IsPostBack = False Then
            mintID = Request.QueryString(("ID"))
        End If

        If mintID = -1 Then
            mintID = 0
        Else

        End If


        If Not IsPostBack And mintID = 1 Then
            dtStartdate.Text = SetDateFormat(Now.ToShortDateString, mdlMain.IsTime.DateOnly)
            txtCreateDate.Text = SetDateFormat(DateTime.Now.ToShortDateString, mdlMain.IsTime.DateOnly)
        End If
        ViewState("User_SearchUserID") = Request.QueryString("UserID")

        If IsNothing(Request.QueryString("ID")) = False Then
            mintAddressKey = -1
        Else
            ''mintAddressKey = CInt(Session("SUserID"))
            mintAddressKey = Request.QueryString("UserID") 'CInt(ViewState("User_SearchUserID"))

        End If

        Dim txthiddenImage As String = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        'Response.Redirect("User_Search.aspx?ScrID=63", False)

                    Case "Logout"
                        LogoutWSS()
                    Case "Add"
                        cpnlCallTask.Enabled = False
                        cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed

                        txtUserID.ReadOnly = False

                    Case "Ok"

                        If validateForm() Then
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                ' cpnlError.Visible = True
                                lstError.Items.Add("You don't have access rights to Save record...")
                                Exit Sub
                            End If
                            'End of Security Block

                            If mintID = 0 Then
                                If updateUser() = True And txthiddenImage = "Ok" Then
                                    If SaveCompanyList() = True Then
                                        Response.Redirect("User_Search.aspx?ScrID=63", False)
                                    End If
                                End If
                                Exit Select
                            ElseIf mintID = -1 Or mintID = 1 Then
                                If saveUser() = True Then
                                    SaveDefaultCompany()
                                    Response.Redirect("User_Search.aspx?ScrID=63", False)
                                Else
                                    mintID = 1
                                End If
                            End If
                        End If
                    Case "Save"

                        If validateForm() Then
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                '                                cpnlError.Visible = True
                                lstError.Items.Add("You don't have access rights to Save record...")
                                Exit Sub
                            End If
                            'End of Security Block

                            If mintID = 0 Then
                                updateUser()
                                If SaveCompanyList() = True Then

                                End If
                            ElseIf mintID = -1 Or mintID = 1 Then
                                If saveUser() = True Then
                                    cpnlCallTask.Enabled = True
                                    cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                                    cpnlCompany.Enabled = True
                                    cpnlCompany.State = CustomControls.Web.PanelState.Expanded
                                    ''Session.Item("SUserID") = CDDLUserID.CDDLGetValue.Trim
                                    ViewState("User_SearchUserID") = CDDLUserID.CDDLGetValue.Trim
                                    Call SaveDefaultCompany()
                                    mintID = 0

                                Else
                                    mintID = 1
                                End If
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            End If

                        End If

                    Case "Delete"

                        RoleId = Val(Request.Form("txtRoleId"))
                        DeleteAssignedRole(RoleId)

                    Case "Reset"
                        cpnlCallTask.Enabled = False
                        cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                        txtUserID.ReadOnly = False
                End Select




            Catch ex As Exception
                CreateLog("CraeteUser", "Load-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            End Try

        Else
            'dtRLAssDate.CalendarDate = Now.ToShortDateString
            dtrLValidUpto.Text = SetDateFormat(Now.AddYears(1).ToShortDateString, mdlMain.IsTime.DateOnly)
        End If

        '---------------------------------Role CDDL
        ''If Val(Session("SUserID")) > 0 Then
        If Val(ViewState("User_SearchUserID")) > 0 Then
            FillRoleCDDL()
        Else
            CDDLRole.CDDLAddSingleItem("", False, "")
        End If
        '-----------------------------------------

        ''If Not Session("SUserID") = "" Then
        If Not ViewState("User_SearchUserID") = "" Then
            CDDLUserID.Enabled = False
            DDLCompany.Enabled = False
            Call FillContact()
            Call GetCompanyList()

            txtUserID.ReadOnly = True
            fillgrid()
        Else
            CDDLUserID.Enabled = True
            DDLCompany.Enabled = True
        End If

        If CDDLUserID.CDDLGetValue.Trim = "" Then
            cpnlCallTask.Enabled = False
            cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            cpnlCompany.Enabled = False
            cpnlCompany.State = CustomControls.Web.PanelState.Collapsed
        End If


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

    End Sub

    Private Sub FillRoleCDDL()
        CDDLRole.CDDLQuery = "select ROM_IN4_Role_ID_PK as ID,ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompanyName from T070031,T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_IN4_Role_ID_PK not in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = " & CInt(ViewState("User_SearchUserID")) & " and RA_DT8_Assigned_Date <='" & Now.Date & "'and RA_DT8_Valid_UpTo >='" & Now.Date & "') and (ROM_IN4_Company_ID_FK = (select CI_IN4_Business_Relation from t010011 where CI_NU8_Address_Number =" & CInt(ViewState("User_SearchUserID")) & ") or ROM_IN4_Company_ID_FK = 0 )and ROM_DT8_Start_Date<='" & Now.Date & "' and ROM_DT8_End_Date >='" & Now.Date & "' and ROM_VC50_Status_Code_FK ='ENB'"
        CDDLRole.CDDLUDC = False
        CDDLRole.CDDLType = CustomDDL.DDLType.FastEntry
        CDDLRole.CDDLFillDropDown(10, False)
    End Sub

    Private Function validateForm() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        If CDDLUserID.CDDLGetValue.Trim = "" Then
            lstError.Items.Add("Address Book Name cannot be blank...")
            shFlag = 1
        End If
        If txtUserID.Text.Trim = "" Then
            lstError.Items.Add("User ID cannot be blank...")
            shFlag = 1
        End If
        If txtPwd.Text = "" Then
            lstError.Items.Add("Password cannot be blank...")
            shFlag = 1
        End If
        If txtPwd.Text.Length > 0 And txtPwd.Text.Length < 6 Then
            lstError.Items.Add("Password should be atleat 6 characters long ...")
            shFlag = 1
        End If
        If txtPwd.Text <> txtConPwd.Text Then
            lstError.Items.Add("Confirm password is not matching with password ...")
            shFlag = 1
        End If
        If txtPwd.Text.IndexOf(" ") >= 0 Then
            lstError.Items.Add("Password cannot contain blank spaces...")
            shFlag = 1
        End If
        If dtEndDate.Text.Trim.Equals("") Then
        Else
            If IsDate(dtEndDate.Text.Trim) = False Then
                shFlag = 1
                lstError.Items.Add("Enter a correct User Valid Upto Date...")
            Else
                If Not (DateTime.Parse(dtEndDate.Text) >= DateTime.Parse(dtStartdate.Text)) Then
                    shFlag = 1
                    lstError.Items.Add("User Valid upto date should be greater than user created date...")
                End If
            End If
        End If


        If cpnlCallTask.State = CustomControls.Web.PanelState.Expanded And CDDLRole.CDDLGetValue <> "" Then
            If Not (DateTime.Parse(dtrLValidUpto.Text) >= DateTime.Parse(txtAssignDate.Text.Trim)) Then
                shFlag = 1
                lstError.Items.Add("Role Valid upto date should be greater than Role assigned date...")
            End If
        End If

        If IsDate(dtEndDate.Text) Then
            If dtEndDate.Text < Now.Date Then
                shFlag = 1
                lstError.Items.Add("To Date cannot be less than current date...")
            End If
        End If


        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub FillContact()
        Dim strConnection As String = "Server=ion15;database=newwss;uid=sa;"
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String
        Dim strPwd As String

        strSql = "SELECT UM_IN4_Company_AB_ID, UM_NU9_Expiry_Mail_Days,UM_CH1_Mail_Comm,UM_CH1_Admin_Rights,CI_VC36_Name,UM_IN4_Address_No_FK, UM_VC50_UserID, UM_VC30_Password,UM_IN4_Expiry_Days,UM_VC4_User_Type_FK, UM_DT8_From_date, UM_DT8_To_date, UM_DT8_Created_Date, UM_VC4_Status_Code_FK, UM_DT8_Status_Date FROM T060011 U, T010011 A   WHERE  U.UM_IN4_Address_No_FK=A.CI_NU8_Address_Number and UM_IN4_Address_No_FK=" & ViewState("User_SearchUserID").ToString

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        '  SQL.DBTable = "TB_User_Management"

        Try
            sqrdRecords = SQL.Search("User Manage", "FillContact-372", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                sqrdRecords.Read()
                ' txtAddNo.Text = IIf(IsDBNull(sqrdRecords.Item("UM_IN4_Address_No_FK")), " ", sqrdRecords.Item("UM_IN4_Address_No_FK"))
                DDLCompany.SelectedValue = IIf(IsDBNull(sqrdRecords.Item("UM_IN4_Company_AB_ID")), "", sqrdRecords.Item("UM_IN4_Company_AB_ID"))
                CDDLUserID.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("UM_IN4_Address_No_FK")), " ", sqrdRecords.Item("UM_IN4_Address_No_FK")), False, IIf(IsDBNull(sqrdRecords.Item("CI_VC36_Name")), " ", sqrdRecords.Item("CI_VC36_Name")))
                txtUserID.Text = IIf(IsDBNull(sqrdRecords.Item("UM_VC50_UserID")), " ", sqrdRecords.Item("UM_VC50_UserID"))
                strPwd = IIf(IsDBNull(sqrdRecords.Item("UM_VC30_Password")), " ", sqrdRecords.Item("UM_VC30_Password"))

                'ddl_UserType.SelectedValue = IIf(IsDBNull(sqrdRecords.Item("UM_VC4_User_Type_FK")), " ", sqrdRecords.Item("UM_VC4_User_Type_FK"))

                If IsDBNull(sqrdRecords.Item("UM_DT8_From_date")) Then
                Else
                    dtStartdate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("UM_DT8_From_date")), " ", sqrdRecords.Item("UM_DT8_From_date")), mdlMain.IsTime.DateOnly)
                End If

                If IsDBNull(sqrdRecords.Item("UM_DT8_To_date")) Then
                Else
                    dtEndDate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("UM_DT8_To_date")), " ", sqrdRecords.Item("UM_DT8_To_date")), mdlMain.IsTime.DateOnly)
                End If

                If IsDBNull(sqrdRecords.Item("UM_DT8_Created_Date")) Then
                Else
                    txtCreateDate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("UM_DT8_Created_Date")), " ", sqrdRecords.Item("UM_DT8_Created_Date")), mdlMain.IsTime.DateOnly)
                End If

                Ddl_Status.SelectedValue = IIf(IsDBNull(sqrdRecords.Item("UM_VC4_Status_Code_FK")), " ", sqrdRecords.Item("UM_VC4_Status_Code_FK"))

                'txtPassExp.Text = IIf(sqrdRecords.Item("UM_IN4_Expiry_Days") = 0, "", sqrdRecords.Item("UM_IN4_Expiry_Days"))
                Dim strDecrypted As String
                strDecrypted = IONDecrypt(strPwd)
                txtPwd.Attributes.Add("value", strDecrypted)
                txtConPwd.Attributes.Add("value", strDecrypted)
                If Not IsDBNull(sqrdRecords.Item("UM_CH1_Admin_Rights")) Then
                    chkAdminRights.Checked = IIf(sqrdRecords.Item("UM_CH1_Admin_Rights") = 0, False, True)
                End If
                If Not IsDBNull(sqrdRecords.Item("UM_CH1_Mail_Comm")) Then
                    chkMailComm.Checked = IIf(sqrdRecords.Item("UM_CH1_Mail_Comm") = 0, False, True)
                End If

                DDLExpiryDays.SelectedValue = sqrdRecords.Item("UM_NU9_Expiry_Mail_Days")
                sqrdRecords.Close()

            End If

        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Error occur please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("UserManage ", "FillContact-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

    Sub fillgrid()

        Dim dtItemDetail As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtItemDetail = New DataTable
            Dim sqlQuery As String
            Dim row As DataRow

            sqlCon = New SqlConnection(SQL.DBConnection)

            sqlQuery = "Select RA_IN4_User_Role_ID_PK,ROM_VC50_Role_Name,convert(varchar,RA_DT8_Assigned_Date,101)RA_DT8_Assigned_Date,convert(varchar,RA_DT8_Valid_UpTo,101)RA_DT8_Valid_UpTo,CI_VC36_Name,RA_VC4_Status_Code from T060022,T070031,T010011 where ROM_IN4_Role_ID_PK=RA_IN4_Role_ID_FK and CI_NU8_Address_Number=RA_IN4_Assigned_By_FK and RA_IN4_AB_ID_FK=" & ViewState("User_SearchUserID").ToString & " order by RA_VC4_Status_Code desc"

            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtItemDetail)
            dtItemDetail.TableName = "T01"
            mdvtable.Table = dtItemDetail

            If dtItemDetail.Rows.Count = 0 Then
                createRow(dtItemDetail)
            Else

                Dim htDateCols As New Hashtable
                htDateCols.Add("RA_DT8_Assigned_Date", 2)
                htDateCols.Add("RA_DT8_Valid_UpTo", 2)

                SetDataTableDateFormat(mdvtable.Table, htDateCols)

                dgMenu.DataSource = mdvtable.Table
                dgMenu.DataBind()

            End If


        Catch ex As Exception
            CreateLog("UserManage ", "fillgrid-444", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Sub

    Sub createRow(ByRef dtItemDetail As DataTable)
        Dim drow As DataRow
        Try
            For inti As Int16 = 1 To 1
                drow = dtItemDetail.NewRow
                drow.Item("RA_IN4_User_Role_ID_PK") = 0
                drow.Item("ROM_VC50_Role_Name") = ""
                drow.Item("CI_VC36_Name") = ""
                drow.Item("RA_VC4_Status_Code") = ""
                dtItemDetail.Rows.Add(drow)
                dgMenu.DataSource = dtItemDetail
                dgMenu.DataBind()
            Next
        Catch ex As Exception
            CreateLog("UserManage", "createRow-444", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

    Sub myItems_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)

        Try

            Dim dv As DataView = mdvtable
            Dim dcCol As DataColumn
            Dim dc As DataColumnCollection = dv.Table.Columns
            For Each dcCol In dv.Table.Columns

            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("UserManage", "ItemDataBound-343", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Sub

#Region "Save"

    Function saveUser() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        lstError.Items.Clear()

        If CDDLUserID.CDDLGetValue.Trim = "" Then
            '            cpnlError.Visible = True
            lstError.Items.Clear()
            lstError.Items.Add("Address Book No field is blank...")
            Return False
        End If

        If txtUserID.Text = "" Then
            '            cpnlError.Visible = True
            lstError.Items.Clear()
            lstError.Items.Add("User ID field is blank...")
            Return False
        End If

        Try

            If checkUser(CDDLUserID.CDDLGetValue.Trim) Then
                '                cpnlError.Visible = True
                lstError.Items.Add("User Id for this user already exists...")
                Return False
            Else
                If checkID(txtUserID.Text.Trim) Then
                    '                    cpnlError.Visible = True
                    lstError.Items.Add("User Id already exists...")
                    Return False
                End If

                getValues(arColumnName, arRowData)
                mstGetFunctionValue = insertData(arColumnName, arRowData, "T060011")

                If mstGetFunctionValue.ErrorCode = 0 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If

        Catch ex As Exception
            lstError.Items.Add("Error occur please try later ...")
            CreateLog("CraeteUser", "SaveUser-233", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

    Function insertData(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Save(TableName, "User Manage", "insertData-601", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveUser-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Function checkUser(ByVal ABNo As String) As Boolean
        Dim intAddressID As Integer = SQL.Search("UserManage", " checkUser-620", "Select UM_IN4_Address_No_FK from T060011 where UM_IN4_Address_No_FK=" & ABNo & "")

        If intAddressID > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Function getUserCompany(ByVal userID As String) As Int32

        Return Convert.ToInt32(SQL.Search("UserManage", "getUserCompany", "select CI_IN4_Business_Relation from T010011 where CI_NU8_Address_Number = " & Val(CDDLUserID.CDDLGetValue.Trim)))

    End Function

    Function checkID(ByVal userID As String) As Boolean

        Dim intCompId As Int32 = getUserCompany(userID)
        Dim intAddressID As Integer = SQL.Search("UserManage", "checkID", "Select UM_IN4_Address_No_FK from T060011 where UM_VC50_UserID='" & userID & "'")

        If intAddressID > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Sub getValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        arColumnName.Add("UM_IN4_Address_No_FK")
        arColumnName.Add("UM_VC50_UserID")

        If Not txtPwd.Text = "" Then
            arColumnName.Add("UM_VC30_Password")
        End If

        'arColumnName.Add("UM_IN4_Expiry_Days")
        arColumnName.Add("UM_IN4_LockOut_Tries")
        arColumnName.Add("UM_CH1_IsDefault")
        arColumnName.Add("UM_VC4_User_Type_FK")
        arColumnName.Add("UM_DT8_From_date")
        arColumnName.Add("UM_DT8_To_date")
        arColumnName.Add("UM_DT8_Created_Date")
        arColumnName.Add("UM_VC4_Status_Code_FK")
        arColumnName.Add("UM_DT8_Status_Date")
        arColumnName.Add("UM_SI2_Inserted_By")
        arColumnName.Add("UM_DT8_Inserted_On")
        arColumnName.Add("UM_VC15_Inserted_By_System_Code")
        arColumnName.Add("UM_IN4_Company_AB_ID")
        arColumnName.Add("UM_CH1_Admin_Rights")
        arColumnName.Add("UM_CH1_Mail_Comm")
        arColumnName.Add("UM_NU9_Expiry_Mail_Days")
        arColumnName.Add("UM_CH1_Mail_Sent_Modify")
        arColumnName.Add("UM_CH1_Mail_Sent_Expiry")

        arRowData.Add(CInt(CDDLUserID.CDDLGetValue.Trim))
        arRowData.Add(txtUserID.Text.Trim)

        If Not txtPwd.Text = "" Then
            Dim strEncrypted As String
            strEncrypted = IONEncrypt(txtPwd.Text.Trim)
            arRowData.Add(strEncrypted)
        End If
        Dim intCompId As Int32 = getUserCompany(txtUserID.Text.Trim)
        'If txtPassExp.Text.Trim = "" Then
        '    arRowData.Add(0)
        'Else
        '    arRowData.Add(CInt(txtPassExp.Text.Trim))
        'End If

        arRowData.Add(3)
        arRowData.Add("N")
        arRowData.Add("INTR")
        arRowData.Add(dtStartdate.Text.Trim)
        If dtEndDate.Text.Trim.Equals("") Then
            Dim dbn As System.DBNull
            arRowData.Add(DBNull.Value)
        Else
            arRowData.Add(dtEndDate.Text.Trim)
        End If

        arRowData.Add(txtCreateDate.Text.Trim)
        arRowData.Add(Ddl_Status.SelectedValue)
        arRowData.Add(Now)

        arRowData.Add(CInt(insertedBy))
        arRowData.Add(insertedOn)
        arRowData.Add(systemBy)
        arRowData.Add(intCompId)
        arRowData.Add(IIf(chkAdminRights.Checked = True, "1", "0"))
        arRowData.Add(IIf(chkMailComm.Checked = True, "1", "0"))
        arRowData.Add(DDLExpiryDays.SelectedValue)
        arRowData.Add("F")
        arRowData.Add("F")
    End Sub
    Sub getRoleValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        arColumnName.Add("RA_IN4_AB_ID_FK")
        arColumnName.Add("RA_IN4_Role_ID_FK")
        arColumnName.Add("RA_DT8_Assigned_Date")
        arColumnName.Add("RA_DT8_Valid_UpTo")
        arColumnName.Add("RA_IN4_Assigned_By_FK")
        arColumnName.Add("RA_VC4_Status_Code")
        arColumnName.Add("RA_DT8_Status_Date")
        arColumnName.Add("RA_IN4_Inserted_By_FK")
        arColumnName.Add("RA_DT8_Inserted_On")
        arColumnName.Add("RA_VC15_Inserted_By_System_Code")



        arRowData.Add(CInt(CDDLUserID.CDDLGetValue.Trim))
        arRowData.Add(CDDLRole.CDDLGetValue.Trim)
        arRowData.Add(txtAssignDate.Text.Trim)
        arRowData.Add(dtrLValidUpto.Text.Trim)
        arRowData.Add(Session("PropUserID").ToString.Trim)
        arRowData.Add(ddlRLStatus.SelectedValue.Trim)
        arRowData.Add(Now)
        arRowData.Add(CInt(insertedBy))
        arRowData.Add(insertedOn)
        arRowData.Add(systemBy)
    End Sub
#End Region

#Region "Update"
    Function updateUser() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim shFlag As Short

        Try
            getValues(arColumnName, arRowData)
            mstGetFunctionValue = WSSUpdate.UpdateUserLogin(CDDLUserID.CDDLGetValue.Trim, arColumnName, arRowData)

            arColumnName.Clear()
            arRowData.Clear()

            If Not CDDLRole.CDDLGetValue = "" Then
                If txtAssignDate.Text.Trim = "" Then
                    lstError.Items.Add("Role assigned date is blank...")

                    Return False
                End If
                If dtrLValidUpto.Text.Trim = "" Then
                    lstError.Items.Add("Role valid upto date is blank...")

                    Return False
                End If

                Dim intRows As Integer
                If SQL.Search("", "", "select * from T060022 where RA_IN4_Role_ID_FK='" & CDDLRole.CDDLGetValue & "'  and RA_IN4_AB_ID_FK=" & CDDLUserID.CDDLGetValue.Trim & "", intRows) = True Then
                    lstError.Items.Add("Role already exists...")
                    shFlag = 1
                End If

                If shFlag = 1 Then
                    shFlag = 0
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return False
                    Exit Function
                End If


                getRoleValues(arColumnName, arRowData)
                clearRoleText()
                mstGetFunctionValue = insertData(arColumnName, arRowData, "T060022")

                FillRoleCDDL()
                fillgrid()
            End If

            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                Return False
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("CraeteUser", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return False
        End Try

    End Function
    Sub clearRoleText()
        CDDLRole.CDDLSetBlank()
    End Sub
#End Region

    Shared codeID As String

    Private Sub dgMenu_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgMenu.ItemCommand

        Try
            'If Not IsPostBack Then
            If e.CommandName = "select" Then
                Dim i As Integer = dgMenu.Columns.Count
                codeID = Convert.ToString(e.Item.Cells(0).Text)

                'Response.Write(codeID)
                For i = 0 To e.Item.Cells.Count - 1
                    e.Item.Cells(i).BackColor = System.Drawing.Color.LightGray
                    ' e.Item.Cells(i).ForeColor = System.Drawing.Color.WhiteSmoke
                Next
            End If
            'Else
            'posted back bind the grid to the existing record

            codeID = Convert.ToString(e.Item.Cells(1).Text)

        Catch ex As Exception
            CreateLog("CraeteUser", "dgMenu_ItemCommand-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub dgMenu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgMenu.ItemDataBound

        Try

            Dim dv As DataView = mdvtable
            Dim dcCol As DataColumn
            Dim dc As DataColumnCollection = dv.Table.Columns
            Dim strID As String
            Dim i As Integer

            For Each dcCol In dv.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    If e.Item.Cells(4).Text = "DNB" Then
                        For i = 0 To e.Item.Cells.Count - 1
                            e.Item.Cells(i).BackColor = System.Drawing.Color.LightGray
                            e.Item.Cells(i).ForeColor = System.Drawing.Color.DarkGray
                        Next
                    End If

                End If
            Next

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                strID = dgMenu.DataKeys(e.Item.ItemIndex)

                e.Item.Attributes.Add("style", "cursor:hand")
                e.Item.Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & Session("strtbname") & "', '" & e.Item.ItemIndex + 1 & "')")
                e.Item.Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "')")
            End If

        Catch ex As Exception
            CreateLog("PopSearch", "ItemDataBound-343", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub DeleteAssignedRole(ByVal RoleId As Integer)

        If SQL.Delete("usermanage", "DeleteAssignedRole", "Delete from T060022  where RA_IN4_User_Role_ID_PK=" & RoleId & "", SQL.Transaction.Serializable) = True Then

            lstError.Items.Clear()
            lstError.Items.Add("Assgined Role Deleted Successfully...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Else
            lstError.Items.Clear()
            lstError.Items.Add("Assgined Role not Deleted due to some database Problems ...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
        End If
    End Sub

    Private Function GetCompanyList() As Boolean
        Try
            Dim strSQL As String


            If WSSSearch.SearchCompanyType(Val(DDLCompany.SelectedValue)).ExtraValue = "SCM" Then
                strSQL = "select CI_NU8_Address_Number CompID, CI_IN4_Business_Relation CompType, CI_VC36_Name CompName, isnull(UC_BT1_Access,0) Access from T060041, T010011 where CI_NU8_Address_Number *= UC_NU9_Comp_ID_FK and UC_NU9_User_ID_FK=" & Val(CDDLUserID.CDDLGetValue) & " and CI_VC8_Address_Book_Type='COM' AND CI_VC8_Status='ENA'"
            Else
                strSQL = "select CI_NU8_Address_Number CompID, CI_IN4_Business_Relation CompType, CI_VC36_Name CompName, isnull(UC_BT1_Access,0) Access from T060041, T010011 where CI_NU8_Address_Number *= UC_NU9_Comp_ID_FK and UC_NU9_User_ID_FK=" & Val(CDDLUserID.CDDLGetValue) & " and CI_VC8_Address_Book_Type='COM' AND CI_VC8_Status='ENA' and CI_NU8_Address_Number=" & Val(DDLCompany.SelectedValue)
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim dsCompList As New DataSet
            If SQL.Search("T060041", "UserManage", "GetCompanyList", strSQL, dsCompList, "", "") = True Then
                grdCompany.DataSource = dsCompList.Tables(0).DefaultView
                grdCompany.DataBind()
            End If
        Catch ex As Exception
            CreateLog("UserManage", "GetCompanyList-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Private Sub grdCompany_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCompany.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If Val(e.Item.Cells(2).Text.Trim) = Val(DDLCompany.SelectedValue) Then
                If CType(e.Item.FindControl("chkAccess"), CheckBox).Checked = True Then
                    CType(e.Item.FindControl("chkAccess"), CheckBox).Enabled = False
                End If
            End If
            If WSSSearch.SearchCompanyType(Val(DDLCompany.SelectedValue)).ExtraValue = "SCM" Then
            Else
                CType(e.Item.FindControl("chkAccess"), CheckBox).Enabled = False
            End If
        End If
    End Sub

    Private Function SaveDefaultCompany() As Boolean
        Try

            Dim arrColName As New ArrayList
            Dim arrRowData As New ArrayList

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            arrColName.Add("UC_NU9_User_ID_FK")
            arrColName.Add("UC_NU9_Comp_ID_FK")
            arrColName.Add("UC_BT1_Access")
            arrColName.Add("UC_DT8_Insert_Date")
            arrColName.Add("UC_NU9_Inserted_By_FK")
            arrColName.Add("UC_VC50_Inserted_By_IP")

            arrRowData.Add(Val(CDDLUserID.CDDLGetValue))
            arrRowData.Add(Val(DDLCompany.SelectedValue))
            arrRowData.Add(True)
            arrRowData.Add(Now)
            arrRowData.Add(Session("PropUserID"))
            arrRowData.Add(Request.UserHostAddress)

            SQL.Save("T060041", "UserManage", "SaveDefaultCompany-976", arrColName, arrRowData)

        Catch ex As Exception
            CreateLog("UserManage", "SaveCompanyList-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Private Function SaveCompanyList() As Boolean
        Try

            Dim arrColName As New ArrayList
            Dim arrRowData As New ArrayList

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            arrColName.Add("UC_NU9_User_ID_FK")
            arrColName.Add("UC_NU9_Comp_ID_FK")
            arrColName.Add("UC_BT1_Access")
            arrColName.Add("UC_DT8_Insert_Date")
            arrColName.Add("UC_NU9_Inserted_By_FK")
            arrColName.Add("UC_VC50_Inserted_By_IP")
            Dim arrComp As New ArrayList
            Dim strTemp As String
            lstError.Items.Clear()
            lstError.Items.Add("You cannot remove the access for:")
            lstError.Items.Add("Company---SubCategory")
            Dim blnFlag As Boolean = False
            For Each DGI As DataGridItem In grdCompany.Items
                arrRowData = New ArrayList
                arrRowData.Add(Val(CDDLUserID.CDDLGetValue))
                arrRowData.Add(Val(DGI.Cells(2).Text.Trim))
                arrRowData.Add(CType(DGI.FindControl("chkAccess"), CheckBox).Checked)

                arrRowData.Add(Now)
                arrRowData.Add(Session("PropUserID"))
                arrRowData.Add(Request.UserHostAddress)
                If CType(DGI.FindControl("chkAccess"), CheckBox).Checked = False Then
                    strTemp = ValidateCompanyAccess(Val(DGI.Cells(2).Text.Trim))
                    If strTemp.Equals("") = False Then
                        lstError.Items.Add(DGI.Cells(1).Text.Trim & "---" & strTemp)
                        blnFlag = True
                    Else
                        If SQL.Delete("UserManage", "SaveCompanyList", "delete from T060041 where UC_NU9_Comp_ID_FK=" & Val(DGI.Cells(2).Text.Trim) & " and UC_NU9_User_ID_FK=" & Val(CDDLUserID.CDDLGetValue), SQL.Transaction.Serializable) = True Then
                            SQL.Save("T060041", "", "", arrColName, arrRowData)
                        End If
                    End If
                Else
                    If SQL.Delete("UserManage", "SaveCompanyList", "delete from T060041 where UC_NU9_Comp_ID_FK=" & Val(DGI.Cells(2).Text.Trim) & " and UC_NU9_User_ID_FK=" & Val(CDDLUserID.CDDLGetValue), SQL.Transaction.Serializable) = True Then
                        SQL.Save("T060041", "", "", arrColName, arrRowData)
                    End If
                End If

            Next
            If blnFlag = True Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("UserManage", "SaveCompanyList-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Private Function ValidateCompanyAccess(ByVal CompID As Integer) As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim dsTemp As New DataSet
            If SQL.Search("Project", "", "", "select PR_VC20_Name Project from T210011, T210012 where PR_NU9_Project_ID_Pk=PM_NU9_Project_ID_Fk and PM_NU9_Comp_ID_FK=PR_NU9_Comp_ID_FK and PM_NU9_Project_Member_ID=" & Val(CDDLUserID.CDDLGetValue) & " and PM_NU9_Comp_ID_FK=" & CompID, dsTemp, "", "") = True Then
                Dim strProjects As String = ""
                For intI As Integer = 0 To dsTemp.Tables(0).Rows.Count - 1
                    If intI = 0 Then
                        strProjects = dsTemp.Tables(0).Rows(intI).Item("Project")
                    Else
                        strProjects &= ", " & dsTemp.Tables(0).Rows(intI).Item("Project")
                    End If
                Next
                Return strProjects
            Else
                Return ""
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Sub DDLCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLCompany.SelectedIndexChanged
        '---------------------Addres numner CDDL

        'CDDLUserID.CDDLQuery = "select a.CI_NU8_Address_Number as ID,a.CI_VC36_Name as UserName,b.CI_VC36_Name as Company from T010011 a,T010011 b Where a.CI_IN4_Business_Relation=b.CI_NU8_Address_Number And a.CI_VC8_Address_Book_Type<>'COM' and a.CI_VC8_Status='ENA'  and a.CI_IN4_Business_Relation='" & DDLCompany.SelectedValue & "'"
        Dim dsTemp As New DataSet
        If SQL.Search("Project", "", "", "select T010011.CI_NU8_Address_Number as ID,T010011.CI_VC36_Name as UserName,CI_VC36_Name as Company from T010011 where CI_VC8_Address_Book_Type<>'COM' and CI_VC8_Status='ENA' and CI_IN4_Business_Relation='" & DDLCompany.SelectedValue & "' and  T010011.CI_NU8_Address_Number not in(select t060011.UM_IN4_Address_No_FK from T060011)", dsTemp, "", "") = True Then

            CDDLUserID.CDDLQuery = "select T010011.CI_NU8_Address_Number as ID,T010011.CI_VC36_Name as UserName,CI_VC36_Name as Company from T010011 where CI_VC8_Address_Book_Type<>'COM' and CI_VC8_Status='ENA' and CI_IN4_Business_Relation='" & DDLCompany.SelectedValue & "' and  T010011.CI_NU8_Address_Number not in(select t060011.UM_IN4_Address_No_FK from T060011)"

            CDDLUserID.CDDLUDC = False
            CDDLUserID.CDDLType = CustomDDL.DDLType.NonFastEntry
            CDDLUserID.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"

            CDDLUserID.CDDLFillDropDown(10, False)
            '-----------------------------------------
        Else

            lstError.Items.Clear()
            lstError.Items.Add("All Adrress Book name has already user profile..")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
        End If

    End Sub

    Private Function getRecords()
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim dsTemp As New DataSet
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        If SQL.Search("Project", "", "", "select a.CI_NU8_Address_Number as ID,a.CI_VC36_Name as UserName,b.CI_VC36_Name as Company from T010011 a,T010011 b Where a.CI_IN4_Business_Relation=b.CI_NU8_Address_Number And a.CI_VC8_Address_Book_Type<>'COM' and a.CI_VC8_Status='ENA'  and a.CI_IN4_Business_Relation='" & DDLCompany.SelectedValue & "'", dsTemp, "", "") = True Then

            Dt1 = dsTemp.Tables(0)

        End If
        If SQL.Search("Project", "", "", "select UM_IN4_Address_No_FK,UM_VC50_UserID,UName.CI_VC36_Name,case UM_VC4_User_Type_FK when 'INTR' then 'Internal' when 'EXTR' then 'External' end ,convert(varchar,UM_DT8_To_date,101) ValidUpTo,case UM_VC4_Status_Code_FK when 'ENB' then 'Enabled' when 'DNB' then 'Disabled' end ,T010011.CI_VC36_Name from T060011,T010011,T010011 UName where T010011.CI_NU8_Address_Number=T060011.UM_IN4_Company_AB_ID  AND UName.CI_NU8_Address_Number=T060011.UM_IN4_Address_no_fk   AND UM_IN4_Company_AB_ID in ((select UC_NU9_Comp_ID_FK CompID from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=12 and UC_BT1_Access=1 ) union (select 0 CompID)) AND UM_VC4_Status_Code_FK='ENB'", dsTemp, "", "") = True Then
            dt2 = dsTemp.Tables(0)
        End If
        Dt1.Merge(dt2)

    End Function

    Public Function GetUniqueRecords(ByVal UserList As DataTable, ByVal ColName1 As String, ByVal ColName2 As String) As DataTable
        Try
            Dim dt1 As New DataTable
            Dim arList1 As New ArrayList
            Dim arMachine As New ArrayList
            Dim intFlag As Integer = 0

            dt1 = UserList.Copy()

            For intI As Integer = 0 To dt1.Rows.Count - 1
                intFlag = 0

                For intJ As Integer = 0 To dt1.Rows.Count - 1
                    If dt1.Rows(intI).Item(ColName1) = dt1.Rows(intJ).Item(ColName1) And dt1.Rows(intI).Item(ColName2) = dt1.Rows(intJ).Item(ColName2) Then
                        intFlag += 1
                        If intFlag > 1 Then
                            arList1.Add(dt1.Rows(intI).Item(ColName1))
                            arMachine.Add(dt1.Rows(intI).Item(ColName2))
                            Exit For
                        End If
                    End If
                Next
            Next

            For intI As Integer = 0 To arList1.Count - 1
                For intJ As Integer = 0 To dt1.Rows.Count - 1
                    If arList1.Item(intI) = dt1.Rows(intJ).Item(ColName1) And arMachine.Item(intI) = dt1.Rows(intJ).Item(ColName2) Then
                        dt1.Rows.RemoveAt(intJ)
                        Exit For
                    End If
                Next
            Next
            Return dt1
        Catch ex As Exception
            CreateLog("UserManage", "SaveCompanyList-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
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

