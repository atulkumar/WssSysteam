Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data
Imports WSSBLL
Imports Telerik.Web.UI

Partial Class SupportCenter_CallView_Call_View
    Inherits System.Web.UI.Page
    Protected WithEvents dtgTask As New DataGrid
    '*******************************************************************
    ' Function             :-  global declaration
    ' Purpose              :- variable declartion
    ' Date					  Author					        	Modification Date					Description
    ' 2/5/06			      Sachin/Harpreet/Ranvijay   -------------------					Created

    '*******************************************************************
#Region "global level declaration"

    Private mdvtable As New DataView ' store data from table for view grid 
    Private dtvTask As New DataView
    Private objCommonFunctionsBLL As New clsCommonFunctionsBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 
    Public introwvalues As Integer 'stored the selected row's value
    'these variable store the position of the columns
    '****************************************
    Private mintCompId As String
    Private mintSuppComp As String
    Private mintCallOwnerID As String
    Private mintByWhomID As String
    Private mintCallNoRowID As String
    Private CoordinatorColumnNo As String
    Private RelatedCallColumnNo As String
    '************************************
    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer
    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************
    Private arrtextvalue As ArrayList = New ArrayList  'textboxes values
    '*************************************************************
    Private intColumnCount As Integer  'grid columns count
    Private txthiddenImage As String 'stored clicked button's cation  
    Public mstrsuppcomp As String 'stores the support comp during postbacks
    Private mintFileID As Integer
    Private mTaskRowValue As Integer
    Private intComp As String
    Public mstrcomp As String
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Public mstrCallNumber As String
    Public PropCallStatus As String
    Private Shared arrHeadersTask As New ArrayList
    Private Shared arrFooterTask As New ArrayList
    Private Shared arrColumnsNameTask As New ArrayList
    Private Shared arrWidthTask As New ArrayList
    Private Shared arrColumnsWidthTask As New ArrayList
    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments
    Private null As System.DBNull
    Private tclTask() As TemplateColumn

#End Region
    '*******************************************************************
    ' Function             :-  page_load
    ' Purpose              :- fill data in grd and load data and design datagrid and chk security at page load time 
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin/Harpreet/Ranvijay		    -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        'lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        '***********************************
        'to store value in session to stop f5 duplicate data while pressing f5 in data entry

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        If Request.QueryString("ScreenFrom") = "HomePage" Then
            'imgClose.Visible = False
        End If
        If Not Page.IsPostBack Then
            ViewState("update") = Server.UrlEncode(System.DateTime.Now.ToString())
            ViewState("SortOrder") = Nothing
            ViewState("SortWay") = 0
            ViewState("SortWayTask") = Nothing
            ViewState("SortOrderTask") = Nothing
            Dim arColWidth As New ArrayList
            Dim arrTextboxId As New ArrayList
            Dim arrColumnsName As New ArrayList

            ViewState.Add("arColWidth", arColWidth)
            ViewState.Add("arrTextboxId", arrTextboxId)
            ViewState.Add("arrColumnsName", arrColumnsName)

        End If
        ViewState("CallPlus") = "0"
        '**********************************
        'paging
        '******************************************
        Try
            mintPageSize = Val(Request.Form("cpnlCallView$txtPageSize"))
        Catch ex As Exception
        End Try
        '******************************
        If IsPostBack = False Then
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
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                End If
                SavePageSize()
            End If
        End If

        Try
            If dtEstCloseDate.Text = "" Then
                dtEstCloseDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
            End If
            If dtStartDate.Text = "" Then
                dtStartDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
            End If
            If IsPostBack = False Then
                txtCSS(Me.Page, "cpnlCallTask")
            End If
            Dim strFilter As String
            Dim shflagSel As Short
            Dim strSearch As String = " "

            mTaskRowValue = 0
            ViewState("gshPageStatus") = 0
            'javascript function added with controls
            '**********************************************************************************
            TxtSubject_F.Attributes.Add("onmousemove", "ShowToolTip(this,1000);")
            TxtSubject_F.Attributes.Add("onkeypress", "ChangeHeight(this.value,this.id)")
            TxtSubject_F.Attributes.Add("onkeyup", "ChangeHeight(this.value,this.id)")

            BtnGrdSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            BtnGrdSearch1.Attributes.Add("Onclick", "return SaveEdit('Search');")
            TxtEstimatedHrs.Attributes.Add("onkeypress", "UsedHour('" & TxtEstimatedHrs.ClientID & "')")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgMonitor.Attributes.Add("Onclick", "return SaveEdit('Monitor');")
            imgMyCall.Attributes.Add("Onclick", "return SaveEdit('MyCall');")
            imgBtnViewPopup.Attributes.Add("Onclick", "return OpenVW('T040011');")


            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
            ddlstview.Attributes.Add("OnChange", "return SaveEdit('View');")
            '*******************************************************************************
            If IsNothing(ViewState("MyCallTask")) = True Then
                ViewState("MyCallTask") = "ALL"
            End If
            If IsPostBack = False Then
                If ViewState("MyCallTask") = "ALL" Then
                    imgMyCall.ToolTip = "Show My Calls"
                    cpnlCallView.Text = "Call View :  All Calls"
                Else
                    imgMyCall.ToolTip = "Show All Calls"
                    cpnlCallView.Text = "Call View :  My Calls"
                End If
            End If
            '**************************************************************************
            If Not IsPostBack Then
                ViewState("CallDetail") = "C"
                imgCloseCall.ToolTip = "View only closed calls"
            End If
            'if call not open 
            '***********************************************************
            Panel1.Visible = True
            GrdAddSerach.Visible = True
            cpnlCallTask.Enabled = True
            cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
            '***********************************************************
            'cpnlError.Visible = False
            txthiddenImage = Request.Form("txthiddenImage")
            '****************************
            'By Harpreet
            'Used to implement search after comming from call detail page using Broswer Back Button.
            'In call Detali page this session is set to V in Not IsPostBack 
            'Solve the Bug under Call#196-Task#29
            If ViewState("CallDetail") = "V" Then
                txthiddenImage = ""
                ViewState("CallDetail") = "C"
            End If
            '****************************
            introwvalues = Request.Form("txtrowvalues")
            strhiddenTable = Request.Form("txthiddenTable")
            If strhiddenTable = "cpnlCallTask_dtgTask" Then
                ViewState("TaskNo") = Val(Request.Form("txthiddenTaskNo"))
            Else
                ' Clear all textBoxes in fastentry if Task no. is changed and currently we have clicked on Task grid
                If Val(Request.Form("txthiddenCallNo")) <> 0 And Val(ViewState("CallNo")) <> Val(Request.Form("txthiddenCallNo")) Then
                    ClearAllTextBox(cpnlCallTask)
                End If
                ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
                mstrCallNumber = ViewState("CallNo")
            End If

            'find the support comp
            mstrsuppcomp = Request.Form("txthiddensuppcomp")
            If IsPostBack Then
                ViewState("SuppComp") = Request.Form("txthiddensuppcomp")
            End If
            'Find the selected call no. company
            If Request.Form("txtComp") <> "undefined" Then
                If Request.Form("txtComp") <> "" Then
                    intComp = Request.Form("txtComp")
                    ViewState("CompanyName") = Request.Form("txtComp")
                    mstGetFunctionValue = WSSSearch.SearchCompName(Request.Form("txtComp"))
                    mstrcomp = intComp
                    ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
                Else
                    mstrcomp = 0
                End If
            Else
            End If
            '--Dependency combo
            If Val(ViewState("CallNo")) > 0 Then
                FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                If Not IsNothing(Request.Form("cpnlCallTask$DDLDependency_F")) Then
                    If DDLDependency_F.Items.Contains(New ListItem(Request.Form("cpnlCallTask$DDLDependency_F"))) Then
                        DDLDependency_F.SelectedValue = Request.Form("cpnlCallTask$DDLDependency_F")
                    End If
                End If
            End If
            If ViewState("CompanyID") <> Nothing Then
                ViewState("CallStatus") = WSSSearch.GetCallStatus(Val(mstrCallNumber), ViewState("CompanyID"))
                If (ViewState("CallStatus") <> Nothing) Then
                    PropCallStatus = ViewState("CallStatus").ToString()
                End If
                If ViewState("CallStatus") = "CLOSED" Then
                    pnlTask.Visible = False
                Else
                    pnlTask.Visible = True
                End If
            End If

            If Val(ViewState("CompanyID")) <> 0 And Val(ViewState("CallNo")) <> 0 Then ' -- Fill Project Session on the basis of company and call
                ViewState("ProjectID") = WSSSearch.SearchProjectID(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))
            End If
            If IsPostBack Then
                'FillFEDropdowns()
                FillRadCombos()
            End If
            'change by 24 march
            If Val(ViewState("CallNo")) < 1 Then
                cpnlCallTask.TitleCSS = "test2"
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            Else
                cpnlCallTask.TitleCSS = "test"
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
            End If

            'If TxtSubject_F.Text.Trim.Equals("") And CDDLTaskOwner_F.CDDLGetValue.Trim.Equals("") And CDDLTaskType_F.CDDLGetValue.Trim.Equals("") Then
            If TxtSubject_F.Text.Trim.Equals("") And CDDLTaskOwner_F.Text.Trim.Equals("") And CDDLTaskType_F.Text.Trim.Equals("") Then
                shflagSel = 1
            End If

            'these statements check the button click caption 
            '***********************************************
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "View"
                            introwvalues = 0
                            'filling session variables on combo change event
                            ViewState("CallViewName") = ddlstview.SelectedItem.Text
                            ViewState("CallViewValue") = ddlstview.SelectedItem.Value
                            If ViewState("Flag") = "1" Then
                                GetView()
                                ViewState("Flag") = 0
                            Else
                                SaveUserView()
                            End If
                            'SaveUserView()
                            ViewState("CallNo") = 0

                        Case "MyCall"
                            If ViewState("MyCallTask") = "ALL" Then
                                ViewState("MyCallTask") = "MY"
                                imgMyCall.ToolTip = "Show All Calls"
                                cpnlCallView.Text = "Call View :  My Calls"
                            Else
                                ViewState("MyCallTask") = "ALL"
                                imgMyCall.ToolTip = "Show My Calls"
                                cpnlCallView.Text = "Call View :  All Calls"
                            End If
                        Case "Logout"
                            LogoutWSS()
                            Exit Sub
                        Case "Edit"
                            If strhiddenTable = "cpnlCallTask_dtgTask" Then
                                Exit Select
                            Else
                                ' Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=1&CallNumber=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "", False)
                            End If
                        Case "Add"
                            ViewState("CallNo") = 0
                            ' Response.Redirect("Call_Detail.aspx?ScrID=3&ID=-1&CallNumber=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "", False)
                        Case "Select"
                            shflagSel = 1
                            cpnlCallTask.Enabled = True
                        Case "CloseCall"
                            If ViewState("CVmshCall") = 0 Then
                                ViewState("CVmshCall") = 1
                            Else
                                ViewState("CVmshCall") = 0
                            End If
                            mstrCallNumber = "0"
                            ViewState("CallNo") = "0"
                        Case "Save"
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                lstError.Items.Add("You don't have access rights to Save record...")
                                Exit Sub
                            End If
                            'End of Security Block
                            If ViewState("CallNo") < 1 Then
                                Exit Select
                            End If
                            ' Save Task Info 
                            Dim intTaskCount As Int16
                            If SaveTask(intTaskCount) = True Then
                                TxtSubject_F.Height = Unit.Pixel(18)
                                lstError.Items.Clear()
                                DisplayMessage("Records Saved successfully...")
                                If intTaskCount < 2 Then
                                    If ddlstview.SelectedItem.Text.ToLower = "default" Then
                                    Else
                                    End If
                                End If
                            Else
                                If Val(Request.Form("txtHIDSize")) > 0 And TxtSubject_F.Text.Length > 0 Then
                                    TxtSubject_F.Height = Unit.Pixel(Val(Request.Form("txtHIDSize")))
                                End If
                            End If
                        Case "Attach"
                            Response.Write("<script>window.open('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CallNo=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "','Attachments','scrollBars=yes,resizable=No,width=800,height=550,status=yes');</script>")
                    End Select
                Catch ex As Exception
                    CreateLog("Call_View", "Load-286", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
                End Try
            End If
            CreateGridTask()
            CreateDataTableTask(strFilter)
            FillHeaderArrayTask()
            'FillFooterArrayTask()
            createTemplateColumnsTask()
            If Not IsPostBack Then
                ViewState("CallNo") = 0
                mstrCallNumber = 0
                cpnlCallTask.Enabled = False
                If ViewState("CVmshCall") = Nothing Then
                    ViewState("CVmshCall") = 0
                End If
                'fill dropdown combo with view name from database
                GetView()
                ChkSelectedView() 'chk user selected view last time
                If ViewState("CallViewName") <> "" And ViewState("CallViewName") <> "Default" Then
                    ' fill datagrid based on user define columns and combination
                    fillview()
                Else
                    'fill tha datagrid from based on admin defined to the role
                    fillDefault()
                    ViewState("CallViewName") = "Default"
                End If
                CurrentPg.Text = _currentPageNumber.ToString()
                'format the grid based on data base info
                'FormatGrid()
                'format the grid based on data base info
                CreateTextBox()
            Else
                If ViewState("CallNo") > 0 Then
                    If shflagSel <> 1 Then
                        If SaveTask() = True Then       ' Save Task Info 
                            TxtSubject_F.Height = Unit.Pixel(18)
                            Call ClearAllTextBox(cpnlCallTask)
                            txthiddenImage = "Select"
                            DisplayMessage("Records Saved successfully...")
                        Else
                            If Val(Request.Form("txtHIDSize")) > 0 And TxtSubject_F.Text.Length > 0 Then
                                TxtSubject_F.Height = Unit.Pixel(Val(Request.Form("txtHIDSize")))
                            End If
                        End If
                    End If
                End If
                arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
                'this loop filling new arraylist in the arrtextvalue array
                For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                    arrtextvalue.Add(Request.Form("cpnlCallView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
                Next
                If ddlstview.SelectedValue = 0 Then
                    'fill tha datagrid from based on admin defined to the role
                    fillDefault()
                Else
                    ' fill datagrid based on user define columns and combination
                    fillview()
                End If
                arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
                'this loop filling new arraylist in the arrtextvalue array
                For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                    arrtextvalue.Add(Request.Form("cpnlCallView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
                Next
                'format the grid based on data base info
                ' FormatGrid()
                'this function create the texboxes on the top of grid
                CreateTextBox()
            End If
            Try
                'recreate Task Query and bind the grid
                Call CreateDataTableTask(strFilter)
                Call BindGridTask()
                '-----------------------------------------
            Catch ex As Exception
                CreateLog("Call_View", "Load-409", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
            If dtEstCloseDate.Text = "" Then
                dtEstCloseDate.Text = SetDateFormat(Today, mdlMain.IsTime.DateOnly)
            End If
            'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
            If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Then
                FillGRDAfterSearch()
            End If
            If IsNothing(ViewState("SortOrder")) = False Then
                SortGRDDuplicate()
            End If
            GridRowSelection()
            If IsNothing(ViewState("SortOrderTask")) = False Then
                SortGRDDuplicateTask()
            End If
            '*****************************************************************************
        Catch ex As Exception
            CreateLog("Call_View", "fill color Load-207", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
        Dim dtrCall As SqlDataReader
        Dim blnStatus As Boolean
        Try
            dtrCall = SQL.Search("CallView", "Load-462", "select * from T040011 where CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & HttpContext.Current.Session("PropCompanyID"), SQL.CommandBehaviour.Default, blnStatus)
            If blnStatus = True Then
                dtrCall.Read()
                'CDDLPriority_F.CDDLSetSelectedItem(IIf(IsDBNull(dtrCall.Item("CM_VC200_Work_Priority")), "", dtrCall.Item("CM_VC200_Work_Priority")))
                CDDLPriority_F.Text = IIf(IsDBNull(dtrCall.Item("CM_VC200_Work_Priority")), "", dtrCall.Item("CM_VC200_Work_Priority"))
            End If
        Catch ex As Exception
            CreateLog("Call_View", "Load-207", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
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
        If Val(ViewState("CallNo")) > 0 Then
            imgEdit.ToolTip = "Edit Call"
            imgMonitor.ToolTip = "Set Call Monitor"
        Else
            imgEdit.ToolTip = "Select a Call to Edit"
            imgMonitor.ToolTip = "Select a Call to set Call Monitor"
        End If
        If ViewState("CVmshCall") = 0 Then
            imgCloseCall.ToolTip = "View only closed calls"
        Else
            imgCloseCall.ToolTip = "View only open calls"
        End If
        'set alternate color setting on grid
        '*************************************
        GrdAddSerach.AlternatingItemStyle.BackColor = Color.FromArgb(245, 245, 245)
        GrdAddSerach.ItemStyle.BackColor = Color.FromArgb(255, 255, 255)
        '*************************************
        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
    End Sub

    '*******************************************************************
    ' Function             :-  fillDefault
    ' Purpose             :- Fill and design datagrid based on defaultcolumns settings from default  tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Sub fillDefault()
        Try

            Dim dsDefault As New DataSet
            GrdAddSerach.PageSize = mintPageSize ' set the grid page size
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select "
            Dim strwhereQuery As String = " and "
            Dim strQuery As String

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
              & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
              & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =463 And " _
              & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
              & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
              & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
              & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & Val(HttpContext.Current.Session("PropRole")) & " AND " _
              & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=463 and obm_vc4_object_type_fk='VIW') " _
              & " order by OBM.OBM_SI2_Order_By"

            sqrdView = SQL.Search("CallView", "Filldefault-502", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)
            Dim shJoin As Short
            CType(ViewState("arrColumnsName"), ArrayList).Clear()
            CType(ViewState("arColWidth"), ArrayList).Clear()

            If blnView = True Then
                Dim rowcount As Int64
                Dim htDateCols As New Hashtable
                While sqrdView.Read
                    If sqrdView.Item("OBM_VC200_URL") = "CM_VC100_By_Whom" Then
                        strSelect &= "ByWhom." & "UM_VC50_UserID" & ","
                        shJoin += 1
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Call_Owner" Then
                        strSelect &= "Owner." & "UM_VC50_UserID" & "," 'requested by
                        shJoin += 2
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Comp_Id_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                        shJoin += 4
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_CustID_FK" Then
                        strSelect &= "Suppcomp." & "CI_VC36_Name" & ","
                        ' shJoin += 4
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Call_No_PK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & "),"
                        'ProjectID
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
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Call_Start_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    Else
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","
                    End If
                    Dim strcolname As String
                    strcolname = sqrdView.Item("ROD_VC50_ALIAS_NAME")
                    If (InStr(sqrdView.Item("ROD_VC50_ALIAS_NAME"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                    Else
                        CType(ViewState("arrColumnsName"), ArrayList).Add(strcolname)
                    End If
                    CType(ViewState("arColWidth"), ArrayList).Add(sqrdView.Item("OBM_VC200_DESCR")) 'adding columns widthe in arraylist
                    rowcount = rowcount + 1
                End While
                sqrdView.Close()
                If rowcount <= 7 Then
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    'DisplayMessage("Call not opened so far...")
                    lstError.Items.Add("You Don't have Access on Default View...")
                    lstError.Items.Add("Please Select your Own View from View Dropdown...")
                    'cpnlError.Visible = True
                    cpnlCallView.Enabled = False
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallView.TitleCSS = "test2"
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    ViewState("CallNo") = 0
                    cpnlCallTask.Enabled = False
                    cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallTask.TitleCSS = "test2"
                    Exit Sub
                End If
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'chk support comapny 
                '*************************************************************************************************
                If shJoin = 4 Then
                    strSelect &= " from T040011 Call,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator  "
                ElseIf shJoin = 5 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 6 Then
                    strSelect &= " from T040011 Call,T060011 Owner,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 7 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord    where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                End If
                '****************************************************************************************************
                If ViewState("CVmshCall") = 1 Then
                    strSelect &= " and cn_VC20_Call_Status='CLOSED' "
                Else
                    strSelect &= " and cn_VC20_Call_Status<>'CLOSED' "
                End If
                'Added company chk from company access table
                strSelect &= " and CM_NU9_Comp_Id_FK in (" & GetCompanySubQuery() & ")  "
                strSelect &= " order by CM_NU9_Call_No_PK desc "
                If ViewState("MyCallTask") = "MY" Then
                    strSelect = strSelect.Insert(strSelect.IndexOf("order by"), " and Call.CM_NU9_Call_Owner=" & Val(Session("PropUserID")) & "  ")
                End If
                mintCompId = ""
                mintCallOwnerID = ""
                mintSuppComp = ""
                mintByWhomID = ""
                mintCallNoRowID = ""
                CoordinatorColumnNo = ""
                RelatedCallColumnNo = ""
                If SQL.Search("T040011", "CallView", "FillDefault", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsDefault.Tables("T040011").Columns.Count - 1
                        dsDefault.Tables("T040011").Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID" Then
                            mintCompId = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqBy".ToUpper Then
                            mintCallOwnerID = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPCOMP" Then
                            mintSuppComp = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "BYWHOM" Then
                            mintByWhomID = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO" Then
                            mintCallNoRowID = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "Coordinator".ToUpper Then
                            CoordinatorColumnNo = inti
                        End If

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "RelatedCall".ToUpper Then
                            RelatedCallColumnNo = inti
                        End If
                        '------------------------------------------------------------
                    Next
                    mdvtable.Table = dsDefault.Tables("T040011")
                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("Subject", 23)
                    htGrdColumns.Add("CallDesc", 44)
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
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
                    If ViewState("CallViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                        ViewState("SortOrder") = Nothing
                    End If
                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If
                    GrdAddSerach.DataBind()
                    cpnlCallView.Enabled = True
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
                    cpnlCallView.State = CustomControls.Web.PanelState.Expanded
                    cpnlCallView.TitleCSS = "test"
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("Call not opened so far or data not exist according to view query ...")
                    cpnlCallView.Enabled = False
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallView.TitleCSS = "test2"
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    cpnlCallTask.Enabled = False
                    cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallTask.TitleCSS = "test2"
                End If
            Else
                GrdAddSerach.Visible = False
                Panel1.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("Sorry! Call Data not available...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlCallView.Enabled = False
                cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallView.TitleCSS = "test2"
                cpnlCallTask.Enabled = False
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallTask.TitleCSS = "test2"
            End If
            'Response.Write("<meta http-equiv=""Content-Type"" content=""text/html""; charset=""iso-8859-1""><meta http-equiv=""Expires"" content=""0""><meta http-equiv=""Cache-Control"" content=""no-cache""><meta http-equiv=""Pragma"" content=""no-cache"">")
        Catch ex As Exception
            CreateLog("Call_View", "Load-676", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

#End Region

#Region "fill View"
    '*******************************************************************
    ' Function             :-  fillview
    ' Purpose              :- Fill and design datagrid based on user defined columns settings from user tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub fillview()
        '**********************
        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim strwhereQuery As String = " and "
        Dim arcolName As New ArrayList
        Dim strOrderQuery As String = " order by "
        Dim strUnsortQuery As String
        Dim dsFromView As New DataSet
        Dim shJoin As Short
        Dim strcolname As String

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        GrdAddSerach.PageSize = mintPageSize ' set the grid page size

        Try

            sqrdView = SQL.Search("CallView", "FillView-719", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='463'  order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then

                CType(ViewState("arrColumnsName"), ArrayList).Clear()
                CType(ViewState("arColWidth"), ArrayList).Clear()
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
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Call_No_PK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & "),"
                        'ProjectID
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Project_ID" Then
                        strSelect &= "Project." & "PR_VC20_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Coordinator" Then
                        strSelect &= "IsNull(Coord." & "UM_VC50_UserID" & ",'') ,"
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_CustID_FK" Then
                        strSelect &= "suppcomp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Close_Date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Request_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Call_Close_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_DT8_Call_Start_Date" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
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
                    End If
                End While

                sqrdView.Close()

                sqrdView = SQL.Search("CallView", "FillView-785", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='463'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
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

                sqrdView = SQL.Search("CallView", "Fillview-809", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='463' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
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
                                    strwhereQuery &= "convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            Case "CM_DT8_CLOSE_DATE" 'call est close date
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "isnull(convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "isnull(convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101),'')" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
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
                            Case "CM_DT8_Call_Start_Date"
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime, convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
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
                    'strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                    strwhereQuery = strwhereQuery.Remove(Len(strwhereQuery) - 4, 4)
                    '*******************************************
                End If

                If shJoin = 4 Then
                    strSelect &= " from T040011 Call,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 5 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom  and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 6 Then
                    strSelect &= " from T040011 Call,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and  Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 7 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator  "
                Else
                End If

                If ViewState("CVmshCall") = 1 Then ' 1 means not display close call else all
                    strSelect &= " and cn_VC20_Call_Status='CLOSED' "
                Else
                    strSelect &= " and cn_VC20_Call_Status<>'CLOSED' "
                End If

                'Added company chk from company access table
                strSelect &= " and CM_NU9_Comp_Id_FK in (" & GetCompanySubQuery() & ") "

                If strwhereQuery.Equals(" and ") = True Then
                    If ViewState("MyCallTask") = "MY" Then
                        strwhereQuery = " and Call.CM_NU9_Call_Owner=" & Val(Session("PropUserID")) & "  "
                        strSelect &= strwhereQuery
                    End If
                Else
                    If ViewState("MyCallTask") = "MY" Then
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

                mintCompId = ""
                mintCallOwnerID = ""
                mintSuppComp = ""
                mintByWhomID = ""
                mintCallNoRowID = ""
                CoordinatorColumnNo = ""
                RelatedCallColumnNo = ""

                If SQL.Search("T040011", "CallView", "FillView-911", strSelect, dsFromView, "Sachin", "Prashar") = True Then
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = CType(ViewState("arrColumnsName"), ArrayList).Item(inti)

                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "COMPID".ToUpper Then
                            mintCompId = inti
                        End If
                        ' If UCase(arColumnName.Item(inti)) = "CALLOWNER".ToUpper Then
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CallReqBy".ToUpper Then
                            mintCallOwnerID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "SUPPCOMP".ToUpper Then
                            mintSuppComp = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "BYWHOM".ToUpper Then
                            mintByWhomID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "CALLNO".ToUpper Then
                            mintCallNoRowID = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "Coordinator".ToUpper Then
                            CoordinatorColumnNo = inti
                        End If
                        If UCase(CType(ViewState("arrColumnsName"), ArrayList).Item(inti)) = "RelatedCall".ToUpper Then
                            RelatedCallColumnNo = inti
                        End If

                    Next

                    rowvalue = 0
                    rowvalueCall = 0

                    mdvtable.Table = dsFromView.Tables(0)

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("Subject", 23)
                    htGrdColumns.Add("CallDesc", 44)
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.AllowPaging = True
                    GrdAddSerach.PageSize = mintPageSize

                    '*************************************************************************
                    If ViewState("CallViewName") <> ddlstview.SelectedItem.Text Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                        ViewState("SortOrder") = Nothing
                    End If

                    If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) >= mdvtable.Table.Rows.Count Then
                        GrdAddSerach.CurrentPageIndex = 0
                        CurrentPg.Text = 1
                    End If

                    GrdAddSerach.DataBind()
                    cpnlCallView.Enabled = True

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
                    cpnlCallView.State = CustomControls.Web.PanelState.Expanded
                    cpnlCallView.TitleCSS = "test"
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("Call not opened so far or data not exist according to view query ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    cpnlCallView.Enabled = False
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallView.TitleCSS = "test2"
                End If
            Else
                GrdAddSerach.Visible = False
                Panel1.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("Sorry! Call Data not available...")

                cpnlCallView.Enabled = False
                cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallView.TitleCSS = "test2"
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

        Catch ex As Exception
            CreateLog("Call_View", "FillView-945", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
            sqrdView = SQL.Search("CallView", "GetView-1047", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='463' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
            If blnView = True Then
                ddlstview.DataSource = sqrdView
                ddlstview.DataTextField = "UV_VC50_View_Name"
                ddlstview.DataValueField = "UV_IN4_View_ID"
                ddlstview.DataBind()
                sqrdView.Close()
            End If

            If ViewState("CallViewName") = "" Or ViewState("CallViewName") = "Default" Then
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
                ddlstview.SelectedIndex = ddlstview.Items.Count - 1
            Else
                ddlstview.Items.Add("Default")
                ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
            End If

            If ViewState("CallViewName") <> "" And ViewState("CallViewName") <> "Default" Then
                ddlstview.SelectedValue = ViewState("CallViewValue")
            End If

        Catch ex As Exception
            CreateLog("Call_View", "GetView-1050", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
                    'setting textbox width 
                    If intii > 13 And intii < 18 Then
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 18 Then
                        col1cng = col1.Value - 2.8
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.6
                        col1cng = col1cng & "pt"
                    End If
                    'hidding textbox over comment and attachment
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & CType(ViewState("arrColumnsName"), ArrayList).Item(intii) & " runat=""server""  Width=""0"" Visible=""False"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & CType(ViewState("arrColumnsName"), ArrayList).Item(intii) & " runat=""server""  Width=" & col1cng & "  CssClass=SearchTxtBox MaxLength=""20""></asp:TextBox>"))
                    End If
                    _textbox.ID = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    _textbox.Text = ""
                Else
                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intii))
                    'setting text boxes width
                    If intii > 13 And intii < 18 Then
                        col1cng = col1.Value
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 18 Then
                        col1cng = col1.Value - 2.8
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.6
                        col1cng = col1cng & "pt"
                    End If
                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If
                    strcolid = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    'hidding textbox over comment and attachment
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=""0"" CssClass=SearchTxtBox Visible=""False""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""20""></asp:TextBox>"))
                    End If
                    _textbox.ID = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                End If
                CType(ViewState("arrTextboxId"), ArrayList).Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("Call_View", "CreateTextBox-1135", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
            For intI = 0 To CType(ViewState("arrColumnsName"), ArrayList).Count - 1
                If CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intI) = "A" Then
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
                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    Bound_Column.ItemStyle.Wrap = True
                    GrdAddSerach.Columns.Add(Bound_Column)
                End If
            Next
        Catch ex As Exception
            CreateLog("Call_View", "FormatGrid-1384", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
#End Region

#Region "Serach Grid Button Click"
    '*******************************************************************
    ' Function             :-  FillGRDAfterSearch
    ' Purpose              :- grid search based on textbox data function filter the data from dataview
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

            For intI = 0 To arCount
                If Not IsNothing(arrtextvalue(intI)) Then
                    If Not arrtextvalue(intI).Equals("") Then
                        strSearch = arrtextvalue(intI)
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") Then
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                                If IsDate(strSearch) = False Then
                                    'fill own date if some body fill wrong data in date filled 
                                    strSearch = "12/12/1825"
                                End If
                            End If
                            If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Double") = True Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Then
                                strSearch = strSearch.Replace("*", "")
                                If IsNumeric(strSearch) = False Then
                                    'fill own data if somebody fill wrong data in numaric field
                                    strSearch = "-101"
                                End If
                            End If
                            strSearch = strSearch.Replace("*", "")
                            strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            strSearch = arrtextvalue(intI)
                            strSearch = GetSearchString(strSearch)
                            If strSearch.Contains("*") = False Then
                                strSearch &= "*"
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

            If (strRowFilterString Is Nothing) Then
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                '**********************************************
                SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                GrdAddSerach.DataSource = mdvtable
                rowvalue = 0
                rowvalueCall = 0
                If ViewState("CallViewName") <> ddlstview.SelectedItem.Text Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                If (mintPageSize) * (GrdAddSerach.CurrentPageIndex - 1) >= mdvtable.Table.Rows.Count Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                GrdAddSerach.DataBind()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            HTMLEncodeDecode(mdlMain.Action.Decode, mdvtable)
            GetFilteredDataView(mdvtable, strRowFilterString)

            HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)
            SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            GrdAddSerach.DataSource = mdvtable

            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            '**********************************************
            rowvalue = 0
            rowvalueCall = 0

            If ViewState("CallViewName") <> ddlstview.SelectedItem.Text Then
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
            If mdvtable.Count = 0 Then
                lstError.Items.Clear()
                DisplayMessage("Data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Else
            End If
        Catch ex As Exception
            CreateLog("Call_View", "Click-1767", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "btngrdsearch", )
        End Try
    End Sub
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
        Dim CallNo As String 'call no 
        Dim strCompId As String 'compamy's ID
        Dim strtsuppcomp As String

        Dim attSts As Boolean
        Dim intCount As Integer = 7
        Dim rowFlag As Boolean = True
        Dim intcolno As Int16 = 0
        Dim comstat As String
        Dim intcolnoComm As Integer = 5

        'These variables stored columns position in datagrid
        '*************************************
        Dim CallOwnerID As String
        Dim CallOwnerName As String
        Dim ByWhomName As String
        Dim ByWhomID As String
        Dim strtsuppcompName As String
        Dim strtsuppcompID As String
        Dim strCallNoRowID As String
        '*************************************
        'This is return true or false for monitoring 
        Dim Monistat As Boolean

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    'Variables store the position of the columns, +2 means we added two columns manually in datagrid that's why we adding +2
                    '**********************************************
                    CallNo = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strCallNoRowID = (mintCallNoRowID + 2)
                    strCompId = e.Item.Cells(mintCompId + 2).Text()
                    strtsuppcomp = e.Item.Cells(mintSuppComp + 2).Text()
                    'it is display toltip on datagrid columns

                    e.Item.ToolTip = " Call # " & CallNo & " - Company: " & e.Item.Cells(mintCompId + 2).Text.Trim
                    If mintCallOwnerID <> "" Then
                        CallOwnerName = e.Item.Cells(mintCallOwnerID + 2).Text()
                        CallOwnerID = (mintCallOwnerID + 2)
                    End If
                    If mintByWhomID <> "" Then
                        ByWhomName = e.Item.Cells(mintByWhomID + 2).Text()
                        ByWhomID = (mintByWhomID + 2)
                    End If
                    If mintSuppComp <> "" Then
                        strtsuppcompName = e.Item.Cells(mintSuppComp + 2).Text()
                        strtsuppcompID = (mintSuppComp + 2)
                    End If

                    Dim CoodinatorName As String
                    Dim CoordinatorId As String
                    If CoordinatorColumnNo <> "" Then
                        CoodinatorName = e.Item.Cells(CoordinatorColumnNo + 2).Text()
                        CoordinatorId = (CoordinatorColumnNo + 2)
                    End If

                    Dim RelatedCallNo As String
                    Dim RelatedCallNoId As String
                    If RelatedCallColumnNo <> "" Then
                        RelatedCallNo = e.Item.Cells(RelatedCallColumnNo + 2).Text()
                        RelatedCallNoId = RelatedCallColumnNo + 2
                    End If

                    '********************************************************************************************
                    'for attachment image
                    '********************************************************************************************
                    If rowFlag Then
                        attSts = IIf(e.Item.Cells(mdvtable.Table.Columns.Count + 1).Text = "&nbsp;", False, True)
                    End If
                    If Not IsNothing(e.Item.Cells(0).FindControl("imgAtt")) Then
                        If attSts Then
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Attach15_9.gif"
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & CallNo & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach', '" & intCount & "','" & strCompId & "','" & CallNo & "')")
                        Else
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/white.gif"
                        End If
                    End If
                    ' **************************************************************************************************
                    ' for show comment image 
                    ' *************************************************************************************************
                    'for comment images********************
                    If rowFlag Then
                        comstat = e.Item.Cells(mdvtable.Table.Columns.Count).Text
                        'comstat = GComm(strName, strID, strCompId)
                        If Not IsNothing(e.Item.Cells(0).FindControl("imgComm")) Then
                            Select Case comstat
                                Case "1"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment2.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & CallNo & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & CallNo & "')")
                                Case "2"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Unread.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & CallNo & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & CallNo & "')")
                                Case "0"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & CallNo & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & CallNo & "')")
                                Case Else
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & CallNo & "', '" & rowvalue + 1 & "', 'cpnlCallView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & CallNo & "')")
                            End Select
                        End If
                    End If
                    '  *************************************************************************************************
                    'these line of code added click or double click functionality on grid after two columns
                    '***************************************************************************
                    If intcolno >= 2 Then
                        If intcolno = CallOwnerID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & CallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & CallOwnerName & "')")
                        ElseIf intcolno = ByWhomID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & CallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strtsuppcompName & "','" & ByWhomName & "')")
                        ElseIf intcolno = CoordinatorId Then
                            If e.Item.Cells(intcolno).Text.Trim <> "&nbsp;" Then
                                e.Item.Cells(intcolno).ForeColor = Color.Blue
                                e.Item.Cells(intcolno).CssClass = "celltext"
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & CallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & CoodinatorName & "')")
                            End If
                        ElseIf intcolno = RelatedCallNoId Then
                            If Val(RelatedCallNo) > 0 Then
                                e.Item.Cells(intcolno).ForeColor = Color.Blue
                                e.Item.Cells(intcolno).CssClass = "celltext"
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & RelatedCallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                            End If
                        ElseIf intcolno = strCallNoRowID Then
                            'this function return true or false for call in monitoring or not
                            Monistat = GetMonStat(CallNo, strCompId)
                            'if call under monitoring then this code change the color in data grid for call number
                            '******************************
                            If Monistat = True Then
                                e.Item.Cells(intcolno).ForeColor = Color.Red
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck('0'," & CallNo & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & strtsuppcomp & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & CallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                            Else
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck('0'," & CallNo & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & strtsuppcomp & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & CallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                            End If
                            '**************************************************
                        Else
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck('0'," & CallNo & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & strtsuppcomp & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & CallNo & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                        End If
                    Else
                        e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                    End If
                End If
                rowFlag = False
                intcolno = intcolno + 1
            Next
            If Val(ViewState("CallNo")) <> 0 And Val(ViewState("CompanyID")) <> 0 Then
                If CallNo = Val(ViewState("CallNo")) And ViewState("CompanyName") = strCompId Then
                    e.Item.BackColor = Color.FromArgb(212, 212, 212)
                End If
            End If
            rowvalue += 1
            rowvalueCall += 1
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If GetCallPriority(Val(CallNo), strCompId) = True Then
                    For intI As Integer = 0 To e.Item.Cells.Count - 1
                        e.Item.Cells(intI).ForeColor = Color.Red
                    Next
                End If
            End If
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim CallStatus = e.Item.Cells(6).Text
                If CallStatus = "COMPLETE" Then
                    For intI As Integer = 0 To e.Item.Cells.Count - 1
                        e.Item.Cells(intI).ForeColor = Color.Green
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("Call_View", "ItemDataBound-1715", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach", )
        End Try
    End Sub

#End Region
    Function GetMonStat(ByVal callNo As String, ByVal compID As String) As String
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "Setup_Rules"
        SQL.DBTracing = False
        Dim intRows As Integer
        Try
            SQL.Search("Call_View", "GetMonStat-2409", "select table_id from Setup_Rules where Call_No=" & callNo & " and Task_No=0 and Company_id in(select CI_NU8_Address_Number from t010011 where CI_VC36_Name='" & compID & "' and CI_VC8_Address_Book_Type='COM')", intRows)
            If intRows > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Call_View", "GetMonStat-2420", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
            CreateLog("Call-View", "CreateGridTask-2274", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub FormatGridTask()
        Try
            dtgTask.AutoGenerateColumns = False
            dtgTask.AllowPaging = True
            'dtgTask.ShowFooter = True
            dtgTask.ShowHeader = True
            dtgTask.HeaderStyle.CssClass = "GridHeader"
            dtgTask.HeaderStyle.Height = System.Web.UI.WebControls.Unit.Pixel(1)
            dtgTask.Width = System.Web.UI.WebControls.Unit.Percentage(100)
            dtgTask.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
            dtgTask.BorderStyle = BorderStyle.None
            dtgTask.CellPadding = 1
            dtgTask.AllowPaging = False
            dtgTask.CssClass = "Grid"
            dtgTask.HorizontalAlign = HorizontalAlign.Center
            'dtgTask.FooterStyle.CssClass = "GridFixedFooter"
            dtgTask.SelectedItemStyle.CssClass = "GridSelectedItem"
            dtgTask.AlternatingItemStyle.CssClass = "GridAlternateItem"
            dtgTask.ItemStyle.CssClass = "GridItem"
        Catch ex As Exception
            CreateLog("Call-View", "FormatGridTask-2298", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
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
            arrWidthTask.Add(60)
            'arrWidthTask.Add(40)
            'arrWidthTask.Add(49)


            dtgTask.Width = Unit.Pixel(920)

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
            For intCount = 3 To dtvTask.Table.Columns.Count - 2
                tclTask(intCount + 1) = New TemplateColumn
                tclTask(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvTask.Table.Columns(intCount).ToString, dtvTask.Table.Columns(intCount).ToString)
                Dim AddEventOnGrigHeader As New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(intCount).ToString, "", dtvTask.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameTask(intCount).ToString, True, maxchar(intCount))
                AddHandler AddEventOnGrigHeader.OnSort, AddressOf SOrtGrid
                tclTask(intCount + 1).HeaderTemplate = AddEventOnGrigHeader
                tclTask(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvTask.Table.Columns(intCount).ToString + "_F", False)
                tclTask(intCount + 1).ItemStyle.Width = arrColumnsWidthTask(intCount)    'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthTask(intCount))
                dtgTask.Columns.Add(tclTask(intCount + 1))
            Next

            Call ChangeTextBoxWidth()
        Catch ex As Exception
            CreateLog("Call-View", "CreateTemplateColumnTask-2408", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

#End Region

#Region "Create Task Query"
    Private Sub CreateDataTableTask(ByVal strWhereClause As String)

        Dim dsTask As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32

        Try
            strSql = " select TM_CH1_Comment as Blank1, TM_CH1_Attachment as Blank2,TM_CH1_Forms as Blank3,TM_NU9_Task_Order, TM_NU9_Task_no_PK, TM_VC50_Deve_Status,TM_VC1000_Subtsk_Desc,  TM_VC8_task_type,b.UM_VC50_UserID, TM_NU9_Dependency,convert(varchar,TM_DT8_Task_Date) TM_DT8_Task_Date,convert(varchar,TM_DT8_Est_close_date) TM_DT8_Est_close_date, TM_FL8_Est_Hr,TM_CH1_Mandatory,  TM_VC8_Priority, a.TM_VC8_Supp_Owner  From T040021 a, T060011 b Where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK= a.TM_VC8_Supp_Owner "
            strSql = strSql & " Order By TM_NU9_Task_Order asc"

            Call SQL.Search("T040021", "Call_Detail", "CreateDataTableTask-1803", strSql, dsTask, "sachin", "Prashar")
            '-- Preparing Search  String (strFilter) for Task Grid
            Dim strSearchTask As String = ""
            Dim strFilterTask As String = ""
            For intCount = 3 To dsTask.Tables(0).Columns.Count - 1
                strSearchTask = Request.Form("cpnlCallTask$dtgTask$ctl01$" + dsTask.Tables(0).Columns(intCount).ColumnName + "_H")
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
            Dim htDateCols1 As New Hashtable

            htDateCols.Add("TM_DT8_Est_close_date", 2)
            If dtvTask.Table.Rows.Count > 0 Then
                Dim dtTemp As New DataTable
                dtTemp = dtvTask.Table
                SetDataTableDateFormat(dtTemp, htDateCols)
                dtvTask = New DataView
                dtvTask = dtTemp.DefaultView
            End If
            htDateCols1.Add("TM_DT8_Task_Date", 2)
            If dtvTask.Table.Rows.Count > 0 Then
                Dim dtTemp As New DataTable
                dtTemp = dtvTask.Table
                SetDataTableDateFormat(dtTemp, htDateCols1)
                dtvTask = New DataView
                dtvTask = dtTemp.DefaultView
            End If
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
        Catch ex As Exception
            CreateLog("Call-View", "CreateDataTableTask-1750", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "Fill Task Header Array"
    Private Sub FillHeaderArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        arrHeadersTask.Clear()
        If Page.IsPostBack Then
            For intCount = 0 To dtvTask.Table.Columns.Count - 1
                arrHeadersTask.Add(Request.Form("cpnlCallTask$dtgTask$ctl01$" + dtvTask.Table.Columns(intCount).ColumnName + "_H"))
            Next
        End If
    End Sub
#End Region

#Region "Fill Task Footer Array"
    Private Sub FillFooterArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        arrFooterTask.Clear()
        If Page.IsPostBack Then
            For intCount = 0 To dtvTask.Table.Columns.Count - 1
                intFooterIndex = dtvTask.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                arrFooterTask.Add(Request.Form("cpnlCallTask$dtgTask$ct0l" & intFooterIndex.ToString.Trim & ":" + dtvTask.Table.Columns(intCount).ColumnName + "_F"))
            Next
        End If
    End Sub
#End Region

#Region "Bind Task Grid"
    Private Sub BindGridTask()
        Try
            Dim htGrdColumns As New Hashtable
            htGrdColumns.Add("TM_VC1000_Subtsk_Desc", 23)
            HTMLEncodeDecode(mdlMain.Action.Encode, dtvTask, htGrdColumns)
            SetCommentFlag(dtvTask, mdlMain.CommentLevel.TaskLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            dtgTask.DataSource = dtvTask
            If Request.Form("txtrowvaluescall") <> 0 Then
                introwvalues = Request.Form("txtrowvaluescall")
            End If
            dtgTask.DataBind()
        Catch ex As Exception
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
        dg.PageSize = 1

        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dtgTask.DataKeys(0) <> 0 Then
                    For intCount = 0 To 2       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        TaskNo = dtgTask.DataKeys(e.Item.ItemIndex)
                        If strSelected = "1" Then      'If comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "','" & ViewState("CompanyID") & "','" & ViewState("CallNo") & "')")

                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "','" & ViewState("CompanyID") & "','" & ViewState("CallNo") & "')")
                        Else       ' If no comment/attachment is attached
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "','" & ViewState("CompanyID") & "','" & ViewState("CallNo") & "')")
                        End If
                    Next

                    For intCount = 3 To dtvTask.Table.Columns.Count - 2       'for Others
                        If dtvTask.Table.Columns(intCount).DataType.FullName.Equals("System.DateTime") Then
                            If dtBound.Rows(cnt)(intCount).ToString Is null Or dtBound.Rows(cnt)(intCount).ToString = "" Then
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
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString)
                            e.Item.Cells(intCount).ToolTip = HTMLEncodeDecode(mdlMain.Action.Decode, IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString))
                        End If
                        TaskNo = dtgTask.DataKeys(e.Item.ItemIndex)
                        Dim CallNo As Integer = ViewState("CallNo")
                        Dim CompID As Integer = ViewState("CompanyID")

                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheck('" & TaskNo & "','" & CallNo & "', '" & mTaskRowValue + 1 & "','" & introwvalues & "', 'cpnlCallTask_dtgTask','" & CompID & "')")
                        If intCount = 8 Then ' for task owner
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & ViewState("CallNo") & ", '" & rowvalue & "','cpnlCallTask_dtgTask','" & ViewState("SuppComp") & "','" & HTMLEncodeDecode(mdlMain.Action.Decode, e.Item.Cells(intCount).ToolTip) & "'," & dtvTask.Table.Rows(e.Item.ItemIndex).Item("TM_VC8_Supp_Owner") & ")")
                        Else '  not    for taskl owner
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheckTaskEdit('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'cpnlCallTask_dtgTask')")
                        End If
                    Next
                    mTaskRowValue += 1
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
            CreateLog("Call-View", "ItmDataBound-1860", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try
    End Sub

#Region "Set Entry TextBox Width"
    Private Sub ChangeTextBoxWidth()

    End Sub

#End Region

#Region "Save Task Fast Entry"
    Private Function SaveTask(Optional ByRef Count As Int16 = 0) As Boolean

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short
        Dim intCallNo As Integer
        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            lstError.Items.Clear()
            'cpnlError.Text = "Message..."
            lstError.Items.Add("Your Role does not have rights to save Tasks...")
            shFlag = 0
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Function
        End If
        'End of Security Block
        If TxtSubject_F.Text.Trim.Equals("") Then     'Exit If all textbox are blank
            SaveTask = False
            Exit Function
        End If
        '-----------------------------------------------
        'date 7/12/2006
        'to compare session values to stop f5 duplicate data while pressing f5 in data entry
        If ViewState("update").ToString() = ViewState("update").ToString() Then '------------------------------
            'Check call and task status
            '********************************************************************
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strchkcallstatus As String = SQL.Search("CallView", "SaveTask-2070", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CN_VC20_Call_Status='CLOSED'")
            If IsNothing(strchkcallstatus) = False Then
                lstError.Items.Clear()
                lstError.Items.Add("Call Closed so You cannot Add the Task...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Return False
                Exit Function
            End If
            '**************************************************************************
            lstError.Items.Clear()
            Dim dtCallDate As Date = SQL.Search("CallView", "SaveTask-2126", "Select CM_DT8_Request_Date from T040011 where CM_NU9_Call_No_PK=" & ViewState("CallNo") & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "")
            If CDDLTaskOwner_F.Text.Trim.Equals("") Then
                lstError.Items.Add("Task Owner cannot be blank...")
                shFlag = 1
            End If
            If CDDLPriority_F.Text.Trim.Equals("") Then
                lstError.Items.Add("Task Priority  cannot be blank...")
                shFlag = 1
            End If
            If CDDLTaskType_F.Text.Trim.Equals("") Then
                lstError.Items.Add("Task type  cannot be blank...")
                shFlag = 1
            End If
            If Convert.ToDateTime(dtStartDate.Text) < dtCallDate.ToString("yyyy-MMM-dd") Then
                lstError.Items.Add("Task start date should be greater than Call Start Date")
                shFlag = 1
            End If
            If dtEstCloseDate.Text.Trim <> "" Then
                If IsDate(dtEstCloseDate.Text) = False Then
                    lstError.Items.Add("Check date format of estimated close date...")
                    shFlag = 1
                    'ElseIf CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < dtCallDate Or CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                    '    lstError.Items.Add("Estimated Close date cannot be less than Call date or current date...")
                    '    shFlag = 1
                    'End If

                    '''''''''''''''modified by tarun on Jan18 2010 so as to save tasks before current date also but greater than Call date
                ElseIf IsDate(dtStartDate.Text) = False Then
                    lstError.Items.Add("Check date format of start date...")
                    shFlag = 1
                ElseIf CDate(dtEstCloseDate.Text.Trim) < dtCallDate Then 'Or CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                    lstError.Items.Add("Estimated Close date cannot be less than Call date...")
                    shFlag = 1
                ElseIf CDate(dtEstCloseDate.Text.Trim) < CDate(dtStartDate.Text.Trim) Then 'Or CDate(dtEstCloseDate.Text.Trim & " " & Now.ToLongTimeString) < Now.ToLongDateString Then
                    lstError.Items.Add("Estimated Close date cannot be less than Start date...")
                    shFlag = 1
                End If
            Else
                lstError.Items.Add("Please enter date for Task...")
                shFlag = 1
            End If

            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                shFlag = 0
                Return False
                Exit Function
            End If
            lstError.Items.Clear()
            If CDDLTaskType_F.Text.Trim = "" Then
                lstError.Items.Add("Task Type Mismatch...")
                shFlag = 1
            End If
            If CDDLPriority_F.Text.Trim = "" Then
                lstError.Items.Add("Task Priority Mismatch...")
                shFlag = 1
            End If
            Dim intAddressNo As Integer
            intAddressNo = SQL.Search("CallView", "SaveTask-2195", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & CDDLTaskOwner_F.SelectedValue.Trim & "")
            If intAddressNo <= 0 Then
                lstError.Items.Add("Task Owner mismatch...")
                shFlag = 1
            End If
            If CheckRealValuesForDDLS() = False Then
                shFlag = 1
            End If

            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                shFlag = 0
                Exit Function
            End If
            mstGetFunctionValue = CheckUserValiditity(CDDLTaskOwner_F.SelectedValue)
            If mstGetFunctionValue.FunctionExecuted = False Then
                lstError.Items.Clear()
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            End If
            Dim strCallStatus_D As String
            strCallStatus_D = SQL.Search("CallView", "SaveTask-2212", "select CM_VC8_Call_Type from T040011 where CM_NU9_Call_No_PK=" & ViewState("CallNo") & "")
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
                arrColumns.Add("TM_VC8_Call_Type")
                arrColumns.Add("TM_CH1_Mandatory")
                arrColumns.Add("TM_NU9_Dependency")
                arrColumns.Add("TM_FL8_Est_Hr")

                If Not dtEstCloseDate.Text.Trim.Equals("") Then
                    arrColumns.Add("TM_DT8_Est_close_date")
                End If
                arrColumns.Add("TM_CH1_Forms")
                arrColumns.Add("TM_NU9_Task_no_PK")

                If TxtEstimatedHrs.Text.Trim.Equals("") Then
                    arrRows.Add(0)
                Else
                    arrRows.Add(TxtEstimatedHrs.Text.Trim)
                End If

                'arrRows.Add(Now)
                arrRows.Add(dtStartDate.Text.Trim)
                'If Not dtStartDate.Text.Trim.Equals("") Then
                '    arrRows.Add(dtStartDate.Text.Trim)
                'End If
                arrRows.Add(ViewState("CallNo"))
                arrRows.Add(ViewState("CompanyID"))
                arrRows.Add("ASSIGNED")
                arrRows.Add(TxtSubject_F.Text.Trim)
                arrRows.Add(CDDLTaskType_F.Text.Trim.ToUpper)

                Dim intProjectID As Integer = WSSSearch.SearchProjectID(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))
                arrRows.Add(intProjectID)
                arrRows.Add(CDDLTaskOwner_F.SelectedValue)
                arrRows.Add(HttpContext.Current.Session("PropUserID"))
                arrRows.Add(CDDLPriority_F.Text.Trim.ToUpper)
                arrRows.Add(strCallStatus_D)
                If chkMandatory.Checked = True Then
                    arrRows.Add("M")
                Else
                    arrRows.Add("O")
                End If
                arrRows.Add(IIf(DDLDependency_F.SelectedValue = "", DBNull.Value, DDLDependency_F.SelectedValue))
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

                intCallNo = SQL.Search("CallView", "SaveTask-2264", "select isnull(max(TM_NU9_Task_no_PK),0) from T040021 where TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and  TM_NU9_Call_No_FK=" & ViewState("CallNo").ToString)
                intCallNo += 1
                'How much task is there in this call
                Count = intCallNo
                arrRows.Add(intCallNo.ToString)
                mstGetFunctionValue = WSSSave.SaveTask(arrColumns, arrRows, ViewState("CompanyID"), ViewState("CallNo"))
                ' If SQL.Save("Call_View", "SaveTask-2804", arrColumns, arrRows) = True Then
                If mstGetFunctionValue.ErrorCode = 0 Then
                    mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("CallNo"), True, ViewState("CompanyID"))
                    If mstGetFunctionValue.ErrorCode = 0 Then
                        lstError.Items.Add("Task Data Saved Successfully...")
                        '___________________________
                        'update session after saving data related to f5 problem
                        ViewState("update") = Server.UrlEncode(System.DateTime.Now.ToString())
                        '_____________________________________________
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        ViewState("TaskNo") = intCallNo
                        '--Dependency combo
                        '*****************************************************
                        FillNonUDCDropDown(DDLDependency_F, "select TM_NU9_Task_No_Pk, TM_NU9_Task_No_Pk from T040021 where TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and tm_nu9_comp_id_fk =" & ViewState("CompanyID") & " and tm_vc50_deve_status<>'CLOSED'", True)
                        '*****************************************************
                        If GetFiles() = True Then
                        Else
                        End If
                        ClearAllTextBox(cpnlCallTask)
                        garFileID.Clear()
                        Return True
                    ElseIf mstGetFunctionValue.ErrorCode = 1 Or mstGetFunctionValue.ErrorCode = 2 Then
                        lstError.ForeColor = Color.Red
                        lstError.Items.Add("Server is busy please try later...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    End If
                Else
                    lstError.ForeColor = Color.Red
                    lstError.Items.Add("Server is busy please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            Catch ex As Exception
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("Call_View", "SaveTask-1370", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        Else
            ClearAllTextBox(cpnlCallTask)
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
            sqrdTempFiles = SQL.Search("CallView", "GetFiles-2325", "select * from T040041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(Session("PropCompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)
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
        Catch ex As Exception
            CreateLog("Call_View", "GetFiles-2276", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")
        Dim objFolder As IO.DirectoryInfo = New IO.DirectoryInfo(strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\"
                If IO.File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then
                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If
                    Dim strFileLocation As String = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    'Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim
                    'Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    'Move the file to that folder
                    IO.File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
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
                strFilePath = strPath & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBConnection = strSQL
                SQL.DBTracing = False
                dblVersionNo = SQL.Search("CallView", "CreateFolder-2403", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")
                'Check if its a new upload or a new version o f an existing attachment.
                If IO.File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If
                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If
                Dim strFileLocation As String = strPath.Trim & "\" & HttpContext.Current.Session("PropCompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & HttpContext.Current.Session("PropCompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim
                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                IO.File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
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
            CreateLog("Call_View", "CreateFolder-2391", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        lstError.Items.Clear()
        lstError.Items.Add(ErrMsg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        lstError.Items.Clear()
        lstError.Items.Add(Msg)
        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
    End Sub
#End Region

#Region "Clear TextBoxes based on panels"
    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        Try
            TxtSubject_F.Text = ""
            dtEstCloseDate.Text = ""
            TxtEstimatedHrs.Text = ""
            DDLDependency_F.SelectedValue = ""
            CDDLTaskType_F.Text = ""
            CDDLTaskOwner_F.Text = ""
        Catch ex As Exception
            CreateLog("Call-View", "ClearAllTextBoxes-3144", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "Refresh Grid With no selection"
    Private Sub RefreshSelection()

    End Sub
#End Region

    Private Sub FillFastTextBox(ByVal CallNo As String)
        Dim dtrCallno As SqlDataReader
        Dim blnCall As Boolean
        Try
            dtrCallno = SQL.Search("CallView", "FillFastTextBox-2738", "Select * from T040011 Where CM_NU9_Call_No_PK= " + CallNo, SQL.CommandBehaviour.SingleRow, blnCall)
            dtrCallno.Read()
            If blnCall = True Then
                'CDDLPriority_F.CDDLSetSelectedItem(dtrCallno.Item("CM_VC200_Work_Priority"))
                CDDLPriority_F.Text = dtrCallno.Item("CM_VC200_Work_Priority")
            End If
        Catch ex As Exception
            CreateLog("Call_View", "FillFastTextBox-2745", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'date 7/12/2006 
        '-----------------------------------------------
        'to store current viewstate value in session to stop f5 duplicate data while pressing f5 in data entry
        ViewState("update") = ViewState("update")
        '-----------------------------------------------
    End Sub

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click

        GrdAddSerach.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        If ddlstview.SelectedValue = 0 Then
            fillDefault()
        Else
            fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Then
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
            fillDefault()
        Else
            fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Then
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
            fillDefault()
        Else
            fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Then
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
            fillDefault()
        Else
            fillview()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
        GridRowSelection()
    End Sub

    Private Sub SaveUserView()
        Dim intid = 463 ' screen id for call view screen
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
        sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=463 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)
        If blnReturn = False Then
            ViewState("CallViewName") = "Default"
            ViewState("CallViewValue") = "0"
            Exit Sub
        Else
            While sqdrCol.Read
                ViewState("CallViewName") = sqdrCol.Item("UV_VC50_View_Name")
                ViewState("CallViewValue") = sqdrCol.Item("UV_IN4_View_ID")
                ddlstview.SelectedValue = ViewState("CallViewValue")
            End While
            sqdrCol.Close()
        End If
    End Sub
    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            Dim intA As Integer = 0
            For intI = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1 + 2
                If intI > 1 Then
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intA) = "A" Then
                        If e.Item.Cells.Count > 2 Then
                            e.Item.Cells(intA + 2).Width = System.Web.UI.WebControls.Unit.Parse("0px")
                            e.Item.Cells(intA + 2).Visible = False
                        End If
                    Else
                        If e.Item.Cells.Count > 2 Then
                            e.Item.Cells(intA + 2).Width = System.Web.UI.WebControls.Unit.Parse(CType(ViewState("arColWidth"), ArrayList).Item(intA) & "pt")
                        End If
                    End If
                    intA += 1
                ElseIf intI = 0 Then
                    e.Item.Cells(0).Width = System.Web.UI.WebControls.Unit.Parse("20px")
                ElseIf intI = 1 Then
                    If e.Item.Cells.Count > 1 Then
                        e.Item.Cells(1).Width = System.Web.UI.WebControls.Unit.Parse("17px")
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
        GridRowSelection()
    End Sub
    Private Sub SortGRDDuplicate()
        Try
            ' If SortWay Mod 2 = 0 Then
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
            GridRowSelection()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GridRowSelection()
        'Restore the grid selection on click of grid's row when page post back
        '*****************************************************************************
        Dim dgi As DataGridItem
        If mintCompId <> "" Or mintCallNoRowID <> "" Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(mintCompId + 2).Text.Trim = ViewState("CompanyName") And Val(dgi.Cells(mintCallNoRowID + 2).Text.Trim) = Val(ViewState("CallNo")) Then
                    cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                    cpnlCallTask.Enabled = True
                    cpnlCallTask.TitleCSS = "test"
                    cpnlCallTask.Text = "Task View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & "  Company:  " & ViewState("CompanyName") & ")"
                    txtcallNo.Value = ViewState("CallNo")
                    Exit For
                Else
                    cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallTask.Enabled = False
                    cpnlCallTask.TitleCSS = "test2"
                    cpnlCallTask.Text = "Task View"
                End If
            Next
        Else
            cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            cpnlCallTask.Enabled = False
            cpnlCallTask.TitleCSS = "test2"
            cpnlCallTask.Text = "Task View"
        End If
        If GrdAddSerach.Items.Count = 0 Then
            cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            cpnlCallTask.Enabled = False
            cpnlCallTask.TitleCSS = "test2"
            cpnlCallTask.Text = "Task View"
            CurrentPg.Text = 0
        End If
    End Sub

    Private Sub SavePageSize()
        Dim intid = 463
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
    Private Function ChkPageView() As Boolean
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=463 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

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
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
    Protected Sub SOrtGrid(ByVal sender As Object, ByVal e As CommandEventArgs)
        ViewState("SortOrderTask") = e.CommandArgument
        SortGRDTask()
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
    End Sub
    Private Sub SortGRDDuplicateTask()
        Try
            If Val(ViewState("SortWayTask")) Mod 2 = 0 Then
                dtvTask.Sort = ViewState("SortOrderTask") & " ASCC"
            Else
                dtvTask.Sort = ViewState("SortOrderTask") & " DES"
            End If
            mTaskRowValue = 0
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
        Catch ex As Exception
        End Try
    End Sub

#Region " Fills Rad combo and checks validates them during save"
    ''' <summary>
    ''' Fills the rad combo boxes (Task Type, priority, Task Owner) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillRadCombos()
        Dim dsComboTaskType As New DataSet
        Dim dsComboTaskOwner As New DataSet
        Dim dsComboPriority As New DataSet

        Try
            'Task Type combo
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

            ' Task Owner Combo
            dsComboTaskOwner = objCommonFunctionsBLL.FillRadTaskOwner(Val(ViewState("CompanyID")), Val(ViewState("ProjectID")))
            CDDLTaskOwner_F.Items.Clear()
            CDDLTaskOwner_F.DataSource = dsComboTaskOwner
            For Each data As DataRow In dsComboTaskOwner.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("Name"))
                item.Value = CStr(data("ID"))
                CDDLTaskOwner_F.Items.Add(item)
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
            CreateLog("WSS", "Call_View-FillRadCombos-3142", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    ''' <summary>
    ''' Validated values in the combo (Priority and Task Type)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckRealValuesForDDLS() As Boolean
        Try
            Dim bytError As Byte = 0
            Dim strCheckDropDownValue As String = ""
            'Check for task type
            strCheckDropDownValue = SQL.Search("Call_View", "CheckRealValuesForDDLS-3157", "Select Name from UDC where name='" & CDDLTaskType_F.Text.Trim & "'")
            If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                lstError.Items.Add("The Task-type you have entered is not Valid.Please select it from Dropdown")
                bytError = 1
            Else
                bytError = 0
            End If

            'Check for Priority
            strCheckDropDownValue = SQL.Search("Call_View", "CheckRealValuesForDDLS-3157", "Select Name from UDC where name='" & CDDLPriority_F.Text.Trim & "'")
            If String.IsNullOrEmpty(strCheckDropDownValue) = True Then
                lstError.Items.Add("The priority you have entered is not Valid.Please select it from Dropdown")
                bytError += 1
            Else
                bytError += 0
            End If

            If bytError > 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
        End Try
    End Function
#End Region


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
End Class
'Private Function FillFEDropdowns() As Boolean
'    If Val(ViewState("CompanyID")) = 0 Then
'        Exit Function
'    End If
'    ' -- Task Type
'    CDDLTaskType_F.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TKTY""" & _
'    " and UDC.Company=" & ViewState("CompanyID") & "  union " & _
'    " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""TKTY""" & _
'    " and UDC.Company=0 Order By Name"
'    CDDLTaskType_F.CDDLUDC = True
'    CDDLTaskType_F.CDDLFillDropDown(10)
'    '------------------------------------------
'    '--Task Owner
'    CDDLTaskOwner_F.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(ViewState("ProjectID")) & " and PM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA' ) Order By Name"
'    CDDLTaskOwner_F.CDDLUDC = False
'    CDDLTaskOwner_F.CDDLFillDropDown(10, False)
'    '------------------------------------------
'    CDDLPriority_F.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""PRIO""" & _
'    " and UDC.Company=" & ViewState("CompanyID") & "  union " & _
'    " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
'    " and UDC.Company=0 Order By Name"
'    CDDLPriority_F.CDDLUDC = True
'    CDDLPriority_F.CDDLFillDropDown(10)
'    CDDLTaskType_F.CDDLType = CustomDDL.DDLType.FastEntry
'    CDDLTaskOwner_F.CDDLType = CustomDDL.DDLType.FastEntry

'    CDDLPriority_F.CDDLType = CustomDDL.DDLType.FastEntry

'End Function