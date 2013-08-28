'*********************************************************************************************************
' Page                   : - SubCategory View
' Purpose                : -
' Tables used            : - T070011,T070031,T070042,T060011,T060022,T030212,T210011,T010011,T060011
' Date					Author						Modification Date					Description
' 20/07/06				AMIT/Sachin				-------------------					Created
'
' Notes: 
' Code:
'*********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
'''' Session Variables Used on this Page are :::
'ViewState("PageSize_SubCategory")
'ViewState("SortOrder_SubCategory")
'ViewState("SortWay_SubCategory")
'ViewState("PropProjectID")
'ViewState("SAddressNumber_ProjectView")
'ViewState("PropCAComp")
'ViewState("ProjectViewName")
'Session("PropUserID")
'Session("PropUserName")
'Session("PropRole")
'Session("PropCompanyType")
'Session("PropCompanyID")

Partial Class AdministrationCenter_Project_ProjectView
    Inherits System.Web.UI.Page
    ' Protected WithEvents pnlMsg As System.Web.UI.WebControls.Panel

#Region "global level declaration"

    Dim mdvtable As New DataView 'this will  store data from table for view grid 
    Dim rowvalue As Integer 'assigned row value to grid rows and use when action implemented on grid's rows
    Dim arColumnName As New ArrayList

    'thse variables store the grid related information like columns name columns width and textboxes values etc
    '****************************************************
    Private Shared arColWidth As New ArrayList
    Private Shared mTextBox() As TextBox
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer
    '*****************************************************

    Dim mintColumns As Integer
    Dim mshFlag As Short
    Dim shF As Short
    Private Shared mintCompId As String 'this will store company's ID

    Public mintPageSize As Integer
    Protected _currentPageNumber As Int32 = 1


#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        ''cpnlErrorPanel.Visible = False
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            'javascript function added with controls
            '**********************************************************************************
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgPlus.Attributes.Add("Onclick", "return OpenW('T210011');")
            txtPageSize.Attributes.Add("onkeypress", "NumericOnly();") 'check numaric value 
            '********************************************************************************
            BtnGrdSearch1.Attributes.Add("Onclick", "return SaveEdit('Search');")
            BtnGrdSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        End If

        '''''paging
        ''******************************************
        'mintPageSize = Val(Request.Form("txtPageSize"))
        'If mintPageSize = 0 Or mintPageSize < 0 Then
        '    mintPageSize = 25
        'End If

        'txtPageSize.Text = mintPageSize
        mintPageSize = Val(Request.Form("txtPageSize"))
        txtCSS(Me.Page)

        If IsPostBack = False Then
            If ChkPageView() = True Then
                txtPageSize.Text = ViewState("PageSize_SubCategory")
                mintPageSize = ViewState("PageSize_SubCategory")
                'SavePageSize()
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = 20
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize_SubCategory") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize_SubCategory") = mintPageSize
                    SavePageSize()
                End If
            End If
        Else
            If ViewState("PageSize_SubCategory") = mintPageSize Then
            Else
                If mintPageSize = 0 Or mintPageSize < 0 Then
                    mintPageSize = ViewState("PageSize_SubCategory")
                    txtPageSize.Text = ViewState("PageSize_SubCategory")
                    'ViewState("PageSize_SubCategory") = mintPageSize
                Else
                    txtPageSize.Text = mintPageSize
                    ViewState("PageSize_SubCategory") = mintPageSize
                End If

                SavePageSize()
            End If
        End If
        GrdAddSerach.Visible = True
        Panel1.Visible = True

        '******************************************


        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenCompID As String = Request.Form("txthiddenCompID")
        Dim txthiddenProjectID As String = Request.Form("txthiddenProjID")

        If Not IsPostBack Then
            ViewState("PropProjectID") = "-1"
            ViewState("SortOrder_SubCategory") = Nothing
            ViewState("SortWay_SubCategory") = 0

        End If
        If Request.Form("txthidden") = "" Then
        Else
            ViewState("SAddressNumber_ProjectView") = Request.Form("txthidden")
            ViewState("PropCAComp") = WSSSearch.SearchCompName(txthiddenCompID.Trim).ExtraValue
            ViewState("PropProjectID") = txthiddenProjectID.Trim
        End If

        'These statements check the button click caption 
        '***********************************************
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Edit"

                        If ViewState("SAddressNumber_ProjectView") <> "" And ViewState("SAddressNumber_ProjectView") <> "-1" Then
                            '  Response.Redirect("ProjectMasterDetail.aspx?ScrID=670&ProjectID=" & ViewState("PropProjectID") & "&CompanyID=" & ViewState("PropCAComp") & "", False)
                        End If
                    Case "Add"
                        ViewState("PropProjectID") = "-1"
                        ' Response.Redirect("ProjectMasterDetail.aspx?ScrID=670&ProjectID=" & ViewState("PropProjectID") & "&CompanyID=" & ViewState("PropCAComp") & "", False)
                    Case "Delete"
                        If ViewState("SAddressNumber_ProjectView") <> "" And ViewState("SAddressNumber_ProjectView") <> "-1" Then
                            mstGetFunctionValue = WSSDelete.DeleteProject(Val(ViewState("PropCAComp")), Val(ViewState("SAddressNumber_ProjectView")))
                            lstError.Items.Clear()
                            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                            If mstGetFunctionValue.ErrorCode = 0 Then
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            Else
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            End If

                        End If
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("ProjectView", "Load-178", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If
        '***************************************************************************************
        If Not IsPostBack Then
            'fill dropdown combo with view name from database
            GetView()
            'fill tha datagrid from based on admin defined to the role
            fillDefault()
            ViewState("ProjectViewName") = "Default"
            'this will format the grid based on data base info
            '  FormatGrid()
            'this will create textboxesover datagrid's columns
            CreateTextBox()
            CurrentPg.Text = _currentPageNumber.ToString()

        Else
            ' fill the textboxes value into the array 
            '**********************************
            arrtextvalue.Clear() ' clear the old data from arraylist to fill new arraylist
            'This loop filling new arraylist in the arrtextvalue array
            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("up1$" & arCol.Item(i)))
            Next
            '**************************************
            If ddlstview.SelectedValue = 0 Then
                'fill tha datagrid from based on admin defined to the role
                fillDefault()
            Else
                ' fill datagrid based on user define columns and combination
                FillView()
            End If
            'format the grid based on data base info
            ' FormatGrid()
            'this function create the texboxes on the top of grid
            CreateTextBox()
            'this function check the array of textboex have any data or not if yes then call function which fill datagrid based of textboxes data
            '************************************************
            If ChechkValidityforSearch(arrtextvalue) = True Then
                FillGRDAfterSearch()
            End If
            '**********************************************

            If IsNothing(ViewState("SortOrder_SubCategory")) = False Then
                SortGRDDuplicate()
            End If



        End If
        'Security Block
        '***********************************************************
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
        End If
        'End of Security Block
        '**********************************************************
    End Sub

#End Region

#Region "Filldefault view"

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
            'SQL.DBTable = "T010011"
            SQL.DBTracing = False
            GrdAddSerach.PageSize = mintPageSize ' set the grid page size
            '**************
            ' Dim arSetColumnName As New ArrayList
            Dim intViewID As Integer = ddlstview.SelectedValue
            Dim sqrdView As SqlDataReader
            Dim blnView As Boolean
            Dim strSelect As String = "select "
            Dim strQuery As String
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String
            Dim strwhereQuery As String = " and "

            strQuery = "select OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_DESCR from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =40 And " _
                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                    & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=40 and obm_vc4_object_type_fk='VIW') " _
                    & " order by OBM.OBM_SI2_Order_By"

            ' SQL.DBTable = "T070042"
            sqrdView = SQL.Search("AB_Search", "FillDefault-259", strQuery, SQL.CommandBehaviour.CloseConnection, blnView)


            arColumnName.Clear()
            arCol.Clear()
            arSetColumnName.Clear()
            arColWidth.Clear()

            If blnView = True Then

                Dim htDateCols As New Hashtable

                While sqrdView.Read

                    If sqrdView.Item("OBM_VC200_URL") = "PR_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "PR_DT8_Start_Date" Then
                        strSelect &= " convert(varchar,PR_DT8_Start_Date,101) " & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "PR_DT8_Close_Date" Then
                        strSelect &= " convert(varchar,PR_DT8_Close_Date,101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("ROD_VC50_ALIAS_NAME")), 2)
                    ElseIf sqrdView.Item("OBM_VC200_URL") = "PR_NU9_Owner_ID_Fk" Then
                        strSelect &= "owner." & "UM_VC50_UserID" & ","
                    Else
                        strSelect &= sqrdView.Item("OBM_VC200_URL") & ","
                    End If

                    arColWidth.Add(sqrdView.Item("OBM_VC200_DESCR")) 'adding columns widthe in arraylist

                    '*****************************************
                    Dim strcolname As String
                    strcolname = sqrdView.Item("ROD_VC50_ALIAS_NAME")

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
                '**************************************************************************************
                sqrdView = SQL.Search("CallView", "FillView-785", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='T210011'  order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

                While sqrdView.Read
                    ' Check for sort order of the column and if AD value is not unsorted
                    If sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        ' Check for sort order of the column and if AD value is unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower = "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & ", "
                        ' If sort order of the column =0 and AD value is not unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") = 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strUnsortQuery = sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                    End If
                End While
                sqrdView.Close()
                strwhereQuery = strwhereQuery.Remove(Len(strwhereQuery) - 4, 4)

                strSelect &= " from T210011 a,T010011 comp, T060011 owner   where  a.PR_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number and  a.PR_NU9_Owner_ID_Fk=owner.UM_IN4_Address_No_FK "

                strSelect &= " AND PR_NU9_Comp_ID_FK IN(" & GetCompanySubQuery() & ") "

                strSelect &= " order by PR_VC20_Name asc "

                '                SQL.DBTable = "T210011"
                If SQL.Search("T210011", "AB_Search", "FillDefault-301", strSelect, dsDefault, "sachin", "Prashar") = True Then
                    'change the datagrid header columns name 
                    For inti As Integer = 0 To dsDefault.Tables("T210011").Columns.Count - 1
                        dsDefault.Tables("T210011").Columns(inti).ColumnName = arSetColumnName.Item(inti)
                        If UCase(arSetColumnName.Item(inti)) = "SubCategoryComp".ToUpper Then
                            mintCompId = inti
                        End If
                    Next

                    rowvalue = 0
                    mdvtable.Table = dsDefault.Tables("T210011")
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    GrdAddSerach.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke

                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                    End If


                    GrdAddSerach.PageSize = mintPageSize

                    If ViewState("ProjectViewName") <> ddlstview.SelectedItem.Text Then
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
                    ' Panel1.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("SubCategory not  opened so far...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    ''cpnlErrorPanel.Visible = True
                    '  ddlstview.Enabled = False
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Role do not have the Grid Columns  Access on the Subcategory screen...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
            '**********************************

        Catch ex As Exception
            CreateLog("ProjectView", "FillDefault-304", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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

    Private Sub FillView()
        Dim intViewID As Integer = ddlstview.SelectedValue
        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        Dim strSelect As String = "select "
        Dim strwhereQuery As String = " "
        Dim shJoin As Short

        Dim arcolName As New ArrayList
        GrdAddSerach.PageSize = mintPageSize ' set the grid page size
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            Dim strOrderQuery As String = " order by "
            Dim strUnsortQuery As String

            sqrdView = SQL.Search("AB_Search", "FillView-344", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD  from T030212 where UV_IN4_View_ID=" & intViewID & "and UV_VC50_tbl_Name='40' order by uv_in4_id", SQL.CommandBehaviour.CloseConnection, blnView)
            'If SQL.Search("", "", strConnection, "select name,view_id from tbl_userview_name", dsView, "tbl_userview_detail") = True Then
            If blnView = True Then
                Dim dsFromView As New DataSet

                Dim htDateCols As New Hashtable

                arColumnName.Clear()
                arCol.Clear()
                arColWidth.Clear()
                arSetColumnName.Clear()

                While sqrdView.Read
                    ' Check for sort order of the column and if AD value is not unsorted
                    If sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                        ' Check for sort order of the column and if AD value is unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") > 0 And sqrdView.Item("UV_VC5_AD").trim.tolower = "unsorted" Then
                        strOrderQuery &= sqrdView.Item("UV_VC50_COL_Value") & ", "
                        ' If sort order of the column =0 and AD value is not unsorted
                    ElseIf sqrdView.Item("UV_NU9_SO") = 0 And sqrdView.Item("UV_VC5_AD").trim.tolower <> "unsorted" Then
                        strUnsortQuery = sqrdView.Item("UV_VC50_COL_Value") & " " & sqrdView.Item("UV_VC5_AD") & ", "
                    End If

                    If sqrdView.Item("UV_VC50_COL_Value") = "PR_NU9_Comp_ID_FK" Then
                        strSelect &= "comp." & "CI_VC36_Name" & ","
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "PR_DT8_Start_Date" Then
                        strSelect &= " convert(varchar,PR_DT8_Start_Date,101) " & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "PR_DT8_Close_Date" Then
                        strSelect &= " convert(varchar,PR_DT8_Close_Date,101)" & ","
                        htDateCols.Add(CStr(sqrdView.Item("UV_VC50_COL_Name")), 2)
                    ElseIf sqrdView.Item("UV_VC50_COL_Value") = "PR_NU9_Owner_ID_Fk" Then
                        strSelect &= "owner." & "UM_VC50_UserID" & ","
                        shJoin = 1

                    Else
                        strSelect &= sqrdView.Item("UV_VC50_COL_Value") & ","
                    End If

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
                sqrdView = SQL.Search("AB_Search", "FillView-400", "select UV_VC50_COL_Value,UV_VC50_COL_Name,UV_VC10_Col_Width,UV_NU9_SO,UV_VC5_AD, UV_VC5_FA, UV_VC20_Value from T030212 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_TBL_Name='40' and UV_VC5_FA<>'' order by uv_NU9_So", SQL.CommandBehaviour.CloseConnection, blnView)

                If blnView = True Then
                    While sqrdView.Read

                        If sqrdView.Item("UV_VC50_COL_Value") = "PR_NU9_Comp_ID_FK" Then
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
                                        strwhereQuery &= " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                    Else
                                        strwhereQuery += " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                                    End If
                                Else
                                    If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                        Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                        strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                        strSplit = strSplit.Replace("''", "'")
                                        strwhereQuery &= " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                                    Else
                                        strwhereQuery += " comp.CI_VC36_Name " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
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
                        ElseIf sqrdView.Item("UV_VC50_COL_Value") = "PR_DT8_Start_Date" Then
                            If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                strSplit = strSplit.Replace("''", "'")
                                strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                            Else
                                strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                            End If


                        ElseIf sqrdView.Item("UV_VC50_COL_Value") = "PR_NU9_Owner_ID_Fk" Then

                            If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                strwhereQuery += "  Owner.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                            Else
                                strwhereQuery += "  Owner.UM_VC50_UserID " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
                            End If

                        ElseIf sqrdView.Item("UV_VC50_COL_Value") = "PR_DT8_Close_Date" Then
                            If sqrdView.Item("UV_VC5_FA") = "IN" Or sqrdView.Item("UV_VC5_FA") = "NOT IN" Then
                                Dim strSplit As String = sqrdView.Item("UV_VC20_Value")
                                strSplit = "(" & strSplit.Replace(",", "','") & ")"
                                strSplit = strSplit.Replace("''", "'")
                                strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & strSplit & " and "
                            Else
                                strwhereQuery &= "convert(datetime,convert(varchar," & sqrdView.Item("UV_VC50_COL_Value") & ",101),101)" & " " & sqrdView.Item("UV_VC5_FA") & " " & sqrdView.Item("UV_VC20_Value") & " and "
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
                End If
                'strSelect = Replace(strSelect, ",", " ", , 1, CompareMethod.Text)
                If shJoin = 1 Then
                    strSelect &= " from T210011 a, T010011 comp,T060011 owner  where  a.PR_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number and a.PR_NU9_Owner_ID_Fk=owner.UM_IN4_Address_No_FK"
                Else
                    strSelect &= " from T210011 a, T010011 comp  where  a.PR_NU9_Comp_ID_FK = comp.CI_NU8_Address_Number "
                End If

                strSelect &= " AND PR_NU9_Comp_ID_FK IN(" & GetCompanySubQuery() & ") "

                If strwhereQuery.Equals(" ") = True Then
                Else
                    strSelect &= " and" & strwhereQuery
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
                ' SQL.DBTable = "T210011"
                If SQL.Search("T210011", "AB_Search", "Fillview-455", strSelect, dsFromView, "", "") = True Then
                    For inti As Integer = 0 To dsFromView.Tables(0).Columns.Count - 1
                        dsFromView.Tables(0).Columns(inti).ColumnName = arColumnName.Item(inti)
                        If UCase(arColumnName.Item(inti)) = "SubCategoryComp".ToUpper Then
                            mintCompId = inti
                        End If
                    Next

                    rowvalue = 0

                    mdvtable.Table = dsFromView.Tables(0)
                    SetDataTableDateFormat(mdvtable.Table, htDateCols)
                    GrdAddSerach.DataSource = mdvtable.Table
                    ' GrdAddSerach.Columns.Clear()
                    If GrdAddSerach.AutoGenerateColumns = False Then
                        GrdAddSerach.AutoGenerateColumns = True
                        GrdAddSerach.PageSize = mintPageSize
                    End If

                    If ViewState("ProjectViewName") <> ddlstview.SelectedItem.Text Then
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
                Else
                    GrdAddSerach.Visible = False
                    Panel1.Visible = False

                    lstError.Items.Clear()
                    lstError.Items.Add("SubCategory not opened so far or data not exist according to view query...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    '  ddlstview.Enabled = False
                End If

            Else
                Exit Sub
            End If
        Catch ex As Exception
            CreateLog("ProjectView", "FillView-736", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

#End Region

#Region "fill data into the dropdown from view table "
    '*******************************************************************
    ' Function             :-  GetView
    ' Purpose              :- fill value into the dropdown name and id of the field view table
    '								
    ' Date					  Author						Modification Date					Description
    ' 		                       Sachin Prashar		    -------------------					Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GetView()

        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        ddlstview.Items.Clear()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            '            SQL.DBTable = "T030201"
            SQL.DBTracing = False

            sqrdView = SQL.Search("AB_Search", "Getview-566", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='40' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)

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
            CreateLog("ProjectView", "GetView-541", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
                    If intii >= 5 And intii < 11 Then
                        col1cng = col1.Value - 3
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 11 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 1.5
                        col1cng = col1cng & "pt"
                    End If

                    arCol.Add(arSetColumnName.Item(intii))

                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arSetColumnName.Item(intii) & " runat=""server""  Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
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

                    If intii >= 5 And intii < 11 Then
                        col1cng = col1.Value - 3
                        col1cng = col1cng & "pt"
                    ElseIf intii >= 11 Then
                        col1cng = col1.Value - 2.5
                        col1cng = col1cng & "pt"
                    Else
                        col1cng = col1.Value - 1.5
                        col1cng = col1cng & "pt"
                    End If


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
            CreateLog("ProjectView", "CreateTextBox-889", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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

            For intI = 0 To arCol.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                GrdAddSerach.Columns.Add(Bound_Column)
            Next
        Catch ex As Exception
            CreateLog("ProjectView", "FormatGrid-925", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
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
                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"
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
                        strSearch = strSearch.Replace("*", "")
                        strSearch = GetSearchString(strSearch)
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
            mdvtable.RowFilter = strRowFilterString
            GetFilteredDataView(mdvtable, strRowFilterString)
            HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)
            GrdAddSerach.DataSource = mdvtable
            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            If ViewState("ProjectViewName") <> ddlstview.SelectedItem.Text Then
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
            CreateLog("ProjectView", "FillGRDAfterSearch-1213", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
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
        Dim strCompId As String
        'GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strCompId = e.Item.Cells(mintCompId).Text()

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "','" & strCompId & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & strID & ", '" & rowvalue & "','" & strCompId & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("ProjectView", "ItemDataBound-1001", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try
    End Sub

#End Region

    Private Sub ddlstview_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlstview.SelectedIndexChanged

        ViewState("ProjectViewName") = ddlstview.SelectedItem.Text
        ViewState("ProjectViewValue") = ddlstview.SelectedValue

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
        If IsNothing(ViewState("SortOrder_SubCategory")) = False Then
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
        If IsNothing(ViewState("SortOrder_SubCategory")) = False Then
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

        If IsNothing(ViewState("SortOrder_SubCategory")) = False Then
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

        If IsNothing(ViewState("SortOrder_SubCategory")) = False Then
            SortGRDDuplicate()
        End If

    End Sub
    Private Sub SortGRD()

        ' If SortWay Mod 2 = 0 Then
        If Val(ViewState("SortWay_SubCategory")) Mod 2 = 0 Then
            mdvtable.Sort = ViewState("SortOrder_SubCategory") & " ASC"
        Else
            mdvtable.Sort = ViewState("SortOrder_SubCategory") & " DESC"
        End If

        '   SortWay += 1
        ViewState("SortWay_SubCategory") += 1

        If GrdAddSerach.AutoGenerateColumns = False Then
            GrdAddSerach.AutoGenerateColumns = True
        End If
        rowvalue = 0

        GrdAddSerach.DataSource = mdvtable
        GrdAddSerach.DataBind()


        '  GridRowSelection()

    End Sub
    Private Sub SortGRDDuplicate()

        Try

            ' If SortWay Mod 2 = 0 Then
            If Val(ViewState("SortWay_SubCategory")) Mod 2 = 0 Then
                mdvtable.Sort = ViewState("SortOrder_SubCategory") & " DESC"
            Else
                mdvtable.Sort = ViewState("SortOrder_SubCategory") & " ASC"
            End If

            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If

            rowvalue = 0

            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.DataBind()

            ' GridRowSelection()

        Catch ex As Exception
        End Try

    End Sub

    Private Sub GrdAddSerach_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddSerach.SortCommand

        ViewState("SortOrder_SubCategory") = e.SortExpression
        ' StrSortOrder = e.SortExpression
        SortGRD()

    End Sub

    Private Sub GrdAddSerach_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemCreated
        Try
            Dim intA As Integer = 0
            For intI = 0 To arColWidth.Count - 1 + 2
                If intI > 1 Then
                    If e.Item.Cells.Count > 1 Then
                        ' e.Item.Cells(intA + 2).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                        e.Item.Cells(intA).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")
                    End If
                    intA += 1
                ElseIf intI = 0 Then
                    e.Item.Cells(0).Width = System.Web.UI.WebControls.Unit.Parse(arColWidth.Item(intA) & "pt")

                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SavePageSize()
        Dim intid = 40
        Dim strCheck As String = SQL.Search("Historicview", "SavePageSize-3406", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "")

        If Not IsNothing(strCheck) Then

            'update
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            arColumnName.Add("PS_NU9_PSize")

            arRowData.Add(Val(ViewState("PageSize_SubCategory")))

            If SQL.Update("T030214", "SavePageSIZE", "update  T030214 set PS_NU9_PSize=" & Val(ViewState("PageSize_SubCategory")) & "  where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid"), SQL.Transaction.Serializable) = True Then
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

            arRowData.Add(Val(ViewState("PageSize_SubCategory")))
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
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=40 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

            If blnReturn = False Then
                Return False
                Exit Function
            Else
                While sqdrCol.Read
                    ViewState("PageSize_SubCategory") = sqdrCol.Item("PS_NU9_PSize")
                End While
                Return True
            End If

            sqdrCol.Close()
            sqdrCol = Nothing

        Catch ex As Exception
            ddlstview.SelectedValue = 0
            CreateLog("ProjectView", "ChkPageView-1309", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Sub Firstbutton_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles Firstbutton.Command

    End Sub

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
