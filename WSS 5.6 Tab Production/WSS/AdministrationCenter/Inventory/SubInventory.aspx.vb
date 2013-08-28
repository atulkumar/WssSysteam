Imports ION.Data
Imports ION.Logging.EventLogging

Partial Class AdministrationCenter_Inventory_SubInventory
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        imgClose.Attributes.Add(" OnClick", "javascript:window.close();")
        cpnlInventory.Text = cpnlInventory.Text & "[ " & WSSSearch.SearchUserID(Val(Request.QueryString("EmpID"))).ExtraValue & " ]"
        Call BindInventoryGrid()
    End Sub
    Private Function BindInventoryGrid() As Boolean
        Try

            Dim strSql As String
            Dim intCount As Integer
            Dim blnStatus As Boolean
            Dim mdsInventory As New System.Data.DataSet

            strSql = "select InventoryIssueDetailID, Category, Itemname, inv.SerialNo, AllowedTill, inv.Status, Comments from inventoryissuedetail inv," & _
                        " itemmaster where itemid=id and IssueID_fk=" & Val(Request.QueryString("IssID"))

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            If SQL.Search("inventoryissuedetail", "AB_Main", "BindInventoryGrid-907", strSql, mdsInventory, "sachin", "Prashar") = True Then
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsInventory)
                grdInventory.DataSource = mdsInventory
                grdInventory.DataBind()
                cpnlInventory.Enabled = True
                If mdsInventory.Tables(0).Rows.Count > 0 Then

                End If
            Else
                HTMLEncodeDecode(mdlMain.Action.Encode, mdsInventory)
                grdInventory.DataSource = mdsInventory
                grdInventory.DataBind()
                cpnlInventory.Enabled = True
                If mdsInventory.Tables(0).Rows.Count > 0 Then

                End If
            End If

        Catch ex As Exception
            CreateLog("AB_Main", "BindInventoryGrid-900", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Function
End Class
