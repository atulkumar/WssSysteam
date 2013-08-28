Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports ION.Data
Imports Microsoft.Web.UI.WebControls
Imports System.Data


Partial Class MonitoringCenter_JobDetail
    Inherits System.Web.UI.Page

#Region " Variable Section "

    ' This enum will tell which grid row is clicked
    Private Enum WhichGrid
        UDCType = 1
        UDC = 2
    End Enum

    ' Holding text boxes created above UDC Type Grid
    Dim mtxtUDCTypeQuery As TextBox()
    ' Holding text boxes created above UDC Grid
    Dim mtxtUDCQuery As TextBox()

    ' For Storing Column name of UDCType table.
    Private Shared marUDCTypeTextBoxID As New ArrayList
    ' For Storing Textbox value above UDCType Grid.
    Private Shared marUDCTypeTextBoxValue As New ArrayList

    ' For Storing Column name of UDC table.
    Private Shared marUDCTextBoxID As New ArrayList
    ' For Storing Textbox value above UDC Grid.
    Private Shared marUDCTextBoxValue As New ArrayList

    ' Dataview used for filtering UDCType table
    Private mdvUDCType As New DataView
    ' Dataview used for filtering UDCT table
    Private mdvUDC As New DataView

    ' Dataset holding UDC Type records
    Private mdsUDCType As New DataSet
    ' Dataset holding UDC records
    Private mdsUDC As New DataSet

    ' This will hold the UDC Type Product Code
    Private Shared mintUDCTypePC As Integer
    ' This will hold the UDC Type
    Private Shared mstrUDCTypeP As String

    ' This will hold the UDC Product Code
    Private Shared mintUDCPC As Integer
    ' This will hold the UDC Type in UDC
    Private Shared mstrUDCF As String
    ' This will hold the UDC Name
    Private Shared mstrUDCName As String
    ' For making sure that textboxes are created on the UDC grid first time 
    Private Shared mblnCreateTextBox As Boolean
    ' To make sure that textbox are not created second time when UDC Type row is clicked
    Private Shared mshTxtBoxCreated As Short = 0
    ' Name of the company in UDC Table
    Private mstrUDCCompany As String

    ' Name of the company in UDCType Table
    Private mstrUDCTCompany As String

    ' Flag to check the UDC Type Lock
    Private Shared mblnLocked As Boolean

    Private Shared mblnUDCPostBack As Boolean = False
    Private Shared menGrid As WhichGrid
#End Region
    Protected WithEvents imgbtnLeft As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgbtnRight As System.Web.UI.WebControls.ImageButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cpnlUDC.Enabled = False
        cpnlUDCType.Enabled = False

        txtSelectHID.Value = ""
        Call txtCSS(Me.Page, "cpnlUDCType", "cpnlUDC")
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        Dim txthiddenvalue = Request.Form("txthidden")
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBTracing = False
        lstError.Items.Clear()

        cpnlErrorPanel.Visible = False
        GetUDCtype()

        If txthiddenvalue <> "" Then
            Try
                Select Case txthiddenvalue
                    Case "Edit"
                    Case "Print"
                        mintMonitorReportUID = mintUDCTypePC
                        Server.Transfer("../Reports/CMonitor.aspx")
                        mintMonitorReportUID = Nothing
                    Case "Delete"
                    Case "Logout"
                        LogoutWSS()
                        '******************************************
                End Select
            Catch ex As Exception
                Image1.ImageUrl = "../../Images/error_image.gif"
                MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                CreateLog("CreateJob", "JobDetail-153", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If

        ' This arraylist will hold the Column name of UDC Type table that will be displayed in the grid
        Dim arUDCTypeColumn As New ArrayList

        arUDCTypeColumn.Add("UH_NU12_UID")
        arUDCTypeColumn.Add("UH_NU12_URQID")
        arUDCTypeColumn.Add("UH_NU12_URSID")
        arUDCTypeColumn.Add("UH_NU12_UTRAN")
        arUDCTypeColumn.Add("UH_NU12_UTERR")
        arUDCTypeColumn.Add("UH_DT8_URDate")
        arUDCTypeColumn.Add("UH_NU6_URTime")

        If IsPostBack = True Then

            Try
                marUDCTypeTextBoxValue.Clear()
                ' Check the value of each text Box over UDCType grid
                For i As Integer = 0 To marUDCTypeTextBoxID.Count - 1
                    marUDCTypeTextBoxValue.Add(Request.Form("cpnlUDCType:" & marUDCTypeTextBoxID.Item(i)))
                Next

                mtxtUDCTypeQuery = CreateTextBox(Me, True, pnlUDCTypeTxtbox, grdUDCType, arUDCTypeColumn, marUDCTypeTextBoxID, marUDCTypeTextBoxValue)
                'GetUDCtype()
                UDCTypeQuery()

                ' Fill UDC grid
                If mblnUDCPostBack = False Then
                    Exit Try
                End If

                marUDCTextBoxValue.Clear()
                ' Check the value of each text Box over UDC grid
                For i As Integer = 0 To marUDCTextBoxID.Count - 1
                    marUDCTextBoxValue.Add(Request.Form("cpnlUDC:" & marUDCTextBoxID.Item(i)))
                Next

                ' This arraylist will hold the Column name of UDC table that will be displayed in the grid
                Dim arUDCColumn As New ArrayList
                'arUDCColumn.Add("UD_NU12_UDUID")
                'arUDCColumn.Add("UD_NU12_UDLNID")
                'arUDCColumn.Add("UD_VC50_UDName")
                'arUDCColumn.Add("UD_VC15_UDQUE")
                'arUDCColumn.Add("UD_VC10_UDENV")
                'arUDCColumn.Add("UD_VC10_UDUser")
                'arUDCColumn.Add("UD_NU12_UDNum")
                'arUDCColumn.Add("UD_VC20_UDHost")
                'arUDCColumn.Add("UD_VC20_UDOrg")
                'arUDCColumn.Add("UD_DT8_UDSDate")
                'arUDCColumn.Add("UD_NU6_UDSTime")
                'arUDCColumn.Add("UD_DT8_UDADate")
                'arUDCColumn.Add("UD_NU6_UDATime")
                'arUDCColumn.Add("UD_VC1_UDSTS")

                ' Create textboxes over UDC grid
                mtxtUDCQuery = CreateTextBox(Me, True, pnlUDC, grdUDC, arUDCColumn, marUDCTextBoxID, marUDCTextBoxValue)
                arUDCColumn.Clear()
                GetUDC()
                UDCQuery()
            Catch ex As Exception
                'Image1.ImageUrl = "../../Images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenel(cpnlErrorPanel, lstError, Image1, mdlMain.MSG.msgError)
                CreateLog("JobDetail", "Load-217", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        Else
            'Create text boxes over UDCType grid
            mtxtUDCTypeQuery = CreateTextBox(Me, False, pnlUDCTypeTxtbox, grdUDCType, arUDCTypeColumn, marUDCTypeTextBoxID, marUDCTypeTextBoxValue)
            'imgbtnRight.Attributes.Add("onclick", "ShowContents()")
            'imgbtnLeft.Attributes.Add("onclick", "HideContents()")

            cpnlUDC.State = CustomControls.Web.PanelState.Collapsed
            'cpnlUDC.Enabled = False
            cpnlUDC.TitleCSS = "test2"
        End If

        arUDCTypeColumn.Clear()

        'Security Block

        'Dim intId As Integer

        'If Not IsPostBack Then
        '    Dim str As String
        '    str = HttpContext.Current.Session("PropRootDir")
        '    intId = Request.QueryString("ScrID")
        '    Dim obj As New clsSecurityCache
        '    If obj.ScreenAccess(intId) = False Then
        '        Response.Redirect("../../frm_NoAccess.aspx")
        '    End If
        '    obj.ControlSecurity(Me.Page, intId)
        'End If
        'End of Security Block
    End Sub

    Private Sub grdUDCType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUDCType.ItemDataBound
        ' This will change the UDCType grid row color on row click
        Dim itemType As ListItemType = e.Item.ItemType

        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
            e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        End If

    End Sub

    Private Sub grdUDC_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUDC.ItemDataBound
        ' This will change the UDC grid row color on row click
        Dim itemType As ListItemType = e.Item.ItemType

        If ((itemType = ListItemType.Pager) Or (itemType = ListItemType.Header) Or (itemType = ListItemType.Footer)) Then
            Return
        Else
            Dim button As LinkButton = CType(e.Item.Cells(0).Controls(0), LinkButton)
            e.Item.Attributes("onclick") = Page.GetPostBackClientHyperlink(button, "")
        End If

    End Sub



    Private Sub imgbtnLeft_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnLeft.Click
        imgbtnLeft.Attributes.Add("onclick", "HideContents()")
        imgbtnLeft.Visible = False
        imgbtnRight.Visible = True
    End Sub

    Private Sub imgbtnRight_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnRight.Click
        imgbtnRight.Attributes.Add("onclick", "ShowContents()")
        imgbtnLeft.Visible = True
        imgbtnRight.Visible = False
    End Sub



    Private Sub GetUDCtype()
        'SQL.DBTable = "T130111"

        Try
            mdsUDCType.Clear()

            If SQL.Search("T130111", "JobDetail", "GetUDCtype", "select * from T130111", mdsUDCType, "", "") = True Then
                'chkUDCParam.Checked = False
                grdUDCType.DataSource = mdsUDCType.Tables("T130111")
                mdvUDCType.Table = mdsUDCType.Tables("T130111")
                grdUDCType.DataBind()
            End If

        Catch ex As Exception
            CreateLog("JobDetail", "GetUDCType-289", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Private Sub GetUDC()
        'SQL.DBTable = "T130122"

        Try
            mdsUDC.Clear()

            If SQL.Search("T130122", "JObDetail", "GetUDC", "select * from T130122 where UD_NU12_UDUID=" & mintUDCTypePC & "", mdsUDC, "", "") = True Then
                cpnlUDC.State = CustomControls.Web.PanelState.Expanded
                cpnlUDC.Enabled = True
                mdvUDC.Table = mdsUDC.Tables("T130122")
                grdUDC.DataSource = mdsUDC.Tables("T130122")
                grdUDC.DataBind()

                cpnlUDC.TitleCSS = "test"

                ' This arraylist will hold the Column name of UDC table that will be displayed in the grid
                Dim arUDCColumnName As ArrayList

                If mblnCreateTextBox = True Then
                    arUDCColumnName = New ArrayList



                    arUDCColumnName.Add("UD_NU12_UDUID")
                    arUDCColumnName.Add("UD_NU12_UDLNID")
                    arUDCColumnName.Add("UD_VC50_UDName")
                    arUDCColumnName.Add("UD_VC15_UDQUE")
                    arUDCColumnName.Add("UD_VC10_UDENV")
                    arUDCColumnName.Add("UD_VC10_UDUser")
                    arUDCColumnName.Add("UD_NU12_UDNum")
                    arUDCColumnName.Add("UD_VC20_UDHost")
                    arUDCColumnName.Add("UD_VC20_UDOrg")
                    arUDCColumnName.Add("UD_DT8_UDSDate")
                    arUDCColumnName.Add("UD_NU6_UDSTime")
                    arUDCColumnName.Add("UD_DT8_UDADate")
                    arUDCColumnName.Add("UD_NU6_UDATime")
                    arUDCColumnName.Add("UD_VC1_UDSTS")
                    mtxtUDCQuery = CreateTextBox(Me, False, pnlUDC, grdUDC, arUDCColumnName, marUDCTextBoxID, marUDCTextBoxValue)

                    arUDCColumnName.Clear()
                    mblnCreateTextBox = False
                    ' Making sure that text box are not created again
                    mshTxtBoxCreated = 1
                    mblnUDCPostBack = True
                End If
            Else
                cpnlUDC.TitleCSS = "test2"
                cpnlUDC.State = CustomControls.Web.PanelState.Expanded
                cpnlUDC.Text = "UDC - No data found"
                grdUDC.DataSource = mdsUDC.Tables("T130122")
                grdUDC.DataBind()
            End If
        Catch ex As Exception
            CreateLog("JobDetail", "GetUDC-347", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Sub

    Private Sub UDCTypeQuery()
        Dim strRowFilterString As String
        Dim strSearch As String

        Try
            For intI As Integer = 0 To marUDCTypeTextBoxID.Count - 1
                ' Check for the values in the textboxes
                If Not mtxtUDCTypeQuery(intI).Text.Trim.Equals("") Then

                    strSearch = mtxtUDCTypeQuery(intI).Text

                    ' Create the query string to be applied to dataview
                    ' Check for Int, decimal and Datetime datatype. Because in dataview filter like does not work for these.
                    If (mdvUDCType.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvUDCType.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvUDCType.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvUDCType.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mtxtUDCTypeQuery(intI).Text.Trim
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvUDCType.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

        Catch ex As Exception
            CreateLog("JobDetail", "UDCTypeQuery-377", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
        ' if filter string is empty then fill the UDC grid with all the data
        If (strRowFilterString Is Nothing) Then
            GetUDCtype()
            Exit Sub
        End If

        ' Remove the  and from the end of the string
        strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
        ' Apply the filter
        mdvUDCType.RowFilter = strRowFilterString

        grdUDCType.DataSource = mdvUDCType
        grdUDCType.DataBind()

    End Sub

    Private Sub UDCQuery()

        Dim strRowFilterString As String
        Dim strSearch As String

        Try
            For intI As Integer = 0 To marUDCTextBoxID.Count - 1
                ' Check for the values in the textboxes
                If Not mtxtUDCQuery(intI).Text.Trim.Equals("") Then

                    strSearch = mtxtUDCQuery(intI).Text
                    ' Create the query string to be applied to dataview
                    ' Check for Int, decimal and Datetime datatype. Because in dataview filter like does not work for these.
                    If (mdvUDC.Table.Columns(intI).DataType.FullName = "System.Decimal") Or (mdvUDC.Table.Columns(intI).DataType.FullName = "System.Int32") Or (mdvUDC.Table.Columns(intI).DataType.FullName = "System.DateTime") Then
                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvUDCType.Table.Columns(intI).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = mtxtUDCQuery(intI).Text.Trim
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvUDC.Table.Columns(intI).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If

            Next

        Catch ex As Exception
            CreateLog("JobDetail", "UDCQuery", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
        ' if filter string is empty then fill the UDC grid with all the data
        If (strRowFilterString Is Nothing) Then
            GetUDC()
            Exit Sub
        End If

        ' Remove the  and from the end of the string
        strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
        ' Apply the filter
        mdvUDC.RowFilter = strRowFilterString

        grdUDC.DataSource = mdvUDC
        grdUDC.DataBind()
    End Sub


    Private Sub grdUDCType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdUDCType.SelectedIndexChanged
        Try
            mintUDCTypePC = grdUDCType.SelectedItem.Cells(1).Text.Trim
        Catch ex As Exception
            CreateLog("JobDetail", "SelectedIndexChanged-443", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdUDCType")
        End Try

        mblnCreateTextBox = True
        GetUDC()

    End Sub

End Class
