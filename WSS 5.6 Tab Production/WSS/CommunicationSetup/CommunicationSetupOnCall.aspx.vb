'************************************************************************************************************
' Page                     : -  Set Communication set up on calls 
' Purpose                  : -  Create textbox on runtime based on datagrid columns
' Tables                   : -  T010011, priorityobject, PriorityUsers, T060022
' Date					   Author						Modification Date				Description
' 25/05/06			  	   Sachin Prashar               				                created        
'
' Notes: 
' Code:
'************************************************************************************************************
Imports System.Data.SqlClient
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class CommunicationSetup_CommunicationSetupOnCall
    Inherits System.Web.UI.Page
#Region "Form level Variables"
    Private mtxtUDCTypeQuery As TextBox()
    Private Shared arColWidth As New ArrayList
    Private flage As Integer
    Private mdvtable As New DataView
    Private marTextbox() As TextBox
    Private Shared mTextBox() As TextBox
    Private mintColumns As Integer
    Private mshFlag As Short
    Private Expanded2 As New PlaceHolder
    Private ii As WebControls.Unit
    Private rowvalue As Integer
    Private shF As Short
    Private flg As Short
    Public mintPageSize As Integer
    Private arColumnName As New ArrayList
    Private mblnValue As Boolean
    Private flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer

    Private strConn As String
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents ddStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblCallNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblTaskNo As System.Web.UI.WebControls.Label
    Private arRowData As New ArrayList
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'cpnlError.Visible = False
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        txtCSS(Me.Page)
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgReset.Attributes.Add("Onclick", "return formReset();")
        If Request.QueryString("CallNo") <> "" Then
            'HttpContext.Current.Session("PropCallNumber") = Request.QueryString("CallNo")
            'Session("PropCAComp") = WSSSearch.SearchCompName(Request.QueryString("Comp")).ExtraValue()
            ViewState("CallNumber") = Request.QueryString("CallNo")
            ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("Comp")).ExtraValue()
        End If
        cpnlFilds.Text = "Choose Criteria &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNumber") & " " & "  Task#  " & Request.QueryString("TaskNo") & ")"
        ' -- Disable read only controls 
        ddCompany.Enabled = False
        ddEventName.Enabled = False
        ddCallType.Enabled = False
        ddTaskType.Enabled = False
        ddProject.Enabled = False
        ddCallStatus.Enabled = False
        If Request.QueryString("TaskNo") = "" Then
            ddTaskStatus.Enabled = False
        Else
            ddTaskStatus.Enabled = False
        End If
        '-------------------------------
        If (Not Page.IsPostBack) Then
            If (Session("PropCompanyType") = "SCM") Then
                PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM'  and CI_VC8_Status='ENA'  and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")", ddCompany, "Y")
            Else
                ddCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                'PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM' and CI_NU8_Address_Number = " & Val(Session("PropCompanyType")), ddCompany, "Y")
            End If
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y'", ddEventName, "N")
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y' and Event_ID_PK<>18  and Commn_Escl not in('E')", ddEventUser, "Y")
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y' and Event_ID_PK<>18  and Commn_Escl not in('E')", ddEventFired, "Y")
            'PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,ROM_VC50_Role_Name from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB'", ddRoleName, "Y")
            'PopulateDropDownLists("select Name, Description from UDC where UDCType = 'PRIO' and ProductCode = '0'", ddPriority, "Y")
            PopulateDropDownLists("", ddTaskType, "Y")
            PopulateDropDownLists("", ddCallType, "Y")
            PopulateDropDownLists("SELECT SU_NU9_ID_PK, SU_VC50_Status_Name FROM T040081 WHERE SU_NU9_ScreenID IN (3,0 ) ", ddCallStatus, "Y", True)
            PopulateDropDownLists("SELECT SU_NU9_ID_PK, SU_VC50_Status_Name FROM T040081 WHERE SU_NU9_ScreenID in (464,0)", ddTaskStatus, "Y", True)
            'PopulateDropDownLists("Select UM_IN4_Address_No_FK, UM_VC50_UserID from T060011 where UM_IN4_Company_AB_ID = " & Session("PropCompanyID"), ddUserName, "Y")
            If WSSSearch.SearchCompanyType(Val(ViewState("CompanyID"))).ExtraValue = "SCM" Then
                PopulateDropDownLists("Select UM_IN4_Address_No_FK, upper(UM_VC50_UserID) from T060011 where UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID =" & Val(ViewState("CompanyID")) & " order by UM_VC50_UserID", ddUserName, "N")
                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,upper(ROM_VC50_Role_Name) from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB' and (ROM_IN4_Company_ID_FK=" & Val(ViewState("CompanyID")) & " or ROM_IN4_Company_ID_FK = 0) order by ROM_VC50_Role_Name", ddRoleName, "N")
            Else
                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,upper(ROM_VC50_Role_Name) + ' [' + CI_VC36_Name + ']'  from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<=getdate() and ROM_DT8_End_Date >=getdate() and ROM_VC50_Status_Code_FK = 'ENB' order by ROM_VC50_Role_Name ", ddRoleName, "N")
                Dim intProjectID As Integer = WSSSearch.SearchProjectID(ViewState("CallNumber"), ViewState("CompanyID"))
                PopulateDropDownLists("(Select UM_IN4_Address_No_FK as ID, upper(UM_VC50_UserID + ' ['+CI_VC36_Name +']') as Name from T060011,T010011 where CI_NU8_Address_Number=UM_IN4_Company_AB_ID and UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID =" & Val(ViewState("CompanyID")) & ")union(select PM_NU9_Project_Member_ID as ID, upper(A.UM_VC50_UserID +' ['+ b.CI_VC36_Name+']')  as Name from T210012,T060011 A,T010011 B where B.CI_NU8_Address_Number = a.UM_IN4_Company_AB_ID and PM_NU9_Project_Member_ID=A.UM_IN4_Address_No_FK and PM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and PM_NU9_Project_ID_Fk=" & intProjectID & ")order by Name", ddUserName, "N")
            End If
            PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")
            dtStartDate.Text = SetDateFormat(Now.ToShortDateString, mdlMain.IsTime.DateOnly)
            FillForm()
        End If
        ddEventName.SelectedValue = 18
        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                If (Request.Form("txtHidden") = "") Then
                Else
                    Session("txtHidden") = Request.Form("txtHidden")
                End If
                Select Case txthiddenImage
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            '  cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If SaveSetupRule("Setup_Rules").ErrorCode = 0 Then
                            ClearControls()
                        End If

                    Case "Ok"
                        Try
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                lstError.Items.Add("You don't have access rights to Save record...")
                                Exit Sub
                            End If
                            'End of Security Block
                            If SaveSetupRule("Setup_Rules").ErrorCode = 0 Then
                                ClearControls()
                                lstError.Items.Clear()
                                Response.Write("<script>self.opener.callrefresh();</script>")
                            End If
                            If (lstError.Items.Count > 0) Then
                            Else
                                Response.Write("<script>window.close();</script>")
                            End If
                        Catch ex As Exception
                        End Try
                    Case "Close"
                    Case "Logout"
                        LogoutWSS()
                    Case "Reset"
                    Case "Edit"
                        If (Request.Form("txtHidden") = "") Then
                        Else
                            imgOk.Visible = False
                            Dim rdCallRule As SqlDataReader
                            Dim blnStatus As Boolean
                            rdCallRule = SQL.Search("CommsetupOnCall", "load", "select Event_ID_FK, Event_User_ID_FK, Event_Fired_ID_FK, user_id, Role_id, Company_id, Project_ID, Call_Status,Task_Status, Start_Date, Stop_Date, Rule_Status, SMS_on_off, Mail_on_off, Status_Change_on_off  from Setup_rules where Table_ID =" & Val(Request.Form("txthidden")), SQL.CommandBehaviour.Default, blnStatus)
                            While rdCallRule.Read
                                ddEventName.SelectedValue = rdCallRule(0)
                                ddEventUser.SelectedValue = rdCallRule(1)
                                ddEventFired.SelectedValue = rdCallRule(2)
                                ddCompany.SelectedValue = rdCallRule(5)
                                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,ROM_VC50_Role_Name from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB'", ddRoleName, "Y")
                                ddRoleName.SelectedValue = rdCallRule(4)
                                PopulateDropDownLists("Select UM_IN4_Address_No_FK, UM_VC50_UserID from T060011 where UM_IN4_Company_AB_ID = " & ddCompany.SelectedValue.Trim, ddUserName, "Y")
                                ddUserName.SelectedValue = rdCallRule(3)
                                ddProject.SelectedValue = rdCallRule(6)
                                ddCallStatus.SelectedValue = 0
                                ddTaskStatus.SelectedIndex = 0
                                ddRecordStatus.SelectedValue = rdCallRule(11)
                                If (rdCallRule(13) = "1") Then
                                    chkIsMail.Checked = True
                                Else
                                    chkIsMail.Checked = False
                                End If
                                If (rdCallRule(12) = "1") Then
                                    chkIsSMS.Checked = True
                                Else
                                    chkIsSMS.Checked = False
                                End If
                                dtStartDate.Text = rdCallRule(9)
                                If Not IsDBNull(rdCallRule(10)) Then
                                    dtEndDate.Text = rdCallRule(10)
                                End If
                            End While
                        End If
                    Case "Update"
                        Dim isMail As Int16
                        Dim isSMS As Int16
                        If (chkIsMail.Checked) Then
                            isMail = 1
                        Else
                            isMail = 0
                        End If
                        If (chkIsSMS.Checked) Then
                            isSMS = 1
                        Else
                            isSMS = 0
                        End If
                        If (dtEndDate.Text = "") Then
                            SQL.Update("CommSetOncall", "load", "update Setup_Rules set Event_ID_FK = " & Val(ddEventName.SelectedValue) & ", Event_User_ID_FK = " & Val(ddEventUser.SelectedValue) & ", Event_Fired_ID_FK = " & Val(ddEventFired.SelectedValue) & ", user_id = " & Val(ddUserName.SelectedValue) & ", Role_id = " & Val(ddRoleName.SelectedValue) & ",  Company_id = " & Val(ddCompany.SelectedValue) & ", Status_Change_on_off = " & Val(ddStatus.SelectedValue) & ", Task_Status = " & Val(ddTaskStatus.SelectedValue) & ", Call_Status = " & Val(ddCallStatus.SelectedValue) & ", Project_ID = " & Val(ddProject.SelectedValue) & ",  Stop_Date = '" & DBNull.Value & "', SMS_on_off = " & isSMS & ", Mail_on_off = " & isMail & ",Rule_Status = " & Val(ddRecordStatus.SelectedValue) & ",Last_Modified_By_User_ID = '" & Session("PropUserID") & "',Last_Modified_Date = '" & Now.ToShortDateString & "',Last_Modified_By_IP = '" & GetIP() & "' where Table_ID = " & Val(Session("txtHidden")), SQL.Transaction.ReadCommitted)
                        Else
                            SQL.Update("CommSetupOncall", "load", "update Setup_Rules set Event_ID_FK = " & Val(ddEventName.SelectedValue) & ", Event_User_ID_FK = " & Val(ddEventUser.SelectedValue) & ", Event_Fired_ID_FK = " & Val(ddEventFired.SelectedValue) & ", user_id = " & Val(ddUserName.SelectedValue) & ", Role_id = " & Val(ddRoleName.SelectedValue) & ",  Company_id = " & Val(ddCompany.SelectedValue) & ", Status_Change_on_off = " & Val(ddStatus.SelectedValue) & ", Task_Status = " & Val(ddTaskStatus.SelectedValue) & ", Call_Status = " & Val(ddCallStatus.SelectedValue) & ", Project_ID = " & Val(ddProject.SelectedValue) & ",  Stop_Date = '" & dtEndDate.Text & "', SMS_on_off = " & isSMS & ", Mail_on_off = " & isMail & ",Rule_Status = " & Val(ddRecordStatus.SelectedValue) & ",Last_Modified_By_User_ID = '" & Session("PropUserID") & "',Last_Modified_Date = '" & Now.ToShortDateString & "',Last_Modified_By_IP = '" & GetIP() & "' where Table_ID = " & Val(Session("txtHidden")), SQL.Transaction.ReadCommitted)
                        End If

                        lstError.Items.Clear()
                        lstError.Items.Add("The rule has been updated successfully...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        ClearControls()
                        Session("txtHidden") = ""
                        imgOk.Visible = True

                        Exit Sub
                End Select
            Catch ex As Exception
            End Try
        End If
        If Not IsPostBack Then
            '*******************************************************************
            ' Purpose              : - Display data in data grid from database on load
            ' Date					   Author						Modification Date				Description
            ' 25/05/06			   Sachin Prashar		    ------------------					    Created
            '
            ' Notes:  Query Modified by RVS
            ' Code:
            '*******************************************************************
            Dim dsFromView As New DataSet
            Dim strQuery As String
            'strQuery = "Select Table_ID, Name as EventName, Event_User_Name as EventUserName, Event_Fired_ID_FK as EventFired, un.UM_VC50_UserID as UserID ,ROM_VC50_Role_Name as RoleName, ab1.CI_VC36_Name as CompanyName, Priority, Task_type, Call_Type, Status_Change_on_off as Status, Task_Status, Call_Status, Project_ID, Specific_User_id, convert(varchar,Start_Date,101) as StartDate, convert(varchar, Stop_Date,101) as StopDate, SMS_on_off as SMS, Mail_on_off as Mail, Rule_Status from eventmaster em, Setup_rules sr, t010011 ab, t010011 ab1, t070031 rn, t060011 un, t060011 un1, Eventusermaster eum where sr.Event_ID_FK=18 and sr.Event_ID_FK = em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.Inserted_By_User_ID *= ab1.CI_NU8_Address_Number and  sr.user_id *= un.UM_IN4_Address_No_FK and sr.Inserted_By_User_ID *= un1.UM_IN4_Address_No_FK and Call_No = " & Val(Session("PropCallNumber")) & " and Task_No = " & Val(Request.QueryString("TaskNo"))
            strQuery = "Select Table_ID, em.Name as EventName, Event_User_Name as EventUserName, em1.Name as EventFired, un.UM_VC50_UserID as UserID ,ROM_VC50_Role_Name as RoleName,Call_Type As CallType, Task_type as TaskType,Call_Status as CallStatus, Task_Status as TaskStatus , PR.PR_VC20_Name as Project, convert(varchar,Start_Date,101) as StartDate, convert(varchar, Stop_Date,101) as StopDate, SMS_on_off as SMS, Mail_on_off as Mail, Rule_Status as RuleStatus from eventmaster em,eventmaster em1, Setup_rules sr, t010011 ab, t010011 ab1, t070031 rn, t060011 un, t060011 un1, Eventusermaster eum,T210011 as PR where eum.Enabled='Y' and sr.Event_ID_FK=18 and sr.Event_ID_FK = em.Event_ID_PK and Event_Fired_ID_FK*=em1.Event_ID_PK and sr.Project_ID=PR.PR_NU9_Project_ID_Pk and sr.company_id=PR_NU9_Comp_ID_Fk and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.Inserted_By_User_ID *= ab1.CI_NU8_Address_Number and  sr.user_id *= un.UM_IN4_Address_No_FK and sr.Inserted_By_User_ID *= un1.UM_IN4_Address_No_FK and Call_No = " & Val(ViewState("CallNumber")) & " and PR_NU9_Comp_ID_Fk=" & ViewState("CompanyID") & " and  Task_No = " & Val(Request.QueryString("TaskNo")) & " and record_type='C'"
            Session("PropTempFirstGridRows") = 0
            If SQL.Search("T030201", "CommSetUpOnCall", "load-351", strQuery, dsFromView, "", "") Then
                mdvtable.Table = dsFromView.Tables("T030201")
                If dgCommRules.AutoGenerateColumns = False Then
                    dgCommRules.AutoGenerateColumns = True
                End If
                Dim htDateCols As New Hashtable
                htDateCols.Add("StartDate", 2)
                htDateCols.Add("StopDate", 2)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)

                dgCommRules.DataSource = mdvtable
                dgCommRules.DataBind()
                ' -- set session for total rows in first grid
                Session("PropTempFirstGridRows") = dsFromView.Tables("T030201").Rows.Count

                cpnlAD.State = CustomControls.Web.PanelState.Expanded
                cpnlAD.TitleCSS = "test"
                cpnlAD.Enabled = True
            Else
                cpnlAD.State = CustomControls.Web.PanelState.Collapsed
                cpnlAD.TitleCSS = "test2"
                cpnlAD.Enabled = False
            End If
            FormatGrid()
            GetColumns()
            CreateTextBox()
        Else
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlAD$" & arCol.Item(i)))
            Next
            FillView()
        End If
        'Security Block
        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 584
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
    End Sub
    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        If Involvements() = False Then
            Exit Sub
        End If
    End Sub
    Private Function Involvements() As Boolean
    End Function

    Private Function GetPriorityParams() As Boolean
        Dim DT As New DataTable
        Dim dtChq As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            objCon.Open()
            QueryString = "Select Object_Name,Priority from priorityobject order by priority"
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)
            If DT.Rows.Count > 0 Then
            Else
                DisplayError("Record not found...")
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            objCon.Close()
        End Try
    End Function
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub PopulateDropDownLists(ByVal sqlQuery As String, ByRef ddData As DropDownList, ByVal isOptional As Char, Optional ByVal UDC As Boolean = False)
        Dim dtDDData As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            If sqlQuery.Trim <> "" Then
                dtDDData = New DataTable
                sqlCon = New SqlConnection(SQL.DBConnection)
                sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
                sqlCon.Open()
                sqlda.Fill(dtDDData)
                ddData.DataSource = dtDDData
                ddData.DataTextField = dtDDData.Columns(1).ColumnName
                If UDC = True Then
                    ddData.DataValueField = dtDDData.Columns(1).ColumnName
                Else
                    ddData.DataValueField = dtDDData.Columns(0).ColumnName
                End If
            End If
            If (isOptional = "Y") Then
                Dim lt As New ListItem
                lt.Text = "Optional"
                lt.Value = "0"
                ddData.DataBind()
                ddData.Items.Insert(0, lt)
            Else
                Dim row As DataRow
                row = dtDDData.NewRow
                row(0) = "0"
                row(1) = "Select One"
                dtDDData.Rows.InsertAt(row, 0)
                ddData.SelectedValue = "0"
                ddData.DataBind()
            End If

        Catch ex As Exception
            CreateLog("RolePermission", "GetRoleData-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            sqlCon.Close()
        End Try
    End Sub

    Private Sub ddCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddCompany.SelectedIndexChanged
        ViewState("CompanyID") = Val(ddCompany.SelectedValue)
        PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,ROM_VC50_Role_Name from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB' and ROM_IN4_Company_ID_FK = " & ddCompany.SelectedValue.Trim, ddRoleName, "Y")
        PopulateDropDownLists("Select UM_IN4_Address_No_FK, UM_VC50_UserID from T060011 where UM_IN4_Company_AB_ID = " & ddCompany.SelectedValue.Trim, ddUserName, "Y")
        PopulateDropDownLists("Select UM_IN4_Address_No_FK, UM_VC50_UserID from T060011 where UM_IN4_Company_AB_ID = " & ddCompany.SelectedValue.Trim, ddUserName, "Y")
        PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & ViewState("CompanyID"), ddProject, "Y")
    End Sub

    Private Function SaveSetupRule(ByVal TableName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim blnStatus As Boolean
            Dim arrCallTaskType As New ArrayList
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            Dim rdCallTaskType As SqlDataReader

            rdCallTaskType = SQL.Search("Commsetuponcall", "SaveSetupRule", "select  CM_VC200_Work_Priority  from T040011 where CM_NU9_Call_No_PK = " & Val(ViewState("CallNumber")) & " and CM_NU9_Comp_ID_FK = " & Val(ViewState("CompanyID")), SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                rdCallTaskType.Read()
            End If

            arColumnName.Add("Event_ID_FK")
            arColumnName.Add("Event_User_ID_FK")
            arColumnName.Add("Event_Fired_ID_FK")
            arColumnName.Add("user_id")
            arColumnName.Add("Role_id")
            arColumnName.Add("Company_id")
            arColumnName.Add("Priority")
            arColumnName.Add("Task_type")
            arColumnName.Add("Call_Type")
            'arColumnName.Add("Status_Change_on_off")
            arColumnName.Add("Task_Status")
            arColumnName.Add("Call_Status")
            arColumnName.Add("Project_ID")
            arColumnName.Add("Specific_User_id")
            arColumnName.Add("Start_Date")
            arColumnName.Add("Stop_Date")
            arColumnName.Add("SMS_on_off")
            arColumnName.Add("Mail_on_off")
            arColumnName.Add("Rule_Status")
            arColumnName.Add("Inserted_By_User_ID")
            arColumnName.Add("Inserted_Date")
            arColumnName.Add("Inserted_By_IP")
            arColumnName.Add("Last_Modified_By_User_ID")
            arColumnName.Add("Last_Modified_Date")
            arColumnName.Add("Last_Modified_By_IP")

            arColumnName.Add("Task_No")
            arColumnName.Add("Call_No")
            arColumnName.Add("record_type")

            arRowData.Add(ddEventName.SelectedValue.Trim)
            arRowData.Add(ddEventUser.SelectedValue.Trim)
            arRowData.Add(ddEventFired.SelectedValue.Trim)
            arRowData.Add(ddUserName.SelectedValue.Trim)
            arRowData.Add(ddRoleName.SelectedValue.Trim)

            arRowData.Add(ddCompany.SelectedValue.Trim)
            arRowData.Add(rdCallTaskType(0))
            arRowData.Add(IIf(ddTaskType.SelectedValue = "0", System.DBNull.Value, ddTaskType.SelectedValue))
            arRowData.Add(IIf(ddCallType.SelectedValue = "0", System.DBNull.Value, ddCallType.SelectedValue))
            'arRowData.Add(ddStatus.SelectedValue.Trim)
            arRowData.Add(IIf(ddTaskStatus.SelectedValue.Trim = "0", System.DBNull.Value, ddTaskStatus.SelectedValue.Trim))
            arRowData.Add(IIf(ddCallStatus.SelectedValue.Trim = "0", System.DBNull.Value, ddCallStatus.SelectedValue.Trim))
            arRowData.Add(Val(ddProject.SelectedValue))
            arRowData.Add("0")
            arRowData.Add(dtStartDate.Text.Trim)

            If (dtEndDate.Text = "") Then
                arRowData.Add(DBNull.Value)
            Else
                arRowData.Add(dtEndDate.Text.Trim)
            End If

            If (chkIsSMS.Checked = True) Then
                arRowData.Add("1")
            Else
                arRowData.Add("0")
            End If

            If (chkIsMail.Checked = True) Then
                arRowData.Add("1")
            Else
                arRowData.Add("0")
            End If

            arRowData.Add(ddRecordStatus.SelectedValue.Trim)
            arRowData.Add(Session("PropUserID"))
            arRowData.Add(Now.ToShortDateString)
            arRowData.Add(GetIP())
            arRowData.Add(Session("PropUserID"))
            arRowData.Add(Now.ToShortDateString)
            arRowData.Add(GetIP())

            arRowData.Add(Val(Request.QueryString("TaskNo")))
            arRowData.Add(Val(ViewState("CallNumber")))
            arRowData.Add("C")

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If ((ddRoleName.SelectedValue = "0") And (ddUserName.SelectedValue = "0")) Then
                ' cpnlError.Visible = True
                lstError.Items.Clear()
                lstError.Items.Add("You should select atleast one option from RoleName and UserName...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                stReturn.ErrorCode = 1
                Return stReturn
                Exit Function
            End If

            If Date.Parse(dtStartDate.Text) < Now.Date Then
                lstError.Items.Clear()
                lstError.Items.Add("start date not be less than current date...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                stReturn.ErrorCode = 1
                Return stReturn
                Exit Function
            End If

            If dtEndDate.Text.Trim <> "" Then
                If Date.Parse(dtEndDate.Text) < Date.Parse(dtStartDate.Text) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("End date cannot be less than start date...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    stReturn.ErrorCode = 1
                    Return stReturn
                    Exit Function
                End If
            End If

            rdCallTaskType.Close()
            rdCallTaskType = Nothing
            If SQL.Save(TableName, "CommsetupOncall", "SaveSetupRule", arColumnName, arRowData) = False Then
                stReturn.ErrorMessage = "server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                Return stReturn
                Exit Function
            Else
                If Request.Form("txthiddenImage").ToLower = "ok" Then
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Records Saved successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return stReturn
                    Exit Function
                End If
            End If

            Return stReturn
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("server is busy please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("WWSSave", "SaveUser-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
    Private Sub PopulateCommunicationSetupGrid()
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConnection
        SQL.DBTracing = False
        Dim ds As New DataSet
        If SQL.Search("Setup_Rules", "Commsetuponcall", "PopulateCommunicationSetupGrid", "Select * from Setup_Rules", ds, "", "") = True Then
            dgCommRules.DataSource = ""
        End If
    End Sub


#Region "fill View"

    '*******************************************************************

    ' Purpose              : - Display data in data grid from database on Ispost back
    ' Date					   Author						Modification Date				Description
    ' 25/05/06			   Sachin Prashar		    ------------------					    Created
    '
    ' Notes: Query modified by RVS
    ' Code:
    '*******************************************************************

    Private Function FillView()

        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String
        Dim strQuery As String
        '  If (Request.QueryString("TaskNo") <> "") Then
        ' strQuery = "Select Table_ID, Name as EventName, Event_User_Name as EventUserName, Event_Fired_ID_FK as EventFired, un.UM_VC50_UserID as UserID ,ROM_VC50_Role_Name as RoleName, ab1.CI_VC36_Name as CompanyName, Priority, Task_type, Call_Type, Status_Change_on_off as Status, Task_Status, Call_Status, Project_ID, Specific_User_id, convert(varchar,Start_Date,101) as StartDate, convert(varchar, Stop_Date,101) as StopDate, SMS_on_off as SMS, Mail_on_off as Mail, Rule_Status from eventmaster em, Setup_rules sr, t010011 ab, t010011 ab1, t070031 rn, t060011 un, t060011 un1, Eventusermaster eum where sr.Event_ID_FK=18 and sr.Event_ID_FK = em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.Inserted_By_User_ID *= ab1.CI_NU8_Address_Number and  sr.user_id *= un.UM_IN4_Address_No_FK and sr.Inserted_By_User_ID *= un1.UM_IN4_Address_No_FK and Call_No = " & Val(Session("PropCallNumber")) & " and Task_No = " & Val(Request.QueryString("TaskNo"))
        strQuery = "Select Table_ID, em.Name as EventName, Event_User_Name as EventUserName, em1.Name as EventFired, un.UM_VC50_UserID as UserID ,ROM_VC50_Role_Name as RoleName,Call_Type As CallType, Task_type as TaskType,Call_Status as CallStatus, Task_Status as TaskStatus , PR.PR_VC20_Name as Project, convert(varchar,Start_Date,101) as StartDate, convert(varchar, Stop_Date,101) as StopDate, SMS_on_off as SMS, Mail_on_off as Mail, Rule_Status as RuleStatus from eventmaster em,eventmaster em1, Setup_rules sr, t010011 ab, t010011 ab1, t070031 rn, t060011 un, t060011 un1, Eventusermaster eum,T210011 as PR where eum.Enabled='Y' and sr.Event_ID_FK=18 and sr.Event_ID_FK = em.Event_ID_PK and Event_Fired_ID_FK*=em1.Event_ID_PK and sr.Project_ID=PR.PR_NU9_Project_ID_Pk and sr.company_id=PR_NU9_Comp_ID_Fk and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.Inserted_By_User_ID *= ab1.CI_NU8_Address_Number and  sr.user_id *= un.UM_IN4_Address_No_FK and sr.Inserted_By_User_ID *= un1.UM_IN4_Address_No_FK and Call_No = " & Val(ViewState("CallNumber")) & " and PR_NU9_Comp_ID_Fk=" & ViewState("CompanyID") & " and  Task_No = " & Val(Request.QueryString("TaskNo"))
        ' Else
        '      strQuery = "Select Table_ID, Name as EventName, Event_User_Name as EventUserName, Event_Fired_ID_FK as EventFired, un.UM_VC50_UserID as UserID ,ROM_VC50_Role_Name as RoleName, ab1.CI_VC36_Name as CompanyName, Priority, Task_type, Call_Type, Status_Change_on_off as Status, Task_Status, Call_Status, Project_ID, Specific_User_id, convert(varchar,Start_Date,101) as StartDate, convert(varchar, Stop_Date,101) as StopDate, SMS_on_off as SMS, Mail_on_off as Mail, Rule_Status from eventmaster em, Setup_rules sr, t010011 ab, t010011 ab1, t070031 rn, t060011 un, t060011 un1, Eventusermaster eum where sr.Event_ID_FK = em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.Inserted_By_User_ID *= ab1.CI_NU8_Address_Number and  sr.user_id *= un.UM_IN4_Address_No_FK and sr.Inserted_By_User_ID *= un1.UM_IN4_Address_No_FK  and Call_No = " & Val(Session("PropCallNumber"))
        ' End If
        'SQL.DBTable = "T030201"

        '--Session PropTempFirstGridRows is used to store total rows in first grid to deactivate edit button through javascript
        ' -- reinitialize session for total rows in first grid
        Session("PropTempFirstGridRows") = 0

        If SQL.Search("T030201", "CommsetupOncall", "FillView", strQuery, dsFromView, "", "") Then
            Try
                mdvtable.Table = dsFromView.Tables("T030201")
                If dgCommRules.AutoGenerateColumns = False Then
                    dgCommRules.AutoGenerateColumns = True
                End If

                Dim htDateCols As New Hashtable
                htDateCols.Add("StartDate", 2)
                htDateCols.Add("StopDate", 2)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)


                dgCommRules.DataSource = mdvtable
                dgCommRules.DataBind()
                ' -- set session for total rows in first grid
                Session("PropTempFirstGridRows") = dsFromView.Tables("T030201").Rows.Count
                FormatGrid()
                GetColumns()
                'create textbox at run time at head of the datagrid        
                CreateTextBox()

                cpnlAD.State = CustomControls.Web.PanelState.Expanded
                cpnlAD.TitleCSS = "test"
                cpnlAD.Enabled = True

            Catch ex As Exception
                CreateLog("OW Views", "Fill View", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            cpnlAD.State = CustomControls.Web.PanelState.Collapsed
            cpnlAD.TitleCSS = "test2"
            cpnlAD.Enabled = False


            'Dim ds As New DataSet
            'ds.Tables.Add("Dummy")
            'ds.Tables("Dummy").Columns.Add("LinNo")
            'ds.Tables("Dummy").Columns.Add("CallType")
            'ds.Tables("Dummy").Columns.Add("TaskType")
            'ds.Tables("Dummy").Columns.Add("SkillLevel")
            'ds.Tables("Dummy").Columns.Add("Price")
            'ds.Tables("Dummy").Columns.Add("Currency")
            'ds.Tables("Dummy").Columns.Add("ChargeBasis")

            'Dim dtRow As DataRow

            'dtRow = ds.Tables("Dummy").NewRow
            'dtRow.Item(0) = ""

            'mdvtable.Table = ds.Tables("Dummy")

            'If dgCommRules.AutoGenerateColumns = False Then
            '    dgCommRules.AutoGenerateColumns = True
            'End If
            'dgCommRules.DataSource = mdvtable.Table
            'dgCommRules.DataBind()

            'FormatGrid()

        End If

    End Function

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    '*******************************************************************

    ' Purpose              : - Create textbox on runtime based on datagrid columns
    ' Date					   Author						Modification Date				Description
    ' 25/05/06			   Sachin Prashar		    ------------------					    Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************


    Private Sub CreateTextBox()

        arColumns.Clear()

       
        Dim _textbox As TextBox
        Dim intii As Integer

        arCol.Clear()

        arCol.Add("Table_id") 'table_id
        arCol.Add("Event_ID_FK")
        arCol.Add("Event_User_ID_FK")
        arCol.Add("Event_Fired_ID_FK")
        arCol.Add("user_id")
        arCol.Add("Role_id")
        arCol.Add("Company_id")
        arCol.Add("Priority")
        arCol.Add("Task_type")
        arCol.Add("Call_Type")
        arCol.Add("Task_Status")
        arCol.Add("Call_Status")
        arCol.Add("Project_ID")
        arCol.Add("Specific_User_id")
        arCol.Add("Start_Date")
        arCol.Add("Stop_Date")
        arCol.Add("SMS_on_off")
        arCol.Add("Mail_on_off")
        arCol.Add("Rule_Status")
        arCol.Add("Inserted_By_User_ID")
        arCol.Add("Inserted_Date")
        arCol.Add("Inserted_By_IP")

        arCol.Add("Task_No")
        arCol.Add("Call_No")

        'fill the columns count into the array from mdvtable view

        Try

            intCol = 16

            If Not IsPostBack Then
                ReDim mTextBox(intCol)
            End If

            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 10
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(arCol.Item(intii))

                    If arCol.Item(intii) = "Table_id" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server"" Visible=""False"" Width=""0"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If

                    _textbox.ID = arCol.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 10
                    col1cng = col1cng & "pt"

                    If arrtextvalue.Count <> 16 Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)

                    If arCol.Item(intii) = "Table_id" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=""0"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If

                    '  _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                    _textbox.ID = arCol.Item(intii)
                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next

        Catch ex As Exception
            CreateLog("Communicationsetup", "CreateTextBox-665", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try



    End Sub

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(10) 'tableid
        arColWidth.Add(65)
        arColWidth.Add(83)
        arColWidth.Add(84)
        arColWidth.Add(82)
        arColWidth.Add(83)
        arColWidth.Add(84)
        arColWidth.Add(83)
        arColWidth.Add(84)
        'arColWidth.Add(80)
        arColWidth.Add(81)
        arColWidth.Add(82)
        arColWidth.Add(84)
        arColWidth.Add(82)
        arColWidth.Add(84)
        arColWidth.Add(86)
        arColWidth.Add(82)
        arColWidth.Add(82)
        arColWidth.Add(82)
        arColWidth.Add(82)
        arColWidth.Add(61)
        arColWidth.Add(78)
        arColWidth.Add(41)
        arColWidth.Add(41)


        Try
            dgCommRules.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1

                Dim Bound_Column As New BoundColumn

                If intI = 0 Then

                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(0)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(0)
                    Bound_Column.Visible = False
                Else
                    Dim strWidth As String = arColWidth.Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True

                End If

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                dgCommRules.Columns.Add(Bound_Column)
            Next

        Catch ex As Exception
            CreateLog("ViewJobs", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(10) 'tableid
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        'arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(77)
        arColWidth.Add(40)
        arColWidth.Add(40)

        arColumnName.Add("Table_id") 'tableid
        arColumnName.Add("Event_ID_FK")
        arColumnName.Add("Event_User_ID_FK")
        arColumnName.Add("Event_Fired_ID_FK")
        arColumnName.Add("user_id")
        arColumnName.Add("Role_id")
        arColumnName.Add("Company_id")
        arColumnName.Add("Priority")
        arColumnName.Add("Task_type")
        arColumnName.Add("Call_Type")
        'arColumnName.Add("Status_Change_on_off")
        arColumnName.Add("Task_Status")
        arColumnName.Add("Call_Status")
        arColumnName.Add("Project_ID")
        arColumnName.Add("Specific_User_id")
        arColumnName.Add("Start_Date")
        arColumnName.Add("Stop_Date")
        arColumnName.Add("SMS_on_off")
        arColumnName.Add("Mail_on_off")
        arColumnName.Add("Rule_Status")
        arColumnName.Add("Inserted_By_User_ID")
        arColumnName.Add("Inserted_Date")
        arColumnName.Add("Inserted_By_IP")
        arColumnName.Add("Task_No")
        arColumnName.Add("Call_No")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(10)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(40)
        arColWidth.Add(40)

        arColumnName.Add("Event_ID_FK")
        arColumnName.Add("Event_User_ID_FK")
        arColumnName.Add("Event_Fired_ID_FK")
        arColumnName.Add("user_id")
        arColumnName.Add("Role_id")
        arColumnName.Add("Company_id")
        arColumnName.Add("Priority")
        arColumnName.Add("Task_type")
        arColumnName.Add("Call_Type")
        'arColumnName.Add("Status_Change_on_off")
        arColumnName.Add("Task_Status")
        arColumnName.Add("Call_Status")
        arColumnName.Add("Project_ID")
        arColumnName.Add("Specific_User_id")
        arColumnName.Add("Start_Date")
        arColumnName.Add("Stop_Date")
        arColumnName.Add("SMS_on_off")
        arColumnName.Add("Mail_on_off")
        arColumnName.Add("Rule_Status")
        arColumnName.Add("Inserted_By_User_ID")
        arColumnName.Add("Task_No")
        arColumnName.Add("Call_No")

    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        Dim strRowFilterString As String = String.Empty
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

            If String.IsNullOrEmpty(strRowFilterString) = True Then
                shF = 1
                dgCommRules.Columns.Clear()
                If dgCommRules.AutoGenerateColumns = False Then
                    dgCommRules.AutoGenerateColumns = True
                End If
                dgCommRules.DataSource = mdvtable
                dgCommRules.DataBind()
                FormatGrid()
                GetColumns()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            dgCommRules.Columns.Clear()
            dgCommRules.DataSource = mdvtable
            If dgCommRules.AutoGenerateColumns = False Then
                dgCommRules.AutoGenerateColumns = True
            End If
            dgCommRules.DataBind()
            FormatGrid()
            GetColumns()
        Catch ex As Exception
            CreateLog("CommsetupOncall", "Click-1182", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub

#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub dgCommRules_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCommRules.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String
        dgCommRules.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = dgCommRules.DataKeys(e.Item.ItemIndex)

                    strTempName = e.Item.Cells(1).Text
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "','" & rowvalue & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "', '" & strTempName & "','" & strTempName & "','" & rowvalue & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("Commsetuponcall", "ItemDataBound-1211", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "dgCommRules")
        End Try
    End Sub

#End Region

    Private Sub ClearControls()
        ddRoleName.SelectedIndex = 0
        ddUserName.SelectedIndex = 0
        ddRecordStatus.SelectedIndex = 0
        chkIsMail.Checked = False
        chkIsSMS.Checked = False
        dtEndDate.Text = ""
    End Sub
    Private Sub FillForm()
        'Created By Ranvijay to fill Call or Task detail when form is opened
        Dim sqrdCall As SqlDataReader
        Dim flgstatus As Boolean
        Dim strSql As String
        Try
            If Val(Request.QueryString("TaskNo")) > 0 Then
                'SQL.DBTable = "T040021"
                strSql = "Select T040021.*,CN_VC20_Call_Status,CM_VC8_Call_Type,CM_VC50_Project,CM_NU9_Project_ID,CM_NU9_Comp_Id_FK from T040021,T040011 Where TM_NU9_Call_No_FK=CM_NU9_Call_No_PK And TM_NU9_Comp_ID_FK =CM_NU9_Comp_Id_FK " & _
                            " And TM_NU9_Call_No_FK=" & ViewState("CallNumber") & " And CM_NU9_Comp_Id_FK =" & ViewState("CompanyID") & " And TM_NU9_Task_no_PK=" & Request.QueryString("TaskNo")
            Else
                'SQL.DBTable = "T040011"
                strSql = "Select * from T040011 Where CM_NU9_Call_No_PK=" & ViewState("CallNumber") & " And CM_NU9_Comp_Id_FK =" & ViewState("CompanyID")
            End If
            sqrdCall = SQL.Search("CommunicationSetupOnCall", "FillForm-1360", strSql, SQL.CommandBehaviour.CloseConnection, flgstatus)
            If flgstatus = True Then
                sqrdCall.Read()
                'ddCallStatus.SelectedValue = sqrdCall("CN_VC20_Call_Status")
                ddCallStatus.SelectedValue = 0
                ddCompany.SelectedValue = sqrdCall("CM_NU9_Comp_Id_FK")
                ' -- Fill Project on the basis of company selected
                PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")
                ddCallType.Items.Add(sqrdCall("CM_VC8_Call_Type"))
                ddCallType.SelectedValue = sqrdCall("CM_VC8_Call_Type")
                'ddCallStatus.SelectedValue = sqrdCall("CN_VC20_Call_Status")
                ddProject.SelectedValue = sqrdCall("CM_NU9_Project_ID")
                If Val(Request.QueryString("TaskNo")) > 0 Then
                    ddTaskStatus.SelectedIndex = 0
                    ddTaskType.Items.Add(sqrdCall("TM_VC8_task_type"))
                    ddTaskType.SelectedValue = sqrdCall("TM_VC8_task_type")
                End If
            End If
            sqrdCall.Close()
            sqrdCall = Nothing
        Catch ex As Exception
            CreateLog("CommunicationSetupOnCall", "FillForm-1366", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
