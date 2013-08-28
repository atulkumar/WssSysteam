'************************************************************************************************************
' Page                 : - jdInfo
' Purpose              : - it will show the call crDailyMonitoring report,crDailyMonitoring2 report                                     (one at a time)depends upon the value of query string passed .                                                
' Date		    		   Author						Modification Date					Description
' 28/03/06				   Atul 					             					        Created
'
' Notes: 
' Code:
'************************************************************************************************************
#Region "Reffered Assemblies"


Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

#End Region

Partial Class Reports_MonitorReports
    Inherits System.Web.UI.Page
    Private crDocument As New ReportDocument

#Region " Page Load Code"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try


            Dim txthiddenImage = Request.Form("txthiddenImage")
            If txthiddenImage = "Logout" Then
                LogoutWSS()
            End If
            'dtFromDate.LeftPos = 100
            'dtFromDate.TopPos = 30
            'dtToDate.LeftPos = 150
            'dtToDate.TopPos = 30

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "OK"
                    End Select
                Catch ex As Exception
                End Try
            End If


            If Request("ip") = Nothing Then
                Response.Redirect("MonitorIndex.aspx")
            ElseIf Request("ip").ToString = "a" Then

                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 778
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block


                'Response.Write("<head><title>" & "UBE MONITORING" & "</title></head>")
                cpnlReport.Text = "UBE MONITORING"
                cpnlRS.Text = "UBE MONITORING"
                lblHead.Text = "UBE MONITORING"
                Showreport(1)
            ElseIf Request("ip").ToString = "b" Then

                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 779
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block

                'Response.Write("<head><title>" & "DATABASE DETAILS" & "</title></head>")
                cpnlReport.Text = "DATABASE DETAILS"
                cpnlRS.Text = "DATABASE DETAILS"
                lblHead.Text = "DATABASE DETAILS"
                Showreport(2)
            ElseIf Request("ip").ToString = "c" Then

                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 781
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                'Response.Write("<head><title>" & "UBE MONITORING" & "</title></head>")
                cpnlReport.Text = "UBE MONITORING SUMMARY"
                cpnlRS.Text = "UBE MONITORING SUMMARY"
                lblHead.Text = "UBE MONITORING SUMMARY"
                Showreport(3)
            ElseIf Request("ip").ToString = "d" Then

                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 786
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                'Response.Write("<head><title>" & "QUES & REPORTS" & "</title></head>")
                cpnlReport.Text = "QUES & REPORTS"
                cpnlRS.Text = "QUES & REPORTS"
                lblHead.Text = "QUES & REPORTS"
                Showreport(4)
            ElseIf Request("ip").ToString = "e" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 787
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block

                'Response.Write("<head><title>" & "QUES & REPORTS" & "</title></head>")
                cpnlReport.Text = "QUES & REPORTS"
                cpnlRS.Text = "QUES & REPORTS"
                lblHead.Text = "QUES & REPORTS"
                Showreport(5)
            ElseIf Request("ip").ToString = "f" Then
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 786
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block
                'Response.Write("<head><title>" & "QUES & REPORTS" & "</title></head>")
                cpnlReport.Text = "Disk Report"
                cpnlRS.Text = "Disk Report"
                lblHead.Text = "Disk Report"
                Showreport(6)



            ElseIf Request("ip").ToString = "g" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 787
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'End of Security Block

                'Response.Write("<head><title>" & "QUES & REPORTS" & "</title></head>")
                cpnlReport.Text = "DISK REPORT"
                cpnlRS.Text = "DISK REPORT"
                lblHead.Text = "DISK  REPORT"
                Showreport(9)


            ElseIf Request("ip").ToString = "j" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 787
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)

                    'End of Security Block
                End If
                'Response.Write("<head><title>" & "QUES & REPORTS" & "</title></head>")
                cpnlReport.Text = "DISK REPORT"
                cpnlRS.Text = "DISK REPORT"
                lblHead.Text = "DISK  REPORT"
                Showreport(8)


            ElseIf Request("ip").ToString = "k" Then
                'Security Block
                Dim intId As Integer
                If Not IsPostBack Then
                    Dim str As String
                    str = HttpContext.Current.Session("PropRootDir")
                    intId = 787
                    Dim obj As New clsSecurityCache
                    If obj.ScreenAccess(intId) = False Then
                        Response.Redirect("../frm_NoAccess.aspx")
                    End If
                    obj.ControlSecurity(Me.Page, intId)
                End If
                'Response.Write("<head><title>" & "QUES & REPORTS" & "</title></head>")
                cpnlReport.Text = "DISK REPORT"
                cpnlRS.Text = "DISK REPORT"
                lblHead.Text = "DISK  REPORT"
                Showreport(9)

                'End of Security Block
            Else
                Response.Redirect("MonitorIndex.aspx")
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "Function ShowReport "


    Private Sub Showreport(ByVal id As Integer)
        Try
            Dim strRecordSelectionFormula As String
            Dim dtFrom As String
            Dim dtTo As String
            Dim i As Integer

            If dtFromDate.Text = Nothing Then
                i = Today.DayOfWeek
                dtFromDate.Text = SetDateFormat(Today.AddDays(0 - i))
            Else
                i = CType(dtFromDate.Text, DateTime).DayOfWeek
                Dim dt1 As DateTime = dtFromDate.Text
                dtFromDate.Text = dt1.AddDays(0 - i)
            End If
            If dtToDate.Text = Nothing Then
                i = Today.DayOfWeek
                dtToDate.Text = SetDateFormat(Today.AddDays(6 - i))
            Else
                i = CType(dtToDate.Text, DateTime).DayOfWeek
                Dim dt2 As DateTime = dtToDate.Text
                dtToDate.Text = dt2.AddDays(6 - i)
            End If

            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text

            If id = 1 Or id = 3 Then
                If id = 1 Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crDailyMonitoring.rpt")
                    crDocument.Load(Reportpath)

                ElseIf id = 3 Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crDailyMonitoring2.rpt")
                    crDocument.Load(Reportpath)

                End If

                If dtTo = Nothing And dtFrom = Nothing Then
                    '   dtFrom = Today
                    strRecordSelectionFormula += " {T130111.UH_DT8_URDate} in " & "#" & Today & "#" & "to #" & DateAdd("d", 1, Today) & "#"
                    ' strRecordSelectionFormula += " {T130111.UH_DT8_URDate} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {T130111.UH_DT8_URDate} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " {T130111.UH_DT8_URDate} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {T130111.UH_DT8_URDate} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If
                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                'crDocument.RecordSelectionFormula = "{T130111.UH_DT8_URDate} in " & "#03/09/2006#" & " to #03/23/2006#"

            ElseIf id = 2 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crMonitor2.rpt")
                crDocument.Load(Reportpath)

                If dtTo = Nothing And dtFrom = Nothing Then

                    '   dtFrom = Today
                    strRecordSelectionFormula += " {T130163.RS_DT8_RDate} in " & "#" & Today & "#" & "to #" & DateAdd("d", 1, Today) & "#"
                    ' strRecordSelectionFormula += " {T130163.RS_DT8_RDate} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {T130163.RS_DT8_RDate} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " {T130163.RS_DT8_RDate} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {T130163.RS_DT8_RDate} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If
                crDocument.RecordSelectionFormula = strRecordSelectionFormula

            ElseIf id = 4 Or id = 8 Then
                If id = 8 Then
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crDailyMonitoringBrgnDSK.rpt")
                    crDocument.Load(Reportpath)

                Else
                    crDocument = New ReportDocument
                    Dim Reportpath As String
                    Reportpath = Server.MapPath("crDailyMonitoringBrgn.rpt")
                    crDocument.Load(Reportpath)

                End If
                If dtTo = Nothing And dtFrom = Nothing Then
                    '   dtFrom = Today
                    strRecordSelectionFormula += " {Command.ResponseDateTime} in " & "#" & Today & "#" & "to #" & DateAdd("d", 1, Today) & "#"
                    ' strRecordSelectionFormula += " {T130111.UH_DT8_URDate} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {Command.ResponseDateTime} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " {Command.ResponseDateTime} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {Command.ResponseDateTime} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If
                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                ' crDocument.RecordSelectionFormula = "{T130111.UH_DT8_URDate} in " & "#03/09/2006#" & " to #03/23/2006#"
                crvReport.EnableDrillDown = False

                'FillreportMontBer()


            ElseIf id = 5 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crDailyMonitoringBrgnDetail.rpt")
                crDocument.Load(Reportpath)


                If Not Request("responseDate") = Nothing Then
                    strRecordSelectionFormula = "{Command_23.ResponseDateTime}=" & "#" & Request("responseDate") & "#"
                    crDocument.RecordSelectionFormula = strRecordSelectionFormula
                    crvReport.EnableDrillDown = False
                    cpnlRS.Visible = False
                End If
            ElseIf id = 6 Then
                FillreportMontBer()
                If Not Request("responseDate") = Nothing Then
                    crvReport.EnableDrillDown = False
                    cpnlRS.Visible = False
                End If
            ElseIf id = 7 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crDailyMonitoringBrgnDetail.rpt")
                crDocument.Load(Reportpath)


                If dtTo = Nothing And dtFrom = Nothing Then
                    '   dtFrom = Today
                    strRecordSelectionFormula += " {Command.ResponseDateTime} in " & "#" & Today & "#" & "to #" & DateAdd("d", 1, Today) & "#"
                    ' strRecordSelectionFormula += " {T130111.UH_DT8_URDate} <= " & "#" & DateAdd("d", 1, Today) & "#"
                ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {Command.ResponseDateTime} <= " & "#" & DateAdd("d", 1, dtTo) & "#"
                ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                    strRecordSelectionFormula += " {Command.ResponseDateTime} >= " & "#" & dtFrom & "#"
                ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                    strRecordSelectionFormula += " {Command.ResponseDateTime} in " & "#" & dtFrom & "#" & "to #" & DateAdd("d", 1, dtTo) & "#"
                End If
                crDocument.RecordSelectionFormula = strRecordSelectionFormula
                ' crDocument.RecordSelectionFormula = "{T130111.UH_DT8_URDate} in " & "#03/09/2006#" & " to #03/23/2006#"
                crvReport.EnableDrillDown = False

            ElseIf id = 9 Then
                FillreportMontBer()
                FillDiskSpace()
                'crDocument = New CrChkDiscSpace
                If Not Request("responseDate") = Nothing Then
                    crvReport.EnableDrillDown = False
                    cpnlRS.Visible = False
                End If
            End If

            crvReport.HasSearchButton = False
            clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.EnableDrillDown = False

            crvReport.DataBind()

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try


    End Sub
    Sub FillreportMontBer()
        Try
            Dim strSQL As String = "select RS_VC150_CAT4,RS_NU9_COMPANYID_FK,CI_VC36_Name as CompanyName,convert(datetime,  convert(varchar,convert(datetime, RS_VC100_RESPONSE_DATETIME,101), 101), 101) as ResponseDateTime  from T130023 left outer join t010011 on T130023.RS_NU9_COMPANYID_FK = t010011.CI_NU8_Address_Number where  RS_NU9_CompanyID_FK=" & Session("companyID") & " and RS_NU9_PROCESSID=10020012 order by ResponseDateTime "



            Dim dsTemp As New DataSet
            SQL.Search("TBL", "", "", strSQL, dsTemp, "", "")

            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text

            Dim strFilter As String
            If dtFrom <> "" Then
                strFilter = "ResponseDateTime >= '" & dtFrom & "'"
            End If
            If dtTo <> "" Then
                strFilter &= " and ResponseDateTime <='" & dtTo & "'"
            End If

            Dim dvTemp1 As New DataView
            dvTemp1 = dsTemp.Tables(0).DefaultView
            GetFilteredDataView(dvTemp1, strFilter)
            dsTemp.Tables.Clear()
            dsTemp.Tables.Add(dvTemp1.Table)

            Dim dtOut As New DataTable
            dtOut.Columns.Add("RS_VC150_CAT4", System.Type.GetType("System.String"))
            dtOut.Columns.Add("ResponseDateTime", System.Type.GetType("System.DateTime"))
            dtOut.Columns.Add("RS_NU9_COMPANYID_FK", System.Type.GetType("System.Int64"))
            dtOut.Columns.Add("CompanyName", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Sunday", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Monday", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Tuesday", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Wednesday", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Thursday", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Friday", System.Type.GetType("System.String"))
            dtOut.Columns.Add("Saturday", System.Type.GetType("System.String"))
            Dim dr As DataRow

            dr = dtOut.NewRow
            Dim dvTemp As New DataView
            Dim strStatus As String = "N/A"
            Dim intC As Integer = 0
            For intI As Integer = 0 To dsTemp.Tables(0).Rows.Count - 1
                dvTemp = dsTemp.Tables(0).DefaultView
                GetFilteredDataView(dvTemp, "ResponseDateTime='" & dsTemp.Tables(0).Rows(intI).Item("ResponseDateTime") & "'")
                intC = dvTemp.Table.Rows.Count
                If intC > 0 Then
                    GetFilteredDataView(dvTemp, "RS_VC150_CAT4='ER'")
                    If dvTemp.Table.Rows.Count > 0 Then
                        strStatus = "ER"
                    Else
                        strStatus = "NE"
                    End If
                Else
                    strStatus = "N/A"
                End If
                dr.Item("RS_VC150_CAT4") = dsTemp.Tables(0).Rows(intI).Item("RS_VC150_CAT4")
                dr.Item("ResponseDateTime") = dsTemp.Tables(0).Rows(intI).Item("ResponseDateTime")
                dr.Item("CompanyName") = dsTemp.Tables(0).Rows(intI).Item("CompanyName")
                dr.Item("RS_NU9_COMPANYID_FK") = dsTemp.Tables(0).Rows(intI).Item("RS_NU9_COMPANYID_FK")
                dr.Item(CType(dsTemp.Tables(0).Rows(intI).Item("ResponseDateTime"), DateTime).DayOfWeek.ToString) = strStatus

                If CType(dsTemp.Tables(0).Rows(intI).Item("ResponseDateTime"), DateTime).DayOfWeek = DayOfWeek.Saturday Then

                    dr.Item("ResponseDateTime") = dsTemp.Tables(0).Rows(intI).Item("ResponseDateTime")
                    dtOut.Rows.Add(dr)
                    dr = Nothing
                    dr = dtOut.NewRow
                ElseIf (intI + intC + 1) >= dsTemp.Tables(0).Rows.Count And CType(dsTemp.Tables(0).Rows(intI).Item("ResponseDateTime"), DateTime).DayOfWeek <> DayOfWeek.Saturday Then

                    dtOut.Rows.Add(dr)
                    dr = Nothing
                    dr = dtOut.NewRow
                End If
                intI = intI + intC - 1
            Next

            dtOut.AcceptChanges()
            Dim daset As New DataSet
            daset.Tables.Add(dtOut)
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crDailyMonitoringBrgn1.rpt")
            crDocument.Load(Reportpath)

            crDocument.SetDataSource(daset)
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
    Sub FillDiskSpace()
        Try
            Dim disc As String = "DSK"
            Dim FromDate As DateTime
            Dim ToDate As DateTime
            FromDate = dtFromDate.Text
            ToDate = dtToDate.Text

            'CDate(Request("responseDate"))

            'Dim fmtDate As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US", False)

            'Dim dd As String = String.Format("{0:dd/MM/yyyy}", System.Convert.ToDateTime(CDate(Request("responseDate")), fmtDate))

            'Format(dd, "dd-MM-yyyy")
            'DateTime.Parse(dd.ToLongTimeString, "dd/mm/yyyy", )


            Dim strSQL As String = "select RS_VC150_CAT2 as Drive,MM_VC150_Machine_Name,RS_NU9_COMPANYID_FK ,RS_NU9_MACHINE_CODE_FK,RS_NU9_DOMAIN_FK,RS_VC150_CAT3 as FreeSpace, RS_VC150_CAT5 as UsedSpace ,RS_VC150_CAT7 as IP ,RS_VC150_CAT6 as TotalSpace,Convert(datetime,convert(char(10),RS_VC100_RESPONSE_DATETIME,101),101) as ResponseDateTime from t130023,t170012 where RS_NU9_MACHINE_CODE_FK=MM_NU9_MID and RS_NU9_DOMAIN_FK=MM_NU9_DID_FK and RS_VC150_CAT1='" & disc & "' and Convert(datetime,convert(char(10),RS_VC100_RESPONSE_DATETIME,101),101) between '" & FromDate & "' and '" & ToDate & "' "
            ' Dim strSQL As String = "select RS_VC150_CAT2 as Drive,MM_VC150_Machine_Name,RS_NU9_COMPANYID_FK ,RS_NU9_MACHINE_CODE_FK,RS_NU9_DOMAIN_FK,RS_VC150_CAT3 as FreeSpace, RS_VC150_CAT5 as UsedSpace ,MM_VC100_Machine_IP as IP ,RS_VC150_CAT6 as TotalSpace,convert(datetime,  convert(varchar,convert(datetime, RS_VC100_RESPONSE_DATETIME,103), 103), 103) as ResponseDateTime from t130023,t170012 where RS_NU9_MACHINE_CODE_FK=MM_NU9_MID and RS_NU9_DOMAIN_FK=MM_NU9_DID_FK and substring(t130023.RS_VC150_CAT1,1,3) like '" & disc & "' and convert(char(10),convert(datetime,RS_VC100_RESPONSE_DATETIME,103),103)= '" & dd & "'"


            Dim dsDiscspace As New DsChkDiscSpace
            SQL.Search("TbChkDscSpace", "", "", strSQL, dsDiscspace, "", "")
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("CrChkDiscSpace.rpt")
            crDocument.Load(Reportpath)

            crDocument.SetDataSource(dsDiscspace)

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try


    End Sub

#End Region

#Region "All Other Functions "


    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        If Request("ip") = "e" Then
            Server.Transfer("../reports/monitorReports.aspx?ip=" & "d")

        End If
        Response.Redirect("../home.aspx")
    End Sub

    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
        HttpContext.Current.Session("PropCallNumber") = 0
        HttpContext.Current.Session("PropTaskNumber") = 0
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        crDocument.Dispose()
    End Sub

#End Region
End Class
