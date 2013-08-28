'*********************************************************************************************************
' Page                 : - Invoice Details
' Purpose              : - 
' Tables used          : - UDC, T010011, t060011, T080031, T080035, T040011, T040021, T040031, T080034,                                 T010011, T030201, T080022
' Date					   Author						Modification Date				Description
' 15/05/06			       jaswinder		            ------------------			    Created
'
' Notes: 
' Code:
'*********************************************************************************************************


Imports ION.Data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.Mail
Imports Microsoft.Web.UI.WebControls
Imports System.Data
Imports System.Drawing


Partial Class AdministrationCenter_Agreement_InvoiceDetails
    Inherits System.Web.UI.Page

    Private Shared mTextBox() As TextBox
    Protected Shared mdvtable As DataView = New DataView
    Dim rowvalue As Integer
    Shared mintID As Integer
    Private Shared Flag As Boolean

    Dim insertedBy As String
    Dim insertedOn As String
    Dim systemBy As String

    Dim shF As Short
    Dim flg As Short
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared intCol As Integer
    Private Shared arColWidth As New ArrayList
    Dim mshFlag As Short

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        txtDesc.Attributes.Add("onmousemove", "ShowToolTip(this,500);")
        imgbtnSearch.Attributes.Add("onclick", "return CheckLength();")

        insertedBy = Session("PropUserID")
        insertedOn = Now.ToShortDateString
        systemBy = GetIP()
        txtCSS(Me.Page)
        ''Security Block

        'Dim intID As Int32
        'If Not IsPostBack Then
        '    Dim str As String
        '    str = HttpContext.Current.Session("PropRootDir")
        '    intID = 41
        '    Dim obj As New clsSecurityCache
        '    If obj.ScreenAccess(intID) = False Then
        '        Response.Redirect("../../frm_NoAccess.aspx")
        '    End If
        '    obj.ControlSecurity(Me.Page, intID)
        'End If

        ''End of Security Block

        If Not IsPostBack Then


            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            rblTemp.Attributes.Add("Onclick", "return getVal();")

            txtPC.Attributes.Add("onblur", "return calculateAmt();")
            txtPC.Attributes.Add("onclick", "return disPC();")
            txtPC.Attributes.Add("onkeypress", "return NumericOnly();")

        End If
        'cpnlError.Visible = False
        lstError.Items.Clear()
        'txtCSS(Me.Page)
        ViewState("SCompany") = Request.QueryString("SCompany")
        ViewState("SInvID") = Request.QueryString("SInvID")
        If Session("PropCompanyType") = "SCM" Then
            cddlStatus.CDDLQuery = "select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=0 and UDCType=""INST"" Order By Name"
        Else
            cddlStatus.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""INST""" & _
            " and UDC.Company=" & Session("PropCompanyID") & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""INST""" & _
            " and UDC.Company=0 Order By Name"
        End If

        cddlStatus.CDDLUDC = True


        '--Customer

        cddlCustomer.CDDLQuery = "SELECT CI_NU8_Address_Number as ID,CI_VC36_Name as Name,CI_VC36_Name  as AliasName FROM t010011 where CI_VC8_Address_Book_Type='COM'  and CI_VC8_Status='ENA' AND CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ") Order By Name"

        cddlCustomer.CDDLMandatoryField = True
        cddlCustomer.CDDLUDC = False
        cddlCustomer.CDDLAutopostback = True
        '--Customer

        cddlPerson.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid+'  ['+ci_vc36_name+']' as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and CI_IN4_Business_Relation='SCM' Order By Name"

        cddlPerson.CDDLUDC = False


        If Not IsPostBack Then

            cddlStatus.CDDLFillDropDown()
            cddlStatus.Width = Unit.Pixel(120)

            cddlPerson.CDDLFillDropDown(10, False)
            cddlPerson.Width = Unit.Pixel(120)

            cddlCustomer.CDDLFillDropDown(10, False)
            cddlCustomer.Width = Unit.Pixel(120)

        End If
        If Not IsPostBack Then
            If ViewState("SCompany") <> "" Then
                cddlCustomer.CDDLSetSelectedItem(ViewState("SCompany"), False, WSSSearch.SearchCompNameID(Val(ViewState("SCompany"))).ExtraValue)
            Else
                cddlCustomer.CDDLSetSelectedItem(Session("PropCompanyID"), False, WSSSearch.SearchCompNameID(Val(Session("PropCompanyID"))).ExtraValue)
            End If


        End If

        If CDDLTo.CDDLGetValue = "" Then
            CDDLTo.CDDLSetSelectedItem("", False, "")
            If cddlCustomer.CDDLGetValue <> "" Then
                CDDLTo.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id=" & cddlCustomer.CDDLGetValue & ") and UM_IN4_Company_AB_ID=" & cddlCustomer.CDDLGetValue & " Order By Name"
                CDDLTo.CDDLMandatoryField = True
                CDDLTo.CDDLUDC = False
                CDDLTo.CDDLFillDropDown(10, False)
            End If
        End If
        If IsPostBack = False And Request.QueryString("sts") = "cancel" Then
            ViewState("SInvID") = Request.QueryString("SInvID")
            ViewState("SCompany") = Request.QueryString("SCompany")
            mintID = 0
            txtInvNo.Text = ViewState("SInvID")
            FillDetails()
            reconsiderInvoice()
            Exit Sub
        Else

            If IsPostBack = False Then
                mintID = Request.QueryString(("ID"))
            End If

        End If

        If mintID = -1 Then
            mintID = 0
        Else

        End If


        If Not IsPostBack And mintID = 1 Then
            dtToDate.Text = SetDateFormat(Now, mdlMain.IsTime.DateOnly)
        End If

        Dim hiddenImage As String = Request.Form("txthiddenImage")

        If hiddenImage <> "" Then
            Try
                Select Case hiddenImage
                    Case "Close"
                        Response.Redirect("TempInvoices.aspx?")
                    Case "Logout"
                        LogoutWSS()
                    Case "Ok"

                        If validateForm() Then
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                ''cpnlError.Visible = True
                                lstError.Items.Add("You don't have access rights to Save record")
                                Exit Sub
                            End If
                            'End of Security Block


                            Try
                                If mintID = 0 Then
                                    If updateInvoice() = True And hiddenImage = "Ok" Then
                                        Response.Redirect("TempInvoices.aspx?")
                                    End If
                                    Exit Select
                                ElseIf mintID = -1 Or mintID = 1 Then
                                    If saveInvoice() = True Then
                                        Response.Redirect("TempInvoices.aspx?")
                                    Else
                                        mintID = 1
                                    End If
                                End If
                            Catch ex As Exception
                                lstError.Items.Clear()
                                lstError.Items.Add("server is busy please try later...")
                                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                Exit Sub
                            End Try

                        End If

                    Case "Save"

                        If validateForm() Then
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                ''cpnlError.Visible = True
                                lstError.Items.Add("You don't have access rights to Save record...")
                                Exit Sub
                            End If
                            'End of Security Block


                            Try
                                If mintID = 0 Then 'Open in Update mode
                                    If updateInvoice() Then
                                        FillDetails()
                                        FillView() ' Fill Main grid
                                        fillGrid() ' Fill annexure grid
                                        txthiddenImage.Text = ""
                                        Exit Sub
                                    End If

                                ElseIf mintID = -1 Or mintID = 1 Then 'open in Add Mode
                                    If saveInvoice() = True Then

                                        cpnlICall.Enabled = True
                                        cpnlICall.State = CustomControls.Web.PanelState.Expanded
                                        cpnlHrsDetails.Enabled = True
                                        cpnlHrsDetails.State = CustomControls.Web.PanelState.Expanded
                                        ViewState("SInvID") = txtInvNo.Text.Trim
                                        ViewState("SCompany") = cddlCustomer.CDDLGetValue
                                        FillDetails()
                                        FillView() ' Fill Main grid
                                        fillGrid() ' Fill annexure grid
                                        txthiddenImage.Text = ""
                                        mintID = 0
                                        Exit Sub
                                    Else
                                        mintID = 1
                                    End If
                                End If
                            Catch ex As Exception
                                lstError.Items.Clear()
                                lstError.Items.Add("Server is busy please try later...")
                                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                Exit Sub
                            End Try

                        End If

                    Case "Delete"
                        txthiddenImage.Text = ""
                        Exit Sub
                    Case "Search"
                        Dim strInv As String = ""
                        If FillView() Then
                            Try
                                'cpnlError.Visible = False

                                'get previous invoice date if any for the current customer and display

                                strInv = SQL.Search("searchInv", "Inv details", "select IM_DT8_Invoice_To_Date from T080031 where IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue & " and IM_NU9_Invoice_ID_PK in(select max(IM_NU9_Invoice_ID_PK) from T080031 where IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue & ")")
                                If (strInv <> "") Or (Not strInv Is Nothing) Then
                                    lblLastInv.Text = "Invoicing Done Upto Date " & CDate(strInv).AddDays(-1).ToShortDateString.ToString
                                Else
                                    lblLastInv.Text = ""
                                End If
                            Catch ex As Exception
                                CreateLog("InvoiceDetails", "Load-362", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")

                            End Try
                        Else

                            Try
                                cpnlICall.State = CustomControls.Web.PanelState.Collapsed

                                'get previous invoice date if any for the current customer and display

                                strInv = SQL.Search("searchInv", "Inv details", "select IM_DT8_Invoice_To_Date from T080031 where IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue & " and IM_NU9_Invoice_ID_PK in(select max(IM_NU9_Invoice_ID_PK) from T080031 where IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue & ")")
                                If strInv <> "" Or strInv Is Nothing Then
                                    lblLastInv.Text = "Invoicing Done Upto Date " & CDate(strInv).AddDays(-1).ToShortDateString.ToString
                                Else
                                    lblLastInv.Text = ""
                                End If
                            Catch ex As Exception
                                CreateLog("InvoiceDetails", "Load-119", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")

                            End Try

                            lstError.Items.Add("No Calls are available for invoicing during specified Time...")
                            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            Exit Sub
                        End If

                    Case "Reset"
                        cpnlICall.Enabled = True
                        cpnlICall.State = CustomControls.Web.PanelState.Expanded
                        cpnlHrsDetails.Enabled = True
                        cpnlHrsDetails.State = CustomControls.Web.PanelState.Expanded
                        txthiddenImage.Text = ""
                        Exit Sub
                End Select
            Catch ex As Exception
                CreateLog("InvoiceDeails", "Load-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            End Try

        Else

        End If

        If (Not IsPostBack) And (Not ViewState("SInvID") = "") Or (Not txtInvNo.Text = "") Then
            txtInvNo.Text = ViewState("SInvID")
            cddlCustomer.Enabled = False
            FillDetails() 'Fill Invoice master details
            FillView() 'Fill Main invoice grid
            fillGrid() 'Fill annexure grid
            cpnlICall.Enabled = True
            cpnlICall.State = CustomControls.Web.PanelState.Expanded
        ElseIf txthiddenImage.Text = "Search" Then
            cpnlICall.Enabled = True
            cpnlICall.State = CustomControls.Web.PanelState.Expanded
        Else
            cddlCustomer.Enabled = True
            cpnlICall.Enabled = False
            cpnlICall.State = CustomControls.Web.PanelState.Collapsed
        End If

        '*************************************************************************
        'Security Block
        Dim intid As String
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = 717
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If
        'End of Security Block
    End Sub

    Function validateForm() As Boolean

        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        Try
            If cddlCustomer.CDDLGetValue.Trim = "" Then
                lstError.Items.Add("Customer Name cannot be blank...")
                shFlag = 1
            End If
            If CDDLTo.CDDLGetValue.Trim = "" Then
                lstError.Items.Add("To Invoice Field cannot be blank...")
                shFlag = 1
            End If
            If cddlPerson.CDDLGetValue.Trim = "" Then
                lstError.Items.Add("Select Reference Person")
                shFlag = 1
            End If

            If cddlStatus.CDDLGetValue.Trim = "" Then
                lstError.Items.Add("Status of Invoice cannot be Blank...")
                shFlag = 1
            End If
            'sachin
            If dtFromDate.Text = "" Then
                lstError.Items.Add("Invoice From Date  cannot be Blank...")
                shFlag = 1
            End If

            If dtDueDate.Text = "" Then
                lstError.Items.Add("Due date cannot be Blank...")
                shFlag = 1
            End If

            If dtToDate.Text <= Now.Date.AddDays(1) Then
            Else
                lstError.Items.Add("To date cannot be greater than current date...")
                shFlag = 1
            End If
            If IsDate(dtDueDate.Text) And IsDate(dtToDate.Text) Then
                If CDate(dtToDate.Text) >= CDate(dtDueDate.Text) Then
                    lstError.Items.Add("To date cannot be greater than or equal to Due date...")
                    shFlag = 1
                End If
            End If
            If CDate(dtFromDate.Text) >= CDate(dtToDate.Text) Then
                lstError.Items.Add("From date cannot be greater than or equal to To date...")
                shFlag = 1
            End If
            If shFlag = 1 Then
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                shFlag = 0
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ''cpnlError.Visible = True

            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            shFlag = 0
            Return False
        End Try

    End Function

#Region "update"

    Sub FillDetails()

        Dim strConnection As String
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String

        ' get Invoice master details and display

        strSql = "select *,Per.CI_VC36_Name pName,comp.CI_VC36_Name cName from T080031,T010011 as comp,T010011 as Per where Per.CI_NU8_Address_Number=IM_VC8_Invoice_Reference and comp.CI_NU8_Address_Number=IM_NU9_Company_ID_PK and IM_NU9_Invoice_ID_PK=" & txtInvNo.Text.Trim & " and IM_NU9_Company_ID_PK=" & ViewState("SCompany")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        ' SQL.DBTable = "T080031"

        Try
            sqrdRecords = SQL.Search("", "", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                sqrdRecords.Read()

                txtInvNo.Text = IIf(IsDBNull(sqrdRecords.Item("IM_NU9_Invoice_ID_PK")), " ", sqrdRecords.Item("IM_NU9_Invoice_ID_PK"))

                cddlCustomer.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("IM_NU9_Company_ID_PK")), " ", sqrdRecords.Item("IM_NU9_Company_ID_PK")), False, sqrdRecords.Item("cName"))

                txtReference.Text = IIf(IsDBNull(sqrdRecords.Item("IM_VC50_Invoice_Email")), " ", sqrdRecords.Item("IM_VC50_Invoice_Email"))

                CDDLTo.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("IM_NU9_Invoice_To_AB_ID_FK")), " ", sqrdRecords.Item("IM_NU9_Invoice_To_AB_ID_FK")), False)

                cddlStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("IM_VC8_Invoice_Status")), " ", sqrdRecords.Item("IM_VC8_Invoice_Status")))

                dtFromDate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("IM_DT8_Invoice_From_Date")), " ", sqrdRecords.Item("IM_DT8_Invoice_From_Date")), mdlMain.IsTime.DateOnly)

                dtToDate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("IM_DT8_Invoice_To_Date")), " ", sqrdRecords.Item("IM_DT8_Invoice_To_Date")), mdlMain.IsTime.DateOnly)

                dtDueDate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("IM_DT8_Invoice_Due_Date")), " ", sqrdRecords.Item("IM_DT8_Invoice_Due_Date")), mdlMain.IsTime.DateOnly)

                cddlPerson.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("IM_VC8_Invoice_Reference")), " ", sqrdRecords.Item("IM_VC8_Invoice_Reference")), False, sqrdRecords.Item("pName"))

                txtDesc.Text = IIf(IsDBNull(sqrdRecords.Item("IM_VC500_Invoice_Comment")), " ", sqrdRecords.Item("IM_VC500_Invoice_Comment"))

                rblTemp.SelectedValue = sqrdRecords.Item("IM_CH1_Invoice_Temp")

                txtPC.Text = IIf(IsDBNull(sqrdRecords.Item("IM_NU9_Invoice_discount_PC")), "", sqrdRecords.Item("IM_NU9_Invoice_discount_PC"))

                txtDAmt.Text = IIf(IsDBNull(sqrdRecords.Item("IM_NU9_Invoice_discount_Amt")), "", sqrdRecords.Item("IM_NU9_Invoice_discount_Amt"))

                txtTotAmt.Text = IIf(IsDBNull(sqrdRecords.Item("IM_NU9_Invoice_Amount")), "", sqrdRecords.Item("IM_NU9_Invoice_Amount"))


                'sqrdRecords.Close()

                'dtToDate.readOnlyDate = False
                'dtFromDate.readOnlyDate = False

            Else
                ''cpnlError.Visible = True
                'cpnlError.State = CustomControls.Web.PanelState.Expanded
                'cpnlError.Text = "No records..."
            End If
            sqrdRecords.Close()

        Catch ex As Exception
            ' 'cpnlError.Visible = True
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            ' 'cpnlError.Text = "Error"
            lstError.Items.Add("Server is busy Please try Later...")    'ex.Message.ToString()
            CreateLog("InvoiceDetails", "FillDetails-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        Finally

        End Try
    End Sub

    Function updateInvoice() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        If Not txtReference.Text = "" Then
            Try
                'Get current invoice master values from input medium and update

                getValues(arColumnName, arRowData)
                mstGetFunctionValue = WSSUpdate.UpdateInvoice(txtInvNo.Text.Trim, cddlCustomer.CDDLGetValue, arColumnName, arRowData)

                arColumnName.Clear()
                arRowData.Clear()

                If mstGetFunctionValue.ErrorCode = 0 Then

                    Dim dtCallPrice As New DataTable
                    ' SQL.DBTable = "CallPrice"
                    'Read invoice main grid and update invoice details
                    dtCallPrice = readCallPriceGrid()
                    If updatePriceOnCall(dtCallPrice) Then
                        If chkMail.Checked Then 'send mail if checkbox checked
                            Dim mess As String
                            mess = "Invoice No: " & txtInvNo.Text & " created for the Customer: " & cddlCustomer.CDDLGetValueName

                            SMPTEmail("INVOICING INTIMATION", mess, txtReference.Text.Trim)
                        End If
                    End If
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    '  ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ' ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            Catch ex As Exception

                lstError.Items.Add("server is busy please try later...")
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("CraeteUser", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
                Return False
            End Try

        Else

            lstError.Items.Add("Enter reference Person's E-mail ID...")

            ''cpnlError.Visible = True
            'ImgError.ImageUrl = "../../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ''cpnlError.TitleCSS = "test3"
            ''cpnlError.State = CustomControls.Web.PanelState.Expanded
            ''cpnlError.Text = "Message"
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
        End If


    End Function

    Function readCallPriceGrid() As DataTable
        'Create annexure datatable
        Try
            Dim dtCallPrice As New DataTable

            dtCallPrice.Columns.Add("CallType")
            dtCallPrice.Columns.Add("TotalHours")
            dtCallPrice.Columns.Add("TotalAmount")
            dtCallPrice.Columns.Add("DisPC")
            dtCallPrice.Columns.Add("DisAmount")

            Dim gridrow As DataGridItem
            Dim drow As DataRow

            'read grid and create datatable

            For Each gridrow In grdReport.Items
                drow = dtCallPrice.NewRow

                drow.Item("CallType") = CType(gridrow.FindControl("lblCallType"), Label).Text
                drow.Item("TotalHours") = CType(gridrow.FindControl("lblTotalHours"), Label).Text
                drow.Item("TotalAmount") = CType(gridrow.FindControl("lblTotAmt"), Label).Text
                drow.Item("DisPC") = CType(gridrow.FindControl("txtDPC"), TextBox).Text
                drow.Item("DisAmount") = CType(gridrow.FindControl("txtDisAmt"), TextBox).Text

                dtCallPrice.Rows.Add(drow)
            Next

            Return dtCallPrice

        Catch ex As Exception
            CreateLog("InvoiceDetail", "readCallPriceGrid-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Function

    Function updatePriceOnCall(ByRef dtCallPrice As DataTable) As Boolean
        'This function update price in annexure
        Dim cnt As Integer
        Dim arrColumns As New ArrayList
        Dim arRow As New ArrayList

        Try

            arrColumns.Clear()
            arRow.Clear()

            Dim Multi As SQL.AddMultipleRows
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            'SQL.DBTable = "T080035"
            SQL.DBTracing = False

            'delete previously given discounts on call types i.e. with reference Annexure grid

            If SQL.Delete("Invoicedetails", "UpdatePriceOnCall", "delete from T080035 where CP_NU9_Inv_No=" & txtInvNo.Text.Trim & " and CP_NU9_Customer_ID=" & cddlCustomer.CDDLGetValue, SQL.Transaction.Serializable) Then

                arrColumns.Add("CP_NU9_Inv_No")
                arrColumns.Add("CP_NU9_Customer_ID")

                arrColumns.Add("CP_VC8_Call_Type")
                arrColumns.Add("CP_NU9_Billable_Hrs")
                arrColumns.Add("CP_NU9_Total_Amt")
                arrColumns.Add("CP_NU9_Discount_PC")
                arrColumns.Add("CP_NU9_Discounted_Amt")

                For cnt = 0 To dtCallPrice.Rows.Count - 1

                    arRow.Add(txtInvNo.Text.Trim)
                    arRow.Add(cddlCustomer.CDDLGetValue)

                    arRow.Add(dtCallPrice.Rows(cnt).Item("CallType"))
                    arRow.Add(dtCallPrice.Rows(cnt).Item("TotalHours"))
                    arRow.Add(dtCallPrice.Rows(cnt).Item("TotalAmount"))

                    'overwrite discounts on call types in annexure table

                    If txtDisType.Text.Trim = "0" Then
                        arRow.Add(IIf(dtCallPrice.Rows(cnt).Item("DisPC") = "", DBNull.Value, dtCallPrice.Rows(cnt).Item("DisPC")))
                        arRow.Add(dtCallPrice.Rows(cnt).Item("DisAmount"))
                    Else
                        arRow.Add(DBNull.Value)
                        arRow.Add(dtCallPrice.Rows(cnt).Item("TotalAmount"))
                    End If


                    Multi.Add("T080035", arrColumns, arRow)

                Next

                Multi.Save()
                Multi.Dispose()
                Return True
            Else
                Return False
            End If


        Catch ex As Exception
            CreateLog("UpdatePriceOnCall", "UpdatePriceOnCallContact-769", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return False
        End Try
    End Function


#End Region

#Region "Searchable Call Grid"

#Region "fill View"

    Private Function FillView() As Boolean
        'this function fill Invoice grid
        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String

        If txtInvNo.Text = "" Then 'If creating new invoice

            strselect = "select distinct AM_NU9_Call_Number as CallNo,CM_VC8_Call_Type as CallType,AM_NU9_Task_Number as TaskNo,TM_VC8_task_type as TaskType,CN_VC20_Call_Status as CallStatus,CM_VC2000_Call_Desc CallDesc,CM_NU9_Agreement as AgNo from T040011,T040021,T040031 where TM_NU9_Call_No_FK = CM_NU9_Call_No_PK And TM_NU9_Comp_ID_FK = CM_NU9_Comp_Id_FK and TM_NU9_Call_No_FK=AM_NU9_Call_Number and AM_NU9_Task_Number=TM_NU9_Task_no_PK and AM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and AM_DT8_Action_Date between '" & dtFromDate.Text & "' and '" & dtToDate.Text & "' and (AM_CH1_IsInvoiced<>'Y' or AM_CH1_IsInvoiced is null) and AM_NU9_Comp_ID_FK=" & cddlCustomer.CDDLGetValue

        Else ' If Updating existing invoice
            strselect = "select distinct PR_NU9_Call_No as CallNo,PR_VC8_Call_Type as CallType,CM_VC2000_Call_Desc CallDesc,CN_VC20_Call_Status as CallStatus,CM_NU9_Agreement as AgNo,PR_NU9_Task_No as TaskNo,PR_VC8_Task_Type as TaskType from T040011,T080034 where PR_NU9_Call_No=CM_NU9_Call_No_PK and PR_NU9_Customer_ID_FK=CM_NU9_Comp_Id_FK and PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue
        End If

        ' SQL.DBTable = "T030201"


        If SQL.Search("T030201", "Invoice Details", "fill View", strselect, dsFromView, "sachin", "Prashar") Then

            Try
                mdvtable.Table = dsFromView.Tables("T030201")
                HTMLEncodeDecode(mdlMain.Action.Encode, mdvtable)
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                FormatGrid()
                GetColumns()
                Return True

            Catch ex As Exception
                CreateLog("Invoice Details", "Fill View", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            Dim ds As New DataSet
            ds.Tables.Add("Dummy")
            ds.Tables("Dummy").Columns.Add("CallNo")
            ds.Tables("Dummy").Columns.Add("CallType")
            ds.Tables("Dummy").Columns.Add("TaskNo")
            ds.Tables("Dummy").Columns.Add("TaskType")
            ds.Tables("Dummy").Columns.Add("CallDesc")
            ds.Tables("Dummy").Columns.Add("CallStatus")

            Dim dtRow As DataRow

            dtRow = ds.Tables("Dummy").NewRow
            dtRow.Item(0) = ""

            mdvtable.Table = ds.Tables("Dummy")

            GrdAddSerach.DataSource = mdvtable.Table
            GrdAddSerach.DataBind()

            FormatGrid()
            Return False

        End If

    End Function

    Sub fillGrid()
        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String

        'Fill Annexure grid
        strselect = "select CP_VC8_Call_Type CallType,CP_NU9_Billable_Hrs TotalHours,CP_NU9_Total_Amt TotalAmount,CP_NU9_Discounted_Amt DisAmount,CP_NU9_Discount_PC as DisPC from T080035 where CP_NU9_Inv_No=" & txtInvNo.Text.Trim & " and CP_NU9_Customer_ID=" & cddlCustomer.CDDLGetValue


        'SQL.DBTable = "T030201"


        Try

            If SQL.Search("T030201", "InvoiceDetails", "fillGrid", strselect, dsFromView, "sachin", "Prashar") Then
                mdvtable.Table = dsFromView.Tables("T030201")

                grdReport.DataSource = mdvtable
                grdReport.DataBind()

            Else

                Dim ds As New DataSet
                ds.Tables.Add("Dummy")
                ds.Tables("Dummy").Columns.Add("CallType")
                ds.Tables("Dummy").Columns.Add("TotalHours")
                ds.Tables("Dummy").Columns.Add("TotalAmount")
                ds.Tables("Dummy").Columns.Add("DisAmount")

                Dim dtRow As DataRow

                dtRow = ds.Tables("Dummy").NewRow
                dtRow.Item(0) = ""

                mdvtable.Table = ds.Tables("Dummy")

                grdReport.DataSource = mdvtable.Table
                grdReport.DataBind()

                FormatGrid()

            End If
        Catch ex As Exception
            CreateLog("Invoice Details", "Fillgrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Sub CreateTextBox()

        arColumns.Clear()

        Dim _textbox As TextBox
        Dim intii As Integer

        arCol.Clear()

        arCol.Add("CallNo")
        arCol.Add("CallType")
        arCol.Add("TaskNo")
        arCol.Add("TaskType")
        arCol.Add("CallDesc")
        arCol.Add("Status")
        'arCol.Add(""BillHrs"")


        'fill the columns count into the array from mdvtable view

        intCol = mdvtable.Table.Columns.Count


        ReDim mTextBox(intCol)

        Try
            For intii = 0 To intCol - 1
                _textbox = New TextBox

                Dim col1 As Unit
                Dim col1cng As String
                Dim strcolid As String
                col1 = Unit.Parse(arColWidth.Item(intii))
                col1cng = col1.Value + 1
                col1cng = col1cng & "pt"

                If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                    _textbox.Text = ""
                Else
                    _textbox.Text = arrtextvalue.Item(intii)
                End If

                '_textbox.Text = ""
                strcolid = arCol.Item(intii)

                If strcolid = "ScreenName" Then
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " BackColor=""WhiteSmoke"" height=""20px""  readonly=""true"" BorderWidth=""1px"" BorderColor=""Gray""></asp:TextBox>"))
                Else
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                End If


                _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                mTextBox(intii) = _textbox

                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("Invoice Details", "CreateTextBox-956", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(35)
        arColWidth.Add(60)
        arColWidth.Add(35)
        arColWidth.Add(60)
        arColWidth.Add(180)
        arColWidth.Add(80)
        'arColWidth.Add(80)

        Try
            GrdAddSerach.Columns.Clear()
            GrdAddSerach.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                GrdAddSerach.Columns.Add(Bound_Column)
            Next

        Catch ex As Exception
            CreateLog("Invoice Detail", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(60)
        arColWidth.Add(35)
        arColWidth.Add(60)
        arColWidth.Add(180)
        arColWidth.Add(80)
        'arColWidth.Add(80)


        arColumnName.Add("CallNo")
        arColumnName.Add("CallType")
        arColumnName.Add("TaskNo")
        arColumnName.Add("TaskType")
        arColumnName.Add("CallDesc")
        arColumnName.Add("CallStatus")
        'arColumnName.Add("BillHrs")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()


        arColWidth.Add(35)
        arColWidth.Add(60)
        arColWidth.Add(35)
        arColWidth.Add(60)
        arColWidth.Add(180)
        arColWidth.Add(80)
        'arColWidth.Add(80)



        arColumnName.Add("CallNo")
        arColumnName.Add("CallType")
        arColumnName.Add("TaskNo")
        arColumnName.Add("TaskType")
        arColumnName.Add("CallDesc")
        arColumnName.Add("CallStatus")
        'arColumnName.Add("BillHrs")


    End Sub

#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strAgNo, strCType, strTaskNo, strTType As String
        Dim rowFlag, hasAgr As Boolean
        rowFlag = True

        Try
            For Each dcCol In mdvtable.Table.Columns


                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strTaskNo = CType(e.Item.FindControl("lblTaskNo"), Label).Text
                    strAgNo = CType(e.Item.FindControl("lblAgNo"), Label).Text
                    strCType = CType(e.Item.FindControl("lblCType"), Label).Text
                    strTType = CType(e.Item.FindControl("lblTaskType"), Label).Text


                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "','" & rowvalue & "','" & strAgNo & "','" & strCType & "','" & strTaskNo & "','" & strTType & "')")

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "', '" & strAgNo & "','" & strCType & "','" & rowvalue & "')")

                    If rowFlag Then
                        getHrs(strID, CType(e.Item.FindControl("lblActHrs"), Label), CType(e.Item.FindControl("txtBillHrs"), TextBox), CType(e.Item.FindControl("lblCallAmt"), Label), hasAgr, strTaskNo)
                        If Not hasAgr Then
                            For cnt As Integer = 0 To e.Item.Cells.Count - 1
                                e.Item.Cells(cnt).BackColor = Color.LightGray
                                e.Item.Cells(cnt).ForeColor = Color.DarkGray
                                e.Item.Cells(cnt).Font.Bold = True
                            Next
                        End If
                    End If

                End If
                rowFlag = False
            Next


            rowvalue += 1
            txtCnt.Text = mdvtable.Table.Rows.Count

        Catch ex As Exception
            CreateLog("Invoice Detail1", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub


    Sub getHrs(ByVal callNo As String, ByRef actHrs As Label, ByRef billHrs As TextBox, ByRef callAmt As Label, ByRef hasAgr As Boolean, ByVal strTaskNo As String)
        Dim sqlRd As SqlDataReader
        Dim stsFlag As Boolean
        hasAgr = True

        If Not txtInvNo.Text = "" Then
            sqlRd = SQL.Search("getHrs", "839", "select isnull(sum(PR_NU9_Hours_Actual),0) as ahrs,isnull(sum(PR_NU9_Hours),0) as bhrs,isnull(sum(PR_NU9_Amount),0) as cAmt from t080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " and PR_NU9_Call_No=" & callNo & " and PR_NU9_Task_No=" & strTaskNo, SQL.CommandBehaviour.CloseConnection, stsFlag)
            If stsFlag Then
                sqlRd.Read()
                actHrs.Text = sqlRd.Item("ahrs")
                billHrs.Text = sqlRd.Item("bhrs")
                callAmt.Text = sqlRd.Item("cAmt")
            End If
            sqlRd.Close()

            sqlRd = SQL.Search("getHrs", "860", "select PR_NU9_Amount as cAmt from t080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " and PR_NU9_Call_No=" & callNo & " and PR_NU9_Task_No=" & strTaskNo & " and PR_NU9_Amount=0", SQL.CommandBehaviour.CloseConnection, stsFlag)

            If stsFlag Then
                hasAgr = False
            End If

        Else

        End If

    End Sub

#End Region

#End Region

#Region "Save"

    Function saveInvoice() As Boolean
        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            'Generate next invoice number for thw customer

            txtInvNo.Text = SQL.Search("InvoiceMaster", "SaveInvoice", "select isnull(max(IM_NU9_Invoice_ID_PK),0) from T080031 where IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue)

            txtInvNo.Text = CInt(txtInvNo.Text) + 1

            'Get and save the email of the reference person entered

            sqrdRecords = SQL.Search("", "", "select CI_VC28_Email_1,CI_VC28_Email_2,CI_VC28_Email_2 from T010011 where CI_NU8_Address_Number=" & cddlPerson.CDDLGetValue, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                sqrdRecords.Read()
                txtReference.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC28_Email_1")), sqrdRecords.Item("CI_VC28_Email_2"), sqrdRecords.Item("CI_VC28_Email_1"))
            End If


            getValues(arrColumns, arrRows)
            mstGetFunctionValue = saveInvoiceMaster(arrColumns, arrRows, "T080031")

            If mstGetFunctionValue.ErrorCode = 0 Then

                Dim dsFromView As New DataSet
                'SQL.DBTable = "T030201"

                'get all the call/task no. that comes in the invoicing range

                If SQL.Search("T030201", "invoice Details", "saveInvoice", "select distinct AM_NU9_Call_Number as CallNo,CM_VC8_Call_Type as CallType,AM_NU9_Task_Number as TaskNo,TM_VC8_task_type as TaskType,CN_VC20_Call_Status as CallStatus,CM_VC2000_Call_Desc CallDesc,isnull(CM_NU9_Agreement,-1) as AgNo from T040011,T040021,T040031 where TM_NU9_Call_No_FK = CM_NU9_Call_No_PK And TM_NU9_Comp_ID_FK = CM_NU9_Comp_Id_FK and TM_NU9_Call_No_FK=AM_NU9_Call_Number and AM_NU9_Task_Number=TM_NU9_Task_no_PK and AM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and AM_DT8_Action_Date between '" & dtFromDate.Text & "' and '" & dtToDate.Text & "'  and (AM_CH1_IsInvoiced<>'Y' or AM_CH1_IsInvoiced is null) and AM_NU9_Comp_ID_FK=" & cddlCustomer.CDDLGetValue, dsFromView, "sachin", "Prashar") Then
                    ' Save invoice details

                    If saveCall(dsFromView.Tables("T030201")) Then

                    End If
                End If
            End If

            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                ' lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ' ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ' ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            CreateLog("Invoice Detail1", "Save Invoice-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try


    End Function

    Sub getValues(ByRef arrColumns As ArrayList, ByRef arrRows As ArrayList)

        arrColumns.Add("IM_NU9_Invoice_ID_PK")
        arrColumns.Add("IM_NU9_Company_ID_PK")
        arrColumns.Add("IM_VC8_Invoice_Status")
        arrColumns.Add("IM_DT8_Invoice_Status_Date")
        arrColumns.Add("IM_NU9_Invoice_Created_By")
        arrColumns.Add("IM_DT8_Invoice_Created_Date")
        arrColumns.Add("IM_DT8_Invoice_Due_Date")
        arrColumns.Add("IM_VC500_Invoice_Comment")
        arrColumns.Add("IM_VC8_Invoice_Reference")
        arrColumns.Add("IM_VC50_Invoice_Email")
        arrColumns.Add("IM_CH1_Invoice_Temp")
        arrColumns.Add("IM_DT8_Invoice_From_Date")
        arrColumns.Add("IM_DT8_Invoice_To_Date")
        arrColumns.Add("IM_NU9_Invoice_discount_PC")
        arrColumns.Add("IM_NU9_Invoice_discount_Amt")
        arrColumns.Add("IM_NU9_Invoice_Amount")
        arrColumns.Add("IM_NU9_Invoice_Ref_No")
        arrColumns.Add("IM_NU9_Invoice_To_AB_ID_FK")
        arrColumns.Add("IM_NU9_Invoice_From_Comp_ID_FK")

        arrRows.Add(txtInvNo.Text.Trim)
        arrRows.Add(cddlCustomer.CDDLGetValue)
        arrRows.Add(cddlStatus.CDDLGetValue)
        arrRows.Add(Now.ToShortDateString)
        arrRows.Add(Session("propUserID"))
        arrRows.Add(Now.ToShortDateString)
        arrRows.Add(IIf(dtDueDate.Text = "", DBNull.Value, dtDueDate.Text))
        arrRows.Add(txtDesc.Text.Trim)
        arrRows.Add(cddlPerson.CDDLGetValue)
        arrRows.Add(txtReference.Text.Trim)
        arrRows.Add(rblTemp.SelectedValue)
        arrRows.Add(IIf(dtFromDate.Text = "", DBNull.Value, dtFromDate.Text))
        arrRows.Add(dtToDate.Text)
        arrRows.Add(IIf((txtPC.Text.Trim = "NaN" Or txtPC.Text.Trim = ""), DBNull.Value, txtPC.Text.Trim))
        'if discount PC is blank then then net amt=total amt else Net = Total- discount
        arrRows.Add(IIf(txtPC.Text.Trim = "", IIf(txtTotAmt.Text.Trim = "", DBNull.Value, txtTotAmt.Text.Trim), txtDAmt.Text.Trim))
        arrRows.Add(IIf(txtTotAmt.Text.Trim = "", DBNull.Value, txtTotAmt.Text.Trim))
        arrRows.Add(IIf(txtPreInv.Text.Trim = "", DBNull.Value, txtPreInv.Text.Trim))
        arrRows.Add(CDDLTo.CDDLGetValue)
        arrRows.Add(Session("PropCompanyID"))

    End Sub

    Function saveCall(ByVal dtCalls As DataTable) As Boolean
        'This function save invoice details and annexure
        Dim strselect, insQuery, strUpdate As String
        Dim rowCnt As Integer = dtCalls.Rows.Count
        Try
            For cnt As Integer = 0 To rowCnt - 1
                Dim dsFromView As New DataSet
                Dim dsHrs As New DataSet
                Dim extHrs As String
                Dim dsFixCur As New DataSet

                ' get used hours by skill level on a perticular call and task number

                strselect = "select isnull(sum(AM_FL8_Used_Hr),0) as UsedHours,CI_VC8_Level as SkillLevel from T040031, T010011 where AM_NU9_Call_Number = " & dtCalls.Rows(cnt).Item("CallNo") & " And AM_NU9_Task_Number=" & dtCalls.Rows(cnt).Item("TaskNo") & " And AM_NU9_Comp_ID_FK = " & cddlCustomer.CDDLGetValue & " And CI_NU8_Address_Number = AM_VC8_Supp_Owner and AM_DT8_Action_Date between '" & dtFromDate.Text & "' and '" & dtToDate.Text & "' and (AM_CH1_IsInvoiced<>'Y' or AM_CH1_IsInvoiced is null) group by CI_VC8_Level"

                ' SQL.DBTable = "Calls"

                If SQL.Search("Calls", "InvoicinDetails", "Save", strselect, dsFromView, "sachin", "Prashar") Then
                    For cntLevel As Integer = 0 To dsFromView.Tables("Calls").Rows.Count - 1

                        'save the hrs. and skill combination in invoice details

                        insQuery = "insert into T080034 (PR_NU9_Invoice_No_FK,PR_NU9_Customer_ID_FK,PR_NU9_Call_No,PR_VC8_Call_Type,PR_NU9_Skill_Level,PR_NU9_Hours_Actual,PR_NU9_Hours,PR_NU9_Task_No,PR_VC8_Task_Type) values(" & txtInvNo.Text.Trim & "," & cddlCustomer.CDDLGetValue & "," & dtCalls.Rows(cnt).Item("CallNo") & ",'" & dtCalls.Rows(cnt).Item("CallType") & "','" & dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel") & "'," & dsFromView.Tables("Calls").Rows(cntLevel).Item("UsedHours") & "," & dsFromView.Tables("Calls").Rows(cntLevel).Item("UsedHours") & "," & dtCalls.Rows(cnt).Item("TaskNo") & ",'" & dtCalls.Rows(cnt).Item("TaskType") & "')"

                        If SQL.Save("InvoicinDetails", "saveCall-716", insQuery, SQL.Transaction.Serializable) Then

                            'get sum of action hrs of only external action for a skill level 
                            '****************************************
                            strselect = "select isnull(sum(AM_FL8_Used_Hr),0) as UsedHours,CI_VC8_Level as SkillLevel from T040031, T010011 where AM_NU9_Call_Number = " & dtCalls.Rows(cnt).Item("CallNo") & " And AM_NU9_Task_Number=" & dtCalls.Rows(cnt).Item("TaskNo") & " And AM_NU9_Comp_ID_FK = " & cddlCustomer.CDDLGetValue & " And CI_NU8_Address_Number = AM_VC8_Supp_Owner and AM_DT8_Action_Date between '" & dtFromDate.Text & "' and '" & dtToDate.Text & "' and (AM_CH1_IsInvoiced<>'Y' or AM_CH1_IsInvoiced is null) and CI_VC8_Level='" & dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel") & "' and AM_VC8_ActionType='External' group by CI_VC8_Level"

                            If SQL.Search("T040031", "InvoicinDetails", "Save", strselect, dsHrs, "sachin", "Prashar") Then
                                extHrs = dsHrs.Tables(0).Rows(0).Item("UsedHours")
                            Else
                                extHrs = "0"
                            End If
                            'update invoice details

                            strUpdate = "update T080034 set PR_NU9_Hours=" & extHrs & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " and PR_NU9_Call_No=" & dtCalls.Rows(cnt).Item("CallNo") & " and PR_NU9_Task_No=" & dtCalls.Rows(cnt).Item("TaskNo") & " and PR_NU9_Skill_Level='" & dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel") & "'"

                            If SQL.Update("InvoicinDetails", "Save", strUpdate, SQL.Transaction.Serializable) Then
                            End If

                            '************************************

                            Dim Price, Type As String
                            'get specific agreement line and type of agreement (i.e. fixed or hourly) and proceed if type=-1 then price=0 i.e. no specific agr. line found

                            getPriceType(dtCalls.Rows(cnt).Item("CallType"), dtCalls.Rows(cnt).Item("TaskType"), Price, Type, dtCalls.Rows(cnt).Item("AgNo"), dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel"))


                            If Not Type = "-1" Then

                                If Type = "H" Then 'For Hourly Price type

                                    strUpdate = "update T080034 set PR_CH1_Fix_Amount='N',PR_NU9_Amount=PR_NU9_Hours*" & Price & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " and PR_NU9_Call_No=" & dtCalls.Rows(cnt).Item("CallNo") & " and PR_NU9_Task_No=" & dtCalls.Rows(cnt).Item("TaskNo") & " and PR_NU9_Skill_Level='" & dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel") & "'"

                                ElseIf Type = "F" Then 'For Fixed Price type

                                    strUpdate = "update T080034 set PR_CH1_Fix_Amount='Y',PR_NU9_Amount=" & Price & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " and PR_NU9_Call_No=" & dtCalls.Rows(cnt).Item("CallNo") & " and PR_NU9_Task_No=" & dtCalls.Rows(cnt).Item("TaskNo") & " and PR_NU9_Skill_Level='" & dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel") & "'"
                                End If
                            Else 'if no agreement is found
                                strUpdate = "update T080034 set PR_CH1_Fix_Amount='Y',PR_NU9_Amount=" & Price & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " and PR_NU9_Call_No=" & dtCalls.Rows(cnt).Item("CallNo") & " and PR_NU9_Task_No=" & dtCalls.Rows(cnt).Item("TaskNo") & " and PR_NU9_Skill_Level='" & dsFromView.Tables("Calls").Rows(cntLevel).Item("SkillLevel") & "'"
                            End If

                            If SQL.Update("InvoicinDetails", "Save", strUpdate, SQL.Transaction.Serializable) Then
                            End If

                        End If

                    Next

                    Dim sqlRd As SqlDataReader
                    Dim stflg As Boolean
                    Dim billHrs As String

                    'get the actual hrs. for action, if internal=0 otherwise billHrs=ActHrs.

                    strselect = "select AM_NU9_Call_Number as CallNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Action_Number as ActNo,isnull(AM_FL8_Used_Hr,0) ActHrs,AM_VC8_ActionType aType from T040031 where AM_NU9_Task_Number=" & dtCalls.Rows(cnt).Item("TaskNo") & " and AM_NU9_Comp_ID_FK=" & cddlCustomer.CDDLGetValue & " and AM_NU9_Call_Number=" & dtCalls.Rows(cnt).Item("CallNo") & " and AM_DT8_Action_Date between '" & dtFromDate.Text & "' and '" & dtToDate.Text & "' and (AM_CH1_IsInvoiced<>'Y'  or AM_CH1_IsInvoiced is null)"

                    sqlRd = SQL.Search("InvoiceSave", "updateCall-940", strselect, SQL.CommandBehaviour.CloseConnection, stflg)
                    If stflg Then
                        While sqlRd.Read()

                            If sqlRd.Item("aType") = "Internal" Then
                                billHrs = "0"
                            Else
                                billHrs = sqlRd.Item("ActHrs")
                            End If
                            'Update Invoice as created = Y and billable hrs and invoice number
                            SQL.Update("InvoiceSave", "updateCall-940", "update T040031 set AM_CH1_IsInvoiced='Y',AM_NU9_Invoice_No=" & txtInvNo.Text.Trim & ",AM_FL8_BillHrs=" & billHrs & " where AM_NU9_Call_Number=" & sqlRd.Item("CallNo") & " and AM_NU9_Task_Number=" & sqlRd.Item("TaskNo") & " and AM_NU9_Comp_ID_FK=" & cddlCustomer.CDDLGetValue & " and AM_NU9_Action_Number=" & sqlRd.Item("ActNo"), SQL.Transaction.Serializable)
                        End While
                    End If
                    sqlRd.Close()

                End If
            Next

            'Set total amount of the invoice

            strUpdate = "update T080031 set IM_NU9_Invoice_Amount=(select isnull(sum(PR_NU9_Amount),0) from T080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " group by PR_NU9_Invoice_No_FK),IM_NU9_Invoice_Balance=(select isnull(sum(PR_NU9_Amount),0) from T080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " group by PR_NU9_Invoice_No_FK) where IM_NU9_Invoice_ID_PK=" & txtInvNo.Text.Trim & " and IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue

            SQL.Update("InvoicinDetails", "Save", strUpdate, SQL.Transaction.Serializable)

            Dim dsCallPrice As New DataSet
            'SQL.DBTable = "CallPrice"

            'get all the call types and total billable hrs. against them to generate an annexure

            If SQL.Search("CallPrice", "", "", "select PR_VC8_Call_Type CallType,isnull(sum(PR_NU9_Hours),0) TotalHours,isnull(sum(PR_NU9_Amount),0) TotalAmount from T080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue & " group by PR_VC8_Call_Type", dsCallPrice, "sachin", "Prashar") Then

                If savePriceOnCall(dsCallPrice.Tables("CallPrice")) Then
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            ' CreateLog("Invoice Details", SaveCall-807",, , ,,, HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            CreateLog("Invoice Detail1", "SaveCall-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            Return False
        End Try

    End Function

    Function savePriceOnCall(ByRef dtCallPrice As DataTable) As Boolean

        Dim cnt As Integer
        Dim arrColumns As New ArrayList
        Dim arRow As New ArrayList

        Try

            arrColumns.Clear()
            arRow.Clear()

            Dim Multi As SQL.AddMultipleRows
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            ' SQL.DBTable = "T080035"
            SQL.DBTracing = False

            arrColumns.Add("CP_NU9_Inv_No")
            arrColumns.Add("CP_NU9_Customer_ID")

            arrColumns.Add("CP_VC8_Call_Type")
            arrColumns.Add("CP_NU9_Billable_Hrs")
            arrColumns.Add("CP_NU9_Total_Amt")
            arrColumns.Add("CP_NU9_Discounted_Amt")

            'save the annexure details

            For cnt = 0 To dtCallPrice.Rows.Count - 1

                arRow.Add(txtInvNo.Text.Trim)
                arRow.Add(cddlCustomer.CDDLGetValue)

                arRow.Add(dtCallPrice.Rows(cnt).Item("CallType"))
                arRow.Add(dtCallPrice.Rows(cnt).Item("TotalHours"))
                arRow.Add(dtCallPrice.Rows(cnt).Item("TotalAmount"))
                arRow.Add(dtCallPrice.Rows(cnt).Item("TotalAmount"))

                Multi.Add("T080035", arrColumns, arRow)

            Next

            Multi.Save()
            Multi.Dispose()

            Return True

        Catch ex As Exception
            CreateLog("Invoice Details", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
            Return False
        End Try
    End Function


    Function saveInvoiceMaster(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal tblName As String) As ReturnValue

        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            'SQL.DBTable = tblName
            SQL.DBTracing = False

            If SQL.Save(tblName, "WSSSave", "SaveAgreement-806", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2

            CreateLog("WWSSave", "SaveAgreement-929", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Sub getPriceType(ByVal callType As String, ByVal taskType As String, ByRef Price As String, ByRef Type As String, ByVal agNo As String, ByVal skLevel As String)
        Dim slcQry As String
        Dim dsPriceType As New DataSet
        Dim intLineNo As Integer

        'query is used to get the most specific agr. line number against calltype/tasktype/skilllevel for an existing agr. number associated with a perticular call number

        slcQry = "select isnull(isnull(isnull(isnull((select AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & cddlCustomer.CDDLGetValue & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='" & taskType & "' and AL_VC8_Skill_Level='" & skLevel & "'),(select AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & cddlCustomer.CDDLGetValue & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='" & taskType & "' and AL_VC8_Skill_Level='')),(select AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & cddlCustomer.CDDLGetValue & " and AL_VC8_Call_Type='" & callType & "' and  AL_VC8_Skill_Level='" & skLevel & "' and AL_VC8_Task_Type='')),(select AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & cddlCustomer.CDDLGetValue & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='' and  AL_VC8_Skill_Level='' )),0)"

        intLineNo = SQL.Search("InvoiceDetails", "getPriceType", slcQry)

        If intLineNo = 0 Then 'if no agr. line found
            Price = "0"
            Type = "-1"
        Else
            slcQry = "select AL_NU9_Price as price,AL_VC1_Fix_Hour as type from T080022 where  AL_NU9_Line_No_PK=" & intLineNo

            'SQL.DBTable = "T030201"
            If SQL.Search("T030201", "invoiceDetails", "getPriceType", slcQry, dsPriceType, "sachin", "Prashar") Then
                Price = dsPriceType.Tables("T030201").Rows(0).Item("price")
                Type = dsPriceType.Tables("T030201").Rows(0).Item("type")
            Else
                Price = "0"
                Type = "-1"
            End If
        End If

    End Sub

#End Region

#Region "Cancel Invoice"

    Function reconsiderInvoice() As Boolean

        Dim arrColumns As New ArrayList
        Dim arRow As New ArrayList
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean

        'if invoice is cancelled create new drafted invoice with same data

        txtPreInv.Text = txtInvNo.Text.Trim

        txtInvNo.Text = SQL.Search("InvoiceMaster", "SaveInvoice", "select isnull(max(IM_NU9_Invoice_ID_PK),0) from T080031 where IM_NU9_Company_ID_PK=" & cddlCustomer.CDDLGetValue)

        txtInvNo.Text = CInt(txtInvNo.Text) + 1

        'update action master change the invoice number for actions of all the cancelled invoice actions
        If SQL.Update("InvoiceReconsider", "Save-1105", "update T040031 set AM_NU9_Invoice_No=" & txtInvNo.Text.Trim & " where AM_NU9_Invoice_No=" & txtPreInv.Text.Trim & " and AM_NU9_Comp_ID_FK=" & ViewState("SCompany"), SQL.Transaction.Serializable) Then

            'set invoice status=cancelled
            If SQL.Update("InvoiceReconsider", "Save-1105", "update T080031 set IM_VC8_Invoice_Status='CANCELED',IM_DT8_Invoice_Status_Date='" & Now.ToShortDateString & "' where IM_NU9_Invoice_ID_PK=" & txtPreInv.Text.Trim & " and IM_NU9_Company_ID_PK=" & ViewState("SCompany"), SQL.Transaction.Serializable) Then

                Try

                    sqrdRecords = SQL.Search("invoiceDetails", "reconsiderInvoice", "select CI_VC28_Email_1,CI_VC28_Email_2,CI_VC28_Email_2 from T010011 where CI_NU8_Address_Number=" & cddlPerson.CDDLGetValue, SQL.CommandBehaviour.CloseConnection, blnStatus)
                    If blnStatus = True Then
                        sqrdRecords.Read()
                        txtReference.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC28_Email_1")), sqrdRecords.Item("CI_VC28_Email_2"), sqrdRecords.Item("CI_VC28_Email_1"))
                    End If


                    getValues(arrColumns, arRow)
                    mstGetFunctionValue = saveInvoiceMaster(arrColumns, arRow, "T080031")

                    If mstGetFunctionValue.ErrorCode = 0 Then
                        'Get all the invoice details for the cancelled invoice
                        sqrdRecords = SQL.Search("InvoiceReconsider", "Save-1125", "select * from T080034 where PR_NU9_Invoice_No_FK=" & ViewState("SInvID") & "  and PR_NU9_Customer_ID_FK=" & cddlCustomer.CDDLGetValue, SQL.CommandBehaviour.CloseConnection, blnStatus, "imgSave")

                        arrColumns.Clear()
                        arRow.Clear()

                        Dim Multi As SQL.AddMultipleRows
                        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                        ' SQL.DBTable = "T080034"
                        SQL.DBTracing = False

                        arrColumns.Add("PR_NU9_Invoice_No_FK")
                        arrColumns.Add("PR_NU9_Customer_ID_FK")

                        arrColumns.Add("PR_NU9_Call_No")
                        arrColumns.Add("PR_VC8_Call_Type")
                        arrColumns.Add("PR_NU9_Task_No")
                        arrColumns.Add("PR_VC8_Task_Type")
                        arrColumns.Add("PR_NU9_Skill_Level")
                        arrColumns.Add("PR_NU9_Hours_Actual")
                        arrColumns.Add("PR_NU9_Hours")
                        arrColumns.Add("PR_NU9_Amount")
                        arrColumns.Add("PR_CH1_Fix_Amount")

                        If blnStatus = True Then
                            While sqrdRecords.Read
                                arRow.Add(txtInvNo.Text.Trim)
                                arRow.Add(cddlCustomer.CDDLGetValue)

                                arRow.Add(sqrdRecords("PR_NU9_Call_No"))
                                arRow.Add(sqrdRecords("PR_VC8_Call_Type"))
                                arRow.Add(sqrdRecords("PR_NU9_Task_No"))
                                arRow.Add(sqrdRecords("PR_VC8_Task_Type"))
                                arRow.Add(sqrdRecords("PR_NU9_Skill_Level"))
                                arRow.Add(sqrdRecords("PR_NU9_Hours_Actual"))
                                arRow.Add(sqrdRecords("PR_NU9_Hours"))
                                arRow.Add(sqrdRecords("PR_NU9_Amount"))
                                arRow.Add(sqrdRecords("PR_CH1_Fix_Amount"))

                                Multi.Add("T080034", arrColumns, arRow)
                            End While
                            sqrdRecords.Close()
                            Multi.Save()
                            Multi.Dispose()
                            ViewState("SInvID") = txtInvNo.Text.Trim

                            'cpnlError.Text = "Message"
                            lstError.Items.Add("Invoice has been reconsidered...")
                            ' 'cpnlError.Visible = True
                            ''cpnlError.State = CustomControls.Web.PanelState.Expanded
                            'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                            'ImgError.ImageUrl = "../../Images/Pok.gif"
                            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        End If
                        Multi.Dispose()

                        Dim dsFromView As New DataSet
                        Dim strselect As String

                        strselect = "select CP_VC8_Call_Type CallType,CP_NU9_Billable_Hrs TotalHours,CP_NU9_Total_Amt TotalAmount,CP_NU9_Discounted_Amt DisAmount,CP_NU9_Discount_PC as DisPC from T080035 where CP_NU9_Inv_No=" & txtPreInv.Text.Trim & " and CP_NU9_Customer_ID=" & cddlCustomer.CDDLGetValue

                        ' SQL.DBTable = "T030201"

                        If SQL.Search("T030201", "", "", strselect, dsFromView, "sachin", "Prashar") Then
                            updatePriceOnCall(dsFromView.Tables("T030201"))
                        End If


                    End If
                    FillDetails()
                    FillView()
                    fillGrid()
                    Return True
                Catch ex As Exception

                    lstError.Items.Add("server is busy Please try Later...")
                    ''cpnlError.Visible = True
                    'ImgError.ImageUrl = "../../images/warning.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ''cpnlError.TitleCSS = "test3"
                    ''cpnlError.State = CustomControls.Web.PanelState.Expanded
                    ''cpnlError.Text = "Message"
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End Try
            Else

                lstError.Items.Add("server is busy Please try Later...")
                ''cpnlError.Visible = True
                'ImgError.ImageUrl = "../../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ''cpnlError.TitleCSS = "test3"
                ''cpnlError.State = CustomControls.Web.PanelState.Expanded
                ''cpnlError.Text = "Message"
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Else
            Return False
        End If

    End Function

#End Region

    Private Sub grdReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdReport.ItemDataBound
        Try

            'Dim strID As String
            'Dim strAgNo, strCType, strTaskNo, strTType As String

            For cnt As Integer = 0 To e.Item.Cells.Count


                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    CType(e.Item.FindControl("txtDPC"), TextBox).Attributes.Add("onkeypress", "javascript:NumericDis('" & CType(e.Item.FindControl("txtDPC"), TextBox).ClientID & "')")

                    CType(e.Item.FindControl("txtDPC"), TextBox).Attributes.Add("onblur", "javascript:calculateAmtCall('" & CType(e.Item.FindControl("lblTotAmt"), Label).Text.Trim & "','" & CType(e.Item.FindControl("txtDPC"), TextBox).ClientID & "','" & CType(e.Item.FindControl("txtDisAmt"), TextBox).ClientID & "')")

                End If

            Next
            txtCntRep.Text = mdvtable.Table.Rows.Count

        Catch ex As Exception
            CreateLog("Invoice Details", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

    Private Function SMPTEmail(ByVal Subject As String, ByVal Body As String, ByVal Email As String, Optional ByVal EmailCC As String = "", Optional ByVal Pathattachment As String = Nothing) As String

        Dim msgMail As MailMessage = New MailMessage

        'Dim stremailto As String

        Try

            msgMail.To = Email 'stremailto & "," & Email

            If IsNothing(EmailCC) = False Then msgMail.Cc = EmailCC

            msgMail.From = "wss@ion.com"

            msgMail.Subject = Subject

            msgMail.BodyFormat = MailFormat.Html

            msgMail.Body = Body

            msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", 2)

            msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", "mail.ionsoftnet.com")

            SmtpMail.Send(msgMail)

        Catch ex As Exception

            CreateLog("IONServiceAgent", "SMTPEmail-1469", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        Finally

            SmtpMail.SmtpServer = Nothing

        End Try

    End Function
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class
