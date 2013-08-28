'************************************************************************************************************
' Page                 :- Upload Attachment
' Purpose              :- Purpose of this screen is to upload attachment that user can selected to upload                              through File control   

' Tables used          :- T050041, T040041
' Date				Author						Modification Date					Description
' 18/03/06			Ranvijay/Sachin			    -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Partial Class MessageCenter_UploadFiles_UploadAttachment
    Inherits System.Web.UI.Page
    Dim mstrFilePath As String
    Dim arFileID As New ArrayList
    Dim arTFileID As New ArrayList
    Dim arAFileID As New ArrayList
    Dim mstrLevel As String
    Dim mintLevel As Integer
    Dim mstrFileName As String
    Dim mintFileID As Integer
    Dim rowvalue As Integer
    Dim mdvtable As New DataView
    Dim mstrForm As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'Security Block
            Dim intId As Integer
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                intId = 287 'Request.QueryString("ScrID")
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intId) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intId)
            End If
            'End of Security Block
            'Put user code to initialize the page here
            If Not IsPostBack Then

                txtCSS(Me.Page)
                imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
                imgDelete.Attributes.Add("Onclick", "return SaveEdit('Delete');")
                btnUpload.Attributes.Add("Onclick", "return FileUpload();")
                'imgSave.Attributes.Add("Onclick", "return SaveEdit('Save'," & Request.QueryString("CallNo") & ");")

                If Request.QueryString("CallNo") = Nothing Then
                    imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
                Else
                    imgSave.Attributes.Add("Onclick", "return SaveEdit('Save'," & Request.QueryString("CallNo") & ");")
                End If

            End If
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1
            mstrLevel = Request.QueryString("ID")
            mstrForm = Val(Request.QueryString("OPT"))
            Dim strhiddenImage As String
            strhiddenImage = Request.Form("txthiddenImage")
            If strhiddenImage <> "" Then
                Select Case strhiddenImage
                    Case "Save"
                        If Request.QueryString("CallNo") = Nothing Then
                            Response.Write("<script>window.close();self.opener.RefreshAttachment();</script>")
                            ViewState("xy") = 0
                        Else
                            Call SaveAttachments()
                            Response.Write("<script>window.close();self.opener.RefreshAttachment();</script>")
                            ViewState("xy") = 0
                        End If
                End Select
            End If
            ViewState("CompanyID") = Request.QueryString("CompanyID")
            ViewState("CallNo") = Request.QueryString("CallNo")
            If IsNothing(Request.QueryString("From")) = False Then
                If Request.QueryString("From") = "VIEW" Then
                    Select Case Request.QueryString("ID")
                        Case "C"
                            ViewState("TaskNo") = 0
                            ViewState("ActionNo") = 0
                        Case "T"
                            ViewState("TaskNo") = Val(Request.QueryString("VTaskNo"))
                            ViewState("ActionNo") = 0
                        Case "A"
                            ViewState("TaskNo") = Val(Request.QueryString("VTaskNo"))
                            ViewState("ActionNo") = Val(Request.QueryString("VActionNo"))
                    End Select
                End If
            Else
                Select Case Request.QueryString("ID")
                    Case "C"
                        ViewState("TaskNo") = 0
                        ViewState("ActionNo") = 0
                    Case "T"
                        ViewState("TaskNo") = Val(Request.QueryString("VTaskNo"))
                        ViewState("ActionNo") = 0
                    Case "A"
                        ViewState("TaskNo") = Val(Request.QueryString("VTaskNo"))
                        ViewState("ActionNo") = Val(Request.QueryString("VActionNo"))
                End Select
            End If

            txtCallNo.Text = Val(ViewState("CallNo"))
            txtTaskNo.Text = Val(ViewState("TaskNo"))
            txtActionNo.Text = Val(ViewState("ActionNo"))

            GetAttachments()

        Catch ex As Exception
            CreateLog("UploadAttachment", "Load-1900", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If (Upload.PostedFile.ContentLength / 1024) / 1024 > 7 Then
            lstError.Items.Clear()
            lstError.Items.Add("You cannot upload file of size more than 7MB...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If
        If Not Upload.PostedFile Is Nothing And Upload.PostedFile.ContentLength > 0 Then
            mstrFileName = System.IO.Path.GetFileName(Upload.PostedFile.FileName)
            Dim strSaveLocation As String
            Dim strSaveLocDatabase As String
            Dim dirTemp As DirectoryInfo

            If mstrForm <> "0" Then
                strSaveLocation = Server.MapPath("../../TemplateDockyard/Temp") & "\" & mstrFileName
                strSaveLocDatabase = ("../../TemplateDockyard/Temp") & "/" & mstrFileName
                dirTemp = New DirectoryInfo(Server.MapPath("../../TemplateDockyard/Temp"))
            Else
                strSaveLocation = Server.MapPath("../../Dockyard/Temp") & "\" & mstrFileName
                strSaveLocDatabase = ("../../Dockyard/Temp") & "/" & mstrFileName
                dirTemp = New DirectoryInfo(Server.MapPath("../../Dockyard/Temp"))
            End If

            Try
                If dirTemp.Exists = False Then
                    dirTemp.Create()
                End If

                Upload.PostedFile.SaveAs(strSaveLocation)
                gdblSize = Upload.PostedFile.ContentLength

                gdblSize = (gdblSize / (1024))
                txtPath.Text = "SetStockImages\" & mstrFileName
                'gdblSize = Mid(gdblSize, 1, 4)

                If mstrLevel = "C" Then
                    If SaveTempAttachment(strSaveLocDatabase, gdblSize, mstrFileName, AttachLevel.CallLevel) = True Then
                        GetAttachments()
                    End If
                ElseIf mstrLevel = "T" Then
                    If SaveTempAttachment(strSaveLocDatabase, gdblSize, mstrFileName, AttachLevel.TaskLevel) = True Then
                        GetAttachments()
                    End If
                ElseIf mstrLevel = "A" Then
                    If SaveTempAttachment(strSaveLocDatabase, gdblSize, mstrFileName, AttachLevel.ActionLevel) = True Then
                        GetAttachments()
                    End If
                End If
            Catch ex As Exception
                CreateLog("UploadAttachment", "Click-109", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "btnUpload")
            End Try
        Else
            lstError.Items.Clear()
            lstError.Items.Add("Please select a valid file...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
        End If
    End Sub

    Public Function SaveTempAttachment(ByVal AttachmentPath As String, ByVal AttachmentSize As Integer, ByVal AttachmentName As String, ByVal AttachmentLevel As AttachLevel) As Boolean
        Dim strSQL As String
        Dim intRows As Integer
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strTable As String

            If mstrForm = "2" Then
                If AttachmentLevel = mdlMain.AttachLevel.CallLevel Then
                    strSQL = "select * from T050041 where AT_VC255_File_Path='" & AttachmentPath.Trim & "' and AT_NU9_Call_number=" & ViewState("CallNo") & "  and  AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Session("PropUserID")
                ElseIf AttachmentLevel = mdlMain.AttachLevel.TaskLevel Then
                    strSQL = "select * from T050041 where AT_VC255_File_Path='" & AttachmentPath.Trim & "' and AT_NU9_Call_number=" & ViewState("CallNo") & "  and AT_NU9_Task_Number=" & ViewState("TaskNo") & "  and AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Session("PropUserID")
                ElseIf AttachmentLevel = mdlMain.AttachLevel.ActionLevel Then
                    strSQL = "select * from T050041 where AT_VC255_File_Path='" & AttachmentPath.Trim & "' and AT_NU9_Call_number=" & ViewState("CallNo") & "  and AT_NU9_Task_Number=" & ViewState("TaskNo") & " and AT_NU9_Action_Number=" & ViewState("ActionNo") & " and AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Session("PropUserID")
                End If
                SQL.Search("ViewAttachment", "SaveTempAttachment-132", strSQL, intRows, "")
                If intRows > 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("This file is already uploaded...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                End If
                strTable = "T050041"
                mintFileID = SQL.Search("UploadAttachment", "SaveTempAttachment-124", "Select max(AT_NU9_File_ID_PK) from T050041")
            Else
                If AttachmentLevel = mdlMain.AttachLevel.CallLevel Then
                    strSQL = "select * from T040041 where AT_VC255_File_Path='" & AttachmentPath.Trim & "' and AT_NU9_Call_number=" & ViewState("CallNo") & "  and  AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Session("PropUserID")
                ElseIf AttachmentLevel = mdlMain.AttachLevel.TaskLevel Then
                    strSQL = "select * from T040041 where AT_VC255_File_Path='" & AttachmentPath.Trim & "' and AT_NU9_Call_number=" & ViewState("CallNo") & "  and AT_NU9_Task_Number=" & ViewState("TaskNo") & "  and AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Session("PropUserID")
                ElseIf AttachmentLevel = mdlMain.AttachLevel.ActionLevel Then
                    strSQL = "select * from T040041 where AT_VC255_File_Path='" & AttachmentPath.Trim & "' and AT_NU9_Call_number=" & ViewState("CallNo") & "  and AT_NU9_Task_Number=" & ViewState("TaskNo") & " and AT_NU9_Action_Number=" & ViewState("ActionNo") & " and AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & Session("PropUserID")
                End If
                SQL.Search("ViewAttachment", "SaveTempAttachment-149", strSQL, intRows, "")
                If intRows > 0 Then
                    lstError.Items.Clear()
                    lstError.Items.Add("This file is already uploaded...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                End If
                strTable = "T040041"
                mintFileID = SQL.Search("UploadAttachment", "SaveTempAttachment-127", "Select max(AT_NU9_File_ID_PK) from T040041")
            End If
            mintFileID += 1
            'garABNo.Add("ADM")
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("AT_NU9_File_ID_PK")
            arColumnName.Add("AT_VC255_File_Name")
            arColumnName.Add("AT_VC255_File_Size")
            arColumnName.Add("AT_VC255_File_Path")
            arColumnName.Add("AT_VC1_Status")
            arColumnName.Add("AT_NU9_Address_Book_Number")
            arColumnName.Add("AT_NU9_Call_Number")
            arColumnName.Add("AT_NU9_Task_Number")
            arColumnName.Add("AT_NU9_Action_Number")
            arColumnName.Add("AT_NU9_CompId_Fk")
            arColumnName.Add("AT_DT8_Date")
            arColumnName.Add("AT_VC8_Role")
            arColumnName.Add("AT_NU9_Version")
            arColumnName.Add("AT_DT8_Modify_Date")
            arColumnName.Add("AT_IN4_Level")

            arRowData.Add(mintFileID)
            arRowData.Add(AttachmentName.Trim)
            arRowData.Add(AttachmentSize)
            arRowData.Add(AttachmentPath.Trim)
            arRowData.Add("T")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(ViewState("CallNo"))
            arRowData.Add(ViewState("TaskNo"))
            arRowData.Add(ViewState("ActionNo"))
            arRowData.Add(ViewState("CompanyID"))
            arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropRole"))
            arRowData.Add(0)
            arRowData.Add(Now)
            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            If mstrForm = "2" Then
                arColumnName.Add("AT_NU9_TemplateID_FK")
                arRowData.Add(ViewState("SAddressNumber"))
            End If
            If SQL.Save(strTable, "UploadAttachment", "SaveTempAttachment-183", arColumnName, arRowData) = True Then
                Session("Attach") = 1
                Return True
            Else
                Session("Attach") = 0
                Return False
            End If
        Catch ex As Exception
            CreateLog("UploadAttachment", "SaveTempAttachment-1900", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function
    Private Function GetAttachments() As Boolean
        Dim dsAttachments As DataSet
        Select Case mstrLevel
            Case "C"
                mintLevel = 1
            Case "T"
                mintLevel = 2
            Case "A"
                mintLevel = 3
        End Select
        rowvalue = 0
        Try
            dsAttachments = New DataSet
            If mstrForm = "2" Then
                SQL.Delete("UploadAttachment", "GetAttachment-212", "delete from T050041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number= " & HttpContext.Current.Session("PropUserID") & " and At_In4_Level<>" & mintLevel, SQL.Transaction.Serializable)
                dsAttachments = New DataSet
                SQL.Search("T050041", "UploadAttachment", "GetAttachment-214", "Select AT_NU9_File_ID_PK , AT_VC255_File_Name,At_In4_Level FROM T050041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & " and AT_NU9_CompId_Fk=" & ViewState("CompanyID") & "", dsAttachments, "sachin", "Prashar")
                grdAttachments.DataSource = GetCustomDS(dsAttachments, "T050041", mintLevel)
                mdvtable.Table = GetCustomDS(dsAttachments, "T050041", mintLevel).Tables("ColTable")
            Else
                SQL.Delete("UploadAttachment", "GetAttachment-219", "delete from T040041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number= " & HttpContext.Current.Session("PropUserID") & " and At_In4_Level<>" & mintLevel, SQL.Transaction.Serializable)
                dsAttachments = New DataSet
                SQL.Search("T040041", "UploadAttachment", "GetAttachment-221", "Select AT_NU9_File_ID_PK , AT_VC255_File_Name,At_In4_Level FROM T040041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & " and AT_NU9_CompId_Fk=" & ViewState("CompanyID") & "", dsAttachments, "sachin", "Prashar")
                grdAttachments.DataSource = GetCustomDS(dsAttachments, "T040041", mintLevel)
                mdvtable.Table = GetCustomDS(dsAttachments, "T040041", mintLevel).Tables("ColTable")
            End If
            grdAttachments.DataSource = mdvtable.Table
            grdAttachments.DataBind()
            Return True
        Catch ex As Exception
            CreateLog("UploadAttachment", "GetAttachment-231", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function
    Private Function GetAttachments1() As Boolean
        Select Case mstrLevel
            Case "C"
                mintLevel = 1
            Case "T"
                mintLevel = 2
            Case "A"
                mintLevel = 3
        End Select
        rowvalue = 0
        Dim dsAttachments As DataSet
        Try
            SQL.Delete("UploadAttachment", "GetAttachment1-248", "delete from T040041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number= " & HttpContext.Current.Session("PropUserID") & " and At_In4_Level<>" & mintLevel, SQL.Transaction.Serializable)
            dsAttachments = New DataSet
            If mstrForm = 2 Then
                SQL.Search("T050041", "UploadAttachment", "GetAttachment1-251", "Select AT_NU9_File_ID_PK , AT_VC255_File_Name,At_In4_Level FROM T050041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & " and AT_NU9_CompId_Fk=" & ViewState("CompanyID") & "", dsAttachments, "sachin", "Prashar")
            Else
                SQL.Search("T040041", "UploadAttachment", "GetAttachment1-253", "Select AT_NU9_File_ID_PK , AT_VC255_File_Name,At_In4_Level FROM T040041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & " and AT_NU9_CompId_Fk=" & ViewState("CompanyID") & "", dsAttachments, "sachin", "Prashar")
            End If
            grdAttachments.DataSource = GetCustomDS(dsAttachments, "T040041", mintLevel)
            mdvtable.Table = GetCustomDS(dsAttachments, "T040041", mintLevel).Tables("ColTable")
            grdAttachments.DataSource = mdvtable.Table
            grdAttachments.DataBind()
            If mdvtable.Table.Rows.Count > 0 Then
                Session("Attach") = 1
            Else
                Session("Attach") = 0
            End If

            Return True
        Catch ex As Exception
            CreateLog("UploadAttachment", "GetAttachment1-263", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Public Function GetCustomDS(ByVal dsAtt As DataSet, ByVal strTableName As String, ByVal shLevel As Integer) As DataSet
        Try
            Dim myDataSet As DataSet
            Dim myDataTable As DataTable = New DataTable("ColTable")
            Dim myDataColumn As DataColumn
            Dim myDataRow As DataRow
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "AT_NU9_File_ID_PK"
            myDataColumn.Caption = "ID"
            myDataTable.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "AT_VC255_File_Name"
            myDataColumn.Caption = "FileName"
            myDataTable.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "AT_IN4_Level"
            myDataColumn.Caption = "Level"
            myDataTable.Columns.Add(myDataColumn)
            Dim i As Integer
            i = 0
            While i < dsAtt.Tables(0).Rows.Count
                myDataRow = myDataTable.NewRow()
                myDataRow("AT_NU9_File_ID_PK") = dsAtt.Tables(0).Rows(i).Item("AT_NU9_File_ID_PK")
                myDataRow("AT_VC255_File_Name") = dsAtt.Tables(0).Rows(i).Item("AT_VC255_File_Name")
                Select Case shLevel
                    Case 1
                        myDataRow("AT_IN4_Level") = "Call Level"
                    Case 2
                        myDataRow("AT_IN4_Level") = "Task Level"
                    Case 3
                        myDataRow("AT_IN4_Level") = "Action Level"
                End Select
                myDataTable.Rows.Add(myDataRow)
                i = i + 1
            End While
            myDataSet = New DataSet
            myDataSet.Tables.Add(myDataTable)
            Return myDataSet
        Catch ex As Exception
            CreateLog("Upload Attachment", "GetCustomDataset-311", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Private Sub grdAttachments_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAttachments.ItemDataBound

        Dim dv As DataView = mdvtable
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dv.Table.Columns
        Dim strID As String
        ' GrdAddSerach.Columns.Clear()
        Try
            For Each dcCol In mdvtable.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = grdAttachments.DataKeys(e.Item.ItemIndex)

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck(" & strID & ", '" & rowvalue & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("UPATT", "ItemDataBound-334", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Private Sub imgDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDelete.Click
        Dim intFileID As Integer

        If Request.Form("txthiddenDelVal") <> "undefined" Then
            If Request.Form("txthiddenDelVal") <> "" Then
                intFileID = Request.Form("txthiddenDelVal")
            Else
                Exit Sub
            End If
        Else
            Exit Sub
        End If

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            If mstrForm = 2 Then
                '                SQL.DBTable = "T050041"
                SQL.Delete("UploadAttachment", "imgDeleteClick-358", "Delete from T050041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & " and AT_NU9_CompId_Fk='" & ViewState("CompanyID") & "' and AT_NU9_File_ID_PK=" & intFileID & "", SQL.Transaction.Serializable)
                GetAttachments1()
            Else
                ' SQL.DBTable = "T040041"
                SQL.Delete("UploadAttachment", "imgDeleteClick-362", "Delete from T040041 where AT_VC1_Status='T' and AT_NU9_Address_Book_Number=" & HttpContext.Current.Session("PropUserID") & " and AT_NU9_CompId_Fk='" & ViewState("CompanyID") & "'  and AT_NU9_File_ID_PK=" & intFileID & "", SQL.Transaction.Serializable)
                GetAttachments1()
            End If

        Catch ex As Exception
            CreateLog("UpdateAttachment", "imgDelete_Click-367", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "imgDelete")
        End Try
    End Sub

    Private Function SaveAttachments() As Boolean
        Try
            Dim enumAttLevel As AttachLevel
            Select Case Request.QueryString("ID")
                Case "C"
                    enumAttLevel = mdlMain.AttachLevel.CallLevel
                Case "T"
                    enumAttLevel = mdlMain.AttachLevel.TaskLevel
                Case "A"
                    enumAttLevel = mdlMain.AttachLevel.ActionLevel
            End Select
            If Request.QueryString("OPT") = 2 Then
                GetFilesTL(enumAttLevel)
            Else
                GetFiles(enumAttLevel)
            End If
        Catch ex As Exception
            CreateLog("UpdateAttachment", "SaveAttachments-367", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Private Function GetFiles(ByVal Level As AttachLevel) As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim blnRead As Boolean

            Select Case Level
                ' For Calls
                Case AttachLevel.CallLevel

                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1464", "select * from T040041 Where AT_IN4_Level=1 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            If CreateFolder(ViewState("CallNo")) = True Then
                                shAttachments = 1
                                'Return True
                            Else
                                shAttachments = 2
                                'Return False
                            End If
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        'sql.Update("Call_Detail","GetFiles-1600","Update T040011 set CM_NU8_Attach_No="
                        Return True
                    Else
                        Return False
                    End If
                    ' For Tasks
                Case AttachLevel.TaskLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1491", "select * from T040041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("CallNo"), ViewState("TaskNo"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    ' For Actions
                Case AttachLevel.ActionLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Call_Detail", "GetFiles-1512", "select * from T040041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolder(ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
            End Select
            sqrdTempFiles.Close()
        Catch ex As Exception
            CreateLog("Call-Detail", "GetFiles-1837", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, "NA", "NA", "dtgTask")
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo)

        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1577", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-1596", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & ViewState("CompanyID"))

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1624", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel) = True Then
                            SQL.Update("Call_Detail", "CreateFolder-1637", "update t040011 set CM_NU8_Attach_No=1 WHERE CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception

            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                ' SQL.DBTable = "T040051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-2242", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & ViewState("CompanyID"))

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("CallDetail", "CreateFolder-2747", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

    Private Function CreateFolder(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../Dockyard")
        Dim strPathDB As String = ("Dockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Call_Detail", "CreateFolder-2356", "select max(VH_NU9_Version) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & "  and VH_NU9_Action_Number=" & ActionNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "' and VH_NU9_CompId_Fk=" & ViewState("CompanyID"))

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If UpdateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber) = True Then
                        If SaveAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("Call_Detail", "CreateFolder-1661", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
            Return False
        End Try
    End Function

    Private Function GetFilesTL(ByVal Level As AttachLevel) As Boolean
        Dim sqrdTempFiles As SqlDataReader

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' SQL.DBTable = "T050041"
            SQL.DBTracing = False

            Dim blnRead As Boolean

            Select Case Level
                ' For Calls
                Case AttachLevel.CallLevel

                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("", "", "select * from T050041 Where AT_IN4_Level=1 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            If CreateFolderTL(ViewState("CallNo")) = True Then
                                shAttachments = 1
                                'Return True
                            Else
                                shAttachments = 2
                                'Return False
                            End If
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    ' For Tasks
                Case AttachLevel.TaskLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Template Detail", "GetFiles-1154", "select * from T050041 Where AT_IN4_Level=2 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolderTL(ViewState("CallNo"), ViewState("TaskNo"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    ' For Actions
                Case AttachLevel.ActionLevel
                    Dim shAttachments As Short
                    sqrdTempFiles = SQL.Search("Template Detail", "GetFiles-1175", "select * from T050041 Where AT_IN4_Level=3 and AT_NU9_Address_Book_Number=" & Val(Session("PropUserID")) & " and AT_VC1_Status='T' and AT_VC8_Role=" & Val(Session("PropRole")) & " and AT_NU9_CompId_Fk=" & Val(ViewState("CompanyID")) & "", SQL.CommandBehaviour.CloseConnection, blnRead)

                    If blnRead = True Then
                        While sqrdTempFiles.Read
                            mintFileID = sqrdTempFiles.Item("AT_NU9_File_ID_PK")
                            mstrFileName = sqrdTempFiles.Item("AT_VC255_File_Name")
                            mstrFilePath = sqrdTempFiles.Item("AT_VC255_File_Path")
                            CreateFolderTL(ViewState("CallNo"), ViewState("TaskNo"), ViewState("ActionNo"))
                        End While
                    Else
                        Return False
                    End If

                    If shAttachments = 1 Then
                        Return True
                    Else
                        Return False
                    End If
            End Select
        Catch ex As Exception
            CreateLog("TemplateDetail", "ValidateRecords-1245", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Private Function CreateFolderTL(ByVal CallNo As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../TemplateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")
        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0, ViewState("SAddressNumber")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            SQL.Update("TemplateDetail", "CreateFolder-1141", "update T050021 set TCM_NU8_Attach_No=1 WHERE TCM_NU9_Call_No_PK=" & CallNo & " and TCM_NU9_CompID_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("TemplateDetail", "CreateFolder-1160", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                Try
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)
                Catch ex As Exception
                End Try
                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, True, CallNo, 0, ViewState("CompanyID"), 0, ViewState("SAddressNumber")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            SQL.Update("TemplateDetail", "CreateFolder-1188", "update T050021 set TCM_NU8_Attach_No=1 WHERE TCM_NU9_Call_No_PK=" & CallNo & " and TCM_NU9_CompID_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.CallLevel, dblVersionNo, False, CallNo, 0, ViewState("CompanyID"), 0, ViewState("SAddressNumber")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, 0, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.CallLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            SQL.Update("TemplateDetail", "CreateFolder-1201", "update t050021 set TCM_NU8_Attach_No=1 WHERE TCM_NU9_Call_No_PK=" & CallNo & " and TCM_NU9_CompID_FK=" & ViewState("CompanyID") & "", SQL.Transaction.Serializable)
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateFolder-1201", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
            Return False
        End Try
    End Function

    Private Function CreateFolderTL(ByVal CallNo As Integer, ByVal TaskNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../TemplateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"

                If File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T050051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                '  SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("", "", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.TaskLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), 0, ViewState("SAddressNumber")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, 0, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.TaskLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("TemplateDetail", "CreateFolder-1869", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function

    Private Function CreateFolderTL(ByVal CallNo As Integer, ByVal TaskNumber As Integer, ByVal ActionNumber As Integer) As Boolean
        Dim strPath As String = Server.MapPath("../../tempateDockyard")
        Dim strPathDB As String = ("TemplateDockyard")

        Dim objFolder As DirectoryInfo = New DirectoryInfo(strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber)
        Dim strFilePath As String
        Dim dblVersionNo As Double

        Try
            ' This is for the first time when a document is uploaded
            If objFolder.Exists = False Then
                ' Then Create the folder for that call.
                objFolder.Create()
                ' Provide the path for creating the folder.
                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"

                If IO.File.Exists(strPath & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & mstrFileName) Then

                Else
                    ' First time entry of the document
                    If dblVersionNo > 0 Then
                        ' Increment the version number
                        dblVersionNo += 0.1
                    Else
                        dblVersionNo = 1.0
                    End If

                    Dim strFileLocation As String = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                    ' Change the path of the document so that it can be entered into database because in database we have to change "/" to "\"
                    'Dim strChangesPath As String = strPath.Replace("\", "/")
                    Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                    ' Create the sub-directory with the version name
                    objFolder.CreateSubdirectory(dblVersionNo)
                    ' Move the file to that folder
                    File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                    'Save the attachment with version (T040051) and update the primary table where atchmnt is saved as Temp (T040041)
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber, ViewState("SAddressNumber")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            Else
                ' This flag will decide whether to update Date in Version table (T040051) or not for an attachment
                Dim shFlag As Short

                strFilePath = strPath & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\"
                ' Get the connection string from the web config file.
                Dim strSQL As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.DBConnection = strSQL
                'SQL.DBTable = "T050051"
                SQL.DBTracing = False

                dblVersionNo = SQL.Search("Template Detail", "CreateFolder-2156", "select max(VH_NU9_Version) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNumber & "  and VH_NU9_Action_Number=" & ActionNumber & " and VH_VC255_File_Name='" & mstrFileName.Trim & "'")

                ' Check if its a new upload or a new version o f an existing attachment.
                If File.Exists(strPath.Trim & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim) Then
                    ' Check the version number of that document from the database.
                    shFlag = 1
                End If

                ' Increment the version number
                If dblVersionNo > 0 Then
                    dblVersionNo += 0.1
                Else
                    dblVersionNo = 1.0
                End If

                Dim strFileLocation As String = strPath.Trim & "\" & ViewState("CompanyID") & "\" & CallNo & "\" & TaskNumber & "\" & ActionNumber & "\" & dblVersionNo & "\" & mstrFileName.Trim
                ' Change the path of the document so that it can be entered into database
                'Dim strChangesPath As String = strPath.Replace("\", "/")
                Dim strChanges As String = strPathDB.Trim & "/" & ViewState("CompanyID") & "/" & CallNo & "/" & TaskNumber & "/" & ActionNumber & "/" & dblVersionNo & "/" & mstrFileName.Trim

                objFolder.CreateSubdirectory(dblVersionNo)
                ' Move the file to that folder
                File.Move(strPath.Trim & "\Temp" & "\" & mstrFileName.Trim, strFileLocation)

                If shFlag = 1 Then
                    ' if attachment already exist in the table then update the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, True, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber, ViewState("SAddressNumber")) = True Then
                        If UpdateTemplateAttachment(strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                Else
                    ' if attachment is a new one then save the version no
                    If SaveTemplateAttachmentVersion(strChanges, gdblSize, mstrFileName, AttachLevel.ActionLevel, dblVersionNo, False, CallNo, TaskNumber, ViewState("CompanyID"), ActionNumber, ViewState("SAddressNumber")) = True Then
                        If SaveTemplateAttachment(mintFileID, strChanges, mstrFileName, CallNo, TaskNumber, ActionNumber, 0, dblVersionNo, ViewState("CompanyID"), AttachLevel.ActionLevel, ViewState("SAddressNumber"), Server.MapPath("../../TemplateDockyard")) = True Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If

                End If
            End If
        Catch ex As Exception
            CreateLog("Template Detail", "CreateFolder-1986", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try
    End Function
End Class
