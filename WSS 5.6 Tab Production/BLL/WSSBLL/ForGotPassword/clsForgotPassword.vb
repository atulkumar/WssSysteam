#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [22/06/09]
' PURPOSE   : [This Class is used to send the Email for forgot Password ]
' TABLES    : [T060011]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports WSSDAL
Imports ION.Logging.EventLogging
#End Region
Public Class clsForgotPassword
    Private objForgotPassword As ForgotPassword


    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        objForgotPassword = New ForgotPassword(ConnectionString, Provider)
    End Sub
    Public Function GetUserID(ByVal UserName As String) As Integer
        Dim UserID As Integer
        Try
            UserID = objForgotPassword.GetUserID(UserName)
            Return UserID
        Catch ex As Exception
            CreateLog("WSSBLL_clsForgotPassword", "GetUserID-35", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return UserID
        End Try
    End Function
    Public Function UpdateEmailFlag(ByVal UserID As Integer) As Boolean
        Try
            If objForgotPassword.UpdateEmailFlag(UserID) = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("WSSBLL_clsForgotPassword", "UpdateEmailFlag-35", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
    Public Function GetTagInfo() As DataTable
        Try
            Dim dtTag As New DataTable
            dtTag = objForgotPassword.GetTagInfo
            Return dtTag
        Catch ex As Exception
            CreateLog("WSSBLL_clsForgotPassword", "GetTagInfo-55", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
End Class
