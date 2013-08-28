Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Partial Class MonitoringCenter_ReportName
    Inherits System.Web.UI.Page
    Private Shared dvSearch As New DataView
    'Protected WithEvents BtnGrdSearch As System.Web.UI.WebControls.Button
    Shared mintID As Integer


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If RdbtnUID.Checked = False Then
                If RdbtnFile.Checked = False Then
                    TxtPUID.Enabled = False
                    txtPPWD.Enabled = False
                    TxtFile.Enabled = False
                Else
                    TxtPUID.Enabled = False
                    txtPPWD.Enabled = False
                    TxtFile.Enabled = True
                End If
            Else
                TxtFile.Enabled = False
                TxtPUID.Enabled = True
                txtPPWD.Enabled = True
            End If
            txtReportName.Attributes.Add("on keyup", "javascript:this.value.to Upper();")



            Dim txthiddenImage As String = Request.Form("txthiddenImage")
            If txthiddenImage <> "" Then
                Select Case txthiddenImage
                    Case "Save"
                        If Val(Request.QueryString("ID")) > 0 Then
                            If update() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record updated successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)

                            End If
                        Else
                            If SAVERecord() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                                cleartextboxes()
                                BINDGrid()
                                DgReportDetail.SelectedIndex = -1
                            End If
                        End If

                    Case "Delete"


                        'If mintID = "" Then
                        '    lstError.Items.Clear()
                        '    lstError.Items.Add("No Row Selected...")
                        '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        'Else
                        'mintID = Request.Form("txtHiddenID")
                        'If DgReportDetail.SelectedIndex = -1 Then

                        '    lstError.Items.Clear()
                        '    lstError.Items.Add(" Please First  Select Row...")
                        '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                        'Else

                        If DeleteRecord() = True Then

                            lstError.Items.Clear()
                            lstError.Items.Add("Record deleted successfully...")
                            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                            BINDGrid()
                            'If DgReportDetail.SelectedIndex = -1 Then
                            '    cpnlError.Visible = False
                            'End If
                            'End If
                        End If
                        'End If


                        'call delete function 
                        'mintID = Request.Form("txtHiddenID")
                        'If DeleteRecord(mintID) = True Then
                        '    lstError.Items.Clear()
                        '    lstError.Items.Add("Record deleted successfully...")
                        '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        '    BINDGrid()
                        '    DgReportDetail.SelectedIndex = -1
                        'End If

                End Select
            End If

            'If RdbtnUID.Checked = False Then
            '    If RdbtnFile.Checked = False Then
            '        TxtPUID.Enabled = False
            '        txtPPWD.Enabled = False
            '        TxtFile.Enabled = False
            '    Else
            '        TxtPUID.Enabled = False
            '        txtPPWD.Enabled = False
            '        TxtFile.Enabled = True
            '    End If
            'Else
            '    TxtFile.Enabled = False
            '    TxtPUID.Enabled = True
            '    txtPPWD.Enabled = True
            'End If

            If Not (Page.IsPostBack) Then

                If Val(Request.QueryString("ID")) > 0 Then
                    fillEditScreen()
                    RdbtnUID.Attributes.Add("onClick", "return ChkRdodisable();")
                    RdbtnFile.Attributes.Add("onClick", "return ChkRdodisable();")

                    cpnlReportDetail.State = CustomControls.Web.PanelState.Collapsed
                    cpnlReportDetail.Visible = False


                Else
                    'DdlDomain.Items.Clear()
                    GetDomain()
                    'DdlPeopleSoftRel.Items.Clear()
                    GetFillingRelease()
                    'DDLRole.Items.Clear()
                    GetRole()
                    'DdlEnv.Items.Clear()
                    GetEnvironment()
                    BINDGrid()
                    RdbtnUID.Attributes.Add("onClick", "return ChkRdodisable();")
                    RdbtnFile.Attributes.Add("onClick", "return ChkRdodisable();")
                End If
            Else
                If ValidateSearchText() = False Then
                    SearchGrid()
                End If
                'RdbtnUID.Attributes.Add("onClick", "return ChkRdodisable();")
                'RdbtnFile.Attributes.Add("onClick", "return ChkRdodisable();")
            End If
        Catch ex As Exception
        End Try
    End Sub

    'this function fill domain dropdowm acc to selected company from t170011

    Private Function GetDomain() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            If SQL.Search("T170011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                DdlDomain.DataSource = dsTemp.Tables(0)
                DdlDomain.DataTextField = "DM_VC150_DomainName"
                DdlDomain.DataValueField = "DM_NU9_DID_PK"
                DdlDomain.DataBind()
                DdlDomain.Items.Insert(0, "Select")
            Else

                lstError.Items.Add("No domain avilable for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If


        Catch ex As Exception
            CreateLog("ReportName", "FillDomain", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)

            Return Nothing

        End Try
    End Function
    'this function fill machine dropdown list 
    'pass value of selected domain from dropdown Domain
    'table t170012 

    Private Function GetMachine(ByVal Domain As Integer)
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select MM_VC150_Machine_Name ,MM_NU9_MID   from t170012 where MM_NU9_DID_FK=" & Domain & ""
            If SQL.Search("T170012 ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                DdlMachine.DataSource = dsTemp.Tables(0)
                DdlMachine.DataTextField = "MM_VC150_Machine_Name"
                DdlMachine.DataValueField = "MM_NU9_MID"
                DdlMachine.DataBind()
            Else

                lstError.Items.Add("No machine  avilable for selected domain")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False

            End If
        Catch ex As Exception
            CreateLog("ReportName", "GetMachine", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    'this function fill peoplesoft release dropdown acc to selected company
    'table UDC

    Private Function GetFillingRelease()
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select name from udc where udctype='prel' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                DdlPeopleSoftRel.DataSource = dsTemp.Tables(0)
                DdlPeopleSoftRel.DataTextField = "name"
                DdlPeopleSoftRel.DataBind()
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("ReportName", "GetFillingRelease", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    'this function fill peoplesoft Role dropdown acc to selected company
    'table UDC

    Private Function GetRole()
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select name from udc where udctype='prol' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "Report in people Soft", "GetRole", sqstr, dsTemp, "", "") = True Then
                DDLRole.DataSource = dsTemp.Tables(0)
                DDLRole.DataTextField = "name"
                DDLRole.DataBind()
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("ReportName", "GetRole", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    'this function fill peoplesoft ENV dropdown acc to selected company
    'table UDC
    Private Function GetEnvironment()
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select name from udc where udctype='PENV' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "Report in people Soft", "GetRole", sqstr, dsTemp, "", "") = True Then
                DdlEnv.DataSource = dsTemp.Tables(0)
                DdlEnv.DataTextField = "name"
                DdlEnv.DataBind()
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("ReportName", "GetEnvironment", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function

    Private Sub DdlDomain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlDomain.SelectedIndexChanged
        If DdlDomain.SelectedIndex.Equals(0) Then
            lstError.Items.Add("Select domain")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            DdlMachine.Items.Clear()
        Else
            cpnlError.Visible = False
            DdlMachine.Items.Clear()
            GetMachine(DdlDomain.SelectedValue)
        End If


    End Sub
    'this functin fill Reportdetail grid when we save record
    'table t130041
    Private Function BINDGrid() As Boolean
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select  RP_NU9_SQID_PK,RP_VC150_ReportName,case isnull(RP_VC150_PPWD,'') when '' then '' else '*' end as RP_VC150_PPWD  , case isnull(RP_VC150_PUID,'')when '' then '' else '*' end as RP_VC150_PUID ,RP_VC150_FilePath,RP_VC150_Env,RP_VC150_Version, RP_VC150_JobQUE from t130041 where RP_NU9_CompanyID_FK=" & Session("PropCAComp")
            SQL.Search("T130041 ", "report in people soft", "BINDGrid", sqstr, dsTemp, "", "")
            dvSearch = dsTemp.Tables(0).DefaultView
            DgReportDetail.DataSource = dvSearch
            DgReportDetail.DataBind()

        Catch ex As Exception
            CreateLog("ReportName", "Bindgrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try

    End Function

    'fill the records in screen to update from t130041

    Private Function fillEditScreen() As Boolean
        Dim StrSql As String
        Dim SQID As Integer
        Dim subreport As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            SQID = Request.QueryString("ID")
            StrSql = "select* from T130041 where RP_NU9_SQID_PK=" & SQID
            Dim blnStatus As Boolean
            Dim SqDrReport As SqlDataReader
            SqDrReport = SQL.Search("", "", StrSql, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                SqDrReport.Read()
                GetDomain()
                GetFillingRelease()
                GetEnvironment()
                GetRole()

                Dim intDomain As Integer = SqDrReport.Item("RP_NU9_Domain_FK")
                DdlDomain.SelectedValue = intDomain
                'machines fill after selection of Domain
                GetMachine(intDomain)

                'TxtUID.Text = SqDrReport.Item("RP_VC150_Machine_UID")
                DdlMachine.SelectedValue = SqDrReport.Item("RP_NU9_Machine_FK")
                DdlPeopleSoftRel.SelectedValue = SqDrReport.Item("RP_VC150_Release")
                txtReportName.Text = SqDrReport.Item("RP_VC150_ReportName")
                TxtVersion.Text = SqDrReport.Item("RP_VC150_Version")
                DdlSubReport.SelectedValue = Convert.ToString(SqDrReport.Item("RP_CH2_SubmitReport")).Trim
                If DdlSubReport.SelectedValue.Equals("N") Then
                    TxtUID.Enabled = False
                    TxtPWD.Enabled = False
                    DDLRole.Enabled = False
                    DdlEnv.Enabled = False
                    TxtPUID.Enabled = False
                    txtPPWD.Enabled = False
                    TxtFile.Enabled = False
                    RdbtnUID.Enabled = False
                    RdbtnFile.Enabled = False
                    TxtJobQueue.Enabled = False
                Else
                    'TxtUID.Text = Decrypt(SqDrReport.Item("RP_VC150_Machine_UID"))
                    'TxtPWD.Text = Decrypt(SqDrReport.Item("RP_VC150_Machine_PWD"))
                    'TxtPWD.Attributes.Add("value", TxtPWD.Text)

                    fillEditScreenMachineUID()
                    DDLRole.SelectedValue = SqDrReport.Item("RP_VC150_Role")
                    DdlEnv.SelectedValue = SqDrReport.Item("RP_VC150_Env")
                    TxtJobQueue.Text = SqDrReport.Item("RP_VC150_JobQUE")


                    If SqDrReport.Item("RP_VC150_PUID") <> "" Then
                        RdbtnUID.Checked = True
                        RdbtnFile.Checked = False
                        TxtPUID.Enabled = True

                        'forthe text boxes 
                        TxtPUID.Text = Decrypt(SqDrReport.Item("RP_VC150_PUID"))
                        txtPPWD.Enabled = True
                        txtPPWD.Text = Decrypt(SqDrReport.Item("RP_VC150_PPWD"))
                        txtPPWD.Attributes.Add("value", txtPPWD.Text)
                        TxtFile.Enabled = False

                    ElseIf SqDrReport.Item("RP_VC150_FilePath") <> "" Then
                        RdbtnUID.Checked = False
                        RdbtnFile.Checked = True
                        TxtFile.Enabled = True
                        'fo the text boxes 
                        TxtFile.Text = SqDrReport.Item("RP_VC150_FilePath")
                        TxtPUID.Enabled = False
                        txtPPWD.Enabled = False
                    End If
                End If
            End If
            SqDrReport.Close()
        Catch ex As Exception
        End Try
    End Function
    ' this function fill machine txtuid and txtpwd 
    'according to selected domain and machine
    'table t170011 and t130041

    Private Function fillEditScreenMachineUID() As Boolean
        Dim StrSql As String
        Dim SQID As Integer
        Dim subreport As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            SQID = Request.QueryString("ID")
            StrSql = "select MM_VC500_UID,MM_VC500_PWD,a.RP_NU9_DOMAIN_FK,a.RP_NU9_MACHINE_FK,a.RP_NU9_SQID_PK from T170012,T130041 A where MM_NU9_DID_FK=A.RP_NU9_DOMAIN_FK AND MM_NU9_MID=A.RP_NU9_MACHINE_FK AND  RP_NU9_SQID_PK=" & SQID
            Dim blnStatus As Boolean
            Dim SqDrReport As SqlDataReader
            SqDrReport = SQL.Search("", "", StrSql, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                SqDrReport.Read()
                ' reader read value from t170011 and fill text boxes
                TxtUID.Text = Decrypt(SqDrReport.Item("MM_VC500_UID"))
                TxtPWD.Text = Decrypt(SqDrReport.Item("MM_VC500_PWD"))
                TxtPWD.Attributes.Add("value", TxtPWD.Text)

            End If
            SqDrReport.Close()
        Catch ex As Exception
        End Try
    End Function
    'this function update record in t130041 and t170012

    Private Function update() As Boolean
        Try
            Dim StrSql As String
            Dim SQID As Integer
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            Dim PUID As String
            Dim PPWD As String
            Dim FilePath As String

            'pass the id from first screen to udate record
            SQID = Request.QueryString("ID")
            StrSql = "update T130041 where RP_NU9_SQID_PK=" & SQID

            'this function check validation before save that all Fields are fill or not if submit report is N

            If ValidationRequest() = False Then
                Exit Function
            End If
            arColName.Add("RP_NU9_Machine_FK") 'machine
            arColName.Add("RP_NU9_Domain_FK") 'Domain 
            arColName.Add("RP_VC150_Release") 'peoplesoft Release
            arColName.Add("RP_VC150_ReportName") 'Report name
            arColName.Add("RP_VC150_Version") 'version
            arColName.Add("RP_CH2_SubmitReport") 'submit Report
            arColName.Add("RP_NU9_CompanyID_FK") 'company id
            'arColName.Add("RP_VC150_Machine_UID")
            'arColName.Add("RP_VC150_Machine_PWD")
            arRowData.Add(DdlMachine.SelectedValue)
            arRowData.Add(DdlDomain.SelectedValue)
            arRowData.Add(DdlPeopleSoftRel.SelectedValue)
            arRowData.Add(txtReportName.Text)
            arRowData.Add(TxtVersion.Text)
            arRowData.Add(DdlSubReport.SelectedValue)
            arRowData.Add(Session("PropCAComp"))

            If DdlSubReport.SelectedValue.Equals("N") Then

                arColName.Add("RP_VC150_Role")
                arColName.Add("RP_VC150_Env")
                arColName.Add("RP_VC150_PUID")
                arColName.Add("RP_VC150_PPWD")
                arColName.Add("RP_VC150_FilePath")
                arColName.Add("RP_VC150_JobQUE")


                arRowData.Add("")
                arRowData.Add("")
                arRowData.Add("")
                arRowData.Add("")
                arRowData.Add("")
                arRowData.Add("")
                'arRowData.Add("")
                'arRowData.Add("")
            End If

            'update record according to submit report Y or n
            If DdlSubReport.SelectedValue.Equals("Y") Then

                'this function check validation before save that all Fields are fill or not if submit report is Y

                If ValidateRequest() = False Then
                    Exit Function
                End If

                If RdbtnUID.Checked = True Then

                    PUID = Encrypt(TxtPUID.Text)
                    PPWD = Encrypt(txtPPWD.Text)
                    FilePath = TxtFile.Text

                End If
                If RdbtnFile.Checked = True Then
                    PUID = TxtPUID.Text
                    PPWD = txtPPWD.Text
                    FilePath = TxtFile.Text
                End If

                'arColName.Add("RP_VC150_Machine_UID")
                'arColName.Add("RP_VC150_Machine_PWD")
                arColName.Add("RP_VC150_Role") '
                arColName.Add("RP_VC150_Env")
                arColName.Add("RP_VC150_PUID")
                arColName.Add("RP_VC150_PPWD")
                arColName.Add("RP_VC150_FilePath")
                arColName.Add("RP_VC150_JobQUE")

                'arRowData.Add(IONEncrypt(TxtUID.Text))
                'arRowData.Add(IONEncrypt(TxtPWD.Text))
                arRowData.Add(DDLRole.SelectedValue)
                arRowData.Add(DdlEnv.SelectedValue)
                arRowData.Add(PUID)
                arRowData.Add(PPWD)
                arRowData.Add(FilePath)
                arRowData.Add(TxtJobQueue.Text)
                SaveMachineUIDRequest()

            End If

            If SQL.Update("t130041", "report in People soft", "Update", "Select* from T130041 where RP_NU9_SQID_PK=" & SQID, arColName, arRowData) = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("dataobjectentry", "SAVERecord", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing

        End Try
    End Function

    'this function save Machine UID and PWD  acoording to selected domain and machine
    'table T170012

    Private Function SaveMachineUIDRequest() As Boolean
        Try
            Dim StrSql As String
            Dim SQID As Integer
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            StrSql = "update T170012 where MM_NU9_DID_FK=" + DdlDomain.SelectedValue + " and MM_NU9_MID=" + DdlMachine.SelectedValue + " "


            arColName.Add("MM_VC500_UID") 'machine UID
            arColName.Add("MM_VC500_PWD") 'Machine PWD

            arRowData.Add(Encrypt(TxtUID.Text))
            arRowData.Add(Encrypt(TxtPWD.Text))

            If SQL.Update("t170012", "report in People soft", "Update", "Select* from T170012 where MM_NU9_DID_FK=" + DdlDomain.SelectedValue + " and MM_NU9_MID=" + DdlMachine.SelectedValue + " ", arColName, arRowData) = True Then
                Return True
            Else
                Return False

            End If
        Catch ex As Exception

        End Try
    End Function
    'this function check all fields are fill when submit report Y before record save

    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If TxtUID.Text.Equals("") Then
            lstError.Items.Add("UserId cannot be blank...")
            shFlag = 1
        End If

        If TxtJobQueue.Text.Equals("") Then
            lstError.Items.Add("JobQueue cannot be blank...")
            shFlag = 1
        End If

        If RdbtnUID.Checked = False Then
            If RdbtnFile.Checked = False Then
                lstError.Items.Add(" userid or file Path  IS MANDATORY....")
                shFlag = 1
            Else
                If TxtFile.Text.Equals("") Then
                    lstError.Items.Add("File cannot be blank.....")
                    shFlag = 1
                End If
            End If
        Else
            If TxtPUID.Text.Equals("") Then
                lstError.Items.Add("User ID cannot be blank.....")
                shFlag = 1
            End If
        End If

        If shFlag = 1 Then
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            Return False
        Else
            Return True
        End If

    End Function
    'this function check all fields are fill when submit report N before record save

    Private Function ValidationRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtReportName.Text.Equals("") Then
            lstError.Items.Add("Report Name cannot be blank...")
            shFlag = 1
        End If

        If TxtVersion.Text.Equals("") Then
            lstError.Items.Add("Version cannot be blank...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            Return False
        Else
            Return True
        End If

    End Function
    'this function clear text boxes when record saved

    Private Function cleartextboxes()
        TxtUID.Text = ""
        TxtPWD.Text = ""
        DdlDomain.SelectedIndex = 0
        DdlMachine.SelectedIndex = 0
        DdlPeopleSoftRel.SelectedIndex = 0
        DDLRole.SelectedIndex = 0
        DdlEnv.SelectedIndex = 0
        TxtPUID.Text = ""
        txtPPWD.Text = ""
        TxtFile.Text = ""
        TxtJobQueue.Text = ""
        txtReportName.Text = ""
        TxtVersion.Text = ""
        RdbtnUID.Checked = False
        RdbtnFile.Checked = False
        DdlMachine.Items.Clear()
        'RdbtnUID.Enabled = False
        'RdbtnFile.Enabled = False

    End Function

    'THIS FUNCTION SAVE RECORD IN TABLE T130041 and t170012

    Private Function SAVERecord() As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            Dim PUID As String
            Dim PPWD As String
            Dim FilePath As String

            Dim intMax As Integer = SQL.Search("Report in People Soft", "SaveRecord", "select max(RP_NU9_SQID_PK) from T130041", "")
            intMax += 1

            'this function check validation before save that all Fields are fill or not if submit report is  N



            arColName.Add("RP_NU9_SQID_PK")
            arColName.Add("RP_NU9_Machine_FK") 'MACHINE 
            arColName.Add("RP_NU9_Domain_FK") 'domain
            arColName.Add("RP_VC150_Release") 'peoplesoft release
            arColName.Add("RP_CH2_SubmitReport") 'submit report
            arColName.Add("RP_VC150_ReportName") 'report name
            arColName.Add("RP_VC150_Version") 'version
            arColName.Add("RP_NU9_CompanyID_FK") 'company id


            arRowData.Add(intMax)
            arRowData.Add(DdlMachine.SelectedValue)
            arRowData.Add(DdlDomain.SelectedValue)
            arRowData.Add(DdlPeopleSoftRel.SelectedValue)
            arRowData.Add(DdlSubReport.SelectedValue)
            arRowData.Add(txtReportName.Text)
            arRowData.Add(TxtVersion.Text)
            arRowData.Add(Session("PropCAComp"))

            If DdlSubReport.SelectedValue.Equals("N") Then
                If ValidationRequest() = False Then
                    Exit Function
                End If 'validation request to check the fields fill or not according to submit report y

                'End If
            Else

                'If DdlSubReport.SelectedValue.Equals("Y") Then
                'validation request to check the fields fill or not according to submit report y
                If ValidateRequest() = False Then
                    Exit Function
                End If
                If ValidationRequest() = False Then
                    Exit Function
                End If

                'radio button check and acc to that value saved in t130041

                If RdbtnUID.Checked = True Then

                    PUID = Encrypt(TxtPUID.Text)
                    PPWD = Encrypt(txtPPWD.Text)
                    FilePath = TxtFile.Text
                End If

                If RdbtnFile.Checked = True Then
                    PUID = TxtPUID.Text
                    PPWD = txtPPWD.Text
                    FilePath = TxtFile.Text
                End If
                ' other fields that are saved when submit report is Y  

                arColName.Add("RP_VC150_Role") 'peoplesoft release
                arColName.Add("RP_VC150_Env") 'peoplesoft env
                arColName.Add("RP_VC150_PUID") 'UID
                arColName.Add("RP_VC150_PPWD") 'PWD
                arColName.Add("RP_VC150_FilePath") 'File path
                arColName.Add("RP_VC150_JobQUE") ' JOB Queue

                arRowData.Add(DDLRole.SelectedValue)
                arRowData.Add(DdlEnv.SelectedValue)
                arRowData.Add(PUID)
                arRowData.Add(PPWD)
                arRowData.Add(FilePath)
                arRowData.Add(TxtJobQueue.Text)

                'this function save machine UID and PWD in t170012
                SaveMachineUIDRequest()
            End If

            If SQL.Save("t130041", "Report in people soft", "SaveRecord", arColName, arRowData, "") = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("dataobjectentry", "SAVERecord", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    'this function delete record from t130041 
    'pass value of SQID

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

    Private Sub DgReportDetail_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DgReportDetail.SelectedIndexChanged
        mintID = DgReportDetail.SelectedItem.Cells(1).Text.Trim

    End Sub

    Private Sub DgReportDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgReportDetail.ItemDataBound
        'Dim itemType As ListItemType = e.Item.ItemType

        'If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
        '    Return
        'Else
        '    Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)

        '    e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        If e.Item.ItemType = ListItemType.Item Or ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onclick", "GridClick(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(1).Text.Trim & ")")
        End If
    End Sub

    'THIS FUCTION CHECK THE TEXT BOXES BEFORE SEARCH
    Private Function ValidateSearchText() As Boolean
        Dim shFlag As Short
        shFlag = 0
        'lstError.Items.Clear()

        If TxtReportName_s.Text.Equals("") = False Then
            shFlag = 1
        End If

        If TxtFile_s.Text.Equals("") = False Then
            shFlag = 1
        End If

        If TxtEnv_s.Text.Equals("") = False Then
            shFlag = 1
        End If

        If TxtVersion_s.Text.Equals("") = False Then
            shFlag = 1
        End If

        If TxtJobQueue_s.Text.Equals("") = False Then
            shFlag = 1
        End If

        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            Return False
        Else
            Return True
        End If


    End Function
    'this fuction search data in grid t130041

    Private Function SearchGrid()
        Dim strFilter As String

        If TxtReportName_s.Text.Trim.Equals("") = False Then
            strFilter = "RP_VC150_ReportName like '" & TxtReportName_s.Text.Trim & "' and "
        End If

        If TxtFile_s.Text.Trim.Equals("") = False Then
            strFilter &= "RP_VC150_FilePath like '" & TxtFile_s.Text.Trim & "' and "
        End If

        If TxtEnv_s.Text.Trim.Equals("") = False Then
            strFilter &= "RP_VC150_Env like '" & TxtEnv_s.Text.Trim & "' and "
        End If

        If Not TxtVersion_s.Text.Trim.Equals("") Then
            strFilter &= "RP_VC150_Version like'" & TxtVersion_s.Text.Trim & "' and "
        End If

        If Not TxtJobQueue_s.Text.Trim.Equals("") Then
            strFilter &= "RP_VC150_JobQUE like'" & TxtJobQueue_s.Text.Trim & "' and "
        End If

        If (IsNothing(strFilter)) = True Then
            strFilter = strFilter.Replace("*", "%")
            dvSearch.RowFilter = strFilter
            DgReportDetail.DataSource = dvSearch
            DgReportDetail.DataBind()
        ElseIf (strFilter.Trim.Equals(String.Empty)) = True Then

        Else
            strFilter = strFilter.Remove(strFilter.Length - 4, 4)
            strFilter = strFilter.Replace("*", "%")
            dvSearch.RowFilter = strFilter
            DgReportDetail.DataSource = dvSearch
            DgReportDetail.DataBind()
        End If
    End Function

    Private Sub DdlSubReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlSubReport.SelectedIndexChanged

        'enable disable fields acc to submit report Y oR N

        If DdlSubReport.SelectedValue.Equals("N") Then
            TxtUID.Enabled = False
            TxtPWD.Enabled = False
            DDLRole.Enabled = False
            DdlEnv.Enabled = False
            TxtPUID.Enabled = False
            txtPPWD.Enabled = False
            TxtFile.Enabled = False
            RdbtnUID.Enabled = False
            RdbtnFile.Enabled = False
            TxtJobQueue.Enabled = False
            cpnlError.Visible = False

        Else
            TxtUID.Enabled = True
            TxtPWD.Enabled = True
            DDLRole.Enabled = True
            DdlEnv.Enabled = True
            'TxtPUID.Enabled = True
            'txtPPWD.Enabled = True
            'TxtFile.Enabled = True
            TxtJobQueue.Enabled = True
            RdbtnUID.Enabled = True
            RdbtnFile.Enabled = True
            cpnlError.Visible = False

        End If
    End Sub

    Private Function Encrypt(ByVal Data As String) As String
        Dim shaM As SHA1Managed = New SHA1Managed
        System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        '// Getting the bytes of the encrypted data.//
        Dim bytEncrypt() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        '// Converting the byte into string.//
        Dim strEncrypt As String = System.Convert.ToBase64String(bytEncrypt)
        Encrypt = strEncrypt
    End Function

    Private Function Decrypt(ByVal Data As String) As String
        Dim bytData() As Byte = System.Convert.FromBase64String(Data)
        Dim strData As String = ASCIIEncoding.ASCII.GetString(bytData)
        Decrypt = strData
    End Function
    'Private Sub DdlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlCompany.SelectedIndexChanged

    '    DdlMachine.Items.Clear()
    '    cpnlMachine.State = CustomControls.Web.PanelState.Expanded
    '    cpnlReport.State = CustomControls.Web.PanelState.Expanded


    '    If txthiddenSQID <> "" Then
    '        fillEditScreen()
    '    Else
    '        DdlDomain.Items.Clear()
    '        GetDomain()
    '        DdlPeopleSoftRel.Items.Clear()
    '        GetFillingRelease()
    '        DDLRole.Items.Clear()
    '        GetRole()
    '        DdlEnv.Items.Clear()
    '        GetEnvironment()

    '        cpnlReportDetail.State = CustomControls.Web.PanelState.Expanded
    '        BINDGrid()
    '    End If
    'End Sub

    'radoibutton checked
    'If SqDrReport.Item("RP_VC150_PUID") = "" Then
    '    If SqDrReport.Item("RP_VC150_FilePath") = "" Then
    '        RdbtnUID.Enabled = False
    '        RdbtnFile.Enabled = False
    '        TxtPUID.Enabled = False
    '        txtPPWD.Enabled = False
    '        TxtFile.Enabled = False
    '    End If
    'End If
    'If SqDrReport.Item("RP_VC150_PUID") = "" Then
    '    RdbtnUID.Checked = False
    '    RdbtnFile.Checked = True
    '    'fo the text boxes 
    '    TxtFile.Text = SqDrReport.Item("RP_VC150_FilePath")
    '    TxtFile.Enabled = True
    '    TxtPUID.Enabled = False
    '    txtPPWD.Enabled = False
    'ElseIf SqDrReport.Item("RP_VC150_FilePath") = "" Then
    '    RdbtnUID.Checked = True
    '    RdbtnFile.Checked = False
    '    'forthe text boxes 
    '    TxtPUID.Text = IONDecrypt(SqDrReport.Item("RP_VC150_PUID"))
    '    txtPPWD.Text = IONDecrypt(SqDrReport.Item("RP_VC150_PPWD"))
    '    txtPPWD.Attributes.Add("value", txtPPWD.Text)
    '    TxtFile.Enabled = False
    '    TxtPUID.Enabled = True
    '    txtPPWD.Enabled = True

    'End If
    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    If SAVERecord() = True Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("Record saved successfully...")
    '        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
    '        cleartextboxes()
    '        BINDGrid()
    '        DgReportDetail.SelectedIndex = -1
    '    End If

    'End Sub
    'Private Sub Imgbtnsave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    'If txthiddenSQID <> "" Then
    '    If update() = True Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("record updated successfully...")
    '    End If
    'Else
    '    If SAVERecord() = True Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("Record saved successfully...")
    '        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
    '        cleartextboxes()
    '        BINDGrid()
    '        DgReportDetail.SelectedIndex = -1
    '    End If
    'End If
    'End Sub
    'Private Sub Imgbtndelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    If DeleteRecord(mintID) = True Then
    '        lstError.Items.Clear()
    '        lstError.Items.Add("Record deleted successfully...")
    '        BINDGrid()
    '        DgReportDetail.SelectedIndex = -1
    '    End If
    'End Sub
    'Private Sub Imgbtnclose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Response.Redirect("report.aspx")
    'End Sub
End Class
