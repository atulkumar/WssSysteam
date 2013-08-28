Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

Partial Class SupportCenter_CallView_UserInfo
    Inherits System.Web.UI.Page
    Dim CallOwnerName As String
    Dim CallOwnerID As Integer
    Dim CompID As String
    Dim CallNo As String
    Dim AdressBookID As Integer
    Public Shared mstrMailID As String
    Public Shared mstrMailSub As String
    Dim StrCompID As String
    Dim StrcompName As String

    Dim strscrID As String


    '*******************************************************************
    ' Function             :-  pageload
    ' Purpose              :- fill and design datagrid based on user view data from user tables
    '								
    ' Date					  Author						Modification Date					Description
    ' 06/06/2006			      Sachin Prashar           -------------------	Created
    '
    '*******************************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        Try

            imgClose.Attributes.Add(" OnClick", "javascript:window.close();")
            AdressBookID = Val(Request.QueryString("ADDNO"))

            strscrID = Request.QueryString("ScreenID")

            txtCSS(Me.Page)
            If strscrID = "463" Then
                mstrMailSub = "Regarding Call"
            ElseIf strscrID = "464" Then
                mstrMailSub = "Regarding Task"
            ElseIf strscrID = "502" Then
                mstrMailSub = "Regarding TO DO LIST"
            End If

            ' If AdressBookID = 0 Then ' If Address Book No of the user is not known
            CallOwnerName = Request.QueryString("CALLOWNER")
            CallNo = Request.QueryString("CALLNO")
            If Val(Request.QueryString("Comp")) > 0 Then
                CallOwnerID = Val(Request.QueryString("Comp"))
            Else
                CompID = Request.QueryString("Comp")
                CallOwnerID = WSSSearch.SearchCompName(CompID).ExtraValue
            End If
            GetAddBookNo()
            '   Else  ' If Address Book No of the user is known
            GetUserInfo()
            '   End If
            lbInv.Attributes.Add("OnClick", "return OpenInv('" & AdressBookID & "')")
        Catch ex As Exception
            CreateLog("UserInfo", "UserInfo-91", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try

    End Sub

    '*******************************************************************
    ' Function             :-  GetAddressbookNo
    ' Purpose              :- get address book no from t060011 
    '								
    ' Date					     Author						Modification Date	    Description
    ' 06/06/2006		     Sachin Prashar           ------------------  	    Created
    ' Notes: 
    ' Code:
    '*******************************************************************

    Function GetAddBookNo()

        Try

            Dim sqrdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean

            sqrdr = SQL.Search("UserInfo", "GetAddBookNo", "select UM_IN4_Address_No_FK from T060011 where UM_VC50_UserID='" & CallOwnerName & "'", SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                While sqrdr.Read

                    AdressBookID = IIf(Not IsDBNull(sqrdr("UM_IN4_Address_No_FK")), sqrdr("UM_IN4_Address_No_FK"), "")

                End While
                sqrdr.Close()
            End If

        Catch ex As Exception
            CreateLog("UserInfo", "GetAddBookNo-113", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function
    '*******************************************************************
    ' Function             :-  GetUserInfo
    ' Purpose              :- Get address book no from T010011 and T010043 
    '								
    ' Date					     Author						Modification Date	    Description
    ' 07/06/2006		     Sachin Prashar           ------------------  	    Created
    ' Notes: 
    ' Code:
    '*******************************************************************

    Function GetUserInfo()

        Try

            Dim sqrdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean

            'sqrdr = SQL.Search("UserInfo", "GetUserInfo", "select * from T010011  LEFT OUTER JOIN T010043  on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No where  T010011.CI_NU8_Address_Number=" & AdressBookID & " ", SQL.CommandBehaviour.SingleRow, blnStatus, "")
            sqrdr = SQL.Search("UserInfo", "GetUserInfo", "(select *,(select description from UDC where UDCType='CTY' and UDC.[Name] =(select CI_VC8_City  from T010011  inner JOIN T010043 on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No and T010011.CI_NU8_Address_Number=" & AdressBookID & "))as City,(select description as country  from UDC where UDCType='CNTY' and UDC.[Name] =(select CI_VC8_Country  from T010011  inner JOIN T010043 on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No and T010011.CI_NU8_Address_Number=" & AdressBookID & ")) as Country from T010011  LEFT OUTER JOIN T010043 on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No where  T010011.CI_NU8_Address_Number=" & AdressBookID & ")", SQL.CommandBehaviour.SingleRow, blnStatus, "")

            If blnStatus = True Then
                While sqrdr.Read

                    lblUserID.Text = WSSSearch.SearchUserID(AdressBookID).ExtraValue   'CallOwnerName
                    lblFullName.Text = IIf(Not IsDBNull(sqrdr("CI_VC36_Name")), sqrdr("CI_VC36_Name"), "")

                    StrCompID = IIf(Not IsDBNull(sqrdr("CI_IN4_Business_Relation")), sqrdr("CI_IN4_Business_Relation"), "")
                    StrcompName = WSSSearch.SearchCompNameID(StrCompID).ExtraValue
                    lblCompany.Text = StrcompName

                    'lblJobRole.Text = IIf(Not IsDBNull(sqrdr("CI_VC8_Address_Book_Type")), sqrdr("CI_VC8_Address_Book_Type"), "")
                    'New Field Added Update Feb2009
                    lblJobRole.Text = IIf(Not IsDBNull(sqrdr("PI_NU9_JobRole")), sqrdr("PI_NU9_JobRole"), "")


                    Dim strMobileno As String
                    Dim strCountryCode1 As String
                    Dim strAreaCode1 As String
                    Dim strPhoneNo1 As String

                    strCountryCode1 = IIf(Not IsDBNull(sqrdr("CI_VC8_Country_Code_1")), sqrdr("CI_VC8_Country_Code_1"), "")
                    strAreaCode1 = IIf(Not IsDBNull(sqrdr("CI_VC8_Area_code_1")), sqrdr("CI_VC8_Area_code_1"), "")
                    strPhoneNo1 = IIf(Not IsDBNull(sqrdr("CI_NU16_Phone_Number_1")), sqrdr("CI_NU16_Phone_Number_1"), "")

                    strMobileno = ""

                    If strCountryCode1.Equals("") Or strCountryCode1.Equals("0") Then
                    Else
                        strMobileno &= strCountryCode1
                    End If
                    If strAreaCode1.Equals("") Or strAreaCode1.Equals("0") Then
                    Else
                        If strCountryCode1.Equals("") Or strCountryCode1.Equals("0") Then
                            strMobileno &= strAreaCode1
                        Else
                            strMobileno &= "-" & Val(strAreaCode1)
                        End If
                    End If
                    If strMobileno.Equals("") Or strMobileno.Equals("0") Then
                        strMobileno &= strPhoneNo1
                    Else
                        strMobileno &= "-" & strPhoneNo1
                    End If

                    lblMobNo.Text &= strMobileno


                    Dim strOffPhoneNo As String
                    Dim strCountryCode2 As String
                    Dim strAreaCode2 As String
                    Dim strPhoneNo2 As String

                    strCountryCode2 = IIf(Not IsDBNull(sqrdr("CI_VC8_Country_Code_2")), sqrdr("CI_VC8_Country_Code_2"), "")
                    strAreaCode2 = IIf(Not IsDBNull(sqrdr("CI_VC8_Area_code_2")), sqrdr("CI_VC8_Area_code_2"), "")
                    strPhoneNo2 = IIf(Not IsDBNull(sqrdr("CI_NU16_Phone_Number_2")), sqrdr("CI_NU16_Phone_Number_2"), "")

                    strOffPhoneNo = ""

                    If strCountryCode2.Equals("") Or strCountryCode2.Equals("0") Then
                    Else
                        strOffPhoneNo &= strCountryCode2
                    End If
                    If strAreaCode2.Equals("") Or strAreaCode2.Equals("0") Then
                    Else
                        If strCountryCode2.Equals("") Or strCountryCode2.Equals("0") Then
                            strOffPhoneNo &= strAreaCode2
                        Else
                            strOffPhoneNo &= "-" & Val(strAreaCode2)
                        End If
                    End If
                    If strOffPhoneNo.Equals("") Or strOffPhoneNo.Equals("0") Then
                        strOffPhoneNo &= strPhoneNo2
                    Else
                        strOffPhoneNo &= "-" & strPhoneNo2
                    End If

                    lblOffPhone.Text = strOffPhoneNo

                    Dim strphone1type As String
                    strphone1type = IIf(Not IsDBNull(sqrdr("CI_VC8_Phone_Type_1")), sqrdr("CI_VC8_Phone_Type_1"), "")
                    If strphone1type.Equals("") Then
                    Else
                        lblPhone1Type.Text = "(" & strphone1type & ")"
                    End If

                    Dim strphone2type As String
                    strphone2type = IIf(Not IsDBNull(sqrdr("CI_VC8_Phone_Type_2")), sqrdr("CI_VC8_Phone_Type_2"), "")
                    If strphone2type.Equals("") Then
                    Else
                        lblPhone2Type.Text = "(" & strphone2type & ")"
                    End If



                    lblMailAdd.Text = IIf(Not IsDBNull(sqrdr("CI_VC28_Email_1")), sqrdr("CI_VC28_Email_1"), "")
                    mstrMailID = ""
                    mstrMailID = lblMailAdd.Text

                    lblSex.Text = IIf(Not IsDBNull(sqrdr("PI_VC8_Sex")), sqrdr("PI_VC8_Sex"), "")
                    lblTimeZone.Text = IIf(Not IsDBNull(sqrdr("PI_VC4_TimeZone")), sqrdr("PI_VC4_TimeZone"), "")

                    lblCity.Text = IIf(Not IsDBNull(sqrdr("City")), sqrdr("City"), "")
                    lblCountry.Text = IIf(Not IsDBNull(sqrdr("Country")), sqrdr("Country"), "")

                    Dim StrTimFrom As String
                    Dim StrTimTo As String

                    StrTimFrom = IIf(Not IsDBNull(sqrdr("PI_VC8_WHr_From")), sqrdr("PI_VC8_WHr_From"), "")
                    StrTimTo = IIf(Not IsDBNull(sqrdr("PI_VC8_WHr_To")), sqrdr("PI_VC8_WHr_To"), "")

                    lblWorkingHr.Text = "From " & StrTimFrom & " To " & StrTimTo
                    If Not IsDBNull(sqrdr("PI_VC8_Department")) Then
                        lblDepartment.Text = GetUDCDescription("DPT", sqrdr("PI_VC8_Department"), StrCompID)
                    Else
                        lblDepartment.Text = ""
                    End If
                    If Not IsDBNull(sqrdr("PI_NU9_Manager")) Then
                        lblManager.Text = GetManagerName(sqrdr("PI_NU9_Manager"))
                    Else
                        lblManager.Text = ""
                    End If

                    If IsDBNull(sqrdr("PI_VC4_Picture")) = False Then
                        If sqrdr("PI_VC4_Picture") = "" Then
                            Image1.ImageUrl = "../../Images/NoPhoto.jpg"
                        Else
                            Image1.ImageUrl = IIf(System.IO.File.Exists(Server.MapPath(sqrdr("PI_VC4_Picture"))), sqrdr("PI_VC4_Picture"), "../../Images/NoPhoto.jpg")
                        End If
                    Else
                        Image1.ImageUrl = "../../Images/NoPhoto.jpg"
                    End If
                    ' Image1.ImageUrl = IIf(Not IsDBNull(sqrdr("PI_VC4_Picture")), sqrdr("PI_VC4_Picture"), "../../Images/NoPhoto.jpg")

                End While
                sqrdr.Close()
            End If
        Catch ex As Exception
            CreateLog("UserInfo", "GetUserInfo-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function
End Class
