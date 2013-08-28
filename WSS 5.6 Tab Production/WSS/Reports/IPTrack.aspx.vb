'************************************************************************************************************
' Page                 : - IPTrack
' Purpose              : - it will show the IP Tracking Report for the user 

' Date		    		   Author						Modification Date					Description
' 24/07/07			Suresh	Kharod     				             					        Created
'
' Notes: 
' Code:
'************************************************************************************************************
#Region "Refered Assemblies"

Imports ION.Data

Imports System.Web.Security
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data

#End Region

Partial Class Reports_IPTrack
    Inherits System.Web.UI.Page
    'Protected WithEvents crvIPTrack As CrystalDecisions.Web.CrystalReportViewer
#Region "Page Level Variables "

    Private ConStr As String = System.Configuration.ConfigurationSettings.AppSettings("ConnectionString")
    Private crDocument As ReportDocument
    Private objReports As clsReportData
    Public mstrCallNumber As String

#End Region

#Region "Page Load Functions "


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
            crvIPTrack.ToolbarStyle.Width = New Unit("860px")
            'HIDSCRID.Value = Request.QueryString("ScrID")
            'Security Block
            Dim intId As Integer
            If Not IsPostBack Then
                Dim str As String
                str = HttpContext.Current.Session("PropRootDir")
                intId = 907
                Dim obj As New clsSecurityCache
                If obj.ScreenAccess(intId) = False Then
                    Response.Redirect("../frm_NoAccess.aspx")
                End If
                obj.ControlSecurity(Me.Page, intId)
            End If
            'End of Security Block
            If Not IsPostBack Then
                fill_company()
                fill_employee(HttpContext.Current.Session("PropCompanyID"), 0, 2)
                FillIPCombo()
                mintflagIPTrack = Nothing
            End If
            Dim txthiddenImage = Request.Form("txthiddenImage")
            If Session("PropCompanyType") <> "SCM" Then
                ddlCompany.Enabled = False
            End If

            ' imgOK.Attributes.Add("OnClick", "ShowImg();")
            'ddlCompany.Attributes.Add("OnChange", "DDLChange(0, '" & ddlCompany.ClientID & "','" & ddlEmployee.ClientID & "');")

            If txthiddenImage = "Logout" Then
                LogoutWSS()
            End If

            If txthiddenImage <> "" Then
                Try
                    Select Case txthiddenImage
                        Case "Attach"
                            Response.Write("<script>window.open('../MessageCenter/UploadFiles/ViewAttachments.aspx?ID=AL','Attachments','scrollBars=yes,resizable=No,width=800,height=350,status=yes');</script>")
                        Case "OK"
                    End Select
                Catch ex As Exception
                End Try
            End If
            If mintflagIPTrack = Nothing Then
            Else
                ShowReport()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region " Fill Drop Down List Boxes"

    Private Function FillIPCombo()
        Try

            Dim strIP As String = System.Configuration.ConfigurationManager.AppSettings("IPLIST")
            Dim arrIP As New ArrayList
            For intI As Integer = 0 To strIP.Split(":").Length - 1
                arrIP.Add(strIP.Split(":")(intI))
            Next
            arrIP.Insert(0, "ALL")
            arrIP.Insert(arrIP.Count, "Other IPs")
            ddlIP.DataSource = arrIP
            ddlIP.DataBind()
        Catch ex As Exception

        End Try

    End Function

    Private Sub fill_company()
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.extractCompany(2)
            ddlCompany.DataSource = dt
            ddlCompany.DataTextField = "Name"
            ddlCompany.DataValueField = "ID"
            ddlCompany.DataBind()
            If ddlCompany.Items(0).Text <> "--ALL--" Then
                ddlCompany.Items.Insert(0, New ListItem("--ALL--", "0"))
            End If
            If HttpContext.Current.Session("PropCompanyID") <> Nothing Then
                ddlCompany.SelectedValue = HttpContext.Current.Session("PropCompanyID")
            End If
            If Request("cid") <> Nothing Then
                ddlCompany.SelectedValue = CInt(Request("cid"))
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub
    Private Sub fill_employee(ByVal companyID As String, ByVal projectID As Integer, ByVal category As Integer)
        Try
            Dim dt As New DataTable
            objReports = New clsReportData
            dt = objReports.ExtractEmployees(companyID)
            'dt = objReports.extractCustomer(companyID, projectID, category)
            ddlEmployee.DataSource = dt
            ddlEmployee.DataTextField = "Name"
            ddlEmployee.DataValueField = "AddressNo"
            ddlEmployee.DataBind()
            If ddlEmployee.Items(0).Text <> "--ALL--" Then
                ddlEmployee.Items.Insert(0, New ListItem("--ALL--", "0"))
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        Finally
            objReports = Nothing
        End Try
    End Sub
#End Region

#Region "Show Reprt"
    Private Function ShowReport() As Boolean
        Try
            crvIPTrack.ReportSource = Nothing

            crDocument = New ReportDocument
            Dim Reportpath As String
            Reportpath = Server.MapPath("rptIPTrack.rpt")
            crDocument.Load(Reportpath)


            'Dim rptTrack As New rptIPTrack
            'If IsPostBack Then
            '    FillAjaxDropDown(ddlEmployee, Request.Form("txthiddenOwner"), "cpnlRS:" & ddlEmployee.ID, New ListItem("--ALL--", 0))
            'End If
            Dim dtFrom As String
            Dim dtTo As String
            Dim intCallFrom, intCallTo As Integer
            intCallFrom = 0
            intCallTo = 0
            dtFrom = dtFromDate.Text
            dtTo = dtToDate.Text
            Dim dtToDate1 As DateTime
            Dim i As Integer
            'If dtFromDate.CalendarDate = Nothing Then
            '    dtFrom = Today
            'End If
            'If dtToDate.CalendarDate = Nothing Then
            '    dtTo = DateAdd("d", 0, Today)
            'ElseIf dtToDate.CalendarDate < dtFromDate.CalendarDate Then
            '    dtTo = dtFromDate.CalendarDate
            '    dtToDate.CalendarDate = dtTo
            'ElseIf dtToDate.CalendarDate < Today And dtFromDate.CalendarDate = Nothing Then
            '    dtTo = Today
            '    dtToDate.CalendarDate = dtTo
            'End If
            If dtFrom = Nothing And dtTo = Nothing Then
                lstError.Items.Clear()
                lstError.Items.Add("Please Select Date....")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            ElseIf dtFrom = Nothing And dtTo <> Nothing Then
                Dim From As String = "01/01/1932"
                dtFrom = CDate(From)
                'lstError.Items.Clear()
                'lstError.Items.Add("Please Select From Date... ")
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                'Exit Function

            ElseIf dtFrom <> Nothing And dtTo = Nothing Then
                If CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                Else
                    dtTo = Date.Now.ToShortDateString
                    lstError.Items.Clear()
                End If
                'lstError.Items.Clear()
                'lstError.Items.Add("Select To Date... ")
                'ShowMsgPenel(cpnlError, lstError, Image1, mdlMain.MSG.msgInfo)
                'Exit Function
            ElseIf dtFrom <> Nothing And dtTo <> Nothing Then
                If CDate(dtFrom) > CDate(dtTo) Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than To Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                ElseIf CDate(dtFrom) > Date.Now() Then
                    lstError.Items.Clear()
                    lstError.Items.Add("From Date can not be greater than Current Date... ")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    Exit Function
                End If
            Else
                lstError.Items.Clear()
            End If

            'dtToDate.CalendarDate = SetDateFormat(dtTo)
            'dtFromDate.CalendarDate = SetDateFormat(dtFrom)
            Dim dsTrack As New dsIPTrack
            Dim intCompID As Integer = ddlCompany.SelectedValue

            'For All IP's

            Dim strIP As String = System.Configuration.ConfigurationManager.AppSettings("IPLIST")
            Dim strList As String = ""

            For intI As Integer = 0 To strIP.Split(":").Length - 1
                strList &= "'" & strIP.Split(":")(intI) & "',"
            Next
            If Not strList.Equals("") Then
                strList = strList.Remove(strList.Length - 1, 1)
            End If
            Dim strSQL As String
            Dim intEmpID As Integer = ddlEmployee.SelectedValue
            Dim htCols As New Hashtable
            htCols.Add("LoginDate", 2)
            Select Case ddlIP.SelectedItem.Text.Trim.ToUpper
                Case "All".ToUpper
                    If Val(ddlCompany.SelectedValue) = 0 Then
                        If Val(ddlEmployee.SelectedValue) = 0 Then
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' order by LoginDate Desc,LoginTime Desc"
                        Else
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' AND  IP_NU9_User_ID_FK=" & intEmpID & " and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' order by LoginDate Desc,LoginTime Desc"
                        End If
                    Else
                        If Val(ddlEmployee.SelectedValue) = 0 Then
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' and IP_NU9_Comp_ID_FK=" & intCompID & " AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' order by LoginDate Desc,LoginTime Desc"
                        Else
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_User_ID_FK=" & intEmpID & " and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' and IP_NU9_Comp_ID_FK=" & intCompID & " AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' order by LoginDate Desc,LoginTime Desc"
                        End If
                    End If
                Case "Other IPs".ToUpper
                    If Val(ddlCompany.SelectedValue) = 0 Then
                        If Val(ddlEmployee.SelectedValue) = 0 Then
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP not in(" & strList & ") order by LoginDate Desc,LoginTime Desc"
                        Else
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_User_ID_FK=" & intEmpID & " and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP not in(" & strList & ") order by LoginDate Desc,LoginTime Desc"
                        End If
                    Else
                        If Val(ddlEmployee.SelectedValue) = 0 Then
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' and IP_NU9_Comp_ID_FK=" & intCompID & " AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP not in(" & strList & ") order by LoginDate Desc,LoginTime Desc"
                        Else
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_User_ID_FK=" & intEmpID & " and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' and IP_NU9_Comp_ID_FK=" & intCompID & " AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP not in(" & strList & ") order by LoginDate Desc,LoginTime Desc"
                        End If
                    End If
                Case Else
                    If Val(ddlCompany.SelectedValue) = 0 Then
                        If Val(ddlEmployee.SelectedValue) = 0 Then
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP='" & ddlIP.SelectedItem.Text.Trim & "' order by LoginDate Desc,LoginTime Desc"
                        Else
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_User_ID_FK=" & intEmpID & " and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP='" & ddlIP.SelectedItem.Text.Trim & "' order by LoginDate Desc,LoginTime Desc"
                        End If
                    Else
                        If Val(ddlEmployee.SelectedValue) = 0 Then
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' and IP_NU9_Comp_ID_FK=" & intCompID & " AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP='" & ddlIP.SelectedItem.Text.Trim & "' order by LoginDate Desc,LoginTime Desc"
                        Else
                            strSQL = "select T1.CI_VC36_Name EmpName,T2.CI_VC36_Name CompName,IP_VC36_System_IP SystemIP,ROM_VC50_Role_Name RoleName,Convert(varchar,IP_DT8_Login_Date,101) LoginDate ,Convert(varchar,IP_DT8_Login_Date,114) LoginTime From T060031,T010011 T1,T010011 T2,T070031 where IP_NU9_User_ID_FK=T1.CI_NU8_Address_Number and ROM_IN4_Role_ID_PK=IP_NU9_Role_ID_FK and IP_NU9_User_ID_FK=" & intEmpID & " and IP_NU9_Comp_ID_FK=T2.CI_NU8_Address_Number and T2.CI_VC8_Address_Book_Type='COM' and IP_NU9_Comp_ID_FK=" & intCompID & " AND  convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110) >='" & dtFrom & "'and convert(datetime,(convert(varchar,IP_DT8_Login_Date,101)),110)<='" & dtTo & "' and IP_VC36_System_IP='" & ddlIP.SelectedItem.Text.Trim & "' order by LoginDate Desc,LoginTime Desc"
                        End If
                    End If

            End Select
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Search("Table", "", "", strSQL, dsTrack, "", "") = True Then
                SetDataTableDateFormat(dsTrack.Tables(0), htCols)
                If dsTrack.Tables(0).Rows.Count > 0 Then
                Else
                    Dim dr As DataRow
                    dr = dsTrack.Tables(0).NewRow()
                    dr.Item("EmpName") = ""
                    dr.Item("CompName") = ""
                    dr.Item("SystemIP") = ""
                    dr.Item("RoleName") = ""
                    dr.Item("LoginDate") = ""
                    dr.Item("LoginTime") = ""
                    dsTrack.Tables(0).Rows.Add(dr)
                End If
            Else
                'Dim dr As DataRow
                'dr = dsTrack.Tables(0).NewRow()
                'dr.Item("EmpName") = ""
                'dr.Item("CompName") = ""
                'dr.Item("SystemIP") = ""
                'dr.Item("RoleName") = ""
                'dr.Item("LoginDate") = ""
                'dr.Item("LoginTime") = ""
                'dsTrack.Tables(0).Rows.Add(dr)
            End If
            'cpnlError.Visible = False
            'Dim dsIPTrack As New DataSet
            'dsIPTrack = dsTrack
            crDocument.SetDataSource(dsTrack)
            'rptTrack.SetDataSource(dsTrack)
            crvIPTrack.ReportSource = crDocument
            crvIPTrack.EnableDrillDown = False
            mintflagIPTrack = 1
        Catch ex As Exception

        End Try
    End Function
#End Region

    Private Sub imgOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOK.Click
        ShowReport()
    End Sub

    Private Sub ddlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        fill_employee(ddlCompany.SelectedValue, 0, 2)
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub

End Class
