Imports ION.Data
Imports ION.Logging
Imports ION.Logging.EventLogging
Imports System.Data
Partial Class SupportCenter_CallView_MultiTaskFwd
    Inherits System.Web.UI.Page
    Private mdvtable As System.Data.DataView = New System.Data.DataView
    Private arCallNo As New ArrayList
    Private arTaskNo As New ArrayList
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

                If Session("PropCompanyType") = "SCM" Then
                    FillCompanyDdl(ddlCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")")
                Else
                    ddlCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                    ddlCompany.SelectedValue = Session("PropCompanyID")
                    ddlCompany.Enabled = False
                    fillclientcomp()
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
            CreateLog("MultitaskFwd", "Load-138", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Function UpdateTask() As Boolean
        Try
            If ddlTaskOwner.SelectedItem.Text.Trim.Equals("") Then
                lstError.Items.Clear()
                lstError.Items.Add("Task owner cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
            End If
            ''''Check the status of the task becuase if it is CLOSED then it cannot be forwarded
            ''''get the status of the task becuase only ASSIGNED tasks can be deleted
            Dim strTaskStatus As String
            strTaskStatus = WSSSearch.GetTaskStatus(ViewState("CallNo"), ViewState("TaskNO"), ViewState("CompanyID"))

            If strTaskStatus = "CLOSED" Then
                lstError.Items.Clear()
                lstError.Items.Add("CLOSED Task cannot be forwarded...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Return False
            Else
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False

                Dim intTaskOwner As Integer = SQL.Search("Task_Fwd", "Load-70", "Select TM_VC8_Supp_Owner from T040021 where TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & ViewState("TaskNO") & "")
                mstGetFunctionValue = TaskForward(ddlTaskOwner.SelectedItem.Text, ddlTaskOwner.SelectedValue, ddlAssignTo.SelectedItem.Text, HttpContext.Current.Session("PropUserID"), ddlTaskOwner.SelectedValue, intTaskOwner, ViewState("CompanyID"), ViewState("CallNo"), ViewState("TaskNO"))

                If mstGetFunctionValue.ErrorCode = 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Task Forwarded successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                End If
                If mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Error! occur please try later...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
        Catch ex As Exception
            CreateLog("MultitaskFwd", "UpdateTask-190", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Function
    Private Sub fillclientcomp()
        Try
            FillCompanyDdl(ddlProject, "select PR_NU9_Project_ID_Pk as ID,PR_VC20_Name Name  from  " & _
       " T210011 WHERE PR_NU9_Comp_ID_FK ='" & ddlCompany.SelectedValue & "' and PR_Vc8_Status='Enable' order by PR_VC20_Name")

        Catch ex As Exception
            CreateLog("MultiTaskFwd", "fillclientcomp-200", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Try
            FillCompanyDdl(ddlProject, "select PR_NU9_Project_ID_Pk as ID,PR_VC20_Name Name  from  " & _
            " T210011 WHERE PR_NU9_Comp_ID_FK ='" & ddlCompany.SelectedValue & "' and PR_Vc8_Status='Enable' order by PR_VC20_Name")
            ddlTaskOwner.SelectedItem.Text = ""
            ddlAssignTo.SelectedItem.Text = ""
            FillView()
        Catch ex As Exception
            CreateLog("MultiTaskFwd", "ddlCompany_SelectedIndexChanged-211", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub FillView()
        Try
            Dim dsFromView As New DataSet
            Dim strselect As String = String.Empty
            strselect = "select convert(varchar,TM_NU9_Call_No_FK)as callno,TM_NU9_Task_no_PK as taskno,comp.CI_VC36_Name as company,TM_VC8_Priority as priority,TM_VC1000_Subtsk_Desc as taskdesc,ABy.UM_VC50_UserID as assignby,convert(varchar,TM_DT8_Est_close_date,101)as EstCloseDate,Project.PR_VC20_Name as Project,SOwner.UM_VC50_UserID as taskowner from T040011,T040021 Task,T060011 SOwner,T060011 ABy,T010011 comp,T210011 Project where task.TM_NU9_Comp_ID_FK=comp.CI_NU8_Address_Number and ABy.UM_IN4_Address_No_FK=Task.TM_NU9_Assign_by and SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and T040011.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and task.TM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and TM_VC50_Deve_Status<>'Closed'  and  comp.CI_VC36_Name ='" & ddlCompany.SelectedItem.Text.Trim & "' and  Project.PR_VC20_Name ='" & ddlProject.SelectedItem.Text.Trim & "' and   SOwner.UM_VC50_UserID = '" & ddlTaskOwner.SelectedItem.Text.Trim & "'  and CM_NU9_Call_No_PK=TM_NU9_Call_No_FK and CM_NU9_Comp_Id_FK=TM_NU9_Comp_ID_FK  and TM_NU9_Comp_ID_FK in (" & GetCompanySubQuery() & ") "
            If SQL.Search("T040021", "FillView", "FillView-241", strselect, dsFromView, "sachin", "Prashar") Then
                Dim htDateCols As New Hashtable
                Dim htGrdCols As New Hashtable
                mdvtable.Table = dsFromView.Tables("T040021")
                htDateCols.Add("EstCloseDate", 2)
                htGrdCols.Add("taskdesc", 33)
                HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable, htGrdCols)
                SetDataTableDateFormat(mdvtable.Table, htDateCols)
                GrdTask.DataSource = mdvtable
                GrdTask.DataBind()
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
            End If

        Catch ex As Exception
            CreateLog("MultiTaskFwd", "ddlCompany_SelectedIndexChanged-248", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub ddlProject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProject.SelectedIndexChanged
        Try
            FillCompanyDdl(ddlTaskOwner, "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & ddlProject.SelectedValue & " and PM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name")
        Catch ex As Exception
            CreateLog("MultiTaskFwd", "ddlCompany_SelectedIndexChanged-258", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub ddlTaskOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTaskOwner.SelectedIndexChanged
        Try
            FillCompanyDdl(ddlAssignTo, "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & ddlProject.SelectedValue & " and PM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & "  and PM_NU9_Project_Member_ID<>" & ddlTaskOwner.SelectedValue & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name")
            FillView()
        Catch ex As Exception
            CreateLog("MultiTaskFwd", "ddlCompany_SelectedIndexChanged-272", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub GrdTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdTask.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                CType(e.Item.FindControl("chkReq"), System.Web.UI.WebControls.CheckBox).Attributes.Add("onclick", "CheckBox('" & CType(e.Item.FindControl("chkReq"), System.Web.UI.WebControls.CheckBox).ClientID & "')")
                e.Item.Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & ")")
            ElseIf e.Item.ItemType = ListItemType.Header Then
                CType(e.Item.FindControl("chkAll"), System.Web.UI.WebControls.CheckBox).Attributes.Add("onclick", "CheckAll('" & CType(e.Item.FindControl("chkAll"), System.Web.UI.WebControls.CheckBox).ClientID & "')")
            End If
        Catch ex As Exception
            CreateLog("MultiTaskFwd", "GrdTask_ItemDataBound-277", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Private Sub ReadGrid()
        Try

            Dim gridrow As DataGridItem
            Dim strMailStatus As String = String.Empty
            Dim i As Integer = 0
            Dim intCallNo As Integer
            Dim intTaskNo As Integer

            If chkMailstatus.Checked = True Then
                strMailStatus = "Y"
            Else
                strMailStatus = "N"
            End If

            For Each gridrow In GrdTask.Items
                If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                    arCallNo.Add(CType(gridrow.FindControl("lblCallNo"), Label).Text)
                    arTaskNo.Add(CType(gridrow.FindControl("lbTaskNo"), Label).Text)
                End If
            Next

            If ChkValidation() = False Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return
            End If

            If arCallNo.Count > 0 Then
                For i = 0 To arCallNo.Count - 1
                    intCallNo = arCallNo(i)
                    intTaskNo = arTaskNo(i)
                    ViewState("CallNo") = Val(intCallNo)
                    ViewState("TaskNo") = Val(intTaskNo)
                    ViewState("CompanyID") = Val(ddlCompany.SelectedValue)
                    mstGetFunctionValue = TaskForward(ddlTaskOwner.SelectedItem.Text, ddlTaskOwner.SelectedValue, ddlAssignTo.SelectedItem.Text, HttpContext.Current.Session("PropUserID"), ddlAssignTo.SelectedValue, ddlTaskOwner.SelectedValue, ddlCompany.SelectedValue, ViewState("CallNo"), ViewState("TaskNo"), strMailStatus)
                Next
                If mstGetFunctionValue.ErrorCode = 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Task Forwarded successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    FillView()
                End If
                If mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Error! occur please try later ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                End If
            End If
        Catch ex As Exception
            CreateLog("MultiTaskFwd", "ReadGrid-355", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Private Function ChkValidation() As Boolean
        Try
            Dim err As Integer = 0
            lstError.Items.Clear()

            If ddlCompany.SelectedItem.Text.Trim.Equals("") = True Then
                lstError.Items.Add("Please select Company...")
                err = 1
            End If
            If ddlProject.SelectedItem.Text.Trim.Equals("") = True Then
                lstError.Items.Add("Please select SubCategory...")
                err = 1
            End If
            If ddlTaskOwner.SelectedItem.Text.Trim.Equals("") = True Then
                lstError.Items.Add("Please select Task Owner...")
                err = 1
            End If
            If ddlAssignTo.SelectedItem.Text.Trim.Equals("") = True Then
                lstError.Items.Add("Please select user to Assign the task...")
                err = 1
            End If
            If arCallNo.Count = 0 Then
                lstError.Items.Add("Please select the task to forward ...")
                err = 1
            End If
            If err = 1 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("MultiTaskFwd", "ChkValidation-396", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
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
