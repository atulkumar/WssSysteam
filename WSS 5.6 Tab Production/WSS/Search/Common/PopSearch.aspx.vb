'************************************************************************************************************
' Page                 :-Pop Search
' Purpose              :-Purpose of this screen is to search any User defined Code (UDC).   
' Date				Author						Modification Date					Description
' 4/03/06			Jaswinder		           -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports System.Data



Partial Class Search_Common_PopSearch
    Inherits System.Web.UI.Page
    Protected Shared colcount As Integer
    Protected Shared widt As Unit
    Protected Shared exwid As Unit
    Protected txtwid As Unit
    Protected btcwid As Unit
    Protected Shared dgcolcount As Integer
    Protected Shared mdvtable As DataView = New DataView

    Private Shared strtbname As String
    Private Shared arrtx() As String
    Private Shared strQuerry As String

    Dim rowvalue As Integer
    Dim strRowFilterString As String
    Dim strSearch As String
    Private sqlcon As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
    Dim dsdataset As New DataSet
    'Protected WithEvents imgOk As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgClose As System.Web.UI.WebControls.ImageButton
    Dim flagststus As Integer
    Public strNameParam As String
    'Protected WithEvents txtFastSearch As System.Web.UI.WebControls.TextBox
    Public strIdParam As String


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents pndgsrch As System.Web.UI.WebControls.Panel
    Protected WithEvents txtValue As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Toolbar2 As Microsoft.Web.UI.WebControls.Toolbar
    Protected WithEvents imgbtnSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents DropDownList19 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tbrRight As Microsoft.Web.UI.WebControls.Toolbar
    ' Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    ' Protected WithEvents lblError As System.Web.UI.WebControls.Label
    'Protected WithEvents cpnlErrorPanel As CustomControls.Web.CollapsiblePanel
    ' Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    ' Protected WithEvents btsearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtargument As System.Web.UI.WebControls.TextBox
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'If User.Identity.Name = "" Then
        '    Response.Write("<script>window.close(); </script>")
        'End If

        imgOk.Attributes.Add("Onclick", "return closeWindow('Ok');")
        imgClose.Attributes.Add("Onclick", "return closeWindow('Close');")


        Dim txthiddenvalue = Request.Form("txthidden")
        If txthiddenvalue <> "" Then
            Try
                Select Case txthiddenvalue
                    Case "Search"
                        BindSet()
                        bindgrid()
                        DataGrid1.DataSource = mdvtable
                        DataGrid1.DataBind()
                        createTestBox()
                        Call Button1_Click(Button1, New EventArgs)
                        Exit Sub
                End Select
            Catch ex As Exception
                CreateLog("PopSearch", "Load-90", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        If Not IsPostBack Then

            Try

                HttpContext.Current.Session("strtbname") = Request.QueryString("tbname")
                strQuerry = Request.QueryString("ID")
                '& txtViewName.Text.Trim.Replace(" ' ", " ' ' ")
                strQuerry = strQuerry.Replace("""", "'")
                ''''''''''''''''''''''''''UPdated for keyboard scrolling''''''''''''''''''''''
                Dim sqRDR As SqlClient.SqlDataReader
                Dim blnStatus As Boolean
                sqRDR = SQL.Search("", "", strQuerry, SQL.CommandBehaviour.Default, blnStatus)
                While sqRDR.Read
                    strIdParam = strIdParam & sqRDR(0) & "###"
                    strNameParam = strNameParam & sqRDR(1) & "###"
                End While
                sqRDR.Close()
            Catch ex As Exception
                CreateLog("PopSearch", "Load-116", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try

            DataGrid1.Attributes.Add("ONKEYDOWN", "return checkArrows(this, event, '" & strIdParam & "', '" & strNameParam & "', '" & HttpContext.Current.Session("strtbname") & "')")
            DataGrid1.Attributes.Add("onkeypress", "Select('" & HttpContext.Current.Session("strtbname") & "')")


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            BindSet()
            bindgrid()
            createTestBox()
            Button1.Attributes.Add("onclick", "KeyCheck()")
        Else
            BindSet()
            bindgrid()
            DataGrid1.DataSource = mdvtable
            DataGrid1.DataBind()
            createTestBox()
            Call Button1_Click(Button1, New EventArgs)
        End If
        Dim wdt As Unit
        wdt = pndgsrch.Width
        DataGrid1.Width = wdt

    End Sub

#Region "Bind Dataset"
    Private Sub BindSet()

        If Not dsdataset.Tables("TOE_0011") Is Nothing Then
            dsdataset.Tables("TOE_0011").Clear()
        End If

        Try
            Dim dasqlda As SqlDataAdapter = New SqlDataAdapter(strQuerry, sqlcon)
            dasqlda.Fill(dsdataset, "TOE_0011")
            mdvtable.Table = dsdataset.Tables("TOE_0011")
            DataGrid1.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
            DataGrid1.Width = pndgsrch.Width
            DataGrid1.DataSource = mdvtable
            DataGrid1.DataBind()
        Catch ex As Exception
            CreateLog("PopSearch", "BindSet-131", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "Bind grid"
    Sub bindgrid()
        colcount = mdvtable.Table.Columns.Count
        DataGrid1.AutoGenerateColumns = False
        DataGrid1.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke
        'DataGrid1.Width = pndgsrch.Width

        Dim count As Integer
        Try
            For count = 0 To colcount - 1
                Dim bcl As BoundColumn = New BoundColumn
                'we can also bind it to the datatable 
                If (count = 0) Then
                    bcl.ItemStyle.Width = New System.Web.UI.WebControls.Unit(90)
                Else
                    bcl.ItemStyle.Width = New System.Web.UI.WebControls.Unit(200)
                End If
                bcl.DataField = mdvtable.Table.Columns(count).ColumnName
                bcl.HeaderText = mdvtable.Table.Columns(count).ColumnName
                DataGrid1.Columns.Add(bcl)
            Next
        Catch ex As Exception
            CreateLog("PopSearch", "BindGrid-158", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
#End Region

#Region "create Boxes"

    Sub createTestBox()
        Dim c As Integer

        pndgsrch.Controls.Clear()
        pndgsrch.Controls.Add(btsearch)

        pndgsrch.Width = DataGrid1.Width
        dgcolcount = DataGrid1.Columns.Count

        Dim wid As String
        Dim txtnew As TextBox
        Dim tcount As Integer

        Try
            If Not IsPostBack Then
                For tcount = 1 To dgcolcount - 1
                    txtnew = New TextBox
                    btcwid = DataGrid1.Columns(tcount).ItemStyle.Width
                    wid = btcwid.Value + 4
                    wid = wid & "pt"
                    If (tcount = 1) Then
                        txtnew.Width = New System.Web.UI.WebControls.Unit(92)
                    Else
                        txtnew.Width = New System.Web.UI.WebControls.Unit(202)
                    End If
                    ' txtnew.Width = DataGrid1.Columns(tcount).ItemStyle.Width
                    txtnew.ID = DataGrid1.Columns(tcount).HeaderText

                    txtnew.BackColor = New System.Drawing.Color
                    ' txtnew.BackColor = Color.FromArgb(253, 254, 223)
                    txtnew.CssClass = "SearchTxtBox"
                    pndgsrch.Controls.Add(txtnew)
                Next
            Else
                'if after post back then create the equal number of the text boxes and text remauning same
                For tcount = 1 To dgcolcount - 1
                    txtnew = New TextBox
                    btcwid = DataGrid1.Columns(tcount).ItemStyle.Width
                    wid = btcwid.Value + 4
                    wid = wid & "pt"
                    If (tcount = 1) Then
                        txtnew.Width = New System.Web.UI.WebControls.Unit(92)
                    Else
                        txtnew.Width = New System.Web.UI.WebControls.Unit(202)
                    End If

                    txtnew.CssClass = "SearchTxtBox"
                    txtnew.ID = DataGrid1.Columns(tcount).HeaderText
                    pndgsrch.Controls.Add(txtnew)

                Next
            End If
        Catch ex As Exception
            CreateLog("PopSearch", "createTestBox-218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

#Region "Data Grid Item command"

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        Try
            'DataGrid1.DataSource = mdvtable
            'DataGrid1.DataBind()

            Dim test As String
            Dim i As Integer

            If e.CommandName = "select" Then
                For i = 0 To e.Item.Cells.Count - 1
                    e.Item.Cells(i).BackColor = System.Drawing.Color.LightGray
                Next
            End If
        Catch ex As Exception
            CreateLog("PopSearch", "ItemCommand-241", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "datagrid1")
        End Try
    End Sub

#End Region

#Region "Search Button"
    Private Sub btsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' get the txt of the textboxes created at runtime 
        getText()
        Querrystring()

    End Sub
#End Region

#Region "Get Text"
    Sub getText()
        Dim gtcount As Integer
        gtcount = pndgsrch.Controls.Count
        ReDim arrtx(gtcount - 1)
        Dim tx As TextBox
        Dim inttx As Integer

        For inttx = 1 To gtcount - 1
            tx = CType(pndgsrch.Controls(inttx), TextBox)
            arrtx(inttx - 1) = tx.Text
            ' Response.Write(arrtx(inttx))
        Next
    End Sub
#End Region

#Region "QueryString"

    Sub Querrystring()
        Dim qcount As Integer

        Try
            For qcount = 0 To colcount - 1
                If Not (arrtx(qcount).Equals("")) Then
                    strSearch = arrtx(qcount)
                    'delibrately put the " * " afetr the text of the search
                    'strSearch = strSearch + "*"
                    If (mdvtable.Table.Columns(qcount).DataType.FullName = "System.Decimal") Or (mdvtable.Table.Columns(qcount).DataType.FullName = "System.Int32") Or (mdvtable.Table.Columns(qcount).DataType.FullName = "System.DateTime") Then

                        If (mdvtable.Table.Columns(qcount).DataType.FullName = "System.DateTime") = True Then
                            Dim chk As Date

                            If IsDate(strSearch) Then
                            Else
                                '***************give some message
                                Exit Sub
                            End If

                        End If

                        strSearch = strSearch.Replace("*", "")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(qcount).ColumnName & " = '" & strSearch & "' AND "
                    Else
                        strSearch = strSearch.Replace("*", "%")
                        strRowFilterString = strRowFilterString & mdvtable.Table.Columns(qcount).ColumnName & " like " & "'" & strSearch & "' AND "
                    End If
                End If
            Next

            rowvalue = 0

            If (strRowFilterString Is Nothing) Then
                BindSet()
                Exit Sub
            End If

            If strRowFilterString.Length.Equals(0) Then
                BindSet()
                Exit Sub
            End If
            DataGrid1.Width = pndgsrch.Width

            strRowFilterString = strRowFilterString.Remove((strRowFilterString.Length - 4), 4)
            mdvtable.RowFilter = strRowFilterString

            DataGrid1.DataSource = mdvtable
            DataGrid1.DataBind()

        Catch ex As Exception
            CreateLog("PopSearch", "QueryString-325", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

#End Region

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Try

            Dim dv As DataView = mdvtable
            Dim dcCol As DataColumn
            Dim dc As DataColumnCollection = dv.Table.Columns
            Dim strID As String
            Dim strTempName As String

            For Each dcCol In dv.Table.Columns
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    strID = DataGrid1.DataKeys(e.Item.ItemIndex)
                    strTempName = e.Item.Cells(2).Text

                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("style", "cursor:hand")
                    'e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & HttpContext.Current.Session("strtbname") & "', '" & rowvalue & "','" & strTempName & "')")
                    e.Item.Attributes.Add("onclick", "KeyCheck('" & e.Item.Cells(1).Text.Trim & "' ,'" & e.Item.Cells(2).Text.Trim & "', '" & e.Item.ItemIndex + 1 & "', '" & HttpContext.Current.Session("strtbname") & "', '" & strIdParam & "','" & strTempName & "', '" & strNameParam & "')")
                    e.Item.Cells(dc.IndexOf(dc(dcCol.ColumnName))).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & HttpContext.Current.Session("strtbname") & "', '" & rowvalue & "','" & strTempName & "')")
                End If
            Next
            rowvalue += 1

        Catch ex As Exception
            CreateLog("PopSearch", "ItemDataBound-353", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub Toolbar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim objButtons As ToolbarItem = CType(sender, ToolbarItem)

        Try
            Select Case objButtons.ID
                Case "tbbtnok"
                    Button1_Click(Me, New EventArgs)
                    Response.Write("<script>window.close();</script>")
                Case "tbrbtnClose"

            End Select
        Catch ex As Exception
            CreateLog("PopSearch", "Toolbar1_ButtonClick-369", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        getText()
        Querrystring()
    End Sub
End Class
