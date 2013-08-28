#Region "NameSpace"
Imports WSSDAL
Imports ION.Logging.EventLogging
#End Region
Public Class clsSpocWeeklyBLL
    Private mobjSpoc As clsSpocWeekly
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        mobjSpoc = New clsSpocWeekly(ConnectionString, Provider)
    End Sub

    Public Function GetCallDetail(ByVal FromDate As String, ByVal ToDate As String, ByVal strCallStatus As String) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjSpoc.GetCallDetail(FromDate, ToDate, strCallStatus)
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_SpocCallDetail_clsSpocWeeklyBLL", "GetCallDetail-17", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function getWeeklyTaskDetail(ByVal empId As Integer, ByVal toDate As DateTime, ByVal fromDate As DateTime, ByVal teamId As Integer, ByVal TLId As Integer) As DataSet
        Dim ds As New DataSet()
        ds = mobjSpoc.getWeeklyTaskDetail(empId, toDate, fromDate, teamId, TLId)
        Return ds
    End Function
    Public Function getSpocCallDetail(ByVal dept As String, ByVal projectID As Integer, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal callStatus As String) As DataSet
        Try
            Dim dsTask As New DataSet()

            dsTask = mobjSpoc.getSpocCallDetail(dept, projectID, fromDate, toDate, callStatus)
            Return dsTask
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_SpocCallDetail_clsSpocWeeklyBLL", "getWeeklyTaskDetail-18", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
End Class

