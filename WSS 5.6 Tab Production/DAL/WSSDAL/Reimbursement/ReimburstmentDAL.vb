#Region "Purpose"
' "CREATION"
' CREATED BY: [Mandeep]
' CREATED ON: [22/06/09]
' PURPOSE   : [This Class is used Get the information related to Reimbrusement Module like Getdata,update,insert                and delet]
' TABLES    : [Emp_Reimbursement_BillDetail,Emp_Reimbursement_Detail,Bill History,BillsubmissionDate,                            Reimbursement_Type]
' "Updation [ ]"
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports ION.Common.DAL
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
#End Region

Public Class ReimburstmentDAL
    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        clsData.ConnectionString = ConnectionString
        clsData.DBProvider = Provider
    End Sub

#Region "Functions"
#Region "GetReimburstmentType"
    ''' <summary>
    ''' Function to fill Reimburstment Type  according to EMPID
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReimburstmentType(ByVal EmpID As Integer, ByVal FinacialYear As String) As DataTable
        Try
            Dim dtRBM As New DataTable
            dtRBM = clsData.Search("select dbo.udf_GetReimbursement(RBM_ID) RBMType,RBM_ID from Emp_Reimbursement_Detail where EMP_ID=" & EmpID & " and RBM_Yearly<>0 and RBM_FinanicalYear='" & FinacialYear & "'", "Emp_Reimbursement_Detail")
            Return dtRBM
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetReimburstmentType-39", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            Dim dtRBM As New DataTable
            dtRBM = clsData.Search("select RBM_Type as RBMType,RBM_ID_PK As RBMID from Reimbursement_Type", "Reimbursement_Type")
            Return dtRBM
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetReimburstmentName-57", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            Dim dtEmpName As New DataTable
            dtEmpName = clsData.Search("select distinct(EMPName.CI_VC36_name),Emp_ID from T010011 EMPName ,Emp_Reimbursement_Detail ERD where EMPName.CI_NU8_Address_Number=ERD.Emp_ID", "Emp_Reimbursement_Detail")
            Return dtEmpName
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmployeesName-75", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetFinacialYear"
    ''' <summary>
    ''' This Function is used to get the Finanical Year
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFinacialYear() As String
        Dim FinacialYear As String = String.Empty
        Try
            FinacialYear = clsData.SearchSingleValue("Select BillSubmissionFinacialYear from BillSubmissionDate")
            Return FinacialYear
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetFinacialYear-93", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return FinacialYear
        End Try
    End Function
#End Region

#Region "sav_rec"
    ''' <summary>
    '''  Function To save Bill submission Information
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function sav_rec(ByVal arValues As ArrayList) As Boolean
        Try
            Dim cmd As New SqlCommand()
            cmd.Parameters.AddWithValue("@RBMType", arValues(0))
            cmd.Parameters.AddWithValue("@Amount", arValues(1))
            cmd.Parameters.AddWithValue("@Month", arValues(2))
            cmd.Parameters.AddWithValue("@year", arValues(3))
            cmd.Parameters.AddWithValue("@RBMDate", arValues(4))
            cmd.Parameters.AddWithValue("@EmployeeID", arValues(5))
            cmd.Parameters.AddWithValue("@Financial_Year", arValues(6))
            cmd.Parameters.AddWithValue("@AttachFileName", arValues(7))
            cmd.Parameters.AddWithValue("@BillFileName", arValues(8))
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_SaveBills"

            If clsData.ExecuteProcedure(cmd) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "sav_rec-125", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
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
    Public Function GetInsertedBill(ByVal EmployeeID As Integer, ByVal Year As String, ByVal RBM_Type As Integer) As Integer
        Try
            Dim SubmittedAmt As Integer
            SubmittedAmt = clsData.SearchSingleValue("Select sum(isnull(BillSubmitted,0)) from Emp_Reimbursement_BillDetail where Emp_ID=" & EmployeeID & " and RBM_ID_FK=" & RBM_Type & " and FinnacialYear='" & Year & "'")
            Return SubmittedAmt
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetInsertedBill-147", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return 0
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
        Try
            Dim RBM_AmtAllowed As Integer
            RBM_AmtAllowed = clsData.SearchSingleValue("Select RBM_Yearly from Emp_Reimbursement_Detail where Emp_ID = " & EmployeeID & " and RBM_ID=" & RBM_Type)
            Return RBM_AmtAllowed
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetAmtRBMAllowed-167", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return 0
        End Try
    End Function
#End Region

#Region "GetBillDateSubmission"
    ''' <summary>
    ''' This Function is used to get the Bill submission Detail
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBillDateSubmission() As DataTable
        Try
            Dim dtBillSubDate As New DataTable
            dtBillSubDate = clsData.Search("Select isnull(BillSubmissionStartDate,0) as StartDate,isnull(BillSubmissionEndDate,0) as EndDate ,isnull(BillSubmissionFinacialYear,0) as FinacialYear From BillSubmissionDate", "BillSubmissionDate")
            Return dtBillSubDate
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetBillDateSubmission-185", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
    ''' <param name="FinanicalYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpYearlyBillDetail(ByVal EmpID As Integer, ByVal Month As Integer, ByVal BillYear As Integer, ByVal FinanicalYear As String) As DataTable
        Try
            Dim dtBillSubmitted As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_GetEmployeeBillDetail"
            cmd.Parameters.AddWithValue("@BillType", 1)
            cmd.Parameters.AddWithValue("@EmpID", EmpID)
            cmd.Parameters.AddWithValue("@Month", Month)
            cmd.Parameters.AddWithValue("@BillYear", BillYear)
            cmd.Parameters.AddWithValue("@Finanical_Year", FinanicalYear)
            dtBillSubmitted = clsData.Search(cmd)
            Return dtBillSubmitted
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpYearlyBillDetail-215", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
    ''' <param name="FinanicalYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpMonthlyBillDetails(ByVal EmpID As Integer, ByVal Month As Integer, ByVal BillYear As Integer, ByVal FinanicalYear As String) As DataTable
        Try
            Dim dtMonthlyBillSubmitted As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_GetEmployeeBillDetail"
            cmd.Parameters.AddWithValue("@BillType", 2)
            cmd.Parameters.AddWithValue("@EmpID", EmpID)
            cmd.Parameters.AddWithValue("@Month", Month)
            cmd.Parameters.AddWithValue("@BillYear", BillYear)
            cmd.Parameters.AddWithValue("@Finanical_Year", FinanicalYear)
            dtMonthlyBillSubmitted = clsData.Search(cmd)
            Return dtMonthlyBillSubmitted
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpMonthlyBillDetails-245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "GetEmpBillSubmitted"
    ''' <summary>
    ''' This Function is used to get the Detail of Bill submitted by User
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <param name="Month"></param>
    ''' <param name="FinanicalYear"></param>
    ''' <param name="ApprovedFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpBillSubmitted(ByVal EmpID As Integer, ByVal Month As Integer, ByVal FinanicalYear As String, ByVal ApprovedFlag As Integer) As DataTable
        Try
            Dim dtBillSubmitted As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_EmpBillSubmitted"
            cmd.Parameters.AddWithValue("@EMPID", EmpID)
            cmd.Parameters.AddWithValue("@Month", Month)
            cmd.Parameters.AddWithValue("@Financial_Year", FinanicalYear)
            cmd.Parameters.AddWithValue("@VeriFiedFlag", ApprovedFlag)
            dtBillSubmitted = clsData.Search(cmd)
            Return dtBillSubmitted
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpBillSubmitted-273", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "SaveVeriFiedBillSubmitted"
    ''' <summary>
    ''' This Function is used to save the Verified Fills
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveVeriFiedBillSubmitted(ByVal arValues As ArrayList) As Boolean
        Try
            Dim cmd As New SqlCommand()
            cmd.Parameters.AddWithValue("@Amount", arValues(0))
            cmd.Parameters.AddWithValue("@EmpID", arValues(1))
            cmd.Parameters.AddWithValue("@RBMType", arValues(2))
            cmd.Parameters.AddWithValue("@Month", arValues(3))
            cmd.Parameters.AddWithValue("@year", arValues(4))
            cmd.Parameters.AddWithValue("@BillID", arValues(5))
            cmd.Parameters.AddWithValue("@RBMDate", arValues(6))
            cmd.Parameters.AddWithValue("@Finanical_Year", arValues(7))
            cmd.Parameters.AddWithValue("@CurrentMonth", arValues(8))
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_UpdateRBMBills"

            If clsData.ExecuteProcedure(cmd) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SaveVeriFiedBillSubmitted-307", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function

#End Region

#Region "GetRBMPaid"
    ''' <summary>
    ''' This Function is used to get the Detail of Reimburstment Paid To particular Employee
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRBMPaid(ByVal arValues As ArrayList) As DataTable
        Try
            Dim dtRBMPaid As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_GetBills"
            cmd.Parameters.AddWithValue("@Month", arValues(0))
            cmd.Parameters.AddWithValue("@year", arValues(1))
            cmd.Parameters.AddWithValue("@RBM_Type", arValues(2))
            cmd.Parameters.AddWithValue("@UpdatedDate", arValues(3))
            cmd.Parameters.AddWithValue("@EmpID", arValues(4))
            cmd.Parameters.AddWithValue("@Financial_Year", arValues(5))
            dtRBMPaid = clsData.Search(cmd)
            Return dtRBMPaid
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetRBMPaid-336", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
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
            Dim dtRBMRecord As New DataTable
            dtRBMRecord = clsData.Search("select EMP.CI_VC36_Name,EMP.CI_NU8_Address_Number,(select ERD.RBM_Yearly from Emp_Reimbursement_Detail ERD where ERD.EMP_ID=EMP.CI_NU8_ADDRESS_NUMBER and rbm_id=1  and RBM_FinanicalYear='" & FinacialYear & "'  )as 'Medical',(select ERD.RBM_Yearly from Emp_Reimbursement_Detail ERD where ERD.EMP_ID=EMP.CI_NU8_ADDRESS_NUMBER and rbm_id=2 and RBM_FinanicalYear='" & FinacialYear & "' )as 'Telephone',(select ERD.RBM_Yearly from Emp_Reimbursement_Detail ERD where ERD.EMP_ID=EMP.CI_NU8_ADDRESS_NUMBER and rbm_id=3 and RBM_FinanicalYear='" & FinacialYear & "' )as 'LTA',(select ERD.JoinMonth from Emp_Reimbursement_Detail ERD where ERD.EMP_ID=EMP.CI_NU8_ADDRESS_NUMBER and rbm_id=3 and RBM_FinanicalYear='" & FinacialYear & "' ) 'JoinMonth' from T010011 EMP where EMP.CI_VC8_Status='ENA' and EMP.CI_VC8_Address_Book_Type='EM' and EMP.CI_IN4_Business_Relation in(select CI_IN4_Business_Relation   from t010011 where  CI_NU8_Address_number= EMP.CI_NU8_Address_Number) order by CI_VC36_Name asc ", "T010011")
            Return dtRBMRecord
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetNameEmployees-355", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
    Public Function SaveInto_Emp_Reimbursement_Detail(ByVal RBM_ID As Integer, ByVal Emp_Id As Integer, ByVal arrYearly As String, ByVal arrMonthly As String, ByVal strFinancialYear As String, ByVal BillYear As Integer, ByVal JoinMonth As Integer) As Boolean
        Try
            If clsData.Save("insert into Emp_Reimbursement_Detail(RBM_id,emp_id,rbm_yearly,RBM_MonthlyEntitlement,RBM_FinanicalYear,BillYear,JoinMonth) values('" & RBM_ID & "'," & Emp_Id & ",'" & arrYearly & "','" & arrMonthly & "','" & strFinancialYear & "'," & BillYear & "," & JoinMonth & " )", True) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SaveInto_Emp_Reimbursement_Detail-381", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            If clsData.Save("insert into BillSubmissionDate (BillSubmission_ID,BillSubmissionStartDate,BillSubmissionEndDate,BillSubmissionFinacialYear)values(" & intSubmissionID & "," & intStartDate & "," & intEndDate & ",'" & strFinancialYear & "')", True) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "InsertBillSubmissionDate-405", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            Dim cmd As New SqlCommand()
            cmd.Parameters.AddWithValue("@FromMonth", intFromMonth)
            cmd.Parameters.AddWithValue("@ToMonth", intToMonth)
            cmd.Parameters.AddWithValue("@FromYear", intFromYear)
            cmd.Parameters.AddWithValue("@Flag", intFlag)
            cmd.Parameters.AddWithValue("@FinacialYear", FinacialYear)
            cmd.Parameters.AddWithValue("@Empid", EmpID)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_InsertDefaultData"
            If clsData.ExecuteProcedure(cmd) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "InsertDefaultValues-434", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            Dim dtGetBills As New DataTable
            Dim strSql As String
            Dim strSQLQ As String
            If Month > 0 Then
                strSql = "Select (EmpName.CI_VC36_name) as EMPName,DATENAME(MONTH,dateadd(month,RBM_Month-1,0)) As MName,dbo.udf_GetReimbursement(RBM_ID_FK) as Reimbursement,(select EMRD.RBM_MonthlyEntitlement from Emp_Reimbursement_Detail EMRD where EMRD.Emp_ID=RBMBD.Emp_ID  and EMRD.RBM_ID=RBMBD.RBM_ID_FK and RBM_FinanicalYear='" & Finanicalyear & "' ) as 'ReimbursementAllowed',isnull(BillSubmitted,0) 'BillSubmitted',isnull(RBM_ToBePaid,0) 'ReimbursementPaid',isnull(Bill_InAdvance,0) 'Bill_InAdvance',isnull(Bill_Due,0) 'Bill_Due',RBM_Year 'Year',Emp_ID 'EMPID', Emp_RBM_BillDetail_ID 'BillDetail_ID' from T010011 EmpName ,Emp_Reimbursement_BillDetail RBMBD where EmpName.CI_NU8_Address_Number=RBMBD.emp_id and  FinnacialYear='" & Finanicalyear & "' and  RBM_Month=" & Month
                strSql += strWhere
                strSql += "order by Emp_RBM_BillDetail_ID  "
            Else
                strSql = "(Select (EmpName.CI_VC36_name) as EMPName,DATENAME(MONTH,dateadd(month,RBM_Month-1,0)) As MName,RBM_Month,dbo.udf_GetReimbursement(RBM_ID_FK) as Reimbursement,(select EMRD.RBM_MonthlyEntitlement from Emp_Reimbursement_Detail EMRD where EMRD.Emp_ID=RBMBD.Emp_ID  and EMRD.RBM_ID=RBMBD.RBM_ID_FK and RBM_FinanicalYear='" & Finanicalyear & "' ) as 'ReimbursementAllowed',isnull(BillSubmitted,0) 'BillSubmitted',isnull(RBM_ToBePaid,0) 'ReimbursementPaid',isnull(Bill_InAdvance,0) 'Bill_InAdvance',isnull(Bill_Due,0) 'Bill_Due',RBM_Year 'Year',Emp_ID 'EMPID', Emp_RBM_BillDetail_ID 'BillDetail_ID' from T010011 EmpName , Emp_Reimbursement_BillDetail RBMBD  where EmpName.CI_NU8_Address_Number=RBMBD.emp_id and FinnacialYear='" & Finanicalyear & "' "
                strSql += strWhere
                strSQLQ = "Union Select (EmpName.CI_VC36_name) as EMPName,DATENAME(MONTH,dateadd(month,RBM_Month-1,0)) As MName,RBM_Month,dbo.udf_GetReimbursement(RBM_ID_FK) as Reimbursement,(select EMRD.RBM_MonthlyEntitlement from Emp_Reimbursement_Detail EMRD where EMRD.Emp_ID=RBMBD.Emp_ID  and EMRD.RBM_ID=RBMBD.RBM_ID_FK and RBM_FinanicalYear='" & Finanicalyear & "' ) as 'ReimbursementAllowed',isnull(BillSubmitted,0) 'BillSubmitted',isnull(RBM_ToBePaid,0) 'ReimbursementPaid',isnull(Bill_InAdvance,0) 'Bill_InAdvance',isnull(Bill_Due,0) 'Bill_Due',RBM_Year 'Year',Emp_ID 'EMPID', Emp_RBM_BillDetail_ID 'BillDetail_ID' from T010011 EmpName ,Emp_Reimbursement_BillDetail RBMBD  where EmpName.CI_NU8_Address_Number=RBMBD.emp_id and  FinnacialYear='" & Finanicalyear & "' and RBM_Month<=3 "
                strSql += strSQLQ
                strSql += strWhere
                strSql += " )order by Emp_RBM_BillDetail_ID  "
            End If
            dtGetBills = clsData.Search(strSql, "T010011")
            Return dtGetBills
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetRBMDetail-469", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
    Public Function DeleteBillSubmissionDate(ByVal Submission_ID As Integer) As Boolean
        Try
            If clsData.Delete("Delete from BillSubmissionDate where BillSubmission_ID=" & Submission_ID, IsolationLevel.Serializable) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "DeleteBillSubmissionDate-490", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
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
    Public Function DeleteRBMAmt(ByVal Emp_ID As Integer, ByVal FinacialYear As String) As Boolean
        Try
            If clsData.Delete("Delete from Emp_Reimbursement_Detail where RBM_FinanicalYear='" & FinacialYear & "' and Emp_ID=" & Emp_ID, IsolationLevel.Serializable) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "DeleteRBMAmt-511", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
    Public Function AdjustBills(ByVal arValues As ArrayList) As Boolean
        Try
            Dim cmd As New SqlCommand()
            cmd.Parameters.AddWithValue("@Month", arValues(0))
            cmd.Parameters.AddWithValue("@UpdatedDate", arValues(1))
            cmd.Parameters.AddWithValue("@Financial_Year", arValues(2))
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_AdjustBills1"
            If clsData.ExecuteProcedure(cmd) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "AdjustBills-539", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "DeleteBill"
    ''' <summary>
    ''' This function is used to delete the records from  Emp_Reimbursement_Detail before Update
    ''' </summary>
    ''' <param name="BillID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteBill(ByVal BillID As Integer) As Boolean
        Try
            If clsData.Delete("Delete from Bill_History where ID=" & BillID, IsolationLevel.Serializable) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "DeleteBill-563", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "UpdateBill"
    ''' <summary>
    ''' This function is used to update the records 
    ''' </summary>
    ''' <param name="BillID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBill(ByVal BillID As Integer, ByVal BILLSUBMITTED As String, ByVal fileName As String, ByVal filePath As String) As Boolean
        Try
            If String.IsNullOrEmpty(fileName) Then
                If clsData.Update("update Bill_History set BILLSUBMITTED='" & BILLSUBMITTED & "' where ID=" & BillID) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                If clsData.Update("update Bill_History set BILLSUBMITTED='" & BILLSUBMITTED & "',billfileName='" & fileName & "', BillFilePath='" & filePath & "' where ID=" & BillID) = True Then
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "UpdateBill-583", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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
            dsNew = clsData.SearchDS("select * from Bill_History where ID='" & BillID & "'")
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SearchExistingBills", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
        Return dsNew
    End Function
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

            Dim dtGetBills As New DataTable

            Dim strSql As String

            Dim strSQLQ As String

            If Month > 0 Then


                strSql = "Select EmpName.CI_NU8_Address_Number,(EmpName.CI_VC36_name) as EMPName,EmpName.CI_VC36_ID_1 as EmpCode,DATENAME(MONTH,dateadd(month,RBM_Month-1,0)) As MName,dbo.udf_GetReimbursement(RBM_ID_FK) as Reimbursement,isnull((select Sum(isnull(BillSubmitted,0))as 'Total' from Emp_Reimbursement_BillDetail EMRD where EMRD.Emp_ID=RBMBD.Emp_ID and EMRD.RBM_ID_FK=RBMBD.RBM_ID_FK and EMRD.FinnacialYear='" & Finanicalyear & "' ),0) as 'ReimbursementAllowed',isnull(BillSubmitted,0) 'BillSubmitted',isnull(RBM_ToBePaid,0) 'ReimbursementPaid',isnull(Bill_InAdvance,0) 'Bill_InAdvance',isnull(Bill_Due,0) 'Bill_Due',RBM_Year 'Year',Emp_ID 'EMPID', Emp_RBM_BillDetail_ID 'BillDetail_ID' from T010011 EmpName ,Emp_Reimbursement_BillDetail RBMBD,T060011 where FinnacialYear='" & Finanicalyear & "' and RBM_Month=" & Month

                strSql += strWhere

                strSql += " and CI_VC8_Address_Book_Type='EM'  and CI_VC36_ID_1<>'' and CI_NU9_SalaryID>0 and UM_SI2_CreateSalary=1 and UM_IN4_Address_No_FK=CI_NU8_Address_Number order by CI_NU9_SalaryID "

            Else

                strSql = "(Select EmpName.CI_NU8_Address_Number,(EmpName.CI_VC36_name) as EMPName,EmpName.CI_VC36_ID_1 as EmpCode,DATENAME(MONTH,dateadd(month,RBM_Month-1,0)) As MName,RBM_Month,dbo.udf_GetReimbursement(RBM_ID_FK) as Reimbursement,isnull((select Sum(isnull(BillSubmitted,0))as 'Total' from Emp_Reimbursement_BillDetail EMRD where EMRD.Emp_ID=RBMBD.Emp_ID and EMRD.RBM_ID_FK=RBMBD.RBM_ID_FK and EMRD.FinnacialYear='" & Finanicalyear & "' ),0) as 'ReimbursementAllowed',isnull(BillSubmitted,0) 'BillSubmitted',isnull(RBM_ToBePaid,0) 'ReimbursementPaid',isnull(Bill_InAdvance,0) 'Bill_InAdvance',isnull(Bill_Due,0) 'Bill_Due',RBM_Year 'Year',Emp_ID 'EMPID', Emp_RBM_BillDetail_ID 'BillDetail_ID' from T010011 EmpName , Emp_Reimbursement_BillDetail RBMBD,T060011 where EmpName.CI_NU8_Address_Number*=RBMBD.emp_id and FinnacialYear='" & Finanicalyear & "' "

                strSql += strWhere

                strSQLQ = "Union Select EmpName.CI_NU8_Address_Number,(EmpName.CI_VC36_name) as EMPName,EmpName.CI_VC36_ID_1 as EmpCode,DATENAME(MONTH,dateadd(month,RBM_Month-1,0)) As MName,RBM_Month,dbo.udf_GetReimbursement(RBM_ID_FK) as Reimbursement,isnull((select Sum(isnull(BillSubmitted,0))as 'Total' from Emp_Reimbursement_BillDetail EMRD where EMRD.Emp_ID=RBMBD.Emp_ID and EMRD.RBM_ID_FK=RBMBD.RBM_ID_FK and EMRD.FinnacialYear='" & Finanicalyear & "' ),0) as 'ReimbursementAllowed',isnull(BillSubmitted,0) 'BillSubmitted',isnull(RBM_ToBePaid,0) 'ReimbursementPaid',isnull(Bill_InAdvance,0) 'Bill_InAdvance',isnull(Bill_Due,0) 'Bill_Due',RBM_Year 'Year',Emp_ID 'EMPID', Emp_RBM_BillDetail_ID 'BillDetail_ID' from T010011 EmpName ,Emp_Reimbursement_BillDetail RBMBD,T060011 where EmpName.CI_NU8_Address_Number*=RBMBD.emp_id and FinnacialYear='" & Finanicalyear & "' and RBM_Month<=3 "

                strSql += strSQLQ

                strSql += strWhere

                strSql += " and CI_VC8_Address_Book_Type='EM' and UM_SI2_CreateSalary=1 and UM_IN4_Address_No_FK=CI_NU8_Address_Number and CI_VC36_ID_1<>'' and CI_NU9_SalaryID>0"

                strSql += " )"

            End If

            dtGetBills = clsData.Search(strSql, "T010011")

            Return dtGetBills

        Catch ex As Exception

            CreateLog("ReimburstmentDAL", "GetRBMDetail-469", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

            Return Nothing

        End Try

    End Function

#End Region

#Region "GetDisapproved Bill"
    ''' <summary>
    ''' This Function is used to get the Details of Disaaproved Bills
    ''' </summary>
    ''' <param name="EmpID"></param>
    ''' <param name="Month"></param>
    ''' <param name="FinanicalYear"></param>
    ''' <param name="DispprovedFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEmpDisapprovedBill(ByVal EmpID As Integer, ByVal Month As Integer, ByVal FinanicalYear As String, ByVal DispprovedFlag As Integer) As DataTable
        Try
            Dim dtBillSubmitted As New DataTable
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_EmpDisapprovedBills"
            cmd.Parameters.AddWithValue("@EMPID", EmpID)
            cmd.Parameters.AddWithValue("@Month", Month)
            cmd.Parameters.AddWithValue("@Financial_Year", FinanicalYear)
            cmd.Parameters.AddWithValue("@DiapprovedFlag", DispprovedFlag)
            dtBillSubmitted = clsData.Search(cmd)
            Return dtBillSubmitted
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "GetEmpDisapprovedBill-480", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
#End Region

#Region "SetDisapprovedBillsStatus"
    ''' <summary>
    ''' This Function is used to set the Status for dissaproval Bills that is BillStatus=1 
    ''' </summary>
    ''' <param name="arValues"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetDisapprovedBillsStatus(ByVal arValues As ArrayList) As Boolean
        Try

            If clsData.Update("update  Bill_History Set BillStatus=1 , DisapprovalReason='" & arValues(1) & "' where ID = " & arValues(0)) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReimburstmentDAL", "SetDisapprovedBillsStatus-729", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return False
        End Try
    End Function
#End Region
#End Region
End Class
