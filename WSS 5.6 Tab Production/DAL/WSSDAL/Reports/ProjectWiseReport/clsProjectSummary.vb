#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [15/07/09]
' PURPOSE   : [This Class is used Get the information Of team members,Esthous of paryicular project and                    company
' TABLES    : [T040011,T040021,T040031]
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
Public Class clsProjectSummary
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        clsData.ConnectionString = ConnectionString
        clsData.DBProvider = Provider
    End Sub

#Region "Functions"
    Public Function GetCompany() As DataTable
        Try
            Dim dtCompany As New DataTable
            dtCompany = clsData.Search("select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")order by CI_VC36_Name", "T010011")
            Return dtCompany
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_ProjectWiseReport_clsProjectSummary", "GetCompany-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Function GetProject(ByVal CompanyID As Integer, ByVal ProjectID As Integer) As DataSet
        Try
            Dim dsProjectDetail As New DataSet
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_GetProjectDetail"
            cmd.Parameters.AddWithValue("@CompID", CompanyID)
            cmd.Parameters.AddWithValue("@ProjectID", ProjectID)
            dsProjectDetail = clsData.SearchDSPro(cmd)
            Return dsProjectDetail
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_ProjectWiseReport_clsProjectSummary", "GetProject-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetProject(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dtCompany As New DataTable
            dtCompany = clsData.Search(" select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & CompanyID & " and PR_VC8_Status='Enable' order by PR_VC20_Name", "T210011")
            Return dtCompany
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_ProjectWiseReport_clsProjectSummary", "GetProject-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Function GetCompanySubQuery() As String
        Try
            Return "(select UC_NU9_Comp_ID_FK CompID from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1 ) union (select 0 CompID)"
        Catch ex As Exception
            CreateLog("Reports_ProjectWiseReport_clsProjectSummary", "GetCompanySubQuery-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

        End Try
    End Function
#End Region

End Class
