Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Collections.Generic
Imports ION.BusinessUnit
Imports ION.BusinessLogic
'*******************************************************************
' Function                To Display Comments
' Date					  18/03/2009
' Author				  Atul Sirpal(atul.kumar@ionsoftnet.com)
'*******************************************************************


Partial Class NewCall_CommentView_Simple
    Inherits System.Web.UI.Page
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Logout"
                        LogoutWSS()
                    Case "Close"
                        Response.Redirect("../../Home.aspx", False)
                    Case "CloseCall"
                        If Session("blnOldComment") = 0 Then
                            Session("blnOldComment") = 1
                        Else
                            Session("blnOldComment") = 0
                        End If
                        Session("PropCallNumber") = "0"
                        GrdComments.MasterTableView.SortExpressions.Clear()
                        GrdComments.MasterTableView.FilterExpression = Nothing
                        GrdComments.Rebind()
                End Select
            Catch ex As Exception
                CreateLog("DocumentView", "Load-218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        'For Case-Insensitive search
        GrdComments.GroupingSettings.CaseSensitive = False

        If Not IsPostBack Then
            'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgCloseCall.Attributes.Add("Onclick", "return SaveEdit('CloseCall');")

            If IsNothing(Session("blnOldComment")) = True Then
                Session("blnOldComment") = 1
            End If
        Else

        End If

        If Session("blnOldComment") = 1 Then
            imgCloseCall.ToolTip = "View Only UnRead Comments"
        Else
            imgCloseCall.ToolTip = "View Only Read Comments"
        End If

        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")

            intId = 968
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
        'End of Security Block
        'To check Page Size in database if yes then set the grid pagesize from db
        If ChkPageView() = True Then
            GrdComments.PageSize = Convert.ToInt32(Session("PageSize"))
        Else
            GrdComments.PageSize = 20
        End If
    End Sub

#Region "#######################################Bind Grid#####################################"
    Private Sub BindCommentAlertGrid()
        Try
            Dim dsComments As New DataSet
            Dim blnCommentFlag As Integer = 1
            If Session("blnOldComment") = 0 Then
                blnCommentFlag = 1
            Else
                blnCommentFlag = 0
            End If
            Dim objCommentViewBO As New ION.BusinessUnit.CommentViewBO()
            objCommentViewBO.blnCommentFlag = Convert.ToBoolean(blnCommentFlag)
            objCommentViewBO.intPropUserID = Convert.ToInt32(Val(Session("PropUserID")))
            Dim objCommentViewBAL As New ION.BusinessLogic.CommentViewBAL()
            If (ViewState("dsViewComments") Is Nothing) Then
                dsComments = objCommentViewBAL.GetComments(objCommentViewBO)
                
                ViewState("dsViewComments") = dsComments
            Else
                dsComments = DirectCast(ViewState("dsViewComments"), DataSet)
            End If
            Dim dtview As DataView = dsComments.Tables(0).DefaultView
            If Session("blnOldComment") = 0 Then
                dtview.RowFilter = "CommentFlag=0"
            Else
                dtview.RowFilter = "CommentFlag=1"
            End If
            
            'If there are no rows in table TooltipManger will not be visible.
            If (dsComments.Tables(0).Rows.Count <= 0) Then
                Me.RadToolTipManager1.Visible = False
            End If
            GrdComments.DataSource = dtview
        Catch ex As Exception
            CreateLog("Home", "BindCommentAlertGrid-204", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTask")
        End Try
    End Sub
#End Region

#Region "###################################Grid Related Events###################################"

    Protected Sub GrdComments_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles GrdComments.ColumnCreated
        Try
            Dim col As GridColumn = e.Column
            col.AutoPostBackOnFilter = True
            col.FilterListOptions = GridFilterListOptions.VaryByDataType
            Dim filteringItem As GridColumn = CType(e.Column, GridColumn)

            'If col.UniqueName = "CallNo" Then
            '    col.FilterListOptions.
            'End If
            'If (col.DataTypeName = "System.Decimal") Then
            Dim menu As GridFilterMenu = GrdComments.FilterMenu
            Dim i As Integer = 0
            While i < menu.Items.Count
                If menu.Items(i).Text = "Between" Or menu.Items(i).Text = "NotBetween" Then
                    menu.Items.RemoveAt(i)
                Else
                    i = i + 1
                End If
            End While
            'End If
        Catch ex As Exception
            CreateLog("CommentView", "ColumnCreated", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        
    End Sub

    Protected Sub GrdComments_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GrdComments.ItemCreated
        'Setting the Width of Filtering Boxes i.e setting the width of columns of main table(Calls table)

        If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then
            Try
                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filteringItem As GridFilteringItem = CType(e.Item, GridFilteringItem)
                    'set dimensions for the filter textbox
                    Dim box As RadNumericTextBox = CType(filteringItem("CallNo").Controls(0), RadNumericTextBox)
                    box.Width = Unit.Pixel(40)
                    box = CType(filteringItem("TaskNo").Controls(0), RadNumericTextBox)
                    box.Width = Unit.Pixel(40)
                    box = CType(filteringItem("ActionNo").Controls(0), RadNumericTextBox)
                    box.Width = Unit.Pixel(40)

                    Dim box1 As TextBox
                    box1 = CType(filteringItem("CommentTo").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(60)
                    box1 = CType(filteringItem("CommentBy").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(60)
                    box1 = CType(filteringItem("CommentLevel").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(60)
                    box1 = CType(filteringItem("CommentDesc").Controls(0), TextBox)
                    box1.Width = Unit.Pixel(250)
                End If
            Catch ex As Exception
                CreateLog("CommentView", "ItemCreated", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
            
        End If
    End Sub

    Protected Sub GrdComments_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GrdComments.ItemDataBound
        Try
            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then

                If TypeOf e.Item Is GridDataItem Then
                    e.Item.Attributes.Add("style", "cursor:hand")

                    If Not IsNothing(e.Item.Cells(0).FindControl("imgComment")) Then 'Level,CompID,CN,TN,AN
                        'CType(e.Item.Cells(0).FindControl("imgComment"), System.Web.UI.WebControls.Image).Attributes.Add("OnClick", "OpenComment('" & e.Item.Cells(7).Text.Trim & "','" & e.Item.Cells(5).Text.Trim & "','" & e.Item.Cells(11).Text.Trim & "','" & e.Item.Cells(12).Text.Trim & "','" & e.Item.Cells(13).Text.Trim & "' )")
                        CType(e.Item.Cells(0).FindControl("imgComment"), System.Web.UI.WebControls.Image).Attributes.Add("OnClick", "OpenComment('" & CType(e.Item.DataItem, DataRowView)("CM_VC2_Flag") & "','" & CType(e.Item.DataItem, DataRowView)("CompID") & "','" & CType(e.Item.DataItem, DataRowView)("CallNo") & "','" & CType(e.Item.DataItem, DataRowView)("TaskNo") & "','" & CType(e.Item.DataItem, DataRowView)("ActionNo") & "' )")
                        e.Item.Cells(0).Attributes.Add("style", "cursor:hand")

                    End If
                    'If e.Item.Cells(13).Text.Trim = "1" Then
                    If CType(e.Item.DataItem, DataRowView)("CommentFlag") = "1" Then
                        CType(e.Item.FindControl("imgComment"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment_Unread.gif"
                    Else
                        CType(e.Item.FindControl("imgComment"), System.Web.UI.WebControls.Image).ImageUrl = "../../Images/comment2.gif"
                    End If

                    'Code For Tooltip 
                    Dim blnCallReqBy As Boolean = False
                    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim lnkCommentBy As String
                    If Not IsNothing(e.Item.FindControl("LnkRequestBy")) Then
                        Dim Callno As String = dataItem("CALLNO").Text
                        Dim CompId As String = dataItem("COMPID").Text
                        Dim CallOwner As String = dataItem("CommentBy").Text

                        Dim LnkRequestBy As LinkButton
                        LnkRequestBy = e.Item.FindControl("LnkRequestBy")

                        lnkCommentBy = LnkRequestBy.ID
                        LnkRequestBy.Attributes("href") = "#"

                        'Code to open window onClick
                        'LnkRequestBy.Attributes("onclick") = [String].Format("return ShowCallReqByInfo('{0}','{1}','{2}','{3}');", CompId, Callno, CallOwner, e.Item.ItemIndex)

                        Dim hlAddProducts As Control = e.Item.FindControl(lnkCommentBy)
                        If IsNothing(hlAddProducts) = False Then
                            Dim currentRow As DataRowView = CType(e.Item.DataItem, DataRowView)
                            If IsNothing(Me.RadToolTipManager1) = False Then
                                Me.RadToolTipManager1.TargetControls.Add(hlAddProducts.ClientID, LnkRequestBy.Text, True)
                                blnCallReqBy = True
                            End If
                        End If
                        If (blnCallReqBy = False) Then
                            Me.RadToolTipManager1.Visible = False
                        Else
                            Me.RadToolTipManager1.Visible = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            CreateLog("AB_Search", "ItemDataBound-720", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Protected Sub GrdComments_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles GrdComments.NeedDataSource
        Try
            If Not e.IsFromDetailTable Then
                Call BindCommentAlertGrid()
            End If
        Catch ex As Exception
            CreateLog("CommentView", "NeedDataSource", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        
    End Sub
#End Region

    Protected Sub RadToolTipManager1_AjaxUpdate(ByVal sender As Object, ByVal e As Telerik.Web.UI.ToolTipUpdateEventArgs)
        Try
            Dim ctrl As Control = Page.LoadControl("UserDetails.ascx")
            e.UpdatePanel.ContentTemplateContainer.Controls.Add(ctrl)
            Dim details As UserDetails = DirectCast(ctrl, UserDetails)
            details.GetCallOwner = e.Value.ToString()
        Catch ex As Exception
            CreateLog("CommentView", "Load-218", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub
    Private Sub SavePageSize()
        Dim intid = 968
        Dim intcount As Integer

        Dim strCheck As String = SQL.Search("Historicview", "SavePageSize-3406", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "")

        If Not IsNothing(strCheck) Then

            'update
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            arColumnName.Add("PS_NU9_PSize")

            arRowData.Add(Val(Session("PageSize")))


            If SQL.Update("T030214", "SavePageSIZE", "update  T030214 set PS_NU9_PSize=" & Val(Session("PageSize")) & "  where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID='" & intid & "' and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid"), SQL.Transaction.Serializable) = True Then
                'Save message
            Else
                'Error message
            End If
        Else
            'save
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList

            arColumnName.Add("PS_NU9_PSize")
            arColumnName.Add("PS_NU9_ScreenID")
            arColumnName.Add("PS_NU9_RoleID")
            arColumnName.Add("PS_NU9_ComID")
            arColumnName.Add("PS_NU9_UserID") 'Added new field to store user id with view records

            arRowData.Add(Val(Session("PageSize")))
            arRowData.Add(intid)
            arRowData.Add(Session("PropRole"))
            arRowData.Add(Session("PropCompanyID"))
            arRowData.Add(Session("PropUserID"))

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Save("T030214", "SaveUserView", "SaveUserView-3436", arColumnName, arRowData) = True Then
                'Save message
            Else
                'Error message
            End If
        End If
    End Sub

    Private Function ChkPageView() As Boolean
        Dim sqdrCol As SqlDataReader
        Dim blnReturn As Boolean

        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select * from T030214 where  PS_NU9_RoleID=" & Val(Session("PropRole")) & " and PS_NU9_ScreenID=968 and PS_NU9_ComID=" & Val(Session("PropCompanyID")) & " And PS_NU9_UserID = " & Session("Propuserid") & "", SQL.CommandBehaviour.CloseConnection, blnReturn)

            If blnReturn = False Then
                Return False
                Exit Function
            Else
                While sqdrCol.Read
                    Session("PageSize") = sqdrCol.Item("PS_NU9_PSize")
                End While
                Return True
            End If

            sqdrCol.Close()
            sqdrCol = Nothing

        Catch ex As Exception
            CreateLog("Comments View", "ChkSelectedView-2080", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Protected Sub GrdComments_PageSizeChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles GrdComments.PageSizeChanged
        If e.NewPageSize >= 0 Then
            Session("PageSize") = e.NewPageSize.ToString()
            SavePageSize()
        Else

        End If
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
