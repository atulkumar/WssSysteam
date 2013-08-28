'**********************************************************************************************************
' Page                   : - Object_Assignment
' Purpose                : - 
' Tables used            : - T120011
' Date		    		Author						Modification Date					Description
' 25/03/06				Amandeep				    ----------------	        		Created
'
''*********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data

Partial Class ChangeManagement_Object_Assignment
    Inherits System.Web.UI.Page

    Private Shared dtItemDetail As DataTable
    Private intProjNo As Int32
    'Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgSave As System.Web.UI.WebControls.ImageButton
    ' Protected WithEvents lblTitleLabelobjassign As System.Web.UI.WebControls.Label
    Private intRowNo As Int32

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        txtCSS(Me.Page)
        Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1


        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")


        intProjNo = Val(Request.QueryString("codeID"))
        intRowNo = Val(Request.QueryString("rowNo"))

        Dim txthiddenImage = Request.Form("txthiddenImage")
        Dim txthiddenAdno = Request.Form("txthiddenAdno")

        cpnlError.Visible = False
        cpnlError.State = CustomControls.Web.PanelState.Collapsed

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SUserID") = Request.Form("txthidden")
            HttpContext.Current.Session("SUser") = Request.Form("txthiddenUser")
            HttpContext.Current.Session("SRole") = Request.Form("txthiddenRole")
            HttpContext.Current.Session("SCompany") = Request.Form("txthiddenCompany")
        End If

        If (Not Page.IsPostBack) Then
            If PopulateObjectsGrid() = False Then
                cpnlError.Visible = True
                cpnlError.State = CustomControls.Web.PanelState.Expanded
            End If
        Else

        End If

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                            Exit Sub
                        End If
                        'End of Security Block
                        If SaveFormsRecords() = True Then
                            lstError.Items.Clear()
                            lstError.Items.Add("Record saved successfully...")
                            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            'cpnlError.Visible = True
                            'cpnlError.State = CustomControls.Web.PanelState.Expanded
                        Else
                            lstError.Items.Clear()
                            lstError.Items.Add("Server is busy please try later...")
                            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                            'cpnlError.Visible = True
                            'cpnlError.State = CustomControls.Web.PanelState.Expanded
                        End If

                    Case "Close"
                        Response.Redirect("Object_assignment.aspx")
                    Case "Search"

                    Case "Logout"
                        LogoutWSS()
                End Select
            Catch ex As Exception
                'Dim str As String = ex.ToString
                CreateLog("Object_Assignment", "Load-118", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try

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

    Private Function ReadObjectDetailsGrid() As Boolean

        dtItemDetail.Rows.Clear()
        Dim drow As DataRow
        Dim gridrow As DataGridItem
        For Each gridrow In dgObjectDetails.Items
            If (Not CType(gridrow.FindControl("txtObjName"), TextBox).Text = "") Then
                drow = dtItemDetail.NewRow
                drow.Item("OD_IN4_Proj_no") = intProjNo
                drow.Item("OD_VC60_Object_name") = CType(gridrow.FindControl("txtObjName"), TextBox).Text
                drow.Item("OD_VC2000_Object_desc") = CType(gridrow.FindControl("txtObjDesc"), TextBox).Text
                drow.Item("OD_VC2000_Special_inst") = CType(gridrow.FindControl("txtSpecInst"), TextBox).Text
                drow.Item("OD_IN4_Inserted_By") = HttpContext.Current.Session("PropUserID")
                drow.Item("OD_DT8_Inserted_On") = DateTime.Now
                dtItemDetail.Rows.Add(drow)
            End If
        Next

    End Function


    Private Function PopulateObjectsGrid() As Boolean

        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter

        dtItemDetail = New DataTable
        Dim sqlQuery As String
        Dim row As DataRow
        sqlCon = New SqlConnection(SQL.DBConnection)

        sqlQuery = "Select OD_IN4_Obj_no, OD_IN4_Proj_no, OD_VC60_Object_name, OD_VC2000_Object_desc, OD_VC2000_Special_inst, OD_IN4_Inserted_By, OD_DT8_Inserted_On from T120011 where OD_IN4_Proj_no = " & intProjNo & " and OD_IN4_Row_no=" & intRowNo
        Try
            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtItemDetail)
            sqlCon.Close()
            'dtItemDetail.Columns.Add("IsExisted")

            If (dtItemDetail.Rows.Count < 1) Then
                CreateSingleRow()
            End If

            'mdvtable.Table = dtItemDetail
            dgObjectDetails.DataSource = dtItemDetail
            dgObjectDetails.DataBind()
            Return True

        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("Object_Assignment", "PopulateObjectsGrid-190", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        Finally
            sqlCon.Close()
        End Try

    End Function

    Private Sub CreateSingleRow()

        Dim drow As DataRow
        drow = dtItemDetail.NewRow
        drow.Item("OD_VC2000_Object_desc") = ""
        drow.Item("OD_VC2000_Special_inst") = ""
        dtItemDetail.Rows.Add(drow)

    End Sub

    Private Sub AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddRow.Click

        Try
            ReadObjectDetailsGrid()
            Call CreateSingleRow()
            dgObjectDetails.DataSource = dtItemDetail
            dgObjectDetails.DataBind()
        Catch ex As Exception
            CreateLog("Object_Assignment", "Click-216", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "AddRow")
            'lblErrMsg.Text = ex.ToString
            'lblErrMsg.Visible = True
        End Try

    End Sub


    Private Function SaveObjectInfo() As ReturnValue

        Dim stReturn As ReturnValue
        Dim con As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim sqlQuery As String = "Select OD_IN4_Proj_no,OD_IN4_Row_no,OD_VC60_Object_name, OD_VC2000_Object_desc, OD_VC2000_Special_inst, OD_IN4_Inserted_By, OD_DT8_Inserted_On from T120011 where OD_IN4_Proj_no = -1"
        Dim sqlAdPhone As SqlDataAdapter
        Dim dsdataset As DataSet = New DataSet
        Dim gridrow As DataGridItem
        Dim drow As DataRow

        If Not dsdataset.Tables("T120011") Is Nothing Then
            dsdataset.Tables("T120011").Clear()
        End If

        Try
            SQL.Delete("Object_Assignment", "SaveObjectInfo", "delete from t120011 where OD_IN4_Proj_no = " & intProjNo & " and OD_IN4_Row_no=" & intRowNo, SQL.Transaction.Serializable)
            con.Open()
            sqlAdPhone = New SqlDataAdapter(sqlQuery.Trim, con)
            sqlAdPhone.Fill(dsdataset, "T120011")
            Dim intdgrcount As Integer = dgObjectDetails.Items.Count

            If intdgrcount = 0 Then
                Exit Function
            End If

            For Each gridrow In dgObjectDetails.Items
                If (Not CType(gridrow.FindControl("txtObjName"), TextBox).Text = "") Then
                    drow = dsdataset.Tables("T120011").NewRow
                    drow.Item("OD_IN4_Proj_no") = intProjNo
                    drow.Item("OD_IN4_Row_no") = intRowNo
                    drow.Item("OD_VC60_Object_name") = CType(gridrow.FindControl("txtObjName"), TextBox).Text
                    drow.Item("OD_VC2000_Object_desc") = CType(gridrow.FindControl("txtObjDesc"), TextBox).Text
                    drow.Item("OD_VC2000_Special_inst") = CType(gridrow.FindControl("txtSpecInst"), TextBox).Text
                    drow.Item("OD_IN4_Inserted_By") = HttpContext.Current.Session("PropUserID")
                    drow.Item("OD_DT8_Inserted_On") = DateTime.Now
                    dsdataset.Tables("T120011").Rows.Add(drow)
                End If
            Next

            Dim getchanges As System.Data.DataTable = dsdataset.GetChanges.Tables("T120011")
            If (Not (getchanges Is Nothing)) Then    'add data in the database 
                Dim cmdb As SqlCommandBuilder = New SqlCommandBuilder(sqlAdPhone)
                sqlAdPhone.Update(dsdataset, "T120011")
            End If
            stReturn.ErrorCode = 0
            stReturn.FunctionExecuted = True
            stReturn.ErrorMessage = "Record saved successfully..."
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is  busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("Object_Assignment", "SaveobjectInfo-275", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return stReturn
        Finally
            con.Close()
        End Try

    End Function

    Private Function SaveFormsRecords() As Boolean

        mstGetFunctionValue = SaveObjectInfo()

        If mstGetFunctionValue.ErrorCode = 0 Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'cpnlError.Text = "Message"
            'cpnlError.Visible = True
            'ImgError.ImageUrl = "../images/Pok.gif"                  'Run time image change of Message panel
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            Return True
        ElseIf mstGetFunctionValue.ErrorCode = 1 Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'cpnlError.Text = "Message"
            'cpnlError.TitleCSS = "test3"
            'ImgError.ImageUrl = "../images/warning.gif"                  'Run time image change of Message panel
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            'cpnlError.Visible = True
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            Return False
        ElseIf mstGetFunctionValue.ErrorCode = 2 Then
            lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
            'cpnlError.Text = "Message"
            'cpnlError.TitleCSS = "test3"
            'ImgError.ImageUrl = "../images/warning.gif"                  'Run time image change of Message panel
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            'cpnlError.Visible = True
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            Return False
        End If

    End Function

    Private Sub dgObjectDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgObjectDetails.ItemDataBound

    End Sub
End Class
