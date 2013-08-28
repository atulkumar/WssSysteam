'************************************************************************************************************
' Page                   :- Folder Settings
' Purpose                :- Purpose of this screen is to Create and Edit details of folder.
' Tables used            :- T025021 
' Date					Author						Modification Date					Description
' 06/02/08			    Ranvijay Sahay	                -------------------				    Created
' Session Variable required: SelectedCompanyName, ParentFolderID, FolderID
' Viewstate: FolderID
' QueryStringRequired: FolderEmpty(True/False)
'************************************************************************************************************
Imports ION.Data
Imports ION.Logging
Imports System.Data
Imports ION.Logging.EventLogging

'Session("PropUserID")
'Session("FolderID")
'Session("ParentFolderID")
'Session("FolderPath")

Partial Class DocumentsMgt_FolderSettings
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' -- if user is coming on the screen to add new folder. After this check will be on the folder id session
        '-- remove HTML tags
        txtCompany.Text = Regex.Replace(HttpContext.Current.Session("SelectedCompanyName"), "<[^>]*>", "")

        txtDescription.Attributes.Add("onmousemove", "ShowToolTip(this,5000);")
        If Page.IsPostBack = False Then
            txtCSS(Me.Page)
            ViewState("ButtonClick") = Request.QueryString("ButtonClick")
            ViewState("CompanyID") = WSSSearch.SearchCompName(txtCompany.Text.Trim).ExtraValue
            '-- viewsate is used instead of session because session cannot be changed as it is being used on other screen also
            ViewState("FolderID") = Session("FolderID")
            ViewState("ParentFolderID") = Session("ParentFolderID")

            If ViewState("ButtonClick") = "ADD" Then
                '-- set parentfolderid to folder id and folderid to 0
                ViewState("ParentFolderID") = ViewState("FolderID")
                ViewState("FolderID") = 0
                ' If folder already contains a folder/file then folder name cannot be edited
            Else
                If CType(Request.QueryString("FolderEmpty"), Boolean) = True Then
                    txtFolderName.ReadOnly = False
                Else
                    txtFolderName.ReadOnly = True
                End If
            End If
        End If
        txtParentFolder.Text = WSSSearch.GetFolderName(viewstate("ParentFolderID")).ExtraValue
        txtFolderPath.Text = Regex.Replace(Session("FolderPath"), "<[^>]*>", "")
        If viewstate("ButtonClick") = "ADD" Then  'If user wants to edit folder properties
            txtFolderName.ReadOnly = False '-- enable foldername if in add mode
        End If

        '-- fill data when in edit mode
        If Page.IsPostBack = False Then
            If Val(Viewstate("FolderID")) > 0 Then 'If user wants to edit folder properties
                FillData()
            End If
        End If

        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOK.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")

        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                
            Select Case txthiddenImage
                Case "Save"
                    Dim intID As Int32
                    If Not IsPostBack Then
                        intID = 995
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intID) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intID)
                    End If
                    'End of Security Block
                    If Val(ViewState("FolderID")) > 0 Then 'If user wants to edit folder properties
                        If UpdateFolder() = True Then
                            '   Response.Write("<script>self.opener.Location='FolderMgtMaster.aspx'</script>")
                        End If
                    Else
                        If SaveFolder() = True Then
                            '   Response.Write("<script>self.opener.Location='FolderMgtMaster.aspx'</script>")
                        End If
                    End If
                Case "Close"
                    Response.Write("<script>window.close();</script>")
                    Response.Write("<script>self.opener.location='FolderMgtMaster.aspx';</script>")

                Case "Ok"
                    'Security Block
                    If imgSave.Enabled = False Or imgSave.Visible = False Then
                        lstError.Items.Clear()
                        lstError.Items.Add("You don't have access rights to Save record...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                    'End of Security Block
                    If Val(ViewState("FolderID")) > 0 Then 'If user wants to edit folder properties
                        If UpdateFolder() = True Then
                            Response.Write("<script>self.opener.location='FolderMgtMaster.aspx';</script>")
                            Response.Write("<script>window.close();</script>")
                        End If
                    Else
                        If SaveFolder() = True Then
                            Response.Write("<script>self.opener.location='FolderMgtMaster.aspx';</script>")

                            Response.Write("<script>window.close();</script>")
                        End If
                    End If
                Case "Reset"
            End Select
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Error occured  please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End Try
        End If
        
    End Sub

    Private Sub FillData()
        Dim dsFolder As New DataSet

        Try
            ' If IsNothing(dsFolder) = False Then
            If Val(Viewstate("FolderID")) <> 0 Then
                dsFolder = WSSSearch.GetFolderDetail(Val(Viewstate("FolderID"))).ExtraValue()
            End If
            txtFolderName.Text = dsFolder.Tables(0).Rows(0).Item("FD_VC255_Folder_Name")
            txtDescription.Text = IIf(IsDBNull(dsFolder.Tables(0).Rows(0).Item("FD_VC5000_Folder_Description")), "", dsFolder.Tables(0).Rows(0).Item("FD_VC5000_Folder_Description"))
            'txtFolderPath.Text = IIf(IsDBNull(dsFolder.Tables(0).Rows(0).Item("FD_VC255_Folder_Path")), "", dsFolder.Tables(0).Rows(0).Item("FD_VC255_Folder_Path"))
            '   End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Error occured  please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
        End Try
    End Sub

    Private Function UpdateFolder() As Boolean
        If ValidateFields() = False Then
            Return False
            Exit Function
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        '---------------------
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        arColumnName.Add("FD_NU9_Company_ID_FK")
        arColumnName.Add("FD_NU9_Parent_Folder_ID_FK")
        arColumnName.Add("FD_VC255_Folder_Name")
        arColumnName.Add("FD_VC5000_Folder_Description")
        arColumnName.Add("FD_VC255_Folder_Path")
        arColumnName.Add("FD_VC16_Status")
        arColumnName.Add("FD_NU9_ModifyBy_ID_FK")
        arColumnName.Add("FD_VC32_Modify_From_IP")
        arColumnName.Add("FD_DT8_Modify_ON")

        arRowData.Add(WSSSearch.SearchCompName(txtCompany.Text.Trim).ExtraValue)
        arRowData.Add(viewstate("ParentFolderID"))
        arRowData.Add(txtFolderName.Text.Trim)
        arRowData.Add(txtDescription.Text.Trim)
        arRowData.Add(txtFolderPath.Text.Trim)
        arRowData.Add("")
        arRowData.Add(Session("PropUserID"))
        arRowData.Add(GetIP)
        arRowData.Add(Now)

        '----------------------
        mstGetFunctionValue = WSSUpdate.UpdateFolder(arColumnName, arRowData, "T250021", "Select * From T250021 Where FD_NU9_Folder_ID_PK=" & Viewstate("FolderID"))
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

    End Function
    Private Function SaveFolder() As Boolean

        If ValidateFields() = False Then
            Return False
            Exit Function
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        arColumnName.Add("FD_NU9_Company_ID_FK")
        arColumnName.Add("FD_NU9_Parent_Folder_ID_FK")
        arColumnName.Add("FD_VC255_Folder_Name")
        arColumnName.Add("FD_VC5000_Folder_Description")
        arColumnName.Add("FD_VC255_Folder_Path")
        arColumnName.Add("FD_VC16_Status")
        arColumnName.Add("FD_NU9_CreatedBy_ID_FK")
        arColumnName.Add("FD_VC32_Created_From_IP")
        arColumnName.Add("FD_DT8_Created_ON")

        arRowData.Add(Viewstate("CompanyID"))
        arRowData.Add(viewstate("ParentFolderID"))
        arRowData.Add(txtFolderName.Text.Trim)
        arRowData.Add(txtDescription.Text.Trim)
        arRowData.Add(txtFolderPath.Text.Trim)
        arRowData.Add("")
        arRowData.Add(Session("PropUserID"))
        arRowData.Add(GetIP)
        arRowData.Add(Now)

        mstGetFunctionValue = WSSSave.SaveFolder(arColumnName, arRowData, txtFolderName.Text.Trim, viewstate("ParentFolderID"), viewstate("CompanyID"))
        If mstGetFunctionValue.ErrorCode = 0 Then
            Viewstate("FolderID") = mstGetFunctionValue.ExtraValue
            Viewstate("ButtonClick") = "EDIT"

            lstError.Items.Clear()
            lstError.Items.Add("Folder Created successfully...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Return True
        End If
        If mstGetFunctionValue.ErrorCode = 1 Then
            lstError.Items.Clear()
            lstError.Items.Add("Error occur please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End If

    End Function

    Private Function ValidateFields() As Boolean
        Try
            lstError.Items.Clear()
            If txtFolderName.Text.Trim = "" Then
                lstError.Items.Add("Folder name cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
            End If
            'If viewstate("ButtonClick") = "ADD" Then
            If WSSSearch.CheckFolderExistance(txtFolderName.Text.Trim, viewstate("ParentFolderID"), Viewstate("CompanyID"), Viewstate("FolderID")).ExtraValue = True Then
                lstError.Items.Add("Folder name already exist...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
            End If
            ' End If
            Return True
        Catch ex As Exception
            CreateLog("ValidateFields", "ValidateFields-250", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

End Class
