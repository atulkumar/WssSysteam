#Region "Purpose"
'************************************************************************************************************
' Page                 :- User Management
' Purpose              :- To change the old password 

' Tables used          :- T060011
' Date				Author						Modification Date					Description
' 28/08/06			Harpreet			       -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
#End Region


#Region "Session Used"
'Session Used
'Session("PropUserID")
'Session("PropUserName")
#End Region

#Region "NameSpace"
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data
#End Region


Partial Class Security_UserPassword
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents BtnGrdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents imgbtnSearch As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lblTitleLabelUserSearch As System.Web.UI.WebControls.Label
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    'Protected WithEvents cpnlErrorPanel As CustomControls.Web.CollapsiblePanel
    Protected WithEvents cpnlCallTask As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents CDDLUserID As CustomDDL
    'Protected WithEvents imgSave As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgOK As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgReset As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtNewPassword As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtConNewPassword As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblUserID As System.Web.UI.WebControls.Label
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents cpnlUserPassword As CustomControls.Web.CollapsiblePanel
    'Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
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

    Private Shared mstrPassword As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        imgReset.Attributes.Add("onclick", "return SaveEdit('Reset');")
        imgSave.Attributes.Add("onclick", "return SaveEdit('Save');")
        imgOK.Attributes.Add("onclick", "return SaveEdit('Ok');")


        CDDLUserID.CDDLUDC = False
        CDDLUserID.CDDLQuery = "select UM_IN4_Address_No_FK ID,UM_VC50_UserID Name, UM_IN4_Company_AB_ID Company from t060011 where UM_VC4_Status_Code_FK='ENB'"
        CDDLUserID.CDDLFillDropDown(10, False)
        CDDLUserID.CDDLAutopostback = True
        If Not IsPostBack Then
            txtCSS(Me.Page)
            CDDLUserID.CDDLSetSelectedItem(Session("PropUserID"), False, Session("PropUserName"))
        End If


        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")

        If strhiddenImage <> "" Then

            Select Case strhiddenImage
                Case "Save"
                    If SavePassword() = True Then
                        FillDetail()
                    End If
                Case "Ok"
                    If SavePassword() = True Then
                        Response.Redirect("../Home.aspx", False)
                    End If
                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        If Not IsPostBack Then
            FillDetail()
        Else
            'txtPassword.Attributes.Add("value", txtPassword.Text)
        End If
    End Sub
    Private Sub FillDetail()
        Try
            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            sqRDR = SQL.Search("UserPassword", "FillDetail", "select UM_VC30_Password from T060011 where UM_IN4_Address_No_FK=" & CDDLUserID.CDDLGetValue, SQL.CommandBehaviour.Default, blnStatus)
            If blnStatus = True Then
                Dim strPassword As String
                sqRDR.Read()
                strPassword = sqRDR("UM_VC30_Password")
                strPassword = IONDecrypt(strPassword)
                mstrPassword = strPassword
            End If
            CDDLUserID.Enabled = False
        Catch ex As Exception
            CreateLog("Security_UserPassword", "FillDetail-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Function SavePassword() As Boolean
        Try
            If txtPassword.Text.Equals("") And txtNewPassword.Text.Equals("") And txtConNewPassword.Text.Equals("") Then
                Exit Function
            End If
            Dim shFlag As Short
            shFlag = 0
            lstError.Items.Clear()
            If txtPassword.Text <> mstrPassword Then
                lstError.Items.Add("Old password is incorrect...")
                shFlag = 1
            End If
            If txtNewPassword.Text <> txtConNewPassword.Text Then
                lstError.Items.Add("Passwords donot match...")
                shFlag = 1
            End If
            If txtNewPassword.Text.Length < 6 Then
                lstError.Items.Add("Password length cannot be less than 6 characters...")
                shFlag = 1
            End If
            If txtNewPassword.Text.IndexOf(" ") >= 0 Then
                lstError.Items.Add("Password cannot contain blank spaces...")
                shFlag = 1
            End If
            If txtNewPassword.Text = mstrPassword Then
                lstError.Items.Add("New Password cannot be same as Old Password...")
                shFlag = 1
            End If
            If shFlag = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Return False
            End If
            Dim arrCol As New ArrayList
            Dim arrRow As New ArrayList
            arrCol.Add("UM_VC30_Password")
            arrCol.Add("UM_CH1_Mail_Sent_Modify")
            arrRow.Add(IONEncrypt(txtNewPassword.Text.Trim))
            arrRow.Add("F")
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            'SQL.DBTable = "T060011"

            If SQL.Update("T060011", "UserPassword", "SavePassword", "select * from T060011 where UM_IN4_Address_No_FK=" & CDDLUserID.CDDLGetValue, arrCol, arrRow) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Your password has been changed...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Error occur please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                Return False
            End If

        Catch ex As Exception
            CreateLog("Security_UserPassword", "SavePassword-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, Session("PropUserID"), Session("PropUserName"), "NA")
            Return False
        End Try
    End Function
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
