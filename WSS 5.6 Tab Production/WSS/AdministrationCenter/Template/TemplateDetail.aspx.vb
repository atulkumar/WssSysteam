'************************************************************************************************************
' Page                 : - Template Detail
' Purpose              : - it will contain the Template details like name of template, Template Type,                                   Customer, call type & Template description.
' Tables used          : - T050031, T050041, T050051, T050021, UDC, T010011, T040081, t080011, t060011,                                 T210011

' Date					Author						Modification Date					Description
' 31/07/06				Harpreet					-------------------					Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Data
Imports System.Drawing

Partial Class AdministrationCenter_Template_TemplateDetail
    Inherits System.Web.UI.Page
#Region "Form level Variables"

    Dim mstrCompany As String
    Dim mintID As Integer
    Dim mintFileID As Integer
    Dim mstrFileName As String
    Dim mstrFilePath As String
    Dim mshFlag As Short
    Dim Null As System.DBNull
    Dim arrImageUrlEnabled As New ArrayList  ' Used to store Enabled Image Urls of task Grid
    Dim arrImageUrlDisabled As New ArrayList ' Used to store Disabled Image Urls of task Grid
    Dim arrImageUrlNew As New ArrayList 'Used to store new comments
    Dim mTaskRowValue As Integer
    Dim mActionRowValue As Integer
    Dim shUpdateSave As Short
    Protected WithEvents ImgPlus As System.Web.UI.HtmlControls.HtmlImage
    Public introwvalues As String
    Public strhiddenTable As String

    Public mintPageSize As Integer
    Public mstrCallNumber As String

    Private tclTask() As TemplateColumn
    'Private tclAction() As TemplateColumn
    Private Shared dtvTask As New DataView

    Protected WithEvents dtgTask As New System.Web.UI.WebControls.DataGrid

    Private Shared arrHeadersTask As New ArrayList
    Private Shared arrFooterTask As New ArrayList
    Private Shared arrColumnsNameTask As New ArrayList
    Private Shared arrWidthTask As New ArrayList
    Private Shared arrColumnsWidthTask As New ArrayList
    'Action grid variables
    Private Shared arrHeadersAction As New ArrayList
    Private Shared arrFooterAction As New ArrayList
    Private Shared dtvAction As New DataView
    Private Shared arrColumnsNameAction As New ArrayList
    Private Shared arrWidthAction As New ArrayList
    Private Shared arrColumnsWidthAction As New ArrayList
    Dim mintUserID As Integer
    Private Shared mintCallNoPlace As Integer
    Private Shared mintTemplateNumber As Integer

    Private blnSubCategoryCrossStatus As Boolean = False
    Private intDefaultUserId As Integer

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            '        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            txtTmplDesc.Attributes.Add("onmousemove", "ShowToolTip(this,200);")
            txtDescription.Attributes.Add("onmousemove", "ShowToolTip(this,2000);")
            imgbtnSearch.Attributes.Add("onclick", "return CheckLength();")
            TxtEstimatedHrs.Attributes.Add("onkeypress", "UsedHour('" & TxtEstimatedHrs.ClientID & "')")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            'ImgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgAttachments.Attributes.Add("Onclick", "return SaveEdit('Attach');")
            DDLProject.Attributes.Add("Onchange", "ForcedPostBack();") ' -- This attribute will fill 'forced' in txthiddenImage onchange 
            DDLCustomer.Attributes.Add("Onchange", "ForcedPostBack();") ' -- This attribute will fill 'forced' in txthiddenImage onchange 
        End If
        Call txtCSS(Me.Page, "cpnlCallTask", "cpnlTaskAction")
        If Request.QueryString("AddressNo") <> "-1" Then
            ViewState("SAddressNumber_Template") = Request.QueryString("AddressNo")
        End If

        If IsPostBack = False Then
            If ViewState("SAddressNumber_Template") = "-1" Or ViewState("SAddressNumber_Template") Is Nothing Then
                ViewState("PropCAComp") = Session("PropCompanyID")
                DDLCopyTemplate.Enabled = True
            Else
                ViewState("PropCAComp") = SQL.Search("", "", "select TL_NU9_CustID_FK from T050011 where TL_NU9_ID_PK=" & ViewState("SAddressNumber_Template"), "")
                DDLCopyTemplate.Enabled = False
            End If

            If Session("PropCompanyType") = "SCM" Then
                FillNonUDCDropDown(DDLCustomer, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from  T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' AND CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ")")
            Else
                DDLCustomer.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                DDLCustomer.Enabled = False
            End If
            DDLCustomer.SelectedValue = ViewState("PropCAComp")
            FillNonUDCDropDown(DDLCopyTemplate, "select TL_NU9_ID_PK TemplateID, ('[' + TL_VC8_Tmpl_Type + '] ' + TL_VC30_Template) TemplateName from T050011 where TL_NU9_CustID_FK=" & Val(DDLCustomer.SelectedValue) & " order by TL_VC8_Tmpl_Type", True)
            ' -- Fill SubCategory on the basis of customer
            FillNonUDCDropDown(DDLProject, "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_VC8_Status='Enable' and PR_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " order by PR_VC20_Name", True)
        End If

        '-----------------------------------------
        If IsPostBack = False Then
            FillCustomDDl()
        End If

        '------------------------------------------------------------

        Dim shRedirect As Short
        Dim strFilter As String     ' Used to Store Where Condition for Task grid
        Dim strSearch As String    ' Used to store Value of Search Control
        Dim strSqlFilterAction As String    ' Used to Store Where Condition for Task grid
        Dim intCount As Int32 ' Used as counter in loops
        Dim lcTask As New LiteralControl ' Used for displaying labels inplace of Task Grid 
        Dim lcAction As New LiteralControl ' Used for displaying labels inplace of Action Grid
        If Request.QueryString("ID") = -1 Then ImgPlus.Visible = False


        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        strFilter = " "
        strSqlFilterAction = " "
        mActionRowValue = 0
        mTaskRowValue = 0


        If IsPostBack = False Then
            Try

                ViewState("SortWayTask") = Nothing
                ViewState("SortOrderTask") = Nothing

                shUpdateSave = Request.QueryString("CV")
                ViewState("PropTaskNumber") = 0
                ViewState("PropActionNumber") = 0
                ViewState("PropCallNumber") = 0
                txtTmplBy.Text = HttpContext.Current.Session("PropUserID").ToString
                txtTmplByName.Text = WSSSearch.SearchUserName(HttpContext.Current.Session("PropUserID")).ExtraValue

            Catch ex As Exception
                CreateLog("TemplateDetail", "Load-234", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        '--Dependency after grid bind
        If shUpdateSave = 1 Then
            ViewState("gshPageStatus") = 0
        End If

        mTaskRowValue = 0

        '************Changed by amit************
        strhiddenTable = Request.Form("txthiddenTable")
        introwvalues = Request.Form("txtrowvalues")

        mstrCallNumber = ViewState("PropCallNumber")

        If strhiddenTable = "cpnlCallTask_dtgTask" Then
            ViewState("PropTaskNumber") = Val(Request.Form("txthiddenSkil"))
            mstrCallNumber = ViewState("PropTaskNumber")
        Else
            ViewState("PropActionNumber") = Val(Request.Form("txthiddenSkil"))
            mstrCallNumber = ViewState("PropActionNumber")
        End If

        '***********************************************
        mshFlag = 0
        If ViewState("gshPageStatus") = 1 Then
            mintID = 0
        Else
            mintID = Request.QueryString("ID")
        End If

        Dim txthiddenImage As String = Request.Form("txthiddenImage")

        If ViewState("SAddressNumber_Template") = -1 Or ViewState("SAddressNumber_Template") Is Nothing Then
            CDDLStatus.CDDLSetSelectedItem("OPEN", True)
            CDDLStatus.Enabled = False
            ViewState("PropActionNumber") = 0
            ViewState("PropTaskNumber") = 0
            ViewState("PropCallNumber") = 0
            txtTmplDate.Text = DateTime.Now.ToShortDateString
            cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
        Else
            If ViewState("PropCallNumber") > 0 And txthiddenImage <> "forced" Then
                CDDLStatus.CDDLSetSelectedItem("ASSIGNED", True)
            End If
            mintTemplateNumber = ViewState("SAddressNumber_Template")
        End If
        lstError.Items.Clear()
        'cpnlError.Visible = False
        ' --Calling Task Functions
        Call CreateDataTableTask(strFilter)
        'If dtvTask.Table.Rows.Count > 0 Then
        Call CreateGridTask()
        Call FillHeaderArrayTask()
        Call FillFooterArrayTask()
        Call createTemplateColumnsTask()
        Call BindGridTask()

        If ViewState("SAddressNumber_Template") > 0 And Val(ViewState("PropCallNumber")) > 0 Then
            DDLCustomer.Enabled = False
        End If
        If txthiddenImage <> "" Then
            lstError.Items.Clear()
            Try
                Select Case txthiddenImage

                    Case "Fwd"

                    Case "Edit"
                        If strhiddenTable = "cpnlCallTask_dtgTask" Then
                            Exit Select
                        End If
                    Case "Close"
                        ViewState("gshPageStatus") = 0
                        If Request.QueryString("PageID") = 1 Then
                            Response.Redirect("Call_View.aspx?ScrID=4")
                        ElseIf Request.QueryString("PageID") = 2 Then
                            Response.Redirect("Task_View.aspx")
                        ElseIf Request.QueryString("PageID") = 3 Then
                            Response.Redirect("../../WorkCenter/DoList/ToDoList.aspx?ScrID=8")
                        ElseIf Request.QueryString("PageID") = 4 Then
                            Response.Redirect("../../Home.aspx")
                        Else
                            Response.Redirect("Template.aspx")
                        End If
                    Case "Add"
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            ' cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If mshFlag = 1 Then
                            Exit Select
                        Else
                            If ViewState("SAddressNumber_Template") = -1 Or ViewState("SAddressNumber_Template") Is Nothing Then
                                If SaveTemplate() = True Then
                                    If mintTemplateNumber > 0 And (CDDLTemplateType.CDDLGetValue.Trim = "CAO" Or CDDLTemplateType.CDDLGetValue.Trim = "CNT") Then
                                    End If
                                End If
                            Else
                                If UpdateTemplate() = True Then
                                    If mintTemplateNumber > 0 And (CDDLTemplateType.CDDLGetValue.Trim = "CAO" Or CDDLTemplateType.CDDLGetValue.Trim = "CNT") Then
                                        If SaveCall() = True Then
                                            shRedirect = 1
                                        End If
                                    End If
                                End If
                            End If

                            If ViewState("SAddressNumber_Template") > 0 And (CDDLTemplateType.CDDLGetValue.Trim = "TAO" Or CDDLTemplateType.CDDLGetValue.Trim = "CNT") Then
                                If SaveTask() = True Then
                                    DisplayMessage("Records Saved successfully...")
                                    Response.Redirect("Template.aspx", False)
                                Else
                                    If shRedirect = 1 Then
                                        Dim focusScript As String = "<script language='javascript'>" & _
                                        "window.parent.closeTab();""</script>"
                                        ' Add the JavaScript code to the page.
                                        Page.RegisterStartupScript("FocusScript", focusScript)
                                        'Response.Redirect("Template.aspx", False)
                                    End If
                                    'Exit Select
                                End If
                            End If
                        End If
                        ' Response.Redirect("Template.aspx", False)
                    Case "Save"

                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If ViewState("SAddressNumber_Template") = -1 Or ViewState("SAddressNumber_Template") Is Nothing Then
                            If SaveTemplate() = False Then
                                Exit Sub
                            End If

                        Else
                            If UpdateTemplate() = False Then
                                Exit Select
                            Else
                                If ViewState("SAddressNumber_Template") > 0 And (CDDLTemplateType.CDDLGetValue.Trim = "CAO" Or CDDLTemplateType.CDDLGetValue.Trim = "CNT") Then
                                    If SaveCall() = True Then
                                        Exit Select
                                    ElseIf (CDDLTemplateType.CDDLGetValue.Trim = "TAO" Or CDDLTemplateType.CDDLGetValue.Trim = "CNT") Then
                                        If SaveTask() = True Then
                                            lstError.Items.Clear()
                                            DisplayMessage("Records Saved successfully...")
                                            'garFileID.Clear()
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    Case "Delete"
                        '**************************************
                        If strhiddenTable = "cpnlCallTask_dtgTask" Then


                            'Added by Atul
                            Dim intTaskOrder As Integer
                            intTaskOrder = SQL.Search("", "", "select TTM_NU9_Task_Order from T050031  where TTM_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & "and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & ViewState("PropCallNumber") & " and TTM_NU9_Task_no_PK=" & ViewState("PropTaskNumber"))


                            Call ChangeTaskOrder1(mdlMain.EnumTaskOrder.DeleteTask, intTaskOrder)
                            ' Check that task is not in progress
                            mstGetFunctionValue = WSSDelete.DeleteTemplateTask(ViewState("SAddressNumber_Template"), ViewState("PropTaskNumber"))


                            '--Dependency
                            FillNonUDCDropDown(DDLDependancy_F, "select TTM_NU9_Task_No_Pk,TTM_NU9_Task_No_Pk from T050031 where TTM_NU9_Call_No_FK=" & ViewState("PropCallNumber") & " and Ttm_nu9_comp_id_fk =" & ViewState("PropCAComp") & " and TTM_NU9_TemplateID_FK=" & mintTemplateNumber, True)


                            If mstGetFunctionValue.ErrorCode = 0 Then


                                '''''''''''Rollback Call Status'''''''''''
                                Dim intRows As Integer
                                If SQL.Search("Template", "Load-506", "select * from T050031 where TTM_NU9_Call_No_FK=" & ViewState("PropCallNumber") & " and TTM_NU9_Comp_ID_FK=" & ViewState("PropCAComp"), intRows) = False Then
                                    SQL.Update("", "", "update T050021 set TCM_VC20_Call_Status='OPEN'where TCM_NU9_Call_No_PK=" & ViewState("PropCallNumber") & " and TCM_NU9_CompId_FK=" & ViewState("PropCAComp"), SQL.Transaction.Serializable, "")
                                    DDLProject.Enabled = True
                                End If

                                lstError.Items.Clear()
                                lstError.Items.Add("Task Deleted successfully...")
                                ''ImgError.ImageUrl = "../../Images/Pok.gif"
                                MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                                '  cpnlError.Visible = True
                                introwvalues = 0
                                mstrCallNumber = 0



                            ElseIf mstGetFunctionValue.ErrorCode = 1 OrElse mstGetFunctionValue.ErrorCode = 2 Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Server is busy please try later...")
                            End If
                        End If
                        '***************************************
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                    Case "Attach"

                End Select
            Catch ex As Exception
                'ImgError.ImageUrl = "../../images/error_image.gif"
                MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                CreateLog("TemplateDetail", "Load-509", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            End Try
        End If

        strFilter = ""
        ' Check whether form is called for Edit or New entry
        ' Add new records
        If ViewState("SAddressNumber_Template") > 0 And (CDDLTemplateType.CDDLGetValue.Trim.ToUpper = "TAO" Or CDDLTemplateType.CDDLGetValue.Trim.ToUpper = "CNT") Then
            'recreate Task Query and bind the grid
            Call FillTemplate()
            Call CreateDataTableTask(strFilter)

            Call BindGridTask()

            cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            cpnlCallTask.TitleCSS = "test2"

        ElseIf ViewState("SAddressNumber_Template") > 0 Then

            mstrCompany = Val(DDLCustomer.SelectedValue) '' ViewState("PropCAComp")
            cpnlCallTask.Enabled = True
            cpnlCallTask.TitleCSS = "test"
            If IsPostBack = False Then
                Call FillTemplate()
            End If
        End If
        ' Get the information of the call
        Try
            If ViewState("SAddressNumber_Template") > 0 Then
                If SaveTask() = True Then    ' Save Task Info 
                    If shRedirect = 1 Then
                        Response.Redirect("Template.aspx")
                    End If
                    Call ClearAllTextBox(cpnlCallTask)
                End If
            End If

            Dim sqrdCall As SqlDataReader

            mstGetFunctionValue = WSSSearch.SearchTemplateCall(ViewState("SAddressNumber_Template"), mstrCompany, sqrdCall)

            If mstGetFunctionValue.ErrorCode = 0 And txthiddenImage <> "forced" Then
                While sqrdCall.Read
                    DDLCustomer.Enabled = False

                    CDDLCallOwner.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Call_Owner")), "", sqrdCall.Item("TCM_NU9_Call_Owner")), False, IIf(IsDBNull(sqrdCall.Item("UM_VC50_UserID")), "", sqrdCall.Item("UM_VC50_UserID")))
                    ' CDDLCoordinator.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Coordinator")), "", sqrdCall.Item("TCM_NU9_Coordinator")), False, IIf(IsDBNull(sqrdCall.Item("Coordinator")), "", sqrdCall.Item("Coordinator")))
                    CDDLCoordinator.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Coordinator")), "", sqrdCall.Item("TCM_NU9_Coordinator")), False, IIf(IsDBNull(sqrdCall.Item("Coordinator")), "", sqrdCall.Item("Coordinator")))
                    CDDLCategory.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Category")), "", sqrdCall.Item("TCM_VC8_Category")), True, IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Category")), "", sqrdCall.Item("TCM_VC8_Category")))
                    CDDLCauseCode.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Cause_Code")), "", sqrdCall.Item("TCM_VC8_Cause_Code")), True, IIf(IsDBNull(sqrdCall.Item("TCM_VC8_Cause_Code")), "", sqrdCall.Item("TCM_VC8_Cause_Code")))
                    txtCustomer.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_CompId_FK")), "", sqrdCall.Item("TCM_NU9_CompId_FK"))
                    txtCustomerName.Text = IIf(IsDBNull(sqrdCall.Item("CI_VC36_Name")), "", sqrdCall.Item("CI_VC36_Name"))
                    txtDescription.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC2000_Call_Desc")), "", sqrdCall.Item("TCM_VC2000_Call_Desc"))
                    CDDLPriority.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC200_Work_Priority")), "", sqrdCall.Item("TCM_VC200_Work_Priority")))
                    DDLProject.SelectedValue = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Project_ID")), "", sqrdCall.Item("TCM_NU9_Project_ID"))
                    'Fill the task Owner DropDown
                    txtReference.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC50_Reference_Id")), "", sqrdCall.Item("TCM_VC50_Reference_Id"))
                    'Added on 06-07-09
                    txtCateCode1.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_1")), "", sqrdCall.Item("TCM_NU9_Category_Code_1"))
                    txtCateCode2.Text = IIf(IsDBNull(sqrdCall.Item("TCM_NU9_Category_Code_2")), "", sqrdCall.Item("TCM_NU9_Category_Code_2"))

                    txtSubject.Text = IIf(IsDBNull(sqrdCall.Item("TCM_VC100_Subject")), "", sqrdCall.Item("TCM_VC100_Subject"))
                    CDDLStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqrdCall.Item("TCM_VC20_Call_Status")), "", sqrdCall.Item("TCM_VC20_Call_Status")))
                    '-----------------------------------------------------------------------------------------------------------
                    'If the status of the call is not OPEN then user cannot change the Agreement and SubCategory of the call
                    'By Harpreet
                    If Not IsDBNull(sqrdCall.Item("TCM_VC20_Call_Status")) Then
                        If sqrdCall.Item("TCM_VC20_Call_Status") <> "OPEN" Then
                            DDLProject.Enabled = False
                        End If
                    End If
                    ''-----------------------------------------------------------------------------------------------------------
                    ViewState("PropCallNumber") = sqrdCall.Item("TCM_NU9_Call_No_PK")
                    'CDDLPriority_F.CDDLSetSelectedItem(CDDLPriority.CDDLGetValue)
                    '*************************
                End While
                sqrdCall.Close()
            Else
                ViewState("PropCallNumber") = GetCallNumber(ViewState("SAddressNumber_Template"))
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "Load-579", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try


        Try

            If Page.IsPostBack Then

                '-- Preparing Search  String (strFilter) for Task Grid
                For intCount = 2 To dtvTask.Table.Columns.Count - 1
                    strSearch = Request.Form("cpnlCallTask$dtgTask$ctl01$" + dtvTask.Table.Columns(intCount).ColumnName + "_H")
                    If IsNothing(strSearch) = False Then
                        If Not IsDBNull(strSearch) Then
                            If Not strSearch.Trim.Equals("") Then
                                ' -- Format Search String
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
                    End If
                Next

            End If

            If Not strFilter.Trim.Equals("") Then
                strFilter = strFilter.Remove((strFilter.Length - 4), 4)
            End If

            'recreate Task Query and bind the grid
            Call CreateDataTableTask(strFilter)
            Call BindGridTask()
            'Dependency  Combo

            If IsNothing(ViewState("SortOrderTask")) = False Then
                SortGRDDuplicateTask()
            End If


            If Not IsPostBack Then
                FillNonUDCDropDown(DDLDependancy_F, "select TTM_NU9_Task_No_Pk,TTM_NU9_Task_No_Pk from T050031 where TTM_NU9_Call_No_FK=" & ViewState("PropCallNumber") & " and Ttm_nu9_comp_id_fk =" & ViewState("PropCAComp") & " and TTM_NU9_TemplateID_FK=" & mintTemplateNumber, True)

            End If
            '-----------------------------------------
            Call EnableDisablePanel()

        Catch ex As Exception
            CreateLog("TemplateDetail", "Load-677", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try


        'Security Block

        Dim intId As Integer

        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            'intId = Request.QueryString("ScrID")
            intId = 419
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If

        If CDDLStatus.CDDLGetValueName <> "OPEN" Then
            DDLProject.Enabled = False
        End If
        ''-----------------------------------------------------------------------------------------------------------
    End Sub

    Private Function SaveCall() As Boolean

        lstError.Items.Clear()
        If ValidateRecords() = False Then
            Exit Function
        End If
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            arColumnName.Add("TCM_NU9_CompID_FK")
            arColumnName.Add("TCM_VC8_Call_Type")
            arColumnName.Add("TCM_NU9_Call_Owner")
            arColumnName.Add("TCM_VC200_Work_Priority")
            arColumnName.Add("TCM_VC50_Reference_Id")
            'Added on 06-07-09
            arColumnName.Add("TCM_NU9_Category_Code_1")
            arColumnName.Add("TCM_NU9_Category_Code_2")

            arColumnName.Add("TCM_VC100_Subject")
            arColumnName.Add("TCM_VC2000_Call_Desc")
            arColumnName.Add("TCM_NU9_Project_ID")
            arColumnName.Add("TCM_NU9_CustID_FK")
            arColumnName.Add("TCM_NU9_TemplateID_FK")
            arColumnName.Add("TCM_VC8_Template")
            arColumnName.Add("TCM_VC8_Tmpl_Type")
            arColumnName.Add("TCM_VC8_Category")
            arColumnName.Add("TCM_VC8_Cause_Code")
            arColumnName.Add("TCM_NU9_Coordinator")


            arRowData.Add(DDLCustomer.SelectedValue)
            arRowData.Add(CDDLCallType.CDDLGetValue.Trim.ToUpper)
            If CDDLCallOwner.CDDLGetValue.Trim = "" Then
                arRowData.Add("1")
            Else
                arRowData.Add(CDDLCallOwner.CDDLGetValue.Trim)
            End If
            arRowData.Add(CDDLPriority.CDDLGetValue.Trim.ToUpper)
            arRowData.Add(txtReference.Text.Trim)
            'Added on 06-07-09
            arRowData.Add(txtCateCode1.Text.Trim)
            arRowData.Add(txtCateCode2.Text.Trim)

            arRowData.Add(txtSubject.Text.Trim)
            arRowData.Add(txtDescription.Text.Trim)
            ' arRowData.Add(txtProject.Text.Trim)
            arRowData.Add(DDLProject.SelectedValue)
            arRowData.Add(ViewState("PropCAComp"))
            arRowData.Add(ViewState("SAddressNumber_Template"))
            arRowData.Add(txtTmplName.Text)
            arRowData.Add(CDDLTemplateType.CDDLGetValue)
            arRowData.Add(CDDLCategory.CDDLGetValue)
            arRowData.Add(CDDLCauseCode.CDDLGetValue)
            If CDDLCoordinator.CDDLGetValue = "" Then
                arRowData.Add(DBNull.Value)
            Else
                arRowData.Add(CDDLCoordinator.CDDLGetValue)
            End If

            mstGetFunctionValue = WSSSave.SaveTemplateCall(arColumnName, arRowData, ViewState("PropCAComp"), ViewState("SAddressNumber_Template"))
            Dim shReturn As Short

            If mstGetFunctionValue.ErrorCode = 0 Then
                'ImgError.ImageUrl = "../../images/Pok.gif"
                MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                cpnlCallTask.Enabled = True
                'For intI As Integer = 0 To garFileID.Count - 1
                If GetFiles(mdlMain.AttachLevel.CallLevel) = True Then
                    shReturn = 1
                Else
                    shReturn = 2
                End If
                garFileID.Clear()
                If shReturn = 1 Then
                    MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    lstError.Items.Add("Record Saved Successfully...")
                    ViewState("gshPageStatus") = 1
                    mshFlag = 0
                    lstError.ForeColor = Color.Black
                    cpnlCallTask.Enabled = True
                    cpnlCallTask.TitleCSS = "test"
                    'Change Open Status from Add to Edit
                    mintID = 0
                    Return True
                ElseIf shReturn = 2 Then
                    ViewState("gshPageStatus") = 1
                    lstError.Items.Add("Record Saved Successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    SQL.Delete("TemplateDetail", "SaveCall-900", "delete from T050041 where AT_NU9_Call_Number=" & ViewState("PropCallNumber") & " and AT_VC255_File_Name='" & mstrFileName & "' and AT_VC1_Status='T'", SQL.Transaction.Serializable)
                    mshFlag = 0
                    Return True
                Else
                    lstError.Items.Add("Record Saved Successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    ViewState("gshPageStatus") = 1
                    cpnlCallTask.Enabled = True
                    cpnlCallTask.TitleCSS = "test"
                    mshFlag = 0
                    'Fill task fields according to call 
                    '*********************************
                    'CDDLPriority_F.CDDLSetSelectedItem(CDDLPriority.CDDLGetValue)
                    '************************************
                    Return True
                End If
            End If

        Catch ex As Exception
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("TemplateDetail", "SaveCall-923", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return False
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Function

    Private Function ValidateRecords() As Boolean
        Try

            lstError.Items.Clear()

            If txtSubject.Text.Trim.Equals("") Then
                lstError.Items.Add("Subject cannot be blank...")
                mshFlag = 1
            End If

            If txtDescription.Text.Trim.Equals("") Then
                lstError.Items.Add("Description cannot be blank...")
                mshFlag = 1
            End If
            If DDLProject.SelectedIndex < 1 Then
                lstError.Items.Add("SubCategory cannot be blank...")
                mshFlag = 1
            End If

            If CDDLCallOwner.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Requested By cannot be blank...")
                mshFlag = 1
            End If

            If CDDLCallType.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Call Type cannot be blank...")
                mshFlag = 1
            End If

            If CDDLPriority.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Priority  cannot be blank...")
                mshFlag = 1
            End If

            If mshFlag = 1 Then
                'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If

            Dim strErrorMessage As String
            Dim strUDCType As String
            Dim strName As String

            Dim intAddressNo As Integer

            intAddressNo = SQL.Search("TemplateDetail", "ValidateRecords-987", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & CDDLCallOwner.CDDLGetValue.Trim & "")
            If intAddressNo <= 0 Then
                lstError.Items.Add("Requested By mismatch...")
                mshFlag = 1
            End If

            intAddressNo = SQL.Search("TemplateDetail", "ValidateRecords-993", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & txtTmplBy.Text.Trim & "")
            If intAddressNo <= 0 Then
                lstError.Items.Add("Value in Call by mismatch... ")
                mshFlag = 1
            End If

            If mshFlag = 1 Then
                'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgWarning)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "ValidateRecords-1245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

    Private Function GetFiles2(ByVal Level As AttachLevel) As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T050041"
            SQL.DBTracing = False

            Dim blnRead As Boolean

            Select Case Level
                ' For Calls
                Case AttachLevel.CallLevel

                    sqrdTempFiles = SQL.Search("TemplateDetail", "GetFiles-1048", "select * from T050041 Where AT_IN4_Level=1 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            If CreateFolder(ViewState("PropCallNumber")) = True Then
                                Return True
                            Else
                                Return False
                            End If
                        End While
                    Else
                        Return False
                    End If

                    ' For Tasks
                Case AttachLevel.TaskLevel
                    sqrdTempFiles = SQL.Search("TemplateDetail", "GetFiles-1066", "select * from T050041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("PropCallNumber"), ViewState("PropTaskNumber"))
                        End While
                    Else
                        Return False
                    End If

                    ' For Actions
                Case AttachLevel.ActionLevel
                    sqrdTempFiles = SQL.Search("TemplateDetail", "GetFiles-1082", "select * from T050041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("PropCallNumber"), ViewState("PropTaskNumber"), ViewState("PropActionNumber"))
                        End While
                    Else
                        Return False
                    End If
            End Select
        Catch ex As Exception
            CreateLog("TemplateDetail", "ValidateRecords-1245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function
    Private Function GetFiles(ByVal Level As AttachLevel) As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T050041"
            SQL.DBTracing = False

            Dim blnRead As Boolean

            Select Case Level
                ' For Calls
                Case AttachLevel.CallLevel

                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("", "", "select * from T050041 Where AT_IN4_Level=1 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            If CreateFolder(ViewState("PropCallNumber")) = True Then
                                shAttachments = 1
                                'Return True
                            Else
                                shAttachments = 2
                                'Return False
                            End If
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    ' For Tasks
                Case AttachLevel.TaskLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Template Detail", "GetFiles-1154", "select * from T050041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("PropCallNumber"), ViewState("PropTaskNumber"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    ' For Actions
                Case AttachLevel.ActionLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Template Detail", "GetFiles-1175", "select * from T050041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("PropCAComp")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("PropCallNumber"), ViewState("PropTaskNumber"), ViewState("PropActionNumber"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
            End Select
        Catch ex As Exception
            CreateLog("TemplateDetail", "ValidateRecords-1245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../TemplateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("PropCAComp") & "\" & CallNo)

        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            SQL.Update("TemplateDetail", "CreateFolder-1141", "update T050021 set TCM_NU8_Attach_No=1 WHERE TCM_NU9_Call_No_PK=" & CallNo & " and TCM_NU9_CompID_FK=" & ViewState("PropCAComp") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("TemplateDetail", "CreateFolder-1160", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                Try
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
                Catch ex As Exception
                End Try
                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            SQL.Update("TemplateDetail", "CreateFolder-1188", "update T050021 set TCM_NU8_Attach_No=1 WHERE TCM_NU9_Call_No_PK=" & CallNo & " and TCM_NU9_CompID_FK=" & ViewState("PropCAComp") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            SQL.Update("TemplateDetail", "CreateFolder-1201", "update t050021 set TCM_NU8_Attach_No=1 WHERE TCM_NU9_Call_No_PK=" & CallNo & " and TCM_NU9_CompID_FK=" & ViewState("PropCAComp") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateFolder-1201", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
            Return False
        End Try
    End Function

#Region "Create Task Grid"

    Private Sub CreateGridTask()
        Dim lc1 As New LiteralControl
        Dim lc2 As New LiteralControl
        Try
            dtgTask.ID = "dtgTask"
            dtgTask.DataKeyField = "TTM_NU9_Task_no_PK"
            Call FormatGridTask()

            PlaceHolder1.Controls.Add(dtgTask)
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateGridTask-1531", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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
            CreateLog("TemplateDetail", "FormatGridTask-1555", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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
            'arrColumnsNameTask.Add("EstClDate")
            arrColumnsNameTask.Add("EHr")
            arrColumnsNameTask.Add("Act")

            'arrColumnsNameTask.Add("Prio")

            arrWidthTask.Clear()

            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(17)
            arrWidthTask.Add(70)
            arrWidthTask.Add(349)
            arrWidthTask.Add(64)
            arrWidthTask.Add(72)
            arrWidthTask.Add(40)
            'arrWidthTask.Add(88)
            arrWidthTask.Add(33)
            arrWidthTask.Add(24)
            'arrWidthTask.Add(57)


            dtgTask.Width = Unit.Pixel(743)
            arrColumnsWidthTask.Clear()
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(0)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(1)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(2)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(3)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(4)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(5)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(6)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(7)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(8)))
            '  arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(9)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(9)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(10)))
            arrColumnsWidthTask.Add(System.Web.UI.WebControls.Unit.Pixel(arrWidthTask(11)))

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

            For intCount = 3 To dtvTask.Table.Columns.Count - 1
                tclTask(intCount + 1) = New TemplateColumn
                tclTask(intCount + 1).ItemTemplate = New IONGrid.CreateItemTemplateLabel(dtvTask.Table.Columns(intCount).ToString, dtvTask.Table.Columns(intCount).ToString)
                Dim AddEventOnGrigHeader As New IONGrid.CreateItemTemplateTextBoxForHeader(dtvTask.Table.Columns(intCount).ToString, "", dtvTask.Table.Columns(intCount).ToString + "_H", False, arrColumnsNameTask(intCount), True)
                AddHandler AddEventOnGrigHeader.OnSort, AddressOf SOrtGrid
                tclTask(intCount + 1).HeaderTemplate = AddEventOnGrigHeader
                tclTask(intCount + 1).FooterTemplate = New IONGrid.CreateItemTemplateTextBox("", dtvTask.Table.Columns(intCount).ToString + "_F", False)
                tclTask(intCount + 1).ItemStyle.Width = arrColumnsWidthTask(intCount)    'System.Web.UI.WebControls.Unit..Pixel(arrColumnsWidthTask(intCount))
                dtgTask.Columns.Add(tclTask(intCount + 1))
            Next

            Call ChangeTextBoxWidth()
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateTemplateColumnsTask-1661", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
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

            strSql = "select TTM_CH1_Comment as Blank1, TTM_CH1_Attachment as Blank2,TTM_CH1_Forms as Blank3,TTM_NU9_Task_Order, TTM_NU9_Task_no_PK,TTM_VC50_Deve_Status, TTM_VC1000_Subtsk_Desc, TtM_VC8_task_type,  b.UM_VC50_UserID+'~'+convert(varchar(8),a.TTM_VC8_Supp_Owner) as UM_VC50_UserID,TTM_NU9_Dependency," & _
             " TTM_FL8_Est_Hr, TTM_CH1_Mandatory  From T050031 a,T060011  b  Where TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template") & " and b.UM_IN4_Address_No_FK=a.TTM_VC8_Supp_Owner"
            strSql = strSql & " Order By TTM_NU9_Task_Order asc"
            Call SQL.Search("T050031", "TemplateDetail", "CreatedateTableTask-1362", strSql, dsTask, "sachin", "Prashar")
            dtvTask = New DataView
            dtvTask.Table = dsTask.Tables(0)

            If Not strWhereClause.Trim.Equals("") Then
                GetFilteredDataView(dtvTask, strWhereClause)
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

            If dtvTask.Table.Rows.Count > 0 Then
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                cpnlCallTask.TitleCSS = "test"
            Else
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallTask.TitleCSS = "test2"
            End If
            If IsNothing(strWhereClause) Then
                strWhereClause = ""
            End If
            If strWhereClause.Trim <> "" Then
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                cpnlCallTask.TitleCSS = "test"
            End If
            Call EnableDisablePanel()
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateDataTableTask-1386", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub
#End Region

#Region "Fill Task Header Array"
    Private Sub FillHeaderArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        Try
            arrHeadersTask.Clear()
            If Page.IsPostBack Then
                For intCount = 0 To dtvTask.Table.Columns.Count - 1
                    arrHeadersTask.Add(Request.Form("cpnlCallTask$dtgTask$ctl01$" + dtvTask.Table.Columns(intCount).ColumnName + "_H"))
                Next
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "FillHeaderArrayTask-1773", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub
#End Region

#Region "Fill Task Footer Array"
    Private Sub FillFooterArrayTask()
        Dim t As New Control
        Dim intCount As Integer
        Dim intFooterIndex As Integer
        arrFooterTask.Clear()
        If Page.IsPostBack Then
            If Not dtvTask.Table Is Nothing Then
                For intCount = 0 To dtvTask.Table.Columns.Count - 1
                    intFooterIndex = dtvTask.Count + 2    'dtgrt.Controls(0).Controls.Count - 1
                    arrFooterTask.Add(Request.Form("cpnlCallTask$dtgTask$ctl" & intFooterIndex.ToString.Trim & ":" + dtvTask.Table.Columns(intCount).ColumnName + "_F"))
                Next
            End If
        End If
    End Sub
#End Region

#Region "Bind Task Grid"
    Private Sub BindGridTask()
        Try
            mTaskRowValue = 0
            'add code to align the text of grid
            Dim htGrdColumns As New Hashtable
            htGrdColumns.Add("TTM_VC1000_Subtsk_Desc", 23)

            HTMLEncodeDecode(mdlMain.Action.Encode, dtvTask, htGrdColumns)
            SetCommentFlag(dtvTask, mdlMain.CommentLevel.TemplateTaskLevel, ViewState("PropCAComp"), ViewState("PropCallNumber"), ViewState("PropTaskNumber"), ViewState("PropActionNumber"))
            dtgTask.DataSource = dtvTask
            dtgTask.DataBind()
        Catch ex As Exception
            CreateLog("TemplateDetail", "BindGridTask-1764", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub

#End Region

    Private Sub dtgTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTask.ItemDataBound
        Dim dg As DataGrid = CType(sender, DataGrid)
        Dim intCount As Integer
        Dim dv As DataView = dtvTask
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim TaskNo As String
        Dim cnt As Integer = e.Item.ItemIndex + (dg.PageSize * dg.CurrentPageIndex)
        Dim dtBound As DataTable = dtvTask.ToTable()
        Dim strSelected As String
        Dim structTempTaskOwner As Owners '-- will keep taskowners ID and Name
        dg.PageSize = 1
        Try
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dtgTask.DataKeys(0) <> 0 Then
                    For intCount = 0 To 2       'For Image Fields
                        strSelected = IIf(IsDBNull(dtBound.Rows(cnt)(intCount)), "0", dtBound.Rows(cnt)(intCount).ToString)
                        TaskNo = dtgTask.DataKeys(e.Item.ItemIndex)
                        If strSelected = "1" Then      'If comment/ attachment is there 

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlEnabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "')")

                        ElseIf strSelected = "2" Then      'If new  comment/ attachment is there 

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlNew(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "')")

                        Else       ' If no comment/attachment is attached

                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(intCount)
                            CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyImage('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'T','" & intCount & "')")

                        End If
                    Next

                    For intCount = 3 To dtvTask.Table.Columns.Count - 1           'for Others

                        If dtvTask.Table.Columns(intCount).DataType.FullName.Equals("System.DateTime") Then

                            If dtBound.Rows(cnt)(intCount).ToString Is Null Or dtBound.Rows(cnt)(intCount).ToString = "" Then
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
                                    maxchar = 0
                                Case 7
                                    maxchar = 7
                                Case 8
                                    maxchar = 10
                                Case 9
                                    maxchar = 8
                                Case 10
                                    maxchar = 4
                                Case 12, 13, 14
                                    maxchar = 5
                                Case Else
                                    maxchar = 0
                            End Select
                            Dim strTemp As String = IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString)
                            If intCount = 8 Then ' for task owner
                                If strTemp.Trim <> "" Then
                                    structTempTaskOwner.Id = strTemp.Split("~")(1)
                                    structTempTaskOwner.Name = strTemp.Split("~")(0)
                                    CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = Mid(structTempTaskOwner.Name, 1, maxchar)
                                Else
                                    structTempTaskOwner.Id = ""
                                    structTempTaskOwner.Name = ""
                                    CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = "  "
                                End If
                                'Tootip should have full value
                                e.Item.Cells(intCount).ToolTip = structTempTaskOwner.Name
                            Else
                                If maxchar > 0 Then 'if characters required to be truncated
                                    CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", Mid(dtBound.Rows(cnt)(intCount).ToString, 1, maxchar))
                                Else
                                    CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString)
                                End If
                            End If '///////
                            'Tootip should have full value
                            e.Item.Cells(intCount).ToolTip = IIf(dtBound.Rows(cnt)(intCount).ToString Is Null, " ", dtBound.Rows(cnt)(intCount).ToString)
                        End If

                        TaskNo = dtgTask.DataKeys(e.Item.ItemIndex)

                        e.Item.Cells(intCount).Attributes.Add("onclick", "javascript:KeyCheck('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'cpnlCallTask_dtgTask')")
                        If intCount = 8 Then 'for task owner
                            e.Item.Cells(intCount).ForeColor = Color.Blue
                            e.Item.Cells(intCount).CssClass = "celltext"
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:OpenUserInfo2('" & structTempTaskOwner.Id & "')")
                        Else ' for cells other task owner
                            e.Item.Cells(intCount).Attributes.Add("onDBlclick", "javascript:KeyCheckTaskEdit('" & TaskNo & "', '" & mTaskRowValue + 1 & "', 'cpnlCallTask_dtgTask','" & ViewState("SAddressNumber_Template") & "','" & ViewState("PropCAComp") & "','" & ViewState("PropCallNumber") & "')")
                        End If
                    Next
                    mTaskRowValue += 1
                Else
                    For intCount = 0 To 1       'For Image Fields
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), System.Web.UI.WebControls.Image).ImageUrl = arrImageUrlDisabled(1)
                    Next
                    For intCount = 3 To dtvTask.Table.Columns.Count - 1
                        CType(e.Item.Cells(intCount).FindControl(dtvTask.Table.Columns(intCount).ToString), Label).Text = " "
                    Next
                End If
            End If

        Catch ex As Exception
            CreateLog("Template-Detail", "ItemDataBound-1689", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
        End Try


    End Sub

#Region "Set Entry TextBox Width"
    Private Sub ChangeTextBoxWidth()
        ''TxtTaskNo_F.Width = System.Web.UI.WebControls.Unit.Point(60)    'System.Web.UI.WebControls.Unit.Point(arrWidthTask(2))
        ''TxtSubject_F.Width = System.Web.UI.WebControls.Unit.Point(127)
        ''CDDLTaskType_F.SetWidth = 70    'System.Web.UI.WebControls.Unit.Point(arrWidthTask(5) - 8)
        ''TxtProject_F.Width = System.Web.UI.WebControls.Unit.Point(75)    'System.Web.UI.WebControls.Unit.Point(arrWidthTask(6))
        '''TxtTaskOwner_FName.Width = System.Web.UI.WebControls.Unit.Point(60)    'System.Web.UI.WebControls.Unit.Point(arrWidthTask(7) - 8)
        ''CDDLPriority_F.SetWidth = 45   'System.Web.UI.WebControls.Unit.Point(arrWidthTask(8) - 8)
        ''txtTaskNo.Width = System.Web.UI.WebControls.Unit.Percentage(100)

        '     dtEstCloseDate.Width = System.Web.UI.WebControls.Unit.Percentage(75)
        'TxtProject_F.Width = System.Web.UI.WebControls.Unit.Pixel(100)
        'TxtDescription_F.Width = System.Web.UI.WebControls.Unit.Pixel(300)
        ' TxtEstimatedHrs.Width = System.Web.UI.WebControls.Unit.Percentage(75)
        'CDDLAgmt_F.Width = System.Web.UI.WebControls.Unit.Pixel(100)
        'CDDLDependency_F.Width = System.Web.UI.WebControls.Unit.Pixel(100)
        'CDDLTaskOwner_F.Width = System.Web.UI.WebControls.Unit.Pixel(100)
        'CDDLTaskType_F.Width = System.Web.UI.WebControls.Unit.Pixel(100)
        'chkMandatory.Width = System.Web.UI.WebControls.Unit.Pixel(100)
        'CDDLPriority_F.Width = System.Web.UI.WebControls.Unit.Pixel(100)

    End Sub
#End Region

#Region "Save Task Fast Entry"

    Private Function SaveTask() As Boolean
        If TxtSubject_F.Text.Length > 950 Then
            lstError.Items.Clear()
            lstError.Items.Add("The length of Task Subject exceeded than maximum provided length.")
            lstError.Items.Add("Please write a small Task Subject within 950 characters.")
            'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Function
        End If

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        'Dim intNo As Int64
        Dim shFlag As Short
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intTaskNo As Integer

        'If blnCheckValidation = False Then    'Exit If all textbox are blank
        If TxtSubject_F.Text.Trim.Equals("") Then    ' Not to save if subject is blank
            SaveTask = False
            Exit Function
        End If
        Try
            lstError.Items.Clear()

            If CDDLTaskType_F.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Task Type cannot be blank...")
                shFlag = 1
            ElseIf CDDLTaskOwner_F.CDDLGetValue.Trim.Equals("") Then
                lstError.Items.Add("Task Owner cannot be blank...")
                shFlag = 1
                'ElseIf CDDLPriority_F.CDDLGetValue.trim.Equals("") Then
                '    lstError.Items.Add("Task Priority cannot be blank...")
                '    shFlag = 1
                'ElseIf TxtTaskOwner_F.Text.Trim.Equals("") Then
                '    lstError.Items.Add("Task Owner cannot be blank")
                '    shFlag = 1
                'ElseIf TxtStatus_F.Text.Trim.Equals("") Then
                '    lstError.Items.Add("Task Status cannot be blank")
                '    shFlag = 1
                'ElseIf dtEstCloseDate.CalendarDate.Trim <> "" Then
                '    If IsDate(dtEstCloseDate.CalendarDate) = False Then
                '        lstError.Items.Add("Check date format of estimated close date...")
                '        shFlag = 1
                '    Else
                '        If CDDLTemplateType.CDDLGetValue.Trim.ToUpper = "CNT" Then
                '            ' If CDate(dtEstCloseDate.CalendarDate.Trim) < CDate(txtCallDate.Text.Trim) Then
                '            If CDate(Format(CDate(dtEstCloseDate.CalendarDate.Trim), "MM/dd/yyyy")) < CDate(Format(CDate(txtCallDate.Text.Trim), "MM/dd/yyyy")) Then
                '                lstError.Items.Add("Estimated Close date cannot be less than Call date...")
                '                shFlag = 1
                '            End If
                '        End If
                '    End If
            End If

            If shFlag = 1 Then
                shFlag = 0
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            lstError.Items.Clear()

            strUDCType = "TKTY"
            strName = CDDLTaskType_F.CDDLGetValue.Trim.ToUpper
            strErrorMessage = "Task Type Mismatch..."

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            End If

            'strUDCType = "PRIO"
            'strName = CDDLPriority_F.CDDLGetValue.Trim.ToUpper
            'strErrorMessage = "Task Priority Mismatch..."

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            End If
            'Dim intAddressNo As Integer
            'intAddressNo = SQL.Search("", "", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & TxtTaskOwner_F.Text.Trim & "")
            'If intAddressNo <= 0 Then
            '    lstError.Items.Add("Task Owner mismatch")
            '    shFlag = 1
            'End If


            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            End If
            'Check validaity of task owner
            mstGetFunctionValue = CheckUserValiditity(CDDLTaskOwner_F.CDDLGetValue)
            If mstGetFunctionValue.FunctionExecuted = False Then
                lstError.Items.Clear()
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Function
            End If

            arrColumns.Add("TTM_DT8_Task_Date")
            arrColumns.Add("TTM_NU9_Call_No_FK")
            arrColumns.Add("TTM_NU9_TemplateID_FK")
            arrColumns.Add("TTM_NU9_Comp_ID_FK")
            arrColumns.Add("TTM_VC50_Deve_status")
            arrColumns.Add("TTM_VC1000_Subtsk_Desc")
            arrColumns.Add("TTM_VC8_task_type")
            arrColumns.Add("TTM_NU9_Project_ID")
            arrColumns.Add("TTM_VC8_Supp_Owner")
            arrColumns.Add("TTM_NU9_Assign_by")
            arrColumns.Add("TTM_VC8_Priority")
            arrColumns.Add("TTM_NU9_Dependency")

            arrColumns.Add("TTM_CH1_Comment")
            arrColumns.Add("TTM_CH1_Mandatory")
            'If Not dtEstCloseDate.CalendarDate.Trim.Equals("") Then
            '  arrColumns.Add("TTM_DT8_Est_close_date")
            '   End If
            arrColumns.Add("TTM_FL8_Est_Hr")
            arrColumns.Add("TTM_CH1_Forms")
            arrColumns.Add("TTM_NU9_Task_no_PK")



            arrRows.Add(Now)
            arrRows.Add(ViewState("PropCallNumber"))
            arrRows.Add(ViewState("SAddressNumber_Template"))
            arrRows.Add(DDLCustomer.SelectedValue)
            arrRows.Add(TxtStatus_F.Text.Trim.ToUpper)
            arrRows.Add(TxtSubject_F.Text.Trim)
            arrRows.Add(CDDLTaskType_F.CDDLGetValue.Trim.ToUpper)
            arrRows.Add(Val(DDLProject.SelectedValue))

            If CDDLTaskOwner_F.CDDLGetValue.Trim.Equals("") Then
                arrRows.Add(1)
            Else
                arrRows.Add(Val(CDDLTaskOwner_F.CDDLGetValue.Trim))
            End If

            arrRows.Add(HttpContext.Current.Session("PropUserID"))
            arrRows.Add("HIGH")
            arrRows.Add(IIf(DDLDependancy_F.SelectedValue = "", System.DBNull.Value, Val(DDLDependancy_F.SelectedValue)))
            arrRows.Add("0")
            If chkMandatory.Checked = True Then
                arrRows.Add("M")
            Else
                arrRows.Add("O")
            End If

            arrRows.Add(Val(TxtEstimatedHrs.Text))
            'Get Form is Assigned then Put Value 2 only for CNT template Type Else 0 
            'If WSSSearch.GetNoOfAssignedForms(CDDLTaskType_F.CDDLGetValue, True) > 0 Or CDDLTemplateType.CDDLGetValue = "CNT" Then
            If WSSSearch.GetNoOfAssignedForms(CDDLTaskType_F.CDDLGetValue, True, ViewState("CompanyID"), ViewState("SAddressNumber_Template"), ViewState("PropCallNumber")) > 0 Then
                arrRows.Add(2)
            Else
                arrRows.Add(0)
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            '  SQL.DBTable = "T050031"

            intTaskNo = SQL.Search("Template Detail", "SaveTask-1904", "select isnull(max(TTM_NU9_Task_no_PK),0) from T050031 where TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template"))
            intTaskNo += 1

            ' ViewState("PropTaskNumber") = intCallNo
            arrRows.Add(intTaskNo)

            'Done by atul for Task Order
            Dim intTaskOrder As Int32
            arrColumns.Add("TTM_NU9_Task_Order")
            intTaskOrder = SQL.Search("Template Detail", "SaveTask-1904", "select isnull(max(TTM_NU9_Task_Order),0) from T050031 where TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template"))
            intTaskOrder = intTaskOrder + 1
            arrRows.Add(intTaskOrder.ToString())

            If SQL.Save("T050031", "Template Detail", "SaveTask-1910", arrColumns, arrRows) = True Then
                DDLProject.Enabled = False
                DDLCustomer.Enabled = False
                ViewState("PropTaskNumber") = intTaskNo
                'Change Templarte Call Status
                If SQL.Update("Template Detail", "SaveTask-1910", "Update T050021 set TCM_VC20_Call_Status='ASSIGNED' WHERE  TCM_NU9_TemplateID_FK=" & mintTemplateNumber, SQL.Transaction.ReadCommitted) = True Then
                    ' dO NOTHING
                End If

                'For intI As Integer = 0 To garTFileID.Count - 1
                If GetFiles(mdlMain.AttachLevel.TaskLevel) = True Then
                    'shReturn = 1
                Else
                    'shReturn = 2
                End If
                'Next
                garTFileID.Clear()
                'mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("PropCallNumber"), True)
                ClearAllTextBox(cpnlCallTask)
                Call DisplayMessage("Records Save successfully...")
                ''ImgError.ImageUrl = "../../images/Pok.gif"


                FillNonUDCDropDown(DDLDependancy_F, "select TTM_NU9_Task_No_Pk,TTM_NU9_Task_No_Pk from T050031 where TTM_NU9_Call_No_FK=" & ViewState("PropCallNumber") & " and Ttm_nu9_comp_id_fk =" & ViewState("PropCAComp") & " and TTM_NU9_TemplateID_FK=" & mintTemplateNumber, True)



                'Fill task fields according to call 
                '*********************************
                ' CDDLTaskType_F.CDDLSetSelectedItem(CDDLTaskType.CDDLGetValue)
                'TxtTaskOwner_F.Text = txtCustomer.Text
                '???????????
                ''CDDLPriority_F.CDDLSetSelectedItem(CDDLPriority.CDDLGetValue)
                'TxtProject_F.Text = DDLProject.SelectedItem.Text   ' txtProject.Text

                '************************************

            Else
                Call DisplayError("Error While Saving Task Info...")
                ''ImgError.ImageUrl = "../../images/warning.gif"
                Return False
            End If
            Return True
        Catch ex As Exception
            CreateLog("TemplateDetail", "SaveTask-1724", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function
#End Region

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../TemplateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T050051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                '  SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("", "", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateFolder-1869", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../tempateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Template Detail", "CreateFolder-2156", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & "  and VH_NU9_Action_Number=" & ActionNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("PropCAComp") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("PropCAComp") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber, ViewState("SAddressNumber_Template")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber, ViewState("SAddressNumber_Template")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel, ViewState("SAddressNumber_Template"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("Template Detail", "CreateFolder-1986", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

#Region "Display in Error Panel"
    Private Sub DisplayError(ByVal ErrMsg As String)
        Try
            ' cpnlError.Visible = True
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            lstError.Items.Add(ErrMsg)
            'lstError.ForeColor = Color.Black
            'cpnlError.TitleCSS = "Test3"
            'cpnlError.Text = "Error Occured"
            'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
        Catch ex As Exception
            CreateLog("TemplateDetail", "DisplayError-2394", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub DisplayMessage(ByVal Msg As String)
        Try
            'cpnlError.Visible = True
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            'lstError.ForeColor = Color.Black
            lstError.Items.Add(Msg)
            'cpnlError.TitleCSS = "Test"
            'cpnlError.Text = "Message..."
            'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgOK)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception
            CreateLog("TemplateDetail", "DisplayMessage-2406", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Clear TextBoxes based on panels"
    Private Sub ClearAllTextBox(ByVal CPnl As CustomControls.Web.CollapsiblePanel)
        Dim objTextBox As Control
        Dim objCtl As Control
        Try
            For Each objCtl In CPnl.Controls
                If TypeOf objCtl Is TextBox Then
                    CType(objCtl, TextBox).Text = ""
                End If
                If TypeOf objCtl Is Panel Then
                    For Each objTextBox In objCtl.Controls
                        If TypeOf objTextBox Is TextBox Then
                            CType(objTextBox, TextBox).Text = ""
                        End If
                    Next
                End If
            Next
            'dtEstCloseDate.CalendarDate = ""
            TxtStatus_F.Text = "ASSIGNED"
            CDDLTaskType_F.CDDLSetSelectedItem("")
            DDLDependancy_F.SelectedValue = ""
            TxtEstimatedHrs.Text = ""
            TxtSubject_F.Text = ""
        Catch ex As Exception
            CreateLog("TemplateDetail", "ClearAllTextboxes-2427", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub
#End Region

#Region "Refresh Grid With no selection"
    Private Sub RefreshSelection()
        ' ViewState("PropCallNumber") = 0    '//For refreshing seleted callnumber
        ' mstrCallNumber = 0
        ViewState("PropTaskNumber") = 1
        Call CreateDataTableTask("")
        Call BindGridTask()
    End Sub
#End Region

#Region "CopyTemplate"

    Private Function CopyTempate(ByVal TemplateID As Integer) As Boolean
        Try

            'Copy Template Info
            If CopyTemplateData(TemplateID) = True Then

                'Copy Call Data
                Call CopyCallData(TemplateID)
                'Copy Task Data
                Call CopyTaskData(TemplateID)
                'Copy Attachment Data
                Call CopyAttachmentData(TemplateID)
                'Copy Version Attachment Data
                Call CopyAttachmentVersionData(TemplateID)
                'Copy Comment Data
                Call CopyCommentData(TemplateID)
                'Copy Form Head Data
                Call CopyFormsData(TemplateID)

                Response.Redirect("TemplateDetail.aspx", False)

            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyTempate-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyTemplateData(ByVal OldTemplateID As Integer) As Boolean

        Try
            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String


            strSQL = "select * from T050011 where TL_NU9_ID_PK=" & OldTemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then
                arrColumns.Add("TL_NU9_Copy_Template_ID_FK")
                arrColumns.Add("TL_VC30_Template")
                arrColumns.Add("TL_NU9_CustID_FK")
                arrColumns.Add("TL_VC8_Tmpl_Type")
                arrColumns.Add("TL_VC8_Call_type")
                arrColumns.Add("TL_NU9_ProjectID_FK") ' project ID/SubCategory ID
                arrColumns.Add("TL_VC200_Desc")
                arrColumns.Add("TL_VC8_Cat1")
                arrColumns.Add("TL_VC8_Cat2")
                arrColumns.Add("TL_NU9_CreatedById")
                arrColumns.Add("TL_DT8_TemplateDate")
                arrColumns.Add("TL_NU9_ID_PK")

                arrRows.Add(DDLCopyTemplate.SelectedValue)
                arrRows.Add(txtTmplName.Text.Trim)
                arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_NU9_CustID_FK"))
                arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_VC8_Tmpl_Type"))
                arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_VC8_Call_type"))

                'arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_NU9_ProjectID_FK")) 'Projrct ID/SubCategory ID
                ' Checking either  project same or not
                If Not DDLProject.SelectedItem.Equals("") Then
                    If DDLProject.SelectedValue = dsData.Tables(0).Rows(0).Item("TL_NU9_ProjectID_FK") Then
                        arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_NU9_ProjectID_FK")) 'Projrct ID/SubCategory ID
                    Else
                        arrRows.Add(DDLProject.SelectedValue)
                        blnSubCategoryCrossStatus = True 'value true if subcategory id diffrent 
                        If dsData.Tables(0).Rows(0).Item("TL_VC8_Tmpl_Type") = "TAO" Or dsData.Tables(0).Rows(0).Item("TL_VC8_Tmpl_Type") = "CNT" Or dsData.Tables(0).Rows(0).Item("TL_VC8_Tmpl_Type") = "CAO" Then
                            If blnSubCategoryCrossStatus = True Then 'Get default taskowner from changed project
                                intDefaultUserId = GetDefaultUserForTemplate(DDLProject.SelectedValue, DDLCustomer.SelectedValue)
                                If intDefaultUserId = 0 Then
                                    lstError.Items.Add("Please assign users to SubCategory...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                                    Return False
                                End If
                            End If
                        End If
                    End If
                Else
                    arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_NU9_ProjectID_FK")) 'Projrct ID/SubCategory ID
                End If

                arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_VC200_Desc"))
                arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_VC8_Cat1"))
                arrRows.Add(dsData.Tables(0).Rows(0).Item("TL_VC8_Cat2"))
                arrRows.Add(Val(Session("PropUserID")))
                arrRows.Add(Now)
                Dim intTmplNo As Integer = SQL.Search("", "", "select isnull(max(TL_NU9_ID_PK),0) from T050011 ")
                intTmplNo += 1
                arrRows.Add(intTmplNo)
                If SQL.Save("T050011", "", "", arrColumns, arrRows) = True Then
                    ViewState("SAddressNumber_Template") = intTmplNo
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyTemplateData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyCallData(ByVal TemplateID As Integer) As Boolean
        Try
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String
            strSQL = "select * from T050021 where TCM_NU9_TemplateID_FK=" & TemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            Dim intCallNo As Integer = SQL.Search("WSSSave", "SaveTemplCall-751", "select isnull(max(TCM_NU9_Call_No_PK),0) from t050021")
            intCallNo += 1

            arColumnName.Add("TCM_DT8_Request_Date")
            arColumnName.Add("TCM_VC100_By_Whom")
            arColumnName.Add("TCM_NU8_Attach_No")
            arColumnName.Add("TCM_NU9_Category_Code_1")
            arColumnName.Add("TCM_NU9_Category_Code_2")
            arColumnName.Add("TCM_VC20_Call_Status")
            arColumnName.Add("TCM_NU9_CompID_FK")
            arColumnName.Add("TCM_VC8_Call_Type")
            arColumnName.Add("TCM_NU9_Call_Owner")
            arColumnName.Add("TCM_VC200_Work_Priority")
            arColumnName.Add("TCM_VC50_Reference_Id")
            arColumnName.Add("TCM_VC100_Subject")
            arColumnName.Add("TCM_VC2000_Call_Desc")
            arColumnName.Add("TCM_NU9_Project_ID")
            arColumnName.Add("TCM_NU9_CustID_FK")
            arColumnName.Add("TCM_NU9_TemplateID_FK")
            arColumnName.Add("TCM_VC8_Template")
            arColumnName.Add("TCM_VC8_Tmpl_Type")
            arColumnName.Add("TCM_VC8_Category")
            arColumnName.Add("TCM_VC8_Cause_Code")
            arColumnName.Add("TCM_NU9_Coordinator")
            arColumnName.Add("TCM_NU9_Call_No_PK")

            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then

                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_DT8_Request_Date"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC100_By_Whom"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU8_Attach_No"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Category_Code_1"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Category_Code_2"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC20_Call_Status"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_CompID_FK"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC8_Call_Type"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Call_Owner"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC200_Work_Priority"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC50_Reference_Id"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC100_Subject"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC2000_Call_Desc"))
                'arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Project_ID"))
                If blnSubCategoryCrossStatus = True Then ' In Case Selected project is diffrent from template project, set new value 
                    arRowData.Add(DDLProject.SelectedValue)
                Else
                    arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Project_ID"))
                End If
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_CustID_FK"))
                arRowData.Add(ViewState("SAddressNumber_Template"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC8_Template"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC8_Tmpl_Type"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC8_Category"))
                arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_VC8_Cause_Code"))
                'arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Coordinator"))
                If blnSubCategoryCrossStatus = True Then
                    If Not IsDBNull(dsData.Tables(0).Rows(0).Item("TCM_NU9_Coordinator")) Then
                        Dim IntCoordinatorID As Integer
                        IntCoordinatorID = dsData.Tables(0).Rows(0).Item("TCM_NU9_Coordinator")
                        If ChkMembersInSubcategory(IntCoordinatorID, DDLProject.SelectedValue, DDLCustomer.SelectedValue) = True Then
                            arRowData.Add(IntCoordinatorID) ' if exists in project than pass same 
                        Else
                            arRowData.Add(intDefaultUserId) ' if not exists than change with default user
                        End If
                    Else
                        arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Coordinator"))
                    End If
                Else
                    arRowData.Add(dsData.Tables(0).Rows(0).Item("TCM_NU9_Coordinator"))
                End If
                arRowData.Add(intCallNo)
                If SQL.Save("T050021", "", "", arColumnName, arRowData) = True Then
                    ViewState("PropCallNumber") = intCallNo
                    Return True
                Else
                    Return False
                End If
            Else

                arRowData.Add(Now)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add("ASSIGNED")
                arRowData.Add(Session("PropCompanyID")) 'Company Id
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value) 'ProjectID/SubCategoryID
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(ViewState("SAddressNumber_Template")) ' Template ID
                arRowData.Add(System.DBNull.Value)
                arRowData.Add("TAO")
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(System.DBNull.Value)
                arRowData.Add(intCallNo)

                If SQL.Save("T050021", "", "", arColumnName, arRowData) = True Then
                    ViewState("PropCallNumber") = intCallNo
                    Return True
                Else
                    Return False
                End If

            End If
        Catch ex As Exception
            CreateLog("Template Detail", "CopyCallData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyTaskData(ByVal TemplateID As Integer) As Boolean
        Try
            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String


            strSQL = "select * from T050031 where TTM_NU9_TemplateID_FK=" & TemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then



                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    arrColumns = New ArrayList
                    arrColumns.Add("TTM_DT8_Task_Date")
                    arrColumns.Add("TTM_NU9_Call_No_FK")
                    arrColumns.Add("TTM_NU9_TemplateID_FK")
                    arrColumns.Add("TTM_NU9_Comp_ID_FK")
                    arrColumns.Add("TTM_VC50_Deve_status")
                    arrColumns.Add("TTM_VC1000_Subtsk_Desc")
                    arrColumns.Add("TTM_VC8_task_type")
                    arrColumns.Add("TTM_NU9_Project_ID")
                    arrColumns.Add("TTM_VC8_Supp_Owner")
                    arrColumns.Add("TTM_NU9_Assign_by")
                    arrColumns.Add("TTM_VC8_Priority")
                    arrColumns.Add("TTM_NU9_Dependency")
                    arrColumns.Add("TTM_CH1_Comment")
                    arrColumns.Add("TTM_CH1_Mandatory")
                    arrColumns.Add("TTM_FL8_Est_Hr")
                    arrColumns.Add("TTM_CH1_Forms")
                    arrColumns.Add("TTM_NU9_Task_no_PK")
                    arrColumns.Add("TTM_CH1_Attachment")

                    arrRows = New ArrayList
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_DT8_Task_Date"))
                    arrRows.Add(ViewState("PropCallNumber"))
                    arrRows.Add(ViewState("SAddressNumber_Template"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_NU9_Comp_ID_FK"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_VC50_Deve_status"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_VC1000_Subtsk_Desc"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_VC8_task_type"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_NU9_Project_ID"))

                    'arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_VC8_Supp_Owner"))
                    If blnSubCategoryCrossStatus = True Then
                        Dim IntTaskOwnerID As Integer
                        IntTaskOwnerID = dsData.Tables(0).Rows(intI).Item("TTM_VC8_Supp_Owner")
                        If ChkMembersInSubcategory(IntTaskOwnerID, DDLProject.SelectedValue, DDLCustomer.SelectedValue) = True Then
                            arrRows.Add(IntTaskOwnerID) ' if exists in project than pass same 
                        Else
                            arrRows.Add(intDefaultUserId) ' if not exists than change with default user
                        End If
                    Else
                        arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_VC8_Supp_Owner"))
                    End If

                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_NU9_Assign_by"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_VC8_Priority"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_NU9_Dependency"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_CH1_Comment"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_CH1_Mandatory"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_FL8_Est_Hr"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_CH1_Forms"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_NU9_Task_no_PK"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("TTM_CH1_Attachment"))

                    If SQL.Save("T050031", "Template Detail", "CopyTaskData-1910", arrColumns, arrRows) = True Then

                    End If
                Next
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyTaskData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyAttachmentData(ByVal TemplateID As Integer) As Boolean
        Try
            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String
            strSQL = "select * from T050041 where AT_NU9_TemplateID_FK=" & TemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then
                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    arrColumns = New ArrayList
                    arrColumns.Add("AT_NU9_File_ID_PK")
                    arrColumns.Add("AT_VC255_File_Name")
                    arrColumns.Add("AT_VC255_File_Size")
                    arrColumns.Add("AT_VC255_File_Path")
                    arrColumns.Add("AT_VC1_STatus")
                    arrColumns.Add("AT_NU9_Call_Number")
                    arrColumns.Add("AT_NU9_Task_Number")
                    arrColumns.Add("AT_NU9_Action_Number")
                    arrColumns.Add("AT_NU9_CompId_Fk")
                    arrColumns.Add("AT_DT8_Date")
                    arrColumns.Add("AT_VC8_Role")
                    arrColumns.Add("AT_NU9_Version")
                    arrColumns.Add("AT_DT8_Modify_Date")
                    arrColumns.Add("AT_IN4_Level")
                    arrColumns.Add("AT_NU9_TemplateID_FK")
                    arrColumns.Add("AT_Folder_Path")
                    arrColumns.Add("VH_VC4_Active_Status")

                    arrRows = New ArrayList
                    Dim intFileID As Integer = SQL.Search("mdlMain", "SaveTemplateAttachmentVersion", "Select isnull(max(AT_NU9_File_ID_PK),0) from T050041")
                    intFileID += 1
                    arrRows.Add(intFileID)
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_VC255_File_Name"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_VC255_File_Size"))
                    Dim strOldPath As String = CStr(dsData.Tables(0).Rows(intI).Item("AT_VC255_File_Path"))
                    Dim strPath() As String = strOldPath.Split("/")
                    Dim strNewPath As String = ""
                    strPath(2) = ViewState("PropCallNumber")
                    For intL As Integer = 0 To strPath.Length - 1
                        strNewPath &= strPath(intL) & "/"
                    Next
                    strNewPath = strNewPath.Remove(strNewPath.Length - 1, 1)
                    arrRows.Add(strNewPath)
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_VC1_STatus"))
                    arrRows.Add(ViewState("PropCallNumber"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_NU9_Task_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_NU9_Action_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_NU9_CompId_Fk"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_DT8_Date"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_VC8_Role"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_NU9_Version"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_DT8_Modify_Date"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_IN4_Level"))
                    arrRows.Add(ViewState("SAddressNumber_Template"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("AT_Folder_Path"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_VC4_Active_Status"))

                    If SQL.Save("T050041", "Template Detail", "CopyAttachmentData-1910", arrColumns, arrRows) = True Then
                        If File.Exists(MapPath("../../" & strOldPath)) Then
                            If Directory.Exists(System.IO.Path.GetDirectoryName(MapPath("../../" & strNewPath))) = False Then
                                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(MapPath("../../" & strNewPath)))
                            End If
                            File.Copy(MapPath("../../" & strOldPath), MapPath("../../" & strNewPath), True)
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyAttachmentData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyAttachmentVersionData(ByVal TemplateID As Integer) As Boolean
        Try
            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String
            strSQL = "select * from T050051 where VH_NU9_TemplateID_FK=" & TemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then
                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    arrColumns = New ArrayList
                    arrColumns.Add("VH_NU9_TemplateID_FK")
                    arrColumns.Add("VH_NU9_File_ID_PK")
                    arrColumns.Add("VH_VC255_File_Name")
                    arrColumns.Add("VH_VC255_File_Size")
                    arrColumns.Add("VH_VC255_File_Path")
                    arrColumns.Add("VH_VC1_Status")
                    arrColumns.Add("VH_NU9_Address_Book_Number")
                    arrColumns.Add("VH_NU9_Call_Number")
                    arrColumns.Add("VH_NU9_Task_Number")
                    arrColumns.Add("VH_NU9_Action_Number")
                    arrColumns.Add("VH_NU9_CompId_Fk")
                    arrColumns.Add("VH_DT8_Date")
                    arrColumns.Add("VH_VC8_Role")
                    arrColumns.Add("VH_NU9_Version")
                    arrColumns.Add("VH_DT8_Modify_Date")
                    arrColumns.Add("VH_IN4_Level")
                    arrColumns.Add("VH_VC4_Active_Status")

                    arrRows = New ArrayList
                    arrRows.Add(ViewState("SAddressNumber_Template"))
                    Dim intFileID As Integer = SQL.Search("mdlMain", "SaveTemplateAttachmentVersion", "Select isnull(max(VH_NU9_File_ID_PK),0) from T050051")
                    intFileID += 1
                    arrRows.Add(intFileID)
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_VC255_File_Name"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_VC255_File_Size"))
                    Dim strOldPath As String = CStr(dsData.Tables(0).Rows(intI).Item("VH_VC255_File_Path"))
                    Dim strPath() As String = strOldPath.Split("/")
                    Dim strNewPath As String = ""
                    strPath(2) = ViewState("PropCallNumber")
                    For intL As Integer = 0 To strPath.Length - 1
                        strNewPath &= strPath(intL) & "/"
                    Next
                    strNewPath = strNewPath.Remove(strNewPath.Length - 1, 1)
                    arrRows.Add(strNewPath)
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_VC1_Status"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_NU9_Address_Book_Number"))
                    arrRows.Add(ViewState("PropCallNumber"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_NU9_Task_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_NU9_Action_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_NU9_CompId_Fk"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_DT8_Date"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_VC8_Role"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_NU9_Version"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_DT8_Modify_Date"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_IN4_Level"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("VH_VC4_Active_Status"))

                    If SQL.Save("T050051", "Template Detail", "CopyAttachmentVersionData-1910", arrColumns, arrRows) = True Then
                        If File.Exists(MapPath("../../" & strOldPath)) Then
                            If Directory.Exists(System.IO.Path.GetDirectoryName(MapPath("../../" & strNewPath))) = False Then
                                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(MapPath("../../" & strNewPath)))
                            End If
                            File.Copy(MapPath("../../" & strOldPath), MapPath("../../" & strNewPath), True)
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyAttachmentVersionData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyCommentData(ByVal TemplateID As Integer) As Boolean
        Try
            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String
            strSQL = "select * from T050061 where CM_NU9_TemplateID_FK=" & TemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then
                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    arrColumns = New ArrayList

                    arrColumns.Add("CM_NU9_Comment_Number_PK")
                    arrColumns.Add("CM_NU9_TemplateID_FK")
                    arrColumns.Add("CM_NU9_Comment_To")
                    arrColumns.Add("CM_CH1_Flag")
                    arrColumns.Add("CM_NU9_AB_Number")
                    arrColumns.Add("CM_DT8_Date")
                    arrColumns.Add("CM_VC256_Comments")
                    arrColumns.Add("CM_VC2_Flag")
                    arrColumns.Add("CM_NU9_Call_Number")
                    arrColumns.Add("CM_NU9_Task_Number")
                    arrColumns.Add("CM_NU9_Action_Number")
                    arrColumns.Add("CM_VC30_Type")
                    arrColumns.Add("CM_NU9_CompId_Fk")
                    arrColumns.Add("CM_VC50_IE")
                    arrColumns.Add("CM_VC1000_MailList")
                    arrColumns.Add("CM_CH1_MailSent")


                    arrRows = New ArrayList
                    Dim intComment As Integer
                    intComment = SQL.Search("WSSSave", "SaveComments-347", "select isnull(max(CM_NU9_Comment_Number_PK),0) from T050061")
                    intComment += 1
                    arrRows.Add(intComment)
                    arrRows.Add(ViewState("SAddressNumber_Template"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_NU9_Comment_To"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_CH1_Flag"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_NU9_AB_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_DT8_Date"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_VC256_Comments"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_VC2_Flag"))
                    arrRows.Add(ViewState("PropCallNumber"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_NU9_Task_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_NU9_Action_Number"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_VC30_Type"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_NU9_CompId_Fk"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_VC50_IE"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_VC1000_MailList"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("CM_CH1_MailSent"))

                    If SQL.Save("T050061", "Template Detail", "CopyCommentData-1910", arrColumns, arrRows) = True Then

                    End If
                Next
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyCommentData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyFormsData(ByVal TemplateID As Integer) As Boolean
        Try
            Dim intFormNo As Integer

            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String
            strSQL = "select * from T050071 where FD_IN4_Temp_Id=" & TemplateID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then
                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    arrColumns = New ArrayList

                    arrColumns.Add("FD_IN4_Form_no")
                    arrColumns.Add("FD_IN4_Temp_Id")
                    arrColumns.Add("FD_IN4_Call_no")
                    arrColumns.Add("FD_IN4_Task_no")
                    arrColumns.Add("FD_VC50_Call_form_Name")
                    arrColumns.Add("FD_IN4_Comp_id")
                    arrColumns.Add("FD_VC50_RPA")
                    arrColumns.Add("FD_IN4_User1")
                    arrColumns.Add("FD_IN4_Inserted_By")
                    arrColumns.Add("FD_IN4_Inserted_On")

                    arrRows = New ArrayList
                    intFormNo = clsNextNo.GetNextNo(101, "COM", System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
                    arrRows.Add(intFormNo)
                    arrRows.Add(ViewState("SAddressNumber_Template"))
                    arrRows.Add(ViewState("PropCallNumber"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_IN4_Task_no"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_VC50_Call_form_Name"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_IN4_Comp_id"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_VC50_RPA"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_IN4_User1"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_IN4_Inserted_By"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("FD_IN4_Inserted_On"))

                    If SQL.Save("T050071", "Template Detail", "CopyCommentData-1910", arrColumns, arrRows) = True Then
                        ' Copy Form Tab Data
                        Call CopyFormTabData(TemplateID, intFormNo)

                    End If
                Next
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyFormsData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CopyFormTabData(ByVal TemplateID As Integer, ByVal FormNo As Integer) As Boolean
        Try

            Dim arrColumns As New ArrayList
            Dim arrRows As New ArrayList
            Dim dsData As New DataSet
            Dim strSQL As String
            strSQL = "select * from T050072 where Tb_IN4_Form_No=(select FD_IN4_Form_no from T050071 where FD_IN4_Temp_Id=" & TemplateID & ")"
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("TemplateData", "", "", strSQL, dsData, "", "") = True Then
                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    arrColumns = New ArrayList

                    arrColumns.Add("Tb_IN4_Tab_No")
                    arrColumns.Add("Tb_IN4_Form_No")
                    arrColumns.Add("Tb_VC200_Field1")
                    arrColumns.Add("Tb_VC200_Field2")
                    arrColumns.Add("Tb_VC200_Field3")
                    arrColumns.Add("Tb_VC200_Field4")
                    arrColumns.Add("Tb_VC200_Field5")
                    arrColumns.Add("Tb_VC200_Field6")
                    arrColumns.Add("Tb_VC200_Field7")
                    arrColumns.Add("Tb_VC200_Field8")
                    arrColumns.Add("Tb_VC200_Field9")
                    arrColumns.Add("Tb_VC200_Field10")
                    arrColumns.Add("Tb_VC200_Field11")
                    arrColumns.Add("Tb_VC200_Field12")
                    arrColumns.Add("Tb_VC2000_Field13")
                    arrColumns.Add("Tb_VC2000_Field14")
                    arrColumns.Add("Tb_VC2000_Field15")
                    arrColumns.Add("Tb_DT8_Date1")
                    arrColumns.Add("Tb_DT8_Date2")
                    arrColumns.Add("Tb_DT8_Date3")

                    arrRows = New ArrayList
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_IN4_Tab_No"))
                    arrRows.Add(FormNo)
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field1"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field2"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field3"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field4"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field5"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field6"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field7"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field8"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field9"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field10"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field11"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC200_Field12"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC2000_Field13"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC2000_Field14"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_VC2000_Field15"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_DT8_Date1"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_DT8_Date2"))
                    arrRows.Add(dsData.Tables(0).Rows(intI).Item("Tb_DT8_Date3"))

                    If SQL.Save("T050072", "Template Detail", "CopyCommentData-1910", arrColumns, arrRows) = True Then

                    End If
                Next
            End If

        Catch ex As Exception
            CreateLog("Template Detail", "CopyFormTabData-2042", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "Save Template "
    Private Function SaveTemplate() As Boolean
        Dim shFlag As Short
        Dim intRows As Integer

        If DDLCopyTemplate.SelectedValue.Equals("") = False Then
            If txtTmplName.Text.Trim.Equals("") Then
                lstError.Items.Add("Template Name have to be entered...")
                shFlag = 1
            End If

            If SQL.Search("Template Detail", "SaveTemplate-2379", "Select TL_VC30_Template From T050011 Where upper(TL_VC30_Template)='" & txtTmplName.Text.Trim.ToUpper.Replace("'", "''") & "' And TL_NU9_CustID_FK='" & DDLCustomer.SelectedValue & "'", intRows) = True Then
                shFlag = 1
                lstError.Items.Add("This Template Name Already Exists...")
            End If

            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                shFlag = 0
                Exit Function
            End If

            Call CopyTempate(DDLCopyTemplate.SelectedValue)
            Return True
        End If

        If txtTmplDesc.Text.Length > 200 Then
            lstError.Items.Clear()
            lstError.Items.Add("The length of template description text exceeded than maximum provided length.")
            lstError.Items.Add("Please write a small description within 200 characters.")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Function
        End If

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intTmplNo As Integer
        Dim intAddressNo As Integer

        lstError.Items.Clear()

        If txtTmplName.Text.Trim.Equals("") Then
            lstError.Items.Add("Template Name have to be entered...")
            shFlag = 1
        End If
        If CDDLTemplateType.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Template Type cannot be blank...")
            shFlag = 1
        End If
        If DDLProject.SelectedValue.Trim.Equals("") Then
            lstError.Items.Add("SubCategory cannot be blank...")
            shFlag = 1
        End If
        If CDDLCallType.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Call  Type cannot be blank...")
            shFlag = 1
        End If

        If txtTmplBy.Text.Trim.Equals("") Then
            lstError.Items.Add("Entered by cannot be blank...")
            mshFlag = 1
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Exit Function
        End If

        lstError.Items.Clear()

        intAddressNo = SQL.Search("Template Detail", "SaveTemplate-2330", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & DDLCustomer.SelectedValue.Trim & "")
        If intAddressNo <= 0 Then
            lstError.Items.Add("Customer Name Mismatch...")
            shFlag = 1
        End If

        strUDCType = "TMPL"
        strName = CDDLTemplateType.CDDLGetValue.Trim.ToUpper
        strErrorMessage = "Template Type Mismatch..."

        If CheckUDCValue(0, strUDCType, strName) = False Then
            lstError.Items.Add(strErrorMessage)
            shFlag = 1
        End If

        If Not (CDDLCallType.CDDLGetValue.Trim.Equals("")) Then
            strUDCType = "CALL"
            strName = CDDLCallType.CDDLGetValue.Trim.ToUpper
            strErrorMessage = "Template Call Type Mismatch..."

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            End If
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Exit Function
        End If

        If SQL.Search("Template Detail", "SaveTemplate-2379", "Select TL_VC30_Template From T050011 Where upper(TL_VC30_Template)='" & txtTmplName.Text.Trim.ToUpper.Replace("'", "''") & "' And TL_NU9_CustID_FK='" & DDLCustomer.SelectedValue & "'", intRows) = True Then
            shFlag = 1
            lstError.Items.Add("This Template Name Already Exists...")
        End If

        If shFlag = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Exit Function
        End If


        Try

            arrColumns.Add("TL_VC30_Template")
            arrColumns.Add("TL_NU9_CustID_FK")
            arrColumns.Add("TL_VC8_Tmpl_Type")
            arrColumns.Add("TL_VC8_Call_type")
            arrColumns.Add("TL_NU9_ProjectID_FK")
            arrColumns.Add("TL_VC200_Desc")
            arrColumns.Add("TL_VC8_Cat1")
            arrColumns.Add("TL_VC8_Cat2")

            arrColumns.Add("TL_NU9_CreatedById")
            arrColumns.Add("TL_DT8_TemplateDate")
            arrColumns.Add("TL_NU9_ID_PK")


            arrRows.Add(txtTmplName.Text.Trim)
            arrRows.Add(DDLCustomer.SelectedValue.Trim)
            arrRows.Add(CDDLTemplateType.CDDLGetValue.Trim)
            arrRows.Add(CDDLCallType.CDDLGetValue.Trim)
            arrRows.Add(DDLProject.SelectedValue)
            arrRows.Add(txtTmplDesc.Text.Trim)
            arrRows.Add(txtTmplCat1.Text.Trim)
            arrRows.Add(txtTmplCat2.Text.Trim)

            If txtTmplBy.Text.Trim.Equals("") Then
                arrRows.Add(1)
            Else
                arrRows.Add(txtTmplBy.Text.Trim)
            End If

            arrRows.Add(Now)

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            intTmplNo = SQL.Search("", "", "select isnull(max(TL_NU9_ID_PK),0) from T050011 ")
            intTmplNo += 1

            arrRows.Add(intTmplNo.ToString)

            If SQL.Save("T050011", "", "", arrColumns, arrRows) = True Then
                ViewState.Add("TemplateName", txtTmplName.Text.Trim.ToUpper)
                cpnlCallView.State = CustomControls.Web.PanelState.Expanded
                ViewState("SAddressNumber_Template") = intTmplNo
                lstError.Items.Add("Template Data Saved Successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Call FillTemplate()
                ViewState("PropCAComp") = DDLCustomer.SelectedValue
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If

            Call EnableDisablePanel()
            Return True
        Catch ex As Exception
            CreateLog("TemplateDetail", "SaveTemplate-2183", LogType.Application, LogSubType.Exception, ex.ToString, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function
#End Region

#Region "Update Template"
    Private Function UpdateTemplate() As Boolean

        If txtTmplDesc.Text.Length > 200 Then
            lstError.Items.Clear()
            lstError.Items.Add("The length of template description text exceeded than maximum provided length.")
            lstError.Items.Add("Please write a small description within 200 characters.")
            'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Function
        End If

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim shFlag As Short
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intTmplNo As Integer
        Dim ctrlTextBox As Control
        Dim blnCheckValidation As Boolean
        Dim intAddressNo As Integer

        lstError.Items.Clear()

        If txtTmplName.Text.Trim.Equals("") Then
            lstError.Items.Add("Template Name have to be entered....")
            shFlag = 1
        End If
        If CDDLTemplateType.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Template Type cannot be blank...")
            shFlag = 1
        End If
        If DDLProject.SelectedValue.Trim.Equals("") Then
            lstError.Items.Add("SubCategory cannot be blank...")
            shFlag = 1
        End If
        If CDDLCallType.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Call  Type cannot be blank...")
            shFlag = 1
        End If



        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Exit Function
        End If

        lstError.Items.Clear()

        'intAddressNo = SQL.Search("Template Deatil", "UpdateTemplate-2499", "select CI_NU8_Address_Number from T010011 where CI_NU8_Address_Number=" & DDLCustomer.SelectedValue.Trim & "")
        'If intAddressNo <= 0 Then
        '    lstError.Items.Add("Customer Name Mismatch...")
        '    mshFlag = 1
        'End If

        strUDCType = "TMPL"
        strName = CDDLTemplateType.CDDLGetValue.Trim.ToUpper
        strErrorMessage = "Template Type Mismatch..."

        If CheckUDCValue(0, strUDCType, strName) = False Then
            lstError.Items.Add(strErrorMessage)
            shFlag = 1
        End If

        If Not (CDDLCallType.CDDLGetValue.Trim.Equals("")) Then
            strUDCType = "CALL"
            strName = CDDLCallType.CDDLGetValue.Trim.ToUpper
            strErrorMessage = "Template Call Type Mismatch..."

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                shFlag = 1
            End If
        End If
        If ViewState("TemplateName") <> txtTmplName.Text.Trim.ToUpper Then
            Dim intRows As Integer
            If SQL.Search("Template Detail", "SaveTemplate-2379", "Select TL_VC30_Template From T050011 Where upper(TL_VC30_Template)='" & txtTmplName.Text.Trim.ToUpper.Replace("'", "''") & "' And TL_NU9_CustID_FK='" & DDLCustomer.SelectedValue & "'", intRows) = True Then
                shFlag = 1
                lstError.Items.Add("This Template Name Already Exists...")
            End If
        End If
        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, 'ImgError, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            shFlag = 0
            Exit Function
        End If

        Try

            arrColumns.Add("TL_VC30_Template")
            arrColumns.Add("TL_NU9_CustID_FK")
            arrColumns.Add("TL_VC8_Tmpl_Type")
            arrColumns.Add("TL_VC8_Call_type")
            arrColumns.Add("TL_NU9_ProjectID_FK")
            arrColumns.Add("TL_VC200_Desc")
            arrColumns.Add("TL_VC8_Cat1")
            arrColumns.Add("TL_VC8_Cat2")



            arrRows.Add(txtTmplName.Text.Trim)
            arrRows.Add(DDLCustomer.SelectedValue.Trim)
            arrRows.Add(CDDLTemplateType.CDDLGetValue.Trim)
            arrRows.Add(CDDLCallType.CDDLGetValue.Trim)
            arrRows.Add(DDLProject.SelectedValue)
            arrRows.Add(txtTmplDesc.Text.Trim)
            arrRows.Add(txtTmplCat1.Text.Trim)
            arrRows.Add(txtTmplCat2.Text.Trim)



            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            'SQL.DBTable = "T050011"

            'intTmplNo = SQL.Search("", "", "select isnull(max(TL_NU9_ID_PK),0) from T050011 ")
            'intTmplNo += 1
            'arrRows.Add(intTmplNo.ToString)

            If SQL.Update("T050011", "Template Detail", "UpdateTemplate-2576", "Select * from T050011 where TL_NU9_ID_PK=" & ViewState("SAddressNumber_Template"), arrColumns, arrRows) = True Then

                'mstGetFunctionValue = WSSUpdate.UpdateCallStatus(ViewState("PropCallNumber"), True)
                mintTemplateNumber = ViewState("SAddressNumber_Template")
                DisplayMessage("Template Data Saved Successfully...")
                ViewState("gshPageStatus") = 1
                garTFileID.Clear()
                ViewState("PropCAComp") = DDLCustomer.SelectedValue
                Return True

            Else
                Call DisplayError("Server is busy please try later...")
                Return False
            End If

        Catch ex As Exception
            CreateLog("TemplateDetail", "SaveCall-923", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
#End Region

#Region "Fill Templates"
    Private Sub FillTemplate()
        Dim blnTemplate As Boolean
        Dim sqrdTempate As SqlDataReader

        Try

            Dim sqrdCall As SqlDataReader
            Dim txthiddenImage As String = Request.Form("txthiddenImage")

            If txthiddenImage <> "forced" Then
                sqrdTempate = SQL.Search("", "", "Select a.*,b.CI_VC36_Name from T050011 a, T010011 b  Where a.TL_NU9_CustID_FK =b.CI_NU8_Address_Number  AND a.TL_NU9_ID_PK=" + CType(ViewState("SAddressNumber_Template"), String), SQL.CommandBehaviour.SingleRow, blnTemplate)

                If blnTemplate = True Then
                    While sqrdTempate.Read
                        txtTmplName.Text = IIf(IsDBNull(sqrdTempate.Item("TL_VC30_Template")), "", sqrdTempate.Item("TL_VC30_Template"))
                        ViewState.Add("TemplateName", txtTmplName.Text.Trim.ToUpper)
                        CDDLCallType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdTempate.Item("TL_VC8_Call_type")), "", sqrdTempate.Item("TL_VC8_Call_type")))
                        txtTmplCat1.Text = IIf(IsDBNull(sqrdTempate.Item("TL_VC8_Cat1")), "", sqrdTempate.Item("TL_VC8_Cat1"))
                        txtTmplCat2.Text = IIf(IsDBNull(sqrdTempate.Item("TL_VC8_Cat2")), "", sqrdTempate.Item("TL_VC8_Cat2"))
                        ' -- Fill SubCategory on the basis of customer
                        FillNonUDCDropDown(DDLProject, "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_VC8_Status='Enable' and PR_NU9_Comp_ID_FK=" & sqrdTempate.Item("TL_NU9_CustID_FK") & " order by PR_VC20_Name", True)
                        DDLCustomer.SelectedValue = IIf(IsDBNull(sqrdTempate.Item("TL_NU9_CustID_FK")), "", sqrdTempate.Item("TL_NU9_CustID_FK"))

                        txtTmplDesc.Text = IIf(IsDBNull(sqrdTempate.Item("TL_VC200_Desc")), "", sqrdTempate.Item("TL_VC200_Desc"))
                        DDLProject.SelectedValue = IIf(IsDBNull(sqrdTempate.Item("TL_NU9_ProjectID_FK")), "", sqrdTempate.Item("TL_NU9_ProjectID_FK"))
                        SetFieldsAfterProjectChange()
                        CDDLTemplateType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdTempate.Item("TL_VC8_Tmpl_Type")), "", sqrdTempate.Item("TL_VC8_Tmpl_Type")))

                        'fill call fields based on template
                        '******************************
                        txtCustomerName.Text = IIf(IsDBNull(sqrdTempate.Item("CI_VC36_Name")), "", sqrdTempate.Item("CI_VC36_Name"))
                        txtCustomer.Text = IIf(IsDBNull(sqrdTempate.Item("TL_NU9_CustID_FK")), "", sqrdTempate.Item("TL_NU9_CustID_FK"))
                        'added new two fields

                        txtTmplDate.Text = SetDateFormat(IIf(IsDBNull(sqrdTempate.Item("TL_DT8_TemplateDate")), Now.ToShortDateString, sqrdTempate.Item("TL_DT8_TemplateDate")), mdlMain.IsTime.WithTime)

                        txtTmplBy.Text = IIf(IsDBNull(sqrdTempate.Item("TL_NU9_CreatedById")), "", sqrdTempate.Item("TL_NU9_CreatedById"))
                        txtTmplByName.Text = WSSSearch.SearchUserName(IIf(IsDBNull(sqrdTempate.Item("TL_NU9_CreatedById")), "", sqrdTempate.Item("TL_NU9_CreatedById"))).ExtraValue
                        Try
                            DDLCopyTemplate.SelectedValue = IIf(IsDBNull(sqrdTempate.Item("TL_NU9_Copy_Template_ID_FK")), "", sqrdTempate.Item("TL_NU9_Copy_Template_ID_FK"))
                        Catch ex As Exception
                        End Try
                        '**********************************
                        If CDDLTemplateType.CDDLGetValue = "TAO" Then
                            cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                            Dim intRows As Integer
                            If SQL.Search("", "", "select * from T050031 where TTM_NU9_TemplateID_FK=" & ViewState("SAddressNumber_Template"), intRows) = True Then
                                DDLProject.Enabled = False
                                DDLCustomer.Enabled = False
                            Else
                                DDLProject.Enabled = True
                                DDLCustomer.Enabled = True
                            End If
                        End If
                    End While
                    sqrdTempate.Close()
                    ViewState("PropCAComp") = txtCustomer.Text
                    'sqrdCall.Close()
                ElseIf mstGetFunctionValue.ErrorCode = 1 Then

                ElseIf mstGetFunctionValue.ErrorCode = 2 Then

                End If
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "FillTemplate-2543", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try


    End Sub
#End Region

#Region "Panels Enable Disable"
    Private Sub EnableDisablePanel()
        Try
            If CDDLTemplateType.CDDLGetValue.Trim = "CAO" Then
                If ViewState("SAddressNumber_Template") > 0 Then
                    cpnlCallView.Enabled = True
                    cpnlCallView.TitleCSS = "test"
                    cpnlCallView.State = CustomControls.Web.PanelState.Expanded
                Else
                    cpnlCallView.Enabled = False
                    cpnlCallView.TitleCSS = "test2"
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                End If
                cpnlCallTask.Enabled = False
                cpnlCallTask.TitleCSS = "test2"
            ElseIf CDDLTemplateType.CDDLGetValue.Trim = "TAO" Then
                cpnlCallView.Enabled = False
                cpnlCallView.TitleCSS = "test2"
                cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                If ViewState("SAddressNumber_Template") > 0 Then
                    cpnlCallTask.Enabled = True
                    cpnlCallTask.TitleCSS = "test"
                    cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
                Else
                    cpnlCallTask.Enabled = False
                    cpnlCallTask.TitleCSS = "test2"
                    cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
                End If
            ElseIf CDDLTemplateType.CDDLGetValue.Trim = "CNT" Then
                If ViewState("SAddressNumber_Template") > 0 Then
                    cpnlCallView.Enabled = True
                    cpnlCallView.TitleCSS = "test"
                    cpnlCallView.State = CustomControls.Web.PanelState.Expanded
                Else
                    cpnlCallView.Enabled = False
                    cpnlCallView.TitleCSS = "test2"
                    cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                End If
                If ViewState("PropCallNumber") > 0 Then
                    cpnlCallTask.Enabled = True
                    cpnlCallTask.TitleCSS = "test"
                Else
                    cpnlCallTask.Enabled = False
                    cpnlCallTask.TitleCSS = "test2"
                End If
            Else    ' if txtTmplType = ""
                cpnlCallView.Enabled = False
                cpnlCallView.TitleCSS = "test2"
                cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
                cpnlCallTask.Enabled = False
                cpnlCallTask.TitleCSS = "test2"
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed

            End If

            If cpnlCallView.Enabled = False Then
                '   cpnlCallView.State = CustomControls.Web.PanelState.Collapsed
            Else
                '  cpnlCallView.State = CustomControls.Web.PanelState.Expanded
            End If
            If cpnlCallTask.Enabled = False Then
                cpnlCallTask.State = CustomControls.Web.PanelState.Collapsed
            Else
                cpnlCallTask.State = CustomControls.Web.PanelState.Expanded
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "EnableDisablePanel-2861", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

    Private Function GetCallNumber(ByVal TemplateId As Integer) As Integer
        Return SQL.Search("", "", "Select TCM_NU9_Call_No_PK from t050021 where TCM_NU9_TemplateID_FK=" & TemplateId)
    End Function

    Private Sub FillCustomDDl()
        Try
            If Val(ViewState("PropCAComp")) > 0 Then

                ' -- Call Type template
                CDDLCallType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""CALL""" & _
                    " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
                    " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""CALL""" & _
                    " and UDC.Company=0 Order By Name"

                CDDLCallType.CDDLMandatoryField = True
                '------------------------------------------

                ' -- Task Type

                CDDLTaskType_F.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TKTY""" & _
                  " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
                  " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""TKTY""" & _
                  " and UDC.Company=0 Order By Name"
                CDDLTaskType_F.CDDLMandatoryField = True
                '------------------------------------------
                '--Priority

                CDDLPriority.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""PRIO""" & _
                " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
                " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
                " and UDC.Company=0 Order By Name"

                CDDLPriority.CDDLMandatoryField = True
                '------------------------------------------
                '--Call Status
                CDDLStatus.CDDLQuery = "select SU_VC50_Status_Name as ID,SU_VC500_Status_Description as description,CI_VC36_Name as Company " & _
                    " from T040081,T010011 Where (SU_NU9_ScreenID=3  or SU_NU9_ScreenID=0) and SU_NU9_CompID*=CI_NU8_Address_Number and SU_NU9_CompID=" & ViewState("PropCAComp") & "  union " & _
                    "select SU_VC50_Status_Name as Name,SU_VC500_Status_Description as description,"""" as Company " & _
                    " from T040081 Where (SU_NU9_ScreenID=3  or SU_NU9_ScreenID=0) and SU_NU9_ID_PK>3 and SU_NU9_CompID=0 Order By ID"

                CDDLStatus.CDDLMandatoryField = True
                '------------------------------------------

                '--Priority
                'CDDLPriority_F.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""PRIO""" & _
                '" and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
                '" Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
                '" and UDC.Company=0 Order By Name"
                '-----------------------------------------

                '--Category
                CDDLCategory.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CATG' and UDC.Company=" & ViewState("PropCAComp") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CATG' and UDC.Company=0 Order By Name"
                CDDLCategory.CDDLUDC = True

                '--Cause Code
                CDDLCauseCode.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CACD' and UDC.Company=" & ViewState("PropCAComp") & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CACD' and UDC.Company=0 Order By Name"
                CDDLCauseCode.CDDLUDC = True

                '--TemplateType

                CDDLTemplateType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TMPL""" & _
                    " and UDC.Company=" & ViewState("PropCAComp") & "  union " & _
                    " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""TMPL""" & _
                 " and UDC.Company=0 Order By Name"


                '--Requested By
                CDDLCallOwner.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id=" & ViewState("PropCAComp") & ") and UM_IN4_Company_AB_ID=" & ViewState("PropCAComp") & " Order By Name"
                CDDLCallOwner.CDDLMandatoryField = True
                CDDLCallOwner.CDDLUDC = False

                CDDLCallOwner.CDDLFillDropDown(10, False)
                '-----------------------------------------

                CDDLStatus.CDDLFillDropDown(10, True)
                CDDLStatus.CDDLSetSelectedItem("OPEN")
                CDDLCallType.CDDLFillDropDown(10)
                CDDLTaskType_F.CDDLFillDropDown(10)
                CDDLTaskType_F.CDDLType = CustomDDL.DDLType.FastEntry
                CDDLPriority.CDDLFillDropDown(10, True)
                'CDDLPriority_F.CDDLFillDropDown(10)
                'CDDLPriority_F.CDDLType = CustomDDL.DDLType.FastEntry
                CDDLCategory.CDDLFillDropDown(10)
                CDDLCauseCode.CDDLFillDropDown(10)
                CDDLTemplateType.CDDLFillDropDown(10)

            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "FillCustomDDL-2726", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub DDLCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLCustomer.SelectedIndexChanged
        ViewState("PropCAComp") = Val(DDLCustomer.SelectedValue)
        CDDLCallOwner.CDDLSetBlank()
        CDDLPriority.CDDLSetBlank()
        'CDDLPriority_F.CDDLSetBlank()
        CDDLStatus.CDDLSetBlank()
        CDDLTaskOwner_F.CDDLSetBlank()
        CDDLTaskType_F.CDDLSetBlank()
        FillCustomDDl()
        ' Fill SubCategory According to customer selected
        FillNonUDCDropDown(DDLProject, "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_VC8_Status='Enable' and PR_NU9_Comp_ID_FK=" & ViewState("PropCAComp") & " order by PR_VC20_Name", True)
        FillNonUDCDropDown(DDLCopyTemplate, "select TL_NU9_ID_PK TemplateID, ('[' + TL_VC8_Tmpl_Type + '] ' + TL_VC30_Template) TemplateName from T050011 where TL_NU9_CustID_FK=" & Val(DDLCustomer.SelectedValue) & " order by TL_VC8_Tmpl_Type", True)
        txtCustomer.Text = DDLCustomer.SelectedValue
        txtCustomerName.Text = DDLCustomer.SelectedItem.Text
    End Sub
    Private Sub DDLProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLProject.SelectedIndexChanged
        SetFieldsAfterProjectChange()
    End Sub
    Private Sub SetFieldsAfterProjectChange()
        '--Task Owner
        Try
            Session("PropProjectID") = DDLProject.SelectedValue
            CDDLTaskOwner_F.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(Session("PropProjectID")) & "  and PM_NU9_Comp_ID_FK=" & DDLCustomer.SelectedValue & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name"
            CDDLTaskOwner_F.CDDLMandatoryField = True
            CDDLTaskOwner_F.CDDLType = CustomDDL.DDLType.FastEntry
            CDDLTaskOwner_F.CDDLFillDropDown(10, False)

            'cordinator
            CDDLCoordinator.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & Val(Session("PropProjectID")) & "  and PM_NU9_Comp_ID_FK=" & DDLCustomer.SelectedValue & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name"

            CDDLCoordinator.CDDLMandatoryField = False
            CDDLCoordinator.CDDLUDC = False
            CDDLCoordinator.CDDLFillDropDown(10, False)

        Catch ex As Exception
            CreateLog("TemplateDetail", "SetFieldsAfterProjectChange-2816", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Public Function ChangeTaskOrder1(ByVal enuChange As EnumTaskOrder, ByVal OldTaskOrder As Integer, Optional ByVal NewTaskOrder As Integer = 0) As Boolean
        Try
            Dim strSQL As String
            Dim objCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
            Dim objADP As SqlClient.SqlDataAdapter
            Dim dsTaskOrder As New DataSet
            Select Case enuChange
                Case EnumTaskOrder.DeleteTask
                    strSQL = "select * from T050031  where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & Val(ViewState("PropCallNumber")) & " and TTM_NU9_Task_Order>" & OldTaskOrder & " order by TTM_NU9_Task_Order asc"
                    objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                    objADP.Fill(dsTaskOrder, "T050031")
                    Dim intC As Integer = OldTaskOrder
                    For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                        dsTaskOrder.Tables(0).Rows(intI).Item("TTM_NU9_Task_Order") = intC
                        intC += 1
                    Next

                Case EnumTaskOrder.UpdateTask
                    If NewTaskOrder > OldTaskOrder Then
                        strSQL = "select * from T050031 where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & Val(ViewState("PropCallNumber")) & " and TTM_NU9_Task_Order>" & OldTaskOrder & " and TTM_NU9_Task_Order<=" & NewTaskOrder & " order by TTM_NU9_Task_Order asc"
                        objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                        objADP.Fill(dsTaskOrder, "T050031")
                        Dim intC As Integer = OldTaskOrder
                        For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                            dsTaskOrder.Tables(0).Rows(intI).Item("TTM_NU9_Task_Order") = intC 'dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") - 1
                            intC += 1
                        Next
                    ElseIf NewTaskOrder < OldTaskOrder Then
                        strSQL = "select * from T050031 where TTM_NU9_Comp_ID_FK=" & Val(ViewState("PropCAComp")) & " and ttm_nu9_TEMPLATEiD_fk=" & ViewState("SAddressNumber_Template") & " and TTM_NU9_Call_No_FK=" & Val(ViewState("PropCallNumber")) & " and TTM_NU9_Task_Order>=" & NewTaskOrder & " and TTM_NU9_Task_Order<" & OldTaskOrder & " order by TTM_NU9_Task_Order asc"
                        objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                        objADP.Fill(dsTaskOrder, "T050031")
                        Dim intC As Integer = NewTaskOrder + 1
                        For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                            dsTaskOrder.Tables(0).Rows(intI).Item("TTM_NU9_Task_Order") = intC 'dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") + 1
                            intC += 1
                        Next
                    End If
            End Select

            Dim DatasetChanges = dsTaskOrder.GetChanges
            If Not IsNothing(DatasetChanges) Then
                Dim objCMDBldr As New SqlClient.SqlCommandBuilder(objADP)
                objADP.Update(dsTaskOrder, "T050031")
                objCMDBldr.Dispose()
                objADP.Dispose()
            End If
            objCon.Dispose()
        Catch ex As Exception
            CreateLog("TemplateTask_Edit", "ChangeTaskOrder-2342", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
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
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
