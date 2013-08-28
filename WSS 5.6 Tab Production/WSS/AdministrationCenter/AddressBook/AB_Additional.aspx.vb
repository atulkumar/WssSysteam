'*******************************************************************
' Page                   : - AB_Additional
' Purpose                : - Its purpose is to get additional information about user like name, Contact info,                             additional address lines, city, province.
' Tables used            :   T010023

'Date					Author	Amit 					Modification Date					Description
' 30/03/06											    -------------------					Created 
'
' Notes: 
'*******************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Partial Class AdministrationCenter_AddressBook_AB_Additional
    Inherits System.Web.UI.Page

    Dim mintAddressNumber As Integer
    Dim mintSubAddressNo As Integer
    Dim mintID As Integer
    Dim mshFlag As Short

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            '      txtPostalCode.Attributes.Add("onkeypress", "NumericOnly();")
            txtName.Attributes.Add("onkeypress", "CharacterOnly();")
            txtContactPerson.Attributes.Add("onkeypress", "CharacterOnly();")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            ImgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        End If

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1


        'Security Block

        Dim intId As Integer
        txtCSS(Me.Page)
        If Not IsPostBack Then

            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 540
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If

        'End of Security Block




        Dim txthiddenImage As String = Request.Form("txthiddenImage")

        mshFlag = 0
        mintID = CInt(Request.QueryString("ID"))
        mintAddressNumber = Request.QueryString("AddressNo") ' HttpContext.Current.Session("SAddressNumber_AddressBook")    'HttpContext.Current.Session("SAddressKey")
        mintSubAddressNo = Request.QueryString("AddressKey") 'HttpContext.Current.Session("SSubAddressKey")
        'cpnlError.Visible = False

        '*********************************************************************
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            ''cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        UpdateorSave()
                        If mshFlag = 1 Then
                            Exit Select
                        End If
                        Response.Write("<script>window.close();</script>")
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            ' 'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        UpdateorSave()
                    Case "btnClose"
                        Response.Write("<script>window.close();</script>")

                End Select
            Catch ex As Exception
                CreateLog("AB_Additional", "Load-120", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
        '*********************************************************************

        If Not IsPostBack Then
            mintAddressNumber = Request.QueryString("AddressNo") 'CInt(HttpContext.Current.Session("SAddressNumber_AddressBook"))    'CInt(HttpContext.Current.Session("SAddressKey"))
            'cpnlError.Visible = False

            ' Check whether form is called for Edit or New entry

            ' Add new records
            If mintID = -1 Then
                'Security Block

                Dim intId1 As Integer

                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId1 = 540
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId1) = False Then
                        Response.Redirect("../../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId1)
                End If

                'End of Security Block

                Exit Sub
            Else
                'mintSubAddressNo = HttpContext.Current.Session("SSubAddressKey")
                txtName.Enabled = False
                'txtAlias.Enabled = False

                Dim sqrdAB As SqlDataReader
                Dim blnCheck As Boolean

                Try
                    SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                    'SQL.DBTable = ""
                    SQL.DBTracing = False

                    sqrdAB = SQL.Search("AB_Additional", "Load-164", "Select * from T010023 where AA_NU8_Address_Number=" & mintAddressNumber & " and AA_NU8_Address_Sub_Number=" & mintSubAddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

                    If blnCheck = True Then
                        While sqrdAB.Read
                            txtName.Text = sqrdAB.Item("AA_VC36_Name")
                            txtAD_Type.Text = sqrdAB.Item("AA_VC8_AddressType")
                            txtContactPerson.Text = sqrdAB.Item("AA_VC36_Contact_Person")
                            txtStatus.Text = sqrdAB.Item("AA_VC8_Status")
                            txtAddLine1.Text = sqrdAB.Item("AA_VC36_Address_Line_1")
                            txtAddLine2.Text = sqrdAB.Item("AA_VC36_Address_Line_2")
                            txtAddLine3.Text = sqrdAB.Item("AA_VC36_Address_Line_3")
                            txtCity.Text = sqrdAB.Item("AA_VC8_City")
                            txtCountry.Text = sqrdAB.Item("AA_VC8_Country")
                            txtPostalCode.Text = sqrdAB.Item("AA_NU8_Postal_Code")
                            txtProvince.Text = sqrdAB.Item("AA_VC8_Province")
                        End While
                    Else
                        'cpnlError.Text = "Message"
                        lstError.Items.Add("No records found in the Database...")
                        'cpnlError.State = CustomControls.Web.PanelState.Expanded
                    End If
                Catch ex As Exception
                    'cpnlError.Text = "Message"
                    'cpnlError.TitleCSS = "test3"
                    lstError.Items.Add("Server is busy please try later...")
                    'cpnlError.State = CustomControls.Web.PanelState.Expanded
                    CreateLog("AB_Additional", "Load-173", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
                End Try
            End If

        End If

    End Sub

    'Private Sub tbrMain_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim objButton As ToolbarItem = CType(sender, ToolbarItem)

    '    Select Case objButton.ID
    '        Case "btnOk"
    '            UpdateorSave()
    '            If mshFlag = 1 Then
    '                Exit Select
    '            End If
    '            Response.Write("<script>window.close();</script>")
    '        Case "btnSave"
    '            UpdateorSave()
    '        Case "btnClose"
    '            Response.Write("<script>window.close();</script>")
    '    End Select
    'End Sub

    Private Function SaveAAddress(ByVal ColumnName As ArrayList, ByVal RowValue As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        '  SQL.DBTable = "T010023"
        SQL.DBTracing = False

        Try
            If SQL.Save("T010023", "AB_Additional", "SaveAAdress-241", ColumnName, RowValue) = True Then
                ClearTextBox()
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("AB_Additional", "SaveAAddress-240", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return stReturn
        End Try
    End Function

    Private Function UpdateAAddress(ByVal AddressID As String, ByVal ColumnName As ArrayList, ByVal RowValue As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T010023"
        SQL.DBTracing = False

        Try
            If SQL.Update("T010023", "AB_Additional", "UpdateAddress-270", "Select * from T010023 where AA_NU8_Address_Number=" & mintAddressNumber & " and AA_NU8_Address_Sub_Number=" & mintSubAddressNo & "", ColumnName, RowValue) = True Then
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("AB_Additional", "UpdateAAddress-268", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return stReturn
        End Try
    End Function

    Private Sub ClearTextBox()
        txtAD_Type.Text = ""
        txtAddLine1.Text = ""
        txtAddLine2.Text = ""
        txtAddLine3.Text = ""
        'txtAlias.Text = ""
        txtCity.Text = ""
        txtContactPerson.Text = ""
        txtCountry.Text = ""
        txtName.Text = ""
        txtPostalCode.Text = ""
        txtProvince.Text = ""
        txtStatus.Text = ""
    End Sub

    Private Sub UpdateorSave()
        'cpnlError.Visible = False

        lstError.Items.Clear()
        If txtName.Text.Trim.Equals("") Then
            lstError.Items.Add("Name cannot be blank...")
            'txtName.BackColor = Color.Red
            mshFlag = 1
        End If

        If txtAD_Type.Text.Trim.Equals("") Then
            lstError.Items.Add("Address type cannot be blank...")
            'txtAD_Type.BackColor = Color.Red
            mshFlag = 1
        End If

        If txtAddLine1.Text.Trim.Equals("") And txtAddLine2.Text.Trim.Equals("") And txtAddLine3.Text.Trim.Equals("") Then
            lstError.Items.Add("Address cannot be blank... ")
            mshFlag = 1
        End If

        'If Not (txtPostalCode.Text.Trim.Equals("") OrElse txtPostalCode.Text.Trim.Equals("0")) And IsNumeric(txtPostalCode.Text.Trim) = False Then
        '    lstError.Items.Add("Postal Code is not numeric...")
        '    mshFlag = 1
        'End If
        '**Begin
        If txtCity.Text.Trim.Equals("") Then
            lstError.Items.Add("City cannot be blank...")
            mshFlag = 1
        End If
        If txtProvince.Text.Trim.Equals("") Then
            lstError.Items.Add("Province cannot be blank...")
            mshFlag = 1
        End If

        If txtCountry.Text.Trim.Equals("") Then
            lstError.Items.Add("Country cannot be blank...")
            mshFlag = 1
        End If
        '**end





        If mshFlag = 1 Then
            'Image1.ImageUrl = "..\..\images\warning.gif"
            ' MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ''cpnlError.Visible = True
            ''cpnlError.TitleCSS = "test3"
            ' 'cpnlError.State = CustomControls.Web.PanelState.Expanded
            ' 'cpnlError.Text = "Error Summary..."
            'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String

        lstError.Items.Clear()
        For intI As Integer = 0 To 4
            Select Case intI
                Case 0
                    strUDCType = "ADTY"
                    strName = txtAD_Type.Text.Trim.ToUpper
                    strErrorMessage = "Address type Mismatch..."
                Case 1
                    strUDCType = "STA"
                    strName = txtStatus.Text.Trim.ToUpper
                    strErrorMessage = "Status Mismatch..."
                Case 2
                    strUDCType = "CTY"
                    strName = txtCity.Text.Trim.ToUpper
                    strErrorMessage = "City Mismatch..."
                Case 3
                    strUDCType = "PROV"
                    strName = txtProvince.Text.Trim.ToUpper
                    strErrorMessage = "Province Mismatch..."
                Case 4
                    strUDCType = "CNTY"
                    strName = txtCountry.Text.Trim.ToUpper
                    strErrorMessage = "Country Mismatch..."
            End Select

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                mshFlag = 1
            End If
        Next

        If mshFlag = 1 Then
            ' Image1.ImageUrl = "..\..\images\warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgWarning)
            ''cpnlError.Visible = True
            ''cpnlError.Text = "Message..."
            'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            ''cpnlError.State = CustomControls.Web.PanelState.Expanded

            Exit Sub
        End If

        Dim arColumnName As New ArrayList
        Dim arRowvalue As New ArrayList

        If mintID = -1 Then

            Try
                arColumnName.Add("AA_NU8_Address_Number")
                arColumnName.Add("AA_NU8_Address_Sub_Number")
                arColumnName.Add("AA_VC36_Name")
                arColumnName.Add("AA_VC8_Status")
                arColumnName.Add("AA_VC8_AddressType")
                arColumnName.Add("AA_VC36_Address_Line_1")
                arColumnName.Add("AA_VC36_Address_Line_2")
                arColumnName.Add("AA_VC36_Address_Line_3")
                arColumnName.Add("AA_VC8_City")
                arColumnName.Add("AA_VC8_Province")
                arColumnName.Add("AA_NU8_Postal_Code")
                arColumnName.Add("AA_VC8_Country")
                arColumnName.Add("AA_VC36_Contact_Person")

                mintSubAddressNo = SQL.Search("AB_Additional", "UpdateorSave-405", "select max(AA_NU8_Address_Sub_Number) from T010023 where AA_NU8_Address_Number=" & mintAddressNumber & "")
                mintSubAddressNo += 1

                arRowvalue.Add(mintAddressNumber)
                arRowvalue.Add(mintSubAddressNo)
                arRowvalue.Add(txtName.Text.Trim)
                arRowvalue.Add(txtStatus.Text.Trim.ToUpper)
                arRowvalue.Add(txtAD_Type.Text.Trim)
                arRowvalue.Add(txtAddLine1.Text.Trim)
                arRowvalue.Add(txtAddLine2.Text.Trim)
                arRowvalue.Add(txtAddLine3.Text.Trim)
                arRowvalue.Add(txtCity.Text.Trim.ToUpper)
                arRowvalue.Add(txtProvince.Text.Trim.ToUpper)
                arRowvalue.Add(txtPostalCode.Text.Trim)
                arRowvalue.Add(txtCountry.Text.Trim.ToUpper)
                arRowvalue.Add(txtContactPerson.Text.Trim)

                mstGetFunctionValue = SaveAAddress(arColumnName, arRowvalue)

                If mstGetFunctionValue.FunctionExecuted = True Then
                    'lstError.Items.Add("You can add more addresses...")
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)


                Else
                    'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgError)
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

                End If
                arColumnName.Clear()
                arRowvalue.Clear()

            Catch ex As Exception
                'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgError)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                ' 'cpnlError.State = CustomControls.Web.PanelState.Expanded
                CreateLog("AB_Additional", "UpdateorSave-430", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        Else
            Try
                ''arColumnName.Add("AA_NU8_Address_Number")
                ''arColumnName.Add("AA_NU8_Address_Sub_Number")
                arColumnName.Add("AA_VC36_Name")
                arColumnName.Add("AA_VC8_Status")
                arColumnName.Add("AA_VC8_AddressType")
                arColumnName.Add("AA_VC36_Address_Line_1")
                arColumnName.Add("AA_VC36_Address_Line_2")
                arColumnName.Add("AA_VC36_Address_Line_3")
                arColumnName.Add("AA_VC8_City")
                arColumnName.Add("AA_VC8_Province")
                arColumnName.Add("AA_NU8_Postal_Code")
                arColumnName.Add("AA_VC8_Country")
                arColumnName.Add("AA_VC36_Contact_Person")

                ''arRowvalue.Add(mintAddressNumber)
                ''arRowvalue.Add(mintSubAddressNo)
                arRowvalue.Add(txtName.Text.Trim)
                arRowvalue.Add(txtStatus.Text.Trim.ToUpper)
                arRowvalue.Add(txtAD_Type.Text.Trim.ToUpper)
                arRowvalue.Add(txtAddLine1.Text.Trim)
                arRowvalue.Add(txtAddLine2.Text.Trim)
                arRowvalue.Add(txtAddLine3.Text.Trim)
                arRowvalue.Add(txtCity.Text.Trim.ToUpper)
                arRowvalue.Add(txtProvince.Text.Trim.ToUpper)
                arRowvalue.Add(txtPostalCode.Text.Trim)
                arRowvalue.Add(txtCountry.Text.Trim.ToUpper)
                arRowvalue.Add(txtContactPerson.Text.Trim)

                mstGetFunctionValue = UpdateAAddress(mintAddressNumber, arColumnName, arRowvalue)

                If mstGetFunctionValue.FunctionExecuted = True Then
                    'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    ' 'cpnlError.State = CustomControls.Web.PanelState.Expanded
                Else
                    'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgError)
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

                    ' 'cpnlError.State = CustomControls.Web.PanelState.Expanded
                End If

                arColumnName.Clear()
                arRowvalue.Clear()

            Catch ex As Exception
                'ShowMsgPenel('cpnlError, lstError, Image1, mdlMain.MSG.msgError)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

                ' 'cpnlError.State = CustomControls.Web.PanelState.Expanded
                CreateLog("AB_Additional", "UpdateorSave-490", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try

        End If
    End Sub
End Class
