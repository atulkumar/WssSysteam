Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Data.SqlClient

Public Class WSSUpdate

#Region " AB_Main "

    Public Shared Function UpdatePersonalInfo(ByVal AddressNo As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Update("T010043", "WSSUpdate", "UpdatePersonalInfo-18", "select * from T010043 where PI_NU8_Address_no=" & AddressNo & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdatePersonalInfo-33", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    '*************************
    Public Shared Function UpdateCategoryInfo(ByVal AddressNo As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Update("T010053", "WSSUpdate", "UpdateCategoryInfo-48", "select * from T010053 where CC_NU8_Address_No=" & AddressNo & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateCategoryInfo-74", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return stReturn
        End Try
    End Function

#End Region

#Region " CreateUser "

    Public Shared Function UpdateUserLogin(ByVal AddressNo As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Update("T060011", "WSSUpdate", "UpdateUserLogin-83", "select * from T060011 where UM_IN4_Address_No_FK=" & AddressNo & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateUserLogin-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " Call, Task, Action "
    Public Shared Function UpdateCall(ByVal CallNumber As Integer, ByVal Company As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, Optional ByVal StatusChanged As Boolean = False, Optional ByVal CallStatus As String = "") As ReturnValue
        Dim stReturn As ReturnValue
        Dim i As Int16
        Dim blnFound As Boolean
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Update("T040011", "WSSUpdate", "UpdateCall-117", "select * from T040011 where CM_NU9_Comp_Id_FK=" & Company & " and  CM_NU9_Call_No_PK=" & CallNumber & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            If stReturn.ErrorCode = 0 Then
                ColumnName.Add("CM_NU9_Call_No_PK")
                ColumnName.Add("CM_NU9_Comp_Id_FK")
                ColumnName.Add("CM_NU4_Event_ID")
                ColumnName.Add("CM_CH1_MailSent")
                ColumnName.Add("CM_DT8_Log_Date")
                ColumnName.Add("CM_NU9_ModifyBy")

                RowData.Add(CallNumber)
                RowData.Add(Company)
                ' Add event if status is closed then 14 else 19 
                For i = (RowData.Count - 1) To 0 Step -1
                    If IsDBNull(RowData(i)) OrElse RowData(i).ToString <> "CLOSED" Then
                        blnFound = False
                    Else
                        blnFound = True
                        Exit For
                    End If
                Next
                If blnFound = True Then 'Task is closed
                    RowData.Add(14)
                ElseIf StatusChanged = True Then
                    RowData.Add(13)
                Else
                    RowData.Add(19)
                End If
                RowData.Add("F")
                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))
                SQL.Save("T990011", "WSSUpdate", "UpdateCall-129", ColumnName, RowData)
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateCall-142", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
    Public Shared Function UpdateCallStatus(ByVal CallNumber As Integer, ByVal Assigned As Boolean, ByVal CompanyID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim strCallStatus As String
        Dim strSql As String
        Dim strPrvStatus As String
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            strPrvStatus = SQL.Search("WSSUpdate", "UpdateCallStatus-185", "Select CN_VC20_Call_Status From T040011 Where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID)

            Dim strUpdate As String
            strCallStatus = ""
            strUpdate = ""

            If Assigned = True And strPrvStatus <> "ASSIGNED" Then
                strUpdate = "update T040011 set CN_VC20_Call_Status='ASSIGNED' where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID & ""
                strCallStatus = "ASSIGNED"
            ElseIf Assigned = False And strPrvStatus <> "PROGRESS" Then

                Dim chkStatusRange As Boolean
                Dim rocount As Integer

                If chkStatusRange = SQL.Search("updatecallstatus", "UpdateCallStatus-201", "Select * From T040081 Where SU_VC50_Status_Name='" & strPrvStatus & "' and SU_NU9_Status_Code>=21 and SU_NU9_Status_Code<=30 ", rocount) = False Then
                Else
                    strUpdate = "update T040011 set CN_VC20_Call_Status='PROGRESS' where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID & ""
                    strCallStatus = "PROGRESS"
                End If
            End If
            Dim strStatus As String
            strStatus = SQL.Search("WSSUpdate", "UpdateCallStatus-155", "select CN_VC20_Call_Status from T040011 where CM_NU9_Call_No_PK=" & CallNumber & " and CN_VC20_Call_Status='PROGRESS' and CM_NU9_Comp_Id_FK=" & CompanyID & "")
            If IsNothing(strStatus) = False Then
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                Return stReturn
            End If
            If strUpdate.Trim <> "" Then
                If SQL.Update("WSSUpdate", "UpdateCallStatus-164", strUpdate, SQL.Transaction.Serializable) = False Then
                    stReturn.ErrorMessage = "Server is busy please try later..."
                    stReturn.FunctionExecuted = False
                    stReturn.ErrorCode = 1
                Else
                    stReturn.ErrorMessage = "Records updated successfully..."
                    stReturn.FunctionExecuted = True
                    stReturn.ErrorCode = 0
                End If
            End If
            ' Creating Log
            If stReturn.ErrorCode = 0 And strCallStatus.Trim <> "" Then
                strSql = "INSERT INTO  T990011(CM_NU9_Call_No_PK ,CM_NU9_Comp_Id_FK ," & _
                    " CM_NU9_CustID_FK ,CM_NU9_Code,CM_VC8_Call_Type ,CM_VC2000_Call_Desc ," & _
                    " CM_DT8_Request_Date ,CM_DT8_Close_Date ,CM_VC200_Work_Priority ,CN_VC20_Call_Status ," & _
                    " CM_VC100_By_Whom ,CM_NU8_Attach_No,CM_VC50_Reference_Id ,CM_VC4_Call_Request ," & _
                    " CM_NU9_Attended_by,CM_NU9_On_Behalf_Comp,CM_NU9_Submitted_by,CM_NU9_Call_Own_Comp," & _
                    " CM_NU9_Call_Owner,CM_NU9_On_behalf_emp,CM_NU9_Business_Code_Fk,CM_NU9_Ac_Code_Fk," & _
                    " CM_DT8_Request_Time ,CM_VC100_Subject ,CM_NU9_Project_ID ,CM_NU9_Rule_Mst_Id,CM_NU9_Category_Code_1," & _
                    " CM_NU9_Category_Code_2,CM_NU9_Category_Code_3,CM_NU9_Category_Code_4,CM_NU9_Category_Code_5," & _
                    " CM_VC2000_Call_Close_Desc ,CM_VC500_Send_To ,CM_VC500_Send_To_1 ,CM_CM_NU8_500_Send_From ,CM_VC500_Send_Subject ," & _
                    " CM_VC500_Send_Description ,CM_NU9_Send_Add_Time,CM_VC8_Template ,CM_VC8_Tmpl_Type ,CM_DT8_Call_Close_Date ," & _
                    " CM_CH1_Comment ,CM_NU9_Agreement ,CM_NU4_Event_ID ,CM_CH1_MailSent, CM_DT8_Log_Date,CM_NU9_ModifyBy)" & _
                    " 	    SELECT CM_NU9_Call_No_PK ,CM_NU9_Comp_Id_FK ," & _
                    " CM_NU9_CustID_FK ,CM_NU9_Code,CM_VC8_Call_Type ,CM_VC2000_Call_Desc ," & _
                    " CM_DT8_Request_Date ,CM_DT8_Close_Date ,CM_VC200_Work_Priority ,CN_VC20_Call_Status ," & _
                    " CM_VC100_By_Whom ,CM_NU8_Attach_No,CM_VC50_Reference_Id ,CM_VC4_Call_Request ," & _
                    " CM_NU9_Attended_by,CM_NU9_On_Behalf_Comp,CM_NU9_Submitted_by,CM_NU9_Call_Own_Comp," & _
                    " CM_NU9_Call_Owner,CM_NU9_On_behalf_emp,CM_NU9_Business_Code_Fk,CM_NU9_Ac_Code_Fk," & _
                    " CM_DT8_Request_Time ,CM_VC100_Subject ,CM_NU9_Project_ID ,CM_NU9_Rule_Mst_Id,CM_NU9_Category_Code_1," & _
                    " CM_NU9_Category_Code_2,CM_NU9_Category_Code_3,CM_NU9_Category_Code_4,CM_NU9_Category_Code_5," & _
                    " CM_VC2000_Call_Close_Desc ,CM_VC500_Send_To ,CM_VC500_Send_To_1 ,CM_CM_NU8_500_Send_From ,CM_VC500_Send_Subject ," & _
                    " CM_VC500_Send_Description ,CM_NU9_Send_Add_Time,CM_VC8_Template ,CM_VC8_Tmpl_Type ,CM_DT8_Call_Close_Date ," & _
                    " CM_CH1_Comment ,CM_NU9_Agreement,13,'F',getdate() as CM_DT8_Log_Date," & HttpContext.Current.Session("PropUserId") & " from t040011 " & _
                    " WHERE CM_NU9_Call_No_PK=" & CallNumber & " AND CM_NU9_Comp_Id_FK = " & CompanyID
                SQL.Save("WSSUpdate", "UpdateCallStatus", strSql, SQL.Transaction.ReadCommitted)
                SQL.Update("WSSUpdate", "UpdateCallStatus-195", "UPDATE T990021  set TM_VC8_Call_Status='" & strCallStatus & "' where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Comp_Id_FK=" & CompanyID & " and TM_VC8_Call_Status is null", SQL.Transaction.Serializable)
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateCallStatus-189", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Public Shared Function UpdateCallHours(ByVal CompID As Integer, ByVal CallNumber As Integer) As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False
            Dim strUpdateSQL As String
            strUpdateSQL = "Update T040011 set CM_FL8_Total_Reported_Time=(select isnull(round(sum(AM_FL8_Used_Hr),1),0) TotalRptTime from T040031 where AM_NU9_Comp_ID_FK=" & CompID & " and AM_NU9_Call_Number=" & CallNumber & "), CM_FL8_Total_Est_Time=(select isnull(round(sum(TM_FL8_Est_Hr),1),0) TotalEstTime from T040021 where TM_NU9_Comp_ID_FK=" & CompID & " and TM_NU9_Call_No_FK=" & CallNumber & ")where CM_NU9_Comp_Id_FK=" & CompID & " and CM_NU9_Call_No_PK=" & CallNumber
            If SQL.Update("WSSUpdate", "UpdateTask-192", strUpdateSQL, SQL.Transaction.Serializable) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("WWSUpdate", "UpdateCallHours-270", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Public Shared Function UpdateTask(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, Optional ByVal StatusChanged As Boolean = False) As ReturnValue
        Dim stReturn As ReturnValue

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False

        Try
            If SQL.Update("T040021", "WSSUpdate", "UpdateTask-192", "Select * from T040021 where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK= " & TaskNumber & " and TM_NU9_Comp_ID_FK=" & compID, ColumnName, RowData) = True Then
                Call UpdateCallHours(compID, CallNumber)
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If
            If stReturn.ErrorCode = 0 Then
                Dim blnStatus As Boolean
                Dim dtrCall As SqlDataReader
                Dim i As Int16
                Dim blnFound As Boolean

                ColumnName.Add("TM_NU9_Call_No_FK")
                ColumnName.Add("TM_NU9_Comp_ID_FK")
                ColumnName.Add("TM_NU9_Task_no_PK")
                ColumnName.Add("TM_NU4_Event_ID")
                ColumnName.Add("TM_CH1_MailSent")
                ColumnName.Add("TM_DT8_Log_Date")
                ColumnName.Add("TM_NU9_ModifyBy")

                RowData.Add(CallNumber)
                RowData.Add(compID)
                RowData.Add(TaskNumber)
                ' Add event if status is closed then 15 else 19
                For i = (RowData.Count - 1) To 0 Step -1
                    If IsDBNull(RowData(i)) OrElse RowData(i).ToString <> "CLOSED" Then
                        blnFound = False
                    Else
                        blnFound = True
                        Exit For
                    End If
                Next
                If blnFound = True Then 'Task is closed
                    RowData.Add(15)
                ElseIf StatusChanged = True Then
                    RowData.Add(16)
                Else
                    RowData.Add(19)
                End If



                If blnFound = True Then
                    If (HttpContext.Current.Session("PropUserId")) Then
                        Dim TaskOwner As String
                        Dim dtrTask As SqlDataReader = SQL.Search("wssupdate", "updateTask-245", "Select TM_NU9_assign_By from T040021 where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK= " & TaskNumber & " and TM_NU9_Comp_ID_FK=" & compID, SQL.CommandBehaviour.Default, blnStatus)
                        While dtrTask.Read
                            TaskOwner = dtrTask("TM_NU9_assign_By").ToString
                        End While
                        If (TaskOwner = HttpContext.Current.Session("PropUserId")) Then
                            RowData.Add("T")
                        End If
                    Else
                        RowData.Add("F")
                    End If
                Else
                    RowData.Add("F") 'default value
                End If

                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))
                dtrCall = SQL.Search("wssupdate", "updateTask-245", "Select CM_VC8_Call_Type,CN_VC20_Call_Status,CM_VC200_Work_Priority From T040011 Where CM_NU9_Call_No_PK=" & CallNumber & " And CM_NU9_Comp_Id_FK= " & compID, SQL.CommandBehaviour.Default, blnStatus)
                If blnStatus = True Then
                    While dtrCall.Read
                        ColumnName.Add("TM_VC8_Call_Type")
                        ColumnName.Add("TM_VC8_Call_Status")
                        RowData.Add(dtrCall("CM_VC8_Call_Type"))
                        RowData.Add(dtrCall("CN_VC20_Call_Status"))
                    End While
                    dtrCall.Close()
                End If
                'Get the task assign by to store task assign by id in log table.
                'assign by is needed in log table to prepare mail body
                Dim intTaskAssignedBy As Integer = SQL.Search("", "", "select TM_NU9_Assign_by from T040021 where  TM_NU9_Task_no_PK=" & TaskNumber & " and TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Comp_ID_FK=" & compID)
                ColumnName.Add("TM_NU9_Assign_by")
                RowData.Add(intTaskAssignedBy)
                SQL.Save("T990021", "WSSUpdate", "UpdateTask-236", ColumnName, RowData)
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateTask-217", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
    Public Shared Function UpdateTaskStatus(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal compID As Integer) As ReturnValue

        Dim stReturn As ReturnValue
        Dim strSql As String
        Dim strPrvStatus As String
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False

        Try
            strPrvStatus = SQL.Search("WSSUpdate", "UpdateTaskStatus-344", "Select TM_VC50_Deve_Status from T040021 where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK= " & TaskNumber & " and TM_NU9_Comp_ID_FK=" & compID)
            If strPrvStatus <> "PROGRESS" Then
                If SQL.Update("WSSUpdate", "UpdateTaskStatus-221", "update T040021 set TM_VC50_Deve_status='PROGRESS' where TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Task_no_PK= " & TaskNumber & " and TM_NU9_Comp_ID_FK=" & compID, SQL.Transaction.ReadUncommitted) = True Then
                    stReturn.ErrorMessage = ""
                    stReturn.FunctionExecuted = True
                    stReturn.ErrorCode = 0
                Else
                    stReturn.ErrorMessage = "Server is busy please try later..."
                    stReturn.FunctionExecuted = False
                    stReturn.ErrorCode = 1
                End If
                'Creating Log
                If stReturn.ErrorCode = 0 Then
                    strSql = "INSERT into T990021(TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK," & _
                                   " TM_NU9_Code,TM_VC100_Req_Status,TM_VC1000_Subtsk_Desc," & _
                                   " TM_DT8_Task_Date,TM_VC8_task_type,TM_VC8_Supp_Owner,TM_DT8_Plan_Cl_Date,TM_FL8_Est_Hr, " & _
                                   " TM_NU9_Dependency,TM_DT8_Close_Date,TM_VC100_Comp_Actions,TM_NU9_Forward_To,TM_NU9_At_No, " & _
                                   " TM_VC50_Close_case,TM_DT8_Est_close_date,TM_NU9_Attach_no,TM_NU9_Assign_by,TM_VC50_Deve_status," & _
                                   " TM_NU9_Est_Bill_Hrs,TM_DT8_Task_Close_Date,TM_NU9_Draft_Invoice,TM_NU9_Bargain_Loss," & _
                                   " TM_VC50_Fully_Paid,TM_NU9_Non_Bill_Hrs,TM_FL8_Actual_Bill_Hrs,TM_DT8_Remind_date,TM_VC16_Remind_time," & _
                                   " TM_VC50_Remind_Status, TM_NU9_Forwd_grp, TM_NU9_Forwd_emp, TM_NU9_forwd_Call, TM_DT8_Forwd_Dt_Time,  " & _
                                   " TM_VC8_Project,TM_VC8_Priority,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Mandatory,TM_NU9_Case_No,TM_NU9_Agmt_No," & _
                                   " TM_CH1_Forms, TM_CH1_Invoice_Pending, TM_VC50_Cat_Code_1, TM_VC50_Cat_Code_2, TM_VC50_Cat_Code_3, TM_VC50_Cat_Code_4, " & _
                                   " TM_VC50_Cat_Code_5,TM_DT8_Log_Date,TM_NU4_Event_ID,TM_CH1_MailSent,TM_VC8_Call_Status,TM_VC8_Call_Type,TM_NU9_ModifyBy )" & _
                                    " select TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK  " & _
                                   " ,TM_NU9_Code,TM_VC100_Req_Status,TM_VC1000_Subtsk_Desc, " & _
                                   " TM_DT8_Task_Date,TM_VC8_task_type,TM_VC8_Supp_Owner,TM_DT8_Plan_Cl_Date,TM_FL8_Est_Hr, " & _
                                   " TM_NU9_Dependency,TM_DT8_Close_Date,TM_VC100_Comp_Actions,TM_NU9_Forward_To,TM_NU9_At_No, " & _
                                   " TM_VC50_Close_case,TM_DT8_Est_close_date,TM_NU9_Attach_no,TM_NU9_Assign_by,'PROGRESS', " & _
                                   " TM_NU9_Est_Bill_Hrs,TM_DT8_Task_Close_Date,TM_NU9_Draft_Invoice,TM_NU9_Bargain_Loss, " & _
                                   " TM_VC50_Fully_Paid,TM_NU9_Non_Bill_Hrs,TM_FL8_Actual_Bill_Hrs,TM_DT8_Remind_date,TM_VC16_Remind_time, " & _
                                   " TM_VC50_Remind_Status,TM_NU9_Forwd_grp,TM_NU9_Forwd_emp,TM_NU9_forwd_Call,TM_DT8_Forwd_Dt_Time, " & _
                                  " TM_VC8_Project,TM_VC8_Priority,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Mandatory,TM_NU9_Case_No,TM_NU9_Agmt_No, " & _
                                   " TM_CH1_Forms,TM_CH1_Invoice_Pending,TM_VC50_Cat_Code_1,TM_VC50_Cat_Code_2,TM_VC50_Cat_Code_3,TM_VC50_Cat_Code_4, " & _
                                   " TM_VC50_Cat_Code_5,getdate() as TM_DT8_Log_Date,16 as TM_NU4_Event_ID,'F' as TM_CH1_MailSent,CN_VC20_Call_Status, " & _
                                   " CM_VC8_Call_Type," & HttpContext.Current.Session("PropUserId") & "  " & _
                                   " from t040021,t040011  " & _
                                   " Where  TM_Nu9_Call_No_Fk=CM_Nu9_Call_No_Pk AND TM_NU9_Comp_ID_Fk=CM_NU9_Comp_ID_Fk " & _
                                   " AND TM_Nu9_Call_No_Fk=" & CallNumber & " and TM_NU9_Comp_ID_Fk=" & compID & " and Tm_nu9_Task_no_pk=  " & TaskNumber

                    SQL.Save("WSSUpdate", "UpdateTaskStatus-277", strSql, SQL.Transaction.ReadCommitted)
                End If
            End If
            SQL.Update("WSSUpdate", "UpdateTaskStatus-269", "update T990031 set AM_VC8_Task_status='PROGRESS' where AM_NU9_Call_Number=" & CallNumber & " and AM_NU9_Task_Number= " & TaskNumber & " and AM_NU9_Comp_ID_FK=" & compID & " and AM_VC8_Task_status is null ", SQL.Transaction.Serializable)
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateTaskStatus-245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Public Shared Function UpdateAction(ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer, ByVal compID As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue

        Dim stReturn As ReturnValue
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False

        Try
            If SQL.Update("T040031", "WSSUpdate", "UpdateAction-250", "Select * from T040031 where AM_NU9_Call_Number=" & CallNumber & " and AM_NU9_Task_Number= " & TaskNumber & " and AM_NU9_Action_Number=" & ActionNumber & " and AM_NU9_Comp_ID_FK=" & compID, ColumnName, RowData) = True Then
                Call WSSUpdate.UpdateCallHours(compID, CallNumber)
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            ' Creating Log
            If stReturn.ErrorCode = 0 Then
                Dim blnStatus As Boolean
                Dim dtrCall As SqlDataReader
                ColumnName.Add("AM_NU9_Action_Number")
                ColumnName.Add("AM_NU9_Task_Number")
                ColumnName.Add("AM_NU9_Call_Number")
                ColumnName.Add("AM_NU9_Comp_ID_FK")
                ColumnName.Add("AM_NU4_Event_ID")
                ColumnName.Add("AM_CH1_MailSent")
                ColumnName.Add("AM_DT8_Log_Date")
                ColumnName.Add("AM_NU9_ModifyBy")

                RowData.Add(ActionNumber)
                RowData.Add(TaskNumber)
                RowData.Add(CallNumber)
                RowData.Add(compID)
                RowData.Add(19)
                RowData.Add("F")
                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))

                dtrCall = SQL.Search("wssupdate", "updateAction-350", "Select CM_VC8_Call_Type,CN_VC20_Call_Status,CM_VC200_Work_Priority From T040011 Where CM_NU9_Call_No_PK=" & CallNumber & " And CM_NU9_Comp_Id_FK= " & compID, SQL.CommandBehaviour.Default, blnStatus)
                dtrCall.Read()
                If blnStatus = True Then
                    ColumnName.Add("AM_VC8_Call_Type")
                    ColumnName.Add("AM_VC8_Call_Status")
                    ColumnName.Add("AM_VC8_Priority")
                    RowData.Add(dtrCall("CM_VC8_Call_Type"))
                    RowData.Add(dtrCall("CN_VC20_Call_Status"))
                    RowData.Add(dtrCall("CM_VC200_Work_Priority"))
                End If
                dtrCall = SQL.Search("wsssave", "saveaction-392", "Select TM_VC8_task_type,TM_VC50_Deve_status From T040021 Where TM_NU9_Task_no_PK=" & TaskNumber & " And TM_NU9_Call_No_FK=" & CallNumber & " And TM_NU9_Comp_ID_FK= " & compID, SQL.CommandBehaviour.Default, blnStatus)
                dtrCall.Read()
                If blnStatus = True Then
                    ColumnName.Add("AM_VC8_Task_Type")
                    ColumnName.Add("AM_VC8_Task_status")
                    RowData.Add(dtrCall("TM_VC8_task_type"))
                    RowData.Add(dtrCall("TM_VC50_Deve_status"))
                End If
                SQL.Save("T990031", "WSSUpdate", "UpdateAction-322", ColumnName, RowData)
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateAction-273", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region "Update Call, Task, Action For Attachment"
    Public Shared Function UpdateForAttachment(ByVal CallNo As Int32, ByVal TaskNo As Int32, ByVal ActionNo As Int32, ByVal compID As Int32, ByVal ALvl As AttachLevel)
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False
        Select Case ALvl
            Case mdlMain.AttachLevel.CallLevel
            Case mdlMain.AttachLevel.TaskLevel
                SQL.Update("WSSUpdate", "UpdateForAttachment-281", "Update T040021 Set TM_CH1_Attachment='1' Where TM_NU9_Call_No_FK=" & CallNo.ToString & " And TM_NU9_Comp_ID_FK=" & compID & " and TM_NU9_Task_no_PK= " & TaskNo.ToString, SQL.Transaction.ReadCommitted)
            Case mdlMain.AttachLevel.ActionLevel
                SQL.Update("WSSUpdate", "UpdateForAttachment-284", "Update T040031 Set AM_CH1_Attachment='1' Where AM_NU9_Call_Number=" & CallNo.ToString & " And AM_NU9_Task_Number= " & TaskNo.ToString & " And AM_NU9_Action_Number=" & ActionNo.ToString, SQL.Transaction.ReadCommitted)
        End Select
    End Function

    Public Shared Function UpdateForTemplateAttachment(ByVal CallNo As Int32, ByVal TaskNo As Int32, ByVal ActionNo As Int32, ByVal compID As Int32, ByVal ALvl As AttachLevel)
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False
        Select Case ALvl
            Case mdlMain.AttachLevel.TaskLevel
                ' SQL.DBTable = "T050031"
                SQL.Update("WSSUpdate", "UpdateForTemplateAttachment-294", "Update T050031 Set TTM_CH1_Attachment='1' Where TTM_NU9_Call_No_FK=" & CallNo.ToString & " And TTM_NU9_Task_no_PK= " & TaskNo.ToString & " and TTM_NU9_Comp_ID_FK=" & compID, SQL.Transaction.ReadCommitted)
        End Select
    End Function

#End Region

#Region "Template Call/Task"
    Public Shared Function UpdateTemplateCallStatus(ByVal CallNumber As Integer, ByVal CompanyID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            Dim strUpdate As String
            Dim strTStatus As String = SQL.Search("WSSUpdate", "UpdateTemplCallStatus-313", "select tm_nu9_task_no_pk from T040021 where TM_NU9_Call_No_fk=" & CallNumber & " and TM_NU9_Comp_Id_FK=" & CompanyID & "")
            Dim strAStatus As String = SQL.Search("WSSUpdate", "UpdateTemplCallStatus-314", "select AM_nu9_action_number from T040031 where AM_NU9_Call_Number=" & CallNumber & " and AM_NU9_Comp_Id_FK=" & CompanyID & "")
            If IsNothing(strTStatus) = True Then
                strUpdate = "update T040011 set CN_VC20_Call_Status='OPEN' where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID & ""
            Else
                If IsNothing(strAStatus) = True Then
                    strUpdate = "update T040011 set CN_VC20_Call_Status='ASSIGNED' where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID & ""
                Else
                    strUpdate = "update T040011 set CN_VC20_Call_Status='PROGRESS' where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID & ""
                End If
            End If
            If SQL.Update("WSSUpdate", "UpdateTemplCallStatus-328", strUpdate, SQL.Transaction.Serializable) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateTemplateCallStatus-349", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
    Public Shared Function UpdateTemplateCall(ByVal TemplateNumber As Integer, ByVal Company As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Update("T050021", "WSSUpdate", "UpdateTemplCall-358", "select * from T050021 where TCM_NU9_TemplateID_FK=" & TemplateNumber & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "TemplateUpdateCall-334", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Public Shared Function UpdateTemplateTask(ByVal TemplateID As Integer, ByVal TaskNumber As Integer, ByVal ColumnName As ArrayList, ByVal RowValue As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SQL.DBTracing = False
        Try
            If SQL.Update("T050031", "WSSUpdate", "UpdateTemplTask-387", "Select * from T050031 where TTM_NU9_TemplateID_FK=" & TemplateID & " and TTM_NU9_Task_no_PK= " & TaskNumber & "", ColumnName, RowValue) = True Then
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateTemplateTask-362", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region "Agreement"
    Public Shared Function UpdateAgreement(ByVal AgreementID As Integer, ByVal Company As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Update("T080011", "WSSUpdate", "UpdateAgreement-420", "select * from T080011 where AG_NU8_ID_PK=" & AgreementID & " And AG_VC8_Cust_Name='" & Company & "'", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateAgreement-395", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
    Public Shared Function UpdateInvoice(ByVal InvID As Integer, ByVal Company As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Update("T080031", "WSSUpdate", "UpdateAgreement-420", "select * from T080031 where IM_NU9_Invoice_ID_PK=" & InvID & " And IM_NU9_Company_ID_PK='" & Company & "'", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Error occured while updating records"
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateAgreement-395", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " SubCategory "
    Public Shared Function UpdateProject(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal tblName As String, ByVal SelectQry As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Update(tblName, "WSSUpdate", "UpdateProject-696", SelectQry, ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateProject-711", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

#Region " Document/Folder Management "
    Public Shared Function UpdateFolder(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal tblName As String, ByVal SelectQry As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Update(tblName, "WSSUpdate", "UpdateFolder-782", SelectQry, ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateFolder-797", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

#Region " update file Details "
    Public Shared Function UpdateFile(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal tblName As String, ByVal SelectQry As String) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Update(tblName, "WSSUpdate", "UpdateFile-814", SelectQry, ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateFile-829", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

#Region "Save EmailDate"
    ''' <summary>
    ''' This Function is used to save the Data in T990011 to send Mail on user request 
    ''' CreatedBy:Mandeep
    ''' CreatedOn:22/07/09
    ''' </summary>
    ''' <param name="CallNumber"></param>
    ''' <param name="Company"></param>
    ''' <param name="ColumnName"></param>
    ''' <param name="RowData"></param>
    ''' <param name="StatusChanged"></param>
    ''' <param name="CallStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveEmailData(ByVal CallNumber As Integer, ByVal Company As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, Optional ByVal StatusChanged As Boolean = False, Optional ByVal CallStatus As String = "") As ReturnValue
        Dim stReturn As ReturnValue
        Dim i As Int16
        Dim blnFound As Boolean
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            stReturn.ErrorCode = 0
            If stReturn.ErrorCode = 0 Then
                ColumnName.Add("CM_NU9_Call_No_PK")
                ColumnName.Add("CM_NU9_Comp_Id_FK")
                ColumnName.Add("CM_NU4_Event_ID")
                ColumnName.Add("CM_CH1_MailSent")
                ColumnName.Add("CM_DT8_Log_Date")
                ColumnName.Add("CM_NU9_ModifyBy")
                RowData.Add(CallNumber)
                RowData.Add(Company)
                ' Add event if status is closed then 14 else 19 
                For i = (RowData.Count - 1) To 0 Step -1
                    If IsDBNull(RowData(i)) OrElse RowData(i).ToString <> "CLOSED" Then
                        blnFound = False
                    Else
                        blnFound = True
                        Exit For
                    End If
                Next
                If blnFound = True Then 'Task is closed
                    RowData.Add(14)
                ElseIf StatusChanged = True Then
                    RowData.Add(13)
                Else
                    RowData.Add(19)
                End If
                RowData.Add("F")
                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))
                SQL.Save("T990011", "WSSUpdate", "UpdateCall-129", ColumnName, RowData)
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "SaveEmailData-900", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region
End Class