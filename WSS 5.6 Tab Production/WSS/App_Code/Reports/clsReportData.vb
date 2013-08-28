Imports ION.Net
Imports ION.Data
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Data


Public Class clsReportData

    '*******************************************************************
    '' Author            :      Atul Sharama
    '' Create Date       :      20 March 2006
    '' Modification Date :      9 January 2007 ( UserWise  )    
    '' Purpose           :      Function returns Call number against the company ID 
    '                           Input: Sort ID ( sorting of call numbers) and Company ID 
    '                           OutPut: call number against which report will be shown
    ''*******************************************************************
    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
    Dim strUserID As String = HttpContext.Current.Session("PropUserID")
    Dim strCompanyID As String = HttpContext.Current.Session("PropCompanyID")

    Public Function extractCallNo(ByVal sortID As Integer, ByVal CompanyID As Integer, ByVal ProjectID As Integer) As DataTable
        Dim dsCallNo As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        ''  SQL.DBTable = "T040011"
        SQL.DBTracing = False
        Try

            ' if AdminRole for reports ? Yes
            If strPropAdmin = "1" Then
                If CompanyID = 0 Then
                    SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                End If
                If sortID = 1 Then
                    If CompanyID <> 0 And ProjectID = 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'  order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                    ElseIf CompanyID <> 0 And ProjectID <> 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'and  CM_NU9_Project_ID ='" & ProjectID & "'  order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                    End If

                Else
                    If CompanyID <> 0 And ProjectID = 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'  order by CM_NU9_Call_No_PK desc", dsCallNo, "sachin", "Prashar")
                    ElseIf CompanyID <> 0 And ProjectID <> 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'and  CM_NU9_Project_ID ='" & ProjectID & "'  order by CM_NU9_Call_No_PK desc", dsCallNo, "sachin", "Prashar")
                    End If
                End If
                ' for Other users 
            Else
                If CompanyID = 0 Then
                    SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where  (CM_NU9_Call_Owner= '" & strUserID & " '    ) order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                End If
                If sortID = 1 Then
                    If CompanyID <> 0 And ProjectID = 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'and (CM_NU9_Call_Owner= '" & strUserID & " '    ) order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                    ElseIf CompanyID <> 0 And ProjectID <> 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'and  CM_NU9_Project_ID ='" & ProjectID & "' and (CM_NU9_Call_Owner= '" & strUserID & " '    ) order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                    End If

                Else
                    If CompanyID <> 0 And ProjectID = 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'  and (CM_NU9_Call_Owner= '" & strUserID & " '    ) order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                    ElseIf CompanyID <> 0 And ProjectID <> 0 Then
                        SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'and  CM_NU9_Project_ID ='" & ProjectID & "'  and (CM_NU9_Call_Owner= '" & strUserID & " '    ) order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
                    End If
                End If
            End If


            Return dsCallNo.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallNo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCallNo = Nothing
        End Try
    End Function


    Public Function extractCallNoNewFirst(ByVal CompanyID As Integer) As DataTable
        Dim dsCallNoFirst As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        ''  SQL.DBTable = "T040011"
        SQL.DBTracing = False
        Try
            If CompanyID = 0 Then
                SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 order by CM_NU9_Call_No_PK asc", dsCallNoFirst, "sachin", "Prashar")
            Else
                SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "'  order by CM_NU9_Call_No_PK asc", dsCallNoFirst, "sachin", "Prashar")
            End If
            Return dsCallNoFirst.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallNo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCallNoFirst = Nothing
        End Try
    End Function


    Public Function extractCallNoNewSecond(ByVal CompanyID As Integer, ByVal PrevCallNo As Integer) As DataTable
        Dim dsCallNo As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        ''  SQL.DBTable = "T040011"
        SQL.DBTracing = False
        Try
            If CompanyID = 0 Then
                SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011  where CM_NU9_Call_No_PK >=" & PrevCallNo & "  order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
            Else
                SQL.Search("T040011", "WSSReportsD", "ExtractCallNo", "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & CompanyID & "' and  CM_NU9_Call_No_PK >=" & PrevCallNo & " order by CM_NU9_Call_No_PK asc", dsCallNo, "sachin", "Prashar")
            End If
            Return dsCallNo.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallNo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCallNo = Nothing
        End Try
    End Function
    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      01 April March 2006
    '' Modification Date :      8 January 2007 ( UserWise  ) 
    '' Purpose       :      Function returns type of company 
    '                       Input: Company ID of which Company a company Type is needed
    '                       OutPut: Returns Company ID 
    ''*******************************************************************

    Public Function extractCompanyType(ByVal CompanyID As Integer) As DataTable
        Dim dsType As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T010011"
        SQL.DBTracing = False

        Try

            SQL.Search("T010011", "WSSReportsD", "ExtractCallNo", "select   CI_IN4_Business_Relation as Type from t010011  where            CI_NU8_Address_Number='" & CompanyID & "'", dsType, "sachin", "Prashar")
            Return dsType.Tables(0)

        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      03 April March 2006
    '' Modification Date :      9 January 2007 ( UserWise  ) 
    '' Purpose       :      Function returns the name of the companies against their category and ID
    '                       Input: Company ID and Category of the company 
    '                       OutPut: Returns Company ID 
    ''*******************************************************************

    Public Function extractCustomer(ByVal CompanyID As Integer, ByVal projectID As Integer, ByVal Category As Integer) As DataTable
        Dim dsCustomer As New DataSet
        Dim dsType As New DataSet


        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T010011"
        SQL.DBTracing = False
        Try
            If CompanyID = 0 And Category = 1 Then
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct AM_VC8_Supp_Owner as AddressNo,um_vc50_userid, CI_VC36_Name, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name   from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number  order by CI_VC36_Name,um_vc50_userid", dsCustomer, "ExtractEmployee", "ExtractEmployee")
                Return dsCustomer.Tables(0)
                Exit Function
            End If
            SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select   CI_IN4_Business_Relation from t010011  where CI_NU8_Address_Number='" & CompanyID & "'", dsType, "Atul", "Sharma")

            If dsType.Tables(0).Rows(0).Item(0) = "SCM" And Category = 1 And projectID = 0 Then
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select  CI_VC36_Name as Name, CI_NU8_Address_Number as AddressNo   from t010011 where CI_VC8_Address_Book_Type='EM'  order by CI_VC36_Name asc", dsCustomer, "ExtractEmployee", "ExtractEmployee")
            ElseIf projectID <> 0 Then
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "SELECT um_in4_address_no_fk as AddressNo,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & projectID & " and PM_NU9_Comp_ID_FK=" & CompanyID & ") Order By Name", dsCustomer, "ExtractEmployee", "ExtractEmployee")
            Else
                ' This Query is to select all the employees of the currently logged in company and the employees of the support company who has worked for that client company 

                'SQL.Search("T010011", "WSSReportsD", "extractCustomer", "   (SELECT distinct um_in4_address_no_fk as AddressNo, um_vc50_userid  + ' ['+CI_VC36_Name + ']'as Name FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id  and UM_IN4_Company_AB_ID=ci_nu8_address_number and um_in4_company_ab_id='" & CompanyID & "')union (select distinct AM_VC8_Supp_Owner as AddressNo, um_vc50_userid   + ' ['+CI_VC36_Name + ']' as Name  from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number and AM_NU9_Comp_ID_FK='" & CompanyID & "' ) order by Name", dsCustomer, "ExtractEmployee", "ExtractEmployee")

                ' this query will select only those employees of Support Company who has 
                ' worked for the selected Client company .

                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "   select distinct AM_VC8_Supp_Owner as AddressNo,  upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name   from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number and AM_NU9_Comp_ID_FK='" & CompanyID & "' order by Name", dsCustomer, "ExtractEmployee", "ExtractEmployee")

                'SQL.Search("WSSReportsD", "ExtractCallNo", "select  CI_VC36_Name as Name, CI_NU8_Address_Number as AddressNo   from t010011 where CI_VC8_Address_Book_Type='EM' and CI_IN4_Business_Relation= '" & CompanyID & "' order by CI_VC36_Name asc", dsCustomer)
            End If

            Return dsCustomer.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCustomer", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCustomer = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      03 September 2006
    '' Modification Date :      9 January 2007 ( UserWise  ) 
    '' Purpose       :      Function returns the name of the Call Owner s according to the Project wise and 
    '                       Company Wise 
    '                       OutPut: Returns a list of Call Owners 
    ''*******************************************************************
    Public Function extractCallOwner(ByVal CompanyID As Integer, ByVal projectID As Integer, ByVal Category As Integer) As DataTable
        Dim dsCallOwner As New DataSet
        Dim dsType As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try

            If strPropAdmin = 1 Then 'Then check for the admin role for the reports

                If CompanyID = 0 And projectID = 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number)   order by CompanyName , Name  ", dsCallOwner, "ExtractEmployee", "ExtractEmployee")
                    Return dsCallOwner.Tables(0)

                ElseIf CompanyID = 0 And projectID <> 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & projectID & " order by Name ", dsCallOwner, "ExtractEmployee", "ExtractEmployee")
                    Return dsCallOwner.Tables(0)

                ElseIf CompanyID <> 0 And projectID = 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & CompanyID & " and a.CI_VC8_Status='ENA' order by Name ", dsCallOwner, "ExtractCallOwner", "ExtractCallOwner")
                    Return dsCallOwner.Tables(0)
                ElseIf CompanyID <> 0 And projectID <> 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & projectID & "  and  cm_nu9_comp_id_fk=" & CompanyID & " order by Name  ", dsCallOwner, "ExtractCallOwner", "ExtractCallOwner")
                    Return dsCallOwner.Tables(0)

                End If
            Else 'Check for the Non Admins

                ' Check For the call / Task type reports 
                If Category = 1 Then  ' Call Type Reports
                    If CompanyID = 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_call_owner= '" & strUserID & "'  order by CompanyName , Name  ", dsCallOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsCallOwner.Tables(0)

                    ElseIf CompanyID = 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & projectID & " and  cm_nu9_call_owner= '" & strUserID & "' order by Name ", dsCallOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsCallOwner.Tables(0)

                    ElseIf CompanyID <> 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & CompanyID & " and  cm_nu9_call_owner= '" & strUserID & "' order by Name ", dsCallOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsCallOwner.Tables(0)
                    ElseIf CompanyID <> 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & projectID & "  and  cm_nu9_comp_id_fk=" & CompanyID & " and  cm_nu9_call_owner= '" & strUserID & "' order by Name  ", dsCallOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsCallOwner.Tables(0)

                    End If

                Else ' Task Type Reports
                    If CompanyID = 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number)  order by CompanyName , Name  ", dsCallOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsCallOwner.Tables(0)

                    ElseIf CompanyID = 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & projectID & "  order by Name ", dsCallOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsCallOwner.Tables(0)

                    ElseIf CompanyID <> 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & CompanyID & "      order by Name ", dsCallOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsCallOwner.Tables(0)
                    ElseIf CompanyID <> 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & projectID & "  and  cm_nu9_comp_id_fk=" & CompanyID & "   order by Name  ", dsCallOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsCallOwner.Tables(0)

                    End If


                End If

            End If


        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCustomer", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCallOwner = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      04 September 2006
    '' Modification Date :      10 January 2007 ( UserWise  ) 
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
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_vc8_supp_owner as  addressNo,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  a.CI_vc8_Status='ENA' order by  d.CI_VC36_Name, a.ci_vc36_name  ", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsTaskOwner.Tables(0)

                    ElseIf CompanyID = 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & projectID & " and a.CI_vc8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name", dsTaskOwner, "ExtractEmployee", "ExtractEmployee")
                        Return dsTaskOwner.Tables(0)

                    ElseIf CompanyID <> 0 And projectID = 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo, a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & "  and a.CI_vc8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                        Return dsTaskOwner.Tables(0)
                    ElseIf CompanyID <> 0 And projectID <> 0 Then
                        SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo,tm_nu9_project_id as projectID,a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where a.CI_vc8_Status='ENA' and tm_nu9_Comp_id_fk=" & CompanyID & " and tm_nu9_project_id =" & projectID & " order by   d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
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
                        'SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo, a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & " and tm_vc8_supp_owner ='" & strUserID & "' order by    d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "ExtractCallOwner", "ExtractCallOwner")
                        'SQL.Search("t210012", "WSSReports", "ExtractTaskOwner", "SELECT  upper(UM_VC50_UserID) + '[' + t010011.CI_VC36_Name + ']' as Name,PM_NU9_Comp_ID_FK as compID,PM_NU9_Project_ID_Fk,PM_NU9_Project_Member_ID as AddressNo, UM_VC50_UserID,PM_NU9_Comp_ID_FK from T060011,T210012,t010011 where UM_IN4_Address_No_FK=PM_NU9_Project_Member_ID and Um_in4_Company_ab_Id=t010011.CI_NU8_Address_Number and PM_NU9_Comp_ID_FK=" & CompanyID & "", dsTaskOwner, "", "")
                        SQL.Search("t210012", "WSSReports", "ExtractTaskOwner", "select distinct tm_nu9_Comp_id_fk as compID,tm_vc8_supp_owner as  addressNo, a.ci_vc36_name,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & CompanyID & "  and a.CI_vc8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name ", dsTaskOwner, "", "")
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
    '' Author        :      Atul Sharama
    '' Create Date   :      05 April March 2006
    '' Modification Date :  09 Jan 2007 ( User Wise Company )
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
            SQL.Search("UDC", "WSSReportsD", "ExtractCompany", "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") order by CI_VC36_Name", dsCompany, "sachin", "Prashar")

            Return dsCompany.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCompany = Nothing
        End Try
    End Function
    Public Function extractTeams() As DataTable
        Dim dsTeam As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try
            SQL.Search("T570011", "WSSReportsD", "extractTeams", "Select MT_NU9_Team_ID_PK as TeamID,MT_VC16_Team_Name TeamName From T570011 Order By TeamName ")
            Return dsTeam.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractTeams", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)

        End Try
    End Function
   
    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      10 April March 2006
    '' Purpose       :      Function returns the UID for monitoring report 
    '                       Input:
    '                       OutPut: All Available UID s  
    ''*******************************************************************

    Public Function extractUID() As DataTable
        Dim dsCompany As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "UDC"
        SQL.DBTracing = False
        Try
            SQL.Search("UDC", "WSSReportsD", "ExtractUID", " SELECT distinct UH_NU12_UID  as UID from  T130111", dsCompany, "sachin", "Prashar")
            Return dsCompany.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractUID", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsCompany = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      12 April March 2006
    '' Purpose       :      Function returns the Call and Task Status of all the call s and task s 
    '                       Input: ID to know a call or task
    '                       OutPut: Call or Task status 
    ''*******************************************************************

    Public Function extractCTStatus(ByVal ID As Integer) As DataTable
        Dim dsType As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T010011"
        SQL.DBTracing = False

        Try
            SQL.Search("T010011", "WSSReportsD", "ExtractCallNo", "select distinct CN_VC20_Call_Status as CallStatus from t040011 select distinct TM_VC50_Deve_status as TaskStatus from t040021", dsType, "sachin", "Prashar")
            If ID = 1 Then
                Return dsType.Tables(0)
            ElseIf ID = 2 Then
                Return dsType.Tables(1)
            End If


        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function
    '*******************************************************************
    '' Author        :      Jagmit sidhu
    '' Create Date   :      29  Oct  2007
    '' Purpose       :      Function returns the Call status(All)
    '                       Input: ID to know a call 
    '                       OutPut: Call status 
    ''*******************************************************************

    Public Function extractCallStatus(ByVal CompanyID As Integer) As DataTable
        Dim dsType As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try
            SQL.Search("T010011", "WSSReportsD", "ExtractCallNo", "select  distinct su_vc50_status_name as CallStatus from T040081 where (SU_NU9_CompID=0 or SU_NU9_CompID=" & CompanyID & ") and (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=3)", dsType, "Jagmit", "Sidhu")
            Return dsType.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallStatus", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function
    Public Function extractTaskStatus(ByVal CompanyID As Integer) As DataTable
        Dim dsType As New DataSet
        SQL.DBConnection = SQL.GetConncetionString("ConnectionString")
        SQL.DBTracing = False
        Try
            SQL.Search("T010011", "WSSReportsD", "ExtractCallNo", "select  distinct su_vc50_status_name as TaskStatus from T040081 where (SU_NU9_CompID=0 or SU_NU9_CompID=" & CompanyID & ") and (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=464)", dsType, "Jagmit", "Sidhu")
            Return dsType.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallStatus", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function
    '*******************************************************************
    '' Author        :      Jagmit sidhu
    '' Create Date   :      20  Nov  2007
    '' Purpose       :      Function returns the Call status(All)
    '                       Input: ID to know a call 
    '                       OutPut: Call status 
    ''*******************************************************************

    Public Function extractCallCategory(ByVal CompanyID As Integer) As DataTable
        Dim dsType As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try
            SQL.Search("CallCategory", "WSSReportsD", "ExtractCallNo", "select distinct CM_VC8_Category as CallCategory from T040011 where  CM_NU9_Comp_Id_FK=" & CompanyID & " and CM_VC8_Category<>'NULL'and CM_VC8_Category<>''", dsType, "Jagmit", "Sidhu")
            Return dsType.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallCategory", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function
    '*******************************************************************
    '' Author        :      Jagmit sidhu
    '' Create Date   :      20  Nov  2007
    '' Purpose       :      Function returns the Call status(All)
    '                       Input: ID to know a call 
    '                       OutPut: Call status 
    ''*******************************************************************

    Public Function extractActionOwner(ByVal CompanyID As Integer, Optional ByVal ProjectID As Integer = 0) As DataTable
        Dim dsType As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        SQL.DBTracing = False
        Try
            If ProjectID = 0 Then
                SQL.Search("ActionOwner", "WSSReportsD", "ExtractCallNo", "select distinct(AM_VC8_Supp_Owner) as ActionOwnerID,CI_VC36_name as ActionOwner from t040031,t010011 where   AM_VC8_Supp_Owner=t010011.CI_NU8_Address_Number and AM_NU9_Comp_ID_FK=" & CompanyID & " and  CI_VC8_Status='ENA'  ", dsType, "Jagmit", "Sidhu")
            Else
                SQL.Search("ActionOwner", "WSSReportsD", "ExtractCallNo", "select distinct(AM_VC8_Supp_Owner) as ActionOwnerID,CI_VC36_name as ActionOwner from t040031,t010011 where   AM_VC8_Supp_Owner=t010011.CI_NU8_Address_Number and AM_NU9_Comp_ID_FK=" & CompanyID & "  and  CI_VC8_Status='ENA' and AM_NU9_Call_Number in (select distinct(CM_NU9_Call_No_PK) from t040011 where CM_NU9_Project_ID =" & ProjectID & " and CM_NU9_Comp_Id_FK=" & CompanyID & " ) ", dsType, "Jagmit", "Sidhu")
            End If
            Return dsType.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractActionOwner", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function
    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      20 April March 2006
    '' Purpose       :      Function returns the Count of Various Task and Call status types 
    '                       Input: Company ID and ID to know whether it is call or task , strStatus : status     ''                      of the call or task against which we need count of hours, dtfrom and dtTo : date     ''                      reange in between to count the number of hours spent 
    '                       OutPut: Count of No of Hours Spent 
    ''*******************************************************************

    Public Function extractCTStatusReport(ByVal id As Integer, ByVal intCompany As Integer, ByVal strStatus As String, ByVal dtFrom As String, ByVal dtTo As String) As DataTable
        Dim dsType As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T010011"
        SQL.DBTracing = False

        Try
            If id = 1 Then
                If intCompany = 0 Then

                    If strStatus = "N" Then
                        SQL.Search("T010011", "WSSReportsD", "ExtractTaskStatus", "select count(*) as Count from t040011 where convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) >= ('" & dtFrom & "') and convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) <=  ('" & dtTo & "')", dsType, "Atul", "Sharma")
                    Else
                        SQL.Search("T010011", "WSSReportsD", "ExtractCallStatus", "select count(*) as Count from t040011 where CN_VC20_Call_Status='" & strStatus & "' and convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) >= ('" & dtFrom & "') and convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) <=  ('" & dtTo & "')", dsType, "sachin", "Prashar")
                    End If
                Else


                    If strStatus = "N" Then
                        SQL.Search("T010011", "WSSReportsD", "ExtractTaskStatus", "select count(*) as Count from t040011 where convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) >= ('" & dtFrom & "') and convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) <=  ('" & dtTo & "')", dsType, "Atul", "Sharma")
                    Else

                        SQL.Search("T010011", "WSSReportsD", "ExtractCallStatus", "select count(*) as Count from t040011 where CN_VC20_Call_Status='" & strStatus & "' and convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) >= ('" & dtFrom & "') and convert(datetime,convert(varchar,CM_DT8_Request_Date,101),101) <=  ('" & dtTo & "') and CM_NU9_Comp_Id_Fk= '" & intCompany & "' ", dsType, "sachin", "Prashar")
                    End If

                End If
            ElseIf id = 2 Then

                If intCompany = 0 Then
                    If strStatus = "N" Then
                        SQL.Search("T010011", "WSSReportsD", "ExtractTaskStatus", "select count(*) as Count  from t040021 where convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) >= ('" & dtFrom & "') and  convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) <=  ('" & dtTo & "') ", dsType, "Atul", "Sharma")
                    Else
                        SQL.Search("T010011", "WSSReportsD", "ExtractCallStatus", "select count(*) as Count  from t040021 where TM_VC50_Deve_status='" & strStatus & "' and convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) >= ('" & dtFrom & "') and  convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) <=  ('" & dtTo & "') ", dsType, "sachin", "Prashar")
                    End If
                Else
                    If strStatus = "N" Then
                        SQL.Search("T010011", "WSSReportsD", "ExtractTaskStatus", "select count(*) as Count  from t040021 where  convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) >= ('" & dtFrom & "') and  convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) <=  ('" & dtTo & "') ", dsType, "Atul", "Sharma")
                    Else

                        SQL.Search("T010011", "WSSReportsD", "ExtractCallStatus", "select count(*) as Count  from t040021 where TM_VC50_Deve_status='" & strStatus & "' and convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) >= ('" & dtFrom & "') and  convert(datetime,convert(varchar,TM_DT8_Task_Date,101),101) <=  ('" & dtTo & "') and TM_NU9_Comp_Id_Fk='" & intCompany & "' ", dsType, "sachin", "Prashar")
                    End If
                End If
            End If
            Return dsType.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "ExtractCallStatus", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsType = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      12 April March 2006
    '' Purpose       :      Function Returns the List of Invoice Numbers as per company wise .
    '                       Input: Company ID for which the Invoice number s needed
    '                       OutPut: A list of all the invoice numbers 
    ''*******************************************************************


    Public Function extractInvoiceNo(ByVal sortID As Integer, ByVal CompanyID As Integer) As DataTable
        Dim dsInvoiceNo As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T080031"
        SQL.DBTracing = False
        Try
            If sortID = 1 Then
                SQL.Search("T080031", "WSSReportsD", "ExtractCallNo", "select distinct(IM_NU9_Invoice_ID_PK) as InvoiceNumber from t080031 where IM_NU9_Company_ID_PK='" & CompanyID & "'  order by IM_NU9_Invoice_ID_PK asc", dsInvoiceNo, "sachin", "Prashar")
            Else
                SQL.Search("T080031", "WSSReportsD", "ExtractCallNo", "select distinct(IM_NU9_Invoice_ID_PK) as InvoiceNumber from t080031 where IM_NU9_Company_ID_PK='" & CompanyID & "'  order by IM_NU9_Invoice_ID_PK desc", dsInvoiceNo, "sachin", "Prashar")
            End If

            Return dsInvoiceNo.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallNo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsInvoiceNo = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      12 August 2006
    '' Purpose       :      Function Returns the Status  of the invoices for the company specified .
    '                       Input: ComapnyID for which the Status of Invoice is needed .
    '                       OutPut: List of the Invoice Status 
    ''*******************************************************************
    Public Function extractInvoiceStatus(ByVal CompanyID As Integer) As DataTable
        Dim dsStatus As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        '        SQL.DBTable = "T080031"
        SQL.DBTracing = False
        Try

            SQL.Search("T080031", "WSSReportsD", "ExtractStatus", "select distinct IM_VC8_Invoice_Status as Status from t080031 where im_nu9_company_id_pk='" & CompanyID & "' ", dsStatus, "sachin", "Prashar")

            Return dsStatus.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "ExtractStatus", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            dsStatus = Nothing
        End Try
    End Function

    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      05 April March 2006
    '' Modification Date :  10 jan 2007 
    '' Purpose       :      Function returns the list containing the name of the projects 
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
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 where PR_VC8_Status='Enable' order by PR_VC20_Name", dsProject, "sachin", "Prashar")

                Else
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID ,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011  where PR_NU9_Comp_ID_FK='" & CompanyID & "' and  PR_VC8_Status='Enable' order by PR_VC20_Name", dsProject, "sachin", "Prashar")
                End If
            Else
                If CompanyID = 0 Then
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 where and  PR_VC8_Status='Enable'  order by PR_VC20_Name", dsProject, "sachin", "Prashar")

                Else
                    SQL.Search("T210011", "WSSReportsD", "ExtractProject", "select distinct  PR_VC20_Name as Name,PR_NU9_Project_ID_Pk as ID,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 where PR_NU9_Comp_ID_FK='" & CompanyID & "' and   PR_VC8_Status='Enable'   order by PR_VC20_Name ", dsProject, "sachin", "Prashar")

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
    '' Author        :      jagmit
    '' Create Date   :      08 Jan 2007 
    '' Purpose       :      Function Returns the list of all employees of a company 
    '                       Input:  
    '                       OutPut: Returns the list of all employees of a company .
    ''*******************************************************************
    Public Function ExtractEmployees(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dsEmployees As New DataSet
            Dim dsType As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            'SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(ci_vc36_name) as Name,CI_IN4_Business_Relation,ci_nu8_address_number as AddressNo from t010011 left outer join t010043 on ci_nu8_address_number=pi_nu8_address_no where ci_vc8_address_book_type='em' and ci_in4_business_relation='8'", dsEmployees, "ExtractEmployee", "ExtractEmployee")

            If strPropAdmin = "1" Then
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011 where UM_IN4_Company_AB_ID='" & CompanyID & "' and UM_Vc4_Status_code_Fk='ENB' order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
            Else
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011 where UM_IN4_Company_AB_ID='" & CompanyID & "' and  UM_IN4_Address_No_FK= '" & strUserID & "' and UM_Vc4_Status_code_Fk='ENB' order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
            End If

            'upper(a.ci_vc36_name) + '['+upper(d.CI_VC36_Name) + ']' as Name

            Return dsEmployees.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractProject", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function


    '*******************************************************************
    '' Author        :      jagmit
    '' Create Date   :      08 Jan 2007 
    '' Purpose       :      Function Returns the list of Various Priorities of call 
    '                       Input:  
    '                       OutPut: Returns the list of Various Priorities of call .
    ''*******************************************************************

    Public Function ExtractPriority() As DataTable
        Try
            Dim dsPriority As New DataSet
            Dim dsType As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            SQL.Search("T040011", "WSSReportsD", "extractPriority", "select distinct cm_vc200_work_priority  as priority from t040011", dsPriority, "jagmit", "sidhu")
            Return dsPriority.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "ExtractPriority", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function

    '*******************************************************************
    '' Author        :      jagmit
    '' Create Date   :      08 Jan 2007 
    '' Purpose       :      Function Returns the list of Various Categories of call 
    '                       Input:  
    '                       OutPut: Returns the list of Various Categories of call .
    ''*******************************************************************

    Public Function extractCategoryCode(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dsPriority As New DataSet
            Dim dsType As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            SQL.Search("UDC", "WSSReportsD", "extractPriority", "Select Name as ID from UDC  where  ProductCode=0   and UDCType='CCAT' and (UDC.Company=" & CompanyID & " OR udc.Company=0)", dsPriority, "jagmit", "sidhu")
            Return dsPriority.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "ExtractPriority", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function
    '*******************************************************************
    '' Author        :      jagmit
    '' Create Date   :      08 Jan 2007 
    '' Purpose       :      Function Returns the list of Various Categories of call 
    '                       Input:  
    '                       OutPut: Returns the list of Various Categories of call .
    ''*******************************************************************

    Public Function extractTaskType(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dsPriority As New DataSet
            Dim dsType As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            SQL.Search("UDC", "WSSReportsD", "extractPriority", "Select Name as ID from UDC  where  ProductCode=0   and UDCType='TKTY' and (UDC.Company=" & CompanyID & " OR udc.Company=0)", dsPriority, "jagmit", "sidhu")
            Return dsPriority.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "ExtractPriority", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function
    '*******************************************************************
    '' Author        :          Suresh
    '' Create Date   :      17 Oct 2007 
    '' Purpose       :         Function Returns the list of Roles Assigned to a particular User
    '' Input:  
    '' OutPut:                  Returns the list of Various Priorities of call .
    ''*******************************************************************
    Public Function GetRoleDetails(ByVal CompanyID As String) As DataTable
        Try
            Dim dsRoleDetails As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            'SQL.Search("T040011", "WSSReports", "GetRoleDetails", "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_VC50_UserID='" & UserName & "' )  and RA_VC4_Status_Code='ENB') and ROM_VC50_Status_Code_FK='ENB'", dsRoleDetails, "Suresh", "Kharod")
            If CompanyID = 0 Then
                SQL.Search("T040011", "WSSReports", "GetRoleDetails", "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_VC50_Status_Code_FK='ENB'", dsRoleDetails, "Suresh", "Kharod")
            Else

                SQL.Search("T040011", "WSSReports", "GetRoleDetails", "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_VC50_Status_Code_FK='ENB' and  ROM_IN4_Company_ID_FK=" & CompanyID & "", dsRoleDetails, "Suresh", "Kharod")
            End If

            Return dsRoleDetails.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "GetRoleDetails", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function
    '*******************************************************************
    '' Author           :      Suresh
    '' Create Date   :      25 Aug 2007 
    '' Purpose         :      Function Returns the list of all employees of a company 
    '                               Input:  
    '                               OutPut: Returns the list of all employees of a company .
    ''*******************************************************************
    Public Function ExtractIPEmployees(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dsEmployees As New DataSet
            Dim dsType As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            'SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(ci_vc36_name) as Name,CI_IN4_Business_Relation,ci_nu8_address_number as AddressNo from t010011 left outer join t010043 on ci_nu8_address_number=pi_nu8_address_no where ci_vc8_address_book_type='em' and ci_in4_business_relation='8'", dsEmployees, "ExtractEmployee", "ExtractEmployee")

            If strPropAdmin = "1" Then
                If CompanyID = 0 Then
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011  order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
                Else
                    SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011 where UM_IN4_Company_AB_ID='" & CompanyID & "' order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
                End If
            Else
                SQL.Search("T010011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011 where UM_IN4_Company_AB_ID='" & CompanyID & "' and  UM_IN4_Address_No_FK= '" & strUserID & "' order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
            End If

            'upper(a.ci_vc36_name) + '['+upper(d.CI_VC36_Name) + ']' as Name

            Return dsEmployees.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractProject", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function


    '*******************************************************************
    '' Author        :      Suresh
    '' Create Date   :      25 Aug 2007 
    '' Purpose       :      Function Returns the list of all employees of a company 
    '                       Input:  
    '                       OutPut: Returns the list of all employees of a company .
    ''*******************************************************************
    Public Function ExtractSecPrmEmployees(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dsEmployees As New DataSet
            Dim dsType As New DataSet

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            'SQL.Search("T010011", "WSSReportsD", "extractCustomer", "Select RA_IN4_Role_ID_FK,RA_IN4_AB_ID_FK,upper(UM_VC50_UserID)as name,* From T060022,T060011 where UM_IN4_Address_No_FK=RA_IN4_AB_ID_FK and RA_IN4_Role_ID_FK=114", dsEmployees, "ExtractEmployee", "ExtractEmployee")

            If strPropAdmin = "1" Then
                If CompanyID = 0 Then
                    SQL.Search("T060011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011  where UM_IN4_Address_No_FK in (select distinct RA_IN4_AB_ID_FK  from T060022) order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
                Else
                    SQL.Search("T060011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID) as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011 where UM_IN4_Address_No_FK in (select distinct RA_IN4_AB_ID_FK  from T060022) and  UM_IN4_Company_AB_ID='" & CompanyID & "' order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
                End If
            Else
                SQL.Search("T060011", "WSSReportsD", "extractCustomer", "select upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation,UM_IN4_Address_No_FK as AddressNo from t060011 where UM_IN4_Address_No_FK in (select distinct RA_IN4_AB_ID_FK  from T060022) and UM_IN4_Company_AB_ID='" & CompanyID & "' and  UM_IN4_Address_No_FK= '" & strUserID & "' order by  upper(UM_VC50_UserID) ", dsEmployees, "ExtractEmployee", "ExtractEmployee")
            End If

            'upper(a.ci_vc36_name) + '['+upper(d.CI_VC36_Name) + ']' as Name

            Return dsEmployees.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractProject", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function

    Public Function extractCallType(ByVal CompanyID As Integer) As DataTable
        Try
            Dim dsCallType As New DataSet
            Dim dsType As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
            SQL.DBTracing = False
            SQL.Search("UDC", "WSSReportsD", "extractPriority", "Select Name as ID from UDC  where  ProductCode=0   and UDCType='Call' and (UDC.Company=" & CompanyID & " OR udc.Company=0)", dsCallType, "jagmit", "sidhu")
            Return dsCallType.Tables(0)
        Catch ex As Exception
            CreateLog("WSSReportsData", "extractCallType", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Function
End Class


