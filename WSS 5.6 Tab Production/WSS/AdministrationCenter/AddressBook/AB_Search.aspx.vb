'*******************************************************************
' Page                   : - Ab_Search"address book view screen"
' Purpose                : - It is meant for search any address book according to various search crieteria  like name or address number
' Date					Author	 			Modification Date					Description
' 11/03/06			sachin prashar					-------------------					Created
'
' Notes: 
' Code:
'*******************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Drawing
Imports System.Data
'''' Session and ViewState Variables Used on this Page are ::
'ViewState("PageSize")
'ViewState("PropShowAllAB")
'ViewState("SortOrder")
'ViewState("SortWay")
'ViewState("SAddressNumber_AddressBook")
'ViewState("ABViewName")
'ViewState("ABViewValue") 
'ViewState("Flag")
'Session("PropUserID")
'Session("PropUserName")
'Session("PropRole")
'Session("ProjectViewName")
'Session("PropCompanyType")
'Session("PropCompanyID")

Partial Class AdministrationCenter_AddressBook_AB_Search
    Inherits System.Web.UI.Page

#Region "global level declaration"

    Dim mdvtable As New DataView ' store data from table for view grid 
    Dim arColumnName As New ArrayList ' this is stored grid's columns name to assined value to the texboxes
    Dim rowvalue As Integer ' assigned row value to grid rows and use when action implemented on grid's rows

    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer
    Private Shared mTextBox() As TextBox
    Private Shared arColWidth As New ArrayList
    '***************************************************

    Dim mshFlag As Short
    Dim shF As Short

    Dim arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Dim arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Dim arrImageUrlNew As New ArrayList 'Used to store new comments

    Dim StrResumePath As String


    Public mintPageSize As Integer
    Protected _currentPageNumber As Int32 = 1


#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        TextBox1.BorderStyle = BorderStyle.None
        TextBox1.BackColor = Color.White

        Panel1.Visible = True
        GrdAddSerach.Visible = True
        'cpnlErrorPanel.Visible = False

        'javascript function added with controls
        '**********************************************************************************
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgShowAll.Attributes.Add("Onclick", "return SaveEdit('ShowAll');")
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgPassportInfo.Attributes.Add("Onclick", "return SaveEdit('EditPassportInfo');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgPlus.Attributes.Add("Onclick", "return OpenW('T010011');")
            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
            '*******************************************************************************
            ddlstview.Attributes.Add("OnChange", "return SaveEdit('View');") ' for view 
            BtnGrdSearch1.Attributes.Add("Onclick", "return SaveEdit('Search');")
            BtnGrdSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        End If
        arrImageUrlDisabled.Clear()
        '  arrImageUrlDisabled.Add("../../Images/comment_blank.gif")
        arrImageUrlDisabled.Add("../../Images/white.gif")

        arrImageUrlNew.Clear()
        arrImageUrlNew.Add("../../Images/white.gif")

        arrImageUrlEnabled.Clear()
        arrImageUrlEnabled.Add("../../Images/attach15_9.gif")



        '''''paging

        ''******************************************
        'mintPageSize = Val(Request.Form("txtPageSize"))
        'If mintPageSize = 0 Or mintPageSize < 0 Then
        '    mintPageSize = 25
        'End If
        'txtPageSize.Text = mintPageSize
        ''******************************************


        'mintPageSize = Val(Request.Form("txtPageSize"))
        mintPageSize = Val(txtPageSize.Text)
        txtCSS(Me.Page)

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
                    'ViewState("PageSize") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize") = mintPageSize
                End If

                SavePageSize()
            End If
        End If


        If Not IsPostBack Then
            If IsNothing(ViewState("PropShowAllAB")) Then
                ViewState("PropShowAllAB") = "0"
                ViewState("SortOrder") = Nothing
                ViewState("SortWay") = 0

            End If
        End If
        If Not IsNothing(ViewState("PropShowAllAB")) Then
            If ViewState("PropShowAllAB") = "0" Then
                imgShowAll.ToolTip = "Show All"
            Else
                imgShowAll.ToolTip = "Hide Disabled"
            End If
        End If

        Dim txthiddenImage = Request.Form("txthiddenImage")
        If Request.Form("txthidden") = "" Then
        Else
            ViewState("SAddressNumber_AddressBook") = Request.Form("txthidden")
        End If


        'these statements check the button click caption 
        '***********************************************
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "ShowAll"
                        If Not IsNothing(ViewState("PropShowAllAB")) Then
                            If ViewState("PropShowAllAB") = "1" Then
                                ViewState("PropShowAllAB") = "0"
                            Else
                                ViewState("PropShowAllAB") = "1"
                            End If
                        Else
                            ViewState("PropShowAllAB") = "1"
                        End If

                    Case "Edit"
                        If ViewState("SAddressNumber_AddressBook") <> "" And ViewState("SAddressNumber_AddressBook") <> "-1" Then
                            ' Response.Redirect("AB_Main.aspx?ScrID=194&AddressNo=" + ViewState("SAddressNumber_AddressBook") + "", False)
                        End If

                    Case "Add"
                        ViewState("SAddressNumber_AddressBook") = "-1"
                        'Response.Redirect("AB_Main.aspx?ScrID=194", False)

                    Case "Delete"
                        If ViewState("SAddressNumber_AddressBook") <> "" And ViewState("SAddressNumber_AddressBook") <> "-1" Then
                            Dim intAddressNumber As Integer = ViewState("SAddressNumber_AddressBook")

                            If WSSDelete.DeleteAddressBookEntry(intAddressNumber) = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            Else
                                lstError.Items.Clear()
                                lstError.Items.Add("You cannot delete this record as it has been used in User Profile...")
                                ' ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            End If
                        End If

                    Case "View"
                        'filling session variables on combo change event
                        ViewState("ABViewName") = ddlstview.SelectedItem.Text
                        ViewState("ABViewValue") = ddlstview.SelectedItem.Value
                        If ViewState("Flag") = "1" Then
                            GetView()
                            ViewState("Flag") = 0
                        Else
                            SaveUserView()

                        End If


                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("AB_Search", "Load-151", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
        '********************************************************************
        If Not IsNothing(ViewState("PropShowAllAB")) Then
            If ViewState("PropShowAllAB") = "0" Then
                imgShowAll.ToolTip = "Show All"
            Else
                imgShowAll.ToolTip = "Hide Disabled"
            End If
        End If

        If Not IsPostBack Then
            'fill dropdown combo with view name from database
            GetView()
            ChkSelectedView() 'chk user selected view last time
            'fill tha datagrid from based on admin defined to the role
            If ViewState("ABViewName") <> "" And ViewState("ABViewName") <> "Default" Then
                ' fill datagrid based on user define columns and combination
                FillView()
            Else
                'fill tha datagrid from based on admin defined to the role
                fillDefault()
                ViewState("ABViewName") = "Default"
            End If
            'fillDefault()
            'ViewState("ABViewName")= "Default"
            'this will create textboxesover datagrid's columns
            CreateTextBox()
            CurrentPg.Text = _currentPageNumber.ToString()
        Else

            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist

            'this loop filling new arraylist in the arrtextvalue array
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlCallView$" & arCol.Item(i)))
            Next
            '**************************************
            If ddlstview.SelectedValue = 0 Then
                'fill tha datagrid from based on admin defined to the role
                fillDefault()
            Else
                ' fill datagrid based on user define columns and combination
                FillView()
            End If

            'this function create the texboxes on the top of grid
            CreateTextBox()

            'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
            '************************************************
            If ChechkValidityforSearch(arrtextvalue) = True Then
                FillGRDAfterSearch()
            End If


            If IsNothing(ViewState("SortOrder")) = False Then
                SortGRDDuplicate()
            End If



        End If


        'Security Block
        '************************************************************
        Dim intID As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
            'End of Security Block
            '***********************************************************
        End If
    End Sub

#End Region

#Region "Fill default view"
    '*******************************************************************
    ' Function             :-  fillDefault
    ' Purpose              :- fill and design datagrid based on default view data from security tables
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

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            '     SQL.DBTable = "T010011"
            SQL.DBTracing = False
            '**************
            GrdAddSerach.PageSize = mintPageSize ' set the grid page size


            ' Dim arSetColumnName As New ArrayList
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select "
            Dim strQuery As String
            Dim htDateCols As New Hashtable

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =229 And " _
                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                    & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=229 and obm_vc4_object_type_fk='VIW') " _
                    & " order by OBM.OBM_SI2_Order_By"

            '   SQL.DBTable = "T070042"
            sqrdView = SQL.Search("AB_Search", "FillDefault-259", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)

            arColumnName.Clear()
            arCol.Clear()
            arSetColumnName.Clear()
            arColWidth.Clear()
            Dim StrBRColumn As String = ""

            If blnView = True Then
                While sqrdView.Read

                    If sqrdView.Item("OBM_VC200_URL") = "CI_DT8_Date_Created" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "CI_DT8_Date_Modified" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("OBM_VC200_URL") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 1)
                    Else
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","
                    End If



                    '*****************************************
                    arColWidth.Add(sqrdView.Item("OBM_VC200_DESCR")) 'adding columns widthe in arraylist
                    Dim strcolname As String
                    strcolname = sqrdView.Item("ROD_VC50_ALIAS_NAME")

                    If CStr(sqrdView.Item("OBM_VC200_URL")).ToUpper.Equals("CI_IN4_Business_Relation".ToUpper) Then
                        StrBRColumn = sqrdView.Item("ROD_VC50_ALIAS_NAME")
                    End If


                    If (InStr(sqrdView.Item("ROD_VC50_ALIAS_NAME"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        arSetColumnName.Add(strcolname)
                        arCol.Add(strcolname) 'adding columns name in arraylist

                    Else
                        arSetColumnName.Add(strcolname)
                        arCol.Add(strcolname) 'adding columns name in arraylist
                    End If
                    '**********************************************
                End While
                sqrdView.Close()

                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                If Session("PropCompanyType") = "SCM" Then
                    strSelect &= " from T010011"
                    If ViewState("PropShowAllAB") = "0" Then
                        
                        strSelect &= " where (CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "

                        Dim sqrdView1 As SqlDataReader = SQL.Search("AB_Search", "FillDefault-259", GetCompanySubQuery(), SQL.CommandBehaviour.CloseConnection, blnView)
                        If blnView = True Then
                            While sqrdView1.Read
                                strSelect &= " or (CI_IN4_Business_Relation= '" & sqrdView1.Item("CompID") & "')"
                            End While
                        End If
                        strSelect &= " ) and CI_VC8_Status='ENA'"
                    Else
                        strSelect &= " where (CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "
                        Dim sqrdView1 As SqlDataReader = SQL.Search("AB_Search", "FillDefault-259", GetCompanySubQuery(), SQL.CommandBehaviour.CloseConnection, blnView)
                        If blnView = True Then
                            While sqrdView1.Read
                                strSelect &= " or (CI_IN4_Business_Relation= '" & sqrdView1.Item("CompID") & "')"
                            End While
                            strSelect &= " )"
                        End If
                    End If

                Else
                    strSelect &= " from T010011 where (CI_IN4_Business_Relation='" & Session("PropCompanyID") & "'"
                    strSelect &= " and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "


                    Dim sqrdView1 As SqlDataReader = SQL.Search("AB_Search", "FillDefault-259", GetCompanySubQuery(), SQL.CommandBehaviour.CloseConnection, blnView)
                    If blnView = True Then
                        While sqrdView1.Read
                            strSelect &= " or (CI_IN4_Business_Relation= '" & sqrdView1.Item("CompID") & "')"
                        End While
                    End If
                    If ViewState("PropShowAllAB") = "0" Then
                        strSelect &= " and CI_VC8_Status='ENA')"
                    End If

                End If

                'Check to show all or not
                'Added company chk from company access table
                'strSelect &= " and CI_IN4_Business_Relation in (" & GetCompanySubQuery() & ") "
                



                '    SQL.DBTable = "T010011"
                If SQL.Search("T010011", "AB_Search", "FillDefault-301", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    'change the datagrid header columns name 
                    For inti As Integer = 0 To dsDefault.Tables("T010011").Columns.Count - 1
                        dsDefault.Tables("T010011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                    Next
                    mdvtable.Table = dsDefault.Tables("T010011")
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)


                    '***********************************************

                    If Not StrBRColumn.Trim.Equals("") Then

                        Dim htComs As New Hashtable
                        Dim DsTemp As New DataSet
                        SQL.Search("Comp", "AB_Search", "FillDefault-301", "Select CI_NU8_Address_Number ABNumber,CI_VC36_Name CompName from T010011 where CI_VC8_Address_Book_Type='COM'", DsTemp, "sachin", "Prashar")
                        For inti As Integer = 0 To DsTemp.Tables(0).Rows.Count - 1
                            htComs.Add(CStr(DsTemp.Tables(0).Rows(inti).Item("ABNumber")), CStr(DsTemp.Tables(0).Rows(inti).Item("CompName")))
                        Next
                        For intI As Integer = 0 To mdvtable.Table.Rows.Count - 1
                            If htComs.Contains(mdvtable.Table.Rows(intI).Item(StrBRColumn)) Then
                                mdvtable.Table.Rows(intI).Item(StrBRColumn) = htComs(mdvtable.Table.Rows(intI).Item(StrBRColumn))
                            End If
                        Next

                    End If

                    rowvalue = 0

                    '*******************************************
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If

                    GrdAddSerach.PageSize = mintPageSize

                    If ViewState("ABViewName") <> ddlstview.SelectedItem.Text Then
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
                    'CurrentPage.Text = _currentPageNumber.ToString()
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

                Else

                    ' GrdAddSerach.Visible = False
                    ' Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("Employee or Company not  Entred so far...")
                    ' ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    ' cpnlErrorPanel.Visible = True
                    'ddlstview.Enabled = False

                End If
            Else
                '  GrdAddSerach.Visible = False
                ' Panel1.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("Sorry!Address Book Data not available...")
                '   ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                'cpnlErrorPanel.Visible = True
            End If
        Catch ex As Exception
            CreateLog("AB_Search", "FillDefault-304", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

#End Region

#Region "fill View"
    '*******************************************************************
    ' Function             :-  fillview
    ' Purpose              :- fill and design datagrid based on user view data from user tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 2/5/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function FillView()
        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim strwhereQuery As String = " "
        ' Dim strConnection As String = SQL.GetConncetionString("strConnectionString")
        Dim arcolName As New ArrayList
        Dim StrBRColumn As String = ""
        GrdAddSerach.PageSize = mintPageSize ' set the grid page size

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        '  SQL.DBTable = "T030212"
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            Dim htDateCols As New Hashtable


            sqrdView = SQL.Search("AB_Search", "FillView-344", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=" & intViewID & "and UV_VC50_tbl_Name='229' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            'If SQL.Search("", "", strConnection, "select name,view_id from tbl_userview_name", dsView, "tbl_userview_detail") = True Then
            If blnView = True Then
                Dim dsFromView As New DataSet

                arColumnName.Clear()
                arCol.Clear()
                arColWidth.Clear()
                arSetColumnName.Clear()

                While sqrdView.Read
                    ' Check for sort order of the column and if AD value is not unsorted

                    If CStr(sqrdView.Item("UV_VC50_COL_Value")).ToUpper.Equals("CI_IN4_Business_Relation".ToUpper) Then
                        StrBRColumn = sqrdView.Item("UV_VC50_COL_Name")
                    End If

                    If sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        ' Check for sort order of the column and if AD value is unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower = "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & ", "
                        ' If sort order of the column =0 and AD value is not unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") = 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strUnsortQuery = sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                    End If


                    ' strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","


                    If sqrdView.Item("UV_VC50_COL_Value") = "CI_DT8_Date_Created" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "CI_DT8_Date_Modified" Then
                        strSelect &= "replace(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 110), '-', '/') + ' ' + right(convert(varchar(20), " & sqrdView.Item("UV_VC50_COL_Value") & ", 100), 7) + '' ,"
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 1)
                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If


                    ' arColumnName.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    'arCol.Add(sqrdView.Item("UV_VC50_COL_Name"))
                    arColWidth.Add(sqrdView.Item("UV_VC10_Col_Width"))
                    'code for blank space in alias name 
                    '******************************************************
                    Dim strcolname As String
                    strcolname = sqrdView.Item("UV_VC50_COL_Name")
                    If (InStr(sqrdView.Item("UV_VC50_COL_Name"), " ")) Then
                        strcolname = strcolname.Replace(" ", "_")
                        arColumnName.Add(strcolname)
                        arCol.Add(strcolname)
                        arSetColumnName.Add(strcolname)
                    Else
                        arColumnName.Add(strcolname)
                        arCol.Add(strcolname)
                        arSetColumnName.Add(strcolname)
                    End If

                    '*****************************************************
                End While
                sqrdView.Close()
                strSelect = strSelect.Remove(Len(strSelect) - 1, 1)
                'add where clause in query 
                '*************************************
                sqrdView = SQL.Search("AB_Search", "FillView-400", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='229' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)
                If blnView = True Then
                    While sqrdView.Read
                        If sqrdView.Item("UV_VC50_COL_Value") = "CI_IN4_Business_Relation" Then
                            Dim strrelationname As String
                            Dim intrelationid As Integer
                            strrelationname = sqrdView.Item("UV_VC20_Value")
                            If Not IsNumeric(strrelationname) = True Then
                                strrelationname = strrelationname.Replace("'", "")
                                mstGetFunctionValue = WSSSearch.SearchCompName(strrelationname)
                                intrelationid = mstGetFunctionValue.ExtraValue
                                If intrelationid > 0 Then
                                    If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                        Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                        strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                        strSplit = strSplit.Replace("''", "'")
                                        strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                    Else
                                        strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & "'" & intrelationid & "'" & " and "
                                    End If

                                Else

                                    If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                        Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                        strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                        strSplit = strSplit.Replace("''", "'")
                                        strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                    Else
                                        strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                    End If
                                End If
                            Else
                                If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                    Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                    strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                    strSplit = strSplit.Replace("''", "'")
                                    strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                Else
                                    strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                End If
                            End If
                        Else
                            If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                strSplit = strSplit.Replace("''", "'")
                                strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                            Else
                                strwhereQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                            End If
                        End If
                    End While
                    sqrdView.Close()
                    strwhereQuery = strwhereQuery.Remove(Len(strwhereQuery) - 4, 4)
                    '*******************************************
                    If Session("PropCompanyType") = "SCM" Then
                    Else
                        strwhereQuery += " and CI_IN4_Business_Relation='" & Session("PropCompanyID") & "'"
                    End If
                    'Check to show all or not
                    'If ViewState("PropShowAllAB") = "0" Then
                    '    strwhereQuery &= " and CI_VC8_Status='ENA'"
                    'End If
                End If



                If strwhereQuery.Equals(" ") = True Then
                    If ViewState("PropShowAllAB") = "0" Then
                        strwhereQuery &= " CI_VC8_Status='ENA'"
                    End If
                Else
                    'if got some data from database then add in query
                    If ViewState("PropShowAllAB") = "0" Then
                        strwhereQuery &= " and CI_VC8_Status='ENA'"
                    End If
                End If

                'Added company chk from company access table
                strwhereQuery &= " and CI_IN4_Business_Relation in (" & GetCompanySubQuery() & ") "
                strwhereQuery &= "and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "


                'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
                strSelect &= " from T010011"
                If strwhereQuery.Equals(" ") = True Then
                Else
                    strSelect &= " where" & strwhereQuery
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
                '     SQL.DBTable = "T010011"
                If SQL.Search("T010011", "AB_Search", "Fillview-455", strSelect, dsFromView, "sachin", "Prashar") = True Then
                    ' DataGrid1.DataSource = dsFromView
                    'dsFromView.Tables(0).Columns(0).ColumnName = ""
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                    Next
                    mdvtable.Table = dsFromView.Tables(0)
                    HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)




                    '***********************************************

                    If Not StrBRColumn.Trim.Equals("") Then

                        Dim htComs As New Hashtable
                        Dim DsTemp As New DataSet
                        SQL.Search("Comp", "AB_Search", "FillDefault-301", "Select CI_NU8_Address_Number ABNumber,CI_VC36_Name CompName from T010011 where CI_VC8_Address_Book_Type='COM'", DsTemp, "sachin", "Prashar")
                        For inti As Integer = 0 To DsTemp.Tables(0).Rows.Count - 1
                            htComs.Add(CStr(DsTemp.Tables(0).Rows(inti).Item("ABNumber")), CStr(DsTemp.Tables(0).Rows(inti).Item("CompName")))
                        Next
                        For intI As Integer = 0 To mdvtable.Table.Rows.Count - 1
                            If htComs.Contains(mdvtable.Table.Rows(intI).Item(StrBRColumn)) Then
                                mdvtable.Table.Rows(intI).Item(StrBRColumn) = htComs(mdvtable.Table.Rows(intI).Item(StrBRColumn))
                            End If
                        Next

                    End If
                    '*******************************************

                    rowvalue = 0

                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    '  GrdAddSerach.Columns.Clear()
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                        GrdAddSerach.PageSize = mintPageSize
                    End If


                    If ViewState("ABViewName") <> ddlstview.SelectedItem.Text Then
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
                    'CurrentPage.Text = _currentPageNumber.ToString()
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

                Else

                    GrdAddSerach.Visible = False
                    Panel1.Visible = False

                    lstError.Items.Clear()
                    lstError.Items.Add("Employee or Company not Entred so far or data not exist according to view query ...")
                    '     ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    'cpnlErrorPanel.Visible = True

                End If
            Else

                Exit Function

            End If
        Catch ex As Exception
            CreateLog("AB_Search", "FillView-436", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

    End Function

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
            '      SQL.DBTable = "T030201"
            SQL.DBTracing = False

            sqrdView = SQL.Search("AB_Search", "Getview-566", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='229' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)

            If blnView = True Then
                ddlstview.DataSource = sqrdView
                ddlstview.DataTextField = "UV_VC50_View_Name"
                ddlstview.DataValueField = "UV_IN4_View_ID"
                ddlstview.DataBind()
                sqrdView.Close()
            Else

            End If

            ddlstview.Items.Add("Default")
            ddlstview.Items(ddlstview.Items.Count - 1).Value = 0
            ddlstview.SelectedIndex = ddlstview.Items.Count - 1

        Catch ex As Exception
            CreateLog("AB_Search", "GetView-541", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
    Private Function CreateTextBox()


        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arColumns.Clear()

        'fill the columns count into the array from mdvtable view

        Try
            intCol = mdvtable.Table.Columns.Count
        Catch ex As Exception

        End Try

        If Not IsPostBack Then
            ReDim mTextBox(intCol)
        End If

        If mTextBox.Length < intCol Then
            ReDim mTextBox(intCol)
        End If

        Try
            For intii = 0 To intCol - 1
                _textbox = New TextBox

                If Not IsPostBack Then
                    Dim col1 As Unit
                    Dim col1cng As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    'col1cng = col1.Value + 1
                    'col1cng = col1cng & "pt"
                    If intii > 8 And intii < 17 Then
                        col1cng = col1.Value - 1.5
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 17 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    End If

                    arCol.Add(arSetColumnName.Item(intii))

                    If arSetColumnName.Item(intii) = "hyatt" Or arSetColumnName.Item(intii) = "A" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=""0"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If

                    _textbox.ID = arSetColumnName.Item(intii)
                    _textbox.Text = ""
                    mTextBox(intii) = _textbox

                Else

                    Dim col1 As Unit
                    Dim col1cng As String
                    Dim strcolid As String
                    col1 = Unit.Parse(arColWidth.Item(intii))
                    'col1cng = col1.Value + 1
                    'col1cng = col1cng & "pt"

                    If intii > 8 And intii < 17 Then
                        col1cng = col1.Value - 1.5
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 17 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    End If
                    If Not IsNothing(mdvtable.Table.Columns.Count) Then
                        If arrtextvalue.Count <> Val(mdvtable.Table.Columns.Count) Then
                            _textbox.Text = ""
                        Else
                            _textbox.Text = arrtextvalue.Item(intii)
                        End If
                    End If

                    '_textbox.Text = ""
                    strcolid = arCol.Item(intii)

                    If arCol.Item(intii) = "hyatt" Or arCol.Item(intii) = "A" Then
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=""0"" CssClass=SearchTxtBox></asp:TextBox>"))
                    Else
                        Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    End If

                    _textbox.ID = arCol.Item(intii)
                    mTextBox(intii) = _textbox
                End If
                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("ABSearch", "CreateTextBox-624", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

    End Function

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
            'GrdAddSerach.AutoGenerateColumns = False
            For intI = 0 To arCol.Count - 1
                If arCol.Item(intI) = "hyatt" Or arCol.Item(intI) = "A" Then
                    Dim Bound_Column As New BoundColumn
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(0)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(0)
                    Bound_Column.Visible = False
                    GrdAddSerach.Columns.Add(Bound_Column)
                Else
                    Dim Bound_Column As New BoundColumn
                    Dim strWidth As String = arColWidth.Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True
                    'Bound_Column.HeaderText = arColumnName.Item(intI)
                    GrdAddSerach.Columns.Add(Bound_Column)
                End If
            Next
        Catch ex As Exception
            CreateLog("ABSearch", "FormatGrid-766", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
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
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text
                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
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
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                Exit Sub
            End If
            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            HTMLEncodeDecode(mdlMain.Action.Decode, mdvtable)
            GetFilteredDataView(mdvtable, strRowFilterString)
            HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)
            GrdAddSerach.DataSource = mdvtable
            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If

            If ViewState("ABViewName") <> ddlstview.SelectedItem.Text Then
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
            'CurrentPage.Text = _currentPageNumber.ToString()
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
                lstError.Items.Add("Data not found according to your search string...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If

        Catch ex As Exception
            CreateLog("AB_Search", "Click-974", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch", )
        End Try
    End Sub
#End Region

#Region "Search Grid Item Data Bound Event"
    '*******************************************************************
    ' Function             :-  GrdAddSerach_ItemDataBound1
    ' Purpose              :-Display attachment, comment based on database and and bound java script on columns like selection and double click
    '								
    ' Date					  Author						Modification Date					Description
    '                			  Sachin Prashar		    -------------------					Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim Resumeflg As Boolean
        Dim intcolno As Short = 0


        ' GrdAddSerach.Columns.Clear()
        Try
            For ii As Integer = 0 To mdvtable.Table.Columns.Count


                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    If intcolno > 0 Then
                        strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                        e.Item.Cells(ii).Attributes.Add("style", "cursor:hand")
                        e.Item.Cells(ii).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "')")
                        e.Item.Cells(ii).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "')")
                    End If

                    If Not IsNothing(e.Item.Cells(0).FindControl("hyAtt")) Then
                        Resumeflg = GetResumeStat(strID)
                        If Resumeflg = True Then
                            CType(e.Item.Cells(0).FindControl("hyAtt"), System.Web.UI.WebControls.HyperLink).ImageUrl = "../../Images/Attach15_9.gif"
                            CType(e.Item.Cells(0).FindControl("hyAtt"), System.Web.UI.WebControls.HyperLink).NavigateUrl = StrResumePath
                        Else
                            CType(e.Item.Cells(0).FindControl("hyAtt"), System.Web.UI.WebControls.HyperLink).ImageUrl = "../../Images/divider.gif"
                        End If
                    End If

                End If
                intcolno = intcolno + 1
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("AB_Search", "ItemDataBound-1001", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch", )
        End Try
    End Sub
#End Region

    Function GetResumeStat(ByVal AddressNo As Integer) As String
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        Dim dstable As New DataSet

        Try
            If SQL.Search("T010043", "AB_Search", "FillDefault-301", " SELECT     PI_VC100_Resume  FROM T010043 WHERE     PI_VC100_Resume IS NOT NULL AND PI_VC100_Resume <> '' AND PI_NU8_Address_No = " & AddressNo & "", dstable, "sachin", "Prashar") = True Then

                StrResumePath = dstable.Tables(0).Rows(0).Item("PI_VC100_Resume")
                Return True
            Else
                Return False

            End If
        Catch ex As Exception
            CreateLog("Addressbook", "GetResumeStat-942", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function
    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            Dim intA As Integer = 0
            For intI = 0 To arColWidth.Count - 1 + 2

                If intI > 1 Then
                    If e.Item.Cells.Count > 1 Then
                        e.Item.Cells(intA + 1).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                    End If
                    intA += 1
                ElseIf intI = 0 Then
                    e.Item.Cells(0).Width = System.Web.UI.WebControls.Unit.Parse("13pt")
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ddlstview_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged
        ViewState("ABViewName") = ddlstview.SelectedItem.Text
        ViewState("ABViewValue") = ddlstview.SelectedValue
    End Sub
    Private Sub Firstbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Firstbutton.Click
        GrdAddSerach.CurrentPageIndex = Convert.ToInt32("0")
        _currentPageNumber = 1
        CurrentPg.Text = _currentPageNumber
        If ddlstview.SelectedValue = 0 Then
            fillDefault()
        Else
            FillView()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
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
            FillView()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
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
            FillView()
        End If
        If ChechkValidityforSearch(arrtextvalue) = True Then
            FillGRDAfterSearch()
        End If
        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
    End Sub
    Private Sub Lastbutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Lastbutton.Click
        GrdAddSerach.CurrentPageIndex = (GrdAddSerach.PageCount - 1)
        _currentPageNumber = Int32.Parse(TotalPages.Text)
        CurrentPg.Text = _currentPageNumber

        If ddlstview.SelectedValue = 0 Then
            fillDefault()
        Else
            FillView()
        End If

        If ChechkValidityforSearch(arrtextvalue) = True Then
            FillGRDAfterSearch()
        End If

        If IsNothing(ViewState("SortOrder")) = False Then
            SortGRDDuplicate()
        End If
    End Sub
    Private Sub SortGRD()
        ' If SortWay Mod 2 = 0 Then
        If Val(ViewState("SortWay")) Mod 2 = 0 Then
            mdvtable.Sort = ViewState("SortOrder") & " ASC"
        Else
            mdvtable.Sort = ViewState("SortOrder") & " DESC"
        End If
        '   SortWay += 1
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
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GrdAddSerach_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddSerach.SortCommand
        ViewState("SortOrder") = e.SortExpression
        ' StrSortOrder = e.SortExpression
        SortGRD()
    End Sub
    Private Sub SaveUserView()
        Dim intid = 229 ' screen id for call view screen
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

        sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030213 where  UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_SCREEN_ID=229 and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & " And UV_NU9_USER_ID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

        If blnReturn = False Then
            ViewState("ABViewName") = "Default"
            ViewState("ABViewValue") = "0"
            Exit Sub
        Else
            While sqdrCol.Read
                ViewState("ABViewName") = sqdrCol.Item("UV_VC50_View_Name")
                ViewState("ABViewValue") = sqdrCol.Item("UV_IN4_View_ID")

                ddlstview.SelectedValue = ViewState("ABViewValue")

            End While
            sqdrCol.Close()
        End If
    End Sub

    Private Sub SavePageSize()
        Dim intid = 229
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
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=229 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

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
End Class
