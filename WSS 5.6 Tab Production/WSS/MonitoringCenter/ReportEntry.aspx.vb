Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Cryptography

Partial Class MonitoringCenter_ReportEntry
    Inherits System.Web.UI.Page
    Private Shared dvSearch As New DataView
    Private arrSearchText As New ArrayList
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList
    Private Shared arrColumnName As New ArrayList

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        txtCSS(Me.Page, "cpnlReportDetail")
        'javascript function added (Report name in uppercase)
        txtReportName.Attributes.Add("onblur", "javascript: this.value=this.value.toUpperCase();")
        'javascript function added(TxtVersion in UpperCase)
        TxtVer.Attributes.Add("onblur", "javascript: this.value=this.value.toUpperCase();")
        '  'javascript function added(Txtalias name in UpperCase) 
        TxtAname_F.Attributes.Add("onblur", "javascript: this.value=this.value.toUpperCase();")

        If Not (IsPostBack) Then
            'calling javascript function to fill machine dropdown
            DdlDomain.Attributes.Add("OnChange", "DomainChange('" & DdlDomain.ClientID & "','" & DdlMachine.ClientID & "');")
            DdlMachine.Items.Add(New ListItem("Select", "0"))
        End If

        If IsPostBack Then
            'fill machine dropdown by selected domain without post back
            FillAjaxDropDown(DdlMachine, Request.Form("txtMachineInfo"), "cpnlReportDetail:" & DdlMachine.ID, New ListItem("Select", "0"))
        End If

        Try
            'enable or disable RadioButton
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

            If Not (Page.IsPostBack) Then
                cpnlReportDetail.Text = "Report Detail [" & WSSSearch.SearchCompNameID(HttpContext.Current.Session("PropCAComp")).ExtraValue & "]"
                'DdlDomain.Items.Clear()
                DefineGridColumnData()
                GetDomain()
                'DdlPeopleSoftRel.Items.Clear()
                GetFillingRelease()
                'DDLRole.Items.Clear()
                GetRole()
                'DdlEnv.Items.Clear()
                GetEnvironment()
                'BINDGrid()
                'Calling javascriptfunction 
                RdbtnUID.Attributes.Add("onClick", "return ChkRdodisable();")
                RdbtnFile.Attributes.Add("onClick", "return ChkRdodisable();")

            Else
                Dim txthiddenImage As String = Request.Form("txthiddenImage")
                If txthiddenImage <> "" Then
                    Select Case txthiddenImage
                        Case "Save"
                            'call SAVERecord Function
                            If SAVERecord() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record saved successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                                cleartextboxes()
                                'BINDGrid()
                                DgReportDetail.SelectedIndex = -1
                            End If
                            'Call DeleteRecord Function
                        Case "Delete"
                            If DeleteRecord() = True Then
                                lstError.Items.Clear()
                                lstError.Items.Add("Record deleted successfully...")
                                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                                'BINDGrid()
                            End If

                    End Select
                End If
            End If
            'call BindGrid Function
            BINDGrid()
            FormatGrid()
        Catch ex As Exception
            CreateLog("ReportEntry", "PageLoad-157", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Sub
    '*****************************************************************
    'Created By:    Mandeep
    'Create Date:   ------
    'Purpose:       This function fill domain dropdowm acc to selected company from t170011
    'Modify Date:   ------
    '*******************************************************************
    Private Function GetDomain() As Boolean
        Try
            'this used to Getconnection string from webconfig
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            '
            Dim dsTemp As New DataSet
            'StrSQL hold sqlQuery
            Dim strSQL As String
            'this function fetch domain against  a company
            strSQL = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" & Session("PropCAComp")
            If SQL.Search("T170011", "ReportEntry", "GetDomain", strSQL, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlDomain dropdown fill
                DdlDomain.DataSource = dsTemp.Tables(0)
                'Domain name
                DdlDomain.DataTextField = "DM_VC150_DomainName"
                'save as domain id
                DdlDomain.DataValueField = "DM_NU9_DID_PK"
                DdlDomain.DataBind()
                DdlDomain.Items.Insert(0, New ListItem("Select", "0"))
            Else
                'SQL.Search is False Msgpanel show no domain exist for  selected company         
                lstError.Items.Clear()
                lstError.Items.Add("No domain avilable for this company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetDomain-193", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Function
    '****************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           this function fill machine dropdown list 
    '                   pass value of selected domain from dropdown Domain
    '                   table t170012 
    'Modify Date:       ------
    '*****************************************************************************
    Private Function GetMachine(ByVal Domain As Integer)
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            'StrSQL hold SQL Query
            Dim strSQL As String
            'this function fetch machines against domain
            strSQL = "select MM_VC150_Machine_Name ,MM_NU9_MID   from t170012 where MM_NU9_DID_FK=" & Domain & ""
            If SQL.Search("T170012 ", "ReportEntry", "GetMachine", strSQL, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlMachine dropdown fill acc to selected domain
                DdlMachine.DataSource = dsTemp.Tables(0)
                'Machine name
                DdlMachine.DataTextField = "MM_VC150_Machine_Name"
                'save value as Machine id
                DdlMachine.DataValueField = "MM_NU9_MID"
                DdlMachine.DataBind()
            Else
                'SQL.Search is False Msgpanel show no Machine exist for  selected Domain
                lstError.Items.Add("No machine  avilable for selected domain")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetMachine-227", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
        End Try
    End Function

    '*****************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill peoplesoft release dropdown acc to selected company
    '                   Table UDC
    'Modify Date:       ------
    '*****************************************************************************************
    Private Function GetFillingRelease()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch PeopleSoft Release against  a company
            sqstr = "select name from udc where udctype='prel' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "ReportEntry", "GetFillingRelease", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlPeopleSoftRel dropdown fill acc to selected company
                DdlPeopleSoftRel.DataSource = dsTemp.Tables(0)
                DdlPeopleSoftRel.DataTextField = "name"
                DdlPeopleSoftRel.DataBind()
            Else
                'SQL.Search is False ddlPeopleSoftRel dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Release  avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetFillingRelease-259", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill peoplesoft Role dropdown acc to selected company
    '                   Table UDC
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetRole()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch role against domain
            sqstr = "select name from udc where udctype='prol' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "ReportEntry", "GetRole", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlRole dropdown fill acc to selected company
                DDLRole.DataSource = dsTemp.Tables(0)
                DDLRole.DataTextField = "name"
                DDLRole.DataBind()
            Else
                'SQL.Search is False ddlRole dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Role  avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetRole-290", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill peoplesoft ENV dropdown acc to selected company
    '                   Table UDC
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetEnvironment()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch  ENV against a company
            sqstr = "select name from udc where udctype='PENV' and company=" & Session("PropCAComp")
            If SQL.Search("udc ", "ReportEntry", "GetEnvironment", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then ddlENV dropdown fill acc to selected company
                DdlEnv.DataSource = dsTemp.Tables(0)
                'Env Name
                DdlEnv.DataTextField = "name"
                DdlEnv.DataBind()
            Else
                'SQL.Search is False ddlENV dropdown will be empty
                lstError.Items.Clear()
                lstError.Items.Add("No Env  avilable for selected Company")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "GetEnvironment-321", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

    'Private Sub DdlDomain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlDomain.SelectedIndexChanged
    '    If DdlDomain.SelectedIndex.Equals(0) Then
    '        lstError.Items.Add("Select domain")
    '        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
    '        DdlMachine.Items.Clear()
    '    Else
    '        cpnlError.Visible = False
    '        DdlMachine.Items.Clear()
    '        GetMachine(DdlDomain.SelectedValue)
    '    End If


    'End Sub
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill ReportDetail Grid acc to selected Company
    '                   Table T130041
    'Modify Date:       ------
    '***************************************************************************************
    Private Function BINDGrid() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'this function fetch data from t1300041 against a company
            sqstr = "select b.DM_VC150_DomainName, a.MM_VC150_Machine_Name, RP_VC150_Release,MM_VC500_UID='*',MM_VC500_PWD='*',RP_CH2_SubmitReport, RP_VC150_ReportName,RP_VC50_AliasName,RP_VC150_Version,case isnull(RP_VC150_PUID,'')when '' then '' else '*' end as RP_VC150_PUID , case isnull(RP_VC150_PPWD,'') when '' then '' else '*' end as RP_VC150_PPWD  , RP_VC150_FilePath, RP_VC150_Role,RP_VC150_Env, RP_VC150_JobQUE,RP_NU9_SQID_PK from t130041,T170012 a,T170011 b,T170012 C   where  RP_NU9_Machine_FK=a.MM_NU9_MID and RP_NU9_Domain_FK=b.DM_NU9_DID_PK  and RP_NU9_Domain_FK=a.MM_NU9_DID_FK AND C.MM_NU9_DID_FK=RP_NU9_Domain_FK AND C.MM_NU9_MID=RP_NU9_Machine_FK  and RP_NU9_CompanyID_FK=" & Session("PropCAComp")

            If SQL.Search("T130041 ", "ReportEntry", "BINDGrid", sqstr, dsTemp, "", "") = True Then
                'SQL.Search is true then we fill dvsearch Dataview with dsTemp(Dataset)
                'bindGrid show data in grid acc to selected company
                'dataview
                dvSearch = dsTemp.Tables(0).DefaultView
                'filter dataview
                dvSearch.RowFilter = GetRowFilter()
                DgReportDetail.DataSource = dvSearch.Table
                DgReportDetail.DataBind()
            Else
                ' SQL.Search is False then grid show only columns in grid with no data
                DgReportDetail.DataSource = dsTemp.Tables(0)
                DgReportDetail.DataBind()
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "BindGrid-369", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function get the text from search textboxes in an array list
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetSeacrhText()
        Try
            'get the text from search textboxes 
            Dim strSearch As String
            For inti As Integer = 0 To arrTextboxName.Count - 1
                strSearch = Request.Form("cpnlReportDetail:DgReportDetail:_ctl1:" & arrTextboxName(inti) & "")
                If strSearch = "" Then
                Else
                    'if search text box has value  pass to getsearch text
                    strSearch = GetSearchString(strSearch)
                End If
                'add value of textboxex to array
                arrSearchText.Add(strSearch)
            Next
        Catch ex As Exception
            CreateLog("ReportEntry", "GetSeacrhText-394", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function Gets the row filter string
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetRowFilter() As String
        Try
            'call getsearchtext function
            GetSeacrhText()
            Dim strRowFilter As String
            For inti As Integer = 0 To dvSearch.Table.Columns.Count - 2
                'check arraysearch text box
                'empty
                If arrSearchText(inti) <> "" Then
                    'contain string
                    If dvSearch.Table.Columns(inti).DataType.FullName = "System.String" Then
                        strRowFilter &= " " & arrColumnName(inti) & " like '" & arrSearchText(inti) & "' and"
                        'contain decimal or date etc
                    ElseIf dvSearch.Table.Columns(inti).DataType.FullName = "System.Decimal" Then
                        If IsNumeric(arrSearchText(inti)) = False Then
                            strRowFilter &= " " & arrColumnName(inti) & "=-101019 and"      'change row filter to non existing value
                        Else
                            strRowFilter &= " " & arrColumnName(inti) & "=" & arrSearchText(inti) & " and"
                        End If
                    Else
                        strRowFilter &= " " & arrColumnName(inti) & " like " & arrSearchText(inti) & " and"
                    End If
                End If
            Next

            If strRowFilter <> Nothing Then
                strRowFilter = strRowFilter.Replace("*", "%")
                If strRowFilter.Substring(strRowFilter.Length - 3, 3) = "and" Then
                    strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3)
                Else
                    strRowFilter = strRowFilter
                End If
            End If
            Return strRowFilter
        Catch ex As Exception
            CreateLog("ReportEntry", "GetRowFilter-438", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function check all fields are fill when submit report Y before record save
    'Modify Date:       ------
    '***************************************************************************************

    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()


        If txtReportName.Text.Equals("") Then
            lstError.Items.Add("Report Name cannot be blank...")
            shFlag = 1
        End If

        If TxtVer.Text.Equals("") Then
            lstError.Items.Add("Version cannot be blank...")

        End If
        If TxtAname_F.Text.Equals("") Then
            lstError.Items.Add("Alias name cannot be blank...")
            shFlag = 1
        End If


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
        'if shFlag=1 means any textbox empty 
        If shFlag = 1 Then
            'message panel display msg
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            'If Shflag=0 no text box empty
            'return True
            Return True
        End If

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function check all fields are fill when submit report N before record save
    'Modify Date:       ------
    '***************************************************************************************

    Private Function ValidationRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtReportName.Text.Equals("") Then
            lstError.Items.Add("Report Name cannot be blank...")
            shFlag = 1
        End If

        If TxtVer.Text.Equals("") Then
            lstError.Items.Add("Version cannot be blank...")

        End If
        If TxtAname_F.Text.Equals("") Then
            lstError.Items.Add("Alias name cannot be blank...")
            shFlag = 1
        End If


        If shFlag = 1 Then
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function clear text boxes when record saved 
    'Modify Date:       ------
    '***************************************************************************************
    Private Function cleartextboxes()
        'clear textboxe and reset dropdown after save
        TxtUID.Text = ""
        TxtPWD.Text = ""
        TxtPWD.Attributes.Add("value", TxtPWD.Text)
        DdlDomain.SelectedIndex = 0
        DdlMachine.SelectedIndex = 0
        DdlPeopleSoftRel.SelectedIndex = 0
        DDLRole.SelectedIndex = 0
        DdlEnv.SelectedIndex = 0
        TxtPUID.Text = ""
        txtPPWD.Text = ""
        TxtFile.Text = ""
        TxtAname_F.Text = ""
        TxtJobQueue.Text = ""
        txtReportName.Text = ""
        TxtVer.Text = ""
        RdbtnUID.Checked = False
        RdbtnFile.Checked = False
        DdlMachine.Items.Clear()
        'RdbtnUID.Enabled = False
        'RdbtnFile.Enabled = False

    End Function

    '*********************************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function Save record in table t130041 acc to submit Report Y or N
    '                   check Alias name should be unique for same company
    '                   Update machine Uid and Pwd in table t170012 by calling  SaveMachineUIDRequest()function
    'Modify Date:       ------
    '*********************************************************************************************************

    Private Function SAVERecord() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            Dim PUID As String
            Dim PPWD As String
            Dim FilePath As String
            Dim strsql As String
            'this query fetch SQID from T130041
            Dim intMax As Integer = SQL.Search("Report in People Soft", "SaveRecord", "select max(RP_NU9_SQID_PK) from T130041", "")
            intMax += 1
            'this function check validation before save that all Fields are fill or not if submit report is  N
            arColName.Add("RP_NU9_SQID_PK")
            arColName.Add("RP_NU9_Machine_FK") 'MACHINE 
            arColName.Add("RP_NU9_Domain_FK") 'domain
            arColName.Add("RP_VC150_Release") 'peoplesoft release
            arColName.Add("RP_CH2_SubmitReport") 'submit report
            arColName.Add("RP_VC150_ReportName") 'report name
            arColName.Add("RP_VC50_AliasName") 'Alias name
            arColName.Add("RP_VC150_Version") 'version
            arColName.Add("RP_VC150_Env") 'peoplesoft env
            arColName.Add("RP_NU9_CompanyID_FK") 'company id

            arRowData.Add(intMax)
            arRowData.Add(DdlMachine.SelectedValue)
            arRowData.Add(DdlDomain.SelectedValue)
            arRowData.Add(DdlPeopleSoftRel.SelectedValue)
            arRowData.Add(DdlSubReport.SelectedValue)
            arRowData.Add(txtReportName.Text)
            arRowData.Add(TxtAname_F.Text)
            arRowData.Add(TxtVer.Text)
            arRowData.Add(DdlEnv.SelectedValue)
            arRowData.Add(Session("PropCAComp"))

            If DdlSubReport.SelectedValue.Equals("N") Then
                'If DdlSubReport.SelectedValue.Equals("N") Then
                ' validation request to check the fields fill or not according to submit report N
                If ValidationRequest() = False Then
                    Exit Function
                End If
            Else
                'If DdlSubReport.SelectedValue.Equals("Y") Then
                ' validate and validation request to check the fields fill or not according to submit report y
                If ValidateRequest() = False Then
                    Exit Function
                End If
                'If ValidationRequest() = False Then
                '    Exit Function
                'End If
                ' if radio button UID  check and acc to that value saved in t130041
                If RdbtnUID.Checked = True Then
                    PUID = Encrypt(TxtPUID.Text)
                    PPWD = Encrypt(txtPPWD.Text)
                    FilePath = TxtFile.Text
                End If
                ' if radio button File  check and acc to that value saved in t130041

                If RdbtnFile.Checked = True Then
                    PUID = TxtPUID.Text
                    PPWD = txtPPWD.Text
                    FilePath = TxtFile.Text
                End If
                ' other fields that are saved when submit report is Y  
                arColName.Add("RP_VC150_Role") 'peoplesoft release
                arColName.Add("RP_VC150_PUID") 'UID
                arColName.Add("RP_VC150_PPWD") 'PWD
                arColName.Add("RP_VC150_FilePath") 'File path
                arColName.Add("RP_VC150_JobQUE") ' JOB Queue

                arRowData.Add(DDLRole.SelectedValue)
                arRowData.Add(PUID)
                arRowData.Add(PPWD)
                arRowData.Add(FilePath)
                arRowData.Add(TxtJobQueue.Text)
                'Calling saveMachineRequest function
                SaveMachineUIDRequest()
            End If
            'strsql hold sql query that select alias name from t130041
            strsql = "select RP_VC50_AliasName from t130041 where RP_VC50_AliasName='" & TxtAname_F.Text & "' and RP_NU9_CompanyID_FK = " & Session("PropCAComp")
            'strAliasname hold the value of Alias name
            Dim strAliasName As String = SQL.Search("", "", strsql)
            If IsNothing(strAliasName) Then
                'if no Alias name exist for selected company then data will save
                If SQL.Save("t130041", "Report in people soft", "SaveRecord", arColName, arRowData, "") = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                'comparing alias name in dataset and the alias name enter by user if cond false then save data
                If strAliasName.Equals(TxtAname_F.Text.Trim) = False Then

                    If SQL.Save("t130041", "Report in people soft", "SaveRecord", arColName, arRowData, "") = True Then
                        Return True
                    Else
                        Return False
                    End If

                Else
                    '  comparing alias name in dataset and the alias name enter by user if cond true then msgpanel show msg
                    lstError.Items.Clear()
                    lstError.Items.Add("This Alias Name Already Exist")
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                End If
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "SaveRecord-693", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function
    '***************************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function save Machine UID and PWD  acoording to selected domain and machine 
    '                   Table T170012 
    'Modify Date:       ------
    '***************************************************************************************************
    Private Function SaveMachineUIDRequest() As Boolean
        Try
            Dim StrSql As String
            Dim SQID As Integer
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList
            'this query update Machine UID and Pwd in t170012
            StrSql = "update T170012 where MM_NU9_DID_FK=" + DdlDomain.SelectedValue + " and MM_NU9_MID=" + DdlMachine.SelectedValue + " "

            arColName.Add("MM_VC500_UID") 'machine UID
            arColName.Add("MM_VC500_PWD") 'Machine PWD

            arRowData.Add(Encrypt(TxtUID.Text))
            arRowData.Add(Encrypt(TxtPWD.Text))

            If SQL.Update("t170012", "reportEntry", "Update", "Select* from T170012 where MM_NU9_DID_FK=" + DdlDomain.SelectedValue + " and MM_NU9_MID=" + DdlMachine.SelectedValue + " ", arColName, arRowData) = True Then
                'if sql.update true then it update machineUID and PWD in table t170012
                Return True
            Else
                'if sql.update False then it will not update machineUID and PWD in table t170012 in case we select submit report N
                Return False

            End If
        Catch ex As Exception

        End Try
    End Function

    '***********************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function Enable or Disable textboxes acc to selected report is Y or N
    'Modify Date:       ------
    '***********************************************************************************************

    Private Sub DdlSubReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlSubReport.SelectedIndexChanged
        'enable or disable text boxes and dropdown if submit report is N 
        If DdlSubReport.SelectedValue.Equals("N") Then
            TxtUID.Enabled = False
            TxtPWD.Enabled = False
            DDLRole.Enabled = False
            'DdlEnv.Enabled = False
            TxtPUID.Enabled = False
            txtPPWD.Enabled = False
            TxtFile.Enabled = False
            RdbtnUID.Enabled = False
            RdbtnFile.Enabled = False
            TxtJobQueue.Enabled = False
        Else
            'enable or disable text boxes and dropdown if submit report is Y
            TxtUID.Enabled = True
            TxtPWD.Enabled = True
            DDLRole.Enabled = True
            'DdlEnv.Enabled = True
            TxtPUID.Enabled = True
            txtPPWD.Enabled = True
            TxtFile.Enabled = True
            TxtJobQueue.Enabled = True
            RdbtnUID.Enabled = True
            RdbtnFile.Enabled = True
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

    Private Sub DgReportDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgReportDetail.ItemDataBound
        Try

            If (e.Item.ItemType = ListItemType.Item Or ListItemType.AlternatingItem) And e.Item.ItemType <> ListItemType.Header Then
                e.Item.Attributes.Add("onclick", "GridClick(" & e.Item.ItemIndex + 1 & "," & e.Item.Cells(0).Text.Trim & ")")

            Else
                For inti As Integer = 0 To arrTextboxName.Count - 1
                    Dim txt As TextBox
                    txt = e.Item.FindControl(arrTextboxName(inti))
                    If TypeOf txt Is TextBox Then
                        CType(txt, TextBox).Text = Request.Form("cpnlReportDetail:DgReportDetail:_ctl1:" & arrTextboxName(inti))
                    End If
                Next
            End If
        Catch ex As Exception
            CreateLog("ReportEntry", "dgrBGDailyMonitor_ItemDataBound-798", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Sub
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function delete record from t130041 
    '                   pass value of SQID
    'Modify Date:       ------
    '***************************************************************************************
    Private Function DeleteRecord() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            'intSeqId hold value of SQID that in txthiddenID which is not visible
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
    '**************************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function define Grid Column Data and also define width of columns in grid
    'Modify Date:       ------
    '**************************************************************************************************

    Private Function DefineGridColumnData()
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(72) 'Domain
        arrColWidth.Add(82) 'Machine Name
        arrColWidth.Add(60) 'Release
        arrColWidth.Add(80) 'Machine UID
        arrColWidth.Add(80) 'Machine PWD
        arrColWidth.Add(60) 'Submit REport
        arrColWidth.Add(101) 'Report name
        arrColWidth.Add(101) 'Alias Name
        arrColWidth.Add(80) 'Version
        arrColWidth.Add(91) 'UID
        arrColWidth.Add(80) 'PWD
        arrColWidth.Add(118) 'File
        arrColWidth.Add(59) 'Role
        arrColWidth.Add(60) 'Env
        arrColWidth.Add(79) 'JobQueue

        arrTextboxName.Clear()
        arrTextboxName.Add("TxtDomain_s")
        arrTextboxName.Add("TxtMachine_s")
        arrTextboxName.Add("TxtRelease_s")
        arrTextboxName.Add("TxtMacUID_s")
        arrTextboxName.Add("TxtMacPWD_s")
        arrTextboxName.Add("TxtSubmitReport_s")
        arrTextboxName.Add("TxtReportName_s")
        arrTextboxName.Add("TxtAliasName_s")
        arrTextboxName.Add("TxtVersion_s")
        arrTextboxName.Add("TxtUID_s")
        arrTextboxName.Add("TxtPWD_s")
        arrTextboxName.Add("TxtFile_s")
        arrTextboxName.Add("TxtRole_s")
        arrTextboxName.Add("TxtEnv_s")
        arrTextboxName.Add("TxtJobQueue_s")

        arrColumnName.Clear()
        arrColumnName.Add("DM_VC150_DomainName") 'Domain
        arrColumnName.Add("MM_VC150_Machine_Name") 'Machine Name
        arrColumnName.Add("RP_VC150_Release") 'Release
        arrColumnName.Add("MM_VC500_UID") 'Machine UID
        arrColumnName.Add("MM_VC500_PWD") 'Machine PWD
        arrColumnName.Add("RP_CH2_SubmitReport") 'Submit REport
        arrColumnName.Add("RP_VC150_ReportName") 'Report name
        arrColumnName.Add("RP_VC50_AliasName") 'Alias name
        arrColumnName.Add("RP_VC150_Version") 'Version
        arrColumnName.Add("RP_VC150_PUID") 'UID
        arrColumnName.Add("RP_VC150_PPWD") 'PWD
        arrColumnName.Add("RP_VC150_FilePath") 'File
        arrColumnName.Add("RP_VC150_Role") 'Role
        arrColumnName.Add("RP_VC150_Env") 'Env
        arrColumnName.Add("RP_VC150_JobQUE") 'JobQueue
        arrColumnName.Add("RQ_NU9_SQID_PK")

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       11/01/2007
    'Purpose:           This function is used to Format grid
    'Table              t130022
    'Modify Date:       ------
    '***********************************************************************************
    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                DgReportDetail.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                DgReportDetail.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                DgReportDetail.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("ReportEntry", "FormatGrid-899", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try

    End Function
    '**************************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill machine uid and password in Txtmacuid and Txtmacpwd
    '                   Table used  t170012
    'Modify Date:       ------
    '**************************************************************************************************
    Private Function fillMachineUIDPWD() As Boolean
        Dim StrSql As String
        Dim SQID As Integer
        Dim subreport As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            'this function fetch uid and pwd from t170012 and show in txtuid and txtpwd
            StrSql = "select MM_VC500_UID,MM_VC500_PWD from T170012 where MM_NU9_DID_FK=" & DdlDomain.SelectedValue & " AND MM_NU9_MID=" & DdlMachine.SelectedValue & " "

            Dim blnStatus As Boolean
            If StrSql = "" Then
                blnStatus = False
            End If
            'reader
            Dim SqDrReport As SqlDataReader
            SqDrReport = SQL.Search("", "", StrSql, SQL.CommandBehaviour.SingleRow, blnStatus, "")
            If blnStatus = True Then
                SqDrReport.Read()
                If IsDBNull(SqDrReport.Item("MM_VC500_UID")) = False Then
                    ' reader read value from t170011 and fill text boxes
                    'UId Txtbox
                    TxtUID.Text = Decrypt(SqDrReport.Item("MM_VC500_UID"))
                    'PWD Txtbox
                    TxtPWD.Text = Decrypt(SqDrReport.Item("MM_VC500_PWD"))
                    TxtPWD.Attributes.Add("value", TxtPWD.Text)
                Else
                    TxtUID.Text = ""
                    TxtPWD.Text = ""
                    TxtPWD.Attributes.Add("value", TxtPWD.Text)
                End If
            Else
                TxtUID.Text = ""
                TxtPWD.Text = ""
                TxtPWD.Attributes.Add("value", TxtPWD.Text)
            End If
            SqDrReport.Close()
        Catch ex As Exception
        End Try
    End Function
    Private Sub DdlMachine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlMachine.SelectedIndexChanged
        'call fill machine UIDPWD function
        fillMachineUIDPWD()
    End Sub
End Class
