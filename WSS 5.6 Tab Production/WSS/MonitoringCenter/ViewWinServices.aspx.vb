Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports ION.Net
Imports System.Web
Imports System
Imports System.Security.Principal
Imports System.Runtime.InteropServices
Imports System.Data
Imports System.ServiceProcess

Partial Class MonitoringCenter_ViewWinServices
    Inherits System.Web.UI.Page
    Dim LOGON32_LOGON_INTERACTIVE As Integer = 2
    Dim LOGON32_PROVIDER_DEFAULT As Integer = 0
    'Protected WithEvents lblTitleLabelWinSvcStatus As System.Web.UI.WebControls.Label



    Dim impersonationContext As WindowsImpersonationContext

    Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String, _
                            ByVal lpszDomain As String, _
                            ByVal lpszPassword As String, _
                            ByVal dwLogonType As Integer, _
                            ByVal dwLogonProvider As Integer, _
                            ByRef phToken As IntPtr) As Integer

    Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
                            ByVal ExistingTokenHandle As IntPtr, _
                            ByVal ImpersonationLevel As Integer, _
                            ByRef DuplicateTokenHandle As IntPtr) As Integer

    Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long

    'Enumerations for the Requests 

    Enum RequestID
        Start = 1
        SStop = 2
        Pause = 3
        Refresh = 4
    End Enum

    Enum RequestStatus
        Pending = 0
        Done = 1
        Errors = 2
    End Enum

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
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared intCol As Integer

#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        cpnlErrorPanel.Visible = False
        Dim arColName As New ArrayList
        Dim arRowData As New ArrayList
        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenValue = Request.Form("txthidden")

        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SAddressNumber") = Request.Form("txthidden")
        End If

        arColName.Add("SC_VC100_ServiceName")
        arColName.Add("SC_IN4_Request_Status")
        arColName.Add("SC_IN4_Inserted_By_FK")
        arColName.Add("SC_DT8_Inserted_On")
        arColName.Add("SC_VC15_Inserted_By_System_Code")
        arColName.Add("SC_IN4_Request_ID")

        Dim bytServiceExists As Byte

        If txthiddenImage <> "" Then
            Try
                arRowData.Add(Request.Form("txthidden"))
                arRowData.Add(RequestStatus.Pending)
                arRowData.Add(Session("PropUserID"))
                arRowData.Add(Now)
                arRowData.Add(Network.GetIPAddress("", "", Network.GetMachineName("", "")))

                If impersonateValidUser("asaluja", "IONSOFTNET", "Ionuser12345") Then

                    Dim myController As New System.ServiceProcess.ServiceController
                    myController.MachineName = "10.10.10.30"

                    Select Case txthiddenImage

                        Case "Stop"
                            myController.ServiceName = Request.Form("txthidden")
                            myController.Stop()
                            lstError.Items.Clear()
                            lstError.Items.Add("The service has been stopped...")
                            'cpnlErrorPanel.Visible = True
                            'cpnlErrorPanel.Text = "Confirmation Message"
                            'ImgError.ImageUrl = "../images/Pok.gif"
                            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgInfo)
                            'arRowData.Add(RequestID.SStop)
                        Case "Start"
                            myController.ServiceName = Request.Form("txthidden")
                            myController.Start()
                            lstError.Items.Clear()
                            lstError.Items.Add("The service has been started...")
                            'cpnlErrorPanel.Visible = True
                            'cpnlErrorPanel.Text = "Confirmation Message"
                            'ImgError.ImageUrl = "../images/Pok.gif"
                            ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgOK)
                            'arRowData.Add(RequestID.Start)
                        Case "Pause"
                            arRowData.Add(RequestID.Pause)
                        Case "Refresh"
                            arRowData.Add(RequestID.Refresh)
                        Case "Logout"
                            LogoutWSS()
                    End Select

                Else
                    ' You do not have permissions.
                End If

                'bytServiceExists = searchData(Request.Form("txthidden"), "T150010")
                'If (bytServiceExists = 1) Then
                '    mstGetFunctionValue = updateData(arColName, arRowData, "T150010")
                '    lstError.Items.Clear()
                '    lstError.Items.Add("Your request has been generated.")
                '    cpnlErrorPanel.Visible = True
                '    cpnlErrorPanel.Text = "Confirmation Message"
                '    ImgError.ImageUrl = "../images/Pok.gif"
                'ElseIf (bytServiceExists = 0) Then
                '    mstGetFunctionValue = insertData(arColName, arRowData, "T150010")
                '    lstError.Items.Clear()
                '    lstError.Items.Add("Your request has been generated.")
                '    cpnlErrorPanel.Visible = True
                '    cpnlErrorPanel.Text = "Confirmation Message"
                '    ImgError.ImageUrl = "../images/Pok.gif"
                'Else
                '    lstError.Items.Clear()
                '    lstError.Items.Add("A critical error occured during the transaction")
                '    cpnlErrorPanel.Visible = True
                '    cpnlErrorPanel.Text = "Error Message"
                '    ImgError.ImageUrl = "../images/error_image.gif"
                'End If

            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                'cpnlErrorPanel.Visible = True
                'cpnlErrorPanel.Text = "Error Message"
                'ImgError.ImageUrl = "../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
                CreateLog("ViewJobs", "Load-174", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If

        Dim objShell
        Dim strDomain
        objShell = CreateObject("Wscript.Shell")
        strDomain = objShell.RegRead("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\DefaultDomainName")
        If impersonateValidUser("asaluja", "IONSOFTNET", "Ionuser12345") Then
            '      If impersonateValidUser("jkaushal", strDomain, "userIon123") Then

            If Not IsPostBack Then

                chkgridwidth()

                Dim dt As New DataTable

                Try
                    dt.TableName = "Services"
                    dt.Columns.Add("ID")
                    dt.Columns.Add("ServiceName")
                    dt.Columns.Add("DisplayName")
                    'dt.Columns.Add("CanPause")
                    'dt.Columns.Add("CanShutDown")
                    dt.Columns.Add("CanStop")
                    dt.Columns.Add("MachineName")
                    dt.Columns.Add("Status")

                    Dim dr As DataRow
                    Try
                        Dim myAllServices() As ServiceController
                        Dim myController As New ServiceController

                        Dim SCPA As ServiceControllerPermissionAccess = New ServiceControllerPermissionAccess
                        SCPA = ServiceControllerPermissionAccess.Control
                        Dim permission As ServiceControllerPermission = New ServiceControllerPermission(System.Security.Permissions.PermissionState.Unrestricted)

                        'myController.MachineName = Network.GetIPAddress("", "", Network.GetMachineName("", ""))
                        'myAllServices = myController.GetServices(Network.GetIPAddress("", "", Network.GetMachineName("", "")))
                        myController.MachineName = "10.10.10.30"
                        myAllServices = ServiceController.GetServices("10.10.10.30")
                        For inti As Int16 = 0 To myController.GetServices.Length - 1
                            dr = dt.NewRow
                            dr.Item(0) = inti + 1
                            dr.Item(1) = myAllServices(inti).ServiceName
                            dr.Item(2) = myAllServices(inti).DisplayName()
                            'dr.Item(3) = myAllServices(inti).CanPauseAndContinue
                            ' dr.Item(4) = myAllServices(inti).CanShutdown
                            dr.Item(3) = myAllServices(inti).CanStop

                            If (myAllServices(inti).MachineName = ".") Then
                                dr.Item(4) = myController.MachineName
                            Else
                                dr.Item(4) = myAllServices(inti).MachineName
                            End If

                            dr.Item(5) = myAllServices(inti).Status
                            dt.Rows.Add(dr)
                        Next

                    Catch ex As Exception
                        lstError.Items.Clear()
                        lstError.Items.Add("Server is busy please try later...")
                        ShowMsgPenel(cpnlErrorPanel, lstError, ImgError, mdlMain.MSG.msgError)
                        CreateLog("ViewJobs", "Load-174", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                    End Try

                    mdvtable.Table = dt

                    '**************

                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                    GrdAddSerach.DataBind()
                    FormatGrid()
                    GetColumns()

                    'create textbox at run time at head of the datagrid        

                    CreateTextBox()

                Catch ex As Exception

                    CreateLog("ViewWinServices", "Load-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try
            Else

                ' fill the textboxes value into the array 
                '**********************************
                arrtextvalue.Clear()
                For i As Integer = 0 To arColumns.Count - 1
                    arrtextvalue.Add(Request.Form("cpnlViewJobs:" & arCol.Item(i)))
                Next
                '**************************************
                'fill data in datagrid on load on post back event
                FillView()

                If txthiddenImage <> "" Then
                    Try
                        Select Case txthiddenImage

                            Case "Search"
                                Call BtnGrdSearch_Click(Me, New EventArgs)
                        End Select

                        Call BtnGrdSearch_Click(Me, New EventArgs)

                    Catch ex As Exception
                        CreateLog("ViewWinServices", "Load-262", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                    End Try

                End If

            End If

        Else
            'Your impersonation failed. Therefore, include a fail-safe mechanism here.
        End If
        ''Security Block

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
    End Sub

#End Region

    Function insertData(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            txtCSS(Me.Page)
            SQL.DBConnection = strConnection
            ' Table name
            ' SQL.DBTable = TableName
            SQL.DBTracing = False

            If SQL.Save(TableName, "ViewWinServices", "insertData", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Error occured while saving records"
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("ViewWinServices", "InsertData-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Function updateData(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            '  SQL.DBTable = TableName
            SQL.DBTracing = False

            If SQL.Update("ViewWinServices", "updataData", "update T150010 set SC_IN4_Request_ID = " & RowData(5) & " , SC_IN4_Request_Status = 0, SC_IN4_Inserted_By_FK = " & RowData(2) & " , SC_DT8_Inserted_On = '" & Now & "', SC_VC15_Inserted_By_System_Code = '" & RowData(4) & "' where SC_VC100_ServiceName = '" & RowData(0) & "'", SQL.Transaction.ReadCommitted) = True Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("ViewWinServices", "updateData-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function


    Function searchData(ByVal serviceName As String, ByVal TableName As String) As Byte

        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            '  SQL.DBTable = TableName
            SQL.DBTracing = False
            Dim intrCount As Integer

            If SQL.Search("ViewWinService", "SearchData", "Select SC_VC100_ServiceName from T150010 where SC_VC100_ServiceName = '" & serviceName & "'", intrCount) = True Then
                If (intrCount > 0) Then
                    Return 1
                Else
                    Return 0
                End If
            Else
                Return 0
            End If


        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("ViewWinServices", "SearchData-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return 2
        End Try

    End Function


#Region "fill View"

    Private Function FillView()
        Try
            Dim dt As New DataTable
            dt.TableName = "Services"
            dt.Columns.Add("ID")
            dt.Columns.Add("ServiceName")
            dt.Columns.Add("DisplayName")
            'dt.Columns.Add("CanPause")
            'dt.Columns.Add("CanShutDown")
            dt.Columns.Add("CanStop")
            dt.Columns.Add("MachineName")
            dt.Columns.Add("Status")

            Dim dr As DataRow

            Try
                Dim myAllServices() As ServiceController
                Dim myController As New ServiceController

                myController.MachineName = "10.10.10.30"
                myAllServices = myController.GetServices

                For inti As Int16 = 0 To myController.GetServices.Length - 1
                    dr = dt.NewRow
                    dr.Item(0) = inti + 1
                    dr.Item(1) = myAllServices(inti).ServiceName
                    dr.Item(2) = myAllServices(inti).DisplayName()
                    'dr.Item(3) = myAllServices(inti).CanPauseAndContinue
                    ' dr.Item(4) = myAllServices(inti).CanShutdown
                    dr.Item(3) = myAllServices(inti).CanStop
                    If (myAllServices(inti).MachineName = ".") Then
                        dr.Item(4) = "10.10.10.30"
                    Else
                        dr.Item(4) = myAllServices(inti).MachineName
                    End If


                    dr.Item(5) = myAllServices(inti).Status
                    dt.Rows.Add(dr)
                Next

            Catch ex As Exception
                CreateLog("ViewWinServices", "FillView-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            End Try

            mdvtable.Table = dt

            GrdAddSerach.DataSource = mdvtable.Table
            GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
            GrdAddSerach.DataBind()
            FormatGrid()
            GetColumns()

            'create textbox at run time at head of the datagrid        

            CreateTextBox()
        Catch ex As Exception
            CreateLog("ViewWinServices", "FillView-371", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

#End Region

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
                    arCol.Add(arCol.Item(intii))
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
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
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("ViewWinServices", "CreateTextBox-536", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(50)
        arColWidth.Add(150)
        arColWidth.Add(150)
        'arColWidth.Add(60)
        'arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(60)

        Try
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
            CreateLog("ViewWinServices", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()
        arColWidth.Add(50)
        arColWidth.Add(150)
        arColWidth.Add(150)
        'arColWidth.Add(60)
        'arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(60)


        arColumnName.Add("ID")
        arColumnName.Add("ServiceName")
        arColumnName.Add("DisplayName")
        'arColumnName.Add("CanPause")
        'arColumnName.Add("CanShutDown")
        arColumnName.Add("CanStop")
        arColumnName.Add("MachineName")
        arColumnName.Add("Status")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Add(50)
        arColWidth.Add(150)
        arColWidth.Add(150)
        'arColWidth.Add(60)
        'arColWidth.Add(80)
        arColWidth.Add(60)
        arColWidth.Add(80)
        arColWidth.Add(60)


        arCol.Add("ID")
        arCol.Add("ServiceName")
        arCol.Add("DisplayName")
        'arCol.Add("CanPause")
        'arCol.Add("CanShutDown")
        arCol.Add("CanStop")
        arCol.Add("MachineName")
        arCol.Add("Status")

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

                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"

                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
                            End If
                        End If

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
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
            GrdAddSerach.AutoGenerateColumns = True
            GrdAddSerach.DataBind()
            FormatGrid()
            GetColumns()
        Catch ex As Exception
            CreateLog("ViewWinServices", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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

        GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    'strTempName = e.Item.Cells(1).Text

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "','" & strTempName & "')")
                    'e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "', '" & strTempName & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("ViewWinServices", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

    Function impersonateValidUser(ByVal userName As String, _
    ByVal domain As String, ByVal password As String) As Boolean

        Try
            Dim tempWindowsIdentity As WindowsIdentity
            Dim token As IntPtr = IntPtr.Zero
            Dim tokenDuplicate As IntPtr = IntPtr.Zero
            impersonateValidUser = False

            If RevertToSelf() Then
                If LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                    If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                        tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                        impersonationContext = tempWindowsIdentity.Impersonate()
                        If Not impersonationContext Is Nothing Then
                            impersonateValidUser = True
                        End If
                    End If
                End If
            End If

            If Not tokenDuplicate.Equals(IntPtr.Zero) Then
                CloseHandle(tokenDuplicate)
            End If
            If Not token.Equals(IntPtr.Zero) Then
                CloseHandle(token)
            End If

        Catch ex As Exception
            CreateLog("ViewWinServices", "impersonateValidUser-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Function

    Private Sub undoImpersonation()
        impersonationContext.Undo()
    End Sub

End Class
