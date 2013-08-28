Imports ION.Data
Imports ION.Logging
Imports ION.Logging.EventLogging
Imports System.Data
Imports WSSBLL
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Partial Class SupportCenter_CallView_Task_Fwd
    Inherits System.Web.UI.Page
    Dim intProjectID As Integer
    Private objCommonFunctionsBLL As New clsCommonFunctionsBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        txtCallNo.Text = Request.QueryString("CallNo")
        txtTaskno.Text = Request.QueryString("TASKNO")
        If IsNumeric(Request.QueryString("CompID")) = True Then
            ViewState("TaskFwdCompanyID") = Request.QueryString("CompID")
        Else
            ViewState("TaskFwdCompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID").Trim).ExtraValue
        End If


        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgOK.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")

        intProjectID = WSSSearch.SearchProjectID(txtCallNo.Text, ViewState("TaskFwdCompanyID"))
        Dim strSQL As String
        strSQL = "SELECT TM_VC8_SUPP_OWNER FROM T040021 where TM_NU9_Comp_ID_FK=" & ViewState("TaskFwdCompanyID") & " and TM_NU9_Task_No_PK=" & txtTaskno.Text.Trim & " and TM_NU9_Call_No_FK=" & txtCallNo.Text.Trim
        ViewState("PreviousTaskOwnerID") = SQL.Search("Task_Fwd", "Load", strSQL)

        '--Task Owner
        CDDLTaskOwner.Items.Clear()
        fillRadCombo(intProjectID, ViewState("TaskFwdCompanyID"), ViewState("PreviousTaskOwnerID"))
        'CDDLTaskOwner.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid + ' [' + T1.CI_VC36_Name +']' as Name,T2.ci_vc36_name as Company FROM t060011,t010011 T1,T010011 T2 where T2.ci_nu8_address_number=um_in4_company_ab_id and T1.ci_nu8_address_number=UM_IN4_Address_No_FK and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & intProjectID & " and PM_NU9_Comp_ID_FK=" & ViewState("TaskFwdCompanyID") & "  and PM_NU9_Project_Member_ID<>" & ViewState("PreviousTaskOwnerID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name"
        'CDDLTaskOwner.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid + ' [' + T2.CI_VC36_Name +']' as Name,ci_vc36_name as Company FROM t060011,t010011 T1,T010011 T2 where T2.ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & intProjectID & " and PM_NU9_Comp_ID_FK=" &ViewState("TaskFwdCompanyID") & "  and PM_NU9_Project_Member_ID<>" & ViewState("TaskFwdOwnerID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name"

        'CDDLTaskOwner.CDDLMandatoryField = True
        'CDDLTaskOwner.CDDLUDC = False
        '------------------------------------------
        If IsPostBack = False Then

            txtCSS(Me.Page)
            'CDDLTaskOwner.CDDLFillDropDown(10, False)
        End If

        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                        'End of Security Block
                        If UpdateTask() = True Then
                            SaveComments() ' -- Save comments after task is updated
                            'Response.Write("<script>self.opener.Form1.submit();</script>")
                        End If
                    Case "Close"
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                        'End of Security Block
                        If UpdateTask() = True Then
                            SaveComments() ' -- Save comments after task is updated
                            Response.Write("<script>self.opener.Form1.submit();</script>")
                            Response.Write("<script>window.close();</script>")
                        End If
                    Case "Reset"
                End Select
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Error occur  please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End Try
        End If
        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
        'End of Security Block
    End Sub
    Private Sub fillRadCombo(ByVal ProjectID As Integer, ByVal CompanyID As Integer, ByVal TaskOwnerID As Integer)
        Dim dsComboTaskOwnerType As New DataSet
        Try
            dsComboTaskOwnerType = objCommonFunctionsBLL.FillRadTaskOwnerForTaskForward(ProjectID, CompanyID, TaskOwnerID)
            CDDLTaskOwner.Items.Clear()
            CDDLTaskOwner.DataSource = dsComboTaskOwnerType
            For Each data As DataRow In dsComboTaskOwnerType.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("Name"))
                item.Value = CStr(data("ID"))
                CDDLTaskOwner.Items.Add(item)
                item.DataBind()
            Next
        Catch ex As Exception
            CreateLog("Comment", "FillRadCombo-203", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Function CheckRealValuesForRadDDLS() As Boolean
        Try
            Dim strCommentTo As String = String.Empty
            strCommentTo = SQL.Search("", "", "SELECT um_in4_address_no_fk as ID,T1.CI_VC36_Name + ' [' + um_vc50_userid +']' as Name,T2.ci_vc36_name as Company FROM t060011,t010011 T1,T010011 T2 where T2.ci_nu8_address_number=um_in4_company_ab_id and T1.ci_nu8_address_number=UM_IN4_Address_No_FK and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & intProjectID & " and PM_NU9_Comp_ID_FK=" & ViewState("TaskFwdCompanyID") & "  and PM_NU9_Project_Member_ID<>" & ViewState("PreviousTaskOwnerID") & ") and T1.CI_VC36_Name + ' [' + um_vc50_userid +']'='" & CDDLTaskOwner.Text & "'  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name", "")
            If String.IsNullOrEmpty(strCommentTo) = True Then
                Return False
            End If
            Return True
        Catch ex As Exception
        End Try
    End Function
    Private Function UpdateTask() As Boolean

        If CDDLTaskOwner.Text.Trim.Equals("") Then
            lstError.Items.Clear()
            lstError.Items.Add("Task owner cannot be blank...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        End If
        If CheckRealValuesForRadDDLS() = False Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select a valid Task Owner from Dropdown.")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        End If
        'Check the status of the task becuase if it is CLOSED then it cannot be forwarded
        'get the status of the task becuase only ASSIGNED tasks can be deleted
        Dim strTaskStatus As String
        strTaskStatus = WSSSearch.GetTaskStatus(txtCallNo.Text, txtTaskno.Text, ViewState("TaskFwdCompanyID"))

        If strTaskStatus = "CLOSED" Then
            lstError.Items.Clear()
            lstError.Items.Add("CLOSED Task cannot be forwarded...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Return False
        Else
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim intTaskOwner As Integer = SQL.Search("Task_Fwd", "Load-70", "Select TM_VC8_Supp_Owner from T040021 where TM_NU9_Comp_ID_FK=" & ViewState("TaskFwdCompanyID") & " and TM_NU9_Call_No_FK=" & txtCallNo.Text & " and TM_NU9_Task_no_PK=" & txtTaskno.Text.Trim & "")
            mstGetFunctionValue = TaskForward(WSSSearch.SearchUserID(Val(ViewState("PreviousTaskOwnerID"))).ExtraValue, ViewState("PreviousTaskOwnerID"), CDDLTaskOwner.Text, HttpContext.Current.Session("PropUserID"), CDDLTaskOwner.SelectedValue, intTaskOwner, Val(ViewState("TaskFwdCompanyID")), txtCallNo.Text, txtTaskno.Text)
            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Task Forwarded successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                If txtComment.Text = "" Then
                    CDDLTaskOwner.Text = ""
                End If
                Return True
            End If
            If mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Clear()
                lstError.Items.Add("Error occur please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        End If

    End Function
    Private Function SaveComments() As Boolean
        If txtComment.Text.Trim.Equals("") Then
            Return True
            Exit Function
        End If
        If CheckRealValuesForRadDDLS() = False Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select a valid Task Owner from Dropdown.")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        End If
        Try
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            Dim strCommentToMail As String

            arColumnName.Add("CM_NU9_Comment_To")
            arColumnName.Add("CM_CH1_Flag")
            arColumnName.Add("CM_NU9_AB_Number")
            arColumnName.Add("CM_DT8_Date")
            arColumnName.Add("CM_VC256_Comments")
            arColumnName.Add("CM_VC2_Flag")
            arColumnName.Add("CM_NU9_Call_Number")
            arColumnName.Add("CM_NU9_Task_Number")
            arColumnName.Add("CM_NU9_Action_Number")
            arColumnName.Add("CM_VC30_Type")
            arColumnName.Add("CM_NU9_CompId_Fk")
            arColumnName.Add("CM_VC50_IE")
            arColumnName.Add("CM_VC1000_MailList")
            arColumnName.Add("CM_CH1_MailSent")

            If Val(Request.QueryString("OPT")) = "2" Then
                arColumnName.Add("CM_NU9_TemplateID_FK")
            End If

            arRowData.Add(IIf(CDDLTaskOwner.SelectedValue = "", System.DBNull.Value, CDDLTaskOwner.SelectedValue))

            arRowData.Add("1")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(Now)
            arRowData.Add(txtComment.Text.Trim)
            arRowData.Add("T")
            arRowData.Add(txtCallNo.Text.Trim)
            arRowData.Add(txtTaskno.Text.Trim)
            arRowData.Add(Val(ViewState("ActionNo")))
            arRowData.Add("F")
            arRowData.Add(ViewState("TaskFwdCompanyID"))
            arRowData.Add("External")


            If CDDLTaskOwner.SelectedValue <> "" Then
                strCommentToMail = SQL.Search("", "", "select CI_VC28_Email_1 from T010011 where CI_NU8_Address_Number=" & Val(CDDLTaskOwner.SelectedValue))
            End If


            arRowData.Add(strCommentToMail)
            arRowData.Add("F")

            If Val(Request.QueryString("OPT")) = 2 Then
                arRowData.Add(HttpContext.Current.Session("SAddressNumber"))
                mstGetFunctionValue = WSSSave.SaveComments(arColumnName, arRowData, 2)
            Else
                mstGetFunctionValue = WSSSave.SaveComments(arColumnName, arRowData)
            End If
            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                txtComment.Text = ""
                Session("SEmailId") = ""
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Clear()
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Return False
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Return False
            End If

        Catch ex As Exception
            CreateLog("Comment", "SaveComments-203", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function
End Class
