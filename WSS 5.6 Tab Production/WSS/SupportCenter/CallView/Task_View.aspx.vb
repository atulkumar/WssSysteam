
#Region "NAmespaces"
Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports ION.Logging
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data
#End Region

#Region "Session Used"
' Session("SortOrder") replace with  ViewState("SortOrder")
' Session("SortWay")  replace with ViewState("SortWay")
'Session("DepTask")  replace with ViewState("DepTask")
'Session("PageSize")  replace with  ViewState("PageSize")
'Session("CompName")  replace with ViewState("CompName")
'Session("PropCAComp")  replace with ViewState("CompanyID")
'Session("PropActionNumber")  replace with ViewState("ActionNumber") 
'Session("PropTaskNumber")  replace with ViewState("TaskNumber") 
'Session("PropCallNumber")  replace with  ViewState("CallNo")
'Session("PropTaskStatus") replace with ViewState("TaskStatus")
'Session("TaskViewName")  replace with   ViewState("TaskViewName")
'Session("TaskViewValue")  replace with ViewState("TaskViewValue")
'Session("TVmshCall")     replace with   ViewState("TVmshCall")
'Session("PropRootDir")
'Session("PropUserName")
'Session("PropRole")
'Session("PropUserID")  
'Session("gshPageStatus") 
#End Region
Partial Class SupportCenter_CallView_Task_View
    Inherits System.Web.UI.Page
    Protected WithEvents dtActionDate As DateSelector
    'Protected WithEvents ImgClose As System.Web.UI.WebControls.ImageButton

#Region "Global level declaration"

    Private mdvtable As New DataView ' store data from table for view grid 
    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 

    Public introwvalues As Integer 'current selected row's value stored
    Private intColumnCount As Integer  'grid columns count
    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************

    Private arrtextvalue As ArrayList = New ArrayList
    ' Private Shared arSetColumnName As New ArrayList
    '************************************************
    Public mintPageSize As Integer
    Protected _currentPageNumber As Int32 = 1
    'these variable store the position of the columns
    '****************************************
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
    Private RelatedCallColumnNo As String = String.Empty
    '********************************************
    Private txthiddenImage As String 'stored clicked button's cation  
    ' Protected Shared mshCall As Short 'store info when click on closed task button

    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Public mstrCallNumber As String
    Public mstrTaskNumber As String
    Public mstrTaskStatus As String
    Public mstrcomp As String
    Private null As System.DBNull

    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments

    Private mintUserID As Integer
    Private intComp As String
    '*******************
    Dim inttxtvalueResult As Integer
    Public intHIDAttach As Integer

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
        Dim strFilter As String
        Dim strSearch As String
        txtCSS(Me.Page)
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")

        If Request.QueryString("ScreenFrom") = "HomePage" Then
            imgClose.Visible = False
        End If

        If IsPostBack = False Then
            'sorting session variable clear at page first time load 
            ViewState("SortOrder") = Nothing
            ViewState("SortWay") = 0
            imgCloseCall.ToolTip = "View only closed tasks"

            If ViewState("DepTask") = Nothing Then ' if session is not assigned then set to default
                ViewState("DepTask") = 0
            End If

            Dim arColWidth As New ArrayList
            Dim arrTextboxId As New ArrayList
            Dim arrColumnsName As New ArrayList

            ViewState.Add("arColWidth", arColWidth)
            ViewState.Add("arrTextboxId", arrTextboxId)
            ViewState.Add("arrColumnsName", arrColumnsName)

        End If

        arrImageUrlEnabled.Clear()
        arrImageUrlEnabled.Add("../../Images/comment2.gif")
        arrImageUrlEnabled.Add("../../Images/attach15_9.gif")

        arrImageUrlDisabled.Clear()
        arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
        arrImageUrlDisabled.Add("../../Images/white.gif")

        arrImageUrlNew.Clear()
        arrImageUrlNew.Add("../../Images/comment_Unread.gif")


        'Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")


        'if task not opened
        '*******************************************
        Panel1.Visible = True
        GrdAddSerach.Visible = True
        'ddlstview.Enabled = True
        '********************************************
        'paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlTaskView$txtPageSize"))


        If IsPostBack = False Then
            Call txtCSS(Me.Page, "cpnlTaskAction")
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1

            'javascript function added with controls
            '**********************************************************************************
            'imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            BtnGrdSearch.Attributes.Add("onclick", "return SaveEdit('Search');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgAttachments.Attributes.Add("Onclick", "return SaveEdit('Attach');")
            imgFWD.Attributes.Add("Onclick", "return SaveEdit('Fwd');")
            ImgActionView.Attributes.Add("Onclick", "return SaveEdit('ActionView');")
            imgMonitor.Attributes.Add("Onclick", "return SaveEdit('Monitor');")
            imgBtnViewPopup.Attributes.Add("Onclick", "return OpenVW('T040011');")
            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
            ddlstview.Attributes.Add("OnChange", "return SaveEdit('View');")
            imgShowReleased.Attributes.Add("Onclick", "return SaveEdit('ShowReleased');")

            '***************************************************************************
            If ChkPageView() = True Then
                txtPageSize.Text = ViewState("PageSize")
                mintPageSize = ViewState("PageSize")
                'SavePageSize()
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = 20
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                    SavePageSize()
                End If
            End If
        Else
            If ViewState("PageSize") = mintPageSize Then
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = ViewState("PageSize")
                    txtPageSize.Text = ViewState("PageSize")
                    'ViewState("PageSize") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                End If

                SavePageSize()
            End If
        End If


        Dim StrUserID As String
        'get logged user id
        mintUserID = HttpContext.Current.Session("PropUserID")

        StrUserID = HttpContext.Current.Session("PropUserID")
        Session("gshPageStatus") = 0

        'cpnlError.Visible = False
        txthiddenImage = Request.Form("txthiddenImage")
        introwvalues = Request.Form("txtrowvalues")

        If Request.Form("txtComp") <> "undefined" Or Request.Form("txtComp") = Nothing Then
            If Request.Form("txtComp") <> "" And Request.Form("txtComp") <> "0" Then
                intComp = Request.Form("txtComp")
                ViewState("CompName") = Request.Form("txtComp")
                mstGetFunctionValue = WSSSearch.SearchCompName(Request.Form("txtComp"))
                mstrcomp = intComp
                ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
            Else
                mstrcomp = 0
            End If
        Else
            ' mstrcomp = 0
        End If

        strhiddenTable = Request.Form("txthiddenTable")
        If strhiddenTable = "cpnlTaskAction_grdAction" Then
            ViewState("ActionNumber") = Val(Request.Form("txtTask"))
            mstrTaskNumber = ViewState("ActionNumber")
        Else
            ViewState("ActionNumber") = 0
            If Not IsNothing(Request.Form("txtTask")) Then
                ViewState("TaskNumber") = Val(Request.Form("txtTask"))
            End If
            mstrTaskNumber = ViewState("TaskNumber")
            If Not IsNothing(Request.Form("txthiddenCallNo")) Then
                ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
            End If
            mstrCallNumber = ViewState("CallNo")
            Dim strStatus As String = WSSSearch.GetTaskStatus(mstrCallNumber, mstrTaskNumber, Val(ViewState("CompanyID")))
            ViewState("TaskStatus") = strStatus
            mstrTaskStatus = ViewState("TaskStatus")
        End If

        'these statements check the button click caption 
        '***********************************************
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "View"

                        ViewState("TaskViewName") = ddlstview.SelectedItem.Text
                        ViewState("TaskViewValue") = ddlstview.SelectedValue
                        SaveUserView()

                        ViewState("CallNo") = 0
                        ViewState("TaskNumber") = 0
                        ViewState("CompanyID") = 0

                    Case "Edit"
                        If strhiddenTable = "cpnlTaskAction_grdAction" Then
                            Exit Select
                        Else
                            ' Response.Redirect("Call_Detail.aspx?CallNumber=" & ViewState("CallNo") & "&ScrID=3&ID=0&PageID=2", False)
                        End If
                    Case "Add"
                        'Response.Redirect("Call_Detail.aspx?CallNumber=" & ViewState("CallNo") & " &ScrID=3&ID=-1&PageID=2", False)
                    Case "Select"

                    Case "Monitor"
                        Dim strScript As String
                        strScript = "<script>window.open('../../CommunicationSetup/CommunicationSetupOnCall.aspx?TaskNo=" & Request.Form("txtTask") & "','Attachment','scrollBars=yes,resizable=No,width=800,height=600,status=no');</script>"
                        Response.Write(strScript)
                    Case "CloseCall"
                        If ViewState("TVmshCall") = 0 Then
                            ViewState("TVmshCall") = 1
                        Else
                            ViewState("TVmshCall") = 0
                        End If

                    Case "ShowReleased"
                        If Val(ViewState("DepTask")) = 0 Then
                            ViewState("DepTask") = 1
                        Else
                            ViewState("DepTask") = 0
                        End If

                    Case "Delete"
                        If ViewState("ActionNumber") = 0 Then
                            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                            SQL.DBTracing = False
                            Dim strchkcallstatus As String = SQL.Search("Task_View", "Load-278", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")
                            If IsNothing(strchkcallstatus) = False Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Call Closed so You cannot change the Task... ")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                                Exit Select
                            End If
                            'get the status of the task becuase only ASSIGNED tasks can be deleted
                            Dim strTaskStatus As String
                            strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNumber"), ViewState("CompanyID"))
                            If strTaskStatus = "ASSIGNED" Then
                                ' Delete TASK
                                mstGetFunctionValue = WSSDelete.DeleteTask(ViewState("CallNo"), ViewState("TaskNumber"), ViewState("CompanyID"))
                                If mstGetFunctionValue.ErrorCode = 0 Then
                                    '''''''''''Rollback Call Status'''''''''''
                                    Dim intRows As Integer
                                    If SQL.Search("Call_Detail", "Load-506", "select * from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                        Dim arrCallStatusColUpdate As New ArrayList
                                        Dim arrCallStatusRowUpdate As New ArrayList
                                        arrCallStatusColUpdate.Add("CN_VC20_Call_Status")
                                        arrCallStatusRowUpdate.Add("OPEN")
                                        WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arrCallStatusColUpdate, arrCallStatusRowUpdate)
                                    End If
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    lstError.Items.Clear()
                                    'cpnlError.Text = "Message..."
                                    lstError.Items.Add("Task Deleted successfully...")
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
                                lstError.Items.Add("This task is  " & strTaskStatus & " so it cannot be deleted...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            End If
                        End If
                    Case "Save"
                    Case "Fwd"
                        Response.Write("<script>window.open('Task_Fwd.aspx?ScrID=340','Fwd','scrollBars=yes,resizable=No,width=400,height=350,status=yes');</script>")
                    Case "Attach"
                        Response.Write("<script>window.open('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=550,status=yes');</script>")
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select


                If Val(ViewState("DepTask")) = 0 Then ' -- Change Tooltip for show released tasks
                    imgShowReleased.ToolTip = "Show only released tasks"
                Else
                    imgShowReleased.ToolTip = "Show all tasks"
                End If


            Catch ex As Exception
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("Task_View", "Load-386", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            End Try
        End If
        '*********************************************************************

        strFilter = ""
        strSearch = ""

        If Not IsPostBack Then
            ViewState("CallNo") = 0
            mstrCallNumber = 0
            'fill dropdown combo with view name from database
            GetView()

            If ViewState("TVmshCall") = Nothing Then
                ViewState("TVmshCall") = 0 ' VARIABLE USE FOR SHOW CLOSE TASK
            End If

            ChkSelectedView() 'chk user selected view last time

            If ViewState("TaskViewName") <> "" And ViewState("TaskViewName") <> "Default" Then
                ' fill datagrid based on user define columns and combination
                Fillview()
            Else
                'fill tha datagrid from based on admin defined to the role
                FillDefault()
                ViewState("TaskViewName") = "Default"

            End If
            CurrentPg.Text = _currentPageNumber.ToString()
            CreateTextBox()
        Else
            'This loop filling new arraylist in the arrtextvalue array
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
            For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                arrtextvalue.Add(Request.Form("cpnlTaskView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
            Next

            If ddlstview.SelectedValue = 0 Then
                FillDefault()
            Else
                Fillview()
            End If

            'This loop filling new arraylist in the arrtextvalue array
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
            For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                arrtextvalue.Add(Request.Form("cpnlTaskView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
            Next
            CreateTextBox()
        End If

        'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
        '************************************************
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Or CHKF.Checked = True Then
            FillGRDAfterSearch()
        End If
        '***********************************************
        'Restore the grid selection on click of grid's row when page post back
        '********'************************************************************************
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        '*************************************************************************
        If ViewState("TVmshCall") = 0 Then
            imgCloseCall.ToolTip = "View only closed task"
        Else
            imgCloseCall.ToolTip = "View only open tasks"
        End If
        '*************************************************************************

        If Val(ViewState("CallNo")) > 0 Then
            imgEdit.ToolTip = "Edit Task"
            imgDelete.ToolTip = "Delete Task"
            imgFWD.ToolTip = "Forward Task"
            imgMonitor.ToolTip = "Set Task Monitor"
            ImgActionView.ToolTip = "View Actions"
            If ChangeAttachmentToolTip(ViewState("CompanyID"), ViewState("CallNo")) = True Then
                imgAttachments.ToolTip = "View Attachment"
                intHIDAttach = 1
            Else
                imgAttachments.ToolTip = "No Attachment Uploaded"
                intHIDAttach = 0
            End If
        Else
            imgAttachments.ToolTip = "Select a Call to View Attachment"
            intHIDAttach = -1

            imgEdit.ToolTip = "Select a Task to Edit"
            imgDelete.ToolTip = "Select a Task to Delete"
            imgFWD.ToolTip = "Select a Task to Forward"
            imgMonitor.ToolTip = "Select a Task to set Task Monitor"
            ImgActionView.ToolTip = "Select a Task to View Actions"
        End If

        If Val(ViewState("DepTask")) = 0 Then ' -- Change Tooltip for show released tasks
            imgShowReleased.ToolTip = "Show only released tasks"
        Else
            imgShowReleased.ToolTip = "Show all tasks"
        End If


        'to apply grid color
        '************************************************
        GrdAddSerach.AlternatingItemStyle.BackColor = Color.FromArgb(245, 245, 245)
        GrdAddSerach.ItemStyle.BackColor = Color.FromArgb(255, 255, 255)
        '*******************************************************
        'Security Block
        '***********************************************
        Dim intid As String
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = 5
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If
        'End of Security Block
        '********************************************************
    End Sub

#End Region

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
    Private Sub FillDefault()

        Try

            Dim dsDefault As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select "
            Dim strwhereQuery As String = " and "
            Dim shJoin As Short
            GrdAddSerach.PageSize = mintPageSize ' set the grid page size
            Dim strQuery As String 'fatching default data from tables for particular role


            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
              & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
              & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =464 And " _
              & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
              & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
              & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
              & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
              & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=464 and obm_vc4_object_type_fk='VIW') " _
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
                    rowcount = rowcount + 1
                End While
                sqrdView.Close()
                If rowcount = 16 Then
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("You Dont have Access on Default View...")
                    lstError.Items.Add("Please Select your Own View from View Dropdown...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskView.Enabled = False
                    cpnlTaskView.TitleCSS = "test2"
                    ViewState("CallNo") = 0
                    Exit Sub
                End If
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                If shJoin = 1 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                ElseIf shJoin = 2 Then
                    strSelect &= " from T040011,T040021 Task,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number  and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                ElseIf shJoin = 3 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where  task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                End If
                If ViewState("TVmshCall") = 1 Then
                    strSelect &= " and TM_VC50_Deve_Status='Closed' "
                Else
                    strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
                End If
                'If ViewState("DepTask") = 1 Then
                '    strSelect &= " and TM_NU9_Dependency  is not null  "
                'End If
                strSelect &= " and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK "
                'Added company chk from company access table
                strSelect &= " and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
                strSelect &= " order by TM_NU9_Call_No_FK desc"

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
                RelatedCallColumnNo = ""

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
                            RelatedCallColumnNo = inti
                        End If
                    Next

                    mdvtable.Table = dsDefault.Tables("T040021")

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("SubTaskDesc", 45)
                    htGrdColumns.Add("CallDesc", 44)
                    htGrdColumns.Add("CallSubject", 29)
                    htGrdColumns.Add("TaskDesc", 45)

                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNumber"), ViewState("ActionNumber"))

                    SetDataTableDateFormat(mdvtable.Table, htDateCols)

                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    rowvalue = 0
                    rowvalueCall = 0

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.AllowPaging = True
                    GrdAddSerach.PageSize = mintPageSize

                    If ViewState("TaskViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
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

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.DataBind()
                    '********************************************************
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
                    cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskView.Enabled = True
                    cpnlTaskView.TitleCSS = "test"
                    '***********************************************
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No Task Assigned...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskView.Enabled = False
                    cpnlTaskView.TitleCSS = "test2"
                End If
            Else
                GrdAddSerach.Visible = False
                Panel1.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("Sorry! Task View Data not available...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                cpnlTaskView.Enabled = False
                cpnlTaskView.TitleCSS = "test2"
            End If
            '***********************************************************
        Catch ex As Exception
            CreateLog("Task_View", "Load-792", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

#End Region

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
    Private Sub Fillview()

        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim arcolName As New ArrayList

        GrdAddSerach.PageSize = mintPageSize ' set the grid page size
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            Dim shJoin As Short
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            Dim strwhereQuery As String = " and "

            sqrdView = SQL.Search("Task_View", "FillView-846", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='464' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
                sqrdView = SQL.Search("Task_View", "FillView-897", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='464'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
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
                sqrdView = SQL.Search("Task_View", "FillView-921", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='464' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
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
                If shJoin = 1 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_VC8_Supp_Owner=SOwner.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom  and Coord.UM_IN4_Address_No_FK =* T040011.CM_NU9_Coordinator "
                ElseIf shJoin = 2 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                    'strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and Task.TM_NU9_Assign_by=ABy.UM_IN4_Address_No_FK and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom"
                ElseIf shJoin = 3 Then
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                Else
                    strSelect &= " from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project,T060011 ReqBy, T060011 EntBy,T060011 Coord where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ReqBy.UM_IN4_Address_No_FK=T040011.CM_NU9_Call_Owner  and EntBy.UM_IN4_Address_No_FK=T040011.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* T040011.CM_NU9_Coordinator "
                End If
                If ViewState("TVmshCall") = 1 Then
                    strSelect &= " and TM_VC50_Deve_Status='Closed' "
                Else
                    strSelect &= " and TM_VC50_Deve_Status<>'Closed' "
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
                'Added new for call columns
                mstrCallEntBy = ""
                mstrCallEntByID = ""
                mstrCallReqBy = ""
                mstrCallReqByID = ""
                mstrCoordinator = ""
                mstrCoordinatorID = ""
                RelatedCallColumnNo = ""

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
                            RelatedCallColumnNo = inti
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
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                        GrdAddSerach.AllowPaging = True
                        GrdAddSerach.PageSize = mintPageSize
                    End If

                    '*************************************************************************
                    rowvalue = 0
                    rowvalueCall = 0

                    If ViewState("TaskViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
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

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If

                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.DataBind()
                    '********************************************************
                    'paging count
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
                    cpnlTaskView.State = CustomControls.Web.PanelState.Expanded
                    cpnlTaskView.Enabled = True
                    cpnlTaskView.TitleCSS = "test"
                    '***********************************************
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    ' DisplayMessage("No Task Assigned or data not exist according to view query...")
                    lstError.Items.Add("No Task Assigned or data not exist according to view query...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    cpnlTaskView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlTaskView.Enabled = False
                    cpnlTaskView.TitleCSS = "test2"
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("Task_View", "FillView-1021", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

#End Region

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
            sqrdView = SQL.Search("Task_View", "GetView-1200", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='464' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
        Dim col1 As Unit
        Dim col1cng As String
        Dim strcolid As String

        CType(ViewState("arrTextboxId"), ArrayList).Clear()

        Try
            intColumnCount = mdvtable.Table.Columns.Count
        Catch ex As Exception
        End Try

        Try

            For intii = 0 To intColumnCount - 1
                _textbox = New TextBox
                If Not IsPostBack Then
                    col1 = Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intii))
                    If intii > 5 And intii < 25 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.0
                        col1cng = col1cng & "pt"
                    End If
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "F" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "CallReqByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "CallEntByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "CoordinatorID" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & CType(ViewState("arrColumnsName"), ArrayList).Item(intii) & " runat=""server"" Visible=""False"" Width=""0"" CssClass=""SearchTxtBox""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & CType(ViewState("arrColumnsName"), ArrayList).Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=""SearchTxtBox"" MaxLength=""20""></asp:TextBox>"))
                    End If
                    _textbox.ID = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    _textbox.Text = ""
                Else
                    col1 = Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intii))
                    If intii > 5 And intii < 25 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.0
                        col1cng = col1cng & "pt"
                    End If
                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If
                    strcolid = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "F" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "CallReqByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "CallEntByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "CoordinatorID" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Visible=""False"" Width=""0"" CssClass=""SearchTxtBox""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=""SearchTxtBox"" MaxLength=""20""></asp:TextBox>"))
                    End If
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                End If

                CType(ViewState("arrTextboxId"), ArrayList).Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("Task_View", "CreateTextBox-1271", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
            Dim Comm As New TemplateColumn
            Comm.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(20)
            Comm.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(20)
            Comm.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            Comm.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            Comm.HeaderText = "C"
            GrdAddSerach.Columns.Add(Comm)

            Dim Attach As New TemplateColumn
            Attach.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(20)
            Attach.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(20)
            Attach.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            Attach.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            Attach.HeaderText = "A"
            GrdAddSerach.Columns.Add(Attach)


            Dim frm As New TemplateColumn
            frm.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(17)
            frm.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(17)
            frm.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            frm.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            frm.HeaderText = "F"
            GrdAddSerach.Columns.Add(frm)

            For intI = 0 To CType(ViewState("arrColumnsName"), ArrayList).Count - 1

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
                    GrdAddSerach.Columns.Add(Bound_Column)
                End If
            Next

        Catch ex As Exception
            CreateLog("Task_View", "FormatGrid-1303", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
    Private Sub FillGRDAfterSearch()

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim arCount As Integer = arrtextvalue.Count - 1
        Dim intI As Integer

        Try

            'For intI As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
            For intI = 0 To arCount
                If Not IsNothing(arrtextvalue(intI)) Then
                    If Not arrtextvalue(intI).Equals("") Then
                        strSearch = arrtextvalue(intI)
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") Then
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                                Dim chk As Date
                                If IsDate(strSearch) = False Then
                                    strSearch = "12/12/1825"
                                    '*******************************************************************
                                End If
                            End If

                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") = True Then
                                strSearch = strSearch.Replace("*", "")
                                If IsNumeric(strSearch) = False Then
                                    strSearch = "-101"
                                    '******************************************************************
                                End If
                            End If
                            strSearch = strSearch.Replace("*", "")
                            strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            strSearch = arrtextvalue(intI)
                            strSearch = GetSearchString(strSearch)
                            'strSearch = strSearch.Replace("*", "%")
                            strSearch &= "%"
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

            HTMLEncodeDecode(mdlMain.Action.Decode, mdvtable)
            GetFilteredDataView(mdvtable, strRowFilterString)
            HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)
            'SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel)
            SetCommentFlag(mdvtable, mdlMain.CommentLevel.AllTaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNumber"), ViewState("ActionNumber"))
            GrdAddSerach.DataSource = mdvtable

            rowvalue = 0
            rowvalueCall = 0

            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            '**********************************************
            If ViewState("TaskViewName") <> ddlstview.SelectedItem.Text Then
                GrdAddSerach.CurrentPageIndex = 0
                CurrentPg.Text = 1
            End If
            If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                GrdAddSerach.CurrentPageIndex = 0
                CurrentPg.Text = 1
            End If

            GrdAddSerach.DataBind()

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

            If mdvtable.Count = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If

        Catch ex As Exception
            CreateLog("Task_View", "Click-1585", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "btngrdsearch")
        End Try

    End Sub

#End Region

#Region "Search Grid Item Data Bound Event"
    '*******************************************************************
    ' Function             :-  GrdAddSerach_ItemDataBound1
    ' Purpose              :-Display attachment, comment forms, based on database and and bound java script on columns like selection and double click
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
        Dim strID As String = String.Empty
        Dim strName As String = String.Empty
        Dim strCompId As String = String.Empty
        Dim intcolno As Integer = 0
        Dim attSts As Boolean
        Dim frmSts As String = String.Empty
        Dim rowflag As Boolean = True
        Dim intCount As Integer = 2
        Dim comstat As String = String.Empty
        Dim intcolnoComm As Integer = 0
        Dim intCountfrm As Integer = 3

        'these variables stored columns position in datagrid
        '*************************************
        Dim strSuppOwnID As String = String.Empty
        Dim suppownrowid As String = String.Empty
        Dim strSuppOwn As String = String.Empty
        Dim strSuppOwnrowID As String = String.Empty
        Dim strAssignBy As String = String.Empty
        Dim strAssignByRowID As String = String.Empty
        Dim strAssignByID As String = String.Empty
        Dim AssignByRowID As String = String.Empty
        Dim strTaskNoRowID As String = String.Empty

        'Added New Columns for Call Columns 
        Dim strCallEntBy As String = String.Empty
        Dim strCallEntByRowID As String = String.Empty

        Dim strCallEntByID As String = String.Empty
        Dim strCallEntByIDRowID As String = String.Empty

        Dim strCallReqBy As String = String.Empty
        Dim strCallRegByRowID As String = String.Empty

        Dim strCallReqByID As String = String.Empty
        Dim strCallReqByIDRowID As String = String.Empty

        Dim strCoordinator As String = String.Empty
        Dim strCoordinatorRowID As String = String.Empty

        Dim strCoordinatorID As String = String.Empty
        Dim strCoordinatorIDRowID As String = String.Empty

        Dim RelatedCallNo As String = String.Empty
        Dim RelatedCallNoId As String = String.Empty

        '*************************************************************************************************
        Dim Monistat As Boolean 'this is return true or false for monitoring 
        '  GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    'Variables store the position of the columns, +3 means we added three columns manually(comment attachment and form) in datagrid that's why we adding +3
                    '**********************************************
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strName = e.Item.Cells(mintCallNoPlace + 3).Text()
                    strCompId = e.Item.Cells(mintCompId + 3).Text()
                    strSuppOwnID = e.Item.Cells(mintSuppOwnID + 3).Text()
                    suppownrowid = mintSuppOwnID + 3
                    strTaskNoRowID = mintTaskNoRowID + 3

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

                    'Added new columns for Calls
                    If mstrCallEntBy.Equals("") = False Then
                        strCallEntBy = e.Item.Cells(mstrCallEntBy + 3).Text()
                        strCallEntByRowID = mstrCallEntBy + 3
                    End If
                    If mstrCallEntByID.Equals("") = False Then
                        strCallEntByID = e.Item.Cells(mstrCallEntByID + 3).Text()
                        strCallEntByIDRowID = mstrCallEntByID
                    End If
                    If mstrCallReqBy.Equals("") = False Then
                        strCallReqBy = e.Item.Cells(mstrCallReqBy + 3).Text()
                        strCallRegByRowID = mstrCallReqBy + 3
                    End If
                    If mstrCallReqByID.Equals("") = False Then
                        strCallReqByID = e.Item.Cells(mstrCallEntByID + 3).Text()
                        strCallReqByIDRowID = mstrCallEntByID + 3
                    End If
                    If mstrCoordinator.Equals("") = False Then
                        strCoordinator = e.Item.Cells(mstrCoordinator + 3).Text()
                        strCoordinatorRowID = mstrCoordinator + 3
                    End If
                    If mstrCoordinatorID.Equals("") = False Then
                        strCoordinatorID = e.Item.Cells(mstrCoordinatorID + 3).Text()
                        strCoordinatorIDRowID = mstrCoordinatorID + 3
                    End If
                    If RelatedCallColumnNo.Equals("") = False Then
                        RelatedCallNo = e.Item.Cells(RelatedCallColumnNo + 3).Text()
                        RelatedCallNoId = RelatedCallColumnNo + 3
                    End If
                    '*******************************************
                    'It is display toltip on datagrid columns
                    e.Item.ToolTip = " Call # " & strName & "  Task # " & strID & " "
                    'Show attachment and comment image 
                    '*****************************************************************
                    'for attacment images********************
                    If rowflag Then
                        attSts = IIf(e.Item.Cells(mdvtable.Table.Columns.Count + 1).Text = "&nbsp;", False, True)
                        'attSts = getAttach(strName, strID, strCompId)
                    End If
                    If attSts Then
                        CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Attach15_9.gif"
                        CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ToolTip = "Click To View Attachments"
                        'CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlTaskView_GrdAddSerach','" & intCount & "','" & strCompId & "','" & strName & "')")
                        CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCount & "','" & strCompId & "','" & strName & "')")
                    Else
                        CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ToolTip = "No Attachment"
                    End If

                    'for comment images********************
                    If rowflag Then
                        comstat = e.Item.Cells(mdvtable.Table.Columns.Count).Text
                        'comstat = GComm(strName, strID, strCompId)
                        Select Case comstat
                            Case "1"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment2.gif"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "Old Comments"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & strName & "')")

                            Case "2"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Unread.gif"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "New Comments"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & strName & "')")

                            Case "0"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "No Comment"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & strName & "')")
                            Case Else
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ToolTip = "No Comment"
                                CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlTaskView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & strName & "')")
                        End Select
                    End If

                    'for form image***********************************
                    If rowflag Then
                        frmSts = e.Item.Cells(mdvtable.Table.Columns.Count + 2).Text
                    End If
                    If frmSts = "1" Then
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Form1.jpg"
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ToolTip = "Filled Form"
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCountfrm & "','" & strCompId & "','" & strName & "')")
                    ElseIf frmSts = "2" Then
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Form2.gif"
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ToolTip = "Empty Form"
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlTaskView_GrdAddSerach', '" & intCountfrm & "','" & strCompId & "','" & strName & "')")
                    Else
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        CType(e.Item.Cells(2).FindControl("imgform"), System.Web.UI.WebControls.Image).ToolTip = "No Form"
                    End If
                    '*********************************************************************************************
                    'Attcah double click event in grid columna after image columns
                    'these line of code added click or double click functionality on grid after three columns
                    '**************************************************************************************************
                    If intcolno >= 3 Then
                        If intcolno = Val(strSuppOwnrowID) Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            ' e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strSuppOwnID & ",'" & strSuppOwn & "')")
                        ElseIf intcolno = Val(strAssignByRowID) Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            ' e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strAssignByID & ",'" & strAssignBy & "')")
                        ElseIf intcolno = Val(strCallEntByRowID) Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            ' e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strCallEntByID & ",'" & strCallEntBy & "')")
                        ElseIf intcolno = Val(strCallRegByRowID) Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            ' e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strCallReqByID & ",'" & strCallReqBy & "')")
                        ElseIf intcolno = Val(strCoordinatorRowID) Then
                            If Val(strCoordinatorID) > 0 Then
                                e.Item.Cells(intcolno).ForeColor = Color.Blue
                                e.Item.Cells(intcolno).CssClass = "celltext"
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                ' e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555( " & strCoordinatorID & ",'" & strCoordinator & "')")
                            End If
                        ElseIf intcolno = Val(RelatedCallNoId) Then
                            If Val(RelatedCallNo) > 0 Then
                                e.Item.Cells(intcolno).ForeColor = Color.Blue
                                e.Item.Cells(intcolno).CssClass = "celltext"
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & "," & RelatedCallNo & ", '" & rowvalue & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            End If
                        ElseIf intcolno = strTaskNoRowID Then
                            Monistat = GetMonStat(strName, strID, strCompId)
                            If Monistat = True Then
                                e.Item.Cells(intcolno).ForeColor = Color.Red
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & "," & e.Item.Cells(mintCallNoPlace + 3).Text & ", '" & rowvalue & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            Else
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & "," & e.Item.Cells(mintCallNoPlace + 3).Text & ", '" & rowvalue & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            End If
                        Else
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & "," & strName & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & "," & e.Item.Cells(mintCallNoPlace + 3).Text & ", '" & rowvalue & "','cpnlTaskView_GrdAddSerach','" & strCompId & "')")
                        End If
                    Else
                        e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                    End If
                End If
                rowflag = False
                intcolno = intcolno + 1
                '*********************************************************
            Next

            rowvalue += 1
            rowvalueCall += 1

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                If Val(ViewState("CallNo")) <> 0 And Val(ViewState("CompanyID")) <> 0 And Val(ViewState("TaskNumber")) <> 0 Then
                    If strName = Val(ViewState("CallNo")) And ViewState("CompName") = strCompId And Val(strID) = Val(ViewState("TaskNumber")) Then
                        e.Item.BackColor = Color.FromArgb(212, 212, 212)
                    End If
                End If

                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    If GetDepStat(Val(strName), strCompId, Val(strID)) = True Then
                        For intI As Integer = 0 To e.Item.Cells.Count - 1
                            e.Item.Cells(intI).BackColor = System.Drawing.Color.FromArgb(210, 233, 255)
                        Next
                    End If
                    If GetCallPriority(Val(strName), strCompId, strID) = True Then
                        For intI As Integer = 0 To e.Item.Cells.Count - 1
                            e.Item.Cells(intI).ForeColor = Color.Red
                        Next
                    End If
                End If

            End If

        Catch ex As Exception
            CreateLog("Task_View", "ItemDataBound-1626", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdaddserach")
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

#End Region

#Region "GetMonStat"
    '*******************************************************************
    ' Function             :-  GetMonStat
    ' Purpose              :-Return the monitoring status  true or false
    '								
    ' Date					  Author						Modification Date					Description
    ' 18/01/06			  Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
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

#End Region

#Region "Create Action Grid"

    ''Private Sub CreateGridAction()
    ''    Dim lc1 As New LiteralControl
    ''    Dim lc2 As New LiteralControl
    ''    grdAction.ID = "grdAction"
    ''    grdAction.DataKeyField = "AM_NU9_Action_Number"
    ''    Call FormatGridAction()

    ''    PlaceHolder1.Controls.Add(grdAction)
    ''End Sub

    ''Private Sub FormatGridAction()
    ''    grdAction.AutoGenerateColumns = False
    ''    grdAction.AllowPaging = True
    ''    '  grdAction.ShowFooter = True
    ''    grdAction.ShowHeader = True
    ''    grdAction.HeaderStyle.CssClass = "GridHeader"
    ''    grdAction.HeaderStyle.Height = System.Web.UI.WebControls.Unit.Pixel(1)
    ''    grdAction.Width = System.Web.UI.WebControls.Unit.Percentage(100)
    ''    grdAction.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
    ''    grdAction.BorderStyle = BorderStyle.None
    ''    grdAction.CellPadding = 1
    ''    grdAction.AllowPaging = False
    ''    grdAction.CssClass = "Grid"
    ''    grdAction.HorizontalAlign = HorizontalAlign.Center
    ''    'grdAction.FooterStyle.CssClass = "GridFixedFooter"
    ''    grdAction.SelectedItemStyle.CssClass = "GridSelectedItem"
    ''    grdAction.AlternatingItemStyle.CssClass = "GridAlternateItem"
    ''    grdAction.ItemStyle.CssClass = "GridItem"
    ''End Sub
#End Region

#Region "Create Template Column Action Grid"
    ''Private Sub createTemplateColumnsAction()
    ''    Dim intCount As Integer
    ''    ReDim tclAction(dtvAction.Table.Columns.Count)

    ''    arrImageUrlEnabled.Clear()
    ''    arrImageUrlEnabled.Add("../../Images/comment2.gif")
    ''    arrImageUrlEnabled.Add("../../Images/attach15_9.gif")

    ''    arrImageUrlDisabled.Clear()
    ''    arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
    ''    arrImageUrlDisabled.Add("../../Images/white.gif")

    ''    arrImageUrlNew.Clear()
    ''    arrImageUrlNew.Add("../../Images/comment_Unread.gif")

    ''    arrColumnsNameAction.Clear()
    ''    arrColumnsNameAction.Add("Com")
    ''    arrColumnsNameAction.Add("Att")
    ''    arrColumnsNameAction.Add("Act#")
    ''    'arrColumnsNameAction.Add("Action")
    ''    arrColumnsNameAction.Add("Description")
    ''    arrColumnsNameAction.Add("Act.")
    ''    arrColumnsNameAction.Add("Used_Hr.")
    ''    arrColumnsNameAction.Add("Action<u>O</u>wner")
    ''    'arrColumnsNameAction.Add("Priority")
    ''    arrColumnsNameAction.Add("Action_Date")

    ''    arrWidthAction.Clear()
    ''    arrWidthAction.Add(10)
    ''    arrWidthAction.Add(10)
    ''    arrWidthAction.Add(30)
    ''    'arrWidthAction.Add(50)
    ''    arrWidthAction.Add(47)
    ''    arrWidthAction.Add(10)
    ''    arrWidthAction.Add(15)
    ''    arrWidthAction.Add(60)
    ''    'arrWidthAction.Add(45)
    ''    arrWidthAction.Add(64)


    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(0)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(1)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(2)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Percentage(arrWidthAction(3)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(4)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(5)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(6)))
    ''    arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(7)))


    ''    'tclAction(0) = New TemplateColumn
    ''    'tclAction(0).Visible = False
    ''    'tclAction(0).HeaderTemplate = New IONGrid.CreateItemTemplateSubmitButton("", "btn")
    ''    'grdAction.Columns.Add(tclAction(0))

    ''    tclAction(0) = New TemplateColumn
    ''    tclAction(0).Visible = True
    ''    tclAction(0).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvAction.Table.Columns(0).ToString + "_H", False, arrColumnsNameAction(0), False)
    ''    tclAction(0).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvAction.Table.Columns(0).ToString, arrImageUrlDisabled(0))
    ''    tclAction(0).ItemStyle.HorizontalAlign = HorizontalAlign.Center
    ''    tclAction(0).ItemStyle.Width = arrColumnsWidthAction(0)
    ''    grdAction.Columns.Add(tclAction(0))

    ''    tclAction(1) = New TemplateColumn
    ''    tclAction(1).Visible = True
    ''    tclAction(1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvAction.Table.Columns(1).ToString + "_H", False, arrColumnsNameAction(1), False)
    ''    tclAction(1).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvAction.Table.Columns(1).ToString, arrImageUrlDisabled(1))
    ''    tclAction(1).ItemStyle.HorizontalAlign = HorizontalAlign.Center
    ''    tclAction(1).ItemStyle.Width = arrColumnsWidthAction(1)
    ''    grdAction.Columns.Add(tclAction(1))

    ''    For intCount = 2 To dtvAction.Table.Columns.Count - 1
    ''        tclAction(intCount + 1) = New TemplateColumn
    ''        tclAction(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvAction.Table.Columns(intCount).ToString, dtvAction.Table.Columns(intCount).ToString)
    ''        tclAction(intCount + 1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvAction.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameAction(intCount), True)
    ''        tclAction(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvAction.Table.Columns(intCount).ToString + "_F", False)
    ''        tclAction(intCount + 1).ItemStyle.Width = arrColumnsWidthAction(intCount)    'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthAction(intCount))
    ''        grdAction.Columns.Add(tclAction(intCount + 1))
    ''    Next

    ''End Sub

#End Region

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Add(Msg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
    End Sub
#End Region

#Region "Clear TextBoxes based on panels"
    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        Dim objTextBox As Control
        For Each objTextBox In CPnl.Controls
            If TypeOf objTextBox Is TextBox Then
                CType(objTextBox, TextBox).Text = ""

            End If
        Next
        dtActionDate.CalendarDate = ""
    End Sub
#End Region

    '#Region "Close Button Event"
    '    Private Sub ImgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgClose.Click
    '        Try
    '            Response.Redirect("../../home.aspx", False)
    '        Catch ex As Exception
    '            CreateLog("Task_View", "ImgClose_Click-2716", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
    '        End Try
    '    End Sub
    '#End Region

#Region "Dropdown Events"
    Private Sub ddlstview_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged
        'Session("TaskViewName") = ddlstview.SelectedItem.Text
        ' ViewState("TaskViewValue") = ddlstview.SelectedValue
        'SaveUserView()
    End Sub
#End Region

#Region "Paging Events"
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
    End Sub
#End Region

#Region "Save User View"
    Private Sub SaveUserView()
        Dim intid = 464
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
#End Region

#Region "Save Page Size"
    Private Sub SavePageSize()
        Dim intid = 464
        Dim strCheck As String = SQL.Search("Historicview", "SavePageSize-3406", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "")
        If Not IsNothing(strCheck) Then
            'update
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            arColumnName.Add("PS_NU9_PSize")
            arRowData.Add(Val(ViewState("PageSize")))
            If SQL.Update("T030214", "SavePageSIZE", "update  T030214 set PS_NU9_PSize=" & Val(ViewState("PageSize")) & "  where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid"), SQL.Transaction.Serializable) = True Then
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

            arRowData.Add(Val(ViewState("PageSize")))
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
#End Region

#Region "Check Page View"
    Private Function ChkPageView() As Boolean
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=464 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)
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
            CreateLog("Task_View", "ChkSelectedView-2080", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
#End Region

#Region "Grid Events"

    Private Sub ChkSelectedView()
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=464 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

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
    Private Sub GrdAddSerach_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddSerach.SortCommand
        ViewState("SortOrder") = e.SortExpression
        Call SortGRD()
    End Sub
    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            Dim intA As Integer = 0
            For intI = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1 + 3
                If intI > 2 Then
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "SuppOwnID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "AssignByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "A" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "F" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "CallReqByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "CallEntByID" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "CoordinatorID" Then
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
#End Region

#Region "Sorting function"
    Private Sub SortGRD()
        ' If SortWay Mod 2 = 0 Then
        If Val(ViewState("SortWay")) Mod 2 = 0 Then
            mdvtable.Sort = ViewState("SortOrder") & " ASC"
        Else
            mdvtable.Sort = ViewState("SortOrder") & " DESC"
        End If
        ViewState("SortWay") += 1
        If GrdAddSerach.AutoGenerateColumns = False Then
            GrdAddSerach.AutoGenerateColumns = True
        End If
        rowvalue = 0
        GrdAddSerach.DataSource = mdvtable
        GrdAddSerach.DataBind()
    End Sub
    Private Sub SortGRDDuplicate()
        Try
            If Val(ViewState("SortWay")) Mod 2 = 0 Then
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
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "BindGridTask"
    Private Sub BindGridTask()

        Dim htGrdColumns As New Hashtable
        htGrdColumns.Add("TaskDesc", 23)

        HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
        GrdAddSerach.DataSource = mdvtable.Table
        GrdAddSerach.DataBind()
    End Sub
#End Region

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

    Private Function GetCallPriority(ByVal callno As String, ByVal CompName As String, ByVal TaskNo As Integer) As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim intRows As Integer
            Dim SQLQuery As String

            SQLQuery = " select * from T040021 where TM_NU9_Comp_ID_FK=(select CI_NU8_Address_Number from T010011 where CI_VC36_Name='" & CompName & "' ) and TM_NU9_Task_no_PK=" & TaskNo & " and TM_NU9_Call_No_FK=" & callno & " and TM_VC8_Priority='1'"

            SQL.Search("Task_View", "GetCallPriority-2398", SQLQuery, intRows)
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
End Class
