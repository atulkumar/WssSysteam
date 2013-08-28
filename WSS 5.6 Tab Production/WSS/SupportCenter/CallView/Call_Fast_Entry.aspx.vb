'Session("CallOpened")--Unused Session Variable,Not used in whole wss pages
'Session("PropActionNumber")--Unused Session Variable
Imports System.IO
Imports System.Data
Imports ION.Data
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Telerik.Web.UI
Imports WSSBLL

Partial Class SupportCenter_CallView_Call_Fast_Entry
    Inherits System.Web.UI.Page
    Private txthiddenImage As String 'Stored clicked button's cation  
    Private artxtbox As New ArrayList
    Private rowvalue As Integer
    Private mintFileID As Integer
    Private mstrFileName As String
    Private mstrFilePath As String
    Private strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Security Block
        '****************************************
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(833) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, 833)
        End If
        'End of Security Block
        '*****************************************
        If Request.QueryString("ScreenFrom") = "HomePage" Then
            'imgClose.Visible = False
        End If
        'Put user code to initialize the page here
        If Not IsPostBack Then
            txtCSS(Me.Page)
            ViewState("CallNo") = 0
        End If
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        txtDesc.Attributes.Add("onmousemove", "ShowToolTip(this,2000);")
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('OK');")
        imgAttachment.Attributes.Add("onclick", "return OpenAttach(" & Session("PropCompanyID") & ")")
        imgComment.Attributes.Add("onclick", "return KeyImage(" & Val(ViewState("CallNo")) & ",'" & Session("PropCompany") & "', 'C','0')")

        ViewState("CompanyID") = Session("PropCompanyID")
        txtCompany.Text = Session("PropCompany")
        If GetComment() = True Then
            imgComment.ImageUrl = "..\..\images\comment_unread.gif"
        Else
            imgComment.ImageUrl = "..\..\images\comment_Blank.gif"
        End If

        '************************************************************
        If IsPostBack = False Then
            FillRadCombos()
        End If

        If IsNothing(Session("Attach")) = False Then
            If Session("Attach") = 1 Then
                imgAttachment.Src = "../../Images/Attach_Yellow.gif"
            Else
                imgAttachment.Src = "../../Images/Attach15_9.gif"
            End If
        End If
        'If Not IsPostBack Then
        '    ViewState("CallNo") = 0

        '    'FillCustomDDl()
        '    FillNonUDCDropDown(DDLProject, "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & Session("PropCompanyId"), True)
        'End If

        txthiddenImage = Request.Form("txthiddenImage")

        'check value from hidden text box based on toolbar click
        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Edit"
                    Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=4", False)
                Case "Save"
                    If SaveCall() = True Then
                        lstError.Items.Clear()

                        lstError.Items.Add("Call Saved Successfully and Your Call Number is " & ViewState("CallNo"))
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                        ClearTxtBoxs()
                        rowvalue = 0
                        ViewState("CallNo") = 0
                    End If
                    Exit Sub
                Case "OK"
                    If SaveCall() = True Then
                        lstError.Items.Clear()

                        lstError.Items.Add("Call Saved Successfully and Your Call Number is " & ViewState("CallNo"))
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                        ClearTxtBoxs()
                        rowvalue = 0
                        ViewState("CallNo") = 0
                        ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:tabClose();</script>", False)
                    End If
                    Exit Sub
                Case "Logout"
                    LogoutWSS()
                    Exit Sub
            End Select
        End If

    End Sub

    Private Function SaveCall() As Boolean

        If ValidationCallData() = False Then
            Exit Function
        End If

        Dim arColumnName As New ArrayList
        Dim arDataRow As New ArrayList

        Try
            arColumnName.Add("CM_NU9_Comp_Id_FK")
            arColumnName.Add("CM_VC8_Call_Type")
            arColumnName.Add("CM_DT8_Request_Date")
            arColumnName.Add("CM_NU9_Call_Owner")
            arColumnName.Add("CM_VC100_By_Whom")
            arColumnName.Add("CM_VC200_Work_Priority")
            arColumnName.Add("CM_VC100_Subject")
            arColumnName.Add("CM_VC2000_Call_Desc")
            arColumnName.Add("CM_NU9_Project_ID")
            arColumnName.Add("CM_NU9_CustID_FK")
            arColumnName.Add("CN_VC20_Call_Status")
            arColumnName.Add("CM_NU9_Agreement")
            arColumnName.Add("CM_NU9_Category_Code_1")
            arColumnName.Add("CM_NU9_Category_Code_2")
            arColumnName.Add("CM_NU9_Category_Code_3")
            arColumnName.Add("CM_NU9_Coordinator") 'added new field 
            arColumnName.Add("CM_BT1_Fast_Entry_Flag")

            arDataRow.Add(Session("PropCompanyID"))

            If CDDLCallType.Text = "" Then
            Else
                arDataRow.Add(CDDLCallType.Text.Trim.ToUpper)
            End If
            arDataRow.Add(Now)
            arDataRow.Add(CDDLCallOwner.SelectedValue)
            arDataRow.Add(Val(Session("PropUserID")))
            If CDDLPriority.Text = "" Then
                arDataRow.Add("NRM".ToUpper)
            Else
                arDataRow.Add(CDDLPriority.SelectedValue.Trim.ToUpper)
            End If

            arDataRow.Add(txtSubject.Text.Trim)
            arDataRow.Add(txtDesc.Text.Trim)
            arDataRow.Add(CDDLProject.SelectedValue)
            arDataRow.Add(Session("PropCompanyID"))
            arDataRow.Add("Default".ToUpper)
            arDataRow.Add(0) 'Agreement
            arDataRow.Add(0) 'Category1 
            arDataRow.Add(0) 'Category2
            arDataRow.Add(0) 'Category3
            arDataRow.Add(DBNull.Value) 'added new field for coordinator
            arDataRow.Add(1)
            Dim stReturnValue As New ReturnValue
            stReturnValue = WSSSave.SaveCall(arColumnName, arDataRow, Val(Session("PropCompanyID")), "Default".ToUpper)

            ViewState("CallNo") = stReturnValue.ExtraValue
            If GetFiles(mdlMain.AttachLevel.CallLevel) = True Then
                'shReturn = 1
            Else
                ' shReturn = 2
            End If

            If GetComment() = True Then

            End If
            imgComment.ImageUrl = "..\..\images\comment_Blank.gif"
            imgAttachment.Src = "../../Images/Attach15_9.gif"
            Return True

        Catch ex As Exception
            CreateLog("Call_Fast_Entry", "SaveCall-163", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            Return False
        End Try

    End Function

    Private Function CheckRealValuesForDDLS() As Boolean
 
        Try

            Dim bytError As Byte = 0
            Dim dsRequestedBy As New DataSet
            Dim dsComboCallType As New DataSet

            If CDDLCallType.Text = "" Then
                lstError.Items.Add("Call Type cannot be blank...")
                bytError += 1
            End If

            If CDDLPriority.Text = "" Then
                lstError.Items.Add("Call Priority cannot be blank...")
                bytError += 1
            End If

            If CDDLProject.Text = "" Then
                lstError.Items.Add("Sub Category cannot be blank...")
                bytError += 1
            End If

            If CDDLCallOwner.Text = "" Then
                lstError.Items.Add("Requested By cannot be blank...")
                bytError += 1
            End If
            If bytError > 0 Then
                Return False
            Else
                Return True
            End If

            If SQL.Search("T040103", "CallDetails", "Page_Load-222", "select ct_VC8_calltype_fk from T040103 where ct_VC8_calltype_fk='" & CDDLCallType.Text & "'", dsComboCallType, "", "") = True Then
                If dsComboCallType.Tables.Count > 0 Then
                    If dsComboCallType.Tables(0).Rows.Count = 0 Then
                        lstError.Items.Add("The call-type you have entered is not Valid.. Please select it from Dropdown")
                        bytError += 1
                    End If
                End If
            Else
                lstError.Items.Add("The call-type you have entered is not Valid.. Please select it from Dropdown")
                bytError += 1
            End If

            If SQL.Search("T040103", "CallDetails", "Page_Load-222", "SELECT um_in4_address_no_fk as ID,(UName.ci_vc36_name + '[' + um_vc50_userid + ']') as Name,t010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where UM_VC4_Status_Code_FK='ENB' and t010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and (um_in4_company_ab_id=" & ViewState("CompanyID") & ") and UM_IN4_Company_AB_ID=" & ViewState("CompanyID") & " and (UName.ci_vc36_name + '[' + um_vc50_userid + ']')='" & CDDLCallOwner.Text & "'", dsRequestedBy, "", "") = True Then
                If dsRequestedBy.Tables.Count > 0 Then
                    If dsRequestedBy.Tables(0).Rows.Count = 0 Then
                        lstError.Items.Add("The user you have selected in Requested by is not Valid.. Please select a valid user from Dropdown")
                        bytError += 1
                    End If
                End If
            Else
                lstError.Items.Add("The user you have selected in Requested by is not Valid.. Please select a valid user from Dropdown")
                bytError += 1
            End If


            If SQL.Search("T040103", "CallDetails", "Page_Load-222", "Select Name as ID from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='PRIO' and UDC.Company=" & Session("PropCompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='PRIO' and UDC.Company=0 Order By Name", dsComboCallType, "", "") = True Then
                If dsComboCallType.Tables.Count > 0 Then
                    If dsComboCallType.Tables(0).Rows.Count = 0 Then
                        lstError.Items.Add("The call-type you have entered is not Valid.. Please select it from Dropdown")
                        bytError += 1
                    End If
                End If
            Else
                lstError.Items.Add("The call-type you have entered is not Valid.. Please select it from Dropdown")
                bytError += 1
            End If

            'Check for Priority
            Dim strCheckDropDownValue As String = SQL.Search("Call_View", "CheckRealValuesForDDLS-3157", "Select Name from UDC where name='" & CDDLPriority.Text.Trim & "'")
            If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                lstError.Items.Add("The priority you have entered is not Valid.Please select it from Dropdown")
                bytError += 1
            Else
                bytError += 0
            End If

            If bytError > 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("Call_Fast_Entry", "CheckRealValuesForDDLS-197", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Function

    Function ValidationCallData() As Boolean
        Dim shFlag As Short = 0
        lstError.Items.Clear()

        If txtDesc.Text.Trim.Length > 2000 Then
            lstError.Items.Add("Description text cannot be more than 2000 characters...")
            shFlag = 1
        End If

        If txtSubject.Text.Trim.Equals("") Then
            lstError.Items.Add("Subject cannot be blank...")
            shFlag = 1
        Else
        End If

        If txtDesc.Text.Trim.Equals("") Then
            lstError.Items.Add("Description cannot be blank...")
            shFlag = 1
        End If

        
        If CheckRealValuesForDDLS() = False Then
            shFlag = 1
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            'ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function

    Private Function GetMaxCallNo() As Integer
        Try
            Dim maxcallno As Integer
            SQL.DBConnection = strSQL
            maxcallno = SQL.Search("t040091", "", "Select  max(FE_IN4_ID_PK) from T040091 where FE_IN9_CompID=" & Session("PropCompanyID") & "")
            Return maxcallno
        Catch ex As Exception
            CreateLog("Call_Fast_Entry", "GetMaxCallNo-260", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Function

    Private Sub ClearTxtBoxs()
        txtSubject.Text = ""
        txtDesc.Text = ""
        CDDLCallOwner.Text = String.Empty
        CDDLCallType.ClearSelection()
        CDDLCallType.Text = String.Empty
        CDDLCallOwner.ClearSelection()
        CDDLPriority.Text = String.Empty
        CDDLPriority.ClearSelection()
        CDDLProject.Text = String.Empty
        CDDLProject.ClearSelection()
        Session("Attach") = Nothing
    End Sub

    Private Sub FillRadCombos()
        Dim dsComboCallType As New DataSet
        Dim dsComboRequestedBy As New DataSet
        Dim dsComboPriority As New DataSet
        Dim dsComboProject As New DataSet
        Dim objCommonFunctionsBLL As New clsCommonFunctionsBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)

        dsComboCallType = objCommonFunctionsBLL.fillRadCallTypeDDL(ViewState("CompanyID"))
        CDDLCallType.Items.Clear()
        CDDLCallType.DataSource = dsComboCallType
        For Each data As DataRow In dsComboCallType.Tables(0).Rows

            Dim item As RadComboBoxItem = New RadComboBoxItem()
            item.Text = CStr(data("ID"))
            item.Value = CStr(data("ID"))
            CDDLCallType.Items.Add(item)
            item.DataBind()
        Next

        dsComboRequestedBy = objCommonFunctionsBLL.fillRadRequestedByDDL(ViewState("CompanyID"))
        CDDLCallOwner.Items.Clear()
        CDDLCallOwner.DataSource = dsComboRequestedBy

        For Each data As DataRow In dsComboRequestedBy.Tables(0).Rows
            Dim item As RadComboBoxItem = New RadComboBoxItem()
            item.Text = CStr(data("Name"))
            item.Value = CStr(data("ID"))
            CDDLCallOwner.Items.Add(item)
            item.DataBind()
        Next
        'End If

        dsComboPriority = objCommonFunctionsBLL.FillRadCallPriority(ViewState("CompanyID"))
        CDDLPriority.Items.Clear()
        CDDLPriority.DataSource = dsComboPriority

        For Each data As DataRow In dsComboPriority.Tables(0).Rows
            Dim item As RadComboBoxItem = New RadComboBoxItem()
            item.Text = CStr(data("ID"))
            item.Value = CStr(data("ID"))
            CDDLPriority.Items.Add(item)
            item.DataBind()
        Next
        'End If

        dsComboProject = objCommonFunctionsBLL.FillRadProject(ViewState("CompanyID"))
        CDDLProject.Items.Clear()
        CDDLProject.DataSource = dsComboProject

        For Each data As DataRow In dsComboProject.Tables(0).Rows
            Dim item As RadComboBoxItem = New RadComboBoxItem()
            item.Text = CStr(data("Name"))
            item.Value = CStr(data("ID"))
            CDDLProject.Items.Add(item)
            item.DataBind()
        Next
    End Sub

    'Private Sub FillCustomDDl(Optional ByVal CustomerChanged As Boolean = False)
    '    Try
    '        '------------------------------------------
    '        FillRadCombos()
    '        '--Priority
    '        CDDLPriority.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='PRIO' and UDC.Company=" & Session("PropCompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='PRIO' and UDC.Company=0 Order By Name"
    '        CDDLPriority.CDDLMandatoryField = True
    '        CDDLPriority.CDDLUDC = True
    '        '------------------------------------------

    '        If IsPostBack = False Or CustomerChanged = True Then
    '            CDDLPriority.CDDLFillDropDown(10, True)
    '        End If
    '    Catch ex As Exception
    '        CreateLog("Call_Fast_Entry", "FillCustomDDL-4243", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
    '    End Try
    'End Sub

    Private Function GetFiles(ByVal Level As AttachLevel) As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            '  SQL.DBTable = "T040041"
            SQL.DBTracing = False

            Dim blnRead As Boolean

            Select Case Level
                ' For Calls
                Case AttachLevel.CallLevel

                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1464", "select * from T040041 Where AT_IN4_Level=1 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(Session("PropCompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            If CreateFolder(ViewState("CallNo")) = True Then
                                shAttachments = 1
                                'Return True
                            Else
                                shAttachments = 2
                                'Return False
                            End If
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        SQL.Update("Call_Detail", "GetFiles-1600", "Update T040011 set CM_NU8_Attach_No=1 where CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & Session("PropCompanyID") & "   ", SQL.Transaction.Serializable)
                        Return True
                    Else
                        Return False
                    End If

            End Select
            sqrdTempFiles.Close()
        Catch ex As Exception
            CreateLog("Call_Fast_Entry", "GetFiles-360", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & Session("PropCompanyID") & "\" & CallNo)
        'Dim objFile As File
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & Session("PropCompanyID") & "\" & CallNo & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & Session("PropCompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    Dim strChanges As String = strPathDB.Trim & "/" & Session("PropCompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1577", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & Session("PropCompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & Session("PropCompanyID") & "\" & CallNo & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-1596", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & Session("PropCompanyID"))

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & Session("PropCompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                Dim strChanges As String = strPathDB.Trim & "/" & Session("PropCompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1624", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & Session("PropCompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1637", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & Session("PropCompanyID") & "", SQL.Transaction.Serializable)
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
            CreateLog("Call_Fast_Entry", "CreateFolder-477", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function GetComment() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim IntRows As Integer
            ' For Calls comment
            SQL.Search("Call_Detail", "GetComment-721", "select * from T040061 Where CM_VC2_Flag='C' and CM_NU9_Call_Number=0 and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0   and CM_NU9_CompId_Fk=" & Val(Session("PropCompanyID")) & " and CM_NU9_AB_Number=" & Session("PropUserID") & "", IntRows)
            If IntRows > 0 Then
                If SQL.Update("Call_Detail", "GetComment-724", "update T040061 set CM_CH1_Flag=1, CM_NU9_Call_Number=" & ViewState("CallNo") & " WHERE CM_NU9_AB_Number=" & Session("PropUserID") & " and CM_NU9_CompId_Fk=" & Session("PropCompanyID") & " and CM_VC2_Flag='C' and CM_NU9_Call_Number=0 and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0 ", SQL.Transaction.Serializable) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Call_Fast_Entry", "GetComment-499", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
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
