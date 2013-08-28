'**********************************************************************************************************
' Page                   : -ShowCallTaskForm
' Purpose                : -Purpose of this screen is to show the form associated with specific task of a                                call.  
' Tables used            : -T010011, T050021, T040011, T110022, T050071, T100011
' Date		    		Author						Modification Date					Description
' 25/03/06				Jaswinder				    ----------------	        		Created
'
''*********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class ChangeManagement_showCallTaskForm
    Inherits System.Web.UI.Page
    Dim compname As String
    Dim strConnection As String

#Region "global level declaration"

    Protected Shared mdvtable As DataView = New DataView

    Dim rowvalue As Integer
    Dim callType As String
    Dim taskType As String
    Dim compID As String

#End Region

#Region "Page Load Code"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        'Security Block
        Dim intid As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = 262
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If

        'End of Security Block

        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)

        compID = HttpContext.Current.Session("PropCompanyID")
        cpnlErrorPanel.Visible = False
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        Dim txthiddenImage = Request.Form("txthiddenImage")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        If Request.Form("txthidden") = "" Then
        Else
            ViewState("SFormID") = Request.Form("txthidden")
            ViewState("SUser") = Request.Form("txthiddenUser")
            ViewState("SRole") = Request.Form("txthiddenRole")
            ViewState("SCompany") = Request.Form("txthiddenCompany")
        End If

        If Not IsPostBack Then
            If Val(Request.QueryString("OPT")) = 2 Then
                txtCallNo.Text = Request.QueryString("cno")
                txtTaskNo.Text = Request.QueryString("tno")
                Session("propTaskNumber") = Request.QueryString("tno")

                callType = getTypes(taskType)
            Else
                compname = Request.QueryString("compid")

                If Not compname Is Nothing Then
                    ViewState("CompanyID") = getCompID(compname)
                End If
                txtCallNo.Text = Request.QueryString("cno")
                txtTaskNo.Text = Request.QueryString("tno")
                compID = HttpContext.Current.Session("PropCompanyID")

                If Not (Request.QueryString("ct") Is Nothing And Request.QueryString("tt") Is Nothing) Then
                    callType = Request.QueryString("ct")
                    taskType = Request.QueryString("tt")
                Else
                    callType = getTypes(taskType)
                End If
            End If

            fillGrid()
        End If

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        Response.Write("<script>window.close();</script>")
                    Case "Edit"
                        ViewState("SFormID") = ""
                    Case "Logout"
                        LogoutWSS()
                End Select

            Catch ex As Exception
                'Dim str As String = ex.ToString
                CreateLog("showCallTaskForm", "Load-111", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If
    End Sub

#End Region
    Private Function getCompID(ByVal compname As String) As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T040011"
            SQL.DBTracing = False

            compname = SQL.Search("ShowCallTaskForm", "getCompID-155", "select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & compname & "' and CI_VC8_Address_Book_Type='COM'")
            Return compname
        Catch ex As Exception

            CreateLog("ShowCallTaskForm", "getCompID-160", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function getTypes(ByRef taskType As String) As String
        Dim callType As String
        Dim dsForms As New DataSet
        strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        ' Table name
        If Request.QueryString("OPT") = 2 Then
            ' SQL.DBTable = "T050021"
            SQL.DBTracing = False

            Try
                SQL.Search("T050021", "showCallTaskForm", "getTypes", "select TL_VC8_Call_Type,TTM_VC8_task_type from T050011,T050031 where TTM_NU9_TemplateID_FK=TL_NU9_ID_PK and TL_NU9_ID_PK=" & HttpContext.Current.Session("SAddressNumber_Template") & " and TTM_NU9_Task_no_PK=" & txtTaskNo.Text.Trim & " and TL_NU9_CustId_FK=TTM_NU9_Comp_ID_FK and TTM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), dsForms, "", "")
                callType = dsForms.Tables("T050021").Rows(0).Item(0)
                taskType = dsForms.Tables("T050021").Rows(0).Item(1)

            Catch ex As Exception
                CreateLog("showCallTaskForm", "getTypes-185", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else
            SQL.DBTracing = False
            Try

                SQL.Search("T040011", "showCallTaskForm", "getTypes-196", "select CM_VC8_Call_Type,TM_VC8_task_type from T040011,T040021 where TM_NU9_Call_No_FK=CM_NU9_Call_No_PK and TM_NU9_Call_No_FK=" & txtCallNo.Text.Trim & " and TM_NU9_Task_no_PK=" & txtTaskNo.Text.Trim & " and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), dsForms, "", "")

                callType = dsForms.Tables("T040011").Rows(0).Item(0)
                taskType = dsForms.Tables("T040011").Rows(0).Item(1)

            Catch ex As Exception
                CreateLog("showCallTaskForm", "FillGrid-154", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If
        Return callType
    End Function

#Region "Grid functions"

    Private Sub fillGrid()
        Dim dsForms As New DataSet
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConnection
        SQL.DBTracing = False

        Try
            If Val(Request.QueryString("OPT")) = 2 Then
                SQL.Search("T110022", "showCallTaskForm", "fillGrid", "(select FT_VC100_form_name Form_Name from T110022 where FT_VC8_Call_Type='" & callType & "' and FT_VC8_Task_Type='" & taskType & "' and FT_IN4_Comp_id=" & ViewState("CompanyID") & ") union (select FD_VC50_Call_Form_Name Form_Name  from T050071 where FD_IN4_Call_no=" & txtCallNo.Text.Trim & " and FD_IN4_Task_no=" & txtTaskNo.Text.Trim & " and FD_IN4_Comp_id=" & ViewState("CompanyID") & ")", dsForms, "", "")
            Else
                SQL.Search("T110022", "showCallTaskForm", "fillGrid", "(select FT_VC100_form_name Form_Name from T110022 where FT_VC8_Call_Type='" & callType & "' and FT_VC8_Task_Type='" & taskType & "' and FT_IN4_Comp_id=" & ViewState("CompanyID") & ") union (select FD_VC50_Call_Form_Name Form_Name from T100011 where FD_IN4_Call_no=" & txtCallNo.Text.Trim & " and FD_IN4_Task_no=" & txtTaskNo.Text.Trim & " and FD_IN4_Comp_id=" & ViewState("CompanyID") & ")", dsForms, "", "")
            End If

            mdvtable.Table = dsForms.Tables("T110022")
            GrdAddSerach.DataSource = mdvtable.Table
            GrdAddSerach.DataBind()

        Catch ex As Exception
            CreateLog("showCallTaskForm", "FillGrid-154", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Private Sub dgFormAssign_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound
        Dim strID As String
        GrdAddSerach.Columns.Clear()
        Dim rowFlag As Boolean
        rowFlag = True
        Dim cnt As Integer
        Try
            For cnt = 0 To e.Item.Cells.Count - 1
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)

                    e.Item.Cells(cnt).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(cnt).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "')")
                    e.Item.Cells(cnt).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "')")

                    If rowFlag Then
                        If checkForm(strID, Val(Request.QueryString("OPT"))) Then
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Visible = True
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).ImageUrl = "../images/Form1.jpg"
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "')")

                        Else
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).ImageUrl = "../images/Form2.gif"
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Visible = True
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "')")
                        End If

                    End If
                End If
                rowFlag = False
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("showCallTaskForm", "ItemDatabound-179", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try
    End Sub

    Private Function checkForm(ByVal formName As String, ByVal opt As Integer) As Boolean
        Dim dtForm As New DataSet
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBConnection = strConnection
        SQL.DBTracing = False

        Dim strQuery As String

        If opt = 2 Then
            strQuery = "Select * from T050071 where FD_IN4_Call_no=" & txtCallNo.Text.Trim & " and FD_IN4_Task_no=" & Session("propTaskNumber") & " and FD_VC50_Call_form_Name='" & formName & "' and FD_IN4_Comp_id=" & ViewState("CompanyID")
        Else
            strQuery = "Select * from T100011 where FD_IN4_Call_no=" & txtCallNo.Text.Trim & " and FD_IN4_Task_no=" & txtTaskNo.Text.Trim & " and FD_VC50_Call_form_Name='" & formName & "' and FD_IN4_Comp_id=" & ViewState("CompanyID")
        End If

        Try
            If SQL.Search("T100011", "showCallTaskForm", "checkForm", strQuery, dtForm, "", "") Then
                If dtForm.Tables("T100011").Rows.Count > 0 Then
                    Return True
                Else : Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("showCallTaskForm", "checkForm-1127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

#End Region

End Class
