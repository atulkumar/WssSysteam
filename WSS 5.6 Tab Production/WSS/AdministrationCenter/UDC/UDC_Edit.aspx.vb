'************************************************************************************************************
' Page                 : - UDC_Edit
' Purpose              : - This form will used for editing the UDC entered before.User can edit UDC                                     description or UDC name,product code etc.
' Tables used          : - T010011, UDC  
' Date				   Author						Modification Date					Description
' 04/06/06			   Amandeep						-------------------					Created
'
' Notes: 
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports System.Drawing
Imports System.Data

Partial Class AdministrationCenter_UDC_UDC_Edit
    Inherits System.Web.UI.Page

    Dim mintProductCode As Integer
    Dim mstrUDCType As String
    Dim mstrUDCName As String
    Dim mstrUDCCompany As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        '     Dim strSQL As String = "select CI_NU8_Address_Number as ID,CI_VC36_Name as Name,CI_IN4_Business_Relation as Type from T010011 where CI_VC8_Address_Book_Type='COM'"
        '  FillNonUDCDropDown(txtCompanyDDL, 2, strSQL)
        '  txtCompanyDDL.Attributes.Add("OnChange", "SelectDDL('COM','txtCompanyDDL');")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        cpnlErrorPanel.Visible = False

        '--Customer
        CDDLCustomer.CDDLQuery = "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "
        CDDLCustomer.CDDLMandatoryField = True
        CDDLCustomer.CDDLUDC = False
        Call txtCSS(Me.Page)
        '-----------------------------------------
        If IsPostBack = False Then

            CDDLCustomer.CDDLFillDropDown(10, False)
        Else
            CDDLCustomer.CDDLSetItem()
        End If
        ViewState("UDCCompany") = Request.QueryString("UDCCompany")
        If Not IsPostBack Then
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        End If

        mintProductCode = Request.QueryString("Code") 'HttpContext.Current.Session("SProductCodeU")
        mstrUDCType = Request.QueryString("Type") 'HttpContext.Current.Session("SUDCTypeF")
        mstrUDCName = Request.QueryString("Name") 'HttpContext.Current.Session("SName")

        If ViewState("UDCCompany") = "&nbsp;" Then
            mstrUDCCompany = ""
        Else
            mstrUDCCompany = ViewState("UDCCompany")
        End If

        If Not IsPostBack Then
            If IsNothing(mintProductCode) Or IsNothing(mstrUDCType) Then
                Exit Sub
            End If

            'txtProductCode.Text = mintProductCode
            'txtUCDTypeF.Text = mstrUDCType
            'txtCompany.Text = mstrUDCCompany
            'txtName.Text = HttpContext.Current.Session("SName")
            'txtDescription.Text = HttpContext.Current.Session("SUDCDescription")
            Call FillForm(mintProductCode, mstrUDCType, mstrUDCName)

        End If
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlErrorPanel.Visible = True
                            lblError.Text = "You don't have access rights to Save record..."
                            Exit Sub
                        End If
                        'End of Security Block
                        UpdateUDC()
                    Case "Close"
                        Response.Write("<script>window.close();</script>")
                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlErrorPanel.Visible = True
                            lblError.Text = "You don't have access rights to Save record..."
                            Exit Sub
                        End If
                        'End of Security Block
                        UpdateUDC()
                        Response.Write("<script>window.close();</script>")
                End Select
            Catch ex As Exception
                'ImgError.ImageUrl = "../../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                lblError.Text = "Server is busy please try later..."

                CreateLog("UDCEdit", "Load-120", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 473
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        'End of Security Block

    End Sub

    Private Sub UpdateUDC()

        If txtName.Text.Equals("") Then
            cpnlErrorPanel.Visible = True
            cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
            cpnlErrorPanel.TitleCSS = "test3"
            Image1.ImageUrl = "../../Images/warning.gif"
            lblError.ForeColor = Color.Red
            lblError.Text = "Name cannot be blank..."
            '    txtCompanyDDL.Items(0).Text = txtCompanyName.Text
            '  txtCompanyDDL.Items(0).Value = txtCompany.Text
            '   txtCompanyDDL.SelectedIndex = 0
            Exit Sub
        End If

        Dim sqrdCheck As SqlDataReader
        Dim blnCheck As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            ' SQL.DBTable = "UDC"

            Dim arColumnName As New ArrayList
            Dim arRowValue As New ArrayList

            arColumnName.Add("Name")
            arColumnName.Add("Description")
            arColumnName.Add("Company")

            arRowValue.Add(txtName.Text.Trim.ToUpper)
            arRowValue.Add(txtDescription.Text.Trim)
            If CDDLCustomer.CDDLGetValue = "" Then
                arRowValue.Add(0)
            Else
                arRowValue.Add(CDDLCustomer.CDDLGetValue)
            End If


            Dim strSelect As String

            '    If mstrUDCCompany.Trim.Equals("") Then
            strSelect = "select * from UDC where ProductCode=" & mintProductCode & " and UDCType ='" & mstrUDCType.Trim & "' and Name='" & mstrUDCName.Trim & "'"
            '       Else
            '         strSelect = "select * from UDC where ProductCode=" & mintProductCode & " and UDCType ='" & mstrUDCType.Trim & "' and Name='" & mstrUDCName.Trim & "' and Company='" & txtCompany.Text.Trim & "'"
            '    End If

            If SQL.Update("UDC", "UDC_Edit", "UpdateUDC-183", strSelect, arColumnName, arRowValue) = False Then
                Image1.ImageUrl = "../../Images/warning.gif"
                lblError.ForeColor = Color.Red
                cpnlErrorPanel.Visible = True
                cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
                cpnlErrorPanel.TitleCSS = "test3"
                lblError.Text = "Error Occured while updating..."
                Exit Sub
            Else
                Image1.ImageUrl = "../../Images/Pok.gif"
                lblError.ForeColor = Color.Black
                cpnlErrorPanel.Visible = True
                'cpnlErrorPanel.TitleCSS = "test3"
                cpnlErrorPanel.Text = "Message"
                cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
                lblError.Text = "Data Updated..."
                Call FillForm(mintProductCode, mstrUDCType, txtName.Text.Trim.ToUpper)
                Exit Sub
            End If
        Catch ex As Exception
            Image1.ImageUrl = "../../Images/error_image.gif"
            lblError.ForeColor = Color.Red
            CreateLog("UDCEdit", "UpdateUDC-202", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try

    End Sub

    'Private Sub Toolbar1_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Toolbar1.ButtonClick
    '    Dim objButton As ToolbarItem = CType(sender, ToolbarItem)

    '    Select Case objButton.ID
    '        Case "tbrbtnSave"
    '            UpdateUDC()
    '        Case "tbrbtnClose"
    '            Response.Write("<script>window.close();</script>")
    '        Case "tbrbtnOk"
    '            UpdateUDC()
    '            Response.Write("<script>window.close();</script>")
    '    End Select
    'End Sub

    Private Sub FillForm(ByVal ProductCode As Int32, ByVal UDCType As String, ByVal Name As String)
        Dim drUDCType As SqlDataReader
        Dim blnUDCType As Boolean
        Dim strSql As String
        Try
            If Session("PropCompanyType") = "SCM" Then
                drUDCType = SQL.Search("UDCType_Edit", "FillForm-227", "select *,CI_VC36_Name as CompanyName from UDC,T010011 where company*=CI_NU8_Address_Number And ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' And Name='" & Name & "'", SQL.CommandBehaviour.CloseConnection, blnUDCType)
            Else
                If IsNothing(ViewState("UDCCompany")) Then
                    strSql = "select *,' ' as CompanyName from UDC where  ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' And Name='" & Name & "' and company=0 "
                Else
                    strSql = strSql & " select *,CI_VC36_Name as CompanyName from UDC,T010011 where company=CI_NU8_Address_Number and  ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' And Name='" & Name & "' and company=" & ViewState("UDCCompany")
                End If
                drUDCType = SQL.Search("UDCType_Edit", "FillForm-227", strSql, SQL.CommandBehaviour.CloseConnection, blnUDCType)
            End If
            If blnUDCType = True Then
                drUDCType.Read()
                'txtCompany.Text = drUDCType.Item("Company")
                'txtCompanyDDL.Items(0).Text = IIf(IsDBNull(drUDCType.Item("CompanyName")), "", drUDCType.Item("CompanyName"))
                'txtCompanyDDL.Items(0).Value = drUDCType.Item("Company")
                'txtCompanyDDL.SelectedIndex = 0
                'txtCompanyName.Text = IIf(IsDBNull(drUDCType.Item("CompanyName")), "", drUDCType.Item("CompanyName"))
                CDDLCustomer.CDDLSetSelectedItem(drUDCType.Item("Company"), False, IIf(IsDBNull(drUDCType.Item("CompanyName")), "", drUDCType.Item("CompanyName")))

                txtDescription.Text = drUDCType.Item("Description")
                txtName.Text = drUDCType.Item("Name")
                txtProductCode.Text = drUDCType.Item("ProductCode")
                txtUCDTypeF.Text = drUDCType.Item("UdcType")
            End If
        Catch ex As Exception
            CreateLog("UDCType_Edit", "FillForm-220", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("ProUserId"), Session("ProUserName"))
        End Try
    End Sub
End Class
