'**********************************************************************************************************
' Page                   : - AJAX Server page for Login info
' Purpose                : - Function in this page are used to get ID and Name info for Login Comp
'                            and Role  
' Date		    		Author						Modification Date					Description
' 14/07/06				Harpreet 					14/07/06	        				Created
'
''*********************************************************************************************************
Imports ION.Data
Imports ION.Logging.EventLogging
Imports ION.Net
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data

Partial Class AJAX_Server_GetLoginInfo
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call GetInfo(Request.QueryString("Type"))
    End Sub

#Region "*****---GetInfo---------*****"
    '************************************************************************
    ' Page                   : - AJAX Server page for Login info
    ' Purpose              : - This function will get the list of records in ID and Name format in a XML string  
    ' Date		    			Author						Modification Date					Description
    ' 14/07/06				Harpreet 					14/07/06	        					Created
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
            Dim strXmlComp As String
            'it stores the company id
            Dim strCompID As String
            'it stores the password
            Dim strPassword As String
            'it stores the team id
            Dim strTeamID As String
            Dim strRole As String
            'get the connection string
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString


            Select Case strType
                'Get All available companies
                'it will return all companies in XML format
                'it requires user id and password to search company for the selected user
                Case "COMP"
                    strUserID = Request.QueryString("UserID").ToString
                    strUserID = DecodeASCII(strUserID)
                    strPassword = Request.QueryString("Password").ToString
                    Dim dsUserPWD As New DataSet
                    If SQL.Search("T060011", "", "", "select  UM_IN4_Address_No_FK, UPPER( UM_VC50_UserID) UM_VC50_UserID, UM_VC30_Password from T060011", dsUserPWD, "", "") = True Then
                        For intI As Integer = 0 To dsUserPWD.Tables(0).Rows.Count - 1
                            dsUserPWD.Tables(0).Rows(inti).Item("UM_VC30_Password") = IONDecrypt(dsUserPWD.Tables(0).Rows(inti).Item("UM_VC30_Password"))
                        Next
                        dsUserPWD.AcceptChanges()
                        Dim dvTemp As New DataView
                        dvTemp = dsUserPWD.Tables(0).DefaultView
                        dvTemp.Table.CaseSensitive = True
                        GetFilteredDataView(dvTemp, "UM_VC50_UserID='" & strUserID.ToUpper & "' and UM_VC30_Password='" & strPassword & "'")
                        If dvTemp.Table.Rows.Count > 0 Then
                            Dim strWhere As String
                            For intJ As Integer = 0 To dvTemp.Table.Rows.Count - 1
                                strWhere = strWhere & dvTemp.Table.Rows(intJ).Item("UM_IN4_Address_No_FK") & ", "
                            Next
                            strWhere = strWhere.Remove(strWhere.Length - 2, 2)
                            strSQL = "select UM_IN4_Company_AB_ID as ID, CI_VC36_Name as Name from T060011,T010011 where CI_NU8_Address_Number=UM_IN4_Company_AB_ID and UM_IN4_Address_No_FK in (" & strWhere & ")"
                        Else
                            strSQL = "select CI_NU8_Address_Number as ID, CI_VC36_Name as Name, CI_VC8_Status as Status from T010011 where CI_NU8_Address_Number=0"
                        End If
                        dsUserPWD.Dispose()
                        dvTemp.Dispose()
                    End If
                    'Get All available roles for used: 
                    'it will return all roles in XML format
                    'it requires company id and used id to search roles for user
                Case "ROLE"
                    strCompID = Request.QueryString("CompID").ToString
                    strUserID = Request.QueryString("UserID").ToString
                    strUserID = DecodeASCII(strUserID)
                    strSQL = "select ROM_IN4_Role_ID_PK as ID, ROM_VC50_Role_Name as Name, ROM_VC50_Status_Code_FK as Status from T070031 where ROM_IN4_Role_ID_PK in (select RA_IN4_Role_ID_FK from t060022 where RA_IN4_AB_ID_FK = (select UM_IN4_Address_No_FK from t060011 where UM_VC50_UserID='" & strUserID & "' " & "and UM_IN4_Company_AB_ID=" & strCompID & ")  and RA_VC4_Status_Code='ENB') and ROM_VC50_Status_Code_FK='ENB'"

                Case "Status"
                    '0-->Address Book Entry Enable/Disbale
                    '1-->User Table Entry Enable/Disbale
                    '2-->User ID Expired
                    '3-->Company Enable/Disable
                    '4-->Role Enalble/Disable
                    '5-->Role Expired
                    '6-->User Assigned Role Enable/Disable
                    '7-->User Assigned Role Expired

                    strCompID = Request.QueryString("CompID").ToString
                    strUserID = Request.QueryString("UserID").ToString
                    strUserID = DecodeASCII(strUserID)
                    strUserID = WSSSearch.SearchUserIdByName(strUserID, Val(strCompID)).ExtraValue
                    strPassword = Request.QueryString("Password").ToString
                    strRole = Request.QueryString("Role").ToString

                    Dim shFlag As Short
                    shFlag = 0

                    Dim htSQL As New Hashtable

                    'Address Book Entry Enable/Disbale
                    htSQL.Add(0, "select * from T010011 where CI_VC8_Status='ENA' and CI_NU8_Address_Number=" & strUserID)
                    'User Table Entry Enable/Disbale
                    htSQL.Add(1, "select * from T060011 where UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK=" & strUserID)
                    'User ID Expired
                    htSQL.Add(2, "select * from T060011 where   isnull(UM_DT8_To_date,'" & Now.Date & "')>='" & Now.Date & "' and UM_DT8_From_date<='" & Now.Date & "'  and UM_IN4_Address_No_FK=" & strUserID)
                    'Company Enable/Disable
                    htSQL.Add(3, "select * from T010011 where CI_VC8_Status='ENA' and CI_NU8_Address_Number=" & strCompID)
                    'Role Enalble/Disable
                    htSQL.Add(4, "select * from T070031 where ROM_VC50_Status_Code_FK='ENB' and ROM_IN4_Role_ID_PK=" & strRole)
                    'Role Expired
                    htSQL.Add(5, "select * from T070031 where  ROM_DT8_End_Date>='" & Now.Date & "' and ROM_IN4_Role_ID_PK=" & strRole)
                    'User Assigned Role Enable/Disable
                    htSQL.Add(6, "select * from T060022 where  RA_VC4_Status_Code='ENB' and RA_IN4_AB_ID_FK=" & strUserID & " and RA_IN4_Role_ID_FK=" & strRole)
                    'User Assigned Role Enable/Disable
                    htSQL.Add(7, "select * from T060022 where  RA_VC4_Status_Code='ENB' and RA_IN4_AB_ID_FK=" & strUserID & " and RA_IN4_Role_ID_FK=" & strRole & " and RA_DT8_Valid_UpTo >='" & Now.Date & "'")

                    Dim intRowCount As Integer
                    Dim intMsg As Integer
                    Dim strMsg As String = "OK"
                    For intI As Integer = 0 To htSQL.Keys.Count - 1
                        If SQL.Search("", "", htSQL.Item(intI), intRowCount, "") = False Then
                            shFlag = 1
                            intMsg = intI
                            Exit For
                        End If
                    Next
                    If shFlag = 1 Then
                        Select Case intMsg
                            Case 0
                                strMsg = "Sorry, Your Profile is disabled  by the Administrator."
                            Case 1
                                strMsg = "Sorry, User ID is disabled by the Administrator."
                                'Case 2
                                '    strMsg = "User ID is expired, Please contact Administrator."
                            Case 3
                                strMsg = "Sorry, Company id disabled by the Administrator."
                            Case 4
                                strMsg = "Sorry, Role is Disabled by the Administrator."
                            Case 5
                                strMsg = "Role is expired, Please contact Administrator."
                            Case 6
                                strMsg = "User Assigned role is disabled by the Administrator."
                            Case 7
                                strMsg = "User Assigned role is expired, Please contact Administrator."
                        End Select
                    End If
                    strXmlComp = "<ITEM ID=""Status"" Name=""" & strMsg & """ />"
                    blnFound = True
                    If strMsg = "OK" Then
                        HttpContext.Current.Session("PropUserID") = strUserID
                        HttpContext.Current.Session("PropUserName") = DecodeASCII(Request.QueryString("UserID").ToString)
                        HttpContext.Current.Session("PropRole") = strRole
                        HttpContext.Current.Session("PropCompanyID") = Val(strCompID)
                        HttpContext.Current.Session("PropCompanyType") = SQL.Search("", "", "select CI_IN4_Business_Relation from T010011 where  CI_NU8_Address_Number=" & strCompID)
                        HttpContext.Current.Session("PropCompany") = WSSSearch.SearchUserName(HttpContext.Current.Session("PropCompanyID").ToString).ExtraValue
                        HttpContext.Current.Session("PropAdmin") = SQL.Search("", "", "SELECT UM_CH1_Admin_Rights  FROM T060011 WHERE UM_IN4_Address_No_FK = '" & strUserID & "'")

                    End If
            End Select
            If strType <> "Status" Then

                'sql data reader to hold the search results
                Dim sqRDR As SqlDataReader
                'boolean check to hold the reader status
                Dim blnStatus As Boolean
                sqRDR = SQL.Search("GetLoginInfo", "GetInfo", strSQL, SQL.CommandBehaviour.Default, blnStatus, "")
                If blnStatus = True Then
                    While sqRDR.Read
                        strXmlComp &= "<ITEM ID=""" & sqRDR("ID") & """  Name=""" & sqRDR("Name") & """ />"
                    End While
                    sqRDR.Close()
                    blnFound = True
                End If

            End If

            If blnFound = True Then
                strXmlComp = "<INFO>" & strXmlComp & "</INFO>"
            Else
                strXmlComp = "<INFO><ITEM ID=""""  Name="""" /></INFO>"
            End If

            Response.ContentType = "text/xml"
            Response.Write(strXmlComp)

        Catch ex As Exception
            CreateLog("AJAX Server", "GetInfo", LogType.Application, LogSubType.Exception, "", ex.Message, User.Identity.Name, User.Identity.Name, "")
        End Try
    End Function

    Private Function DecodeASCII(ByVal ASCII As String) As String
        Try
            Dim arUserID() As String
            arUserID = ASCII.Split(",")
            ASCII = ""
            For intI As Integer = 0 To arUserID.Length - 1
                If arUserID(intI).Equals("") = False Then
                    ASCII &= Convert.ToChar(CType(arUserID(intI), Integer)).ToString
                End If
            Next
            Return ASCII
        Catch ex As Exception
            CreateLog("AJAX Server", "DecodeASCII", LogType.Application, LogSubType.Exception, "", ex.Message, User.Identity.Name, User.Identity.Name, "")
        End Try
    End Function

#End Region

End Class
