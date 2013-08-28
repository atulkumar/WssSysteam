#Region "Purpose"
'***********************************************************************************************************
' Page :                 :- Escalation setup
' Purpose                :- It will create the  different setup rules for sending mails  
' Date					   Author						Modification Date				Description
' 27/06/06			  	   jagtar                            				            Modified by          
'
' Notes: 
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
'Session("UNameComn") replace with ViewState("Esc_UserName")
'Session("RNameComn")replace with ViewState("Esc_RoleName")
#End Region

#Region "NameSpace"
Imports System.Data.SqlClient
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
#End Region


Partial Class Escalation_EscalationSetup
    Inherits System.Web.UI.Page

#Region "Varibles"
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
    Private Shared dtDefaultEvent As New DataTable
    Private strConn As String
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Private arRowData As New ArrayList
    Private intEscaltionTime, intEscalationFreq As Int32
#End Region

#Region "Page_Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'Put user code to initialize the page here
        txtTime.Attributes.Add("onkeypress", "NumericOnly();")
        txtFreq.Attributes.Add("onkeypress", "NumericOnly();")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        lstError.Items.Clear()
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        If (Not Page.IsPostBack) Then
            txtCSS(Me.Page, "cpnlAD")
            chkIsMail.Text = ""
            chkIsSMS.Text = ""
            If (Session("PropCompanyType") = "SCM") Then

                PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM'  and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")", ddCompany, "Y")

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

        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Save"
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
                        arColumnName.Add("Start_Date") ' date
                        arColumnName.Add("Stop_Date") 'date
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

                        Dim dbNull As DBNull
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
                        arRowData.Add("0")
                        arRowData.Add("0")
                        arRowData.Add("E")
                        If (ddEventName.SelectedValue = 8 Or ddEventName.SelectedValue = 9) Then

                            If txtTime.Text = "" Then
                                arRowData.Add("-1")
                                intEscaltionTime = -1
                            Else
                                arRowData.Add(Val(txtTime.Text.Trim))
                                intEscaltionTime = Val(txtTime.Text.Trim)
                            End If

                            If txtFreq.Text = "" Then
                                arRowData.Add("-1")
                                intEscalationFreq = -1
                            Else
                                arRowData.Add(Val(txtFreq.Text.Trim))
                                intEscalationFreq = Val(txtFreq.Text.Trim)
                            End If
                        Else
                            arRowData.Add(IIf(txtTime.Text.Trim = "", 0, Val(txtTime.Text.Trim)))
                            arRowData.Add(IIf(txtFreq.Text.Trim = "", 0, Val(txtFreq.Text.Trim)))
                            intEscaltionTime = Val(txtTime.Text.Trim)
                            intEscalationFreq = Val(txtFreq.Text.Trim)

                        End If



                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then

                            lstError.Items.Add("You don't have access rights to Save record...")

                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            Exit Sub
                        End If

                        If isExisted() = False Then
                            SaveSetupRule(arColumnName, arRowData, "Setup_Rules")
                        Else
                            lstError.Items.Add("Same rule is already existed...")

                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)

                        End If

                    Case "Ok"

                        'Security Block

                        If imgSave.Enabled = False Or imgSave.Visible = False Then

                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If


                    Case "Close"
                        'Response.Redirect("Agreement_Details.aspx?")

                    Case "Logout"
                        LogoutWSS()

                    Case "Reset"

                        ddCallStatus.SelectedIndex = "0"
                        ddCallType.SelectedIndex = "0"
                        ddCompany.SelectedIndex = "0"
                        ddEventName.SelectedIndex = "0"
                        ddEventUser.SelectedIndex = "0"
                        ddPriority.SelectedIndex = "0"
                        ddProject.SelectedIndex = "0"
                        ddRecordStatus.SelectedIndex = "0"
                        ddEventFired.SelectedIndex = "0"
                        ddRoleName.SelectedIndex = "0"
                        'ddStatus.SelectedIndex = "0"
                        ddTaskStatus.SelectedIndex = "0"
                        ddTaskType.SelectedIndex = "0"
                        ddUserName.SelectedIndex = "0"
                        chkIsMail.Checked = False
                        chkIsSMS.Checked = False
                        dtStartDate.Text = SetDateFormat(Now.ToShortDateString, mdlMain.IsTime.DateOnly)
                        dtEndDate.Text = ""

                End Select

            Catch ex As Exception

            End Try
        End If

        Try

            If Not IsPostBack Then
                '*******************************************************************

                ' Purpose              : - Display data in data grid on load 
                ' Date					   Author						Modification Date				Description
                ' 25/05/06			   Sachin Prashar		    ------------------					    Created
                '
                ' Notes: 
                ' Code:
                '*******************************************************************



                Dim str As String
                Dim dsFromView As New DataSet

                If (Session("PropCompanyType") = "SCM") Then
                    str = "Select sr.Table_ID as RNo,sr.Mail_on_off as M,sr.SMS_on_off as S,Escalation_Time as Days,Mail_Frequency as Freq,convert(varchar,sr.Start_Date,101) as StartDate,convert(varchar, sr.Stop_Date,101) as StopDate,em.Name as Event, Event_User_Name as DefaultUser,em1.name as OnEvent,ab.CI_VC36_Name as Comp,un.UM_VC50_UserID as UserName,ROM_VC50_Role_Name as Role,sr.Priority,sr.Call_Type as CType, call.SU_VC50_Status_Name as CStatus, sr.Task_type as TType, task.SU_VC50_Status_Name as TStatus,PR.PR_VC20_Name as Proj, sr.Rule_Status as Stat from eventmaster em, eventmaster em1, Setup_rules sr,T040081 call, T040081 task, t010011 ab, t070031 rn, t060011 un, Eventusermaster eum,T210011 as PR where sr.Event_ID_FK *= em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.user_id *= un.UM_IN4_Address_No_FK and sr.Event_Fired_ID_FK *= em1.Event_ID_PK and sr.Call_Status *= call.SU_VC50_Status_Name and sr.Task_Status *= task.SU_VC50_Status_Name and eum.Enabled='Y' and  sr.Project_ID*=PR.PR_NU9_Project_ID_Pk and sr.company_id*=PR_NU9_Comp_ID_Fk and sr.Event_ID_FK <> 18  and record_type='E'"
                Else
                    str = "Select sr.Table_ID as RNo,sr.Mail_on_off as M,sr.SMS_on_off as S,Escalation_Time as Days,Mail_Frequency as Freq,convert(varchar,sr.Start_Date,101) as StartDate,convert(varchar, sr.Stop_Date,101) as StopDate,em.Name as Event, Event_User_Name as DefaultUser,em1.name as OnEvent,ab.CI_VC36_Name as Comp,un.UM_VC50_UserID as UserName,ROM_VC50_Role_Name as Role,sr.Priority,sr.Call_Type as CType, call.SU_VC50_Status_Name as CStatus, sr.Task_type as TType, task.SU_VC50_Status_Name as TStatus, pr.PR_VC20_Name as Proj, sr.Rule_Status as Stat from eventmaster em, eventmaster em1, Setup_rules sr,T040081 call, T040081 task, t010011 ab, t070031 rn, t060011 un, Eventusermaster eum,T210011 as PR where sr.Event_ID_FK *= em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.user_id *= un.UM_IN4_Address_No_FK and sr.Event_Fired_ID_FK *= em1.Event_ID_PK and sr.Call_Status *= call.SU_VC50_Status_Name and sr.Task_Status *= task.SU_VC50_Status_Name and eum.Enabled='Y'  and  sr.Project_ID*=PR.PR_NU9_Project_ID_Pk and sr.company_id*=PR_NU9_Comp_ID_Fk and sr.Event_ID_FK <> 18  and record_type='E' and (sr.Company_id=0 or sr.Company_id=" & Session("PropCompanyID") & ")"
                End If

                str &= " and Company_id  in (" & GetCompanySubQuery() & ")  "

                If SQL.Search("eventmaster", "Escalation", "load-431", str, dsFromView, "", "") Then
                    mdvtable.Table = dsFromView.Tables(0)
                    If dgCommRules.AutoGenerateColumns = False Then
                        dgCommRules.AutoGenerateColumns = True
                    End If

                    Dim htDateCols As New Hashtable
                    htDateCols.Add("StartDate", 2)
                    htDateCols.Add("StopDate", 2)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)

                    dgCommRules.DataSource = mdvtable
                    dgCommRules.DataBind()

                    FormatGrid()
                    GetColumns()
                    CreateTextBox()

                Else

                    dsFromView.Tables(0).NewRow.Item(0) = "0"
                    mdvtable.Table = dsFromView.Tables(0)
                    If dgCommRules.AutoGenerateColumns = False Then
                        dgCommRules.AutoGenerateColumns = True
                    End If
                    dgCommRules.DataSource = mdvtable
                    dgCommRules.DataBind()
                    FormatGrid()
                    GetColumns()
                    'create textbox at run time at head of the datagrid        
                    CreateTextBox()
                End If
            Else
                arrtextvalue.Clear()
                For i As Integer = 0 To arColumns.Count - 1
                    arrtextvalue.Add(Request.Form("cpnlAD$" & arCol.Item(i)))
                Next
                FillView()
                If txthiddenImage = "Search" Then
                    BtnGrdSearch_Click(Nothing, Nothing)
                End If
                FormatGrid()
                GetColumns()
                'create textbox at run time at head of the datagrid        
                CreateTextBox()

            End If
            'Security Block
            Dim intID As Int32
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                'intID = Request.QueryString("ScrID")
                intID = 544
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intID) = False Then
                    Response.Redirect("../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intID)
            End If
            'End of Security Block
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Functions"
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

            QueryString = QueryString & " and Escalation_Time=" & intEscaltionTime _
            & " and Mail_Frequency =" & intEscalationFreq & " and record_type='E' and Rule_Status = 1"

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
            CreateLog("ComSetup", "ShowValues-451", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            objCon.Close()
        End Try
        ViewState.Add("EventStatus", dtDefaultEvent)

    End Function
#End Region
#Region "GetPriorityParams"
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
                DisplayError("Record not found")
            End If
        Catch ex As Exception

            CreateLog("ComSetup", "ShowValues-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        Finally
            objCon.Close()
        End Try
    End Function
#End Region

#Region "GetEventStatus"
    Private Function GetEventStatus() As Boolean

        If IsNothing(ViewState("EventStatus")) Then
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
            ViewState.Add("EventStatus", dtDefaultEvent)
            'HttpContext.Current.Cache.Insert("EventStatus", dtDefaultEvent, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
        Else

            dtDefaultEvent = CType(ViewState("EventStatus"), DataTable)

        End If
    End Function
#End Region


#Region "DisplayError"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
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
            If udc = True Then
                ddData.DataValueField = dtDDData.Columns(1).ColumnName
            Else
                ddData.DataValueField = dtDDData.Columns(0).ColumnName
            End If
            ddData.DataTextField = dtDDData.Columns(1).ColumnName

            If (isOptional = "Y") Then
                Dim lt As New ListItem
                lt.Text = "Opt"
                lt.Value = "0"
                ddData.DataBind()
                ddData.Items.Insert(0, lt)

            Else
                Dim row As DataRow
                row = dtDDData.NewRow
                row(0) = "0"
                row(1) = "Select"
                dtDDData.Rows.InsertAt(row, 0)
                ddData.SelectedValue = "0"
                ddData.DataBind()
                If ddData.ID = "ddUserName" Then
                    If dtDDData.Rows.Count > 0 Then
                        dtDDData.Rows(0).Delete()
                    End If
                    ''Session.Add("UNameComn", dtDDData)
                    ViewState.Add("Esc_UserName", dtDDData)
                ElseIf ddData.ID = "ddRoleName" Then
                    If dtDDData.Rows.Count > 0 Then
                        dtDDData.Rows(0).Delete()
                    End If
                    ''Session.Add("RNameComn", dtDDData)
                    ViewState.Add("Esc_RoleName", dtDDData)
                End If
            End If

        Catch ex As Exception
            CreateLog("RolePermission", "GetRoleData-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            sqlCon.Close()
        End Try

    End Sub
#End Region
#Region "SaveSetupRule"
    Private Function SaveSetupRule(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            ' Table name
            '            SQL.DBTable = TableName
            SQL.DBTracing = False
            If ddEventName.SelectedValue = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Select EventName...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            If dtEndDate.Text.Trim <> "" Then
                If DateTime.Parse(dtEndDate.Text) >= DateTime.Parse(dtStartDate.Text) Then
                Else

                    lstError.Items.Add("Escalation Stop Date cannot be less than Started date...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            End If

            If ddEventUser.Enabled = False Then
                If ((ddRoleName.SelectedValue = "0") And (ddUserName.SelectedValue = "0")) Then
                    lstError.Items.Add("You should select atleast one option from RoleName and UserName...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            Else
                If ddEventUser.SelectedValue = 0 Then
                    lstError.Items.Add("Select DefaultUser Name...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            End If
            If SQL.Save(TableName, "Escalation", "SaveSetUpRule", ColumnName, RowData) = False Then

                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Exit Function
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Record Saved successfully...")
                txtTime.Text = ""
                txtFreq.Text = ""
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                ClearControls()
                Exit Function
            End If

            Return stReturn
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("WWSSave", "SaveUser-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function
#End Region

#Region "ClearControls"
    Private Sub ClearControls()
        Try
            ddEventName.SelectedIndex = "0"
            ddEventUser.SelectedIndex = "0"
            ddCompany.SelectedIndex = "0"
            ddCallStatus.SelectedIndex = "0"
            ddCallType.SelectedIndex = "0"
            ddPriority.SelectedIndex = "0"
            ddProject.SelectedIndex = "0"
            ddEventFired.SelectedIndex = "0"
            ddRoleName.SelectedIndex = "0"
            ddTaskStatus.SelectedIndex = "0"
            ddTaskType.SelectedIndex = "0"
            ddUserName.SelectedIndex = "0"
            chkIsMail.Checked = False
            chkIsSMS.Checked = False
            dtEndDate.Text = ""
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "PopulateCommunicationSetupGrid"
    Private Sub PopulateCommunicationSetupGrid()
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConnection

        SQL.DBTracing = False
        Dim ds As New DataSet
        If SQL.Search("Setup_Rules", "EscalationSetup", "PopulateCommunicationSetupGrid", "Select * from Setup_Rules", ds, "", "") = True Then
            dgCommRules.DataSource = ""
        Else
        End If
    End Sub
#End Region


#Region "fill View"

    '*******************************************************************

    ' Purpose              : - Display data in data grid from database on Ispost back
    ' Date					   Author						Modification Date				Description
    ' 25/05/06			   Sachin Prashar		    ------------------					    Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************

    Private Sub FillView()

        Dim dsFromView As New DataSet
        Dim str As String

        If (Session("PropCompanyType") = "SCM") Then
            str = "Select sr.Table_ID as RNo,sr.Mail_on_off as M,sr.SMS_on_off as S,Escalation_Time as Days,Mail_Frequency as Freq,convert(varchar,sr.Start_Date,101) as StartDate,convert(varchar, sr.Stop_Date,101) as StopDate,em.Name as Event, Event_User_Name as DefaultUser,em1.name as OnEvent,ab.CI_VC36_Name as Comp,un.UM_VC50_UserID as UserName,ROM_VC50_Role_Name as Role,sr.Priority,sr.Call_Type as CType, call.SU_VC50_Status_Name as CStatus, sr.Task_type as TType, task.SU_VC50_Status_Name as TStatus,PR.PR_VC20_Name as Proj, sr.Rule_Status as Stat from eventmaster em, eventmaster em1, Setup_rules sr,T040081 call, T040081 task, t010011 ab, t070031 rn, t060011 un, Eventusermaster eum,T210011 as PR where sr.Event_ID_FK *= em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.user_id *= un.UM_IN4_Address_No_FK and sr.Event_Fired_ID_FK *= em1.Event_ID_PK and sr.Call_Status *= call.SU_VC50_Status_Name and sr.Task_Status *= task.SU_VC50_Status_Name and eum.Enabled='Y' and  sr.Project_ID*=PR.PR_NU9_Project_ID_Pk and sr.company_id*=PR_NU9_Comp_ID_Fk and sr.Event_ID_FK <> 18  and record_type='E'"
        Else
            str = "Select sr.Table_ID as RNo,sr.Mail_on_off as M,sr.SMS_on_off as S,Escalation_Time as Days,Mail_Frequency as Freq,convert(varchar,sr.Start_Date,101) as StartDate,convert(varchar, sr.Stop_Date,101) as StopDate,em.Name as Event, Event_User_Name as DefaultUser,em1.name as OnEvent,ab.CI_VC36_Name as Comp,un.UM_VC50_UserID as UserName,ROM_VC50_Role_Name as Role,sr.Priority,sr.Call_Type as CType, call.SU_VC50_Status_Name as CStatus, sr.Task_type as TType, task.SU_VC50_Status_Name as TStatus, pr.PR_VC20_Name as Proj, sr.Rule_Status as Stat from eventmaster em, eventmaster em1, Setup_rules sr,T040081 call, T040081 task, t010011 ab, t070031 rn, t060011 un, Eventusermaster eum,T210011 as PR where sr.Event_ID_FK *= em.Event_ID_PK and sr.Event_User_ID_FK *= eum.Event_User_ID_PK and sr.Role_id *= rn.ROM_IN4_Role_ID_PK and sr.Company_id *= ab.CI_NU8_Address_Number and sr.user_id *= un.UM_IN4_Address_No_FK and sr.Event_Fired_ID_FK *= em1.Event_ID_PK and sr.Call_Status *= call.SU_VC50_Status_Name and sr.Task_Status *= task.SU_VC50_Status_Name and eum.Enabled='Y'  and  sr.Project_ID*=PR.PR_NU9_Project_ID_Pk and sr.company_id*=PR_NU9_Comp_ID_Fk and sr.Event_ID_FK <> 18  and record_type='E' and (sr.Company_id=0 or sr.Company_id=" & Session("PropCompanyID") & ")"
        End If


        str &= " and Company_id  in (" & GetCompanySubQuery() & ")  "

        If SQL.Search("T030201", "EscalationSetup", "FillView", str, dsFromView, "", "") Then
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

            Catch ex As Exception
                CreateLog("OW Views", "Fill View", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            dsFromView.Tables(0).NewRow.Item(0) = "0"
            mdvtable.Table = dsFromView.Tables(0)

            If dgCommRules.AutoGenerateColumns = False Then
                dgCommRules.AutoGenerateColumns = True
            End If

            Dim htDateCols As New Hashtable
            htDateCols.Add("StartDate", 2)
            htDateCols.Add("StopDate", 2)
            SetDataTableDateFormat(mdvtable.Table, htDateCols)

            dgCommRules.DataSource = mdvtable
            dgCommRules.DataBind()

        End If
    End Sub

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
        arCol.Add("RuleNo")
        arCol.Add("Mail_on_off")
        arCol.Add("SMS_on_off")
        arCol.Add("Escalation_Time")
        arCol.Add("Mail_Frequency")
        arCol.Add("Start_Date")
        arCol.Add("Stop_Date")
        arCol.Add("Event_ID_FK")
        arCol.Add("Event_User_ID_FK")
        arCol.Add("Event_Fired_ID_FK")
        arCol.Add("user_id")
        arCol.Add("Role_id")
        arCol.Add("Priority")
        arCol.Add("Call_Type")
        arCol.Add("Call_Status")
        arCol.Add("Task_type")
        arCol.Add("Task_Status")
        arCol.Add("Project_ID")
        arCol.Add("Company_id")
        arCol.Add("Rule_Status")

        'fill the columns count into the array from mdvtable view
        Try

            intCol = mdvtable.Table.Columns.Count


            If Not IsPostBack Then
                ReDim mTextBox(intCol)
            End If


            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(arCol.Item(intii))
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    'End If

                    _textbox.ID = arCol.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox

                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"

                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)

                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
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

        arColWidth.Add(25)
        arColWidth.Add(15)
        arColWidth.Add(15)
        arColWidth.Add(25)
        arColWidth.Add(25)
        arColWidth.Add(72)
        arColWidth.Add(72)

        arColWidth.Add(105)
        arColWidth.Add(70)
        arColWidth.Add(105)


        arColWidth.Add(40)
        arColWidth.Add(80)
        arColWidth.Add(80)


        arColWidth.Add(40)
        arColWidth.Add(40)
        arColWidth.Add(45)
        arColWidth.Add(40)
        arColWidth.Add(48)
        arColWidth.Add(40)
        arColWidth.Add(40)

        Try
            dgCommRules.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                'End If


                'Bound_Column.HeaderText = arColumnName.Item(intI)
                dgCommRules.Columns.Add(Bound_Column)
            Next

        Catch ex As Exception
            CreateLog("ViewJobs", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    '*******************************************************************

    ' Purpose              : - fill array with width and column name for datagrid columns for formating
    ' Date					   Author						Modification Date				Description
    ' 25/05/06			   Sachin Prashar		    ------------------					    Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************



    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()


        arColWidth.Add(23)
        arColWidth.Add(12)
        arColWidth.Add(11)
        arColWidth.Add(20)
        arColWidth.Add(22)
        arColWidth.Add(70)
        arColWidth.Add(69)

        arColWidth.Add(100)
        arColWidth.Add(68)
        arColWidth.Add(100)

        arColWidth.Add(37)
        arColWidth.Add(78)
        arColWidth.Add(75)


        arColWidth.Add(39)
        arColWidth.Add(35)
        arColWidth.Add(45)
        arColWidth.Add(38)
        arColWidth.Add(41)
        arColWidth.Add(39)
        arColWidth.Add(39)

        arColumnName.Add("RuleNo")
        arColumnName.Add("Mail_on_off")
        arColumnName.Add("SMS_on_off")
        arColumnName.Add("Escalation_Time")
        arColumnName.Add("Mail_Frequency")
        arColumnName.Add("Start_Date")
        arColumnName.Add("Stop_Date")

        arColumnName.Add("Event_ID_FK")
        arColumnName.Add("Event_User_ID_FK")
        arColumnName.Add("Event_Fired_ID_FK")

        arColumnName.Add("Company_id")
        arColumnName.Add("user_id")
        arColumnName.Add("Role_id")


        arColumnName.Add("Priority")
        arColumnName.Add("Call_Type")
        arColumnName.Add("Call_Status")
        arColumnName.Add("Task_type")
        arColumnName.Add("Task_Status")
        arColumnName.Add("Project_ID")
        arColumnName.Add("Rule_Status")


    End Sub

#End Region

#Region "check the grid width from database"
    '*******************************************************************

    ' Purpose              : - fill array with width and column name for datagrid columns for formating
    ' Date					   Author						Modification Date				Description
    ' 25/05/06			   Sachin Prashar		    ------------------					    Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()


        arColWidth.Add(40)
        arColWidth.Add(40)
        arColWidth.Add(40)
        arColWidth.Add(40)
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
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)


        arColumnName.Add("Mail_on_off")
        arColumnName.Add("SMS_on_off")
        arColumnName.Add("Start_Date")
        arColumnName.Add("Stop_Date")
        arColumnName.Add("Event_ID_FK")
        arColumnName.Add("Event_User_ID_FK")
        arColumnName.Add("Event_Fired_ID_FK")
        arColumnName.Add("user_id")
        arColumnName.Add("Role_id")
        arColumnName.Add("Priority")
        arColumnName.Add("Call_Type")
        arColumnName.Add("Call_Status")
        arColumnName.Add("Task_type")
        arColumnName.Add("Task_Status")
        arColumnName.Add("Project_ID")
        arColumnName.Add("Company_id")
        arColumnName.Add("Rule_Status")

    End Sub

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
#Region "imgClose_Click"
    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Involvements() = False Then
            Exit Sub
        End If
    End Sub
    Private Function Involvements() As Boolean

    End Function
#End Region

#Region "ddCompany_SelectedIndexChanged"
    Private Sub ddCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddCompany.SelectedIndexChanged
        Dim dtDDData As DataTable
        Dim strFilter As String
        Dim dv As DataView
        Dim lt As New ListItem

        ''dtDDData = Session("UNameComn")
        dtDDData = ViewState("Esc_UserName")
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
        dtDDData = ViewState("Esc_RoleName")
        ddRoleName.Items.Clear()
        If ddCompany.SelectedValue <> 0 Then
            strFilter = "ROM_IN4_Company_ID_FK=" & ddCompany.SelectedValue
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

        ' -- SubCategory
        PopulateDropDownLists("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Val(ddCompany.SelectedValue), ddProject, "Y")
    End Sub
#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not arrtextvalue(intI).Equals("") Then
                    strSearch = arrtextvalue(intI)
                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
                            End If
                        End If

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
                            strSearch = strSearch.Replace("*", "")
                            If IsNumeric(strSearch) = False Then
                                strSearch = "-101"
                            End If
                        End If
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = arrtextvalue(intI)
                        If strSearch.Contains("*") = True Then
                            strSearch = strSearch.Replace("*", "%")
                        Else
                            strSearch &= "%"
                        End If
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
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
            CreateLog("ViewJobs", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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
        Dim strCom As String
        dgCommRules.Columns.Clear()
        Dim inti As Integer = 0
        Try

            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    If e.Item.Cells(19).Text = 0 Then
                        e.Item.Cells(inti).ForeColor = System.Drawing.Color.Gray
                        inti = inti + 1
                    End If
                End If
            Next

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                strID = dgCommRules.DataKeys(e.Item.ItemIndex)
                strTempName = e.Item.Cells(1).Text
                e.Item.Attributes.Add("style", "cursor:hand")
                e.Item.Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "','" & rowvalue & "')")
                strCom = e.Item.Cells(10).Text.Trim
                If (Session("PropCompanyType") = "SCM") Then
                    e.Item.Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "','" & strCom & "',1)")
                Else
                    e.Item.Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "','" & strCom & "',0)")
                End If
            End If


            rowvalue += 1
        Catch ex As Exception
            CreateLog("ViewJobs", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "dgCommRules")
        End Try
    End Sub

#End Region

#Region "ddEventName_SelectedIndexChanged"
    Private Sub ddEventName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddEventName.SelectedIndexChanged
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

        Else
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
    End Sub
#End Region
#End Region
End Class
