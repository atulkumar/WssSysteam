'*******************************************************************************************************
' Page                 : - ProjectMemEdit
' Purpose              : - 
' Tables used          : - t060011,t010011,T070031,t060022
' Date					   Author						Modification Date				Description
' 20/07/06			      Ranvijay		                ------------------				Created
'
' Notes: 
' Code:
'*******************************************************************************************************
Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Data.SqlClient
Partial Class AdministrationCenter_Project_ProjectMemEdit
    Inherits System.Web.UI.Page
#Region " Form Level Variables "
    Private Shared intMemberSNO As Int32 'Keep Selected Member's Sno
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            intMemberSNO = Request.QueryString("MemberSNO")
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            If IsPostBack Then
                FillAjaxDropDown(DDLRole_F, Request.Form("txthiddenRole"), "cpnlMember:" & DDLRole_F.ID, New ListItem("", ""))
                FillAjaxDropDown(DDLReportsTo_F, Request.Form("txthiddenReportsTo"), "cpnlMember:" & DDLReportsTo_F.ID, New ListItem("", ""))
            End If

            ''''''added on 12 Aug by Tarun so as to Remove Sessions and using ViewState... 

            ViewState("PropProjectID") = Request.QueryString("ProjectID")
            ViewState("PropCAComp") = Request.QueryString("CompanyID")
            txtCSS(Me.Page)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Page.IsPostBack = False Then

                'imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
                imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
                ddlMember.Attributes.Add("onChange", "MemberChange('" & ViewState("PropCAComp") & "');")
                '-- Fill DDLMember Combo
                ddlMember.Items.Add("")

                'FillNonUDCDropDown(ddlMember, " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select ci_nu8_address_number from t010011 where ci_vc8_status='ENA') and  um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='SCM') Order By Name")
                FillNonUDCDropDown(ddlMember, "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and UC_BT1_Access=1)  Order By Name", True)



                FillData()
            End If

            'Security Block
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(756) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, 756)
                'End of Security Block
            End If
        Catch ex As Exception
            CreateLog("ProjectMemEdit", "PageLoad-64", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub
#Region "Update ProjectMember"
    Private Function UpdateProjectMember() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim StucReturn As ReturnValue
        Dim mshFlag As Int16
        Dim strSql As String
        Try
            lstError.Items.Clear()
            If ddlMember.SelectedIndex = -1 Then
                Return True
            End If
            '--Validations
            Dim intCountDuplicate As Int16
            intCountDuplicate = SQL.Search("ProjectMasterDetail", "SaveProjectInfo", "Select isnull(count(*),0) From T210012 Where PM_NU9_ID_PK<>" & intMemberSNO & " AND PM_NU9_Project_Member_ID=" & ddlMember.SelectedValue & " And PM_NU9_Project_ID_Fk=" & ViewState("PropProjectID") & " And PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp"))
            If intCountDuplicate > 0 Then
                lstError.Items.Add("Selected user is already a Member of this SubCategory...")
                mshFlag = 1
            End If
            If DDLRole_F.SelectedValue.Equals("") Then
                lstError.Items.Add("Please select member's role...")
                mshFlag = 1
            End If
            If DDLReportsTo_F.SelectedValue.Equals("") Then
                lstError.Items.Add("Please select [Reports To] from the combo...")
                mshFlag = 1
            End If
            '------------------------------
            If mshFlag = 1 Then
                'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If

            arColumnName.Add("PM_NU9_Project_Member_ID")
            arColumnName.Add("PM_NU9_Project_ID_Fk")
            arColumnName.Add("PM_NU9_Comp_ID_FK")
            arColumnName.Add("PM_NU9_Role")
            arColumnName.Add("PM_NU9_Reports_To")
            arRowData.Add(ddlMember.SelectedValue)
            arRowData.Add(ViewState("PropProjectID"))
            arRowData.Add(ViewState("PropCAComp"))
            arRowData.Add(DDLRole_F.SelectedValue)
            arRowData.Add(DDLReportsTo_F.SelectedValue)

            strSql = "Select * from T210012 Where PM_NU9_Project_ID_Fk=" & ViewState("PropProjectID") & " And PM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & " And PM_NU9_ID_PK=" & intMemberSNO
            If WSSUpdate.UpdateProject(arColumnName, arRowData, "T210012", strSql).ErrorCode = 0 Then
                lstError.Items.Clear()
                DisplayMessage("Records Updated Successfully...")

                Dim focusScript As String = "<script language='javascript'>" & _
                          "self.opener.callrefresh();</script>"

                ' Add the JavaScript code to the page.
                Page.RegisterStartupScript("FocusScript", focusScript)
                FillData()

                Return True
            End If

        Catch ex As Exception
            CreateLog("ProjectMasterDetail", "SaveProjectMember-130", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Function
#End Region

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        Try
            'cpnlError.Visible = True
            ';cpnlError.State = CustomControls.Web.PanelState.Expanded
            'lstError.ForeColor = Color.Red
            lstError.Items.Add(ErrMsg)
            'cpnlError.TitleCSS = "Test3"
            'cpnlError.Text = "Error Occured"
            'ImgError.ImageUrl = "../../Images/warning.gif"
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
        Catch ex As Exception
            CreateLog("Call-Detail", "DisplayError-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        Try
            ' cpnlError.Visible = True
            ' cpnlError.State = CustomControls.Web.PanelState.Expanded
            lstError.Items.Add(Msg)
            'lstError.ForeColor = Color.Black
            'cpnlError.TitleCSS = "Test"
            'cpnlError.Text = "Message..."
            'ImgError.ImageUrl = "../../Images/pok.gif"
            ' ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception
            CreateLog("Call-Detail", "DisplayMessage-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
#End Region

    Private Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        'Update Changes
        UpdateProjectMember()
    End Sub
    'Private Sub FillCustomDDl(Optional ByVal CustomerChanged As Boolean = False)
    '    Try
    '        ' -- SubCategory Type
    '        If Val(ViewState("PropCAComp")) = 0 Then
    '            Return
    '        End If

    '        ' -- Role
    '        cddlRole.CDDLQuery = "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name from T070031 where ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_IN4_Address_No_FK=" & ddlMember.SelectedValue & ") and RA_DT8_Assigned_Date <='" & Today & "' and RA_DT8_Valid_UpTo >='" & Today & "' and RA_VC4_Status_Code ='ENB') "
    '        cddlRole.CDDLUDC = False
    '        ' ---------------------------------
    '        ' -- Reports To
    '        cddlReportsTo.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name  FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and ( um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='SCM')) and um_in4_address_no_fk<>" & ddlMember.SelectedValue & " Order By Name"
    '        cddlReportsTo.CDDLUDC = False
    '        '-----------------------------------------

    '        ' If IsPostBack = False Then
    '        cddlRole.CDDLFillDropDown(10, False)
    '        cddlReportsTo.CDDLFillDropDown(10, False)
    '        'ElseIf IsPostBack = True Then
    '        cddlRole.CDDLSetItem()
    '        cddlReportsTo.CDDLSetItem()
    '        'End If

    '    Catch ex As Exception
    '        CreateLog("ProjectMemEdit", "FillCustomDDL-175", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
    '    End Try
    'End Sub

    'Private Sub ddlMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMember.SelectedIndexChanged
    '    'FillCustomDDl(True)
    'End Sub

    Private Sub FillData()
        Dim strSql As String
        Dim Sqldr As SqlDataReader
        Dim boolStatus As Boolean
        Try
            strSql = "select ROM_VC50_Role_Name, a.um_vc50_userid as Membername, b.um_vc50_userid as ReportsTo,T210012.* from T210012,T070031,t060011 a,t060011 b " & _
             " Where(PM_NU9_Project_Member_ID = a.um_in4_address_no_fk) " & _
                " And PM_Nu9_Role=ROM_IN4_Role_ID_PK " & _
                " And PM_Nu9_Reports_To=b.um_in4_address_no_fk " & _
                " And PM_NU9_Project_ID_Fk= " & ViewState("PropProjectID") & _
                " And PM_NU9_Comp_ID_FK= " & ViewState("PropCAComp") & _
                " And PM_NU9_ID_PK =" & intMemberSNO
            Sqldr = SQL.Search("ProjectMemEdit", "FillData-209", strSql, SQL.CommandBehaviour.CloseConnection, boolStatus)
            If boolStatus = True Then
                Sqldr.Read()
                ddlMember.SelectedValue = Sqldr.Item("PM_NU9_Project_Member_ID").ToString()
                ' -- FillCustomDDl is called here because we can fill other combo only after filling member 
                '  FillCustomDDl(False)
                'Fill the Role and Report To dropdowns according to selected member
                FillNonUDCDropDown(DDLRole_F, "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_IN4_Address_No_FK=" & Val(ddlMember.SelectedValue) & ") and RA_DT8_Assigned_Date <='" & Today & "' and RA_DT8_Valid_UpTo >='" & Today & "' and RA_VC4_Status_Code ='ENB') ", True)

                'FillNonUDCDropDown(DDLReportsTo_F, "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select ci_nu8_address_number from t010011 where ci_vc8_status='ENA') and  um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation = 'SCM' or ci_nu8_address_number=" & ViewState("PropCAComp") & "  ) and um_in4_address_no_fk<>" & Val(ddlMember.SelectedValue) & " Order By Name", True)
                FillNonUDCDropDown(DDLReportsTo_F, "select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK<>" & ddlMember.SelectedValue & " and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and UC_BT1_Access=1)  Order By Name", True)

                '    cddlReportsTo.CDDLSetSelectedItem(Sqldr.Item("PM_Nu9_Reports_To"), False, Sqldr.Item("ReportsTo"))
                '   cddlRole.CDDLSetSelectedItem(Sqldr.Item("PM_Nu9_Role"), False, Sqldr.Item("ROM_VC50_Role_Name"))
                DDLRole_F.SelectedValue = Sqldr.Item("PM_Nu9_Role").ToString()
                DDLReportsTo_F.SelectedValue = Sqldr.Item("PM_Nu9_Reports_To").ToString()

                'Page.RegisterStartupScript("Role", "<script>document.Form1.txthiddenRole.value=" & Sqldr.Item("PM_Nu9_Role") & "</script> ")
                'Page.RegisterStartupScript("Reports", "<script>document.Form1.txthiddenReportsTo.value=" & Sqldr.Item("PM_Nu9_Reports_To") & "</script> ")

                Sqldr.Close()
            End If
        Catch ex As Exception
            CreateLog("ProjectMemEdit", "FillData-210", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"))
        End Try
    End Sub

    Private Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        If UpdateProjectMember() = True Then
            Dim focusScript As String = "<script language='javascript'>" & _
                                    "window.close();</script>"
            ' Add the JavaScript code to the page.
            Page.RegisterStartupScript("FocusScript1", focusScript)
        End If
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
