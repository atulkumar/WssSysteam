Imports ION.Data
Imports ION.Net
Imports ION.Logging.EventLogging
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient

Partial Class _Error
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        txtCSS(Me.Page)
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("Error", "Load-52", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If



    End Sub

End Class
