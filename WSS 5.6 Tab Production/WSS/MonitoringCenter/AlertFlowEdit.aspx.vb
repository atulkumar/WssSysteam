Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Drawing


Partial Class MonitoringCenter_AlertFlowEdit
    Inherits System.Web.UI.Page
    Public mintLineID As Integer
    'Protected WithEvents lblTitleLabelAlertFlowEdit As System.Web.UI.WebControls.Label
    'Protected WithEvents DdlAlertType As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LbltempType As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlTemplateType As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LblTemplate As System.Web.UI.WebControls.Label
    'Protected WithEvents Ddltemp As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cpnlReport As CustomControls.Web.CollapsiblePanel
    Public mintAlertID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)


        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        '--User
        CDDLUser.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        CDDLUser.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id Order By Name"
        CDDLUser.CDDLUDC = False
        CDDLUser.Width = Unit.Point(150)
        '------------------------------------------

        ''--------Alert Action Type------------
        'CDDLAlertActionType.CDDLUDC = True
        'CDDLAlertActionType.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        'CDDLAlertActionType.CDDLQuery = "select name ID, Description, company  from UDC where UDCType=""ALTY"""
        'CDDLAlertActionType.Width = Unit.Point(150)
        ''''''''''''''''''''''''''''''''''''''''''''''''
        '--------Status------------
        CDDLStatus.CDDLUDC = True
        CDDLStatus.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        CDDLStatus.CDDLQuery = "select name ID, Description, company  from UDC where UDCType=""ALST"""
        CDDLStatus.Width = Unit.Point(150)
        '''''''''''''''''''''''''''''''''''''''''''''''
        '---------Template----------
        'FillTemplate()
        '''''''''''''''''''''''''''''

        If IsPostBack = False Then
            CDDLUser.CDDLFillDropDown(, False)
            'CDDLAlertActionType.CDDLFillDropDown(, True)
            CDDLStatus.CDDLFillDropDown(, True)
        Else
            CDDLUser.CDDLSetItem()
            'CDDLAlertActionType.CDDLSetItem()
            CDDLStatus.CDDLSetItem()




        End If
        '  btnClick.Attributes.Add("onclick", "SaveEdit('Save');")




        mintAlertID = Request.QueryString("AID")
        mintLineID = Request.QueryString("LineID")


        If txthiddenImage <> "" Then

            Select Case txthiddenImage
                Case "Ok"
                    If UpdateAlertFlow() = True Then
                        Response.Write("<script>window.close();</script>")
                    End If
                Case "Save"
                    If UpdateAlertFlow() = True Then

                    End If
                Case "Logout"
                    LogoutWSS()
            End Select

        End If

        If IsPostBack = False Then


            Dim blnStatus As Boolean
            Dim sqRDR As SqlClient.SqlDataReader
            Try
                sqRDR = SQL.Search("", "", "select * from T180012 where AF_NU9_AID_FK=" & mintAlertID & " and AF_NU9_LNID=" & mintLineID, SQL.CommandBehaviour.SingleResult, blnStatus)
                'Dim strsql As String = "select AF_NU9_ABNUM,AF_NU9_LNID,AF_VC8_Type,AF_VC50_COM,AF_VC12_Template_Type,TL_VC30_TEMPLATE,AF_VC8_Status from T180012, T050011 where  AF_NU9_Template_ID_FK *= TL_NU9_ID_PK and AF_NU9_AID_FK=" & mintAlertID & " and AF_NU9_LNID=" & mintLineID



                'sqRDR = SQL.Search("", "", strsql, SQL.CommandBehaviour.SingleResult, blnStatus)

                If blnStatus = True Then
                    While sqRDR.Read
                        CDDLUser.CDDLSetSelectedItem(IIf(IsDBNull(sqRDR("AF_NU9_ABNUM")), "", sqRDR("AF_NU9_ABNUM")), False)
                        CDDLStatus.CDDLSetSelectedItem(IIf(IsDBNull(sqRDR("AF_VC8_Status")), "", sqRDR("AF_VC8_Status")), True)
                        DdlAlertType.SelectedValue = IIf(IsDBNull(sqRDR("AF_VC8_Type")), "", sqRDR("AF_VC8_Type"))
                        If DdlAlertType.SelectedValue.Equals("EML") Then

                            txtCommMode.Text = IIf(IsDBNull(sqRDR("AF_VC50_COM")), "", sqRDR("AF_VC50_COM"))
                            ddlTemplateType.Enabled = False
                            Ddltemp.Enabled = False
                        Else

                            ddlTemplateType.SelectedValue = IIf(IsDBNull(sqRDR("AF_VC12_Template_Type")), "", sqRDR("AF_VC12_Template_Type"))
                            Dim strTemp As String = IIf(IsDBNull(sqRDR("AF_VC12_Template_Type")), "", sqRDR("AF_VC12_Template_Type"))
                            ddlTemplateType.SelectedValue = strTemp

                            FillTemplate(strTemp)
                            'Ddltemp.SelectedValue = sqRDR.Item("TL_VC30_Template")

                            Ddltemp.SelectedValue = IIf(IsDBNull(sqRDR("AF_NU9_Template_ID_FK")), "", sqRDR("AF_NU9_Template_ID_FK"))
                            'End If


                            txtCommMode.Enabled = False
                            txtCommMode.BackColor = Color.LightGray


                        End If

                    End While
                End If
                sqRDR.Close()
            Catch ex As Exception
                CreateLog("AlertFlow", "Load-154", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If

    End Sub


    Private Function FillTemplate(ByVal templateType As String)
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select TL_NU9_ID_PK , TL_VC30_Template  from T050011 where TL_VC8_Tmpl_Type ='" & templateType & "'"
            If SQL.Search("T050011", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                Ddltemp.DataSource = dsTemp.Tables(0)
                Ddltemp.DataTextField = "TL_VC30_Template"
                Ddltemp.DataValueField = "TL_NU9_ID_PK"
                Ddltemp.DataBind()
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("ReportName", "GetFillingRelease", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    Private Function UpdateAlertFlow() As Boolean

        If DdlAlertType.SelectedValue.Equals("EML") Then
            If ValidateAlertFlow() = False Then
                Exit Function
            End If
        ElseIf DdlAlertType.SelectedValue.Equals("WSS") Then

            If ValidateAlertWSSFlow() = False Then
                Exit Function
            End If
        End If

        Try
            Dim sqlUpdate As String

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If DdlAlertType.SelectedValue.Equals("EML") Then


                sqlUpdate = "update T180012 set AF_VC8_Type='" & DdlAlertType.SelectedValue & "' , AF_VC50_COM='" & txtCommMode.Text & "' , AF_NU9_ABNUM=" & CDDLUser.CDDLGetValue & " , AF_VC8_Status='" & CDDLStatus.CDDLGetValue & "' ,AF_VC12_Template_Type='',AF_NU9_Template_ID_FK=Null where  AF_NU9_AID_FK=" & mintAlertID & " and AF_NU9_LNID=" & mintLineID
            Else
                sqlUpdate = "update T180012 set AF_VC8_Type='" & DdlAlertType.SelectedValue & "' , AF_VC50_COM='' , AF_NU9_ABNUM=" & CDDLUser.CDDLGetValue & " , AF_VC8_Status='" & CDDLStatus.CDDLGetValue & "' ,AF_NU9_Template_ID_FK=" & Ddltemp.SelectedValue & ",AF_VC12_Template_Type='" & ddlTemplateType.SelectedValue & "' where  AF_NU9_AID_FK=" & mintAlertID & " and AF_NU9_LNID=" & mintLineID

            End If

            If SQL.Update("AlertFlowEdit", "UpdateAlertFlow", sqlUpdate, SQL.Transaction.Serializable) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Record Saved successfully...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Return True
            Else
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


    Private Function ValidateAlertFlow() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If DdlAlertType.SelectedValue.Equals("") Then
            lstError.Items.Add("Alert Action Type cannot be blank...")
            shFlag = 1
        End If
        If txtCommMode.Text.Equals("") Then
            lstError.Items.Add("Communication Mode cannot be blank...")
            shFlag = 1
        End If
        If CDDLUser.CDDLGetValue.Equals("") Then
            lstError.Items.Add("User cannot be blank...")
            shFlag = 1
        End If
        If CDDLStatus.CDDLGetValue.Equals("") Then
            lstError.Items.Add("Status cannot be blank...")
            shFlag = 1
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function

    Private Function ValidateAlertWSSFlow() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If DdlAlertType.SelectedValue.Equals("") Then
            lstError.Items.Add("Alert Action Type cannot be blank...")
            shFlag = 1
        End If

        If CDDLUser.CDDLGetValue.Equals("") Then
            lstError.Items.Add("User cannot be blank...")
            shFlag = 1
        End If
        If CDDLStatus.CDDLGetValue.Equals("") Then
            lstError.Items.Add("Status cannot be blank...")
            shFlag = 1
        End If
        If Ddltemp.SelectedValue.Equals("") Then
            lstError.Items.Add("Template cannot be blank...")
            shFlag = 1
        End If

        If ddlTemplateType.SelectedValue.Equals("") Then
            lstError.Items.Add("Template type  cannot be blank...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub ddlTemplateType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTemplateType.SelectedIndexChanged

        Ddltemp.Items.Clear()
        FillTemplate(ddlTemplateType.SelectedValue)

    End Sub

    Private Sub DdlAlertType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlAlertType.SelectedIndexChanged
        If DdlAlertType.SelectedValue.Equals("EML") Then
            txtCommMode.Enabled = True
            txtCommMode.BackColor = Color.White
            'ddlTemplateType.Items.Clear()
            'Ddltemp.Items.Clear()

            ddlTemplateType.Enabled = False
            Ddltemp.Enabled = False

        Else

            txtCommMode.BackColor = Color.LightGray
            ddlTemplateType.Enabled = True
            Ddltemp.Enabled = True
            txtCommMode.Enabled = False


        End If
    End Sub
    Private Function update() As Boolean
        Try
            Dim StrSql As String
            Dim SQID As Integer
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList


            StrSql = "update T180012 where AF_NU9_AID_FK=" & mintAlertID & " and AF_NU9_LNID=" & mintLineID

            'this function check validation before save that all Fields are fill or not if submit report is N


            arColName.Add("AF_VC8_Type") 'Alert TYPE
            arColName.Add("AF_NU9_ABNUM") 'USER
            arColName.Add("AF_VC8_Status") 'Status

            arRowData.Add(DdlAlertType.SelectedValue)
            arRowData.Add(CDDLUser.CDDLGetValue)
            arRowData.Add(CDDLStatus.CDDLGetValue)


            If DdlAlertType.SelectedValue.Equals("EML") Then
                If ValidateAlertFlow() = False Then
                    Exit Function
                End If

                arColName.Add("AF_VC50_COM")
                arColName.Add("AF_VC12_Template_Type")
                arColName.Add("AF_NU9_Template_ID_FK")

                arRowData.Add(txtCommMode.Text)
                arRowData.Add("")
                arRowData.Add("")

            End If

            If DdlAlertType.SelectedValue.Equals("WSS") Then

                If ValidateAlertWSSFlow() = False Then
                    Exit Function
                End If
                arColName.Add("AF_VC50_COM")
                arColName.Add("AF_VC12_Template_Type")
                arColName.Add("AF_NU9_Template_ID_FK")

                arRowData.Add("")
                arRowData.Add(ddlTemplateType.SelectedValue)
                arRowData.Add(Ddltemp.SelectedValue)


            End If

            If SQL.Update("t180012", "EditAlertFLOw", "Update", "select * from T180012 where AF_NU9_AID_FK=" & mintAlertID & " and AF_NU9_LNID=" & mintLineID, arColName, arRowData) = True Then

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("dataobjectentry", "SAVERecord", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing

        End Try
    End Function


End Class
