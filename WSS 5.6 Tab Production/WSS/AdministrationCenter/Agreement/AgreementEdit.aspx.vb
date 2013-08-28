'************************************************************************************************************
' Page                 : - Aggrement_Edit
' Purpose              : - It is meant for Editing the Aggrement Number, Customer, call type, task type,                                price etc.
' Tables used          : - T070011, T070031, T070042, T060011, T060022, T030212,T030201

' Date					Author	Jaswinder					Modification Date					Description
' 27/04/06											       -------------------					Created
'
' Notes: 
' Code:
'************************************************************************************************************

Imports System.Data.SqlClient
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class AdministrationCenter_Agreement_AgreementEdit
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents imgOk As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents pnlMsg As System.Web.UI.WebControls.Panel
    'Protected WithEvents imgSave As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgReset As System.Web.UI.WebControls.ImageButton
    ' Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    ' Protected WithEvents cpnlError As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents lblName3 As System.Web.UI.WebControls.Label
    ' Protected WithEvents txtStatusName As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents lblName4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtTaskTypeName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblName7 As System.Web.UI.WebControls.Label
    Protected WithEvents txtTaskOwner As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblName9 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblName6 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    ' Protected WithEvents txtCustomer As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents txtAGNo As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents txtPrice As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents imgError As System.Web.UI.WebControls.Image

    ' Protected WithEvents txtLineNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblName8 As System.Web.UI.WebControls.Label
    ' Protected WithEvents rblHour As System.Web.UI.WebControls.RadioButtonList
    ' Protected WithEvents lblTitleLabelAggEdit As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        If Session("PropCompanyType") = "SCM" Then
            cddlcall.CDDLQuery = "select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=0 and UDCType=""CALL"" Order By Name"
        Else
            cddlcall.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""CALL""" & _
            " and UDC.Company=" & Session("PropCompanyID") & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
            " and UDC.Company=0 Order By Name"
        End If

        cddlcall.CDDLUDC = True

        cddlcall.CDDLMandatoryField = True

        If Session("PropCompanyType") = "SCM" Then
            cddlTask.CDDLQuery = "select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=0 and UDCType=""TKTY"" Order By Name"
        Else
            cddlTask.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TKTY""" & _
            " and UDC.Company=" & Session("PropCompanyID") & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
            " and UDC.Company=0 Order By Name"
        End If

        cddlTask.CDDLUDC = True

        If Session("PropCompanyType") = "SCM" Then
            cddllevel.CDDLQuery = "select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=0 and UDCType=""LEVL"" Order By Name"
        Else
            cddllevel.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""LEVL""" & _
            " and UDC.Company=" & Session("PropCompanyID") & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""PRIO""" & _
            " and UDC.Company=0 Order By Name"
        End If

        cddllevel.CDDLUDC = True

        If Not IsPostBack Then
            cddlcall.CDDLFillDropDown()
            cddlcall.Width = Unit.Pixel(129)
            cddlTask.CDDLFillDropDown()
            cddlTask.Width = Unit.Pixel(129)
            cddllevel.CDDLFillDropDown()
            cddllevel.Width = Unit.Pixel(129)
        Else
            cddlcall.CDDLSetItem()
            cddlTask.CDDLSetItem()
            cddllevel.CDDLSetItem()
        End If
        Call txtCSS(Me.Page)   'From Module
        If Not IsPostBack Then

            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        End If
        If IsPostBack = False Then
            txtLineNo.Value = Request.QueryString("CodeID")
            FillDetails()
        End If

        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Save"

                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            '  cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If updateAgreement() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Agreement Line updated successfully...")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server Is busy Please try Later...")
                        End If
                    Case "Ok"

                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If updateAgreement() = True Then

                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server Is busy Please try Later...")
                        End If
                End Select

            Catch ex As Exception

            End Try
        End If

        'Security Block

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 556
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        'End of Security Block
    End Sub

#Region "fill contacts"
    Private Sub FillDetails()
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'Dim dsSearch As DataSet
        'Dim inti As Integer
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String

        strSql = "select *,CI_VC36_Name from T080022,T010011 WHERE AL_VC8_Customer=CI_NU8_Address_Number and AL_NU9_Line_No_PK=" & txtLineNo.Value.Trim

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        'SQL.DBTable = "T080022"

        Try
            sqrdRecords = SQL.Search("Aggrement_Edit", "FillDetails-198", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                sqrdRecords.Read()

                txtCustomer.Text = IIf(IsDBNull(sqrdRecords.Item("CI_VC36_Name")), " ", sqrdRecords.Item("CI_VC36_Name"))

                txtAGNo.Text = IIf(IsDBNull(sqrdRecords.Item("AL_NU9_Agr_No")), " ", sqrdRecords.Item("AL_NU9_Agr_No"))

                cddlcall.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("AL_VC8_Call_Type")), " ", sqrdRecords.Item("AL_VC8_Call_Type")))

                cddlTask.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("AL_VC8_Task_Type")), " ", sqrdRecords.Item("AL_VC8_Task_Type")))

                cddllevel.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("AL_VC8_Skill_Level")), " ", sqrdRecords.Item("AL_VC8_Skill_Level")))

                txtPrice.Text = IIf(IsDBNull(sqrdRecords.Item("AL_NU9_Price")), " ", sqrdRecords.Item("AL_NU9_Price"))

                rblHour.SelectedValue = IIf(IsDBNull(sqrdRecords.Item("AL_VC1_Fix_Hour")), "H", sqrdRecords.Item("AL_VC1_Fix_Hour"))


                sqrdRecords.Close()
                '            'Enable Below Panels
                '            Call EnablePanels()
            Else
                'imgError.ImageUrl = "../../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                lstError.Items.Add("Server Is busy Please try Later...")
                'cpnlError.Text = "Message"
                'cpnlError.TitleCSS = "test3"
                'cpnlError.Visible = True
                'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End If


        Catch ex As Exception
            CreateLog("Agreemnet Edit", "FillDeails-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Update Role"
    Function updateAgreement() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try

            getValues(arColumnName, arRowData)
            mstGetFunctionValue = UpdateAgreementLine(txtLineNo.Value.Trim, arColumnName, arRowData)

            If mstGetFunctionValue.ErrorCode = 0 Then
                'imgError.ImageUrl = "../../images/Pok.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ' cpnlError.Text = "Message"
                ' cpnlError.Visible = True
                'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                'imgError.ImageUrl = "../../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'cpnlError.Text = "Message"
                'cpnlError.TitleCSS = "test3"
                'cpnlError.Visible = True
                'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            'imgError.ImageUrl = "../../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            lstError.Items.Add("Server is busy please try Later...")
            'cpnlError.Text = "Message"
            'cpnlError.TitleCSS = "test3"
            'cpnlError.Visible = True
            'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("CraeteUser", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    Function UpdateAgreementLine(ByVal LineNo As Integer, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            'SQL.DBTable = "T060022"
            SQL.DBTracing = False

            If SQL.Update("T060022", "AggrementEdit", "UpdateAggrementLine", "select * from T080022 where AL_NU9_Line_No_PK=" & LineNo, ColumnName, RowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try Later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try Later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSUpdate", "UpdateUserLogin-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Sub getValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)
        arColumnName.Clear()
        arRowData.Clear()

        arColumnName.Add("AL_VC8_Call_Type")
        arColumnName.Add("AL_VC8_Task_Type")
        arColumnName.Add("AL_VC8_Skill_Level")
        arColumnName.Add("AL_NU9_Price")
        arColumnName.Add("AL_VC1_Fix_Hour")

        arRowData.Add(cddlcall.CDDLGetValue)
        arRowData.Add(cddlTask.CDDLGetValue)
        arRowData.Add(cddllevel.CDDLGetValue)
        arRowData.Add(txtPrice.Text.Trim)
        arRowData.Add(rblHour.SelectedValue)

    End Sub
#End Region
End Class
