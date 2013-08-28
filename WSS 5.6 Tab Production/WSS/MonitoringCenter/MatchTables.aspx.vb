Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Partial Class MonitoringCenter_MatchTables
    Inherits System.Web.UI.Page
    Private dvSearch As New DataView
    Private Shared mintSeqID As Integer
    Private mstrStatus As String
    Private mintReqID As Integer
    Private arrSearchText As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Dim bytFlag As Byte
    'Protected WithEvents dgrMatchTable1 As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    'Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Imagebutton2 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Imagebutton3 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Imagebutton4 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Button6 As System.Web.UI.WebControls.Button
    'Protected WithEvents Panel2 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    'Protected WithEvents btnGST As System.Web.UI.WebControls.Button
    Private Shared arrColumnName As New ArrayList
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mintSeqID = Request.QueryString("ID")

        mintReqID = Request.QueryString("ReqID")
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Save"
                    If SaveTbData() = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Record saved successfully...")
                        ShowMsgPenel(cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgOK)


                    End If
                Case "Close"
                    Response.Redirect("MatchTableSearch.aspx", False)


            End Select

        End If


        If IsPostBack = False Then

            GetEnvironment()
            DefineGridColumnData()

            If mintSeqID <> -1 Then
                Dim blnStatus As Boolean
                Dim sqRDR As SqlClient.SqlDataReader

                sqRDR = SQL.Search("RPA", "Load", "select RQ_CH2_STATUS , RQ_NU9_REQUEST_ID from T130022 where RQ_NU9_SQID_PK=" & mintSeqID, SQL.CommandBehaviour.SingleRow, blnStatus)
                If blnStatus = True Then
                    While sqRDR.Read
                        mintReqID = Val(sqRDR("RQ_NU9_REQUEST_ID"))
                        mstrStatus = CType(sqRDR("RQ_CH2_STATUS"), String).Trim
                    End While
                    sqRDR.Close()
                    Filldata()
                End If
            Else
                mstrStatus = "Add"
            End If
            BINDGrid()
            FormatGrid()
        Else
            ReadGrid()
        End If
    End Sub

    Private Function BINDGrid() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim dttemp As New DataTable
            Dim sqstr As String
            'this function fetch data from t1300041 against a company
            sqstr = "select  RS_VC150_CAT5,RS_VC150_CAT6, RS_VC150_CAT7,RS_VC150_CAT8,RS_VC150_CAT9,rs_nu9_sqid_fk,RS_VC150_CAT5  from T130023  where   RS_NU9_REQUESTID_FK=" & mintReqID

            If SQL.Search("T130023 ", "ReportEntry", "BINDGrid", sqstr, dsTemp, "", "") = True Then

                dvSearch = dsTemp.Tables(0).DefaultView
                ModifyDataView()
                GetFilteredDataView(dvSearch, GetRowFilter)

                'filter dataview
                dgrMatchTable1.DataSource = dvSearch.Table
                dgrMatchTable1.DataBind()
            Else
                dttemp = dsTemp.Tables(0)
                dvSearch = dttemp.DefaultView
                dgrMatchTable1.DataSource = dttemp
                dgrMatchTable1.DataBind()

                'Dim drTemp As DataRow
                'For inti As Integer = 0 To arrColumnName.Count - 1
                '    dttemp.Columns.Add(arrColumnName(inti))
                'Next
                'drTemp = dttemp.NewRow
                'For intI As Integer = 0 To dttemp.Columns.Count - 1
                '    drTemp.Item(inti) = ""
                'Next
                'dttemp.Rows.Add(drTemp)
                'dttemp.AcceptChanges()
                'dvSearch = dttemp.DefaultView ' use for paging
                'dgrEnvDetail.DataSource = dttemp

                'CurrentPg.Text = 0
                ''''End If
                'dgrEnvDetail.DataBind()

            End If

            'Dim intRows As Integer = dvSearch.Table.Rows.Count

            'Dim _totalPages As Double = 1
            'Dim _totalrecords As Int32
            'If Not Page.IsPostBack Then
            '    _totalrecords = intRows
            '    _totalPages = _totalrecords / mintPageSize
            '    TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
            'Else
            '    _totalrecords = intRows
            '    If CurrentPg.Text = 0 And _totalrecords > 0 Then
            '        CurrentPg.Text = 1
            '    End If
            '    If _totalrecords = 0 Then
            '        CurrentPg.Text = 0
            '    End If
            '    _totalPages = _totalrecords / mintPageSize
            '    TotalPages.Text = (System.Math.Ceiling(_totalPages)).ToString()
            '    _totalPages = Double.Parse(TotalPages.Text)
            'End If

        Catch ex As Exception
            CreateLog("ReportEntry", "BindGrid-369", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

    Private Function DefineGridColumnData()
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(0)

        arrColWidth.Add(20)
        arrColWidth.Add(180) 'COLNAME
        arrColWidth.Add(40) 'KEY
        arrColWidth.Add(100) 'DATATYPE
        arrColWidth.Add(40) ' SIZE
        arrColWidth.Add(70) ' DBNULL

        arrTextboxName.Clear()
        arrTextboxName.Add("txtColName")
        arrTextboxName.Add("txtKey")
        arrTextboxName.Add("txtData")
        arrTextboxName.Add("txtSize")
        arrTextboxName.Add("txtDBNULL")

        arrColumnName.Clear()
        arrColumnName.Add("RS_VC150_CAT5") 'Domain
        arrColumnName.Add("RS_VC150_CAT6") 'Submit REport
        arrColumnName.Add("RS_VC150_CAT7") 'Machine Name
        arrColumnName.Add("RS_VC150_CAT8") 'Machine UID
        arrColumnName.Add("RS_VC150_CAT9") 'Machine PWD

        arrColumnName.Add("rs_nu9_sqid_fk")
        arrColumnName.Add("RS_VC150_CAT5")
    End Function

    Private Function Filldata() As Boolean
        Dim sqrdr As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqrdr = SQL.Search("ProcessEnvironmentEntry", "Load", "select * from T130022 where RQ_NU9_SQID_PK=" & mintSeqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                While sqrdr.Read
                    GetEnvironment()

                    If sqrdr.Item("RQ_vc150_cat2") = "GST" Then
                        ddlENV1.SelectedValue = sqrdr("RQ_VC150_CAT3")
                        ddlEnv2.SelectedValue = sqrdr("RQ_VC150_CAT5")
                        txtTableName.Text = sqrdr("RQ_VC150_CAT4")

                    Else

                        Dim strEnv As String = sqrdr("RQ_VC150_CAT3")
                        Dim strEnv1() As String = Split(strEnv, ",")
                        'For intj As Integer = 0 To strEnv1.Length - 1
                        ddlENV1.SelectedValue = strEnv1(0)
                        ddlEnv2.SelectedValue = strEnv1(1) 'sqrdr("RQ_VC150_CAT5")
                        txtTableName.Text = sqrdr("RQ_VC150_CAT4")
                        'Next
                    End If


                End While
                sqrdr.Close()
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenel(cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgError)
            CreateLog("ProcessEnvEntry", "Load-111", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Private Function SaveTBEntry() As Boolean
        Try
            Dim Multi As SQL.AddMultipleRows
            Dim strDomainName As String
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020019'"
            'reader
            sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold domainname
                strDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
                'hold machineCode
                intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
                sqRDR.Close()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Domain is available for this process...")
                ShowMsgPenel(cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            Dim intMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            Dim intReqMax As Integer = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_REQUEST_ID) from T130022", "")
            intReqMax += 1


            'define column name
            arColName.Add("RQ_NU9_SQID_PK")
            arColName.Add("RQ_NU9_PROCESSID")
            arColName.Add("RQ_VC150_CAT1") 'Env
            arColName.Add("RQ_VC150_CAT2") 'Type
            arColName.Add("RQ_VC150_CAT3") 'Env1
            arColName.Add("RQ_VC150_CAT4") 'Table name
            arColName.Add("RQ_VC150_CAT5") 'env2
            arColName.Add("RQ_VC100_REQUEST_DATE") 'request date

            arColName.Add("RQ_NU9_Domain_FK") 'domain
            arColName.Add("RQ_NU9_Machine_Code_FK") 'Machine
            arColName.Add("RQ_CH2_STATUS") 'Status
            arColName.Add("RQ_NU9_REQUEST_ID") 'RequestID

            arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

            arRowData.Add(intMax)
            arRowData.Add("10020019")
            arRowData.Add("ENV")
            arRowData.Add("GST")
            arRowData.Add(ddlENV1.SelectedItem.Text)
            arRowData.Add(txtTableName.Text)
            arRowData.Add(ddlEnv2.SelectedItem.Text)
            arRowData.Add(Now)
            arRowData.Add(strDomainName)
            arRowData.Add(intMachineCode)
            arRowData.Add("P")
            arRowData.Add(intReqMax)

            arRowData.Add(Session("PropCAComp"))
            'define and save data when Object Typeis RPT
            If SQL.Save("T130022", "ENV", "SaveRecord", arColName, arRowData, "") = True Then
                Return True
            Else
                Return False
            End If

            Return True
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "SaveBGRequest-862", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
            Return False
        End Try

    End Function

    Private Sub Datagrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Try
            Dim itemType As ListItemType = e.Item.ItemType

            If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
                Return
            Else
                Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)

                e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
            End If

        Catch ex As Exception
            CreateLog("BGDailyMonitor", "dgrBGDailyMonitor_ItemDataBound-501", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub

    Private Function ReadGrid()
        Try
            Dim gridrow As DataGridItem
            Dim arSQId As New ArrayList
            For Each gridrow In dgrMatchTable1.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked = True Then
                    'add Sqid of checked Cheeckboxes
                    arSQId.Add(gridrow.Cells(1).Text)
                End If
            Next
            For cnt As Integer = 0 To arSQId.Count - 1
                lstColName.Items.Add(arSQId.Item(cnt))
            Next
            For Each gridrow In dgrMatchTable1.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    CType(gridrow.FindControl("chkReq1"), CheckBox).Checked = False
                    CType(gridrow.FindControl("chkReq1"), CheckBox).Enabled = False
                End If
            Next
        Catch ex As Exception
        End Try
    End Function

    Private Function Checkedbox()
        Try
            Dim gridrow As DataGridItem
            Dim arSQId As New ArrayList
            For Each gridrow In dgrMatchTable1.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked = True Then
                    'add Sqid of checked Cheeckboxes
                    arSQId.Add(gridrow.Cells(1).Text)
                End If
            Next
            For cnt As Integer = 0 To arSQId.Count - 1
                lstColName.Items.Add(arSQId.Item(cnt))
            Next
            For Each gridrow In dgrMatchTable1.Items
                'check which row is chcked in Grid
                If CType(gridrow.FindControl("chkReq1"), CheckBox).Checked Then
                    CType(gridrow.FindControl("chkReq1"), CheckBox).Checked = False
                    CType(gridrow.FindControl("chkReq1"), CheckBox).Enabled = False
                End If
            Next
        Catch ex As Exception
        End Try
    End Function

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ReadGrid()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim strcolumanname As New ArrayList

        For inti As Integer = 0 To lstColName.Items.Count - 1
            If lstColName.Items(inti).Selected = True Then
                strcolumanname.Add(lstColName.Items(inti).Text)
            End If
        Next

        For intj As Integer = 0 To strcolumanname.Count - 1
            lstColName.Items.Remove(strcolumanname(intj))
        Next

        Dim gridrow As DataGridItem

        For Each gridrow In dgrMatchTable1.Items
            For cnt As Integer = 0 To strcolumanname.Count - 1
                If (gridrow.Cells(1).Text) = strcolumanname(cnt) Then
                    CType(gridrow.FindControl("chkReq1"), CheckBox).Enabled = True
                End If
            Next
        Next
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        lstColName.Items.Add(txtcolumanName.Text)
        txtcolumanName.Text = ""
    End Sub

    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                dgrMatchTable1.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrMatchTable1.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                dgrMatchTable1.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("ReportEntry", "FormatGrid-899", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    Private Function GetEnvironment()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch  ENV against a company
            sqstr = "select name from udc where udctype='PENV' "
            If SQL.Search("udc ", "ReportEntry", "GetEnvironment", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlENV dropdown fill acc to selected company
                ddlENV1.DataSource = dsTemp.Tables(0)
                'Env Name
                ddlENV1.DataTextField = "name"
                ddlENV1.DataBind()

                ddlEnv2.DataSource = dsTemp.Tables(0)
                'Env Name
                ddlEnv2.DataTextField = "name"
                ddlEnv2.DataBind()
            Else
                'SQL.Search is False ddlENV dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Env  avilable for selected Company")
                ShowMsgPenel(cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetEnvironment-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    Private Function ModifyDataView()
        Try

            For inti As Integer = 0 To dvSearch.Table.Rows.Count - 1

                Dim strNullValue As String = dvSearch.Table.Rows(inti).Item("RS_VC150_CAT9")


                Select Case strNullValue.Trim.ToUpper
                    Case "FALSE"
                        dvSearch.Table.Rows(inti).Item("RS_VC150_CAT9") = "Not Null"
                    Case "TRUE"
                        dvSearch.Table.Rows(inti).Item("RS_VC150_CAT9") = "Null"

                End Select
            Next
            dvSearch.Table.AcceptChanges()
        Catch ex As Exception
            CreateLog("MatchTable", "ModifyDataView-230", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    Private Sub dgrMatchTable1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrMatchTable1.ItemDataBound
        Try

            If (e.Item.ItemType = ListItemType.Item Or ListItemType.AlternatingItem) And e.Item.ItemType <> ListItemType.Header Then

                'e.Item.Attributes.Add("ondblclick", "KeyCheck55(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                If IsNothing(dvSearch) = True Then
                    Exit Sub
                Else
                    For inti As Integer = 0 To dvSearch.Table.Columns.Count - 2
                        'If CType(e.Item.Cells(4).FindControl("lblDBValue"), Label).Text.Trim <> "True" Then

                        '    'checkbox enabled is false
                        '    CType(e.Item.FindControl("chkbox1"), CheckBox).Enabled = False
                        '    CType(e.Item.FindControl("chkbox1"), CheckBox).Checked = True
                        'Else
                        '    'checkbox enabled is True
                        '    CType(e.Item.FindControl("chkbox1"), CheckBox).Enabled = False
                        '    CType(e.Item.FindControl("chkbox1"), CheckBox).Checked = False
                        'End If
                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "GridClick(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                    Next
                End If
            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlEnvEntry:dgrEnvDetail:_ctl2:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "dgrBGDailyMonitor_ItemDataBound-798", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Sub

    Private Function GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                strSearch = Request.Form("cpnlEnvEntry:dgrMatchTable1:_ctl1:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    strSearch = GetSearchString(strSearch)
                End If
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("RPA", "GetSeacrhText-160", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Function GetRowFilter() As String
        Try
            GetSeacrhText()
            Dim strRowFilter As String

            For inti As Integer = 0 To dvSearch.Table.Columns.Count - 2
                If arrSearchText(inti) <> "" Then
                    If dvSearch.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                    ElseIf dvSearch.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("RPA", "GetRowFilter-195", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Function SaveTbData() As Boolean
        Try
            'If ValidateRequest() = False Then
            '    Exit Function
            'End If
            Dim intMax As Integer
            Dim intReqMax As Integer
            Dim strenv As String

            Dim sbColumnName As New System.Text.StringBuilder("")
            For inti As Integer = 0 To lstColName.Items.Count - 1
                sbColumnName.Append(lstColName.Items(inti).Text & ",")

            Next
            If sbColumnName.Length = 0 Then
                sbColumnName.Append("")
            Else
                sbColumnName.Remove(sbColumnName.Length - 1, 1)
            End If

            Dim Multi As SQL.AddMultipleRows
            Dim strDomainName As String
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020019'"
            'reader
            sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold domainname
                strDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
                'hold machineCode
                intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
                sqRDR.Close()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Domain is available for this process...")
                ShowMsgPenel(cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            intMax = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_SQID_PK) from T130022", "")
            intMax += 1

            If mintSeqID <> -1 Then
                'intMax = mintSeqID
                intReqMax = mintReqID
            Else
                intReqMax = SQL.Search("BGDailyMonitor", "SaveBGRequest", "select max(RQ_NU9_REQUEST_ID) from T130022", "")
                intReqMax += 1
            End If
            strenv = ddlENV1.SelectedValue & ","
            'define column name
            arColName.Add("RQ_NU9_SQID_PK")
            arColName.Add("RQ_NU9_PROCESSID")
            arColName.Add("RQ_VC150_CAT1") 'Env
            arColName.Add("RQ_VC150_CAT2") 'Type
            arColName.Add("RQ_VC150_CAT3") 'Env1
            arColName.Add("RQ_VC150_CAT4") 'Env2
            arColName.Add("RQ_VC150_CAT5") 'Table Name
            arColName.Add("RQ_VC150_CAT6") 'Table Name
            arColName.Add("RQ_VC100_REQUEST_DATE") 'request date

            arColName.Add("RQ_NU9_Domain_FK") 'domain
            arColName.Add("RQ_NU9_Machine_Code_FK") 'Machine
            arColName.Add("RQ_CH2_STATUS") 'Status
            arColName.Add("RQ_NU9_REQUEST_ID") 'RequestID

            arColName.Add("RQ_NU9_ClientID_FK") 'Client ID

            arRowData.Add(intMax)
            arRowData.Add("10020019")
            arRowData.Add("ENV")
            arRowData.Add("GDT")
            arRowData.Add(ddlENV1.SelectedItem.Text)
            arRowData.Add(ddlEnv2.SelectedItem.Text)
            arRowData.Add(txtTableName.Text)
            arRowData.Add(sbColumnName.ToString)
            arRowData.Add(Now)
            arRowData.Add(strDomainName)
            arRowData.Add(intMachineCode)
            arRowData.Add("P")
            arRowData.Add(intReqMax)

            arRowData.Add(Session("PropCAComp"))
            'define and save data when Object Typeis RPT


            'add Multiple rows in t130022
            If SQL.Save("T130022", "ENV", "SaveRecord", arColName, arRowData, "") = True Then
                Return True
            Else
                Return False
            End If

            Return True
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "SaveBGRequest-862", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
            Return False
        End Try

    End Function

    Private Sub btnGST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGST.Click
        If SaveTBEntry() = True Then
            lstError.Items.Clear()
            lstError.Items.Add("Record saved successfully...")
            ShowMsgPenel(cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgOK)
            'cleartextboxes()
        End If
    End Sub
End Class
