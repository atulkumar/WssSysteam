'***********************************************************************************************************
' Page                 :-Add Help 
' Purpose              :-Purpose of this screen is to add help file to WSS.
' Date					   Author						Modification Date				Description
' 18/05/06	  	          Harpreet           			                                Created
'
' Notes: 
' Code:
'***********************************************************************************************************
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.Web.Security

Partial Class Help_AddHelp
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        txtCSS(Me.Page)

        imgReset.Attributes.Add("onclick", "return Reset();")

        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")

        If strhiddenImage <> "" Then

            Select Case strhiddenImage

                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        If IsPostBack = False Then
            Call FillScreenID()
            GetScreenInfo(ddlScrID.SelectedValue)
        End If


    End Sub
    Private Function FillScreenID()
        Try
            Dim sqRDR As SqlDataReader
            Dim blnStatus As Boolean
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            '  SQL.DBTable = "T070011"

            sqRDR = SQL.Search("", "", "select OBM_IN4_Object_ID_PK, obm_vc50_Alias_Name from T070011 where (OBM_VC4_Object_Type_FK='SCR' or OBM_VC4_Object_Type_FK='POP') and obm_vc4_status_code_fk='ENB' order by obm_vc50_Alias_Name", SQL.CommandBehaviour.Default, blnStatus)
            If blnStatus = True Then
                ddlScrID.Items.Clear()
                While sqRDR.Read
                    ddlScrID.Items.Add(New ListItem(sqRDR("obm_vc50_Alias_Name"), sqRDR("OBM_IN4_Object_ID_PK")))

                End While
                sqRDR.Close()
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Sub ddlScrID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScrID.SelectedIndexChanged
        'cpnlError.Visible = False
        txtScreenID.Text = ddlScrID.SelectedValue
        Call GetScreenInfo(ddlScrID.SelectedValue)

    End Sub

    Private Sub GetScreenInfo(ByVal intScreenID As Integer)
        txtScreenID.Text = intScreenID
        Try
            Dim blnStatus As Boolean
            Dim sqRDR As SqlDataReader
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            'SQL.DBTable = "T070011"

            sqRDR = SQL.Search("", "", "select obm_vc50_object_Name, obm_vc200_helpfileurl,obm_vc1000_HelpTitle from T070011 where  OBM_IN4_Object_ID_PK=" & intScreenID, SQL.CommandBehaviour.Default, blnStatus)
            If blnStatus = True Then
                While sqRDR.Read
                    txtHelpFile.Text = IIf(IsDBNull(sqRDR("obm_vc200_helpfileurl")), "", sqRDR("obm_vc200_helpfileurl"))
                    txtHelpTitle.Text = IIf(IsDBNull(sqRDR("obm_vc1000_HelpTitle")), "", sqRDR("obm_vc1000_HelpTitle"))
                End While
                sqRDR.Close()
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Function SaveHelpInfo() As Boolean
        Try
            Dim arColumn As New ArrayList
            Dim arRow As New ArrayList

            arColumn.Add("OBM_VC200_HelpFileURL")
            arColumn.Add("OBM_VC1000_HelpTitle")

            arRow.Add(txtHelpFile.Text.Trim)
            arRow.Add(txtHelpTitle.Text.Trim)
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T070011"

            If SQL.Update("T070011", "", "", "select * from T070011 where OBM_IN4_Object_ID_PK=" & ddlScrID.SelectedValue, arColumn, arRow) = True Then
                'cpnlError.Visible = True
                'cpnlError.Text = "Message"
                lstError.Items.Clear()
                lstError.Items.Add("Record Saved Successfully")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                'ImgError.ImageUrl = "../Images/Pok.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
            Else
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                lstError.Items.Clear()
                lstError.Items.Add("Critical Error Occured")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                'ImgError.ImageUrl = "../Images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Function ValidateHelpInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        If txtHelpFile.Text = "" Then
            lstError.Items.Add("Help File Cannot be Blank")
            shFlag = 1
        End If
        If txtHelpTitle.Text = "" Then
            lstError.Items.Add("Help Title Cannot be Blank")
            shFlag = 1
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../Images/warning.gif"
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            Return False
        Else
            Return True
        End If

    End Function
    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        If ValidateHelpInfo() = True Then
            SaveHelpInfo()
        End If
    End Sub

End Class
