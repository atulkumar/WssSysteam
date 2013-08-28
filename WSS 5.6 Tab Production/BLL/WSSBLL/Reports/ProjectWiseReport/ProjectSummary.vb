#Region "NameSpace"
Imports WSSDAL
Imports ION.Logging.EventLogging
#End Region
Public Class ProjectSummary
    Private mobjProject As clsProjectSummary

    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        mobjProject = New clsProjectSummary(ConnectionString, Provider)
    End Sub
    Public Function GetCompany() As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjProject.GetCompany
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_ProjectWiseReport_clsProjectSummary", "GetCompany-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetProject(ByVal intCompanyID, ByVal inProjectID) As DataSet
        Try
            Dim dsProject As New DataSet
            dsProject = mobjProject.GetProject(intCompanyID, inProjectID)
            Return dsProject
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_ProjectWiseReport_clsProjectSummary", "GetCompany-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetProject(ByVal ComID As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjProject.GetProject(ComID)
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_ProjectWiseReport_clsProjectSummary", "GetCompany-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function

End Class
