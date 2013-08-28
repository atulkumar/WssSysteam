Imports ION.Data
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Partial Class AdministrationCenter_AddressBook_AB_Skill
    Inherits System.Web.UI.Page

    Dim mintAddressNumber As Integer
    Dim mintSkillNo As Integer
    Dim mshFlag As Short

    Dim txthiddenImage As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Call txtCSS(Me.Page)
        If Not IsPostBack Then

            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        End If
        'mshFlag = 0
        Dim sqrdAB As SqlDataReader
        Dim blnCheck As Boolean

        'mintSkillNo = 1
        ' mintAddressNumber = 1

        mintSkillNo = CInt(Request.QueryString("ID"))
        mintAddressNumber = Request.QueryString("AddressNo") ' HttpContext.Current.Session("SAddressNumber_AddressBook")
        cpnlError.Visible = False

        txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Ok"
                        'Security Block
                        If imgOk.Enabled = False Or imgOk.Visible = False Then
                            ' cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        Call UpdateorSave()
                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        Call UpdateorSave()
                    Case "Close"
                        Response.Write("<script>window.close();</script>")
                End Select
            Catch ex As Exception
                CreateLog("AB_Skill", "Load-100", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If

        If Not IsPostBack Then
            mintAddressNumber = Request.QueryString("AddressNo") 'HttpContext.Current.Session("SAddressNumber_AddressBook")
            'cpnlError.Visible = False
            Try
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '  SQL.DBTable = "T010033"
                SQL.DBTracing = False

                sqrdAB = SQL.Search("AB_Skill", "Load-112", "Select * from T010033 where ST_NU8_Address_Number=" & mintAddressNumber & " and ST_NU8_Skill_Number=" & mintSkillNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

                If blnCheck = True Then
                    sqrdAB.Read()
                    txtSkillType.Text = sqrdAB.Item("ST_VC8_Skill_Type")
                    txtSkill.Text = sqrdAB.Item("ST_VC8_Skill")
                    txtSkillComment.Text = sqrdAB.Item("ST_VC156_Comment")
                Else
                    cpnlError.Text = "Message"
                    lstError.Items.Add("No records found in the Database...")
                    cpnlError.State = CustomControls.Web.PanelState.Expanded
                End If
            Catch ex As Exception
                'Image1.ImageUrl = "..\..\images\error_image.gif"
                'cpnlError.Text = "Message"
                'cpnlError.TitleCSS = "test3"
                lstError.Items.Add("Some Error Occured while transacting...")
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                'cpnlError.State = CustomControls.Web.PanelState.Expanded
                CreateLog("AB_Skill", "Load-130", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If

        Dim intid As Integer

        'Security Block
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then


                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
            'End of Security Block


        End If

    End Sub

    Private Function UpdateSkillSet(ByVal AddressID As String, ByVal ColumnName As ArrayList, ByVal RowValue As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T010033"
        SQL.DBTracing = False

        Try
            If SQL.Update("T010033", "AB_Skill", "UpdateSkillSet-163", "Select * from T010033 where ST_NU8_Address_Number=" & mintAddressNumber & " and ST_NU8_Skill_Number=" & mintSkillNo & "", ColumnName, RowValue) = True Then
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Error occured while saving records..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("AB_Skill", "UpdateSkillSet-196", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return stReturn
        End Try
    End Function

    Private Sub ClearTextBox()
        txtSkillType.Text = ""
        txtSkillType.Text = ""
        txtSkillComment.Text = ""
    End Sub

    Private Sub UpdateorSave()
        Dim strErrorMessage As String
        Dim strUDCType As String
        Dim strName As String
        Dim arColumnName As New ArrayList
        Dim arRowvalue As New ArrayList

        cpnlError.Visible = False
        lstError.Items.Clear()
        If txtSkillType.Text.Trim.Equals("") Then
            lstError.Items.Add("SkillType cannot be blank...")
            mshFlag = 1
        End If

        If txtSkill.Text.Trim.Equals("") Then
            lstError.Items.Add("Skill cannot be blank...")
            mshFlag = 1
        End If

        If mshFlag = 1 Then

            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            'cpnlError.Visible = True
            'cpnlError.TitleCSS = "test3"
            ' cpnlError.State = CustomControls.Web.PanelState.Expanded
            ' cpnlError.Text = "Error Summary"
            ' Image1.ImageUrl = "..\..\icons\warning.gif"


            Exit Sub
        End If

        lstError.Items.Clear()
        For intI As Integer = 0 To 1
            Select Case intI
                Case 0
                    strUDCType = "SKTY"
                    strName = txtSkillType.Text.Trim.ToUpper
                    strErrorMessage = "Skill type Mismatch..."
                Case 1
                    strUDCType = "SKL"
                    strName = txtSkill.Text.Trim.ToUpper
                    strErrorMessage = "SKill Mismatch..."
            End Select

            If CheckUDCValue(0, strUDCType, strName) = False Then
                lstError.Items.Add(strErrorMessage)
                mshFlag = 1
            End If
        Next

        If mshFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Sub
        End If


        Try

            arColumnName.Add("ST_VC8_Skill_Type")
            arColumnName.Add("ST_VC8_Skill")
            arColumnName.Add("ST_VC156_Comment")

            arRowvalue.Add(txtSkillType.Text.Trim)
            arRowvalue.Add(txtSkill.Text.Trim)
            arRowvalue.Add(txtSkillComment.Text.Trim)

            mstGetFunctionValue = UpdateSkillSet(mintAddressNumber, arColumnName, arRowvalue)

            If mstGetFunctionValue.FunctionExecuted = True Then
                'Image1.ImageUrl = "..\..\images\Pok.gif"
                'cpnlError.Visible = True
                'cpnlError.Text = "Message"
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                'cpnlError.State = CustomControls.Web.PanelState.Expanded

                If txthiddenImage = "Ok" Then    'Close window if OK image is pressed
                    Response.Write("<script>window.close();</script>")
                End If

            Else
                'Image1.ImageUrl = "..\..\icons\warning.gif"
                'cpnlError.Visible = True
                'cpnlError.Text = "Error"
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgWarning)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                'cpnlError.State = CustomControls.Web.PanelState.Expanded
            End If

            arColumnName.Clear()
            arRowvalue.Clear()

        Catch ex As Exception
            'Image1.ImageUrl = "..\..\images\error_image.gif"
            'cpnlError.Visible = True
            'cpnlError.Text = "Error"
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            ' cpnlError.State = CustomControls.Web.PanelState.Expanded
            CreateLog("AB_Skill", "UpdateorSave-313", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

        '  End If
    End Sub
End Class
