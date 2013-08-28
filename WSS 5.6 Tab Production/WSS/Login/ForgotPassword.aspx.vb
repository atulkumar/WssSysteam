#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [22/06/09]
' PURPOSE   : [This Screen is used Sent mail when user click on ForGot password] 
' TABLES    : [T060011]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "Name Space"
Imports WSSBLL
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.IO

#End Region
Partial Class Login_ForgotPassword
    Inherits System.Web.UI.Page
    Private objPwd As New clsForgotPassword(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName.ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        'Dim dtSystem As New DataTable
        'dtSystem = objPwd.GetTagInfo()
        'If dtSystem.Rows.Count > 0 Then
        '    For inti As Integer = 0 To dtSystem.Rows.Count - 1
        '        If dtSystem.Rows(inti).Item(0) = "Logo" Then
        '            'get logo image  from database
        '            imgLogo.ImageUrl = "../Images/" & dtSystem.Rows(inti).Item(1)
        '        ElseIf dtSystem.Rows(inti).Item(0) = "PunchLineTitle" Then
        '            'get PunchLineTitle  from database
        '            lblPunchLineTitle.Text = dtSystem.Rows(inti).Item(1)
        '        ElseIf dtSystem.Rows(inti).Item(0) = "PunchLineSubTitle" Then
        '            'get PunchLineSubTitle  from database
        '            lblPunchLineSubTitle.Text = dtSystem.Rows(inti).Item(1)
        '        ElseIf dtSystem.Rows(inti).Item(0) = "AccountHeading" Then
        '            'get AccountHeading  from database
        '            lblAccountHeading.Text = dtSystem.Rows(inti).Item(1)
        '        End If
        '    Next
        '    dtSystem.Clear()
        'End If


    End Sub

    Protected Sub btnpwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnpwd.Click
        Dim UserID As Integer
        'lblError.Text = ""
        Server.HtmlEncode(txtUserName.Text.Trim.ToUpper)
        If txtUserName.Text.Equals("") = False Then
            UserID = objPwd.GetUserID(txtUserName.Text.Trim.ToUpper)
            If UserID > 0 Then
                If objPwd.UpdateEmailFlag(UserID) = True Then

                    lblError.Text = "Password sent to Your Email ID....!!"
                    txtUserName.Text = ""
                End If
            Else
                lblError.Text = "User Name does not exist....!!"
                txtUserName.Text = ""
            End If

        Else

            lblError.Text = "Please Enter User Name....!!"
            txtUserName.Text = ""
        End If
    End Sub

 
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("login.aspx", False)
    End Sub
End Class
