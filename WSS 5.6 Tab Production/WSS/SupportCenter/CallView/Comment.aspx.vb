Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports ION.Logging
Imports Microsoft.Web.UI.WebControls
Imports ION.data
Imports System.Data
Imports WSSBLL
Imports Telerik.Web.UI
Imports ION.Net
Imports System.IO
Imports System.Web.Security
Imports System.Drawing

Partial Class SupportCenter_CallView_Comment
    Inherits System.Web.UI.Page
    Private Shared mstrOption As String
    Private objCommonFunctionsBLL As New clsCommonFunctionsBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        Session.Remove("UserId")
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        'Stylesheet function for Textboxes
        If Not IsPostBack Then
            Call txtCSS(Me.Page)
        End If
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'For clearing cache from browser history
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        '------------------------------------------
        lstError.Items.Clear()

        'Client side attributes of server controls
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgEmailList.Attributes.Add("Onclick", "return SaveEdit('EmailList');")
        txtComment.Attributes.Add("onmousemove", "ShowToolTip(this,1000);")
        txtSendMail.Attributes.Add("ReadOnly", "ReadOnly")
        '--------------------------------------------------------------------------------------
        'Comment Window is Opened from Home Page
        '------------------------------------------------
        If Not IsPostBack Then
            If Request.QueryString("From") = "Home" Then
                mstrOption = Request.QueryString("Level")
                ViewState("CompanyID") = Request.QueryString("CID")
                ViewState("CallNo") = Request.QueryString("CN")
                ViewState("TaskNo") = Request.QueryString("TN")
                ViewState("ActionNo") = Request.QueryString("AN")
            End If
        End If
        '------------------------------------------------
        'Get the action number from query string

        'check for task number
        If Request.QueryString("tbname") = "TASK" Then
            ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("comp")).ExtraValue
            ViewState("CallNo") = Request.QueryString("CallNo")
            ViewState("TaskNo") = Val(Request.QueryString("ID"))
            ViewState("ActionNo") = 0
        ElseIf Request.QueryString("tbname") = "T" Then
            ViewState("TaskNo") = Val(Request.QueryString("TaskNo"))
            ViewState("ActionNo") = 0
            ViewState("CallNo") = Val(Request.QueryString("CallNo"))
            If IsNumeric(Request.QueryString("CompID")) = True Then
                ViewState("CompanyID") = Request.QueryString("CompID")
            Else
                ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
            End If
        ElseIf Request.QueryString("tbname") = "C" Then
            '#############
            ViewState("TaskNo") = 0
            ViewState("ActionNo") = 0
            If Request.QueryString("CompId") <> "" Then
                ViewState("CallNo") = Val(Request.QueryString("CallNo"))
                'ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompId")).ExtraValue
                If IsNumeric(Request.QueryString("CompID")) = True Then
                    ViewState("CompanyID") = Request.QueryString("CompID")
                Else
                    ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
                End If
            End If
        ElseIf Request.QueryString("tbname") = "A" Then
            If IsNumeric(Request.QueryString("CompID")) = True Then
                ViewState("CompanyID") = Request.QueryString("CompID")
            Else
                ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
            End If
            ViewState("CallNo") = Val(Request.QueryString("CallNo"))
            ViewState("TaskNo") = Val(Request.QueryString("TaskNo"))
            ViewState("ActionNo") = Val(Request.QueryString("ActionNo"))
        ElseIf Request.QueryString("tbname") = "AO" Then
            ViewState("CompanyID") = Request.QueryString("CompID")
            ViewState("CallNo") = Val(Request.QueryString("CallNo"))
            ViewState("TaskNo") = Val(Request.QueryString("TaskNo"))
            ViewState("ActionNo") = Val(Request.QueryString("ActionNo"))
        End If
        If Val(Request.QueryString("OPT")) = 2 Then
            ViewState("ProjectID") = Val(Session("PropProjectID"))
        Else
            ViewState("ProjectID") = WSSSearch.SearchProjectID(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))
        End If

        If Not IsNothing(ViewState("CompanyID")) And Not IsNothing(ViewState("CallNo")) Then
            ' If Val(ViewState("CallNo")) = 0 Then
            If Not IsPostBack Then
                fillRadCombo(Val(ViewState("CompanyID")), Val(ViewState("CallNo")), Val(ViewState("TaskNo")))
            End If

            'CDDLCommentTo.CDDLQuery = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name, T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and UC_BT1_Access=1)  Order By Name"
            'Else
            ' CDDLCommentTo.CDDLQuery = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name, T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and UC_BT1_Access=1)  Order By Name"
            ' End If

            ' CDDLCommentTo.CDDLUDC = False
            ' CDDLCommentTo.CDDLFillDropDown(10, False)
        End If
        Dim txthiddenvalue = Request.Form("txthidden")
        If Page.IsPostBack = False Then
            If Not IsNothing(Request.QueryString("tbname")) Then
                mstrOption = Request.QueryString("tbname")
            End If
            'bind the comment grid
            BindCommentGrid()
            'change the flag in the database after binding grid
            If Val(Request.QueryString("OPT")) = 2 Then
                'change the flag in template comment table
                Call ChangeFlag(2)
            Else
                'chnage the comment flag
                Call ChangeFlag()
            End If
        End If

        If txthiddenvalue <> "" Then
            Try
                Select Case txthiddenvalue

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            '                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        SaveComments()

                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            '                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If SaveComments() = True Then
                            'SaveComments()
                            If Request.QueryString("Page_name") = "Call_Heirarchy" Then
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "myScript", "<script>javascript:Close();</script>", False)
                            Else
                                Response.Write("<script>self.opener.Form1.submit();</script>")
                                Response.Write("<script>window.close();</script>")
                            End If
                        End If

                End Select
            Catch ex As Exception
                CreateLog("Comment", "Load-98", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If

        'Show call no in textbox
        txtCallNo.Text = Val(ViewState("CallNo"))
        'show task no in textbox
        txtTaskNo.Text = Val(ViewState("TaskNo"))
        'show action no in textbox
        txtActionNo.Text = Val(ViewState("ActionNo"))


        If Not IsPostBack Then
            Dim dsGetMailId As New DataSet
            dsGetMailId = objCommonFunctionsBLL.FilltxtMailIdDefault(Val(ViewState("CallNo")), Val(ViewState("TaskNo")))
            Dim MailUserID As String = String.Empty
            txtSendMail.Text = ""
            For iEmailId As Integer = 0 To dsGetMailId.Tables(0).Rows.Count - 1
                'txtSendMail.Text += dsGetMailId.Tables(0).Rows(iEmailId)("CI_VC28_Email_1").ToString() & ";"
                MailUserID += dsGetMailId.Tables(0).Rows(iEmailId)("TM_VC8_Supp_Owner").ToString() & ";"
            Next
            If Not String.IsNullOrEmpty(txtSendMail.Text) Then
                'txtSendMail.Text = txtSendMail.Text.Substring(0, txtSendMail.Text.Length - 1)
            End If
            Session("CommentPage_EmailId") = MailUserID
        End If
        




        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 329
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
        'End of Security Block
        'disable the external/internal ddl for client company
        'because client company cannot write internal comment
        If HttpContext.Current.Session("PropCompanyType") = "CCMP" Then
            ddlntExt.Enabled = False
        Else
            ddlntExt.Enabled = True
        End If
        '----------------------------
    End Sub
    Private Sub fillRadCombo(ByVal CompanyID As Integer, ByVal CallNo As Integer, ByVal TaskNo As Integer)
        Dim dsComboCommentType As New DataSet
        'Dim dsGetMailId As New DataSet
        Try
            dsComboCommentType = objCommonFunctionsBLL.fillRadCommentType(CompanyID, CallNo)
            CDDLCommentTo.Items.Clear()
            CDDLCommentTo.DataSource = dsComboCommentType
            For Each data As DataRow In dsComboCommentType.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("Name"))
                item.Value = CStr(data("ID"))
                CDDLCommentTo.Items.Add(item)
                item.DataBind()
            Next
            'CDDLCommentTo.SelectedValue = objCommonFunctionsBLL.FillRadCallOwnerDefault(CallNo)
            'dsGetMailId = objCommonFunctionsBLL.FilltxtMailIdDefault(CallNo, TaskNo)
            'Dim MailUserID As String = String.Empty
            'txtSendMail.Text = ""
            'For iEmailId As Integer = 0 To dsGetMailId.Tables(0).Rows.Count - 1
            '    txtSendMail.Text += dsGetMailId.Tables(0).Rows(iEmailId)("CI_VC28_Email_1").ToString() & ";"
            '    MailUserID += dsGetMailId.Tables(0).Rows(iEmailId)("TM_VC8_Supp_Owner").ToString() & ";"
            'Next
            'Session("CommentPage_EmailId") = MailUserID


        Catch ex As Exception
            CreateLog("Comment", "FillRadCombo-203", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Function CheckRealValuesForRadDDLS() As Boolean
        Try

            Dim strCommentTo As String = String.Empty
            Dim ds As New DataSet
            'commented for Artic wss
            'strCommentTo = SQL.Search("", "", "select (isnull(T2.CI_VC36_Name,'')+'['+isnull(T010043.PI_VC8_Department,'')+']'+'['+isnull(t2.CI_VC36_ID_1,'')+']') as Name from T060011, T010011 T1, T010011 T2,T010043 where T010043.PI_NU8_Address_No=T060011.um_in4_address_no_fk  and T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and UC_BT1_Access=1) and replace((isnull(T2.CI_VC36_Name,'')+'['+isnull(T010043.PI_VC8_Department,'')+']'+'['+isnull(t2.CI_VC36_ID_1,'')+']'),char(160),char(32))=replace('" & CDDLCommentTo.Text & "',char(160),char(32))", "")
            'earliercommented
            'strCommentTo = SQL.Search("", "", "select T2.CI_VC36_Name as Name from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and UC_BT1_Access=1) and replace(T2.ci_vc36_name,char(160),char(32))=replace('" & CDDLCommentTo.Text & "',char(160),char(32))", "")

            'Uncommented for Artic wss
            ' strCommentTo = SQL.Search("", "", "select T2.ci_vc36_name + '[' + um_vc50_userid + '][' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and UC_BT1_Access=1) and T1.ci_nu8_address_number = replace('" & CDDLCommentTo.Text & "','  ','  ')  Order By Name", "")

            strCommentTo = SQL.Search("", "", "select T2.ci_vc36_name + ' [' + um_vc50_userid + '] [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and UC_BT1_Access=1) and T2.ci_vc36_name + ' [' + um_vc50_userid + '] [' + T1.ci_vc36_name + ']'='" & CDDLCommentTo.Text & "'  Order By Name", "")
            If String.IsNullOrEmpty(strCommentTo) = True Then
                Return False
            End If
            Return True
        Catch ex As Exception
        End Try
    End Function
    Private Function SaveComments() As Boolean
        If txtComment.Text.Trim.Equals("") Then
            lstError.Items.Clear()
            lstError.Items.Add("Comment cannot be blank...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
            Exit Function
        End If
        If CheckRealValuesForRadDDLS() = False And CDDLCommentTo.Text <> "" Then
            lstError.Items.Clear()
            lstError.Items.Add("The Comment To you have entered is not Valid.. Please select a valid value from Dropdown")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
            Exit Function
        End If
        Try
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

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

            arRowData.Add(IIf(CDDLCommentTo.SelectedValue = "", System.DBNull.Value, CDDLCommentTo.SelectedValue))

            arRowData.Add("1")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(Now)
            arRowData.Add(txtComment.Text.Trim)
            If Request.QueryString("tbname") = "TASK" Then
                arRowData.Add("T")
            Else
                If Not IsNothing(Request.QueryString("tbname")) Then
                    arRowData.Add(Request.QueryString("tbname")) ' "A")
                Else
                    arRowData.Add(Request.QueryString("Level")) ' "A")
                End If
            End If
            arRowData.Add(ViewState("CallNo"))
            arRowData.Add(ViewState("TaskNo"))
            arRowData.Add(ViewState("ActionNo"))
            arRowData.Add("F")
            arRowData.Add(ViewState("CompanyID"))

            arRowData.Add(ddlntExt.SelectedValue.Trim)
            If CDDLCommentTo.SelectedValue <> "" Then
                Dim strCommentToMail As String = SQL.Search("", "", "select CI_VC28_Email_1 from T010011 where CI_NU8_Address_Number=" & Val(CDDLCommentTo.SelectedValue))
                If IsNothing(strCommentToMail) = False Then
                    If txtSendMail.Text.Trim.ToUpper.IndexOf(strCommentToMail.ToUpper) = -1 Then
                        arRowData.Add(txtSendMail.Text.Trim & IIf(txtSendMail.Text.Trim = "", "", ";") & strCommentToMail)
                    Else
                        arRowData.Add(txtSendMail.Text.Trim)
                    End If
                Else
                    arRowData.Add(txtSendMail.Text.Trim)
                End If
            Else
                arRowData.Add(txtSendMail.Text.Trim)
            End If
            arRowData.Add("F")

            If Val(Request.QueryString("OPT")) = 2 Then
                arRowData.Add(HttpContext.Current.Session("SAddressNumber"))
                mstGetFunctionValue = WSSSave.SaveComments(arColumnName, arRowData, 2)
            Else
                mstGetFunctionValue = WSSSave.SaveComments(arColumnName, arRowData)
            End If
            If mstGetFunctionValue.ErrorCode = 0 Then
                BindCommentGrid()
                '                cpnlError.Visible = True
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                txtComment.Text = ""
                txtSendMail.Text = ""
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
    'This function will be used for changing the flag of the comment
    'Author:-Harpreet Singh
    'Create Date:-12/12/2006
    'Modified Date:- ----
    Private Function ChangeFlag(Optional ByVal OPT As Integer = 0)

        Dim strSql As String
        Dim blnStatus As Boolean

        Try
            If OPT = 2 Then ' for templates
                Select Case mstrOption
                    Case "T"
                End Select
            Else
                Select Case mstrOption
                    Case "C"
                        strSql = "update T040061 set CM_CH1_Flag='0'  where CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0 and (CM_NU9_Comment_To=" & Session("PropUserID") & " or  CM_NU9_Comment_To is null)"
                    Case "T"
                        strSql = "update T040061 set CM_CH1_Flag='0'  where CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=0 and (CM_NU9_Comment_To=" & Session("PropUserID") & " or  CM_NU9_Comment_To is null)"
                    Case "TASK"
                        strSql = "update T040061 set CM_CH1_Flag='0'  where CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=0 and (CM_NU9_Comment_To=" & Session("PropUserID") & " or  CM_NU9_Comment_To is null)"
                    Case "A"
                        strSql = "update T040061 set CM_CH1_Flag='0'  where CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=" & ViewState("ActionNo") & "  and (CM_NU9_Comment_To=" & Session("PropUserID") & " or  CM_NU9_Comment_To is null)"
                End Select
            End If
            Dim strCompType As String = HttpContext.Current.Session("PropCompanyType")
            If UCase(strCompType) <> "SCM" Then
                strSql = strSql & " and CM_VC50_IE='External'"
            End If
            blnStatus = SQL.Update("Comment", "UpdateFlag-343", strSql, SQL.Transaction.ReadCommitted)
        Catch ex As Exception
            CreateLog("Comment", "UpdateFlag-344", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

    End Function
    'This function will be used for filling the comment grid
    'Author:-Harpreet Singh
    'Create Date:-12/12/2006
    'Modified Date:- ----
    Private Sub BindCommentGrid()
        Dim strSql As String
        '   Dim blnStatus As Boolean
        ' Dim strTable As String
        Dim intOPT As Integer = Val(Request.QueryString("OPT"))
        Try
            If intOPT = 2 Then ' for templates
                Select Case mstrOption
                    Case "T"
                        strSql = "select B.UM_VC50_UserID CommentTo, CM_NU9_Comment_Number_PK,A.UM_VC50_UserID WrittenBy, case CM_CH1_Flag when 1 then 'FALSE' else 'TRUE' end CommentRead, convert(varchar,CM_DT8_Date,100) CommentDate, replace( CM_VC1000_MailList,';','; ') MailSent, CM_VC256_Comments Comment, CM_VC50_IE InternalExternal from T050061,T060011 A, T060011 B where B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CM_NU9_AB_Number=A.UM_IN4_Address_No_FK and    CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=0"
                End Select
            Else
                Select Case mstrOption.Trim.ToUpper
                    Case "C"
                        'To check where we are comming from call fast entry temprary comment when call is null
                        'added login user id condition in query
                        If Val(ViewState("CallNo")) = 0 Then
                            strSql = "select B.UM_VC50_UserID CommentTo, CM_DT8_Date CommentDate1, CM_NU9_Comment_Number_PK,A.UM_VC50_UserID WrittenBy, case CM_CH1_Flag when 1 then 'FALSE' else 'TRUE' end CommentRead, convert(varchar,CM_DT8_Date,100) CommentDate, replace( CM_VC1000_MailList,';','; ') MailSent, CM_VC256_Comments Comment, CM_VC50_IE InternalExternal from T040061,T060011 A, T060011 B where B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CM_NU9_AB_Number=A.UM_IN4_Address_No_FK and  CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0 and CM_NU9_AB_Number=" & Val(Session("PropUserID"))
                        Else
                            strSql = "select B.UM_VC50_UserID CommentTo, CM_DT8_Date CommentDate1, CM_NU9_Comment_Number_PK,A.UM_VC50_UserID WrittenBy, case CM_CH1_Flag when 1 then 'FALSE' else 'TRUE' end CommentRead, convert(varchar,CM_DT8_Date,100) CommentDate, replace( CM_VC1000_MailList,';','; ') MailSent, CM_VC256_Comments Comment, CM_VC50_IE InternalExternal from T040061,T060011 A, T060011 B where B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CM_NU9_AB_Number=A.UM_IN4_Address_No_FK and  CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0"
                        End If
                    Case "T"
                        strSql = "select B.UM_VC50_UserID CommentTo, CM_DT8_Date CommentDate1, CM_NU9_Comment_Number_PK,A.UM_VC50_UserID WrittenBy, case CM_CH1_Flag when 1 then 'FALSE' else 'TRUE' end CommentRead, convert(varchar,CM_DT8_Date,100) CommentDate, replace( CM_VC1000_MailList,';','; ') MailSent, CM_VC256_Comments Comment, CM_VC50_IE InternalExternal from T040061,T060011 A, T060011 B where B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CM_NU9_AB_Number=A.UM_IN4_Address_No_FK and   CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=0"
                    Case "TASK"
                        strSql = "select B.UM_VC50_UserID CommentTo, CM_DT8_Date CommentDate1, CM_NU9_Comment_Number_PK,A.UM_VC50_UserID WrittenBy, case CM_CH1_Flag when 1 then 'FALSE' else 'TRUE' end CommentRead, convert(varchar,CM_DT8_Date,100) CommentDate, replace( CM_VC1000_MailList,';','; ') MailSent, CM_VC256_Comments Comment, CM_VC50_IE InternalExternal from T040061,T060011 A, T060011 B where B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CM_NU9_AB_Number=A.UM_IN4_Address_No_FK and  CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=0"
                    Case "A", "AO"
                        strSql = "select B.UM_VC50_UserID CommentTo, CM_DT8_Date CommentDate1, CM_NU9_Comment_Number_PK,A.UM_VC50_UserID WrittenBy, case CM_CH1_Flag when 1 then 'FALSE' else 'TRUE' end CommentRead, convert(varchar,CM_DT8_Date,100) CommentDate, replace( CM_VC1000_MailList,';','; ') MailSent, CM_VC256_Comments Comment, CM_VC50_IE InternalExternal from T040061,T060011 A, T060011 B where B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CM_NU9_AB_Number=A.UM_IN4_Address_No_FK and  CM_NU9_CompId_Fk=" & ViewState("CompanyID") & " and CM_NU9_Call_Number=" & ViewState("CallNo") & " and CM_NU9_Task_Number=" & ViewState("TaskNo") & " and CM_NU9_Action_Number=" & ViewState("ActionNo")
                End Select
            End If
            Dim strCompType As String = HttpContext.Current.Session("PropCompanyType")
            If UCase(strCompType) <> "SCM" Then
                strSql = strSql & " and CM_VC50_IE='External'"
            End If
            strSql &= " order by CM_NU9_Comment_Number_PK desc"
            Dim dsComment As New DataSet
            If SQL.Search("Comment", "", "", strSql, dsComment, "", "") = True Then

                Dim htDateCols As New Hashtable
                htDateCols.Add("CommentDate", 1)
                'htDateCols.Add("CommentDate", 1)
                SetDataTableDateFormat(dsComment.Tables(0), htDateCols)

                Dim htDescCols As New Hashtable
                htDescCols.Add("Comment", 40)
                HTMLEncodeDecode(mdlMain.Action.Encode, dsComment.Tables(0), htDescCols)
                grdComment.DataSource = dsComment.Tables(0).DefaultView
                grdComment.DataBind()
                tblComment.Width = "2000px"
            Else
                tblComment.Width = "0px"
            End If

        Catch ex As Exception
            CreateLog("Comment", "UpdateFlag-344", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub grdComment_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdComment.ItemDataBound
        Dim strToolTip As String
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If CType(e.Item.FindControl("chkRead"), CheckBox).Checked = True Then
                e.Item.Cells(1).ToolTip = "Read"
                strToolTip = "This is an old Comment..."
            Else
                e.Item.Cells(1).ToolTip = "Unread"
                strToolTip = "This is a new Comment..."
            End If
            If e.Item.Cells(6).Text.Trim.ToUpper.Equals("&nbsp;".ToUpper) Then
                strToolTip &= vbCrLf & "No Mail sent with this comment..."
            Else
                strToolTip &= vbCrLf & "Mail sent with this comment..."
            End If
            For intI As Integer = 2 To 6
                e.Item.Cells(intI).ToolTip = strToolTip
            Next
            e.Item.Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex & ")")
        End If
    End Sub
End Class
