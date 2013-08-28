Imports ION.Logging.EventLogging
Imports System.Web.Security
Public Class ComingSoon
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user codeto initialize the page here
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("AB_Main", "Load-300", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

    End Sub
End Class
