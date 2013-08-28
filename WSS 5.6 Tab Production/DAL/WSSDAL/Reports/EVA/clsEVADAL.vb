
#Region "NameSpace"
Imports ION.Common.DAL
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.Web
Imports System.Data
#End Region
Public Class clsEVADAL
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        clsData.ConnectionString = ConnectionString
        clsData.DBProvider = Provider
    End Sub
    Public Function GetCompany() As DataTable
        Try
            Dim dtCompany As New DataTable
            dtCompany = clsData.Search("select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")order by CI_VC36_Name", "T010011")
            Return dtCompany
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_EVA_clsEVADAL", "GetCompany-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetCompanySubQuery() As String
        Try
            Return "(select UC_NU9_Comp_ID_FK CompID from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1 ) union (select 0 CompID)"
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_EVA_clsEVADAL", "GetCompanySubQuery-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

        End Try
    End Function
    Public Function GetProject(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dtCompany As New DataTable
            dtCompany = clsData.Search(" select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & CompanyID & " and PR_VC8_Status='Enable' order by PR_VC20_Name", "T210011")
            Return dtCompany
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_EVA_clsEVADAL", "GetProject-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Function GetCall(ByVal CompanyID As Integer, ByVal ProjectID As Integer) As DataTable
        Try
            Dim dtCall As New DataTable
            If CompanyID <> 0 And ProjectID = 0 Then
                dtCall = clsData.Search("Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK as CallID from t040011 where CM_NU9_Comp_Id_FK '" & CompanyID & "  order by CM_NU9_Call_No_PK asc", "T040011")
            Else
                dtCall = clsData.Search("Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK as CallID from t040011 where CM_NU9_Comp_Id_FK =" & CompanyID & " and  CM_NU9_Project_ID =" & ProjectID & "  order by CM_NU9_Call_No_PK asc", "T040011")
            End If
            Return dtCall
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_EVA_clsEVADAL", "GetCall-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetTask(ByVal CompanyID As Integer, ByVal ProjectID As Integer, ByVal CallNo As Integer) As DataTable
        Try
            Dim dtCall As New DataTable
            dtCall = clsData.Search("select distinct TM_NU9_Task_no_PK as TaskNo,TM_NU9_Task_no_PK  as TaskID from T040021 where TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Comp_ID_FK=" & CompanyID & "  and TM_NU9_Project_ID=" & ProjectID & "   order by TM_NU9_Task_no_PK asc", "T040021")
            Return dtCall
        Catch ex As Exception
            CreateLog("WSSDAL_Reports_EVA_clsEVADAL", "GetTask-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    Public Function GetEVADetail(ByVal CompanyID As Integer, ByVal ProjectID As Integer, ByVal CallNo As Integer, ByVal TaskNumber As Integer) As DataTable
        Try
            Dim dtEVADetail As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_EVADetail"
            cmd.Parameters.AddWithValue("@ProjectID", ProjectID)
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID)
            cmd.Parameters.AddWithValue("@CallID", CallNo)
            cmd.Parameters.AddWithValue("@TaskNo", TaskNumber)
            dtEVADetail = clsData.Search(cmd)
            Return dtEVADetail
        Catch ex As Exception
            CreateLog("WSSDAL_SpocCallDetail_clsSpocWeekly", "GetCallDetail-336", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
End Class
