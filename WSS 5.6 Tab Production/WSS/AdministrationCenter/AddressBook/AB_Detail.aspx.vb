'*******************************************************************
' Page                   : - Ab_Detail"address book view screen"
' Purpose                : - It is meant for editing the employee information such as personal details, passport details, etc
' Date					Author	 			Modification Date					Description
' 31/03/11			Amit Dewan 					-------------------					Created
'
' Notes: 
' Code:
'*******************************************************************
Imports ION.Data
Imports ION.Logging
Imports System.Web.Security
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports Microsoft.Win32

Partial Class AdministrationCenter_AddressBook_AB_Detail
    Inherits System.Web.UI.Page

#Region " Form Level Variables "
    'Local Variables used on the screen.
    Shared UserAddId As Integer 'Stores ID of the user.
    Dim dtRelations As New DataTable() 'Stores the relationsDropdownlist items.
    Dim dtBloodGrps As New DataTable() 'Stores the blood groups if a DropdownList is used(not used currently).
    Dim dtRelationFill As New DataTable() 'Stores Family Relations of user.
    Shared dsPersonalInfo As New DataSet() 'Stores all the personal info fetched from tables.
    Dim dsGridInfo1 As New DataSet() 'Temporary dataset to store FAMILY Details'
    Dim dsGridInfo2 As New DataSet() 'Temporary dataset to store SIBLING Details.
    Dim dsGridInfo3 As New DataSet() 'Temporary dataset to store CHILD Details.
    Dim lstError As New ListBox 'Stores message to be displayed.
    Private mintAddressKey As Integer ' Will keep Contact key and used to store contact key in HttpContext.Current.Session
    Private mintAddSkill As Boolean 'Stores State Whether Skill is to Added or Updated
    Shared strValidity As String
#End Region

#Region "Page Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        dtAnniversary.MinDate = CDate("1900-1-1")
        dtPassportIssueDate.MinDate = CDate("1900-1-1")
        dtVehicleInsuranceExpiry.MinDate = CDate("1900-1-1")
        dtPassportExpiry.MinDate = CDate("1900-1-1")
        dtRelationDOB.MinDate = CDate("1900-1-1")

        If Not IsNothing(Request.QueryString("AddressNo")) Then
            ViewState("SAddressNumber_AddressBook") = Request.QueryString("AddressNo")
            UserAddId = Request.QueryString("AddressNo")
        Else
            UserAddId = Session("PropUserID")
        End If
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        If Not IsPostBack Then
            FillRelations()
            FillBloodGrp()
            FillRelationsTable()
            PersonalInfo()
            ddlRelation_SelectedIndexChanged()
        End If
        If Not IsNothing(Request.QueryString("AddressNo")) Then
            ViewState("SAddressNumber_AddressBook") = Request.QueryString("AddressNo")
        End If
        'Hide the plus button when page is opening in edit mode
        If ViewState("SAddressNumber_AddressBook") <> "-1" Then
        End If
        ' -- mintAddressKey will be -1 for add and 0 for no change else for edit
        Dim txthiddenGrid As String = Request.Form("txthiddenGrid")
        Dim txthiddenSkil As String = Request.Form("txthiddenSkil")
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        Dim txthidden As String = Request.Form("txthidden")
        Dim intID As Integer
        'Security Block
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
            'End of Security Block
        End If
        If IsNothing(Request.QueryString("AddressNo")) = True And IsNothing(ViewState("SAddressNumber_AddressBook")) = True Then
            mintAddressKey = -1
        Else
            mintAddressKey = CInt(ViewState("SAddressNumber_AddressBook"))
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                CreateLog("AB_Detail", "Load-345", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
    End Sub
#End Region
#Region "Methods and Functions"
    '------NOTE:Code commented in this portion can be used if textbox or dropdownlist is required in case of the current control being used.------

    Public Sub PersonalInfo() 'Select personal information from tables and filling in controls.
        Try
            dsPersonalInfo.Clear()
            dsPersonalInfo.Dispose()
            'Dim strSql = "select CI_VC36_ID_1,CI_VC36_Name,PI_VC15_BloodGroup,PI_DT8_Date_Of_Birth,PI_VC8_Marital_Status,PI_DT8_Date_Of_Joining,PI_VC8_Sex,PI_DT_MarAnniversary,PI_VC50_HighestQualification from T010043 T1 inner join T010011 as T2 on T1.PI_NU8_Address_No=T2.CI_NU8_Address_Number where T1.PI_NU8_Address_No=" & UserAddId
            Dim strSql = "select CI_VC36_ID_1,CI_VC36_Name,PI_VC36_First_Name,PI_VC36_Middle_Name,PI_VC36_Last_Name,CI_NU16_Phone_Number_1,CI_VC28_Email_1,T2.CI_VC36_Address_Line_1 + T2.CI_VC36_Address_Line_2 + T2.CI_VC36_Address_Line_3 as [CurrentAddress],PI_VC500_PermanentAddress,Country_of_Birth,UM_VC16_Employee_Designation,PI_VC100_FatherName,EmpFatherDOB,EmpFatherCountry,EmpMotherName,EmpMotherDOB,EmpMotherCountry,EmpSpouseName,EmpSpouseDOB,EmpSpouseCountry,PI_VC15_BloodGroup,PI_DT8_Date_Of_Birth,PI_VC8_Marital_Status,PI_DT8_Date_Of_Joining,PI_VC200_ESIDetails,PI_VC200_PFDetails,PI_VC8_Sex,PI_DT_MarAnniversary,PI_VC50_HighestQualification,BirthPlace,CI_VC8_Level,PI_VC50_Nationality,PI_IN4_ReportingTo,PI_VC8_Department,PI_VC100_PANNo,PI_VC50_PassportNo,EmpPassportDateValidUntil,EmpPassportType,EmpPassportIssueDate,EmpPassportIssuedBy,PI_VC100_VehicleType,PI_VC100_VehicleNumber,VehicleInsurance,VehicleInsuranceExpiry,PI_VC100_FoodPrefrence,OfficialNumber,SkillSet,Roles_Respon,(select CI_VC36_Name from T010011 where CI_NU8_Address_Number= (T4.ReportedToId)) as ReportingHead from  T010011 as T2 Left Join T010043 as  T1  on T1.PI_NU8_Address_No=T2.CI_NU8_Address_Number Left Join T060011 as T3 on T1.PI_NU8_Address_No = T3.UM_IN4_Address_No_FK Left Join tblReportedTo as T4 on T1.PI_NU8_Address_No=T4.EmpId where T1.PI_NU8_Address_No=" & UserAddId
            If SQL.Search("T010043", "SearchPerInfo", "GetInfo", strSql, dsPersonalInfo, "", "") Then
                txtName.Text = dsPersonalInfo.Tables(0).Rows(0)("CI_VC36_Name").ToString().Trim()
                txtEmpID.Text = dsPersonalInfo.Tables(0).Rows(0)("CI_VC36_ID_1").ToString().Trim()
                'ddlBloodGrp.DataSource = dtBloodGrps
                'ddlBloodGrp.DataValueField = "BloodGroups"
                'ddlBloodGrp.SelectedValue = dsPersonalInfo.Tables(0).Rows(0)("PI_VC15_BloodGroup").ToString().Trim()
                'ddlBloodGrp.DataBind()
                txtBloodGrp.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC15_BloodGroup").ToString().Trim()

                'dtDateOfBirth.Text = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Birth") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Birth")), IsTime.DateOnly)
                If dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Birth").ToString() <> "" Then
                    txtDateOfBirth.Text = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Birth") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Birth")), IsTime.DateOnly).Trim()
                End If


                'ddlMaritalStatus.Items.Add("Married")
                'ddlMaritalStatus.Items.Add("Single")
                ddlMaritalStatus.SelectedValue = dsPersonalInfo.Tables(0).Rows(0)("PI_VC8_Marital_Status").ToString().Trim()
                'txtMaritalStatus.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC8_Marital_Status").ToString().Trim()

                'dtDateOfJoining.Text = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Joining") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Joining")), IsTime.DateOnly)
                If dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Joining").ToString() <> "" Then
                    txtDateOfJoining.Text = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Joining") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Joining")), IsTime.DateOnly).Trim()
                End If


                'ddlGender.Items.Add("Male")
                'ddlGender.Items.Add("Female")
                'ddlGender.SelectedValue = dsPersonalInfo.Tables(0).Rows(0)("PI_VC8_Sex").ToString().Trim()
                txtGender.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC8_Sex").ToString().Trim()

                If dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary").ToString() <> "1/1/1900 12:00:00 AM" Then
                    dtAnniversary.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary")), IsTime.DateOnly)

                    'dtAnniversary.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary")), IsTime.DateOnly)
                    'If dtAnniversary.SelectedDate = "1900-Jan-01" Then
                    '    dtAnniversary.SelectedDate = ""
                    'End If
                    'txtAnniversary.Text = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary")), IsTime.DateOnly).Trim()
                End If
                txtQualification.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC50_HighestQualification").ToString().Trim()
                Dim BornDate As Date = SetDateFormat(dsPersonalInfo.Tables(0).Rows(0)("PI_DT8_Date_Of_Birth"), IsTime.DateOnly)
                Dim AgeInterval As TimeSpan = Now - BornDate
                Dim YearsAge As Integer = CInt(AgeInterval.TotalDays / 365)
                txtAge.Text = YearsAge.ToString().Trim()
                txtMobNumber.Text = dsPersonalInfo.Tables(0).Rows(0)("CI_NU16_Phone_Number_1").ToString().Trim()
                If txtMobNumber.Text = "0" Then
                    txtMobNumber.Text = ""
                End If
                txtEmail.Text = dsPersonalInfo.Tables(0).Rows(0)("CI_VC28_Email_1").ToString().Trim()
                txtPanNo.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC100_PANNo").ToString().Trim()
                txtPassportNo.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC50_PassportNo").ToString().Trim()
                If dsPersonalInfo.Tables(0).Rows(0)("EmpPassportDateValidUntil").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("EmpPassportDateValidUntil").ToString() <> "1/1/1900 12:00:00 AM" Then
                    dtPassportExpiry.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("EmpPassportDateValidUntil") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("EmpPassportDateValidUntil")), IsTime.DateOnly).Trim()
                    'If dtPassportExpiry.Text = "1900-Jan-01" Then
                    '    dtPassportExpiry.Text = ""
                    'End If
                End If
                txtPassportType.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpPassportType").ToString().Trim()
                If dsPersonalInfo.Tables(0).Rows(0)("EmpPassportIssueDate").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("EmpPassportIssueDate").ToString() <> "1/1/1900 12:00:00 AM" Then
                    dtPassportIssueDate.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("EmpPassportIssueDate") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("EmpPassportIssueDate")), IsTime.DateOnly).Trim()
                    'If dtPassportIssueDate.Text = "1900-Jan-01" Then
                    '    dtPassportIssueDate.Text = ""
                    'End If
                End If
                txtPassportIssuedBy.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpPassportIssuedBy").ToString().Trim()
                txtVehicleType.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC100_VehicleType").ToString().Trim()
                txtVehicleNo.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC100_VehicleNumber").ToString().Trim()
                txtVehicleInsurance.Text = dsPersonalInfo.Tables(0).Rows(0)("VehicleInsurance").ToString().Trim()
                If dsPersonalInfo.Tables(0).Rows(0)("VehicleInsuranceExpiry").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("VehicleInsuranceExpiry").ToString() <> "1/1/1900 12:00:00 AM" Then

                    dtVehicleInsuranceExpiry.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("VehicleInsuranceExpiry") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("VehicleInsuranceExpiry")), IsTime.DateOnly).Trim()
                    'If dtVehicleInsuranceExpiry.Text = "1900-Jan-01" Then
                    '    dtVehicleInsuranceExpiry.Text = ""
                    'End If
                End If
                txtCurrentAddress.Text = dsPersonalInfo.Tables(0).Rows(0)("CurrentAddress").ToString().Trim()
                txtPermanentAddress.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC500_PermanentAddress").ToString().Trim()
                txtFoodPreference.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC100_FoodPrefrence").ToString().Trim()
                ddlRelation.DataSource = dtRelations
                ddlRelation.DataValueField = "FamilyRelations"
                ddlRelation.DataBind()
                'ddlRelation.Items.Insert(0, "--Select--")
                txtFirstName.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC36_First_Name").ToString().Trim()
                txtMiddleName.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC36_Middle_Name").ToString().Trim()
                txtLastName.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC36_Last_Name").ToString().Trim()
                txtBirthPlace.Text = dsPersonalInfo.Tables(0).Rows(0)("BirthPlace").ToString().Trim()
                txtNationality.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC50_Nationality").ToString().Trim()
                txtCountry.Text = dsPersonalInfo.Tables(0).Rows(0)("Country_of_Birth").ToString().Trim()
                txtReportingHead.Text = dsPersonalInfo.Tables(0).Rows(0)("ReportingHead").ToString().Trim()
                txtDesignation.Text = dsPersonalInfo.Tables(0).Rows(0)("CI_VC8_Level").ToString().Trim()
                txtDepartment.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC8_Department").ToString().Trim()
                txtOfficialPhone.Text = dsPersonalInfo.Tables(0).Rows(0)("BirthPlace").ToString().Trim()
                txtESIDetail.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC200_ESIDetails").ToString().Trim()
                txtPFDetail.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC200_PFDetails").ToString().Trim()
                txtOfficialPhone.Text = dsPersonalInfo.Tables(0).Rows(0)("OfficialNumber").ToString().Trim()
                If txtOfficialPhone.Text = "0" Then
                    txtOfficialPhone.Text = ""
                End If
                txtSkillSet.Text = dsPersonalInfo.Tables(0).Rows(0)("SkillSet").ToString().Trim()
                txtRole.Text = dsPersonalInfo.Tables(0).Rows(0)("Roles_Respon").ToString().Trim()
                grdEmpInfo.DataSource = dtRelationFill 'Filling Family relations datagrid
                grdEmpInfo.DataBind()
            End If
        Catch ex As Exception
            CreateLog("AB_Detail", "FillPersonalInfo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "PersonalInfo", )
        End Try
    End Sub
    'Filling controls in Family Relation Controls when Edit button in DataGrid is clicked
    Public Sub EditRecord(ByVal s As Object, ByVal e As DataGridCommandEventArgs)
        ddlRelation.SelectedValue = grdEmpInfo.Items(e.Item.ItemIndex).Cells(0).Text.Trim()
        If grdEmpInfo.Items(e.Item.ItemIndex).Cells(1).Text.Trim() <> "&nbsp;" Then
            txtRelationName.Text = grdEmpInfo.Items(e.Item.ItemIndex).Cells(1).Text.Trim()
        Else
            txtRelationName.Text = ""
        End If
        If grdEmpInfo.Items(e.Item.ItemIndex).Cells(2).Text.Trim() <> "&nbsp;" Then
            'SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("PI_DT_MarAnniversary")), IsTime.DateOnly)
            dtRelationDOB.SelectedDate = SetDateFormat(IIf(grdEmpInfo.Items(e.Item.ItemIndex).Cells(2).Text.Trim() Is DBNull.Value, "", grdEmpInfo.Items(e.Item.ItemIndex).Cells(2).Text.Trim()), IsTime.DateOnly)
            'dtRelationDOB.SelectedDate = grdEmpInfo.Items(e.Item.ItemIndex).Cells(2).Text.Trim()
        Else
            dtRelationDOB.Clear()
        End If
        If grdEmpInfo.Items(e.Item.ItemIndex).Cells(3).Text.Trim() <> "&nbsp;" Then
            txtRelationCountry.Text = grdEmpInfo.Items(e.Item.ItemIndex).Cells(3).Text.Trim()
        Else
            txtRelationCountry.Text = ""
        End If
        If grdEmpInfo.Items(e.Item.ItemIndex).Cells(5).Text.Trim() <> "&nbsp;" Then
            TempTextBox.Text = grdEmpInfo.Items(e.Item.ItemIndex).Cells(5).Text.Trim()
        Else
            TempTextBox.Text = ""
        End If
    End Sub
    'selected index change of relations dropdownlist
    Public Sub ddlRelation_SelectedIndexChanged()
        txtRelationName.Text = ""
        dtRelationDOB.Clear()
        txtRelationCountry.Text = ""
        If ddlRelation.SelectedValue = "Father" Then
            txtRelationName.Text = dsPersonalInfo.Tables(0).Rows(0)("PI_VC100_FatherName").ToString().Trim()
            If dsPersonalInfo.Tables(0).Rows(0)("EmpFatherDOB").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("EmpFatherDOB").ToString() <> "1/1/1900 12:00:00 AM" Then
                dtRelationDOB.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("EmpFatherDOB") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("EmpFatherDOB")), IsTime.DateOnly).Trim()
            End If
            txtRelationCountry.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpFatherCountry").ToString().Trim()
        ElseIf ddlRelation.SelectedValue = "Mother" Then
            txtRelationName.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpMotherName").ToString().Trim()
            If dsPersonalInfo.Tables(0).Rows(0)("EmpMotherDOB").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("EmpMotherDOB").ToString() <> "1/1/1900 12:00:00 AM" Then
                dtRelationDOB.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("EmpMotherDOB") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("EmpMotherDOB")), IsTime.DateOnly).Trim()
            End If
            txtRelationCountry.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpMotherCountry").ToString().Trim()
        ElseIf ddlRelation.SelectedValue = "Spouse" Then
            txtRelationName.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpSpouseName").ToString().Trim()
            If dsPersonalInfo.Tables(0).Rows(0)("EmpSpouseDOB").ToString() <> "" And dsPersonalInfo.Tables(0).Rows(0)("EmpSpouseDOB").ToString() <> "1/1/1900 12:00:00 AM" Then
                dtRelationDOB.SelectedDate = SetDateFormat(IIf(dsPersonalInfo.Tables(0).Rows(0)("EmpSpouseDOB") Is DBNull.Value, "", dsPersonalInfo.Tables(0).Rows(0)("EmpSpouseDOB")), IsTime.DateOnly).Trim()
            End If
            txtRelationCountry.Text = dsPersonalInfo.Tables(0).Rows(0)("EmpSpouseCountry").ToString().Trim()
        Else
            txtRelationName.Text = ""
            dtRelationDOB.Clear()
            txtRelationCountry.Text = ""
        End If
    End Sub
    'list of blood Groups if dropdown is used(Currently Not used)
    Public Sub FillBloodGrp()
        Dim BloodGrpDataColumn As New DataColumn()
        BloodGrpDataColumn.ColumnName = "BloodGroups"
        dtBloodGrps.Columns.Add(BloodGrpDataColumn)
        dtBloodGrps.Rows.Add("A+")
        dtBloodGrps.Rows.Add("A-")
        dtBloodGrps.Rows.Add("B+")
        dtBloodGrps.Rows.Add("A-")
        dtBloodGrps.Rows.Add("AB+")
        dtBloodGrps.Rows.Add("AB-")
        dtBloodGrps.Rows.Add("O+")
        dtBloodGrps.Rows.Add("O-")
    End Sub
    'fill relations dropdown
    Public Sub FillRelations()
        Dim RelationDataColumn As New DataColumn()
        RelationDataColumn.ColumnName = "FamilyRelations"
        dtRelations.Columns.Add(RelationDataColumn)
        dtRelations.Rows.Add("--Select--")
        dtRelations.Rows.Add("Father")
        dtRelations.Rows.Add("Mother")
        dtRelations.Rows.Add("Spouse")
        dtRelations.Rows.Add("Sibling")
        dtRelations.Rows.Add("Child")
    End Sub
    'fill dtRelationFill Table according to data available in relations table as per family details
    Public Sub FillRelationsTable()
        Dim TempDataColumn1 As New DataColumn()
        TempDataColumn1.ColumnName = "Relation"
        Dim TempDataColumn2 As New DataColumn()
        TempDataColumn2.ColumnName = "Name"
        Dim TempDataColumn3 As New DataColumn()
        TempDataColumn3.ColumnName = "DOB"
        Dim TempDataColumn4 As New DataColumn()
        TempDataColumn4.ColumnName = "Country"
        Dim TempDataColumn5 As New DataColumn()
        TempDataColumn5.ColumnName = "SNo"
        dtRelationFill.Columns.Add(TempDataColumn1)
        dtRelationFill.Columns.Add(TempDataColumn2)
        dtRelationFill.Columns.Add(TempDataColumn3)
        dtRelationFill.Columns.Add(TempDataColumn4)
        dtRelationFill.Columns.Add(TempDataColumn5)
        Try
            Dim strFillgrd1 = "select PI_VC100_FatherName,EmpFatherDOB,EmpFatherCountry,EmpMotherName,EmpMotherDOB,EmpMotherCountry,EmpSpouseName,EmpSpouseDOB,EmpSpouseCountry from T010043 where PI_NU8_Address_No=" & UserAddId
            Dim strFillgrd2 = "select SNo,SiblingName,SiblingDOB,SiblingCountry from EmployeeRelationDetail where CI_NU8_Address_Number=" & UserAddId
            Dim strFillgrd3 = "select SNo,ChildName,ChildDOB,ChildCountry from EmployeeRelationDetail where CI_NU8_Address_Number=" & UserAddId
            If SQL.Search("T010043", "SearchPerInfogrd1", "GetGrdInfo1", strFillgrd1, dsGridInfo1, "", "") Then
                Dim drFather As DataRow = dtRelationFill.NewRow()

                drFather("Relation") = "Father"
                drFather("Name") = dsGridInfo1.Tables(0).Rows(0)("PI_VC100_FatherName").ToString()
                If dsGridInfo1.Tables(0).Rows(0)("EmpFatherDOB").ToString() <> "" Then
                    drFather("DOB") = SetDateFormat(dsGridInfo1.Tables(0).Rows(0)("EmpFatherDOB"), IsTime.DateOnly)
                    If drFather("DOB") = "1900-Jan-01" Then
                        drFather("DOB") = ""
                    End If
                End If
                drFather("Country") = dsGridInfo1.Tables(0).Rows(0)("EmpFatherCountry").ToString()
                dtRelationFill.Rows.Add(drFather)
                Dim drMother As DataRow = dtRelationFill.NewRow()
                drMother("Relation") = "Mother"
                drMother("Name") = dsGridInfo1.Tables(0).Rows(0)("EmpMotherName").ToString()
                If dsGridInfo1.Tables(0).Rows(0)("EmpMotherDOB").ToString() <> "" Then
                    drMother("DOB") = SetDateFormat(dsGridInfo1.Tables(0).Rows(0)("EmpMotherDOB"), IsTime.DateOnly)
                    If drMother("DOB") = "1900-Jan-01" Then
                        drMother("DOB") = ""
                    End If
                End If
                drMother("Country") = dsGridInfo1.Tables(0).Rows(0)("EmpMotherCountry").ToString()
                dtRelationFill.Rows.Add(drMother)
                Dim drSpouse As DataRow = dtRelationFill.NewRow()
                drSpouse("Relation") = "Spouse"
                drSpouse("Name") = dsGridInfo1.Tables(0).Rows(0)("EmpSpouseName").ToString()
                If dsGridInfo1.Tables(0).Rows(0)("EmpSpouseDOB").ToString() <> "" Then
                    drSpouse("DOB") = SetDateFormat(dsGridInfo1.Tables(0).Rows(0)("EmpSpouseDOB"), IsTime.DateOnly)
                    If drSpouse("DOB") = "1900-Jan-01" Then
                        drSpouse("DOB") = ""
                    End If
                End If
                drSpouse("Country") = dsGridInfo1.Tables(0).Rows(0)("EmpSpouseCountry").ToString()
                dtRelationFill.Rows.Add(drSpouse)
            End If
            If SQL.Search("T010043", "SearchPerInfogrd2", "GetGrdInfo2", strFillgrd2, dsGridInfo2, "", "") Then
                Dim i As Integer
                For i = 0 To dsGridInfo2.Tables(0).Rows.Count - 1
                    Dim drSibling As DataRow = dtRelationFill.NewRow()
                    drSibling("SNo") = dsGridInfo2.Tables(0).Rows(i)("SNo").ToString()
                    drSibling("Relation") = "Sibling"
                    drSibling("Name") = dsGridInfo2.Tables(0).Rows(i)("SiblingName").ToString()
                    If dsGridInfo2.Tables(0).Rows(i)("SiblingDOB").ToString() <> "" Then
                        drSibling("DOB") = SetDateFormat(dsGridInfo2.Tables(0).Rows(i)("SiblingDOB"), IsTime.DateOnly)
                        If drSibling("DOB") = "1900-Jan-01" Then
                            drSibling("DOB") = ""
                        End If
                    End If
                    drSibling("Country") = dsGridInfo2.Tables(0).Rows(i)("SiblingCountry").ToString()
                    If drSibling("Name") <> "" Then
                        dtRelationFill.Rows.Add(drSibling)
                    End If
                Next
            End If
            If SQL.Search("T010043", "SearchPerInfogrd3", "GetGrdInfo3", strFillgrd3, dsGridInfo3, "", "") Then
                Dim j As Integer
                For j = 0 To dsGridInfo3.Tables(0).Rows.Count - 1
                    If dsGridInfo3.Tables(0).Rows(j)("ChildName").ToString() <> "" Then
                        Dim drChild As DataRow = dtRelationFill.NewRow()
                        drChild("SNo") = dsGridInfo3.Tables(0).Rows(j)("SNo").ToString()
                        drChild("Relation") = "Child"
                        drChild("Name") = dsGridInfo3.Tables(0).Rows(j)("ChildName").ToString()
                        If dsGridInfo3.Tables(0).Rows(j)("ChildDOB").ToString() <> "" Then
                            drChild("DOB") = SetDateFormat(dsGridInfo3.Tables(0).Rows(j)("ChildDOB"), IsTime.DateOnly)
                            If drChild("DOB") = "1900-Jan-01" Then
                                drChild("DOB") = ""
                            End If
                        End If
                        drChild("Country") = dsGridInfo3.Tables(0).Rows(j)("ChildCountry").ToString()
                        If drChild("Name") <> "" Then
                            dtRelationFill.Rows.Add(drChild)
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("AB_Detail", "FillPersonalInfoGrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "PersonalInfoGrid", )
        End Try
    End Sub
    'Update Data on save button click
    Public Sub UpdateData()
        Try

            Dim strMarAnniv As String = String.Empty 'Marriage anniversary
            Dim strVehicleInsuranceExpiry As String = String.Empty 'vehicle insurance expiry date
            Dim strPassportIssueDate As String = String.Empty 'passport issue data
            Dim strPassportValidUntil As String = String.Empty 'passport expiry date
            Dim strOfficialNo As String = String.Empty 'official phone number
            Dim strMobNo As String = String.Empty 'mobile number
            Dim strInsertRelation As String = String.Empty 'string to store insert into relationsTable query
            Dim strUpdateMainRelation As String = String.Empty 'string to store update query
            Dim strRelationType As String = String.Empty 'stores relation type for sibling and child
            Dim strMainRelationType As String = String.Empty 'stores relation type for father,mother and spouse
            Dim strRelationDOBvalue As String = String.Empty
            Dim strAddressline3 As String = String.Empty
            'Dim strCurrentAddress

            If ddlRelation.SelectedValue.Trim() = "Sibling" Or ddlRelation.SelectedValue.Trim() = "Child" Then
                strRelationType = ddlRelation.SelectedValue.Trim()
            Else
                strMainRelationType = ddlRelation.SelectedValue.Trim()
            End If
            If dtAnniversary.SelectedDate.ToString() <> "" Then
                strMarAnniv = CDate(dtAnniversary.SelectedDate)
            Else
                strMarAnniv = dtAnniversary.SelectedDate.ToString()
            End If
            If dtVehicleInsuranceExpiry.SelectedDate.ToString() <> "" Then
                strVehicleInsuranceExpiry = CDate(dtVehicleInsuranceExpiry.SelectedDate)
            Else
                strVehicleInsuranceExpiry = dtVehicleInsuranceExpiry.SelectedDate.ToString()
            End If
            If dtPassportIssueDate.SelectedDate.ToString() <> "" Then
                strPassportIssueDate = CDate(dtPassportIssueDate.SelectedDate)
            Else
                strPassportIssueDate = dtPassportIssueDate.SelectedDate.ToString()
            End If
            If dtPassportExpiry.SelectedDate.ToString() <> "" Then
                strPassportValidUntil = CDate(dtPassportExpiry.SelectedDate)
            Else
                strPassportValidUntil = dtPassportExpiry.SelectedDate.ToString()
            End If
            If txtOfficialPhone.Text.Trim = "" Then
                strOfficialNo = "0"
            Else
                strOfficialNo = txtOfficialPhone.Text.Trim()
            End If
            If txtMobNumber.Text.Trim = "" Then
                strMobNo = "0"
            Else
                strMobNo = txtMobNumber.Text.Trim()
            End If
            If dtRelationDOB.SelectedDate.ToString() <> "" Then
                strRelationDOBvalue = CDate(dtRelationDOB.SelectedDate)
            Else
                strRelationDOBvalue = dtRelationDOB.SelectedDate.ToString()
            End If

            'updating in Table T010043
            Dim strUpdateT010043_1 = "Update T010043 set PI_VC8_Marital_Status='" & ddlMaritalStatus.Text.Trim() & "',PI_DT_MarAnniversary='" & strMarAnniv & "',PI_VC50_HighestQualification='" & txtQualification.Text.Trim() & "',BirthPlace='" & txtBirthPlace.Text.Trim() & "',Country_of_Birth='" & txtCountry.Text.Trim() & "',PI_VC500_PermanentAddress='" & txtPermanentAddress.Text.Trim() & "',PI_VC100_VehicleNumber='" & txtVehicleNo.Text.Trim() & "',PI_VC100_VehicleType='" & txtVehicleType.Text.Trim() & "',VehicleInsurance='" & txtVehicleInsurance.Text.Trim() & "',VehicleInsuranceExpiry='" & strVehicleInsuranceExpiry & "',PI_VC50_PassportNo='" & txtPassportNo.Text.Trim() & "',EmpPassportIssueDate='" & strPassportIssueDate & "',EmpPassportType='" & txtPassportType.Text.Trim() & "',EmpPassportDateValidUntil='" & strPassportValidUntil & "',EmpPassportIssuedBy='" & txtPassportIssuedBy.Text.Trim() & "',PI_VC100_PANNo='" & txtPanNo.Text.Trim() & "',PI_VC100_FoodPrefrence='" & txtFoodPreference.Text.Trim() & "',OfficialNumber='" & strOfficialNo & "',PI_VC50_Nationality='" & txtNationality.Text.Trim() & "',PI_VC200_ESIDetails='" & txtESIDetail.Text.Trim() & "',PI_VC200_PFDetails='" & txtPFDetail.Text.Trim() & "',SkillSet='" & txtSkillSet.Text.Trim() & "',Roles_Respon='" & txtRole.Text.Trim() & "' where PI_NU8_Address_No=" & UserAddId
            'updating in Table T010011
            Dim strUpdateT010011_1 = "Update T010011 set CI_NU16_Phone_Number_1='" & strMobNo & "',CI_VC36_Address_Line_1='" & txtCurrentAddress.Text.Trim() & "',CI_VC36_Address_Line_2='',CI_VC36_Address_Line_3='' where CI_NU8_Address_Number=" & UserAddId
            If strRelationType = "Sibling" And TempTextBox.Text.Trim() <> "" Then
                'update if to be edited item is sibling
                strInsertRelation = "Update EmployeeRelationDetail set SiblingName='" & txtRelationName.Text.Trim & "',SiblingDOB='" & strRelationDOBvalue & "',SiblingCountry='" & txtRelationCountry.Text.Trim() & "' where SNo='" & TempTextBox.Text.Trim() & "' and CI_NU8_Address_Number=" & UserAddId
            ElseIf strRelationType = "Child" And TempTextBox.Text <> "" Then
                'update if to be edited item is child
                strInsertRelation = "Update EmployeeRelationDetail set ChildName='" & txtRelationName.Text.Trim & "',ChildDOB='" & strRelationDOBvalue & "',ChildCountry='" & txtRelationCountry.Text.Trim() & "' where SNo='" & TempTextBox.Text.Trim() & "' and CI_NU8_Address_Number=" & UserAddId
            ElseIf strRelationType = "Sibling" Then
                If txtRelationName.Text = "" Then
                    lstError.Items.Add("Please Select Family Member Name")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                    lstError.Items.Clear()
                    dtRelationDOB.Clear()
                    txtRelationCountry.Text = ""
                Else
                    If CheckValidDate() = True And CheckTextValidity() = True Then
                        strInsertRelation = "INSERT into EmployeeRelationDetail(CI_NU8_Address_Number,SiblingName,SiblingDOB,SiblingCountry) values ('" & UserAddId & "','" & txtRelationName.Text.Trim & "', '" & strRelationDOBvalue & "','" & txtRelationCountry.Text.Trim() & "')"
                    Else
                        If CheckTextValidity() = False Then
                            lstError.Items.Add(strValidity)
                            ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                            lstError.Items.Clear()
                        Else
                            lstError.Items.Add("Passport issue date cannot be greater than Passport expiry date")
                            ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                            lstError.Items.Clear()
                        End If
                    End If
                    'insert if item is sibling and not exist in DB

                End If
            ElseIf strRelationType = "Child" Then
                If txtRelationName.Text = "" Then
                    lstError.Items.Add("Please Select Family Member Name")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                    lstError.Items.Clear()
                Else
                    'insert if item is child and not exist in DB
                    If CheckValidDate() = True And CheckTextValidity() = True Then
                        strInsertRelation = "INSERT into EmployeeRelationDetail(CI_NU8_Address_Number,ChildName,ChildDOB,ChildCountry) values ('" & UserAddId & "','" & txtRelationName.Text.Trim & "', '" & strRelationDOBvalue & "','" & txtRelationCountry.Text.Trim() & "')"
                    Else
                        If CheckTextValidity() = False Then
                            lstError.Items.Add(strValidity)
                            ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                            lstError.Items.Clear()
                        Else
                            lstError.Items.Add("Passport issue date cannot be greater than Passport expiry date")
                            ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                            lstError.Items.Clear()
                        End If
                    End If
                End If
            End If
            If TempTextBox.Text.Trim() <> "" Then
                'update if temporary textbox having value
                If CheckValidDate() = True And CheckTextValidity() = True Then
                    If SQL.Update("EmpUpdateRelation", "UpdateRelation", strInsertRelation, SQL.Transaction.Serializable) = True Then
                        lstError.Items.Add("Updated Successfully")
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgOK)
                        lstError.Items.Clear()
                    End If
                Else
                    If CheckTextValidity() = False Then
                        lstError.Items.Add(strValidity)
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        lstError.Items.Clear()
                    Else
                        lstError.Items.Add("Passport issue date cannot be greater than Passport expiry date")
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        lstError.Items.Clear()
                    End If
                End If

            ElseIf strRelationType = "Sibling" OrElse strRelationType = "Child" Then
                'insert if new sibling or child is being added
                If CheckValidDate() = True And CheckTextValidity() = True Then
                    If SQL.Save("EmpRelation", "InsertEmpRelation", strInsertRelation, SQL.Transaction.Serializable) = True Then
                        lstError.Items.Add("Updated Successfully")
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgOK)
                        lstError.Items.Clear()
                    End If
                Else
                    If CheckTextValidity() = False Then
                        lstError.Items.Add(strValidity)
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        lstError.Items.Clear()
                    Else
                        lstError.Items.Add("Passport issue date cannot be greater than Passport expiry date")
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        lstError.Items.Clear()
                    End If
                End If

            End If
            If strMainRelationType = "Father" Then
                strUpdateMainRelation = "Update T010043 set PI_VC100_FatherName='" & txtRelationName.Text.Trim() & "',EmpFatherDOB='" & strRelationDOBvalue & "',EmpFatherCountry='" & txtRelationCountry.Text.Trim() & "' where PI_NU8_Address_No=" & UserAddId
            ElseIf strMainRelationType = "Mother" Then
                strUpdateMainRelation = "Update T010043 set EmpMotherName='" & txtRelationName.Text.Trim() & "',EmpMotherDOB='" & strRelationDOBvalue & "',EmpMotherCountry='" & txtRelationCountry.Text.Trim() & "' where PI_NU8_Address_No=" & UserAddId
            ElseIf strMainRelationType = "Spouse" Then
                strUpdateMainRelation = "Update T010043 set EmpSpouseName='" & txtRelationName.Text.Trim() & "',EmpSpouseDOB='" & strRelationDOBvalue & "',EmpSpouseCountry='" & txtRelationCountry.Text.Trim() & "' where PI_NU8_Address_No=" & UserAddId
            End If

            If strMainRelationType = "Father" OrElse strMainRelationType = "Mother" OrElse strMainRelationType = "Spouse" Then
                If CheckValidDate() = True And CheckTextValidity() = True Then
                    If SQL.Update("T010043", "UpdateMainRelationInfo", strUpdateMainRelation, SQL.Transaction.Serializable) = True Then
                        lstError.Items.Add("Updated Successfully")
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgOK)
                        lstError.Items.Clear()
                    End If
                Else
                    If CheckTextValidity() = False Then
                        lstError.Items.Add(strValidity)
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        lstError.Items.Clear()
                    Else
                        lstError.Items.Add("Passport issue date cannot be greater than Passport expiry date")
                        ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                        lstError.Items.Clear()
                    End If
                End If
            End If

            If CheckValidDate() = True And CheckTextValidity() = True Then
                If SQL.Update("T010043", "UpdateEmpInfoT010043", strUpdateT010043_1, SQL.Transaction.Serializable) = True Then
                    lstError.Items.Add("Updated Successfully")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgOK)
                    lstError.Items.Clear()
                End If
                If SQL.Update("T010011", "UpdateEmpInfoT010011", strUpdateT010011_1, SQL.Transaction.Serializable) = True Then
                    lstError.Items.Add("Updated Successfully")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgOK)
                    lstError.Items.Clear()
                End If
            Else
                If CheckTextValidity() = False Then
                    lstError.Items.Add(strValidity)
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                    lstError.Items.Clear()
                Else
                    lstError.Items.Add("Passport issue date cannot be greater than Passport expiry date")
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgInfo)
                    lstError.Items.Clear()
                End If
            End If

            ddlRelation.SelectedValue = "--Select--"
            txtRelationName.Text = ""
            dtRelationDOB.Clear()
            txtRelationCountry.Text = ""
            strRelationType = ""
            strMainRelationType = ""
            TempTextBox.Text = ""
        Catch ex As Exception
            CreateLog("AB_Detail", "UpdatePerInfo", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Public Function CheckValidDate() As Boolean
        If dtPassportIssueDate.SelectedDate.ToString() <> "" And dtPassportExpiry.SelectedDate.ToString() <> "" Then
            If CDate(dtPassportIssueDate.SelectedDate) > CDate(dtPassportExpiry.SelectedDate) Then
                Return False
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function

    Public Function CheckTextValidity() As Boolean
        If (IsNumeric(txtMobNumber.Text.Trim()) OrElse txtMobNumber.Text.Trim() = "") And (IsNumeric(txtOfficialPhone.Text.Trim()) OrElse txtOfficialPhone.Text.Trim() = "") Then
            Return True
        Else
            If Not IsNumeric(txtMobNumber.Text.Trim()) Then
                strValidity = "Mobile Number must be Numeric"
            ElseIf Not IsNumeric(txtOfficialPhone.Text.Trim()) Then
                strValidity = "Official Phone Number must be Numeric"
            End If
            Return False
        End If
    End Function
#End Region
#Region "Save and Refresh Button click event"

    Protected Sub imgSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        UpdateData()
        FillRelationsTable()
        FillRelations()
        PersonalInfo()
    End Sub

    Protected Sub imgRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRefresh.Click
        Response.Redirect(Request.Url.AbsoluteUri)
    End Sub
#End Region
End Class

