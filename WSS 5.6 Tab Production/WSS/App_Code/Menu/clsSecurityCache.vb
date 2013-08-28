'*******************************************************************
' Page                   : - class clsSecurityCache
' Purpose              : - It will return the mail message
' Date		    			Author						Modification Date					Description
' 12/02/06				Jagtar 					06/03/2006        					Created
'
' Notes: This class hold the function GetMail(). This function accepts some parameters and 
'             returns the complete mail message.
' Code:
'*******************************************************************

Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports System.Web.Caching.Cache
Imports System.Web.HttpContext
Imports System.Data


Public Class clsSecurityCache
    Private dtData As DataTable
    Private mobjTreeMenu As System.Web.UI.WebControls.TreeView   ' This will be called by the page where we will create the side menu
    Private mobjParentNode As System.Web.UI.WebControls.TreeNode    ' This will be the Parent node in the Menu
    Private mobjChildNode As System.Web.UI.WebControls.TreeNode   ' Child node under parent node
    Private strEnable, strView, strlblAlias As String

    Public Function FillObjectData(ByVal MenuPnl As Panel, ByVal MenuName As String, ByVal ScreenName As String)

        MenuPnl.Controls.Clear()
        Dim dsObject As New DataSet
        Dim obj As New clsSecurityCache
        Dim dt As DataTable
        If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
            obj.FillCache()
        End If
        dt = Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")

        obj.dtData = dt
        Dim intCountMenu, intCountScreen, i, j As Integer
        Dim dvMenu As DataView
        Dim dvScreen As DataView
        Dim strURL As String
        Dim str As String
        dvMenu = obj.FilterCacheTable("MNU")
        dvScreen = obj.FilterCacheTable("SCR")
        intCountMenu = dvMenu.Count
        mobjTreeMenu = New System.Web.UI.WebControls.TreeView
        mobjTreeMenu.Nodes.Clear()
        ' Parent Node
        'mobjParentNode = New System.Web.UI.WebControls.TreeNode

        'hide plus sign in side menu
        '******************
        mobjTreeMenu.ShowLines = False
        mobjTreeMenu.ShowExpandCollapse = True
        mobjTreeMenu.EnableTheming = False
        'mobjTreeMenu.CssClass = " ../Images/Js/StyleSheet1/TreeNodeCSS"
        'mobjTreeMenu.SelectedNodeStyle.BackColor = Drawing.Color.White
        mobjTreeMenu.ExpandImageUrl = "~/Images/Leaf.jpg"
        mobjTreeMenu.CollapseImageUrl = "~/Images/Leaf.jpg"
        mobjTreeMenu.NoExpandImageUrl = "~/Images/Leaf.jpg"
        Dim browser As HttpBrowserCapabilities = HttpContext.Current.Request.Browser

        'mobjTreeMenu.NodeStyle.NodeSpacing = Unit.Pixel(1)
        mobjTreeMenu.ParentNodeStyle.ForeColor = Drawing.Color.Blue
        If (browser.Version.Equals("8.0")) Then
            mobjTreeMenu.NodeStyle.VerticalPadding = Unit.Pixel(2)
        Else
            mobjTreeMenu.NodeStyle.VerticalPadding = Unit.Pixel(4)
        End If
        mobjTreeMenu.NodeStyle.CssClass = "content"


        Try
            For i = 0 To intCountMenu - 1
                mobjParentNode = New System.Web.UI.WebControls.TreeNode
                mobjParentNode.SelectAction = TreeNodeSelectAction.Expand


                ''mobjParentNode.SelectAction = TreeNodeSelectAction.SelectExpand

                mobjParentNode.Expanded = False
                ' Set the parent node text
                mobjParentNode.Text = "<font Size=""1"" face=""verdana"" color=""Black"" background color=""Red""><b>" & dvMenu.Item(i).Item("AName") & "</b></font>"
                'mobjParentNode.Text = "&nbsp;&nbsp;<font Size=""1"" color=""Red""  bgcolor=""Red"" face=""verdana""><b></b></font>"

                'mobjParentNode.Text = dvMenu.Item(i).Item("AName")
                If dvMenu.Item(i).Item("AName") = HttpContext.Current.Session("PropMNU") Then
                    mobjParentNode.Expanded = True
                Else
                    mobjParentNode.Expanded = False

                End If
                If Not IsDBNull(dvMenu.Item(i).Item("PageURL")) Then
                    mobjParentNode.NavigateUrl = dvMenu.Item(i).Item("PageURL")
                Else
                    mobjParentNode.NavigateUrl = ""
                End If
                'mobjTreeMenu.NodeIndent = 5
                mobjTreeMenu.Nodes.Add(mobjParentNode)
                dvScreen = obj.FilterDataTable(dvMenu.Item(i).Item(0))
                intCountScreen = dvScreen.Count
                mobjTreeMenu.SelectedNodeStyle.VerticalPadding = 1
                mobjTreeMenu.SelectedNodeStyle.HorizontalPadding = 0
                mobjTreeMenu.SelectedNodeStyle.NodeSpacing = 1

                Try
                    For j = 0 To intCountScreen - 1
                        mobjChildNode = New System.Web.UI.WebControls.TreeNode
                        mobjChildNode.SelectAction = TreeNodeSelectAction.None

                        If Not IsDBNull(dvScreen.Item(j).Item("PageURL")) Then
                            strURL = dvScreen.Item(j).Item("PageURL")
                            If strURL.Substring(strURL.Length - 4) = "aspx" Then
                                'Modified by Atul
                                'strURL += "?ScrID=" & dvScreen.Item(j).Item("ObjectID")
                                strURL += "?ScrID=" & dvScreen.Item(j).Item("ObjectID")
                                'Added By Atul
                                strURL += "&HeaderText=" & dvScreen.Item(j).Item("AName")
                            Else
                                'Modified by Atul
                                'strURL += "&ScrID=" & dvScreen.Item(j).Item("ObjectID")
                                strURL += "&ScrID=" & dvScreen.Item(j).Item("ObjectID")
                                'Added By Atul
                                strURL += "&HeaderText=" & dvScreen.Item(j).Item("AName")

                            End If
                            'Modified by Atul
                            ' mobjChildNode.NavigateUrl = strURL
                            If (strURL.StartsWith("../")) Then
                                strURL = strURL.Substring(3, strURL.Length - 3)
                                'mobjChildNode.NavigateUrl = "#?strurl=" & strURL
                                'mobjChildNode.NavigateUrl = "javascript:void(0);"
                                'mobjChildNode.Target = "_self"
                                'mobjChildNode.Value = strURL
                            Else
                                'mobjChildNode.NavigateUrl = "#?strurl=" & strURL
                                'mobjChildNode.NavigateUrl = "javascript:void(0);"
                                'mobjChildNode.Target = "_self"
                                'mobjChildNode.Value = strURL
                            End If
                        End If
                        'mobjChildNode.Text = "<a href='#' onclick='enableTab('" & dvScreen.Item(j).Item("AName") & "','" & strURL & "')'>" & dvScreen.Item(j).Item("AName") & "</a>"

                        If dvScreen.Item(j).Item("AName") = ScreenName Then
                            'mobjChildNode.Text = dvScreen.Item(j).Item("AName")
                            ' mobjChildNode.Text = "<font Size=""1"" face=""verdana"" color=""Blue"" > " & dvScreen.Item(j).Item("AName") & "</font>"
                            'mobjChildNode.Text = "<a href='#' onclick='alert(""hello"")'>" & dvScreen.Item(j).Item("AName") & "</a>"
                            mobjChildNode.Text = "<a href='#' onclick=""enableTab('" & dvScreen.Item(j).Item("AName") & "','" & strURL & "','" & dvScreen.Item(j).Item("ObjectID") & "',-1)"">" & "<font Size=""1"" face=""verdana"" color=""Blue"" font-underline=""false"">" & dvScreen.Item(j).Item("AName") & "</font>" & "</a>"
                            'mobjChildNode.Text = "<font Size=""1""   face=""verdana""><a style=""text-decoration: none;color:blue"" target=""MainPage"" href=""" & strURL & """ onclick=""ChildNode('" & dvScreen.Item(j).Item("AName") & "')"">" & dvScreen.Item(j).Item("AName") & "</a></font>"
                        Else
                            'mobjChildNode.Text = "<a href='#' onclick='alert(""hello"")'>" & dvScreen.Item(j).Item("AName") & "</a>"
                            'mobjChildNode.Text = "<a href='#' onclick='enableTab("" & dvScreen.Item(j).Item(""AName"") & " ','" & strURL & "')'>" & dvScreen.Item(j).Item("AName") & "</a>"
                            mobjChildNode.Text = "<a href='#' onclick=""enableTab('" & dvScreen.Item(j).Item("AName") & "','" & strURL & "','" & dvScreen.Item(j).Item("ObjectID") & "',-1)"">" & "<font Size=""1"" face=""verdana"" color=""Blue"">" & dvScreen.Item(j).Item("AName") & "</font>" & "</a>"

                            'mobjChildNode.Text = "<font Size=""1"" face=""verdana"" color=""Blue"">" & dvScreen.Item(j).Item("AName") & "</font>"
                            'mobjChildNode.Text = dvScreen.Item(j).Item("AName")
                            'mobjChildNode.Text = "<font Size=""1""   bgcolor=white face=""verdana""><a  style=""text-decoration: none;color:blue""   target=""MainPage"" href=""" & strURL & """  onclick=""ChildNode('" & dvScreen.Item(j).Item("AName") & "')"">" & dvScreen.Item(j).Item("AName") & "</a></font>"
                        End If
                        If dvScreen.Item(j).Item("AName") = HttpContext.Current.Session("PropSCR") Then
                            mobjParentNode.Expanded = True
                            'Dim css As New Microsoft.Web.UI.WebControls.CssCollection("color: black; background-color:Lightgreen;")
                            '  mobjChildNode.DefaultStyle = css
                        Else

                        End If
                        mobjChildNode.Target = "dashboard.aspx"



                        'If Not IsDBNull(dvScreen.Item(j).Item("ImageURL")) Then
                        '    mobjChildNode.ImageUrl = dvScreen.Item(j).Item("ImageURL")
                        '    mobjChildNode.SelectedImageUrl = dvScreen.Item(j).Item("ImageURL")
                        'Else
                        '    mobjChildNode.ImageUrl = ""
                        '    mobjChildNode.SelectedImageUrl = ""
                        'End If

                        mobjTreeMenu.Nodes(i).ChildNodes.Add(mobjChildNode)
                    Next
                Catch ex As Exception
                    CreateLog("clsSecurityCache.vb", "FillObjectData-104", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
                End Try
            Next
            MenuPnl.Controls.Add(mobjTreeMenu)
        Catch ex As Exception
            CreateLog("clsSecurityCache.vb", "FillObjectData-110", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Private Function MenuAccess(ByVal intPID As Integer) As Boolean
        Dim dvPC As DataView
        Dim strFilter As String
        'Dim dtTemp As DataTable
        Dim dtAll As DataTable

        dtAll = Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")

        strFilter = "ObjectID = " & intPID
        dvPC = New DataView(dtAll, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        If dvPC.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function ScreenAccess(ByVal intScrID As Integer) As Boolean
        Dim dvPC As DataView
        Dim strFilter As String
        'Dim dtTemp As DataTable
        Dim dtAll As DataTable
        Dim intPID As Int32
        Try
            If IsNothing(HttpContext.Current.Session("PropRole")) Then
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath & "/Login/Login.aspx")
                Exit Function
            End If
            If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
                FillCache()
            End If
            dtAll = Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")

            strFilter = "(ObjType = 'SCR' or ObjType = 'POP') and ObjectID = " & intScrID
            dvPC = New DataView(dtAll, strFilter, "ObjectID", DataViewRowState.CurrentRows)
            If dvPC.Count > 0 Then
                intPID = dvPC.Item(0).Item("ObjectPID")
                If MenuAccess(intPID) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath & "/Login/Login.aspx")
            Exit Function
        End Try
    End Function
    Private Function FilterCacheTable(ByVal objType As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        Dim dtTemp As DataTable

        strFilter = "ObjType = '" & objType & "'"

        dvPC = New DataView(dtData, strFilter, "ObjType", DataViewRowState.CurrentRows)
        Return dvPC
    End Function
    Private Function FilterDataTable(ByVal objPID As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        strFilter = "ObjectPID = " & objPID & " and ObjType = 'SCR'"
        dvPC = New DataView(dtData, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        Return dvPC
    End Function

    Private Function FilterControlTable(ByVal objPID As String) As DataView
        Dim dvPC As DataView
        Dim strFilter, strCount As String
        strFilter = "ObjectPID = " & objPID & " and ObjType <> 'MNU' and ObjType <> 'SCR' and ObjType <> 'POP'"
        dvPC = New DataView(dtData, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        strCount = dvPC.Count
        Return dvPC
    End Function
    Private Function FilterGridControlGrid(ByVal objPID As String, ByVal gdName As String) As DataView
        Dim dvPC As DataView
        Dim dt As DataTable
        If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
            FillCache()
        End If
        dt = Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")
        Dim strFilter, strCount As String
        strFilter = "ObjectPID = " & objPID & " and ObjType = 'GRD'"
        dvPC = New DataView(dt, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        strCount = dvPC.Count
        Return dvPC
    End Function

    Private Function FilterGridControl(ByVal objPID As String, ByVal gdName As String) As DataView
        Dim dvPC As DataView
        Dim dt As DataTable
        If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
            FillCache()
        End If
        dt = Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")
        Dim strFilter, strCount As String
        strFilter = "ObjectPID = " & objPID & " and (ObjType = 'GRD' or ObjType = 'COL') and GName ='" & gdName & "'"
        dvPC = New DataView(dt, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        strCount = dvPC.Count
        Return dvPC
    End Function

    Private Sub setControlAttribute(ByVal ctl As Control, ByVal dvCtl As DataView)
        If isExisted(dvCtl, ctl.ID()) = True Then
            If strView = "H" Then
                ctl.Visible = False
            End If
            If strEnable = "D" Then
                If TypeOf ctl Is Button Then
                    Dim ctl2 As Button
                    ctl2 = CType(ctl, Button)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is Web.UI.WebControls.Panel Then '// RVS 27/02/08
                    Dim ctl2 As Web.UI.WebControls.Panel
                    ctl2 = CType(ctl, Web.UI.WebControls.Panel)
                    ctl2.Enabled = False
                    For Each ctl3 As Control In ctl.Controls
                        If TypeOf ctl3 Is TextBox Then
                            CType(ctl3, TextBox).Enabled = False

                            'ElseIf TypeOf ctl3 Is DateSelector Then
                            '    CType(ctl3, DateSelector).readOnlyDate = False
                        End If
                    Next
                ElseIf TypeOf ctl Is TextBox Then
                    Dim ctl2 As TextBox
                    ctl2 = CType(ctl, TextBox)
                    ctl2.ReadOnly = True
                ElseIf TypeOf ctl Is Label Then
                    Dim ctl2 As Label
                    ctl2 = CType(ctl, Label)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is DropDownList Then
                    Dim ctl2 As DropDownList
                    ctl2 = CType(ctl, DropDownList)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is ListBox Then
                    Dim ctl2 As ListBox
                    ctl2 = CType(ctl, ListBox)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is ImageButton Then
                    Dim ctl2 As ImageButton
                    Dim strURL, strMod As String
                    ctl2 = CType(ctl, ImageButton)
                    ctl2.Enabled = False
                    'If ctl2.ID = "imgSave" Then
                    strURL = ctl2.ImageUrl
                    strMod = strURL.Substring(0, strURL.Length - 4)
                    strMod = strMod & "1" & strURL.Substring(strURL.Length - 4)
                    Dim objFile As System.IO.File
                    If objFile.Exists(HttpContext.Current.Server.MapPath(strMod)) = True Then
                        ctl2.ImageUrl = strMod
                    Else
                        ctl2.ImageUrl = strURL
                    End If
                    objFile = Nothing

                    'End If
                ElseIf TypeOf ctl Is RadioButton Then
                    Dim ctl2 As RadioButton
                    ctl2 = CType(ctl, RadioButton)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is CheckBox Then
                    Dim ctl2 As CheckBox
                    ctl2 = CType(ctl, CheckBox)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is HyperLink Then
                    Dim ctl2 As HyperLink
                    ctl2 = CType(ctl, HyperLink)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is DataGrid Then
                    Dim ctl2 As DataGrid
                    ctl2 = CType(ctl, DataGrid)
                    'ctl2.Enabled = False
                    For i As Int16 = 0 To ctl2.Items.Count - 1
                        ctl2.Enabled = False
                        ctl2.Items(i).Enabled = False
                        ctl2.Items.Item(i).Enabled = False
                    Next
                End If
            End If
            If TypeOf ctl Is Label Then
                Dim ctl2 As Label
                ctl2 = CType(ctl, Label)
                ctl2.Text = strlblAlias
            End If
        End If
    End Sub

    Public Function GridManipulationWithoutCtls(ByVal ctl As Control, ByVal dvCtl As DataView, ByVal ctlMain As Control)
        Dim dgi As DataGridItem
        Dim dg As DataGrid
        Dim dvGrid As DataView
        Dim str As String
        Dim j As Int16 = 0
        Dim str2 As String
        dvCtl.Sort = "AName"
        dg = CType(ctl, DataGrid)
        For j = 0 To dg.Columns.Count - 1
            If isExistedGCol(dvCtl, dg.Columns(j).HeaderText) Then
                If strView = "H" Then
                    Dim ctlSText As Control = ctlMain.FindControl(dg.Columns(j).HeaderText)
                    If Not IsNothing(ctlSText) Then
                        ctlSText.Visible = False
                        dg.Columns(j).Visible = False
                    End If
                End If
            End If

        Next
    End Function

    Public Function GridManipulation(ByVal ctl As Control, ByVal dvCtl As DataView)
        Dim dgi As DataGridItem
        Dim dg As DataGrid
        Dim dvGrid As DataView
        Dim str As String
        Dim j As Int16 = 0
        Dim str2 As String
        dg = CType(ctl, DataGrid)
        For Each dgi In dg.Items
            For j = 0 To dgi.Controls.Count - 1
                For Each ctl1 As Control In dgi.Cells(j).Controls
                    If isExisted(dvCtl, ctl1.ID()) = True Then
                        If strView = "H" Then
                            ctl1.Visible = False
                            dg.Columns(j).Visible = False
                        End If
                        If strEnable = "D" Then
                            If TypeOf ctl1 Is Button Then
                                Dim ctl2 As Button
                                ctl2 = CType(ctl1, Button)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is TextBox Then
                                Dim ctl2 As TextBox
                                ctl2 = CType(ctl1, TextBox)
                                ctl2.ReadOnly = True
                            ElseIf TypeOf ctl1 Is Label Then
                                Dim ctl2 As Label
                                ctl2 = CType(ctl1, Label)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is DropDownList Then
                                Dim ctl2 As DropDownList
                                ctl2 = CType(ctl1, DropDownList)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is ListBox Then
                                Dim ctl2 As ListBox
                                ctl2 = CType(ctl1, ListBox)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is ImageButton Then
                                Dim ctl2 As ImageButton
                                ctl2 = CType(ctl1, ImageButton)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is RadioButton Then
                                Dim ctl2 As RadioButton
                                ctl2 = CType(ctl1, RadioButton)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is CheckBox Then
                                Dim ctl2 As CheckBox
                                ctl2 = CType(ctl1, CheckBox)
                                ctl2.Enabled = False
                            ElseIf TypeOf ctl1 Is HyperLink Then
                                Dim ctl2 As HyperLink
                                ctl2 = CType(ctl1, HyperLink)
                                ctl2.Enabled = False
                            End If
                        End If
                    End If
                Next
            Next
        Next
    End Function
    Public Function GridControlSecurity(ByVal frm As Control, ByVal intScrID As Integer) As Boolean
        Dim dvCtl As DataView
        For Each ctl As Control In frm.Controls
            For Each ctl1 As Control In ctl.Controls
                If (TypeOf (ctl1) Is CustomControls.Web.CollapsiblePanel) Then

                    For Each ctl2 As Control In ctl1.Controls
                        If TypeOf (ctl2) Is System.Web.UI.WebControls.DataGrid Then
                            dvCtl = FilterGridControlGrid(intScrID, ctl2.ID)
                            dvCtl.Sort = "Name"
                            GridManipulation(ctl2, dvCtl)
                        End If
                    Next
                Else
                    If TypeOf (ctl1) Is System.Web.UI.WebControls.DataGrid Then
                        dvCtl = FilterGridControlGrid(intScrID, ctl1.ID)
                        dvCtl.Sort = "Name"
                        GridManipulation(ctl1, dvCtl)
                    End If
                End If
            Next
        Next
    End Function
    Public Function ControlSecurity(ByVal frm As Control, ByVal intScrID As Integer) As Boolean
        Dim dtScrTable As New DataTable
        Dim dt As DataTable
        Dim colPnl As CustomControls.Web.CollapsiblePanel
        If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
            FillCache()
        End If
        dt = Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")
        dtData = dt
        Dim dvCtl As DataView

        dvCtl = FilterControlTable(intScrID)
        dvCtl.Sort = "Name"

        For Each ctl As Control In frm.Controls
            For Each ctl1 As Control In ctl.Controls
                If (TypeOf (ctl1) Is CustomControls.Web.CollapsiblePanel) Then
                    setControlAttribute(ctl1, dvCtl)
                    colPnl = CType(ctl1, CustomControls.Web.CollapsiblePanel)
                    If colPnl.Visible = True Then
                        For Each ctl2 As Control In ctl1.Controls
                            If TypeOf (ctl2) Is System.Web.UI.WebControls.DataGrid Then
                                dvCtl = FilterGridControl(intScrID, ctl2.ID)
                                dvCtl.Sort = "Name"
                                GridManipulationWithoutCtls(ctl2, dvCtl, ctl1)
                                dvCtl = FilterControlTable(intScrID)
                                dvCtl.Sort = "Name"
                            End If
                            If (TypeOf (ctl2) Is System.Web.UI.WebControls.Panel) Or (TypeOf (ctl2) Is System.Web.UI.WebControls.DataGrid) Or (TypeOf (ctl2) Is System.Web.UI.WebControls.Image) Or (TypeOf (ctl2) Is TextBox) Or (TypeOf (ctl2) Is Button) Or (TypeOf (ctl2) Is Label) Or (TypeOf (ctl2) Is DropDownList) Or (TypeOf (ctl2) Is ListBox) Or (TypeOf (ctl2) Is ImageButton) Or (TypeOf (ctl2) Is RadioButton) Or (TypeOf (ctl2) Is CheckBox) Or (TypeOf (ctl2) Is HyperLink) Then
                                If TypeOf (ctl2) Is System.Web.UI.WebControls.Panel Then
                                    'Dim a As String
                                    'a = ctl2.ID
                                End If
                                setControlAttribute(ctl2, dvCtl)
                            End If
                        Next
                    End If
                Else
                    If (TypeOf (ctl1) Is System.Web.UI.WebControls.Panel) Or (TypeOf (ctl1) Is System.Web.UI.WebControls.DataGrid) Or (TypeOf (ctl1) Is System.Web.UI.WebControls.Image) Or (TypeOf (ctl1) Is TextBox) Or (TypeOf (ctl1) Is Button) Or (TypeOf (ctl1) Is Label) Or (TypeOf (ctl1) Is DropDownList) Or (TypeOf (ctl1) Is ListBox) Or (TypeOf (ctl1) Is ImageButton) Or (TypeOf (ctl1) Is RadioButton) Or (TypeOf (ctl1) Is CheckBox) Or (TypeOf (ctl1) Is HyperLink) Then
                        setControlAttribute(ctl1, dvCtl)
                    End If
                    If TypeOf (ctl1) Is System.Web.UI.WebControls.DataGrid Then
                        dvCtl = FilterGridControl(intScrID, ctl1.ID)
                        dvCtl.Sort = "Name"
                        GridManipulationWithoutCtls(ctl1, dvCtl, ctl)
                        dvCtl = FilterControlTable(intScrID)
                        dvCtl.Sort = "Name"
                    End If
                End If
            Next
        Next
    End Function
    Private Function isExistedGCol(ByRef dvCtl As DataView, ByVal strCtlAName As String) As Boolean
        Dim intIndex As Int16
        intIndex = dvCtl.Find(strCtlAName)
        If intIndex <> -1 Then
            strView = dvCtl.Item(intIndex).Item("VH")
            strEnable = dvCtl.Item(intIndex).Item("ED")
            Return True
        Else
            Return False
        End If
    End Function


    Private Function isExisted(ByRef dvCtl As DataView, ByVal strCtlName As String) As Boolean
        Dim intIndex As Int16
        intIndex = dvCtl.Find(strCtlName)
        If intIndex <> -1 Then
            strView = dvCtl.Item(intIndex).Item("VH")
            strEnable = dvCtl.Item(intIndex).Item("ED")
            strlblAlias = dvCtl.Item(intIndex).Item("AName")
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub FillCache()
        Dim DA As SqlDataAdapter
        Dim strQuery, strConn As String
        Dim objCommand As SqlCommand
        Dim dtData As New DataTable
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString


        strQuery = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name," _
                            & " ROD_VC50_Alias_Name as AName," _
                            & " OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL, " _
                            & " OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType, " _
                            & " ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH, " _
                            & " OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName " _
                            & " from " _
                            & " T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                            & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                            & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND " _
                            & " RA.RA_VC4_Status_Code = 'ENB' AND " _
                            & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                            & " ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                            & " (((OBM.OBM_VC4_Object_Type_FK <>'MNU' and OBM.OBM_VC4_Object_Type_FK <>'SCR' and OBM.OBM_VC4_Object_Type_FK <>'VIW' and OBM.OBM_VC4_Object_Type_FK <>'POP') and (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D')) OR " _
                            & " ((OBM.OBM_VC4_Object_Type_FK ='MNU' or OBM.OBM_VC4_Object_Type_FK ='SCR' or OBM.OBM_VC4_Object_Type_FK ='VIW' or OBM.OBM_VC4_Object_Type_FK ='LBL' or OBM.OBM_VC4_Object_Type_FK ='POP' or OBM.OBM_VC4_Object_Type_FK ='COL') and (ROD.ROD_CH1_View_Hide <> 'H'))) and" _
                            & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                            & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND " _
                            & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and " _
                            & " ROM_IN4_Role_ID_PK = " & HttpContext.Current.Session("PropRole") _
                            & " order by OBM.OBM_SI2_Order_By"


        'strQuery = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name," _
        '                    & " ROD_VC50_Alias_Name as AName," _
        '                    & " OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL, " _
        '                    & " OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType, " _
        '                    & " ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH, " _
        '                    & " OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName " _
        '                    & " from " _
        '                    & " T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
        '                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
        '                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND " _
        '                    & " RA.RA_VC4_Status_Code = 'ENB' AND " _
        '                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
        '                    & " ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
        '                    & " (((OBM.OBM_VC4_Object_Type_FK <>'MNU' and OBM.OBM_VC4_Object_Type_FK <>'SCR' and OBM.OBM_VC4_Object_Type_FK <>'VIW' and OBM.OBM_VC4_Object_Type_FK <>'POP') and (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D')) OR " _
        '                    & " ((OBM.OBM_VC4_Object_Type_FK ='MNU' or OBM.OBM_VC4_Object_Type_FK ='SCR' or OBM.OBM_VC4_Object_Type_FK ='VIW' or OBM.OBM_VC4_Object_Type_FK ='LBL' or OBM.OBM_VC4_Object_Type_FK ='POP' or OBM.OBM_VC4_Object_Type_FK ='COL') and (ROD.ROD_CH1_View_Hide <> 'H'))) and" _
        '                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
        '                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND " _
        '                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and " _
        '                    & " ROM_IN4_Company_ID_FK = " & HttpContext.Current.Session("PropCompanyID") & " and " _
        '                    & " ROM_IN4_Role_ID_PK = " & HttpContext.Current.Session("PropRole") _
        '                    & " order by OBM.OBM_SI2_Order_By"





        'strQuery = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name," _
        '                           & " ROD_VC50_Alias_Name as AName," _
        '                           & " OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL, " _
        '                           & " OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType, " _
        '                           & " ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH, " _
        '                           & " OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName " _
        '                           & " from " _
        '                           & " T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
        '                           & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
        '                           & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND " _
        '                           & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
        '                           & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
        '                           & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
        '                           & " (((OBM.OBM_VC4_Object_Type_FK <>'MNU' and OBM.OBM_VC4_Object_Type_FK <>'SCR' and OBM.OBM_VC4_Object_Type_FK <>'VIW' and OBM.OBM_VC4_Object_Type_FK <>'POP') and (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D')) OR " _
        '                           & " ((OBM.OBM_VC4_Object_Type_FK ='MNU' or OBM.OBM_VC4_Object_Type_FK ='SCR' or OBM.OBM_VC4_Object_Type_FK ='VIW' or OBM.OBM_VC4_Object_Type_FK ='LBL' or OBM.OBM_VC4_Object_Type_FK ='POP' or OBM.OBM_VC4_Object_Type_FK ='COL') and (ROD.ROD_CH1_View_Hide <> 'H'))) and" _
        '                           & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
        '                           & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND " _
        '                           & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and " _
        '                           & " ROM_IN4_Company_ID_FK = " & HttpContext.Current.Session("PropCompanyID") & " and " _
        '                           & " ROM_IN4_Role_ID_PK = " & HttpContext.Current.Session("PropRole") _
        '                           & " order by OBM.OBM_SI2_Order_By"


        Dim objCon As SqlConnection = New SqlConnection(strConn)

        Try
            objCon.Open()
            objCommand = New SqlCommand
            With objCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(dtData)
            If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "Security")) Then
                Current.Cache.Insert(HttpContext.Current.Session("PropUserID") & "Security", dtData, Nothing, Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20))
            Else
                Current.Cache.Remove(HttpContext.Current.Session("PropUserID") & "Security")
                Current.Cache.Insert(HttpContext.Current.Session("PropUserID") & "Security", dtData, Nothing, Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20))
            End If
            'Dim ds As New DataSet
            'ds.Tables.Add(dtData)
            'ds.WriteXml("d:\2005\SecurityXML.xml")
        Catch ex As Exception
            'Dim str As String
            'str = ex.Message
            'Throw
            CreateLog("clsSecurityCache.vb", "FillCache-494", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        Finally
            objCon.Close()
        End Try

    End Sub
    Private Sub GetDataControls(ByVal ObjectType As String, ByRef dtObject As DataTable)
        Dim DA As SqlDataAdapter
        Dim strQuery, strConn As String
        Dim objCommand As SqlCommand
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        strQuery = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name," _
                    & " OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL, " _
                    & " OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType, " _
                    & " ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH from " _
                    & " T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND " _
                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                    & " (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D') AND " _
                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND " _
                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK AND " _
                    & " OBM.OBM_VC4_Object_Type_FK <>'MNU' and OBM.OBM_VC4_Object_Type_FK <>'SCR' and OBM.OBM_VC4_Object_Type_FK <>'VIW'" _
                    & " order by OBM.OBM_SI2_Order_By"
        Dim objCon As SqlConnection = New SqlConnection(strConn)

        Try
            objCon.Open()
            objCommand = New SqlCommand
            With objCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)
            DA.Fill(dtObject)

        Catch ex As Exception
            'Dim str As String
            'str = ex.Message
            'Throw
            CreateLog("clsSecurityCache.vb", "GetDataControls-538", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        Finally
            objCon.Close()
        End Try

    End Sub
    Private Sub GetData(ByVal ObjectType As String, ByVal TableName As String, ByRef dsObject As DataSet)
        Dim DA As SqlDataAdapter
        Dim strQuery, strConn As String
        Dim objCommand As SqlCommand
        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        strQuery = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID, OBM.OBM_VC50_Alias_Name as AName," _
                    & " OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL, " _
                    & " OBM_VC200_Image as ImageURL from " _
                    & " T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND " _
                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                    & " ROD.ROD_CH1_View_Hide <> 'H' AND ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND " _
                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK AND " _
                    & " OBM.OBM_VC4_Object_Type_FK ='" & ObjectType _
                    & "' order by OBM.OBM_SI2_Order_By"
        Dim objCon As SqlConnection = New SqlConnection(strConn)

        Try
            objCon.Open()
            objCommand = New SqlCommand
            With objCommand
                .CommandText = strQuery
                .CommandType = CommandType.Text
                .Connection = objCon
            End With
            DA = New SqlDataAdapter(objCommand)

            DA.Fill(dsObject, TableName)

        Catch ex As Exception
            'Dim str As String
            'str = ex.Message
            'Throw
            CreateLog("clsSecurityCache.vb", "GetData-104", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        Finally
            objCon.Close()
        End Try

    End Sub

End Class
