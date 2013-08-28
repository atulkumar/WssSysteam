Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text
Imports System.IO
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Net.NetworkInformation
Partial Class AdministrationCenter_CommanUserCreation_UserCreation

#Region "Page Level Variabls"
    Inherits System.Web.UI.Page
    Private _errorMsg As String
    Private _statusMsg As String
    Private objDataset As New DataSet()
    Private objTempDataTable As New DataTable()
    Dim intStatus As Integer = 0
    Dim valMsgWSS As String = String.Empty
    Dim valMsgSPOC As String = String.Empty
    Dim valMsgINC As String = String.Empty
    Dim strExceptionWSS As String = String.Empty
    Dim strUserCreatedWSS As String = String.Empty
    Dim strInvalidDataSpoc As String = String.Empty
    Dim strInvalidDatainc As String = String.Empty
    Dim strExceptionSpoc As String = String.Empty
    Dim strExceptioninc As String = String.Empty
    Dim strUserCreatedSPoc As String = String.Empty
    Dim strUserCreatedinc As String = String.Empty
    Dim strUserId As String = String.Empty
    Dim SavePath As String = String.Empty
    Dim sConnectionString As String
    Shared path As String
    Dim EmpID__1 As String
    Private mailMsg As MailMessage = New MailMessage()
    Dim SmtpServer As New SmtpClient()
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        Dim txthiddenImage = Request.Form("txthiddenImage")
        If txthiddenImage = "Logout" Then
            FormsAuthentication.SignOut()
            Call ClearVariables()
            Response.Redirect("../Login/Login.aspx")
        End If
        imgExportToExcel.Enabled = False
    End Sub

    ''' <summary>
    ''' Cretae user Button code  
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click
        ''''sesion to get file from the user
        If fuCreateUser.HasFile = False Then
            lstError.Items.Clear()
            lstError.Items.Add(" Uplaod Excel for User Creation...  ")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Return
        Else
            Try
                SavePath = Server.MapPath("UserCreationFiles\\" & fuCreateUser.FileName)
                Dim fileInfo As New FileInfo(SavePath)
                If Not fileInfo.Extension.ToString() = ".xls" And Not fileInfo.Extension.ToString() = ".xlsx" Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Upload Excel file only...  ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Return
                End If
                Dim fi As New FileInfo(SavePath)
                If fi.Exists Then
                    fi.Delete()
                End If
                fuCreateUser.SaveAs(SavePath)
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add(" You have not access for User Uploading...  ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Return
            End Try
        End If
        '''''''''''
        path = SavePath
        'Dim path As String = SavePath
        sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & path & ";Extended Properties=Excel 12.0;"
        Dim objConnection = New OleDbConnection(sConnectionString)
        Const query As String = "Select * from [Users$] where Status='NEW'"
        objConnection.Open()
        Dim objCommand = New OleDbCommand(query, objConnection)
        Dim objAdapter = New OleDbDataAdapter()
        objAdapter.SelectCommand = objCommand
        objAdapter.Fill(objDataset)
        If objDataset.Tables Is Nothing Then
            lstError.Items.Clear()
            lstError.Items.Add(" No new user for cretaion in excel...  ")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return
        End If
        objAdapter.Dispose()
        objCommand.Dispose()
        objConnection.Close()
        objConnection.Dispose()
        ''****************WSS User Creation************''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If intStatus = 0 Then
            For count As Int16 = 0 To objDataset.Tables(0).Rows.Count - 1
                Dim Res As String = validateUser(count, ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                Try
                    If Res = String.Empty Then
                        InsertMainData(count, ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                        If strUserCreatedWSS = String.Empty Then
                            strUserCreatedWSS = "Users created in WSS:" & objDataset.Tables(0).Rows(count)("Name").ToString()
                        Else
                            strUserCreatedWSS += "," & objDataset.Tables(0).Rows(count)("Name").ToString()
                        End If
                        lblUserCreatedWSS.Text = strUserCreatedWSS
                        lblUserCreatedWSS.Visible = True
                        lblUserCreatedWSS.ForeColor = Drawing.Color.Green
                    Else
                        If valMsgWSS = String.Empty Then
                            valMsgWSS = "For WSS: User Data is not correct in excel for users:- " & objDataset.Tables(0).Rows(count)("Name").ToString() & "--Exception:" & Res
                        Else
                            valMsgWSS += ", " & objDataset.Tables(0).Rows(count)("Name").ToString() & "--Exception: " & Res
                        End If
                        lblInvalidDataWSS.Text = valMsgWSS
                        lblInvalidDataWSS.Visible = True
                        lblInvalidDataWSS.ForeColor = Drawing.Color.Blue
                    End If
                Catch ex As Exception
                    If strExceptionWSS = String.Empty Then
                        strExceptionWSS = "Exception For Users in WSS: " & objDataset.Tables(0).Rows(count)("Name").ToString()
                    Else
                        strExceptionWSS += "," & objDataset.Tables(0).Rows(count)("Name").ToString()
                    End If
                    lblException.Text = strExceptionWSS
                    lblException.Visible = True
                    lblException.ForeColor = Drawing.Color.Red
                End Try

            Next
            intStatus = 1
        End If
        ''///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''****************SPOC User Creation*********************''''''''''''''''''''''''''''
        If intStatus = 1 Then
            For count As Int16 = 0 To objDataset.Tables(0).Rows.Count - 1
                Try
                    Dim Res As String = validateUser(count, ConfigurationManager.ConnectionStrings("Spoc").ConnectionString)
                    If Res = String.Empty Then
                        InsertMainData(count, ConfigurationManager.ConnectionStrings("Spoc").ConnectionString)
                        If strUserCreatedSPoc = String.Empty Then
                            strUserCreatedSPoc = "Users created in SPOC:" & objDataset.Tables(0).Rows(count)("Name").ToString()
                        Else
                            strUserCreatedSPoc += "," & objDataset.Tables(0).Rows(count)("Name").ToString()
                        End If
                        lblUserCreatedSpoc.Text = strUserCreatedSPoc
                        lblUserCreatedSpoc.Visible = True
                        lblUserCreatedSpoc.ForeColor = Drawing.Color.Green
                    Else
                        If strInvalidDataSpoc = String.Empty Then
                            strInvalidDataSpoc = "FOR SPOC: User Data is not correct in excel for users:-" & objDataset.Tables(0).Rows(count)("Name").ToString() & "--Exception:" & Res
                        Else
                            strInvalidDataSpoc += ", " & objDataset.Tables(0).Rows(count)("Name").ToString() & " Exception: " & Res
                        End If
                        lblInvalidDateSPOC.Text = strInvalidDataSpoc
                        lblInvalidDateSPOC.Visible = True
                        lblInvalidDateSPOC.ForeColor = Drawing.Color.Blue
                    End If
                Catch ex As Exception
                    If strExceptionSpoc = String.Empty Then
                        strExceptionSpoc = "Exception For Users in SPOC: " & objDataset.Tables(0).Rows(count)("Name").ToString()
                    Else
                        strExceptionSpoc += "," & objDataset.Tables(0).Rows(count)("Name").ToString()
                    End If
                    lblExceptionSOpc.Text = strExceptionWSS
                    lblExceptionSOpc.Visible = True
                    lblExceptionSOpc.ForeColor = Drawing.Color.Red
                End Try

            Next
            intStatus = 2
        End If
        ''****'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''************Incidence User Creation***********************'''''''''''''''''''''''''
        If intStatus = 2 Then
            For count As Int16 = 0 To objDataset.Tables(0).Rows.Count - 1
                Try
                    Dim Res As String = validateUser(count, ConfigurationManager.ConnectionStrings("Incidence").ConnectionString)
                    If Res = String.Empty Then
                        InsertMainData(count, ConfigurationManager.ConnectionStrings("Incidence").ConnectionString)
                        If strUserCreatedinc = String.Empty Then
                            strUserCreatedinc = "Users created in Incidence:" & objDataset.Tables(0).Rows(count)("Name").ToString()
                        Else
                            strUserCreatedinc += "," & objDataset.Tables(0).Rows(count)("Name").ToString()
                        End If
                        lblUserCreatedInc.Text = strUserCreatedinc
                        lblUserCreatedInc.Visible = True
                        lblUserCreatedInc.ForeColor = Drawing.Color.Green
                    Else
                        If strInvalidDatainc = String.Empty Then
                            strInvalidDatainc = "FOR Incidence: User Data is not correct in excel for users" & objDataset.Tables(0).Rows(count)("Name").ToString() & " Exception:" & Res
                        Else
                            strInvalidDatainc += "," & objDataset.Tables(0).Rows(count)("Name").ToString() & " Exception: " & Res
                        End If
                        lblInvalidInc.Text = strInvalidDatainc
                        lblInvalidInc.Visible = True
                        lblInvalidInc.ForeColor = Drawing.Color.Blue
                    End If
                Catch ex As Exception
                    If strExceptioninc = String.Empty Then
                        strExceptioninc = "Exception For Users in Incidence: " & objDataset.Tables(0).Rows(count)("Name").ToString()
                    Else
                        strExceptioninc += "," & objDataset.Tables(0).Rows(count)("Name").ToString()
                    End If
                    lblExcetioninc.Text = strExceptioninc
                    lblExcetioninc.Visible = True
                    lblExcetioninc.ForeColor = Drawing.Color.Red
                End Try
            Next
        End If
    End Sub
    Protected Sub imgExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToExcel.Click
        Response.Clear()
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", "inline;filename=UsersCreated.xls")
        'Response.WriteFile(path)
        Response.TransmitFile(path)
        Response.Flush()
    End Sub

    ''' <summary>
    ''' Function To Validate Data in Excel sheet
    ''' </summary>
    ''' <param name="count"></param>
    ''' <param name="strConnectionstring"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function validateUser(ByVal count As Int32, ByVal strConnectionstring As String) As String
        Dim intLoop As Integer = 0
        Dim strStatus As String = String.Empty

        If objDataset.Tables(0).Rows(count)("Employee Type").ToString() = "" Then

            strStatus = strStatus + "Employee Type Blank "
        End If

        If objDataset.Tables(0).Rows(count)("Name").ToString() = "" Then
            strStatus = strStatus + "Name Blank "
        End If
        If objDataset.Tables(0).Rows(count)("Gender").ToString() = "" Then
            strStatus = strStatus + "Blank Gender "
        End If
        'If Not IsDate(objDataset.Tables(0).Rows(count)("Birth Date").ToString()) Then

        '    strStatus = strStatus + "Invalid Date of Birth "
        'End If
        If Not IsDate(objDataset.Tables(0).Rows(count)("DOJ").ToString()) Then

            strStatus = strStatus + "Invalid Date of Joining "
        End If

        If objDataset.Tables(0).Rows(count)("Designation").ToString() = "" Then

            strStatus = strStatus + "Blank Designation "

        End If

        If objDataset.Tables(0).Rows(count)("Department").ToString() = "" Then
            strStatus = strStatus + "Blank Department"
        End If
        If objDataset.Tables(0).Rows(count)("Current Address").ToString() = "" Then

            strStatus = strStatus + "Blank Current Address "
        End If
        If objDataset.Tables(0).Rows(count)("Current Proviance").ToString() = "" Then

            strStatus = strStatus + "Blank Current Province "
        End If
        If objDataset.Tables(0).Rows(count)("Current City").ToString() = "" Then

            strStatus = strStatus + "Blank Current City "
        End If
        If objDataset.Tables(0).Rows(count)("Current Pin").ToString() = "" Then

            strStatus = strStatus + "Blank Current Pin "
        End If
        If objDataset.Tables(0).Rows(count)("Business Relation").ToString() = "" Then

            strStatus = strStatus + "Blank Business Relation "
        End If
        If objDataset.Tables(0).Rows(count)("Office Email").ToString() = "" Then
            strStatus = strStatus + "Blank Office Email "
        End If
        If Not IsNumeric(objDataset.Tables(0).Rows(count)("Official Phone").ToString()) And objDataset.Tables(0).Rows(count)("Official Phone").ToString() <> "" Then

            strStatus = strStatus + "Invalid Office Phone"
        End If
        If Not IsNumeric(objDataset.Tables(0).Rows(count)("Mobile No").ToString()) And objDataset.Tables(0).Rows(count)("Mobile No").ToString() <> "" Then

            strStatus = strStatus + "Invalid Mobile No "
        End If
        If objDataset.Tables(0).Rows(count)("Role WSS").ToString() = "" Then

            strStatus = strStatus + "Blank WSS Role"
        End If
        If objDataset.Tables(0).Rows(count)("Role SPOC").ToString() = "" Then

            strStatus = strStatus + "Blank SPOC Role"
        End If
        If objDataset.Tables(0).Rows(count)("Role Incidence").ToString() = "" Then

            strStatus = strStatus + "Blank Incidence Role"
        End If
        If objDataset.Tables(0).Rows(count)("Subcategory WSS").ToString() = "" Then

            strStatus = strStatus + "Blank Sub-Category WSS"
        End If
        If objDataset.Tables(0).Rows(count)("Subcategory SPOC").ToString() = "" Then

            strStatus = strStatus + "Blank Sub-Category SPOC"
        End If
        If objDataset.Tables(0).Rows(count)("WSS Company").ToString() = "" Then

            strStatus = strStatus + "Blank Comapny WSS"
        End If
        If objDataset.Tables(0).Rows(count)("SNo").ToString = "" Then
            strStatus = strStatus + "Blank S.No"
        End If
        Dim strsubcat As String = String.Empty
        If strConnectionstring.Contains("WSS_ATT") = True Then
            strsubcat = objDataset.Tables(0).Rows(count)("Subcategory WSS").ToString()
        ElseIf strConnectionstring.Contains("WSS_SPOC") = True Then
            strsubcat = objDataset.Tables(0).Rows(count)("Subcategory SPOC").ToString()
        ElseIf strConnectionstring.Contains("WSS_incidence") = True Then
            strsubcat = objDataset.Tables(0).Rows(count)("Subcategory Incidence").ToString()
        End If
        If ChechSubcategory(strsubcat, strConnectionstring) = False Then
            strStatus = strStatus + "Subcategory not found.."
            Return strStatus
        End If
        CheckDesignation(objDataset.Tables(0).Rows(count)("Designation").ToString(), strConnectionstring)
        CheckDepartment(objDataset.Tables(0).Rows(count)("Department").ToString(), strConnectionstring)
        Return strStatus
    End Function

    ''' <summary>
    ''' Function to check subcategory exist or not...
    ''' </summary>
    ''' <param name="subcat"></param>
    ''' <param name="strconnectionstring"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ChechSubcategory(ByVal subcat As String, ByVal strconnectionstring As String) As Boolean
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        Dim strID As String = String.Empty
        Try
            objConn = New SqlConnection(strconnectionstring)
            strCommand = "select pr_vc20_name from T210011 where pr_vc20_name=@subCat"
            objComm = New SqlCommand(strCommand, objConn)
            If objConn.State = ConnectionState.Closed Then
                objConn.Open()
            End If
            objComm.Parameters.Add("@subCat", SqlDbType.VarChar, 50)
            objComm.Parameters("@subCat").Value = subcat
            strID = objComm.ExecuteScalar
            objConn.Close()
            If strID <> String.Empty Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Insert Main data
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <param name="strConnectionString"></param>
    ''' <remarks></remarks>
    Public Sub InsertMainData(ByVal count As Integer, ByVal strConnectionString As String)
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        Dim intRecordCount As Integer = 0
        Dim intTableID As Integer = 0
        Dim intTableIDSend As Integer = 0
        'Try
        strCommand = "Select max(CI_NU8_Address_Number) from T010011"
        objConn = New SqlConnection(strConnectionString)
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        intTableID = objComm.ExecuteScalar() + 1
        intTableIDSend = intTableID
        objConn.Close()

        strCommand = "INSERT INTO T010011 (CI_NU9_SalaryID,CI_VC36_ID_1,ci_dt8_date_modified,ci_dt8_date_created,CI_IN4_Business_Relation, CI_VC8_City,CI_VC8_Province,CI_VC36_Postal_Code,CI_VC8_Country,CI_VC8_Status,CI_VC8_Email_Type_1,CI_VC8_Email_Type_2,CI_VC8_Phone_Type_1,CI_VC8_Phone_Type_2,CI_VC8_Address_Book_Type,CI_NU8_Address_Number ,CI_VC36_Name,CI_VC36_Address_Line_1,CI_VC28_Email_1,CI_VC28_Email_2,CI_NU16_Phone_Number_1,CI_NU16_Phone_Number_2) VALUES "
        strCommand = strCommand & "( @CI_NU9_SalaryID, @CI_VC36_ID_1,@ci_dt8_date_modified,@ci_dt8_date_created,@CI_IN4_Business_Relation,@CI_VC8_City ,@CI_VC8_Province,@CI_VC36_Postal_Code,@CI_VC8_Country,@CI_VC8_Status,@CI_VC8_Email_Type_1,@CI_VC8_Email_Type_2,@CI_VC8_Phone_Type_1,@CI_VC8_Phone_Type_2,@CI_VC8_Address_Book_Type,@CI_NU8_Address_Number,@CI_VC36_Name,@CI_VC36_Address_Line_1,@CI_VC28_Email_1,@CI_VC28_Email_2,@CI_NU16_Phone_Number_1,@CI_NU16_Phone_Number_2)"



        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        objComm.Parameters.Add("@CI_VC36_Name", SqlDbType.VarChar, 30)
        objComm.Parameters("@CI_VC36_Name").Value = objDataset.Tables(0).Rows(count)("Name").ToString()

        objComm.Parameters.Add("@CI_NU8_Address_Number", SqlDbType.Int, 18)
        objComm.Parameters("@CI_NU8_Address_Number").Value = intTableID

        objComm.Parameters.Add("@CI_VC36_Address_Line_1", SqlDbType.VarChar, 36)
        Dim intGetData As Integer = 0
        If objDataset.Tables(0).Rows(count)("Current Address").ToString().Length > 500 Then
            intGetData = 500
        End If
        If objDataset.Tables(0).Rows(count)("Current Address").ToString().Length = 0 Then
            intGetData = 0
        Else
            intGetData = Strings.Len(objDataset.Tables(0).Rows(count)("Current Address").ToString())
        End If
        objComm.Parameters("@CI_VC36_Address_Line_1").Value = objDataset.Tables(0).Rows(count)("Current Address").ToString().Substring(0, intGetData)

        objComm.Parameters.Add("@CI_VC8_City", SqlDbType.VarChar, 8)
        objComm.Parameters("@CI_VC8_City").Value = CheckMasterCodes(objDataset.Tables(0).Rows(count)("Current City").ToString(), "CTY", strConnectionString)

        objComm.Parameters.Add("@CI_VC8_Province", SqlDbType.VarChar, 8)
        objComm.Parameters("@CI_VC8_Province").Value = CheckMasterCodes(objDataset.Tables(0).Rows(count)("Current Proviance").ToString(), "PROV", strConnectionString)

        objComm.Parameters.Add("@CI_VC36_Postal_Code", SqlDbType.VarChar, 10)
        objComm.Parameters("@CI_VC36_Postal_Code").Value = objDataset.Tables(0).Rows(count)("Current Pin").ToString()


        objComm.Parameters.Add("@CI_VC8_Country", SqlDbType.VarChar, 20)
        objComm.Parameters("@CI_VC8_Country").Value = "IND"

        objComm.Parameters.Add("@CI_VC28_Email_1", SqlDbType.VarChar, 40)
        objComm.Parameters("@CI_VC28_Email_1").Value = objDataset.Tables(0).Rows(count)("Office Email").ToString()

        objComm.Parameters.Add("@CI_VC8_Email_Type_1", SqlDbType.VarChar, 10)
        objComm.Parameters("@CI_VC8_Email_Type_1").Value = "OFF"

        objComm.Parameters.Add("@CI_VC28_Email_2", SqlDbType.VarChar, 40)
        objComm.Parameters("@CI_VC28_Email_2").Value = objDataset.Tables(0).Rows(count)("Persol Email").ToString()

        objComm.Parameters.Add("@CI_VC8_Email_Type_2", SqlDbType.VarChar, 10)
        objComm.Parameters("@CI_VC8_Email_Type_2").Value = "PRSL"

        objComm.Parameters.Add("@CI_NU16_Phone_Number_1", SqlDbType.BigInt, 18)
        If String.IsNullOrEmpty(objDataset.Tables(0).Rows(count)("Official Phone").ToString()) Then
            objComm.Parameters("@CI_NU16_Phone_Number_1").Value = 0
        Else
            objComm.Parameters("@CI_NU16_Phone_Number_1").Value = objDataset.Tables(0).Rows(count)("Official Phone").ToString()
        End If

        objComm.Parameters.Add("@CI_VC8_Phone_Type_1", SqlDbType.VarChar, 10)
        objComm.Parameters("@CI_VC8_Phone_Type_1").Value = "OFF"

        objComm.Parameters.Add("@CI_NU16_Phone_Number_2", SqlDbType.BigInt, 18)
        If String.IsNullOrEmpty(objDataset.Tables(0).Rows(count)("Mobile No").ToString()) Then
            objComm.Parameters("@CI_NU16_Phone_Number_2").Value = 0
        Else
            objComm.Parameters("@CI_NU16_Phone_Number_2").Value = objDataset.Tables(0).Rows(count)("Mobile No").ToString()
        End If

        objComm.Parameters.Add("@CI_VC8_Phone_Type_2", SqlDbType.VarChar, 10)
        objComm.Parameters("@CI_VC8_Phone_Type_2").Value = "PRSL"

        objComm.Parameters.Add("@CI_VC8_Address_Book_Type", SqlDbType.VarChar, 8)
        objComm.Parameters("@CI_VC8_Address_Book_Type").Value = "EM"

        objComm.Parameters.Add("@CI_VC8_Status", SqlDbType.VarChar, 8)
        objComm.Parameters("@CI_VC8_Status").Value = "ENA"

        objComm.Parameters.Add("@CI_IN4_Business_Relation", SqlDbType.VarChar, 8)
        objComm.Parameters("@CI_IN4_Business_Relation").Value = GetCompanyID(objDataset.Tables(0).Rows(count)("Business Relation").ToString(), strConnectionString)

        objComm.Parameters.Add("@ci_dt8_date_created", SqlDbType.DateTime)
        objComm.Parameters("@ci_dt8_date_created").Value = DateAndTime.DateAdd(DateInterval.Day, -1, System.DateTime.Now)

        objComm.Parameters.Add("@ci_dt8_date_modified", SqlDbType.DateTime)
        objComm.Parameters("@ci_dt8_date_modified").Value = System.DateTime.Now

        objComm.Parameters.Add("@CI_VC36_ID_1", SqlDbType.VarChar, 15)
        If strConnectionString.Contains("WSS_ATT") = True Then
            EmpID__1 = GenrateEmpID(objDataset.Tables(0).Rows(count)("Employee Type").ToString(), strConnectionString)
            objComm.Parameters("@CI_VC36_ID_1").Value = EmpID__1
        Else
            EmpID__1 = GetEmpIdFromWSS(objDataset.Tables(0).Rows(count)("Name").ToString())
            objComm.Parameters("@CI_VC36_ID_1").Value = EmpID__1
        End If

        objComm.Parameters.Add("@CI_NU9_SalaryID", SqlDbType.VarChar, 30)
        objComm.Parameters("@CI_NU9_SalaryID").Value = EmpID__1.Substring(3, 3)

        objComm.ExecuteNonQuery()
        objConn.Close()


        objConn.Close()

        InsertDetails(intTableIDSend, strConnectionString, count)



    End Sub


    ''' <summary>
    ''' Get department, designation code from UDC
    ''' </summary>
    ''' <param name="strDesName"></param>
    ''' <param name="strUdcType"></param>
    ''' <param name="strConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckMasterCodes(ByVal strDesName As String, ByVal strUdcType As String, ByVal strConnectionString As String) As String
        Dim objConn As SqlConnection = Nothing
        Dim objComm As SqlCommand = Nothing
        Dim strCommand As String = String.Empty
        Dim strDepCode As String = Nothing
        Try
            objConn = New SqlConnection(strConnectionString)

            strCommand = "select Name from udc where Description= @DesName and UDCType=@UDCType"

            objComm = New SqlCommand(strCommand, objConn)
            objConn.Open()
            objComm.Parameters.Add("@DesName", SqlDbType.VarChar, 25)
            objComm.Parameters("@DesName").Value = strDesName
            objComm.Parameters.Add("@UDCType", SqlDbType.VarChar, 15)
            objComm.Parameters("@UDCType").Value = strUdcType
            strDepCode = objComm.ExecuteScalar()
            objConn.Close()
            If String.IsNullOrEmpty(strDepCode) Then
                strDepCode = "NA"
            End If
            Return strDepCode
        Catch ex As Exception
            Throw ex
        Finally
            objConn = Nothing
            objComm = Nothing
            strCommand = Nothing
            strDepCode = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get company id by company name
    ''' </summary>
    ''' <param name="strComName"></param>
    ''' <param name="strConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCompanyID(ByVal strComName As String, ByVal strConnectionString As String) As Integer
        Dim objConn As SqlConnection = Nothing
        Dim objComm As SqlCommand = Nothing
        Dim strCommand As String = String.Empty
        Dim strComCode As Integer = 0
        Try
            objConn = New SqlConnection(strConnectionString)

            strCommand = "Select ci_nu8_address_number from T010011 where ci_vc36_name =@ci_vc36_name"
            objComm = New SqlCommand(strCommand, objConn)
            objConn.Open()
            objComm.Parameters.Add("@ci_vc36_name", SqlDbType.VarChar, 30)
            objComm.Parameters("@ci_vc36_name").Value = strComName

            strComCode = objComm.ExecuteScalar()
            objConn.Close()
            If strComCode = 0 Then
                strComCode = 8
            End If
            Return strComCode
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub UpdateExcel(ByVal SNo As Int32, ByVal EmpID As String)

        Dim objUpdateConnection = New OleDbConnection(sConnectionString)
        Dim updateQuery As String = "Update [Users$] set EmpID=" & "'" & EmpID & "', Status = 'Done' where SNo=" & SNo & ""
        'Const updateQuery As String = "Update [Users$] set Status = 'Done' where Status = 'new'"
        Dim objUpdateCommand = New OleDbCommand(updateQuery, objUpdateConnection)
        objUpdateConnection.Open()
        objUpdateCommand.ExecuteNonQuery()
        objUpdateCommand.Dispose()
        objUpdateConnection.Close()
        objUpdateConnection.Dispose()

        imgExportToExcel.Enabled = True
    End Sub

    ''' <summary>
    ''' Insert data into detail Table
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <param name="intTableId"></param>
    ''' <param name="strConnectionString"></param>
    ''' <remarks></remarks>
    Public Sub InsertDetails(ByVal intTableId As Integer, ByVal strConnectionString As String, ByVal count As Integer)
        Dim objConn As SqlConnection = Nothing
        Dim objComm As SqlCommand = Nothing
        Dim strCommand As String = String.Empty
        Dim intRecordCount As Integer = 0
        Dim intTableIdHold As Integer = 0

        intTableIdHold = intTableId
        objConn = New SqlConnection(strConnectionString)

        strCommand = "INSERT INTO T010043(PI_VC30_Role,pi_vc4_timezone, PI_VC36_First_Name,PI_VC36_Middle_Name,PI_VC36_Last_Name,PI_VC8_Marital_Status,PI_NU8_Address_No,PI_DT8_Date_Of_Birth,PI_VC8_Sex,PI_DT8_Date_Of_Joining,PI_VC15_BloodGroup,PI_VC50_PassportNo,PI_VC8_Department,PI_NU9_JobRole) VALUES "
        strCommand = strCommand & "(@PI_VC30_Role,@pi_vc4_timezone,@PI_VC36_First_Name,@PI_VC36_Middle_Name,@PI_VC36_Last_Name,@PI_VC8_Marital_Status,@PI_NU8_Address_No ,@PI_DT8_Date_Of_Birth ,@PI_VC8_Sex ,@PI_DT8_Date_Of_Joining  ,@PI_VC15_BloodGroup ,@PI_VC50_PassportNo ,@PI_VC8_Department ,@PI_NU9_JobRole)"

        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        objComm.Parameters.Add("@PI_NU8_Address_No", SqlDbType.Int)
        objComm.Parameters("@PI_NU8_Address_No").Value = intTableId

        objComm.Parameters.Add("@PI_DT8_Date_Of_Birth", SqlDbType.VarChar, 100)
        If String.IsNullOrEmpty(objDataset.Tables(0).Rows(count)("Birth Date").ToString()) Then
            If Not objDataset.Tables(0).Rows(count)("Birth Date").ToString() = String.Empty Then
                objComm.Parameters("@PI_DT8_Date_Of_Birth").Value = Convert.ToDateTime(objDataset.Tables(0).Rows(count)("Birth Date").ToString()).ToString()
            Else
                objComm.Parameters("@PI_DT8_Date_Of_Birth").Value = ""
            End If
        Else
            objComm.Parameters("@PI_DT8_Date_Of_Birth").Value = ""
        End If
        objComm.Parameters.Add("@PI_VC8_Sex", SqlDbType.VarChar, 100)
        If objDataset.Tables(0).Rows(count)("Gender").ToString() = "M" Then
            objComm.Parameters("@PI_VC8_Sex").Value = "Male"
        ElseIf objDataset.Tables(0).Rows(count)("Gender").ToString() = "F" Then
            objComm.Parameters("@PI_VC8_Sex").Value = "Female"
        Else
            objComm.Parameters("@PI_VC8_Sex").Value = ""
        End If

        objComm.Parameters.Add("@PI_DT8_Date_Of_Joining", SqlDbType.DateTime)
        objComm.Parameters("@PI_DT8_Date_Of_Joining").Value = Convert.ToDateTime(objDataset.Tables(0).Rows(count)("DOJ").ToString())

        objComm.Parameters.Add("@PI_VC8_Marital_Status", SqlDbType.VarChar, 8)
        If String.IsNullOrEmpty(objDataset.Tables(0).Rows(count)("Marriage Anniversary").ToString()) Then
            objComm.Parameters("@PI_VC8_Marital_Status").Value = "Single"
        Else
            objComm.Parameters("@PI_VC8_Marital_Status").Value = "Married"
        End If

        objComm.Parameters.Add("@PI_VC15_BloodGroup", SqlDbType.VarChar, 18)
        objComm.Parameters("@PI_VC15_BloodGroup").Value = objDataset.Tables(0).Rows(count)("Blood Group").ToString()

        objComm.Parameters.Add("@PI_VC50_PassportNo", SqlDbType.VarChar, 18)
        objComm.Parameters("@PI_VC50_PassportNo").Value = objDataset.Tables(0).Rows(count)("Passport No#").ToString()

        objComm.Parameters.Add("@PI_VC8_Department", SqlDbType.VarChar, 8)
        objComm.Parameters("@PI_VC8_Department").Value = CheckMasterCodes(objDataset.Tables(0).Rows(count)("Department").ToString(), "DPT", strConnectionString)

        objComm.Parameters.Add("@PI_NU9_JobRole", SqlDbType.VarChar, 100)

        objComm.Parameters("@PI_NU9_JobRole").Value = objDataset.Tables(0).Rows(count)("Designation").ToString()

        objComm.Parameters.Add("@PI_VC30_Role", SqlDbType.VarChar, 8)
        objComm.Parameters("@PI_VC30_Role").Value = CheckMasterCodes(objDataset.Tables(0).Rows(count)("Designation").ToString(), "JOBR", strConnectionString)
        'objDataset.Tables(0).Rows(count)("Designation").ToString().Replace(" ", "")

        Dim arrstr As String() = Nothing
        arrstr = objDataset.Tables(0).Rows(count)("Name").ToString().Split(" "c)

        objComm.Parameters.Add("@PI_VC36_First_Name", SqlDbType.VarChar, 36)
        If arrstr.Length > 0 Then
            objComm.Parameters("@PI_VC36_First_Name").Value = arrstr(0)
        Else
            objComm.Parameters("@PI_VC36_First_Name").Value = ""
        End If


        objComm.Parameters.Add("@PI_VC36_Middle_Name", SqlDbType.VarChar, 36)
        objComm.Parameters.Add("@PI_VC36_Last_Name", SqlDbType.VarChar, 36)
        If arrstr.Length > 2 Then
            objComm.Parameters("@PI_VC36_Middle_Name").Value = arrstr(1)
            objComm.Parameters("@PI_VC36_Last_Name").Value = arrstr(2)
        ElseIf arrstr.Length = 2 Then
            objComm.Parameters("@PI_VC36_Middle_Name").Value = ""
            objComm.Parameters("@PI_VC36_Last_Name").Value = arrstr(1)
        Else
            objComm.Parameters("@PI_VC36_Middle_Name").Value = ""
            objComm.Parameters("@PI_VC36_Last_Name").Value = ""
        End If

        objComm.Parameters.Add("@pi_vc4_timezone", SqlDbType.VarChar, 100)
        objComm.Parameters("@pi_vc4_timezone").Value = "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi  [India Standard Time]"

        objComm.ExecuteNonQuery()

        objConn.Close()
        objConn.Close()
        GenratePassword(intTableIdHold, strConnectionString, count)

    End Sub


    ''' <summary>
    ''' Genrate user-id and Password
    ''' </summary>
    ''' <param name="objinput"></param>
    ''' <param name="intTableId"></param>
    ''' <param name="strConnectionString"></param>
    ''' <remarks></remarks>
    Public Sub GenratePassword(ByVal intTableId As Integer, ByVal strConnectionString As String, ByVal count As Integer)
        Dim objConn As SqlConnection = Nothing
        Dim objComm As SqlCommand = Nothing
        Dim strCommand As String = String.Empty
        Dim intRecordCount As Integer = 0
        Dim blnSucess As Boolean = False
        Dim blnInserted As Boolean = False
        Dim intUserId As Integer = 0
        ''sDim strUserId As String = String.Empty
        Dim strUserIdFixed As String = String.Empty
        Dim intTableIdSend As Integer = 0

        intTableIdSend = intTableId
        objConn = New SqlConnection(strConnectionString)


        intUserId = 0
        strUserIdFixed = GetUserID(objDataset.Tables(0).Rows(count)("Name").ToString())
        blnSucess = False
        While blnSucess = False
            strCommand = "Select count(*) from T060011 where UM_VC50_UserID =@UM_VC50_UserID"
            objConn = New SqlConnection(strConnectionString)
            objComm = New SqlCommand(strCommand, objConn)
            objConn.Open()
            objComm.Parameters.Add("@UM_VC50_UserID", SqlDbType.VarChar)
            If intUserId <> 0 Then
                strUserId = strUserIdFixed & Convert.ToString(intUserId)
            Else
                strUserId = strUserIdFixed
            End If
            objComm.Parameters("@UM_VC50_UserID").Value = strUserId
            Dim res As Integer = objComm.ExecuteScalar()
            If res < 1 Then
                blnSucess = True
            End If
            objConn.Close()
            intUserId = intUserId + 1
        End While

        strCommand = "INSERT INTO T060011(UM_VC50_Access_Card_No,um_dt8_inserted_on,um_in4_lockout_tries,um_in4_expiry_days,UM_VC4_Status_Code_FK,UM_CH1_Mail_Sent_Modify,UM_CH1_Mail_Sent_Expiry,UM_NU9_Expiry_Mail_Days,UM_DT8_Status_Date,UM_DT8_Created_Date,UM_IN4_Address_No_FK,UM_VC50_UserID,UM_VC30_Password,UM_IN4_Company_AB_ID,UM_DT8_From_date,UM_DT8_To_date)     VALUES"
        strCommand = strCommand & "(@UM_VC50_Access_Card_No,@um_dt8_inserted_on,@um_in4_lockout_tries,@um_in4_expiry_days,@UM_VC4_Status_Code_FK,@UM_CH1_Mail_Sent_Modify,@UM_CH1_Mail_Sent_Expiry,@UM_NU9_Expiry_Mail_Days,@UM_DT8_Status_Date,@UM_DT8_Created_Date,@UM_IN4_Address_No_FK, @UM_VC50_UserID,@UM_VC30_Password,@UM_IN4_Company_AB_ID,@UM_DT8_From_date,@UM_DT8_To_date)"




        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        objComm.Parameters.Add("@UM_IN4_Address_No_FK", SqlDbType.Int)
        objComm.Parameters("@UM_IN4_Address_No_FK").Value = intTableId

        Dim arrstr As String() = Nothing
        arrstr = objDataset.Tables(0).Rows(count)("Name").ToString().Split(" "c)

        objComm.Parameters.Add("@UM_VC50_UserID", SqlDbType.VarChar, 50)
        objComm.Parameters("@UM_VC50_UserID").Value = strUserId

        objComm.Parameters.Add("@UM_VC30_Password", SqlDbType.VarChar, 30)
        objComm.Parameters("@UM_VC30_Password").Value = IONEncrypt("User@123")

        objComm.Parameters.Add("@UM_IN4_Company_AB_ID", SqlDbType.Int)
        objComm.Parameters("@UM_IN4_Company_AB_ID").Value = GetCompanyID(objDataset.Tables(0).Rows(count)("Business Relation").ToString(), strConnectionString)

        objComm.Parameters.Add("@UM_DT8_From_date", SqlDbType.DateTime)
        objComm.Parameters("@UM_DT8_From_date").Value = DateTime.Now.AddDays(-1)


        objComm.Parameters.Add("@UM_DT8_To_date", SqlDbType.DateTime)
        objComm.Parameters("@UM_DT8_To_date").Value = DateTime.Now.AddMonths(12)

        objComm.Parameters.Add("@UM_DT8_Created_Date", SqlDbType.DateTime)
        objComm.Parameters("@UM_DT8_Created_Date").Value = System.DateTime.Now

        objComm.Parameters.Add("@UM_DT8_Status_Date", SqlDbType.DateTime)
        objComm.Parameters("@UM_DT8_Status_Date").Value = System.DateTime.Now

        objComm.Parameters.Add("@UM_NU9_Expiry_Mail_Days", SqlDbType.Int)
        objComm.Parameters("@UM_NU9_Expiry_Mail_Days").Value = 1

        objComm.Parameters.Add("@UM_CH1_Mail_Sent_Modify", SqlDbType.[Char], 1)
        objComm.Parameters("@UM_CH1_Mail_Sent_Modify").Value = "F"

        objComm.Parameters.Add("@UM_CH1_Mail_Sent_Expiry", SqlDbType.[Char], 1)
        objComm.Parameters("@UM_CH1_Mail_Sent_Expiry").Value = "F"

        objComm.Parameters.Add("@UM_VC4_Status_Code_FK", SqlDbType.VarChar, 4)
        objComm.Parameters("@UM_VC4_Status_Code_FK").Value = "ENB"

        objComm.Parameters.Add("@um_in4_expiry_days", SqlDbType.Int)
        objComm.Parameters("@um_in4_expiry_days").Value = 365

        objComm.Parameters.Add("@um_in4_lockout_tries", SqlDbType.Int)
        objComm.Parameters("@um_in4_lockout_tries").Value = 3

        objComm.Parameters.Add("@um_dt8_inserted_on", SqlDbType.DateTime)
        objComm.Parameters("@um_dt8_inserted_on").Value = System.DateTime.Now

        objComm.Parameters.Add("@UM_VC50_Access_Card_No", SqlDbType.VarChar, 50)
        objComm.Parameters("@UM_VC50_Access_Card_No").Value = objDataset.Tables(0).Rows(count)("Access Card No").ToString()

        objComm.ExecuteNonQuery()
        blnInserted = True
        objConn.Close()
        'objinput.pCSVData(intRecordCount).Uid = strUserId;
        'objinput.pCSVData(intRecordCount).Pwd = "User@123";

        CompanyAccess(intTableIdSend, strConnectionString, count)


    End Sub

    ''' <summary>
    ''' Get user Id
    ''' </summary>
    ''' <param name="strName"></param>
    ''' <returns></returns>
    Public Function GetUserID(ByVal strName As String) As String
        Dim strSplit As String() = Nothing
        Try
            strSplit = strName.Split(" "c)

            If strSplit.Length > 2 Then
                Return strSplit(0).Substring(0, 1) & strSplit(2)
            ElseIf strSplit.Length > 1 Then
                Return strSplit(0).Substring(0, 1) & strSplit(1)
            Else
                Return strSplit(0)

            End If
        Catch ex As Exception
            Throw ex
        Finally
            strSplit = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Encrypt Password before storing in DB
    ''' </summary>
    ''' <param name="strPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IONEncrypt(ByVal strPassword As String) As String

        Dim bytKey As Byte() = Nothing
        Dim intLoop1 As Integer = 0
        Dim intLoop2 As Integer = 0
        Dim strCharNew As String = String.Empty
        Try
            bytKey = Encoding.ASCII.GetBytes(strPassword)
            intLoop2 = bytKey.Length
            'multiply each byte by 2 
            For intLoop1 = 0 To bytKey.Length - 1
                Dim newbyte As Int32 = Nothing
                newbyte = bytKey(intLoop1)
                newbyte = newbyte * 2
                If newbyte = 160 Then
                    newbyte = 161
                End If
                If newbyte = 128 Then
                    newbyte = 129
                End If
                bytKey(intLoop1) = newbyte
            Next
            'declare a new string for the password to return 

            'loop for converting the byte in char which will be new string 
            For inti As Integer = 0 To bytKey.Length - 1
                Dim s As Char = Convert.ToChar(bytKey(inti))
                strCharNew = strCharNew & s
            Next
            'return the ne string 
            Return strCharNew
        Catch ex As Exception
            Throw ex

        Finally
        End Try
    End Function

    ''' <summary>
    ''' Genrate Employee ID
    ''' </summary>
    ''' <param name="strEmpType"></param>
    ''' <param name="strConnectionString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenrateEmpID(ByVal strEmpType As String, ByVal strConnectionString As String) As String
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        Dim intID As Integer
        Dim strID As String
        Try
            objConn = New SqlConnection(strConnectionString)
            strCommand = "Select max(cast(case when len(ci_vc36_id_1)>5 then right(ci_vc36_id_1,3) else right(ci_vc36_id_1,2) end as integer)) +1 from T010011 where left(ci_vc36_id_1,3) = @EmpType"
            objComm = New SqlCommand(strCommand, objConn)
            objConn.Open()
            objComm.Parameters.Add("@EmpType", SqlDbType.VarChar, 3)
            objComm.Parameters("@EmpType").Value = strEmpType.Substring(0, 3)
            strID = objComm.ExecuteScalar
            objConn.Close()
            If strID <> "" Then
                intID = Convert.ToInt32(strID)
                Return strEmpType.Substring(0, 3) & intID.ToString("000")
            Else
                Return strEmpType.Substring(0, 3) & "001"
            End If
        Catch ex As Exception
            Return strEmpType.Substring(0, 3) & "-" & "001"
        Finally
            objConn = Nothing
            objComm = Nothing
            strCommand = Nothing
            intID = Nothing
            strID = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Give access to company
    ''' </summary>
    ''' <param name="objinput"></param>
    ''' <param name="intTableId"></param>
    ''' <param name="strConnectionString"></param>
    ''' <remarks></remarks>
    Public Sub CompanyAccess(ByVal intTableId As Integer, ByVal strConnectionString As String, ByVal count As Int32)
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        Dim intRecordCount As Integer = 0
        Dim blnInserted As Boolean = False
        Dim intCompanyId As Integer
        Dim intTableIDSend As Integer
        Dim intTableIDSendBase As Integer

        intTableIDSendBase = intTableId
        'strCommand = "Select max(UC_NU9_ID_PK) from T060041"
        'objConn = New SqlConnection(strConnectionString)
        'objComm = New SqlCommand(strCommand, objConn)
        'objConn.Open()

        'intTableIDSend = objComm.ExecuteScalar() + 1
        'objConn.Close()

        objConn = New SqlConnection(strConnectionString)
        strCommand = "Select CI_IN4_Business_Relation from T010011 where CI_NU8_Address_Number=@CI_NU8_Address_Number"
        objConn = New SqlConnection(strConnectionString)
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        objComm.Parameters.Add("@CI_NU8_Address_Number", SqlDbType.Int)
        objComm.Parameters("@CI_NU8_Address_Number").Value = intTableId
        intCompanyId = objComm.ExecuteScalar()
        objConn.Close()
        If strConnectionString.Contains("WSS_ATT") = True Then
            If objDataset.Tables(0).Rows(count)("Department").ToString().ToUpper.Contains("ERGO") Then
                intCompanyId = 147
            End If
            If objDataset.Tables(0).Rows(count)("Department").ToString().ToUpper.Contains("NORMAN") Then
                intCompanyId = 138
            End If
        End If
        strCommand = "INSERT INTO T060041 (UC_NU9_User_ID_FK,UC_NU9_Comp_ID_FK,UC_BT1_Access,UC_DT8_Insert_Date)"
        strCommand = strCommand + "VALUES(@UC_NU9_User_ID_FK, @UC_NU9_Comp_ID_FK, @UC_BT1_Access, @UC_DT8_Insert_Date)"

        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        'objComm.Parameters.Add("@UC_NU9_ID_PK", SqlDbType.Int)
        'objComm.Parameters("@UC_NU9_ID_PK").Value = intTableIDSend

        objComm.Parameters.Add("@UC_NU9_User_ID_FK", SqlDbType.Int)
        objComm.Parameters("@UC_NU9_User_ID_FK").Value = intTableId

        objComm.Parameters.Add("@UC_NU9_Comp_ID_FK", SqlDbType.Int)
        objComm.Parameters("@UC_NU9_Comp_ID_FK").Value = intCompanyId

        objComm.Parameters.Add("@UC_BT1_Access", SqlDbType.Int)
        objComm.Parameters("@UC_BT1_Access").Value = 1

        objComm.Parameters.Add("@UC_DT8_Insert_Date", SqlDbType.DateTime)
        objComm.Parameters("@UC_DT8_Insert_Date").Value = Date.Now

        objComm.ExecuteNonQuery()
        blnInserted = True
        objConn.Close()
        objConn.Close()
        InsertRole(intTableIDSendBase, strConnectionString, count)

    End Sub

    ''' <summary>
    ''' Insert Role
    ''' </summary>
    ''' <param name="objinput"></param>
    ''' <param name="intBaseUserID"></param>
    ''' <param name="strConnectionString"></param>
    ''' <remarks></remarks>
    Public Sub InsertRole(ByVal intBaseUserID As Integer, ByVal strConnectionString As String, ByVal count As Int32)
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        Dim intRecordCount As Integer = 0
        Dim blnInserted As Boolean = False
        Dim intRoleid As Integer
        Dim intTableIDSend As Integer
        Dim intTableIDSendBase As Integer


        strCommand = "Select max(RA_IN4_User_Role_ID_PK) from T060022"
        objConn = New SqlConnection(strConnectionString)
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        intTableIDSendBase = objComm.ExecuteScalar() + 1
        intTableIDSend = intTableIDSendBase
        objConn.Close()

        objConn = New SqlConnection(strConnectionString)


        strCommand = "Select ROM_IN4_Role_ID_PK from t070031 where ROM_VC50_Role_Name =@ROM_VC50_Role_Name"
        objConn = New SqlConnection(strConnectionString)
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        objComm.Parameters.Add("@ROM_VC50_Role_Name", SqlDbType.VarChar)
        If strConnectionString.Contains("WSS_ATT") = True Then
            objComm.Parameters("@ROM_VC50_Role_Name").Value = objDataset.Tables(0).Rows(count)("Role WSS").ToString()
        ElseIf strConnectionString.Contains("WSS_SPOC") = True Then
            objComm.Parameters("@ROM_VC50_Role_Name").Value = objDataset.Tables(0).Rows(count)("Role SPOC").ToString()
        ElseIf strConnectionString.Contains("WSS_incidence") = True Then
            objComm.Parameters("@ROM_VC50_Role_Name").Value = objDataset.Tables(0).Rows(count)("Role Incidence").ToString()
        End If
        intRoleid = objComm.ExecuteScalar()
        objConn.Close()

        strCommand = "INSERT INTO T060022 (RA_IN4_AB_ID_FK,RA_IN4_Role_ID_FK,RA_DT8_Assigned_Date,RA_DT8_Valid_UpTo,RA_IN4_Assigned_By_FK,RA_VC4_Status_Code,RA_DT8_Status_Date,RA_IN4_Inserted_By_FK,RA_DT8_Inserted_On)"
        strCommand = strCommand + "VALUES(@RA_IN4_AB_ID_FK,@RA_IN4_Role_ID_FK,@RA_DT8_Assigned_Date,@RA_DT8_Valid_UpTo,@RA_IN4_Assigned_By_FK,@RA_VC4_Status_Code,@RA_DT8_Status_Date,@RA_IN4_Inserted_By_FK,@RA_DT8_Inserted_On)"

        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        objComm.Parameters.Add("@RA_IN4_User_Role_ID_PK", SqlDbType.Int)
        objComm.Parameters("@RA_IN4_User_Role_ID_PK").Value = intTableIDSend

        objComm.Parameters.Add("@RA_IN4_AB_ID_FK", SqlDbType.Int)
        objComm.Parameters("@RA_IN4_AB_ID_FK").Value = intBaseUserID

        objComm.Parameters.Add("@RA_IN4_Role_ID_FK", SqlDbType.Int)
        objComm.Parameters("@RA_IN4_Role_ID_FK").Value = intRoleid

        objComm.Parameters.Add("@RA_DT8_Assigned_Date", SqlDbType.DateTime)
        objComm.Parameters("@RA_DT8_Assigned_Date").Value = DateAdd(DateInterval.Day, -1, Date.Now)

        objComm.Parameters.Add("@RA_DT8_Valid_UpTo", SqlDbType.DateTime)
        objComm.Parameters("@RA_DT8_Valid_UpTo").Value = DateAdd(DateInterval.Year, 1, Date.Now)

        objComm.Parameters.Add("@RA_IN4_Assigned_By_FK", SqlDbType.Int, 4)
        objComm.Parameters("@RA_IN4_Assigned_By_FK").Value = 14

        objComm.Parameters.Add("@RA_VC4_Status_Code", SqlDbType.VarChar, 4)
        objComm.Parameters("@RA_VC4_Status_Code").Value = "ENB"

        objComm.Parameters.Add("@RA_DT8_Status_Date", SqlDbType.DateTime)
        objComm.Parameters("@RA_DT8_Status_Date").Value = Date.Now

        objComm.Parameters.Add("@RA_IN4_Inserted_By_FK", SqlDbType.Int)
        objComm.Parameters("@RA_IN4_Inserted_By_FK").Value = 12

        objComm.Parameters.Add("@RA_DT8_Inserted_On", SqlDbType.DateTime)
        objComm.Parameters("@RA_DT8_Inserted_On").Value = Date.Now
        objComm.ExecuteNonQuery()
        blnInserted = True
        objConn.Close()
        objConn.Close()
        InsertSubCategory(intBaseUserID, strConnectionString, count)

    End Sub

    ''' <summary>
    ''' Insert Sub-Category
    ''' </summary>
    ''' <param name="objinput"></param>
    ''' <param name="intBaseUserID"></param>
    ''' <param name="strConnectionString"></param>
    ''' <remarks></remarks>
    Public Sub InsertSubCategory(ByVal intBaseUserID As Integer, ByVal strConnectionString As String, ByVal count As Int32)
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        Dim intRecordCount As Integer = 0
        Dim blnInserted As Boolean = False
        Dim strProjectCompany As String
        Dim intTableIDSend As Integer
        Dim intTableIDSendBase As Integer


        strCommand = "Select max(PM_NU9_ID_PK) from T210012"
        objConn = New SqlConnection(strConnectionString)
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        intTableIDSendBase = objComm.ExecuteScalar() + 1
        intTableIDSend = intTableIDSendBase
        objConn.Close()

        objConn = New SqlConnection(strConnectionString)


        strCommand = "select cast(pr_nu9_project_id_pk as varchar) + '~' + cast(PR_NU9_Comp_ID_FK as varchar) from T210011 a,T010011 b where a.pr_vc20_name=@pr_vc20_name and b.CI_VC36_Name=@CI_VC36_Name"
        objConn = New SqlConnection(strConnectionString)
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        objComm.Parameters.Add("@pr_vc20_name", SqlDbType.VarChar)
        If strConnectionString.Contains("WSS_ATT") = True Then
            objComm.Parameters("@pr_vc20_name").Value = objDataset.Tables(0).Rows(count)("Subcategory WSS").ToString()
        ElseIf strConnectionString.Contains("WSS_SPOC") = True Then
            objComm.Parameters("@pr_vc20_name").Value = objDataset.Tables(0).Rows(count)("Subcategory SPOC").ToString()
        ElseIf strConnectionString.Contains("WSS_incidence") = True Then
            objComm.Parameters("@pr_vc20_name").Value = objDataset.Tables(0).Rows(count)("Subcategory Incidence").ToString()
        End If

        objComm.Parameters.Add("@CI_VC36_Name", SqlDbType.VarChar)
        If strConnectionString.Contains("WSS_ATT") = True Then
            objComm.Parameters("@CI_VC36_Name").Value = objDataset.Tables(0).Rows(count)("WSS Company").ToString()
            'ElseIf strConnectionString.Contains("WSS_SPOC") = True Then
            '    objComm.Parameters("@CI_VC36_Name").Value = "ION"
        Else
            objComm.Parameters("@CI_VC36_Name").Value = "ION"
        End If
        strProjectCompany = objComm.ExecuteScalar()
        Dim s() As String
        s = strProjectCompany.Split("~")
        objConn.Close()

        strCommand = "INSERT INTO T210012 (PM_NU9_ID_PK,PM_NU9_Project_ID_Fk,PM_NU9_Comp_ID_FK,PM_NU9_Project_Member_ID,PM_NU9_Role,PM_NU9_Reports_To)"
        strCommand = strCommand + "VALUES (@PM_NU9_ID_PK,@PM_NU9_Project_ID_Fk,@PM_NU9_Comp_ID_FK,@PM_NU9_Project_Member_ID,@PM_NU9_Role,@PM_NU9_Reports_To)"

        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()

        objComm.Parameters.Add("@PM_NU9_ID_PK", SqlDbType.Int)
        objComm.Parameters("@PM_NU9_ID_PK").Value = intTableIDSend

        objComm.Parameters.Add("@PM_NU9_Project_ID_Fk", SqlDbType.Int)
        objComm.Parameters("@PM_NU9_Project_ID_Fk").Value = s(0)

        objComm.Parameters.Add("@PM_NU9_Comp_ID_FK", SqlDbType.Int)
        objComm.Parameters("@PM_NU9_Comp_ID_FK").Value = s(1)

        objComm.Parameters.Add("@PM_NU9_Project_Member_ID", SqlDbType.Int)
        objComm.Parameters("@PM_NU9_Project_Member_ID").Value = intBaseUserID

        objComm.Parameters.Add("@PM_NU9_Role", SqlDbType.Int)
        objComm.Parameters("@PM_NU9_Role").Value = 217

        objComm.Parameters.Add("@PM_NU9_Reports_To", SqlDbType.Int)
        objComm.Parameters("@PM_NU9_Reports_To").Value = 47

        objComm.ExecuteNonQuery()
        blnInserted = True
        objConn.Close()
        objConn.Close()
        Dim str As String
        If strConnectionString.Contains("WSS_ATT") = True Then
            str = "WSS"
            SendingMail(count, str)
            UpdateExcel(Convert.ToInt32(objDataset.Tables(0).Rows(count)("SNo")), EmpID__1)
        End If
        If strConnectionString.Contains("WSS_SPOC") = True Then
            str = "SPOC"
            SendingMail(count, str)
        End If
        If strConnectionString.Contains("WSS_incidence") = True Then
            str = "INCIDENCE"
            SendingMail(count, str)
        End If
        imgExportToExcel.Enabled = True
    End Sub
    Public Sub SendingMail(ByVal count As Integer, ByVal str As String)
        mailMsg.From = New MailAddress("HR@Ionnor.com")
        mailMsg.To.Add(objDataset.Tables(0).Rows(count)("Office Email").ToString())
        mailMsg.CC.Add("amit.dewan@ionnor.com")
        mailMsg.Subject = "Details"
        Dim htmlBody As New StringBuilder()
        htmlBody.Append("Hi User,<br/><br/>Your Account has been created in " + str + ".<br/>Following are the credentials to login <br/><br/> <b>UserName:</b> " + strUserId + " <br/> <b>Password:</b> User@123 <br/><br/> Best Regards,<br/>HR-Admin")



        'htmlBody.Append("<Table   cellpadding='1' border='1'>")
        'htmlBody.Append("<tr>")
        'htmlBody.Append("<td colspan='2' align='center' style='font-size:medium;font-weight:bold' bgColor='#788798'>")
        'htmlBody.Append("Your Accout has been created in " + str + "  ")
        'htmlBody.Append("</td>")
        'htmlBody.Append("</tr>")
        'htmlBody.Append("<tr>")
        'htmlBody.Append("<td style='font-weight:bold' bgColor='#B9C7D2'>")
        'htmlBody.Append("LoginID")
        'htmlBody.Append("</td>")
        'htmlBody.Append("<td bgColor='#B9C7D2'>")
        'htmlBody.Append(strUserId)
        'htmlBody.Append("</td>")
        'htmlBody.Append("</tr>")
        'htmlBody.Append("<tr>")
        'htmlBody.Append("<td style='font-weight:bold' bgColor='#B9C7D2'>")
        'htmlBody.Append("Password")
        'htmlBody.Append("</td>")
        'htmlBody.Append("<td bgColor='#B9C7D2'>")
        'htmlBody.Append("User@123")
        'htmlBody.Append("</td>")
        'htmlBody.Append("</tr>")
        'htmlBody.Append("</Table>")
        mailMsg.Body = htmlBody.ToString()
        mailMsg.IsBodyHtml = True
        'mailMsg.Body = "Your Accout has been created in " + str + " " + System.Environment.NewLine + "" + "Your Login ID:" + strUserId + "" + System.Environment.NewLine + "" + "Your Password is:Ionuser@123"
        SmtpServer.Host = "mail.ionnor.com"
        SmtpServer.Send(mailMsg)
        mailMsg.To.Clear()
        mailMsg.CC.Clear()
    End Sub
    ''' <summary>
    ''' To get Emp Id for spoc and incidence...
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetEmpIdFromWSS(ByVal p1 As String) As String
        Dim objConn As SqlConnection
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        objConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        strCommand = "select CI_VC36_ID_1 from t010011 where CI_VC36_Name='" & p1 & "' order by CI_NU8_Address_Number desc"
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        Dim strID As String = objComm.ExecuteScalar
        objConn.Close()
        Return strID
    End Function

    ''' <summary>
    ''' Funtion to insert Designation if desgnation does't Exist
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <param name="strConnectionstring"></param>
    ''' <remarks></remarks>
    Private Sub CheckDesignation(ByVal p1 As String, ByVal strConnectionstring As String)
        Dim objConn As SqlConnection
        Dim blnSucess As Boolean = False
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        objConn = New SqlConnection(strConnectionstring)
        strCommand = "select count(*) from UDC where UDCType='JOBR' and Description=@Designation"
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        objComm.Parameters.Add("@Designation", SqlDbType.VarChar)
        objComm.Parameters("@Designation").Value = p1
        Dim strID As Int16 = objComm.ExecuteScalar
        objConn.Close()
        If Not strID > 0 Then
            Dim desName As String
            Dim len As Int32
            len = 7
            desName = p1.Replace(" ", "")
            desName = desName.Substring(0, len)
            blnSucess = False
            While blnSucess = False
                strCommand = "select count(*) from udc where Name=@desName and UDCType='JOBR'"
                objConn = New SqlConnection(strConnectionstring)
                objComm = New SqlCommand(strCommand, objConn)
                objConn.Open()
                objComm.Parameters.Add("@desName", SqlDbType.VarChar)

                objComm.Parameters("@desName").Value = desName
                Dim res As Integer = objComm.ExecuteScalar()
                If res < 1 Then
                    blnSucess = True
                Else
                    len = len - 1
                    desName = desName.Substring(0, len)
                End If
                objConn.Close()
            End While
            strCommand = "insert into udc (ProductCode,UDCType,Description,name,Company) values (0,'JOBR','" & p1 & "','" & desName & "','0')"
            objComm = New SqlCommand(strCommand, objConn)
            objConn.Open()
            objComm.ExecuteNonQuery()
        End If
    End Sub

    ''' <summary>
    ''' Function to insert Department id department does't Exist
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <param name="strConnectionstring"></param>
    ''' <remarks></remarks>
    Private Sub CheckDepartment(ByVal p1 As String, ByVal strConnectionstring As String)
        Dim objConn As SqlConnection
        Dim blnSucess As Boolean = False
        Dim objComm As SqlCommand
        Dim strCommand As String = String.Empty
        objConn = New SqlConnection(strConnectionstring)
        strCommand = "select count(*) from UDC where UDCType='DPT' and Description=@Department"
        objComm = New SqlCommand(strCommand, objConn)
        objConn.Open()
        objComm.Parameters.Add("@Department", SqlDbType.VarChar)
        objComm.Parameters("@Department").Value = p1
        Dim strID As Int16 = objComm.ExecuteScalar
        objConn.Close()
        If Not strID > 0 Then
            Dim depName As String
            Dim len As Int32
            len = 7
            depName = p1.Replace(" ", "")
            If depName.Length >= len Then
                depName = depName.Substring(0, len)
            End If

            blnSucess = False
            While blnSucess = False
                strCommand = "select count(*) from udc where Name=@depName and UDCType='DPT'"
                objConn = New SqlConnection(strConnectionstring)
                objComm = New SqlCommand(strCommand, objConn)
                objConn.Open()
                objComm.Parameters.Add("@depName", SqlDbType.VarChar)

                objComm.Parameters("@depName").Value = depName
                Dim res As Integer = objComm.ExecuteScalar()
                If res < 1 Then
                    blnSucess = True
                Else
                    len = len - 1
                    depName = depName.Substring(0, len)
                End If
                objConn.Close()
            End While
            strCommand = "insert into udc (ProductCode,UDCType,Description,name,Company) values (0,'dpt','" & p1 & "','" & depName & "','0')"
            objComm = New SqlCommand(strCommand, objConn)
            objConn.Open()
            objComm.ExecuteNonQuery()
        End If
    End Sub

End Class
