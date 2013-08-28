'***********************************************************************************************************
' Page                   : - EditOverRiddenrule
' Purpose                : - Purpose of this screen is to edit the call or task alert 
'                            and events on which these alerts will fired.

' Date		    		  Author					Modification Date					Description
' 17/05/06				  Jagtar 					06/06/2006        					Created
' Tables used            : -T010011, EventMaster, EventUserMaster, T040081, T060011, T070031, T210011,                                   Setup_rules

' Notes: 
' Code:
'***********************************************************************************************************
Imports System.Data.SqlClient
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class CommunicationSetup_EditOverriddenRule
    Inherits System.Web.UI.Page
    Dim intInvid As Integer
    Private Shared dtDefaultEvent As New DataTable

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        txtCSS(Me.Page)
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")

        'dtStartDate.readOnlyDate = False

        If (Not Page.IsPostBack) Then
            If (Session("PropCompanyType") = "SCM") Then
                PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM'  and CI_VC8_Status='ENA'  and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")", ddCompany, "Y")
            Else
                ddCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))

                'PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM' and CI_NU8_Address_Number = " & Val(Session("PropCompanyType")), ddCompany, "Y")
            End If
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y'  and Commn_Escl not in('E')", ddEventName, "N")
            PopulateDropDownLists("select Event_User_ID_PK, Event_User_Name from EventUserMaster where Enabled = 'Y'", ddEventUser, "N")
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y' and Is_Default_Event = 'Y'   and Commn_Escl not in('E')", ddEventFired, "Y")
            'PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,ROM_VC50_Role_Name from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB'", ddRoleName, "Y")
            'PopulateDropDownLists("select Name, Description from UDC where UDCType = 'PRIO' and ProductCode = '0'", ddPriority, "Y")
            PopulateDropDownLists("", ddTaskType, "Y")
            PopulateDropDownLists("", ddCallType, "Y")
            PopulateDropDownLists("SELECT SU_NU9_ID_PK, SU_VC50_Status_Name FROM T040081 WHERE SU_NU9_ScreenID IN (3,0 ) ", ddCallStatus, "Y", True)
            PopulateDropDownLists("SELECT SU_NU9_ID_PK, SU_VC50_Status_Name FROM T040081 WHERE SU_NU9_ScreenID in (464,0)", ddTaskStatus, "Y", True)
            'PopulateDropDownLists("Select UM_IN4_Address_No_FK, UM_VC50_UserID from T060011 where UM_VC4_Status_Code_FK ='ENB'", ddUserName, "Y")

            If WSSSearch.SearchCompanyType(Val(Session("PropCAComp"))).ExtraValue = "SCM" Then
                PopulateDropDownLists("Select UM_IN4_Address_No_FK, upper(UM_VC50_UserID) from T060011 where UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID =" & Val(Session("PropCAComp")) & " order by UM_VC50_UserID", ddUserName, "N")
                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,upper(ROM_VC50_Role_Name) from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB' and (ROM_IN4_Company_ID_FK=" & Val(Session("PropCAComp")) & " or ROM_IN4_Company_ID_FK = 0) order by ROM_VC50_Role_Name", ddRoleName, "N")
            Else
                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,upper(ROM_VC50_Role_Name) + ' [' + CI_VC36_Name + ']'  from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<=getdate() and ROM_DT8_End_Date >=getdate() and ROM_VC50_Status_Code_FK = 'ENB' order by ROM_VC50_Role_Name ", ddRoleName, "N")
                Dim intProjectID As Integer = WSSSearch.SearchProjectID(Session("PropCallNumber"), Session("PropCAComp"))
                PopulateDropDownLists("(Select UM_IN4_Address_No_FK as ID, upper(UM_VC50_UserID + ' ['+CI_VC36_Name +']') as Name from T060011,T010011 where CI_NU8_Address_Number=UM_IN4_Company_AB_ID and UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID =" & Val(Session("PropCAComp")) & ")union(select PM_NU9_Project_Member_ID as ID, upper(A.UM_VC50_UserID +' ['+ b.CI_VC36_Name+']')  as Name from T210012,T060011 A,T010011 B where B.CI_NU8_Address_Number = a.UM_IN4_Company_AB_ID and PM_NU9_Project_Member_ID=A.UM_IN4_Address_No_FK and PM_NU9_Comp_ID_FK=" & Val(Session("PropCAComp")) & " and PM_NU9_Project_ID_Fk=" & intProjectID & ")order by Name", ddUserName, "N")
            End If
            ' -- SubCategory
            PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")

            dtStartDate.Text = SetDateFormat(Now.ToShortDateString, mdlMain.IsTime.DateOnly)
            GetEventStatus()
            EnableDisableDD()
        End If

        If Request.QueryString("InvId") = "" Then
            intInvid = 0
            ' Exit Sub
        Else
            intInvid = Request.QueryString("InvId")

        End If
        If Not IsPostBack Then
            ddCallType.Enabled = False
            ddTaskType.Enabled = False
            ddCallStatus.Enabled = False
            ddTaskStatus.Enabled = False
            Filldata()
        End If
    End Sub
    Private Function GetEventStatus() As Boolean
        Dim dtChq As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strConn As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            objCon.Open()
            QueryString = "Select Event_ID_PK from EventMaster where Is_Default_Event = 'Y'"
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(dtDefaultEvent)
        Catch ex As Exception

            CreateLog("ComSetup", "ShowValues-451", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        Finally
            objCon.Close()
        End Try
    End Function

    Private Sub Filldata()
        Dim blnStatus As Boolean
        Dim rdCallRule As SqlDataReader
        If intInvid = 0 Then
        Else

            Try
                rdCallRule = SQL.Search("editoverriddenrule", "Filldata", "select Mail_on_off, SMS_on_off, Start_Date, Stop_Date,  Event_ID_FK,  Event_User_ID_FK, Event_Fired_ID_FK, user_id, Role_id, Priority, Call_Type, Call_Status, Task_type, Task_Status, Project_ID, Company_id, Rule_Status,Call_NO,Task_NO from Setup_rules where Table_ID =" & intInvid, SQL.CommandBehaviour.Default, blnStatus)
                While rdCallRule.Read

                    If (rdCallRule(0) = "1") Then
                        chkIsMail.Checked = True
                    Else
                        chkIsMail.Checked = False
                    End If
                    If (rdCallRule(1) = "1") Then
                        chkIsSMS.Checked = True
                    Else
                        chkIsSMS.Checked = False
                    End If

                    If rdCallRule(4) = "0" Then
                        ddEventName.SelectedIndex = 0
                    Else
                        ddEventName.SelectedValue = rdCallRule(4)
                    End If

                    lblCallNo.Text = IIf(IsDBNull(rdCallRule("Call_No")), "", "Call NO : " & rdCallRule("Call_No"))
                    lblTaskNo.Text = IIf(rdCallRule("Task_No") = "0", "", "Task NO : " & rdCallRule("Task_No"))

                    If IsDBNull(rdCallRule(2)) Then
                    Else
                        dtStartDate.Text = SetDateFormat(IIf(IsDBNull(rdCallRule(2)), "", rdCallRule(2)), mdlMain.IsTime.DateOnly)
                    End If
                    If IsDBNull(rdCallRule(3)) Then
                    Else
                        dtEndDate.Text = SetDateFormat(IIf(IsDBNull(rdCallRule(3)), "", rdCallRule(3)), mdlMain.IsTime.DateOnly)
                    End If


                    If IsDBNull(rdCallRule(5)) Then
                        ddEventUser.SelectedIndex = 0
                    Else
                        ddEventUser.SelectedValue = rdCallRule(5)
                    End If

                    If IsDBNull(rdCallRule(6)) Then
                        ddEventFired.SelectedIndex = 0
                    Else
                        ddEventFired.SelectedValue = rdCallRule(6)
                    End If

                    If IsDBNull(rdCallRule(7)) Then
                        ddUserName.SelectedIndex = 0
                    Else
                        Dim str As String = rdCallRule(7)
                        ddUserName.SelectedValue = rdCallRule(7)
                    End If

                    If IsDBNull(rdCallRule(8)) Then
                        ddRoleName.SelectedIndex = 0
                    Else
                        ddRoleName.SelectedValue = rdCallRule(8)
                    End If

                    If IsDBNull(rdCallRule("Company_id")) Then
                        ddCompany.SelectedIndex = 0
                    Else
                        ddCompany.SelectedValue = rdCallRule("Company_id")
                    End If
                    ' -- Fill SubCategory on the basis of Company
                    PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")
                    If IsDBNull(rdCallRule(14)) Then
                        ddProject.SelectedIndex = 0
                    Else
                        ddProject.SelectedValue = rdCallRule(14)
                    End If

                    If IsDBNull(rdCallRule(16)) Then
                        ddRecordStatus.SelectedIndex = 0
                    Else
                        ddRecordStatus.SelectedValue = rdCallRule(16)
                    End If

                    If IsDBNull(rdCallRule(11)) Then
                        ddCallStatus.SelectedIndex = 0
                    Else
                        'ddCallStatus.SelectedValue = rdCallRule(11)
                        ddCallStatus.SelectedIndex = 0
                    End If

                    'If rdCallRule(9) = "0" Then
                    '    ddPriority.SelectedIndex = 0
                    'Else
                    '    ddPriority.SelectedValue = rdCallRule(9)
                    'End If

                    If IsDBNull(rdCallRule(10)) Then
                        ddCallType.SelectedIndex = 0
                    Else
                        ddCallType.Items.Add(rdCallRule(10))
                        ddCallType.SelectedValue = rdCallRule(10)
                    End If

                    If IsDBNull(rdCallRule(12)) Then
                        ddTaskType.SelectedIndex = 0
                    Else
                        ddTaskType.Items.Add(rdCallRule(12))
                        ddTaskType.SelectedValue = rdCallRule(12)
                    End If


                    If IsDBNull(rdCallRule(13)) Then
                        ddTaskStatus.SelectedIndex = 0
                    Else
                        ddTaskStatus.SelectedIndex = 0
                        'ddTaskStatus.SelectedValue = rdCallRule(13)
                    End If

                End While
            Catch ex As Exception
                CreateLog("EditOverriddenRule", "FillData-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try

        End If
    End Sub
    Private Function PopulateDropDownLists(ByVal sqlQuery As String, ByRef ddData As DropDownList, ByVal isOptional As Char, Optional ByVal UDC As Boolean = False)

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
            CreateLog("Editoveriddenrule", "GetRoleData-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            sqlCon.Close()
        End Try

    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Dim isMail As Int16
        Dim isSMS As Int16

        If dtEndDate.Text.Trim <> "" Then
            If Date.Parse(dtEndDate.Text) < Date.Parse(dtStartDate.Text) Then
                lstError.Items.Clear()
                lstError.Items.Add("Stop date cannot be less than Start date...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        End If


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
        Dim dbNull As DBNull
        Dim arrColumns As New ArrayList
        Dim arrRowData As New ArrayList

        Try
            ' SQL.DBTable = "Setup_Rules"
            arrColumns.Add("Call_Type")
            arrColumns.Add("Task_type")
            arrColumns.Add("Event_ID_FK")
            arrColumns.Add("Event_User_ID_FK")
            arrColumns.Add("Event_Fired_ID_FK")
            arrColumns.Add("user_id")
            arrColumns.Add("Role_id")
            arrColumns.Add("Company_id")
            arrColumns.Add("Task_Status")
            arrColumns.Add("Call_Status")
            arrColumns.Add("Project_ID")
            arrColumns.Add("Stop_Date")
            arrColumns.Add("SMS_on_off")
            arrColumns.Add("Mail_on_off")
            arrColumns.Add("Rule_Status")
            arrColumns.Add("Last_Modified_By_User_ID")
            arrColumns.Add("Last_Modified_Date")
            arrColumns.Add("Last_Modified_By_IP")


            arrRowData.Add(IIf(ddCallType.SelectedValue.Trim = "0", dbNull.Value, ddCallType.SelectedValue))
            arrRowData.Add(IIf(ddTaskType.SelectedValue.Trim = "0", dbNull.Value, ddTaskType.SelectedValue))
            arrRowData.Add(Val(ddEventName.SelectedValue))
            arrRowData.Add(Val(ddEventUser.SelectedValue))
            arrRowData.Add(Val(ddEventFired.SelectedValue))
            arrRowData.Add(Val(ddUserName.SelectedValue))
            arrRowData.Add(Val(ddRoleName.SelectedValue))
            arrRowData.Add(Val(ddCompany.SelectedValue))
            arrRowData.Add(IIf(ddTaskStatus.SelectedValue = "0", dbNull.Value, ddTaskStatus.SelectedValue))
            arrRowData.Add(IIf(ddCallStatus.SelectedValue = "0", dbNull.Value, ddCallStatus.SelectedValue))
            arrRowData.Add(Val(ddProject.SelectedValue))
            If (dtEndDate.Text = "") Then
                arrRowData.Add(dbNull.Value)
            Else
                arrRowData.Add(dtEndDate.Text)
            End If
            arrRowData.Add(isSMS)
            arrRowData.Add(isMail)
            arrRowData.Add(Val(ddRecordStatus.SelectedValue))
            arrRowData.Add(Session("PropUserID"))
            arrRowData.Add(Now.ToShortDateString)
            arrRowData.Add(GetIP())
            If SQL.Update("Setup_Rules", "CommunicationSetupRule", "imgSave_Click", "Select * from setup_rules where Table_ID = " & intInvid, arrColumns, arrRowData, ) Then

                Filldata()

                'Save Message
                lstError.Items.Clear()
                lstError.Items.Add("The rule has been updated successfully...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("The rule Could not be updated...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("EditoverriddenRule", "imgSave_Click", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Sub ddEventFired_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddEventFired.SelectedIndexChanged
        If (ddEventFired.SelectedValue <> "0") Then
            'ddRoleName.Enabled = False
            'ddUserName.Enabled = False
            ddTaskType.Enabled = False
            ddCallType.Enabled = False
            ddProject.Enabled = False
            ddCallStatus.Enabled = False
            ddTaskStatus.Enabled = False
            ddRecordStatus.Enabled = False
            'ddStatus.Enabled = False
        Else
            'ddRoleName.Enabled = True
            'ddUserName.Enabled = True
            ddTaskType.Enabled = True
            ddCallType.Enabled = True
            'ddProject.Enabled = True  // commented acc to requirement
            ddCallStatus.Enabled = True
            ddTaskStatus.Enabled = False
            ddRecordStatus.Enabled = True
            'ddStatus.Enabled = True
        End If
        'Filldata()
    End Sub
    Private Sub EnableDisableDD()
        Dim dv As DataView
        dv = dtDefaultEvent.DefaultView
        dv.Sort = "Event_ID_PK"
        If dv.Find(ddEventName.SelectedValue) <> -1 Then
            ddCallStatus.Enabled = False
            ddCallType.Enabled = False
            'ddEventName.Enabled = True
            ddProject.Enabled = False
            ddRecordStatus.Enabled = False
            ddEventFired.Enabled = False
            ddRoleName.Enabled = False
            ddEventUser.Enabled = True
            'ddCompany.Enabled = True // commented acc to requirement
            'ddStatus.Enabled = True
            ddTaskStatus.Enabled = False
            ddTaskType.Enabled = False
            ddUserName.Enabled = False
            ddCallStatus.SelectedIndex = "0"
            ddCallType.SelectedIndex = "0"
            ddProject.SelectedIndex = "0"
            ddEventFired.SelectedIndex = "0"
            ddRoleName.SelectedIndex = "0"
            ddTaskStatus.SelectedIndex = "0"
            ddTaskType.SelectedIndex = "0"
            ddUserName.SelectedIndex = "0"
        Else
            ddEventUser.SelectedIndex = "0"
            ddCallStatus.Enabled = False
            'ddCallStatus.Enabled = True
            ddCallType.Enabled = False
            'ddEventName.Enabled = True
            'ddProject.Enabled = True // commented acc to requirement
            ddRecordStatus.Enabled = True
            ddEventFired.Enabled = True
            ddRoleName.Enabled = True
            ddEventUser.Enabled = False
            'ddCompany.Enabled = True // commented acc to requirement
            '            ddStatus.Enabled = True
            ddTaskStatus.Enabled = False
            ddTaskType.Enabled = False
            ddUserName.Enabled = True

        End If


    End Sub
    Private Sub ddEventName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddEventName.SelectedIndexChanged
        EnableDisableDD()
    End Sub

    Private Sub imgClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Write("<script>window.close();</script>")
    End Sub

    Private Sub imgOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOk.Click
        Dim isMail As Int16
        Dim isSMS As Int16
        Dim arrColumns As New ArrayList
        Dim arrRowData As New ArrayList

        If dtEndDate.Text.Trim <> "" Then
            If Date.Parse(dtEndDate.Text) < Date.Parse(dtStartDate.Text) Then
                lstError.Items.Clear()
                lstError.Items.Add("Stop date cannot be less than Start date...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        End If



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
        Dim dbNull As DBNull


        Try
            'SQL.DBTable = "Setup_Rules"
            arrColumns.Add("Call_Type")
            arrColumns.Add("Task_type")
            arrColumns.Add("Event_ID_FK")
            arrColumns.Add("Event_User_ID_FK")
            arrColumns.Add("Event_Fired_ID_FK")
            arrColumns.Add("user_id")
            arrColumns.Add("Role_id")
            arrColumns.Add("Company_id")
            arrColumns.Add("Task_Status")
            arrColumns.Add("Call_Status")
            arrColumns.Add("Project_ID")
            arrColumns.Add("Stop_Date")
            arrColumns.Add("SMS_on_off")
            arrColumns.Add("Mail_on_off")
            arrColumns.Add("Rule_Status")
            arrColumns.Add("Last_Modified_By_User_ID")
            arrColumns.Add("Last_Modified_Date")
            arrColumns.Add("Last_Modified_By_IP")

            arrRowData.Add(IIf(ddCallType.SelectedValue.Trim = "0", dbNull.Value, ddCallType.SelectedValue))
            arrRowData.Add(IIf(ddTaskType.SelectedValue.Trim = "0", dbNull.Value, ddTaskType.SelectedValue))
            arrRowData.Add(Val(ddEventName.SelectedValue))
            arrRowData.Add(Val(ddEventUser.SelectedValue))
            arrRowData.Add(Val(ddEventFired.SelectedValue))
            arrRowData.Add(Val(ddUserName.SelectedValue))
            arrRowData.Add(Val(ddRoleName.SelectedValue))
            arrRowData.Add(Val(ddCompany.SelectedValue))
            arrRowData.Add(IIf(ddTaskStatus.SelectedValue = "0", dbNull.Value, ddTaskStatus.SelectedValue))
            arrRowData.Add(IIf(ddCallStatus.SelectedValue = "0", dbNull.Value, ddCallStatus.SelectedValue))
            arrRowData.Add(Val(ddProject.SelectedValue))
            If (dtEndDate.Text = "") Then
                arrRowData.Add(dbNull.Value)
            Else
                arrRowData.Add(dtEndDate.Text)
            End If
            arrRowData.Add(isSMS)
            arrRowData.Add(isMail)
            arrRowData.Add(Val(ddRecordStatus.SelectedValue))
            arrRowData.Add(Session("PropUserID"))
            arrRowData.Add(Now.ToShortDateString)
            arrRowData.Add(GetIP())
            If SQL.Update("Setup_Rules", "CommunicationSetupRule", "imgSave_Click", "Select * from setup_rules where Table_ID = " & intInvid, arrColumns, arrRowData, ) Then
                Filldata()

                'Save Message
                lstError.Items.Clear()
                lstError.Items.Add("The rule has been updated successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("The rule has been Could not be updated")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If
            Response.Write("<script>window.close();</script>")
        Catch ex As Exception
            CreateLog("EditoverriddenRule", "imgOK_Click", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Sub ddCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCompany.SelectedIndexChanged
        Session("PropCAComp") = Val(ddCompany.SelectedValue)
        ' -- SubCategory
        PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Session("PropCAComp"), ddProject, "Y")
    End Sub
End Class
