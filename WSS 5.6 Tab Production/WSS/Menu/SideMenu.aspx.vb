'*******************************************************************
' Page                 : - Side Menu
' Purpose              : - its purpose is to create the Menu for all screens
' Tables used          : T070011 
' Date					Author						Modification Date					Description
' 17/03/06				Amandeep					-------------------					Created

' Notes: 
' Code:
'*******************************************************************
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports ION.Data
Imports Microsoft.Web.UI.WebControls
Imports System.Data
Partial Class Menu_SideMenu
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '----------------------------------------
        'Store the DateTime stamp in session+

        If Not Page.IsPostBack Then
            Session("FP_F5") = Server.UrlEncode(System.DateTime.Now.ToString())
        Else
            HttpContext.Current.Session("PropRole") = ddlRoleName.SelectedValue
        End If
        '-----------------------------------------
        lblCompLogo.Text = HttpContext.Current.Session("PropCompany")
        HIDUserName.Value = HttpContext.Current.Session("PropUserName")
        HIDRoleID.Value = HttpContext.Current.Session("PropRole")
        HIDNowDate.Value = Now.Date
        txtFastPath.Attributes.Add("onkeypress", "Submit()")
        Try
            Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
            lblUserID.Text = "Welcome <b>" & HttpContext.Current.Session("PropUserName") & "</b>"
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1


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
                        Response.Redirect("../Login/Login.aspx?Logout=1", False)
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
            lblEnv.Text = "&nbsp;" & Request.ServerVariables("APPL_MD_PATH").Substring(Request.ServerVariables("APPL_MD_PATH").LastIndexOf("/") + 1)

        Catch ex As Exception
            CreateLog("SideMenu", "Load-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        'Date:----- 8/12/2006 
        '-----------------------------------------------
        'Assign the Session Value to Viewstate
        ViewState("FP_F5") = Session("FP_F5")
        '-----------------------------------------------

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
        Response.Write("<script>window.open('../Home.aspx','MainPage')</script>")
    End Sub
End Class

