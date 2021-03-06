#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [22/06/09]
' PURPOSE   : [This Screen is used By Finance Dept to approved the will.User can see                  both approved and not approved Bills 
' TABLES    : [Bill_History]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "Name Space"
Imports WSSBLL
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.IO
Imports Telerik.Web.UI
#End Region
Partial Class Reimbursement_Default
    Inherits System.Web.UI.Page

#Region "Variables"
    Private objRMB As New ReimburstmentBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Private dtBindGrid As New Data.DataTable 'To hold the Grid Data
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
        imgOk.Visible = False
        '###########################
        txthiddenImage = Request.Form("txthiddenImage")
        If IsPostBack = False Then
            'To Avoid save Funcyinality on Refresh
            Session("update") = Server.UrlEncode(System.DateTime.Now.ToString())
            'Get the Value of Financial Year
            ViewState("FinancialYear") = objRMB.GetFinacialYear()
            rbtnApproved.Checked = True
            If IsNothing(ViewState("FinancialYear")) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("No Financial Year set please contact Finance Dept....!!")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            Call FillEmployeeName()
            FillEmpBillSubmittedGrid()
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            Catch ex As Exception
                CreateLog("BillVerification", "Page_Load-80", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If

    End Sub
#End Region

#Region "Page_PreRender"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'To Avoid save Funcyinality on Refresh
        ViewState("update") = Session("update")
    End Sub
#End Region

#End Region

#Region "Functions"

#Region "FillEmployeeName"
    ''' <summary>
    ''' This Function is used to Fill the Employee dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillEmployeeName()
        Try
            Dim dtEmpName As New Data.DataTable
            dtEmpName = objRMB.GetEmployeesName
            ddlNameOfEmp.DataSource = dtEmpName
            ddlNameOfEmp.DataTextField = "CI_VC36_Name"
            ddlNameOfEmp.DataValueField = "Emp_ID"
            ddlNameOfEmp.DataBind()
        Catch ex As Exception
            CreateLog("BillVerification", "FillEmployeeName-81", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "FillEmpBillSubmittedGrid"
    ''' <summary>
    ''' This Function is used to Fill the Grid according to Checked Radio button
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FillEmpBillSubmittedGrid()
        Try
            'check which radio button checked
            If rbtnApproved.Checked = True Then
                dtBindGrid = objRMB.GetEmpBillSubmitted(ddlNameOfEmp.SelectedValue, ddlMonth.SelectedValue, ViewState("FinancialYear"), 1)
            ElseIf rbtnNotApproved.Checked = True Then
                dtBindGrid = objRMB.GetEmpBillSubmitted(ddlNameOfEmp.SelectedValue, ddlMonth.SelectedValue, ViewState("FinancialYear"), 0)
            Else
                dtBindGrid = objRMB.GetEmpDisapprovedBill(ddlNameOfEmp.SelectedValue, ddlMonth.SelectedValue, ViewState("FinancialYear"), 1)
            End If
            rgvBillSubmitted.DataSource = dtBindGrid
            rgvBillSubmitted.DataBind()

            'make the column visbile according to slected Radio button
            If rbtnApproved.Checked = True Then
                rgvBillSubmitted.Columns(13).Visible = False
                rgvBillSubmitted.Columns(15).Visible = False
            ElseIf rbtnNotApproved.Checked = True Then
                rgvBillSubmitted.Columns(13).Visible = True
                rgvBillSubmitted.Columns(15).Visible = False
            Else
                rgvBillSubmitted.Columns(13).Visible = False
                rgvBillSubmitted.Columns(15).Visible = True
            End If
        Catch ex As Exception
            CreateLog("BillVerification", "FillEmpBillSubmittedGrid-101", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#End Region

#Region "Button Event"
#Region "imgSave_Click"
    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Try
            Dim arrValue As New ArrayList 'To hold the Values to save Records
            If Session("update").ToString() = ViewState("update").ToString() Then
                For Each Griditem As GridDataItem In rgvBillSubmitted.SelectedItems
                    arrValue.Clear()
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("BillSubmitted").Text) 'Amount
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("EMPID").Text) 'Empid
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("RBMID").Text) 'RBMID
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("MonthID").Text) 'MonthID
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("Year").Text) 'Year
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("ID").Text) 'ID
                    arrValue.Add(Now.ToShortDateString) 'Date
                    arrValue.Add(ViewState("FinancialYear")) 'FinancialYear
                    arrValue.Add(Now.Month) 'Current month
                    'Call Save Function
                    If objRMB.SaveBillVerified(arrValue) = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Bill Approved Sucessfully ,Please Adjust the bills...!")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        arrValue.Clear()
                    End If
                Next
                Session("update") = Server.UrlEncode(System.DateTime.Now.ToString())
                If (Now.Month.Equals(3) = False) Then
                    imgOk_Click(Nothing, Nothing)
                End If

            End If
            FillEmpBillSubmittedGrid()
        Catch ex As Exception
            CreateLog("BillVerification", "imgSave_Click-136", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "rbtnApproved_CheckedChanged"
    Protected Sub rbtnApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnApproved.CheckedChanged
        Call FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "rbtnDisApproved_CheckedChanged"
    Protected Sub rbtDisapproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDisapproved.CheckedChanged
        Call FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "rbtnNotApproved_CheckedChanged"
    Protected Sub rbtnNotApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnNotApproved.CheckedChanged
        Call FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "imgOk_Click"
    Protected Sub imgOk_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOk.Click
        Dim arrValues As New ArrayList
        arrValues.Clear()
        arrValues.Add(Now.Month)
        arrValues.Add(Now.ToShortDateString)
        arrValues.Add(ViewState("FinancialYear"))
        If objRMB.AdjustBills(arrValues) = True Then
            lstError.Items.Clear()
            lstError.Items.Add("Bill Adjust Sucessfully for next Month ....! ")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            arrValues.Clear()
        End If
    End Sub
#End Region

#End Region

#Region "Grid Event"

#Region "rgvBillSubmitted_GroupsChanging"
    Protected Sub rgvBillSubmitted_GroupsChanging(ByVal source As Object, ByVal e As Telerik.Web.UI.GridGroupsChangingEventArgs) Handles rgvBillSubmitted.GroupsChanging
        FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "rgvBillSubmitted_ItemDataBound"

    Protected Sub rgvBillSubmitted_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgvBillSubmitted.ItemCommand
        Dim strFileName As String = e.CommandName
        Dim strFilePath As String = Server.MapPath(e.CommandArgument.ToString)
        Try
            Dim strmFile As Stream = File.OpenRead(strFilePath)
            Dim buffer(strmFile.Length) As Byte
            strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
            Response.BinaryWrite(buffer)
            Response.End()
        Catch ex As Exception
            CreateLog("ViewAttachments", "ItemCommand-214", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try
    End Sub
    Protected Sub rgvBillSubmitted_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgvBillSubmitted.ItemDataBound
        Try
            If e.Item.ItemType = Telerik.Web.UI.GridItemType.Item Or e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem Then
                If e.Item.ItemIndex = -1 Then
                Else
                    'Disable the Check box if the Verified field is true
                    CType(e.Item.FindControl("rntxtReason"), TextBox).Visible = False
                    CType(e.Item.FindControl("rnlblReason"), Label).Visible = False
                    rgvBillSubmitted.Columns(13).Visible = False
                    rgvBillSubmitted.Columns(15).Visible = False
                    If CType(CType(e.Item.DataItem, DataRowView)("Verified"), Boolean) = True Then
                        Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                        Dim checkBox As CheckBox = DirectCast(item("ClientSelectColumn").Controls(0), CheckBox)
                        checkBox.Enabled = False
                        'check the status  to hide the columns 
                    ElseIf (CType(e.Item.FindControl("BillStatus"), Label).Text.Trim) = 1 Then
                        CType(e.Item.FindControl("rnlblReason"), Label).Visible = True
                        rgvBillSubmitted.Columns(15).Visible = True
                    Else
                        CType(e.Item.FindControl("rntxtReason"), TextBox).Visible = True
                        rgvBillSubmitted.Columns(13).Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            CreateLog("BillVerification", "rgvBillSubmitted_ItemDataBound-151", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region

#Region "rgvBillSubmitted_PageIndexChanged"

  
    Protected Sub rgvBillSubmitted_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles rgvBillSubmitted.PageIndexChanged
        FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "rgvBillSubmitted_PageSizeChanged"
    Protected Sub rgvBillSubmitted_PageSizeChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles rgvBillSubmitted.PageSizeChanged
        FillEmpBillSubmittedGrid()
    End Sub
#End Region

#End Region

#Region "SelectIndexChanged"
    Protected Sub ddlNameOfEmp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNameOfEmp.SelectedIndexChanged
        FillEmpBillSubmittedGrid()
    End Sub

    Protected Sub ddlMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMonth.SelectedIndexChanged
        FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "ImgDelete_Click"
    ''' <summary>
    ''' This button is used to dissproved  bills and set the statusfor dissproved bills
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ImgDisapproved_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgDisapproved.Click
        Try
            Dim arrValue As New ArrayList 'To hold the Values to save Records
            If Session("update").ToString() = ViewState("update").ToString() Then
                For Each Griditem As GridDataItem In rgvBillSubmitted.SelectedItems
                    arrValue.Clear()
                    arrValue.Add(rgvBillSubmitted.Items(Griditem.ItemIndex).Item("ID").Text) 'ID
                    Dim strReason As String = CType(Griditem.FindControl("rntxtReason"), TextBox).Text
                    arrValue.Add(strReason) 'ID
                    'Call Save Function
                    If objRMB.SetDisapprovedBillsStatus(arrValue) = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Bill Disapproved Sucessfully ...!")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        arrValue.Clear()
                    End If
                Next
                Session("update") = Server.UrlEncode(System.DateTime.Now.ToString())
            End If
            FillEmpBillSubmittedGrid()
        Catch ex As Exception
            CreateLog("BillVerification", "ImgDisapproved_Click-296", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region
End Class
