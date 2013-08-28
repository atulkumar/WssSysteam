Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

Partial Class SupportCenter_CallView_CallView_Simple
    Inherits System.Web.UI.Page
    '*******************************************************************
    ' Function             :-  global declaration
    ' Purpose              :- variable declartion
    ' Date					  Author					        	Modification Date					Description
    ' 2/5/06			      Sachin Prashar/Ranvijay/Harpreet   -------------------					Created

    '*******************************************************************
#Region "global level declaration"

    Private mdvtable As New DataView  ' store data from table for view grid 
    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 
    ' Private arColumnName As New ArrayList ' this is stored grid's columns name to assined value to the texboxes
    Public introwvalues As Integer 'stored the selected row's value
    Private intColumnCount As Integer  'grid columns count
    'Shared mshCall As Short 'store info when click on closed call button
    ' Private Shared intCol As Integer 'grid columns count
    'these variable store the position of the columns
    '****************************************
    Private compColumnNo As String
    Private suppCompColumnNo As String
    Private callOwnerColumnNo As String
    Private byWhomColumnNo As String
    Private callNoColumnNo As String
    Private coordinatorColumnNo As String
    Private relatedCallColumnNo As String
    '************************************
    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer
    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************
    Private arrtextvalue As ArrayList = New ArrayList  'textboxes values
    '*************************************************************
    Private txthiddenImage As String 'stored clicked button's cation  
    Public mstrsuppcomp As String 'stores the support comp during postbacks
    Private mintFileID As Integer
    Private mTaskRowValue As Integer
    Private intComp As String
    Public mstrcomp As String
    ' Private marTextbox() As TextBox
    'Dim mblnValue As Boolean
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Public mstrCallNumber As String
    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments
    Private null As System.DBNull
    Public intHIDAttach As Integer

#End Region
    '*******************************************************************
    ' Function             :-  page_load
    ' Purpose              :- fill data in grd and load data and design datagrid and chk security at page load time 
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin Prashar/Ranvijay/Harpreet		    -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            If obj.ScreenAccess(845) = False Then
                Response.Redirect("../../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, 845)
        End If
        'End of Security Block
        '*****************************************


        'Put user code to initialize the page here
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        '***********************************
        'to store value in session to stop f5 duplicate data while pressing f5 in data entry
        If Not Page.IsPostBack Then

            ViewState("update") = Server.UrlEncode(System.DateTime.Now.ToString())
            ViewState("SortOrder") = Nothing
            ViewState("SortWay") = 0

            Dim arColWidth As New ArrayList
            Dim arrTextboxId As New ArrayList
            Dim arrColumnsName As New ArrayList

            ViewState.Add("arColWidth", arColWidth)
            ViewState.Add("arrTextboxId", arrTextboxId)
            ViewState.Add("arrColumnsName", arrColumnsName)


        End If
        '**********************************
        '''''paging
        '******************************************
        Try
            mintPageSize = Val(Request.Form("cpnlCallView$txtPageSize"))
        Catch ex As Exception
        End Try
        'If mintPageSize = 0 Or mintPageSize < 0 Then
        '    mintPageSize = 20
        'End If
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
                    'viewstate("PageSize") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                End If

                SavePageSize()
            End If
        End If
        txtPageSize.Text = mintPageSize
        '******************************

        Try
            'TextBox2.Width = System.Web.UI.WebControls.Unit.Pixel(32)
            cpnlCallView.Enabled = False

            txtCSS(Me.Page, "cpnlCallTask")

            Dim strFilter As String
            Dim shflagSel As Short
            Dim strSearch As String = " "


            arrImageUrlEnabled.Clear()
            arrImageUrlEnabled.Add("../../Images/comment2.gif")
            arrImageUrlEnabled.Add("../../Images/attach15_9.gif")

            arrImageUrlDisabled.Clear()
            arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
            arrImageUrlDisabled.Add("../../Images/white.gif")

            arrImageUrlNew.Clear()
            arrImageUrlNew.Add("../../Images/comment_Unread.gif")


            mTaskRowValue = 0
            ViewState("gshPageStatus") = 0

            'javascript function added with controls
            '**********************************************************************************
            'Modified by Atul
            'The next line is commented because in this page no need for this function
            'BtnGrdSearch.Attributes.Add("onclick", "return CheckLength();")

            'A Search function is called so that page will not postback fully
            BtnGrdSearch.Attributes.Add("onclick", "return SaveEdit('Search');")
            imgTask.Attributes.Add("Onclick", "return SaveEdit('Tasks');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgAttachments.Attributes.Add("Onclick", "return SaveEdit('Attach');")
            imgMonitor.Attributes.Add("Onclick", "return SaveEdit('Monitor');")
            imgMyCall.Attributes.Add("Onclick", "return SaveEdit('MyCall');")
            imgBtnViewPopup.Attributes.Add("Onclick", "return OpenVW('T040011');")
            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 

            ddlstview.Attributes.Add("OnChange", "return SaveEdit('View');")

            '*******************************************************************************
            If IsNothing(ViewState("MyCall")) = True Then
                ViewState("MyCall") = "ALL"
            End If
            If IsPostBack = False Then
                If ViewState("MyCall") = "ALL" Then
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

            ' ddlstview.Enabled = True
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
                End If
                ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
                mstrCallNumber = ViewState("CallNo")
            End If

            'find the support comp
            mstrsuppcomp = Request.Form("txthiddensuppcomp")
            If IsPostBack Then
                ' session("SuppComp") = Request.Form("txthiddensuppcomp")
                ViewState("SuppComp") = Request.Form("txthiddensuppcomp")
            End If

            'Find the selected call no. company
            If Request.Form("txtComp") <> "undefined" Then
                If Request.Form("txtComp") <> "" Then
                    intComp = Request.Form("txtComp")
                    ViewState("CompName") = Request.Form("txtComp")
                    mstGetFunctionValue = WSSSearch.SearchCompName(Request.Form("txtComp"))
                    mstrcomp = intComp
                    ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
                Else
                    mstrcomp = 0
                End If
            Else
            End If

            If ViewState("CompanyID") <> Nothing Then
                ViewState("CallStatus") = WSSSearch.GetCallStatus(Val(mstrCallNumber), ViewState("CompanyID"))
            End If

            If Val(ViewState("CompanyID")) <> 0 And Val(ViewState("CallNo")) <> 0 Then ' -- Fill Project Session on the basis of company and call
                ViewState("CallStatus") = WSSSearch.SearchProjectID(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))
            End If

            'change by 24 march

            'these statements check the button click caption 
            '***********************************************
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage

                        Case "View"

                            introwvalues = 0
                            'filling session variables on combo change event
                            ViewState("CallViewsimpleName") = ddlstview.SelectedItem.Text
                            ViewState("CallViewSimpleValue") = ddlstview.SelectedItem.Value
                            If ViewState("Flag") = "1" Then
                                GetView()
                                ViewState("Flag") = 0
                            Else
                                SaveUserView()

                            End If
                            ViewState("CallNo") = 0

                        Case "MyCall"
                            If ViewState("MyCall") = "ALL" Then
                                ViewState("MyCall") = "MY"
                                imgMyCall.ToolTip = "Show All Calls"
                                cpnlCallView.Text = "Call View :  My Calls"
                            Else
                                ViewState("MyCall") = "ALL"
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
                                '  Response.Redirect("Call_Detail.aspx?ScrID=3&ID=0&PageID=5&CallNumber=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "", False)
                            End If

                        Case "Add"
                            ViewState("CallNo") = 0
                            'Response.Redirect("Call_Detail.aspx?ScrID=3&ID=-1&CallNumber=" & ViewState("CallNo") & "&CompID=" & ViewState("CompanyID") & "", False)

                        Case "Select"
                            shflagSel = 1
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
                            Response.Write("<script>window.open('../../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL&CompID=" & ViewState("CompanyID") & "&CallNo=" & ViewState("CallNo") & ",'Attachments','scrollBars=yes,resizable=No,width=800,height=550,status=yes');</script>")
                    End Select

                Catch ex As Exception
                    CreateLog("Call_View", "Load-286", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
                End Try
            End If
            '*********************************************************************

            strFilter = " "
            strSearch = " "

            If Not IsPostBack Then
                ViewState("CallNo") = 0
                mstrCallNumber = 0

                If ViewState("CVSmshCall") = Nothing Then
                    ViewState("CVSmshCall") = 0
                End If
                'fill dropdown combo with view name from database
                GetView()
                ChkSelectedView() 'chk user selected view last time
                If ViewState("CallViewsimpleName") <> "" And ViewState("CallViewsimpleName") <> "Default" Then
                    ' fill datagrid based on user define columns and combination
                    fillview()
                Else
                    'fill tha datagrid from based on admin defined to the role
                    fillDefault()
                    ViewState("CallViewsimpleName") = "Default"
                End If
                CurrentPg.Text = _currentPageNumber.ToString()
                CreateTextBox()
            Else


                'this loop filling new arraylist in the arrtextvalue array
                arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
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
                'this function create the texboxes on the top of grid

                'this loop filling new arraylist in the arrtextvalue array
                arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
                For i As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
                    arrtextvalue.Add(Request.Form("cpnlCallView$" & CType(ViewState("arrColumnsName"), ArrayList).Item(i)))
                Next

                CreateTextBox()
            End If
            'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
            If ChechkValidityforSearch(arrtextvalue) = True Or CHKA.Checked = True Or CHKC.Checked = True Then
                FillGRDAfterSearch()
            End If

            If IsNothing(ViewState("SortOrder")) = False Then
                SortGRDDuplicate()
            End If

            'Restore the grid selection on click of grid's row when page post back
            '*****************************************************************************
            Dim dgi As DataGridItem
            If compColumnNo <> "" Or callNoColumnNo <> "" Then
                For Each dgi In GrdAddSerach.Items
                    If dgi.Cells(compColumnNo + 2).Text.Trim = ViewState("CompName") And Val(dgi.Cells(callNoColumnNo + 2).Text.Trim) = Val(ViewState("CallNo")) Then

                        Exit For
                    Else

                    End If
                Next
            Else

            End If

            If GrdAddSerach.Items.Count = 0 Then
                CurrentPg.Text = 0
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
            End If
        Catch ex As Exception
            CreateLog("Call_View", "Load-207", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

        If ViewState("CVSmshCall") = 0 Then
            imgCloseCall.ToolTip = "View only closed calls"
        Else
            imgCloseCall.ToolTip = "View only open calls"
        End If

        If Val(ViewState("CallNo")) > 0 Then
            imgEdit.ToolTip = "Edit Call"
            imgMonitor.ToolTip = "Set Call Monitor"
            imgTask.ToolTip = "View Task"
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

            imgEdit.ToolTip = "Select a Call to Edit"
            imgMonitor.ToolTip = "Select a Call to set Call Monitor"
            imgTask.ToolTip = "Select a Call to View Task"
        End If

        'set alternate color setting on grid
        '*************************************
        GrdAddSerach.AlternatingItemStyle.BackColor = Color.FromArgb(245, 245, 245)
        GrdAddSerach.ItemStyle.BackColor = Color.FromArgb(255, 255, 255)
        '*************************************
        'cpnlCallView.Enabled = False

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
            Dim strQuery As String = String.Empty
            Dim shJoin As Short

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
              & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
              & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =799 And " _
              & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
              & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
              & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
              & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
              & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & Val(HttpContext.Current.Session("PropRole")) & " AND " _
              & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=799 and obm_vc4_object_type_fk='VIW') " _
              & " order by OBM.OBM_SI2_Order_By"

            '  SQL.DBTable = "T070042"
            sqrdView = SQL.Search("CallView", "Filldefault-502", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)

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
                        strSelect &= "Owner." & "UM_VC50_UserID" & ","
                        shJoin += 2
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Comp_Id_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                        shJoin += 4
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_CustID_FK" Then
                        strSelect &= "Suppcomp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Call_No_PK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & "),"
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Project_ID" Then
                        strSelect &= "Project." & "PR_VC20_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_NU9_Coordinator" Then
                        strSelect &= "Coord." & "UM_VC50_UserID" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Close_Date" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("OBM_VC200_URL") & ",101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CM_DT8_Request_Date" Then
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
                    lstError.Items.Add("You Dont have Access on Default View...")
                    lstError.Items.Add("Please Select your Own View from View Dropdown...")
                    cpnlCallView.Enabled = False
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallView.TitleCSS = "test2"
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    ViewState("CallNo") = 0
                    Exit Sub
                End If
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'chk support comapny 
                '*************************************************************************************************

                If shJoin = 4 Then
                    strSelect &= " from T040011 Call,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 5 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 6 Then
                    strSelect &= " from T040011 Call,T060011 Owner,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator  "
                ElseIf shJoin = 7 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                End If

                '****************************************************************************************************
                If ViewState("CVSmshCall") = 1 Then
                    strSelect &= " and cn_VC20_Call_Status='CLOSED' "
                Else
                    strSelect &= " and cn_VC20_Call_Status<>'CLOSED' "
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
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke

                    rowvalue = 0
                    rowvalueCall = 0

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.AllowPaging = True
                    GrdAddSerach.PageSize = mintPageSize

                    If ViewState("CallViewsimpleName") <> ddlstview.SelectedItem.Text Then
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
                    cpnlCallView.Enabled = True

                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("Call not opened so far or data not exist according to view query ...")
                    cpnlCallView.Enabled = False
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                    cpnlCallView.TitleCSS = "test2"
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
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
    '
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
            sqrdView = SQL.Search("CallView", "FillView-719", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='799'  order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CM_NU9_Call_No_PK" Then
                        strSelect &= "convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & "),"
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

                sqrdView = SQL.Search("CallView", "FillView-785", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='799'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
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
                sqrdView = SQL.Search("CallView", "Fillview-809", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='799' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

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
                            Case "CM_DT8_Call_Start_Date".ToUpper
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "('" & strSplit.Replace(",", "','") & "')"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
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
                End If

                If shJoin = 4 Then
                    strSelect &= " from T040011 Call,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 5 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T010011 comp,T010011 suppcomp,T210011 Project,T060011 Coord   where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom  and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator  "
                ElseIf shJoin = 6 Then
                    strSelect &= " from T040011 Call,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and  Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and  call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                ElseIf shJoin = 7 Then
                    strSelect &= " from T040011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project,T060011 Coord  where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator "
                End If

                If ViewState("CVSmshCall") = 1 Then ' 1 means not display close call else all
                    strSelect &= " and cn_VC20_Call_Status='CLOSED' "
                Else
                    strSelect &= " and cn_VC20_Call_Status<>'CLOSED' "
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

                    rowvalue = 0
                    rowvalueCall = 0

                    mdvtable.Table = dsFromView.Tables(0)

                    Dim htGrdColumns As New Hashtable
                    htGrdColumns.Add("Subject", 23)
                    htGrdColumns.Add("CallDesc", 44)

                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdColumns)
                    SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, Session("PropCompanyID"), ViewState("CallNo"), 0, 0)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If
                    GrdAddSerach.AllowPaging = True
                    GrdAddSerach.PageSize = mintPageSize

                    '*************************************************************************
                    If ViewState("CallViewsimpleName") <> ddlstview.SelectedItem.Text Then
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
                    cpnlCallView.Enabled = True
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
                '*********************
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
            sqrdView = SQL.Search("CallView", "GetView-1047", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='799' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
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
                    If intii > 13 And intii < 18 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 18 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    End If

                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Then
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
                    If intii > 13 And intii < 18 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 18 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    End If
                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If
                    strcolid = CType(ViewState("arrColumnsName"), ArrayList).Item(intii)
                    If CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "C" Or CType(ViewState("arrColumnsName"), ArrayList).Item(intii) = "A" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=""0"" Visible=""False"" CssClass=SearchTxtBox></asp:TextBox>"))
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

            ' For intI As Integer = 0 To CType(ViewState("arColWidth"), ArrayList).Count - 1
            For intI = 0 To arCount
                If Not IsNothing(arrtextvalue(intI)) Then
                    If Not arrtextvalue(intI).Equals("") Then
                        strSearch = arrtextvalue(intI)
                        'delibrately put the " * " afetr the text of the search
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

            If (strRowFilterString Is Nothing) Then
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, Session("PropCompanyID"), ViewState("CallNo"), 0, 0)
                GrdAddSerach.DataSource = mdvtable
                rowvalue = 0
                rowvalueCall = 0
                If ViewState("CallViewsimpleName") <> ddlstview.SelectedItem.Text Then
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
            SetCommentFlag(mdvtable, mdlMain.CommentLevel.CallLevel, Session("PropCompanyID"), ViewState("CallNo"), 0, 0)
            GrdAddSerach.DataSource = mdvtable
            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            '**********************************************
            rowvalue = 0
            rowvalueCall = 0

            If ViewState("CallViewsimpleName") <> ddlstview.SelectedItem.Text Then
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
            '''
            If mdvtable.Count = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If

            '***********************************************************************
        Catch ex As Exception
            CreateLog("Call_View", "Click-1575", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "btngrdsearch", )
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
        Dim strID As String 'call no 
        Dim strCompId As String 'compamy's ID
        Dim strtsuppcomp As String

        Dim attSts As Boolean
        Dim intCount As Integer = 7
        Dim rowFlag As Boolean = True
        Dim intcolno As Int16 = 0
        Dim comstat As String
        Dim intcolnoComm As Integer = 5

        'these variables stored columns position in datagrid
        '*************************************
        Dim CallOwnerID As String
        Dim CallOwnerName As String
        Dim ByWhomName As String
        Dim ByWhomID As String
        Dim strtsuppcompName As String
        Dim strtsuppcompID As String
        Dim strCallNoRowID As String
        '*************************************

        'this is return true or false for monitoring 
        Dim Monistat As Boolean

        ' GrdAddSerach.Columns.Clear()

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    'Variables store the position of the columns, +2 means we added two columns manually in datagrid that's why we adding +2
                    '**********************************************
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strCallNoRowID = (callNoColumnNo + 2)
                    strCompId = e.Item.Cells(compColumnNo + 2).Text()
                    strtsuppcomp = e.Item.Cells(suppCompColumnNo + 2).Text()
                    'it is display toltip on datagrid columns
                    e.Item.ToolTip = " Call # " & strID & " - Company: " & e.Item.Cells(compColumnNo + 2).Text.Trim
                    If callOwnerColumnNo <> "" Then
                        CallOwnerName = e.Item.Cells(callOwnerColumnNo + 2).Text()
                        CallOwnerID = (callOwnerColumnNo + 2)
                    End If
                    If byWhomColumnNo <> "" Then
                        ByWhomName = e.Item.Cells(byWhomColumnNo + 2).Text()
                        ByWhomID = (byWhomColumnNo + 2)
                    End If
                    If suppCompColumnNo <> "" Then
                        strtsuppcompName = e.Item.Cells(suppCompColumnNo + 2).Text()
                        strtsuppcompID = (suppCompColumnNo + 2)
                    End If

                    Dim CoodinatorName As String
                    Dim CoordinatorId As String
                    If coordinatorColumnNo <> "" Then
                        CoodinatorName = e.Item.Cells(coordinatorColumnNo + 2).Text()
                        CoordinatorId = (coordinatorColumnNo + 2)
                    End If

                    Dim RelatedCallNo As String
                    Dim RelatedCallNoId As String
                    If relatedCallColumnNo <> "" Then
                        RelatedCallNo = e.Item.Cells(relatedCallColumnNo + 2).Text()
                        RelatedCallNoId = (relatedCallColumnNo + 2)
                    End If

                    '*************************************************************************************************
                    'for attachment image
                    '*************************************************************************************************
                    If rowFlag Then
                        attSts = IIf(e.Item.Cells(mdvtable.Table.Columns.Count + 1).Text = "&nbsp;", False, True)
                    End If
                    If Not IsNothing(e.Item.Cells(0).FindControl("imgAtt")) Then
                        If attSts Then
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/Attach15_9.gif"
                            CType(e.Item.Cells(1).FindControl("imgAtt"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach', '" & intCount & "','" & strCompId & "','" & strID & "')")
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
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & strID & "')")
                                    e.Item.Cells(0).Attributes.Add("style", "cursor:hand")
                                Case "2"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Unread.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & strID & "')")
                                    e.Item.Cells(0).Attributes.Add("style", "cursor:hand")
                                Case "0"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "','cpnlCallView_GrdAddSerach', '" & intcolnoComm & "','" & strCompId & "','" & strID & "')")
                                    e.Item.Cells(0).Attributes.Add("style", "cursor:hand")
                                Case Else
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Blank.gif"
                                    CType(e.Item.Cells(0).FindControl("imgComm"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & rowvalue + 1 & "', 'cpnlCallView_GrdAddSerach','" & intcolnoComm & "','" & strCompId & "','" & strID & "')")
                                    e.Item.Cells(0).Attributes.Add("style", "cursor:hand")
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
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & strID & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & CallOwnerName & "')")
                        ElseIf intcolno = ByWhomID Then
                            e.Item.Cells(intcolno).ForeColor = Color.Blue
                            e.Item.Cells(intcolno).CssClass = "celltext"
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & strID & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strtsuppcompName & "','" & ByWhomName & "')")

                        ElseIf intcolno = CoordinatorId Then
                            If e.Item.Cells(intcolno).Text.Trim <> "&nbsp;" Then
                                e.Item.Cells(intcolno).ForeColor = Color.Blue
                                e.Item.Cells(intcolno).CssClass = "celltext"
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & strID & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & CoodinatorName & "')")
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
                            Monistat = GetMonStat(strID, strCompId)
                            'if call under monitoring then this code change the color in data grid for call number
                            '******************************

                            If Monistat = True Then
                                e.Item.Cells(intcolno).ForeColor = Color.Red
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & strtsuppcomp & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                            Else
                                e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                                e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & strtsuppcomp & "')")
                                e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                            End If
                            '*************************
                        Else
                            e.Item.Cells(intcolno).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & rowvalueCall & "','cpnlCallView_GrdAddSerach','" & strCompId & "','" & strtsuppcomp & "')")
                            e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "','cpnlCallView_GrdAddSerach','" & strCompId & "')")
                        End If
                    End If
                    '****************************************************
                End If
                rowFlag = False
                intcolno = intcolno + 1
            Next

            If Val(ViewState("CallNo")) <> 0 And Val(ViewState("CompanyID")) <> 0 Then
                If strID = Val(ViewState("CallNo")) And ViewState("CompName") = strCompId Then
                    e.Item.BackColor = Color.FromArgb(212, 212, 212)
                End If
            End If

            rowvalue += 1
            rowvalueCall += 1

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If GetCallPriority(Val(strID), strCompId) = True Then
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

    ''Private Sub CreateGridTask()
    ''    Dim lc1 As New LiteralControl
    ''    Dim lc2 As New LiteralControl
    ''    Try
    ''        dtgTask.ID = "dtgTask"
    ''        dtgTask.DataKeyField = "TM_NU9_Task_no_PK"
    ''        Call FormatGridTask()


    ''    Catch ex As Exception
    ''        CreateLog("Call-View", "CreateGridTask-2274", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
    ''    End Try
    ''End Sub

    ''Private Sub FormatGridTask()
    ''    Try
    ''        dtgTask.AutoGenerateColumns = False
    ''        dtgTask.AllowPaging = True
    ''        '  dtgTask.ShowFooter = True
    ''        dtgTask.ShowHeader = True
    ''        dtgTask.HeaderStyle.CssClass = "GridHeader"
    ''        dtgTask.HeaderStyle.Height = System.Web.UI.WebControls.Unit.Pixel(1)
    ''        dtgTask.Width = System.Web.UI.WebControls.Unit.Percentage(100)
    ''        dtgTask.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
    ''        dtgTask.BorderStyle = BorderStyle.None
    ''        dtgTask.CellPadding = 1
    ''        dtgTask.AllowPaging = False
    ''        dtgTask.CssClass = "Grid"
    ''        dtgTask.HorizontalAlign = HorizontalAlign.Center
    ''        'dtgTask.FooterStyle.CssClass = "GridFixedFooter"
    ''        dtgTask.SelectedItemStyle.CssClass = "GridSelectedItem"
    ''        dtgTask.AlternatingItemStyle.CssClass = "GridAlternateItem"
    ''        dtgTask.ItemStyle.CssClass = "GridItem"
    ''    Catch ex As Exception
    ''        CreateLog("Call-View", "FormatGridTask-2298", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
    ''    End Try
    ''End Sub
#End Region

#Region "Create Template Column Task Grid"

    ''Private Sub createTemplateColumnsTask()
    ''    Dim intCount As Integer
    ''    Try
    ''        ReDim tclTask(dtvTask.Table.Columns.Count)

    ''        arrImageUrlEnabled.Clear()
    ''        arrImageUrlEnabled.Add("../../Images/comment2.gif")
    ''        arrImageUrlEnabled.Add("../../Images/attach15_9.gif")
    ''        arrImageUrlEnabled.Add("../../Images/Form1.jpg")

    ''        arrImageUrlDisabled.Clear()
    ''        arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
    ''        arrImageUrlDisabled.Add("../../Images/white.gif")
    ''        arrImageUrlDisabled.Add("../../Images/white.gif")

    ''        arrImageUrlNew.Clear()
    ''        arrImageUrlNew.Add("../../Images/comment_Unread.gif")
    ''        arrImageUrlNew.Add("../../Images/white.gif")
    ''        arrImageUrlNew.Add("../../Images/Form2.gif")
    ''        arrColumnsNameTask.Clear()
    ''        arrColumnsNameTask.Add("C")
    ''        arrColumnsNameTask.Add("A")
    ''        arrColumnsNameTask.Add("F")
    ''        arrColumnsNameTask.Add("ID")
    ''        arrColumnsNameTask.Add("Stat")
    ''        arrColumnsNameTask.Add("Subject")
    ''        arrColumnsNameTask.Add("TType")
    ''        arrColumnsNameTask.Add("Ownr")
    ''        arrColumnsNameTask.Add("Dep")
    ''        arrColumnsNameTask.Add("EstClDate")
    ''        arrColumnsNameTask.Add("EHr")
    ''        arrColumnsNameTask.Add("Act")

    ''        arrColumnsNameTask.Add("Prio")
    ''        'arrColumnsNameTask.Add("Proj")
    ''        'arrColumnsNameTask.Add("Agmt")

    ''        arrWidthTask.Clear()
    ''        arrWidthTask.Add(17)
    ''        arrWidthTask.Add(17)
    ''        arrWidthTask.Add(17)
    ''        arrWidthTask.Add(17)
    ''        arrWidthTask.Add(70)
    ''        arrWidthTask.Add(216)
    ''        arrWidthTask.Add(64)
    ''        arrWidthTask.Add(72)
    ''        arrWidthTask.Add(40)
    ''        arrWidthTask.Add(88)
    ''        arrWidthTask.Add(33)
    ''        arrWidthTask.Add(24)
    ''        arrWidthTask.Add(57)
    ''        ' arrWidthTask.Add(40)
    ''        'arrWidthTask.Add(49)


    ''        dtgTask.Width = Unit.Pixel(818)
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(0)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(1)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(2)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(3)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(4)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(5)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(6)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(7)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(8)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(9)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(10)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(11)))
    ''        arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(12)))



    ''        tclTask(0) = New TemplateColumn
    ''        tclTask(0).Visible = True
    ''        tclTask(0).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvTask.Table.Columns(0).ToString + "_H", False, arrColumnsNameTask(0), False)
    ''        tclTask(0).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(0).ToString, arrImageUrlDisabled(0))
    ''        tclTask(0).ItemStyle.HorizontalAlign = HorizontalAlign.Center
    ''        tclTask(0).ItemStyle.Width = arrColumnsWidthTask(0)
    ''        dtgTask.Columns.Add(tclTask(0))

    ''        tclTask(1) = New TemplateColumn
    ''        tclTask(1).Visible = True
    ''        tclTask(1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvTask.Table.Columns(1).ToString + "_H", False, arrColumnsNameTask(1), False)
    ''        tclTask(1).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(1).ToString, arrImageUrlDisabled(1))
    ''        tclTask(1).ItemStyle.HorizontalAlign = HorizontalAlign.Center
    ''        tclTask(1).ItemStyle.Width = arrColumnsWidthTask(1)
    ''        dtgTask.Columns.Add(tclTask(1))

    ''        tclTask(2) = New TemplateColumn
    ''        tclTask(2).Visible = True
    ''        tclTask(2).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvTask.Table.Columns(2).ToString + "_H", False, arrColumnsNameTask(2), False)
    ''        tclTask(2).ItemTemplate = New IONGrid.CreateItemTemplateImage(dtvTask.Table.Columns(2).ToString, arrImageUrlDisabled(1))
    ''        tclTask(2).ItemStyle.HorizontalAlign = HorizontalAlign.Center
    ''        tclTask(2).ItemStyle.Width = arrColumnsWidthTask(2)
    ''        dtgTask.Columns.Add(tclTask(2))

    ''        Dim maxchar() As Int16 = {-1, -1, -1, 3, 7, 20, 7, 8, 3, 10, 4, 1, 5, 2, 5, 5} 'Variable to store MaxLength of TextBoxes
    ''        For intCount = 3 To dtvTask.Table.Columns.Count - 2
    ''            tclTask(intCount + 1) = New TemplateColumn
    ''            tclTask(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvTask.Table.Columns(intCount).ToString, dtvTask.Table.Columns(intCount).ToString)
    ''            tclTask(intCount + 1).HeaderTemplate = New IONGrid.CreateItemTemplateTextBoxForHeader("", dtvTask.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameTask(intCount), True, maxchar(intCount))
    ''            tclTask(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvTask.Table.Columns(intCount).ToString + "_F", False)
    ''            tclTask(intCount + 1).ItemStyle.Width = arrColumnsWidthTask(intCount)    'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthTask(intCount))
    ''            dtgTask.Columns.Add(tclTask(intCount + 1))
    ''        Next

    ''        ' Call ChangeTextBoxWidth()
    ''    Catch ex As Exception
    ''        CreateLog("Call-View", "CreateTemplateColumnTask-2408", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
    ''    End Try
    ''End Sub

#End Region

#Region "Create Task Query"
    ''Private Sub CreateDataTableTask(ByVal strWhereClause As String)
    ''    Dim dsTask As New DataSet
    ''    Dim strSql As String
    ''    Dim rowTemp As System.Data.DataRow
    ''    Dim intCount As Int32
    ''    Try
    ''        If IsNothing(strWhereClause) = True OrElse strWhereClause.Trim.Equals("") Then
    ''            ',TM_VC8_Project,TM_NU9_Agmt_No
    ''            strSql = " select TM_CH1_Comment as Blank1, TM_CH1_Attachment as Blank2,TM_CH1_Forms as Blank3, TM_NU9_Task_no_PK, TM_VC50_Deve_Status,TM_VC1000_Subtsk_Desc, " & _
    ''             " TM_VC8_task_type,b.UM_VC50_UserID, TM_NU9_Dependency,TM_DT8_Est_close_date, TM_FL8_Est_Hr,TM_CH1_Mandatory, " & _
    ''             " TM_VC8_Priority, a.TM_VC8_Supp_Owner " & _
    ''             " From T040021 a, T060011 b Where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK= a.TM_VC8_Supp_Owner "
    ''        Else
    ''            ',TM_VC8_Project,TM_NU9_Agmt_No
    ''            strSql = " select TM_CH1_Comment as Blank1, TM_CH1_Attachment as Blank2,TM_CH1_Forms as Blank3, TM_NU9_Task_no_PK, TM_VC50_Deve_Status,TM_VC1000_Subtsk_Desc, " & _
    ''            " TM_VC8_task_type,b.UM_VC50_UserID, TM_NU9_Dependency,TM_DT8_Est_close_date, TM_FL8_Est_Hr,TM_CH1_Mandatory, " & _
    ''            " TM_VC8_Priority, a.TM_VC8_Supp_Owner " & _
    ''            " From T040021 a, T060011 b Where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK= a.TM_VC8_Supp_Owner and  " & strWhereClause
    ''        End If
    ''        strSql = strSql & " Order By TM_NU9_Task_no_PK desc"

    ''        Call SQL.Search("T040021", "Call_Detail", "CreateDataTableTask-1803", strSql, dsTask, "sachin", "Prashar")

    ''        dtvTask.Table = dsTask.Tables(0)


    ''        If IsPostBack Then
    ''            If intComp = "" Then
    ''                mstGetFunctionValue = WSSSearch.SearchCompNameID(Val(HttpContext.Current.ViewState("CompanyID")))
    ''                intComp = mstGetFunctionValue.ExtraValue
    ''            End If

    ''        Else

    ''        End If
    ''        '===============================
    ''        If dsTask.Tables(0).Rows.Count = 0 Then
    ''            rowTemp = dsTask.Tables(0).NewRow
    ''            For intCount = 0 To dsTask.Tables(0).Columns.Count - 1
    ''                Select Case dsTask.Tables(0).Columns(intCount).DataType.FullName
    ''                    Case "System.String"
    ''                        rowTemp.Item(intCount) = " "
    ''                    Case "System.Decimal", "System.Double", "System.Int32", "System.Int16"
    ''                        rowTemp.Item(intCount) = 0
    ''                    Case "System.DateTime"
    ''                End Select
    ''            Next
    ''            dsTask.Tables(0).Rows.Add(rowTemp)
    ''            dtvTask.Table = dsTask.Tables(0)
    ''        End If

    ''    Catch ex As Exception
    ''        CreateLog("Call-View", "CreateDataTableTask-1750", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
    ''    End Try
    ''    '===============================
    ''End Sub

#End Region

#Region "Fill Task Header Array"
    ''Private Sub FillHeaderArrayTask()
    ''    Dim t As New Control
    ''    Dim intCount As Integer
    ''    arrHeadersTask.Clear()
    ''    If Page.IsPostBack Then
    ''        For intCount = 0 To dtvTask.Table.Columns.Count - 1
    ''            arrHeadersTask.Add(Request.Form("cpnlCallTask:dtgTask:_ctl1:" + dtvTask.Table.Columns(intCount).ColumnName + "_H"))
    ''        Next
    ''    End If
    ''End Sub
#End Region

#Region "Fill Task Footer Array"
    ''Private Sub FillFooterArrayTask()
    ''    Dim t As New Control
    ''    Dim intCount As Integer
    ''    Dim intFooterIndex As Integer
    ''    arrFooterTask.Clear()
    ''    If Page.IsPostBack Then
    ''        For intCount = 0 To dtvTask.Table.Columns.Count - 1
    ''            intFooterIndex = dtvTask.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
    ''            arrFooterTask.Add(Request.Form("cpnlCallTask:dtgTask:_ctl" & intFooterIndex.ToString.Trim & ":" + dtvTask.Table.Columns(intCount).ColumnName + "_F"))
    ''        Next
    ''    End If
    ''End Sub
#End Region

#Region "Bind Task Grid"
    ''Private Sub BindGridTask()
    ''    HTMLEncodeDecode(mdlMain.Action.Encode, dtvTask)
    ''    SetCommentFlag(dtvTask, mdlMain.CommentLevel.TaskLevel)
    ''    dtgTask.DataSource = dtvTask
    ''    If Request.Form("txtrowvaluescall") <> 0 Then
    ''        introwvalues = Request.Form("txtrowvaluescall")
    ''    End If
    ''    dtgTask.DataBind()
    ''End Sub
#End Region

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
    ''Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
    ''    Dim objTextBox As Control

    ''    Try
    ''        TxtSubject_F.Text = ""
    ''        dtEstCloseDate.CalendarDate = ""
    ''        TxtEstimatedHrs.Text = ""
    ''        CDDLDependency_F.CDDLSetSelectedItem("")
    ''        CDDLTaskType_F.CDDLSetSelectedItem("")
    ''        CDDLTaskOwner_F.CDDLSetSelectedItem("")
    ''    Catch ex As Exception
    ''        CreateLog("Call-View", "ClearAllTextBoxes-3144", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
    ''    End Try
    ''End Sub
#End Region



    Private Sub ddlstview_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged

        Try
        Catch ex As Exception
            CreateLog("Call_View", "ddlstview_SelectedIndexChanged-3601", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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

        Dim dgi As DataGridItem
        If compColumnNo <> "" Or callNoColumnNo <> "" Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(compColumnNo + 2).Text.Trim = ViewState("CompName") And Val(dgi.Cells(callNoColumnNo + 2).Text.Trim) = Val(ViewState("CallNo")) Then
                    Exit For
                End If
            Next
        Else
        End If
        If GrdAddSerach.Items.Count = 0 Then
            CurrentPg.Text = 0
        End If
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

        Dim dgi As DataGridItem
        If compColumnNo <> "" Or callNoColumnNo <> "" Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(compColumnNo + 2).Text.Trim = ViewState("CompName") And Val(dgi.Cells(callNoColumnNo + 2).Text.Trim) = Val(ViewState("CallNo")) Then
                    Exit For
                Else
                End If
            Next
        Else
        End If

        If GrdAddSerach.Items.Count = 0 Then
            CurrentPg.Text = 0
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


        Dim dgi As DataGridItem
        If compColumnNo <> "" Or callNoColumnNo <> "" Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(compColumnNo + 2).Text.Trim = ViewState("CompName") And Val(dgi.Cells(callNoColumnNo + 2).Text.Trim) = Val(ViewState("CallNo")) Then

                    Exit For
                Else
                End If
            Next
        Else
        End If

        If GrdAddSerach.Items.Count = 0 Then
            CurrentPg.Text = 0
        End If

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

        Dim dgi As DataGridItem
        If compColumnNo <> "" Or callNoColumnNo <> "" Then
            For Each dgi In GrdAddSerach.Items
                If dgi.Cells(compColumnNo + 2).Text.Trim = ViewState("CompName") And Val(dgi.Cells(callNoColumnNo + 2).Text.Trim) = Val(ViewState("CallNo")) Then

                    Exit For
                Else

                End If
            Next
        Else

        End If

        If GrdAddSerach.Items.Count = 0 Then
            CurrentPg.Text = 0
        End If

    End Sub

    Private Sub SaveUserView()
        Dim intid = 799 ' screen id for call view screen
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

        sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=799 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

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
    Private Sub SavePageSize()
        Dim intid = 799
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
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=799 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

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
