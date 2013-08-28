'************************************************************************************************************
' Page                 : - frm_ObjectData
' Purpose              : - This screen holds the menus & screen entries .User can enter a new screen or menu                            entry or can change the order or parent screen.   
' Tables used          : - T070011, T070042
' Date					Author						Modification Date					Description
' 04/06/2006			jagtar		                -------------------				    Created

'************************************************************************************************************
Imports ION.Data
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Text.StringBuilder
Imports System.Web.Mail
Imports Microsoft.Web.UI.WebControls


Partial Class Security_frm_ObjectData
    Inherits System.Web.UI.Page
    Private Shared intObjID As Int32
    Private Shared strPType As String
    Private Shared intObjPID As Int32
    Private Shared intKeyMode As Int16
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        If Not IsPostBack Then
            txtCSS(Me.Page)
            FillList()
            clsObjectData.FillObjectData(Me.NewTree)
            'setEnableDisable(False)
            intObjID = NewTree.Nodes(0).ID
            FillFields()
        End If
        dtStatusDate.LeftPos = 450
        dtStatusDate.TopPos = 350
        imgDelete.Attributes.Add("onClick", "return ConfirmDelete();")
    End Sub
    Private Sub FillList()
        With cboObjectType
            .Items.Add(New ListItem("Main Menu", "MNU"))
            .Items.Add(New ListItem("Screen", "SCR"))
            .Items.Add(New ListItem("Pop Screen", "POP"))
            .Items.Add(New ListItem("Collapsible Panel", "PNL"))
            .Items.Add(New ListItem("Panel", "PN1"))
            .Items.Add(New ListItem("View", "VIW"))
            .Items.Add(New ListItem("Grid Control", "GRD"))
            .Items.Add(New ListItem("Grid Column", "COL"))
            .Items.Add(New ListItem("Button", "BTN"))
            .Items.Add(New ListItem("Text Box", "TXT"))
            .Items.Add(New ListItem("Label", "LBL"))
            .Items.Add(New ListItem("Combo Box", "CMB"))
            .Items.Add(New ListItem("List Box", "LST"))
            .Items.Add(New ListItem("Check Box", "CHK"))
            .Items.Add(New ListItem("Optional Button", "OPT"))
            .Items.Add(New ListItem("Image", "IMG"))
            .Items.Add(New ListItem("HyperLink", "HLK"))

        End With
        With cboStatus
            .Items.Add(New ListItem("Enabled", "ENB"))
            .Items.Add(New ListItem("Disabled", "DSB"))
        End With

    End Sub
    Private Sub imgAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click
        intKeyMode = 1
        If cboObjectType.SelectedValue = "SCR" Then
            txtFPath.Enabled = True
        Else
            txtFPath.Enabled = False
        End If
        'setEnableDisable(True)
        ClearCtls()
        SetFocus(txtObjName)
    End Sub
    Private Function DeleteRecord() As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        Dim strConn As String
        Dim objTrn As SqlTransaction
        If isUsed() = True Then

            Me.RegisterStartupScript("key", "<script language=javascript>alert('This object is already used, cannot be deleted');</script>")

            Exit Function
        End If
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        objConn = New SqlConnection(strConn)
        objConn.Open()
        objTrn = objConn.BeginTransaction(IsolationLevel.RepeatableRead)
        Try
            strQuery = "Delete from T070011 where OBM_IN4_Object_ID_PK = " & intObjID
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                .Transaction = objTrn
                .ExecuteNonQuery()
            End With

            objTrn.Commit()
            Return True
        Catch ex As Exception
            ' DisplayError(ex.Message)
            objTrn.Rollback()
            Response.Write(ex.Message)
            Return False
        Finally
            objConn.Close()

        End Try

    End Function
    Private Function isUsed() As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        Dim strConn As String
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim obj As Object
        Try
            strQuery = "Select count(*) from T070042 where ROD_IN4_Object_ID_FK =" & intObjID
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                obj = .ExecuteScalar()
            End With
            If (Not obj Is Nothing) Then
                intcount = Convert.ToInt32(obj)
            End If
            If intcount > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ' DisplayError(ex.Message)
            Response.Write(ex.Message)
            Return False
        Finally
            objConn.Close()

        End Try

    End Function


    Private Function SaveData() As Boolean
        Dim strMan, strDescWidth, strURLColName, strFPath As String
        Dim DA As SqlDataAdapter
        Dim strQuery, strConn As String
        Dim objCommand As SqlCommand
        Dim dtData As New DataTable
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        If chkIsMandatory.Checked = True Then
            strMan = "Y"
        Else
            strMan = "N"
        End If
        If cboObjectType.SelectedValue = "VIW" Then
            strDescWidth = txtVColWidth.Text.Trim
            strURLColName = txtVColName.Text.Trim
        Else
            strDescWidth = ""
            strURLColName = txtPageURL.Text.Trim
        End If
        If cboObjectType.SelectedValue = "SCR" Then
            strFPath = txtFPath.Text.Trim
        Else
            strFPath = ""
        End If

        If intKeyMode = 1 Then
            strQuery = "Insert into T070011(OBM_VC4_Object_Type_FK,OBM_VC50_Object_Name," _
            & " OBM_VC50_Alias_Name,OBM_VC50_Grid_Name,OBM_CH1_Mandatory," _
            & " OBM_IN4_Object_PID_FK,OBM_VC200_Descr,OBM_VC4_Status_Code_FK," _
            & "OBM_DT8_Status_Date,OBM_VC200_URL,OBM_VC200_Image,OBM_VC8_FPath,OBM_SI2_Order_By)" _
            & " Values('" & cboObjectType.SelectedValue & "','" & txtObjName.Text.Trim & "','" _
            & txtObjAName.Text.Trim & "','" & txtGName.Text.Trim & "','" & strMan & "'," _
            & Val(cboPObjName.SelectedValue) & ",'" & strDescWidth & "','" & cboStatus.SelectedValue & "','" _
            & dtStatusDate.CalendarDate & "','" & strURLColName & "','" & txtImgURL.Text.Trim _
            & "','" & strFPath & "'," & Val(txtOrderBy.Text.Trim) & ")"
        ElseIf intKeyMode = 2 Then
            strQuery = "Update T070011 set OBM_VC4_Object_Type_FK = '" & cboObjectType.SelectedValue & "'," _
            & "OBM_VC50_Object_Name = '" & txtObjName.Text.Trim & "'," _
            & "OBM_VC50_Alias_Name = '" & txtObjAName.Text.Trim & "'," _
            & "OBM_VC50_Grid_Name='" & txtGName.Text.Trim & "'," _
            & "OBM_CH1_Mandatory='" & strMan & "'," _
            & "OBM_IN4_Object_PID_FK = " & Val(cboPObjName.SelectedValue) _
            & ",OBM_VC200_Descr ='" & strDescWidth & "'," _
            & "OBM_VC4_Status_Code_FK = '" & cboStatus.SelectedValue & "'," _
            & "OBM_DT8_Status_Date='" & dtStatusDate.CalendarDate & "'," _
            & "OBM_VC200_URL='" & strURLColName & "'," _
            & "OBM_VC200_Image='" & txtImgURL.Text.Trim & "'," _
            & "OBM_VC8_FPath='" & strFPath & "'," _
            & "OBM_SI2_Order_By=" & Val(txtOrderBy.Text.Trim) _
            & " where OBM_IN4_Object_ID_PK =" & intObjID
        End If

        Dim objCon As SqlConnection = New SqlConnection(strConn)

        Try
            objCon.Open()
            objCommand = New SqlCommand
            With objCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objCon
                .ExecuteNonQuery()
            End With
            If intKeyMode = 1 Then
                intKeyMode = 0
            End If
            Return True
        Catch ex As Exception
            Return False
            Dim str As String
            str = ex.Message
        Finally
            objCon.Close()
        End Try
    End Function
    Private Function FillFields()
        Dim dvData As DataView
        Dim strObjType As String
        Dim dv As DataView
        dvData = FilterData(intObjID)
        If dvData.Count > 0 Then
            strObjType = dvData.Item(0).Item("ObjType")
            If strObjType = "MNU" Then
                cboPObjName.Items.Clear()
                cboPObjName.Items.Add(New ListItem("None", "0"))
            ElseIf strObjType = "SCR" Then
                cboPObjName.Items.Clear()
                dv = GetParent("MNU")
                Dim i As Int16 = dv.Count
                cboPObjName.DataSource = dv
                cboPObjName.DataTextField = "AName"
                cboPObjName.DataValueField = "ObjectID"
                cboPObjName.DataBind()
            ElseIf strObjType = "POP" Then
                cboPObjName.Items.Clear()
                cboPObjName.DataSource = GetParent("MNU")
                cboPObjName.DataTextField = "AName"
                cboPObjName.DataValueField = "ObjectID"
                cboPObjName.DataBind()
            Else
                Dim dv1 As DataView
                dv1 = GetParent("SCR")
                Dim i As Int16 = dv1.Count
                cboPObjName.Items.Clear()
                cboPObjName.DataSource = GetParent("SCR")
                cboPObjName.DataTextField = "AName"
                cboPObjName.DataValueField = "ObjectID"
                cboPObjName.DataBind()
            End If
            cboObjectType.SelectedValue = strObjType
            If Not IsDBNull(dvData.Item(0).Item("ObjectPID")) Then
                cboPObjName.SelectedValue = dvData.Item(0).Item("ObjectPID")
            Else
                cboPObjName.SelectedIndex = -1
            End If
            If Not IsDBNull(dvData.Item(0).Item("Name")) Then
                txtObjName.Text = dvData.Item(0).Item("Name")
            Else
                txtObjName.Text = ""
            End If
            If Not IsDBNull(dvData.Item(0).Item("AName")) Then
                txtObjAName.Text = dvData.Item(0).Item("AName")
            Else
                txtObjAName.Text = ""
            End If
            If Not IsDBNull(dvData.Item(0).Item("FPath")) Then
                txtFPath.Text = dvData.Item(0).Item("FPath")
            Else
                txtFPath.Text = ""
            End If

            If strObjType <> "VIW" Then
                If Not IsDBNull(dvData.Item(0).Item("ImageURL")) Then
                    txtImgURL.Text = dvData.Item(0).Item("ImageURL")
                Else
                    txtImgURL.Text = ""
                End If
                If Not IsDBNull(dvData.Item(0).Item("PageURL")) Then
                    txtPageURL.Text = dvData.Item(0).Item("PageURL")
                Else
                    txtPageURL.Text = ""
                End If
            Else
                txtImgURL.Text = ""
                txtPageURL.Text = ""
            End If
            If Not IsDBNull(dvData.Item(0).Item("OrderBy")) Then
                txtOrderBy.Text = dvData.Item(0).Item("OrderBy")
            Else
                txtOrderBy.Text = ""
            End If
            If Not IsDBNull(dvData.Item(0).Item("Mandatory")) Then
                If dvData.Item(0).Item("Mandatory") = "Y" Then
                    chkIsMandatory.Checked = True
                Else
                    chkIsMandatory.Checked = False
                End If
            Else
                chkIsMandatory.Checked = False
            End If
            If Not IsDBNull(dvData.Item(0).Item("StatusCode")) Then
                cboStatus.SelectedValue = dvData.Item(0).Item("StatusCode")
            Else
                cboStatus.SelectedIndex = -1
            End If
            If Not IsDBNull(dvData.Item(0).Item("StatusDate")) Then
                dtStatusDate.CalendarDate = dvData.Item(0).Item("StatusDate")
            Else
                dtStatusDate.CalendarDate = ""
            End If
            If strObjType = "VIW" Then
                If Not IsDBNull(dvData.Item(0).Item("Descr")) Then
                    txtVColWidth.Text = dvData.Item(0).Item("Descr")
                Else
                    txtVColWidth.Text = ""
                End If
                If Not IsDBNull(dvData.Item(0).Item("PageURL")) Then
                    txtVColName.Text = dvData.Item(0).Item("PageURL")
                Else
                    txtVColName.Text = ""
                End If
            End If
            intKeyMode = 2
        End If
    End Function
    Private Function GetParent(ByVal objType As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        Dim dtTemp As DataTable
        If IsNothing(Cache(Session("PropUserID") & "ObjectData")) Then
            Dim obj As New clsObjectData
            obj.FillCache()
        End If
        dtTemp = Cache(Session("PropUserID") & "ObjectData")
        If objType = "CTL" Then
            strFilter = "ObjType not in('MNU','SCR','POP')"
        Else
            strFilter = "ObjType = '" & objType & "' or ObjType ='POP'"
        End If
        dvPC = New DataView(dtTemp, strFilter, "ObjectID", DataViewRowState.CurrentRows)
        dvPC.Sort = "OrderBy"
        Return dvPC
    End Function
    Private Function FilterData(ByVal objID As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        Dim dtTemp As DataTable
        If IsNothing(Cache(Session("PropUserID") & "ObjectData")) Then
            Dim obj As New clsObjectData
            obj.FillCache()
        End If
        dtTemp = Cache(Session("PropUserID") & "ObjectData")
        strFilter = "ObjectID = " & objID
        dvPC = New DataView(dtTemp, strFilter, "ObjectID", DataViewRowState.CurrentRows)
        dvPC.Sort = "OrderBy"
        Return dvPC
    End Function

    Private Sub NewTree_SelectedIndexChange(ByVal sender As Object, ByVal e As Microsoft.Web.UI.WebControls.TreeViewSelectEventArgs)
        Dim Tn As TreeNode
        Tn = NewTree.GetNodeFromIndex(e.NewNode)
        intObjID = Val(Tn.ID)
        FillFields()
    End Sub

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        If intKeyMode = 1 Or intKeyMode = 2 Then
            If cboObjectType.SelectedValue = "SCR" Then
                If isFPathExisted() = True Then
                    Response.Write("Fast path already existed")
                    Exit Sub
                End If
            End If
            If SaveData() = True Then
                RefeshData()
            End If
        End If
    End Sub
    Private Function isFPathExisted() As Boolean
        Dim strQuery As String
        Dim cmdCommand As New SqlCommand
        Dim objConn As SqlConnection
        Dim intcount As Int16
        Dim strConn As String
        If txtFPath.Text.Trim = "" Then
            Exit Function
        End If
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        objConn = New SqlConnection(strConn)
        objConn.Open()
        Dim obj As Object
        Try
            If intKeyMode = 1 Then
                strQuery = "Select count(*) from T070011 where OBM_VC8_FPath ='" & txtFPath.Text.Trim & "'" _
                            & " and OBM_VC4_Object_Type_FK = 'SCR'"
            Else
                strQuery = "Select count(*) from T070011 where OBM_VC8_FPath ='" & txtFPath.Text.Trim & "'" _
                            & " and OBM_VC4_Object_Type_FK = 'SCR' and OBM_IN4_Object_ID_PK <> " & intObjID
            End If
            With cmdCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objConn
                obj = .ExecuteScalar()
            End With
            If (Not obj Is Nothing) Then
                intcount = Convert.ToInt32(obj)
            End If
            If intcount > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ' DisplayError(ex.Message)
            Response.Write(ex.Message)
            Return False
        Finally
            objConn.Close()

        End Try

    End Function
    Private Sub imgSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim host As System.Net.IPHostEntry
        Dim strComputerName As String
        Dim obj As HttpContext

        obj = HttpContext.Current
        strComputerName = obj.Request.UserHostName()

        'host = System.Net.Dns.GetHostByAddress(Request.ServerVariables.Item("REMOTE_HOST"))
        'strComputerName = host.HostName

    End Sub
    Private Sub ClearCtls()
        txtObjAName.Text = ""
        txtObjName.Text = ""
        txtOrderBy.Text = ""
        chkIsMandatory.Checked = False
        txtVColWidth.Text = ""
        txtVColName.Text = ""
        txtGName.Text = ""
        txtFPath.Text = ""
    End Sub
    Private Sub setEnableDisable(ByVal value As Boolean)
        cboPObjName.Enabled = value
        cboObjectType.Enabled = value
        txtObjName.Enabled = value
        txtPageURL.Enabled = value
        txtImgURL.Enabled = value
        txtOrderBy.Enabled = value
        chkIsMandatory.Enabled = value
        cboStatus.Enabled = value
        txtVColWidth.Enabled = value
        txtVColName.Enabled = value
        txtGName.Enabled = value
        txtObjAName.Enabled = value
    End Sub
    Private Sub imgEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        intKeyMode = 2
        If cboObjectType.SelectedValue = "SCR" Then
            txtFPath.Enabled = True
        Else
            txtFPath.Enabled = False
        End If

        'setEnableDisable(False)
    End Sub

    Private Sub imgDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDelete.Click
        If DeleteRecord() = True Then
            RefeshData()
        End If
    End Sub
    Private Sub RefeshData()
        NewTree.Nodes.Clear()
        Dim obj As New clsObjectData
        obj.FillCache()
        clsObjectData.FillObjectData(Me.Newtree)
        intObjID = NewTree.Nodes(0).ID
        FillFields()
    End Sub
    Private Sub imgReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReset.Click
        RefeshData()
    End Sub

   
    Protected Sub Logout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Logout.Click
        LogoutWSS()
    End Sub
End Class
