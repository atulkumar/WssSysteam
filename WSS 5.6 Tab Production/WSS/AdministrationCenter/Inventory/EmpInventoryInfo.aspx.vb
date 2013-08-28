Imports ION.Data
Imports ION.Logging.EventLogging

Partial Class AdministrationCenter_Inventory_EmpInventoryInfo
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

            strSql = "select ItemID, ItemGroup, ItemName,  Quantity, convert(varchar, IssueDate,101) IssueDate, case Returnable when 1 then 'Y' when 0 then 'N' end Returnable , convert(varchar, ExpectedReturnDate,101) ExpectedReturnDate, InventoryIssue.Status, Comments from InventoryIssue, ItemMaster where ID=ItemID and EmployeeID=" & Val(Request.QueryString("EmpID"))

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            If SQL.Search("InventoryIssue", "AB_Main", "BindInventoryGrid-907", strSql, mdsInventory, "sachin", "Prashar") = True Then
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
