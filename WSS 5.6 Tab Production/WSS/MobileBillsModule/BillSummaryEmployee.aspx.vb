Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI

Partial Class MobileBillsModule_BillSummaryEmployee
    Inherits System.Web.UI.Page
    Dim strConnectionMobileBilling As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString
    'Dim objSqlMobileBill As SqlConnection = New SqlConnection(strConnection)
    Dim objSqlMobileBill As SqlConnection = New SqlConnection(strConnectionMobileBilling)

    'Dim objSqlMobileBill As SqlConnection = New SqlConnection("Data Source=ION-125\SQLEXPRESS;database=WSS_ION;User Id=sa;pwd=readsoft1234@;")
    Dim dsOfficialDS As DataSet
    Dim dsMobileNo As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack = False) Then
            dsMobileNo = fetchMobileNumber()
            If dsMobileNo.Tables(0).Rows.Count > 1 Then
                ddlMobileNo.DataSource = dsMobileNo.Tables(0)
                ddlMobileNo.DataTextField = "MobileNo"
                ddlMobileNo.DataValueField = "MobileNo"
                ddlMobileNo.DataBind()
                Session("MobileNumber") = ddlMobileNo.SelectedValue
            Else
                Session("MobileNumber") = dsMobileNo.Tables(0).Rows(0)(1).ToString()
                lblMobileNo.Visible = False
                ddlMobileNo.Visible = False
            End If
            fillMonthDDL()
            'refreshDatabase()
            fillHeaderDetails()
            fillCallAmountDetails()
            FillGrid()
            rgShowAllDetails.Visible = False


        End If
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not txthiddenImage Is Nothing Then
            If txthiddenImage.ToString() = "Logout" Then
                LogoutWSS()
            End If
        End If

    End Sub
    Private Sub checkLockStatus()
        Try
            Dim cmd As New SqlCommand
            cmd.CommandText = "SELECT COUNT(*) AS Row_Count FROM T610121 WHERE FileID_Int ='" & ddlMonth.SelectedItem.Value & "' and Lock_Flag='L'"
            If objSqlMobileBill.State = ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.CommandType = CommandType.Text
            cmd.Connection = objSqlMobileBill

            Dim iRows As UInt32 = Convert.ToUInt32(cmd.ExecuteScalar)
            If iRows <> 0 Then
                rgBillDetails.Columns(0).Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function fetchMobileNumber() As DataSet
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select MobileMasterId,MobileNo from T610031 where User_Id='" & HttpContext.Current.Session("PropUserID") & "'"
            cmd.CommandType = CommandType.Text
            If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill

            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)
            Return dsValues


            objSqlMobileBill.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Function
    Private Sub fillMonthDDL()
        Try
            'data should be like Jan 2011- Feb 2011

            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select FileID_Int,ltrim(right(convert(varchar(11),Bill_date,113),8)) Bill_date from T6100100 where owner_num is null order by FileID_Int desc"

            cmd.CommandType = CommandType.Text
            cmd.Connection = objSqlMobileBill
            If objSqlMobileBill.State = ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If

            cmd.Connection = objSqlMobileBill
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)

            ddlMonth.DataSource = dsValues.Tables(0)
            ddlMonth.DataTextField = "Bill_date"
            ddlMonth.DataValueField = "FileID_Int"
            ddlMonth.DataBind()
            'If dsValues.Tables.Count > 0 Then
            '    If (dsValues.Tables(0).Rows.Count > 0) Then
            '        ddlMonth.DataSource = dsValues.Tables(0)
            '        ddlMonth.DataTextField = "Bill_date"
            '        ddlMonth.DataValueField = "FileID_Int"
            '        ddlMonth.DataBind()
            '        objSqlMobileBill.Close()
            '    Else
            '        objSqlMobileBill.Close()

            '    End If
            'End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub fillHeaderDetails()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet
            cmd.CommandText = "select * from T610101 a inner join T610031 b on b.MobileNo= a.Owner_Num inner join T6100100 c on c.FileID_Int=a.FileID_Int where b.[user_ID] = '" & HttpContext.Current.Session("PropUserID") & "' and a.FileID_Int = '" & ddlMonth.SelectedItem.Value & "' and a.Owner_Num='" & Session("MobileNumber") & "'"
            If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)
            Session("owner_num") = dsValues.Tables(0).Rows(0)("Owner_Num").ToString()
            'lblAccountNumber.Text = dsValues.Tables(0).Rows(0)("Owner_Acct").ToString()
            'lblMobileNumber.Text = dsValues.Tables(0).Rows(0)("MobileNo").ToString()
            'lblOneTimeCharges.Text = "0.00" 'dsValues.Tables(0).Rows(0)("Owner_Acct").ToString()
            lblMonthlyCharges.Text = dsValues.Tables(0).Rows(0)("Monthly_Amt").ToString()
            lblCallCharges.Text = dsValues.Tables(0).Rows(0)("Call_Amt").ToString()
            lblValueAddedCharges.Text = dsValues.Tables(0).Rows(0)("VAS_Amt").ToString()
            lblRoamingCharges.Text = dsValues.Tables(0).Rows(0)("Roaming_amt").ToString()
            lblDiscount.Text = dsValues.Tables(0).Rows(0)("Discount_Amt").ToString()
            lblTax.Text = dsValues.Tables(0).Rows(0)("Tax_amt").ToString()
            lblTotalCharges.Text = dsValues.Tables(0).Rows(0)("Total_Amt").ToString()
            objSqlMobileBill.Close()
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
        fillHeaderDetails()
        FillGrid()
        fillCallAmountDetails()

    End Sub
    Protected Sub ddlMobileNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMobileNo.SelectedIndexChanged
        Session("MobileNumber") = ddlMobileNo.SelectedValue
        fillHeaderDetails()
        fillCallAmountDetails()
        FillGrid()
        rgShowAllDetails.Visible = False
    End Sub
    Private Sub refreshDatabase()
        Try
            dsOfficialDS = New DataSet
            Dim cmd As New SqlCommand
            Dim adp As SqlDataAdapter
            cmd.CommandText = "select Number from T610010 where U_UserID = '" & HttpContext.Current.Session("PropUserID") & "'"
            adp = New SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            If objSqlMobileBill.State = ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            adp.Fill(dsOfficialDS)
            cmd.Dispose()
            objSqlMobileBill.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillGrid()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select distinct called_num,isnull(dbo.getNameAndCompanyFromNumber(called_num,owner_num), ' ') as [CalledPersonName], sum(charges_amt) ChargedAmount,case when Official_Flag='C' then 'ION-Approved' when Official_Flag = 'W' then 'Waiting Approval' when Official_Flag = 'A' then 'TL Approved' when Official_Flag = 'R' then 'TL Rejected' else 'Personal' end Official_Flag from T610121 a where a.FileID_Int = '" & ddlMonth.SelectedItem.Value & "' and a.owner_num='" & Session("MobileNumber") & "' group by called_num,owner_num,Official_Flag order by Official_Flag desc"


            If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)

            rgBillDetails.DataSource = dsValues.Tables(0)
            rgBillDetails.DataBind()
            objSqlMobileBill.Close()
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
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
    Protected Sub btnShowAllDetails_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAllDetails.Click
        Try
            'refreshDatabase()
            If btnShowAllDetails.Text.Contains("View complete call details for the selected month") Then
                btnShowAllDetails.Text = "Click here to mark numbers as Official/Personal"
                btnUpdate.Visible = False
                rgBillDetails.Visible = False
                rgShowAllDetails.Visible = True
                lblLabelDetails.Text = "Call Details"
                FillAllDetailsGrid()
                Exit Sub
            End If
            If btnShowAllDetails.Text.Contains("Click here to mark numbers as Official/Personal") Then
                btnShowAllDetails.Text = "View complete call details for the selected month"
                btnUpdate.Visible = False
                rgBillDetails.Visible = True
                rgShowAllDetails.Visible = False
                lblLabelDetails.Text = "Mark Number Official"
                FillGrid()
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillAllDetailsGrid()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet
            cmd.CommandText = "select Called_num,convert(varchar(11),Called_Date,113)Called_Date,convert(varchar(8),Called_Time,114)Called_Time,Duration_vol,Charges_Amt,case when Official_Flag='C' then 'ION-Approved' when Official_Flag = 'W' then 'Waiting Approval' when Official_Flag = 'A' then 'TL Approved' when Official_Flag = 'R' then 'TL Rejected' else 'Personal' end Official_Flag , c.Category_Desc from T610121 a, T610031 b, T610131 c where a.owner_num=b.mobileNo and b.[user_id]='" & HttpContext.Current.Session("PropUserID") & "' and a.FileID_Int = '" & ddlMonth.SelectedItem.Value & "' and b.MobileNo='" & Session("MobileNumber") & "' and c.category_id=a.cat1 order by Id"
            If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)

            rgShowAllDetails.DataSource = dsValues.Tables(0).DefaultView
            rgShowAllDetails.DataBind()
            objSqlMobileBill.Close()
            If dsValues.Tables(0).Rows.Count = 0 Then
                rgShowAllDetails.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("No data found ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Else
                rgShowAllDetails.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try

            For Each Griditem As GridDataItem In rgBillDetails.SelectedItems
                Dim strNumber As String = rgBillDetails.Items(Griditem.ItemIndex).Item("CalledNumber").Text
                Dim cmdCount As New SqlCommand
                cmdCount.CommandText = "select count(*) from T610010 where U_UserID = '" & HttpContext.Current.Session("PropUserID") & "' and number='" & strNumber & "'"
                cmdCount.CommandType = CommandType.Text
                cmdCount.Connection = objSqlMobileBill
                If objSqlMobileBill.State = ConnectionState.Closed Then
                    objSqlMobileBill.Open()
                End If
                Dim strValue As String = cmdCount.ExecuteScalar.ToString()
                If Convert.ToInt16(strValue) <= 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Kindly add official details first the the selected Item(s)....")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            Next
            refreshDatabase()
            FillGrid()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub rgBillDetails_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgBillDetails.ItemCreated
        Try


            If TypeOf e.Item Is GridDataItem Then
                Dim editLink As HyperLink = DirectCast(e.Item.FindControl("EditLink"), HyperLink)
                Dim editLink1 As HyperLink = DirectCast(e.Item.FindControl("EditLink"), HyperLink)

                editLink.Attributes("href") = "#"
                editLink.Attributes("onclick") = [String].Format("return ShowEditForm('{0}','{1}','{2}');", CType(e.Item.DataItem, DataRowView)("Called_Num"), e.Item.ItemIndex, ddlMonth.SelectedItem.Value.ToString())

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub RadAjaxManager1_AjaxRequest(ByVal sender As Object, ByVal e As AjaxRequestEventArgs)
        If e.Argument = "Rebind" Then
            rgBillDetails.MasterTableView.SortExpressions.Clear()
            rgBillDetails.MasterTableView.GroupByExpressions.Clear()
            rgBillDetails.Rebind()
        ElseIf e.Argument = "RebindAndNavigate" Then
            rgBillDetails.MasterTableView.SortExpressions.Clear()
            rgBillDetails.MasterTableView.GroupByExpressions.Clear()
            rgBillDetails.MasterTableView.CurrentPageIndex = rgBillDetails.MasterTableView.PageCount - 1
            rgBillDetails.Rebind()
        End If
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
        If rgShowAllDetails.Visible Then
            If rgShowAllDetails.Columns.Count <= 0 Then
                lstError.Items.Clear()
                lstError.Items.Add("There is no data in the list to export....")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            rgShowAllDetails.MasterTableView.ExportToPdf()
            Exit Sub
        End If
    End Sub
    Private Sub fillCallAmountDetails()
        Try
            Dim strBillCharges As String = lblTotalCharges.Text 'Request.QueryString("BillAmt").ToString()
            Dim strFileID As String = ddlMonth.SelectedItem.Value 'Request.QueryString("FileID").ToString()
            Dim cmd As New SqlCommand

            cmd.CommandText = "select intLimit from T610031 where User_Id='" & HttpContext.Current.Session("PropUserID") & "' and  MobileNo='" & Session("MobileNumber") & "' "
            If objSqlMobileBill.State = ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill

            lblEmpNumber.Text = Session("MobileNumber")
            lblOnBillCharges.Text = strBillCharges
            ' Setting PERMISSIBLE BILL LIMIT''''''''''
            Dim strBillLimit As String = cmd.ExecuteScalar.ToString()
            cmd.Dispose()
            lblPermissibleLimit.Text = strBillLimit

            cmd = New SqlCommand("select isnull(sum(charges_amt),0.00) as TotalApprovedCharges from T610121 where Owner_Num ='" & Session("MobileNumber") & "' and FileID_Int ='" & strFileID & "' and (Official_Flag='A')")
            cmd.Connection = objSqlMobileBill

            'Setting APPROVED CALLS AMOUNT'''''
            Dim strApprovedCalls As String = cmd.ExecuteScalar.ToString()
            cmd.Dispose()
            Dim dTaxAmt As Decimal
            Try
                dTaxAmt = Math.Round((Convert.ToDecimal(strApprovedCalls) * 10.3) / 100, 2)
                lblApprovedOfficialCalls.Text = Convert.ToString(Convert.ToDecimal(strApprovedCalls) + dTaxAmt) & " (Actual Amount: " & strApprovedCalls & ")"

            Catch ex As Exception
            End Try

            strApprovedCalls = Convert.ToString(Convert.ToDecimal(strApprovedCalls) + dTaxAmt) ' lblApprovedOfficialCalls.Text

            'Setting BALANCE PERSONAl''''''''''
            lblBalancePersonal.Text = Convert.ToString(Convert.ToDecimal(strBillCharges) - Convert.ToDecimal(strApprovedCalls))

            'Setting TO BE PAID AMOUNT''
            lblToBePaid.Text = Convert.ToString(Convert.ToDecimal(lblBalancePersonal.Text) - Convert.ToDecimal(lblPermissibleLimit.Text))

            'Setting WAITING APPROVAL OF CALLS

            cmd = New SqlCommand("select isnull(sum(charges_amt),0) as TotalApprovedCharges from T610121 where Owner_Num ='" & Session("MobileNumber") & "' and FileID_Int ='" & strFileID & "' and Official_Flag='W'")
            cmd.Connection = objSqlMobileBill

            'Setting APPROVED CALLS AMOUNT'''''
            Dim strWaitingApprovalCalls As String = cmd.ExecuteScalar.ToString()
            cmd.Dispose()
            lblWaitingApprovalCallsAmt.Text = strWaitingApprovalCalls

            lblTotalBillAmtToBePaid.Text = Convert.ToString(Convert.ToDecimal(lblToBePaid.Text))

            objSqlMobileBill.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Sub
End Class
