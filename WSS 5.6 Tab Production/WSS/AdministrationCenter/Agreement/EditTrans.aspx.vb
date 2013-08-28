'************************************************************************************************************
' Page                 : - Edit Transaction
' Purpose              : - Update allready received errors
' Tables used          : - T080033
' Date					   Author						Modification Date				Description
' 24/05/06			   Sachin Prashar		           ------------------			    Created
'
' Notes: 
' Code:
'************************************************************************************************************

Imports System
Imports ION.Data
Imports ION.Logging
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.SessionState
Partial Class AdministrationCenter_Agreement_EditTrans
    Inherits System.Web.UI.Page
    Dim TrasnID As String
    Dim TotReceived As Double
    Dim AmtBalance As Double
    Private Shared amtupdate As Double

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        TrasnID = Request.QueryString("TranID")
        txtCSS(Me.Page)
        lstError.Items.Clear()
        cpnlErrorPanel.Visible = False
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            txttaxes.Attributes.Add("onchange", "calculatebalance();")
            txtConvRate.Attributes.Add("onchange", "calculatebalance();")
            txtamtreceivednow.Attributes.Add("onchange", "calculatebalance();")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        End If
        If Not IsPostBack Then
            GetData()
        End If

    End Sub
    '*******************************************************************
    ' Date					   Author						Modification Date				Description
    ' 22/05/06			   Sachin Prashar		    ------------------					    Created
    'Function Detail    fatch the data from database based on transaction ID
    ' Notes: 
    ' Code:
    '*******************************************************************
    Function GetData()

        Try
            Dim dsdataset As DataSet = New DataSet
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            If SQL.Search("T080033", "AdminEditView", "LoadEntries-192", "select * from T080033 where IP_NU9_Invoice_Pricing_ID_PK=" & TrasnID & "", dsdataset, "sachin", "Prashar") = True Then
                dtreceivedate.CalendarDate = SetDateFormat(dsdataset.Tables(0).Rows(0).Item("IP_DT8_Invoice_Amt_Rec_Date"), mdlMain.IsTime.WithTime)

                txtamtreceivednow.Text = dsdataset.Tables(0).Rows(0).Item("IP_NU9_Invoice_Amt_Rec")
                amtupdate = dsdataset.Tables(0).Rows(0).Item("IP_NU9_Invoice_Amt_Rec") ' store old value

                txtConvRate.Text = dsdataset.Tables(0).Rows(0).Item("IP_NU9_Invoice_Exchange_Rate")
                txttaxes.Text = dsdataset.Tables(0).Rows(0).Item("IP_NU9_Invoice_Bank_charges")
                txtActuRecev.Text = dsdataset.Tables(0).Rows(0).Item("IP_NU_Invoice_Amt_rec_Actual")
                txtBankName.Text = dsdataset.Tables(0).Rows(0).Item("IP_VC50_Bank_Name")
                txtDraftNo.Text = dsdataset.Tables(0).Rows(0).Item("IP_VC50_Draft_No")
            End If
        Catch ex As Exception
            CreateLog("EditTrans", "GetData-119", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Function

    Private Function UpdateTran() As Boolean
        Try

            'display error if view name blank
            '***********************
            If txtamtreceivednow.Text.Equals("") Or txtamtreceivednow.Text.Equals("0") Then
                lstError.Items.Add("Received Amount cannot be blank or zero...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            '******************************
            If txtActuRecev.Text.Equals("") Or txtActuRecev.Text.Equals("0") Then
                lstError.Items.Add("Actual Amount cannot be blank or zero...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If

            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("IP_DT8_Invoice_Amt_Rec_Date")
            arColumnName.Add("IP_NU9_Invoice_Amt_Rec")
            arColumnName.Add("IP_NU9_Invoice_Exchange_Rate")
            arColumnName.Add("IP_NU9_Invoice_Bank_charges")
            arColumnName.Add("IP_NU_Invoice_Amt_rec_Actual")
            arColumnName.Add("IP_VC50_Bank_Name")
            arColumnName.Add("IP_VC50_Draft_No")

            arRowData.Add(dtreceivedate.CalendarDate)
            arRowData.Add(txtamtreceivednow.Text)
            arRowData.Add(IIf(txtConvRate.Text.Trim = "", 1, txtConvRate.Text.Trim))
            arRowData.Add(txttaxes.Text)
            arRowData.Add(txtActuRecev.Text)
            arRowData.Add(txtBankName.Text)
            arRowData.Add(txtDraftNo.Text)


            If SQL.Update("T080033", "EditTrans", "UpdateTran", "select* from T080033 where IP_NU9_Invoice_Pricing_ID_PK=" & TrasnID, arColumnName, arRowData) = True Then

                Dim focusScript As String = "<script language='javascript'>" & _
                                     "self.opener.Form1.submit();</script>"
                ' Add the JavaScript code to the page.
                Page.RegisterStartupScript("FocusScript", focusScript)
                lstError.Items.Add("Records Updated successfully...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True

                'GetTotalReceived()
                'txtBalanceAvailable.Text = Val(txtAgreementAmt.Text) - Val(txtbalance.Text)
                'If Val(txtAgreementAmt.Text) < Val(txtbalance.Text) Or Val(txtAgreementAmt.Text) = Val(txtbalance.Text) Then
                '    SQL.Update("", "", "update T080031 set IM_VC8_Invoice_Status='CLEARED' where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid & "", SQL.Transaction.Serializable)
                'End If

            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("EditTrans", "UpdateTrans-119", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            Return False
        End Try
    End Function
    Function GetTotalReceived()
        Dim dsgettotreceived As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T080033"
        SQL.DBTracing = False
        Try
            Dim strCheck As String = SQL.Search("T080033", "EditInvoice", "GetTotalReceived", "select  sum(IP_NU9_Invoice_Amt_Rec) from T080033 where IP_NU9_Invoice_ID_FK=" & Session("InvoiceID") & " and IP_NU9_Company_ID_FK=" & Session("InvoiceCompID"), dsgettotreceived, "sachin", "Prashar")

            TotReceived = dsgettotreceived.Tables("T080033").Rows(0).Item(0)
            ' txtbalance.Text = TotReceived

        Catch ex As Exception
            CreateLog("EditInvoice", "GetTotalReceived-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            Return False
        End Try
    End Function
    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Try
            Dim balamt As Double

            GetAgreementAmt(Session("InvoiceID"), Session("InvoiceCompID"))
            GetTotalReceived()

            If AmtBalance > (TotReceived - amtupdate) Then
                balamt = AmtBalance - (TotReceived - amtupdate)
                If balamt < txtamtreceivednow.Text.Trim Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Received Amount is greater than your balance amount...")
                    'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Sub
                End If
            End If

            UpdateTran()
            amtupdate = txtamtreceivednow.Text.Trim

            If balamt = AmtBalance - (TotReceived - amtupdate) Then
                SQL.Update("edittrans", "load-202", "update T080031 set IM_VC8_Invoice_Status='CLEARED' where IM_NU9_Invoice_ID_PK=" & Session("InvoiceID") & " and IM_NU9_Company_ID_PK=" & Session("InvoiceCompID") & "", SQL.Transaction.Serializable)
            Else
                SQL.Update("edittrans", "load-222", "update T080031 set IM_VC8_Invoice_Status='PENDING' where IM_NU9_Invoice_ID_PK=" & Session("InvoiceID") & " and IM_NU9_Company_ID_PK=" & Session("InvoiceCompID") & "", SQL.Transaction.Serializable)
            End If



        Catch ex As Exception
        End Try

    End Sub

    Private Sub imgOk_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOk.Click
        Dim balamt As Double

        GetAgreementAmt(Session("InvoiceID"), Session("InvoiceCompID"))
        GetTotalReceived()

        If AmtBalance > (TotReceived - amtupdate) Then
            balamt = AmtBalance - (TotReceived - amtupdate)
            If balamt < txtamtreceivednow.Text.Trim Then
                lstError.Items.Clear()
                lstError.Items.Add("Received Amount is greater than your balance amount...")
                'ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If
        End If

        If UpdateTran() = True Then
            amtupdate = txtamtreceivednow.Text.Trim
            If balamt = AmtBalance - (TotReceived - amtupdate) Then
                SQL.Update("edittrans", "load-202", "update T080031 set IM_VC8_Invoice_Status='CLEARED' where IM_NU9_Invoice_ID_PK=" & Session("InvoiceID") & " and IM_NU9_Company_ID_PK=" & Session("InvoiceCompID") & "", SQL.Transaction.Serializable)
            Else
                SQL.Update("edittrans", "load-222", "update T080031 set IM_VC8_Invoice_Status='PENDING' where IM_NU9_Invoice_ID_PK=" & Session("InvoiceID") & " and IM_NU9_Company_ID_PK=" & Session("InvoiceCompID") & "", SQL.Transaction.Serializable)
            End If
            Response.Write("<script>window.close();</script>")
        End If

    End Sub
    Function GetAgreementAmt(ByVal invoiceid As Integer, ByVal compid As String) As Boolean
        Dim dsagrrementamt As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        ' SQL.DBTable = "T080031"
        SQL.DBTracing = False
        Try
            Dim strCheck As String = SQL.Search("T080031", "EditInvoice", "GetAgreementAmt", "select IM_NU9_Invoice_discount_Amt from T080031 where IM_NU9_Invoice_ID_PK=" & invoiceid & " and IM_NU9_Company_ID_PK=" & compid, dsagrrementamt, "sachin", "Prashar")
            If dsagrrementamt.Tables("T080031").Rows(0).Item(0) Then
                AmtBalance = dsagrrementamt.Tables("T080031").Rows(0).Item(0)
                ' txtAgreementAmt.Text = AmtBalance
            End If
        Catch ex As Exception
            CreateLog("EditInvoice", "GetAggrementAmt-169", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

End Class
