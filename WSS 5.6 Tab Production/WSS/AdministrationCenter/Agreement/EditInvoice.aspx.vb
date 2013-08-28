'************************************************************************************************************
' Page                   : - Edit Invoice
' Purpose              : -Display all pending invoice 
' Date					   Author						Modification Date				Description
' 23/05/06			   Sachin Prashar		           ------------------				Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

Partial Class AdministrationCenter_Agreement_EditInvoice
    Inherits System.Web.UI.Page

    Dim AmtBalance As Double
    Dim invoiceid, compid As String
    Dim TotReceived As Double
    Protected Shared mdvtable As DataView = New DataView
    Dim rowvalue As Integer


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        lstError.Items.Clear()
        cpnlErrorPanel.Visible = False

        If Not IsPostBack Then
            dtreceivedate.Text = SetDateFormat(Now, mdlMain.IsTime.WithTime)
        End If
        txtCSS(Me.Page)
        If Not IsPostBack Then

            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")

            txttaxes.Attributes.Add("onchange", "calculatebalance();")
            txtConvRate.Attributes.Add("onchange", "calculatebalance();")
            txtamtreceivednow.Attributes.Add("onchange", "calculatebalance();")
        End If
        invoiceid = Request.QueryString("CodeID")
        Session("InvoiceID") = Request.QueryString("CodeID")
        compid = Request.QueryString("comp")
        Session("InvoiceCompID") = Request.QueryString("comp")

        If Not IsPostBack Then
            Try
                lblCur.Text = SQL.Search("", "", "select top 1 AG_VC8_Currency from T080011 where AG_VC8_Cust_Name=" & compid)
            Catch ex As Exception
                CreateLog("EditInvoice", "save invoice-217", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        GetAgreementAmt(invoiceid, compid)
        GetTotalReceived()
        txtBalanceAvailable.Text = Val(txtAgreementAmt.Text) - Val(txtbalance.Text)

        If Val(txtAgreementAmt.Text) < Val(txtbalance.Text) Or Val(txtAgreementAmt.Text) = Val(txtbalance.Text) Then
            SQL.Update("EditInvoice", "load-124", "update T080031 set IM_VC8_Invoice_Status='CLEARED' where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid & "", SQL.Transaction.Serializable)
        Else
            SQL.Update("EditInvoice", "load-124", "update T080031 set IM_VC8_Invoice_Status='PENDING' where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid & "", SQL.Transaction.Serializable)
        End If

        FillInvoiceDetail()

        '*************************************************************************
        'Security Block
        Dim intid As String
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = 708
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If
        'End of Security Block

    End Sub
    Function GetAgreementAmt(ByVal invoiceid As Integer, ByVal compid As String) As Boolean
        Dim dsagrrementamt As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        ' SQL.DBTable = "T080031"
        SQL.DBTracing = False
        Try
            Dim strCheck As String = SQL.Search("T080031", "EditInvoice", "GetAgreementAmt", "select IM_NU9_Invoice_discount_Amt from T080031 where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid, dsagrrementamt, "sachin", "Prashar")
            If Not IsDBNull(dsagrrementamt.Tables("T080031").Rows(0).Item(0)) Then
                If dsagrrementamt.Tables("T080031").Rows(0).Item(0) Then
                    AmtBalance = dsagrrementamt.Tables("T080031").Rows(0).Item(0)
                    txtAgreementAmt.Text = AmtBalance
                End If
            End If
        Catch ex As Exception
            CreateLog("EditInvoice", "GetAggrementAmt-169", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

        Try

            'display error if view name blank
            '***********************
            If txtamtreceivednow.Text.Equals("") Or txtamtreceivednow.Text.Equals("0") Then
                lstError.Items.Add("Received Amount cannot be blank or zero...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            '********************************************************
            If txtActuRecev.Text.Equals("") Or txtActuRecev.Text.Equals("0") Then
                lstError.Items.Add("Actual Amount cannot be blank or zero...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

            If Val(txtBalanceAvailable.Text.Trim) < Val(txtamtreceivednow.Text.Trim) Then
                lstError.Items.Add("Received Amount cannot be greater than Balance Amount...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If


            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("IP_NU9_Invoice_ID_FK")
            arColumnName.Add("IP_NU9_Company_ID_FK")

            arColumnName.Add("IP_DT8_Invoice_Amt_Rec_Date")
            arColumnName.Add("IP_NU9_Invoice_Amt_Rec")
            arColumnName.Add("IP_NU9_Invoice_Exchange_Rate")
            arColumnName.Add("IP_NU9_Invoice_Bank_charges")
            arColumnName.Add("IP_NU_Invoice_Amt_rec_Actual")
            arColumnName.Add("IP_VC50_Bank_Name")
            arColumnName.Add("IP_VC50_Draft_No")

            arRowData.Add(invoiceid)
            arRowData.Add(compid)
            arRowData.Add(dtreceivedate.Text)
            arRowData.Add(txtamtreceivednow.Text)
            arRowData.Add(txtConvRate.Text)
            If txttaxes.Text.Trim = "" Then
                arRowData.Add(0)
            Else
                arRowData.Add(txttaxes.Text)
            End If
            arRowData.Add(txtActuRecev.Text)
            arRowData.Add(txtBankName.Text)
            arRowData.Add(txtDraftNo.Text)

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' save the view name
            SQL.DBConnection = strConnection
            ' SQL.DBTable = "T080033"
            SQL.DBTracing = False

            If SQL.Save("T080033", "imgsave", "click-243", arColumnName, arRowData) = True Then
                lstError.Items.Add("Records Saved successfully...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                clearText()
                FillInvoiceDetail()
                GetTotalReceived()
                txtBalanceAvailable.Text = Val(txtAgreementAmt.Text) - Val(txtbalance.Text)
                If Val(txtAgreementAmt.Text) < Val(txtbalance.Text) Or Val(txtAgreementAmt.Text) = Val(txtbalance.Text) Then
                    SQL.Update("imgSave", "click-263", "update T080031 set IM_VC8_Invoice_Status='CLEARED' where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid & "", SQL.Transaction.Serializable)
                Else
                    SQL.Update("imgSave", "click-265", "update T080031 set IM_VC8_Invoice_Status='PENDING' where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid & "", SQL.Transaction.Serializable)
                End If

            End If
        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            CreateLog("EditInvoice", "imgSave_click-217", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Sub clearText()
        txtActuRecev.Text = ""
        txtamtreceivednow.Text = ""
        txttaxes.Text = ""
        txtBankName.Text = ""
        txtConvRate.Text = ""
        txtDraftNo.Text = ""
    End Sub

    Function FillInvoiceDetail()

        Dim dsagrrementamt As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        '  SQL.DBTable = "T080033"
        SQL.DBTracing = False
        Try
            Dim strCheck As String = SQL.Search("T080033", "EditInvoice", "FillInvoiceDetail", " select IP_NU9_Invoice_ID_FK,IP_NU9_Company_ID_FK,IP_NU9_Invoice_Exchange_Rate,IP_NU9_Invoice_Bank_charges,IP_NU9_Invoice_Amt_Rec, convert(varchar,IP_DT8_Invoice_Amt_Rec_Date,100) IP_DT8_Invoice_Amt_Rec_Date, IP_NU9_Invoice_Pricing_ID_PK,IP_NU_Invoice_Amt_rec_Actual,IP_VC50_Bank_Name,IP_VC50_Draft_No   from T080033 where IP_NU9_Invoice_ID_FK=" & invoiceid & " and IP_NU9_Company_ID_FK=" & compid, dsagrrementamt, "sachin", "Prashar")

            mdvtable.Table = dsagrrementamt.Tables(0)

            Dim htDateCols As New Hashtable
            htDateCols.Add("IP_DT8_Invoice_Amt_Rec_Date", 1)
            SetDataTableDateFormat(mdvtable.Table, htDateCols)


            grdInvoiceDetail.DataSource = mdvtable.Table
            grdInvoiceDetail.DataBind()

        Catch ex As Exception
            CreateLog("EditInvoice", "FillInvoiceDetail-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            Return False
        End Try
    End Function
    Function GetTotalReceived()
        Dim dsgettotreceived As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T080033"
        SQL.DBTracing = False
        Try
            Dim strCheck As String = SQL.Search("T080033", "EditInvoice", "GetTotalReceived", "select  sum(IP_NU9_Invoice_Amt_Rec) from T080033 where IP_NU9_Invoice_ID_FK=" & invoiceid & " and IP_NU9_Company_ID_FK=" & compid, dsgettotreceived, "sachin", "Prashar")

            If Not IsDBNull(dsgettotreceived.Tables("T080033").Rows(0).Item(0)) Then TotReceived = dsgettotreceived.Tables("T080033").Rows(0).Item(0)
            txtbalance.Text = TotReceived

        Catch ex As Exception
            CreateLog("EditInvoice", "GetTotalReceived-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            Return False
        End Try
    End Function

    Private Sub grdInvoiceDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdInvoiceDetail.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = grdInvoiceDetail.DataKeys(e.Item.ItemIndex)
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("ViewJobs", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub
End Class
