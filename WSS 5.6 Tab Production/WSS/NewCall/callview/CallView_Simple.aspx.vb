Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI


Partial Class NewWork_CallView_Simple
    Inherits System.Web.UI.Page
    Private mdvtable As New DataView  ' store data from table for view grid 
    Private compColumnNo As String
    Private suppCompColumnNo As String
    Private callOwnerColumnNo As String
    Private byWhomColumnNo As String
    Private callNoColumnNo As String
    Private coordinatorColumnNo As String
    Private relatedCallColumnNo As String
    Private intValue As Integer = 0
    Private intVal As Integer = 0
    Protected controlCollection As Dictionary(Of String, String) = New Dictionary(Of String, String)
    Private txthiddenImage As String 'stored clicked button's cation  
    Private intComp As String
    Public mstrcomp As String
    'Dim mblnValue As Boolean
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Public mstrCallNumber As String
    Public introwvalues As Integer 'stored the selected row's value

    ''' <summary>
    ''' Page Load Event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        'Security Block
        '****************************************
        ' Dim intId As Integer
        If Request.QueryString("ScreenFrom") = "HomePage" Then
            imgClose.Visible = False
        End If
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            '   intId = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(2194) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, 2194)
        End If
        'End of Security Block
        '*****************************************


        imgBtnViewPopup.Attributes("onclick") = [String].Format("return ShowSimpleCallViewsForm();")
        txthiddenImage = Request.Form("txthiddenImage")
        introwvalues = Request.Form("txtrowvalues")

        strhiddenTable = Request.Form("txthiddenTable")
        If strhiddenTable = "cpnlCallTask_dtgTask" Then
            'HttpContext.Current.session("TaskNumber") = Val(Request.Form("txthiddenTaskNo"))
            ViewState("TaskNumber") = Val(Request.Form("txthiddenTaskNo"))
        Else
            ' Clear all textBoxes in fastentry if Task no. is changed and currently we have clicked on Task grid
            If Val(Request.Form("txthiddenCallNo")) <> 0 And Val(ViewState("CallNo")) <> Val(Request.Form("txthiddenCallNo")) Then
            End If
            'HttpContext.Current.session("CallNumber") = Val(Request.Form("txthiddenCallNo"))
            ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
            mstrCallNumber = ViewState("CallNo") 'HttpContext.Current.viewstate("CallNo")
        End If
        If Not Page.IsPostBack Then
            'Fill the ddlstview i.e to Load views for Call View page 
            If (Session("PropRole") Is Nothing) Then
                LogoutWSS()
            End If
            GrdAddSerach.GroupingSettings.CaseSensitive = False
            'Fill the ddlstview i.e to Load views for Call View page 
            GetView()
            ChkSelectedView()

            Dim arColWidth As New ArrayList
            Dim arrTextboxId As New ArrayList
            Dim arrColumnsName As New ArrayList
            ViewState.Add("arColWidth", arColWidth)
            ViewState.Add("arrTextboxId", arrTextboxId)
            ViewState.Add("arrColumnsName", arrColumnsName)
            'Session("CallPlus") = "1"
            ViewState("CallPlus") = "1"
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "View"
                        introwvalues = 0
                        'filling session variables on combo change event
                        'session("CallViewsimpleName") = ddlstview.SelectedItem.Text
                        'session("CallViewSimpleValue") = ddlstview.SelectedItem.Value
                        ViewState("CallViewsimpleName") = ddlstview.SelectedItem.Text
                        ViewState("CallViewSimpleValue") = ddlstview.SelectedItem.Value
                        If Session("Flag") = "1" Then
                            GetView()
                            Session("Flag") = 0
                        Else
                            SaveUserView()
                        End If
                        ViewState("CallNo") = 0
                    Case "MyCall"
                        If ViewState("MyCall") = "ALL" Then
                            ViewState("MyCall") = "MY"
                            imgMyCall.ToolTip = "Show All Calls"
                            'cpnlCallView.Text = "Call View :  My Calls"
                        Else
                            ViewState("MyCall") = "ALL"
                            imgMyCall.ToolTip = "Show My Calls"
                            'cpnlCallView.Text = "Call View :  All Calls"
                        End If
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                    Case "Edit"
                        If strhiddenTable = "cpnlCallTask_dtgTask" Then
                            Exit Select
                        Else
                            Dim callNo As Integer = ViewState("CallNo")
                            Dim compID As String = Session("PropCompanyID")
                            '  Response.Redirect("../../Supportcenter/CallView/Call_Detail.aspx?CallNumber=" & ViewState("CallNo") & "&compID=" & Session("PropCompanyID") & "", False)
                        End If

                    Case "Add"
                        ViewState("CallNo") = 0
                        'Response.Redirect("Call_Detail.aspx?ScrID=3&ID=-1", False)
                    Case "Select"
                        'shflagSel = 1
                        '   cpnlCallTask.Enabled = True
                    Case "CloseCall"
                        If ViewState("CVSmshCall") = 0 Then
                            ViewState("CVSmshCall") = 1
                        Else
                            ViewState("CVSmshCall") = 0
                        End If
                        mstrCallNumber = "0"
                        ViewState("CallNo") = "0"
                    Case "Save"
                    Case "Attach"
                        Response.Write("<script>window.open('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=550,status=yes');</script>")
                End Select
            Catch ex As Exception
                CreateLog("Call_View", "Load-286", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If

    End Sub
    Private Function ChkPageView() As Boolean
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=1024 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

            If blnReturn = False Then
                Return False
                Exit Function
            Else
                While sqdrCol.Read
                    ViewState("PageSize") = sqdrCol.Item("PS_NU9_PSize")
                End While
                Return True
            End If

            sqdrCol.Close()
            sqdrCol = Nothing

        Catch ex As Exception
            ddlstview.SelectedValue = 0
            CreateLog("Call+", "ChkSelectedView-2080", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Protected Sub ddlstview_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged
        ViewState("CallViewsimpleName") = ddlstview.SelectedItem.Text
        ViewState("CallViewSimpleValue") = ddlstview.SelectedItem.Value
        SaveUserView()
        GrdAddSerach.MasterTableView.SortExpressions.Clear()
        'GrdAddSerach.Columns.Clear()
        GrdAddSerach.Rebind()
        ''viewstate("CallNo") = 0
    End Sub

    ''' <summary>
    ''' A Procedure to Fill Dropdown of Views
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetView()
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        ddlstview.Items.Clear()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            sqrdView = SQL.Search("CallView", "GetView-1047", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='1024' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then
                ddlstview.DataSource = sqrdView
                ddlstview.DataTextField = "UV_VC50_View_Name"
                ddlstview.DataValueField = "UV_IN4_View_ID"
                ddlstview.DataBind()
                sqrdView.Close()
            End If

            If ViewState("CallViewsimpleName") = "" Or ViewState("CallViewsimpleName") = "Default" Then
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
                ddlstview.SelectedIndex = ddlstview.Items.Count - 1
            Else
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
            End If

            If ViewState("CallViewsimpleName") <> "" And ViewState("CallViewsimpleName") <> "Default" Then
                ddlstview.SelectedValue = ViewState("CallViewSimpleValue")
            End If

        Catch ex As Exception
            CreateLog("Call_View", "GetView-1050", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub SaveUserView()
        Dim intid = 1024 ' screen id for call view screen
        Dim intcount As Integer

        Dim strCheck As String = SQL.Search("Historicview", "SaveUserView-3406", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID='" & intid & "' and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "")

        If Not IsNothing(strCheck) Then
            'update
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("UV_VC50_View_Name")
            arColumnName.Add("UV_IN4_View_ID")

            arRowData.Add(ddlstview.SelectedItem.Text.Trim)
            arRowData.Add(ddlstview.SelectedValue.Trim)

            If SQL.Update("T030213", "SaveUserView", "update  T030213 set UV_IN4_View_ID=" & ddlstview.SelectedValue.Trim & ", UV_VC50_View_Name='" & ddlstview.SelectedItem.Text.Trim & "' where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID='" & intid & "' and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid"), SQL.Transaction.Serializable) = True Then
                'Save message
            Else
                'Error message
            End If

        Else
            'save
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("UV_VC50_View_Name")
            arColumnName.Add("UV_IN4_View_ID")
            arColumnName.Add("UV_VC50_SCREEN_ID")
            arColumnName.Add("UV_IN4_Role_ID")
            arColumnName.Add("UV_NU9_Comp_ID")
            arColumnName.Add("UV_NU9_User_ID") 'Added new field to store user id with view records


            arRowData.Add(ddlstview.SelectedItem.Text.Trim)
            arRowData.Add(ddlstview.SelectedValue.Trim)
            arRowData.Add(intid)
            arRowData.Add(Session("PropRole"))
            arRowData.Add(Session("PropCompanyID"))
            arRowData.Add(Session("PropUserID"))

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Save("T030213", "SaveUserView", "SaveUserView-3436", arColumnName, arRowData) = True Then
            End If

        End If
    End Sub

    Private Sub ChkSelectedView()
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=1024 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

        If blnReturn = False Then
            ViewState("CallViewsimpleName") = "Default"
            ViewState("CallViewSimpleValue") = "0"
            Exit Sub
        Else
            While sqdrCol.Read
                ViewState("CallViewsimpleName") = sqdrCol.Item("UV_VC50_View_Name")
                ViewState("CallViewSimpleValue") = sqdrCol.Item("UV_IN4_View_ID")
                ddlstview.SelectedValue = ViewState("CallViewSimpleValue")
            End While
            sqdrCol.Close()
        End If
    End Sub
    Private Function fillDefault() As DataTable
        Try
            '*************************
            Dim IntNewCountDef As Integer
            Dim dsDefault As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            '**************
            ' Dim arSetColumnName As New ArrayList
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select distinct "
            Dim strwhereQuery As String = " and "

            Dim strQuery As String

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
              & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
              & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =1024 And " _
              & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
              & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
              & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
              & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & Val(HttpContext.Current.Session("PropRole")) & " AND " _
              & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=1024 and obm_vc4_object_type_fk='VIW') " _
              & " order by OBM.OBM_SI2_Order_By"

            '  SQL.DBTable = "T070042"
            sqrdView = SQL.Search("CallView", "Filldefault-502", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)

            Dim shJoin As Short

            CType(ViewState("arrColumnsName"), ArrayList).Clear()
            CType(ViewState("arColWidth"), ArrayList).Clear()


            If blnView = True Then
                Dim rowcount As Int32
                Dim htDateCols As New Hashtable

                While sqrdView.Read
                    If sqrdView.Item("OBM_VC200_URL") = "CM_VC100_By_Whom" Then
                        strSelect &= "ByWhom." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Call_Owner" Then
                        strSelect &= "Owner." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Comp_Id_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                        shJoin += 4
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_CustID_FK" Then
                        strSelect &= "Suppcomp." & "CI_VC36_Name" & ","
                        ' shJoin += 4
                        'ProjectID
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Call_No_PK" Then
                        'strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & "),"
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","

                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Project_ID" Then
                        strSelect &= "Project." & "PR_VC20_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Coordinator" Then
                        strSelect &= "Coord." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Close_Date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Request_Date" Then
                        ' strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",100)" & ","
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Call_Close_Date" Then
                        'strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",100)" & ","
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                        'ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_FL8_Total_Est_Time" Then
                        '    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        '        strSelect &= "(select sum(a.TM_FL8_est_hr) From T040021 a where a.tm_nu9_call_no_fk=Call.CM_NU9_Call_No_PK and a.TM_DT8_Task_Date>='" & dtFromDate.Text & "' and a.TM_DT8_Task_Date<='" & dtToDate.Text & "') as CM_FL8_Total_Est_Time,"
                        '    ElseIf dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        '        strSelect &= "(select sum(a.TM_FL8_est_hr) From T040021 a where a.tm_nu9_call_no_fk=Call.CM_NU9_Call_No_PK and a.TM_DT8_Task_Date>='" & dtFromDate.Text & "' ) as CM_FL8_Total_Est_Time,"
                        '    ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        '        strSelect &= "(select sum(a.TM_FL8_est_hr) From T040021 a where a.tm_nu9_call_no_fk=Call.CM_NU9_Call_No_PK and a.TM_DT8_Task_Date<='" & dtToDate.Text & "') as CM_FL8_Total_Est_Time,"
                        '    ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        '        strSelect &= "(select sum(a.TM_FL8_est_hr) From T040021 a where a.tm_nu9_call_no_fk=Call.CM_NU9_Call_No_PK) as CM_FL8_Total_Est_Time,"
                        '    End If
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_FL8_Total_Reported_Time" Then
                        If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date<='" & dtToDate.Text & "') as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date<='" & dtToDate.Text & "')/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours',"

                        ElseIf dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "') as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "')/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours',"

                        ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date<='" & dtToDate.Text & "') as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date<='" & dtToDate.Text & "')/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours',"

                        ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK) as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK)/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours', "

                        End If
                    Else
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","
                    End If

                    'CM_DT8_Close_Date <---------------call est. close date data field
                    Dim strcolname As String
                    strcolname = sqrdView.Item("ROD_VC50_ALIAS_NAME")
                    If (InStr(sqrdView.Item("ROD_VC50_ALIAS_NAME"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)

                    Else
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                        If strcolname = "TotalRptTime" Then
                            If Not CType(ViewState("arrColumnsName"), ArrayList).Contains("% Calculated Hours") Then CType(ViewState("arrColumnsName"), ArrayList).Add("% Calculated Hours")
                        End If
                    End If
                    CType(ViewState("arColWidth"), ArrayList).Add(sqrdView.Item("OBM_VC200_DESCR")) 'adding columns widthe in arraylist
                    rowcount = rowcount + 1
                End While
                sqrdView.Close()

                If rowcount <= 7 Then
                    GrdAddSerach.Visible = False
                    'Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("You Dont have Access on Default View...")
                    lstError.Items.Add("Please Select your Own View from View Dropdown...")
                    'cpnlCallView.Enabled = False
                    'cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                    'cpnlCallView.TitleCSS = "test2"
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    ViewState("CallNo") = 0
                    Exit Function
                End If
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'chk support comapny 
                '*************************************************************************************************

                If shJoin = 4 Then
                    strSelect &= " from T040011 Call,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number "
                ElseIf shJoin = 5 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number "
                ElseIf shJoin = 6 Then
                    strSelect &= " from T040011 Call,T060011 Owner,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number  "
                ElseIf shJoin = 7 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number "
                End If

                '****************************************************************************************************

                If ViewState("CVSmshCall") = 1 Then
                    strSelect &= " and cn_VC20_Call_Status='CLOSED' "
                Else
                    strSelect &= " and cn_VC20_Call_Status<>'CLOSED' "
                End If

                If dtFromDate.Text.Equals("") = False Then
                    strSelect &= " and Actiontbl.AM_DT8_Action_Date >='" & dtFromDate.Text & "' "
                End If

                If dtToDate.Text.Equals("") = False Then
                    strSelect &= " and Actiontbl.AM_DT8_Action_Date <='" & dtToDate.Text & "' "
                End If
                'Added company chk from company access table
                strSelect &= " and CM_NU9_Comp_Id_FK in (" & GetCompanySubQuery() & ")  "
                strSelect &= " order by CM_NU9_Call_No_PK desc "

                If ViewState("MyCall") = "MY" Then
                    strSelect = strSelect.Insert(strSelect.IndexOf("order by"), " and Call.CM_NU9_Call_Owner=" & Val(Session("PropUserID")) & "  ")
                End If

                compColumnNo = ""
                callOwnerColumnNo = ""
                suppCompColumnNo = ""
                byWhomColumnNo = ""
                callNoColumnNo = ""
                coordinatorColumnNo = ""

                If SQL.Search("T040011", "CallView", "FillDefault", strSelect, dsDefault, "sachin", "Prashar") = True Then

                    For inti As Integer = 0 To dsDefault.Tables("T040011").Columns.Count - 1
                        dsDefault.Tables("T040011").Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID" Then
                            compColumnNo = inti
                        End If
                        'If UCase(arSetColumnName.Item(inti)) = "CALLOWNER" Then
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqBy".ToUpper Then
                            callOwnerColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPCOMP" Then
                            suppCompColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "BYWHOM" Then
                            byWhomColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO" Then
                            callNoColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "Coordinator".ToUpper Then
                            coordinatorColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "RelatedCall".ToUpper Then
                            relatedCallColumnNo = inti
                        End If
                        '------------------------------------------------------------
                    Next
                    mdvtable.Table = dsDefault.Tables("T040011")

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("Subject", 23)
                    htGrdColumns.Add("CallDesc", 44)

                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, Session("PropCompanyID"), ViewState("CallNo"), 0, 0)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    'GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    'rowvalue = 0
                    'rowvalueCall = 0
                    'For intI As Integer = 0 To mdvtable.Table.Columns.Count - 1
                    '    If mdvtable.Table.Columns(intI).ColumnName = "CallNo" Then
                    '        Dim num As New GridNumericColumn
                    '        num.UniqueName = "CallNo"
                    '        num.DataField = "CallNo"
                    '        num.HeaderText = "CallNo"
                    '        num.HeaderStyle.Width = "20"
                    '        GrdAddSerach.MasterTableView.Columns.Add(num)
                    '    Else
                    '        Dim num1 As New GridBoundColumn
                    '        num1.UniqueName = mdvtable.Table.Columns(intI).ColumnName
                    '        num1.DataField = mdvtable.Table.Columns(intI).ColumnName
                    '        num1.HeaderText = mdvtable.Table.Columns(intI).ColumnName
                    '        GrdAddSerach.MasterTableView.Columns.Add(num1)

                    '    End If
                    'Next
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If
                    'GrdAddSerach.DataBind()
                    GrdAddSerach.Visible = True
                Else
                    GrdAddSerach.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No Record Found on this search Criteria")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                End If

            Else
                GrdAddSerach.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("No Record Found on this search Criteria")
                ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                'lstError.Items.Clear()
                'lstError.Items.Add("Call not opened so far or data not exist according to view query ...")
            End If
            Return mdvtable.Table
            ' Response.Write("<meta http-equiv=""Content-Type"" content=""text/html""; charset=""iso-8859-1""><meta http-equiv=""Expires"" content=""0""><meta http-equiv=""Cache-Control"" content=""no-cache""><meta http-equiv=""Pragma"" content=""no-cache"">")
        Catch ex As Exception
            CreateLog("Call_View", "Load-676", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function

    Protected Sub GrdAddSerach_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles GrdAddSerach.ColumnCreated
        Try
            Dim col As GridColumn = e.Column
            col.AutoPostBackOnFilter = True
            If col.HeaderText = "CallNo" Then
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub GrdAddSerach_DetailTableDataBind(ByVal source As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles GrdAddSerach.DetailTableDataBind
        Try

            Dim dataItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
            Select Case e.DetailTableView.DataMember
                Case "Tasks"
                    Dim Callid As String = dataItem("CallNO").Text
                    Dim Compid As String = dataItem("CompId").Text
                    Dim dt As New DataTable
                    'Call Function To load Tasks for a particular call of a particular company
                    dt = dtTasksData(Callid, Compid)
                    e.DetailTableView.DataSource = dt

                Case "TaskActions"
                    Dim taskid As Int32 = Convert.ToInt32(dataItem("TM_NU9_Task_no_PK").Text)
                    Dim Callid As Int32 = Convert.ToInt32(dataItem("TM_NU9_Call_No_FK").Text)
                    Dim Compid As Int32 = Convert.ToInt32(dataItem("TM_NU9_Comp_ID_FK").Text)
                    Dim dt As New DataTable
                    'Call Function To load Actions for a particular call of a particular company
                    dt = dtActionsData(Callid, Compid, taskid)
                    e.DetailTableView.DataSource = dt

            End Select
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            If TypeOf e.Item Is GridDataItem Then
            End If
            'Setting the Width of Filtering Boxes i.e setting the width of columns of main table(Calls table)
            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then
                If TypeOf e.Item Is GridDataItem Then
                End If
                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filteringItem As GridFilteringItem = CType(e.Item, GridFilteringItem)
                    'set dimensions for the filter textbox
                    Dim box As TextBox = CType(filteringItem("CallNo").Controls(0), TextBox)
                    box.Width = Unit.Pixel(75)
                    Dim box1 As TextBox = CType(filteringItem("CallDesc").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(300)
                    Dim box2 As TextBox = CType(filteringItem("Subject").Controls(0), TextBox)
                    box2.Width = Unit.Pixel(200)
                    Dim box3 As RadNumericTextBox = CType(filteringItem("TotalEstTime").Controls(0), RadNumericTextBox)
                    box3.Width = Unit.Pixel(25)
                    Dim box4 As RadNumericTextBox = CType(filteringItem("TotalRptTime").Controls(0), RadNumericTextBox)
                    box4.Width = Unit.Pixel(25)
                    Dim box5 As TextBox = CType(filteringItem("Tmp_Type").Controls(0), TextBox)
                    box5.Text = "Tmp Type"
                    ' dataItem.Style("width") = "10"
                    'box = CType(filteringItem("CallReqBy").Controls(0), TextBox)
                    'box.Width = Unit.Pixel(45)
                End If
            End If
           
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub GrdAddSerach_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GrdAddSerach.ItemDataBound
        Try
            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "0") Then
                If TypeOf e.Item Is GridDataItem Then
                    Dim ParentItem As GridDataItem = CType(GrdAddSerach.MasterTableView.GetItems(GridItemType.Item)(0), GridDataItem)
                    If Not GrdAddSerach.MasterTableView.DataSource Is Nothing Then
                        Dim Callno As String = CType(GrdAddSerach.MasterTableView.DataSource, DataTable).Rows(intVal)(0) ' CType(e.Item.DataItem, DataRowView)("CallNo") ' ParentItem.OwnerTableView.DataKeyValues(intVal)("callno")
                        If Not Callno Is Nothing Then
                            intVal += 1
                        End If
                    End If
                End If
            End If
            If TypeOf e.Item Is GridFilteringItem Then
                Dim a As String = ""

                Dim Fitem As GridFilteringItem = CType(e.Item, GridFilteringItem)
                ' If CType(e.Item, GridFilteringItem).Controls.Count > 10 Then
                For intt As Integer = 0 To CType(e.Item, GridFilteringItem).Controls.Count - 1
                    CType(CType(e.Item, GridFilteringItem).Controls(intt), TableCell).Attributes.Add("onkeyup", "CheckChar(this, event)")
                Next
                'End If

                'For Each col As GridColumn In GrdAddSerach.MasterTableView.Columns
                '    CType(Fitem(col.UniqueName).Controls(0), TextBox).Attributes.Add("onkeyup", "CheckChar(this, event)")
                'Next
            End If
            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then
                If TypeOf e.Item Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim Callno As String = CType(e.Item.DataItem, DataRowView)("CallNo")
                    Dim CompId As String = CType(e.Item.DataItem, DataRowView)("CompID")
                    Dim AttachmentExists As String = String.Empty
                    Dim CommentsExists As String = String.Empty
                    If Not CType(e.Item.DataItem, DataRowView)("A") Is DBNull.Value Then
                        AttachmentExists = CType(e.Item.DataItem, DataRowView)("A")
                    End If
                    If Not CType(e.Item.DataItem, DataRowView)("C") Is DBNull.Value Then
                        CommentsExists = CType(e.Item.DataItem, DataRowView)("C")
                    End If
                    Dim imgAtt As ImageButton = CType(e.Item.FindControl("imgAtt"), ImageButton)
                    Dim imgComm As ImageButton = CType(e.Item.FindControl("imgComm"), ImageButton)
                    If (AttachmentExists = "1") Then
                        imgAtt.ImageUrl = "../../images/Attach15_9.gif"
                        imgAtt.Attributes("href") = "#"
                        imgAtt.Attributes("onclick") = [String].Format("return ShowAttachmentForm('{0}','{1}','{2}','{3}');return false;", Callno, CompId, Callno, e.Item.ItemIndex)
                    Else
                        imgAtt.Visible = False
                    End If

                    If (CommentsExists = "0") Then
                        'If there are no comments
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("javascript:ShowCommentsForm('{0}','{1}','{2}');", CompId, Callno, e.Item.ItemIndex)
                    ElseIf (CommentsExists = "1") Then
                        'If comments are already read
                        imgComm.ImageUrl = "../../images/comment2.gif"
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("javascript:ShowCommentsForm('{0}','{1}','{2}');", CompId, Callno, e.Item.ItemIndex)
                    ElseIf (CommentsExists = "2") Then
                        'if new Comments are posted
                        imgComm.ImageUrl = "../../images/comment_Unread.gif"
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("javascript:ShowCommentsForm('{0}','{1}','{2}');", CompId, Callno, e.Item.ItemIndex)
                    End If
                    Dim j As Int32
                    If (e.Item.Cells.Count > 4) Then
                        For j = 4 To e.Item.Cells.Count - 1
                            e.Item.Cells(j).Attributes.Add("style", "cursor:hand")
                            'e.Item.Cells(j).Attributes.Add("onclick", "javascript:KeyCheck(" & Callno & ", '" & 1 & "','" & 1 & "','cpnlCallView_GrdAddSerach','" & CompId & "','" & strtsuppcomp & "')")
                            e.Item.Cells(j).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & Callno & ", '" & 1 & "','cpnlCallView_GrdAddSerach','" & CompId & "')")
                        Next
                    End If
                    Dim lnk As LinkButton = CType(e.Item.FindControl("lnkCallReqBy"), LinkButton)
                End If
                'CallReqBy Info link creation 
                GrdAddSerach.AutoGenerateColumns = True
                Dim col() As GridColumn = GrdAddSerach.MasterTableView.AutoGeneratedColumns

                Dim en As IEnumerator = col.GetEnumerator()
                Dim blnCallReqBy As Boolean = False

                While en.MoveNext()
                    Dim gridcolumn As GridColumn = DirectCast(en.Current, GridColumn)
                    If (gridcolumn.UniqueName = "C") Then
                        gridcolumn.Visible = False
                    End If
                    If (gridcolumn.UniqueName = "A") Then
                        gridcolumn.Visible = False
                        Dim i As Int32 = gridcolumn.OrderIndex
                    End If
                    If TypeOf e.Item Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                        If (gridcolumn.UniqueName = "CallReqBy") Then
                            blnCallReqBy = True
                            Dim Callno As String = dataItem("CALLNO").Text
                            Dim CompId As String = dataItem("COMPID").Text
                            Dim CallOwner As String = dataItem("CallReqBy").Text
                            Dim tb As TableCell = dataItem("CallReqBy")
                            Dim lnk As New LinkButton
                            lnk.Text = dataItem("CallReqBy").Text
                            lnk.EnableViewState = True

                            lnk.ID = dataItem("CallReqBy").Text
                            Dim lnkCallReqBy As String = lnk.ID

                            lnk.Attributes("href") = "#"
                            'lnk.Attributes("onclick") = [String].Format("return ShowCallReqByInfo('{0}','{1}','{2}','{3}');", CompId, Callno, CallOwner, e.Item.ItemIndex)
                            tb.Controls.Add(lnk)
                            Dim hlAddProducts As Control = e.Item.FindControl(lnkCallReqBy)
                            If IsNothing(hlAddProducts) = False Then
                                Dim currentRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                                If Not controlCollection.ContainsKey(hlAddProducts.ClientID) Then
                                    controlCollection.Add(hlAddProducts.ClientID, lnk.Text)
                                End If
                                If IsNothing(Me.RadToolTipManager1) = False Then
                                    Me.RadToolTipManager1.TargetControls.Add(hlAddProducts.ClientID, lnk.Text, True)
                                End If
                            End If
                        End If
                        If (blnCallReqBy = False) Then
                            Me.RadToolTipManager1.Visible = False
                        Else
                            Me.RadToolTipManager1.Visible = True
                        End If
                    End If
                End While
            Else
                Dim currentRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                If Not currentRow Is Nothing Then
                    If currentRow.DataView.Table.Columns.Count > 11 Then
                        For intI As Integer = 0 To currentRow.DataView.Count - 1
                            If Not IsDBNull(currentRow.Item(7)) And Not String.IsNullOrEmpty(currentRow.Item(8)) And Not IsDBNull(currentRow.Item(9)) And Not String.IsNullOrEmpty(currentRow.Item(10)) Then
                                If currentRow.Item(9) > currentRow(7) Then
                                    e.Item.Style.Add("color", "#9D009D")
                                End If
                                If currentRow.Item(7) = 0 Then
                                    e.Item.Style.Add("color", "Blue")
                                End If
                                If DateValue(currentRow.Item(8)) < DateValue(currentRow(10)) Then
                                    e.Item.Style.Add("color", "#9D009D")
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Function fillview() As DataTable
        '**********************
        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select distinct "
        Dim strwhereQuery As String = " and "
        Dim arcolName As New ArrayList
        Dim strOrderQuery As String = " order by "
        Dim strUnsortQuery As String
        Dim dsFromView As New DataSet
        Dim shJoin As Short
        Dim strcolname As String

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            sqrdView = SQL.Search("CallView", "FillView-719", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='1024'  order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then

                CType(ViewState("arColWidth"), ArrayList).Clear()
                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                Dim htDateCols As New Hashtable

                While sqrdView.Read
                    If sqrdView.Item("UV_VC50_COL_Value") = "CM_VC100_By_Whom" Then
                        strSelect &= "ByWhom." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Call_Owner" Then
                        strSelect &= "Owner." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Comp_Id_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                        shJoin += 4
                        'ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Call_No_PK" Then
                        '    strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & "),"

                        'ProjectID
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Project_ID" Then
                        strSelect &= "Project." & "PR_VC20_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_CustID_FK" Then
                        strSelect &= "suppcomp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Coordinator" Then
                        strSelect &= "Coord." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Close_Date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Request_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Call_Close_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_FL8_Total_Reported_Time" Then
                        If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date<='" & dtToDate.Text & "') as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date<='" & dtToDate.Text & "')/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours',"

                        ElseIf dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "') as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date>='" & dtFromDate.Text & "')/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours',"

                        ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date<='" & dtToDate.Text & "') as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK and a.AM_DT8_Action_Date<='" & dtToDate.Text & "')/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours',"

                        ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                            strSelect &= "(select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK) as CM_FL8_Total_Reported_Time,"
                            strSelect &= "(select case when(select CM_FL8_Total_Est_Time) <> 0 then cast((select sum(a.AM_FL8_Used_Hr) from T040031 a where a.am_nu9_Call_Number=Call.CM_NU9_Call_No_PK)/CM_FL8_Total_Est_Time * 100 as decimal(10,2)) else '0' end) as '% Calculate Hours', "

                        End If
                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If
                    CType(ViewState("arColWidth"), ArrayList).Add(sqrdView.Item("UV_VC10_Col_Width"))
                    strcolname = sqrdView.Item("UV_VC50_COL_Name")
                    If (InStr(sqrdView.Item("UV_VC50_COL_Name"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                    Else
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                        If strcolname = "TotalRptTime" Then
                            If Not CType(ViewState("arrColumnsName"), ArrayList).Contains("% Calculated Hours") Then CType(ViewState("arrColumnsName"), ArrayList).Add("% Calculated Hours")
                        End If
                    End If
                End While

                sqrdView.Close()

                sqrdView = SQL.Search("CallView", "FillView-785", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='1024'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
                While sqrdView.Read
                    ' Check for sort order of the column and if AD value is not unsorted
                    If sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        If sqrdView.Item("UV_VC50_COL_Value") = "CM_VC2000_Call_Desc" Then
                            strOrderQuery &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ") " & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        Else
                            strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        End If
                        ' Check for sort order of the column and if AD value is unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower = "unsorted" Then
                        If sqrdView.Item("UV_VC50_COL_Value") = "CM_VC2000_Call_Desc" Then
                            strOrderQuery &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & "),"
                        Else
                            strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & ", "
                        End If
                        ' If sort order of the column =0 and AD value is not unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") = 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        If sqrdView.Item("UV_VC50_COL_Value") = "CM_VC2000_Call_Desc" Then
                            strOrderQuery &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ") " & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        Else
                            strUnsortQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        End If
                    End If
                End While
                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'add where clause in query 
                '*************************************
                sqrdView = SQL.Search("CallView", "Fillview-809", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='1024' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

                Dim blnIsCoordinator As Boolean = False
                If blnView = True Then
                    While sqrdView.Read
                        Select Case CType(sqrdView.Item("UV_VC50_COL_Value"), String).Trim.ToUpper
                            Case "CM_NU9_CALL_OWNER"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += "  Owner.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += "  Owner.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_NU9_Coordinator".ToUpper
                                blnIsCoordinator = True
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += "  Coord.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += "  Coord.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If

                            Case "CM_NU9_COMP_ID_FK"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_NU9_CUSTID_FK"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " suppcomp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += " suppcomp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_VC100_BY_WHOM"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " ByWhom.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += " ByWhom.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If

                            Case "CM_NU9_PROJECT_ID"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " Project.PR_VC20_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += " Project.PR_VC20_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_REQUEST_DATE"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_CLOSE_DATE" 'call est close date
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_CALL_CLOSE_DATE" 'call close date
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case Else
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery += sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                        End Select
                    End While

                    sqrdView.Close()
                    strwhereQuery = strwhereQuery.Remove(Len(strwhereQuery) - 4, 4)
                    '*******************************************
                    'If Session("PropCompanyType") = "SCM" Then
                    '    'strwhereQuery += " and CM_NU9_Comp_Id_FK=" & Session("PropCompanyID")
                    'Else
                    '    strwhereQuery += " and CM_NU9_Comp_Id_FK=" & Session("PropCompanyID")
                    'End If
                End If
                'If Session("PropCompanyType") <> "SCM" And strwhereQuery = " and " Then
                '    strwhereQuery += "  CM_NU9_Comp_Id_FK=" & Session("PropCompanyID")
                'End If

                If shJoin = 4 Then
                    strSelect &= " from T040011 Call,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number "
                ElseIf shJoin = 5 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom  and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator  and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number  "
                ElseIf shJoin = 6 Then
                    strSelect &= " from T040011 Call,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and  Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number "
                ElseIf shJoin = 7 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord,T040031 ActionTBL  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator  and Call.CM_NU9_Comp_Id_FK = Actiontbl.AM_NU9_Comp_ID_FK and Call.CM_NU9_Call_No_PK = Actiontbl.AM_NU9_Call_Number "
                End If

                If ViewState("CVSmshCall") = 1 Then ' 1 means not display close call else all
                    strSelect &= " and cn_VC20_Call_Status='CLOSED' "
                Else
                    strSelect &= " and cn_VC20_Call_Status<>'CLOSED' "
                End If

                If dtFromDate.Text.Equals("") = False Then
                    strSelect &= " and Actiontbl.AM_DT8_Action_Date >='" & dtFromDate.Text & "' "
                End If

                If dtToDate.Text.Equals("") = False Then
                    strSelect &= " and Actiontbl.AM_DT8_Action_Date <='" & dtToDate.Text & "' "
                End If

                'Added company chk from company access table
                strSelect &= " and CM_NU9_Comp_Id_FK in (" & GetCompanySubQuery() & ") "

                If strwhereQuery.Equals(" and ") = True Then
                    If ViewState("MyCall") = "MY" Then
                        strwhereQuery = " and Call.CM_NU9_Call_Owner=" & Val(Session("PropUserID")) & "  "
                        strSelect &= strwhereQuery
                    End If
                Else
                    If ViewState("MyCall") = "MY" Then
                        strwhereQuery &= " and Call.CM_NU9_Call_Owner=" & Val(Session("PropUserID")) & "  "
                    End If
                    strSelect &= strwhereQuery
                End If

                If IsNothing(strUnsortQuery) = False Then
                    strUnsortQuery = strUnsortQuery.TrimEnd
                    strUnsortQuery = strUnsortQuery.Remove(Len(strUnsortQuery) - 1, 1)
                    If IsNothing(strOrderQuery) = True Then
                        strSelect &= strUnsortQuery
                    Else
                        strSelect &= strOrderQuery & " " & strUnsortQuery
                    End If
                Else
                    If strOrderQuery.Equals(" order by ") = False Then
                        strOrderQuery = strOrderQuery.TrimEnd
                        strOrderQuery = strOrderQuery.Remove(Len(strOrderQuery) - 1, 1)
                        strSelect &= strOrderQuery
                    End If
                End If

                If blnIsCoordinator = True Then
                    strSelect = strSelect.Replace("*", "")
                End If

                compColumnNo = ""
                callOwnerColumnNo = ""
                suppCompColumnNo = ""
                byWhomColumnNo = ""
                callNoColumnNo = ""
                coordinatorColumnNo = ""

                If SQL.Search("T040011", "CallView", "FillView-911", strSelect, dsFromView, "Sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID".ToUpper Then
                            compColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqBy".ToUpper Then
                            callOwnerColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPCOMP".ToUpper Then
                            suppCompColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "BYWHOM".ToUpper Then
                            byWhomColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO".ToUpper Then
                            callNoColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "Coordinator".ToUpper Then
                            coordinatorColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "RelatedCall".ToUpper Then
                            relatedCallColumnNo = inti
                        End If
                    Next

                    mdvtable.Table = dsFromView.Tables(0)

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("Subject", 23)
                    htGrdColumns.Add("CallDesc", 44)

                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, Session("PropCompanyID"), ViewState("CallNo"), 0, 0)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.Visible = True

                    'For intI As Integer = 0 To mdvtable.Table.Columns.Count - 1
                    '    If mdvtable.Table.Columns(intI).ColumnName = "CallNo" Then
                    '        Dim num As New GridNumericColumn
                    '        num.UniqueName = "CallNo"
                    '        num.DataField = "CallNo"
                    '        num.HeaderText = "CallNo"
                    '        'num.HeaderStyle.Width = "20"
                    '        GrdAddSerach.MasterTableView.Columns.Add(num)
                    '    Else
                    '        Dim num1 As New GridBoundColumn
                    '        num1.UniqueName = mdvtable.Table.Columns(intI).ColumnName
                    '        num1.DataField = mdvtable.Table.Columns(intI).ColumnName
                    '        num1.HeaderText = mdvtable.Table.Columns(intI).ColumnName
                    '        GrdAddSerach.MasterTableView.Columns.Add(num1)

                    '    End If
                    'Next

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If
                    'GrdAddSerach.DataSource = mdvtable.Table
                    Return mdvtable.Table
                Else
                    ' GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No Record Found on this search Criteria")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                    '  GrdAddSerach.DataBind()
                End If
            Else
                '*********************
                GrdAddSerach.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("No Record Found on this search Criteria")
                ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)

                Exit Function
            End If
        Catch ex As Exception
            CreateLog("Call_View", "FillView-945", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function
    Protected Sub GrdAddSerach_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles GrdAddSerach.NeedDataSource
        Try
            If Not e.IsFromDetailTable Then
                If ViewState("CallViewsimpleName") <> "" And ViewState("CallViewsimpleName") <> "Default" Then
                    ' fill datagrid based on user define columns and combination
                    Dim dt As DataTable = fillview()
                    GrdAddSerach.DataSource = dt
                Else
                    'fill tha datagrid from based on admin defined to the role
                    Dim dt As DataTable = fillDefault()
                    GrdAddSerach.DataSource = dt
                    ViewState("CallViewsimpleName") = "Default"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub RadAjaxManager1_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs) Handles RadAjaxManager1.AjaxRequest
        If (e.Argument = "Rebind") Then
            GrdAddSerach.MasterTableView.SortExpressions.Clear()
            RadToolTipManager1.TargetControls.Clear()
            GrdAddSerach.Rebind()
        End If
    End Sub

    Protected Sub RadToolTipManager1_AjaxUpdate(ByVal sender As Object, ByVal e As Telerik.Web.UI.ToolTipUpdateEventArgs)
        Dim ctrl As Control = Page.LoadControl("UserDetails.ascx")
        e.UpdatePanel.ContentTemplateContainer.Controls.Add(ctrl)
        Dim details As UserDetails = DirectCast(ctrl, UserDetails)
        details.GetCallOwner = e.Value.ToString()
    End Sub

    Private Function dtTasksData(Optional ByVal Callno As Int32 = 0, Optional ByVal CompId As String = "") As DataTable
        Dim dt As New DataTable
        Dim Callid As String = Callno
        Dim Companyid As String = CompId
        Dim conn As New SqlConnection()
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        conn.Open()
        Dim query As String
        If (Callid <> 0 And Companyid <> "") Then
            If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                query = "SELECT distinct convert(varchar,a.TM_NU9_Call_No_FK) as TM_NU9_Call_No_FK, a.TM_NU9_Comp_ID_FK,a.TM_NU9_Task_no_PK, SOwner.UM_VC50_UserID,a.TM_VC50_Deve_status AS TM_VC50_Deve_status, a.TM_VC1000_Subtsk_Desc AS TM_VC1000_Subtsk_Desc,a.TM_VC8_task_type,a.TM_FL8_Est_Hr AS EST_HOURS,convert(varchar(11),isnull(a.TM_DT8_Est_close_date,getdate()),113) AS EST_CLOSE_DATE,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = a.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = a.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK)) AS ACTUAL_HOURS, convert(varchar(11),isnull(a.TM_DT8_Task_Close_Date,getdate()),113) AS ACT_CLOSE_DATE,(SELECT     CASE WHEN(SELECT     TM_FL8_Est_Hr FROM          T040021 WHERE      TM_NU9_Call_No_FK = a.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = a.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK) <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = a.TM_NU9_Call_No_FK AND am_nu9_task_number = a.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK) / a.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS CALCULATE_HOURS FROM         T040021 AS a INNER JOIN T060011 AS SOwner ON a.TM_VC8_Supp_Owner = SOwner.UM_IN4_Address_No_FK  inner join T010011 comp on a.TM_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE     (a.TM_NU9_Call_No_FK = " & Callid & ") AND (comp.CI_VC36_Name='" & Companyid & "') and ActionTBL.AM_DT8_Action_Date >='" & dtFromDate.Text & "' and ActionTBL.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
            ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                query = "SELECT distinct convert(varchar,a.TM_NU9_Call_No_FK) as TM_NU9_Call_No_FK, a.TM_NU9_Comp_ID_FK,a.TM_NU9_Task_no_PK, SOwner.UM_VC50_UserID,a.TM_VC50_Deve_status AS TM_VC50_Deve_status, a.TM_VC1000_Subtsk_Desc AS TM_VC1000_Subtsk_Desc,a.TM_VC8_task_type,a.TM_FL8_Est_Hr AS EST_HOURS,convert(varchar(11),isnull(a.TM_DT8_Est_close_date,getdate()),113) AS EST_CLOSE_DATE,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = a.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = a.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK)) AS ACTUAL_HOURS, convert(varchar(11),isnull(a.TM_DT8_Task_Close_Date,getdate()),113) AS ACT_CLOSE_DATE,(SELECT     CASE WHEN(SELECT     TM_FL8_Est_Hr FROM          T040021 WHERE      TM_NU9_Call_No_FK = a.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = a.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK) <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = a.TM_NU9_Call_No_FK AND am_nu9_task_number = a.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK) / a.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS CALCULATE_HOURS FROM         T040021 AS a INNER JOIN T060011 AS SOwner ON a.TM_VC8_Supp_Owner = SOwner.UM_IN4_Address_No_FK  inner join T010011 comp on a.TM_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE     (a.TM_NU9_Call_No_FK = " & Callid & ") AND (comp.CI_VC36_Name='" & Companyid & "') and ActionTBL.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
                'query = "SELECT convert(varchar,TM_NU9_Call_No_FK) as TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK,comp.CI_VC36_Name,SOwner.UM_VC50_UserID,TM_VC1000_Subtsk_Desc,TM_VC50_Deve_status,CM_VC8_Call_Type,TM_VC8_Supp_Owner,TM_NU9_Assign_by,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Forms from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project where(task.TM_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number)and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and TM_VC50_Deve_Status<>'Closed' And CM_NU9_Call_No_PK = TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK and TM_NU9_Call_No_FK=" & Callid & " and comp.CI_VC36_Name='" & Companyid & "'"
            ElseIf dtToDate.Text.Equals("") = True And dtFromDate.Text.Equals("") = False Then
                query = "SELECT distinct convert(varchar,a.TM_NU9_Call_No_FK) as TM_NU9_Call_No_FK, a.TM_NU9_Comp_ID_FK,a.TM_NU9_Task_no_PK, SOwner.UM_VC50_UserID,a.TM_VC50_Deve_status AS TM_VC50_Deve_status, a.TM_VC1000_Subtsk_Desc AS TM_VC1000_Subtsk_Desc,a.TM_VC8_task_type,a.TM_FL8_Est_Hr AS EST_HOURS,convert(varchar(11),isnull(a.TM_DT8_Est_close_date,getdate()),113) AS EST_CLOSE_DATE,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = a.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = a.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK)) AS ACTUAL_HOURS, convert(varchar(11),isnull(a.TM_DT8_Task_Close_Date,getdate()),113) AS ACT_CLOSE_DATE,(SELECT     CASE WHEN(SELECT     TM_FL8_Est_Hr FROM          T040021 WHERE      TM_NU9_Call_No_FK = a.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = a.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK) <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = a.TM_NU9_Call_No_FK AND am_nu9_task_number = a.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK) / a.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS CALCULATE_HOURS FROM         T040021 AS a INNER JOIN T060011 AS SOwner ON a.TM_VC8_Supp_Owner = SOwner.UM_IN4_Address_No_FK  inner join T010011 comp on a.TM_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE     (a.TM_NU9_Call_No_FK = " & Callid & ") AND (comp.CI_VC36_Name='" & Companyid & "') and ActionTBL.AM_DT8_Action_Date >='" & dtFromDate.Text & "'"
            ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                query = "SELECT distinct convert(varchar,a.TM_NU9_Call_No_FK) as TM_NU9_Call_No_FK, a.TM_NU9_Comp_ID_FK,a.TM_NU9_Task_no_PK, SOwner.UM_VC50_UserID,a.TM_VC50_Deve_status AS TM_VC50_Deve_status, a.TM_VC1000_Subtsk_Desc AS TM_VC1000_Subtsk_Desc,a.TM_VC8_task_type,a.TM_FL8_Est_Hr AS EST_HOURS,convert(varchar(11),isnull(a.TM_DT8_Est_close_date,getdate()),113) AS EST_CLOSE_DATE,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = a.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = a.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK)) AS ACTUAL_HOURS, convert(varchar(11),isnull(a.TM_DT8_Task_Close_Date,getdate()),113) AS ACT_CLOSE_DATE,(SELECT     CASE WHEN(SELECT     TM_FL8_Est_Hr FROM          T040021 WHERE      TM_NU9_Call_No_FK = a.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = a.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = a.TM_NU9_Comp_ID_FK) <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = a.TM_NU9_Call_No_FK AND am_nu9_task_number = a.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK) / a.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS CALCULATE_HOURS FROM         T040021 AS a INNER JOIN T060011 AS SOwner ON a.TM_VC8_Supp_Owner = SOwner.UM_IN4_Address_No_FK  inner join T010011 comp on a.TM_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE     (a.TM_NU9_Call_No_FK = " & Callid & ") AND (comp.CI_VC36_Name='" & Companyid & "')"
            End If
        Else
            query = "select convert(varchar,TM_NU9_Call_No_FK) as TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK,comp.CI_VC36_Name,SOwner.UM_VC50_UserID,TM_VC1000_Subtsk_Desc,TM_VC50_Deve_status,CM_VC8_Call_Type,TM_VC8_Supp_Owner,TM_NU9_Assign_by,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Forms from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project where(task.TM_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number)and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and TM_VC50_Deve_Status<>'Closed' And CM_NU9_Call_No_PK = TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK"
        End If
        Dim ad As New SqlDataAdapter
        Dim cmd As New SqlCommand(query)
        cmd.Connection = conn
        ad.SelectCommand = cmd
        ad.Fill(dt)
        conn.Close()
        Return dt
    End Function

    Private Function dtActionsData(Optional ByVal Callno As Int32 = 0, Optional ByVal CompId As Int32 = 0, Optional ByVal TaskNo As Int32 = 0) As DataTable
        Dim dt As New DataTable
        Dim Callid As String = Callno
        Dim Companyid As String = CompId
        Dim TaskNumber As String = TaskNo
        Dim conn As New SqlConnection()
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        conn.Open()
        Dim query As String
        If (Callid <> 0 And Companyid <> 0 And TaskNumber <> 0) Then
            If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                query = "select distinct  AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK=" & CompId.ToString() & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date >='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
            ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                query = "select distinct AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK=" & CompId.ToString() & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
            ElseIf dtToDate.Text.Equals("") = True And dtFromDate.Text.Equals("") = False Then
                query = "select distinct AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK=" & CompId.ToString() & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date >='" & dtFromDate.Text & "'"
            ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                query = "select distinct AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK=" & CompId.ToString() & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "'"
            End If
            'query = "SELECT * FROM T040031 where AM_NU9_Call_Number=" & Callid & " and AM_NU9_Comp_ID_FK=" & Companyid & "and AM_NU9_Task_Number=" & TaskNumber
        Else
            query = "select AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner and AM_NU9_Comp_ID_FK=" & CompId.ToString() & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date >='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
        End If
        Dim ad As New SqlDataAdapter
        Dim cmd As New SqlCommand(query)
        cmd.Connection = conn
        ad.SelectCommand = cmd
        ad.Fill(dt)
        conn.Close()
        Return dt
    End Function

    Protected Sub imgAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click
        ViewState("CallNo") = 0
        Dim focusScript As String = "<script language='javascript'>" & _
        "window.parent.OpenTabOnAddClick('Call Entry', 'SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=-1&PageID=5' , '3'); </script>"
        Page.RegisterStartupScript("FocusScript", focusScript)
        'Response.Redirect("../../Supportcenter/Callview/Call_Detail.aspx?ScrID=3&ID=-1", False)
    End Sub

    Protected Sub imgCloseCall_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCloseCall.Click
        If ViewState("CVSmshCall") = 0 Then
            ViewState("CVSmshCall") = 1
            imgCloseCall.ToolTip = "View All Calls"
        Else
            ViewState("CVSmshCall") = 0
            imgCloseCall.ToolTip = "View Only Closed Calls"
        End If
        ' mstrCallNumber = "0"
        ViewState("CallNo") = "0"
        GrdAddSerach.Rebind()
    End Sub

    Protected Sub imgMyCall_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgMyCall.Click
        If ViewState("MyCall") = "ALL" Then
            ViewState("MyCall") = "MY"
            imgMyCall.ToolTip = "Show All Calls"
            'cpnlCallView.Text = "Call View :  My Calls"
        Else
            ViewState("MyCall") = "ALL"
            imgMyCall.ToolTip = "Show My Calls"
            ' cpnlCallView.Text = "Call View :  All Calls"
        End If
        GrdAddSerach.Rebind()
    End Sub

    Protected Sub Logout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Logout.Click
        LogoutWSS()
    End Sub

    Protected Sub GrdAddSerach_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdAddSerach.PreRender
        'Dim col As GridColumn
        'Dim filter As GridFilteringItem
        'For Each col In GrdAddSerach.MasterTableView.RenderColumns
        '    If (col.UniqueName = "CallReqBy") Then
        '        col.HeaderStyle.Width = Unit.Pixel(70)
        '        col.ItemStyle.Width = Unit.Pixel(70)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("CallReqBy").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(50)
        '        Next
        '    End If

        '    If (col.UniqueName = "CallStatus") Then
        '        col.HeaderStyle.Width = Unit.Pixel(90)
        '        col.ItemStyle.Width = Unit.Pixel(90)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("CallStatus").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(70)
        '        Next
        '    End If
        '    If (col.UniqueName = "Priority") Then
        '        col.HeaderStyle.Width = Unit.Pixel(70)
        '        col.ItemStyle.Width = Unit.Pixel(70)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("Priority").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(50)
        '        Next
        '    End If
        '    If (col.UniqueName = "CallType") Then
        '        col.HeaderStyle.Width = Unit.Pixel(70)
        '        col.ItemStyle.Width = Unit.Pixel(70)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("CallType").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(50)
        '        Next
        '    End If
        '    If (col.UniqueName = "SubCategory") Then
        '        col.HeaderStyle.Width = Unit.Pixel(70)
        '        col.ItemStyle.Width = Unit.Pixel(70)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("SubCategory").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(50)
        '        Next
        '    End If
        '    If (col.UniqueName = "AP_Unit") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("AP_Unit").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        '    If (col.UniqueName = "CCode_2") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("CCode_2").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        '    If (col.UniqueName = "CatCode3") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("CatCode3").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        '    If (col.UniqueName = "Agreement") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("Agreement").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        '    If (col.UniqueName = "Agreement") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As TextBox = CType(filter("Agreement").Controls(0), TextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        '    If (col.UniqueName = "TotalEstTime") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As RadNumericTextBox = CType(filter("TotalEstTime").Controls(0), RadNumericTextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        '    If (col.UniqueName = "TotalRptTime") Then
        '        col.HeaderStyle.Width = Unit.Pixel(50)
        '        col.ItemStyle.Width = Unit.Pixel(50)
        '        For Each filter In GrdAddSerach.MasterTableView.GetItems(GridItemType.FilteringItem)
        '            Dim box As RadNumericTextBox = CType(filter("TotalRptTime").Controls(0), RadNumericTextBox)
        '            box.Width = Unit.Pixel(30)
        '        Next
        '    End If
        'Next

    End Sub

    Protected Sub imgExportToExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToExcel.Click
        GrdAddSerach.MasterTableView.ExportToExcel()
    End Sub

    Protected Sub imgExportToWord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToWord.Click
        GrdAddSerach.MasterTableView.ExportToWord()

    End Sub

    Protected Sub hid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hid.Click
        Me.GrdAddSerach.Rebind()
    End Sub
    Protected Sub hid1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hid1.Click
        Response.Redirect("../../NewCall/CallView/CallView_Simple.aspx")
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Me.GrdAddSerach.Rebind()
    End Sub
   
End Class
