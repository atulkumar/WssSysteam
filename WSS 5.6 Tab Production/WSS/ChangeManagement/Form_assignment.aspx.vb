'**********************************************************************************************************
' Page                   : - Form_assignment
' Purpose                : - Purpose of this screen is to show all forms created so that User can select                                  forms to associate it with task selected from dropdown.There are dropdowns to                                select company ,call & task.   
' Tables used            : - T040021, T050031, T110022
' Date		    		Author						Modification Date					Description
' 22/03/06				Amandeep					----------------	        		Created
'
''*********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class ChangeManagement_Form_assignment
    Inherits System.Web.UI.Page

    Private Shared dtItemDetail As DataTable

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Toolbar1 As Microsoft.Web.UI.WebControls.Toolbar

    Protected WithEvents Toolbar2 As Microsoft.Web.UI.WebControls.Toolbar
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents LblErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button

    'Protected WithEvents imgbtnSearch As System.Web.UI.WebControls.ImageButton
    ' Protected WithEvents BtnGrdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents Button5 As System.Web.UI.WebControls.Button
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents GrdAddSerach As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    ' Protected WithEvents 'cpnlError As CustomControls.Web.CollapsiblePanel
    ' Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    ' Protected WithEvents ImgError As System.Web.UI.WebControls.Image
    'Protected WithEvents dgFormAssign As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents Collapsiblepanel1 As CustomControls.Web.CollapsiblePanel
    ' Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
    ' Protected WithEvents imgSave As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgSearch As System.Web.UI.WebControls.ImageButton

    'Protected WithEvents cddlcall As New CustomDDL
    'Protected WithEvents cddltask As New CustomDDL
    'Protected WithEvents cddlcomp As New CustomDDL
    'Protected WithEvents lblTitleLabelFrmAssign As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "global level declaration"


    ' Holding text boxes created above UDC Type Grid
    Dim mtxtUDCTypeQuery As TextBox()

    Private Shared arColWidth As New ArrayList
    Dim flage As Integer
    Dim mdvtable As New DataView
    Dim marTextbox() As TextBox
    Private Shared mTextBox() As TextBox
    Dim mintColumns As Integer
    Dim mshFlag As Short
    Dim Expanded2 As New PlaceHolder
    Dim ii As WebControls.Unit
    Dim rowvalue As Integer
    Dim shF As Short
    Dim flg As Short
    Public mintPageSize As Integer
    Dim arColumnName As New ArrayList
    Dim mblnValue As Boolean
    Dim flgview As Short
    Private Shared arColumns As ArrayList = New ArrayList
    Private Shared arCol As ArrayList = New ArrayList
    Private Shared arCol2 As ArrayList = New ArrayList
    Private Shared arrtextvalue As ArrayList = New ArrayList
    Private Shared arOriginalColumnName As New ArrayList
    Private Shared arSetColumnName As New ArrayList
    Private Shared intCol As Integer

#End Region

#Region "Page Load Code"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        ' Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")

        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1


        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        ' imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")
        'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgSearch.Attributes.Add("Onclick", "return SaveEdit('Search');")


        '--Customer

        cddlComp.CDDLQuery = "SELECT CI_NU8_Address_Number as ID,CI_VC36_Name as Name,CI_VC16_Alias  as AliasName FROM t010011 where CI_VC8_Address_Book_Type='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")  Order By Name"


        cddlComp.CDDLMandatoryField = True
        cddlComp.CDDLUDC = False
        cddlComp.CDDLAutopostback = True

        If Not IsPostBack Then
            txtCSS(Me.Page)
            cddlComp.CDDLFillDropDown(10, False)
            cddlComp.Width = Unit.Pixel(120)

            cddlComp.CDDLSetSelectedItem(Session("propCompanyId"), False, Session("propCompany"))



            cddlCall.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""CALL""" & _
            " and UDC.Company=" & cddlComp.CDDLGetValue & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""CALL""" & _
            " and UDC.Company=0 Order By Name"


            cddlCall.CDDLUDC = True

            cddlCall.CDDLMandatoryField = True


            cddlCall.CDDLFillDropDown()
            cddlCall.Width = Unit.Point(80)


            cddlTask.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TKTY""" & _
                    " and UDC.Company=" & cddlComp.CDDLGetValue & "  union " & _
                    " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""TKTY""" & _
                    " and UDC.Company=0 Order By Name"

            cddlTask.CDDLUDC = True
            cddlTask.CDDLMandatoryField = True



            cddlTask.CDDLFillDropDown()
            cddlTask.Width = Unit.Point(79)

            If Not Session("PropCompanyType") = "SCM" Then
                cddlComp.CDDLSetSelectedItem(Session("propCompanyId"), False, Session("propCompany"))
                cddlComp.Enabled = False
            End If

        Else



            cddlCall.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""CALL""" & _
                        " and UDC.Company=" & cddlComp.CDDLGetValue & "  union " & _
                        " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""CALL""" & _
                        " and UDC.Company=0 Order By Name"


            cddlCall.CDDLUDC = True

            cddlCall.CDDLMandatoryField = True


            cddlCall.CDDLFillDropDown()
            cddlCall.Width = Unit.Point(80)


            cddlTask.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""TKTY""" & _
                    " and UDC.Company=" & cddlComp.CDDLGetValue & "  union " & _
                    " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""TKTY""" & _
                    " and UDC.Company=0 Order By Name"

            cddlTask.CDDLUDC = True
            cddlTask.CDDLMandatoryField = True



            cddlTask.CDDLFillDropDown()
            cddlTask.Width = Unit.Point(79)



            cddlCall.CDDLSetItem()
            cddlTask.CDDLSetItem()
            cddlComp.CDDLSetItem()

        End If


        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenAdno = Request.Form("txthiddenAdno")

        'cpnlError.Visible = False
        'cpnlError.State = CustomControls.Web.PanelState.Collapsed

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SUserID") = Request.Form("txthidden")
            HttpContext.Current.Session("SUser") = Request.Form("txthiddenUser")
            HttpContext.Current.Session("SRole") = Request.Form("txthiddenRole")
            HttpContext.Current.Session("SCompany") = Request.Form("txthiddenCompany")
        End If

        lstError.Items.Clear()

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Edit"
                        'HttpContext.Current.Session("SUserID") = txthiddenAdno
                        'Response.Redirect("UserManage.aspx?ID=-1")
                    Case "Add"
                        HttpContext.Current.Session("SUserID") = ""
                        'Response.Redirect("UserManage.aspx?ID=1")

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            ''cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                            ' Return False
                            Exit Sub
                        End If
                        'End of Security Block
                        If ((cddlComp.CDDLGetValue = "") Or (cddlCall.CDDLGetValue = "") Or (cddlTask.CDDLGetValue = "")) Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Please select the Company, Call Type and Task Type fields to assign the form...")
                            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                            ''cpnlError.Visible = True
                            ''cpnlError.State = CustomControls.Web.PanelState.Expanded
                        Else
                            If IsNothing(dtItemDetail) = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("There are no records to save the form...")
                                ''cpnlError.Visible = True
                                ''cpnlError.State = CustomControls.Web.PanelState.Expanded
                                'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                            Else
                                If (dtItemDetail.Rows.Count < 1) Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("There are no records to save the form...")
                                    ' 'cpnlError.Visible = True
                                    ' 'cpnlError.State = CustomControls.Web.PanelState.Expanded
                                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                                Else
                                    'If readGrid() = False Then
                                    '    lstError.Items.Clear()
                                    '    lstError.Items.Add("Please select records to save the form...")
                                    '    ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                                    'Else
                                    If DeleteFormsRecords() = True And SaveFormsRecords() = True Then
                                        'lstError.Items.Clear()
                                        ''cpnlError.Visible = True
                                        ''cpnlError.State = CustomControls.Web.PanelState.Expanded
                                        'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                                    Else
                                    End If
                                End If
                                ' End If
                            End If
                        End If

                    Case "Close"
                        Response.Redirect("../home.aspx?ScrID=107")

                    Case "Search"
                        If ((cddlComp.CDDLGetValue = "")) Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Please fill the desired company to show the records...")
                            'cpnlError.Visible = True
                            'cpnlError.State = CustomControls.Web.PanelState.Expanded
                        Else
                            If PopulateFormsGrid() = False Then
                                'lstError.Items.Clear()
                                'cpnlError.Visible = True
                                'cpnlError.State = CustomControls.Web.PanelState.Expanded
                            End If
                        End If


                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                'Dim str As String = ex.ToString
                CreateLog("Form_Assingment", "Load-196", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        Else
            PopulateFormsGrid()
        End If

        'Security Block
        Dim intid As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If

        'End of Security Block


    End Sub

    Private Function PopulateFormsGrid() As Boolean


        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter

        dtItemDetail = New DataTable
        Dim sqlQuery As String
        Dim row As DataRow
        sqlCon = New SqlConnection(SQL.DBConnection)

        sqlQuery = "select FN_IN4_form_no, FN_VC100_Form_name from T110011" 'where FN_IN4_Company_ID=" & Val(cddlcomp.CDDLGetValue)

        sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
        sqlCon.Open()
        sqlda.Fill(dtItemDetail)
        sqlCon.Close()
        dtItemDetail.Columns.Add("IsExisted")
        Try
            'Fill a temp table which contains all the selected forms list for a particular call type/task type
            Dim dtTemp As New DataTable
            sqlQuery = "select FT_VC100_form_name from T110022 where FT_VC8_Call_Type = '" & cddlCall.CDDLGetValue.Trim & "' and FT_VC8_Task_Type =  '" & cddlTask.CDDLGetValue & "' and FT_IN4_Comp_id = " & Val(cddlComp.CDDLGetValue) & ""

            Dim sqlda1 As SqlDataAdapter
            sqlda1 = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda1.Fill(dtTemp)

            Dim drTemp As DataRow
            Dim i As Int16
            Dim dr As DataRow

            For i = 0 To dtTemp.Rows.Count - 1
                For Each drTemp In dtItemDetail.Rows
                    If (dtTemp.Rows(i).Item("FT_VC100_form_name") = drTemp.Item("FN_VC100_form_name")) Then
                        drTemp.Item("IsExisted") = "Y"
                    End If
                Next
            Next

            dtItemDetail.TableName = "T11"
            mdvtable.Table = dtItemDetail
            dgFormAssign.DataSource = mdvtable.Table
            dgFormAssign.DataBind()
            dgFormAssign.Columns(2).Visible = False

            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            CreateLog("CM_Form_assignment", "PopulateFormGrid-268", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            sqlCon.Close()
        End Try

    End Function

#End Region

    Private Sub dgFormAssign_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFormAssign.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String

        Session("propCAComp") = cddlComp.CDDLGetValue

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            If (CType(e.Item.Cells(2).FindControl("txtIsExists"), TextBox).Text = "Y") Then
                CType(e.Item.Cells(3).FindControl("rdlIsInserted"), RadioButtonList).SelectedValue = "Yes"
            Else
                CType(e.Item.Cells(3).FindControl("rdlIsInserted"), RadioButtonList).SelectedValue = "No"
            End If
        End If

        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    strID = dgFormAssign.DataKeys(e.Item.ItemIndex)
                    'strID = e.Item.Cells(1).Text()

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "')")

                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("Form_assignment", "ItemDatabound-179", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "GrdAddSearch")
        End Try


    End Sub
    Private Function SaveFormsRecords() As Boolean
        mstGetFunctionValue = InsertData()
        If mstGetFunctionValue.ErrorCode = 0 Then
            updateTaskTemplate()
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

            Return True
        ElseIf mstGetFunctionValue.ErrorCode = 1 Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

            Return False
        ElseIf mstGetFunctionValue.ErrorCode = 2 Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

            Return False
        End If
    End Function
    Function updateTaskTemplate() As Boolean
        Dim strValue As String
        Try
            If readGrid() Then
                strValue = "2"
            Else
                strValue = "0"
            End If
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            '    SQL.DBTable = "T060022"
            SQL.DBTracing = False
            If SQL.Update("invoiceEdit", "updateInvoiceLine-197", "update T040021 set TM_CH1_Forms='" & strValue & "' where TM_NU9_Call_No_FK  in(select CM_NU9_Call_No_PK from T040011 where CM_NU9_Comp_Id_FK=" & cddlComp.CDDLGetValue.Trim & " and CM_VC8_Call_Type='" & cddlCall.CDDLGetValue.Trim & "') and TM_VC8_task_type='" & cddlTask.CDDLGetValue.Trim & "' and TM_CH1_Forms<>'1' and TM_NU9_Comp_ID_FK=" & cddlComp.CDDLGetValue.Trim, SQL.Transaction.Serializable) Then
                If SQL.Update("invoiceEdit", "updateInvoiceLine-197", "update T050031 set TTM_CH1_Forms='" & strValue & "' where TTM_NU9_Call_No_FK in(select TCM_NU9_Call_No_PK from T050021 where TCM_NU9_CompId_FK=" & cddlComp.CDDLGetValue.Trim & " and TCM_VC8_Call_Type='" & cddlCall.CDDLGetValue.Trim & "') and TTM_VC8_task_type='" & cddlTask.CDDLGetValue.Trim & "' and  TTM_CH1_Forms<>'1' and TTM_NU9_Comp_ID_FK=" & cddlComp.CDDLGetValue.Trim, SQL.Transaction.Serializable) Then

                End If
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("server is busy please try later...")
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)

            CreateLog("CM_Form_assignment", "updateTaskTemplate-393", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
    Function readGrid() As Boolean
        Dim gridrow As DataGridItem
        For Each gridrow In dgFormAssign.Items
            If CType(gridrow.FindControl("rdlIsInserted"), RadioButtonList).SelectedValue = "Yes" Then
                Return True
            End If
        Next
        Return False
    End Function

    Function InsertData() As ReturnValue

        Dim strQuery As String
        Dim strConn As String
        Dim stReturn As ReturnValue

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConn
        SQL.DBTracing = False
        Dim i As Int16 = 0
        Dim gridrow As DataGridItem
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim intCompId As Int16 = Val(cddlComp.CDDLGetValue)
        Dim strCallType As String = cddlCall.CDDLGetValue
        Dim strTaskType As String = cddlTask.CDDLGetValue
        Try
            For Each gridrow In dgFormAssign.Items
                If (CType(gridrow.FindControl("rdlIsInserted"), RadioButtonList).SelectedValue = "Yes") Then
                    Dim str1 As String = dtItemDetail.Rows(i).Item("FN_VC100_form_name")

                    strQuery = "insert into T110022(FT_VC8_Call_Type, FT_VC8_Task_Type, FT_VC100_form_name, FT_IN4_Comp_id) values('" & strCallType & "','" & strTaskType & "','" & str1 & "'," & intCompId & ")"
                    With cmdCommand
                        .CommandText = strQuery
                        .CommandType = CommandType.Text
                        .Connection = objConn
                        .ExecuteNonQuery()
                    End With
                End If
                i = i + 1
            Next
            stReturn.ErrorMessage = "Records Saved Successfully..."
            stReturn.FunctionExecuted = True
            stReturn.ErrorCode = 0
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("CM_Form_assignment", "InsertData-362", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return stReturn
        Finally
            objConn.Close()
        End Try

    End Function

    Private Function DeleteFormsRecords() As Boolean

        Dim strQuery As String
        Dim strConn As String

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConn
        SQL.DBTracing = False

        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        objConn = New SqlConnection(strConn)
        Try
            objConn.Open()
            strQuery = "Delete from T110022 where FT_VC8_Call_Type = '" & cddlCall.CDDLGetValue & "' and FT_VC8_Task_Type = '" & cddlTask.CDDLGetValue & "' and FT_IN4_Comp_id = " & Val(cddlComp.CDDLGetValue)
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .ExecuteNonQuery()
            End With
            Return True
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("server is busy please try later...")
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("CM_Form_assignment", "DeleteFormsRecords-393", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            objConn.Close()
        End Try

    End Function
End Class
