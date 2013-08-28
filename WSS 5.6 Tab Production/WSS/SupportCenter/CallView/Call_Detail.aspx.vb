'*******************************************************************
' Page                   : - Call Detail
' Purpose                : - Whole Call Cycle will be created or edited from this page
'Tables used             : - T010011, UDC, T210011, T040021, T040031, T050011, T040041, T060011, T040011,                                 T040051, T040081, T080011
' Date					Author						Modification Date					Description
' 17/09/09			Atul/saachin/Tarun												
' Notes: The string created will request the WebService to look for processes for the initatior
' Code: 'SCR ID  = 3
'*******************************************************************
Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Data
Imports System.Drawing
Imports Telerik.Web.UI
Imports WSSBLL

'Viewstate("Update")
'Session("CallDetail")
'Viewstate("CustID")
'Viewstate("CallPriority")
'Viewstate("SortWayTask")
'Viewstate("SortOrderTask")
'Viewstate("SortWayAction")
'Viewstate("SortOrderAction")
'Session("PropCompanyID")
'Session("PropCompanyType")
'Session("PropUserID")
'Viewstate("SAddressNumber")
'Session("PropUserName")
'Viewstate("gshPageStatus")
'Viewstate("PropTaskNumer")
'Viewstate("PropTaskStatus")
'Viewstate("PropProjectID")
'Viewstate("TemplateName")
'Session("PropRootDir")
'Viewstate("CAComp")

Partial Class SupportCenter_CallView_Call_Detail
    Inherits System.Web.UI.Page
    Private objCommonFunctionsBLL As New clsCommonFunctionsBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Protected WithEvents dtgTask As New System.Web.UI.WebControls.DataGrid
    Protected WithEvents grdAction As New System.Web.UI.WebControls.DataGrid

#Region "Form level Variables"

    Private mstrCompany As String
    Public mintID As Integer
    Private mintFileID As Integer
    Private mstrFileName As String
    Private mstrFilePath As String
    Private mshFlag As Short
    Private Null As System.DBNull
    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments
    Private mTaskRowValue As Integer
    Private mActionRowValue As Integer
    Private shUpdateSave As Short
    Public introwvalues As String
    Public strhiddenTable As String

    Public mintPageSize As Integer
    Public mstrCallNumber As String
    Public mstrTaskNumber As String
    Public mstrTaskStatus As String
    Private tclTask() As TemplateColumn
    Private tclAction() As TemplateColumn
    Private Shared dtvTask As New DataView
    Private Shared arrHeadersTask As New ArrayList
    Private Shared arrFooterTask As New ArrayList
    Private Shared arrColumnsNameTask As New ArrayList
    Private Shared arrWidthTask As New ArrayList
    Private Shared arrColumnsWidthTask As New ArrayList
    'Action grid variables
    Private Shared arrHeadersAction As New ArrayList
    Private Shared arrFooterAction As New ArrayList
    Private Shared dtvAction As New DataView
    'Private Shared dtvActionMain As New DataView
    Private Shared arrColumnsNameAction As New ArrayList
    Private Shared arrWidthAction As New ArrayList
    Private Shared arrColumnsWidthAction As New ArrayList
    Private mintUserID As Integer
    Private Shared mflgTemplateValueFilled As Boolean
    Public Shared mflgChangeTemplate As Boolean
    Private Shared mintCallNoPlace As Integer
    Private Shared mstrPrvStatus As String
    Dim intFlag As Byte 'Flag to check State of Action
    Private boolRequireCallDataRefresh As Boolean '-- Will store data whether the call is saved successfully or not and is further used for calling filldata of call
    '-- Will keep previous search string of task grid. This is required to close the action panel when some search is performed on task grid
    Private Shared mTaskPrvSearchString As String = " "

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            lstError.Items.Clear()

            If Request.QueryString("ScreenFrom") = "HomePage" Then
                imgClose.Visible = False
            End If

            If Request.QueryString("CallNo") <> "" And Request.QueryString("CompID") <> "" Then
                ViewState("CallNo") = Request.QueryString("CallNo").ToString()
                If IsNumeric(Request.QueryString("CompID")) = True Then
                    ViewState("CompanyID") = Request.QueryString("CompID")
                Else
                    ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
                End If
            End If


            If Not Page.IsPostBack Then
                ViewState("Update") = Server.UrlEncode(System.DateTime.Now.ToString())   'to store value in session to stop f5 duplicate data while pressing f5 in data entry
                If Request.QueryString("CallNumber") <> "" Then
                    ViewState("CallNo") = Request.QueryString("CallNumber").ToString()
                    If IsNumeric(Request.QueryString("CompID")) = True Then
                        ViewState("CompanyID") = Request.QueryString("CompID")
                    Else
                        ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
                    End If
                End If
            End If
            'ViewState("existingCallNumber") = txtCallNumber.Text.Trim
            If IsPostBack Then
                If Request.QueryString("ID") = 0 Then
                    If Not IsNothing(Request.Form("txthiddenImage")) Then
                        If Request.Form("txthiddenImage") = "GoToCall" Then
                            If IsNumeric(txtCallNumber.Text.Trim) = False Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Please enter valid call number...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Else
                                If CheckCallExists(Val(txtCallNumber.Text.Trim)) = True Then
                                    'ViewState("CallNo") = txtCallNumber.Text.Trim
                                    'ViewState("TaskNo") = 0
                                    '--Dependency
                                    FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                                    ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:GoToCall(" & txtCallNumber.Text.Trim & ");</script>", False)
                                    ' txtCallNumber.Text = ViewState("existingCallNumber") 'Request.QueryString("CallNumber")
                                    'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "myScript", "<script>javascript:GoToCall();</script>", False)
                                Else
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Call # " & txtCallNumber.Text.Trim & " doesn't exists under selected customer...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                                End If
                            End If
                        End If
                    End If
                Else
                    If Not IsNothing(Request.Form("txthiddenImage")) Then
                        If Request.Form("txthiddenImage") = "GoToCall" Then
                            If IsNumeric(txtCallNumber.Text.Trim) = False Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Please enter valid call number...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Else
                                If CheckCallExists(Val(txtCallNumber.Text.Trim)) = True Then
                                    ViewState("CompanyID") = Val(DDLCustomer.SelectedValue)
                                    'ViewState("CallNo") = Val(txtCallNumber.Text.Trim)
                                    'ViewState("TaskNo") = 0
                                    ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:GoToCall(" & txtCallNumber.Text.Trim & ");</script>", False)

                                    'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "myScript", "<script>javascript:GoToCall();</script>", False)
                                    'Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=4&CallNumber=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "", False)
                                    Exit Sub
                                Else
                                    'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "myScript", "<script>javascript:varGlobal=0;</script>", False)
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Call # " & txtCallNumber.Text.Trim & " doesn't exists under selected customer...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If Not IsPostBack Then
                txtCSS(Me.Page)
                ViewState("CustID") = Nothing
                ViewState("CallPriority") = Nothing
                CDDLCauseCode.Enabled = False

                ViewState("SortWayTask") = Nothing
                ViewState("SortOrderTask") = Nothing
                ViewState("SortWayAction") = Nothing
                ViewState("SortOrderAction") = Nothing
                ''''ADDED all attributs on contols
                Call ADDAttributs()
                '================================
            Else
                Call txtCSS(Me.Page, "cpnlCallTask", "cpnlTaskAction")
            End If
            'Maintain the state of the DDL for Agreement and Task Owner which are filled by AJAX
            If IsPostBack Then
                FillAjaxDropDown(DDLAgreement, Request.Form("txtAgreementHID"), "cpnlCallView$" & DDLAgreement.ID, New ListItem("", ""))
                'FillAjaxDropDown(DDLTaskOwner, Request.Form("txtTaskOwnerHID"), "cpnlCallTask$" & DDLTaskOwner.ID, New ListItem("", ""))
                FillAjaxDropDown(DDLCoordinator, Request.Form("txtProjectHID"), "cpnlCallView$" & DDLCoordinator.ID, New ListItem("", ""))
            Else
                DDLAgreement.Items.Clear()
                DDLAgreement.Items.Add("")
                DDLCoordinator.Items.Clear()
                DDLCoordinator.Items.Add("")
            End If
            If IsNothing(ViewState("CompanyID")) Or (Request.QueryString("ID") = -1 And IsPostBack = False) Then
                ViewState("CompanyID") = Session("PropCompanyID")
            End If
            '--Customer
            If IsPostBack = False Then
                If Session("PropCompanyType") = "SCM" Then
                    FillNonUDCDropDown(DDLCustomer, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") order by Name")
                Else
                    DDLCustomer.Items.Add(New ListItem(WSSSearch.SearchCompNameID(ViewState("CompanyID")).ExtraValue, ViewState("CompanyID")))
                    DDLCustomer.Enabled = False
                End If
                ' -- when the first page loads and in Add Mode then default Customer should be automatically selected
                'If DDLCustomer.Items.Count > 0 And ViewState("CallNo") = -1 Then
                If DDLCustomer.Items.Count > 0 And ViewState("CallNo") <= 0 Then
                    DDLCustomer.SelectedValue = Session("PropCompanyID")
                    ViewState("CompanyID") = DDLCustomer.SelectedValue
                End If
                'Fill Template Type dropdown
                Dim strSQL As String = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='TMPL'  and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='TMPL'  and UDC.Company=0 Order By Name"
                FillTemplTypeDropDown(DDLTemplType, strSQL, True)
            End If

            If IsPostBack = False Then
                FillCustomDDl()
                FillNonUDCDropDown(DDLProject, "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and PR_Vc8_Status='Enable' order by PR_VC20_Name ", True)
                'Delete Temporary Attachments
                Try
                    Dim dsTempAttach As New DataSet
                    If SQL.Search("T040041", "CallDetails", "Page_Load-222", "select * from T040041 where AT_IN4_Level=1 and AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")), dsTempAttach, "", "") = True Then
                        For intI As Integer = 0 To dsTempAttach.Tables(0).Rows.Count - 1
                            Dim objFile As New System.IO.FileInfo(Server.MapPath(dsTempAttach.Tables(0).Rows(intI).Item("AT_VC255_File_Path")))
                            If objFile.Exists = True Then
                                objFile.Delete()
                            End If
                            objFile = Nothing
                        Next
                        SQL.Delete("CallDetails", "Page_Load-287", "DELETE from T040041 where AT_IN4_Level=1 and AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")), SQL.Transaction.Serializable)
                    End If
                Catch ex As Exception
                End Try
            End If
            DDLProject.Attributes.Add("OnChange", "ProjectChange('" & DDLProject.ClientID & "','" & DDLAgreement.ClientID & "','" & DDLTaskOwner.ClientID & "','" & DDLCoordinator.ClientID & "','" & Val(DDLCustomer.SelectedValue) & "')")
            Dim strFilter As String     ' Used to Store Where Condition for Task grid
            Dim strSearch As String    ' Used to store Value of Search Control
            Dim strSqlFilterAction As String    ' Used to Store Where Condition for Task grid
            Dim strSearchControlAction As String   ' Used to store Value of Search Control
            Dim intCount As Int32 ' Used as counter in loops
            Dim lcTask As New LiteralControl ' Used for displaying labels inplace of Task Grid 
            Dim lcAction As New LiteralControl ' Used for displaying labels inplace of Action Grid
            If Request.QueryString("ID") = -1 Then
                If IsDate(txtCallDate.Text) = False Then
                    txtCallDate.Text = SetDateFormat(Now, mdlMain.IsTime.WithTime)
                End If
            End If
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1
            strFilter = " "
            strSqlFilterAction = " "
            mActionRowValue = 0
            mTaskRowValue = 0
            If IsPostBack = False Then
                Try
                    shUpdateSave = Request.QueryString("CV")
                    ViewState("TaskNo") = 0
                    ViewState("ActionNo") = 0
                    If mintID = -1 Then
                        ViewState("SAddressNumber") = 0
                        If IsDate(txtCallDate.Text) = False Then
                            txtCallDate.Text = SetDateFormat(Now, mdlMain.IsTime.WithTime)
                        End If
                    End If
                    ' -- if page is called from search, then task no. will come in query string "SEARTASKNO"
                    ViewState("TaskNo") = Val(Request.QueryString("SEARTASKNO"))
                    '-------------------------------------------
                    txtCallBy.Value = Session("PropUserID")
                    txtCallByName.Text = Session("PropUserName")
                    mflgTemplateValueFilled = True
                Catch ex As Exception
                    CreateLog("Call_Detail", "Load-276", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
                End Try
            Else
            End If
            If shUpdateSave = 1 Then
                ViewState("gshPageStatus") = 0
            End If
            mTaskRowValue = 0
            '************Changed by amit************
            strhiddenTable = Request.Form("txthiddenTable")
            introwvalues = Request.Form("txtrowvalues")
            mstrCallNumber = ViewState("CallNo")
            mstrTaskNumber = ViewState("PropTaskNumer")
            mstrTaskStatus = ViewState("PropTaskStatus")
            If strhiddenTable = "cpnlCallTask_dtgTask" Then
                ' Clear all textBoxes in fastentry if Task no. is changed and currently we have clicked on Task grid
                If Val(Request.Form("txthiddenSkil")) <> 0 And Val(ViewState("TaskNo")) <> Val(Request.Form("txthiddenSkil")) Then
                    ClearAllTextBox(cpnlTaskAction)
                End If
                ViewState("TaskNo") = Val(Request.Form("txthiddenSkil"))
                mstrCallNumber = ViewState("TaskNo")
                mstrTaskNumber = ViewState("TaskNo")
            Else
                ViewState("ActionNo") = Val(Request.Form("txthiddenSkil"))
                mstrCallNumber = ViewState("ActionNo")
            End If
            If ViewState("CallNo") > 0 Then
                imgComment.Attributes.Add("onclick", "return KeyImage(" & ViewState("CallNo") & ", '', 'C','0'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
            Else
                imgComment.Attributes.Add("onclick", "return false;")
            End If
            '***********************************************
            mshFlag = 0
            If ViewState("gshPageStatus") = 1 Then
                mintID = 0
            Else
                mintID = Request.QueryString("ID")
            End If
            If mintID = -1 Then
                CDDLStatus.CDDLSetSelectedItem("OPEN")
                CDDLStatus.Enabled = False
                ViewState("ActionNo") = 0
                ViewState("TaskNo") = 0
                ViewState("CallNo") = 0
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallView.Text = "Call View &nbsp;&nbsp;"
            Else
                cpnlCallView.Text = "Call View &nbsp;" & "  " & " (Call# " & ViewState("CallNo") & ")"
            End If
            Dim txthiddenImage As String = Request.Form("txthiddenImage")
            Call CreateDataTableTask(strFilter, False)
            Call CreateGridTask()
            Call FillHeaderArrayTask()
            Call FillFooterArrayTask()
            Call createTemplateColumnsTask()
            Call CreateDataTableAction(strSqlFilterAction)
            Call CreateGridAction()
            Call FillHeaderArrayAction()
            Call FillFooterArrayAction()
            Call createTemplateColumnsAction()
            If mintID = -1 Then
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                If ViewState("CallNo") < 1 Then
                    cpnlCallTask.Enabled = False
                    cpnlTaskAction.Enabled = False
                Else
                    cpnlCallTask.Enabled = True
                End If
                cpnlCallTask.TitleCSS = "test2"
                cpnlTaskAction.TitleCSS = "test2"
            End If
            If txthiddenImage <> "Save" And txthiddenImage <> "OK" Then ' if Save is not called then set flag to true to call filldata
                boolRequireCallDataRefresh = True
            End If
            'ADD select case in function
            If txthiddenImage <> "" Then
                Call ChkSelectValueFromContols(txthiddenImage)
            End If
            'hide the fast entry for action if teh status of the task is closed
            Dim strTS As String = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
            ViewState("PropTaskStatus") = strTS
            If strTS = "CLOSED" Then
                PnlAction.Visible = False
            Else
                'Hide the action fast entry when client company logins
                If IsPostBack Then
                    If Session("PropCompanyType") = "SCM" Then
                        PnlAction.Visible = True
                        If Val(ViewState("TaskNo")) > 0 Then
                            Dim intTaskOwner As Integer = SQL.Search("Call Detail", "Load-1086", "select TM_VC8_Supp_Owner from T040021 where TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Task_No_PK=" & Val(ViewState("TaskNo")))
                            If Val(Session("PropUserID")) = intTaskOwner Then
                                PnlAction.Visible = True
                                's dtActionDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
                            Else
                                PnlAction.Visible = False
                            End If
                        End If
                    Else
                        PnlAction.Visible = False
                    End If
                End If
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
                imgAttachments.ToolTip = "Call Not Saved Yet"
            End If
            If Val(ViewState("CallNo")) > 0 Then
                imgCloseTask.ToolTip = "Close Selected Task"
            Else
                imgCloseTask.ToolTip = "Select a Task to Close"
            End If
            '**************************************************************
            strFilter = ""
            If mintID = 0 Or ViewState("CallNo") > 0 Then
                Call FillCallDetailControlsValue(txthiddenImage)
            End If
            ' Add new records
            If mintID = -1 Then
                Call CreateDataTableTask(strFilter, False)
                Call BindGridTask()
                Call CreateDataTableAction(strSqlFilterAction)
                Call BindGridAction()
            End If
            Try
                If Page.IsPostBack Then
                    '-- Preparing Search  String (strFilter) for Task Grid
                    For intCount = 3 To dtvTask.Table.Columns.Count - 1
                        strSearch = Request.Form("cpnlCallTask$dtgTask$ctl01$" + dtvTask.Table.Columns(intCount).ColumnName + "_H")
                        If IsNothing(strSearch) = False Then
                            If Not IsDBNull(strSearch) Then
                                If Not strSearch.Trim.Equals("") Then
                                    ' -- Format Search String
                                    strSearch = mdlMain.GetSearchString(strSearch)
                                    If dtvTask.Table.Columns(intCount).DataType.FullName = "System.Decimal" Or dtvTask.Table.Columns(intCount).DataType.FullName = "System.Int32" Or dtvTask.Table.Columns(intCount).DataType.FullName = "System.Int16" Or dtvTask.Table.Columns(intCount).DataType.FullName = "System.Double" Then
                                        strSearch = strSearch.Replace("*", "")
                                        strFilter = strFilter & dtvTask.Table.Columns(intCount).ColumnName & " = '" & strSearch & "' AND "
                                    Else
                                        If strSearch.Contains("*") = True Then
                                            strSearch = strSearch.Replace("*", "%")
                                        Else
                                            strSearch &= "%"
                                        End If
                                        strFilter = strFilter & dtvTask.Table.Columns(intCount).ColumnName & " like " & "'" & strSearch & "' AND "
                                    End If
                                End If
                            End If
                        End If
                    Next
                    ' -----------------------------------------------------------------
                    '-- Preparing Filter query String for Action Grid
                    If ViewState("TaskNo") > 0 Then
                        For intCount = 2 To dtvAction.Table.Columns.Count - 1
                            strSearchControlAction = Request.Form("cpnlTaskAction$grdAction$ctl01$" + dtvAction.Table.Columns(intCount).ColumnName + "_H")
                            If IsNothing(strSearchControlAction) = False Then
                                If Not strSearchControlAction.Trim.Equals("") And Not IsDBNull(strSearchControlAction) Then
                                    strSearchControlAction = mdlMain.GetSearchString(strSearchControlAction)
                                    If dtvAction.Table.Columns(intCount).DataType.FullName = "System.Decimal" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Int32" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Int16" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Double" Then
                                        strSearchControlAction = strSearchControlAction.Replace("*", "")
                                        strSqlFilterAction = strFilter & dtvAction.Table.Columns(intCount).ColumnName & " = '" & strSearchControlAction & "' AND "
                                    Else
                                        If strSearchControlAction.Contains("*") = True Then
                                            strSearchControlAction = strSearchControlAction.Replace("*", "%")
                                        Else
                                            strSearchControlAction &= "%"
                                        End If
                                        strSqlFilterAction = strSqlFilterAction & dtvAction.Table.Columns(intCount).ColumnName & " like " & "'" & strSearchControlAction & "' AND "
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
                If Not strFilter.Trim.Equals("") Then
                    strFilter = strFilter.Remove((strFilter.Length - 4), 4)
                End If
                If ViewState("TaskNo") > 0 Then
                    If Not strSqlFilterAction.Trim.Equals("") Then
                        strSqlFilterAction = strSqlFilterAction.Remove((strSqlFilterAction.Length - 4), 4)
                    End If
                End If
                If ViewState("CallNo") > 0 Then
                    If SaveTask() = True Then    ' Save Task Info 
                        txtTotalEstimatedHours.Text = SQL.Search("", "", "select CM_FL8_Total_Est_Time from T040011 where CM_NU9_Comp_Id_FK=" & Val(ViewState("CompanyID")) & " and CM_NU9_Call_No_PK=" & Val(ViewState("CallNo")))
                        Call ClearAllTextBox(cpnlCallTask)
                        TxtSubject_F.Height = Unit.Pixel(18)
                    Else
                        If Val(Request.Form("txtHIDSize")) > 0 And TxtSubject_F.Text.Length > 0 Then
                            TxtSubject_F.Height = Unit.Pixel(Val(Request.Form("txtHIDSize")))
                        End If
                    End If
                End If
                If ViewState("CallNo") > 0 Then
                    If SaveAction() = True Then
                        Call ClearAllTextBox(cpnlTaskAction)
                    End If
                End If
                Call CreateDataTableTask(strFilter, True)
                Call BindGridTask()
                If Not IsPostBack Then
                    FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                End If
                Call CreateDataTableAction(strSqlFilterAction)
                Call BindGridAction()
            Catch ex As Exception
                CreateLog("Call_Detail", "Load-758", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            End Try
            TxtActionOwner_F.Value = HttpContext.Current.Session("PropUserId")
            CDDLActionOwner_F.CDDLSetSelectedItem(Session("PropUserId"), False, Session("PropUserName"))
            'hide the fast entry for task if teh status of the call is closed
            Dim strCallStatus As String = WSSSearch.GetCallStatus(ViewState("CallNo"), ViewState("CompanyID"))
            Session("PropCallStatus") = strCallStatus
            If strCallStatus = "CLOSED" Or CDDLStatus.CDDLGetValueName = "CLOSED" Then
                pnlTask.Visible = False
            Else
                pnlTask.Visible = True
            End If
            'If the status of the call is not OPEN then user cannot change the Agreement and Project of the call
            'By Harpreet
            If CDDLStatus.CDDLGetValueName <> "OPEN" And CDDLStatus.CDDLGetValueName <> "DEFAULT" Then
                DDLAgreement.Enabled = False
                DDLProject.Enabled = False
            End If
            If Val(DDLAgreement.SelectedValue) = 0 And CDDLStatus.CDDLGetValue <> "CLOSED" Then
                DDLAgreement.Enabled = True
            ElseIf Val(DDLAgreement.SelectedValue) = 0 And CDDLStatus.CDDLGetValue <> "OPEN" Then
                DDLAgreement.Enabled = False
            End If
            ''-------------------------------------------------------------------------------------
            'if Call detail screen open through Search Module then Close button will not be Visible
            'Amit
            If Request.QueryString("SearchID") = 1 Then
                'ImgClose.Visible = False
                Logout.Visible = False
            End If
            ''-------------------------------------------------------------------------------------
            If dtEstCloseDate.Text = "" Then
                dtEstCloseDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
            End If
            If dtEstStartDate.Text = "" Then
                dtEstStartDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
            End If
            If IsPostBack Then
                If Val(ViewState("TaskNo")) > 0 Then
                    Dim dgi As DataGridItem
                    For Each dgi In dtgTask.Items
                        If Val(CType(dgi.Cells(4).FindControl("TM_NU9_Task_no_PK"), Label).Text) = Val(ViewState("TaskNo")) Then
                            dgi.BackColor = Color.FromArgb(212, 212, 212)
                            cpnlTaskAction.Text = "Action View (Task# " & ViewState("TaskNo") & " &nbsp;&nbsp;Company:" & DDLCustomer.SelectedItem.Text & ")"
                            cpnlTaskAction.TitleCSS = "test"
                            cpnlTaskAction.Enabled = True
                            cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                            Exit For
                        Else
                            If dgi.ItemIndex Mod 2 = 0 Then
                                dgi.BackColor = Color.FromArgb(255, 255, 255)
                            Else
                                dgi.BackColor = Color.FromArgb(245, 245, 245)
                            End If
                            cpnlTaskAction.Text = "Action View "
                            cpnlTaskAction.TitleCSS = "test2"
                            cpnlTaskAction.Enabled = False
                            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                        End If
                    Next
                End If
            Else
                cpnlTaskAction.Text = "Action View "
                cpnlTaskAction.TitleCSS = "test2"
                cpnlTaskAction.Enabled = False
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            End If
            If Val(ViewState("TaskNo")) = 0 Or dtvTask.Table.Rows.Count = 0 Then
                cpnlTaskAction.Text = "Action View "
                cpnlTaskAction.TitleCSS = "test2"
                cpnlTaskAction.Enabled = False
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            End If
            ' -- Security Block
            Dim intId As Integer
            '     If Not IsPostBack Then 'This is a fake block for executing security because visibility of controls is changing in programming 
            Dim strPath As String
            strPath = Session("PropRootDir")
            If Request.QueryString("ID") = 0 Then
                intId = 939
            Else
                intId = 3
            End If
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, intId)
        Catch ex As Exception
            CreateLog("Call_Detail", "Page_Load-1086", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Private Sub ADDAttributs()
        imgCalls.Attributes.Add("OnClick", "return OpenCalls();")
        TxtEstimatedHrs.Attributes.Add("onkeypress", "UsedHour('cpnlCallTask_TxtEstimatedHrs')")
        imgbtnSearch.Attributes.Add("onclick", "return CheckLength();")
        btnGoToCall.Attributes.Add("OnClick", "return GoToCallCheck();")
        'txtCallNumber.Attributes.Add("onkeypress", "return GoToCallKeyPress();")
        DDLCustomer.Attributes.Add("OnChange", "CompanyChange();")
        'TxtUsedHr_F.Attributes.Add("onkeypress", "NumericOnly();")
        TxtUsedHr_F.Attributes.Add("onkeypress", "UsedHour1('cpnlTaskAction_TxtUsedHr_F')")
        TxtTmplName.Attributes.Add("ReadOnly", "ReadOnly")
        TxtTmplId.Attributes.Add("ReadOnly", "ReadOnly")
        ImgMail.Attributes.Add("OnClick", "return SaveEdit('SendMail');")
        imgCloseTask.Attributes.Add("OnClick", "return SaveEdit('CloseTask');")

        txtDescription.Attributes.Add("onmousemove", "ShowToolTip(this,2000);")
        TxtSubject_F.Attributes.Add("onmousemove", "ShowToolTip(this,1000);")
        TxtTmplName.Attributes.Add("style", "cursor:hand")
        TxtSubject_F.Attributes.Add("onkeypress", "ChangeHeight(this.value,this.id)")
        TxtSubject_F.Attributes.Add("onkeyup", "ChangeHeight(this.value,this.id)")

        'txtDescription.Attributes.Add("onkeypress", "ChangeHeight(this.value,this.id)")
        'txtDescription.Attributes.Add("onkeyup", "ChangeHeight(this.value,this.id)")

        TxtSubject_F.Attributes.Add("onkeydown", "ChangeHeight(this.value,this.id)")
        TxtTmplName.Attributes.Add("onClick", "OpenTMPL('" & TxtTmplName.ClientID & "','" & DDLTemplType.ClientID & "');")
        imgTemplate.Attributes.Add("onClick", "return OpenTMPL('" & TxtTmplName.ClientID & "','" & DDLTemplType.ClientID & "');")
        DDLTemplType.Attributes.Add("OnChange", "OpenTMPL('" & TxtTmplName.ClientID & "','" & DDLTemplType.ClientID & "');")

        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
        'ImgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgFWD.Attributes.Add("Onclick", "return SaveEdit('Fwd');")
        imgAttachments.Attributes.Add("Onclick", "return SaveEdit('Attach');")
        Logout.Attributes.Add("Onclick", "return LogoutWSS();")
    End Sub

    Private Sub ChkSelectValueFromContols(ByVal txthiddenImage As String)

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Swap"
                        Dim intCallNoTemp As Integer = ViewState("CallNo")
                        ViewState("CallNo") = Val(txtCallRef.Text.Trim)
                        Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=7&CCallNo=" & intCallNoTemp, False)
                    Case "Fwd"
                    Case "Edit"
                        If strhiddenTable = "cpnlCallTask_dtgTask" Then
                            Exit Select
                        End If
                        'Send Mail
                    Case "SendMail"
                        SendMail()
                    Case "Close"
                        ViewState("gshPageStatus") = 0
                        If Request.QueryString("PageID") = 1 Then
                            Response.Redirect("Call_View.aspx?ScrID=4", False)
                        ElseIf Request.QueryString("PageID") = 2 Then
                            Response.Redirect("Task_View.aspx", False)
                        ElseIf Request.QueryString("PageID") = 3 Then
                            Response.Redirect("../../WorkCenter/DoList/ToDoList.aspx?ScrID=8", False)
                        ElseIf Request.QueryString("PageID") = 4 Then
                            Response.Redirect("../../Home.aspx", False)
                        ElseIf Request.QueryString("PageID") = 5 Then ' to simple call view screen
                            Response.Redirect("CallView_Simple.aspx?ScrID=799", False)
                        ElseIf Request.QueryString("PageID") = 6 Then
                            Response.Redirect("../../WorkCenter/DoList/WorkView.aspx?ScrID=536", False)
                        ElseIf Request.QueryString("PageID") = 7 Then
                            ViewState("CallNo") = Val(Request.QueryString("CCallNo"))
                            Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=4", False)
                        ElseIf Request.QueryString("PageID") = 15 Then
                            Response.Redirect("../../NewCall/CallView/Callview_simple.aspx?ScrID=1024", False)
                        Else
                            Response.Redirect("Call_View.aspx?ScrID=4", False)
                        End If

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
                    Case "Add"
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            Exit Sub
                        End If
                        'End of Security Block
                        Dim shRedirect As Short
                        If mshFlag = 1 Then
                            Exit Select
                        Else
                            If mintID = -1 Then
                                If SaveCall() = True Then
                                    shRedirect = 1
                                End If
                            Else
                                If UpdateCall() = True Then
                                    shRedirect = 1
                                End If
                            End If
                            If SaveTask() = True Then
                                lstError.Items.Clear()
                                DisplayMessage("Records Saved Successfully...")
                                If Request.QueryString("PageID") = 1 Then
                                    Response.Redirect("Call_View.aspx?ScrID=4", False)
                                ElseIf Request.QueryString("PageID") = 2 Then
                                    Response.Redirect("Task_View.aspx", False)
                                ElseIf Request.QueryString("PageID") = 5 Then ' to simple call view screen
                                    Response.Redirect("CallView_Simple.aspx?ScrID=799", False)
                                Else
                                    Response.Redirect("Call_View.aspx?ScrID=4", False)
                                End If
                            Else
                                If shRedirect = 1 Then
                                    If Request.QueryString("PageID") = 1 Then
                                        ' Response.Redirect("Call_View.aspx?ScrID=4", False)
                                        ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:tabClose();</script>", False)
                                    ElseIf Request.QueryString("PageID") = 2 Then
                                        'Response.Redirect("Task_View.aspx", False)
                                        ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:tabClose();</script>", False)
                                    ElseIf Request.QueryString("PageID") = 5 Then ' to simple call view screen
                                        ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:tabClose();</script>", False)
                                        'Response.Redirect("CallView_Simple.aspx?ScrID=799", False)
                                    Else
                                        ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:tabClose();</script>", False)
                                        'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "abcd", "javascript:return tabClose();", True)
                                        ' Response.Redirect("Call_View.aspx?ScrID=4", False)
                                    End If
                                End If
                                Exit Select
                            End If
                        End If

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            Exit Sub
                        End If
                        'End of Security Block

                        If mintID = -1 Then
                            If SaveCall() = False Then
                                Exit Select
                                boolRequireCallDataRefresh = False
                            Else
                                boolRequireCallDataRefresh = True
                                cpnlCallTask.TitleCSS = "test"
                                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                                cpnlCallView.Text = "Call View &nbsp;" & "  " & " (Call# " & ViewState("CallNo") & ")"
                            End If
                        Else
                            If UpdateCall() = False Then
                                Exit Select
                                boolRequireCallDataRefresh = False
                            Else
                                boolRequireCallDataRefresh = True
                            End If
                        End If

                        If SaveTask() = True Then
                            TxtSubject_F.Height = Unit.Pixel(18)
                            lstError.Items.Clear()
                            lstError.Items.Add("Records Saved Successfully...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        Else
                            If Val(Request.Form("txtHIDSize")) > 0 And TxtSubject_F.Text.Length > 0 Then
                                TxtSubject_F.Height = Unit.Pixel(Val(Request.Form("txtHIDSize")))
                            End If
                        End If

                    Case "Delete"
                        '**************************************
                        If strhiddenTable = "cpnlCallTask_dtgTask" Then
                            'get the status of the task becuase only ASSIGNED tasks can be deleted
                            Dim strTaskStatus As String
                            strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                            mstrTaskStatus = strTaskStatus
                            If strTaskStatus = "ASSIGNED" Then
                                ' Delete TASK
                                mstGetFunctionValue = WSSDelete.DeleteTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                                '--Dependency
                                FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                                If mstGetFunctionValue.ErrorCode = 0 Then
                                    '''''''''''Rollback Call Status'''''''''''
                                    Dim intRows As Integer
                                    If SQL.Search("Call_Detail", "Load-506", "select * from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                        Dim arrCallStatusColUpdate As New ArrayList
                                        Dim arrCallStatusRowUpdate As New ArrayList
                                        arrCallStatusColUpdate.Add("CN_VC20_Call_Status")
                                        arrCallStatusRowUpdate.Add("OPEN")
                                        WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arrCallStatusColUpdate, arrCallStatusRowUpdate)
                                        DDLProject.Enabled = True
                                        DDLAgreement.Enabled = True
                                    End If
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Task Deleted successfully...")
                                    txthidden.Value = ""
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                    introwvalues = 0
                                    mstrCallNumber = 0
                                ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Server is busy please try later...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If
                            Else
                                lstError.Items.Clear()
                                lstError.Items.Add("This task is  " & strTaskStatus & " so it cannot be deleted...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            End If
                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            'Updated 10/06/08
                            ' To calculate Estimate Hours if any Task Deleted
                            'call function to update estimate hours
                            Call WSSUpdate.UpdateCallHours(Val(ViewState("CompanyID")), Val(ViewState("CallNo")))
                            'show the new estimated hours after task is deleted
                            txtTotalEstimatedHours.Text = SQL.Search("", "", "select CM_FL8_Total_Est_Time from T040011 where CM_NU9_Comp_Id_FK=" & Val(ViewState("CompanyID")) & " and CM_NU9_Call_No_PK=" & Val(ViewState("CallNo")))
                        Else
                            ' Delete ACTION
                            'get the status of the task 
                            Dim strTaskStatus As String
                            strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))

                            If strTaskStatus = "CLOSED" Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Action cannot be deleted for a Closed Task...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            Else
                                mstGetFunctionValue = WSSDelete.DeleteAction(ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"), ViewState("CompanyID"))
                                If mstGetFunctionValue.ErrorCode = 0 Then
                                    '''''''''''Rollback Task Status'''''''''''
                                    Dim intRows As Integer
                                    If SQL.Search("Call_Detail", "load-541", "select * from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Task_Number=" & ViewState("TaskNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
                                        Dim arrTaskStatusColUpdate As New ArrayList
                                        Dim arrTaskStatusRowUpdate As New ArrayList
                                        arrTaskStatusColUpdate.Add("TM_VC50_Deve_status")
                                        arrTaskStatusRowUpdate.Add("ASSIGNED")
                                        WSSUpdate.UpdateTask(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"), arrTaskStatusColUpdate, arrTaskStatusRowUpdate)
                                    End If
                                    '''''''''''Rollback Call Status if there is no action left behind the call'''''''''''
                                    If SQL.Search("Call_Detail", "Load-552", "select * from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID"), intRows) = False Then
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
                                    mstrCallNumber = 0
                                ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                    lstError.Items.Add("Server is busy please try later...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If
                            End If
                        End If
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            Catch ex As Exception
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("Call_Detail", "Load-548", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            End Try
        End If
    End Sub

    Private Sub FillCallDetailControlsValue(ByVal txthiddenImage As String)
        Try
            mstrCompany = Session("PropCompanyID")
            cpnlCallTask.Enabled = True
            cpnlCallTask.TitleCSS = "test"
            Dim sqrdCall As SqlDataReader
            mstGetFunctionValue = WSSSearch.SearchCall(ViewState("CallNo"), ViewState("CompanyID"), sqrdCall)
            ' -- When Call data refresh is required and it is not a forced postback due to selection of Project
            If mstGetFunctionValue.ErrorCode = 0 And boolRequireCallDataRefresh = True And txthiddenImage <> "forced" Then
                While sqrdCall.Read
                    DDLCustomer.Enabled = False
                    'store the cust id, will be used whiile updating call
                    txtCallNumber.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Call_No_PK")), 0, sqrdCall.Item("CM_NU9_Call_No_PK"))
                    'ViewState("existingCallNumber") = txtCallNumber.Text.Trim
                    ViewState("CustID") = IIf(IsDBNull(sqrdCall.Item("CM_NU9_CustID_FK")), 0, sqrdCall.Item("CM_NU9_CustID_FK"))
                    txtCallBy.Value = IIf(IsDBNull(sqrdCall.Item("CM_VC100_By_Whom")), "", sqrdCall.Item("CM_VC100_By_Whom"))
                    txtCallByName.Text = WSSSearch.SearchUserID(txtCallBy.Value.Trim).ExtraValue
                    'CDDLCallOwner.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CM_NU9_Call_Owner")), "", sqrdCall.Item("CM_NU9_Call_Owner")), False, sqrdCall.Item("CI_VC36_Name"))
                    CDDLCallOwner.SelectedValue = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Call_Owner")), "", sqrdCall.Item("CM_NU9_Call_Owner"))
                    txtCateCode1.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Category_Code_1")), "", sqrdCall.Item("CM_NU9_Category_Code_1"))
                    txtCateCode2.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Category_Code_2")), "", sqrdCall.Item("CM_NU9_Category_Code_2"))
                    CDDLCallCategory.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CM_VC8_Call_Category")), "", sqrdCall.Item("CM_VC8_Call_Category")), True, IIf(IsDBNull(sqrdCall.Item("CM_VC8_Call_Category")), "", sqrdCall.Item("CM_VC8_Call_Category")))
                    CDDLCategory.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CM_VC8_Category")), "", sqrdCall.Item("CM_VC8_Category")), True, IIf(IsDBNull(sqrdCall.Item("CM_VC8_Category")), "", sqrdCall.Item("CM_VC8_Category")))
                    CDDLCauseCode.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CM_VC8_Cause_Code")), "", sqrdCall.Item("CM_VC8_Cause_Code")), True, IIf(IsDBNull(sqrdCall.Item("CM_VC8_Cause_Code")), "", sqrdCall.Item("CM_VC8_Cause_Code")))
                    txtCallRef.Text = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Related_Call")), "", sqrdCall.Item("CM_NU9_Related_Call"))
                    If Val(txtCallRef.Text.Trim) > 0 Then
                        txtCallRef.Attributes.Add("OnDblClick", "return SaveEdit('Swap');")
                        txtCallRef.Attributes.Add("Style", "Cursor:Hand")
                        txtCallRef.ToolTip = "Double Click to See Details of this Call"
                    End If

                    'CDDLCallType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CM_VC8_Call_Type")), "", sqrdCall.Item("CM_VC8_Call_Type")), True, IIf(IsDBNull(sqrdCall.Item("CM_VC8_Call_Type")), "", sqrdCall.Item("CM_VC8_Call_Type")))
                    CDDLCallType.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Call_Type")), "", sqrdCall.Item("CM_VC8_Call_Type"))
                    txtTotalEstimatedHours.Text = IIf(IsDBNull(sqrdCall.Item("CM_FL8_Total_Est_Time")), 0, sqrdCall.Item("CM_FL8_Total_Est_Time"))
                    txtTotalReportedHours.Text = IIf(IsDBNull(sqrdCall.Item("CM_FL8_Total_Reported_Time")), "", sqrdCall.Item("CM_FL8_Total_Reported_Time"))

                    '--Changed on 06/06/2006
                    mstGetFunctionValue = WSSSearch.SearchCompNameID(sqrdCall.Item("CM_NU9_Comp_Id_FK"))
                    DDLCustomer.SelectedValue = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Comp_Id_FK")), "", sqrdCall.Item("CM_NU9_Comp_Id_FK"))
                    txtDescription.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC2000_Call_Desc")), "", sqrdCall.Item("CM_VC2000_Call_Desc"))
                    CDDLPriority.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CM_VC200_Work_Priority")), "", sqrdCall.Item("CM_VC200_Work_Priority")), True, IIf(IsDBNull(sqrdCall.Item("CM_VC200_Work_Priority")), "", sqrdCall.Item("CM_VC200_Work_Priority")))

                    ViewState("CallPriority") = CDDLPriority.CDDLGetValue ' Store call priority to compaire next time when data will save 

                    'txtProject.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC50_Project")), "", sqrdCall.Item("CM_VC50_Project"))
                    DDLProject.SelectedValue = IIf(IsDBNull(sqrdCall("CM_NU9_Project_ID")), "", sqrdCall("CM_NU9_Project_ID"))
                    ' --  Put current Project ID in Session
                    ViewState("PropProjectID") = IIf(IsDBNull(sqrdCall("CM_NU9_Project_ID")), "", sqrdCall("CM_NU9_Project_ID"))
                    SetFieldsAfterProjectChange() ' -- Set Task owners acco. to Project
                    txtReference.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC50_Reference_Id")), "", sqrdCall.Item("CM_VC50_Reference_Id"))
                    txtSubject.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC100_Subject")), "", sqrdCall.Item("CM_VC100_Subject"))
                    If Not CStr(IIf(IsDBNull(sqrdCall.Item("CM_DT8_Close_Date")), "", sqrdCall.Item("CM_DT8_Close_Date"))).Equals("") Then
                        dtEstFinishDate.Text = SetDateFormat(IIf(IsDBNull(sqrdCall.Item("CM_DT8_Close_Date")), "", sqrdCall.Item("CM_DT8_Close_Date")), mdlMain.IsTime.DateOnly)
                    Else
                        dtEstFinishDate.Text = ""
                    End If
                    'Call start date
                    If Not CStr(IIf(IsDBNull(sqrdCall.Item("CM_DT8_Call_Start_Date")), "", sqrdCall.Item("CM_DT8_Call_Start_Date"))).Equals("") Then
                        DTCallStartDate.DbSelectedDate = IIf(IsDBNull(sqrdCall.Item("CM_DT8_Call_Start_Date")), "", sqrdCall.Item("CM_DT8_Call_Start_Date"))
                        DTCallStartDate.DateInput.DisplayDateFormat = "yyyy-MMM-dd HH:mm tt"
                    Else
                        DTCallStartDate.DbSelectedDate = System.DBNull.Value
                    End If

                    CDDLStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("CN_VC20_Call_Status")), "", sqrdCall.Item("CN_VC20_Call_Status")), True, IIf(IsDBNull(sqrdCall.Item("CN_VC20_Call_Status")), "", sqrdCall.Item("CN_VC20_Call_Status")))
                    ViewState("CallStatus") = IIf(IsDBNull(sqrdCall.Item("CN_VC20_Call_Status")), "", sqrdCall.Item("CN_VC20_Call_Status"))
                    '-----------------------------------------------------------------------------------------------------------
                    'If the status of the call is not OPEN then user cannot change the Agreement and Project of the call
                    'By Harpreet
                    If Not IsDBNull(sqrdCall.Item("CN_VC20_Call_Status")) Then
                        If sqrdCall.Item("CN_VC20_Call_Status") <> "OPEN" And CStr(sqrdCall.Item("CN_VC20_Call_Status")).ToUpper <> "DEFAULT" Then
                            DDLAgreement.Enabled = False
                            DDLProject.Enabled = False
                        End If
                    End If
                    ''-----------------------------------------------------------------------------------------------------------
                    FillNonUDCDropDown(DDLAgreement, "select AG_NU8_ID_PK as ID,AG_NU8_ID_PK Description,CI_VC36_Name 'Contact Person' from T080011 Ag,T010011 AB where ag.AG_VC8_Cust_Name =" & Val(ViewState("CompanyID")) & " and ab.CI_NU8_Address_Number = ag.AG_VC8_Contact_Person  and AG_NU9_Project_ID=" & DDLProject.SelectedValue & " and AG_VC8_Status='ACTIVE'", True)
                    If DDLTaskOwner.SelectedValue = "" Then
                        ' FillNonUDCDropDown(DDLTaskOwner, " SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,T010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where T010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(DDLProject.SelectedValue) & " and PM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name", True)
                        FillNonUDCDropDown(DDLCoordinator, " SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,T010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where T010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(DDLProject.SelectedValue) & " and PM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name", True)
                    End If
                    Try
                        DDLCoordinator.SelectedValue = IIf(IsDBNull(sqrdCall.Item("CM_NU9_Coordinator")), "", sqrdCall.Item("CM_NU9_Coordinator"))
                    Catch ex As Exception
                    End Try
                    DDLAgreement.SelectedValue = IIf(sqrdCall.Item("CM_NU9_Agreement") = 0, "", sqrdCall.Item("CM_NU9_Agreement"))
                    TxtTmplName.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Template")), "", sqrdCall.Item("CM_VC8_Template"))
                    If IsDBNull(sqrdCall.Item("CM_VC8_Tmpl_Type")) = True Then
                        DDLTemplType.SelectedValue = IIf(IsDBNull(sqrdCall.Item("CM_VC8_Tmpl_Type")), "All Types", sqrdCall.Item("CM_VC8_Tmpl_Type"))
                    Else
                        DDLTemplType.SelectedValue = IIf(sqrdCall.Item("CM_VC8_Tmpl_Type") = "", "All Types", sqrdCall.Item("CM_VC8_Tmpl_Type"))
                    End If

                    txtHIDIndex.Value = DDLTemplType.SelectedIndex
                    ViewState("TemplateName") = TxtTmplName.Text

                    If CDDLPriority_F.Text = "" Then
                        CDDLPriority_F.Text = IIf(IsDBNull(sqrdCall.Item("CM_VC200_Work_Priority")), "", sqrdCall.Item("CM_VC200_Work_Priority"))
                    End If

                    txtCallDate.Text = SetDateFormat(IIf(IsDBNull(sqrdCall.Item("CM_DT8_Request_Date")), "", sqrdCall.Item("CM_DT8_Request_Date")), mdlMain.IsTime.WithTime)
                    Dim intCommentFlag As Int16
                    SetCommentFlag(intCommentFlag, mdlMain.CommentLevel.CallLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                    Select Case intCommentFlag
                        Case 0
                            imgComment.ImageUrl = arrImageUrlDisabled(0)
                        Case 1
                            imgComment.ImageUrl = arrImageUrlEnabled(0)
                        Case 2
                            imgComment.ImageUrl = arrImageUrlNew(0)
                    End Select

                    If IsDBNull(sqrdCall.Item("CM_NU8_Attach_No")) = False Then
                        imgAddAttachment.Src = "../../Images/Attach_Yellow.gif"
                    Else
                        imgAddAttachment.Src = "../../Images/Attach15_9.gif"
                    End If

                    ' -- Storing previous Call Status
                    mstrPrvStatus = IIf(IsDBNull(sqrdCall.Item("CN_VC20_Call_Status")), "", sqrdCall.Item("CN_VC20_Call_Status"))
                    If CDDLStatus.CDDLGetValue = "CLOSED" Then ' If Call is closed then disable controls else enable
                        DisableCallInfo()
                    Else
                        EnableCallInfo()
                    End If
                End While
                sqrdCall.Close()
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
            End If

            mstGetFunctionValue = WSSSearch.SearchUserName(DDLCustomer.SelectedValue.Trim)

            If ViewState("CompanyID") = 0 Then
                ViewState("CompanyID") = DDLCustomer.SelectedValue
            End If

        Catch ex As Exception
            CreateLog("Call_Detail", "Load-639", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Function SaveCall() As Boolean
        '-----------------------------------------------
        'date 7/12/2006
        'to compare session values to stop f5 duplicate data while pressing f5 in data entry
        If ViewState("Update").ToString() = ViewState("update").ToString() Then '------------------------------

            Dim blnError As Boolean = False
            lstError.Items.Clear()


            If TxtTmplName.Text.Trim.Equals("") = False Then
                If DDLTemplType.SelectedValue.Trim.Equals("TAO") = False And (mflgTemplateValueFilled = False Or Request.Form("txtChangeTemplate") = "1") Then
                    ' -- Fill Call From Template
                    Call FillCallFromTemplate()
                End If
            End If

            If ValidateRecords() = False Then
                blnError = True
            End If

            Dim strEstDate As String
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            If Not dtEstFinishDate.Text.Trim.Equals("") Then
                If IsDate(dtEstFinishDate.Text) = False Then
                    lstError.Items.Add("Check date format of Estimated Close date...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    blnError = True
                Else
                    If CDate(Format(CDate(dtEstFinishDate.Text.Trim), "MM/dd/yyyy")) < CDate(Format(CDate(txtCallDate.Text.Trim), "MM/dd/yyyy")) Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Estimated Close date cannot be less than Call open date...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        blnError = True
                    End If
                End If
                strEstDate = CStr(dtEstFinishDate.Text.Trim)
                If strEstDate.Trim.Length < 12 Then
                    strEstDate = strEstDate & " " & Now.ToShortTimeString
                End If
            Else
                strEstDate = ""
            End If

            If blnError = True Then
                Exit Function
            End If

            Try
                arColumnName.Add("CM_NU9_Comp_Id_FK")
                arColumnName.Add("CM_VC8_Call_Type")
                arColumnName.Add("CM_DT8_Request_Date")
                arColumnName.Add("CM_NU9_Call_Owner")
                arColumnName.Add("CM_VC100_By_Whom")
                arColumnName.Add("CM_VC200_Work_Priority")
                arColumnName.Add("CM_VC50_Reference_Id")
                arColumnName.Add("CM_DT8_Close_Date")
                arColumnName.Add("CM_VC100_Subject")
                arColumnName.Add("CM_VC2000_Call_Desc")
                arColumnName.Add("CM_NU9_Project_ID")
                arColumnName.Add("CM_NU9_CustID_FK")
                arColumnName.Add("CN_VC20_Call_Status")
                arColumnName.Add("CM_VC8_Template")
                arColumnName.Add("CM_VC8_Tmpl_Type")
                arColumnName.Add("CM_NU9_Agreement")
                arColumnName.Add("CM_NU9_Coordinator")
                arColumnName.Add("CM_VC8_Category")
                arColumnName.Add("CM_VC8_Call_Category")
                arColumnName.Add("CM_VC8_Cause_Code")
                arColumnName.Add("CM_NU9_Related_Call")

                arColumnName.Add("CM_NU9_Category_Code_1")
                arColumnName.Add("CM_NU9_Category_Code_2")

                arColumnName.Add("CM_DT8_Call_Start_Date")

                arRowData.Add(DDLCustomer.SelectedValue.Trim)
                'arRowData.Add(CDDLCallType.CDDLGetValue.Trim.ToUpper)
                arRowData.Add(CDDLCallType.Text.Trim.ToUpper)
                arRowData.Add(Now)
                'arRowData.Add(CDDLCallOwner.CDDLGetValue.Trim.ToUpper)
                arRowData.Add(CDDLCallOwner.SelectedValue)
                arRowData.Add(txtCallBy.Value.Trim)
                arRowData.Add(CDDLPriority.CDDLGetValue.Trim.ToUpper)
                arRowData.Add(txtReference.Text.Trim)
                If strEstDate.Trim.Equals("") Then
                    arRowData.Add(DBNull.Value)
                Else
                    arRowData.Add(strEstDate)
                End If
                arRowData.Add(txtSubject.Text.Trim)
                arRowData.Add(txtDescription.Text.Trim)
                arRowData.Add(Val(DDLProject.SelectedValue))
                arRowData.Add(Session("PropCompanyID"))
                If TxtTmplName.Text = "" Then
                    arRowData.Add("OPEN")
                Else
                    arRowData.Add(CDDLStatus.CDDLGetValue.Trim)
                End If
                arRowData.Add(TxtTmplName.Text)
                arRowData.Add(IIf(DDLTemplType.SelectedValue = "All Types", "", DDLTemplType.SelectedValue))
                arRowData.Add(Val(DDLAgreement.SelectedValue))

                If DDLCoordinator.SelectedValue = "" Then
                    arRowData.Add(DBNull.Value)
                Else
                    arRowData.Add(DDLCoordinator.SelectedValue)
                End If

                arRowData.Add(CDDLCategory.CDDLGetValue)
                arRowData.Add(CDDLCallCategory.CDDLGetValue)
                arRowData.Add(CDDLCauseCode.CDDLGetValue)
                arRowData.Add(IIf(Val(txtCallRef.Text.Trim) > 0, Val(txtCallRef.Text.Trim), System.DBNull.Value))
                arRowData.Add(IIf(txtCateCode1.Text.Trim.Equals("") = True, System.DBNull.Value, txtCateCode1.Text.Trim))
                arRowData.Add(IIf(txtCateCode2.Text.Trim.Equals("") = True, System.DBNull.Value, txtCateCode2.Text.Trim))
                'Add Call start date new field
                If IsNothing(DTCallStartDate.SelectedDate) = False Then
                    If DTCallStartDate.SelectedDate.Equals("") = False Then
                        arRowData.Add(DTCallStartDate.SelectedDate)
                    Else
                        arRowData.Add(System.DBNull.Value)
                    End If
                Else
                    arRowData.Add(System.DBNull.Value)
                End If


                mstGetFunctionValue = WSSSave.SaveCall(arColumnName, arRowData, DDLCustomer.SelectedValue.Trim, CDDLStatus.CDDLGetValue.Trim)
                Dim shReturn As Short

                If mstGetFunctionValue.ErrorCode = 0 Then
                    txtCallNumber.Text = mstGetFunctionValue.ExtraValue
                    'ViewState("existingCallNumber") = txtCallNumber.Text.Trim
                    CDDLCauseCode.Enabled = True
                    '___________________________
                    ViewState("Update") = Server.UrlEncode(System.DateTime.Now.ToString())
                    '_____________________________________________
                    ViewState("CallNo") = mstGetFunctionValue.ExtraValue
                    ViewState("CompanyID") = DDLCustomer.SelectedValue.Trim
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    dtEstCloseDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
                    dtEstStartDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
                    imgComment.Attributes.Add("onclick", "return KeyImage(" & ViewState("CallNo") & ", '', 'C','0'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")

                    If GetFiles(mdlMain.AttachLevel.CallLevel) = True Then
                        shReturn = 1
                    Else
                        shReturn = 2
                    End If

                    ' -- For Call and Call and Task Type
                    If DDLTemplType.SelectedValue.Trim.Equals("All Types") = False And TxtTmplName.Text.Trim.Equals("") = False Then
                        If DDLTemplType.SelectedValue.Trim.Equals("TAO") = False Then
                            Dim intTemplateID As Integer
                            intTemplateID = Val(TxtTmplId.Text)
                            mstGetFunctionValue = WSSSave.SaveTemplate(TxtTmplId.Text, DDLTemplType.SelectedValue.Trim.ToUpper, Val(ViewState("CompanyID")), Val(ViewState("CallNo")))
                            If mstGetFunctionValue.ErrorCode = 0 Then
                                DisplayMessage("Call Data Saved Successfully...")

                                ViewState("SAddressNumber") = intTemplateID
                                ViewState("gshPageStatus") = 1
                                cpnlTaskAction.Enabled = True
                                mintID = 0
                            Else
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            End If
                        End If
                    End If

                    ' -- For Task only type
                    If DDLTemplType.SelectedValue.Trim.Equals("") = False And TxtTmplName.Text.Trim.Equals("") = False Then
                        If DDLTemplType.SelectedValue.Trim.Equals("TAO") = True Then
                            Dim intTemplateID As Integer = SQL.Search("Call_Detail", "SaveCall-1015", "Select TL_NU9_ID_PK from T050011 where upper(TL_VC30_Template)='" & TxtTmplName.Text.Trim.ToUpper & "' and (TL_NU9_CustID_FK=" & DDLCustomer.SelectedValue.Trim & ") or  TL_NU9_CustID_FK=0 or TL_NU9_CustID_FK is null")
                            mstGetFunctionValue = WSSSave.SaveTemplate(intTemplateID, DDLTemplType.SelectedValue.Trim, ViewState("CompanyID"), ViewState("CallNo"))

                            If mstGetFunctionValue.ErrorCode = 0 Then
                                DisplayMessage("Call Data Saved Successfully...")
                                ViewState("SAddressNumber") = intTemplateID
                                ViewState("gshPageStatus") = 1
                                cpnlTaskAction.Enabled = True
                            Else
                                Call DisplayError("Server is busy please try later...")
                            End If

                        End If
                    End If

                    Call UpdateTaskPriority()

                    mstGetFunctionValue = WSSUpdate.UpdateTemplateCallStatus(ViewState("CallNo"), ViewState("CompanyID"))
                    garFileID.Clear()
                    ' ChecK for attachment from templates
                    If DDLTemplType.SelectedValue.Trim.Equals("TAO") = False And (mflgTemplateValueFilled = False Or Request.Form("txtChangeTemplate") = "1") Then
                        ' WSSSave.SaveTemplateAttachmentCall(ViewState("CallNo"), Viewstate("SAddressNumber"))
                    End If
                    txtChangeTemplate.Value = 0
                    If shReturn = 1 Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Call data Saved...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        ViewState("gshPageStatus") = 1
                        mshFlag = 0
                        cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                        cpnlCallTask.Enabled = True
                        cpnlCallTask.TitleCSS = "test"
                        mintID = 0
                        Return True
                    ElseIf shReturn = 2 Then
                        lstError.Items.Clear()
                        ViewState("gshPageStatus") = 1
                        lstError.Items.Add("Call data Saved...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                        SQL.Delete("Call_Detail", "SaveCall-1065", "delete from T040041 where AT_NU9_Call_Number=" & ViewState("CallNo") & " and AT_VC255_File_Name='" & mstrFileName & "' and AT_VC1_Status='T' and AT_NU9_CompId_Fk=" & ViewState("CompanyID"), SQL.Transaction.Serializable)
                        dtEstCloseDate.Text = SetDateFormat(Now, mdlMain.IsTime.DateOnly)
                        dtEstStartDate.Text = SetDateFormat(Now, mdlMain.IsTime.DateOnly)
                        mshFlag = 0
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Call data Saved...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        ViewState("gshPageStatus") = 1
                        cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                        cpnlCallTask.Enabled = True
                        cpnlCallTask.TitleCSS = "test"
                        mshFlag = 0
                        Return True
                    End If
                End If

            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("Call_Detail", "Save Call-1017", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Return False
            Finally

                'Dim strHeaderText As String = "Call # " & ViewState("CallNo")
                'Dim intId As Integer
                ''     If Not IsPostBack Then 'This is a fake block for executing security because visibility of controls is changing in programming 
                'Dim strPath As String
                'strPath = Session("PropRootDir")
                'If Request.QueryString("ID") = 0 Then
                '    intId = 939
                'Else
                '    intId = 3
                'End If
                'ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:javascript:GoToCall(" & ViewState("CallNo") & ");</script>", False)
                'ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:tabClose();</script>", False)
                'enableTab('" & dvScreen.Item(j).Item("AName") & "','" & strURL & "','" & dvScreen.Item(j).Item("ObjectID") & "',-1)"">" & "<font Size=""1"" face=""verdana"" color=""Blue"" font-underline=""false"">" & dvScreen.Item(j).Item("AName") & "</font>" & "</a>"''\""'""'"''
                'ScriptManager.RegisterStartupScript(Page, Type.GetType("System.String"), "", "<script>javascript:window.parent.OpenTabOnDBClick('" & strHeaderText & "' ,""SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" & ViewState("CallNo") & "&CompId='" & ViewState("CompanyID") & "','" & strHeaderText & "','" & intId & "');</script>", False)
                arColumnName.Clear()
                arRowData.Clear()
            End Try
        End If
    End Function

    Private Function UpdateCall() As Boolean
        Dim intTaskNo As Integer
        Dim dsOpenTasks As DataSet
        Dim intCount As Integer
        Dim sqlQuery As String
        Dim intAction As String
        If TxtTmplName.Text.Trim.Equals("") = False Then
            If DDLTemplType.SelectedValue.Trim.Equals("TAO") = False And (mflgTemplateValueFilled = False Or txtChangeTemplate.Value = "1") Then
                ' -- Fill Call From Template
                Call FillCallFromTemplate()
            End If
        End If

        If CDDLStatus.CDDLGetValue.Trim.Equals("CLOSED") Then
            intTaskNo = SQL.Search("Call_Detail", "UpdateCall-1111", "Select TM_Nu9_Task_No_Pk from T040021 where TM_Nu9_Call_No_Fk=" & ViewState("CallNo") & " and TM_VC50_Deve_Status<>'Closed' and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"))
        End If

        If intTaskNo > 0 Then
            dsOpenTasks = SQL.FetchTables("Call_Detail", "UpdateCall-1111", "Select TM_Nu9_Task_No_Pk from T040021 where TM_Nu9_Call_No_Fk=" & ViewState("CallNo") & " and TM_VC50_Deve_Status<>'Closed' and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID"))
            If dsOpenTasks.Tables(0).Rows.Count > 0 Then
                For intCount = 0 To dsOpenTasks.Tables(0).Rows.Count - 1
                    intTaskNo = dsOpenTasks.Tables(0).Rows(intCount)(0)
                    intAction = SQL.Search("Action_Detail", "UpdateCall-1111", "Select AM_NU9_Action_Number from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo") & "  and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and AM_NU9_Task_Number =" & intTaskNo)
                    If intAction > 0 Then
                        sqlQuery = "update T040021 set TM_VC50_Deve_status = 'Closed' where TM_Nu9_Task_No_Pk = " & intTaskNo & " and TM_Nu9_Call_No_Fk=" & ViewState("CallNo") & "  and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID")
                    Else
                        sqlQuery = "update T040021 set TM_VC50_Deve_status = 'Closed',TM_CH1_Mandatory='O' where TM_Nu9_Task_No_Pk = " & intTaskNo & " and TM_Nu9_Call_No_Fk=" & ViewState("CallNo") & "  and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID")
                    End If
                    SQL.Update("calldetail", "updatecall-1423", sqlQuery, SQL.Transaction.Serializable)
                Next
            End If
            'lstError.Items.Clear()
            'lstError.Items.Add("All the Tasks for the call are not closed...")
            'ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            'Return False
            'Exit Function
        End If

        Dim strEstDate As String
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        lstError.Items.Clear()

        If ValidateRecords() = False Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Function
        End If

        If Not dtEstFinishDate.Text.Trim.Equals("") Then
            If IsDate(dtEstFinishDate.Text) = False Then
                lstError.Items.Add("Check date format of Estimated Close date...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            Else
                Dim dtCloseDate As Date = CDate(dtEstFinishDate.Text.Trim)
                If CDate(Format(CDate(dtCloseDate), "MM/dd/yyyy")) < CDate(Format(CDate(txtCallDate.Text.Trim), "MM/dd/yyyy")) Then
                    lstError.Items.Add("Estimated Close date cannot be less than Call open date...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            End If
            strEstDate = CStr(dtEstFinishDate.Text.Trim)
        Else
            strEstDate = ""
        End If

        Try
            arColumnName.Add("CM_NU9_CustID_FK")
            arColumnName.Add("CM_VC8_Call_Type")
            arColumnName.Add("CM_DT8_Request_Date")
            arColumnName.Add("CM_NU9_Call_Owner")
            arColumnName.Add("CM_VC100_By_Whom")
            arColumnName.Add("CM_VC200_Work_Priority")
            arColumnName.Add("CM_VC50_Reference_Id")
            arColumnName.Add("CM_DT8_Close_Date")
            arColumnName.Add("CM_VC100_Subject")
            arColumnName.Add("CM_VC2000_Call_Desc")
            arColumnName.Add("CM_NU9_Project_ID")
            arColumnName.Add("CN_VC20_Call_Status")
            arColumnName.Add("CM_VC8_Template")
            If ViewState("TemplateName") <> TxtTmplName.Text Then
                arColumnName.Add("CM_VC8_Tmpl_Type")
            End If
            arColumnName.Add("CM_DT8_Call_Close_Date")
            arColumnName.Add("CM_NU9_Agreement")

            arColumnName.Add("CM_NU9_Coordinator")
            arColumnName.Add("CM_VC8_Category")
            arColumnName.Add("CM_VC8_Call_Category")
            arColumnName.Add("CM_VC8_Cause_Code")
            arColumnName.Add("CM_NU9_Related_Call")

            arColumnName.Add("CM_NU9_Category_Code_1")
            arColumnName.Add("CM_NU9_Category_Code_2")

            arColumnName.Add("CM_DT8_Call_Start_Date") 'Call start date new field



            arRowData.Add(Val(ViewState("CustID")))
            'arRowData.Add(CDDLCallType.CDDLGetValue.Trim.ToUpper)
            arRowData.Add(CDDLCallType.Text.Trim.ToUpper)
            arRowData.Add(txtCallDate.Text)
            'arRowData.Add(CDDLCallOwner.CDDLGetValue.Trim.ToUpper)
            arRowData.Add(CDDLCallOwner.SelectedValue)
            arRowData.Add(txtCallBy.Value.Trim)
            arRowData.Add(CDDLPriority.CDDLGetValue.Trim.ToUpper)

            arRowData.Add(txtReference.Text.Trim)
            If dtEstFinishDate.Text.Trim.Equals("") Then
                arRowData.Add(DBNull.Value)
            Else
                arRowData.Add(dtEstFinishDate.Text.Trim)
            End If
            arRowData.Add(txtSubject.Text.Trim)
            arRowData.Add(txtDescription.Text.Trim)
            arRowData.Add(Val(DDLProject.SelectedValue))
            arRowData.Add(CDDLStatus.CDDLGetValue.Trim.ToUpper)
            arRowData.Add(TxtTmplName.Text)
            If ViewState("TemplateName") <> TxtTmplName.Text Then
                arRowData.Add(IIf(DDLTemplType.SelectedValue = "All Types", "", DDLTemplType.SelectedValue))
            End If
            If CDDLStatus.CDDLGetValue.Trim.ToUpper.Equals("CLOSED") Then
                arRowData.Add(Now)
            Else
                arRowData.Add(DBNull.Value)
            End If
            arRowData.Add(Val(DDLAgreement.SelectedValue))

            If DDLCoordinator.SelectedValue = "" Then
                arRowData.Add(DBNull.Value)
            Else
                arRowData.Add(DDLCoordinator.SelectedValue)
            End If

            arRowData.Add(CDDLCategory.CDDLGetValue)
            arRowData.Add(CDDLCallCategory.CDDLGetValue)
            arRowData.Add(CDDLCauseCode.CDDLGetValue)
            arRowData.Add(IIf(Val(txtCallRef.Text.Trim) > 0, Val(txtCallRef.Text.Trim), System.DBNull.Value))
            arRowData.Add(IIf(txtCateCode1.Text.Trim.Equals("") = True, System.DBNull.Value, txtCateCode1.Text.Trim))
            arRowData.Add(IIf(txtCateCode2.Text.Trim.Equals("") = True, System.DBNull.Value, txtCateCode2.Text.Trim))
            'Add new call start date
            If IsNothing(DTCallStartDate.SelectedDate) = False Then
                If DTCallStartDate.SelectedDate.Equals("") = False Then
                    arRowData.Add(DTCallStartDate.SelectedDate)
                Else
                    arRowData.Add(System.DBNull.Value)
                End If
            Else
                arRowData.Add(System.DBNull.Value)
            End If


            If mstrPrvStatus <> "" And mstrPrvStatus <> CDDLStatus.CDDLGetValue Then
                'If status is changed manually then pass true to Update Call function
                mstGetFunctionValue = WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arColumnName, arRowData, True, CDDLStatus.CDDLGetValue.Trim.ToUpper)
            Else
                mstGetFunctionValue = WSSUpdate.UpdateCall(ViewState("CallNo"), ViewState("CompanyID"), arColumnName, arRowData, False, CDDLStatus.CDDLGetValue.Trim.ToUpper)
            End If
            'Update Status after saving in database and logging
            mstrPrvStatus = CDDLStatus.CDDLGetValue
            Dim shReturn As Short

            'Check the call status for adding comments to the call
            If ViewState("CallStatus") <> "" And ViewState("CallStatus") <> CDDLStatus.CDDLGetValue Then
                If Not (CDDLStatus.CDDLGetValue = "OPEN" Or CDDLStatus.CDDLGetValue = "ASSIGNED" Or CDDLStatus.CDDLGetValue = "PROGRESS") Then
                    Call SaveCallStatusComments(ViewState("CallStatus"), CDDLStatus.CDDLGetValue)
                End If
            End If
            ViewState("CallStatus") = CDDLStatus.CDDLGetValue
            If TxtTmplName.Text.Trim.Equals("") = False And (txtChangeTemplate.Value = "2" Or txtChangeTemplate.Value = "1") Then
                Dim intTemplateID As Integer = SQL.Search("Call_Detail", "UpdateCall-1246", "Select TL_NU9_ID_PK from T050011 where TL_VC8_Tmpl_Type IN('TAO','CNT')  and upper(TL_VC30_Template)='" & TxtTmplName.Text.Trim.ToUpper & "' and (TL_NU9_CustID_FK=" & DDLCustomer.SelectedValue.Trim & ") or  TL_NU9_CustID_FK=0 or TL_NU9_CustID_FK is null")
                mstGetFunctionValue = WSSSave.SaveTemplate(intTemplateID, IIf(DDLTemplType.SelectedValue.Trim = "All Types", "", DDLTemplType.SelectedValue.Trim), ViewState("CompanyID"), ViewState("CallNo"))

                ' Updating the call status
                If CDDLStatus.CDDLGetValue.Trim.ToUpper.Equals("CLOSED") = False Then
                    mstGetFunctionValue = WSSUpdate.UpdateTemplateCallStatus(ViewState("CallNo"), ViewState("CompanyID"))
                End If

                If mstGetFunctionValue.ErrorCode = 0 Then
                    lstError.Items.Add("Call Data Saved Successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    ViewState("SAddressNumber") = intTemplateID
                    txtChangeTemplate.Value = 0
                    ViewState("gshPageStatus") = 1
                    cpnlTaskAction.Enabled = True

                    Call UpdateTaskPriority()
                    '--Dependency
                    FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
            If mstGetFunctionValue.ErrorCode = 0 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                If GetFiles(mdlMain.AttachLevel.CallLevel) = True Then
                    shReturn = 1
                Else
                    shReturn = 2
                End If
                If CDDLStatus.CDDLGetValue.Trim.ToUpper.Equals("CLOSED") = False Then
                End If
                garFileID.Clear()
                Call UpdateTaskPriority()
                '--Dependency
                'FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                If shReturn = 1 Then
                    lstError.Items.Add("Call data Updated...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    mshFlag = 0
                    Return True
                ElseIf shReturn = 2 Then
                    lstError.Items.Add("Call data Updated...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    mshFlag = 0
                    Return True
                Else
                    mshFlag = 0
                    lstError.Items.Add("Call data Updated...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                End If
            End If

        Catch ex As Exception
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("Call_Detail", "UC-1240", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Function

    Private Function UpdateTaskPriority() As Boolean
        Try
            Dim sqlQuery As String
            sqlQuery = "update T040021 set TM_VC8_Priority='" & CDDLPriority.CDDLGetValue & "' where TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and TM_NU9_Call_No_FK = " & ViewState("CallNo") & " and TM_VC50_Deve_status<>'CLOSED' "
            SQL.Update("calldetail", "updatecall-1423", sqlQuery, SQL.Transaction.Serializable)
        Catch ex As Exception
            CreateLog("Call_Detail", "UpdateTaskPriority-1434", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

    Private Function ValidateRecords() As Boolean
        Try
            lstError.Items.Clear()
            If DDLCustomer.SelectedValue.Trim.Equals("") = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Customer data cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
            End If
            If CDDLStatus.CDDLGetValue.Trim.Equals("") = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Call status cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
            End If
            

            If CDDLCallType.Text.Trim.Equals("") Then
                lstError.Items.Add("Call type cannot be blank...")
                mshFlag = 1
            End If

            If dtEstFinishDate.Text.Trim.Equals("") Then
                lstError.Items.Add("Est Close Date cannot be blank...")
                mshFlag = 1
            End If

            If txtSubject.Text.Trim.Equals("") Then
                lstError.Items.Add("Subject cannot be blank...")
                mshFlag = 1
            End If

            If txtDescription.Text.Trim.Equals("") Then
                lstError.Items.Add("Description cannot be blank...")
                mshFlag = 1
            End If

            If txtCallBy.Value.Trim.Equals("") Then
                lstError.Items.Add("Entered by cannot be blank...")
                mshFlag = 1
            End If

            If txtCallRef.Text.Trim.Equals("0") Then
                lstError.Items.Add("Invalid Related Call Number...")
                mshFlag = 1
            End If

            If Val(txtCallRef.Text.Trim) = Val(ViewState("CallNo")) And txtCallRef.Text.Trim.Equals("") = False Then
                lstError.Items.Add("Related Call Number cannot be same as Current Call Number...")
                mshFlag = 1
            End If

            If CDDLCallOwner.Text.Trim.Equals("") Then
                lstError.Items.Add("Requested By cannot be blank...")
                mshFlag = 1
            End If

            If CDDLPriority.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Priority cannot be blank...")
                mshFlag = 1
            End If

            If Val(DDLProject.SelectedValue) <= 0 Then
                lstError.Items.Add("SubCategory cannot be blank...")
                mshFlag = 1
            End If

            If DDLTemplType.SelectedValue.Trim <> "All Types" And TxtTmplName.Text.Trim.Equals("") Then
                lstError.Items.Add("Template selection is mandatory if template type is chosen...")
                mshFlag = 1
            End If

            '''''''''''''''This function will Check the Items Filled in RAD COMBO'''''''
            If CheckRealValuesForDDLS() = False Then
                mshFlag = 1
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If mshFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If
            'Check Related Call 
            If txtCallRef.Text.Trim.Equals("") = False Then
                Dim intRows As Integer
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                If SQL.Search("", "", "select * from T040011 where CM_NU9_Comp_Id_FK=" & Val(ViewState("CompanyID")) & " and CM_NU9_Call_No_PK=" & Val(txtCallRef.Text.Trim), intRows) = False Then
                    lstError.Items.Add("Related Call Number does not exist in unser selected Customer")
                    mshFlag = 1
                End If
            End If
            'Check Requested By Validity
            mstGetFunctionValue = CheckUserValiditity(CDDLCallOwner.SelectedValue)
            If mstGetFunctionValue.FunctionExecuted = False Then
                lstError.Items.Add("Requested By: " & mstGetFunctionValue.ErrorMessage)
                mshFlag = 1
            End If
            'Check project validity
            mstGetFunctionValue = CheckProjectValidity(ViewState("CompanyID"), DDLProject.SelectedValue)
            If mstGetFunctionValue.FunctionExecuted = False Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                mshFlag = 1
            End If
            If IsNothing(DTCallStartDate.SelectedDate) = False And IsNothing(dtEstFinishDate.Text.Trim) = False Then
                If dtEstFinishDate.Text.Trim.Equals("") = False And dtEstFinishDate.Text.Trim.Equals("") = False Then
                    Dim dtCloseDate As Date = CDate(dtEstFinishDate.Text.Trim)
                    If CDate(Format(CDate(dtCloseDate), "MM/dd/yyyy")) < CDate(Format(CDate(DTCallStartDate.SelectedDate), "MM/dd/yyyy")) Then
                        lstError.Items.Add("Received Date  cannot be greater than Call Close date...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                        Exit Function
                    End If
                End If

            End If

            If mshFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("Call-Detail", "ValidateRecords-1695", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Function

    Private Function CheckRealValuesForDDLS() As Boolean
        Try
            Dim strRequestedBy As String = String.Empty
            Dim strCallType As String = String.Empty
            strRequestedBy = SQL.Search("", "", "select ct_VC8_calltype_fk from T040103 where ct_VC8_calltype_fk='" & CDDLCallType.Text & "'", "")
            If String.IsNullOrEmpty(strRequestedBy) = True Then
                lstError.Items.Add("The call-type you have entered is not Valid.. Please select it from Dropdown")
                Return False
            End If

            strCallType = SQL.Search("", "", "SELECT um_in4_address_no_fk as ID,(rtrim(ltrim(UName.ci_vc36_name)) + '[' + um_vc50_userid + ']') as Name,t010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where UM_VC4_Status_Code_FK='ENB' and t010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and (um_in4_company_ab_id=" & ViewState("CompanyID") & ") and UM_IN4_Company_AB_ID=" & ViewState("CompanyID") & " and (rtrim(ltrim(UName.ci_vc36_name)) + '[' + um_vc50_userid + ']')='" & CDDLCallOwner.Text & "'", "")
            If String.IsNullOrEmpty(strCallType) = True Then
                lstError.Items.Add("The user you have selected in Requested by is not Valid.. Please select a valid user from Dropdown")
                Return False
            End If
            Return True
        Catch ex As Exception
        End Try
    End Function

    Private Function GetFiles(ByVal Level As AttachLevel) As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim blnRead As Boolean

            Select Case Level
                ' For Calls
                Case AttachLevel.CallLevel

                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1464", "select * from T040041 Where AT_IN4_Level=1 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            If CreateFolder(ViewState("CallNo")) = True Then
                                shAttachments = 1
                                'Return True
                            Else
                                shAttachments = 2
                                'Return False
                            End If
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        'sql.Update("Call_Detail","GetFiles-1600","Update T040011 set CM_NU8_Attach_No="
                        Return True
                    Else
                        Return False
                    End If
                    ' For Tasks
                Case AttachLevel.TaskLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1491", "select * from T040041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("CallNo"), ViewState("TaskNo"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    ' For Actions
                Case AttachLevel.ActionLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1512", "select * from T040041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

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

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
            End Select
            sqrdTempFiles.Close()
        Catch ex As Exception
            CreateLog("Call-Detail", "GetFiles-1837", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1577", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-1596", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & ViewState("CompanyID"))

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

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1624", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
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
                            SQL.Update("Call_Detail", "CreateFolder-1637", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
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

            Return False
        End Try
    End Function

#Region "Create Task Grid"

    Private Sub CreateGridTask()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        Try
            dtgTask.ID = "dtgTask"
            dtgTask.DataKeyField = "TM_NU9_Task_no_PK"
            Call FormatGridTask()

            PlaceHolder1.Controls.Add(dtgTask)
        Catch ex As Exception
            CreateLog("Call-Detail", "CreateGridTask-1916", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub

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
#End Region

#Region "Create Template Column Task Grid"
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

            arrColumnsNameTask.Clear()
            arrColumnsNameTask.Add("C")
            arrColumnsNameTask.Add("A")
            arrColumnsNameTask.Add("F")
            arrColumnsNameTask.Add("TO")
            arrColumnsNameTask.Add("ID")
            arrColumnsNameTask.Add("Stat")
            arrColumnsNameTask.Add("Subject")
            arrColumnsNameTask.Add("TType")
            arrColumnsNameTask.Add("Ownr")
            arrColumnsNameTask.Add("Dep")
            ''''''''''''''Modified by tarun on Jan 30 2010'''''''''''
            arrColumnsNameTask.Add("TaskStartDate")
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            arrColumnsNameTask.Add("EstClDate")
            arrColumnsNameTask.Add("EHr")
            arrColumnsNameTask.Add("AcM")

            arrColumnsNameTask.Add("Prio")
            ' arrColumnsNameTask.Add("Proj")
            'arrColumnsNameTask.Add("Agmt")

            arrWidthTask.Clear()
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(70)
            arrWidthTask.Add(220)
            arrWidthTask.Add(65)
            arrWidthTask.Add(70)
            arrWidthTask.Add(35)
            '''''''''''''modified by tarun on Jan 30, 2010'''''
            arrWidthTask.Add(88)
            '''''''''''''''''''''''''''''''''''''''''''''''''''
            arrWidthTask.Add(85)
            arrWidthTask.Add(35)
            arrWidthTask.Add(25)
            arrWidthTask.Add(40)
            'arrWidthTask.Add(40)
            'arrWidthTask.Add(49)


            dtgTask.Width = Unit.Pixel(700)

            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(0)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(1)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(2)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(3)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(4)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(5)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(6)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(7)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(8)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(9)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(10)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(11)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(12)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(13)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(14)))


            tclTask(0) = New TemplateColumn
            tclTask(0).Visible = True
            tclTask(0).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(0).ToString, "", dtvTask.Table.Columns(0).ToString + "_H", False, arrColumnsNameTask(0), False)
            tclTask(0).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(0).ToString, arrImageUrlDisabled(0))
            tclTask(0).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclTask(0).ItemStyle.Width = arrColumnsWidthTask(0)
            dtgTask.Columns.Add(tclTask(0))

            tclTask(1) = New TemplateColumn
            tclTask(1).Visible = True
            tclTask(1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(1).ToString, "", dtvTask.Table.Columns(1).ToString + "_H", False, arrColumnsNameTask(1), False)
            tclTask(1).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(1).ToString, arrImageUrlDisabled(1))
            tclTask(1).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclTask(1).ItemStyle.Width = arrColumnsWidthTask(1)
            dtgTask.Columns.Add(tclTask(1))

            tclTask(2) = New TemplateColumn
            tclTask(2).Visible = True
            tclTask(2).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(2).ToString, "", dtvTask.Table.Columns(2).ToString + "_H", False, arrColumnsNameTask(2), False)
            tclTask(2).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(2).ToString, arrImageUrlDisabled(1))
            tclTask(2).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclTask(2).ItemStyle.Width = arrColumnsWidthTask(2)
            dtgTask.Columns.Add(tclTask(2))

            Dim maxchar() As Int16 = {-1, -1, -1, 3, 3, 7, 20, 7, 8, 3, 12, 4, 1, 5, 2, 5, 5} 'Variable to store MaxLength of TextBoxes

            For intCount = 3 To dtvTask.Table.Columns.Count - 1

                tclTask(intCount + 1) = New TemplateColumn
                tclTask(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvTask.Table.Columns(intCount).ToString, dtvTask.Table.Columns(intCount).ToString)
                Dim AddEventOnGrigHeader As New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(intCount).ToString, "", dtvTask.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameTask(intCount).ToString, True, maxchar(intCount))
                AddHandler AddEventOnGrigHeader.OnSort, AddressOf SortGridTask
                tclTask(intCount + 1).HeaderTemplate = AddEventOnGrigHeader

                tclTask(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvTask.Table.Columns(intCount).ToString + "_F", False)
                tclTask(intCount + 1).ItemStyle.Width = arrColumnsWidthTask(intCount)    'System.Web.UI.WebControls.Unit..Pixel(arrColumnsWidthTask(intCount))
                dtgTask.Columns.Add(tclTask(intCount + 1))
            Next
        Catch ex As Exception
            lstError.Items.Add("Unexpected Error..")
            CreateLog("Call-Detail", "CreateTemplateColumnsTask-1998", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub

#End Region

#Region "Create Task Query"
    Private Sub CreateDataTableTask(ByVal strWhereClause As String, Optional ByVal FinalWhere As Boolean = False)
        '-- FinalWhere will tell whether whereclause is to be checked or not
        Dim dsTask As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32
        If IsNothing(strWhereClause) Then
            strWhereClause = ""
        End If
        Try

            strSql = " select TM_CH1_Comment as Blank1, TM_CH1_Attachment as Blank2,TM_CH1_Forms as Blank3,TM_NU9_Task_Order, TM_NU9_Task_no_PK,  TM_VC50_Deve_Status,TM_VC1000_Subtsk_Desc,  TM_VC8_task_type,b.UM_VC50_UserID+'~'+convert(varchar(8),a.TM_VC8_Supp_Owner) as UM_VC50_UserID, TM_NU9_Dependency,convert(varchar,TM_DT8_Task_Date) TM_DT8_Task_Date,convert(varchar,TM_DT8_Est_close_date) TM_DT8_Est_close_date, TM_FL8_Est_Hr,TM_CH1_Mandatory,  TM_VC8_Priority  From T040021 a, T060011 b Where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK= a.TM_VC8_Supp_Owner "

            strSql = strSql & " Order By TM_NU9_Task_Order asc"

            Call SQL.Search("T040021", "Call_Detail", "CreateDataTableTask-1803", strSql, dsTask, "sachin", "Prashar")

            dtvTask = New DataView
            dtvTask.Table = dsTask.Tables(0)

            Dim htDateCols As New Hashtable
            htDateCols.Add("TM_DT8_Est_close_date", 2)
            SetDataTableDateFormat(dtvTask.Table, htDateCols)
            If Not strWhereClause.Trim.Equals("") Then
                GetFilteredDataView(dtvTask, strWhereClause)
            End If

            'Clear Session when grid is blank
            If dsTask.Tables(0).Rows.Count = 0 Then
                ViewState("TaskNo") = 0
                ViewState("ActionNo") = 0
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

            If Val(ViewState("CallNo")) = 0 Then
                cpnlCallTask.Enabled = False
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallTask.CssClass = "test2"
            Else
                cpnlCallTask.Enabled = True
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                cpnlCallTask.CssClass = "test"
            End If

            If FinalWhere = True And strWhereClause.Trim <> "" Then
                If isTaskInGrid(Session("propTaskNumber")) = False Then
                    ViewState("TaskNo") = 0
                    introwvalues = 0 ' -- remove the selection of task
                    'dtgTask.Items.Item(1).BackColor = Color.FromArgb(212, 212, 212)
                End If
                'mTaskPrvSearchString = strWhereClause
            End If

            If dsTask.Tables(0).Rows.Count = 0 Then
            Else
                If imgAddAttachment.Src.Equals("../../Images/Attach_Yellow.gif") = False Then
                    For intI As Integer = 0 To dsTask.Tables(0).Rows.Count - 1
                        If dsTask.Tables(0).Rows(intI).Item("Blank2").Equals("1") = True Then
                            imgAddAttachment.Src = "../../Images/Attach_Yellow.gif"
                            Exit For
                        Else
                            imgAddAttachment.Src = "../../Images/Attach15_9.gif"
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("Call-Detail", "CreateDataTableTask-1750", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
        '===============================
    End Sub
#End Region

#Region "Fill Task Header Array"
    Private Sub FillHeaderArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        Try
            arrHeadersTask.Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvTask.Table.Columns.Count - 1
                    arrHeadersTask.Add(Request.Form("cpnlCallTask$dtgTask$ctl01$" + dtvTask.Table.Columns(intCount).ColumnName + "_H"))
                Next
            End If
        Catch ex As Exception
            CreateLog("Call-Detail", "FillHeaderArrayTask-2144", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
#End Region

#Region "Fill Task Footer Array"
    Private Sub FillFooterArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        Try
            arrFooterTask.Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvTask.Table.Columns.Count - 1
                    intFooterIndex = dtvTask.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                    arrFooterTask.Add(Request.Form("cpnlCallTask$dtgTask$ctl01" & intFooterIndex.ToString.Trim & ":" + dtvTask.Table.Columns(intCount).ColumnName + "_F"))
                Next
            End If
        Catch ex As Exception
            CreateLog("Call_Detail", "FillFooterArrayTask-2155", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
#End Region

#Region "Bind Task Grid"
    Private Sub BindGridTask()
        Try
            Dim htGrdColumns As New Hashtable
            htGrdColumns.Add("TM_VC1000_Subtsk_Desc", 28)

            HTMLEncodeDecode(mdlMain.Action.Encode, dtvTask, htGrdColumns)
            SetCommentFlag(dtvTask, mdlMain.CommentLevel.TaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            mTaskRowValue = 0
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
        Catch Ex As Exception
            CreateLog("Call_Detail", "BindGridTask-2122", LogType.Application, LogSubType.Exception, Ex.TargetSite.Attributes, Ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
#End Region

    Private Sub dtgTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTask.ItemDataBound
        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvTask
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim TaskNo As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvTask.ToTable()
        Dim strSelected As String
        Dim structTempTaskOwner As Owners '-- will keep taskowners ID and Name
        dg.PageSize = 1
        Dim TaskStatus As String = String.Empty
        Dim CallStatus As String = String.Empty
        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dtgTask.DataKeys(0) <> 0 Then
                    For intCount = 0 To 2       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        TaskNo = dtgTask.DataKeys(e.Item.ItemIndex)
                        TaskStatus = dtvTask.Table.Rows(cnt).Item(5).ToString
                        CallStatus = CDDLStatus.CDDLGetValue

                        If strSelected = "1" Then      'If comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "'," & ViewState("CallNo") & "," & TaskNo & "," & ViewState("CompanyID") & ")")
                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "','" & mTaskRowValue + 1 & "', 'T','" & intCount & "'," & ViewState("CallNo") & "," & TaskNo & "," & ViewState("CompanyID") & ")")
                        Else       ' If no comment/attachment is attached
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "','" & mTaskRowValue + 1 & "', 'T','" & intCount & "'," & ViewState("CallNo") & "," & TaskNo & "," & ViewState("CompanyID") & ")")
                        End If
                    Next

                    For intCount = 3 To dtvTask.Table.Columns.Count - 1       'for Others
                        If dtvTask.Table.Columns(intCount).DataType.FullName.Equals("System.DateTime") Then
                            If dtBound.Rows(cnt)(intCount).ToString Is Null Or dtBound.Rows(cnt)(intCount).ToString = "" Then
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = " "
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = CType(dtBound.Rows(cnt)(intCount).ToString, DateTime).ToShortDateString
                            End If
                        Else
                            Dim maxchar As Byte
                            Select Case intCount 'Truncate characters
                                Case 4
                                    maxchar = 8
                                Case 5
                                    maxchar = 0
                                Case 6
                                    maxchar = 7
                                Case 7
                                    maxchar = 10
                                Case 8
                                    maxchar = 2
                                Case 9
                                    maxchar = 12
                                Case 10
                                    maxchar = 4
                                Case 12, 13, 14
                                    maxchar = 5
                                Case Else
                                    maxchar = 0
                            End Select
                            If intCount = 8 Then ' for task owner
                                structTempTaskOwner.Id = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(1)
                                structTempTaskOwner.Name = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = structTempTaskOwner.Name
                                'Tootip should have full value
                                e.Item.Cells(intCount).ToolTip = HTMLEncodeDecode(mdlMain.Action.Decode, structTempTaskOwner.Name)
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString)
                                e.Item.Cells(intCount).ToolTip = HTMLEncodeDecode(mdlMain.Action.Decode, IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString))
                            End If
                        End If

                        TaskNo = dtgTask.DataKeys(e.Item.ItemIndex)
                        Dim CompID As Integer = ViewState("CompanyID")
                        Dim CallNo As Integer = ViewState("CallNo")

                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheckTask('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'cpnlCallTask_dtgTask','" & TaskStatus & "','" & CompID & "','" & CallNo & "','" & CallStatus & "')")
                        If intCount = 8 Then 'for task owner
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & structTempTaskOwner.Id & "')")
                        Else ' for cells other task owner
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheckTaskEdit('" & TaskNo & "', '" & mTaskRowValue + 1 & "','cpnlCallTask_dtgTask','" & CompID & "','" & CallNo & "','0')")
                        End If
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Width = arrColumnsWidthTask(intCount)
                    Next
                    mTaskRowValue += 1
                Else
                    For intCount = 0 To 1       'For Image Fields
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(1)
                    Next
                    For intCount = 3 To dtvTask.Table.Columns.Count - 1
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If
            If e.Item.ItemType = ListItemType.Header Then
                For intCol As Integer = 0 To dtvTask.Table.Columns.Count - 1
                    If intCol >= 3 Then
                        CType(e.Item.Cells(intCol).FindControl(dtvTask.Table.Columns(intCol).ToString & "_H"), TextBox).Width = arrColumnsWidthTask(intCol)
                    End If
                    CType(e.Item.Cells(intCol).FindControl("lbl" & dtvTask.Table.Columns(intCol).ToString & "_H"), LinkButton).Width = arrColumnsWidthTask(intCol)
                    CType(e.Item.Cells(intCol).FindControl("lbl" & dtvTask.Table.Columns(intCol).ToString & "_H"), LinkButton).Text = "" & CType(e.Item.Cells(intCol).FindControl("lbl" & dtvTask.Table.Columns(intCol).ToString & "_H"), LinkButton).Text
                Next
            End If
        Catch ex As Exception
            CreateLog("Call-Detail", "ItmDataBound-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub

#Region "Save Task Fast Entry"
    Private Function SaveTask() As Boolean

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intCallNo As Integer
        Dim ctrlTextBox As Control
        Dim blnCheckValidation As Boolean



        SaveTask = False
        For Each ctrlTextBox In cpnlCallTask.Controls
            If TypeOf ctrlTextBox Is TextBox Then
                If CType(ctrlTextBox, TextBox).Text.Trim.Equals("") Or CType(ctrlTextBox, TextBox).ID = "TxtProject_F" Or CType(ctrlTextBox, TextBox).ID = "TxtPriority_F" Or CType(ctrlTextBox, HtmlInputHidden).Value = "TxtPriority_FName" Or CType(ctrlTextBox, TextBox).ID = "TxtStatus_F" Then
                    blnCheckValidation = False
                Else
                    If Not CType(ctrlTextBox, TextBox).Text.Trim.Equals("ASSIGNED") Then
                        blnCheckValidation = True
                        Exit For
                    End If
                End If
            End If
        Next

        If TxtSubject_F.Text.Trim.Equals("") Then    'Exit If all textbox are blank
            SaveTask = False
            Exit Function
        End If
        '-----------------------------------------------
        'date 7/12/2006
        'to compare session values to stop f5 duplicate data while pressing f5 in data entry
        If ViewState("Update").ToString() = ViewState("update").ToString() Then '------------------------------
            Dim strStatus As String
            strStatus = GetStatus(ViewState("CompanyID"), ViewState("CallNo"), mdlMain.StatusType.CallStatus)

            If strStatus = "CLOSED" Then
                lstError.Items.Clear()

                lstError.Items.Add("You can't assign task to a call whose status is in " & CDDLStatus.CDDLGetValue & ".")
                lstError.Items.Add("  You need to change the call status to REOPEN and then add new task...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Return False
                Exit Function
            End If

            '**************************************************************************

            lstError.Items.Clear()
            Dim dtCallDate As Date = SQL.Search("CallView", "SaveTask-2126", "Select CM_DT8_Request_Date from T040011 where CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "")
            If CDDLTaskType_F.Text.Trim.Equals("") Then
                lstError.Items.Add("Task Type cannot be blank...")
                shFlag = 1

            ElseIf DDLTaskOwner.SelectedValue.Equals("") Then
                lstError.Items.Add("Task Owner cannot be blank...")
                shFlag = 1
            ElseIf Convert.ToDateTime(dtEstStartDate.Text) < dtCallDate.ToString("yyyy-MMM-dd") Then
                lstError.Items.Add("Task start date should be greater than Call Start Date")
                shFlag = 1
            ElseIf dtEstCloseDate.Text.Trim <> "" Then
                If IsDate(dtEstCloseDate.Text) = False Then
                    lstError.Items.Add("Check date format of estimated close date...")
                    shFlag = 1
                Else

                    If CDate(Format(CDate(dtEstCloseDate.Text.Trim), "MM/dd/yyyy")) < CDate(Format(CDate(txtCallDate.Text.Trim), "MM/dd/yyyy")) Or CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                        lstError.Items.Add("Estimated Close date cannot be less than Call date or current date")
                        shFlag = 1
                    End If
                End If
            ElseIf dtEstStartDate.Text.Trim <> "" Then
                If IsDate(dtEstStartDate.Text) = False Then
                    lstError.Items.Add("Check date format of Start date...")
                    shFlag = 1
                Else

                    If CDate(Format(CDate(dtEstStartDate.Text.Trim), "MM/dd/yyyy")) < CDate(Format(CDate(txtCallDate.Text.Trim), "MM/dd/yyyy")) Or CDate(dtEstStartDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                        lstError.Items.Add("Start date cannot be less than Call date or current date")
                        shFlag = 1
                    End If
                End If
            End If
            If dtEstStartDate.Text.Trim <> "" And dtEstCloseDate.Text.Trim <> "" Then
                If CDate(dtEstCloseDate.Text.Trim) < CDate(dtEstStartDate.Text.Trim) Then 'Or CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                    lstError.Items.Add("Estimated Close date cannot be less than Start date...")
                    shFlag = 1
                End If
            End If



            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                shFlag = 0
                Exit Function
            End If

            lstError.Items.Clear()

            strUDCType = "TKTY"
            strName = CDDLTaskType_F.Text.Trim.ToUpper
            strErrorMessage = "Task Type Mismatch..."

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            End If

            strUDCType = "PRIO"
            strName = CDDLPriority_F.Text.ToUpper
            strErrorMessage = "Task Priority Mismatch..."

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            End If
            If shFlag = 1 Then
                shFlag = 0
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            End If

            mstGetFunctionValue = CheckUserValiditity(DDLTaskOwner.SelectedValue)
            If mstGetFunctionValue.FunctionExecuted = False Then
                lstError.Items.Clear()
                lstError.Items.Add("Task Owner:" & mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            End If

            Try
                arrColumns.Add("TM_FL8_ETC")
                arrColumns.Add("TM_DT8_Task_Date")
                arrColumns.Add("TM_NU9_Call_No_FK")
                arrColumns.Add("TM_NU9_Comp_ID_FK")
                arrColumns.Add("TM_VC50_Deve_status")
                arrColumns.Add("TM_VC1000_Subtsk_Desc")
                arrColumns.Add("TM_VC8_task_type")
                arrColumns.Add("TM_NU9_Project_ID")
                arrColumns.Add("TM_VC8_Supp_Owner")
                arrColumns.Add("TM_NU9_Assign_by")
                arrColumns.Add("TM_VC8_Priority")
                arrColumns.Add("TM_CH1_Comment")
                arrColumns.Add("TM_CH1_Mandatory")
                arrColumns.Add("TM_NU9_Dependency")
                arrColumns.Add("TM_NU9_Agmt_No")
                arrColumns.Add("TM_FL8_Est_Hr")

                If Not dtEstCloseDate.Text.Trim.Equals("") Then
                    arrColumns.Add("TM_DT8_Est_close_date")
                End If
                arrColumns.Add("TM_CH1_Forms")
                arrColumns.Add("TM_NU9_Task_no_PK")

                'add new Field

                If TxtEstimatedHrs.Text.Trim.Equals("") Then
                    arrRows.Add(0)
                Else
                    arrRows.Add(TxtEstimatedHrs.Text.Trim)
                End If
                If Not dtEstStartDate.Text.Trim.Equals("") Then
                    arrRows.Add(dtEstStartDate.Text.Trim)
                End If
                arrRows.Add(ViewState("CallNo"))
                arrRows.Add(DDLCustomer.SelectedValue.Trim)
                arrRows.Add("ASSIGNED")
                arrRows.Add(TxtSubject_F.Text.Trim)
                arrRows.Add(CDDLTaskType_F.Text.Trim.ToUpper)
                arrRows.Add(Val(DDLProject.SelectedValue))
                arrRows.Add(DDLTaskOwner.SelectedValue)
                arrRows.Add(Session("PropUserID"))
                arrRows.Add(CDDLPriority_F.Text.Trim.ToUpper)
                arrRows.Add("0")
                If chkMandatory.Checked = True Then
                    arrRows.Add("M")
                Else
                    arrRows.Add("O")
                End If
                arrRows.Add(IIf(DDLDependency_F.SelectedValue = "", DBNull.Value, DDLDependency_F.SelectedValue))
                arrRows.Add(IIf(DDLAgreement.SelectedValue = "", DBNull.Value, DDLAgreement.SelectedValue))

                If TxtEstimatedHrs.Text.Trim.Equals("") Then
                    arrRows.Add(0)
                Else
                    arrRows.Add(TxtEstimatedHrs.Text.Trim)
                End If
                If Not dtEstCloseDate.Text.Trim.Equals("") Then
                    arrRows.Add(dtEstCloseDate.Text.Trim)
                End If
                'Get Form is Assigned then Put Value 2 Else 0
                If WSSSearch.GetNoOfAssignedForms(CDDLTaskType_F.Text, False, ViewState("CompanyID"), 0, ViewState("CallNo")) > 0 Then
                    arrRows.Add(2)
                Else
                    arrRows.Add(0)
                End If

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False

                intCallNo = SQL.Search("Call_Detail", "SaveTask-2136", "select isnull(max(TM_NU9_Task_no_PK),0) from T040021 where TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and TM_NU9_Call_No_FK=" & ViewState("CallNo").ToString)
                intCallNo += 1

                arrRows.Add(intCallNo.ToString)
                mstGetFunctionValue = WSSSave.SaveTask(arrColumns, arrRows, ViewState("CompanyID"), ViewState("CallNo"))
                If mstGetFunctionValue.ErrorCode = 0 Then
                    mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("CallNo"), True, ViewState("CompanyID"))
                    If mstGetFunctionValue.ErrorCode = 0 Then
                        If CDDLStatus.CDDLGetValueName = "OPEN" Or CDDLStatus.CDDLGetValueName = "DEFAULT" Then
                            CDDLStatus.CDDLSetSelectedItem("ASSIGNED")
                        End If
                        CDDLStatus.Enabled = True
                        lstError.Items.Clear()
                        ViewState("Update") = Server.UrlEncode(System.DateTime.Now.ToString())
                        DisplayMessage("Task Data Saved Successfully...")
                        ViewState("TaskNo") = intCallNo

                        '--Dependency
                        FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)

                        ClearAllTextBox(cpnlCallTask)
                        ViewState("gshPageStatus") = 1
                        If GetFiles(mdlMain.AttachLevel.TaskLevel) = True Then
                            'shReturn = 1
                        Else
                            'shReturn = 2
                        End If
                        'Next
                        garTFileID.Clear()
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        introwvalues = 1 ' Set Selection of grid to first row
                        'introwvalues = 0 ' Set Selection of grid to first row
                        If DDLTaskOwner.SelectedValue = Val(Session("PropUserID")) Then
                            PnlAction.Visible = True
                            dtActionDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
                        Else
                            PnlAction.Visible = False
                        End If
                        Return True
                    End If

                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If

            Catch ex As Exception
                CreateLog("SaveTask", "SaveTask-2453", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            End Try
        Else
            ClearAllTextBox(cpnlCallTask)
        End If
    End Function
#End Region

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                ' SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-2242", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & ViewState("CompanyID"))

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
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
            CreateLog("CallDetail", "CreateFolder-2747", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-2356", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & "  and VH_NU9_Action_Number=" & ActionNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & ViewState("CompanyID"))

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

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

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
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
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
            CreateLog("Call_Detail", "CreateFolder-1661", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        Try
            lstError.Items.Add(ErrMsg)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
        Catch ex As Exception
            CreateLog("Call-Detail", "DisplayError-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
    Private Sub DisplayWaring(ByVal ErrMsg As String)
        Try
            lstError.Items.Add(ErrMsg)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
        Catch ex As Exception
            CreateLog("Call-Detail", "DisplayError-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        Try
            lstError.Items.Add(Msg)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception
            CreateLog("Call-Detail", "DisplayMessage-2907", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
#End Region

#Region "Clear TextBoxes based on panels"
    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        TxtEstimatedHrs.Text = ""
        TxtSubject_F.Text = ""
        dtEstCloseDate.Text = ""
        TxtDescription_F.Text = ""
        TxtUsedHr_F.Text = ""
        dtEstStartDate.Text = ""
        CDDLActionOwner_F.CDDLSetBlank()

        DDLDependency_F.SelectedValue = ""
        CDDLTaskType_F.Text = ""
    End Sub
#End Region

#Region "Refresh Grid With no selection"
    Private Sub RefreshSelection()
        ViewState("CallNo") = 0    '//For refreshing seleted callnumber
        mstrCallNumber = 0
        ViewState("TaskNo") = 1
        Call CreateDataTableTask("", False)
        Call BindGridTask()
        Call CreateDataTableAction("")
        Call BindGridAction()
    End Sub
#End Region

#Region "Create Action Grid"

    Private Sub CreateGridAction()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        Try
            grdAction.ID = "grdAction"
            grdAction.DataKeyField = "AM_NU9_Action_Number"
            Call FormatGridAction()

            PlaceHolder2.Controls.Add(grdAction)
        Catch ex As Exception
            CreateLog("Call-Detail", "CreateGridAction-2827", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
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
        Try
            ReDim tclAction(dtvAction.Table.Columns.Count)

            arrImageUrlEnabled.Clear()
            arrImageUrlEnabled.Add("../../Images/comment2.gif")
            arrImageUrlEnabled.Add("../../Images/attach15_9.gif")
            arrImageUrlEnabled.Add("../../Images/form1.jpg")


            arrImageUrlDisabled.Clear()
            arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
            arrImageUrlDisabled.Add("../../Images/white.gif")
            arrImageUrlDisabled.Add("../../Images/white.gif")

            arrImageUrlNew.Clear()
            arrImageUrlNew.Add("../../Images/comment_Unread.gif")
            arrImageUrlNew.Add("../../Images/white.gif")
            arrImageUrlNew.Add("../../Images/Form2.gif")

            arrColumnsNameAction.Clear()
            arrColumnsNameAction.Add("Com")
            arrColumnsNameAction.Add("Att")
            arrColumnsNameAction.Add("Act#")
            'arrColumnsNameAction.Add("Action")
            arrColumnsNameAction.Add("Description")
            'arrColumnsNameAction.Add("Act.")
            arrColumnsNameAction.Add("Action_Date")
            arrColumnsNameAction.Add("HrM.")
            arrColumnsNameAction.Add("Hrs.")
            arrColumnsNameAction.Add("<u>A</u>ctionOwner")
            'arrColumnsNameAction.Add("Priority")


            arrWidthAction.Clear()

            arrWidthAction.Add(10)
            arrWidthAction.Add(10)
            arrWidthAction.Add(50)
            'arrWidthAction.Add(50)
            arrWidthAction.Add(350) '550 '67  %
            'arrWidthAction.Add(31) '35
            arrWidthAction.Add(80) '80
            arrWidthAction.Add(15)
            arrWidthAction.Add(15)
            arrWidthAction.Add(40)
            'arrWidthAction.Add(45)


            grdAction.Width = Unit.Point(590)
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(0)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(1)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(2)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(3)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(4)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(5)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(6)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(7)))
            '   arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(7)))


            'tclAction(0) = New TemplateColumn
            'tclAction(0).Visible = False
            'tclAction(0).HeaderTemplate = New IONGrid.CreateItemTemplateSubmitButton("", "btn")
            'grdAction.Columns.Add(tclAction(0))

            tclAction(0) = New TemplateColumn
            tclAction(0).Visible = True
            tclAction(0).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvAction.Table.Columns(0).ToString, "", dtvAction.Table.Columns(0).ToString + "_H", False, arrColumnsNameAction(0), False)
            tclAction(0).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvAction.Table.Columns(0).ToString, arrImageUrlDisabled(0))
            tclAction(0).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclAction(0).ItemStyle.Width = arrColumnsWidthAction(0)
            grdAction.Columns.Add(tclAction(0))

            tclAction(1) = New TemplateColumn
            tclAction(1).Visible = True
            tclAction(1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader(dtvAction.Table.Columns(1).ToString, "", dtvAction.Table.Columns(1).ToString + "_H", False, arrColumnsNameAction(1), False)
            tclAction(1).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvAction.Table.Columns(1).ToString, arrImageUrlDisabled(1))
            tclAction(1).ItemStyle.HorizontalAlign = HorizontalAlign.Center
            tclAction(1).ItemStyle.Width = arrColumnsWidthAction(1)
            grdAction.Columns.Add(tclAction(1))

            For intCount = 2 To dtvAction.Table.Columns.Count - 1
                tclAction(intCount + 1) = New TemplateColumn
                tclAction(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvAction.Table.Columns(intCount).ToString, dtvAction.Table.Columns(intCount).ToString)
                Dim AddEventOnGrigHeader As New IONGrid.CreateItemTemplateTextBoxForHeader(dtvAction.Table.Columns(intCount).ToString, "", dtvAction.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameAction(intCount), True)
                AddHandler AddEventOnGrigHeader.OnSort, AddressOf SortGridAction
                tclAction(intCount + 1).HeaderTemplate = AddEventOnGrigHeader
                tclAction(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvAction.Table.Columns(intCount).ToString + "_F", False)
                tclAction(intCount + 1).ItemStyle.Width = arrColumnsWidthAction(intCount)     'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthAction(intCount))
                grdAction.Columns.Add(tclAction(intCount + 1))
            Next

            'Call ChangeActionTextBoxWidth()
        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            CreateLog("Call-Detail", "CreateTemplateColumnsAction-2851", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
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
            If Val(ViewState("CompanyID")) = 0 Or Val(ViewState("TaskNo")) = 0 Then
                cpnlTaskAction.Text = "Action View "
                cpnlTaskAction.TitleCSS = "test2"
                cpnlTaskAction.Enabled = False
                cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            Else
                cpnlTaskAction.Text = "Action View (Task# " & ViewState("TaskNo") & " &nbsp;&nbsp;Company:" & DDLCustomer.SelectedItem.Text & ")"
                cpnlTaskAction.TitleCSS = "test"
                cpnlTaskAction.Enabled = True
                cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
            End If
            If Session("PropCompanyType") = "SCM" Then

                '    If strWhereClause.Trim.Equals("") Then
                strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_CH1_Mandatory,AM_FL8_Used_Hr,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID From T040031 a,T060011 b   Where AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And AM_NU9_Task_Number=" & Val(ViewState("TaskNo"))
                'Else
                '    strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID ," & _
                '     " convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date From T040031 a,T060011 b  Where AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And AM_NU9_Task_Number=" & Val(ViewState("TaskNo")) & " And " & strWhereClause
                'End If
            Else
                '  If strWhereClause.Trim.Equals("") Then
                strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_CH1_Mandatory,AM_FL8_Used_Hr,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID From T040031 a,T060011 b   Where AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and a.AM_VC8_ActionType='External' And b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And AM_NU9_Task_Number=" & Val(ViewState("TaskNo"))
                'Else
                '    strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID ," & _
                '     " convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date From T040031 a,T060011 b  Where AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and AM_NU9_Call_Number=" & Val(ViewState("CallNo")) & " and a.AM_VC8_ActionType='External' And b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And AM_NU9_Task_Number=" & Val(ViewState("TaskNo")) & " And " & strWhereClause
                ' End If
            End If
            strSql = strSql & " Order By AM_NU9_Action_Number  asc"

            Call SQL.Search("T040031", "Call_Detail", "CreateDataTableAction-2594", strSql, dsTask, "sachin", "Prashar")
            dtvAction = New DataView
            dtvAction.Table = dsTask.Tables(0)

            Dim htDateCols As New Hashtable
            htDateCols.Add("AM_DT8_Action_Date", 1)
            SetDataTableDateFormat(dtvAction.Table, htDateCols)

            If Not strWhereClause.Trim.Equals("") Then
                GetFilteredDataView(dtvAction, strWhereClause)
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


            If dsTask.Tables(0).Rows.Count = 0 Then
            Else
                If imgAddAttachment.Src.Equals("../../Images/Attach_Yellow.gif") = False Then
                    For intI As Integer = 0 To dsTask.Tables(0).Rows.Count - 1
                        If dsTask.Tables(0).Rows(intI).Item("Blank2").Equals("1") = True Then
                            imgAddAttachment.Src = "../../Images/Attach_Yellow.gif"
                            imgAttachments.ToolTip = "View Attachment"
                            Exit For
                        Else
                            imgAddAttachment.Src = "../../Images/Attach15_9.gif"
                            imgAttachments.ToolTip = "No Attachment Uploaded"
                        End If
                    Next
                End If

            End If


        Catch ex As Exception
            CreateLog("Call_Detail", "CreateDataTableAction-2940", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

#End Region

#Region "Fill Action Header Array"
    Private Sub FillHeaderArrayAction()
        Dim t As New Control
        Dim intCount As Integer
        Try
            arrHeadersAction.Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvAction.Table.Columns.Count - 1
                    arrHeadersAction.Add(Request.Form("cpnlTaskAction$grdAction$_ctl01$" + dtvAction.Table.Columns(intCount).ColumnName + "_H"))
                Next
            End If
        Catch ex As Exception
            CreateLog("Call-Detail", "FillHeaderArrayAction-3016", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub
#End Region

#Region "Fill Action Footer Array"
    Private Sub FillFooterArrayAction()
        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        Try
            arrFooterAction.Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvAction.Table.Columns.Count - 1
                    intFooterIndex = dtvAction.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                    arrFooterAction.Add(Request.Form("cpnlTaskAction:grdAction:_ctl01" & intFooterIndex.ToString.Trim & ":" + dtvAction.Table.Columns(intCount).ColumnName + "_F"))
                Next
            End If
        Catch ex As Exception
            CreateLog("Call_Detail", "FillFooterArrayAction-2982", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Sub
#End Region

#Region "Bind Action Grid"
    Private Sub BindGridAction()
        Try

            Dim htGrdColumns As New Hashtable
            htGrdColumns.Add("AM_VC_2000_Description", 40)

            HTMLEncodeDecode(mdlMain.Action.Encode, dtvAction, htGrdColumns)
            SetCommentFlag(dtvAction, mdlMain.CommentLevel.ActionLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            grdAction.DataSource = dtvAction
            grdAction.DataBind()

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
            CreateLog("Call_Detail", "BindGridAction-2981", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
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
        Dim structTempActionOwner As Owners '-- will keep Action owner's ID and Name
        dg.PageSize = 1
        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If grdAction.DataKeys(0) <> 0 Then
                    For intCount = 0 To 1       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        ActionNo = grdAction.DataKeys(e.Item.ItemIndex)
                        If strSelected = "1" Then      'If comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'A','" & intCount & "'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'A','" & intCount & "'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
                        Else       ' If no comment/attachment is attached
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'A','" & intCount & "'," & ViewState("CallNo") & "," & ViewState("TaskNo") & "," & ViewState("CompanyID") & ")")
                        End If
                    Next
                    For intCount = 2 To dtvAction.Table.Columns.Count - 1       'for Others
                        If dtvAction.Table.Columns(intCount).DataType.FullName.Equals("System.DateTime") Then
                            If dtBound.Rows(cnt)(intCount).ToString Is Null Or dtBound.Rows(cnt)(intCount).ToString = "" Then
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = " "
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = CType(dtBound.Rows(cnt)(intCount).ToString, DateTime).ToShortDateString
                            End If
                        Else
                            If intCount = 7 Then ' -- for action owner cell
                                structTempActionOwner.Id = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(1)
                                structTempActionOwner.Name = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = structTempActionOwner.Name
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString)
                            End If
                        End If

                        ActionNo = grdAction.DataKeys(e.Item.ItemIndex)
                        Dim CompID As Integer = ViewState("CompanyID")
                        Dim CallNo As Integer = ViewState("CallNo")
                        Dim TaskNo As Integer = ViewState("TaskNo")
                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheckAction('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & CompID & "','" & CallNo & "','" & TaskNo & "')")
                        If intCount = 7 Then ' for action owner
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & structTempActionOwner.Id & "')")
                        Else ' for cells other than action owner
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheckTaskEdit('" & ActionNo & "', '" & mActionRowValue + 1 & "', 'cpnlTaskAction_grdAction','" & CompID & "','" & CallNo & "','" & TaskNo & "')")
                        End If
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
        Catch ex As Exception
            CreateLog("Call_Detail", "grdaction_ItemDataBound-3039", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try

    End Sub

    Private Sub FillCallFromTemplate()
        Try
            Dim sqrdCall As SqlDataReader
            If IsNothing(mstrCompany) Then
                mstrCompany = Session("PropCompanyID")
            End If
            mstGetFunctionValue = WSSSearch.SearchTemplateCall(Val(TxtTmplId.Text), mstrCompany, sqrdCall)
            If mstGetFunctionValue.ErrorCode = 0 Then
                While sqrdCall.Read
                    DDLProject.SelectedValue = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Project_ID")), "", sqrdCall.Item("TCM_NU9_Project_ID"))
                    'CDDLCallOwner.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Call_Owner")), "", sqrdCall.Item("TCM_NU9_Call_Owner")), False, sqrdCall.Item("CI_VC36_Name"))
                    CDDLCallOwner.SelectedValue = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Call_Owner")), "", sqrdCall.Item("TCM_NU9_Call_Owner"))
                    ' CDDLCoordinator.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Coordinator")), "", sqrdCall.Item("TCM_NU9_Coordinator")), False, sqrdCall.Item("Coordinator"))
                    FillNonUDCDropDown(DDLCoordinator, " SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,T010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where T010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk  and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(DDLProject.SelectedValue) & " and PM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name", True)
                    Try
                        DDLCoordinator.SelectedValue = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Coordinator")), "", sqrdCall.Item("TCM_NU9_Coordinator"))
                    Catch ex As Exception
                    End Try
                    CDDLCategory.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Category")), "", sqrdCall.Item("TCM_VC8_Category")), True, IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Category")), "", sqrdCall.Item("TCM_VC8_Category")))
                    CDDLCauseCode.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Cause_Code")), "", sqrdCall.Item("TCM_VC8_Cause_Code")), True, IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Cause_Code")), "", sqrdCall.Item("TCM_VC8_Cause_Code")))
                    'CDDLCallType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Call_Type")), "", sqrdCall.Item("TCM_VC8_Call_Type")))
                    CDDLCallType.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Call_Type")), "", sqrdCall.Item("TCM_VC8_Call_Type"))
                    'add for priority
                    CDDLPriority.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC200_Work_Priority")), "", sqrdCall.Item("TCM_VC200_Work_Priority")))
                    '  txtCatCode1.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_1")), "", sqrdCall.Item("TCM_NU9_Category_Code_1"))
                    '  txtCatCode2.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_2")), "", sqrdCall.Item("TCM_NU9_Category_Code_2"))
                    '  txtCatCode3.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_3")), "", sqrdCall.Item("TCM_NU9_Category_Code_3"))
                    mstGetFunctionValue = WSSSearch.SearchCompNameID(sqrdCall.Item("TCM_NU9_CompId_FK"))
                    DDLCustomer.SelectedValue = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_CompId_FK")), "", sqrdCall.Item("TCM_NU9_CompId_FK"))
                    txtDescription.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC2000_Call_Desc")), "", sqrdCall.Item("TCM_VC2000_Call_Desc"))
                    CDDLStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC200_Work_Priority")), "", sqrdCall.Item("TCM_VC200_Work_Priority")))
                    'txtProject.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC50_Project")), "", sqrdCall.Item("TCM_VC50_Project"))
                    txtReference.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC50_Reference_Id")), "", sqrdCall.Item("TCM_VC50_Reference_Id"))
                    'Added on 06-07-09
                    txtCateCode1.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_1")), "", sqrdCall.Item("TCM_NU9_Category_Code_1"))
                    txtCateCode2.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_2")), "", sqrdCall.Item("TCM_NU9_Category_Code_2"))
                    '-----------------
                    txtSubject.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC100_Subject")), "", sqrdCall.Item("TCM_VC100_Subject"))
                    dtEstFinishDate.Text = IIf(IsDBNull(sqrdCall.Item("TCM_DT8_Close_Date")), "", sqrdCall.Item("TCM_DT8_Close_Date"))
                    CDDLStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC20_Call_Status")), "", sqrdCall.Item("TCM_VC20_Call_Status")))
                    ' TxtProject_F.Text = txtProject.Text
                    TxtTmplName.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Template")), "", sqrdCall.Item("TCM_VC8_Template"))
                    'CDDLTemplateType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Tmpl_Type")), "", sqrdCall.Item("TCM_VC8_Tmpl_Type")))
                    DDLTemplType.SelectedValue = IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Tmpl_Type")), "", sqrdCall.Item("TCM_VC8_Tmpl_Type"))
                    mflgTemplateValueFilled = True    ' First Time value field from template
                    mflgChangeTemplate = False    'Changed Value Saved
                End While
                sqrdCall.Close()
                If DDLTemplType.SelectedValue = "CAO" Then
                    txtChangeTemplate.Value = "0"
                End If
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
            End If
        Catch ex As Exception
            CreateLog("Call_Detail", "FillCallFromTemplate-3540", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub DDLCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLCustomer.SelectedIndexChanged

        ViewState("CompanyID") = DDLCustomer.SelectedValue
        mstrCompany = DDLCustomer.SelectedValue

        DDLAgreement.Items.Clear()
        DDLAgreement.Items.Add("")
        DDLCoordinator.Items.Clear()
        DDLCoordinator.Items.Add("")

        'CDDLCallOwner.CDDLSetBlank()
        CDDLCallOwner.Text = ""
        DDLCoordinator.SelectedValue = ""
        CDDLCategory.CDDLSetBlank()
        CDDLCallCategory.CDDLSetBlank()
        CDDLCauseCode.CDDLSetBlank()

        'CDDLCallType.CDDLSetBlank()
        CDDLCallType.Text = ""
        CDDLPriority.CDDLSetBlank()
        CDDLPriority_F.Text = ""
        CDDLStatus.CDDLSetBlank()
        CDDLTaskType_F.Text = ""
        FillCustomDDl(True)
        ' Fill Project According to customer selected
        FillNonUDCDropDown(DDLProject, "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and PR_Vc8_Status='Enable' order by PR_VC20_Name", True)
    End Sub

    Private Sub FillRadCombos()
        Dim dsComboCallType As New DataSet
        Dim dsComboRequestedBy As New DataSet
        Dim dsComboTaskType As New DataSet

        Dim dsComboPriority As New DataSet
        Try
            dsComboCallType = objCommonFunctionsBLL.fillRadCallTypeDDL(Val(ViewState("CompanyID")))
            CDDLCallType.Items.Clear()
            CDDLCallType.DataSource = dsComboCallType
            For Each data As DataRow In dsComboCallType.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("ID"))
                item.Value = CStr(data("ID"))
                CDDLCallType.Items.Add(item)
                item.DataBind()
            Next
            dsComboRequestedBy = objCommonFunctionsBLL.fillRadRequestedByDDL(Val(ViewState("CompanyID")))
            CDDLCallOwner.Items.Clear()
            CDDLCallOwner.DataSource = dsComboRequestedBy
            For Each data As DataRow In dsComboRequestedBy.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("Name"))
                item.Value = CStr(data("ID"))
                CDDLCallOwner.Items.Add(item)
                item.DataBind()
            Next
            dsComboTaskType = objCommonFunctionsBLL.FillRadTaskType(Val(ViewState("CompanyID")))
            CDDLTaskType_F.Items.Clear()
            CDDLTaskType_F.DataSource = dsComboTaskType
            For Each data As DataRow In dsComboTaskType.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("ID"))
                item.Value = CStr(data("ID"))
                CDDLTaskType_F.Items.Add(item)
                item.DataBind()
            Next


            ' Task Priority Combo
            dsComboPriority = objCommonFunctionsBLL.FillRadTaskPriority(Val(ViewState("CompanyID")))
            CDDLPriority_F.Items.Clear()
            CDDLPriority_F.DataSource = dsComboPriority
            For Each data As DataRow In dsComboPriority.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("ID"))
                item.Value = CStr(data("ID"))
                CDDLPriority_F.Items.Add(item)
                item.DataBind()
            Next
        Catch ex As Exception
            CreateLog("Call_Detail", "FillRadCombos-3217", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub FillCustomDDl(Optional ByVal CustomerChanged As Boolean = False)
        Try
            ' -- Call Type
            If Val(ViewState("CompanyID")) = 0 Then
                Return
            End If
            Dim strSQLQuery As String
            strSQLQuery = "select CT_VC8_CallType_FK as ID,Description as Description, isnull(CI_VC36_Name,'') Company  from T040103,UDC ,T010011 where CT_BT1_CallEnteryFlag =1 and UDCType='CALL' and CT_NU9_CompID_FK in (0," & Val(ViewState("CompanyID")) & ") and CT_VC8_CallType_FK=name and CI_NU8_Address_Number=*CT_NU9_CompID_FK Order By Name"
            '  CDDLCallType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CALL' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CALL' and UDC.Company=0 Order By Name"
            'CDDLCallType.CDDLQuery = strSQLQuery
            ' End If
            '''''functions to Load the RAD COMBOS
            FillRadCombos()
            '''''''''''following code added by Tarun Pahuja on 7-Aug'09 to fill RadCombo
            ' CDDLCallType.CDDLMandatoryField = True
            '   CDDLCallType.CDDLUDC = True
            '------------------------------------------
            ' -- Task Type
            'CDDLTaskType_F.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='TKTY' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='TKTY' and UDC.Company=0 Order By Name"
            'CDDLTaskType_F.CDDLMandatoryField = True
            'CDDLTaskType_F.CDDLUDC = True
            '------------------------------------------
            '--Priority
            CDDLPriority.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='PRIO' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='PRIO' and UDC.Company=0 Order By Name"
            CDDLPriority.CDDLMandatoryField = True
            CDDLPriority.CDDLUDC = True
            '------------------------------------------
            '--Call Status
            CDDLStatus.CDDLQuery = "select SU_VC50_Status_Name as ID,SU_VC500_Status_Description as description,CI_VC36_Name as Company  from T040081,T010011 Where (SU_NU9_ScreenID=3  or SU_NU9_ScreenID=0) and SU_NU9_CompID*=CI_NU8_Address_Number and SU_NU9_CompID=" & ViewState("CompanyID") & "  union select SU_VC50_Status_Name as Name,SU_VC500_Status_Description as description,'' as Company  from T040081 Where (SU_NU9_ScreenID=3  or SU_NU9_ScreenID=0) and SU_NU9_ID_PK>3 and SU_NU9_CompID=0 Order By ID"
            CDDLStatus.CDDLMandatoryField = True
            '------------------------------------------
            '--Priority
            '  CDDLPriority_F.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='PRIO' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='PRIO' and UDC.Company=0 Order By Name"
            '  CDDLPriority_F.CDDLUDC = True
            '--Category
            CDDLCategory.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CATG' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CATG' and UDC.Company=0 Order By Name"
            CDDLCategory.CDDLUDC = True
            '--Knowledge DB
            CDDLCallCategory.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CCAT' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CCAT' and UDC.Company=0 Order By Name"
            CDDLCallCategory.CDDLUDC = True
            '--Cause Code
            CDDLCauseCode.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CACD' and UDC.Company=" & ViewState("CompanyID") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CACD' and UDC.Company=0 Order By Name"
            CDDLCauseCode.CDDLUDC = True
            '--Action Owner
            ' CDDLActionOwner_F.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and ( um_in4_company_ab_id in (select ci_nu8_address_number from t010011 where ci_in4_business_relation='SCM')) Order By Name"
            ' CDDLActionOwner_F.CDDLMandatoryField = True
            '  CDDLActionOwner_F.CDDLUDC = False
            '------------------------------------------
            '--Requested By
            'CDDLCallOwner.CDDLQuery = "SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,t010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where UM_VC4_Status_Code_FK='ENB' and t010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and (um_in4_company_ab_id=" & ViewState("CompanyID") & ") and UM_IN4_Company_AB_ID=" & ViewState("CompanyID") & " Order By Name"
            'CDDLCallOwner.CDDLMandatoryField = True
            'CDDLCallOwner.CDDLUDC = False
            'CDDLCallOwner.CDDLFillDropDown(10, False)
            '-----------------------------------------
            '--Coordinator
            'CDDLCoordinator.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where UM_VC4_Status_Code_FK='ENB' and ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id=" & ViewState("CompanyID") & ") and UM_IN4_Company_AB_ID=" & ViewState("CompanyID") & " Order By Name"
            'CDDLCoordinator.CDDLQuery = " SELECT um_in4_address_no_fk as ID,(um_vc50_userid + ' [' + ci_vc36_name + ']') as Name,ci_vc36_name  as Company FROM t060011,t010011 where UM_VC4_Status_Code_FK='ENB' and ci_nu8_address_number=um_in4_company_ab_id and (T010011.CI_IN4_Business_Relation='SCM' or UM_IN4_Company_AB_ID=" & Session("PropCompanyID") & ") and UM_IN4_Company_AB_ID in (" & GetCompanySubQuery() & ") Order By Name"
            'CDDLCoordinator.CDDLMandatoryField = True
            'CDDLCoordinator.CDDLUDC = False
            'CDDLCoordinator.CDDLFillDropDown(10, False)
            ''-----------------------------------------
            If IsPostBack = False Or CustomerChanged = True Then
                CDDLStatus.CDDLFillDropDown(10, True)
                CDDLStatus.CDDLSetSelectedItem("OPEN")
                'CDDLCallType.CDDLFillDropDown(10, True)
                ' CDDLTaskType_F.CDDLFillDropDown(10)
                ' CDDLTaskType_F.CDDLType = CustomDDL.DDLType.FastEntry
                CDDLPriority.CDDLFillDropDown(10, True)
                ' CDDLPriority_F.CDDLFillDropDown(10)
                'CDDLPriority_F.CDDLType = CustomDDL.DDLType.FastEntry
                CDDLCategory.CDDLFillDropDown(10)
                CDDLCallCategory.CDDLFillDropDown(10)
                CDDLCauseCode.CDDLFillDropDown(10)
            End If
        Catch ex As Exception
            CreateLog("Call_Detail", "FillCustomDDL-4243", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub SetFieldsAfterProjectChange()
        Try
            ViewState("PropProjectID") = Val(DDLProject.SelectedValue)
            Dim dsComboTaskOwner As New DataSet
            ' Task Owner Combo
            dsComboTaskOwner = objCommonFunctionsBLL.FillRadTaskOwner(Val(ViewState("CompanyID")), Val(ViewState("PropProjectID")))
            DDLTaskOwner.Items.Clear()
            DDLTaskOwner.DataSource = dsComboTaskOwner
            For Each data As DataRow In dsComboTaskOwner.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("Name"))
                item.Value = CStr(data("ID"))
                DDLTaskOwner.Items.Add(item)
                item.DataBind()
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DisableCallInfo()
        Try
            CDDLPriority.Enabled = False
            CDDLCallType.Enabled = False
            CDDLCallOwner.Enabled = False
            DDLCoordinator.Enabled = False
            CDDLCategory.Enabled = False
            CDDLCallCategory.Enabled = False
            CDDLCauseCode.Enabled = False
            txtCallRef.ReadOnly = True

            DDLTemplType.Enabled = False
            TxtTmplName.Enabled = False
            'imgAttachments.Enabled = False
            DDLAgreement.Enabled = False
            'dtEstFinishDate.readOnlyDate = False
            'txtCatCode1.ReadOnly = True
            'txtCatCode2.ReadOnly = True
            txtReference.ReadOnly = True
            'txtCatCode3.ReadOnly = True
            txtSubject.ReadOnly = True
            txtDescription.ReadOnly = True
        Catch ex As Exception
            CreateLog("Call_View", "DisableCallInfo-4370", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub EnableCallInfo()
        Try

            CDDLPriority.Enabled = True
            CDDLCallType.Enabled = True
            CDDLCallOwner.Enabled = True

            DDLCoordinator.Enabled = True
            CDDLCategory.Enabled = True
            CDDLCallCategory.Enabled = True
            CDDLCauseCode.Enabled = True
            txtCallRef.ReadOnly = False

            DDLTemplType.Enabled = True
            TxtTmplName.Enabled = True
            imgAttachments.Enabled = True

            'dtEstFinishDate.readOnlyDate = True
            'txtCatCode1.ReadOnly = False
            'txtCatCode2.ReadOnly = False
            txtReference.ReadOnly = False
            'txtCatCode3.ReadOnly = False
            txtSubject.ReadOnly = False
            txtDescription.ReadOnly = False

        Catch ex As Exception
            CreateLog("Call_View", "EnableCallInfo-4392", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
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

    Public Function FillTemplTypeDropDown(ByVal ddlCustom As DropDownList, ByVal strSQL As String, Optional ByVal OptionalField As Boolean = False) As Boolean
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            If OptionalField = True Then
                ddlCustom.Items.Add(New ListItem("All Types", "All Types"))
            End If
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                sqRDR.Close()
                Return True
            End If
        Catch ex As Exception
            CreateLog("Call Entry", "FillTemplTypeDropDown", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
            Return False
        End Try
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'date 7/12/2006 
        '-----------------------------------------------
        'to store current viewstate value in session to stop f5 duplicate data while pressing f5 in data entry
        ViewState("update") = ViewState("Update")
        '-----------------------------------------------
    End Sub

    Private Function CheckCallExists(ByVal CallNumber As Integer) As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If Val(SQL.Search("", "", "select CM_NU9_Call_No_PK from T040011 where CM_NU9_Comp_Id_FK=" & DDLCustomer.SelectedValue & " and CM_NU9_Call_No_PK=" & CallNumber)) = CallNumber Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Call Entry", "CheckCallExists", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function SaveCallStatusComments(ByVal OldStatus As String, ByVal NewStatus As String) As Boolean
        Try
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            arColumnName.Add("CM_NU9_Comment_To")
            arColumnName.Add("CM_CH1_Flag")
            arColumnName.Add("CM_NU9_AB_Number")
            arColumnName.Add("CM_DT8_Date")
            arColumnName.Add("CM_VC256_Comments")
            arColumnName.Add("CM_VC2_Flag")
            arColumnName.Add("CM_NU9_Call_Number")
            arColumnName.Add("CM_NU9_Task_Number")
            arColumnName.Add("CM_NU9_Action_Number")
            arColumnName.Add("CM_VC30_Type")
            arColumnName.Add("CM_NU9_CompId_Fk")
            arColumnName.Add("CM_VC50_IE")
            arColumnName.Add("CM_VC1000_MailList")
            arColumnName.Add("CM_CH1_MailSent")
            arColumnName.Add("CM_CH1_Status_Comment")
            'arRowData.Add(IIf(CDDLCallOwner.CDDLGetValue = "", System.DBNull.Value, CDDLCallOwner.CDDLGetValue))
            arRowData.Add(IIf(CDDLCallOwner.SelectedValue = "", System.DBNull.Value, CDDLCallOwner.SelectedValue))
            arRowData.Add("1") 'Read Status Flag
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(Now)
            arRowData.Add("Call Status changed from [" & OldStatus & "] to [" & NewStatus & "] by " & Session("PropUserName"))
            arRowData.Add("C")
            arRowData.Add(ViewState("CallNo"))
            arRowData.Add(0)
            arRowData.Add(0)
            arRowData.Add("F")
            arRowData.Add(ViewState("CompanyID"))
            arRowData.Add("External")
            arRowData.Add("")  'SQL.Search("", "", "select CI_VC28_Email_1 from T010011 where CI_NU8_Address_Number=" & Val(CDDLCallOwner.CDDLGetValue)))
            arRowData.Add("F")
            arRowData.Add("T")

            mstGetFunctionValue = WSSSave.SaveComments(arColumnName, arRowData)

            If mstGetFunctionValue.ErrorCode = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Call Entry", "SaveStatusComments-3527", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

#Region "Save Action Fast Entry"
    Private Function SaveAction() As Boolean
        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short
        Dim strUDCType As String
        Dim intCallNo As Integer
        Dim blnCheckValidation As Boolean
        Dim strSql As String

        'CDDLActionOwner_F.CDDLGetValue.Trim.Equals("") OrElse dtActionDate.CalendarDate.Trim.Equals("") OrElse
        If TxtDescription_F.Text.Trim.Equals("") Then
            blnCheckValidation = False
            Exit Function
        Else
            blnCheckValidation = True
        End If
        lstError.Items.Clear()

        If blnCheckValidation = False Then    'Exit If all textbox are blank
            SaveAction = False
            Exit Function
        End If
        'Dim strchkcallstatus As String = SQL.Search("Call_Detail", "SaveAction-2789", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")
        'If IsNothing(strchkcallstatus) = False Then
        Dim strStatus As String
        strStatus = GetStatus(ViewState("CompanyID"), ViewState("CallNo"), mdlMain.StatusType.CallStatus)
        If strStatus = "CLOSED" Then
            lstError.Items.Clear()
            lstError.Items.Add("Call Closed so You can not Add the Action")
            Return False
            Exit Function
        End If

        'Check Dependency status
        '****************************************************************
        'SQL.DBTable = "T040021"
        SQL.DBTracing = False
        Dim flgDependency As Boolean
        Dim introws As Integer
        strSql = "select * from t040021 where tm_nu9_call_no_fk=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk=" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED' " & _
        " and tm_nu9_task_no_pk=(select tm_nu9_dependency from t040021 " & _
        " where tm_nu9_call_no_fk=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk=" & ViewState("CompanyID") & " and tm_nu9_task_no_pk=" & ViewState("TaskNo") & " ) "
        flgDependency = SQL.Search("Call_Detail", "SaveAction-2930", strSql, introws)

        If flgDependency = True Then
            lstError.Items.Clear()

            DisplayWaring("Please check Task Dependency")
            TxtSubject_F.Text = ""
            TxtDescription_F.Text = ""
            Return False
            Exit Function
        End If
        'If IsNothing(strchktaskstatus) = False Then
        Session("TaskNumber") = Request.Form("txthiddenSkil")
        strStatus = GetStatus(ViewState("CompanyID"), ViewState("CallNo"), mdlMain.StatusType.TaskStatus, Session("TaskNumber"))
        If strStatus = "CLOSED" Then
            lstError.Items.Clear()
            lstError.Items.Add("Task Closed so You can not Add the Action")
            Return False
            Exit Function
        End If
        '**************************************************************************

        lstError.Items.Clear()

        Dim dtTaskDate As Date = SQL.Search("Call_Detail", "SaveAction-2825", "Select tm_DT8_Task_Date from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNo") & " and TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & "")

        If TxtUsedHr_F.Text.Trim.Equals("") And chkMandatoryHr.Checked = True Then
            lstError.Items.Add("Used hours cannot be blank")
            shFlag = 1
        ElseIf TxtUsedHr_F.Text.Trim.Equals("") = False Then
            If IsNumeric(TxtUsedHr_F.Text.Trim) = False Then
                lstError.Items.Add("Used hour is not numeric")
                shFlag = 1
            End If
        End If

        If CDDLActionOwner_F.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Action Owner cannot be blank")
            shFlag = 1
        ElseIf dtActionDate.Text.Trim <> "" Then
            If IsDate(dtActionDate.Text) = False Then
                lstError.Items.Add("Check Action date format")
                shFlag = 1
                'ElseIf CDate(dtActionDate.Text.Trim) < CDate(dtTaskDate.ToShortDateString.Trim) Then
                '    lstError.Items.Add("Action date cannot be less than Task Date")
                '    shFlag = 1
            ElseIf CDate(dtActionDate.Text.Trim) < dtTaskDate.ToString("yyyy-MMM-dd") Then
                lstError.Items.Add("Action date cannot be less than Task Date...")
                shFlag = 1
            ElseIf CDate(dtActionDate.Text.Trim) > Now.ToShortDateString Then
                lstError.Items.Add("Action date cannot be more than current date")
                shFlag = 1
            End If
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Exit Function
        End If

        lstError.Items.Clear()
        strUDCType = "ACT"

        Dim intAddressNo As Integer
        intAddressNo = SQL.Search("Call_Detail", "SaveAction-2874", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & CDDLActionOwner_F.CDDLGetValue.Trim & "")
        If intAddressNo <= 0 Then
            lstError.Items.Add("Action owner mismatch")
            shFlag = 1
        End If

        If shFlag = 1 Then
            MessagePanelListStyle(lstError, mdlMain.MSG.msgWarning)
            Exit Function
        End If

        Try

            arrColumns.Add("AM_NU9_Call_Number")
            arrColumns.Add("AM_NU9_Task_Number")
            arrColumns.Add("AM_VC_2000_Description")
            'arrColumns.Add("AM_VC100_Action_type")
            arrColumns.Add("AM_NU9_Comp_ID_FK")
            arrColumns.Add("AM_VC8_Supp_Owner")
            arrColumns.Add("AM_DT8_Action_Date_Auto")
            arrColumns.Add("AM_CH1_Comment")
            arrColumns.Add("AM_CH1_Mandatory")
            arrColumns.Add("AM_FL8_Used_Hr")
            If Not dtActionDate.Text.Trim.Equals("") Then
                arrColumns.Add("AM_DT8_Action_Date")
            End If
            arrColumns.Add("AM_NU9_Action_Number")
            arrColumns.Add("AM_VC8_ActionType")

            arrRows.Add(ViewState("CallNo"))
            arrRows.Add(ViewState("TaskNo"))
            'arrRows.Add(TxtActions_F.Text.Trim)
            arrRows.Add(TxtDescription_F.Text.Trim)
            'arrRows.Add(TxtActionType_F.Text.Trim.ToUpper)
            arrRows.Add(DDLCustomer.SelectedValue.Trim)
            arrRows.Add(CDDLActionOwner_F.CDDLGetValue.Trim)
            arrRows.Add(Now.Date)
            arrRows.Add("0")
            If chkMandatoryHr.Checked = True Then
                arrRows.Add("M")
            Else
                arrRows.Add("O")
            End If
            'arrRows.Add(0)
            arrRows.Add(Val(TxtUsedHr_F.Text.Trim))
            If Not dtActionDate.Text.Trim.Equals("") Then
                arrRows.Add(dtActionDate.Text.Trim & " " & Now.ToShortTimeString)
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            intCallNo = SQL.Search("Call_Detail", "SaveAction-2930", "select isnull(max(AM_NU9_Action_Number),0) from T040031 where AM_NU9_Call_Number=" & ViewState("CallNo").ToString & " And AM_NU9_Task_Number=" & ViewState("TaskNo") & " and AM_NU9_Comp_ID_FK=" & ViewState("CompanyID").ToString)
            intCallNo += 1

            arrRows.Add(intCallNo)
            arrRows.Add("External")
            mstGetFunctionValue = WSSSave.SaveAction(arrColumns, arrRows, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"))
            ' If SQL.Save("", "", arrColumns, arrRows) = True Then
            If mstGetFunctionValue.ErrorCode = 0 Then
                mstGetFunctionValue = WSSUpdate.UpdateTaskStatus(ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID"))
                If mstGetFunctionValue.ErrorCode = 0 Then
                    mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("CallNo"), False, ViewState("CompanyID"))
                    If mstGetFunctionValue.ErrorCode = 0 Then
                        CDDLStatus.CDDLSetSelectedItem("PROGRESS")
                        CDDLStatus.Enabled = True
                        'txtStatus.Text = "PROGRESS"
                        DisplayMessage("Action Data Saved Successfully")
                        ViewState("ActionNo") = intCallNo
                        'For intI As Integer = 0 To garAFileID.Count - 1
                        If GetFiles(mdlMain.AttachLevel.ActionLevel) = True Then
                            'shReturn = 1
                        Else
                            'shReturn = 2
                        End If
                        ClearAllTextBox(cpnlTaskAction)
                        garAFileID.Clear()
                        MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        Return True
                    End If
                End If
            Else
                Call DisplayError("Error While Saving Action Info")
                MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                Return False
            End If

        Catch ex As Exception
            CreateLog("CallDetail", "SaveAction-3282", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", False)
            MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
        End Try
    End Function
#End Region

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
                    If CreateTaskAutoAction(ViewState("PropTaskStatus"), "CLOSED", ViewState("CallNo"), ViewState("TaskNo"), ViewState("CompanyID")) = True Then

                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Errors occur please try later...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    End If
                    ViewState("TaskNo") = 0
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

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

#Region "Sorting functions"
    Protected Sub SortGridTask(ByVal sender As Object, ByVal e As CommandEventArgs)
        ViewState("SortOrderTask") = e.CommandArgument
        SortGRDTask()
    End Sub
    Protected Sub SortGridAction(ByVal sender As Object, ByVal e As CommandEventArgs)
        ViewState("SortOrderAction") = e.CommandArgument
        SortGRDAction()
    End Sub
    Private Sub SortGRDTask()
        If Val(ViewState("SortWayTask")) Mod 2 = 0 Then
            dtvTask.Sort = ViewState("SortOrderTask") & " DESC"
        Else
            dtvTask.Sort = ViewState("SortOrderTask") & " ASC"
        End If
        ViewState("SortWayTask") += 1
        mTaskRowValue = 0
        dtgTask.DataSource = dtvTask
        dtgTask.DataBind()
        Call TaskGridSelection()
    End Sub
    Private Sub SortGRDDuplicateTask()
        Try
            If Val(ViewState("SortWayTask")) Mod 2 = 0 Then
                dtvTask.Sort = ViewState("SortOrderTask") & " ASC"
            Else
                dtvTask.Sort = ViewState("SortOrderTask") & " DESC"
            End If
            mTaskRowValue = 0
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
            Call TaskGridSelection()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub SortGRDAction()
        If Val(ViewState("SortWayAction")) Mod 2 = 0 Then
            dtvAction.Sort = ViewState("SortOrderAction") & " DESC"
        Else
            dtvAction.Sort = ViewState("SortOrderAction") & " ASC"
        End If
        ViewState("SortWayAction") += 1
        mActionRowValue = 0
        grdAction.DataSource = dtvAction
        grdAction.DataBind()
    End Sub
    Private Sub SortGRDDuplicateAction()
        Try
            If Val(ViewState("SortWayAction")) Mod 2 = 0 Then
                dtvAction.Sort = ViewState("SortOrderAction") & " ASC"
            Else
                dtvAction.Sort = ViewState("SortOrderAction") & " DESC"
            End If
            mActionRowValue = 0
            grdAction.DataSource = dtvAction
            grdAction.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub TaskGridSelection()
        If Val(ViewState("TaskNo")) > 0 Then
            Dim dgi As DataGridItem
            For Each dgi In dtgTask.Items
                If Val(CType(dgi.Cells(4).FindControl("TM_NU9_Task_no_PK"), Label).Text) = Val(ViewState("TaskNo")) Then
                    dgi.BackColor = Color.FromArgb(212, 212, 212)
                    cpnlTaskAction.Text = "Action View (Task# " & ViewState("TaskNo") & " &nbsp;&nbsp;Company:" & DDLCustomer.SelectedItem.Text & ")"
                    cpnlTaskAction.TitleCSS = "test"
                    cpnlTaskAction.Enabled = True
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
                    Exit For
                Else
                    If dgi.ItemIndex Mod 2 = 0 Then
                        dgi.BackColor = Color.FromArgb(255, 255, 255)
                    Else
                        dgi.BackColor = Color.FromArgb(245, 245, 245)
                    End If
                    cpnlTaskAction.Text = "Action View "
                    cpnlTaskAction.TitleCSS = "test2"
                    cpnlTaskAction.Enabled = False
                    cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
                End If
            Next
        Else
            cpnlTaskAction.Text = "Action View "
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
        End If
        If dtgTask.Items.Count = 0 Then
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.Text = "Action View"
        End If
    End Sub

#End Region

#Region "Email Function"
    Private Sub SendMail()
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Try
            If ViewState("Update").ToString() = ViewState("update").ToString() Then

                If Val(txtCallNumber.Text) > 0 Then
                    Dim strEstDate As String
                    Dim dsTemp As New DataSet
                    lstError.Items.Clear()

                    If ValidateRecords() = False Then
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If

                    If Not dtEstFinishDate.Text.Trim.Equals("") Then
                        If IsDate(dtEstFinishDate.Text) = False Then
                            lstError.Items.Add("Check date format of Estimated Close date...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            Exit Sub
                        Else
                            Dim dtCloseDate As Date = CDate(dtEstFinishDate.Text.Trim)
                            If CDate(Format(CDate(dtCloseDate), "MM/dd/yyyy")) < CDate(Format(CDate(txtCallDate.Text.Trim), "MM/dd/yyyy")) Then
                                lstError.Items.Add("Estimated Close date cannot be less than Call open date...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                                Exit Sub
                            End If
                        End If
                        strEstDate = CStr(dtEstFinishDate.Text.Trim)
                    Else
                        strEstDate = ""
                    End If

                    Dim strSql As String = " select CM_NU9_CustID_FK ,CM_VC8_Call_Type,CM_DT8_Request_Date,CM_NU9_Call_Owner,CM_VC100_By_Whom,CM_VC200_Work_Priority,CM_VC50_Reference_Id,CM_DT8_Close_Date,CM_VC100_Subject,CM_VC2000_Call_Desc,CM_NU9_Project_ID,CN_VC20_Call_Status,CM_VC8_Template,CM_VC8_Tmpl_Type,CM_DT8_Call_Close_Date,CM_NU9_Agreement,CM_NU9_Coordinator,CM_VC8_Category,CM_VC8_Call_Category,CM_VC8_Cause_Code,CM_NU9_Related_Call,CM_NU9_Category_Code_1,CM_NU9_Category_Code_2,CM_DT8_Call_Start_Date from T040011 where CM_NU9_Call_No_PK=" & ViewState("CallNo")
                    If SQL.Search("T130022", "Basicmonitoring", "BindDiskGrid", strSql, dsTemp, "", "") = True Then

                    End If
                    arColumnName.Add("CM_NU9_CustID_FK")
                    arColumnName.Add("CM_VC8_Call_Type")
                    arColumnName.Add("CM_DT8_Request_Date")
                    arColumnName.Add("CM_NU9_Call_Owner")
                    arColumnName.Add("CM_VC100_By_Whom")
                    arColumnName.Add("CM_VC200_Work_Priority")
                    arColumnName.Add("CM_VC50_Reference_Id")
                    arColumnName.Add("CM_DT8_Close_Date")
                    arColumnName.Add("CM_VC100_Subject")
                    arColumnName.Add("CM_VC2000_Call_Desc")
                    arColumnName.Add("CM_NU9_Project_ID")
                    arColumnName.Add("CN_VC20_Call_Status")
                    arColumnName.Add("CM_VC8_Template")
                    If ViewState("TemplateName") <> TxtTmplName.Text Then
                        arColumnName.Add("CM_VC8_Tmpl_Type")
                    End If
                    arColumnName.Add("CM_DT8_Call_Close_Date")
                    arColumnName.Add("CM_NU9_Agreement")
                    arColumnName.Add("CM_NU9_Coordinator")
                    arColumnName.Add("CM_VC8_Category")
                    arColumnName.Add("CM_VC8_Call_Category")
                    arColumnName.Add("CM_VC8_Cause_Code")
                    arColumnName.Add("CM_NU9_Related_Call")
                    arColumnName.Add("CM_NU9_Category_Code_1")
                    arColumnName.Add("CM_NU9_Category_Code_2")
                    arColumnName.Add("CM_DT8_Call_Start_Date") 'Call start date new field

                    'Row Data
                    arRowData.Add(Val(ViewState("CustID")))
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC8_Call_Type").ToString.Trim.ToUpper)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_DT8_Request_Date").ToString.Trim)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_NU9_Call_Owner").ToString.Trim.ToUpper)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC100_By_Whom").ToString)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC200_Work_Priority").ToString.Trim.ToUpper)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC50_Reference_Id").ToString.Trim)
                    If dsTemp.Tables(0).Rows(0)("CM_DT8_Close_Date").ToString.Trim.Equals("") = True Then
                        arRowData.Add(DBNull.Value)
                    Else
                        arRowData.Add(CDate(dsTemp.Tables(0).Rows(0)("CM_DT8_Close_Date").ToString.Trim))
                    End If

                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC100_Subject").ToString.Trim)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC2000_Call_Desc").ToString.Trim)
                    arRowData.Add(Val(dsTemp.Tables(0).Rows(0)("CM_NU9_Project_ID").ToString.Trim))
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CN_VC20_Call_Status").ToString.Trim.ToUpper)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC8_Template").ToString.Trim)

                    If ViewState("TemplateName") <> dsTemp.Tables(0).Rows(0)("CM_VC8_Template").ToString.Trim Then
                        arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC8_Tmpl_Type").ToString)
                    End If

                    If dsTemp.Tables(0).Rows(0)("CM_DT8_Call_Close_Date").ToString.Trim.Equals("") = True Then
                        arRowData.Add(DBNull.Value)
                    Else
                        arRowData.Add(CDate(dsTemp.Tables(0).Rows(0)("CM_DT8_Call_Close_Date").ToString.Trim))
                    End If

                    arRowData.Add(Val(dsTemp.Tables(0).Rows(0)("CM_NU9_Agreement").ToString))
                    If dsTemp.Tables(0).Rows(0)("CM_NU9_Coordinator").ToString.Equals("") = True Then
                        arRowData.Add(DBNull.Value)
                    Else
                        arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_NU9_Coordinator").ToString)
                    End If

                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC8_Category").ToString)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC8_Call_Category").ToString)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_VC8_Cause_Code").ToString)
                    arRowData.Add(Val(dsTemp.Tables(0).Rows(0)("CM_NU9_Related_Call").ToString.Trim))
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_NU9_Category_Code_1").ToString.Trim)
                    arRowData.Add(dsTemp.Tables(0).Rows(0)("CM_NU9_Category_Code_2").ToString.Trim)
                    'Add new call start date
                    If dsTemp.Tables(0).Rows(0)("CM_DT8_Call_Start_Date").ToString.Trim.Equals("") = True Then
                        arRowData.Add(DBNull.Value)
                    Else
                        arRowData.Add(CDate(dsTemp.Tables(0).Rows(0)("CM_DT8_Call_Start_Date").ToString.Trim))
                    End If

                    If mstrPrvStatus <> "" And mstrPrvStatus <> CDDLStatus.CDDLGetValue Then
                        'If status is changed manually then pass true to Update Call function
                        mstGetFunctionValue = WSSUpdate.SaveEmailData(ViewState("CallNo"), ViewState("CompanyID"), arColumnName, arRowData, True, dsTemp.Tables(0).Rows(0)("CN_VC20_Call_Status").ToString.Trim.ToUpper)
                    Else
                        mstGetFunctionValue = WSSUpdate.SaveEmailData(ViewState("CallNo"), ViewState("CompanyID"), arColumnName, arRowData, False, dsTemp.Tables(0).Rows(0)("CN_VC20_Call_Status").ToString.Trim.ToUpper)
                    End If

                    If mstGetFunctionValue.ErrorCode = 0 Then
                        ViewState("Update") = Server.UrlEncode(System.DateTime.Now.ToString())
                        lstError.Items.Clear()
                        lstError.Items.Add("Mail send Sucessfully ...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    End If
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Please save the call ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                End If
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("Call_Detail", "Save Call-1017", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Sub
#End Region
    Protected Sub imgAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click
        ViewState.Clear()
        Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=4", False)
    End Sub
End Class
