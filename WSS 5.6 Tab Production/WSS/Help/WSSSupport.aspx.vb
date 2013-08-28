Imports System.IO

Partial Class Help_WSSSupport
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Call BindSupportGrid()

    End Sub

    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("../Home.aspx", False)
    End Sub

    Private Function BindSupportGrid() As Boolean
        Try
            If System.IO.Directory.Exists(Server.MapPath("../Dockyard/WSSSupport")) = True Then
                Dim dirSupport As New System.IO.DirectoryInfo(Server.MapPath("../Dockyard/WSSSupport"))
                grdSupport.DataSource = dirSupport.GetFiles()
                grdSupport.DataBind()
                dirSupport = Nothing
            End If

        Catch ex As Exception
        End Try
    End Function

    Private Sub grdSupport_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdSupport.ItemCommand
        'Dim strFileName As String = e.CommandName
        'Dim strFilePath As String = e.CommandArgument.ToString()

        'Try
        '    Dim strmFile As Stream = File.OpenRead(strFilePath)
        '    Dim buffer(strmFile.Length) As Byte

        '    strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))

        '    Response.ClearHeaders()
        '    Response.ClearContent()
        '    Response.ContentType = "application/octet-stream"
        '    Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(strFileName))
        '    Response.BinaryWrite(buffer)
        '    Response.End()
        'Catch ex As Exception

        'End Try
    End Sub
End Class
