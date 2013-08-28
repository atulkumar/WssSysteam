Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports System.Text
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data
Partial Class Reports_ReportsPages_SLAReport
    Inherits System.Web.UI.Page
#Region "Page Level Variables"
    Dim crDocument As New ReportDocument
    Private objReports As clsReportData
    Private params As ParameterFieldDefinitions
    Private paramValues As ParameterValues
    Private paramCallNo As ParameterFieldDefinition
    Private paramCustomerID As ParameterFieldDefinition
    Private paramDate As ParameterFieldDefinition
    Private paramDiscrete As ParameterDiscreteValue
    Private paramRangeValue As ParameterRangeValue
    Private formulas As FormulaFieldDefinitions
    Public formulaServerName As FormulaFieldDefinition
    Private strServerName As String
    Public mstrCallNumber As String
    Dim dtFrom As String
    Dim dtTo As String
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Put user code to initialize the page here

            'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
            '###########################
            Dim meta As HtmlMeta
            meta = New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
            Me.Header.Controls.Add(meta)
            '###########################
            crvReport.ToolbarStyle.Width = New Unit("1000px")
            Dim txthiddenImage = Request.Form("txthiddenImage")

            If txthiddenImage = "Logout" Then
                FormsAuthentication.SignOut()
                Call ClearVariables()
                Response.Redirect("../Login/Login.aspx")
            End If
            If Not IsPostBack Then
                imgOK.Attributes.Add("OnClick", "ShowImg();")
                fill_company()
                fill_Project(HttpContext.Current.Session("PropCompanyID"))
                fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                fill_AssignedBy(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                fill_CallStatus(HttpContext.Current.Session("PropCompanyID"))
                fill_TaskStatus(HttpContext.Current.Session("PropCompanyID"))
            Else
                ShowReport()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#Region "Fill Functions"

    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCompany(2)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()


            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If

        Catch ex As Exception

            Dim str As String = ex.Message.ToString


        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_CallStatus(ByVal comapnyID As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCallStatus(comapnyID)
            DDLCallStatus.DataSource = dt
            DDLCallStatus.DataTextField = "CallStatus"
            DDLCallStatus.DataValueField = "CallStatus"
            DDLCallStatus.DataBind()
            DDLCallStatus.Items.Insert(0, New ListItem("--ALL--", 0))

            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_TaskStatus(ByVal comapnyID As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractTaskStatus(comapnyID)
            DDLTaskStatus.DataSource = dt
            DDLTaskStatus.DataTextField = "TaskStatus"
            DDLTaskStatus.DataValueField = "TaskStatus"
            DDLTaskStatus.DataBind()
            DDLTaskStatus.Items.Insert(0, New ListItem("--ALL--", 0))

            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_Project(ByVal comapnyID As Integer)
        Try

            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractProject(comapnyID)
            ddlProject.DataSource = dt
            ddlProject.DataTextField = "Name"
            ddlProject.DataValueField = "ID"
            ddlProject.DataBind()
            If ddlProject.Items(0).Text <> "--ALL--" Then
                ddlProject.Items.Insert(0, "--ALL--")
            End If
        Catch ex As Exception
            'Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)

        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractTaskOwner(companyID, projectID, 2)
            ddlEmployee.DataSource = dt
            ddlEmployee.DataTextField = "Name"
            ddlEmployee.DataValueField = "AddressNo"
            ddlEmployee.DataBind()
            ddlEmployee.Items.Insert(0, "--ALL--")
            dt = Nothing

            dt = New DataTable
            dt = objReports.extractCallOwner(companyID, projectID, 2)
            ddlCallReqBy.DataSource = dt
            ddlCallReqBy.DataTextField = "Name"
            ddlCallReqBy.DataValueField = "AddressNo"
            ddlCallReqBy.DataBind()
            ddlCallReqBy.Items.Insert(0, "--ALL--")

        Catch ex As Exception
            Dim str As String = ex.Message.ToString

        Finally
            objReports = Nothing
        End Try
    End Sub

    Private Sub fill_AssignedBy(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Dim dt As New DataTable
        objReports = New clsReportData
        dt = objReports.extractCallOwner(companyID, projectID, category)
        ddlCallReqBy.DataSource = dt
        ddlCallReqBy.DataTextField = "Name"
        ddlCallReqBy.DataValueField = "AddressNo"
        ddlCallReqBy.DataBind()
        ddlCallReqBy.Items.Insert(0, "--ALL--")
    End Sub

#End Region


    Sub ShowReport()
        Try
            crvReport.ReportSource = Nothing

            Dim strSQL As String = ""
            Dim dtFrom As String = dtFromDate.Text
            Dim dtTo As String = dtToDate.Text
            If dtFrom = Nothing And dtTo = Nothing Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Date....")
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                Dim From As String = "01/01/1932"
                dtFrom = CDate(From)

            ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                If CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                Else
                    dtTo = Date.Now.ToShortDateString
                    lstError.Items.Clear()
                End If
            ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                If CDate(dtFrom) > CDate(dtTo) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than To Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                ElseIf CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Sub
                End If
            Else
                lstError.Items.Clear()
            End If

            'crDocument = New crcall
            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("crSALReport.rpt")
            crDocument.Load(Reportpath)
            Dim dsSAL As New DataSet
            Dim strFilter As String = ""
            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text

            Dim htCols As New Hashtable
            htCols.Add("ActionDate", 2)

            If Session("PropAdmin") = "1" Then
                strSQL = "Select TM_NU9_Call_No_FK CallNo,CM_VC8_Call_Type CallType,CM_VC200_Work_Priority Priority,CM_VC2000_Call_Desc CallDescription,TM_NU9_Task_no_PK TaskNo,TM_VC8_task_type TaskType,TM_VC1000_Subtsk_Desc TaskDesc,T040021.TM_DT8_Task_Date TaskAssTime,TM_FL8_Est_Hr EstimatedTime,AM_DT8_Action_Date TaskStartTime,AM_DT8_Action_Date_Auto AS TaskStartTime1,'' ResponseTime,TM_DT8_Close_Date CloseTime,'' TotalTimeTaken,TM_VC50_Deve_status TaskStatus From T040021,T040011,T040031 where T040021.TM_NU9_Call_No_FK=T040011.CM_NU9_Call_No_PK and T040021.TM_NU9_Call_No_FK=T040031.AM_NU9_Call_Number and T040011.CM_NU9_Call_No_PK=T040031.AM_NU9_Call_Number and AM_NU9_Action_Number=1 and T040021.TM_NU9_Task_no_PK=T040031.AM_NU9_Task_Number and T040021.TM_NU9_Comp_ID_FK=T040011.CM_NU9_Comp_Id_FK and T040021.TM_NU9_Comp_ID_FK=T040031.AM_NU9_Comp_ID_FK and T040011.CM_NU9_Comp_Id_FK=T040031.AM_NU9_Comp_ID_FK and TM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & " "

            Else
                strSQL = "Select TM_NU9_Call_No_FK CallNo,CM_VC8_Call_Type CallType,CM_VC200_Work_Priority Priority,CM_VC2000_Call_Desc CallDescription,TM_NU9_Task_no_PK TaskNo,TM_VC8_task_type TaskType,TM_VC1000_Subtsk_Desc TaskDesc,T040021.TM_DT8_Task_Date TaskAssTime,TM_FL8_Est_Hr EstimatedTime,AM_DT8_Action_Date TaskStartTime,AM_DT8_Action_Date_Auto AS TaskStartTime1,'' ResponseTime,TM_DT8_Close_Date CloseTime,'' TotalTimeTaken,TM_VC50_Deve_status TaskStatus From T040021,T040011,T040031 where T040021.TM_NU9_Call_No_FK=T040011.CM_NU9_Call_No_PK and T040021.TM_NU9_Call_No_FK=T040031.AM_NU9_Call_Number and T040011.CM_NU9_Call_No_PK=T040031.AM_NU9_Call_Number and AM_NU9_Action_Number=1 and T040021.TM_NU9_Task_no_PK=T040031.AM_NU9_Task_Number and T040021.TM_NU9_Comp_ID_FK=T040011.CM_NU9_Comp_Id_FK and T040021.TM_NU9_Comp_ID_FK=T040031.AM_NU9_Comp_ID_FK and T040011.CM_NU9_Comp_Id_FK=T040031.AM_NU9_Comp_ID_FK and TM_NU9_Comp_ID_FK=" & ddlCompany.SelectedValue & " "
                'Select TM_NU9_Call_No_FK CallNo,CM_VC8_Call_Type CallType,CM_VC200_Work_Priority Priority,CM_VC2000_Call_Desc CallDescription,TM_NU9_Task_no_PK TaskNo,TM_VC8_task_type TaskType,TM_VC1000_Subtsk_Desc TaskDesc,convert(varchar,T040021.TM_DT8_Task_Date,101) TaskAssTime,TM_FL8_Est_Hr EstimatedTime,AM_DT8_Action_Date TaskStartTime,'' ResponseTime,TM_DT8_Close_Date CloseTime,'' TotalTimeTaken,TM_VC50_Deve_status TaskStatus From T040021,T040011,T040031 where T040021.TM_NU9_Call_No_FK=T040011.CM_NU9_Call_No_PK and T040021.TM_NU9_Call_No_FK=T040031.AM_NU9_Call_Number and T040011.CM_NU9_Call_No_PK=T040031.AM_NU9_Call_Number and AM_NU9_Action_Number=1 and T040021.TM_NU9_Task_no_PK=T040031.AM_NU9_Task_Number and T040021.TM_NU9_Comp_ID_FK=T040011.CM_NU9_Comp_Id_FK and T040021.TM_NU9_Comp_ID_FK=T040031.AM_NU9_Comp_ID_FK and T040011.CM_NU9_Comp_Id_FK=T040031.AM_NU9_Comp_ID_FK and TM_NU9_Comp_ID_FK=8 and convert(datetime,convert(varchar,T040021.TM_DT8_Task_Date,102))>= '2008-02-04'and convert(datetime,convert(varchar,T040021.TM_DT8_Task_Date,102))<='2008-02-04'and T040021.TM_VC8_Supp_Owner=59 and T040021.TM_VC50_Deve_status='CLOSED'and T040021.TM_NU9_Project_ID=16 and T040011.CM_VC100_By_Whom=59 and T040011.CN_VC20_Call_Status='CLOSED' Order by CallNo,TaskNo,TaskAssTime

            End If
            If ddlProject.SelectedIndex <> -1 Then
                If ddlProject.SelectedItem.Text.Trim <> "--ALL--" Then
                    strFilter += " and T040021.TM_NU9_Project_ID=" & ddlProject.SelectedItem.Value.Trim & ""
                End If
            End If
            If ddlEmployee.SelectedIndex <> -1 Then
                If ddlEmployee.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and T040021.TM_VC8_Supp_Owner='" & ddlEmployee.SelectedItem.Value.Trim & "'"
                End If
            End If
            If ddlCallReqBy.SelectedIndex <> -1 Then
                If ddlCallReqBy.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and T040011.CM_VC100_By_Whom='" & ddlCallReqBy.SelectedItem.Value.Trim & "'"
                End If
            End If
            If DDLTaskStatus.SelectedIndex <> -1 Then
                If DDLTaskStatus.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and T040021.TM_VC50_Deve_status='" & DDLTaskStatus.SelectedItem.Text & "'"
                End If
            End If
            If DDLCallStatus.SelectedIndex <> -1 Then
                If DDLCallStatus.SelectedItem.Text <> "--ALL--" Then
                    strFilter += " and T040011.CN_VC20_Call_Status='" & DDLCallStatus.SelectedItem.Text & "'"
                End If
            End If
            If dtFrom <> Nothing Then
                If IsDate(dtFrom) = True Then
                    strFilter += "  and convert(datetime,convert(varchar,T040021.TM_DT8_Task_Date,102)) >='" & CDate(dtFrom) & "'"
                End If
            End If
            If dtTo <> Nothing Then
                If IsDate(dtTo) = True Then
                    strFilter += " and convert(datetime,convert(varchar,T040021.TM_DT8_Task_Date,102)) <='" & CDate(dtTo) & "'"
                End If
            End If
            strFilter += "  Order by CallNo,TaskNo,TaskAssTime"
            strSQL += strFilter
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("SALTable", "WSSReportsD", "ExtractCallNo", strSQL, dsSAL, "Suresh", "Kharod")
            SetDataTableDateFormat(dsSAL.Tables(0), htCols)
            Dim intRespTime As Integer
            Dim intTimeTaken As Integer
            For intI As Integer = 0 To dsSAL.Tables(0).Rows.Count - 1
                intRespTime = 0
                intTimeTaken = 0
                If Not dsSAL.Tables(0).Rows(intI).Item("TaskStartTime") Is DBNull.Value Then
                    intRespTime = DateDiff(DateInterval.Second, CType(dsSAL.Tables(0).Rows(intI).Item("TaskAssTime"), DateTime), CType(dsSAL.Tables(0).Rows(intI).Item("TaskStartTime"), DateTime))
                    If intRespTime >= 0 Then
                        dsSAL.Tables(0).Rows(intI).Item("ResponseTime") = GetTime(intRespTime)
                    Else
                        intRespTime = 0
                        'intRespTime = DateDiff(DateInterval.Second, CType(dsSAL.Tables(0).Rows(intI).Item("TaskAssTime"), DateTime), CType(dsSAL.Tables(0).Rows(intI).Item("TaskStartTime1"), DateTime))
                        dsSAL.Tables(0).Rows(intI).Item("ResponseTime") = GetTime(intRespTime)
                    End If
                End If
                If Not dsSAL.Tables(0).Rows(intI).Item("CloseTime") Is DBNull.Value Then
                    intTimeTaken = DateDiff(DateInterval.Second, CType(dsSAL.Tables(0).Rows(intI).Item("TaskStartTime"), DateTime), CType(dsSAL.Tables(0).Rows(intI).Item("CloseTime"), DateTime))
                    dsSAL.Tables(0).Rows(intI).Item("TotalTimeTaken") = GetTime(intTimeTaken)
                End If
            Next
            'cpnlError.Visible = False
            crDocument.SetDataSource(dsSAL)
            crvReport.EnableDrillDown = True
            cpnlReport.State = CustomControls.Web.PanelState.Expanded
            clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub
    'Function to convert the Seconds in HH:mm:ss Format to get the total time

    Public Function GetTime(ByVal intSeconds As Long) As String
        Try
            Dim strTime As String = ""
            Dim intH As Long = 0
            Dim intM As Long = 0
            Dim intS As Long = 0
            If intSeconds >= 3600 Then
                Dim intTemp As Long
                intH = System.Math.DivRem(intSeconds, 3600, intTemp)
                If intTemp >= 60 Then
                    intM = System.Math.DivRem(intTemp, 60, intS)
                Else
                    intM = 0
                    intS = intTemp
                End If
            Else
                intH = 0
                If intSeconds >= 60 Then
                    intM = System.Math.DivRem(intSeconds, 60, intS)
                Else
                    intM = 0
                    intS = intSeconds
                End If
            End If
            strTime = IIf(intH.ToString.Length = 1, "0" & intH.ToString, intH.ToString) & ":" & IIf(intM.ToString.Length = 1, "0" & intM.ToString, intM.ToString) & ":" & IIf(intS.ToString.Length = 1, "0" & intS.ToString, intS.ToString)
            Return strTime
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
            Return ""
        End Try
    End Function
    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Dim intcompanyId As Integer = Val(ddlCompany.SelectedValue)
        fill_Project(intcompanyId)
        fill_employee(intcompanyId, 0, 2)
        fill_AssignedBy(intcompanyId, 0, 2)
    End Sub

    Private Sub imgOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        ShowReport()
    End Sub

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
