'***********************************************************************************************************
' Page                 :- Action view
' Purpose              :- This screen shows the actions for a selected task in task view screen.
' Tables used          :- T040031, T040011, T040021 T010011, T070011, T070031, T070042, T060011, T060022,                              T030212
' Date					  Author						Modification Date				Description
' 17/03/06	  	       sachin/ Amit          			                                    Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports ION.Logging
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data

Partial Class SupportCenter_CallView_ActionView
    Inherits System.Web.UI.Page
    Private Shared intID As Int16

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Toolbar1 As Microsoft.Web.UI.WebControls.Toolbar

    Protected WithEvents Toolbar2 As Microsoft.Web.UI.WebControls.Toolbar
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents LblErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button5 As System.Web.UI.WebControls.Button
    Protected WithEvents grdAction As New DataGrid
    Protected WithEvents TxtStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtSubject As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtProject As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtPriority As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaskNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtTaskOwner As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtTaskType As System.Web.UI.WebControls.TextBox
    Protected WithEvents cpnlTaskView As CustomControls.Web.CollapsiblePanel
    Protected WithEvents TxtActions_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtSubject_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtTaskOwner_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtPriority_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtNo_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtAc_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtActo_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtUsed_F As System.Web.UI.WebControls.TextBox
    Protected WithEvents Placeholder2 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgPlusw As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgDefault As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Private designerPlaceholderDeclaration As System.Object
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

#Region "global level declaration"

    Public introwvalues As String
    Public strhiddenTable As String
    Private txthiddenImage As String
    Dim intComp As String
    Public mstrcomp As String
    Public mstrCallNumber As String
    Public mstrTaskNumber As String
    Private null As System.DBNull
    Private arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Private arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Private arrImageUrlNew As New ArrayList 'Used to store new comments
    Private tclAction() As TemplateColumn
    Private mTaskRowValue As Integer
    Private Shared arrHeadersTask As New ArrayList
    Private Shared arrFooterTask As New ArrayList
    Private Shared dtvAction As New DataView
    Private Shared arrColumnsNameAction As New ArrayList
    Private Shared arrWidthAction As New ArrayList
    Private Shared arrColumnsWidthAction As New ArrayList
    Private mintUserID As Integer
    Private Shared mintCallNoPlace As Integer
    Private Shared mintCallnoPlaceInTask As Integer
    Public intCallNo As Integer
    Public intTaskNo As Integer
    Public strCompName As String
    Private strFilter As String

#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        Call txtCSS(Me.Page, "cpnlTaskAction")

        'If Page.IsPostBack = False Then
        intCallNo = Request.QueryString("intCallNo")
        intTaskNo = Request.QueryString("intTaskNo")
        strCompName = Request.QueryString("strComp")

        If strCompName <> "" Then
            intComp = Request.Form("txtComp")
            mstGetFunctionValue = WSSSearch.SearchCompName(strCompName)
            mstrcomp = mstGetFunctionValue.ExtraValue
            intComp = mstGetFunctionValue.ExtraValue
            ViewState("CompanyID") = mstGetFunctionValue.ExtraValue
        Else
            mstrcomp = 0
        End If

        Dim intRow As Integer
        If SQL.Search("Task_View", "Load-341", "select * from T040031 where AM_NU9_Call_Number=" & intCallNo & " and AM_NU9_Task_Number=" & intTaskNo & " and AM_NU9_Comp_ID_FK=" & intComp, intRow) = False Then
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.Text = "ActionView"
            lstError.Items.Clear()
            lstError.Items.Add("No Action Exists for this Task...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        cpnlTaskAction.Text = "ActionView: Call#" & intCallNo & "-Task#" & intTaskNo & "-Company:" & strCompName

        Dim intCount As Integer
        Dim strSearch As String
        Dim StrUserID As String

        'get logged user id
        mintUserID = HttpContext.Current.Session("PropUserID")
        StrUserID = HttpContext.Current.Session("PropUserID")
        ViewState("gshPageStatus") = 0
        cpnlError.Visible = False
        txthiddenImage = Request.Form("txthiddenImage")
        introwvalues = Request.Form("txtrowvalues")
        ViewState("CompanyID") = mstrcomp
        strhiddenTable = "cpnlTaskAction_grdAction"
        '*******************************************************
        If strhiddenTable = "cpnlTaskAction_grdAction" Then
            ViewState("ActionNo") = Val(Request.Form("txtTask"))
            ViewState("ActionNo") = 0
            ViewState("TaskNo") = Val(Request.Form("txtTask"))
            mstrTaskNumber = ViewState("TaskNo")
            ViewState("CallNo") = Val(Request.Form("txthiddenCallNo"))
            mstrCallNumber = ViewState("CallNo")
            ViewState("TaskNo") = intTaskNo
            ViewState("CallNo") = intCallNo
        End If
        '****************************************************************
        If ViewState("ActionNo") < 1 And ViewState("TaskNo") < 1 Then
            cpnlTaskAction.Enabled = False
            cpnlTaskAction.TitleCSS = "test2"
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
        Else
            cpnlTaskAction.Enabled = True
            cpnlTaskAction.TitleCSS = "test"
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
        End If

        strFilter = ""
        strSearch = ""

        If Page.IsPostBack Then
            '-- Preparing Search String
            For intCount = 2 To dtvAction.Table.Columns.Count - 1
                strSearch = Request.Form("cpnlTaskAction$grdAction$ctl01$" + dtvAction.Table.Columns(intCount).ColumnName + "_H")
                If Not IsNothing(strSearch) Then
                    If Not strSearch.Trim.Equals("") Then
                        If dtvAction.Table.Columns(intCount).DataType.FullName = "System.Decimal" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Int32" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Int16" Or dtvAction.Table.Columns(intCount).DataType.FullName = "System.Double" Then
                            strSearch = strSearch.Replace("*", "")
                            strFilter = strFilter & dtvAction.Table.Columns(intCount).ColumnName & " = '" & strSearch & "' AND "
                        Else
                            If strSearch.Contains("*") = True Then
                                strSearch = strSearch.Replace("*", "%")
                            Else
                                strSearch &= "%"
                            End If
                            strFilter = strFilter & dtvAction.Table.Columns(intCount).ColumnName & " like '" & strSearch & "' AND "
                        End If
                    End If
                End If
            Next
        Else
            ViewState("SortOrder") = Nothing
            ViewState("SortWay") = Nothing
        End If

        If Not strFilter.Trim.Equals("") Then
            strFilter = strFilter.Remove((strFilter.Length - 4), 4)
        End If

        CreateGridAction()
        CreateDataTableAction(strFilter)
        FillHeaderArrayAction()
        FillFooterArrayAction()
        createTemplateColumnsAction()
        Call BindGridAction()

        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If

        If Not IsPostBack Then
        Else
            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Search"
                            'Call BtnGrdSearch_Click(Me, New EventArgs)
                    End Select
                Catch ex As Exception
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    CreateLog("Action View", "Load-264", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                End Try
            End If

        End If
        'Security Block
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            intID = 8 '8 is ToDoList's ID which is parent SCREEN
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block
    End Sub

#End Region

#Region "Create Action Grid"
    Private Sub CreateGridAction()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        grdAction.ID = "grdAction"
        grdAction.DataKeyField = "AM_NU9_Action_Number"
        grdAction.AllowSorting = True
        Call FormatGridAction()
        PlaceHolder1.Controls.Add(grdAction)
    End Sub

    Private Sub FormatGridAction()
        grdAction.AutoGenerateColumns = False
        grdAction.AllowPaging = True
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

            arrColumnsNameAction.Clear()

            arrColumnsNameAction.Add("Com")
            arrColumnsNameAction.Add("Att")
            arrColumnsNameAction.Add("Act#")
            arrColumnsNameAction.Add("Description")
            arrColumnsNameAction.Add("Action<u>O</u>wner")
            arrColumnsNameAction.Add("Action_Date")
            arrColumnsNameAction.Add("Hrs.")

            arrWidthAction.Clear()

            arrWidthAction.Add(10)
            arrWidthAction.Add(10)
            arrWidthAction.Add(30)
            arrWidthAction.Add(50)
            arrWidthAction.Add(50)
            arrWidthAction.Add(74)
            arrWidthAction.Add(15)

            arrColumnsWidthAction.Clear()

            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(0)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(1)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(2)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Percentage(arrWidthAction(3)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(4)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(5)))
            arrColumnsWidthAction.Add(System.Web.UI.WebControls.Unit.Point(arrWidthAction(6)))

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
                AddHandler AddEventOnGrigHeader.OnSort, AddressOf SOrtGrid
                tclAction(intCount + 1).HeaderTemplate = AddEventOnGrigHeader
                tclAction(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvAction.Table.Columns(intCount).ToString + "_F", False)
                tclAction(intCount + 1).ItemStyle.Width = arrColumnsWidthAction(intCount)    'System.Web.UI.WebControls.Unit.Point(arrColumnsWidthAction(intCount))
                grdAction.Columns.Add(tclAction(intCount + 1))
            Next

        Catch ex As Exception
            CreateLog("TODO List", "CreateTemplateColumnsAction-1121", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction", )
        End Try

    End Sub
#End Region

#Region "CreateDataTableAction"
    Private Sub CreateDataTableAction(ByVal strWhereClause As String)

        Dim dsAction As New DataSet
        Dim strSql As String
        Dim rowTemp As System.Data.DataRow
        Dim intCount As Int32
        'New check-> Only dispaly External action.
        If Session("PropCompanyType") = "SCM" Then
            strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID," & _
                     " convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr  From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & ViewState("CallNo").ToString & " and AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number=" & ViewState("TaskNo").ToString
        Else
            strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description,b.UM_VC50_UserID+'~'+convert(varchar(8),a.AM_VC8_Supp_Owner) as UM_VC50_UserID," & _
                     " convert(varchar,AM_DT8_Action_Date) AM_DT8_Action_Date,AM_FL8_Used_Hr From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & ViewState("CallNo").ToString & " and AM_NU9_Comp_ID_FK=" & Val(ViewState("CompanyID")) & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  and AM_VC8_ActionType='External' and AM_NU9_Task_Number=" & ViewState("TaskNo").ToString
        End If
        strSql = strSql & " Order By AM_NU9_Action_Number asc"
        Call SQL.Search("T040031", "ToDoList", "CreateDataTableAction-1911", strSql, dsAction, "sachin", "Prashar")

        dtvAction = New DataView
        dtvAction.Table = dsAction.Tables(0)

        Dim htDateCols As New Hashtable
        htDateCols.Add("AM_DT8_Action_Date", 1)
        SetDataTableDateFormat(dtvAction.Table, htDateCols)

        If Not strWhereClause.Trim.Equals("") Then
            GetFilteredDataView(dtvAction, strWhereClause)
        End If
        If dtvAction.Table.Rows.Count > 0 Then
            cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
            cpnlTaskAction.TitleCSS = "test"
        Else
            cpnlTaskAction.State = CustomControls.Web.PanelState.Collapsed
            cpnlTaskAction.TitleCSS = "test2"
        End If
        If IsNothing(strWhereClause) Then
            strWhereClause = ""
        End If
        If strWhereClause.Trim <> "" Then
            cpnlTaskAction.State = CustomControls.Web.PanelState.Expanded
            cpnlTaskAction.TitleCSS = "test"
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
        End If
    End Sub
#End Region

#Region "Fill Action Header Array"
    Private Sub FillHeaderArrayAction()

        Dim t As New Control
        Dim intCount As Integer
        arrHeadersTask.Clear()
        If Page.IsPostBack Then
            For intCount = 0 To dtvAction.Table.Columns.Count - 1
                arrHeadersTask.Add(Request.Form("cpnlTaskAction$grdAction$ctl01$" + dtvAction.Table.Columns(intCount).ColumnName + "_H"))
            Next
        End If
    End Sub
#End Region

#Region "Fill Action Footer Array"
    Private Sub FillFooterArrayAction()

        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        arrFooterTask.Clear()
        If Page.IsPostBack Then
            For intCount = 0 To dtvAction.Table.Columns.Count - 1
                intFooterIndex = dtvAction.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                arrFooterTask.Add(Request.Form("cpnlTaskAction$grdAction$ct0l$" & intFooterIndex.ToString.Trim & "$" + dtvAction.Table.Columns(intCount).ColumnName + "_F"))
            Next
        End If
    End Sub
#End Region

#Region "Bind Action Grid"
    Private Sub BindGridAction()
        Try
            Dim htGrdColumns As New Hashtable
            htGrdColumns.Add("AM_VC_2000_Description", 44)
            HTMLEncodeDecode(mdlMain.Action.Encode, dtvAction, htGrdColumns)
            SetCommentFlag(dtvAction, mdlMain.CommentLevel.ActionLevel, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
            grdAction.DataSource = dtvAction
            grdAction.DataBind()
            HTMLEncodeDecode(mdlMain.Action.Decode, dtvAction)
        Catch ex As Exception
            CreateLog("TODO List", "BindGridAction-1221", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction", )
        End Try

    End Sub
#End Region

#Region "ActionGridItemDataBound"

    Private Sub grdAction_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAction.ItemDataBound

        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvAction
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As New DataTable
        dtBound = dtvAction.ToTable()
        'dtBound.AcceptChanges()
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
                            If intCount = 4 Then ' -- for action owner cell
                                structTempActionOwner.Id = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(1)
                                structTempActionOwner.Name = CType(IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString), String).Split("~")(0)
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = structTempActionOwner.Name
                            Else
                                CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is null, " ", dtBound.Rows(cnt)(intCount).ToString)
                                'CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = IIf(dv.Table.Rows(cnt).Item(intCount).ToString Is null, " ", dv.Table.Rows(cnt).Item(intCount).ToString)
                            End If
                        End If

                        strID = grdAction.DataKeys(e.Item.ItemIndex)

                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "',0, '" & mTaskRowValue + 1 & "', 'cpnlTaskAction_grdAction')")
                        If intCount = 4 Then
                            e.Item.Cells(intCount).ForeColor = System.Drawing.Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & structTempActionOwner.Id & "')")
                        End If
                    Next
                    mTaskRowValue += 1
                Else
                    For intCount = 0 To 1      'For Image Fields
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(1)
                    Next
                    For intCount = 2 To dtvAction.Table.Columns.Count - 1
                        CType(e.Item.Cells(intCount).FindControl(dtvAction.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If
        Catch ex As Exception
            CreateLog("TODO List", "ItemDataBound-2055", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction", )
        End Try
    End Sub
#End Region
    Protected Sub SOrtGrid(ByVal sender As Object, ByVal e As CommandEventArgs)

        ViewState("SortOrder") = e.CommandArgument
        SortGRD()

    End Sub
    Private Sub SortGRD()

        'grdAction.Columns.Clear()
        'CreateGridAction()
        ''CreateDataTableAction(strFilter)

        If Val(ViewState("SortWay")) Mod 2 = 0 Then
            dtvAction.Sort = ViewState("SortOrder") & " ASC"
        Else
            dtvAction.Sort = ViewState("SortOrder") & " DESC"
        End If
        ViewState("SortWay") += 1
        mTaskRowValue = 0
        'FillHeaderArrayAction()
        'FillFooterArrayAction()
        ''createTemplateColumnsAction()

        'Dim htGrdColumns As New Hashtable
        'htGrdColumns.Add("AM_VC_2000_Description", 44)
        'HTMLEncodeDecode(mdlMain.Action.Encode, dtvAction, htGrdColumns)
        'SetCommentFlag(dtvAction, mdlMain.CommentLevel.ActionLevel)
        'grdAction.DataSource = Nothing
        grdAction.DataSource = dtvAction
        grdAction.DataBind()
        'HTMLEncodeDecode(mdlMain.Action.Decode, dtvAction)

    End Sub
    Private Sub SortGRDDuplicate()
        Try

            If Val(ViewState("SortWay")) Mod 2 = 0 Then
                dtvAction.Sort = ViewState("SortOrder") & " DESC"
            Else
                dtvAction.Sort = ViewState("SortOrder") & " ASC"
            End If
            mTaskRowValue = 0
            grdAction.DataSource = dtvAction
            grdAction.DataBind()

        Catch ex As Exception
        End Try
    End Sub
End Class
