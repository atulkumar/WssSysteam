Imports System.Data
Imports System.Data.SqlClient
Imports ION.Data
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls

Partial Class MonitoringCenter_PSFTHome
    Inherits System.Web.UI.Page
    Private Shared mobjParentNode As TreeNode
    Private Shared mobjChildNode As TreeNode
    Private Shared mobjContolNode As TreeNode


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' IFR.Attributes.Add("src", "BGDailyMonitor.aspx")
        If Not IsPostBack Then
            CreatePSFTMenu(PSFTTree)
        End If
        imgSave.Attributes.Add("onclick", "return SubmitIFrame('Save');")
        imgDelete.Attributes.Add("onclick", "return SubmitIFrame('Delete');")
        imgAdd.Attributes.Add("onclick", "return SubmitIFrame('Add');")


        If IsPostBack Then
            Dim strhiddenImage As String
            strhiddenImage = Request.Form("txthiddenImage")

            Dim intID As Integer = Val(Request.Form("txthiddenID"))
            Dim intReqID As Integer = Val(Request.Form("txthiddenReqID"))

            If strhiddenImage <> "" Then

                Select Case strhiddenImage
                    Case "Logout"
                        LogoutWSS()
                End Select

            End If
        Else



        End If

    End Sub

    Private Function CreatePSFTMenu(ByVal mobjTreeMenu As TreeView)
        Try
            Dim strCon As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            Dim sqCON As New SqlConnection(strCon)
            Dim dsMenu As New DataSet
            Dim dsSCR As New DataSet
            Dim dsBTN As New DataSet
            Dim sqAdp As SqlDataAdapter
            If sqCON.State = ConnectionState.Closed Then
                sqCON.Open()
            End If
            Dim strMnuSQL As String = "select OBM_IN4_Object_ID_PK as ObjectID,OBM_VC50_Object_Name as Name, OBM_VC50_Alias_Name as AName,OBM_VC200_Image as ImageURL from T070011 where OBM_VC4_Object_Type_FK in ('MMNU') and  OBM_VC4_Status_Code_FK = 'ENB'  order by OBM_IN4_Object_ID_PK"
            sqAdp = New SqlDataAdapter(strMnuSQL, sqCON)
            dsMenu.Clear()
            sqAdp.Fill(dsMenu)

            mobjTreeMenu.ShowLines = False
            mobjTreeMenu.ShowPlus = False
            mobjTreeMenu.SelectExpands = False

            mobjTreeMenu.Font.Name = "Verdana"
            mobjTreeMenu.ForeColor = System.Drawing.Color.LightGray
            mobjTreeMenu.ExpandedImageUrl = "../images/sort_down.gif"
            mobjTreeMenu.ExpandLevel = 5

            For intI As Integer = 0 To dsMenu.Tables(0).Rows.Count - 1

                mobjParentNode = New TreeNode
                mobjParentNode.Expanded = False
                mobjParentNode.ImageUrl = "../images/sort_right.gif"
                mobjParentNode.ExpandedImageUrl = dsMenu.Tables(0).Rows(intI).Item("ImageURL")
                mobjParentNode.Text = "<font Size=""1""  bgcolor=""lightgray"" face=""verdana""><b>" & dsMenu.Tables(0).Rows(intI).Item("AName") & "</b></font>"
                mobjTreeMenu.Nodes.Add(mobjParentNode)

                Dim strScrSQL = "select OBM_IN4_Object_ID_PK as ObjectID,OBM_VC4_Object_Type_FK as ObjType,OBM_VC50_Object_Name as Name, OBM_VC50_Alias_Name as AName, OBM_IN4_Object_PID_FK as ObjectPID,OBM_VC200_URL as PageURL,OBM_VC200_Image as ImageURL from T070011 where OBM_VC4_Object_Type_FK in ('MSCR') and  OBM_VC4_Status_Code_FK = 'ENB'  and OBM_IN4_Object_PID_FK=" & dsMenu.Tables(0).Rows(intI).Item("ObjectID") & " order by OBM_VC4_Object_Type_FK "
                sqAdp = New SqlDataAdapter(strScrSQL, sqCON)
                dsSCR.Clear()
                sqAdp.Fill(dsSCR)
                For intJ As Integer = 0 To dsSCR.Tables(0).Rows.Count - 1
                    mobjChildNode = New TreeNode
                    mobjChildNode.Expanded = False
                    mobjChildNode.Text = "<font Size=""1""  bgcolor=""lightgray"" face=""verdana""><b>" & dsSCR.Tables(0).Rows(intJ).Item("AName") & "</b></font>"
                    mobjChildNode.ImageUrl = "../images/sort_right.gif"
                    mobjChildNode.ExpandedImageUrl = dsSCR.Tables(0).Rows(intJ).Item("ImageURL")
                    mobjTreeMenu.Nodes(intI).Nodes.Add(mobjChildNode)
                    Dim strBtnSQL = "select OBM_IN4_Object_ID_PK as ObjectID,OBM_VC4_Object_Type_FK as ObjType,OBM_VC50_Object_Name as Name, OBM_VC50_Alias_Name as AName, OBM_IN4_Object_PID_FK as ObjectPID, OBM_VC200_URL as PageURL,OBM_VC200_Image as ImageURL from T070011 where OBM_VC4_Object_Type_FK in ('MBTN') and  OBM_VC4_Status_Code_FK = 'ENB'  and OBM_IN4_Object_PID_FK=" & dsSCR.Tables(0).Rows(intJ).Item("ObjectID") & " order by OBM_VC4_Object_Type_FK "
                    sqAdp = New SqlDataAdapter(strBtnSQL, sqCON)
                    dsBTN.Clear()
                    sqAdp.Fill(dsBTN)
                    For intK As Integer = 0 To dsBTN.Tables(0).Rows.Count - 1
                        mobjContolNode = New TreeNode
                        mobjContolNode.Expanded = False
                        mobjContolNode.ImageUrl = "../images/sort_right.gif"
                        mobjContolNode.ExpandedImageUrl = dsBTN.Tables(0).Rows(intK).Item("ImageURL")
                        mobjContolNode.Text = "<font Size=""1""  bgcolor=""lightgray"" face=""verdana""><b>" & dsBTN.Tables(0).Rows(intK).Item("AName") & "</b></font>"
                        mobjContolNode.NavigateUrl = dsBTN.Tables(0).Rows(intK).Item("PageURL")
                        mobjContolNode.Target = "IFrame"
                        mobjTreeMenu.Nodes(intI).Nodes(intJ).Nodes.Add(mobjContolNode)
                    Next

                Next

            Next
            sqCON.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub imgClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        Response.Redirect("Configuration.aspx", False)
    End Sub
End Class
