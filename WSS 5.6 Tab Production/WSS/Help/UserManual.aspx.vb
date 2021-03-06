'***********************************************************************************************************
' Page                 :-User Manual 
' Purpose              :-This screen shows instructions to use each screen of WSS. It guides user about each                          & everything about each screen.
' Date					   Author						Modification Date				Description
' 20/05/06	  	          Harpreet           			                                Created
'
' Notes: 
' Code:
'************************************************************************************************************

Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Security
Partial Class Help_UserManual
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")
        txtCSS(Me.Page)
        If strhiddenImage <> "" Then

            Select Case strhiddenImage

                Case "Logout"
                    LogoutWSS()

            End Select

        End If
        If IsPostBack = False Then
            Call LoadHelpManual()
        End If



    End Sub


    Private Function LoadHelpManual()
        Try
            Dim strFileURL As String
            Dim strmReader As StreamReader

            strmReader = File.OpenText(Server.MapPath(Request.ApplicationPath) & "\Help\HELP file for WSS.htm")

            Dim strHelp As String
            strHelp = strmReader.ReadToEnd
            strmReader.Close()
            spHellpManual.InnerHtml = strHelp

        Catch ex As Exception
            CreateLog("WSSHelp", "GetHelpFileName", LogType.Application, LogSubType.Exception, "", ex.Message)
        End Try
    End Function
End Class
