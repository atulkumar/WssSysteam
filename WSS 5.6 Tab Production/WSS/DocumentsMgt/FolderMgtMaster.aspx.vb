#Region "Namespaces"
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports System.Text
Imports Microsoft.Win32
Imports ION.Net
Imports System.Web.Security
Imports System.Security.Cryptography
Imports System.Drawing
Imports System.Data
Imports System.Configuration
Imports System.Data.Common
Imports System.IO
#End Region

'Session("PermissionDownLoad")
'Session("FolderID")
'Session("ParentFolderID")
'Session("intFolderId")
'Session("PropUserID")
'Session("Path")
'Session("PropRole")
'Session("CompanyId")
'Session("SelectedCompanyName")

Partial Class DocumentsMgt_FolderMgtMaster
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    End Sub
    Protected WithEvents cpnlItemList As CustomControls.Web.CollapsiblePanel
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "private variables for class"
    Private dsFolderMaster As New DataSet
    Private dsTempFolderMaster As New DataSet
    Private txthiddenImage As String 'stored clicked button's caption
    Private arrTextBox As New ArrayList
    Private arrColumns As New ArrayList
    Private dsFilesInfo As New DataSet
    Private Flag As Short
    Private IsAdminRights As Boolean
#End Region

#Region "Page Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        Try


            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################

            txthiddenImage = Request.Form("txthiddenImage")
            imgPermission.Attributes.Add("Onclick", "return SaveEdit('Permission');")
            imgEditFolder.Attributes.Add("Onclick", "return SaveEdit('EditFolder');")
            imgAdd.Attributes.Add("Onclick", "return SaveEdit('AddFolder');")
            imgAddFiles.Attributes.Add("Onclick", "return SaveEdit('AddFile');")
            imgEditFile.Attributes.Add("Onclick", "return SaveEdit('EditFile');")

            imgDeleteFolder.Attributes.Add("Onclick", "return SaveEdit('DeleteFolder');")
            imgDeleteFile.Attributes.Add("Onclick", "return SaveEdit('DeleteFile');")

            GetTextboxs() 'getting login role having admin rights or not
            Session("PermissionDownLoad") = 0
            Call GetIsAdmin()

            If Not IsPostBack Then
                Session("FolderID") = ""
                Session("FolderID") = 0
                Session("ParentFolderID") = 0
                ViewState("IsMnuCompany") = 1
                txtIsComp.Value = 1
                GetDataSetFromFolderMaster(mobjTreeMenu)
                DisabledButtons()
            End If
            'Changed from view state to Session(intFolderId) coz its not working by viewstate
            '**saurabh**
            If Not Val(Session("intFolderId")) = 0 Or Not Val(Session("intFolderId")) = 0 Then
                'GetFolderPermision()
                If Val(ViewState("intFolderUserId")) = Val(Session("PropUserID")) Or IsAdminRights = True Then
                    Session("PermissionDownLoad") = 1
                    EnabledButtons()
                Else
                    GetFolderPermision()
                End If
                GetFilesData()
            End If
            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                    Case "DeleteFolder"
                        If Val(Session("intFolderId")) > 0 Then
                            If ViewState("FolderEmpty") = True Then
                                If DeleteFolder() = True Then
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Folder Deleted Successfully ...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                                    GetDataSetFromFolderMaster(mobjTreeMenu)     ' Rebind tree menu after delete the folder
                                    Session("intFolderId") = Nothing
                                    Session("FolderID") = Nothing
                                    ViewState("FolderEmpty") = Nothing
                                    txtIsFolder.value = ""
                                Else
                                    lstError.Items.Clear()
                                    lstError.Items.Add("Error Occur while deleting ...")
                                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                                End If
                            Else
                                lstError.Items.Clear()
                                lstError.Items.Add("Folder is not empty so you can not delete it ...")
                                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                                Flag = 1
                            End If
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Please select folder for delete ...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        End If
                    Case "DeleteFile"
                        ViewState("intFileId") = Val(Request.Form("txtFileID"))
                        If DeleteFile() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("File Deleted Successfully ...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                            GetParentFolderStatus()
                            GetFilesData()    ' Rebind folder's file data after delete the one file 
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Error Occur ...")
                            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                        End If
                End Select
            End If
            Dim intID1 As Integer = Request.QueryString("ID")
            If intID1 = 1 Then
                Session("Path") = Nothing
            Else
                If Session("Path") <> Nothing Then
                    cpnlFilesList.Text = "FileList  (" & Session("Path").ToString & ")"
                End If
            End If

            'Security Block
            Dim intID As Int32
            If Not IsPostBack Then
                intID = 970
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intID) = False Then
                    Response.Redirect("../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intID)
            End If
            'End of Security Block

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "Page_Load-53", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "Get Role IsAdmin or not"
    Private Sub GetIsAdmin()
        Dim strSQL As String = "select ROM_CH1_IsAdminRight from T070031 where ROM_IN4_Role_ID_PK =" & Session("PropRole") & ""
        IsAdminRights = SQL.Search("", "", strSQL)
    End Sub
#End Region

#Region "GetDataSetFromFolderMaster"
    Public Sub GetDataSetFromFolderMaster(ByVal mobjTreeMenu As System.Web.UI.WebControls.TreeView)
        Try
            Dim sqlQuery As String = "select FD_NU9_Folder_ID_PK,FD_VC255_Folder_Name,FD_NU9_Parent_Folder_ID_FK as ParentID,FD_NU9_Company_ID_FK as CompanyID  from T250021 where FD_NU9_Company_ID_FK in (" & GetCompanySubQuery() & ")"
            If SQL.Search("T240031", "", "", sqlQuery, dsFolderMaster, "", "") = True Then
            End If

            CreateParentTreeMenu(mobjTreeMenu)

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetDataSetFromFolderMaster-65", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "CreateParentTreeMenu"
    '*******************************************************************
    ' Function             :-  CreateParentTreeMenu
    ' Purpose              :- Creating menu(companies) where user having access table is T060041,T010011 and pass parent Id for folders  
    '								
    ' Date					  Author						Modification Date					Description
    ' 10/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function CreateParentTreeMenu(ByVal mobjTreeMenu As System.Web.UI.WebControls.TreeView) As Boolean

        Try

            mobjTreeMenu.Nodes.Clear()
            Dim trParentNode As New System.Web.UI.WebControls.TreeNode
            dsTempFolderMaster = dsFolderMaster
            Dim sqlQuery As String
            Dim dsData As New DataSet
          
            mobjTreeMenu.ShowLines = True

            sqlQuery = "(select UC_NU9_Comp_ID_FK ID,CI_VC36_Name as Name from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1 ) "

            If SQL.Search("T060041", "", "", sqlQuery, dsData, "", "") = True Then
                For intI As Integer = 0 To dsData.Tables(0).Rows.Count - 1
                    trParentNode = New System.Web.UI.WebControls.TreeNode
                    trParentNode.Text = dsData.Tables(0).Rows(intI).Item("Name")
                    trParentNode.Value = dsData.Tables(0).Rows(intI).Item("ID").ToString & "C"
                    'trParentNode.ID = dsData.Tables(0).Rows(intI).Item("ID").ToString & "C"
                    If intI = 0 Then
                        Session("CompanyId") = dsData.Tables(0).Rows(intI).Item("ID")
                        Session("FolderPath") = dsData.Tables(0).Rows(intI).Item("Name") + "/"
                        Session("SelectedCompanyName") = dsData.Tables(0).Rows(intI).Item("Name")
                    End If
                    trParentNode.Text = "<font Size=""1"" color=""Black""   face=""verdana""><b>" & dsData.Tables(0).Rows(intI).Item("Name") & "</b></font>"
                    AddRootChildTreeMenu(trParentNode, dsData.Tables(0).Rows(intI).Item("ID"), dsTempFolderMaster.Tables(0).DefaultView)
                    trParentNode.ImageUrl = "../images/Comp1.jpg"
                    mobjTreeMenu.Nodes.Add(trParentNode)
                Next
                mobjTreeMenu.NodeIndent = 4
            End If
            trParentNode.Expanded = True
            trParentNode.SelectAction = TreeNodeSelectAction.Expand
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "CreateParentTreeMenu-99", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
#End Region

#Region "AddRootChildTreeMenu"
    '*******************************************************************
    ' Function             :-  AddRootChildTreeMenu
    ' Purpose              :-  Creating Sub menus under the Companies Menu  
    '								
    ' Date					  Author						Modification Date					Description
    ' 10/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub AddRootChildTreeMenu(ByRef trvParentNode As System.Web.UI.WebControls.TreeNode, ByVal CompanyID As Integer, ByVal dvChildsMain As DataView)

        Dim trvChildNode As New System.Web.UI.WebControls.TreeNode
        Dim dvLast As DataView
        Dim intL As Integer

        Try
            dvLast = GetFilteredDataView(dvChildsMain, "CompanyID=" & CompanyID & " and ParentID=0")
            For intL = 0 To dvLast.Table.Rows.Count - 1
                trvChildNode = New System.Web.UI.WebControls.TreeNode
                trvChildNode.Text = "<font Size=""1"" face=""verdana"" color=""Black"">" & dvLast.Table.Rows(intL).Item("FD_VC255_Folder_Name") & "</font>"
                trvChildNode.Value = dvLast.Table.Rows(intL).Item("FD_NU9_Folder_ID_PK")
                AddsChildTreeMenu(trvChildNode, dvLast.Table.Rows(intL).Item("FD_NU9_Folder_ID_PK"), dsTempFolderMaster.Tables(0).DefaultView)

                trvChildNode.ImageUrl = "../images/Closed_folder.jpg"
                trvParentNode.ChildNodes.Add(trvChildNode)
            Next
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "AddsChildTreeMenu-121", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "AddsChildTreeMenu"
    '*******************************************************************
    ' Function             :-  AddsChildTreeMenu
    ' Purpose              :-  Creating recursive folders under the folders    
    '								
    ' Date					  Author						Modification Date					Description
    ' 10/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub AddsChildTreeMenu(ByRef trvParentNode As System.Web.UI.WebControls.TreeNode, ByVal ParentID As Integer, ByVal dvChildsMain As DataView)

        Dim trvChildNode As New System.Web.UI.WebControls.TreeNode
        Dim dvLast As DataView
        Dim intL As Integer

        Try

            dvLast = GetFilteredDataView(dvChildsMain, "ParentID=" & ParentID)
            For intL = 0 To dvLast.Table.Rows.Count - 1
                trvChildNode = New System.Web.UI.WebControls.TreeNode
                trvChildNode.Text = "<font Size=""1"" face=""verdana"" color=""Black"">" & dvLast.Table.Rows(intL).Item("FD_VC255_Folder_Name") & "</font>"
                trvChildNode.Value = dvLast.Table.Rows(intL).Item("FD_NU9_Folder_ID_PK")
                Dim dvTemp As New DataView
                dvTemp = GetFilteredDataView(dsFolderMaster.Tables(0).DefaultView, "ParentID =" & dvLast.Table.Rows(intL).Item("FD_NU9_Folder_ID_PK"))
                AddsChildTreeMenu(trvChildNode, dvLast.Table.Rows(intL).Item("FD_NU9_Folder_ID_PK"), dsTempFolderMaster.Tables(0).DefaultView)
                trvChildNode.ImageUrl = "../images/Closed_folder.jpg"
                trvParentNode.ChildNodes.Add(trvChildNode)
            Next
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "AddsChildTreeMenu-121", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "GetFilesData from database"
    '*******************************************************************
    ' Function             :-  GetFilesData
    ' Purpose              :- Geting data from data base based on folder Id. table is T250011,T010011,T060011
    '								
    ' Date				   Author						Modification Date					Description
    ' 10/Feb/08			   Sachin Prashar       -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GetFilesData()

        Dim sqlQuery As String = String.Empty
        Dim tempDS As New DataSet

        Try
            sqlQuery = "select FI_NU9_File_ID_PK  as FileID,FI_VC255_File_URL as FilePath,CI_VC36_Name as Company,FI_VC255_File_Name as fileName,FI_FL8_File_Version as Version,FI_VC500_File_Description as Description ,UM_VC50_UserID as UploadedBy ,convert(varchar,FI_DT8_Upload_ON,101) as UploadedOn   from T250011,T010011,T060011 where  CI_NU8_Address_Number=FI_NU9_Company_ID_FK and FI_NU9_UploadBy_ID_FK=UM_IN4_Address_No_FK and  FI_NU9_Folder_ID_FK =" & Val(Session("FolderID")) & ""
            SQL.Search("T250011", "", "", sqlQuery, tempDS, "", "")

            dsFilesInfo = tempDS

            Dim strTemp As String = GetFilter()

            Dim htDateCols As New Hashtable
            htDateCols.Add("UploadedOn", 2)

            Dim htGrdColumns As New Hashtable
            htGrdColumns.Add("Description", 33)


            If strTemp.Equals("") = False Then
                Dim dtTemp As New DataTable
                dtTemp = GetFilteredDataView(dsFilesInfo.Tables(0).DefaultView, strTemp).Table
                HTMLEncodeDecode(mdlMain.Action.Encode, dtTemp, htGrdColumns)
                SetDataTableDateFormat(dtTemp, htDateCols)

                grdFiles.DataSource = dtTemp.DefaultView
                grdFiles.DataBind()
            Else
                HTMLEncodeDecode(mdlMain.Action.Encode, dsFilesInfo.Tables(0), htGrdColumns)
                SetDataTableDateFormat(dsFilesInfo.Tables(0), htDateCols)
                grdFiles.DataSource = dsFilesInfo
                grdFiles.DataBind()
            End If
            If ViewState("FolderEmpty") = True Then ' -- check for files if no folder exist
                If dsFilesInfo.Tables(0).Rows.Count > 0 Then
                    ViewState("FolderEmpty") = False '-- set  viewstate to false if file exist
                    txtIsFolder.Value = 0
                End If
            End If

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetFilesData-285", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "Set textbox header for search functionality on grid"

    '*******************************************************************
    ' Function             :-  GetTextboxs
    ' Purpose              :- setting grid textboxes id into the Array list
    '								
    ' Date					    Author						Modification Date					Description
    ' 1/Feb/08			        Sachin Prashar              -------------------	                Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GetTextboxs()

        Try
            arrTextBox.Clear()
            arrTextBox.Add("txtFileID_H")
            arrTextBox.Add("txtCompany_H")
            arrTextBox.Add("txtFileName_H")
            arrTextBox.Add("txtDescription_H")
            arrTextBox.Add("txtUploadedOn_H")
            arrTextBox.Add("txtUploadedBy_H")
            arrTextBox.Add("txtVersion_H")

            arrColumns.Clear()
            arrColumns.Add("fileID")
            arrColumns.Add("company")
            arrColumns.Add("FileName")
            arrColumns.Add("Description")
            arrColumns.Add("UploadedOn")
            arrColumns.Add("UploadedBy")
            arrColumns.Add("Version")

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetTextboxs-687", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "Create filter string base on textbox data"
    '*******************************************************************
    ' Function             :-  GetFilter
    ' Purpose              :- Checking the grid textbox array its fill or not if filled then filter the data accordingly
    '								
    ' Date					  Author						Modification Date					Description
    ' 4/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function GetFilter() As String

        Try
            Dim strFilter As String = ""
            For intI As Integer = 0 To arrTextBox.Count - 1
                Dim strTemp As String = Request.Form("cpnlGrdView$cpnlFilesList$grdFiles$ctl01$" & arrTextBox(intI))
                If Not IsNothing(strTemp) Then
                    If strTemp.Trim.Equals("") = False Then
                        Select Case dsFilesInfo.Tables(0).Columns(arrColumns(intI)).DataType.FullName
                            Case "System.String"
                                If strTemp.Contains("*") = True Then
                                    strTemp = strTemp.Replace("*", "%")
                                Else
                                    strTemp &= "%"
                                End If
                                strFilter &= dsFilesInfo.Tables(0).Columns(arrColumns(intI)).ColumnName & " like '" & strTemp & "' AND "
                            Case Else
                                If IsNumeric(strTemp) = False Then
                                    strTemp = "-9999999999"
                                End If
                                strFilter &= dsFilesInfo.Tables(0).Columns(arrColumns(intI)).ColumnName & "=" & strTemp & " AND "
                        End Select
                    End If
                End If
            Next
            If strFilter.Equals("") = False Then
                strFilter = strFilter.Remove(strFilter.Length - 4, 4)
            End If
            strFilter = strFilter.Replace("*", "%")

            Return strFilter
        Catch ex As Exception
            Return ""
            CreateLog("FolderMgtMaster", "GetFilter-236", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
#End Region

#Region "dgvFiles Events"
    '*******************************************************************
    ' Function             :-  grdFiles_ItemDataBound
    ' Purpose              :- Fatching grid header textboxes value and set the permission on file down load if session is 0
    '								
    ' Date					  Author						Modification Date					Description
    ' 4/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub grdFiles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdFiles.ItemDataBound

        Try

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                e.Item.Attributes.Add("OnClick", "SClick('" & Val(CType(e.Item.Cells(0).FindControl("lblFileID"), Label).Text.Trim) & "', " & e.Item.ItemIndex + 1 & ")")
                'e.Item.Attributes.Add("OnDblClick", "DClick('" & Val(CType(e.Item.Cells(0).FindControl("lblFileID"), Label).Text.Trim) & "')")
                If Session("PermissionDownLoad") = 0 Then
                    CType(e.Item.Cells(2).FindControl("systemLink"), LinkButton).Enabled = False ' disabled download
                End If
            End If

            If e.Item.ItemType = ListItemType.Header Then
                For intI As Integer = 0 To arrTextBox.Count - 1
                    CType(e.Item.Cells(intI).FindControl(arrTextBox(intI)), TextBox).Text = Request.Form("cpnlGrdView$cpnlFilesList$grdFiles$ctl01$" & arrTextBox(intI)) 'restore grid textbox data if page is post back
                Next
            End If

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "grdFiles_ItemDataBound-239", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub

    Private Sub grdFiles_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdFiles.ItemCommand

        Try
            If Flag = 1 Then
            Else
                'Dim strFileName As String = Server.UrlEncode(e.CommandName)
                Dim strFileName As String = e.CommandName
                'Dim strFilePath As String = Server.UrlEncode(Server.MapPath("../") & e.CommandArgument.ToString())
                Dim strFilePath As String = Server.MapPath("../") & e.CommandArgument.ToString()
                'Response.Redirect("Handler.ashx?FileName=" & strFileName & "&FilePath=" & strFilePath, False)
                'Response.Write("<script>window.open('Handler.ashx?');</script>")
                'ScriptManager.RegisterClientScriptBlock(Page, System.Type.GetType("System.String"), "tt", "<script>window.open('Handler.ashx?FileName=" & strFileName & "&FilePath=" & strFilePath + "','hgyg','width=10px,height=10px');</script>", False)

                'Dim strFileName As String = e.CommandName
                'Dim strFilePath As String = Server.MapPath("../") & e.CommandArgument.ToString()
                If File.Exists(strFilePath) Then
                    Dim strmFile As Stream = File.OpenRead(strFilePath)
                    Dim buffer(strmFile.Length) As Byte

                    strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))

                    Response.ClearHeaders()
                    Response.ClearContent()
                    Response.ContentType = "application/octet-stream"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
                    Response.BinaryWrite(buffer)

                    'Response.Flush()
                    'Response.End()
                Else
                    Context.Response.Write("<script>alert('File Not Found');</script>")
                End If


            End If
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "grdFiles_ItemCommand-235", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try

    End Sub

#End Region

#Region "GetNodepath"
    Private Function GetNodepath(ByVal tn As System.Web.UI.WebControls.TreeNode) As String
        Dim arrNodeName As New ArrayList
        Dim strPath As String = String.Empty
        Dim intI As Int32

        Try

            GetParentName(tn, arrNodeName)
            For intI = arrNodeName.Count - 1 To 0 Step -1
                strPath = strPath + arrNodeName(intI) + "/"
            Next
            If arrNodeName.Count = 0 Then
                Session("SelectedCompanyName") = tn.Text
            Else
                Session("SelectedCompanyName") = arrNodeName(arrNodeName.Count - 1)
            End If
            'do while parentName=GetParentName
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetNodepath-423", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
        Return strPath
    End Function
#End Region

#Region "GetParentName on menu click"
    Private Sub GetParentName(ByVal tn As Object, ByRef arrNodeName As ArrayList)

        Try

            Dim ParentNode As Object
            If IsNothing(tn) = True Then
                Exit Sub
            Else
                ParentNode = tn.parent

                If ParentNode.GetType.Name = Nothing Then
                    If ParentNode.GetType.Name = "TreeNode" Then
                        arrNodeName.Add(ParentNode.text)
                        GetParentName(ParentNode, arrNodeName)
                        'Return ParentNode
                    Else
                        'Return Nothing
                    End If
                End If
            End If
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetParentName-445", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "GetFolderPermision"
    '*******************************************************************
    ' Function             :-  GetFolderPermision
    ' Purpose              :- Faching permission from databse based on folder , role and company bases and call enable disable function 
    '								
    ' Date					  Author						Modification Date					Description
    ' 7/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub GetFolderPermision()

        Try
            Dim strsqlQuery As String = String.Empty
            Dim dsChkAccess As New DataSet
            Dim dsChkPermission As New DataSet
            Dim intPermissionID As Integer
            Dim htPermissionCols As New Hashtable
            Dim intI As Integer

            htPermissionCols.Clear()

            strsqlQuery = " Select PM_NU9_Permission_ID_PK,isnull(PM_BT1_Have_Access,0) PM_BT1_Have_Access  from T250041 where PM_NU9_Folder_ID_FK=" & Session("FolderID") & " and PM_IN4_PermissionTo_Role_ID_FK=" & Session("PropRole") & ""

            If SQL.Search("T250041", "", "", strsqlQuery, dsChkAccess, "", "") = True Then
                If dsChkAccess.Tables(0).Rows(0).Item("PM_BT1_Have_Access") = True Then
                    intPermissionID = dsChkAccess.Tables(0).Rows(0).Item("PM_NU9_Permission_ID_PK")
                    Dim strPermissionQuery As String = "Select PD_NU9_Permission_Type_ID_FK,PT_VC100_Permission_Name,PD_BT1_Is_Allowed from T250042,T250031 where   PT_NU9_Permission_Type_ID_PK=PD_NU9_Permission_Type_ID_FK and PD_NU9_Permission_ID_FK=" & intPermissionID & ""
                    If SQL.Search("T240042", "", "", strPermissionQuery, dsChkPermission, "", "") = True Then
                        For intI = 0 To dsChkPermission.Tables(0).Rows.Count - 1
                            htPermissionCols.Add(CStr(dsChkPermission.Tables(0).Rows(intI).Item("PT_VC100_Permission_Name")), CStr(dsChkPermission.Tables(0).Rows(intI).Item("PD_BT1_Is_Allowed")))
                        Next
                    End If
                Else
                    DisabledButtons() ' If don't have access 
                End If

            Else
                strsqlQuery = " Select PM_NU9_Permission_ID_PK,isnull(PM_BT1_Have_Access,0) PM_BT1_Have_Access from T250041 where PM_NU9_Folder_ID_FK=" & Session("FolderID") & " and PM_NU9_PermissionTo_Company_ID_FK=" & Session("PropCompanyID") & ""

                If SQL.Search("T250041", "", "", strsqlQuery, dsChkAccess, "", "") = True Then
                    If CStr(dsChkAccess.Tables(0).Rows(0).Item("PM_BT1_Have_Access")) = True Then
                        intPermissionID = dsChkAccess.Tables(0).Rows(0).Item("PM_NU9_Permission_ID_PK")
                        Dim strPermissionQuery As String = "Select PD_NU9_Permission_Type_ID_FK,PT_VC100_Permission_Name,PD_BT1_Is_Allowed from T250042,T250031 where   PT_NU9_Permission_Type_ID_PK=PD_NU9_Permission_Type_ID_FK and PD_NU9_Permission_ID_FK=" & intPermissionID & ""
                        If SQL.Search("T240042", "", "", strPermissionQuery, dsChkPermission, "", "") = True Then
                            For intI = 0 To dsChkPermission.Tables(0).Rows.Count - 1
                                htPermissionCols.Add(CStr(dsChkPermission.Tables(0).Rows(intI).Item("PT_VC100_Permission_Name")), CStr(dsChkPermission.Tables(0).Rows(intI).Item("PD_BT1_Is_Allowed")))
                            Next
                        End If
                    Else
                        DisabledButtons()
                    End If
                Else
                    DisabledButtons()
                End If
            End If
            SetPermissions(htPermissionCols)
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetFolderPermision-445", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try

    End Sub
#End Region

#Region "SetPermissions on Buttons and download files"
    '*******************************************************************
    ' Function             :-  SetPermissions
    ' Purpose              :- setting access based on permissioin on the buttons enabled and disabled
    '								
    ' Date					  Author						Modification Date					Description
    ' 7/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub SetPermissions(ByVal htPermissionCols As Hashtable)

        Try

            Dim en As IDictionaryEnumerator
            en = htPermissionCols.GetEnumerator

            While en.MoveNext

                Select Case en.Key

                    Case "Add Folder"
                        If en.Value = True Then
                            imgAdd.Enabled = True
                            imgAdd.ImageUrl = "../Images/Addfoleders.jpg"
                        Else
                            imgAdd.ImageUrl = "../Images/Addfoleders1.jpg"
                            imgAdd.Enabled = False
                        End If

                    Case "Edit Folder Details"
                        If en.Value = True Then
                            imgEditFolder.Enabled = True
                            imgEditFolder.ImageUrl = "../images/Editfolder.jpg"
                        Else
                            imgEditFolder.ImageUrl = "../images/Editfolder1.gif"
                            imgEditFolder.Enabled = False
                        End If

                    Case "Add File"
                        If en.Value = True Then
                            imgAddFiles.Enabled = True
                            imgAddFiles.ImageUrl = "../images/Addfiles.jpg"
                        Else
                            imgAddFiles.ImageUrl = "../images/Addfiles1.jpg"
                            imgAddFiles.Enabled = False
                        End If

                    Case "Edit File Details"
                        If en.Value = True Then
                            imgEditFile.ImageUrl = "../images/editfiles.jpg"
                            imgEditFile.Enabled = True
                        Else
                            imgEditFile.ImageUrl = "../images/editfiles1.jpg"
                            imgEditFile.Enabled = False
                        End If

                    Case "Delete Files"
                        If en.Value = True Then
                            imgDeleteFile.Enabled = True
                            imgDeleteFile.ImageUrl = "../Images/Deletefiles.jpg"
                        Else
                            imgDeleteFile.ImageUrl = "../Images/Deletefiles1.jpg"
                            imgDeleteFile.Enabled = False
                        End If

                    Case "Delete Folders"
                        If en.Value = True Then
                            imgDeleteFolder.Enabled = True
                            imgDeleteFolder.ImageUrl = "../Images/s2delete01.gif"
                        Else
                            imgDeleteFolder.ImageUrl = "../Images/s2delete011.gif"
                            imgDeleteFolder.Enabled = False
                        End If

                    Case "Download Files"
                        If en.Value = True Then
                            Session("PermissionDownLoad") = 1
                        Else
                            Session("PermissionDownLoad") = 0
                        End If
                    Case "Set Folder Permission"
                        If en.Value = True Then
                            imgPermission.ImageUrl = "../images/q1.jpg"
                            imgPermission.Enabled = True
                        Else
                            imgPermission.ImageUrl = "../images/q31.jpg"
                            imgPermission.Enabled = False
                        End If
                End Select

            End While

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "SetPermissions-571", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
#End Region

#Region "DisabledButtons and EnabledButtons"
    '*******************************************************************
    ' Function             :-  DisabledButtons
    ' Purpose              :-this will disabled all the buttion on the page 
    '								
    ' Date					  Author						Modification Date					Description
    ' 7/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub DisabledButtons()

        Try

            If ViewState("IsMnuCompany") = 1 Then
                imgAdd.Enabled = True
                imgAdd.ImageUrl = "../Images/Addfoleders.jpg"
            Else
                imgAdd.ImageUrl = "../Images/Addfoleders1.jpg"
                imgAdd.Enabled = False
            End If

            imgEditFolder.ImageUrl = "../images/Editfolder1.gif"
            imgEditFolder.Enabled = False

            imgAddFiles.ImageUrl = "../images/Addfiles1.jpg"
            imgAddFiles.Enabled = False

            imgEditFile.ImageUrl = "../images/editfiles1.jpg"
            imgEditFile.Enabled = False

            imgPermission.ImageUrl = "../images/q31.jpg"
            imgPermission.Enabled = False

            imgDeleteFile.ImageUrl = "../Images/Deletefiles1.jpg"
            imgDeleteFile.Enabled = False

            imgDeleteFolder.ImageUrl = "../Images/s2delete011.gif"
            imgDeleteFolder.Enabled = False

            Session("PermissionDownLoad") = 0

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "DisabledButtons-604", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try

    End Sub
    '*******************************************************************
    ' Function             :-  EnabledButtons
    ' Purpose              :-this will Enabled all the buttion on the page 
    '								
    ' Date					  Author						Modification Date					Description
    ' 7/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Sub EnabledButtons()

        Try

            imgAdd.Enabled = True
            imgAdd.ImageUrl = "../Images/Addfoleders.jpg"

            imgEditFolder.Enabled = True
            imgEditFolder.ImageUrl = "../images/Editfolder.jpg"

            imgAddFiles.Enabled = True
            imgAddFiles.ImageUrl = "../images/Addfiles.jpg"

            imgEditFile.ImageUrl = "../images/editfiles.jpg"
            imgEditFile.Enabled = True

            imgPermission.ImageUrl = "../images/q1.jpg"
            imgPermission.Enabled = True

            imgDeleteFile.ImageUrl = "../Images/Deletefiles.jpg"
            imgDeleteFile.Enabled = True

            imgDeleteFolder.ImageUrl = "../Images/s2delete01.gif"
            imgDeleteFolder.Enabled = True

            Session("PermissionDownLoad") = 1

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "EnabledButtons-604", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub
    '*******************************************************************
    ' Function             :-  grdFiles_ItemCommand
    ' Purpose              :-Downlaoding the file on file click
    '								
    ' Date					  Author						Modification Date					Description
    ' 15/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
#End Region

#Region "Delete Files and Folder functions"
    '*******************************************************************
    ' Function             :-  DeleteFolder
    ' Purpose              :-Deleting Folder from T250021
    '								
    ' Date					  Author						Modification Date					Description
    ' 15/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function DeleteFolder() As Boolean

        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim strSQLQuery As String = "delete  from T250042 where PD_NU9_Permission_ID_FK in(select PM_NU9_Permission_ID_PK from T250041 where PM_NU9_Folder_ID_FK=" & Val(Session("intFolderId")) & ");Delete from T250041 where PM_NU9_Folder_ID_FK=" & Val(Session("intFolderId")) & ";Delete  from T250021 where FD_NU9_Folder_ID_PK=" & Val(Session("intFolderId")) & ""

            If SQL.Delete("FolderMgtMaster", "DeleteFolder-714", strSQLQuery, SQL.Transaction.Serializable) = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "DeleteFolder-721", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try
    End Function
    '*******************************************************************
    ' Function             :-  DeleteFile
    ' Purpose              :-Deleting Files from T250011
    '								
    ' Date					  Author						Modification Date					Description
    ' 15/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function DeleteFile() As Boolean

        Try

            GetFilePath() ' return file path which coming for delete to delete physicaly from folder

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim strSQLQuery As String = "Delete  from T250011 where FI_NU9_File_ID_PK=" & Val(ViewState("intFileId")) & ""
            If SQL.Delete("FolderMgtMaster", "DeleteFile-749", strSQLQuery, SQL.Transaction.Serializable) = True Then

                Try
                    File.Delete(Server.MapPath("../") & Session("ImgPath")) 'file remove from folder
                Catch ex As Exception
                End Try

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "DeleteFile-756", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try

    End Function
    '*******************************************************************
    ' Function             :-  GetFilePath
    ' Purpose              :-it will return file path based on file Id
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
#End Region

#Region "GetFilePath"
    Private Function GetFilePath() As Boolean


        Dim strSQLQuery As String = "select  FI_VC255_File_URL  from T250011 where FI_NU9_File_ID_PK=" & Val(ViewState("intFileId")) & ""
        Dim ds As New DataSet

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        If SQL.Search("T250011", "", "", strSQLQuery, ds, "", "") = True Then
            Session("ImgPath") = ds.Tables(0).Rows(0).Item("FI_VC255_File_URL")
        End If

    End Function
#End Region

#Region "GetParentFolderStatus"
    '*******************************************************************
    ' Function             :-  GetParentFolderStatus
    ' Purpose              :-Chking parent Id from database to chk the existance of childs node table T250021
    '								
    ' Date					  Author						Modification Date					Description
    ' 25/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Private Function GetParentFolderStatus() As Boolean

        Try

            Dim intCount As Integer
            Dim strSQLQuery As String = "Select FD_NU9_Parent_Folder_ID_FK from T250021 where FD_NU9_Parent_Folder_ID_FK=" & Session("FolderID") & ""

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("T250021", "", strSQLQuery, intCount, "") = True Then
                ViewState("FolderEmpty") = False
                txtIsFolder.Value = 0
                Return True
            Else
                ViewState("FolderEmpty") = True
                txtIsFolder.Value = 1
                Return False
            End If

        Catch ex As Exception
            CreateLog("FolderMgtMaster", "GetParentFolderStatus-921", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try

    End Function
#End Region

#Region "mobjTreeMenu Events"
    '*******************************************************************
    ' Function             :-  mobjTreeMenu_SelectedIndexChange
    ' Purpose              :-  selecting folder id on menu change event and set the other things also like getting file info and and              '''''''''''''''''''''''''''''''''''''''''''''rechecked the permissions also
    '								
    ' Date					  Author						Modification Date					Description
    ' 10/Feb/08			   Sachin Prashar           -------------------	Created
    ' Notes: 
    ' Code:
    '*******************************************************************
    Protected Sub mobjTreeMenu_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjTreeMenu.SelectedNodeChanged

        Try

            Dim Tn As System.Web.UI.WebControls.TreeNode
            Dim intFolderId As Integer
            ViewState("AddStatus") = 0
            Session.Add("intFolderId", intFolderId)
            Tn = mobjTreeMenu.SelectedNode
            'Tn = mobjTreeMenu.GetNodeFromIndex(e.NewNode)
            Dim strFolderPath As String

            strFolderPath = GetNodepath(Tn)

            Session("FolderPath") = strFolderPath
            If Session("FolderPath") = Nothing Then
                Session("FolderPath") = Tn.Text + "/"
            End If
            strFolderPath = GetNodepath(Tn) + Tn.Text + "/"
            cpnlFilesList.Text = "FileList  (" & strFolderPath & ")"
            intFolderId = Val(Tn.Value)
            Session("intFolderId") = intFolderId
            'to mentain path when updations or adding folder to tree menu i.e putting folder path in seesion
            '****saurabh****
            Session.Add("Path", strFolderPath)

            '****************************************
            If Tn.Value.IndexOf("C") > -1 Then ' -- set folder id to 0 if seslected node is company
                Session("FolderID") = 0
                Session("ParentFolderID") = 0
                ViewState("IsMnuCompany") = 1
                txtIsComp.Value = 1
            Else
                ViewState("IsMnuCompany") = 0
                txtIsComp.Value = 0
                Session("FolderID") = Tn.Value
                Session("ParentFolderID") = IIf(Tn.Parent.Value.IndexOf("C") > -1, 0, Tn.Parent.Value)
            End If
            '*******************************************
            ' -- check for child
            If Tn.ChildNodes.Count = 0 Then
                ViewState("FolderEmpty") = True ' -- set viewstate to true if no child exist
                txtIsFolder.value = 1
            Else
                ViewState("FolderEmpty") = False
                txtIsFolder.value = 0
            End If

            '*********************************************************
            Dim sqlQuery As String = String.Empty
            Dim dsFolderUser As New DataSet
            'Dim intFolderUserId As Integer
            sqlQuery = " select FD_NU9_CreatedBy_ID_FK from T250021 where FD_NU9_Folder_ID_PK=" & Val(Session("FolderID")) & " "
            If SQL.Search("T250021", "", "", sqlQuery, dsFolderUser, "", "") = True Then
                ' ViewState("intFolderUserId") = dsFolderUser.Tables(0).Rows(0).Item("FD_NU9_CreatedBy_ID_FK")
                ViewState("intFolderUserId") = IIf(IsDBNull(dsFolderUser.Tables(0).Rows(0).Item("FD_NU9_CreatedBy_ID_FK")), 0, dsFolderUser.Tables(0).Rows(0).Item("FD_NU9_CreatedBy_ID_FK"))
            End If

            If ViewState("IsMnuCompany") = 1 Then
                DisabledButtons()
            Else
                If Val(ViewState("intFolderUserId")) = Val(Session("PropUserID")) Or IsAdminRights = True Then
                    Session("PermissionDownLoad") = 1
                    EnabledButtons()
                Else
                    GetFolderPermision()
                End If
            End If
            '*********************************************************
            GetFilesData()
        Catch ex As Exception
            CreateLog("FolderMgtMaster", "mobjTreeMenu_SelectedIndexChange-328", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Sub

#End Region

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class
