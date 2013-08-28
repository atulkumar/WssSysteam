Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class Reports_SLASummaryReport
    Inherits System.Web.UI.Page
    Private sqlconnection As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
#Region "Page Level Variables"
    Private objReports As clsReportData
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        If Page.IsPostBack = False Then
            fill_CallTaskStatus(1)
        End If
        Dim txthiddenImage = Request.Form("txthiddenImage")
        If txthiddenImage = "Logout" Then
            FormsAuthentication.SignOut()
            Call ClearVariables()
            Response.Redirect("../Login/Login.aspx")
        End If
    End Sub
    Private Sub bindGrid()
        Try

            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
            If dtFrom = Nothing Or dtTo = Nothing Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Date....")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                Dim From As String = "01/01/1932"
                dtFrom = CDate(From)

            ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                If CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    dtTo = Date.Now.ToShortDateString
                    lstError.Items.Clear()
                End If
            ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                If CDate(dtFrom) > CDate(dtTo) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than To Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf ddlStatus.SelectedValue = "--Select--" Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please select status of the call... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
            Else
                lstError.Items.Clear()
            End If
            Dim dsGrid As New DataSet
            Dim strSqlQuery As String
            strSqlQuery = "select distinct CM_NU9_Call_No_PK as 'Call No' ,comp.CI_VC36_Name as Company,Owner.UM_VC50_UserID as 'Call Owner',replace(convert(varchar(20), CM_DT8_Request_Date, 110), '-', '/') + ' ' + right(convert(varchar(20), CM_DT8_Request_Date, 100), 7) + '' as 'Request Date' ,CN_VC20_Call_Status as 'Call Status',CM_VC200_Work_Priority as 'Work Priority',CM_VC100_Subject 'Call Subject',CM_VC8_Call_Type as 'Call Type',ByWhom.UM_VC50_UserID as 'BY Whom',Project.PR_VC20_Name as 'ProjectName',Coord.UM_VC50_UserID as Coordinator from T990011 Call,T060011 ByWhom,T060011 Owner,T010011 comp, T010011 suppcomp,T210011 Project, T060011 Coord where call.CM_NU9_CustID_FK=suppcomp.CI_NU8_Address_Number and  call.CM_NU9_Comp_Id_FK = comp.CI_NU8_Address_Number and Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and ByWhom.UM_IN4_Address_No_FK=Call.CM_VC100_By_Whom and call.CM_NU9_Project_ID=Project.PR_NU9_Project_ID_Pk and call.CM_NU9_Comp_Id_FK=Project.PR_NU9_Comp_ID_FK and Coord.UM_IN4_Address_No_FK=* Call.CM_NU9_Coordinator and (CONVERT(datetime, CM_DT8_Log_Date) >= '" & dtFromDate.Text & "') and (CONVERT(datetime, CM_DT8_Log_Date) <='" & dtToDate.Text & "') AND (CN_VC20_Call_Status = '" & ddlStatus.SelectedItem.Text & "') "
            strSqlQuery &= "and CM_NU9_Call_No_PK in (select CM_NU9_Call_No_PK from T040011 where T040011.CN_VC20_Call_Status='COMPLETE')"
            strSqlQuery &= " and CM_NU9_Comp_Id_FK in (" & GetCompanySubQuery() & ") AND CM_VC100_By_Whom='104' "
            strSqlQuery &= " order by CM_NU9_Call_No_PK desc "
            Dim cmd As SqlCommand = New SqlCommand(strSqlQuery, sqlconnection)
            sqlconnection.Open()
            Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
            adp.Fill(dsGrid)
            Dim dtGrid As New DataView
            dtGrid = dsGrid.Tables(0).DefaultView
            grdCallSearch.DataSource = dtGrid
            grdCallSearch.DataBind()
            grdCallSearch.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grdCallSearch_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles grdCallSearch.ColumnCreated
        Try
            Dim col As GridColumn = e.Column
            col.AutoPostBackOnFilter = True
            If col.HeaderText = "CallNo" Then
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub grdCallSearch_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdCallSearch.ItemCreated
        Try
            If TypeOf e.Item Is GridDataItem Then
            End If
            'Setting the Width of Filtering Boxes i.e setting the width of columns of main table(Calls table)
            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then
                If TypeOf e.Item Is GridDataItem Then
                End If
                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filteringItem As GridFilteringItem = CType(e.Item, GridFilteringItem)
                    'set dimensions for the filter textbox
                    Dim box As TextBox = CType(filteringItem("Call Subject").Controls(0), TextBox)
                    box.Width = Unit.Pixel(200)
                    Dim box1 As TextBox = CType(filteringItem("Call Description").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(350)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub grdCallSearch_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCallSearch.NeedDataSource
        If Not e.IsFromDetailTable Then
            Call bindGrid()
        End If
    End Sub
#Region "Fill Drop Down List Boxes"

    Private Sub fill_CallTaskStatus(ByVal id As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            If id = 1 Then
                dt = objReports.extractCTStatus(1)
                ddlStatus.DataSource = dt
                ddlStatus.DataTextField = "CallStatus"
                ddlStatus.DataValueField = "CallStatus"
                ddlStatus.DataBind()
                ddlStatus.Items.Insert(0, "--Select--")

            ElseIf id = 2 Then
                dt = objReports.extractCTStatus(2)
                ddlStatus.DataSource = dt
                ddlStatus.DataTextField = "TaskStatus"
                ddlStatus.DataValueField = "TaskStatus"
                ddlStatus.DataBind()
                ddlStatus.Items.Insert(0, "--Select--")

            End If


        Catch ex As Exception
            ' Dim str As String = ex.Message.ToString


        Finally
            objReports = Nothing
        End Try
    End Sub
#End Region

    Protected Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        Call bindGrid()
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
