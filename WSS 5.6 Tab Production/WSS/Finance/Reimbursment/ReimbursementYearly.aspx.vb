#Region "Purpose"
' CREATED BY: [Saurabh]
' CREATED ON: [30/06/09]
' PURPOSE   : [This Screen is used to save the Yearly Reimbursement Amount]
' TABLES    : [Emp_Reimbursement_Detail]
#End Region

#Region "NameSpace"
Imports ION.Logging.EventLogging
Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Data
Imports Telerik.Web.UI
Imports WSSBLL
#End Region

Partial Class Reimbursement_ReimbursementYearly
    Inherits System.Web.UI.Page

#Region "Variables"
    Private objRMB As New ReimburstmentBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private txthiddenImage As String 'stored clicked button's cation  
#End Region

#Region "PageLoad"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        txthiddenImage = Request.Form("txthiddenImage")
        If Page.IsPostBack = False Then
            ViewState("FinancialYear") = objRMB.GetFinacialYear()
            Call FillRBMYearlyGrid()
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            Catch ex As Exception
                CreateLog("Reimbursement_ReimbursementYearly", "Page_Load-39", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If

    End Sub
#End Region

#Region "Grid Event"
    Protected Sub rgvRBMYearlyRecord_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgvRBMYearlyRecord.NeedDataSource
        Call FillRBMYearlyGrid() 'Done to Enable filtering
    End Sub
    Protected Sub rgvRBMYearlyRecord_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles rgvRBMYearlyRecord.PageIndexChanged
        Call FillRBMYearlyGrid() 'Done to Enable paging
    End Sub
#End Region

#Region "Functions"

#Region "Function to fill grid"
    ''' <summary>
    ''' Function to Fill grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillRBMYearlyGrid()
        Dim dtBindGrid As New Data.DataTable
        Try
            dtBindGrid = objRMB.GetNameEmployees(ViewState("FinancialYear"))
            rgvRBMYearlyRecord.DataSource = dtBindGrid
            rgvRBMYearlyRecord.DataBind()
        Catch ex As Exception
            CreateLog("Reimbursement_ReimbursementYearly", "FillRBMYearlyGrid-69", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "Function to Delete Records wrt Emp ID"
    ''' <summary>
    ''' Function to Delete record based upon Employee ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteRBMAmt()
        Dim vLoop As Integer = 0
        Try
            Do While (vLoop < rgvRBMYearlyRecord.MasterTableView.Items.Count)
                Dim Emp_Id As String = rgvRBMYearlyRecord.MasterTableView.Items(vLoop).Cells(3).Text
                vLoop = (vLoop + 1)
                objRMB.DeleteRBMAmt(Emp_Id, ViewState("FinancialYear"))
            Loop
        Catch ex As Exception
            CreateLog("Reimbursement_Reimbursement", "DeleteRBMAmt-88", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "Function to Save RBM Amount"
    ''' <summary>
    ''' Function to save data on button click 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveRBMAmt()
        Try
            Dim vLoop As Integer = 0
            Dim arrYearly As New ArrayList
            Dim arrMonthly As New ArrayList
            Dim strFinancialYear As String = String.Empty
            Dim strBillYear As String
            Dim intCalMonth As Integer
            Dim EmpId As Integer
            strFinancialYear = objRMB.GetFinacialYear() ' + "/" + (Date.Now.Year + 1)
            If IsNothing(strFinancialYear) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("No Financial Year set please contact Finance Dept....!!")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            strBillYear = Date.Now.Year
            Do While (vLoop < rgvRBMYearlyRecord.MasterTableView.Items.Count)

                Dim gvrow As GridItem = rgvRBMYearlyRecord.MasterTableView.Items(vLoop)
                Dim Emp_Id As String = rgvRBMYearlyRecord.MasterTableView.Items(vLoop).Cells(3).Text
                EmpId = rgvRBMYearlyRecord.MasterTableView.Items(vLoop).Cells(3).Text
                Dim strMedical As String = CType(gvrow.FindControl("rntxtMedical"), RadNumericTextBox).Text
                Dim strTelephone As String = CType(gvrow.FindControl("rntxtTelephone"), RadNumericTextBox).Text
                Dim strLTA As String = CType(gvrow.FindControl("rntxtLTA"), RadNumericTextBox).Text
                Dim intJoinMonth As Integer = Val(CType(gvrow.FindControl("rntxtJoinMonth"), RadNumericTextBox).Text)
                intCalMonth = Val(CType(gvrow.FindControl("rntxtJoinMonth"), RadNumericTextBox).Text)
                If intCalMonth > 0 Then
                    If intCalMonth > 4 Then

                        intCalMonth = 12 - (intCalMonth - 4)
                    Else
                        intCalMonth = 4 - (intCalMonth)
                    End If
                End If


                vLoop = (vLoop + 1)

                '''''checking string values
                If String.IsNullOrEmpty(strMedical) Then
                    arrYearly.Add(0)
                Else
                    arrYearly.Add(strMedical)
                End If

                If String.IsNullOrEmpty(strTelephone) Then
                    arrYearly.Add(0)
                Else
                    arrYearly.Add(strTelephone)
                End If

                If String.IsNullOrEmpty(strLTA) Then
                    arrYearly.Add(0)
                Else
                    arrYearly.Add(strLTA)
                End If

                ''''''checking Array Values
                If String.IsNullOrEmpty(arrYearly(0)) Or arrYearly(0) = 0 Then
                    arrMonthly.Add(0)
                Else
                    If intCalMonth > 0 Then
                        arrMonthly.Add(strMedical / intCalMonth)
                    Else
                        arrMonthly.Add(strMedical / 12)
                    End If

                End If
                If String.IsNullOrEmpty(arrYearly(1)) Or arrYearly(1) = 0 Then
                    arrMonthly.Add(0)
                Else
                    If intCalMonth > 0 Then
                        arrMonthly.Add(strTelephone / intCalMonth)
                    Else
                        arrMonthly.Add(strTelephone / 12)
                    End If

                End If
                If String.IsNullOrEmpty(arrYearly(2)) Or arrYearly(2) = 0 Then
                    arrMonthly.Add(0)
                Else
                    If intCalMonth > 0 Then
                        arrMonthly.Add(strLTA / intCalMonth)
                    Else
                        arrMonthly.Add(strLTA / 12)
                    End If
                End If

                If strMedical = "0" Then
                    strMedical = String.Empty
                End If
                If strTelephone = "0" Then
                    strTelephone = String.Empty
                End If
                If strLTA = "0" Then
                    strLTA = String.Empty
                End If

                If Not String.IsNullOrEmpty(strMedical) Or Not String.IsNullOrEmpty(strTelephone) Or Not String.IsNullOrEmpty(strLTA) Then
                    For intI As Integer = 1 To 3
                        If objRMB.SaveInto_Emp_Reimbursement_Detail(intI, Emp_Id, arrYearly(intI - 1), arrMonthly(intI - 1), strFinancialYear, strBillYear, intJoinMonth) = True Then

                            If Now.Month >= 4 Then
                                objRMB.InsertDefaultValues(4, 12, Date.Now.Year, 1, ViewState("FinancialYear"), EmpId) ' Used for month wise entries 
                                objRMB.InsertDefaultValues(1, 3, Date.Now.Year + 1, 2, ViewState("FinancialYear"), EmpId)
                            Else
                                objRMB.InsertDefaultValues(1, 3, Date.Now.Year, 1, ViewState("FinancialYear"), EmpId) ' Used for month wise entries 
                            End If
                            lstError.Items.Clear()
                            lstError.Items.Add("Data Saved Sucessfully...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        End If
                    Next
                End If
                arrYearly.Clear()
                arrMonthly.Clear()
            Loop
            

        Catch ex As Exception
            CreateLog("Reimbursement_ReimbursementYearly", "SaveRBMAmt-177", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
        FillRBMYearlyGrid()
    End Sub
#End Region

#End Region

#Region "Button Event"
    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Call DeleteRBMAmt() ' For deleting and geting records on page load
        Call SaveRBMAmt() 'For saving 
    End Sub
#End Region
End Class
