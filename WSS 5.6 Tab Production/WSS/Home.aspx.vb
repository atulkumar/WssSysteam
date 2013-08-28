Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data

Partial Class Home
    Inherits System.Web.UI.Page
    Private dsAllMsg As New DataSet
    'Protected WithEvents hylCommentView As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents pnlMsg As System.Web.UI.WebControls.Panel
    Private dvTempMsg As New DataView

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents BtnGrdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents imgbtnSearch As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    'Protected WithEvents cpnlError As CustomControls.Web.CollapsiblePanel
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    'Protected WithEvents cpnlCallView As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents cpnlCallTask As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents grdCall As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents grdTask As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents Panel2 As System.Web.UI.WebControls.Panel
    'Protected WithEvents cpnlCalender As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents cpnlAlerts As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents calHome As System.Web.UI.WebControls.Calendar
    'Protected WithEvents IONPOP1 As ION.Web.PopupWin
    'Protected WithEvents lblTitleLabelWssHome As System.Web.UI.WebControls.Label
    'Protected WithEvents dgrCommentAlert As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Private Shared intID As Int16
    Private Shared mdvCommentAlert As DataView


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
        'Code to open or set  Start Page 
        Dim intScreenId As Integer = GetScreenID(HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropRole"))
        If intScreenId > 0 Then
            If intScreenId <> 389 Then
                Dim screenName As String = SQL.Search("StartLocation", "GetScreenID", "select OBM_VC200_URL  from t070011 where OBM_IN4_Object_ID_PK=" & intScreenId)
                'If screenName.IndexOf("?") > 0 Then
                '    screenName = screenName.Remove(screenName.IndexOf("?"), (screenName.Length - screenName.IndexOf("?")))
                'End If
                screenName = screenName.Trim.Remove(0, 3)
                If screenName.IndexOf("?") > 0 Then
                    If screenName.Length - 1 = screenName.IndexOf("?") Then
                        Response.Redirect(screenName & "ScrID=" & intScreenId & "&ScreenFrom=HomePage", False)
                    Else
                        Response.Redirect(screenName & "&ScrID=" & intScreenId & "&ScreenFrom=HomePage", False)
                    End If
                Else
                    Response.Redirect(screenName & "?ScrID=" & intScreenId & "&ScreenFrom=HomePage", False)
                End If
                Exit Sub
            End If
        End If
        If Page.IsPostBack = False Then
            txtCSS(Me.Page)
        End If
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        '  Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'cpnlError.Visible = False
        'Assigning Root Directory
        HttpContext.Current.Session("PropRootDir") = Server.MapPath("")
        Call GetAllMessages()
        IONPOP1.Visible = False
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            BindCommentAlertGrid()

            Dim blnStatus As Boolean
            Dim sqRDR As SqlDataReader

            sqRDR = SQL.Search("Home", "Load", "select top 1 * from T090011 where CL_VC4_Status='ENA' and CL_NU9_Event_Owner_FK=" & Session("PropUserID") & " and CL_NU9_CompID_FK=" & Session("PropCompanyID") & " and CL_DT8_EventDate='" & Today & "' order by CL_DT8_EventDate desc", SQL.CommandBehaviour.SingleRow, blnStatus)

            If blnStatus = True Then
                While sqRDR.Read

                    IONPOP1.HideAfter = 15000
                    IONPOP1.Visible = True
                    IONPOP1.OffsetY = 87
                    IONPOP1.OffsetX = 29
                    IONPOP1.WidthPopup = 449
                    IONPOP1.HeightPopup = 455
                    IONPOP1.Width = Unit.Pixel(250)
                    IONPOP1.Height = Unit.Pixel(100)
                    IONPOP1.Link = "MessageCenter/HomeCalender/Events.aspx?strDate=" & Today
                    Dim strTitle As String
                    strTitle = sqRDR("CL_VC50_Subject")
                    If strTitle.Length > 23 Then
                        strTitle = strTitle.Substring(0, 23) & "....."
                    End If
                    IONPOP1.Title = "<font  size=1  face=verdana ><B>" & sqRDR("CL_VC8_Event") & "&nbsp; :&nbsp;&nbsp; </b>" & strTitle & "</font>"
                    Dim strMsg As String
                    strMsg = sqRDR("CL_VC500_Message")
                    If strMsg.Length > 80 Then
                        strMsg = strMsg.Substring(0, 80) & "......."
                    End If
                    IONPOP1.Message = "<font  size=1  face=verdana >" & strMsg & "</font><BR><font face=verdana color=red size=1>Click To View Detail</font>"
                    IONPOP1.PopupSpeed = 10
                    IONPOP1.DragDrop = True
                    IONPOP1.DockMode = ION.Web.PopupDocking.BottomRight
                    IONPOP1.ColorStyle = ION.Web.PopupColorStyle.Green
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("Home", "Load-149", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("Home", "Load-117", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        lstError.Items.Clear()


        If Page.IsPostBack = False Then
            Dim dsCalls As New DataSet
            Dim strCallSQL As String
            Try
                Dim strView As String
                strView = SQL.Search("", "", "select ROD_CH1_View_Hide from T070042 where ROD_IN4_Role_ID_FK=" & HttpContext.Current.Session("PropRole") & " and ROD_IN4_Object_ID_FK=4", "")
                If strView.Trim.ToUpper.Equals("V") Then

                    'If Session("PropCompanyType") = "SCM" Then
                    ' strCallSQL = "select top 15  CM_NU9_Call_No_PK,CI_VC36_Name,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CM_VC200_Work_Priority,CN_VC20_Call_Status from T040011, T010011 b where T040011.CM_NU9_Comp_Id_FK=b.CI_NU8_Address_Number and cn_VC20_Call_Status<>'CLOSED'   order by CM_NU9_Call_No_PK desc "
                    'strCallSQL = "select top 15  CM_NU9_Call_No_PK,CI_VC36_Name, CM_VC8_Call_Type, CM_VC2000_Call_Desc, CM_VC200_Work_Priority, CN_VC20_Call_Status from T040011, T010011 b where T040011.CM_NU9_Comp_Id_FK=b.CI_NU8_Address_Number and cn_VC20_Call_Status<>'CLOSED'  and CM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and CM_NU9_Project_ID in (select distinct PM_NU9_Project_ID_FK from T210012 where PM_NU9_Project_Member_ID=" & Session("PropUserID") & ") order by CM_NU9_Call_No_PK desc"
                    'Else
                    'strCallSQL = "select top 15  CM_NU9_Call_No_PK,CI_VC36_Name,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CM_VC200_Work_Priority,CN_VC20_Call_Status from T040011, T010011 b where T040011.CM_NU9_Comp_Id_FK=b.CI_NU8_Address_Number and cn_VC20_Call_Status<>'CLOSED'  and t040011.cm_nu9_comp_id_fk=" & HttpContext.Current.Session("PropCompanyID") & " order by CM_NU9_Call_No_PK desc "
                    'End If
                    strCallSQL = "select CM_NU9_Call_No_PK,CI_VC36_Name,CI_NU8_Address_Number as CompanyId ,CM_VC8_Call_Type, CM_VC2000_Call_Desc, CM_VC200_Work_Priority, CN_VC20_Call_Status from T040011, T010011 b where T040011.CM_NU9_Comp_Id_FK=b.CI_NU8_Address_Number and cn_VC20_Call_Status<>'CLOSED'  and CM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and CM_NU9_Call_Owner=" & Val(Session("PropUserID")) & " AND CM_NU9_Comp_ID_FK IN (" & GetCompanySubQuery() & ") order by CM_NU9_Call_No_PK desc"
                    If SQL.Search("T040011", "Home", "Load", strCallSQL, dsCalls, "", "") = True Then
                        Dim htDescCols As New Hashtable
                        htDescCols.Add("CM_VC2000_Call_Desc", 30)
                        grdCall.DataSource = HTMLEncodeDecode(mdlMain.Action.Encode, dsCalls.Tables("T040011"), htDescCols)
                        grdCall.DataBind()
                    Else
                        'lstError.Items.Add("No Call opened by " & StrConv(Session("PropUserName"), VbStrConv.ProperCase) & " so far...")
                        'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                    End If
                Else

                    lstError.Items.Add("You don't have rights to view your recent calls...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)

                End If
                Dim intRows As Integer
                If SQL.Search("", "", "select * from T070042 where ROD_IN4_Role_ID_FK=" & HttpContext.Current.Session("PropRole") & " and ROD_IN4_Object_ID_FK=8  and ROD_CH1_View_Hide='V'", intRows, "") = True Then
                    'Response.Redirect("SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=4", False)
                    dsCalls = New DataSet
                    If SQL.Search("T040021", "", "", "select TM_NU9_Call_No_FK,TM_NU9_Task_no_PK,CI_VC36_Name,CI_NU8_Address_Number as CompanyId, TM_VC1000_Subtsk_Desc, b.UM_VC50_UserID TM_VC8_Supp_Owner, TM_VC50_Deve_status TaskStatus from T040021 a,T060011 b,T010011 c where a.TM_NU9_Comp_Id_FK=c.CI_NU8_Address_Number and a.TM_VC8_Supp_Owner =b.UM_IN4_Address_No_FK and TM_VC8_Supp_Owner=" & Val(Session("PropUserID")) & " and TM_VC50_Deve_Status<>'Closed' AND TM_NU9_Comp_Id_FK IN (" & GetCompanySubQuery() & ") order by TM_NU9_Call_No_FK desc", dsCalls, "", "") = True Then

                        'Else

                        'End If
                        'mstGetFunctionValue = WSSSearch.SearchTask(HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropCompanyID"), dsCalls)
                        ' If mstGetFunctionValue.ErrorCode = 0 Then
                        Dim htDescCols As New Hashtable
                        htDescCols.Add("TM_VC1000_Subtsk_Desc", 30)
                        grdTask.DataSource = HTMLEncodeDecode(mdlMain.Action.Encode, dsCalls.Tables("T040021"), htDescCols)
                        grdTask.DataBind()
                    Else
                        ' lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                        'cpnlError.Visible = True
                        'lstError.Items.Add("No task assigned to " & StrConv(Session("PropUserName"), VbStrConv.ProperCase) & "...")
                        'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                        ' cpnlError.TitleCSS = "test3"
                    End If

                Else
                    lstError.Items.Add("You don't have rights to view ToDoList...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)

                End If


                'Security Block

                'If Not IsPostBack Then
                '    Dim str As String
                '    str = HttpContext.Current.Session("PropRootDir")
                '    intID = Request.QueryString("ScrID")
                '    intID = 3
                '    Dim obj As New clsSecurityCache
                '    If obj.ScreenAccess(intID) = False Then
                '        Response.Redirect("frm_NoAccess.aspx", False)
                '    End If
                '    obj.ControlSecurity(Me.Page, intID)
                'End If

                'End of Security Block


            Catch ex As Exception
                CreateLog("Home", "Load-148", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            End Try
        End If

        '                    Response.Write("<script>window.open('MessageCenter/HomeCalender/MsgCalender.aspx?strDate=" & Today & "','Event','fullscreen=no,width=440,height=430')</script>")
        Session("PropCAComp") = Nothing
        Session("PropCallNumber") = Nothing
        Session("PropTaskNumber") = Nothing
        Session("PropActionNumber") = Nothing
    End Sub

#End Region

    Private Sub grdCall_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCall.ItemDataBound
        Dim itemType As ListItemType = e.Item.ItemType
        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
            'e.Item.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(button, "")
            'e.Item.Attributes("onclick")
            Dim CallNo As String = grdCall.DataKeys(e.Item.ItemIndex)
            Dim CompId As String = e.Item.Cells(7).Text.Trim()
            e.Item.Attributes.Add("onDBlclick", "javascript:openCallDetailsInTab(" & CallNo & "," & CompId & ")")

        End If

    End Sub

    Private Sub grdCall_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdCall.SelectedIndexChanged
        Try
            HttpContext.Current.Session("PropCallNumber") = grdCall.SelectedItem.Cells(1).Text
            'HttpContext.Current.Session("PropCAComp") = grdCall.SelectedItem.Cells(2).Text

            mstGetFunctionValue = WSSSearch.SearchCompName(grdCall.SelectedItem.Cells(2).Text)
            HttpContext.Current.Session("PropCAComp") = mstGetFunctionValue.ExtraValue

            Dim intRows As Integer
            If SQL.Search("", "", "select * from T070042 where ROD_IN4_Role_ID_FK=" & HttpContext.Current.Session("PropRole") & " and ROD_IN4_Object_ID_FK=3  and ROD_CH1_View_Hide='V'", intRows, "") = True Then
                Response.Redirect("SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=4", False)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("You don't have rights to Edit calls...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
            End If

        Catch ex As Exception
            CreateLog("Home", "SelectedIndexChange-177", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdCall")
        End Try

    End Sub

    Private Sub grdTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdTask.ItemDataBound
        Dim itemType As ListItemType = e.Item.ItemType
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If GetDepStat(Val(e.Item.Cells(1).Text), e.Item.Cells(3).Text, Val(e.Item.Cells(2).Text)) = True Then
                For intI As Integer = 0 To e.Item.Cells.Count - 1
                    ' e.Item.Cells(intI).ForeColor = Color.Blue
                    e.Item.Cells(intI).BackColor = System.Drawing.Color.FromArgb(210, 233, 255)
                Next
            End If

            If GetCallPriority(Val(e.Item.Cells(1).Text), e.Item.Cells(3).Text) = True Then
                For intI As Integer = 0 To e.Item.Cells.Count - 1
                    e.Item.Cells(intI).ForeColor = System.Drawing.Color.Red
                Next
            End If

        End If
        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
            'e.Item.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(button, "")
            Dim CallNo As String = grdTask.DataKeys(e.Item.ItemIndex)
            Dim CompId As String = e.Item.Cells(6).Text.Trim()
            e.Item.Attributes.Add("onDBlclick", "javascript:openToDoListInTab(" & CallNo & "," & CompId & ")")
        End If

        
    End Sub

    Private Sub grdTask_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdTask.SelectedIndexChanged
        Try
            HttpContext.Current.Session("PropCallNumber") = grdTask.SelectedItem.Cells(1).Text
            ' HttpContext.Current.Session("PropCAComp") = grdTask.SelectedItem.Cells(3).Text
            mstGetFunctionValue = WSSSearch.SearchCompName(grdTask.SelectedItem.Cells(3).Text)
            HttpContext.Current.Session("PropCAComp") = mstGetFunctionValue.ExtraValue
            Response.Redirect("WorkCenter/DoList/ToDoList.aspx?ScrID=8", False)
        Catch ex As Exception
            CreateLog("Home", "SelectedIndexChanged-204", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try
        'Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=2")
    End Sub


    Private Sub calHome_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles calHome.DayRender
        Try

            dvTempMsg = dsAllMsg.Tables(0).DefaultView
            If GetFilteredDataView(dvTempMsg, "CL_DT8_EventDate='" & e.Day.Date & "'").Table.Rows.Count > 0 Then
                Dim imgFlag As New System.Web.UI.WebControls.Image
                imgFlag.ImageUrl = "Images\MsgFlag.gif"
                imgFlag.ToolTip = "Click To Check Message"
                e.Cell.Controls.Add(imgFlag)
            End If
            e.Cell.Attributes.Add("onclick", "return ShowEvent('" & e.Day.Date & "');")
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetAllMessages() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.Search("T090011", "", "", "select  * from T090011 where CL_NU9_Event_Owner_FK=" & Val(Session("PropUserID")) & " OR CL_NU9_UserID_FK=" & Val(Session("PropUserID")) & " AND CL_NU9_CompID_FK IN (" & GetCompanySubQuery() & ")", dsAllMsg, "", "")
        Catch ex As Exception
        End Try
    End Function


    Private Function BindCommentAlertGrid()
        Try

            Dim TC As New TemplateColumn
            TC.ItemTemplate = New IONGrid.CreateItemTemplateImage("imgComment", "Images/comment_Unread.gif")
            dgrCommentAlert.Columns.Add(TC)
            Dim strSQL As String
            'strSQL = "select case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, B.UM_VC50_UserID CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_NU9_CompId_Fk from T040061,T060011 A,T060011 B,T010011 where A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and B.UM_IN4_Address_No_FK=*CM_NU9_Comment_To and CI_NU8_Address_Number=CM_NU9_CompId_Fk and  CM_CH1_Flag=1 and (CM_NU9_AB_Number=" & Session("PropUserID") & " or CM_NU9_Comment_To is null or CM_NU9_Comment_To=" & Session("PropUserID") & ") AND CM_NU9_CompId_Fk IN (" & GetCompanySubQuery() & ") order by CallNo desc"
            strSQL = "(select case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, '' CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_NU9_CompId_Fk from T040061,T060011 A,T010011 where A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and CI_NU8_Address_Number=CM_NU9_CompId_Fk and  CM_CH1_Flag=1 and CM_NU9_Call_Number in ( select  CM_NU9_Call_Number from T040011  where  CM_NU9_Comp_Id_FK =CM_NU9_CompId_Fk and CM_NU9_Project_ID in (select PM_NU9_Project_ID_Fk from T210012 where PM_NU9_Comp_ID_FK=CM_NU9_CompId_Fk and PM_NU9_Project_Member_ID=" & Val(Session("PropUserID")) & ")) and CM_NU9_Comment_To is null) union all (select case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, B.UM_VC50_UserID CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_NU9_CompId_Fk from T040061,T060011 A,T010011 ,T060011 B where B.UM_IN4_Address_No_FK=CM_NU9_Comment_To and A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and CI_NU8_Address_Number=CM_NU9_CompId_Fk and  CM_CH1_Flag=1  and CM_NU9_Comment_To=" & Val(Session("PropUserID")) & ")"
            Dim dsCommentAlert As New DataSet
            If SQL.Search("CommentAlert", "", "", strSQL, dsCommentAlert, "", "") = True Then
                mdvCommentAlert = dsCommentAlert.Tables(0).DefaultView
                dgrCommentAlert.DataSource = dsCommentAlert
                dgrCommentAlert.DataBind()
                dgrCommentAlert.Visible = True
            Else
                dgrCommentAlert.Visible = False
            End If


        Catch ex As Exception
            CreateLog("Home", "BindCommentAlertGrid-204", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try
    End Function

    Private Sub dgrCommentAlert_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrCommentAlert.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            If Not IsNothing(e.Item.Cells(0).FindControl("imgComment")) Then
                CType(e.Item.Cells(0).FindControl("imgComment"), System.Web.UI.WebControls.Image).Attributes.Add("OnClick", "OpenComment('" & mdvCommentAlert.Table.Rows(e.Item.ItemIndex).Item("CM_VC2_Flag") & "','" & mdvCommentAlert.Table.Rows(e.Item.ItemIndex).Item("CM_NU9_CompId_Fk") & "','" & mdvCommentAlert.Table.Rows(e.Item.ItemIndex).Item("CallNo") & "','" & mdvCommentAlert.Table.Rows(e.Item.ItemIndex).Item("TaskNo") & "','" & mdvCommentAlert.Table.Rows(e.Item.ItemIndex).Item("ActionNo") & "' )")
            End If

        End If

    End Sub

    Private Sub dgrCommentAlert_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrCommentAlert.ItemCreated
        Try
            For intI As Integer = 0 To e.Item.Cells.Count - 1
                If intI > 6 Then
                    e.Item.Cells(intI + 1).Visible = False
                End If
            Next
        Catch ex As Exception
            'CreateLog("Home", "dgrCommentAlert_ItemCreated-204", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try
    End Sub
    Function GetDepStat(ByVal callNo As String, ByVal compID As String, ByVal TaskNo As String) As Boolean
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "Setup_Rules"
        SQL.DBTracing = False
        Dim intRows As Integer
        Dim SQLQuery As String

        SQLQuery = " select * from T040021 where TM_NU9_Comp_ID_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & compID & "' ) and TM_NU9_Call_No_FK=" & callNo & " and TM_NU9_Task_no_PK=(select isnull(TM_NU9_Dependency,0) from T040021 where TM_NU9_Comp_ID_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & compID & "' ) and TM_NU9_Call_No_FK=" & callNo & " and TM_NU9_Task_no_PK=" & TaskNo & " ) and TM_VC50_Deve_status<>'CLOSED'"

        Try
            SQL.Search("Call_View", "GetMonStat-2409", SQLQuery, intRows)
            If intRows > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("HOME", "GetDepStat-384", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try

    End Function
    Private Function GetCallPriority(ByVal callno As String, ByVal CompName As String) As Boolean

        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBTracing = False
            Dim intRows As Integer
            Dim SQLQuery As String

            SQLQuery = " select * from T040011 where CM_NU9_Call_No_PK=" & callno & "  and CM_NU9_Comp_Id_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & CompName & "' ) and CM_VC200_Work_Priority='1' "

            SQL.Search("HOME", "GetCallPriority-400", SQLQuery, intRows)
            If intRows > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("HOME", "GetCallPriority-408", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try

    End Function
    Private Function GetScreenID(ByVal UserID As Integer, ByVal RoleID As Integer) As Integer
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        Dim strSQL As String = "Select UL_NU9_OBJ_ID from t070051 where UL_NU9_User_ID=" & UserID & " and UL_NU9_Role_ID= " & RoleID

        Dim intScreenID As Integer = SQL.Search("StartLocation", "GetScreenID", strSQL)
        Return intScreenID
    End Function
End Class
