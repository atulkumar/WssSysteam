Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

Partial Class SupportCenter_CallView_Tasks
    Inherits System.Web.UI.Page
    '*******************************************************************
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Toolbar1 As Microsoft.Web.UI.WebControls.Toolbar

    Protected WithEvents Toolbar2 As Microsoft.Web.UI.WebControls.Toolbar
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents LblErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button5 As System.Web.UI.WebControls.Button
    Protected WithEvents ddlstview As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dtgTask As New DataGrid
    Protected WithEvents TxtStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtSubject_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaskNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtEstCloseDate As DateSelector
    Protected WithEvents chkMandatory As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtEstimatedHrs As System.Web.UI.WebControls.TextBox
    Protected WithEvents imgSave As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgAdd As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgEdit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgCloseCall As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgAttachments As System.Web.UI.WebControls.ImageButton
    ' Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    ' --- Task controls
    Protected CDDLTaskType_F As New CustomDDL
    Protected CDDLTaskOwner_F As New CustomDDL
    '  Protected CDDLStatus_F As New CustomDDL
    Protected CDDLDependency_F As New CustomDDL
    Protected CDDLPriority_F As New CustomDDL
    '  Protected CDDLAgmt_F As New CustomDDL
    Protected WithEvents TxtStatus_F As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtSubject_F_F As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtProject_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgMonitor As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImgClose As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgBtnViewPopup As System.Web.UI.WebControls.ImageButton
    ' Protected WithEvents lblTitleLabelCallView As System.Web.UI.WebControls.Label
    Protected WithEvents pnl1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Panel2 As System.Web.UI.WebControls.Panel
    Protected WithEvents Panel3 As System.Web.UI.WebControls.Panel
    Protected WithEvents Panel4 As System.Web.UI.WebControls.Panel
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "global level declaration"

    Private mdvtable As New DataView ' store data from table for view grid 
    Private rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows
    Private rowvalueCall As Integer 'this is use with call view grid to stroed or assigned 
    Private arColumnName As New ArrayList ' this is stored grid's columns name to assined value to the texboxes
    Public introwvalues As Integer 'stored the selected row's value
    'Shared mshCall As Short 'store info when click on closed call button
    Private Shared intCol As Integer 'grid columns count
    'these variable store the position of the columns
    '****************************************
    Private Shared mintCompId As String
    Private Shared mintSuppComp As String
    Private Shared mintCallOwnerID As String
    Private Shared mintByWhomID As String
    Private Shared mintCallNoRowID As String
    '************************************
    Protected _currentPageNumber As Int32 = 1
    Public mintPageSize As Integer

    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************
    Private Shared arColWidth As New ArrayList 'width
    Private Shared arColumns As ArrayList = New ArrayList 'name
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList 'textboxes values
    Private Shared arSetColumnName As New ArrayList
    Private Shared mTextBox() As TextBox 'filled textboxes for search
    '*************************************************************
    Private txthiddenImage As String 'stored clicked button's cation  
    Public mstrsuppcomp As String 'stores the support comp during postbacks
    Private mintFileID As Integer
    Private mTaskRowValue As Integer
    Private shF As Short
    Private mshFlag As Short
    Private intComp As String
    Private ii As WebControls.Unit
    Private Shared dtvTask As New DataView
    Public mstrcomp As String
    Private marTextbox() As TextBox
    Private mblnValue As Boolean
    Private mstrFileName As String
    Private mstrFilePath As String
    Public strhiddenTable As String
    Public mstrCallNumber As String
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
    Public intCallNo As Integer
    Public intTaskNo As Integer
    Public strCompName As String
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
        'Put user code to initialize the page here
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        '***********************************
        'to store value in session to stop f5 duplicate data while pressing f5 in data entry
        If Not Page.IsPostBack Then
            ViewState("update") = Server.UrlEncode(System.DateTime.Now.ToString())
            ViewState("SortWayTask") = Nothing
            ViewState("SortOrderTask") = Nothing
        End If
        '**********************************
        cpnlCallTask.TitleClickable = False
        intCallNo = Request.QueryString("intCallNo")
        strCompName = Request.QueryString("strComp")

        ViewState("CallNo") = intCallNo
        ViewState("CompanyID") = WSSSearch.SearchCompName(strCompName).ExtraValue()

        cpnlCallTask.Text = "Task View &nbsp;&nbsp;" & "  " & " (Call# " & ViewState("CallNo") & " " & "  Company:  " & strCompName & ")"

        Try
            txtCSS(Me.Page, "cpnlCallTask")
            Dim strFilter As String
            Dim intCount As Integer
            Dim strSearch As String = " "
            mTaskRowValue = 0
            ViewState("gshPageStatus") = 0
            'javascript function added with controls
            '**********************************************************************************
            'BtnGrdSearch.Attributes.Add("onclick", "return CheckLength();")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            ImgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            '  txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
            '*******************************************************************************
            If Not IsPostBack Then
                ViewState("CallDetail") = "C"
            End If
            'if call not open 
            '***********************************************************
            cpnlCallTask.Enabled = True
            cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
            '***********************************************************
            cpnlError.Visible = False
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
                Else
                    mstrcomp = 0
                End If
            Else

            End If
            If ViewState("CompanyID") <> Nothing Then
                ViewState("CallStatus") = WSSSearch.GetCallStatus(Val(mstrCallNumber), ViewState("CompanyID"))
                If ViewState("CallStatus") = "CLOSED" Then
                    pnlTask.Visible = False
                Else
                    pnlTask.Visible = True
                End If
            End If

            If Val(ViewState("CompanyID")) <> 0 And Val(ViewState("CallNo")) <> 0 Then ' -- Fill Project Session on the basis of company and call
                ViewState("ProjectID") = WSSSearch.SearchProjectID(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))
            End If

            'change by 24 march
            If Val(ViewState("CallNo")) < 1 Then
                cpnlCallTask.TitleCSS = "test2"
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallTask.Enabled = False
            Else
                cpnlCallTask.TitleCSS = "test"
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                cpnlCallTask.Enabled = True
            End If
            'these statements check the button click caption 
            '***********************************************
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Logout"
                            LogoutWSS()
                            Exit Sub
                    End Select
                Catch ex As Exception
                    CreateLog("Call_View", "Load-286", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
                End Try
            End If
            '*********************************************************************

            strFilter = " "
            strSearch = " "

            If Page.IsPostBack Then
                '-- Preparing Search String
                For intCount = 3 To dtvTask.Table.Columns.Count - 1
                    strSearch = Request.Form("cpnlCallTask$dtgTask$ctl01$" + dtvTask.Table.Columns(intCount).ColumnName + "_H")
                    If Not IsDBNull(strSearch) And IsNothing(strSearch) = False Then
                        If Not strSearch.Trim.Equals("") Then
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
                Next
            End If

            If Not strFilter.Trim.Equals("") Then
                strFilter = strFilter.Remove((strFilter.Length - 4), 4)
            End If

            CreateGridTask()
            CreateDataTableTask(strFilter)
            FillHeaderArrayTask()
            'FillFooterArrayTask()
            createTemplateColumnsTask()

            If Not IsPostBack Then
                mstrCallNumber = 0
                cpnlCallTask.Enabled = False
                ViewState("mshCall") = 0
            Else

            End If
            Try
                'recreate Task Query and bind the grid
                Call CreateDataTableTask(strFilter)
                Call BindGridTask()
                '--Dependency combo
                '-----------------------------------------
                If IsNothing(ViewState("SortOrderTask")) = False Then
                    SortGRDDuplicateTask()
                End If

            Catch ex As Exception
                CreateLog("Call_View", "Load-409", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        Catch ex As Exception
            CreateLog("Call_View", "fill color Load-207", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
        'Security Block
        '****************************************
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
        'End of Security Block
        '*****************************************
    End Sub

#End Region

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
            arrColumnsNameTask.Add("EstClDate")
            arrColumnsNameTask.Add("EHr")
            arrColumnsNameTask.Add("Act")

            arrColumnsNameTask.Add("Prio")
            'arrColumnsNameTask.Add("Proj")
            'arrColumnsNameTask.Add("Agmt")

            arrWidthTask.Clear()
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(70)
            arrWidthTask.Add(216)
            arrWidthTask.Add(64)
            arrWidthTask.Add(72)
            arrWidthTask.Add(40)
            arrWidthTask.Add(88)
            arrWidthTask.Add(33)
            arrWidthTask.Add(24)
            arrWidthTask.Add(57)
            ' arrWidthTask.Add(40)
            'arrWidthTask.Add(49)


            dtgTask.Width = Unit.Pixel(818)
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
        If IsNothing(strWhereClause) Then
            strWhereClause = ""
        End If
        Try

            strSql = " select TM_CH1_Comment as Blank1, TM_CH1_Attachment as Blank2,TM_CH1_Forms as Blank3,  TM_NU9_Task_Order,TM_NU9_Task_no_PK, TM_VC50_Deve_Status,TM_VC1000_Subtsk_Desc,  TM_VC8_task_type,b.UM_VC50_UserID, TM_NU9_Dependency,convert(varchar,TM_DT8_Est_close_date,101) TM_DT8_Est_close_date, TM_FL8_Est_Hr,TM_CH1_Mandatory,  TM_VC8_Priority, a.TM_VC8_Supp_Owner  From T040021 a, T060011 b Where TM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and TM_NU9_Call_No_FK=" & Val(ViewState("CallNo")) & " and b.UM_IN4_Address_No_FK= a.TM_VC8_Supp_Owner "

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

            If dtvTask.Table.Rows.Count > 0 Then
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                cpnlCallTask.TitleCSS = "test"
                cpnlCallTask.Enabled = True
            Else
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                cpnlCallTask.TitleCSS = "test"
                cpnlCallTask.Enabled = True
            End If

            If IsNothing(strWhereClause) = False Then
                If strWhereClause.Trim <> "" Then
                    cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                    cpnlCallTask.TitleCSS = "test"
                    cpnlCallTask.Enabled = True
                End If
            End If

            If IsPostBack Then
                If intComp = "" Then
                    mstGetFunctionValue = WSSSearch.SearchCompNameID(Val(ViewState("CompanyID")))
                    intComp = mstGetFunctionValue.ExtraValue
                End If
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

        Catch ex As Exception
            CreateLog("Call-View", "CreateDataTableTask-1750", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
        '===============================
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

        Dim htGrdColumns As New Hashtable
        htGrdColumns.Add("TM_VC1000_Subtsk_Desc", 26)

        HTMLEncodeDecode(mdlMain.Action.Encode, dtvTask, htGrdColumns)
        SetCommentFlag(dtvTask, mdlMain.CommentLevel.TaskLevel, Session("PropCompanyID"), ViewState("CallNo"), ViewState("TaskNo"), 0)
        dtgTask.DataSource = dtvTask
        If Request.Form("txtrowvaluescall") <> 0 Then
            introwvalues = Request.Form("txtrowvaluescall")
        End If
        mTaskRowValue = 0
        dtgTask.DataBind()
    End Sub
#End Region

    Private Sub dtgTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTask.ItemDataBound
        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvTask
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvTask.ToTable()
        Dim strSelected As String
        dg.PageSize = 1
        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dtgTask.DataKeys(0) <> 0 Then
                    For intCount = 0 To 2       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        strID = dtgTask.DataKeys(e.Item.ItemIndex)
                        If strSelected = "1" Then      'If comment/ attachment is there 

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "'," & ViewState("CompanyID") & "," & ViewState("CallNo") & ")")

                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "'," & ViewState("CompanyID") & "," & ViewState("CallNo") & ")")

                        Else       ' If no comment/attachment is attached

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & strID & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "'," & ViewState("CompanyID") & "," & ViewState("CallNo") & ")")

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
                        strID = dtgTask.DataKeys(e.Item.ItemIndex)
                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & mTaskRowValue + 1 & "','" & introwvalues & "', 'cpnlCallTask_dtgTask')")
                        If intCount = 8 Then ' for task owner
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheck555(" & ViewState("CallNo") & ", '" & rowvalue & "','cpnlCallTask_dtgTask','" & ViewState("SuppComp") & "','" & HTMLEncodeDecode(mdlMain.Action.Decode, e.Item.Cells(intCount).ToolTip) & "'," & dtvTask.Table.Rows(e.Item.ItemIndex).Item("TM_VC8_Supp_Owner") & ")")
                        Else '  not    for taskl owner
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheckTaskEdit('" & strID & "', '" & mTaskRowValue + 1 & "', 'cpnlCallTask_dtgTask')")
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
        Dim objTextBox As Control

        Try
            TxtSubject_F.Text = ""
            dtEstCloseDate.CalendarDate = ""
            TxtEstimatedHrs.Text = ""
            CDDLDependency_F.CDDLSetSelectedItem("")
            CDDLTaskType_F.CDDLSetSelectedItem("")
            CDDLTaskOwner_F.CDDLSetSelectedItem("")
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
                CDDLPriority_F.CDDLSetSelectedItem(dtrCallno.Item("CM_VC200_Work_Priority"))
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

    Protected Sub SOrtGrid(ByVal sender As Object, ByVal e As CommandEventArgs)
        ViewState("SortOrderTask") = e.CommandArgument
        SortGRDTask()
    End Sub

    Private Sub SortGRDTask()
        If Val(ViewState("SortWayTask")) Mod 2 = 0 Then
            dtvTask.Sort = ViewState("SortOrderTask") & " ASC"
        Else
            dtvTask.Sort = ViewState("SortOrderTask") & " DESC"
        End If
        ViewState("SortWayTask") += 1
        mTaskRowValue = 0
        dtgTask.DataSource = dtvTask
        dtgTask.DataBind()
    End Sub

    Private Sub SortGRDDuplicateTask()
        Try
            If Val(ViewState("SortWayTask")) Mod 2 = 0 Then
                dtvTask.Sort = ViewState("SortOrderTask") & " DESC"
            Else
                dtvTask.Sort = ViewState("SortOrderTask") & " ASC"
            End If
            mTaskRowValue = 0
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
        Catch ex As Exception
        End Try
    End Sub
End Class
