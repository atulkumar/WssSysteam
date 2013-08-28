Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging

Partial Class MonitoringCenter_MachineEdit
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtMachineDesc.Attributes.Add("OnKeyPress", "return MaxLength('" & txtMachineDesc.ClientID & "','100');")


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
                    Response.Redirect("SearchMachine.aspx")
                Case "Ok"
                    If SaveMachineInfo() = True Then
                        Response.Redirect("SearchMachine.aspx")
                    End If
                Case "Save"
                    SaveMachineInfo()
                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        txtMachineCode.ReadOnly = False
        If mstrqID <> "-1" Then
            txtMachineCode.ReadOnly = True
            Dim sqrdr As System.Data.SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Try
                sqrdr = SQL.Search("MachineEdit", "load", "select * from T130011 where MM_IN4_MCode=" & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                If blnStatus = True Then
                    While sqrdr.Read
                        txtMachineKey.Text = IIf(IsDBNull(sqrdr("MM_VC30_MKey")), "", sqrdr("MM_VC30_MKey"))
                        txtMachineCode.Text = IIf(IsDBNull(sqrdr("MM_IN4_MCode")), "", sqrdr("MM_IN4_MCode"))
                        txtMachineDesc.Text = IIf(IsDBNull(sqrdr("MM_VC100_MDesc")), "", sqrdr("MM_VC100_MDesc"))
                        txtMachineIP.Text = IIf(IsDBNull(sqrdr("MM_VC20_MIP")), "", sqrdr("MM_VC20_MIP"))
                        txtMachineLocation.Text = IIf(IsDBNull(sqrdr("MM_VC10_MSystem")), "", sqrdr("MM_VC10_MSystem"))
                        txtMachineName.Text = IIf(IsDBNull(sqrdr("MM_VC20_MName")), "", sqrdr("MM_VC20_MName"))
                        txtEmail.Text = IIf(IsDBNull(sqrdr("MM_VC50_MMail")), "", sqrdr("MM_VC50_MMail"))
                    End While
                    sqrdr.Close()
                End If
            Catch ex As Exception
                CreateLog("MachineEdit", "Load-99", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")
            End Try
        End If
    End Sub

    Private Function SaveMachineInfo() As Boolean
        Try

            If ValidateMachineInfo() = True Then

                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                '	SQL.DBTable = "T130011"
                SQL.DBTracing = False

                arColumnName.Add("MM_VC30_MKey")
                arColumnName.Add("MM_VC20_MName")
                arColumnName.Add("MM_VC20_MIP")
                arColumnName.Add("MM_VC10_MSystem")             'machine location e.g. ION or LF
                arColumnName.Add("MM_VC100_MDesc")
                arColumnName.Add("MM_NU12_MReq")
                arColumnName.Add("MM_NU12_MRes")
                arColumnName.Add("MM_VC50_MMail")
                arColumnName.Add("MM_VC20_UUser")
                arColumnName.Add("MM_NU8_UDate")
                arColumnName.Add("MM_NU6_UTime")
                arColumnName.Add("MM_VC20_UMID")

                arRowData.Add(txtMachineKey.Text.Trim)
                arRowData.Add(txtMachineName.Text.Trim)
                arRowData.Add(txtMachineIP.Text.Trim)
                arRowData.Add(txtMachineLocation.Text.Trim)
                arRowData.Add(txtMachineDesc.Text.Trim)

                'get max from table T130011 and then increment
                Dim intMReq As Integer
                intMReq = 0
                intMReq = SQL.Search("MachineEdit", "SaveMachineInfo", "Select max(MM_NU12_MReq) from T130011")
                intMReq += 1
                arRowData.Add(intMReq)
                'get max from table T130011 and then increment
                Dim intMRes As Integer
                intMRes = 0
                intMRes = SQL.Search("MachineEdit", "SaveMachineInfo", "Select max(MM_NU12_MRes) from T130011")
                intMRes += 1
                arRowData.Add(intMRes)

                arRowData.Add(txtEmail.Text.Trim)
                arRowData.Add(HttpContext.Current.Session("PropUserName"))
                arRowData.Add(PropSetDate)
                arRowData.Add(PropSetTime)
                arRowData.Add(Network.GetIPAddress("", "", Network.GetMachineName("", "")))
                ' Store the tag values

                If SQL.Update("T130011", "", "", "select * from T130011 where MM_IN4_MCode=" & mstrqID & "", arColumnName, arRowData) = True Then
                    mstrqID = txtMachineCode.Text
                    lstError.Items.Clear()
                    'cpnlError.Visible = True
                    'cpnlError.Text = "Message"
                    lstError.Items.Add("Record updated successfully...")
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    'ImgError.ImageUrl = "../images/Pok.gif"
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
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
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            CreateLog("MachineEdit", "SaveMachineInfo-180", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")
            Return False
        End Try
    End Function

    Private Function ValidateMachineInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        'If txtEmail.Text.Equals("") Then
        '	shFlag = 1
        '	lstError.Items.Add("Email cannot be empty")
        'End If
        If txtMachineCode.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine Code cannot be empty...")
        End If
        If txtMachineIP.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine IP cannot be empty...")
        End If
        If txtMachineKey.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine Key cannot be empty...")
        End If
        If txtMachineLocation.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine Location cannot be empty...")
        End If
        If txtMachineName.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Machine Name cannot be empty...")
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
            Dim intRows As Integer
            If mstrqID = "-1" Then
                If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_VC30_MKey from T130011 where MM_VC30_MKey='" & txtMachineKey.Text & "'", intRows) = True Then
                    lstError.Items.Add("Invalid Machine Key...")
                    shFlag = 1
                End If
                If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_VC20_MName from T130011 where MM_VC20_MName='" & txtMachineName.Text & "'", intRows) = True Then
                    lstError.Items.Add("Invalid Machine Name...")
                    shFlag = 1
                End If
                If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_VC20_MIP from T130011 where MM_VC20_MIP='" & txtMachineIP.Text & "'", intRows) = True Then
                    lstError.Items.Add("Invalid Machine IP...")
                    shFlag = 1
                End If
                If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_IN4_MCode from T130011 where MM_IN4_MCode=" & txtMachineCode.Text, intRows) = True Then
                    lstError.Items.Add("Invalid Machine Code...")
                    shFlag = 1
                End If
            Else

                'read other columns values against mstrqID/mcode

                Dim sqrdr As System.Data.SqlClient.SqlDataReader
                Dim blnStatus As Boolean


                sqrdr = SQL.Search("MachineEdit", "ValidateMachineInfo", "select * from T130011 where MM_IN4_MCode=" & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                If blnStatus = True Then
                    While sqrdr.Read


                        Dim intRowsTemp As Integer

                        If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_VC30_MKey from T130011 where MM_VC30_MKey='" & txtMachineKey.Text.Trim & "' and MM_IN4_MCode<>" & mstrqID, intRowsTemp, "") = True Then
                            lstError.Items.Add("Invalid Machine Key...")
                            shFlag = 1
                        End If
                        If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_VC20_MName from T130011 where MM_VC20_MName='" & txtMachineName.Text.Trim & "' and MM_IN4_MCode<>" & mstrqID, intRowsTemp, "") = True Then
                            lstError.Items.Add("Invalid Machine Name...")
                            shFlag = 1
                        End If
                        If SQL.Search("MachineEdit", "ValidateMachineInfo", "select  MM_VC20_MIP from T130011 where MM_VC20_MIP='" & txtMachineIP.Text.Trim & "' and MM_IN4_MCode<>" & mstrqID, intRowsTemp, "") = True Then
                            lstError.Items.Add("Invalid Machine IP...")
                            shFlag = 1
                        End If

                    End While
                    sqrdr.Close()
                End If
            End If

            If shFlag = 1 Then
                'cpnlError.Visible = True
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                'cpnlError.Text = "Error Message"
                'ImgError.ImageUrl = "../images/warning.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
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
            CreateLog("MachineEdit", "ValidateMachineInfo-290", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")
            Return False
        End Try
    End Function

    Private Sub txtMachineName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMachineName.TextChanged

    End Sub
End Class
