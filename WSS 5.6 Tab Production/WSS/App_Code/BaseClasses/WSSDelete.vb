Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Data


Public Class WSSDelete

	Public Shared Function DeleteUDCType(ByVal ProductCode As Integer, ByVal UDCType As String) As ReturnValue
		Dim stReturn As ReturnValue

		Try
			If SQL.Delete("WSSDelete", "DeleteUDCType-10", "delete from UDCType where ProductCode=" & ProductCode & " and UDCType='" & UDCType & "'", SQL.Transaction.Serializable) = True Then
				stReturn.ErrorCode = 0
				stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = "Record deleted successfully..."
			Else
				stReturn.ErrorCode = 1
				stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Server is busy please try later..."
			End If

			Return stReturn
		Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
			stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeleteUDCType-25", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
			Return stReturn
		End Try

	End Function

	Public Shared Function DeleteUDC(ByVal ProductCode As Integer, ByVal UDCType As String, ByVal Name As String) As ReturnValue
		Dim stReturn As ReturnValue

        Try

            If WSSSearch.SearchUDCInWSSDatabase(UDCType, Name) = False Then

                If SQL.Delete("WSSDelete", "DeleteUDC-35", "delete from UDC where ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' and Name='" & Name & "'", SQL.Transaction.Serializable) = True Then
                    stReturn.ErrorCode = 0
                    stReturn.FunctionExecuted = True
                    stReturn.ErrorMessage = "Record deleted successfully..."
                Else
                    stReturn.ErrorCode = 1
                    stReturn.FunctionExecuted = False
                    stReturn.ErrorMessage = "Server is busy please try later..."
                End If
            Else
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Active UDC cannot be deleted"
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeleteUDC-50", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return stReturn
        End Try

	End Function

#Region " AB_Main "

	Public Shared Function DeletePersonalInfo(ByVal AddressNo As Integer) As ReturnValue
		Dim stReturn As ReturnValue

		Try
			If SQL.Delete("WSSDelete", "DeletePersonalInfo-62", "delete from T010043 where PI_NU8_Address_No =" & AddressNo & "", SQL.Transaction.Serializable) = True Then
				stReturn.ErrorCode = 0
				stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = "Record deleted successfully..."
			Else
				stReturn.ErrorCode = 1
				stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Server is busy please try later..."
			End If

			Return stReturn
		Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
			stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeletePersonalInfo-77", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
			Return stReturn
		End Try

	End Function

	Public Shared Function DeleteCategory(ByVal AddressNo As Integer) As ReturnValue
		Dim stReturn As ReturnValue

		Try
			If SQL.Delete("WSSDelete", "DeleteCategory-87", "delete from T010053 where CC_NU8_Address_No =" & AddressNo & "", SQL.Transaction.Serializable) = True Then
				stReturn.ErrorCode = 0
				stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = "Record deleted successfully..."
			Else
				stReturn.ErrorCode = 1
				stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Server is busy please try later..."
			End If

			Return stReturn
		Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
			stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeleteCategory-102", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
			Return stReturn
		End Try

	End Function

#End Region

#Region " Delete Task and Action "

    Public Shared Function DeleteTask(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False
            Dim strchkcallstatus As String = SQL.Search("WSSDelete", "DeleteTask-121", "select CN_VC20_Call_Status from T040011 where  CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & compID & " and CN_VC20_Call_Status='CLOSED'")

            If IsNothing(strchkcallstatus) = False Then
                stReturn.ErrorMessage = "Call Closed so You cannot change the Task..."
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                Return stReturn
            End If
            Dim intTaskOrder As Integer
            intTaskOrder = SQL.Search("", "", "select TM_NU9_Task_Order from T040021  where TM_NU9_Comp_ID_FK=" & compID & " and TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK=" & TaskNumber)
            Call ChangeTaskOrder(mdlMain.EnumTaskOrder.DeleteTask, intTaskOrder, compID, CallNumber)

            If SQL.Delete("WSSDelete", "DeleteTask-130", "Delete from T040021 where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK=" & TaskNumber & " and TM_NU9_Comp_ID_FK=" & compID, SQL.Transaction.Serializable) = True Then
                'Remove the dependancy of this task in other task
                SQL.Update("", "", "update T040021 set TM_NU9_Dependency=null where TM_NU9_Comp_ID_FK=" & compID & " and TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Dependency=" & TaskNumber, SQL.Transaction.Serializable)

                Call DeleteComment(False, compID, CallNumber, TaskNumber)
                Call DeleteAttachment(False, compID, CallNumber, TaskNumber)

                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = "Record deleted successfully..."
            Else
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Server is busy please try later..."
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeleteTask-131", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

    Public Shared Function DeleteAction(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer, ByVal compID As Integer) As ReturnValue

        Dim stReturn As ReturnValue

        Try
            If SQL.Delete("WSSDelete", "DeleteAction-156", "Delete from T040031 where AM_NU9_Call_Number=" & CallNumber & " and AM_NU9_Task_Number=" & TaskNumber & " and AM_NU9_Action_Number=" & ActionNumber & " and AM_NU9_Comp_ID_FK=" & compID, SQL.Transaction.Serializable) = True Then

                Call DeleteComment(False, compID, CallNumber, TaskNumber, ActionNumber)

                Call DeleteAttachment(False, compID, CallNumber, TaskNumber, ActionNumber)

                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = "Record deleted successfully..."
            Else
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Server is busy please try later..."
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeleteAction-156", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region "Delete Template Task"
    Public Shared Function DeleteTemplateTask(ByVal TemplateNumber As Integer, ByVal TaskNumber As Integer) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            If SQL.Delete("WSSDelete", "DeleteTemplateTask-183", "Delete from T050031 where TTM_NU9_TemplateID_FK=" & TemplateNumber & " and TTM_NU9_Task_no_PK=" & TaskNumber & "", SQL.Transaction.Serializable) = True Then
                'Remove the dependancy of this task in other task
                SQL.Update("", "", "update T050031 set TTM_NU9_Dependency=null where TTM_NU9_TemplateID_FK=" & TemplateNumber & " and TTM_NU9_Dependency=" & TaskNumber, SQL.Transaction.Serializable)

                Call DeleteComment(True, -1, TemplateNumber, TaskNumber)
                Call DeleteAttachment(True, -1, TemplateNumber, TaskNumber)

                stReturn.ErrorCode = 0
                stReturn.FunctionExecuted = True
                stReturn.ErrorMessage = "Record deleted successfully..."
            Else
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                stReturn.ErrorMessage = "Server is busy please try later..."
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSDelete", "DeleteTemplateTask-183", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function
#End Region

#Region "Delete Address Book Entry"

    Public Shared Function DeleteAddressBookEntry(ByVal intABNum As Integer) As Boolean
        Try
            If WSSSearch.SearchABNumber(intABNum, WSSSearch.ABNumType.Both) = False Then
                Dim strSQL As String = "delete from T010011 where CI_NU8_Address_Number=" & intABNum & ";delete from T010023 where AA_NU8_Address_Number=" & intABNum & ";delete from T010033 where ST_NU8_Address_Number=" & intABNum & ";delete from T010043 where PI_NU8_Address_No=" & intABNum & ";delete from T010053 where CC_NU8_Address_No=" & intABNum & ";"

                Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                If sqCon.State <> ConnectionState.Open Then
                    sqCon.Open()
                End If
                Dim sqCMD As New SqlClient.SqlCommand(strSQL, sqCon)
                sqCMD.ExecuteNonQuery()
                sqCMD.Dispose()
                sqCon.Close()
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteAddressBookEntry", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function


#End Region

#Region "Delete User Profile"

    Public Shared Function DeleteUserProfile(ByVal intABNum As Integer) As Boolean
        Try
            If WSSSearch.SearchABNumber(intABNum, WSSSearch.ABNumType.User) = False Then
                Dim strSQL As String = "Delete from T060041 where UC_NU9_User_ID_FK=" & intABNum & ";delete  from T060011 where UM_IN4_Address_No_FK=" & intABNum
                Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                If sqCon.State <> ConnectionState.Open Then
                    sqCon.Open()
                End If
                Dim sqCMD As New SqlClient.SqlCommand(strSQL, sqCon)
                sqCMD.ExecuteNonQuery()
                sqCMD.Dispose()
                sqCon.Close()
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteAddressBookEntry", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function


#End Region

#Region "Delete Comments"

    Public Shared Function DeleteComment(ByVal blnIsTemplate As Boolean, ByVal intCompID As Integer, ByVal intCallNo As Integer, Optional ByVal intTaskNo As Integer = 0, Optional ByVal intActionNo As Integer = 0) As Boolean
        'When this function will be called for template then callno will be treated as template id
        Try
            Dim strSQL As String
            If intCallNo <> 0 And intActionNo <> 0 And intTaskNo <> 0 Then 'Delete Action Comment
                If blnIsTemplate = True Then
                Else
                    strSQL = "delete from T040061 where CM_NU9_CompId_Fk=" & intCompID & " and CM_NU9_Call_Number=" & intCallNo & " and CM_NU9_Task_Number=" & intTaskNo & " and CM_NU9_Action_Number=" & intActionNo
                End If
            ElseIf intCallNo <> 0 And intTaskNo <> 0 And intActionNo = 0 Then 'Delete Task Comment
                If blnIsTemplate = True Then
                    strSQL = "delete from T050061 where CM_NU9_TemplateID_FK=" & intCallNo & " and CM_NU9_Task_Number=" & intTaskNo
                Else
                    strSQL = "delete from T040061 where CM_NU9_CompId_Fk=" & intCompID & " and CM_NU9_Call_Number=" & intCallNo & " and CM_NU9_Task_Number=" & intTaskNo
                End If
            End If
            If Not IsNothing(strSQL) Then
                If SQL.Delete("", "", strSQL, SQL.Transaction.Serializable) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
            Return False
        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteComment", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function

#End Region

#Region "Delete Attachment"

    Public Shared Function DeleteAttachment(ByVal blnIsTemplate As Boolean, ByVal intCompID As Integer, ByVal intCallNo As Integer, Optional ByVal intTaskNo As Integer = 0, Optional ByVal intActionNo As Integer = 0) As Boolean
        'When this function will be called for template then callno will be treated as template id
        Try
            Dim strSQL As String
            Dim strSQLVH As String
            Dim strFilePathSQL As String

            If intCallNo <> 0 And intActionNo <> 0 And intTaskNo <> 0 Then 'Delete Action Attachment
                If blnIsTemplate = False Then
                    strSQL = "delete from T040041 where AT_NU9_CompId_Fk=" & intCompID & " and AT_NU9_Call_Number=" & intCallNo & " and AT_NU9_Task_Number=" & intTaskNo & " and AT_NU9_Action_Number=" & intActionNo
                    strSQLVH = "delete from T040051 where VH_NU9_CompId_Fk=" & intCompID & " and VH_NU9_Call_Number=" & intCallNo & " and VH_NU9_Task_Number=" & intTaskNo & " and VH_NU9_Action_Number=" & intActionNo
                    strFilePathSQL = "select VH_VC255_File_Path from T040051 where VH_NU9_CompId_Fk=" & intCompID & "  and VH_NU9_Call_Number=" & intCallNo & "  and VH_NU9_Task_Number=" & intTaskNo & " and VH_NU9_Action_Number=" & intActionNo
                Else
                    'There is no action in template
                End If
            ElseIf intCallNo <> 0 And intTaskNo <> 0 And intActionNo = 0 Then 'Delete Task Attachment
                If blnIsTemplate = False Then
                    strSQL = "delete from T040041 where AT_NU9_CompId_Fk=" & intCompID & " and AT_NU9_Call_Number=" & intCallNo & " and AT_NU9_Task_Number=" & intTaskNo
                    strSQLVH = "delete from T040051 where VH_NU9_CompId_Fk=" & intCompID & " and VH_NU9_Call_Number=" & intCallNo & " and VH_NU9_Task_Number=" & intTaskNo
                    strFilePathSQL = "select VH_VC255_File_Path from T040051 where VH_NU9_CompId_Fk=" & intCompID & "  and VH_NU9_Call_Number=" & intCallNo & "  and VH_NU9_Task_Number=" & intTaskNo
                Else
                    strSQL = "delete from T050041 where  AT_NU9_TemplateID_FK=" & intCallNo & " and AT_NU9_Task_Number=" & intTaskNo
                    strSQLVH = "delete from T050051 where VH_NU9_TemplateID_FK=" & intCallNo & " and VH_NU9_Task_Number=" & intTaskNo
                    strFilePathSQL = "select VH_VC255_File_Path from T050051 where  VH_NU9_TemplateID_FK=" & intCallNo & "  and VH_NU9_Task_Number=" & intTaskNo
                End If
            End If

            If Not IsNothing(strSQL & ";" & strSQLVH) Then
                Dim arrFilePath As New ArrayList
                If Not IsNothing(strFilePathSQL) Then
                    Dim dsFilePath As New DataSet
                    Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                    Dim sqADP As New SqlClient.SqlDataAdapter(strFilePathSQL, sqCon)
                    sqADP.Fill(dsFilePath)
                    For intI As Integer = 0 To dsFilePath.Tables.Count - 1
                        For intJ As Integer = 0 To dsFilePath.Tables(intI).Rows.Count - 1
                            arrFilePath.Add(dsFilePath.Tables(intI).Rows(intJ).Item(0))
                        Next
                    Next
                    sqCon.Close()
                    sqADP.Dispose()
                    dsFilePath.Dispose()
                End If
                If SQL.Delete("", "", strSQL & ";" & strSQLVH, SQL.Transaction.Serializable) = True Then
                    For intI As Integer = 0 To arrFilePath.Count - 1
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath("../../") & arrFilePath(intI))
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
            Return False
        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteAttachment", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function

#End Region

#Region "Delete Template"

    Public Shared Function DeleteTemplate(ByVal TemplateID As Integer) As Boolean
        Try
            Dim dsAttachment As New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.Search("T050051", "", "", "select * from T050051 where VH_NU9_TemplateID_FK=" & TemplateID, dsAttachment, "", "")
            SQL.Delete("Template", "Load-136", "select * from T050072 where Tb_IN4_Form_No=(select FD_IN4_Form_no from T050071 where FD_IN4_Temp_Id=" & TemplateID & ")", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-136", "delete from T050071 where  FD_IN4_Temp_Id=" & TemplateID & "", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-136", "delete from T050061 where  CM_NU9_TemplateID_FK=" & TemplateID & "", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-135", "delete from T050051 where VH_NU9_TemplateID_FK=" & TemplateID & "", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-134", "delete from T050041 where AT_NU9_TemplateID_FK=" & TemplateID & "", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-133", "delete from T050031 where TTM_NU9_TemplateID_FK=" & TemplateID & "", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-123", "delete from T050021 where TCM_NU9_TemplateID_FK=" & TemplateID & "", SQL.Transaction.Serializable)
            SQL.Delete("Template", "Load-124", "delete from T050011 where TL_NU9_ID_PK=" & TemplateID & "", SQL.Transaction.Serializable)
            If Not IsNothing(dsAttachment) Then
                For intI As Integer = 0 To dsAttachment.Tables(0).Rows.Count - 1
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("../../" & dsAttachment.Tables(0).Rows(intI).Item("VH_VC255_File_Path")))
                Next
            End If
            Return True
        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteTemplate-392", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return True
        End Try
    End Function

#End Region

#Region "Delete SubCategory"

    Public Shared Function DeleteProject(ByVal CompanyID As Integer, ByVal ProjectID As Integer) As ReturnValue
        Dim stReturn As New ReturnValue
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False
            Dim intRows As Integer = 0
            SQL.Search("", "", "select * from T210012 where PM_NU9_Project_ID_Fk=" & ProjectID & " and PM_NU9_Comp_ID_FK=" & CompanyID, intRows)
            'Check if any Project member exists 
            If intRows = 0 Then
                intRows = 0
                SQL.Search("", "", "select * from T040011 where CM_NU9_Project_ID=" & ProjectID & " and CM_NU9_Comp_Id_FK=" & CompanyID, intRows)
                Dim intRows1 As Integer = 0
                SQL.Search("", "", "select * from T050011 where TL_NU9_ProjectID_FK=" & ProjectID & " and TL_NU9_CustID_FK=" & CompanyID, intRows1)
                'check if this project is used any call/template
                If intRows = 0 And intRows1 = 0 Then
                    If SQL.Delete("", "", "Delete FROM T210011 where PR_NU9_Project_ID_Pk=" & ProjectID & " and PR_NU9_Comp_ID_FK=" & CompanyID, SQL.Transaction.Serializable) = True Then
                        stReturn.ErrorCode = 0
                        stReturn.ErrorMessage = "Record Deleted Successfully..."
                    Else
                        stReturn.ErrorCode = 1
                        stReturn.ErrorMessage = "Record Not Deleted..."
                    End If
                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "This SubCategory is in use..."
                End If
            Else
                stReturn.ErrorCode = 1
                stReturn.ErrorMessage = "Please delete all the members of this SubCategory..."
            End If
        Catch ex As Exception
            CreateLog("WWSDelete", "DeleteProject-392", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            stReturn.ErrorCode = 0
            stReturn.ErrorMessage = "SubCategory Not Deleted..."
        End Try
        Return stReturn
    End Function

#End Region

End Class
