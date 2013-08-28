#Region "NameSpace"
Imports System.Data
Imports System.Data.Sql
Imports ION.Net
Imports ION.Data
Imports ION.Logging.EventLogging
Imports WSSBLL
Imports Telerik.Web.UI.Calendar
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Data.SqlClient
Imports ION.Common.DAL
Imports Telerik.Web.UI
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
#End Region

Partial Class Reports_crSPOCcalldetailReport
    Inherits System.Web.UI.Page
    Private objSpocCallDetail As New clsSpocWeeklyBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)


#Region "Variables"
    Private rptDoc As New ReportDocument  'declare object to load Report
    Dim ds As New DataSet
#End Region
#Region "Page Load Function"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")

            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################


            If IsPostBack = False Then
                Dim objReports As New clsReportData
                fill_Department(ddlDepartment, "select distinct PI_VC8_Department from T010043  where PI_VC8_Department is not null  order by PI_VC8_Department ")
                fill_Subcategory(ddlSubCategories, "select PR_NU9_Project_ID_Pk as projectID ,PR_VC20_Name as ProjectName from T210011 order by ProjectName")
                fill_CallStatus(ddlCallStatus, "select distinct CN_VC20_Call_Status from t040011")
            Else
                If ViewState("Flag") = 1 Then
                    ShowReport()
                End If
            End If
            Dim txthiddenImage As String = Request.Form("txthiddenImage")
            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Save"

                    Case "Close"
                        Response.Redirect("../../Home.aspx", False)
                    Case "Logout"
                        LogoutWSS()
                        Exit Sub
                End Select
            End If
            'Security Block
            Dim intId As Integer
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                intId = 967
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(967) = False Then
                    Response.Redirect("../../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, 967)
            End If
            'End of Security Block
        Catch ex As Exception
            CreateLog("Crea", "Load-138", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, Session("PropUserID"), Session("PropUserName"), "NA", )
        End Try
    End Sub
#End Region
#Region "Fill Drop Down List Boxes"
    Private Sub fill_Department(ByVal ddlDepartment As DropDownList, ByVal strSql As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnstatus As Boolean
        Try
            sqRDR = SQL.Search("team", "FillNonUDCDropDown-1719", strSql, SQL.CommandBehaviour.Default, blnstatus)
            ddlDepartment.Items.Clear()

            If blnstatus = True Then
                While sqRDR.Read
                    ddlDepartment.Items.Add(New ListItem(sqRDR("PI_VC8_Department")))
                End While
                ddlDepartment.Items.Insert(0, New ListItem("--ALL--"))
                sqRDR.Close()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            ' objReports = Nothing
        End Try
    End Sub
    Private Sub fill_Subcategory(ByVal ddlsubcategory As DropDownList, ByVal strSql As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnstatus As Boolean
        Try
            sqRDR = SQL.Search("SubCategory", "FillNonUDCDropDown-1719", strSql, SQL.CommandBehaviour.Default, blnstatus)
            ddlsubcategory.Items.Clear()

            If blnstatus = True Then
                While sqRDR.Read
                    ddlsubcategory.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                ddlsubcategory.Items.Insert(0, New ListItem("--ALL--", 0))
                sqRDR.Close()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            ' objReports = Nothing
        End Try
    End Sub
    Private Sub fill_CallStatus(ByVal ddlCallStatus As DropDownList, ByVal strSql As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnstatus As Boolean
        Try
            sqRDR = SQL.Search("SubCategory", "FillNonUDCDropDown-1719", strSql, SQL.CommandBehaviour.Default, blnstatus)
            ddlCallStatus.Items.Clear()

            If blnstatus = True Then
                While sqRDR.Read
                    ddlCallStatus.Items.Add(New ListItem(sqRDR("CN_VC20_Call_Status")))
                End While

                ddlCallStatus.Items.Insert(0, New ListItem("--ALL--"))
                sqRDR.Close()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            ' objReports = Nothing
        End Try
    End Sub
#End Region
#Region "Function Show Report"
    Private Sub ShowReport()
        Dim ds1 As New DataSet
        Try
            Dim dt As New DataTable()
            ViewState("Flag") = 1
            ds = objSpocCallDetail.getSpocCallDetail(ddlDepartment.SelectedValue, ddlSubCategories.SelectedValue, dtFrom.SelectedDate, dtTODate.SelectedDate, ddlCallStatus.SelectedValue)
            If ds.Tables(0).Rows.Count > 0 Then

                lblMsg.Visible = False
                Dim dt2 As DataTable
                dt2 = SelectDistinct("Tbl", ds.Tables(0), "deptName")
                If dt2.Rows.Count > 0 Then

                    dt2.Columns.Add("Percentage")
                    dt2.Columns.Add("totalCalls")


                    Dim tempInt As Integer = 0
                    For i As Integer = 0 To dt2.Rows.Count - 1
                        For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(j)("deptName") = dt2.Rows(i)("deptName") Then
                                tempInt = tempInt + 1
                            End If
                        Next
                        Dim tempPercent As String

                        tempPercent = Convert.ToString((tempInt * 100) / ds.Tables(0).Rows.Count)
                        If (tempPercent = 100) Then
                            dt2.Rows(i)("Percentage") = tempPercent
                        Else
                            tempPercent = tempPercent + ".0000000"
                            dt2.Rows(i)("Percentage") = tempPercent.Substring(0, 5)
                        End If
                        dt2.Rows(i)("totalCalls") = tempInt
                        tempInt = 0
                    Next


                    ''''''''''
                    'Dim dtStatus As New DataTable
                    'dtStatus = SelectDistinct("TblStatus", ds.Tables(0), "Status")
                    'For s As Integer = 0 To dtStatus.Rows.Count - 1
                    '    dt2.Columns.Add("Status" & s)

                    '    For a As Integer = 0 To dt2.Rows.Count - 1
                    '        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    '            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                    '                If ds.Tables(0).Rows(b)("Status") = dtStatus.Rows(s)("Status") Then
                    '                    tempInt = tempInt + 1
                    '                End If
                    '            End If
                    '        Next
                    '        dt2.Rows(a)("Status" & s) = tempInt
                    '        tempInt = 0
                    '    Next

                    'Next


                    ''''''''''''''

                    dt2.Columns.Add("PROGRESS")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "PROGRESS" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("PROGRESS") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("DEFAULT")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "DEFAULT" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("DEFAULT") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("PCA")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "PCA" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("PCA") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("PENDING")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "PENDING" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("PENDING") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("HOLD")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "HOLD" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("HOLD") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("OPEN")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "OPEN" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("OPEN") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("ASSIGNED")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "ASSIGNED" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("ASSIGNED") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("VD")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "VD" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("VD") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("CLOSED")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "CLOSED" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("CLOSED") = tempInt
                        tempInt = 0
                    Next

                    dt2.Columns.Add("REOPEN")
                    For a As Integer = 0 To dt2.Rows.Count - 1
                        For b As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(b)("deptName") = dt2.Rows(a)("deptName") Then
                                If ds.Tables(0).Rows(b)("CN_VC20_Call_Status") = "REOPEN" Then
                                    tempInt = tempInt + 1
                                End If
                            End If
                        Next
                        dt2.Rows(a)("REOPEN") = tempInt
                        tempInt = 0
                    Next
                    Dim strReportPath As String 'To store the Report Path
                    rptDoc = New ReportDocument()
                    strReportPath = Server.MapPath("spocCallDetailReport.rpt") 'to store Report Path
                    rptDoc.Load(strReportPath)
                    rptDoc.SetDataSource(dt2)
                    crvReport.ReportSource = rptDoc
                    crvReport.Visible = True
                    crvReport.HasPageNavigationButtons = True
                End If
            Else
                lblMsg.Visible = True
                lblMsg.Text = "NO DATA !"
                crvReport.Visible = False
            End If
        Catch ex As Exception
            CreateLog("SpocCallDetail_RaimbursementPaidDetail", "btnShow_Click-125", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
#End Region
#Region "Functions"
    Public Function SelectDistinct(ByVal TableName As String, _
                       ByVal SourceTable As DataTable, _
                       ByVal FieldName As String) As DataTable
        Dim dt1 As New DataTable(TableName)
        dt1.Columns.Add(FieldName, SourceTable.Columns(FieldName).DataType)
        Dim dr As DataRow
        Dim LastValue As Object
        For Each dr In SourceTable.Select("", FieldName)
            If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, dr(FieldName)) Then
                LastValue = dr(FieldName)
                dt1.Rows.Add(New Object() {LastValue})
            End If
        Next
        If Not ds Is Nothing Then ds.Tables.Add(dt1)
        Return dt1
    End Function

    Private Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean
        '
        ' Compares two values to determine if they are equal. Also compares DBNULL.Value.
        '
        ' NOTE: If your DataTable contains object fields, you must extend this
        ' function to handle the fields in a meaningful way if you intend to group on them.
        '
        If A Is DBNull.Value And B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value Or B Is DBNull.Value Then Return False ' Only one is DBNull.Value.
        Return A = B                                                ' Value type standard comparison
    End Function
#End Region



#Region "btnShow_Click"
    Protected Sub imgOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        If dtFrom.SelectedDate Is Nothing OrElse dtTODate.SelectedDate Is Nothing Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select Date....")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        If CDate(dtFrom.SelectedDate) > CDate(dtTODate.SelectedDate) Then
            lstError.Items.Clear()
            lstError.Items.Add("From Date can not be greater than To Date... ")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If


        ShowReport()
        
    End Sub
    Protected Sub imgClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("../Home.aspx", False)
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
#End Region

End Class
