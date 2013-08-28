Imports ION.Net
Imports ION.Data
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Configuration.ConfigurationSettings
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Public Class clsDSReports
    '*******************************************************************
    '' Author            :      Suresh Kharod
    '' Create Date       :      16 November 2007
    '' Modification Date :      --
    ''*******************************************************************
    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
    Dim strUserID As String = HttpContext.Current.Session("PropUserID")
    Dim strCompanyID As String = HttpContext.Current.Session("PropCompanyID")

    '*******************************************************************
    '' Author            :      Suresh Kharod
    '' Create Date       :      16 November 2007

    '' Purpose       :      Function returns the name of all the companies ( user wise )
    '                       Input: id as input to tell whether it is based on the Call owners or task owners 
    '                       OutPut: Returns name of all the companies 
    ''*******************************************************************
    Public Function extractCompany(ByVal id As Integer) As DataTable
        Dim dsCompany As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "UDC"
        SQL.DBTracing = False
        Try
            If strPropAdmin = "1" Then ' check for the Admin role for the reports 
                SQL.Search("UDC", "WSSReportsD", "ExtractCompany", "SELECT  distinct   upper(CI_VC36_Name) AS Name, CI_NU8_Address_Number as ID  FROM  T010011 WHERE   CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ") AND   (CI_VC8_Address_Book_Type = 'com') ORDER BY  upper( CI_VC36_Name)", dsCompany, "sachin", "Prashar")
            Else

                If id = 1 Then ' company for the call owners 

                    SQL.Search("UDC", "WSSReportsD", "ExtractCompany", "SELECT  distinct    upper(a.CI_VC36_Name) AS Name, a.CI_NU8_Address_Number as ID  FROM ( T010011 a inner join T040011 b   on b.cm_nu9_comp_id_fk=a.CI_NU8_Address_Number ) WHERE  CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ") AND   b.cm_nu9_call_owner ='" & strUserID & "'  order BY  upper(a.CI_VC36_Name)", dsCompany, "sachin", "Prashar")
                ElseIf id = 2 Then ' company for the task owners 
                    SQL.Search("UDC", "WSSReportsD", "ExtractCompany", "SELECT  distinct    upper(a.CI_VC36_Name) AS Name, a.CI_NU8_Address_Number as ID  FROM ( T010011 a inner join T040021 b  on b.tm_nu9_comp_id_fk=a.CI_NU8_Address_Number ) WHERE CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ") AND    b.TM_VC8_Supp_Owner= '" & strUserID & "' order BY  upper(a.CI_VC36_Name) ", dsCompany, "sachin", "Prashar")
                ElseIf id = 3 Then


                    SQL.Search("UDC", "WSSReportsD", "ExtractCompany", "SELECT  distinct   upper(CI_VC36_Name) AS Name, CI_NU8_Address_Number as ID  FROM  T010011 WHERE   CI_NU8_Address_Number IN (" & GetCompanySubQuery() & ") AND  (CI_VC8_Address_Book_Type = 'com') and  CI_NU8_Address_Number = '" & strCompanyID & "' ORDER BY  upper( CI_VC36_Name)  ", dsCompany, "sachin", "Prashar")

                End If


            End If
            Return dsCompany.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCompany = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author               :      Suresh Kharod
    '' Create Date       :      16 November 2007
    '' Purpose              :      Function returns the list containing the name of the projects 
    ''                          ( company wise and user wise 
    '                       Input: CompanyID for which projects belongs to. 
    '                       OutPut: Returns the list of the Project Names (Company and User Wise ).
    ''*******************************************************************

    Public Function extractProject(ByVal CompanyID As Integer) As DataTable
        Dim dsProject As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T210011"
        SQL.DBTracing = False
        Try
            If strPropAdmin = "1" Then
                If CompanyID = 0 Then
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name", dsProject, "sachin", "Prashar")

                Else
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID ,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011  where PR_NU9_Comp_ID_FK='" & CompanyID & "' order by PR_VC20_Name", dsProject, "sachin", "Prashar")
                End If
            Else
                If CompanyID = 0 Then
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name", dsProject, "sachin", "Prashar")

                Else
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID ,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName from ( T210011  inner join  T210012 on  PR_NU9_Project_ID_Pk=T210012.PM_NU9_Project_ID_Fk   and  PR_NU9_Comp_ID_FK=T210012.PM_NU9_Comp_ID_Fk) where PR_NU9_Comp_ID_FK='" & CompanyID & "'  and PM_NU9_Project_Member_ID='" & strUserID & "' order by PR_VC20_Name ", dsProject, "sachin", "Prashar")

                End If
            End If

            Return dsProject.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractProject", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsProject = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author               :      Suresh Kharod
    '' Create Date       :      16 November 2007
    '' Purpose       :      Function returns the name of the Task Owner s according to the Project wise and 
    '                       Company Wise 
    '                       OutPut: Returns a list of Task Owners 
    ''*******************************************************************
    Public Function extractTaskOwner(ByVal CompanyID As Integer, ByVal projectID As Integer, ByVal Category As Integer) As DataTable
        Dim dsTaskOwner As New DataSet
        Dim dsType As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try
            If Category = 1 Then  ' Assigned By 
                If CompanyID = 0 And projectID = 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_assign_by as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number)order by  d.CI_VC36_Name, a.ci_vc36_name ", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                    Return dsTaskOwner.Tables(0)

                ElseIf CompanyID = 0 And projectID <> 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,tm_nu9_assign_by as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number)where tm_nu9_project_id =" & projectID & " order by   d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                    Return dsTaskOwner.Tables(0)

                ElseIf CompanyID <> 0 And projectID = 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_nu9_assign_by as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " order by  d.CI_VC36_Name, a.ci_vc36_name", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                    Return dsTaskOwner.Tables(0)
                ElseIf CompanyID <> 0 And projectID <> 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_nu9_assign_by as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " and tm_nu9_project_id =" & projectID & " order by    d.CI_VC36_Name,a.ci_vc36_name", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                    Return dsTaskOwner.Tables(0)
                End If
            ElseIf Category = 2 Then ' Assigned To / Task Owner .
                If strPropAdmin = 1 Then 'Then check for the admin role for the reports
                    If CompanyID = 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_vc8_supp_owner as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number)  order by  d.CI_VC36_Name, a.ci_vc36_name  ", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsTaskOwner.Tables(0)

                    ElseIf CompanyID = 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & projectID & " order by    d.CI_VC36_Name,a.ci_vc36_name", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsTaskOwner.Tables(0)

                    ElseIf CompanyID <> 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo, a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " order by    d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsTaskOwner.Tables(0)
                    ElseIf CompanyID <> 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " and tm_nu9_project_id =" & projectID & " order by   d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsTaskOwner.Tables(0)
                    End If
                Else ' Not an Admin 

                    If CompanyID = 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_vc8_supp_owner as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_vc8_supp_owner= '" & strUserID & "' order by  d.CI_VC36_Name, a.ci_vc36_name  ", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsTaskOwner.Tables(0)

                    ElseIf CompanyID = 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & projectID & " and tm_vc8_supp_owner ='" & strUserID & "' order by    d.CI_VC36_Name,a.ci_vc36_name", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsTaskOwner.Tables(0)

                    ElseIf CompanyID <> 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo, a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " and tm_vc8_supp_owner ='" & strUserID & "' order by    d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsTaskOwner.Tables(0)
                    ElseIf CompanyID <> 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " and tm_nu9_project_id =" & projectID & " and tm_vc8_supp_owner ='" & strUserID & "' order by   d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsTaskOwner.Tables(0)
                    End If
                End If

            End If
        Catch ex As Exception

            CreateLog("WSSReportsData", "extractCustomer", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsTaskOwner = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author               :      Suresh Kharod
    '' Create Date       :      07 December 2007
    '' Purpose       :      Function returns the name of the Task Owners according to Company Wise 
    '                       OutPut: Returns a list of Task Owners 
    ''*******************************************************************
    Public Function extractTaskOwnerTR(ByVal CompanyID As Integer) As DataTable
        Dim dsTaskOwner As New DataSet
        Dim dsType As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try
            If CompanyID = 0 Then
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_vc8_supp_owner as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number and  a.CI_VC8_Status='ENA' )order by  d.CI_VC36_Name, a.ci_vc36_name ", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                Return dsTaskOwner.Tables(0)
            ElseIf CompanyID <> 0 Then
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " and  a.CI_VC8_Status='ENA'  order by  d.CI_VC36_Name, a.ci_vc36_name", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                Return dsTaskOwner.Tables(0)
            End If
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCustomer", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsTaskOwner = Nothing
        End Try
    End Function
End Class
