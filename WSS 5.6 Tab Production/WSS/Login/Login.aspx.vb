'***********************************************************************************************************
' Page                 :- Login 
' Purpose              :- It’s the main login screen which includes username, password, company & role.
' Tables used          :- T010011, T040041, T000011
' Date					   Author						Modification Date				Description
' 22/03/06	  	          Amandeep           			                                Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports ION.Data
Imports System.Web.HttpResponse

Partial Class Login_Login
    Inherits System.Web.UI.Page

    Private Shared intLockTry As Int32 = 1
    Dim shortFlag As Short = 0

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request.QueryString("ReturnURL")) Then
            Label1.Text = "----No Session, Please Login To Continue----"
        Else
            If IsNothing(Session("Password")) = True Then
                If Not IsNothing(Request.QueryString("Logout")) Then
                    If Val(Request.QueryString("Logout")) = 1 Then
                        'Expire the Cookie
                        Response.Cookies("WSS").Expires = Now.AddYears(-30)
                        Label1.Text = "You have Logged Out Successfully"
                        shortFlag = 1  'set Flag when we logout the WSS
                    End If
                End If
            End If


        End If
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Imagebutton1.Attributes.Add("onclick", "return SaveEdit('Submit')")
        txtUserName.Attributes.Add("onfocus", "Focus(this)")
        txtUserName.Attributes.Add("onkeydown", "DisableDDL();")
        txtPassword.Attributes.Add("onkeydown", "DisableDDL();")
        txtUserName.Attributes.Add("onblur", "GetCompany();")
        txtPassword.Attributes.Add("onblur", "GetCompany();")
        txtPassword.Attributes.Add("onfocus", "ClearInfo();")
        ddlCompany.Attributes.Add("onChange", "GetRole();")
        ddlRole.Attributes.Add("onChange", "FillHidden();")
        txtUserName.Attributes.Add("onkeypress", "Login();")
        txtPassword.Attributes.Add("onkeypress", "Login();")
        ddlCompany.Attributes.Add("onkeypress", "Login();")
        ddlRole.Attributes.Add("onkeypress", "Login();")
        'lbtnForgotpwd.Attributes.Add("Onclick", "return OpenForGotPassword();")
        Dim strError As String
        strError = CheckCompulsorySettings()
        If strError.Trim.Equals("") = False Then
            Response.Write(strError)
            placeholder1.Visible = False
            Exit Sub
        End If
        'Code when use made the cookie
        If Not IsPostBack Then
            If shortFlag = 0 Then
                If IsNothing(Request.Cookies("WSS")) = False Then
                    If Request.Cookies("WSS").Equals("") = False Then
                        If IsNothing(Request.Cookies("WSS")("UserName")) = False And IsNothing(Request.Cookies("WSS")("Password")) = False And IsNothing(Request.Cookies("WSS")("RoleId")) = False And IsNothing(Request.Cookies("WSS")("CompanyId")) = False Then
                            GetLoginDetail()
                            CheckCurrentStatus()
                            'ClientScript.RegisterStartupScript(System.Type.GetType("System.String"), "aaa", "javascript:document.getElementById('" & Imagebutton1.ClientID & "').click();", True)
                        End If
                    End If
                Else
                    If IsNothing(Session("Password")) = True Then
                        FormsAuthentication.SignOut()
                        Session.Abandon()
                    End If

                End If
            End If
        End If

        If IsPostBack Then
            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage <> Nothing Then
                Select Case txthiddenImage
                    Case "Submit"
                        Call Logon()
                        CheckCurrentStatus()
                End Select
            End If
        Else
            'If Response.Cookies.Count > 2 Then
            '    If Not Response.Cookies(2).Values(0) Is Nothing Then
            '        '  Call Logon()
            '        CheckCurrentStatus()
            '    End If

            'End If
            ' CheckCurrentStatus()
            'FormsAuthentication.SignOut()
            'Session.Abandon()
        End If

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBTracing = False
            Dim dsSystem As New Data.DataSet
            If SQL.Search("T000011", "", "", "select * from T000011", dsSystem, "", "") = True Then
                For inti As Integer = 0 To dsSystem.Tables(0).Rows.Count - 1
                    If dsSystem.Tables("T000011").Rows(inti).Item(0) = "Logo" Then
                        'get logo image  from database
                        imgLogo.ImageUrl = "../Images/" & dsSystem.Tables(0).Rows(inti).Item(1)
                    ElseIf dsSystem.Tables(0).Rows(inti).Item(0) = "PunchLineTitle" Then
                        'get PunchLineTitle  from database
                        lblPunchLineTitle.Text = dsSystem.Tables(0).Rows(inti).Item(1)
                    ElseIf dsSystem.Tables(0).Rows(inti).Item(0) = "PunchLineSubTitle" Then
                        'get PunchLineSubTitle  from database
                        lblPunchLineSubTitle.Text = dsSystem.Tables(0).Rows(inti).Item(1)
                    ElseIf dsSystem.Tables(0).Rows(inti).Item(0) = "AccountHeading" Then
                        'get AccountHeading  from database
                        lblAccountHeading.Text = dsSystem.Tables(0).Rows(inti).Item(1)
                    End If
                Next
                dsSystem.Clear()
            End If

        Catch ex As Exception
            CreateLog("Login", "Load-49", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Private Sub CheckCurrentStatus()
        Try
            If Val(Request.QueryString("CallNo")) > 0 Then
                If Val(Request.QueryString("TaskNo")) > 0 Then
                    Session("PropCallNumber") = Val(Request.QueryString("CallNo"))
                    Session("PropCAComp") = Val(Request.QueryString("CompID"))
                    Session("PropTaskNumb") = Val(Request.QueryString("TaskNo"))
                    Dim strUserID As String = SQL.Search("", "", "select UM_VC50_USERID  from T060011 where UM_IN4_ADDRESS_NO_FK=(select TM_VC8_SUPP_OWNER from T040021 where TM_NU9_CALL_NO_FK='" & Session("PropCallNumber") & "' and TM_NU9_COMP_ID_FK='" & Session("PropCAComp") & "' and TM_NU9_TASK_NO_PK='" & Session("PropTaskNumb") & "')", )
                    If strUserID = HttpContext.Current.Session("PropUserName") Then
                        Response.Redirect("../dashboard.aspx?CallNo=" & Val(Request.QueryString("CallNo")) & "&CompId=" & Val(Request.QueryString("CompID")) & "&TaskNo=" & Val(Request.QueryString("TaskNo")), False)
                    Else
                        Session("PropCallNumber") = Val(Request.QueryString("CallNo"))
                        Session("PropCAComp") = Val(Request.QueryString("CompID"))
                        'Response.Redirect("../IndexMail.htm", False)
                        Response.Redirect("../dashboard.aspx?CallNo=" & Val(Request.QueryString("CallNo")) & "&CompId=" & Val(Request.QueryString("CompID")) & "&TaskNo=" & Val(Request.QueryString("TaskNo")), False)
                    End If
                Else
                    Session("PropCallNumber") = Val(Request.QueryString("CallNo"))
                    Session("PropCAComp") = Val(Request.QueryString("CompID"))
                    Response.Redirect("../dashboard.aspx?CallNo=" & Val(Request.QueryString("CallNo")) & "&CompId=" & Val(Request.QueryString("CompID")), False)
                    'Response.Redirect("../IndexMail.htm", False)
                End If
            Else
                'Response.Redirect("../index.htm", False)
                Response.Redirect("../dashboard.aspx", False)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Logon()
        Try
            FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.Session("PropUserID"), False)
            'To delete temprary attachment file 
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.Delete("", "", "delete from T040041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & "", SQL.Transaction.Serializable)

            'To delete Temprary Comments
            SQL.Delete("", "", "delete from T040061  Where CM_VC2_Flag='C' and CM_NU9_Call_Number=0 and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0   and CM_NU9_CompId_Fk=" & Val(Session("PropCompanyID")) & " and CM_NU9_AB_Number=" & Session("PropUserID") & "", SQL.Transaction.Serializable)


            Dim objCls As New clsSecurityCache
            Dim dtData As Data.DataTable
            If HttpContext.Current.Session("PropUserName") = "" Then
                Response.Write("User name not initialized")
                Exit Sub
            Else
                objCls.FillCache()
            End If
            HttpContext.Current.Session("PropRoleName") = txtRoleName.Value.Trim
            HttpContext.Current.Session("PropDataFormat") = ConfigurationSettings.AppSettings("DataFormat")
            HttpContext.Current.Session("PropDataTimeFormat") = ConfigurationSettings.AppSettings("DataTimeFormat")
            Call SaveIPInfo()
            'code added to save the value of userId and role Id in  Cookies
            If ChkRember.Checked = True Then
                Dim newCookie As HttpCookie = New HttpCookie("WSS")
                Response.Cookies("WSS").Value = "abc"
                newCookie.Values.Add("UserName", Session("PropUserName"))
                newCookie.Values.Add("Password", Session("Password"))
                newCookie.Values.Add("RoleId", txtRole.Value.Trim)
                newCookie.Values.Add("CompanyId", txtCompName.Value.Trim)
                newCookie.Values.Add("RoleName", txtRoleName.Value.Trim)
                newCookie.Expires = DateTime.Now.AddYears(30)
                Response.Cookies.Add(newCookie)
            End If

        Catch ex As Exception
            CreateLog("Login", "Logon-156", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Function SaveIPInfo() As Boolean
        Try
            Dim arrColName As New ArrayList
            Dim arrRowData As New ArrayList
            arrColName.Add("IP_NU9_Comp_ID_FK")
            arrColName.Add("IP_NU9_User_ID_FK")
            arrColName.Add("IP_NU9_Role_ID_FK")
            arrColName.Add("IP_VC36_System_IP")
            arrColName.Add("IP_DT8_Login_Date")
            arrRowData.Add(Session("PropCompanyID"))
            arrRowData.Add(Session("PropUserID"))
            arrRowData.Add(Session("PropRole"))
            arrRowData.Add(Request.UserHostAddress)
            arrRowData.Add(Now)
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.Save("T060031", "Login", "SaveIPInfo", arrColName, arrRowData)
        Catch ex As Exception
            CreateLog("Login", "SaveIPInfo-164", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try


    End Function

    Private Function GetLoginDetail() As Boolean

        'HttpContext.Current.Session("Password") = strPassword

        HttpContext.Current.Session("PropUserID") = WSSSearch.SearchUserIdByName(Request.Cookies("WSS")("UserName"), Val(Request.Cookies("WSS")("CompanyId"))).ExtraValue
        HttpContext.Current.Session("PropUserName") = Request.Cookies("WSS")("UserName")
        HttpContext.Current.Session("PropRole") = Request.Cookies("WSS")("RoleId")
        HttpContext.Current.Session("PropCompanyID") = Val(Request.Cookies("WSS")("CompanyId"))
        HttpContext.Current.Session("PropCompanyType") = SQL.Search("", "", "select CI_IN4_Business_Relation from T010011 where  CI_NU8_Address_Number=" & Request.Cookies("WSS")("CompanyId"))
        HttpContext.Current.Session("PropCompany") = WSSSearch.SearchUserName(HttpContext.Current.Session("PropCompanyID").ToString).ExtraValue()
        HttpContext.Current.Session("PropAdmin") = SQL.Search("", "", "SELECT UM_CH1_Admin_Rights  FROM T060011 WHERE UM_IN4_Address_No_FK = '" & HttpContext.Current.Session("PropUserID") & "'")

        Dim objCls As New clsSecurityCache
        Dim dtData As Data.DataTable
        If HttpContext.Current.Session("PropUserName") = "" Then
            Response.Write("User name not initialized")
            Exit Function
        Else
            objCls.FillCache()
        End If
        HttpContext.Current.Session("PropRoleName") = Request.Cookies("WSS")("RoleName")
        HttpContext.Current.Session("PropDataFormat") = ConfigurationSettings.AppSettings("DataFormat")
        HttpContext.Current.Session("PropDataTimeFormat") = ConfigurationSettings.AppSettings("DataTimeFormat")
        FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.Session("PropUserID"), False)

        If Val(Request.QueryString("CallNo")) > 0 Then
            Session("PropCallNumber") = Val(Request.QueryString("CallNo"))
            Session("PropCAComp") = Val(Request.QueryString("CompID"))
            Response.Redirect("../IndexMail.htm", False)
        Else
            'Response.Redirect("../index.htm", False)
            Response.Redirect("../dashboard.aspx", False)
        End If
    End Function

    Protected Sub lbtnForgotpwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnForgotpwd.Click
        Response.Redirect("ForGotPassword.aspx", False)
        Session("Password") = 1
    End Sub
End Class