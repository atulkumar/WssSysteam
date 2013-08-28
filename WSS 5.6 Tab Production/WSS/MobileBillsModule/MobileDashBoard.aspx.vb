Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI
Imports Telerik.Charting
Imports System.Drawing

Partial Class MobileBillsModule_mobileDashBoard
    Inherits System.Web.UI.Page
    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString
    Dim objSql As SqlConnection = New SqlConnection(strConnection)
    Dim dsMobileNo As DataSet


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack = False) Then
            dsMobileNo = fetchMobileNumber()
            If dsMobileNo.Tables(0).Rows.Count = 0 Then

            Else
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
            End If
            fillMonthDDL()
            FillGridIonApproved()
            FillGridNotIonApproved()
            FillAnalysisChart()
        End If
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not txthiddenImage Is Nothing Then
            If txthiddenImage.ToString() = "Logout" Then
                LogoutWSS()
            End If
        End If
    End Sub
    Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged

        FillGridIonApproved()
        FillGridNotIonApproved()

    End Sub
    Protected Sub ddlMobileNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMobileNo.SelectedIndexChanged
        Session("MobileNumber") = ddlMobileNo.SelectedValue
        FillGridIonApproved()
        FillGridNotIonApproved()
        FillAnalysisChart()
    End Sub

    'Protected Sub rcAnalysis_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles rcAnalysis.DataBound
    '    ' HideLabels(RadChart1)
    'End Sub

    Private Sub FillGridIonApproved()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select top 5 count(Called_num) as Number,Called_num,sum(Charges_Amt) as Charges_Amt  from T610121 a, T610031 b where a.owner_num=b.mobileNo and b.[user_id]='" & HttpContext.Current.Session("PropUserID") & "' and a.FileID_Int = '" & ddlMonth.SelectedItem.Value & "'  and Official_Flag='C' and a.Owner_Num='" & Session("MobileNumber") & "' group by Called_num order by Charges_Amt desc"


            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)

            rgTop5CalledNoIonApproved.DataSource = dsValues.Tables(0)
            rgTop5CalledNoIonApproved.DataBind()
            ' rgTop5CalledNoIonApproved.MasterTableView.Items(0).BackColor = Color.Red

            objSql.Close()
            If (dsValues.Tables(0).Rows.Count = 0) Then
                rgTop5CalledNoIonApproved.Visible = False
            Else
                rgTop5CalledNoIonApproved.Visible = True
                rgTop5CalledNoIonApproved.MasterTableView.Items(0).BackColor = Color.Red
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillAnalysisChart()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select a.Total_Amt,ltrim(right(convert(varchar(11),Bill_date,113),8)) Bill_date from T610101 a inner join T610031 b on b.MobileNo= a.Owner_Num inner join T6100100 c on c.FileID_Int=a.FileID_Int where b.[user_ID] = '" & HttpContext.Current.Session("PropUserID") & "' and a.Owner_Num='" & Session("MobileNumber") & "' order by c.FileID_Int "


            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)
            rcAnalysis.DataSource = dsValues
            rcAnalysis.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    'Protected Sub HideLabels(ByVal radChart As Telerik.Web.UI.RadChart)

    '    For Each series As ChartSeries In radChart.Series
    '        series.Appearance.LabelAppearance.Visible = False
    '    Next
    'End Sub

    Protected Sub rcAnalysis_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles rcAnalysis.DataBound
        'HideLabels(rcAnalysis)
    End Sub

    Private Sub FillGridNotIonApproved()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select top 5 count(Called_num) as Number,Called_num,sum(Charges_Amt) as Charges_Amt from T610121 a, T610031 b where a.owner_num=b.mobileNo and b.[user_id]='" & HttpContext.Current.Session("PropUserID") & "' and a.FileID_Int = '" & ddlMonth.SelectedItem.Value & "' and Official_Flag<>'C' and a.Owner_Num='" & Session("MobileNumber") & "'  group by Called_num order by Charges_Amt desc"


            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)

            rgTop5NoNotIonapproved.DataSource = dsValues.Tables(0)
            rgTop5NoNotIonapproved.DataBind()
            'rgTop5NoNotIonapproved.MasterTableView.Items(0).BackColor = Color.Red
            objSql.Close()
            If (dsValues.Tables(0).Rows.Count = 0) Then
                rgTop5NoNotIonapproved.Visible = False


            Else

                rgTop5NoNotIonapproved.Visible = True
                rgTop5NoNotIonapproved.MasterTableView.Items(0).BackColor = Color.Red
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub fillMonthDDL()
        'Try
        '    'data should be like Jan 2011- Feb 2011

        '    Dim cmd As New SqlCommand
        '    Dim sqlAdp As SqlDataAdapter
        '    Dim dsValues As New DataSet

        '    cmd.CommandText = "select FileID_Int,ltrim(right(convert(varchar(11),Bill_date,113),8)) Bill_date from T6100100 where owner_num='" & Session("MobileNumber") & "' order by FileID_Int desc"

        '    cmd.CommandType = CommandType.Text
        '    cmd.Connection = objSql
        '    If objSql.State = ConnectionState.Closed Then
        '        objSql.Open()
        '    End If

        '    cmd.Connection = objSql
        '    sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
        '    sqlAdp.Fill(dsValues)
        '    If dsValues.Tables.Count > 0 Then
        '        If (dsValues.Tables(0).Rows.Count > 0) Then
        '            ddlMonth.DataSource = dsValues.Tables(0)
        '            ddlMonth.DataTextField = "Bill_date"
        '            ddlMonth.DataValueField = "FileID_Int"
        '            ddlMonth.DataBind()
        '            objSql.Close()
        '        Else
        '            objSql.Close()
        '            cmd = New SqlCommand
        '            cmd.CommandText = "select FileID_Int,ltrim(right(convert(varchar(11),Bill_date,113),8)) Bill_date from T6100100 where owner_num is null order by FileID_Int desc"
        '            cmd.CommandType = CommandType.Text
        '            cmd.Connection = objSql
        '            If objSql.State = ConnectionState.Closed Then
        '                objSql.Open()
        '            End If

        '            cmd.Connection = objSql
        '            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
        '            sqlAdp.Fill(dsValues)

        '            ddlMonth.DataSource = dsValues.Tables(0)
        '            ddlMonth.DataTextField = "Bill_date"
        '            ddlMonth.DataValueField = "FileID_Int"
        '            ddlMonth.DataBind()
        '        End If
        '    End If
        'Catch ex As Exception

        'End Try
        Try
            'data should be like Jan 2011- Feb 2011

            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select FileID_Int,ltrim(right(convert(varchar(11),Bill_date,113),8)) Bill_date from T6100100 where owner_num is null order by FileID_Int desc"

            cmd.CommandType = CommandType.Text
            cmd.Connection = objSql
            If objSql.State = ConnectionState.Closed Then
                objSql.Open()
            End If

            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)

            ddlMonth.DataSource = dsValues.Tables(0)
            ddlMonth.DataTextField = "Bill_date"
            ddlMonth.DataValueField = "FileID_Int"
            ddlMonth.DataBind()
            objSql.Close()
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
    Private Function fetchMobileNumber() As DataSet
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select MobileMasterId,MobileNo from T610031 where User_Id='" & HttpContext.Current.Session("PropUserID") & "'"
            cmd.CommandType = CommandType.Text
            If objSql.State = Data.ConnectionState.Closed Then
                objSql.Open()
            End If
            cmd.Connection = objSql

            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)
            Return dsValues


            objSql.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Function

End Class
