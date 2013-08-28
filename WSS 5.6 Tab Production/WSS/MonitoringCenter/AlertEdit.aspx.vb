Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_AlertEdit
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtAlertDescription.Attributes.Add("OnKeyPress", "return MaxLength('" & txtAlertDescription.ClientID & "','450');")
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not IsPostBack Then
            Call FillAlertTypeDropDown()
            mstrqID = Request.QueryString("ID")
        End If

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        If txthiddenImage <> "" Then

            Select Case txthiddenImage
                Case "Ok"
                    If mstrqID = "-1" Then
                        If SaveAlert() = True Then
                            Response.Write("<script>window.close();</script>")
                        Else
                            Exit Sub
                        End If
                    Else
                        If UpdateAlert() = True Then
                            Response.Write("<script>window.close();</script>")
                        Else
                            Exit Sub
                        End If
                    End If
                Case "Save"
                    If mstrqID = "-1" Then
                        If SaveAlert() = False Then
                            Exit Sub
                        End If
                    Else
                        If UpdateAlert() = False Then
                            Exit Sub
                        End If
                    End If
                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        If mstrqID = "-1" Then


        Else
            Dim sqrdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Try
                sqrdr = SQL.Search("AlertEdit", "load-100", "select * from T180011 where AM_NU9_AID_PK=" & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                If blnStatus = True Then
                    While sqrdr.Read
                        ddlAlertType.SelectedValue = IIf(IsDBNull(sqrdr("AM_VC6_TYPE")), "", sqrdr("AM_VC6_TYPE"))
                        txtAlertCode.Text = IIf(IsDBNull(sqrdr("AM_VC20_Code")), "", sqrdr("AM_VC20_Code"))
                        txtAlertSubject.Text = IIf(IsDBNull(sqrdr("AM_VC150_SUB")), "", sqrdr("AM_VC150_SUB"))
                        txtAlertDescription.Text = IIf(IsDBNull(sqrdr("AM_VC500_DESC")), "", sqrdr("AM_VC500_DESC"))
                    End While
                    sqrdr.Close()
                End If
            Catch ex As Exception
                CreateLog("AlertEdit", "Load-99", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")


            End Try
        End If
    End Sub

    Private Function SaveAlert() As Boolean
        Try
            If ValidateAlert() = True Then
                Dim arCol As New ArrayList
                Dim arRow As New ArrayList

                arCol.Add("AM_VC6_TYPE")
                arCol.Add("AM_VC20_Code")
                arCol.Add("AM_VC150_SUB")
                arCol.Add("AM_VC500_DESC")

                arRow.Add(ddlAlertType.SelectedItem.Text)
                arRow.Add(txtAlertCode.Text.Trim)
                arRow.Add(txtAlertSubject.Text.Trim)
                arRow.Add(txtAlertDescription.Text.Trim)

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                'SQL.DBTable = "T180011"
                SQL.DBTracing = False

                If SQL.Save("T180011", "AlertEdit", "SaveAlert-138", arCol, arRow) = True Then
                    mstrqID = SQL.Search("AlertEdit", "SaveAlert-138", "select max(AM_NU9_AID_pK) from T180011")
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Message"
                    'lstError.Items.Clear()
                    lstError.Items.Add("Record Save successfully...")
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)

                    Return True
                Else
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Error Message..."
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'ImgError.ImageUrl = "../images/warning.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
        Catch ex As Exception
            'cpnlError.Text = "Error Message"
            'cpnlError.Visible = True
            lstError.Items.Clear()
            'lstError.Items.Add(ex.Message)
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            Return False

        End Try

    End Function

    Private Function UpdateAlert() As Boolean
        Try
            If ValidateAlert() = True Then
                Dim arCol As New ArrayList
                Dim arRow As New ArrayList

                arCol.Add("AM_VC6_TYPE")
                arCol.Add("AM_VC20_Code")
                arCol.Add("AM_VC150_SUB")
                arCol.Add("AM_VC500_DESC")

                arRow.Add(ddlAlertType.SelectedItem.Text)
                arRow.Add(txtAlertCode.Text.Trim)
                arRow.Add(txtAlertSubject.Text.Trim)
                arRow.Add(txtAlertDescription.Text.Trim)

                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                'SQL.DBTable = "T180011"
                SQL.DBTracing = False

                If SQL.Update("T180011", "AlertEdit", "UpdateAlert", "select * from T180011 where AM_NU9_AID_pK=" & mstrqID, arCol, arRow) = True Then
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Message"
                    lstError.Items.Clear()
                    lstError.Items.Add("Record Updated successfully...")
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Error Message"
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'ImgError.ImageUrl = "../images/warning.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
        Catch ex As Exception
            'cpnlError.Text = "Error Message"
            'cpnlError.Visible = True
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            'lstError.Items.Add(ex.Message)
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            Return False

        End Try

    End Function
    Private Function ValidateAlert() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        If ddlAlertType.SelectedItem.Text.Equals("") Then
            lstError.Items.Add("Alert Type cannot be blank...")
            shFlag = 1
        End If
        If txtAlertCode.Text.Equals("") Then
            lstError.Items.Add("Alert Code cannot be blank...")
            shFlag = 1
        End If
        If txtAlertSubject.Text.Equals("") Then
            lstError.Items.Add("Alert Subject cannot be blank...")
            shFlag = 1
        End If
        If txtAlertDescription.Text.Equals("") Then
            lstError.Items.Add("Alert Description cannot be blank...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function

    Private Function FillAlertTypeDropDown()
        Dim sqrdr As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqrdr = SQL.Search("AlertEdit", "FillAlertTypeDropDown", "SELECT Name FROM UDC WHERE UDCType='ALM'", SQL.CommandBehaviour.Default, blnStatus, "")
            If blnStatus = True Then
                ddlAlertType.Items.Clear()
                ddlAlertType.Items.Add(New ListItem("", ""))
                While sqrdr.Read
                    ddlAlertType.Items.Add(New ListItem(sqrdr("Name"), sqrdr("Name")))
                End While
                sqrdr.Close()
            End If
        Catch ex As Exception
            CreateLog("AlertEdit", "FillAlertTypeDropDown-279", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")
        End Try
    End Function
End Class
