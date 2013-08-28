Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient

Partial Class MonitoringCenter_Configuration
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim txthiddenImage = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Logout"
                    LogoutWSS()
            End Select
        End If



        If Not IsPostBack Then
            Dim strCompSQL As String
            strCompSQL = "select CI_NU8_Address_Number,CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type='COM' "
            FillCompanyDropDown(DDLCompany, strCompSQL)
            DDLCompany.Items.Insert(0, New ListItem("Select", "0"))

            'DDLCompany.Items.Insert(0, "select")

            If HttpContext.Current.Session("PropCompanyType") = "SCM" Then
                DDLCompany.Enabled = True
                cpnlalert.Visible = False
                cpnlMonitoring.Visible = False
                cpnlReport.Visible = False
                cpnlReports.Visible = False
            Else
                'cpnl visible True and ddlcompany visible False when login by Client company
                If IsNothing(HttpContext.Current.Session("PropCAComp")) Then
                    HttpContext.Current.Session("PropCAComp") = HttpContext.Current.Session("PropCompanyID")
                End If
                DDLCompany.SelectedValue = HttpContext.Current.Session("PropCAComp")
                DDLCompany.Enabled = False
            End If
        Else
            HttpContext.Current.Session("PropCAComp") = DDLCompany.SelectedValue
        End If


        'Security Block
        'this function work when client company login 
        'If HttpContext.Current.Session("PropCompanyType") = "CCMP" Then

        Dim intID1 As Int32
        'If Not IsPostBack Then
        Dim str As String
        str = HttpContext.Current.Session("PropRootDir")
        intID1 = 765
        Dim obj1 As New clsSecurityCache
        If obj1.ScreenAccess(intID1) = False Then
            Response.Redirect("../frm_NoAccess.aspx")
        End If
        obj1.ControlSecurity(Me.Page, intID1)
        'End If
        'End If
        'End of Security Block
    End Sub


    Public Function FillCompanyDropDown(ByVal ddlCustom As DropDownList, ByVal strSQL As String, Optional ByVal OptionalField As Boolean = False)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            If OptionalField = True Then
                ddlCustom.Items.Add(New ListItem("", ""))
            End If
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("Call Entry", "FillTemplTypeDropDown", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function

    Private Sub DDLCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLCompany.SelectedIndexChanged
        Dim strSQL As String

        If DDLCompany.SelectedIndex.Equals(0) Then
            lstError.Items.Clear()
            lstError.Items.Add(" Please Select Company")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            cpnlalert.Visible = False
            cpnlMonitoring.Visible = False
            cpnlReport.Visible = False
            cpnlReports.Visible = False

        Else
            Session("companyID") = DDLCompany.SelectedValue.Trim
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet

            strSQL = "select obm_vc50_object_name,rod_ch1_view_hide,rod_ch1_enable_disable from T070042, T070011 b,t070031 a where b.OBM_IN4_OBJECT_PID_FK=765  and rod_in4_role_id_fk=a.ROM_IN4_Role_ID_PK and ROD_IN4_Object_ID_FK=b.OBM_IN4_Object_id_PK AND a.rom_in4_company_id_fk=" & Session("PropCAComp")
            If SQL.Search("T070042", "dataobjentry", "FILLCompany", strSQL, dsTemp, "", "") = True Then

                cpnlError.Visible = False
                cpnlalert.Visible = True
                cpnlMonitoring.Visible = True
                cpnlreports.Visible = True
                cpnlReport.Visible = True

                ControlSecurity(dsTemp)
            Else
                lstError.Items.Clear()
                lstError.Items.Add(" sorry no link available for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)

                cpnlalert.Visible = False
                cpnlMonitoring.Visible = False
                cpnlReport.Visible = False
                cpnlReports.Visible = False

            End If
        End If
        'checking Domain and Machine for the company

        strSQL = "select DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
        Dim intDomainID As Integer = SQL.Search("", "", strSQL)

        If intDomainID <= 0 Then
            HyAlert.Enabled = False
            HyMonitoring.Enabled = False
            HyReport.Enabled = False
        End If

    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function will show  all controls whether it given to admin or .net acc to selected                         company
    'Modify Date:       ------
    '***************************************************************************************
    Private Function ControlSecurity(ByVal dsSecurity As DataSet)

        Dim dvTemp As DataView
        Dim dvTemp1 As DataView
        For Each ctrlPNL As Control In Me.Page.FindControl("Form1").Controls

            If TypeOf ctrlPNL Is CustomControls.Web.CollapsiblePanel Then
                For Each ctrlHYL As Control In ctrlPNL.Controls
                    Dim bytEnable As Byte = 0
                    Dim bytVisible As Byte = 0

                    If TypeOf ctrlHYL Is HyperLink Then
                        dvTemp1 = dsSecurity.Tables(0).DefaultView

                        dvTemp = GetFilteredDataView(dvTemp1, "obm_vc50_object_name ='" & ctrlHYL.ID & "'")

                        If dvTemp.Table.Rows.Count > 0 Then
                            For intI As Integer = 0 To dvTemp.Table.Rows.Count - 1
                                If dvTemp.Table.Rows(intI).Item("rod_ch1_view_hide") = "V" Then
                                    CType(ctrlHYL, HyperLink).Visible = True
                                    bytVisible = 1
                                Else
                                    If bytVisible = 0 Then CType(ctrlHYL, HyperLink).Visible = False
                                End If

                                If dvTemp.Table.Rows(intI).Item("rod_ch1_enable_disable") = "E" Then
                                    CType(ctrlHYL, HyperLink).Enabled = True
                                    bytEnable = 1
                                Else
                                    If bytEnable = 0 Then CType(ctrlHYL, HyperLink).Enabled = False
                                End If
                            Next
                        Else
                            CType(ctrlHYL, HyperLink).Enabled = False
                            CType(ctrlHYL, HyperLink).Visible = False

                        End If
                    End If
                Next
            End If
        Next

    End Function

End Class
