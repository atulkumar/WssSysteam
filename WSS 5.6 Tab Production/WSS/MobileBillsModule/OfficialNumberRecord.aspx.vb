Imports System.Data.SqlClient
Imports System.Data

Partial Class MobileBillsModule_OfficialNumberRecord
    Inherits System.Web.UI.Page
    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
    Dim strConnectionMobileBill As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString

    Dim objSql As SqlConnection = New SqlConnection(strConnection)
    Dim objSqlMobileBill As SqlConnection = New SqlConnection(strConnectionMobileBill)

    Private strState As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            fillDropDownOfficialType()
            checkValues()
            Me.Page.Title &= " for number " & Request.QueryString("Called_num").ToString()
        End If
    End Sub
    Private Sub fillDropDownOfficialType()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select * from UDC where UDCType='OTYP'"
            cmd.CommandType = CommandType.Text
            cmd.Connection = objSql
            If objSql.State = ConnectionState.Closed Then
                objSql.Open()
            End If

            cmd.Connection = objSql
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSql)
            sqlAdp.Fill(dsValues)

            ddlOfficialType.DataSource = dsValues.Tables(0)
            ddlOfficialType.DataTextField = "Name"
            ddlOfficialType.DataValueField = "Name"
            ddlOfficialType.DataBind()
            ddlOfficialType.Items.Add("--Select--")
            ddlOfficialType.Items(ddlOfficialType.Items.Count - 1).Selected = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub checkValues()
        Try
            Dim strMobNo As String = Request.QueryString("Called_num").ToString()
            Dim strMyNum As String = Request.QueryString("myNum").ToString()
            Dim dsResult As New DataSet
            Dim cmd As New SqlCommand
            Dim adp As SqlDataAdapter
            cmd.CommandText = "select * from T610010 where number = '" & strMobNo & "' and U_UserID='" & HttpContext.Current.Session("PropUserID") & "'"
            adp = New SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            If objSqlMobileBill.State = ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            adp.Fill(dsResult)
            If (dsResult.Tables.Count > 0) Then
                If (dsResult.Tables(0).Rows.Count > 0) Then
                    txtFirstName.Text = dsResult.Tables(0).Rows(0)("Official_Fname")
                    txtLastName.Text = dsResult.Tables(0).Rows(0)("Official_Lname")
                    txtCompanyName.Text = dsResult.Tables(0).Rows(0)("Official_Company")
                    ddlOfficialType.SelectedValue = dsResult.Tables(0).Rows(0)("Official_Type")
                    ViewState("strState") = "Update"
                Else
                    ViewState("strState") = "Insert"
                End If
            Else
                ViewState("strState") = "Insert"
            End If
            objSqlMobileBill.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try
            If validateData() Then
                Dim strCalledNo As String = Request.QueryString("Called_num").ToString()
                Dim strFileID As String = Request.QueryString("myNum").ToString()
                Dim cmd As SqlCommand
                If (ViewState("strState").ToString() = "Update") Then
                    cmd = New SqlCommand
                    cmd.CommandText = "update T610010 set Official_Fname ='" & txtFirstName.Text & "',Official_Lname ='" & txtLastName.Text & "',  Official_Company ='" & txtCompanyName.Text & "' ,Official_Type = '" & ddlOfficialType.SelectedItem.Value & "' where Number='" & strCalledNo & "' and U_UserID='" & HttpContext.Current.Session("PropUserID") & "'"
                Else
                    cmd = New SqlCommand
                    Dim strSQL As String = "insert into T610010 (Number,Requested_By,Requested_Date,Official_Fname,Official_Lname,Official_Company,Official_Type,Approval_Flag,U_UserID,U_Date,FileID_int) values('" & strCalledNo & "','" & HttpContext.Current.Session("PropUserName") & "','" & DateTime.Today & "','" & txtFirstName.Text & "', '" & txtLastName.Text & "','" & txtCompanyName.Text & "','" & ddlOfficialType.SelectedItem.Value & "','W','" & HttpContext.Current.Session("PropUserID") & "','" & DateTime.Today & "','" & strFileID & "');"
                    strSQL &= "update T610121 set Official_Flag='W' where FileID_Int='" & strFileID & "' and Called_Num='" & strCalledNo & "' and Owner_Num='" & Session("MobileNumber") & "';"
                    cmd.CommandText = strSQL
                End If
                If objSqlMobileBill.State = ConnectionState.Closed Then
                    objSqlMobileBill.Open()
                End If
                cmd.Connection = objSqlMobileBill
                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()
                objSqlMobileBill.Close()
                cmd.Dispose()
                lstError.Items.Clear()
                lstError.Items.Add("Data updated...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Function validateData() As Boolean
        Try
            lstError.Items.Clear()
            If txtFirstName.Text = "" Then
                lstError.Items.Add("Please enter first name ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If
            If txtLastName.Text = "" Then
                lstError.Items.Add("Please enter last name ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If
            If txtCompanyName.Text = "" Then
                lstError.Items.Add("Please enter company name ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return False
                Exit Function
            End If
           
            Return True
        Catch ex As Exception

        End Try
    End Function
End Class
