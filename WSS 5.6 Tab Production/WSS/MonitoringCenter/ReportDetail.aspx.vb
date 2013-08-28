Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient

Partial Class MonitoringCenter_ReportDetail
    Inherits System.Web.UI.Page
    Shared mintID As Integer
    'Protected WithEvents cpnlDataBase As CustomControls.Web.CollapsiblePanel
    Private Shared dvSearch As New DataView

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        cpnlError.Visible = False

        If Not Page.IsPostBack Then
            'this function fill datagrid by selected company from table t130041
            BINDGrid()
        End If

        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Select Case txthiddenImage
                Case "Edit"
                    'pass Sqid to report name screen for update the record

                    Dim intID As Integer = Request.Form("txtHiddenID")
                    Response.Redirect("ReportName.aspx?ID=" & intID, False)

                Case "Delete"
                    'call delete function 
                    'If Request.Form("txtHiddenID") = "" Then
                    '    lstError.Items.Clear()
                    '    lstError.Items.Add("No Row Selected...")
                    '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    'Else

                    '    If DeleteRecord(Request.Form("txtHiddenID")) = True Then
                    '        lstError.Items.Clear()
                    '        lstError.Items.Add("Record deleted successfully...")
                    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    '        BINDGrid()
                    '        DgReportName.SelectedIndex = -1
                    '    End If
                    'End If
                    If DeleteRecord() = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Record deleted successfully...")
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        BINDGrid()
                    End If

                    'cpnlError.Visible = False


                Case "Add"
                    'call add to open reportname screen to add new record

                    Response.Redirect("ReportName.aspx", False)
            End Select
        End If
    End Sub

    ' this Function fill the Reportname Grid from table t130041 according to company selection   

    Private Function BINDGrid() As Boolean
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select RP_NU9_SQID_PK,RP_VC150_ReportName,RP_NU9_Domain_FK,RP_NU9_Machine_FK,RP_VC150_Release,a.MM_VC150_Machine_Name,b.DM_VC150_DomainName from t130041,T170012 a,T170011 b where RP_NU9_Machine_FK=a.MM_NU9_MID and RP_NU9_Domain_FK=b.DM_NU9_DID_PK  and RP_NU9_Domain_FK=a.MM_NU9_DID_FK and  RP_NU9_CompanyID_FK=" & Session("PropCAComp")
            SQL.Search("T130041 ", "report in people soft", "BINDGrid", sqstr, dsTemp, "", "")
            dvSearch = dsTemp.Tables(0).DefaultView
            DgReportName.DataSource = dvSearch
            DgReportName.DataBind()

        Catch ex As Exception
            CreateLog("dataobjectentry", "Bindgrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try

    End Function

    'this function delete recod from grid and from t130041

    Private Function DeleteRecord() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String


            Dim intSeqID As Integer = Request.Form("txthiddenID")
            sqstr = "delete from t130041 where  RP_NU9_SQID_PK=" & intSeqID
            If SQL.Delete("MONITORING", "deleterecod", sqstr, SQL.Transaction.Serializable, "") = True Then
                Return True
            Else

                Return False
            End If

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub DgReportName_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgReportName.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("ondblclick", "GridDBLClick(" & e.Item.Cells(1).Text.Trim & ");")
            e.Item.Attributes.Add("onclick", "GridClick(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(1).Text.Trim & ")")

        End If

    End Sub

    Private Sub DgReportName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DgReportName.SelectedIndexChanged
        mintID = DgReportName.SelectedItem.Cells(1).Text.Trim
    End Sub
    ''this will open the next screen to add new report details
    'Private Sub Imgbtnadd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    If ddlcompany.SelectedValue.Equals("Select") Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("select company......")
    '    Else
    '        Response.Redirect("report in people soft.aspx")
    '    End If
    'End Sub
    ''THIS FUNCTION FILL THE DROPDOWN LIST OF COMPANY
    'Private Function GetCompany() As Boolean
    '    Try
    '        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
    '        SQL.DBTracing = False
    '        Dim dsTemp As New DataSet
    '        Dim sqstr As String
    '        sqstr = "select CI_NU8_Address_Number,CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type='COM'"
    '        If SQL.Search("T010011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
    '            ddlcompany.DataSource = dsTemp.Tables(0)
    '            ddlcompany.DataTextField = "CI_VC36_Name"
    '            ddlcompany.DataValueField = "CI_NU8_Address_Number"
    '            ddlcompany.DataBind()
    '            ddlcompany.Items.Insert(0, "Select")
    '        Else
    '            Return False
    '        End If

    '    Catch ex As Exception
    '        CreateLog("Reportin peoplesoft", "FillCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
    '        Return Nothing
    '    End Try
    'End Function
    'Function bindgrid fill the grid according to company selection
    '*****************************************
    'Dim itemType As ListItemType = e.Item.ItemType
    'If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
    '    Return
    'Else
    '    Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
    '    e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
End Class
