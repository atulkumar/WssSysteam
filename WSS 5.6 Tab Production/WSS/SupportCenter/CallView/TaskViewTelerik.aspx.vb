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

Partial Class SupportCenter_CallView_TaskViewTelerik
    Inherits System.Web.UI.Page
    Private mdvtable As New DataView  ' store data from table for view grid 
    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 
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
    Protected controlCollection1 As Dictionary(Of String, String) = New Dictionary(Of String, String)
    Private txthiddenImage As String 'stored clicked button's cation  
    Private intComp As String
    Public mstrcomp As String
    'Dim mblnValue As Boolean
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Public mstrCallNumber As String
    Public introwvalues As Integer 'stored the selected row's value
    Private mintCallNoPlace As Integer
    Private mintCompId As String = String.Empty
    Private mintSuppOwnID As String = String.Empty
    Private mintSuppOwn As String = String.Empty
    Private mintAssignBy As String = String.Empty
    Private mintAssignByID As String = String.Empty
    Private mintTaskNoRowID As String = String.Empty
    'Added call columns 
    Private mstrCallEntBy As String = String.Empty
    Private mstrCallEntByID As String = String.Empty
    Private mstrCallReqBy As String = String.Empty
    Private mstrCallReqByID As String = String.Empty
    Private mstrCoordinator As String = String.Empty
    Private mstrCoordinatorID As String = String.Empty
    'Private _groupChange As Boolean = False
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
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        'Security Block
        '****************************************
        ' Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            '   intId = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(2212) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, 2212)
        End If
        'End of Security Block
        '*****************************************


        'imgBtnViewPopup.Attributes("onclick") = [String].Format("return ShowSimpleCallViewsForm();")
        imgBtnViewPopup.Attributes.Add("Onclick", "return OpenVW('T040011');")
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
        'GrdAddSerach.MasterTableView.GroupsDefaultExpanded = False
        'GrdAddSerach..GroupingEnabled = False
        'If Page.IsPostBack = False Then
        '    GrdAddSerach.MasterTableView.GroupByExpressions.Remove("TaskOwner , sum(Actual_Hours) , sum(EstHr) Group By TaskOwner")

        'End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "View"
                        introwvalues = 0
                        'filling session variables on combo change event
                        'session("CallViewsimpleName") = ddlstview.SelectedItem.Text
                        'session("CallViewSimpleValue") = ddlstview.SelectedItem.Value
                        ViewState("TaskViewName") = ddlstview.SelectedItem.Text
                        ViewState("TaskViewValue") = ddlstview.SelectedItem.Value
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
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=2212 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

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
        'ViewState("CallViewsimpleName") = ddlstview.SelectedItem.Text
        'ViewState("CallViewSimpleValue") = ddlstview.SelectedItem.Value
        ViewState("TaskViewName") = ddlstview.SelectedItem.Text
        ViewState("TaskViewValue") = ddlstview.SelectedValue
        SaveUserView()
        Me.GrdAddSerach.Rebind()
        'GrdAddSerach.MasterTableView.SortExpressions.Clear()
        ''GrdAddSerach.Columns.Clear()
        'GrdAddSerach.Rebind()
        ''viewstate("CallNo") = 0
    End Sub

    ''' <summary>
    ''' A Procedure to Fill Dropdown of Views
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub GetView()
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        ddlstview.Items.Clear()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            sqrdView = SQL.Search("Task_View", "GetView-1200", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='2212' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then
                ddlstview.DataSource = sqrdView
                ddlstview.DataTextField = "UV_VC50_View_Name"
                ddlstview.DataValueField = "UV_IN4_View_ID"
                ddlstview.DataBind()
                sqrdView.Close()
            End If
            If ViewState("TaskViewName") = "" Or ViewState("TaskViewName") = "Default" Then
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
                ddlstview.SelectedIndex = ddlstview.Items.Count - 1
            Else
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
            End If

            If ViewState("TaskViewName") <> "" And ViewState("TaskViewName") <> "Default" Then
                ddlstview.SelectedValue = ViewState("TaskViewValue")
            End If

        Catch ex As Exception
            CreateLog("Task_View", "GetView-1185", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Sub SaveUserView()
        Dim intid = 2212 ' screen id for call view screen
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

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=2212 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

            If blnReturn = False Then
                ViewState("TaskViewName") = "Default"
                ViewState("TaskViewValue") = "0"
                Exit Sub
            Else
                While sqdrCol.Read
                    ViewState("TaskViewName") = sqdrCol.Item("UV_VC50_View_Name")
                    ViewState("TaskViewValue") = sqdrCol.Item("UV_IN4_View_ID")
                    ddlstview.SelectedValue = ViewState("TaskViewValue")
                End While
            End If

            sqdrCol.Close()
            sqdrCol = Nothing

        Catch ex As Exception
            ddlstview.SelectedValue = 0
            CreateLog("Task_View", "ChkSelectedView-2080", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#Region "FillDefault"
    '*******************************************************************
    ' Function             :-  fillDefault
    ' Purpose             :- Fill and design datagrid based on defaultcolumns settings from default  tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/2006			      Sachin Prashar           -------09/07/2009	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function FillDefault() As DataTable

        Try

            Dim dsDefault As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select distinct "
            Dim strwhereQuery As String = " and "
            Dim shJoin As Short
            ' GrdAddSerach.PageSize = mintPageSize ' set the grid page size
            Dim strQuery As String 'fatching default data from tables for particular role


            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
              & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
              & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =2212 And " _
              & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
              & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
              & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
              & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
              & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=2212 and obm_vc4_object_type_fk='VIW') " _
              & " order by OBM.OBM_SI2_Order_By"

            sqrdView = SQL.Search("Task_View", "FillDefault-661", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then

                Dim htDateCols As New Hashtable
                Dim rowcount As Int64
                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                CType(ViewState("arColWidth"), ArrayList).Clear()
                Dim intassignbyID As Int16 = 0

                While sqrdView.Read
                    If sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID" Then
                        strSelect &= "SOwner." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID_Assign" Then
                        strSelect &= "ABy." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID_ReqBy" Then
                        strSelect &= "ReqBy." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID_EntBy" Then
                        strSelect &= "EntBy." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID_Coord" Then
                        strSelect &= "Coord." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Call_No_FK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & "),"
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Project_ID" Then
                        strSelect &= "Project." & "PR_VC20_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Est_close_date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Task_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Task_Close_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Call_Close_Date" Then ' Call Colse date
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Close_Date" Then ' Call Estimated Close date
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Request_Date" Then 'Call Request Date
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_CAll_Start_Date" Then 'Call start Date
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_FL8_Est_Hr" Then
                        If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK)) AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK) <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK) / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                        If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK)  and AM_DT8_Action_Date>='" & dtFromDate.Text & "') AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "') <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "') / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                        If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK) and  convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK  and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                        If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK) and AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If

                    Else
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","
                    End If
                    CType(ViewState("arColWidth"), ArrayList).Add(sqrdView.Item("OBM_VC200_DESCR")) 'adding columns widthe in arraylist
                    Dim strcolname As String
                    strcolname = sqrdView.Item("ROD_VC50_ALIAS_NAME")
                    If (InStr(sqrdView.Item("ROD_VC50_ALIAS_NAME"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                    Else
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                        If strcolname = "EstHr" Then
                            If Not CType(ViewState("arrColumnsName"), ArrayList).Contains("Actual_Hours") Then CType(ViewState("arrColumnsName"), ArrayList).Add("Actual_Hours")
                            If Not CType(ViewState("arrColumnsName"), ArrayList).Contains("%Calculate_Hours") Then CType(ViewState("arrColumnsName"), ArrayList).Add("%Calculate_Hours")
                        End If
                    End If
                    rowcount = rowcount + 1
                End While
                sqrdView.Close()
                If rowcount = 16 Then
                    GrdAddSerach.Visible = False
                    'Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("You Dont have Access on Default View...")
                    lstError.Items.Add("Please Select your Own View from View Dropdown...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    'cpnlTaskView.Enabled = False
                    ' cpnlTaskView.TitleCSS = "test2"
                    ViewState("CallNo") = 0
                    Exit Function
                End If
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                If shJoin = 1 Then

                    'and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK
                    'and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK
                    'and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK
                    'and actionTable.AM_DT8_Action_Date>='5-Oct-2009'
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        ' dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))>='" & dtFromDate.Text & "' and actionTable.AM_DT8_Action_Date<='" & dtToDate.Text & "'"
                    End If


                ElseIf shJoin = 2 Then
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator  and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        ' dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If

                ElseIf shJoin = 3 Then
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        ' dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If

                End If

                If ViewState("TVmshCall") = 1 Then
                    '  strSelect &= " and TM_VC50_Deve_Status='Closed' "
                Else
                    ' strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
                End If
                'If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                '    strSelect &= "left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number"
                '    'strSelect &= " and Actiontbl.AM_DT8_Action_Date >='" & dtFromDate.Text & "' "
                'End If

                'If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                '    strSelect &= "left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE ActionTBL.AM_DT8_Action_Date >='" & dtFromDate.Text & "'"
                '    ' strSelect &= " and Actiontbl.AM_DT8_Action_Date <='" & dtToDate.Text & "' "
                'End If
                'If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                '    strSelect &= "left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE ActionTBL.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
                '    ' strSelect &= " and Actiontbl.AM_DT8_Action_Date <='" & dtToDate.Text & "' "
                'End If
                'If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                '    strSelect &= "left outer join T040031  As ActionTBL on a.TM_NU9_Comp_ID_FK=ActionTBL.AM_NU9_Comp_ID_FK and a.TM_NU9_Call_No_FK=ActionTBL.AM_NU9_Call_Number and a.TM_NU9_Task_no_PK=ActionTBL.AM_NU9_Task_Number  WHERE ActionTBL.AM_DT8_Action_Date >='" & dtFromDate.Text & "' and ActionTBL.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
                '    ' strSelect &= " and Actiontbl.AM_DT8_Action_Date <='" & dtToDate.Text & "' "
                'End If
                'If ViewState("DepTask") = 1 Then
                '    strSelect &= " and TM_NU9_Dependency  is not null  "
                'End If
                strSelect &= " and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK "
                'Added company chk from company access table
                strSelect &= " and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
                'strSelect &= " order by TM_NU9_Call_No_FK desc"
                strSelect &= " "

                mintCallNoPlace = -1
                mintCompId = ""
                mintSuppOwnID = ""
                mintSuppOwn = ""
                mintAssignBy = ""
                mintAssignByID = ""
                mintTaskNoRowID = ""
                'added new for call columns
                mstrCallEntBy = ""
                mstrCallEntByID = ""
                mstrCallReqBy = ""
                mstrCallReqByID = ""
                mstrCoordinator = ""
                mstrCoordinatorID = ""
                relatedCallColumnNo = ""

                If SQL.Search("T040021", "Task_View", "Filldefault-778", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T040021").Columns.Count - 1

                        dsDefault.Tables("T040021").Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO" Then
                            mintCallNoPlace = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID" Then
                            mintCompId = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPOWNID" Then
                            mintSuppOwnID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKOWNER" Then
                            mintSuppOwn = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBY" Then
                            mintAssignBy = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBYID" Then
                            mintAssignByID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKNO" Then
                            mintTaskNoRowID = inti
                        End If
                        'Added for new Call Columns
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqBy".ToUpper Then
                            mstrCallReqBy = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqByID".ToUpper Then
                            mstrCallReqByID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallEntBy".ToUpper Then
                            mstrCallEntBy = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallEntByID".ToUpper Then
                            mstrCallEntByID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "Coordinator".ToUpper Then
                            mstrCoordinator = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CoordinatorID".ToUpper Then
                            mstrCoordinatorID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "RelatedCall".ToUpper Then
                            relatedCallColumnNo = inti
                        End If
                    Next

                    mdvtable.Table = dsDefault.Tables("T040021")

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("SubTaskDesc", 45)
                    htGrdColumns.Add("CallDesc", 44)
                    htGrdColumns.Add("CallSubject", 29)
                    htGrdColumns.Add("TaskDesc", 45)

                    'HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNumber"), ViewState("ActionNumber"))

                    'SetDataTableDateFormat(mdvtable.Table, htDateCols)

                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    rowvalue = 0
                    rowvalueCall = 0

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.AllowPaging = True
                    'GrdAddSerach.PageSize = mintPageSize
                    GrdAddSerach.Visible = True
                    If ViewState("TaskViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        ' CurrentPg.Text = 1
                        ViewState("SortOrder") = Nothing
                    End If

                    If Val(ViewState("DepTask")) = 1 Then
                        Dim arrTemp As New ArrayList
                        For intI As Integer = 0 To mdvtable.Table.Rows.Count - 1
                            If GetDepStat(mdvtable.Table.Rows(intI).Item("CallNo"), mdvtable.Table.Rows(intI).Item("CompID"), mdvtable.Table.Rows(intI).Item("TaskNo")) = True Then
                                arrTemp.Add(mdvtable.Table.Rows(intI))
                            End If
                        Next
                        For intJ As Integer = 0 To arrTemp.Count - 1
                            mdvtable.Table.Rows.Remove(arrTemp(intJ))
                        Next
                    End If

                    'If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                    '    GrdAddSerach.CurrentPageIndex = 0
                    '    CurrentPg.Text = 1
                    'End If
                    'GrdAddSerach.DataSource = mdvtable.Table
                    'GrdAddSerach.DataBind()
                    ''********************************************************
                    ' ''paging count
                    'Dim intRows As Integer = mdvtable.Table.Rows.Count
                    'Dim _totalPages As Double = 1
                    'Dim _totalrecords As Int32
                    'If Not Page.IsPostBack Then
                    '    _totalrecords = intRows
                    '    _totalPages = _totalrecords / mintPageSize
                    '    TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                    '    TotalRecods.Text = _totalrecords
                    'Else
                    '    _totalrecords = intRows
                    '    If CurrentPg.Text = 0 And _totalrecords > 0 Then
                    '        CurrentPg.Text = 1
                    '    End If
                    '    If _totalrecords = 0 Then
                    '        CurrentPg.Text = 0
                    '    End If
                    '    _totalPages = _totalrecords / mintPageSize
                    '    TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                    '    _totalPages = Double.Parse(TotalPages.Text)
                    '    TotalRecods.Text = _totalrecords
                    'End If
                    'cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    'cpnlTaskView.Enabled = True
                    'cpnlTaskView.TitleCSS = "test"
                    '***********************************************
                Else
                    GrdAddSerach.Visible = False
                    'Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No Task Assigned...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    'cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    'cpnlTaskView.Enabled = False
                    'cpnlTaskView.TitleCSS = "test2"
                End If
                Return mdvtable.Table
            Else
                GrdAddSerach.Visible = False
                ' Panel1.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("Sorry! Task View Data not available...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                'cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                'cpnlTaskView.Enabled = False
                'cpnlTaskView.TitleCSS = "test2"
            End If
            '***********************************************************
        Catch ex As Exception
            CreateLog("Task_View", "Load-792", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function

#End Region
    Function GetDepStat(ByVal callNo As String, ByVal compID As String, ByVal TaskNo As String) As Boolean
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBTracing = False
        Dim intRows As Integer
        Dim SQLQuery As String

        SQLQuery = " select TM_NU9_Call_No_FK from T040021 where TM_NU9_Comp_ID_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & compID & "' ) and TM_NU9_Call_No_FK=" & callNo & " and TM_NU9_Task_no_PK=(select isnull(TM_NU9_Dependency,0) from T040021 where TM_NU9_Comp_ID_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & compID & "' ) and TM_NU9_Call_No_FK=" & callNo & " and TM_NU9_Task_no_PK=" & TaskNo & " ) and TM_VC50_Deve_status<>'CLOSED'"

        Try
            SQL.Search("Call_View", "GetMonStat-2409", SQLQuery, intRows)
            If intRows > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("todolisr", "GetDepStat-4134", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try

    End Function
#Region "FillView"
    '*******************************************************************
    ' Function             :-  Fillview
    ' Purpose              :- Fill and design datagrid based on user defined columns settings from user tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function Fillview() As DataTable

        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select distinct "
        Dim arcolName As New ArrayList

        'GrdAddSerach.PageSize = mintPageSize ' set the grid page size
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            Dim shJoin As Short
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            Dim strwhereQuery As String = " and "

            sqrdView = SQL.Search("Task_View", "FillView-846", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='2212' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                Dim dsFromView As New DataSet
                Dim htDateCols As New Hashtable
                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                CType(ViewState("arColWidth"), ArrayList).Clear()
                Dim intSuppID As Int16 = 0
                Dim intassignID As Int16 = 0

                While sqrdView.Read
                    If sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID" Then
                        strSelect &= "SOwner." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID_Assign" Then
                        strSelect &= "ABy." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID_ReqBy" Then
                        strSelect &= "ReqBy." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID_EntBy" Then
                        strSelect &= "EntBy." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID_Coord" Then
                        strSelect &= "Coord." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_NU9_Project_ID" Then
                        strSelect &= "Project." & "PR_VC20_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_NU9_Call_No_FK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & "),"
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_DT8_Est_close_date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_DT8_Task_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_DT8_Task_Close_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Call_Close_Date" Then ' Call Colse date
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Close_Date" Then ' Call Estimated Close date
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Request_Date" Then 'Call Request Date
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_CAll_Start_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)

                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_FL8_Est_Hr" Then
                        If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK)) AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK) <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK) / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                        If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK)  and AM_DT8_Action_Date>='" & dtFromDate.Text & "') AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "') <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "') / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                        If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                            'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)

                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK) and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK  and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                        If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                            'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)

                            strSelect &= "TM_FL8_Est_Hr,(SELECT     sum(AM_FL8_Used_Hr) FROM T040031 WHERE      (AM_NU9_Call_Number = task.TM_NU9_Call_No_FK) AND (AM_NU9_Task_Number = task.TM_NU9_Task_no_PK) AND (AM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK) and AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') AS Actual_Hours, (SELECT     CASE WHEN(SELECT TM_FL8_Est_Hr FROM T040021 WHERE TM_NU9_Call_No_FK = task.TM_NU9_Call_No_FK AND tm_nu9_task_no_pk = task.TM_NU9_Task_no_PK AND TM_NU9_Comp_ID_FK = task.TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') <> 0 THEN CAST((SELECT sum(AM_FL8_Used_Hr) FROM         T040031 WHERE     am_nu9_call_number = task.TM_NU9_Call_No_FK AND am_nu9_task_number = task.TM_NU9_Task_no_PK AND am_nu9_comp_id_fk = TM_NU9_Comp_ID_FK and AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,AM_DT8_Action_Date,101))<='" & dtToDate.Text & "') / task.TM_FL8_Est_Hr AS decimal(10, 2)) * 100 ELSE '0' END AS Expr1) AS '%Calculate_Hours',"
                        End If
                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If
                    CType(ViewState("arColWidth"), ArrayList).Add(sqrdView.Item("UV_VC10_Col_Width"))
                    Dim strcolname As String
                    strcolname = sqrdView.Item("UV_VC50_COL_Name")
                    If (InStr(sqrdView.Item("UV_VC50_COL_Name"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                    Else
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                        If strcolname = "EstHr" Then
                            If Not CType(ViewState("arrColumnsName"), ArrayList).Contains("Actual_Hours") Then CType(ViewState("arrColumnsName"), ArrayList).Add("Actual_Hours")
                            If Not CType(ViewState("arrColumnsName"), ArrayList).Contains("%Calculate_Hours") Then CType(ViewState("arrColumnsName"), ArrayList).Add("%Calculate_Hours")
                        End If
                    End If
                End While
                sqrdView.Close()
                sqrdView = SQL.Search("Task_View", "FillView-897", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='2212'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
                While sqrdView.Read
                    ' Check for sort order of the column and if AD value is not unsorted
                    If sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        ' Check for sort order of the column and if AD value is unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower = "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & ", "
                        ' If sort order of the column =0 and AD value is not unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") = 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strUnsortQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                    End If
                End While
                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'Add where clause in query  from view 
                '***********************************************************
                sqrdView = SQL.Search("Task_View", "FillView-921", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='2212' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
                If blnView = True Then
                    While sqrdView.Read
                        Select Case CType(sqrdView.Item("UV_VC50_COL_Value"), String).Trim.ToUpper
                            'Case "TM_VC8_SUPP_OWNER"
                            Case "UM_VC50_UserID".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += "  SOwner.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += "  SOwner.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "TM_NU9_COMP_ID_FK"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                                'Case "TM_NU9_ASSIGN_BY"
                            Case "UM_VC50_UserID_Assign".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " ABy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & "  and "
                                Else
                                    strwhereQuery += " ABy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & "  and "
                                End If
                            Case "TM_NU9_PROJECT_ID"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " Project.PR_VC20_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery += " Project.PR_VC20_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "TM_DT8_TASK_DATE"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "TM_DT8_Task_Close_Date".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "TM_DT8_Est_close_date".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                                'CALL RELATED COLUMNS ADDED FOR WHERE CLAUSE 
                            Case "UM_VC50_UserID_ReqBy".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " ReqBy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & "  and "
                                Else
                                    strwhereQuery += " ReqBy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & "  and "
                                End If
                            Case "UM_VC50_UserID_EntBy".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " EntBy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & "  and "
                                Else
                                    strwhereQuery += " EntBy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & "  and "
                                End If
                            Case "UM_VC50_UserID_Coord".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strwhereQuery += " Coord.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & "  and "
                                Else
                                    strwhereQuery += " Coord.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & "  and "
                                End If
                            Case "CM_DT8_Request_Date".ToUpper ' Call Request Date
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_Call_Close_Date".ToUpper ' CallCloseDate
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_Close_Date".ToUpper ' CallEstClsDate
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "isnull(convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_CAll_Start_Date".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "isnull(convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                                'End Call Columns in task view
                            Case Else
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                        End Select
                    End While
                    sqrdView.Close()
                    strwhereQuery = strwhereQuery.Remove(Len(strwhereQuery) - 4, 4)
                End If
                'If shJoin = 1 Then
                '    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom  and Coord.UM_IN4_Address_No_FK =* T040011.CM_NU9_Coordinator "
                'ElseIf shJoin = 2 Then
                '    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                '    'strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom"
                'ElseIf shJoin = 3 Then
                '    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                If shJoin = 1 Then

                    'and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK
                    'and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK
                    'and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK
                    'and actionTable.AM_DT8_Action_Date>='5-Oct-2009'
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If


                ElseIf shJoin = 2 Then
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        ' dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator  and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If

                ElseIf shJoin = 3 Then
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                Else
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = True Then
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Comp_ID_FK=Task.TM_NU9_Comp_ID_FK and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                    If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                        'dtToDate.Text = Year(dtToDate.Text) & "-" & MonthName(Month(dtToDate.Text), True) & "-" & (Day(dtToDate.Text) + 1)
                        strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord,T040031 actionTable where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator and actionTable.AM_NU9_Task_Number=Task.TM_NU9_Task_no_PK and actionTable.AM_NU9_Call_Number=Task.TM_NU9_Call_No_FK and actionTable.AM_DT8_Action_Date>='" & dtFromDate.Text & "' and convert(datetime,convert(varchar,actionTable.AM_DT8_Action_Date,101))<='" & dtToDate.Text & "'"
                    End If
                End If
                If ViewState("TVmshCall") = 1 Then
                    ' strSelect &= " and TM_VC50_Deve_Status='Closed' "
                Else
                    'strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
                End If
                If strwhereQuery.Equals(" and ") = True Then
                    'Nothing added in query
                Else
                    'if got some data from database then add in query
                    strSelect &= strwhereQuery
                End If
                strSelect &= " and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK "
                'Added company chk from company access table
                strSelect &= " and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
                If IsNothing(strUnsortQuery) = False Then
                    strUnsortQuery = strUnsortQuery.TrimEnd
                    strUnsortQuery = strUnsortQuery.Remove(Len(strUnsortQuery) - 1, 1)
                    If IsNothing(strOrderQuery) = True Then
                        'strSelect &= strUnsortQuery
                    Else
                        'strSelect &= strOrderQuery & " " & strUnsortQuery
                    End If
                Else
                    If strOrderQuery.Equals(" order by ") = False Then
                        ' strOrderQuery = strOrderQuery.TrimEnd
                        'strOrderQuery = strOrderQuery.Remove(Len(strOrderQuery) - 1, 1)
                        '  strSelect &= strOrderQuery
                    End If
                End If

                mintCallNoPlace = -1
                mintCompId = ""
                mintSuppOwnID = ""
                mintSuppOwn = ""
                mintAssignBy = ""
                mintAssignByID = ""
                mintTaskNoRowID = ""
                'Added new for call columns
                mstrCallEntBy = ""
                mstrCallEntByID = ""
                mstrCallReqBy = ""
                mstrCallReqByID = ""
                mstrCoordinator = ""
                mstrCoordinatorID = ""
                relatedCallColumnNo = ""

                If SQL.Search("T040021", "Task_View", "FillView-1002", strSelect, dsFromView, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO" Then
                            mintCallNoPlace = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID" Then
                            mintCompId = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPOWNID" Then
                            mintSuppOwnID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKOWNER" Then
                            mintSuppOwn = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBY" Then
                            mintAssignBy = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBYID" Then
                            mintAssignByID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKNO" Then
                            mintTaskNoRowID = inti
                        End If
                        'Added for new Call Columns
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqBy".ToUpper Then
                            mstrCallReqBy = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqByID".ToUpper Then
                            mstrCallReqByID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallEntBy".ToUpper Then
                            mstrCallEntBy = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallEntByID".ToUpper Then
                            mstrCallEntByID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "Coordinator".ToUpper Then
                            mstrCoordinator = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CoordinatorID".ToUpper Then
                            mstrCoordinatorID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "RelatedCall".ToUpper Then
                            relatedCallColumnNo = inti
                        End If
                    Next

                    mdvtable.Table = dsFromView.Tables(0)

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("SubTaskDesc", 45)
                    htGrdColumns.Add("CallDesc", 44)
                    htGrdColumns.Add("CallSubject", 29)
                    htGrdColumns.Add("TaskDesc", 45)

                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNumber"), ViewState("ActionNumber"))
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    ' GrdAddSerach.Columns.Clear()
                    GrdAddSerach.Visible = True
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                        GrdAddSerach.AllowPaging = True
                        'GrdAddSerach.PageSize = mintPageSize
                    End If

                    '*************************************************************************
                    rowvalue = 0
                    rowvalueCall = 0

                    If ViewState("TaskViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        'CurrentPg.Text = 1
                        ViewState("SortOrder") = Nothing
                    End If

                    If Val(ViewState("DepTask")) = 1 Then
                        Dim arrTemp As New ArrayList
                        For intI As Integer = 0 To mdvtable.Table.Rows.Count - 1
                            If GetDepStat(mdvtable.Table.Rows(intI).Item("CallNo"), mdvtable.Table.Rows(intI).Item("CompID"), mdvtable.Table.Rows(intI).Item("TaskNo")) = True Then
                                arrTemp.Add(mdvtable.Table.Rows(intI))
                            End If
                        Next
                        For intJ As Integer = 0 To arrTemp.Count - 1
                            mdvtable.Table.Rows.Remove(arrTemp(intJ))
                        Next
                    End If

                    'If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                    '    GrdAddSerach.CurrentPageIndex = 0
                    '    CurrentPg.Text = 1
                    'End If

                    'GrdAddSerach.DataSource = mdvtable.Table
                    'GrdAddSerach.DataBind()
                    '********************************************************
                    'paging count
                    Dim intRows As Integer = mdvtable.Table.Rows.Count
                    Dim _totalPages As Double = 1
                    Dim _totalrecords As Int32
                    'If Not Page.IsPostBack Then
                    '    _totalrecords = intRows
                    '    _totalPages = _totalrecords / mintPageSize
                    '    TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                    '    TotalRecods.Text = _totalrecords
                    'Else
                    '    _totalrecords = intRows
                    '    If CurrentPg.Text = 0 And _totalrecords > 0 Then
                    '        CurrentPg.Text = 1
                    '    End If
                    '    If _totalrecords = 0 Then
                    '        CurrentPg.Text = 0
                    '    End If
                    '    _totalPages = _totalrecords / mintPageSize
                    '    TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                    '    _totalPages = Double.Parse(TotalPages.Text)
                    '    TotalRecods.Text = _totalrecords
                    'End If
                    'cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    'cpnlTaskView.Enabled = True
                    'cpnlTaskView.TitleCSS = "test"
                    '***********************************************
                Else
                    GrdAddSerach.Visible = False
                    ' Panel1.Visible = False
                    lstError.Items.Clear()
                    ' DisplayMessage("No Task Assigned or data not exist according to view query...")
                    lstError.Items.Add("No Task Assigned or data not exist according to view query...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    'cpnlTaskView.Enabled = False
                    'cpnlTaskView.TitleCSS = "test2"
                End If
            Else
                Exit Function
            End If
            Return mdvtable.Table
        Catch ex As Exception
            CreateLog("Task_View", "FillView-1021", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function

#End Region
    Protected Sub GrdAddSerach_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles GrdAddSerach.ColumnCreated
        Try
            Dim col As GridColumn = e.Column
            If e.Column.ColumnType = "GridBoundColumn" Then
                Dim col1 As GridBoundColumn = e.Column

                'If col1.HeaderText = "%Calculate_Hours" Then
                '    ' col1.DataField = "%Calculate_Hours"
                '    col1.Aggregate = GridAggregateFunction.Sum
                'End If
            End If

            col.AutoPostBackOnFilter = True
           
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
                    Dim TaskNo As String = dataItem("TaskNo").Text
                    Dim dt As New DataTable
                    'Call Function To load Tasks for a particular call of a particular company
                    dt = dtActionsData(Callid, Compid, TaskNo)
                    e.DetailTableView.DataSource = dt

                Case "TaskActions"
                    Dim taskid As Int32 = Convert.ToInt32(dataItem("TaskNo").Text)
                    Dim Callid As Int32 = Convert.ToInt32(dataItem("CallNo").Text)
                    Dim Compid As String = dataItem("CompID").Text
                    Dim dt As New DataTable
                    'Call Function To load Actions for a particular call of a particular company
                    dt = dtActionsData(Callid, Compid, taskid)
                    e.DetailTableView.DataSource = dt
                    'e.DetailTableView.DataBind()
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GrdAddSerach_GroupsChanging(ByVal source As Object, ByVal e As Telerik.Web.UI.GridGroupsChangingEventArgs) Handles GrdAddSerach.GroupsChanging
        '    Try

        If e.Action = GridGroupsChangingAction.Group Then
            '  If e.Expression.GroupByFields(0).FieldName = "TaskOwner" Then
            Dim str As String = String.Empty
            Select Case e.Expression.GroupByFields(0).FieldName
                Case "TaskOwner"
                    str = "TaskOwner [Task Owner],Sum(Actual_Hours) [Actual Hours],Sum(EstHr) [Est Hrs]  Group By TaskOwner"
                Case "TaskType"
                    str = "TaskType [Task Type], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]   Group By TaskType"
                Case "SubCategory"
                    str = "SubCategory [Sub Category], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]  Group By SubCategory"
                Case "EstHr"
                    str = "EstHr [Est Hrs ], Sum(Actual_Hours) [Actual Hours]  ,Sum(EstHr) [Est Hrs] Group By EstHr"
                Case "Actual_Hours"
                    str = "Actual_Hours [Actual Hours], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]  Group By Actual_Hours"
                Case "EstCloseDt"
                    str = "EstCloseDt [EstCloseDt], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]  Group By EstCloseDt"
                Case "CallNo"
                    str = "CallNo [Call No], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]  Group By CallNo"
                Case "TaskNo"
                    str = "TaskNo [Task No], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs] Group By TaskNo"
                Case "CompID"
                    str = "CompID [Comp ID], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]  Group By CompID"
                Case "SuppOwnID"
                    str = "SuppOwnID [SuppOwnID], Sum(Actual_Hours) [Actual Hours] ,Sum(EstHr) [Est Hrs]  Group By SuppOwnID"
                Case "AssignByID"
                    str = "AssignByID [AssignByID], Sum(Actual_Hours) [Actual Hours]  ,Sum(EstHr) [Est Hrs] Group By AssignByID"
            End Select
            ViewState("StingExpression") = str
            ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:handleOnClick();</script>", False)
        End If
    End Sub
    Private Sub CustomizeExpression(ByVal expression As GridGroupByExpression)

        Dim existing As GridGroupByField = expression.SelectFields.FindByName("Actual_Hours")
        If existing Is Nothing Then
            'field is not present
            'Construct and add a new aggregate field 
            Dim field As New GridGroupByField()
            field.FieldName = "Actual_Hours"
            field.FieldAlias = "SubTotal"
            field.Aggregate = GridAggregateFunction.Sum
            'field.FormatString = "{0:C}"
            expression.SelectFields.Add(field)
        Else

        End If
    End Sub

    Private Function IsGroupedByCustomer(ByVal groups As GridGroupByExpressionCollection) As Boolean
        For Each e As GridGroupByExpression In groups
            For Each f As GridGroupByField In e.GroupByFields
                If f.FieldName = "TaskOwner" Then
                    Return True
                End If
            Next
        Next
        Return False
    End Function

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
                    Dim box1 As TextBox = CType(filteringItem("TaskDesc").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(300)
                    Dim box2 As TextBox = CType(filteringItem("CallSubject").Controls(0), TextBox)
                    box2.Width = Unit.Pixel(200)
                    Dim box3 As TextBox = CType(filteringItem("CallDesc").Controls(0), TextBox)
                    box3.Width = Unit.Pixel(300)
                    'Dim box3 As RadNumericTextBox = CType(filteringItem("TotalEstTime").Controls(0), RadNumericTextBox)
                    'box3.Width = Unit.Pixel(25)
                    'Dim box4 As RadNumericTextBox = CType(filteringItem("TotalRptTime").Controls(0), RadNumericTextBox)
                    'box4.Width = Unit.Pixel(25)
                    'Dim box5 As TextBox = CType(filteringItem("Tmp_Type").Controls(0), TextBox)
                    'box5.Text = "Tmp Type"
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
            Dim AttachmentExists As String = String.Empty
            Dim CommentsExists As String = String.Empty
            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then
                If TypeOf e.Item Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim Callno As String = CType(e.Item.DataItem, DataRowView)("CallNo")
                    Dim CompId As String = CType(e.Item.DataItem, DataRowView)("CompID")
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
                        imgComm.Attributes("onclick") = [String].Format("return ShowCommentsForm('{0}','{1}','{2}');", CompId, Callno, e.Item.ItemIndex)
                    ElseIf (CommentsExists = "1") Then
                        'If comments are already read
                        imgComm.ImageUrl = "../../images/comment2.gif"
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("return ShowCommentsForm('{0}','{1}','{2}');", CompId, Callno, e.Item.ItemIndex)
                    ElseIf (CommentsExists = "2") Then
                        'if new Comments are posted
                        imgComm.ImageUrl = "../../images/comment_Unread.gif"
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("return ShowCommentsForm('{0}','{1}','{2}');", CompId, Callno, e.Item.ItemIndex)
                    End If
                End If
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
                If (gridcolumn.UniqueName = "F") Then
                    gridcolumn.Visible = False
                End If
                If TypeOf e.Item Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                    If (gridcolumn.UniqueName = "TaskOwner") Then
                        blnCallReqBy = True
                        Dim Callno As String = dataItem("CALLNO").Text
                        Dim CompId As String = dataItem("COMPID").Text
                        Dim CallOwner As String = dataItem("TaskOwner").Text
                        Dim tb As TableCell = dataItem("TaskOwner")
                        Dim lnk As New LinkButton
                        lnk.Text = dataItem("TaskOwner").Text
                        lnk.EnableViewState = True

                        lnk.ID = dataItem("TaskOwner").Text
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
                        Dim lnkCallReqBy1 As String = lnk.ID

                        lnk.Attributes("href") = "#"
                        'lnk.Attributes("onclick") = [String].Format("return ShowCallReqByInfo('{0}','{1}','{2}','{3}');", CompId, Callno, CallOwner, e.Item.ItemIndex)
                        tb.Controls.Add(lnk)
                        Dim hlAddProducts As Control = e.Item.FindControl(lnkCallReqBy1)
                        If IsNothing(hlAddProducts) = False Then
                            Dim currentRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                            If Not controlCollection1.ContainsKey(hlAddProducts.ClientID) Then
                                controlCollection1.Add(hlAddProducts.ClientID, lnk.Text)
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

            If TypeOf e.Item Is GridGroupHeaderItem Then
                '    Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
                '    Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                '    item.DataCell.Text = (item.DataCell.Text + "; Total Actual Hours Taken: ")
                '    'item.DataCell.Text = (item.DataCell.Text + (CType(groupDataRow("Actual_Hours"), Decimal) / Integer.Parse(groupDataRow("Actual_Hours").ToString)).ToString)
                '    item.DataCell.Text = (item.DataCell.Text + groupDataRow("Actual_Hours").ToString())
                '    item.DataCell.Text = (item.DataCell.Text + "Total Estimated Hours: ")
                '    item.DataCell.Text = (item.DataCell.Text + groupDataRow("EstHr").ToString())

            End If


        Catch ex As Exception
            'CreateLog("Task_View", "ItemDataBound-1626", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdaddserach")
        End Try
    End Sub
    Function GetMonStat(ByVal callNo As String, ByVal taskno As String, ByVal compID As String) As String

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "Setup_Rules"
        SQL.DBTracing = False
        Dim intRows As Integer
        Try
            SQL.Search("Task View", "GetMonstat-1976", "select table_id from Setup_Rules where Call_No=" & callNo & " and Task_No=" & taskno & " and Company_id in(select CI_NU8_Address_Number from t010011 where CI_VC36_Name='" & compID & "' and CI_VC8_Address_Book_Type='COM')", intRows)
            If intRows > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Task_View", "GetMonstat-1976", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Protected Sub GrdAddSerach_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles GrdAddSerach.NeedDataSource
        Try
            If Not e.IsFromDetailTable Then
                If ViewState("TaskViewName") <> "" And ViewState("TaskViewName") <> "Default" Then
                    ' fill datagrid based on user define columns and combination
                    Dim dt As DataTable = Fillview()
                    GrdAddSerach.DataSource = dt
                    Session("TaskHicry") = dt
                Else
                    'fill tha datagrid from based on admin defined to the role
                    Dim dt As DataTable = FillDefault()
                    GrdAddSerach.DataSource = dt
                    ViewState("TaskViewName") = "Default"
                    Session("TaskHicry") = dt
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

    Private Function dtActionsData(ByVal Callno As Int32, ByVal CompID As String, Optional ByVal TaskNo As Int32 = 0) As DataTable
        Dim dt As New DataTable
        Dim Callid As String = Callno
        Dim Companyid As Int32 = WSSSearch.SearchCompName(CompID).ExtraValue

        Dim TaskNumber As String = TaskNo
        Dim conn As New SqlConnection()
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        conn.Open()
        Dim query As String
        If (Callid <> 0 And Companyid <> 0 And TaskNumber <> 0) Then
            If dtFromDate.Text.Equals("") = False And dtToDate.Text.Equals("") = False Then
                query = "select   AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK='" & Companyid.ToString() & "' and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date >='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
            ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = False Then
                query = "select  AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK='" & Companyid.ToString() & "' and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
            ElseIf dtToDate.Text.Equals("") = True And dtFromDate.Text.Equals("") = False Then
                query = "select  AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK='" & Companyid.ToString() & "' and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date >='" & dtFromDate.Text & "'"
            ElseIf dtFromDate.Text.Equals("") = True And dtToDate.Text.Equals("") = True Then
                query = "select  AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Callno.ToString & " and AM_NU9_Comp_ID_FK='" & Companyid.ToString() & "' and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "'"
            End If
            'query = "SELECT * FROM T040031 where AM_NU9_Call_Number=" & Callid & " and AM_NU9_Comp_ID_FK=" & Companyid & "and AM_NU9_Task_Number=" & TaskNumber
        Else
            query = "select AM_NU9_Comp_ID_FK,AM_NU9_Call_Number,AM_NU9_Task_Number,AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID as UM_VC50_UserID,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner and AM_NU9_Comp_ID_FK=" & CompID.ToString() & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number='" & TaskNumber.ToString() & "' and a.AM_DT8_Action_Date >='" & dtFromDate.Text & "' and a.AM_DT8_Action_Date <='" & dtToDate.Text & "'"
        End If
        Dim ad As New SqlDataAdapter
        Dim cmd As New SqlCommand(query)
        cmd.Connection = conn
        ad.SelectCommand = cmd
        ad.Fill(dt)
        conn.Close()
        Return dt
    End Function

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

    Protected Sub Logout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Logout.Click
        LogoutWSS()
    End Sub

    Protected Sub GrdAddSerach_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdAddSerach.PreRender
        'Dim col As GridGroupHeaderItem
        'For Each col In GrdAddSerach.MasterTableView.Items

        'Next
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
        Try
            If ViewState("F") = 1 Then
                For Each item As GridGroupHeaderItem In GrdAddSerach.MasterTableView.GetItems(GridItemType.GroupHeader)
                    'Dim grpIndx As String = item.GroupIndex.ToString()
                    'If grpIndx.Length >= 5 Then
                    '    If grpIndx.Substring(grpIndx.Length - 2) = "_0" Then
                    Dim dtaItem As Integer = TraverseGridGroupForDataItem(item)
                    item.DataCell.Text = (item.DataCell.Text + "; Avg: ")
                    'If dtaItem IsNot Nothing Then
                    '    Dim lnk As New HyperLink()
                    '    lnk.ID = "lnkAccountCreditScreen"
                    '    lnk.Text = "Change Accounting Limits"
                    '    lnk.NavigateUrl = "#"
                    '    lnk.Attributes("onclick") = String.Format("return ShowAccountCreditScreen('{0}', '{1}');", dtaItem("AccountId").Text, lnk.ClientID)

                    '    item.DataCell.Controls.Add(lnk)
                    'End If
                    item.DataCell.Text = item.DataCell.Text & dtaItem
                    '        End If
                    '    End If
                Next
            End If
            GrdAddSerach.MasterTableView.Rebind()
        Catch ex As Exception

        End Try




    End Sub
    Private Function TraverseGridGroupForDataItem(ByVal grpHdrItem As GridGroupHeaderItem) As Integer
        Try
            Dim dataItem As Integer = 0
            Dim groupDataRow As DataRowView = CType(grpHdrItem.DataItem, DataRowView)

            If IsDBNull(groupDataRow("Actual_Hours")) = True Then
                dataItem = 0
            ElseIf groupDataRow("EstHr").ToString.Equals("0") = True Or groupDataRow("Actual_Hours").ToString.Equals("0") Then
                dataItem = 0
            Else
                Dim dbcontrol As Double = (CType(groupDataRow("Actual_Hours"), Decimal) / CType(groupDataRow("EstHr"), Decimal)) * 100
                dataItem = dbcontrol
            End If
           
            Return dataItem
        Catch ex As Exception

        End Try

    End Function



    'Private Sub ExportGridView()


    '    Dim attachment As String = "attachment; filename=Contacts.xls"

    '    Response.ClearContent()

    '    Response.AddHeader("content-disposition", attachment)

    '    Response.ContentType = "application/ms-excel"

    '    Dim sw As New StringWriter()

    '    Dim htw As New HtmlTextWriter(sw)

    '    'GrdAddSerach.RenderControl(htw)

    '    Response.Write(sw.ToString())


    '    Response.[End]()
    'End Sub


    'Private Sub PrepareGridViewForExport(ByVal gv As Control)

    '    Dim lb As New LinkButton()
    '    Dim l As New Literal()
    '    Dim name As String = String.Empty
    '    For i As Integer = 0 To gv.Controls.Count - 1
    '        If gv.Controls(i).[GetType]() Is GetType(LinkButton) Then
    '            l.Text = TryCast(gv.Controls(i), LinkButton).Text
    '            gv.Controls.Remove(gv.Controls(i))
    '            gv.Controls.AddAt(i, l)
    '        ElseIf gv.Controls(i).[GetType]() Is GetType(DropDownList) Then
    '            l.Text = TryCast(gv.Controls(i), DropDownList).SelectedItem.Text
    '            gv.Controls.Remove(gv.Controls(i))
    '            gv.Controls.AddAt(i, l)
    '        ElseIf gv.Controls(i).[GetType]() Is GetType(CheckBox) Then
    '            l.Text = If(TryCast(gv.Controls(i), CheckBox).Checked, "True", "False")
    '            gv.Controls.Remove(gv.Controls(i))
    '            gv.Controls.AddAt(i, l)
    '        End If
    '        If gv.Controls(i).HasControls() Then
    '            PrepareGridViewForExport(gv.Controls(i))
    '        End If
    '    Next
    'End Sub

    Protected Sub imgExportToExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToExcel.Click
        'Dim xlApp As New Excel.Application
        'Dim xlBook As Excel.Workbook
        'Dim xlSheet As Excel.Worksheet
        'Dim xlRange As Excel.Range
        'GrdAddSerach.Rebind()

        'PrepareGridViewForExport(GrdAddSerach)

        'ExportGridView()

        'TryCast(GrdAddSerach.MasterTableView.GetItems(GridItemType.Header), GridHeaderItem).Cells.Visible = False
        'For Each gpheaderItem As GridGroupHeaderItem In GrdAddSerach.MasterTableView.GetItems(GridItemType.GroupHeader)

        '    gpheaderItem.Cells.Visible = False
        'Next


        'HttpContext.Current.Response.Clear()
        'HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", fileName))
        'HttpContext.Current.Response.ContentType = "application/ms-excel"
        'Dim sw As StringWriter = New StringWriter
        'Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        ''  Create a form to contain the grid
        'Dim table As Table = New Table
        'table.GridLines = GrdAddSerach.GridLines ' gv.GridLines

        ''  add the header row to the table
        'If (Not (GrdAddSerach. .HeaderRow) Is Nothing) Then
        '    PrepareControlForExport(GrdAddSerach.MasterTableView.HeaderRow)
        '    table.Rows.Add(gv.HeaderRow)
        'End If
        ''  add each of the data rows to the table
        'For Each row As GridViewRow In gv.Rows
        '    GridViewExportUtil.PrepareControlForExport(row)
        '    table.Rows.Add(row)
        'Next
        ''  add the footer row to the table
        'If (Not (gv.FooterRow) Is Nothing) Then
        '    GridViewExportUtil.PrepareControlForExport(gv.FooterRow)
        '    table.Rows.Add(gv.FooterRow)
        'End If
        ''  render the table into the htmlwriter
        'table.RenderControl(htw)
        ''  render the htmlwriter into the response
        'HttpContext.Current.Response.Write(sw.ToString)
        'HttpContext.Current.Response.End()
        GrdAddSerach.MasterTableView.ExportToExcel()
    End Sub
    'Private Shared Sub PrepareControlForExport(ByVal control As Control)
    '    Dim i As Integer = 0
    '    Do While (i < control.Controls.Count)
    '        Dim current As Control = control.Controls(i)
    '        If (TypeOf current Is LinkButton) Then
    '            control.Controls.Remove(current)
    '            control.Controls.AddAt(i, New LiteralControl(CType(current, LinkButton).Text))
    '        ElseIf (TypeOf current Is ImageButton) Then
    '            control.Controls.Remove(current)
    '            control.Controls.AddAt(i, New LiteralControl(CType(current, ImageButton).AlternateText))
    '        ElseIf (TypeOf current Is HyperLink) Then
    '            control.Controls.Remove(current)
    '            control.Controls.AddAt(i, New LiteralControl(CType(current, HyperLink).Text))
    '        ElseIf (TypeOf current Is DropDownList) Then
    '            control.Controls.Remove(current)
    '            control.Controls.AddAt(i, New LiteralControl(CType(current, DropDownList).SelectedItem.Text))
    '        ElseIf (TypeOf current Is CheckBox) Then
    '            control.Controls.Remove(current)
    '            control.Controls.AddAt(i, New LiteralControl(CType(current, CheckBox).Checked))
    '            'TODO: Warning!!!, inline IF is not supported ?
    '        End If
    '        If current.HasControls Then
    '            PrepareControlForExport(current)
    '        End If
    '        i = (i + 1)
    '    Loop
    'End Sub
    Protected Sub imgExportToWord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToWord.Click
        GrdAddSerach.MasterTableView.ExportToWord()

    End Sub

    'Protected Sub hid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hid.Click
    '    Me.GrdAddSerach.Rebind()
    'End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Me.GrdAddSerach.Rebind()
    End Sub

    Protected Sub btnGroupBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGroupBy.Click
        Dim str As String = ViewState("StingExpression")
        Dim expression1 As GridGroupByExpression = GridGroupByExpression.Parse(str) '"TaskOwner [Task Owner], Sum(Actual_Hours) GroupTotal [GroupTotal]  Group By TaskOwner")
        Me.CustomizeExpression(expression1)
        Me.GrdAddSerach.MasterTableView.GroupByExpressions.Add(expression1)
        ViewState("F") = 1
        GrdAddSerach.MasterTableView.Rebind()
    End Sub
End Class
