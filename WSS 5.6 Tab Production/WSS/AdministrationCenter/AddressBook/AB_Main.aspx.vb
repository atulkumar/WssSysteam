'*******************************************************************
' Page                 : - AB_Main
' Purpose              : - Its purpose is to get all the necessary information like name, email address,                                business relation, contact numbers & Address lines from user.
' Tables Used          :-  T010023, T010033, UDC, T010011, TSL9901, T010043, T010053

' Date					Author	Sachin					Modification Date					Description
' 27/03/06											    -------------------					Created
'
' Notes: 
' Code:
'*******************************************************************

Imports ION.Data
Imports ION.Logging
Imports System.Web.Security
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports Microsoft.Win32

Partial Public Class AB_Main
    Inherits System.Web.UI.Page

#Region " Form Level Variables "

    Protected mdsAB_Addl As New DataSet 'used for Additional Address
    Private mdsInventory As New DataSet
    Protected mdsAB_SkillSet As New DataSet 'Used fro Skill Set

    Private mintAddressKey As Integer ' Will keep Contact key and used to store contact key in HttpContext.Current.Session
    Private mintSubAddressKey As Integer    ' This is needed for editing Additional Addressbook
    Public AB_UDCControl As structAB_UDC
    Public a As String
    Private mintAddSkill As Boolean 'Stores State Whether Skill is to Added or Updated

    Dim rowvalue, rowvalue1 As Integer

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'txtPostalCode.Attributes.Add("onkeypress", "NumericOnly();")
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        'clear the b rel session

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        If Not IsPostBack Then
            Session("PropBusinessRelation") = ""
            Session("PropABType") = ""

        End If
        If Not IsNothing(Request.QueryString("AddressNo")) Then
            ViewState("SAddressNumber_AddressBook") = Request.QueryString("AddressNo")
            showDocumentsUploaded()
        End If
        If Not IsPostBack Then
            FillComboTZ()
            GetJobRole()
        End If
        If Not IsPostBack Then
            Call txtCSS(Me.Page, "CpnlSkillSet", "cpnlInventory")
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            txtDept.Attributes.Add("ReadOnly", "ReadOnly")
            txtMgrName.Attributes.Add("ReadOnly", "ReadOnly")
            txtLevel.Attributes.Add("ReadOnly", "ReadOnly")
            txtBrName.Attributes.Add("ReadOnly", "ReadOnly")
            txtEmail1.Attributes.Add("onblur", "mail(this.value,this);")
            txtEmail2.Attributes.Add("onblur", "mail(this.value,this);")
            txtName.Attributes.Add("onkeypress", "CharacterOnly();")
            txtPhone1.Attributes.Add("onkeypress", "PhoneValidation();")
            txtPhone2.Attributes.Add("onkeypress", "PhoneValidation();")

            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1

            Dim strSelect As String = ""
            Dim blnView As Boolean
            Dim sqrdView1 As SqlDataReader = SQL.Search("AB_Search", "FillDefault-259", GetCompanySubQuery(), SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then
                While sqrdView1.Read
                    strSelect &= "'" & sqrdView1.Item("CompID") & "',"
                End While
            End If
            Dim str As String = strSelect.Remove(strSelect.Length - 1, 1)
            imgBR.Attributes.Add("onclick", "OpenBR(0,'COM','cPnlContact_txtBr');")

            txtQuery1.Text = str

            txtNoEmp.Attributes.Add("onkeypress", "NumericOnly();")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            If IsNothing(ViewState("SAddressNumber_AddressBook")) = True Then
                imgOpenManager.Attributes.Add("OnClick", "return OpenManager('cpnlPersonalInfo_txtMgr');")
                imgBtnAddMore.Attributes.Add("onclick", "OpenW_Add_Address('-1');")
            Else
                imgOpenManager.Attributes.Add("OnClick", "return OpenManager('cpnlPersonalInfo_txtMgr'," & ViewState("SAddressNumber_AddressBook") & ");")
                imgBtnAddMore.Attributes.Add("onclick", "OpenW_Add_Address('-1'," & ViewState("SAddressNumber_AddressBook") & ");")
            End If
            btnUpload.Attributes.Add("Onclick", "return SaveEdit('Upload');")
            BtnUploadResume.Attributes.Add("OnClick", "return SaveEdit('UploadResume');")
            btnRem.Attributes.Add("Onclick", "return SaveEdit('Remove');")
            BtnRemoveResume.Attributes.Add("Onclick", "return SaveEdit('RemoveResume');")
            btnFullSize.Attributes.Add("Onclick", "return SaveEdit('FullSize');")
        End If
        'Hide the plus button when page is opening in edit mode
        If ViewState("SAddressNumber_AddressBook") <> "-1" Then
        End If

        ' -- mintAddressKey will be -1 for add and 0 for no change else for edit
        Dim txthiddenGrid As String = Request.Form("txthiddenGrid")
        Dim txthiddenSkil As String = Request.Form("txthiddenSkil")
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        Dim txthidden As String = Request.Form("txthidden")
        Dim intID As Integer

        'Security Block
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
            'End of Security Block
        End If

        If IsNothing(Request.QueryString("AddressNo")) = True And IsNothing(ViewState("SAddressNumber_AddressBook")) = True Then
            mintAddressKey = -1
        Else
            mintAddressKey = CInt(ViewState("SAddressNumber_AddressBook"))
        End If

        mintAddSkill = -1
        If mintAddressKey > 0 Then
            EnablePanels()
        Else
            DisablePanels()
        End If

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Edit"
                        If txthiddenGrid = "cpnlAddress_grdAddress" Then
                            If CInt(txthiddenSkil) > 0 Then
                                ViewState("SSubAddressKey") = txthiddenSkil
                                Response.Write("<script>window.open('AB_Additional.aspx?ID=0&AddressNo=" & ViewState("SAddressNumber_AddressBook") & "&AddressKey=" & ViewState("SSubAddressKey") & "','Additional_Address','top='+ (screen.height - 616) / 2 +',left='+ (screen.width - 432) / 2 +',scrollBars=no,resizable=No,width=400,height=520,status=no');</script>")
                            Else
                                Response.Write("<script>alert('Please select the record to edit')</script>")
                            End If
                        Else
                            If CInt(txthiddenSkil) > 0 Then
                                ViewState("SSubAddressKey") = txthiddenSkil
                                Response.Write("<script>window.open('AB_Skill.aspx?ScrID=219&ID=" & (txthiddenSkil) & "&AddressNo=" & ViewState("SAddressNumber_AddressBook") & "&AddressKey=" & ViewState("SSubAddressKey") & "','Skill_Set','top='+ (screen.height - 566) / 2 +',left='+ (screen.width - 432) / 2 +', scrollBars=no,resizable=No,width=400,height=470,status=no');</script>")
                            Else
                                Response.Write("<script>alert('Please select the record to edit')</script>")
                            End If
                        End If
                    Case "Close"
                        Response.Redirect("ab_search.aspx?ScrID=37", False)
                    Case "Add"
                        Response.Redirect("AB_Main.aspx?ScrID=194&ID=-1")
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If SaveContact() = True Then
                            SaveCategoryInfo()

                            If SavePersonalInfo() = True And AddSkillSet() = True Then
                                Dim focusScript As String = "<script language='javascript'>" & _
                            "window.parent.closeTab();location.href=""ProjectView.aspx?ScrID=40""</script>"

                                ' Add the JavaScript code to the page.
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "FocusScript", focusScript)
                                'Page.RegisterStartupScript("FocusScript", focusScript)
                                'Response.Redirect("ab_search.aspx?ScrID=37", False)
                            Else
                                If txtSkill.Text.Trim().Equals("") And txtSkillType.Text.Trim().Equals("") And txtSkillComment.Text.Trim().Equals("") Then
                                    Dim focusScript As String = "<script language='javascript'>" & _
                                "window.parent.closeTab();location.href=""ProjectView.aspx?ScrID=40""</script>"
                                    ' Add the JavaScript code to the page.
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "FocusScript", focusScript)
                                    'Page.RegisterStartupScript("FocusScript", focusScript)
                                    ' Response.Redirect("ab_search.aspx?ScrID=37", False)
                                End If
                                Exit Sub
                            End If
                        Else
                            Exit Sub
                        End If
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then

                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If SaveContact() = True Then      ' Save contact Info 
                            If SaveCategoryInfo() = True And SavePersonalInfo() Then
                                lstError.Items.Clear()
                                If ViewState("Flag") = "" Then
                                    DisplayError("Record saved successfully...")
                                Else
                                    'In case status is disable
                                    If ViewState("Flag") = "UE" Then
                                        lstError.Items.Add("Record Saved Succesfully but Call/Task/Users are in progress... ")
                                    ElseIf (ViewState("Flag") = "UD") Then
                                        lstError.Items.Add("Record Saved Succesfully but Call/Task are in progress... ")
                                    End If

                                End If
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            End If
                        Else
                            Exit Sub
                        End If
                    Case "Delete"
                        If txthiddenGrid = "cpnlAddress_grdAddress" Then
                            If CInt(txthiddenSkil) > 0 Then
                                ViewState("SSubAddressKey") = txthiddenSkil
                                If SQL.Delete("AB_Main", "Load-300", "Delete From T010023 Where AA_NU8_Address_Number =" _
                                & ViewState("SAddressNumber_AddressBook") _
                                & " AND AA_NU8_Address_Sub_Number=" _
                                & ViewState("SSubAddressKey"), SQL.Transaction.ReadCommitted) Then
                                    lstError.Items.Clear()
                                    DisplayMessage("Record deleted successfully...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                Else
                                    lstError.Items.Clear()
                                    DisplayError("Error occured while deleting record...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If
                            Else
                                Response.Write("<script>alert('Please select the record to be deleted')</script>")
                            End If
                        Else
                            If CInt(txthiddenSkil) > 0 Then
                                If SQL.Delete("AB_Main", "Load-322", "Delete From T010033 Where ST_NU8_Address_Number =" _
                                & ViewState("SAddressNumber_AddressBook") & " AND ST_NU8_Skill_Number=" _
                                & txthiddenSkil, SQL.Transaction.ReadCommitted) Then
                                    lstError.Items.Clear()
                                    DisplayMessage("Record deleted successfully...")
                                Else
                                    DisplayError("Error occured while deleting record...")
                                End If
                            Else
                                Response.Write("<script>alert('Please select the record to be deleted')</script>")
                            End If
                        End If
                    Case "UploadResume"
                        uploadFiles()
                        'uploadResumeFile()
                        'UPdateResume()
                        Exit Sub
                    Case "Upload"
                        uploadImage()
                        Exit Sub
                    Case "Remove"
                        removeImage()
                        Exit Sub
                    Case "RemoveResume"
                        removeResume()
                        DelateResume()
                        Exit Sub
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("AB_Main", "Load-344", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
        ' This when records are coming for editing
        If IsPostBack = False And CInt(ViewState("SAddressNumber_AddressBook")) > 0 Then
            'DisablePanels()
            FillContact()

            Dim sqrdPersonalInfo As SqlDataReader
            mstGetFunctionValue = WSSSearch.SearchPersonalInfo(CInt(ViewState("SAddressNumber_AddressBook")), sqrdPersonalInfo)
            If mstGetFunctionValue.ErrorCode = 0 Then
                While sqrdPersonalInfo.Read
                    txtFirstName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC36_First_Name")), "", sqrdPersonalInfo.Item("PI_VC36_First_Name"))
                    txtMiddleName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC36_Middle_Name")), "", sqrdPersonalInfo.Item("PI_VC36_Middle_Name"))
                    txtLastName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC36_Last_Name")), "", sqrdPersonalInfo.Item("PI_VC36_Last_Name"))
                    If txtAB_Type.Text = "COM" Then
                        ddlWHFrom.SelectedValue = IIf(sqrdPersonalInfo.Item("PI_VC8_WHr_From") Is DBNull.Value, "01:00", sqrdPersonalInfo.Item("PI_VC8_WHr_From"))
                        ddlWHTo.SelectedValue = IIf(sqrdPersonalInfo.Item("PI_VC8_WHr_To") Is DBNull.Value, "01:00", sqrdPersonalInfo.Item("PI_VC8_WHr_To"))
                        txtBenName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC70_Bene_Name")), "", sqrdPersonalInfo.Item("PI_VC70_Bene_Name"))
                        txtAccount.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC30_Account_Number")), "", sqrdPersonalInfo.Item("PI_VC30_Account_Number"))
                        txtBankName0.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC70_Bank_Name")), "", sqrdPersonalInfo.Item("PI_VC70_Bank_Name"))
                        txtBankAdd.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC256_Bank_Address")), "", sqrdPersonalInfo.Item("PI_VC256_Bank_Address"))
                        txtSwiftCode.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC30_Swift_Code")), "", sqrdPersonalInfo.Item("PI_VC30_Swift_Code"))
                        txtRoutNo.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC30_Routing_Number")), "", sqrdPersonalInfo.Item("PI_VC30_Routing_Number"))
                        txtMICR.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC256_MICR")), "", sqrdPersonalInfo.Item("PI_VC256_MICR"))
                        txtICENo.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC256_IEC_Number")), "", sqrdPersonalInfo.Item("PI_VC256_IEC_Number"))
                        cpnlPersonalInfo.Enabled = False
                        cpnlPersonalInfo.State = CustomControls.Web.PanelState.Collapsed
                    Else
                        ddlWHrFrom.SelectedValue = IIf(sqrdPersonalInfo.Item("PI_VC8_WHr_From") Is DBNull.Value, "01:00", sqrdPersonalInfo.Item("PI_VC8_WHr_From"))
                        ddlWHrTo.SelectedValue = IIf(sqrdPersonalInfo.Item("PI_VC8_WHr_To") Is DBNull.Value, "01:00", sqrdPersonalInfo.Item("PI_VC8_WHr_To"))
                        cpnlPIComp.Enabled = False
                        cpnlPIComp.State = CustomControls.Web.PanelState.Collapsed
                    End If
                    ddGender.SelectedValue = IIf(sqrdPersonalInfo.Item("PI_VC8_Sex") Is DBNull.Value, "Male", sqrdPersonalInfo.Item("PI_VC8_Sex"))
                    ddlJob.Text = IIf(sqrdPersonalInfo.Item("PI_NU9_JobRole") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_NU9_JobRole"))
                    ddlbloodgroup.Text = IIf(sqrdPersonalInfo.Item("PI_VC15_BloodGroup") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC15_BloodGroup"))
                    ddMartialStatus.SelectedValue = IIf(sqrdPersonalInfo.Item("PI_VC8_Marital_Status") Is DBNull.Value, "Single", sqrdPersonalInfo.Item("PI_VC8_Marital_Status"))
                    If Not IsDBNull(sqrdPersonalInfo.Item("PI_DT8_Date_Of_Birth")) Then
                        dtDOB.Text = SetDateFormat(IIf(sqrdPersonalInfo.Item("PI_DT8_Date_Of_Birth") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_DT8_Date_Of_Birth")), mdlMain.IsTime.DateOnly)
                    End If
                    If Not IsDBNull(sqrdPersonalInfo.Item("PI_DT8_Date_Of_Joining")) Then
                        dtDOJ.Text = SetDateFormat(IIf(sqrdPersonalInfo.Item("PI_DT8_Date_Of_Joining") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_DT8_Date_Of_Joining")), mdlMain.IsTime.DateOnly)
                    End If
                    If Not IsDBNull(sqrdPersonalInfo.Item("PI_DT8_Date_Of_Leaving")) Then
                        dtDOL.Text = SetDateFormat(IIf(sqrdPersonalInfo.Item("PI_DT8_Date_Of_Leaving") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_DT8_Date_Of_Leaving")), mdlMain.IsTime.DateOnly)
                    End If
                    txtpath.Text = IIf(sqrdPersonalInfo.Item("PI_VC4_Picture") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC4_Picture"))
                    txtResumePath.Text = IIf(sqrdPersonalInfo.Item("PI_VC100_Resume") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC100_Resume"))
                    txtDept.Text = IIf(sqrdPersonalInfo.Item("PI_VC8_Department") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC8_Department"))
                    txtMgr.Text = IIf(sqrdPersonalInfo.Item("PI_NU9_Manager") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_NU9_Manager"))
                    If Not (sqrdPersonalInfo.Item("PI_NU9_Manager") Is DBNull.Value) Then
                        txtMgrName.Text = GetManagerName(sqrdPersonalInfo.Item("PI_NU9_Manager"))
                    End If
                    If Not IsDBNull(sqrdPersonalInfo.Item("PI_DT8_Open_date")) Then
                        dtOD.Text = SetDateFormat(IIf(sqrdPersonalInfo.Item("PI_DT8_Open_date") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_DT8_Open_date")), mdlMain.IsTime.DateOnly)
                    End If
                    txtCompType.Text = IIf(sqrdPersonalInfo.Item("PI_VC10_Company_Type") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC10_Company_Type"))
                    txtNoEmp.Text = IIf(sqrdPersonalInfo.Item("PI_NU9_Total_Employees") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_NU9_Total_Employees"))
                    txtCurrency.Text = IIf(sqrdPersonalInfo.Item("PI_VC8_Currency") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC8_Currency"))
                    If Not IsDBNull(sqrdPersonalInfo.Item("PI_CH1_Mail_Comm")) Then
                        If txtAB_Type.Text.Equals("COM") Then
                            chkCMailComm.Checked = IIf(sqrdPersonalInfo.Item("PI_CH1_Mail_Comm") = "0", False, True)
                        Else
                            chkEMailComm.Checked = IIf(sqrdPersonalInfo.Item("PI_CH1_Mail_Comm") = "0", False, True)
                        End If
                    End If
                    hypresume.Text = System.IO.Path.GetFileName(IIf(sqrdPersonalInfo.Item("PI_VC100_Resume") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC100_Resume")))
                    hypresume.NavigateUrl = IIf(sqrdPersonalInfo.Item("PI_VC100_Resume") Is DBNull.Value, "", sqrdPersonalInfo.Item("PI_VC100_Resume"))
                    If IsDBNull(sqrdPersonalInfo.Item("PI_VC4_TimeZone")) = False Then
                        Dim strUTZ As String = sqrdPersonalInfo.Item("PI_VC4_TimeZone")
                        If strUTZ.IndexOf("[") > 0 Then
                        Else
                            Dim rootkey As RegistryKey
                            Dim subkeynames As String()
                            Dim subkeyname As String
                            rootkey = Registry.LocalMachine
                            subkeynames = rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\", False).GetSubKeyNames
                            Dim slstTZ As New SortedList
                            For Each subkeyname In subkeynames
                                Dim strName As String = rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\" & subkeyname.ToString & "\", False).GetValue("Display")
                                If strName = strUTZ Then
                                    strUTZ = strName & "  [" & rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\" & subkeyname.ToString & "\", False).GetValue("Std") & "]"
                                    Exit For
                                End If
                            Next
                        End If
                        Try
                            ddlTZ.SelectedValue = strUTZ
                        Catch ex As Exception
                        End Try
                    End If
                    If Not txtpath.Text.Trim = "" Then
                        imgDesign.ImageUrl = txtpath.Text.Trim
                    End If

                End While
                sqrdPersonalInfo.Close()
            End If

            Dim sqrdCategoryInfo As SqlDataReader
            mstGetFunctionValue = WSSSearch.SearchCategoryInfo(CInt(ViewState("SAddressNumber_AddressBook")), sqrdCategoryInfo)
            If mstGetFunctionValue.ErrorCode = 0 Then
                While sqrdCategoryInfo.Read
                    txtCatCode1.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code1")
                    txtCatCode2.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code2")
                    txtCatCode3.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code3")
                    txtCatCode4.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code4")
                    txtCatCode5.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code5")
                    txtCatCode6.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code6")
                    txtCatCode7.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code7")
                    txtCatCode8.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code8")
                    txtCatCode9.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code9")
                    txtCatCode10.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code10")
                End While
                sqrdCategoryInfo.Close()
            End If

            ViewState("SSubAddressKey") = -1

            BindAdditionalAddress()
            BindSkillSet()
            BindInventoryGrid()
        ElseIf IsPostBack = True And CInt(ViewState("SAddressNumber_AddressBook")) > 0 Then

            FillContact()

            Dim sqrdPersonalInfo As SqlDataReader
            mstGetFunctionValue = WSSSearch.SearchPersonalInfo(CInt(ViewState("SAddressNumber_AddressBook")), sqrdPersonalInfo)
            If mstGetFunctionValue.ErrorCode = 0 Then
                While sqrdPersonalInfo.Read
                    txtFirstName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC36_First_Name")), "", sqrdPersonalInfo.Item("PI_VC36_First_Name"))
                    txtMiddleName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC36_Middle_Name")), "", sqrdPersonalInfo.Item("PI_VC36_Middle_Name"))
                    txtLastName.Text = IIf(IsDBNull(sqrdPersonalInfo.Item("PI_VC36_Last_Name")), "", sqrdPersonalInfo.Item("PI_VC36_Last_Name"))
                End While
                sqrdPersonalInfo.Close()
            End If

            Dim sqrdCategoryInfo As SqlDataReader
            mstGetFunctionValue = WSSSearch.SearchCategoryInfo(CInt(ViewState("SAddressNumber_AddressBook")), sqrdCategoryInfo)
            If mstGetFunctionValue.ErrorCode = 0 Then
                While sqrdCategoryInfo.Read
                    txtCatCode1.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code1")
                    txtCatCode2.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code2")
                    txtCatCode3.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code3")
                    txtCatCode4.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code4")
                    txtCatCode5.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code5")
                    txtCatCode6.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code6")
                    txtCatCode7.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code7")
                    txtCatCode8.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code8")
                    txtCatCode9.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code9")
                    txtCatCode10.Text = sqrdCategoryInfo.Item("CC_VC8_Category_Code10")
                End While
                sqrdCategoryInfo.Close()
            End If

            ' HttpContext.Current.ViewState("SSubAddressKey") = -1

            BindAdditionalAddress()
            AddSkillSet()
            BindSkillSet()
            BindInventoryGrid()
        ElseIf IsPostBack = False And CInt(ViewState("SAddressNumber_AddressBook")) <= 0 Then
            DisablePanels()
        End If

        a = AB_UDCControl.ConstAB_Type

        If IsPostBack = True And CInt(ViewState("SAddressNumber_AddressBook")) > 0 Then
            If txtAB_Type.Text = "COM" Then
                cpnlPersonalInfo.Enabled = False
                cpnlPersonalInfo.State = CustomControls.Web.PanelState.Collapsed
            Else
                cpnlPIComp.Enabled = False
                cpnlPIComp.State = CustomControls.Web.PanelState.Collapsed
            End If
        End If

        If txthiddenImage = "Upload" Or txthiddenImage = "Remove" Then
            cpnlPersonalInfo.Enabled = True
            cpnlPersonalInfo.State = CustomControls.Web.PanelState.Expanded
        End If
    End Sub


    Private Sub uploadFiles()
        Try
            If Not UploadResume.PostedFile Is Nothing And UploadResume.PostedFile.ContentLength > 0 Then
                Dim fn As String = System.IO.Path.GetFileName(UploadResume.PostedFile.FileName)
                '            Dim Ext = upload.Accept
                Dim strPath As String = Server.MapPath("../../Dockyard/ABResumes")
                Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("SAddressNumber_AddressBook"))
                If objFolder.Exists = False Then
                    ' Then Create the folder for that Resume.
                    objFolder.Create()
                End If

                Dim SaveLocation As String = String.Empty
                Try

                    SaveLocation = Server.MapPath("..\..\Dockyard\ABResumes") & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn

                    UploadResume.PostedFile.SaveAs(SaveLocation)
                    'txtResumePath.Text = "..\..\Dockyard\ABResumes\" & fn
                    txtResumePath.Text = "../../Dockyard/ABResumes" & "/" & ViewState("SAddressNumber_AddressBook") & "/" & fn  ''''' change path for download
                    '  lblResumeName.Text = fn
                    hypresume.Text = fn
                    hypresume.NavigateUrl = txtResumePath.Text


                Catch ex As Exception
                    DisplayError("Resume not uploaded plz Try Later...")
                    CreateLog("AB_Main", "uploadResumeFile-1971", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdskillset", )
                End Try

                Dim cmd As New SqlCommand
                Dim sqlCon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                cmd.CommandText = "insert into tblDocuments(AddressBookID,FileName,FilePath,DocType,UserID) values('" & Convert.ToInt16(ViewState("SAddressNumber_AddressBook")) & "','" & fn & "','" & SaveLocation & "','" & txtDocType.Text & " ','" & Convert.ToInt16(HttpContext.Current.Session("PropUserID")) & "')"
                cmd.Connection = sqlCon
                If sqlCon.State = ConnectionState.Closed Then
                    sqlCon.Open()
                End If
                cmd.ExecuteNonQuery()
                If sqlCon.State = ConnectionState.Open Then
                    sqlCon.Close()
                End If
                cmd.Dispose()
                showDocumentsUploaded()
                lstError.Items.Clear()
                lstError.Items.Add("Document Uploaded Successfuly...")
                'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                '"Please select a file"
            End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub showDocumentsUploaded()
        Try
            Dim cmd As New SqlCommand
            Dim sqlCon As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)

            cmd.CommandText = "select fileName from tblDocuments where addressBookID='" & Convert.ToInt16(ViewState("SAddressNumber_AddressBook")) & "'"
            cmd.Connection = sqlCon
            If sqlCon.State = ConnectionState.Closed Then
                sqlCon.Open()
            End If
            Dim adp As New SqlDataAdapter
            adp.SelectCommand = cmd
            Dim ds As New DataSet
            adp.Fill(ds)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    grdDocs.DataSource = ds.Tables(0)
                    grdDocs.DataBind()
                End If
            End If
            If sqlCon.State = ConnectionState.Open Then
                sqlCon.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillComboTZ()
        Try
            Dim rootkey As RegistryKey
            Dim subkeynames As String()
            Dim subkeyname As String
            ddlTZ.Items.Clear()
            rootkey = Registry.LocalMachine
            subkeynames = rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\", False).GetSubKeyNames
            Dim slstTZ As New SortedList
            ddlTZ.Items.Add("")
            Dim strSlectedTimeZone As String
            For Each subkeyname In subkeynames
                Dim strName As String = rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\" & subkeyname.ToString & "\", False).GetValue("Display") & "  [" & rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\" & subkeyname.ToString & "\", False).GetValue("Std") & "]"
                Dim intIndex As Integer = rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\" & subkeyname.ToString & "\", False).GetValue("Index")
                slstTZ.Add(intIndex, strName)
                If System.TimeZone.CurrentTimeZone.StandardName = rootkey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\" & subkeyname.ToString & "\", False).GetValue("Std") Then
                    strSlectedTimeZone = strName
                End If
            Next
            ddlTZ.DataTextField = "Value"
            ddlTZ.DataValueField = "Value"
            ddlTZ.DataSource = slstTZ
            ddlTZ.DataBind()
            Try
                ddlTZ.SelectedValue = strSlectedTimeZone
            Catch ex As Exception
            End Try
        Catch ex As Exception
            CreateLog("AB_Main", "FillComboTZ-661", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdaddress", )
        End Try
    End Sub
    Private Function GetJobRole()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = " Select Description,Name from UDC where UDCType='JOBR'"
            If SQL.Search("UDC", "AB_Main", "GetJobRole", sqstr, dsTemp, "", "") = True Then

                ddlJob.DataSource = dsTemp.Tables(0)
                'domain Name
                ddlJob.DataTextField = "Description"

                ddlJob.DataBind()
                ddlJob.Items.Insert(0, New ListItem("", "0"))
                'Else
                '    'SQL.Search is False Msgpanel show no domain exist for  selected company
                '    lstError.Items.Clear()
                '    lstError.Items.Add("No JobRole Exist")
                '    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetDomain-206", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Sub grdAddress_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' -- getting primary key from Selected Row
        'mintSubAddressKey = mdsAB_Addl.Tables(0).Rows(grdAddress.SelectedIndex)(0)
        ViewState("SSubAddressKey") = mdsAB_Addl.Tables(0).Rows(grdAddress.SelectedIndex)(1)
        EnablePanels()
    End Sub

    Private Sub grdAddress_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAddress.ItemDataBound
        'Adding Attributes
        Dim dv As DataView = mdsAB_Addl.Tables(0).DefaultView
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim intCounter As Integer

        Try

            'For Each dcCol In dv.Table.Columns
            For intCounter = 1 To 12
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = grdAddress.DataKeys(e.Item.ItemIndex)
                    e.Item.Cells(dc.IndexOf(dc(intCounter - 1))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & ViewState("SAddressNumber_AddressBook") & "', '" & rowvalue1 & "', 'cpnlAddress_grdAddress')")
                    e.Item.Cells(dc.IndexOf(dc(intCounter - 1))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & ViewState("SAddressNumber_AddressBook") & "', '" & rowvalue1 & "', 'cpnlAddress_grdAddress')")

                End If
            Next
            rowvalue1 += 1
        Catch ex As Exception
            CreateLog("AB_Main", "ItemDataBound-490", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdaddress", )
        End Try
        ' End If
    End Sub

    Private Sub imgBtnAddMore_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim VarPara1 As String

        VarPara1 = "AddAddress.aspx?ID=" + mintSubAddressKey.ToString.Trim + " &Key=A&AddressNo=" + ViewState("SAddressNumber_AddressBook") + ""
        Response.Write("<script> window.open(""" + VarPara1 + """, ""ss"",""scrollBars=no,resizable=No,width=420,height=450,status=no "" );</script>")
    End Sub

    Private Sub grdSkillSet_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdSkillSet.ItemDataBound
        Try
            Dim dv As DataView = mdsAB_SkillSet.Tables(0).DefaultView
            Dim dcCol As DataColumn
            Dim dc As DataColumnCollection = dv.Table.Columns
            Dim strID As String
            Dim intCounter As Integer

            'For Each dcCol In dv.Table.Columns
            For intCounter = 2 To 5
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = grdSkillSet.DataKeys(e.Item.ItemIndex)
                    e.Item.Cells(dc.IndexOf(dc(intCounter - 2))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & ViewState("SAddressNumber_AddressBook") & "', '" & rowvalue & "', 'CpnlSkillSet_grdSkillSet')")
                    e.Item.Cells(dc.IndexOf(dc(intCounter - 2))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & ViewState("SAddressNumber_AddressBook") & "', '" & rowvalue & "', 'CpnlSkillSet_grdSkillSet')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("AB_Main", "ItemDataBound-523", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdskillset", )
        End Try

    End Sub

#Region "User Defined Sub And Functions"

    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        Dim objTextBox As Control

        For Each objTextBox In CPnl.Controls
            If TypeOf objTextBox Is TextBox Then
                CType(objTextBox, TextBox).Text = ""
            End If
        Next
    End Sub

    Private Sub BindAdditionalAddress()


        If ViewState("SAddressNumber_AddressBook") = "-1" Then
            lstError.Items.Add("Save Contact info first...")
            Exit Sub
        Else
            cpnlAddress.Enabled = True
        End If

        ' -- Binding Additional Address Grid
        Dim strSql As String
        Dim intCount As Integer
        Dim blnStatus As Boolean

        strSql = "Select AA_NU8_Address_Number, AA_NU8_Address_Sub_Number, AA_VC36_Name, AA_VC8_Status, " _
        & " AA_VC8_AddressType, AA_VC36_Address_Line_1, AA_VC36_Address_Line_2, AA_VC36_Address_Line_3, AA_VC8_City, " _
        & " AA_VC8_Province, AA_NU8_Postal_Code, AA_VC8_Country, AA_VC36_Contact_Person " _
        & " From T010023 Where AA_NU8_Address_Number=" & ViewState("SAddressNumber_AddressBook").ToString
        strSql = strSql & " Order By AA_NU8_Address_Number"
        Try
            intCount = mdsAB_Addl.Tables.Count
            If intCount > 0 Then
                mdsAB_Addl.Clear()
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False

            If SQL.Search("T010023", "AB_Main", "BindAdditionalAddress-583", strSql, mdsAB_Addl, "sachin", "Prashar") = True Then
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsAB_Addl)
                grdAddress.DataSource = mdsAB_Addl
                grdAddress.DataBind()
                cpnlAddress.Enabled = True
                If mdsAB_Addl.Tables(0).Rows.Count > 0 Then
                End If
            Else
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsAB_Addl)
                grdAddress.DataSource = mdsAB_Addl
                grdAddress.DataBind()
                cpnlAddress.Enabled = True
                If mdsAB_Addl.Tables(0).Rows.Count > 0 Then
                End If
            End If
        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("AB_Main", "BindAdditionalAddress-592", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Function BindInventoryGrid() As Boolean
        Try

            Dim strSql As String
            Dim intCount As Integer
            Dim blnStatus As Boolean

            strSql = "select ItemID, ItemGroup, ItemName,  Quantity, convert(varchar, IssueDate,101) IssueDate, case Returnable when 1 then 'Y' when 0 then 'N' end Returnable , convert(varchar, ExpectedReturnDate,101) ExpectedReturnDate, InventoryIssue.Status, Comments from InventoryIssue, ItemMaster where ID=ItemID and EmployeeID=" & Val(ViewState("SAddressNumber_AddressBook").ToString)

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False

            If SQL.Search("InventoryIssue", "AB_Main", "BindInventoryGrid-907", strSql, mdsInventory, "sachin", "Prashar") = True Then
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsInventory)
                grdInventory.DataSource = mdsInventory
                grdInventory.DataBind()
                cpnlInventory.Enabled = True
                If mdsInventory.Tables(0).Rows.Count > 0 Then

                End If
            Else
                If mdsInventory.Tables.Count > 0 Then
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdsInventory)
                    grdInventory.DataSource = mdsInventory
                    grdInventory.DataBind()
                    cpnlInventory.Enabled = True
                    If mdsInventory.Tables(0).Rows.Count > 0 Then

                    End If
                End If
            End If
        Catch ex As Exception
            CreateLog("AB_Main", "BindInventoryGrid-900", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function

    Private Function SaveContact() As Boolean
        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim intAddressNo As Int64
        Dim shFlag As Short
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String



        lstError.Items.Clear()
        If txtName.Text.Trim.Equals("") Then
            lstError.Items.Add("Name cannot be blank...")
            shFlag = 1
        End If

        'To chack duplicate company name in address book
        '****************************************************************************
        If mintAddressKey = -1 Then

            If txtAB_Type.Text.Trim.Equals("COM") Then
                Dim intRows As Integer
                If SQL.Search("", "", "select * from T010011 where CI_VC36_Name='" & txtName.Text.Trim & "' and CI_VC8_Address_Book_Type='COM'", intRows) = True Then
                    lstError.Items.Add("Company Name already exists...")
                    shFlag = 1
                End If
            End If

        Else

            If txtAB_Type.Text.Trim.Equals("COM") Then
                Dim intRows As Integer
                If SQL.Search("", "", "select * from T010011 where CI_VC36_Name='" & txtName.Text.Trim & "' and CI_VC8_Address_Book_Type='COM' and CI_NU8_Address_Number<>" & mintAddressKey & "", intRows) = True Then
                    lstError.Items.Add("Company Name already exists...")
                    shFlag = 1
                End If
            End If

        End If
        '***************************************************************************


        If txtAB_Type.Text.Trim.Equals("") Then
            lstError.Items.Add("Address type cannot be blank...")
            shFlag = 1
        End If

        If txtAddLine1.Text.Trim.Equals("") And txtAddLine2.Text.Trim.Equals("") And txtAddLine3.Text.Trim.Equals("") Then
            lstError.Items.Add("Address cannot be blank...")
            shFlag = 1
        End If

        If (Not (txtPhone1.Text.Trim.Equals("")) And IsNumeric(txtPhone1.Text.Trim) = False) Or (Not (txtPhone2.Text.Trim.Equals("")) And IsNumeric(txtPhone2.Text.Trim) = False) Then
            lstError.Items.Add("Phone numbers are not Numeric...")
            shFlag = 1
        End If
        If Not txtAB_Type.Text.Trim.Equals("COM") Then
            If txtUserProfileID.Text.Trim.Equals("") Then
                lstError.Items.Add("User Profile ID cannot be blank...")
                shFlag = 1
            End If
        End If
        If txtUserProfileID.Enabled = True Then
            If CheckUser() = True Then
                lstError.Items.Add("User Profile ID already exists...")
                shFlag = 1
            End If
        End If
        If txtEmail1.Text.Trim.Equals("") Then
            lstError.Items.Add("First Email ID is mandatory...")
            shFlag = 1
        End If
        'Begin***
        If txtCity.Text.Trim.Equals("") Then
            lstError.Items.Add("City cannot be blank...")
            shFlag = 1
        End If
        If txtProvince.Text.Trim.Equals("") Then
            lstError.Items.Add("Province cannot be blank...")
            shFlag = 1
        End If

        If txtCountry.Text.Trim.Equals("") Then
            lstError.Items.Add("Country cannot be blank...")
            shFlag = 1
        End If
        If txtEmailType1.Text.Trim.Equals("") Then
            lstError.Items.Add("Email Type cannot be blank...")
            shFlag = 1
        End If
        'If txtWebAddress.Text.Trim.Equals("") Then
        '    lstError.Items.Add("Health Card No. cannot be blank...")
        '    shFlag = 1
        'End If
        'If txtReference1.Text.Trim.Equals("") Then
        '    lstError.Items.Add("SSNo. cannot be blank...")
        '    shFlag = 1
        'End If

        'End***


        If shFlag = 1 Then
            shFlag = 0
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
            Exit Function
        End If


        lstError.Items.Clear()
        For intI As Integer = 0 To 12
            strUDCType = ""
            strErrorMessage = ""
            Select Case intI
                Case 0
                    strUDCType = "ABTY"
                    strName = txtAB_Type.Text.Trim.ToUpper
                    strErrorMessage = "Address Book type Mismatch..."
                Case 1
                    strUDCType = "STA"
                    strName = txtStatus.Text.Trim.ToUpper
                    strErrorMessage = "Status Mismatch..."
                Case 2
                    strUDCType = "CTY"
                    strName = txtCity.Text.Trim.ToUpper
                    strErrorMessage = "City Mismatch..."
                Case 3
                    strUDCType = "PROV"
                    strName = txtProvince.Text.Trim.ToUpper
                    strErrorMessage = "Province Mismatch..."
                Case 4
                    strUDCType = "CNTY"
                    strName = txtCountry.Text.Trim.ToUpper
                    strErrorMessage = "Country Mismatch..."
                Case 5
                    strUDCType = "EMLT"
                    strName = txtEmailType1.Text.Trim.ToUpper
                    strErrorMessage = "Email Type1 Mismatch..."
                Case 6
                    If Not (txtEmailType2.Text.Trim.Equals("")) Then
                        strUDCType = "EMLT"
                        strName = txtEmailType2.Text.Trim.ToUpper
                        strErrorMessage = "Email Type2 Mismatch..."
                    End If
                Case 7
                    If Not (txtAreaCode1.Text.Trim.Equals("")) Then
                        strUDCType = "ARCD"
                        strName = txtAreaCode1.Text.Trim.ToUpper
                        strErrorMessage = "Area Code1 Mismatch..."
                    End If
                Case 8
                    If Not (txtAreaCode2.Text.Trim.Equals("")) Then
                        strUDCType = "ARCD"
                        strName = txtAreaCode2.Text.Trim.ToUpper
                        strErrorMessage = "Area Code2 Mismatch..."
                    End If
                Case 9
                    If Not (txtCountryCode1.Text.Trim.Equals("")) Then
                        strUDCType = "CCD"
                        strName = txtCountryCode1.Text.Trim.ToUpper
                        strErrorMessage = "Country Code1 Mismatch..."
                    End If
                Case 10
                    If Not (txtPhoneType1.Text.Trim.Equals("")) Then
                        strUDCType = "PHTY"
                        strName = txtPhoneType1.Text.Trim.ToUpper
                        strErrorMessage = "Phone Type1 Mismatch..."
                    End If
                Case 11
                    If Not (txtCountryCode2.Text.Trim.Equals("")) Then
                        strUDCType = "CCD"
                        strName = txtCountryCode2.Text.Trim.ToUpper
                        strErrorMessage = "Country Code2 Mismatch..."
                    End If
                Case 12
                    If Not (txtPhoneType2.Text.Trim.Equals("")) Then
                        strUDCType = "PHTY"
                        strName = txtPhoneType2.Text.Trim.ToUpper
                        strErrorMessage = "Phone Type2 Mismatch..."
                    End If
            End Select

            Dim ss As String = txtEmail2.Text.Trim
            If strUDCType <> "" Then
                If CheckUDCValue(0, strUDCType, strName) = False Then
                    lstError.Items.Add(strErrorMessage)
                    shFlag = 1
                End If
            End If
        Next

        If txtAB_Type.Text.Trim.Equals("EM") Then
            If txtBrName.Text.Trim.Equals("") Then
                strErrorMessage = "Business Relation cannot be blank..."
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            Else
                Dim intRow As Integer
                If SQL.Search("AB_Main", "SaveContact-716", "select CI_VC36_Name from T010011 where CI_VC36_Name='" & txtBrName.Text.Trim & "'", intRow) = False Then
                    strErrorMessage = "Company Mismatch..."
                    lstError.Items.Add(strErrorMessage)
                    shFlag = 1
                End If
            End If
        ElseIf txtAB_Type.Text.Trim.Equals("COM") Then
            If txtBrName.Text.Trim.Equals("") Then
                strErrorMessage = "Company Business Relation type cannot be blank..."
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            Else
                Dim intRow As Integer
                If SQL.Search("AB_Main", "SaveContact-729", "select Name from UDC where UDCType='" & txtAB_Type.Text.Trim & "' and Name='" & txtBrName.Text.Trim & "'", intRow) = False Then
                    strErrorMessage = "Business Relation Mismatch... with address book type ..."
                    lstError.Items.Add(strErrorMessage)
                    shFlag = 1
                End If
            End If
        End If

        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Return False
            Exit Function
        End If
        'check company opning date and compaire with user joining date and leaving date validations
        '***********************************************************
        If txtAB_Type.Text.Trim.Equals("EM") Then
            Dim dtjoin As String
            dtjoin = SQL.Search("T010043", "checkvalidation-1005", "select PI_DT8_OPEN_DATE from T010043 where PI_NU8_ADDRESS_NO='" & txtBr.Text.Trim & "'")
            If dtjoin <> "" Then
                If dtDOJ.Text.Trim.Equals("") Then
                Else
                    If CDate(dtDOJ.Text.Trim) < CDate(dtjoin) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Employee's Joining date cannot less than company's opened date...")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Return False
                        Exit Function
                    End If
                End If
                If dtDOL.Text.Trim.Equals("") Or dtDOJ.Text.Trim.Equals("") Then
                Else
                    If CDate(dtDOL.Text.Trim) < CDate(dtDOJ.Text.Trim) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Employee's leaving date cannot less than joining date...")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Return False
                        Exit Function
                    End If
                End If
            Else
                If dtDOJ.Text.Trim <> "" Or dtDOL.Text.Trim <> "" Then
                    lstError.Items.Add("First Filled Company's Opening Date...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return False
                    Exit Function
                End If
            End If
        End If
        '************************************************************


        Try
            lstError.Items.Clear()
            arrColumns.Add("CI_VC36_Name")
            arrColumns.Add("CI_VC16_Alias")
            arrColumns.Add("CI_VC8_Address_Book_Type")
            arrColumns.Add("CI_IN4_Business_Relation")
            arrColumns.Add("CI_VC28_Homepage")
            arrColumns.Add("CI_VC28_Web_Address")
            arrColumns.Add("CI_VC8_Email_Type_1")
            arrColumns.Add("CI_VC28_Email_1")
            arrColumns.Add("CI_VC8_Email_Type_2")
            arrColumns.Add("CI_VC28_Email_2")
            arrColumns.Add("CI_VC8_Phone_Type_1")
            arrColumns.Add("CI_VC8_Country_Code_1")
            arrColumns.Add("CI_VC8_Area_code_1")
            arrColumns.Add("CI_NU16_Phone_Number_1")
            arrColumns.Add("CI_VC8_Phone_Type_2")
            arrColumns.Add("CI_VC8_Country_Code_2")
            arrColumns.Add("CI_VC8_Area_code_2")
            arrColumns.Add("CI_NU16_Phone_Number_2")
            arrColumns.Add("CI_VC36_Reference_1")
            arrColumns.Add("CI_VC36_Reference_2")
            arrColumns.Add("CI_VC36_ID_1")
            arrColumns.Add("CI_VC36_ID_2")
            arrColumns.Add("CI_VC36_Address_Line_1")
            arrColumns.Add("CI_VC36_Address_Line_2")
            arrColumns.Add("CI_VC36_Address_Line_3")
            arrColumns.Add("CI_VC8_City")
            arrColumns.Add("CI_VC8_Province")
            arrColumns.Add("CI_VC36_Postal_Code")
            arrColumns.Add("CI_VC8_Country")
            arrColumns.Add("CI_VC8_Status")
            arrColumns.Add("CI_VC8_Level")
            arrColumns.Add("CI_NU9_Modified_By")

            If txtName.Text.Equals("") Then
                lstError.Items.Add("Please enter the Name...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return False
                Exit Function
            End If

            lstError.Items.Clear()
            arrRows.Add(txtName.Text.Trim)
            arrRows.Add(txtAlias.Text.Trim)
            arrRows.Add(txtAB_Type.Text.ToUpper)     'get address book Type

            If txtAB_Type.Text.Trim.Equals("COM") Then
                arrRows.Add(txtBrName.Text.Trim.ToUpper)
                ' ElseIf txtAB_Type.Text.Trim.Equals("EM") Then
            Else
                arrRows.Add(txtBr.Text.Trim.ToUpper)
            End If

            arrRows.Add(txtHomePage.Text.Trim)
            arrRows.Add(txtWebAddress.Text.Trim)
            arrRows.Add(txtEmailType1.Text.Trim.ToUpper)
            arrRows.Add(txtEmail1.Text.Trim)
            arrRows.Add(txtEmailType2.Text.Trim.ToUpper)
            arrRows.Add(txtEmail2.Text.Trim)
            arrRows.Add(txtPhoneType1.Text.Trim.ToUpper)
            arrRows.Add(txtCountryCode1.Text.Trim)
            arrRows.Add(txtAreaCode1.Text.Trim)
            arrRows.Add(IIf(txtPhone1.Text.Trim = "", "0", txtPhone1.Text.Trim))
            arrRows.Add(txtPhoneType2.Text.Trim.ToUpper)
            arrRows.Add(txtCountryCode2.Text.Trim)
            arrRows.Add(txtAreaCode2.Text.Trim)
            arrRows.Add(IIf(txtPhone2.Text.Trim = "", "0", txtPhone2.Text.Trim))
            arrRows.Add(txtReference1.Text.Trim)
            arrRows.Add(txtReference2.Text.Trim)
            arrRows.Add(txtID1.Text.Trim)
            arrRows.Add(txtID2.Text.Trim)
            arrRows.Add(txtAddLine1.Text.Trim)
            arrRows.Add(txtAddLine2.Text.Trim)
            arrRows.Add(txtAddLine3.Text.Trim)
            arrRows.Add(txtCity.Text.Trim.ToUpper)
            arrRows.Add(txtProvince.Text.Trim.ToUpper)
            arrRows.Add(IIf(txtPostalCode.Text.Trim = "", "0", txtPostalCode.Text.Trim))
            arrRows.Add(txtCountry.Text.Trim.ToUpper)
            arrRows.Add(txtStatus.Text.Trim.ToUpper)
            arrRows.Add(txtLevel.Text.Trim.ToUpper)
            arrRows.Add(Session("PropUserID"))

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False
            '    SQL.DBTable = "T010011"

            If mintAddressKey = -1 Then
                intAddressNo = SQL.Search("AB_Main", "SaveContact-833", "select isnull(max(CI_NU8_Address_Number),0) from T010011")
                intAddressNo += 1
                arrColumns.Add("CI_NU8_Address_Number")
                arrRows.Add(intAddressNo.ToString)
                arrColumns.Add("CI_DT8_Date_Created")
                arrRows.Add(Now)
                arrColumns.Add("CI_DT8_Date_Modified")
                arrRows.Add(Now)
                If SQL.Save("T010011", "AB_Main", "SaveContact-838", arrColumns, arrRows) = True Then
                    ViewState("SAddressNumber_AddressBook") = intAddressNo
                    If Not txtAB_Type.Text.Trim.Equals("COM") Then
                        Call SaveUserProfile()
                    End If

                    If txtAB_Type.Text.Trim.Equals("COM") Then
                        Dim arColumn As New ArrayList
                        Dim arRowData As New ArrayList

                        arColumn.Add("NXT_ModuleName")
                        arColumn.Add("NXT_Code_No")
                        arColumn.Add("NXT_Next_No")
                        arColumn.Add("NXT_CH4_Branch_Code_FK")

                        arRowData.Add("Call")
                        arRowData.Add("102")
                        arRowData.Add("1")
                        arRowData.Add(intAddressNo)

                        '  SQL.DBTable = "TSL9901"
                        If SQL.Save("TSL9901", "AB_Main", "SaveContact-857", arColumn, arRowData) = False Then
                            CreateLog("ABMAIN", "SavCont-844", LogType.System, LogSubType.FailureAudit, "999", "Unable to add entry in TSL9901 for " & intAddressNo)
                        End If
                    End If


                    lstError.Items.Add("Data saved successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    EnablePanels()

                    arrColumns.Clear()
                    arrRows.Clear()
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Add("Server is busy please try later...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            Else
                If txtBr.Text <> Session("PropBusinessRelation") Then
                    If WSSSearch.SearchABNumber(mintAddressKey, WSSSearch.ABNumType.Both) = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("You cannot change the business relation as this record has been used with this business relation...")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Return False
                    End If
                End If
                If Session("PropABType") <> txtAB_Type.Text Then
                    If WSSSearch.SearchABNumber(mintAddressKey, WSSSearch.ABNumType.Both) = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("You cannot change the Address Book Type as this record has been already used... ")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Return False
                    End If
                End If
                'check address book status for employee type ab it shoud not be DISA if used anywhere
                If txtStatus.Text.Trim = "DISA" Then
                    If IsUserIDUsed(mintAddressKey, txtAB_Type.Text.Trim) = True Then
                        lstError.Items.Clear()
                        If txtAB_Type.Text.Trim.Equals("COM") Then
                            lstError.Items.Add("You cannot change the Company Status as its Call/Task/Users are in progress... ")
                            ViewState("Flag") = "UE"
                        Else
                            lstError.Items.Add("You cannot change the employee status as its Call/Task is in progress... ")
                            ViewState("Flag") = "UD"
                        End If
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        'Return False
                    End If
                End If
                ' Update the Address number
                arrColumns.Add("CI_DT8_Date_Modified")
                arrRows.Add(Now)
                If SQL.Update("T010011", "AB_Main", "SaveContact-885", "select * from T010011 where CI_NU8_Address_number=" & mintAddressKey & "", arrColumns, arrRows) = True Then
                    Dim strUserID As String = SQL.Search("", "", "select UM_VC50_UserID from T060011 where UM_IN4_Address_No_FK=" & Val(ViewState("SAddressNumber_AddressBook")))
                    If strUserID = Nothing Then
                        strUserID = ""
                    End If
                    If Not txtAB_Type.Text.Trim.Equals("COM") And strUserID.Equals("") Then
                        Call SaveUserProfile()
                    End If
                    lstError.Items.Add("Data updated successfully...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    ViewState("SAddressNumber_AddressBook") = mintAddressKey
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("AB_Main", "SaveContact-898", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

    End Function

    Private Sub FillContact()
        Dim strConnection As String = "Server=ion15;database=newwss;uid=sa;"
        Dim dsSearch As DataSet
        Dim inti As Integer
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String

        strSql = " SELECT *  FROM T010011 As T   WHERE  CI_NU8_Address_number=" & ViewState("SAddressNumber_AddressBook").ToString

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False
        ' SQL.DBTable = "T010011"

        Try
            sqrdRecords = SQL.Search("AB_Main", "FillContact-926", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)

            If blnStatus = False Then
                cpnlAddress.TitleCSS = "test"
                cpnlAddress.PanelCSS = "panel"
                ClearAllTextBox(cPnlContact)
            Else
                sqrdRecords.Read()
                '---------------------
                txtUserProfileID.Text = SQL.Search("", "", "select UM_VC50_UserID from T060011 where UM_IN4_Address_No_FK=" & Val(ViewState("SAddressNumber_AddressBook")))
                If Not txtUserProfileID.Text.Trim.Equals("") Then
                    txtUserProfileID.Enabled = False
                Else
                    txtUserProfileID.Enabled = True
                End If
                '---------------------
                txtName.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC36_Name")), " ", sqrdRecords.Item("CI_VC36_Name"))
                txtAlias.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC16_Alias")), "", sqrdRecords.Item("CI_VC16_Alias"))
                txtAB_Type.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Address_Book_Type")), "", sqrdRecords.Item("CI_VC8_Address_Book_Type"))
                If txtAB_Type.Text.Trim.ToUpper.Equals("COM") Then
                    txtUserProfileID.Enabled = False
                End If
                txtBrName.Text = IIf(IsDBNull(sqrdRecords.Item("CI_IN4_Business_Relation")), "", sqrdRecords.Item("CI_IN4_Business_Relation"))
                txtBr.Text = IIf(IsDBNull(sqrdRecords.Item("CI_IN4_Business_Relation")), "", sqrdRecords.Item("CI_IN4_Business_Relation"))
                'Save the business relatio in session to be used during update
                Session("PropBusinessRelation") = txtBr.Text
                Session("PropABType") = txtAB_Type.Text
                txtHomePage.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC28_Homepage")), "", sqrdRecords.Item("CI_VC28_Homepage"))
                txtWebAddress.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC28_Web_Address")), "", sqrdRecords.Item("CI_VC28_Web_Address"))
                txtEmailType1.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Email_Type_1")), "", sqrdRecords.Item("CI_VC8_Email_Type_1"))
                txtEmail1.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC28_Email_1")), "", sqrdRecords.Item("CI_VC28_Email_1"))
                txtEmailType2.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Email_Type_2")), "", sqrdRecords.Item("CI_VC8_Email_Type_2"))
                txtEmail2.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC28_Email_2")), "", sqrdRecords.Item("CI_VC28_Email_2"))
                txtPhoneType1.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Phone_Type_1")), "", sqrdRecords.Item("CI_VC8_Phone_Type_1"))
                txtCountryCode1.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Country_Code_1")), "", sqrdRecords.Item("CI_VC8_Country_Code_1"))
                txtAreaCode1.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Area_code_1")), "", sqrdRecords.Item("CI_VC8_Area_code_1"))
                txtPhone1.Text = IIf(IsDBNull(sqrdRecords.Item("CI_NU16_Phone_Number_1")), "", sqrdRecords.Item("CI_NU16_Phone_Number_1"))
                txtPhoneType2.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Phone_Type_2")), "", sqrdRecords.Item("CI_VC8_Phone_Type_2"))
                txtCountryCode2.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Country_Code_2")), "", sqrdRecords.Item("CI_VC8_Country_Code_2"))
                txtAreaCode2.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC8_Area_code_2")), "", sqrdRecords.Item("CI_VC8_Area_code_2"))
                txtPhone2.Text = IIf(IsDBNull(sqrdRecords.Item("CI_NU16_Phone_Number_2")), "", sqrdRecords.Item("CI_NU16_Phone_Number_2"))
                txtReference1.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_Reference_1")), "", sqrdRecords.Item("CI_VC36_Reference_1"))
                txtReference2.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_Reference_2")), "", sqrdRecords.Item("CI_VC36_Reference_2"))
                txtID1.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_ID_1")), "", sqrdRecords.Item("CI_VC36_ID_1"))
                txtID2.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_ID_2")), "", sqrdRecords.Item("CI_VC36_ID_2"))
                txtAddLine1.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_Address_Line_1")), "", sqrdRecords.Item("CI_VC36_Address_Line_1"))
                txtAddLine2.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_Address_Line_2")), "", sqrdRecords.Item("CI_VC36_Address_Line_2"))
                txtAddLine3.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_Address_Line_3")), "", sqrdRecords.Item("CI_VC36_Address_Line_3"))
                txtCity.Text = IIf(IsDBNull(sqrdRecords("CI_VC8_City")), "", sqrdRecords.Item("CI_VC8_City"))
                txtProvince.Text = IIf(IsDBNull(sqrdRecords("CI_VC8_Province")), "", sqrdRecords.Item("CI_VC8_Province"))
                txtPostalCode.Text = IIf(IsDBNull(sqrdRecords("CI_VC36_Postal_Code")), "", sqrdRecords.Item("CI_VC36_Postal_Code"))
                txtCountry.Text = IIf(IsDBNull(sqrdRecords("CI_VC8_Country")), "", sqrdRecords.Item("CI_VC8_Country"))
                txtStatus.Text = IIf(IsDBNull(sqrdRecords("CI_VC8_Status")), "", sqrdRecords.Item("CI_VC8_Status"))
                txtLevel.Text = IIf(IsDBNull(sqrdRecords("CI_VC8_Level")), "", sqrdRecords.Item("CI_VC8_Level"))
                sqrdRecords.Close()
                Call EnablePanels()
            End If
            'Get the value of txtBr 
            If IsNumeric(txtBrName.Text.Trim) = True Then
                mstGetFunctionValue = WSSSearch.SearchUserName(txtBr.Text.Trim)
                txtBrName.Text = mstGetFunctionValue.ExtraValue
            Else
            End If
        Catch ex As Exception

            lstError.Items.Add("Server is busy please try later...")    'ex.Message.ToString()
            CreateLog("AB_Main", "FillContact-976", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub EnablePanels()
        cpnlAddress.TitleCSS = "test"
        cpnlAddress.Enabled = True

        cpnlCategory.TitleCSS = "test"
        cpnlCategory.Enabled = True

        cpnlInventory.TitleCSS = "test"
        cpnlInventory.Enabled = True

        If txtAB_Type.Text = "COM" Then
            cpnlPersonalInfo.TitleCSS = "test2"
        Else
            cpnlPersonalInfo.TitleCSS = "test"
        End If

        'cpnlPersonalInfo.State = CustomControls.Web.PanelState.Collapsed
        cpnlPersonalInfo.Enabled = True

        If txtAB_Type.Text <> "COM" Then
            cpnlPIComp.TitleCSS = "test2"
        Else
            cpnlPIComp.TitleCSS = "test"
        End If
        If txtAB_Type.Text.Trim.Equals("COM") Then
            'cpnlPIComp.State = CustomControls.Web.PanelState.Collapsed
            cpnlPIComp.Enabled = True
        End If
        CpnlSkillSet.TitleCSS = "test"
        'CpnlSkillSet.State = CustomControls.Web.PanelState.Collapsed
        CpnlSkillSet.Enabled = True

    End Sub

    Private Sub DisablePanels()
        cpnlAddress.TitleCSS = "test2"
        cpnlAddress.State = CustomControls.Web.PanelState.Collapsed
        cpnlAddress.Enabled = False

        cpnlCategory.TitleCSS = "test2"
        cpnlCategory.State = CustomControls.Web.PanelState.Collapsed
        cpnlCategory.Enabled = False

        cpnlInventory.TitleCSS = "test2"
        cpnlInventory.State = CustomControls.Web.PanelState.Collapsed
        cpnlInventory.Enabled = False

        cpnlPersonalInfo.TitleCSS = "test2"
        cpnlPersonalInfo.State = CustomControls.Web.PanelState.Collapsed
        cpnlPersonalInfo.Enabled = False

        cpnlPIComp.TitleCSS = "test2"
        cpnlPIComp.State = CustomControls.Web.PanelState.Collapsed
        cpnlPIComp.Enabled = False

        CpnlSkillSet.TitleCSS = "test2"
        CpnlSkillSet.Enabled = False
    End Sub

    Private Sub BindSkillSet()
        Dim strSql As String
        Dim intCount As Integer
        Dim blnStatus As Boolean

        strSql = "Select ST_NU8_Address_Number,ST_NU8_Skill_Number,ST_VC8_Skill_Type,ST_VC8_Skill,ST_VC156_Comment " _
        & " From T010033 Where ST_NU8_Address_Number=" & ViewState("SAddressNumber_AddressBook").ToString

        strSql = strSql & " Order By ST_NU8_Address_Number"
        Try
            intCount = mdsAB_SkillSet.Tables.Count
            If intCount > 0 Then
                mdsAB_SkillSet.Clear()
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False

            If SQL.Search("T010033", "AB_Main", "BindSkillSet-1041", strSql, mdsAB_SkillSet, "sachin", "Prashar") = True Then
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsAB_SkillSet)
                grdSkillSet.DataSource = mdsAB_SkillSet
                grdSkillSet.DataBind()
                CpnlSkillSet.Enabled = True
                If mdsAB_SkillSet.Tables(0).Rows.Count > 0 Then
                End If
            Else
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsAB_SkillSet)
                grdSkillSet.DataSource = mdsAB_SkillSet
                grdSkillSet.DataBind()
                CpnlSkillSet.Enabled = True
                If mdsAB_SkillSet.Tables(0).Rows.Count > 0 Then
                End If
            End If
        Catch ex As Exception
            CreateLog("AB_Main", "BindSkillSet-1043", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Function AddSkillSet() As Boolean
        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short = 0
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intSkillNo As Integer

        If txtSkill.Text.Trim().Equals("") And txtSkillType.Text.Trim().Equals("") And txtSkillComment.Text.Trim().Equals("") Then
            AddSkillSet = False
            Exit Function
        End If


        lstError.Items.Clear()
        '****Begin
        '**Check Blank Textbox
        For intI As Integer = 0 To 1

            Select Case intI
                Case 0
                    If txtSkill.Text.Trim.Equals("") Then
                        strErrorMessage = "Skill cannot be blank..."
                        shFlag = 1
                    End If
                Case 1
                    If txtSkillType.Text.Trim.Equals("") Then
                        strErrorMessage = "SkillType cannot be blank..."
                        shFlag = 1
                    End If
            End Select

            If CheckUDCValue(0, strUDCType, strName) = False And shFlag = 1 Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 2
            End If
        Next
        '****Check  skill & skill type Mismatch 
        If Not shFlag = 2 Then
            For intI As Integer = 0 To 1
                Select Case intI
                    Case 0
                        If txtSkill.Text.Trim.Equals("") = False Then
                            strUDCType = "SKL"
                            strName = txtSkill.Text.Trim.ToUpper
                            strErrorMessage = "Skill  Mismatch..."
                            shFlag = 1
                        End If
                    Case 1
                        If txtSkillType.Text.Trim.Equals("") = False Then
                            strUDCType = "SKTY"
                            strName = txtSkillType.Text.Trim.ToUpper
                            strErrorMessage = "Skill type Mismatch..."
                            shFlag = 1
                        End If
                End Select
                If CheckUDCValue(0, strUDCType, strName) = False And shFlag = 1 Then
                    lstError.Items.Add(strErrorMessage)
                    shFlag = 3
                End If

            Next
        End If

        If shFlag = 3 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Return False
            Exit Function
        End If
        If shFlag = 2 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Return False
            Exit Function
        End If
        '****End

        Try
            arrColumns.Add("ST_VC8_Skill_Type")
            arrColumns.Add("ST_VC8_Skill")
            arrColumns.Add("ST_VC156_Comment")

            arrRows.Add(txtSkillType.Text.Trim)
            arrRows.Add(txtSkill.Text.Trim)
            arrRows.Add(txtSkillComment.Text.Trim)

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False
            If mintAddSkill = -1 Then
                intSkillNo = SQL.Search("AB_Main", "AddSkillSet-1125", "select isnull(max(ST_NU8_Skill_Number),0) from T010033 where ST_NU8_Address_Number=" & ViewState("SAddressNumber_AddressBook"))
                intSkillNo += 1
                arrColumns.Add("ST_NU8_Skill_Number")
                arrRows.Add(intSkillNo.ToString)

                arrColumns.Add("ST_NU8_Address_Number")
                arrRows.Add(mintAddressKey)

                If SQL.Save("T010033", "AB_Main", "AddSkillSet-1133", arrColumns, arrRows) = True Then
                    Call DisplayMessage("Skill Set Info Saved...")
                    ClearAllTextBox(CpnlSkillSet)
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    Call DisplayError("Server is busy please try later...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            Else
                If SQL.Update("T010033", "AB_Main", "AddSkillSet-1146", "select * from T010033 where ST_NU8_Address_Number=" & mintAddressKey & " AND ST_NU8_Skill_Number=" & mintAddSkill.ToString, arrColumns, arrRows) = True Then
                    Call DisplayMessage("Skill Set Info Saved...")
                    Return True
                Else
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If

        Catch ex As Exception
            CreateLog("AB_Main", "AddSkillSet-1150", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try

    End Function

    Private Function SavePersonalInfo() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim strRecords As String
        Dim shFlag As Short
        lstError.Items.Clear()
        '   If Not txtAB_Type.Text = "COM" Then
        If Not dtDOB.Text.Trim.Equals("") Then
            If DateDiff(DateInterval.Year, CDate(dtDOB.Text), Now.Today) < 10 Or DateDiff(DateInterval.Year, CDate(dtDOB.Text), Now.Today) > 99 Then
                lstError.Items.Add("The date of Birth should be at least 10 years less than the current date...")
                shFlag = 1
            End If
        End If
        If dtDOB.Text.Trim.Equals("") Or dtDOJ.Text.Trim.Equals("") Then
        Else
            If DateDiff(DateInterval.Year, CDate(dtDOB.Text), CDate(dtDOJ.Text)) < 10 Or DateDiff(DateInterval.Year, CDate(dtDOB.Text), CDate(dtDOJ.Text)) > 100 Then
                lstError.Items.Add("Invalid Date of Joining...")
                shFlag = 1
            End If
        End If
        If dtDOB.Text.Trim.Equals("") Or dtDOL.Text.Trim.Equals("") Then
        Else
            If DateDiff(DateInterval.Year, CDate(dtDOB.Text), CDate(dtDOL.Text)) < 10 Or DateDiff(DateInterval.Year, CDate(dtDOB.Text), CDate(dtDOL.Text)) > 100 Then
                lstError.Items.Add("Date of Leaving should be 10 years greater than date of birth...")
                shFlag = 1
            End If
        End If
        If dtDOJ.Text.Trim.Equals("") Or dtDOL.Text.Trim.Equals("") Then
        Else
            If CDate(dtDOJ.Text) > CDate(dtDOL.Text) Then
                lstError.Items.Add("Invalid Date of Leaving...")
                shFlag = 1
            End If
        End If
        '  Else
        If dtOD.Text.Trim.Equals("") Then
        Else
            If CDate(dtOD.Text) > CDate(Now.Today) Then
                lstError.Items.Add("Invalid Company Opened Date...")
                shFlag = 1
            End If
        End If

        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Return False
            Exit Function
        End If

        If CInt(ViewState("SAddressNumber_AddressBook")) <> 0 Then
            ' Save Personal Info
            strRecords = SQL.Search("AB_Main", "SavePersonalInfo-1169", "Select PI_NU8_Address_No  From T010043 Where PI_NU8_Address_No=" & ViewState("SAddressNumber_AddressBook"))
            If strRecords Is Nothing Then
                arColumnName.Add("PI_NU8_Address_No")
                arColumnName.Add("PI_VC36_First_Name")
                arColumnName.Add("PI_VC36_Middle_Name")
                arColumnName.Add("PI_VC36_Last_Name")
                arColumnName.Add("PI_VC8_Sex")
                arColumnName.Add("PI_VC8_Marital_Status")
                arColumnName.Add("PI_DT8_Date_Of_Birth")
                'arColumnName.Add("PI_VC30_Role")
                arColumnName.Add("PI_VC8_WHr_From")
                arColumnName.Add("PI_VC8_WHr_To")
                arColumnName.Add("PI_VC4_TimeZone")
                arColumnName.Add("PI_VC4_Picture") 'picture
                arColumnName.Add("PI_DT8_Date_Of_Joining")
                arColumnName.Add("PI_DT8_Date_Of_Leaving")
                arColumnName.Add("PI_DT8_Open_date")
                arColumnName.Add("PI_NU9_Total_Employees")
                arColumnName.Add("PI_VC10_Company_Type")
                arColumnName.Add("PI_VC8_Currency")
                'Invoice related fields
                arColumnName.Add("PI_VC70_Bene_Name")
                arColumnName.Add("PI_VC30_Account_Number")
                arColumnName.Add("PI_VC70_Bank_Name")
                arColumnName.Add("PI_VC256_Bank_Address")
                arColumnName.Add("PI_VC30_Swift_Code")
                arColumnName.Add("PI_VC30_Routing_Number")
                arColumnName.Add("PI_VC256_MICR")
                arColumnName.Add("PI_VC256_IEC_Number")
                arColumnName.Add("PI_CH1_Mail_Comm")
                arColumnName.Add("PI_CH1_Mail_Sent")
                arColumnName.Add("PI_VC100_Resume") 'added resume column
                arColumnName.Add("PI_VC8_Department")
                arColumnName.Add("PI_NU9_Manager")
                arColumnName.Add("PI_NU9_JobRole") 'JoB role Feb2009
                arColumnName.Add("PI_VC15_BloodGroup") '''''Blood group Added on Nov 25'09 by tarun.


                arRowData.Add(CInt(ViewState("SAddressNumber_AddressBook")))
                arRowData.Add(txtFirstName.Text.Trim)
                arRowData.Add(txtMiddleName.Text.Trim)
                arRowData.Add(txtLastName.Text.Trim)
                arRowData.Add(ddGender.SelectedItem.Text)
                arRowData.Add(ddMartialStatus.SelectedItem.Text)
                arRowData.Add(IIf(dtDOB.Text.Trim = "", DBNull.Value, dtDOB.Text.Trim))

                ' arRowData.Add(txtRoleName.Text.Trim.ToUpper)

                If txtAB_Type.Text = "COM" Then
                    arRowData.Add(ddlWHFrom.SelectedItem.Text)
                    arRowData.Add(ddlWHTo.SelectedItem.Text)
                Else
                    arRowData.Add(ddlWHrFrom.SelectedItem.Text)
                    arRowData.Add(ddlWHrTo.SelectedItem.Text)
                End If


                arRowData.Add(ddlTZ.SelectedValue)
                arRowData.Add(txtpath.Text)
                arRowData.Add(IIf(dtDOJ.Text.Trim = "", DBNull.Value, dtDOJ.Text.Trim))
                arRowData.Add(IIf(dtDOL.Text.Trim = "", DBNull.Value, dtDOL.Text.Trim))

                arRowData.Add(IIf(dtOD.Text.Trim = "", DBNull.Value, dtOD.Text.Trim))
                arRowData.Add(IIf(txtNoEmp.Text.Trim = "", DBNull.Value, txtNoEmp.Text.Trim))
                arRowData.Add(IIf(txtCompType.Text.Trim = "", DBNull.Value, txtCompType.Text.Trim))
                arRowData.Add(IIf(txtCurrency.Text.Trim = "", DBNull.Value, txtCurrency.Text.Trim))
                arRowData.Add(txtBenName.Text.Trim)
                arRowData.Add(txtAccount.Text.Trim)
                arRowData.Add(txtBankName0.Text.Trim)
                arRowData.Add(txtBankAdd.Text.Trim)
                arRowData.Add(txtSwiftCode.Text.Trim)
                arRowData.Add(txtRoutNo.Text.Trim)
                arRowData.Add(txtMICR.Text.Trim)
                arRowData.Add(txtICENo.Text.Trim)
                If txtAB_Type.Text.Equals("COM") Then
                    arRowData.Add(IIf(chkCMailComm.Checked = True, "1", "0"))
                Else
                    arRowData.Add(IIf(chkEMailComm.Checked = True, "1", "0"))
                End If
                arRowData.Add("F")
                arRowData.Add(txtResumePath.Text.Trim)
                arRowData.Add(txtDept.Text.Trim)
                arRowData.Add(IIf(Val(txtMgr.Text.Trim) = 0, System.DBNull.Value, Val(txtMgr.Text.Trim)))
                arRowData.Add(ddlJob.Text)
                arRowData.Add(ddlbloodgroup.Text)
                mstGetFunctionValue = WSSSave.SavePersonalInfo(arColumnName, arRowData)
            Else
                ' Update the Information

                arColumnName.Add("PI_VC36_First_Name")
                arColumnName.Add("PI_VC36_Middle_Name")
                arColumnName.Add("PI_VC36_Last_Name")
                arColumnName.Add("PI_VC8_Sex")
                arColumnName.Add("PI_VC8_Marital_Status")
                arColumnName.Add("PI_DT8_Date_Of_Birth")
                'arColumnName.Add("PI_VC30_Role")
                arColumnName.Add("PI_VC8_WHr_From")
                arColumnName.Add("PI_VC8_WHr_To")
                arColumnName.Add("PI_VC4_TimeZone")
                arColumnName.Add("PI_VC4_Picture")
                arColumnName.Add("PI_DT8_Date_Of_Joining")
                arColumnName.Add("PI_DT8_Date_Of_Leaving")

                arColumnName.Add("PI_DT8_Open_date")
                arColumnName.Add("PI_NU9_Total_Employees")
                arColumnName.Add("PI_VC10_Company_Type")
                arColumnName.Add("PI_VC8_Currency")
                'Invoice related fields
                arColumnName.Add("PI_VC70_Bene_Name")
                arColumnName.Add("PI_VC30_Account_Number")
                arColumnName.Add("PI_VC70_Bank_Name")
                arColumnName.Add("PI_VC256_Bank_Address")
                arColumnName.Add("PI_VC30_Swift_Code")
                arColumnName.Add("PI_VC30_Routing_Number")
                arColumnName.Add("PI_VC256_MICR")
                arColumnName.Add("PI_VC256_IEC_Number")
                arColumnName.Add("PI_CH1_Mail_Comm")
                arColumnName.Add("PI_CH1_Mail_Sent")
                arColumnName.Add("PI_VC100_Resume")
                arColumnName.Add("PI_VC8_Department")
                arColumnName.Add("PI_NU9_Manager")
                arColumnName.Add("PI_NU9_JobRole") 'JoB role Feb2009
                arColumnName.Add("PI_VC15_BloodGroup") ''''''' Blood group added by tarun on 25 Nov 09
                arRowData.Add(txtFirstName.Text.Trim)
                arRowData.Add(txtMiddleName.Text.Trim)
                arRowData.Add(txtLastName.Text.Trim)
                arRowData.Add(ddGender.SelectedItem.Text)
                arRowData.Add(ddMartialStatus.SelectedItem.Text)
                arRowData.Add(IIf(dtDOB.Text.Trim = "", DBNull.Value, dtDOB.Text.Trim))
                'arRowData.Add(txtRoleName.Text.Trim.ToUpper)

                If txtAB_Type.Text = "COM" Then
                    arRowData.Add(ddlWHFrom.SelectedItem.Text)
                    arRowData.Add(ddlWHTo.SelectedItem.Text)
                Else
                    arRowData.Add(ddlWHrFrom.SelectedItem.Text)
                    arRowData.Add(ddlWHrTo.SelectedItem.Text)
                End If

                arRowData.Add(ddlTZ.SelectedValue)
                arRowData.Add(txtpath.Text)
                arRowData.Add(IIf(dtDOJ.Text.Trim = "", DBNull.Value, dtDOJ.Text.Trim))
                arRowData.Add(IIf(dtDOL.Text.Trim = "", DBNull.Value, dtDOL.Text.Trim))

                arRowData.Add(IIf(dtOD.Text.Trim = "", DBNull.Value, dtOD.Text.Trim)) ' Company Opened Date
                arRowData.Add(IIf(txtNoEmp.Text.Trim = "", DBNull.Value, txtNoEmp.Text.Trim))
                arRowData.Add(IIf(txtCompType.Text.Trim = "", DBNull.Value, txtCompType.Text.Trim))
                arRowData.Add(IIf(txtCurrency.Text.Trim = "", DBNull.Value, txtCurrency.Text.Trim))
                arRowData.Add(txtBenName.Text.Trim)
                arRowData.Add(txtAccount.Text.Trim)
                arRowData.Add(txtBankName0.Text.Trim)
                arRowData.Add(txtBankAdd.Text.Trim)
                arRowData.Add(txtSwiftCode.Text.Trim)
                arRowData.Add(txtRoutNo.Text.Trim)
                arRowData.Add(txtMICR.Text.Trim)
                arRowData.Add(txtICENo.Text.Trim)
                If txtAB_Type.Text.Equals("COM") Then
                    arRowData.Add(IIf(chkCMailComm.Checked = True, "1", "0"))
                Else
                    arRowData.Add(IIf(chkEMailComm.Checked = True, "1", "0"))
                End If
                arRowData.Add("F")
                arRowData.Add(txtResumePath.Text.Trim) 'added resume field
                arRowData.Add(txtDept.Text.Trim)
                arRowData.Add(IIf(Val(txtMgr.Text.Trim) = 0, System.DBNull.Value, Val(txtMgr.Text.Trim)))
                arRowData.Add(ddlJob.Text)
                arRowData.Add(ddlbloodgroup.Text)
                mstGetFunctionValue = WSSUpdate.UpdatePersonalInfo(CInt(ViewState("SAddressNumber_AddressBook")), arColumnName, arRowData)
                If mstGetFunctionValue.ErrorCode = 0 Then

                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                Else

                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                End If
                arColumnName.Clear()
                arRowData.Clear()
            End If
        End If
        If mstGetFunctionValue.ErrorCode = 0 Then

            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            SavePersonalInfo = True
        Else

            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            SavePersonalInfo = False
        End If
    End Function

    Private Function SaveCategoryInfo() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim intRecords As Int32
        If ViewState("SAddressNumber_AddressBook") <> 0 Then
            intRecords = SQL.Search("AB_Main", "SavecategoryInfo-1237", "Select CC_NU8_Address_No  From T010053 Where CC_NU8_Address_No=" & ViewState("SAddressNumber_AddressBook"))
            If intRecords = 0 Then
                arColumnName.Add("CC_NU8_Address_No")
                arColumnName.Add("CC_VC8_Category_Code1")
                arColumnName.Add("CC_VC8_Category_Code2")
                arColumnName.Add("CC_VC8_Category_Code3")
                arColumnName.Add("CC_VC8_Category_Code4")
                arColumnName.Add("CC_VC8_Category_Code5")
                arColumnName.Add("CC_VC8_Category_Code6")
                arColumnName.Add("CC_VC8_Category_Code7")
                arColumnName.Add("CC_VC8_Category_Code8")
                arColumnName.Add("CC_VC8_Category_Code9")
                arColumnName.Add("CC_VC8_Category_Code10")

                arRowData.Add(CInt(ViewState("SAddressNumber_AddressBook")))
                arRowData.Add(txtCatCode1.Text.Trim)
                arRowData.Add(txtCatCode2.Text.Trim)
                arRowData.Add(txtCatCode3.Text.Trim)
                arRowData.Add(txtCatCode4.Text.Trim)
                arRowData.Add(txtCatCode5.Text.Trim)
                arRowData.Add(txtCatCode6.Text.Trim)
                arRowData.Add(txtCatCode7.Text.Trim)
                arRowData.Add(txtCatCode8.Text.Trim)
                arRowData.Add(txtCatCode9.Text.Trim)
                arRowData.Add(txtCatCode10.Text.Trim)

                mstGetFunctionValue = WSSSave.SaveCategory(arColumnName, arRowData)

            Else

                ' Update the Information

                arColumnName.Add("CC_VC8_Category_Code1")
                arColumnName.Add("CC_VC8_Category_Code2")
                arColumnName.Add("CC_VC8_Category_Code3")
                arColumnName.Add("CC_VC8_Category_Code4")
                arColumnName.Add("CC_VC8_Category_Code5")
                arColumnName.Add("CC_VC8_Category_Code6")
                arColumnName.Add("CC_VC8_Category_Code7")
                arColumnName.Add("CC_VC8_Category_Code8")
                arColumnName.Add("CC_VC8_Category_Code9")
                arColumnName.Add("CC_VC8_Category_Code10")

                arRowData.Add(txtCatCode1.Text.Trim)
                arRowData.Add(txtCatCode2.Text.Trim)
                arRowData.Add(txtCatCode3.Text.Trim)
                arRowData.Add(txtCatCode4.Text.Trim)
                arRowData.Add(txtCatCode5.Text.Trim)
                arRowData.Add(txtCatCode6.Text.Trim)
                arRowData.Add(txtCatCode7.Text.Trim)
                arRowData.Add(txtCatCode8.Text.Trim)
                arRowData.Add(txtCatCode9.Text.Trim)
                arRowData.Add(txtCatCode10.Text.Trim)

                mstGetFunctionValue = WSSUpdate.UpdateCategoryInfo(CInt(ViewState("SAddressNumber_AddressBook")), arColumnName, arRowData)
                If mstGetFunctionValue.ErrorCode = 0 Then

                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                Else

                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                End If
                arColumnName.Clear()
                arRowData.Clear()
            End If
        End If
        If mstGetFunctionValue.ErrorCode = 0 Then

            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            SaveCategoryInfo = True
        Else

            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            SaveCategoryInfo = False
        End If
    End Function

#Region "Save"

    Function SaveUserProfile() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim AbID As Integer = ViewState("SAddressNumber_AddressBook")

        Try

            arColumnName.Add("UM_IN4_Address_No_FK")         '**********AdressBookID
            arColumnName.Add("UM_VC50_UserID")
            arColumnName.Add("UM_VC30_Password")
            arColumnName.Add("UM_IN4_Expiry_Days")
            arColumnName.Add("UM_IN4_LockOut_Tries")
            arColumnName.Add("UM_CH1_IsDefault")
            arColumnName.Add("UM_VC4_User_Type_FK")
            arColumnName.Add("UM_DT8_From_date")
            arColumnName.Add("UM_DT8_To_date")
            arColumnName.Add("UM_DT8_Created_Date")
            arColumnName.Add("UM_VC4_Status_Code_FK")
            arColumnName.Add("UM_DT8_Status_Date")
            arColumnName.Add("UM_SI2_Inserted_By")
            arColumnName.Add("UM_DT8_Inserted_On")
            arColumnName.Add("UM_VC15_Inserted_By_System_Code")
            arColumnName.Add("UM_IN4_Company_AB_ID")
            arColumnName.Add("UM_CH1_Admin_Rights")
            arColumnName.Add("UM_CH1_Mail_Comm")
            arColumnName.Add("UM_NU9_Expiry_Mail_Days")
            arColumnName.Add("UM_CH1_Mail_Sent_Modify")
            arColumnName.Add("UM_CH1_Mail_Sent_Expiry")

            arRowData.Add(ViewState("SAddressNumber_AddressBook"))        '***************AdressBookID
            arRowData.Add(txtUserProfileID.Text.Trim)
            arRowData.Add(IONEncrypt(Guid.NewGuid.ToString.Substring(0, 8)))

            Dim intCompId As Int32 = Val(txtBr.Text.Trim)    'getUserCompany(ABID)
            arRowData.Add(365)
            arRowData.Add(3)
            arRowData.Add("N")
            arRowData.Add("INTR")
            arRowData.Add(Now.ToShortDateString)
            arRowData.Add(Now.AddYears(1))
            arRowData.Add(Now.ToShortDateString)
            arRowData.Add("ENB")  'status
            arRowData.Add(Now)
            arRowData.Add(Val(Session("PropUserID")))
            arRowData.Add(Now.ToShortDateString)
            arRowData.Add(Request.UserHostName())
            arRowData.Add(intCompId)
            arRowData.Add(0) 'Admin Rights
            arRowData.Add(1) 'Mail
            arRowData.Add(3)
            arRowData.Add("T")
            arRowData.Add("T")


            mstGetFunctionValue = InsertData(arColumnName, arRowData, "T060011")

            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
            'End If

        Catch ex As Exception
            lstError.Items.Add("server is busy please try later ...")
            CreateLog("CraeteUser", "SaveUser-233", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

    Function InsertData(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' Table name
            'SQL.DBTable = TableName
            SQL.DBTracing = False

            If SQL.Save(TableName, "User Manage", "insertData-601", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                SaveDefaultCompany()
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveUser-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Private Function CheckUser() As Boolean
        Dim intAddressID As Integer = SQL.Search("UserManage", " checkUser-620", "Select UM_IN4_Address_No_FK from T060011 where UM_VC50_UserID='" & txtUserProfileID.Text.Trim & "'")
        If intAddressID > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Function GetUserCompany(ByVal ABID As Integer) As Int32

        Return Convert.ToInt32(SQL.Search("UserManage", "getUserCompany", "select CI_IN4_Business_Relation from T010011 where CI_NU8_Address_Number = " & ABID))

    End Function

    Function CheckID(ByVal userID As String) As Boolean

        Dim intCompId As Int32 = GetUserCompany(userID)
        Dim intAddressID As Integer = SQL.Search("UserManage", "checkID", "Select UM_IN4_Address_No_FK from T060011 where UM_VC50_UserID='" & userID & "'")

        If intAddressID > 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Private Function SaveDefaultCompany() As Boolean
        Try

            Dim arrColName As New ArrayList
            Dim arrRowData As New ArrayList

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            arrColName.Add("UC_NU9_User_ID_FK")
            arrColName.Add("UC_NU9_Comp_ID_FK")
            arrColName.Add("UC_BT1_Access")
            arrColName.Add("UC_DT8_Insert_Date")
            arrColName.Add("UC_NU9_Inserted_By_FK")
            arrColName.Add("UC_VC50_Inserted_By_IP")
            arrRowData.Add(ViewState("SAddressNumber_AddressBook"))
            arrRowData.Add(Val(txtBr.Text.Trim)) 'getUserCompany(ViewState("SAddressNumber_AddressBook")))
            arrRowData.Add(True)
            arrRowData.Add(Now)
            arrRowData.Add(Session("PropUserID"))
            arrRowData.Add(Request.UserHostAddress)

            SQL.Save("T060041", "UserManage", "SaveDefaultCompany-976", arrColName, arrRowData)

        Catch ex As Exception
            CreateLog("UserManage", "SaveCompanyList-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function
#End Region
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Add(Msg)
        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub
#End Region

    Function uploadImage() As Boolean
        If Not upload.PostedFile Is Nothing And upload.PostedFile.ContentLength > 0 Then
            Dim fn As String = System.IO.Path.GetFileName(upload.PostedFile.FileName)
            Dim strPath As String = Server.MapPath("../../Dockyard/ABImages")
            Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("SAddressNumber_AddressBook"))
            If objFolder.Exists = False Then
                ' Then Create the folder for that Resume.
                objFolder.Create()
            End If

            Try

                If File.Exists(Server.MapPath("..\..\Dockyard\ABImages") & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn) Then
                    File.Delete(Server.MapPath("..\..\Dockyard\ABImages") & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn)
                End If


                Dim SaveLocation As String = Server.MapPath("..\..\Dockyard\ABImages") & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn

                upload.PostedFile.SaveAs(SaveLocation)

                With imgDesign
                    ' .ImageUrl = "..\..\Dockyard\ABImages\" & fn
                    .ImageUrl = "..\..\Dockyard\ABImages" & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn
                End With
                'txtpath.Text = "..\..\Dockyard\ABImages\" & fn
                txtpath.Text = "..\..\Dockyard\ABImages" & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn
                lblImageName.Text = fn

            Catch ex As Exception
                DisplayError("Problem in to upload image plz try later or contact with Technical Team...")
                CreateLog("AB_Main", "uploadImage-2038", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdskillset", )
            End Try
        Else
            '"Please select a file"
        End If

    End Function

    Sub removeImage()
        txtpath.Text = ""
        lblImageName.Text = ""
        imgDesign.ImageUrl = ""
    End Sub
    Function uploadResumeFile() As Boolean

        If Not UploadResume.PostedFile Is Nothing And UploadResume.PostedFile.ContentLength > 0 Then
            Dim fn As String = System.IO.Path.GetFileName(UploadResume.PostedFile.FileName)
            '            Dim Ext = upload.Accept
            Dim strPath As String = Server.MapPath("../../Dockyard/ABResumes")
            Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("SAddressNumber_AddressBook"))
            If objFolder.Exists = False Then
                ' Then Create the folder for that Resume.
                objFolder.Create()
            End If

            Try

                Dim SaveLocation As String = Server.MapPath("..\..\Dockyard\ABResumes") & "\" & ViewState("SAddressNumber_AddressBook") & "\" & fn

                UploadResume.PostedFile.SaveAs(SaveLocation)
                'txtResumePath.Text = "..\..\Dockyard\ABResumes\" & fn
                txtResumePath.Text = "../../Dockyard/ABResumes" & "/" & ViewState("SAddressNumber_AddressBook") & "/" & fn  ''''' change path for download
                '  lblResumeName.Text = fn
                hypresume.Text = fn
                hypresume.NavigateUrl = txtResumePath.Text

                lstError.Items.Clear()
                lstError.Items.Add("Resume Uploaded Successfuly...")
                '                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Catch ex As Exception
                DisplayError("Resume not uploaded plz Try Later...")
                CreateLog("AB_Main", "uploadResumeFile-1971", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdskillset", )
            End Try
        Else
            '"Please select a file"
        End If
    End Function
    Sub removeResume()
        txtResumePath.Text = ""
        hypresume.Text = ""
    End Sub

    Function UPdateResume()
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        arColumnName.Add("PI_VC100_Resume")
        arRowData.Add(txtResumePath.Text.Trim)

        mstGetFunctionValue = WSSUpdate.UpdatePersonalInfo(CInt(ViewState("SAddressNumber_AddressBook")), arColumnName, arRowData)
        If mstGetFunctionValue.ErrorCode = 0 Then
            lstError.Items.Clear()
            lstError.Items.Add("Resume Uploaded Successfuly...")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            cpnlPersonalInfo.Enabled = True
            cpnlPersonalInfo.State = CustomControls.Web.PanelState.Expanded
        Else

        End If
        arColumnName.Clear()
        arRowData.Clear()
    End Function
    Function DelateResume()
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        arColumnName.Add("PI_VC100_Resume")
        arRowData.Add("")

        mstGetFunctionValue = WSSUpdate.UpdatePersonalInfo(CInt(ViewState("SAddressNumber_AddressBook")), arColumnName, arRowData)
        If mstGetFunctionValue.ErrorCode = 0 Then
            lstError.Items.Clear()
            lstError.Items.Add("Resume Deleted Successfuly...")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Else

        End If
        arColumnName.Clear()
        arRowData.Clear()
    End Function

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

    Protected Sub txtQuery1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQuery1.TextChanged

    End Sub
End Class
