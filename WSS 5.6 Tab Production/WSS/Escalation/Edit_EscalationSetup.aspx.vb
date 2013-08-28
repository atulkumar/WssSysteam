#Region "Purpose"
'***********************************************************************************************************
' Page                 : - Edit Escalation setup
' Purpose              : - It will used to edit the setup rules
' Date		    			Author						Modification Date					Description
' 28/07/06				    Jagtar 					        					            Created
'
' Note:          
' Code:
'************************************************************************************************************
#End Region

#Region "Session Used"
'Session("PropCompany")
'Session("PropCompanyID")
'Session("PropCompanyType")                      
'Session("PropCompanyID")
'Session("PropUserID")
'Session("PropRootDir")
'Session("PropUserName")
'Session("UNameComn") replace with ViewState("EscEdit_UserName")
'Session("RNameComn")replace with ViewState("EscEdit_RoleName")
#End Region

#Region "NameSpace"
Imports System.Data.SqlClient
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
#End Region

Partial Class Escalation_Edit_EscalationSetup
    Inherits System.Web.UI.Page

#Region "Varibles"
    Private intInvid As Integer
    Private Shared dtDefaultEvent As New DataTable
    Private arRowData As New ArrayList
    Private arColumnName As New ArrayList
    Private intEscaltionTime, intEscalationFreq As Int32
#End Region

#Region "Page_Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        txtTime.Attributes.Add("onkeypress", "NumericOnly();")
        txtFreq.Attributes.Add("onkeypress", "NumericOnly();")
        lstError.Items.Clear()
        If (Not Page.IsPostBack) Then
            txtCSS(Me.Page)
            If (Session("PropCompanyType") = "SCM") Then
                PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM'  and CI_VC8_Status='ENA'  and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")", ddCompany, "Y")
            Else
                ddCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
            End If
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y' and Commn_Escl not in('C')", ddEventName, "N")
            PopulateDropDownLists("select Event_User_ID_PK, Event_User_Name from EventUserMaster where Enabled = 'Y'", ddEventUser, "N")
            PopulateDropDownLists("select Event_ID_PK, Name from EventMaster where Enabled = 'Y' and Is_Default_Event = 'Y' and Commn_Escl not in('C')", ddEventFired, "Y")
            PopulateDropDownLists("select Name, Description from UDC where UDCType = 'PRIO' and ProductCode = '0'", ddPriority, "Y")
            PopulateDropDownLists("select Name, Description from UDC where UDCType = 'TKTY' and ProductCode = '0'", ddTaskType, "Y")
            PopulateDropDownLists("select Name, Description from UDC where UDCType = 'CALL' and ProductCode = '0'", ddCallType, "Y")
            PopulateDropDownLists("SELECT  SU_NU9_ID_PK, SU_VC50_Status_Name FROM T040081 WHERE  SU_NU9_ScreenID in (3,0)", ddCallStatus, "Y", True)
            PopulateDropDownLists("SELECT  SU_NU9_ID_PK, SU_VC50_Status_Name FROM T040081 WHERE  SU_NU9_ScreenID in (464,0)", ddTaskStatus, "Y", True)
            If (Session("PropCompanyType") = "SCM") Then
                PopulateDropDownLists("Select UM_IN4_Address_No_FK,upper( UM_VC50_UserID + ' ['+CI_VC36_Name + ']') UM_VC50_UserID, UM_IN4_Company_AB_ID from T060011 ,T010011 where UM_IN4_Company_AB_ID=CI_NU8_Address_Number and UM_VC4_Status_Code_FK ='ENB' order by UM_VC50_UserID", ddUserName, "N")
                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,ROM_VC50_Role_Name,ROM_IN4_Company_ID_FK from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB' order by ROM_VC50_Role_Name", ddRoleName, "N")
            Else
                PopulateDropDownLists("select ROM_IN4_Role_ID_PK ,ROM_VC50_Role_Name,ROM_IN4_Company_ID_FK from T070031, T010011 where CI_NU8_Address_Number =* ROM_IN4_Company_ID_FK and ROM_DT8_Start_Date<='" & Now & "' and ROM_DT8_End_Date >='" & Now & "' and ROM_VC50_Status_Code_FK = 'ENB' and (ROM_IN4_Company_ID_FK=" & Val(Session("PropCompanyID")) & " or ROM_IN4_Company_ID_FK = 0) order by ROM_VC50_Role_Name", ddRoleName, "N")
                PopulateDropDownLists("Select UM_IN4_Address_No_FK,upper( UM_VC50_UserID + ' ['+CI_VC36_Name + ']') UM_VC50_UserID, UM_IN4_Company_AB_ID from T060011 ,T010011 where UM_IN4_Company_AB_ID=CI_NU8_Address_Number and UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID = " & Val(Session("PropCompanyID")) & " order by UM_VC50_UserID", ddUserName, "N")
            End If
            ' -- SubCategory
            PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")
            dtStartDate.Text = SetDateFormat(Now.ToShortDateString, mdlMain.IsTime.DateOnly)
            GetEventStatus()
        End If

        If Request.QueryString("InvId") = "" Then
            intInvid = 0
            ' Exit Sub
        Else
            intInvid = Request.QueryString("InvId")
        End If
        If Not IsPostBack Then
            Filldata()
        End If
        lblRuleNo.Text = "Rule No." & intInvid
        EnableDisableDD()
        imgReset.Attributes.Add("Onclick", "return formReset();")
        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            'intID = Request.QueryString("ScrID")
            intID = 605
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
    End Sub
#End Region

#Region "Functions"
#Region "GetEventStatus"
    Private Function GetEventStatus() As Boolean

        If IsNothing(HttpContext.Current.Cache("EventStatus")) Then
            Dim dtChq As New DataTable
            Dim DA As SqlDataAdapter
            Dim QueryString As String
            Dim objCommand As SqlCommand
            Dim strConn = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
            Dim objCon As SqlConnection = strConn
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
            HttpContext.Current.Cache.Insert("EventStatus", dtDefaultEvent, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
        Else

            dtDefaultEvent = Cache.Item("EventStatus")
        End If
    End Function
#End Region

#Region "Filldata"
    Private Sub Filldata()
        Dim blnStatus As Boolean
        Dim rdCallRule As SqlDataReader
        Try
            If intInvid = 0 Then
            Else

                rdCallRule = SQL.Search("Escalationsetup", "Filldata-201", "select Mail_on_off, SMS_on_off, Start_Date, Stop_Date,  Event_ID_FK,  Event_User_ID_FK, Event_Fired_ID_FK, user_id, Role_id, Priority, Call_Type, Call_Status, Task_type, Task_Status, Project_ID, Company_id, Rule_Status, Escalation_Time, Mail_Frequency from Setup_rules where Table_ID =" & intInvid, SQL.CommandBehaviour.Default, blnStatus)
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
                    txtTime.Text = IIf(IsDBNull(rdCallRule(17)), "", rdCallRule(17))
                    txtFreq.Text = IIf(IsDBNull(rdCallRule(18)), "", rdCallRule(18))

                    If IsDBNull(rdCallRule(2)) Then
                    Else
                        dtStartDate.Text = SetDateFormat(IIf(IsDBNull(rdCallRule(2)), "", rdCallRule(2)), mdlMain.IsTime.DateOnly)
                    End If


                    If IsDBNull(rdCallRule(3)) Then
                    Else
                        dtEndDate.Text = SetDateFormat(IIf(IsDBNull(rdCallRule(3)), "", rdCallRule(3)), mdlMain.IsTime.DateOnly)
                    End If

                    If IsDBNull(rdCallRule(4)) Then
                        ddEventName.SelectedIndex = 0
                    Else
                        ddEventName.SelectedValue = rdCallRule(4)
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



                    If IsDBNull(rdCallRule(8)) Then
                        ddRoleName.SelectedIndex = 0
                    Else
                        ddRoleName.SelectedValue = rdCallRule(8)
                    End If

                    If IsDBNull(rdCallRule(9)) Then
                        ddPriority.SelectedIndex = 0
                    Else
                        ddPriority.SelectedValue = rdCallRule(9)
                    End If
                    If IsDBNull(rdCallRule(10)) Then
                        ddCallType.SelectedIndex = 0
                    Else
                        ddCallType.SelectedValue = rdCallRule(10)
                    End If
                    If IsDBNull(rdCallRule(11)) Then
                        ddCallStatus.SelectedIndex = 0
                    Else
                        ddCallStatus.SelectedValue = rdCallRule(11)
                    End If
                    If IsDBNull(rdCallRule(12)) Then
                        ddTaskType.SelectedIndex = 0
                    Else
                        ddTaskType.SelectedValue = rdCallRule(12)
                    End If
                    If IsDBNull(rdCallRule(13)) Then
                        ddTaskStatus.SelectedIndex = 0
                    Else
                        ddTaskStatus.SelectedValue = rdCallRule(13)
                    End If

                    If IsDBNull(rdCallRule("Company_id")) Then
                        ddCompany.SelectedIndex = 0
                    Else
                        ddCompany.SelectedValue = rdCallRule("Company_id")
                    End If
                    'Fill user list based on company
                    If ddCompany.SelectedIndex > 0 Then
                        If (Session("PropCompanyType") = "SCM") Then

                            PopulateDropDownLists("Select UM_IN4_Address_No_FK,upper( UM_VC50_UserID + ' ['+CI_VC36_Name + ']') UM_VC50_UserID, UM_IN4_Company_AB_ID from T060011 ,T010011 where UM_IN4_Company_AB_ID=CI_NU8_Address_Number and UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID=" & Val(ddCompany.SelectedValue) & " order by UM_VC50_UserID", ddUserName, "N")

                        End If
                    End If
                    If IsDBNull(rdCallRule(7)) Then
                        ddUserName.SelectedIndex = 0
                    Else
                        Dim str As String = rdCallRule(7)
                        ddUserName.SelectedValue = rdCallRule(7)
                    End If
                    ' -- Fill SubCategory on the basis of company
                    ' -- SubCategory
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

                End While
            End If
        Catch ex As Exception
            CreateLog("edit_ComSetup", "GetRoleData-255", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "PopulateDropDownLists"
    Private Sub PopulateDropDownLists(ByVal sqlQuery As String, ByRef ddData As DropDownList, ByVal isOptional As Char, Optional ByVal udc As Boolean = False)

        Dim dtDDData As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtDDData = New DataTable
            sqlCon = New SqlConnection(SQL.DBConnection)
            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtDDData)
            ddData.DataSource = dtDDData
            ddData.DataTextField = dtDDData.Columns(1).ColumnName
            If udc = True Then
                ddData.DataValueField = dtDDData.Columns(1).ColumnName
            Else
                ddData.DataValueField = dtDDData.Columns(0).ColumnName
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
                If ddData.ID = "ddUserName" Then
                    If dtDDData.Rows.Count > 0 Then
                        dtDDData.Rows(0).Delete()
                    End If
                    ''Session.Add("UNameComn", dtDDData)
                    ViewState.Add("EscEdit_UserName", dtDDData)
                ElseIf ddData.ID = "ddRoleName" Then
                    If dtDDData.Rows.Count > 0 Then
                        dtDDData.Rows(0).Delete()
                    End If
                    ''Session.Add("RNameComn", dtDDData)
                    ViewState.Add("EscEdit_RoleName", dtDDData)
                End If

            End If
            'ddData.DataBind()
        Catch ex As Exception
            CreateLog("RolePermission", "GetRoleData-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            sqlCon.Close()
        End Try

    End Sub
#End Region
#Region "EnableDisableDD"
    Private Sub EnableDisableDD()
        Try

            Dim dv As DataView
            dv = dtDefaultEvent.DefaultView
            dv.Sort = "Event_ID_PK"
            If dv.Find(ddEventName.SelectedValue) <> -1 Then
                ddCallStatus.Enabled = False
                ddCallType.Enabled = False
                ddPriority.Enabled = False
                ddProject.Enabled = False
                ddEventFired.Enabled = False
                ddRoleName.Enabled = False
                ddEventUser.Enabled = True
                ddCompany.Enabled = True
                ddTaskStatus.Enabled = False
                ddTaskType.Enabled = False
                ddUserName.Enabled = False
                ddCallStatus.SelectedIndex = "0"
                ddCallType.SelectedIndex = "0"
                ddPriority.SelectedIndex = "0"
                ddProject.SelectedIndex = "0"
                ddEventFired.SelectedIndex = "0"
                ddRoleName.SelectedIndex = "0"
                ddTaskStatus.SelectedIndex = "0"
                ddTaskType.SelectedIndex = "0"
                ddUserName.SelectedIndex = "0"
            ElseIf dv.Find(ddEventName.SelectedValue) = -1 Then
                ddEventUser.SelectedIndex = "0"
                ddCallStatus.Enabled = True
                ddCallType.Enabled = True
                ddPriority.Enabled = True
                ddProject.Enabled = True
                ddEventFired.Enabled = True
                ddRoleName.Enabled = True
                ddEventUser.Enabled = False
                ddCompany.Enabled = True
                ddTaskStatus.Enabled = True
                ddTaskType.Enabled = True
                ddUserName.Enabled = True
            End If
        Catch ex As Exception
            CreateLog("RolePermission", "EnableDisableDD-469", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
#End Region

#Region "GetEventStatus"

    Private Sub ddEventName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddEventName.SelectedIndexChanged
        EnableDisableDD()
    End Sub
#End Region

#Region "isExisted"

    Private Function isExisted() As Boolean
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strConn As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim objCon As SqlConnection = New SqlConnection(strConn)
        Dim obj As New Object
        Try
            QueryString = "Select count(*) from setup_rules where " _
                    & " Event_ID_FK=" & ddEventName.SelectedValue.Trim _
                    & " and Event_User_ID_FK=" & ddEventUser.SelectedValue.Trim _
                    & " and Event_Fired_ID_FK=" & ddEventFired.SelectedValue.Trim _
                    & " and user_id=" & ddUserName.SelectedValue.Trim _
                    & " and Role_id=" & ddRoleName.SelectedValue.Trim _
                    & " and Company_id=" & ddCompany.SelectedValue.Trim
            If (ddPriority.SelectedValue.Trim = "0") Then
                QueryString = QueryString & " and Priority is null"
            Else
                QueryString = QueryString & " and Priority ='" & ddPriority.SelectedValue.Trim & "'"
            End If

            If (ddTaskType.SelectedValue.Trim = "0") Then
                QueryString = QueryString & " and Task_type is null"
            Else
                QueryString = QueryString & " and Task_type='" & ddTaskType.SelectedValue.Trim & "'"
            End If

            If (ddCallType.SelectedValue.Trim = "0") Then
                QueryString = QueryString & " and Call_Type is null"
            Else
                QueryString = QueryString & " and Call_Type='" & ddCallType.SelectedValue.Trim & "'"
            End If

            If (ddTaskStatus.SelectedValue.Trim = "0") Then
                QueryString = QueryString & " and Task_Status is null"
            Else
                QueryString = QueryString & " and Task_Status='" & ddTaskStatus.SelectedValue.Trim & "'"
            End If

            If (ddCallStatus.SelectedValue.Trim = "0") Then
                QueryString = QueryString & " and Call_Status is null"
            Else
                QueryString = QueryString & " and Call_Status='" & ddCallStatus.SelectedValue.Trim & "'"
            End If

            If (ddProject.SelectedValue.Trim = "0") Then
                QueryString = QueryString & " and Project_ID=" & ddProject.SelectedValue.Trim
            End If

            If (ddEventName.SelectedValue = 8 Or ddEventName.SelectedValue = 9) Then

                If txtTime.Text = "" Then
                    intEscaltionTime = -1
                Else
                    intEscaltionTime = Val(txtTime.Text.Trim)
                End If

                If txtFreq.Text = "" Then
                    intEscalationFreq = -1
                Else
                    intEscalationFreq = Val(txtFreq.Text.Trim)
                End If
            Else
                intEscaltionTime = Val(txtTime.Text.Trim)
                intEscalationFreq = Val(txtFreq.Text.Trim)
            End If
            QueryString = QueryString & " and Escalation_Time=" & intEscaltionTime _
            & " and Mail_Frequency =" & intEscalationFreq _
            & " and record_type='E' and Rule_Status = 1 and Table_ID !=" & intInvid

            objCommand = New SqlCommand
            objCon.Open()
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
                obj = .ExecuteScalar
            End With
            If Not IsDBNull(obj) Then
                If Convert.ToInt16(obj) > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            CreateLog("EscaSetup", "isExisted-451", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return False
        Finally
            objCon.Close()
        End Try
        HttpContext.Current.Cache.Insert("EventStatus", dtDefaultEvent, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
    End Function
#End Region

#Region "Overrides"
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
#End Region
#End Region

#Region "Events"
#Region "imgOk_Click"
    Private Sub imgOk_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOk.Click
        lstError.Items.Clear()
        If ddEventName.SelectedIndex = "0" Then
            lstError.Items.Clear()
            lstError.Items.Add("Select event name...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        If dtEndDate.Text.Trim <> "" Then
            If DateTime.Parse(dtEndDate.Text) >= DateTime.Parse(dtStartDate.Text) Then
            Else
                lstError.Items.Add("Escalation Stop Date cannot be less than Started date...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If
        End If

        If ddEventUser.Enabled = False Then
            If ((ddRoleName.SelectedValue = "0") And (ddUserName.SelectedValue = "0")) Then
                lstError.Items.Clear()
                lstError.Items.Add("You should select atleast one option from RoleName and UserName...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        Else
            If ddEventUser.SelectedValue = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Select DefaultUser Name...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        End If


        If isExisted() = True Then
            lstError.Items.Clear()
            lstError.Items.Add("Same rule is already existed...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        Try

            Dim dbNull As DBNull
            Dim bln As Boolean

            arColumnName.Add("Event_ID_FK")
            arColumnName.Add("Event_User_ID_FK")
            arColumnName.Add("Event_Fired_ID_FK")
            arColumnName.Add("user_id")
            arColumnName.Add("Role_id")
            arColumnName.Add("Company_id")
            arColumnName.Add("Priority")
            arColumnName.Add("Task_type")
            arColumnName.Add("Call_Type")
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
            arColumnName.Add("Escalation_Time")
            arColumnName.Add("Mail_Frequency")

            arRowData.Add(IIf(ddEventName.SelectedValue.Trim = "0", "0", ddEventName.SelectedValue.Trim))
            arRowData.Add(IIf(ddEventUser.SelectedValue.Trim = "0", "0", ddEventUser.SelectedValue.Trim))
            arRowData.Add(IIf(ddEventFired.SelectedValue.Trim = "0", "0", ddEventFired.SelectedValue.Trim))
            arRowData.Add(IIf(ddUserName.SelectedValue.Trim = "0", "0", ddUserName.SelectedValue.Trim))
            arRowData.Add(IIf(ddRoleName.SelectedValue.Trim = "0", "0", ddRoleName.SelectedValue.Trim))
            arRowData.Add(IIf(ddCompany.SelectedValue.Trim = "0", "0", ddCompany.SelectedValue.Trim))
            arRowData.Add(IIf(ddPriority.SelectedValue.Trim = "0", System.DBNull.Value, ddPriority.SelectedValue.Trim))

            arRowData.Add(IIf(ddTaskType.SelectedValue = "0", System.DBNull.Value, ddTaskType.SelectedValue))
            arRowData.Add(IIf(ddCallType.SelectedValue = "0", System.DBNull.Value, ddCallType.SelectedValue))
            arRowData.Add(IIf(ddTaskStatus.SelectedValue.Trim = "0", System.DBNull.Value, ddTaskStatus.SelectedValue.Trim))
            arRowData.Add(IIf(ddCallStatus.SelectedValue.Trim = "0", System.DBNull.Value, ddCallStatus.SelectedValue.Trim))
            arRowData.Add(IIf(ddProject.SelectedValue.Trim = "0", "0", ddProject.SelectedValue.Trim))
            arRowData.Add("0")
            arRowData.Add(dtStartDate.Text.Trim)

            If (dtEndDate.Text = "") Then
                arRowData.Add(dbNull)
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

            If (ddEventName.SelectedValue = 8 Or ddEventName.SelectedValue = 9) Then

                If txtTime.Text = "" Then
                    arRowData.Add("-1")
                    intEscaltionTime = -1
                Else
                    arRowData.Add(txtTime.Text.Trim)
                    intEscaltionTime = txtTime.Text.Trim
                End If

                If txtFreq.Text = "" Then
                    arRowData.Add("-1")
                    intEscalationFreq = -1
                Else
                    arRowData.Add(txtFreq.Text.Trim)
                    intEscalationFreq = txtFreq.Text.Trim
                End If
            Else
                arRowData.Add(IIf(txtTime.Text.Trim = "", 0, txtTime.Text.Trim))
                arRowData.Add(IIf(txtFreq.Text.Trim = "", 0, txtFreq.Text.Trim))
                intEscaltionTime = txtTime.Text.Trim
                intEscalationFreq = txtFreq.Text.Trim

            End If
            If SQL.Update("Setup_Rules", "Escsetup", "imgok_click-741", "Select * from Setup_Rules where Table_ID = " & intInvid, arColumnName, arRowData) = True Then
                bln = True
            End If
            If bln = True Then
                Response.Write("<script>window.close();</script>")
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Record did not save successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("edit_ComSetup", "GetRoleData-319", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        End Try
    End Sub
#End Region

#Region "imgSave_Click"
    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        lstError.Items.Clear()
        If ddEventName.SelectedIndex = "0" Then
            lstError.Items.Clear()
            lstError.Items.Add("Select event name...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        If dtEndDate.Text.Trim <> "" Then
            If DateTime.Parse(dtEndDate.Text) >= DateTime.Parse(dtStartDate.Text) Then
            Else
                lstError.Items.Add("Escalation Stop Date cannot be less than Started date...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If
        End If

        If ddEventUser.Enabled = False Then

            If ((ddRoleName.SelectedValue = "0") And (ddUserName.SelectedValue = "0")) Then
                lstError.Items.Clear()
                lstError.Items.Add("You should select atleast one option from RoleName and UserName...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If
        Else
            If ddEventUser.SelectedValue = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Select DefaultUser Name...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        End If
        If isExisted() = True Then
            lstError.Items.Clear()
            lstError.Items.Add("Same rule is already existed...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        Try
            Dim dbNull As DBNull
            Dim bln As Boolean
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
            arColumnName.Add("Escalation_Time")
            arColumnName.Add("Mail_Frequency")

            arRowData.Add(IIf(ddEventName.SelectedValue.Trim = "0", "0", ddEventName.SelectedValue.Trim))
            arRowData.Add(IIf(ddEventUser.SelectedValue.Trim = "0", "0", ddEventUser.SelectedValue.Trim))
            arRowData.Add(IIf(ddEventFired.SelectedValue.Trim = "0", "0", ddEventFired.SelectedValue.Trim))
            arRowData.Add(IIf(ddUserName.SelectedValue.Trim = "0", "0", ddUserName.SelectedValue.Trim))
            arRowData.Add(IIf(ddRoleName.SelectedValue.Trim = "0", "0", ddRoleName.SelectedValue.Trim))
            arRowData.Add(IIf(ddCompany.SelectedValue.Trim = "0", "0", ddCompany.SelectedValue.Trim))
            arRowData.Add(IIf(ddPriority.SelectedValue.Trim = "0", System.DBNull.Value, ddPriority.SelectedValue.Trim))

            arRowData.Add(IIf(ddTaskType.SelectedValue = "0", System.DBNull.Value, ddTaskType.SelectedValue))
            arRowData.Add(IIf(ddCallType.SelectedValue = "0", System.DBNull.Value, ddCallType.SelectedValue))
            arRowData.Add(IIf(ddTaskStatus.SelectedValue.Trim = "0", System.DBNull.Value, ddTaskStatus.SelectedValue.Trim))
            arRowData.Add(IIf(ddCallStatus.SelectedValue.Trim = "0", System.DBNull.Value, ddCallStatus.SelectedValue.Trim))
            arRowData.Add(IIf(ddProject.SelectedValue.Trim = "0", "0", ddProject.SelectedValue.Trim))
            arRowData.Add("0")
            arRowData.Add(dtStartDate.Text.Trim)

            If (dtEndDate.Text = "") Then
                arRowData.Add(dbNull)
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
            arRowData.Add(IIf(txtTime.Text.Trim = "", 0, txtTime.Text.Trim))
            arRowData.Add(IIf(txtFreq.Text.Trim = "", 0, txtFreq.Text.Trim))
            If SQL.Update("Setup_Rules", "", "", "Select * from Setup_Rules where Table_ID = " & intInvid, arColumnName, arRowData) = True Then
                bln = True
            End If
            If bln = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Records updated successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("server is busy please try later ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Exit Sub
            End If

        Catch ex As Exception
            CreateLog("edit_ComSetup", "GetRoleData-319", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        End Try
    End Sub
#End Region


#Region "imgClose_Click"
    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Write("<script>window.close();</script>")
    End Sub
#End Region

#Region "ddCompany_SelectedIndexChanged"
    Private Sub ddCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCompany.SelectedIndexChanged
        Dim dtDDData As DataTable
        Dim strFilter As String
        Dim dv As DataView
        Dim lt As New ListItem

        ''dtDDData = Session("UNameComn")
        dtDDData = ViewState("EscEdit_UserName")
        ddUserName.Items.Clear()
        If ddCompany.SelectedValue <> 0 Then
            strFilter = "UM_IN4_Company_AB_ID=" & ddCompany.SelectedValue
        Else
            strFilter = ""
        End If
        dv = New DataView(dtDDData, strFilter, "UM_VC50_UserID", DataViewRowState.CurrentRows)
        ddUserName.DataSource = dv
        ddUserName.DataValueField = "UM_IN4_Address_No_FK"
        ddUserName.DataTextField = "UM_VC50_UserID"
        ddUserName.DataBind()
        lt.Text = "Select"
        lt.Value = "0"
        ddUserName.Items.Insert(0, lt)

        ''dtDDData = Session("RNameComn")
        dtDDData = ViewState("EscEdit_RoleName")
        ddRoleName.Items.Clear()
        If ddCompany.SelectedValue <> 0 Then
            strFilter = "ROM_IN4_Company_ID_FK=" & ddCompany.SelectedValue & " or ROM_IN4_Company_ID_FK=0"
        Else
            strFilter = ""
        End If
        dv = New DataView(dtDDData, strFilter, "ROM_VC50_Role_Name", DataViewRowState.CurrentRows)
        ddRoleName.DataSource = dv
        ddRoleName.DataValueField = "ROM_IN4_Role_ID_PK"
        ddRoleName.DataTextField = "ROM_VC50_Role_Name"
        ddRoleName.DataBind()
        lt.Text = "Select"
        lt.Value = "0"
        ddRoleName.Items.Insert(0, lt)

        'Fill user list based on company
        If ddCompany.SelectedIndex > 0 Then
            If (Session("PropCompanyType") = "SCM") Then

                PopulateDropDownLists("Select UM_IN4_Address_No_FK,upper( UM_VC50_UserID + ' ['+CI_VC36_Name + ']') UM_VC50_UserID, UM_IN4_Company_AB_ID from T060011 ,T010011 where UM_IN4_Company_AB_ID=CI_NU8_Address_Number and UM_VC4_Status_Code_FK ='ENB' and UM_IN4_Company_AB_ID=" & Val(ddCompany.SelectedValue) & "  order by UM_VC50_UserID", ddUserName, "N")

            End If
        Else
            PopulateDropDownLists("Select UM_IN4_Address_No_FK,upper( UM_VC50_UserID + ' ['+CI_VC36_Name + ']') UM_VC50_UserID, UM_IN4_Company_AB_ID from T060011 ,T010011 where UM_IN4_Company_AB_ID=CI_NU8_Address_Number and UM_VC4_Status_Code_FK ='ENB' order by UM_VC50_UserID", ddUserName, "N")
        End If
        ' -- SubCategory
        PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")
    End Sub
#End Region
#End Region

End Class
