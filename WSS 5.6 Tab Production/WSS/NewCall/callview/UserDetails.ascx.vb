Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data
Partial Class UserDetails
    Inherits System.Web.UI.UserControl
    Dim AdressBookID As Int32
    Private _CallOwner As String
    Public Shared mstrMailID As String
    Public Shared mstrMailSub As String
    Dim StrCompID As String
    Dim StrcompName As String
    Public Property GetCallOwner() As String
        Get
            Return _CallOwner
        End Get
        Set(ByVal value As String)
            _CallOwner = value
        End Set
    End Property
    Function GetAddBookNo()

        Try

            Dim sqrdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean

            sqrdr = SQL.Search("UserInfo", "GetAddBookNo", "select UM_IN4_Address_No_FK from T060011 where UM_VC50_UserID='" & _CallOwner & "'", SQL.CommandBehaviour.SingleRow, blnStatus, "")
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

    Function GetUserInfo()
        Try
            Dim sqrdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            sqrdr = SQL.Search("UserInfo", "GetUserInfo", "(select *,(select description from UDC where UDCType='CTY' and UDC.[Name] =(select CI_VC8_City  from T010011  inner JOIN T010043 on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No and T010011.CI_NU8_Address_Number=" & AdressBookID & "))as City,(select description as country  from UDC where UDCType='CNTY' and UDC.[Name] =(select CI_VC8_Country  from T010011  inner JOIN T010043 on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No and T010011.CI_NU8_Address_Number=" & AdressBookID & ")) as Country from T010011  LEFT OUTER JOIN T010043 on T010011.CI_NU8_Address_Number=T010043.PI_NU8_Address_No where  T010011.CI_NU8_Address_Number=" & AdressBookID & ")", SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
               
                While sqrdr.Read

                    lblUserID.Text = WSSSearch.SearchUserID(AdressBookID).ExtraValue   'CallOwnerName
                    lblFullName.Text = IIf(Not IsDBNull(sqrdr("CI_VC36_Name")), sqrdr("CI_VC36_Name"), "")

                    StrCompID = IIf(Not IsDBNull(sqrdr("CI_IN4_Business_Relation")), sqrdr("CI_IN4_Business_Relation"), "")
                    StrcompName = WSSSearch.SearchCompNameID(StrCompID).ExtraValue
                    lblCompany.Text = StrcompName

                    lblJobRole.Text = IIf(Not IsDBNull(sqrdr("PI_NU9_JobRole")), sqrdr("PI_NU9_JobRole"), "")
                    lblCity.Text = IIf(Not IsDBNull(sqrdr("City")), sqrdr("City"), "")
                    lblCountry.Text = IIf(Not IsDBNull(sqrdr("Country")), sqrdr("Country"), "")

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

                    lblMobNo.Text = strMobileno


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
                        'lblPhone1Type.Text = "(" & strphone1type & ")"
                    End If

                    Dim strphone2type As String
                    strphone2type = IIf(Not IsDBNull(sqrdr("CI_VC8_Phone_Type_2")), sqrdr("CI_VC8_Phone_Type_2"), "")
                    If strphone2type.Equals("") Then
                    Else
                        'lblPhone2Type.Text = "(" & strphone2type & ")"
                    End If



                    lblMailAdd.Text = IIf(Not IsDBNull(sqrdr("CI_VC28_Email_1")), sqrdr("CI_VC28_Email_1"), "")
                    mstrMailID = ""
                    mstrMailID = lblMailAdd.Text

                    lblSex.Text = IIf(Not IsDBNull(sqrdr("PI_VC8_Sex")), sqrdr("PI_VC8_Sex"), "")
                    'lblTimeZone.Text = IIf(Not IsDBNull(sqrdr("PI_VC4_TimeZone")), sqrdr("PI_VC4_TimeZone"), "")

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
                    'Image1.ImageUrl = IIf(Not IsDBNull(sqrdr("PI_VC4_Picture")), sqrdr("PI_VC4_Picture"), "../../Images/NoPhoto.jpg")

                End While
                sqrdr.Close()
            End If
        Catch ex As Exception
            CreateLog("UserInfo", "GetUserInfo-162", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GetAddBookNo()
        GetUserInfo()

    End Sub
End Class
