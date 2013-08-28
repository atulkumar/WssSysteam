'************************************************************************************************************
' Page                   : - OverwiteView
' Purpose                : - This form is used to copy the view from an existing view & assign it to another                              user.  
' Tables used            : - T010011, T030201, T030212
' Date					Author						Modification Date					Description
' 24/04/06			sachin prashar					-------------------					Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports System.Web.Security
Imports System
Imports ION.Data
Imports ION.Logging
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.SessionState
'Session Names Used on this Page are :-
'-------                     ---------'
'Session("PropRootDir")


Partial Class AdministrationCenter_UserOverwriteView_OverwriteView
    Inherits System.Web.UI.Page

    Dim intMaxViewID As Integer
    'Protected WithEvents GrdAddSerach As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents txtViewID As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtViewN As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Collapsiblepanel1 As CustomControls.Web.CollapsiblePanel
    Protected WithEvents Collapsiblepanel2 As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents cpnlviewinfo As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents cpnlviewdetail As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents imgDelete As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents DDLCompany As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DDLRole As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents lblTitleLabelUserOV As System.Web.UI.WebControls.Label
    'Protected WithEvents imgSearch As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents pnlMsg As System.Web.UI.WebControls.Panel
    Dim intVIewID As Integer

#Region "global level declaration"


    ' Holding text boxes created above UDC Type Grid
    Dim mtxtUDCTypeQuery As TextBox()

    Private Shared arColWidth As New ArrayList
    Dim flage As Integer
    Dim mdvtable As New DataView
    Dim marTextbox() As TextBox
    Private Shared mTextBox() As TextBox
    Dim mintColumns As Integer
    Dim mshFlag As Short
    Dim Expanded2 As New PlaceHolder
    Dim ii As WebControls.Unit
    Dim rowvalue As Integer = 0
    Dim shF As Short
    Dim flg As Short
    Public mintPageSize As Integer
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared intCol As Integer

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            lstError.Items.Clear()
            'cpnlErrorPanel.Visible = False
            If Not IsPostBack Then
                lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
                imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
                imgDelete.Attributes.Add("Onclick", "return SaveEdit('Delete');")
                txtViewN.Attributes.Add("ReadOnly", "ReadOnly")
                DDLCompany.Attributes.Add("OnChange", "CompanyChange();")
                DDLRole.Attributes.Add("OnChange", "FillHidden();")
            End If
            txtCSS(Me.Page)
            If Not IsPostBack Then

                FillNonUDCDropDown(DDLCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from   T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ")", True)
            Else
                Dim strRoleData As String = Request.Form("txthiddenRoleData")
                If strRoleData <> "" Then

                    Dim arr() As String
                    Dim arrID() As String
                    Dim arrName() As String
                    arr = strRoleData.Split("~")
                    arrID = arr(1).Remove(arr(1).Length - 1, 1).Split("^")
                    arrName = arr(0).Remove(arr(0).Length - 1, 1).Split("^")
                    DDLRole.Items.Clear()
                    DDLRole.Items.Add(New ListItem("", ""))
                    For intI As Integer = 0 To arrID.Length - 1
                        DDLRole.Items.Add(New ListItem(arrName(intI), arrID(intI)))
                    Next
                    If Request.Form("txthiddenRoleID") <> "" Then
                        DDLRole.SelectedValue = Request.Form("txthiddenRoleID")
                    End If
                End If
            End If

            ' fill the textboxes value into the array 
            '**********************************
            If IsPostBack Then
                arrtextvalue.Clear()
                For i As Integer = 0 To arColumns.Count - 1
                    arrtextvalue.Add(Request.Form("cpnlviewdetail$" & arCol.Item(i)))
                Next
            End If

            FillView()
            CreateTextBox()
            Dim txthiddenImage = Request.Form("txthiddenImage")

            If Request.Form("txthidden") = "" Then
            Else
                HttpContext.Current.Session("SAddressNumber") = Request.Form("txthidden")
            End If

            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Logout"
                        Call LogoutWSS()
                    Case "Delete"
                        Call DeleteView()
                End Select
            End If

            If txtViewID.Value.Trim <> "" Then
                mdvtable.Sort = "ViewID"
                Dim intRowNo As Integer = mdvtable.Find(txtViewID.Value.Trim)
                GrdAddSerach.Items.Item(intRowNo).BackColor = System.Drawing.Color.FromArgb(212, 212, 212)
            End If
            BtnGrdSearch_Click(Me, New EventArgs)

            ' -- Security Block
            If Not IsPostBack Then
                Dim intId As Integer
                If 1 = 1 Then 'This is a fake block for executing security because visibility of controls is changing in programming 
                    Dim str As String
                    str = Session("PropRootDir")
                    intId = 533
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
            End If
            ' --End of Security Block------------------------------------------------------

        Catch ex As Exception
            CreateLog("OverWriteView", "load-205", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Function DeleteView() As Boolean

        If txtViewN.Text.Equals("Default") Then
            lstError.Items.Clear()
            lstError.Items.Add("Default View cannot be Deleted by any one...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Function
        End If
        ' commented this code because txtViewN is read only and its value is not maintained during post back
        If txtViewN.Text.Equals("") Then
            lstError.Items.Clear()
            lstError.Items.Add("Please select view for delete...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Function
        End If

        Try

            If SQL.Delete("AdminEditView", "DeleteView-194", "delete from T030201 where UV_IN4_View_ID='" & txtViewID.Value.Trim & "' ", SQL.Transaction.Serializable) = True Then
                SQL.Delete("AdminEditView", "DeleteView-195", "delete from T030212 where UV_IN4_View_ID='" & txtViewID.Value.Trim & "' ", SQL.Transaction.Serializable)
                SQL.Delete("AB_ViewColumns", "DeleteView-1347", "delete from T030213 where UV_IN4_VIEW_ID=" & txtViewID.Value.Trim & "", SQL.Transaction.Serializable)   'Delete data from view profile table 

                lstError.Items.Clear()
                lstError.Items.Add("View Deleted successfully... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                '  ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                '****************************************************************
                'to fill to do list session and set default value in sessions
                If ddlViewType.SelectedValue = "502" Then
                    If Session("TDLViewValue") = txtViewID.Value.Trim Then
                        Session("TDLViewName") = "Default"
                        Session("TDLViewValue") = 0
                    End If
                End If
                'to fill Call View session and set default value in sessions
                If ddlViewType.SelectedValue = "463" Then
                    If Session("CallViewValue") = txtViewID.Value.Trim Then
                        Session("CallViewName") = "Default"
                        Session("CallViewValue") = 0
                    End If
                End If
                If ddlViewType.SelectedValue = "799" Then
                    If Session("CallViewSimpleValue") = txtViewID.Value.Trim Then
                        Session("CallViewsimpleName") = "Default"
                        Session("CallViewSimpleValue") = 0
                    End If
                End If

                'to fill Task View session and set default value in sessions
                If ddlViewType.SelectedValue = "464" Then
                    If Session("TasklViewValue") = txtViewID.Value.Trim Then
                        Session("TaskViewName") = "Default"
                        Session("TaskViewValue") = 0
                    End If
                End If
                'to fill Historic View session and set default value in sessions
                If ddlViewType.SelectedValue = "536" Then
                    If Session("HistoricViewValue") = txtViewID.Value.Trim Then
                        Session("HistoricViewName") = "Default"
                        Session("HistoricViewValue") = 0
                    End If
                End If
                '*************************************************************
                txtViewN.Text = ""
                txtViewID.Value = ""
                DDLCompany.SelectedItem.Text = ""

                FillView()

            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ' ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("OverWriteView", "deleteView-1333", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

        Dim shFlag As Short = 0
        Try
            lstError.Items.Clear()
            'display error if view name blank
            '***********************
            If txtViewName.Text.Equals("") Then
                lstError.Items.Add("View Name cannot be blank...")
                shFlag = 1
            End If
            '******************************
            'display error if Company name blank
            '***********************
            If DDLCompany.SelectedItem.Text.Equals("") Then
                lstError.Items.Add("Company Name cannot be blank...")
                shFlag = 1
            End If
            '******************************
            'display error if Company name blank
            '***********************
            If DDLRole.SelectedItem.Text.Equals("") Then
                lstError.Items.Add("Role Name cannot be blank...")
                shFlag = 1
            End If
            '******************************
            'display error if View name not selected
            '***********************
            If txtViewID.Value.Equals("") Then
                lstError.Items.Add("Please Select view to Copy the View Data...")
                shFlag = 1
            End If
            '******************************
            'Display error message if view already exist
            '**************************************
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T030201"
            SQL.DBTracing = False

            Dim strCheck As String = SQL.Search("OverWriteView", "imgsave_click", "select UV_VC50_View_Name from T030201 where UV_VC50_View_Name='" & txtViewName.Text.Trim.Replace("'", "''") & "' and UV_IN4_Role_ID=" & Request.Form("txthiddenRoleID") & " and UV_VC50_tbl_Name='" & ddlViewType.SelectedValue & "' and UV_NU9_Comp_ID=" & DDLCompany.SelectedValue & "")

            If Not IsNothing(strCheck) Then
                lstError.Items.Add("View Name already exist in this Role...")
                shFlag = 1
            End If
            If shFlag = 1 Then
                'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If

            '**************************************
            Dim sqdrCol As SqlDataReader
            Dim blnReturn As Boolean

            'Get max max value for vew view
            '**********************************************
            sqdrCol = SQL.Search("imgsave", "click", "select isnull(max(UV_IN4_View_ID),0) as UV_IN4_View_ID from T030201", SQL.CommandBehaviour.CloseConnection, blnReturn)
            If blnReturn = False Then
                Exit Sub
            Else
                While sqdrCol.Read
                    intMaxViewID = sqdrCol.Item("UV_IN4_View_ID")
                    intMaxViewID += 1
                End While
                sqdrCol.Close()
            End If
            '**************************************************************
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("UV_VC50_View_Name")
            arColumnName.Add("UV_IN4_View_ID")
            arColumnName.Add("UV_VC50_tbl_Name")
            arColumnName.Add("UV_IN4_Role_ID")
            arColumnName.Add("UV_NU9_Comp_ID")


            arRowData.Add(txtViewName.Text.Trim)
            arRowData.Add(intMaxViewID)
            arRowData.Add(ddlViewType.SelectedValue)
            arRowData.Add(DDLRole.SelectedValue)
            arRowData.Add(DDLCompany.SelectedValue)


            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' save the view name
            SQL.DBConnection = strConnection
            'SQL.DBTable = "T030201"
            SQL.DBTracing = False

            If SQL.Save("T030201", "imgsave", "click", arColumnName, arRowData) = True Then

                Dim arCol As New ArrayList
                Dim arRow As New ArrayList
                Dim sqrdView As SqlDataReader
                Dim blnResult As Boolean

                arCol.Clear()
                arRow.Clear()

                SQL.DBConnection = strConnection
                ' SQL.DBTable = "T030212"
                SQL.DBTracing = False

                'get value from grid and base on it fatch data from database
                sqrdView = SQL.Search("OverWriteView", "Click-223", "select * from T030212 where uv_in4_view_id=" & txtViewID.Value.Trim & " order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnResult, "imgSave")

                Dim ViewMulti As SQL.AddMultipleRows

                arCol.Add("UV_IN4_ID")
                arCol.Add("UV_IN4_View_ID")
                arCol.Add("UV_VC50_COL_Name")
                arCol.Add("UV_VC50_COL_Value")
                arCol.Add("UV_SI2_Order_By")
                arCol.Add("UV_VC10_Col_width")
                arCol.Add("UV_VC50_tbl_Name")
                arCol.Add("UV_VC5_AD")
                arCol.Add("UV_NU9_SO")
                arCol.Add("UV_VC5_FA")
                arCol.Add("UV_VC20_Value")

                If blnResult = True Then
                    While sqrdView.Read
                        arRow.Add(sqrdView("UV_IN4_ID"))
                        arRow.Add(intMaxViewID)
                        arRow.Add(sqrdView("UV_VC50_COL_Name"))
                        arRow.Add(sqrdView("UV_VC50_COL_Value"))
                        arRow.Add(sqrdView("UV_SI2_Order_By"))
                        arRow.Add(sqrdView("UV_VC10_Col_width"))
                        arRow.Add(sqrdView("UV_VC50_TBL_Name"))
                        arRow.Add(sqrdView("UV_VC5_AD"))
                        arRow.Add(sqrdView("UV_NU9_SO"))
                        arRow.Add(sqrdView("UV_VC5_FA"))
                        arRow.Add(sqrdView("UV_VC20_Value"))

                        ViewMulti.Add("T030212", arCol, arRow)
                    End While
                    sqrdView.Close()
                    ViewMulti.Save()
                    lstError.Items.Add("Records Saved successfully...")
                    '  ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                End If
                ViewMulti.Dispose()
            End If
            txtViewName.Text = ""
            txtViewN.Text = ""
            txtViewID.Value = ""
            DDLCompany.SelectedIndex = 0
            DDLRole.Items.Clear()
            DDLRole.Items.Add("")
            'Call ddlViewType_SelectedIndexChanged(Me, New System.EventArgs)
            '************************
            'fill data after save new view
            FillView()
            BtnGrdSearch_Click(Me, New EventArgs)
            '****************************
        Catch ex As Exception
            SQL.Delete("imgsave", "click", "Delete from T030201 where UV_IN4_View_ID=" & intMaxViewID & " and UV_VC50_View_Name='" & txtViewName.Text.Trim & "' and UV_VC50_tbl_Name='" & ddlViewType.SelectedValue & "'", SQL.Transaction.Serializable)
            SQL.Delete("imgsave", "click", "Delete from T030212 where UV_IN4_View_ID=" & intMaxViewID & " and UV_VC50_tbl_Name='" & ddlViewType.SelectedValue & "'", SQL.Transaction.Serializable)
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            CreateLog("OW-View", "imgSave_click-220", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#Region "fill View"
    Private Sub FillView()
        Try
            Dim dsFromView As New DataSet
            'If SQL.Search("", "", "select *,ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompName  from T030201,T070031,T010011 where UV_VC50_tbl_Name='" & ddlViewType.SelectedValue & "' and UV_IN4_Role_ID=ROM_IN4_Role_ID_PK and CI_NU8_Address_Number=UV_NU9_Comp_ID") Then
            If SQL.Search("T030201", "OverwriteView", "FillView", "select UV_IN4_View_ID as ViewID,UV_VC50_View_Name as ViewName,UV_VC50_tbl_Name as ScreenName, ROM_VC50_Role_Name as RoleName,CI_VC36_Name as CompName  from T030201,T070031,T010011 where UV_VC50_tbl_Name='" & ddlViewType.SelectedValue & "' and UV_IN4_Role_ID=ROM_IN4_Role_ID_PK and CI_NU8_Address_Number=UV_NU9_Comp_ID AND UV_NU9_Comp_ID IN(" & GetCompanySubQuery() & ") order by ViewID", dsFromView, "sachin", "Prashar") Then
                mdvtable.Table = dsFromView.Tables("T030201")
                GrdAddSerach.Columns.Clear()
                rowvalue = 0
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

                cpnlviewdetail.State = CustomControls.Web.PanelState.Expanded
                cpnlviewdetail.Enabled = True
                cpnlviewdetail.TitleCSS = "test"
            Else
                lstError.Items.Add("No View opened so far...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlviewdetail.State = CustomControls.Web.PanelState.Collapsed
                cpnlviewdetail.Enabled = False
                cpnlviewdetail.TitleCSS = "test2"
            End If
            FormatGrid()
            GetColumns()
            'create textbox at run time at head of the datagrid        
        Catch ex As Exception
            CreateLog("OW Views", "Fill View", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Sub CreateTextBox()

        arColumns.Clear()
        Dim _textbox As TextBox
        Dim intii As Integer

        arCol.Clear()

        arCol.Add("ViewID")
        arCol.Add("ViewName")
        arCol.Add("ScreenName")
        arCol.Add("RoleName")
        arCol.Add("CompanyName")

        Try

            'fill the columns count into the array from mdvtable view
            intCol = mdvtable.Table.Columns.Count
            'If Not IsPostBack Then
            ReDim mTextBox(intCol)
            ' End If
            For intii = 0 To intCol - 1
                _textbox = New TextBox
                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    arCol.Add(arCol.Item(intii))
                    If arCol.Item(intii) = "ScreenName" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " BackColor=""WhiteSmoke"" readonly=""true"" height=""15px"" BorderWidth=""1px"" BorderColor=""Gray""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If
                    _textbox.ID = arCol.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = arrtextvalue.Item(intii)
                    End If
                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)

                    If strcolid = "ScreenName" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " BackColor=""WhiteSmoke"" height=""15px""  readonly=""true"" BorderWidth=""1px"" BorderColor=""Gray""></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName
                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("OW-Views", "CreateTextBox-536", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(50)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(150)

        Try
            GrdAddSerach.Columns.Clear()

            GrdAddSerach.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                GrdAddSerach.Columns.Add(Bound_Column)
            Next

        Catch ex As Exception
            CreateLog("OW-Views", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(49)
        arColWidth.Add(96)
        arColWidth.Add(97)
        arColWidth.Add(95)
        arColWidth.Add(147)

        arColumnName.Add("ViewID")
        arColumnName.Add("ViewName")
        arColumnName.Add("ScreenName")
        arColumnName.Add("RoleName")
        arColumnName.Add("CompanyName")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Add(50)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(150)

        arColumnName.Add("ViewID")
        arColumnName.Add("ViewName")
        arColumnName.Add("ScreenName")
        arColumnName.Add("RoleName")
        arColumnName.Add("CompanyName")

    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0
        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text
                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            If IsDate(strSearch) Then
                            Else
                                Exit Sub
                            End If
                        End If
                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
                        End If
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mTextBox(intI).Text.Trim
                        If strSearch.Contains("*") = True Then
                            strSearch = strSearch.Replace("*", "%")
                        Else
                            strSearch &= "%"
                        End If
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
                shF = 1
                GrdAddSerach.Columns.Clear()
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                rowvalue = 0
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                FormatGrid()
                GetColumns()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            rowvalue = 0
            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            GrdAddSerach.DataBind()
            FormatGrid()
            GetColumns()
            If mdvtable.Count = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Data not found according to your search string...")
                'cpnlErrorPanel.Visible = True
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("OW-Views", "BtnGrdSearch_Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String
        Dim viewid As String
        GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    viewid = GrdAddSerach.DataKeys(e.Item.ItemIndex)

                    strTempName = e.Item.Cells(1).Text
                    Select Case e.Item.Cells(2).Text
                        Case 799
                            e.Item.Cells(2).Text = "Call View"
                        Case 463
                            e.Item.Cells(2).Text = "Call/Task View"
                        Case 464
                            e.Item.Cells(2).Text = "Task View"
                        Case 502
                            e.Item.Cells(2).Text = "TDL View"
                        Case 229
                            e.Item.Cells(2).Text = "AB View"
                        Case 536
                            e.Item.Cells(2).Text = "Historic View"
                        Case 40
                            e.Item.Cells(2).Text = "SubCategory View"
                    End Select
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & strTempName & "','" & strTempName & "','" & rowvalue & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:AdminEditView('" & viewid & "', '" & strTempName & "','" & rowvalue & "')")
                End If
            Next
            rowvalue += 1
        Catch ex As Exception
            CreateLog("OW-Views", "GrdAddSearch_ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

    Private Sub ddlViewType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlViewType.SelectedIndexChanged
        Try
            For cnt As Integer = 0 To 4
                CType(Panel1.FindControl(arColumnName(cnt)), TextBox).Text = ""
            Next
            txtViewID.Value = ""
            txtViewN.Text = ""
            lstError.Items.Clear()
            'cpnlErrorPanel.Visible = False

        Catch ex As Exception
            CreateLog("OW-Views", "ddlViewType_SelectedIndexChanged-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
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
