'**********************************************************************************************************
' Page                   : - Form_Entry_Head
' Purpose                : - Purpose of this screen is to make a new form. It shows the customization                                     facility to choose  form fields. 
' Tables used            : - T110011, T110044, T110033, T110022, T100011
' Date		    		Author						Modification Date					Description
' 21/03/06				Jaswinder				    ----------------	        		Created
'
''*********************************************************************************************************
Imports ION.data
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports Microsoft.Web.UI.WebControls
Imports System.Data

Partial Class ChangeManagement_Form_Entry_Head
    Inherits System.Web.UI.Page
    Dim insertedBy As String
    Dim insertedOn As String

    Dim screenID As String
    Dim compID As String
    Dim columnID As String = "777"

    Dim Tab1 As String = "Request Information"
    Dim Tab2 As String = "SubCategory Details"
    Dim Tab3 As String = "Worksheet Details"
    Dim Tab4 As String = "Tab4"
    Dim Tab5 As String = "Tab5"

    Dim rowvalue As Integer
    Shared mintID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Response.Write("<meta http-equiv=""refresh"" content=""" & str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        insertedBy = HttpContext.Current.Session("PropUserID")
        insertedOn = Date.UtcNow.AddMinutes(330)
        screenID = "259"
        compID = HttpContext.Current.Session("PropCompanyID")

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgDelete.Attributes.Add("Onclick", "return SaveEdit('Delete');")
        'imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")


        addAttributes()
        ViewState("SFormID") = Request.QueryString("SFormID")
        If Not IsPostBack Then
            Call txtCSS(Me.Page)
            mintID = Request.QueryString(("ID"))             'HttpContext.Current.Session("SAddressKey")
        End If


        'mintAddressNumber = HttpContext.Current.Session("SUserID")    ' HttpContext.Current.Session("PropUserID")
        'mstrRole = HttpContext.Current.Session("SRole")    ' HttpContext.Current.Session("PropRole")
        'mstrCompany = HttpContext.Current.Session("SCompany")    'HttpContext.Current.Session("PropCompany")

        lstError.Items.Clear()

        Dim txthiddenImage As String = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage
                    Case "Close"
                        Response.Redirect("Form_Head.aspx?ScrID=14", False)

                    Case "Logout"
                        LogoutWSS()
                    Case "Add"
                        txtFormName.ReadOnly = False

                    Case "Ok"
                        'Security Block
                        If imgOk.Enabled = False Or imgOk.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If mintID = 0 Then
                            If UpdateForm(ViewState("SFormID")) = True And txthiddenImage = "Ok" Then
                                ViewState("SFormID") = ""
                                'Response.Redirect("Form_Head.aspx?ScrID=14", False)
                            End If
                            Exit Select
                        ElseIf mintID = -1 Then
                            If saveForm() = True Then
                                ' Response.Redirect("Form_Head.aspx?ScrID=14", False)
                            End If
                        End If

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            'cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block

                        If mintID = 0 Then
                            If UpdateForm(ViewState("SFormID")) Then
                                txtFormName.ReadOnly = True
                                FillContact(ViewState("SFormID"))
                                ViewState("SFormID") = ""
                            Else

                            End If
                        ElseIf mintID = -1 Then
                            If saveForm() = True Then
                                mintID = 0
                            End If
                        End If
                    Case "Delete"

                        If deleteForm(ViewState("SFormID"), False) Then
                            ViewState("SFormID") = ""
                            Response.Redirect("Form_Head.aspx?ScrID=14", False)
                        Else
                            'cpnlError.Visible = True
                            'cpnlError.State = CustomControls.Web.PanelState.Expanded
                            'cpnlError.Text = "Error"
                            lstError.Items.Add("Form has been used, cannot be Deleted...")
                        End If
                    Case "Reset"
                        txtFormName.ReadOnly = False

                End Select
            Catch ex As Exception
                CreateLog("Form_Entry_Head", "Load-606", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            End Try
        End If


        If Not ViewState("SFormID") = "" Then
            ''cpnlError.Visible = False
            Dim sFormID As String = ViewState("SFormID")
            txtFormName.ReadOnly = True
            FillContact(sFormID)
        Else

        End If


        'Security Block
        Dim intid As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../frm_NoAccess.aspx", False)
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If

        'End of Security Block


    End Sub
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)

        'Code to change CssClass for Pager in Radgrid

        Dim sb As StringBuilder = New StringBuilder

        sb.Append("<script>onEnd();</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onend", sb.ToString, False)

        MyBase.Render(writer)

    End Sub

#Region "Save"
    Function saveForm(Optional ByVal flag As Boolean = True) As Boolean
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        lstError.Items.Clear()

        Try

            If txtFormName.Text.Trim = "" Then
                'cpnlError.Visible = True
                'cpnlError.State = CustomControls.Web.PanelState.Expanded
                lstError.Items.Add("Enter a name for the Form...")
                ShowMsgPenelNew(pnlMsg, lstError, MSG.msgError)
                Return False
            End If

            If checkForm(txtFormName.Text.Trim) Then
                'cpnlError.Visible = True
                'cpnlError.State = CustomControls.Web.PanelState.Expanded
                lstError.Items.Add("Form Name already exists...")
                ShowMsgPenelNew(pnlMsg, lstError, MSG.msgError)
                Return False
            Else
                getValuesForm(arColumnName, arRowData)
                mstGetFunctionValue = insertData(arColumnName, arRowData, "T110011", flag)

                If mstGetFunctionValue.ErrorCode = 0 Then
                    arColumnName.Clear()
                    arRowData.Clear()

                    getValuesTab(arColumnName, arRowData)

                    'save data for tab 1 reuest info

                    arColumnName.Clear()
                    arRowData.Clear()
                    getValuesColumnTab1(arColumnName, arRowData)

                    'save data for tab 2 SubCategory Details

                    arColumnName.Clear()
                    arRowData.Clear()
                    getValuesColumnTab2(arColumnName, arRowData)

                    'save data for tab 3 reuest info

                    arColumnName.Clear()
                    arRowData.Clear()
                    getValuesColumnTab3(arColumnName, arRowData)

                    'save data for tab 4 SubCategory Details

                    arColumnName.Clear()
                    arRowData.Clear()
                    getValuesColumnTab4(arColumnName, arRowData)

                    'save data for tab 5 reuest info

                    arColumnName.Clear()
                    arRowData.Clear()
                    getValuesColumnTab5(arColumnName, arRowData)

                    ' clear all values
                    arColumnName.Clear()
                    arRowData.Clear()

                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ''cpnlError.Text = "Message"
                    ''cpnlError.Visible = True
                    'ImgError.ImageUrl = "../images/Pok.gif"         'Run time image change of Message panel
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgOK)
                    Return True
                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ''cpnlError.Text = "Message"
                    ''cpnlError.TitleCSS = "test3"
                    'ImgError.ImageUrl = "../images/warning.gif"         'Run time image change of Message panel
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ''cpnlError.Visible = True
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgError)
                    Return False
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ''cpnlError.Text = "Message"
                    ''cpnlError.TitleCSS = "test3"
                    'ImgError.ImageUrl = "../images/warning.gif"         'Run time image change of Message panel
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    ''cpnlError.Visible = True
                    'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    ShowMsgPenelNew(pnlMsg, lstError, MSG.msgError)
                    Return False
                End If
            End If

        Catch ex As Exception
            lstError.Items.Add("Server is busy please try later...")
            ' 'cpnlError.Text = "Message"
            ' 'cpnlError.TitleCSS = "test3"
            ' 'cpnlError.Visible = True
            'ShowMsgPenel('cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            ShowMsgPenelNew(pnlMsg, lstError, MSG.msgError)
            CreateLog("Form_Entry_Head", "saveForm-727", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

    Function insertData(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String, ByVal flag As Boolean) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            ' Table name
            '	SQL.DBTable = TableName
            SQL.DBTracing = False

            If SQL.Save(TableName, "Form_Entry_Details", "insertData", ColumnName, RowData) = False Then

                If flag Then
                    stReturn.ErrorMessage = "Problem while Saving the records..."
                Else
                    stReturn.ErrorMessage = "Problem while Updating the records..."
                End If

                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                If flag Then
                    stReturn.ErrorMessage = "Records Saved successfully..."
                Else
                    stReturn.ErrorMessage = "Records Updated successfully..."
                End If
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
            End If

            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("Form_Entry_Head", "insertData-771", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function

#Region "Form Info"

    Function checkForm(ByVal FNo As String) As Boolean
        Try
            Dim strFormID As String = SQL.Search("Form_Entry_Details", "checkForm", "Select FN_VC100_Form_name from T110011 where FN_VC100_Form_name='" & FNo & "' and FN_IN4_Company_ID=" & Session("PropCompanyID"))

            If strFormID <> "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Form_Entry_Head", "checkform-788", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Function getFormName(ByVal FNo As String) As String
        Try
            Dim strFormID As String = SQL.Search("Form_Entry_Head", "getFormName", "Select FN_VC100_Form_name from T110011 where FN_IN4_form_no=" & FNo)

            If strFormID <> "" Then
                Return strFormID
            Else
                Return ""
            End If
        Catch ex As Exception
            CreateLog("Form_Entry_Head", "getFormName-802", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    Sub getValuesForm(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        arColumnName.Add("FN_VC100_Form_name")
        arColumnName.Add("FN_VC100_File_name")
        arColumnName.Add("FN_VC100_Inserted_By")
        arColumnName.Add("FN_VC100_Inserted_On")
        arColumnName.Add("FN_IN4_Company_ID")

        arRowData.Add(txtFormName.Text.Trim)
        arRowData.Add("Form_Entry_Head.aspx")
        arRowData.Add(insertedBy)
        arRowData.Add(insertedOn)
        arRowData.Add(HttpContext.Current.Session("PropCompanyID"))

    End Sub


    Sub getValuesTab(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        Dim formNo As String
        formNo = getFormNo()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows
        SQL.DBConnection = strConnection
        'SQL.DBTable = "T110033"
        SQL.DBTracing = False

        ' define column name

        arColumnName.Add("FB_VC50_Tab_Name")
        arColumnName.Add("FB_IN4_Screen_id")
        arColumnName.Add("FB_IN4_Comp_id")
        arColumnName.Add("FB_VC10_Status")
        arColumnName.Add("FB_VC50_Tab_Alias")
        arColumnName.Add("FB_IN4_form_no")
        arColumnName.Add("FB_IN4_Tab_id_Parent")

        ' add multiple rows in the dataset
        Dim cnt As Integer = 1

        If rblRI.SelectedValue = "Yes" Then
            arRowData.Add(Tab1)
            arRowData.Add(screenID)
            arRowData.Add(compID)
            arRowData.Add("Y")
            arRowData.Add(IIf(txtRI.Text = "", Tab1, txtRI.Text))
            arRowData.Add(formNo)
            arRowData.Add(1)
            Multi.Add("T110033", arColumnName, arRowData)

        End If
        If rblPD.SelectedValue = "Yes" Then
            arRowData.Add(Tab2)
            arRowData.Add(screenID)
            arRowData.Add(compID)
            arRowData.Add("Y")
            arRowData.Add(IIf(txtPD.Text = "", Tab2, txtPD.Text))
            arRowData.Add(formNo)
            arRowData.Add(2)
            Multi.Add("T110033", arColumnName, arRowData)

        End If
        If rblWD.SelectedValue = "Yes" Then
            arRowData.Add(Tab3)
            arRowData.Add(screenID)
            arRowData.Add(compID)
            arRowData.Add("Y")
            arRowData.Add(IIf(txtWD.Text = "", Tab3, txtWD.Text))
            arRowData.Add(formNo)
            arRowData.Add(3)
            Multi.Add("T110033", arColumnName, arRowData)

        End If
        If rblT4.SelectedValue = "Yes" Then
            arRowData.Add(Tab4)
            arRowData.Add(screenID)
            arRowData.Add(compID)
            arRowData.Add("Y")
            arRowData.Add(IIf(txtT4.Text = "", Tab4, txtT4.Text))
            arRowData.Add(formNo)
            arRowData.Add(4)
            Multi.Add("T110033", arColumnName, arRowData)

        End If
        If rblT5.SelectedValue = "Yes" Then
            arRowData.Add(Tab5)
            arRowData.Add(screenID)
            arRowData.Add(compID)
            arRowData.Add("Y")
            arRowData.Add(IIf(txtT5.Text = "", Tab5, txtT5.Text))
            arRowData.Add(formNo)
            arRowData.Add(5)
            Multi.Add("T110033", arColumnName, arRowData)

        End If
        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()
    End Sub

    Function getFormNo() As String

        Dim formNo As String
        Dim dtForm As New DataSet

        If SQL.Search("T110011", "Form_Entry_Head", "getFormNo", "Select FN_IN4_form_no from T110011 where FN_VC100_Form_name='" & txtFormName.Text.Trim & "' and FN_IN4_Company_ID=" & Session("propCompanyID"), dtForm, "", "") Then

            formNo = dtForm.Tables(0).Rows(0).Item(0)
            Return formNo
        Else
            'cpnlError.Visible = True
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            lstError.Items.Add("Server is busy please try later...")
            Return ""
        End If

    End Function
#End Region

#Region "Tab 1 Save"

    Sub getValuesColumnTab1(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        Dim formNo As String

        formNo = getFormNo()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows

        SQL.DBConnection = strConnection
        'SQL.DBTable = "T110044"
        SQL.DBTracing = False

        ' define column name

        arColumnName.Add("CA_IN4_Screen_id")
        arColumnName.Add("CA_IN4_Col_id_fk")
        arColumnName.Add("CA_IN4_Comp_id")
        arColumnName.Add("CA_VC50_Alias_Name")
        arColumnName.Add("CA_VC10_Col_Status")
        arColumnName.Add("CA_IN4_Tab_Id")
        arColumnName.Add("CA_IN4_Form_No")
        arColumnName.Add("CA_IN4_Column_No")
        arColumnName.Add("CA_IN4_Sequence")

        ' add multiple rows in the dataset
        Dim cnt As Integer = 1

        If rblRI.SelectedValue = "Yes" Then

            If rblReqBy.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtReqBy.Text = ""), lblReqBy.Text, txtReqBy.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(1)
                arRowData.Add(IIf(txtReqBySeq.Text.Trim = "", cnt, txtReqBySeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblReqDate.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtReqDate.Text = ""), lblReqDate.Text, txtReqDate.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(2)
                arRowData.Add(IIf(txtReqDateSeq.Text.Trim = "", cnt, txtReqDateSeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPro.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPro.Text = ""), lblPro.Text, txtPro.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(3)
                arRowData.Add(IIf(txtProSeq.Text.Trim = "", cnt, txtProSeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPriority.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPriority.Text = ""), lblPriority.Text, txtPriority.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(4)
                arRowData.Add(IIf(txtPrioritySeq.Text.Trim = "", cnt, txtPrioritySeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblAuthby.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtAuthBy.Text = ""), lblAuthBy.Text, txtAuthBy.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(5)
                arRowData.Add(IIf(txtAuthBySeq.Text.Trim = "", cnt, txtAuthBySeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If


            If rblRIF1.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF1.Text = ""), lblRIF1.Text, txtRIF1.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(6)
                arRowData.Add(IIf(txtRISeqF1.Text.Trim = "", cnt, txtRISeqF1.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF2.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF2.Text = ""), lblRIF2.Text, txtRIF2.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(7)
                arRowData.Add(IIf(txtRISeqF2.Text.Trim = "", cnt, txtRISeqF2.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF3.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF3.Text = ""), lblRIF3.Text, txtRIF3.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(8)
                arRowData.Add(IIf(txtRISeqF3.Text.Trim = "", cnt, txtRISeqF3.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF4.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF4.Text = ""), lblRIF4.Text, txtRIF4.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(9)
                arRowData.Add(IIf(txtRISeqF4.Text.Trim = "", cnt, txtRISeqF4.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF5.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF5.Text = ""), lblRIF5.Text, txtRIF5.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(10)
                arRowData.Add(IIf(txtRISeqF5.Text.Trim = "", cnt, txtRISeqF5.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF6.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF6.Text = ""), lblRIF6.Text, txtRIF6.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(11)
                arRowData.Add(IIf(txtRISeqF6.Text.Trim = "", cnt, txtRISeqF6.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF7.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF7.Text = ""), lblRIF7.Text, txtRIF7.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(12)
                arRowData.Add(IIf(txtRISeqF7.Text.Trim = "", cnt, txtRISeqF7.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF8.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF8.Text = ""), lblRIF8.Text, txtRIF8.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(13)
                arRowData.Add(IIf(txtRISeqF8.Text.Trim = "", cnt, txtRISeqF8.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF9.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF9.Text = ""), lblRIF9.Text, txtRIF9.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(14)
                arRowData.Add(IIf(txtRISeqF9.Text.Trim = "", cnt, txtRISeqF9.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblRIF10.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtRIF10.Text = ""), lblRIF10.Text, txtRIF10.Text))
                arRowData.Add("Y")
                arRowData.Add(1)
                arRowData.Add(formNo)
                arRowData.Add(15)
                arRowData.Add(IIf(txtRISeqF10.Text.Trim = "", cnt, txtRISeqF10.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
        End If
        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()
    End Sub

#End Region

#Region "Tab 2 Save"
    Sub getValuesColumnTab2(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        Dim formNo As String

        formNo = getFormNo()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows
        SQL.DBConnection = strConnection
        'SQL.DBTable = "T110044"
        SQL.DBTracing = False

        ' define column name

        arColumnName.Add("CA_IN4_Screen_id")
        arColumnName.Add("CA_IN4_Col_id_fk")
        arColumnName.Add("CA_IN4_Comp_id")
        arColumnName.Add("CA_VC50_Alias_Name")
        arColumnName.Add("CA_VC10_Col_Status")
        arColumnName.Add("CA_IN4_Tab_Id")
        arColumnName.Add("CA_IN4_Form_No")
        arColumnName.Add("CA_IN4_Column_No")
        arColumnName.Add("CA_IN4_Sequence")

        ' add multiple rows in the dataset
        Dim cnt As Integer = 1

        If rblPD.SelectedValue = "Yes" Then

            If rblPname.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPname.Text = ""), lblPName.Text, txtPname.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(1)
                arRowData.Add(IIf(txtPnameSeq.Text.Trim = "", cnt, txtPnameSeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPdesc.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPDesc.Text = ""), lblPDesc.Text, txtPDesc.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(2)
                arRowData.Add(IIf(txtPdescSeq.Text.Trim = "", cnt, txtPdescSeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPAppBy.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPAppBy.Text = ""), lblPAppBy.Text, txtPAppBy.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(3)
                arRowData.Add(IIf(txtPAppBySeq.Text.Trim = "", cnt, txtPAppBySeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPReqDate.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPReqDate.Text = ""), lblPReqDate.Text, txtPReqDate.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(4)
                arRowData.Add(IIf(txtPReqDateSeq.Text.Trim = "", cnt, txtPReqDateSeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPSpIns.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPSpIns.Text = ""), lblPSplIns.Text, txtPSpIns.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(5)
                arRowData.Add(IIf(txtPSpInsSeq.Text.Trim = "", cnt, txtPSpInsSeq.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If

            If rblPF1.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF1.Text = ""), lblPF1.Text, txtPF1.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(6)
                arRowData.Add(IIf(txtPSeqF1.Text.Trim = "", cnt, txtPSeqF1.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF2.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF2.Text = ""), lblPF2.Text, txtPF2.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(7)
                arRowData.Add(IIf(txtPSeqF2.Text.Trim = "", cnt, txtPSeqF2.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF3.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF3.Text = ""), lblPF3.Text, txtPF3.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(8)
                arRowData.Add(IIf(txtPSeqF3.Text.Trim = "", cnt, txtPSeqF3.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF4.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF4.Text = ""), lblPF4.Text, txtPF4.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(9)
                arRowData.Add(IIf(txtPSeqF4.Text.Trim = "", cnt, txtPSeqF4.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF5.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF5.Text = ""), lblPF5.Text, txtPF5.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(10)
                arRowData.Add(IIf(txtPSeqF5.Text.Trim = "", cnt, txtPSeqF5.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF6.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF6.Text = ""), lblPF6.Text, txtPF6.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(11)
                arRowData.Add(IIf(txtPSeqF6.Text.Trim = "", cnt, txtPSeqF6.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF7.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF7.Text = ""), lblPF7.Text, txtPF7.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(12)
                arRowData.Add(IIf(txtPSeqF7.Text.Trim = "", cnt, txtPSeqF7.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF8.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF8.Text = ""), lblPF8.Text, txtPF8.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(13)
                arRowData.Add(IIf(txtPSeqF8.Text.Trim = "", cnt, txtPSeqF8.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF9.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF9.Text = ""), lblPF9.Text, txtPF9.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(14)
                arRowData.Add(IIf(txtPSeqF9.Text.Trim = "", cnt, txtPSeqF9.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblPF10.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtPF10.Text = ""), lblPF10.Text, txtPF10.Text))
                arRowData.Add("Y")
                arRowData.Add(2)
                arRowData.Add(formNo)
                arRowData.Add(15)
                arRowData.Add(IIf(txtPSeqF10.Text.Trim = "", cnt, txtPSeqF10.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
        End If
        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()
    End Sub
#End Region

#Region "Tab 3 Save"

    Sub getValuesColumnTab3(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        Dim formNo As String

        formNo = getFormNo()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows
        SQL.DBConnection = strConnection
        'SQL.DBTable = "T110044"
        SQL.DBTracing = False

        ' define column name

        arColumnName.Add("CA_IN4_Screen_id")
        arColumnName.Add("CA_IN4_Col_id_fk")
        arColumnName.Add("CA_IN4_Comp_id")
        arColumnName.Add("CA_VC50_Alias_Name")
        arColumnName.Add("CA_VC10_Col_Status")
        arColumnName.Add("CA_IN4_Tab_Id")
        arColumnName.Add("CA_IN4_Form_No")
        arColumnName.Add("CA_IN4_Column_No")
        arColumnName.Add("CA_IN4_Sequence")

        ' add multiple rows in the dataset
        Dim cnt As Integer = 1

        If rblWD.SelectedValue = "Yes" Then

            If rblWF1.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF1.Text = ""), lblWF1.Text, txtWF1.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(1)
                arRowData.Add(IIf(txtWSEqF1.Text.Trim = "", cnt, txtWSEqF1.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF2.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF2.Text = ""), lblWF2.Text, txtWF2.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(2)
                arRowData.Add(IIf(txtWSEqF2.Text.Trim = "", cnt, txtWSEqF2.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF3.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF3.Text = ""), lblWF3.Text, txtWF3.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(3)
                arRowData.Add(IIf(txtWSEqF3.Text.Trim = "", cnt, txtWSEqF3.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF4.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF4.Text = ""), lblWF4.Text, txtWF4.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(4)
                arRowData.Add(IIf(txtWSEqF4.Text.Trim = "", cnt, txtWSEqF4.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF5.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF5.Text = ""), lblWF5.Text, txtWF5.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(5)
                arRowData.Add(IIf(txtWSEqF5.Text.Trim = "", cnt, txtWSEqF5.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF6.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF6.Text = ""), lblWF6.Text, txtWF6.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(6)
                arRowData.Add(IIf(txtWSEqF6.Text.Trim = "", cnt, txtWSEqF6.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF7.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF7.Text = ""), lblWF7.Text, txtWF7.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(7)
                arRowData.Add(IIf(txtWSEqF7.Text.Trim = "", cnt, txtWSEqF7.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF8.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF8.Text = ""), lblWF8.Text, txtWF8.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(8)
                arRowData.Add(IIf(txtWSEqF8.Text.Trim = "", cnt, txtWSEqF8.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF9.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF9.Text = ""), lblWF9.Text, txtWF9.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(9)
                arRowData.Add(IIf(txtWSEqF9.Text.Trim = "", cnt, txtWSEqF9.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF10.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF10.Text = ""), lblWF10.Text, txtWF10.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(10)
                arRowData.Add(IIf(txtWSEqF10.Text.Trim = "", cnt, txtWSEqF10.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF11.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF11.Text = ""), lblWF11.Text, txtWF11.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(11)
                arRowData.Add(IIf(txtWSEqF11.Text.Trim = "", cnt, txtWSEqF11.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF12.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF12.Text = ""), lblWF12.Text, txtWF12.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(12)
                arRowData.Add(IIf(txtWSEqF12.Text.Trim = "", cnt, txtWSEqF12.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblWF13.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtWF13.Text = ""), lblWF13.Text, txtWF13.Text))
                arRowData.Add("Y")
                arRowData.Add(3)
                arRowData.Add(formNo)
                arRowData.Add(13)
                arRowData.Add(IIf(txtWSEqF13.Text.Trim = "", cnt, txtWSEqF13.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
        End If
        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()
    End Sub
#End Region

#Region "Tab 4 Save"

    Sub getValuesColumnTab4(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)
        Dim formNo As String

        formNo = getFormNo()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows
        SQL.DBConnection = strConnection
        'SQL.DBTable = "T110044"
        SQL.DBTracing = False

        ' define column name

        arColumnName.Add("CA_IN4_Screen_id")
        arColumnName.Add("CA_IN4_Col_id_fk")
        arColumnName.Add("CA_IN4_Comp_id")
        arColumnName.Add("CA_VC50_Alias_Name")
        arColumnName.Add("CA_VC10_Col_Status")
        arColumnName.Add("CA_IN4_Tab_Id")
        arColumnName.Add("CA_IN4_Form_No")
        arColumnName.Add("CA_IN4_Column_No")
        arColumnName.Add("CA_IN4_Sequence")

        ' add multiple rows in the dataset
        Dim cnt As Integer = 1

        If rblT4.SelectedValue = "Yes" Then

            If rblT4F1.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F1.Text = ""), lblT4F1.Text, txtT4F1.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(1)
                arRowData.Add(IIf(txtT4SeqF1.Text.Trim = "", cnt, txtT4SeqF1.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F2.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F2.Text = ""), lblT4F2.Text, txtT4F2.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(2)
                arRowData.Add(IIf(txtT4SeqF2.Text.Trim = "", cnt, txtT4SeqF2.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F3.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F3.Text = ""), lblT4F3.Text, txtT4F3.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(3)
                arRowData.Add(IIf(txtT4SeqF3.Text.Trim = "", cnt, txtT4SeqF3.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F4.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F4.Text = ""), lblT4F4.Text, txtT4F4.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(4)
                arRowData.Add(IIf(txtT4SeqF4.Text.Trim = "", cnt, txtT4SeqF4.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F5.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F5.Text = ""), lblT4F5.Text, txtT4F5.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(5)
                arRowData.Add(IIf(txtT4SeqF5.Text.Trim = "", cnt, txtT4SeqF5.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F6.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F6.Text = ""), lblT4F6.Text, txtT4F6.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(6)
                arRowData.Add(IIf(txtT4SeqF6.Text.Trim = "", cnt, txtT4SeqF6.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F7.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F7.Text = ""), lblT4F7.Text, txtT4F7.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(7)
                arRowData.Add(IIf(txtT4SeqF7.Text.Trim = "", cnt, txtT4SeqF7.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F8.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F8.Text = ""), lblT4F8.Text, txtT4F8.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(8)
                arRowData.Add(IIf(txtT4SeqF8.Text.Trim = "", cnt, txtT4SeqF8.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F9.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F9.Text = ""), lblT4F9.Text, txtT4F9.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(9)
                arRowData.Add(IIf(txtT4SeqF9.Text.Trim = "", cnt, txtT4SeqF9.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F10.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F10.Text = ""), lblT4F10.Text, txtT4F10.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(10)
                arRowData.Add(IIf(txtT4SeqF10.Text.Trim = "", cnt, txtT4SeqF10.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F11.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F11.Text = ""), lblT4F11.Text, txtT4F11.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(11)
                arRowData.Add(IIf(txtT4SeqF11.Text.Trim = "", cnt, txtT4SeqF11.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F12.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F12.Text = ""), lblT4F12.Text, txtT4F12.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(12)
                arRowData.Add(IIf(txtT4SeqF12.Text.Trim = "", cnt, txtT4SeqF12.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F13.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F13.Text = ""), lblT4F13.Text, txtT4F13.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(13)
                arRowData.Add(IIf(txtT4SeqF13.Text.Trim = "", cnt, txtT4SeqF13.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F14.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F14.Text = ""), lblT4F14.Text, txtT4F14.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(14)
                arRowData.Add(IIf(txtT4SeqF14.Text.Trim = "", cnt, txtT4SeqF14.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F15.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F15.Text = ""), lblT4F15.Text, txtT4F15.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(15)
                arRowData.Add(IIf(txtT4SeqF15.Text.Trim = "", cnt, txtT4SeqF15.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F16.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F16.Text = ""), lblT4F16.Text, txtT4F16.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(16)
                arRowData.Add(IIf(txtT4SeqF16.Text.Trim = "", cnt, txtT4SeqF16.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F17.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F17.Text = ""), lblT4F17.Text, txtT4F17.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(17)
                arRowData.Add(IIf(txtT4SeqF17.Text.Trim = "", cnt, txtT4SeqF17.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT4F18.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT4F18.Text = ""), lblT4F18.Text, txtT4F18.Text))
                arRowData.Add("Y")
                arRowData.Add(4)
                arRowData.Add(formNo)
                arRowData.Add(18)
                arRowData.Add(IIf(txtT4SeqF18.Text.Trim = "", cnt, txtT4SeqF18.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
        End If
        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()
    End Sub
#End Region

#Region "Tab 5 Save"
    Sub getValuesColumnTab5(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)
        Dim formNo As String

        formNo = getFormNo()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows
        SQL.DBConnection = strConnection
        'SQL.DBTable = "T110044"
        SQL.DBTracing = False

        ' define column name

        arColumnName.Add("CA_IN4_Screen_id")
        arColumnName.Add("CA_IN4_Col_id_fk")
        arColumnName.Add("CA_IN4_Comp_id")
        arColumnName.Add("CA_VC50_Alias_Name")
        arColumnName.Add("CA_VC10_Col_Status")
        arColumnName.Add("CA_IN4_Tab_Id")
        arColumnName.Add("CA_IN4_Form_No")
        arColumnName.Add("CA_IN4_Column_No")
        arColumnName.Add("CA_IN4_Sequence")

        ' add multiple rows in the dataset
        Dim cnt As Integer = 1

        If rblT5.SelectedValue = "Yes" Then

            If rblT5F1.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F1.Text = ""), lblT5F1.Text, txtT5F1.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(1)
                arRowData.Add(IIf(txtT5SeqF1.Text.Trim = "", cnt, txtT5SeqF1.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F2.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F2.Text = ""), lblT5F2.Text, txtT5F2.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(2)
                arRowData.Add(IIf(txtT5SeqF2.Text.Trim = "", cnt, txtT5SeqF2.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F3.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F3.Text = ""), lblT5F3.Text, txtT5F3.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(3)
                arRowData.Add(IIf(txtT5SeqF3.Text.Trim = "", cnt, txtT5SeqF3.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F4.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F4.Text = ""), lblT5F4.Text, txtT5F4.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(4)
                arRowData.Add(IIf(txtT5SeqF4.Text.Trim = "", cnt, txtT5SeqF4.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F5.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F5.Text = ""), lblT5F5.Text, txtT5F5.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(5)
                arRowData.Add(IIf(txtT5SeqF5.Text.Trim = "", cnt, txtT5SeqF5.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F6.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F6.Text = ""), lblT5F6.Text, txtT5F6.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(6)
                arRowData.Add(IIf(txtT5SeqF6.Text.Trim = "", cnt, txtT5SeqF6.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F7.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F7.Text = ""), lblT5F7.Text, txtT5F7.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(7)
                arRowData.Add(IIf(txtT5SeqF7.Text.Trim = "", cnt, txtT5SeqF7.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F8.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F8.Text = ""), lblT5F8.Text, txtT5F8.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(8)
                arRowData.Add(IIf(txtT5SeqF8.Text.Trim = "", cnt, txtT5SeqF8.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F9.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F9.Text = ""), lblT5F9.Text, txtT5F9.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(9)
                arRowData.Add(IIf(txtT5SeqF9.Text.Trim = "", cnt, txtT5SeqF9.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F10.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F10.Text = ""), lblT5F10.Text, txtT5F10.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(10)
                arRowData.Add(IIf(txtT5SeqF10.Text.Trim = "", cnt, txtT5SeqF10.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F11.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F11.Text = ""), lblT5F11.Text, txtT5F11.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(11)
                arRowData.Add(IIf(txtT5SeqF11.Text.Trim = "", cnt, txtT5SeqF11.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F12.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F12.Text = ""), lblT5F12.Text, txtT5F12.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(12)
                arRowData.Add(IIf(txtT5SeqF12.Text.Trim = "", cnt, txtT5SeqF12.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F13.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F13.Text = ""), lblT5F13.Text, txtT5F13.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(13)
                arRowData.Add(IIf(txtT5SeqF13.Text.Trim = "", cnt, txtT5SeqF13.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F14.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F14.Text = ""), lblT5F14.Text, txtT5F14.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(14)
                arRowData.Add(IIf(txtT5SeqF14.Text.Trim = "", cnt, txtT5SeqF14.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F15.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F15.Text = ""), lblT5F15.Text, txtT5F15.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(15)
                arRowData.Add(IIf(txtT5SeqF15.Text.Trim = "", cnt, txtT5SeqF15.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F16.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F16.Text = ""), lblT5F16.Text, txtT5F16.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(16)
                arRowData.Add(IIf(txtT5SeqF16.Text.Trim = "", cnt, txtT5SeqF16.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F17.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F17.Text = ""), lblT5F17.Text, txtT5F17.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(17)
                arRowData.Add(IIf(txtT5SeqF17.Text.Trim = "", cnt, txtT5SeqF17.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
            If rblT5F18.SelectedValue = "Yes" Then
                arRowData.Add(screenID)
                arRowData.Add(columnID)
                arRowData.Add(compID)
                arRowData.Add(IIf((txtT5F18.Text = ""), lblT5F18.Text, txtT5F18.Text))
                arRowData.Add("Y")
                arRowData.Add(5)
                arRowData.Add(formNo)
                arRowData.Add(18)
                arRowData.Add(IIf(txtT5SeqF18.Text.Trim = "", cnt, txtT5SeqF18.Text.Trim))
                Multi.Add("T110044", arColumnName, arRowData)
                cnt += 1
            End If
        End If
        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()
    End Sub
#End Region

#End Region

#Region "Fill Details"
    Sub FillContact(ByVal sFormID As String)
        txtFormName.Text = getFormName(sFormID)
        Dim dsForm As New DataSet
        Dim strQuery As String = "select FB_VC50_Tab_Alias,FB_IN4_Tab_id_Parent,CA_VC50_Alias_Name,CA_IN4_Tab_Id,CA_IN4_Column_No,CA_IN4_Sequence from T110033,T110044 where FB_IN4_form_no=CA_IN4_Form_No and FB_IN4_form_no=" & sFormID

        SQL.Search("T110033", "Form_Entry_Head", "FillContact", strQuery, dsForm, "", "")
        Dim dtform As DataRow

        ' fill tab names

        For Each dtform In dsForm.Tables(0).Rows
            Select Case dtform.Item("FB_IN4_Tab_id_Parent")
                Case 1
                    txtRI.Text = dtform.Item("FB_VC50_Tab_Alias")
                    rblRI.SelectedValue = "Yes"
                Case 2
                    txtPD.Text = dtform.Item("FB_VC50_Tab_Alias")
                    rblPD.SelectedValue = "Yes"
                Case 3
                    txtWD.Text = dtform.Item("FB_VC50_Tab_Alias")
                    rblWD.SelectedValue = "Yes"
                Case 4
                    txtT4.Text = dtform.Item("FB_VC50_Tab_Alias")
                    rblT4.SelectedValue = "Yes"
                Case 5
                    txtT5.Text = dtform.Item("FB_VC50_Tab_Alias")
                    rblT5.SelectedValue = "Yes"
            End Select

            'fill field names for each tab

            Select Case dtform.Item("CA_IN4_Tab_Id")
                Case 1
                    Select Case dtform.Item("CA_IN4_Column_No")
                        Case 1
                            rblReqBy.SelectedValue = "Yes"
                            txtReqBy.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtReqBySeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 2
                            rblReqDate.SelectedValue = "Yes"
                            txtReqDate.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtReqDateSeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 3
                            rblPro.SelectedValue = "Yes"
                            txtPro.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtProSeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 4
                            rblPriority.SelectedValue = "Yes"
                            txtPriority.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPrioritySeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 5
                            rblAuthby.SelectedValue = "Yes"
                            txtAuthBy.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtAuthBySeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 6
                            rblRIF1.SelectedValue = "Yes"
                            txtRIF1.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF1.Text = dtform.Item("CA_IN4_Sequence")
                        Case 7
                            rblRIF2.SelectedValue = "Yes"
                            txtRIF2.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF2.Text = dtform.Item("CA_IN4_Sequence")
                        Case 8
                            rblRIF3.SelectedValue = "Yes"
                            txtRIF3.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF3.Text = dtform.Item("CA_IN4_Sequence")
                        Case 9
                            rblRIF4.SelectedValue = "Yes"
                            txtRIF4.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF4.Text = dtform.Item("CA_IN4_Sequence")
                        Case 10
                            rblRIF5.SelectedValue = "Yes"
                            txtRIF5.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF5.Text = dtform.Item("CA_IN4_Sequence")
                        Case 11
                            rblRIF6.SelectedValue = "Yes"
                            txtRIF6.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF6.Text = dtform.Item("CA_IN4_Sequence")
                        Case 12
                            rblRIF7.SelectedValue = "Yes"
                            txtRIF7.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF7.Text = dtform.Item("CA_IN4_Sequence")
                        Case 13
                            rblRIF8.SelectedValue = "Yes"
                            txtRIF8.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF8.Text = dtform.Item("CA_IN4_Sequence")
                        Case 14
                            rblRIF9.SelectedValue = "Yes"
                            txtRIF9.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF9.Text = dtform.Item("CA_IN4_Sequence")
                        Case 15
                            rblRIF10.SelectedValue = "Yes"
                            txtRIF10.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtRISeqF10.Text = dtform.Item("CA_IN4_Sequence")
                    End Select
                Case 2
                    Select Case dtform.Item("CA_IN4_Column_No")
                        Case 1
                            rblPname.SelectedValue = "Yes"
                            txtPname.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPnameSeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 2
                            rblPdesc.SelectedValue = "Yes"
                            txtPDesc.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPdescSeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 3
                            rblPAppBy.SelectedValue = "Yes"
                            txtPAppBy.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPAppBySeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 4
                            rblPReqDate.SelectedValue = "Yes"
                            txtPReqDate.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPReqDateSeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 5
                            rblPSpIns.SelectedValue = "Yes"
                            txtPSpIns.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSpInsSeq.Text = dtform.Item("CA_IN4_Sequence")
                        Case 6
                            rblPF1.SelectedValue = "Yes"
                            txtPF1.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF1.Text = dtform.Item("CA_IN4_Sequence")
                        Case 7
                            rblPF2.SelectedValue = "Yes"
                            txtPF2.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF2.Text = dtform.Item("CA_IN4_Sequence")
                        Case 8
                            rblPF3.SelectedValue = "Yes"
                            txtPF3.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF3.Text = dtform.Item("CA_IN4_Sequence")
                        Case 9
                            rblPF4.SelectedValue = "Yes"
                            txtPF4.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF4.Text = dtform.Item("CA_IN4_Sequence")
                        Case 10
                            rblPF5.SelectedValue = "Yes"
                            txtPF5.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF5.Text = dtform.Item("CA_IN4_Sequence")
                        Case 11
                            rblPF6.SelectedValue = "Yes"
                            txtPF6.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF6.Text = dtform.Item("CA_IN4_Sequence")
                        Case 12
                            rblPF7.SelectedValue = "Yes"
                            txtPF7.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF7.Text = dtform.Item("CA_IN4_Sequence")
                        Case 13
                            rblPF8.SelectedValue = "Yes"
                            txtPF8.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF8.Text = dtform.Item("CA_IN4_Sequence")
                        Case 14
                            rblPF9.SelectedValue = "Yes"
                            txtPF9.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF9.Text = dtform.Item("CA_IN4_Sequence")
                        Case 15
                            rblPF10.SelectedValue = "Yes"
                            txtPF10.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtPSeqF10.Text = dtform.Item("CA_IN4_Sequence")
                    End Select
                Case 3
                    Select Case dtform.Item("CA_IN4_Column_No")
                        Case 1
                            rblWF1.SelectedValue = "Yes"
                            txtWF1.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF1.Text = dtform.Item("CA_IN4_Sequence")
                        Case 2
                            rblWF2.SelectedValue = "Yes"
                            txtWF2.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF2.Text = dtform.Item("CA_IN4_Sequence")
                        Case 3
                            rblWF3.SelectedValue = "Yes"
                            txtWF3.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF3.Text = dtform.Item("CA_IN4_Sequence")
                        Case 4
                            rblWF4.SelectedValue = "Yes"
                            txtWF4.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF4.Text = dtform.Item("CA_IN4_Sequence")
                        Case 5
                            rblWF5.SelectedValue = "Yes"
                            txtWF5.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF5.Text = dtform.Item("CA_IN4_Sequence")
                        Case 6
                            rblWF6.SelectedValue = "Yes"
                            txtWF6.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF6.Text = dtform.Item("CA_IN4_Sequence")
                        Case 7
                            rblWF7.SelectedValue = "Yes"
                            txtWF7.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF7.Text = dtform.Item("CA_IN4_Sequence")
                        Case 8
                            rblWF8.SelectedValue = "Yes"
                            txtWF8.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF8.Text = dtform.Item("CA_IN4_Sequence")
                        Case 9
                            rblWF9.SelectedValue = "Yes"
                            txtWF9.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF9.Text = dtform.Item("CA_IN4_Sequence")
                        Case 10
                            rblWF10.SelectedValue = "Yes"
                            txtWF10.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF10.Text = dtform.Item("CA_IN4_Sequence")
                        Case 11
                            rblWF11.SelectedValue = "Yes"
                            txtWF11.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF11.Text = dtform.Item("CA_IN4_Sequence")
                        Case 12
                            rblWF12.SelectedValue = "Yes"
                            txtWF12.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF12.Text = dtform.Item("CA_IN4_Sequence")
                        Case 13
                            rblWF13.SelectedValue = "Yes"
                            txtWF13.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtWSEqF13.Text = dtform.Item("CA_IN4_Sequence")
                    End Select
                Case 4
                    Select Case dtform.Item("CA_IN4_Column_No")
                        Case 1
                            rblT4F1.SelectedValue = "Yes"
                            txtT4F1.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF1.Text = dtform.Item("CA_IN4_Sequence")
                        Case 2
                            rblT4F2.SelectedValue = "Yes"
                            txtT4F2.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF2.Text = dtform.Item("CA_IN4_Sequence")
                        Case 3
                            rblT4F3.SelectedValue = "Yes"
                            txtT4F3.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF3.Text = dtform.Item("CA_IN4_Sequence")
                        Case 4
                            rblT4F4.SelectedValue = "Yes"
                            txtT4F4.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF4.Text = dtform.Item("CA_IN4_Sequence")
                        Case 5
                            rblT4F5.SelectedValue = "Yes"
                            txtT4F5.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF5.Text = dtform.Item("CA_IN4_Sequence")
                        Case 6
                            rblT4F6.SelectedValue = "Yes"
                            txtT4F6.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF6.Text = dtform.Item("CA_IN4_Sequence")
                        Case 7
                            rblT4F7.SelectedValue = "Yes"
                            txtT4F7.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF7.Text = dtform.Item("CA_IN4_Sequence")
                        Case 8
                            rblT4F8.SelectedValue = "Yes"
                            txtT4F8.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF8.Text = dtform.Item("CA_IN4_Sequence")
                        Case 9
                            rblT4F9.SelectedValue = "Yes"
                            txtT4F9.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF9.Text = dtform.Item("CA_IN4_Sequence")
                        Case 10
                            rblT4F10.SelectedValue = "Yes"
                            txtT4F10.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF10.Text = dtform.Item("CA_IN4_Sequence")
                        Case 11
                            rblT4F11.SelectedValue = "Yes"
                            txtT4F11.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF11.Text = dtform.Item("CA_IN4_Sequence")
                        Case 12
                            rblT4F12.SelectedValue = "Yes"
                            txtT4F12.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF12.Text = dtform.Item("CA_IN4_Sequence")
                        Case 13
                            rblT4F13.SelectedValue = "Yes"
                            txtT4F13.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF13.Text = dtform.Item("CA_IN4_Sequence")
                        Case 14
                            rblT4F14.SelectedValue = "Yes"
                            txtT4F14.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF14.Text = dtform.Item("CA_IN4_Sequence")
                        Case 15
                            rblT4F15.SelectedValue = "Yes"
                            txtT4F15.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF15.Text = dtform.Item("CA_IN4_Sequence")
                        Case 16
                            rblT4F16.SelectedValue = "Yes"
                            txtT4F16.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF16.Text = dtform.Item("CA_IN4_Sequence")
                        Case 17
                            rblT4F17.SelectedValue = "Yes"
                            txtT4F17.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF17.Text = dtform.Item("CA_IN4_Sequence")
                        Case 18
                            rblT4F18.SelectedValue = "Yes"
                            txtT4F18.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT4SeqF18.Text = dtform.Item("CA_IN4_Sequence")
                    End Select
                Case 5
                    Select Case dtform.Item("CA_IN4_Column_No")
                        Case 1
                            rblT5F1.SelectedValue = "Yes"
                            txtT5F1.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF1.Text = dtform.Item("CA_IN4_Sequence")
                        Case 2
                            rblT5F2.SelectedValue = "Yes"
                            txtT5F2.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF2.Text = dtform.Item("CA_IN4_Sequence")
                        Case 3
                            rblT5F3.SelectedValue = "Yes"
                            txtT5F3.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF3.Text = dtform.Item("CA_IN4_Sequence")
                        Case 4
                            rblT5F4.SelectedValue = "Yes"
                            txtT5F4.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF4.Text = dtform.Item("CA_IN4_Sequence")
                        Case 5
                            rblT5F5.SelectedValue = "Yes"
                            txtT5F5.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF5.Text = dtform.Item("CA_IN4_Sequence")
                        Case 6
                            rblT5F6.SelectedValue = "Yes"
                            txtT5F6.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF6.Text = dtform.Item("CA_IN4_Sequence")
                        Case 7
                            rblT5F7.SelectedValue = "Yes"
                            txtT5F7.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF7.Text = dtform.Item("CA_IN4_Sequence")
                        Case 8
                            rblT5F8.SelectedValue = "Yes"
                            txtT5F8.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF8.Text = dtform.Item("CA_IN4_Sequence")
                        Case 9
                            rblT5F9.SelectedValue = "Yes"
                            txtT5F9.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF9.Text = dtform.Item("CA_IN4_Sequence")
                        Case 10
                            rblT5F10.SelectedValue = "Yes"
                            txtT5F10.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF10.Text = dtform.Item("CA_IN4_Sequence")
                        Case 11
                            rblT5F11.SelectedValue = "Yes"
                            txtT5F11.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF11.Text = dtform.Item("CA_IN4_Sequence")
                        Case 12
                            rblT5F12.SelectedValue = "Yes"
                            txtT5F12.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF12.Text = dtform.Item("CA_IN4_Sequence")
                        Case 13
                            rblT5F13.SelectedValue = "Yes"
                            txtT5F13.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF13.Text = dtform.Item("CA_IN4_Sequence")
                        Case 14
                            rblT5F14.SelectedValue = "Yes"
                            txtT5F14.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF14.Text = dtform.Item("CA_IN4_Sequence")
                        Case 15
                            rblT5F15.SelectedValue = "Yes"
                            txtT5F15.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF15.Text = dtform.Item("CA_IN4_Sequence")
                        Case 16
                            rblT5F16.SelectedValue = "Yes"
                            txtT5F16.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF16.Text = dtform.Item("CA_IN4_Sequence")
                        Case 17
                            rblT5F17.SelectedValue = "Yes"
                            txtT5F17.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF17.Text = dtform.Item("CA_IN4_Sequence")
                        Case 18
                            rblT5F18.SelectedValue = "Yes"
                            txtT5F18.Text = dtform.Item("CA_VC50_Alias_Name")
                            txtT5SeqF18.Text = dtform.Item("CA_IN4_Sequence")
                    End Select
            End Select
        Next


    End Sub
#End Region

#Region "Update"
    Function UpdateForm(ByVal formNo As Integer) As Boolean
        If deleteForm(formNo) Then
            If saveForm(False) Then
                Return True
            End If
        Else

        End If

    End Function

    Function deleteForm(ByVal formNo As Integer, Optional ByVal flagDU As Boolean = True) As Boolean

        Dim retflag, checkFlag, flg As Boolean
        Dim strFormName As String = txtFormName.Text.Trim
        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBConnection = strConnection
        'SQL.DBTable = "T060022"
        SQL.DBTracing = False

        If flagDU = False Then
            If checkFormDetails(txtFormName.Text.Trim) Then
                checkFlag = False
            Else
                checkFlag = True
            End If
        Else
            checkFlag = True
        End If

        If checkFlag Then
            'Update the Task Table To Set the Form Bit to 0
            'Based on various condtions
            Dim strUpdate As String
            strUpdate = "update T040021 set TM_CH1_Forms='0' where Ltrim(convert(varchar(16),TM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Comp_ID_FK)) in (select Ltrim(convert(varchar(16),TM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TM_NU9_Comp_ID_FK)) from T040021,T040011 where TM_NU9_Call_No_FK=CM_NU9_Call_No_PK and CM_NU9_Comp_ID_FK=TM_NU9_Comp_ID_FK and CM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and TM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and TM_CH1_Forms<>'0' and CM_VC8_Call_type + TM_VC8_Task_Type in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name='" & strFormName & "' and  FT_VC8_Call_type + FT_VC8_Task_Type not in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name<>'" & strFormName & "' and FT_IN4_Comp_ID=" & Session("PropCompanyID") & ") and FT_IN4_Comp_ID=" & Session("PropCompanyID") & "))"
            If SQL.Update("Form_Entry_Head", "DeleteForm", strUpdate, SQL.Transaction.Serializable) = True Then

            End If
            'Update the Template Task Table To Set the Form Bit to 0
            'Based on various condtions
            strUpdate = "update T050031 set TTM_CH1_Forms='0' where Ltrim(convert(varchar(16),TTM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Comp_ID_FK)) in (select Ltrim(convert(varchar(16),TTM_NU9_Call_No_FK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Task_No_PK)) + '|' + Ltrim(convert(varchar(16),TTM_NU9_Comp_ID_FK)) from T050031,T050021 where TTM_NU9_Call_No_FK=TCM_NU9_Call_No_PK and TCM_NU9_CompID_FK=TTM_NU9_Comp_ID_FK and TCM_NU9_CompID_FK=" & Session("PropCompanyID") & " and TTM_NU9_Comp_ID_FK=" & Session("PropCompanyID") & " and TTM_CH1_Forms<>'0' and TCM_VC8_Call_type + TTM_VC8_Task_Type in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name='" & strFormName & "' and  FT_VC8_Call_type + FT_VC8_Task_Type not in (select FT_VC8_Call_type+FT_VC8_Task_Type from T110022 where FT_VC100_form_name<>'" & strFormName & "' and FT_IN4_Comp_ID=" & Session("PropCompanyID") & ") and FT_IN4_Comp_ID=" & Session("PropCompanyID") & "))"
            If SQL.Update("Form_Entry_Head", "DeleteForm", strUpdate, SQL.Transaction.Serializable) = True Then

            End If
            'delete field info
            flg = SQL.Delete("Form_Entry_Head", "deleteForm", "delete from T110044 where CA_IN4_Form_No=" & formNo, SQL.Transaction.ReadCommitted)

            'delete tabs
            flg = SQL.Delete("Form_Entry_Head", "deleteForm", "delete from T110033 where FB_IN4_form_no=" & formNo, SQL.Transaction.ReadCommitted)

            'delete form
            If SQL.Delete("Form_Entry_Head", "deleteForm", "delete from T110011 where FN_IN4_form_no=" & formNo, SQL.Transaction.ReadCommitted) Then
                retflag = True
            Else
                retflag = False
            End If


            ' give deletion message id delete called

            If retflag And Not flagDU Then
                'Run time image change of Message panel
                flg = SQL.Delete("Form_Entry_Head", "deleteForm", "delete from T110022 where FT_VC100_form_name='" & txtFormName.Text.Trim & "' and FT_IN4_Comp_id=" & Session("PropCompanyID"), SQL.Transaction.ReadCommitted)
            End If


        Else

        End If


        Return retflag

    End Function

    Function checkFormDetails(ByVal formName As String) As Boolean
        Try
            Dim strFormID As Integer = SQL.Search("Form_Entry_Head", "checkFormDetails", "Select FD_IN4_Form_no from T100011 where FD_VC50_Call_form_Name='" & formName & "' and FD_IN4_Comp_id=" & Session("PropCompanyID"))

            If strFormID > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Form_Entry_Head", "checkformdetails-2594", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

#End Region

#Region "attributes"
    Sub addAttributes()
        txtReqBySeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtReqDateSeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtProSeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPrioritySeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtAuthBySeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF1.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF2.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF3.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF4.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF5.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF6.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF7.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF8.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF9.Attributes.Add("onKeyPress", "NumericOnly()")
        txtRISeqF10.Attributes.Add("onKeyPress", "NumericOnly()")
        'tab 2
        txtPnameSeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPdescSeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPAppBySeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPReqDateSeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSpInsSeq.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF1.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF2.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF3.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF4.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF5.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF6.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF7.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF8.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF9.Attributes.Add("onKeyPress", "NumericOnly()")
        txtPSeqF10.Attributes.Add("onKeyPress", "NumericOnly()")
        'tab3
        txtWSEqF1.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF2.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF3.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF4.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF5.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF6.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF7.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF8.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF9.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF10.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF11.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF12.Attributes.Add("onKeyPress", "NumericOnly()")
        txtWSEqF13.Attributes.Add("onKeyPress", "NumericOnly()")
        'tab4
        txtT4SeqF1.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF2.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF3.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF4.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF5.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF6.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF7.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF8.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF9.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF10.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF11.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF12.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF13.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF14.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF15.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF16.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF17.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT4SeqF18.Attributes.Add("onKeyPress", "NumericOnly()")
        'tab 5
        txtT5SeqF1.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF2.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF3.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF4.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF5.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF6.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF7.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF8.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF9.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF10.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF11.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF12.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF13.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF14.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF15.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF16.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF17.Attributes.Add("onKeyPress", "NumericOnly()")
        txtT5SeqF18.Attributes.Add("onKeyPress", "NumericOnly()")
    End Sub
#End Region


    Private Sub rblRIF9_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblRIF9.SelectedIndexChanged

    End Sub
End Class
