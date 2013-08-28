'***********************************************************************************************************
' Page                   : - TemplateTask_Edit
' Purpose                : - it is for Editing purpose that conatins editable fields that user can change                                 like status,task type ,estimated Hours,priority,aggrement etc. 
' Tables used            : - UDC, T010011, T040081, T050031, T080011, T060011, T050041, T050051

' Date					Author					    	Modification Date					Description
' 14/03/06			Ranvijay/Harpreet/Sachin		14/11/2007			        		Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Data
Imports System.IO
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class AdministrationCenter_Template_TemplateTask_Edit
    Inherits System.Web.UI.Page
#Region "Form Level Variables"
    Public intPropTaskNumber As Integer
    Public intPropCallNumber As Integer
    Dim mintFileID As Integer
    Dim mstrFileName As String
    Dim mstrFilePath As String
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1

            ViewState("PropCAComp") = Request.QueryString("companyID")
            ViewState("PropTaskNumber") = Request.QueryString("TaskNO")
            ViewState("SAddressNumber_Template") = Request.QueryString("TemplateID")
            If Not IsPostBack Then
                lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
                txtEstimatedHrs.Attributes.Add("onkeypress", "UsedHour('txtEstimatedHrs')")
                ' txtSubject.Attributes.Add("onkeypress", "return MaxLength('" & txtSubject.ClientID & "','100');")
                txtSubject.Attributes.Add("onmousemove", "ShowToolTip(this,1000);")
                imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
                imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
                imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
                imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
                imgForm.Attributes.Add("Onclick", "return SaveEdit('Forms','" & ViewState("PropTaskNumber") & "');")
                'Code by Atul
                'To allow numeric chracters only
                txttaskorder.Attributes.Add("onkeypress", "NumericOnly()")
                txttaskid.Attributes.Add("onkeypress", "NumericOnly()")
            End If
            ' -- Coding for CDDL's\
            ' -- Task Type

            CDDLTaskType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TKTY""" & _
 " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
 " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""TKTY""" & _
 " and UDC.Company=0 Order By Name"

            CDDLTaskType.CDDLMandatoryField = False
            CDDLTaskType.CDDLFillDropDown(10)
            '------------------------------------------
            ' -- Task Owner
            CDDLTaskOwner.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid + ' [' + T1.CI_VC36_Name +']' as Name,T2.ci_vc36_name  as Company FROM t060011,t010011 T1,t010011 T2 where T2.ci_nu8_address_number=um_in4_company_ab_id and T1.ci_nu8_address_number=UM_IN4_Address_No_FK and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(Session("PropProjectID")) & " and PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name"
            'CDDLTaskOwner.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(Session("PropProjectID")) & " and PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name"
            CDDLTaskOwner.CDDLMandatoryField = True
            CDDLTaskOwner.CDDLUDC = False
            CDDLTaskOwner.CDDLFillDropDown(10, False)
            '------------------------------------------
            '--Priority

            'CDDLPriority.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""PRIO""" & _
            ' " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
            '" Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
            ' " and UDC.Company=0 Order By Name"
            'CDDLPriority.CDDLFillDropDown(10)
            '-----------------------------------------
            ' -- Status

            CDDLStatus.CDDLQuery = "select SU_VC50_Status_Name as ID,SU_VC500_Status_Description as description,CI_VC36_Name as Company " & _
           " from T040081,T010011 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_CompID*=CI_NU8_Address_Number and SU_NU9_CompID=" & ViewState("PropCAComp") & "  union " & _
           "select SU_VC50_Status_Name as Name,SU_VC500_Status_Description as description,"""" as Company " & _
           " from T040081 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_ID_PK>3 and SU_NU9_CompID=0 Order By ID"
            CDDLStatus.CDDLFillDropDown(10)
            '-----------------------------------------
            ' -- Dependency
            If Not IsPostBack Then
                FillNonUDCDropDown(DDLDependancy, "select TTM_NU9_Task_No_Pk , TTM_NU9_Task_No_Pk from T050031 where isnull(TTM_NU9_Dependency,0)<>" & Val(Request.QueryString("TASKNO")) & " and TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template") & " and TTm_nu9_comp_id_fk =" & ViewState("PropCAComp") & " and TTM_NU9_Task_No_Pk<>" & Request.QueryString("TASKNO") & " and TTm_vc50_deve_status<>'CLOSED'", True)
            End If
            '-----------------------------------------

            '--Agreement
            CDDLAgreement.CDDLQuery = "select AG_NU8_ID_PK as ID,AG_VC200_Desc Description,CI_VC36_Name ""Contact Person"" from T080011 Ag,T010011 AB where ag.AG_VC8_Cust_Name =8 and ab.CI_NU8_Address_Number = ag.AG_VC8_Contact_Person"
            CDDLAgreement.CDDLMandatoryField = True
            CDDLAgreement.CDDLFillDropDown(10)
            '-----------------------------------------

            If IsPostBack = True Then
                CDDLTaskType.CDDLSetItem()
                CDDLTaskOwner.CDDLSetItem()
                CDDLStatus.CDDLSetItem()
                'CDDLPriority.CDDLSetItem()
                CDDLAgreement.CDDLSetItem()
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "Load-234", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try


        Dim sqrdAB As SqlDataReader
        Dim blnCheck As Boolean
        Dim strhiddenImage As String
        Dim intImageOption As Integer

        cpnlError.Visible = False
        Call txtCSS(Me.Page)   'From Module
        If Page.IsPostBack = False Then

            ViewState("PropTaskNumber") = Request.QueryString("TASKNO")
            intPropTaskNumber = ViewState("PropTaskNumber")
            intPropCallNumber = Request.QueryString("CallNo") 'HttpContext.Current.Session("PropCallNumber")
        End If

        If Page.IsPostBack = False Then
            If Not IsNothing(Request.QueryString("TASKNO")) Then
                ViewState("PropTaskNumber") = Request.QueryString("TASKNO")
            End If

            If ViewState("PropTaskNumber") > 0 Then
                intPropTaskNumber = ViewState("PropTaskNumber")
                intPropCallNumber = Request.QueryString("CallNo") 'HttpContext.Current.Session("PropCallNumber")
            End If
        End If
        imgComment.Attributes.Add("onclick", "return OpenComm(" & Request.QueryString("CallNo") & ", " & ViewState("PropTaskNumber") & "," & ViewState("PropCAComp") & ");")

        SetCommentFlag(intImageOption, mdlMain.CommentLevel.TemplateTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
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
                            '  cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If UpdateorSave() = True Then
                            Response.Write("<script>self.opener.callrefresh();</script>")
                            Response.Write("<script>window.close();</script>")
                        End If
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            ' cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        Call UpdateorSave()
                    Case "Close"
                        Response.Write("<script>self.opener.callrefresh();</script>")
                        Response.Write("<script>window.close();</script>")
                    Case "Reset"

                End Select
            Catch ex As Exception
                CreateLog("Template Task_Edit", "Load-125", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If
        'Security Block

        Dim intId As Integer

        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 421
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If

        'End of Security Block

        If Page.IsPostBack = False Then
            FillData()
        End If

    End Sub
#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        'cpnlError.Visible = True
        lstError.Items.Clear()
        lstError.Items.Add(ErrMsg)
        'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgError)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        'cpnlError.Visible = True
        lstError.Items.Add(Msg)
        'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub
#End Region

#Region "Clear TextBoxes"
    Private Sub ClearTextBox()
        CDDLStatus.CDDLSetSelectedItem("")
        'CDDLPriority.CDDLSetSelectedItem("")
        txtSubject.Text = ""
        CDDLTaskOwner.CDDLSetSelectedItem("")
        CDDLTaskType.CDDLSetSelectedItem("")
    End Sub
#End Region

#Region "Fill Values to Save in an arrayList"

    Private Function UpdateorSave() As Boolean

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intCallNo As Integer
        Dim ctrlTextBox As Control

        lstError.Items.Clear()

        If chkMandatory.Checked = True Then

        End If

        If CDDLTaskType.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Task Type cannot be blank...")
            shFlag = 1
        ElseIf CDDLTaskOwner.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Task Owner cannot be blank...")
            shFlag = 1
        ElseIf CDDLStatus.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Task Status cannot be blank...")
            shFlag = 1
        End If
        'Add by atul
        'To implement validations
        If txttaskorder.Text.Trim.Equals("") Then
            lstError.Items.Add("Task Order cannot be blank...")
            shFlag = 1
        End If
        If Val(txttaskorder.Text.Trim) < 1 Then
            lstError.Items.Add("Task Order cannot be less than 1...")
            shFlag = 1
        End If
        Dim intMaxOrder As Integer
        'intMaxOrder = SQL.Search("", "", "select count(*) from T050031  where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and TTM_NU9_Call_No_FK=" & Val(HttpContext.Current.Session("PropCallNumber")))
        intMaxOrder = SQL.Search("", "", "select count(*) from T050031  where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template"))

        If Val(txttaskorder.Text.Trim) > intMaxOrder Then
            lstError.Items.Add("Task Order cannot be greater than " & intMaxOrder.ToString & "...")
            shFlag = 1
        End If
        Dim dtCallDate As Date = SQL.Search("TemplateTask_Edit", "UpdateorSave-237", "Select TCM_DT8_Request_Date from T050021 where TCM_NU9_Call_No_PK=" & Request.QueryString("CallNo") & " and TCM_NU9_CompID_FK=" & ViewState("PropCAComp") & "")

        If shFlag = 1 Then
            'cpnlError.Visible = True
            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Exit Function
        End If

        lstError.Items.Clear()

        strUDCType = "TKTY"
        strName = CDDLTaskType.CDDLGetValue.Trim.ToUpper
        strErrorMessage = "Task Type Mismatch..."

        If CheckUDCValue(0, strUDCType, strName) = False Then
            lstError.Items.Add(strErrorMessage)
            shFlag = 1
        End If

        Dim intAddressNo As Integer
        intAddressNo = SQL.Search("TemplateTask_Edit", "UpdateorSave-277", "select UM_IN4_Address_No_FK from T060011 where UM_IN4_Address_No_FK =" & CDDLTaskOwner.CDDLGetValue.Trim & "")
        If intAddressNo <= 0 Then
            lstError.Items.Add("Task Owner mismatch...")
            shFlag = 1
        End If

        'strUDCType = "PRIO"
        'strName = CDDLPriority.CDDLGetValue.Trim.ToUpper
        'strErrorMessage = "Task Priority Mismatch..."

        'If CheckUDCValue(0, strUDCType, strName) = False Then
        '    lstError.Items.Add(strErrorMessage)
        '    shFlag = 1
        'End If

        If shFlag = 1 Then
            'cpnlError.Visible = True
            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Exit Function
        End If
        'Check the validaity of task owner
        mstGetFunctionValue = CheckUserValiditity(CDDLTaskOwner.CDDLGetValue.Trim)
        If mstGetFunctionValue.FunctionExecuted = False Then
            lstError.Items.Clear()
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Function
        End If

        Try

            arrColumns.Add("TTM_FL8_Est_Hr")
            arrColumns.Add("TTM_VC50_Deve_status")
            arrColumns.Add("TTM_VC1000_Subtsk_Desc")
            arrColumns.Add("TTM_VC8_task_type")
            ' arrColumns.Add("TTM_VC8_Project")
            arrColumns.Add("TTM_VC8_Supp_Owner")
            arrColumns.Add("TTM_NU9_Assign_by")
            'arrColumns.Add("TTM_VC8_Priority")
            arrColumns.Add("TTM_NU9_Agmt_No")
            arrColumns.Add("TTM_NU9_Dependency")
            arrColumns.Add("TTM_CH1_Mandatory")
            arrColumns.Add("TTM_VC50_Deve_status")
            arrColumns.Add("TTM_CH1_Forms")
            arrColumns.Add("TTM_NU9_Task_Order")

            arrRows.Add(Val(txtEstimatedHrs.Text.Trim))
            arrRows.Add(CDDLStatus.CDDLGetValue.Trim)
            arrRows.Add(txtSubject.Text.Trim)
            arrRows.Add(CDDLTaskType.CDDLGetValue.Trim)
            '  arrRows.Add(txtProject.Text.Trim)
            arrRows.Add(CDDLTaskOwner.CDDLGetValue.Trim)
            arrRows.Add(HttpContext.Current.Session("PropUserID"))
            'arrRows.Add(CDDLPriority.CDDLGetValue.Trim)
            arrRows.Add(Val(CDDLAgreement.CDDLGetValue.trim))
            arrRows.Add(IIf(DDLDependancy.SelectedValue = "", DBNull.Value, DDLDependancy.SelectedValue))
            If chkMandatory.Checked = True Then
                arrRows.Add("M")
            Else
                arrRows.Add("O")
            End If
            arrRows.Add(CDDLStatus.CDDLGetValue.Trim)
            'Update Bit only if it was not previously 1 
            Dim chrFormBit As Char = SQL.Search("TemplateTask_Edit", "UpdateorSave-413", "Select TTM_CH1_Forms from T050031 where TTM_NU9_Task_no_PK=" & ViewState("PropTaskNumber") & " AND TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & "")
            If chrFormBit <> "1" Then
                'Get Form is Assigned then Put Value 2 Else 0
                If WSSSearch.GetNoOfAssignedForms(CDDLTaskType.CDDLGetValue, True, ViewState("PropCAComp"), ViewState("SAddressNumber_Template"), intPropCallNumber) > 0 Then
                    arrRows.Add(2)
                Else
                    arrRows.Add(0)
                End If
            Else
                arrRows.Add(1)
            End If
            arrRows.Add(txttaskorder.Text.Trim)
            If shFlag = 0 Then
                lstError.Items.Clear()
                ChangeTaskOrder1(mdlMain.EnumTaskOrder.UpdateTask, ViewState("PropOldTaskOrder"), txttaskorder.Text.Trim)

                mstGetFunctionValue = WSSUpdate.UpdateTemplateTask(ViewState("SAddressNumber_Template"), ViewState("PropTaskNumber"), arrColumns, arrRows)
                ViewState("PropOldTaskOrder") = txttaskorder.Text.Trim

                If mstGetFunctionValue.ErrorCode = 0 Then
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    If GetFiles() = True Then
                        shFlag = 0
                    Else
                    End If

                    garTFileID.Clear()
                    If shFlag = 1 Then
                        ' cpnlError.Visible = True
                        lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                        'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        shFlag = 0
                        Exit Function
                    Else
                        ' cpnlError.Visible = True
                        lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                        'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    End If

                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    ' cpnlError.Visible = True
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                End If

            End If
        Catch ex As Exception
            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("Template Task_Edit", "Updateorsave-353", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        Return True
    End Function
#End Region

#Region "Fill Data"
    Private Sub FillData()
        Dim drTask As SqlDataReader
        Dim blnStatus As Boolean
        Try
            drTask = SQL.Search("TemplateTask_Edit", "FillData-391", "Select UM_VC50_UserID TaskOwner,* from T050031, T060011 where TTM_VC8_Supp_Owner=UM_IN4_Address_No_FK and TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Task_no_PK= " & ViewState("PropTaskNumber").ToString, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                drTask.Read()
                txtEstimatedHrs.Text = IIf(IsDBNull(drTask.Item("TTM_FL8_Est_Hr")), "", drTask.Item("TTM_FL8_Est_Hr"))
                CDDLStatus.CDDLSetSelectedItem(IIf(IsDBNull(drTask.Item("TTM_VC50_Deve_status")), "", drTask.Item("TTM_VC50_Deve_status")))
                txtSubject.Text = IIf(IsDBNull(drTask.Item("TTM_VC1000_Subtsk_Desc")), "", drTask.Item("TTM_VC1000_Subtsk_Desc"))
                CDDLTaskType.CDDLSetSelectedItem(IIf(IsDBNull(drTask.Item("TTM_VC8_task_type")), "", drTask.Item("TTM_VC8_task_type")))
                ' txtProject.Text = IIf(IsDBNull(drTask.Item("TTM_VC8_Project")), "", drTask.Item("TTM_VC8_Project"))
                CDDLTaskOwner.CDDLSetSelectedItem(IIf(IsDBNull(drTask.Item("TTM_VC8_Supp_Owner")), "", drTask.Item("TTM_VC8_Supp_Owner")), False, drTask.Item("TaskOwner"))
                'CDDLPriority.CDDLSetSelectedItem(IIf(IsDBNull(drTask.Item("TTM_VC8_Priority")), "", drTask.Item("TTM_VC8_Priority")))

                If IsDBNull(drTask.Item("TTM_NU9_Dependency")) = True Then
                    DDLDependancy.SelectedValue = ""
                Else
                    If DDLDependancy.Items.Contains(New ListItem(drTask.Item("TTM_NU9_Dependency"), drTask.Item("TTM_NU9_Dependency"))) = True Then
                        DDLDependancy.SelectedValue = drTask.Item("TTM_NU9_Dependency")
                    Else
                        DDLDependancy.Items.Add(New ListItem(drTask.Item("TTM_NU9_Dependency"), drTask.Item("TTM_NU9_Dependency")))
                        DDLDependancy.SelectedValue = drTask.Item("TTM_NU9_Dependency")
                    End If

                End If
                If IsDBNull(drTask.Item("TTM_NU9_Agmt_No")) = True Then
                    CDDLAgreement.CDDLSetBlank()
                Else
                    CDDLAgreement.CDDLSetSelectedItem(drTask.Item("TTM_NU9_Agmt_No"))
                End If

                If IsDBNull(drTask.Item("TTM_CH1_Mandatory")) Then
                    chkMandatory.Checked = False
                Else
                    If drTask.Item("TTM_CH1_Mandatory") = "M" Then
                        chkMandatory.Checked = True
                    Else
                        chkMandatory.Checked = False
                    End If
                End If
                txttaskid.Text = IIf(IsDBNull(drTask.Item("TTM_NU9_Task_no_PK")), drTask.Item("TTM_NU9_Task_no_PK"), drTask.Item("TTM_NU9_Task_no_PK"))
                txttaskorder.Text = IIf(IsDBNull(drTask.Item("TTM_NU9_Task_Order")), "", drTask.Item("TTM_NU9_Task_Order")) ' task order field
                ViewState("PropOldTaskOrder") = Val(txttaskorder.Text.Trim) 'stores the old task order
            End If
        Catch ex As Exception
            CreateLog("TemplateTaskEdit", "FillData-503", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
#End Region

    Private Function GetFiles() As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T050041"
            SQL.DBTracing = False

            Dim blnRead As Boolean
            ' For Tasks

            sqrdTempFiles = SQL.Search("TemplateTask_Edit", "GetFiles-428", "select * from T050041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

            If blnRead = True Then
                While sqrdTempFiles.Read
                    mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                    mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                    mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                    CreateFolder(Request.QueryString("CallNo"), ViewState("PropTaskNumber"))
                End While
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("Template Task_Edit", "GetFiles-415", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../TemplateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                ' SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("TemplateTask_Edit", "CreateFolder-507", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

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

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
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
            CreateLog("Template Task_Edit", "CreateFolder-530", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return False
        End Try
    End Function

    Public Function ChangeTaskOrder1(ByVal enuChange As EnumTaskOrder, ByVal OldTaskOrder As Integer, Optional ByVal NewTaskOrder As Integer = 0) As Boolean
        Try
            Dim strSQL As String
            Dim objCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
            Dim objADP As SqlClient.SqlDataAdapter
            Dim dsTaskOrder As New DataSet
            Select Case enuChange
                Case EnumTaskOrder.DeleteTask
                    strSQL = "select * from T050031  where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & Val(HttpContext.Current.Session("PropCallNumber")) & " and TTM_NU9_Task_Order>" & OldTaskOrder & " order by TTM_NU9_Task_Order asc"
                    objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                    objADP.Fill(dsTaskOrder, "T050031")
                    Dim intC As Integer = OldTaskOrder
                    For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                        dsTaskOrder.Tables(0).Rows(intI).Item("TTM_NU9_Task_Order") = intC
                        intC += 1
                    Next

                Case EnumTaskOrder.UpdateTask
                    If NewTaskOrder > OldTaskOrder Then
                        strSQL = "select * from T050031 where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & Request.QueryString("CallNo") & " and TTM_NU9_Task_Order>" & OldTaskOrder & " and TTM_NU9_Task_Order<=" & NewTaskOrder & " order by TTM_NU9_Task_Order asc"
                        objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                        objADP.Fill(dsTaskOrder, "T050031")
                        Dim intC As Integer = OldTaskOrder
                        For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                            dsTaskOrder.Tables(0).Rows(intI).Item("TTM_NU9_Task_Order") = intC 'dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") - 1
                            intC += 1
                        Next
                    ElseIf NewTaskOrder < OldTaskOrder Then
                        strSQL = "select * from T050031 where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & Request.QueryString("CallNo") & " and TTM_NU9_Task_Order>=" & NewTaskOrder & " and TTM_NU9_Task_Order<" & OldTaskOrder & " order by TTM_NU9_Task_Order asc"
                        objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                        objADP.Fill(dsTaskOrder, "T050031")
                        Dim intC As Integer = NewTaskOrder + 1
                        For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                            dsTaskOrder.Tables(0).Rows(intI).Item("TTM_NU9_Task_Order") = intC 'dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") + 1
                            intC += 1
                        Next
                    End If
            End Select

            Dim DatasetChanges = dsTaskOrder.GetChanges
            If Not IsNothing(DatasetChanges) Then
                Dim objCMDBldr As New SqlClient.SqlCommandBuilder(objADP)
                objADP.Update(dsTaskOrder, "T050031")
                objCMDBldr.Dispose()
                objADP.Dispose()
            End If
            objCon.Dispose()
        Catch ex As Exception
            CreateLog("TemplateTask_Edit", "ChangeTaskOrder-2342", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
End Class
