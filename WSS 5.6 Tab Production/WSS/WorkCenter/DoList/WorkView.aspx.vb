Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports ION.Logging
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

Partial Class WorkCenter_DoList_WorkView
    Inherits System.Web.UI.Page
    Protected WithEvents chkMandatoryHr As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TxtDescription_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtUsedHr_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents grdAction As New DataGrid
    Protected WithEvents dtActionDate As DateSelector
    Protected WithEvents TxtActionOwner_F As System.Web.UI.WebControls.TextBox
#Region "global level declaration"

    Private mdvtable As New DataView ' store data from table for view grid 

    ' Private arColumnName As New ArrayList ' this is stored grid's columns name to assined value to the texboxes
    Public introwvalues As Integer
    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 
    Private intColumnCount As Integer  'grid columns count

    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************
    Private arrtextvalue As ArrayList = New ArrayList
    ' Private Shared mTextBox() As TextBox
    '****************************************************
    Private mintPageSize As Integer
    Protected _currentPageNumber As Int32 = 1

    'these variable store the position of the columns
    '****************************************
    Private mintCallNoPlace As Integer
    Private mintCompId As String
    Private mintSuppOwnID As String
    Private mintSuppOwn As String
    Private mintAssignBy As String
    Private mintAssignByID As String
    Private mintTaskNoRowID As String

    '**************************************
    Private txthiddenImage As String 'stored clicked button's caption  
    Public mstrCallNumber As String 'stored call number when click on task grid
    Public mstrTaskNumber As String 'stored task number when click on task grid
    Public mstrcomp As String

    Private mintuserid As Integer
    Private mintFileID As Integer
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Private null As System.DBNull
    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments
    Private tclAction() As TemplateColumn
    Private mTaskRowValue As Integer

    Private dtvAction As New DataView
    Private intComp As String


#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'src="../../Login/Login.aspx" 
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        Dim strFilter As String
        Dim strSearch As String

        If IsPostBack = False Then
            txtCSS(Me.Page)
            ViewState("TaskNo") = 0
            'sorting session variable clear at page first time load 
            ViewState("SortOrder") = Nothing
            Session("SortWay") = 0

            ViewState("SortWayAction") = Nothing
            ViewState("SortOrderAction") = Nothing

            Dim arColWidth As New ArrayList
            Dim arrTextboxId As New ArrayList
            Dim arrColumnsName As New ArrayList

            ViewState.Add("arColWidth", arColWidth)
            ViewState.Add("arrTextboxId", arrTextboxId)
            ViewState.Add("arrColumnsName", arrColumnsName)

            'variables used in fast entry screen 
            '***********************************************
            Dim arrHeadersTask As New ArrayList
            Dim arrFooterTask As New ArrayList
            ' Dim dtvAction As New DataView
            Dim arrColumnsNameAction As New ArrayList
            Dim arrWidthAction As New ArrayList
            Dim arrColumnsWidthAction As New ArrayList

            ViewState.Add("arrHeadersTask", arrHeadersTask)
            ViewState.Add("arrFooterTask", arrFooterTask)
            ' ViewState.Add("dtvAction", dtvAction)
            ViewState.Add("arrColumnsNameAction", arrColumnsNameAction)
            ViewState.Add("arrWidthAction", arrWidthAction)
            ViewState.Add("arrColumnsWidthAction", arrColumnsWidthAction)
            '******************************************************
        End If
        'paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlTaskView$txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 15
        End If
        txtPageSize.Text = mintPageSize
        '******************************
        arrImageUrlDisabled.Clear()
        arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
        arrImageUrlDisabled.Add("../../Images/white.gif")

        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Call txtCSS(Me.Page, "cpnlTaskAction")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'javascript function added with controls
        '**********************************************************************************
        If IsPostBack = False Then
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgFWD.Attributes.Add("Onclick", "return SaveEdit('Fwd');")
            imgBtnViewPopup.Attributes.Add("Onclick", "return OpenVW('T040011');")
            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
            ddlstview.Attributes.Add("OnChange", "return SaveEdit('View');")
        End If
        '*********************************************************************************
        'if task not opened
        '*******************************************
        Panel1.Visible = True
        GrdAddSerach.Visible = True
        ' ddlstview.Enabled = True
        '********************************************
        Dim StrUserID As String
        'get logged user id
        mintuserid = HttpContext.Current.Session("PropUserID")
        StrUserID = HttpContext.Current.Session("PropUserID")
        ViewState("gshPageStatus") = 0

        txthiddenImage = Request.Form("txthiddenImage")
        introwvalues = Request.Form("txtrowvalues")

        If Request.Form("txtComp") <> "undefined" Or Request.Form("txtComp") = Nothing Then
            If Request.Form("txtComp") <> "" And Request.Form("txtComp") <> "0" Then
                intComp = Request.Form("txtComp")
                ViewState("CompanyName") = Request.Form("txtComp")
                mstGetFunctionValue = WSSSearch.SearchCompName(Request.Form("txtComp"))
                mstrcomp = intComp
                ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
            Else
                mstrcomp = 0
            End If
        End If
        strhiddenTable = Request.Form("txthiddenTable")
        If strhiddenTable = "cpnlTaskAction_grdAction" Then
            ViewState("ActionNo") = Val(Request.Form("txtTask"))
            mstrTaskNumber = ViewState("ActionNo")
        Else
            ViewState("ActionNo") = 0
            If Not IsNothing(Request.Form("txtTask")) Then
                ViewState("TaskNo") = Val(Request.Form("txtTask"))
            End If
            mstrTaskNumber = ViewState("TaskNo")
            If Not IsNothing(Request.Form("txthiddenCallNo")) Then
                ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
            End If
            mstrCallNumber = ViewState("CallNo")
            Dim strStatus As String = WSSSearch.GetTaskStatus(mstrCallNumber, mstrTaskNumber, ViewState("CompanyID"))
            ViewState("TaskStatus") = strStatus
        End If

        If ViewState("ActionNo") < 1 And ViewState("TaskNo") < 1 Then
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
        Else
            cpnlTaskAction.Enabled = True
            cpnlTaskAction.TitleCSS = "test"
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
        End If
        'these statements check the button click caption 
        '***********************************************
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "View"

                        ViewState("HistoricViewName") = ddlstview.SelectedItem.Text
                        ViewState("HistoricViewValue") = ddlstview.SelectedValue
                        SaveUserView()

                        ViewState("CallNo") = 0
                        ViewState("TaskNo") = 0
                        ViewState("CompanyID") = 0

                    Case "Edit"
                        If strhiddenTable = "cpnlTaskAction_grdAction" Then
                            Exit Select
                        Else
                            'Response.Redirect("../../supportcenter/callview/Call_Detail.aspx?ScrID=3&ID=0&PageID=6&CallNumber=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "", False)
                        End If
                    Case "Add"
                        'Response.Redirect("../../supportcenter/callview/Call_Detail.aspx?ScrID=3&ID=-1&PageID=6", False)
                    Case "Select"
                    Case "CloseCall"
                        If ViewState("WVmshCall") = 0 Then
                            ViewState("WVmshCall") = 1
                        Else
                            ViewState("WVmshCall") = 0
                        End If
                    Case "Delete"
                        If ViewState("ActionNo") = 0 Then
                            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                            SQL.DBTracing = False
                            Dim strchkcallstatus As String = SQL.Search("Work_view", "Load-278", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")
                            If IsNothing(strchkcallstatus) = False Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Call Closed so You cannot change the Task... ")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)

                                Exit Select
                            End If
                            ' Check that task is not in progress
                            mstGetFunctionValue = WSSSearch.SearchTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                            If mstGetFunctionValue.ErrorCode = 1 Then
                                ' Delete TASK
                                mstGetFunctionValue = WSSDelete.DeleteTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                                If mstGetFunctionValue.ErrorCode = 0 Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Task Deleted successfully...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                                    introwvalues = 0
                                    mstrTaskNumber = 0
                                ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Error occur please try later...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

                                End If
                            Else
                                lstError.Items.Clear()
                                lstError.Items.Add("This task is in PROGRESS so it cannot be deleted...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                            End If
                        Else
                            ' Delete ACTION
                            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                            SQL.DBTracing = False
                            Dim strchkcallstatus As String = SQL.Search("Work_view", "Load-323", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")
                            If IsNothing(strchkcallstatus) = False Then
                                lstError.Items.Add("Call Closed so You cannot change the Task and Action...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)

                                Exit Select
                            End If
                            mstGetFunctionValue = WSSDelete.DeleteAction(ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"), ViewState("CompanyID"))
                            If mstGetFunctionValue.ErrorCode = 0 Then
                                '''''''''''Rollback Task Status'''''''''''
                                Dim intRows As Integer
                                If SQL.Search("Work_view", "Load-341", "select * from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Task_Number=" & ViewState("TaskNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                    Dim arrTaskStatusColUpdate As New ArrayList
                                    Dim arrTaskStatusRowUpdate As New ArrayList
                                    arrTaskStatusColUpdate.Add("TM_VC50_Deve_status")
                                    arrTaskStatusRowUpdate.Add("ASSIGNED")
                                    WSSUpdate.UpdateTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"), arrTaskStatusColUpdate, arrTaskStatusRowUpdate)
                                End If
                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                '''''''''''Rollback Call Status if there is no action left behind the call'''''''''''
                                If SQL.Search("Work_view", "Load-352", "select * from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                    Dim arrCallStatusColUpdate As New ArrayList
                                    Dim arrCallStatusRowUpdate As New ArrayList
                                    arrCallStatusColUpdate.Add("CN_VC20_Call_Status")
                                    arrCallStatusRowUpdate.Add("ASSIGNED")
                                    WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arrCallStatusColUpdate, arrCallStatusRowUpdate)
                                End If
                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                lstError.Items.Clear()
                                lstError.Items.Add("Action Deleted successfully...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                introwvalues = 0
                                mstrTaskNumber = 0
                            ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Errors occur Please try Later...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                            End If
                        End If
                    Case "Save"
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Clear()
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                        If TxtActionOwner_F.Text.Trim.Equals("") OrElse dtActionDate.CalendarDate.Trim.Equals("") OrElse TxtUsedHr_F.Text.Trim.Equals("") OrElse TxtDescription_F.Text.Trim.Equals("") OrElse dtActionDate.CalendarDate.Trim.Equals("") Then
                            Exit Select
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select

            Catch ex As Exception
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("Work_view", "Load-386", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            End Try

        End If
        '**********************************************************************************************

        strFilter = ""
        strSearch = ""

        Call CreateDataTableAction(strFilter)
        Call CreateGridAction()
        Call FillHeaderArrayAction()
        Call FillFooterArrayAction()
        Call createTemplateColumnsAction()

        '//////////////////

        If Not IsPostBack Then

            ViewState("CallNo") = 0
            mstrCallNumber = 0

            If ViewState("WVmshCall") = Nothing Then
                ViewState("WVmshCall") = 0
            End If


            'fill dropdown combo with view name from database
            GetView()
            ChkSelectedView() 'chk user selected view last time

            If ViewState("HistoricViewName") <> "" And ViewState("HistoricViewName") <> "Default" Then
                ' fill datagrid based on user define columns and combination
                Fillview()
            Else
                'fill tha datagrid from based on admin defined to the role
                FillDefault()
                ViewState("HistoricViewName") = "Default"
            End If
            CurrentPg.Text = _currentPageNumber.ToString()
            'this will create textboxesover datagrid's columns
            CreateTextBox()
        Else

            '**********************************
            'this loop filling new arraylist in the arrtextvalue array
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
            For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                arrtextvalue.Add(Request.Form("cpnlTaskView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
            Next
            '**************************************

            If ddlstview.SelectedValue = 0 Then
                'fill tha datagrid from based on admin defined to the role
                FillDefault()
            Else
                ' fill datagrid based on user define columns and combination
                Fillview()
            End If

            'this loop filling new arraylist in the arrtextvalue array
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
            For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                arrtextvalue.Add(Request.Form("cpnlTaskView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
            Next

            CreateTextBox()

        End If

        'recreate Action Query and bind the grid
        Call CreateDataTableAction(strFilter)
        'If dtvAction.Table.Rows.Count > 0 And blnActionBinded = True Then
        Call BindGridAction()
        ' End If

        If Not IsPostBack Then
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View &nbsp;&nbsp;"
        ElseIf Not ViewState("TaskNo") = 0 Then
            cpnlTaskAction.Enabled = True
            cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
            cpnlTaskAction.TitleCSS = "test"
        End If

        'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
        '************************************************
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If
        '****************************************************

        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If

        Call GridRowSelection()

        If IsNothing(ViewState("SortOrderAction")) = False Then
            SortGRDDuplicateAction()
        End If

        If Val(ViewState("CallNo")) > 0 Then
            imgEdit.ToolTip = "Edit Task"
            imgDelete.ToolTip = "Delete Action"
            imgFWD.ToolTip = "Forward Task"
        Else
            imgEdit.ToolTip = "Select a Task to Edit"
            imgDelete.ToolTip = "Select a Task to Delete Action"
            imgFWD.ToolTip = "Select a Task to Forward"
        End If

        If Val(ViewState("WVmshCall")) = 1 Then
            imgCloseCall.ToolTip = "View only closed tasks"
        Else
            imgCloseCall.ToolTip = "View only open tasks"
        End If


        '***********Attachment button*********************************
        If Val(ViewState("CallNo")) > 0 Then
            If ChangeAttachmentToolTip(ViewState("CompanyID"), ViewState("CallNo")) = True Then
                imgAttachments.Attributes.Add("Onclick", "return ShowAttachment(1);")
                imgAttachments.ToolTip = "View Attachment"
            Else
                imgAttachments.Attributes.Add("Onclick", "return ShowAttachment(0);")
                imgAttachments.ToolTip = "No Attachment Uploaded"
            End If
        Else
            imgAttachments.Attributes.Add("Onclick", "return ShowAttachment(-1);")
            imgAttachments.ToolTip = "Select a call to View Attachment"
        End If
        '**************************************************************

        'set alternate color setting on grid
        '*************************************
        GrdAddSerach.AlternatingItemStyle.BackColor = Color.FromArgb(245, 245, 245)
        GrdAddSerach.ItemStyle.BackColor = Color.FromArgb(255, 255, 255)
        '**************************************

        'Security Block
        '*************************************************
        Dim intid As String
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = 536
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If
        '***********************************************
        'End of Security Block
        'Disable the action grid if user clicks on closed task button
        If txthiddenImage = "CloseCall" Then
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View"
        Else
        End If

    End Sub

#End Region
    '*******************************************************************
    ' Function             :-  FillDefault
    ' Purpose             :- Fill and design datagrid based on defaultcolumns settings from default  tables
    '								
    ' Date					  Author						Modification Date					Description
    '		                      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub FillDefault()

        'chk grid width in database
        Try

            '*************************
            GrdAddSerach.PageSize = mintPageSize ' set the grid page size
            Dim dsDefault As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            '**************
            ' Dim arSetColumnName As New ArrayList
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select "
            ' Dim strwhereQuery As String = " and "
            Dim shJoin As Short
            Dim strQuery As String

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
              & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
              & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =536 And " _
              & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
              & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
              & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
              & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
              & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=536 and obm_vc4_object_type_fk='VIW') " _
              & " order by OBM.OBM_SI2_Order_By"

            sqrdView = SQL.Search("Work_view", "FillDefault-661", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                Dim htDateCols As New Hashtable

                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                CType(ViewState("arColWidth"), ArrayList).Clear()

                While sqrdView.Read
                    If sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID" Then
                        strSelect &= "SOwner." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID_Assign" Then
                        strSelect &= "ABy." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Est_close_date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Call_No_FK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & "),"
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Task_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Task_Close_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
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
                    End If

                End While
                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)

                If shJoin = 1 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK"
                ElseIf shJoin = 2 Then
                    strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK"
                ElseIf shJoin = 3 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                Else
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                End If

                If ViewState("WVmshCall") = 1 Then 'to see all calls
                Else
                    strSelect &= " and TM_VC50_Deve_Status='Closed' "
                End If
                strSelect &= " and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK "
                'Added company chk from company access table
                strSelect &= " and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
                strSelect &= " order by TM_NU9_Call_No_FK desc,TM_NU9_Task_no_PK asc"

                mintCallNoPlace = -1
                mintCompId = ""
                mintSuppOwnID = ""
                mintSuppOwn = ""
                mintAssignBy = ""
                mintAssignByID = ""
                mintTaskNoRowID = ""

                If SQL.Search("T040021", "Work_view", "Filldefault-778", strSelect, dsDefault, "sachin", "Prashar") = True Then
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
                    Next

                    mdvtable.Table = dsDefault.Tables("T040021")

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("SubTaskDesc", 37)
                    htGrdColumns.Add("CallDesc", 44)
                    htGrdColumns.Add("CallSubject", 29)
                    htGrdColumns.Add("TaskDesc", 45)



                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)

                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), 0)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    rowvalue = 0
                    rowvalueCall = 0

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    '*************************************************************************
                    GrdAddSerach.AllowPaging = True
                    GrdAddSerach.PageSize = mintPageSize

                    If ViewState("HistoricViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                        ViewState("SortOrder") = Nothing
                    End If

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If

                    GrdAddSerach.DataBind()
                    '***************************************************

                    ''paging count
                    Dim intRows As Integer = mdvtable.Table.Rows.Count
                    'CurrentPage.Text = _currentPageNumber.ToString()
                    Dim _totalPages As Double = 1
                    Dim _totalrecords As Int32
                    If Not Page.IsPostBack Then
                        _totalrecords = intRows
                        _totalPages = _totalrecords / mintPageSize
                        TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                        TotalRecods.Text = _totalrecords
                    Else
                        _totalrecords = intRows
                        If CurrentPg.Text = 0 And _totalrecords > 0 Then
                            CurrentPg.Text = 1
                        End If
                        If _totalrecords = 0 Then
                            CurrentPg.Text = 0
                        End If
                        _totalPages = _totalrecords / mintPageSize
                        TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                        _totalPages = Double.Parse(TotalPages.Text)
                        TotalRecods.Text = _totalrecords
                    End If

                    '''
                    cpnlTaskAction.Enabled = True
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskAction.TitleCSS = "test2"
                    cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskView.Enabled = True
                    '***************************************************
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No Task in Closed Status...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    cpnlTaskAction.Enabled = False
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.TitleCSS = "test2"

                    cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskView.Enabled = False
                    cpnlTaskView.TitleCSS = "test2"

                End If
            Else

                lstError.Items.Clear()
                lstError.Items.Add(" Data not available in security tables or may be server not available...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                cpnlTaskAction.Enabled = False
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskAction.TitleCSS = "test2"
                cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskView.Enabled = False
                cpnlTaskView.TitleCSS = "test2"
            End If
            '**************
        Catch ex As Exception
            CreateLog("Work_view", "Load-792", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    '*******************************************************************
    ' Function             :-  fillview
    ' Purpose              :- Fill and design datagrid based on user defined columns settings from user tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 			                  Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub Fillview()

        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        GrdAddSerach.PageSize = mintPageSize ' set the grid page size

        Try

            Dim shJoin As Short
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            Dim strwhereQuery As String = " and "

            sqrdView = SQL.Search("Work_view", "FillView-846", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='536' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then

                Dim dsFromView As New DataSet
                Dim htDateCols As New Hashtable

                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                CType(ViewState("arColWidth"), ArrayList).Clear()


                While sqrdView.Read

                    If sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID" Then
                        strSelect &= "SOwner." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "UM_VC50_UserID_Assign" Then
                        strSelect &= "ABy." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
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
                    End If
                End While

                sqrdView.Close()

                sqrdView = SQL.Search("Work_view", "FillView-897", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='536'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

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
                'add where clause in query 
                '***********************************************************
                sqrdView = SQL.Search("Work_view", "FillView-921", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='536' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

                If blnView = True Then
                    While sqrdView.Read
                        Select Case CType(sqrdView.Item("UV_VC50_COL_Value"), String).Trim.ToUpper
                            ' Case "TM_VC8_SUPP_OWNER"
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
                                    strwhereQuery += " ABy.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
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
                    '**********************************************************
                    If Session("PropCompanyType") = "SCM" Then
                    Else
                        strwhereQuery += " and  TM_NU9_Comp_ID_FK=" & Session("PropCompanyID")
                    End If
                End If

                If Session("PropCompanyType") <> "SCM" And strwhereQuery = " and " Then
                    strwhereQuery += "  TM_NU9_Comp_ID_FK=" & Session("PropCompanyID")
                End If

                If shJoin = 1 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK"
                ElseIf shJoin = 2 Then
                    strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK"
                ElseIf shJoin = 3 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                Else
                    strSelect &= " from T040011,T040021  task,T010011 comp where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number"
                End If

                If ViewState("WVmshCall") = 1 Then
                Else
                    strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
                End If
                If strwhereQuery.Equals(" and ") = True Then
                    'nothing added in query
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

                mintCallNoPlace = -1
                mintCompId = ""
                mintSuppOwnID = ""
                mintSuppOwn = ""
                mintAssignBy = ""
                mintAssignByID = ""
                mintTaskNoRowID = ""

                If SQL.Search("T040021", "Work_view", "FillView-1002", strSelect, dsFromView, "sachin", "Prashar") = True Then
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
                    Next

                    mdvtable.Table = dsFromView.Tables(0)

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("SubTaskDesc", 37)
                    htGrdColumns.Add("CallDesc", 44)
                    htGrdColumns.Add("CallSubject", 29)

                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)

                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), 0)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If
                    '*************************************************************************
                    rowvalue = 0
                    rowvalueCall = 0

                    If ViewState("HistoricViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                        ViewState("SortOrder") = Nothing
                    End If

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If

                    GrdAddSerach.DataBind()

                    ''paging count
                    Dim intRows As Integer = mdvtable.Table.Rows.Count
                    'CurrentPage.Text = _currentPageNumber.ToString()
                    Dim _totalPages As Double = 1
                    Dim _totalrecords As Int32
                    If Not Page.IsPostBack Then
                        _totalrecords = intRows
                        _totalPages = _totalrecords / mintPageSize
                        TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                        TotalRecods.Text = _totalrecords
                    Else
                        _totalrecords = intRows
                        If CurrentPg.Text = 0 And _totalrecords > 0 Then
                            CurrentPg.Text = 1
                        End If
                        If _totalrecords = 0 Then
                            CurrentPg.Text = 0
                        End If
                        _totalPages = _totalrecords / mintPageSize
                        TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                        _totalPages = Double.Parse(TotalPages.Text)
                        TotalRecods.Text = _totalrecords
                    End If

                    '''
                    '***************************************************
                    cpnlTaskAction.Enabled = True
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskAction.TitleCSS = "test2"
                    cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskView.Enabled = True
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No Task Assigned or data not exist according to view query... !")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'cpnlError.Visible = True
                    cpnlTaskAction.Enabled = False
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.TitleCSS = "test2"

                    cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskView.Enabled = False
                    cpnlTaskView.TitleCSS = "test2"

                End If

            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("Work_view", "FillView-1021", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#Region "fill data into the dropdown from view table "

    '*******************************************************************
    ' Function             :-  GetView
    ' Purpose              :- fill value into the dropdown name and id of the field view table
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/3/06			      Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************

    Private Sub GetView()

        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        ddlstview.Items.Clear()
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            sqrdView = SQL.Search("Work_view", "GetView-1200", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='536' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then
                ddlstview.DataSource = sqrdView
                ddlstview.DataTextField = "UV_VC50_View_Name"
                ddlstview.DataValueField = "UV_IN4_View_ID"
                ddlstview.DataBind()
                sqrdView.Close()
            End If
            If ViewState("HistoricViewName") = "" Or ViewState("HistoricViewName") = "Default" Then
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
                ddlstview.SelectedIndex = ddlstview.Items.Count - 1
            Else
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
            End If
            If ViewState("HistoricViewName") <> "" And ViewState("HistoricViewName") <> "Default" Then
                ddlstview.SelectedValue = ViewState("HistoricViewValue")
            End If
        Catch ex As Exception
            CreateLog("Work_view", "GetView-1185", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"
    '*******************************************************************
    ' Function             :-  CreateTextBox
    ' Purpose              :- create textboxes at run time on datagrid based on datagrid columns
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/3/06			      Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub CreateTextBox()

        Dim _textbox As TextBox
        Dim intii As Integer
        CType(ViewState("arrTextboxId"), ArrayList).Clear()
        'fill the columns count into the array from mdvtable view
        Try
            intColumnCount = mdvtable.Table.Columns.Count
        Catch ex As Exception
        End Try

        Try
            For intii = 0 To intColumnCount - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intii))
                    'If intii > 12 And intii < 16 Then
                    '    col1cng = col1.Value
                    '    col1cng = col1cng & "pt"
                    'Else
                    '    col1cng = col1.Value + 1
                    '    col1cng = col1cng & "pt"
                    'End If

                    If intii > 12 And intii < 16 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2
                        col1cng = col1cng & "pt"
                    End If

                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "F" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & CType(ViewState("arrColumnsName"), ArrayList).Item(intii) & " runat=""server""  Width=""0"" Visible=""False"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & CType(ViewState("arrColumnsName"), ArrayList).Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""20""></asp:TextBox>"))
                    End If
                    _textbox.ID = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    _textbox.Text = ""
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intii))
                    'If intii > 12 And intii < 16 Then
                    '    col1cng = col1.Value
                    '    col1cng = col1cng & "pt"
                    'Else
                    '    col1cng = col1.Value + 1
                    '    col1cng = col1cng & "pt"
                    'End If
                    If intii > 12 And intii < 16 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2
                        col1cng = col1cng & "pt"
                    End If
                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If
                    strcolid = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "F" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=""0"" Visible=""False""  CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""20""></asp:TextBox>"))
                    End If
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                End If
                CType(ViewState("arrTextboxId"), ArrayList).Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("Work_view", "CreateTextBox-1271", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "Serach Grid Button Click"
    '*******************************************************************
    ' Function             :-  FillGRDAfterSearch
    ' Purpose              :- grid search based on textbox data function filter the data from dataview
    ' Date					  Author						Modification Date					Description
    ' 18/01/06			  Sachin Prashar		    -------------------					Created
    '*******************************************************************
    Private Sub FillGRDAfterSearch()

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim arCount As Integer = arrtextvalue.Count - 1
        Dim intI As Integer

        Try

            For intI = 0 To arCount
                If Not IsNothing(arrtextvalue(intI)) Then
                    If Not arrtextvalue(intI).Equals("") Then
                        strSearch = arrtextvalue(intI)
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") Then
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                                If IsDate(strSearch) = False Then
                                    strSearch = "12/12/1825"
                                End If
                            End If
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") = True Then
                                strSearch = strSearch.Replace("*", "")
                                If IsNumeric(strSearch) = False Then
                                    strSearch = "-101"
                                End If
                            End If
                            strSearch = strSearch.Replace("*", "")
                            strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            strSearch = arrtextvalue(intI)
                            strSearch = GetSearchString(strSearch)
                            If strSearch.Contains("*") = True Then
                                strSearch = strSearch.Replace("*", "%")
                            Else
                                strSearch &= "%"
                            End If
                            strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                        End If
                    End If
                End If
            Next

            If CHKA.Checked = True Then
                strRowFilterString &= " A <>0  AND"
            End If
            If CHKC.Checked = True Then
                strRowFilterString &= " C <>0 AND"
            End If
            If CHKF.Checked = True Then
                strRowFilterString &= " F <>0 AND"
            End If

            If (strRowFilterString Is Nothing) Then
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)

            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If

            '*************************************************************************
            HTMLEncodeDecode(mdlMain.Action.Decode, mdvtable)
            SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), 0)
            GetFilteredDataView(mdvtable, strRowFilterString)
            HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)

            GrdAddSerach.DataSource = mdvtable
            rowvalue = 0
            rowvalueCall = 0

            If ViewState("HistoricViewName") <> ddlstview.SelectedItem.Text Then
                GrdAddSerach.CurrentPageIndex = 0
                CurrentPg.Text = 1
            End If
            If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                GrdAddSerach.CurrentPageIndex = 0
                CurrentPg.Text = 1
            End If

            GrdAddSerach.DataBind()

            ''paging count
            Dim intRows As Integer = mdvtable.Table.Rows.Count
            Dim _totalPages As Double = 1
            Dim _totalrecords As Int32

            If Not Page.IsPostBack Then
                _totalrecords = intRows
                _totalPages = _totalrecords / mintPageSize
                TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                TotalRecods.Text = _totalrecords
            Else
                _totalrecords = intRows
                If CurrentPg.Text = 0 And _totalrecords > 0 Then
                    CurrentPg.Text = 1
                End If
                If _totalrecords = 0 Then
                    CurrentPg.Text = 0
                End If
                _totalPages = _totalrecords / mintPageSize
                TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
                _totalPages = Double.Parse(TotalPages.Text)
                TotalRecods.Text = _totalrecords
            End If
            '''
            If mdvtable.Count = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Data not found according to your search string...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

            End If

        Catch ex As Exception
            CreateLog("Work_view", "Click-1585", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "btngrdsearch")
        End Try

    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"
    '*******************************************************************
    ' Function             :-  GrdAddSerach_ItemDataBound1
    ' Purpose              :-Display attachment, comment based on database and and bound java script on columns like selection and double click
    '								
    ' Date					  Author						Modification Date					Description
    '                			  Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound


        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String 'stored task no
        Dim strName As String
        Dim strCompId As String 'stored company's ID
        Dim rowflag As Boolean = True
        Dim attSts As Boolean
        Dim comstat As String
        Dim intcolno As Int16 = 0
        Dim intcolnoComm As Integer = 0
        Dim intCount As Integer = 2
        Dim intCountfrm As Integer = 3
        Dim frmSts As String
        'these variables stored columns position in datagrid
        '*************************************
        Dim strSuppOwnID As String
        Dim suppownrowid As String
        Dim strSuppOwn As String
        Dim strSuppOwnrowID As String
        Dim strAssignBy As String
        Dim strAssignByRowID As String
        Dim strAssignByID As String
        Dim AssignByRowID As String
        Dim strTaskNoRowID As String
        '***************************************
        Dim Monistat As Boolean

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strName = e.Item.Cells(mintCallNoPlace + 3).Text()
                    strCompId = e.Item.Cells(mintCompId + 3).Text()


                    If mintSuppOwnID <> "" Then
                        strSuppOwnID = e.Item.Cells(mintSuppOwnID + 3).Text()
                        suppownrowid = mintSuppOwnID + 3
                    End If

                    If mintSuppOwn <> "" Then
                        strSuppOwn = e.Item.Cells(mintSuppOwn + 3).Text()
                        strSuppOwnrowID = mintSuppOwn + 3
                    End If

                    If mintAssignBy <> "" Then
                        strAssignBy = e.Item.Cells(mintAssignBy + 3).Text()
                        strAssignByRowID = mintAssignBy + 3
                    End If

                    If mintAssignByID <> "" Then
                        strAssignByID = e.Item.Cells(mintAssignByID + 3).Text()
                        AssignByRowID = mintAssignByID + 3
                    End If


                    'for attacment images********************
                    If rowflag Then
                        attSts = IIf(e.Item.Cells(mdvtable.Table.Columns.Count + 1).Text = "&nbsp;", False, True)
                    End If
                    If Not IsNothing(e.Item.Cells(0).FindControl("imgAtt")) Then
                        If attSts Then
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Attach15_9.gif"
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ToolTip = "Click To View Attachment"
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCount & "','" & strCompId & "','" & strName & "')")
                        Else
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        End If
                    End If


                    'for comment images********************
                    If rowflag Then
                        comstat = e.Item.Cells(mdvtable.Table.Columns.Count).Text
                        If Not IsNothing(e.Item.Cells(0).FindControl("imgComm")) Then
                            Select Case comstat
                                Case "1"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment2.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "Old Comments"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & strName & "')")

                                Case "2"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Unread.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "New Comments"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & strName & "')")

                                Case "0"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "No Comment"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & strName & "')")
                                Case Else
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "No Comment"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlTaskView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & strName & "')")
                            End Select
                        End If
                    End If

                    '************************************************************

                    'for form image***********************************

                    If rowflag Then
                        frmSts = e.Item.Cells(mdvtable.Table.Columns.Count + 2).Text
                    End If
                    If Not IsNothing(e.Item.Cells(0).FindControl("imgform")) Then

                        If frmSts = "1" Then
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Form1.jpg"
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ToolTip = "Filled Form"
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCountfrm & "','" & strCompId & "','" & strName & "')")
                        ElseIf frmSts = "2" Then
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Form2.gif"
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ToolTip = "Empty Form"
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCountfrm & "','" & strCompId & "','" & strName & "')")

                        Else
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        End If

                    End If

                    '*********************************************************************************************

                    If intcolno >= 3 Then
                        If intcolno = strSuppOwnrowID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"

                            e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strSuppOwnID & ",'" & strSuppOwn & "')")

                        ElseIf intcolno = strAssignByRowID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"

                            e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strAssignByID & ",'" & strAssignBy & "')")
                        Else
                            e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & "," & strName & ", '" & rowvalue & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                        End If
                    End If
                End If

                rowflag = False
                intcolno = intcolno + 1
            Next

            If Val(ViewState("CallNo")) <> 0 And Val(ViewState("CompanyID")) <> 0 And Val(ViewState("TaskNo")) <> 0 Then
                If strName = Val(ViewState("CallNo")) And ViewState("CompanyName") = strCompId And strID = Val(ViewState("TaskNo")) Then
                    e.Item.BackColor = Color.FromArgb(212, 212, 212)
                End If
            End If

            rowvalue += 1
            rowvalueCall += 1

        Catch ex As Exception
            CreateLog("Work_View", "ItemDataBound-1830", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdaddserach")
        End Try
    End Sub

#End Region

    Private Function CreateFolder(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, Session("PropCompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, Session("PropCompanyID"), AttachLevel.CallLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Work_view", "CreateFolder-1718", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, Session("PropCompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, Session("PropCompanyID"), AttachLevel.CallLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, Session("PropCompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, Session("PropCompanyID"), AttachLevel.CallLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("Work_view", "CreateFolder-1744", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

#Region "Create Action Grid"

    Private Sub CreateGridAction()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        grdAction.ID = "grdAction"
        grdAction.DataKeyField = "AM_NU9_Action_Number"
        Call FormatGridAction()

        PlaceHolder1.Controls.Add(grdAction)
    End Sub

    Private Sub FormatGridAction()
        grdAction.AutoGenerateColumns = False
        grdAction.AllowPaging = True
        '  grdAction.ShowFooter = True
        grdAction.ShowHeader = True
        grdAction.HeaderStyle.CssClass = "GridHeader"
        grdAction.HeaderStyle.Height = System.Web.UI.WebControls.Unit.Pixel(1)
        grdAction.Width = System.Web.UI.WebControls.Unit.Percentage(100)
        grdAction.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
        grdAction.BorderStyle = BorderStyle.None
        grdAction.CellPadding = 1
        grdAction.AllowPaging = False
        grdAction.CssClass = "Grid"
        grdAction.HorizontalAlign = HorizontalAlign.Center
        'grdAction.FooterStyle.CssClass = "GridFixedFooter"
        grdAction.SelectedItemStyle.CssClass = "GridSelectedItem"
        grdAction.AlternatingItemStyle.CssClass = "GridAlternateItem"
        grdAction.ItemStyle.CssClass = "GridItem"
    End Sub
#End Region

#Region "Create Template Column Action Grid"
    Private Sub createTemplateColumnsAction()



        Dim intCount As Integer

        ReDim tclAction(dtvAction.Table.Columns.Count)

        arrImageUrlEnabled.Clear()
        arrImageUrlEnabled.Add("../../Images/comment2.gif")
        arrImageUrlEnabled.Add("../../Images/attach15_9.gif")

        arrImageUrlDisabled.Clear()
        arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
        arrImageUrlDisabled.Add("../../Images/white.gif")

        arrImageUrlNew.Clear()
        arrImageUrlNew.Add("../../Images/comment_Unread.gif")

        CType(ViewState("arrColumnsNameAction"), ArrayList).Clear()
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Com")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Att")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Act#")
        'arrColumnsNameAction.Add("Action")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Description")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Act.")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Hrs.")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("ActionOwner")
        'arrColumnsNameAction.Add("Priority")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("ActionDate")
        CType(ViewState("arrColumnsNameAction"), ArrayList).Add("ActionType")

        CType(ViewState("arrWidthAction"), ArrayList).Clear()
        CType(ViewState("arrWidthAction"), ArrayList).Add(10)
        CType(ViewState("arrWidthAction"), ArrayList).Add(10)
        CType(ViewState("arrWidthAction"), ArrayList).Add(30)
        'arrWidthAction.Add(50)
        CType(ViewState("arrWidthAction"), ArrayList).Add(47)
        CType(ViewState("arrWidthAction"), ArrayList).Add(10)
        CType(ViewState("arrWidthAction"), ArrayList).Add(15)
        CType(ViewState("arrWidthAction"), ArrayList).Add(60)
        'arrWidthAction.Add(45)
        CType(ViewState("arrWidthAction"), ArrayList).Add(64)
        CType(ViewState("arrWidthAction"), ArrayList).Add(64)


        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(0)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(1)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(2)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Percentage(CType(ViewState("arrWidthAction"), ArrayList)(3)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(4)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(5)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(6)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(7)))
        CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Point(CType(ViewState("arrWidthAction"), ArrayList)(8)))


        tclAction(0) = New TemplateColumn
        tclAction(0).Visible = True
        tclAction(0).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvAction.Table.Columns(0).ToString, "", dtvAction.Table.Columns(0).ToString + "_H", False, CType(ViewState("arrColumnsNameAction"), ArrayList)(0), False)
        tclAction(0).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvAction.Table.Columns(0).ToString, arrImageUrlDisabled(0))
        tclAction(0).ItemStyle.HorizontalAlign = HorizontalAlign.Center
        tclAction(0).ItemStyle.Width = CType(ViewState("arrColumnsWidthAction"), ArrayList)(0)
        grdAction.Columns.Add(tclAction(0))

        tclAction(1) = New TemplateColumn
        tclAction(1).Visible = True
        tclAction(1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvAction.Table.Columns(1).ToString, "", dtvAction.Table.Columns(1).ToString + "_H", False, CType(ViewState("arrColumnsNameAction"), ArrayList)(1), False)
        tclAction(1).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvAction.Table.Columns(1).ToString, arrImageUrlDisabled(1))
        tclAction(1).ItemStyle.HorizontalAlign = HorizontalAlign.Center
        tclAction(1).ItemStyle.Width = CType(ViewState("arrColumnsWidthAction"), ArrayList)(1)
        grdAction.Columns.Add(tclAction(1))

        For intCount = 2 To dtvAction.Table.Columns.Count - 1
            tclAction(intCount + 1) = New TemplateColumn
            tclAction(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvAction.Table.Columns(intCount).ToString, dtvAction.Table.Columns(intCount).ToString)
            Dim AddEventOnGrigHeader As New IONGrid.CreateItemTemplateTextBoxForHeader(dtvAction.Table.Columns(intCount).ToString, "", dtvAction.Table.Columns(intCount).ToString + "_H", False, CType(ViewState("arrColumnsNameAction"), ArrayList)(intCount), True)
            AddHandler AddEventOnGrigHeader.OnSort, AddressOf SortGridAction
            tclAction(intCount + 1).HeaderTemplate = AddEventOnGrigHeader
            tclAction(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvAction.Table.Columns(intCount).ToString + "_F", False)
            tclAction(intCount + 1).ItemStyle.Width = CType(ViewState("arrColumnsWidthAction"), ArrayList)(intCount)      'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthAction(intCount))
            grdAction.Columns.Add(tclAction(intCount + 1))
        Next

        Call ChangeTextBoxWidth()

    End Sub

#End Region

#Region "Create Action Query"
    Private Sub CreateDataTableAction(ByVal strWhereClause As String)
        Dim dsTask As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32
        If IsNothing(strWhereClause) Then
            strWhereClause = ""
        End If
        Try

            strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,AM_CH1_Mandatory,AM_FL8_Used_Hr,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID," & _
             "  convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date, AM_VC8_ActionType From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & "   and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And AM_NU9_Task_Number=" & Val(ViewState("TaskNo"))
            strSql = strSql & " Order By AM_NU9_Action_Number desc"
            Call SQL.Search("T040031", "", "", strSql, dsTask, "sachin", "Prashar")

            dtvAction = New DataView '***************************
            dtvAction.Table = dsTask.Tables(0)

            Dim htDateCols As New Hashtable
            htDateCols.Add("AM_DT8_Action_Date", 2)
            SetDataTableDateFormat(dtvAction.Table, htDateCols)

            Dim strFilter As String = ""
            Dim strSearch As String = ""

            For intCount = 2 To dtvAction.Table.Columns.Count - 1
                strSearch = Request.Form("cpnlTaskAction$grdAction$ctl01$" + dtvAction.Table.Columns(intCount).ColumnName + "_H")
                If Not IsNothing(strSearch) Then
                    If Not strSearch.Trim.Equals("") Then
                        ' -- Format Search String
                        strSearch = mdlMain.GetSearchString(strSearch)
                        If dtvAction.Table.Columns(intCount).DataType.FullName = "System.Decimal" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Int32" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Int16" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Double" Then
                            strSearch = strSearch.Replace("*", "")
                            strFilter = strFilter & dtvAction.Table.Columns(intCount).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            If strSearch.Contains("*") = True Then
                                strSearch = strSearch.Replace("*", "%")
                            Else
                                strSearch &= "%"
                            End If
                            strFilter = strFilter & dtvAction.Table.Columns(intCount).ColumnName & " like " & "'" & strSearch & "' AND "
                        End If
                    End If
                End If
            Next
            If Not strFilter.Trim.Equals("") Then
                strFilter = strFilter.Remove((strFilter.Length - 4), 4)
            End If
            strWhereClause = strFilter
            If Not strWhereClause.Trim.Equals("") Then
                GetFilteredDataView(dtvAction, strWhereClause)
            End If

            If dtvAction.Table.Rows.Count > 0 Then
                mstGetFunctionValue = WSSSearch.SearchTaskOwner(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))

                If mstGetFunctionValue.ErrorCode = 0 Then
                    ' TxtActionOwner_F.Text = mstGetFunctionValue.ExtraValue
                End If

                cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                cpnlTaskAction.TitleCSS = "test"
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & " Task# " & ViewState("TaskNo") & " Company:  " & intComp & ")"
            Else
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskAction.TitleCSS = "test2"
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & " Task# " & ViewState("TaskNo") & " Company:  " & intComp & ")"
            End If

            If IsNothing(strWhereClause) Then
                strWhereClause = ""
            End If

            If strWhereClause.Trim <> "" Then
                cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                cpnlTaskAction.TitleCSS = "test"
            End If
            '===============================
            '===============================
            If dtvAction.Table.Rows.Count = 0 Then
                rowTemp = dtvAction.Table.NewRow
                For intCount = 0 To dtvAction.Table.Columns.Count - 1
                    Select Case dtvAction.Table.Columns(intCount).DataType.FullName
                        Case "System.String"
                            rowTemp.Item(intCount) = " "
                        Case "System.Decimal", "System.Double", "System.Int32", "System.Int16"
                            rowTemp.Item(intCount) = 0
                        Case "System.DateTime"
                    End Select
                Next
                dtvAction.Table.Rows.Add(rowTemp)
            End If
            '===============================
            If IsPostBack Then
                If intComp = "" Then
                    mstGetFunctionValue = WSSSearch.SearchCompNameID(ViewState("CompanyID"))
                    intComp = mstGetFunctionValue.ExtraValue
                End If
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & " Task# " & ViewState("TaskNo") & " Company:  " & intComp & ")"
            Else
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;"
            End If

        Catch ex As Exception
            CreateLog("Work_view", "CreateDataTableAction", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
#End Region

#Region "Fill Action Header Array"
    Private Sub FillHeaderArrayAction()
        Dim t As New Control
        Dim intCount As Integer
        CType(ViewState("arrHeadersTask"), ArrayList).Clear()
        If Page.IsPostBack Then
            For intCount = 0 To dtvAction.Table.Columns.Count - 1
                CType(ViewState("arrHeadersTask"), ArrayList).Add(Request.Form("cpnlTaskAction$grdAction$ctl01$" + dtvAction.Table.Columns(intCount).ColumnName + "_H"))
            Next
        End If
    End Sub
#End Region

#Region "Fill Action Footer Array"
    Private Sub FillFooterArrayAction()
        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        CType(ViewState("arrFooterTask"), ArrayList).Clear()
        If Page.IsPostBack Then
            For intCount = 0 To dtvAction.Table.Columns.Count - 1
                intFooterIndex = dtvAction.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                CType(ViewState("arrFooterTask"), ArrayList).Add(Request.Form("cpnlTaskAction$grdAction$_ctl" & intFooterIndex.ToString.Trim & ":" + dtvAction.Table.Columns(intCount).ColumnName + "_F"))
            Next
        End If
    End Sub
#End Region

#Region "Bind Action Grid"
    Private Sub BindGridAction()

        If Request.Form("txtrowvaluescall") <> 0 Then
            introwvalues = Request.Form("txtrowvaluescall")
        End If

        Dim htGrdColumns As New Hashtable
        htGrdColumns.Add("AM_VC_2000_Description", 42)

        HTMLEncodeDecode(mdlMain.Action.Encode, dtvAction, htGrdColumns)
        SetCommentFlag(dtvAction, mdlMain.CommentLevel.ActionLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
        grdAction.DataSource = dtvAction
        grdAction.DataBind()
    End Sub
#End Region

    Private Sub grdAction_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAction.ItemDataBound

        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvAction
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvAction.ToTable()
        Dim strSelected As String
        Dim structTempActionOwner As Owners '-- will keep Action owner's ID and Name
        dg.PageSize = 1

        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If grdAction.DataKeys(0) <> 0 Then
                    For intCount = 0 To 1       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        strID = grdAction.DataKeys(e.Item.ItemIndex)
                        If strSelected = "1" Then      'If comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & mTaskRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & intCount & "')")
                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & mTaskRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & intCount & "')")
                        Else       ' If no comment/attachment is attached
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & mTaskRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & intCount & "')")
                        End If
                    Next
                    For intCount = 2 To dtvAction.Table.Columns.Count - 1       'for Others
                        If dtvAction.Table.Columns(intCount).DataType.FullName.Equals("System.DateTime") Then
                            If dtBound.Rows(cnt)(intCount).ToString Is null Or dtBound.Rows(cnt)(intCount).ToString = "" Then
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = " "
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = CType(dtBound.Rows(cnt)(intCount).ToString, DateTime).ToShortDateString
                            End If

                        Else
                            If intCount = 6 Then ' -- for action owner cell
                                structTempActionOwner.Id = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(1)
                                structTempActionOwner.Name = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = structTempActionOwner.Name
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString)
                            End If
                        End If
                        'End If

                        strID = grdAction.DataKeys(e.Item.ItemIndex)

                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "',0, '" & mTaskRowValue + 1 & "','" & introwvalues & "', 'cpnlTaskAction_grdAction')")
                        If intCount = 6 Then
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & structTempActionOwner.Id & "')")
                        Else
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "',0, '" & mTaskRowValue + 1 & "', 'cpnlTaskAction_grdAction')")
                        End If
                    Next
                    mTaskRowValue += 1
                Else
                    For intCount = 0 To 1       'For Image Fields
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(1)
                    Next
                    For intCount = 2 To dtvAction.Table.Columns.Count - 1
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("Work_view", "ItemDataBound-2031", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdaction")
        End Try

    End Sub

#Region "Set Entry TextBox Width"
    Private Sub ChangeTextBoxWidth()
        'TxtActionNo_F.Width = System.Web.UI.WebControls.Unit.Point(25)    'System.Web.UI.WebControls.Unit.Point(arrWidthAction(2))
        'TxtDescription_F.Width = System.Web.UI.WebControls.Unit.Point(280)
        'chkMandatoryHr.Width = System.Web.UI.WebControls.Unit.Point(22)
        'TxtUsedHr_F.Width = System.Web.UI.WebControls.Unit.Point(49)    'System.Web.UI.WebControls.Unit.Point(arrWidthAction(6))
        'TxtActionOwner_FName.Width = System.Web.UI.WebControls.Unit.Point(74)    'System.Web.UI.WebControls.Unit.Point(arrWidthAction(7) - 8)
        'dtActionDate.Width = System.Web.UI.WebControls.Unit.Percentage(75)    'System.Web.UI.WebControls.Unit.Point(arrWidthAction(9))
    End Sub

#End Region

#Region "Save Action Fast Entry"
    Private Function SaveAction() As Boolean
        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        'Dim intNo As Int64
        Dim shFlag As Short
        Dim intCallNo As Integer
        Dim blnCheckValidation As Boolean

        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            shFlag = 0
            lstError.Items.Clear()
            lstError.Items.Add("Your Role does not have rights to save Actions...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Function
        End If
        'End of Security Block
        If TxtActionOwner_F.Text.Trim.Equals("") OrElse dtActionDate.CalendarDate.Trim.Equals("") OrElse TxtDescription_F.Text.Trim.Equals("") Then
            blnCheckValidation = False
            Exit Function
        Else
            blnCheckValidation = True
        End If
        lstError.Items.Clear()
        If chkMandatoryHr.Checked = True Then
            If IsNumeric(TxtUsedHr_F.Text.Trim) = True Then
                If TxtUsedHr_F.Text.Trim.Equals("0") = True Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Used hours are mandatory and cannot be 0...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    SaveAction = False
                    Exit Function
                Else
                    SaveAction = True
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Used hours are mandatory...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                SaveAction = False
                Exit Function
            End If
        End If

        If blnCheckValidation = False Then    'Exit If all textbox are blank
            SaveAction = False
            Exit Function
        End If
        'check status of call and task 
        '****************************************************************************
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T040011"
        SQL.DBTracing = False
        Dim strchkcallstatus As String = SQL.Search("Work_view", "SaveAction-2155", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")
        If IsNothing(strchkcallstatus) = False Then
            lstError.Items.Clear()
            lstError.Items.Add("Call Closed so You cannot change the Action...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return False
            Exit Function
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        Dim strchktaskstatus As String = SQL.Search("Work_view", "SaveAction-2171", "select TM_VC50_Deve_status from T040021 where  TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNo") & " and TM_VC50_Deve_status='CLOSED'")
        If IsNothing(strchktaskstatus) = False Then
            lstError.Items.Clear()
            lstError.Items.Add("Task Closed so You cannot change the Action...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return False
            Exit Function
        End If
        '*****************************************************************************************
        lstError.Items.Clear()
        If TxtActionOwner_F.Text.Trim.Equals("") Then
            lstError.Items.Add("Action Owner cannot be blank...")
            shFlag = 1
        End If

        If TxtUsedHr_F.Text.Trim.Equals("") Then
            lstError.Items.Add("Used hours cannot be blank...")
            shFlag = 1
        Else
            If IsNumeric(TxtUsedHr_F.Text.Trim) = False Then
                lstError.Items.Add("Used hour is not numeric...")
                shFlag = 1
            End If
        End If

        If TxtDescription_F.Text.Trim.Equals("") Then
            lstError.Items.Add("Description cannot be blank...")
            shFlag = 1
        End If

        Dim dtTaskDate As Date = SQL.Search("Work_view", "SaveAction-2207", "Select tm_DT8_Task_Date from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNo") & "")

        If dtActionDate.CalendarDate.Trim <> "" Then
            If IsDate(dtActionDate.CalendarDate) = False Then
                lstError.Items.Add("Check Action date format...")
                shFlag = 1
                'ElseIf CDate(dtActionDate.CalendarDate.Trim & " " & Now.ToLongTimeString) < dtTaskDate Then
                '    lstError.Items.Add("Action date cannot be less than Task Date...")
                '    shFlag = 1
            ElseIf CDate(dtActionDate.CalendarDate) < dtTaskDate.ToString("yyyy-MMM-dd") Then
                lstError.Items.Add("Action date cannot be less than Task Date...")
                shFlag = 1
            ElseIf CDate(dtActionDate.CalendarDate.Trim) > Now.ToShortDateString Then
                lstError.Items.Add("Action date cannot be more than current date...")
                shFlag = 1
            End If
        Else
            lstError.Items.Add("Please enter date for the Action...")
            shFlag = 1
        End If

        Dim intAddressNo As Integer
        intAddressNo = SQL.Search("Work_view", "SaveAction-2228", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & TxtActionOwner_F.Text.Trim & "")
        If intAddressNo <= 0 Then
            lstError.Items.Add("Task Owner mismatch...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Return False
            Exit Function
        End If

        lstError.Items.Clear()

        Try

            arrColumns.Add("AM_NU9_Call_Number")
            arrColumns.Add("AM_NU9_Task_Number")
            arrColumns.Add("AM_NU9_Comp_ID_FK")
            arrColumns.Add("AM_VC_2000_Description")
            arrColumns.Add("AM_CH1_Mandatory")
            arrColumns.Add("AM_FL8_Used_Hr")
            arrColumns.Add("AM_VC8_Supp_Owner")
            arrColumns.Add("AM_DT8_Action_Date_Auto")
            If Not dtActionDate.CalendarDate.Trim.Equals("") Then
                arrColumns.Add("AM_DT8_Action_Date")
            End If
            arrColumns.Add("AM_NU9_Action_Number")


            arrRows.Add(ViewState("CallNo"))
            arrRows.Add(ViewState("TaskNo"))
            arrRows.Add(ViewState("CompanyID"))
            arrRows.Add(TxtDescription_F.Text.Trim)
            If chkMandatoryHr.Checked = True Then
                arrRows.Add("M")
            Else
                arrRows.Add("O")
            End If
            arrRows.Add(TxtUsedHr_F.Text.Trim)
            arrRows.Add(TxtActionOwner_F.Text.Trim)
            arrRows.Add(Now)
            If Not dtActionDate.CalendarDate.Trim.Equals("") Then
                arrRows.Add(dtActionDate.CalendarDate.Trim & " " & Now.ToShortTimeString)
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            intCallNo = SQL.Search("Work_view", "SaveAction-2295", "select isnull(max(AM_NU9_Action_Number),0) from T040031 where AM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and AM_NU9_Call_Number=" & ViewState("CallNo").ToString & " And AM_NU9_Task_Number=" & ViewState("TaskNo").ToString)
            intCallNo += 1

            arrRows.Add(intCallNo.ToString)

            If SQL.Save("T040031", "WorkView", "saveAction", arrColumns, arrRows) = True Then

                mstGetFunctionValue = WSSUpdate.UpdateTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                If mstGetFunctionValue.ErrorCode = 0 Then
                    mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("CallNo"), False, ViewState("CompanyID"))
                    If mstGetFunctionValue.ErrorCode = 0 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Action Data Saved Successfully")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        ViewState("ActionNo") = intCallNo
                        If GetFiles() = True Then
                            'shReturn = 1
                        Else
                            'shReturn = 2
                        End If
                        'Next
                        'SendActionEmail()
                        ClearAllTextBox(cpnlTaskAction)
                        garFileID.Clear()
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        Return True
                    End If

                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Error occur please try later")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If

        Catch ex As Exception
            CreateLog("WorkView", "SaveAction-2239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
#End Region

    Private Function GetFiles() As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim blnRead As Boolean
            ' For Tasks

            sqrdTempFiles = SQL.Search("Work_view", "GetFiles-2354", "select * from T040041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(Session("PropCompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

            If blnRead = True Then
                While sqrdTempFiles.Read
                    mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                    mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                    mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                    CreateFolder(ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                End While
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("Work_view", "GetFiles-2269", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, Session("PropCompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, Session("PropCompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                ' SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Work_view", "CreateFolder-2432", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & "  and VH_NU9_Action_Number=" & ActionNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, True, CallNo, TaskNumber, Session("PropCompanyID"), ActionNumber) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, Session("PropCompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, Session("PropCompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, Session("PropCompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("Work_view", "CreateFolder-2384", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Add(Msg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub
#End Region

#Region "Clear TextBoxes based on panels"
    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        Dim objTextBox As Control
        Dim objCtl As Control
        Dim objTextBox1 As Control
        For Each objTextBox In CPnl.Controls
            If TypeOf objTextBox Is TextBox Then
                CType(objTextBox, TextBox).Text = ""

            End If

            If TypeOf objCtl Is Panel Then
                For Each objTextBox1 In objCtl.Controls
                    If TypeOf objTextBox1 Is TextBox Then
                        CType(objTextBox1, TextBox).Text = ""
                    End If
                Next
            End If
        Next
        dtActionDate.CalendarDate = ""
    End Sub
#End Region

#Region "Refresh Grid With no selection"
    Private Sub RefreshSelection()
        'viewstate("CallNo") = -1    '//For refreshing seleted callnumber
        'mstrCallNumber = -1
        'viewstate("TaskNo") = 1
        'Call CreateDataTableAction("")
        'Call BindGridAction()
    End Sub
#End Region

    Private Function CreateFooterLabelAction() As String
        Dim strHTML As String
        strHTML = strHTML + "  <TABLE bordercolor=#c0c0c0 style='BORDER-COLLAPSE: collapse'; cellSpacing='0'; width='100%'; border='1' ;background-color: #E0E0E0 >"
        strHTML = strHTML + " <TR bgcolor= #E0E0E0>"
        strHTML = strHTML + " <TD width='3%' border=1  height=20><b><font face='Verdana' size='1'>Com </font></b>  </TD>"
        strHTML = strHTML + " <TD width='3%'><b><font face='Verdana' size='1'>Att</font></b></TD>"
        strHTML = strHTML + " <TD width='13%'><b><font face='Verdana' size='1'>Action_No</font></b></TD>"
        'strHTML = strHTML + " <TD width='13%'><b><font face='Verdana' size='1'>Action</font></b></TD>"
        strHTML = strHTML + " <TD width='38%'><b><font face='Verdana' size='1'>Description</font></b></TD>"
        strHTML = strHTML + " <TD width='5%'><b><font face='Verdana' size='1'>M/O</font></b></TD>"
        strHTML = strHTML + " <TD width='11%'><b><font face='Verdana' size='1'>Used_Hr.</font></b></TD>"
        strHTML = strHTML + " <TD width='14%'><b><font face='Verdana' size='1'>Action_Owner</font></b></TD>"
        'strHTML = strHTML + " <TD width='13%'><b><font face='Verdana' size='1'>Priority</font></b></TD>"
        strHTML = strHTML + " <TD width='13%'><b><font face='Verdana' size='1'>Action_Date</font></b></TD>"
        strHTML = strHTML + "</TR>"
        strHTML = strHTML + "</TABLE>"
        Return strHTML
    End Function


    Private Sub ddlstview_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged
        'viewstate("HistoricViewName") = ddlstview.SelectedItem.Text
        'viewstate("HistoricViewValue") = ddlstview.SelectedValue
        'SaveUserView()
    End Sub
    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        GrdAddSerach.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber

        If ddlstview.SelectedValue = 0 Then
            FillDefault()
        Else
            Fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub
    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (GrdAddSerach.CurrentPageIndex > 0) Then
            GrdAddSerach.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        If ddlstview.SelectedValue = 0 Then
            FillDefault()
        Else
            Fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub
    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (GrdAddSerach.CurrentPageIndex < (GrdAddSerach.PageCount - 1)) Then
            GrdAddSerach.CurrentPageIndex += 1
            If GrdAddSerach.PageCount = Int32.Parse(CurrentPg.Text) Then
                CurrentPg.Text = GrdAddSerach.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        If ddlstview.SelectedValue = 0 Then
            FillDefault()
        Else
            Fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If

        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub
    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        GrdAddSerach.CurrentPageIndex = (GrdAddSerach.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        If ddlstview.SelectedValue = 0 Then
            FillDefault()
        Else
            Fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub
    Private Sub SaveUserView()
        Dim intid = 536
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
                'Save message
            Else
                'Error message
            End If
        End If
    End Sub
    Private Sub ChkSelectedView()
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=536 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

        If blnReturn = False Then
            ViewState("HistoricViewName") = "Default"
            ViewState("HistoricViewValue") = "0"
            Exit Sub
        Else
            While sqdrCol.Read
                ViewState("HistoricViewName") = sqdrCol.Item("UV_VC50_View_Name")
                ViewState("HistoricViewValue") = sqdrCol.Item("UV_IN4_View_ID")

                ddlstview.SelectedValue = ViewState("HistoricViewValue")

            End While
            sqdrCol.Close()
        End If
    End Sub

    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            Dim intA As Integer = 0
            For intI = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1 + 3
                If intI > 2 Then
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "F" Then
                        If e.Item.Cells.Count > 3 Then
                            e.Item.Cells(intA + 3).Width = System.Web.UI.WebControls.Unit.Parse("0px")
                            e.Item.Cells(intA + 3).Visible = False
                        End If
                    Else
                        If e.Item.Cells.Count > 3 Then
                            e.Item.Cells(intA + 3).Width = System.Web.UI.WebControls.Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intA) & "pt")
                        End If
                    End If
                    intA += 1
                ElseIf intI = 0 Then
                    e.Item.Cells(0).Width = System.Web.UI.WebControls.Unit.Parse("20px")
                ElseIf intI = 1 Then
                    If e.Item.Cells.Count > 1 Then
                        e.Item.Cells(1).Width = System.Web.UI.WebControls.Unit.Parse("20px")
                    End If
                ElseIf intI = 2 Then
                    If e.Item.Cells.Count > 2 Then
                        e.Item.Cells(2).Width = System.Web.UI.WebControls.Unit.Parse("17px")
                    End If
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GrdAddSerach_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddSerach.SortCommand
        ViewState("SortOrder") = e.SortExpression
        SortGRD()
    End Sub
    Private Sub SortGRD()
        ' If SortWay Mod 2 = 0 Then
        If Val(Session("SortWay")) Mod 2 = 0 Then
            mdvtable.Sort = ViewState("SortOrder") & " ASC"
        Else
            mdvtable.Sort = ViewState("SortOrder") & " DESC"
        End If
        '   SortWay += 1
        Session("SortWay") += 1
        If GrdAddSerach.AutoGenerateColumns = False Then
            GrdAddSerach.AutoGenerateColumns = True
        End If
        rowvalue = 0
        GrdAddSerach.DataSource = mdvtable
        GrdAddSerach.DataBind()
        GridRowSelection()
    End Sub
    '*******************************************************************
    ' Function             :-  SortGRDDuplicate
    ' Purpose             :- if some one click on the grid column sorting and
    '                                page post back from somewhere else the maintain the sorting     
    '                                call this funcation
    '								
    ' Date					  Author						Modification Date					Description
    '		                      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub SortGRDDuplicate()
        Try
            ' If SortWay Mod 2 = 0 Then
            If Val(Session("SortWay")) Mod 2 = 0 Then
                mdvtable.Sort = ViewState("SortOrder") & " DESC"
            Else
                mdvtable.Sort = ViewState("SortOrder") & " ASC"
            End If

            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            rowvalue = 0
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.DataBind()
            GridRowSelection()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GridRowSelection()
        Dim dgi As DataGridItem
        If mintCompId <> "" Or mintTaskNoRowID <> "" Or mintCallNoPlace <> -1 Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(mintCompId + 3).Text.Trim = ViewState("CompanyName") And Val(dgi.Cells(mintCallNoPlace + 3).Text.Trim) = Val(ViewState("CallNo")) And Val(dgi.Cells(mintTaskNoRowID + 3).Text.Trim) = Val(ViewState("TaskNo")) Then
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskAction.Enabled = True
                    cpnlTaskAction.TitleCSS = "test"
                    cpnlTaskAction.Text = "Action View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & " Task# " & ViewState("TaskNo") & " Company:  " & intComp & ")"
                    Exit For
                Else
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.Enabled = False
                    cpnlTaskAction.TitleCSS = "test2"
                    cpnlTaskAction.Text = "Action View"
                End If
            Next
        Else
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View"
        End If
        If GrdAddSerach.Items.Count = 0 Then
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View"
            CurrentPg.Text = 0
        End If
    End Sub
    Protected Sub SortGridAction(ByVal sender As Object, ByVal e As CommandEventArgs)
        ViewState("SortOrderAction") = e.CommandArgument
        SortGRDAction()
    End Sub
    Private Sub SortGRDAction()
        If Val(ViewState("SortWayAction")) Mod 2 = 0 Then
            dtvAction.Sort = ViewState("SortOrderAction") & " ASC"
        Else
            dtvAction.Sort = ViewState("SortOrderAction") & " DESC"
        End If
        ViewState("SortWayAction") += 1
        grdAction.DataSource = dtvAction
        grdAction.DataBind()
    End Sub
    Private Sub SortGRDDuplicateAction()
        Try
            If Val(ViewState("SortWayAction")) Mod 2 = 0 Then
                dtvAction.Sort = ViewState("SortOrderAction") & " DESC"
            Else
                dtvAction.Sort = ViewState("SortOrderAction") & " ASC"
            End If
            grdAction.DataSource = dtvAction
            grdAction.DataBind()
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
