Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging

Partial Class MonitoringCenter_ProcessEdit
    Inherits System.Web.UI.Page
    Public shFlag As Short

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtProcessDescription.Attributes.Add("OnKeyPress", " return MaxLength('" & txtProcessDescription.ClientID & "','500');")
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not IsPostBack Then
            mstrqID = Request.QueryString("ID")
        End If

        If txthiddenImage <> "" Then

            Select Case txthiddenImage
                Case "Close"
                    Response.Redirect("SearchProcess.aspx")
                Case "Ok"
                    If SaveProcessInfo() = True Then
                        Response.Redirect("SearchProcess.aspx")
                    End If
                Case "Save"
                    SaveProcessInfo()
                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        txtProcessCode.ReadOnly = True
        Dim sqrdr As System.Data.SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqrdr = SQL.Search("ProcessEdit", "Load", "select * from T130031 where PM_IN4_PCode=" & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                While sqrdr.Read
                    txtProcessCode.Text = IIf(Not IsDBNull(sqrdr("PM_IN4_PCode")), sqrdr("PM_IN4_PCode"), "")
                    txtProcessEnvironment.Text = IIf(Not IsDBNull(sqrdr("PM_VC20_PENV")), sqrdr("PM_VC20_PENV"), "")
                    txtProcessName.Text = IIf(Not IsDBNull(sqrdr("PM_VC20_PName")), sqrdr("PM_VC20_PName"), "")
                    txtProcessType.Text = IIf(Not IsDBNull(sqrdr("PM_VC10_PType")), sqrdr("PM_VC10_PType"), "")
                    txtProcessDescription.Text = IIf(Not IsDBNull(sqrdr("PM_VC500_PDesc")), sqrdr("PM_VC500_PDesc"), "")
                    txtProcessPath.Text = IIf(Not IsDBNull(sqrdr("PM_VC250_PPath")), sqrdr("PM_VC250_PPath"), "")
                    txtProcessEXE1.Text = IIf(Not IsDBNull(sqrdr("PM_VC50_PExe")), sqrdr("PM_VC50_PExe"), "")
                    Dim strAck As String = IIf(Not IsDBNull(sqrdr("PM_VC1_PAck")), sqrdr("PM_VC1_PAck"), "")
                    If strAck = "Y" Then
                        chkAck.Checked = True
                    Else
                        chkAck.Checked = False
                    End If

                End While
                sqrdr.Close()
            End If
        Catch ex As Exception
            CreateLog("ProcessEdit", "Load-105", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Private Function SaveProcessInfo() As Boolean
        Try

            If ValidateProcessInfo() = True Then
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '  SQL.DBTable = "T130031"
                SQL.DBTracing = False

                arColumnName.Add("PM_VC20_PENV")
                arColumnName.Add("PM_VC20_PName")
                arColumnName.Add("PM_VC10_PType")
                arColumnName.Add("PM_VC500_PDesc")
                arColumnName.Add("PM_VC250_PPath")
                arColumnName.Add("PM_VC50_PExe")
                arColumnName.Add("PM_VC1_PAck")
                arColumnName.Add("PM_VC20_UUser")
                arColumnName.Add("PM_NU8_UDate")
                arColumnName.Add("PM_NU6_UTime")
                arColumnName.Add("PM_VC20_UMID")

                arRowData.Add(txtProcessEnvironment.Text.Trim)
                arRowData.Add(txtProcessName.Text.Trim)
                arRowData.Add(txtProcessType.Text.Trim)
                arRowData.Add(txtProcessDescription.Text.Trim)
                arRowData.Add(txtProcessPath.Text.Trim)
                arRowData.Add(txtProcessEXE1.Text.Trim)
                If chkAck.Checked = True Then
                    arRowData.Add("Y")
                Else
                    arRowData.Add("N")
                End If
                arRowData.Add(HttpContext.Current.Session("PropUserName"))
                arRowData.Add(PropSetDate)
                arRowData.Add(PropSetTime)
                arRowData.Add(Network.GetIPAddress("", "", Network.GetMachineName("", "")))

                If SQL.Update("T130031", "ProcessEdit", "SaveProcessInfo", "select * from T130031 where PM_IN4_PCode=" & mstrqID & "", arColumnName, arRowData) = True Then
                    mstrqID = txtProcessCode.Text.Trim
                    lstError.Items.Clear()
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Message"
                    lstError.Items.Add("Record updated successfully...")
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    Return True
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("Server is busy please try later...")
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Error Message"
                    'ImgError.ImageUrl = "../images/error_image.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If

        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            '         cpnlError.Visible = True
            '         cpnlError.Text = "Error Message"
            '         ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEdit", "SaveProcessInfo-175", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function

    Private Function ValidateProcessInfo() As Boolean
        shFlag = 0
        lstError.Items.Clear()

        If txtProcessCode.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Code cannot be empty...")
        End If
        If txtProcessEnvironment.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Environment cannot be empty...")
        End If
        If txtProcessName.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Name cannot be empty...")
        End If
        If txtProcessType.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Type cannot be empty...")
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        End If
        lstError.Items.Clear()

        Dim arrUDCList As New ArrayList
        arrUDCList.Add("PRTY")
        arrUDCList.Add("PACK")
        CheckUDC(arrUDCList)

        Try
            Dim intRows As Integer
            If mstrqID = "-1" Then
                If SQL.Search("ProcessEdit", "ValidateProcessInfo", "select  * from t130031 where PM_IN4_PCode=" & txtProcessCode.Text.Trim & " and PM_VC20_PENV='" & txtProcessEnvironment.Text.Trim & "'", intRows) = True Then
                    lstError.Items.Add("This Process Already Exists in the given Environment...")
                    shFlag = 1
                End If
            End If

            If shFlag = 1 Then
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                'ImgError.ImageUrl = "../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If

            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("ProcessEdit", "ValidateProcessInfo-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function
    Public Function CheckUDC(ByVal arrUDC As ArrayList) As Boolean
        Dim intCount As Integer
        Dim intRows As Integer
        For intCount = 0 To arrUDC.Count - 1
            Select Case arrUDC.Item(intCount)
                Case "PRTY"
                    If SQL.Search("ProcessEdit", "CheckUDC", "select  Name from UDC where UDCType='PRTY' and Name='" & txtProcessType.Text.Trim & "'", intRows) = False Then
                        lstError.Items.Add("Process Type Mismatch...")
                        shFlag = 1
                    End If
            End Select
        Next

        If shFlag = 1 Then
            Return False
        Else
            Return True
        End If

    End Function

End Class
