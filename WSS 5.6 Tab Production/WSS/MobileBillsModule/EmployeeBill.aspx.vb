Imports System.Data
Imports System.Data.SqlClient
Partial Class MobileBillsModule_EmployeeBill
    Inherits System.Web.UI.Page
    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString
    Dim objSql As SqlConnection = New SqlConnection(strConnection)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                fillCallAmountDetails()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub fillCallAmountDetails()
        Try
            Dim strBillCharges As String = Request.QueryString("BillAmt").ToString()
            Dim strFileID As String = Request.QueryString("FileID").ToString()
            Dim cmd As New SqlCommand

            cmd.CommandText = "select intLimit from T610031 where User_Id='" & HttpContext.Current.Session("PropUserID") & "'"
            If objSql.State = ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql

            lblEmpNumber.Text = Session("MobileNumber")
            lblOnBillCharges.Text = strBillCharges
            ' Setting PERMISSIBLE BILL LIMIT''''''''''
            Dim strBillLimit As String = cmd.ExecuteScalar.ToString()
            cmd.Dispose()
            lblPermissibleLimit.Text = strBillLimit

            cmd = New SqlCommand("select sum(charges_amt) as TotalApprovedCharges from T610121 where Owner_Num ='" & Session("MobileNumber") & "' and FileID_Int ='" & strFileID & "' and (Official_Flag='A' or Official_Flag='C')")
            cmd.Connection = objSql

            'Setting APPROVED CALLS AMOUNT'''''
            Dim strApprovedCalls As String = cmd.ExecuteScalar.ToString()
            cmd.Dispose()
            lblApprovedOfficialCalls.Text = strApprovedCalls

            'Setting BALANCE PERSONAl''''''''''
            lblBalancePersonal.Text = Convert.ToString(Convert.ToDecimal(strBillCharges) - Convert.ToDecimal(strApprovedCalls))

            'Setting TO BE PAID AMOUNT''
            lblToBePaid.Text = Convert.ToString(Convert.ToDecimal(lblBalancePersonal.Text) - Convert.ToDecimal(lblPermissibleLimit.Text))

            'Setting WAITING APPROVAL OF CALLS

            cmd = New SqlCommand("select isnull(sum(charges_amt),0) as TotalApprovedCharges from T610121 where Owner_Num ='" & Session("MobileNumber") & "' and FileID_Int ='" & strFileID & "' and Official_Flag='W'")
            cmd.Connection = objSql

            'Setting APPROVED CALLS AMOUNT'''''
            Dim strWaitingApprovalCalls As String = cmd.ExecuteScalar.ToString()
            cmd.Dispose()
            lblWaitingApprovalCallsAmt.Text = strWaitingApprovalCalls

            lblTotalBillAmtToBePaid.Text = Convert.ToString(Convert.ToDecimal(lblToBePaid.Text) - Convert.ToDecimal(strWaitingApprovalCalls))

            objSql.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Sub
End Class
