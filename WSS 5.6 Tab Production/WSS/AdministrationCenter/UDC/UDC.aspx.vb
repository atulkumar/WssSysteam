'*******************************************************************
' Page                 : - UDC
' Purpose              : - It will validate the Machine code.
' Date				   Author						Modification Date					Description
' 16/03/06			   Amit							-------------------					Created
'
' Notes: The string created will request the WebService to look for processes for the initatior
' Code:
'*******************************************************************
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports ION.Data
Imports Microsoft.Web.UI.WebControls
Imports System.Data


'''''''''''''''''ViewState Variables Used on this page are:'''''''''''''''''
'ViewState("WhichGrid")
'ViewState("SProductCodeU")
'ViewState("SUDCTypeF")
'ViewState("SName") 
'ViewState("SUDCCompany")
'ViewState("SUDCDescription")
'ViewState("SUDCTypePC")
'ViewState("SUDCTypeP") 
'ViewState("SUDCTypeCompany") 
'ViewState("SUDCTypeText") 
''''''''''''''''''''''''''''
Partial Class AdministrationCenter_UDC_UDC
    Inherits System.Web.UI.Page

#Region " Variable Section "

    ' This enum will tell which grid row is clicked
    Private Enum WhichGrid
        UDCType = 1
        UDC = 2
    End Enum

    ' Holding text boxes created above UDC Type Grid
    Dim mtxtUDCTypeQuery As TextBox()
    ' Holding text boxes created above UDC Grid
    Dim mtxtUDCQuery As TextBox()

    ' For Storing Column name of UDCType table.
    Private Shared marUDCTypeTextBoxID As New ArrayList
    ' For Storing Textbox value above UDCType Grid.
    Private Shared marUDCTypeTextBoxValue As New ArrayList

    ' For Storing Column name of UDC table.
    Private Shared marUDCTextBoxID As New ArrayList
    ' For Storing Textbox value above UDC Grid.
    Private Shared marUDCTextBoxValue As New ArrayList

    ' Dataview used for filtering UDCType table
    Private mdvUDCType As New DataView
    ' Dataview used for filtering UDCT table
    Private mdvUDC As New DataView

    ' Dataset holding UDC Type records
    Private mdsUDCType As New DataSet
    ' Dataset holding UDC records
    Private mdsUDC As New DataSet

    ' This will hold the UDC Type Product Code
    Private Shared mintUDCTypePC As Integer
    ' This will hold the UDC Type
    Private Shared mstrUDCTypeP As String

    ' This will hold the UDC Product Code
    Private Shared mintUDCPC As Integer
    ' This will hold the UDC Type in UDC
    Private Shared mstrUDCF As String
    ' This will hold the UDC Name
    Private Shared mstrUDCName As String
    ' For making sure that textboxes are created on the UDC grid first time 
    Private Shared mblnCreateTextBox As Boolean
    ' To make sure that textbox are not created second time when UDC Type row is clicked
    Private Shared mshTxtBoxCreated As Short = 0
    ' Name of the company in UDC Table
    Private mstrUDCCompany As String

    ' Name of the company in UDCType Table
    Private mstrUDCTCompany As String

    ' Flag to check the UDC Type Lock
    Private Shared mblnLocked As Boolean
    Dim inttxtvalueResult As Integer
    Private Shared mblnUDCPostBack As Boolean = False
    Private Shared menGrid As WhichGrid
    Dim dltsat As Short = 0 'chkdelete status

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim intID As Int16
        intID = Request.QueryString("ScrID")
        Dim obj As New clsSecurityCache
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
            txtProductCode.Attributes.Add("onkeypress", "NumericOnly();")
        End If
        txtSelectHID.Value = ""

        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Dim txthiddenvalue = Request.Form("txthidden")
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBTracing = False
        lstError.Items.Clear()
        '--UDCCompany
        CDDLUDCCompany.CDDLQuery = "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "
        CDDLUDCCompany.CDDLMandatoryField = True
        CDDLUDCCompany.CDDLUDC = False
        '-----------------------------------------
        '--UDCTypeCompany
        CDDLUDCTypeCompany.CDDLQuery = "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "

        CDDLUDCTypeCompany.CDDLMandatoryField = True
        CDDLUDCTypeCompany.CDDLUDC = False
        '-----------------------------------------
        Call txtCSS(Me.Page, "cpnlUDCType", "cpnlUDC")
        If IsPostBack = False Then

            CDDLUDCCompany.CDDLFillDropDown(10, False)
            CDDLUDCTypeCompany.CDDLFillDropDown(10, False)
        Else
            CDDLUDCCompany.CDDLSetItem()
            CDDLUDCTypeCompany.CDDLSetItem()
        End If

        'cpnlErrorPanel.Visible = False
        GetUDCtype()

        CDDLUDCTypeCompany.Enabled = False ' disabled udc company dropdown control



        If txthiddenvalue <> "" Then
            Try
                Select Case txthiddenvalue
                    Case "Edit"
                        '******************************************
                        Select Case menGrid
                            Case WhichGrid.UDC
                                Dim mintProductCode As String = ViewState("SProductCodeU") 'HttpContext.Current.Session("SProductCodeU")
                                Dim mstrUDCType As String = ViewState("SUDCTypeF") 'HttpContext.Current.Session("SUDCTypeF")
                                Dim mstrUDCName As String = ViewState("SName") 'HttpContext.Current.Session("SName")

                                Response.Write("<script>window.open('UDC_Edit.aspx?UDCCompany=" + ViewState("UDCCompany") + "&Code=" + mintProductCode + "&Type=" + mstrUDCType + "&Name=" + mstrUDCName + "', 'fg', 'top='+ (screen.height - 596) / 2 +',left='+ (screen.width - 532) / 2 +',scrollBars=no,resizable=No,width=500,height=300,status=yes');</script>")

                            Case WhichGrid.UDCType

                                If mblnLocked = True Then
                                    lstError.Items.Add("UDC Type  is locked so it cannot be Edited...")
                                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                                    Exit Select
                                Else
                                    Dim mintProductCode1 As String = ViewState("SUDCTypePC") 'HttpContext.Current.Session("SProductCodeU")
                                    Dim mstrUDCType2 As String = ViewState("SUDCTypeP") 'HttpContext.Current.Session("SUDCTypeF")
                                    Dim strCompany As String = ViewState("SUDCCompany")
                                    Response.Write("<script> window.open('UDCType_Edit.aspx?Code=" + mintProductCode1 + "&Type=" + mstrUDCType2 + "&Company=" + strCompany + "', 'fg', 'top='+ (screen.height - 596) / 2 +',left='+ (screen.width - 532) / 2 +',scrollBars=no,resizable=No,width=500,height=300,status=yes');</script>")
                                End If

                        End Select
                        '******************************************
                    Case "Save"
                        '******************************************
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            ' cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            Exit Sub
                        End If
                        'End of Security Block

                        If txtProductCode.Text.Equals("") And txtUDCTypeP.Text.Equals("") Then
                        Else
                            SaveUDCType()
                            ' Define the JavaScript function for the specified control.
                            Dim focusScript As String = "<script language='javascript'>" & _
                              "document.getElementById('cpnlUDCType_txtProductCode').focus();</script>"

                            ' Add the JavaScript code to the page.
                            Page.RegisterStartupScript("FocusScript", focusScript)

                        End If

                        If txtProductCodeUDC.Text.Equals("") And txtUDCTypeF.Text.Equals("") And txtName.Text.Equals("") Then
                        Else
                            SaveUDC()

                            ' Define the JavaScript function for the specified control.
                            Dim focusScript As String = "<script language='javascript'>" & _
                              "document.getElementById('cpnlUDC_txtName').focus();</script>"

                            ' Add the JavaScript code to the page.
                            Page.RegisterStartupScript("FocusScript", focusScript)

                            '  Response.Write("<script> javascript:document.Form1.txtName.focus();</script>")
                        End If

                        '******************************************
                    Case "Delete"
                        '******************************************
                        Select Case menGrid
                            Case WhichGrid.UDC
                                ' Check for sapce
                                If mstrUDCName = "&nbsp;" Then
                                    mstrUDCName = " "
                                End If

                                mstGetFunctionValue = WSSDelete.DeleteUDC(mintUDCPC, mstrUDCF, mstrUDCName)
                                If mstGetFunctionValue.FunctionExecuted = True Then
                                    grdUDC.SelectedIndex = -1
                                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                    GetUDC()
                                Else
                                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgInfo)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                                End If
                            Case WhichGrid.UDCType
                                If mblnLocked = True Then
                                    lstError.Items.Add("UDC Type  is locked so UDC cannot be deleted...")
                                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                                    Exit Select
                                End If

                                mstGetFunctionValue = WSSDelete.DeleteUDCType(mintUDCTypePC, mstrUDCTypeP)

                                If mstGetFunctionValue.FunctionExecuted = True Then
                                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)

                                    ' ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgOK)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)


                                    GetUDCtype()

                                    txtUDCTypeF.Text = ""
                                    txtProductCodeUDC.Text = ""
                                    mstrUDCTypeP = "0"
                                    mintUDCTypePC = 0
                                    dltsat = 1

                                    GetUDC()
                                Else
                                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgError)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If

                        End Select
                End Select

                '******************************************




            Catch ex As Exception
                'Image1.ImageUrl = "../../Images/error_image.gif"
                ' MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("UDC", "Load-302", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Logout"
                    LogoutWSS()
            End Select
        End If

        ' This arraylist will hold the Column name of UDC Type table that will be displayed in the grid
        Dim arUDCTypeColumn As New ArrayList

        arUDCTypeColumn.Add("ProductCode")
        arUDCTypeColumn.Add("UDCType")
        arUDCTypeColumn.Add("UDCTypeText")
        arUDCTypeColumn.Add("Company")
        arUDCTypeColumn.Add("UDCParam")

        If IsPostBack = True Then

            Try
                Dim arrtextvalueold As ArrayList
                arrtextvalueold = marUDCTypeTextBoxValue.Clone
                Dim txtoldvalue() As Object = arrtextvalueold.ToArray()
                Dim stroldvalue As String = Join(txtoldvalue, "~")


                ' Fill UDC Type grid
                marUDCTypeTextBoxValue.Clear()
                ' Check the value of each text Box over UDCType grid
                For i As Integer = 0 To marUDCTypeTextBoxID.Count - 1
                    marUDCTypeTextBoxValue.Add(Request.Form("cpnlUDCType$" & marUDCTypeTextBoxID.Item(i)))
                Next

                Dim txtnewvalue() As Object = marUDCTypeTextBoxValue.ToArray()
                Dim strnewvalue As String = Join(txtnewvalue, "~")
                '**************************************
                inttxtvalueResult = StrComp(strnewvalue, stroldvalue)

                mtxtUDCTypeQuery = CreateTextBox(Me, True, pnlUDCTypeTxtbox, grdUDCType, arUDCTypeColumn, marUDCTypeTextBoxID, marUDCTypeTextBoxValue)
                'GetUDCtype()
                UDCTypeQuery()

                ' Fill UDC grid
                If dltsat = 1 Then
                    cpnlUDC.State = CustomControls.Web.PanelState.Collapsed
                    cpnlUDC.Enabled = False
                    cpnlUDC.TitleCSS = "test2"
                    cpnlUDC.Text = "UDC"
                Else
                    cpnlUDC.State = CustomControls.Web.PanelState.Expanded
                    cpnlUDC.Enabled = False
                    cpnlUDC.TitleCSS = "test"
                End If


                If mblnUDCPostBack = False Then
                    Exit Try
                End If

                If marUDCTypeTextBoxValue.Count > 0 Then
                    cpnlUDC.State = CustomControls.Web.PanelState.Collapsed
                    cpnlUDC.Enabled = False
                    cpnlUDC.TitleCSS = "test2 "
                    'CType(grdUDCType.FindControl("select"), Button).Text()
                End If

                marUDCTextBoxValue.Clear()
                ' Check the value of each text Box over UDC grid
                For i As Integer = 0 To marUDCTextBoxID.Count - 1
                    marUDCTextBoxValue.Add(Request.Form("cpnlUDC$" & marUDCTextBoxID.Item(i)))
                Next

                ' This arraylist will hold the Column name of UDC table that will be displayed in the grid
                Dim arUDCColumn As New ArrayList

                arUDCColumn.Add("ProductCode")
                arUDCColumn.Add("UDCType")
                arUDCColumn.Add("Name")
                arUDCColumn.Add("Description")
                arUDCColumn.Add("Company")

                ' Create textboxes over UDC grid
5:              mtxtUDCQuery = CreateTextBox(Me, True, pnlUDC, grdUDC, arUDCColumn, marUDCTextBoxID, marUDCTextBoxValue)
                arUDCColumn.Clear()
                GetUDC()
                'mshOpenUDCPanel = 2
                UDCQuery()

                If inttxtvalueResult.Equals(-1) Then
                    cpnlUDC.State = CustomControls.Web.PanelState.Collapsed
                    cpnlUDC.Enabled = False
                    cpnlUDC.TitleCSS = "test2"
                Else
                    cpnlUDC.State = CustomControls.Web.PanelState.Expanded
                    cpnlUDC.Enabled = True
                    cpnlUDC.TitleCSS = "test"
                End If
            Catch ex As Exception
                ' Image1.ImageUrl = "../../Images/error_image.gif"
                ' MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                CreateLog("UDC", "Load-358", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        Else
            'Create text boxes over UDCType grid
            mtxtUDCTypeQuery = CreateTextBox(Me, False, pnlUDCTypeTxtbox, grdUDCType, arUDCTypeColumn, marUDCTypeTextBoxID, marUDCTypeTextBoxValue)
            'imgbtnRight.Attributes.Add("onclick", "ShowContents()")
            'imgbtnLeft.Attributes.Add("onclick", "HideContents()")

            cpnlUDC.State = CustomControls.Web.PanelState.Collapsed
            cpnlUDC.Enabled = False
            cpnlUDC.TitleCSS = "test2"

        End If

        arUDCTypeColumn.Clear()
        If IsPostBack Then
            ViewState("WhichGrid") = menGrid
        Else
            ViewState("WhichGrid") = ""
        End If

        If dltsat = 1 Then
            cpnlUDC.State = CustomControls.Web.PanelState.Collapsed
            cpnlUDC.Enabled = False
            cpnlUDC.TitleCSS = "test2"
            cpnlUDC.Text = "UDC"
        Else
            'cpnlUDC.State = CustomControls.Web.PanelState.Expanded
            'cpnlUDC.Enabled = False
            'cpnlUDC.TitleCSS = "test"
        End If



        'Security Block

        Dim intID1 As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID1 = Request.QueryString("ScrID")
            Dim obj1 As New clsSecurityCache
            If obj1.ScreenAccess(intID1) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj1.ControlSecurity(Me.Page, intID1)
        End If

        'End of Security Block

    End Sub

    Private Sub imgbtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnSearch.Click
        If txtProductCode.Text = "" And txtUDCTypeP.Text = "" Then
        Else
            SaveUDCType()

            ' Define the JavaScript function for the specified control.
            Dim focusScript As String = "<script language='javascript'>" & _
              "document.getElementById('cpnlUDCType_txtProductCode').focus();</script>"

            ' Add the JavaScript code to the page.
            Page.RegisterStartupScript("FocusScript", focusScript)

        End If

        If txtName.Text.Equals("") And txtDescription.Text.Equals("") And CDDLUDCCompany.CDDLGetValue = "" Then

        Else
            SaveUDC()

            ' Define the JavaScript function for the specified control.
            Dim focusScript As String = "<script language='javascript'>" & _
              "document.getElementById('cpnlUDC_txtName').focus();</script>"

            ' Add the JavaScript code to the page.
            Page.RegisterStartupScript("FocusScript", focusScript)

        End If

    End Sub

    Private Sub grdUDCType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUDCType.ItemDataBound
        ' This will change the UDCType grid row color on row click
        Dim itemType As ListItemType = e.Item.ItemType

        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
            e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        End If

    End Sub

    Private Sub grdUDC_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUDC.ItemDataBound
        ' This will change the UDC grid row color on row click
        Dim itemType As ListItemType = e.Item.ItemType

        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
            e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        End If

    End Sub

    Private Sub grdUDC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdUDC.SelectedIndexChanged
        cpnlUDC.State = CustomControls.Web.PanelState.Expanded
        cpnlUDC.Enabled = True
        cpnlUDC.TitleCSS = "test"


        txtSelectHID.Value = "selected"
        mintUDCPC = grdUDC.SelectedItem.Cells(1).Text.Trim
        mstrUDCF = grdUDC.SelectedItem.Cells(2).Text.Trim
        mstrUDCName = grdUDC.SelectedItem.Cells(3).Text.Trim
        mstrUDCCompany = grdUDC.SelectedItem.Cells(5).Text.Trim

        ViewState("SProductCodeU") = mintUDCPC
        ViewState("SUDCTypeF") = mstrUDCF
        ViewState("SName") = mstrUDCName
        ViewState("SUDCCompany") = mstrUDCCompany
        ViewState("SUDCDescription") = grdUDC.SelectedItem.Cells(4).Text.Trim
        menGrid = WhichGrid.UDC
    End Sub

    Private Sub grdUDCType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdUDCType.SelectedIndexChanged
        'cpnlUDC.State = CustomControls.Web.PanelState.Expanded
        'cpnlUDC.Enabled = True
        'cpnlUDC.TitleCSS = "test"
        ' Get the Product Code and UDCType
        Try
            txtSelectHID.Value = "selected"
            mintUDCTypePC = grdUDCType.SelectedItem.Cells(1).Text.Trim
            mstrUDCTypeP = grdUDCType.SelectedItem.Cells(2).Text.Trim
            mstrUDCTCompany = grdUDCType.SelectedItem.Cells(4).Text.Trim
        Catch ex As Exception
            CreateLog("UDC", "SelectedIndexChanged-453", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")
        End Try

        If grdUDCType.SelectedItem.Cells(5).Text = 0 Then
            mblnLocked = False
        Else
            mblnLocked = True
        End If

        txtProductCodeUDC.Text = mintUDCTypePC
        txtUDCTypeF.Text = mstrUDCTypeP

        ViewState("SUDCTypePC") = mintUDCTypePC
        ViewState("SUDCTypeP") = mstrUDCTypeP
        ViewState("SUDCTypeCompany") = mstrUDCTCompany
        ViewState("SUDCTypeText") = grdUDCType.SelectedItem.Cells(3).Text.Trim

        If grdUDCType.SelectedItem.Cells(5).Text = "1" Then
            HttpContext.Current.Session("SUDCTLock") = 1
        Else
            HttpContext.Current.Session("SUDCTLock") = 0
        End If


        If mshTxtBoxCreated = 0 Then
            mblnCreateTextBox = True
        End If
        menGrid = WhichGrid.UDCType

        GetUDC()
    End Sub

    'Private Sub imgbtnLeft_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnLeft.Click
    '    imgbtnLeft.Attributes.Add("onclick", "HideContents()")
    '    imgbtnLeft.Visible = False
    '    imgbtnRight.Visible = True
    'End Sub

    'Private Sub imgbtnRight_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnRight.Click
    '    imgbtnRight.Attributes.Add("onclick", "ShowContents()")
    '    imgbtnLeft.Visible = True
    '    imgbtnRight.Visible = False
    'End Sub

#Region "Private Sub and Function "

    Private Sub GetUDCtype()
        'SQL.DBTable = "UDCType"

        Try
            mdsUDCType.Clear()

            If SQL.Search("UDCType", "UDC", "GetUDCType-501", "select ProductCode,UDCType,UDCTypeText,CI_VC36_Name as Company,UDCParam from UDCType,T010011 where company*= CI_NU8_Address_Number", mdsUDCType, "", "") = True Then
                'chkUDCParam.Checked = False
                grdUDCType.DataSource = mdsUDCType.Tables("UDCType")
                mdvUDCType.Table = mdsUDCType.Tables("UDCType")
                grdUDCType.DataBind()
            End If

        Catch ex As Exception
            CreateLog("UDC", "GetUDCType-513", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Sub GetUDC()
        '   SQL.DBTable = "UDC"

        Try
            mdsUDC.Clear()

            If SQL.Search("UDC", "UDC", "GetUDC-520", "select ProductCode,UDCType,Name,Description,CI_VC36_Name as Company from UDC,T010011 where company*=CI_NU8_Address_Number and  productcode=" & mintUDCTypePC & " and UDCType='" & mstrUDCTypeP & "'", mdsUDC, "", "") = True Then
                cpnlUDC.State = CustomControls.Web.PanelState.Expanded
                cpnlUDC.Enabled = True
                cpnlUDC.Text = "UDC"

                mdvUDC.Table = mdsUDC.Tables("UDC")
                grdUDC.DataSource = mdsUDC.Tables("UDC")
                grdUDC.DataBind()

                ' cpnlUDC.TitleCSS = "test"

                ' This arraylist will hold the Column name of UDC table that will be displayed in the grid
                Dim arUDCColumnName As ArrayList

                If mblnCreateTextBox = True Then
                    arUDCColumnName = New ArrayList

                    arUDCColumnName.Add("ProductCode")
                    arUDCColumnName.Add("UDCType")
                    arUDCColumnName.Add("Name")
                    arUDCColumnName.Add("Description")
                    arUDCColumnName.Add("Company")

                    mtxtUDCQuery = CreateTextBox(Me, False, pnlUDC, grdUDC, arUDCColumnName, marUDCTextBoxID, marUDCTextBoxValue)

                    arUDCColumnName.Clear()
                    mblnCreateTextBox = False
                    ' Making sure that text box are not created again
                    mshTxtBoxCreated = 1
                    mblnUDCPostBack = True
                End If
            Else
                cpnlUDC.TitleCSS = "test2"
                cpnlUDC.State = CustomControls.Web.PanelState.Expanded
                ' cpnlUDC.Text = "UDC - No data found..."
                grdUDC.DataSource = mdsUDC.Tables("UDC")
                grdUDC.DataBind()
            End If
        Catch ex As Exception
            CreateLog("UDC", "GetUDC-562", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
    Private Sub UDCTypeQuery()
        Dim strRowFilterString As String
        Dim strSearch As String
        Try
            For intI As Integer = 0 To marUDCTypeTextBoxID.Count - 1
                ' Check for the values in the textboxes
                If Not mtxtUDCTypeQuery(intI).Text.Trim.Equals("") Then
                    strSearch = mtxtUDCTypeQuery(intI).Text
                    If (mdvUDCType.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvUDCType.Table.Columns(intI).DataType.FullName = "System.Int32") Then
                        strSearch = strSearch.Replace("*", "")
                        If IsNumeric(strSearch) = False Then
                            strSearch = "-101"
                        End If
                        If mdvUDCType.Table.Columns(intI).DataType.FullName = "System.DateTime" Then
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
                            End If
                        End If
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvUDCType.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mtxtUDCTypeQuery(intI).Text.Trim
                        strSearch = GetSearchString(strSearch)
                        If strSearch.Contains("*") = True Then
                            strSearch = strSearch.Replace("*", "%")
                        Else
                            strSearch &= "%"
                        End If
                        strRowFilterString = strRowFilterString & mdvUDCType.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

        Catch ex As Exception
            CreateLog("UDC", "UDCTypeQuery-582", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        ' if filter string is empty then fill the UDC grid with all the data
        If (strRowFilterString Is Nothing) Then
            GetUDCtype()
            Exit Sub
        End If
        ' Remove the  and from the end of the string
        strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
        ' Apply the filter
        mdvUDCType.RowFilter = strRowFilterString
        grdUDCType.DataSource = mdvUDCType
        grdUDCType.DataBind()
    End Sub

    Private Sub UDCQuery()
        Dim strRowFilterString As String
        Dim strSearch As String
        Try
            For intI As Integer = 0 To marUDCTextBoxID.Count - 1
                If Not mtxtUDCQuery(intI).Text.Trim.Equals("") Then
                    strSearch = mtxtUDCQuery(intI).Text
                    If (mdvUDC.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvUDC.Table.Columns(intI).DataType.FullName = "System.Int32") Then
                        strSearch = strSearch.Replace("*", "")
                        If IsNumeric(strSearch) = False Then
                            strSearch = "-101"
                        End If
                        If mdvUDC.Table.Columns(intI).DataType.FullName = "System.DateTime" Then
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
                            End If
                        End If
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvUDCType.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mtxtUDCQuery(intI).Text.Trim
                        strSearch = GetSearchString(strSearch)
                        If strSearch.Contains("*") = True Then
                            strSearch = strSearch.Replace("*", "%")
                        Else
                            strSearch &= "%"
                        End If
                        strRowFilterString = strRowFilterString & mdvUDC.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If

            Next

        Catch ex As Exception
            CreateLog("UDC", "UDCQuery-636", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        ' if filter string is empty then fill the UDC grid with all the data
        If (strRowFilterString Is Nothing) Then
            GetUDC()
            Exit Sub
        End If

        ' Remove the  and from the end of the string
        strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
        ' Apply the filter
        mdvUDC.RowFilter = strRowFilterString

        grdUDC.DataSource = mdvUDC
        grdUDC.DataBind()
    End Sub
    Public Sub SaveUDCType()
        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            lstError.Items.Clear()
            'cpnlErrorPanel.Text = "Message..."
            lstError.Items.Add("Your Role does not have rights to save UDC...")
            ' mshFlag = 0
            'ShowMsgPenel('cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Sub
        End If
        'End of Security Block
        Dim shError As Short
        If txtProductCode.Text.Trim.Equals("") Then
            lstError.Items.Add("Product Code cannot be blank...")
            shError = 1
        ElseIf IsNumeric(txtProductCode.Text.Trim) = False Then
            lstError.Items.Add("Product Code is not numeric...")
            shError = 1
        End If

        If txtUDCTypeP.Text.Trim.Equals("") Then
            lstError.Items.Add("Type in a UDCType...")
            shError = 1
        End If

        If txtUDCTypeText.Text.Trim.Equals("") Then
            lstError.Items.Add("Type in a UDCType Text...")
            shError = 1
        End If

        If shError = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        'cpnlErrorPanel.Visible = False
        mstGetFunctionValue = WSSSearch.SearchUDCType(txtProductCode.Text.Trim, txtUDCTypeP.Text.Trim)
        If mstGetFunctionValue.FunctionExecuted = True Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        Dim arColumnName As New ArrayList
        Dim arRowValue As New ArrayList

        Try
            arColumnName.Add("ProductCode")
            arColumnName.Add("UDCType")
            arColumnName.Add("UDCTypeText")
            arColumnName.Add("UDCParam")
            arColumnName.Add("Company")

            arRowValue.Add(Val(txtProductCode.Text.Trim))
            arRowValue.Add(txtUDCTypeP.Text.Trim.ToUpper)
            arRowValue.Add(txtUDCTypeText.Text.Trim)
            If chkUDCParam.Checked = True Then
                arRowValue.Add(1)
            Else
                arRowValue.Add(0)
            End If
            If CDDLUDCTypeCompany.CDDLGetValue.trim = "" Then
                arRowValue.Add(0)
            Else
                arRowValue.Add(CDDLUDCTypeCompany.CDDLGetValue.Trim)
            End If
            chkUDCParam.Checked = False
            mstGetFunctionValue = WSSSave.SaveUDCType(arColumnName, arRowValue)
            If mstGetFunctionValue.FunctionExecuted = False Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Else
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                GetUDCtype()
                txtProductCode.Text = ""
                txtUDCTypeP.Text = ""
                txtUDCTypeText.Text = ""
                CDDLUDCTypeCompany.CDDLSetBlank()
                CDDLUDCTypeCompany.CDDLSetBlank()
                chkUDCParam.Checked = False
            End If
        Catch ex As Exception
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("UDC", "SaveUDCType-740", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub SaveUDC()
        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            lstError.Items.Clear()
            'cpnlErrorPanel.Text = "Message..."
            lstError.Items.Add("Your Role does not have rights to save UDC...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Sub
        End If
        'End of Security Block
        Dim shError As Short
        If Not txtName.Text.Trim.Equals("") Or Not txtDescription.Text.Trim.Equals("") Or CDDLUDCCompany.CDDLGetValue <> "" Then
            If txtUDCTypeF.Text = "" Then
                lstError.Items.Add("UDC Type cannot be blank...")
                shError = 1
            ElseIf txtProductCodeUDC.Text = "" Then
                lstError.Items.Add("Product Code cannot be blank...")
                shError = 1
            ElseIf txtName.Text = "" Then
                lstError.Items.Add("Name cannot be blank...")
                shError = 1
            ElseIf txtDescription.Text = "" Then
                lstError.Items.Add("Description cannot be blank...")
                shError = 1
            End If
        Else
            Exit Sub
        End If
        If shError = 1 Then
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        'cpnlErrorPanel.Visible = False
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Try
            mstGetFunctionValue = WSSSearch.SearchUDC(txtProductCodeUDC.Text.Trim, txtUDCTypeF.Text.Trim, txtName.Text.Trim)
            If mstGetFunctionValue.FunctionExecuted = True Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Exit Sub
            Else
                Dim arColumnName As New ArrayList
                Dim arRowValue As New ArrayList

                arColumnName.Add("ProductCode")
                arColumnName.Add("UDCType")
                arColumnName.Add("Name")
                arColumnName.Add("Description")
                arColumnName.Add("Company")

                arRowValue.Add(txtProductCodeUDC.Text.Trim)
                arRowValue.Add(txtUDCTypeF.Text.Trim)
                arRowValue.Add(txtName.Text.Trim.ToUpper)
                arRowValue.Add(txtDescription.Text.Trim)
                If CDDLUDCCompany.CDDLGetValue.Trim = "" Then
                    arRowValue.Add(0)
                Else
                    arRowValue.Add(CDDLUDCCompany.CDDLGetValue.Trim)
                End If
                ' Save the record in the table
                mstGetFunctionValue = WSSSave.SaveUDC(arColumnName, arRowValue)
                If mstGetFunctionValue.FunctionExecuted = False Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Exit Sub
                Else
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    txtDescription.Text = ""
                    txtName.Text = ""
                    GetUDC()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("UDC", "SaveUDC-824", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
