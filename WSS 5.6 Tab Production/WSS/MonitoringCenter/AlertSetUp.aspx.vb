Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_AlertSetUp
    Inherits System.Web.UI.Page
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
    Dim rowvalue As Integer
    Dim shF As Short
    Dim flg As Short
    Public mintPageSize As Integer
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    ' Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer

    'for alert flow
    Dim AlertarColumnName As New ArrayList
    Private Shared AlertarOriginalColumnName As New ArrayList
    Private Shared AlertarSetColumnName As New ArrayList
    Private Shared AlertarColWidth As New ArrayList
    Private Shared AlertarColumns As ArrayList = New ArrayList
    Private Shared AlertarCol As ArrayList = New ArrayList
    Private Shared Alertarrtextvalue As ArrayList = New ArrayList
    Dim Alertmdvtable As New DataView
    Private Shared AlertmTextBox() As TextBox


#End Region

    Private mintAlertID As Integer
    Private mintSelectedIndex As Integer
#Region "Page Load Code"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        mintAlertID = txthiddenAlertID.Value
        mintSelectedIndex = txthiddenSV.Value

        Call DefineColumnData()



        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        txtCSS(Me.Page, "cpnlAlerFlow")


        '--------Status------------
        CDDLStatus.CDDLUDC = True
        CDDLStatus.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        CDDLStatus.CDDLQuery = "select name ID, Description, company  from UDC where UDCType=""ALST"""
        CDDLStatus.CDDLType = CustomDDL.DDLType.FastEntry
        '''''''''''''''''''''''''''''''''''''''''''''''

        ''--------Template------------

        CDDLTempl.CDDLUDC = False
        CDDLTempl.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        CDDLTempl.CDDLQuery = "select TL_NU9_ID_PK ID, TL_VC30_Template Name, TL_VC200_Desc Description from T050011 where TL_VC8_Tmpl_Type =""" & DDLTemplType.SelectedValue & """"
        CDDLTempl.CDDLType = CustomDDL.DDLType.FastEntry
        CDDLTempl.CDDLFillDropDown(, False)
        '''''''''''''''''''''''''''''''''''''''''''''''


        If IsPostBack = False Then
            cpnlViewJobs.Text = "Alerts [" & WSSSearch.SearchCompNameID(HttpContext.Current.Session("PropCAComp")).ExtraValue & "]"
            DDLTemplType.Enabled = False
            CDDLStatus.CDDLFillDropDown(, True)
        Else
            CDDLTempl.CDDLSetItem()
            CDDLStatus.CDDLSetItem()
        End If

        cpnlAlerFlow.State = CustomControls.Web.PanelState.Collapsed
        cpnlAlerFlow.Enabled = False
        cpnlAlerFlow.TitleCSS = "test2"
        cpnlError.Visible = False

        Dim srtQueryWhere As String
        Dim chkand As Short
        chkand = 0

        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenValue = Request.Form("txthidden")
        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SAddressNumber") = Request.Form("txthidden")
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        Response.Redirect("Configuration.aspx")
                    Case "Delete"
                        'If DeleteAlert() = True Then

                        'End If
                    Case "Save"
                        If SaveAlertFlow() = False Then
                            'Exit Sub
                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is unable to process your request Please try later...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                CreateLog("AlertSetUp", "Load-154", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If

        If Not IsPostBack Then

            Try
                Dim dsDefault As New DataSet

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                'SQL.DBTable = "T180011"
                SQL.DBTracing = False

                Dim strSelect As String = "select "

                Dim strItem As String
                For inti As Integer = 0 To arOriginalColumnName.Count - 1
                    strSelect &= arOriginalColumnName.Item(inti) & ","
                Next
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                strSelect &= " from T180011"
                If srtQueryWhere <> "" Then
                    strSelect = strSelect & " where " + srtQueryWhere
                End If
                'SQL.DBTable = "T180011"
                If SQL.Search("T180011", "", "", strSelect, dsDefault, "", "") = True Then

                    'change the datagrid header columns name 
                    For inti As Integer = 0 To dsDefault.Tables("T180011").Columns.Count - 1
                        dsDefault.Tables("T180011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                End If
                mdvtable.Table = dsDefault.Tables("T180011")
                If mdvtable.Count < 1 Then
                    cpnlViewJobs.State = CustomControls.Web.PanelState.Collapsed
                    cpnlViewJobs.Enabled = False
                End If
                'End If
                '**************

                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                GrdAddSerach.DataBind()
                FormatGrid(arColumnName)

                'create textbox at run time at head of the datagrid        

                CreateTextBox()
            Catch ex As Exception
                CreateLog("AlertSetUp", "Load-219", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else
            cpnlViewJobs.State = CustomControls.Web.PanelState.Expanded
            cpnlViewJobs.Enabled = True
            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear()
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlViewJobs:" & arSetColumnName.Item(i)))
            Next


            '''''''''''''''''
            'for alert flow grid

            AlertarOriginalColumnName.Clear()
            AlertarOriginalColumnName.Add("AF_NU9_LNID")
            AlertarOriginalColumnName.Add("AF_VC8_Type")
            AlertarOriginalColumnName.Add("AF_VC50_COM")
            AlertarOriginalColumnName.Add("AF_VC12_Template_Type")
            AlertarOriginalColumnName.Add("AF_NU9_Template_ID_FK")
            AlertarOriginalColumnName.Add("AF_VC8_Status")

            AlertarSetColumnName.Clear()
            AlertarSetColumnName.Add("LineID")
            AlertarSetColumnName.Add("AlType")
            AlertarSetColumnName.Add("ComMode")
            AlertarSetColumnName.Add("TemplateType")
            AlertarSetColumnName.Add("Template")
            AlertarSetColumnName.Add("Status")

            AlertarColumnName.Clear()
            AlertarColumnName.Add("LineID")
            AlertarColumnName.Add("AlType")
            AlertarColumnName.Add("ComMode")
            AlertarColumnName.Add("TemplateType")
            AlertarColumnName.Add("Template")
            AlertarColumnName.Add("Status")

            AlertarColWidth.Clear()
            AlertarColWidth.Add(40)
            AlertarColWidth.Add(50)
            AlertarColWidth.Add(270)
            AlertarColWidth.Add(80)
            AlertarColWidth.Add(120)
            AlertarColWidth.Add(50)



            'If SaveAlertFlow() = False Then
            '    'Exit Sub
            'End If


            Alertarrtextvalue.Clear()
            For i As Integer = 0 To AlertarColumns.Count - 1
                Alertarrtextvalue.Add(Request.Form("cpnlAlerFlow:" & AlertarColumnName.Item(i)))
            Next


            '**************************************
            'fill data in datagrid on load on post back event
            FillView()

            FillAlertFlow()




            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage

                        Case "Search"
                            Call BtnGrdSearch_Click(Me, New EventArgs)

                    End Select
                Catch ex As Exception
                    CreateLog("AlertSetup", "Load-242", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try
            End If

        End If

        If ChechkValidityforSearch(arrtextvalue) = True Then

            ' Call BtnGrdSearch_Click(Me, New EventArgs)

            BtnGrdSearch_Click(Me, New System.EventArgs)

        End If
        'Security Block

        'Dim intId As Integer

        'If Not IsPostBack Then
        '    Dim str As String
        '    str = HttpContext.Current.Session("PropRootDir")
        '    intId = Request.QueryString("ScrID")
        '    Dim obj As New clsSecurityCache
        '    If obj.ScreenAccess(intId) = False Then
        '        Response.Redirect("../../frm_NoAccess.aspx")
        '    End If
        '    obj.ControlSecurity(Me.Page, intId)
        'End If

        ''End of Security Block
        GrdAddSerach.SelectedIndex = IIf(mintSelectedIndex = -1, mintSelectedIndex, mintSelectedIndex - 1)
    End Sub

#End Region

#Region "fill View"

    Private Function FillView()
        Dim intViewID As Integer = 0
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim arcolName As New ArrayList

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String


            Dim dsFromView As New DataSet



            Dim strItem As String
            For inti As Integer = 0 To arOriginalColumnName.Count - 1
                strSelect &= arOriginalColumnName.Item(inti) & ","
            Next

            strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
            'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
            strSelect &= " from T180011"

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


            'SQL.DBTable = "T180011"

            If SQL.Search("T180011", "AlertSetUp", "FillView", strSelect, dsFromView, "", "") = True Then
                ' DataGrid1.DataSource = dsFromView
                'dsFromView.Tables(0).Columns(0).ColumnName = ""
                For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                    dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                Next

                mdvtable.Table = dsFromView.Tables(0)
                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.Columns.Clear()

                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If

                GrdAddSerach.DataBind()
                '  DataGrid1.Visible = True
                ' ReSizeGrid()
                FormatGrid(arColumnName)

                CreateTextBox()
            Else
            End If
            'Else
            '    Exit Function
            'End If
        Catch ex As Exception
            CreateLog("AlertSetUp", "Fillview-351", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

#End Region

    Private Function FillAlertFlow()

        Dim intViewID As Integer = 0
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim arcolName As New ArrayList

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String


            Dim dsFromView As New DataSet



            Dim strItem As String
            For inti As Integer = 0 To AlertarOriginalColumnName.Count - 1
                strSelect &= AlertarOriginalColumnName.Item(inti) & ","
            Next

            strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
            strSelect &= " from T180012 where AF_NU9_CompID_FK=" & Session("PropCAComp") & " and AF_NU9_AID_FK=" & mintAlertID

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


            ' SQL.DBTable = "T180012"

            If SQL.Search("T180012", "AlertSeup", "FillAlertFlow", strSelect, dsFromView, "", "") = True Then
                ' DataGrid1.DataSource = dsFromView
                'dsFromView.Tables(0).Columns(0).ColumnName = ""
                For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                    dsFromView.Tables(0).Columns(inti).ColumnName = AlertarColumnName.Item(inti)
                Next

                Alertmdvtable.Table = dsFromView.Tables(0)
                GrdAlertFlow.DataSource = Alertmdvtable.Table
                GrdAlertFlow.Columns.Clear()

                If GrdAlertFlow.AutoGenerateColumns = False Then
                    GrdAlertFlow.AutoGenerateColumns = True
                End If

                GrdAlertFlow.DataBind()
                '  DataGrid1.Visible = True
                ' ReSizeGrid()
                AlertFormatGrid(AlertarColumnName)
                AlertCreateTextBox()
                cpnlAlerFlow.TitleCSS = "test"
                cpnlAlerFlow.Enabled = True
                cpnlAlerFlow.State = CustomControls.Web.PanelState.Expanded

            Else

                Dim dtTemp As New DataTable
                Dim dtRow As DataRow
                dtTemp.Columns.Add("StatusName")
                dtTemp.Columns.Add("Description")
                dtTemp.Columns.Add("Company")
                dtTemp.Columns.Add("ScreenID")
                dtTemp.Columns.Add("Code")
                dtRow = dtTemp.NewRow
                dtRow.Item(0) = ""
                GrdAlertFlow.DataSource = dtTemp
                GrdAlertFlow.DataBind()
                cpnlAlerFlow.TitleCSS = "test"
                cpnlAlerFlow.Enabled = True
                cpnlAlerFlow.State = CustomControls.Web.PanelState.Expanded

                FormatHeader()

            End If
            'Else
            '    Exit Function
            'End If
        Catch ex As Exception
            CreateLog("AlertSetUp", "Fillview-351", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

    Private Function FormatHeader()
        AlertarColWidth.Clear()
        AlertarColWidth.Add(40)
        AlertarColWidth.Add(50)
        AlertarColWidth.Add(270)
        AlertarColWidth.Add(80)
        AlertarColWidth.Add(120)
        AlertarColWidth.Add(50)
        For inti As Integer = 0 To AlertarColWidth.Count - 1
            GrdAlertFlow.Columns(inti).HeaderStyle.Width = Unit.Point(AlertarColWidth.Item(inti))
        Next

    End Function

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Function CreateTextBox()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arColumns.Clear()

        'fill the columns count into the array from mdvtable view
        intCol = mdvtable.Table.Columns.Count

        If Not IsPostBack Then
            ReDim mTextBox(intCol)
        End If

        Try

            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    'arSetColumnName.Item(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = arSetColumnName.Item(intii)
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
                    strcolid = arSetColumnName(intii)
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("AlertSetUp", "CreateTextBox-516", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function


    Private Function AlertCreateTextBox()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        AlertarColumns.Clear()

        'fill the columns count into the array from mdvtable view
        intCol = Alertmdvtable.Table.Columns.Count


        ReDim AlertmTextBox(intCol)


        Try
            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(AlertarColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"
                    'arCol.Add(arOriginalColumnName.Item(intii))
                    AlertarCol.Add(AlertarSetColumnName.Item(intii))
                    Panel2.Controls.Add(Page.ParseControl("<asp:TextBox id=" & AlertarSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox></asp:TextBox>"))
                    _textbox.ID = AlertarSetColumnName.Item(intii)
                    _textbox.Text = ""
                    AlertmTextBox(intii) = _textbox
                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(AlertarColWidth.Item(intii))
                    col1cng = col1.Value + 1
                    col1cng = col1cng & "pt"

                    If Alertarrtextvalue.Count <> Alertmdvtable.Table.Columns.Count Then
                        _textbox.Text = ""
                    Else
                        _textbox.Text = Alertarrtextvalue.Item(intii)
                    End If

                    '_textbox.Text = ""
                    strcolid = AlertarSetColumnName.Item(intii)
                    Panel2.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox ></asp:TextBox>"))
                    _textbox.ID = Alertmdvtable.Table.Columns(intii).ColumnName

                    AlertmTextBox(intii) = _textbox
                End If
                mshFlag = 1
                AlertarColumns.Add(_textbox.ID)

            Next
        Catch ex As Exception
            CreateLog("AlertSetUp", "CreateTextBox-516", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function



#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid(ByVal ColumnName As ArrayList)
        Dim intI As Integer

        Try
            GrdAddSerach.AutoGenerateColumns = False
            ' chkgridwidth()

            For intI = 0 To ColumnName.Count - 1
                Dim Bound_Column As New BoundColumn
                If ColumnName.Item(intI) = "AlertID" Then

                    Dim strWidth As String = arColWidth.Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True
                    Bound_Column.Visible = False
                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    GrdAddSerach.Columns.Add(Bound_Column)
                Else

                    Dim strWidth As String = arColWidth.Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True

                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    GrdAddSerach.Columns.Add(Bound_Column)

                End If
            Next
            ColumnName.Clear()

        Catch ex As Exception
            CreateLog("AlertSetUp", "FormatGrid-623", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Sub AlertFormatGrid(ByVal AlertColumnName As ArrayList)
        Dim intI As Integer

        Try
            GrdAlertFlow.AutoGenerateColumns = False
            ' chkgridwidth()

            For intI = 0 To AlertColumnName.Count - 1
                Dim Bound_Column As New BoundColumn


                Dim strWidth As String = AlertarColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                GrdAlertFlow.Columns.Add(Bound_Column)


            Next
            AlertColumnName.Clear()

        Catch ex As Exception
            CreateLog("AlertSetUp", "FormatGrid-623", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub


#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        'If SaveAlertFlow() = False Then
        '    'Exit Sub
        'End If

        arOriginalColumnName.Clear()
        arOriginalColumnName.Add("AM_NU9_AID_PK")
        arOriginalColumnName.Add("AM_VC6_TYPE")
        arOriginalColumnName.Add("AM_VC20_Code")
        arOriginalColumnName.Add("AM_VC150_SUB")
        arOriginalColumnName.Add("AM_VC500_DESC")

        arSetColumnName.Clear()
        arSetColumnName.Add("AlertID")
        arSetColumnName.Add("AlertType")
        arSetColumnName.Add("AlertCode")
        arSetColumnName.Add("AlertSubject")
        arSetColumnName.Add("AlertDescription")

        arColumnName.Clear()
        arColumnName.Add("AlertID")
        arColumnName.Add("AlertType")
        arColumnName.Add("AlertCode")
        arColumnName.Add("AlertSubject")
        arColumnName.Add("AlertDescription")


        arColWidth.Clear()
        arColWidth.Add(0)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(150)
        arColWidth.Add(256)



        '''''''''''''''''
        'for alert flow grid

        AlertarOriginalColumnName.Clear()
        AlertarOriginalColumnName.Add("AF_NU9_LNID")
        AlertarOriginalColumnName.Add("AF_VC8_Type")
        AlertarOriginalColumnName.Add("AF_VC50_COM")
        AlertarOriginalColumnName.Add("AF_VC12_Template_Type")
        AlertarOriginalColumnName.Add("AF_NU9_Template_ID_FK")
        AlertarOriginalColumnName.Add("AF_VC8_Status")

        AlertarSetColumnName.Clear()
        AlertarSetColumnName.Add("LineID")
        AlertarSetColumnName.Add("AlType")
        AlertarSetColumnName.Add("ComMode")
        AlertarSetColumnName.Add("TemplateType")
        AlertarSetColumnName.Add("Template")
        AlertarSetColumnName.Add("Status")

        AlertarColumnName.Clear()
        AlertarColumnName.Add("LineID")
        AlertarColumnName.Add("AlType")
        AlertarColumnName.Add("ComMode")
        AlertarColumnName.Add("TemplateType")
        AlertarColumnName.Add("Template")
        AlertarColumnName.Add("Status")

        AlertarColWidth.Clear()
        AlertarColWidth.Add(40)
        AlertarColWidth.Add(50)
        AlertarColWidth.Add(270)
        AlertarColWidth.Add(80)
        AlertarColWidth.Add(120)
        AlertarColWidth.Add(50)




        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text

                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"

                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/2005"
                            End If
                        End If

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
                            strSearch = strSearch.Replace("*", "")
                            If IsNumeric(strSearch) = False Then
                                strSearch = "-101"
                            End If
                        End If
                        ' mdvTable.Table.Columns(intI).DataType.FullName = System.DateTime
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mTextBox(intI).Text.Trim
                        strSearch = GetSearchString(strSearch)

                        strSearch = strSearch.Replace("*", "%")
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
                FormatGrid(arColumnName)

                'for alert
                SearchalertFlow()

                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.AutoGenerateColumns = True
            GrdAddSerach.DataBind()

            FormatGrid(arColumnName)



            ''''for search
            SearchalertFlow()

        Catch ex As Exception
            CreateLog("AlertSetUp", "Click-757", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub
#End Region


    Function SearchalertFlow()
        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = AlertmTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To AlertarColumns.Count - 1
                If Not AlertmTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = AlertmTextBox(intI).Text

                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"

                    If (Alertmdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (Alertmdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (Alertmdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (Alertmdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) Then
                            Else
                                'LblErrMsg.Text = " Check Your Date Format First"
                                Exit Function
                            End If
                        End If

                        If (Alertmdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then

                        End If
                        ' mdvTable.Table.Columns(intI).DataType.FullName = System.DateTime
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & Alertmdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = AlertmTextBox(intI).Text.Trim
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & Alertmdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
                shF = 1
                GrdAlertFlow.Columns.Clear()
                If GrdAlertFlow.AutoGenerateColumns = False Then
                    GrdAlertFlow.AutoGenerateColumns = True
                End If

                GrdAlertFlow.DataSource = Alertmdvtable
                GrdAlertFlow.DataBind()
                AlertFormatGrid(AlertarColumnName)
                Exit Function
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            Alertmdvtable.RowFilter = strRowFilterString

            GrdAlertFlow.Columns.Clear()
            GrdAlertFlow.DataSource = Alertmdvtable
            GrdAlertFlow.AutoGenerateColumns = True
            GrdAlertFlow.DataBind()

            AlertFormatGrid(AlertarColumnName)

        Catch ex As Exception
            CreateLog("AlertSetUp", "SearchalertFlow-757", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try


    End Function


#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        'If UpdateAlert() = True Then
        '    Call RefreshGrid()
        'End If
        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String

        GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strTempName = e.Item.Cells(1).Text

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "','" & strTempName & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "', '" & strTempName & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("AlertSetUp", "ItemDataBound-787", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try
    End Sub

#End Region

    Private Function SaveAlertFlow() As Boolean
        Try

            If ValidateAlert() = True Then
                Dim arCol As New ArrayList
                Dim arRow As New ArrayList

                arCol.Add("AF_NU9_AID_FK")
                arCol.Add("AF_NU9_LNID")
                arCol.Add("AF_VC8_Type")
                arCol.Add("AF_VC50_COM")
                arCol.Add("AF_VC8_Status")
                arCol.Add("AF_VC10_User")
                arCol.Add("AF_NU9_Template_ID_FK")
                arCol.Add("AF_VC12_Template_Type")
                arCol.Add("AF_NU9_CompID_FK")
                arCol.Add("AF_NU9_ABNUM")

                arRow.Add(mintAlertID)
                Dim intNext As Integer
                intNext = SQL.Search("", "", "select max(AF_NU9_LNID) from T180012 where AF_NU9_AID_FK=" & mintAlertID)
                intNext = intNext + 1
                arRow.Add(intNext)
                arRow.Add(DDLAlertActionType.SelectedValue)
                arRow.Add(txtCom.Text)
                arRow.Add(CDDLStatus.CDDLGetValue)
                arRow.Add(Session("PropUserName"))
                arRow.Add(IIf(CDDLTempl.CDDLGetValue = "", System.DBNull.Value, CDDLTempl.CDDLGetValue))
                arRow.Add(DDLTemplType.SelectedValue)
                arRow.Add(Session("PropCAComp"))
                arRow.Add(Session("PropUserID"))

                If SQL.Save("T180012", "AlertSetUp", "SaveAlertFlow", arCol, arRow) = True Then
                    Call ClearFE()
                    lstError.Items.Clear()
                    lstError.Items.Add("Record Saved successfully...")
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("SErver is busy Please Try Later...")
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy Please Try Later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

    Private Function ValidateAlert() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        If DDLAlertActionType.SelectedValue = "EML" Then
            If txtCom.Text.Equals("") Then
                lstError.Items.Add("Email Communication Field cannot be blank...")
                shFlag = 1
            End If
            If CDDLStatus.CDDLGetValue = "" Then
                lstError.Items.Add("Status cannot be blank...")
                shFlag = 1
            End If

        ElseIf DDLAlertActionType.SelectedValue = "WSS" Then
            If CDDLTempl.CDDLGetValue = "" Then
                lstError.Items.Add("Template Name cannot be blank...")
                shFlag = 1
            Else
                If DDLTemplType.SelectedValue.Equals("") Then
                    lstError.Items.Add("Select Template Type...")
                    shFlag = 1
                End If
            End If
            If CDDLStatus.CDDLGetValue = "" Then
                lstError.Items.Add("Status cannot be blank...")
                shFlag = 1
            End If
        Else
            Return False
        End If



        If shFlag = 1 Then
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function



    Private Function ClearFE()
        If DDLAlertActionType.SelectedValue = "EML" Then
            txtCom.Text = ""
        ElseIf DDLAlertActionType.SelectedValue = "WSS" Then
            DDLTemplType.SelectedIndex = 0
            CDDLTempl.CDDLSetSelectedItem("", False, "")
        End If
        CDDLStatus.CDDLSetSelectedItem("", True, "")
        DDLAlertActionType.SelectedIndex = 0

    End Function

    Private Function CheckFE() As Boolean
        If txtCom.Text.Equals("") Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function DeleteAlert() As Boolean
        Dim intAlertID As Integer = Request.Form("txthidden")
        Try

            If SQL.Delete("AlertSetUp", "DeleteAlert", "delete from T180011 where AM_NU9_AID_PK=" & intAlertID, SQL.Transaction.Serializable) = True Then
                'cpnlError.Visible = True
                'cpnlError.Text = "Message"
                lstError.Items.Clear()
                lstError.Items.Add("Record Deleted successfully...")
                'ImgError.ImageUrl = "../images/Pok.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Return True
            Else
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                'ImgError.ImageUrl = "../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                Return False
            End If

        Catch ex As Exception
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            lstError.Items.Clear()
            'lstError.Items.Add(ex.Message)
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

#Region "Refresh Grid After Saving Fast Entry"
    Private Function RefreshGrid()
        Dim intViewID As Integer = 0
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim arcolName As New ArrayList

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String


            Dim dsFromView As New DataSet



            Dim strItem As String
            For inti As Integer = 0 To arOriginalColumnName.Count - 1
                strSelect &= arOriginalColumnName.Item(inti) & ","
            Next

            strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
            'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
            strSelect &= " from T180011"

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


            'SQL.DBTable = "T180011"

            If SQL.Search("T180011", "AlertSetup", "RefreshGrid", strSelect, dsFromView, "", "") = True Then
                ' DataGrid1.DataSource = dsFromView
                'dsFromView.Tables(0).Columns(0).ColumnName = ""
                For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                    dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                Next

                mdvtable.Table = dsFromView.Tables(0)
                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.Columns.Clear()

                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If

                GrdAddSerach.DataBind()
                '  DataGrid1.Visible = True
                ' ReSizeGrid()
                FormatGrid(arColumnName)

                '  CreateTextBox()
            Else
            End If
            'Else
            '    Exit Function
            'End If
        Catch ex As Exception
            CreateLog("AlertSetUp", "RefreshGrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function
#End Region

    Private Sub GrdAlertFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAlertFlow.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim strLineID = GrdAlertFlow.DataKeys(e.Item.ItemIndex)
            Dim strTempName = e.Item.Cells(1).Text

            e.Item.Attributes.Add("style", "cursor:hand")
            e.Item.Attributes.Add("onclick", "javascript:KeyCheckAF('" & strLineID & "', '" & e.Item.ItemIndex + 1 & "','" & strTempName & "')")
            e.Item.Attributes.Add("onDBlclick", "javascript:KeyCheck55AF(" & strLineID & ", '" & e.Item.ItemIndex + 1 & "', '" & strTempName & "')")
        End If


    End Sub

    Private Sub DDLAlertActionType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLAlertActionType.SelectedIndexChanged
        If DDLAlertActionType.SelectedValue = "EML" Then
            DDLTemplType.Enabled = False
            DDLTemplType.SelectedIndex = 0
            CDDLTempl.CDDLSetSelectedItem("", False, "")
            CDDLTempl.Enabled = False
            txtCom.Enabled = True
        Else
            DDLTemplType.Enabled = True
            CDDLTempl.Enabled = True
            txtCom.Enabled = False
            txtCom.Text = ""

        End If
    End Sub

    Private Sub DDLTemplType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTemplType.SelectedIndexChanged
        ''--------Template------------

        CDDLTempl.CDDLUDC = False
        CDDLTempl.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        CDDLTempl.CDDLQuery = "select TL_NU9_ID_PK ID, TL_VC30_Template Name, TL_VC200_Desc Description from T050011 where TL_VC8_Tmpl_Type =""" & DDLTemplType.SelectedValue & """"
        CDDLTempl.CDDLType = CustomDDL.DDLType.FastEntry
        CDDLTempl.CDDLFillDropDown(, False)
        '''''''''''''''''''''''''''''''''''''''''''''''
    End Sub
    Private Function DefineColumnData()
        arOriginalColumnName.Clear()
        arOriginalColumnName.Add("AM_NU9_AID_PK")
        arOriginalColumnName.Add("AM_VC6_TYPE")
        arOriginalColumnName.Add("AM_VC20_Code")
        arOriginalColumnName.Add("AM_VC150_SUB")
        arOriginalColumnName.Add("AM_VC500_DESC")

        arSetColumnName.Clear()
        arSetColumnName.Add("AlertID")
        arSetColumnName.Add("AlertType")
        arSetColumnName.Add("AlertCode")
        arSetColumnName.Add("AlertSubject")
        arSetColumnName.Add("AlertDescription")

        arColumnName.Clear()
        arColumnName.Add("AlertID")
        arColumnName.Add("AlertType")
        arColumnName.Add("AlertCode")
        arColumnName.Add("AlertSubject")
        arColumnName.Add("AlertDescription")

        arColWidth.Clear()
        arColWidth.Add(0)
        arColWidth.Add(100)
        arColWidth.Add(100)
        arColWidth.Add(150)
        arColWidth.Add(256)


        '''''''''''''''''
        'for alert flow grid

        AlertarOriginalColumnName.Clear()
        AlertarOriginalColumnName.Add("AF_NU9_LNID")
        AlertarOriginalColumnName.Add("AF_VC8_Type")
        AlertarOriginalColumnName.Add("AF_VC50_COM")
        AlertarOriginalColumnName.Add("AF_VC12_Template_Type")
        AlertarOriginalColumnName.Add("AF_NU9_Template_ID_FK")
        AlertarOriginalColumnName.Add("AF_VC8_Status")

        AlertarSetColumnName.Clear()
        AlertarSetColumnName.Add("LineID")
        AlertarSetColumnName.Add("AlType")
        AlertarSetColumnName.Add("ComMode")
        AlertarSetColumnName.Add("TemplateType")
        AlertarSetColumnName.Add("Template")
        AlertarSetColumnName.Add("Status")

        AlertarColumnName.Clear()
        AlertarColumnName.Add("LineID")
        AlertarColumnName.Add("AlType")
        AlertarColumnName.Add("ComMode")
        AlertarColumnName.Add("TemplateType")
        AlertarColumnName.Add("Template")
        AlertarColumnName.Add("Status")

        AlertarColWidth.Clear()
        AlertarColWidth.Add(40)
        AlertarColWidth.Add(50)
        AlertarColWidth.Add(270)
        AlertarColWidth.Add(80)
        AlertarColWidth.Add(120)
        AlertarColWidth.Add(50)


    End Function
End Class
