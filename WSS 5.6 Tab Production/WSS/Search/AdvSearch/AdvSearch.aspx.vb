'************************************************************************************************************
' Page                 :-AdvSearch
' Purpose              :-Purpose of this screen is to search call, task, actions filtered by 
'                        From date & to date entered by user.User can also search call wise. 
' Date				Author						Modification Date					Description
' 4/03/06			Sachin		     -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data


Partial Class Search_AdvSearch_AdvSearch
    Inherits System.Web.UI.Page

    Dim MdvTableCallSearch As New DataView
    Dim MdvtableTasksearch As New DataView
    Dim MdvTableActionSearch As New DataView
    Dim MdvTableCommSearch As New DataView
    Dim MdvTableAttachSearch As New DataView

    Dim txthiddenImage As String
    Dim txthiddentaskno As String
    Shared FlgSearch As Short = 0
    Dim mintCompID As Integer
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents imgbtnSearch As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgSave As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgOk As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgEdit As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgReset As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgDelete As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImgError As System.Web.UI.WebControls.Image
    'Protected WithEvents lstError As System.Web.UI.WebControls.ListBox
    'Protected WithEvents cpnlError As CustomControls.Web.CollapsiblePanel
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
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        Try
            txtCSS(Me.Page)
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgEdit.Attributes.Add("Onclick", "return SaveEdit('Edit');")
            imgDelete.Attributes.Add("Onclick", "return ConfirmDelete('Delete');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            txtsearch.Attributes.Add("onkeyup", "return  SelectComp();")
            TxtCallFrm.Attributes.Add("onkeypress", "return NumericOnly();")
            TxtCallTo.Attributes.Add("onkeypress", "return NumericOnly();")

            ' txtsearch.Attributes.Add("onkeypress", "return  checkCapsLock();")

            'query build for call table serch ...............................
            '''***************************************************
            If Not IsPostBack Then
                ddlProject.Items.Clear()
                ddlProject.Items.Add("")
            End If

            Dim strquery As String = " "
            Dim queryflg As Short = 0

            ' dtFrom.readOnlyDate = False
            '  dtTo.readOnlyDate = False
            'dtFrom.Editable = True
            ' dtTo.Editable = True

            If Not IsPostBack Then
                FlgSearch = 0

                cpnlSearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlSearch.Enabled = False
                cpnlSearch.TitleCSS = "test2"

                cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlcallsearch.Enabled = False
                cpnlcallsearch.TitleCSS = "test2"
                cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
                cpnltasksearch.Enabled = False
                cpnltasksearch.TitleCSS = "test2"
                cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlactionsearch.Enabled = False
                cpnlactionsearch.TitleCSS = "test2"

                cpnlattachment.State = CustomControls.Web.PanelState.Collapsed
                cpnlattachment.Enabled = False
                cpnlattachment.TitleCSS = "test2"

                cpnlcomm.State = CustomControls.Web.PanelState.Collapsed
                cpnlcomm.Enabled = False
                cpnlcomm.TitleCSS = "test2"
            End If


            txthiddenImage = Request.Form("txthiddenImage")
            txthiddentaskno = Request.Form("txtTaskno")


            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Edit"
                        'Not Used Session
                        'Session("propcallnumber") = Request.Form("txthidden")
                        'Session("proptasknumber") = Request.Form("txtTaskno")
                        'Session("propCAComp") = Request.Form("txtComp")
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                        '  Response.Redirect("..\..\SupportCenter/CallView/Call_Detail.aspx?ScrID=3&ID=0&PageID=1", False)
                End Select
            End If

            lstError.Items.Clear()
            'cpnlError.Visible = False

            If IsPostBack = False Then
                If Session("PropCompanyType") = "SCM" Then
                    'FillCompanyDdl(ddlCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from  " & _
                    '" T010011 WHERE CI_VC8_Address_Book_Type ='COM'  and CI_VC8_Status='ENA'")
                    FillCompanyDdl(ddlCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")")

                Else
                    ddlCompany.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                    ddlCompany.SelectedValue = Session("PropCompanyID")
                    'Call ddlCompany_SelectedIndexChanged(Me, New EventArgs)
                    ddlCompany.Enabled = False
                    fillclientcomp()
                End If
            End If

            'data display 
            'If FlgSearch = 1 Then
            If txtsearch.Text <> "" And ddlCompany.SelectedItem.Text <> "" Then
                BindDataGrids()
            End If
            'End If


        Catch ex As Exception
            CreateLog("Advance-Seacrh", "Page_Load-189", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Try
            FillCompanyDdl(ddlProject, "select PR_NU9_Project_ID_Pk as ID,PR_VC20_Name Name  from  " & _
            " T210011 WHERE PR_NU9_Comp_ID_FK ='" & ddlCompany.SelectedValue & "' order by Name")
            FlgSearch = 1
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "ddlCompany_SelectedIndexChanged-311", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

#Region "Search functions"

    Private Sub fillclientcomp()
        Try
            FillCompanyDdl(ddlProject, "select PR_NU9_Project_ID_Pk as ID,PR_VC20_Name Name  from  " & _
       " T210011 WHERE PR_NU9_Comp_ID_FK ='" & ddlCompany.SelectedValue & "'")
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "fillclientcomp-254", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    '*******************************************************************
    ' Function             :-  WSSCallSearch
    ' Purpose              :- Search on call level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/7/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub WSSCallSearch()

        Dim strtxtstring As String
        Dim strwherestring As String = " where ( "
        Dim strcallsearchstring As String
        Dim dssearch As New DataSet
        Dim qryflg As New Short
        Dim IntCallFrm As Integer
        Dim IntCallTo As Integer
        Dim dtfromdate As String
        Dim dttodate As String

        Try
            strtxtstring = txtsearch.Text.Trim
            strtxtstring = GetSearchString(strtxtstring)

            If strtxtstring.Contains("*") = True Then
                strtxtstring = strtxtstring.Replace("*", "%")
            Else
                strtxtstring &= "%"
            End If

            If chkcallSubject.Checked = True Then
                strwherestring &= "CM_VC100_Subject like " & "'" & strtxtstring & "' "
                qryflg = 1
            End If

            If chkcalldesc.Checked Then
                If qryflg = 1 Then
                    strwherestring &= " OR CM_VC2000_Call_Desc like " & "'" & strtxtstring & "' "
                Else
                    strwherestring &= "CM_VC2000_Call_Desc like " & "'" & strtxtstring & "' "
                    qryflg = 1
                End If
            End If

            If chkcalltype.Checked Then
                If qryflg = 1 Then
                    strwherestring &= " OR CM_VC8_Call_Type like " & "'" & strtxtstring & "' "
                Else
                    strwherestring &= "CM_VC8_Call_Type like " & "'" & strtxtstring & "' "
                    qryflg = 1
                End If
            End If

            If chkcallowner.Checked = True Then
                If qryflg = 1 Then
                    strwherestring &= " OR UM_VC50_UserID like " & "'" & strtxtstring & "' ) and  Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and CM_NU9_Comp_Id_FK=" & mintCompID & " "
                Else
                    strwherestring &= "UM_VC50_UserID like " & "'" & strtxtstring & "' ) and  Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and CM_NU9_Comp_Id_FK=" & mintCompID & ""
                    qryflg = 1
                End If
                strcallsearchstring = " select CM_NU9_Call_No_PK,CM_VC2000_Call_Desc,CM_VC100_Subject,CM_VC8_Call_Type, Owner.UM_VC50_UserID  from T040011 call, T060011 Owner " & strwherestring
            Else
                Dim ser As String = " ) and  Owner.UM_IN4_Address_No_FK=Call.CM_NU9_Call_Owner and CM_NU9_Comp_Id_FK=" & mintCompID & " "
                strcallsearchstring = " select CM_NU9_Call_No_PK,CM_VC2000_Call_Desc,CM_VC100_Subject,CM_VC8_Call_Type, Owner.UM_VC50_UserID from T040011 call, T060011 Owner " & strwherestring & ser
            End If


            If ddlProject.SelectedItem.Text <> "" Then
                Dim prjvalue As String
                prjvalue = ddlProject.SelectedValue
                strcallsearchstring &= " and CM_NU9_Project_ID=" & prjvalue & ""
            End If

            If TxtCallFrm.Text <> "" Then
                IntCallFrm = TxtCallFrm.Text.Trim
                strcallsearchstring &= " and CM_NU9_Call_No_PK>= " & TxtCallFrm.Text.Trim & ""
            End If
            If TxtCallTo.Text <> "" Then
                IntCallTo = TxtCallTo.Text.Trim
                strcallsearchstring &= " and CM_NU9_Call_No_PK<=" & TxtCallTo.Text.Trim & ""
            End If
            If dtFrom.Text <> "" Then
                dtfromdate = dtFrom.Text
                strcallsearchstring &= " and   convert(datetime,(convert(varchar,CM_DT8_Request_Date,101)),110) >='" & dtfromdate & "'"
            End If
            If dtTo.Text <> "" Then
                dttodate = dtTo.Text
                strcallsearchstring &= " and  convert(datetime,(convert(varchar,CM_DT8_Request_Date,101)),110) <='" & dttodate & "'"
            End If


            'SQL.DBTable = "T040011"
            'strConnection =  System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("T040011", "freesearch", "freesearch-166", strcallsearchstring, dssearch, "Amit", "Amit") = True Then

                MdvTableCallSearch.Table = dssearch.Tables(0)

                Dim htGrdColumns As New Hashtable
                htGrdColumns.Add("CM_VC2000_Call_Desc", 20)
                htGrdColumns.Add("CM_VC100_Subject", 10)
                HTMLEncodeDecode(mdlMain.Action.Encode, MdvTableCallSearch, htGrdColumns)

                grdcall.DataSource = MdvTableCallSearch
                grdcall.DataBind()

                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                cpnlSearch.Enabled = True
                cpnlSearch.TitleCSS = "test"

                cpnlcallsearch.State = CustomControls.Web.PanelState.Expanded
                cpnlcallsearch.Enabled = True
                cpnlcallsearch.TitleCSS = "test"
            Else
                lstError.Items.Add("Call data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlcallsearch.TitleCSS = "test2"
                cpnlcallsearch.Enabled = False
            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "WSSCallSearch-432", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    '*******************************************************************
    ' Function             :-  WSSTaskSearch
    ' Purpose              :- Search on task level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/7/06			  Sachin Prashar      -------------------	               Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************

    Private Sub WSSTaskSearch()

        Dim strtxtstring As String
        Dim strwherestring As String = " where ( "
        Dim strcallsearchstring As String
        Dim dssearch As New DataSet
        Dim QryFlg As New Short
        Dim IntCallFrm As Integer
        Dim IntCallTo As Integer
        Dim dtfromdate As String
        Dim dttodate As String

        Try

            strtxtstring = txtsearch.Text.Trim
            strtxtstring = GetSearchString(strtxtstring)
            ' strtxtstring = strtxtstring.Replace("*", "%")
            If strtxtstring.Contains("*") = True Then
                strtxtstring = strtxtstring.Replace("*", "%")
            Else
                strtxtstring &= "%"
            End If

            'If chkSubject.Checked = True Then
            '    strwherestring &= "CM_VC100_Subject like " & "'" & strtxtstring & "' "
            '    qry = 1

            'End If

            If chktskdesc.Checked Then
                ' If qry = 1 Then
                'strwherestring &= " OR TM_VC1000_Subtsk_Desc like " & "'" & strtxtstring & "' "
                ' Else
                strwherestring &= "TM_VC1000_Subtsk_Desc like " & "'" & strtxtstring & "' "
                QryFlg = 1
                '  End If
            End If

            If chktskowner.Checked = True Then
                If QryFlg = 1 Then
                    strwherestring &= " OR UM_VC50_UserID like " & "'" & strtxtstring & "' ) and  SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and TM_NU9_Comp_ID_FK=" & mintCompID & ""
                Else
                    strwherestring &= "UM_VC50_UserID like " & "'" & strtxtstring & "' ) and  SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and TM_NU9_Comp_ID_FK=" & mintCompID & " "
                    QryFlg = 1
                End If
                strcallsearchstring = " select TM_NU9_Call_No_FK,TM_NU9_Task_no_PK,TM_VC8_task_type,TM_VC1000_Subtsk_Desc, SOwner.UM_VC50_UserID  from T040021 Task, T060011 SOwner " & strwherestring
            Else
                Dim ser As String = " ) and  SOwner.UM_IN4_Address_No_FK=Task.TM_VC8_Supp_Owner and TM_NU9_Comp_ID_FK=" & mintCompID & " "
                strcallsearchstring = " select TM_NU9_Call_No_FK,TM_NU9_Task_no_PK,TM_VC8_task_type,TM_VC1000_Subtsk_Desc, SOwner.UM_VC50_UserID  from T040021 Task, T060011 SOwner " & strwherestring & ser
            End If

            If ddlProject.SelectedItem.Text <> "" Then
                Dim prjvalue As String
                prjvalue = ddlProject.SelectedValue
                strcallsearchstring &= " and TM_NU9_Project_ID=" & prjvalue & ""
            End If

            If TxtCallFrm.Text <> "" Then
                IntCallFrm = TxtCallFrm.Text.Trim
                strcallsearchstring &= " and TM_NU9_Call_No_FK>= " & TxtCallFrm.Text.Trim & ""
            End If

            If TxtCallTo.Text <> "" Then
                IntCallTo = TxtCallTo.Text.Trim
                strcallsearchstring &= " and TM_NU9_Call_No_FK<=" & TxtCallTo.Text.Trim & ""
            End If

            If dtFrom.Text <> "" Then
                dtfromdate = dtFrom.Text
                strcallsearchstring &= " and  TM_DT8_Task_Date >='" & dtfromdate & "'"
            End If

            If dtTo.Text <> "" Then
                dttodate = dtTo.Text
                strcallsearchstring &= "  and TM_DT8_Task_Date <='" & dttodate & "'"
            End If


            'strcallsearchstring = " select * from T040021 " & strwherestring
            '            SQL.DBTable = "T040021"
            'strConnection =  System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("T040021", "freesearch", "freesearch-166", strcallsearchstring, dssearch, "Amit", "Amit") = True Then

                MdvtableTasksearch.Table = dssearch.Tables(0)

                Dim htGrdColumns As New Hashtable
                htGrdColumns.Add("TM_VC1000_Subtsk_Desc", 20)
                HTMLEncodeDecode(mdlMain.Action.Encode, MdvtableTasksearch, htGrdColumns)

                grdtask.DataSource = MdvtableTasksearch
                grdtask.DataBind()

                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                cpnlSearch.Enabled = True
                cpnlSearch.TitleCSS = "test"
                cpnltasksearch.State = CustomControls.Web.PanelState.Expanded
                cpnltasksearch.Enabled = True
                cpnltasksearch.TitleCSS = "test"
            Else
                lstError.Items.Add("Task data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
                cpnltasksearch.Enabled = False
                cpnltasksearch.TitleCSS = "test2"

            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "WSSTaskSearch-549", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    '*******************************************************************
    ' Function             :- WSSDocumentSearch
    ' Purpose              :- Search on Documents 
    '								
    ' Date					  Author						
    ' 2/9/08			  Saurabh Mohal              
    '
    '*******************************************************************

    Private Sub WSSDocumentSearch()

        Dim strtxtstring As String
        Dim strwherestring As String = " where ( "
        Dim strcallsearchstring As String
        Dim dssearch As New DataSet
        Dim QryFlg As New Short
     

        Try

            strtxtstring = txtsearch.Text.Trim
            strtxtstring = GetSearchString(strtxtstring)
            'strtxtstring = strtxtstring.Replace("*", "%")
            If strtxtstring.Contains("*") = True Then
                strtxtstring = strtxtstring.Replace("*", "%")
            Else
                strtxtstring &= "%"
            End If

            If Chkfilename.Checked Then

                If ChkDesc.Checked = True Then
                    strwherestring &= "FI_VC255_File_Name like " & "'" & strtxtstring & "'"
                    strwherestring &= " OR  FI_VC500_File_Description like " & "'" & strtxtstring & "' ) "
                Else

                    strwherestring &= "FI_VC255_File_Name like " & "'" & strtxtstring & "')"
                End If
                strwherestring &= " and  FI_NU9_UploadBy_ID_FK=CI_NU8_Address_Number"
                strwherestring &= " and  FI_NU9_Folder_ID_FK=FD_NU9_Folder_ID_PK"
                QryFlg = 1

            End If

            If ChkDesc.Checked = True Then
                If QryFlg = 1 Then
                    'strwherestring &= " OR ( FI_VC500_File_Description like " & "'" & strtxtstring & "' ) "
                    
                Else
                    strwherestring &= "FI_VC500_File_Description like " & "'" & strtxtstring & "' ) "
                    strwherestring &= " and  FI_NU9_UploadBy_ID_FK=CI_NU8_Address_Number"
                    strwherestring &= " and  FI_NU9_Folder_ID_FK=FD_NU9_Folder_ID_PK"
                End If

            End If

            strcallsearchstring = " Select FI_VC255_File_Name,FI_VC500_File_Description,FI_DT8_Upload_ON ,CI_VC36_Name,(FD_VC255_Folder_Path + FD_VC255_Folder_Name) Fname from T250011,T010011,T250021" & strwherestring

           


            'strcallsearchstring = " select * from T040021 " & strwherestring
            '            SQL.DBTable = "T040021"
            'strConnection =  System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("T250011", "freesearch", "freesearch-166", strcallsearchstring, dssearch, "Amit", "Amit") = True Then

                MdvtableTasksearch.Table = dssearch.Tables(0)

                Dim htGrdColumns As New Hashtable
                htGrdColumns.Add("FI_VC255_File_Name", 20)
                HTMLEncodeDecode(mdlMain.Action.Encode, MdvtableTasksearch, htGrdColumns)

                grddoc.DataSource = MdvtableTasksearch
                grddoc.DataBind()

                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                cpnlSearch.Enabled = True
                cpnlSearch.TitleCSS = "test"
                cpnldocument.State = CustomControls.Web.PanelState.Expanded
                cpnldocument.Enabled = True
                cpnldocument.TitleCSS = "test"
            Else
                lstError.Items.Add("Task data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnldocument.State = CustomControls.Web.PanelState.Collapsed
                cpnldocument.Enabled = False
                cpnldocument.TitleCSS = "test2"

            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "WSSTaskSearch-549", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub





    '*******************************************************************
    ' Function             :-  WSSActionSearch
    ' Purpose              :- Search on Action level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/7/06			  Sachin Prashar           -------------------	                    Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub WSSActionSearch()
        Dim strtxtstring As String
        Dim strwherestring As String = " where ( "
        Dim strcallsearchstring As String
        Dim dssearch As New DataSet
        Dim QryFlg As New Short
        Dim IntCallFrm As Integer
        Dim IntCallTo As Integer
        Dim dtfromdate As String
        Dim dttodate As String

        Try

            strtxtstring = txtsearch.Text.Trim
            strtxtstring = GetSearchString(strtxtstring)
            'strtxtstring = strtxtstring.Replace("*", "%")
            If strtxtstring.Contains("*") = True Then
                strtxtstring = strtxtstring.Replace("*", "%")
            Else
                strtxtstring &= "%"
            End If

            'If chkSubject.Checked = True Then
            '    strwherestring &= "CM_VC100_Subject like " & "'" & strtxtstring & "' "
            '    qry = 1
            'End If
            If chkactdesc.Checked Then
                ' If qry = 1 Then
                'strwherestring &= " OR TM_VC1000_Subtsk_Desc like " & "'" & strtxtstring & "' "
                ' Else
                strwherestring &= "AM_VC_2000_Description like " & "'" & strtxtstring & "' "
                QryFlg = 1
                '  End If
            End If
            If ChkActOwn.Checked = True Then
                If QryFlg = 1 Then
                    strwherestring &= " OR UM_VC50_UserID like " & "'" & strtxtstring & "' ) and  Owner.UM_IN4_Address_No_FK=Action.AM_VC8_Supp_Owner and AM_NU9_Comp_ID_FK=" & mintCompID & " "
                Else
                    strwherestring &= "UM_VC50_UserID like " & "'" & strtxtstring & "' ) and  Owner.UM_IN4_Address_No_FK=Action.AM_VC8_Supp_Owner and AM_NU9_Comp_ID_FK=" & mintCompID & ""
                    QryFlg = 1
                End If
                strcallsearchstring = " select AM_NU9_Call_Number,AM_NU9_Task_Number,AM_NU9_Action_Number,AM_VC_2000_Description,Owner.UM_VC50_UserID  from T040031 action, T060011 Owner " & strwherestring
            Else
                Dim ser As String = " ) and  Owner.UM_IN4_Address_No_FK=Action.AM_VC8_Supp_Owner and AM_NU9_Comp_ID_FK=" & mintCompID & " "
                strcallsearchstring = " select AM_NU9_Call_Number,AM_NU9_Task_Number,AM_NU9_Action_Number,AM_VC_2000_Description,Owner.UM_VC50_UserID T040031 action, T060011 Owner " & strwherestring & ser
            End If


            If TxtCallFrm.Text <> "" Then
                IntCallFrm = TxtCallFrm.Text.Trim
                strcallsearchstring &= " and AM_NU9_Call_Number>= " & TxtCallFrm.Text.Trim & ""
            End If

            If TxtCallTo.Text <> "" Then
                IntCallTo = TxtCallTo.Text.Trim
                strcallsearchstring &= " and AM_NU9_Call_Number<=" & TxtCallTo.Text.Trim & ""
            End If

            If dtFrom.Text <> "" Then
                dtfromdate = dtFrom.Text
                strcallsearchstring &= " and   AM_DT8_Action_Date >='" & dtfromdate & "'"
            End If

            If dtTo.Text <> "" Then
                dttodate = dtTo.Text
                strcallsearchstring &= " and AM_DT8_Action_Date <='" & dttodate & "'"
            End If

            ' strcallsearchstring = " select * from T040031 " & strwherestring
            '            SQL.DBTable = "T040031"
            'strConnection =  System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("T040031", "freesearch", "freesearch-166", strcallsearchstring, dssearch, "Amit", "Amit") = True Then
                MdvTableActionSearch.Table = dssearch.Tables(0)

                Dim htGrdColumns As New Hashtable
                htGrdColumns.Add("AM_VC_2000_Description", 25)
                HTMLEncodeDecode(mdlMain.Action.Encode, MdvTableActionSearch, htGrdColumns)

                grdaction.DataSource = MdvTableActionSearch
                grdaction.DataBind()

                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                cpnlSearch.Enabled = True
                cpnlSearch.TitleCSS = "test"
                cpnlactionsearch.State = CustomControls.Web.PanelState.Expanded
                cpnlactionsearch.Enabled = True
                cpnlactionsearch.TitleCSS = "test"

            Else

                lstError.Items.Add("Action data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlactionsearch.Enabled = False
                cpnlactionsearch.TitleCSS = "test2"

            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "WSSActionSearch-654", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    '*******************************************************************
    ' Function             :-  WssAttachmentSearch
    ' Purpose              :- Search on call level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/7/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub WssAttachmentSearch()

        Dim strtxtstring As String
        Dim strCommsearchstring As String
        Dim dssearch As New DataSet
        Dim QryFlg As New Short
        Dim IntCallFrm As Integer
        Dim IntCallTo As Integer
        Dim dtfromdate As String
        Dim dttodate As String

        Try


            strtxtstring = txtsearch.Text.Trim
            strtxtstring = GetSearchString(strtxtstring)
            'strtxtstring = strtxtstring.Replace("*", "%")
            If strtxtstring.Contains("*") = True Then
                strtxtstring = strtxtstring.Replace("*", "%")
            Else
                strtxtstring &= "%"
            End If

            'call
            If chkcallAtt.Checked = True And chktskatt.Checked = False And Chkactatt.Checked = False Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and  VH_IN4_Level in ('1') and VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
                'task
            ElseIf chkcallAtt.Checked = False And chktskatt.Checked = True And Chkactatt.Checked = False Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and  VH_IN4_Level in ('2') and VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
                'action
            ElseIf chkcallAtt.Checked = False And chktskatt.Checked = False And Chkactatt.Checked = True Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and  VH_IN4_Level in ('3') and VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
                'call and task
            ElseIf chkcallAtt.Checked = True And chktskatt.Checked = True And Chkactatt.Checked = False Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and  VH_IN4_Level in ('1','2') and VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
                'call and action
            ElseIf chkcallAtt.Checked = True And chktskatt.Checked = False And Chkactatt.Checked = True Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and  VH_IN4_Level in ('1','3') and VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
                'task and action
            ElseIf chkcallAtt.Checked = False And chktskatt.Checked = True And Chkactatt.Checked = True Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and VH_IN4_Level in ('2','3') and VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
                'call ,task and action
            ElseIf chkcallAtt.Checked = True And chktskatt.Checked = True And Chkactatt.Checked = True Then
                strCommsearchstring = "select distinct VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,convert(varchar,VH_DT8_Date,101) VH_DT8_Date,VH_VC255_File_Name ,AttOwn.UM_VC50_UserID  from T040051 Att, T060011 AttOwn,T040011 where VH_VC255_File_Name like '" & strtxtstring & "' and   VH_NU9_CompId_Fk=" & mintCompID & " and AttOwn.UM_IN4_Address_No_FK=Att.VH_NU9_Address_Book_Number and vh_nu9_call_number=CM_NU9_Call_No_Pk "
            End If


            If TxtCallFrm.Text <> "" Then
                IntCallFrm = TxtCallFrm.Text.Trim
                strCommsearchstring &= " and VH_NU9_Call_Number>= " & TxtCallFrm.Text.Trim & ""
            End If

            If TxtCallTo.Text <> "" Then
                IntCallTo = TxtCallTo.Text.Trim
                strCommsearchstring &= " and VH_NU9_Call_Number<=" & TxtCallTo.Text.Trim & ""
            End If

            If dtFrom.Text <> "" Then
                dtfromdate = dtFrom.Text
                strCommsearchstring &= " and  VH_DT8_Date >='" & dtfromdate & "'"
            End If

            If dtTo.Text <> "" Then
                dttodate = dtTo.Text
                strCommsearchstring &= " and  VH_DT8_Date <='" & dttodate & "'"
            End If

            If ddlProject.SelectedItem.Text.Trim <> "" Then
                Dim prjvalue As String
                prjvalue = ddlProject.SelectedValue
                strCommsearchstring &= " and CM_NU9_Project_ID=" & prjvalue & "  "
            End If

            '            SQL.DBTable = "T040051"
            If SQL.Search("T040051", "freesearch", "freesearch-166", strCommsearchstring, dssearch, "Amit", "Amit") = True Then
                MdvTableAttachSearch.Table = dssearch.Tables(0)

                Dim htDateCols As New Hashtable
                htDateCols.Add("VH_DT8_Date", 2)
                SetDataTableDateFormat(dssearch.Tables(0), htDateCols)


                grdatt.DataSource = dssearch.Tables(0)
                grdatt.DataBind()
                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                cpnlSearch.Enabled = True
                cpnlSearch.TitleCSS = "test"
                cpnlattachment.State = CustomControls.Web.PanelState.Expanded
                cpnlattachment.Enabled = True
                cpnlattachment.TitleCSS = "test"
            Else
                lstError.Items.Add("Attachment data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlattachment.State = CustomControls.Web.PanelState.Collapsed
                cpnlattachment.Enabled = False
                cpnlattachment.TitleCSS = "test2"
            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "WssAttachmentSearch-753", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub


    '*******************************************************************
    ' Function             :-  WssCommentSearch
    ' Purpose              :- Search on call level 
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/7/06			      Sachin Prashar           -------------------	Created
    '
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub WssCommentSearch()
        Dim strtxtstring As String
        Dim strCommsearchstring As String
        Dim dssearch As New DataSet
        Dim QryFlg As New Short
        Dim IntCallFrm As Integer
        Dim IntCallTo As Integer
        Dim dtfromdate As String
        Dim dttodate As String

        Try

            strtxtstring = txtsearch.Text.Trim
            strtxtstring = GetSearchString(strtxtstring)
            'strtxtstring = strtxtstring.Replace("*", "%")
            If strtxtstring.Contains("*") = True Then
                strtxtstring = strtxtstring.Replace("*", "%")
            Else
                strtxtstring &= "%"
            End If

            'call........
            If chkcallcomm.Checked = True And chktskcomm.Checked = False And chkactcomm.Checked = False Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('C')  and CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and  CM_VC2_Flag in ('C') and CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number  and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
                'task........
            ElseIf chkcallcomm.Checked = False And chktskcomm.Checked = True And chkactcomm.Checked = False Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('T')  and CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('T')  and CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
                'action............
            ElseIf chkcallcomm.Checked = False And chktskcomm.Checked = False And chkactcomm.Checked = True Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('A') and CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('A') and CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
                'call and task..............
            ElseIf chkcallcomm.Checked = True And chktskcomm.Checked = True And chkactcomm.Checked = False Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('C','T')  and CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk  "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('C','T')  and CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
                'call and action..............
            ElseIf chkcallcomm.Checked = True And chktskcomm.Checked = False And chkactcomm.Checked = True Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('C','A') and CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('C','A')  and CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
                'task and action..................
            ElseIf chkcallcomm.Checked = False And chktskcomm.Checked = True And chkactcomm.Checked = True Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('T','A') and CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and CM_VC2_Flag in ('T','A') and CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
                'call ,task and action....................
            ElseIf chkcallcomm.Checked = True And chktskcomm.Checked = True And chkactcomm.Checked = True Then
                If Session("PropCompanyType") = "SCM" Then
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and   CM_NU9_CompId_Fk=" & mintCompID & " and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                Else
                    strCommsearchstring = "select distinct CM_NU9_Comment_Number_PK,CM_NU9_Call_Number,CM_NU9_Task_Number,CM_NU9_Action_Number,convert(varchar,CM_DT8_Date,101) CM_DT8_Date,CM_VC256_Comments,CommOwn.UM_VC50_UserID  from T040061 comm, T060011 CommOwn,T040011 where CM_VC256_Comments like '" & strtxtstring & "' and   CM_NU9_CompId_Fk=" & mintCompID & " and CM_VC50_IE='External'  and CommOwn.UM_IN4_Address_No_FK=comm.CM_NU9_AB_Number and CM_NU9_Call_Number=CM_NU9_Call_No_Pk "
                End If
            End If


            If TxtCallFrm.Text <> "" Then
                IntCallFrm = TxtCallFrm.Text.Trim
                strCommsearchstring &= " and CM_NU9_Call_Number>= " & TxtCallFrm.Text.Trim & ""
            End If

            If TxtCallTo.Text <> "" Then
                IntCallTo = TxtCallTo.Text.Trim
                strCommsearchstring &= " and CM_NU9_Call_Number<=" & TxtCallTo.Text.Trim & ""
            End If

            If dtFrom.Text <> "" Then
                dtfromdate = dtFrom.Text
                strCommsearchstring &= " and  CM_DT8_Date >='" & dtfromdate & "'"
            End If

            If dtTo.Text <> "" Then
                dttodate = dtTo.Text
                strCommsearchstring &= " and  CM_DT8_Date <='" & dttodate & "'"
            End If

            If ddlProject.SelectedItem.Text.Trim <> "" Then
                Dim prjvalue As String
                prjvalue = ddlProject.SelectedValue
                strCommsearchstring &= " and CM_NU9_Project_ID=" & prjvalue & " "
            End If

            '            SQL.DBTable = "T040061"
            If SQL.Search("T040061", "freesearch", "freesearch-166", strCommsearchstring, dssearch, "Amit", "Amit") = True Then
                MdvTableCommSearch.Table = dssearch.Tables(0)

                Dim htDateCols As New Hashtable
                htDateCols.Add("CM_DT8_Date", 2)
                SetDataTableDateFormat(dssearch.Tables(0), htDateCols)

                grdcomm.DataSource = dssearch.Tables(0)
                grdcomm.DataBind()

                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                cpnlSearch.Enabled = True
                cpnlSearch.TitleCSS = "test"
                cpnlcomm.State = CustomControls.Web.PanelState.Expanded
                cpnlcomm.Enabled = True
                cpnlcomm.TitleCSS = "test"

            Else

                lstError.Items.Add("Comment data not found according to your search string... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlcomm.State = CustomControls.Web.PanelState.Collapsed
                cpnlcomm.Enabled = False
                cpnlcomm.TitleCSS = "test2"

            End If

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "WssCommentSearch-885", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

#End Region

    Private Sub grdcall_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdcall.ItemDataBound

        Dim intcolno As Int16 = 0
        Dim dcCol As DataColumn
        Dim compid As Integer
        Dim grdrowid As Integer
        Dim intcallno As Integer

        Try

            compid = ddlCompany.SelectedValue 'getting company ID 

            For Each dcCol In MdvTableCallSearch.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    intcallno = grdcall.DataKeys(e.Item.ItemIndex) 'getting call number
                    e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & intcallno & ", '" & grdrowid & "','cpnlcall_grdcall'," & compid & ",0)")
                    e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & intcallno & ", '" & grdrowid & "','cpnlcall_grdcall'," & compid & ",0)")
                End If
                intcolno = intcolno + 1
            Next
            grdrowid = +1

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdcall_ItemDataBound-915", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Sub grdtask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdtask.ItemDataBound

        Dim intcolno As Int16 = 0
        Dim dcCol As DataColumn
        Dim compid As Integer
        Dim grdrowid As Integer
        Dim intTaskno As Integer
        Dim intCallno As Integer

        Try
            compid = ddlCompany.SelectedValue

            For Each dcCol In MdvtableTasksearch.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    intTaskno = e.Item.Cells(1).Text() 'getting task number
                    intCallno = e.Item.Cells(0).Text() 'getting call number
                    e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & intCallno & ", '" & grdrowid & "','cpnltask_grdtask'," & compid & "," & intTaskno & ")")
                    e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & intCallno & ", '" & grdrowid & "','cpnltask_grdtask'," & compid & "," & intTaskno & ")")

                End If
                intcolno = intcolno + 1
            Next
            grdrowid = +1

        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdtask_ItemDataBound-946", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub

    Private Sub grdaction_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdaction.ItemDataBound

        Dim intcolno As Int16 = 0
        Dim dcCol As DataColumn
        Dim compid As Integer
        Dim grdrowid As Integer
        Dim intCallno As Integer
        Dim intTaskno As Integer
        Dim intActionno As Integer

        Try
            compid = ddlCompany.SelectedValue
            For Each dcCol In MdvTableActionSearch.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    intCallno = e.Item.Cells(0).Text() ' getting call number
                    intTaskno = e.Item.Cells(1).Text() 'getting task number
                    intActionno = e.Item.Cells(2).Text 'getting action numbers
                    e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & intCallno & ", '" & grdrowid & "','cpnlaction_grdaction'," & compid & "," & intTaskno & ")")
                    e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & intCallno & ", '" & grdrowid & "','cpnlaction_grdaction'," & compid & "," & intTaskno & ")")

                End If
                intcolno = intcolno + 1
            Next
            grdrowid = +1
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdaction_ItemDataBound-977", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
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
            ' Call BtnSearch_Click(Me, New EventArgs)
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
    Private Sub grdcomm_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdcomm.PageIndexChanged
        Try
            grdcomm.CurrentPageIndex = e.NewPageIndex
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdcomm_PageIndexChanged-1010", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub grdatt_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdatt.PageIndexChanged
        Try
            grdatt.CurrentPageIndex = e.NewPageIndex
            BindDataGrids()
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdatt_PageIndexChanged-1018", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Private Sub grdcomm_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdcomm.ItemDataBound

        Dim intcolno As Int16 = 0
        Dim dcCol As DataColumn
        Dim compid As Integer
        Dim grdrowid As Integer
        Dim intCallno As Integer
        Dim intTaskno As Integer
        Dim intActionno As Integer

        Try
            compid = ddlCompany.SelectedValue
            For Each dcCol In MdvTableCommSearch.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    'Set the Comments level, call , task and action level 
                    If e.Item.Cells(0).Text <> 0 And e.Item.Cells(1).Text = 0 And e.Item.Cells(2).Text = 0 Then
                        e.Item.Cells(6).Text = "Call Level"
                    ElseIf e.Item.Cells(0).Text <> 0 And e.Item.Cells(1).Text <> 0 And e.Item.Cells(2).Text = 0 Then
                        e.Item.Cells(6).Text = "Task Level"
                    Else
                        e.Item.Cells(6).Text = "Action Level"
                    End If

                    intCallno = e.Item.Cells(0).Text() ' getting call number 
                    intTaskno = e.Item.Cells(1).Text() 'getting task number
                    intActionno = e.Item.Cells(2).Text 'getting action number

                    e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & intCallno & ", '" & grdrowid & "','cpnlaction_grdaction'," & compid & "," & intTaskno & ")")
                    e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & intCallno & ", '" & grdrowid & "','cpnlaction_grdaction'," & compid & "," & intTaskno & ")")

                End If
                intcolno = intcolno + 1
            Next
            grdrowid = +1
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdcomm_ItemDataBound-1048", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub

    Private Sub grdatt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdatt.ItemDataBound
        Dim intcolno As Int16 = 0
        Dim dcCol As DataColumn
        Dim compid As Integer
        Dim grdrowid As Integer
        Dim intCallno As Integer
        Dim intTaskno As Integer
        Dim intActionno As Integer

        Try
            compid = ddlCompany.SelectedValue
            For Each dcCol In MdvTableAttachSearch.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    'Set the Attachment level, call , task and action level 
                    If e.Item.Cells(0).Text <> 0 And e.Item.Cells(1).Text = 0 And e.Item.Cells(2).Text = 0 Then
                        e.Item.Cells(6).Text = "Call Level"
                    ElseIf e.Item.Cells(0).Text <> 0 And e.Item.Cells(1).Text <> 0 And e.Item.Cells(2).Text = 0 Then
                        e.Item.Cells(6).Text = "Task Level"
                    Else
                        e.Item.Cells(6).Text = "Action Level"
                    End If


                    intCallno = e.Item.Cells(0).Text() 'getting call number
                    intTaskno = e.Item.Cells(1).Text() 'getting task number
                    intActionno = e.Item.Cells(2).Text 'getting action number
                    e.Item.Cells(intcolno).Attributes.Add("onclick", "javascript:KeyCheck(" & intCallno & ", '" & grdrowid & "','cpnlaction_grdaction'," & compid & "," & intTaskno & ")")
                    e.Item.Cells(intcolno).Attributes.Add("onDBlclick", "javascript:KeyCheck55(" & intCallno & ", '" & grdrowid & "','cpnlaction_grdaction'," & compid & "," & intTaskno & ")")

                End If
                intcolno = intcolno + 1
            Next
            grdrowid = +1
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "grdatt_ItemDataBound-1078", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
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

            If ddlCompany.SelectedItem.Text = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Company Name cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                cpnlSearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlSearch.TitleCSS = "test2"
                cpnlSearch.Enabled = False

                Exit Sub
            End If

            If txtsearch.Text.Trim = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Search text box cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlSearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlSearch.TitleCSS = "test2"
                cpnlSearch.Enabled = False

                Exit Sub
            End If

            Dim compname As String
            compname = ddlCompany.SelectedItem.Text.Trim
            mstGetFunctionValue = WSSSearch.SearchCompName(compname)
            mintCompID = mstGetFunctionValue.ExtraValue

            lstError.Items.Clear()
            'call
            If chkcalldesc.Checked = True Or chkcallowner.Checked = True Or chkcallSubject.Checked = True Or chkcalltype.Checked = True Then
                WSSCallSearch()
            Else
                cpnlcallsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlcallsearch.Enabled = False
                cpnlcallsearch.TitleCSS = "test2"
            End If

            'task
            If chktskdesc.Checked = True Or chktskowner.Checked = True Then
                WSSTaskSearch()
            Else
                cpnltasksearch.State = CustomControls.Web.PanelState.Collapsed
                cpnltasksearch.Enabled = False
                cpnltasksearch.TitleCSS = "test2"
            End If

            'action
            If chkactdesc.Checked = True Or ChkActOwn.Checked = True Then
                WSSActionSearch()
            Else
                cpnlactionsearch.State = CustomControls.Web.PanelState.Collapsed
                cpnlactionsearch.Enabled = False
                cpnlactionsearch.TitleCSS = "test2"
            End If

            'comment
            If chkcallcomm.Checked = True Or chktskcomm.Checked = True Or chkactcomm.Checked = True Then
                WssCommentSearch()
            Else
                cpnlcomm.State = CustomControls.Web.PanelState.Collapsed
                cpnlcomm.Enabled = False
                cpnlcomm.TitleCSS = "test2"
            End If

            'attachment
            If chkcallAtt.Checked = True Or chktskatt.Checked = True Or Chkactatt.Checked = True Then
                WssAttachmentSearch()
            Else
                cpnlattachment.State = CustomControls.Web.PanelState.Collapsed
                cpnlattachment.Enabled = False
                cpnlattachment.TitleCSS = "test2"

            End If

            'documents
            If Chkfilename.Checked = True Or ChkDesc.Checked = True Then
                WSSDocumentSearch()
            Else
                cpnldocument.State = CustomControls.Web.PanelState.Collapsed
                cpnldocument.Enabled = False
                cpnldocument.TitleCSS = "test2"

            End If
            'End If
        Catch ex As Exception
            CreateLog("Advance-Seacrh", "BindDataGrids-1074", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try

    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
  
End Class
