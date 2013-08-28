'***********************************************************************************************************
' Page                 :- Action edit
' Purpose              :- Purpose of this screen is to edit the action filled earlier.User can edit action                             description, used hours, action owner etc.  
' Tables used          :- UDC, T010011, T060011, T040031, T040011, T040021, T040041, T040051 
' Date					  Author						Modification Date				Description
' 17/03/06	  	          SAchin/Amit          			                                    Created
'
' Notes: 
' Code:
'************************************************************************************************************
'----------------------------------------------------------
'ScrID=294
'----------------------------------------------------------
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.IO
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Partial Class SupportCenter_CallView_Action_edit
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
  
    Protected WithEvents imgAttachment As System.Web.UI.HtmlControls.HtmlImage
    
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Form Level Variables"
  
    Private mintFileID As Integer
    Private mstrFileName As String
    Private mstrFilePath As String

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
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


        CDDLActionOwner.Enabled = False
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        txtUsedHR.Attributes.Add("onkeypress", "UsedHour('txtUsedHR');")

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        If Page.IsPostBack = False Then
            txtdescription.Attributes.Add("onmousemove", "ShowToolTip(this,2000);")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        End If
        
        CDDLActionType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='ACT' and UDC.Company=" & Session("PropCompanyID") & "  union Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='ACT' and UDC.Company=0 Order By Name"

        CDDLActionType.CDDLMandatoryField = False
        CDDLActionOwner.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and CI_IN4_Business_Relation=""SCM"" Order By Name"
        CDDLActionOwner.CDDLMandatoryField = True
        CDDLActionOwner.CDDLUDC = False
        '------------------------------------------

        If IsPostBack = False Then
            Call txtCSS(Me.Page)
            CDDLActionType.CDDLFillDropDown(10)
            CDDLActionOwner.CDDLFillDropDown(10, False)
        Else
            CDDLActionType.CDDLSetItem()
            CDDLActionOwner.CDDLSetItem()
        End If

        Dim strhiddenImage As String
        Dim intImageOption As Integer

        ViewState("ActionNo") = Request.QueryString("ACTIONNO")

        If Page.IsPostBack = False Then
            ViewState("CallNo") = Request.QueryString("CallNo")
            ViewState("TaskNo") = Request.QueryString("TaskNo")
            ViewState("ActionNo") = Request.QueryString("ACTIONNO")
            'ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID").Trim).ExtraValue
            If IsNumeric(Request.QueryString("CompID")) = True Then
                ViewState("CompanyID") = Request.QueryString("CompID")
            Else
                ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
            End If
        End If

        imgComment.Attributes.Add("onclick", "return OpenComm(" & ViewState("TaskNo") & ", " & ViewState("ActionNo") & "," & ViewState("CompanyID") & "," & ViewState("CallNo") & ");")
        imgAttach.Attributes.Add("onclick", "return OpenAtt(" & ViewState("CompanyID") & "," & ViewState("CallNo") & ", " & ViewState("TaskNo") & "," & ViewState("ActionNo") & ");")
        SetCommentFlag(intImageOption, mdlMain.CommentLevel.ActionLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))

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
                            '                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        Call UpdateorSave()
                    Case "Close"
                        'Response.Write("<script>self.opener.callrefresh();</script>")
                        'Response.Write("<script>window.close();</script>")
                    Case "Reset"

                End Select
            Catch ex As Exception
                CreateLog("Action_Edit", "Load-136", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
       
        If Page.IsPostBack = False Then
            Call FillData()
        End If
        If Val(CDDLActionOwner.CDDLGetValue) > 0 Then
            If Val(CDDLActionOwner.CDDLGetValue) <> Val(Session("PropUserID")) Then
                DisableControls()
                txtUsedHR.Visible = False
                lblName6.Visible = False
            End If
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
        txtdescription.Text = ""
        txtUsedHR.Text = ""
    End Sub
#End Region

#Region "Fill Values to Save in an arrayList"

    Private Function UpdateorSave() As Boolean

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short

        lstError.Items.Clear()
        Try
            'check call status
            '*********************************************************
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T040011"
            SQL.DBTracing = False

            Dim strchkcallstatus As String = SQL.Search("Action_Edit", "UpdateorSave-247", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")

            If IsNothing(strchkcallstatus) = False Then
                lstError.Items.Add("Call Closed so You cannot change the Task and Action...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
                Exit Function
            End If
            '*******************************************************
            'Check Invoice Status
            '*********************************************************
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T040031"
            SQL.DBTracing = False

            Dim strInvoiceStatus As String = SQL.Search("Action_Edit", "UpdateorSave-269", "select isnull(AM_CH1_IsInvoiced,'N') from T040031 where  AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and AM_NU9_Task_Number=" & ViewState("TaskNo") & " And AM_NU9_Action_Number=" & ViewState("ActionNo"))
            If IsNothing(strInvoiceStatus) = False Then
                If strInvoiceStatus.Equals("Y") Then
                    lstError.Items.Add("Action is already invoiced. Modification is not allowed...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                    Exit Function
                End If
            End If

            '*********************************************************
            If CDDLActionOwner.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Action Owner cannot be blank...")
                shFlag = 1
            ElseIf txtdescription.Text.Trim.Equals("") Then
                lstError.Items.Add("Action Description cannot be blank...")
                shFlag = 1
            End If

            Dim dtTaskDate As Date = SQL.Search("Action_Edit", "UpdateorSave-271", "Select tm_DT8_Task_Date from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"))

            If dtEstFinishDate.Text.Trim <> "" Then
                If IsDate(dtEstFinishDate.Text) = False Then
                    lstError.Items.Add("Check Action date format...")
                    shFlag = 1
                    'ElseIf CDate(dtEstFinishDate.Text.Trim).ToShortDateString < CDate(dtTaskDate.ToShortDateString) Then
                    '    lstError.Items.Add("Action date cannot be less than Task Date...")
                    '    shFlag = 1
                ElseIf CDate(dtEstFinishDate.Text.Trim) < dtTaskDate.ToString("yyyy-MMM-dd") Then
                    lstError.Items.Add("Action date cannot be less than Task Date...")
                    shFlag = 1
                    'ElseIf CDate(dtEstFinishDate.Text.Trim).ToShortDateString > Now.ToShortDateString Then
                    '    lstError.Items.Add("Action date cannot be more than current date...")
                    '    shFlag = 1
                ElseIf CDate(dtEstFinishDate.Text.Trim) > Now.ToString() Then
                    lstError.Items.Add("Action date cannot be more than current date...")
                    shFlag = 1

                End If
            End If
            If ChkMandatoryHr.Checked = True And Val(txtUsedHR.Text) = 0 Then
                lstError.Items.Add("Used Hours cannot be zero...")

                shFlag = 1
            End If

            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                shFlag = 0
                Return False
                Exit Function
            End If

            lstError.Items.Clear()

            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                shFlag = 0
                Return False
                Exit Function
            End If


            arrColumns.Add("AM_VC100_Action_type")
            arrColumns.Add("AM_VC8_ActionType")
            arrColumns.Add("AM_VC_2000_Description")
            arrColumns.Add("AM_FL8_Used_Hr")
            arrColumns.Add("AM_VC8_Supp_Owner")
            arrColumns.Add("AM_CH1_Mandatory")
            arrColumns.Add("AM_DT8_Action_Date_Auto")

            If Not dtEstFinishDate.Text.Trim.Equals("") Then
                arrColumns.Add("AM_DT8_Action_Date")
            End If

            arrRows.Add(CDDLActionType.CDDLGetValue.Trim)
            '  arrRows.Add(ddlntExt.SelectedValue)  'Option for internal/external
            If ChkActType.Checked = True Then
                arrRows.Add("External")
            Else
                arrRows.Add("Internal")
            End If

            arrRows.Add(txtdescription.Text.Trim)
            arrRows.Add(Val(txtUsedHR.Text.Trim))
            'arrRows.Add(txtProject.Text.Trim)
            arrRows.Add(CDDLActionOwner.CDDLGetValue.Trim)
            If ChkMandatoryHr.Checked = True Then
                arrRows.Add("M")
            Else
                arrRows.Add("O")
            End If
            arrRows.Add(CDate(Now))
            If Not dtEstFinishDate.Text.Trim.Equals("") Then
                arrRows.Add(CDate(CDate(dtEstFinishDate.Text.Trim).ToShortDateString & " " & Now.ToLongTimeString))
            End If

            If shFlag = 0 Then
                lstError.Items.Clear()
                mstGetFunctionValue = WSSUpdate.UpdateAction(ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"), ViewState("CompanyID"), arrColumns, arrRows)

                If mstGetFunctionValue.ErrorCode = 0 Then
                    Call FillData()
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    If GetFiles() = True Then
                        shFlag = 0
                    Else
                        shFlag = 1
                    End If
                    'Next

                    garFileID.Clear()
                    'garABNo.Clear()

                    If shFlag = 1 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Records saved successfully...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        shFlag = 0
                        Return True
                    Else
                        '                        cpnlError.Visible = True
                        lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                        Return True
                    End If

                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                End If

            End If
        Catch ex As Exception
            lstError.Items.Add(ex.Message)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("Action_Edit", "UpdateorSave-379", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
#End Region

#Region "Fill Data"
    Private Sub FillData()
        Dim drTask As SqlDataReader
        Dim blnStatus As Boolean
        Try
            drTask = SQL.Search("Action_Edit", "FillData-421", "Select *,CI_VC36_Name from T040031,T010011 where CI_NU8_Address_Number=AM_VC8_Supp_Owner and  AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and AM_NU9_Task_Number= " & Val(ViewState("TaskNo")) & " And  AM_NU9_Action_Number=" & Val(ViewState("ActionNo")) & " and AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")), SQL.CommandBehaviour.CloseConnection, blnStatus)

            If blnStatus = True Then
                drTask.Read()
                Dim acttype As String
                acttype = IIf(IsDBNull(drTask.Item("AM_VC8_ActionType")), "", drTask.Item("AM_VC8_ActionType"))
                If acttype = "External" Then
                    ChkActType.Checked = True
                Else
                    ChkActType.Checked = False
                End If
                CDDLActionType.CDDLSetSelectedItem(IIf(IsDBNull(drTask.Item("AM_VC100_Action_type")), "", drTask.Item("AM_VC100_Action_type")))
                txtdescription.Text = IIf(IsDBNull(drTask.Item("AM_VC_2000_Description")), "", drTask.Item("AM_VC_2000_Description"))
                txtUsedHR.Text = IIf(IsDBNull(drTask.Item("AM_FL8_Used_Hr")), "", drTask.Item("AM_FL8_Used_Hr"))
                CDDLActionOwner.CDDLSetSelectedItem(IIf(IsDBNull(drTask.Item("AM_VC8_Supp_Owner")), "", drTask.Item("AM_VC8_Supp_Owner")), False, drTask.Item("CI_VC36_Name"))
                If IsDBNull(drTask.Item("AM_CH1_Mandatory")) Then
                    ChkMandatoryHr.Checked = False
                Else
                    If drTask.Item("AM_CH1_Mandatory") = "M" Then
                        ChkMandatoryHr.Checked = True
                    Else
                        ChkMandatoryHr.Checked = False
                    End If
                End If
                If IsDBNull(drTask.Item("AM_DT8_Action_Date")) = False Then
                    dtEstFinishDate.Text = SetDateFormat(CDate(drTask.Item("AM_DT8_Action_Date")), mdlMain.IsTime.DateOnly)
                End If
            End If
        Catch ex As Exception
            lstError.Items.Add("Unexpected Error..")
            CreateLog("Action-Edit", "FillData-461", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "")
        End Try
    End Sub
#End Region

    Private Function GetFiles() As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim blnRead As Boolean
            sqrdTempFiles = SQL.Search("Action_Edit", "GetFiles-463", "select * from T040041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

            If blnRead = True Then
                While sqrdTempFiles.Read
                    mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                    mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                    mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                    CreateFolder(Val(ViewState("CallNo")), Val(ViewState("TaskNo")), Val(ViewState("ActionNo")))
                End While
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("Action_Edit", "GetFiles-447", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                '	SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Action_Edit", "CreateFolder-542", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

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

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
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
            CreateLog("Action_Edit", "CreateFolder-562", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Sub DisableControls()
        Try
            CDDLActionType.Enabled = False
            CDDLActionOwner.Enabled = False
            ChkMandatoryHr.Enabled = False
            ChkActType.Enabled = False
            txtdescription.Enabled = False
            'dtEstFinishDate.readOnlyDate = False
            'imgAttachment.Attributes.Remove("onclick")
        Catch ex As Exception
            CreateLog("Action_Edit", "DisableControls-660", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
End Class
