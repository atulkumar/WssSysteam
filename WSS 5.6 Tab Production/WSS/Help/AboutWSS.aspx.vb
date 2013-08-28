'***********************************************************************************************************
' Page                 :-About WSS 
' Purpose              :-It summarises the WSS like its pupose, function, aim, facilities in Wss etc.
' Date					   Author						Modification Date				Description
' 20/05/06	  	          Harpreet           			                                Created
'
' Notes: 
' Code:
'***********************************************************************************************************
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Security
Partial Class Help_AboutWSS
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

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
            Call LoadAboutHelp()
        End If


    End Sub


    Private Function LoadAboutHelp()
        Try
            Dim strFileURL As String
            Dim strmReader As StreamReader

            strmReader = File.OpenText(Server.MapPath(Request.ApplicationPath) & "\Help\AboutWSS.htm")
            'strmReader = File.OpenText("\Help\AboutWSS.htm")

            Dim strHelp As String
            strHelp = strmReader.ReadToEnd
            strmReader.Close()
            spHellpAbout.InnerHtml = strHelp

        Catch ex As Exception
            CreateLog("WSSHelp", "GetHelpFileName", LogType.Application, LogSubType.Exception, "", ex.Message)
        End Try
    End Function
End Class
