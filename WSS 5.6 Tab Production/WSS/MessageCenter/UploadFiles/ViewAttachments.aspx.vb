'************************************************************************************************************
' Page                 :- View Attachment
' Purpose              :- Purpose of this screen is to list all the attachments associated with each call,                             Task & action, Attachment date, user Name etc.   

' Tables used          :- T010011, T010011, T040051
' Date				Author						Modification Date					Description
' 24/03/06			Ranvijay/Sachin			    -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.IO
Imports System.Data

Partial Class MessageCenter_UploadFiles_ViewAttachments
    Inherits System.Web.UI.Page

    Private mstrAttachLevel As String
    Private mTaskNo As Integer
    Private mActionNo As Integer
    Private mstrForm As String
    Private txthiddenImage As String 'stored clicked button's cation  
    Private strTable As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 288 'Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
            txtCSS(Me.Page)
        End If
        'End of Security Block

        If Not IsPostBack Then
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgDelete.Attributes.Add("Onclick", "return SaveEdit('Delete');")

        End If

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        mstrAttachLevel = Request.QueryString("ID")
        mTaskNo = Request.QueryString("TaskNo")
        mActionNo = Request.QueryString("ACTIONNO")
        mstrForm = Request.QueryString("OPT")
        'Put user code to initialize the page here
        lstError.Items.Clear()

        If mstrForm = "2" Then
            strTable = "T050051"
        Else
            strTable = "T040051"
        End If

        txthiddenImage = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Delete"
                    Call ReadGrids()
            End Select
        End If

        If Request.QueryString("CompID") <> "" Then
            If IsNumeric(Request.QueryString("CompID")) Then
                ViewState("CompanyID") = Request.QueryString("CompID")
            Else
                ViewState("CompanyID") = WSSSearch.SearchCompName(Request.QueryString("CompID")).ExtraValue
            End If
            ViewState("CallNo") = Request.QueryString("CallNo")
            'ViewState("CompanyID") = Request.QueryString("CompID")

            If mstrAttachLevel.Trim.ToUpper.Equals("AO") Then
                GetAttachments(ViewState("CompanyID"))
            Else
                GetAttachments(ViewState("CompanyID"))
            End If
        Else
            If mstrForm = "2" Then
                GetAttachments(ViewState("CompanyID"))
            Else
                GetAttachments(ViewState("CompanyID"))
            End If
        End If

    End Sub

    Private Sub GetAttachments(ByVal CompanyId As String)

        Dim dsAttachments As DataSet

        Try
            dsAttachments = New DataSet
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If mstrForm = "2" Then
                strTable = "T050051"
            Else
                strTable = "T040051"
            End If

            SQL.DBTracing = False
            Dim htDateCols As New Hashtable
            htDateCols.Add("VH_DT8_Date", 1)

            Select Case mstrAttachLevel
                Case "AL"
                    'Call
                    dsAttachments.Clear()
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-88", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name, convert(varchar,VH_DT8_Date,100) VH_DT8_Date, VH_NU9_Version,VH_DT8_Modify_Date,VH_NU9_Call_Number,VH_NU9_File_ID_PK  from " & strTable & ",T010011,T010011 comp  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and T010011.CI_NU8_Address_Number=" & strTable.Trim & ".VH_NU9_Address_Book_Number and VH_IN4_Level=1 and comp.CI_NU8_Address_Number=" & strTable & ".VH_NU9_CompId_Fk and  VH_VC4_Active_Status='ENB'  and  comp.CI_NU8_Address_Number=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then

                        grdAttachments.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdAttachments.DataSource = dsAttachments
                        grdAttachments.DataBind()
                    Else
                        grdAttachments.Visible = False
                        'cpnlErrorPanel.Visible = True
                        cPnlCallAtt.State = CustomControls.Web.PanelState.Collapsed
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlCallAtt.TitleCSS = "test2"
                        cPnlCallAtt.Visible = False

                        lstError.Items.Add("No attachments found for the call...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                    End If

                    'Task
                    dsAttachments.Clear()
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-103", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name, convert(varchar,VH_DT8_Date,100) VH_DT8_Date, VH_NU9_Version,VH_DT8_Modify_Date,VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_File_ID_PK from " & strTable & ",T010011,T010011 comp  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and T010011.CI_NU8_Address_Number=" & strTable.Trim & ".VH_NU9_Address_Book_Number and VH_IN4_Level=2  and comp.CI_NU8_Address_Number=" & strTable & ".VH_NU9_CompId_Fk and  VH_VC4_Active_Status='ENB' and comp.CI_NU8_Address_Number=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then
                        grdTask.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdTask.DataSource = dsAttachments
                        grdTask.DataBind()
                    Else
                        grdTask.Visible = False
                        cPnlTaskAtt.State = CustomControls.Web.PanelState.Collapsed
                        'cpnlErrorPanel.Visible = True
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlTaskAtt.TitleCSS = "test2"
                        cPnlTaskAtt.Visible = False

                        lstError.Items.Add("No attachments found for the Task ...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    End If

                    'Action
                    dsAttachments.Clear()
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-118", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name, convert(varchar,VH_DT8_Date,100) VH_DT8_Date,VH_NU9_Version,VH_DT8_Modify_Date,VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,VH_NU9_File_ID_PK from " & strTable & ",T010011,T010011 comp  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and T010011.CI_NU8_Address_Number=" & strTable.Trim & ".VH_NU9_Address_Book_Number and VH_IN4_Level=3 and comp.CI_NU8_Address_Number=" & strTable & ".VH_NU9_CompId_Fk and  VH_VC4_Active_Status='ENB' and comp.CI_NU8_Address_Number=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then
                        grdAction.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdAction.DataSource = dsAttachments
                        grdAction.DataBind()
                    Else
                        grdAction.Visible = False
                        cPnlActionAtt.State = CustomControls.Web.PanelState.Collapsed
                        'cpnlErrorPanel.Visible = True
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlActionAtt.TitleCSS = "test2"
                        cPnlActionAtt.Visible = False

                        lstError.Items.Add("No attachments found for the Actions...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)


                    End If

                Case "C"
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-132", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name, convert(varchar,VH_DT8_Date,100) VH_DT8_Date,VH_NU9_Version,VH_DT8_Modify_Date, VH_NU9_Call_Number,VH_NU9_File_ID_PK from " & strTable & ",T010011  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and T010011.CI_NU8_Address_Number=" & strTable.Trim & ".VH_NU9_Address_Book_Number and  VH_VC4_Active_Status='ENB' and VH_IN4_Level=1 and " & strTable & ".VH_NU9_CompId_Fk=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then
                        grdAttachments.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdAttachments.DataSource = dsAttachments
                        grdAttachments.DataBind()
                    Else
                        grdAttachments.Visible = False
                        'cpnlErrorPanel.Visible = True
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlCallAtt.TitleCSS = "test2"
                        cPnlCallAtt.Visible = False

                        lstError.Items.Add("No attachments found for Call...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    End If
                    cPnlTaskAtt.Visible = False
                    cPnlActionAtt.Visible = False
                Case "T"
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-146", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name, convert(varchar,VH_DT8_Date,100) VH_DT8_Date,VH_NU9_Version,VH_DT8_Modify_Date,VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_File_ID_PK from " & strTable & ",T010011  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and VH_NU9_Task_number=" & mTaskNo & " and T010011.CI_NU8_Address_Number= " & strTable.Trim & ".VH_NU9_Address_Book_Number and  VH_VC4_Active_Status='ENB' and   VH_IN4_Level=2 and " & strTable & ".VH_NU9_CompId_Fk=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then
                        grdTask.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdTask.DataSource = dsAttachments
                        grdTask.DataBind()
                    Else
                        grdTask.Visible = False
                        'cpnlErrorPanel.Visible = True
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlTaskAtt.TitleCSS = "test2"
                        cPnlTaskAtt.Visible = False

                        lstError.Items.Add("No attachments found for Task...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)


                    End If
                    cPnlCallAtt.Visible = False
                    cPnlActionAtt.Visible = False
                Case "A"
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-160", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name,convert(varchar,VH_DT8_Date,100) VH_DT8_Date,VH_NU9_Version,VH_DT8_Modify_Date,VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,VH_NU9_File_ID_PK from " & strTable & ",T010011  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and VH_NU9_Task_number=" & mTaskNo & " and VH_NU9_Action_number=" & mActionNo & " and T010011.CI_NU8_Address_Number=" & strTable.Trim & ".VH_NU9_Address_Book_Number and VH_IN4_Level=3 and  VH_VC4_Active_Status='ENB'  and " & strTable & ".VH_NU9_CompId_Fk=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then
                        grdAction.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdAction.DataSource = dsAttachments
                        grdAction.DataBind()
                    Else
                        grdAction.Visible = False
                        'cpnlErrorPanel.Visible = True
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlActionAtt.TitleCSS = "test2"
                        cPnlActionAtt.Visible = False
                        lstError.Items.Add("No attachments found for Action...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    End If
                    cPnlCallAtt.Visible = False
                    cPnlTaskAtt.Visible = False
                Case "AO"
                    If SQL.Search(strTable, "ViewAttachment", "GetAttachments-160", "select VH_VC255_File_Path, VH_VC255_File_Name, T010011.CI_VC36_Name,convert(varchar,VH_DT8_Date,100) VH_DT8_Date,VH_NU9_Version,VH_DT8_Modify_Date,VH_NU9_Call_Number,VH_NU9_Task_Number,VH_NU9_Action_Number,VH_NU9_File_ID_PK from " & strTable & ",T010011  where VH_NU9_Call_Number=" & ViewState("CallNo") & " and VH_NU9_Task_number=" & mTaskNo & " and VH_NU9_Action_number=" & mActionNo & " and T010011.CI_NU8_Address_Number=" & strTable.Trim & ".VH_NU9_Address_Book_Number and VH_IN4_Level=3 and  VH_VC4_Active_Status='ENB'  and " & strTable & ".VH_NU9_CompId_Fk=" & CompanyId, dsAttachments, "sachin", "Prashar") = True Then
                        grdAction.Visible = True
                        SetDataTableDateFormat(dsAttachments.Tables(0), htDateCols)
                        grdAction.DataSource = dsAttachments
                        grdAction.DataBind()
                    Else
                        grdAction.Visible = False
                        'cpnlErrorPanel.Visible = True
                        'cpnlErrorPanel.TitleCSS = "test3"
                        cPnlActionAtt.TitleCSS = "test2"
                        cPnlActionAtt.Visible = False
                        lstError.Items.Add("No attachments found for Action...")
                        'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    End If
                    cPnlCallAtt.Visible = False
                    cPnlTaskAtt.Visible = False
            End Select

        Catch ex As Exception
            CreateLog("ViewAttachments", "GetAttachments-171", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub grdAttachments_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdAttachments.ItemCommand

        Dim strFileName As String = e.CommandName
        Dim strFilePath As String = Server.MapPath("../../") & e.CommandArgument.ToString()

        Try
            If IsNothing(ViewState("xy")) = False Then
                If ViewState("xy") = 0 Then

                Else
                    Dim strmFile As Stream = File.OpenRead(strFilePath)
                    Dim buffer(strmFile.Length) As Byte
                    strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
                    Response.ClearHeaders()
                    Response.ClearContent()

                    Response.ContentType = "application/octet-stream"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
                    'Response.BinaryWrite(buffer) 
                    Response.WriteFile(strFilePath) '''' modified on 17 Nov '09 by tarun.
                    Response.End()
                End If
            Else
                Dim strmFile As Stream = File.OpenRead(strFilePath)
                Dim buffer(strmFile.Length) As Byte
                strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
                Response.ClearHeaders()
                Response.ClearContent()
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
                Response.WriteFile(strFilePath) '''' modified on 17 Nov '09 by tarun.
                'Response.BinaryWrite(buffer)
                Response.End()
            End If

            ViewState("xy") = 1


        Catch ex As Exception
            ' CreateLog("ViewAttachments", "ItemCommand-192", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub grdTask_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdTask.ItemCommand

        Dim strFileName As String = e.CommandName
        Dim strFilePath As String = Server.MapPath("../../") & e.CommandArgument.ToString()

        Try
            Dim strmFile As Stream = File.OpenRead(strFilePath)
            Dim buffer(strmFile.Length) As Byte
            strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
            Response.BinaryWrite(buffer)
            Response.End()

        Catch ex As Exception
            CreateLog("ViewAttachments", "ItemCommand-214", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try

    End Sub

    Private Sub grdAction_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdAction.ItemCommand

        Dim strFileName As String = e.CommandName
        Dim strFilePath As String = Server.MapPath("../../") & e.CommandArgument.ToString()

        Try
            Dim strmFile As Stream = File.OpenRead(strFilePath)
            Dim buffer(strmFile.Length) As Byte
            strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
            Response.BinaryWrite(buffer)
            Response.End()

        Catch ex As Exception
            CreateLog("ViewAttachments", "ItemCommand-235", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdAction")
        End Try

    End Sub

    Private Sub ReadGrids()
        Try

            'Check call level checked rows
            Dim arCallFileID As String = ""
            Dim arCallNo As New ArrayList
            Dim gridCallrow As DataGridItem

            For Each gridCallrow In grdAttachments.Items
                If CType(gridCallrow.FindControl("chkCall"), CheckBox).Checked Then
                    arCallFileID &= gridCallrow.Cells(8).Text.Trim & ", "
                    arCallNo.Add(gridCallrow.Cells(4).Text.Trim)
                End If
            Next

            If arCallFileID <> "" Then
                arCallFileID = arCallFileID.Remove(arCallFileID.Length - 2, 2)
            End If

            'Check Task level checked rows
            Dim arTasksFileID As String = ""
            Dim gridTaskrow As DataGridItem
            Dim artaskCallNo As New ArrayList
            Dim arTaskNo As New ArrayList

            For Each gridTaskrow In grdTask.Items
                If CType(gridTaskrow.FindControl("ChkTask"), CheckBox).Checked Then
                    arTasksFileID &= gridTaskrow.Cells(9).Text.Trim & ", "
                    artaskCallNo.Add(gridTaskrow.Cells(4).Text.Trim)
                    arTaskNo.Add(gridTaskrow.Cells(5).Text.Trim)
                End If
            Next

            If arTasksFileID <> "" Then
                arTasksFileID = arTasksFileID.Remove(arTasksFileID.Length - 2, 2)
            End If

            'Check Action level checked rows
            Dim gridActionrow As DataGridItem
            Dim arActionFileID As String = ""
            Dim arActionCallNo As New ArrayList
            Dim aractionTaskNo As New ArrayList
            Dim arActionNo As New ArrayList

            For Each gridActionrow In grdAction.Items
                If CType(gridActionrow.FindControl("chkAction"), CheckBox).Checked Then
                    arActionFileID &= gridActionrow.Cells(10).Text.Trim & ", "
                    arActionCallNo.Add(gridActionrow.Cells(4).Text.Trim)
                    aractionTaskNo.Add(gridActionrow.Cells(5).Text.Trim)
                    arActionNo.Add(gridActionrow.Cells(6).Text.Trim)
                End If
            Next

            If arActionFileID <> "" Then
                arActionFileID = arActionFileID.Remove(arActionFileID.Length - 2, 2)
            End If

            If arCallFileID = "" And arTasksFileID = "" And arActionFileID = "" Then
                lstError.Items.Clear()
                lstError.Items.Add("Please select File to delete...")
                'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                Exit Sub
            End If

            If grdAction.Visible = False And grdAttachments.Visible = False And grdTask.Visible = False Then
                lstError.Items.Clear()
                lstError.Items.Add("No record to delete...")
                'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                Exit Sub
            End If

            '''''Records disable in call task and action level
            '''****************************************************************************************
            If arCallFileID <> "" Then
                If SQL.Update(strTable, "Attachments ", "update " & strTable & " set VH_VC4_Active_Status ='DSB' where VH_NU9_File_ID_PK in(" & arCallFileID & ") ", SQL.Transaction.Serializable) = True Then

                    Dim intcount As Integer
                    If SQL.Search(strTable, "Attachments", "Select * from " & strTable & "  where  VH_NU9_Call_Number = " & ViewState("CallNo") & "  and VH_NU9_CompId_Fk= " & ViewState("CompanyID") & " and VH_VC4_Active_Status = 'ENB' and VH_IN4_Level=1 ", intcount) = False Then
                        If intcount > 0 Then
                        Else
                            If mstrForm = "2" Then 'Template************************
                                SQL.Update("T050021", "Attachments ", "Update T050021 set CM_NU8_Attach_No =null where CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CM_NU9_Call_No_PK=" & ViewState("CallNo") & " ", SQL.Transaction.Serializable)
                            Else
                                SQL.Update("T040011", "Attachments ", "Update T040011 set CM_NU8_Attach_No =null where CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & " and CM_NU9_Call_No_PK=" & ViewState("CallNo") & " ", SQL.Transaction.Serializable)

                            End If
                        End If
                    End If
                    lstError.Items.Clear()
                    lstError.Items.Add("Record Deleted Successfully...")
                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    arCallFileID = ""
                End If
            End If

            If arTasksFileID <> "" Then
                If SQL.Update(strTable, "Attachments ", "update " & strTable & " set VH_VC4_Active_Status ='DSB' where VH_NU9_File_ID_PK in(" & arTasksFileID & ") ", SQL.Transaction.Serializable) = True Then

                    Dim i As Integer = 0
                    For i = 0 To arTaskNo.Count - 1
                        Dim intcount As Integer
                        If SQL.Search(strTable, "Attachments", "Select * from " & strTable & "  where  VH_NU9_Call_Number = " & ViewState("CallNo") & "  and VH_NU9_CompId_Fk= " & ViewState("CompanyID") & " and VH_VC4_Active_Status = 'ENB' and VH_IN4_Level=2 and VH_NU9_Task_Number=" & arTaskNo(i) & " ", intcount) = False Then
                            If intcount > 0 Then
                            Else
                                If mstrForm = "2" Then 'Template task table ************************
                                    SQL.Update("T050031", "Attachments ", "Update T050031 set TTM_CH1_Attachment =null where TTM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and TTM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TTM_NU9_Task_no_PK=" & arTaskNo(i) & " ", SQL.Transaction.Serializable)
                                Else 'master task table ************************
                                    SQL.Update("T040021", "Attachments ", "Update T040021 set TM_CH1_Attachment =null where TM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and TM_NU9_Call_No_FK=" & ViewState("CallNo") & " and TM_NU9_Task_no_PK=" & arTaskNo(i) & "  ", SQL.Transaction.Serializable)

                                End If
                            End If
                        End If
                    Next

                    lstError.Items.Clear()
                    lstError.Items.Add("Record Deleted Successfully...")
                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)

                    arTasksFileID = ""
                End If
            End If

            'Action Level*********************************
            If arActionFileID <> "" Then
                If SQL.Update(strTable, "Attachments ", "update " & strTable & " set VH_VC4_Active_Status ='DSB' where VH_NU9_File_ID_PK in(" & arActionFileID & ") ", SQL.Transaction.Serializable) = True Then

                    Dim i As Integer = 0
                    For i = 0 To arActionNo.Count - 1
                        Dim intcount As Integer
                        If SQL.Search(strTable, "Attachments", "Select * from " & strTable & "  where  VH_NU9_Call_Number = " & ViewState("CallNo") & "  and VH_NU9_CompId_Fk= " & ViewState("CompanyID") & " and VH_VC4_Active_Status = 'ENB' and VH_IN4_Level=3 and VH_NU9_Task_Number=" & aractionTaskNo(i) & " and VH_NU9_Action_Number=" & arActionNo(i) & " ", intcount) = False Then
                            If intcount > 0 Then
                            Else
                                If mstrForm = "2" Then 'Template Action table ************************
                                Else 'master Action table ************************
                                    SQL.Update("T040031", "Attachments ", "Update T040031 set AM_CH1_Attachment =null where AM_NU9_Comp_ID_FK=" & ViewState("CompanyID") & " and AM_NU9_Call_Number=" & ViewState("CallNo") & " and AM_NU9_Task_Number=" & aractionTaskNo(i) & " and AM_NU9_Action_Number=" & arActionNo(i) & "  ", SQL.Transaction.Serializable)

                                End If
                            End If
                        End If

                    Next
                    lstError.Items.Clear()
                    lstError.Items.Add("Record Deleted Successfully...")
                    'ShowMsgPenel('cpnlErrorPanel, lstError, Image2, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    arActionFileID = ""
                End If
            End If
            '******************************************************************************************
            ' Response.Write("<script>self.opener.Form1.submit();</script>")
        Catch ex As Exception
            CreateLog("Attachments", "ReadGrid-296", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Sub grdAttachments_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAttachments.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                CType(e.Item.Cells(1).FindControl("imgCallAttach"), ImageButton).Attributes.Add("OnClick", "return OpenAttachCall();")
            End If
        Catch ex As Exception
            CreateLog("Attachments", "grdAttachments_ItemDataBound-296", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Sub grdTask_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdTask.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                CType(e.Item.Cells(1).FindControl("imgTaskAttach"), ImageButton).Attributes.Add("OnClick", "return OpenAttachTask(" & Val(e.Item.Cells(5).Text) & ")")
            End If
        Catch ex As Exception
            CreateLog("Attachments", "grdTask_ItemDataBound-296", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub

    Private Sub grdAction_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAction.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                CType(e.Item.Cells(1).FindControl("imgActionAttach"), ImageButton).Attributes.Add("OnClick", "return OpenAttachAction(" & Val(e.Item.Cells(5).Text) & "," & Val(e.Item.Cells(6).Text) & ")")
            End If
        Catch ex As Exception
            CreateLog("Attachments", "grdAction_ItemDataBound-296", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
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
