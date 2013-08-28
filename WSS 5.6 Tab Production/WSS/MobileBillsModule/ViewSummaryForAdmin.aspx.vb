Imports System.Data.SqlClient
Imports System.Data

Partial Class MobileBillsModule_ViewSummaryForAdmin
    Inherits System.Web.UI.Page
    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString
    Dim objSql As SqlConnection = New SqlConnection(strConnection)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Page.IsPostBack = False) Then
            fillMonthDDL()
            'refreshDatabase()
            'fillHeaderDetails()
            FillGrid()
            'rgShowAllDetails.Visible = False
        End If

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
        Catch ex As Exception

        End Try

    End Sub
    Private Sub FillGrid()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet
            cmd.CommandText = "select distinct called_num,sum(charges_amt) ChargedAmount from T610121 a, T610031 b where a.owner_num=b.mobileNo and b.[user_id]='" & HttpContext.Current.Session("PropUserID") & "' and a.FileID_Int = '" & ddlMonth.SelectedItem.Value & "' and called_num not in (select MobileNo from T610031) group by called_num"
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
                lstError.Items.Add("No data found ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Else
                rgBillDetails.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
