Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports ION.Logging
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

'Session("PropTaskNumber") wityh ViewState("TaskNo")
'Session("propCAComp") with ViewState("CompanyID")
'session("propcallnumber") with ViewState("CallNo")

Partial Public Class ToDoList
    Inherits System.Web.UI.Page
    Protected WithEvents grdAction As New DataGrid
    Protected WithEvents dtgTask As New System.Web.UI.WebControls.DataGrid
    Protected WithEvents ImgClose As System.Web.UI.WebControls.ImageButton
    Private intID As Int16
    Private intFlag As Byte 'Flag to check State of Action

#Region "global level declaration"

    Private mdvtable As New DataView  ' store data from table for view grid 
    Private dtvAction As New DataView
    Private dtvTask As New DataView

    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 
    Public introwvalues As Integer

    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '*************************************************
    Private arrtextvalue As ArrayList = New ArrayList
    '*************************************************

    Public mintPageSize As Integer
    Protected _currentPageNumber As Int32 = 1

    'these variable store the position of the columns
    '****************************************
    Private callNoColumnNo As Integer
    Private compIdColumnNo As String
    Private suppOwnIDColumnNo As String
    Private suppOwnColumnNo As String
    Private assignByColumnNo As String
    Private assignByIdColumnNo As String
    Private taskNoColumnNo As String
    '****************************************

    Private txthiddenImage As String 'Stored clicked button's caption  
    Public mstrTaskNumber As String 'Stored task number when click on task grid
    Public mstrCallNumber As String 'Stored call number when click on task grid
    Private mActionRowValue As Integer 'Stored row's value when u click on task grid

    Private mintUserID As Integer 'store login user ID
    Private intComp As String 'store login company ID
    Private intColumnCount As Integer  'grid columns count

    Public mstrcomp As String

    Private mintFileID As Integer
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Private null As System.DBNull
    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments
    Private tclAction() As TemplateColumn
    Private tclTask() As TemplateColumn

    'Private strFilterTask As String    ' Used to Store Where Condition for Task grid
    'Private strSearchTask As String   ' Used to store Value of Search Control
    Private strPanelState As String


#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '***********************************
        'to store value in session to stop f5 duplicate data while pressing f5 in data entry
        strPanelState = cpnlCallTask.State.ToString


        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx;"
        Me.Header.Controls.Add(meta)
        '###########################

        If Not Page.IsPostBack Then
            Session("tdlupdate") = Server.UrlEncode(System.DateTime.Now.ToString())

            Session("tdlTaskSortWay") = 0
            Session("tdlTaskSortOrder") = Nothing

            Session("tdlCallTaskSortWay") = Nothing
            Session("tdlCallTaskOrderTask") = Nothing
            Session("tdlActionSortWay") = Nothing
            Session("tdlActionSortOrder") = Nothing

            ViewState("Flag") = 0 'To  know the status of Task whether it is closed
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
            Dim arrColumnsNameAction As New ArrayList
            Dim arrWidthAction As New ArrayList
            Dim arrColumnsWidthAction As New ArrayList

            ViewState.Add("arrHeadersTask", arrHeadersTask)
            ViewState.Add("arrFooterTask", arrFooterTask)
            ViewState.Add("arrColumnsNameAction", arrColumnsNameAction)
            ViewState.Add("arrWidthAction", arrWidthAction)
            ViewState.Add("arrColumnsWidthAction", arrColumnsWidthAction)
            '******************************************************

            Dim arrColumnsNameTask As New ArrayList
            Dim arrWidthTask As New ArrayList
            Dim arrColumnsWidthTask As New ArrayList

            ViewState.Add("arrColumnsNameTask", arrColumnsNameTask)
            ViewState.Add("arrWidthTask", arrWidthTask)
            ViewState.Add("arrColumnsWidthTask", arrColumnsWidthTask)

        End If
        'paging
        mintPageSize = Val(Request.Form("cpnlTaskView$txtPageSize"))
        If IsPostBack = False Then
            If ChkPageView() = True Then
                txtPageSize.Text = Session("tdlPageSize")
                mintPageSize = Session("tdlPageSize")
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = 20
                    txtPageSize.Text = mintPageSize
                    Session("tdlPageSize") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    Session("tdlPageSize") = mintPageSize
                    SavePageSize()
                End If
            End If
        Else
            If Session("tdlPageSize") = mintPageSize Then
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = Session("tdlPageSize")
                    txtPageSize.Text = Session("tdlPageSize")
                Else
                    txtPageSize.Text = mintPageSize
                    Session("tdlPageSize") = mintPageSize
                End If

                SavePageSize()
            End If
        End If
        txtPageSize.Text = mintPageSize
        '******************************

        If IsPostBack = False Then
            txtCSS(Me.Page)
            dtActionDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
        Else
            If Val(ViewState("TaskNo")) > 0 And Val(Request.Form("txthiddenCallNo")) > 0 And Request.Form("txtComp") <> "" Then
                If Val(Request.Form("txtTask")) <> Val(ViewState("TaskNo")) Or Val(ViewState("CallNo")) <> Val(Request.Form("txthiddenCallNo")) Or Session("CompName") <> Request.Form("txtComp") Then
                    dtActionDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
                    Txtdescription_F.Text = ""
                    TxtUsedHr_F.Text = ""
                    SetControlFocus(Txtdescription_F)
                End If
            ElseIf Val(Request.Form("txthiddenCallNo")) > 0 And Request.Form("txtComp") <> "" Then
                SetControlFocus(Txtdescription_F)
            End If
        End If

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Response.Expires = -1

        'javascript function added with controls
        '**********************************************************************************
        Txtdescription_F.Attributes.Add("onmousemove", "ShowToolTip(this,2000);")
        BtnGrdSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        BtnGrdSearch1.Attributes.Add("Onclick", "return SaveEdit('Search');")
        TxtUsedHr_F.Attributes.Add("onkeypress", "UsedHour('cpnlTaskAction_TxtUsedHr_F')")
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgShowReleased.Attributes.Add("Onclick", "return SaveEdit('ShowReleased');")
        imgActionView.Attributes.Add("Onclick", "return SaveEdit('ActionView');")
        imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
        imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        imgFWD.Attributes.Add("Onclick", "return SaveEdit('Fwd');")
        imgAttachAction.Attributes.Add("OnClick", "return ShowActions();")
        imgCloseTask.Attributes.Add("OnClick", "return SaveEdit('CloseTask');")
        Txtdescription_F.Attributes.Add("onkeypress", "ChangeHeight(this.value,this.id)")
        Txtdescription_F.Attributes.Add("onkeyup", "ChangeHeight(this.value,this.id)")
        imgPlusCSS.Attributes.Add("Onclick", "return OpenVW('T040021');")
        txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
        ddlstview.Attributes.Add("OnChange", "return SaveEdit('View');")
        '*******************************************************************************
        Call txtCSS(Me.Page, "cpnlTaskAction")
        If IsPostBack Then
            If Val(Request.Form("txtHIDSize")) > 0 And Txtdescription_F.Text.Length > 0 Then
                Txtdescription_F.Height = Unit.Pixel(Val(Request.Form("txtHIDSize")))
            End If
        End If
        If dtActionDate.Text = "" Then
            dtActionDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
        End If
        'if task not opened
        '******************************************
        GrdAddSerach.Visible = True
        Panel1.Visible = True
        'ddlstview.Enabled = True
        '******************************************
        Dim StrUserID As String
        'get logged user id
        mintUserID = HttpContext.Current.Session("PropUserID")
        StrUserID = HttpContext.Current.Session("PropUserID")
        Session("gshPageStatus") = 0

        txthiddenImage = Request.Form("txthiddenImage")
        introwvalues = Request.Form("txtrowvalues")
        'Find the selected call no. company
        If Request.Form("txtComp") <> "undefined" Then
            If IsPostBack Then
                If Request.Form("txtComp") <> "" Then
                    intComp = Request.Form("txtComp")
                    Session("CompName") = Request.Form("txtComp")
                    mstGetFunctionValue = WSSSearch.SearchCompName(Request.Form("txtComp"))
                    mstrcomp = intComp
                    ' ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
                    ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
                    'Else
                    mstrcomp = 0
                End If
            End If
        Else
            Dim a As Integer
            a = 0
        End If
        strhiddenTable = Request.Form("txthiddenTable")
        If strhiddenTable = "cpnlTaskAction_grdAction" Then
            ViewState("ActionNo") = Val(Request.Form("txtActionNo"))

        Else
            ' Clear all textBoxes in fastentry if Task no. is changed and currently we have clicked on Task grid
            If Val(Request.Form("txtTask")) <> 0 And Val(ViewState("TaskNo")) <> Val(Request.Form("txtTask")) Then
                ClearAllTextBox(cpnlTaskAction)
            End If
            '-----------------------------------------
            ViewState("ActionNo") = 0
            If IsNothing(Request.Form("txtTask")) = False Then
                'ViewState("TaskNo") = Val(Request.Form("txtTask"))
                ViewState("TaskNo") = Val(Request.Form("txtTask"))
                'mstrTaskNumber = ViewState("TaskNo")
                mstrTaskNumber = Val(ViewState("TaskNo"))

            End If
            ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
            mstrCallNumber = ViewState("CallNo")
            Dim strStatus As String = WSSSearch.GetTaskStatus(mstrCallNumber, mstrTaskNumber, ViewState("CompanyID"))
            ViewState("tdlTaskStatus") = strStatus
            txtTaskStatus.Value = strStatus 'this txt box used in HTML CODE
            If strStatus = "CLOSED" Then
                PnlAction.Visible = False
            Else
                PnlAction.Visible = True
            End If
        End If

        '------------------------------------------
        If Val(ViewState("ActionNo")) < 1 And Val(ViewState("TaskNo")) < 1 Then
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
                        Session("TDLViewName") = ddlstview.SelectedItem.Text
                        Session("TDLViewValue") = ddlstview.SelectedValue
                        If Session("Flag") = "1" Then
                            GetView()
                            Session("Flag") = 0
                        Else
                            SaveUserView()
                        End If
                        'Session("propCallNumber")  = 0
                        ViewState("CallNo") = 0
                        'Session("PropTaskNumber") = 0
                        ViewState("TaskNo") = 0
                        ' ViewState("CompanyID") = 0
                        ViewState("CompanyID") = 0

                    Case "Edit"
                        If strhiddenTable = "cpnlTaskAction_grdAction" Then
                            Exit Select
                        End If
                    Case "ShowReleased" '--Show Workable tasks only
                        If Val(Session("ShowReleased")) = 0 Then ' -- Toggle
                            Session("ShowReleased") = 1
                        Else
                            Session("ShowReleased") = 0
                        End If
                    Case "Select"
                    Case "CloseTask"
                        If SaveAction() = True Then       ' Save Task Info 
                            lstError.Items.Clear()
                            lstError.Items.Add("Action Data Saved Successfully...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            intFlag = 1 'Action Saved
                        Else
                            intFlag = 0 ' action not saved
                        End If
                        UpdateTask()
                    Case "CloseCall"
                        If Session("TDLmshCall") = 0 Then
                            Session("TDLmshCall") = 1
                            'for clear the selection of action grid
                            '**********************************************
                            'ViewState("TaskNo") = 0
                            ViewState("TaskNo") = 0
                            ViewState("CallNo") = 0
                            mstrTaskNumber = "0"
                            mstrCallNumber = "0"
                            '*************************************
                        Else
                            Session("TDLmshCall") = 0
                            'for clear the action grid
                            '***************************
                            'ViewState("TaskNo") = 0
                            ViewState("TaskNo") = 0
                            ViewState("CallNo") = 0
                            mstrTaskNumber = "0"
                            mstrCallNumber = "0"
                            '***************************************
                        End If
                    Case "Delete"
                        If ViewState("ActionNo") = 0 Then
                            ' Check that task is not in progress
                            mstGetFunctionValue = WSSSearch.SearchTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                            If mstGetFunctionValue.ErrorCode = 1 Then
                                ' Delete TASK
                                mstGetFunctionValue = WSSDelete.DeleteTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                                If mstGetFunctionValue.ErrorCode = 0 Then
                                    '''''''''''Rollback Call Status'''''''''''
                                    Dim intRows As Integer
                                    If SQL.Search("ToDoList", "Load-289", "select * from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                        Dim arrCallStatusColUpdate As New ArrayList
                                        Dim arrCallStatusRowUpdate As New ArrayList
                                        arrCallStatusColUpdate.Add("CN_VC20_Call_Status")
                                        arrCallStatusRowUpdate.Add("OPEN")
                                        WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arrCallStatusColUpdate, arrCallStatusRowUpdate)
                                    End If
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Task Deleted Successfully...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                    introwvalues = 0
                                    mstrTaskNumber = 0
                                ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Error occured while deleting record...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If
                            Else
                                lstError.Items.Clear()
                                lstError.Items.Add("This task is in PROGRESS so it cannot be deleted...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            End If
                        Else
                            ' Delete ACTION
                            'get the status of the task 
                            Dim strTaskStatus As String
                            strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                            If strTaskStatus = "CLOSED" Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Action cannot be deleted for a Closed Task...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Else
                                mstGetFunctionValue = WSSDelete.DeleteAction(ViewState("CallNo"), ViewState("TaskNo"), Val(Request.Form("txtActionNo")), ViewState("CompanyID"))
                                If mstGetFunctionValue.ErrorCode = 0 Then
                                    '''''''''''Rollback Task Status'''''''''''
                                    Dim intRows As Integer
                                    If SQL.Search("ToDoList", "Load-326", "select * from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Task_Number=" & ViewState("TaskNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                        Dim arrTaskStatusColUpdate As New ArrayList
                                        Dim arrTaskStatusRowUpdate As New ArrayList
                                        arrTaskStatusColUpdate.Add("TM_VC50_Deve_status")
                                        arrTaskStatusRowUpdate.Add("ASSIGNED")
                                        WSSUpdate.UpdateTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"), arrTaskStatusColUpdate, arrTaskStatusRowUpdate)
                                    End If
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    '''''''''''Rollback Call Status if there is no action left behind the call'''''''''''
                                    If SQL.Search("ToDoList", "Load-337", "select * from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                        Dim arrCallStatusColUpdate As New ArrayList
                                        Dim arrCallStatusRowUpdate As New ArrayList
                                        arrCallStatusColUpdate.Add("CN_VC20_Call_Status")
                                        arrCallStatusRowUpdate.Add("ASSIGNED")
                                        WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arrCallStatusColUpdate, arrCallStatusRowUpdate)
                                    End If
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Action Deleted Successfully...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                    introwvalues = 0
                                    mstrTaskNumber = ViewState("TaskNo")
                                ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Action Not Deleted please try later...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If
                            End If
                        End If
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Clear()
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If
                        'End of Security Block
                        If Val(ViewState("TaskNo")) <> 0 Then
                            If dtActionDate.Text.Trim.Equals("") OrElse Txtdescription_F.Text.Trim.Equals("") Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Action description can not be blank...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            End If
                            If SaveAction() = True Then       ' Save Task Info 
                                lstError.Items.Clear()
                                lstError.Items.Add("Action Data Saved Successfully...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            End If
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Please select task to save Action...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        End If
                    Case "Attach"
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("TODO List", "Load-399", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If
        imgAttachments.ToolTip = "Please select a Task"
        imgAttachments.Attributes.Add("Onclick", "return ViewAttachment(-1);")
        If Val(ViewState("CallNo")) > 0 Then
            If ChangeAttachmentToolTip(ViewState("CompanyID"), ViewState("CallNo")) = True Then
                imgAttachments.ToolTip = "View Attachments"
                imgAttachments.Attributes.Add("Onclick", "return ViewAttachment(1);")
            Else
                imgAttachments.ToolTip = "No Attachments Uploaded"
                imgAttachments.Attributes.Add("Onclick", "return ViewAttachment(0);")
            End If
        End If
        If Not IsPostBack Then
            imgAttachments.ToolTip = "Please select a Task"
            imgAttachments.Attributes.Add("Onclick", "return ViewAttachment(-1);")
        End If
        If Session("TDLmshCall") = 0 Then
            imgCloseCall.ToolTip = "View only closed tasks"
        Else
            imgCloseCall.ToolTip = "View only open tasks"
        End If
        If Val(Session("ShowReleased")) = 0 Then ' -- Change Tooltip for show released tasks
            imgShowReleased.ToolTip = "Show only released tasks"
        Else
            imgShowReleased.ToolTip = "Show all tasks"
        End If

        Try

            If Page.IsPostBack And IsNothing(dtvAction.Table) = False Then
            End If
            CreateGridAction()
            CreateDataTableAction()
            FillHeaderArrayAction()
            FillFooterArrayAction()
            createTemplateColumnsAction()
            ' BindGridAction()
            '//////////////////

            If Not IsPostBack Then
                cpnlCallTask.Text = "Call Detail"
                cpnlCallTask.TitleCSS = "test2"
                cpnlCallTask.Enabled = False
            Else
                If Val(ViewState("CallNo")) > 0 Then
                    cpnlCallTask.Text = "Call Detail (Call # " & Val(ViewState("CallNo")) & " Company: " & intComp & ")"
                    cpnlTaskList.Text = "Task List (Call # " & Val(ViewState("CallNo")) & " Company: " & intComp & ")"
                    cpnlCallTask.TitleCSS = "test"
                    cpnlCallTask.Enabled = True
                    Call FillCallDetail()
                End If
            End If

            dtgTask.Columns.Clear()
            dtgTask.Controls.Clear()
            Call CreateDataTableTask()
            Call CreateGridTask()
            Call FillFooterArrayTask()
            Call createTemplateColumnsTask()
            Call BindGridTask()

            If Not IsPostBack Then
                If Session("TDLmshCall") = Nothing Then
                    Session("TDLmshCall") = 0
                End If
                If Session("ShowReleased") = Nothing Then ' if session is not assigned then set to default
                    Session("ShowReleased") = 0
                End If
                imgCloseCall.ToolTip = "View only closed tasks"

                ViewState("CallNo") = 0
                mstrCallNumber = 0
                'fill dropdown combo with view name from database
                GetView()
                ChkSelectedView() 'chk user selected view last time

                If Session("TDLViewName") <> "" And Session("TDLViewName") <> "Default" Then
                    ' fill datagrid based on user define columns and combination
                    Fillview()
                Else
                    'fill tha datagrid from based on admin defined to the role
                    fillDefault()
                    Session("TDLViewName") = "Default"
                End If
                CurrentPg.Text = _currentPageNumber.ToString()
                'format the grid based on data base info
                CreateTextBox()
            Else
                If ViewState("CallNo") > 0 Then
                    If SaveAction() = True Then    ' Save Task Info 
                        Call ClearAllTextBox(cpnlTaskAction)
                        txthiddenImage = "Select"
                    Else
                    End If
                End If
                '**********************************
                arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
                ChkActType.Checked = True
                'this loop filling new arraylist in the arrtextvalue array
                For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                    arrtextvalue.Add(Request.Form("cpnlTaskView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
                Next

                If ddlstview.SelectedValue = 0 Then
                    'fill tha datagrid from based on admin defined to the role
                    fillDefault()
                Else
                    ' fill datagrid based on user define columns and combination
                    Fillview()
                End If

                arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
                ChkActType.Checked = True
                'this loop filling new arraylist in the arrtextvalue array
                For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                    arrtextvalue.Add(Request.Form("cpnlTaskView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
                Next
                CreateTextBox()
            End If
            'recreate Task Query and bind the grid
            If GrdAddSerach.Enabled = True Then
                Call CreateDataTableAction()
                Call BindGridAction()
            End If

            If GrdAddSerach.Enabled = False Then
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                If dtActionDate.Text = "" Then
                    dtActionDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
                End If
            End If
        Catch ex As Exception
            CreateLog("TODO List", "Load-771", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        Try

            If Not IsPostBack Then
                cpnlTaskAction.Enabled = False
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskAction.TitleCSS = "test2"
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;"
            ElseIf Not ViewState("TaskNo") = "0" Then
                cpnlTaskAction.Enabled = True
                cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                cpnlTaskAction.TitleCSS = "test"
            End If

            'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
            '************************************************
            If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
                FillGRDAfterSearch()
            End If


            If IsNothing(Session("SortOrder")) = False Then
                SortGRDDuplicate()
            End If

            If IsNothing(Session("tdlCallTaskOrderTask")) = False Then
                SortGRDDuplicateTask()
            End If

            If IsNothing(Session("tdlActionSortOrder")) = False Then
                SortGRDDuplicateAction()
            End If

            If Val(ViewState("TaskNo")) > 0 And Val(Request.Form("txthiddenCallNo")) > 0 And Request.Form("txtComp") <> "" Then
                If GetDepStat(ViewState("CallNo"), Session("CompName"), ViewState("TaskNo")) = True Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please check Task Dependency...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            End If


            'Restore the grid selection on click of grid's row when page post back
            '*************************************************************************
            GridRowSelection()
            '**********************************************************************

            If Val(ViewState("CallNo")) > 0 Then
                imgSave.ToolTip = "Save Action"
                imgEdit.ToolTip = "Edit Selected Task/Action"
                imgDelete.ToolTip = "Delete Selected Action"
                imgFWD.ToolTip = "Forward Selecetd Task"
                imgCloseTask.ToolTip = "Close Selected Task"
            Else
                imgSave.ToolTip = "Select a Task to Add Actions"
                imgEdit.ToolTip = "Select a Task to Edit"
                imgDelete.ToolTip = "Select a Task to Delete Action"
                imgFWD.ToolTip = "Select a Task to Forward"
                imgCloseTask.ToolTip = "Select a Task to Close"
            End If

            If Val(Session("TDLmshCall")) = 1 Then
                imgCloseCall.ToolTip = "View only open tasks"
            Else
                imgCloseCall.ToolTip = "View only closed tasks"
            End If

            If Val(Session("ShowReleased")) = 1 Then
                imgShowReleased.ToolTip = "Show all tasks"
            Else
                Session("ShowReleased") = "Show only released tasks"
            End If

        Catch ex As Exception
            CreateLog("TODO List", "row color Load-822", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try

        'set alternate color setting on grid
        ' *************************************
        GrdAddSerach.AlternatingItemStyle.BackColor = Color.FromArgb(245, 245, 245)
        GrdAddSerach.ItemStyle.BackColor = Color.FromArgb(255, 255, 255)
        '**************************************

        'Security Block
        '******************************************
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            intID = 8
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
            '**********************************************************
        End If
    End Sub

#End Region

    Sub fillDefault()


        Try
            Dim dsDefault As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            '**************
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select "
            Dim strwhereQuery As String = " and "
            Dim shJoin As Short

            GrdAddSerach.PageSize = mintPageSize ' set the grid page size

            Dim strQuery As String

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =502 And " _
                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                    & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=502  and obm_vc4_object_type_fk='VIW') " _
                    & " order by OBM.OBM_SI2_Order_By"

            ' SQL.DBTable = "T070042"
            sqrdView = SQL.Search("ToDoList", "Filldefault-590", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then

                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                CType(ViewState("arColWidth"), ArrayList).Clear()

                Dim htDateCols As New Hashtable
                While sqrdView.Read
                    If sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID" Then
                        strSelect &= "SOwner." & "UM_VC50_UserID" & ","
                        shJoin += 1

                    ElseIf sqrdView.Item("OBM_VC200_URL") = "UM_VC50_UserID_Assign" Then
                        strSelect &= "ABy." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_NU9_Call_No_FK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & "),"
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Est_close_date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Task_Date" Then
                        ' strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",100)" & ","
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "TM_DT8_Task_Close_Date" Then
                        'strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",100)" & ","
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    Else
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","
                    End If

                    CType(ViewState("arColWidth"), ArrayList).Add(sqrdView.Item("OBM_VC200_DESCR"))  'adding columns widthe in arraylist

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
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp where  TM_VC8_Supp_Owner=" & mintUserID & " and task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK"
                ElseIf shJoin = 2 Then
                    strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp where  TM_VC8_Supp_Owner=" & mintUserID & " and  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK"
                ElseIf shJoin = 3 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where  TM_VC8_Supp_Owner=" & mintUserID & " and task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                Else
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where  TM_VC8_Supp_Owner=" & mintUserID & " and task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                End If

                If Session("TDLmshCall") = 1 Then
                    strSelect &= " and TM_VC50_Deve_Status='Closed' "
                Else
                    strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
                End If

                'If Val(Session("ShowReleased")) = 1 Then ' -- If user has pressed Show Released Tasks buttons 
                '    strSelect &= " AND TM_NU9_Dependency is null or TM_NU9_Dependency in (select tm_nu9_task_no_pk from T040021 Dependents  where  Dependents.TM_NU9_Call_No_FK=T040021.TM_NU9_Call_No_FK and Dependents.TM_NU9_Comp_ID_FK=T040021.TM_NU9_Comp_ID_FK and Dependents.TM_VC50_Deve_status='CLOSED')"
                'End If

                strSelect &= " and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  "
                'Added company chk from company access table
                strSelect &= " and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
                strSelect &= " order by TM_NU9_Call_No_FK desc,TM_NU9_Task_no_PK asc"

                callNoColumnNo = -1
                compIdColumnNo = ""
                suppOwnIDColumnNo = ""
                suppOwnColumnNo = ""
                assignByColumnNo = ""
                assignByIdColumnNo = ""
                taskNoColumnNo = ""


                '  SQL.DBTable = "T040021"
                If SQL.Search("T040021", "ToDoList", "Filldefault-678", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T040021").Columns.Count - 1
                        dsDefault.Tables("T040021").Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO" Then
                            callNoColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID" Then
                            compIdColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPOWNID" Then
                            suppOwnIDColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKOWNER" Then
                            suppOwnColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBY" Then
                            assignByColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBYID" Then
                            assignByIdColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKNO" Then
                            taskNoColumnNo = inti
                        End If
                    Next

                    mdvtable.Table = dsDefault.Tables("T040021")

                    Dim htDescCols As New Hashtable
                    htDescCols.Add("TaskDesc", 37)
                    htDescCols.Add("CallDesc", 44)
                    htDescCols.Add("CallSubject", 29)
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htDescCols)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    rowvalue = 0
                    rowvalueCall = 0

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.AllowPaging = True
                    GrdAddSerach.PageSize = mintPageSize

                    If Session("TDLViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                        Session("tdlTaskSortOrder") = Nothing
                    End If

                    If Val(Session("ShowReleased")) = 1 Then
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

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= (mdvtable.Table.Rows.Count) Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If

                    GrdAddSerach.DataBind()
                    HTMLEncodeDecode(mdlMain.Action.Decode, mdvtable, htDateCols)

                    ''paging count
                    Dim intRows As Integer = (mdvtable.Table.Rows.Count)
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
                    '*****************************************************
                    cpnlTaskAction.Enabled = True
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.TitleCSS = "test2"

                    cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskView.Enabled = True
                    cpnlTaskView.TitleCSS = "test"
                    '******************************************************

                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False

                    lstError.Items.Clear()
                    lstError.Items.Add("No Task Assigned...")
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
                lstError.Items.Add("Sorry! To Do List Data  not available...")
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

            CreateLog("TODO List", "FillDefault-1138", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    '*******************************************************************
    ' Function             :-  fillview
    ' Purpose              :- Fill and design datagrid based on user defined columns settings from user tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function Fillview()

        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim arcolName As New ArrayList

        Dim IntNewCount As Integer
        GrdAddSerach.PageSize = mintPageSize ' set the grid page size

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim shJoin As Short
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            Dim strwhereQuery As String = " and "

            sqrdView = SQL.Search("ToDoList", "FillView-751", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='502' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            'If SQL.Search("", "", strConnection, "select name,view_id from tbl_userview_name", dsView, "tbl_userview_detail") = True Then
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
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_NU9_Call_No_FK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & "),"
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_DT8_Est_close_date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_DT8_Task_Date" Then
                        'strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",100)" & ","
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "TM_DT8_Task_Close_Date" Then
                        'strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",100)" & ","
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
                sqrdView = SQL.Search("ToDoList", "FillView-806", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='502'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
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
                sqrdView = SQL.Search("ToDoList", "FillView-831", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='502' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

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
                            Case "TM_DT8_Est_close_date".ToUpper
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

                If shJoin = 1 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp where TM_VC8_Supp_Owner=" & mintUserID & " and task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK"
                ElseIf shJoin = 2 Then
                    strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp where TM_VC8_Supp_Owner=" & mintUserID & " and  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK"
                ElseIf shJoin = 3 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where TM_VC8_Supp_Owner=" & mintUserID & " and task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                Else
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp where TM_VC8_Supp_Owner=" & mintUserID & " and task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner"
                End If


                If Session("TDLmshCall") = 1 Then
                    strSelect &= " and TM_VC50_Deve_Status='Closed' "
                Else
                    strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
                End If

                'If Val(Session("ShowReleased")) = 1 Then ' -- If user has pressed Show Released Tasks buttons 
                '    strSelect &= " AND TM_NU9_Dependency is null or TM_NU9_Dependency in (select tm_nu9_task_no_pk from T040021 Dependents  where  Dependents.TM_NU9_Call_No_FK=T040021.TM_NU9_Call_No_FK and Dependents.TM_NU9_Comp_ID_FK=T040021.TM_NU9_Comp_ID_FK and Dependents.TM_VC50_Deve_status='CLOSED')"
                'End If

                If strwhereQuery.Equals(" and ") = True Then
                    'nothing added in query
                Else
                    'if got some data from database then add in query
                    strSelect &= strwhereQuery
                End If


                strSelect &= " and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  "

                'Added company chk from company access table
                strSelect &= " and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "


                If IsNothing(strUnsortQuery) = False Then
                    strUnsortQuery = strUnsortQuery.TrimEnd
                    strUnsortQuery = strUnsortQuery.Remove(Len(strUnsortQuery) - 1, 1)
                    'strSelect &= strOrderQuery & " " & strUnsortQuery
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

                'SQL.DBTable = "T040021"

                callNoColumnNo = -1
                compIdColumnNo = ""
                suppOwnIDColumnNo = ""
                suppOwnColumnNo = ""
                assignByColumnNo = ""
                assignByIdColumnNo = ""
                taskNoColumnNo = ""

                If SQL.Search("T040021", "ToDoList", "FillView-950", strSelect, dsFromView, "sachin", "Prashar") = True Then
                    ' DataGrid1.DataSource = dsFromView
                    'dsFromView.Tables(0).Columns(0).ColumnName = ""
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO" Then
                            callNoColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID" Then
                            compIdColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPOWNID" Then
                            suppOwnIDColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKOWNER" Then
                            suppOwnColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBY" Then
                            assignByColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "ASSIGNBYID" Then
                            assignByIdColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "TASKNO" Then
                            taskNoColumnNo = inti
                        End If

                    Next
                    mdvtable.Table = dsFromView.Tables(0)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)


                    Dim htDescCols As New Hashtable
                    htDescCols.Add("TaskDesc", 37)
                    htDescCols.Add("CallDesc", 44)
                    htDescCols.Add("CallSubject", 29)
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htDescCols)

                    GrdAddSerach.DataSource = mdvtable.Table
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    rowvalue = 0
                    rowvalueCall = 0

                    GrdAddSerach.AllowPaging = True

                    If Session("TDLViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                        Session("tdlTaskSortOrder") = Nothing
                    End If



                    If Val(Session("ShowReleased")) = 1 Then
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

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= (mdvtable.Table.Rows.Count) Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If

                    GrdAddSerach.DataBind()
                    HTMLEncodeDecode(mdlMain.Action.Decode, mdvtable)

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
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.TitleCSS = "test2"
                    cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskView.Enabled = True
                    cpnlTaskView.TitleCSS = "test"
                    '***************************************************************
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False

                    lstError.Items.Clear()
                    lstError.Items.Add("No Task Assigned or data not exist according to view query...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    cpnlTaskAction.Enabled = False
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.TitleCSS = "test2"

                    cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskView.Enabled = False
                    cpnlTaskView.TitleCSS = "test2"
                End If

            Else
                Exit Function
            End If
        Catch ex As Exception
            CreateLog("TODO List", "FillView-1000", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function

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
            'SQL.DBTable = "T030201"
            SQL.DBTracing = False

            sqrdView = SQL.Search("ToDoList", "GetVew-1139", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='502' and UV_IN4_Role_ID=" & Session("PropRole") & "  and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " and (UV_NU9_User_ID=" & Session("PropUserID") & " or UV_NU9_User_ID is null ) order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                ddlstview.DataSource = sqrdView
                ddlstview.DataTextField = "UV_VC50_View_Name"
                ddlstview.DataValueField = "UV_IN4_View_ID"
                ddlstview.DataBind()
                sqrdView.Close()
            Else

            End If
            If Session("TDLViewName") = "" Or Session("TDLViewName") = "Default" Then
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
                ddlstview.SelectedIndex = ddlstview.Items.Count - 1
            Else
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
            End If

            If Session("TDLViewName") <> "" And Session("TDLViewName") <> "Default" Then
                ddlstview.SelectedValue = Session("TDLViewValue")
            End If

        Catch ex As Exception
            CreateLog("ToDoList", "GetView-1155", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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


        ' arColumns.Clear()
        CType(ViewState("arrTextboxId"), ArrayList).Clear()
        'fill the columns count into the array from mdvtable view

        Try
            If Not mdvtable.Table Is Nothing Then
                intColumnCount = mdvtable.Table.Columns.Count
            Else
                Exit Sub
            End If
        Catch ex As Exception
            ' Exit Function
            CreateLog("ToDoList", "Mdvtable.count=0-CreateTextBox-1181", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

        'If Not IsPostBack Then
        '    ReDim mTextBox(intCol)
        'End If

        'If mTextBox.Length < intCol Then
        '    ReDim mTextBox(intCol)
        'End If

        Try
            For intii = 0 To intColumnCount - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intii))
                    'col1cng = col1.Value + 1
                    'col1cng = col1cng & "pt"
                    If intii > 12 And intii < 16 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2
                        col1cng = col1cng & "pt"
                    End If

                    'arCol.Add(arSetColumnName.Item(intii))
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
                    'col1cng = col1.Value + 1
                    'col1cng = col1cng & "pt"
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

                    '_textbox.Text = ""
                    strcolid = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "F" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=""0"" Visible=""False"" CssClass=""SearchTxtBox""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""20""></asp:TextBox>"))
                    End If
                    '  Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                    ' mTextBox(intii) = _textbox
                End If
                'mshFlag = 1
                CType(ViewState("arrTextboxId"), ArrayList).Add(_textbox.ID)
                ' arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("TODO List", "CreateTextBox-1239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

#End Region

#Region "Format datagrid columns size according to database"
    '*******************************************************************
    ' Function             :-  FormatGrid
    ' Purpose              :- Change the datagrid columns size at run time 
    '								
    ' Date					  Author						Modification Date					Description
    ' 18/01/06			  Sachin Prashar			-------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub FormatGrid()

        Dim intI As Integer

        Try
            GrdAddSerach.AutoGenerateColumns = False
            For intI = 0 To CType(ViewState("arrColumnsName"), ArrayList).Item(intI) - 1
                If CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "F" Then
                    Dim Bound_Column As New BoundColumn
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(0)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(0)
                    Bound_Column.Visible = False
                    GrdAddSerach.Columns.Add(Bound_Column)
                Else
                    Dim Bound_Column As New BoundColumn
                    Dim strWidth As String = CType(ViewState("arColWidth"), ArrayList).Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True

                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    GrdAddSerach.Columns.Add(Bound_Column)
                End If
            Next

        Catch ex As Exception
            CreateLog("TODO List", "FormatGrid-1429", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

#End Region

#Region "Serach Grid Button Click"
    '*******************************************************************
    ' Function             :-  FillGRDAfterSearch
    ' Purpose              :- grid search based on textbox data function filter the data from dataview
    '								
    ' Date					  Author						Modification Date					Description
    ' 18/01/06			  Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Function FillGRDAfterSearch()
        Dim strRowFilterString As String
        Dim strSearch As String
        Dim arCount As Integer = arrtextvalue.Count - 1
        Dim intI As Integer

        Try

            For intI = 0 To arCount
                If Not IsNothing(arrtextvalue(intI)) Then
                    'If Not mTextBox(intI).Text.Trim.Equals("") Then
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
                strRowFilterString &= " A <>0  AND "
            End If
            If CHKC.Checked = True Then
                strRowFilterString &= " C <>0 AND "
            End If
            If CHKF.Checked = True Then
                strRowFilterString &= " F <>0 AND "
            End If


            If (strRowFilterString Is Nothing) = True Then
                'shF = 1
                Exit Function
            Else
                strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)

                'mdvtable.RowFilter = strRowFilterString
                GetFilteredDataView(mdvtable, strRowFilterString)
                '  GrdAddSerach.Columns.Clear()
                HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)
                SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                GrdAddSerach.DataSource = mdvtable
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                'add columns for attachment and comment
                '*********************************************************

                '*************************************************************************
                rowvalue = 0
                rowvalueCall = 0

                If Session("TDLViewName") <> ddlstview.SelectedItem.Text Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= (mdvtable.Table.Rows.Count) Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If

                GrdAddSerach.DataBind()
                ''paging count
                Dim intRows As Integer = (mdvtable.Table.Rows.Count)
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
                If mdvtable.Count = 0 Then
                    lstError.Items.Clear()
                    If intFlag = 1 Then 'if action saved before task closed
                        lstError.Items.Add("Action Saved Successfully.. ")
                    End If
                    If ViewState("Flag") = 1 Then 'if user search any string but string not exist and also close task
                        lstError.Items.Add("Task Closed Successfully.. ")
                        lstError.Items.Add("Data not found according to your search string... ")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Else
                        If ViewState("Flag") = 0 Then
                            lstError.Items.Add("Data not found according to your search string... ") 'only search any string but string not exist
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            CreateLog("TODO List", "Click-1617", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "btngrdsearch")
        End Try
    End Function

#End Region

#Region "Search Grid Item Data Bound Event"
    '*******************************************************************
    ' Function             :-  GrdAddSerach_ItemDataBound1
    ' Purpose              :-Display attachment, comment based on database and and bound java script on columns like selection and double click
    '								
    ' Date					  Author						Modification Date					Description
    ' 18/01/06			  Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound


        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim TaskNo As String
        Dim CallNo As String
        Dim CompanyID As String
        Dim rowflag As Boolean = True
        Dim attSts As Boolean
        Dim comstat As String
        Dim intCount As Integer = 2
        Dim intcolnoComm As Integer = 0
        Dim intcolno As Int16 = 0
        Dim frmSts As String
        Dim intCountfrm As Integer = 3

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
        '***********************************

        '  GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    TaskNo = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    CallNo = e.Item.Cells(callNoColumnNo + 3).Text()
                    CompanyID = e.Item.Cells(compIdColumnNo + 3).Text()


                    e.Item.ToolTip = " Call # " & CallNo & " Task  # " & TaskNo & "  Company: " & CompanyID
                    strTaskNoRowID = taskNoColumnNo + 3

                    If suppOwnIDColumnNo <> "" Then
                        strSuppOwnID = e.Item.Cells(suppOwnIDColumnNo + 3).Text()
                        suppownrowid = suppOwnIDColumnNo + 3
                    End If

                    If suppOwnColumnNo <> "" Then
                        strSuppOwn = e.Item.Cells(suppOwnColumnNo + 3).Text()
                        strSuppOwnrowID = suppOwnColumnNo + 3
                    End If

                    If assignByColumnNo <> "" Then
                        strAssignBy = e.Item.Cells(assignByColumnNo + 3).Text()
                        strAssignByRowID = assignByColumnNo + 3
                    End If

                    If assignByIdColumnNo <> "" Then
                        strAssignByID = e.Item.Cells(assignByIdColumnNo + 3).Text()
                        AssignByRowID = assignByIdColumnNo + 3
                    End If
                    'Show attachment and comment image 
                    '************************************************************************************
                    'for attacment images********************
                    If rowflag Then
                        attSts = IIf(e.Item.Cells(mdvtable.Table.Columns.Count + 1).Text = "&nbsp;", False, True)
                        'attSts = getAttach(strName, strID, strCompId)
                    End If

                    If Not IsNothing(e.Item.Cells(0).FindControl("imgAtt")) Then
                        If attSts Then
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Attach15_9.gif"
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ToolTip = "Click To View Attachments"
                            'CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlTaskView_GrdAddSerach','" & intCount & "','" & strCompId & "','" & strName & "')")
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCount & "','" & CompanyID & "','" & CallNo & "')")
                        Else
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        End If
                    End If

                    'for comment images********************
                    If rowflag Then
                        comstat = e.Item.Cells(mdvtable.Table.Columns.Count).Text
                        'comstat = GComm(strName, strID, strCompId)
                        If Not IsNothing(e.Item.Cells(0).FindControl("imgComm")) Then
                            Select Case comstat
                                Case "1"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment2.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "Old Comments"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intcolnoComm & "','" & CompanyID & "','" & CallNo & "')")
                                    ' CType(e.Item.Cells(0).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlCallView_GrdAddSerach','" & intCount & "','" & strCompId & "','" & strID & "')")
                                Case "2"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Unread.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "New Comments"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach','" & intcolnoComm & "','" & CompanyID & "','" & CallNo & "')")
                                    'CType(e.Item.Cells(0).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlCallView_GrdAddSerach','" & intCount & "','" & strCompId & "','" & strID & "')")
                                Case "0"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "No Comment"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intcolnoComm & "','" & CompanyID & "','" & CallNo & "')")
                                Case Else
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "No Comment"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "', 'cpnlTaskView_GrdAddSerach','" & intcolnoComm & "','" & CompanyID & "','" & CallNo & "')")
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
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCountfrm & "','" & CompanyID & "','" & CallNo & "')")
                        ElseIf frmSts = "2" Then
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Form2.gif"
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ToolTip = "Empty Form"
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyViewImage('" & TaskNo & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCountfrm & "','" & CompanyID & "','" & CallNo & "')")
                        Else
                            CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        End If
                    End If
                    '*********************************************************************************************
                    If intcolno >= 3 Then
                        If intcolno = strSuppOwnrowID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strSuppOwnID & ",'" & strSuppOwn & "')")
                        ElseIf intcolno = strAssignByRowID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strAssignByID & ",'" & strAssignBy & "')")
                        Else
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheckMyTask(" & TaskNo & "," & CallNo & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & CompanyID & "','0')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & TaskNo & "," & CallNo & ", '" & rowvalue & "','cpnlTaskView_GrdAddSerach','" & CompanyID & "')")
                        End If
                    Else
                        e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                    End If
                End If
                rowflag = False
                intcolno = intcolno + 1
            Next

            If Val(ViewState("CallNo")) <> 0 And Val(ViewState("CompanyID")) <> 0 And Val(ViewState("TaskNo")) <> 0 Then
                If CallNo = Val(ViewState("CallNo")) And Session("CompName") = CompanyID And TaskNo = Val(ViewState("TaskNo")) Then
                    e.Item.BackColor = Color.FromArgb(212, 212, 212)
                End If
            End If
            rowvalue += 1
            rowvalueCall += 1
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If GetDepStat(Val(CallNo), CompanyID, Val(TaskNo)) = True Then
                    For intI As Integer = 0 To e.Item.Cells.Count - 1
                        e.Item.Cells(intI).BackColor = System.Drawing.Color.FromArgb(210, 233, 255)
                    Next
                End If
                If GetCallPriority(Val(CallNo), CompanyID) = True Then
                    For intI As Integer = 0 To e.Item.Cells.Count - 1
                        e.Item.Cells(intI).ForeColor = Color.Red
                    Next
                End If
            End If
        Catch ex As Exception
            CreateLog("TODO List", "ItemdataBound-1956", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try
    End Sub

#End Region

    Private Function CreateFolder(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo)
        'Dim objFile As File
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
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
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
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("ToDoList", "CreateFolder-1724", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

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
                'Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
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
            CreateLog("TODO List", "CreateFolder-1780", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

#Region "Create Action Grid"

    Private Sub CreateGridAction()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        Try
            grdAction.ID = "grdAction"
            grdAction.DataKeyField = "AM_NU9_Action_Number"
            Call FormatGridAction()

            PlaceHolder1.Controls.Add(grdAction)
        Catch ex As Exception
            CreateLog("TODO List", "CreateGridAction-2030", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
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
        grdAction.HorizontalAlign = HorizontalAlign.Left
        'grdAction.FooterStyle.CssClass = "GridFixedFooter"
        grdAction.SelectedItemStyle.CssClass = "GridSelectedItem"
        grdAction.AlternatingItemStyle.CssClass = "GridAlternateItem"
        grdAction.ItemStyle.CssClass = "GridItem"
    End Sub
#End Region

#Region "Create Template Column Action Grid"
    Private Sub createTemplateColumnsAction()
        Dim intCount As Integer
        Try
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
            'CType(ViewState("arrColumnsNameAction"), ArrayList).Add("ActType")
            CType(ViewState("arrColumnsNameAction"), ArrayList).Add("HrM")
            CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Hrs.")
            CType(ViewState("arrColumnsNameAction"), ArrayList).Add("ActionDate")
            'arrColumnsNameAction.Add("Action<u>O</u>wner")
            'arrColumnsNameAction.Add("Priority")
            CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Type")
            CType(ViewState("arrColumnsNameAction"), ArrayList).Add("Owner")

            CType(ViewState("arrWidthAction"), ArrayList).Clear()
            CType(ViewState("arrWidthAction"), ArrayList).Add(17)
            CType(ViewState("arrWidthAction"), ArrayList).Add(17)
            CType(ViewState("arrWidthAction"), ArrayList).Add(30)
            'arrWidthAction.Add(50)
            CType(ViewState("arrWidthAction"), ArrayList).Add(350)
            'CType(ViewState("arrWidthAction"), ArrayList).Add(80)
            CType(ViewState("arrWidthAction"), ArrayList).Add(30)
            CType(ViewState("arrWidthAction"), ArrayList).Add(40)
            CType(ViewState("arrWidthAction"), ArrayList).Add(140)
            'arrWidthAction.Add(60)
            'arrWidthAction.Add(45)

            CType(ViewState("arrWidthAction"), ArrayList).Add(40)
            CType(ViewState("arrWidthAction"), ArrayList).Add(70)

            grdAction.Width = Unit.Pixel(760)
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Clear()
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(0)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(1)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(2)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(3)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(4)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(5)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(6)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(7)))
            CType(ViewState("arrColumnsWidthAction"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthAction"), ArrayList)(8)))

            'tclAction(0) = New TemplateColumn
            'tclAction(0).Visible = False
            'tclAction(0).HeaderTemplate = New IONGrid.CreateItemTemplateSubmitButton("", "btn")
            'grdAction.Columns.Add(tclAction(0))

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
                tclAction(intCount + 1).ItemStyle.Width = CType(ViewState("arrColumnsWidthAction"), ArrayList)(intCount)    'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthAction(intCount))
                grdAction.Columns.Add(tclAction(intCount + 1))
            Next

        Catch ex As Exception
            CreateLog("TODO List", "CreateTemplateColumnsAction-2135", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try
    End Sub
#End Region

#Region "Create Action Query"
    Private Sub CreateDataTableAction()
        Dim dsAction As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32

        Try

            strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,AM_CH1_Mandatory,AM_FL8_Used_Hr, convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,  case AM_VC8_ActionType when 'External' then 'Ext' when 'Internal' then 'Int' end as AM_VC8_ActionType, UM_VC50_UserID  "
            strSql = strSql & " From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And AM_NU9_Task_Number=" & Val(ViewState("TaskNo"))
            strSql = strSql & " Order By AM_NU9_Action_Number"
            Call SQL.Search("T040031", "ToDoList", "CreateDataTableAction-1911", strSql, dsAction, "sachin", "Prashar")
            Dim strFilter As String = ""
            Dim strSearch As String = ""
            '-- Preparing Search String
            For intCount = 2 To dsAction.Tables(0).Columns.Count - 1
                strSearch = Request.Form("cpnlTaskAction$grdAction$ctl01$" + dsAction.Tables(0).Columns(intCount).ColumnName + "_H")
                If Not IsNothing(strSearch) Then
                    If Not strSearch.Trim.Equals("") Then
                        If dsAction.Tables(0).Columns(intCount).DataType.FullName = "System.Decimal" Or dsAction.Tables(0).Columns(intCount).DataType.FullName = "System.Int32" Or dsAction.Tables(0).Columns(intCount).DataType.FullName = "System.Int16" Or dsAction.Tables(0).Columns(intCount).DataType.FullName = "System.Double" Then
                            strSearch = strSearch.Replace("*", "")
                            If IsNumeric(strSearch) = False Then
                                strSearch = "-99999999999"
                            End If
                            strFilter = strFilter & dsAction.Tables(0).Columns(intCount).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            If strSearch.Contains("*") = True Then
                                strSearch = strSearch.Replace("*", "%")
                            Else
                                strSearch &= "%"
                            End If
                            strFilter = strFilter & dsAction.Tables(0).Columns(intCount).ColumnName & " like " & "'" & strSearch & "' AND "
                        End If
                    End If
                End If
            Next
            dtvAction = New DataView
            If Not strFilter.Trim.Equals("") Then
                strFilter = strFilter.Remove((strFilter.Length - 4), 4)
                dtvAction = GetFilteredDataView(dsAction.Tables(0).DefaultView, strFilter)
            Else
                dtvAction.Table = dsAction.Tables(0)
            End If

            Dim htDateCols As New Hashtable
            htDateCols.Add("AM_DT8_Action_Date", 1)
            If dtvAction.Table.Rows.Count > 0 Then
                Dim dtTemp As New DataTable
                dtTemp = dtvAction.Table
                SetDataTableDateFormat(dtTemp, htDateCols)
                dtvAction = New DataView
                dtvAction = dtTemp.DefaultView
            End If

            If dtvAction.Table.Rows.Count > 0 Then
                cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                cpnlTaskAction.TitleCSS = "test"
            Else
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskAction.TitleCSS = "test2"
            End If

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
                    mstGetFunctionValue = WSSSearch.SearchCompNameID(Val(ViewState("CompanyID")))
                    intComp = mstGetFunctionValue.ExtraValue
                End If
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & " Task# " & ViewState("TaskNo") & " Company:  " & intComp & ")"
            Else
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;"
            End If
            If Val(ViewState("TaskNo")) = 0 Then
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskAction.Enabled = False
                cpnlTaskAction.Text = "Action View &nbsp;&nbsp;"
            End If

            'code to make attachment yellow if action hae attachment
            If dsAction.Tables(0).Rows.Count = 0 Then
            Else
                If imgCallAttach.ImageUrl.Equals("../../Images/Attach_Yellow.gif") = False Then
                    For intI As Integer = 0 To dsAction.Tables(0).Rows.Count - 1
                        If dsAction.Tables(0).Rows(intI).Item("Blank2").Equals("1") = True Then
                            imgCallAttach.ImageUrl = "../../Images/Attach_Yellow.gif"
                            imgCallAttach.ToolTip = "View Attachments"
                            Exit For
                        Else
                            imgCallAttach.ImageUrl = "../../Images/Attach15_9.gif"
                            imgCallAttach.ToolTip = "No Attachments"
                        End If
                    Next
                End If

            End If

        Catch ex As Exception
            CreateLog("TODO List", "CreateDataTableAction-2199", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
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
                CType(ViewState("arrFooterTask"), ArrayList).Add(Request.Form("cpnlTaskAction$grdAction$ctl" & intFooterIndex.ToString.Trim & "$" + dtvAction.Table.Columns(intCount).ColumnName + "_F"))
            Next
        End If
    End Sub
#End Region

#Region "Bind Action Grid"
    Private Sub BindGridAction()
        Try

            If Request.Form("txtrowvaluescall") <> 0 Then
                introwvalues = Request.Form("txtrowvaluescall")
            End If
            Dim htDescCols As New Hashtable
            htDescCols.Add("AM_VC_2000_Description", 39)
            HTMLEncodeDecode(mdlMain.Action.Encode, dtvAction, htDescCols)
            SetCommentFlag(dtvAction, mdlMain.CommentLevel.ActionLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            grdAction.DataSource = dtvAction
            grdAction.DataBind()
            HTMLEncodeDecode(mdlMain.Action.Decode, dtvAction)

            'Add code to check whether action are mandatory for task or not
            mstGetFunctionValue = WSSSearch.SearchTasKMandatory(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
            If mstGetFunctionValue.ErrorCode = 0 Then
                'Action Not mandatory
                chkMandatoryHr.Checked = False
            Else
                'Action Mandatory
                chkMandatoryHr.Checked = True
            End If

        Catch ex As Exception
            CreateLog("TODO List", "BindGridAction-2242", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try
    End Sub
#End Region

    Private Sub grdAction_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAction.ItemDataBound

        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvAction
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim ActionNo As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvAction.ToTable()
        Dim strSelected As String
        dg.PageSize = 1

        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If grdAction.DataKeys(0) <> 0 Then
                    For intCount = 0 To 1       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        ActionNo = grdAction.DataKeys(e.Item.ItemIndex)

                        If strSelected = "1" Then      'If comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & intCount & "'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & intCount & "'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
                        Else       ' If no comment/attachment is attached
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & intCount & "'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
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
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString)
                            e.Item.Cells(intCount).ToolTip = HTMLEncodeDecode(mdlMain.Action.Decode, (IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString)))
                        End If
                        If dtvAction.Table.Columns(intCount).ColumnName.ToUpper.Equals("AM_DT8_Action_Date".ToUpper) Then
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Width = Unit.Pixel(76)
                        End If
                        ActionNo = grdAction.DataKeys(e.Item.ItemIndex)
                        Dim CallNo As Integer = ViewState("CallNo")
                        Dim TaskNo As Integer = ViewState("TaskNo")
                        Dim CompID As Integer = ViewState("CompanyID")

                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheckAction(" & TaskNo & "," & CallNo & ",'" & mActionRowValue + 1 & "','" & introwvalues & "', 'cpnlTaskAction_grdAction','" & CompID & "','" & ActionNo & "','" & txtTaskStatus.Value & "')")
                        e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & ActionNo & "',0, '" & mActionRowValue + 1 & "', 'cpnlTaskAction_grdAction')")
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Width = CType(ViewState("arrColumnsWidthAction"), ArrayList)(intCount)
                    Next
                    mActionRowValue += 1
                Else
                    For intCount = 0 To 1       'For Image Fields
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(1)
                    Next
                    For intCount = 2 To dtvAction.Table.Columns.Count - 1
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If
            If e.Item.ItemType = ListItemType.Header Then
                For intCol As Integer = 2 To dtvAction.Table.Columns.Count - 1
                    CType(e.Item.Cells(intCol).FindControl(dtvAction.Table.Columns(intCol).ToString & "_H"), TextBox).Width = CType(ViewState("arrColumnsWidthAction"), ArrayList)(intCol)
                    CType(e.Item.Cells(intCol).FindControl("lbl" & dtvAction.Table.Columns(intCol).ToString & "_H"), LinkButton).Text = "<BR>" & CType(e.Item.Cells(intCol).FindControl("lbl" & dtvAction.Table.Columns(intCol).ToString & "_H"), LinkButton).Text
                Next
            End If
        Catch ex As Exception
            CreateLog("TODO List", "ItemDataBound-2455", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try

    End Sub

#Region "Save Action Fast Entry"
    Private Function SaveAction() As Boolean
        '-----------------------------------------------
        'date 7/12/2006
        'to compare session values to stop f5 duplicate data while pressing f5 in data entry
        If Session("tdlupdate").ToString() = ViewState("update").ToString() Then '------------------------------

            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim shFlag As Short
            Dim intActionNo As Integer
            Dim blnCheckValidation As Boolean

            Try
                If dtActionDate.Text.Trim.Equals("") OrElse Txtdescription_F.Text.Trim.Equals("") Then
                    blnCheckValidation = False
                    Exit Function
                Else
                    blnCheckValidation = True
                End If
                'Security Block
                If imgSave.Enabled = False Or imgSave.Visible = False Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Your Role does not have rights to save Action...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                End If
                'End of Security Block
                lstError.Items.Clear()
                'Check call and task status
                '********************************************************************
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False
                Dim strchkcallstatus As String = SQL.Search("ToDoList", "SaveAction-2131", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")

                If IsNothing(strchkcallstatus) = False Then

                    lstError.Items.Clear()
                    lstError.Items.Add("Call Closed so You cannot Fill Action...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    Return False
                    Exit Function
                End If

                Dim strStatus As String
                ' Session("PropTaskNumber") = Request.Form("txtTask")
                strStatus = GetStatus(ViewState("CompanyID"), ViewState("CallNo"), mdlMain.StatusType.TaskStatus, ViewState("TaskNo"))
                If strStatus = "CLOSED" Then
                    lstError.Items.Clear()
                    Dim strTaskStatus As String
                    strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                    lstError.Items.Add("Task status is " & strTaskStatus & " so You cannot fill Action...")
                    lstError.Items.Add("You can change the task status to REOPEN and then add the Actions...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return False
                    Exit Function
                End If
                '**************************************************************************

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
                        SaveAction = False
                        lstError.Items.Clear()
                        lstError.Items.Add("Used hours are mandatory...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)

                        Exit Function
                    End If
                End If

                If blnCheckValidation = False Then    'Exit If all textbox are blank
                    SaveAction = False
                    Exit Function
                End If

                If CheckTaskDependency(ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo")) = True Then
                    TxtUsedHr_F.Text = ""
                    Txtdescription_F.Text = ""
                    lstError.Items.Clear()
                    lstError.Items.Add("Please check Task Dependency...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return False
                    Exit Function
                End If

                lstError.Items.Clear()

                If Txtdescription_F.Text.Trim.Equals("") Then
                    lstError.Items.Add("Description cannot be blank...")
                    shFlag = 1
                End If

                Dim dtTaskDate As Date = SQL.Search("ToDoList", "SaveAction-2186", "Select tm_DT8_Task_Date from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNo") & "")

                If dtActionDate.Text.Trim <> "" Then
                    If IsDate(dtActionDate.Text) = False Then
                        lstError.Items.Add("Check Action date format...")
                        shFlag = 1
                        'ElseIf CDate(dtActionDate.Text.Trim & " " & Now.ToShortDateString) < dtTaskDate Then
                        '    lstError.Items.Add("Action date cannot be less than Task Date...")
                        '    shFlag = 1
                        '''''''''''modified by tarun on 16 Jan 2010
                    ElseIf CDate(dtActionDate.Text.Trim) < dtTaskDate.ToString("yyyy-MMM-dd") Then
                        lstError.Items.Add("Action date cannot be less than Task Date...")
                        shFlag = 1

                    ElseIf CDate(dtActionDate.Text.Trim) > Now.ToString() Then
                        lstError.Items.Add("Action date cannot be more than current date...")
                        shFlag = 1
                    End If

                End If

                If shFlag = 1 Then
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    shFlag = 0
                    Return False
                    Exit Function
                End If

                lstError.Items.Clear()

                arrColumns.Add("AM_NU9_Call_Number")
                arrColumns.Add("AM_NU9_Task_Number")
                arrColumns.Add("AM_NU9_Comp_ID_FK")
                arrColumns.Add("AM_VC_2000_Description")
                arrColumns.Add("AM_CH1_Mandatory")
                arrColumns.Add("AM_FL8_Used_Hr")
                arrColumns.Add("AM_VC8_Supp_Owner")
                arrColumns.Add("AM_DT8_Action_Date_Auto")
                If Not dtActionDate.Text.Trim.Equals("") Then
                    arrColumns.Add("AM_DT8_Action_Date")
                End If
                arrColumns.Add("AM_NU9_Action_Number")
                arrColumns.Add("AM_VC8_ActionType")
                arrColumns.Add("AM_VC100_Action_type")
                arrRows.Add(ViewState("CallNo"))
                arrRows.Add(ViewState("TaskNo"))
                arrRows.Add(Val(ViewState("CompanyID")))
                arrRows.Add(Txtdescription_F.Text.Trim)
                If chkMandatoryHr.Checked = True Then
                    arrRows.Add("M")
                Else
                    arrRows.Add("O")
                End If
                arrRows.Add(Val(TxtUsedHr_F.Text.Trim))
                arrRows.Add(Session("PropUserID"))
                arrRows.Add(Now.Date)
                If Not dtActionDate.Text.Trim.Equals("") Then
                    arrRows.Add(dtActionDate.Text.Trim & " " & Now.ToShortTimeString)
                End If

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False

                intActionNo = SQL.Search("ToDoList", "SaveAction-2273", "select isnull(max(AM_NU9_Action_Number),0) from T040031 where AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and AM_NU9_Call_Number=" & ViewState("CallNo").ToString & " And AM_NU9_Task_Number=" & ViewState("TaskNo").ToString)
                intActionNo += 1

                arrRows.Add(intActionNo.ToString)

                If ChkActType.Checked = True Then
                    arrRows.Add("External")
                Else
                    arrRows.Add("Internal")
                End If
                arrRows.Add("")

                mstGetFunctionValue = WSSSave.SaveAction(arrColumns, arrRows, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"))
                If mstGetFunctionValue.ErrorCode = 0 Then
                    mstGetFunctionValue = WSSUpdate.UpdateTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                    If mstGetFunctionValue.ErrorCode = 0 Then
                        mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("CallNo"), False, ViewState("CompanyID"))

                        If mstGetFunctionValue.ErrorCode = 0 Then
                            lstError.Items.Clear()
                            '___________________________
                            'update session after saving data related to f5 problem
                            Session("tdlupdate") = Server.UrlEncode(System.DateTime.Now.ToString())
                            '_____________________________________________
                            lstError.Items.Clear()
                            lstError.Items.Add("Action Data Saved Successfully...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                            ViewState("ActionNo") = intActionNo
                            If GetFiles() = True Then
                            Else
                            End If
                            ClearAllTextBox(cpnlTaskAction)
                            garFileID.Clear()
                            '**************************************************************
                            'Refresh the Call information and Task Grid under call
                            dtgTask.Columns.Clear()
                            dtgTask.Controls.Clear()
                            Call FillCallDetail()
                            Call CreateDataTableTask()
                            Call CreateGridTask()
                            Call FillFooterArrayTask()
                            Call createTemplateColumnsTask()
                            Call BindGridTask()
                            '**************************************************************
                            SetControlFocus(Txtdescription_F)
                            Return True
                        End If

                    End If
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is unable to process your request please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

                    Return False
                End If
            Catch ex As Exception
                CreateLog("TODO List", "SaveAction-2286", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        Else
            ClearAllTextBox(cpnlTaskAction)
        End If
    End Function
#End Region

    Private Function GetFiles() As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim blnRead As Boolean
            ' For Tasks

            sqrdTempFiles = SQL.Search("ToDoList", "GetFiles-2330", "select * from T040041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(Session("PropCompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

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
            CreateLog("TODO List", "GetFiles-2316", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean

        Dim strPath As String = Server.MapPath("../../Dockyard")
        'Dim strPath As String = System.Configuration.ConfigurationSettings.AppSettings("WSSFilePath").ToString
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
                    Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim
                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, ViewState("CompanyID"), dblVersionNo, AttachLevel.ActionLevel) = True Then
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
                SQL.DBTracing = False
                dblVersionNo = SQL.Search("ToDoList", "CreateFolder-2408", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & "  and VH_NU9_Action_Number=" & ActionNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")
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
                Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim
                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, ViewState("CompanyID"), dblVersionNo, AttachLevel.ActionLevel) = True Then
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
            CreateLog("TODO List", "CreateFolder-2431", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    'Task close when close task image was press.
#Region "Close Task"

    Private Function UpdateTask() As Boolean
        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        lstError.Items.Clear()
        Dim strStatus As String
        strStatus = GetStatus(ViewState("CompanyID"), ViewState("CallNo"), mdlMain.StatusType.CallStatus)
        If strStatus = "CLOSED" Then
            lstError.Items.Clear()
            lstError.Items.Add("Call Closed so You cannot change the Task...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return False
            Exit Function
        End If
        'get the status of the task 
        Dim strTaskStatus As String
        strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
        If strTaskStatus = "CLOSED" Then
            lstError.Items.Clear()
            lstError.Items.Add("Task is already closed...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
        Else
            mstGetFunctionValue = WSSSearch.SearchAction(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
            If mstGetFunctionValue.ErrorCode = 0 Then
                Dim sqTaskRDR As SqlDataReader
                WSSSearch.SearchTask(ViewState("TaskNo"), ViewState("CallNo"), ViewState("CompanyID"), sqTaskRDR)
                If sqTaskRDR.HasRows Then
                    arrColumns.Add("TM_VC1000_Subtsk_Desc")
                    arrColumns.Add("TM_VC8_task_type")
                    arrColumns.Add("TM_FL8_Est_Hr")
                    arrColumns.Add("TM_VC8_Supp_Owner")
                    arrColumns.Add("TM_NU9_Assign_by")
                    arrColumns.Add("TM_VC8_Priority")
                    arrColumns.Add("TM_NU9_Agmt_No")
                    arrColumns.Add("TM_NU9_Dependency")
                    arrColumns.Add("TM_CH1_Mandatory")
                    arrColumns.Add("TM_DT8_Est_close_date")
                    arrColumns.Add("TM_DT8_Close_Date")
                    arrColumns.Add("TM_DT8_Task_Close_Date")
                    arrColumns.Add("TM_CH1_Invoice_Pending")
                    arrColumns.Add("TM_CH1_Forms")
                    arrColumns.Add("TM_DT8_Task_Date")

                    sqTaskRDR.Read()
                    arrRows.Add(sqTaskRDR("TM_VC1000_Subtsk_Desc"))
                    arrRows.Add(sqTaskRDR("TM_VC8_task_type"))
                    arrRows.Add(sqTaskRDR("TM_FL8_Est_Hr"))
                    arrRows.Add(sqTaskRDR("TM_VC8_Supp_Owner"))
                    arrRows.Add(sqTaskRDR("TM_NU9_Assign_by"))
                    arrRows.Add(sqTaskRDR("TM_VC8_Priority"))
                    arrRows.Add(sqTaskRDR("TM_NU9_Agmt_No"))
                    arrRows.Add(sqTaskRDR("TM_NU9_Dependency"))
                    arrRows.Add(sqTaskRDR("TM_CH1_Mandatory"))
                    arrRows.Add(sqTaskRDR("TM_DT8_Est_close_date"))
                    arrRows.Add(Now)
                    arrRows.Add(Now)
                    arrRows.Add(sqTaskRDR("TM_CH1_Invoice_Pending"))
                    arrRows.Add(sqTaskRDR("TM_CH1_Forms"))
                    arrRows.Add(sqTaskRDR("TM_DT8_Task_Date"))
                    sqTaskRDR.Close()
                End If

                arrColumns.Add("TM_VC50_Deve_status")
                arrRows.Add("CLOSED")
                mstGetFunctionValue = WSSUpdate.UpdateTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"), arrColumns, arrRows, True)
                If mstGetFunctionValue.ErrorCode = 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Task Closed Successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    ViewState("Flag") = 1 'set viewstate("Flag") value if call is closed 
                    If CreateTaskAutoAction(ViewState("tdlTaskStatus"), "CLOSED", ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID")) = True Then
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Errors occur please try later...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    End If
                    ViewState("TaskNo") = 0
                    ViewState("CallNo") = 0
                    mstrTaskNumber = "0"
                    mstrCallNumber = "0"
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Errors occur please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Task must have at least one Action with Mandatory status with Hours before closing it...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
        End If
    End Function
#End Region

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        Try
            lstError.Items.Add(ErrMsg)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
        Catch ex As Exception
            CreateLog("TODO List", "DisplayError-2761", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        Try
            lstError.Items.Add(Msg)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception
            CreateLog("TODO List", "DisplayMessage-2774", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Clear TextBoxes based on panels"
    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        Dim objTextBox As Control
        Dim objCtl As Control
        Try
            For Each objCtl In CPnl.Controls
                If TypeOf objCtl Is TextBox Then
                    CType(objCtl, TextBox).Text = ""
                End If

                If TypeOf objCtl Is Panel Then
                    For Each objTextBox In objCtl.Controls
                        If TypeOf objTextBox Is TextBox Then
                            CType(objTextBox, TextBox).Text = ""
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            CreateLog("TODO List", "CreateFolder-2800", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        'dtActionDate.CalendarDate = ""
        Txtdescription_F.Height = Unit.Pixel(18)
    End Sub
#End Region

#Region "Refresh Grid With no selection"
    Private Sub RefreshSelection()
        ViewState("CallNo") = -1    '//For refreshing seleted callnumber
        mstrCallNumber = -1
        ViewState("TaskNo") = 1
        Call CreateDataTableAction()
        Call BindGridAction()
    End Sub
#End Region

    Private Sub ImgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgClose.Click
        Response.Redirect("../../home.aspx", False)
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

    Private Sub ddlstview_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged
        'Session("TDLViewName") = ddlstview.SelectedItem.Text
        'Session("TDLViewValue") = ddlstview.SelectedValue
        'SaveUserView()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'date 7/12/2006 
        '-----------------------------------------------
        'to store current viewstate value in session to stop f5 duplicate data while pressing f5 in data entry
        ViewState("update") = Session("tdlupdate")
        '-----------------------------------------------
    End Sub

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        GrdAddSerach.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber

        If ddlstview.SelectedValue = 0 Then
            fillDefault()
        Else
            Fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(Session("SortOrder")) = False Then
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
            fillDefault()
        Else
            Fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If

        If IsNothing(Session("SortOrder")) = False Then
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
            fillDefault()
        Else
            Fillview()
        End If

        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If

        If IsNothing(Session("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        GrdAddSerach.CurrentPageIndex = (GrdAddSerach.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber

        If ddlstview.SelectedValue = 0 Then
            fillDefault()
        Else
            Fillview()
        End If

        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If

        If IsNothing(Session("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub

    Private Sub SaveUserView()
        Dim intid = 502
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
                'Save message
            Else
                'Error message
            End If
        End If
    End Sub

    Private Sub ChkSelectedView()
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=502 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

        If blnReturn = False Then
            Session("TDLViewName") = "Default"
            Session("TDLViewValue") = "0"
            Exit Sub
        Else
            While sqdrCol.Read
                Session("TDLViewName") = sqdrCol.Item("UV_VC50_View_Name")
                Session("TDLViewValue") = sqdrCol.Item("UV_IN4_View_ID")

                ddlstview.SelectedValue = Session("TDLViewValue")

            End While
            sqdrCol.Close()
        End If
    End Sub

    Private Sub CreateGridTask()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        Try
            dtgTask.ID = "dtgTask"
            dtgTask.DataKeyField = "TM_NU9_Task_no_PK"
            Call FormatGridTask()

            Placeholder3.Controls.Add(dtgTask)
        Catch ex As Exception
            CreateLog("Call-Detail", "CreateGridTask-1916", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub

    Private Sub createTemplateColumnsTask()
        Dim intCount As Integer
        Try

            ReDim tclTask(dtvTask.Table.Columns.Count)

            arrImageUrlEnabled.Clear()
            arrImageUrlEnabled.Add("../../Images/comment2.gif")
            arrImageUrlEnabled.Add("../../Images/attach15_9.gif")
            arrImageUrlEnabled.Add("../../Images/Form1.jpg")

            arrImageUrlDisabled.Clear()
            arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
            arrImageUrlDisabled.Add("../../Images/white.gif")
            arrImageUrlDisabled.Add("../../Images/white.gif")

            arrImageUrlNew.Clear()
            arrImageUrlNew.Add("../../Images/comment_Unread.gif")
            arrImageUrlNew.Add("../../Images/white.gif")
            arrImageUrlNew.Add("../../Images/Form2.gif")

            CType(ViewState("arrColumnsNameTask"), ArrayList).Clear()
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("C")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("A")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("F")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("TO")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("ID")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("Stat")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("Subject")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("TType")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("Ownr")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("Dep")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("EstClDate")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("EHr")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("AcM")
            CType(ViewState("arrColumnsNameTask"), ArrayList).Add("Prio")

            CType(ViewState("arrWidthTask"), ArrayList).Clear()
            CType(ViewState("arrWidthTask"), ArrayList).Add(17)
            CType(ViewState("arrWidthTask"), ArrayList).Add(17)
            CType(ViewState("arrWidthTask"), ArrayList).Add(17)
            CType(ViewState("arrWidthTask"), ArrayList).Add(17)
            CType(ViewState("arrWidthTask"), ArrayList).Add(17)
            CType(ViewState("arrWidthTask"), ArrayList).Add(70)
            CType(ViewState("arrWidthTask"), ArrayList).Add(190)
            CType(ViewState("arrWidthTask"), ArrayList).Add(64)
            CType(ViewState("arrWidthTask"), ArrayList).Add(72)
            CType(ViewState("arrWidthTask"), ArrayList).Add(40)
            CType(ViewState("arrWidthTask"), ArrayList).Add(80)
            CType(ViewState("arrWidthTask"), ArrayList).Add(33)
            CType(ViewState("arrWidthTask"), ArrayList).Add(24)
            CType(ViewState("arrWidthTask"), ArrayList).Add(57)

            dtgTask.Width = Unit.Pixel(757)

            CType(ViewState("arrColumnsWidthTask"), ArrayList).Clear()
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(0)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(1)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(2)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(3)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(4)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(5)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(6)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(7)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(8)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(9)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(10)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(11)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(12)))
            CType(ViewState("arrColumnsWidthTask"), ArrayList).Add(System.Web.UI.WebControls.Unit.Pixel(CType(ViewState("arrWidthTask"), ArrayList)(13)))

            tclTask(0) = New TemplateColumn
            tclTask(0).Visible = True
            tclTask(0).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(0).ToString, "", dtvTask.Table.Columns(0).ToString + "_H", False, CType(ViewState("arrColumnsNameTask"), ArrayList)(0), False)
            tclTask(0).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(0).ToString, arrImageUrlDisabled(0))
            tclTask(0).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclTask(0).ItemStyle.Width = CType(ViewState("arrColumnsWidthTask"), ArrayList)(0)
            dtgTask.Columns.Add(tclTask(0))

            tclTask(1) = New TemplateColumn
            tclTask(1).Visible = True
            tclTask(1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(1).ToString, "", dtvTask.Table.Columns(1).ToString + "_H", False, CType(ViewState("arrColumnsNameTask"), ArrayList)(1), False)
            tclTask(1).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(1).ToString, arrImageUrlDisabled(1))
            tclTask(1).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclTask(1).ItemStyle.Width = CType(ViewState("arrColumnsWidthTask"), ArrayList)(1)
            dtgTask.Columns.Add(tclTask(1))

            tclTask(2) = New TemplateColumn
            tclTask(2).Visible = True
            tclTask(2).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(2).ToString, "", dtvTask.Table.Columns(2).ToString + "_H", False, CType(ViewState("arrColumnsNameTask"), ArrayList)(2), False)
            tclTask(2).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(2).ToString, arrImageUrlDisabled(1))
            tclTask(2).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclTask(2).ItemStyle.Width = CType(ViewState("arrColumnsWidthTask"), ArrayList)(2)
            dtgTask.Columns.Add(tclTask(2))

            Dim maxchar() As Int16 = {-1, -1, -1, 3, 3, 7, 20, 7, 8, 3, 12, 4, 1, 5, 2, 5, 5} 'Variable to store MaxLength of TextBoxes

            For intCount = 3 To dtvTask.Table.Columns.Count - 1
                tclTask(intCount + 1) = New TemplateColumn
                tclTask(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvTask.Table.Columns(intCount).ToString, dtvTask.Table.Columns(intCount).ToString)
                Dim AddEventOnGrigHeader As New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(intCount).ToString, "", dtvTask.Table.Columns(intCount).ToString + "_H", False, CType(ViewState("arrColumnsNameTask"), ArrayList)(intCount).ToString, True, maxchar(intCount))
                AddHandler AddEventOnGrigHeader.OnSort, AddressOf SortGridTask
                tclTask(intCount + 1).HeaderTemplate = AddEventOnGrigHeader
                tclTask(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvTask.Table.Columns(intCount).ToString + "_F", False)
                tclTask(intCount + 1).ItemStyle.Width = CType(ViewState("arrColumnsWidthTask"), ArrayList)(intCount)    'System.Web.UI.WebControls.Unit..Pixel(arrColumnsWidthTask(intCount))
                dtgTask.Columns.Add(tclTask(intCount + 1))
            Next
        Catch ex As Exception
            lstError.Items.Add("Unexpected Error..")
            CreateLog("Call-Detail", "CreateTemplateColumnsTask-1998", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub

    Private Sub CreateDataTableTask()
        '-- FinalWhere will tell whether whereclause is to be checked or not
        Dim dsTask As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32

        Try
            strSql = "select TM_CH1_Comment as Blank1,TM_CH1_Attachment as Blank2,TM_CH1_Forms as TaskBlank3,TM_NU9_Task_Order ,TM_NU9_Task_no_PK, TM_VC50_Deve_Status,TM_VC1000_Subtsk_Desc,  TM_VC8_task_type,b.UM_VC50_UserID+'~'+convert(varchar(8),a.TM_VC8_Supp_Owner) as UM_VC50_UserID, TM_NU9_Dependency,convert(varchar,TM_DT8_Est_close_date) TM_DT8_Est_close_date, TM_FL8_Est_Hr,TM_CH1_Mandatory,  TM_VC8_Priority  From T040021 a, T060011 b Where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK= a.TM_VC8_Supp_Owner "

            'strSql = strSql & " Order By TM_NU9_Task_no_PK asc"
            strSql = strSql & " Order By TM_NU9_Task_Order asc"

            Call SQL.Search("T040021", "Call_Detail", "CreateDataTableTask-1803", strSql, dsTask, "sachin", "Prashar")
            '-- Preparing Search  String (strFilter) for Task Grid
            Dim strSearchTask As String = ""
            Dim strFilterTask As String = ""
            For intCount = 3 To dsTask.Tables(0).Columns.Count - 1
                strSearchTask = Request.Form("cpnlCallTask$cpnlTaskList$dtgTask$ctl01$" + dsTask.Tables(0).Columns(intCount).ColumnName + "_H")
                If IsNothing(strSearchTask) = False Then
                    If Not IsDBNull(strSearchTask) Then
                        If Not strSearchTask.Trim.Equals("") Then
                            strSearchTask = mdlMain.GetSearchString(strSearchTask)
                            If dsTask.Tables(0).Columns(intCount).DataType.FullName = "System.Decimal" Or dsTask.Tables(0).Columns(intCount).DataType.FullName = "System.Int32" Or dsTask.Tables(0).Columns(intCount).DataType.FullName = "System.Int16" Or dsTask.Tables(0).Columns(intCount).DataType.FullName = "System.Double" Then
                                strSearchTask = strSearchTask.Replace("*", "")
                                If IsNumeric(strSearchTask) = False Then
                                    strSearchTask = -9999999999999
                                End If
                                strFilterTask = strFilterTask & dsTask.Tables(0).Columns(intCount).ColumnName & "='" & strSearchTask & "' AND "
                            Else
                                If strSearchTask.Contains("*") = True Then
                                    strSearchTask = strSearchTask.Replace("*", "%")
                                Else
                                    strSearchTask &= "%"
                                End If
                                strFilterTask = strFilterTask & dsTask.Tables(0).Columns(intCount).ColumnName & " like '" & strSearchTask & "' AND "
                            End If
                        End If
                    End If
                End If
            Next
            dtvTask = New DataView
            If Not strFilterTask.Trim.Equals("") Then
                strFilterTask = strFilterTask.Remove((strFilterTask.Length - 4), 4)
                dtvTask = GetFilteredDataView(dsTask.Tables(0).DefaultView, strFilterTask)
            Else
                dtvTask.Table = dsTask.Tables(0)
            End If
            Dim htDateCols As New Hashtable
            htDateCols.Add("TM_DT8_Est_close_date", 2)
            If dtvTask.Table.Rows.Count > 0 Then
                Dim dtTemp As New DataTable
                dtTemp = dtvTask.Table
                SetDataTableDateFormat(dtTemp, htDateCols)
                dtvTask = New DataView
                dtvTask = dtTemp.DefaultView
            End If

            '===============================
            If dtvTask.Table.Rows.Count = 0 Then
                rowTemp = dtvTask.Table.NewRow
                For intCount = 0 To dtvTask.Table.Columns.Count - 1
                    Select Case dtvTask.Table.Columns(intCount).DataType.FullName
                        Case "System.String"
                            rowTemp.Item(intCount) = " "
                        Case "System.Decimal", "System.Double", "System.Int32", "System.Int16"
                            rowTemp.Item(intCount) = 0
                        Case "System.DateTime"
                    End Select
                Next
                dtvTask.Table.Rows.Add(rowTemp)
            End If
            '===============================
            'Code to attchment Yellow 
            If dsTask.Tables(0).Rows.Count = 0 Then
            Else
                If imgCallAttach.ImageUrl.Equals("../../Images/Attach_Yellow.gif") = False Then
                    For intI As Integer = 0 To dsTask.Tables(0).Rows.Count - 1
                        If dsTask.Tables(0).Rows(intI).Item("Blank2").Equals("1") = True Then
                            imgCallAttach.ImageUrl = "../../Images/Attach_Yellow.gif"
                            imgCallAttach.ToolTip = "View Attachments"
                            Exit For
                        Else
                            imgCallAttach.ImageUrl = "../../Images/Attach15_9.gif"
                            imgCallAttach.ToolTip = "No Attachments"
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("Call-Detail", "CreateDataTableTask-1750", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
        '===============================
    End Sub

    Private Function isTaskInGrid(ByVal taskNumber As Integer) As Boolean
        Dim i As Integer
        Dim dtvTemp As DataView
        dtvTemp = dtvTask.Table.DefaultView
        For i = 0 To dtvTemp.Count - 1
            If dtvTemp.Item(i).Item("TM_NU9_Task_no_PK") = taskNumber Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function

    Private Sub FormatGridTask()
        dtgTask.AutoGenerateColumns = False
        dtgTask.AllowPaging = True
        '  dtgTask.ShowFooter = True
        dtgTask.ShowHeader = True
        dtgTask.HeaderStyle.CssClass = "GridHeader"
        dtgTask.HeaderStyle.Height = System.Web.UI.WebControls.Unit.Pixel(1)
        dtgTask.Width = System.Web.UI.WebControls.Unit.Percentage(100)
        dtgTask.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
        dtgTask.BorderStyle = BorderStyle.None
        dtgTask.CellPadding = 1
        dtgTask.AllowPaging = False
        dtgTask.CssClass = "Grid"
        dtgTask.HorizontalAlign = HorizontalAlign.Left
        'dtgTask.FooterStyle.CssClass = "GridFixedFooter"
        dtgTask.SelectedItemStyle.CssClass = "GridSelectedItem"
        dtgTask.AlternatingItemStyle.CssClass = "GridAlternateItem"
        dtgTask.ItemStyle.CssClass = "GridItem"
    End Sub

    Private Sub BindGridTask()
        Try
            Dim htDescCols As New Hashtable
            htDescCols.Add("TM_VC1000_Subtsk_Desc", 22)
            HTMLEncodeDecode(mdlMain.Action.Encode, dtvTask, htDescCols)
            SetCommentFlag(dtvTask, mdlMain.CommentLevel.TaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
        Catch Ex As Exception
            CreateLog("Call_Detail", "BindGridTask-2122", LogType.Application, LogSubType.Exception, Ex.TargetSite.Attributes, Ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub FillFooterArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        Try
            CType(ViewState("arrFooterTask"), ArrayList).Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvTask.Table.Columns.Count - 1
                    intFooterIndex = dtvTask.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                    CType(ViewState("arrFooterTask"), ArrayList).Add(Request.Form("cpnlCallTask$cpnlTaskList$dtgTask$ctl" & intFooterIndex.ToString.Trim & "$" + dtvTask.Table.Columns(intCount).ColumnName + "_F"))
                Next
            End If
        Catch ex As Exception
            CreateLog("Call_Detail", "FillFooterArrayTask-2155", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub dtgTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTask.ItemDataBound
        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvTask
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvTask.ToTable()
        Dim strSelected As String
        Dim structTempTaskOwner As Owners '-- will keep taskowners ID and Name
        dg.PageSize = 1
        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dtgTask.DataKeys(0) <> 0 Then
                    For intCount = 0 To 2       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        strID = dtgTask.DataKeys(e.Item.ItemIndex)
                        If strSelected = "1" Then      'If comment/ attachment is there 

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImageTask(" & ViewState("CompanyID") & "," & ViewState("CallNo") & "," & e.Item.ItemIndex + 1 & ",0,'T'," & intCount & ")")

                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImageTask(" & ViewState("CompanyID") & "," & ViewState("CallNo") & "," & e.Item.ItemIndex + 1 & ",0" & ViewState("ActionNo") & ",'T'," & intCount & ")")

                        Else       ' If no comment/attachment is attached

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImageTask(" & ViewState("CompanyID") & "," & ViewState("CallNo") & "," & e.Item.ItemIndex + 1 & ",0,'T','" & intCount & "')")

                        End If
                    Next

                    For intCount = 3 To dtvTask.Table.Columns.Count - 1       'for Others

                        If dtvTask.Table.Columns(intCount).DataType.FullName.Equals("System.DateTime") Then

                            If dtBound.Rows(cnt)(intCount).ToString Is null Or dtBound.Rows(cnt)(intCount).ToString = "" Then
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = " "
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = CType(dtBound.Rows(cnt)(intCount).ToString, DateTime).ToShortDateString
                            End If
                        Else
                            If intCount = 8 Then ' for task owner
                                structTempTaskOwner.Id = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(1)
                                structTempTaskOwner.Name = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = structTempTaskOwner.Name
                                'Tootip should have full value
                                e.Item.Cells(intCount).ToolTip = HTMLEncodeDecode(mdlMain.Action.Decode, structTempTaskOwner.Name)
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString)
                                'Tootip should have full value
                                e.Item.Cells(intCount).ToolTip = HTMLEncodeDecode(mdlMain.Action.Decode, IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString))
                            End If
                        End If

                        strID = dtgTask.DataKeys(e.Item.ItemIndex)
                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheckTask(" & e.Item.ItemIndex + 1 & ", 'cpnlCallTask_cpnlTaskList_dtgTask','" & strID & "')")
                        If intCount = 8 Then 'for task owner
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & structTempTaskOwner.Id & "')")
                        End If
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Width = CType(ViewState("arrColumnsWidthTask"), ArrayList)(intCount)
                    Next
                Else
                    For intCount = 0 To 1       'For Image Fields
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(1)
                    Next
                    For intCount = 3 To dtvTask.Table.Columns.Count - 2
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("Call-Detail", "ItmDataBound-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try

    End Sub

    Private Function FillCallDetail() As Boolean
        Try
            Dim sqrdCall As SqlDataReader
            mstGetFunctionValue = WSSSearch.SearchCall(ViewState("CallNo"), ViewState("CompanyID"), sqrdCall)
            While sqrdCall.Read
                txtCallNumber.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Call_No_PK")), 0, sqrdCall.Item("CM_NU9_Call_No_PK"))
                txtCallEnteredBy.Text = WSSSearch.SearchUserID(Val(sqrdCall.Item("CM_VC100_By_Whom"))).ExtraValue
                'Add new to get ID of call entered by for clicking on pic
                txtCallBy.Text = Val(sqrdCall.Item("CM_VC100_By_Whom"))

                txtrequestedBy.Text = WSSSearch.SearchUserID(Val(sqrdCall.Item("CM_NU9_Call_Owner"))).ExtraValue
                'Add new to get ID of CallOwner for clicking on pic
                CDDLCallOwner.Text = Val(sqrdCall.Item("CM_NU9_Call_Owner"))

                txtCallType.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Call_Type")), "", sqrdCall.Item("CM_VC8_Call_Type"))
                txtCallReportedHours.Text = IIf(IsDBNull(sqrdCall.Item("CM_FL8_Total_Reported_Time")), "", sqrdCall.Item("CM_FL8_Total_Reported_Time"))
                txtCallEstHours.Text = IIf(IsDBNull(sqrdCall.Item("CM_FL8_Total_Est_Time")), "", sqrdCall.Item("CM_FL8_Total_Est_Time"))
                txtCauseCode.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Cause_Code")), "", sqrdCall.Item("CM_VC8_Cause_Code"))
                txtCoordinator.Text = IIf(IsDBNull(sqrdCall.Item("Coordinator")), "", sqrdCall.Item("Coordinator"))
                'add new to get ID of coordinator
                txtCordinator.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Coordinator")), "", sqrdCall.Item("CM_NU9_Coordinator"))

                txtCategory.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Category")), "", sqrdCall.Item("CM_VC8_Category"))
                txtrelatedCall.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Related_Call")), "", sqrdCall.Item("CM_NU9_Related_Call"))
                mstGetFunctionValue = WSSSearch.SearchCompNameID(sqrdCall.Item("CM_NU9_Comp_Id_FK"))
                txtCustomer.Text = mstGetFunctionValue.ExtraValue
                txtCallDescription.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC2000_Call_Desc")), "", sqrdCall.Item("CM_VC2000_Call_Desc"))
                txtPriority.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC200_Work_Priority")), "", sqrdCall.Item("CM_VC200_Work_Priority"))
                WSSSearch.SearchProjectName(sqrdCall.Item("CM_NU9_Project_ID"), Val(ViewState("CompanyID")))
                txtProject.Text = WSSSearch.SearchProjectName(sqrdCall.Item("CM_NU9_Project_ID"), Val(ViewState("CompanyID")))
                txtreference.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC50_Reference_Id")), "", sqrdCall.Item("CM_VC50_Reference_Id"))
                txtSubject.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC100_Subject")), "", sqrdCall.Item("CM_VC100_Subject"))
                txtEstCloseDate.Text = (IIf(IsDBNull(sqrdCall.Item("CM_DT8_Close_Date")), "", sqrdCall.Item("CM_DT8_Close_Date")))
                If Not txtEstCloseDate.Text.Equals("") Then
                    txtEstCloseDate.Text = IIf(txtEstCloseDate.Text.Equals(""), "", SetDateFormat(CDate(txtEstCloseDate.Text), mdlMain.IsTime.DateOnly))
                End If
                txtCallStatus.Text = IIf(IsDBNull(sqrdCall.Item("CN_VC20_Call_Status")), "", sqrdCall.Item("CN_VC20_Call_Status"))
                txtAgreement.Text = IIf(sqrdCall.Item("CM_NU9_Agreement") = 0, "", sqrdCall.Item("CM_NU9_Agreement"))

                txtTemplateName.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Template")), "", sqrdCall.Item("CM_VC8_Template"))

                If IsDBNull(sqrdCall.Item("CM_VC8_Tmpl_Type")) = True Then
                    txtTemplateType.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Tmpl_Type")), "", sqrdCall.Item("CM_VC8_Tmpl_Type"))
                Else
                    txtTemplateType.Text = IIf(sqrdCall.Item("CM_VC8_Tmpl_Type") = "", "", sqrdCall.Item("CM_VC8_Tmpl_Type"))
                End If
                txtCallDate.Text = SetDateFormat(CDate(IIf(IsDBNull(sqrdCall.Item("CM_DT8_Request_Date")), "", sqrdCall.Item("CM_DT8_Request_Date"))), mdlMain.IsTime.WithTime)
                'Call start date
                If Not CStr(IIf(IsDBNull(sqrdCall.Item("CM_DT8_Call_Start_Date")), "", sqrdCall.Item("CM_DT8_Call_Start_Date"))).Equals("") Then
                    dtCallStartDate.DbSelectedDate = IIf(IsDBNull(sqrdCall.Item("CM_DT8_Call_Start_Date")), "", sqrdCall.Item("CM_DT8_Call_Start_Date"))
                    dtCallStartDate.DateInput.DisplayDateFormat = "yyyy-MMM-dd HH:mm tt"
                Else
                    dtCallStartDate.DbSelectedDate = System.DBNull.Value
                End If

                Dim intCommentFlag As Int16
                SetCommentFlag(intCommentFlag, mdlMain.CommentLevel.CallLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                Select Case intCommentFlag
                    Case 0
                        imgComment.ImageUrl = "..\..\Images\comment_Blank.gif"
                    Case 1
                        imgComment.ImageUrl = "..\..\Images\comment2.gif"
                    Case 2
                        imgComment.ImageUrl = "..\..\Images\comment_Unread.gif"
                End Select
                imgComment.Attributes.Add("onclick", "return KeyImageTask(" & ViewState("CompanyID") & "," & ViewState("CallNo") & ",0,0,'C','0')")
                Dim intAttachF As Integer = IIf(IsDBNull(sqrdCall.Item("CM_NU8_Attach_No")), 0, sqrdCall.Item("CM_NU8_Attach_No"))

                If IsDBNull(sqrdCall.Item("CM_NU8_Attach_No")) = False Then
                    imgCallAttach.ImageUrl = "../../Images/Attach_Yellow.gif"
                Else
                    imgCallAttach.ImageUrl = "../../Images/Attach15_9.gif"
                End If
                If intAttachF > 0 Then
                    imgCallAttach.Attributes.Add("OnClick", "return CallAttach(1);")
                    imgCallAttach.ToolTip = "View Attachments"
                Else
                    imgCallAttach.Attributes.Add("OnClick", "return CallAttach(0);")
                    imgCallAttach.ToolTip = "No Attachment"
                End If
            End While
            sqrdCall.Close()
            cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
        Catch ex As Exception
            CreateLog("ToDoList", "FillCallDetail-4238", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try

    End Function

    Private Sub GridRowSelection()

        '*****************************************************************************
        Dim dgi As DataGridItem
        If compIdColumnNo <> "" Or taskNoColumnNo <> "" Or callNoColumnNo <> -1 Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(compIdColumnNo + 3).Text.Trim = Session("CompName") And Val(dgi.Cells(callNoColumnNo + 3).Text.Trim) = Val(ViewState("CallNo")) And Val(dgi.Cells(taskNoColumnNo + 3).Text.Trim) = Val(ViewState("TaskNo")) Then

                    cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskAction.Enabled = True
                    cpnlTaskAction.TitleCSS = "test"
                    cpnlTaskAction.Text = "Action View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & " Task# " & ViewState("TaskNo") & " Company:  " & intComp & ")"

                    If strPanelState.ToUpper.Equals("EXPANDED") Then
                        cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                    Else
                        cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                    End If


                    cpnlCallTask.Text = "Call Detail (Call # " & Val(ViewState("CallNo")) & " Company: " & intComp & ")"
                    cpnlTaskList.Text = "Task List (Call # " & Val(ViewState("CallNo")) & " Company: " & intComp & ")"
                    cpnlCallTask.TitleCSS = "test"
                    cpnlCallTask.Enabled = True
                    Exit For
                Else
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskAction.Enabled = False
                    cpnlTaskAction.TitleCSS = "test2"
                    cpnlTaskAction.Text = "Action View"

                    cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallTask.Enabled = False
                    cpnlCallTask.TitleCSS = "test2"
                    cpnlCallTask.Text = "Call Detail"
                End If
            Next
        Else
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View"

            cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            cpnlCallTask.Enabled = False
            cpnlCallTask.TitleCSS = "test2"
            cpnlCallTask.Text = "Call Detail"
        End If

        If GrdAddSerach.Items.Count = 0 Then
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View"
            CurrentPg.Text = 0

            cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            cpnlCallTask.Enabled = False
            cpnlCallTask.TitleCSS = "test2"
            cpnlCallTask.Text = "Call Detail"
        End If

    End Sub

    Private Sub GrdAddSerach_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddSerach.SortCommand
        Session("tdlTaskSortOrder") = e.SortExpression
        SortGRD()
    End Sub

    Private Sub SortGRD()
        ' If SortWay Mod 2 = 0 Then
        If Val(Session("tdlTaskSortWay")) Mod 2 = 0 Then
            mdvtable.Sort = Session("tdlTaskSortOrder") & " ASC"
        Else
            mdvtable.Sort = Session("tdlTaskSortOrder") & " DESC"
        End If
        '   SortWay += 1
        Session("tdlTaskSortWay") += 1
        If GrdAddSerach.AutoGenerateColumns = False Then
            GrdAddSerach.AutoGenerateColumns = True
        End If
        rowvalue = 0
        GrdAddSerach.DataSource = mdvtable
        GrdAddSerach.DataBind()
        GridRowSelection()
    End Sub

    Private Sub SortGRDDuplicate()
        Try
            ' If SortWay Mod 2 = 0 Then
            If Val(Session("tdlTaskSortWay")) Mod 2 = 0 Then
                mdvtable.Sort = Session("tdlTaskSortOrder") & " DESC"
            Else
                mdvtable.Sort = Session("tdlTaskSortOrder") & " ASC"
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

    Private Function GetCallPriority(ByVal callno As String, ByVal CompName As String) As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim intRows As Integer
            Dim SQLQuery As String

            SQLQuery = " select * from T040011 where CM_NU9_Call_No_PK=" & callno & "  and CM_NU9_Comp_Id_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & CompName & "' ) and CM_VC200_Work_Priority='1' "

            SQL.Search("Call_View", "GetCallPriority-4157", SQLQuery, intRows)
            If intRows > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("todolisr", "GetCallPriority-4164", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

    Private Sub SavePageSize()
        Dim intid = 502
        Dim strCheck As String = SQL.Search("Historicview", "SavePageSize-3406", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "")
        If Not IsNothing(strCheck) Then
            'update
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            arColumnName.Add("PS_NU9_PSize")
            arRowData.Add(Val(Session("tdlPageSize")))
            If SQL.Update("T030214", "SavePageSIZE", "update  T030214 set PS_NU9_PSize=" & Val(Session("tdlPageSize")) & "  where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid"), SQL.Transaction.Serializable) = True Then
                'Save message
            Else
                'Error message
            End If
        Else
            'save
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("PS_NU9_PSize")
            arColumnName.Add("PS_NU9_ScreenID")
            arColumnName.Add("PS_NU9_RoleID")
            arColumnName.Add("PS_NU9_ComID")
            arColumnName.Add("PS_NU9_UserID") 'Added new field to store user id with view records

            arRowData.Add(Val(Session("tdlPageSize")))
            arRowData.Add(intid)
            arRowData.Add(Session("PropRole"))
            arRowData.Add(Session("PropCompanyID"))
            arRowData.Add(Session("PropUserID"))

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Save("T030214", "SaveUserView", "SaveUserView-3436", arColumnName, arRowData) = True Then
                'Save message
            Else
                'Error message
            End If
        End If
    End Sub

    Private Function ChkPageView() As Boolean
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=502 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)
            If blnReturn = False Then
                Return False
                Exit Function
            Else
                While sqdrCol.Read
                    Session("tdlPageSize") = sqdrCol.Item("PS_NU9_PSize")
                End While
                Return True
            End If
            sqdrCol.Close()
            sqdrCol = Nothing

        Catch ex As Exception
            ddlstview.SelectedValue = 0
            CreateLog("Task_View", "ChkSelectedView-2080", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

    Protected Sub SortGridTask(ByVal sender As Object, ByVal e As CommandEventArgs)
        Session("tdlCallTaskOrderTask") = e.CommandArgument
        SortGRDTask()
    End Sub

    Protected Sub SortGridAction(ByVal sender As Object, ByVal e As CommandEventArgs)
        Session("tdlActionSortOrder") = e.CommandArgument
        SortGRDAction()
    End Sub

    Private Sub SortGRDTask()
        If Val(Session("tdlCallTaskSortWay")) Mod 2 = 0 Then
            dtvTask.Sort = Session("tdlCallTaskOrderTask") & " DESC"
        Else
            dtvTask.Sort = Session("tdlCallTaskOrderTask") & " ASC"
        End If
        Session("tdlCallTaskSortWay") += 1
        'mActionRowValue = 0
        dtgTask.DataSource = dtvTask
        dtgTask.DataBind()
    End Sub

    Private Sub SortGRDDuplicateTask()
        Try
            If Val(Session("tdlCallTaskSortWay")) Mod 2 = 0 Then
                dtvTask.Sort = Session("tdlCallTaskOrderTask") & " ASCC"
            Else
                dtvTask.Sort = Session("tdlCallTaskOrderTask") & " DES"
            End If
            'mActionRowValue = 0
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SortGRDAction()
        If Val(Session("tdlActionSortWay")) Mod 2 = 0 Then
            dtvAction.Sort = Session("tdlActionSortOrder") & " DESC"
        Else
            dtvAction.Sort = Session("tdlActionSortOrder") & " ASC"
        End If
        Session("tdlActionSortWay") += 1
        mActionRowValue = 0
        grdAction.DataSource = dtvAction
        grdAction.DataBind()
    End Sub

    Private Sub SortGRDDuplicateAction()
        Try
            If Val(Session("tdlActionSortWay")) Mod 2 = 0 Then
                dtvAction.Sort = Session("tdlActionSortOrder") & " ASC"
            Else
                dtvAction.Sort = Session("tdlActionSortOrder") & " DESC"
            End If
            mActionRowValue = 0
            grdAction.DataSource = dtvAction
            grdAction.DataBind()
        Catch ex As Exception
        End Try
    End Sub

End Class