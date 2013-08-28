Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.Data


Public Class clsNextNo

	Public Shared Function GetNextNo(ByVal code As Int16, ByVal strBCode As String, ByVal strCon As String) As Long
		Dim intNextNum As Integer
		Dim strQuery As String
		Dim cmdCommand As New SqlCommand
		Dim objTrn As SqlTransaction
		Dim obj As Object
		Dim objConn As SqlConnection

		Try
			strQuery = "Select NXT_Next_No from TSL9901 where NXT_Code_No=" & code & " and" _
			& " NXT_CH4_Branch_Code_FK='" & strBCode.Trim & "'"
			objConn = New SqlConnection(strCon)
			objConn.Open()
            objTrn = objConn.BeginTransaction(IsolationLevel.RepeatableRead)

			cmdCommand.Transaction = objTrn
			With cmdCommand
				.CommandText = strQuery
				.CommandType = CommandType.Text
				.Connection = objConn
				obj = .ExecuteScalar()
			End With
			cmdCommand.CommandText = "Update TSL9901 set NXT_Next_No=NXT_Next_No+1 where NXT_Code_No=" & code & " and" _
			& " NXT_CH4_Branch_Code_FK='" & strBCode.Trim & "'"
			cmdCommand.ExecuteNonQuery()
			If (Not obj Is Nothing) Then
				intNextNum = Convert.ToInt32(obj)
			End If
			objTrn.Commit()
		Catch ex As Exception
			objTrn.Rollback()
			CreateLog("clsNextNo", "GetNextNo-36", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
			Return 0
		Finally
			objConn.Close()
		End Try
		Return intNextNum
	End Function

End Class
