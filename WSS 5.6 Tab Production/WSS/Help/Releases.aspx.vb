Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data
Partial Class Help_Releases
    Inherits System.Web.UI.Page
    Private Shared dvSearch As New DataView
    Public mintPageSize As Integer
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private arrSearchText As New ArrayList
    Dim dvtemp As DataView
    Private mdvUpdation As DataView
    Public mintID As String
    Dim txthiddenImage As String 'Stored clicked button's cation  
    Protected _currentPageNumber As Int32 = 1

    Private Sub GetModule()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = " Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name, ROD_VC50_Alias_Name as AName, OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL,  OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType,  ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH,  OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName,convert(varchar,'False') Booked  from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "'  AND  UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND  RA.RA_VC4_Status_Code = 'ENB' AND  RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND  ROM.ROM_VC50_Status_Code_FK = 'ENB' and((OBM.OBM_VC4_Object_Type_FK ='MNU'  ) and (ROD.ROD_CH1_View_Hide <> 'H')) and ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND  OBM.OBM_VC4_Status_Code_FK = 'ENB' AND  OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and  ROM_IN4_Role_ID_PK =  " & HttpContext.Current.Session("PropRole") & "  order by OBM.OBM_SI2_Order_By  "
            If SQL.Search("T070011 ", "Releases", "GetModule", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlModule dropdown fill acc to User
                ddlModule.DataSource = dsTemp.Tables(0)

                'Machine Name
                ddlModule.DataTextField = "Name"
                ddlModule.DataValueField = "ObjectID"
                ddlModule.DataBind()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Module  avilable for Login User")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("Releases", "GetModule", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Sub
    Private Sub GetReleaseType()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = " Select Name, Description from UDc where UDCType='UPDT'  "
            If SQL.Search("UDC ", "Release", "GetReleaseType", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlModule dropdown fill acc to User
                ddlType.DataSource = dsTemp.Tables(0)
                'Machine Name
                ddlType.DataTextField = "Name"
                ddlType.DataValueField = "Description"
                ddlType.DataBind()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Releases Type  avilable ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("Releases", "GetReleaseType", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Sub

    Private Function SAVERecord() As Boolean

        If ValidateRequest() = False Then
            Exit Function
        End If

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            Dim intMax As Integer = SQL.Search("Releases", "SAVERecord", "Select isnull(Max(RE_NU9_ID_PK),0) from t070062", "")
            intMax += 1

            arColName.Add("RE_NU9_ID_PK")
            arColName.Add("RE_VC50_ModName")
            arColName.Add("RE_NU9_ModID")
            arColName.Add("RE_VC250_Subject")
            arColName.Add("RE_VC8000_Desc")
            arColName.Add("RE_VC30_Date")
            arColName.Add("RE_VC50_Type")

            arRowData.Add(intMax)
            arRowData.Add(ddlModule.SelectedItem.Text)
            arRowData.Add(ddlModule.SelectedValue)
            arRowData.Add(txtSubject.Text)
            arRowData.Add(txtDesc.Text)
            arRowData.Add(dtDate.Text)
            arRowData.Add(ddlType.SelectedValue)


            If SQL.Save("t070062", "Releases", "SaveRecord", arColName, arRowData, "") = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Releases", "SAVERecord", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function

    Private Function UpdateRecord(ByVal mintID As Integer) As Boolean

        If ValidateRequest() = False Then
            Exit Function
        End If

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList


            arColName.Add("RE_VC50_ModName")
            arColName.Add("RE_NU9_ModID")
            arColName.Add("RE_VC250_Subject")
            arColName.Add("RE_VC8000_Desc")
            arColName.Add("RE_VC30_Date")
            arColName.Add("RE_VC50_Type")


            arRowData.Add(ddlModule.SelectedItem.Text)
            arRowData.Add(ddlModule.SelectedValue)
            arRowData.Add(txtSubject.Text)
            arRowData.Add(txtDesc.Text)
            arRowData.Add(dtDate.Text)
            arRowData.Add(ddlType.SelectedValue)


            If SQL.Update("T070062", "Releases", "UpdateRecord-203", "select * from T070062 where RE_NU9_ID_PK=" & mintID & "", arColName, arRowData) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Releases", "UpdateRecord", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try

    End Function
    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        'if anytext box empty then shFlag set to 1

        If ddlModule.SelectedValue.Equals("0") Then
            lstError.Items.Add("Module Cannot be blank ...")
            shFlag = 1
        End If

        If txtSubject.Text.Equals("") Then
            lstError.Items.Add("Subject  cannot be blank...")
            shFlag = 1
        End If

        If txtDesc.Text.Equals("") Then
            lstError.Items.Add("Description  cannot be blank...")
            shFlag = 1
        End If


        If dtDate.Text.Equals("") Then
            lstError.Items.Add("Date  cannot be blank...")
            shFlag = 1

        End If

        If shFlag = 1 Then
            'shgFlag=1 then msg panel show msg 
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        Else
            'shgFlag=0 then all fields are fill 
            Return True
        End If

    End Function
    Private Sub cleartextBoxes()
        txtSubject.Text = ""
        txtDesc.Text = ""
        dtDate.Text = ""
        ddlModule.SelectedIndex = 0
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load



        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")


        'paging
        '******************************************
        mintPageSize = Val(Request.Form("cpnlUpdation:txtPageSize"))
        If mintPageSize = 0 Or mintPageSize < 0 Then
            mintPageSize = 18
        End If
        txtPageSize.Text = mintPageSize

        If IsPostBack = False Then
            txtCSS(Me.Page)
            GetModule()
            GetReleaseType()
            CurrentPg.Text = _currentPageNumber.ToString()
            DefineGridColumnData()
        End If
        txthiddenImage = Request.Form("txthiddenImage")


        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Save"
                    If ViewState("ID") <> "" Then
                        If UpdateRecord(ViewState("ID")) = True Then
                            BindGrid()
                            lstError.Items.Add("Records Updated Successfully")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        End If
                    Else
                        If SAVERecord() = True Then
                            cleartextBoxes()
                            lstError.Items.Add("Records Saved Successfully")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        End If
                    End If
                Case "Edit"
                    mintID = Request.Form("txtID")
                    ViewState("ID") = mintID
                    Fillview(mintID)
                Case "Add"
                    cleartextBoxes()
                    ViewState("ID") = ""
            End Select
        End If
        BindGrid()
        FormatGrid()
    End Sub

    Private Sub BindGrid()
        Dim dtemp As DataTable
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            DgReleses.PageSize = mintPageSize ' set the grid page size
            ' This function will fetch data from t130022  against process and a company

            sqstr = "select RE_VC50_ModName as ModName,RE_VC30_Date as RelDate,RE_VC50_Type as RelType,RE_VC250_Subject as Sub,RE_VC8000_Desc as Description,RE_NU9_ID_PK as ID from T070062 "

            If SQL.Search("T070062", "Releases", "BindDiskGrid", sqstr, dsTemp, "", "") = True Then
                'if sql search is true then we bind grid
                'put value of dataset to dataview
                mdvUpdation = dsTemp.Tables("T070062").DefaultView
                dvtemp = dsTemp.Tables("T070062").DefaultView ' use for paging
                'filterdataview
                Dim htDescCols As New Hashtable
                htDescCols.Add("Description", 55)
                htDescCols.Add("Sub", 30)

                HTMLEncodeDecode(mdlMain.Action.Encode, mdvUpdation, htDescCols)

                GetFilteredDataView(mdvUpdation, GetRowFilter)
                'Datagrid fetch data from dataview
                DgReleses.DataSource = mdvUpdation.Table
                GetFilteredDataView(dvtemp, GetRowFilter)

                HTMLEncodeDecode(mdlMain.Action.Encode, dvtemp, htDescCols)
                'Paging
                If (mintPageSize) * (DgReleses.CurrentPageIndex) > dvtemp.Table.Rows.Count - 1 Then
                    DgReleses.CurrentPageIndex = 0
                    CurrentPg.Text = 1
                End If
                'bind grid
                DgReleses.DataBind()
            Else
                dtemp = dsTemp.Tables("T070062")
                dvtemp = dtemp.DefaultView
                'if sql search is false then dummy row of columns shown in datagrid
                'mdvDiskMonitor = dsTemp.Tables("T130022").DefaultView
                DgReleses.DataSource = dtemp
                DgReleses.DataBind()

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
            CreateLog("Releases", "bindDiskGrid-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function
            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To mdvUpdation.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If mdvUpdation.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf mdvUpdation.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
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
            CreateLog("Releases", "GetRowFilter-361", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Function
    Private Sub GetSeacrhText()
        Try
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                'get value of search text box
                strSearch = Request.Form("cpnlUpdation:DgReleses:_ctl1:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("Releases", "GetSeacrhText-380", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Private Sub DefineGridColumnData()
        'define width of columns
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(80) 'Module
        arrColWidth.Add(90) 'Date   
        arrColWidth.Add(110) '	Updation Type
        arrColWidth.Add(200) '	Subject
        arrColWidth.Add(300) '	Description

        arrTextboxName.Clear()
        arrTextboxName.Add("txtModule")
        arrTextboxName.Add("txtRelDate")
        arrTextboxName.Add("txtRelType")
        arrTextboxName.Add("txtSub")
        arrTextboxName.Add("txtDesc1")


        arrColumnName.Clear()
        arrColumnName.Add("ModName") 'ModName
        arrColumnName.Add("RelDate") 'RelDate
        arrColumnName.Add("RelType") 'RelType
        arrColumnName.Add("Sub") 'Sub
        arrColumnName.Add("Desc") 'Desc   
        arrColumnName.Add("ID") 'ID

    End Sub
    Private Sub FormatGrid()
        Try
            For inti As Integer = 1 To arrColWidth.Count - 1
                DgReleses.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                DgReleses.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                DgReleses.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("Releases", "FormatGrid-421", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub
    Private Function Fillview(ByVal mintID As Integer)
        Dim blnStatus As Boolean
        Dim sqRDR As SqlClient.SqlDataReader
        Try
            'Dim strSQL = "select * from T130022 where RQ_NU9_SQID_PK=" & mintSQID
            sqRDR = SQL.Search("BGdailyMonitorEdit", "Monitoredit", "select RE_VC50_ModName as ModName,RE_VC30_Date as RelDate,RE_VC50_Type as RelType,RE_VC250_Subject as Sub,RE_VC8000_Desc as Description from T070062 where RE_NU9_ID_PK =" & mintID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            'if blnStatus True 
            If blnStatus = True Then
                While sqRDR.Read
                    txtSubject.Text = IIf(IsDBNull(sqRDR("Sub")), "", sqRDR("Sub"))
                    txtDesc.Text = IIf(IsDBNull(sqRDR("Description")), "", sqRDR("Description"))
                    ddlModule.SelectedItem.Text = IIf(IsDBNull(sqRDR("ModName")), "", sqRDR("ModName"))
                    dtDate.Text = IIf(IsDBNull(sqRDR("RelDate")), "", sqRDR("RelDate"))
                    ddlType.SelectedItem.Text = IIf(IsDBNull(sqRDR("RelType")), "", sqRDR("RelType"))

                End While
            End If

            sqRDR.Close()
        Catch ex As Exception
            CreateLog("Releases", "bindDiskGrid-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

    Private Sub DgReleses_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgReleses.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                If IsNothing(mdvUpdation) = True Then
                    Exit Sub
                Else

                    For inti As Integer = 0 To mdvUpdation.Table.Columns.Count - 1
                        e.Item.Cells(inti + 1).Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")
                        e.Item.Cells(inti + 1).Attributes.Add("style", "cursor:hand")
                        e.Item.Cells(inti + 1).Attributes.Add("ondblclick", "KeyCheck56(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text & ")")

                    Next
                End If
            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlUpdation:DgReleses:_ctl1:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("Basicmonitoring", "dgrDiskMonitor_ItemDataBound-1485", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Sub
    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        DgReleses.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        BindGrid()
    End Sub
    Private Sub Prevbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Prevbutton.Click
        If (DgReleses.CurrentPageIndex > 0) Then
            DgReleses.CurrentPageIndex -= 1
            _currentPageNumber = Int32.Parse(CurrentPg.Text) - 1
            CurrentPg.Text = _currentPageNumber
        End If
        BindGrid()
    End Sub
    Private Sub Nextbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Nextbutton.Click
        If (DgReleses.CurrentPageIndex < (DgReleses.PageCount - 1)) Then
            DgReleses.CurrentPageIndex += 1

            If DgReleses.PageCount = CurrentPg.Text Then
                CurrentPg.Text = DgReleses.PageCount
            Else
                _currentPageNumber = Int32.Parse(CurrentPg.Text) + 1
                CurrentPg.Text = _currentPageNumber
            End If
        End If
        BindGrid()
    End Sub
    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        DgReleses.CurrentPageIndex = (DgReleses.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber
        BindGrid()
    End Sub

  
End Class
