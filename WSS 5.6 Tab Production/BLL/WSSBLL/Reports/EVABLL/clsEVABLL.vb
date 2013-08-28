#Region "NameSpace"
Imports WSSDAL
Imports ION.Logging.EventLogging
#End Region
Public Class clsEVABLL
    Private objEVA As clsEVADAL
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        objEVA = New clsEVADAL(ConnectionString, Provider)
    End Sub
    Public Function GetCompany() As DataTable
        Try
            Dim dt As New DataTable()
            dt = objEVA.GetCompany
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_EVABLL_clsEVABLL", "GetCompany-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetProject(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = objEVA.GetProject(CompanyID)
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_EVABLL_clsEVABLL", "GetProject-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetCall(ByVal CompanyID As Integer, ByVal ProjectID As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = objEVA.GetCall(CompanyID, ProjectID)
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_EVABLL_clsEVABLL", "GetCall-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetTask(ByVal CompanyID As Integer, ByVal ProjectID As Integer, ByVal CallNo As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = objEVA.GetTask(CompanyID, ProjectID, CallNo)
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_EVABLL_clsEVABLL", "GetTask-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetEvaDetail(ByVal CompanyID As Integer, ByVal ProjectID As Integer, ByVal CallNo As Integer, ByVal TaskNumber As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = objEVA.GetEVADetail(CompanyID, ProjectID, CallNo, TaskNumber)
            Return dt
        Catch ex As Exception
            CreateLog("WSSBLL_Reports_EVABLL_clsEVABLL", "GetEvaDetail-17", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
End Class
