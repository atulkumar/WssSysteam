Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient

Partial Class MonitoringCenter_MacProRel
    Inherits System.Web.UI.Page
    Private Shared dvSearch As New DataView
    Shared mintID As Integer
    Private Shared arrTextboxName As New ArrayList
    Private Shared arrColWidth As New ArrayList

    'Protected WithEvents TxtCompanyId As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtDomain As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtProcessId As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtMachine As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DdlDomain As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cpnlMachineProcess As CustomControls.Web.CollapsiblePanel
    Private Shared arrColumnName As New ArrayList

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        cpnlError.Visible = False

        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")
        If IsPostBack Then
            If strhiddenImage <> "" Then

                Select Case strhiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select

            End If
        End If
        'CDDLDomainType_F.CDDLQuery = "select DM_NU9_DID AS ID, DM_VC150_DomainName  as Name,DM_VC150_DomainName AS Description from T170011 where DM_NU9_Company_ID_FK='" + DdlCompany.SelectedValue + "' "
        'CDDLDomainType_F.CDDLUDC = False
        'CDDLDomainType_F.CDDLType = CustomDDL.DDLType.FastEntry

        If Not (Page.IsPostBack) Then
            cpnlError.Visible = False
            GetCompany()
            GetProcessId()
            BINDGrid()
            FormatGrid()

        Else
            SearchGrid()

        End If

        Dim intID1 As Int32
        'If Not IsPostBack Then
        Dim str As String
        str = HttpContext.Current.Session("PropRootDir")
        intID1 = 535
        Dim obj1 As New clsSecurityCache
        If obj1.ScreenAccess(intID1) = False Then
            Response.Redirect("../frm_NoAccess.aspx")
        End If
        obj1.ControlSecurity(Me.Page, intID1)
        'End If
        'End If
        'End of Security Block
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function is used for search in text boxes 
    '                   Table T010011
    'Modify Date:       ------
    '***************************************************************************************


    Private Function SearchGrid()
        'strFilter is used to get value of search textbox
        Dim strFilter As String

        If Not TxtCompanyId.Text.Trim.Equals("") Then
            strFilter = "CI_VC36_Name like '" & TxtCompanyId.Text.Trim & "%' and "
        End If

        If Not TxtDomain.Text.Trim.Equals("") Then
            strFilter &= "DM_VC150_DomainName like '" & TxtDomain.Text.Trim & "%' and "
        End If

        If Not TxtProcessId.Text.Trim.Equals("") Then
            strFilter &= "PM_VC20_PName like '" & TxtProcessId.Text.Trim & "%' and "
        End If

        If Not TxtMachine.Text.Trim.Equals("") Then
            strFilter &= "MM_VC150_Machine_Name like '" & TxtMachine.Text.Trim & "%' and "
        End If

        If (IsNothing(strFilter)) = True Then
        ElseIf (strFilter.Trim.Equals(String.Empty)) = True Then
        Else
            strFilter = strFilter.Remove(strFilter.Length - 4, 4)

            dvSearch.RowFilter = strFilter
            DgDataEntry.DataSource = dvSearch
            DgDataEntry.DataBind()
        End If
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill company dropdown 
    '                   Table T010011
    'Modify Date:       ------
    '***************************************************************************************

    Private Function GetCompany() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select CI_NU8_Address_Number,CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type='COM'"
            If SQL.Search("T010011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then

                DdlCompany.DataSource = dsTemp.Tables(0)
                DdlCompany.DataTextField = "CI_VC36_Name"
                DdlCompany.DataValueField = "CI_NU8_Address_Number"
                DdlCompany.DataBind()
                DdlCompany.Items.Insert(0, "Select")
            Else
                lstError.Items.Add("Sorry No Machine Available For Selected Domain ")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                cpnlMachineProcess.State = CustomControls.Web.PanelState.Collapsed
                cpnlMachineProcess.Enabled = False

                Return False
            End If

        Catch ex As Exception
            CreateLog("dataobjectentry", "FillCompany", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing

        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill domain dropdown list according to selected company  
    'Modify Date:       ------
    '***************************************************************************************

    Private Function GetDomain() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select DM_VC150_DomainName,DM_NU9_DID_PK from T170011 where DM_NU9_Company_ID_FK=" + DdlCompany.SelectedValue + " "
            If SQL.Search("T170011", "dataobjentry", "FILLCompany", sqstr, dsTemp, "", "") = True Then
                'Sql.search is true then ddldomain will fill with domain
                DdlDomain.DataSource = dsTemp.Tables(0)
                DdlDomain.DataTextField = "DM_VC150_DomainName"
                DdlDomain.DataValueField = "DM_NU9_DID_PK"
                DdlDomain.DataBind()
                DdlDomain.Items.Insert(0, New ListItem("Select", "0"))

            Else
                'Sql.search is False then msgPanel Show Msg
                lstError.Items.Clear()
                lstError.Items.Add(" Sorry no Domain Available for Selected Company ")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If

        Catch ex As Exception
            CreateLog("dataobjectentry", "FillDomain", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing

        End Try
    End Function


    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill domain dropdown list according to selected company  
    'Modify Date:       ------
    '***************************************************************************************
    'Private Function FillFEDropdowns() As Boolean

    '    CDDLDomainType_F.CDDLFillDropDown(10, False)

    'End Function

    Private Function GetProcessId() As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select PM_NU9_PCode,PM_VC20_PName from T130031 "
            If SQL.Search("T130031 ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                'Sql.search is true then ddlProcessID  fill with Process
                DdlProcessId.DataSource = dsTemp.Tables(0)
                DdlProcessId.DataTextField = "PM_VC20_PName"
                DdlProcessId.DataValueField = "PM_NU9_PCode"
                DdlProcessId.DataBind()
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("dataobjectentry", "FillProcessId", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing

        End Try
    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill Machine dropdown list according to selected Domain  
    'Modify Date:       ------
    '***************************************************************************************
    Private Function GetMachine(ByVal Domain As Integer)
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select MM_VC150_Machine_Name ,MM_NU9_MID   from t170012 where MM_NU9_DID_FK=" & Domain & " and  MM_CH1_IsEnable='E' "
            If SQL.Search("T170012 ", "dataobjentry", "FILLProcessId", sqstr, dsTemp, "", "") = True Then
                'Sql.search is true then ddlMachine will fill with Machine acc to selected domain
                DdlMachineName.DataSource = dsTemp.Tables(0)
                DdlMachineName.DataTextField = "MM_VC150_Machine_Name"
                DdlMachineName.DataValueField = "MM_NU9_MID"
                DdlMachineName.DataBind()

            Else
                'Sql.search is False then msgPanel Show Msg
                lstError.Items.Clear()
                lstError.Items.Add("Sorry No Machine Available For Selected Domain ")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
                Return False
            End If

        Catch ex As Exception
            CreateLog("Reportin peoplesoft", "GetMachine", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try
    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function Save record intable t130033  
    'Modify Date:       ------
    '***************************************************************************************
    Private Function SAVERecord() As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            Dim arColName As New ArrayList
            Dim arRowData As New ArrayList

            arColName.Add("MP_NU9_CompanyID_FK_PK")
            arColName.Add("MP_NU9_DomainID_FK_PK")
            arColName.Add("MP_NU9_ProcessID_FK_PK")
            arColName.Add("MP_NU9_MachineID_FK_PK")

            arRowData.Add(DdlCompany.SelectedValue)
            arRowData.Add(DdlDomain.SelectedValue)
            arRowData.Add(DdlProcessId.SelectedValue)
            arRowData.Add(DdlMachineName.SelectedValue)

            If SQL.Save("t130033", "dataobjectentry", "SaveRecord", arColName, arRowData, "") = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("dataobjectentry", "SAVERecord", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try

    End Function
    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function delete record from grid  
    'Modify Date:       ------
    '***************************************************************************************
    Private Function DeleteRecord(ByVal ID As Integer)
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "delete from t130033 where MP_NU9_ID_PK=" & ID
            If SQL.Delete("Dataobjentry", "deleterecod", sqstr, SQL.Transaction.Serializable, "") = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try

    End Function

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function fill grid with data  
    'Modify Date:       ------
    '***************************************************************************************
    Private Function BINDGrid() As Boolean
        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString.ToString
            SQL.DBTracing = False
            Dim dsTemp As New DataSet
            Dim sqstr As String
            sqstr = "select " & _
            "b.CI_VC36_Name,c.DM_VC150_DomainName,d.PM_VC20_PName,e.MM_VC150_Machine_Name ,f.MP_NU9_ID_PK MPID " & _
            "from t010011 b,t170011 c,t130031 d,t170012 e,t130033 f where " & _
            "f.MP_NU9_CompanyID_FK_PK=b.CI_NU8_Address_Number and " & _
            "f.MP_NU9_DomainID_FK_PK=c.DM_NU9_DID_PK and " & _
            "f.MP_NU9_ProcessID_FK_PK=d.PM_NU9_PCode and " & _
            "f.MP_NU9_MachineID_FK_PK=e.MM_NU9_MID and " & _
            "f.MP_NU9_DomainID_FK_PK=e.MM_NU9_DID_FK"

            SQL.Search("T130033 ", "dataobjentry", "BINDGrid", sqstr, dsTemp, "", "")
            'SQL.Search is true then we fill dvsearch Dataview with dsTemp(Dataset) for search
            dvSearch = dsTemp.Tables(0).DefaultView
            DgDataEntry.DataSource = dvSearch
            DgDataEntry.DataBind()

        Catch ex As Exception
            CreateLog("dataobjectentry", "Bindgrid", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes.ToString, ex.ToString, "Mandeep", "", "", False)
            Return Nothing
        End Try

    End Function


    Private Sub ImgbtnSAVE_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgbtnSAVE.Click

        If ValidateRequest() = False Then
            Exit Sub
        End If

        If SAVERecord() = True Then
            'saverecord is true then entries saved in database and msgpnl show msg
            lstError.Items.Clear()
            lstError.Items.Add("Record saved successfully...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            BINDGrid()
            DdlCompany.SelectedIndex = -1
            DdlProcessId.SelectedIndex = -1
            DdlDomain.Items.Clear()
            DdlMachineName.Items.Clear()

            DgDataEntry.SelectedIndex = -1

        Else
            'saverecord is false then msgpnl show msg
            lstError.Items.Add("Record already saved ...")
            lstError.Items.Add("Choose different ProcessID or MachineID")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
        End If
    End Sub

    '***************************************************************************************
    'Created By:        Mandeep
    'Create Date:       ------
    'Purpose:           This function check taht no field should be empty before save record  
    'Modify Date:       ------
    '***************************************************************************************
    Private Function ValidateRequest() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        'if anytext box empty then shFlag set to 1

        If DdlDomain.SelectedValue.Equals("0") Then
            lstError.Items.Add("Select Domain ...")
            shFlag = 1
        End If

        If DdlMachineName.SelectedValue.Equals("") Then
            lstError.Items.Add("Machine Type cannot be blank...")
            shFlag = 1
        End If


        If shFlag = 1 Then
            'shgFlag=1 then msg panel show msg 
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            'shgFlag=0 then all fields are fill 
            Return True
        End If

    End Function

    Private Sub DgDataEntry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DgDataEntry.SelectedIndexChanged
        mintID = DgDataEntry.SelectedItem.Cells(1).Text.Trim

    End Sub

    Private Sub DgDataEntry_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgDataEntry.ItemDataBound
        Dim itemType As ListItemType = e.Item.ItemType

        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)

            e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        End If
    End Sub

    Private Sub Imgbtndelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imgbtndelete.Click
        If DgDataEntry.SelectedIndex = -1 Then
            lstError.Items.Clear()
            lstError.Items.Add(" Please First  Select Row...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)

        Else
            If DeleteRecord(mintID) = True Then
                lstError.Items.Clear()
                lstError.Items.Add("Record deleted successfully...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                BINDGrid()
                DgDataEntry.SelectedIndex = -1
            End If
        End If
    End Sub

    Private Function FormatGrid()
        Try

            For inti As Integer = 1 To arrColWidth.Count - 1
                DgDataEntry.Columns(inti).HeaderStyle.Width = Unit.Pixel(arrColWidth(inti))
                DgDataEntry.Columns(inti).ItemStyle.Width = Unit.Pixel(arrColWidth(inti))
                DgDataEntry.Columns(inti).ItemStyle.Wrap = True
            Next
        Catch ex As Exception
            CreateLog("BGDailyMonitor", "FormatGrid-133", LogType.Application, LogSubType.Exception, "", ex.ToString)
        End Try
    End Function

    Private Sub DgDataEntry_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DgDataEntry.PageIndexChanged
        DgDataEntry.CurrentPageIndex = e.NewPageIndex
        BINDGrid()

    End Sub

    Private Function DefineGridColumnData()
        arrColWidth.Clear()
        arrColWidth.Add(0)
        arrColWidth.Add(50) 'company id
        arrColWidth.Add(85) 'domain
        arrColWidth.Add(85) 'process id
        arrColWidth.Add(85) 'machine


        arrTextboxName.Clear()
        arrTextboxName.Add("Textbox1")
        arrTextboxName.Add("txtDomain")
        arrTextboxName.Add("txtProcess")
        arrTextboxName.Add("txtMachine")


        arrColumnName.Clear()
        arrColumnName.Add("CI_VC36_Name") 'Company id
        arrColumnName.Add("DM_VC150_DomainName") 'Domain
        arrColumnName.Add("PM_VC20_PName") 'Process
        arrColumnName.Add("MM_VC150_Machine_Name") 'machine
        arrColumnName.Add("MPID")

    End Function
    Private Sub DdlCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlCompany.SelectedIndexChanged
        'CDDLDomainType_F.CDDLSetBlank()
        DdlDomain.Items.Clear()
        DdlMachineName.Items.Clear()
        'calling Getdomain Function
        GetDomain()
        'FillFEDropdowns()
    End Sub
    Private Sub DdlDomain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlDomain.SelectedIndexChanged

        DdlMachineName.Items.Clear()
        'calling get machine function
        GetMachine(DdlDomain.SelectedValue)

    End Sub
    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("Configuration.aspx")
    End Sub
    Private Sub DdlMachineName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlMachineName.SelectedIndexChanged
        'If DdlMachineName.SelectedIndex.Equals(0) Then
        '    lstError.Items.Add("Select Machine")
        '    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgWarning)
        'End If

    End Sub
End Class
