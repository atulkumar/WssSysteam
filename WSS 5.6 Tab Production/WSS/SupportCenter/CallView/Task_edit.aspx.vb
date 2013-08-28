Imports ION.Data
Imports System.IO
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports ION.Logging.EventLogging
Imports Telerik.Web.UI
Imports WSSBLL
Imports System.Data

Partial Class SupportCenter_CallView_Task_edit
    Inherits System.Web.UI.Page

#Region "Form Level Variables"
    Public intPropTaskNumber As Integer
    Public intPropCallNumber As Integer
    Private mintFileID As Integer
    Private mstrFileName As String
    Private mstrFilePath As String
    Private Shared mStrPrvTaskStatus As String ' To be used in Logging of Status Change Event
    Private Shared mStrTaskStatus As String ' To be used in  Status Change Event
    Private Shared strTaskDate As String
    Private objCommonFunctionsBLL As New clsCommonFunctionsBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    'used to store task date, which will be used while updating task 
    'so that task date should be saved in logs table
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        Try
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            Call txtCSS(Me.Page)   'From Module
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgForm.Attributes.Add("Onclick", "return SaveEdit('Forms');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            txtEstimatedHrs.Attributes.Add("onkeypress", "UsedHour('txtEstimatedHrs')")
            txtETC.Attributes.Add("onkeypress", "ETCHour('txtETC')")
            txttaskorder.Attributes.Add("onkeypress", "NumericOnly()")
            'add attributes
            'saurabh
            txttaskid.Attributes.Add("onkeypress", "NumericOnly()")
            'call the fucntion to make the fields readonly if it is opened from todolist
            txtSubject.Attributes.Add("onmousemove", "ShowToolTip(this,1000);")
            'clear the form bit session 
            ' this will be filled in FillData()
            If Not IsPostBack Then
                ViewState("FromBit") = Nothing
                strTaskDate = ""
            End If

            If Not IsNothing(Request.QueryString("CompID")) Then
                If IsNumeric(Request.QueryString("CompID")) = True Then
                    ViewState("CompanyID") = Request.QueryString("CompID")
                Else
                    ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
                End If
            End If
            If Not IsNothing(Request.QueryString("CallNo")) Then
                ViewState("CallNo") = Val(Request.QueryString("CallNo"))
            End If
            If Not IsNothing(Request.QueryString("TASKNO")) Then
                ViewState("TaskNo") = Val(Request.QueryString("TASKNO"))
            End If
            ViewState("ProjectID") = WSSSearch.SearchProjectID(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))

            ' -- Coding for CDDL's
            FillRadCombos()

            ' -- Dependency
            If Not IsPostBack Then
                FillNonUDCDropDown(DDLDependency, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where isnull(TM_NU9_Dependency,0)<>" & Val(Request.QueryString("TASKNO")) & " AND TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and Tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and TM_NU9_Task_No_Pk<>" & Request.QueryString("TASKNO") & " and Tm_vc50_deve_status<>'CLOSED'", True)
            End If
            '-----------------------------------------
            '--Agreement
            CDDLAgreement.CDDLQuery = "select AG_NU8_ID_PK as ID,AG_VC200_Desc Description,CI_VC36_Name ""Contact Person"" from T080011 Ag,T010011 AB where ag.AG_VC8_Cust_Name =8 and ab.CI_NU8_Address_Number = ag.AG_VC8_Contact_Person"
            CDDLAgreement.CDDLMandatoryField = True
            CDDLAgreement.CDDLFillDropDown(10)
            '-----------------------------------------
        Catch ex As Exception
            CreateLog("Task-Edit", "Load-167", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try

        Dim strhiddenImage As String
        Dim intImageOption As Integer

        If Page.IsPostBack = False Then
            If Not IsNothing(Request.QueryString("TASKNO")) Then
                ViewState("TaskNo") = Request.QueryString("TASKNO")
            End If
            If Not IsNothing(Request.QueryString("CALLNO")) Then
                ViewState("CallNo") = Request.QueryString("CALLNO")
            End If

            If ViewState("TaskNo") > 0 Then
                intPropTaskNumber = ViewState("TaskNo")
                intPropCallNumber = ViewState("CallNo")
            End If
        End If
        imgAttach.Attributes.Add("onclick", "return OpenAtt(" & ViewState("CompanyID") & "," & ViewState("CallNo") & ", " & ViewState("TaskNo") & ");")
        imgComment.Attributes.Add("onclick", "return OpenComm(" & ViewState("TaskNo") & ", '0'," & ViewState("CompanyID") & "," & ViewState("CallNo") & ");")
        imgForms.Attributes.Add("onclick", "return OpenForms();")
        SetCommentFlag(intImageOption, mdlMain.CommentLevel.TaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
        Select Case intImageOption
            Case 1
                imgComment.ImageUrl = "../../Images/comment2.gif"
            Case 2
                imgComment.ImageUrl = "../../Images/comment_Unread.gif"
            Case Else
                imgComment.ImageUrl = "../../Images/comment_blank.gif"
        End Select
        strhiddenImage = Request.Form("txthiddenImage")
        If strhiddenImage <> "" Then
            Try
                Select Case strhiddenImage
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If UpdateorSave() = True Then
                            'Response.Write("<script>self.opener.Form1.submit();</script>")
                            Response.Write("<script>window.close();</script>")
                        End If
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            '                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If UpdateorSave() = True Then
                            'Response.Write("<script>self.opener.callrefresh();</script>")
                        End If
                    Case "Close"
                        'Response.Write("<script>self.opener.callrefresh();</script>")
                        Response.Write("<script>window.close();</script>")
                    Case "Reset"

                End Select
            Catch ex As Exception
                CreateLog("Task_Edit", "Load-132", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If

        If Page.IsPostBack = False Then
            FillData()
        End If

        If IsPostBack = True Then
            If mStrTaskStatus = "CLOSED" Then
                DisableTaskInfo()
            Else
                EnableTaskInfo()
            End If
        End If
        If Request.QueryString("ReadOnly") = 1 Then
            DisableTaskInfo()
        End If

    End Sub

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub

    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Add(Msg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub

#End Region

#Region "Clear TextBoxes"
    Private Sub ClearTextBox()
        CDDLStatus.Text = " "
        CDDLPriority.Text = " "
        txtSubject.Text = ""
        CDDLTaskOwner.Text = " "
        CDDLTaskType.Text = " "
    End Sub
#End Region

#Region "Fill Values to Save in an arrayList"

    Private Function UpdateorSave() As Boolean

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short
        Dim dtFlag As Boolean

        lstError.Items.Clear()
        Dim callStartdate As DateTime
        callStartdate = SQL.Search("Call_View", "CheckCallDate", "Select CM_DT8_Request_Date from t040011 where CM_NU9_Call_No_PK='" & Val(ViewState("CallNo")) & "' and CM_NU9_Comp_Id_FK='" & Val(ViewState("CompanyID")) & "'")
        If Convert.ToDateTime(dtStartDate.Text) < callStartdate.ToString("yyyy-MMM-dd") Then
            lstError.Items.Add("Task start date should be greater than Call Start Date")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return False
            Exit Function
        End If
        Dim strStatus As String
        strStatus = GetStatus(ViewState("CompanyID"), ViewState("CallNo"), mdlMain.StatusType.CallStatus)
        If strStatus = "CLOSED" Then
            lstError.Items.Add("Call Status is " & WSSSearch.GetCallStatus(Val(ViewState("CallNo")), Val(ViewState("CompanyID"))) & " so You cannot change the Task...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return False
            Exit Function
        End If

        If chkMandatory.Checked = True Then
            If CDDLStatus.Text.Trim.Equals("CLOSED") Then
                mstGetFunctionValue = WSSSearch.SearchAction(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                If mstGetFunctionValue.ErrorCode = 0 Then
                Else
                    lstError.Items.Add("Task must have at least one action with Mandatory status with Hours before closing it...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                End If
            End If
        End If

        If CDDLTaskType.Text.Trim.Equals("") Then
            lstError.Items.Add("Task Type cannot be blank...")
            shFlag = 1
        End If
        If CDDLTaskOwner.Text.Trim.Equals("") Then
            lstError.Items.Add("Task Owner cannot be blank...")
            shFlag = 1
        End If
        If CDDLStatus.Text.Trim.Equals("") Then
            lstError.Items.Add("Task Status cannot be blank...")
            shFlag = 1
        End If
        If txtSubject.Text.Trim.Equals("") Then
            lstError.Items.Add("Task Subject cannot be blank...")
            shFlag = 1
        End If
        If txttaskorder.Text.Trim.Equals("") Then
            lstError.Items.Add("Task Order cannot be blank...")
            shFlag = 1
        End If
        If Val(txttaskorder.Text.Trim) < 1 Then
            lstError.Items.Add("Task Order cannot be less than 1...")
            shFlag = 1
        End If

        If CheckRealValuesForDDLS() = False Then
            shFlag = 1
        End If

        Dim intMaxOrder As Integer
        intMaxOrder = SQL.Search("", "", "select count(*) from T040021 where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")))
        If Val(txttaskorder.Text.Trim) > intMaxOrder Then
            lstError.Items.Add("Task Order cannot be greater than " & intMaxOrder.ToString & "...")
            shFlag = 1
        End If
        If dtStartDate.Text.Trim <> "" And dtEstFinishDate.Text.Trim <> "" Then
            If CDate(dtEstFinishDate.Text.Trim) < CDate(dtStartDate.Text.Trim) Then 'Or CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                lstError.Items.Add("Estimated Close date cannot be less than Start date...")
                shFlag = 1
            End If
        End If
        Dim dtCallDate As Date = SQL.Search("Task_Edit", "UpdateorSave-263", "Select CM_DT8_Request_Date from T040011 where CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "")

        If dtEstFinishDate.Text.Trim <> "" Then
            If IsDate(dtEstFinishDate.Text) = False Then
                lstError.Items.Add("Check date format of estimated close date...")
                shFlag = 1
            ElseIf CDate(dtEstFinishDate.Text.Trim) < CDate(dtCallDate.ToShortDateString) Then
                If CDDLStatus.Text.Trim.ToUpper <> "CLOSED" Then
                    lstError.Items.Add("Estimated Close date cannot be less than Call date...")
                    shFlag = 1
                End If
            End If

        End If

        Dim intAddressNo As Integer
        intAddressNo = SQL.Search("Task_Edit", "UpdateorSave-305", "select UM_IN4_Address_No_FK from T060011 where UM_IN4_Address_No_FK=" & CDDLTaskOwner.SelectedValue.Trim & "")
        If intAddressNo <= 0 Then
            lstError.Items.Add("Task Owner mismatch...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Exit Function
        End If

        mstGetFunctionValue = CheckUserValiditity(CDDLTaskOwner.SelectedValue.Trim)
        If mstGetFunctionValue.FunctionExecuted = False Then
            lstError.Items.Clear()
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Function
        End If

        Try
            'Add new Column ETC
            arrColumns.Add("TM_FL8_ETC")
            Dim TaskStartdate As DateTime
            TaskStartdate = SQL.Search("Call_View", "CheckCallDate", "Select TM_DT8_Task_Date from t040021 where TM_NU9_Call_No_FK='" & Val(ViewState("CallNo")) & "' and TM_NU9_Comp_ID_FK='" & Val(ViewState("CompanyID")) & "' and TM_NU9_Task_no_PK ='" & Val(ViewState("TaskNo")) & "'")
            If Not dtStartDate.Text.Trim.Equals("") Then
                If Not Convert.ToDateTime(dtStartDate.Text) = TaskStartdate.ToString("yyyy-MMM-dd") Then
                    arrColumns.Add("TM_DT8_Task_Date")
                End If
            End If

            arrColumns.Add("TM_NU9_Task_Order")
            arrColumns.Add("TM_VC1000_Subtsk_Desc")
            arrColumns.Add("TM_VC8_task_type")
            arrColumns.Add("TM_FL8_Est_Hr")
            arrColumns.Add("TM_VC8_Supp_Owner")
            'arrColumns.Add("TM_NU9_Assign_by")
            arrColumns.Add("TM_VC8_Priority")
            arrColumns.Add("TM_NU9_Agmt_No")
            arrColumns.Add("TM_NU9_Dependency")
            arrColumns.Add("TM_CH1_Mandatory")
            If Not dtEstFinishDate.Text.Trim.Equals("") Then
                arrColumns.Add("TM_DT8_Est_close_date")
            End If
            arrColumns.Add("TM_VC50_Deve_status")

            arrColumns.Add("TM_DT8_Close_Date")
            arrColumns.Add("TM_DT8_Task_Close_Date")
            arrColumns.Add("TM_CH1_Invoice_Pending")
            arrColumns.Add("TM_CH1_Forms")

            arrRows.Add(Val(txtETC.Text)) 'New Column ETC
            'arrRows.Add(strTaskDate)

            If Not dtStartDate.Text.Trim.Equals("") Then
                If Not Convert.ToDateTime(dtStartDate.Text) = TaskStartdate.ToString("yyyy-MMM-dd") Then
                    arrRows.Add(dtStartDate.Text.Trim & " " & TimeOfDay)
                End If
            End If
            arrRows.Add(txttaskorder.Text.Trim)
            arrRows.Add(txtSubject.Text.Trim)
            arrRows.Add(CDDLTaskType.Text.Trim.ToUpper)
            If txtEstimatedHrs.Text.Trim.Equals("") Then
                arrRows.Add("0")
            Else
                arrRows.Add(txtEstimatedHrs.Text.Trim)
            End If
            arrRows.Add(CDDLTaskOwner.SelectedValue.Trim)
            'arrRows.Add(HttpContext.Current.Session("PropUserID"))
            arrRows.Add(CDDLPriority.Text.Trim.ToUpper)
            arrRows.Add(Val(CDDLAgreement.CDDLGetValue.trim))
            arrRows.Add(IIf(DDLDependency.SelectedValue = "", DBNull.Value, DDLDependency.SelectedValue))
            If chkMandatory.Checked = True Then
                arrRows.Add("M")
            Else
                arrRows.Add("O")
            End If
            If Not dtEstFinishDate.Text.Trim.Equals("") Then
                arrRows.Add(dtEstFinishDate.Text.Trim)
            End If
            arrRows.Add(CDDLStatus.Text.Trim.ToUpper)

            If CDDLStatus.Text.Trim = "CLOSED" Then
                arrRows.Add(Now())
                arrRows.Add(Now())
                arrRows.Add("Y") 'Pending Invoice when task is closed
                dtFlag = False
            Else
                arrRows.Add(DBNull.Value)
                arrRows.Add(DBNull.Value)
                arrRows.Add(DBNull.Value) 'Pending Invoice
                dtFlag = True
            End If
            'Update Bit only if it was not previously 1 
            Dim chrFormBit As Char = SQL.Search("TemplateTask_Edit", "UpdateorSave-523", "Select TM_CH1_Forms from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & "")
            If chrFormBit <> "1" Then
                'Get Form is Assigned then Put Value 2 Else 0
                If WSSSearch.GetNoOfAssignedForms(CDDLTaskType.Text, False, ViewState("CompanyID"), 0, ViewState("CallNo")) > 0 Then
                    arrRows.Add(2)
                Else
                    arrRows.Add(0)
                End If
            Else
                arrRows.Add(1)
            End If
            If shFlag = 0 Then
                lstError.Items.Clear()
                ChangeTaskOrder(mdlMain.EnumTaskOrder.UpdateTask, ViewState("OldTaskOrder"), ViewState("CompanyID"), ViewState("CallNo"), txttaskorder.Text.Trim)
                If mStrPrvTaskStatus <> CDDLStatus.Text Then
                    mstGetFunctionValue = WSSUpdate.UpdateTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"), arrColumns, arrRows, True)
                Else
                    mstGetFunctionValue = WSSUpdate.UpdateTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"), arrColumns, arrRows)
                End If
                ViewState("OldTaskOrder") = txttaskorder.Text.Trim
                ' -- update Status after updating database and creating Log
                mStrPrvTaskStatus = CDDLStatus.Text
                'check the call status for adding comments to the call
                If ViewState("TaskStatus") <> "" And ViewState("TaskStatus") <> CDDLStatus.Text Then
                    If Not (CDDLStatus.Text.Trim.ToUpper = "ASSIGNED" Or CDDLStatus.Text.Trim.ToUpper = "PROGRESS") Then
                        CreateTaskAutoAction(ViewState("TaskStatus"), CDDLStatus.Text.Trim.ToUpper, ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                        'Call SaveTaskStatusComments(ViewState("TaskStatus"), CDDLStatus.CDDLGetValue)
                    End If
                End If
                ViewState("TaskStatus") = CDDLStatus.Text.Trim.ToUpper

                If mstGetFunctionValue.ErrorCode = 0 Then
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    If GetFiles() = True Then
                        shFlag = 0
                    Else
                        shFlag = 1
                    End If

                    Call FillData()
                    'save current status in shared variable
                    mStrTaskStatus = CDDLStatus.Text.Trim.ToUpper
                    garFileID.Clear()
                    If shFlag = 1 Then
                        lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        shFlag = 0
                        'Exit Function
                    Else
                        lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    End If
                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                End If
                If CDDLStatus.Text.Trim.ToUpper = "CLOSED" And Request.QueryString("ReadOnly") = 1 Then
                    'for clear action grid to do list******************
                    '*******
                    ViewState("TaskNo") = 0
                    ViewState("CallNo") = 0
                    '*******No Need of this line*************
                    Response.Write("<script>self.opener.Form1.txtrowvaluesCall.value =0;  </script>")
                    Response.Write("<script>self.opener.Form1.txtTask.value =0;  </script>")
                    '*****************************
                End If
                Return True
            End If
        Catch ex As Exception
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("Task_Edit", "Updateorsave-390", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function
#End Region

#Region "Fill Data"

    Private Sub FillData()
        Dim drTask As SqlDataReader
        Dim blnStatus As Boolean
        Try
            drTask = SQL.Search("Task_Edit", "FillData-437", "Select T1.UM_VC50_UserID TaskOwner,T2.UM_VC50_UserID AssignBy,* from T040021,T040011,T060011 T1, T060011 T2 where T1.UM_IN4_Address_No_FK=TM_VC8_Supp_Owner and T2.UM_IN4_Address_No_FK=TM_NU9_Assign_by and TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK= " & ViewState("TaskNo").ToString & " and TM_NU9_Call_No_FK=CM_NU9_Call_No_PK and TM_NU9_Comp_ID_FK= CM_Nu9_Comp_id_fk and TM_NU9_Comp_ID_FK =" & ViewState("CompanyID"), SQL.CommandBehaviour.CloseConnection, blnStatus)

            If blnStatus = True Then
                drTask.Read()
                'New Column ETC
                If IsDBNull(drTask.Item("TM_FL8_Est_Hr")) = False Then
                    txtETC.Text = IIf(IsDBNull(drTask.Item("TM_FL8_ETC")), Val(drTask.Item("TM_FL8_Est_Hr")), drTask.Item("TM_FL8_ETC"))
                Else
                    txtETC.Text = 0
                End If

                strTaskDate = IIf(IsDBNull(drTask.Item("TM_DT8_Task_Date")), "", drTask.Item("TM_DT8_Task_Date"))
                ViewState("FromBit") = IIf(IsDBNull(drTask.Item("TM_CH1_Forms")), "", drTask.Item("TM_CH1_Forms"))
                mStrTaskStatus = IIf(IsDBNull(drTask.Item("TM_VC50_Deve_status")), "", drTask.Item("TM_VC50_Deve_status"))
                CDDLStatus.Text = IIf(IsDBNull(drTask.Item("TM_VC50_Deve_status")), "", drTask.Item("TM_VC50_Deve_status"))
                If IsPostBack = False Then
                    mStrPrvTaskStatus = IIf(IsDBNull(drTask.Item("TM_VC50_Deve_status")), "", drTask.Item("TM_VC50_Deve_status"))
                End If
                'For Status comments
                ViewState("TaskStatus") = IIf(IsDBNull(drTask.Item("TM_VC50_Deve_status")), "", drTask.Item("TM_VC50_Deve_status"))
                txtSubject.Text = IIf(IsDBNull(drTask.Item("TM_VC1000_Subtsk_Desc")), "", drTask.Item("TM_VC1000_Subtsk_Desc"))
                If IsDBNull(drTask.Item("TM_DT8_Task_Date")) = False Then
                    dtStartDate.Text = SetDateFormat(drTask.Item("TM_DT8_Task_Date"), mdlMain.IsTime.DateOnly)
                Else
                    dtStartDate.Text = 0
                End If
                'txtStartDate.Text = SetDateFormat(IIf(IsDBNull(drTask.Item("TM_DT8_Task_Date")), "", drTask.Item("TM_DT8_Task_Date")), mdlMain.IsTime.WithTime)
                txtAssignBy.Text = IIf(IsDBNull(drTask.Item("AssignBy")), "", drTask.Item("AssignBy"))
                CDDLTaskType.Text = IIf(IsDBNull(drTask.Item("TM_VC8_task_type")), "", drTask.Item("TM_VC8_task_type"))
                txtEstimatedHrs.Text = IIf(IsDBNull(drTask.Item("TM_FL8_Est_Hr")), "", drTask.Item("TM_FL8_Est_Hr"))
                txttaskorder.Text = IIf(IsDBNull(drTask.Item("TM_NU9_Task_Order")), "", drTask.Item("TM_NU9_Task_Order")) ' task order field
                ViewState("OldTaskOrder") = Val(txttaskorder.Text.Trim) 'stores the old task order
                CDDLTaskOwner.SelectedValue = IIf(IsDBNull(drTask.Item("TM_VC8_Supp_Owner")), "", drTask.Item("TM_VC8_Supp_Owner")) ' , False, drTask.Item("TaskOwner"))
                CDDLPriority.Text = IIf(IsDBNull(drTask.Item("TM_VC8_Priority")), "", drTask.Item("TM_VC8_Priority"))
                If IsDBNull(drTask.Item("TM_NU9_Dependency")) = True Then
                    DDLDependency.SelectedValue = ""
                Else
                    DDLDependency.SelectedValue = drTask.Item("TM_NU9_Dependency")
                End If

                If IsDBNull(drTask.Item("TM_NU9_Agmt_No")) = True Then
                    CDDLAgreement.CDDLSetBlank()
                Else
                    CDDLAgreement.CDDLSetSelectedItem(drTask.Item("TM_NU9_Agmt_No"))
                End If

                If IsDBNull(drTask.Item("TM_DT8_Est_close_date")) = False Then
                    dtEstFinishDate.Text = SetDateFormat(drTask.Item("TM_DT8_Est_close_date"), mdlMain.IsTime.DateOnly)
                End If
                If IsDBNull(drTask.Item("TM_CH1_Mandatory")) Then
                    chkMandatory.Checked = False
                Else
                    If drTask.Item("TM_CH1_Mandatory") = "M" Then
                        chkMandatory.Checked = True
                    Else
                        chkMandatory.Checked = False
                    End If
                End If
            End If

            txtCallType.Value = IIf(IsDBNull(drTask.Item("CM_VC8_Call_Type")), drTask.Item("CM_VC8_Call_Type"), drTask.Item("CM_VC8_Call_Type"))
            txtCallNo.Value = IIf(IsDBNull(drTask.Item("TM_NU9_Call_No_FK")), drTask.Item("TM_NU9_Call_No_FK"), drTask.Item("TM_NU9_Call_No_FK"))
            txtTaskNo.Value = IIf(IsDBNull(drTask.Item("TM_NU9_Task_no_PK")), drTask.Item("TM_NU9_Task_no_PK"), drTask.Item("TM_NU9_Task_no_PK"))
            'for txtbox taskID to show task id
            '""""saurabh""
            txttaskid.Text = IIf(IsDBNull(drTask.Item("TM_NU9_Task_no_PK")), drTask.Item("TM_NU9_Task_no_PK"), drTask.Item("TM_NU9_Task_no_PK"))
            If IsDBNull(drTask.Item("TM_CH1_Forms")) OrElse drTask.Item("TM_CH1_Forms") = 0 Then ' form is not available
                imgForms.ImageUrl = "../../Images/White.gif"
            ElseIf drTask.Item("TM_CH1_Forms") = 1 Then ' Form is filled
                imgForms.ImageUrl = "../../Images/Form1.jpg"
            ElseIf drTask.Item("TM_CH1_Forms") = 2 Then ' Form is assigned but not filled
                imgForms.ImageUrl = "../../Images/Form2.gif"
            End If
            If mStrTaskStatus = "CLOSED" Then
                DisableTaskInfo()
            Else
                EnableTaskInfo()
            End If
        Catch ex As Exception
            CreateLog("Task_Edit", "FillData-617", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

    End Sub
#End Region

    Private Function GetFiles() As Boolean
        Dim sqrdTempFiles As SqlDataReader
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim blnRead As Boolean
            ' For Tasks
            sqrdTempFiles = SQL.Search("Task_Edit", "GetFiles-481", "select * from T040041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)
            If blnRead = True Then
                While sqrdTempFiles.Read
                    mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                    mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                    mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                    CreateFolder(ViewState("CallNo"), ViewState("TaskNo"))
                End While
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Task_Edit", "GetFiles-458", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim
                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBConnection = strSQL
                SQL.DBTracing = False
                dblVersionNo = SQL.Search("Task_Edit", "CreateFolder-560", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")
                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If
                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If
                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim
                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                End If
            End If
        Catch ex As Exception
            CreateLog("Task_Edit", "CreateFolder-573", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Sub DisableTaskInfo()
        Try
            CDDLAgreement.Enabled = False
            DDLDependency.Enabled = False
            CDDLPriority.Enabled = False
            CDDLTaskType.Enabled = False
            txtEstimatedHrs.ReadOnly = True
            txttaskorder.ReadOnly = True
            txttaskid.ReadOnly = True
            txtSubject.ReadOnly = True
            chkMandatory.Enabled = False
            imgForm.Enabled = False
        Catch ex As Exception
            CreateLog("Task_Edit", "DisableTaskInfo-867", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Sub EnableTaskInfo()
        Try
            DDLDependency.Enabled = True
            CDDLPriority.Enabled = True
            CDDLTaskOwner.Enabled = True
            CDDLTaskType.Enabled = True
            txtEstimatedHrs.ReadOnly = False
            txttaskorder.ReadOnly = False
            txttaskid.ReadOnly = True
            txtSubject.ReadOnly = False
            chkMandatory.Enabled = True
            imgAttach.Disabled = False
            imgForm.Enabled = True
        Catch ex As Exception
            CreateLog("Task_Edit", "EnableTaskInfo-880", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Function SaveTaskStatusComments(ByVal OldStatus As String, ByVal NewStatus As String) As Boolean
        Try
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

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
            arColumnName.Add("CM_CH1_Status_Comment")
            arRowData.Add(IIf(CDDLTaskOwner.SelectedValue = "", System.DBNull.Value, CDDLTaskOwner.SelectedValue))
            arRowData.Add("1") 'Read Status Flag
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(Now)
            arRowData.Add("Task Status changed from [" & OldStatus & "] to [" & NewStatus & "] by " & Session("PropUserName"))
            arRowData.Add("T")
            arRowData.Add(Val(ViewState("CallNo")))
            arRowData.Add(Val(ViewState("TaskNo")))
            arRowData.Add(0)
            arRowData.Add("F")
            arRowData.Add(ViewState("CompanyID"))
            arRowData.Add("External")
            arRowData.Add("")  'SQL.Search("", "", "select CI_VC28_Email_1 from T010011 where CI_NU8_Address_Number=" & Val(CDDLTaskOwner.CDDLGetValue)))
            arRowData.Add("F")
            arRowData.Add("T")

            mstGetFunctionValue = WSSSave.SaveComments(arColumnName, arRowData)

            If mstGetFunctionValue.ErrorCode = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Call Entry", "SaveStatusComments-3527", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
#Region " Fills Rad combo and checks validates them during save"
    ''' <summary>
    ''' Fills the rad combo boxes (Task Type, priority, Task Owner) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillRadCombos()
        Dim dsComboTaskType As New DataSet
        Dim dsComboTaskOwner As New DataSet
        Dim dsComboPriority As New DataSet
        Dim dsComboStatus As New DataSet

        Try
            'Task Type combo
            dsComboTaskType = objCommonFunctionsBLL.FillRadTaskType(Val(ViewState("CompanyID")))
            CDDLTaskType.Items.Clear()
            CDDLTaskType.DataSource = dsComboTaskType
            For Each data As DataRow In dsComboTaskType.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("ID"))
                item.Value = CStr(data("ID"))
                CDDLTaskType.Items.Add(item)
                item.DataBind()
            Next
            ' Task Owner Combo
            dsComboTaskOwner = objCommonFunctionsBLL.FillRadTaskOwner(Val(ViewState("CompanyID")), Val(ViewState("ProjectID")))
            CDDLTaskOwner.Items.Clear()
            CDDLTaskOwner.DataSource = dsComboTaskOwner
            For Each data As DataRow In dsComboTaskOwner.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("Name"))
                item.Value = CStr(data("ID"))
                CDDLTaskOwner.Items.Add(item)
                item.DataBind()
            Next
            ' Task Priority Combo
            dsComboPriority = objCommonFunctionsBLL.FillRadTaskPriority(Val(ViewState("CompanyID")))
            CDDLPriority.Items.Clear()
            CDDLPriority.DataSource = dsComboPriority
            For Each data As DataRow In dsComboPriority.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("ID"))
                item.Value = CStr(data("ID"))
                CDDLPriority.Items.Add(item)
                item.DataBind()
            Next
            ' Status
            dsComboStatus = objCommonFunctionsBLL.FillRadStatus(Val(ViewState("CompanyID")))
            CDDLStatus.Items.Clear()
            CDDLStatus.DataSource = dsComboStatus
            For Each data As DataRow In dsComboStatus.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("ID"))
                item.Value = CStr(data("ID"))
                CDDLStatus.Items.Add(item)
                item.DataBind()
            Next
        Catch ex As Exception
            CreateLog("WSS", "Call_View-FillRadCombos-3142", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
    ''' <summary>
    ''' Validated values in the combo (Priority and Task Type)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckRealValuesForDDLS() As Boolean
        Try
            Dim bytError As Byte = 0
            Dim strCheckDropDownValue As String = ""
            'Check for task type
            strCheckDropDownValue = SQL.Search("Call_View", "CheckRealValuesForDDLS-892", "Select Name from UDC where name='" & CDDLTaskType.Text.Trim & "'")
            If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                lstError.Items.Add("Task-type mismatch.")
                bytError = 1
            Else
                bytError = 0
            End If

            'Check for Priority
            strCheckDropDownValue = SQL.Search("Call_View", "CheckRealValuesForDDLS-892", "Select Name from UDC where name='" & CDDLPriority.Text.Trim & "'")
            If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                lstError.Items.Add("Priority mismatch.")
                bytError += 1
            Else
                bytError += 0
            End If

            ' Check for Status
            Select Case CDDLStatus.Text.ToUpper
                Case "PROGRESS"
                    If Request.QueryString("ReadOnly") <> 1 And Request.QueryString("ReadOnly") <> 0 Then
                        lstError.Items.Add("Status mismatch.")
                        bytError = 1
                    End If
                Case "CLOSED"
                Case Else
                    ' If we are opening if from to-do list
                    If Request.QueryString("ReadOnly") = 1 Then
                        strCheckDropDownValue = SQL.Search("Call_View", "CheckRealValuesForDDLS-892", "select SU_VC50_Status_Name from T040081,T010011 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_CompID*=CI_NU8_Address_Number and ( SU_NU9_CompID=" & Session("PropCompanyID") & " or SU_NU9_CompID=0 ) " & "  and SU_VC50_Status_Name='" & CDDLStatus.Text.Trim & "' union select SU_VC50_Status_Name from T040081 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_ID_PK>3 and SU_NU9_CompID=0 and SU_VC50_Status_Name='" & CDDLStatus.Text.Trim & "'")
                        If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                            lstError.Items.Add("Status mismatch.")
                            bytError += 1
                        Else
                            bytError += 0
                        End If

                        ' if we are opening it from call-task view
                    Else
                        If CDDLStatus.Text.Trim.ToUpper.Equals("ASSIGNED") = False Then
                            strCheckDropDownValue = SQL.Search("Call_View", "CheckRealValuesForDDLS-892", "select SU_VC50_Status_Name from T040081,T010011 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_CompID*=CI_NU8_Address_Number and (SU_NU9_CompID=" & Session("PropCompanyID") & " or SU_NU9_CompID=0 )" & "  and SU_VC50_Status_Name='" & CDDLStatus.Text.Trim & "' union select SU_VC50_Status_Name from T040081 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_ID_PK>3 and SU_NU9_CompID=0 and SU_VC50_Status_Name='" & CDDLStatus.Text.Trim & "'")
                            If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                                lstError.Items.Add("Status mismatch.")
                                bytError += 1
                            Else
                                bytError += 0
                            End If
                        End If
                    End If
            End Select

            If bytError > 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
        End Try
    End Function
#End Region
End Class
