Imports System.Data.SqlClient
Imports System.Data
Imports Telerik.Web.UI
Partial Class MobileBillsModule_TraceMobileNo
    Inherits System.Web.UI.Page
    Dim strConnectionMobileBilling As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringMobileBillingModule").ToString
    Dim objSqlMobileBill As SqlConnection = New SqlConnection(strConnectionMobileBilling)


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Panel1.Visible = False
        If IsPostBack = False Then
            FillRadCombos()
        End If
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not txthiddenImage Is Nothing Then
            If txthiddenImage.ToString() = "Logout" Then
                LogoutWSS()

            End If
        End If
    End Sub
    Private Sub FillRadCombos()
        Dim dsComboMobileNo As New DataSet
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select MobileNo,user_Name from t610031"
            If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)
            CDDLmobileNo.Items.Clear()
            CDDLmobileNo.DataSource = dsValues
            For Each data As DataRow In dsValues.Tables(0).Rows
                Dim item As RadComboBoxItem = New RadComboBoxItem()
                item.Text = CStr(data("user_Name"))
                item.Value = CStr(data("MobileNo"))
                CDDLmobileNo.Items.Add(item)
                item.DataBind()
            Next
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TraceIonNumber()
        Try
            If CDDLmobileNo.SelectedIndex <> -1 Then
                Panel2.Visible = False
                Dim flagValidate As String = validateControls()
                If flagValidate = "False" Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please fill all the fields ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Panel1.Visible = False
                ElseIf flagValidate = "To date should not be less than From date ..." Then
                    lstError.Items.Clear()
                    lstError.Items.Add("To date should not be less than From date ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Else
                    fillThreeGrids()
                    Panel1.Visible = True

                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please select Employee Name...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
            
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Try
            
            If ddlCategory.SelectedValue = 1 Then
                TraceIonNumber()
            ElseIf ddlCategory.SelectedValue = 2 Then
                TraceIonNoCallToOtherNo()
            ElseIf ddlCategory.SelectedValue = 3 Then
                TraceOtherNumber()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please select Category...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TraceIonNoCallToOtherNo()
        Try
            If CDDLmobileNo.SelectedIndex <> -1 Then
                Dim flagValidate As String = validateControls()
                If flagValidate = "False" Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Please fill all the fields ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Panel1.Visible = False
                ElseIf flagValidate = "To date should not be less than From date ..." Then
                    lstError.Items.Clear()
                    lstError.Items.Add("To date should not be less than From date ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                ElseIf txtCalledNo.Text = "" Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Fill Called Number ...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Else
                    showDetailForParticularNo()
                    Panel2.Visible = True
                    Panel4.Visible = True
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please select Employee Name...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
            Panel1.Visible = False
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub rgParticularNoDetail_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rgParticularNoDetail.SelectedIndexChanged
        Try
            detailOfNoAccToCalledNo()
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub TraceOtherNumber()
        Try
            Dim flagValidate As String = validateControls()
            If txtMobileNo.Text = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Enter Mobile Number ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Panel1.Visible = False
            ElseIf flagValidate = False Then
                lstError.Items.Clear()
                lstError.Items.Add("Please fill all the fields ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Panel1.Visible = False
            ElseIf flagValidate = "To date should not be less than From date ..." Then
                lstError.Items.Clear()
                lstError.Items.Add("To date should not be less than From date ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Else
                pnlCallingDetail.Visible = True
                getCallingDetail()
            End If
            Panel1.Visible = False
            Panel4.Visible = False
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        clearFeilds()
        If ddlCategory.SelectedValue = 1 Then
            Panel3.Visible = False
            txtMobileNo.Visible = False
            CDDLmobileNo.Visible = True
            Panel4.Visible = False
            Panel2.Visible = False
            pnlCallingDetail.Visible = False
        ElseIf ddlCategory.SelectedValue = 2 Then
            txtMobileNo.Visible = False
            Panel1.Visible = False
            Panel4.Visible = True
            pnlCallingDetail.Visible = False
            CDDLmobileNo.Visible = True
        ElseIf ddlCategory.SelectedValue = 3 Then
            Panel1.Visible = False
            Panel4.Visible = False
            Panel3.Visible = False
            Panel2.Visible = False
            txtMobileNo.Visible = True
            CDDLmobileNo.Visible = False
            lblMobileNo.Text = "Enter No"
        End If
    End Sub

    Protected Sub rgcallingDetail_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgcallingDetail.NeedDataSource
        pnlCallingDetail.Visible = True
        getCallingDetail()
    End Sub
    Protected Sub rgParticularNoDetail_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgParticularNoDetail.NeedDataSource
        showDetailForParticularNo()
    End Sub
    Protected Sub clearFeilds()
        txtCalledNo.Text = ""
        txtMobileNo.Text = ""
        ddlTime.SelectedValue = 0
    End Sub
    Private Sub fillThreeGrids()
        Try
            Dim cmd As New SqlCommand
            Dim sqlAdp As SqlDataAdapter
            Dim dsValues As New DataSet

            cmd.CommandText = "select distinct called_num,isnull(dbo.getNameAndCompanyFromNumber(called_num,owner_num), ' ') as [CalledPersonName], sum(charges_amt) ChargedAmount,case when Official_Flag='C' then 'ION-Approved' when Official_Flag = 'W' then 'Waiting Approval' when Official_Flag = 'A' then 'TL Approved' when Official_Flag = 'R' then 'TL Rejected' else 'Personal' end Official_Flag from T610121 a where Called_Date>'" & dtFromDate.Text & "' and Called_Date<'" & dtToDate.Text & "' and  a.owner_num='" & CDDLmobileNo.SelectedValue & "' group by called_num,owner_num,Official_Flag order by Official_Flag desc;select top 5 count(Called_num) as Number,Called_num,sum(Charges_Amt) as Charges_Amt from T610121 a, T610031 b where a.owner_num=b.mobileNo and  a.Called_Date>'" & dtFromDate.Text & "' and a.Called_Date<'" & dtToDate.Text & "' and a.Owner_Num='" & CDDLmobileNo.SelectedValue & "'  group by Called_num order by Charges_Amt desc;select distinct called_num,isnull(dbo.getNameAndCompanyFromNumber(called_num,owner_num), ' ') as [CalledPersonName], sum(charges_amt) ChargedAmount,isnull(dbo.getCategory(Cat1, Cat2),' ') as [Category],case when Official_Flag='C' then 'ION-Approved' when Official_Flag = 'W' then 'Waiting Approval' when Official_Flag = 'A' then 'TL Approved' when Official_Flag = 'R' then 'TL Rejected' else 'Personal' end Official_Flag from T610121 a,T610131 b where b.Category_id=Cat1 and  Called_Date>'" & dtFromDate.Text & "' and Called_Date<'" & dtToDate.Text & "' and  a.owner_num='" & CDDLmobileNo.SelectedValue & "' group by called_num,owner_num,Official_Flag,Category_Desc,  Cat1 ,Cat2 order by Official_Flag desc"


            If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                objSqlMobileBill.Open()
            End If
            cmd.Connection = objSqlMobileBill
            sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
            sqlAdp.Fill(dsValues)

            rgCallDetail.DataSource = dsValues.Tables(0)
            rgCallDetail.DataBind()
            rgTop5Number.DataSource = dsValues.Tables(1)
            rgTop5Number.DataBind()
            rgCategoryWise.DataSource = dsValues.Tables(2)
            rgCategoryWise.DataBind()
            objSqlMobileBill.Close()
            If (dsValues.Tables(0).Rows.Count = 0) Then
                rgCallDetail.Visible = False
                Panel1.Visible = False
                lstError.Items.Clear()
                lstError.Items.Add("No data found ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Else
                rgCallDetail.Visible = True

            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub showDetailForParticularNo()
        Try
            Dim flag As Boolean = validateControls()
            If flag = True Then
                Dim cmd As New SqlCommand
                Dim sqlAdp As SqlDataAdapter
                Dim dsValues As New DataSet
                cmd.CommandText = "select  called_num,Called_Time,CONVERT(CHAR(10), Called_Date, 101) as calledDate,Duration_Vol,isnull(dbo.getNameAndCompanyFromNumber(called_num,owner_num), ' ') as [CalledPersonName],isnull(dbo.getCategory(Cat1, Cat2),' ') as [Category], charges_amt as ChargedAmount,case when Official_Flag='C' then 'ION-Approved' when Official_Flag = 'W' then 'Waiting Approval' when Official_Flag = 'A' then 'TL Approved' when Official_Flag = 'R' then 'TL Rejected' else 'Personal' end Official_Flag from T610121 a,T610131 b where  b.Category_id=Cat1 and Called_Date>'" & dtFromDate.Text & "' and Called_Date<'" & dtToDate.Text & "' and  a.owner_num='" & CDDLmobileNo.SelectedValue & "' and called_num='" & txtCalledNo.Text & "' order by Official_Flag desc"
                If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                    objSqlMobileBill.Open()
                End If
                cmd.Connection = objSqlMobileBill
                sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
                sqlAdp.Fill(dsValues)
                rgParticularNoDetail.DataSource = dsValues.Tables(0)
                rgParticularNoDetail.DataBind()
                Panel2.Visible = True
                RadGrid1.Visible = False
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please fill all the fields ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If

        Catch ex As Exception

        End Try

    End Sub
    Protected Function validateControls() As String
     
        If dtFromDate.Text = "" Then
            Return "False"
        ElseIf dtToDate.Text = "" Then
            Return "False"
        ElseIf Convert.ToDateTime(dtFromDate.Text) > Convert.ToDateTime(dtToDate.Text) Then
            Return "To date should not be less than From date ..."
        Else
            Return "True"
        End If

    End Function

    Private Sub detailOfNoAccToCalledNo()
        Try
            If ddlTime.SelectedValue <> 0 Then
                Dim selectedItem As GridDataItem = rgParticularNoDetail.SelectedItems(0)
                Dim strDate As String = selectedItem(rgParticularNoDetail.MasterTableView.Columns(3)).Text()
                Dim strTime As String = selectedItem(rgParticularNoDetail.MasterTableView.Columns(4)).Text()
                Dim datetime As String = strDate + " " + strTime
                Panel3.Visible = True
                Dim cmd As New SqlCommand
                Dim sqlAdp As SqlDataAdapter
                Dim dsValues As New DataSet
                cmd.CommandText = "select  called_num,Called_Time,CONVERT(CHAR(10), Called_Date, 101) as calledDate,Duration_Vol,isnull(dbo.getNameAndCompanyFromNumber(called_num,owner_num), ' ') as [CalledPersonName],isnull(dbo.getCategory(Cat1, Cat2),' ') as [Category], charges_amt as ChargedAmount,case when Official_Flag='C' then 'ION-pproved' when Official_Flag = 'W' then 'Waiting Approval' when Official_Flag = 'A' then 'TL Approved' when Official_Flag = 'R' then 'TL Rejected'else 'Personal' end Official_Flag from T610121 a,T610131 b where b.Category_id=Cat1 and Called_Date='" & strDate & "' and  a.owner_num='" & CDDLmobileNo.SelectedValue & "' and Called_Time>CONVERT(VARCHAR(8),dateadd(n,0,'" & strTime & "'),108) and Called_Time<CONVERT(VARCHAR(8),dateadd(n," & ddlTime.SelectedValue & ",'" & strTime & "'),108) order by Official_Flag desc"
                If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                    objSqlMobileBill.Open()
                End If
                cmd.Connection = objSqlMobileBill
                sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
                sqlAdp.Fill(dsValues)
                RadGrid1.DataSource = dsValues
                RadGrid1.DataBind()

                RadGrid1.Visible = True
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please select time duration ...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub getCallingDetail()
        Try
            If txtMobileNo.Text <> "" Then
                Dim cmd As New SqlCommand
                Dim sqlAdp As SqlDataAdapter
                Dim dsValues As New DataSet
                cmd.CommandText = "select  owner_num,User_Name,User_Id,CONVERT(CHAR(10), Called_Date, 101) as calledDate,Called_Time,Duration_Vol,isnull(dbo.getCategory(Cat1, Cat2),' ') as [Category]  ,charges_amt as ChargedAmount from T610121 a,T610131 b,T610031 c  where b.Category_id=Cat1 and called_num='" & txtMobileNo.Text & "' and c.MobileNo= a.owner_num and Called_Date>'" & dtFromDate.Text & "' and Called_Date<'" & dtToDate.Text & "' "
                If objSqlMobileBill.State = Data.ConnectionState.Closed Then
                    objSqlMobileBill.Open()
                End If
                cmd.Connection = objSqlMobileBill
                sqlAdp = New SqlClient.SqlDataAdapter(cmd.CommandText, objSqlMobileBill)
                sqlAdp.Fill(dsValues)
                rgcallingDetail.DataSource = dsValues
                rgcallingDetail.DataBind()


            End If
           
        Catch ex As Exception
        End Try
    End Sub

End Class
