Imports ION.Net
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_Machines
    Inherits System.Web.UI.Page
#Region " Variables "


    Public mintPageSize As Integer
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private arrSearchText As New ArrayList

    Private Shared dvSearch As New DataView
    Private mdvMachineMonitor As DataView
    Private intdomain As Integer
    Dim dvtemp As DataView
    Shared mintID As Integer
    Shared mintstatus As String
    Protected _currentPageNumber As Int32 = 1

#End Region

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
    'Public mintPageSize As Integer
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer

#End Region

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to fill domain dropdown acc to selected company
    '                   Table t170011
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetDomain()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            If SQL.Search("T170011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlDomain dropdown fill acc to company
                Ddldomain.DataSource = dsTemp.Tables(0)
                'domain Name
                Ddldomain.DataTextField = "DM_VC150_DomainName"
                'domainId
                Ddldomain.DataValueField = "DM_NU9_DID_PK"
                Ddldomain.DataBind()
                Ddldomain.Items.Insert(0, New ListItem("Select", "0"))
            Else
                'SQL.Search is False Msgpanel show no domain exist for  selected company
                lstError.Items.Clear()
                lstError.Items.Add("No domain avilable for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetDomain-206", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Sub BindGrid(ByVal DomainID As Integer)
        Dim dtemp As DataTable

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            GrdAddSerach.PageSize = mintPageSize ' set the grid page size
            ' This function will fetch data from t130022  against process and a company
            sqstr = "select MM_VC150_Machine_Name,MM_VC100_Machine_IP,MM_VC8_Machine_Type,MM_CH1_IsEnable,MM_VC20_Cat1,MM_VC20_Cat2,MM_VC20_Cat3,MM_VC150_Machine_Name from T170012 where  MM_NU9_DID_FK=" & DomainID

            If SQL.Search("T170012", "Machine", "BindGrid", sqstr, dsTemp, "", "") = True Then
                'if sql search is true then we bind grid
                'put value of dataset to dataview
                For inti As Integer = 0 To dsTemp.Tables(0).Rows.Count - 1
                    If dsTemp.Tables(0).Rows(inti).Item(3) = "E" Then
                        dsTemp.Tables(0).Rows(inti).Item(3) = "<refresh><img src='../Images/GreenAlert.gif' align='middle'></refresh>"
                    ElseIf dsTemp.Tables(0).Rows(inti).Item(3) = "D" Then
                        dsTemp.Tables(0).Rows(inti).Item(3) = "<refresh><img src='../Images/RedAlert.gif' align='middle'></refresh>"
                    End If

                Next
                dsTemp.AcceptChanges()
                mdvMachineMonitor = dsTemp.Tables("T170012").DefaultView
                dvtemp = dsTemp.Tables("T170012").DefaultView ' use for paging
                'filterdataview
                GetFilteredDataView(mdvMachineMonitor, GetRowFilter)
                'Datagrid fetch data from dataview
                GrdAddSerach.DataSource = mdvMachineMonitor.Table
                GetFilteredDataView(dvtemp, GetRowFilter)
                'Paging
                If (mintPageSize) * (GrdAddSerach.CurrentPageIndex) > dvtemp.Table.Rows.Count - 1 Then
                    GrdAddSerach.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                ''bind grid
                GrdAddSerach.DataBind()
            Else
                dtemp = dsTemp.Tables("T170012")
                dvtemp = dtemp.DefaultView
                'if sql search is false then dummy row of columns shown in datagrid
                mdvMachineMonitor = dsTemp.Tables("T170012").DefaultView
                GrdAddSerach.DataSource = dtemp
                GrdAddSerach.DataBind()

            End If

            Dim intRows As Integer = dvtemp.Table.Rows.Count
            'paging
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

        Catch ex As Exception
            CreateLog("BasicMonitoring", "bindDiskGrid-1019", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:      20/08/2007
    'Purpose:           This function is used  to get the text from search textboxes in                         an array list
    'Modify Date:       ------
    '***********************************************************************************
    Private Sub GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlMachine:GrdAddSerach:_ctl2:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("BasicMonitoring", "GetsearchText-1045", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:        20/08/2007
    'Purpose:           This function is used  to Gets the row filter string
    'Modify Date:       ------
    '***********************************************************************************
    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function
            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To mdvMachineMonitor.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If mdvMachineMonitor.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf mdvMachineMonitor.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
                        If IsNumeric(arrSearchText(inti)) = False Then
                            strRowFilter &= " " & arrColumnName(inti) & "=-101019 and"      'change row filter to non existing value
                        Else
                            strRowFilter &= " " & arrColumnName(inti) & "=" & arrSearchText(inti) & " and"
                        End If
                    Else
                        strRowFilter &= " " & arrColumnName(inti) & " like " & arrSearchText(inti) & " and"
                    End If
                End If
            Next

            If strRowFilter <> Nothing Then
                strRowFilter = strRowFilter.Replace("*", "%")
                If strRowFilter.Substring(strRowFilter.Length - 3, 3) = "and" Then
                    strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3)
                Else
                    strRowFilter = strRowFilter
                End If
            End If
            Return strRowFilter
        Catch ex As Exception
            CreateLog("BasicMonitoring", "GetRowFilter-1085", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Function

    Private Sub DefineGridColumnData()
        'define width of columns
        arrColWidth.Clear()

        arrColWidth.Add(0) 'Machine   
        arrColWidth.Add(110) 'Machine   
        arrColWidth.Add(110) 'start date
        arrColWidth.Add(110) 'End Date
        arrColWidth.Add(80) 'Time
        arrColWidth.Add(80) 'uid
        arrColWidth.Add(80) 'pwd
        arrColWidth.Add(80) 'alert


        arrTextboxName.Clear()
        arrTextboxName.Add("txtMachineName")
        arrTextboxName.Add("TxtMachineIP")
        arrTextboxName.Add("txtMachineType")
        arrTextboxName.Add("txtStatus")
        arrTextboxName.Add("txtCat1")
        arrTextboxName.Add("TxtCat2")
        arrTextboxName.Add("TxtCat3")


        arrColumnName.Clear()
        arrColumnName.Add("MM_VC150_Machine_Name") 'Drive
        arrColumnName.Add("MM_VC100_Machine_IP") 'Machine
        arrColumnName.Add("MM_VC8_Machine_Type") 'satrt date
        arrColumnName.Add("MM_CH1_IsEnable") 'End date
        arrColumnName.Add("MM_VC20_Cat1") 'Time
        arrColumnName.Add("MM_VC20_Cat2") 'UID
        arrColumnName.Add("MM_VC20_Cat3") 'PWD
        arrColumnName.Add("MM_VC150_Machine_Name") 'PWD

    End Sub

    Private Sub FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                GrdAddSerach.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                GrdAddSerach.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                GrdAddSerach.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("BasicMonitoring", "FormatGrid-331", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Sub Ddldomain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ddldomain.SelectedIndexChanged
        cpnlMachine.State = CustomControls.Web.PanelState.Expanded
        'cpnldatabase enabled fales
        cpnlMachine.Enabled = True
        BindGrid(Ddldomain.SelectedValue)
        FormatGrid()
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            txtCSS(Me.Page, "cpnlMachine")
            cpnlError.Visible = False
            ' javascript function added with controls check numeric value and decimal 
            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 

            '''paging
            '******************************************
            mintPageSize = Val(Request.Form("cpnlMachine:txtPageSize"))
            If mintPageSize = 0 Or mintPageSize < 0 Then
                mintPageSize = 5
            End If
            txtPageSize.Text = mintPageSize
            ''textbox objectname and reportdropdown list visible acc to selected value of object type
            '******************************  
            If Not IsPostBack Then
                'Session("DomainID") = ""
                GetDomain()
                CurrentPg.Text = _currentPageNumber.ToString()
                DefineGridColumnData()
                cpnlMachine.State = CustomControls.Web.PanelState.Collapsed
                'cpnldatabase enabled fales
                cpnlMachine.Enabled = False
            End If
            BindGrid(Ddldomain.SelectedValue)
            FormatGrid()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub GrdAddSerach_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                If IsNothing(mdvMachineMonitor) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To mdvMachineMonitor.Table.Columns.Count - 2
                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & ",'" & e.Item.Cells(0).Text & "'," & Ddldomain.SelectedValue.Trim & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                    Next
                End If
            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then

                        CType(txt, TextBox).Text = Request.Form(" cpnlMachine:GrdAddSerach:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "dgrBGDailyMonitor_ItemDataBound-501", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        GrdAddSerach.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindGrid(Ddldomain.SelectedValue)

    End Sub

    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (GrdAddSerach.CurrentPageIndex > 0) Then
            GrdAddSerach.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindGrid(Ddldomain.SelectedValue)

    End Sub

    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (GrdAddSerach.CurrentPageIndex < (GrdAddSerach.PageCount - 1)) Then
            GrdAddSerach.CurrentPageIndex += 1

            If GrdAddSerach.PageCount = CurrentPg.Text Then
                CurrentPg.Text = GrdAddSerach.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindGrid(Ddldomain.SelectedValue)

    End Sub

    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        GrdAddSerach.CurrentPageIndex = (GrdAddSerach.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindGrid(Ddldomain.SelectedValue)
    End Sub
End Class
