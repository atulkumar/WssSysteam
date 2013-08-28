'***********************************************************************************************************
' Page                 :-WSSHelp
' Purpose              :-This screen shows instructions to use each screen of WSS. It guides user about each                          & everything about each screen.
' Date					   Author						Modification Date				Description
' 18/05/06	  	          Harpreet           			                                Created
'
' Notes: 
' Code:
'************************************************************************************************************
Imports ION.Data
Imports System.IO
Imports ION.Logging.EventLogging

Partial Class Help_WSSHelp
    Inherits System.Web.UI.Page
    'it will hold the screen ID
    Private mintScreenID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        mintScreenID = Request.QueryString("ScreenID")

        Call GetHelpFile(mintScreenID)

    End Sub

    'Private Function GetHelpFile(ByVal intScreenID As Integer) As String
    '    Try


    '        If mintScreenID = 9999 Then
    '            strmReader = File.OpenText(Server.MapPath(Request.ApplicationPath) & "\Help\StartUp Page.htm")
    '        Else
    '            strFileURL = SQL.Search("", "", "select OBM_VC200_HelpFileURL, OBM_VC1000_HelpTitle from T070011 where OBM_IN4_Object_ID_PK=" & intScreenID)
    '            strmReader = File.OpenText(Server.MapPath(Request.ApplicationPath) & "\Help\" & strFileURL)
    '            lblHelpTitle.Text = SQL.Search("", "", "select  OBM_VC1000_HelpTitle from T070011 where OBM_IN4_Object_ID_PK=" & intScreenID)
    '        End If
    '        Dim strHelp As String
    '        strHelp = strmReader.ReadToEnd
    '        strmReader.Close()
    '        spHellp.InnerHtml = strHelp

    '    Catch ex As Exception
    '        CreateLog("WSSHelp", "GetHelpFileName", LogType.Application, LogSubType.Exception, "", ex.Message)
    '    End Try
    'End Function

    Private Function GetHelpFile(ByVal intScreenID As Integer) As String

        Try
            Dim strFileURL As String = ""
            If mintScreenID = 9999 Then
                helpFrame.Attributes.Add("src", "StartUp Page.htm")
            Else
                strFileURL = SQL.Search("", "", "select OBM_VC200_HelpFileURL, OBM_VC1000_HelpTitle from T070011 where OBM_IN4_Object_ID_PK=" & intScreenID)
                lblHelpTitle.Text = SQL.Search("", "", "select  OBM_VC1000_HelpTitle from T070011 where OBM_IN4_Object_ID_PK=" & intScreenID)
            End If
            helpFrame.Attributes.Add("src", strFileURL)
        Catch ex As Exception
            CreateLog("WSSHelp", "GetHelpFileName", LogType.Application, LogSubType.Exception, "", ex.Message)
        End Try
    End Function
End Class
