#Region "Class MailBody"

'************************************************************************
' Page                   : - class clsMailBody
' Purpose              : - It will return the mail message
' Date		    			Author						Modification Date					Description
' 17/05/06				Harpreet 					10/07/2006        					Created
'
' Notes: This class hold the function GetMail(). This function accepts some parameters and 
'             returns the complete mail message.
' Code:
'************************************************************************

Imports ION.Data
Imports System.Web.Mail
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.VisualBasic

'class for generating mail messages

Public Class clsMailBody

#Region "Private Variables"

    'stores the Event ID for mail in mailtype enum
    Private menuEventID As MailEvent
    'stores the Event ID for mail
    Private mintEventID As Integer
    'stores the numeric log table ID
    Private mintLogNo As Integer
    'stores the numeric company ID 
    Private mintCompID As Integer
    'stores the call number
    Private mintCallNo As Integer
    'stores the task number
    Private mintTaskNo As Integer
    'stores the action number
    Private mintActionNo As Integer
    'stores the body for mail in string format
    Private mstrMailBody As String
    'stores the mail subject 
    Private mstrMailSubject As String
    'stores the mail message for returning
    Private mmMailMsg As New MailMessage
    Private strLink As String
    'Stores the To email ID for mail messsage
    Private mstrToMail As String
    'Stores the comment number
    Private mintCommentNo As Integer

#End Region

#Region "Enumerations"

    'enumeration for specifying the mail event
    Public Enum MailEvent
        CallOpen = 1
        TaskAssigned = 2
        ActionFilled = 3
        TaskForward = 4
        CallStatusChanged = 13
        CallClosed = 14
        TaskClosed = 15
        TaskStatusChanged = 16
        RecordUpdated = 19
    End Enum

    'enumeration for specifying the type of mail while calling the GetMail function
    Public Enum MailType
        CallMail = 1
        TaskMail = 2
        ActionMail = 3
        CallComment = 4
        TaskComment = 5
        ActionComment = 6
    End Enum

#End Region

#Region "GetMail"

    '*******************************************************************
    ' Page                   : -Function GetMail
    ' Purpose              : -function for getting mail message 
    ' Date		    			Author						Modification Date					Description
    ' 17/05/06				Harpreet 					06/06/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete mail message.
    ' Code:
    '*******************************************************************

    'function for getting mail message 
    Public Function GetMail(ByVal intEventID As Integer, ByVal intLogNo As Integer, ByVal enuMailType As MailType, ByVal intCompID As Integer, ByVal intCallNo As Integer, Optional ByVal intTaskNo As Integer = 0, Optional ByVal intActionNo As Integer = 0, Optional ByVal intCommentNo As Integer = 0) As MailMessage

        'Assign the Parameter variables to the class variables
        mintLogNo = intLogNo
        menuEventID = intEventID
        mintCompID = intCompID
        mintCallNo = intCallNo
        mintTaskNo = intTaskNo
        mintActionNo = intActionNo
        mintCommentNo = intCommentNo
        ''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim sqRDR As SqlDataReader

        'flag for checking the status of the reader
        Dim blnStatus As Boolean = False

        Try
            'get the connection string
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'get the WSS link from config file
            ' strLink = ConfigurationSettings.AppSettings("WSSLink")
            'get the WSS link from database
            strLink = SQL.Search("clsMailBody", "GetMail", "select SS_VC500_Value from T000011 where SS_VC36_Type='WSSLink'")

            'if the mail is for call
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If enuMailType = MailType.CallMail Then
                'get the mail info in a Reader
                sqRDR = SQL.Search("GetMailBody", "clsMailBody", "select CN_VC20_Call_Status,CM_NU9_Call_Owner,CM_VC8_Call_Type,CM_VC100_Subject,CM_VC200_Work_Priority,CM_DT8_Request_Date,a.CI_VC28_Email_1 as ToMail, a.CI_VC36_Name as Owner, b.CI_VC36_Name as ByWhom FROM T990011,T010011 a,T010011 b where CM_NU9_Call_No_PK=" & intCallNo & " and T990011.CM_NU9_Call_Owner=a.CI_NU8_Address_Number and T990011.CM_VC100_By_Whom = b.CI_NU8_Address_Number  and CM_NU9_Comp_Id_FK=" & mintCompID & " and CM_NU9_CallLog_No=" & mintLogNo, SQL.CommandBehaviour.Default, blnStatus, "")
                'if record found
                If blnStatus = True Then
                    While sqRDR.Read
                        mstrMailBody = GetCallBody(IIf(IsDBNull(sqRDR("CM_VC8_Call_Type")), "", sqRDR("CM_VC8_Call_Type")), IIf(IsDBNull(sqRDR("CM_VC100_Subject")), "", sqRDR("CM_VC100_Subject")), IIf(IsDBNull(sqRDR("CM_VC200_Work_Priority")), "", sqRDR("CM_VC200_Work_Priority")), IIf(IsDBNull(sqRDR("CM_DT8_Request_Date")), "", sqRDR("CM_DT8_Request_Date")), IIf(IsDBNull(sqRDR("Owner")), "", sqRDR("Owner")), IIf(IsDBNull(sqRDR("ByWhom")), "", sqRDR("ByWhom")), IIf(IsDBNull(sqRDR("CN_VC20_Call_Status")), "", sqRDR("CN_VC20_Call_Status")))
                        'Get the Call Owner To mailid
                        mstrToMail = sqRDR("ToMail")
                    End While
                End If
                'close the raeder
                sqRDR.Close()

                'if the mail is for task
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ElseIf enuMailType = MailType.TaskMail Then
                'get the mail info
                sqRDR = SQL.Search("GetMailBody", "clsMailBody", "select C.CM_VC100_Subject, T.TM_VC1000_Subtsk_Desc, T.TM_DT8_Task_Date, T.TM_DT8_Est_close_date, N1.CI_VC28_Email_1 as ToMail, N.CI_VC36_Name as AssignBy, N1.CI_VC36_Name as SuppOwner FROM T990021 T,T040011 C, T010011 N, T010011 N1  where N.CI_NU8_Address_Number=T.TM_NU9_Assign_by and N1.CI_NU8_Address_Number=T.TM_VC8_Supp_Owner and T.TM_NU9_Call_No_FK=C.CM_NU9_Call_No_PK and TM_NU9_Call_No_FK=" & mintCallNo & " and TM_NU9_Task_no_PK=" & mintTaskNo & " and TM_NU9_Comp_ID_FK = " & mintCompID & " and C.CM_NU9_Comp_ID_FK = " & mintCompID & " and T.TM_NU9_TaskLog_No = " & mintLogNo, SQL.CommandBehaviour.Default, blnStatus, "")

                'if record found
                If blnStatus = True Then
                    While sqRDR.Read
                        mstrMailBody = GetTaskBody(IIf(IsDBNull(sqRDR("CM_VC100_Subject")), "", sqRDR("CM_VC100_Subject")), IIf(IsDBNull(sqRDR("TM_VC1000_Subtsk_Desc")), "", sqRDR("TM_VC1000_Subtsk_Desc")), IIf(IsDBNull(sqRDR("AssignBy")), "", sqRDR("AssignBy")), IIf(IsDBNull(sqRDR("TM_DT8_Task_Date")), "", sqRDR("TM_DT8_Task_Date")), IIf(IsDBNull(sqRDR("TM_DT8_Est_close_date")), "", sqRDR("TM_DT8_Est_close_date")), IIf(IsDBNull(sqRDR("SuppOwner")), "", sqRDR("SuppOwner")))

                        'Get the Task Owner To mailid
                        mstrToMail = sqRDR("ToMail")
                    End While

                End If
                sqRDR.Close()

                'if the mail is for action
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ElseIf enuMailType = MailType.ActionMail Then
                'get the mail info
                sqRDR = SQL.Search("GetMailBody", "clsMailBody", "select CM_VC100_Subject, CN_VC20_Call_Status, TM_VC1000_Subtsk_Desc, AM_DT8_Action_Date, AM_FL8_Used_Hr, TM_VC50_Deve_status ,AM_VC_2000_Description, TM_NU9_Assign_by, N1.CI_VC28_Email_1 as ToMail, N.CI_VC36_Name as AssignBy,AM_VC8_Supp_Owner,N1.CI_VC36_Name as ActionOwner from T990031 A, T040011 C, T040021 T, T010011 N, T010011 N1 where N.CI_NU8_Address_Number=T.TM_NU9_Assign_by and N1.CI_NU8_Address_Number=A.AM_VC8_Supp_Owner and T.TM_NU9_Assign_by=N.CI_NU8_Address_Number and T.TM_NU9_Task_no_PK=A.AM_NU9_Task_Number and C.CM_NU9_Call_No_PK=T.TM_NU9_Call_No_FK and C.CM_NU9_Call_No_PK=A.AM_NU9_Call_Number and A.AM_NU9_Call_Number=" & mintCallNo & " and A.AM_NU9_Task_Number=" & mintTaskNo & " and A.AM_NU9_Action_Number=" & mintActionNo & " and A.AM_NU9_Comp_ID_FK=" & mintCompID & " and T.TM_NU9_Comp_ID_FK=" & mintCompID & " and C.CM_NU9_Comp_ID_FK=" & mintCompID & " and A.AM_NU9_ActionLog_No=" & mintLogNo, SQL.CommandBehaviour.Default, blnStatus, "")

                If blnStatus = True Then
                    While sqRDR.Read
                        mstrMailBody = GetActionBody(IIf(IsDBNull(sqRDR("CM_VC100_Subject")), "", sqRDR("CM_VC100_Subject")), IIf(IsDBNull(sqRDR("CN_VC20_Call_Status")), "", sqRDR("CN_VC20_Call_Status")), IIf(IsDBNull(sqRDR("TM_VC1000_Subtsk_Desc")), "", sqRDR("TM_VC1000_Subtsk_Desc")), IIf(IsDBNull(sqRDR("TM_VC50_Deve_status")), "", sqRDR("TM_VC50_Deve_status")), IIf(IsDBNull(sqRDR("AM_DT8_Action_Date")), "", sqRDR("AM_DT8_Action_Date")), IIf(IsDBNull(sqRDR("AM_VC_2000_Description")), "", sqRDR("AM_VC_2000_Description")), IIf(IsDBNull(sqRDR("AssignBy")), "", sqRDR("AssignBy")), IIf(IsDBNull(sqRDR("ActionOwner")), "", sqRDR("ActionOwner")), IIf(IsDBNull(sqRDR("AM_FL8_Used_Hr")), "", sqRDR("AM_FL8_Used_Hr")))

                        'Get the Action Owner To mailid
                        mstrToMail = sqRDR("ToMail")
                    End While

                End If
                'close the raeder
                sqRDR.Close()

                'if the mail is for call comment
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ElseIf enuMailType = MailType.CallComment Then
                'get the mail info
                sqRDR = SQL.Search("GetMailBody", "clsMailBody", "select N.CI_VC36_Name WrittenBy, CM_DT8_Date CommentDate, CM_VC256_Comments Comment from T040061 C,T010011 N where N.CI_NU8_Address_Number=C.CM_NU9_AB_Number and CM_VC2_Flag='C' and CM_NU9_Comment_Number_PK=" & mintCommentNo & " and CM_NU9_CompID_FK=" & mintCompID, SQL.CommandBehaviour.SingleResult, blnStatus, "")

                If blnStatus = True Then
                    While sqRDR.Read
                        mstrMailBody = GetCallCommentBody(sqRDR("Comment"), sqRDR("WrittenBy"), sqRDR("CommentDate"))
                        'Get the  To mailids
                        'mstrToMail = sqRDR("MailList")
                    End While

                End If
                'close the reader
                sqRDR.Close()

                'if the mail is for task comment
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ElseIf enuMailType = MailType.TaskComment Then

                'get the mail info
                sqRDR = SQL.Search("GetMailBody", "clsMailBody", "select N.CI_VC36_Name WrittenBy, CM_DT8_Date CommentDate, CM_VC256_Comments Comment from T040061 C,T010011 N where N.CI_NU8_Address_Number=C.CM_NU9_AB_Number and CM_VC2_Flag='T' and CM_NU9_Comment_Number_PK=" & mintCommentNo & " and CM_NU9_CompID_FK=" & mintCompID, SQL.CommandBehaviour.SingleResult, blnStatus, "")

                If blnStatus = True Then
                    While sqRDR.Read
                        mstrMailBody = GetTaskCommentBody(sqRDR("Comment"), sqRDR("WrittenBy"), sqRDR("CommentDate"))
                        'Get the  To mailids
                        '                        mstrToMail = sqRDR("MailList")
                    End While

                End If
                'close the reader
                sqRDR.Close()

                'if the mail is for action comment
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ElseIf enuMailType = MailType.ActionComment Then
                'get the mail info
                sqRDR = SQL.Search("GetMailBody", "clsMailBody", "select N.CI_VC36_Name WrittenBy, CM_DT8_Date CommentDate, CM_VC256_Comments Comment from T040061 C,T010011 N where N.CI_NU8_Address_Number=C.CM_NU9_AB_Number and CM_VC2_Flag='A' and CM_NU9_Comment_Number_PK=" & mintCommentNo & " and CM_NU9_CompID_FK=" & mintCompID, SQL.CommandBehaviour.SingleResult, blnStatus, "")

                If blnStatus = True Then
                    While sqRDR.Read
                        mstrMailBody = GetActionCommentBody(sqRDR("Comment"), sqRDR("WrittenBy"), sqRDR("CommentDate"))
                        'Get the  To mailids
                        '                        mstrToMail = sqRDR("MailList")
                    End While

                End If
                'close the reader
                sqRDR.Close()

            End If



            'fill the mail message 
            mmMailMsg.To = mstrToMail
            mmMailMsg.Body = mstrMailBody
            mmMailMsg.Subject = mstrMailSubject
            mmMailMsg.BodyFormat = MailFormat.Html

            'return the mail message
            Return mmMailMsg
        Catch ex As Exception
            CreateLog("clsmailbody", "GetMail-281", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "CallMail"
    '*******************************************************************
    ' Page                   : -Function GetCallBody
    ' Purpose              : -function for getting Call Body for mail message 
    ' Date		    			Author						Modification Date					Description
    ' 17/05/06				Harpreet 					06/06/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete HTML 
    '              body for mail message for Call Email.
    ' Code:
    '*******************************************************************

    Private Function GetCallBody(ByVal strCallType As String, ByVal strCallSubject As String, ByVal strPriority As String, ByVal strCallDate As String, ByVal strCallOwner As String, ByVal strByWhom As String, ByVal strCallStatus As String) As String




        Dim HTML As String
        HTML = HTML & "<font size=2 face=""Verdana"">&nbsp;"

        HTML = HTML & "<font size=1 face=""Verdana"">&nbsp;"

        HTML = HTML & "<BR>&nbsp;You received this E-mail based on your interest in products and services offered by IONSoftNet Pvt Ltd.</font>"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<BR><BR>&nbsp;Dear " & strCallOwner

        HTML = HTML & "<font size=2 face=""Verdana"">"

        Select Case menuEventID
            Case MailEvent.CallOpen
                HTML = HTML & "<BR><BR>&nbsp;We are glad to receive your request. The current status of your call is  " & strCallStatus & ". You can view the latest status online at</font>"
                'Make the subject for call mail
                mstrMailSubject = "New Call confirmation, Call# " & mintCallNo
            Case MailEvent.CallClosed
                HTML = HTML & "<BR><BR>&nbsp;Your call has been Closed. The current status of your call is " & strCallStatus & ". You can view the latest status online at</font>"
                'Make the subject for call mail
                mstrMailSubject = "Call Closed confirmation, Call# " & mintCallNo
            Case MailEvent.CallStatusChanged
                HTML = HTML & "<BR><BR>&nbsp;The status of Your call has been updated. The current status of your call is " & strCallStatus & ". You can view the latest status online at</font>"
                'Make the subject for call mail
                mstrMailSubject = "Call Status Changed, Call# " & mintCallNo
            Case MailEvent.RecordUpdated
                HTML = HTML & "<BR><BR>&nbsp;Your call has been updated. The current status of your call is " & strCallStatus & ". You can view the latest status online at</font>"
                'Make the subject for call mail
                mstrMailSubject = "Call# " & mintCallNo & " updated"
        End Select

        HTML = HTML & "<font color=#000099 size=2 face=""Verdana"">"

        HTML = HTML & "&nbsp;<b><a href=""" & strLink & """>Web Support System</a></b></font><BR><BR>"

        HTML = HTML & "<br>Call Type &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;" & strCallType

        HTML = HTML & "<br>Call Subject &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;" & strCallSubject

        HTML = HTML & "<br>Work Priority &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- &nbsp;&nbsp;" & strPriority

        HTML = HTML & "<br>Date/Time &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;" & strCallDate

        HTML = HTML & "<br>Owner of the call &nbsp;- &nbsp;&nbsp;" & strCallOwner

        HTML = HTML & "<br>submitted by &nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp; - &nbsp;&nbsp;" & strByWhom

        HTML = HTML & "<br><br>&nbsp;You will be updated once it has been sent to the authorized support person. Support person will get in touch with you ASAP regarding the status of your call.<BR>"

        HTML = HTML & "<BR><BR>&nbsp;If you have any queries, you can e-mail us at "

        HTML = HTML & "<font color=#000099 size=2 face=""Verdana"">"

        HTML = HTML & "wss@ionsoftnet.com</font>"

        HTML = HTML & "<BR><BR>&nbsp;Thank you for using ION services."

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<BR><BR><BR>&nbsp;Regards,"

        HTML = HTML & "<font size=2 face=""Verdana"">"


        HTML = HTML & "<BR><BR><BR>&nbsp;Web Support System"

        Return HTML

    End Function

#End Region

#Region "TaskMail"

    '*******************************************************************
    ' Page                   : -Function GetTaskBody
    ' Purpose              : -function for getting Task Body for mail message 
    ' Date		    			Author						Modification Date					Description
    ' 17/05/06				Harpreet 					06/06/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete HTML 
    '              body for mail message for Task Email.
    ' Code:
    '*******************************************************************

    Private Function GetTaskBody(ByVal strCallSubject As String, ByVal strTaskDesc As String, ByVal strAssignedBy As String, ByVal strTaskDate As String, ByVal strEstCloseDate As String, ByVal strSuppOwner As String) As String

        Dim HTML

        HTML = "<!DOCTYPE HTML PUBLIC""-//IETF//DTD HTML//EN"">"

        HTML = HTML & "<html>"

        HTML = HTML & "<head>"

        HTML = HTML & "<meta http-equiv=""Content-Type"""

        HTML = HTML & "content=""text/html; charset=iso-8859-1"">"

        HTML = HTML & "<meta name=""GENERATOR"""

        HTML = HTML & " content=""Microsoft Visual Studio 6.0"">"

        HTML = HTML & "<title>HTMLMail</title>"

        HTML = HTML & "</head>"

        HTML = HTML & "<body>"



        HTML = HTML & "Dear " & strSuppOwner & "<br><br>"

        Select Case menuEventID
            Case MailEvent.TaskAssigned
                HTML = HTML & "You have been assigned a New Task, Please follow up in your 'Do List' in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for task mail
                mstrMailSubject = "WSS Alert --- Task#  " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Assigned to " & strSuppOwner
            Case MailEvent.TaskClosed
                HTML = HTML & "Your task has been closed, Please follow up in your 'Do List' in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for task mail
                mstrMailSubject = "WSS Alert --- Task#  " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Assigned to " & strSuppOwner & " Closed"
            Case MailEvent.TaskForward
                HTML = HTML & "Task is Forwarded to you, Please follow up in your 'Do List' in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for task mail
                mstrMailSubject = "WSS Alert --- Task#  " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Forwarded to " & strSuppOwner
            Case MailEvent.TaskStatusChanged
                HTML = HTML & "Status of Your Task has been Updated, Please follow up in your 'Do List' in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for task mail
                mstrMailSubject = "WSS Alert --- Task#  " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Assigned to " & strSuppOwner & "--Status Changed"
            Case MailEvent.RecordUpdated
                HTML = HTML & "Your Task has been Updated, Please follow up in your 'Do List' in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for task mail
                mstrMailSubject = "WSS Alert --- Task#  " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Assigned to " & strSuppOwner & " updated"
        End Select

        HTML = HTML & "<BR><table bgcolor=""#f5f5f5"" style=""border-collapse: collapse"" border=""1"" width=""85%"" height=""95"" bordercolor=""#e0e0e0"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""28"" align=""left"" bgcolor=""#e0e0e0"">"

        HTML = HTML & "<font color=#000000 size=2 face=""Verdana"">"

        HTML = HTML & "<b><u>New Task Assigned To:</u> " & strSuppOwner & "</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call No. :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintCallNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call Subject :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCallSubject

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"



        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Task No.:</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintTaskNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"" valign=""top"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Task Description :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strTaskDesc

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Assigned by :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strAssignedBy

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Date/Time :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & Now()

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Estimated Close Date:</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strEstCloseDate

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr><br><br>"

        HTML = HTML & "</table>"



        HTML = HTML & "<BR><table style=""border-collapse: collapse"" border=""0"" width=""85%"" height=""5"" bordercolor=""#94BED7"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"" >"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "Thanks</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "WSS Admin</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "</table>"

        HTML = HTML & "</body>"

        HTML = HTML & "</html>"

        Return HTML

    End Function
#End Region

#Region "ActionMail"
    '*******************************************************************
    ' Page                   : -Function GetActionBody
    ' Purpose              : -function for getting Action Body for mail message 
    ' Date		    			Author						Modification Date					Description
    ' 17/05/06				Harpreet 					06/06/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete HTML 
    '              body for mail message for Action Email.
    ' Code:
    '*******************************************************************


    Private Function GetActionBody(ByVal strCallSubject As String, ByVal strCallStatus As String, ByVal strTaskDesc As String, ByVal strTaskStatus As String, ByVal strActionDate As String, ByVal strActionDesc As String, ByVal strAssignBy As String, ByVal strActionOwner As String, ByVal strUsedHours As String) As String


        Dim HTML


        HTML = "<!DOCTYPE HTML PUBLIC""-//IETF//DTD HTML//EN"">"

        HTML = HTML & "<html>"

        HTML = HTML & "<head>"

        HTML = HTML & "<meta http-equiv=""Content-Type"""

        HTML = HTML & "<meta name=""GENERATOR"""

        HTML = HTML & " content=""Microsoft Visual Studio 6.0"">"

        HTML = HTML & "<title>HTMLMail</title>"

        HTML = HTML & "</head>"

        HTML = HTML & "<body>"

        'Action Owner
        HTML = HTML & "Dear " & strActionOwner & "<br><br>"
        Select Case menuEventID
            Case MailEvent.ActionFilled
                HTML = HTML & "New Action Filled for the Assigned Task in WSS on<b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for action mail
                mstrMailSubject = "WSS Alert --- Action#  " & mintActionNo & " for Task# " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Action fill by " & strActionOwner
            Case MailEvent.RecordUpdated
                HTML = HTML & "Action updated for the Assigned Task in WSS on<b><a href=""" & strLink & """>Web Support System</a></b></font>"
                'Make the subject for action mail
                mstrMailSubject = "WSS Alert --- Action#  " & mintActionNo & " for Task# " & mintTaskNo & "  on Call#  " & mintCallNo & " --- Action updated by " & strActionOwner
        End Select

        HTML = HTML & "<BR><table bgcolor=""#f5f5f5"" style=""border-collapse: collapse"" border=""1"" width=""85%"" height=""95"" bordercolor=""#e0e0e0"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""28"" align=""left"" bgcolor=""#e0e0e0"">"

        HTML = HTML & "<font color=#000000 size=2 face=""Verdana"">"

        'Action Owner
        HTML = HTML & "<b><u>New Action Fill by:</u> " & strActionOwner & "</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call No. :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        'ASsign the call Number
        HTML = HTML & mintCallNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"


        HTML = HTML & "<b>Call Subject :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"
        'call description

        HTML = HTML & strCallSubject

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call Status :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        'call Status
        HTML = HTML & strCallStatus

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Task No.:</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        'Task Number
        HTML = HTML & mintTaskNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Assigned by :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"
        'task assigned by
        HTML = HTML & strAssignBy

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Date/Time :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"
        'Date Time
        HTML = HTML & strActionDate

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"



        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Used Hours :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"
        'Fill Used Hours
        HTML = HTML & strUsedHours

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"



        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Description :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"
        'Action description
        HTML = HTML & strActionDesc

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr><br><br>"

        HTML = HTML & "</table>"



        HTML = HTML & "<BR><table style=""border-collapse: collapse"" border=""0"" width=""85%"" height=""5"" bordercolor=""#94BED7"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"" >"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "Thanks</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "WSS Admin</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "</table>"



        HTML = HTML & "</body>"

        HTML = HTML & "</html>"
        Return HTML
    End Function

#End Region

#Region "CallCommentMail"
    '*******************************************************************
    ' Page                   : -Function GetCallCommentBody
    ' Purpose              : -function for getting Call Comment Body for mail message 
    ' Date		    			Author						Modification Date					Description
    ' 10/07/06				Harpreet 					10/07/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete HTML 
    '              body for mail message for Call Comment Email.
    ' Code:
    '*******************************************************************
    Private Function GetCallCommentBody(ByVal strComment As String, ByVal strCommentWrittenBy As String, ByVal strCommentDate As String) As String
        'Make the subject for task mail
        mstrMailSubject = "WSS Alert --- " & strCommentWrittenBy & " Has Written Comment on Call#" & mintCallNo

        Dim HTML

        HTML = "<!DOCTYPE HTML PUBLIC""-//IETF//DTD HTML//EN"">"

        HTML = HTML & "<html>"

        HTML = HTML & "<head>"

        HTML = HTML & "<meta http-equiv=""Content-Type"""

        HTML = HTML & "content=""text/html; charset=iso-8859-1"">"

        HTML = HTML & "<meta name=""GENERATOR"""

        HTML = HTML & " content=""Microsoft Visual Studio 6.0"">"

        HTML = HTML & "<title>HTMLMail</title>"

        HTML = HTML & "</head>"

        HTML = HTML & "<body>"



        HTML = HTML & "Dear User<br><br>"

        HTML = HTML & "<b>Comment</b> for you, Please follow up in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"

        HTML = HTML & "<BR><table bgcolor=""#f5f5f5"" style=""border-collapse: collapse"" border=""1"" width=""85%"" height=""95"" bordercolor=""#e0e0e0"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""28"" align=""left"" bgcolor=""#e0e0e0"">"

        HTML = HTML & "<font color=#000000 size=2 face=""Verdana"">"

        HTML = HTML & "<b><u>New Comment</u></b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment Written By:</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCommentWrittenBy

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call No. :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintCallNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"



        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment:</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strComment

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"" valign=""top"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment Date :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCommentDate

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr><br><br>"

        HTML = HTML & "</table>"



        HTML = HTML & "<BR><table style=""border-collapse: collapse"" border=""0"" width=""85%"" height=""5"" bordercolor=""#94BED7"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"" >"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "Thanks</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "WSS Admin</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "</table>"

        HTML = HTML & "</body>"

        HTML = HTML & "</html>"

        Return HTML
    End Function

#End Region

#Region "TaskCommentMail"
    '*******************************************************************
    ' Page                   : -Function GetTaskCommentBody
    ' Purpose              : -function for getting Task Comment Body for mail message 
    ' Date		    			Author						Modification Date					Description
    ' 10/07/06				Harpreet 					10/07/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete HTML 
    '              body for mail message for Task Comment Email.
    ' Code:
    '*******************************************************************

    Private Function GetTaskCommentBody(ByVal strComment As String, ByVal strCommentWrittenBy As String, ByVal strCommentDate As String) As String
        'Make the subject for task mail
        mstrMailSubject = "WSS Alert --- " & strCommentWrittenBy & " Has Written Comment on Task#" & mintTaskNo & " For Call#" & mintCallNo

        Dim HTML

        HTML = "<!DOCTYPE HTML PUBLIC""-//IETF//DTD HTML//EN"">"

        HTML = HTML & "<html>"

        HTML = HTML & "<head>"

        HTML = HTML & "<meta http-equiv=""Content-Type"""

        HTML = HTML & "content=""text/html; charset=iso-8859-1"">"

        HTML = HTML & "<meta name=""GENERATOR"""

        HTML = HTML & " content=""Microsoft Visual Studio 6.0"">"

        HTML = HTML & "<title>HTMLMail</title>"

        HTML = HTML & "</head>"

        HTML = HTML & "<body>"



        HTML = HTML & "Dear User<br><br>"

        HTML = HTML & "<b>Comment</b> for you, Please follow up in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"

        HTML = HTML & "<BR><table bgcolor=""#f5f5f5"" style=""border-collapse: collapse"" border=""1"" width=""85%"" height=""95"" bordercolor=""#e0e0e0"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""28"" align=""left"" bgcolor=""#e0e0e0"">"

        HTML = HTML & "<font color=#000000 size=2 face=""Verdana"">"

        HTML = HTML & "<b><u>New Comment</u></b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment Written By:</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCommentWrittenBy

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call No. :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintCallNo


        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"



        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Task No.:</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintTaskNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"" valign=""top"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment:</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strComment

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment Date :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCommentDate

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr><br><br>"

        HTML = HTML & "</table>"



        HTML = HTML & "<BR><table style=""border-collapse: collapse"" border=""0"" width=""85%"" height=""5"" bordercolor=""#94BED7"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"" >"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "Thanks</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "WSS Admin</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "</table>"

        HTML = HTML & "</body>"

        HTML = HTML & "</html>"

        Return HTML
    End Function

#End Region

#Region "ActionCommentMail"
    '*******************************************************************
    ' Page                   : -Function GetActionCommentBody
    ' Purpose              : -function for getting Action Comment Body for mail message 
    ' Date		    			Author						Modification Date					Description
    ' 10/07/06				Harpreet 					10/07/2006        					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete HTML 
    '              body for mail message for Action Comment Email.
    ' Code:
    '*******************************************************************

    Private Function GetActionCommentBody(ByVal strComment As String, ByVal strCommentWrittenBy As String, ByVal strCommentDate As String) As String
        mstrMailSubject = "WSS Alert --- " & strCommentWrittenBy & " Has Written Comment on Action#" & mintActionNo & " of Task#" & mintTaskNo & " For Call#" & mintCallNo

        Dim HTML

        HTML = "<!DOCTYPE HTML PUBLIC""-//IETF//DTD HTML//EN"">"

        HTML = HTML & "<html>"

        HTML = HTML & "<head>"

        HTML = HTML & "<meta http-equiv=""Content-Type"""

        HTML = HTML & "content=""text/html; charset=iso-8859-1"">"

        HTML = HTML & "<meta name=""GENERATOR"""

        HTML = HTML & " content=""Microsoft Visual Studio 6.0"">"

        HTML = HTML & "<title>HTMLMail</title>"

        HTML = HTML & "</head>"

        HTML = HTML & "<body>"



        HTML = HTML & "Dear User<br><br>"

        HTML = HTML & "<b>Comment</b> for you, Please follow up in WSS on <b><a href=""" & strLink & """>Web Support System</a></b></font>"

        HTML = HTML & "<BR><table bgcolor=""#f5f5f5"" style=""border-collapse: collapse"" border=""1"" width=""85%"" height=""95"" bordercolor=""#e0e0e0"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""28"" align=""left"" bgcolor=""#e0e0e0"">"

        HTML = HTML & "<font color=#000000 size=2 face=""Verdana"">"

        HTML = HTML & "<b><u>New Comment</u></b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment Written By:</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCommentWrittenBy

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Call No. :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""40%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintCallNo


        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"



        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Task No.:</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintTaskNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"" valign=""top"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Action No. :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & mintActionNo

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment :</b></font> "

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strComment

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "<b>Comment Date :</b></font>"

        HTML = HTML & "</td>"

        HTML = HTML & "<td width=""20%"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & strCommentDate

        HTML = HTML & "</font></td>"

        HTML = HTML & "</tr><br><br>"

        HTML = HTML & "</table>"



        HTML = HTML & "<BR><table style=""border-collapse: collapse"" border=""0"" width=""85%"" height=""5"" bordercolor=""#94BED7"">"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"" >"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "Thanks</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "<tr>"

        HTML = HTML & "<td colspan=""2"" height=""8"" align=""left"">"

        HTML = HTML & "<font size=2 face=""Verdana"">"

        HTML = HTML & "WSS Admin</font>"

        HTML = HTML & "</td>"

        HTML = HTML & "</tr>"

        HTML = HTML & "</table>"

        HTML = HTML & "</body>"

        HTML = HTML & "</html>"

        Return HTML
    End Function

#End Region

End Class
#End Region
