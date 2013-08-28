Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Text
Imports System.Security.Cryptography

Partial Class MonitoringCenter_ProcessEnvironmentEntry
    Inherits System.Web.UI.Page
    'Protected WithEvents ProcessEnvironmentEdit As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents lblTitleLabelProcessEnvEdit As System.Web.UI.WebControls.Label
    'Protected WithEvents lblEnv As System.Web.UI.WebControls.Label
    'Protected WithEvents txtOwner As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtMachine As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDatabase As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlEnv As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents lblDB As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDatabase As System.Web.UI.WebControls.Label
    'Protected WithEvents lblMachine As System.Web.UI.WebControls.Label
    Public intID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            intID = CType(Request.QueryString("ID"), Integer)
        Catch ex As Exception

        End Try
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)
        Dim txthiddenImage As String = Request.Form("txthiddenImage")


        If txthiddenImage <> "" Then

            Select Case txthiddenImage

                Case "Close"
                    Response.Write("<script>window.close();</script>")
                Case "Ok"
                    If SaveEnvironmentInfo() = True Then
                        Response.Write("<script>window.close();</script>")
                    End If
                Case "Save"
                    SaveEnvironmentInfo()
                Case "Logout"
                    LogoutWSS()
            End Select

        End If

        Dim sqrdr As System.Data.SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqrdr = SQL.Search("ProcessEnvironmentEntry", "Load", "select * from T130172 where EV_NU9_ID_PK=" & intID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                While sqrdr.Read
                    GetEnvironment()
                    ddlEnv.SelectedValue = sqrdr("EV_VC30_Environment_Name")
                    txtOwner.Text = sqrdr("EV_VC30_Owner")
                    txtMachine.Text = sqrdr("EV_VC100_SystemID")
                    txtDatabase.Text = sqrdr("EV_VC50_Database")
                    txtUserID.Text = sqrdr(Decrypt("EV_VC50_UserID"))
                    txtPassword.Attributes.Add("value", IIf(Not IsDBNull(sqrdr(Decrypt("EV_VC50_Password"))), sqrdr(Decrypt("EV_VC50_Password")), ""))
                End While
                sqrdr.Close()
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEnvEntry", "Load-111", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Function SaveEnvironmentInfo() As Boolean
        Try

            If ValidateEnvironmentInfo() = True Then

                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '	SQL.DBTable = "T130172"
                SQL.DBTracing = False

                arColumnName.Add("EV_VC30_Environment_Name")
                arColumnName.Add("EV_VC30_Owner")
                arColumnName.Add("EV_VC100_SystemID")
                arColumnName.Add("EV_VC50_UserID")
                arColumnName.Add("EV_VC50_Password")
                arColumnName.Add("EV_VC50_Database")

                arRowData.Add(ddlEnv.SelectedValue)
                arRowData.Add(txtOwner.Text.Trim)
                arRowData.Add(txtMachine.Text.Trim)
                arRowData.Add(Encrypt(txtUserID.Text.Trim))
                arRowData.Add(Encrypt(txtPassword.Text.Trim))
                arRowData.Add(txtDatabase.Text.Trim)

                If SQL.Update("T130172", "", "", "select * from T130172 where EV_NU9_ID_PK=" & intID, arColumnName, arRowData) = True Then
                    lstError.Items.Clear()
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Message"
                    lstError.Items.Add("Record Updated successfully...")
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Error Message"
                    'ImgError.ImageUrl = "../images/error_image.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If

            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEnvEntry", "SaveEnvInfo-164", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function ValidateEnvironmentInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()


        If txtOwner.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Database Owner cannot be empty...")
        End If
        If txtMachine.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine cannot be empty...")
        End If
        If txtUserID.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("UserID cannot be empty...")
        End If
        If txtDatabase.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Database cannot be empty...")
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False

        End If
        lstError.Items.Clear()
        Try
            Dim introws As Integer

            'If SQL.Search("ProcesEnvironmentEntry", "ValidateEnvironmentInfo", "select  MM_IN4_MCode from t130011 where MM_VC20_MIP='" & txtMachine.Text & "'", introws) = False Then
            '    lstError.Items.Add("MachineID Mismatch...")
            '    shFlag = 1
            'End If

            'If shFlag = 1 Then
            '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            '    'cpnlError.Visible = True
            '    'cpnlError.Text = "Error Message"
            '    'ImgError.ImageUrl = "../images/warning.gif"
            '    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "error message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEnvEntry", "ValidateEnvInfo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function
    Private Function GetEnvironment()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New System.Data.DataSet
            Dim sqstr As String
            'this function fetch  ENV against a company
            sqstr = "select name from udc where udctype='PENV' "
            If SQL.Search("udc ", "ReportEntry", "GetEnvironment", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlENV dropdown fill acc to selected company
                ddlEnv.DataSource = dsTemp.Tables(0)
                'Env Name
                ddlEnv.DataTextField = "name"
                ddlEnv.DataBind()

            Else
                'SQL.Search is False ddlENV dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Env  avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetEnvironment-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    Private Function Encrypt(ByVal Data As String) As String
        Dim shaM As SHA1Managed = New SHA1Managed
        System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        '// Getting the bytes of the encrypted data.//
        Dim bytEncrypt() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        '// Converting the byte into string.//
        Dim strEncrypt As String = System.Convert.ToBase64String(bytEncrypt)
        Encrypt = strEncrypt
    End Function

    Private Function Decrypt(ByVal Data As String) As String
        Dim bytData() As Byte = System.Convert.FromBase64String(Data)
        Dim strData As String = ASCIIEncoding.ASCII.GetString(bytData)
        Decrypt = strData
    End Function

End Class
