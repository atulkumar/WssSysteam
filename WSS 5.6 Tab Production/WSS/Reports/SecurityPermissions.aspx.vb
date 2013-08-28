'************************************************************************************************************
' Page                 : - IPTrack
' Purpose              : - it will show the IP Tracking Report for the user 

' Date		    		 Author						Modification Date					Description
' 17/10/2007 		 Suresh	Kharod     				             					        Created
'
' Notes: Coding for the Security Permissions Reports 
' Code:
'************************************************************************************************************

#Region "Refered Assemblies"

Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

#End Region
Partial Class Reports_SecurityPermissions
    Inherits System.Web.UI.Page
    'Protected WithEvents crvSecurityPermissions As CrystalDecisions.Web.CrystalReportViewer
#Region "Page Level Variables "

    ' Dim ConStr As String = System.Configuration.ConfigurationSettings.AppSettings("ConnectionString")
    Dim crDocument As ReportDocument
    Private objReports As clsReportData
    Private params As ParameterFieldDefinitions
    Private paramValues As ParameterValues
    Private paramCallNo As ParameterFieldDefinition
    Private paramCustomerID As ParameterFieldDefinition
    Private paramDate As ParameterFieldDefinition
    Private paramDiscrete As ParameterDiscreteValue
    Private paramRangeValue As ParameterRangeValue
    Private formulas As FormulaFieldDefinitions
    Public formulaServerName As FormulaFieldDefinition
    Private strServerName As String
    Public mstrCallNumber As String

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'HIDSCRID.Value = Request.QueryString("ScrID")
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        crvSecurityPermissions.ToolbarStyle.Width = New Unit("820px")

        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 951
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
        'End of Security Block
        If Not IsPostBack Then
            fill_company()
            ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            'fill_employee(Val(ddlCompany.SelectedValue))
            'ddlUserID.SelectedValue = HttpContext.Current.Session("PropUserID")
            FillRoleCombo(Val(ddlCompany.SelectedValue))
            mintSecPerm = Nothing
            rblSecurityPermission.Items(0).Selected = True
        End If
        Dim txthiddenImage = Request.Form("txthiddenImage")
        If Session("PropCompanyType") <> "SCM" Then
            ddlCompany.Enabled = False
        End If

        imgShowReport.Attributes.Add("OnClick", "ShowImg();")

        If txthiddenImage = "Logout" Then
            LogoutWSS()
        End If

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Attach"
                        Response.Write("<script>window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');</script>")
                    Case "OK"
                End Select
            Catch ex As Exception
            End Try
        End If
        If mintSecPerm = Nothing Then
        Else
            ShowReport()
        End If
    End Sub
#Region " Fill Drop Down List Boxes"

    Private Function FillRoleCombo(ByVal CompanyID)
        Try

            Dim dt As New DataTable
            objReports = New clsReportData
            ddlRoles.Items.Clear()
            dt = objReports.GetRoleDetails(CompanyID)
            ddlRoles.DataSource = dt
            ddlRoles.DataTextField = "Name"
            ddlRoles.DataValueField = "ID"
            ddlRoles.DataBind()


        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try

    End Function

    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCompany(2)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            'If ddlCompany.Items(0).Text <> "--ALL--" Then
            '    ddlCompany.Items.Insert(0, "--ALL--")
            'End If
            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If
            If Request("cid") <> Nothing Then
                ddlCompany.SelectedValue = CInt(Request("cid"))
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub
    'Private Sub fill_employee(ByVal companyID As String)
    '    Try

    '        Dim dt As New DataTable
    '        objReports = New clsReportData
    '        ddlUserID.Items.Clear()
    '        dt = objReports.ExtractSecPrmEmployees(companyID)
    '        ddlUserID.DataSource = dt
    '        ddlUserID.DataTextField = "Name"
    '        ddlUserID.DataValueField = "AddressNo"
    '        ddlUserID.DataBind()
    '    Catch ex As Exception
    '        Dim str As String = ex.Message.ToString
    '    Finally
    '        objReports = Nothing
    '    End Try
    'End Sub
#End Region

    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged

        FillRoleCombo(Val(ddlCompany.SelectedValue))
        If Not IsPostBack Then
        Else
            ShowReport()
        End If
    End Sub

    Private Function ShowReport() As Boolean
        Try

            Dim strSQL As String = "Select  convert(varchar,'" & ddlCompany.SelectedItem.Text & "') CompName,  convert(varchar,'" & ddlCompany.SelectedItem.Text & "') UserName, 'Hide' MenuViewHide, 'Hide' ScreenViewHide, 'Hide' ViewHide, convert(varchar,'" & Val(ddlRoles.SelectedValue) & "') RoleID, convert(varchar,'" & ddlRoles.SelectedItem.Text & "') RoleName, Menu.OBM_IN4_Object_ID_PK MenuID,Menu.OBM_VC4_Object_Type_FK MenuObjectType,Menu.OBM_VC50_Alias_Name MenuName,Screen.OBM_IN4_Object_ID_PK ScreenID,Screen.OBM_VC4_Object_Type_FK ScreenType,Screen.OBM_VC50_Alias_Name ScreenName, T070011.OBM_IN4_Object_ID_PK ObjectID,T070011.OBM_VC4_Object_Type_FK ObjectType,T070011.OBM_VC50_Alias_Name ObjectName,T070011.OBM_IN4_Object_PID_FK ParentID From T070011,T070011 Screen,T070011 Menu where (T070011.OBM_VC4_Object_Type_FK<>'SCR'and T070011.OBM_VC4_Object_Type_FK<>'POP'and T070011.OBM_VC4_Object_Type_FK<>'MNU')and Screen.OBM_VC4_Object_Type_FK<>'MNU'and  Menu.OBM_VC4_Object_Type_FK='MNU'and T070011.OBM_VC4_Status_Code_FK<>'DSB'and Screen.OBM_VC4_Status_Code_FK<>'DSB'and Menu.OBM_VC4_Status_Code_FK<>'DSB'and (T070011.OBM_CH4_ProductCode_FK is null or T070011.OBM_CH4_ProductCode_FK=0) and (Screen.OBM_CH4_ProductCode_FK is null or Screen.OBM_CH4_ProductCode_FK=0)and (Menu.OBM_CH4_ProductCode_FK is null or Menu.OBM_CH4_ProductCode_FK=0)and Screen.OBM_IN4_Object_PID_FK=Menu.OBM_IN4_Object_ID_PK  and T070011.OBM_IN4_Object_PID_FK=*Screen.OBM_IN4_Object_ID_PK  Order by Menu.OBM_SI2_Order_By ;select  convert(varchar,RA_IN4_Role_ID_FK) RoleID,convert(varchar,RA_IN4_AB_ID_FK) UserID,convert(varchar,T060011.UM_VC50_UserID) UserName From T060022,T060011 where RA_IN4_AB_ID_FK=T060011.UM_IN4_Address_No_FK and RA_IN4_Role_ID_FK=" & Val(ddlRoles.SelectedValue) & ""

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            Dim dsSecPerm As New DataSet

            If SQL.Search("T070011", "", "", strSQL, dsSecPerm, "", "") = True Then
                Dim strSQLRole As String = "select * from T070042 where ROD_IN4_Role_ID_FK=" & Val(ddlRoles.SelectedValue)
                Dim dsRole As New DataSet
                SQL.Search("T070042", "", "", strSQLRole, dsRole, "", "")
                Dim arrDC(1) As DataColumn
                arrDC(0) = dsRole.Tables(0).Columns("ROD_IN4_Object_ID_FK")
                dsRole.Tables(0).PrimaryKey = arrDC

                For intI As Integer = 0 To dsSecPerm.Tables(0).Rows.Count - 1
                    If Not IsNothing(dsRole.Tables(0).Rows.Find(dsSecPerm.Tables(0).Rows(intI).Item("MenuID"))) Then
                        dsSecPerm.Tables(0).Rows(intI).Item("MenuViewHide") = IIf(dsRole.Tables(0).Rows.Find(dsSecPerm.Tables(0).Rows(intI).Item("MenuID")).Item("ROD_CH1_View_Hide") = "V", "View", "Hide")
                    End If
                    If Not IsNothing(dsRole.Tables(0).Rows.Find(dsSecPerm.Tables(0).Rows(intI).Item("ScreenID"))) Then
                        dsSecPerm.Tables(0).Rows(intI).Item("ScreenViewHide") = IIf(dsRole.Tables(0).Rows.Find(dsSecPerm.Tables(0).Rows(intI).Item("ScreenID")).Item("ROD_CH1_View_Hide") = "V", "View", "Hide")
                    End If
                    If Not IsNothing(dsRole.Tables(0).Rows.Find(dsSecPerm.Tables(0).Rows(intI).Item("ObjectID"))) Then
                        dsSecPerm.Tables(0).Rows(intI).Item("ViewHide") = IIf(dsRole.Tables(0).Rows.Find(dsSecPerm.Tables(0).Rows(intI).Item("ObjectID")).Item("ROD_CH1_View_Hide") = "V", "View", "Hide")
                    End If
                Next
                dsSecPerm.Tables(0).AcceptChanges()
            End If

            dsSecPerm.Tables(0).TableName = "SecPermission"
            dsSecPerm.Tables(1).TableName = "UserNames"
            dsSecPerm.AcceptChanges()

            Dim RoleID As String = ""
            Dim UserID As String = ""
            Dim UserName As String = ""
            Dim intJ As Integer = 1
            Dim drNew As DataRow
            drNew = dsSecPerm.Tables("UserNames").NewRow
            If dsSecPerm.Tables("UserNames").Rows.Count > 0 Then
                For intI As Integer = 0 To dsSecPerm.Tables("UserNames").Rows.Count - 1
                    If intI = 0 Then
                        RoleID += dsSecPerm.Tables("UserNames").Rows(intI).Item("RoleID").ToString
                        UserID += dsSecPerm.Tables("UserNames").Rows(intI).Item("UserID").ToString
                        UserName += intJ.ToString + ". " + dsSecPerm.Tables("UserNames").Rows(intI).Item("UserName").ToString
                    Else
                        intJ += 1
                        RoleID += " , " + dsSecPerm.Tables("UserNames").Rows(intI).Item("RoleID").ToString
                        UserID += " ,  " + dsSecPerm.Tables("UserNames").Rows(intI).Item("UserID").ToString
                        UserName += " ,  " + intJ.ToString + ". " + dsSecPerm.Tables("UserNames").Rows(intI).Item("UserName").ToString
                    End If
                Next
                dsSecPerm.Tables("UserNames").Rows.Clear()
                drNew.Item("RoleID") = RoleID
                drNew.Item("UserID") = UserID
                drNew.Item("UserName") = UserName
                dsSecPerm.Tables("UserNames").Rows.Add(drNew)
            Else
                drNew.Item("RoleID") = ""
                drNew.Item("UserID") = ""
                drNew.Item("UserName") = "No Users in this Role"
                dsSecPerm.Tables("UserNames").Rows.Add(drNew)
            End If
            'dsSecPerm.Tables("UserNames").Rows(0).Item("RoleID") = RoleID
            'dsSecPerm.Tables("UserNames").Rows(0).Item("UserID") = UserID
            'dsSecPerm.Tables("UserNames").Rows(0).Item("UserName") = UserName

            If rblSecurityPermission.SelectedValue = 0 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionMenu.rpt")
                crDocument.Load(Reportpath)


                'Dim rptSecurityPermissions As New SecurityPermissionMenu
                crDocument.SetDataSource(dsSecPerm)
                'crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument

            ElseIf rblSecurityPermission.SelectedValue = 1 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionScreens.rpt")
                crDocument.Load(Reportpath)

                'Dim rptSecurityPermissions As New SecurityPermissionScreens
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument

            ElseIf rblSecurityPermission.SelectedValue = 2 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionsReport.rpt")
                crDocument.Load(Reportpath)

                'Dim rptSecurityPermissions As New SecurityPermissionsReport
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument
            Else
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionsReport.rpt")
                crDocument.Load(Reportpath)
                'Dim rptSecurityPermissions As New SecurityPermissionsReport
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument
            End If

            crvSecurityPermissions.EnableDrillDown = False
            mintSecPerm = 1
            'End If



        Catch ex As Exception
        End Try
    End Function

#Region "Show Reprt"
    Function ShowReport3() As Boolean
        Try
            Dim dsSecPerm As New DataSet
            Dim drNew As DataRow
            Dim strSQL As String
            Select Case ddlCompany.SelectedItem.Text

                Case "--All--".ToUpper

                    strSQL = "Select  distinct T070042.ROD_IN4_Role_ID_FK RoleID,T070031.ROM_VC50_Role_Name RoleName,T010011.CI_VC36_Name CompName,T010011.CI_VC36_Name UserName,Control.MenuID,Control.MenuObjectType, T070042.ROD_VC50_Alias_Name MenuName,T070042.ROD_CH1_View_Hide MenuViewHide,Control.ScreenID,Control.ScreenType,Control.ScreenName,SCRView.ROD_CH1_View_Hide ScreenViewHide,Control.ObjectID,Control.ObjectType,Control.ObjectName,ObjectView.ROD_CH1_View_Hide ViewHide,Control.OrderBy from T070042,T070042 SCRView,T070042 ObjectView, (Select Menu.OBM_SI2_Order_By OrderBy, Menu.OBM_IN4_Object_ID_PK MenuID,Menu.OBM_VC4_Object_Type_FK MenuObjectType,Menu.OBM_VC50_Alias_Name MenuName,Screen.OBM_IN4_Object_ID_PK ScreenID,Screen.OBM_VC4_Object_Type_FK ScreenType,Screen.OBM_VC50_Alias_Name ScreenName, T070011.OBM_IN4_Object_ID_PK ObjectID,T070011.OBM_VC4_Object_Type_FK ObjectType,T070011.OBM_VC50_Alias_Name ObjectName,T070011.OBM_IN4_Object_PID_FK ParentID From T070011,T070011 Screen,T070011 Menu where (T070011.OBM_VC4_Object_Type_FK<>'SCR'and T070011.OBM_VC4_Object_Type_FK<>'POP'and T070011.OBM_VC4_Object_Type_FK<>'MNU')and Screen.OBM_VC4_Object_Type_FK<>'MNU'and  Menu.OBM_VC4_Object_Type_FK='MNU'and T070011.OBM_VC4_Status_Code_FK<>'DSB'and Screen.OBM_VC4_Status_Code_FK<>'DSB'and Menu.OBM_VC4_Status_Code_FK<>'DSB'and (T070011.OBM_CH4_ProductCode_FK is null or T070011.OBM_CH4_ProductCode_FK=0)and (Screen.OBM_CH4_ProductCode_FK is null or Screen.OBM_CH4_ProductCode_FK=0)and (Menu.OBM_CH4_ProductCode_FK is null or Menu.OBM_CH4_ProductCode_FK=0)and Screen.OBM_IN4_Object_PID_FK=Menu.OBM_IN4_Object_ID_PK  and T070011.OBM_IN4_Object_PID_FK=Screen.OBM_IN4_Object_ID_PK) Control, T070031,T010011 Where T070042.ROD_IN4_Object_ID_FK=Control.MenuID and  SCRView.ROD_IN4_Object_ID_FK=Control.ScreenID and ObjectView.ROD_IN4_Object_ID_FK=Control.ObjectID and ObjectView.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and SCRView.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and T070042.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and T070042.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and SCRView.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and ObjectView.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and T010011.CI_NU8_Address_Number=T070031.ROM_IN4_Company_ID_FK  order by OrderBy Asc,ScreenType desc ; select convert(varchar,RA_IN4_Role_ID_FK) RoleID,RA_IN4_AB_ID_FK UserID,convert(varchar,T060011.UM_VC50_UserID) UserName From T060022,T060011 where RA_IN4_AB_ID_FK=T060011.UM_IN4_Address_No_FK and RA_IN4_Role_ID_FK=" & Val(ddlRoles.SelectedValue) & ";select convert(varchar,null) RoleID,convert(varchar,null) RoleName,convert(varchar,null) CompName,convert(varchar,null) UserName,T070011.OBM_IN4_Object_ID_PK MenuID,OBM_VC4_Object_Type_FK MenuObjectType,OBM_VC50_Alias_Name MenuName,'H' MenuViewHide,null ScreenID,Null ScreenType,'No Access' ScreenName,Null ScreenViewHide,Null ObjectID,null ObjectType,null ObjectName,null ViewHide,null OrderBy From T070011 where OBM_VC4_Object_Type_FK='MNU'and (T070011.OBM_CH4_ProductCode_FK is null or T070011.OBM_CH4_ProductCode_FK=0)and T070011.OBM_VC4_Status_Code_FK<>'DSB'  "
                    'MenuID,ScreenID,ObjectID
                Case Else
                    Dim intCompID As Integer = ddlCompany.SelectedValue
                    strSQL = "Select   distinct T070042.ROD_IN4_Role_ID_FK RoleID,T070031.ROM_VC50_Role_Name RoleName,T010011.CI_VC36_Name CompName,T010011.CI_VC36_Name UserName,Control.MenuID,Control.MenuObjectType,T070042.ROD_VC50_Alias_Name MenuName,T070042.ROD_CH1_View_Hide MenuViewHide,Control.ScreenID,Control.ScreenType,Control.ScreenName,SCRView.ROD_CH1_View_Hide ScreenViewHide,Control.ObjectID,Control.ObjectType,Control.ObjectName,ObjectView.ROD_CH1_View_Hide ViewHide,Control.OrderBy from T070042,T070042 SCRView,T070042 ObjectView, (Select Menu.OBM_SI2_Order_By OrderBy, Menu.OBM_IN4_Object_ID_PK MenuID,Menu.OBM_VC4_Object_Type_FK MenuObjectType,Menu.OBM_VC50_Alias_Name MenuName,Screen.OBM_IN4_Object_ID_PK ScreenID,Screen.OBM_VC4_Object_Type_FK ScreenType,Screen.OBM_VC50_Alias_Name ScreenName, T070011.OBM_IN4_Object_ID_PK ObjectID,T070011.OBM_VC4_Object_Type_FK ObjectType,T070011.OBM_VC50_Alias_Name ObjectName,T070011.OBM_IN4_Object_PID_FK ParentID From T070011,T070011 Screen,T070011 Menu where (T070011.OBM_VC4_Object_Type_FK<>'SCR'and T070011.OBM_VC4_Object_Type_FK<>'POP'and T070011.OBM_VC4_Object_Type_FK<>'MNU')and Screen.OBM_VC4_Object_Type_FK<>'MNU'and  Menu.OBM_VC4_Object_Type_FK='MNU'and T070011.OBM_VC4_Status_Code_FK<>'DSB'and Screen.OBM_VC4_Status_Code_FK<>'DSB'and Menu.OBM_VC4_Status_Code_FK<>'DSB'and (T070011.OBM_CH4_ProductCode_FK is null or T070011.OBM_CH4_ProductCode_FK=0) and (Screen.OBM_CH4_ProductCode_FK is null or Screen.OBM_CH4_ProductCode_FK=0)and (Menu.OBM_CH4_ProductCode_FK is null or Menu.OBM_CH4_ProductCode_FK=0)and Screen.OBM_IN4_Object_PID_FK=Menu.OBM_IN4_Object_ID_PK  and T070011.OBM_IN4_Object_PID_FK=Screen.OBM_IN4_Object_ID_PK) Control, T070031,T010011 Where T070042.ROD_IN4_Object_ID_FK=Control.MenuID and  SCRView.ROD_IN4_Object_ID_FK=Control.ScreenID and ObjectView.ROD_IN4_Object_ID_FK=Control.ObjectID and ObjectView.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and SCRView.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and T070042.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and T070042.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and SCRView.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and ObjectView.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and T010011.CI_NU8_Address_Number=T070031.ROM_IN4_Company_ID_FK and T070031.ROM_IN4_Company_ID_FK=" & Val(ddlCompany.SelectedValue) & " order by OrderBy  Asc,ScreenType desc ;select  convert(varchar,RA_IN4_Role_ID_FK) RoleID,convert(varchar,RA_IN4_AB_ID_FK) UserID,convert(varchar,T060011.UM_VC50_UserID) UserName From T060022,T060011 where RA_IN4_AB_ID_FK=T060011.UM_IN4_Address_No_FK and RA_IN4_Role_ID_FK=" & Val(ddlRoles.SelectedValue) & ";select convert(varchar,null) RoleID,convert(varchar,null) RoleName,convert(varchar,null) CompName,convert(varchar,null) UserName,T070011.OBM_IN4_Object_ID_PK MenuID,OBM_VC4_Object_Type_FK MenuObjectType,OBM_VC50_Alias_Name MenuName,'H' MenuViewHide,null ScreenID,Null ScreenType,'No Access' ScreenName,Null ScreenViewHide,Null ObjectID,null ObjectType,null ObjectName,null ViewHide,null OrderBy From T070011 where OBM_VC4_Object_Type_FK='MNU'and (T070011.OBM_CH4_ProductCode_FK is null or T070011.OBM_CH4_ProductCode_FK=0)and T070011.OBM_VC4_Status_Code_FK<>'DSB' "
                    'strSQL = "Select   distinct T070042.ROD_IN4_Role_ID_FK RoleID,T070031.ROM_VC50_Role_Name RoleName,T010011.CI_VC36_Name CompName,T010011.CI_VC36_Name UserName,Control.MenuObjectType,T070042.ROD_VC50_Alias_Name MenuName,T070042.ROD_CH1_View_Hide MenuViewHide,Control.ScreenID,Control.ScreenType,Control.ScreenName,SCRView.ROD_CH1_View_Hide ScreenViewHide,Control.ObjectID,Control.ObjectType,Control.ObjectName,ObjectView.ROD_CH1_View_Hide ViewHide,Control.OrderBy from T070042,T070042 SCRView,T070042 ObjectView, (Select Menu.OBM_SI2_Order_By OrderBy, Menu.OBM_IN4_Object_ID_PK MenuID,Menu.OBM_VC4_Object_Type_FK MenuObjectType,Menu.OBM_VC50_Alias_Name MenuName,Screen.OBM_IN4_Object_ID_PK ScreenID,Screen.OBM_VC4_Object_Type_FK ScreenType,Screen.OBM_VC50_Alias_Name ScreenName, T070011.OBM_IN4_Object_ID_PK ObjectID,T070011.OBM_VC4_Object_Type_FK ObjectType,T070011.OBM_VC50_Alias_Name ObjectName,T070011.OBM_IN4_Object_PID_FK ParentID From T070011,T070011 Screen,T070011 Menu where (T070011.OBM_VC4_Object_Type_FK<>'SCR'and T070011.OBM_VC4_Object_Type_FK<>'POP'and T070011.OBM_VC4_Object_Type_FK<>'MNU')and Screen.OBM_VC4_Object_Type_FK<>'MNU'and  Menu.OBM_VC4_Object_Type_FK='MNU'and T070011.OBM_VC4_Status_Code_FK<>'DSB'and Screen.OBM_VC4_Status_Code_FK<>'DSB'and Menu.OBM_VC4_Status_Code_FK<>'DSB'and (T070011.OBM_CH4_ProductCode_FK is null or T070011.OBM_CH4_ProductCode_FK=0)and (Screen.OBM_CH4_ProductCode_FK is null or Screen.OBM_CH4_ProductCode_FK=0)and (Menu.OBM_CH4_ProductCode_FK is null or Menu.OBM_CH4_ProductCode_FK=0)and Screen.OBM_IN4_Object_PID_FK=Menu.OBM_IN4_Object_ID_PK  and T070011.OBM_IN4_Object_PID_FK=*Screen.OBM_IN4_Object_ID_PK) Control, T070031,T010011 Where T070042.ROD_IN4_Object_ID_FK=Control.MenuID and  SCRView.ROD_IN4_Object_ID_FK=Control.ScreenID and ObjectView.ROD_IN4_Object_ID_FK*=Control.ObjectID and ObjectView.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and SCRView.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and T070042.ROD_IN4_Role_ID_FK =" & Val(ddlRoles.SelectedValue) & " and T070042.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and SCRView.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and ObjectView.ROD_IN4_Role_ID_FK=T070031.ROM_IN4_Role_ID_PK and T010011.CI_NU8_Address_Number=T070031.ROM_IN4_Company_ID_FK and T070031.ROM_IN4_Company_ID_FK=" & Val(ddlCompany.SelectedValue) & " order by OrderBy  ;select  convert(varchar,RA_IN4_Role_ID_FK) RoleID,convert(varchar,RA_IN4_AB_ID_FK) UserID,convert(varchar,T060011.UM_VC50_UserID) UserName From T060022,T060011 where RA_IN4_AB_ID_FK=T060011.UM_IN4_Address_No_FK and RA_IN4_Role_ID_FK=" & Val(ddlRoles.SelectedValue) & " "

            End Select
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.Search("SecPermission", "", "", strSQL, dsSecPerm, "", "")
            dsSecPerm.Tables(0).TableName = "SecPermission"
            dsSecPerm.Tables(1).TableName = "UserNames"
            dsSecPerm.Tables(2).TableName = "MenuItems"


            For intM As Integer = 0 To dsSecPerm.Tables("MenuItems").Rows.Count - 1
                dsSecPerm.Tables("MenuItems").Rows(intM).Item("RoleID") = Val(ddlRoles.SelectedValue)
                dsSecPerm.Tables("MenuItems").Rows(intM).Item("RoleName") = ddlRoles.SelectedItem.Text.Trim
                dsSecPerm.Tables("MenuItems").Rows(intM).Item("CompName") = ddlCompany.SelectedItem.Text.Trim
                dsSecPerm.Tables("MenuItems").Rows(intM).Item("UserName") = ddlCompany.SelectedItem.Text.Trim
            Next

            For intI As Integer = 0 To dsSecPerm.Tables("MenuItems").Rows.Count - 1
                Dim dvTemp As New DataView
                dvTemp = dsSecPerm.Tables("SecPermission").DefaultView
                dvTemp = GetFilteredDataView(dvTemp, "MenuID=" & dsSecPerm.Tables("MenuItems").Rows(inti).Item("MenuID"))
                If dvTemp.Table.Rows.Count > 0 Then
                Else
                    Dim drTemp As DataRow
                    drTemp = dsSecPerm.Tables("SecPermission").NewRow
                    For intK As Integer = 0 To dsSecPerm.Tables("SecPermission").Columns.Count - 1
                        drTemp.Item(dsSecPerm.Tables("SecPermission").Columns(intK).ColumnName) = dsSecPerm.Tables("MenuItems").Rows(intI).Item(dsSecPerm.Tables("SecPermission").Columns(intK).ColumnName)
                    Next
                    dsSecPerm.Tables("SecPermission").Rows.Add(drTemp)
                End If
            Next
            dsSecPerm.Tables("SecPermission").AcceptChanges()

            Dim RoleID As String = ""
            Dim UserID As String = ""
            Dim UserName As String = ""
            Dim intJ As Integer = 1
            drNew = dsSecPerm.Tables("UserNames").NewRow
            If dsSecPerm.Tables("UserNames").Rows.Count > 0 Then
                For intI As Integer = 0 To dsSecPerm.Tables("UserNames").Rows.Count - 1
                    If intI = 0 Then
                        RoleID += dsSecPerm.Tables("UserNames").Rows(intI).Item("RoleID").ToString
                        UserID += dsSecPerm.Tables("UserNames").Rows(intI).Item("UserID").ToString
                        UserName += intJ.ToString + ". " + dsSecPerm.Tables("UserNames").Rows(intI).Item("UserName").ToString
                    Else
                        intJ += 1
                        RoleID += " , " + dsSecPerm.Tables("UserNames").Rows(intI).Item("RoleID").ToString
                        UserID += " ,  " + dsSecPerm.Tables("UserNames").Rows(intI).Item("UserID").ToString
                        UserName += " ,  " + intJ.ToString + ". " + dsSecPerm.Tables("UserNames").Rows(intI).Item("UserName").ToString
                    End If
                Next
                dsSecPerm.Tables("UserNames").Rows.Clear()
                drNew.Item("RoleID") = RoleID
                drNew.Item("UserID") = UserID
                drNew.Item("UserName") = UserName
                dsSecPerm.Tables("UserNames").Rows.Add(drNew)
            Else
                drNew.Item("RoleID") = ""
                drNew.Item("UserID") = ""
                drNew.Item("UserName") = ""
                dsSecPerm.Tables("UserNames").Rows.Add(drNew)
            End If
            'dsSecPerm.Tables("UserNames").Rows(0).Item("RoleID") = RoleID
            'dsSecPerm.Tables("UserNames").Rows(0).Item("UserID") = UserID
            'dsSecPerm.Tables("UserNames").Rows(0).Item("UserName") = UserName

            If rblSecurityPermission.SelectedValue = 0 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionMenu.rpt")
                crDocument.Load(Reportpath)

                Dim rptSecurityPermissions As New SecurityPermissionMenu
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument

            ElseIf rblSecurityPermission.SelectedValue = 1 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionScreens.rpt")
                crDocument.Load(Reportpath)

                Dim rptSecurityPermissions As New SecurityPermissionScreens
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument
            ElseIf rblSecurityPermission.SelectedValue = 2 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionsReport.rpt")
                crDocument.Load(Reportpath)
                Dim rptSecurityPermissions As New SecurityPermissionsReport
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument
            Else
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("SecurityPermissionsReport.rpt")
                crDocument.Load(Reportpath)
                Dim rptSecurityPermissions As New SecurityPermissionsReport
                crDocument.SetDataSource(dsSecPerm)
                crvSecurityPermissions.ReportSource = crDocument
            End If

            crvSecurityPermissions.EnableDrillDown = False
            mintSecPerm = 1
            'End If
        Catch ex As Exception

        End Try
    End Function
#End Region

    Private Sub imgShowReport_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgShowReport.Click
        Try
            ShowReport()
        Catch ex As Exception

        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class
