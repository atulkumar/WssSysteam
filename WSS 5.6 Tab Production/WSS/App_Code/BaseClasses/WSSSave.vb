Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Text

Public Class WSSSave

    Enum AttachLevel
        CallLevel = 1
        TaskLevel = 2
        ActionLevel = 3
    End Enum


#Region " UDC "

    Public Shared Function SaveUDCType(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            '  SQL.DBTable = "UDCType"
            SQL.DBTracing = False

            If SQL.Save("UDCType", "WSSSave", "SaveUDCType-28", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveUDCType-43", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Public Shared Function SaveUDC(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            '    SQL.DBTable = "UDC"
            SQL.DBTracing = False

            If SQL.Save("UDC", "WSSSave", "SaveUDC-58", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveUDC-73", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " Ab_Main "

    Public Shared Function SavePersonalInfo(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' SQL.DBTable = "T010043"
            SQL.DBTracing = False

            If SQL.Save("T010043", "WSSSave", "SavePersonalInfo-92", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SavePersonalInfo-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Public Shared Function SaveCategory(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            '  SQL.DBTable = "T010053"
            SQL.DBTracing = False

            If SQL.Save("T010053", "WSSSave", "SaveCategory-122", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveCategory-137", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " Create User Login "

    Public Shared Function SaveUser(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' Table name
            '  SQL.DBTable = "T010061"
            SQL.DBTracing = False

            If SQL.Save("T010061", "WSSSave", "saveUser-157", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveUser-172", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

#End Region

#Region " Call, Task, Action Detail "

    Public Shared Function SaveCall(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal Company As String, Optional ByVal CallStatus As String = "") As ReturnValue
        Dim stReturn As ReturnValue
        Dim i As Int16
        Dim blnFound As Boolean
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            '         Dim intCallNo As Integer = SQL.Search("WSSSave", "SaveCall-191", "select max(CM_NU9_Call_No_PK) from t040011 where CM_NU9_Comp_Id_FK=" & Company & "")
            '		intCallNo += 1
            Dim intCallNo As Integer = clsNextNo.GetNextNo(102, Val(Company), strConnection)
            'CreateLog("WSSSAVE", "SC-194", LogType.Application, LogSubType.Information, "999", HttpContext.Current.Session("PropCallNumber"), HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), intCallNo)
            ColumnName.Add("CM_NU9_Call_No_PK")
            RowData.Add(intCallNo)

            If SQL.Save("T040011", "WSSSave", "SaveCall-198", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                stReturn.ExtraValue = intCallNo
                'HttpContext.Current.Session("PropCallNumber") = intCallNo
            End If
            ' Creating Log

            If stReturn.ErrorCode = 0 Then

                ColumnName.Add("CM_NU4_Event_ID")
                ColumnName.Add("CM_CH1_MailSent")
                ColumnName.Add("CM_DT8_Log_Date")
                ColumnName.Add("CM_NU9_ModifyBy")
                ' Add event if status is closed then 14 else 1 
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
                Else
                    RowData.Add(1)
                End If
                '   If CallStatus.ToUpper.Equals(System.Configuration.ConfigurationSettings.AppSettings("NoMailCallStatus").ToString.ToUpper) Then
                'RowData.Add("T")
                '  Else
                RowData.Add("F")
                '   End If
                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))

                SQL.Save("T990011", "", "SaveCall-212", ColumnName, RowData)
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveCall-215", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

    Public Shared Function SaveMail(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Dim intCallNo As Integer = SQL.Search("WSSSave", "SaveMail-224", "select max(SM_NU9_ID_PK) from T040071")

        intCallNo += 1

        Try
            ColumnName.Add("SM_NU9_ID_PK")
            RowData.Add(intCallNo)

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' Table name
            '            SQL.DBTable = "T040071"
            SQL.DBTracing = False

            If SQL.Save("T040071", "WSSSave", "SaveMail-239", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                stReturn.ExtraValue = intCallNo
                ' HttpContext.Current.Session("PropCallNumber") = intCallNo
                ' HttpContext.Current.Session("PropCallNumber") = intCallNo
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveCall-257", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

    Public Shared Function SaveTask(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal CompanyID As Integer, ByVal CallNo As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim i As Int16
        Dim blnFound As Boolean
        Dim ColumnName1 As New ArrayList
        Try

            Dim intTaskOrder As Integer
            If Not (ColumnName.Contains("TM_NU9_Task_Order")) Then
                intTaskOrder = SQL.Search("CallView", "SaveTask-2573", "select count(*)+1 from T040021 where TM_NU9_Call_No_FK=" & Val(CallNo) & " and TM_NU9_Comp_ID_FK=" & Val(CompanyID))
                'ColumnName.Remove("TM_NU9_Task_Order")
                ColumnName.Add("TM_NU9_Task_Order")
                RowData.Add(intTaskOrder.ToString)
            End If

            
            'make independent copy of column arraylist
            ColumnName1 = ColumnName.Clone
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' Table name
            'SQL.DBTable = "T040021"
            SQL.DBTracing = False


            If SQL.Save("T040021", "WSSSave", "SaveTask-274", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                Call WSSUpdate.UpdateCallHours(Val(CompanyID), Val(CallNo))
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If
            ' Creating Log
            '  SQL.DBTable = "T990021"
            If stReturn.ErrorCode = 0 Then
                ColumnName1.Add("TM_NU4_Event_ID")
                ColumnName1.Add("TM_CH1_MailSent")
                ColumnName1.Add("TM_DT8_Log_Date")
                ColumnName1.Add("TM_NU9_ModifyBy")

                ' Add event if status is closed then 15 else 2 
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
                Else
                    RowData.Add(2)
                End If

                RowData.Add("F")
                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))

                SQL.Save("T990021", "WSSSave", "SaveTask-299", ColumnName1, RowData)
                '   SQL.DBTable = "T040021"
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveTask-289", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

    Public Shared Function SaveAction(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal CompanyID As Integer, ByVal CallNo As Integer, ByVal taskNo As Integer) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' Table name
            '     SQL.DBTable = "T040031"
            SQL.DBTracing = False

            If SQL.Save("T040031", "WSSSave", "SaveAction-306", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                Call WSSUpdate.UpdateCallHours(Val(CompanyID), Val(CallNo))
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            ' Creating Log
            If stReturn.ErrorCode = 0 Then
                Dim blnStatus As Boolean
                Dim dtrCall As SqlDataReader
                ColumnName.Add("AM_NU4_Event_ID")
                ColumnName.Add("AM_CH1_MailSent")
                ColumnName.Add("AM_DT8_Log_Date")
                ColumnName.Add("AM_NU9_ModifyBy")

                RowData.Add(3)
                RowData.Add("F")
                RowData.Add(Now)
                RowData.Add(HttpContext.Current.Session("PropUserId"))


                ' SQL.DBTable = "T040011"
                dtrCall = SQL.Search("wsssave", "saveaction-350", "Select CM_VC8_Call_Type,CN_VC20_Call_Status,CM_VC200_Work_Priority From T040011 Where CM_NU9_Call_No_PK=" & CallNo & " And CM_NU9_Comp_Id_FK= " & CompanyID, SQL.CommandBehaviour.Default, blnStatus)
                If blnStatus = True Then
                    dtrCall.Read()
                    ColumnName.Add("AM_VC8_Call_Type")
                    ColumnName.Add("AM_VC8_Call_Status")
                    ColumnName.Add("AM_VC8_Priority")
                    RowData.Add(dtrCall("CM_VC8_Call_Type"))
                    RowData.Add(dtrCall("CN_VC20_Call_Status"))
                    RowData.Add(dtrCall("CM_VC200_Work_Priority"))
                End If
                '   SQL.DBTable = "T040021"
                dtrCall = SQL.Search("wsssave", "saveaction-392", "Select TM_VC8_task_type,TM_VC50_Deve_status From T040021 Where TM_NU9_Task_no_PK=" & taskNo & " And TM_NU9_Call_No_FK=" & CallNo & " And TM_NU9_Comp_ID_FK= " & CompanyID, SQL.CommandBehaviour.Default, blnStatus)
                If blnStatus = True Then
                    dtrCall.Read()
                    ColumnName.Add("AM_VC8_Task_Type")
                    ColumnName.Add("AM_VC8_Task_status")
                    RowData.Add(dtrCall("TM_VC8_task_type"))
                    RowData.Add(dtrCall("TM_VC50_Deve_status"))
                End If
                'SQL.DBTable = "T990031"
                SQL.Save("T990031", "WSSSave", "SaveAction-362", ColumnName, RowData)
                'dtrCall.Close()
                'SQL.DBTable = "T040031"
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveAction-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try

    End Function

#End Region

#Region " Comments "

    Public Shared Function SaveComments(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, Optional ByVal OPT As Integer = 0) As ReturnValue
        Dim stReturn As ReturnValue
        Dim tblComment As String
        If OPT = 2 Then
            tblComment = "T050061"
        Else
            tblComment = "T040061"
        End If
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' SQL.DBTable = tblComment
            SQL.DBTracing = False

            Dim intComment As Integer
            intComment = SQL.Search("WSSSave", "SaveComments-347", "select max(CM_NU9_Comment_Number_PK) from " & tblComment)

            intComment += 1
            ColumnName.Add("CM_NU9_Comment_Number_PK")
            RowData.Add(intComment)

            If SQL.Save(tblComment, "WSSSave", "SaveComments-353", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2

            CreateLog("WWSSave", "SaveComments-369", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " Template "

    Public Shared Function SaveTemplate(ByVal TemplateID As Integer, ByVal TemplateType As String, ByVal CompanyID As Integer, ByVal CallNo As Integer, Optional ByVal Update As Boolean = False) As ReturnValue

        Dim stReturn As ReturnValue
        Dim dirTemp As DirectoryInfo
        Dim intLastTaskNo As Integer

        'Array list which set the Dependency date  ------ 27/03/2008
        Dim arTmpTaskNo As New ArrayList
        Dim arTaskNo As New ArrayList
        Dim arDepTaskNo As New ArrayList

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SQL.DBTracing = False
            Dim blnCheck As Boolean
            Dim sqrdTemplate As SqlClient.SqlDataReader

            If TemplateType = "TAO" Or TemplateType = "CNT" Then
                'Modified by Atul for Task Order
                sqrdTemplate = SQL.Search("WSSSave", "SaveTemplate-392", "Select TTM_DT8_Task_Date,TTM_NU9_Call_No_FK,TTM_NU9_Comp_ID_FK,TTM_VC50_Deve_status,TTM_VC1000_Subtsk_Desc,TTM_VC8_task_type,TTM_VC8_Project,TTM_NU9_Project_ID,TTM_VC8_Supp_Owner,TTM_NU9_Assign_by,TTM_VC8_Priority,TTM_CH1_Comment,TTM_CH1_Mandatory,TTM_DT8_Est_close_date,TTM_NU9_Task_no_PK,TTM_CH1_Attachment,TTM_CH1_Forms,TTM_FL8_Est_Hr,TTM_NU9_Dependency,TTM_NU9_Task_Order from T050031 where TTM_NU9_TemplateID_FK=" & TemplateID & " order by TTM_NU9_Task_Order", SQL.CommandBehaviour.CloseConnection, blnCheck)

                Dim i As Int16
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList

                arColumnName.Add("TM_DT8_Task_Date")
                arColumnName.Add("TM_NU9_Call_No_FK")
                arColumnName.Add("TM_NU9_Comp_ID_FK")
                arColumnName.Add("TM_VC50_Deve_status")
                arColumnName.Add("TM_VC1000_Subtsk_Desc")
                arColumnName.Add("TM_VC8_task_type")
                arColumnName.Add("TM_VC8_Project")
                arColumnName.Add("TM_NU9_Project_ID")
                arColumnName.Add("TM_VC8_Supp_Owner")
                arColumnName.Add("TM_NU9_Assign_by")
                arColumnName.Add("TM_VC8_Priority")
                arColumnName.Add("TM_CH1_Comment")
                arColumnName.Add("TM_CH1_Mandatory")
                arColumnName.Add("TM_DT8_Est_close_date")
                arColumnName.Add("TM_CH1_Attachment")
                arColumnName.Add("TM_CH1_Forms")
                arColumnName.Add("TM_NU9_Task_no_PK")
                arColumnName.Add("TM_FL8_Est_Hr")
                arColumnName.Add("TM_NU9_Dependency")
                arColumnName.Add("TM_NU9_Task_Order")
                intLastTaskNo = SQL.Search("WSSSave", "SaveTemplate-412", "select isnull(max(TM_NU9_Task_no_PK),0) from T040021 where TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Comp_ID_FK=" & CompanyID)
                'Add By atul
                Dim intLastTaskOrder As Int32
                intLastTaskOrder = SQL.Search("WSSSave", "SaveTemplate-412", "select isnull(max(TM_NU9_Task_Order),0) from T040021 where TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Comp_ID_FK=" & CompanyID)

                Dim intOldTaskno As Integer = 0
                While sqrdTemplate.Read
                    Dim intTaskno As Integer = SQL.Search("WSSSave", "SaveTemplate-415", "select isnull(max(TM_NU9_Task_no_PK),0) from T040021 where TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Comp_ID_FK=" & CompanyID)
                    'Add By atul for Task Order
                    If (intLastTaskOrder = 0) Then
                        intLastTaskOrder = 0
                    Else
                        intLastTaskOrder += 1
                    End If

                    intTaskno += 1
                    intOldTaskno += 1


                    arTaskNo.Add(intTaskno)
                    arDepTaskNo.Add(IIf(IsDBNull(sqrdTemplate.Item("TTM_NU9_Dependency")), 0, sqrdTemplate.Item("TTM_NU9_Dependency")))
                    arTmpTaskNo.Add(sqrdTemplate.Item("TTM_NU9_Task_no_PK"))

                    'no need to copy task date from template as it will be current date
                    'arRowData.Add(sqrdTemplate.Item("TTM_DT8_Task_Date"))
                    arRowData.Add(Now)
                    arRowData.Add(CallNo)
                    arRowData.Add(sqrdTemplate.Item("TTM_NU9_Comp_ID_FK"))
                    arRowData.Add(sqrdTemplate.Item("TTM_VC50_Deve_status"))
                    arRowData.Add(sqrdTemplate.Item("TTM_VC1000_Subtsk_Desc"))
                    arRowData.Add(sqrdTemplate.Item("TTM_VC8_task_type"))
                    arRowData.Add(sqrdTemplate.Item("TTM_VC8_Project"))
                    arRowData.Add(sqrdTemplate.Item("TTM_NU9_Project_ID"))
                    arRowData.Add(sqrdTemplate.Item("TTM_VC8_Supp_Owner"))
                    arRowData.Add(sqrdTemplate.Item("TTM_NU9_Assign_by"))
                    arRowData.Add(sqrdTemplate.Item("TTM_VC8_Priority"))
                    arRowData.Add(sqrdTemplate.Item("TTM_CH1_Comment"))
                    arRowData.Add(sqrdTemplate.Item("TTM_CH1_Mandatory"))
                    arRowData.Add(System.DBNull.Value)
                    arRowData.Add(sqrdTemplate.Item("TTM_CH1_Attachment"))
                    arRowData.Add(sqrdTemplate.Item("TTM_CH1_Forms"))
                    arRowData.Add(intTaskno)
                    arRowData.Add(sqrdTemplate.Item("TTM_FL8_Est_Hr"))
                    arRowData.Add(sqrdTemplate.Item("TTM_NU9_Dependency"))
                    'Add By atul for Task Order
                    If (intLastTaskOrder = 0) Then
                        arRowData.Add(sqrdTemplate.Item("TTM_NU9_Task_Order"))
                    Else
                        arRowData.Add(intLastTaskOrder.ToString())
                    End If

                    mstGetFunctionValue = SaveTask(arColumnName, arRowData, CompanyID, CallNo)
                    'HttpContext.Current.Session("PropTaskNumber") = intTaskno
                    arRowData.Clear()

                    'clear array for second round save 
                    '****************************************
                    If arColumnName.Count = 21 Then
                        arColumnName.RemoveAt(arColumnName.Count - 1)
                        arColumnName.RemoveAt(arColumnName.Count - 1)
                        arColumnName.RemoveAt(arColumnName.Count - 1)
                    End If

                    '***************************************
                    'Copy Comments from template to general comment table
                    If CopyFromTemplateComments(TemplateID, intTaskno, intOldTaskno, CallNo, CompanyID) = True Then
                    End If
                    'Copy Forms from template to general Forms table if template Type is CNT
                    If TemplateType = "CNT" Then
                        CopyFromTemplateForm(TemplateID, intTaskno, intOldTaskno, CallNo, CompanyID)
                    End If
                End While
                sqrdTemplate.Close()

                If mstGetFunctionValue.ErrorCode = 0 Then
                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Records Saved successfully..."
                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "Error Accured ! please try later..."
                End If
            End If
            ' Save Attachment - T040041 to T050041
            If CallNo > 0 Then
                sqrdTemplate = SQL.Search("WSSSave", "SaveTemplate-454", "Select * from T050041 where AT_NU9_TemplateID_FK=" & TemplateID & " And AT_VC1_Status='U'", SQL.CommandBehaviour.CloseConnection, blnCheck)
                If blnCheck = True Then
                    Dim arColumnName As New ArrayList
                    Dim arRowData As New ArrayList
                    Dim strPath As String

                    arColumnName.Add("AT_NU9_File_ID_PK")
                    arColumnName.Add("AT_VC255_File_Name")
                    arColumnName.Add("AT_VC255_File_Size")
                    arColumnName.Add("AT_VC255_File_Path")
                    arColumnName.Add("AT_VC1_Status")
                    arColumnName.Add("AT_NU9_Address_Book_Number")
                    arColumnName.Add("AT_NU9_Call_Number")
                    arColumnName.Add("AT_NU9_Task_Number")
                    arColumnName.Add("AT_NU9_Action_Number")
                    arColumnName.Add("AT_NU9_CompId_Fk")
                    arColumnName.Add("AT_DT8_Date")
                    arColumnName.Add("AT_VC8_Role")
                    arColumnName.Add("AT_NU9_Version")
                    arColumnName.Add("AT_DT8_Modify_Date")
                    arColumnName.Add("AT_IN4_Level")
                    arColumnName.Add("VH_VC4_Active_Status")

                    While sqrdTemplate.Read
                        Dim intTaskno As Integer = SQL.Search("WSSSave", "SaveTemplate-477", "select isnull(max(AT_NU9_File_ID_PK),0) from T040041")
                        intTaskno += 1
                        arRowData.Clear()
                        arRowData.Add(intTaskno)
                        arRowData.Add(sqrdTemplate.Item("AT_VC255_File_Name"))
                        arRowData.Add(sqrdTemplate.Item("AT_VC255_File_Size"))

                        strPath = "Dockyard/" & CompanyID & "/" & CallNo & "/" & intLastTaskNo + sqrdTemplate.Item("AT_NU9_Task_Number") & "/" & sqrdTemplate.Item("AT_NU9_Version") & "/" & sqrdTemplate.Item("AT_VC255_File_Name")
                        strPath = strPath.Replace("\", "/")
                        '-----------------------------------------------------------------------------------------
                        arRowData.Add(strPath)
                        arRowData.Add(sqrdTemplate.Item("AT_VC1_Status"))
                        arRowData.Add(sqrdTemplate.Item("AT_NU9_Address_Book_Number"))
                        arRowData.Add(CallNo)
                        If sqrdTemplate.Item("AT_NU9_Task_Number") = 0 Then
                            arRowData.Add(intLastTaskNo + 1)
                        Else
                            arRowData.Add(intLastTaskNo + sqrdTemplate.Item("AT_NU9_Task_Number"))
                        End If
                        arRowData.Add(sqrdTemplate.Item("AT_NU9_Action_Number"))
                        arRowData.Add(CompanyID)
                        arRowData.Add(sqrdTemplate.Item("AT_DT8_Date"))
                        arRowData.Add(sqrdTemplate.Item("AT_VC8_Role"))
                        arRowData.Add(sqrdTemplate.Item("AT_NU9_Version"))
                        arRowData.Add(sqrdTemplate.Item("AT_DT8_Modify_Date"))
                        arRowData.Add(sqrdTemplate.Item("AT_IN4_Level"))
                        arRowData.Add(sqrdTemplate.Item("VH_VC4_Active_Status"))
                        'SQL.DBTable = "T040041"
                        If SQL.Save("T040041", "WSSSave", "SaveTemplate-504", arColumnName, arRowData) = False Then
                            stReturn.ErrorCode = 1
                            stReturn.ErrorMessage = "Error occured while saving records"
                        End If
                        If stReturn.ErrorCode = 0 And TemplateType <> "TAO" And sqrdTemplate.Item("AT_NU9_Task_Number") = "0" Then
                            ' SQL.DBTable = "T040011"
                            SQL.Update("WssSave", "SaveTemplateAttachmentCall-804", "Update T040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & CompanyID & "", SQL.Transaction.Serializable)
                        End If
                        ' File Copy ------------------------------------------------------------
                        Try

                            If Not (sqrdTemplate.Item("AT_VC255_File_Path") Is DBNull.Value) Then

                                'Create New Path-----------------------------------------------------------------
                                strPath = HttpContext.Current.Session("PropRootDir") & "/" & "Dockyard/" & CompanyID & "/" & CallNo & "/" & intLastTaskNo + sqrdTemplate.Item("AT_NU9_Task_Number") & "/" & sqrdTemplate.Item("AT_NU9_Version") & "/" & sqrdTemplate.Item("AT_VC255_File_Name")
                                strPath = strPath.Replace("\", "/")
                                '-----------------------------------------------------------------------------------------
                                dirTemp = New DirectoryInfo(strPath.Substring(0, strPath.LastIndexOf("/")))

                                If dirTemp.Exists = False Then
                                    dirTemp.Create()
                                End If
                                Try
                                    File.Copy(HttpContext.Current.Session("PropRootDir").Replace("\", "/") & "/" & CType(sqrdTemplate.Item("AT_VC255_File_Path"), String), strPath, True)
                                Catch ex As Exception
                                End Try
                                'WSSUpdate.UpdateForAttachment(HttpContext.Current.Session("PropCallNumber"), sqrdTemplate.Item("AT_NU9_Task_Number"), 0, sqrdTemplate.Item("AT_IN4_Level"))
                            End If
                        Catch ex As Exception
                            CreateLog("WWSSave", "SaveTemplate-615", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                        End Try
                        '----------------------------------------------------------
                    End While
                    sqrdTemplate.Close()
                End If
                If mstGetFunctionValue.ErrorCode = 0 Then
                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Records Saved successfully..."

                    'Call function to update dependency which is set by the template
                    UpdateTmpDependency(CallNo, CompanyID, arDepTaskNo, arTaskNo, arTmpTaskNo)

                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "Server is busy please try later..."
                End If
            End If
            '--------
            'Save Version- T040051 to T050051
            If CallNo > 0 Then
                Dim strPath As String
                '	SQL.DBTable = "T050051"
                sqrdTemplate = SQL.Search("WSSSave", "SaveTemplate-545", "Select * from T050051 where VH_NU9_TemplateID_FK=" & TemplateID & "", SQL.CommandBehaviour.CloseConnection, blnCheck)
                If blnCheck = True Then
                    Dim arColumnName As New ArrayList
                    Dim arRowData As New ArrayList

                    arColumnName.Add("VH_NU9_File_ID_PK")
                    arColumnName.Add("VH_VC255_File_Name")
                    arColumnName.Add("VH_VC255_File_Size")
                    arColumnName.Add("VH_VC255_File_Path")
                    arColumnName.Add("VH_VC1_Status")
                    arColumnName.Add("VH_IN4_Level")
                    arColumnName.Add("VH_NU9_Address_Book_Number")
                    arColumnName.Add("VH_NU9_Call_Number")
                    arColumnName.Add("VH_NU9_Task_Number")
                    arColumnName.Add("VH_NU9_Action_Number")
                    arColumnName.Add("VH_NU9_CompId_Fk")
                    arColumnName.Add("VH_DT8_Date")
                    arColumnName.Add("VH_VC8_Role")
                    arColumnName.Add("VH_NU9_Version")
                    arColumnName.Add("VH_DT8_Modify_Date")
                    arColumnName.Add("VH_VC4_Active_Status")

                    While sqrdTemplate.Read
                        Dim intTaskno As Integer = SQL.Search("WSSSave", "SaveTemplate-567", "select isnull(max(VH_NU9_File_ID_PK),0) from T040051 ")
                        intTaskno += 1
                        arRowData.Clear()
                        arRowData.Add(intTaskno)

                        'Create New Path-----------------------------------------------------------------
                        strPath = "Dockyard/" & CompanyID & "/" & CallNo & "/" & intLastTaskNo + sqrdTemplate.Item("VH_NU9_Task_Number") & "/" & sqrdTemplate.Item("VH_NU9_Version") & "/" & sqrdTemplate.Item("VH_VC255_File_Name")
                        strPath = strPath.Replace("\", "/")
                        '-----------------------------------------------------------------------------------------
                        arRowData.Add(sqrdTemplate.Item("VH_VC255_File_Name"))
                        arRowData.Add(sqrdTemplate.Item("VH_VC255_File_Size"))
                        'arRowData.Add(sqrdTemplate.Item("VH_VC255_File_Path"))
                        arRowData.Add(strPath)
                        arRowData.Add(sqrdTemplate.Item("VH_VC1_Status"))
                        arRowData.Add(sqrdTemplate.Item("VH_IN4_Level"))
                        arRowData.Add(sqrdTemplate.Item("VH_NU9_Address_Book_Number"))
                        arRowData.Add(CallNo)
                        If sqrdTemplate.Item("VH_NU9_Task_Number") = 0 Then
                            arRowData.Add(intLastTaskNo + 1)
                        Else
                            arRowData.Add(intLastTaskNo + sqrdTemplate.Item("VH_NU9_Task_Number"))
                        End If
                        arRowData.Add(sqrdTemplate.Item("VH_NU9_Action_Number"))
                        arRowData.Add(CompanyID)
                        arRowData.Add(sqrdTemplate.Item("VH_DT8_Date"))
                        arRowData.Add(sqrdTemplate.Item("VH_VC8_Role"))
                        arRowData.Add(sqrdTemplate.Item("VH_NU9_Version"))
                        arRowData.Add(sqrdTemplate.Item("VH_DT8_Modify_Date"))
                        arRowData.Add(sqrdTemplate.Item("VH_VC4_Active_Status"))
                        'SQL.DBTable = "T040051"
                        If SQL.Save("T040051", "WSSSave", "SaveTemplate-596", arColumnName, arRowData) = False Then
                            stReturn.ErrorCode = 1
                            stReturn.ErrorMessage = "Error occured while saving records"
                        End If
                        ' File Copy ------------------------------------------------------------
                        Try

                            If Not (sqrdTemplate.Item("VH_VC255_File_Path") Is DBNull.Value) Then
                                'Create New Path-----------------------------------------------------------------
                                strPath = HttpContext.Current.Session("PropRootDir") & "/" & "Dockyard/" & CompanyID & "/" & CallNo & "/" & intLastTaskNo + sqrdTemplate.Item("VH_NU9_Task_Number") & "/" & sqrdTemplate.Item("VH_NU9_Version") & "/" & sqrdTemplate.Item("VH_VC255_File_Name")
                                strPath = strPath.Replace("\", "/")
                                '-----------------------------------------------------------------------------------------
                                dirTemp = New DirectoryInfo(strPath.Substring(0, strPath.LastIndexOf("/")))
                                If dirTemp.Exists = False Then
                                    dirTemp.Create()
                                End If
                                Try
                                    File.Copy(HttpContext.Current.Session("PropRootDir").Replace("\", "/") & "/" & CType(sqrdTemplate.Item("VH_VC255_File_Path"), String), strPath, True)
                                Catch ex As Exception
                                End Try
                            End If
                        Catch ex As Exception
                            CreateLog("WWSSave", "SaveTemplate-725", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                        End Try
                        '----------------------------------------------------------
                    End While
                    sqrdTemplate.Close()
                End If
                If mstGetFunctionValue.ErrorCode = 0 Then
                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Records Saved successfully..."
                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "Server is busy please try later..."
                End If
            End If
            '-------------------------------------

            Return stReturn
        Catch ex As Exception
            CreateLog("WSSSAVE", "SaveTemplate-635", LogType.System, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function
    'set dependency 
    Shared Function UpdateTmpDependency(ByVal intCallNo As Integer, ByVal intCompID As Integer, ByVal arDepTaskNo As ArrayList, ByVal arTaskNo As ArrayList, ByVal arTmpTaskNo As ArrayList)

        Try

            Dim i As Integer
            Dim j As Integer

            For i = 0 To arDepTaskNo.Count - 1
                If arDepTaskNo(i) > 0 Then
                    Dim TmpDepTaskNo As Integer
                    Dim TaskNo As Integer

                    TmpDepTaskNo = arDepTaskNo(i)
                    TaskNo = arTaskNo(i)

                    For j = 0 To arTmpTaskNo.Count - 1
                        If TmpDepTaskNo = arTmpTaskNo(j) Then
                            Dim NewDepTaskno As Integer
                            NewDepTaskno = arTaskNo(j)

                            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                            If SQL.Update("T040021", "UpdateTmpDependency-822", "Update T040021 set TM_NU9_Dependency=" & NewDepTaskno & " WHERE TM_NU9_Call_No_FK=" & Val(intCallNo) & " and TM_NU9_Comp_ID_FK=" & Val(intCompID) & " and TM_NU9_Task_no_PK=" & Val(TaskNo) & "", SQL.Transaction.Serializable) = True Then

                            End If

                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            CreateLog("WSSSAVE", "UpdateTmpDependency-837", LogType.System, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function
    Public Shared Function SaveTemplateAttachmentCall(ByVal CallNumber As Integer, ByVal TemplateID As Integer, ByVal companyID As Integer)
        '-----------------------------------------------------------------------------------
        ' This procedure is made only to copy Attachment for Call from template 
        '-----------------------------------------------------------------------------------
        Dim blnCheck As Boolean
        Dim blnAttachmentExists As Boolean = False
        Dim stReturn As ReturnValue
        Dim sqrdTemplate As SqlDataReader
        Dim dirTemp As DirectoryInfo
        Try
            ' Save Attachment - T040041 to T050041
            If CallNumber > 0 Then
                sqrdTemplate = SQL.Search("WSSSave", "SaveTemplateAttachment-745", "Select * from T050041 where AT_NU9_TemplateID_FK=" & TemplateID & " And AT_VC1_Status='U' And AT_Nu9_Task_Number=0  and AT_IN4_LEVEL=1", SQL.CommandBehaviour.CloseConnection, blnCheck)
                If blnCheck = True Then
                    blnAttachmentExists = True
                    Dim arColumnName As New ArrayList
                    Dim arRowData As New ArrayList
                    Dim strPath As String

                    arColumnName.Add("AT_NU9_File_ID_PK")
                    arColumnName.Add("AT_VC255_File_Name")
                    arColumnName.Add("AT_VC255_File_Size")
                    arColumnName.Add("AT_VC255_File_Path")
                    arColumnName.Add("AT_VC1_Status")
                    arColumnName.Add("AT_NU9_Address_Book_Number")
                    arColumnName.Add("AT_NU9_Call_Number")
                    arColumnName.Add("AT_NU9_Task_Number")
                    arColumnName.Add("AT_NU9_Action_Number")
                    arColumnName.Add("AT_NU9_CompId_Fk")
                    arColumnName.Add("AT_DT8_Date")
                    arColumnName.Add("AT_VC8_Role")
                    arColumnName.Add("AT_NU9_Version")
                    arColumnName.Add("AT_DT8_Modify_Date")
                    arColumnName.Add("AT_IN4_Level")

                    While sqrdTemplate.Read
                        Dim intFileID As Integer = SQL.Search("WSSSave", "SaveTemplate-477", "select isnull(max(AT_NU9_File_ID_PK),0) from T040041")
                        intFileID += 1
                        arRowData.Clear()
                        arRowData.Add(intFileID)
                        arRowData.Add(sqrdTemplate.Item("AT_VC255_File_Name"))
                        arRowData.Add(sqrdTemplate.Item("AT_VC255_File_Size"))

                        strPath = "Dockyard/" & companyID & "/" & CallNumber & "/" & sqrdTemplate.Item("AT_NU9_Version") & "/" & sqrdTemplate.Item("AT_VC255_File_Name")
                        strPath = strPath.Replace("\", "/")
                        '-----------------------------------------------------------------------------------------
                        arRowData.Add(strPath)
                        arRowData.Add(sqrdTemplate.Item("AT_VC1_Status"))
                        arRowData.Add(sqrdTemplate.Item("AT_NU9_Address_Book_Number"))
                        arRowData.Add(CallNumber) 'Call Number
                        arRowData.Add(0) 'Task number
                        arRowData.Add(0) 'Action Number
                        arRowData.Add(companyID)
                        arRowData.Add(sqrdTemplate.Item("AT_DT8_Date"))
                        arRowData.Add(sqrdTemplate.Item("AT_VC8_Role"))
                        arRowData.Add(sqrdTemplate.Item("AT_NU9_Version"))
                        arRowData.Add(sqrdTemplate.Item("AT_DT8_Modify_Date"))
                        arRowData.Add(sqrdTemplate.Item("AT_IN4_Level"))
                        '   SQL.DBTable = "T040041"
                        If SQL.Save("T040041", "WSSSave", "SaveTemplate-504", arColumnName, arRowData) = False Then
                            stReturn.ErrorCode = 1
                            stReturn.ErrorMessage = "Server is busy please try later..."
                        End If
                        If stReturn.ErrorCode = 0 Then
                            '      SQL.DBTable = "T040011"
                            SQL.Update("T040011", "WssSave", "SaveTemplateAttachmentCall-804", "Update T040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & companyID & "", SQL.Transaction.Serializable)
                        End If
                        ' File Copy ------------------------------------------------------------
                        Try

                            If Not (sqrdTemplate.Item("AT_VC255_File_Path") Is DBNull.Value) Then

                                'Create New Path-----------------------------------------------------------------
                                strPath = HttpContext.Current.Session("PropRootDir") & "/" & "Dockyard/" & companyID & "/" & CallNumber & "/" & sqrdTemplate.Item("AT_NU9_Version") & "/" & sqrdTemplate.Item("AT_VC255_File_Name")
                                strPath = strPath.Replace("\", "/")
                                '-----------------------------------------------------------------------------------------
                                dirTemp = New DirectoryInfo(strPath.Substring(0, strPath.LastIndexOf("/")))

                                If dirTemp.Exists = False Then
                                    dirTemp.Create()
                                End If
                                File.Copy(HttpContext.Current.Session("PropRootDir").Replace("\", "/") & "/" & CType(sqrdTemplate.Item("AT_VC255_File_Path"), String), strPath, True)
                                'WSSUpdate.UpdateForAttachment(HttpContext.Current.Session("PropCallNumber"), sqrdTemplate.Item("AT_NU9_Task_Number"), 0, sqrdTemplate.Item("AT_IN4_Level"))
                            End If
                        Catch ex As Exception
                            CreateLog("WWSSave", "SaveTemplate-615", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                        End Try
                        '----------------------------------------------------------
                    End While
                    sqrdTemplate.Close()
                End If
                If mstGetFunctionValue.ErrorCode = 0 Then
                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Records Saved successfully..."
                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "Server is busy please try later..."
                End If
            End If
            '--------
            If blnAttachmentExists = True Then
                'Save Version- T040051 to T050051
                If CallNumber > 0 Then
                    Dim strPath As String
                    ' SQL.DBTable = "T050051"
                    sqrdTemplate = SQL.Search("WSSSave", "SaveTemplate-545", "Select * from T050051 where  VH_IN4_Level=1 and VH_NU9_TemplateID_FK=" & TemplateID & "", SQL.CommandBehaviour.CloseConnection, blnCheck)
                    If blnCheck = True Then
                        Dim arColumnName As New ArrayList
                        Dim arRowData As New ArrayList

                        arColumnName.Add("VH_NU9_File_ID_PK")
                        arColumnName.Add("VH_VC255_File_Name")
                        arColumnName.Add("VH_VC255_File_Size")
                        arColumnName.Add("VH_VC255_File_Path")
                        arColumnName.Add("VH_VC1_Status")
                        arColumnName.Add("VH_IN4_Level")
                        arColumnName.Add("VH_NU9_Address_Book_Number")
                        arColumnName.Add("VH_NU9_Call_Number")
                        arColumnName.Add("VH_NU9_Task_Number")
                        arColumnName.Add("VH_NU9_Action_Number")
                        arColumnName.Add("VH_NU9_CompId_Fk")
                        arColumnName.Add("VH_DT8_Date")
                        arColumnName.Add("VH_VC8_Role")
                        arColumnName.Add("VH_NU9_Version")
                        arColumnName.Add("VH_DT8_Modify_Date")

                        While sqrdTemplate.Read
                            Dim intTaskno As Integer = SQL.Search("WSSSave", "SaveTemplate-567", "select isnull(max(VH_NU9_File_ID_PK),0) from T040051 ")
                            intTaskno += 1
                            arRowData.Clear()
                            arRowData.Add(intTaskno)

                            'Create New Path-----------------------------------------------------------------
                            strPath = "Dockyard/" & companyID & "/" & CallNumber & "/" & sqrdTemplate.Item("VH_NU9_Version") & "/" & sqrdTemplate.Item("VH_VC255_File_Name")
                            strPath = strPath.Replace("\", "/")
                            '-----------------------------------------------------------------------------------------
                            arRowData.Add(sqrdTemplate.Item("VH_VC255_File_Name"))
                            arRowData.Add(sqrdTemplate.Item("VH_VC255_File_Size"))
                            'arRowData.Add(sqrdTemplate.Item("VH_VC255_File_Path"))
                            arRowData.Add(strPath)
                            arRowData.Add(sqrdTemplate.Item("VH_VC1_Status"))
                            arRowData.Add(sqrdTemplate.Item("VH_IN4_Level"))
                            arRowData.Add(sqrdTemplate.Item("VH_NU9_Address_Book_Number"))
                            arRowData.Add(CallNumber)
                            arRowData.Add(0)
                            arRowData.Add(sqrdTemplate.Item("VH_NU9_Action_Number"))
                            arRowData.Add(companyID)
                            arRowData.Add(sqrdTemplate.Item("VH_DT8_Date"))
                            arRowData.Add(sqrdTemplate.Item("VH_VC8_Role"))
                            arRowData.Add(sqrdTemplate.Item("VH_NU9_Version"))
                            arRowData.Add(sqrdTemplate.Item("VH_DT8_Modify_Date"))
                            'SQL.DBTable = "T040051"
                            If SQL.Save("T040051", "WSSSave", "SaveTemplate-596", arColumnName, arRowData) = False Then
                                stReturn.ErrorCode = 1
                                stReturn.ErrorMessage = "Server is busy please try later..."
                            End If

                            ' File Copy ------------------------------------------------------------
                            ' Try

                            If Not (sqrdTemplate.Item("VH_VC255_File_Path") Is DBNull.Value) Then

                                'Create New Path-----------------------------------------------------------------
                                strPath = HttpContext.Current.Session("PropRootDir") & "/" & "Dockyard/" & companyID & "/" & CallNumber & "/" & sqrdTemplate.Item("VH_NU9_Version") & "/" & sqrdTemplate.Item("VH_VC255_File_Name")
                                strPath = strPath.Replace("\", "/")
                                '-----------------------------------------------------------------------------------------
                                dirTemp = New DirectoryInfo(strPath.Substring(0, strPath.LastIndexOf("/")))

                                If dirTemp.Exists = False Then
                                    dirTemp.Create()
                                End If
                                File.Copy(HttpContext.Current.Session("PropRootDir").Replace("\", "/") & "/" & CType(sqrdTemplate.Item("VH_VC255_File_Path"), String), strPath, True)
                            End If
                            'Catch ex As Exception
                            '                            CreateLog("WWSSave", "SaveTemplate-725", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                            '   End Try
                            '----------------------------------------------------------
                        End While
                        sqrdTemplate.Close()
                    End If
                    If mstGetFunctionValue.ErrorCode = 0 Then
                        stReturn.ErrorCode = 0
                        stReturn.ErrorMessage = "Records Saved successfully..."
                    Else
                        stReturn.ErrorCode = 1
                        stReturn.ErrorMessage = "Server is busy please try later..."
                    End If
                End If
            End If
            '-------------------------------------
        Catch ex As Exception
            CreateLog("WWSSave", "SaveTemplate-926", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Function CheckFileName(ByVal FileName As String, ByVal CallNumber As Integer, ByVal TaskNumber As Integer, ByVal Level As AttachLevel) As Double
        Dim dblVersion As Double
        Select Case Level
            Case AttachLevel.CallLevel
                dblVersion = SQL.Search("WSSSave", "CheckFileName-654", "select VH_NU9_Version from T040051 where VH_VC255_File_Name='" & FileName & "' and VH_NU9_Call_Number=" & CallNumber & "")
            Case AttachLevel.TaskLevel
                dblVersion = SQL.Search("WSSSave", "CheckFileName-656", "select VH_NU9_Version from T040051 where VH_VC255_File_Name='" & FileName & "' and VH_NU9_Call_Number=" & CallNumber & " and VH_NU9_Task_Number=" & TaskNumber & "")
        End Select

        Return dblVersion
    End Function

    Private Shared Function CopyFromTemplateComments(ByVal TemplateID As Integer, ByVal NewTaskId As Integer, ByVal TemplateTaskId As Integer, ByVal CallNumber As Integer, ByVal companyID As Integer)
        Dim stReturn As ReturnValue
        Dim strSql As String
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SQL.DBTable = "T050061"
        SQL.DBTracing = False

        Dim blnCheck As Boolean
        Dim sqrdTemplate As SqlClient.SqlDataReader
        Try


            strSql = "Select * from T050061 where CM_NU9_TemplateID_FK=" & TemplateID & " And CM_NU9_Task_Number= " & TemplateTaskId
            'Save Comments- T040061 to T050061
            If CallNumber > 0 Then
                sqrdTemplate = SQL.Search("WSSSave", "CopyFromTemplateComments-676", strSql, SQL.CommandBehaviour.CloseConnection, blnCheck)
                If blnCheck = True Then
                    Dim arColumnName As New ArrayList
                    Dim arRowData As New ArrayList

                    arColumnName.Add("CM_NU9_Comment_Number_PK")
                    arColumnName.Add("CM_NU9_AB_Number")
                    arColumnName.Add("CM_DT8_Date")
                    arColumnName.Add("CM_VC256_Comments")
                    arColumnName.Add("CM_VC2_Flag")
                    arColumnName.Add("CM_NU9_Call_Number")
                    arColumnName.Add("CM_NU9_Task_Number")
                    arColumnName.Add("CM_NU9_Action_Number")
                    arColumnName.Add("CM_VC30_Type")
                    arColumnName.Add("CM_NU9_CompId_Fk")

                    arColumnName.Add("CM_VC1000_MailList")
                    arColumnName.Add("CM_CH1_MailSent")
                    arColumnName.Add("CM_VC50_IE")
                    arColumnName.Add("CM_CH1_Flag")
                    arColumnName.Add("CM_NU9_Comment_To")

                    While sqrdTemplate.Read
                        'Condition to get max Comment number is changed after discussion with Amit on 26/07/2006
                        Dim intTaskno As Integer = SQL.Search("WSSSave", "CopyFromTmpl-695", "select isnull(max(CM_NU9_Comment_Number_PK),0) from T040061") 'where CM_NU9_Call_Number=" & HttpContext.Current.Session("PropCallNumber") & " and CM_NU9_Task_Number=" & NewTaskId)
                        intTaskno += 1
                        arRowData.Clear()
                        arRowData.Add(intTaskno)
                        arRowData.Add(sqrdTemplate.Item("CM_NU9_AB_Number"))
                        arRowData.Add(sqrdTemplate.Item("CM_DT8_Date"))
                        arRowData.Add(sqrdTemplate.Item("CM_VC256_Comments"))
                        arRowData.Add(sqrdTemplate.Item("CM_VC2_Flag"))
                        arRowData.Add(CallNumber)
                        arRowData.Add(NewTaskId)
                        arRowData.Add(sqrdTemplate.Item("CM_NU9_Action_Number"))
                        arRowData.Add(sqrdTemplate.Item("CM_VC30_Type"))
                        arRowData.Add(companyID)

                        arRowData.Add(sqrdTemplate.Item("CM_VC1000_MailList"))
                        arRowData.Add(sqrdTemplate.Item("CM_CH1_MailSent"))
                        arRowData.Add(sqrdTemplate.Item("CM_VC50_IE"))
                        arRowData.Add(sqrdTemplate.Item("CM_CH1_Flag"))
                        arRowData.Add(sqrdTemplate.Item("CM_NU9_Comment_To"))

                        '    SQL.DBTable = "T040061"
                        If SQL.Save("T040061", "WSSSave", "CopyFromtmpl-710", arColumnName, arRowData) = False Then
                            stReturn.ErrorCode = 1
                            stReturn.ErrorMessage = "Error occured while saving records"
                        End If
                    End While
                    sqrdTemplate.Close()
                End If
                If mstGetFunctionValue.ErrorCode = 0 Then
                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Records Saved successfully..."
                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "please try later..."
                End If
            End If
        Catch ex As Exception
            CreateLog("WssSave", "CopyFromTemplateComments-833", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        '-------------------------------------
    End Function


    Public Shared Function SaveTemplateCall(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal Company As String, ByVal TemplateNumber As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim intRows As Integer

        ' SQL.DBTable = "T050021"
        SQL.DBTracing = False
        intRows = SQL.Search("WSSSave", "SaveTemplCall-738", "select TCM_NU9_Call_No_PK from T050021 where TCM_NU9_TemplateID_FK=" & TemplateNumber & "")
        If intRows > 0 Then
            'HttpContext.Current.Session("PropCallNumber") = intRows
            If SQL.Update("T050021", "WSSSave", "SaveTemplCall-741", "select * from T050021 where TCM_NU9_TemplateID_FK=" & TemplateNumber & "", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Error occured while Updating records"
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Updated successfully..."
                stReturn.ErrorCode = 0
            End If
            Return stReturn
        End If

        Dim intCallNo As Integer = SQL.Search("WSSSave", "SaveTemplCall-751", "select isnull(max(TCM_NU9_Call_No_PK),0) from t050021")

        intCallNo += 1

        Try
            ColumnName.Add("TCM_NU9_Call_No_PK")
            RowData.Add(intCallNo)

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' Table name
            'SQL.DBTable = "T050021"
            SQL.DBTracing = False
            'Add the status for the call
            'It is not needed while updating call in the above code
            ColumnName.Add("TCM_VC20_Call_Status")
            RowData.Add("OPEN")
            If SQL.Save("T050021", "WSSSave", "SaveTemplCall-766", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                stReturn.ExtraValue = intCallNo
                'HttpContext.Current.Session("PropCallNumber") = intCallNo
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveTemplateCall-890", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

#Region "Agreement"
    Public Shared Function SaveAgreement(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal tblName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            ' SQL.DBTable = tblName
            SQL.DBTracing = False

            If SQL.Save(tblName, "WSSSave", "SaveAgreement-806", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Error occured while saving records"
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2

            CreateLog("WWSSave", "SaveAgreement-929", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#End Region

#Region " TemplateForm "
    Private Shared Function CopyFromTemplateForm(ByVal TemplateID As Integer, ByVal NewTaskId As Integer, ByVal TemplateTaskId As Integer, ByVal CallNumber As Integer, ByVal companyID As Integer)
        Dim stReturn As ReturnValue
        Dim strSql As String
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SQL.DBTable = "T050071"
        SQL.DBTracing = False

        Dim blnCheck As Boolean
        Dim sqrdTemplate As SqlClient.SqlDataReader
        Dim sqrdT050072 As SqlClient.SqlDataReader
        Try
            strSql = "Select * from T050071 where FD_IN4_Temp_Id=" & TemplateID & " AND FD_IN4_Task_no=" & TemplateTaskId
            'Save Comments- T040061 to T050061
            If CallNumber > 0 Then
                sqrdTemplate = SQL.Search("WSSSave", "CopyFromTemplateForm-1127", strSql, SQL.CommandBehaviour.CloseConnection, blnCheck)
                If blnCheck = True Then
                    Dim arColumnName As New ArrayList
                    Dim arRowData As New ArrayList

                    While sqrdTemplate.Read
                        arColumnName.Clear()
                        arColumnName.Add("FD_IN4_Form_no")
                        arColumnName.Add("FD_IN4_Call_no")
                        arColumnName.Add("FD_IN4_Task_no")
                        arColumnName.Add("FD_VC50_Call_form_Name")
                        arColumnName.Add("FD_IN4_Comp_id")
                        arColumnName.Add("FD_VC50_RPA")
                        arColumnName.Add("FD_IN4_User1")
                        arColumnName.Add("FD_IN4_Inserted_By")
                        arColumnName.Add("FD_IN4_Inserted_On")

                        '  sqrdTemplate.Read()
                        Dim intFormno As Integer = clsNextNo.GetNextNo(101, "COM", System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString) 'SQL.Search("WSSSave", "CopyFromTemplateForm-1145", "select isnull(max(FD_IN4_Form_no),0) +1 from T100011")
                        arRowData.Clear()
                        arRowData.Add(intFormno)
                        arRowData.Add(CallNumber)
                        arRowData.Add(NewTaskId)
                        arRowData.Add(sqrdTemplate.Item("FD_VC50_Call_form_Name"))
                        arRowData.Add(companyID)
                        arRowData.Add(sqrdTemplate.Item("FD_VC50_RPA"))
                        arRowData.Add(sqrdTemplate.Item("FD_IN4_User1"))
                        arRowData.Add(HttpContext.Current.Session("PropUserID"))
                        arRowData.Add(Now.Today)

                        '  SQL.DBTable = "T100011"
                        If SQL.Save("T100011", "WSSSave", "CopyFromtmplateForm-1161", arColumnName, arRowData) = False Then
                            stReturn.ErrorCode = 1
                            stReturn.ErrorMessage = "Server is busy please try later..."
                        Else
                            'If records from T050071 is successfully saved in T100011 then
                            'Copy Records from T050072 to T100022
                            strSql = "Select * From T050072 Where Tb_IN4_Form_No=" & sqrdTemplate.Item("FD_IN4_Form_no")
                            sqrdT050072 = SQL.Search("WSSSave", "CopyFromTemplateForm-1163", strSql, SQL.CommandBehaviour.CloseConnection, blnCheck)
                            If blnCheck = True Then
                                arColumnName.Clear()
                                arColumnName.Add("Tb_IN4_Tab_No")
                                arColumnName.Add("Tb_IN4_Form_No")
                                arColumnName.Add("Tb_VC200_Field1")
                                arColumnName.Add("Tb_VC200_Field2")
                                arColumnName.Add("Tb_VC200_Field3")
                                arColumnName.Add("Tb_VC200_Field4")
                                arColumnName.Add("Tb_VC200_Field5")
                                arColumnName.Add("Tb_VC200_Field6")
                                arColumnName.Add("Tb_VC200_Field7")
                                arColumnName.Add("Tb_VC200_Field8")
                                arColumnName.Add("Tb_VC200_Field9")
                                arColumnName.Add("Tb_VC200_Field10")
                                arColumnName.Add("Tb_VC200_Field11")
                                arColumnName.Add("Tb_VC200_Field12")
                                arColumnName.Add("Tb_VC2000_Field13")
                                arColumnName.Add("Tb_VC2000_Field14")
                                arColumnName.Add("Tb_VC2000_Field15")
                                arColumnName.Add("Tb_DT8_Date1")
                                arColumnName.Add("Tb_DT8_Date2")
                                arColumnName.Add("Tb_DT8_Date3")

                                While sqrdT050072.Read()
                                    arRowData.Clear()
                                    arRowData.Add(sqrdT050072.Item("Tb_IN4_Tab_No"))
                                    arRowData.Add(intFormno)
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field1"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field2"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field3"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field4"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field5"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field6"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field7"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field8"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field9"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field10"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field11"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC200_Field12"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC2000_Field13"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC2000_Field14"))
                                    arRowData.Add(sqrdT050072.Item("Tb_VC2000_Field15"))
                                    arRowData.Add(sqrdT050072.Item("Tb_DT8_Date1"))
                                    arRowData.Add(sqrdT050072.Item("Tb_DT8_Date2"))
                                    arRowData.Add(sqrdT050072.Item("Tb_DT8_Date3"))
                                    'SQL.DBTable = "T100022"
                                    SQL.Save("T100022", "WSSSave", "CopyFromtmplateForm-1161", arColumnName, arRowData)
                                End While
                                sqrdT050072.Close()
                            End If

                            '----------------------------------------------------
                        End If
                    End While

                    sqrdTemplate.Close()

                End If
                If mstGetFunctionValue.ErrorCode = 0 Then
                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Records Saved successfully..."
                Else
                    stReturn.ErrorCode = 1
                    stReturn.ErrorMessage = "Server is busy please try later..."
                End If

            End If
        Catch ex As Exception
            CreateLog("WssSave", "CopyFromTemplateForm-1176", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        '-------------------------------------
    End Function
#End Region

#Region " SubCategory Info, Member, Overview "
    Public Shared Function SaveProject(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal tblName As String) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            '  SQL.DBTable = tblName
            SQL.DBTracing = False

            If SQL.Save(tblName, "WSSSave", "SaveAgreement-806", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Error occured while saving records"
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2

            CreateLog("WWSSave", "SaveAgreement-929", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

#Region " Folder/Document Management "
    Public Shared Function SaveFolder(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal FolderName As String, ByVal ParentFolderID As String, ByVal CompanyID As Int32) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            '    SQL.DBTable = "UDC"
            SQL.DBTracing = False

            If SQL.Save("T250021", "WSSSave", "SaveFolder-1349", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                Dim intFolderID As Integer
                intFolderID = SQL.Search("WSSSave", "SaveFolder-1355", "select max(FD_NU9_Folder_ID_PK) from T250021 Where FD_NU9_Parent_Folder_ID_FK=" & ParentFolderID & " And FD_VC255_Folder_Name='" & FolderName & "' and FD_NU9_Company_ID_FK=" & CompanyID)

                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                stReturn.ExtraValue = intFolderID
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveFolder-1364", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

#Region " Save Documents "
    Public Shared Function SaveFile(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal FileName As String, ByVal FolderID As Int32) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            SQL.DBConnection = strConnection
            SQL.DBTracing = False

            If SQL.Save("T250011", "WSSSave", "SaveFolder-1349", ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                Dim intFileID As Integer
                intFileID = SQL.Search("WSSSave", "SaveFile-1387", "select max(FI_NU9_File_ID_PK) from T250011 Where FI_NU9_Folder_ID_FK=" & FolderID & " And FI_VC255_File_Name='" & FileName & "'")
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                stReturn.ExtraValue = intFileID
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveFile-1397", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
#End Region

End Class