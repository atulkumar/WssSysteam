﻿Imports Microsoft.VisualBasic
Imports System.Data
Imports ION.Logging.EventLogging
Imports ION.Data
Imports ION.BusinessUnit

Namespace ION.DataLayer
    Public Class CommentViewDAL

        Public Function GetCommentsView(ByVal objCommentsViewBU As CommentViewBO) As DataSet
            Try
                Dim dsComments As New DataSet
                Dim strSQL As String
                'strSQL = "(select CM_DT8_Date, convert(varchar,CM_DT8_Date) CommentDate, replace( CM_VC1000_MailList,';','; ') MailList, CM_CH1_Flag ReadFlag, CM_VC2_Flag FLag, CM_NU9_CompId_Fk CompID, CM_VC256_Comments CommentDesc,case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, '' CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_CH1_Flag as CommentFlag, CM_NU9_CompId_Fk from T040061,T060011 A,T010011 where A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and CI_NU8_Address_Number=CM_NU9_CompId_Fk and  CM_CH1_Flag=" & Convert.ToInt16(objCommentsViewBU.blnCommentFlag) & " and CM_NU9_Call_Number in ( select  CM_NU9_Call_Number from T040011  where  CM_NU9_Comp_Id_FK =CM_NU9_CompId_Fk and CM_NU9_Project_ID in (select PM_NU9_Project_ID_Fk from T210012 where PM_NU9_Comp_ID_FK=CM_NU9_CompId_Fk and PM_NU9_Project_Member_ID=" & Val(objCommentsViewBU.intPropUserID) & "))  and CM_NU9_CompId_Fk IN(" & GetCompanySubQuery() & ") and CM_NU9_Comment_To is null) union all (select CM_DT8_Date, convert(varchar,CM_DT8_Date) CommentDate, replace( CM_VC1000_MailList,';','; ') MailList, CM_CH1_Flag ReadFlag, CM_VC2_Flag FLag, CM_NU9_CompId_Fk CompID, CM_VC256_Comments CommentDesc,case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, B.UM_VC50_UserID CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_CH1_Flag as CommentFlag, CM_NU9_CompId_Fk from T040061,T060011 A,T010011 ,T060011 B where B.UM_IN4_Address_No_FK=CM_NU9_Comment_To and A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and CI_NU8_Address_Number=CM_NU9_CompId_Fk and  CM_CH1_Flag=" & Convert.ToInt16(objCommentsViewBU.blnCommentFlag) & "  and CM_NU9_Comment_To=" & Val(objCommentsViewBU.intPropUserID) & "  and CM_NU9_CompId_Fk IN(" & GetCompanySubQuery() & ")) order by CM_DT8_Date DESC"
                strSQL = "(select CM_DT8_Date, convert(varchar,CM_DT8_Date) CommentDate, replace( CM_VC1000_MailList,';','; ') MailList, CM_CH1_Flag ReadFlag, CM_VC2_Flag FLag, CM_NU9_CompId_Fk CompID, CM_VC256_Comments CommentDesc,case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, '' CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_CH1_Flag as CommentFlag, CM_NU9_CompId_Fk from T040061,T060011 A,T010011 where A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and CI_NU8_Address_Number=CM_NU9_CompId_Fk  and CM_NU9_Call_Number in ( select  CM_NU9_Call_Number from T040011  where  CM_NU9_Comp_Id_FK =CM_NU9_CompId_Fk and CM_NU9_Project_ID in (select PM_NU9_Project_ID_Fk from T210012 where PM_NU9_Comp_ID_FK=CM_NU9_CompId_Fk and PM_NU9_Project_Member_ID=" & Val(objCommentsViewBU.intPropUserID) & "))  and CM_NU9_CompId_Fk IN(" & GetCompanySubQuery() & ") and CM_NU9_Comment_To is null) union all (select CM_DT8_Date, convert(varchar,CM_DT8_Date) CommentDate, replace( CM_VC1000_MailList,';','; ') MailList, CM_CH1_Flag ReadFlag, CM_VC2_Flag FLag, CM_NU9_CompId_Fk CompID, CM_VC256_Comments CommentDesc,case CM_VC2_Flag when 'C' then 'Call Level' when 'T' then 'Task Level' when 'A' then 'Action Level' end CommentLevel, A.UM_VC50_UserID CommentBy, B.UM_VC50_UserID CommentTo, CI_VC36_Name Company, CM_NU9_Call_Number CallNo, CM_NU9_Task_Number TaskNo, CM_NU9_Action_Number ActionNo,CM_VC2_Flag,CM_CH1_Flag as CommentFlag, CM_NU9_CompId_Fk from T040061,T060011 A,T010011 ,T060011 B where B.UM_IN4_Address_No_FK=CM_NU9_Comment_To and A.UM_IN4_Address_No_FK=CM_NU9_AB_Number and CI_NU8_Address_Number=CM_NU9_CompId_Fk  and CM_NU9_CompId_Fk IN(" & GetCompanySubQuery() & ")) order by CM_DT8_Date DESC"
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                If SQL.Search("CommentAlert", "", "", strSQL, dsComments, "", "") = True Then
                    Return dsComments
                End If
            Catch ex As Exception
            End Try
        End Function
    End Class
End Namespace