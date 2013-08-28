#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [22/06/09]
' PURPOSE   : [This Screen is used to save the Bill Amount and to view the Bill detail Yearly and Monthly]
' TABLES    : [Emp_Reimbursement_BillDetail,Emp_Reimbursement_Detail]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports ION.Logging.EventLogging
Imports System.Data
Imports Telerik.Web.UI
Imports WSSBLL
Imports System.IO
#End Region
Partial Class Finance_Reimbursment_ReimbursementBillSubmission
    Inherits System.Web.UI.Page

#Region "Variables"
    Private objRMB As New ReimburstmentBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private txthiddenImage As String 'stored clicked button's cation  
#End Region

#Region "PageEvent"
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

        If Request.QueryString("ScreenFrom") = "HomePage" Then
            imgClose.Visible = False
        End If
        txthiddenImage = Request.Form("txthiddenImage")
        If IsPostBack = False Then
            Session("update") = Server.UrlEncode(System.DateTime.Now.ToString())
            Dim dtBillSub As New DataTable
            'Get the BillSubmissionDetail
            dtBillSub = objRMB.GetBillSubDateDetail
            If dtBillSub.Rows.Count > 0 Then
                For inti As Integer = 0 To dtBillSub.Rows.Count - 1
                    ViewState("BillStartDate") = Val(dtBillSub.Rows(inti)("StartDate").ToString)
                    ViewState("BillEndDate") = Val(dtBillSub.Rows(inti)("EndDate").ToString)
                    ViewState("BillFinacialYear") = dtBillSub.Rows(inti)("FinacialYear").ToString.Trim
                Next
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Financial Year set please contact Finance Dept....!!")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

            FillReimbursementType()
            FillEmpYeralyDetail()
            FillEmpMonthlyDetail()
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            Catch ex As Exception
                CreateLog("Finance_Reimbursment_ReimbursementBillSubmission", "Page_Load-54", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If
    End Sub
#End Region

#Region "Page_PreRender"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ViewState("update") = Session("update")
    End Sub
#End Region
#End Region

#Region "Functions"
#Region "FillReimbursementType"
    ''' <summary>
    ''' Function to Fill Reimbursement Type Combo based upon user name
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillReimbursementType()
        Try
            Dim dtRBM As New Data.DataTable
            Dim dr As DataRow
            dtRBM = objRMB.GetReimburstmentType(Session("PropUserID"), ViewState("BillFinacialYear"))
            ddlRBMType.DataSource = dtRBM
            dr = dtRBM.NewRow
            dr(0) = "Please select"
            dr(1) = 0
            dtRBM.Rows.InsertAt(dr, 0)
            dtRBM.AcceptChanges()
            ddlRBMType.DataTextField = "RBMType"
            ddlRBMType.DataValueField = "RBM_ID"
            ddlRBMType.DataBind()
            If dtRBM.Rows.Count <= 1 Then
                lstError.Items.Clear()
                lstError.Items.Add("Reimbursement not set for you please contact Finance Dept....!!")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("Finance_Reimbursment_ReimbursementBillSubmission", "FillReimbursementType-88", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillEmpYeralyDetail"
    ''' <summary>
    ''' This function is used to get the employee Records Yearly
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillEmpYeralyDetail()
        Try
            Dim dtEmpDeatil As New DataTable
            dtEmpDeatil = objRMB.GetEmpYearlyBillDetail(Session("PropUserID"), Now.Month, Now.Year, ViewState("BillFinacialYear"))
            rgEmpYearlyDetail.DataSource = dtEmpDeatil
            rgEmpYearlyDetail.DataBind()
        Catch ex As Exception
            CreateLog("Finance_Reimbursment_ReimbursementBillSubmission", "FillEmpYeralyDetail-105", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillEmpMonthlyDetail"
    ''' <summary>
    ''' This function is used to get the employee Records Monthly
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillEmpMonthlyDetail()
        Try
            Dim dtEmpMonthDeatil As New DataTable
            dtEmpMonthDeatil = objRMB.GetEmpMonthlyBillDetail(Session("PropUserID"), Now.Month, Now.Year, ViewState("BillFinacialYear"))
            rgEmpMonthlyDetail.DataSource = dtEmpMonthDeatil
            rgEmpMonthlyDetail.DataBind()

        Catch ex As Exception
            CreateLog("Finance_Reimbursment_ReimbursementBillSubmission", "FillEmpMonthlyDetail-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "ValidationCheck"
    ''' <summary>
    ''' This Function is used to validate the enter data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidationCheck() As Boolean
        Try
            'Dim intSubmittedAmt As Integer
            'Dim RBM_AmtAllowed As Integer
            lstError.Items.Clear()
            Dim shtFlag As Short = 0
            If Val(rntxtAmount.Text) < 0 Then
                lstError.Items.Add("Please Enter Correct amount  ...!")
                shtFlag = 1
            End If

            If ddlRBMType.SelectedValue = 0 Then
                lstError.Items.Add("Please Select Reimbursement Type ...")
                shtFlag = 1
            End If

            If rntxtAmount.Text.Equals("") = True Then
                lstError.Items.Add("Bill amount cannot be blank ...! ")
                shtFlag = 1
            End If

            If File1.Value.Equals("") = True Then
                lstError.Items.Add("Please Attach Bill ...! ")
                shtFlag = 1
            End If

            'Check whether Datatable have values
            If ViewState("BillFinacialYear").Equals("") = True Or ViewState("BillStartDate") = 0 Or ViewState("BillEndDate") = 0 Then
                lstError.Items.Add("There is No Finacial year set Please contact Finance Dept. ...! ")
                shtFlag = 1
            End If

            If Now.Day >= ViewState("BillStartDate") And Now.Day <= ViewState("BillEndDate") Then
            Else
                lstError.Items.Add("Bill Submission Date Between " & ViewState("BillStartDate") & "to" & ViewState("BillEndDate"))
                shtFlag = 1
                ddlRBMType.SelectedIndex = 0
                rntxtAmount.Text = ""
            End If

            'If ddlRBMType.SelectedValue > 0 Then
            '    'Getting sum of bill which is being submitted for particular Reimbursement Type
            '    intSubmittedAmt = objRMB.GetInsertedBill(Session("PropUserID"), ViewState("BillFinacialYear"), ddlRBMType.SelectedValue)
            '    'Getting Reimbursement Amount which is being allowed
            '    RBM_AmtAllowed = objRMB.GetAmtRBMAllowed(Session("PropUserID"), ddlRBMType.SelectedValue)
            '    If intSubmittedAmt + rntxtAmount.Text > RBM_AmtAllowed Then
            '        lstError.Items.Add("Reimburstment Amount cannot be greater than reimburstment allowed...")
            '        shtFlag = 1
            '    End If
            'End If

            If shtFlag = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

        End Try
    End Function
#End Region

#End Region

#Region "Button Event"
#Region "imgSave_Click"
    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Dim arrvalues As New ArrayList
        Dim fn As String = String.Empty
        Dim strAttachLocation As String = String.Empty
        Try
            If Session("update").ToString() = ViewState("update").ToString() Then
                If ValidationCheck() = False Then
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
                If Not File1.PostedFile Is Nothing And File1.PostedFile.ContentLength > 0 Then
                    fn = System.IO.Path.GetFileName(File1.PostedFile.FileName)
                    '            Dim Ext = upload.Accept
                    Dim strPath As String = Server.MapPath("../RBMBills/" & Session("PropUserName") & "-" & Now.Month)
                    Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\")
                    If objFolder.Exists = False Then
                        ' Then Create the folder
                        objFolder.Create()
                    End If
                    Dim SaveLocation As String = Server.MapPath("../RBMBills/" & Session("PropUserName") & "-" & Now.Month & "\" & fn)
                    strAttachLocation = ("../RBMBills/" & Session("PropUserName") & "-" & Now.Month & "\" & fn)
                    File1.PostedFile.SaveAs(SaveLocation)
                End If
                arrvalues.Clear()
                'Add Values in Array List
                arrvalues.Add(ddlRBMType.SelectedValue)
                arrvalues.Add(rntxtAmount.Text.Trim)
                arrvalues.Add(Now.Month)
                arrvalues.Add(Now.Year)
                arrvalues.Add(Now.ToShortDateString)
                arrvalues.Add(Session("PropUserID")) 'ID
                arrvalues.Add(ViewState("BillFinacialYear"))
                arrvalues.Add(strAttachLocation)
                arrvalues.Add(fn)
                If objRMB.sav_rec(arrvalues) = True Then
                    Session("update") = Server.UrlEncode(System.DateTime.Now.ToString())
                    lstError.Items.Clear()
                    lstError.Items.Add("Bill Sent for verification...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                End If
                ddlRBMType.SelectedIndex = 0
                rntxtAmount.Text = ""
            End If
        Catch ex As Exception
            CreateLog("Reimbursement_Reimbursement", "Finance_Reimbursment_ReimbursementBillSubmission-227", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "imgReset_Click"
    Protected Sub imgReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReset.Click
        ddlRBMType.SelectedIndex = 0
        rntxtAmount.Text = ""
    End Sub
#End Region
#End Region

#Region "Grid Event"
    Protected Sub rgEmpMonthlyDetail_GroupsChanging(ByVal source As Object, ByVal e As Telerik.Web.UI.GridGroupsChangingEventArgs) Handles rgEmpMonthlyDetail.GroupsChanging
        FillEmpMonthlyDetail()
    End Sub
#End Region

    Protected Sub rgEmpYearlyDetail_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgEmpYearlyDetail.ItemDataBound
        Try
            If e.Item.ItemType = Telerik.Web.UI.GridItemType.Item Or e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem Then
                Dim dr As DataRowView = DirectCast(e.Item.DataItem, DataRowView)
                Dim BIllPending As Integer
                BIllPending = Val(dr("Bill Pending for a full year").ToString)
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                If BIllPending >= 0 Then
                    item("BillPending").ForeColor = Drawing.Color.Blue
                    item("BillPending").Font.Bold = True
                Else
                    item("BillPending").ForeColor = Drawing.Color.Red
                    item("BillPending").Font.Bold = True
                End If
            End If
        Catch ex As Exception
            CreateLog("Reimbursement_Reimbursement", "rgEmpYearlyDetail_ItemDataBound-276", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Protected Sub rgEmpMonthlyDetail_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgEmpMonthlyDetail.ItemDataBound
        Try
            If e.Item.ItemType = Telerik.Web.UI.GridItemType.Item Or e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem Then
                Dim dr As DataRowView = DirectCast(e.Item.DataItem, DataRowView)
                Dim BIllAdvance As Integer
                Dim BillDue As Integer
                BIllAdvance = Val(dr("Bill_InAdvance").ToString)
                BillDue = Val(dr("Bill_Due").ToString)
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                If BIllAdvance > 0 Then
                    item("Bill_InAdvance").ForeColor = Drawing.Color.Blue
                    item("Bill_InAdvance").Font.Bold = True
                End If
                If BillDue > 0 Then
                    item("Bill_Due").ForeColor = Drawing.Color.Red
                    item("Bill_Due").Font.Bold = True
                End If
            End If
        Catch ex As Exception
            CreateLog("Reimbursement_Reimbursement", "rgEmpMonthlyDetail_ItemDataBound-299", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
End Class
