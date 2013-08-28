'***********************************************************************************************************
' Page                   : - AJAX Server page for Name/ID search
' Purpose                : - Function in this page are used to get ID and Name info
'                            and Role  
' Date		    		Author						Modification Date					Description
' 20/07/06				Harpreet 					20/07/06	        				Created
'
''**********************************************************************************************************
Imports ION.Data
Imports ION.Logging.EventLogging
Imports ION.Net
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data


Partial Class AJAX_Server_AjaxInfo
    Inherits System.Web.UI.Page


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Call GetInfo(Request.QueryString("Type"))
    End Sub

#Region "*****---GetInfo---------*****"
    '************************************************************************
    ' Page                   : - AJAX Server page for Name/ID Search
    ' Purpose              : - This function will get the list of records in ID and Name format in a XML string  
    ' Date		    			Author						Modification Date					Description
    ' 20/07/06				Harpreet 					20/07/06	        					Created
    '
    ''************************************************************************
    Private Function GetInfo(ByVal strType As String)
        Try
            'it stores the SQL statement
            Dim strSQL As String
            'used to store used id
            Dim strUserID As String
            'boolean check
            Dim blnFound As Boolean = False
            'it stores the XML output string
            Dim strXml As String
            'it stores the company id
            Dim strCompID As String
            'it stores the password
            Dim strPassword As String
            'get the connection string
            Dim dsAjax As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString


            Select Case strType
                'Get All available companies
                'it will return all companies in XML format
                'it requires user id and password to search company for the selecrted user
                Case "COMP"
                    strUserID = Request.QueryString("UserID").ToString
                    strPassword = Request.QueryString("Password").ToString
                    strSQL = "select CI_NU8_Address_Number as ID, CI_VC36_Name as Name, CI_VC8_Status as Status from T010011 where CI_NU8_Address_Number in (select UM_IN4_Company_AB_ID from T060011 where UM_VC30_Password='" & IONEncrypt(strPassword) & "' and UM_VC50_UserID ='" & strUserID & "')"
                    If SQL.Search("T010011", "Ajaxinfo", "Getinfo", strSQL, dsAjax, "", "") = True Then

                    End If
                    'Get All available roles for used: 
                    'it will return all roles in XML format
                    'it requires company id and used id to search roles for user
                Case "ROLE"
                    strCompID = Request.QueryString("CompID").ToString
                    strUserID = Request.QueryString("UserID").ToString
                    strSQL = "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_VC50_UserID='" & strUserID & "' " & "and UM_IN4_Company_AB_ID=" & strCompID & ") and RA_DT8_Assigned_Date <='" & Today & "' and RA_DT8_Valid_UpTo >='" & Today & "' and RA_VC4_Status_Code ='ENB') "
                    If SQL.Search("T070031", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then

                    End If
                    'Get projects availabel for a selected company
                    'it will return all projects for a selected company
                    'it requires company id for search
                Case "PROJECT"
                    strCompID = Request.QueryString("CompID").ToString
                    strSQL = "select PR_NU9_Project_ID_PK  ID, PR_VC20_Name Name from T210011 where PR_NU9_Comp_ID_FK=" & Val(strCompID)
                    If SQL.Search("T210011", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then

                    End If


                Case "ROLE_AND_MEMBER"
                    Dim intMemberID As Integer = Val(Request.QueryString("MemberID"))
                    Dim intCompany As Integer = Val(Request.QueryString("CompID"))
                    strSQL = "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name from T070031 where  ROM_VC50_Status_Code_FK='ENB' and  ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_IN4_Address_No_FK=" & intMemberID & ") and RA_DT8_Assigned_Date <='" & Today & "' and RA_DT8_Valid_UpTo >='" & Today & "' and RA_VC4_Status_Code ='ENB') ;select um_in4_address_no_fk as ID,um_vc50_userid + ' [' +  T2.ci_vc36_name + ']  [' +  T1.ci_vc36_name + ']' as Name   from T060011, T010011 T1, T010011 T2 where T1.ci_nu8_address_number=um_in4_company_ab_id and T2.ci_nu8_address_number=UM_IN4_Address_No_FK and UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK<>" & intMemberID & " and  UM_IN4_Address_No_FK in  (select UC_NU9_User_ID_FK from T060041 where UC_NU9_Comp_ID_FK=" & intCompany & " and UC_BT1_Access=1)  Order By Name"
                    If SQL.Search("T070031", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then

                    End If


                Case "ROLEBYCOMP"

                    strCompID = Request.QueryString("CompID").ToString
                    strSQL = "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as RoleName from T070031 where  ROM_IN4_Company_ID_FK=" & Val(strCompID)
                    If SQL.Search("T070031", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then

                    End If
                Case "FillSession" 'For Task View Screen
                    Dim TaskNumber As Integer
                    Dim CallNo As Integer
                    Dim CompanyID As Integer

                    CallNo = Request.QueryString("CallNo")
                    TaskNumber = Request.QueryString("TaskNo")
                    CompanyID = WSSSearch.SearchCompName(Request.QueryString("Comp")).ExtraValue
                    'viewstate("CallNo") = Request.QueryString("CallNo")
                    'HttpContext.Current.Session("PropTaskNumber") = Request.QueryString("TaskNo")
                    'viewstate("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("Comp")).ExtraValue
                    Dim strStatus As String = WSSSearch.GetTaskStatus(CallNo, TaskNumber, CompanyID)
                    ViewState("TaskStatus") = strStatus
                    Dim dtStatus As New DataTable
                    dtStatus.Columns.Add("col1")
                    dtStatus.Columns.Add("col2")
                    Dim dr As DataRow
                    dr = dtStatus.NewRow
                    dr.Item(0) = strStatus
                    If ChangeAttachmentToolTip(CompanyID, CallNo) = True Then
                        dr.Item(1) = 1
                    Else
                        dr.Item(1) = 0
                    End If
                    dtStatus.Rows.Add(dr)
                    dsAjax.Tables.Add(dtStatus)
                    dsAjax.AcceptChanges()
                Case "MenuSession"
                    If Request.QueryString("ID") = "0" Then
                        HttpContext.Current.Session("PropMNU") = Request.QueryString("Name")
                    Else
                        HttpContext.Current.Session("PropSCR") = Request.QueryString("Name")
                    End If
                Case "FillCallViewSession" 'For Call View Screen
                    ViewState("CallNo") = Request.QueryString("CallNo")
                    ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("Comp")).ExtraValue
                    Dim strStatus As String = WSSSearch.GetCallStatus(Val(ViewState("CallNo")), Val(ViewState("CompanyID")))
                    ViewState("CallStatus") = strStatus
                    Dim dtStatus As New DataTable
                    dtStatus.Columns.Add("col1")
                    dtStatus.Columns.Add("col2")
                    Dim dr As DataRow
                    dr = dtStatus.NewRow
                    dr.Item(0) = strStatus
                    If ChangeAttachmentToolTip(ViewState("CompanyID"), ViewState("CallNo")) = True Then
                        dr.Item(1) = 1
                    Else
                        dr.Item(1) = 0
                    End If
                    dtStatus.Rows.Add(dr)
                    dsAjax.Tables.Add(dtStatus)
                    dsAjax.AcceptChanges()

                Case "Aggreement_Task_Owner" '    For call detail page to fetch agreement number and task owners accordin to SubCategory
                    Dim intProjectID As Integer = Val(Request.QueryString("ProjectID"))
                    ViewState("CompanyID") = Val(Request.QueryString("CompanyID"))
                    strSQL = "select AG_NU8_ID_PK as ID,AG_NU8_ID_PK Description,CI_VC36_Name 'Contact Person' from T080011 Ag,T010011 AB where ag.AG_VC8_Cust_Name =" & Val(ViewState("CompanyID")) & " and ab.CI_NU8_Address_Number = ag.AG_VC8_Contact_Person  and AG_NU9_Project_ID=" & intProjectID & " and AG_VC8_Status='ACTIVE';SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,T010011.ci_vc36_name  as Company FROM t060011,t010011,T010011 UName where T010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk  and  um_in4_address_no_fk in (select PM_NU9_Project_Member_ID from t210012 Where PM_NU9_Project_ID_FK=" & intProjectID & " and PM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & ")  and um_in4_address_no_fk in (select CI_NU8_Address_number from t010011 Where CI_VC8_Status='ENA') Order By Name;select Top 1 AG_NU8_ID_PK from T080011 where '" & Now.Date & "' between AG_dt_valid_from and AG_dt_valid_to  AND AG_VC8_Cust_Name=" & Val(ViewState("CompanyID")) & " and AG_NU9_Project_ID=" & intProjectID & "  and AG_VC8_Status='ACTIVE' Order By ag_dt_valid_from desc "
                    If SQL.Search("T080011", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then

                    End If

                Case "CallStatus"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim intProjectID As Integer = Val(Request.QueryString("ProjectID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")


                    Dim shDDLBit As Short = Request.QueryString("DDL")
                    If strPropAdmin = 1 Then  'check for the admin role on reports

                        If intCompID = 0 And intProjectID = 0 Then
                            'Requested By
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number and a.CI_VC8_Status='ENA' )   order by CompanyName , Name  ;"

                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,a.ci_vc36_name,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number and a.CI_VC8_Status='ENA')order by  d.CI_VC36_Name, a.ci_vc36_name ;"
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,a.ci_vc36_name, c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number and a.CI_VC8_Status='ENA')order by  d.CI_VC36_Name, a.ci_vc36_name ; "
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Requested By
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & " order by Name; "

                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number)where tm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA' order by   d.CI_VC36_Name,a.ci_vc36_name ;"
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name;"
                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Requested By
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " order by Name ;"
                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,tm_nu9_Comp_id_fk as compID,a.ci_vc36_name,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and a.CI_VC8_Status='ENA' order by  d.CI_VC36_Name, a.ci_vc36_name;"
                            'Employee 
                            'strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID, a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " order by    d.CI_VC36_Name,a.ci_vc36_name ;"

                            '13/Mar/08 -Changed Query to Remove Employee Redundancy
                            strSQL &= "SELECT  distinct AddressNo,Name,compID,UM_VC50_UserID from (select um_VC4_STATUS_CODE_fk,PM_NU9_Project_Member_ID as AddressNo,upper(UM_VC50_UserID) + '[' + t010011.CI_VC36_Name + ']' as Name,PM_NU9_Comp_ID_FK as compID,PM_NU9_Project_ID_Fk, UM_VC50_UserID,PM_NU9_Comp_ID_FK from T060011,T210012,t010011 where UM_IN4_Address_No_FK=PM_NU9_Project_Member_ID and Um_in4_Company_ab_Id=t010011.CI_NU8_Address_Number and PM_NU9_Comp_ID_FK=" & intCompID & ") A WHERE um_VC4_STATUS_CODE_fk='ENB' order by Name;"

                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Requested By
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " and a.CI_VC8_Status='ENA' order by Name  ;"

                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name;"
                            'Employee
                            'strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_nu9_project_id =" & intProjectID & " order by   d.CI_VC36_Name,a.ci_vc36_name; "
                            strSQL &= "SELECT  PM_NU9_Project_Member_ID as AddressNo,upper(UM_VC50_UserID) + '[' + t010011.CI_VC36_Name + ']' as Name,PM_NU9_Comp_ID_FK as compID,PM_NU9_Project_ID_Fk, UM_VC50_UserID,PM_NU9_Comp_ID_FK from T060011,T210012,t010011 where UM_IN4_Address_No_FK=PM_NU9_Project_Member_ID and Um_in4_Company_ab_Id=t010011.CI_NU8_Address_Number and PM_NU9_Comp_ID_FK=" & intCompID & " and PM_NU9_Project_ID_Fk=" & intProjectID & " and T060011.UM_VC4_Status_Code_FK='ENB';"
                        End If

                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name where PR_VC8_Status='Enable';"
                            Else
                                strSQL &= "select distinct PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as ProjName, PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID  from T210011  where PR_NU9_Comp_ID_FK='" & intCompID & "' and PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            End If

                        End If
                        If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                        End If

                    Else ' for non admin role on reports 
                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number and a.CI_VC8_Status='ENA')   order by CompanyName , Name  ;"

                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,a.ci_vc36_name,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number and a.CI_VC8_Status='ENA')order by  d.CI_VC36_Name, a.ci_vc36_name ;"
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,a.ci_vc36_name, c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_vc8_supp_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA'  order by  d.CI_VC36_Name, a.ci_vc36_name ; "
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA'  order by Name; "

                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number)where tm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA' order by   d.CI_VC36_Name,a.ci_vc36_name ;"
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & intProjectID & " and tm_vc8_supp_owner= '" & strPropUserID & "'  and a.CI_VC8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name;"
                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " order by Name ;"
                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name ,tm_nu9_Comp_id_fk as compID,a.ci_vc36_name,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & "  and a.CI_VC8_Status='ENA' order by  d.CI_VC36_Name, a.ci_vc36_name;"
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID, a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_vc8_supp_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA'  order by    d.CI_VC36_Name,a.ci_vc36_name ;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " and a.CI_VC8_Status='ENA' order by Name  ;"

                            'AssignBy
                            strSQL &= "select distinct tm_nu9_assign_by as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_nu9_assign_by=a.ci_nu8_address_number) left outer join T010011 c on tm_nu9_assign_by =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name;"
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_nu9_project_id =" & intProjectID & " and tm_vc8_supp_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA'  order by   d.CI_VC36_Name,a.ci_vc36_name; "
                        End If

                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 where PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName from ( T210011  inner join  T210012 on  PR_NU9_Project_ID_Pk=T210012.PM_NU9_Project_ID_Fk   and  PR_NU9_Comp_ID_FK=T210012.PM_NU9_Comp_ID_Fk) where PR_NU9_Comp_ID_FK=" & intCompID & "  and PM_NU9_Project_Member_ID='" & strPropUserID & "' and PR_VC8_Status='Enable';"
                            End If

                        End If
                        If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                        End If


                    End If

                Case "AddressInfo"
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")
                    If strPropAdmin = "1" And intCompID <> 0 Then
                        strSQL &= "select UM_IN4_Address_No_FK as AddressNo,upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation from t060011 where UM_IN4_Company_AB_ID='" & intCompID & "' and UM_Vc4_Status_code_Fk='ENB' order by  upper(UM_VC50_UserID) "
                        SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "")
                    Else
                        If intCompID <> 0 Then
                            strSQL &= "select UM_IN4_Address_No_FK as AddressNo,upper(UM_VC50_UserID)as name,UM_IN4_Company_AB_ID as CI_IN4_Business_Relation from t060011 where UM_IN4_Company_AB_ID='" & intCompID & "' & UM_IN4_Address_No_FK=" & strPropUserID & "  and UM_Vc4_Status_code_Fk='ENB' order by  upper(UM_VC50_UserID) "
                            SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "")
                        Else
                            dsAjax.Clear()
                            Exit Function
                        End If

                    End If





                Case "DailyAction"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim intProjectID As Integer = Val(Request.QueryString("ProjectID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")


                    Dim shDDLBit As Short = Request.QueryString("DDL")
                    If strPropAdmin = "1" Then ' check for the Admin role for the reports 
                        If intCompID = 0 And intProjectID = 0 Then

                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,a.ci_vc36_name, c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number and  a.CI_VC8_Status='ENA')order by  d.CI_VC36_Name, a.ci_vc36_name ; "
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & intProjectID & " and a.CI_VC8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name;"
                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID, a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and a.CI_VC8_Status='ENA' order by    d.CI_VC36_Name,a.ci_vc36_name ;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_nu9_project_id =" & intProjectID & "  and a.CI_VC8_Status='ENA' order by   d.CI_VC36_Name,a.ci_vc36_name; "
                        End If


                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011  where PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as ProjName, PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID  from T210011  where PR_NU9_Comp_ID_FK='" & intCompID & "' and PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            End If

                        End If
                    Else ' for non admin users for the reports .
                        If intCompID = 0 And intProjectID = 0 Then

                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,a.ci_vc36_name, c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_vc8_supp_owner ='" & strPropUserID & "' and a.CI_VC8_Status='ENA' order by  d.CI_VC36_Name, a.ci_vc36_name ; "
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where  tm_nu9_project_id =" & intProjectID & " and   tm_vc8_supp_owner ='" & strPropUserID & "' and a.CI_VC8_Status='ENA'  order by    d.CI_VC36_Name,a.ci_vc36_name;"
                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID, a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and   tm_vc8_supp_owner ='" & strPropUserID & "' and a.CI_VC8_Status='ENA'  order by    d.CI_VC36_Name,a.ci_vc36_name ;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Employee
                            strSQL &= "select distinct tm_vc8_supp_owner as  addressNo,upper(a.ci_vc36_name) + ' ['+upper(d.CI_VC36_Name) + ']' as Name,tm_nu9_Comp_id_fk as compID,tm_nu9_project_id as projectID,a.ci_vc36_name ,c.CI_VC36_Name,c.ci_nu8_address_number,c.ci_in4_business_relation,d.CI_VC36_Name from (((( T040021 left outer join T010011 a on tm_vc8_supp_owner=a.ci_nu8_address_number) left outer join T010011 c on tm_vc8_supp_owner =c.ci_nu8_address_number)left outer join T010011 d on c.ci_in4_business_relation =d.ci_nu8_address_number)left outer join T010011 b on tm_nu9_Comp_id_fk=b.ci_nu8_address_number) where tm_nu9_Comp_id_fk=" & intCompID & " and tm_nu9_project_id =" & intProjectID & " and   tm_vc8_supp_owner ='" & strPropUserID & "' and a.CI_VC8_Status='ENA' order by   d.CI_VC36_Name,a.ci_vc36_name; "
                        End If


                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 where PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName from ( T210011  inner join  T210012 on  PR_NU9_Project_ID_Pk=T210012.PM_NU9_Project_ID_Fk   and  PR_NU9_Comp_ID_FK=T210012.PM_NU9_Comp_ID_Fk) where PR_NU9_Comp_ID_FK=" & intCompID & "  and PM_NU9_Project_Member_ID='" & strPropUserID & "' and PR_VC8_Status='Enable';"
                            End If

                        End If

                    End If



                    If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                    End If






                Case "CallDetails"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim intProjectID As Integer = Val(Request.QueryString("ProjectID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")

                    Dim shDDLBit As Short = Request.QueryString("DDL")
                    If strPropAdmin = "1" Then ' check for the admin role of the reports .
                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number)  and a.CI_VC8_Status='ENA'  order by CompanyName , Name  ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and a.CI_VC8_Status='ENA' order by Name; "

                            'call no
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK =" & intCompID & ";"

                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & "   and a.CI_VC8_Status='ENA' order by Name ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "'  order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " and a.CI_VC8_Status='ENA'order by Name  ;"

                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and CM_NU9_Project_ID=" & intProjectID & " order by CM_NU9_Call_No_PK asc;"
                        End If

                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 where PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as ProjName, PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID  from T210011  where PR_NU9_Comp_ID_FK='" & intCompID & "' and PR_VC8_Status='Enable' order by PR_VC20_Name;"
                            End If

                        End If

                    Else ' check for the non admin role of the reports .
                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_call_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA'   order by CompanyName , Name  ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where cm_nu9_call_owner= '" & strPropUserID & "'   order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  where cm_nu9_call_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA'  order by Name; "

                            'call no
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK =" & intCompID & " and cm_nu9_call_owner= '" & strPropUserID & "'   ;"

                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " and cm_nu9_call_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA' order by Name ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and cm_nu9_call_owner= '" & strPropUserID & "' order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " and cm_nu9_call_owner= '" & strPropUserID & "' and a.CI_VC8_Status='ENA' order by Name  ;"

                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and CM_NU9_Project_ID=" & intProjectID & " and cm_nu9_call_owner= '" & strPropUserID & "' order by CM_NU9_Call_No_PK asc;"
                        End If

                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name and PR_VC8_Status='Enable';"
                            Else
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName from ( T210011  inner join  T210012 on  PR_NU9_Project_ID_Pk=T210012.PM_NU9_Project_ID_Fk   and  PR_NU9_Comp_ID_FK=T210012.PM_NU9_Comp_ID_Fk) where PR_NU9_Comp_ID_FK=" & intCompID & "  and PM_NU9_Project_Member_ID='" & strPropUserID & "' and PR_VC8_Status='Enable';"
                            End If

                        End If
                    End If

                    If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                    End If


                Case "CallDetails2"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim intProjectID As Integer = Val(Request.QueryString("ProjectID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")

                    Dim shDDLBit As Short = Request.QueryString("DDL")
                    If strpropadmin = 1 Then ' Check for the Admin role 

                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number)   order by CompanyName , Name  ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 order by CM_NU9_Call_No_PK desc;"
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & " order by Name; "

                            'call no
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK =" & intCompID & ";"

                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " order by Name ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "'  order by CM_NU9_Call_No_PK desc;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " order by Name  ;"

                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and CM_NU9_Project_ID=" & intProjectID & " order by CM_NU9_Call_No_PK desc;"
                        End If
                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as ProjName, PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID  from T210011  where PR_NU9_Comp_ID_FK='" & intCompID & "' order by PR_VC20_Name;"
                            End If

                        End If



                        If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                        End If

                    Else ' for non admin role of the reports 

                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where    T040011.cm_nu9_call_owner = '" & strPropUserID & "'  order by CompanyName , Name  ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where    T040011.cm_nu9_call_owner = '" & strPropUserID & "'  order by CM_NU9_Call_No_PK desc  ;"
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & " and T040011.cm_nu9_call_owner = '" & strPropUserID & "'  order by Name; "

                            'call no
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK =" & intCompID & " and  T040011.cm_nu9_call_owner = '" & strPropUserID & "'  ;"

                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " and  T040011.cm_nu9_call_owner = '" & strPropUserID & "' order by Name ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and  T040011.cm_nu9_call_owner = '" & strPropUserID & "'   order by CM_NU9_Call_No_PK desc;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " and  T040011.cm_nu9_call_owner = '" & strPropUserID & "' order by Name  ;"

                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and CM_NU9_Project_ID=" & intProjectID & " and  T040011.cm_nu9_call_owner = '" & strPropUserID & "' order by CM_NU9_Call_No_PK desc;"
                        End If
                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName from ( T210011  inner join  T210012 on  PR_NU9_Project_ID_Pk=T210012.PM_NU9_Project_ID_Fk   and  PR_NU9_Comp_ID_FK=T210012.PM_NU9_Comp_ID_Fk) where PR_NU9_Comp_ID_FK=" & intCompID & "  and PM_NU9_Project_Member_ID='" & strPropUserID & "';"
                            End If

                        End If



                        If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                        End If


                    End If



                Case "CallDetails"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim intProjectID As Integer = Val(Request.QueryString("ProjectID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")

                    Dim shDDLBit As Short = Request.QueryString("DDL")

                    If strPropAdmin = "1" Then ' check for the Admiin Role for the reports 

                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number)   order by CompanyName , Name  ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & " order by Name; "

                            'call no
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK =" & intCompID & ";"

                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " order by Name ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "'  order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " order by Name  ;"

                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and CM_NU9_Project_ID=" & intProjectID & " order by CM_NU9_Call_No_PK asc;"
                        End If

                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as ProjName, PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID  from T210011  where PR_NU9_Comp_ID_FK='" & intCompID & "' order by PR_VC20_Name;"
                            End If

                        End If
                    Else ' For Non Admin Users ( Reports )


                        If intCompID = 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number)  where cm_nu9_call_owner='" & strPropUserID & "'  order by CompanyName , Name  ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where  cm_nu9_call_owner='" & strPropUserID & "' order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID = 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & " and cm_nu9_call_owner='" & strPropUserID & "' order by Name; "

                            'call no
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK =" & intCompID & " and cm_nu9_call_owner='" & strPropUserID & "'  ;"

                        ElseIf intCompID <> 0 And intProjectID = 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name ,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_comp_id_fk=" & intCompID & " and  cm_nu9_call_owner='" & strPropUserID & "' order by Name ;"
                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and  cm_nu9_call_owner='" & strPropUserID & "'  order by CM_NU9_Call_No_PK asc;"
                        ElseIf intCompID <> 0 And intProjectID <> 0 Then
                            'Call Owner
                            strSQL &= "select distinct a.ci_nu8_address_number as addressNo, upper(a.ci_vc36_name) + ' ['+upper(b.ci_vc36_name) + ']' as Name,cm_nu9_call_owner as CallOwner ,cm_nu9_comp_id_fk as companyID ,b.ci_vc36_name as CompanyName from (( T040011 left outer join T010011 a on cm_nu9_call_owner=a.ci_nu8_address_number)left outer join T010011 b on cm_nu9_comp_id_fk=b.ci_nu8_address_number) where cm_nu9_project_id =" & intProjectID & "  and  cm_nu9_comp_id_fk=" & intCompID & " and  cm_nu9_call_owner='" & strPropUserID & "' order by Name  ;"

                            'Call No asc
                            strSQL &= "Select distinct CM_NU9_Call_No_PK  as CallNo,CM_NU9_Call_No_PK from t040011 where CM_NU9_Comp_Id_FK ='" & intCompID & "' and CM_NU9_Project_ID=" & intProjectID & " and  cm_nu9_call_owner='" & strPropUserID & "' order by CM_NU9_Call_No_PK asc;"
                        End If

                        If shDDLBit = 0 Then
                            If intCompID = 0 Then
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName  from T210011 order by PR_VC20_Name;"
                            Else
                                strSQL &= "select distinct  PR_NU9_Project_ID_Pk as ID ,PR_VC20_Name as Name,PR_NU9_Comp_ID_FK as CompanyID,PR_VC20_Name as ProjName from ( T210011  inner join  T210012 on  PR_NU9_Project_ID_Pk=T210012.PM_NU9_Project_ID_Fk   and  PR_NU9_Comp_ID_FK=T210012.PM_NU9_Comp_ID_Fk) where PR_NU9_Comp_ID_FK=" & intCompID & "  and PM_NU9_Project_Member_ID='" & strPropUserID & "';"
                            End If

                        End If


                    End If



                    If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                    End If


                Case "TimeRgs"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")

                    Dim shDDLBit As Short = Request.QueryString("DDL")


                    If strPropAdmin = "1" Then ' check for the admin role of reports 
                        If intCompID = 0 Then
                            'Task Owner
                            strSQL &= "select distinct AM_VC8_Supp_Owner as AddressNo, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name,um_vc50_userid, CI_VC36_Name   from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number and UM_VC4_Status_Code_FK='ENB'   order by CI_VC36_Name,um_vc50_userid;"

                        ElseIf intCompID <> 0 Then
                            'Task Owner
                            strSQL &= "select distinct AM_VC8_Supp_Owner as AddressNo,  upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name   from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number and AM_NU9_Comp_ID_FK='" & intCompID & "' and UM_VC4_Status_Code_FK='ENB'  order by Name ;"

                        End If
                    Else    ' for non admin role of reports .
                        If intCompID = 0 Then
                            'Task Owner
                            strSQL &= "select distinct AM_VC8_Supp_Owner as AddressNo, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name,um_vc50_userid, CI_VC36_Name   from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number and AM_VC8_Supp_Owner='" & strPropUserID & "' and UM_VC4_Status_Code_FK='ENB'  order by CI_VC36_Name,um_vc50_userid;"

                        ElseIf intCompID <> 0 Then
                            'Task Owner
                            strSQL &= "select distinct AM_VC8_Supp_Owner as AddressNo,  upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name   from T060011,T040031,T010011 where UM_IN4_Address_No_FK=AM_VC8_Supp_Owner and UM_IN4_Company_AB_ID=ci_nu8_address_number and AM_NU9_Comp_ID_FK='" & intCompID & "' and   AM_VC8_Supp_Owner='" & strPropUserID & "' and UM_VC4_Status_Code_FK='ENB'  order by Name ;"

                        End If

                    End If


                    If shDDLBit = 0 Then

                    End If


                    If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                    End If
                Case "IPTrack"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))
                    Dim strPropAdmin As String = HttpContext.Current.Session("PropAdmin")
                    Dim strPropUserID As String = HttpContext.Current.Session("PropUserID")

                    Dim shDDLBit As Short = Request.QueryString("DDL")


                    If strPropAdmin = "1" Then ' check for the admin role of reports 
                        If intCompID = 0 Then
                            'Task Owner
                            strSQL &= "select distinct UM_IN4_Address_No_FK as AddressNo, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name,um_vc50_userid, CI_VC36_Name   from T060011,T010011 where  UM_IN4_Company_AB_ID=ci_nu8_address_number  order by CI_VC36_Name,um_vc50_userid;"

                        ElseIf intCompID <> 0 Then
                            'Task Owner
                            strSQL &= "select distinct UM_IN4_Address_No_FK as AddressNo, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name,um_vc50_userid, CI_VC36_Name   from T060011,T010011 where  UM_IN4_Company_AB_ID=ci_nu8_address_number and UM_IN4_Company_AB_ID=" & intCompID & " order by CI_VC36_Name,um_vc50_userid ;"

                        End If
                    Else    ' for non admin role of reports .
                        If intCompID = 0 Then
                            'Task Owner
                            strSQL &= "select distinct UM_IN4_Address_No_FK as AddressNo, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name,um_vc50_userid, CI_VC36_Name   from T060011,T010011 where  UM_IN4_Company_AB_ID=ci_nu8_address_number and UM_IN4_Address_No_FK=" & strPropUserID & "  order by CI_VC36_Name,um_vc50_userid;"

                        ElseIf intCompID <> 0 Then
                            'Task Owner
                            strSQL &= "select distinct UM_IN4_Address_No_FK as AddressNo, upper(um_vc50_userid)   + ' ['+upper(CI_VC36_Name) + ']' as Name,um_vc50_userid, CI_VC36_Name   from T060011,T010011 where  UM_IN4_Company_AB_ID=ci_nu8_address_number and UM_IN4_Company_AB_ID=" & intCompID & "  and UM_IN4_Address_No_FK=" & strPropUserID & "  order by CI_VC36_Name,um_vc50_userid;"

                        End If

                    End If


                    If shDDLBit = 0 Then

                    End If


                    If SQL.Search("CallStatus", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                    End If

                Case "Invoices"

                    Dim intCompID As Integer = Val(Request.QueryString("CompID"))


                    Dim shDDLBit As Short = Request.QueryString("DDL")

                    If intCompID = 0 Then
                    ElseIf intCompID <> 0 Then
                        strSQL &= "select distinct IM_VC8_Invoice_Status as Status,IM_VC8_Invoice_Status as Status from t080031 where im_nu9_company_id_pk='" & intCompID & "'; "
                    End If

                    If shDDLBit = 0 Then
                    End If
                    If SQL.Search("Invoices", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then
                    End If


                Case "DomainMachine"
                    Dim intDomainID As Integer
                    intDomainID = Request.QueryString("DomainID").ToString
                    strSQL = "select  MM_NU9_MID, MM_VC150_Machine_Name  from t170012 where MM_NU9_DID_FK=" & intDomainID

                    If SQL.Search("t170012", "AjaxInfo", "GetInfo", strSQL, dsAjax, "", "") = True Then

                    End If

            End Select
            Dim dtTemp As DataTable
            strXml = "<INFO>"
            For intCount As Int16 = 0 To dsAjax.Tables.Count - 1
                dtTemp = dsAjax.Tables(intCount)
                strXml &= "<TABLE>"
                For intR As Integer = 0 To dtTemp.Rows.Count - 1
                    strXml &= "<ITEM "
                    For intC As Integer = 0 To dtTemp.Columns.Count - 1
                        strXml &= " COL" & intC & "=""" & dtTemp.Rows(intR).Item(intC) & """"
                    Next
                    strXml &= " />"
                Next
                strXml &= "</TABLE>"
            Next
            strXml &= "</INFO>"



            Response.ContentType = "text/xml"
            Response.Write(strXml)

        Catch ex As Exception
            CreateLog("AJAXinfo", "GetInfo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, User.Identity.Name, User.Identity.Name, "")
        End Try
    End Function

#End Region

End Class

