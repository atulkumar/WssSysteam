'*******************************************************************************************************
' Page                 : - ProjectMasterDetail
' Purpose              : - 
' Tables used          : - t060011,t010011,T070031,t060022,
' Date					   Author						    Modification Date				Description
' 20/07/06			   Ranvijay/Harpreet   	18/12/07                			Modified
'
' Notes: 
' Code:
'*******************************************************************************************************
Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Data

Partial Class AdministrationCenter_Project_ProjectMasterDetail
    Inherits System.Web.UI.Page
#Region " From Level Variables "
    Private Shared arrColumnsNameMember As New ArrayList ' Array to store ColumnNames
    Private tclMember() As TemplateColumn 'Object of template column
    Private Shared dtvMember As New DataView ' Dataview of member table
    Private Shared arrHeadersMember As New ArrayList
    Private Shared arrFooterMember As New ArrayList
    Private Shared arrWidthMember As New ArrayList ' 
    Private Shared arrColumnsWidthMember As New ArrayList
    Dim mMemberRowValue As Integer 'Keep rowselected
    Private Shared mintMemberID As Int32 'Keep Selected Member's Sno
    Dim boolFunctionCallStatus As Boolean = False ' this will keep track whether the save function is called once
    Dim strSqlWhere As String ' this will keep where conditions
    Protected WithEvents dtgMember As New System.Web.UI.WebControls.DataGrid
#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strSqlFilter As String = " "
        Dim e1 As New System.Web.UI.ImageClickEventArgs(1, 2)
        Try
            ' Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(Session.Timeout) * 60) & ";Login.aspx"" />")
            If Not IsPostBack Then
                lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
                txtBudget.Attributes.Add("onkeypress", "NumericOnly();")
                ddlCustomer.Attributes.Add("OnChange", "ForcedPostBack();")
                ddlMember_F.Attributes.Add("onChange", "MemberChange();")
                DDLReportsTo_F.Attributes.Add("onkeypress", "DoSubmit();")
                DDLRole_F.Attributes.Add("onkeypress", "DoSubmit();")
                ddlMember_F.Attributes.Add("onkeypress", "MemberChange();")
                imgOk.Attributes.Add("onclick", "return CheckLength();")
                imgbtnSearch.Attributes.Add("onclick", "return CheckLength();")
                txtComment.Attributes.Add("onmousemove", "ShowToolTip(this,200);")
                imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            End If

            ''''''added on 12 Aug by Tarun so as to Remove Sessions and using ViewState... 
            If Not IsNothing(Request.QueryString("ProjectID")) And Request.QueryString("ProjectID") <> "-1" And ViewState("PropProjectID") <> "-1" Then
                ViewState("PropProjectID") = Request.QueryString("ProjectID")
            Else
                If IsNothing(ViewState("PropProjectID")) = True Then
                    ViewState("PropProjectID") = "-1"
                    ' Or ViewState("PropProjectID") <> "-1" Then
                Else
                End If
            End If

            If IsNumeric(Request.QueryString("CompanyID")) = True Then
                ViewState("PropCAComp") = Request.QueryString("CompanyID")
            Else
                If IsNothing(Request.QueryString("CompanyID")) = False And Request.QueryString("CompanyID") <> "" Then
                    ViewState("PropCAComp") = WSSSearch.SearchCompName(Request.QueryString("CompanyID")).ExtraValue
                Else
                    ViewState("PropCAComp") = WSSSearch.SearchCompName(Session("PropCompany")).ExtraValue
                End If
            End If
            If ViewState("PropCAComp").ToString() = "" And ddlCustomer.SelectedValue <> "" Then
                ViewState("PropCAComp") = ddlCustomer.SelectedValue
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            boolFunctionCallStatus = False 'Initialize Flag
            If Request.Form("txthiddenImage") = "forced" Then
                boolFunctionCallStatus = True
            Else
                boolFunctionCallStatus = False
            End If
            mintMemberID = Val(Request.Form("txthidden")) ' -- get selected member
            'Set Prop
            txtCSS(Me.Page, "cpnlMember")
            If Not IsPostBack Then

                If Val(ViewState("PropProjectID")) = -1 Then ViewState("PropCAComp") = Session("PropCompanyID")
            End If
            Call createDataTableMember(strSqlFilter)
            Call CreateGridMember()
            Call FillHeaderArrayMember()
            Call createTemplateColumnsMember()
            Dim txthiddenImage As String
            txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select
            End If
            If IsPostBack Then
                FillAjaxDropDown(DDLRole_F, Request.Form("txthiddenRole"), "cpnlMember$" & DDLRole_F.ID, New ListItem("", ""))
                FillAjaxDropDown(DDLReportsTo_F, Request.Form("txthiddenReportsTo"), "cpnlMember$" & DDLReportsTo_F.ID, New ListItem("", ""))
            Else
                DDLRole_F.Items.Clear()
                DDLReportsTo_F.Items.Clear()
                DDLRole_F.Items.Add("")
                DDLReportsTo_F.Items.Add("")
            End If

            If Page.IsPostBack = False Then

                imgSave.Attributes.Add("Onclick", "return saveclicked();")
                '  imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
                imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
                imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
                'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
                imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit','" & ViewState("PropProjectID") & "','" & ViewState("PropCAComp") & "');")


                '-- Fill DDLCustomer
                FillNonUDCDropDown(ddlCustomer, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from  T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' AND CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ")")
                '-- Fill DDLMember Combo
                ddlMember_F.Items.Add("")
                'FillNonUDCDropDown(ddlMember_F, "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select ci_nu8_address_number from t010011 where ci_vc8_status='ENA') and  um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='SCM') and UM_VC4_Status_Code_FK='ENB'  Order By Name", True)

                If Val(ViewState("PropCAComp")) > 0 Then
                    ddlCustomer.SelectedValue = ViewState("PropCAComp")
                Else
                    ddlCustomer.SelectedValue = Session("PropCompanyID")
                End If
                FillCustomDDl(False)
            Else           '-- If IsPostBack is true
                ''Problem with this code bcoz if we press the enter then we donot know the enter is pressed for serach or for save the records
                'If txthiddensaveclicked.Value <> "save" Then ' If Save image button is not clicked then call imgsaveclick
                '    imgSave_Click("Page_Load", e1)
                '    txthiddensaveclicked.Value = ""
                'End If
            End If


            '-- Preparing Filter query String for Action Grid
            Dim intCount As Int16
            Dim strSearchControlMember As String
            If ViewState("PropProjectID") > 0 Then
                For intCount = 0 To dtvMember.Table.Columns.Count - 2
                    strSearchControlMember = Request.Form("cpnlMember$dtgMember$ctl01$" + dtvMember.Table.Columns(intCount).ColumnName + "_H")
                    If IsNothing(strSearchControlMember) = False Then
                        If Not strSearchControlMember.Trim.Equals("") And Not IsDBNull(strSearchControlMember) Then
                            ' -- Format Search String
                            strSearchControlMember = mdlMain.GetSearchString(strSearchControlMember)

                            If dtvMember.Table.Columns(intCount).DataType.FullName = "System.Decimal" Or dtvMember.Table.Columns(intCount).DataType.FullName = "System.Int32" Or dtvMember.Table.Columns(intCount).DataType.FullName = "System.Int16" Or dtvMember.Table.Columns(intCount).DataType.FullName = "System.DateTime" Then
                                If dtvMember.Table.Columns(intCount).DataType.FullName = "System.DateTime" Then
                                    Dim chk As Date
                                    If IsDate(strSearchControlMember) Then
                                    Else
                                        'LblErrMsg.Text = " Check Your Date Format First"
                                        Exit For
                                    End If
                                    strSqlFilter = strSqlFilter & " Cast(DateDiff(d, 0, " & dtvMember.Table.Columns(intCount).ColumnName & ")  As DateTime) " & " = '" & strSearchControlMember & "' And "
                                Else
                                    strSearchControlMember = strSearchControlMember.Replace("*", "")
                                    strSqlFilter = strSqlFilter & dtvMember.Table.Columns(intCount).ColumnName & " = '" & strSearchControlMember & "' AND "
                                End If
                            Else
                                strSearchControlMember = strSearchControlMember.Replace("*", "%")
                                strSqlFilter = strSqlFilter & dtvMember.Table.Columns(intCount).ColumnName & " like " & "'" & strSearchControlMember & "' AND "
                            End If
                        End If
                    End If
                Next
            End If

            '  If ViewState("PropProjectID") <> -1 And IsPostBack = False Then
            If IsPostBack = False Then
                '-- Fill SubCategory Info if Form is opened in edit mode
                FillData()
                FillNonUDCDropDown(ddlMember_F, "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ddlCustomer.SelectedValue) & " and UC_BT1_Access=1)  Order By Name", True)

            End If
            '   strhiddenTable = Request.Form("txthiddenImage")

            If ViewState("PropProjectID") > 0 Then
                If Not strSqlFilter.Trim.Equals("") Then
                    strSqlFilter = strSqlFilter.Remove((strSqlFilter.Length - 4), 4)
                End If
            End If

            Call createDataTableMember(strSqlFilter)
            strSqlWhere = strSqlFilter
            Call BindGridMember()
            ' -- Enable/Disable Panels
            If ViewState("PropProjectID") = -1 Then
                EnableControls()
                cpnlOverview.State = CustomControls.Web.PanelState.Collapsed
                cpnlOverview.Enabled = False
                cpnlMember.State = CustomControls.Web.PanelState.Collapsed
                cpnlMember.Enabled = False
                cpnlOverview.CssClass = "test2"
                cpnlMember.CssClass = "test2"
            Else
                DisableControls()
                cpnlOverview.State = CustomControls.Web.PanelState.Expanded
                cpnlOverview.Enabled = True
                cpnlMember.State = CustomControls.Web.PanelState.Expanded
                cpnlMember.Enabled = True
                cpnlOverview.CssClass = "test2"
                cpnlMember.CssClass = "test2"
            End If



            'Security Block
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(670) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, 670)
                'End of Security Block
            End If

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "PageLoad-165", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub FillData()
        '--Function to fill SubCategory Info
        Dim sqrdProjectInfo As SqlDataReader
        Dim FlgStatus As Boolean
        Dim strSql As String
        Try
            strSql = "Select PR.*,PO.UM_VC50_UserID as ProjectOwner,CB.UM_VC50_UserID As CreatedBy, PR.PR_VC20_Name as Parent From T210011 PR,T060011 PO,T060011 CB " & _
                " Where PR_NU9_Project_ID_Pk=" & ViewState("PropProjectID") & " And PR_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & _
                " And  PR.PR_NU9_Owner_ID_Fk=PO.UM_IN4_Address_No_Fk  And Pr.PR_NU9_Project_CreatedBy_Fk=CB.UM_IN4_Address_No_Fk "

            sqrdProjectInfo = SQL.Search("ProjectMasterDetail", "FillData", strSql, SQL.CommandBehaviour.CloseConnection, FlgStatus)
            If FlgStatus = True Then
                If sqrdProjectInfo.HasRows Then
                    sqrdProjectInfo.Read()
                    ddlCustomer.SelectedValue = sqrdProjectInfo.Item("PR_NU9_Comp_ID_FK")
                    txtProjectName.Text = sqrdProjectInfo.Item("PR_VC20_Name")
                    cddlProjectType.CDDLSetSelectedItem(sqrdProjectInfo.Item("PR_VC8_Type"))
                    cddlParentProject.CDDLSetSelectedItem(sqrdProjectInfo.Item("PR_NU9_Parent_ID_FK"), False, sqrdProjectInfo.Item("Parent"))
                    cddlProjectOwner.CDDLSetSelectedItem(sqrdProjectInfo.Item("PR_NU9_Owner_ID_Fk"), False, sqrdProjectInfo.Item("ProjectOwner"))
                    dtProjectStartDate.Text = SetDateFormat(sqrdProjectInfo.Item("PR_DT8_Start_Date"), mdlMain.IsTime.DateOnly)

                    If IsDBNull(sqrdProjectInfo.Item("PR_DT8_Close_Date")) = False Then
                        If Not CStr(IsDBNull(sqrdProjectInfo.Item("PR_DT8_Close_Date"))).Equals("") Then
                            dtClosedDate.Text = SetDateFormat(sqrdProjectInfo.Item("PR_DT8_Close_Date"), mdlMain.IsTime.DateOnly)
                        End If
                    Else
                        dtClosedDate.Text = ""
                    End If
                    cddlStatus.CDDLSetSelectedItem(sqrdProjectInfo.Item("PR_VC8_Status"))
                    If IsDBNull(sqrdProjectInfo.Item("PR_VC200_Comment")) = False Then
                        txtComment.Text = sqrdProjectInfo.Item("PR_VC200_Comment")
                    Else
                        txtComment.Text = ""
                    End If
                    If IsDBNull(sqrdProjectInfo.Item("PR_VC50_Cat_Code1")) = False Then
                        txtCat1.Text = sqrdProjectInfo.Item("PR_VC50_Cat_Code1")
                    Else
                        txtCat1.Text = ""
                    End If
                    If IsDBNull(sqrdProjectInfo.Item("PR_VC50_Cat_Code2")) = False Then
                        txtCat2.Text = sqrdProjectInfo.Item("PR_VC50_Cat_Code2")
                    Else
                        txtCat2.Text = ""
                    End If

                    If IsDBNull(sqrdProjectInfo.Item("PR_VC200_Description")) = False Then
                        txtDescription.Text = sqrdProjectInfo.Item("PR_VC200_Description")
                    Else
                        txtDescription.Text = ""
                    End If
                    If IsDBNull(sqrdProjectInfo.Item("PR_VC200_Key_Deliverance")) = False Then
                        txtKeyDeliverance.Text = sqrdProjectInfo.Item("PR_VC200_Key_Deliverance")
                    Else
                        txtKeyDeliverance.Text = ""
                    End If
                    txtBudget.Text = sqrdProjectInfo.Item("PR_MN8_Budget")
                End If
                sqrdProjectInfo.Close()
            End If
            cddlStatus.CDDLSetItem()
            cddlParentProject.CDDLSetItem()
            cddlProjectOwner.CDDLSetItem()
            cddlProjectType.CDDLSetItem()

            '--Disable Controls if Form is opened in Edit Mode
            '  DisableControls()
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "PageLoad-165", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))

        End Try
    End Sub

#Region " Save Data "
    Private Function SaveProjectInfo() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim StucReturn As ReturnValue


        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            lstError.Items.Clear()
            'cpnlError.Text = "Message..."
            lstError.Items.Add("Your Role does not have rights to save SubCategory and Member...")
            ' mshFlag = 0
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)

            Exit Function
        End If
        'End of Security Block



        If ValidateRecords() = False Then
            Exit Function
        End If
        Try
            arColumnName.Add("PR_NU9_Comp_ID_FK")
            arColumnName.Add("PR_VC20_Name")
            arColumnName.Add("PR_VC8_Type")
            arColumnName.Add("PR_NU9_Parent_ID_Fk")
            arColumnName.Add("PR_NU9_Owner_ID_Fk")
            arColumnName.Add("PR_DT8_Start_Date")
            arColumnName.Add("PR_DT8_Close_Date")
            arColumnName.Add("PR_VC8_Status")
            arColumnName.Add("PR_VC200_Comment")
            arColumnName.Add("PR_VC50_Cat_Code1")
            arColumnName.Add("PR_VC50_Cat_Code2")
            arColumnName.Add("PR_VC200_Description")
            arColumnName.Add("PR_VC200_Key_Deliverance")
            arColumnName.Add("PR_MN8_Budget")
            '----
            If ViewState("PropProjectID") = -1 Then ' If new SubCategory then insert creation date else insert modification date
                arColumnName.Add("PR_DT_Project_Created_On")
                arColumnName.Add("PR_NU9_Project_CreatedBy_Fk")
            Else
                arColumnName.Add("PR_DT_Project_Modified_On")
                arColumnName.Add("PR_NU9_Project_ModifiedBy_Fk")
            End If

            arRowData.Add(ddlCustomer.SelectedValue)
            arRowData.Add(txtProjectName.Text)
            arRowData.Add(cddlProjectType.CDDLGetValue)
            arRowData.Add(Val(cddlParentProject.CDDLGetValue))
            arRowData.Add(cddlProjectOwner.CDDLGetValue)
            arRowData.Add(dtProjectStartDate.Text)
            arRowData.Add(dtClosedDate.Text)
            arRowData.Add(cddlStatus.CDDLGetValue)
            arRowData.Add(txtComment.Text.Trim)
            arRowData.Add(txtCat1.Text)
            arRowData.Add(txtCat2.Text)
            arRowData.Add(txtDescription.Text.Trim)
            arRowData.Add(txtKeyDeliverance.Text.Trim)
            arRowData.Add(Val(txtBudget.Text))
            '--In both cases i.e new SubCategory and old SubCategory value will remain same only columns in which to insert will be changed 
            'for creation and modification date
            arRowData.Add(Now)
            arRowData.Add(Session("PropUserID"))


            If Val(ViewState("PropProjectID")) = -1 Then '--Add new Record
                Dim intMaxProjectInfoID As Int32 ' --To Store ProjectInfo ID
                arColumnName.Add("PR_NU9_Project_ID_Pk")
                ' --Get Max SubCategory Info ID from  Table T210011
                intMaxProjectInfoID = SQL.Search("ProjectMasterDetail", "SaveProjectInfo", "Select isnull(Max(PR_NU9_Project_ID_Pk),0) From T210011 Where PR_NU9_Comp_ID_FK=" & ddlCustomer.SelectedValue)
                intMaxProjectInfoID += 1
                arRowData.Add(intMaxProjectInfoID)
                '--Save Record
                StucReturn = WSSSave.SaveProject(arColumnName, arRowData, "T210011")
                If StucReturn.ErrorCode = 0 Then
                    ViewState("PropProjectID") = intMaxProjectInfoID
                    '--Disable Controls if Form is opened in Edit Mode
                    DisableControls()
                    lstError.Items.Clear()
                    lstError.Items.Add("Data Saved Successfully...")
                    ViewState("PropProjectID") = intMaxProjectInfoID
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    EnableDisablePanel()

                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is unable to process your request. Please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            Else '--Update Record
                Dim SelectQry As String = "Select * From T210011 Where PR_NU9_Project_ID_Pk=" & ViewState("PropProjectID") & " AND PR_NU9_Comp_ID_FK=" & ViewState("PropCAComp")
                StucReturn = WSSUpdate.UpdateProject(arColumnName, arRowData, "T210011", SelectQry)
                '--Disable Controls if Form is opened in Edit Mode
                DisableControls()
                lstError.Items.Clear()
                lstError.Items.Add("Data updated successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                ' ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Return True
            End If
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "SaveProjectInfo-130", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Private Function SaveProjectMember() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim StucReturn As ReturnValue
        Dim mshFlag As Int16
        Try

            'Security Block
            If imgSave.Enabled = False Or imgSave.Visible = False Then
                lstError.Items.Clear()
                'cpnlError.Text = "Message..."
                lstError.Items.Add("Your Role does not have rights to save SubCategory Member...")
                mshFlag = 0
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                ' ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Exit Function
            End If
            'End of Security Block



            If ddlMember_F.SelectedIndex < 1 Then
                Return True
            End If
            lstError.Items.Clear()
            '--Validations
            Dim intCountDuplicate As Int16
            intCountDuplicate = SQL.Search("ProjectMasterDetail", "SaveProjectInfo", "Select isnull(count(*),0) From T210012 Where PM_NU9_Project_Member_ID=" & ddlMember_F.SelectedValue & " And PM_NU9_Project_ID_Fk=" & ViewState("PropProjectID") & " And PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp"))
            If intCountDuplicate > 0 Then
                lstError.Items.Add("Selected user is already a member of this SubCategory...")
                mshFlag = 1
            End If
            If DDLRole_F.SelectedValue.Equals("") Then
                lstError.Items.Add("Please select the role of the member...")
                mshFlag = 1
            End If
            If DDLReportsTo_F.SelectedValue.Equals("") Then
                lstError.Items.Add("Please select [reports to] Member...")
                mshFlag = 1
            End If
            '------------------------------
            If mshFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If

            arColumnName.Add("PM_NU9_Project_Member_ID")
            arColumnName.Add("PM_NU9_Project_ID_Fk")
            arColumnName.Add("PM_NU9_Comp_ID_FK")
            arColumnName.Add("PM_NU9_Role")
            arColumnName.Add("PM_NU9_Reports_To")

            arRowData.Add(ddlMember_F.SelectedValue)
            arRowData.Add(ViewState("PropProjectID"))
            arRowData.Add(ViewState("PropCAComp"))
            arRowData.Add(DDLRole_F.SelectedValue)
            arRowData.Add(DDLReportsTo_F.SelectedValue)

            Dim intMaxPrMemID As Int32 ' --To Store Member Max ID
            arColumnName.Add("PM_NU9_ID_PK")
            ' --Get Max SubCategory Info ID from  Table T210012
            intMaxPrMemID = SQL.Search("ProjectMasterDetail", "SaveProjectInfo", "Select isnull(Max(PM_NU9_ID_PK),0) From T210012 Where PM_NU9_Project_ID_Fk=" & ViewState("PropProjectID") & " And PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp"))
            intMaxPrMemID += 1
            arRowData.Add(intMaxPrMemID)

            If WSSSave.SaveProject(arColumnName, arRowData, "T210012").ErrorCode = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Data saved successfuly...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                ' ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ClearFastEntry()
                createDataTableMember(strSqlWhere)  're fetch data from database
                BindGridMember() 'rebind grid    
                Return True
            End If


        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "SaveProjectMember-130", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function
#End Region

#Region "Clear FastEntry"
    Private Sub ClearFastEntry()
        ddlMember_F.SelectedIndex = 0
        DDLRole_F.Items.Clear()
        DDLReportsTo_F.Items.Clear()

    End Sub
#End Region

    Private Function ValidateRecords() As Boolean
        Dim mshFlag As Int16
        Try
            lstError.Items.Clear()
            If ViewState("PropProjectID") = -1 Then
                Dim intCountDuplicate As Int16
                intCountDuplicate = SQL.Search("ProjectMasterDetail", "SaveProjectInfo", "Select isnull(count(*),0) From T210011 Where Upper(PR_VC20_Name) = '" & txtProjectName.Text.Trim.ToUpper & "' And PR_NU9_Comp_ID_FK=" & ViewState("PropCAComp"))
                If intCountDuplicate > 0 Then
                    lstError.Items.Add("SubCategory name already exist, Please change SubCategory name...")
                    mshFlag = 1
                End If
            End If
            If ddlCustomer.SelectedValue.Trim.Equals("") = True Then
                lstError.Items.Add("Customer cannot be blank...")
                mshFlag = 1
            End If

            If txtProjectName.Text.Trim.Equals("") = True Then
                lstError.Items.Add("SubCategory Name cannot be blank...")
                mshFlag = 1
            End If

            If cddlProjectType.CDDLGetValue.Trim.Equals("") = True Then
                lstError.Items.Add("SubCategory Type cannot be blank...")
                mshFlag = 1
            End If

            If cddlProjectOwner.CDDLGetValue.Trim.Equals("") = True Then
                lstError.Items.Add("SubCategory Owner cannot be blank...")
                mshFlag = 1
            End If

            If dtProjectStartDate.Text = "" Then
                lstError.Items.Add("SubCategory Start Date cannot be blank...")
                mshFlag = 1
            End If

            If dtClosedDate.Text <> "" Then
                If dtProjectStartDate.Text <> "" Then
                    If CDate(dtProjectStartDate.Text) > CDate(dtClosedDate.Text) Then
                        lstError.Items.Add("SubCategory Start Date cannot be greater than SubCategory Closed Date...")
                        mshFlag = 1
                    End If
                End If
            Else
                lstError.Items.Add("SubCategory Close Date cannot be blank...")
                mshFlag = 1
            End If

            If cddlStatus.CDDLGetValue.Trim.Equals("") = True Then
                lstError.Items.Add("SubCategory Status cannot be blank...")
                mshFlag = 1
            End If
            If Not txtBudget.Text.Trim.Equals("") And Not IsNumeric(txtBudget.Text.Trim) Then
                lstError.Items.Add("Please enter valid budget value...")
                mshFlag = 1
            End If
            If mshFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            Else
                Return True
            End If

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "ValidateRecords-1695", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Function

#Region "Create Member Grid"

    Private Sub CreateGridMember()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        Try
            dtgMember.ID = "dtgMember"
            dtgMember.DataKeyField = "PM_NU9_ID_PK"
            Call FormatGridMember()

            Placeholder1.Controls.Add(dtgMember)
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "CreateGridMember-411", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub FormatGridMember()
        Try
            dtgMember.AutoGenerateColumns = False
            dtgMember.AllowPaging = True
            '  dtgMember.ShowFooter = True
            dtgMember.ShowHeader = True
            dtgMember.HeaderStyle.CssClass = "GridHeader"
            dtgMember.HeaderStyle.Height = System.Web.UI.WebControls.Unit.Pixel(1)
            dtgMember.Width = System.Web.UI.WebControls.Unit.Percentage(100)
            dtgMember.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
            dtgMember.BorderStyle = BorderStyle.None
            dtgMember.CellPadding = 1
            dtgMember.AllowPaging = False
            dtgMember.CssClass = "Grid"
            dtgMember.HorizontalAlign = HorizontalAlign.Left
            'dtgMember.FooterStyle.CssClass = "GridFixedFooter"
            dtgMember.SelectedItemStyle.CssClass = "GridSelectedItem"
            dtgMember.AlternatingItemStyle.CssClass = "GridAlternateItem"
            dtgMember.ItemStyle.CssClass = "GridItem"
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "FormatGridMember-435", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub
#End Region

#Region "Create Template Column Task Grid"
    Private Sub createTemplateColumnsMember()
        Dim intCount As Integer
        Try

            ReDim tclMember(dtvMember.Table.Columns.Count)

            arrColumnsNameMember.Clear()

            arrColumnsNameMember.Add("SNo")
            arrColumnsNameMember.Add("Name")
            arrColumnsNameMember.Add("Role")
            arrColumnsNameMember.Add("ReportsTo")
            arrColumnsNameMember.Add("Id")

            arrWidthMember.Clear()

            arrWidthMember.Add(50)
            arrWidthMember.Add(250)
            arrWidthMember.Add(220)
            arrWidthMember.Add(250)
            arrWidthMember.Add(0)

            dtgMember.Width = Unit.Pixel(770)
            arrColumnsWidthMember.Clear()

            arrColumnsWidthMember.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthMember(0)))
            arrColumnsWidthMember.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthMember(1)))
            arrColumnsWidthMember.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthMember(2)))
            arrColumnsWidthMember.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthMember(3)))
            arrColumnsWidthMember.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthMember(3)))

            Dim maxchar() As Int16 = {3, 8, 8, 10, 8} 'Variable to store MaxLength of TextBoxes

            For intCount = 0 To dtvMember.Table.Columns.Count - 2

                tclMember(intCount + 1) = New TemplateColumn
                tclMember(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvMember.Table.Columns(intCount).ToString, dtvMember.Table.Columns(intCount).ToString)
                tclMember(intCount + 1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", "", dtvMember.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameMember(intCount).ToString, True, maxchar(intCount))

                tclMember(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvMember.Table.Columns(intCount).ToString + "_F", False)
                tclMember(intCount + 1).ItemStyle.Width = arrColumnsWidthMember(intCount)    'System.Web.UI.WebControls.Unit..Pixel(arrColumnsWidthMember(intCount))
                dtgMember.Columns.Add(tclMember(intCount + 1))
            Next

        Catch ex As Exception
            lstError.Items.Add("Unexpected Error..")
            CreateLog("ProjectMasterDetail", "CreateTemplateColumnsMember-524", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

#End Region

    Private Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Try
            '    If boolFunctionCallStatus = False Then 'Save records only when if it is called first time after page_load
            If SaveProjectInfo() = True Then ' -- If SubCategory gets successfuly saved then only SubCategory member will save
                If sender.GetType.FullName = "System.String" Then 'If Called from PageLoad then don't display update message for update SubCategory Info
                    lstError.Items.Clear()
                    'cpnlError.State = CustomControls.Web.PanelState.Collapsed
                    'cpnlError.Visible = False
                End If
                SaveProjectMember()
            End If
            '    boolFunctionCallStatus = True 'Set Status to Called
            '   End If
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "imgSave_Click-494", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub EnableControls()
        'Enable Controls if Form is opened in Edit Mode
        Try
            ddlCustomer.Enabled = True

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "EnableControls-504", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub DisableControls()
        'Disable Controls if Form is opened in Edit Mode
        Try
            ddlCustomer.Enabled = False
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "DisableControls-514", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub FillCustomDDl(Optional ByVal CustomerChanged As Boolean = False)
        Try
            ' -- SubCategory Type
            If Val(ViewState("PropCAComp")) = 0 Then
                Return
            End If
            cddlProjectType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""PJTY""" & _
            " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
            " Select Name as ID,Description,"" "" as Company from UDC  where  ProductCode=0   and UDCType=""PJTY""" & _
            " and UDC.Company=0 Order By Name"
            cddlProjectType.CDDLMandatoryField = True
            '------------------------------------------

            ' -- SubCategory Status
            cddlStatus.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""PJST""" & _
                      " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
                      " Select Name as ID,Description,"" "" as Company from UDC  where  ProductCode=0   and UDCType=""PJST""" & _
                      " and UDC.Company=0 Order By Name"
            cddlProjectType.CDDLMandatoryField = True
            '------------------------------------------

            '--Parent SubCategory
            cddlParentProject.CDDLQuery = " Select PR_NU9_Project_ID_Pk as ID, PR_VC20_Name as Description From T210011 Where PR_NU9_Comp_ID_FK= " & ViewState("PropCAComp")
            If ViewState("PropProjectID") <> -1 Then
                cddlParentProject.CDDLQuery = cddlParentProject.CDDLQuery & " And Pr_Nu9_Project_ID_Pk <>" & ViewState("PropProjectID")
            End If


            '-- SubCategory Owner
            cddlProjectOwner.CDDLQuery = "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ddlCustomer.SelectedValue) & " and UC_BT1_Access=1)  Order By Name"
            'cddlProjectOwner.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id=" & ddlCustomer.SelectedValue & ") and UM_IN4_Company_AB_ID=" & ddlCustomer.SelectedValue & " Order By Name"

            cddlProjectOwner.CDDLMandatoryField = True
            cddlProjectOwner.CDDLUDC = False
            cddlProjectOwner.CDDLFillDropDown(10, False)
            '-----------------------------------------

            If IsPostBack = False Or CustomerChanged = True Then
                If CustomerChanged = True Then

                End If
                cddlStatus.CDDLFillDropDown(10, True)
                cddlStatus.CDDLSetSelectedItem("Enable")
                cddlParentProject.CDDLFillDropDown(10, False)
                cddlProjectOwner.CDDLFillDropDown(10, False)
                cddlProjectType.CDDLFillDropDown(10, True)
            ElseIf IsPostBack = True Then
                cddlStatus.CDDLSetItem()
                cddlParentProject.CDDLSetItem()
                cddlProjectOwner.CDDLSetItem()
                cddlProjectType.CDDLSetItem()
            End If
            FillNonUDCDropDown(ddlMember_F, "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ddlCustomer.SelectedValue) & " and UC_BT1_Access=1)  Order By Name", True)

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "FillCustomDDL-607", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCustomer.SelectedIndexChanged
        ViewState("PropCAComp") = ddlCustomer.SelectedValue
        'cpnlError.Visible = False
        FillCustomDDl(True)
    End Sub

    Private Sub createDataTableMember(ByVal strWhereClause As String)
        Dim dsMember As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32
        Try

            strSql = "Select PM_NU9_ID_PK,mem.UM_VC50_UserID+'~'+convert(varchar(8),PM_NU9_Project_Member_ID) as ""mem.UM_VC50_UserID"",role.ROM_VC50_Role_Name,rep.UM_VC50_UserID+'~'+convert(varchar(8),PM_NU9_Reports_To) as ""rep.UM_VC50_UserID"", PM_NU9_Project_Member_ID From T210012,T070031 as role, T060011 as Rep,T060011 as Mem" & _
                    " Where PM_NU9_Project_ID_Fk=" & ViewState("PropProjectID") & " And PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & _
                    " And PM_NU9_Project_Member_ID=mem.UM_IN4_Address_No_FK and PM_NU9_Reports_To=rep.UM_IN4_Address_No_FK " & _
                    " And PM_NU9_Role=role.ROM_IN4_Role_ID_PK And Mem.UM_VC4_Status_Code_FK <> 'DNB'"
            If strWhereClause.Trim.Equals("") = False Then
                strSql = strSql & " AND " & strWhereClause
            End If
            strSql = strSql & " order by PM_NU9_ID_PK"
            ' SQL.DBTable = "T210012"
            Call SQL.Search("T210012", "ProjectMasterDetail", "CreateDataTableMember-690", strSql, dsMember, "sachin", "Prashar")
            dtvMember.Table = dsMember.Tables(0)
            '==Add blank row ===============
            If dsMember.Tables(0).Rows.Count = 0 Then
                rowTemp = dsMember.Tables(0).NewRow
                For intCount = 0 To dsMember.Tables(0).Columns.Count - 1
                    Select Case dsMember.Tables(0).Columns(intCount).DataType.FullName
                        Case "System.String"
                            rowTemp.Item(intCount) = " "
                        Case "System.Decimal", "System.Double", "System.Int32", "System.Int16"
                            rowTemp.Item(intCount) = 0
                        Case "System.DateTime"
                    End Select
                Next
                dsMember.Tables(0).Rows.Add(rowTemp)
                dtvMember.Table = dsMember.Tables(0)
            End If
            '===============================
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "CreateDataTableMember-640", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

#Region "Fill Action Header Array"
    Private Sub FillHeaderArrayMember()
        Dim t As New Control
        Dim intCount As Integer
        Try
            arrHeadersMember.Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvMember.Table.Columns.Count - 1
                    arrHeadersMember.Add(Request.Form("cpnlMember$dtgMember$ctl01$" + dtvMember.Table.Columns(intCount).ColumnName + "_H"))
                Next
            End If
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "FillHeaderArrayMember-655", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub
#End Region

#Region "Bind Member Grid"
    Private Sub BindGridMember()
        Try
            dtgMember.DataSource = dtvMember
            dtgMember.DataBind()
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "BindGridMember-712", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub
#End Region

    Private Sub dtgMember_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMember.ItemDataBound
        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvMember
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvMember.Table
        Dim strSelected As String
        dg.PageSize = 1
        Try
            If e.Item.ItemType = ListItemType.Header Then
                For intCount = 0 To dtvMember.Table.Columns.Count - 2     'for Others
                    CType(e.Item.Cells(intCount).FindControl("lbl" & dtvMember.Table.Columns(intCount).ToString & "_H"), LinkButton).Width = arrColumnsWidthMember(intCount)
                    CType(e.Item.Cells(intCount).FindControl("lbl" & dtvMember.Table.Columns(intCount).ToString & "_H"), Label).Text = "" & CType(e.Item.Cells(intCount).FindControl("lbl" & dtvMember.Table.Columns(intCount).ToString & "_H"), Label).Text

                    'CType(e.Item.Cells(intCount).Controls(0), TextBox).Width = arrColumnsWidthMember(intCount)
                    'CType(e.Item.Cells(intCount).Controls(1), Label).Text = "<BR>" & CType(e.Item.Cells(intCount).Controls(1), Label).Text
                Next
            End If
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dtgMember.DataKeys(0) <> 0 Then

                    For intCount = 0 To dtvMember.Table.Columns.Count - 2     'for Others

                        If intCount = 1 Or intCount = 3 Then 'if owner's column
                            CType(e.Item.Cells(intCount).FindControl(dtvMember.Table.Columns(intCount).ToString), Label).Text = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is DBNull.Value, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                            e.Item.Cells(intCount).ToolTip = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is DBNull.Value, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                        Else
                            CType(e.Item.Cells(intCount).FindControl(dtvMember.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is DBNull.Value, " ", dtBound.Rows(cnt)(intCount).ToString)
                            'Tootip should have full value
                            e.Item.Cells(intCount).ToolTip = IIf(dtBound.Rows(cnt)(intCount).ToString Is DBNull.Value, " ", dtBound.Rows(cnt)(intCount).ToString)
                        End If
                        CType(e.Item.Cells(intCount).FindControl(dtvMember.Table.Columns(intCount).ToString), Label).Width = arrColumnsWidthMember(intCount)

                        strID = dtgMember.DataKeys(e.Item.ItemIndex)
                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & e.Item.ItemIndex + 1 & "', 'cpnlCallTask_dtgTask')")
                        mMemberRowValue = e.Item.ItemIndex
                        If intCount = 1 Or intCount = 3 Then
                            e.Item.Cells(intCount).ForeColor = System.Drawing.Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is DBNull.Value, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(1) & "')")
                        Else
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "','" & ViewState("PropProjectID") & "','" & ViewState("PropCAComp") & "')")
                        End If
                    Next
                    mMemberRowValue += 1
                Else
                    For intCount = 0 To dtvMember.Table.Columns.Count - 2
                        CType(e.Item.Cells(intCount).FindControl(dtvMember.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "dtgMember_ItemDataBound-814", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
    Private Function DeleteMembers() As Boolean
        'Dim boolMemberUsed As Boolean
        Dim strSql As String
        Dim shFlag As Int16 = 0
        Dim intCountRows As Int32 = 0
        Try
            If ViewState("PropCAComp") Is Nothing Or ViewState("PropProjectID") Is Nothing Then
                'DisplayError("Call Status cann't be blank")
                Return False
            End If
            Dim intMemberID = SQL.Search("ProjectDetail", "Delete Member", "select PM_NU9_Project_Member_ID from T210012 where PM_NU9_ID_PK=" & mintMemberID & " and PM_NU9_Project_ID_FK=" & Val(ViewState("PropProjectID")) & " and PM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")))
            'Check Whether the SubCategory is already used
            If Not Session("PropRole") = 114 Then
                If WSSSearch.IsProjectMemberInUse(intMemberID, Val(ViewState("PropProjectID")), Val(ViewState("PropCAComp"))) = True Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Active member cannot be deleted... ")
                    shFlag = 1
                End If
            End If
            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If

            'Delete Member's Record
            strSql = "Delete From T210012 Where PM_NU9_Project_ID_Fk=" & ViewState("PropProjectID") & " And PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & " And PM_NU9_ID_PK=" & mintMemberID
            If SQL.Delete("ProjectMasterDetail", "DeleteMembers-935", strSql, SQL.Transaction.ReadCommitted) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Member Successfully Deleted...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                '  ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                txthidden.Text = 0
                '--- refresh grid
                createDataTableMember("")
                BindGridMember()
                ' ------------------------------
            Else
            End If

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "DeleteMembers-905", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function

    Private Sub imgOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOk.Click
        Try
            If SaveProjectMember() = True Then
                If SaveProjectInfo() = True Then                    'Save ProjectInfo only if imgSave is Clicked by the user
                    '///Enter Code here to close this window and go back to SubCategory View
                    Dim focusScript As String = "<script language='javascript'>" & _
                           "window.parent.closeTab();location.href=""ProjectView.aspx?ScrID=40""</script>"

                    ' Add the JavaScript code to the page.
                    Page.RegisterStartupScript("FocusScript", focusScript)
                    Exit Sub
                End If
            End If
            FillData()
        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "imgSave_Click-494", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub
   
    Private Sub imgDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDelete.Click
        DeleteMembers()
    End Sub
    Private Sub EnableDisablePanel()
        ' -- Enable/Disable Panels
        If ViewState("PropProjectID") = -1 Then
            EnableControls()
            cpnlOverview.State = CustomControls.Web.PanelState.Collapsed
            cpnlOverview.Enabled = False
            cpnlMember.State = CustomControls.Web.PanelState.Collapsed
            cpnlMember.Enabled = False
            cpnlOverview.CssClass = "test2"
            cpnlMember.CssClass = "test2"
        Else
            DisableControls()
            cpnlOverview.State = CustomControls.Web.PanelState.Expanded
            cpnlOverview.Enabled = True
            cpnlMember.State = CustomControls.Web.PanelState.Expanded
            cpnlMember.Enabled = True
            cpnlOverview.CssClass = "test2"
            cpnlMember.CssClass = "test2"
        End If
    End Sub
End Class
