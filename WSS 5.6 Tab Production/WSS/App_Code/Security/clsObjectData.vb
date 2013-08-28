Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls
Imports System.Web.Caching.Cache
Imports System.Web.HttpContext
Imports ION.Data
Imports System.Data

Public Class clsObjectData
    Private dtData As DataTable
    Private Shared mobjParentNode As TreeNode  ' This will be the Parent node in the Menu
    Private Shared mobjChildNode As TreeNode   ' Child node under parent node
    Private Shared mobjContolNode As TreeNode

    Public Shared Function FillObjectData(ByVal mobjTreeMenu As TreeView)

        Dim dsObject As New DataSet
        Dim obj As New clsObjectData
        Dim dt As DataTable
        mobjTreeMenu.Nodes.Clear()
        If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "ObjectData")) Then
            obj.FillCache()
        End If
        dt = Current.Cache(HttpContext.Current.Session("PropUserID") & "ObjectData")
        '        obj.dtData = dt
        Dim intCountMenu, intCountScreen, intCountControl, i, j, k As Integer
        Dim dvMenu As DataView
        Dim dvScreen As DataView
        Dim dvControl As DataView
        Dim strURL As String
        Dim str As String
        dvMenu = obj.FilterCacheTable("MNU")
        'dvScreen = obj.FilterCacheTable("SCR")

        intCountMenu = dvMenu.Count
        'mobjTreeMenu = New TreeView
        'hide plus sign in side menu
        '******************
        mobjTreeMenu.ShowLines = True
        mobjTreeMenu.ShowPlus = True
        '***************************
        mobjTreeMenu.SelectExpands = True
        'Set Font and other options on Tree Menu
        mobjTreeMenu.AutoSelect = True



        mobjTreeMenu.Font.Name = "Verdana"
        mobjTreeMenu.ForeColor = System.Drawing.Color.LightGray
        mobjTreeMenu.ExpandedImageUrl = "../images/sort_down.gif"
        mobjTreeMenu.ExpandLevel = 5
        Try
            For i = 0 To intCountMenu - 1
                mobjParentNode = New TreeNode
                mobjParentNode.Expanded = False

                ' Set the parent node text
                mobjParentNode.Text = "<font Size=""1""  bgcolor=""lightgray"" face=""verdana""><b>" & dvMenu.Item(i).Item("AName") & "</b></font>"
                'If Not IsDBNull(dvMenu.Item(i).Item("PageURL")) Then
                '    mobjParentNode.NavigateUrl = dvMenu.Item(i).Item("PageURL")
                'Else
                '    mobjParentNode.NavigateUrl = ""
                'End If
                mobjParentNode.ID = dvMenu.Item(i).Item("ObjectID")
                mobjParentNode.Expandable = ExpandableValue.Auto
                mobjTreeMenu.AutoSelect = True
                If Not IsDBNull(dvMenu.Item(i).Item("ImageURL")) Then
                    mobjParentNode.ImageUrl = dvMenu.Item(i).Item("ImageURL")
                Else
                    mobjParentNode.ImageUrl = ""
                End If

                'Add the Node to the tree menu
                mobjTreeMenu.Nodes.Add(mobjParentNode)

                dvScreen = obj.FilterDataTable(dvMenu.Item(i).Item(0))
                intCountScreen = dvScreen.Count
                Try
                    For j = 0 To intCountScreen - 1
                        mobjChildNode = New TreeNode
                        mobjChildNode.Expanded = False
                        mobjChildNode.Expandable = ExpandableValue.Auto
                        mobjChildNode.Text = "<font Size=""1""   bgcolor=lightgray face=""verdana"">" & dvScreen.Item(j).Item("AName") & "</font>"
                        'If Not IsDBNull(dvScreen.Item(j).Item("PageURL")) Then
                        '    strURL = dvScreen.Item(j).Item("PageURL")
                        '    If strURL.Substring(strURL.Length - 4) = "aspx" Then
                        '        strURL += "?ScrID=" & dvScreen.Item(j).Item("ObjectID")
                        '    Else
                        '        strURL += "&ScrID=" & dvScreen.Item(j).Item("ObjectID")
                        '    End If
                        '    mobjChildNode.NavigateUrl = strURL
                        'End If
                        ' mobjChildNode.Target = "MainPage"
                        mobjChildNode.ID = dvScreen.Item(j).Item("ObjectID")
                        mobjChildNode.ExpandedImageUrl = "../images/sort_down.gif"
                        If Not IsDBNull(dvScreen.Item(j).Item("ImageURL")) Then
                            mobjChildNode.ImageUrl = dvScreen.Item(j).Item("ImageURL")
                            mobjChildNode.SelectedImageUrl = dvScreen.Item(j).Item("ImageURL")
                        Else
                            mobjChildNode.ImageUrl = ""
                            mobjChildNode.SelectedImageUrl = ""
                        End If

                        mobjTreeMenu.Nodes(i).Nodes.Add(mobjChildNode)


                        dvControl = obj.FilterDataTable(dvScreen.Item(j).Item(0))
                        intCountControl = dvControl.Count

                        For k = 0 To intCountControl - 1
                            mobjContolNode = New TreeNode
                            mobjContolNode.Expanded = False
                            mobjContolNode.Expandable = ExpandableValue.Auto
                            mobjContolNode.Text = "<font Size=""1""   bgcolor=lightgray face=""verdana"">" & dvControl.Item(k).Item("AName") & "</font>"
                            'mobjContolNode.Target = "MainPage"
                            'mobjContolNode.ExpandedImageUrl = "../images/sort_down.gif"
                            mobjContolNode.ID = dvControl.Item(k).Item("ObjectID")
                            If Not IsDBNull(dvControl.Item(k).Item("ImageURL")) Then
                                mobjContolNode.ImageUrl = dvControl.Item(k).Item("ImageURL")
                                mobjContolNode.SelectedImageUrl = dvControl.Item(k).Item("ImageURL")
                            Else
                                mobjContolNode.ImageUrl = ""
                                mobjContolNode.SelectedImageUrl = ""
                            End If

                            mobjTreeMenu.Nodes(i).Nodes(j).Nodes.Add(mobjContolNode)
                        Next

                    Next
                Catch ex As Exception
                    Dim str1 As String
                    str1 = ex.Message
                End Try
            Next
            'MenuPnl.Controls.Add(mobjTreeMenu)
        Catch ex As Exception
            Dim str1 As String
            str1 = ex.Message
        End Try
    End Function
    Private Function Filter(ByVal objPID As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        Dim dtTemp As DataTable
        dtTemp = Current.Cache(HttpContext.Current.Session("PropUserID") & "ObjectData")
        strFilter = "ObjectPID = " & objPID
        dvPC = New DataView(dtTemp, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        dvPC.Sort = "OrderBy"
        Return dvPC
    End Function

    Private Function FilterCacheTable(ByVal objType As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        Dim dtTemp As DataTable
        dtTemp = Current.Cache(HttpContext.Current.Session("PropUserID") & "ObjectData")
        strFilter = "ObjType = '" & objType & "'"

        dvPC = New DataView(dtTemp, strFilter, "ObjType", DataViewRowState.CurrentRows)
        dvPC.Sort = "OrderBy"
        Return dvPC
    End Function
    Private Function FilterDataTable(ByVal objPID As String) As DataView
        Dim dvPC As DataView
        Dim strFilter As String
        Dim dtTemp As DataTable
        dtTemp = Current.Cache(HttpContext.Current.Session("PropUserID") & "ObjectData")
        strFilter = "ObjectPID = " & objPID
        dvPC = New DataView(dtTemp, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        dvPC.Sort = "OrderBy"
        Return dvPC
    End Function

    Public Sub FillCache()
        Dim DA As SqlDataAdapter
        Dim strQuery, strConn As String
        Dim objCommand As SqlCommand
        Dim dtData As New DataTable
        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString



        strQuery = "Select OBM_IN4_Object_ID_PK as ObjectID,OBM_VC4_Object_Type_FK as ObjType,OBM_VC50_Object_Name as Name," _
                      & " OBM_VC50_Alias_Name as AName,OBM_VC50_Grid_Name as GName,OBM_CH1_Mandatory as Mandatory," _
                      & " OBM_IN4_Object_PID_FK as ObjectPID,OBM_VC200_Descr as Descr,OBM_VC4_Status_Code_FK as StatusCode," _
                      & "OBM_DT8_Status_Date as StatusDate,OBM_VC200_URL as PageURL,OBM_VC200_Image as ImageURL,OBM_VC8_FPath as FPath,OBM_SI2_Order_By as OrderBy " _
                      & " from T070011 where " _
                      & " OBM_VC4_Status_Code_FK = 'ENB' " _
                      & " order by OBM_SI2_Order_By"

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
            If IsNothing(Current.Cache(HttpContext.Current.Session("PropUserID") & "ObjectData")) Then
                Current.Cache.Insert(HttpContext.Current.Session("PropUserID") & "ObjectData", dtData, Nothing, Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20))
            Else
                Current.Cache.Remove(HttpContext.Current.Session("PropUserID") & "ObjectData")
                Current.Cache.Insert(HttpContext.Current.Session("PropUserID") & "ObjectData", dtData, Nothing, Current.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20))
            End If
        Catch ex As Exception
            Dim str As String
            str = ex.Message
            'Throw
        Finally
            objCon.Close()
        End Try

    End Sub

End Class
