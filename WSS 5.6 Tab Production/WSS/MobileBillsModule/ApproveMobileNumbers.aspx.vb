Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class MobileBillsModule_ApproveMobileNumbers
    Inherits System.Web.UI.Page
    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
    Dim strConnectionMobileBilling As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString
    Dim objSql As SqlConnection = New SqlConnection(strConnectionMobileBilling)
    Dim objSqlLive As SqlConnection = New SqlConnection(strConnection)
    'Dim objSql As SqlConnection = New SqlConnection("Data Source=ION-125\SQLEXPRESS;database=WSS_ION;User Id=sa;pwd=readsoft1234@;")
    Dim dsOfficialDS As DataSet
    Dim dsValuesWithReportedTo As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            fillMonthDDL()
            fetchDataFromReportedTo()
            FillGrid()
        End If
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not txthiddenImage Is Nothing Then
            If txthiddenImage.ToString() = "Logout" Then
                LogoutWSS()
            End If
        End If
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApprove.Click
        Try
            Dim arrValue As New ArrayList
            For Each Griditem As GridDataItem In rgBillDetails.SelectedItems
                Try
                    fetchMobileNumber(rgBillDetails.Items(Griditem.ItemIndex).Item("U_UserID").Text)
                    Dim strUserID As String = rgBillDetails.Items(Griditem.ItemIndex).Item("UserID").Text
                    Dim strCalledNum As String = rgBillDetails.Items(Griditem.ItemIndex).Item("NumberCalled").Text
                    Dim cmd As New SqlCommand
                    If String.IsNullOrEmpty(ddlMonth.SelectedItem.Value) Then
                        cmd.CommandText = "update T610010 set Approval_Flag='A', Approved_By='" & HttpContext.Current.Session("PropUserID") & "', Approved_date='" & DateTime.Today & "' where Requested_By='" & strUserID & "' and Number='" & strCalledNum & "' and FileID_int in (select max(FileID_int) from T610010 where Requested_By='" & strUserID & "');"
                        cmd.CommandText &= "update T610121 set Official_Flag='A' where FileID_Int in (select max(FileID_int) from T610010 where Requested_By='" & strUserID & "') and owner_num='" & Session("MobileNumberUser") & "' and Called_Num='" & strCalledNum & "'"
                    Else
                        cmd.CommandText = "update T610010 set Approval_Flag='A', Approved_By='" & HttpContext.Current.Session("PropUserID") & "', Approved_date='" & DateTime.Today & "' where Requested_By='" & strUserID & "' and Number='" & strCalledNum & "' and FileID_int='" & ddlMonth.SelectedItem.Value & "';"
                        cmd.CommandText &= "update T610121 set Official_Flag='A' where FileID_Int='" & ddlMonth.SelectedItem.Value & "' and owner_num='" & Session("MobileNumberUser") & "' and Called_Num='" & strCalledNum & "'"
                    End If

                    cmd.CommandType = CommandType.Text
                    cmd.Connection = objSql
                    If objSql.State = ConnectionState.Closed Then
                        objSql.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    objSql.Close()
                Catch ex As Exception
                End Try
            Next
            If btnShowDisApproved.Text = "View To be Approved numbers" Then
                FillGridDisApproved()
            Else
                FillGrid()
            End If
            lstError.Items.Clear()
            lstError.Items.Add("Numbers Approved Successfully..")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnShowDisApproved_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowDisApproved.Click
        Try
            If btnShowDisApproved.Text.Contains("View Disapproved numbers") Then
                btnShowDisApproved.Text = "View To be Approved numbers"
                btnDisApprove.Visible = False
                ddlMonth.Items(ddlMonth.Items.Count - 1).Selected = True
                ddlMonth.Enabled = False
                FillGridDisApproved()

                Exit Sub
            End If
            If btnShowDisApproved.Text.Contains("View To be Approved numbers") Then
                btnShowDisApproved.Text = "View Disapproved numbers"
                btnDisApprove.Visible = True
                ddlMonth.Items(ddlMonth.Items.Count - 1).Selected = True
                ddlMonth.Enabled = True
                FillGrid()
                Exit Sub
            End If
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnDisApprove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisApprove.Click
        Try
            Dim arrValue As New ArrayList
            For Each Griditem As GridDataItem In rgBillDetails.SelectedItems
                Try
                    fetchMobileNumber(rgBillDetails.Items(Griditem.ItemIndex).Item("U_UserID").Text)
                    Dim strUserID As String = rgBillDetails.Items(Griditem.ItemIndex).Item("UserID").Text
                    Dim strCalledNum As String = rgBillDetails.Items(Griditem.ItemIndex).Item("NumberCalled").Text

                    Dim cmd As New SqlCommand
                    If String.IsNullOrEmpty(ddlMonth.SelectedItem.Value) Then
                        cmd.CommandText = "update T610010 set Approval_Flag='R' where Requested_By='" & strUserID & "' and Number='" & strCalledNum & "' and FileID_int in (select max(fileID_int) from T610010 where Requested_By='" & strUserID & "');"
                        cmd.CommandText &= "update T610121 set Official_Flag='R' where FileID_Int in (select max(fileID_int) from T610010 where Requested_By='" & strUserID & "')  and owner_num='" & Session("MobileNumberUser") & "' and Called_Num='" & strCalledNum & "'"
                    Else
                        cmd.CommandText = "update T610010 set Approval_Flag='R' where Requested_By='" & strUserID & "' and Number='" & strCalledNum & "' and FileID_int='" & ddlMonth.SelectedItem.Value & "';"
                        cmd.CommandText &= "update T610121 set Official_Flag='R' where FileID_Int='" & ddlMonth.SelectedItem.Value & "' and owner_num='" & Session("MobileNumberUser") & "' and Called_Num='" & strCalledNum & "'"
                        cmd.CommandType = CommandType.Text
                    End If

                    cmd.Connection = objSql
                    If objSql.State = ConnectionState.Closed Then
                        objSql.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    objSql.Close()
                Catch ex As Exception
                End Try
            Next
            FillGrid()
            lstError.Items.Clear()
            lstError.Items.Add("Numbers Disapproved Successfully..")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub fetchMobileNumber(ByVal strUserID As String)
        Try
            Dim cmd As New SqlCommand
            Dim dsValues As New DataSet
            cmd.CommandText = "select MobileNo from T610031 where User_Id='" & strUserID & "'"
            cmd.CommandType = CommandType.Text
            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql
            Dim mobileNumber As String = cmd.ExecuteScalar.ToString()
            Session("MobileNumberUser") = mobileNumber
            objSql.Close()

            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillGrid()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet
            Dim query As String
            If String.IsNullOrEmpty(ddlMonth.SelectedItem.Value) Then
                If dsValuesWithReportedTo.Tables(0).Rows.Count > 0 Then

                    query = "select U_UserID,upper(Requested_By) as[UserID],Number as [NumberCalled],(Official_Fname + ' ' +  Official_Lname) as [CalledPersonName],Official_Company [Company],Official_Type[CallType]  from T610010 where FileID_int in (select distinct fileID_int from T610010 where fileID_int is not null) and Approval_Flag='W' and  U_userID in ("

                    For i As Integer = 0 To dsValuesWithReportedTo.Tables(0).Rows.Count - 1
                        Dim id As Integer = dsValuesWithReportedTo.Tables(0).Rows(i)("empID")
                        query += Convert.ToString(id) + ","
                    Next
                    query = query.Substring(0, query.Length - 1) + ")"
                    cmd.CommandText = query
                End If

            Else
                query = "select U_UserID,upper(Requested_By) as[UserID],Number as [NumberCalled],(Official_Fname + ' ' +  Official_Lname) as [CalledPersonName],Official_Company [Company],Official_Type[CallType]  from T610010  where FileID_int = '" & ddlMonth.SelectedItem.Value & "' and Approval_Flag='W' and U_userID in ("

                If dsValuesWithReportedTo.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To dsValuesWithReportedTo.Tables(0).Rows.Count - 1
                        Dim id As Integer = dsValuesWithReportedTo.Tables(0).Rows(i)("empID")
                        query += Convert.ToString(id) + ","
                    Next
                    query = query.Substring(0, query.Length - 1) + ")"
                    cmd.CommandText = query
                End If
                End If

                If objSql.State = Data.ConnectionState.Closed Then
                    objSql.Open()
                End If
                cmd.Connection = objSql
                sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
                sqlAdp.Fill(dsValues)

                rgBillDetails.DataSource = dsValues.Tables(0)
                rgBillDetails.DataBind()
                objSql.Close()
                If (dsValues.Tables(0).Rows.Count = 0) Then
                    rgBillDetails.Visible = False
                    lstError.Items.Clear()
                    lstError.Items.Add("No data found ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Else
                    rgBillDetails.Visible = True
                    checkLockStatus()
                End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillGridDisApproved()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet
            If String.IsNullOrEmpty(ddlMonth.SelectedItem.Value) Then
                cmd.CommandText = "select U_UserID,upper(Requested_By) as[UserID],Number as [NumberCalled],(Official_Fname + ' ' +  Official_Lname) as [CalledPersonName],Official_Company [Company],Official_Type[CallType]  from T610010 where FileID_int in (select distinct fileID_int from T610010 where fileID_int is not null) and Approval_Flag='R' "

            Else
                cmd.CommandText = "select U_UserID,upper(Requested_By) as[UserID],Number as [NumberCalled],(Official_Fname + ' ' +  Official_Lname) as [CalledPersonName],Official_Company [Company],Official_Type[CallType]  from T610010 where FileID_int  = '" & ddlMonth.SelectedItem.Value & "' and Approval_Flag='R' "
            End If

            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)

            rgBillDetails.DataSource = dsValues.Tables(0)
            rgBillDetails.DataBind()
            objSql.Close()
            If (dsValues.Tables(0).Rows.Count = 0) Then
                rgBillDetails.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("No data found ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Else
                rgBillDetails.Visible = True
                checkLockStatus()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub checkLockStatus()
        Try
            Dim cmd As New SqlCommand
            cmd.CommandText = "SELECT COUNT(*) AS Row_Count FROM T610121 WHERE FileID_Int ='" & ddlMonth.SelectedItem.Value & "' and Lock_Flag='L'"
            If objSql.State = ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.CommandType = CommandType.Text
            cmd.Connection = objSql

            Dim iRows As Integer = Convert.ToInt16(cmd.ExecuteScalar)
            If iRows <> 0 Then
                rgBillDetails.Columns(0).Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub fetchDataFromReportedTo()
        Try
            'data should be like Jan 2011- Feb 2011
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            cmd.CommandText = "select empID from tblReportedTo where reportedToID = '" & HttpContext.Current.Session("PropUserID") & "'"
            cmd.CommandType = CommandType.Text

            If objSqlLive.State = Data.ConnectionState.Closed Then
                objSqlLive.Open()
            End If

            cmd.Connection = objSqlLive
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlLive)
            sqlAdp.Fill(dsValuesWithReportedTo)
            objSqlLive.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub fillMonthDDL()
        Try
            'data should be like Jan 2011- Feb 2011
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select FileID_Int,ltrim(right(convert(varchar(11),Bill_date,113),8)) Bill_date from T6100100 order by FileID_Int desc"
            cmd.CommandType = CommandType.Text

            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If

            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)

            ddlMonth.DataSource = dsValues.Tables(0)
            ddlMonth.DataTextField = "Bill_date"
            ddlMonth.DataValueField = "FileID_Int"
            ddlMonth.DataBind()
            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "All Months"
            ddlMonth.Items.Add(lstItem)
            ddlMonth.Items(ddlMonth.Items.Count - 1).Selected = True
            ddlMonth.Enabled = False
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub imgExportToPDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToPDF.Click
        If rgBillDetails.Visible Then
            If rgBillDetails.Columns.Count <= 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("There is no data in the list to export....")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            rgBillDetails.MasterTableView.ExportToPdf()
            Exit Sub
        End If
    End Sub
    Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
        FillGrid()
        checkLockStatus()
    End Sub
End Class
