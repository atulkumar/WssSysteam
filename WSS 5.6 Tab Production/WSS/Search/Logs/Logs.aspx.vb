'************************************************************************************************************
' Page                 :-WSS logs
' Purpose              :-Purpose of this screen is to search call, task, actions logs. 
' Date				Author						Modification Date					Description
' 15/05/07			Sachin	Prashar	     -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
#Region "Session On page"
'Session("PropCompanyType")
'Session("PropCompany")
'Session("PropCompanyID")
'Not Used
'Session("propcallnumber")
'Session("proptasknumber")
'Session("propCAComp")
#End Region

Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data

Partial Class Search_Logs_Logs
    Inherits System.Web.UI.Page
    Dim MdvTableCallSearch As New DataView
    Dim MdvtableTasksearch As New DataView
    Dim MdvTableActionSearch As New DataView
    Dim MdvTableCommSearch As New DataView
    Dim MdvTableAttachSearch As New DataView
    Dim txthiddenImage As String
    Dim txthiddentaskno As String
    Shared FlgSearch As Short = 0
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents imgSearch As System.Web.UI.WebControls.ImageButton
    Dim mintCompID As Integer


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CpnlSkillSet As CustomControls.Web.CollapsiblePanel
    Protected WithEvents cpnlCategory As CustomControls.Web.CollapsiblePanel
    Protected WithEvents Search As System.Web.UI.WebControls.Label
    Protected WithEvents cpnlAdvSearch0 As CustomControls.Web.CollapsiblePanel
    Protected WithEvents cpnlAdvSearch1 As CustomControls.Web.CollapsiblePanel
    Protected WithEvents chkcomment0 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox9 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox15 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox17 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox21 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox23 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DdlDomain As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
        ' imgBR.Attributes.Add("onclick", "OpenBR(0,'COM','cPnlContact_txtBr');")

        Try
            txtCSS(Me.Page)
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")

            TxtCallNo.Attributes.Add("onkeypress", "return NumericOnly();")
            TxtTaskNo.Attributes.Add("onkeypress", "return NumericOnly();")

            'query build for call table serch ...............................
            '''***************************************************

            Dim strquery As String = " "
            Dim queryflg As Short = 0


            If Not IsPostBack Then
                FlgSearch = 0


                cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlcallsearch.Enabled = False
                cpnlcallsearch.TitleCSS = "test2"
                cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
                cpnltasksearch.Enabled = False
                cpnltasksearch.TitleCSS = "test2"
                cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlactionsearch.Enabled = False
                cpnlactionsearch.TitleCSS = "test2"

            End If

            txthiddenImage = Request.Form("txthiddenImage")
            txthiddentaskno = Request.Form("txtTaskno")


            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Edit"
                        ''Session("propcallnumber") = Request.Form("txthidden")
                        ''Session("proptasknumber") = Request.Form("txtTaskno")
                        ''Session("propCAComp") = Request.Form("txtComp")
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                        '  Response.Redirect("..\..\SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1", False)
                End Select
            End If

            lstError.Items.Clear()
            If IsPostBack = False Then
                If Session("PropCompanyType") = "SCM" Then
                    FillCompanyDdl(ddlCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from  " & _
                    " T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA' order by Name")
                Else
                    ddlCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                    ddlCompany.SelectedValue = Session("PropCompanyID")
                    'Call ddlCompany_SelectedIndexChanged(Me, New EventArgs)
                    ddlCompany.Enabled = False
                End If
            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "Page_Load-189", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

#Region "Search functions"

    '*******************************************************************
    ' Function             :-  WSSCallSearch
    ' Purpose              :- Search on call level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 15/5/07			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub WSSCallLogSearch()

        Dim IntCallNo As Integer
        Dim intCompID As Integer
        Dim strSQL As String
        Dim Ds As New DataSet

        intCompID = ddlCompany.SelectedValue
        IntCallNo = TxtCallNo.Text.Trim

        Try
            ' strSQL = "Select * from T990011 where CM_NU9_Call_No_PK=" & IntCallNo & " and CM_NU9_Comp_Id_FK=" & intCompID & ""
            strSQL = "Select convert(varchar,CM_DT8_Close_Date,100) CM_DT8_Close_Date,CM_NU9_Call_No_PK,CM_VC2000_Call_Desc,CM_VC100_Subject,CM_VC8_Call_Type,CN_VC20_Call_Status,convert(varchar,CM_DT8_Log_Date,100) CM_DT8_Log_Date, Owner.UM_VC50_UserID as UserID , Modi.UM_VC50_UserID as ModifyBy,CM_VC200_Work_Priority from T990011 LOG, T060011 Owner, T060011 Modi where Owner.UM_IN4_Address_No_FK=LOG.CM_NU9_Call_Owner and modi.UM_IN4_Address_No_FK=*LOG.CM_NU9_ModifyBy  and CM_NU9_Call_No_PK=" & IntCallNo & " and CM_NU9_Comp_Id_FK=" & intCompID & "  order by CM_NU9_Call_No_PK "

            If SQL.Search("T990011", "Log", "Log-266", strSQL, Ds, "sachin", "prashar") = True Then


                Dim htDateCols As New Hashtable
                htDateCols.Add("CM_DT8_Log_Date", 1)
                htDateCols.Add("CM_DT8_Close_Date", 2)
                SetDataTableDateFormat(Ds.Tables(0), htDateCols)


                grdcall.DataSource = Ds.Tables(0)
                grdcall.DataBind()

                cpnlcallsearch.State = CustomControls.Web.PanelState.Expanded
                cpnlcallsearch.Enabled = True
                cpnlcallsearch.TitleCSS = "test"

            Else

                cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlcallsearch.Enabled = False
                cpnlcallsearch.TitleCSS = "test2"

                lstError.Items.Add("Call data not found according to your search string...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

            End If
        Catch ex As Exception
            CreateLog("LOG-Seacrh", "WSSCallLogSearch-235", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    '*******************************************************************
    ' Function             :-  WSSTaskSearch
    ' Purpose              :- Search on task level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 15/5/07			  Sachin Prashar           -------------------	               Created
    '
    '*******************************************************************

    Private Sub WSSTaskLogSearch()

        Try
            Dim IntCallNo As Integer
            Dim intTaskNo As Integer
            Dim StrCompName As String
            Dim intCompID As Integer
            Dim strSQL As String
            Dim Ds As New DataSet

            intCompID = ddlCompany.SelectedValue
            IntCallNo = TxtCallNo.Text.Trim
            intTaskNo = Val(TxtTaskNo.Text.Trim)

            If TxtTaskNo.Text.Trim.Equals("") Then

                strSQL = " select convert(varchar,TM_DT8_Est_close_date,100) TM_DT8_Est_close_date,TM_NU9_Call_No_FK,TM_NU9_Task_no_PK,TM_VC8_task_type,TM_VC1000_Subtsk_Desc, SOwner.UM_VC50_UserID,convert(varchar,TM_DT8_Log_Date,100) TM_DT8_Log_Date ,TM_VC50_Deve_status,Modi.UM_VC50_UserID as ModifyBy,TM_VC8_Priority  from T990021 LOG, T060011 SOwner, T060011 Modi where SOwner.UM_IN4_Address_No_FK=LOG.TM_VC8_Supp_Owner and Modi.UM_IN4_Address_No_FK=*LOG.TM_NU9_ModifyBy  and TM_NU9_Call_No_FK=" & IntCallNo & "  and TM_NU9_Comp_ID_FK=" & intCompID & " order by TM_NU9_Task_no_PK  "
            Else
                strSQL = " select convert(varchar,TM_DT8_Est_close_date,100) TM_DT8_Est_close_date,TM_NU9_Call_No_FK,TM_NU9_Task_no_PK,TM_VC8_task_type,TM_VC1000_Subtsk_Desc, SOwner.UM_VC50_UserID,convert(varchar,TM_DT8_Log_Date,100) TM_DT8_Log_Date,TM_VC50_Deve_status,Modi.UM_VC50_UserID as ModifyBy,TM_VC8_Priority   from T990021 LOG, T060011 SOwner, T060011 Modi where SOwner.UM_IN4_Address_No_FK=LOG.TM_VC8_Supp_Owner and Modi.UM_IN4_Address_No_FK=*LOG.TM_NU9_ModifyBy and TM_NU9_Call_No_FK=" & IntCallNo & " and TM_NU9_Task_no_PK=" & intTaskNo & " and TM_NU9_Comp_ID_FK=" & intCompID & "  order by TM_NU9_Task_no_PK "

            End If

            If SQL.Search("T990021", "Log", "Log-266", strSQL, Ds, "sachin", "prashar") = True Then

                Dim ht1 As New Hashtable
                ht1.Add("TM_VC1000_Subtsk_Desc", 20)

                Dim htDateCols As New Hashtable
                htDateCols.Add("TM_DT8_Log_Date", 1)
                htDateCols.Add("TM_DT8_Est_close_date", 2)

                HTMLEncodeDecode(mdlMain.Action.Encode, Ds.Tables(0), ht1)
                SetDataTableDateFormat(Ds.Tables(0), htDateCols)

                grdtask.DataSource = Ds.Tables(0)
                grdtask.DataBind()

                cpnltasksearch.State = CustomControls.Web.PanelState.Expanded
                cpnltasksearch.Enabled = True
                cpnltasksearch.TitleCSS = "test"
            Else
                cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
                cpnltasksearch.Enabled = False
                cpnltasksearch.TitleCSS = "test2"

                lstError.Items.Add("Task data not found according to your search string...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            End If
        Catch ex As Exception
            CreateLog("LOG-Seacrh", "WSSTaskLogSearch-259", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    '*******************************************************************
    ' Function             :-  WSSActionSearch
    ' Purpose              :- Search on Action level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 15/5/07			  Sachin Prashar           -------------------	                    Created
    '
    '*******************************************************************
    Private Sub WSSActionLogSearch()

        Try

            Dim IntCallNo As Integer
            Dim intTaskNo As Integer
            Dim StrCompName As String
            Dim intCompID As Integer
            Dim strSQL As String
            Dim Ds As New DataSet

            intCompID = ddlCompany.SelectedValue
            IntCallNo = TxtCallNo.Text.Trim
            intTaskNo = Val(TxtTaskNo.Text.Trim)

            If TxtTaskNo.Text.Trim.Equals("") Then
                strSQL = " select AM_NU9_Call_Number,AM_NU9_Task_Number,AM_NU9_Action_Number,AM_VC_2000_Description,Owner.UM_VC50_UserID,AM_VC8_Call_Status,convert(varchar,AM_DT8_Log_Date,100) AM_DT8_Log_Date,Modi.UM_VC50_UserID as ModifyBy from T990031 LOG, T060011 Owner, T060011 Modi where Owner.UM_IN4_Address_No_FK=*LOG.AM_VC8_Supp_Owner and Modi.UM_IN4_Address_No_FK=LOG.AM_NU9_ModifyBy and AM_NU9_Call_Number=" & IntCallNo & " and AM_NU9_Comp_ID_FK=" & intCompID & " order by AM_NU9_Task_Number,AM_NU9_Action_Number"
            Else
                strSQL = " select AM_NU9_Call_Number,AM_NU9_Task_Number,AM_NU9_Action_Number,AM_VC_2000_Description,Owner.UM_VC50_UserID,AM_VC8_Call_Status,convert(varchar,AM_DT8_Log_Date,100) AM_DT8_Log_Date,Modi.UM_VC50_UserID as ModifyBy from T990031 LOG, T060011 Owner ,T060011 Modi where Owner.UM_IN4_Address_No_FK=*LOG.AM_VC8_Supp_Owner and Modi.UM_IN4_Address_No_FK=LOG.AM_NU9_ModifyBy and AM_NU9_Call_Number=" & IntCallNo & " and AM_NU9_Comp_ID_FK=" & intCompID & " and AM_NU9_Task_Number=" & intTaskNo & " order by AM_NU9_Task_Number,AM_NU9_Action_Number"

            End If


            If SQL.Search("T990021", "Log", "Log-266", strSQL, Ds, "sachin", "prashar") = True Then

                Dim htDateCols As New Hashtable
                htDateCols.Add("AM_DT8_Log_Date", 1)
                SetDataTableDateFormat(Ds.Tables(0), htDateCols)

                grdaction.DataSource = Ds.Tables(0)
                grdaction.DataBind()

                cpnlactionsearch.State = CustomControls.Web.PanelState.Expanded
                cpnlactionsearch.Enabled = True
                cpnlactionsearch.TitleCSS = "test"
            Else
                cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlactionsearch.Enabled = False
                cpnlactionsearch.TitleCSS = "test2"

                lstError.Items.Add("Action data not found according to your search string...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

            End If
        Catch ex As Exception
            CreateLog("LOG-Seacrh", "WSSTaskLogSearch-259", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

#End Region

    Private Sub grdcall_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdcall.ItemDataBound

        Dim intcolno As Int16 = 0
     

    End Sub

    Private Sub grdtask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdtask.ItemDataBound

        Dim intcolno As Int16 = 0
       

    End Sub

    Private Sub grdaction_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdaction.ItemDataBound
        Dim intcolno As Int16 = 0
    End Sub
    Private Sub grdcall_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdcall.PageIndexChanged
        Try
            grdcall.CurrentPageIndex = e.NewPageIndex
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdcall_PageIndexChanged-985", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub grdtask_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdtask.PageIndexChanged
        Try
            grdtask.CurrentPageIndex = e.NewPageIndex
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdtask_PageIndexChanged-993", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub grdaction_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdaction.PageIndexChanged
        Try
            grdaction.CurrentPageIndex = e.NewPageIndex
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdaction_PageIndexChanged-1002", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub
    Private Sub grdcomm_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Try
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdcomm_PageIndexChanged-1010", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub grdatt_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Try
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdatt_PageIndexChanged-1018", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    '*******************************************************************
    ' Function             :-  BtnSearch_Click
    ' Purpose              :- Function call binddatagrid function and it returns value on check boxes selections
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/07/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub BtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles BtnSearch.Click
        Try

            BindDataGrids()

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "BtnSearch_Click-997", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    '*******************************************************************
    ' Function             :-  BindDataGrids
    ' Purpose              :- Function will return value on selected check boxs 
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/07/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub BindDataGrids()

        Try


            If DataValidation() = False Then
                Exit Sub
            End If


            If chkcallLevel.Checked = True Then
                WSSCallLogSearch()
            Else
                cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlcallsearch.Enabled = False
                cpnlcallsearch.TitleCSS = "test2"
            End If


            If chktskLevel.Checked = True Then
                WSSTaskLogSearch()
            Else
                cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
                cpnltasksearch.Enabled = False
                cpnltasksearch.TitleCSS = "test2"
            End If


            If ChkActLevel.Checked Then
                WSSActionLogSearch()
            Else
                cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlactionsearch.Enabled = False
                cpnlactionsearch.TitleCSS = "test2"

            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "BindDataGrids-1074", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub
    Private Function DataValidation() As Boolean

        Dim StrFlg As Short = 0
        lstError.Items.Clear()

        If ddlCompany.SelectedItem.Text = "" Then
            lstError.Items.Add("Company Name cannot be blank...")
            StrFlg = 1
        End If

        If TxtCallNo.Text.Trim.Equals("") Then
            lstError.Items.Add("Call Number cannot be blank...")
            StrFlg = 1
        End If


        If chkcallLevel.Checked = False And chktskLevel.Checked = False And ChkActLevel.Checked = False Then
            lstError.Items.Add("Please Select atleast one Search level...")
            StrFlg = 1
        End If


        If StrFlg = 1 Then

            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

            cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
            cpnlcallsearch.Enabled = False
            cpnlcallsearch.TitleCSS = "test2"

            cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
            cpnltasksearch.Enabled = False
            cpnltasksearch.TitleCSS = "test2"

            cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
            cpnlactionsearch.Enabled = False
            cpnlactionsearch.TitleCSS = "test2"


            Return False
        Else
            Return True
        End If

    End Function
    Private Sub imgbtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnSearch.Click
        BindDataGrids()
    End Sub
End Class
