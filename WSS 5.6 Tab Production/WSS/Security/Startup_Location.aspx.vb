#Region "Purpose"
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
#End Region

#Region "Session Used"
'Session("PropUserID")
'Session("PropUserName")
'Session("PropRole")
#End Region

Partial Class Security_Startup_Location
    Inherits System.Web.UI.Page
#Region "Varibles"
    Private Shared dvSearch As New DataView
#End Region

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
     
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            If IsPostBack = True Then
                'Code to Maintain the State of Radio button after postback
                Dim arr As New ArrayList
                Dim intK As Integer = 0
                CType(ViewState("dtStartLoc"), DataTable).Columns("Booked").ReadOnly = False
                Dim gridrow As DataGridItem
                For Each gridrow In dgControls.Items
                    If CType(gridrow.FindControl("rdDisable"), RadioButton).Checked Then
                        CType(ViewState("dtStartLoc"), DataTable).Rows.Find(dgControls.DataKeys(intK)).Item("Booked") = "True"
                    Else
                        If IsDBNull(dgControls.DataKeys(intK)) = False Then
                            CType(ViewState("dtStartLoc"), DataTable).Rows.Find(dgControls.DataKeys(intK)).Item("Booked") = "False"
                        End If

                    End If
                    intK += 1
                Next
            End If

            BINDGrid()

            If IsPostBack = False Then
                txtCSS(Me.Page)
                'code to checked the Radio button against Object ID
                Dim intScreenID As Integer = GetScreenID(HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropRole"))
                If intScreenID > 0 Then
                    Dim gridrow1 As DataGridItem
                    For Each gridrow1 In dgControls.Items
                        If intScreenID = gridrow1.Cells(1).Text Then
                            CType(gridrow1.FindControl("rdDisable"), RadioButton).Checked = True

                        End If
                    Next
                End If
            End If

            Dim strhiddenImage As String
            strhiddenImage = Request.Form("txthiddenImage")

            If strhiddenImage <> "" Then
                Select Case strhiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select
            End If
        Catch ex As Exception
            CreateLog("Security_Startup_Location", "Page-Load", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       23/04/2008
    'Purpose:           This function is used to show the various screen to set as home page acc to user and role
    '                         Table t170011
    'Modify Date:     28/04/2008
    '***************************************************************************************
    Private Function BINDGrid() As Boolean
        Try
            Dim dtClaimDet As New DataTable
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = " Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name, ROD_VC50_Alias_Name as AName, OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL,  OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType,  ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH,  OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName,convert(varchar,'False') Booked  from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA  WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND  UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND  RA.RA_VC4_Status_Code = 'ENB' AND  RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND  ROM.ROM_VC50_Status_Code_FK = 'ENB' AND  (((OBM.OBM_VC4_Object_Type_FK <>'MNU' and OBM.OBM_VC4_Object_Type_FK <>'SCR' ) and (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D')) OR  ((OBM.OBM_VC4_Object_Type_FK ='MNU' or OBM.OBM_VC4_Object_Type_FK ='SCR' ) and (ROD.ROD_CH1_View_Hide <> 'H'))) and ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND  OBM.OBM_VC4_Status_Code_FK = 'ENB' AND  OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and  ROM_IN4_Role_ID_PK =  " & HttpContext.Current.Session("PropRole") & " and OBM_Int4_HScreen=1 order by OBM.OBM_SI2_Order_By ; Select  distinct top 1 OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name, ROD_VC50_Alias_Name as AName, OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL,  OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType,  ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH,  OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName,convert(varchar,'False') Booked  from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA  WHERE OBM_IN4_Object_ID_PK=389"

            SQL.Search("T070011 ", "StartUp", "BINDGrid", sqstr, dsTemp, "", "")
            dsTemp.Tables(0).Merge(dsTemp.Tables(1))

            'dtClaimDet = DistinctRows(dsTemp.Tables(0), "ObjectID")

            'SQL.Search is true then we fill dvsearch Dataview with dsTemp(Dataset) for search
            dvSearch = dtClaimDet.DefaultView

            If IsNothing(ViewState("dtStartLoc")) = True Then
                dtClaimDet = dsTemp.Tables(0)
            Else
                dtClaimDet = CType(ViewState("dtStartLoc"), DataTable)
            End If
            dtClaimDet.AcceptChanges()

            Dim arDataCol(1) As DataColumn
            arDataCol(0) = dtClaimDet.Columns("ObjectID")
            dtClaimDet.PrimaryKey = arDataCol
            ViewState.Add("dtStartLoc", dtClaimDet)

            dgControls.DataSource = ViewState("dtStartLoc")
            dgControls.DataBind()
        Catch ex As Exception
            CreateLog("Security_Startup_Location", "Bindgrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function

    Public Function DistinctRows(ByVal dt As DataTable, ByVal keyfield As String) As DataTable
        Dim newTable As DataTable = dt.Clone
        Dim keyval As Int32 = 0
        Dim dv As DataView = dt.DefaultView
        dv.Sort = keyfield

        If dv.Table.Rows.Count > 0 Then
            For Each dr As DataRow In dv.Table.Rows
                If Not dr.Item(keyfield) = keyval Then
                    newTable.ImportRow(dr)
                    keyval = dr.Item(keyfield)
                End If
            Next
        Else
            newTable = dt.Clone
        End If

        Return newTable
    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click

        Dim intScreenID As Integer
        For Each item As DataGridItem In Me.dgControls.Items
            If CType(item.FindControl("rdDisable"), RadioButton).Checked Then
                intScreenID = item.Cells(1).Text
            End If
        Next
        Dim UserID As Integer = HttpContext.Current.Session("PropUserID")
        Dim RoleID As Integer = HttpContext.Current.Session("PropRole")

        Dim intID As Integer = GetID(UserID, RoleID)
        If intID > 0 Then
            'Function to Update Record
            If UpdateScreenID(intScreenID, intID) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Record Updated successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End If
        Else
            'Function to Save Record
            If SaveStartInfo(intScreenID, UserID, RoleID) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Record saved successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End If
        End If
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       28/04/2008
    'Purpose:           This function is used to update Records
    '                         Table t170051
    'Modify Date:     29/04/2008
    '***************************************************************************************
    Private Function UpdateScreenID(ByVal screenID As Integer, ByVal ID As Integer) As Boolean
        Try
            'it hold Update SqlQuery
            Dim sqlUpdate As String
            'get connectionstrig from Web config
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqlUpdate = "update T070051 set UL_NU9_OBJ_ID=" & screenID & " where UL_NU9_ID_PK= " & ID
            If SQL.Update("Startup_Location", "UpdateScreenID", sqlUpdate, SQL.Transaction.Serializable) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Security_Startup_Location", "UpdateScreenID-197", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try
        'End If
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       23/04/2008
    'Purpose:           This function is used to Save information of StartUp Page
    '                         Table t170051
    'Modify Date:     28/04/2008
    '***************************************************************************************
    Private Function SaveStartInfo(ByVal ScreenID As Integer, ByVal UserID As Integer, ByVal RoleID As Integer) As Boolean
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            'define column name
            arColName.Add("UL_NU9_OBJ_ID") ' Object ID
            arColName.Add("UL_NU9_User_ID") 'User ID
            arColName.Add("UL_NU9_Role_ID") 'Role ID

            arRowData.Add(ScreenID)
            arRowData.Add(UserID)
            arRowData.Add(RoleID)
            SQL.Save("T070051", "StartLocation", "Save", arColName, arRowData)

            Return True
        Catch ex As Exception
            CreateLog("Startup_Location", "SaveStartInfo-248", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
            Return False
        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       28/04/2008
    'Purpose:           This function is used to get Screen ID 
    '                         Table t170051
    'Modify Date: 
    '***************************************************************************************
    Private Function GetScreenID(ByVal UserID As Integer, ByVal RoleID As Integer) As Integer
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim strSQL As String = "Select UL_NU9_OBJ_ID from t070051 where UL_NU9_User_ID=" & UserID & " and UL_NU9_Role_ID= " & RoleID

            Dim intScreenID As Integer = SQL.Search("StartLocation", "GetScreenID", strSQL)
            Return intScreenID
        Catch ex As Exception
            CreateLog("Startup_Location", "GetScreenID-263", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       28/04/2008
    'Purpose:           This function is used to get Object ID to update Record or Save record
    '                         Table t170051
    'Modify Date: 
    '***************************************************************************************
    Private Function GetID(ByVal UserID As Integer, ByVal RoleID As Integer) As Integer
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim strSQL As String = "Select UL_NU9_ID_PK from t070051 where UL_NU9_User_ID=" & UserID & " and UL_NU9_Role_ID= " & RoleID

            Dim intObjectID As Integer = SQL.Search("StartLocation", "GetID", strSQL)
            Return intObjectID
        Catch ex As Exception
            CreateLog("Startup_Location", "GetID-284", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
