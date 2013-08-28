Imports System.Web.UI.HtmlControls
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports ION.Data
Imports Microsoft.Web.UI.WebControls
Imports System.Data
Partial Class DashBoard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'To Redirect to Login Page if the Session is timeOut
        If (Session("PropUserID") Is Nothing Or Session("PropUserName") Is Nothing) Then
            FormsAuthentication.SignOut()
            HttpContext.Current.Session.Abandon()
            Response.Redirect("Login/Login.aspx?Logout=1", False)
        End If
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str((Val(HttpContext.Current.Session.Timeout) * 60) - 10) & ";URL=Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        '----------------------------------------
        'Store the DateTime stamp in session+

        If Not Page.IsPostBack Then
            Session("FP_F5") = Server.UrlEncode(System.DateTime.Now.ToString())
        Else
            HttpContext.Current.Session("PropRole") = ddlRoleName.SelectedValue
        End If
        '-----------------------------------------

        'lblCompLogo.Text = HttpContext.Current.Session("PropCompany")
        HIDUserName.Value = HttpContext.Current.Session("PropUserName")
        HIDRoleID.Value = HttpContext.Current.Session("PropRole")
        HIDNowDate.Value = Now.Date
        txtFastPath.Attributes.Add("onkeypress", "Submit()")


        Try
            'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
            'lblUserID.Text = "Welcome <b>" & HttpContext.Current.Session("PropUserName") & "</b>"
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1
            lblUserID.Text = "Welcome <b>" & HttpContext.Current.Session("PropUserName") & "</b>"


            Dim strSQL As String
            Dim strScreenURL As String
            Dim intScrID As Integer
            If txtFastPath.Text <> "" Then

                '-----------------------------------------------
                'Date:--- 8/12/2006
                'Compare the Viewstate DateTime stamp with Session DateTime stamp
                '------------------------------------------------
                If Session("FP_F5").ToString() = ViewState("FP_F5").ToString() Then
                    If txtFastPath.Text.ToUpper = "LG" Then
                        FormsAuthentication.SignOut()
                        HttpContext.Current.Session.Abandon()
                        Response.Redirect("Login/Login.aspx?Logout=1", False)
                    Else
                        strSQL = "select OBM_IN4_Object_ID_PK, OBM_VC200_URL, OBM_VC50_Alias_Name, OBM_IN4_Object_PID_FK from T070011 where OBM_VC8_FPath='" & txtFastPath.Text & "'"
                        Dim sqlRdr As SqlClient.SqlDataReader
                        Dim blnStatus As Boolean
                        sqlRdr = SQL.Search("sidemenu", "Page_Load", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "NA")
                        If blnStatus = True Then
                            sqlRdr.Read()

                            strScreenURL = sqlRdr.Item("OBM_VC200_URL")
                            intScrID = sqlRdr("OBM_IN4_Object_ID_PK")

                            If strScreenURL.IndexOf("?") = -1 Then
                                Response.Write("<script>window.open('" & strScreenURL & "?ScrID=" & intScrID & "','MainPage')</script>")
                            Else
                                Response.Write("<script>window.open('" & strScreenURL & "&ScrID=" & intScrID & "','MainPage')</script>")
                            End If
                            txtFastPath.Text = ""
                            sqlRdr.Close()
                        End If
                    End If
                    '-----------------------------------------------------------
                    'Reset the session after redirecting from the Fast Path
                    Session("FP_F5") = Server.UrlEncode(System.DateTime.Now.ToString())
                    '-----------------------------------------------------------
                Else
                    txtFastPath.Text = ""
                End If
            Else

            End If
            Dim strSelectedScreenFP As String
            Dim strSelectedMenuFP As String
            If Not IsNothing(System.Web.HttpContext.Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
                Dim dtMenu As DataTable
                'Get the menu datatable from cache
                dtMenu = System.Web.HttpContext.Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")
                Dim dvTemp As DataView
                Dim dvTemp1 As DataView
                dvTemp = dtMenu.DefaultView
                'Get the record against screen id of the fast path screen
                GetFilteredDataView(dvTemp, "ObjectID=" & intScrID)
                If dvTemp.Table.Rows.Count > 0 Then
                    strSelectedScreenFP = dvTemp.Table.Rows(0).Item(2)
                    dvTemp1 = dtMenu.DefaultView
                    'Get parent id of the fast path screen
                    GetFilteredDataView(dvTemp1, "ObjectID=" & dvTemp.Table.Rows(0).Item(3))
                    If dvTemp1.Table.Rows.Count > 0 Then
                        strSelectedMenuFP = dvTemp1.Table.Rows(0).Item(2)
                    End If
                End If
            End If
            Dim objMenu As New clsSecurityCache

            objMenu.FillObjectData(pnlMenu, strSelectedMenuFP, strSelectedScreenFP)
            objMenu = Nothing
            GetUserRoles()
            'lblEnv.Text = "&nbsp;" & Request.ServerVariables("APPL_MD_PATH").Substring(Request.ServerVariables("APPL_MD_PATH").LastIndexOf("/") + 1)
            If (True) Then
                'ClientScript.RegisterStartupScript(Me.GetType(), "openTab", "enableTab1('google','http://www.google.com');", True)
                'Iframe0.Attributes.Add("src", "http://www.google.com")
            End If
            If Val(Request.QueryString("CallNo")) > 0 Then
                If Val(Request.QueryString("TaskNo")) > 0 Then
                    Dim strUserID As String = SQL.Search("", "", "select UM_VC50_USERID  from T060011 where UM_IN4_ADDRESS_NO_FK=(select TM_VC8_SUPP_OWNER from T040021 where TM_NU9_CALL_NO_FK='" & Session("PropCallNumber") & "' and TM_NU9_COMP_ID_FK='" & Session("PropCAComp") & "' and TM_NU9_TASK_NO_PK='" & Session("PropTaskNumb") & "')", )
                    If strUserID = HttpContext.Current.Session("PropUserName") Then
                        'Response.Redirect("../indexTaskMail.htm", False)
                        ClientScript.RegisterStartupScript(Me.GetType(), "openTab", "openPageFromMail('To Do List');", True)
                        Iframe0.Attributes.Add("src", "WorkCenter/DoList/ToDoList.aspx")
                    Else
                        Session("PropCallNumber") = Val(Request.QueryString("CallNo"))
                        Session("PropCAComp") = Val(Request.QueryString("CompID"))
                        'Response.Redirect("../IndexMail.htm", False)
                        ClientScript.RegisterStartupScript(Me.GetType(), "openTab", "openPageFromMail('Call #" + Val(Request.QueryString("CallNo")).ToString() + "');", True)
                        Iframe0.Attributes.Add("src", "SupportCenter/CallView/Call_detail.aspx?CallNo=" & Val(Request.QueryString("CallNo")) & "&CompId=" & Val(Request.QueryString("CompID")))
                    End If
                Else
                    'Session("PropCallNumber") = Val(Request.QueryString("CallNo"))
                    'Session("PropCAComp") = Val(Request.QueryString("CompID"))
                    ClientScript.RegisterStartupScript(Me.GetType(), "openTab", "openPageFromMail('Call #" + Val(Request.QueryString("CallNo")).ToString() + "');", True)
                    Iframe0.Attributes.Add("src", "SupportCenter/CallView/Call_detail.aspx?CallNo=" & Val(Request.QueryString("CallNo")) & "&CompId=" & Val(Request.QueryString("CompID")))
                End If
            End If

        Catch ex As Exception
            CreateLog("SideMenu", "Load-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
        'lblEnv.Text = "&nbsp;" & Request.ServerVariables("APPL_MD_PATH").Substring(Request.ServerVariables("APPL_MD_PATH").LastIndexOf("/") + 1)

    End Sub
    Private Function GetUserRoles() As Boolean
        Dim dstable As New DataSet
        Dim strSql As String
        Dim strCompID As String
        Dim strUserID As String
        Try
            strCompID = Val(Session("PropCompanyID"))
            strUserID = Session("PropUserName")

            strSql = "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_VC50_Status_Code_FK='ENB' and ROM_DT8_End_Date>='" & Today & "' and  ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_VC50_UserID='" & strUserID & "' " & "and UM_IN4_Company_AB_ID=" & strCompID & ") and RA_DT8_Assigned_Date <='" & Today & "' and RA_DT8_Valid_UpTo >='" & Today & "' and RA_VC4_Status_Code ='ENB') "
            If SQL.Search("T070031", "AjaxInfo", "GetInfo", strSql, dstable, "", "") = True Then
                ddlRoleName.DataSource = dstable.Tables(0).DefaultView
                ddlRoleName.DataValueField = "ID"
                ddlRoleName.DataTextField = "Name"
                ddlRoleName.DataBind()
                ddlRoleName.SelectedValue = HttpContext.Current.Session("PropRole")
            End If
        Catch ex As Exception
        End Try
    End Function
    Private Sub ddlRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRoleName.SelectedIndexChanged
        Session("PropRole") = ddlRoleName.SelectedValue
        Session("PropRoleID") = Session("PropRole")
        Dim obj As New clsSecurityCache
        obj.FillCache()
        obj.FillObjectData(pnlMenu, "", "")
        obj = Nothing
        'Response.Write("<script>window.open('../Home.aspx','MainPage')</script>")
        Response.Redirect("dashboard.aspx")
    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Date:----- 8/12/2006 
        '-----------------------------------------------
        'Assign the Session Value to Viewstate
        ViewState("FP_F5") = Session("FP_F5")
        '-----------------------------------------------
    End Sub
End Class

