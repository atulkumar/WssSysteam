Imports System.Drawing.Image
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports ION.Data
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

Partial Class Reports_AddressInfo
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    'Protected WithEvents imgOK As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    'Protected WithEvents cpnlError As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents lblCompany As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlCompany As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddlEmployee As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cpnlRS As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents crvReport As CrystalDecisions.Web.CrystalReportViewer
    'Protected WithEvents cpnlReport As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents Ok As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    'Protected WithEvents HIDSCRID As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents pnlMsg As System.Web.UI.WebControls.Panel
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private crDocument As New ReportDocument
    Private objReports As clsReportData
    Private strServerName As String
    Public mstrCallNumber As String
    Private instance As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf delegateGetThumbnailImageAbort)

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        Try
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            crvReport.ToolbarStyle.Width = New Unit("900px")
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
            End If

            ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlEmployee.ClientID & "');")
            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If
            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Attach"
                            Response.Write("<script>window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');</script>")
                        Case "OK"
                    End Select
                Catch ex As Exception
                End Try
            End If
            show()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
    Shared Function delegateGetThumbnailImageAbort() As Boolean
        'do nothin
    End Function

    Private Sub show()
        Try
            Select Case Request("ip")
                Case "AI"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(793) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, 793)
                    End If
                    'End of Security Block

                    'Response.Write("<head><title>" & "EMPLOYEE INFORMATION REPORT" & "</title></head>")

                    cpnlReport.Text = "EMPLOYEE INFORMATION REPORT"
                    cpnlRS.Text = "EMPLOYEE INFORMATION REPORT"
                    If IsPostBack Then
                        FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenEmployee"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
                    Else
                        fill_company()
                        fill_employee(HttpContext.Current.Session("PropCompanyID"))
                    End If

                    If ddlCompany.SelectedIndex = 0 Then
                        ddlEmployee.Enabled = False
                    Else
                        ddlEmployee.Enabled = True
                    End If
                    lblHead.Text = "Address Book"
                    If IsPostBack Then
                        Showreport(1)
                    End If
                Case Else
                    Response.Redirect("reportsindex.aspx", False)
            End Select
        Catch ex As Exception
            'Dim str As String = ex.Message.ToString
        End Try
    End Sub

    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCompany(3)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            ddlCompany.Items.Insert(0, New ListItem("--Select--", 0))

            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_employee(ByVal companyID As String)

        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.ExtractEmployees(companyID)
            ddlEmployee.DataSource = dt
            ddlEmployee.DataTextField = "Name"
            ddlEmployee.DataValueField = "AddressNo"
            ddlEmployee.DataBind()
            ddlEmployee.Items.Insert(0, "--ALL--")
            dt = Nothing

            dt = New DataTable
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub Showreport(ByVal id As Integer)
        Try
            Select Case id
                Case 1
                    crDocument.Load(Server.MapPath("crAddressBook.rpt"))
            End Select

            Dim strCallStatus As New StringBuilder
            Dim strTaskStatus As New StringBuilder
            strCallStatus.Append("[")
            strTaskStatus.Append("[")

            Dim strRecordSelectionFormula As String = ""
            If ddlCompany.SelectedIndex = 0 Then
                lstError.Items.Clear()
                '  cpnlError.Visible = True
                lstError.Items.Add(" Select company to view information... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            If id = 1 Then
                'cpnlError.Visible = False
                fillReportEd()
            End If

            Dim objReports As New clsReportData
            Dim intCompany As Integer = 0
            If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 Then
                intCompany = ddlCompany.SelectedItem.Value.Trim
            End If

            objReports = Nothing
            crvReport.HasSearchButton = False
            crvReport.HasViewList = False
            crvReport.HasDrillUpButton = False
            crvReport.EnableDrillDown = False
            'clsReportData.LogonInformation(crDocument)
            crDocument.RecordSelectionFormula = strRecordSelectionFormula
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        crDocument.Dispose()
    End Sub

    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
    End Sub

    Private Sub fillReportEd()
        Try
            Dim s As String
            Dim ds As New DSAddressBook
            Dim inti As Integer
            Dim htCols As New Hashtable
            htCols.Add("DOJ", 2)

            If Session("PropAdmin") = "1" Then
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlEmployee.SelectedIndex < 1 Then
                    s = "(select UM_VC50_UserID as CI_VC36_Name, isnull(pi_vc4_picture,'') as pi_vc4_picture, ci_vc8_status,ci_vc36_address_line_1,ci_vc8_city,ci_vc8_province,ci_vc8_country,ci_vc8_email_type_1,ci_vc28_email_1,ci_vc8_phone_type_1,ci_vc8_country_code_1,ci_vc8_area_code_1,ci_nu16_phone_number_1,CI_vc8_level,PI_VC36_First_Name, PI_VC36_Middle_Name, PI_VC36_Last_Name, PI_VC8_WHr_To, PI_VC8_WHr_From, pi_vc8_sex, PI_VC30_Role, convert(varchar,PI_DT8_Date_Of_Joining,101) DOJ,replace(PI_VC4_TimeZone,'(GMT-12:00) International Date Line West','(GMT-12:00)') as PI_VC4_TimeZone, CI_IN4_Business_Relation, ci_nu8_address_number  from t010011, t010043 , T060011 where  UM_IN4_Address_No_FK=CI_NU8_Address_Number and ci_nu8_address_number=pi_nu8_address_no  and ci_vc8_address_book_type='em' and ci_in4_business_relation='" & ddlCompany.SelectedValue & "') order by UM_VC50_UserID ;select ci_vc36_name,ci_vc8_address_book_type,ci_nu8_address_number from t010011 where ci_vc8_address_book_type='com'and  ci_nu8_address_number='" + ddlCompany.SelectedValue + "'"

                Else
                    If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlEmployee.SelectedIndex <> 0 Then
                        s = "select UM_VC50_UserID as CI_VC36_Name, isnull(pi_vc4_picture,'') as pi_vc4_picture, ci_vc8_status,ci_vc36_address_line_1,ci_vc8_city,ci_vc8_province,ci_vc8_country,ci_vc8_email_type_1,ci_vc28_email_1,ci_vc8_phone_type_1,ci_vc8_country_code_1,ci_vc8_area_code_1,ci_nu16_phone_number_1,CI_vc8_level,PI_VC36_First_Name, PI_VC36_Middle_Name, PI_VC36_Last_Name, PI_VC8_WHr_To, PI_VC8_WHr_From, pi_vc8_sex, PI_VC30_Role, convert(varchar,PI_DT8_Date_Of_Joining,101) DOJ,replace(PI_VC4_TimeZone,'(GMT-12:00) International Date Line West','(GMT-12:00)') as PI_VC4_TimeZone, CI_IN4_Business_Relation, ci_nu8_address_number  from t010011, t010043 , T060011 where  UM_IN4_Address_No_FK=CI_NU8_Address_Number and ci_nu8_address_number=pi_nu8_address_no  and ci_vc8_address_book_type='em' and ci_in4_business_relation='" & ddlCompany.SelectedValue & "' and CI_NU8_ADDRESS_NUMBER='" + ddlEmployee.SelectedValue + "' order by UM_VC50_UserID;select ci_vc36_name,ci_vc8_address_book_type,ci_nu8_address_number from t010011 where ci_vc8_address_book_type='com'and  ci_nu8_address_number='" + ddlCompany.SelectedValue + "' "
                    End If
                End If
            Else ' Non Admin
                If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlEmployee.SelectedIndex < 1 Then
                    s = "(select UM_VC50_UserID as CI_VC36_Name, isnull(pi_vc4_picture,'') as pi_vc4_picture, ci_vc8_status,ci_vc36_address_line_1,ci_vc8_city,ci_vc8_province,ci_vc8_country,ci_vc8_email_type_1,ci_vc28_email_1,ci_vc8_phone_type_1,ci_vc8_country_code_1,ci_vc8_area_code_1,ci_nu16_phone_number_1,CI_vc8_level,PI_VC36_First_Name, PI_VC36_Middle_Name, PI_VC36_Last_Name, PI_VC8_WHr_To, PI_VC8_WHr_From, pi_vc8_sex, PI_VC30_Role,convert(varchar,PI_DT8_Date_Of_Joining,101)  DOJ,replace(PI_VC4_TimeZone,'(GMT-12:00) International Date Line West','(GMT-12:00)') as PI_VC4_TimeZone, CI_IN4_Business_Relation, ci_nu8_address_number  from t010011, t010043 , T060011 where  UM_IN4_Address_No_FK=CI_NU8_Address_Number and ci_nu8_address_number=pi_nu8_address_no  and ci_vc8_address_book_type='em' and ci_in4_business_relation='" & ddlCompany.SelectedValue & "' and CI_NU8_Address_Number = '" & Session("PropUserID") & "') order by UM_VC50_UserID ;select ci_vc36_name,ci_vc8_address_book_type,ci_nu8_address_number from t010011 where ci_vc8_address_book_type='com'and  ci_nu8_address_number='" + ddlCompany.SelectedValue + "'"

                Else
                    If ddlCompany.SelectedIndex <> 0 And ddlCompany.Items.Count > 1 And ddlEmployee.SelectedIndex <> 0 Then
                        s = "select UM_VC50_UserID as CI_VC36_Name, isnull(pi_vc4_picture,'') as pi_vc4_picture, ci_vc8_status,ci_vc36_address_line_1,ci_vc8_city,ci_vc8_province,ci_vc8_country,ci_vc8_email_type_1,ci_vc28_email_1,ci_vc8_phone_type_1,ci_vc8_country_code_1,ci_vc8_area_code_1,ci_nu16_phone_number_1,CI_vc8_level,PI_VC36_First_Name, PI_VC36_Middle_Name, PI_VC36_Last_Name, PI_VC8_WHr_To, PI_VC8_WHr_From, pi_vc8_sex, PI_VC30_Role,convert(varchar,PI_DT8_Date_Of_Joining,101)  DOJ,replace(PI_VC4_TimeZone,'(GMT-12:00) International Date Line West','(GMT-12:00)') as PI_VC4_TimeZone, CI_IN4_Business_Relation, ci_nu8_address_number  from t010011, t010043 , T060011 where  UM_IN4_Address_No_FK=CI_NU8_Address_Number and ci_nu8_address_number=pi_nu8_address_no  and ci_vc8_address_book_type='em' and ci_in4_business_relation='" & ddlCompany.SelectedValue & "' and CI_NU8_Address_Number = '" & Session("PropUserID") & "' order by UM_VC50_UserID;select ci_vc36_name,ci_vc8_address_book_type,ci_nu8_address_number from t010011 where ci_vc8_address_book_type='com'and  ci_nu8_address_number='" + ddlCompany.SelectedValue + "' "
                    End If
                End If
            End If
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("Table", "WSSReportsD", "ExtractCallNo", s, ds, "jagmit", "sidhu")
            SetDataTableDateFormat(ds.Tables(0), htCols)
            Dim crfr As FormulaFieldDefinition
            crfr = CType(crDocument.DataDefinition.FormulaFields("Location"), FormulaFieldDefinition)

            Dim strLoc As String
            Dim fname, NewFname As String
            For inti = 0 To ds.Tables(0).Rows.Count - 1
                strLoc = Server.MapPath(Request.ApplicationPath) & "\Dockyard\ABImages\"
                If ds.Tables(0).Rows(inti).Item("pi_vc4_picture") <> "" Then
                    fname = System.IO.Path.GetFileName(ds.Tables(0).Rows(inti).Item("pi_vc4_picture"))
                    NewFname = createThumbNailImage(strLoc, fname)
                    ds.Tables(0).Rows(inti).Item("pi_vc4_picture") = NewFname
                Else
                    ds.Tables(0).Rows(inti).Item("pi_vc4_picture") = Server.MapPath(Request.ApplicationPath) & "\images\" & "NoPhoto.jpg"
                End If
            Next
            ds.AcceptChanges()
            crDocument.SetDataSource(ds)
            ' End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function ScaleImage(ByVal Photo As System.Drawing.Image, ByVal RequiredWidth As Integer, ByVal RequiredHeight As Integer) As System.Drawing.Image
        Try
            Dim sourceWidth As Integer = Photo.Width ' -- Original width of the image
            Dim sourceHeight As Integer = Photo.Height ' -- Original Height of Image
            Dim destWidth As Integer ' -- Required width of image
            Dim destHeight As Integer ' -- Required height of image
            Dim nPercent As Single = 0 ' -- percentage of scaling required to be done on width as well as height
            Dim nPercentW As Single = 0 ' -- percentage calculated on the basis of required width
            Dim nPercentH As Single = 0 ' -- percentage calculated on the basis of required height
            nPercentW = (CType(RequiredWidth, Single) / CType(sourceWidth, Single)) ' -- Calculate width percentage
            nPercentH = (CType(RequiredHeight, Single) / CType(sourceHeight, Single)) ' -- Calculate height percentage

            If nPercentH < nPercentW Then
                nPercent = nPercentH
            Else
                nPercent = nPercentW
            End If
            destWidth = CType((sourceWidth * nPercent), Integer) ' -- Calculate scaled image width
            destHeight = CType((sourceHeight * nPercent), Integer) ' -- calculate scaled image height
            Return Photo.GetThumbnailImage(destWidth, destHeight, instance, IntPtr.Zero)   ' -- Return scaled Image
        Catch ex As Exception
            ' -- nothing to do when exceptions comes
        End Try
    End Function

    Private Function createThumbNailImage(ByVal strImagePath As String, ByVal strImageName As String) As String
        '***************************************************************************************************
        'Created By : RVS -ION Softnet Created On: 12/10/2006
        'Modified By: Modified On: 
        'Purpose : This function creates Thumnail Image of uploaded Image
        '***************************************************************************************************
        Dim strNewImageName As String ' -- Name of Thumbnail Image
        Dim strNewImageNameWithoutEx As String ' -- Name of Thumbnail Image without extension
        Const maxSize = 125
        Try
            Dim imgTemp As System.Drawing.Image     ' -- Temporary Image
            Dim imgNew As System.Drawing.Image ' -- New Scaled Image
            Dim sp As String
            sp = strImagePath 'Server.MapPath("dockyard\abimages")
            ' -- Add backslash when it is already there
            If sp.EndsWith("\") = False Then
                sp += "\"
            End If
            sp += strImageName
            imgTemp = System.Drawing.Image.FromFile(sp) ' -- ReOpen uploaded Image
            strNewImageName = strImagePath + strImageName ' -- Initialize thmb name to original name which will be used for small image
            strNewImageNameWithoutEx = strImagePath + System.IO.Path.GetFileNameWithoutExtension(strNewImageName) ' -- Initialize thmb name to original name which will be used for small image
            strNewImageName = strNewImageName.Insert(strNewImageNameWithoutEx.Length, "_Thmb")   ' -- Construct new name
            If IO.File.Exists(strNewImageName) = False Then
                If imgTemp.Height > maxSize Or imgTemp.Width > maxSize Then ' -- check whether new image for thumbnail is required or not
                    imgNew = ScaleImage(imgTemp, maxSize, maxSize) ' -- Scale Image
                    sp = strNewImageName
                    imgNew.Save(sp, System.Drawing.Imaging.ImageFormat.Jpeg) 'Save Image with new name
                    changePicSize(imgNew.Width, imgNew.Height)
                Else
                    strNewImageName = strImagePath + strImageName
                End If
            Else
                imgTemp.Dispose()
                imgTemp = System.Drawing.Image.FromFile(strNewImageName)
                changePicSize(imgTemp.Width, imgTemp.Height)
            End If
            imgTemp.Dispose()
            imgNew.Dispose()
        Catch ex As Exception
            '--nothing to do
        End Try
        Return strNewImageName
    End Function

    Private Sub changePicSize(ByVal intWidth, ByVal intHeight)
        Dim crpic As PictureObject
        crpic = crDocument.ReportDefinition.ReportObjects("picture3")
        crpic.ObjectFormat.EnableCanGrow = False
        crpic.Height = intHeight * 15
        crpic.Width = intWidth * 15
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub


End Class
