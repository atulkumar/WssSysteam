Imports Telerik.Web.UI
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports ION.Common.DAL
Imports System.Collections.Generic
Imports System.Drawing

Partial Class SupportCenter_CallView_ActionViewOnly
    Inherits System.Web.UI.Page

    Private intID As Int16
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Security Block
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intID = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
        End If
        'End of Security Block

        'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")

        ViewState("ActionViewCallNo") = Request.QueryString("intCallNo")
        ViewState("ActionViewTaskNo") = Request.QueryString("intTaskNo")
        ViewState("ActionViewCompanyName") = Request.QueryString("strComp")
        mstGetFunctionValue = WSSSearch.SearchCompName(ViewState("ActionViewCompanyName"))
        ViewState("ActionViewCompanyID") = mstGetFunctionValue.ExtraValue

    End Sub

#Region "CreateDataTableAction"
    Private Sub CreateDataTableAction(ByVal CompanyID As Integer, ByVal CallNo As Integer, ByVal TaskNo As Integer)

        Dim dsAction As New DataSet
        Dim strSql As String
        'New check-> Only dispaly External action.
        If Session("PropCompanyType") = "SCM" Then
            strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description as Description,b.UM_VC50_UserID as ActionOwner, convert(varchar,AM_DT8_Action_Date) as ActionDate,AM_FL8_Used_Hr  as UserHours From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & CallNo & " and AM_NU9_Comp_ID_FK=" & CompanyID & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  And  AM_NU9_Task_Number=" & TaskNo
        Else
            strSql = "select AM_CH1_Comment as Blank1, AM_CH1_Attachment as Blank2,AM_NU9_Action_Number,AM_VC_2000_Description as Description,b.UM_VC50_UserID as ActionOwner, convert(varchar,AM_DT8_Action_Date) as ActionDate,AM_FL8_Used_Hr as UserHours From T040031 a,T060011 b   Where AM_NU9_Call_Number=" & CallNo & " and AM_NU9_Comp_ID_FK=" & CompanyID & " and b.UM_IN4_Address_No_FK=a.AM_VC8_Supp_Owner  and AM_VC8_ActionType='External' and AM_NU9_Task_Number=" & TaskNo
        End If
        strSql = strSql & " Order By AM_NU9_Action_Number asc"

        clsData.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        clsData.DBProvider = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ProviderName.ToString

        dsAction = clsData.SearchDS(strSql)
        'Dim intActionNo As Integer = dsAction.Tables(0).Rows(0).Item("AM_NU9_Action_Number")
        SetCommentFlag(dsAction.Tables(0).DefaultView, mdlMain.CommentLevel.ActionLevel, CompanyID, CallNo, TaskNo, 0)
        radActionOnly.DataSource = dsAction.Tables(0).DefaultView
    End Sub
#End Region

    Protected Sub radActionOnly_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles radActionOnly.ColumnCreated
        Try
            Dim col As GridColumn = e.Column
            col.AutoPostBackOnFilter = True
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub radActionOnly_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles radActionOnly.ItemDataBound
        Try
            Dim lnk As New LinkButton
            Dim blnUser As Boolean = False

            If (e.Item.OwnerTableView.DetailTableIndex.ToString() = "-1") Then
                If TypeOf e.Item Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
                    Dim AttachmentExists As Integer = 0
                    Dim CommentsExists As Integer = 0

                    If Not CType(e.Item.DataItem, DataRowView)("Blank2") Is DBNull.Value Then
                        AttachmentExists = CType(e.Item.DataItem, DataRowView)("Blank2")
                    End If

                    If Not CType(e.Item.DataItem, DataRowView)("Blank1") Is DBNull.Value Then
                        CommentsExists = CType(e.Item.DataItem, DataRowView)("Blank1")
                    End If

                    Dim imgAtt As ImageButton = CType(e.Item.FindControl("imgAtt"), ImageButton)
                    Dim imgComm As ImageButton = CType(e.Item.FindControl("imgComm"), ImageButton)

                    If (AttachmentExists = "1") Then
                        imgAtt.ImageUrl = "../../Images/Attach15_9.gif"
                        imgAtt.Attributes("href") = "#"
                        imgAtt.Attributes("onclick") = [String].Format("return ShowAttachmentForm('{0}','{1}','{2}',{3});return false;", ViewState("ActionViewCompanyID"), ViewState("ActionViewCallNo"), ViewState("ActionViewTaskNo"), CType(e.Item.DataItem, DataRowView)("AM_NU9_Action_Number"))
                    Else
                        imgAtt.Visible = False
                    End If

                    If (CommentsExists = "0") Then
                        'If there are no comments
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("return ShowCommentsForm('{0}','{1}','{2}',{3});", ViewState("ActionViewCompanyID"), ViewState("ActionViewCallNo"), ViewState("ActionViewTaskNo"), CType(e.Item.DataItem, DataRowView)("AM_NU9_Action_Number"))
                    ElseIf (CommentsExists = "1") Then
                        'If comments are already read
                        imgComm.ImageUrl = "../../images/comment2.gif"
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("return ShowCommentsForm('{0}','{1}','{2}',{3});", ViewState("ActionViewCompanyID"), ViewState("ActionViewCallNo"), ViewState("ActionViewTaskNo"), CType(e.Item.DataItem, DataRowView)("AM_NU9_Action_Number"))
                    ElseIf (CommentsExists = "2") Then
                        'if new Comments are posted
                        imgComm.ImageUrl = "../../images/comment_Unread.gif"
                        imgComm.Attributes("href") = "#"
                        imgComm.Attributes("onclick") = [String].Format("return ShowCommentsForm('{0}','{1}','{2}',{3});", ViewState("ActionViewCompanyID"), ViewState("ActionViewCallNo"), ViewState("ActionViewTaskNo"), CType(e.Item.DataItem, DataRowView)("AM_NU9_Action_Number"))
                    End If
                End If
                If TypeOf e.Item Is GridDataItem Then
                    Dim dataItem1 As GridDataItem = CType(e.Item, GridDataItem)
                    Dim lnkCommentBy As String
                    If Not IsNothing(e.Item.FindControl("LnkRequestBy")) Then
                        ' Dim Callno As String = dataItem1("CALLNO").Text
                        ' Dim CompId As String = dataItem1("COMPID").Text
                        ' Dim CallOwner As String = dataItem1("CommentBy").Text

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
                                'blnCallReqBy = True
                            End If
                        End If
                        'If (blnCallReqBy = False) Then
                        '    Me.RadToolTipManager1.Visible = False
                        'Else
                        '    Me.RadToolTipManager1.Visible = True
                        'End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
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
    Protected Sub radActionOnly_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radActionOnly.NeedDataSource
        CreateDataTableAction(ViewState("ActionViewCompanyID"), ViewState("ActionViewCallNo"), ViewState("ActionViewTaskNo"))
    End Sub
End Class
