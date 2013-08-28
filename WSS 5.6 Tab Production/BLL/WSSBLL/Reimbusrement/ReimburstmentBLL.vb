#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [22/06/09]
' PURPOSE   : [This Class is used Get the Function from Reimbursement DAL]
' TABLES    : [Emp_Reimbursement_BillDetail,Emp_Reimbursement_Detail,Bill History,BillsubmissionDate,                            Reimbursement_Type]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports WSSDAL
Imports ION.Logging.EventLogging
#End Region

Public Class ReimburstmentBLL
    Private mobjReimburstmentDAL As ReimburstmentDAL

    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        mobjReimburstmentDAL = New ReimburstmentDAL(ConnectionString, Provider)
    End Sub

#Region "FUNCTIONS"
#Region "Common Function In Reimburstment Module"

#Region "GetReimburstmentType"
    ''' <summary>
    ''' Function to fill Reimburstment Type  according to EMPID
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReimburstmentType(ByVal EmpID As Integer, ByVal FinacialYear As String) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetReimburstmentType(EmpID, FinacialYear)
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetReimburstmentType-40", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetReimburstmentName"
    ''' <summary>
    ''' Function to fill Reimburstment Type Combo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReimburstmentName() As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetReimburstmentName()
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetReimburstmentName-59", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetEmployeesName"
    ''' <summary>
    ''' Function To Get Name of Employee To Fill the Employee DropDown
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmployeesName() As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetEmployeesName()
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmployeesName-77", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region
#End Region

#Region "sav_rec"
    ''' <summary>
    '''  Function To save Bill submission Information
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function sav_rec(ByVal arvalues As ArrayList) As Boolean
        Try
            If mobjReimburstmentDAL.sav_rec(arvalues) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "sav_rec-100", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function

#End Region

#Region "GetEmpBillSubmitted"
    ''' <summary>
    ''' This Function is used to get the Detail of Bill submitted by User
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <param name="Month"></param>
    ''' <param name="FinancialYear"></param>
    ''' <param name="ApprovedFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpBillSubmitted(ByVal EmpID As Integer, ByVal Month As Int32, ByVal FinancialYear As String, ByVal ApprovedFlag As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetEmpBillSubmitted(EmpID, Month, FinancialYear, ApprovedFlag)
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpBillSubmitted-123", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetRBMPaid"
    ''' <summary> 
    ''' This Function is used to get the Detail of Reimburstment Paid To particular Employee 
    ''' </summary> 
    ''' <param name="arvalues">Passing Empcode,Month ,Year,RBMType</param> 
    ''' <returns></returns> 
    Public Function GetRBMPaid(ByVal arvalues As ArrayList) As DataTable
        Try
            Dim dtRBMPaid As New DataTable()
            dtRBMPaid = mobjReimburstmentDAL.GetRBMPaid(arvalues)
            Return dtRBMPaid
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetRBMPaid-141", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetInsertedBill"
    ''' <summary>
    ''' Function to get  sum of Bill Amount which has been submitted
    ''' </summary>
    ''' <param name="EmployeeID"></param>
    ''' <param name="Year"></param>
    ''' <param name="RBM_Type"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInsertedBill(ByVal EmployeeID As Integer, ByVal Year As String, ByVal RBM_Type As Int32) As Integer
        Dim SubmittedAmt As Integer
        Try
            SubmittedAmt = mobjReimburstmentDAL.GetInsertedBill(EmployeeID, Year, RBM_Type)
            Return SubmittedAmt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetInsertedBill-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return SubmittedAmt
        End Try
    End Function
#End Region

#Region "GetAmtRBMAllowed"
    ''' <summary>
    ''' Function to check Allowed Reimbursement Amount for particular User
    ''' </summary>
    ''' <param name="EmployeeID"></param>
    ''' <param name="RBM_Type"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAmtRBMAllowed(ByVal EmployeeID As Integer, ByVal RBM_Type As Integer) As Integer

        Dim RBM_AmtAllowed As Integer
        Try
            RBM_AmtAllowed = mobjReimburstmentDAL.GetAmtRBMAllowed(EmployeeID, RBM_Type)
            Return RBM_AmtAllowed
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetAmtRBMAllowed-183", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return RBM_AmtAllowed
        End Try
    End Function
#End Region

#Region "SaveBillVerified"
    ''' <summary>
    ''' This Function is used to save the Verified Fills
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveBillVerified(ByVal arvalues As ArrayList) As Boolean
        Try
            If mobjReimburstmentDAL.SaveVeriFiedBillSubmitted(arvalues) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SaveBillVerified-204", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "GetNameEmployees"
    ''' <summary>
    ''' Function to get name of all employee's 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNameEmployees(ByVal FinacialYear As String) As DataTable
        Try
            Dim dtEmpName As New DataTable()
            dtEmpName = mobjReimburstmentDAL.GetNameEmployees(FinacialYear)
            Return dtEmpName
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetNameEmployees-222", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetEmpYearlyBillDetail"
    ''' <summary>
    ''' This Function is used to Fill the Employee Yearly Bill Detail
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <param name="Month"></param>
    ''' <param name="BillYear"></param>
    ''' <param name="FinancialYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpYearlyBillDetail(ByVal EmpID As Integer, ByVal Month As Integer, ByVal BillYear As Int32, ByVal FinancialYear As String) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetEmpYearlyBillDetail(EmpID, Month, BillYear, FinancialYear)
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpYearlyBillDetail-244", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetEmpMonthlyBillDetails"
    ''' <summary>
    ''' This Function is used to Fill the Employee Monthly Bill Detail
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <param name="Month"></param>
    ''' <param name="BillYear"></param>
    ''' <param name="FinancialYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpMonthlyBillDetail(ByVal EmpID As Integer, ByVal Month As Integer, ByVal BillYear As Int32, ByVal FinancialYear As String) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetEmpMonthlyBillDetails(EmpID, Month, BillYear, FinancialYear)
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpMonthlyBillDetail-266", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetBillSubDateDetail"
    ''' <summary>
    ''' This Function is used to get the Bill submission Detail
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function GetBillSubDateDetail() As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetBillDateSubmission()
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetBillSubDateDetail-285", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "SaveInto_Emp_Reimbursement_Detail"
    ''' <summary>
    ''' This Function is used To save the records in Emp_Reimbursement_Detail where user set the RBM for user
    ''' </summary>
    ''' <param name="RBM_ID"></param>
    ''' <param name="Emp_Id"></param>
    ''' <param name="arrYearly"></param>
    ''' <param name="arrMonthly"></param>
    ''' <param name="strFinancialYear"></param>
    ''' <param name="BillYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveInto_Emp_Reimbursement_Detail(ByVal RBM_ID As Integer, ByVal Emp_Id As String, ByVal arrYearly As String, ByVal arrMonthly As String, ByVal strFinancialYear As String, ByVal BillYear As Integer, ByVal JoinMonth As Integer) As Boolean
        Try
            If mobjReimburstmentDAL.SaveInto_Emp_Reimbursement_Detail(RBM_ID, Emp_Id, arrYearly, arrMonthly, strFinancialYear, BillYear, JoinMonth) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SaveInto_Emp_Reimbursement_Detail-311", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "InsertDefaultValues"
    ''' <summary>
    ''' This function is used to insert the Defaut values in Bill detail Table of employess for which we set the Reimbrusement(like medical amount,Telephone etc)
    ''' </summary>
    ''' <param name="intFromMonth"></param>
    ''' <param name="intToMonth"></param>
    ''' <param name="intFromYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertDefaultValues(ByVal intFromMonth As Integer, ByVal intToMonth As Integer, ByVal intFromYear As Integer, ByVal intFlag As Integer, ByVal FinacialYear As String, ByVal EmpID As Integer) As Boolean
        Try
            If mobjReimburstmentDAL.InsertDefaultValues(intFromMonth, intToMonth, intFromYear, intFlag, FinacialYear, EmpID) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "InsertDefaultValues-334", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "GetFinacialYear"
    ''' <summary>
    ''' This Function is used to get the Finanical Year
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function GetFinacialYear() As String
        Dim FinacialYear As String = String.Empty
        Try
            FinacialYear = mobjReimburstmentDAL.GetFinacialYear()
            Return FinacialYear
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetFinacialYear-353", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return FinacialYear
        End Try
    End Function

#End Region

#Region "DeleteRBMAmt"
    ''' <summary>
    ''' This function is used to delete the records from  Emp_Reimbursement_Detail before Update
    ''' </summary>
    ''' <param name="Emp_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function DeleteRBMAmt(ByVal Emp_ID As Integer, ByVal FinacialYear As String) As Boolean
        Try
            If mobjReimburstmentDAL.DeleteRBMAmt(Emp_ID, FinacialYear) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "DeleteRBMAmt-376", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "InsertBillSubmissionDate"
    ''' <summary>
    ''' This Function is used to insert the Bill Submission Date Detail
    ''' </summary>
    ''' <param name="intSubmissionID"></param>
    ''' <param name="intStartDate"></param>
    ''' <param name="intEndDate"></param>
    ''' <param name="strFinancialYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertBillSubmissionDate(ByVal intSubmissionID As Integer, ByVal intStartDate As Integer, ByVal intEndDate As Integer, ByVal strFinancialYear As String) As Boolean
        Try
            If mobjReimburstmentDAL.InsertBillSubmissionDate(intSubmissionID, intStartDate, intEndDate, strFinancialYear) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "InsertBillSubmissionDate-400", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "GetRBMDetail"
    ''' <summary>
    ''' This function is used to get the Reimbrusement Detail according to the filter 
    ''' </summary>
    ''' <param name="Finanicalyear"></param>
    ''' <param name="Month"></param>
    ''' <param name="strWhere"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRBMDetail(ByVal Finanicalyear As String, ByVal Month As Integer, ByVal strWhere As String) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetRBMDetail(Finanicalyear, Month, strWhere)
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetRBMDetail-421", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "DeleteBillSubmissionDate"
    ''' <summary>
    ''' This Function is used to delete the record before Updation
    ''' </summary>
    ''' <param name="Submission_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteBillSubmissionDate(ByVal Submission_ID As Integer)
        Try
            If mobjReimburstmentDAL.DeleteBillSubmissionDate(Submission_ID) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "DeleteBillSubmissionDate-442", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "AdjustBills"
    ''' <summary>
    ''' This function is used to adust the Advance or Bill Bue in next Month after verification
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function AdjustBills(ByVal arvalues As ArrayList) As Boolean
        Try
            If mobjReimburstmentDAL.AdjustBills(arvalues) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "AdjustBills-464", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "DeleteBillSubmissionDate"
    ''' <summary>
    ''' This Function is used to delete the record before Updation
    ''' </summary>
    ''' <param name="BillID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteBill(ByVal BillID As Integer)
        Try
            If mobjReimburstmentDAL.DeleteBill(BillID) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "DeleteBill-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "UpdateBillSubmissionDate"
    ''' <summary>
    ''' This Function is used to update the record 
    ''' </summary>
    ''' <param name="BillID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBill(ByVal BillID As Integer, ByVal BILLSUBMITTED As String, ByVal fileName As String, ByVal filePath As String) As Boolean
        Try
            If mobjReimburstmentDAL.UpdateBill(BillID, BILLSUBMITTED, fileName, filePath) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "UpdateBill", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "SearchExistingBills"
    ''' <summary>
    ''' This Function is used to Search the Bills 
    ''' </summary>
    ''' <param name="BillID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchExistingBills(ByVal BillID As Integer) As DataSet
        Dim dsNew As New DataSet
        Try
            dsNew = mobjReimburstmentDAL.SearchExistingBills(BillID)
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SearchExistingBills", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
        Return dsNew
    End Function
#End Region

#End Region
#Region "GetRBMSummary"

    ''' <summary>
    ''' This function is used to get the Reimbrusement Detail according to the filter 
    ''' </summary>
    ''' <param name="Finanicalyear"></param>
    ''' <param name="Month"></param>
    ''' <param name="strWhere"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function GetRBMSummary(ByVal Finanicalyear As String, ByVal Month As Integer, ByVal strWhere As String) As DataTable

        Try

            Dim dt As New DataTable()

            dt = mobjReimburstmentDAL.GetRBMSummary(Finanicalyear, Month, strWhere)

            Return dt

        Catch ex As Exception

            CreateLog("ReimburstmentDAL", "GetRBMDetail-421", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

            Return Nothing

        End Try

    End Function

#End Region

#Region "GetEmpDisapprovedBill"
    ''' <summary>
    ''' This Function is used to get the Details of Disaaproved Bills
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <param name="Month"></param>
    ''' <param name="FinancialYear"></param>
    ''' <param name="DisapprovedFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpDisapprovedBill(ByVal EmpID As Integer, ByVal Month As Int32, ByVal FinancialYear As String, ByVal DisapprovedFlag As Integer) As DataTable
        Try
            Dim dt As New DataTable()
            dt = mobjReimburstmentDAL.GetEmpDisapprovedBill(EmpID, Month, FinancialYear, DisapprovedFlag)
            Return dt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpDisapprovedBill-446", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "SetDisapprovedBillsStatus"
    ''' <summary>
    ''' This function is used to set the status for Disapproved the bills 
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function SetDisapprovedBillsStatus(ByVal arValues As ArrayList) As Boolean
        Try
            If mobjReimburstmentDAL.SetDisapprovedBillsStatus(arValues) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SetDisapprovedBillsStatus-604", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

End Class