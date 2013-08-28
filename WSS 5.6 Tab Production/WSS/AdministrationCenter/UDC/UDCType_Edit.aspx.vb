'************************************************************************************************************
' Page                 : - UDCType_Edit
' Purpose              : - it’s meant for editing the UDC type, product code, UDC text, Company etc.
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
Imports System.Data
Imports System.Drawing


Partial Class AdministrationCenter_UDC_UDCType_Edit
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        'imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")
        'imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
        Call txtCSS(Me.Page)
        If Not IsPostBack Then

            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        End If
        'imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        '--Customer
        CDDLCompany.CDDLQuery = "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ") "
        CDDLCompany.CDDLMandatoryField = True
        CDDLCompany.CDDLUDC = False
        '-----------------------------------------
        If IsPostBack = False Then
            CDDLCompany.CDDLFillDropDown(10, False)
        Else
            CDDLCompany.CDDLSetItem()
        End If

        If Not IsPostBack Then
            cpnlErrorPanel.Visible = False

            Dim intProductCode As Integer
            Dim strUDCType As String
            Dim strUDCTCompany As String

            intProductCode = Request.QueryString("Code") 'HttpContext.Current.Session("SUDCTypePC")
            strUDCType = Request.QueryString("Type") 'HttpContext.Current.Session("SUDCTypeP")
            ViewState("SUDCCompany") = Request.QueryString("Company")
            'If HttpContext.Current.Session("SUDCTLock") = 1 Then
            '    chkUDCParam.Checked = True
            'Else
            '    chkUDCParam.Checked = False
            'End If

            'If IsNothing(intProductCode) Or IsNothing(strUDCType) Then
            '    Exit Sub
            'End If

            'txtUDCTypePC.Text = intProductCode
            'txtUDCTypeP.Text = strUDCType

            'If HttpContext.Current.Session("SUDCTypeText") = "&nbsp;" Then
            '    txtUDCText.Text = ""
            'Else
            '    txtUDCText.Text = HttpContext.Current.Session("SUDCTypeText")
            'End If

            'If HttpContext.Current.Session("SUDCTypeCompany") = "&nbsp;" Then
            '    txtCompany.Text = ""
            'Else
            '    txtCompany.Text = HttpContext.Current.Session("SUDCTypeCompany")
            'End If
            Call FillForm(intProductCode, strUDCType)

        End If


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
                        UpdateUDCType()
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
                        UpdateUDCType()
                        Response.Write("<script>window.close();</script>")
                End Select
            Catch ex As Exception
                CreateLog("UDCTypeEdit", "Load-121", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try

        End If

        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 474
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        'End of Security Block

    End Sub

    Private Sub UpdateUDCType()

        Dim sqrdCheck As SqlDataReader
        Dim blnCheck As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            'SQL.DBTable = "UDCType"

            Dim arColumns As New ArrayList
            Dim arRows As New ArrayList

            arColumns.Add("ProductCode")
            arColumns.Add("UDCType")
            arColumns.Add("UDCTypeText")
            arColumns.Add("UDCParam")
            arColumns.Add("Company")

            arRows.Add(txtUDCTypePC.Text.Trim)
            arRows.Add(txtUDCTypeP.Text.Trim)
            arRows.Add(txtUDCText.Text.Trim)
            If chkUDCParam.Checked = True Then
                arRows.Add(1)
            Else
                arRows.Add(0)
            End If
            arRows.Add(CDDLCompany.CDDLGetValue.Trim)

            If SQL.Update("UDCType", "UDCType_Edit", "UpdateUDCType-177", "select * from UDCType where ProductCode=" & txtUDCTypePC.Text.Trim & " and UDCType='" & txtUDCTypeP.Text.Trim & "'", arColumns, arRows) = False Then
                Image1.ImageUrl = "../../Images/warning.gif"
                lblError.ForeColor = Color.Red
                cpnlErrorPanel.Visible = True
                cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
                cpnlErrorPanel.TitleCSS = "test3"
                lblError.Text = "Server is busy please try later..."
                Exit Sub
            Else
                Image1.ImageUrl = "../../Images/Pok.gif"
                lblError.ForeColor = Color.Black
                cpnlErrorPanel.Visible = True
                'cpnlErrorPanel.TitleCSS = "test3"
                cpnlErrorPanel.Text = "Message"
                cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
                lblError.Text = "Data Updated..."
                Exit Sub
            End If
        Catch ex As Exception
            Image1.ImageUrl = "../../Images/error_image.gif"
            lblError.ForeColor = Color.Red
            CreateLog("UDCTypeEdit", "UpdateUDCType-194", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    'Private Sub Toolbar1_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Toolbar1.ButtonClick
    '    Dim objButtons As ToolbarItem = CType(sender, ToolbarItem)

    '    Try
    '        Select Case objButtons.ID
    '            Case "tbrbtnSave"
    '                UpdateUDCType()
    '            Case "tbrbtnClose"
    '                Response.Write("<script>window.close();</script>")
    '            Case "tbrbtnOk"
    '                UpdateUDCType()
    '                Response.Write("<script>window.close();</script>")
    '        End Select
    '    Catch ex As Exception
    '        CreateLog("UDCTypeEdit", "ButtonClick-213", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "ToolBar1")
    '    End Try
    'End Sub

    Private Sub FillForm(ByVal ProductCode As Int32, ByVal UDCType As String)
        Dim drUDCType As SqlDataReader
        Dim blnUDCType As Boolean
        Dim strSql As String
        Try
            If Session("PropCompanyType") = "SCM" Then
                drUDCType = SQL.Search("UDCType_Edit", "FillForm-225", "select *,CI_VC36_Name as CompanyName from UDCType,T010011 where company*=CI_NU8_Address_Number And ProductCode=" & ProductCode & " and UDCType='" & UDCType & "'", SQL.CommandBehaviour.CloseConnection, blnUDCType)
            Else
                If IsNothing(ViewState("SUDCCompany")) Then
                    strSql = "select *,' ' as CompanyName from UDCType where  ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' and company =0"
                Else
                    strSql = "select *,CI_VC36_Name as CompanyName from UDCType,T010011 where  company=CI_NU8_Address_Number and ProductCode=" & ProductCode & " and UDCType='" & UDCType & "' and company =" & ViewState("SUDCCompany")
                End If
                drUDCType = SQL.Search("UDCType_Edit", "FillForm-227", strSql, SQL.CommandBehaviour.CloseConnection, blnUDCType)
            End If


            If blnUDCType = True Then
                drUDCType.Read()
                'CDDLCompany.drUDCType.Item("Company")
                'txtCompanyName.Text = IIf(IsDBNull(drUDCType.Item("CompanyName")), "", drUDCType.Item("CompanyName"))
                CDDLCompany.CDDLSetSelectedItem(drUDCType.Item("Company"), False, IIf(IsDBNull(drUDCType.Item("CompanyName")), "", drUDCType.Item("CompanyName")))
                txtUDCText.Text = drUDCType.Item("UdcTypeText")
                txtUDCTypeP.Text = drUDCType.Item("UDCType")
                txtUDCTypePC.Text = drUDCType.Item("ProductCode")
            End If
        Catch ex As Exception
            CreateLog("UDCType_Edit", "FillForm-220", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("ProUserId"), Session("ProUserName"))
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Write("<script>window.close();</script>")
    End Sub

End Class
