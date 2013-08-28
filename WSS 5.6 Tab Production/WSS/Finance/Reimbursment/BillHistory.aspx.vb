﻿Imports ION.Logging.EventLogging
Imports System.Data
Imports System.IO
Imports Telerik.Web.UI
Imports WSSBLL

#Region "Sessionused"
'Session("PropUserName")
'Session("PropUserID")

#End Region
Partial Class Finance_Reimbursment_BillHistory
    Inherits System.Web.UI.Page
    Private objRMB As New ReimburstmentBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)
    Public Sub FillEmpBillSubmittedGrid()
        Try
            Dim dtBindGrid As New DataTable
            ViewState("FinancialYear") = objRMB.GetFinacialYear()
            If IsNothing(Session("PropUserID")) = True Then
                LogoutWSS()
                Exit Sub
            End If
            If Val(Session("PropUserID")) = 0 Then
                LogoutWSS()
                Exit Sub
            End If

            If rbtnApproved.Checked = True Then
                dtBindGrid = objRMB.GetEmpBillSubmitted(Session("PropUserID"), 0, ViewState("FinancialYear"), 1)
            ElseIf rbtnNotApproved.Checked = True Then
                dtBindGrid = objRMB.GetEmpBillSubmitted(Session("PropUserID"), 0, ViewState("FinancialYear"), 0)
            Else
                dtBindGrid = objRMB.GetEmpDisapprovedBill(Session("PropUserID"), 0, ViewState("FinancialYear"), 1)
            End If
            rgvBillSubmitted.DataSource = dtBindGrid
            rgvBillSubmitted.DataBind()
            'This code is used to hide the columns in grid according to the checkbox checked
            If rbtnDisapproved.Checked = True Then
                rgvBillSubmitted.Columns(5).Visible = True
                rgvBillSubmitted.Columns(6).Visible = False
                rgvBillSubmitted.Columns(8).Visible = False
            ElseIf rbtnNotApproved.Checked = True Then
                rgvBillSubmitted.Columns(5).Visible = False
                rgvBillSubmitted.Columns(6).Visible = True
                rgvBillSubmitted.Columns(8).Visible = True
            Else
                rgvBillSubmitted.Columns(5).Visible = False
                rgvBillSubmitted.Columns(6).Visible = False
                rgvBillSubmitted.Columns(8).Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "rbtnApproved_CheckedChanged"
    Protected Sub rbtnApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnApproved.CheckedChanged
        Call FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "rbtnNotApproved_CheckedChanged"
    Protected Sub rbtnDisapprovedCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnDisapproved.CheckedChanged
        Call FillEmpBillSubmittedGrid()
    End Sub
#End Region

#Region "rbtnNotApproved_CheckedChanged"
    Protected Sub rbtnNotApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnNotApproved.CheckedChanged
        Call FillEmpBillSubmittedGrid()
    End Sub
#End Region
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        FillEmpBillSubmittedGrid()
    End Sub

    Protected Sub rgvBillSubmitted_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgvBillSubmitted.ItemCommand
        Try
            If e.CommandName = "Delete" Then
                Dim BillID As Integer
                BillID = Val(e.CommandArgument.ToString)
                ''''''following code added by Tarun to delete files from Solution explorer
                Dim dsNew As New DataSet
                Dim strOldFileName As String = String.Empty
                Dim strOldFilePath As String = String.Empty
                dsNew = objRMB.SearchExistingBills(BillID)
                If dsNew.Tables.Count > 0 Then
                    If dsNew.Tables(0).Rows.Count > 0 Then
                        strOldFileName = dsNew.Tables(0).Rows(0)("BillFileName").ToString
                        strOldFilePath = dsNew.Tables(0).Rows(0)("BillFilePath").ToString
                    End If
                End If
                Dim strPath As String = Server.MapPath("../RBMBills/" & Session("PropUserName") & "-" & Now.Month)
                strPath = strPath + "\" + strOldFileName
                Dim FO As FileInfo = New FileInfo(strPath)
                If FO.Exists Then
                    FO.Delete()
                End If
                If objRMB.DeleteBill(BillID) = True Then
                    FillEmpBillSubmittedGrid()
                End If
            Else
                Dim strFileName As String = e.CommandName
                Dim strFilePath As String = Server.MapPath(e.CommandArgument.ToString)

                Dim strmFile As Stream = File.OpenRead(strFilePath)
                Dim buffer(strmFile.Length) As Byte
                strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
                Response.ClearHeaders()
                Response.ClearContent()
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
                Response.BinaryWrite(buffer)
                Response.End()
            End If

        Catch ex As Exception
            CreateLog("ViewAttachments", "ItemCommand-214", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try
    End Sub
    Protected Sub rgvBillSubmitted_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgvBillSubmitted.ItemDataBound
        If e.Item.ItemType = Telerik.Web.UI.GridItemType.Item Or e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem Then
            If CType(CType(e.Item.DataItem, DataRowView)("Verified"), Boolean) = True Then
                CType(e.Item.FindControl("LinkButton_Delete"), LinkButton).Visible = False
                e.Item.Cells.RemoveAt(e.Item.Cells.Count - 1)
            End If
        End If
    End Sub
    Protected Sub abc(ByVal sender As Object, ByVal e As EventArgs)
        Dim strBillAmt As String = String.Empty
        Dim strFileUploadName As String = String.Empty
        'Dim strFileUploadPath As String = String.Empty
        Dim strAttachLocation As String = String.Empty
        Dim fn As String
        Dim File1 As RadUpload
        Dim strID As Integer
        Try
            strBillAmt = CType(CType(sender, Button).NamingContainer.FindControl("description"), TextBox).Text
            'strFileUploadName = CType(CType(sender, Button).NamingContainer.FindControl("fupolicyfile"), FileUpload).PostedFile.FileName
            File1 = CType(CType(sender, Button).NamingContainer.FindControl("fupolicyfile1"), RadUpload)
            If File1.UploadedFiles.Count > 0 Then
                If File1.UploadedFiles(0).ContentLength > 0 Then
                    fn = System.IO.Path.GetFileName(File1.UploadedFiles(0).FileName)
                    Dim strPath As String = Server.MapPath("../RBMBills/" & Session("PropUserName") & "-" & Now.Month)
                    Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\")
                    If objFolder.Exists = False Then
                        ' Then Create the folder
                        objFolder.Create()
                    End If
                    Dim SaveLocation As String = Server.MapPath("../RBMBills/" & Session("PropUserName") & "-" & Now.Month & "\" & fn)
                    strAttachLocation = ("../RBMBills/" & Session("PropUserName") & "-" & Now.Month & "\" & fn)
                    File1.UploadedFiles(0).SaveAs(SaveLocation)
                End If
            End If
            If String.IsNullOrEmpty(fn) Then
                strID = CInt(Request.Form("ValueBillID"))
                '''''Update command
                objRMB.UpdateBill(strID, strBillAmt, fn, strAttachLocation)
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "myScript", "<script>javascript:Close();</script>", False)
                rgvBillSubmitted.Rebind()
            Else
                ''''delete command 
                strID = CInt(Request.Form("ValueBillID"))
                Dim dsNew As New DataSet
                Dim strOldFileName As String = String.Empty
                Dim strOldFilePath As String = String.Empty
                dsNew = objRMB.SearchExistingBills(strID)
                If dsNew.Tables.Count > 0 Then
                    If dsNew.Tables(0).Rows.Count > 0 Then
                        strOldFileName = dsNew.Tables(0).Rows(0)("BillFileName").ToString
                        strOldFilePath = dsNew.Tables(0).Rows(0)("BillFilePath").ToString
                    End If
                End If
                Dim strPath As String = Server.MapPath("../RBMBills/" & Session("PropUserName") & "-" & Now.Month)
                strPath = strPath + "\" + strOldFileName
                Dim FO As FileInfo = New FileInfo(strPath)
                If FO.Exists Then
                    FO.Delete()
                End If
                objRMB.UpdateBill(strID, strBillAmt, fn, strAttachLocation)
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "myScript", "<script>javascript:Close();</script>", False)
                rgvBillSubmitted.Rebind()
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
