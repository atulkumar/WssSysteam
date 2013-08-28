Imports System.Web.Security
Partial Class frm_NoAccess
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")
        If strhiddenImage <> "" Then
            Select Case strhiddenImage
                Case "Logout"
                    LogoutWSS()
            End Select
        End If
        lblRole.Text = lblRole.Text & " '" & HttpContext.Current.Session("PropRoleName") & "' Role"
    End Sub
End Class
