Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_BGDailyMonitorEdit
    Inherits System.Web.UI.Page
    Public mintSQID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here

        'It hold value of sqid pass from BGDailyMonitor Form
        mintSQID = Request.QueryString("ID")
        Dim txthiddenImage As String = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then

            Select Case txthiddenImage
                Case "Ok"
                    'call updateMonitor function
                    If UpdateMonitor() = True Then
                        Response.Write("<script>window.close();</script>")
                    End If
                Case "Save"
                    'call updateMonitor function
                    If UpdateMonitor() = True Then
                    End If
                Case "Logout"
                    LogoutWSS()
            End Select

        End If

        If IsPostBack = False Then
            Dim blnStatus As Boolean
            Dim sqRDR As SqlClient.SqlDataReader
            Try
                'Dim strSQL = "select * from T130022 where RQ_NU9_SQID_PK=" & mintSQID
                sqRDR = SQL.Search("BGdailyMonitorEdit", "Monitoredit", "select * from T130022 where RQ_NU9_SQID_PK=" & mintSQID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                'if blnStatus True 
                If blnStatus = True Then
                    'sqRDR reader read data from t130022 and fill Textboxes
                    While sqRDR.Read
                        TxtDBType.Text = IIf(IsDBNull(sqRDR("RQ_VC150_CAT1")), "", sqRDR("RQ_VC150_CAT1"))
                        'if DbType is RPT
                        If TxtDBType.Text.Equals("RPT") Then
                            Dim sqRD As SqlClient.SqlDataReader
                            'Reader read Report Id
                            Dim intRid As Integer = sqRDR.Item("RQ_VC150_CAT2")
                            'this function fetch report name against report id from t130041
                            Dim strSQL As String = "select RP_VC50_AliasName from T130041 where RP_NU9_SQID_PK=" & intRid
                            TxtDBName.Text = SQL.Search("", "", strSQL)
                            'if db type is QUE
                        ElseIf TxtDBType.Text.Equals("QUE") Then
                            'fill textbox with DBName
                            TxtDBName.Text = IIf(IsDBNull(sqRDR("RQ_VC150_CAT2")), "", sqRDR("RQ_VC150_CAT2"))
                        Else
                            'in case of STS DBname textbox is enabled false
                            TxtDBName.Enabled = False
                        End If
                        txtStartDate.Text = JulianToDate(sqRDR.Item("RQ_VC150_CAT4")).ToShortDateString
                        TxtEndDate.Text = JulianToDate(sqRDR.Item("RQ_VC150_CAT7")).ToShortDateString

                        TxtTime.Text = JuliantoTime(sqRDR.Item("RQ_VC150_CAT5")).ToShortTimeString
                        DdlStatus.SelectedValue = Convert.ToString(IIf(IsDBNull(sqRDR("RQ_CH2_STATUS")), "", sqRDR("RQ_CH2_STATUS"))).Trim
                        txtReason.Text = IIf(IsDBNull(sqRDR("RQ_VC150_CAT20")), "", sqRDR("RQ_VC150_CAT20"))
                    End While
                End If
                sqRDR.Close()
            Catch ex As Exception
                CreateLog("BGDailyMonitoring", "filltextboxes-113", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            End Try
        End If

    End Sub
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used to check all fields are fill before save
    'Modify Date:       ------
    '***************************************************************************************
    Private Function ValidateMonitorEdit() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If DdlStatus.SelectedValue.Equals("") Then
            lstError.Items.Add("status Type cannot be blank...")
            shFlag = 1
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
            Exit Function
        Else
            Return True
        End If
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       3/01/2007
    'Purpose:           This function Update record in t130022 
    'Modify Date:       ------
    '***************************************************************************************
    Private Function UpdateMonitor() As Boolean

        If ValidateMonitorEdit() = False Then
            Exit Function
        End If

        Try
            'it hold Update SqlQuery
            Dim sqlUpdate As String
            'get connectionstrig from Web config
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqlUpdate = "update T130022 set RQ_CH2_STATUS='" & DdlStatus.SelectedValue & "',RQ_VC150_CAT20='" & txtReason.Text & "'  where  RQ_NU9_SQID_PK=" & mintSQID
            If SQL.Update("DBDailyMonitorEditEdit", "UpdateDBDailyMonitorEditEdit", sqlUpdate, SQL.Transaction.Serializable) = True Then
                'if SQL.Update is true then record updated
                lstError.Items.Clear()
                lstError.Items.Add("Record Saved successfully...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Return True
            Else
                'if SQL.Update is False then Msg Panel show msg  
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                Return False
            End If

        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy Please Try Later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("AlertFlow", "Load-154", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False

        End Try
        'End If
    End Function
    '***************************************************************************************
    'Created By:        Nitin
    'Create Date:       ------
    'Purpose:           This function is used change Julain time 
    'Modify Date:       ------
    '***************************************************************************************
    Private Function JuliantoTime(ByVal TimeString As Integer) As DateTime

        Dim strTime As String = CStr(TimeString)
        Dim intLength As Byte = Len(strTime)

        Dim intHH As Integer

        Dim intMM As Integer

        Dim intSS As Integer


        Select Case intLength

            Case 1

                intHH = 0

                intMM = 0

                intSS = strTime.Substring(0, 1)

            Case 2

                intHH = 0

                intMM = 0

                intSS = strTime.Substring(0, 2)

            Case 3

                intHH = 0

                intMM = strTime.Substring(0, 1)

                intSS = strTime.Substring(1, 2)

            Case 4

                intHH = 0

                intMM = strTime.Substring(0, 2)

                intSS = strTime.Substring(2, 2)

            Case 5

                intHH = strTime.Substring(0, 1)

                intMM = strTime.Substring(1, 2)

                intSS = strTime.Substring(3, 2)

            Case 6

                intHH = strTime.Substring(0, 2)

                intMM = strTime.Substring(2, 2)

                intSS = strTime.Substring(4, 2)

        End Select

        Dim dtTime As DateTime

        Dim dt As DateTime

        dt = dtTime.AddHours(intHH)

        dt = dt.AddMinutes(intMM)

        dt = dt.AddSeconds(intSS)

        Return dt

    End Function


    Private Sub txtSubject_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReason.TextChanged

    End Sub
End Class
