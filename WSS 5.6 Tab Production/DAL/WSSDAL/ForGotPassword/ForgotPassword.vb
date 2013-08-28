#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [31/08/09]
' PURPOSE   : [This Class is used Get the information related to Forgot Password]
' TABLES    : [T060011]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports ION.Common.DAL
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
#End Region
Public Class ForgotPassword
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        clsData.ConnectionString = ConnectionString
        clsData.DBProvider = Provider
    End Sub
#Region "GetFinacialYear"
    ''' <summary>
    ''' This Function is used to get the Finanical Year
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserID(ByVal UserName As String) As Integer
        Dim UserID As Integer
        Try
            UserID = clsData.SearchSingleValue("select UM_IN4_Address_No_FK from t060011 where UM_VC50_useriD='" & UserName & "'")
            Return UserID
        Catch ex As Exception
            CreateLog("WSSDAL_ForgotPassword", "GetUserID-35", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return UserID
        End Try
    End Function

    Public Function UpdateEmailFlag(ByVal UserID As Integer) As Boolean
        Try
            If clsData.Update("Update t060011  set UM_CH1_Mail_Sent_Modify='F' where UM_IN4_Address_No_FK =" & UserID) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("WSSDAL_ForgotPassword", "UpdateEmailFlag-50", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
    Public Function GetTagInfo() As DataTable
        Try
            Dim dtTag As New DataTable
            dtTag = clsData.Search("select * from T000011", "T000011")
            Return dtTag
        Catch ex As Exception
            CreateLog("WSSDAL_ForgotPassword", "GetTagInfo-60", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

End Class
