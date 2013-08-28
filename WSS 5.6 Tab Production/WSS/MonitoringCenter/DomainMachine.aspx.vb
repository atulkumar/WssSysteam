Imports ION.data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports System.Text
Imports System.Security.Cryptography
Imports System.Data
Imports System.Drawing



Partial Class MonitoringCenter_DomainMachine
    Inherits System.Web.UI.Page


    Private rowvalue As Integer
    Private insertedBy As String
    Private insertedOn As String
    Private systemBy As String

    Private Shared intCol As Integer
    Private Shared mTextBox() As TextBox
    Protected Shared mdvtable As DataView = New DataView
    Private Shared arColWidth As New ArrayList
    Private arColumnName As New ArrayList
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList

    Private mshFlag As Short

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtCSS(Me.Page)

        cpnlDom.Enabled = False
        cpnlDom.State = CustomControls.Web.PanelState.Collapsed
        cddlDomain.CDDLQuery = "select DM_NU9_DID_PK as ID,DM_VC150_DomainName Domain,DM_NU9_Company_ID_FK as Company from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")

        cddlDomain.CDDLUDC = False
        cddlDomain.CDDLPopUpURL = "../Search/Common/PopSearch1.aspx"
        cddlDomain.CDDLMandatoryField = True

        If Not IsPostBack Then
            cddlDomain.CDDLFillDropDown(, False)
            cddlDomain.Width = Unit.Point(120)
        Else
            If Not cddlDomain.CDDLGetCount = 0 Then
                cddlDomain.CDDLSetItem()
            End If
        End If

        If Not (txtDom.Text = "" And txtDomID.Text = "") Then
            cpnlDom.Enabled = True
            cpnlDom.State = CustomControls.Web.PanelState.Expanded
        End If

        If IsPostBack Then
            arrtextvalue.Clear()

            For i As Integer = 0 To arColumns.Count - 1
                arrtextvalue.Add(Request.Form("cpnlDom:" & arCol.Item(i)))
            Next
        End If

    End Sub

#Region " Grid1 "

#Region " Fill View "

    Private Function FillView() As Boolean

        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String

        If txtStatus.Text.Trim = "S" Then
            strselect = "select MM_VC150_Machine_Name as MachineName from T170012 where MM_NU9_DID_FK =" & txtDomID.Text.Trim
        Else
            strselect = "select TM_VC150_MachineName as MachineName from T170013 where TM_VC150_DomainName ='" & txtDom.Text.Trim & "' and TM_NU9_REQ_PK=" & txtReqNo.Text.Trim & ""
        End If

        If SQL.Search("T170013", "DomainMachine", "Fillview", strselect, dsFromView, "", "") Then
            Try
                mdvtable.Table = dsFromView.Tables("T170013")

                GrdAddSerach.DataSource = mdvtable
                GrdAddSerach.DataBind()
                FormatGrid()
                GetColumns()
                CreateTextBox()
                Return True

            Catch ex As Exception
                CreateLog("Domain Machine", "FillView-143", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else

            Dim ds As New DataSet
            ds.Tables.Add("Dummy")

            ds.Tables("Dummy").Columns.Add("MachName")

            Dim dtRow As DataRow

            dtRow = ds.Tables("Dummy").NewRow
            dtRow.Item(0) = ""

            mdvtable.Table = ds.Tables("Dummy")

            GrdAddSerach.DataSource = mdvtable.Table
            GrdAddSerach.DataBind()

            FormatGrid()
        End If
    End Function

#End Region

#Region "Create textboxes at run time based on datagrid culumns count"

    'create textbox on runtime based on datagrid columns
    Private Function CreateTextBox()
        arColumns.Clear()

        Dim intFirstColumn As Integer
        Dim strHTML As String
        Dim _textbox As TextBox
        Dim ii As WebControls.Unit
        Dim i As String
        Dim intii As Integer

        arCol.Clear()
        arCol.Add("MachName")

        'fill the columns count into the array from mdvtable view
        intCol = mdvtable.Table.Columns.Count

        ReDim mTextBox(intCol)

        Try
            For intii = 0 To intCol - 1
                _textbox = New TextBox

                Dim col1 As Unit
                Dim col1cng As String
                Dim strcolid As String
                col1 = Unit.Parse(arColWidth.Item(intii))
                col1cng = col1.Value + 1
                col1cng = col1cng & "pt"

                If arrtextvalue.Count <> mdvtable.Table.Columns.Count Then
                    _textbox.Text = ""
                Else
                    _textbox.Text = arrtextvalue.Item(intii)
                End If

                '_textbox.Text = ""
                strcolid = arCol.Item(intii)

                If strcolid = "ScreenName" Then
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & arCol.Item(intii) & " runat=""server""  Width=" & col1cng & " BackColor=""WhiteSmoke"" height=""20px""  readonly=""true"" BorderWidth=""1px"" BorderColor=""Gray""></asp:TextBox>"))
                Else
                    Panel1.Controls.Add(Page.ParseControl("<asp:TextBox id=" & strcolid & " runat=""server"" Width=" & col1cng & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                End If

                _textbox.ID = mdvtable.Table.Columns(intii).ColumnName

                mTextBox(intii) = _textbox

                mshFlag = 1
                arColumns.Add(_textbox.ID)
            Next
        Catch ex As Exception
            CreateLog("DomainMachine", "CreateTextBox-223", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

#End Region

#Region "Format datagrid columns size according to database"

    'change the datagrid columns size at run time 
    Private Sub FormatGrid()
        Dim intI As Integer

        arColWidth.Clear()

        arColWidth.Add(20)
        arColWidth.Add(120)

        Try
            GrdAddSerach.Columns.Clear()
            GrdAddSerach.AutoGenerateColumns = False

            For intI = 0 To arColWidth.Count - 1
                Dim Bound_Column As New BoundColumn
                Dim strWidth As String = arColWidth.Item(intI) & "pt"
                Bound_Column.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(strWidth)
                Bound_Column.ItemStyle.Wrap = True

                GrdAddSerach.Columns.Add(Bound_Column)
            Next
        Catch ex As Exception
            CreateLog("DomainMachine", "FormatGrid-254", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

#End Region

#Region "get columns from database"

    Private Sub GetColumns()

        arColWidth.Clear()
        arColumnName.Clear()
        arColWidth.Add(120)
        arColumnName.Add("MachName")

    End Sub

#End Region

#Region "check the grid width from database"

    Private Sub chkgridwidth()

        arColWidth.Clear()
        arColumnName.Clear()
        arColWidth.Add(120)
        arColumnName.Add("MachName")

    End Sub

#End Region

#Region "Serach Grid Button Click"

    Private Sub BtnGrdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGrdSearch.Click

        Dim strRowFilterString As String
        Dim strSearch As String
        Dim intQ As Integer = arrtextvalue.Count
        rowvalue = 0

        Try
            For intI As Integer = 0 To arColumns.Count - 1
                If Not arrtextvalue.Item(intI) = "" Then
                    strSearch = arrtextvalue.Item(intI)

                    If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date
                            If IsDate(strSearch) = False Then
                                strSearch = "12/12/1825"
                            End If
                        End If

                        If (mdvtable.Table.Columns(intI).DataType.FullName = "System.Decimal") = True Then
                            strSearch = strSearch.Replace("*", "")
                            If IsNumeric(strSearch) = False Then
                                strSearch = "-101"
                            End If
                        End If

                        ' mdvTable.Table.Columns(intI).DataType.FullName = System.DateTime
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = arrtextvalue.Item(intI)
                        strSearch = GetSearchString(strSearch)

                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            If (strRowFilterString Is Nothing) Then
                FillView()
                Exit Sub
            End If

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            GrdAddSerach.DataSource = mdvtable
            GrdAddSerach.DataBind()
            FormatGrid()
            GetColumns()
            CreateTextBox()
        Catch ex As Exception
            CreateLog("DomainMachine", "BtnGrdSearch.Click-343", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "BtnGrdSearch")
        End Try
    End Sub

#End Region

#Region "Search Grid Item Data Bound Event"

    Private Sub GrdAddSerach_ItemDataBound1(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID, strEnb As String
        Dim cnt As Integer
        Dim i As Integer

        Dim rowFlag As Boolean
        rowFlag = True

        Try
            For cnt = 0 To e.Item.Cells.Count - 1
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = GrdAddSerach.DataKeys(e.Item.ItemIndex)

                    If MachExists(strID, strEnb) Then

                        e.Item.Cells(cnt).Attributes.Add("style", "cursor:hand")
                        e.Item.Cells(cnt).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "','" & rowvalue & "')")

                        e.Item.Cells(cnt).Attributes.Add("ondblclick", "javascript:KeyCheck55('" & strID & "', '" & txtDomID.Text.Trim & "')")

                        If strEnb = "E" Then
                            CType(e.Item.FindControl("chkReq"), CheckBox).Checked = True
                            For i = 0 To e.Item.Cells.Count - 1
                                e.Item.Cells(i).BackColor = Color.LightGray
                                e.Item.Cells(i).ForeColor = Color.DarkBlue
                            Next
                        Else
                            CType(e.Item.FindControl("chkReq"), CheckBox).Checked = False
                            For i = 0 To e.Item.Cells.Count - 1
                                e.Item.Cells(i).BackColor = Color.LightGray
                                e.Item.Cells(i).ForeColor = Color.DarkBlue
                            Next
                        End If

                    End If

                End If
                rowFlag = False
            Next
            rowvalue += 1
        Catch ex As Exception
            CreateLog("DomainMachine", "GrdAddSearch.ItemDataBound-396", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSerach")
        End Try
    End Sub

#End Region

#End Region

    Private Sub imgSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click

        Try
            Dim arCols As New ArrayList
            Dim arRows As New ArrayList
            Dim mintSubAddressNo As Integer

            mintSubAddressNo = SQL.Search("AB_Additional", "UpdateorSave-405", "select isnull(max(RQ_NU9_SQID_PK),0) from T130022")
            mintSubAddressNo += 1

            arCols.Add("RQ_NU9_PROCESSID")
            arCols.Add("RQ_VC150_CAT1")
            arCols.Add("RQ_CH2_STATUS")
            arCols.Add("RQ_VC100_REQUEST_DATE")
            arCols.Add("RQ_NU9_SQID_PK")
            arCols.Add("RQ_NU9_CLIENTID_FK")

            arRows.Add("10020015")
            arRows.Add("TD")
            arRows.Add("P")
            arRows.Add(Now.ToShortDateString)
            arRows.Add(mintSubAddressNo)
            arRows.Add(Session("PropCAComp"))

            Try
                mstGetFunctionValue = SaveDomReq(arCols, arRows)

                If mstGetFunctionValue.FunctionExecuted = True Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                Else
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                End If
                arCols.Clear()
                arRows.Clear()

                cddlDomain.CDDLFillDropDown(, False)

                If cddlDomain.CDDLGetCount = 0 Then
                    cpnlDom2.Enabled = False
                    cpnlDom2.State = CustomControls.Web.PanelState.Collapsed
                Else
                    cpnlDom2.Enabled = True
                End If

            Catch ex As Exception
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                CreateLog("DomainMachine", "Imgsearch.click-453", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        Catch ex As Exception
            CreateLog("DomainMachine", "Imgsearch.click-456", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Sub imgAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click

        Dim arCols As New ArrayList
        Dim arRows As New ArrayList
        Dim mintSubAddressNo As Integer

        Dim sqlRd As SqlDataReader
        Dim stsFlg As Boolean
        Dim flg As Boolean = False

        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Dim strSQL As String
        'this query is used to fetch MachineIP against selected domain  and machine
        strSQL = "select DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
        'reader
        sqRDR = SQL.Search("GETMachine ", "Check domain", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
        If blnStatus = True Then
            If cddlDomain.CDDLGetValue = "" Then
                lstError.Items.Clear()
                flg = True
                lstError.Items.Clear()
                lstError.Items.Add("Select Domain Name")
            End If
            sqRDR.Close()
        Else
            lstError.Items.Clear()
            lstError.Items.Add(" Please Get the domain First...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        If flg Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            grdDom.Visible = False
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        Try
            arCols.Add("RQ_NU9_PROCESSID") '10020015
            arCols.Add("RQ_VC150_CAT1") 'DM
            arCols.Add("RQ_VC150_CAT2") 'DomName
            arCols.Add("RQ_VC150_CAT3") 'DomID
            arCols.Add("RQ_CH2_STATUS") 'Status
            arCols.Add("RQ_VC100_REQUEST_DATE") 'Status
            arCols.Add("RQ_NU9_SQID_PK")
            arCols.Add("RQ_NU9_CLIENTID_FK")

            mintSubAddressNo = SQL.Search("AB_Additional", "UpdateorSave-405", "select isnull(max(RQ_NU9_SQID_PK),0) from T130022")
            mintSubAddressNo += 1

            arRows.Add("10020015")
            arRows.Add("DM")
            arRows.Add(cddlDomain.CDDLGetValueName)
            arRows.Add(cddlDomain.CDDLGetValue)
            arRows.Add("P")
            arRows.Add(Now.ToShortDateString)
            arRows.Add(mintSubAddressNo)
            arRows.Add(Session("PropCAComp"))

            mstGetFunctionValue = SaveDomReq(arCols, arRows)

            If mstGetFunctionValue.FunctionExecuted = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Request for getting machines in a Domain is placed in the queue...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            End If
            arCols.Clear()
            arRows.Clear()

            Dim dsFromView As New DataSet
            Dim blnView As Boolean
            Dim strselect As String

            strselect = "select RQ_VC150_CAT3 as DomID,RQ_VC150_CAT2 as DomName,RQ_CH2_Status Status,RQ_NU9_SQID_PK as ReqNo from T130022 where RQ_NU9_PROCESSID ='10020015' and RQ_VC150_CAT1='DM' and RQ_CH2_Status<>'C'"

            If SQL.Search("T030201", "Imgadd", "click", strselect, dsFromView, "", "") Then
                grdDom.DataSource = dsFromView.Tables("T030201")
                grdDom.DataBind()
            End If

        Catch ex As Exception
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            CreateLog("DomainMachine", "imgAdd.click-548", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub

    Private Function SaveDomReq(ByVal ColumnName As ArrayList, ByVal RowValue As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False

        Try
            If SQL.Save("T130022", "AB_Additional", "SaveDomReq-650", ColumnName, RowValue) = True Then
                stReturn.ErrorMessage = "Request for getting Domain placed in the queue successfully"
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy plaese try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy plaese try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("DomainMachine", "SaveDomReq-574", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Private Function SaveMachReq(ByVal ColumnName As ArrayList, ByVal RowValue As ArrayList) As ReturnValue
        Dim stReturn As ReturnValue

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        'SQL.DBTable = "T130032"
        SQL.DBTracing = False

        Try
            If SQL.Save("T130032", "Domain_Machine", "SaveMachReq-641", ColumnName, RowValue) = True Then
                stReturn.ErrorMessage = "Request for getting Machines placed in the queue successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            Else
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("Domain_Machine", "SaveMachReq-602", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

    Sub myItems_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Try
            Dim itemType As ListItemType = e.Item.ItemType

            If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
                Return
            Else
                Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)

                e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
            End If
        Catch ex As Exception
            CreateLog("Domain_Machine", "myItems_ItemDataBound-619", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Sub grdDom_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdDom.ItemCommand
        Try
            If e.CommandName = "select" Then

                Dim i As Integer = grdDom.Columns.Count
                For i = 0 To grdDom.Items.Count - 1
                    If i Mod 2 = 0 Then
                        grdDom.Items(i).BackColor = Color.WhiteSmoke
                    Else
                        grdDom.Items(i).BackColor = Color.White
                    End If
                Next
                e.Item.BackColor = Color.LightGray
                txtDom.Text = CType(e.Item.FindControl("lblDomName"), Label).Text
                txtReqNo.Text = CType(e.Item.FindControl("lblReqNo"), Label).Text
                'txtMach.Text = CType(e.Item.FindControl("lblMach"), Label).Text
                txtDomID.Text = CType(e.Item.FindControl("lblDomID"), Label).Text ' getDomID(txtDom.Text.Trim)
                txtStatus.Text = CType(e.Item.FindControl("lblStatus"), Label).Text

                FillView()

                cpnlDom.Enabled = True
                cpnlDom.State = CustomControls.Web.PanelState.Expanded
            End If
        Catch ex As Exception
            CreateLog("Domain_Machine", "grdDOM_ItemCommand-648", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        Try

            Dim arMach As New ArrayList
            Dim arCols As New ArrayList
            Dim arRow As New ArrayList
            Dim DomID, Mid As Integer
            Dim flg As Boolean

            If txtUserID.Text.Trim = "" Then
                lstError.Items.Clear()
                flg = True
                lstError.Items.Add("Enter valid User ID for domain...")
            End If

            If txtPwd.Text.Trim = "" Then
                lstError.Items.Clear()
                flg = True
                lstError.Items.Add("Enter valid User password for domain...")
            End If

            If flg Then
                lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                FillView()
                Exit Sub
            End If

            ReadGrid(arMach)

            Dim Multi As SQL.AddMultipleRows
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBTracing = False

            DomID = txtDomID.Text.Trim

            arCols.Add("MM_NU9_DID_FK")
            arCols.Add("MM_VC150_Machine_Name")
            arCols.Add("MM_NU9_MID")
            arCols.Add("MM_CH1_IsEnable")
            arCols.Add("MM_VC30_MKey")

            Mid = SQL.Search("Domain_Machine", "click-405", "select isnull(max(MM_NU9_MID),99) from T170012 where MM_NU9_DID_FK=" & txtDomID.Text.Trim)
            Mid = Mid + 1

            Dim intMKey As Integer
            intMKey = SQL.Search("Domain_Machine", "click-405", "select isnull(max(MM_VC30_MKey),1001) from T170012 where MM_NU9_DID_FK=" & txtDomID.Text.Trim)

            For cnt As Integer = 0 To arMach.Count - 1
                arRow.Add(DomID)
                arRow.Add(arMach.Item(cnt))
                arRow.Add(Mid)
                arRow.Add("E")
                arRow.Add(intMKey)
                Multi.Add("T170012", arCols, arRow)
                intMKey = intMKey + 1
                Mid = Mid + 1
            Next
            Multi.Save()
            Multi.Dispose()
            'save the domian info in T130032
            lstError.Items.Clear()
            lstError.Items.Add("Machine Information saved successfully...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            If Not (txtStatus.Text.Trim = "C") Then
                SQL.Update("imgsave", "click", "update T130022 set RQ_CH2_STATUS='C' where RQ_NU9_SQID_PK=" & txtReqNo.Text.Trim, SQL.Transaction.Serializable)
                SQL.Delete("imgsave", "click", "delete from T170013 where Tm_nu9_req_pk=" & txtReqNo.Text.Trim, SQL.Transaction.Serializable)
            End If

            ' MI request
            SaveDomainUserPwd()
            cpnlDom.Enabled = False
            cpnlDom.State = CustomControls.Web.PanelState.Collapsed
        Catch ex As Exception
            CreateLog("Domain_Machine", "Imgsaveclick.Click-728", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Private Function SaveDomainUserPwd() As Boolean
        Dim arMach As New ArrayList
        Dim arRow As New ArrayList
        Dim arCols As New ArrayList
        Dim DomID, Mid As Integer

        Try
            readGrid2(arMach)

            Dim Multi As SQL.AddMultipleRows
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            '            SQL.DBTable = "T130032"
            SQL.DBTracing = False

            DomID = txtDomID.Text.Trim
            Dim strDomainName As String
            Dim intMachineCode As Integer

            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Dim strSQL As String
            'this query is used to fetch MachineIP against selected domain  and machine
            strSQL = "select MP_NU9_DomainID_FK_PK, MP_NU9_MachineID_FK_PK from T130033 where MP_NU9_CompanyID_FK_PK=" & Session("PropCAComp") & " and MP_NU9_ProcessID_FK_PK='10020015'"
            'reader
            sqRDR = SQL.Search("BGDailyMonitor", "SaveBGRequest", strSQL, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                'if blnstatus true reader read data from database
                sqRDR.Read()
                'hold domainname
                strDomainName = sqRDR("MP_NU9_DomainID_FK_PK")
                'hold machineCode
                intMachineCode = sqRDR("MP_NU9_MachineID_FK_PK")
                sqRDR.Close()
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Machines are saved but process is not registered. Please register the process and save request again.")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            arCols.Add("RQ_NU9_SQID_PK")
            arCols.Add("RQ_NU9_PROCESSID")
            arCols.Add("RQ_VC150_CAT1")
            arCols.Add("RQ_VC150_CAT2")
            arCols.Add("RQ_VC150_CAT3")
            arCols.Add("RQ_VC150_CAT4")
            arCols.Add("RQ_VC150_CAT5")
            arCols.Add("RQ_CH2_Status")
            arCols.Add("RQ_NU9_DOMAIN_FK")
            arCols.Add("RQ_NU9_MACHINE_CODE_FK")
            arCols.Add("RQ_NU9_CLIENTID_FK")
            arCols.Add("RQ_VC100_REQUEST_DATE")

            Mid = SQL.Search("DomainMachine", "SaveDomainUserPwd-405", "select isnull(max(RQ_NU9_SQID_PK),0) from T130022")
            Mid = Mid + 1

            For cnt As Integer = 0 To arMach.Count - 1
                arRow.Add(Mid)
                arRow.Add("10020015")
                arRow.Add("MI")
                arRow.Add(txtDom.Text.Trim)
                arRow.Add(arMach.Item(cnt))
                arRow.Add(Encrypt(txtUserID.Text.Trim))
                arRow.Add(Encrypt(txtPwd.Text.Trim))
                arRow.Add("P")
                arRow.Add(strDomainName)
                arRow.Add(intMachineCode)
                arRow.Add(Session("PropCAComp"))
                arRow.Add(Now.ToShortDateString)

                Multi.Add("T130022", arCols, arRow)
                Mid = Mid + 1
            Next

            Multi.Save()
            Multi.Dispose()
            lstError.Items.Clear()
            lstError.Items.Add("Request for Machine Information saved successfully...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
        Catch ex As Exception
            CreateLog("Domain_Machine", "SaveDomainUserPwd-813", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Sub ReadGrid(ByRef dtMach As ArrayList)
        Try
            Dim gridrow As DataGridItem
            Dim drow As DataRow
            Dim strEnb As String

            For Each gridrow In GrdAddSerach.Items

                If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                    If Not MachExists(CType(gridrow.FindControl("lblMachName"), Label).Text, strEnb) Then
                        dtMach.Add(CType(gridrow.FindControl("lblMachName"), Label).Text)
                    End If
                End If
            Next
        Catch ex As Exception
            CreateLog("DomainMachine", "ReadGrid-832", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Sub ReadGrid2(ByRef dtMach As ArrayList)
        Try
            Dim gridrow As DataGridItem
            Dim drow As DataRow
            For Each gridrow In GrdAddSerach.Items
                If CType(gridrow.FindControl("chkReq"), CheckBox).Checked Then
                    dtMach.Add(CType(gridrow.FindControl("lblMachName"), Label).Text)
                End If
            Next
        Catch ex As Exception
            CreateLog("DomainMachine", "ReadGrid2-847", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Function MachExists(ByVal MachName As String, ByRef strEnb As String) As Boolean
        Try
            Dim domID As Integer

            domID = txtDomID.Text.Trim ' getDomID(txtDom.Text.Trim)
            strEnb = SQL.Search("DomainMachine", "MachExists", "select MM_CH1_IsEnable as enb from T170012 where MM_NU9_DID_FK=" & domID & " and MM_VC150_Machine_Name='" & MachName & "'")

            If IsNothing(strEnb) = False Then
                If strEnb.Trim.Equals("") = False Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Domain_Machine", "MachExists-869", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Function GetDomID(ByVal domName As String) As Integer

        Dim mintSubAddressNo As Integer
        Try
            mintSubAddressNo = SQL.Search("DomainMachine", "getDomID-405", "select top 1 DM_NU9_DID_pk from T170011 where DM_VC150_DomainName='" & domName & "'")

        Catch ex As Exception
            CreateLog("Domain_Machine", "GetDomID-880", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
        Return mintSubAddressNo

    End Function

    Private Sub imgShow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgShow.Click
        Dim dsFromView As New DataSet
        Dim blnView As Boolean
        Dim strselect As String = ""
        If cddlDomain.CDDLGetValue = "" Then
            lstError.Items.Clear()
            lstError.Items.Add("Please select Domain First...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            grdDom.Visible = False
            Exit Sub
        Else
            grdDom.Visible = True
            lstError.Items.Clear()
            cpnlError.Visible = False
            strselect = "select RQ_VC150_CAT3 as DomID,RQ_VC150_CAT2 as DomName,RQ_CH2_Status Status,RQ_NU9_SQID_PK as ReqNo from T130022 where RQ_NU9_PROCESSID ='10020015' and RQ_VC150_CAT1='DM' and RQ_NU9_CLIENTID_FK='" & Session("PropCAComp") & " ' and RQ_VC150_CAT3=" & cddlDomain.CDDLGetValue
        End If

        If SQL.Search("T030201", "imgshow", "click", strselect, dsFromView, "", "") Then
            Try
                grdDom.DataSource = dsFromView.Tables("T030201")
                grdDom.DataBind()
            Catch ex As Exception
                CreateLog("Domain_Machine", "imgShow.click-908", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If
        Dim ds As New DataSet
        ds.Tables.Add("Dummy")

        ds.Tables("Dummy").Columns.Add("MachName")

        Dim dtRow As DataRow

        dtRow = ds.Tables("Dummy").NewRow
        dtRow.Item(0) = ""

        mdvtable.Table = ds.Tables("Dummy")

        GrdAddSerach.DataSource = mdvtable.Table
        GrdAddSerach.DataBind()

        FormatGrid()

        cpnlDom.Enabled = False
        cpnlDom.State = CustomControls.Web.PanelState.Collapsed
    End Sub

    Private Sub imgReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReset.Click
        FillView()
    End Sub

    Private Function PopulateDropDownLists(ByVal sqlQuery As String, ByRef ddData As DropDownList, ByVal isOptional As Char)
        Dim dtDDData As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtDDData = New DataTable
            sqlCon = New SqlConnection(SQL.DBConnection)
            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtDDData)
            ddData.DataSource = dtDDData
            ddData.DataTextField = dtDDData.Columns(1).ColumnName
            ddData.DataValueField = dtDDData.Columns(0).ColumnName
            If (isOptional = "Y") Then
                Dim row As DataRow
                row = dtDDData.NewRow
                row(0) = "0"
                row(1) = "Optional"
                dtDDData.Rows.InsertAt(row, 0)
                ddData.SelectedValue = "0"
            End If
            ddData.DataBind()
        Catch ex As Exception
            CreateLog("DomainMachine", "PopulateDropDownLists-959", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        Finally
            sqlCon.Close()
        End Try

    End Function

    Private Function Encrypt(ByVal Data As String) As String
        Dim shaM As SHA1Managed = New SHA1Managed
        System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        '// Getting the bytes of the encrypted data.//
        Dim bytEncrypt() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        '// Converting the byte into string.//
        Dim strEncrypt As String = System.Convert.ToBase64String(bytEncrypt)
        Encrypt = strEncrypt
    End Function

    Private Function Decrypt(ByVal Data As String) As String
        Dim bytData() As Byte = System.Convert.FromBase64String(Data)
        Dim strData As String = ASCIIEncoding.ASCII.GetString(bytData)
        Decrypt = strData
    End Function

    Private Sub ImgbtnClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgbtnClose.Click
        Response.Redirect("configuration.aspx")
    End Sub

End Class
