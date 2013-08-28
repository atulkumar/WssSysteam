'************************************************************************************************************
' Page                 :- MsgCalender
' Purpose              :- This screen will show the calendar.User can click on any  date to know about events                          on that date.   

' Tables used          :- UDC, T010011, T090011
' Date				Author						Modification Date					Description
' 12/06/06			Harpreet			       -------------------					Created
' ' Note
' ' Code:
'************************************************************************************************************
Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient


Partial Class MessageCenter_HomeCalender_MsgCalender
    Inherits System.Web.UI.Page

    Private strDate As String
    Private Shared intID As Integer
    Private Shared strTask As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)
        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
        txtMessage.Attributes.Add("onmousemove", "ShowToolTip(this,500);")

        'txtMessage.Attributes.Add("onkeypress", "MaxLength('" & txtMessage.ClientID & "',200)")

        If IsPostBack = False Then
            strTask = Request.QueryString("Task")
            If strTask = "Add" Then

            Else
                intID = Request.QueryString("ID")
            End If
        End If
        strDate = Request.QueryString("strDate")
        TitleLabelDate.Text = SetDateFormat(CDate(strDate), mdlMain.IsTime.DateOnly)

        '''Event type''''''''''''''''''''''
        If Session("PropCompanyType") = "SCM" Then
            CDDLEventType.CDDLQuery = "select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011 where UDC.Company*=CI_NU8_Address_Number and ProductCode=0 and UDCType=""CALE"" Order By Name"
        Else
            CDDLEventType.CDDLQuery = " Select Name as ID,Description,CI_VC36_Name as Company from UDC,T010011  where UDC.Company=CI_NU8_Address_Number and ProductCode=0   and UDCType=""CALE""" & _
            " and UDC.Company=" & Session("PropCompanyID") & "  union " & _
            " Select Name as ID,Description,"""" as Company from UDC  where  ProductCode=0   and UDCType=""CALE""" & _
            " and UDC.Company=0 Order By Name"
        End If
        CDDLEventType.CDDLUDC = True
        CDDLEventType.CDDLType = CustomDDL.DDLType.NonFastEntry

        '''''''''''''''''''''''''''''''''''''''''
        '''Event User''''''''''''''''''''''
        If Session("PropCompanyType") = "SCM" Then
            'Old Query
            '    CDDLUser.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and UM_IN4_Company_AB_ID in (select CI_NU8_Address_Number from T010011 where CI_IN4_Business_Relation='SCM') Order By Name"
            'Else
            '    CDDLUser.CDDLQuery = "SELECT um_in4_address_no_fk as ID,um_vc50_userid as Name,ci_vc36_name  as Company FROM t060011,t010011 where ci_nu8_address_number=um_in4_company_ab_id and (UM_IN4_Company_AB_ID in (select CI_NU8_Address_Number from T010011 where CI_IN4_Business_Relation='SCM') or UM_IN4_Company_AB_ID=" & Val(Session("PropCompanyID")) & ") Order By Name"

            'new Query  to get full name

            CDDLUser.CDDLQuery = " SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,t010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where UM_VC4_Status_Code_FK='ENB' and t010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and  UM_IN4_Company_AB_ID in (select CI_NU8_Address_Number from T010011 where CI_IN4_Business_Relation='SCM') Order By Name "

        Else
            CDDLUser.CDDLQuery = " SELECT um_in4_address_no_fk as ID,(um_vc50_userid + '[' + UName.ci_vc36_name + ']') as Name,t010011.ci_vc36_name  as Company FROM T060011,T010011,T010011 UName where UM_VC4_Status_Code_FK='ENB' and t010011.ci_nu8_address_number=um_in4_company_ab_id and UName.ci_nu8_address_number=um_in4_address_no_fk and (um_in4_company_ab_id=" & Val(Session("PropCompanyID")) & ")  and UM_IN4_Company_AB_ID in (select CI_NU8_Address_Number from T010011 where CI_IN4_Business_Relation='SCM') Order By Name "


        End If
        CDDLUser.CDDLUDC = False
        CDDLUser.CDDLType = CustomDDL.DDLType.NonFastEntry

        '''''''''''''''''''''''''''''''''''''''''
        If IsPostBack = False Then
            CDDLEventType.CDDLFillDropDown(10, True)
            CDDLUser.CDDLFillDropDown(10, False)
            CDDLUser.CDDLSetSelectedItem(Session("PropUserID"), False, Session("PropUserName"))
        End If

        Dim strhiddenImage As String
        strhiddenImage = Request.Form("txthiddenImage")
        If strhiddenImage <> "" Then
            Try
                Select Case strhiddenImage
                    Case "Ok"
                        If SaveUpdateEvent() = True Then
                            Response.Write("<script>self.opener.ParentRefresh();</script>")
                            Response.Write("<script>self.opener.Form1.submit();</script>")
                            Response.Write("<script>window.close();</script>")
                        End If
                    Case "Save"
                        If SaveUpdateEvent() = True Then
                            Response.Write("<script>self.opener.ParentRefresh();</script>")
                            Response.Write("<script>self.opener.Form1.submit();</script>")
                        End If

                End Select
            Catch ex As Exception
                CreateLog("msgcalender", "Load-132", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        If strTask = "Edit" Then

            Try

                Dim sqRDR As SqlDataReader

                Dim blnStatus As Boolean
                sqRDR = SQL.Search("msgcalender", "Load-132", "select CL_NU9_ID_PK MsgID, CL_VC8_Event MsgType, A.UM_VC50_UserID as MsgFromName, CL_NU9_Event_Owner_FK MsgToID, B.UM_VC50_UserID as MsgToName, CL_NU9_UserID_FK MsgFromID, CL_VC50_Subject Subject, CL_VC500_Message Msg, CL_VC4_Status Status from T090011 , T060011 A, T060011 B where CL_NU9_UserID_FK=A.UM_IN4_Address_No_FK and CL_NU9_Event_Owner_FK=B.UM_IN4_Address_No_FK and CL_NU9_ID_PK=" & intID, SQL.CommandBehaviour.SingleRow, blnStatus)
                If blnStatus = True Then
                    While sqRDR.Read

                        txtSubject.Text = IIf(IsDBNull(sqRDR("Subject")), "", sqRDR("Subject"))
                        txtMessage.Text = IIf(IsDBNull(sqRDR("Msg")), "", sqRDR("Msg"))
                        CDDLEventType.CDDLSetSelectedItem(IIf(IsDBNull(sqRDR("MsgType")), "", sqRDR("MsgType")), True, IIf(IsDBNull(sqRDR("MsgType")), "", sqRDR("MsgType")))
                        CDDLUser.CDDLSetSelectedItem(IIf(IsDBNull(sqRDR("MsgToID")), "", sqRDR("MsgToID")), False, IIf(IsDBNull(sqRDR("MsgToName")), "", sqRDR("MsgToName")))
                        cbEnable.Checked = IIf(sqRDR("Status") = "ENA", True, False)
                    End While
                    sqRDR.Close()
                Else
                    lstError.Items.Clear()
                    lstError.Items.Add("No Message Defined...")
                    'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                    cpnlEvent.State = CustomControls.Web.PanelState.Collapsed
                End If
            Catch ex As Exception
                CreateLog("msgcalender", "Load-139", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try

        End If
    End Sub


    Private Function SaveUpdateEvent() As Boolean
        Try
            If ValidateEvent() = True Then

                Dim arCol As New ArrayList
                Dim arRow As New ArrayList

                SQL.DBTracing = False
                '                SQL.DBTable = "T090011"
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString


                arCol.Add("CL_VC8_Event")
                arCol.Add("CL_VC50_Subject")
                arCol.Add("CL_VC500_Message")
                arCol.Add("CL_VC4_Status")
                arCol.Add("CL_DT8_EventDate")
                arCol.Add("CL_DT8_DateTime")
                arCol.Add("CL_NU9_Event_Owner_FK")

                arRow.Add(CDDLEventType.CDDLGetValue)
                arRow.Add(txtSubject.Text.Trim)
                arRow.Add(txtMessage.Text.Trim)

                arRow.Add(IIf(cbEnable.Checked = True, "ENA", "DISA"))
                arRow.Add(strDate)
                'Save the date time when the record is saved or updated
                arRow.Add(Now)
                arRow.Add(CDDLUser.CDDLGetValue)

                Dim intRows As Integer
                Dim strSQL As String
                strSQL = "select * from T090011 where CL_NU9_ID_PK=" & intID
                SQL.Search("", "", strSQL, intRows)

                If strTask = "Edit" Then
                    If SQL.Update("T090011", "", "", strSQL, arCol, arRow) = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Message Updated Successfully...")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Please Try Later... ")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                        Return False
                    End If

                Else

                    arCol.Add("CL_NU9_ID_PK")
                    arCol.Add("CL_NU9_UserID_FK")
                    arCol.Add("CL_NU9_CompID_FK")

                    Dim intMax As Integer = SQL.Search("msgcalender", "saveUpdateEvent-202", "select max(CL_NU9_ID_PK) from T090011")
                    intMax = intMax + 1
                    arRow.Add(intMax)
                    arRow.Add(Session("PropUserID"))
                    arRow.Add(Session("PropCompanyID"))

                    If SQL.Save("T090011", "msgcalender", "saveUpdateEvent-208", arCol, arRow) = True Then
                        lstError.Items.Clear()
                        lstError.Items.Add("Message Saved Successfully...")
                        'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                        strTask = "Edit"
                        intID = intMax
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Please Try Later...")
                        '  ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
                        Return False
                    End If

                End If
            Else
                Return False
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Please Try Later...")
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

    Private Function ValidateEvent() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()
        If CDDLEventType.CDDLGetValue = "" Then
            lstError.Items.Add("Message Type cannot be blank...")
            shFlag = 1
        End If
        If CDDLUser.CDDLGetValue = "" Then
            lstError.Items.Add("Message To cannot be blank...")
            shFlag = 1
        End If
        If txtSubject.Text.Trim.Equals("") Then
            lstError.Items.Add("Message Subject cannot be blank...")
            shFlag = 1
        End If
        If txtMessage.Text.Trim.Equals("") Then
            lstError.Items.Add("Message cannot be blank...")
            shFlag = 1
        End If

        If shFlag = 1 Then
            'ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        'Code to change CssClass for Pager in Radgrid
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("<script>onEnd();</script>")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)
        MyBase.Render(writer)
    End Sub
End Class
