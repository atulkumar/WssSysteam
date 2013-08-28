
Partial Class AdministrationCenter_Inventory_Inventory_Detail
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub ImgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgClose.Click
        Response.Redirect("Inventory_View.aspx", False)
    End Sub
End Class
