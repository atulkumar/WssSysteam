Imports System.Web.UI.MobileControls.MobilePage

Partial Class _Default
    Inherits System.Web.UI.Page
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("Login/Login.aspx", False)
    End Sub
End Class
