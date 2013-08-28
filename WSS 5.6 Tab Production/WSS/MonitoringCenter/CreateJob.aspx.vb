Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging

Partial Class MonitoringCenter_CreateJob
    Inherits System.Web.UI.Page
    Public intSeqID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)

        btnClick.Attributes.Add("onclick", "SaveEdit('Save');")

        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not IsPostBack Then
            mstrqID = Request.QueryString("ID")
        End If

        If txthiddenImage <> "" Then

            Select Case txthiddenImage

                Case "Close"
                    Response.Redirect("ViewJobs.aspx")
                Case "Ok"
                    If SaveJobInfo() = True Then
                        Response.Redirect("ViewJobs.aspx")
                    End If
                Case "Save"
                    SaveJobInfo()
                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        If mstrqID <> "-1" Then
            Dim sqrdr As System.Data.SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Try
                sqrdr = SQL.Search("CreateJob", "Load", "select * from T130032 where EP_NU9_SEQ_ID=" & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                If blnStatus = True Then
                    While sqrdr.Read
                        txtProcessCode.Text = sqrdr("EP_NU9_Porcess_ID")
                        dtJobDate.CalendarDate = JulianToDate(sqrdr("EP_VC50_Field1"))
                        txtJobTime.Text = sqrdr("EP_VC50_Field2")
                        txtMachineCode.Text = sqrdr("EP_NU9_Machine_Code")
                    End While
                    sqrdr.Close()
                End If
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                'ImgError.ImageUrl = "../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                CreateLog("CreateJob", "Load-101", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If
        If dtJobDate.CalendarDate.Equals("") Then
            dtJobDate.CalendarDate = Today
        End If

    End Sub

    Private Function SaveJobInfo() As Boolean
        Try

            If ValidateJobInfo() = True Then
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                'SQL.DBTable = "T130032"
                SQL.DBTracing = False

                arColumnName.Add("EP_NU9_Porcess_ID")
                arColumnName.Add("EP_VC50_Field1")
                arColumnName.Add("EP_VC50_Field2")
                arColumnName.Add("EP_NU9_Machine_Code")
                If mstrqID = "-1" Then
                    arColumnName.Add("EP_NU9_SEQ_ID")
                    arColumnName.Add("EP_CH2_Status")
                End If
                arRowData.Add(txtProcessCode.Text.Trim)
                arRowData.Add(DateToJulian(dtJobDate.CalendarDate))
                arRowData.Add(txtJobTime.Text.Trim)
                arRowData.Add(txtMachineCode.Text.Trim)
                If mstrqID = "-1" Then
                    intSeqID = 0
                    intSeqID = SQL.Search("CreateJob", "SaveJobInfo", "select max(EP_NU9_SEQ_ID) from T130032")
                    intSeqID += 1
                    arRowData.Add(intSeqID)
                    arRowData.Add("C")
                End If
                If mstrqID = "-1" Then
                    If SQL.Save("T130032", "CreateJob", "SaveJobInfo", arColumnName, arRowData) = True Then
                        mstrqID = intSeqID
                        lstError.Items.Clear()
                        cpnlError.Visible = True
                        'cpnlError.Text = "Message"
                        lstError.Items.Add("Record saved successfully...")
                        ' ImgError.ImageUrl = "../images/Pok.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Server is busy please try later...")
                        cpnlError.Visible = True
                        'cpnlError.Text = "Error Message"
                        'ImgError.ImageUrl = "../images/error_image.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        Return False
                    End If
                Else
                    If SQL.Update("T130032", "Createjob", "SaveJobInfo", "select * from T130032 where EP_NU9_SEQ_ID=" & mstrqID & "", arColumnName, arRowData) = True Then
                        lstError.Items.Clear()
                        ' cpnlError.Visible = True
                        ' cpnlError.Text = "Message"
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
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("CreateJob", "SaveJobInfo-184", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try

    End Function
    Private Function ValidateJobInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtProcessCode.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Process Codel cannot be empty...")
        End If
        If txtMachineCode.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine Code cannot be empty...")
        End If
        If dtJobDate.CalendarDate.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Job Date cannot be empty...")
        End If
        If txtJobTime.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Job Time cannot be empty...")
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
        Try
            Dim introws As Integer

            If SQL.Search("CreateJob", "ValidateJobInfo", "select  MM_IN4_MCode from t130011 where MM_IN4_MCode='" & txtMachineCode.Text & "'", introws) = False Then
                lstError.Items.Add("Machine Key Mismatch...")
                shFlag = 1
            End If
            If SQL.Search("CreateJob", "ValidateJobInfo", "select  PM_IN4_PCode from t130031 where PM_IN4_PCode='" & txtProcessCode.Text & "'", introws) = False Then
                lstError.Items.Add("Process Code Mismatch...")
                shFlag = 1
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
            'cpnlError.Text = "error message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("CreateJob", "ValidateJobInfo-247", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
    End Function


    Private Sub btnClick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClick.Click

    End Sub
End Class
