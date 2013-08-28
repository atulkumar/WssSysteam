
Partial Class AdministrationCenter_AddressBook_ShowImage
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strPath As String
        strPath = Request.QueryString("ID")
        imgDesign.ImageUrl = strPath
    End Sub
End Class
