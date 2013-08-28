'*********************************************************************************************************
' Page                   : - StatusUDCEdit
' Purpose                : - User can edit UDC status & it’s Description, status code as well through                                     StatusUDCEdit screen. 
' Tables used            : - T040081
' Date					Author						Modification Date					Description
' 05/03/06				Harpreet singh			   -------------------					Created
'
' Notes: 
' Code:
'*********************************************************************************************************
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Partial Class AdministrationCenter_StatusUDC_StatusUDCEdit
    Inherits System.Web.UI.Page

    Public intID As Integer
    Public intCompID As Integer
    Public intScrID As Integer
    Private Shared mstrStatusName As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Call txtCSS(Me.Page)
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")

            txtDescription.Attributes.Add("OnKeyPress", "return MaxLength('" & txtDescription.ClientID & "','450');")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            txtStatusCode.Attributes.Add("onkeypress", "NumericOnly();")
        End If
        intID = Request.QueryString("ID")
        intCompID = Request.QueryString("compID")
        intScrID = Request.QueryString("scrID")
        Dim txthiddenImage As String
        txthiddenImage = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then

            Select Case txthiddenImage
                Case "Save"
                    If UpdateStatusUDC() = True Then
                        Session("gridBindStatus") = 1
                    End If
                Case "Ok"
                    If UpdateStatusUDC() = True Then
                        Session("gridBindStatus") = 1
                        Response.Write("<script>window.close();</script>")
                    End If
            End Select

        End If



        If IsPostBack = False Then
            Dim sqRDR As System.Data.SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Try
                sqRDR = SQL.Search("StatusUDCEdit", "Load", "select * from T040081 where SU_NU9_ID_PK=" & intID & " and (SU_NU9_CompID=0 or SU_NU9_CompID=" & intCompID & ") and (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=" & intScrID & ")", SQL.CommandBehaviour.Default, blnStatus)

                While sqRDR.Read
                    txtScreenName.Text = Request.QueryString("scrName")
                    txtCompany.Text = Request.QueryString("compName")
                    txtStatusName.Text = sqRDR("SU_VC50_Status_Name")
                    mstrStatusName = ""
                    mstrStatusName = txtStatusName.Text.Trim
                    txtDescription.Text = sqRDR("SU_VC500_Status_Description")
                    txtStatusCode.Text = sqRDR("SU_NU9_Status_Code")
                End While
                sqRDR.Close()
            Catch ex As Exception

            End Try
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' -- Security Block
        'Dim intId As Integer

        If 1 = 1 Then 'This is a fake block for executing security because visibility of controls is changing in programming 
            Dim str As String
            str = Session("PropRootDir")
            '   intId = 549
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(549) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, 549)
        End If
        ' --End of Security Block------------------------------------------------------
    End Sub
    Private Function UpdateStatusUDC() As Boolean
        If ValidateInput() = True Then
            Dim arCol As New ArrayList
            Dim arRow As New ArrayList

            arCol.Add("SU_VC50_Status_Name")
            arCol.Add("SU_VC500_Status_Description")
            arCol.Add("SU_NU9_Status_Code")

            arRow.Add(txtStatusName.Text.Trim)
            arRow.Add(txtDescription.Text.Trim.Trim)
            arRow.Add(txtStatusCode.Text.Trim)

            Try
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                SQL.DBTracing = False
                ' SQL.DBTable = "T040081"

                If SQL.Update("T040081", "StatusUDCEdit", "UpdateStatusUDC", "select * from T040081 where SU_NU9_ID_PK=" & intID & " and SU_NU9_CompID=" & intCompID & " and SU_NU9_ScreenID=" & intScrID, arCol, arRow) = True Then
                    lstError.Items.Clear()
                    lstError.Items.Add("Record Updated successfully...")
                    ' ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                    Return False
                End If
            Catch ex As Exception
                If ex.Message = "There is no row at position 0." Then
                    lstError.Items.Clear()
                    lstError.Items.Add("You did't have permission to edit this UDC as this UDC is common used by other companies also...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                End If
                CreateLog("StatusUDC", "SaveStatusUDC-118", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
                Return True
            End Try
        End If
    End Function
    Private Function ValidateInput() As Boolean
        Dim shFlag As Short
        lstError.Items.Clear()
        If txtStatusName.Text.Equals("") Then
            lstError.Items.Add("Status name cannot be blank...")
            shFlag = 1
        End If
        If txtDescription.Text.Equals("") Then
            lstError.Items.Add("Status description cannot be blank...")
            shFlag = 1
        End If
        If txtStatusCode.Text.Equals("") Then
            lstError.Items.Add("Status code cannot be blank...")
            shFlag = 1
        End If
        If txtStatusName.Text <> mstrStatusName Then

            Try

                SQL.DBTracing = False
                ' SQL.DBTable = "T040081"
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                Dim dsTemp As New System.Data.DataSet
                Dim strSQL As String
                strSQL = "select * from T040081 where (SU_NU9_ScreenID=0 or SU_NU9_ScreenID=" & intScrID & ") and (SU_NU9_CompID=0 or SU_NU9_CompID=" & intCompID & ") and SU_VC50_Status_Name='" & txtStatusName.Text.Trim & "'"
                If SQL.Search("T040081", "StatusUDCEdit", "validateInput", strSQL, dsTemp, "", "") = True Then
                    lstError.Items.Add("This UDC is already used...")
                    shFlag = 1
                End If


            Catch ex As Exception
                CreateLog("StatusUDCEdit", "ValidateInput-118", LogType.Application, LogSubType.Exception, "", ex.Message, Session("PropUserID"), Session("PropUserName"), "")
            End Try
        End If
        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If
    End Function


End Class
