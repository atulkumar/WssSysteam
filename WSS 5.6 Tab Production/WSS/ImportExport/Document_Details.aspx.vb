Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Data

Partial Class ImportExport_Document_Details
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            txtCSS(Me.Page)
            cpnlErrorPanel.Visible = False
            imgDelete.Attributes.Add("onclick", "return SaveEdit('Delete')")
            Dim strhiddenImage As String
            strhiddenImage = Request.Form("txthiddenImage")
            If strhiddenImage <> "" Then

                Select Case strhiddenImage
                    Case "Delete"
                        If DeleteRule(Val(Request.Form("txtRuleID"))) = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Rule deleted successfully...")
                            'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            Call ShowRuleGrid(Val(HttpContext.Current.Session("FileNo")))
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Rule not deleted, Please try again...")
                            'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select

            End If

            If Not IsPostBack Then
                cpnlFilePermissions.State = CustomControls.Web.PanelState.Collapsed
                Button1.Attributes.Add("onclick", "return CheckReqdFields();")
                txtDescription1.Attributes.Add("onkeypress", "return CheckLength();")
                imgSave.Attributes.Add("onclick", "return CheckFile();")
                FillNonUDCDropDown(DDLCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")")
                DDLCompany.SelectedValue = Session("PropCompanyID")

                FillNonUDCDropDown(DDLFileComp, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")")
                DDLFileComp.SelectedValue = Session("PropCompanyID")

                FillNonUDCDropDown(ddlGroup, "Select Name as ID, Description from UDC  where  ProductCode=0   and UDCType='GRP' and (UDC.Company=" & DDLFileComp.SelectedValue & " OR udc.Company=0)", False)

                FillNonUDCDropDown(DDLFileGroup, "Select Name as ID, Description from UDC  where  ProductCode=0   and UDCType='GRP' and (UDC.Company=" & DDLCompany.SelectedValue & " OR udc.Company=0)", False)

                If HttpContext.Current.Session("FileNo") Is Nothing Then
                    'imgSave.Enabled = False
                    cpnlFileInfo.Enabled = False
                    cpnlFileInfo.State = CustomControls.Web.PanelState.Collapsed
                    cpnlFileInfo.TitleCSS = "Test2"
                    cpnlFilePermissions.Enabled = False
                    cpnlFilePermissions.State = CustomControls.Web.PanelState.Collapsed
                    cpnlFilePermissions.TitleCSS = "Test2"
                    cpnlAccessInfo.Enabled = False
                    cpnlAccessInfo.State = CustomControls.Web.PanelState.Collapsed
                    cpnlAccessInfo.TitleCSS = "Test2"
                Else
                    ShowRuleGrid(HttpContext.Current.Session("FileNo"))

                    hypFile.Text = "Download File" 'HttpContext.Current.Session("FileNo")
                    txtFID.Text = HttpContext.Current.Session("FileNo")
                    Dim dtFileData As DataTable = CType(GetFileData(HttpContext.Current.Session("FileNo")), DataTable)
                    hypFile.NavigateUrl = "../Dockyard/ImportExport" & "/" & dtFileData.Rows(0).Item("FileGroup") & "/" & dtFileData.Rows(0).Item("FileVersion") & "/" & dtFileData.Rows(0).Item("FileName")
                    txtCompanyName.Text = dtFileData.Rows(0).Item("CompName")
                    txtCompanyID.Text = dtFileData.Rows(0).Item("CompID")
                    txtDocumentName.Text = dtFileData.Rows(0).Item("FileName")
                    FillNonUDCDropDown(DDLFileGroup, "Select Name as ID, Description from UDC  where  ProductCode=0   and UDCType='GRP' and (UDC.Company=" & Val(txtCompanyID.Text) & " OR udc.Company=0)", False)
                    DDLFileGroup.SelectedValue = dtFileData.Rows(0).Item("FileGroup")
                    ViewState("Group") = dtFileData.Rows(0).Item("FileGroup")
                    txtDocDescription.Text = IIf(dtFileData.Rows(0).Item("FileDescription") Is DBNull.Value, "", dtFileData.Rows(0).Item("FileDescription"))
                    txtVersion.Text = dtFileData.Rows(0).Item("FileVersion")
                    txtFileSize.Text = dtFileData.Rows(0).Item("FileSize")
                    cpnlviewinfo.Enabled = False
                    cpnlviewinfo.State = CustomControls.Web.PanelState.Collapsed
                    cpnlviewinfo.TitleCSS = "Test2"
                    cpnlFileInfo.Text &= "( FileID : " & HttpContext.Current.Session("FileNo") & ")"
                End If
                Call DLLObjectType_SelectedIndexChanged(Me, New EventArgs)
            End If

            'Security Block
            Dim intID As Int32
            If Not IsPostBack Then
                intID = 954
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intID) = False Then
                    Response.Redirect("../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intID)
            End If
            'End of Security Block

        Catch ex As Exception
            CreateLog("PageLoad", "DocumentDetail", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserID"), Session("PropUserName"))

        End Try

    End Sub

    Private Function GetAllUsers() As Boolean
        Try
            Dim dsUsers As New DataSet
            If SQL.Search("T060011", "DocumentDetail", "GetUsers", "select UM_VC50_UserID UserName, UM_IN4_Address_No_FK UserID from T060011 where UM_IN4_Company_AB_ID=" & DDLCompany.SelectedItem.Value & "", dsUsers, "", "") = True Then
                cblObjects.DataSource = dsUsers.Tables(0)
                cblObjects.DataTextField = "UserName"
                cblObjects.DataValueField = "UserID"
                cblObjects.DataBind()
                cblObjects.RepeatDirection = RepeatDirection.Vertical
            Else
                cblObjects.Items.Clear()
            End If
        Catch ex As Exception
            CreateLog("DocumentDetail", "GetRoles", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Private Function GetRoles() As Boolean
        Try
            Dim dsRoles As New DataSet
            If SQL.Search("T060031", "DocumentDetail", "GetRoles", "select ROM_IN4_Role_ID_PK RoleID, ROM_VC50_Role_Name RoleName from T070031 where ROM_IN4_Company_ID_FK=" & DDLCompany.SelectedItem.Value & "", dsRoles, "", "") = True Then
                cblObjects.DataSource = dsRoles.Tables(0)
                cblObjects.DataTextField = "RoleName"
                cblObjects.DataValueField = "RoleID"
                cblObjects.DataBind()
            End If
        Catch ex As Exception
            CreateLog("DocumentDetail", "GetRoles", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function


    Function uploadFile() As Boolean

        If Not FileUpload.PostedFile Is Nothing And FileUpload.PostedFile.ContentLength > 0 Then
            Dim FileVersion As Double = 1.0
            Dim strFileName As String = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName)
            FileVersion = GetVersion(strFileName, ddlGroup.SelectedValue)
            Dim strPath As String = Server.MapPath("../Dockyard/ImportExport") & "/" & ddlGroup.SelectedValue & "/" & FileVersion
            Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\")
            If objFolder.Exists = False Then
                objFolder.Create()
            End If
            Try
                Dim SaveLocation As String
                Dim strRelPath As String = "..\Dockyard\ImportExport" & "\" & ddlGroup.SelectedValue & "\" & FileVersion & "\" & strFileName
                SaveLocation = Server.MapPath("..\Dockyard\ImportExport") & "\" & ddlGroup.SelectedValue & "\" & FileVersion & "\" & strFileName
                FileUpload.PostedFile.SaveAs(SaveLocation)
                txtFilepath.Text = "../Dockyard/ImportExport" & "/" & ddlGroup.SelectedValue & "/" & FileVersion & "/" & strFileName

                Dim FileId As Integer
                Dim DblFileSize As Double = FileUpload.PostedFile.ContentLength
                Dim strFileSize As String
                If System.Math.Round((DblFileSize / 1024) / 1024, 2) = 0 Then
                    If System.Math.Round(DblFileSize / 1024, 2) = 0 Then
                        strFileSize = System.Math.Round(DblFileSize, 2).ToString & "Bytes"
                    Else
                        strFileSize = System.Math.Round(DblFileSize / 1024, 2).ToString & "KB"
                    End If
                Else
                    strFileSize = System.Math.Round((DblFileSize / 1024) / 1024, 2).ToString & "MB"
                End If
                If SaveFile(strRelPath, DblFileSize, strFileSize, strFileName) = True Then

                    FileId = GetFileID()

                    If FileId <> 0 Then
                        hypFile.Text = "Download File" 'FileId
                        txtFID.Text = FileId
                        'txtFname.Text = fn
                        txtCompanyName.Text = DDLFileComp.SelectedItem.Text
                        txtCompanyID.Text = DDLFileComp.SelectedValue
                        hypFile.NavigateUrl = txtFilepath.Text
                        FillNonUDCDropDown(DDLFileGroup, "Select Name as ID, Description from UDC  where  ProductCode=0   and UDCType='GRP' and (UDC.Company=" & Val(txtCompanyID.Text) & " OR udc.Company=0)", False)
                        DDLFileGroup.SelectedValue = ddlGroup.SelectedValue
                        txtDocumentName.Text = strFileName
                        txtDocDescription.Text = txtDescription1.Text.Trim
                        txtFileSize.Text = strFileSize
                    End If
                    txtVersion.Text = GetVersion(strFileName, DDLFileGroup.SelectedValue)
                End If

                cpnlviewinfo.Enabled = False
                cpnlviewinfo.State = CustomControls.Web.PanelState.Collapsed
                cpnlviewinfo.TitleCSS = "Test2"
                cpnlFileInfo.Enabled = True
                cpnlFileInfo.State = CustomControls.Web.PanelState.Expanded
                cpnlFileInfo.TitleCSS = "Test"
                If HttpContext.Current.Session("FileNo") Is Nothing Then
                    cpnlFileInfo.Text &= "(FileID : " & FileId & ")"
                End If
                cpnlFilePermissions.Enabled = True
                cpnlFilePermissions.State = CustomControls.Web.PanelState.Expanded
                cpnlFilePermissions.TitleCSS = "Test"

            Catch ex As Exception
                CreateLog("AB_Main", "FileUploadFile-1971", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdskillset", )
            End Try
        Else

        End If
    End Function

    Public Function SaveFile(ByVal FilePath As String, ByVal FileSize As Double, ByVal strFileSize As String, ByVal FileName As String) As Boolean
        Dim strSQL As String
        Dim intRows As Integer
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strTable As String
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("FM_NU9_File_ID_PK")
            arColumnName.Add("FM_NU9_Company_ID_FK")
            arColumnName.Add("FM_VC256_FileName")
            arColumnName.Add("FM_VC512_File_Path")
            arColumnName.Add("FM_VC512_File_Description")
            arColumnName.Add("FM_VC8_File_Group")
            arColumnName.Add("FM_FL8_File_Size")
            arColumnName.Add("FM_VC256_File_Size")
            arColumnName.Add("FM_DT8_DateTime")
            arColumnName.Add("FM_NU9_Uploaded_By")
            arColumnName.Add("FM_VC256_File_Version")
            arColumnName.Add("FM_NU9_Inserted_By_FK")
            arColumnName.Add("FM_NU9_Inserted_Date")
            arColumnName.Add("FM_VC256_Inserted_By_IP")
            ''Column Data DDLCompany.SelectedItem.Value.Trim
            arRowData.Add(GetFileID() + 1)
            arRowData.Add(DDLFileComp.SelectedValue)
            arRowData.Add(FileName)
            arRowData.Add(FilePath)
            arRowData.Add(txtDescription1.Text.Trim)  'Description
            arRowData.Add(ddlGroup.SelectedValue)
            arRowData.Add(FileSize)
            arRowData.Add(strFileSize)
            arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(GetVersion(FileName, ddlGroup.SelectedValue))   ''Version will be maintained later 
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(Now)
            arRowData.Add(Request.UserHostName())
            ViewState("Group") = ddlGroup.SelectedValue
            If SQL.Save("T220011", "UploadFile", "SaveFile-183", arColumnName, arRowData) = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("UploadFile", "SaveTempFile-1900", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function
    Private Function GetVersion(ByVal Filename As String, ByVal FileGroup As String) As Double
        Try
            Dim strFileVersion As Double
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            strFileVersion = Val(SQL.Search("GetVersion", "GetActionType", "SELECT Max(FM_VC256_File_Version) AS Version FROM T220011 WHERE FM_VC256_FileName ='" & Filename & "' and FM_VC8_File_Group='" & FileGroup & "'"))
            If strFileVersion <> 0 Then
                strFileVersion += 0.1
            Else
                strFileVersion = 1.0
            End If
            Return strFileVersion
        Catch ex As Exception
            CreateLog("DocumentDetail", "GetActionType", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    Private Function GetFileID() As Integer
        Try
            Dim intFileID As Integer
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            intFileID = Val(SQL.Search("GetVersion", "GetActionType", "SELECT Max(FM_NU9_File_ID_PK) AS FileID FROM T220011"))
            Return intFileID
        Catch ex As Exception
            CreateLog("DocumentDetail", "GetFileID", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function

    Private Function GetFileData(ByVal FileID As Integer) As DataTable
        Try
            Dim dsFile As New DataSet
            SQL.Search("t220011", "DocumentDetail", "GetFileData", "select FM_VC256_File_Size FileSize, FM_NU9_Company_ID_Fk CompID, CI_VC36_Name CompName, FM_VC8_File_Group as FileGroup,FM_VC256_File_Version as FileVersion,FM_VC512_File_Path as FilePath,FM_VC256_FileName as Filename,FM_VC512_File_Description as FileDescription from t220011,T010011 where FM_NU9_Company_ID_Fk=CI_NU8_Address_Number and FM_NU9_File_ID_PK=" & FileID & "", dsFile, "", "")
            Return dsFile.Tables(0)
        Catch ex As Exception
            CreateLog("DocumentDetail", "GetFileData", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        uploadFile()
    End Sub

    Private Function SaveDocumentData() As Boolean
        Dim strSQL As String
        Dim intRows As Integer
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strTable As String
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList


            arColumnName.Add("FS_NU9_Company_ID_FK")
            arColumnName.Add("FS_NU9_File_ID_FK")
            arColumnName.Add("FS_VC8_Object_Type")  'Role or User
            arColumnName.Add("FS_NU9_Object_ID_FK") 'RoleID or UserId Multiple
            arColumnName.Add("FS_DT8_Valid_From")
            arColumnName.Add("FS_DT8_Valid_UpTo")
            arColumnName.Add("FS_VC4_Status")
            arColumnName.Add("FS_NU9_Inserted_By_FK")
            arColumnName.Add("FS_NU9_Inserted_Date")
            arColumnName.Add("FS_VC256_Inserted_By_IP")
            ''Column Data
            For intI = 0 To cblObjects.Items.Count - 1
                If cblObjects.Items(intI).Selected = True Then
                    If GetFileData(txtFID.Text.Trim, cblObjects.Items(intI).Value, DLLObjectType.SelectedItem.Value) = True Then
                        arRowData.Add(DDLCompany.SelectedItem.Value.Trim)
                        arRowData.Add(txtFID.Text.Trim)
                        arRowData.Add(DLLObjectType.SelectedItem.Value)
                        arRowData.Add(cblObjects.Items(intI).Value)

                        If dtValidFromDate.CalendarDate <> Nothing Then
                            arRowData.Add(dtValidFromDate.CalendarDate)   'Valid From
                        Else
                            arRowData.Add(Now.ToShortDateString)
                        End If
                        If dtValidToDate.CalendarDate <> Nothing Then
                            arRowData.Add(dtValidToDate.CalendarDate)   'Valid To
                        Else
                            arRowData.Add(Now.ToShortDateString)
                        End If

                        arRowData.Add(ddlStatus.SelectedItem.Value)
                        arRowData.Add(HttpContext.Current.Session("PropUserID"))
                        arRowData.Add(Now)
                        arRowData.Add(Request.UserHostName())
                        If SQL.Save("T220021", "UploadFile", "SaveFile-183", arColumnName, arRowData) = True Then
                            arRowData.Clear()
                        End If
                    End If
                End If
            Next
            arRowData.Clear()
            arColumnName.Clear()
            Return True
        Catch ex As Exception
            CreateLog("SaveDocumentData", "SaveDocumentData-294", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Dim shFlg As Short = 0
        Dim shAcessFlag As Short = 0
        lstError.Items.Clear()

        If txtFID.Text.Trim <> "" Then
            If UpdateFileInfo() = True Then
                For intI = 0 To cblObjects.Items.Count - 1
                    If cblObjects.Items(intI).Selected = True Then
                        shAcessFlag = 1
                        Exit For
                    End If
                Next
                If shAcessFlag = 0 Then
                    lstError.Items.Add("Records saved sucesfully...")
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Exit Sub
                End If
                If dtValidFromDate.CalendarDate.Trim Is "" Then
                    shFlg = 1
                    lstError.Items.Add("Valid From date cannot be blank ...")
                End If
                If dtValidToDate.CalendarDate.Trim Is "" Then
                    shFlg = 1
                    lstError.Items.Add("Valid To date cannot be blank ...")
                End If
                If dtValidFromDate.CalendarDate.Trim <> "" And dtValidToDate.CalendarDate.Trim <> "" Then
                    If CDate(dtValidFromDate.CalendarDate) > CDate(dtValidToDate.CalendarDate) Then
                        shFlg = 1
                        lstError.Items.Add(" Valid From date cannot  Greater than Valid To Date...")
                    End If
                End If
                If dtValidFromDate.CalendarDate.Trim <> "" And dtValidToDate.CalendarDate.Trim <> "" Then
                    If CDate(dtValidFromDate.CalendarDate) < Now.ToShortDateString Then
                        shFlg = 1
                        lstError.Items.Add(" Valid From date cannot  less than Current Date...")
                    End If
                End If
                If shFlg = 1 Then
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
                cpnlErrorPanel.Visible = False
                If SaveDocumentData() = True Then
                    'Panels 
                    cpnlAccessInfo.Enabled = True
                    cpnlAccessInfo.State = CustomControls.Web.PanelState.Expanded
                    cpnlAccessInfo.TitleCSS = "Test"
                    lstError.Items.Clear()
                    lstError.Items.Add("Records saved sucesfully...")
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    ShowRuleGrid(Val(txtFID.Text.Trim))
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy,Plz try Later...")
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Record Not Updated, Please Try Later...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        End If
    End Sub
    Sub ShowRuleGrid(ByVal FileID As Integer)
        Try
            Dim dsDocument As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.Search("T220021", "", "", "select FS_NU9_Policy_ID_PK RuleID, COM.Ci_vc36_name as Company,FS_VC8_Object_Type as ObjectType,Role.ROM_VC50_Role_Name as Name,convert(varchar,FS_DT8_Valid_From,101) as ValidFrom, convert(varchar,FS_DT8_Valid_UpTo,101) as ValidTo,(case when FS_VC4_Status='ENB' then 'Enable' when FS_VC4_Status='DSB' then 'Disable' else '' end ) as Status from T220021,T220011,t010011 COM,T070031 Role where FS_NU9_File_ID_FK=FM_NU9_File_ID_PK and FS_NU9_Company_ID_FK=COM.CI_NU8_Address_Number and FS_NU9_Object_ID_FK=ROM_IN4_Role_ID_PK and FS_NU9_File_ID_FK=" & FileID & " union select  FS_NU9_Policy_ID_PK RuleID, COM.Ci_vc36_name as Company,FS_VC8_Object_Type as ObjectType,ABUser.Ci_vc36_name as Name,convert(varchar,FS_DT8_Valid_From,101) as ValidFrom,convert(varchar,FS_DT8_Valid_UpTo,101) as ValidTo,(case when FS_VC4_Status='ENB' then 'Enable' when FS_VC4_Status='DSB' then 'Disable' else '' end ) as Status from T220021,T220011,t010011 COM,t010011 ABUser where FS_NU9_File_ID_FK=FM_NU9_File_ID_PK and FS_NU9_Company_ID_FK=COM.CI_NU8_Address_Number and FS_NU9_Object_ID_FK=ABUser.CI_NU8_Address_Number and FS_NU9_File_ID_FK=" & FileID & "", dsDocument, "", "")
            Dim htDateCols As New Hashtable
            htDateCols.Add("ValidFrom", 2)
            htDateCols.Add("ValidTo", 2)
            SetDataTableDateFormat(dsDocument.Tables(0), htDateCols)
            If dsDocument.Tables(0).Rows.Count > 0 Then
                GrdDocuments.DataSource = dsDocument
                GrdDocuments.DataBind()
            Else
                cpnlAccessInfo.Enabled = False
                cpnlAccessInfo.State = CustomControls.Web.PanelState.Collapsed
                cpnlAccessInfo.TitleCSS = "Test2"
            End If


        Catch ex As Exception
            CreateLog("ShowDocumentGrid", "DocumentDetail", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub DDLCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLCompany.SelectedIndexChanged
        If DDLCompany.SelectedIndex <> -1 Then
            FillNonUDCDropDown(ddlGroup, "Select Name as ID, Description from UDC  where  ProductCode=0   and UDCType='GRP' and (UDC.Company=" & DDLCompany.SelectedValue & " OR udc.Company=0)", False)
        End If
        Call DLLObjectType_SelectedIndexChanged(Me, New EventArgs)
    End Sub

    Private Sub DLLObjectType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DLLObjectType.SelectedIndexChanged
        Try
            Select Case DLLObjectType.SelectedValue
                Case "ROLE"
                    Call GetRoles()
                Case "USER"
                    Call GetAllUsers()
            End Select

        Catch ex As Exception
            CreateLog("DocumentDetail", "DDLAccessType_SelectedIndexChanged", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Function UpdateFileInfo() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            If DDLFileGroup.SelectedValue <> ViewState("Group") Then
                Dim DIR As New DirectoryInfo(MapPath("../Dockyard/ImportExport/" & DDLFileGroup.SelectedValue & "/"))
                If DIR.Exists Then

                Else
                    DIR.Create()
                End If

                Dim strNewPath As String
                Dim strVersion As String
                strVersion = GetVersion(txtDocumentName.Text.Trim, DDLFileGroup.SelectedValue)

                strNewPath = "../Dockyard/ImportExport/" & DDLFileGroup.SelectedValue & "/" & strVersion & "/" & txtDocumentName.Text.Trim
                If Directory.Exists(System.IO.Path.GetDirectoryName(MapPath(strNewPath))) = False Then
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(MapPath(strNewPath)))
                End If
                If File.Exists(MapPath(strNewPath)) Then
                    File.Delete(MapPath(strNewPath))
                End If
                If File.Exists(MapPath(hypFile.NavigateUrl.Trim)) Then
                    File.Move(MapPath(hypFile.NavigateUrl.Trim), MapPath(strNewPath))
                End If

                If SQL.Update("", "", "Update T220011 set FM_VC256_File_Version='" & strVersion & "', FM_VC512_File_Path='" & strNewPath & "', FM_VC8_File_Group='" & DDLFileGroup.SelectedValue & "', FM_VC512_File_Description='" & txtDocDescription.Text.Trim & "' where FM_NU9_File_ID_PK=" & txtFID.Text.Trim & "", SQL.Transaction.Serializable) = True Then
                    hypFile.NavigateUrl = strNewPath
                    txtVersion.Text = strVersion
                    ViewState("Group") = DDLFileGroup.SelectedValue
                End If
            Else
                If SQL.Update("", "", "Update T220011 set FM_VC512_File_Description='" & txtDocDescription.Text.Trim & "' where FM_NU9_File_ID_PK=" & txtFID.Text.Trim & "", SQL.Transaction.ReadCommitted) = True Then

                End If
            End If
            Return True
        Catch ex As Exception
            CreateLog("UpdateFileInfo", "DocumentDetail", LogType.Application, LogSubType.Exception, "", ex.ToString, Session("PropUserID"), Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function GetFileData(ByVal FileID As Integer, ByVal ObjectID As Integer, ByVal ObjectType As String) As Boolean
        Try
            Dim dsFile As New DataSet
            SQL.Search("t220011", "DocumentDetail", "GetFileData", "select count(FS_NU9_Policy_ID_PK) from t220021 where FS_NU9_File_ID_FK=" & FileID & " and FS_VC8_Object_Type='" & ObjectType & "' and FS_NU9_Object_ID_FK=" & ObjectID & "", dsFile, "", "")
            If Val(dsFile.Tables(0).Rows(0).Item(0)) > 0 Then
                dsFile = Nothing
                Return False
            Else
                dsFile = Nothing
                Return True
            End If
        Catch ex As Exception
            CreateLog("DocumentDetail", "GetFileData", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function

    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("DocumentView.aspx", False)
    End Sub

    Private Sub DDLFileComp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLFileComp.SelectedIndexChanged
        FillNonUDCDropDown(ddlGroup, "Select Name as ID, Description from UDC  where  ProductCode=0   and UDCType='GRP' and (UDC.Company=" & DDLFileComp.SelectedValue & " OR udc.Company=0)", False)

    End Sub

    Private Sub GrdDocuments_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDocuments.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            e.Item.Attributes.Add("OnClick", "KeyCheck('" & GrdDocuments.DataKeys(e.Item.ItemIndex) & "','" & e.Item.ItemIndex + 1 & "')")
        End If
    End Sub

    Private Function DeleteRule(ByVal RuleID As Integer) As Boolean
        Try
            Dim strSQL As String
            strSQL = "delete from T220021 where FS_NU9_Policy_ID_PK=" & RuleID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Delete("", "", strSQL, SQL.Transaction.Serializable) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("DocumentDetail", "DeleteRule", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
            Return False
        End Try
    End Function
End Class
