#Region "Purpose"
' CREATED BY: [Saurabh]
' CREATED ON: [30/06/09]
' PURPOSE   : [This Screen is used to save dates for running Financial Year]
' TABLES    : [BillSubmissionDate]
#End Region

#Region "NameSpace"
Imports ION.Logging.EventLogging
Imports System.Configuration
Imports System.Data
Imports WSSBLL
#End Region

Partial Class Reimbursement_BillSubmissionDate
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
        If IsPostBack = False Then
            ddlStartdate.SelectedIndex = 0
            ddlEndDate.SelectedIndex = 0
            Dim strFinancialYear As String
            strFinancialYear = Date.Now.Year & "-" & Date.Now.Year + 1
            txtFinancialYear.Text = strFinancialYear
            GetBillSubDateDetai()
            For intI As Integer = 1 To 31
                ddlStartdate.Items.Add(intI)
                ddlEndDate.Items.Add(intI)
            Next
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            Catch ex As Exception
                CreateLog("Reimbursement_BillSubmissionDate", "Page_Load-46", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Button Event"
#Region "Button Click to Save Dates"
    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Dim intSubmissionID As Integer
        Try
            If ddlStartdate.Text <> "" And ddlEndDate.Text <> "" Then
                If Val(ddlStartdate.Text.Trim) >= Val(ddlEndDate.Text.Trim) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Start cannot be equal or more than End Date...!")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
                intSubmissionID = "1"
                DeleteBillSubmissionDate()
                If objRMB.InsertBillSubmissionDate(intSubmissionID, ddlStartdate.Text.ToString.Trim, ddlEndDate.Text.ToString.Trim, txtFinancialYear.Text.ToString.Trim) = True Then

                    GetBillSubDateDetai()
                    lstError.Items.Clear()
                    lstError.Items.Add("Dates Saved Sucessfully...!")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please enter Start and End Dates...!")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("Reimbursement_BillSubmissionDate", "imgSave_Click-79", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "imgReset_Click"
    Protected Sub imgReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReset.Click
        ddlStartdate.SelectedIndex = 0
        ddlEndDate.SelectedIndex = 0
    End Sub
#End Region


#End Region

#Region "Function"
#Region "Function used to Delete Dates"
    ''' <summary>
    ''' Function used to Delete existing dates
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteBillSubmissionDate()
        Dim intSubmission_ID As Integer
        intSubmission_ID = "1"
        objRMB.DeleteBillSubmissionDate(intSubmission_ID)
    End Sub

#Region "GetBillSubDateDetai"
    ''' <summary>
    ''' This Function is used To get the set Dates
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetBillSubDateDetai()
        Dim dtBillDateDetail As New DataTable
        dtBillDateDetail = objRMB.GetBillSubDateDetail
        rgvBillSubmittedDate.DataSource = dtBillDateDetail
        rgvBillSubmittedDate.DataBind()
    End Sub
#End Region

#End Region
#End Region

End Class
