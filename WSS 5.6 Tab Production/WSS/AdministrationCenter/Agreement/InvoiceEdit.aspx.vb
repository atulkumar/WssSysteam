'*********************************************************************************************************
' Page                 : - Invoice Edit
' Purpose              : - 
' Tables used          : - T080011, T080031, T080033
' Date					   Author						Modification Date				Description
' 17/05/06			       jaswinder		            ------------------			    Created
'
' Notes: 
' Code:
'*********************************************************************************************************

Imports ION.data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports System.Data


Partial Class AdministrationCenter_Agreement_InvoiceEdit
    Inherits System.Web.UI.Page
    Private Shared mTextBox() As TextBox
    Protected Shared mdvtable As DataView = New DataView
    Dim rowvalue As Integer
    Shared mintID As Integer
    Private Shared Flag As Boolean

    Dim insertedBy As String
    Dim insertedOn As String
    Dim systemBy As String

    Dim shF As Short
    Dim flg As Short
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared intCol As Integer
    Private Shared arColWidth As New ArrayList
    Dim mshFlag As Short

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call txtCSS(Me.Page)   'From Module
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgUp.Attributes.Add("Onclick", "return SaveEdit('Up');")
            imgDown.Attributes.Add("Onclick", "return SaveEdit('Down');")
        End If
        If IsPostBack = False Then
            txtCallNo.Text = Request.QueryString("CallNo")
            txtComp.Text = Request.QueryString("CompID")
            txtRowNo.Text = Request.QueryString("RowNo")
            txtInvNo.Text = Request.QueryString("InvNo")
            txtAgNo.Text = Request.QueryString("AgNo")
            txtCType.Text = Request.QueryString("CType")
            txtTaskNo.Text = Request.QueryString("TaskNo")
            txtTaskType.Text = Request.QueryString("TaskType")
            FillView()
            fillView2()

            FormatGrid()
            GetColumns()

        End If

        Dim txthiddenImage = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Save"

                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If updateAgreement() = True Then
                            FillView()
                            fillView2()

                            FormatGrid()
                            GetColumns()

                            lstError.Items.Clear()
                            lstError.Items.Add("Agreement Line updated successfully...")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Error occured while updating the record...")
                        End If
                    Case "Ok"

                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If updateAgreement() = True Then
                            Response.Write("<script>self.opener.Form1.submit();</script>")
                            Response.Write("<script>window.close();</script>")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("server is busy please try later...")
                        End If

                    Case "Up"

                        If moveUp() = True Then
                            FillView()
                            fillView2()

                            FormatGrid()
                            GetColumns()

                            lstError.Items.Clear()
                            lstError.Items.Add("Agreement Line updated successfully...")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("server is busy please try later...")
                        End If

                    Case "Down"

                        If moveDown() = True Then
                            FillView()
                            fillView2()

                            FormatGrid()
                            GetColumns()

                            lstError.Items.Clear()
                            lstError.Items.Add("Agreement Line updated successfully...")
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("server is busy please try later...")
                        End If

                End Select

            Catch ex As Exception
                CreateLog("InvoiceEdit", "Load-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        ' Security(Block)

        Dim intID As Int32
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = 724
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If

        ' End of Security Block
    End Sub

#Region "Update Section"

    Function moveUp() As Boolean
        Dim strAct As String

        'get a string that lists all the checked actions to be moved to UP
        strAct = readNonInvoicedGrid()

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            'SQL.DBTable = "T060022"
            SQL.DBTracing = False

            'update all those actions to "type=External"
            If SQL.Update("invoiceEdit", "updateInvoiceLine-197", "update T040031 set AM_VC8_ActionType='External' where AM_NU9_Call_Number='" & txtCallNo.Text.Trim & "' and AM_NU9_Task_Number='" & txtTaskNo.Text.Trim & "' and AM_NU9_Comp_ID_FK=" & txtComp.Text.Trim & " and AM_NU9_Action_Number in(" & strAct & ")", SQL.Transaction.Serializable) Then
                Return True

            End If


            Return False

        Catch ex As Exception
            CreateLog("InvoiceEdit", "moveUp-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    Function moveDown() As Boolean
        Dim strAct As String

        'get a string that lists all the checked actions to be moved to DOWN
        strAct = readInvoicedGrid()

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            'SQL.DBTable = "T060022"
            SQL.DBTracing = False

            'update all those actions to "type=Internal"
            If SQL.Update("invoiceEdit", "updateInvoiceLine-197", "update T040031 set AM_VC8_ActionType='Internal',AM_FL8_BillHrs=0 where AM_NU9_Call_Number='" & txtCallNo.Text.Trim & "' and AM_NU9_Task_Number='" & txtTaskNo.Text.Trim & "' and AM_NU9_Comp_ID_FK=" & txtComp.Text.Trim & " and AM_NU9_Action_Number in(" & strAct & ")", SQL.Transaction.Serializable) Then

                Return True

            End If

            Return False

        Catch ex As Exception
            CreateLog("InvoiceEdit", "moveDown-259", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    'read external actions grid for all the checked actions to be moved DOWN
    Function readInvoicedGrid() As String

        Dim gridrow As DataGridItem
        Dim strAct As String

        For Each gridrow In GrdAddSerach.Items
            If CType(gridrow.FindControl("chkAct1"), CheckBox).Checked Then
                strAct &= CType(gridrow.FindControl("lblActNo"), Label).Text.Trim & ","
            End If
        Next
        strAct = strAct.Remove(strAct.Length - 1, 1) 'create a string as 1,4,5 etc. of action number
        Return strAct

    End Function

    'read internal actions grid for all the checked actions to be moved UP
    Function readNonInvoicedGrid() As String

        Dim gridrow As DataGridItem
        Dim strAct As String

        For Each gridrow In GrdAddSerach2.Items
            If CType(gridrow.FindControl("chkAct2"), CheckBox).Checked Then
                strAct &= CType(gridrow.FindControl("lblActNo2"), Label).Text.Trim & ","
            End If
        Next
        strAct = strAct.Remove(strAct.Length - 1, 1) 'create a string as 1,4,5 etc. of action number
        Return strAct

    End Function

    ' to get updated billable hrs. and amt. for the call and task numbet according to the agr. number

    Function updateAgreement()
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            ' to update invoice details
            mstGetFunctionValue = UpdateInvoiceLine(txtInvNo.Text.Trim, txtCallNo.Text.Trim, txtComp.Text.Trim)

            If mstGetFunctionValue.ErrorCode = 0 Then
                ' imgError.ImageUrl = "../../images/Pok.gif"
                ' MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'cpnlError.Text = "Message"
                'cpnlError.Visible = True
                'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgOK)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Return True
            ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                'imgError.ImageUrl = "../../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                'cpnlError.Text = "Message"
                'cpnlError.TitleCSS = "test3"
                'cpnlError.Visible = True
                'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

                Return False
            End If
        Catch ex As Exception
            'imgError.ImageUrl = "../../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            lstError.Items.Add("server is busy please try later...")
            'cpnlError.Text = "Message"
            'cpnlError.TitleCSS = "test3"
            'cpnlError.Visible = True
            'ShowMsgPenel(cpnlError, lstError, imgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("CraeteUser", "UpdateUser-413", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Function UpdateInvoiceLine(ByVal InvNo As Integer, ByVal CallNo As Integer, ByVal comp As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Dim dtSkill As New DataTable
        Dim dsFixCur As New DataSet
        Dim strUpdate As String

        dtSkill = readGrid()

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            'SQL.DBTable = "T060022"
            SQL.DBTracing = False

            'save the hrs. and skill combination in invoice details

            For cnt As Integer = 0 To dtSkill.Rows.Count - 1

                If SQL.Update("invoiceEdit", "updateInvoiceLine-197", "update T080034 set PR_NU9_Hours=" & dtSkill.Rows(cnt).Item("UsedHours") & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " and PR_NU9_Call_No=" & txtCallNo.Text.Trim & " and PR_NU9_Skill_Level='" & dtSkill.Rows(cnt).Item("SkillLevel") & "' and PR_NU9_Task_No=" & txtTaskNo.Text.Trim, SQL.Transaction.Serializable) Then

                    'get specific agreement line and type of agreement (i.e. fixed or hourly) and proceed if type=-1 then price=0 i.e. no specific agr. line found

                    Dim Price, Type As String

                    getPriceType(txtCType.Text.Trim, txtTaskType.Text.Trim, Price, Type, txtAgNo.Text.Trim, dtSkill.Rows(cnt).Item("SkillLevel"))


                    If Not Type = "-1" Then

                        If Type = "H" Then 'For Hourly Price type


                            strUpdate = "update T080034 set PR_CH1_Fix_Amount='N',PR_NU9_Amount=PR_NU9_Hours*" & Price & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " and PR_NU9_Call_No=" & txtCallNo.Text.Trim & " and PR_NU9_Task_No=" & txtTaskNo.Text.Trim & " and PR_NU9_Skill_Level='" & dtSkill.Rows(cnt).Item("SkillLevel") & "'"

                        ElseIf Type = "F" Then 'For Fixed Price type

                            strUpdate = "update T080034 set PR_CH1_Fix_Amount='Y',PR_NU9_Amount=" & Price & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " and PR_NU9_Call_No=" & txtCallNo.Text.Trim & " and PR_NU9_Task_No=" & txtTaskNo.Text.Trim & " and PR_NU9_Skill_Level='" & dtSkill.Rows(cnt).Item("SkillLevel") & "'"
                        End If
                    Else
                        strUpdate = "update T080034 set PR_CH1_Fix_Amount='Y',PR_NU9_Amount=" & Price & " where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " and PR_NU9_Call_No=" & txtCallNo.Text.Trim & " and PR_NU9_Task_No=" & txtTaskNo.Text.Trim & " and PR_NU9_Skill_Level='" & dtSkill.Rows(cnt).Item("SkillLevel") & "'"

                    End If

                    If SQL.Update("InvoicinDetails", "Save", strUpdate, SQL.Transaction.Serializable) Then
                    End If

                Else

                    stReturn.ErrorMessage = "Server is busy please try later... "
                    stReturn.FunctionExecuted = False
                    stReturn.ErrorCode = 1

                End If

            Next

            'Set total amount of the invoice

            strUpdate = "update T080031 set IM_NU9_Invoice_Amount=(select sum(PR_NU9_Amount) from T080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " group by PR_NU9_Invoice_No_FK),IM_NU9_Invoice_Balance=(select sum(PR_NU9_Amount) from T080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " group by PR_NU9_Invoice_No_FK) where IM_NU9_Invoice_ID_PK=" & txtInvNo.Text.Trim & " and IM_NU9_Company_ID_PK=" & txtComp.Text.Trim

            SQL.Update("InvoicinDetails", "Save", strUpdate, SQL.Transaction.Serializable)

            Dim dsCallPrice As New DataSet
            ' SQL.DBTable = "CallPrice"

            'get all the call types and total billable hrs. against them to generate an annexure

            If SQL.Search("CallPrice", "InvoiceEdit", "UpdateInvoiceLine", "select PR_VC8_Call_Type CallType,sum(PR_NU9_Hours) TotalHours,sum(PR_NU9_Amount) TotalAmount from T080034 where PR_NU9_Invoice_No_FK=" & txtInvNo.Text.Trim & " and PR_NU9_Customer_ID_FK=" & txtComp.Text.Trim & " group by PR_VC8_Call_Type", dsCallPrice, "sachin", "Prashar") Then

                If savePriceOnCall(dsCallPrice.Tables("CallPrice")) Then

                End If

            End If

            Return stReturn

        Catch ex As Exception
            stReturn.ErrorMessage = "server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("InvoiceEdit", "UpdateInvoiceLine-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Sub getPriceType(ByVal callType As String, ByVal taskType As String, ByRef Price As String, ByRef Type As String, ByVal agNo As String, ByVal skLevel As String)
        Dim slcQry As String
        Dim dsPriceType As New DataSet
        Dim intLineNo As Integer

        If agNo = "" Then
            agNo = "-1"
        End If

        'query is used to get the most specific agr. line number against calltype/tasktype/skilllevel for an existing agr. number associated with a perticular call number

        slcQry = "select isnull(isnull(isnull(isnull((select top 1 AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & txtComp.Text.Trim & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='" & taskType & "' and AL_VC8_Skill_Level='" & skLevel & "'),(select top 1  AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & txtComp.Text.Trim & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='" & taskType & "' and AL_VC8_Skill_Level='')),(select top 1 AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & txtComp.Text.Trim & " and AL_VC8_Call_Type='" & callType & "' and  AL_VC8_Skill_Level='" & skLevel & "' and AL_VC8_Task_Type='')),(select top 1 AL_NU9_Line_No_PK from T080022 where  AL_NU9_Agr_No=" & agNo & " and AL_VC8_Customer=" & txtComp.Text.Trim & " and AL_VC8_Call_Type='" & callType & "' and AL_VC8_Task_Type='' and  AL_VC8_Skill_Level='' )),0)"

        intLineNo = SQL.Search("", "", slcQry)

        If intLineNo = 0 Then 'if no agr. line found
            Price = "0"
            Type = "-1"
        Else
            slcQry = "select AL_NU9_Price as price,AL_VC1_Fix_Hour as type from T080022 where  AL_NU9_Line_No_PK=" & intLineNo

            ' SQL.DBTable = "T030201"
            If SQL.Search("T030201", "InvoiceEdit", "getPriceType", slcQry, dsPriceType, "sachin", "Prashar") Then
                Price = dsPriceType.Tables("T030201").Rows(0).Item("price")
                Type = dsPriceType.Tables("T030201").Rows(0).Item("type")
            End If
        End If

    End Sub


    Function savePriceOnCall(ByRef dtCallPrice As DataTable) As Boolean

        Dim cnt As Integer
        Dim arrColumns As New ArrayList
        Dim arRow As New ArrayList

        Try

            arrColumns.Clear()
            arRow.Clear()

            Dim Multi As SQL.AddMultipleRows
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            '  SQL.DBTable = "T080035"
            SQL.DBTracing = False

            If SQL.Delete("InvoiceEdit", "svePriceOnCall", "delete from T080035 where CP_NU9_Inv_No=" & txtInvNo.Text.Trim & " and CP_NU9_Customer_ID=" & txtComp.Text.Trim, SQL.Transaction.Serializable) Then

                arrColumns.Add("CP_NU9_Inv_No")
                arrColumns.Add("CP_NU9_Customer_ID")

                arrColumns.Add("CP_VC8_Call_Type")
                arrColumns.Add("CP_NU9_Billable_Hrs")
                arrColumns.Add("CP_NU9_Total_Amt")
                arrColumns.Add("CP_NU9_Discounted_Amt")

                'save the annexure details

                For cnt = 0 To dtCallPrice.Rows.Count - 1

                    arRow.Add(txtInvNo.Text.Trim)
                    arRow.Add(txtComp.Text.Trim)

                    arRow.Add(dtCallPrice.Rows(cnt).Item("CallType"))
                    arRow.Add(dtCallPrice.Rows(cnt).Item("TotalHours"))
                    arRow.Add(dtCallPrice.Rows(cnt).Item("TotalAmount"))
                    arRow.Add(dtCallPrice.Rows(cnt).Item("TotalAmount"))

                    Multi.Add("T080035", arrColumns, arRow)

                Next

                Multi.Save()
                Multi.Dispose()
                Return True
            Else
                Return False
            End If


        Catch ex As Exception
            CreateLog("InvoiceEdit", "SavePriceOnCall-107", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Function readGrid() As DataTable

        Dim dtSkill As New DataTable

        Dim slcQry As String

        Dim gridrow As DataGridItem
        'Dim drow As DataRow

        For Each gridrow In GrdAddSerach.Items

            'update the actions to set their bill hrs.=gridTextBoxBillHrs.text
            SQL.Update("InvoiceEdit", "readGrid", "update T040031 set AM_FL8_BillHrs=" & IIf(CType(gridrow.FindControl("txtBillHrs"), TextBox).Text = "", 0, CType(gridrow.FindControl("txtBillHrs"), TextBox).Text) & " where AM_NU9_Call_Number=" & txtCallNo.Text.Trim & " and AM_NU9_Task_Number=" & txtTaskNo.Text.Trim & " and AM_NU9_Action_Number=" & CType(gridrow.FindControl("lblActNo"), Label).Text & " and AM_NU9_Comp_ID_FK=" & txtComp.Text.Trim, SQL.Transaction.Serializable)

        Next

        ' get sum of new billable hrs. for the call no. task no
        slcQry = "select sum(AM_FL8_BillHrs) as UsedHours,CI_VC8_Level as SkillLevel from T040031,T010011 where AM_VC8_Supp_Owner=CI_NU8_Address_Number and AM_NU9_Call_Number=" & txtCallNo.Text.Trim & " and AM_NU9_Task_Number=" & txtTaskNo.Text.Trim & " and AM_NU9_Comp_ID_FK=" & txtComp.Text.Trim & " and AM_CH1_IsInvoiced='Y' and AM_NU9_Invoice_No=" & txtInvNo.Text.Trim & " group by CI_VC8_Level"

        Dim dsFromView As New DataSet
        'SQL.DBTable = "T030201"

        If SQL.Search("T030201", "InvoiceEdit", "readGrid", slcQry, dsFromView, "sachin", "Prashar") Then
            Return dsFromView.Tables("T030201")
        End If

    End Function

#End Region

#Region "Searchable Call Grid"

#Region "fill View"

    Private Sub FillView()

        Dim dsFromView As New DataSet
        'Dim blnView As Boolean
        Dim strselect As String

        strselect = "select AM_NU9_Call_Number as CallNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Action_Number as ActNo,AM_VC_2000_Description as ActDesc,level.CI_VC8_Level as SkillLevel,AM_FL8_Used_Hr ActHrs,owner.CI_VC36_Name as ActOwner,AM_FL8_BillHrs BillHrs from T040031,T010011 level,T010011 owner where level.CI_NU8_Address_Number=AM_VC8_Supp_Owner and owner.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Task_Number=" & txtTaskNo.Text.Trim & " and AM_NU9_Comp_ID_FK=" & txtComp.Text.Trim & " and AM_NU9_Call_Number=" & txtCallNo.Text.Trim & " and AM_CH1_IsInvoiced='Y' and AM_NU9_Invoice_No=" & txtInvNo.Text.Trim & " and (AM_VC8_ActionType='External' or AM_VC8_ActionType is null)"

        'SQL.DBTable = "T030201"

        Try

            If SQL.Search("T030201", "InvoiceEdit", "Fillview-559", strselect, dsFromView, "sachin", "Prashar") Then

                mdvtable.Table = dsFromView.Tables("T030201")

                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()

            Else

                Dim ds As New DataSet
                ds.Tables.Add("Dummy")

                ds.Tables("Dummy").Columns.Add("CallNo")
                ds.Tables("Dummy").Columns.Add("TaskNo")
                ds.Tables("Dummy").Columns.Add("ActNo")
                ds.Tables("Dummy").Columns.Add("ActDesc")
                ds.Tables("Dummy").Columns.Add("ActOwner")
                ds.Tables("Dummy").Columns.Add("SkillLevel")
                ds.Tables("Dummy").Columns.Add("ActHrs")
                ds.Tables("Dummy").Columns.Add("BillHrs")

                Dim dtRow As DataRow

                dtRow = ds.Tables("Dummy").NewRow
                dtRow.Item(0) = ""

                mdvtable.Table = ds.Tables("Dummy")

                GrdAddSerach.DataSource = mdvtable.Table
                GrdAddSerach.DataBind()

            End If

        Catch ex As Exception
            CreateLog("InvoiceEdit", "Fill View-593", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub


    Sub fillView2()

        Dim dsFromView As New DataSet
        'Dim blnView As Boolean
        Dim strselect As String

        strselect = "select AM_NU9_Call_Number as CallNo,AM_NU9_Task_Number as TaskNo,AM_NU9_Action_Number as ActNo,AM_VC_2000_Description as ActDesc,level.CI_VC8_Level as SkillLevel,AM_FL8_Used_Hr ActHrs,owner.CI_VC36_Name as ActOwner,AM_FL8_BillHrs BillHrs from T040031,T010011 level,T010011 owner where level.CI_NU8_Address_Number=AM_VC8_Supp_Owner and owner.CI_NU8_Address_Number=AM_VC8_Supp_Owner and AM_NU9_Task_Number=" & txtTaskNo.Text.Trim & " and AM_NU9_Comp_ID_FK=" & txtComp.Text.Trim & " and AM_NU9_Call_Number=" & txtCallNo.Text.Trim & " and AM_CH1_IsInvoiced='Y' and AM_NU9_Invoice_No=" & txtInvNo.Text.Trim & " and AM_VC8_ActionType='Internal'"

        ' SQL.DBTable = "T030201"

        Try

            If SQL.Search("T030201", "InvoiceEdit", "fillView2", strselect, dsFromView, "sachin", "Prashar") Then

                mdvtable.Table = dsFromView.Tables("T030201")

                GrdAddSerach2.DataSource = mdvtable
                GrdAddSerach2.DataBind()

            Else

                Dim ds As New DataSet
                ds.Tables.Add("Dummy")

                ds.Tables("Dummy").Columns.Add("CallNo")
                ds.Tables("Dummy").Columns.Add("TaskNo")
                ds.Tables("Dummy").Columns.Add("ActNo")
                ds.Tables("Dummy").Columns.Add("ActDesc")
                ds.Tables("Dummy").Columns.Add("ActOwner")
                ds.Tables("Dummy").Columns.Add("SkillLevel")
                ds.Tables("Dummy").Columns.Add("ActHrs")
                ds.Tables("Dummy").Columns.Add("BillHrs")

                Dim dtRow As DataRow

                dtRow = ds.Tables("Dummy").NewRow
                dtRow.Item(0) = ""

                mdvtable.Table = ds.Tables("Dummy")

                GrdAddSerach2.DataSource = mdvtable.Table
                GrdAddSerach2.DataBind()

            End If

        Catch ex As Exception
            CreateLog("InvoiceEdit", "FillView2", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(35)
        arColWidth.Add(35)
        arColWidth.Add(35)
        arColWidth.Add(120)
        arColWidth.Add(70)
        arColWidth.Add(50)
        arColWidth.Add(40)
        arColWidth.Add(40)

        Try
            GrdAddSerach.Columns.Clear()
            GrdAddSerach.AutoGenerateColumns = False

            GrdAddSerach2.Columns.Clear()
            GrdAddSerach2.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                GrdAddSerach.Columns.Add(Bound_Column)
                GrdAddSerach2.Columns.Add(Bound_Column)
            Next

        Catch ex As Exception
            CreateLog("InvoiceEdit", "FormatGrid-643", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(35)
        arColWidth.Add(35)
        arColWidth.Add(120)
        arColWidth.Add(70)
        arColWidth.Add(50)
        arColWidth.Add(40)
        arColWidth.Add(40)

        arColumnName.Add("CallNo")
        arColumnName.Add("TaskNo")
        arColumnName.Add("ActNo")
        arColumnName.Add("ActDesc")
        arColumnName.Add("ActOwner")
        arColumnName.Add("SkillLevel")
        arColumnName.Add("ActHrs")
        arColumnName.Add("BillHrs")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()

        arColWidth.Add(35)
        arColWidth.Add(35)
        arColWidth.Add(35)
        arColWidth.Add(120)
        arColWidth.Add(70)
        arColWidth.Add(50)
        arColWidth.Add(40)
        arColWidth.Add(40)

        arColumnName.Add("CallNo")
        arColumnName.Add("TaskNo")
        arColumnName.Add("ActNo")
        arColumnName.Add("ActDesc")
        arColumnName.Add("ActOwner")
        arColumnName.Add("SkillLevel")
        arColumnName.Add("ActHrs")
        arColumnName.Add("BillHrs")

    End Sub

#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        Dim strTempName As String

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)
                    strTempName = e.Item.Cells(1).Text
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                End If
            Next
            rowvalue += 1

            txtCnt.Text = mdvtable.Table.Rows.Count

        Catch ex As Exception
            CreateLog("InvoiceEdit", "GrdAddSearch_ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

#End Region

    Private Sub GrdAddSerach2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach2.ItemDataBound


        Try
            For cnt As Integer = 0 To e.Item.Cells.Count - 1
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    CType(e.Item.FindControl("txtBillHrs2"), TextBox).Text = "0"
                End If
            Next

        Catch ex As Exception
            CreateLog("InvoiceEdit", "ItemDataBound-807", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try

    End Sub
End Class
