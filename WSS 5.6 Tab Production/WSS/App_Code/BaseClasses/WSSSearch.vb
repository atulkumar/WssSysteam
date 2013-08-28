Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Data

Public Class WSSSearch

#Region " UDC "

    Public Shared Function SearchUDCType(ByVal ProductCode As Integer, ByVal UDCType As String) As ReturnValue
        Dim stReturn As ReturnValue
        Dim sqrdCheck As SqlClient.SqlDataReader
        Dim blnCheck As Boolean
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        SQL.DBConnection = strConnection
        ' SQL.DBTable = "UDCType"
        SQL.DBTracing = False

        Try
            'Check for existing UDCType value in the table
            sqrdCheck = SQL.Search("WSSSearch", "SearchUDCType-20", "select * from UDCType where ProductCode=" & ProductCode & " and UDCType='" & UDCType & "'", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = "UDCType already exists..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUDCType-37", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Public Shared Function SearchUDC(ByVal ProductCode As Integer, ByVal UDCType As String, ByVal UDCName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Dim sqrdCheck As SqlClient.SqlDataReader
        Dim blnCheck As Boolean
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        SQL.DBConnection = strConnection
        'SQL.DBTable = "UDC"
        SQL.DBTracing = False

        Try
            'Check for existing UDCType value in the table
            sqrdCheck = SQL.Search("WSSSearch", "SearchUDC-54", "select * from UDC where ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' and Name='" & UDCName & "'", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = "UDC Exist in the database..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUDC-71", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " AB_Main "

    Public Shared Function SearchPersonalInfo(ByVal AddressNo As Integer, ByRef PersonalInfoReader As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T010043"
            SQL.DBTracing = False

            PersonalInfoReader = SQL.Search("WSSSearch", "SearchPersonalInfo-88", "select * from T010043 where PI_NU8_Address_No=" & AddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchPersonalInfo-105", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchUserInfo(ByVal AddressNo As Integer, ByRef PersonalInfoReader As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T010043"
            SQL.DBTracing = False

            PersonalInfoReader = SQL.Search("WSSSearch", "SearchUserInfo-119", "select * from T010011 where CI_NU8_Address_Number=" & AddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUserInfo-136", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchCategoryInfo(ByVal AddressNo As Integer, ByRef CategoryReader As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T010053"
            SQL.DBTracing = False

            CategoryReader = SQL.Search("WSSSearch", "SearchCategoryInfo-151", "select * from T010053 where CC_NU8_Address_No=" & AddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchCategoryInfo-167", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " CreateUser "

    Public Shared Function SearchUserName(ByVal AddressNo As Integer, ByVal UserName As String, ByVal Company As String) As ReturnValue
        Dim stReturn As ReturnValue
        Dim sqrdCheck As SqlClient.SqlDataReader
        Dim blnCheck As Boolean
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        SQL.DBConnection = strConnection
        ' SQL.DBTable = "T010061"
        SQL.DBTracing = False

        Try
            sqrdCheck = SQL.Search("WSSSearch", "SearchUserName-187", "select UL_VC36_User_Name_PK from T010061 where UL_NU8_Address_No_FK=" & AddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = "User Name exist in the database."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUserName-207", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try
    End Function

    Public Shared Function SearchAddressNo(ByVal AddressNo As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim sqrdCheck As SqlClient.SqlDataReader
        Dim blnCheck As Boolean
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        SQL.DBConnection = strConnection
        'SQL.DBTable = "T010061"
        SQL.DBTracing = False

        Try

            sqrdCheck = SQL.Search("WSSSearch", "SearchAddressNo-221", "select UL_NU8_Address_No_FK from T010061 where UL_NU8_Address_No_FK=" & AddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = "User Name exist in the database."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchAddressNo-241", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try
    End Function

    Public Shared Function SearchAddressNo(ByVal AddressNo As Integer, ByVal Role As String, ByVal Company As String, ByRef HoldRecords As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        'Dim sqrdCheck As SqlClient.SqlDataReader
        Dim blnCheck As Boolean
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        SQL.DBConnection = strConnection
        ' SQL.DBTable = "T010061"
        SQL.DBTracing = False

        Try

            HoldRecords = SQL.Search("WSSSearch", "SearchAddressNo-255", "select * from T010061 where  UL_NU8_Address_No_FK=" & AddressNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "User not found in database"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchAddressNo-275", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try
    End Function

#End Region

#Region " Call, Task, Action "

    Public Shared Function SearchCall(ByVal Company As String, ByVal HoldInformation As DataSet) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '   SQL.DBTable = "T040011"
            SQL.DBTracing = False

            If SQL.Search("T040011", "WSSSearch", "SearchCall-289", "select top 15  CM_NU9_Call_No_PK,CI_VC36_Name,CM_VC8_Call_Type,CM_VC2000_Call_Desc,CM_VC200_Work_Priority,CN_VC20_Call_Status from T040011, T010011 b where T040011.CM_NU9_Comp_Id_FK=b.CI_NU8_Address_Number and cn_VC20_Call_Status<>'CLOSED'  and t040011.cm_nu9_comp_id_fk=" & Company & " order by CM_NU9_Call_No_PK desc ", HoldInformation, "", "") = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "No Calls opened so far"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchCall-307", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchCall(ByVal CallNo As Integer, ByVal Company As String, ByRef PersonalInfoReader As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '  SQL.DBTable = "T040011"
            SQL.DBTracing = False
            'Modified date 21/12/06
            'Chnaged the join from T010011 to T060011 so that user id can be displayed as call owner
            'instead of address book name
            PersonalInfoReader = SQL.Search("WSSSearch", "SearchCall-318", "select T1.UM_VC50_UserID as CI_VC36_Name, T2.UM_VC50_UserID as Coordinator, * from T040011,T060011 T1, T060011 T2 where T1.UM_IN4_Address_No_FK=CM_NU9_Call_Owner And T2.UM_IN4_Address_No_FK=* CM_NU9_Coordinator And CM_NU9_Comp_Id_FK=" & Company & " and CM_NU9_Call_No_PK=" & CallNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchCall-338", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

	Public Shared Function SearchTask(ByVal UserID As Integer, ByVal Company As String, ByVal HoldTasks As DataSet) As ReturnValue
		Dim stReturn As ReturnValue
		Dim blnCheck As Boolean
		Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T040021"
            SQL.DBTracing = False

            If SQL.Search("T040021", "WSSSearch", "SearchTask-349", "select top 15 TM_NU9_Call_No_FK,TM_NU9_Task_no_PK,CI_VC36_Name, TM_VC1000_Subtsk_Desc, b.UM_VC50_UserID TM_VC8_Supp_Owner from T040021 a,T060011 b,T010011 c where a.TM_NU9_Comp_Id_FK=c.CI_NU8_Address_Number and a.TM_VC8_Supp_Owner =b.UM_IN4_Address_No_FK and TM_VC8_Supp_Owner=" & UserID & " and TM_VC50_Deve_Status<>'Closed' ", HoldTasks, "", "") = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "No task assigned"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchTask-400", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchTask(ByVal TaskNo As Integer, ByVal CallNo As Integer, ByVal compID As Integer, ByRef PersonalInfoReader As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T040011"
            SQL.DBTracing = False

            PersonalInfoReader = SQL.Search("WSSSearch", "SearchTask-378", "select * from T040021 where TM_NU9_Task_no_PK=" & TaskNo & " and TM_NU9_Call_No_FK=" & CallNo & " and tm_nu9_comp_id_fk=" & compID, SQL.CommandBehaviour.CloseConnection, blnCheck)


            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records found in the database"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchTask-432", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

	Public Shared Function SearchTaskStatus(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As ReturnValue
		Dim stReturn As ReturnValue

		Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T040011"
            SQL.DBTracing = False

            Dim intTaskNumber As Integer = SQL.Search("WSSSearch", "SearcTaskStatus-410", "select TM_NU9_Task_no_PK from T040021 where TM_VC50_Deve_status='PROGRESS'  and TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK=" & TaskNumber & " and tm_nu9_comp_id_fk=" & compID)

            If intTaskNumber > 0 Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records found in the database"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchTaskStatus-463", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try
    End Function
    '''''''This function will return the status of the task''
    '''''''Created by Harpreet
    '''''''30/06/2006
    Public Shared Function GetTaskStatus(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '   SQL.DBTable = "T040021"
            SQL.DBTracing = False
            Dim strTaskStatus As String = SQL.Search("WSSSearch", "SearcTaskStatus-410", "select TM_VC50_Deve_status from T040021 where  TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK=" & TaskNumber & " and tm_nu9_comp_id_fk=" & compID)
            Return strTaskStatus
        Catch ex As Exception
            CreateLog("WWSSearch", "GetTaskStatus-442", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return ""
        End Try
    End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''This function will return the status of the call''
    '''''''Created by Harpreet
    '''''''11/07/2006
    Public Shared Function GetCallStatus(ByVal CallNumber As Integer, ByVal compID As Integer) As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T040011"
            SQL.DBTracing = False
            Dim strCallStatus As String = SQL.Search("WSSSearch", "SearcTaskStatus-410", "select CN_VC20_Call_Status from T040011 where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & compID)
            Return strCallStatus
        Catch ex As Exception
            CreateLog("WWSSearch", "GetCallStatus-458", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return ""
        End Try
    End Function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Public Shared Function SearchAction(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '        SQL.DBTable = "T040011"
            SQL.DBTracing = False
            Dim strSQL As String
            Dim strMandatory As String = SQL.Search("WSSSearch", "SearchAction", "select TM_CH1_Mandatory from T040021 where TM_NU9_Comp_ID_FK=" & compID & " and TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK=" & TaskNumber)
            If strMandatory.Equals("M") Then
                strSQL = "select AM_NU9_Action_Number from T040031 where AM_CH1_Mandatory='M' and AM_NU9_Task_Number=" & TaskNumber & " and AM_NU9_Call_Number=" & CallNumber & " and AM_NU9_Comp_ID_FK=" & compID

                Dim intTaskNumber As Integer = SQL.Search("WSSSearch", "SearchAction-440", strSQL)

                If intTaskNumber > 0 Then
                    stReturn.ErrorMessage = ""
                    stReturn.ErrorCode = 0
                    stReturn.FunctionExecuted = True
                Else
                    stReturn.ErrorMessage = "There are no records found in the database"
                    stReturn.ErrorCode = 1
                    stReturn.FunctionExecuted = False
                End If
            Else
                'if actions are optional for a task then there is no need for searching actions.
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchAction-493", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchCallOwner(ByVal CallNumber As Integer, ByVal compID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean

        If CallNumber = 0 Then
            stReturn.ErrorMessage = ""
            stReturn.ErrorCode = 1
            stReturn.FunctionExecuted = False
            Return stReturn
        End If

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T040011"
            SQL.DBTracing = False

            Dim sqrdCall As SqlClient.SqlDataReader = SQL.Search("WSSSearch", "SearchCallOwner-479", "select CM_VC100_By_Whom,CM_NU9_Call_Owner from T040011 where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & compID, SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                While sqrdCall.Read
                    If HttpContext.Current.Session("PropUserID") = sqrdCall.Item("CM_VC100_By_Whom") Or HttpContext.Current.Session("PropUserID") = sqrdCall.Item("CM_NU9_Call_Owner") Then
                        stReturn.ErrorMessage = ""
                        stReturn.ErrorCode = 0
                        stReturn.FunctionExecuted = True
                    Else
                        stReturn.ErrorMessage = ""
                        stReturn.ErrorCode = 1
                        stReturn.FunctionExecuted = False
                    End If
                End While

                sqrdCall.Close()
            Else
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchCallOwner-542", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchTaskOwner(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As ReturnValue
        Dim stReturn As ReturnValue

        If CallNumber = 0 Then
            stReturn.ErrorMessage = ""
            stReturn.ErrorCode = 1
            stReturn.FunctionExecuted = False
            Return stReturn
        End If

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            ' SQL.DBTable = "T040021"
            SQL.DBTracing = False

            stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchTaskOwner-527", "select TM_VC8_Supp_Owner from T040021 where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_No_Pk=" & TaskNumber & " and TM_NU9_Comp_ID_FK=" & compID)

            If IsNothing(stReturn.ExtraValue) = False Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchTaskOwner-580", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

    'Function to check whether action are optional or mandatory for Task
    Public Shared Function SearchTasKMandatory(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '        SQL.DBTable = "T040011"
            SQL.DBTracing = False
            Dim strSQL As String
            Dim strMandatory As String = ""
            strMandatory = SQL.Search("WSSSearch", "SearchAction", "select TM_CH1_Mandatory from T040021 where TM_NU9_Comp_ID_FK=" & compID & " and TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK=" & TaskNumber)
            If IsNothing(strMandatory) = False Then
                If strMandatory.Equals("M") Then
                    'if actions areMandatory for a task 
                    stReturn.ErrorMessage = ""
                    stReturn.ErrorCode = 1
                    stReturn.FunctionExecuted = False
                Else
                    'if actions are optional for a task then there is no need for searching actions.
                    stReturn.ErrorMessage = ""
                    stReturn.ErrorCode = 0
                    stReturn.FunctionExecuted = True
                End If
            Else
                'if actions are optional for a task then there is no need for searching actions.
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database"
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchTasKMandatory-617", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function

#End Region

#Region " User Name "

    Public Shared Function SearchUserName(ByVal AddressNO As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '  SQL.DBTable = "T040011"
            SQL.DBTracing = False

            stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchUserName-564", "select CI_VC36_Name from T010011 where CI_NU8_Address_Number=" & AddressNO & "")

            If stReturn.ExtraValue <> "" Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUserName-617", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

#End Region

#Region " User Name "

    Public Shared Function SearchCompName(ByVal AddressName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '  SQL.DBTable = "T040011"
            SQL.DBTracing = False

            stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchCompName-599", "select CI_NU8_Address_Number from T010011 where CI_VC36_Name like '" & AddressName & "'")
            If IsNothing(stReturn.ExtraValue) = False Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                'if the address number to the corresponding address name is not found in T010011 then
                'id is searched in T250021 corresponding to address name
                'added on 17-01-2012 by Pooja Kanwar
                stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchCompanyID-599", "select FD_NU9_Company_ID_FK from T250021 where FD_VC255_Folder_Name like'" & AddressName & "'")
                If IsNothing(stReturn.ExtraValue) = False Then
                    stReturn.ErrorMessage = ""
                    stReturn.ErrorCode = 0
                    stReturn.FunctionExecuted = True
                Else
                    stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                    stReturn.ErrorCode = 1
                    stReturn.FunctionExecuted = False
                End If
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchCompName-651", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

#End Region

#Region " User Name ID "

    Public Shared Function SearchCompNameID(ByVal AddressID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '  SQL.DBTable = "T040011"
            SQL.DBTracing = False

            stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchCompID-633", "select CI_VC36_Name from T010011 where  CI_NU8_Address_Number= " & AddressID & "")
            If IsNothing(stReturn.ExtraValue) = False Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchCompNameID-685", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

#End Region

#Region " User ID Name(T060011) "

    Public Shared Function SearchUserID(ByVal AddressNO As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            ' SQL.DBTable = "T040011"
            SQL.DBTracing = False

            stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchUserID-667", "select UM_VC50_UserID from T060011 where UM_IN4_Address_No_FK=" & AddressNO & "")

            If stReturn.ExtraValue <> "" Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUserID-720", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

    Public Shared Function SearchUserIdByName(ByVal strUserID As String, ByVal intCompID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False

            stReturn.ExtraValue = SQL.Search("WSSSearch", "SearchUserIdByName-667", "select UM_IN4_Address_No_FK from T060011 where UM_VC50_UserID='" & strUserID & "' and UM_IN4_Company_AB_ID=" & intCompID)

            If stReturn.ExtraValue <> Nothing Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Address Number..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Error occured while transacting with Database..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchUserIdByName-720", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

#End Region

#Region " Comment "

    '''''Public Shared Function SearchComment(ByVal CallNumber As Integer, ByVal TaskNo As Integer, ByVal compID As Integer, ByRef CommentInfoReader As SqlClient.SqlDataReader, Optional ByVal OPT As Integer = 0, Optional ByVal TemplateId As Integer = 0, Optional ByVal ActionNo As Integer = 0) As ReturnValue
    '''''    Dim stReturn As ReturnValue
    '''''    Dim sqrdCheck As SqlClient.SqlDataReader
    '''''    Dim blnCheck As Boolean
    '''''    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    '''''    Dim strTable As String
    '''''    If OPT = 2 Then
    '''''        strTable = "T050061"
    '''''    Else
    '''''        strTable = "T040061"
    '''''    End If
    '''''    SQL.DBConnection = strConnection
    '''''    ' SQL.DBTable = strTable
    '''''    SQL.DBTracing = False

    '''''    Try
    '''''        'Check for existing UDCType value in the table
    '''''        If HttpContext.Current.Session("PropCompanyType") = "SCM" Then
    '''''            If OPT = 2 Then
    '''''                CommentInfoReader = SQL.Search("WSSSearch", "SearchComment-712", "select * from " & strTable & " where CM_NU9_Call_Number=" & CallNumber & " and CM_NU9_Task_Number=" & TaskNo & " and CM_NU9_TemplateID_FK=" & TemplateId & " and  CM_NU9_CompId_Fk=" & compID & " order by CM_DT8_Date desc", SQL.CommandBehaviour.CloseConnection, blnCheck)
    '''''            Else
    '''''                CommentInfoReader = SQL.Search("WSSSearch", "SearchComment-714", "select * from " & strTable & " where CM_NU9_Call_Number=" & CallNumber & " and CM_NU9_Task_Number=" & TaskNo & " and CM_NU9_Action_Number =" & ActionNo & " and CM_NU9_CompId_Fk=" & compID & " order by CM_DT8_Date desc", SQL.CommandBehaviour.CloseConnection, blnCheck)
    '''''            End If
    '''''        Else
    '''''            If OPT = 2 Then
    '''''                CommentInfoReader = SQL.Search("WSSSearch", "SearchComment-712", "select * from " & strTable & " where CM_NU9_Call_Number=" & CallNumber & " and CM_NU9_Task_Number=" & TaskNo & " and CM_NU9_TemplateID_FK=" & TemplateId & " and  CM_NU9_CompId_Fk=" & compID & " and  CM_VC50_IE='External'  order by CM_DT8_Date desc", SQL.CommandBehaviour.CloseConnection, blnCheck)
    '''''            Else
    '''''                CommentInfoReader = SQL.Search("WSSSearch", "SearchComment-714", "select * from " & strTable & " where CM_NU9_Call_Number=" & CallNumber & " and CM_NU9_Task_Number=" & TaskNo & " and CM_NU9_Action_Number =" & ActionNo & " and CM_NU9_CompId_Fk=" & compID & "  and  CM_VC50_IE='External'  order by CM_DT8_Date desc", SQL.CommandBehaviour.CloseConnection, blnCheck)
    '''''            End If
    '''''        End If
    '''''        If blnCheck = True Then
    '''''            'stReturn.ErrorMessage = "UDCType already exist."
    '''''            stReturn.ErrorCode = 0
    '''''            stReturn.FunctionExecuted = True
    '''''        Else
    '''''            stReturn.ErrorMessage = ""
    '''''            stReturn.ErrorCode = 1
    '''''            stReturn.FunctionExecuted = False
    '''''        End If

    '''''        Return stReturn
    '''''    Catch ex As Exception
    '''''        stReturn.ErrorMessage = "Server is busy please try later...."
    '''''        stReturn.ErrorCode = 2
    '''''        stReturn.FunctionExecuted = False
    '''''        CreateLog("WWSSearch", "SearchComment-768", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
    '''''        Return stReturn
    '''''    End Try
    '''''End Function

    ''''''Public Shared Function SearchComment(ByVal CallNumber As Integer, ByVal TaskNo As Integer, ByVal ActionNo As Integer, ByVal compID As Integer, ByRef CommentInfoReader As SqlClient.SqlDataReader) As ReturnValue

    ''''''    Dim stReturn As ReturnValue
    ''''''    Dim sqrdCheck As SqlClient.SqlDataReader
    ''''''    Dim blnCheck As Boolean
    ''''''    Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

    ''''''    SQL.DBConnection = strConnection
    ''''''    '  SQL.DBTable = "T040061"
    ''''''    SQL.DBTracing = False

    ''''''    Try
    ''''''        'Check for existing UDCType value in the table

    ''''''        If HttpContext.Current.Session("PropCompanyType") = "SCM" Then
    ''''''            CommentInfoReader = SQL.Search("WSSSearch", "SearchComment-750", "select * from T040061 where CM_NU9_Call_Number=" & CallNumber & " and CM_NU9_Task_Number=" & TaskNo & " and CM_NU9_Action_Number=" & ActionNo & " and CM_NU9_CompId_Fk=" & compID & " order by CM_DT8_Date desc", SQL.CommandBehaviour.CloseConnection, blnCheck)
    ''''''        Else
    ''''''            CommentInfoReader = SQL.Search("WSSSearch", "SearchComment-750", "select * from T040061 where CM_NU9_Call_Number=" & CallNumber & " and CM_NU9_Task_Number=" & TaskNo & " and CM_NU9_Action_Number=" & ActionNo & " and CM_NU9_CompId_Fk=" & compID & " and  CM_VC50_IE='External' order by CM_DT8_Date desc", SQL.CommandBehaviour.CloseConnection, blnCheck)
    ''''''        End If


    ''''''        If blnCheck = True Then
    ''''''            'stReturn.ErrorMessage = "UDCType already exist."
    ''''''            stReturn.ErrorCode = 0
    ''''''            stReturn.FunctionExecuted = True
    ''''''        Else
    ''''''            stReturn.ErrorMessage = ""
    ''''''            stReturn.ErrorCode = 1
    ''''''            stReturn.FunctionExecuted = False
    ''''''        End If

    ''''''        Return stReturn
    ''''''    Catch ex As Exception
    ''''''        stReturn.ErrorMessage = "Server is busy please try later..."
    ''''''        stReturn.ErrorCode = 2
    ''''''        stReturn.FunctionExecuted = False
    ''''''        CreateLog("WWSSearch", "SearchComment-802", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
    ''''''        Return stReturn
    ''''''    End Try
    ''''''End Function

#End Region

#Region "Template Call/Task/Action"
    Public Shared Function SearchTemplateCall(ByVal TemplateNo As Integer, ByVal Company As String, ByRef PersonalInfoReader As SqlClient.SqlDataReader) As ReturnValue
        Dim stReturn As ReturnValue
        Dim blnCheck As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T050021"
            SQL.DBTracing = False

            PersonalInfoReader = SQL.Search("WSSSearch", "SearchTemplateCall-784", "select b.CI_VC36_Name,c.UM_VC50_UserID, d.UM_VC50_UserID Coordinator, a.* from T050021 a, T010011 b, T060011 c, T060011 d where a.TCM_NU9_CompId_FK =b.CI_NU8_Address_Number AND a.TCM_NU9_Call_Owner=c.UM_IN4_Address_No_FK  And a.TCM_NU9_Coordinator *= d.UM_IN4_Address_No_FK  And TCM_NU9_TemplateID_FK =" & TemplateNo & "", SQL.CommandBehaviour.CloseConnection, blnCheck)

            If blnCheck = True Then
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
            Else
                stReturn.ErrorMessage = "There are no records in the database for the Template..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.ErrorCode = 2
            stReturn.FunctionExecuted = False
            CreateLog("WWSSearch", "SearchTemplateCall-836", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try

    End Function
#End Region

#Region " Change Management"
    Public Shared Function GetNoOfAssignedForms(ByVal TaskType As String, ByVal Template As Boolean, ByVal companyID As Integer, ByVal addressNumber As Integer, ByVal callNumber As Integer) As Integer
        Dim stReturn As ReturnValue
        Dim NoofFormsAttached As Integer = 0
        Dim blnCheck As Boolean
        Dim strSql As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SQL.DBTable = "T110022"
            SQL.DBTracing = False
            If Template = False Then
                strSql = " Select isnull(count(FT_IN4_Type_No),0) From T110022 A,T040011 B  " & _
                        " Where(FT_VC8_Call_Type = CM_VC8_Call_Type) " & _
                        " And FT_VC8_Task_Type='" & TaskType & "'" & _
                        " and FT_IN4_Comp_id=" & companyID & _
                        " AND CM_NU9_Call_No_Pk = " & callNumber
            Else
                strSql = " Select isnull(count(FT_IN4_Type_No),0) From T110022 A,T050011 B  " & _
                                        " Where(FT_VC8_Call_Type = TL_VC8_Call_Type) " & _
                                        " And FT_VC8_Task_Type='" & TaskType & "'" & _
                                        " and FT_IN4_Comp_id=" & companyID & _
                                        " AND TL_NU9_ID_PK=" & addressNumber
            End If


            NoofFormsAttached = SQL.Search("WSSSearch", "GetNoOfAssignedForms-832", strSql)

            Return NoofFormsAttached
        Catch ex As Exception
            CreateLog("WWSSearch", "GetNoOfAssignedForms-849", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return 0
        End Try
    End Function
#End Region

#Region " SubCategory "

    Public Shared Function SearchProjectID(ByVal intCallNumber As Integer, ByVal intCompID As Integer, Optional ByVal blnTemplate As Boolean = False) As Integer
        'When callled from template call number will be treated as template id
        Try
            Dim strSQL As String
            Dim intProjectID As Integer
            If blnTemplate = False Then
                strSQL = "select CM_NU9_Project_ID from t040011 where CM_NU9_call_No_PK=" & intCallNumber & " and CM_NU9_Comp_ID_FK=" & intCompID
            Else
                strSQL = "select TL_NU9_ProjectID_FK from T050011 where TL_NU9_CustID_FK=" & intCompID & " and TL_NU9_ID_PK=" & intCallNumber
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            '  SQL.DBTable = "T040011"
            SQL.DBTracing = False
            intProjectID = SQL.Search("WssSearch", "SearchProjectId", strSQL)
            Return intProjectID
        Catch ex As Exception
            CreateLog("WWSSearch", "SearchProjectID-881", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Public Shared Function SearchProjectName(ByVal intProjectID As Integer, ByVal intCompID As Integer) As String
        Try
            Dim strSQL As String
            Dim strProjectName As String
            strSQL = "select PR_VC20_Name from T210011 where PR_NU9_Project_ID_PK=" & intProjectID & " and PR_NU9_Comp_ID_FK=" & intCompID
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            ' SQL.DBTable = "T210011"
            SQL.DBTracing = False
            strProjectName = SQL.Search("Wsssearch", "SearchProjectName-911", strSQL)
            Return strProjectName
        Catch ex As Exception
            CreateLog("WWSSearch", "SearchProjectName-898", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Public Shared Function IsProjectInUse(ByVal intProjectID As Integer, ByVal intCompID As Integer) As Boolean
        'This function will tell whether the SubCategory is in use or not
        Dim strSQL As String
        Dim boolInUse As Boolean
        Dim intCountRows As Int32
        Try

            'Check Whether the SubCategory is already used in Calls
            'SQL.DBTable = "T040011"
            strSQL = "Select isnull(count(*),0) From T040011 Where CM_NU9_Comp_Id_FK= " & intCompID & "And CM_NU9_Project_ID= " & intProjectID
            If SQL.Search("ProjectMasterDetail", "DeleteMembers-906", strSQL, intCountRows) = True Then
                If intCountRows > 0 Then
                    Return True
                End If
            End If

            'Check Whether the SubCategory is already used in TemplateCalls
            '   SQL.DBTable = "T050021"
            strSQL = "Select isnull(count(*),0) From T050021 Where TCM_NU9_CompId_FK= " & intCompID & "And TCM_NU9_Project_ID= " & intProjectID
            If SQL.Search("ProjectMasterDetail", "DeleteMembers-916", strSQL, intCountRows) = True Then
                If intCountRows > 0 Then
                    Return True
                End If
            End If
            Return False
        Catch ex As Exception
            CreateLog("WWSSearch", "IsProjectInUse-920", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Public Shared Function IsProjectMemberInUse(ByVal intMemberID As Integer, ByVal intProjectID As Integer, ByVal intCompID As Integer) As Boolean
        'This function will tell whether the SubCategory member is in use or not
        Dim blnFound As Boolean = False
        Dim strSQL As String
        Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        Try

            strSQL = "select top 1 TM_VC8_Supp_Owner from T040021, T040011 where  TM_NU9_Comp_ID_FK=CM_NU9_Comp_ID_FK and TM_NU9_Call_No_FK=CM_NU9_Call_No_PK and TM_NU9_Comp_ID_FK=" & intCompID & " and TM_VC8_Supp_Owner=" & intMemberID & "  and CM_NU9_Project_ID=" & intProjectID & " and TM_VC50_Deve_status !='CLOSED';select top 1 TTM_VC8_Supp_Owner from T050031, T050011 where TL_NU9_ID_PK=TTM_NU9_TemplateID_FK and TL_NU9_CustID_FK=TTM_NU9_Comp_ID_FK and TTM_NU9_Comp_ID_FK=" & intCompID & " and TTM_VC8_Supp_Owner=" & intMemberID & "  and TL_NU9_ProjectID_FK=" & intProjectID


            If sqCon.State <> ConnectionState.Open Then
                sqCon.Open()
            End If
            Dim dsABNum As New DataSet
            Dim sqADP As New SqlClient.SqlDataAdapter(strSQL, sqCon)
            sqADP.Fill(dsABNum)

            If dsABNum.Tables.Count > 0 Then
                For intI As Integer = 0 To dsABNum.Tables.Count - 1
                    If dsABNum.Tables(intI).Rows.Count > 0 Then
                        blnFound = True
                        Exit For
                    End If
                Next
            End If
            sqCon.Close()
            sqADP.Dispose()
            dsABNum.Dispose()
            Return blnFound
        Catch ex As Exception
            CreateLog("WWSSearch", "IsProjectMemberInUse-963", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region " Search Address Book Number"
    Public Enum ABNumType
        User = 1
        Company = 2
        Both = 3
    End Enum

    Public Shared Function SearchABNumber(ByVal intABNo As Integer, Optional ByVal enuABNumType As ABNumType = ABNumType.Company) As Boolean
        Try
            Dim blnFound As Boolean = False
            Dim strSQL As String
            Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)

            If enuABNumType = ABNumType.User Then
                strSQL = "select TOP 1 CM_VC100_By_Whom from T040011 where CM_VC100_By_Whom=" & intABNo & "; select TOP 1 CM_NU9_Call_Owner from T040011 where CM_NU9_Call_Owner=" & intABNo & "; select TOP 1 TM_VC8_Supp_Owner from T040021 where TM_VC8_Supp_Owner=" & intABNo & "; select TOP 1 TM_NU9_Assign_By from T040021 where TM_NU9_Assign_By=" & intABNo & "; select TOP 1 AM_VC8_Supp_Owner from T040031 where AM_VC8_Supp_Owner=" & intABNo & "; select TOP 1 TCM_VC100_By_Whom from T050021 where TCM_VC100_By_Whom=" & intABNo & "; select TOP 1 TCM_NU9_Call_Owner from T050021 where TCM_NU9_Call_Owner=" & intABNo & "; select TOP 1 TTM_VC8_Supp_Owner from T050031 where TTM_VC8_Supp_Owner=" & intABNo & "; select TOP 1 TTM_NU9_Assign_By from T050031 where TTM_NU9_Assign_By=" & intABNo & "; select TOP 1 RA_IN4_AB_ID_FK from T060022 where RA_IN4_AB_ID_FK=" & intABNo & "; select TOP 1 RA_IN4_Assigned_By_FK from T060022 where RA_IN4_Assigned_By_FK=" & intABNo & "; select TOP 1 RA_IN4_Inserted_By_FK from T060022 where RA_IN4_Assigned_By_FK=" & intABNo & "; select TOP 1 PR_NU9_Owner_ID_FK from T210011 where PR_NU9_Owner_ID_FK=" & intABNo & "; select TOP 1 PR_NU9_Project_CreatedBy_FK from T210011 where PR_NU9_Project_CreatedBy_FK=" & intABNo & "; select TOP 1 PR_NU9_Project_ModifiedBy_FK from T210011 where PR_NU9_Project_ModifiedBy_FK=" & intABNo & "; select TOP 1 PM_NU9_Project_Member_ID from T210012 where PM_NU9_Project_Member_ID=" & intABNo & "; select TOP 1 PM_NU9_Reports_To from T210012 where PM_NU9_Reports_To=" & intABNo & "; select TOP 1 AG_VC8_Contact_Person from T080011 where AG_VC8_Contact_Person=" & intABNo & "; select TOP 1 IM_NU9_Invoice_Created_By from T080031 where IM_NU9_Invoice_Created_By=" & intABNo & "; select TOP 1 IM_VC8_Invoice_Reference from T080031 where IM_VC8_Invoice_Reference=" & intABNo & "; select TOP 1 user_ID from Setup_Rules where user_ID=" & intABNo & "; select TOP 1 Specific_User_ID from Setup_Rules where Specific_User_ID=" & intABNo & "; select TOP 1 Inserted_By_User_ID from Setup_Rules where Inserted_By_User_ID=" & intABNo & "; select TOP 1 Last_Modified_By_User_ID from Setup_Rules where Last_Modified_By_User_ID=" & intABNo & "; select TOP 1 FD_IN4_User1 from T100011 where FD_IN4_User1=" & intABNo & "; select TOP 1 FD_IN4_Inserted_By from T100011 where FD_IN4_Inserted_By=" & intABNo & "; "


            ElseIf enuABNumType = ABNumType.Company Then
                strSQL = "select TOP 1 UM_IN4_Company_AB_ID from T060011 where UM_IN4_Company_AB_ID=" & intABNo & "; select TOP 1 AG_VC8_Cust_Name from T080011 where AG_VC8_Cust_Name=" & intABNo & "; select TOP 1 Company_id from Setup_Rules where Company_id=" & intABNo & "; select TOP 1 UV_NU9_Comp_ID from T030201 where UV_NU9_Comp_ID=" & intABNo & "; select TOP 1 IM_NU9_Company_ID_PK from T080031 where IM_NU9_Company_ID_PK=" & intABNo & "; select TOP 1 RA_IN4_AB_ID_Fk from T060022 where RA_IN4_AB_ID_Fk=" & intABNo & "; select TOP 1 ROM_IN4_Company_ID_FK from T070031 where ROM_IN4_Company_ID_FK=" & intABNo & ";select TOP 1 Company from UDC where Company=" & intABNo & ";select TOP 1 Company from UDCType where Company=" & intABNo & ";select TOP 1 SU_NU9_CompID from T040081 where SU_NU9_CompID=" & intABNo & ";"


            ElseIf enuABNumType = ABNumType.Both Then
                strSQL = "select UM_IN4_Address_No_FK from T060011 where UM_IN4_Address_No_FK=" & intABNo & ";select TOP 1 UM_IN4_Company_AB_ID from T060011 where UM_IN4_Company_AB_ID=" & intABNo & "; select TOP 1 AG_VC8_Cust_Name from T080011 where AG_VC8_Cust_Name=" & intABNo & "; select TOP 1 Company_id from Setup_Rules where Company_id=" & intABNo & "; select TOP 1 UV_NU9_Comp_ID from T030201 where UV_NU9_Comp_ID=" & intABNo & "; select TOP 1 IM_NU9_Company_ID_PK from T080031 where IM_NU9_Company_ID_PK=" & intABNo & "; select TOP 1 RA_IN4_AB_ID_Fk from T060022 where RA_IN4_AB_ID_Fk=" & intABNo & "; select TOP 1 ROM_IN4_Company_ID_FK from T070031 where ROM_IN4_Company_ID_FK=" & intABNo & ";select TOP 1 Company from UDC where Company=" & intABNo & ";select TOP 1 Company from UDCType where Company=" & intABNo & ";select TOP 1 SU_NU9_CompID from T040081 where SU_NU9_CompID=" & intABNo & ";"
            End If

            If sqCon.State <> ConnectionState.Open Then
                sqCon.Open()
            End If
            Dim dsABNum As New DataSet
            Dim sqADP As New SqlClient.SqlDataAdapter(strSQL, sqCon)
            sqADP.Fill(dsABNum)

            If dsABNum.Tables.Count > 0 Then
                For intI As Integer = 0 To dsABNum.Tables.Count - 1
                    If dsABNum.Tables(intI).Rows.Count > 0 Then
                        If dsABNum.Tables(intI).Rows(0)(0) <> 0 Then
                            blnFound = True
                            Exit For
                        End If

                    End If
                Next
            End If
            sqCon.Close()
            sqADP.Dispose()
            dsABNum.Dispose()
            Return blnFound
        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteAddressBookEntry", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        Finally

        End Try

    End Function

#End Region

#Region "Search UDC in Whole WSS Database"

    Public Shared Function SearchUDCInWSSDatabase(ByVal strUDCType As String, ByVal strUDC As String) As Boolean
        Try
            Dim blnFound As Boolean = False
            Dim strSQL As String
            Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
            Dim htQuery As New Collections.Hashtable

            htQuery.Add("ABTY", "select Top 1 * from T010011 where CI_VC8_Address_Book_Type='" & strUDC & "'")
            htQuery.Add("ACT", "select Top 1 * from T040031 where AM_VC100_Action_type='" & strUDC & "'")
            htQuery.Add("ADTY", "select Top 1 * from T010023 where AA_VC8_AddressType='" & strUDC & "'")
            htQuery.Add("AGST", "select Top 1 * 	from T080011 where AG_VC8_Status='" & strUDC & "'")
            htQuery.Add("AGTP", "select Top 1 * from T080011 where AG_VC8_Ag_Type='" & strUDC & "'")
            htQuery.Add("ALM", "select Top 1 * from T180011 where AM_VC6_TYPE='" & strUDC & "'")
            htQuery.Add("ALTY", "select Top 1 * from T180012 where AF_VC8_Type='" & strUDC & "'")
            htQuery.Add("ALST", "select Top 1 * from T180012 where AF_VC8_Status='" & strUDC & "'")
            htQuery.Add("ARCD", "select Top 1 * from T010011 where CI_VC8_Area_code_1='" & strUDC & "';select Top 1 * from T010011 where CI_VC8_Area_code_2='" & strUDC & "'")
            htQuery.Add("BREL", "select Top 1 * from T010011 where CI_IN4_Business_Relation='" & strUDC & "'")
            htQuery.Add("CALE", "select Top 1 * from T090011 where CL_VC8_Event='" & strUDC & "'")
            htQuery.Add("CALL", "select Top 1 * from T040011 where CM_VC8_Call_Type='" & strUDC & "'")
            htQuery.Add("CCD", "select Top 1 * from T010011 where CI_VC8_Country_Code_1='" & strUDC & "';select Top 1 * from T010011 where CI_VC8_Country_Code_2='" & strUDC & "'")
            htQuery.Add("CNTY", "select Top 1 * from T010011 where CI_VC8_Country='" & strUDC & "';select Top 1 * from T010023 where AA_VC8_Country='" & strUDC & "'")
            htQuery.Add("COMT", "select Top 1 * from T010043 where PI_VC10_Company_Type='" & strUDC & "'")
            htQuery.Add("CTY", "select Top 1 * from T010011 where CI_VC8_City='" & strUDC & "';select Top 1 * from T010023 where AA_VC8_City='" & strUDC & "'")
            htQuery.Add("CUR", "select Top 1 * from T010043 where PI_VC8_Currency='" & strUDC & "'")
            htQuery.Add("INST", "select Top 1 * from T080031 where IM_VC8_Invoice_Status='" & strUDC & "'")
            htQuery.Add("LEVL", "select Top 1 * from T010011 where CI_VC8_Level='" & strUDC & "'")
            htQuery.Add("OBTY", "select Top 1 * from T130131 where OM_VC4_OType='" & strUDC & "'")
            htQuery.Add("PACK", "select Top 1 * from T130031 where PM_VC1_PAck='" & strUDC & "'")
            htQuery.Add("PHTY", "select Top 1 * from T010011 where CI_VC8_Phone_Type_1='" & strUDC & "';select Top 1 * from T010011 where CI_VC8_Phone_Type_2='" & strUDC & "'")
            htQuery.Add("PJST", "select Top 1 * from T210011 where PR_VC8_Status='" & strUDC & "'")
            htQuery.Add("PJTY", "select Top 1 * from T210011 where PR_VC8_Type='" & strUDC & "'")
            htQuery.Add("PRIO", "select Top 1 * from T040011 where  CM_VC200_Work_Priority='" & strUDC & "';select Top 1 * from T040021 where TM_VC8_Priority='" & strUDC & "'")
            htQuery.Add("PROV", "select Top 1 * from T010011 where CI_VC8_Province='" & strUDC & "';select Top 1 * from T010023 where AA_VC8_Province='" & strUDC & "'")
            htQuery.Add("PRTY", "select Top 1 * from T130031 where PM_VC10_PType='" & strUDC & "'")
            htQuery.Add("SKTY", "select Top 1 * from T010033 where ST_VC8_Skill_Type='" & strUDC & "'")
            htQuery.Add("SKL", "select Top 1 * from T010033 where ST_VC8_Skill='" & strUDC & "'")
            htQuery.Add("TKTY", "select Top 1 * from T040021 where TM_VC8_task_type='" & strUDC & "'")
            htQuery.Add("TMPL", "select Top 1 * from T050011 where TL_VC8_Tmpl_Type='" & strUDC & "'")

            strSQL = htQuery.Item(strUDCType)

            'If strSQL = Nothing Then
            '    Return True
            'End If

            If sqCon.State <> ConnectionState.Open Then
                sqCon.Open()
            End If
            Dim dsABNum As New DataSet
            Dim sqADP As New SqlClient.SqlDataAdapter(strSQL, sqCon)
            sqADP.Fill(dsABNum)

            If dsABNum.Tables.Count > 0 Then
                For intI As Integer = 0 To dsABNum.Tables.Count - 1
                    If dsABNum.Tables(intI).Rows.Count > 0 Then
                        blnFound = True
                        Exit For
                    End If
                Next
            End If
            sqCon.Close()
            sqADP.Dispose()
            dsABNum.Dispose()
            Return blnFound

        Catch ex As Exception

        End Try

    End Function

#End Region

#Region "Search Company Type"

    Public Shared Function SearchCompanyType(ByVal intCompanyID As Integer) As ReturnValue
        Try
            Dim strCompType As String
            Dim stReturn As ReturnValue

            strCompType = SQL.Search("WSSSearch", "SearchCompanyType", "select CI_IN4_Business_Relation from T010011 where CI_NU8_Address_Number=" & intCompanyID)
            If strCompType <> "" Then
                stReturn.ExtraValue = strCompType.Trim
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
            Else
                stReturn.ExtraValue = strCompType.Trim
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "No Record Found"
                stReturn.ErrorCode = 1
            End If
            Return stReturn
        Catch ex As Exception
            CreateLog("WWSSearch", "SearchCompanyType-1157", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region " Document/Folder Management "
    Public Shared Function GetFolderName(ByVal intFolderID As Integer) As ReturnValue
        Try
            Dim strFolderName As String
            Dim stReturn As ReturnValue

            strFolderName = SQL.Search("WSSSearch", "GetFolderName", "select FD_VC255_Folder_Name from T250021 where FD_NU9_Folder_ID_PK=" & intFolderID)
            If strFolderName <> "" Then
                stReturn.ExtraValue = strFolderName.Trim
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
            Else
                stReturn.ExtraValue = ""
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "No Record Found"
                stReturn.ErrorCode = 1
            End If
            Return stReturn
        Catch ex As Exception
            CreateLog("WWSSearch", "GetFolderName-1208", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Public Shared Function GetFolderDetail(ByVal intFolderID As Integer) As ReturnValue
        Try
            Dim dsFolder As New DataSet
            Dim stReturn As ReturnValue

            SQL.Search("T250021", "WSSSearch", "GetFolderDetail", "select * from T250021 where FD_NU9_Folder_ID_PK=" & intFolderID, dsFolder, "", "")
            If Not dsFolder Is Nothing Then
                stReturn.ExtraValue = dsFolder
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
            Else
                stReturn.ExtraValue = Nothing
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "No Record Found"
                stReturn.ErrorCode = 1
            End If
            Return stReturn
        Catch ex As Exception
            CreateLog("WWSSearch", "GetFolderDetail-1231", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Public Shared Function CheckFolderExistance(ByVal FolderName As String, ByVal ParentFolderID As Int32, ByVal CompanyID As Int32, ByVal FolderID As Int32) As ReturnValue
        Try
            Dim intFolder As New Int32
            Dim stReturn As ReturnValue

            SQL.Search("WSSSearch", "CheckFolderExistance", "select FD_NU9_Folder_ID_PK from T250021 where upper(FD_VC255_Folder_Name)='" & UCase(FolderName) & "' AND FD_NU9_Parent_Folder_ID_FK= " & ParentFolderID & " And FD_NU9_Company_ID_FK=" & CompanyID & " and FD_NU9_Folder_ID_PK <>" & FolderID, intFolder)
            If intFolder > 0 Then
                stReturn.ExtraValue = True
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
            Else
                stReturn.ExtraValue = False
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "No Record Found"
                stReturn.ErrorCode = 1
            End If
            Return stReturn
        Catch ex As Exception
            CreateLog("WWSSearch", "CheckFolderExistance-1254", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region " File Details "
    Public Shared Function GetFileDetail(ByVal intFileID As Integer) As ReturnValue
        Try
            Dim dsFile As New DataSet
            Dim stReturn As ReturnValue

            SQL.Search("T250011", "WSSSearch", "GetFolderDetail", "select * from T250011 where FI_NU9_File_ID_PK=" & intFileID, dsFile, "", "")
            If Not dsFile Is Nothing Then
                stReturn.ExtraValue = dsFile
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = ""
                stReturn.ErrorCode = 0
            Else
                stReturn.ExtraValue = Nothing
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "No Record Found"
                stReturn.ErrorCode = 1
            End If
            Return stReturn
        Catch ex As Exception
            CreateLog("WWSSearch", "GetFolderDetail-1256", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
#End Region

End Class