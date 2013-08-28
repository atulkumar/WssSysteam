Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data


Partial Class MonitoringCenter_RPA
    Inherits System.Web.UI.Page

    Private mdvObjects As DataView
    Private Shared mintSeqID As Integer
    Private mstrStatus As String
    Private mintReqID As Integer


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtNotes.Attributes.Add("OnKeyPress", "return MaxLength('" & txtNotes.ClientID & "','50');")
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        cpnlError.Visible = False
        txtCSS(Me.Page)

        imgClose.Attributes.Add("onclick", "return SaveEdit('Close')")
        imgSave.Attributes.Add("onclick", "return SaveEdit('Save')")
        imgOk.Attributes.Add("onclick", "return SaveEdit('Ok')")

        Try
            If Not IsPostBack Then
                mintSeqID = Request.QueryString("ID")
            End If

            If mintSeqID <> -1 Then
                Dim blnStatus As Boolean
                Dim sqRDR As SqlClient.SqlDataReader

                sqRDR = SQL.Search("RPA", "Load", "select RQ_CH2_STATUS , RQ_NU9_REQUEST_ID from T130022 where RQ_NU9_SQID_PK=" & mintSeqID, SQL.CommandBehaviour.SingleRow, blnStatus)
                If blnStatus = True Then
                    While sqRDR.Read
                        mintReqID = Val(sqRDR("RQ_NU9_REQUEST_ID"))
                        mstrStatus = CType(sqRDR("RQ_CH2_STATUS"), String).Trim
                    End While
                    sqRDR.Close()
                End If
            Else
                mstrStatus = "Add"
            End If

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "t130032"

            DisableControls()

            'FillNonUDCDropDown(DDLCompany, "select CI_VC36_Name as ID,CI_VC36_Name Name,CI_VC8_Status Status  from  T010011 WHERE CI_VC8_Address_Book_Type ='COM'")
            If IsPostBack = False Then
                FillNonUDCDropDown(DDLCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status from T010011,t130033 a where  CI_NU8_Address_Number=a.MP_nu9_companyid_fk_pk and MP_NU9_ProcessID_FK_PK=10020017 and CI_VC8_Address_Book_Type ='COM'and ci_vc8_status='ENA'", False)
                DDLCompany.Items.Insert(0, New ListItem("Select", "0"))
            End If

            'DDLCompany.SelectedValue = Session("PropCompany")
            'Dim intCompID As Integer = WSSSearch.SearchCompName(Session("PropCompany")).ExtraValue
            'FillNonUDCDropDown(DDLTemplate, "select TL_NU9_ID_PK, TL_VC30_Template from T050011 where TL_VC8_Tmpl_Type in ('CAO','CNT') and TL_NU9_CustID_FK=" & intCompID)

            'If Not IsPostBack Then
            '    Dim dsEnv As New DataSet
            '    If SQL.Search("udc", "RPA", "Load", "select name from udc where udctype='PENV' and company=" & intCompID, dsEnv, "", "") = True Then
            '        cblEnv.DataSource = dsEnv
            '        cblEnv.DataTextField = dsEnv.Tables(0).Columns(0).ColumnName
            '        cblEnv.DataValueField = dsEnv.Tables(0).Columns(0).ColumnName
            '        cblEnv.DataBind()
            '    End If
            'Else
            'End If

            Dim strhiddenImage As String
            strhiddenImage = Request.Form("txthiddenImage")

            If strhiddenImage <> "" Then

                Select Case strhiddenImage
                    Case "Save"
                        If mstrStatus = "E" Then
                        Else
                            If mintSeqID = -1 Then
                                SaveRPARequest("AT")
                            Else
                                UpdateRPARequest("AT")
                            End If
                        End If
                    Case "Ok"
                        If mstrStatus = "E" Then
                            Response.Redirect("RPA_Search.aspx", False)
                        Else
                            If mintSeqID = -1 Then
                                If SaveRPARequest("AT") = True Then
                                    Response.Redirect("RPA_Search.aspx", False)
                                End If
                            Else
                                If UpdateRPARequest("AT") = True Then
                                    Response.Redirect("RPA_Search.aspx", False)
                                End If
                            End If
                        End If
                    Case "Close"
                        Response.Redirect("RPA_Search.aspx", False)
                    Case "Logout"
                        LogoutWSS()
                End Select

            End If

            If Request.QueryString("ID") = "-1" Then
                cpnlObject.Enabled = False
                cpnlObject.State = CustomControls.Web.PanelState.Collapsed
                cpnlObject.TitleCSS = "test2"
            Else
                Filldata()
                BindObjectGrid()
            End If
        Catch ex As Exception
            CreateLog("RPA", "Load-164", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Sub

    Private Function Filldata()
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim blnStatus As Boolean
            Dim strSQL As String
            'strSQL = "select * from T130032 where  EP_NU9_SEQ_ID=" & mintSeqID 
            strSQL = "select * from T130022 where  RQ_NU9_SQID_PK=" & mintSeqID
            Dim sqRDR As SqlClient.SqlDataReader
            sqRDR = SQL.Search("RPA", "FillData", strSQL, SQL.CommandBehaviour.CloseConnection, blnStatus)

            If blnStatus = True Then
                While sqRDR.Read

                    'DDLCompany.SelectedValue = IIf(IsDBNull(sqRDR("EP_VC50_Client")), "", sqRDR("EP_VC50_Client"))
                    'DDLTemplate.SelectedValue = IIf(IsDBNull(sqRDR("EP_VC50_Field4")), "", sqRDR("EP_VC50_Field4"))
                    'txtProject.Text = IIf(IsDBNull(sqRDR("EP_VC50_Field1")), "", sqRDR("EP_VC50_Field1"))
                    'txtNotes.Text = IIf(IsDBNull(sqRDR("EP_VC50_Field8")), "", sqRDR("EP_VC50_Field8"))
                    'cbWebGen.Checked = IIf(sqRDR("EP_VC50_Field7") = 1, True, False)

                    DDLCompany.SelectedValue = IIf(IsDBNull(sqRDR("RQ_NU9_CLIENTID_FK")), "", sqRDR("RQ_NU9_CLIENTID_FK"))
                    GetTemplate(IIf(IsDBNull(sqRDR("RQ_NU9_CLIENTID_FK")), "", sqRDR("RQ_NU9_CLIENTID_FK")))
                    DDLTemplate.SelectedValue = IIf(IsDBNull(sqRDR("RQ_VC150_CAT4")), "", sqRDR("RQ_VC150_CAT4"))
                    txtProject.Text = IIf(IsDBNull(sqRDR("RQ_VC150_CAT1")), "", sqRDR("RQ_VC150_CAT1"))
                    txtNotes.Text = IIf(IsDBNull(sqRDR("RQ_VC150_CAT8")), "", sqRDR("RQ_VC150_CAT8"))

                    cbWebGen.Checked = IIf(IsDBNull(sqRDR("RQ_VC150_CAT7")) = True, 0, 1)

                    GetEnvironments(IIf(IsDBNull(sqRDR("RQ_NU9_CLIENTID_FK")), 0, sqRDR("RQ_NU9_CLIENTID_FK")))

                    Dim strEnv As String = IIf(IsDBNull(sqRDR("RQ_VC150_CAT5")), "", sqRDR("RQ_VC150_CAT5"))
                    Dim arrstrenv() As String = strEnv.Split(",")

                    For inti As Integer = 0 To arrstrenv.Length - 1
                        For intj As Integer = 0 To cblEnv.Items.Count - 1
                            If cblEnv.Items(intj).Text = arrstrenv(inti).ToString Then
                                cblEnv.Items(intj).Selected = True
                            End If
                        Next
                    Next
                End While
                sqRDR.Close()
            End If

        Catch ex As Exception
            CreateLog("RPA", "Filldata-196", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    Private Function SaveRPARequest(ByVal strRequestType As String) As Boolean
        Try

            Dim shFlag As Short
            shFlag = 0
            lstError.Items.Clear()
            If txtProject.Text.Equals("") Then
                lstError.Items.Add("SubCategory Name cannot be blank...")
                shFlag = 1
            End If
            If DDLTemplate.SelectedValue.Equals("") Then
                lstError.Items.Add("Template cannot be blank...")
                shFlag = 1
            End If

            Dim strEnv As String
            For inti As Integer = 0 To cblEnv.Items.Count - 1
                If cblEnv.Items(inti).Selected = True Then
                    strEnv &= cblEnv.Items(inti).Text & ","
                End If
            Next
            If IsNothing(strEnv) Then
                lstError.Items.Add("Select at least one environment...")
                shFlag = 1
            Else
                strEnv = strEnv.Remove(strEnv.Length - 1, 1)
            End If

            If shFlag = 1 Then
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                Return False
            End If

            Dim intDomainName As Integer
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & DDLCompany.SelectedValue & " and MP_NU9_ProcessID_FK_PK='10020017'"
            'reader
            sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold domainname
                intDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
                'hold machineCode
                intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
                sqRDR.Close()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Domain is available for this process...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            Dim arrCol As New ArrayList
            Dim arrRow As New ArrayList

            'arrCol.Add("EP_NU9_Process_ID")
            'arrCol.Add("EP_VC50_Field1") ' This will hold the Project Name
            'arrCol.Add("EP_VC50_Field2") ' This will hold the date when the request was placed
            'arrCol.Add("EP_VC50_Field3") ' This will hold the operation type "AT" or "MT"
            'arrCol.Add("EP_VC50_Field4") ' This will hold the Template ID
            'arrCol.Add("EP_VC50_Field5") ' This will hold the Environments
            'arrCol.Add("EP_VC50_Field6") ' This will hold the UserID
            'arrCol.Add("EP_VC50_Field7") ' This will hold the Web Generation 
            'arrCol.Add("EP_VC50_Field8") ' This will hold the Notes
            'arrCol.Add("EP_VC50_Client")
            'arrCol.Add("EP_CH2_Status")
            'arrCol.Add("EP_NU9_SEQ_ID")
            'arrCol.Add("EP_VC50_RequestID")
            'arrCol.Add("EP_NU9_Machine_Code")

            arrCol.Add("RQ_NU9_PROCESSID")
            arrCol.Add("RQ_VC150_CAT1") ' This will hold the Project Name
            arrCol.Add("RQ_VC150_CAT2") ' This will hold the date when the request was placed
            arrCol.Add("RQ_VC150_CAT3") ' This will hold the operation type "AT" or "MT"
            arrCol.Add("RQ_VC150_CAT4") ' This will hold the Template ID
            arrCol.Add("RQ_VC150_CAT5") ' This will hold the Environments
            arrCol.Add("RQ_VC150_CAT6") ' This will hold the UserID
            arrCol.Add("RQ_VC150_CAT7") ' This will hold the Web Generation 
            arrCol.Add("RQ_VC150_CAT8") ' This will hold the Notes
            arrCol.Add("RQ_NU9_CLIENTID_FK")
            arrCol.Add("RQ_CH2_STATUS")
            arrCol.Add("RQ_NU9_SQID_PK")
            arrCol.Add("RQ_NU9_REQUEST_ID")
            arrCol.Add("RQ_NU9_MACHINE_CODE_FK")
            arrCol.Add("RQ_VC100_REQUEST_DATE")
            arrCol.Add("RQ_NU9_DOMAIN_FK")

            arrRow.Add("10020017")
            arrRow.Add(txtProject.Text.Trim)
            arrRow.Add(Now.ToShortDateString)
            arrRow.Add(strRequestType)
            arrRow.Add(DDLTemplate.SelectedValue.Trim)
            arrRow.Add(strEnv)
            arrRow.Add(Session("PropUserID"))
            arrRow.Add(IIf(cbWebGen.Checked, 1, 0))
            arrRow.Add(txtNotes.Text.Trim)
            arrRow.Add(DDLCompany.SelectedValue)
            arrRow.Add("P")
            Dim sqRDR1 As SqlClient.SqlDataReader
            Dim blnStatus1 As Boolean
            sqRDR1 = SQL.Search("", "", "select max(RQ_NU9_SQID_PK) from T130022;select max(convert(numeric(12),RQ_NU9_REQUEST_ID)) from T130022 where RQ_NU9_PROCESSID=10020017", SQL.CommandBehaviour.CloseConnection, blnStatus1, "")
            Dim intTempID As Integer = -1
            If blnStatus1 = True Then
                While sqRDR1.Read
                    If intTempID = -1 Then
                        intTempID = Val(IIf(IsDBNull(sqRDR1(0)), 0, sqRDR1(0))) + 1
                    End If
                    arrRow.Add(Val(IIf(IsDBNull(sqRDR1(0)), 0, sqRDR1(0))) + 1)
                    sqRDR1.NextResult()
                End While
            End If
            arrRow.Add(intMachineCode)
            arrRow.Add(Now)
            arrRow.Add(intDomainName)

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T130032"
            SQL.DBTracing = False
            If SQL.Save("T130022", "RPA", "SaveRPARequest", arrCol, arrRow) = True Then
                mintSeqID = intTempID
                lstError.Items.Clear()
                lstError.Items.Add("Request Saved Successfully...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Return True
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            CreateLog("RPA", "SaveRPARequest-288", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Function UpdateRPARequest(ByVal strRequestType As String) As Boolean
        Try
            Dim shFlag As Short
            shFlag = 0
            lstError.Items.Clear()
            If txtProject.Text.Equals("") Then
                lstError.Items.Add("SubCategory Name cannot be blank...")
                shFlag = 1
            End If
            If DDLTemplate.SelectedValue.Equals("") Then
                lstError.Items.Add("Template cannot be blank...")
                shFlag = 1
            End If

            Dim strEnv As String
            For inti As Integer = 0 To cblEnv.Items.Count - 1
                If cblEnv.Items(inti).Selected = True Then
                    strEnv &= cblEnv.Items(inti).Text & ","
                End If
            Next
            If IsNothing(strEnv) Then
                lstError.Items.Add("Select at least one environment...")
                shFlag = 1
            Else
                strEnv = strEnv.Remove(strEnv.Length - 1, 1)
            End If

            If shFlag = 1 Then
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Return False
            End If
            Dim arrCol As New ArrayList
            Dim arrRow As New ArrayList

            arrCol.Add("RQ_VC150_CAT1") 'SubCategory
            arrCol.Add("RQ_VC150_CAT2") ' This will hold the date when the request was placed
            arrCol.Add("RQ_VC150_CAT3") ' This will hold the operation type "AT" or "MT"
            arrCol.Add("RQ_VC150_CAT4") ' This will hold the Template ID
            arrCol.Add("RQ_VC150_CAT5") ' This will hold the Environments
            arrCol.Add("RQ_VC150_CAT6") ' This will hold the UserID
            arrCol.Add("RQ_VC150_CAT7") ' This will hold the Web Generation 
            arrCol.Add("RQ_VC150_CAT8") ' This will hold the Notes

            arrCol.Add("RQ_CH2_STATUS")


            arrRow.Add(txtProject.Text.Trim)
            arrRow.Add(Now.ToShortDateString)
            arrRow.Add(strRequestType)
            arrRow.Add(DDLTemplate.SelectedValue)
            arrRow.Add(strEnv)
            arrRow.Add(Session("PropUsedID"))
            arrRow.Add(IIf(cbWebGen.Checked, 1, 0))
            arrRow.Add(txtNotes.Text.Trim)

            arrRow.Add("P")
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T130032"
            SQL.DBTracing = False

            If SQL.Update("T130022", "RPA", "UpdateRPARequest", "select * from T130022 where RQ_NU9_SQID_PK=" & mintSeqID, arrCol, arrRow) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Request Updated Successfully...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Return True
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                Return False
            End If
        Catch ex As Exception
            CreateLog("RPA", "UpdateRPARequest-360", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    Private Function BindObjectGrid() As Boolean
        Try
            Dim strSQL As String
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'cpnlObject.State = CustomControls.Web.PanelState.Expanded
            'cpnlObject.Enabled = True
            '  SQL.DBTable = "T190011"

            strSQL = "SELECT RS_VC150_CAT3 as Project,RS_VC150_CAT4 Object,RS_VC150_CAT5 ObjectType ,'' as ObjectStatus,RS_VC150_CAT6 ,RS_VC150_CAT7,RS_VC150_CAT8,RS_VC150_CAT9 Comment from T130023 where RS_NU9_SQID_FK=" & mintSeqID & " and RS_nu9_companyid_fk='" & DDLCompany.SelectedValue & "'"
            Dim dsObjects As New DataSet
            mdvObjects = New DataView
            If SQL.Search("T130023", "RPA", "BindObjectGrid", strSQL, dsObjects, "", "") = True Then
                mdvObjects.Table = dsObjects.Tables(0)
                FillStatus()
                dgrObject.AutoGenerateColumns = False
                dgrObject.DataSource = mdvObjects.Table
                dgrObject.DataBind()

            End If
        Catch ex As Exception
            CreateLog("RPA", "BindObjectGrid-388", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    Private Function FillStatus()
        Try

            Dim strStatus As String
            mdvObjects.Table.Columns.Add("ImgStatus")
            For inti As Integer = 0 To mdvObjects.Table.Rows.Count - 1
                Select Case mdvObjects.Table.Rows(inti).Item("RS_VC150_CAT6") 'status
                    Case 1
                        strStatus = " Just Added, "
                    Case 2
                        strStatus = " Checked In, "
                    Case 3
                        strStatus = " Checked Out, "
                    Case 4
                        strStatus = " To Be Deleted, "
                    Case 5
                        strStatus = " Obsolete, "
                    Case 6
                        strStatus = " Has Been Deleted, "
                End Select
                Select Case mdvObjects.Table.Rows(inti).Item("RS_VC150_CAT7") 'heck status
                    Case 0
                        strStatus &= " Not Checked Out, "
                    Case 1
                        strStatus &= " Checked Out, "
                End Select
                Select Case mdvObjects.Table.Rows(inti).Item("RS_VC150_CAT8") 'fustring
                    Case 0
                        strStatus &= " No Token "
                    Case 1
                        strStatus &= " Has The Token "
                End Select
                mdvObjects.Table.Rows(inti).Item("ObjectStatus") = strStatus.Trim
                mdvObjects.Table.Rows(inti).Item("ImgStatus") = mdvObjects.Table.Rows(inti).Item("RS_VC150_CAT6") & mdvObjects.Table.Rows(inti).Item("RS_VC150_CAT7") & mdvObjects.Table.Rows(inti).Item("RS_VC150_CAT8")


            Next
            mdvObjects.Table.AcceptChanges()
        Catch ex As Exception
            CreateLog("RPA", "FillStatus-429", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Sub dgrObject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgrObject.ItemDataBound
        Try

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                e.Item.Attributes.Add("onclick", "KeyCheck(" & e.Item.ItemIndex + 1 & ")")
                e.Item.Attributes.Add("style", "cursor:hand")
                If mdvObjects.Table.Rows(e.Item.ItemIndex).Item("ImgStatus").trim = "201" Or mdvObjects.Table.Rows(e.Item.ItemIndex).Item("ImgStatus").trim = "0201" Then
                    CType(e.Item.FindControl("imgStatus"), System.Web.UI.WebControls.Image).ImageUrl = "../images/GreenAlert.gif"
                Else
                    CType(e.Item.FindControl("imgStatus"), System.Web.UI.WebControls.Image).ImageUrl = "../images/RedAlert.gif"
                End If
                CType(e.Item.FindControl("imgStatus"), System.Web.UI.WebControls.Image).ToolTip = mdvObjects.Table.Rows(e.Item.ItemIndex).Item("ObjectStatus")
                CType(e.Item.FindControl("chkSelect"), System.Web.UI.WebControls.CheckBox).ToolTip = mdvObjects.Table.Rows(e.Item.ItemIndex).Item("ObjectStatus")
                CType(e.Item.FindControl("txtComment"), System.Web.UI.WebControls.TextBox).ToolTip = mdvObjects.Table.Rows(e.Item.ItemIndex).Item("ObjectStatus")
                If Not IsNothing(Request.Form("cpnlObject:dgrObject:_ctl" & e.Item.ItemIndex + 2 & ":txtComment")) Then
                    Dim txt As TextBox
                    txt = e.Item.FindControl("txtComment")
                    CType(txt, TextBox).Text = Request.Form("cpnlObject:dgrObject:_ctl" & e.Item.ItemIndex + 2 & ":txtComment")
                Else
                    CType(e.Item.FindControl("txtComment"), System.Web.UI.WebControls.TextBox).Text = mdvObjects.Table.Rows(e.Item.ItemIndex).Item("Comment")
                End If


            End If

        Catch ex As Exception
            CreateLog("RPA", "dgrObject_ItemDataBound-443", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Sub

    Private Function DisableControls()
        Select Case mstrStatus.Trim.ToUpper
            Case "ADD"
                DDLCompany.Enabled = True
                cpnlObject.State = CustomControls.Web.PanelState.Collapsed
                cpnlObject.Enabled = False
                cpnlObject.TitleCSS = "test2"
            Case "C", "S"
                cblEnv.Enabled = False
                DDLCompany.Enabled = False
                DDLTemplate.Enabled = False
                txtNotes.ReadOnly = True
                txtProject.ReadOnly = True
                cbWebGen.Enabled = False
                'cpnlObject.State = CustomControls.Web.PanelState.Collapsed
                'cpnlObject.Enabled = False
                'cpnlObject.TitleCSS = "test2"
            Case "P"
                cblEnv.Enabled = True
                DDLCompany.Enabled = False
                DDLTemplate.Enabled = True
                txtNotes.ReadOnly = False
                txtProject.ReadOnly = False
                cbWebGen.Enabled = True
                btnRaiseRPA.Enabled = True
                cpnlObject.State = CustomControls.Web.PanelState.Collapsed
                cpnlObject.Enabled = False
            Case "ER"
                DDLCompany.Enabled = False
                txtProject.ReadOnly = True
        End Select

    End Function

    Private Sub DDLCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLCompany.SelectedIndexChanged
        'DDLCompany.SelectedValue = Session("PropCompany")
        'Dim intCompID As Integer = WSSSearch.SearchCompName(Session("PropCompany")).ExtraValue
        Dim intCompID As Integer = DDLCompany.SelectedValue
        'FillNonUDCDropDown(DDLTemplate, "select TL_NU9_ID_PK, TL_VC30_Template from T050011 where TL_VC8_Tmpl_Type in ('CAO','CNT') and TL_NU9_CustID_FK=" & intCompID)
        txtProject.Enabled = True
        GetTemplate(intCompID)
        GetEnvironments(intCompID)
    End Sub

    Private Sub GetEnvironments(ByVal CompanyID As Integer)
        Dim dsEnv As New DataSet
        If SQL.Search("udc", "RPA", "Load", "select name from udc where udctype='PENV' and company=" & CompanyID, dsEnv, "", "") = True Then
            cblEnv.DataSource = dsEnv
            cblEnv.DataTextField = dsEnv.Tables(0).Columns(0).ColumnName
            cblEnv.DataValueField = dsEnv.Tables(0).Columns(0).ColumnName
            cblEnv.DataBind()
        Else
            lstError.Items.Clear()
            lstError.Items.Add("No ENV exit for selected company plz add env for selected company...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            txtProject.Enabled = False

        End If
    End Sub

    Private Function GetTemplate(ByVal CompanyID As Integer)
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            Dim intCompID As Integer = DDLCompany.SelectedValue
            sqstr = "select TL_NU9_ID_PK, TL_VC30_Template from T050011 where TL_VC8_Tmpl_Type in ('CAO','CNT') and TL_NU9_CustID_FK=" & CompanyID
            If SQL.Search("T050011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlDomain dropdown fill acc to company
                DDLTemplate.DataSource = dsTemp.Tables(0)
                'Company Name
                DDLTemplate.DataTextField = "TL_VC30_Template"
                'CompanyId
                DDLTemplate.DataValueField = "TL_NU9_ID_PK"
                DDLTemplate.DataBind()

            Else
                'SQL.Search is False Msgpanel show no template exist for  selected company
                lstError.Items.Clear()
                lstError.Items.Add("No Template avilable for this company Plz add template for selected company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                txtProject.Enabled = False

            End If
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "GetDomain-206", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Function UpdateComments() As Boolean
        Dim Gridrow As DataGridItem

        For Each Gridrow In dgrObject.Items
            Dim strobject As String
            Dim strobjecttype As String
            Dim strproject As String
            Dim strcomment As String
            strobject = CType(Gridrow.FindControl("lblobject"), Label).Text.Trim
            strobjecttype = CType(Gridrow.FindControl("lblObjectType"), Label).Text.Trim
            strproject = CType(Gridrow.FindControl("lblProject"), Label).Text.Trim
            strcomment = CType(Gridrow.FindControl("txtComment"), TextBox).Text

            Dim sqlUpdate As String
            'get Connecting string From Webconfig
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'this function update t130023 against sqid
            sqlUpdate = "update T130023 set Rs_VC150_cat9='" & strcomment & "'  where  RS_VC150_CAT3='" & strproject & "' and   RS_VC150_CAT4='" & strobject & "' and RS_VC150_CAT5='" & strobjecttype & "'"
            If SQL.Update("RPAEdit", "UpdateRPA", sqlUpdate, SQL.Transaction.Serializable) = True Then

            End If

        Next

        Return True

    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        SaveRPARequest("AT")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        UpdateComments()
    End Sub
End Class
