#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [24/09/09]
' PURPOSE   : [This Class is used to get the records for spoc Calls
' TABLES    : [T040011,T040021,T040031,T010011]
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
Imports System.Web
Imports System.Data


#End Region
Public Class clsSpocWeekly
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        clsData.ConnectionString = ConnectionString
        clsData.DBProvider = Provider
    End Sub
    Public Function GetCallDetail(ByVal FromDate As String, ByVal ToDate As String, ByVal strCallStatus As String) As DataTable
        Try
            Dim dtCallDetail As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_CallDetail"
            cmd.Parameters.AddWithValue("@FromDate", FromDate)
            cmd.Parameters.AddWithValue("@ToDate", ToDate)
            cmd.Parameters.AddWithValue("@CallStatus", strCallStatus)
            dtCallDetail = clsData.Search(cmd)
            Return dtCallDetail
        Catch ex As Exception
            CreateLog("WSSDAL_SpocCallDetail_clsSpocWeekly", "GetCallDetail-336", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function getWeeklyTaskDetail(ByVal empId As Integer, ByVal toDate As DateTime, ByVal fromDate As DateTime, ByVal teamId As Integer, ByVal TLId As Integer) As DataSet
        Try
            Dim dsTaskDetail As New DataSet
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_weeklyReport"
            cmd.Parameters.AddWithValue("@empId", empId)
            cmd.Parameters.AddWithValue("@dateTo", toDate)
            cmd.Parameters.AddWithValue("@dateFrom", fromDate)
            cmd.Parameters.AddWithValue("@teamId", teamId)
            cmd.Parameters.AddWithValue("@TLId", TLId)
            dsTaskDetail = clsData.SearchDSPro(cmd)
            Return dsTaskDetail
        Catch ex As Exception
            CreateLog("WSSDAL_SpocCallDetail_clsSpocWeekly", "getWeeklyTaskDetail-337", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function getSpocCallDetail(ByVal dept As String, ByVal projectID As Integer, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal callStatus As String) As DataSet
        Try
            Dim dsCallDetail As New DataSet
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_spocCallDetailReport"
            cmd.Parameters.AddWithValue("@Department", dept)
            cmd.Parameters.AddWithValue("@ProjectID", projectID)
            cmd.Parameters.AddWithValue("@dateFrom", fromDate)
            cmd.Parameters.AddWithValue("@dateTo", toDate)
            cmd.Parameters.AddWithValue("@callStatus", callStatus)
            dsCallDetail = clsData.SearchDSPro(cmd)
            Return dsCallDetail
        Catch ex As Exception
            CreateLog("WSSDAL_SpocCallDetail_clsSpocWeekly", "getWeeklyTaskDetail-337", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
End Class
