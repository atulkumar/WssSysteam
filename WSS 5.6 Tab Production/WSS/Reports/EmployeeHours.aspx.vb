Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Partial Class Reports_EmployeeHours
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim intId As Integer
            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
            Me.Header.Controls.Add(meta)

            '###########################
            crvReport.ToolbarStyle.Width = New Unit("900px")
            If Not IsPostBack Then

                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                intId = 862
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intId) = False Then
                    Response.Redirect("../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intId)
                Dim dsTemp As New DataSet
                Dim SelectQuery As String
                If Session("PropCompanyType") = "SCM" Then
                    SelectQuery = "Select CI_nu8_address_number,Ci_vc36_name from t010011 where Ci_vc8_address_Book_type='COM' and CI_VC8_status='ENA'"
                Else '
                    SelectQuery = "Select CI_nu8_address_number,Ci_vc36_name from t010011 where Ci_vc8_address_Book_type='COM' and CI_VC8_status='ENA' and CI_nu8_address_number=" & Session("PropCompanyID") & ""
                End If
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

                SQL.Search("t010011", "WSSReportsD", "ExtractHours", SelectQuery, dsTemp, "jagmit", "sidhu")
                '**********************
                CheckBoxList1.DataSource = dsTemp.Tables(0).DefaultView
                CheckBoxList1.DataTextField = "Ci_vc36_name"
                CheckBoxList1.DataValueField = "CI_nu8_address_number"
                CheckBoxList1.DataBind()
                CheckBoxList1.EnableViewState = True
                RBtnPortrait.Checked = True
            End If
            If Session("PropCompanyType") <> "SCM" Then
                CheckBoxList1.Items(0).Selected = True
                CheckBoxList1.Enabled = False
            End If

            If IsPostBack Then
                crvReport.ToolbarStyle.Width = New Unit("800px")
                Showreport()
            End If

            HIDSCRID.Value = Request.QueryString("ScrID")

        Catch ex As Exception
            CreateLog("EmployeeHoursReport", "page_Load", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub
    Sub Showreport()
        Dim crActHoursLandscape As ReportDocument
        Dim crActHoursPortrait As ReportDocument
        Dim dsHRS As New DSHours
        Dim dtFrom As String
        Dim dtTo As String
        Dim intCount As Integer = 0
        dtFrom = dtFromDate.Text
        dtTo = dtToDate.Text

        Dim strCompnies As String = "("
        For intI As Integer = 0 To CheckBoxList1.Items.Count - 1
            If CheckBoxList1.Items(intI).Selected Then
                If intCount = 0 Then
                    strCompnies &= CheckBoxList1.Items(intI).Value
                    intCount += 1
                Else
                    strCompnies &= "," & CheckBoxList1.Items(intI).Value
                End If
            End If
        Next

        If intCount = 0 And dtFrom = Nothing And dtTo = Nothing Then
            lstError.Items.Clear()
            lstError.Items.Add("Please select company and Date ...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        ElseIf intCount <> 0 And dtFrom = Nothing And dtTo = Nothing Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select Date Range....")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        ElseIf intCount <> 0 And dtFrom = Nothing And dtTo <> Nothing Then
            Dim From As String = "01/01/1932"
            dtFrom = CDate(From)
        ElseIf intCount = 0 And dtFrom = Nothing And dtTo <> Nothing Then
            lstError.Items.Clear()
            lstError.Items.Add("Please Select Company ... ")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        ElseIf intCount <> 0 And dtFrom <> Nothing And dtTo = Nothing Then
            If CDate(dtFrom) > Date.Now() Then
                lstError.Items.Clear()
                lstError.Items.Add("Date From can not be greater than Current Date... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            Else
                dtTo = Date.Now.ToShortDateString
                lstError.Items.Clear()
            End If
        ElseIf intCount = 0 And dtFrom <> Nothing And dtTo = Nothing Then
            If CDate(dtFrom) > Date.Now() Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Company and From Date less than current date... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Company... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        ElseIf intCount <> 0 And dtFrom <> Nothing And dtTo <> Nothing Then
            If CDate(dtFrom) > CDate(dtTo) Then
                lstError.Items.Clear()
                lstError.Items.Add("Date From can not be greater than To Date... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            ElseIf CDate(dtFrom) > Date.Now() Then
                lstError.Items.Clear()
                lstError.Items.Add("Date From can not be greater than Current Date... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
        ElseIf intCount = 0 And dtFrom <> Nothing And dtTo <> Nothing Then
            lstError.Items.Clear()
            lstError.Items.Add("Please select company....")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        Else
            lstError.Items.Clear()
        End If
        strCompnies &= ")"


        Dim strSQL As String

        strSQL = "Select  am_nu9_comp_id_fk,Am_vc8_Supp_owner,(' ' + Ci_vc36_name) as company,('   ' + um_vc50_userid )as um_vc50_userid,isnull(sum(Am_FL8_Used_hr),0) as UsedHrs,isnull(sum(Am_fl8_BillHrs),0) as BillHrs, AM_DT8_Action_Date from t040031,t010011,t060011 where Am_nu9_comp_id_fk=t010011.Ci_nu8_address_number and Am_vc8_Supp_owner=Um_in4_address_no_fk  "


        If dtFrom <> "" Then
            strSQL &= " and convert(datetime,convert(varchar,am_dt8_action_date,101),101)  >= '" & CDate(dtFrom) & "' "
        Else
            strSQL &= " and convert(datetime,convert(varchar,am_dt8_action_date,101),101) ='" & Now & "'"
        End If

        If dtTo <> "" Then
            strSQL &= " and convert(datetime,convert(varchar,am_dt8_action_date,101),101)  <='" & CDate(dtTo) & "' "
        End If

        strSQL &= " and am_nu9_comp_id_fk in " & strCompnies & " "

        strSQL &= " group by am_nu9_comp_id_fk,Am_vc8_Supp_owner,Ci_vc36_name,um_vc50_userid,AM_DT8_Action_Date order by am_nu9_comp_id_fk"

        dsHRS = New DSHours
        SQL.Search("tbHours", "WSSReportsD", "ExtractHours", strSQL, dsHRS, "jagmit", "sidhu")

        If RBtnPortrait.Checked Then
            'crHoursPortrait = New CrEmpHoursPortrait
            crActHoursPortrait = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crCallTaskActionDS.rpt")
            crActHoursPortrait.Load(Reportpath)


            crActHoursPortrait = New ReportDocument
            Dim Reportpath1 As String
            Reportpath1 = Server.MapPath("crEmpHrsPortrait.rpt")
            crActHoursPortrait.Load(Reportpath1)

            'crActHoursPortrait = New crEmpHrsPortrait
            'crvReport.ReportSource = crHoursPortrait
            crActHoursPortrait.SetDataSource(dsHRS)
            crvReport.ReportSource = crActHoursPortrait
        Else
            crActHoursLandscape = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crEmpActHrsLanscape.rpt")
            crActHoursLandscape.Load(Reportpath)


            'crHours = New crEmpHoursLandScape
            'crActHoursLandscape = New crEmpActHrsLanscape
            crActHoursLandscape.SetDataSource(dsHRS)
            crvReport.ReportSource = crActHoursLandscape
        End If
        lstError.Items.Clear()
        'cpnlError.Visible = False
        crvReport.DataBind()
        'End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class
