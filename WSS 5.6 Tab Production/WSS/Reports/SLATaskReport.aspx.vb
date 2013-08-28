Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class Reports_SLATaskReport
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
                lstError.Items.Add("Please Select Date Parameters....")
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
                End If
            Else
                lstError.Items.Clear()
            End If
            Dim dsGrid As New DataSet
            Dim strSqlQuery As String


            strSqlQuery = "select CM_NU9_Call_No_PK ,CM_VC100_Subject as [Call Subject],CM_VC200_Work_Priority as [Priority],CM_DT8_Request_Date as [Registered Date],(select min(Comment.CM_DT8_Date)  from T040061 Comment where  Comment.cm_nu9_call_number = a.CM_NU9_Call_No_PK and cm_nu9_compID_fk=38 and cm_nu9_ab_number= 104)as [Comment Date],datediff(minute,CM_DT8_Request_Date,(select min(Comment.CM_DT8_Date)  from T040061 Comment where  Comment.cm_nu9_call_number = a.CM_NU9_Call_No_PK and cm_nu9_compID_fk=38 and cm_nu9_ab_number= 104))as [Difference In minutes], (case when convert(varchar,CM_DT8_Request_Date,8)  between convert(datetime,'08:00',8) and convert(datetime,'18:00:00',8) then 'Yes' else 'No' end) as [Business Hours]  from T040011 a where convert(datetime,CONVERT(varchar, CM_DT8_Request_Date,101)) >= '" & dtFromDate.Text & "' and convert(datetime,CONVERT(varchar, CM_DT8_Request_Date,101)) <='" & dtToDate.Text & "' and CM_VC200_Work_Priority=1 and CM_NU9_Comp_Id_FK=38 and CM_VC100_By_Whom <> '104' "
            'strSqlQuery = "select CM_NU9_Call_No_PK ,CM_VC100_Subject as [Call Subject],CM_VC200_Work_Priority as [Priority],CM_DT8_Request_Date as [Registered Date],(select min(Comment.CM_DT8_Date)  from T040061 Comment where  Comment.cm_nu9_call_number = a.CM_NU9_Call_No_PK and cm_nu9_compID_fk=38 and cm_nu9_ab_number= 104)as [Comment Date],datediff(mi,CM_DT8_Request_Date,(select min(Comment.CM_DT8_Date)  from T040061 Comment where  Comment.cm_nu9_call_number = a.CM_NU9_Call_No_PK)) as [Differene In Minutes],(case when convert(varchar,CM_DT8_Request_Date,8)  between convert(datetime,'08:00',8) and convert(datetime,'18:00:00',8) then 'Yes' else 'No' end) as [Business Hours]  from T040011 a where cm_nu9_call_no_pk in (select TM_nu9_call_no_fk from t040021 where TM_NU9_Assign_by=104) and (CONVERT(datetime, CM_DT8_Request_Date) >= '" & dtFromDate.Text & "') and (CONVERT(datetime, CM_DT8_Request_Date) <='" & dtToDate.Text & "')and CM_VC200_Work_Priority=1 and CM_NU9_Comp_Id_FK=38"

            'strSqlQuery = "select distinct CM_NU9_Call_No_PK as CallNo,CM_VC100_Subject as [Call Subject],CM_VC200_Work_Priority as [Priority],CM_DT8_Request_Date as [Request Date],CM_DT8_Call_Start_Date as [Received Date],datediff(mi,CM_DT8_Call_Start_Date,CM_DT8_Request_Date) as [Difference In minutes],(case when convert(varchar,CM_DT8_Request_Date,8)  between convert(datetime,'08:00',8) and convert(datetime,'18:00:00',8) then 'Yes' else 'No' end) as [Business Hours] from t040011 where CM_VC100_By_Whom= 104 and (CONVERT(datetime, CM_DT8_Request_Date) >= '" & dtFromDate.Text & "') and (CONVERT(datetime, CM_DT8_Request_Date) <='" & dtToDate.Text & "') order by CM_DT8_Request_Date desc"
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

    Protected Sub imgExportToPDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToExcel.Click
        If grdCallSearch.Columns.Count <= 0 Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select the search parameters....")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        grdCallSearch.MasterTableView.ExportToExcel()
    End Sub

    Protected Sub imgExportToExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToPDF.Click
        If grdCallSearch.Columns.Count <= 0 Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select the search parameters....")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        grdCallSearch.MasterTableView.ExportToPdf()
    End Sub

    Protected Sub grdCallSearch_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles grdCallSearch.ColumnCreated
        Try
            Dim col As GridColumn = e.Column

            col.AutoPostBackOnFilter = True
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
