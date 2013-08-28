Imports System.Data
Imports System.Data.DataView
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
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_WeeklyTaskReport
    Inherits System.Web.UI.Page
    Private mdvtable As System.Data.DataView = New System.Data.DataView
    Private arCallNoToAdd As New ArrayList
    Private arTaskNoToAdd As New ArrayList
    Private arProjectIdToAdd As New ArrayList
    Private arTaskNoDelete As New ArrayList
    Private arCallNoToDelete As New ArrayList
    Private intCallNo As Integer
    Private intTaskNo As Integer
    Private intProjectId As Integer
    '[ Private objReports As New clsReportData

    Private rptdoc As New ReportDocument


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
                SetDatesWeekly(DateTime.Now)
                Dim objReports As New clsReportData
                Dim dsTeamAndTL As New DataSet
                Dim dtCompy As New DataTable

                Dim s As String
                objReports.extractCompany(1)
                s = getTeamAndTL(220)
                fillTeamDdl(ddlTeam, "Select MT_NU9_Team_ID_PK as TeamID,MT_VC16_Team_Name TeamName From T570011 Order By TeamName ")
                'dsTeamAndTL = objReports.getTeamAndTL(Session("PropUserID"))
                ' fill_Team()
                'ddlTeam.SelectedValue = dsTeamAndtl.Tables(0).Rows(0).Item("TE_NU9_TEAM_ID_FK")
                'If Session("PropCompanyType") = "SCM" Then
                '    'FillCompanyDdl(ddlCompany, "select CI_NU8_Address_Number as ID,CI_VC36_Name Name,CI_VC8_Status Status  from T010011 WHERE CI_VC8_Address_Book_Type ='COM' and CI_VC8_Status='ENA' and CI_NU8_Address_Number in (" & GetCompanySubQuery() & ")")
                '    'ddlCompany.SelectedValue = Session("PropCompanyID")
                '    'fill_company()
                '    ddlTeam_SelectedIndexChanged(New Object, New System.EventArgs)
                'Else
                '    ddlTeam.Items.Add(New ListItem(Session("PropCompany"), Session("PropCompanyID")))
                '    ddlTeam.SelectedValue = Session("PropCompanyID")
                '    ddlTeam.Enabled = False
                ' 'fillclientcomp()
                ' End If
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

    Private Sub fill_Team()
        Try
            Dim dt As New DataTable
            Dim objReports As New clsReportData
            dt = objReports.extractTeams()
            ddlTeam.DataSource = dt
            ddlTeam.DataTextField = "Teamame"
            ddlTeam.DataValueField = "TeamID"
            ddlTeam.DataBind()
            ddlTeam.Items.Insert(0, New ListItem("--ALL--", 0))
            'If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
            '    ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            'End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            ' objReports = Nothing
        End Try
    End Sub
    Private Function getTeamAndTL(ByVal empid As Integer)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("team&tl", "", "select TE_NU9_TEAM_ID_FK from T570012 WHERE TE_NU9_Employee_ID_FK=" & empid & "", SQL.CommandBehaviour.Default, blnStatus)
            Return sqRDR(1)
        Catch ex As Exception

        End Try
    End Function
    'fill team drop dwon
    Public Sub fillTeamDdl(ByVal ddlteam As DropDownList, ByVal strSql As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnstatus As Boolean
        Try
            sqRDR = SQL.Search("team", "FillNonUDCDropDown-1719", strSql, SQL.CommandBehaviour.Default, blnstatus)
            ddlteam.Items.Clear()

            If blnstatus = True Then
                While sqRDR.Read
                    ddlteam.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                ddlteam.Items.Insert(0, New ListItem("--Select--", 0))
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("team", "fillTeamDdl", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")

        End Try
    End Sub

    'fill company drop down 
    Public Sub FillCompanyDdl(ByVal ddlCustom As DropDownList, ByVal strSQL As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            'ddlCustom.Items.Add("--ALL--")
            ddlCustom.Items.Insert(0, New ListItem("--All--", 0))
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillCompanyDdl", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Sub

    Public Sub fiilTlDropDown(ByVal ddlTL As DropDownList, ByVal strSQL As String)
        Dim sqlRDR As SqlClient.SqlDataReader
        Dim blnstatus As Boolean
        Try
            sqlRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnstatus)

            ddlTL.Items.Clear()
            'ddlTL.Items.Add("--Select--")
            ddlTL.Items.Insert(0, New ListItem("--All--", 0))
            If blnstatus = True Then
                While sqlRDR.Read
                    ddlTL.Items.Add(New ListItem(sqlRDR(1), sqlRDR(0)))
                End While
                sqlRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillCompanyDdl", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Sub
    Protected Sub ddlTL_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTL.SelectedIndexChanged
        Try
            If ddlTL.SelectedValue <> 0 Then
                FillCompanyDdl(ddlTaskOwner, "select CI_NU8_Address_Number as EmployeeId,CI_VC36_Name as EmployeeName,(select CI_VC36_Name from T010011 where ReportedToId=CI_NU8_Address_Number) as teamlead from  T010011 inner join tblReportedTo on CI_NU8_Address_Number=EmpId inner join dbo.T010043 on CI_NU8_Address_Number=PI_NU8_Address_No where ReportedToId=" & ddlTL.SelectedValue & " and AttTeamId=" & ddlTeam.SelectedValue & " and  CI_VC8_Status='ENA' order by CI_VC36_Name")
            Else
                FillCompanyDdl(ddlTaskOwner, "select CI_NU8_Address_Number as EmployeeId,CI_VC36_Name as EmployeeName,(select CI_VC36_Name from T010011 where ReportedToId=CI_NU8_Address_Number) as teamlead from  T010011 inner join tblReportedTo on CI_NU8_Address_Number=EmpId inner join dbo.T010043 on CI_NU8_Address_Number=PI_NU8_Address_No where AttTeamId=" & ddlTeam.SelectedValue & " and  CI_VC8_Status='ENA' order by CI_VC36_Name")

            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillCompanyDdl", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Sub


    Protected Sub ddlTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTeam.SelectedIndexChanged
        Try
            fiilTlDropDown(ddlTL, "select TLEmpId,CI_VC36_Name from  tblAssignTL left join   T010011 on tblAssignTL.TLEmpId = T010011.CI_NU8_Address_Number where tblAssignTL.TeamId =" & ddlTeam.SelectedValue & " order by CI_VC36_Name")
            ddlTaskOwner_SelectedIndexChanged(New Object, New System.EventArgs)
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "ddlCompany_SelectedIndexChanged-211", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub


    Private objSpoc As New clsSpocWeeklyBLL(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString, System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName)

    Private Function FillView()
        Try
            Dim strPath As String

            strPath = Server.MapPath("WeekysCurrentTak.rpt")

            Dim dsTAskDetail As DataSet
            dsTAskDetail = objSpoc.getWeeklyTaskDetail(ddlTaskOwner.SelectedValue, dtDateTO.SelectedDate, dtDateFrom.SelectedDate, ddlTeam.SelectedValue, ddlTL.SelectedValue)
            Return dsTAskDetail
        Catch ex As Exception
            CreateLog("Create Weekly Schedule", "Fill_upperGridView", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function


    Protected Sub ddlTaskOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTaskOwner.SelectedIndexChanged
        Try
            grdTask.Visible = True
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "ddlTaskOwner_SelectedIndexChanged-272", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    Public Sub SetDatesWeekly(ByVal Selecteddate As DateTime)
        Dim dsSetDates As New DataSet
        Dim strselect As String = String.Empty
        strselect = "SELECT convert(datetime,'" & Selecteddate & "') - (DATEPART(DW, convert(datetime,'" & Selecteddate & "')) - 1) as 'Week Start Date'  ,convert(datetime,'" & Selecteddate & "') + (7 - DATEPART(DW,convert(datetime,'" & Selecteddate & "'))) as 'Week End Date' , DATEPART( wk, convert(datetime,'" & Selecteddate & "')) as 'Week No'"
        If SQL.Search("T040021", "GetDatesWeekly", "GetDatesWeekly", strselect, dsSetDates, "Anuj", "Kumar") Then
            dtDateFrom.SelectedDate = dsSetDates.Tables(0).Rows(0)("Week Start Date")
            dtDateTO.SelectedDate = dsSetDates.Tables(0).Rows(0)("Week End Date")
            lblweekno.Text = dsSetDates.Tables(0).Rows(0)("Week No")
        End If
    End Sub
    Protected Sub dtDateFrom_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs)
        SetDatesWeekly(dtDateFrom.SelectedDate)
    End Sub
    Protected Sub dtDateTo_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs)
        SetDatesWeekly(dtDateTO.SelectedDate)
    End Sub

    Protected Sub grdTask_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Try
            Dim dstaskDetail As DataSet
            dstaskDetail = FillView()
            dstaskDetail.Tables(0).DefaultView.RowFilter = "hours<>0"
            dstaskDetail.Tables(1).DefaultView.RowFilter = "hours<>0"
            If rdbCurrentWeeK.Checked = True Then
                grdTask.DataSource = dstaskDetail.Tables(0).DefaultView
            Else
                grdTask.DataSource = dstaskDetail.Tables(1).DefaultView
            End If
            grdTask.DataBind()
            cpnlSearch.State = CustomControls.Web.PanelState.Expanded
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub rdbCurrentWeeK_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbCurrentWeeK.CheckedChanged

    End Sub

    Protected Sub rdbPreviousWeek_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbPreviousWeek.CheckedChanged, rdbCurrentWeeK.CheckedChanged
        lblweekno.Text = lblweekno.Text - 1
    End Sub
    Protected Sub grdTask_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
        Try
            If ddlTeam.SelectedValue <> 0 Then
                Dim dstaskDetail As DataSet
                dstaskDetail = FillView()
                'Dim obj As DataView
                dstaskDetail.Tables(0).DefaultView.RowFilter = "hours<>0"
                dstaskDetail.Tables(1).DefaultView.RowFilter = "hours<>0"
                If rdbCurrentWeeK.Checked = True Then
                    grdTask.DataSource = dstaskDetail.Tables(0).DefaultView
                Else
                    grdTask.DataSource = dstaskDetail.Tables(1).DefaultView
                End If
                grdTask.DataBind()
                cpnlSearch.State = CustomControls.Web.PanelState.Expanded
                If rdbPreviousWeek.Checked = True Then
                    lblweekno.Text = lblweekno.Text - 1
                End If

            End If
          
        Catch ex As Exception

        End Try
       
    End Sub
    Protected Sub imgExportToExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgExportToExcel.Click
        Try
            grdTask.ExportSettings.IgnorePaging = True
            grdTask.Rebind()
            grdTask.MasterTableView.ExportToExcel()
        Catch ex As Exception
            CreateLog("CreateWeeklySchedule", "imgExportToExcel_Click-272", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

        End Try

    End Sub
End Class
