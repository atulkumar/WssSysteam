#Region "Namespace"
Imports System.Data.SqlClient
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
#End Region

#Region "Session Used"
'Session("PropUserID")
'Session("PropUserName")
'Session("PropRootDir")
#End Region

Partial Class Security_Role_edit
    Inherits System.Web.UI.Page
#Region "Varibles"
    Public Shared codeID As String
#End Region

#Region "Page load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        'Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        txtAssByName.Text = WSSSearch.SearchUserID(HttpContext.Current.Session("PropUserID")).ExtraValue
        txtAssBy.Text = HttpContext.Current.Session("PropUserID")
        codeID = Request.QueryString("codeID")
        'dtAssignDate.readOnlyDate = False

        If IsPostBack = False Then
            FillContact()
            txtCSS(Me.Page)
        End If

        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Save"

                        If DateTime.Parse(dtValidUpto.Text) >= DateTime.Parse(dtAssignDate.Text) Then
                        Else

                            lstError.Items.Add("Role Valid upto date cannot be less than Role assigned date...")
                            Exit Sub
                        End If
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If updateUser() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Role updated successfully...")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Error occured while updating the record...")
                        End If
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If updateUser() = True Then
                            Response.Write("<script>self.opener.Form1.submit();</script>")
                            Response.Write("<script>window.close();</script>")
                        End If
                End Select

            Catch ex As Exception

            End Try
        End If

        'Security Block
        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block

    End Sub
#End Region

#Region "fill contacts"
    Private Sub FillContact()
        Dim strConnection As New SqlConnection

        strConnection = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim dsSearch As DataSet
        Dim inti As Integer
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String

        strSql = "select * from T060022 WHERE  RA_IN4_User_Role_ID_PK=" & codeID
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            sqrdRecords = SQL.Search("", "", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                sqrdRecords.Read()
                txtRole.Text = IIf(IsDBNull(sqrdRecords.Item("RA_IN4_Role_ID_FK")), " ", sqrdRecords.Item("RA_IN4_Role_ID_FK"))
                dtAssignDate.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("RA_DT8_Assigned_Date")), " ", sqrdRecords.Item("RA_DT8_Assigned_Date")), mdlMain.IsTime.DateOnly)
                dtValidUpto.Text = SetDateFormat(IIf(IsDBNull(sqrdRecords.Item("RA_DT8_Valid_UpTo")), " ", sqrdRecords.Item("RA_DT8_Valid_UpTo")), mdlMain.IsTime.DateOnly)

                Ddl_Status.SelectedValue = IIf(IsDBNull(sqrdRecords.Item("RA_VC4_Status_Code")), -1, sqrdRecords.Item("RA_VC4_Status_Code"))

                sqrdRecords.Close()
            Else
                lstError.Items.Add("server is busy please try later...")
            End If
        Catch ex As Exception
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            'cpnlError.Text = "Error"
            lstError.Items.Add("Error occur please try later...")    'ex.Message.ToString()
        End Try
    End Sub
#End Region

#Region "Update Role"
    Function updateUser() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        If DateTime.Parse(dtValidUpto.Text) >= DateTime.Parse(dtAssignDate.Text) Then
        Else
            lstError.Items.Add("Role Valid upto date cannot be less than Role assigned date...")
            Exit Function
        End If

        Try
            getRoleValues(arColumnName, arRowData)
            mstGetFunctionValue = UpdateRole(codeID, arColumnName, arRowData)

            If mstGetFunctionValue.ErrorCode = 0 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            CreateLog("CraeteUser", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    Function UpdateRole(ByVal AddressNo As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Update("T060022", "UpdateRole", "UpdateRole-251", "select * from T060022 where RA_IN4_User_Role_ID_PK=" & AddressNo & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Error occured while updating records..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateUserLogin-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Sub getRoleValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)
        arColumnName.Clear()
        arRowData.Clear()

        arColumnName.Add("RA_IN4_Assigned_By_FK")
        arColumnName.Add("RA_DT8_Assigned_Date")
        arColumnName.Add("RA_DT8_Valid_UpTo")
        arColumnName.Add("RA_VC4_Status_Code")
        arColumnName.Add("RA_DT8_Status_Date")

        arRowData.Add(txtAssBy.Text.Trim)
        arRowData.Add(CDate(dtAssignDate.Text))
        arRowData.Add(CDate(dtValidUpto.Text))
        arRowData.Add(Ddl_Status.SelectedValue)
        arRowData.Add(Now)

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
