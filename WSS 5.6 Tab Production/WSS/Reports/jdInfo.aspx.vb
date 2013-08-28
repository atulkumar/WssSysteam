'************************************************************************************************************
' Page                 : - jdInfo
' Purpose              : - it will show the call crMachineOverview report,crSystemOverview,                                             crMachineOnlineStatus,report (one at a time)depends upon the value of query string                           passed.
' Date		    		   Author						Modification Date					Description
' 01/05/06				   Atul 					        24/07/2007(Suresh)	        Created
'
' Notes: Create new reports for the Online machine Status and Machine Information
' Code:
'************************************************************************************************************
#Region " Reffered Assemblies"


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
Partial Class Reports_jdInfo
    Inherits System.Web.UI.Page
    Private ConStr As String = System.Configuration.ConfigurationSettings.AppSettings("ConnectionString")
#Region " Page Load Event "


    Private crDocument As New ReportDocument

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim txthiddenImage = Request.Form("txthiddenImage")

        HIDSCRID.Value = Request.QueryString("ScrID")
        If txthiddenImage = "Logout" Then
            LogoutWSS()
        End If
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "OK"
                End Select
            Catch ex As Exception
            End Try
        End If
        Showreport()
    End Sub
#End Region

#Region "Function ShowReport To Select and Show the Report"

    Private Sub Showreport()

        Try

            Select Case Request("ip")
                Case "minfo"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 788
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block
                    'Response.Write("<head><title>" & "MACHINE OVERVIEW" & "</title></head>")
                    cpnlReport.Text = "MACHINE OVERVIEW"
                    lblHead.Text = "MACHINE OVERVIEW"
                    Showreport(1)
                Case "sinfo"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 789
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block



                    'Response.Write("<head><title>" & "SYSTEM OVERVIEW" & "</title></head>")
                    cpnlReport.Text = "SYSTEM OVERVIEW"
                    lblHead.Text = "SYSTEM OVERVIEW"
                    Showreport(2)

                Case "mosts"

                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 790
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block


                    'Response.Write("<head><title>" & "MACHINE ONLINE STATUS" & "</title></head>")
                    cpnlReport.Text = "MACHINE ONLINE STATUS"
                    lblHead.Text = "MACHINE ONLINE STATUS"
                    Showreport(3)
                Case "mostatus"
                    'Security Block
                    Dim intId As Integer
                    If Not IsPostBack Then
                        Dim str As String
                        str = HttpContext.Current.Session("PropRootDir")
                        intId = 791
                        Dim obj As New clsSecurityCache
                        If obj.ScreenAccess(intId) = False Then
                            Response.Redirect("../frm_NoAccess.aspx")
                        End If
                        obj.ControlSecurity(Me.Page, intId)
                    End If
                    'End of Security Block
                    'Response.Write("<head><title>" & "MACHINE ONLINE STATUS" & "</title></head>")
                    cpnlReport.Text = "MACHINE ONLINE STATUS"
                    lblHead.Text = "MACHINE ONLINE STATUS"
                    Showreport(4)
                Case Else
                    Response.Redirect("monitorindex.aspx", False)
            End Select

        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub

    Private Sub Showreport(ByVal id As Integer)

        Try
            If id = 1 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crMachineOverview.rpt")
                crDocument.Load(Reportpath)


            ElseIf id = 2 Then
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("crSystemOverview.rpt")
                crDocument.Load(Reportpath)


            ElseIf id = 3 Then
                '********* OLD CODE****************
                'crDocument = New crMachineOnlineStatus
                crvReport.SeparatePages = True
                '*************************
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("rptNewOLStatus.rpt")
                crDocument.Load(Reportpath)
                'Dim crMachineST As New rptNewOLStatus

                Dim ds As New DataSet
                Dim sqlADPShow As SqlDataAdapter
                sqlADPShow = New SqlDataAdapter(" select MM_NU9_DID_FK DomainID,MM_NU9_MID MachineID,dm_vc150_domainname Domain, MM_VC150_Machine_Name Mname,MM_VC8_Machine_Type MType from T170012,T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp") & "and MM_NU9_DID_FK=DM_NU9_DID_PK ", ConStr)
                sqlADPShow.Fill(ds)
                '******Code To Enter the Light Status
                ds.Tables(0).Columns.Add("LightStatus")
                For IntI As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim MCID As Integer = 0
                    Dim SQLADP As SqlDataAdapter
                    Dim dsSus As New DataSet
                    MCID = ds.Tables(0).Rows(IntI).Item(1)
                    SQLADP = New SqlDataAdapter(" select Rs_NU9_PROCESSID ProcessID,RS_VC150_CAT4 Status,RQ_VC150_CAT12 MName,RQ_VC150_CAT13 Domain,RQ_NU9_DOMAIN_FK DomainID,RQ_NU9_MACHINE_CODE_FK MachineID from T130023, T130022 , T170012 where Rs_NU9_PROCESSID in (10020012,10020020) and RQ_NU9_SQID_PK=RS_NU9_SQID_FK and RQ_NU9_CLIENTID_FK=8 and convert(datetime,(convert(char(10),RS_VC100_RESPONSE_DATETIME,101)),101)= convert(Datetime,convert(Varchar(10),getdate(),101),101) and RQ_VC150_CAT12= T170012.MM_VC150_Machine_Name and T170012.MM_NU9_MID=" & MCID & " ", ConStr)
                    SQLADP.Fill(dsSus)
                    Dim IntJ As Integer
                    Dim strSTP1 As Integer = 0
                    Dim strSTP2 As Integer = 0
                    If dsSus.Tables(0).Rows.Count > 0 Then
                        For IntJ = 0 To dsSus.Tables(0).Rows.Count - 1
                            If dsSus.Tables(0).Rows(IntJ).Item(1) = "ER" Then
                                strSTP1 = 1
                            ElseIf dsSus.Tables(0).Rows(IntJ).Item(1) = "NE" Then
                                strSTP2 = 1
                            End If
                        Next
                        If strSTP1 = 1 And strSTP2 = 0 Then
                            ds.Tables(0).Rows(IntI).Item("LightStatus") = "R"
                        ElseIf strSTP1 = 1 And strSTP2 = 1 Then
                            ds.Tables(0).Rows(IntI).Item("LightStatus") = "O"
                        ElseIf strSTP1 = 0 And strSTP2 = 1 Then
                            ds.Tables(0).Rows(IntI).Item("LightStatus") = "G"
                        End If
                    Else
                        ds.Tables(0).Rows(IntI).Item("LightStatus") = "B"
                    End If
                Next

                '********************************
                crDocument.SetDataSource(ds)
                'crMachineST.SetDataSource(ds)
                crvReport.ReportSource = crDocument
                crvReport.EnableDrillDown = False
                Exit Sub

            ElseIf id = 4 Then
                '********* OLD CODE****************
                'crDocument = New crMachineOnlineStatus_Details
                'Dim strRecordSelectionFormula As String
                'strRecordSelectionFormula = "{Command.did}= " & Request("did") & " and" & "{Command.mid}= " & Request("mid") & " and" & "{Command.ObjectID}= " & Request("oid") & " and" & "{Command.StatusID}= " & Request("sid")
                'crDocument.RecordSelectionFormula = strRecordSelectionFormula
                '*************************
                crDocument = New ReportDocument
                Dim Reportpath As String
                Reportpath = Server.MapPath("rptNewMachineOnlineST_Dtl.rpt")
                crDocument.Load(Reportpath)



                'Dim crMachineST_Dtl As New rptNewMachineOnlineST_Dtl
                Dim DID As Integer = Val(Request("did"))
                Dim MID As Integer = Val(Request("mid"))
                Dim ds As New DataSet
                Dim sqlADPShow As SqlDataAdapter
                sqlADPShow = New SqlDataAdapter("select MM_VC8_Machine_Type MachineType,MM_VC150_Machine_Name MachineName,MM_VC100_Machine_IP IPAddress,MM_VC200_Proc_Desc ProcessorDesc ,MM_NU9_Proc_Speed ProcSpeed,MM_VC200_OS_Name OSName,MM_VC100_OS_Ver OSVersion,MM_VC100_OS_SP ServicePack,MM_CH1_IsEnable EnableStatus,MM_VC20_Cat1 as Cat1,MM_VC20_Cat2 as Cat2,MM_VC20_Cat3 as Cat3 from T170012 where MM_NU9_DID_FK=" & DID & " and MM_NU9_MID=" & MID & ";select RS_NU9_PROCESSID ProcessID, RS_VC150_CAT1 DBase,RS_VC150_CAT2 DiskInfo,RS_VC150_CAT3 FSCrDt,RS_VC150_CAT4 Error,RS_VC150_CAT5 UsedSp,RS_VC150_CAT6 TotSp,am_vc500_desc AlertDesc from T130023,T180011,T130022, T170012 where RS_NU9_PROCESSID in (10020012,10020020) and RS_VC150_CAT4='ER' and T130023.RS_NU9_SQID_FK=T130022.RQ_NU9_SQID_PK and T180011.AM_NU9_AID_PK=T130022.RQ_NU9_ALERT_FK and T170012.MM_VC150_Machine_Name=RQ_VC150_CAT12 and T170012.MM_NU9_MID=" & MID & " and convert(Datetime,convert(Varchar(10),RS_VC100_RESPONSE_DATETIME,101),101) = convert(Datetime,convert(Varchar(10),getdate(),101),101)  ", ConStr)

                sqlADPShow.Fill(ds)
                crDocument.SetDataSource(ds)
                'crMachineST_Dtl.SetDataSource(ds)
                crvReport.ReportSource = crDocument
                crvReport.EnableDrillDown = False

                Exit Sub
            Else
                Exit Sub
            End If
            clsReport.LogonInformation(crDocument)
            crvReport.ReportSource = crDocument
            crvReport.DataBind()
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Sub
#End Region

#Region "All Other Functions"

    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        If Request("ip") = "mostatus" Then
            Server.Transfer("jdinfo.aspx?ip=mosts")

        End If
        Response.Redirect("../home.aspx", False)
    End Sub
#End Region
End Class
