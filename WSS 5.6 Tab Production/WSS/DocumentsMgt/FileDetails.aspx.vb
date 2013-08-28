'************************************************************************************************************
' Page                   :- File Details
' Purpose                :- Purpose of this screen is to Create and Edit details of file/document.
' Tables used            :- T250011 
' Date					Author						Modification Date					Description
' 11/02/08			    Ranvijay Sahay	                -------------------				    Created
' Session Variable required: SelectedCompanyName,  FolderID
' QueryStringRequired: ButtonClick(ADD/EDIT)
'************************************************************************************************************
Imports ION.Data
Imports ION.Logging
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Data

'Session("FileID")
'Session("SelectedCompanyName")
'Session("FolderPath")

Partial Class DocumentsMgt_FileDetails
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' -- if user is coming on the screen to add new folder. After this check will be on the folder id session
        '-- remove HTML tags
        If Page.IsPostBack = False Then
            ViewState("ButtonClick") = Request.QueryString("ButtonClick")
            Session("FileID") = Request.QueryString("FileID")
        End If
        If Viewstate("ButtonClick") = "ADD" Then 'If user wants to edit folder properties
            '-- set parentfolderid to folder id and folderid to 0
            'Session("ParentFolderID") = Session("FolderID")
            Session("FileID") = 0
            FileUpload.Disabled = False '-- enable foldername if in add mode
        Else
            FileUpload.Disabled = True
        End If
        txtCompany.Text = Regex.Replace(HttpContext.Current.Session("SelectedCompanyName"), "<[^>]*>", "")
        txtParentFolder.Text = WSSSearch.GetFolderName(HttpContext.Current.Session("FolderID")).ExtraValue
        txtFolderPath.Text = Regex.Replace(Session("FolderPath"), "<[^>]*>", "") & txtParentFolder.Text & "/"
        '-- fill data when in edit mode
        If Val(Session("FileID")) > 0 And IsPostBack = False Then 'If user wants to edit folder properties
            FillData()
        End If
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        'imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgOK.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        txtDescription.Attributes.Add("onmousemove", "ShowToolTip(this,5000);")
        txtCSS(Me.Page)

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
                        If Viewstate("ButtonClick") = "EDIT" Then 'If user wants to edit folder properties
                            If UpdateFile() = True Then
                                '   Response.Write("<script>self.opener.Location='FolderMgtMaster.aspx'</script>")
                            End If
                        Else
                            If SaveFile(FileUpload.PostedFile.FileName) = True Then
                                '   Response.Write("<script>self.opener.Location='FolderMgtMaster.aspx'</script>")
                            End If
                        End If
                    Case "Close"
                        ' Response.Write("<script>self.opener.location='FolderMgtMaster.aspx';</script>")
                        Response.Write("<script>self.opener.Form1.submit();</script>")
                        Response.Write("<script>window.close();</script>")
                    Case "Ok"
                        'Security Block
                        Dim intID As Int32
                        If Not IsPostBack Then
                            intID = 994
                            Dim obj As New clsSecurityCache
                            If obj.ScreenAccess(intID) = False Then
                                Response.Redirect("../frm_NoAccess.aspx")
                            End If
                            obj.ControlSecurity(Me.Page, intID)
                        End If
                        'End of Security Block
                        If Viewstate("ButtonClick") = "EDIT" Then 'If user wants to edit folder properties
                            If UpdateFile() = True Then
                                'Response.Write("<script>self.opener.location='FolderMgtMaster.aspx';</script>")
                                Response.Write("<script>self.opener.Form1.submit();</script>")
                                Response.Write("<script>window.close();</script>")
                            End If
                        Else
                            If SaveFile(FileUpload.PostedFile.FileName) = True Then
                                'Response.Write("<script>self.opener.location='FolderMgtMaster.aspx';</script>")
                                Response.Write("<script>self.opener.Form1.submit();</script>")
                                Response.Write("<script>window.close();</script>")
                            End If
                        End If
                    Case "Reset"
                End Select
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Error occured  please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("FileDetails", "Page_Load-147", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
    End Sub
    Private Sub FillData()
        Dim dsFolder As DataSet

        Try
            FileUpload.Disabled = True
            If Val(Session("FolderID")) <> 0 Then
                dsFolder = WSSSearch.GetFileDetail(Val(Session("FileID"))).ExtraValue()
            End If
            If Session("PermissionDownLoad") = 1 Then
                hplFileLink.Text = dsFolder.Tables(0).Rows(0).Item("FI_VC255_File_Name")
            Else
                hplFileLink.Text = ""
            End If
            hplFileLink.Target = Session.SessionID

            hplFileLink.NavigateUrl = "../" & dsFolder.Tables(0).Rows(0).Item("FI_VC255_File_URL")
            txtFileSize.Text = Math.Round(Val(dsFolder.Tables(0).Rows(0).Item("FI_VC255_File_Size")), 2) & " KB"
            txtDescription.Text = IIf(IsDBNull(dsFolder.Tables(0).Rows(0).Item("FI_VC500_File_Description")), "", dsFolder.Tables(0).Rows(0).Item("FI_VC500_File_Description"))
            txtversion.Text = dsFolder.Tables(0).Rows(0).Item("FI_FL8_File_Version")

        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Error occured  please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("FileDetails", "FillData-183", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Function UpdateFile() As Boolean
        If ValidateFields() = False Then
            Return False
            Exit Function
        End If
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            '---------------------
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("FI_VC500_File_Description")
            arColumnName.Add("FI_NU9_ModifyBy_ID_FK")
            arColumnName.Add("FI_VC32_Modify_From_IP")
            arColumnName.Add("FI_DT8_Modify_ON")

            arRowData.Add(txtDescription.Text.Trim)
            arRowData.Add(Session("PropUserID"))
            arRowData.Add(GetIP)
            arRowData.Add(Now)
            '----------------------
            mstGetFunctionValue = WSSUpdate.UpdateFile(arColumnName, arRowData, "T250011", "Select * From T250011 Where FI_NU9_File_ID_PK=" & HttpContext.Current.Session("FileID"))
            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Folder updated successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            End If
            If mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Clear()
                lstError.Items.Add("Error occur please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            CreateLog("FileDetails", "UpdateFile-226", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Function SaveFile(ByVal FileName As String) As Boolean
        Dim FolderPath As String ' -- Value will be assigned from the function CreateFolder
        Dim FolderUrl As String ' -- Value will be assigned from the function CreateFolder
        Dim Version As Double ' -- Value will be assigned from the function CreateFolder

        If ValidateFields() = False Then
            Return False
            Exit Function
        End If
        Try
            ' -- remove company from the folder path because I'll adding companyid instead
            FolderPath = txtFolderPath.Text.Substring(txtFolderPath.Text.Trim.IndexOfAny("/") + 1)
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            If CreateFolder(WSSSearch.SearchCompName(txtCompany.Text.Trim).ExtraValue, FolderPath, FolderUrl, FileName, Version) = True Then

                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList

                arColumnName.Add("FI_NU9_Company_ID_FK")
                arColumnName.Add("FI_NU9_Folder_ID_FK")
                arColumnName.Add("FI_VC255_File_Name")
                arColumnName.Add("FI_VC500_File_Description")
                arColumnName.Add("FI_VC255_File_Size")
                arColumnName.Add("FI_VC255_File_Path")
                arColumnName.Add("FI_VC255_File_URL")
                arColumnName.Add("FI_FL8_File_Version")
                arColumnName.Add("FI_VC16_Status")
                arColumnName.Add("FI_NU9_UploadBy_ID_FK")
                arColumnName.Add("FI_VC32_Upload_From_IP")
                arColumnName.Add("FI_DT8_Upload_ON")

                arRowData.Add(WSSSearch.SearchCompName(txtCompany.Text.Trim).ExtraValue)
                arRowData.Add(HttpContext.Current.Session("FolderID"))
                arRowData.Add(FileName)
                arRowData.Add(txtDescription.Text.Trim)
                arRowData.Add(FileUpload.PostedFile.ContentLength / 1024)
                arRowData.Add(FolderPath)
                arRowData.Add(FolderUrl) 'url
                arRowData.Add(Version) 'url
                arRowData.Add("ENB")
                arRowData.Add(Session("PropUserID"))
                arRowData.Add(GetIP)
                arRowData.Add(Now)

                mstGetFunctionValue = WSSSave.SaveFile(arColumnName, arRowData, FileName, Session("FolderID"))
            Else
                mstGetFunctionValue.ErrorCode = 1 ' if file is not saved successfully
            End If

            If mstGetFunctionValue.ErrorCode = 0 Then
                '-- after saving record open in Edit Mode
                Session("FileID") = mstGetFunctionValue.ExtraValue
                Viewstate("ButtonClick") = "EDIT"
                FillData()

                lstError.Items.Clear()
                lstError.Items.Add("File Created successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            End If
            If mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Clear()
                lstError.Items.Add("Error occur please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            CreateLog("FileDetails", "SaveFile-300", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Function ValidateFields() As Boolean
        Try
            If ViewState("ButtonClick") = "ADD" Then
                If (FileUpload.PostedFile.ContentLength / 1024) / 1024 > 7 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("You cannot upload file of size more than 7MB...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return False
                End If

                If FileUpload.Value.Trim = "" Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Upload Document can not be blank...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            CreateLog("ValidateFields", "ValidateFields-250", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End Try
    End Function
    Private Function CreateFolder(ByVal CompanyID As Integer, ByRef FolderPath As String, ByRef FolderUrl As String, ByRef FileName As String, ByRef Version As Double) As Boolean
        Dim strPath As String = Server.MapPath("../Documents")
        Dim strPathDB As String = ("Documents")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & CompanyID & "\" & FolderPath)
        Dim objFile As File
        Dim strFilePath As String
        Dim dblVersionNo As Double
        Dim strFileLocation As String
        FileName = ExtractFileName(FileName)
        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & CompanyID & "\" & FolderPath
                If File.Exists(strFilePath & FileName) Then
                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If
                    strFileLocation = strFilePath & dblVersionNo & "\" & FileName.Trim
                    strFileLocation = strFileLocation.Replace("/", "\")
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & CompanyID & "/" & FolderPath & dblVersionNo & "/" & FileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    FolderPath = strFilePath & dblVersionNo & "\"
                    FolderUrl = strChanges
                    Version = dblVersionNo
                End If
            Else
                strFilePath = strPath & "\" & CompanyID & "\" & FolderPath.Replace("//", "")
                strFilePath = strFilePath.Replace("/", "\")
                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-343", "select max(FI_FL8_File_Version) from T250011 where FI_NU9_Folder_ID_FK=" & Session("FolderID") & " and FI_VC255_File_Name='" & FileName.Trim & "' and FI_NU9_Company_ID_FK=" & CompanyID)
                If dblVersionNo > 0 Then
                    ' Increment the version number
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If
                strFileLocation = strFilePath & dblVersionNo & "\" & FileName.Trim
                ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & CompanyID & "/" & FolderPath & dblVersionNo & "/" & FileName.Trim

                ' Create the sub-directory with the version name
                objFolder.CreateSubdirectory(dblVersionNo)
                FolderPath = strFilePath & dblVersionNo & "\"
                FolderUrl = strChanges
                Version = dblVersionNo
            End If
            ' create the file in set folder
            FileUpload.PostedFile.SaveAs(strFileLocation)
            'objFile.Create(strFileLocation)
            objFile = Nothing
            objFolder = Nothing
            Return True
        Catch ex As Exception
            CreateLog("FileDetails", "CreateFolder-386", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Function ExtractFileName(ByVal FileFullName As String) As String
        Return FileFullName.Substring(FileFullName.LastIndexOfAny("\") + 1)
    End Function
End Class
