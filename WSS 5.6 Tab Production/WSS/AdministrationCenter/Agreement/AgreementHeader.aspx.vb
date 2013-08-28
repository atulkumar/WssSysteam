'************************************************************************************************************
' Page                 : - AggrementHaeder
' Purpose              : - This form contains various fields to get all information from user about aggrement                           like aggrement type, project to which it will assisgned etc. 
' Tables used          : - T010011, T010043, T080011, T060011, T210011, T080022, T030201

' Date					Author	Jaswinder					Modification Date					Description
' 05/01/06											       -------------------					Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports System.Data

Partial Class AdministrationCenter_Agreement_AgreementHeader
    Inherits System.Web.UI.Page
  

#Region " Page Level variables"

    Private Shared mTextBox() As TextBox
    Private mintAddressKey As Integer ' Will keep Contact key and used to store contact key in session
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

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################


        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            'txtDesc.Attributes.Add("OnKeyPress", "return  MaxLength('" & txtDesc.ClientID & "','450');")
            txtPrice.Attributes.Add("OnKeyPress", "return NumericOnly();")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgbtnSearch.Attributes.Add("onclick", "return CheckLength();")
            txtdesc.Attributes.Add("onmousemove", "ShowToolTip(this,500);")
        End If
        'cpnlError.Visible = False
        lstError.Items.Clear()
        'dtValidUpto.Editable = True
        insertedBy = Session("PropUserID")
        insertedOn = Now.ToShortDateString
        systemBy = GetIP()


        '--Customer----------------------------
        txtCSS(Me.Page, "cpnlAD")
        ViewState("SCompany") = Request.QueryString("SCompany")
        ViewState("SAggID") = Request.QueryString("SAggID")
        If IsPostBack = False Then

            FillNonUDCDropDown(DDLCustomer, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' AND CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ")")

            DDLCustomer.SelectedValue = IIf(ViewState("SCompany") = "", Session("PropCompanyID"), ViewState("SCompany"))
            FillAggCustomDDL()

            Dim strCur As String = SQL.Search("WSSSave", "SaveAgreement-800", "select isnull(PI_VC8_Currency,' ') from T010043 where PI_NU8_Address_No='" & DDLCustomer.SelectedValue.Trim & "'")

            CDDLCur.CDDLSetSelectedItem(IIf(strCur = " " Or strCur Is Nothing, " ", strCur))
        Else
            SetCDDLItems()
        End If




        ''''''---------------------------------------
        'If ViewState("SCompany") = "" Then
        '    DDLCustomer.Enabled = True
        'Else
        '    DDLCustomer.SelectedValue = ViewState("SCompany")
        '    DDLCustomer.Enabled = False
        'End If


        If IsPostBack = False Then
            mintID = Request.QueryString(("ID"))
        End If

        If mintID = -1 Then
            mintID = 0
        Else

        End If


        If Not IsPostBack And mintID = 1 Then
            'cddlcur.Enabled = False
            dtValidFrom.Text = SetDateFormat(Now.ToShortDateString, mdlMain.IsTime.DateOnly)
            dtValidUpto.Text = SetDateFormat(DateTime.Now.AddYears(1).ToShortDateString, mdlMain.IsTime.DateOnly)

        End If


        If IsNothing(Request.QueryString("ID")) = False Then
            mintAddressKey = -1
        Else
            mintAddressKey = Request.QueryString("SAggID") 'CInt(ViewState("SAggID"))

        End If

        Dim hiddenImage As String = Request.Form("txthiddenImage")

        If hiddenImage <> "" Then
            Try
                Select Case hiddenImage
                    Case "Close"
                        Response.Redirect("Agreement_Details.aspx?")
                    Case "Logout"
                        LogoutWSS()

                    Case "Ok"

                        If validateForm() Then
                            'Security Block
                            If imgSave.Enabled = False Or imgSave.Visible = False Then
                                ''cpnlError.Visible = True
                                lstError.Items.Add("You don't have access rights to Save record...")
                                Exit Sub
                            End If
                            'End of Security Block

                            If mintID = 0 Then
                                If updateAgreement() = True And hiddenImage = "Ok" Then
                                    Dim focusScript As String = "<script language='javascript'>" & _
                          "window.parent.closeTab();location.href=""Agreement_Details.aspx?ScrID=63""</script>"

                                    ' Add the JavaScript code to the page.
                                    Page.RegisterStartupScript("FocusScript", focusScript)
                                    'Response.Redirect("Agreement_Details.aspx?")
                                End If
                                Exit Select
                            ElseIf mintID = -1 Or mintID = 1 Then
                                If saveAgreement() = True Then
                                    Dim focusScript As String = "<script language='javascript'>" & _
                          "window.parent.closeTab();location.href=""Agreement_Details.aspx?ScrID=63""</script>"

                                    ' Add the JavaScript code to the page.
                                    Page.RegisterStartupScript("FocusScript", focusScript)
                                    'Response.Redirect("Agreement_Details.aspx?ScrID=63")
                                Else
                                    mintID = 1
                                End If
                            End If
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

                            If mintID = 0 Then
                                If updateAgreement() Then
                                    txthiddenImage.Text = ""
                                End If
                            ElseIf mintID = -1 Or mintID = 1 Then
                                If saveAgreement() = True Then
                                    cpnlAD.Enabled = True
                                    cpnlAD.State = CustomControls.Web.PanelState.Expanded
                                    ViewState("SAggID") = txtAggNo.Text.Trim
                                    txthiddenImage.Text = ""
                                    mintID = 0
                                Else
                                    mintID = 1
                                End If
                            End If
                        End If

                    Case "Delete"
                        txthiddenImage.Text = ""
                    Case "Search"
                        txthiddenImage.Text = "Search"
                        ''cpnlError.Visible = False
                    Case "Reset"
                        cpnlAD.Enabled = False
                        cpnlAD.State = CustomControls.Web.PanelState.Collapsed
                        txthiddenImage.Text = ""
                End Select
            Catch ex As Exception
                CreateLog("CraeteUser", "Load-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            End Try

        Else

        End If


        If Not ViewState("SAggID") = "" Then

            arrtextvalue.Clear()

            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlAD$" & arCol.Item(i)))
            Next

            If IsPostBack And (cddlcall.CDDLGetValue <> "" Or cddllevel.CDDLGetValue <> "" Or txtPrice.Text <> "") Then
                If validateForm() Then
                    updateAgreement()
                    '  'cpnlError.Visible = True
                End If
            End If

            'DisablePanels

            FillDetails()
            FillView()
        Else

        End If


        If txtAggNo.Text.Trim = "" Then
            cpnlAD.Enabled = False
            cpnlAD.State = CustomControls.Web.PanelState.Collapsed
        End If

        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 41
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        'End of Security Block

    End Sub

    Private Function validateForm() As Boolean

        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()


        If cddlAGType.CDDLGetValue.Trim = "" Then
            lstError.Items.Add("Agreement Type cannot be blank...")
            shFlag = 1
        End If

        If CDDLProject.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("SubCategory Name cannot be blank...")
            shFlag = 1
        End If
        If cddlPerson.CDDLGetValue.Trim.Equals("") Then
            lstError.Items.Add("Contact Person cannot be blank...")
            shFlag = 1
        End If

        If dtValidUpto.Text = "" Then
        Else
            If CDate(dtValidFrom.Text) >= CDate(dtValidUpto.Text) Then
                lstError.Items.Add("Valid From date cannot be greater than or equal to Valid To date...")
                shFlag = 1
            End If
        End If

        If txtdesc.Text.Trim.Equals("") Then
            lstError.Items.Add("Description cannot be blank...")
            shFlag = 1
        End If

        If cddlcall.CDDLGetValue <> "" Or cddllevel.CDDLGetValue <> "" Or txtPrice.Text <> "" Then

            If cddlcall.CDDLGetValue.Trim = "" Then
                lstError.Items.Add("Call Type cannot be blank...")
                shFlag = 1
            End If

            If txtPrice.Text.Trim = "" Then
                lstError.Items.Add("Enter Price for Call Type...")
                shFlag = 1
            End If

            If CDDLCur.CDDLGetValue.Trim = "" Then
                lstError.Items.Add("Enter Currency for Price measurement...")
                CDDLCur.Enabled = True
                shFlag = 1
            End If

        End If

        If shFlag = 1 Then
            ''cpnlError.Visible = True
            'ImgError.ImageUrl = "../../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ''cpnlError.TitleCSS = "test3"
            ''cpnlError.State = CustomControls.Web.PanelState.Expanded
            ''cpnlError.Text = "Message"
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Return False
        Else
            Return True
        End If

    End Function

    Sub FillDetails()

        Dim strConnection As String = "Server=ion15;database=newwss;uid=sa;"
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String = String.Empty

        strSql = "select AG_NU8_ID_PK as AggNo,AG_VC8_Cust_Name as CID,comp.CI_VC36_Name as Customer,AG_VC8_Contact_Person as perNo,per.CI_VC36_Name as perName,AG_VC20_Ref as Reference,AG_VC8_Ag_Type as AgreementType,AG_NU9_Project_ID as Project,convert(varchar,AG_DT_Valid_From,101) as ValidFrom,convert(varchar,AG_DT_Valid_To,101) as ValidUpto,AG_VC8_Status as Status,AG_VC200_Desc as Description,AG_VC8_Currency as Currency from T080011,T010011 comp,T010011 per where per.CI_NU8_Address_Number=AG_VC8_Contact_Person and comp.CI_NU8_Address_Number=AG_VC8_Cust_Name and AG_NU8_ID_PK=" & ViewState("SAggID") & " and AG_VC8_Cust_Name=" & ViewState("SCompany")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        ' SQL.DBTable = "T080011"

        Try
            sqrdRecords = SQL.Search("Aggrement_Header", "FillDetails-397", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                DDLCustomer.Enabled = False
                CDDLCur.Enabled = False

                sqrdRecords.Read()

                txtAggNo.Text = IIf(IsDBNull(sqrdRecords.Item("AggNo")), " ", sqrdRecords.Item("AggNo"))
                DDLCustomer.SelectedValue = sqrdRecords.Item("CID")
                '                cddlCustomer.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("CID")), " ", sqrdRecords.Item("CID")), False, sqrdRecords.Item("Customer"))

                txtreference.Text = IIf(IsDBNull(sqrdRecords.Item("Reference")), " ", sqrdRecords.Item("Reference"))

                cddlStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("Status")), " ", sqrdRecords.Item("Status")))

                cddlAGType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("AgreementType")), " ", sqrdRecords.Item("AgreementType")))

                dtValidFrom.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("ValidFrom")), " ", sqrdRecords.Item("ValidFrom")), mdlMain.IsTime.DateOnly)

                If IsDBNull(sqrdRecords.Item("ValidUpto")) Then
                Else
                    dtValidUpto.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("ValidUpto")), " ", sqrdRecords.Item("ValidUpto")), mdlMain.IsTime.DateOnly)
                End If


                cddlPerson.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("perNo")), " ", sqrdRecords.Item("perNo")), False, sqrdRecords.Item("perName"))

                txtdesc.Text = IIf(IsDBNull(sqrdRecords.Item("Description")), " ", sqrdRecords.Item("Description"))

                'txtProject.Text = IIf(IsDBNull(sqrdRecords.Item("Project")), " ", sqrdRecords.Item("Project"))
                CDDLProject.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("Project")), " ", sqrdRecords.Item("Project")), False)
                'strCur = SQL.Search("WSSSave", "SaveAgreement-800", "select isnull(PI_VC8_Currency,' ') from T010043 where PI_NU8_Address_No='" & DDLCustomer.SelectedValue.Trim & "'")

                CDDLCur.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("Currency")), " ", sqrdRecords.Item("Currency")))

                If CDDLCur.CDDLGetValue.trim = "" Then
                    CDDLCur.Enabled = True
                Else
                    ' cddlcur.Enabled = False
                End If

                sqrdRecords.Close()

                'Enable Below Panels
                'Call EnablePanels()
                'Else
                '    'cpnlError.Visible = True
                '    'cpnlError.State = CustomControls.Web.PanelState.Expanded
                '    'cpnlError.Text = "No records"
            End If


        Catch ex As Exception
            ''cpnlError.Visible = True
            ''cpnlError.State = CustomControls.Web.PanelState.Expanded
            ''cpnlError.Text = "Error"
            lstError.Items.Add("server is busy please try Later...")
            'ex.Message.ToString()
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("AG_Main", "FillContact-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Function FillAggCustomDDL()
        Dim strCompanyID As Integer = DDLCustomer.SelectedValue
        'CDDL Currency
        CDDLCur.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CUR' and UDC.Company=" & strCompanyID & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CUR' and UDC.Company=0 Order By Name"
        CDDLCur.CDDLUDC = True
        CDDLCur.CDDLFillDropDown()
        'CDDL Call Type
        cddlcall.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='CALL' and UDC.Company=" & strCompanyID & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='CALL' and UDC.Company=0 Order By Name"
        cddlcall.CDDLUDC = True
        cddlcall.CDDLFillDropDown()
        cddlcall.CDDLType = CustomDDL.DDLType.FastEntry
        'CDDL Task type
        cddlTask.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='TKTY'  and UDC.Company=" & strCompanyID & "  union   Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='TKTY' and UDC.Company=0 Order By Name"
        cddlTask.CDDLUDC = True
        cddlTask.CDDLFillDropDown()
        cddlTask.CDDLType = CustomDDL.DDLType.FastEntry
        'CDDL Level
        cddllevel.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='LEVL' and UDC.Company=" & strCompanyID & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='LEVL' and UDC.Company=0 Order By Name"
        cddllevel.CDDLUDC = True
        cddllevel.CDDLFillDropDown()
        cddllevel.CDDLType = CustomDDL.DDLType.FastEntry
        'CDDL Status
        cddlStatus.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='AGST' and UDC.Company=" & strCompanyID & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='AGST' and UDC.Company=0 Order By Name"
        cddlStatus.CDDLUDC = True
        cddlStatus.CDDLFillDropDown()
        'CDDL Agg type
        cddlAGType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='AGTP' and UDC.Company=" & strCompanyID & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='AGTP' and UDC.Company=0 Order By Name"
        cddlAGType.CDDLUDC = True
        cddlAGType.CDDLFillDropDown()
        'CDDL Person
        cddlPerson.CDDLQuery = " SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (um_in4_company_ab_id=" & strCompanyID & " ) Order By Name"
        cddlPerson.CDDLUDC = False
        cddlPerson.CDDLFillDropDown(10, False)
        'CDDL Project
        CDDLProject.CDDLQuery = "select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & strCompanyID
        CDDLProject.CDDLUDC = False
        CDDLProject.CDDLFillDropDown(10, False)

    End Function
    Private Function SetCDDLItems()
        'CDDLProject.CDDLSetItem()
        'cddlcur.CDDLSetItem()
        'cddlAGType.CDDLSetItem()
        'cddlStatus.CDDLSetItem()
        'cddlPerson.CDDLSetItem()

    End Function


#Region "Save"

    Function saveAgreement() As Boolean

        Dim arrColumns As New ArrayList
        Dim arrRows As New ArrayList
        'Dim intNo As Int64
        Dim shFlag As Short
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim intCallNo As Integer
        Dim ctrlTextBox As Control
        Dim blnCheckValidation As Boolean
        Dim strCur As String

        lstError.Items.Clear()

        If shFlag = 1 Then
            ''cpnlError.Visible = True
            ''cpnlError.State = CustomControls.Web.PanelState.Expanded
            ''cpnlError.Text = "Message..."

            'ImgError.ImageUrl = "../../../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            shFlag = 0
            Exit Function
        End If

        Try

            Dim intAgreement As Integer

            intAgreement = SQL.Search("WSSSave", "SaveAgreement-800", "select isnull(max(AG_NU8_ID_PK),0) from T080011 where AG_VC8_Cust_Name='" & DDLCustomer.SelectedValue.Trim & "'")

            intAgreement += 1
            txtAggNo.Text = intAgreement

            getValues(arrColumns, arrRows)

            mstGetFunctionValue = WSSSave.SaveAgreement(arrColumns, arrRows, "T080011")

            If mstGetFunctionValue.ErrorCode = 0 Then

                'strCur = SQL.Search("WSSSave", "SaveAgreement-800", "select isnull(PI_VC8_Currency,' ') from T010043 where PI_NU8_Address_No='" & DDLCustomer.SelectedValue.Trim & "'")

                'cddlcur.CDDLSetSelectedItem(IIf(strCur = " " Or strCur Is Nothing, " ", strCur))

                If CDDLCur.CDDLGetValue.trim = "" Then
                    CDDLCur.Enabled = True
                Else
                    'cddlcur.Enabled = False
                End If

                lstError.Items.Clear()
                lstError.Items.Add("Record saved successfully...")
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                CDDLCur.Enabled = False
                DDLCustomer.Enabled = False
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If

        Catch ex As Exception
            'ImgError.ImageUrl = "../../../images/error_image.gif"
            ' MessagePanelListStyle(lstError, mdlMain.MSG.msgError)

            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("Agreemant_Main", "SaveAgreement-576", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

    Function getLineValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList) As Boolean

        If Not chkExists(CInt(txtAggNo.Text.Trim), DDLCustomer.SelectedValue.Trim, cddlcall.CDDLGetValue, cddlTask.CDDLGetValue, cddllevel.CDDLGetValue) Then
            arColumnName.Add("AL_NU9_Agr_No")
            arColumnName.Add("AL_VC8_Customer")
            arColumnName.Add("AL_VC8_Call_Type")
            arColumnName.Add("AL_VC8_Task_Type")
            arColumnName.Add("AL_VC8_Skill_Level")
            arColumnName.Add("AL_NU9_Price")
            arColumnName.Add("AL_VC8_Currency")
            arColumnName.Add("AL_VC1_Fix_Hour")


            arRowData.Add(CInt(txtAggNo.Text.Trim))
            arRowData.Add(DDLCustomer.SelectedValue.Trim)
            arRowData.Add(cddlcall.CDDLGetValue)
            arRowData.Add(cddlTask.CDDLGetValue)
            arRowData.Add(cddllevel.CDDLGetValue)
            arRowData.Add(txtPrice.Text.Trim)
            arRowData.Add(CDDLCur.CDDLGetValue)
            arRowData.Add(rblHour.SelectedValue)
            Return True
        Else
            Return False
        End If

    End Function

    Function chkExists(ByVal agNo, ByVal custID, ByVal callType, ByVal taskType, ByVal sklLevel) As Boolean

        Dim slcQry As String
        Dim intLineNo As Integer

        Try
            slcQry = "select AL_NU9_Line_No_PK from T080022 where AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & custID & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='" & taskType & "' and AL_VC8_Skill_Level='" & sklLevel & "'"

            intLineNo = SQL.Search("AggrementHeader", "chkExists-638", slcQry)

            If intLineNo = 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            CreateLog("Agreemant_Main", "chkExists-645", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try


    End Function
    Sub getValues(ByRef arrColumns As ArrayList, ByRef arrRows As ArrayList)

        arrColumns.Add("AG_NU8_ID_PK")
        arrColumns.Add("AG_VC8_Cust_Name")
        arrColumns.Add("AG_VC20_Ref")
        arrColumns.Add("AG_VC8_Ag_Type")
        arrColumns.Add("AG_VC8_Contact_Person")
        arrColumns.Add("AG_NU9_Project_ID")
        arrColumns.Add("AG_DT_Valid_From")
        arrColumns.Add("AG_DT_Valid_To")
        arrColumns.Add("AG_VC200_Desc")
        arrColumns.Add("AG_VC8_Status")
        arrColumns.Add("AG_VC8_Currency")

        arrRows.Add(txtAggNo.Text.Trim)
        arrRows.Add(DDLCustomer.SelectedValue.Trim)
        arrRows.Add(txtreference.Text.Trim)
        arrRows.Add(cddlAGType.CDDLGetValue)
        arrRows.Add(cddlPerson.CDDLGetValue)
        arrRows.Add(CDDLProject.CDDLGetValue.Trim)
        arrRows.Add(dtValidFrom.Text)
        If dtValidUpto.Text = "" Then
            arrRows.Add(DBNull.Value)
        Else
            arrRows.Add(dtValidUpto.Text)
        End If


        arrRows.Add(txtdesc.Text.Trim)
        arrRows.Add(cddlStatus.CDDLGetValue)
        arrRows.Add(CDDLCur.CDDLGetValue)

    End Sub

#End Region

#Region "Update"

    Sub clearDetailText()
        cddlcall.CDDLSetSelectedItem("")
        cddlTask.CDDLSetSelectedItem("")
        cddllevel.CDDLSetSelectedItem("")
        txtPrice.Text = ""
        rblHour.SelectedValue = "H"
    End Sub

    Function updateAgreement() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Dim intRows As Integer

        Try
            getValues(arColumnName, arRowData)
            mstGetFunctionValue = WSSUpdate.UpdateAgreement(txtAggNo.Text.Trim, DDLCustomer.SelectedValue.Trim, arColumnName, arRowData)

            arColumnName.Clear()
            arRowData.Clear()


            If SQL.Search("Aggrement Header", "updateAgreement-702", "select * from T010043 where PI_NU8_Address_No=" & DDLCustomer.SelectedValue.Trim, intRows) Then

                SQL.Update("Aggrement Header", "updateAgreement-704", "update T010043 set PI_VC8_Currency='" & CDDLCur.CDDLGetValue & "' where PI_NU8_Address_No=" & DDLCustomer.SelectedValue.Trim, SQL.Transaction.Serializable)

            Else

                SQL.Save("Aggrement Header", "updateAgreement-708", "insert into T010043(PI_NU8_Address_No,PI_VC8_Currency) values(" & DDLCustomer.SelectedValue.Trim & ",'" & CDDLCur.CDDLGetValue & "')", SQL.Transaction.Serializable)

            End If




            If Not cddlcall.CDDLGetValue = "" Then
                If getLineValues(arColumnName, arRowData) Then
                    clearDetailText()
                    mstGetFunctionValue = WSSSave.SaveAgreement(arColumnName, arRowData, "T080022")
                Else
                    lstError.Items.Clear()
                    ' ImgError.ImageUrl = "../../images/error_image.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    lstError.Items.Add("Agreement Line already exists...")
                    ''cpnlError.Text = "Message"
                    ''cpnlError.TitleCSS = "test3"
                    ''cpnlError.Visible = True
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                    Exit Function
                End If
            End If

            If mstGetFunctionValue.ErrorCode = 0 Then
                ' ImgError.ImageUrl = "../../images/Pok.gif"
                ' MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ' 'cpnlError.Text = "Message"
                ''cpnlError.Visible = True
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ' 'cpnlError.Text = "Message"
                ''cpnlError.TitleCSS = "test3"
                ''cpnlError.Visible = True
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ''cpnlError.Text = "Message"
                ''cpnlError.TitleCSS = "test3"
                ''cpnlError.Visible = True
                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            'ImgError.ImageUrl = "../../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            lstError.Items.Add("server is busy please try Later...")
            ''cpnlError.Text = "Message"
            ''cpnlError.TitleCSS = "test3"
            ''cpnlError.Visible = True
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("AGGHeader", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

#End Region

#Region "fill View"

    Private Function FillView()

        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String

        'SQL.DBTable = "T030201"

        Try

            If SQL.Search("T030201", "Aggrement Header", "FillView-784", "select AL_NU9_Line_No_PK as LinNo,AL_VC8_Call_Type as CallType,AL_VC8_Task_Type as TaskType ,AL_VC8_Skill_Level SkillLevel,AL_NU9_Price as Price,AL_VC1_Fix_Hour ChargeBasis  from T080022,T010011 where AL_VC8_Customer=CI_NU8_Address_Number and AL_NU9_Agr_No=" & txtAggNo.Text.Trim & " and AL_VC8_Customer=" & DDLCustomer.SelectedValue.Trim, dsFromView, "sachin", "Prashar") Then


                mdvtable.Table = dsFromView.Tables("T030201")
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()


                FormatGrid()
                GetColumns()

                'create textbox at run time at head of the datagrid        

                CreateTextBox()

                If cpnlAD.Visible = True Then
                    Call BtnGrdSearch_Click(Me, New EventArgs)
                End If
            Else

                Dim ds As New DataSet
                ds.Tables.Add("Dummy")
                ds.Tables("Dummy").Columns.Add("LinNo")
                ds.Tables("Dummy").Columns.Add("CallType")
                ds.Tables("Dummy").Columns.Add("TaskType")
                ds.Tables("Dummy").Columns.Add("SkillLevel")
                ds.Tables("Dummy").Columns.Add("Price")
                ds.Tables("Dummy").Columns.Add("ChargeBasis")

                Dim dtRow As DataRow

                dtRow = ds.Tables("Dummy").NewRow
                dtRow.Item(0) = ""

                mdvtable.Table = ds.Tables("Dummy")

                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If
                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.DataBind()

                FormatGrid()

            End If

        Catch ex As Exception
            CreateLog("OW Views", "Fill View", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Function CreateTextBox()

        arColumns.Clear()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arCol.Clear()

        arCol.Add("LinNo")
        arCol.Add("CallType")
        arCol.Add("TaskType")
        arCol.Add("SkillLevel")
        arCol.Add("Price")
        arCol.Add("ChargeBasis")


        'fill the columns count into the array from mdvtable view

        intCol = mdvtable.Table.Columns.Count


        ReDim mTextBox(intCol)

        Try
            For intii = 1 To intCol - 1
                _textbox = New TextBox

                Dim col1 As Unit
                Dim col1cng As String
                Dim strcolid As String
                col1 = Unit.Parse(arColWidth.Item(intii))
                col1cng = col1.Value
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
            CreateLog("AGHeader", "CreateTextBox-772", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(0)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(82)
        arColWidth.Add(80)

        Try
            GrdAddSerach.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1
                Dim Bound_Column As New BoundColumn
                If intI = 0 Then
                    Bound_Column.Visible = False
                Else
                    Dim strWidth As String = arColWidth.Item(intI) & "pt"
                    Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                    Bound_Column.ItemStyle.Wrap = True
                End If

                'Bound_Column.HeaderText = arColumnName.Item(intI)
                GrdAddSerach.Columns.Add(Bound_Column)
            Next

        Catch ex As Exception
            CreateLog("AGHeader", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(0)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(82)
        arColWidth.Add(80)



        arColumnName.Add("LinNo")
        arColumnName.Add("CallType")
        arColumnName.Add("TaskType")
        arColumnName.Add("SkillLevel")
        arColumnName.Add("Price")
        arColumnName.Add("ChargeBasis")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()


        arColWidth.Add(0)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(80)
        arColWidth.Add(82)
        arColWidth.Add(80)



        arColumnName.Add("LinNo")
        arColumnName.Add("CallType")
        arColumnName.Add("TaskType")
        arColumnName.Add("SkillLevel")
        arColumnName.Add("Price")
        arColumnName.Add("ChargeBasis")

    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = mTextBox.Length
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not mTextBox(intI).Text.Trim.Equals("") Then
                    strSearch = mTextBox(intI).Text

                    'delibrately put the " * " afetr the text of the search
                    ' strSearch = strSearch + "*"

                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) Then
                            Else
                                'LblErrMsg.Text = " Check Your Date Format First"
                                Exit Sub
                            End If
                        End If

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then

                        End If
                        ' mdvTable.Table.Columns(intI).DataType.FullName = System.DateTime
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mTextBox(intI).Text.Trim
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
                shF = 1
                GrdAddSerach.Columns.Clear()
                If GrdAddSerach.AutoGenerateColumns = False Then
                    GrdAddSerach.AutoGenerateColumns = True
                End If

                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                FormatGrid()
                GetColumns()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.Columns.Clear()
            GrdAddSerach.DataSource = mdvtable
            If GrdAddSerach.AutoGenerateColumns = False Then
                GrdAddSerach.AutoGenerateColumns = True
            End If
            GrdAddSerach.DataBind()
            FormatGrid()
            GetColumns()
        Catch ex As Exception
            CreateLog("AGGHeader", "Click-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub

#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String

        GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strTempName = e.Item.Cells(1).Text
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "','" & rowvalue & "')")

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "', '" & strTempName & "','" & strTempName & "','" & rowvalue & "')")

                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("ViewJobs", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

    Private Sub DDLCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLCustomer.SelectedIndexChanged
        FillAggCustomDDL()
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
