'**********************************************************************************************************
' Page                   : - Form_Entry_Details
' Purpose                : - Purpose of this screen is to get information from user regarding Requested By,                               Requested date, Project, Authorised by whom etc.
' Tables used            : - T100022, T100011, T110033, T110011, T040021, T120011
' Date		    		Author						Modification Date					Description
' 27/03/06				Jaswinder				    ----------------	        		Created
'
''*********************************************************************************************************
Imports ION.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data
Imports IONCalendar


Partial Class ChangeManagement_Form_Entry_Details
    Inherits System.Web.UI.Page



#Region "global level declaration"
    'Protected Shared mdvtable As DataView = New DataView

    Dim insertedBy As String
    Dim insertedOn As String
    Dim Null As System.DBNull

    Dim rowvalue As Integer
    Dim formName As String '= "Urgent"
    Dim taskType As String '= "Codings"
    Dim compID As String

    Shared mintID As Integer
    Shared dtTab2 As New DataTable
    Shared dtTab3 As New DataTable
    Shared dtTab4 As New DataTable
    Shared dtTab5 As New DataTable
    Dim flag As Boolean = False
    Shared flagSaveUp As Boolean = True
    Dim doneObj As Boolean = False
    Dim prevFormNo As String
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
        txtRIF7.Attributes.Add("OnKeyPress", "return maxLength('" & txtRIF7.ClientID & "','1950');")
        txtRIF8.Attributes.Add("OnKeyPress", "return maxLength('" & txtRIF7.ClientID & "','1950');")
        insertedBy = HttpContext.Current.Session("PropUserID")
        insertedOn = Now.ToShortDateString
        compID = HttpContext.Current.Session("PropCompanyID")
        'txtCSS(Me.Page)
        cpnlError.Visible = False


        imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
        imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
        imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
        imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")


        addAttributes()

        Dim hiddenImage = Request.Form("txthiddenImage")

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        If Request.Form("txthidden") = "" Then
        Else
            HttpContext.Current.Session("SFormID") = Request.Form("txthidden")
            HttpContext.Current.Session("SUser") = Request.Form("txthiddenUser")
            HttpContext.Current.Session("SRole") = Request.Form("txthiddenRole")
            HttpContext.Current.Session("SCompany") = Request.Form("txthiddenCompany")
        End If

        If Not IsPostBack Then
            formName = Request.QueryString("formName")
            formName = formName.Replace("5z_q", " ")

            txtFormName.Text = formName
            txtCompany.Text = HttpContext.Current.Session("PropCompany")
            txtUser.Text = HttpContext.Current.Session("PropUserName")

            txtCallNo.Text = Request.QueryString("cno")
            txtTaskNo.Text = Request.QueryString("tno")

            setVisibile()

            'set the form
            initForm()

            If Not fillLabels(formName) Then
                lstError.Items.Add("Server is busy please try later...")
                cpnlError.Visible = True
                cpnlError.State = CustomControls.Web.PanelState.Expanded
            End If
        Else
            formName = txtFormName.Text.Trim
        End If

        If hiddenImage <> "" Then
            Try
                Select Case hiddenImage
                    Case "Close"
                        Response.Write("<script>window.close();</script>")

                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ' Return False
                            Exit Sub
                        End If
                        'End of Security Block
                        If Not flagSaveUp Then
                            If updateData() = True Then
                                Response.Write("<script>window.close();</script>")
                            End If
                        Else
                            If saveData() = True Then
                                flagSaveUp = False
                                Response.Write("<script>window.close();</script>")
                            Else
                                mintID = 1
                            End If
                        End If

                    Case "Save"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlError.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            ' Return False
                            Exit Sub
                        End If
                        'End of Security Block
                        If Not flagSaveUp Then
                            If updateData() = True Then
                                txthiddenImage.Text = ""
                                initForm()
                                flagSaveUp = False
                                cpnlError.Visible = True
                                cpnlError.State = CustomControls.Web.PanelState.Expanded
                            End If
                        Else
                            If saveData() = True Then
                                txthiddenImage.Text = ""
                                initForm()
                                flagSaveUp = False
                                cpnlError.Visible = True
                                cpnlError.State = CustomControls.Web.PanelState.Expanded
                            Else
                                flagSaveUp = True
                            End If
                        End If
                    Case "Refresh"
                        txthiddenImage.Text = ""
                        initForm()
                End Select

                'Security Block

            Catch ex As Exception
                CreateLog("CM_Form_Entry_Details", "Load-235", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            End Try
        End If

        Dim intid As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intid = Request.QueryString("ScrID")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intid) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intid)
        End If

        'End of Security Block

        If txtCallNo.Text = -1 And txtTaskNo.Text = -1 Then
            imgOk.Enabled = False
            imgSave.Enabled = False
            imgReset.Enabled = False
            imgOk.ImageUrl = "../Images/s1ok021.gif"
            imgSave.ImageUrl = "../Images/S2Save011.gif"
            imgReset.ImageUrl = "../Images/reset_201.gif"


        End If


    End Sub


#Region "Update"
    Function updateData() As Boolean
        If deleteForm(txtFormNo.Text.Trim) Then
            If saveData(False) Then
                Return True
            End If
        Else

        End If

    End Function

    Function deleteForm(ByVal formNo As Integer, Optional ByVal flagDU As Boolean = True) As Boolean
        Dim retflag, checkFlag, flg As Boolean

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBConnection = strConnection
        '     SQL.DBTable = "T060022"
        SQL.DBTracing = False

        'If flagDU = False Then
        '    If checkFormDetails(txtFormName.Text.Trim) Then
        '        checkFlag = False
        '    Else
        '        checkFlag = True
        '    End If
        'Else
        '    checkFlag = True
        'End If

        checkFlag = True

        If checkFlag Then

            ''delete objects 
            'flg = SQL.Delete("", "", "delete from T120011 where FB_IN4_form_no=" & formNo, SQL.Transaction.ReadCommitted)

            'delete field details
            flg = SQL.Delete("Form_Entry_Details", "deleteForm", "delete from T100022 where Tb_IN4_Form_No=" & formNo, SQL.Transaction.ReadCommitted)

            'delete form

            If flg Then
                If SQL.Delete("Form_Entry_Details", "deleteForm", "delete from T100011 where FD_IN4_Form_no=" & formNo, SQL.Transaction.ReadCommitted) Then
                    retflag = True
                Else
                    retflag = False
                End If
            End If



            ' give deletion message id delete called

            If Not flagDU Then
                lstError.Items.Add("Form Deleted Successfully...")
                'cpnlError.Text = "Message"
                'cpnlError.Visible = True
                'ImgError.ImageUrl = "../images/Pok.gif"                  'Run time image change of Message panel
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
            End If


        Else
            'cpnlError.Visible = True
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            'cpnlError.Text = "Error"
            'lstError.Items.Add("Error occured while transacting with database.")
            lstError.Items.Clear()
            lstError.Items.Add("server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
        End If

        Return retflag
    End Function
#End Region

#Region "Form Labels"

    Sub initForm()
        flagSaveUp = True
        fillTabRI()
        fillTab(2)
        fillTab(3)
        fillTab(4)
        fillTab(5)
    End Sub

    Sub setVisibile()
        pnl1.Visible = False
        pnl2.Visible = False
        pnl3.Visible = False
        pnl4.Visible = False
        pnl5.Visible = False

        ' for pnl 1
        lblReqBy.Visible = False
        txtReqBy.Visible = False
        lblReqDate.Visible = False
        dtReqDate.Visible = False
        lblPro.Visible = False
        txtPro.Visible = False
        lblPriority.Visible = False
        txtPriority.Visible = False
        txtAuthBy.Visible = False
        lblAuthBy.Visible = False
        lblRIF1.Visible = False
        txtRIF1.Visible = False
        lblRIF2.Visible = False
        txtRIF2.Visible = False
        lblRIF3.Visible = False
        txtRIF3.Visible = False
        lblRIF4.Visible = False
        txtRIF4.Visible = False
        lblRIF5.Visible = False
        txtRIF5.Visible = False
        lblRIF6.Visible = False
        txtRIF6.Visible = False
        lblRIF7.Visible = False
        txtRIF7.Visible = False
        lblRIF8.Visible = False
        txtRIF8.Visible = False
        lblRIF9.Visible = False
        dtRIF9.Visible = False
        lblRIF10.Visible = False
        dtRIF10.Visible = False
    End Sub

    Function FillLabels(ByVal formName) As Boolean
        Dim sFormID As String

        sFormID = getFormNo(formName)

        Dim dsForm As New DataSet
        Dim strQuery As String = "select FB_VC50_Tab_Alias,FB_IN4_Tab_id_Parent,CA_VC50_Alias_Name,CA_IN4_Tab_Id,CA_IN4_Column_No,CA_IN4_Sequence from T110033,T110044 where FB_IN4_form_no=CA_IN4_Form_No and FB_IN4_form_no=" & sFormID

        Try
            If SQL.Search("T110033", "Form_Entry_Details", "Filllabels", strQuery, dsForm, "", "") = True Then
                Dim dtform As DataRow

                ' fill tab names

                For Each dtform In dsForm.Tables(0).Rows
                    Select Case dtform.Item("FB_IN4_Tab_id_Parent")
                        Case 1
                            pnl1.Text = dtform.Item("FB_VC50_Tab_Alias")
                            pnl1.Visible = True
                        Case 2
                            pnl2.Text = dtform.Item("FB_VC50_Tab_Alias")
                            pnl2.Visible = True
                        Case 3
                            pnl3.Text = dtform.Item("FB_VC50_Tab_Alias")
                            pnl3.Visible = True
                        Case 4
                            pnl4.Text = dtform.Item("FB_VC50_Tab_Alias")
                            pnl4.Visible = True
                        Case 5
                            pnl5.Text = dtform.Item("FB_VC50_Tab_Alias")
                            pnl5.Visible = True
                    End Select

                    'fill field names for each tab

                    Select Case dtform.Item("CA_IN4_Tab_Id")
                        Case 1
                            Select Case dtform.Item("CA_IN4_Column_No")
                                Case 1
                                    lblReqBy.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblReqBy.Visible = True
                                    txtReqBy.Visible = True
                                Case 2
                                    lblReqDate.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblReqDate.Visible = True
                                    dtReqDate.Visible = True
                                Case 3
                                    lblPro.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblPro.Visible = True
                                    txtPro.Visible = True
                                Case 4
                                    lblPriority.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblPriority.Visible = True
                                    txtPriority.Visible = True
                                Case 5
                                    lblAuthBy.Text = dtform.Item("CA_VC50_Alias_Name")
                                    txtAuthBy.Visible = True
                                    lblAuthBy.Visible = True
                                Case 6
                                    lblRIF1.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF1.Visible = True
                                    txtRIF1.Visible = True
                                Case 7
                                    lblRIF2.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF2.Visible = True
                                    txtRIF2.Visible = True
                                Case 8
                                    lblRIF3.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF3.Visible = True
                                    txtRIF3.Visible = True
                                Case 9
                                    lblRIF4.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF4.Visible = True
                                    txtRIF4.Visible = True
                                Case 10
                                    lblRIF5.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF5.Visible = True
                                    txtRIF5.Visible = True
                                Case 11
                                    lblRIF6.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF6.Visible = True
                                    txtRIF6.Visible = True
                                Case 12
                                    lblRIF7.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF7.Visible = True
                                    txtRIF7.Visible = True
                                Case 13
                                    lblRIF8.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF8.Visible = True
                                    txtRIF8.Visible = True
                                Case 14
                                    lblRIF9.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF9.Visible = True
                                    dtRIF9.Visible = True
                                Case 15
                                    lblRIF10.Text = dtform.Item("CA_VC50_Alias_Name")
                                    lblRIF10.Visible = True
                                    dtRIF10.Visible = True
                            End Select
                    End Select
                Next
            End If
            Return True

        Catch ex As Exception
            CreateLog("Form_Entry_Details", "FillLabels-485", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return True
        End Try

    End Function

    Function getFormNo(ByVal formName) As String
        Dim formNo As String
        Dim dtForm As New DataSet

        '       If SQL.Search("", "", "Select FN_IN4_form_no from T110011 where FN_VC100_Form_name='" & formName & "' and FN_IN4_Company_ID=" & Session("propCAComp"), dtForm) Then
        If SQL.Search("T110011", "Form_Entry_Details", "getFormno", "Select FN_IN4_form_no from T110011 where FN_VC100_Form_name='" & formName & "'", dtForm, "", "") Then
            formNo = dtForm.Tables(0).Rows(0).Item(0)
            Return formNo
        Else
            'cpnlError.Visible = True
            'cpnlError.State = CustomControls.Web.PanelState.Expanded
            'lstError.Items.Add("Error occured while transacting with Database")
            lstError.Items.Clear()
            lstError.Items.Add("server is busy please try later...")
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            Return ""
        End If

    End Function
#End Region

#Region "Save"

    Function saveData(Optional ByVal flag As Boolean = True) As Boolean

        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        lstError.Items.Clear()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBConnection = strConnection
        ' Table name
        '    SQL.DBTable = "T040021"
        SQL.DBTracing = False

        Try
            getValues(arColumnName, arRowData)
            mstGetFunctionValue = InsertData(arColumnName, arRowData, "T100011", flag)

            If mstGetFunctionValue.ErrorCode = 0 Then

                SQL.Update("Form_Entry_Details", "saveData", "update T040021 set TM_CH1_Forms=1 where TM_NU9_Call_No_FK=" & txtCallNo.Text.Trim & " and TM_NU9_Task_no_PK=" & txtTaskNo.Text.Trim & " and TM_NU9_Comp_ID_FK=" & Session("propCAComp"), SQL.Transaction.ReadCommitted)

                ' save tabs
                arColumnName.Clear()
                arRowData.Clear()
                getTabValues(arColumnName, arRowData)
                mstGetFunctionValue = InsertData(arColumnName, arRowData, "T100022", flag)

                'save tab2
                arColumnName.Clear()
                arRowData.Clear()
                getGridValues(arColumnName, arRowData)

                arColumnName.Clear()
                arRowData.Clear()

                If mstGetFunctionValue.ErrorCode = 0 Then

                    'lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    'cpnlError.Text = "Message"
                    'cpnlError.Visible = True
                    'ImgError.ImageUrl = "../images/Pok.gif"                  'Run time image change of Message panel
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)

                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                    Return True
                ElseIf mstGetFunctionValue.ErrorCode = 1 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    'cpnlError.Text = "Message"
                    'cpnlError.TitleCSS = "test3"
                    'ImgError.ImageUrl = "../images/warning.gif"                  'Run time image change of Message panel
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    'cpnlError.Visible = True
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                ElseIf mstGetFunctionValue.ErrorCode = 2 Then
                    lstError.Items.Add(mstGetFunctionValue.ErrorMessage)
                    'cpnlError.Text = "Message"
                    'cpnlError.TitleCSS = "test3"
                    'ImgError.ImageUrl = "../images/warning.gif"                  'Run time image change of Message panel
                    'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                    'cpnlError.Visible = True
                    ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                    Return False
                End If
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Text = "Message"
            'cpnlError.TitleCSS = "test3"
            'cpnlError.Visible = True
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
            CreateLog("CM_Form_Entry_Details", "SaveData-570", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")

            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            Return False
        End Try

    End Function

    Sub getValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        arColumnName.Clear()
        arRowData.Clear()

        Dim intNextNum As Long

        prevFormNo = txtFormNo.Text.Trim

        intNextNum = clsNextNo.GetNextNo(101, "COM", System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        txtFormNo.Text = intNextNum

        arColumnName.Add("FD_IN4_Form_no")
        arColumnName.Add("FD_IN4_Call_no")
        arColumnName.Add("FD_IN4_Task_no")
        arColumnName.Add("FD_VC50_Call_form_Name")
        arColumnName.Add("FD_IN4_Comp_id")
        arColumnName.Add("FD_VC50_RPA")
        arColumnName.Add("FD_IN4_User1")
        arColumnName.Add("FD_IN4_Inserted_By")
        arColumnName.Add("FD_IN4_Inserted_On")

        arRowData.Add(txtFormNo.Text.Trim)
        arRowData.Add(txtCallNo.Text.Trim)
        arRowData.Add(txtTaskNo.Text.Trim)
        arRowData.Add(txtFormName.Text.Trim)
        arRowData.Add(HttpContext.Current.Session("PropCAComp"))
        arRowData.Add(IIf(txtRPA.Text.Trim = "", "", txtRPA.Text.Trim))
        arRowData.Add(HttpContext.Current.Session("PropUserID"))
        arRowData.Add(CInt(insertedBy))
        arRowData.Add(insertedOn)

    End Sub

    Sub getTabValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        arColumnName.Clear()
        arRowData.Clear()

        arColumnName.Add("Tb_IN4_Tab_No")
        arColumnName.Add("Tb_IN4_Form_No")

        arColumnName.Add("Tb_VC200_Field1")
        arColumnName.Add("Tb_VC200_Field2")
        arColumnName.Add("Tb_VC200_Field3")
        arColumnName.Add("Tb_VC200_Field4")
        arColumnName.Add("Tb_VC200_Field5")
        arColumnName.Add("Tb_VC200_Field6")
        arColumnName.Add("Tb_VC200_Field7")
        arColumnName.Add("Tb_VC200_Field8")
        arColumnName.Add("Tb_VC200_Field9")
        arColumnName.Add("Tb_VC200_Field10")

        arColumnName.Add("Tb_VC2000_Field13")
        arColumnName.Add("Tb_VC2000_Field14")

        arColumnName.Add("Tb_DT8_Date1")
        arColumnName.Add("Tb_DT8_Date2")
        arColumnName.Add("Tb_DT8_Date3")

        'for RI tab (pnl 1)

        arRowData.Add(1)
        arRowData.Add(txtFormNo.Text.Trim)

        arRowData.Add(IIf(txtReqBy.Text.Trim = "", "", txtReqBy.Text.Trim))
        arRowData.Add(IIf(txtPro.Text.Trim = "", "", txtPro.Text.Trim))
        arRowData.Add(IIf(txtPriority.Text.Trim = "", "", txtPriority.Text.Trim))
        arRowData.Add(IIf(txtAuthBy.Text.Trim = "", "", txtAuthBy.Text.Trim))
        arRowData.Add(IIf(txtRIF1.Text.Trim = "", "", txtRIF1.Text.Trim))
        arRowData.Add(IIf(txtRIF2.Text.Trim = "", "", txtRIF2.Text.Trim))
        arRowData.Add(IIf(txtRIF3.Text.Trim = "", "", txtRIF3.Text.Trim))
        arRowData.Add(IIf(txtRIF4.Text.Trim = "", "", txtRIF4.Text.Trim))
        arRowData.Add(IIf(txtRIF5.Text.Trim = "", "", txtRIF5.Text.Trim))
        arRowData.Add(IIf(txtRIF6.Text.Trim = "", "", txtRIF6.Text.Trim))

        arRowData.Add(IIf(txtRIF7.Text.Trim = "", "", txtRIF7.Text.Trim))
        arRowData.Add(IIf(txtRIF8.Text.Trim = "", "", txtRIF8.Text.Trim))

        arRowData.Add(IIf(dtreqDate.Text = "", DBNull.Value, dtreqDate.Text))
        arRowData.Add(IIf(dtrIF9.Text = "", DBNull.Value, dtrIF9.Text))
        arRowData.Add(IIf(dtrIF10.Text = "", DBNull.Value, dtrIF10.Text))

    End Sub

    Sub getGridValues(ByRef arColumnName As ArrayList, ByRef arRowData As ArrayList)

        readGrid2()
        readGrid3()
        readGrid4()
        readGrid5()

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim Multi As SQL.AddMultipleRows
        SQL.DBConnection = strConnection
        'SQL.DBTable = "T100022"
        SQL.DBTracing = False

        arColumnName.Clear()
        arRowData.Clear()

        arColumnName.Add("Tb_IN4_Tab_No")
        arColumnName.Add("Tb_IN4_Form_No")

        arColumnName.Add("Tb_VC200_Field1")
        arColumnName.Add("Tb_VC200_Field2")
        arColumnName.Add("Tb_VC200_Field3")
        arColumnName.Add("Tb_VC200_Field4")
        arColumnName.Add("Tb_VC200_Field5")
        arColumnName.Add("Tb_VC200_Field6")
        arColumnName.Add("Tb_VC200_Field7")
        arColumnName.Add("Tb_VC200_Field8")
        arColumnName.Add("Tb_VC200_Field9")
        arColumnName.Add("Tb_VC200_Field10")
        arColumnName.Add("Tb_VC200_Field11")
        arColumnName.Add("Tb_VC200_Field12")

        arColumnName.Add("Tb_VC2000_Field13")
        arColumnName.Add("Tb_VC2000_Field14")
        arColumnName.Add("Tb_VC2000_Field15")

        arColumnName.Add("Tb_DT8_Date1")
        arColumnName.Add("Tb_DT8_Date2")
        arColumnName.Add("Tb_DT8_Date3")

        ' add multiple rows in the dataset

        'for tab 2

        Dim dtRow As DataRow

        Dim Cnt As Integer

        If pnl2.Visible Then
            For Cnt = 0 To dtTab2.Rows.Count - 1

                arRowData.Add(2)
                arRowData.Add(txtFormNo.Text.Trim)

                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field1"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field2"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field3"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field4"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field5"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field6"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field7"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field8"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC200_Field9"))

                arRowData.Add(DBNull.Value)
                arRowData.Add(DBNull.Value)
                arRowData.Add(DBNull.Value)

                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC2000_Field13"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC2000_Field14"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_VC2000_Field15"))

                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_DT8_Date1"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_DT8_Date2"))
                arRowData.Add(dtTab2.Rows(Cnt).Item("Tb_DT8_Date3"))
                Multi.Add("T100022", arColumnName, arRowData)

            Next

        End If

        If pnl3.Visible Then
            'for tab3

            For Each dtRow In dtTab3.Rows
                arRowData.Add(3)
                arRowData.Add(txtFormNo.Text.Trim)

                arRowData.Add(dtRow.Item("Tb_VC200_Field1"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field2"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field3"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field4"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field5"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field6"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field7"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field8"))

                arRowData.Add(DBNull.Value)
                arRowData.Add(DBNull.Value)
                arRowData.Add(DBNull.Value)
                arRowData.Add(DBNull.Value)

                arRowData.Add(dtRow.Item("Tb_VC2000_Field13"))
                arRowData.Add(dtRow.Item("Tb_VC2000_Field14"))

                arRowData.Add(DBNull.Value)

                arRowData.Add(dtRow.Item("Tb_DT8_Date1"))
                arRowData.Add(dtRow.Item("Tb_DT8_Date2"))
                arRowData.Add(dtRow.Item("Tb_DT8_Date3"))
                Multi.Add("T100022", arColumnName, arRowData)
            Next
        End If

        If pnl4.Visible Then
            'for tab4
            For Each dtRow In dtTab4.Rows
                arRowData.Add(4)
                arRowData.Add(txtFormNo.Text.Trim)

                arRowData.Add(dtRow.Item("Tb_VC200_Field1"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field2"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field3"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field4"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field5"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field6"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field7"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field8"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field9"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field10"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field11"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field12"))

                arRowData.Add(dtRow.Item("Tb_VC2000_Field13"))
                arRowData.Add(dtRow.Item("Tb_VC2000_Field14"))
                arRowData.Add(dtRow.Item("Tb_VC2000_Field15"))

                arRowData.Add(dtRow.Item("Tb_DT8_Date1"))
                arRowData.Add(dtRow.Item("Tb_DT8_Date2"))
                arRowData.Add(dtRow.Item("Tb_DT8_Date3"))
                Multi.Add("T100022", arColumnName, arRowData)
            Next

        End If

        If pnl5.Visible Then
            'for tab5

            For Each dtRow In dtTab5.Rows
                arRowData.Add(5)
                arRowData.Add(txtFormNo.Text.Trim)

                arRowData.Add(dtRow.Item("Tb_VC200_Field1"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field2"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field3"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field4"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field5"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field6"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field7"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field8"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field9"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field10"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field11"))
                arRowData.Add(dtRow.Item("Tb_VC200_Field12"))

                arRowData.Add(dtRow.Item("Tb_VC2000_Field13"))
                arRowData.Add(dtRow.Item("Tb_VC2000_Field14"))
                arRowData.Add(dtRow.Item("Tb_VC2000_Field15"))

                arRowData.Add(dtRow.Item("Tb_DT8_Date1"))
                arRowData.Add(dtRow.Item("Tb_DT8_Date2"))
                arRowData.Add(dtRow.Item("Tb_DT8_Date3"))
                Multi.Add("T100022", arColumnName, arRowData)
            Next
        End If

        ' save row in the table detail view for that view
        Multi.Save()
        Multi.Dispose()

    End Sub

    Function InsertData(ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal TableName As String, ByVal flag As Boolean) As ReturnValue
        Dim stReturn As ReturnValue

        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

            SQL.DBConnection = strConnection
            ' Table name
            'SQL.DBTable = TableName
            SQL.DBTracing = False

            If SQL.Save(TableName, "Form_Entry_Details", "InserData", ColumnName, RowData) = False Then
                If flag Then
                    stReturn.ErrorMessage = "Server is busy please try later..."
                Else
                    stReturn.ErrorMessage = "Server is busy please try later..."
                End If
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                SQL.Update("Form_Entry_Details", "InsertData", "update T120011 set OD_IN4_Proj_no=" & txtFormNo.Text.Trim & " where OD_IN4_Proj_no=" & prevFormNo, SQL.Transaction.ReadCommitted)

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
            CreateLog("Form_Entry_Details", "InsertData-881", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
            Return stReturn
        End Try
    End Function

#End Region

#Region "tab2"
    Sub fillTab(ByVal NUM As Int16)
        Dim dsForms As New DataSet

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConnection
        ' Table name
        '  SQL.DBTable = "T100011"
        SQL.DBTracing = False

        Try
            SQL.Search("T100011", "form_Entry_Details", "filltab", "select  convert(varchar,tb_dt8_date1,101)Tb_DT8_Date1,convert(varchar,tb_dt8_date2,101)Tb_DT8_Date2,convert(varchar,tb_dt8_date3,101)Tb_DT8_Date3,* from T100011,T100022 where FD_IN4_Call_no='" & txtCallNo.Text.Trim & "' and FD_IN4_Task_no='" & txtTaskNo.Text.Trim & "' and FD_VC50_Call_form_Name='" & formName & "' and Tb_IN4_Tab_No=" & NUM & " and T100011.FD_IN4_Form_no = T100022.Tb_IN4_Form_No and T100011.FD_IN4_Comp_id=" & HttpContext.Current.Session("PropCAComp"), dsForms, "", "")

            If Not dsForms.Tables("T100011").Rows.Count = 0 Then

                If Not doneObj Then
                    SQL.Update("Form_Entry_Details", "fillTab", "update T120011 set OD_IN4_Proj_no=" & dsForms.Tables("T100011").Rows(0).Item("FD_IN4_Form_no") & " where OD_IN4_Proj_no=" & txtFormNo.Text.Trim, SQL.Transaction.ReadCommitted)
                End If

                txtFormNo.Text = dsForms.Tables("T100011").Rows(0).Item("FD_IN4_Form_no")

                If Not txtFormNo.Text.Trim = "" Then
                    flagSaveUp = False
                End If

            End If





            Select Case NUM
                Case 2
                    dtTab2 = dsForms.Tables("T100011")
                    Dim htDateCols As New Hashtable
                    htDateCols.Add("Tb_DT8_Date1", 2)
                    htDateCols.Add("Tb_DT8_Date2", 2)
                    htDateCols.Add("Tb_DT8_Date3", 2)
                    SetDataTableDateFormat(dtTab2, htDateCols)
                Case 3
                    dtTab3 = dsForms.Tables("T100011")
                    Dim htDateCols As New Hashtable
                    htDateCols.Add("Tb_DT8_Date1", 2)
                    htDateCols.Add("Tb_DT8_Date2", 2)
                    htDateCols.Add("Tb_DT8_Date3", 2)
                    SetDataTableDateFormat(dtTab3, htDateCols)
                Case 4
                    dtTab4 = dsForms.Tables("T100011")
                    Dim htDateCols As New Hashtable
                    htDateCols.Add("Tb_DT8_Date1", 2)
                    htDateCols.Add("Tb_DT8_Date2", 2)
                    htDateCols.Add("Tb_DT8_Date3", 2)
                    SetDataTableDateFormat(dtTab4, htDateCols)
                Case 5
                    dtTab5 = dsForms.Tables("T100011")
                    Dim htDateCols As New Hashtable
                    htDateCols.Add("Tb_DT8_Date1", 2)
                    htDateCols.Add("Tb_DT8_Date2", 2)
                    htDateCols.Add("Tb_DT8_Date3", 2)
                    SetDataTableDateFormat(dtTab5, htDateCols)
            End Select





            If dsForms.Tables("T100011").Rows.Count = 0 Then
                CreateRow(NUM)
            ElseIf NUM = 2 Then



                grdTab2.DataSource = dtTab2
                grdTab2.DataBind()
            ElseIf NUM = 3 Then


                grdTab3.DataSource = dtTab3
                grdTab3.DataBind()
            ElseIf NUM = 4 Then
                grdTab4.DataSource = dtTab4
                grdTab4.DataBind()
            ElseIf NUM = 5 Then
                grdTab5.DataSource = dtTab5
                grdTab5.DataBind()
            End If

        Catch ex As Exception
            CreateLog("Form_Entry_Details", "FillTab-943", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Sub CreateRow(ByVal num As Integer)
        Dim drow As DataRow

        Try
            For inti As Int16 = 1 To 1

                If num = 2 Then
                    drow = dtTab2.NewRow
                    drow.Item("Tb_VC200_Field1") = ""
                    dtTab2.Rows.Add(drow)
                    grdTab2.DataSource = dtTab2
                    grdTab2.DataBind()
                ElseIf num = 3 Then
                    drow = dtTab3.NewRow
                    drow.Item("Tb_VC200_Field1") = ""
                    dtTab3.Rows.Add(drow)
                    grdTab3.DataSource = dtTab3
                    grdTab3.DataBind()
                ElseIf num = 4 Then
                    drow = dtTab4.NewRow
                    drow.Item("Tb_VC200_Field1") = ""
                    dtTab4.Rows.Add(drow)
                    grdTab4.DataSource = dtTab4
                    grdTab4.DataBind()
                ElseIf num = 5 Then
                    drow = dtTab5.NewRow
                    drow.Item("Tb_VC200_Field1") = ""
                    dtTab5.Rows.Add(drow)
                    grdTab5.DataSource = dtTab5
                    grdTab5.DataBind()
                End If
            Next
        Catch ex As Exception
            CreateLog("Form_Entry_Details", "CreateRow-981", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Sub

    Sub fillTabRI()
        Dim dsForms As New DataSet

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBConnection = strConnection
        ' Table name
        '   SQL.DBTable = "T100011"
        SQL.DBTracing = False

        ''' iif dbnull.value handle

        Try
            SQL.Search("T100011", "Form_Entry_Details", "fillTabRI", "select *,convert(varchar,Tb_DT8_Date1,101) Tb_DT8_Date1  from T100011,T100022 where FD_IN4_Call_no='" & txtCallNo.Text.Trim & "' and FD_IN4_Task_no='" & txtTaskNo.Text.Trim & "' and FD_VC50_Call_form_Name='" & formName & "' and Tb_IN4_Tab_No=1 and T100011.FD_IN4_Form_no = T100022.Tb_IN4_Form_No and T100011.FD_IN4_Comp_id=" & HttpContext.Current.Session("PropCAComp"), dsForms, "", "")

            If Not dsForms.Tables("T100011").Rows.Count = 0 Then
                txtReqBy.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field1")
                If Not IsDate(dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date1")) Then
                Else
                    dtreqDate.Text = SetDateFormat(IIf(IsDBNull(dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date1")), "", dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date1")), mdlMain.IsTime.DateOnly)
                End If

                txtPro.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field2")
                txtPriority.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field3")
                txtAuthBy.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field4")
                txtRIF1.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field5")
                txtRIF2.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field6")
                txtRIF3.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field7")
                txtRIF4.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field8")
                txtRIF5.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field9")
                txtRIF6.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC200_Field10")

                If Not IsDate(dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date2")) Then

                Else
                    dtrIF9.Text = SetDateFormat(IIf(IsDBNull(dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date2")), "", dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date2")), mdlMain.IsTime.DateOnly)
                End If

                If Not IsDate(dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date3")) Then
                Else
                    dtrIF10.Text = SetDateFormat(IIf(IsDBNull(dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date3")), "", dsForms.Tables("T100011").Rows(0).Item("Tb_DT8_Date3")), mdlMain.IsTime.DateOnly)
                End If

                txtRIF7.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC2000_Field13")
                txtRIF8.Text = dsForms.Tables("T100011").Rows(0).Item("Tb_VC2000_Field14")
                txtRPA.Text = dsForms.Tables("T100011").Rows(0).Item("FD_VC50_RPA")

                If Not doneObj Then

                    SQL.Update("Form_Entry_Details", "fillTabRI", "update T120011 set OD_IN4_Proj_no=" & dsForms.Tables("T100011").Rows(0).Item("FD_IN4_Form_no") & " where OD_IN4_Proj_no=" & txtFormNo.Text.Trim, SQL.Transaction.ReadCommitted)

                End If

                txtFormNo.Text = dsForms.Tables("T100011").Rows(0).Item("FD_IN4_Form_no")

                If Not txtFormNo.Text.Trim = "" Then
                    flagSaveUp = False
                End If

            End If

        Catch ex As Exception
            CreateLog("Form_Entry_Details", "FillTabRI-1032", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Sub

    Function getDataTable() As DataTable

        Dim dsForm As New DataSet

        Try
            Dim sFormID As String

            sFormID = getFormNo(txtFormName.Text.Trim)

            Dim strQuery As String = "select FB_VC50_Tab_Alias,FB_IN4_Tab_id_Parent,CA_VC50_Alias_Name,CA_IN4_Tab_Id,CA_IN4_Column_No,CA_IN4_Sequence from T110033,T110044 where FB_IN4_form_no=CA_IN4_Form_No and FB_IN4_form_no=" & sFormID

            SQL.Search("T110033", "Form_Entry_Details", "getDataTable-1090", strQuery, dsForm, "", "")
            Return dsForm.Tables(0)

        Catch ex As Exception
            CreateLog("Form_Entry_Details", "getDataTable-1052", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

    Private Sub grdTab2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdTab2.ItemDataBound

        Dim cnt As Integer
        Dim flag As String
        Dim rowFlag As Boolean
        rowFlag = True
        grdTab2.Columns.Clear()

        Dim strID As String
        Try
            For cnt = 0 To e.Item.Cells.Count - 1
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

                    strID = txtFormNo.Text.Trim
                    If strID = "" Then
                        strID = "-1"
                    End If
                    rowvalue = e.Item.ItemIndex + 1

                    If rowFlag Then

                        If Not grdTab2.DataKeys(e.Item.ItemIndex) Is DBNull.Value Then

                            e.Item.Cells(cnt).Attributes.Add("style", "cursor:hand")
                            e.Item.Cells(cnt).Attributes.Add("onclick", "javascript:KeyCheck('" & strID & "', '" & rowvalue & "')")
                            e.Item.Cells(cnt).Attributes.Add("onDBlclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "', '" & flag & "')")

                        End If

                        If checkObj(strID, rowvalue) Then
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Visible = True
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Attributes.Add("onclick", "javascript:KeyCheck55('" & strID & "', '" & rowvalue & "', '" & flag & "')")

                            flag = "f"
                        Else
                            CType(e.Item.FindControl("imgObj"), System.Web.UI.WebControls.Image).Visible = False
                            flag = "b"
                        End If

                    End If

                End If
                rowFlag = False
            Next

        Catch ex As Exception
            CreateLog("Form_Entry_Details", "ItemdataBound-1100", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "grdTab2")
        End Try
    End Sub

    Function checkObj(ByVal strID As String, ByVal rowvalue As String) As Boolean
        Dim formNo As String
        Dim dtForm As New DataSet

        Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString

        SQL.DBConnection = strConnection
        ' Table name
        ' SQL.DBTable = "T120011"
        SQL.DBTracing = False

        Dim strQuery As String = "Select * from T120011 where OD_IN4_Proj_no=" & strID & " and OD_IN4_Row_no=" & rowvalue & ""

        Try
            If SQL.Search("T120011", "Form_Entry_Details", "checkObj", strQuery, dtForm, "", "") Then
                If dtForm.Tables("T120011").Rows.Count > 0 Then
                    Return True
                Else : Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("Form_Entry_Details", "checkobj-1127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try

    End Function

    Sub myBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)

        'Dim dv As DataView = dtTab2
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dtTab2.Columns
        Dim dtform As DataRow
        Dim dsForm As New DataTable

        grdTab2.Columns.Clear()

        Dim strID As String

        dsForm = getDataTable()
        'mdvtable.Table = dsForm.Tables(0)

        If e.Item.ItemType = ListItemType.Header Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 2
                        Try
                            Select Case dtform.Item("CA_IN4_Column_No")
                                Case 1
                                    e.Item.Cells(1).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 2
                                    e.Item.Cells(2).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 3
                                    e.Item.Cells(3).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 4
                                    e.Item.Cells(4).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 5
                                    e.Item.Cells(5).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 6
                                    e.Item.Cells(6).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 7
                                    e.Item.Cells(7).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 8
                                    e.Item.Cells(8).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 9
                                    e.Item.Cells(9).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 10
                                    e.Item.Cells(10).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 11
                                    e.Item.Cells(11).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 12
                                    e.Item.Cells(12).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 13
                                    e.Item.Cells(13).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 14
                                    e.Item.Cells(14).Text = dtform.Item("CA_VC50_Alias_Name")
                                Case 15
                                    e.Item.Cells(15).Text = dtform.Item("CA_VC50_Alias_Name")
                            End Select
                        Catch ex As Exception
                            CreateLog("Form_Entry_Details", "myBound-1186", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                        End Try
                End Select
            Next

        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 2
                        Try
                            Select Case dtform.Item("CA_IN4_Column_No")
                                Case 1
                                    CType(e.Item.FindControl("PDF1"), TextBox).Visible = True
                                Case 2
                                    CType(e.Item.FindControl("PDF2"), TextBox).Visible = True
                                Case 3
                                    CType(e.Item.FindControl("PDF3"), TextBox).Visible = True
                                Case 4
                                    CType(e.Item.FindControl("PDD1"), Customcalendar).Visible = True
                                Case 5
                                    CType(e.Item.FindControl("PDF4"), TextBox).Visible = True
                                Case 6
                                    CType(e.Item.FindControl("PDF5"), TextBox).Visible = True
                                Case 7
                                    CType(e.Item.FindControl("PDF6"), TextBox).Visible = True
                                Case 8
                                    CType(e.Item.FindControl("PDF7"), TextBox).Visible = True
                                Case 9
                                    CType(e.Item.FindControl("PDF8"), TextBox).Visible = True
                                Case 10
                                    CType(e.Item.FindControl("PDF9"), TextBox).Visible = True
                                Case 11
                                    CType(e.Item.FindControl("PDF13"), TextBox).Visible = True
                                Case 12
                                    CType(e.Item.FindControl("PDF14"), TextBox).Visible = True
                                Case 13
                                    CType(e.Item.FindControl("PDF15"), TextBox).Visible = True
                                Case 14
                                    CType(e.Item.FindControl("PDD2"), DateSelector).Visible = True
                                Case 15
                                    CType(e.Item.FindControl("PDD3"), DateSelector).Visible = True
                            End Select
                        Catch ex As Exception
                            CreateLog("Form_Entry_Details", "myBound-1230", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
                        End Try
                End Select
            Next

        End If
    End Sub

#End Region

#Region "tab3"
    Private Sub grdTab3_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdTab3.ItemDataBound
        'Dim dv As DataView = dtTab2
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dtTab3.Columns
        Dim dtform As DataRow
        Dim dsForm As New DataTable

        grdTab3.Columns.Clear()
        dsForm = getDataTable()
        'mdvtable.Table = dsForm.Tables(0)


        If e.Item.ItemType = ListItemType.Header Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 3
                        Select Case dtform.Item("CA_IN4_Column_No")
                            Case 1
                                e.Item.Cells(0).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 2
                                e.Item.Cells(1).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 3
                                e.Item.Cells(2).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 4
                                e.Item.Cells(3).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 5
                                e.Item.Cells(4).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 6
                                e.Item.Cells(5).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 7
                                e.Item.Cells(6).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 8
                                e.Item.Cells(7).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 9
                                e.Item.Cells(8).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 10
                                e.Item.Cells(9).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 11
                                e.Item.Cells(10).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 12
                                e.Item.Cells(11).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 13
                                e.Item.Cells(12).Text = dtform.Item("CA_VC50_Alias_Name")
                        End Select
                End Select
            Next

        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 3
                        Select Case dtform.Item("CA_IN4_Column_No")
                            Case 1
                                CType(e.Item.FindControl("WF1"), TextBox).Visible = True
                            Case 2
                                CType(e.Item.FindControl("WF2"), TextBox).Visible = True
                            Case 3
                                CType(e.Item.FindControl("WF3"), TextBox).Visible = True
                            Case 4
                                CType(e.Item.FindControl("WF4"), TextBox).Visible = True
                            Case 5
                                CType(e.Item.FindControl("WF5"), TextBox).Visible = True
                            Case 6
                                CType(e.Item.FindControl("WF6"), TextBox).Visible = True
                            Case 7
                                CType(e.Item.FindControl("WF7"), TextBox).Visible = True
                            Case 8
                                CType(e.Item.FindControl("WF8"), TextBox).Visible = True

                            Case 9
                                CType(e.Item.FindControl("WF13"), TextBox).Visible = True
                            Case 10
                                CType(e.Item.FindControl("WF14"), TextBox).Visible = True

                            Case 11
                                CType(e.Item.FindControl("WD1"), DateSelector).Visible = True
                            Case 12
                                CType(e.Item.FindControl("WD2"), DateSelector).Visible = True
                            Case 13
                                CType(e.Item.FindControl("WD3"), DateSelector).Visible = True
                        End Select
                End Select
            Next

        End If

    End Sub
#End Region

#Region "tab4"

    Private Sub grdTab4_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdTab4.ItemDataBound
        'Dim dv As DataView = dtTab2
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dtTab4.Columns
        Dim dtform As DataRow
        Dim dsForm As New DataTable

        grdTab4.Columns.Clear()
        dsForm = getDataTable()
        'mdvtable.Table = dsForm.Tables(0)

        If e.Item.ItemType = ListItemType.Header Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 4
                        Select Case dtform.Item("CA_IN4_Column_No")
                            Case 1
                                e.Item.Cells(0).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 2
                                e.Item.Cells(1).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 3
                                e.Item.Cells(2).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 4
                                e.Item.Cells(3).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 5
                                e.Item.Cells(4).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 6
                                e.Item.Cells(5).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 7
                                e.Item.Cells(6).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 8
                                e.Item.Cells(7).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 9
                                e.Item.Cells(8).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 10
                                e.Item.Cells(9).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 11
                                e.Item.Cells(10).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 12
                                e.Item.Cells(11).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 13
                                e.Item.Cells(12).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 14
                                e.Item.Cells(13).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 15
                                e.Item.Cells(14).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 16
                                e.Item.Cells(15).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 17
                                e.Item.Cells(16).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 18
                                e.Item.Cells(17).Text = dtform.Item("CA_VC50_Alias_Name")
                        End Select
                End Select
            Next

        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 4
                        Select Case dtform.Item("CA_IN4_Column_No")
                            Case 1
                                CType(e.Item.FindControl("T4F1"), TextBox).Visible = True
                            Case 2
                                CType(e.Item.FindControl("T4F2"), TextBox).Visible = True
                            Case 3
                                CType(e.Item.FindControl("T4F3"), TextBox).Visible = True
                            Case 4
                                CType(e.Item.FindControl("T4F4"), TextBox).Visible = True
                            Case 5
                                CType(e.Item.FindControl("T4F5"), TextBox).Visible = True
                            Case 6
                                CType(e.Item.FindControl("T4F6"), TextBox).Visible = True
                            Case 7
                                CType(e.Item.FindControl("T4F7"), TextBox).Visible = True
                            Case 8
                                CType(e.Item.FindControl("T4F8"), TextBox).Visible = True
                            Case 9
                                CType(e.Item.FindControl("T4F9"), TextBox).Visible = True
                            Case 10
                                CType(e.Item.FindControl("T4F10"), TextBox).Visible = True
                            Case 11
                                CType(e.Item.FindControl("T4F11"), TextBox).Visible = True
                            Case 12
                                CType(e.Item.FindControl("T4F12"), TextBox).Visible = True
                            Case 13
                                CType(e.Item.FindControl("T4F13"), TextBox).Visible = True
                            Case 14
                                CType(e.Item.FindControl("T4F14"), TextBox).Visible = True
                            Case 15
                                CType(e.Item.FindControl("T4F15"), TextBox).Visible = True
                            Case 16
                                CType(e.Item.FindControl("T4D1"), DateSelector).Visible = True
                            Case 17
                                CType(e.Item.FindControl("T4D2"), DateSelector).Visible = True
                            Case 18
                                CType(e.Item.FindControl("T4D3"), DateSelector).Visible = True
                        End Select
                End Select
            Next

        End If

    End Sub
#End Region

#Region "tab5"

    Private Sub grdTab5_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdTab5.ItemDataBound
        'Dim dv As DataView = dtTab2
        Dim dcCol As DataColumn
        Dim dc As DataColumnCollection = dtTab5.Columns
        Dim dtform As DataRow
        Dim dsForm As New DataTable

        grdTab5.Columns.Clear()
        dsForm = getDataTable()
        'mdvtable.Table = dsForm.Tables(0)

        If e.Item.ItemType = ListItemType.Header Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 5
                        Select Case dtform.Item("CA_IN4_Column_No")
                            Case 1
                                e.Item.Cells(0).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 2
                                e.Item.Cells(1).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 3
                                e.Item.Cells(2).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 4
                                e.Item.Cells(3).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 5
                                e.Item.Cells(4).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 6
                                e.Item.Cells(5).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 7
                                e.Item.Cells(6).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 8
                                e.Item.Cells(7).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 9
                                e.Item.Cells(8).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 10
                                e.Item.Cells(9).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 11
                                e.Item.Cells(10).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 12
                                e.Item.Cells(11).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 13
                                e.Item.Cells(12).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 14
                                e.Item.Cells(13).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 15
                                e.Item.Cells(14).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 16
                                e.Item.Cells(15).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 17
                                e.Item.Cells(16).Text = dtform.Item("CA_VC50_Alias_Name")
                            Case 18
                                e.Item.Cells(17).Text = dtform.Item("CA_VC50_Alias_Name")
                        End Select
                End Select
            Next

        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

            For Each dtform In dsForm.Rows
                Select Case dtform.Item("CA_IN4_Tab_Id")
                    Case 5
                        Select Case dtform.Item("CA_IN4_Column_No")
                            Case 1
                                CType(e.Item.FindControl("T5F1"), TextBox).Visible = True
                            Case 2
                                CType(e.Item.FindControl("T5F2"), TextBox).Visible = True
                            Case 3
                                CType(e.Item.FindControl("T5F3"), TextBox).Visible = True
                            Case 4
                                CType(e.Item.FindControl("T5F4"), TextBox).Visible = True
                            Case 5
                                CType(e.Item.FindControl("T5F5"), TextBox).Visible = True
                            Case 6
                                CType(e.Item.FindControl("T5F6"), TextBox).Visible = True
                            Case 7
                                CType(e.Item.FindControl("T5F7"), TextBox).Visible = True
                            Case 8
                                CType(e.Item.FindControl("T5F8"), TextBox).Visible = True
                            Case 9
                                CType(e.Item.FindControl("T5F9"), TextBox).Visible = True
                            Case 10
                                CType(e.Item.FindControl("T5F10"), TextBox).Visible = True
                            Case 11
                                CType(e.Item.FindControl("T5F11"), TextBox).Visible = True
                            Case 12
                                CType(e.Item.FindControl("T5F12"), TextBox).Visible = True
                            Case 13
                                CType(e.Item.FindControl("T5F13"), TextBox).Visible = True
                            Case 14
                                CType(e.Item.FindControl("T5F14"), TextBox).Visible = True
                            Case 15
                                CType(e.Item.FindControl("T5F15"), TextBox).Visible = True
                            Case 16
                                CType(e.Item.FindControl("T5D1"), DateSelector).Visible = True
                            Case 17
                                CType(e.Item.FindControl("T5D2"), DateSelector).Visible = True
                            Case 18
                                CType(e.Item.FindControl("T5D3"), DateSelector).Visible = True
                        End Select
                End Select
            Next

        End If

    End Sub
#End Region

#Region "Add Row"
    Sub readGrid2()
        Try


            dtTab2.Rows.Clear()

            Dim gridrow As DataGridItem
            Dim drow As DataRow

            For Each gridrow In grdTab2.Items
                drow = dtTab2.NewRow

                'If CType(gridrow.FindControl("imgObj"), System.Web.UI.WebControls.Image).Visible Then

                'End If

                drow.Item("Tb_VC200_Field1") = CType(gridrow.FindControl("PDF1"), TextBox).Text
                drow.Item("Tb_VC200_Field2") = CType(gridrow.FindControl("PDF2"), TextBox).Text
                drow.Item("Tb_VC200_Field3") = CType(gridrow.FindControl("PDF3"), TextBox).Text
                drow.Item("Tb_VC200_Field4") = CType(gridrow.FindControl("PDF4"), TextBox).Text
                drow.Item("Tb_VC200_Field5") = CType(gridrow.FindControl("PDF5"), TextBox).Text
                drow.Item("Tb_VC200_Field6") = CType(gridrow.FindControl("PDF6"), TextBox).Text
                drow.Item("Tb_VC200_Field7") = CType(gridrow.FindControl("PDF7"), TextBox).Text
                drow.Item("Tb_VC200_Field8") = CType(gridrow.FindControl("PDF8"), TextBox).Text
                drow.Item("Tb_VC200_Field9") = CType(gridrow.FindControl("PDF9"), TextBox).Text
                drow.Item("Tb_VC2000_Field13") = CType(gridrow.FindControl("PDF13"), TextBox).Text
                drow.Item("Tb_VC2000_Field14") = CType(gridrow.FindControl("PDF14"), TextBox).Text
                drow.Item("Tb_VC2000_Field15") = CType(gridrow.FindControl("PDF15"), TextBox).Text


                If CType(gridrow.FindControl("PDD1"), Customcalendar).Text = "" Then
                Else
                    drow.Item("Tb_DT8_Date1") = SetDateFormat(IIf(CType(gridrow.FindControl("PDD1"), Customcalendar).Text = "", Null.Value, CType(gridrow.FindControl("PDD1"), Customcalendar).Text), mdlMain.IsTime.DateOnly)
                End If

                If CType(gridrow.FindControl("PDD2"), DateSelector).CalendarDate = "" Then
                Else
                    drow.Item("Tb_DT8_Date2") = SetDateFormat(IIf(CType(gridrow.FindControl("PDD2"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("PDD2"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
                End If

                If CType(gridrow.FindControl("PDD3"), DateSelector).CalendarDate = "" Then
                Else
                    drow.Item("Tb_DT8_Date3") = SetDateFormat(IIf(CType(gridrow.FindControl("PDD3"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("PDD3"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
                End If



                dtTab2.Rows.Add(drow)
            Next

        Catch ex As Exception
        End Try

    End Sub

    Sub readGrid3()

        dtTab3.Rows.Clear()

        Dim gridrow As DataGridItem
        Dim drow As DataRow

        For Each gridrow In grdTab3.Items
            drow = dtTab3.NewRow

            drow.Item("Tb_VC200_Field1") = CType(gridrow.FindControl("WF1"), TextBox).Text
            drow.Item("Tb_VC200_Field2") = CType(gridrow.FindControl("WF2"), TextBox).Text
            drow.Item("Tb_VC200_Field3") = CType(gridrow.FindControl("WF3"), TextBox).Text
            drow.Item("Tb_VC200_Field4") = CType(gridrow.FindControl("WF4"), TextBox).Text
            drow.Item("Tb_VC200_Field5") = CType(gridrow.FindControl("WF5"), TextBox).Text
            drow.Item("Tb_VC200_Field6") = CType(gridrow.FindControl("WF6"), TextBox).Text
            drow.Item("Tb_VC200_Field7") = CType(gridrow.FindControl("WF7"), TextBox).Text
            drow.Item("Tb_VC200_Field8") = CType(gridrow.FindControl("WF8"), TextBox).Text

            drow.Item("Tb_VC2000_Field13") = CType(gridrow.FindControl("WF13"), TextBox).Text
            drow.Item("Tb_VC2000_Field14") = CType(gridrow.FindControl("WF14"), TextBox).Text

            If CType(gridrow.FindControl("WD1"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date1") = SetDateFormat(IIf(CType(gridrow.FindControl("WD1"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("WD1"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If

            If CType(gridrow.FindControl("WD2"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date2") = SetDateFormat(IIf(CType(gridrow.FindControl("WD2"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("WD2"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If

            If CType(gridrow.FindControl("WD3"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date3") = SetDateFormat(IIf(CType(gridrow.FindControl("WD3"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("WD3"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)

            End If


            dtTab3.Rows.Add(drow)
        Next
    End Sub
    Sub readGrid4()

        dtTab4.Rows.Clear()

        Dim gridrow As DataGridItem
        Dim drow As DataRow

        For Each gridrow In grdTab4.Items
            drow = dtTab4.NewRow

            drow.Item("Tb_VC200_Field1") = CType(gridrow.FindControl("T4F1"), TextBox).Text
            drow.Item("Tb_VC200_Field2") = CType(gridrow.FindControl("T4F2"), TextBox).Text
            drow.Item("Tb_VC200_Field3") = CType(gridrow.FindControl("T4F3"), TextBox).Text
            drow.Item("Tb_VC200_Field4") = CType(gridrow.FindControl("T4F4"), TextBox).Text
            drow.Item("Tb_VC200_Field5") = CType(gridrow.FindControl("T4F5"), TextBox).Text
            drow.Item("Tb_VC200_Field6") = CType(gridrow.FindControl("T4F6"), TextBox).Text
            drow.Item("Tb_VC200_Field7") = CType(gridrow.FindControl("T4F7"), TextBox).Text
            drow.Item("Tb_VC200_Field8") = CType(gridrow.FindControl("T4F8"), TextBox).Text
            drow.Item("Tb_VC200_Field9") = CType(gridrow.FindControl("T4F9"), TextBox).Text
            drow.Item("Tb_VC200_Field10") = CType(gridrow.FindControl("T4F10"), TextBox).Text
            drow.Item("Tb_VC200_Field11") = CType(gridrow.FindControl("T4F11"), TextBox).Text
            drow.Item("Tb_VC200_Field12") = CType(gridrow.FindControl("T4F12"), TextBox).Text
            drow.Item("Tb_VC2000_Field13") = CType(gridrow.FindControl("T4F13"), TextBox).Text
            drow.Item("Tb_VC2000_Field14") = CType(gridrow.FindControl("T4F14"), TextBox).Text
            drow.Item("Tb_VC2000_Field15") = CType(gridrow.FindControl("T4F15"), TextBox).Text

            If CType(gridrow.FindControl("T4D1"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date1") = SetDateFormat(IIf(CType(gridrow.FindControl("T4D1"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("T4D1"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If

            If CType(gridrow.FindControl("T4D2"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date2") = SetDateFormat(IIf(CType(gridrow.FindControl("T4D2"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("T4D2"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If

            If CType(gridrow.FindControl("T4D3"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date3") = SetDateFormat(IIf(CType(gridrow.FindControl("T4D3"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("T4D3"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)

            End If


            dtTab4.Rows.Add(drow)
        Next
    End Sub
    Sub readGrid5()

        dtTab5.Rows.Clear()

        Dim gridrow As DataGridItem
        Dim drow As DataRow

        For Each gridrow In grdTab5.Items
            drow = dtTab5.NewRow

            drow.Item("Tb_VC200_Field1") = CType(gridrow.FindControl("T5F1"), TextBox).Text
            drow.Item("Tb_VC200_Field2") = CType(gridrow.FindControl("T5F2"), TextBox).Text
            drow.Item("Tb_VC200_Field3") = CType(gridrow.FindControl("T5F3"), TextBox).Text
            drow.Item("Tb_VC200_Field4") = CType(gridrow.FindControl("T5F4"), TextBox).Text
            drow.Item("Tb_VC200_Field5") = CType(gridrow.FindControl("T5F5"), TextBox).Text
            drow.Item("Tb_VC200_Field6") = CType(gridrow.FindControl("T5F6"), TextBox).Text
            drow.Item("Tb_VC200_Field7") = CType(gridrow.FindControl("T5F7"), TextBox).Text
            drow.Item("Tb_VC200_Field8") = CType(gridrow.FindControl("T5F8"), TextBox).Text
            drow.Item("Tb_VC200_Field9") = CType(gridrow.FindControl("T5F9"), TextBox).Text
            drow.Item("Tb_VC200_Field10") = CType(gridrow.FindControl("T5F10"), TextBox).Text
            drow.Item("Tb_VC200_Field11") = CType(gridrow.FindControl("T5F11"), TextBox).Text
            drow.Item("Tb_VC200_Field12") = CType(gridrow.FindControl("T5F12"), TextBox).Text
            drow.Item("Tb_VC2000_Field13") = CType(gridrow.FindControl("T5F13"), TextBox).Text
            drow.Item("Tb_VC2000_Field14") = CType(gridrow.FindControl("T5F14"), TextBox).Text
            drow.Item("Tb_VC2000_Field15") = CType(gridrow.FindControl("T5F15"), TextBox).Text

            If CType(gridrow.FindControl("T5D1"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date1") = SetDateFormat(IIf(CType(gridrow.FindControl("T5D1"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("T5D1"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If

            If CType(gridrow.FindControl("T5D2"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date2") = SetDateFormat(IIf(CType(gridrow.FindControl("T5D2"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("T5D2"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If

            If CType(gridrow.FindControl("T5D3"), DateSelector).CalendarDate = "" Then
            Else
                drow.Item("Tb_DT8_Date3") = SetDateFormat(IIf(CType(gridrow.FindControl("T5D3"), DateSelector).CalendarDate = "", Null.Value, CType(gridrow.FindControl("T5D3"), DateSelector).CalendarDate), mdlMain.IsTime.DateOnly)
            End If


            dtTab5.Rows.Add(drow)
        Next
    End Sub

    Private Sub AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddRow.Click
        readGrid2()
        CreateRow(2)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        readGrid3()
        CreateRow(3)
    End Sub

    Private Sub Linkbutton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton2.Click
        readGrid4()
        CreateRow(4)
    End Sub

    Private Sub Linkbutton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton3.Click
        readGrid5()
        CreateRow(5)
    End Sub

    Sub addAttributes()
        AddRow.Attributes.Add("onclick", "return SaveEdit('Add');")
        Linkbutton1.Attributes.Add("onclick", "return SaveEdit('Add');")
        Linkbutton2.Attributes.Add("onclick", "return SaveEdit('Add');")
        Linkbutton3.Attributes.Add("onclick", "return SaveEdit('Add');")
    End Sub
#End Region
End Class
