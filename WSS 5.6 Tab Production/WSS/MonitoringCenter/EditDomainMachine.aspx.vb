Imports System.Data.SqlClient
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data


Partial Class MonitoringCenter_EditDomainMachine
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call txtCSS(Me.Page)   'From Module
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        If Session("PropCompanyType") = "SCM" Then
            cddlType.CDDLQuery = "select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=0 and UDCType=""MATY"" Order By Name"
        Else
            cddlType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""MATY""" & _
            " and UDC.Company=" & Session("PropCompanyID") & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""MATY""" & _
            " and UDC.Company=0 Order By Name"
        End If

        cddlType.CDDLUDC = True

        cddlType.CDDLMandatoryField = True

        If Not IsPostBack Then
            cddlType.CDDLFillDropDown()
            cddlType.Width = Unit.Pixel(129)

        Else
            cddlType.CDDLSetItem()
        End If

        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")

        If IsPostBack = False Then
            txtDomID.Text = Request.QueryString("DomID")
            txtMachName.Text = Request.QueryString("MachName")
            FillDetails()
        End If

        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Save"

                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If updateRecord() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Record updated successfully...")
                            Response.Write("<script>self.opener.callrefresh();</script>")
                            Response.Write("<script>window.close();</script>")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server is busy please try later...")
                        End If
                    Case "Ok"

                        'Security Block

                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If updateRecord() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Record updated successfully...")
                            Response.Write("<script>self.opener.callrefresh();</script>")
                            Response.Write("<script>window.close();</script>")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server is busy please try later...")
                        End If
                End Select

            Catch ex As Exception
                CreateLog("EditDomainMachine", "load-144", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

            End Try
        End If

    End Sub


#Region "fill details"

    Private Sub FillDetails()
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim dsSearch As DataSet
        Dim inti As Integer
        Dim sqrdRecords As SqlDataReader
        Dim blnStatus As Boolean
        Dim strSql As String

        strSql = "SELECT     TOP 1 *, T180011.AM_VC20_Code AS code FROM T170011,T170012 LEFT OUTER JOIN T180011 ON T170012.MM_IN4_AID_FK = T180011.AM_nu9_AID_pK WHERE MM_NU9_DID_FK=" & txtDomID.Text.Trim & " and MM_VC150_Machine_Name='" & txtMachName.Text.Trim & "' and DM_NU9_DID_PK=MM_NU9_DID_FK"

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        '        SQL.DBTable = "T080022"

        FillGrtid()

        Try
            sqrdRecords = SQL.Search("EditDomainMachine", "FillDetails", strSql, SQL.CommandBehaviour.CloseConnection, blnStatus)
            If blnStatus = True Then
                sqrdRecords.Read()

                txtUser.Text = IIf(IsDBNull(sqrdRecords.Item("mm_nu9_mid")), "", sqrdRecords.Item("mm_nu9_mid"))
                txtCat1.Text = IIf(IsDBNull(sqrdRecords.Item("MM_VC20_Cat1")), "", sqrdRecords.Item("MM_VC20_Cat1"))
                txtCat2.Text = IIf(IsDBNull(sqrdRecords.Item("MM_VC20_Cat2")), "", sqrdRecords.Item("MM_VC20_Cat2"))
                txtCat3.Text = IIf(IsDBNull(sqrdRecords.Item("MM_VC20_Cat3")), "", sqrdRecords.Item("MM_VC20_Cat3"))
                rblSts.SelectedValue = IIf(IsDBNull(sqrdRecords.Item("MM_VC4_Status")), "E", sqrdRecords.Item("MM_VC4_Status"))
                txtMachName.Text = IIf(IsDBNull(sqrdRecords.Item("MM_VC150_Machine_Name")), "", sqrdRecords.Item("MM_VC150_Machine_Name"))

                Dim strDecPwd As String

                strDecPwd = IONDecrypt(IIf(IsDBNull(sqrdRecords.Item("DM_VC150_Password")), "", sqrdRecords.Item("DM_VC150_Password")))
                cddlType.CDDLSetSelectedItem(IIf(IsDBNull(sqrdRecords.Item("MM_VC8_Machine_Type")), " ", sqrdRecords.Item("MM_VC8_Machine_Type")))

                Dim chksataus As String
                chksataus = IIf(IsDBNull(sqrdRecords.Item("MM_CH1_IsEnable")), "", sqrdRecords.Item("MM_CH1_IsEnable"))

                If chksataus = "" Then
                Else
                    rblSts.SelectedValue = chksataus
                End If

                sqrdRecords.Close()

                If cddlType.CDDLGetValue = "S" Then
                    cddlType.Enabled = False
                End If
            Else
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
            End If

        Catch ex As Exception
            CreateLog("Agreemnet Edit", "FillDeails-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Update"
    Function updateRecord() As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try

            getValues(arColumnName, arRowData)
            mstGetFunctionValue = updateDomMach(txtDomID.Text.Trim, txtMachName.Text.Trim, arColumnName, arRowData)

            If mstGetFunctionValue.ErrorCode = 0 Then
                'imgError.ImageUrl = "../images/Pok.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgOK)
                'cpnlError.Text = "Message"
                'cpnlError.Visible = True
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                'imgError.ImageUrl = "../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
                'cpnlError.Text = "Message"
                'cpnlError.TitleCSS = "test3"
                'cpnlError.Visible = True
                Return False
            End If
        Catch ex As Exception
            'imgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Text = "Message"
            'cpnlError.TitleCSS = "test3"
            'cpnlError.Visible = True
            CreateLog("EditDomainMachine", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    Function updateDomMach(ByVal DomId As Integer, ByVal MachName As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            '            SQL.DBTable = "T170012"
            SQL.DBTracing = False

            'SQL.Update("EditDomainMachine", "UpdateDomMach", "update T170011 set DM_VC150_UserID='" & txtUser.Text.Trim & "', DM_VC150_Password='" & IONEncrypt(txtPass.Text.Trim) & "' where DM_Nu9_DID_PK=" & txtDomID.Text.Trim, SQL.Transaction.Serializable)

            If SQL.Update("EditDomainMachine", "UpdateDomMach", "update T170012 set " & ColumnName(0) & "='" & RowData(0) & "'," & ColumnName(1) & "='" & RowData(1) & "'," & ColumnName(2) & "='" & RowData(2) & "'," & ColumnName(3) & "='" & RowData(3) & "'," & ColumnName(4) & "='" & RowData(4) & "'," & ColumnName(5) & "=" & RowData(5) & "," & ColumnName(6) & "='" & RowData(6) & "'  where MM_NU9_DID_FK=" & DomId & " and MM_VC150_Machine_Name='" & MachName & "'", SQL.Transaction.Serializable) = False Then

                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records updated successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("Monitoin", "UpdateMachine-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", True)
            Return stReturn
        End Try
    End Function

    Sub getValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)
        arColumnName.Clear()
        arRowData.Clear()

        arColumnName.Add("MM_CH1_IsEnable")
        arColumnName.Add("MM_VC20_Cat1")
        arColumnName.Add("MM_VC20_Cat2")
        arColumnName.Add("MM_VC20_Cat3")
        arColumnName.Add("MM_VC8_Machine_Type")
        arColumnName.Add("MM_IN4_Last_Modified_By")
        arColumnName.Add("MM_DT8_Last_Modified_Date")

        arRowData.Add(rblSts.SelectedValue)
        arRowData.Add(IIf(txtCat1.Text.Trim = "", DBNull.Value, txtCat1.Text.Trim))
        arRowData.Add(IIf(txtCat2.Text.Trim = "", DBNull.Value, txtCat2.Text.Trim))
        arRowData.Add(IIf(txtCat3.Text.Trim = "", DBNull.Value, txtCat3.Text.Trim))
        arRowData.Add(IIf(cddlType.CDDLGetValue = "", DBNull.Value, cddlType.CDDLGetValue))
        arRowData.Add(Session("propUserID"))
        arRowData.Add(Now.ToShortDateString)

    End Sub
#End Region

    Private Sub FillGrtid()
        Dim dsDefault As New DataSet
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            Dim strSelect As String = "select top 1 MM_VC100_Machine_IP as IP,MM_VC200_Proc_Desc as ProcessorDesc,MM_NU9_Proc_Speed as PSpeed,MM_VC200_OS_Name as OSName,MM_VC100_OS_Ver as OSVersion,MM_VC100_OS_SP as OSSerPack from T170012 where MM_NU9_DID_FK=" & txtDomID.Text.Trim & " and MM_VC150_Machine_Name='" & txtMachName.Text.Trim & "'"
            If SQL.Search("T080011", "EditDomainMachine", "fillgrtid", strSelect, dsDefault, "", "") = True Then
                grdDom.DataSource = dsDefault.Tables("T080011")
                grdDom.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
                grdDom.DataBind()
            End If
        Catch ex As Exception
            CreateLog("EditDom,DomainMachine", "FillDeails-844", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

        End Try

    End Sub

    'Private Sub GetMachineType()

    '    Try
    '        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
    '        SQL.DBTracing = False
    '        Dim dsTemp As New DataSet
    '        Dim sqstr As String
    '        'this function fetch  ENV against a company
    '        sqstr = "select description from udc where udctype='MATY' and company=" & Session("PropCAComp")
    '        If SQL.Search("udc ", "GetMachineType", "getDB", sqstr, dsTemp, "", "") = True Then
    '            'SQL.Search is true then ddlENV dropdown fill acc to selected company
    '            ddlMachineType.DataSource = dsTemp.Tables(0)
    '            'Env Name
    '            ddlMachineType.DataTextField = "name"
    '            ddlMachineType.DataBind()
    '        Else
    '            'SQL.Search is False ddlENV dropdown will be empty
    '            lstError.Items.Clear()
    '            lstError.Items.Add("No Machine Type  avilable for selected Company")
    '            ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgWarning)
    '        End If
    '    Catch ex As Exception
    '        CreateLog("editmachinetype", "GetMachineType-372", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
    '    End Try
    'End Sub

End Class
