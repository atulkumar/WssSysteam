'*******************************************************************
' Page                   : - class clsComnSetup
' Purpose              : - It will send the mail/SMS message
' Date		    			Author						Modification Date					Description
' 17/05/06				Jagtar 					06/06/2006        					Created
'
' Notes: This class hold the function GetMail(). This function accepts some parameters and 
'             returns the complete mail message.
' Code:
'*******************************************************************

Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Mail
Imports System.Web
'Imports System.Net.Mail
Imports System.Net
Imports System.Data


'1	Call Opened
'2	Task Assigned
'3	Action Filled
'4	Task Forwarded
'8	Additive
'9	Conditional
'13	Change Status Call
'14	Call Closed
'15	Task Closed
'16	Change Status Task
'17	New User
'18	OverRide

Public Class clsComSetup

    Private Shared strConn As String
    Private htUserCondition As New Hashtable
    Private htUserDefaultCondition As New Hashtable
    Private htUserRoleCondition As New Hashtable
    Private htDefaultUser As New Hashtable
    Private htUsers As New Hashtable
    Private htUserRole As New Hashtable
    Private htUserInvolved As New Hashtable
    Private htOverRideUsers As New Hashtable
    Private htOverRideRolesUsers As New Hashtable
    Private intFiredEvent As Int16
    Private strPriority, strTaskType, strCallType, strTaskStatus, strCallStatus As String
    Private dtLogDate As Date
    Private strMainFilter, strConditionFilter, strBody, strSubject, strMailListComnt As String
    Private alRoleUser As New ArrayList
    Private intCompanyID, intProjectID, intCallNo, intTaskNo, intBaseEvent, intLogNo, intActionNo As Int32
    Private strInputString As String
    Private flag As Int16
    Private isUpdated As Boolean
    Private intEvntID As Int32
    Private objCon As SqlConnection
    Private dtMain As New DataTable
    Private dtPrority As New DataTable
    Private dtProrityDesc As New DataTable
    Private dtUserPriority As New DataTable
    Private strDNS As String
    Public Shared htDomainValidation As New Hashtable
    Enum GeneralComment
        General = 1
        Comment = 2
    End Enum

    Private Function SendMailOnComment() As Boolean
        Dim strMailListValid, strMailListInValid As String
        Dim objMail As MailMessage
        Dim objBody As New clsMailBody
        strMailListComnt = "dknitinjain@30gigs.com;ranvijay.sahay@ionsoftnet.com;jsidhu@abc.com;jagtar_sidhu@yahoo.com"
        If strMailListComnt = "" Then
            Return False
        End If
        Dim strDelimt As String = ";"
        Dim chDelimt As Char() = strDelimt.ToCharArray()
        Dim strarrMialID As String() = Nothing
        strarrMialID = strMailListComnt.Split(chDelimt)
        Dim strMailID As String
        For Each strMailID In strarrMialID
            strDNS = strMailID.Substring(strMailID.IndexOf("@") + 1)
            If ValidateDomain(strDNS) = True Then
                If strMailListValid = "" Then
                    strMailListValid = strMailID
                Else
                    strMailListValid = strMailListValid & ";" & strMailID
                End If
            Else
                If strMailListInValid = "" Then
                    strMailListInValid = strMailID
                Else
                    strMailListInValid = strMailListInValid & ";" & strMailID
                End If
            End If
        Next
        Try
            If intActionNo > 0 Then
                objMail = objBody.GetMail(intEvntID, 0, clsMailBody.MailType.ActionComment, Me.intCompanyID, Me.intCallNo, intTaskNo, intActionNo, intLogNo)
            ElseIf intTaskNo > 0 Then
                objMail = objBody.GetMail(intEvntID, 0, clsMailBody.MailType.TaskComment, Me.intCompanyID, Me.intCallNo, intTaskNo, 0, intLogNo)
            Else
                objMail = objBody.GetMail(intEvntID, 0, clsMailBody.MailType.CallComment, Me.intCompanyID, Me.intCallNo, 0, 0, intLogNo)
            End If
            If strMailListValid <> "" Then
                Dim url As String = "http://www.microsoft.com"

                objMail.To = strMailListValid
                objMail.BodyFormat = MailFormat.Html

                objMail.From = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                objMail.UrlContentBase = url
                SmtpMail.SmtpServer = System.Configuration.ConfigurationSettings.AppSettings("ServerMail")
                SmtpMail.Send(objMail)
            End If
            If strMailListInValid <> "" Then
                Dim objMailFDel As New MailMessage
                Dim strBody As String
                strBody = "E-mail could not be delivered to following e-mail ID(s) because of invalid domain name<BR>"
                strBody = strBody & "<b>" & strMailListInValid & "</b><br>" & objMail.Body
                Try
                    With objMailFDel
                        .From = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                        .To = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                        .BodyFormat = MailFormat.Html
                        .Body = strBody
                        .Subject = "Failure delivery on Comment mails !!! " & objMail.Subject
                    End With
                    SmtpMail.Send(objMailFDel)
                Catch ex As Exception
                    CreateLog("Send Mail Invalid", "ShowValues-274", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message & " To= " & objMailFDel.To & " CC=" & objMailFDel.Cc, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
                Finally
                    objMailFDel = Nothing
                End Try
            End If
        Catch ex As Exception
            CreateLog("Send Mail", "ShowValues-281", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message & " To= " & objMail.To & " CC=" & objMail.Cc, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            objMail = Nothing
        End Try
        Return True
    End Function
    Public Function InvolvedUsers(ByVal GenComnt As GeneralComment, ByVal strMailListComnt As String, ByVal intCallNo As Int32, ByVal intTaskNo As Int32, ByVal intActionNo As Int32, ByVal intFiredEvent As Int32, ByVal strPriority As String, ByVal intProjectID As String, ByVal strCallType As String, ByVal strTaskType As String, ByVal strCallStatus As String, ByVal strTaskStatus As String, ByVal dtLogDate As Date, ByVal intCompanyID As Int32, ByVal intLogNo As Int32, ByVal intBaseEvent As clsMailBody.MailType, ByVal objCon As SqlConnection) As Boolean
        Dim dt As DataTable
        Dim i, j, k As Int16
        Dim str As String
        Dim ent As DictionaryEntry
        Dim objMail As MailMessage
        'System.Diagnostics.Debugger.Launch()
        'D Default Users
        'R Role Users
        'O Other Users
        'A Override Users
        'B Override Role Users
        'strConn =  System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        If intCallNo = 0 And intTaskNo = 0 Then
            Exit Function
        End If
        Me.intCallNo = intCallNo
        Me.intTaskNo = intTaskNo
        Me.intActionNo = intActionNo
        Me.intCompanyID = intCompanyID
        Me.strPriority = strPriority
        Me.intProjectID = intProjectID
        Me.strTaskType = strTaskType
        Me.strCallType = strCallType
        Me.strTaskStatus = strTaskStatus
        Me.strCallStatus = strCallStatus
        Me.dtLogDate = dtLogDate
        Me.intLogNo = intLogNo

        Me.objCon = objCon
        Me.intBaseEvent = intBaseEvent

        If GenComnt = GeneralComment.Comment Then
            Me.strMailListComnt = strMailListComnt
            If SendMailOnComment() = True Then
                Return True
            Else
                Return False
            End If
        End If
        Try

            htUserCondition.Clear()
            htUserDefaultCondition.Clear()
            htUserRoleCondition.Clear()
            htDefaultUser.Clear()
            htUsers.Clear()
            htUserRole.Clear()
            htUserInvolved.Clear()
            htOverRideUsers.Clear()
            htOverRideRolesUsers.Clear()


            If intFiredEvent = 19 Then
                If intCallNo <> 0 And intTaskNo = 0 Then
                    Me.intFiredEvent = 1
                ElseIf intCallNo <> 0 And intTaskNo <> 0 And intActionNo = 0 Then
                    Me.intFiredEvent = 2
                ElseIf intCallNo <> 0 And intTaskNo <> 0 And intActionNo <> 0 Then
                    Me.intFiredEvent = 3
                Else
                    Me.intFiredEvent = 0
                End If
                'isUpdated = True
            Else
                'isUpdated = False
                Me.intFiredEvent = intFiredEvent
            End If
            intEvntID = intFiredEvent
            If Me.intFiredEvent = 0 Then
                Exit Function
            End If
            'intFiredEvent = 14
            'strPriority = "URN"
            'intCompanyID = 8
            'intProjectID = 0
            'strTaskType = "COD"
            'strCallType = "WSS"
            'strTaskStatus = "CLS"
            'strCallStatus = "CLS"
            'dtStart = "01-01-2005"
            'dtStop = "01-01-2007"
            If intFiredEvent = 14 Then
                strInputString = "(Company_ID=" & intCompanyID & " or Company_ID =0) and " _
                & " (Priority='" & strPriority & "' or Priority is null) and " _
                & " (Event_Fired_ID_FK in (" & Me.intFiredEvent & ",13)" & " or Event_Fired_ID_FK = 0) and " _
                & " (Task_Type='" & strTaskType & "' or Task_Type is null) and " _
                & " (Call_Type = '" & strCallType & "' Or Call_Type Is null) and " _
                & " (Project_ID=" & intProjectID & " or Project_id = 0 ) and " _
                & " (Task_Status='" & strTaskStatus & "' or Task_Status is null) and " _
                & " (Call_Status ='" & strCallStatus & "' Or Call_Status Is null) and " _
                & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
                & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
                & " or (Start_Date is null and Stop_Date is null))"

            ElseIf intFiredEvent = 15 Then
                strInputString = "(Company_ID=" & intCompanyID & " or Company_ID =0) and " _
                & " (Priority='" & strPriority & "' or Priority is null) and " _
                & " (Event_Fired_ID_FK in (" & Me.intFiredEvent & ",16)" & " or Event_Fired_ID_FK = 0) and " _
                & " (Task_Type='" & strTaskType & "' or Task_Type is null) and " _
                & " (Call_Type = '" & strCallType & "' Or Call_Type Is null) and " _
                & " (Project_ID=" & intProjectID & " or Project_id = 0 ) and " _
                & " (Task_Status='" & strTaskStatus & "' or Task_Status is null) and " _
                & " (Call_Status ='" & strCallStatus & "' Or Call_Status Is null) and " _
                & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
                & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
                & " or (Start_Date is null and Stop_Date is null))"

            Else
                strInputString = "(Company_ID=" & intCompanyID & " or Company_ID =0) and " _
                            & " (Priority='" & strPriority & "' or Priority is null) and " _
                            & " (Event_Fired_ID_FK=" & Me.intFiredEvent & " or Event_Fired_ID_FK = 0) and " _
                            & " (Task_Type='" & strTaskType & "' or Task_Type is null) and " _
                            & " (Call_Type = '" & strCallType & "' Or Call_Type Is null) and " _
                            & " (Project_ID=" & intProjectID & " or Project_id = 0 ) and " _
                            & " (Task_Status='" & strTaskStatus & "' or Task_Status is null) and " _
                            & " (Call_Status ='" & strCallStatus & "' Or Call_Status Is null) and " _
                            & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
                            & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
                            & " or (Start_Date is null and Stop_Date is null))"
            End If
        Catch ex As Exception
            CreateLog("First Involved users," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-175", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
        Try
            FillCache()
            GetPriorityParams()
        Catch ex As Exception
            CreateLog("FillCache and GetPriorityParams," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-181", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)

        End Try

        Try
            'dt = HttpContext.Current.Cache("UserPriority")
            Try
                dt = dtUserPriority

                For i = 0 To dt.Rows.Count - 1
                    Select Case dt.Rows(i).Item("User_Priority_ID")

                        Case 1
                            For Each ent In htOverRideUsers
                                If htUserInvolved.Contains(ent.Key) = False Then
                                    htUserInvolved.Add(ent.Key, ent.Value)
                                End If
                            Next
                        Case 2
                            For Each ent In htOverRideRolesUsers
                                If htUserInvolved.Contains(ent.Key) = False Then
                                    htUserInvolved.Add(ent.Key, ent.Value)
                                End If
                            Next

                        Case 3
                            For Each ent In htUsers
                                If htUserCondition.Contains(ent.Key) = True Then
                                    If htUserInvolved.Contains(ent.Key) = False Then
                                        htUserInvolved.Add(ent.Key, htUserCondition(ent.Key))
                                    End If
                                Else
                                    If htUserInvolved.Contains(ent.Key) = False Then
                                        htUserInvolved.Add(ent.Key, ent.Value)
                                    End If
                                End If
                            Next
                        Case 4
                            For Each ent In htUserRole
                                If htUserRoleCondition.Contains(ent.Key) = True Then
                                    If htUserInvolved.Contains(ent.Key) = False Then
                                        htUserInvolved.Add(ent.Key, htUserRoleCondition(ent.Key))
                                    End If
                                Else
                                    If htUserInvolved.Contains(ent.Key) = False Then
                                        htUserInvolved.Add(ent.Key, ent.Value)
                                    End If
                                End If
                            Next
                        Case 5
                            For Each ent In htDefaultUser
                                If htUserDefaultCondition.Contains(ent.Key) = True Then
                                    If htUserInvolved.Contains(ent.Key) = False Then
                                        htUserInvolved.Add(ent.Key, htUserDefaultCondition(ent.Key))
                                    End If
                                Else
                                    If htUserInvolved.Contains(ent.Key) = False Then
                                        htUserInvolved.Add(ent.Key, ent.Value)
                                    End If
                                End If
                            Next
                    End Select
                Next
            Catch ex As Exception
                CreateLog("Loop hash users," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-235", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
            Dim intSMSBit, intMailBit As Int32
            Dim strTemp As String
            Dim strIDUsers As String
            Dim dtSMSMailID As DataTable
            Dim dv As DataView
            Try
                If htUserInvolved.Count = 0 Then
                    Return True
                End If
                For Each ent In htUserInvolved
                    If strIDUsers <> "" Then
                        strIDUsers = strIDUsers & "," & ent.Key
                    Else
                        strIDUsers = strIDUsers & ent.Key
                    End If

                Next
            Catch ex As Exception
                CreateLog("getstringIdUsers," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-265", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

            End Try

            Try
                dtSMSMailID = GetSMSMailIDs(strIDUsers)
                dv = New DataView(dtSMSMailID)
                dv.Sort = "CI_NU8_Address_Number"
            Catch ex As Exception
                CreateLog("Get mail IDs," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-257", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

            End Try
            Dim strMailList As String

            Dim intIndex As Int16
            Dim strMail, strSMS As String
            Dim intIsDefIncluded As Int16
            Dim objBody As New clsMailBody
            Dim strMailTo, strMailCC As String
            Dim strInvalidID As String
            Dim intIsToDisabled As Int16
            Try
                If intBaseEvent = clsMailBody.MailType.CallMail Then
                    objMail = objBody.GetMail(intEvntID, intLogNo, clsMailBody.MailType.CallMail, Me.intCompanyID, Me.intCallNo, 0, 0)
                ElseIf intBaseEvent = clsMailBody.MailType.TaskMail Then
                    objMail = objBody.GetMail(intEvntID, intLogNo, clsMailBody.MailType.TaskMail, Me.intCompanyID, Me.intCallNo, intTaskNo, 0)
                ElseIf intBaseEvent = clsMailBody.MailType.ActionMail Then
                    objMail = objBody.GetMail(intEvntID, intLogNo, clsMailBody.MailType.ActionMail, Me.intCompanyID, Me.intCallNo, intTaskNo, intActionNo)
                End If
                strMailTo = objMail.To.ToString
                strMailTo = strMailTo.ToLower.Trim
                strDNS = strMailTo.Substring(strMailTo.IndexOf("@") + 1)
                If ValidateDomain(strDNS) = False Then
                    strInvalidID = strMailTo
                    objMail.To = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                End If
            Catch ex As Exception
                CreateLog("Get mail body," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-261", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                Return False
            End Try
            Try

                For Each ent In htUserInvolved
                    str = str & ent.Key & ";" & ent.Value & "#"
                    strTemp = ent.Value
                    intSMSBit = Val(strTemp.Substring(0, 1))
                    intMailBit = Val(strTemp.Substring(2, 1))
                    intIndex = dv.Find(ent.Key)
                    strMailCC = dv.Item(intIndex).Item("CI_VC28_Email_1")
                    strMailCC = strMailCC.ToLower.Trim
                    strDNS = strMailCC.Substring(strMailCC.IndexOf("@") + 1)
                    If ValidateDomain(strDNS) = True Then
                        If (strMailTo = strMailCC) Then
                            intIsDefIncluded = 1
                        End If
                        If (strMailTo = strMailCC) And intMailBit = 0 Then
                            objMail.To = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                            intIsToDisabled = 1
                        ElseIf (strMailTo <> strMailCC) Then
                            If intMailBit = 1 Then
                                'intIndex = dv.Find(ent.Key)
                                'strMail = dv.Item(intIndex).Item("CI_VC28_Email_1")
                                If strMailList = "" Then
                                    strMailList = strMailCC
                                Else
                                    strMailList = strMailList & ";" & strMailCC
                                End If
                            Else
                                strSMS = dv.Item(intIndex).Item("CI_NU16_Phone_Number_1")
                            End If
                        End If
                    Else
                        strInvalidID = strInvalidID & " " & strMailCC
                    End If
                Next

                If intIsDefIncluded = 0 Then
                    objMail.To = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                End If
            Catch ex As Exception
                CreateLog("Prepare mail list," & intCallNo & "," & intTaskNo & "," & intActionNo, "ShowValues-328", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

            End Try

            Try
                If Not (intIsDefIncluded = 0 And strMailList = "") Then
                    If Not (strMailList = "" And intIsToDisabled = 1) Then
                        Dim url As String = "http://www.microsoft.com"

                        objMail.Cc = strMailList

                        objMail.From = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                        objMail.UrlContentBase = url

                        'objMail.Body = strBody
                        'objMail.Subject = strSubject
                        SmtpMail.SmtpServer = System.Configuration.ConfigurationSettings.AppSettings("ServerMail")
                        SmtpMail.Send(objMail)
                    End If
                End If
                'SmtpMail.Send(objMail.From, objMail.To, objMail.Subject, objMail.Body)
                If strInvalidID <> "" Then
                    Dim objMailFDel As New MailMessage
                    Dim strBody As String
                    strBody = "E-mail could not be delivered to following e-mail ID(s) because of invalid domain name<BR>"
                    strBody = strBody & "<b>" & strInvalidID & "</b><br>" & objMail.Body
                    Try
                        With objMailFDel
                            .From = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                            .To = System.Configuration.ConfigurationSettings.AppSettings("FromMail")
                            .BodyFormat = MailFormat.Html
                            .Body = strBody
                            .Subject = "Failure delivery !!! " & objMail.Subject
                        End With
                        SmtpMail.Send(objMailFDel)
                    Catch ex As Exception
                        CreateLog("Send Mail Invalid", "ShowValues-274", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message & " To= " & objMailFDel.To & " CC=" & objMailFDel.Cc, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

                    Finally
                        objMailFDel = Nothing
                    End Try

                End If
            Catch ex As Exception
                CreateLog("Send Mail", "ShowValues-281", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message & " To= " & objMail.To & " CC=" & objMail.Cc, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                Return False
            Finally
                objMail = Nothing

            End Try

            'For Each ent In htUserInvolved
            '    str = str & ent.Key & ";" & ent.Value & "#"
            '    strTemp = ent.Value
            '    intSMSBit = Val(strTemp.Substring(0, 1))
            '    intMailBit = Val(strTemp.Substring(2, 1))
            '    intIndex = dv.Find(ent.Key)
            '    strMail = dv.Item(intIndex).Item("CI_VC28_Email_1")
            '    strSMS = dv.Item(intIndex).Item("CI_NU16_Phone_Number_1")

            '    If intSMSBit <> 0 Or intMailBit <> 0 And (strMail <> "" Or strSMS <> "") Then
            '        SendSMSMail(ent.Key, intSMSBit, intMailBit, strMail, strSMS)
            '    End If
            'Next
            Return True
        Catch ex As Exception
            objMail = Nothing
            CreateLog("ComSetup", "InvolvedUsers-311", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")
            Return False
        End Try
    End Function
    Private Function ValidateDomain(ByVal strDomain As String) As Boolean
        If htDomainValidation.ContainsKey(strDNS) = False Then
            If ValidateDomainNow(strDomain) = True Then

                htDomainValidation.Add(strDomain, "T")
                Return True
            Else
                htDomainValidation.Add(strDomain, "F")
                Return False
            End If
        Else
            If htDomainValidation(strDomain) = "T" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Private Function ValidateDomainNow(ByVal strDomain As String) As Boolean
        Try
            'Dim host As String() = (strMailID.Split("@"))
            'Dim hostname As String = host(1)
            Dim IPhst As IPHostEntry = Dns.Resolve(strDomain)

            'Dim endPt As IPEndPoint = New IPEndPoint(IPhst.AddressList(0), 25)
            'Dim s As System.Net.Sockets.Socket = New System.Net.Sockets.Socket(endPt.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
            's.Connect(endPt)
            Return True
        Catch ex As Exception
            CreateLog("Send Mail", "ValidateDomainNow-534", LogType.Application, LogSubType.Exception, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    'Private Function SendSMSMail(ByVal intAB As Int32, ByVal intSMS As Int32, ByVal intMail As Int32, ByVal strMail As String, ByVal strSMS As String) As Boolean
    '    'Dim strSMS, strMail As String
    '    'GetSMSMailIDs(intAB, strSMS, strMail)
    '    System.Diagnostics.Debugger.Launch()
    '    If intMail = 1 Then

    '        If strMail <> "" And strMail <> "0" Then
    '            Dim objMessageTO As MailMessage = New MailMessage
    '            Try
    '                Dim url As String = "http://www.microsoft.com"
    '                SmtpMail.SmtpServer = System.Configuration.ConfigurationSettings.AppSettings("ServerMail")
    '                'Dim strSubject As String = "Mail for Challan"

    '                'objMessageTO = objMail

    '                With objMail
    '                    .To = strMail
    '                    .From = "WSS"
    '                    '.Subject = strSubject
    '                    '.Body = strBody
    '                    '.BodyFormat = MailFormat.Html
    '                    .UrlContentBase = url
    '                End With
    '                SmtpMail.Send(objMail)
    '                'objMessageTO = Nothing
    '                Return True
    '            Catch ex As Exception
    '                objMessageTO = Nothing
    '                Return False
    '                CreateLog("ComSetup", "ShowValues-1171", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA", False)

    '            End Try
    '        End If
    '    End If
    '    If intSMS = 1 Then
    '        If strSMS <> "" And strSMS <> "0" Then

    '        End If
    '    End If

    'End Function
    Private Function GetSMSMailIDs(ByVal strInUsers As String) As DataTable
        Dim DT As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strFilter As String
        Dim dvData As DataView
        ''Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            QueryString = "select CI_NU8_Address_Number, CI_VC28_Email_1,CI_NU16_Phone_Number_1 from T010011 where " _
            & " CI_NU8_Address_Number in (" & strInUsers & ")"
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
                .ExecuteNonQuery()
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)
            Return DT
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-1217", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function

    Private Function GetCallTaskUsers(ByVal Mode As Int16) As DataTable
        Dim DT As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strFilter As String
        Dim dvData As DataView
        'Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            If Mode = 1 Then
                QueryString = "select CM_NU9_Call_Owner as Call_Owner,CM_NU9_On_behalf_emp as call_Opened_By from T040011 where " _
                & " CM_NU9_Call_No_PK =" & intCallNo & " And CM_NU9_Comp_Id_FK =" & intCompanyID
            ElseIf Mode = 2 Then
                If intFiredEvent = 4 Then
                    QueryString = "Select TM_NU9_Forwd_emp as Forward_By, TM_NU9_Forward_To as Forwarded_To," _
                    & " TM_NU9_Assign_by as Assigned_By,TM_VC8_Supp_Owner as assigned_To " _
                    & " from T990021 where " _
                    & " TM_NU9_Task_no_PK = " & intTaskNo & " and " _
                    & " TM_NU9_Comp_ID_FK = " & intCompanyID & " and " _
                    & " TM_NU9_Call_No_FK = " & intCallNo & " and " _
                    & " TM_NU9_TaskLog_No = " & intLogNo

                Else
                    QueryString = "Select TM_NU9_Forwd_emp as Forward_By, TM_NU9_Forward_To as Forwarded_To," _
                & " TM_NU9_Assign_by as Assigned_By,TM_VC8_Supp_Owner as assigned_To " _
                & " from T040021 where " _
                & " TM_NU9_Task_no_PK = " & intTaskNo & " and " _
                & " TM_NU9_Comp_ID_FK = " & intCompanyID & " and " _
                & " TM_NU9_Call_No_FK = " & intCallNo

                End If
            ElseIf Mode = 3 Then

            End If
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)
            Return DT
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-1255", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function

    Private Function FillCache() As Boolean
        Dim DT As New DataTable
        Dim DT1 As New DataTable
        Dim DT2 As New DataTable
        Dim DT3 As New DataTable
        Dim dtChq As New DataTable
        Dim DA As SqlDataAdapter
        Dim DA1 As SqlDataAdapter
        Dim DA2 As SqlDataAdapter
        Dim DA3 As SqlDataAdapter
        Dim DA4 As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        'Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            QueryString = "Select a.* from setup_rules a,EventUserMaster b,EventMaster c " _
            & " where a.Rule_Status=1 and a.Event_User_ID_FK *= b.Event_User_ID_PK and " _
            & " a.Event_ID_FK = c.Event_ID_PK and (c.Enabled = 'Y' or c.event_id_pk=18) order by b.Order_By"
            If objCon.State = ConnectionState.Closed Then
                objCon.Open()
            End If
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(dtMain)
            QueryString = "Select obj_id,Object_Name,Priority from priorityobject where Priority <> 0 " _
                            & " order by priority asc"
            With objCommand
                .CommandText = QueryString
            End With
            DA2 = New SqlDataAdapter(objCommand)

            DA2.Fill(dtPrority)

            QueryString = "Select obj_id,Object_Name,Priority from priorityobject where Priority <> 0 " _
                            & " order by priority desc"
            With objCommand
                .CommandText = QueryString
            End With
            DA4 = New SqlDataAdapter(objCommand)

            DA4.Fill(dtProrityDesc)

            QueryString = "Select * from PriorityUsers order by priority"


            With objCommand
                .CommandText = QueryString
            End With
            DA3 = New SqlDataAdapter(objCommand)
            DA3.Fill(dtUserPriority)
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-1329", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function

    Private Function FillCache1() As Boolean
        Dim DT As New DataTable
        Dim DT1 As New DataTable
        Dim DT2 As New DataTable
        Dim DT3 As New DataTable
        Dim dtChq As New DataTable
        Dim DA As SqlDataAdapter
        Dim DA1 As SqlDataAdapter
        Dim DA2 As SqlDataAdapter
        Dim DA3 As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        'Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            If IsNothing(HttpContext.Current.Cache("CommSetup")) Then
                QueryString = "Select a.* from setup_rules a,EventUserMaster b,EventMaster c " _
                & " where a.Rule_Status=1 and a.Event_User_ID_FK *= b.Event_User_ID_PK and " _
                & " a.Event_ID_FK = c.Event_ID_PK and c.Enabled = 'Y' order by b.Order_By"

                objCommand = New SqlCommand
                With objCommand
                    .CommandText = QueryString
                    .CommandType = CommandType.Text
                    .Connection = objCon
                End With
                DA = New SqlDataAdapter(objCommand)
                DA.Fill(DT)
                HttpContext.Current.Cache.Remove("CommSetup")
                If IsNothing(HttpContext.Current.Cache("CommSetup")) Then
                    HttpContext.Current.Cache.Insert("CommSetup", DT, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
                Else
                    HttpContext.Current.Cache.Remove("CommSetup")
                    HttpContext.Current.Cache.Insert("CommSetup", DT, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
                End If
            End If

            If IsNothing(HttpContext.Current.Cache("PListDesc")) Then
                QueryString = "Select obj_id,Object_Name,Priority from priorityobject where Priority <> 0 " _
                                & " order by priority desc"
                With objCommand
                    .CommandText = QueryString
                End With
                DA2 = New SqlDataAdapter(objCommand)
                DA2.Fill(DT2)
                If IsNothing(HttpContext.Current.Cache("PListDesc")) Then
                    HttpContext.Current.Cache.Insert("PListDesc", DT2, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(200))
                Else
                    HttpContext.Current.Cache.Remove("PListDesc")
                    HttpContext.Current.Cache.Insert("PListDesc", DT2, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(200))
                End If

            End If
            QueryString = "Select * from PriorityUsers order by priority"
            With objCommand
                .CommandText = QueryString
            End With
            If IsNothing(HttpContext.Current.Cache("UserPriority")) Then
                DA3 = New SqlDataAdapter(objCommand)
                DA3.Fill(DT3)
                If IsNothing(HttpContext.Current.Cache("UserPriority")) Then
                    HttpContext.Current.Cache.Insert("UserPriority", DT3, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(200))
                Else
                    HttpContext.Current.Cache.Remove("UserPriority")
                    HttpContext.Current.Cache.Insert("UserPriority", DT3, Nothing, HttpContext.Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(200))
                End If
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-1329", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function
    Private Function GetRoleUsers(ByVal RoleID As Int32, ByVal intSMS As Int16, ByVal intMail As Int16, ByVal intMode As Int16) As Boolean
        Dim DT As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strFilter As String
        Dim dvData As DataView
        'Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            QueryString = "Select RA_IN4_AB_ID_FK from T060022 where RA_VC4_Status_Code = 'ENB' " _
            & " and RA_IN4_Role_ID_FK =" & RoleID
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)
            For i As Int16 = 0 To DT.Rows.Count - 1
                If intMode = 1 Then
                    If IsNothing(htUserRole.Item(DT.Rows(i).Item(0))) Then
                        htUserRole.Add(DT.Rows(i).Item(0), intSMS & "," & intMail & "#" & "R")
                    End If
                End If
            Next
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-1363", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function
    Private Function CheckConditionDefaultUsers(ByVal intUser As Int32) As Boolean
        Dim dvTemp As DataView
        Dim DT As DataTable
        Dim dtRules As DataTable
        'DT = HttpContext.Current.Cache("PListDesc")
        DT = dtPrority
        Dim strDateRange As String
        Dim i, j, k As Int16
        'dtRules = HttpContext.Current.Cache("CommSetup")
        dtRules = dtMain
        Dim dvCondition As DataView
        Dim strFilter As String
        Dim intSMS, intMail, intRecCount As Int16
        flag = 0
        strFilter = "event_id_fk = 9 and " & strInputString & " and (user_id=" & intUser & " or (user_id=0 and role_id=0))"
        'Filter records
        Try
            dvCondition = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)


            If dvCondition.Count > 1 Then
                'DT = HttpContext.Current.Cache("PListDesc")
                DT = dtPrority
                dvTemp = dvCondition
                intRecCount = dvTemp.Count


                For k = 0 To DT.Rows.Count - 1
                    Dim str As String
                    Try
                        Select Case DT.Rows(k).Item(0)
                            Case 1
                                'Event_Fired_ID_FK
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intFiredEvent
                            Case 2
                                'User_ID
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intUser
                            Case 3
                                'Role_ID

                            Case 4
                                'Company_ID
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intCompanyID
                            Case 5
                                'Priority
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                            Case 6
                                'Task_Type
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strTaskType & "'"
                            Case 7
                                'Call_Type
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strCallType & "'"
                            Case 8
                                'Project_ Name
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intProjectID
                            Case 9
                                'Status_Change_on_off
                            Case 10
                                'Task_Status
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strTaskStatus & "'"
                            Case 11
                                'Call_Status
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strCallStatus & "'"
                            Case 12
                                'Start_Date
                            Case 13
                                'Stop_Date
                        End Select
                    Catch ex As Exception
                    End Try
                    If dvTemp.Count = 1 Then
                        intSMS = dvTemp.Item(0).Item("SMS_on_off")
                        intMail = dvTemp.Item(0).Item("Mail_on_off")
                        Exit For
                    End If

                    If dvTemp.Count > 1 Then
                        If dvTemp.Count < intRecCount Then
                            strFilter = dvTemp.RowFilter
                            intRecCount = dvTemp.Count
                        End If
                    End If
                    If k = DT.Rows.Count - 1 Then
                        dvTemp.RowFilter = strFilter
                        intSMS = dvTemp.Item(dvTemp.Count - 1).Item("SMS_on_off")
                        intMail = dvTemp.Item(dvTemp.Count - 1).Item("Mail_on_off")
                    End If
                Next
                If htUserDefaultCondition.ContainsKey(intUser) = False Then
                    htUserDefaultCondition.Add(intUser, intSMS & "," & intMail & "#" & "D")
                End If
            ElseIf dvCondition.Count = 1 Then
                If htUserDefaultCondition.ContainsKey(intUser) = False Then
                    htUserDefaultCondition.Add(intUser, dvCondition.Item(0).Item("SMS_on_off") & "," & dvCondition.Item(0).Item("Mail_on_off") & "#" & "D")
                End If
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "ShowValues-1456", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function

    Private Function GetPriorityParams() As Boolean
        Dim strFilter As String
        Dim dvData As DataView
        Dim dvDefault As DataView
        Dim dtRules As DataTable
        Dim i, j As Int16
        Try

            'Get all records of fired event
            ' 8 for Additive, 9 for Conditional
            If intFiredEvent = 14 Then
                strFilter = " (event_id_fk in (" & intFiredEvent & ",13)" & " or event_id_fk = 8) " _
                & " and Event_User_ID_FK = 0  and " & strInputString
            ElseIf intFiredEvent = 15 Then
                strFilter = " (event_id_fk in (" & intFiredEvent & ",16)" & " or event_id_fk = 8) " _
                & " and Event_User_ID_FK = 0  and " & strInputString
            Else
                strFilter = " (event_id_fk = " & intFiredEvent & " or event_id_fk = 8) " _
                & " and Event_User_ID_FK = 0  and " & strInputString
            End If

            strMainFilter = strFilter
            'dtRules = HttpContext.Current.Cache("CommSetup")
            dtRules = dtMain

            'Filter records
            dvData = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)
            If intFiredEvent = 14 Then
                strFilter = " event_id_fk in (" & intFiredEvent & ",13)" _
                & " and Event_User_ID_FK <> 0 and (Company_ID=" & intCompanyID & " or Company_ID =0) and " _
                & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
                & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
                & " or (Start_Date is null and Stop_Date is null))"

            ElseIf intFiredEvent = 15 Then
                strFilter = " event_id_fk in (" & intFiredEvent & ",16)" _
                & " and Event_User_ID_FK <> 0 and (Company_ID=" & intCompanyID & " or Company_ID =0) and " _
                & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
                & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
                & " or (Start_Date is null and Stop_Date is null))"
            Else
                strFilter = " event_id_fk = " & intFiredEvent _
                & " and Event_User_ID_FK <> 0 and (Company_ID=" & intCompanyID & " or Company_ID =0) and " _
                & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
                & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
                & " or (Start_Date is null and Stop_Date is null))"

            End If


            'Filter records
            dvDefault = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)
            Dim dtCallUser As DataTable
            Dim dtTaskUser As DataTable
            Dim intUserID1 As Int32
            If intCallNo <> 0 And intTaskNo = 0 Then
                dtCallUser = GetCallTaskUsers(1)
            ElseIf intTaskNo <> 0 Then
                dtCallUser = GetCallTaskUsers(1)
                dtTaskUser = GetCallTaskUsers(2)
            End If
            Try
                For i = 0 To dvDefault.Count - 1
                    If (Not IsDBNull(dvDefault.Item(i).Item("Event_User_ID_FK"))) Then
                        If Val(dvDefault.Item(i).Item("Event_User_ID_FK")) <> 0 Then
                            If htDefaultUser.ContainsKey(dvDefault.Item(i).Item("Event_User_ID_FK")) = False Then
                                Select Case dvDefault.Item(i).Item("Event_User_ID_FK")
                                    Case 1
                                        If Not IsDBNull(dtCallUser.Rows(0).Item("call_Opened_By")) Then
                                            intUserID1 = dtCallUser.Rows(0).Item("call_Opened_By")
                                            CheckConditionDefaultUsers(intUserID1)
                                            If htDefaultUser.ContainsKey(intUserID1) = False Then
                                                htDefaultUser.Add(intUserID1, dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                                            End If
                                        End If
                                    Case 2
                                        If Not IsDBNull(dtCallUser.Rows(0).Item("Call_Owner")) Then
                                            intUserID1 = dtCallUser.Rows(0).Item("Call_Owner")
                                            CheckConditionDefaultUsers(intUserID1)
                                            If htDefaultUser.ContainsKey(intUserID1) = False Then
                                                htDefaultUser.Add(intUserID1, dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                                            End If
                                        End If

                                    Case 3
                                        If Not IsDBNull(dtTaskUser.Rows(0).Item("Assigned_By")) Then
                                            intUserID1 = dtTaskUser.Rows(0).Item("Assigned_By")
                                            CheckConditionDefaultUsers(intUserID1)
                                            If htDefaultUser.ContainsKey(intUserID1) = False Then
                                                htDefaultUser.Add(intUserID1, dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                                            End If
                                        End If
                                    Case 4
                                        If Not IsDBNull(dtTaskUser.Rows(0).Item("assigned_To")) Then
                                            intUserID1 = dtTaskUser.Rows(0).Item("assigned_To")
                                            CheckConditionDefaultUsers(intUserID1)
                                            If htDefaultUser.ContainsKey(intUserID1) = False Then
                                                htDefaultUser.Add(intUserID1, dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                                            End If
                                        End If

                                    Case 5
                                        If Not IsDBNull(dtTaskUser.Rows(0).Item("Forward_By")) Then
                                            intUserID1 = dtTaskUser.Rows(0).Item("Forward_By")
                                            CheckConditionDefaultUsers(intUserID1)
                                            If htDefaultUser.ContainsKey(intUserID1) = False Then
                                                htDefaultUser.Add(intUserID1, dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                                            End If
                                        End If
                                    Case 6
                                        If Not IsDBNull(dtTaskUser.Rows(0).Item("Forwarded_To")) Then
                                            intUserID1 = dtTaskUser.Rows(0).Item("Forwarded_To")
                                            CheckConditionDefaultUsers(intUserID1)
                                            If htDefaultUser.ContainsKey(intUserID1) = False Then
                                                htDefaultUser.Add(intUserID1, dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                                            End If
                                        End If
                                    Case 7
                                    Case 8
                                    Case 9
                                End Select
                                'htDefaultUser.Add(dvDefault.Item(i).Item("Event_User_ID_FK"), dvDefault.Item(i).Item("SMS_on_off") & "," & dvDefault.Item(i).Item("Mail_on_off") & "#" & "D")
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                CreateLog("ComSetup", "GetPriorityParams-918" & intCompanyID & "," & intCallNo & "," & intTaskNo & "," & intActionNo, LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")
            End Try
            getRoleArray(dvData)
            Dim str As String
            Try
                For j = 0 To alRoleUser.Count - 1
                    str = alRoleUser(j)
                    GetRoleUsers(str.Substring(0, str.IndexOf("#")), str.Substring(str.IndexOf("#") + 1, 1), str.Substring(str.IndexOf(",") + 1, 1), 1)
                    CheckConditionRoles(str.Substring(0, str.IndexOf("#")), str.Substring(str.IndexOf("#") + 1, 1), str.Substring(str.IndexOf(",") + 1, 1))
                Next
            Catch ex As Exception
                CreateLog("ComSetup", "GetPriorityParams-929", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")
            End Try

            dvData.RowFilter = strMainFilter
            Dim k As Int16
            k = dvData.Count - 1
            Try
                For i = 0 To k
                    If (Not IsDBNull(dvData.Item(i).Item("Event_ID_FK"))) Then
                        If Val(dvData.Item(i).Item("Event_ID_FK")) <> 0 Then
                            If (Not IsDBNull(dvData.Item(i).Item("User_ID"))) Then
                                If Val(dvData.Item(i).Item("User_ID")) <> 0 Then
                                    getMainUsers(dvData, Val(dvData.Item(i).Item("User_ID")))
                                    dvData.RowFilter = strMainFilter
                                    CheckConditionUser(Val(dvData.Item(i).Item("User_ID")))
                                    dvData.RowFilter = strMainFilter
                                End If
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                CreateLog("ComSetup", "GetPriorityParams-951", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA", False)
            End Try

            Dim dvOverRide As DataView

            strFilter = "event_id_fk = 18 and " & strInputString & " and ((call_No=" & intCallNo & " and Task_no=0 )" _
         & " or (Task_No=" & intTaskNo & " or call_No = 0) or (Task_No=" & intTaskNo & " or call_No=" & intCallNo _
         & ")) and (user_id <> 0 or role_id <>0) and " _
         & " (Company_ID=" & intCompanyID & ") and " _
         & " ((Start_Date<='" & dtLogDate & "' and Stop_Date>='" & dtLogDate & "')" _
         & " or (Start_Date<='" & dtLogDate & "' and Stop_Date is null )" _
         & " or (Start_Date is null and Stop_Date is null))"
            dvOverRide = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)


            Try
                For i = 0 To dvOverRide.Count - 1
                    If Val(dvOverRide.Item(i).Item("User_ID")) <> 0 Then
                        intUserID1 = dvOverRide.Item(i).Item("User_ID")
                        If htOverRideUsers.ContainsKey(intUserID1) = False Then
                            htOverRideUsers.Add(intUserID1, dvOverRide.Item(i).Item("SMS_on_off") & "," & dvOverRide.Item(i).Item("Mail_on_off") & "#" & "A")
                        End If
                    End If
                    If Val(dvOverRide.Item(i).Item("Role_ID")) <> 0 Then
                        GetOverrideRoleUsers(dvOverRide.Item(i).Item("Role_ID"), dvOverRide.Item(i).Item("SMS_on_off"), dvOverRide.Item(i).Item("Mail_on_off"), 1)
                    End If
                Next
            Catch ex As Exception
                CreateLog("ComSetup", "GetPriorityParams-979", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")
            End Try

        Catch ex As Exception

            CreateLog("ComSetup", "GetPriorityParams-972", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function

    Private Function GetOverrideRoleUsers(ByVal RoleID As Int32, ByVal intSMS As Int16, ByVal intMail As Int16, ByVal intMode As Int16) As Boolean
        Dim DT As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strFilter As String
        Dim dvData As DataView
        Dim intUserID As Int32
        'Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            QueryString = "Select RA_IN4_AB_ID_FK from T060022 where RA_VC4_Status_Code = 'ENB' " _
            & " and RA_IN4_Role_ID_FK =" & RoleID
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)

            For i As Int16 = 0 To DT.Rows.Count - 1
                If intMode = 1 Then
                    intUserID = DT.Rows(i).Item(0)
                    If IsNothing(htOverRideRolesUsers.Item(intUserID)) Then
                        htOverRideRolesUsers.Add(intUserID, intSMS & "," & intMail & "#" & "B")
                    End If
                End If
            Next
        Catch ex As Exception
            CreateLog("ComSetup", "GetPriorityParams-1165", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function


    Private Function getRoleArray(ByVal dvData As DataView)
        'DT for priority list, if Records found more than 1
        Dim i, j, k, l As Int16
        Dim dt As DataTable
        Dim dvTemp As DataView
        Dim dvCount As DataView
        Dim strFilter, strFilterTemp As String

        Dim intSMS, intMail, intRoleID, intRecCount As Int16
        Try
            strFilter = strMainFilter & " and Role_ID<>0"
            strFilterTemp = strFilter
            dvData.RowFilter = strFilter
            dvCount = dvData
            dvTemp = dvData
            alRoleUser.Clear()
            flag = 0
            If dvCount.Count > 1 Then
                'dt = HttpContext.Current.Cache("PListDesc")
                dt = dtPrority
                dvTemp = dvData
                intRecCount = dvTemp.Count
                For j = 0 To dvTemp.Count - 1
                    For k = 0 To dt.Rows.Count - 1
                        Dim str As String
                        Try
                            Select Case dt.Rows(k).Item(0)
                                Case 1
                                    'Event_Fired_ID_FK
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "=" & intFiredEvent
                                    dvTemp.RowFilter = str
                                Case 2
                                    'User_ID
                                    'dvTemp.RowFilter = dt.Rows(j).Item(1) & "=" & alListUsers(i)
                                Case 3
                                    'Role_ID
                                Case 4
                                    'Company_ID
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "=" & intCompanyID
                                    dvTemp.RowFilter = str
                                Case 5
                                    'Priority
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strPriority & "'"
                                    dvTemp.RowFilter = str
                                Case 6
                                    'Task_Type
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strTaskType & "'"
                                    dvTemp.RowFilter = str
                                Case 7
                                    'Call_Type
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strCallType & "'"
                                    dvTemp.RowFilter = str
                                Case 8
                                    'Project_ Name
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "=" & intProjectID
                                    dvTemp.RowFilter = str
                                Case 9
                                    'Status_Change_on_off
                                Case 10
                                    'Task_Status
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strTaskStatus & "'"
                                    dvTemp.RowFilter = str
                                Case 11
                                    'Call_Status
                                    str = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strCallStatus & "'"
                                    dvTemp.RowFilter = str
                                Case 12
                                    'Start_Date
                                Case 13
                                    'Stop_Date
                            End Select
                        Catch ex As Exception
                            CreateLog("ComSetup", "GetPriorityParams-1245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

                        End Try

                        If dvTemp.Count > 1 Then
                            If dvTemp.Count < intRecCount Then
                                'strFilterTemp = dvTemp.RowFilter
                                intRecCount = dvTemp.Count
                                strFilter = dvTemp.RowFilter
                            End If
                        End If
                        If dvTemp.Count = 1 Then
                            intRoleID = dvTemp.Item(0).Item("Role_ID")
                            intSMS = dvTemp.Item(0).Item("SMS_on_off")
                            intMail = dvTemp.Item(0).Item("Mail_on_off")
                            Exit For
                        End If
                        If k = dt.Rows.Count - 1 Then
                            dvTemp.RowFilter = strFilter
                            intRoleID = dvTemp.Item(dvTemp.Count - 1).Item("Role_ID")
                            intSMS = dvTemp.Item(dvTemp.Count - 1).Item("SMS_on_off")
                            intMail = dvTemp.Item(dvTemp.Count - 1).Item("Mail_on_off")
                        End If
                    Next

                    alRoleUser.Add(intRoleID & "#" & intSMS & "," & intMail & "#" & "R")

                    strFilter = strFilterTemp & " and " & "Role_ID<>" & intRoleID
                    strFilterTemp = strFilter

                    dvData.RowFilter = strFilter
                    dvTemp = dvData
                    intRecCount = dvTemp.Count
                    If dvData.Count = 1 Then
                        alRoleUser.Add(dvTemp.Item(0).Item("Role_ID") & "#" & dvTemp.Item(0).Item("SMS_on_off") & "," & dvTemp.Item(0).Item("Mail_on_off") & "#" & "R")
                        Exit For
                    ElseIf dvData.Count = 0 Then
                        Exit For
                    End If
                Next
            ElseIf dvCount.Count = 1 Then
                alRoleUser.Add(dvTemp.Item(0).Item("Role_ID") & "#" & dvTemp.Item(0).Item("SMS_on_off") & "," & dvTemp.Item(0).Item("Mail_on_off") & "#" & "R")
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "GetPriorityParams-1288", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function

    Private Function getMainUsers(ByVal dvData As DataView, ByVal intUserid As Int32)
        'DT for priority list, if Records found more than 1
        Dim i, k As Int16
        Dim dt As DataTable
        Dim dvTemp As DataView
        Dim strFilter As String
        Dim intSMS, intMail, intRecCount As Int16
        Try
            strFilter = strMainFilter & " and User_ID=" & intUserid

            dvTemp = dvData
            dvTemp.RowFilter = strFilter
            If dvTemp.Count > 1 Then
                'dt = HttpContext.Current.Cache("PListDesc")
                intRecCount = dvTemp.Count
                'strFilterTemp = strFilter
                dt = dtPrority
                '                dvTemp = dvData
                For k = 0 To dt.Rows.Count - 1
                    Dim str As String
                    Try
                        Select Case dt.Rows(k).Item(0)
                            Case 1
                                'Event_Fired_ID_FK
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "=" & intFiredEvent
                            Case 2
                                'User_ID
                            Case 3
                                'Role_ID
                            Case 4
                                'Company_ID
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "=" & intCompanyID
                            Case 5
                                'Priority
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strPriority & "'"
                            Case 6
                                'Task_Type
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strTaskType & "'"
                            Case 7
                                'Call_Type
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strCallType & "'"
                            Case 8
                                'Project_ Name
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "=" & intProjectID
                            Case 9
                                'Status_Change_on_off
                            Case 10
                                'Task_Status
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strTaskStatus & "'"
                            Case 11
                                'Call_Status
                                dvTemp.RowFilter = strFilter & " and " & dt.Rows(k).Item(1) & "='" & strCallStatus & "'"
                            Case 12
                                'Start_Date
                            Case 13
                                'Stop_Date
                        End Select
                    Catch ex As Exception
                        CreateLog("ComSetup", "getMainUsers-1799", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

                    End Try
                    If dvTemp.Count > 1 Then
                        If dvTemp.Count < intRecCount Then
                            'strFilterTemp = dvTemp.RowFilter
                            intRecCount = dvTemp.Count
                            strFilter = dvTemp.RowFilter
                        End If
                    End If
                    If dvTemp.Count = 1 Then
                        intSMS = dvTemp.Item(0).Item("SMS_on_off")
                        intMail = dvTemp.Item(0).Item("Mail_on_off")
                        Exit For
                    End If
                    If k = dt.Rows.Count - 1 Then
                        dvTemp.RowFilter = strFilter
                        intSMS = dvTemp.Item(dvTemp.Count - 1).Item("SMS_on_off")
                        intMail = dvTemp.Item(dvTemp.Count - 1).Item("Mail_on_off")
                    End If
                Next
                If IsNothing(htUsers(intUserid)) Then
                    htUsers.Add(intUserid, (intSMS & "," & intMail & "#" & "O"))
                End If
                If IsNothing(htUsers(intUserid)) Then
                    htUsers.Add(intUserid, (intSMS & "," & intMail & "#" & "O"))
                End If
            ElseIf dvTemp.Count = 1 Then
                If IsNothing(htUsers(intUserid)) Then
                    htUsers.Add(intUserid, (dvTemp.Item(0).Item("SMS_on_off") & "," & dvTemp.Item(0).Item("Mail_on_off") & "#" & "O"))
                End If
                If IsNothing(htUsers(intUserid)) Then
                    htUsers.Add(intUserid, (dvTemp.Item(0).Item("SMS_on_off") & "," & dvTemp.Item(0).Item("Mail_on_off") & "#" & "O"))
                End If
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "getMainUsers-1388", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function

    Private Function CheckConditionRoles(ByVal intRole As Int32, ByVal intSMS1 As Int16, ByVal intMail1 As Int16) As Boolean
        Dim dvTemp As DataView
        Dim DT As DataTable
        Dim dtRules As DataTable
        Dim strDateRange, strValue As String
        Dim i, j, k As Int16
        'dtRules = HttpContext.Current.Cache("CommSetup")
        dtRules = dtMain
        Dim dvCondition As DataView
        Dim strFilter As String
        Dim intSMS, intMail, intRecCount As Int16
        intSMS = intSMS1
        intMail = intMail1
        flag = 0
        Try
            strFilter = "event_id_fk = 9 and " & strInputString & " and (role_id=" & intRole & " or (user_id=0 and role_id=0 ))"


            strConditionFilter = strFilter & " and "

            'Filter records
            dvCondition = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)

            If dvCondition.Count > 1 Then
                'DT = HttpContext.Current.Cache("PListDesc")
                dvTemp = dvCondition
                intRecCount = dvTemp.Count
                ' strFilterTemp = strFilter
                DT = dtPrority

                For k = 0 To DT.Rows.Count - 1
                    Dim str As String
                    Try
                        Select Case DT.Rows(k).Item(0)
                            Case 1
                                'Event_Fired_ID_FK
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "=" & intFiredEvent
                            Case 2
                                'User_ID
                            Case 3
                                'Role_ID
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "=" & intRole
                            Case 4
                                'Company_ID
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "=" & intCompanyID
                            Case 5
                                'Priority
                                'Dim str1 As String = strConditionFilter & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                            Case 6
                                'Task_Type
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "='" & strTaskType & "'"
                            Case 7
                                'Call_Type
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "='" & strCallType & "'"
                            Case 8
                                'Project_ Name
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "=" & intProjectID
                            Case 9
                                'Status_Change_on_off
                            Case 10
                                'Task_Status
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "='" & strTaskStatus & "'"
                            Case 11
                                'Call_Status
                                dvTemp.RowFilter = strConditionFilter & DT.Rows(k).Item(1) & "='" & strCallStatus & "'"
                            Case 12
                                'Start_Date
                            Case 13
                                'Stop_Date
                        End Select
                    Catch ex As Exception
                        CreateLog("Send Mail", "checkConditionRoles-281", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                    End Try

                    If dvTemp.Count > 1 Then
                        If dvTemp.Count < intRecCount Then
                            strFilter = dvTemp.RowFilter
                            intRecCount = dvTemp.Count
                        End If
                    End If
                    If dvTemp.Count = 1 Then
                        intSMS = dvTemp.Item(0).Item("SMS_on_off")
                        intMail = dvTemp.Item(0).Item("Mail_on_off")
                        Exit For
                    End If
                    If k = DT.Rows.Count - 1 Then
                        dvTemp.RowFilter = strFilter
                        intSMS = dvTemp.Item(dvTemp.Count - 1).Item("SMS_on_off")
                        intMail = dvTemp.Item(dvTemp.Count - 1).Item("Mail_on_off")
                    End If
                Next
                GetRoleUsersCondition(intRole, intSMS, intMail, 1)
            ElseIf dvCondition.Count = 1 Then
                GetRoleUsersCondition(intRole, dvCondition.Item(0).Item("SMS_on_off"), dvCondition.Item(0).Item("Mail_on_off"), 1)
            Else
                GetRoleUsersCondition(intRole, intSMS, intMail, 1)
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "CheckConditionRoles-1918", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function
    Private Function GetRoleUsersCondition(ByVal RoleID As Int32, ByVal intSMS As Int16, ByVal intMail As Int16, ByVal intMode As Int16) As Boolean
        Dim DT As New DataTable
        Dim DA As SqlDataAdapter
        Dim QueryString As String
        Dim objCommand As SqlCommand
        Dim strFilter As String
        Dim dvData As DataView

        'Dim objCon As SqlConnection = New SqlConnection(strConn)
        Try
            'objCon.Open()
            QueryString = "Select RA_IN4_AB_ID_FK from T060022 where RA_VC4_Status_Code = 'ENB' " _
            & " and RA_IN4_Role_ID_FK =" & RoleID
            objCommand = New SqlCommand
            With objCommand
                .CommandText = QueryString
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(DT)

            Dim intoutSMS As Int32
            Dim intoutMail As Int32
            For i As Int16 = 0 To DT.Rows.Count - 1
                If intMode = 1 Then
                    If htUserRoleCondition.ContainsKey(DT.Rows(i).Item(0)) = False Then
                        If CheckConditionRoleUserCondition(DT.Rows(i).Item(0), intoutSMS, intoutMail) = True Then
                            htUserRoleCondition.Add(DT.Rows(i).Item(0), intoutSMS & "," & intoutMail & "#" & "R")
                        Else
                            htUserRoleCondition.Add(DT.Rows(i).Item(0), intSMS & "," & intMail & "#" & "R")
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            CreateLog("ComSetup", "GetRolesUsersCondition-1958", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        Finally
            'objCon.Close()
        End Try
    End Function
    'This function for checking the conditions of user which is beling to above defined role
    Private Function CheckConditionRoleUserCondition(ByVal intUser As Int32, ByRef intSMS As Int32, ByRef intMail As Int32) As Boolean
        Dim dvTemp As DataView
        Dim DT As DataTable
        Dim dtRules As DataTable
        'DT for priority list, if Records found more than 1
        'DT = HttpContext.Current.Cache("PListDesc")
        DT = dtPrority
        Dim strDateRange As String
        Dim i, j, k, intRecCount As Int16
        'dtRules = HttpContext.Current.Cache("CommSetup")
        dtRules = dtMain
        Dim dvCondition As DataView
        Dim strFilter As String
        Dim ReturnFlag As Boolean
        Try
            flag = 0
            intSMS = 0
            intMail = 0
            strFilter = "event_id_fk = 9 and " & strInputString & " and user_id=" & intUser


            'Filter records
            dvCondition = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)


            If dvCondition.Count > 1 Then
                dvTemp = dvCondition
                intRecCount = dvTemp.Count
                'strFilterTemp = strFilter
                ReturnFlag = True
                'DT = HttpContext.Current.Cache("PListDesc")
                DT = dtPrority

                For k = 0 To DT.Rows.Count - 1
                    Dim str As String
                    Try
                        Select Case DT.Rows(k).Item(0)
                            Case 1
                                'Event_Fired_ID_FK
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intFiredEvent
                            Case 2
                                'User_ID
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intUser
                            Case 3
                                'Role_ID
                            Case 4
                                'Company_ID
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intCompanyID
                            Case 5
                                'Priority
                                'Dim str1 As String = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                            Case 6
                                'Task_Type
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strTaskType & "'"
                            Case 7
                                'Call_Type
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strCallType & "'"
                            Case 8
                                'Project_ Name
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intProjectID
                            Case 9
                                'Status_Change_on_off
                            Case 10
                                'Task_Status
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strTaskStatus & "'"
                            Case 11
                                'Call_Status
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strCallStatus & "'"
                            Case 12
                                'Start_Date
                            Case 13
                                'Stop_Date
                        End Select
                    Catch ex As Exception
                        CreateLog("ComSetup", "ShowValues-2034", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")
                    End Try

                    If dvTemp.Count > 1 Then
                        If dvTemp.Count < intRecCount Then
                            strFilter = dvTemp.RowFilter
                            intRecCount = dvTemp.Count
                        End If
                    End If
                    If dvTemp.Count = 1 Then
                        intSMS = dvTemp.Item(0).Item("SMS_on_off")
                        intMail = dvTemp.Item(0).Item("Mail_on_off")
                        Exit For
                    End If
                    If k = DT.Rows.Count - 1 Then
                        dvTemp.RowFilter = strFilter
                        intSMS = dvTemp.Item(dvTemp.Count - 1).Item("SMS_on_off")
                        intMail = dvTemp.Item(dvTemp.Count - 1).Item("Mail_on_off")
                    End If
                Next
                ReturnFlag = True
            ElseIf dvCondition.Count = 1 Then
                ReturnFlag = True
                intSMS = dvCondition.Item(0).Item("SMS_on_off")
                intMail = dvCondition.Item(0).Item("Mail_on_off")
            End If
            If ReturnFlag = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "CheckConditionRoleUserCondition-2062", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function




    Private Function CheckConditionUser(ByVal intUser As Int32) As Boolean
        '    Dim e As EventLog


        Dim dvTemp As DataView
        Dim DT As DataTable
        Dim dtRules As DataTable
        'DT for priority list, if Records found more than 1
        'DT = HttpContext.Current.Cache("PListDesc")
        DT = dtPrority
        Dim strDateRange As String
        Dim i, j, k As Int16
        'dtRules = HttpContext.Current.Cache("CommSetup")
        dtRules = dtMain
        Dim dvCondition As DataView
        Dim strFilter As String
        Dim intSMS, intMail, intRecCount As Int16
        Try
            flag = 0
            strFilter = "event_id_fk = 9 and " & strInputString & " and (user_id=" & intUser & " or (user_id=0 and role_id=0 ))"

            'Filter records
            dvCondition = New DataView(dtRules, strFilter, "event_id_fk", DataViewRowState.CurrentRows)


            If dvCondition.Count > 1 Then
                'DT = HttpContext.Current.Cache("PListDesc")
                'strFilterTemp = strFilter
                DT = dtPrority
                dvTemp = dvCondition
                intRecCount = dvTemp.Count

                For k = 0 To DT.Rows.Count - 1
                    Dim str As String
                    Try
                        Select Case DT.Rows(k).Item(0)
                            Case 1
                                'Event_Fired_ID_FK
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intFiredEvent
                            Case 2
                                'User_ID
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intUser
                            Case 3
                                'Role_ID
                            Case 4
                                'Company_ID
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intCompanyID
                            Case 5
                                'Priority
                                'Dim str1 As String = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strPriority & "'"
                            Case 6
                                'Task_Type
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strTaskType & "'"
                            Case 7
                                'Call_Type
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strCallType & "'"
                            Case 8
                                'Project_ Name
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "=" & intProjectID
                            Case 9
                                'Status_Change_on_off
                            Case 10
                                'Task_Status
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strTaskStatus & "'"
                            Case 11
                                'Call_Status
                                dvTemp.RowFilter = strFilter & " and " & DT.Rows(k).Item(1) & "='" & strCallStatus & "'"
                            Case 12
                                'Start_Date
                            Case 13
                                'Stop_Date
                        End Select
                    Catch ex As Exception
                        CreateLog("ComSetup", "CheckConditionUser-2135", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

                    End Try
                    If dvTemp.Count > 1 Then
                        If dvTemp.Count < intRecCount Then
                            strFilter = dvTemp.RowFilter
                            intRecCount = dvTemp.Count
                        End If
                    End If
                    If dvTemp.Count = 1 Then
                        intSMS = dvTemp.Item(0).Item("SMS_on_off")
                        intMail = dvTemp.Item(0).Item("Mail_on_off")
                        Exit For
                    End If
                    If k = DT.Rows.Count - 1 Then
                        dvTemp.RowFilter = strFilter
                        intSMS = dvTemp.Item(dvTemp.Count - 1).Item("SMS_on_off")
                        intMail = dvTemp.Item(dvTemp.Count - 1).Item("Mail_on_off")
                    End If
                Next
                If htUserCondition.ContainsKey(intUser) = False Then
                    htUserCondition.Add(intUser, intSMS & "," & intMail & "#" & "O")
                End If
            ElseIf dvCondition.Count = 1 Then
                If htUserCondition.ContainsKey(intUser) = False Then
                    htUserCondition.Add(intUser, dvCondition.Item(0).Item("SMS_on_off") & "," & dvCondition.Item(0).Item("Mail_on_off") & "#" & "O")
                End If
            End If
        Catch ex As Exception
            CreateLog("ComSetup", "CheckConditionUser-2161", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "10", "abc", "NA")

        End Try
    End Function

End Class
