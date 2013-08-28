'************************************************************************************************************
' Page                 :- Events
' Purpose              :- When the user will click on any of the date in Msgcalender.aspx page .it will                                show all the events defined on that particular date. 
' Tables used          :- T090011
' Date				Author						Modification Date					Description
' 13/06/06			Harpreet			       -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.Data


Partial Class MessageCenter_HomeCalender_Events
    Inherits System.Web.UI.Page
    Private strDate As String
    Private dsEvent As New DataSet
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        '  cpnlError.Visible = False
        txtCSS(Me.Page)
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")

        'imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        'imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        imgAdd.Attributes.Add("Onclick", "return SaveEdit('Add');")

        strDate = Request.QueryString("strDate")
        TitleLabelDate.Text = SetDateFormat(CDate(strDate), mdlMain.IsTime.DateOnly)
        txthiddenDate.Value = strDate

        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")
        If strhiddenImage <> "" Then
            Try
                Select Case strhiddenImage
                    Case "Save"
                        UpdateEventStatus()
                End Select
            Catch ex As Exception
                CreateLog("msgcalender", "Load-132", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If


        BindList()

    End Sub

    Private Function BindList() As Boolean

        Try

            SQL.DBTracing = False
            ' SQL.DBTable = "T090011"
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            Dim strSQL As String
            strSQL = "select  CL_NU9_ID_PK, CL_VC8_Event, B.UM_VC50_UserID as CL_NU9_Event_Owner_FK, A.UM_VC50_UserID as CL_NU9_UserID_FK, case CL_VC4_Status when 'ENA' then 'true' else 'false' end CL_VC4_Status, CL_VC50_Subject, CL_VC500_Message from T090011 , T060011 A, T060011 B where CL_NU9_UserID_FK=A.UM_IN4_Address_No_FK and CL_NU9_Event_Owner_FK=B.UM_IN4_Address_No_FK and (CL_NU9_Event_Owner_FK=" & Session("PropUserID") & " OR CL_NU9_UserID_FK=" & Session("PropUserID") & ") and CL_DT8_EventDate='" & strDate & "'"

            If SQL.Search("T090011", "Events", "BindList", strSQL, dsEvent, "", "") = True Then
                dlstEvent.DataSource = dsEvent.Tables("T090011")
                dlstEvent.DataBind()
                cpnlEvent.Enabled = True
                cpnlEvent.State = CustomControls.Web.PanelState.Expanded
            Else
                lstError.Items.Clear()
                lstError.Items.Add("No Message Defined for this date...")
                'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                cpnlEvent.Enabled = False
                cpnlEvent.State = CustomControls.Web.PanelState.Collapsed
            End If


        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Please Try Later...")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
        End Try
    End Function

    Private Sub dlstEvent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlstEvent.ItemDataBound
        Try
            Dim ctrlImg As Control = e.Item.FindControl("imgEdit")
            If TypeOf ctrlImg Is System.Web.UI.WebControls.Image Then
                If CStr(dsEvent.Tables("T090011").Rows(e.Item.ItemIndex).Item("CL_NU9_UserID_FK")).ToUpper.Equals(CStr(Session("PropUserName")).ToUpper) Then
                    CType(ctrlImg, System.Web.UI.WebControls.Image).Attributes.Add("onclick", "return ShowEdit('" & dsEvent.Tables("T090011").Rows(e.Item.ItemIndex).Item(0) & "','" & strDate & "');")
                Else
                    CType(ctrlImg, System.Web.UI.WebControls.Image).Attributes.Add("onclick", "return ShowAlert();")
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function UpdateEventStatus() As Boolean
        Try
            Dim strUpdate As String = ""

            For Each DI As DataListItem In dlstEvent.Items
                Dim ctrlC As Control = DI.FindControl("chkR")
                Dim ctrlT As Control = DI.FindControl("txtID")
                strUpdate &= "update T090011 set CL_VC4_Status='" & IIf(CType(ctrlC, CheckBox).Checked, "ENA", "DISA") & "' where CL_NU9_ID_PK=" & Val(CType(ctrlT, HtmlInputHidden).Value) & ";"
            Next
            If strUpdate.Trim.Equals("") = False Then
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                lstError.Items.Clear()
                If SQL.Update("", "", strUpdate, SQL.Transaction.Serializable) = True Then
                    lstError.Items.Add("Message Status Updated Successfully...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                Else
                    lstError.Items.Add("Message Status Not Updated, Please try Later...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                End If
            End If
        Catch ex As Exception
        End Try
    End Function

    Private Sub imgSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSave.Click
        UpdateEventStatus()
    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
