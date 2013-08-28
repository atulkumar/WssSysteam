Imports System.Data
Imports System.Data.Sql
Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports WSSBLL
Imports Telerik.Web.UI.Calendar
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI

Partial Class WorkCenter_DoList_WeeklySchedule
    Inherits System.Web.UI.Page
    Private mdvtable As System.Data.DataView = New System.Data.DataView
    Private arCallNoToAdd As New ArrayList
    Private arTaskNoToAdd As New ArrayList
    Private arProjectIdToAdd As New ArrayList
    Private arTaskNoDelete As New ArrayList
    Private arCallNoToDelete As New ArrayList
    Private intCallNo As Integer
    Private intTaskNo As Integer
    Private intProjectId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            'imgClose.Attributes.Add("OnClick", "return SaveEdit('Close');")
            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            If IsPostBack = False Then



                SetDatesWeekly(DateTime.Now)
                If Session("PropCompanyType") = "SCM" Then
                    FillCompanyDdl(ddlCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")")
                    ddlCompany.SelectedValue = Session("PropCompanyID")
                    ddlCompany_SelectedIndexChanged(New Object, New System.EventArgs)
                Else
                    ddlCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                    ddlCompany.SelectedValue = Session("PropCompanyID")
                    ddlCompany.Enabled = False
                    'fillclientcomp()
                End If
            End If
            Dim txthiddenImage As String = Request.Form("txthiddenImage")
            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Save"
                        ReadGrid()
                    Case "Close"
                        Response.Redirect("../../Home.aspx", False)
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            End If
            'Security Block
            Dim intId As Integer
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                intId = 967
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(967) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, 967)
            End If
            'End of Security Block
        Catch ex As Exception
            CreateLog("Crea", "Load-138", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub

    'fill company drop down 
    Public Sub FillCompanyDdl(ByVal ddlCustom As DropDownList, ByVal strSQL As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            ddlCustom.Items.Add("")
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillCompanyDdl", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Sub



    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Try
            FillCompanyDdl(ddlTaskOwner, "SELECT um_in4_address_no_fk  as ID, (CI_VC36_Name +'[' + um_vc50_userid +']') as Name FROM t060011,t010011 where ci_nu8_address_number=um_in4_address_no_fk and CI_VC8_Status='ENA' and CI_IN4_Business_Relation=" & ddlCompany.SelectedValue & " and CI_VC8_Address_Book_Type='EM' order by name")
            ddlTaskOwner.SelectedValue = Val(HttpContext.Current.Session("PropUserID"))
            ddlTaskOwner_SelectedIndexChanged(New Object, New System.EventArgs)
            FillView()
            FillLowerGridView()
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "ddlCompany_SelectedIndexChanged-211", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Private Sub FillView()
        Try
            Dim dsFromView As New DataSet
            Dim strselect As String = String.Empty
            If ddlProject.SelectedValue = String.Empty Then
                strselect = "select convert(varchar,TM_NU9_Call_No_FK)as callno,TM_NU9_Task_no_PK as taskno,comp.CI_VC36_Name as company,TM_VC8_Priority as priority,TM_VC1000_Subtsk_Desc as taskdesc,ABy.UM_VC50_UserID as assignby,convert(varchar,TM_DT8_Est_close_date,101)as EstCloseDate,Project.PR_VC20_Name as Project,Project.PR_NU9_Project_ID_Pk as ProjectId,SOwner.UM_VC50_UserID as taskowner,CASE when (select COUNT(*) from tblScheduleDetails where tblScheduleDetails.CallNo=task.TM_NU9_Call_No_FK and tblScheduleDetails.TaskNo=Task.TM_NU9_Task_no_PK  and tblScheduleDetails.Subcategory=Project.PR_NU9_Project_ID_Pk and tblScheduleDetails.Company=Task.TM_NU9_Comp_ID_FK and tblScheduleDetails.WeekNo=" & lblweekno.Text & " and tblScheduleDetails.WeekYear=YEAR('" & dtFrom.SelectedDate & "')) > 0 THEN 'Y' else 'N'  end as ScheduleStatus from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and TM_VC50_Deve_Status<>'Closed'  and  comp.CI_VC36_Name ='" & ddlCompany.SelectedItem.Text.Trim & "'  and SOwner.UM_IN4_Address_No_FK = " & ddlTaskOwner.SelectedValue & "  and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
            Else
                strselect = "select convert(varchar,TM_NU9_Call_No_FK)as callno,TM_NU9_Task_no_PK as taskno,comp.CI_VC36_Name as company,TM_VC8_Priority as priority,TM_VC1000_Subtsk_Desc as taskdesc,ABy.UM_VC50_UserID as assignby,convert(varchar,TM_DT8_Est_close_date,101)as EstCloseDate,Project.PR_VC20_Name as Project,Project.PR_NU9_Project_ID_Pk as ProjectId,SOwner.UM_VC50_UserID as taskowner,CASE when (select COUNT(*) from tblScheduleDetails where tblScheduleDetails.CallNo=task.TM_NU9_Call_No_FK and tblScheduleDetails.TaskNo=Task.TM_NU9_Task_no_PK  and tblScheduleDetails.Subcategory=Project.PR_NU9_Project_ID_Pk and tblScheduleDetails.Company=Task.TM_NU9_Comp_ID_FK and tblScheduleDetails.WeekNo=" & lblweekno.Text & " and tblScheduleDetails.WeekYear=YEAR('" & dtFrom.SelectedDate & "')) > 0 THEN 'Y' else 'N'  end as ScheduleStatus from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and TM_VC50_Deve_Status<>'Closed'  and  comp.CI_VC36_Name ='" & ddlCompany.SelectedItem.Text.Trim & "' and  Project.PR_NU9_Project_ID_Pk =" & ddlProject.SelectedValue & " and Project.PR_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & " and SOwner.UM_IN4_Address_No_FK = " & ddlTaskOwner.SelectedValue & "  and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
            End If

            If SQL.Search("T040021", "FillView", "FillView-241", strselect, dsFromView, "Anuj", "Kumar") Then
                Dim htDateCols As New Hashtable
                Dim htGrdCols As New Hashtable
                mdvtable.Table = dsFromView.Tables("T040021")
                htDateCols.Add("EstCloseDate", 2)
                htGrdCols.Add("taskdesc", 33)
                HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdCols)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)
                GrdTask.DataSource = mdvtable
                GrdTask.DataBind()
                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
            Else
                Dim ds As New DataSet
                Dim dtRow As DataRow
                ds.Tables.Add("Dummy")
                ds.Tables("Dummy").Columns.Add("MachName")
                dtRow = ds.Tables("Dummy").NewRow
                dtRow.Item(0) = ""
                mdvtable.Table = ds.Tables("Dummy")
                GrdTask.DataSource = mdvtable.Table
                GrdTask.DataBind()
                cpnlSearch.State = CustomControls.Web.PanelState.Collapsed
            End If

        Catch ex As Exception
            CreateLog("Create Weekly Schedule", "Fill_upperGridView", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Private Sub FillLowerGridView()
        Try
            Dim dsWeekSchedule As New DataSet
            Dim strselect As String = String.Empty
            If ddlProject.SelectedValue = String.Empty Then
                strselect = "select convert(varchar,TM_NU9_Call_No_FK)as callno,TM_NU9_Task_no_PK as taskno,comp.CI_VC36_Name as company,TM_VC8_Priority as priority,TM_VC1000_Subtsk_Desc as taskdesc,ABy.UM_VC50_UserID as assignby,convert(varchar,TM_DT8_Est_close_date,101)as EstCloseDate,Project.PR_VC20_Name as Project,SOwner.UM_VC50_UserID as taskowner from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK  and  comp.CI_VC36_Name ='" & ddlCompany.SelectedItem.Text.Trim & "'  and SOwner.UM_IN4_Address_No_FK = " & ddlTaskOwner.SelectedValue & "  and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") and Task.TM_NU9_Task_no_PK in (select TaskNo from tblScheduleDetails where Company=TM_NU9_Comp_ID_FK and CallNo=TM_NU9_Call_No_FK and WeekNo=" & lblweekno.Text & " AND WeekYear=year('" & dtFrom.SelectedDate & "')) "
            Else
                strselect = "select convert(varchar,TM_NU9_Call_No_FK)as callno,TM_NU9_Task_no_PK as taskno,comp.CI_VC36_Name as company,TM_VC8_Priority as priority,TM_VC1000_Subtsk_Desc as taskdesc,ABy.UM_VC50_UserID as assignby,convert(varchar,TM_DT8_Est_close_date,101)as EstCloseDate,Project.PR_VC20_Name as Project,SOwner.UM_VC50_UserID as taskowner from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK  and  comp.CI_VC36_Name ='" & ddlCompany.SelectedItem.Text.Trim & "' and  Project.PR_NU9_Project_ID_Pk =" & ddlProject.SelectedValue & " and Project.PR_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & " and SOwner.UM_IN4_Address_No_FK = " & ddlTaskOwner.SelectedValue & "  and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") and Task.TM_NU9_Task_no_PK in (select TaskNo from tblScheduleDetails where Company=TM_NU9_Comp_ID_FK and CallNo=TM_NU9_Call_No_FK and WeekNo=" & lblweekno.Text & " AND WeekYear=year('" & dtFrom.SelectedDate & "')) "
            End If

            If SQL.Search("tblWeeklySchedule", "FillViewWeekly", "FillView-241", strselect, dsWeekSchedule, "Anuj", "Kumar") Then
                Dim htDateCols As New Hashtable
                Dim htGrdCols As New Hashtable
                mdvtable.Table = dsWeekSchedule.Tables("tblWeeklySchedule")
                htDateCols.Add("EstCloseDate", 2)
                htGrdCols.Add("taskdesc", 33)
                HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdCols)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)
                grdWeekSchedule.DataSource = mdvtable
                grdWeekSchedule.DataBind()
                CpnlScheduleGrid.State = CustomControls.Web.PanelState.Expanded
            Else
                Dim ds As New DataSet
                Dim dtRow As DataRow
                ds.Tables.Add("Dummy")
                ds.Tables("Dummy").Columns.Add("MachName")
                dtRow = ds.Tables("Dummy").NewRow
                dtRow.Item(0) = ""
                mdvtable.Table = ds.Tables("Dummy")
                grdWeekSchedule.DataSource = mdvtable.Table
                grdWeekSchedule.DataBind()
                CpnlScheduleGrid.State = CustomControls.Web.PanelState.Collapsed
            End If
        Catch ex As Exception
            CreateLog("Create Weekly Schedule", "Fill_upperGridView", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Protected Sub ddlTaskOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaskOwner.SelectedIndexChanged
        Try
            FillCompanyDdl(ddlProject, "select PR_NU9_Project_ID_Pk as ID,PR_VC20_Name Name from T210011 where PR_NU9_Project_ID_Pk in (select PM_NU9_Project_ID_Fk from T210012 where PM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & " and PM_NU9_Project_Member_ID=" & ddlTaskOwner.SelectedValue & ") and PR_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & " order by Name ")
            'FillCompanyDdl(ddlAssignTo, "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & ddlProject.SelectedValue & " and PM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & "  and PM_NU9_Project_Member_ID<>" & ddlTaskOwner.SelectedValue & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name")
            FillView()
            FillLowerGridView()
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "ddlTaskOwner_SelectedIndexChanged-272", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        Try

            'FillCompanyDdl(ddlTaskOwner, "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & ddlProject.SelectedValue & " and PM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name")
            'ddlTaskOwner.SelectedValue = Val(HttpContext.Current.Session("PropUserID"))
            FillView()
            FillLowerGridView()
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "ddlProject_SelectedIndexChanged-258", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Protected Sub GrdTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdTask.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                CType(e.Item.FindControl("chkReq"), System.Web.UI.WebControls.CheckBox).Attributes.Add("onclick", "CheckBox('" & CType(e.Item.FindControl("chkReq"), System.Web.UI.WebControls.CheckBox).ClientID & "')")
                e.Item.Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & ")")
                Dim lbl As New Label
                lbl = e.Item.FindControl("lblScheduleStatus")
                If lbl.Text = "Y" Then
                    Dim chk As New CheckBox
                    chk = e.Item.FindControl("chkReq")
                    chk.Checked = True
                End If
            ElseIf e.Item.ItemType = ListItemType.Header Then
                CType(e.Item.FindControl("chkAll"), System.Web.UI.WebControls.CheckBox).Attributes.Add("onclick", "CheckAll('" & CType(e.Item.FindControl("chkAll"), System.Web.UI.WebControls.CheckBox).ClientID & "')")
            End If
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "GrdTask_ItemDataBound-277", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Public Sub SetDatesWeekly(ByVal Selecteddate As DateTime)
        Dim dsSetDates As New DataSet
        Dim strselect As String = String.Empty
        strselect = "SELECT convert(datetime,'" & Selecteddate & "') - (DATEPART(DW, convert(datetime,'" & Selecteddate & "')) - 1) as 'Week Start Date'  ,convert(datetime,'" & Selecteddate & "') + (7 - DATEPART(DW,convert(datetime,'" & Selecteddate & "'))) as 'Week End Date' , DATEPART( wk, convert(datetime,'" & Selecteddate & "')) as 'Week No'"
        If SQL.Search("T040021", "GetDatesWeekly", "GetDatesWeekly", strselect, dsSetDates, "Anuj", "Kumar") Then

            dtFrom.SelectedDate = dsSetDates.Tables(0).Rows(0)("Week Start Date")
            dtTODate.SelectedDate = dsSetDates.Tables(0).Rows(0)("Week End Date")
            lblweekno.Text = dsSetDates.Tables(0).Rows(0)("Week No")
        End If
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
    Protected Sub dtFrom_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs)
        SetDatesWeekly(dtFrom.SelectedDate)
        FillView()
        FillLowerGridView()
    End Sub
    Protected Sub dtTODate_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs)
        SetDatesWeekly(dtTODate.SelectedDate)
        FillView()
        FillLowerGridView()
    End Sub
    Private Sub ReadGrid()
        Try

            Dim gridrow As DataGridItem
            Dim strMailStatus As String = String.Empty
            Dim i As Integer = 0

            For Each gridrow In GrdTask.Items
                If CType(gridrow.FindControl("lblScheduleStatus"), Label).Text = "Y" Then
                    If Not CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                        arCallNoToDelete.Add(CType(gridrow.FindControl("lblCallNo"), Label).Text)
                        arTaskNoDelete.Add(CType(gridrow.FindControl("lbTaskNo"), Label).Text)
                    End If
                Else
                    If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                        arCallNoToAdd.Add(CType(gridrow.FindControl("lblCallNo"), Label).Text)
                        arTaskNoToAdd.Add(CType(gridrow.FindControl("lbTaskNo"), Label).Text)
                        arProjectIdToAdd.Add(CType(gridrow.FindControl("lblProjectId"), Label).Text)
                    End If
                End If
            Next
            If ChkValidation() = False Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return
            End If
            If arCallNoToAdd.Count > 0 Then
                For i = 0 To arCallNoToAdd.Count - 1
                    intCallNo = arCallNoToAdd(i)
                    intTaskNo = arTaskNoToAdd(i)
                    intProjectId = arProjectIdToAdd(i)
                    ViewState("CallNo") = Val(intCallNo)
                    ViewState("TaskNo") = Val(intTaskNo)
                    ViewState("CompanyID") = Val(ddlCompany.SelectedValue)
                    mstGetFunctionValue = SaveWeeklySchedule()
                Next
            End If
            If arCallNoToDelete.Count > 0 Then
                For i = 0 To arCallNoToDelete.Count - 1
                    intCallNo = arCallNoToDelete(i)
                    intTaskNo = arTaskNoDelete(i)
                    ViewState("CallNo") = Val(intCallNo)
                    ViewState("TaskNo") = Val(intTaskNo)
                    ViewState("CompanyID") = Val(ddlCompany.SelectedValue)
                    mstGetFunctionValue = UpdateWeeklySchedule()
                Next
            End If
            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("Weekly Schedule Saved successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                FillView()
                FillLowerGridView()
            End If
            If mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Clear()
                lstError.Items.Add("Error! occur please try later ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End If

        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "ReadGrid-355", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' Validation Function
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChkValidation() As Boolean
        Try
            Dim err As Integer = 0
            lstError.Items.Clear()

            If ddlCompany.SelectedItem.Text.Trim.Equals("") = True Then
                lstError.Items.Add("Please select Company...")
                err = 1
            End If

            If ddlTaskOwner.SelectedItem.Text.Trim.Equals("") = True Then
                lstError.Items.Add("Please select Task Owner...")
                err = 1
            End If
            Dim dsSetDates As New DataSet
            Dim strselect As String = String.Empty
            strselect = "SELECT  DATEPART( wk, getdate()) as 'Week No'"
            If SQL.Search("T040021", "GetWeekno.", "GetWeekno.", strselect, dsSetDates, "Anuj", "Kumar") Then
                If Convert.ToInt32(lblweekno.Text) < Convert.ToInt32(dsSetDates.Tables(0).Rows(0)(0)) Then
                    lstError.Items.Add("Week No. is less then current week...")
                    err = 1
                End If
            End If



            If err = 1 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("CreateWeellySchedule", "ChkValidation-396", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Function to save Tasks in weekly schedule table.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveWeeklySchedule() As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strSql As String = String.Empty
            strSql = "insert into tblScheduleDetails(Company,CallNo,TaskNo,TaskOwner,Subcategory,WeekStartDate,WeekEndDate,WeekNo,InsertedBy,WeekYear) Values(" & Val(ddlCompany.SelectedValue) & "," & Val(intCallNo) & "," & Val(intTaskNo) & "," & Val(ddlTaskOwner.SelectedValue) & "," & Val(intProjectId) & ",'" & dtFrom.SelectedDate & "','" & dtTODate.SelectedDate & "'," & lblweekno.Text & "," & Val(HttpContext.Current.Session("PropUserID")) & ",YEAR('" & dtFrom.SelectedDate & "'))"
            SQL.Save("CreateWeeklySchedule", "SaveWeeklySchedule", strSql, SQL.Transaction.Serializable)
            stReturn.ErrorCode = 0
            stReturn.ErrorMessage = "Weekly Schedule Saved successfully..."
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorCode = 1
            stReturn.ErrorMessage = "Error in Saving Weekly Schedule..."
            Return stReturn
            CreateLog("CreateWeeklySchedule", "SaveWeeklySchedule", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Function
    ''' <summary>
    ''' Function to delete Tasks From weekly Schedule table.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateWeeklySchedule() As ReturnValue
        Dim stReturn As ReturnValue
        Try

            Dim strSql As String = String.Empty
            strSql = "delete from tblScheduleDetails where Company=" & Val(ddlCompany.SelectedValue) & " and CallNo=" & Val(intCallNo) & " and TaskNo=" & Val(intTaskNo) & " and WeekNo=" & lblweekno.Text & ""
            SQL.Delete("CreateWeeklySchedule", "SaveWeeklySchedule", strSql, SQL.Transaction.Serializable)
            stReturn.ErrorCode = 0
            stReturn.ErrorMessage = "Weekly Schedule Updated successfully..."
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorCode = 1
            stReturn.ErrorMessage = "Error in Updating Weekly Schedule..."
            Return stReturn
            CreateLog("CreateWeeklySchedule", "SaveWeeklySchedule", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Function
End Class
