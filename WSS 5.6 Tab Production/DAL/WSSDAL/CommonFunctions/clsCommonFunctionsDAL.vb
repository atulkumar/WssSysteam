#Region "Purpose"
' "CREATION"
' CREATED BY: [Tarun Pahuja]
' CREATED ON: [10/08/09]
' PURPOSE   : [This Class is used fill The dropdowns used in WSS..this class will have the common functions used to fill the Dropdowns]
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

Public Class clsCommonFunctionsDAL
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        clsData.ConnectionString = ConnectionString
        clsData.DBProvider = Provider
    End Sub
#Region "Functions"

    ''' <summary>
    ''' This function will return the dataset to bind the Call Type RadDropdown
    ''' to fill call type DDL, one parameter is required.. i.e. Session("PropCAComp")
    ''' </summary>
    ''' <returns>dataset</returns>
    ''' <remarks></remarks>
    Public Function fillRadCallTypeDDL(ByVal strCompID As String) As DataSet
        Dim dsCallTypeDDL As New DataSet
        Try
            dsCallTypeDDL = clsData.SearchDS("select CT_VC8_CallType_FK as ID,Description as Description, isnull(CI_VC36_Name,'') Company  from T040103,UDC ,T010011 where CT_BT1_CallEnteryFlag =1 and UDCType='CALL' and CT_NU9_CompID_FK in (0," & strCompID & ") and CT_VC8_CallType_FK=name and CI_NU8_Address_Number=*CT_NU9_CompID_FK Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-fillRadCallTypeDDL-36", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsCallTypeDDL
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskType(ByVal CompanyID As Integer) As DataSet
        Dim dsTaskTypeDDL As New DataSet
        Try
            dsTaskTypeDDL = clsData.SearchDS("Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company=CI_NU8_Address_Number and ProductCode=0 and UDCType='TKTY' and UDC.Company=" & CompanyID & " union Select Name as ID,Description,'' as Company from UDC where ProductCode=0 and UDCType='TKTY' and UDC.Company=0 Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadTaskType-52", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskTypeDDL
    End Function
    Public Function FillRadCallOwnerDefault(ByVal CallNo As Integer) As String
        Dim strCallOwner As String = String.Empty
        Try
            strCallOwner = clsData.SearchSingleValue("select distinct CM_NU9_Call_Owner from T040011 where CM_NU9_Call_No_PK=" & CallNo & "")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadCallOwner-52", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return strCallOwner
    End Function
    Public Function FillMailIdDefault(ByVal CallNo As Integer, ByVal TaskNo As Integer) As DataSet
        Dim strTaskOwnerMailId As New DataSet
        Try
            strTaskOwnerMailId = clsData.SearchDS("select distinct TM_VC8_Supp_Owner,CI_VC28_Email_1 from T010011 T1,T040021 T2 where T2.TM_NU9_Call_No_FK=" & CallNo & " and T1.CI_NU8_Address_Number=T2.TM_VC8_Supp_Owner")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadTaskOwnerMailID-52", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return strTaskOwnerMailId
    End Function



    ''' <summary>
    ''' This function will return the dataset to bind the Requested By User RadDropdown
    ''' to fill call type DDL, one parameter is required.. i.e. Session("PropCAComp")
    ''' </summary>
    ''' <returns>dataset</returns>
    ''' <remarks></remarks>
    Public Function fillRadRequestedByDDL(ByVal strCompID As String) As DataSet
        Dim dsRequestedByDDL As New DataSet
        Try
            dsRequestedByDDL = clsData.SearchDS("SELECT um_in4_address_no_fk as ID,(rtrim(ltrim(UName.ci_vc36_name)) + '[' + um_vc50_userid + ']') as Name,t010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where UM_VC4_Status_Code_FK='ENB' and t010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and (um_in4_company_ab_id=" & strCompID & ") and UM_IN4_Company_AB_ID=" & strCompID & " Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-fillRadRequestedByDDL-68", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsRequestedByDDL
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <param name="ProjectID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskOwner(ByVal CompanyID As Integer, ByVal ProjectID As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = clsData.SearchDS("SELECT um_in4_address_no_fk as ID,ci_vc36_name as Name,(select ci_vc36_name from t010011 where ci_nu8_address_number=" & CompanyID & ") as Company FROM t060011,t010011 where ci_nu8_address_number=UM_IN4_Address_No_FK and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & ProjectID & " and PM_NU9_Comp_ID_FK=" & CompanyID & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA' ) Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadTaskOwner-85", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskPriority(ByVal CompanyID As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = clsData.SearchDS("Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='PRIO' and UDC.Company=" & CompanyID & "  union Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='PRIO' and UDC.Company=0 Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadTaskPriority-101", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillRadStatus(ByVal CompanyID As Integer) As DataSet
        Dim dsStatus As New DataSet
        Try
            dsStatus = clsData.SearchDS("select SU_VC50_Status_Name as ID,SU_VC500_Status_Description as description,CI_VC36_Name as Company from T040081,T010011 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_CompID*=CI_NU8_Address_Number and SU_NU9_CompID=" & CompanyID & "  union select SU_VC50_Status_Name as Name,SU_VC500_Status_Description as description,'' as Company from T040081 Where (SU_NU9_ScreenID=464  or SU_NU9_ScreenID=0) and SU_NU9_ID_PK>3 and SU_NU9_CompID=0 Order By ID")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadStatus-117", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsStatus
    End Function

    Public Function FillRadCallPriority(ByVal CompanyID As Integer) As DataSet
        Dim dsCallPriority As New DataSet
        Try
            dsCallPriority = clsData.SearchDS("Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType='PRIO' and UDC.Company=" & CompanyID & "  union  Select Name as ID,Description,'' as Company from UDC  where  ProductCode=0   and UDCType='PRIO' and UDC.Company=0 Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadCallPriority-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsCallPriority
    End Function

    Public Function FillRadProjects(ByVal CompanyID As Integer) As DataSet
        Dim dsProjects As New DataSet
        Try
            dsProjects = clsData.SearchDS("select PR_NU9_Project_ID_Pk as ID,  PR_VC20_Name as Name , PR_VC8_Type as ProjectType from T210011 where PR_NU9_Comp_ID_FK=" & CompanyID & " and PR_VC8_Status='Enable' order by Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadProjects-135", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsProjects
    End Function
    Public Function fillRadCommentType(ByVal CompanyID As Integer, ByVal CallNo As Integer) As DataSet
        Dim dsProjects As New DataSet
        Try
            'If CallNo = 0 Then
            '    'dsProjects = clsData.SearchDS("select distinct um_in4_address_no_fk as ID,(isnull(T2.CI_VC36_Name,'')+'['+isnull(t3.PI_VC8_Department,'')+']'+'['+isnull(t2.CI_VC36_ID_1,'')+']') as Name,UM_VC50_UserID from T060011, T010011 T1, T010011 T2, T010043 t3 where t3.PI_NU8_Address_No=T060011.um_in4_address_no_fk and T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in(select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & CompanyID & " and UC_BT1_Access=1)  Order By UM_VC50_UserID")
            '    dsProjects = clsData.SearchDS("select um_in4_address_no_fk as ID,(rtrim(ltrim(T2.CI_VC36_Name))) + '[' + (rtrim(ltrim(um_vc50_userid))) + '][' +  (rtrim(ltrim(T1.ci_vc36_name))) + ']' as Name, T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & CompanyID & " and UC_BT1_Access=1)  Order By Name")
            'Else
            '    'dsProjects = clsData.SearchDS("select distinct um_in4_address_no_fk as ID,(isnull(T2.CI_VC36_Name,'')+'['+isnull(t3.PI_VC8_Department,'')+']'+'['+isnull(t2.CI_VC36_ID_1,'')+']') as Name,UM_VC50_UserID from T060011, T010011 T1, T010011 T2, T010043 t3 where t3.PI_NU8_Address_No=T060011.um_in4_address_no_fk and T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in(select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & CompanyID & " and UC_BT1_Access=1)  Order By UM_VC50_UserID")
            '    dsProjects = clsData.SearchDS("select um_in4_address_no_fk as ID,(rtrim(ltrim(T2.CI_VC36_Name))) + '[' + (rtrim(ltrim(um_vc50_userid))) + '][' +  (rtrim(ltrim(T1.ci_vc36_name))) + ']' as Name, T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & CompanyID & " and UC_BT1_Access=1)  Order By Name")
            'End If

            If CallNo = 0 Then
                dsProjects = clsData.SearchDS("select um_in4_address_no_fk as ID,T2.ci_vc36_name + ' [' + um_vc50_userid + '] [' +  T1.ci_vc36_name + ']' as Name, T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & CompanyID & " and UC_BT1_Access=1)  Order By Name")
            Else
                dsProjects = clsData.SearchDS("select um_in4_address_no_fk as ID,T2.ci_vc36_name + ' [' + um_vc50_userid + '] [' +  T1.ci_vc36_name + ']' as Name, T1.ci_vc36_name Company   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & CompanyID & " and UC_BT1_Access=1)  Order By Name")
            End If
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadCommentType-141", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsProjects
    End Function
    Public Function FillRadTaskOwnerForTaskForward(ByVal ProjectID As Integer, ByVal CompanyID As Integer, ByVal TaskOwnerID As Integer) As DataSet
        Dim dsProjects As New DataSet
        Try

            dsProjects = clsData.SearchDS("SELECT um_in4_address_no_fk as ID,T1.CI_VC36_Name + ' [' + um_vc50_userid +']' as Name,T2.ci_vc36_name as Company FROM t060011,t010011 T1,T010011 T2 where T2.ci_nu8_address_number=um_in4_company_ab_id and T1.ci_nu8_address_number=UM_IN4_Address_No_FK and um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & ProjectID & " and PM_NU9_Comp_ID_FK=" & CompanyID & "  and PM_NU9_Project_Member_ID<>" & TaskOwnerID & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name")
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsDAL-FillRadCommentType-141", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsProjects
    End Function
#End Region

End Class
