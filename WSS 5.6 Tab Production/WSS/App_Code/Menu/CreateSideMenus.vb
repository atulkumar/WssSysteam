Imports ION.Logging.EventLogging
Imports ION.Data
Imports System.Data.SqlClient
Imports Microsoft.Web.UI.WebControls

Public Class CreateSideMenus

	Private Shared mobjTreeMenu As TreeView	  ' This will be called by the page where we will create the side menu
	Private Shared mobjParentNode As TreeNode	 ' This will be the Parent node in the Menu
	Private Shared mobjChildNode As TreeNode	  ' Child node under parent node

	'*******************************************************************
	' Function             :-  CreateMenuFromDB
	' Purpose              :- It will create a menu sttructure on the TreeMenu and will then 
	'								add it to the panel
	' Date					Author						Modification Date					Description
	' 18/01/06			Nitin Jain					-------------------					Created
	'
	' Notes: 
	' Code:
	'*******************************************************************
	Public Shared Function CreateMenuFromDB(ByVal MenuPanel As Panel) As Boolean
		Try
			Dim blnBool As Boolean
			Dim sqReader As SqlDataReader
			Dim intMenu As Integer

			' Provide the connection string from the web config file
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
			' Provide the table name
            'SQL.DBTable = "tbl_mainnode_acc"
			SQL.DBTracing = False

			' Get those Menu Heads for which permission (Right2 is V) is allowed.
            sqReader = SQL.Search("createsidemenu", "CreateMenuFromDB-35", "SELECT tbl_mainnode_acc.ObjID, MENUHEAD.HEAD_DESC,MENUHEAD.HEAD_URL,MENUHEAD.HEAD_IMG FROM tbl_mainnode_acc INNER JOIN MENUHEAD ON tbl_mainnode_acc.ObjID = MENUHEAD.HEAD_ID WHERe (tbl_mainnode_acc.comp = 0) AND (tbl_mainnode_acc.Right2 = 'V')", SQL.CommandBehaviour.CloseConnection, blnBool)

			'Tree Menu
			mobjTreeMenu = New TreeView
			' Parent Node
			mobjParentNode = New TreeNode
			'Child Node
			mobjTreeMenu = New TreeView


			'hide plus sign in side menu
			'******************
			mobjTreeMenu.ShowLines = False
			mobjTreeMenu.ShowPlus = False
			'***************************
			mobjTreeMenu.SelectExpands = True
			'Set Font and other options on Tree Menu

			mobjTreeMenu.Font.Name = "Verdana"
            'mobjTreeMenu.ForeColor = Color.LightGray
			mobjTreeMenu.ExpandedImageUrl = "../images/sort_down.gif"
			mobjTreeMenu.ExpandLevel = 5

			' If  there are menu heads with permission granted to view
			While sqReader.Read
				mobjParentNode = New TreeNode
				mobjParentNode.Expanded = False

                ' Set the parent node text bgcolor=""lightgray""
                mobjParentNode.Text = "<font Size=""1""   face=""verdana""><b>" & sqReader.Item("HEAD_DESC") & "</b></font>"

				If sqReader.Item("HEAD_DESC") = "" Or sqReader.Item("HEAD_DESC") = "#" Then
				Else
					mobjParentNode.NavigateUrl = sqReader.Item("HEAD_URL")
					mobjParentNode.Expandable = ExpandableValue.Auto
				End If

				mobjParentNode.ImageUrl = sqReader.Item("HEAD_IMG")

				'Add the Node to the tree menu
				mobjTreeMenu.Nodes.Add(mobjParentNode)

				' Get all the child heads for that Menu head
                Dim sqrdChild As Data.SqlClient.SqlDataReader
                sqrdChild = SQL.Search("createsidemenu", "CreateMenuFromDB-79", "SELECT * FROM MENUCHILD WHERE HEAD_ID=" & sqReader.Item("ObjID"), SQL.CommandBehaviour.CloseConnection, blnBool)

                ' If child heads are found for that Menu head bgcolor=lightgray
                If blnBool = True Then
                    While sqrdChild.Read
                        mobjChildNode = New TreeNode
                        mobjChildNode.Text = "<font Size=""1""   face=""verdana"">" & sqrdChild.Item("CHILD_DESC") & "</font>"
                        mobjChildNode.NavigateUrl = sqrdChild.Item("CHILD_URL")

                        mobjChildNode.Target = "MainPage"
                        mobjChildNode.ExpandedImageUrl = "../images/sort_down.gif"
                        mobjChildNode.ImageUrl = sqrdChild.Item("CHILD_IMG")
                        mobjChildNode.SelectedImageUrl = sqrdChild.Item("CHILD_IMG")
                        mobjTreeMenu.Nodes(intMenu).Nodes.Add(mobjChildNode)
                    End While

                    sqrdChild.Close()
                End If
                intMenu += 1
            End While

			' Add the treemenu to the panel
			MenuPanel.Controls.Add(mobjTreeMenu)
			sqReader.Close()
		Catch ex As Exception
			CreateLog("CreateSideMenus", "CreateMenuFromDB-104", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
		End Try
	End Function

End Class