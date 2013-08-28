Imports System.Data.SqlClient
Imports ION.Data
Imports Microsoft.Web.UI.WebControls
Imports System.Web.Caching


Public Class clsSecurity
    Public Shared mobjTreeMenu As TreeView   ' This will be called by the page where we will create the side menu
    Private Shared mobjParentNode As TreeNode  ' This will be the Parent node in the Menu
    Private Shared mobjChildNode As TreeNode   ' Child node under parent node
    Private strEnable, strView As String

    Public Shared Function FillObjectData(ByVal MenuPnl As Panel)

        Dim dsObject As New DataSet
        Dim obj As New clsSecurity
        Dim intCountMenu, intCountScreen, i, j As Integer
        Dim dvScreen As DataView
        Dim strURL As String
        obj.GetData("MNU", "dtMenu", dsObject)
        obj.GetData("SCR", "dtScreen", dsObject)
        intCountMenu = dsObject.Tables("dtMenu").Rows.Count()
        mobjTreeMenu = New TreeView
        ' Parent Node
        mobjParentNode = New TreeNode


        'hide plus sign in side menu
        '******************
        mobjTreeMenu.ShowLines = False
        mobjTreeMenu.ShowPlus = False
        '***************************
        mobjTreeMenu.SelectExpands = True
        'Set Font and other options on Tree Menu

        mobjTreeMenu.Font.Name = "Verdana"
        mobjTreeMenu.ForeColor = Color.LightGray
        mobjTreeMenu.ExpandedImageUrl = "../images/sort_down.gif"
        mobjTreeMenu.ExpandLevel = 5
        Try
            For i = 0 To intCountMenu - 1
                mobjParentNode = New TreeNode
                mobjParentNode.Expanded = False

                ' Set the parent node text
                mobjParentNode.Text = "<font Size=""1""  bgcolor=""lightgray"" face=""verdana""><b>" & dsObject.Tables("dtMenu").Rows(i).Item("AName") & "</b></font>"
                If Not IsDBNull(dsObject.Tables("dtMenu").Rows(i).Item("PageURL")) Then
                    mobjParentNode.NavigateUrl = dsObject.Tables("dtMenu").Rows(i).Item("PageURL")
                Else
                    mobjParentNode.NavigateUrl = ""
                End If

                mobjParentNode.Expandable = ExpandableValue.Auto

                If Not IsDBNull(dsObject.Tables("dtMenu").Rows(i).Item("ImageURL")) Then
                    mobjParentNode.ImageUrl = dsObject.Tables("dtMenu").Rows(i).Item("ImageURL")
                Else
                    mobjParentNode.ImageUrl = ""
                End If

                'Add the Node to the tree menu
                mobjTreeMenu.Nodes.Add(mobjParentNode)

                dvScreen = obj.FilterDataTable(dsObject.Tables("dtMenu").Rows(i).Item(0), dsObject.Tables("dtScreen"))
                intCountScreen = dvScreen.Count
                Try
                    For j = 0 To intCountScreen - 1
                        mobjChildNode = New TreeNode

                        mobjChildNode.Text = "<font Size=""1""   bgcolor=lightgray face=""verdana"">" & dvScreen.Item(j).Item("AName") & "</font>"
                        If Not IsDBNull(dvScreen.Item(j).Item("PageURL")) Then
                            strURL = dvScreen.Item(j).Item("PageURL")
                            If strURL.Substring(strURL.Length - 4) = "aspx" Then
                                strURL += "?ScrID=" & dvScreen.Item(j).Item("ObjectID")
                            Else
                                strURL += "&ScrID=" & dvScreen.Item(j).Item("ObjectID")
                            End If
                            mobjChildNode.NavigateUrl = strURL
                        End If

                        mobjChildNode.Target = "MainPage"
                        mobjChildNode.ExpandedImageUrl = "../images/sort_down.gif"
                        If Not IsDBNull(dvScreen.Item(j).Item("ImageURL")) Then
                            mobjChildNode.ImageUrl = dvScreen.Item(j).Item("ImageURL")
                            mobjChildNode.SelectedImageUrl = dvScreen.Item(j).Item("ImageURL")
                        Else
                            mobjChildNode.ImageUrl = ""
                            mobjChildNode.SelectedImageUrl = ""
                        End If

                        mobjTreeMenu.Nodes(i).Nodes.Add(mobjChildNode)
                    Next
                Catch ex As Exception
                End Try
            Next
            MenuPnl.Controls.Add(mobjTreeMenu)
        Catch ex As Exception
        End Try
    End Function
    Private Function FilterDataTable(ByVal objPID As String, ByVal dtObject As DataTable) As DataView
        Dim dtTable As DataTable
        Dim dvPC As DataView
        Dim strFilter As String
        strFilter = "ObjectPID = " & objPID
        dtTable = dtObject
        dvPC = New DataView(dtTable, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
        Return dvPC
    End Function

    Private Function FilterControlTable(ByVal objPID As String, ByVal dtObject As DataTable) As DataView
        Dim dtTable As DataTable
        Dim dvPC As DataView
        Dim strFilter, strCount As String
        strFilter = "ObjectPID = " & objPID
        dtTable = dtObject
        dvPC = New DataView(dtTable, strFilter, "ObjectPID", DataViewRowState.CurrentRows)
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
                    ctl2 = CType(ctl, ImageButton)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is RadioButton Then
                    Dim ctl2 As RadioButton
                    ctl2 = CType(ctl, RadioButton)
                    ctl2.Enabled = False
                ElseIf TypeOf ctl Is CheckBox Then
                    Dim ctl2 As CheckBox
                    ctl2 = CType(ctl, CheckBox)
                    ctl2.Enabled = False
                End If
            End If
        End If

    End Sub

    Public Function ControlSecurity(ByVal frm As Control, ByVal intScrID As Integer) As Boolean
        'System.Web.UI.ControlCollection
        'Dim strTableName, strControlName As String
        Dim dtScrTable As New DataTable
        'Dim intCount, intControlCount As Int16
        GetDataControls("Ctl", dtScrTable)

        '        intControlCount = frm.Controls.Count
        Dim dvCtl As DataView

        dvCtl = FilterControlTable(intScrID, dtScrTable)
        dvCtl.Sort = "Name"

        For Each ctl As Control In frm.Controls
            For Each ctl1 As Control In ctl.Controls
                If (TypeOf (ctl1) Is CustomControls.Web.CollapsiblePanel) Then
                    For Each ctl2 As Control In ctl1.Controls
                        If (TypeOf (ctl2) Is System.Web.UI.WebControls.Image) Or (TypeOf (ctl2) Is TextBox) Or (TypeOf (ctl2) Is Button) Or (TypeOf (ctl2) Is Label) Or (TypeOf (ctl2) Is DropDownList) Or (TypeOf (ctl2) Is ListBox) Or (TypeOf (ctl2) Is ImageButton) Or (TypeOf (ctl2) Is RadioButton) Or (TypeOf (ctl2) Is CheckBox) Then
                            setControlAttribute(ctl2, dvCtl)
                        End If
                    Next
                Else
                    If (TypeOf (ctl1) Is System.Web.UI.WebControls.Image) Or (TypeOf (ctl1) Is TextBox) Or (TypeOf (ctl1) Is Button) Or (TypeOf (ctl1) Is Label) Or (TypeOf (ctl1) Is DropDownList) Or (TypeOf (ctl1) Is ListBox) Or (TypeOf (ctl1) Is ImageButton) Or (TypeOf (ctl1) Is RadioButton) Or (TypeOf (ctl1) Is CheckBox) Then
                        setControlAttribute(ctl1, dvCtl)
                    End If
                End If
            Next
        Next
    End Function
    Private Function isExisted(ByRef dvCtl As DataView, ByVal strCtlName As String) As Boolean
        Dim intIndex As Int16
        '        Dim str1 As String
        'Dim dvCtl1 As DataView

        '   str1 = dvCtl.Count
        '   For i As Int16 = 0 To dvCtl.Count - 1
        '  str1 = dvCtl.Item(i).Item(1)
        '  Next
        intIndex = dvCtl.Find(strCtlName)
        If intIndex <> -1 Then
            strView = dvCtl.Item(intIndex).Item("VH")
            strEnable = dvCtl.Item(intIndex).Item("ED")
            Return True
        Else
            Return False
        End If
    End Function
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
                    & " WHERE UM.UM_VC50_UserID ='" & PropUserName & "' AND " _
                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND " _
                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                    & " (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D') AND " _
                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND " _
                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK AND " _
                    & " OBM.OBM_VC4_Object_Type_FK <>'MNU' and OBM.OBM_VC4_Object_Type_FK <>'SCR'" _
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
            Dim str As String
            str = ex.Message
            'Throw
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
                    & " WHERE UM.UM_VC50_UserID ='" & PropUserName & "' AND " _
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
            Dim str As String
            str = ex.Message
            'Throw
        Finally
            objCon.Close()
        End Try

    End Sub
End Class
