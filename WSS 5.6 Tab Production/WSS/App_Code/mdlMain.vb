Imports System.Text
Imports Microsoft.Win32
Imports ION.Net
Imports ION.Data
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.Security
Imports System.Security.Cryptography
Imports System.Drawing
Imports System.Data
Public Module mdlMain
    Public intPageFlag As Integer = 1
#Region " Structure "

    Public intI As Integer

    Public Structure structAB_UDC
        ' -- Constants to store Fixed UDC Codes
        Public Const ConstStatus = "STA"
        Public Const ConstAB_Type = "ABTY"
        Public Const ConstAddress_Type = "ADTY"
        Public Const ConstCity = "CTY"
        Public Const ConstProvince = "PROV"
        Public Const ConstCountry = "CNTY"
        Public Const ConstBR = "BREL"
        Public Const ConstEmlType = "EMLT"
        Public Const ConstPhoneType = "PHTY"
        Public Const ConstCountryCode = "CCD"
        Public Const ConstAreaCode = "ARCD"
        Public Const ConstSkillType = "SKTY"
        Public Const ProductCode = "00"
        Private intCount As Integer
    End Structure

    Public Structure ReturnValue
        Private strErrorMessage As String
        Private intErrorCode As Integer
        Private blnFunctionExecuted As Boolean
        Private objValue As Object

        Public Property ErrorMessage() As String
            Get
                Return strErrorMessage
            End Get
            Set(ByVal Value As String)
                strErrorMessage = Value
            End Set
        End Property

        Public Property FunctionExecuted() As Boolean
            Get
                Return blnFunctionExecuted
            End Get
            Set(ByVal Value As Boolean)
                blnFunctionExecuted = Value
            End Set
        End Property

        Public Property ErrorCode() As Integer
            Get
                Return intErrorCode
            End Get
            Set(ByVal Value As Integer)
                intErrorCode = Value
            End Set
        End Property

        Public Property ExtraValue() As Object
            Get
                Return objValue
            End Get
            Set(ByVal Value As Object)
                objValue = Value
            End Set
        End Property

    End Structure

    ' This structure is used to keep values of task/action owner 
    Public Structure Owners
        Dim Id As String
        Dim Name As String
    End Structure

#End Region
    Public Enum AttachLevel
        CallLevel = 1
        TaskLevel = 2
        ActionLevel = 3
    End Enum
    Public Enum TransactionLogType
        CallOpened = 1
        TaskAssigned = 2
        ActionFilled = 3
        TaskForwarded = 4
        CallStatusChanged = 13
        CallClosed = 14
        TaskClosed = 15
        TaskStatusChanged = 16
        RecordsModified = 19
    End Enum
    'Enumeration for selecting Encode or Decode Functions
    'This is used in HTMLEncodeDecode() function.
    'Author:-Harpreet Singh
    'Date:- 06/11/2006
    'Modified Date:- ----
    Public Enum Action
        Encode = 1
        Decode = 2
    End Enum
    'This enumeration will be used while calling setcommentflag function
    'Author:-Harpreet Singh
    'Create Date:-12/12/2006
    'Modified Date:- ----
    Public Enum CommentLevel
        CallLevel = 1      'When called for call comment flag
        TaskLevel = 2      'When called for Task comment flag for a particulat Call and Company
        AllTaskLevel = 3      'When called for Task comment flag for all calls and Companies
        ActionLevel = 4      'When called for action comment flag
        TemplateTaskLevel = 5      'When called for Task Template comment flag
    End Enum
    Public gintTaskNo As Integer
    Public gintActionNo As Integer
    Public gdblSize As Double
    Public mstGetFunctionValue As ReturnValue
    Public garFileID As New ArrayList
    Public garTFileID As New ArrayList
    Public garAFileID As New ArrayList
    'Public garABNo As New ArrayList
    'Public gshPageStatus As Short  '1 for sucessful save, 2 for save failed
    Private mintTaskNumber As Integer
    Private mintCallNumber As Integer
    Private mintAction As Integer
    Private mintUserID As Integer
    Private mstrCompany As String
    Private mintCompanyID As String
    Private mintCAComp As String
    Private mstrRole As String
    Private mstrUserName As String
    Public mintMonitorReportUID As Integer
    Public mstrqID As String
    Private mstrRootDir As String
    Private mstrRoleName As String
    Public mstrFrom As String
    Public mstrWSSLink As String
    Public mintflagIPTrack As Integer
    Public mintSecPerm As Integer
    Public Enum StatusType
        CallStatus = 0
        TaskStatus = 1
        TemplateCallStatus = 2
        TemplateTaskStatus = 4
    End Enum
    Public Sub SetFocus(ByVal control As Control)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "<script language='JavaScript'>" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("<!--" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("function SetFocus()" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("{" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("" & Microsoft.VisualBasic.Chr(9) & "document.")
        Dim p As Control = control.Parent
        While Not (TypeOf p Is System.Web.UI.HtmlControls.HtmlForm)
            p = p.Parent
        End While
        sb.Append(p.ClientID)
        sb.Append("['")
        sb.Append(control.UniqueID)
        sb.Append("'].focus();" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("}" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("window.onload = SetFocus;" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("// -->" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "")
        sb.Append("</script>")
        control.Page.RegisterClientScriptBlock("SetFocus", sb.ToString)
    End Sub
    Public Function GetIP() As String
        Dim obj As HttpContext
        obj = HttpContext.Current
        Return obj.Request.UserHostName()
    End Function
    Public Function CreateTextBox(ByVal WebPage As Page, ByVal PostBack As Boolean, ByVal PanelControl As Panel, ByVal GridControl As DataGrid, ByVal ColumnName As ArrayList, ByVal TextBoxID As ArrayList, ByVal TextBoxValue As ArrayList) As TextBox()
        Dim txtOnGrid As TextBox()
        Dim txtGrid As TextBox
        Dim untWidth As Unit
        Dim strWidth As String
        PanelControl.Width = GridControl.Width
        ReDim txtOnGrid(ColumnName.Count)
        Try
            For intii As Integer = 0 To ColumnName.Count - 1
                txtGrid = New TextBox

                If PostBack = False Then
                    untWidth = GridControl.Columns(intii + 1).ItemStyle.Width
                    strWidth = untWidth.Value + 2
                    strWidth = strWidth & "pt"
                    TextBoxID.Add(ColumnName.Item(intii))
                    PanelControl.Controls.Add(WebPage.ParseControl("<asp:TextBox id=" & ColumnName.Item(intii) & " runat=""server"" Width=" & strWidth & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    txtGrid.Text = ""
                    txtOnGrid(intii) = txtGrid
                Else
                    untWidth = GridControl.Columns(intii + 1).ItemStyle.Width
                    strWidth = untWidth.Value + 2
                    strWidth = strWidth & "pt"
                    txtGrid.Text = TextBoxValue.Item(intii)
                    PanelControl.Controls.Add(WebPage.ParseControl("<asp:TextBox id=" & TextBoxID.Item(intii) & " runat=""server"" Width=" & strWidth & " CssClass=SearchTxtBox MaxLength=""15""></asp:TextBox>"))
                    txtGrid.ID = TextBoxID.Item(intii)
                    txtOnGrid(intii) = txtGrid
                End If
            Next

        Catch ex As Exception
            CreateLog("mdlMain", "CreateTextBox-608", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        Return txtOnGrid
    End Function
    Public Function CheckUDCValue(ByVal ProductCode As Integer, ByVal UDCTType As String, ByVal Name As String) As Boolean
        Try
            Sql.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strName As String = SQL.Search("mdlMain", "CheckUDCValue", "select Name from UDC where ProductCode=" & ProductCode & " and UDCType ='" & UDCTType & "' and Name='" & Name & "'")

            If IsNothing(strName) = True Then
                Return False
            ElseIf strName.Trim.ToUpper.Equals(Name.Trim.ToUpper.Trim) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "CheckUDCValue-608", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
    Public Function SaveAttachmentVersion(ByVal AttachmentPath As String, _
    ByVal AttachmentSize As Double, _
    ByVal AttachmentName As String, _
    ByVal AttachmentLevel As AttachLevel, _
    ByVal VersionNo As Double, _
    ByVal Update As Boolean, _
    ByVal CallNo As Integer, _
    ByVal TaskNo As Integer, _
    ByVal CompanyID As Integer, _
    ByVal ActionNo As Integer) As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim intFileID As Integer = SQL.Search("mdlMain", "SaveAttachmentVersion", "Select max(VH_NU9_File_ID_PK) from T040051")
            intFileID += 1
            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            Dim dtCreatedate As Date

            If Update = True Then
                dtCreatedate = SQL.Search("mdlMain", "SaveAttachmentVersion", "select  MIN(VH_DT8_Date) from T040051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNo & " and VH_NU9_Action_Number=" & ActionNo & " and VH_VC255_File_Name='" & AttachmentName.Trim & "' ")
                arColumnName.Add("VH_DT8_Date")
                arRowData.Add(dtCreatedate)
            Else
                arColumnName.Add("VH_DT8_Date")
                arRowData.Add(Now)
            End If

            arColumnName.Add("VH_NU9_File_ID_PK")
            arColumnName.Add("VH_VC255_File_Name")
            arColumnName.Add("VH_VC255_File_Size")
            arColumnName.Add("VH_VC255_File_Path")
            arColumnName.Add("VH_VC1_Status")
            arColumnName.Add("VH_NU9_Address_Book_Number")
            arColumnName.Add("VH_NU9_Call_Number")
            arColumnName.Add("VH_NU9_Task_Number")
            arColumnName.Add("VH_NU9_Action_Number")
            arColumnName.Add("VH_NU9_CompId_Fk")
            'arColumnName.Add("VH_DT8_Date")
            arColumnName.Add("VH_VC8_Role")
            arColumnName.Add("VH_NU9_Version")
            arColumnName.Add("VH_DT8_Modify_Date")
            arColumnName.Add("VH_IN4_Level")
            arColumnName.Add("VH_VC4_Active_Status")


            arRowData.Add(intFileID)
            arRowData.Add(AttachmentName.Trim)
            arRowData.Add(AttachmentSize)
            arRowData.Add(AttachmentPath.Trim)
            arRowData.Add("U")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            'arRowData.Add(11)
            arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(CompanyID)
            'arRowData.Add("ADM")
            'arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropRole").Trim)
            'arRowData.Add("ION")
            arRowData.Add(VersionNo)
            arRowData.Add(Now)

            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            arRowData.Add("ENB")

            If SQL.Save("T040051", "mdlMain", "SaveAttachmentVersion", arColumnName, arRowData) = True Then
                ' Update Attachment Flag in database
                Select Case AttachmentLevel
                    Case AttachLevel.CallLevel
                        WSSUpdate.UpdateForAttachment(CallNo, TaskNo, ActionNo, CompanyID, AttachLevel.CallLevel)
                    Case AttachLevel.TaskLevel
                        WSSUpdate.UpdateForAttachment(CallNo, TaskNo, ActionNo, CompanyID, AttachLevel.TaskLevel)
                    Case AttachLevel.ActionLevel
                        WSSUpdate.UpdateForAttachment(CallNo, TaskNo, ActionNo, CompanyID, AttachLevel.ActionLevel)
                End Select

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("mdlMain", "SaveAttachmentVersion-608", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        End Try

    End Function

    Public Function SaveAttachment(ByVal FileID As Integer, _
    ByVal FilePath As String, _
    ByVal FileName As String, _
    ByVal CallNo As Integer, _
    ByVal TaskNo As Integer, _
    ByVal ActionNo As Integer, _
    ByVal AttachmentSize As Integer, _
    ByVal VersionNo As Integer, _
    ByVal CompanyID As Integer, _
    ByVal AttachmentLevel As AttachLevel) As Boolean

        'Save the location of the file in the database
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            arColumnName.Add("AT_NU9_File_ID_PK")
            arColumnName.Add("AT_VC255_File_Name")
            arColumnName.Add("AT_VC255_File_Size")
            arColumnName.Add("AT_VC255_File_Path")
            arColumnName.Add("AT_VC1_Status")
            arColumnName.Add("AT_NU9_Address_Book_Number")
            arColumnName.Add("AT_NU9_Call_Number")
            arColumnName.Add("AT_NU9_Task_Number")
            arColumnName.Add("AT_NU9_Action_Number")
            arColumnName.Add("AT_NU9_CompId_Fk")
            'arColumnName.Add("AT_DT8_Date")
            arColumnName.Add("AT_VC8_Role")
            arColumnName.Add("AT_NU9_Version")
            arColumnName.Add("AT_DT8_Modify_Date")
            arColumnName.Add("AT_IN4_Level")
            arColumnName.Add("VH_VC4_Active_Status")


            arRowData.Add(FileID)
            arRowData.Add(FileName)
            arRowData.Add(AttachmentSize)
            arRowData.Add(FilePath.Trim)
            arRowData.Add("U")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            'arRowData.Add(52)
            arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(CompanyID)
            'arRowData.Add("ADM")
            ''arRowData.Add(Now.ToShortDateString)
            arRowData.Add(HttpContext.Current.Session("PropRole"))
            'arRowData.Add("ION")
            arRowData.Add(VersionNo)
            arRowData.Add(Now)

            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            arRowData.Add("ENB")


            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T040041"
            SQL.DBTracing = False

            SQL.Update("T040041", "mdlMain", "SaveAttachmentVersion", "select * from T040041 where AT_NU9_File_ID_PK=" & FileID & "", arColumnName, arRowData)
            Return True
        Catch ex As Exception
            CreateLog("mdlMain", "SaveAttachment-608", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Function
    Public Function UpdateAttachment(ByVal FilePath As String, _
    ByVal FileName As String, _
    ByVal CallNo As Integer, _
    ByVal TaskNo As Integer, _
    ByVal ActionNo As Integer, _
    ByVal AttachmentSize As Integer, _
    ByVal VersionNo As Integer, _
    ByVal CompanyID As Integer, _
    ByVal AttachmentLevel As AttachLevel) As Boolean

        'Save the location of the file in the database
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            'arColumnName.Add("AT_NU9_File_ID_PK")
            arColumnName.Add("AT_VC255_File_Name")
            arColumnName.Add("AT_VC255_File_Size")
            arColumnName.Add("AT_VC255_File_Path")
            arColumnName.Add("AT_VC1_Status")
            arColumnName.Add("AT_NU9_Address_Book_Number")
            'arColumnName.Add("AT_NU9_Call_Number")
            arColumnName.Add("AT_NU9_Task_Number")
            arColumnName.Add("AT_NU9_Action_Number")
            arColumnName.Add("AT_NU9_CompId_Fk")
            'arColumnName.Add("AT_DT8_Date")
            arColumnName.Add("AT_VC8_Role")
            arColumnName.Add("AT_NU9_Version")
            arColumnName.Add("AT_DT8_Modify_Date")
            arColumnName.Add("AT_IN4_Level")
            arColumnName.Add("VH_VC4_Active_Status")


            'arRowData.Add(FileID)
            arRowData.Add(FileName)
            arRowData.Add(AttachmentSize)
            arRowData.Add(FilePath.Trim)
            arRowData.Add("U")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            'arRowData.Add(52)
            'arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(CompanyID)
            'arRowData.Add("ADM")
            ''arRowData.Add(Now.ToShortDateString)
            arRowData.Add(HttpContext.Current.Session("PropRole"))
            'arRowData.Add("ION")
            arRowData.Add(VersionNo)
            arRowData.Add(Now)

            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            arRowData.Add("ENB")

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T040041"
            SQL.DBTracing = False

            SQL.Update("T040041", "mdlMain", "SaveAttachmentVersion", "select * from T040041 where AT_NU9_Call_Number=" & CallNo & " and AT_VC255_File_Name='" & FileName.Trim & "' and AT_VC1_Status='U'", arColumnName, arRowData)

            SQL.Delete("mdlMain", "SaveAttachmentVersion", "delete from T040041 where AT_NU9_Call_Number=" & CallNo & " and AT_VC255_File_Name='" & FileName.Trim & "' and AT_VC1_Status='T'", SQL.Transaction.Serializable, "T040041")

            Return True
        Catch ex As Exception
            CreateLog("mdlMain", "UpdateAttachment-608", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return False
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Function
    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
        HttpContext.Current.Session("PropCallNumber") = 0
        HttpContext.Current.Session("PropTaskNumber") = 0
        HttpContext.Current.Session.Clear()
    End Sub
    '*******************************************************************
    ' Page                   : -Function txtCSS
    ' Purpose              : -function for applying stylesheets on textboxes 
    ' Date		    			Author						Modification Date					Description
    ' 10/03/06				Harpreet 					----------------     					Created
    '
    ' Notes:  This function accepts some parameters and  returns the complete mail message.
    ' Code:
    '*******************************************************************
    Public Sub txtCSS(ByVal pageObj As Page, Optional ByVal strCPNL1 As String = "", Optional ByVal strCPNL2 As String = "", Optional ByVal strCPNL3 As String = "")
        Dim pnlID As String
        Dim txt As New TextBox

        Dim pnlCtrl As Control
        Dim txtCtrl As Control
        For Each ctrl As Control In pageObj.FindControl("Form1").Controls
            If TypeOf ctrl Is TextBox Then
                txt = CType(ctrl, TextBox)
                txt.Attributes.Add("onfocus", "FOCUS('" & txt.ID & "');")
                txt.Attributes.Add("onblur", "NOFOCUS('" & txt.ID & "');")
            End If
            SetControlStyle(ctrl, ctrl)
            If TypeOf ctrl Is System.Web.UI.UserControl Then
                For Each txtCtrl In ctrl.Controls
                    If TypeOf txtCtrl Is TextBox Then
                        txt = CType(txtCtrl, TextBox)
                        txt.Attributes.Add("onfocus", "FOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                        txt.Attributes.Add("onblur", "NOFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                    End If
                    SetControlStyle(txtCtrl, ctrl)
                Next
            End If
            'Find Collapsible Panel
            If TypeOf ctrl Is CustomControls.Web.CollapsiblePanel Then
                'Find Controls in Collapsible Panel
                For Each pnlCtrl In ctrl.Controls
                    'if there is dotnet panel within Collapsible Panel(generally used in fast entry)
                    If TypeOf pnlCtrl Is System.Web.UI.WebControls.Panel Then


                        For Each txtCtrl In pnlCtrl.Controls

                            If TypeOf txtCtrl Is TextBox Then
                                txt = CType(txtCtrl, TextBox)
                                If ctrl.ID = strCPNL1 Or ctrl.ID = strCPNL2 Or ctrl.ID = strCPNL3 Then
                                    txt.Attributes.Add("onfocus", "FEFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                                    txt.Attributes.Add("onblur", "FENOFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                                Else
                                    txt.Attributes.Add("onfocus", "FOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                                    txt.Attributes.Add("onblur", "NOFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                                End If
                            End If
                            SetControlStyle(txtCtrl, ctrl, strCPNL1, strCPNL2, strCPNL3)
                            If TypeOf txtCtrl Is System.Web.UI.UserControl Then

                                For Each pnlCtrlUC As Control In txtCtrl.Controls
                                    If TypeOf pnlCtrlUC Is System.Web.UI.WebControls.Panel Then

                                        For Each txtUCtrl As Control In pnlCtrlUC.Controls
                                            If TypeOf txtUCtrl Is TextBox Then
                                                txt = CType(txtUCtrl, TextBox)
                                                If ctrl.ID = strCPNL1 Or ctrl.ID = strCPNL2 Or ctrl.ID = strCPNL3 Then
                                                    txt.Attributes.Add("onfocus", "FEFOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                                    txt.CssClass = "txtNoFocusFE"
                                                    txt.Attributes.Add("onblur", "FENOFOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                                Else
                                                    txt.Attributes.Add("onfocus", "FOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                                    txt.Attributes.Add("onblur", "NOFOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                                End If
                                            End If
                                            SetControlStyle(txtUCtrl, ctrl, strCPNL1, strCPNL2, strCPNL3)
                                        Next

                                    End If

                                Next
                            End If

                        Next
                        'find other controls in Collapsible Panel
                    Else
                        txtCtrl = pnlCtrl
                        If TypeOf txtCtrl Is TextBox Then
                            txt = CType(txtCtrl, TextBox)
                            If ctrl.ID = strCPNL1 Or ctrl.ID = strCPNL2 Or ctrl.ID = strCPNL3 Then
                                txt.Attributes.Add("onfocus", "FEFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                                txt.Attributes.Add("onblur", "FENOFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                            Else
                                txt.Attributes.Add("onfocus", "FOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                                txt.Attributes.Add("onblur", "NOFOCUS('" & ctrl.ID & "_" & txt.ID & "');")
                            End If
                        End If
                        SetControlStyle(txtCtrl, ctrl, strCPNL1, strCPNL2, strCPNL3)

                        If TypeOf txtCtrl Is System.Web.UI.UserControl Then

                            For Each pnlCtrlUC1 As Control In txtCtrl.Controls
                                If TypeOf pnlCtrlUC1 Is System.Web.UI.WebControls.Panel Then

                                    For Each txtUCtrl As Control In pnlCtrlUC1.Controls
                                        If TypeOf txtUCtrl Is TextBox Then
                                            txt = CType(txtUCtrl, TextBox)
                                            If ctrl.ID = strCPNL1 Or ctrl.ID = strCPNL2 Or ctrl.ID = strCPNL3 Then
                                                txt.Attributes.Add("onfocus", "FEFOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                                txt.CssClass = "txtNoFocusFE"
                                                txt.Attributes.Add("onblur", "FENOFOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                            Else
                                                txt.Attributes.Add("onfocus", "FOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                                txt.Attributes.Add("onblur", "NOFOCUS('" & ctrl.ID & "_" & txtCtrl.ID & "_" & txt.ID & "');")
                                            End If
                                        End If
                                        SetControlStyle(txtUCtrl, ctrl, strCPNL1, strCPNL2, strCPNL3)
                                    Next
                                End If
                            Next
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub SetControlStyle(ByVal ctrl As Control, ByVal ctrlP As Control, Optional ByVal strCPNL1 As String = "", Optional ByVal strCPNL2 As String = "", Optional ByVal strCPNL3 As String = "")
        Dim lbl As New Label
        Dim rblst As New RadioButtonList
        Dim chk As New CheckBox
        Dim rbtn As New RadioButton
        Dim cblst As New CheckBoxList
        Dim ddl As New DropDownList
        If TypeOf ctrl Is Label Then
            lbl = CType(ctrl, Label)
            lbl.Font.Name = ""
            '  lbl.Font.Bold = False
            lbl.Font.Size = FontUnit.Empty
            lbl.ForeColor = Color.Empty
            lbl.EnableViewState = False
            If lbl.ID.Length >= 13 Then
                If lbl.ID.Substring(0, 13) = "lblTitleLabel" Then
                    lbl.CssClass = "TitleLabel"
                Else
                    lbl.CssClass = "FieldLabel"
                End If
            Else
                lbl.CssClass = "FieldLabel"
            End If
        End If
        If TypeOf ctrl Is RadioButtonList Then
            rblst = CType(ctrl, RadioButtonList)
            rblst.Font.Name = ""
            rblst.Font.Bold = False
            rblst.Font.Size = FontUnit.Empty
            rblst.ForeColor = Color.Empty
            rblst.CssClass = "FieldLabel"
        End If
        If TypeOf ctrl Is RadioButton Then
            rbtn = CType(ctrl, RadioButton)
            rbtn.Font.Name = ""
            rbtn.Font.Bold = False
            rbtn.Font.Size = FontUnit.Empty
            rbtn.ForeColor = Color.Empty
            rbtn.CssClass = "FieldLabel"
        End If
        If TypeOf ctrl Is CheckBox Then
            chk = CType(ctrl, CheckBox)
            chk.Font.Name = ""
            chk.Font.Bold = False
            chk.Font.Size = FontUnit.Empty
            chk.ForeColor = Color.Empty
            chk.CssClass = "FieldLabel"
        End If
        If TypeOf ctrl Is CheckBoxList Then
            cblst = CType(ctrl, CheckBoxList)
            cblst.Font.Name = ""
            cblst.Font.Bold = False
            cblst.Font.Size = FontUnit.Empty
            cblst.ForeColor = Color.Empty
            cblst.CssClass = "FieldLabel"
        End If
        If TypeOf ctrl Is DropDownList Then

            ddl = CType(ctrl, DropDownList)
            ddl.Font.Name = ""
            ddl.Font.Bold = False
            ddl.Font.Size = FontUnit.Empty
            ddl.ForeColor = Color.Empty
            If ctrlP.ID = strCPNL1 Or ctrlP.ID = strCPNL2 Or ctrlP.ID = strCPNL3 Then
                ddl.CssClass = "DDLFieldFE"
            Else
                ddl.CssClass = "DDLField"
            End If

        End If
    End Sub
    Public Function TaskForward(ByVal OldTaskOwnerName As String, ByVal OldTaskOwnerID As Integer, ByVal TaskOwnerName As String, ByVal ForwardBy As Integer, ByVal ForwardTo As Integer, ByVal TaskOwner As Integer, ByVal intCompanyID As Integer, ByVal intCallNo As Integer, ByVal intTaskNo As Integer, Optional ByVal strMailStatus As String = "") As ReturnValue

        Dim stReturn As ReturnValue
        Dim strSql As String = String.Empty

        If intCallNo = 0 OrElse intTaskNo = 0 Then
            stReturn.ErrorCode = 1
            Return stReturn
        End If

        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            arColumnName.Add("CF_NU9_CompId")
            arColumnName.Add("CF_NU8_Call_Number")
            arColumnName.Add("CF_NU8_Task_Number")
            arColumnName.Add("CF_NU8_Forwarded_By")
            arColumnName.Add("CF_NU8_Forwarded_To")
            arColumnName.Add("CF_DT8_Forwarded_Date")
            arColumnName.Add("CF_NU8_Task_Owner")

            Dim dtNow As Date = Now
            arRowData.Add(intCompanyID)
            arRowData.Add(intCallNo)
            arRowData.Add(intTaskNo)
            arRowData.Add(ForwardBy)
            arRowData.Add(ForwardTo)
            arRowData.Add(dtNow)
            arRowData.Add(ForwardBy)
            'To store the value of previous owner Id
            'Dim intPreviousOwnerID As Integer = HttpContext.Current.Session("PropTaskOwnerID")
            Dim intPreviousOwnerID As Integer = OldTaskOwnerID

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False

            If SQL.Save("T040012", "mdlMain", "TaskForward", arColumnName, arRowData) = True Then
                If SQL.Update("mdlMain", "TaskForward", "update t040021 set TM_VC8_Supp_Owner='" & ForwardTo & "',TM_NU9_Previous_Owner=" & intPreviousOwnerID & " where TM_NU9_Call_No_FK=" & intCallNo & " and TM_NU9_Comp_ID_FK=" & intCompanyID & " and TM_NU9_Task_no_PK=" & intTaskNo & "", SQL.Transaction.Serializable) = True Then

                    ' -- Create Log
                    If strMailStatus = "N" Then  'in case user don't wan't to send the mail to the task owner 

                        strSql = "INSERT into T990021(TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK," & _
                                   " TM_NU9_Code,TM_VC100_Req_Status,TM_VC1000_Subtsk_Desc," & _
                                   " TM_DT8_Task_Date,TM_VC8_task_type,TM_VC8_Supp_Owner,TM_DT8_Plan_Cl_Date,TM_FL8_Est_Hr, " & _
                                   " TM_NU9_Dependency,TM_DT8_Close_Date,TM_VC100_Comp_Actions,TM_NU9_Forward_To,TM_NU9_At_No, " & _
                                   " TM_VC50_Close_case,TM_DT8_Est_close_date,TM_NU9_Attach_no,TM_NU9_Assign_by,TM_VC50_Deve_status," & _
                                   " TM_NU9_Est_Bill_Hrs,TM_DT8_Task_Close_Date,TM_NU9_Draft_Invoice,TM_NU9_Bargain_Loss," & _
                                   " TM_VC50_Fully_Paid,TM_NU9_Non_Bill_Hrs,TM_FL8_Actual_Bill_Hrs,TM_DT8_Remind_date,TM_VC16_Remind_time," & _
                                   " TM_VC50_Remind_Status, TM_NU9_Forwd_grp, TM_NU9_Forwd_emp, TM_NU9_forwd_Call, TM_DT8_Forwd_Dt_Time,  " & _
                                   " TM_VC8_Project,TM_VC8_Priority,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Mandatory,TM_NU9_Case_No,TM_NU9_Agmt_No," & _
                                   " TM_CH1_Forms, TM_CH1_Invoice_Pending, TM_VC50_Cat_Code_1, TM_VC50_Cat_Code_2, TM_VC50_Cat_Code_3, TM_VC50_Cat_Code_4, " & _
                                   " TM_VC50_Cat_Code_5,TM_DT8_Log_Date,TM_NU4_Event_ID,TM_CH1_MailSent,TM_VC8_Call_Status,TM_VC8_Call_Type )" & _
                                    " select TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK  " & _
                                   " ,TM_NU9_Code,TM_VC100_Req_Status,TM_VC1000_Subtsk_Desc, " & _
                                   " TM_DT8_Task_Date,TM_VC8_task_type,TM_VC8_Supp_Owner,TM_DT8_Plan_Cl_Date,TM_FL8_Est_Hr, " & _
                                   " TM_NU9_Dependency,TM_DT8_Close_Date,TM_VC100_Comp_Actions,'" & ForwardTo & "' as Forwarded_To,TM_NU9_At_No, " & _
                                   " TM_VC50_Close_case,TM_DT8_Est_close_date,TM_NU9_Attach_no,TM_NU9_Assign_by,TM_VC50_Deve_status, " & _
                                   " TM_NU9_Est_Bill_Hrs,TM_DT8_Task_Close_Date,TM_NU9_Draft_Invoice,TM_NU9_Bargain_Loss, " & _
                                   " TM_VC50_Fully_Paid,TM_NU9_Non_Bill_Hrs,TM_FL8_Actual_Bill_Hrs,TM_DT8_Remind_date,TM_VC16_Remind_time, " & _
                                   " TM_VC50_Remind_Status,TM_NU9_Forwd_grp,'" & ForwardBy & "' as Forward_By,TM_NU9_forwd_Call,TM_DT8_Forwd_Dt_Time, " & _
                                  " TM_VC8_Project,TM_VC8_Priority,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Mandatory,TM_NU9_Case_No,TM_NU9_Agmt_No, " & _
                                   " TM_CH1_Forms,TM_CH1_Invoice_Pending,TM_VC50_Cat_Code_1,TM_VC50_Cat_Code_2,TM_VC50_Cat_Code_3,TM_VC50_Cat_Code_4, " & _
                                   " TM_VC50_Cat_Code_5,getdate() as TM_DT8_Log_Date,4 as TM_NU4_Event_ID,'T' as TM_CH1_MailSent,CN_VC20_Call_Status, " & _
                                   " CM_VC8_Call_Type " & _
                                   " from t040021,t040011  " & _
                                   " Where  TM_Nu9_Call_No_Fk=CM_Nu9_Call_No_Pk AND TM_NU9_Comp_ID_Fk=CM_NU9_Comp_ID_Fk " & _
                                   " AND TM_Nu9_Call_No_Fk=" & intCallNo & " and TM_NU9_Comp_ID_Fk=" & intCompanyID & " and Tm_nu9_Task_no_pk=  " & intTaskNo
                        '       SQL.DBTable = "T990021"
                    Else

                        strSql = "INSERT into T990021(TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK," & _
                                                     " TM_NU9_Code,TM_VC100_Req_Status,TM_VC1000_Subtsk_Desc," & _
                                                     " TM_DT8_Task_Date,TM_VC8_task_type,TM_VC8_Supp_Owner,TM_DT8_Plan_Cl_Date,TM_FL8_Est_Hr, " & _
                                                     " TM_NU9_Dependency,TM_DT8_Close_Date,TM_VC100_Comp_Actions,TM_NU9_Forward_To,TM_NU9_At_No, " & _
                                                     " TM_VC50_Close_case,TM_DT8_Est_close_date,TM_NU9_Attach_no,TM_NU9_Assign_by,TM_VC50_Deve_status," & _
                                                     " TM_NU9_Est_Bill_Hrs,TM_DT8_Task_Close_Date,TM_NU9_Draft_Invoice,TM_NU9_Bargain_Loss," & _
                                                     " TM_VC50_Fully_Paid,TM_NU9_Non_Bill_Hrs,TM_FL8_Actual_Bill_Hrs,TM_DT8_Remind_date,TM_VC16_Remind_time," & _
                                                     " TM_VC50_Remind_Status, TM_NU9_Forwd_grp, TM_NU9_Forwd_emp, TM_NU9_forwd_Call, TM_DT8_Forwd_Dt_Time,  " & _
                                                     " TM_VC8_Project,TM_VC8_Priority,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Mandatory,TM_NU9_Case_No,TM_NU9_Agmt_No," & _
                                                     " TM_CH1_Forms, TM_CH1_Invoice_Pending, TM_VC50_Cat_Code_1, TM_VC50_Cat_Code_2, TM_VC50_Cat_Code_3, TM_VC50_Cat_Code_4, " & _
                                                     " TM_VC50_Cat_Code_5,TM_DT8_Log_Date,TM_NU4_Event_ID,TM_CH1_MailSent,TM_VC8_Call_Status,TM_VC8_Call_Type )" & _
                                                      " select TM_NU9_Call_No_FK,TM_NU9_Comp_ID_FK,TM_NU9_Task_no_PK  " & _
                                                     " ,TM_NU9_Code,TM_VC100_Req_Status,TM_VC1000_Subtsk_Desc, " & _
                                                     " TM_DT8_Task_Date,TM_VC8_task_type,TM_VC8_Supp_Owner,TM_DT8_Plan_Cl_Date,TM_FL8_Est_Hr, " & _
                                                     " TM_NU9_Dependency,TM_DT8_Close_Date,TM_VC100_Comp_Actions,'" & ForwardTo & "' as Forwarded_To,TM_NU9_At_No, " & _
                                                     " TM_VC50_Close_case,TM_DT8_Est_close_date,TM_NU9_Attach_no,TM_NU9_Assign_by,TM_VC50_Deve_status, " & _
                                                     " TM_NU9_Est_Bill_Hrs,TM_DT8_Task_Close_Date,TM_NU9_Draft_Invoice,TM_NU9_Bargain_Loss, " & _
                                                     " TM_VC50_Fully_Paid,TM_NU9_Non_Bill_Hrs,TM_FL8_Actual_Bill_Hrs,TM_DT8_Remind_date,TM_VC16_Remind_time, " & _
                                                     " TM_VC50_Remind_Status,TM_NU9_Forwd_grp,'" & ForwardBy & "' as Forward_By,TM_NU9_forwd_Call,TM_DT8_Forwd_Dt_Time, " & _
                                                    " TM_VC8_Project,TM_VC8_Priority,TM_CH1_Comment,TM_CH1_Attachment,TM_CH1_Mandatory,TM_NU9_Case_No,TM_NU9_Agmt_No, " & _
                                                     " TM_CH1_Forms,TM_CH1_Invoice_Pending,TM_VC50_Cat_Code_1,TM_VC50_Cat_Code_2,TM_VC50_Cat_Code_3,TM_VC50_Cat_Code_4, " & _
                                                     " TM_VC50_Cat_Code_5,getdate() as TM_DT8_Log_Date,4 as TM_NU4_Event_ID,'F' as TM_CH1_MailSent,CN_VC20_Call_Status, " & _
                                                     " CM_VC8_Call_Type " & _
                                                     " from t040021,t040011  " & _
                                                     " Where  TM_Nu9_Call_No_Fk=CM_Nu9_Call_No_Pk AND TM_NU9_Comp_ID_Fk=CM_NU9_Comp_ID_Fk " & _
                                                     " AND TM_Nu9_Call_No_Fk=" & intCallNo & " and TM_NU9_Comp_ID_Fk=" & intCompanyID & " and Tm_nu9_Task_no_pk=  " & intTaskNo

                    End If

                    SQL.Update("mdlmain", "TaskForward-765", strSql, SQL.Transaction.ReadCommitted)
                    ' ----------------------
                    If CreateTaskForwardAction(OldTaskOwnerName, TaskOwnerName, intCallNo, intTaskNo, intCompanyID) = True Then
                        'SendTaskForwardMail(ForwardTo, ForwardBy)
                    End If

                    stReturn.ErrorCode = 0
                    stReturn.ErrorMessage = "Call Forwarded successfully..."
                    Return stReturn
                Else
                    SQL.Delete("mdlMain", "TaskForward-608", "delete from T040012 where CF_NU9_CompId=" & intCompanyID & " and CF_NU8_Call_Number=" & intCallNo & " and CF_NU8_Task_Number=" & intTaskNo & " and CF_NU8_Forwarded_By=" & ForwardBy & " and CF_NU8_Forwarded_To=" & ForwardTo & "  and CF_DT8_Forwarded_Date='" & dtNow & "'", SQL.Transaction.Serializable, "T040012")
                    stReturn.ErrorCode = 1
                    Return stReturn
                End If

            Else
                stReturn.ErrorCode = 1
                stReturn.ErrorMessage = "Cannot forward call at present. Please try later"
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "TaskForward-608", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
        Return stReturn
    End Function
    Private Function CreateTaskForwardAction(ByVal ForwardFromName As String, ByVal ForwardToName As String, ByVal Callno As Integer, ByVal TaskNo As Integer, ByVal CompanyID As Integer) As Boolean

        SQL.DBTracing = False
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            arColumnName.Add("AM_NU9_Action_Number")
            arColumnName.Add("AM_NU9_Task_Number")
            arColumnName.Add("AM_NU9_Call_Number")
            arColumnName.Add("AM_NU9_Comp_ID_FK")
            arColumnName.Add("AM_CH1_Mandatory")
            arColumnName.Add("AM_FL8_Used_Hr")
            arColumnName.Add("AM_VC_2000_Description")
            arColumnName.Add("AM_DT8_Action_Date")
            arColumnName.Add("AM_VC8_Supp_Owner")
            arColumnName.Add("AM_DT8_Action_Date_Auto")
            arColumnName.Add("AM_VC8_ActionType")
            Dim intActionNo As Integer = SQL.Search("", "", "select isnull(max(AM_NU9_Action_Number),0) from T040031 where AM_NU9_Call_Number=" & Callno & " And AM_NU9_Task_Number=" & TaskNo & " and AM_NU9_Comp_ID_FK=" & CompanyID & "")
            intActionNo += 1

            arRowData.Add(intActionNo)
            arRowData.Add(TaskNo)
            arRowData.Add(Callno)
            arRowData.Add(CompanyID)
            arRowData.Add("O")
            arRowData.Add(0)
            arRowData.Add("Task Forwarded From [" & ForwardFromName & "] To [" & ForwardToName & "] By [" & HttpContext.Current.Session("PropUserName") & "]")
            arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropUserId"))
            arRowData.Add(Now)
            arRowData.Add("External")
            If SQL.Save("T040031", "mdlMain", "CreateTaskForwardAction", arColumnName, arRowData) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "CreateTaskForwardAction-650", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function

    'Action fill when task is closed or reopened. page(Task_edit) (Amit)
    Public Function CreateTaskAutoAction(ByVal OldStatus As String, ByVal NewStatus As String, ByVal CallNO As Integer, ByVal TaskNo As Integer, ByVal CompanyID As Integer) As Boolean
        'SQL.DBTable = "T040031"
        SQL.DBTracing = False

        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            arColumnName.Add("AM_NU9_Action_Number")
            arColumnName.Add("AM_NU9_Task_Number")
            arColumnName.Add("AM_NU9_Call_Number")
            arColumnName.Add("AM_NU9_Comp_ID_FK")
            arColumnName.Add("AM_CH1_Mandatory")
            arColumnName.Add("AM_FL8_Used_Hr")
            arColumnName.Add("AM_VC_2000_Description")
            arColumnName.Add("AM_DT8_Action_Date")
            arColumnName.Add("AM_VC8_Supp_Owner")
            arColumnName.Add("AM_DT8_Action_Date_Auto")
            arColumnName.Add("AM_VC8_ActionType")

            Dim intActionNo As Integer = SQL.Search("", "", "select isnull(max(AM_NU9_Action_Number),0) from T040031 where AM_NU9_Call_Number=" & CallNO & " And AM_NU9_Task_Number=" & TaskNo & " and AM_NU9_Comp_ID_FK=" & CompanyID & "")
            intActionNo += 1

            arRowData.Add(intActionNo)
            arRowData.Add(TaskNo)
            arRowData.Add(CallNO)
            arRowData.Add(CompanyID)
            arRowData.Add("O")
            arRowData.Add(0)
            arRowData.Add("Task Status changed from [" & OldStatus & "] to [" & NewStatus & "] by " & HttpContext.Current.Session("PropUserName"))
            arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropUserId"))
            arRowData.Add(Now)
            arRowData.Add("External")
            If SQL.Save("T040031", "mdlMain", "CreateTaskForwardAction-650", arColumnName, arRowData) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "CreateTaskForwardAction-650", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        End Try
    End Function
    Enum MSG As Integer
        msgOK = 1
        msgError = 2
        msgInfo = 3
        msgWarning = 4
    End Enum
    Public Sub MessagePanelListStyle(ByVal lst As ListBox, ByVal msgStatus As MSG)
        If msgStatus = MSG.msgOK Then
            lst.ForeColor = Color.Black
        ElseIf msgStatus = MSG.msgError Then
            lst.ForeColor = Color.Red
        End If
    End Sub

#Region "Template Attachment Functions"
    Public Function SaveTemplateAttachmentVersion(ByVal AttachmentPath As String, _
    ByVal AttachmentSize As Double, _
    ByVal AttachmentName As String, _
    ByVal AttachmentLevel As AttachLevel, _
    ByVal VersionNo As Double, _
    ByVal Update As Boolean, _
    ByVal CallNo As Integer, _
    ByVal TaskNo As Integer, _
    ByVal CompanyID As Integer, _
    ByVal ActionNo As Integer, ByVal TemplateId As Integer) As Boolean

        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T050051"
            SQL.DBTracing = False

            Dim intFileID As Integer = SQL.Search("mdlMain", "SaveTemplateAttachmentVersion", "Select max(VH_NU9_File_ID_PK) from T050051")
            intFileID += 1

            Dim arColumnName As New ArrayList
            Dim arRowData As New ArrayList
            Dim dtCreatedate As Date

            If Update = True Then
                dtCreatedate = SQL.Search("mdlMain", "SaveTemplateAttachmentVersion", "select  MIN(VH_DT8_Date) from T050051 where VH_NU9_Call_Number=" & CallNo & " and VH_NU9_Task_Number=" & TaskNo & " and VH_NU9_Action_Number=" & ActionNo & " and VH_VC255_File_Name='" & AttachmentName.Trim & "' ")

                arColumnName.Add("VH_DT8_Date")
                arRowData.Add(dtCreatedate)
            Else
                arColumnName.Add("VH_DT8_Date")
                arRowData.Add(Now)
            End If

            arColumnName.Add("VH_NU9_File_ID_PK")
            arColumnName.Add("VH_VC255_File_Name")
            arColumnName.Add("VH_VC255_File_Size")
            arColumnName.Add("VH_VC255_File_Path")
            arColumnName.Add("VH_VC1_Status")
            arColumnName.Add("VH_NU9_Address_Book_Number")
            arColumnName.Add("VH_NU9_Call_Number")
            arColumnName.Add("VH_NU9_Task_Number")
            arColumnName.Add("VH_NU9_Action_Number")
            arColumnName.Add("VH_NU9_CompId_Fk")
            'arColumnName.Add("VH_DT8_Date")
            arColumnName.Add("VH_VC8_Role")
            arColumnName.Add("VH_NU9_Version")
            arColumnName.Add("VH_DT8_Modify_Date")
            arColumnName.Add("VH_IN4_Level")
            arColumnName.Add("VH_NU9_TemplateID_FK")
            arColumnName.Add("VH_VC4_Active_Status")


            arRowData.Add(intFileID)
            arRowData.Add(AttachmentName.Trim)
            arRowData.Add(AttachmentSize)
            arRowData.Add(AttachmentPath.Trim)
            arRowData.Add("U")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            'arRowData.Add(11)
            arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(CompanyID)
            'arRowData.Add("ADM")
            'arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropRole").Trim)
            'arRowData.Add("ION")
            arRowData.Add(VersionNo)
            arRowData.Add(Now)

            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            arRowData.Add(TemplateId)
            arRowData.Add("ENB")

            If SQL.Save("T050051", "mdlMain", "SaveTemplateAttachmentVersion", arColumnName, arRowData) = True Then
                ' Update Attachment Flag in database
                Select Case AttachmentLevel
                    Case AttachLevel.CallLevel
                        WSSUpdate.UpdateForTemplateAttachment(CallNo, TaskNo, ActionNo, CompanyID, AttachLevel.CallLevel)
                    Case AttachLevel.TaskLevel
                        WSSUpdate.UpdateForTemplateAttachment(CallNo, TaskNo, ActionNo, CompanyID, AttachLevel.TaskLevel)
                End Select

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            CreateLog("mdlMain", "SaveTemplateAttachmentVersion", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
            Return False
        End Try

    End Function

    Public Function SaveTemplateAttachment(ByVal FileID As Integer, _
    ByVal FilePath As String, _
    ByVal FileName As String, _
    ByVal CallNo As Integer, _
    ByVal TaskNo As Integer, _
    ByVal ActionNo As Integer, _
    ByVal AttachmentSize As Integer, _
    ByVal VersionNo As Integer, _
    ByVal CompanyID As Integer, _
    ByVal AttachmentLevel As AttachLevel, ByVal TemplateId As Integer, ByVal OriginalPath As String) As Boolean

        'Save the location of the file in the database
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            arColumnName.Add("AT_NU9_File_ID_PK")
            arColumnName.Add("AT_VC255_File_Name")
            arColumnName.Add("AT_VC255_File_Size")
            arColumnName.Add("AT_VC255_File_Path")
            arColumnName.Add("AT_VC1_STatus")
            arColumnName.Add("AT_NU9_Address_Book_Number")
            arColumnName.Add("AT_NU9_Call_Number")
            arColumnName.Add("AT_NU9_Task_Number")
            arColumnName.Add("AT_NU9_Action_Number")
            arColumnName.Add("AT_NU9_CompId_Fk")
            'arColumnName.Add("AT_DT8_Date")
            arColumnName.Add("AT_VC8_Role")
            arColumnName.Add("AT_NU9_Version")
            arColumnName.Add("AT_DT8_Modify_Date")
            arColumnName.Add("AT_IN4_Level")
            arColumnName.Add("AT_NU9_TemplateID_FK")
            arColumnName.Add("AT_Folder_Path")
            arColumnName.Add("VH_VC4_Active_Status")


            arRowData.Add(FileID)
            arRowData.Add(FileName)
            arRowData.Add(AttachmentSize)
            arRowData.Add(FilePath.Trim)
            arRowData.Add("U")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            'arRowData.Add(52)
            arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(CompanyID)
            'arRowData.Add("ADM")
            ''arRowData.Add(Now.ToShortDateString)
            arRowData.Add(HttpContext.Current.Session("PropRole"))
            'arRowData.Add("ION")
            arRowData.Add(VersionNo)
            arRowData.Add(Now)
            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            arRowData.Add(TemplateId)
            arRowData.Add(OriginalPath)
            arRowData.Add("ENB")

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T050041"
            SQL.DBTracing = False

            SQL.Update("T050041", "mdlMain", "SaveTemplateAttachment", "select * from T050041 where AT_NU9_File_ID_PK=" & FileID & "", arColumnName, arRowData)
            Return True
        Catch ex As Exception
            Return False
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Function

    Public Function UpdateTemplateAttachment(ByVal FilePath As String, _
    ByVal FileName As String, _
    ByVal CallNo As Integer, _
    ByVal TaskNo As Integer, _
    ByVal ActionNo As Integer, _
    ByVal AttachmentSize As Integer, _
    ByVal VersionNo As Integer, _
    ByVal CompanyID As Integer, _
    ByVal AttachmentLevel As AttachLevel, ByVal TemplateId As Integer, ByVal OriginalPath As String) As Boolean

        'Save the location of the file in the database
        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList

        Try
            'arColumnName.Add("AT_NU9_File_ID_PK")
            arColumnName.Add("AT_VC255_File_Name")
            arColumnName.Add("AT_VC255_File_Size")
            arColumnName.Add("AT_VC255_File_Path")
            arColumnName.Add("AT_VC1_SATus")
            arColumnName.Add("AT_NU9_Address_Book_Number")
            'arColumnName.Add("AT_NU9_Call_Number")
            arColumnName.Add("AT_NU9_Task_Number")
            arColumnName.Add("AT_NU9_Action_Number")
            arColumnName.Add("AT_NU9_CompId_Fk")
            'arColumnName.Add("AT_DT8_Date")
            arColumnName.Add("AT_VC8_Role")
            arColumnName.Add("AT_NU9_Version")
            arColumnName.Add("AT_DT8_Modify_Date")
            arColumnName.Add("AT_NU9_TemplateID_FK")
            arColumnName.Add("AT_IN4_Level")
            arColumnName.Add("AT_Folder_Path")
            arColumnName.Add("VH_VC4_Active_Status")



            'arRowData.Add(FileID)
            arRowData.Add(FileName)
            arRowData.Add(AttachmentSize)
            arRowData.Add(FilePath.Trim)
            arRowData.Add("U")
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            'arRowData.Add(52)
            'arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(companyid)
            'arRowData.Add("ADM")
            ''arRowData.Add(Now.ToShortDateString)
            arRowData.Add(HttpContext.Current.Session("PropRole"))
            'arRowData.Add("ION")
            arRowData.Add(VersionNo)
            arRowData.Add(Now)
            arRowData.Add(TemplateId)

            If AttachmentLevel = AttachLevel.CallLevel Then
                arRowData.Add(1)
            ElseIf AttachmentLevel = AttachLevel.TaskLevel Then
                arRowData.Add(2)
            ElseIf AttachmentLevel = AttachLevel.ActionLevel Then
                arRowData.Add(3)
            End If
            arRowData.Add(OriginalPath)
            arRowData.Add("ENB")


            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            '     SQL.DBTable = "T050041"
            SQL.DBTracing = False

            SQL.Update("T050041", "mdlMain", "SaveTemplateAttachment", "select * from T050041 where AT_NU9_Call_Number=" & CallNo & " and AT_VC255_File_Name='" & FileName.Trim & "' and AT_VC1_Status='U'", arColumnName, arRowData)

            SQL.Delete("mdlMain", "SaveTemplateAttachment", "delete from T050041 where AT_NU9_Call_Number=" & CallNo & " and AT_VC255_File_Name='" & FileName.Trim & "' and AT_VC1_Status='T'", SQL.Transaction.Serializable, "T050041")

            Return True
        Catch ex As Exception
            CreateLog("mdlMain", "UpdateTemplateAttachment", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
            Return False
        Finally
            arColumnName.Clear()
            arRowData.Clear()
        End Try
    End Function
#End Region
    'Returns Date in a format required for monitoring center
    Public ReadOnly Property PropSetDate() As String
        Get
            Return Now.Year & Now.Month & Now.Day
        End Get
    End Property
    'Returns Time in a format required for monitoring center
    Public ReadOnly Property PropSetTime() As String
        Get
            Return Now.Hour & Now.Minute & Now.Second
        End Get
    End Property
    Public Function SaveMail(ByVal strTo As String, ByVal strFrom As String, ByVal strMessage As String, ByVal strSubject As String, ByVal strMailType As String, ByVal strToID As String, ByVal CallNo As Integer, ByVal TaskNo As Integer, ByVal ActionNo As Integer, ByVal CompanyID As Integer) As ReturnValue
        Dim stReturn As ReturnValue

        Dim strCallType As String
        strCallType = GetCallType(CallNo, CompanyID)
        Dim strTaskType As String
        strTaskType = GetTaskType(TaskNo, CallNo, CompanyID)
        Dim strActionType As String
        strActionType = GetActionType(ActionNo, TaskNo, CallNo, CompanyID)

        Dim arColumnName As New ArrayList
        Dim arRowData As New ArrayList
        Try

            arColumnName.Add("SM_NU9_ID_PK")
            arColumnName.Add("SM_VC72_TO")
            arColumnName.Add("SM_VC72_From")
            arColumnName.Add("SM_VC2000_Message")
            arColumnName.Add("SM_VC100_Subject")
            arColumnName.Add("SM_VC30_Mail_Type")

            arColumnName.Add("SM_DT8_Date")
            arColumnName.Add("SM_VC16_UserID")
            arColumnName.Add("SM_VC8_Status")
            arColumnName.Add("SM_NU9_CallNo")
            arColumnName.Add("SM_NU9_TaskNo")
            arColumnName.Add("SM_NU9_ActionNo")
            arColumnName.Add("SM_VC250_ToID")

            arColumnName.Add("SM_VC16_Call_Type")
            arColumnName.Add("SM_VC16_Task_Type")
            arColumnName.Add("SM_VC16_Action_Type")

            Dim intCallNo As Integer = SQL.Search("mdlMain", "SaveMail", "select max(SM_NU9_ID_PK) from T040071")
            intCallNo += 1
            arRowData.Add(intCallNo)
            arRowData.Add(strTo)
            arRowData.Add(strFrom)
            arRowData.Add(strMessage)
            arRowData.Add(strSubject)
            arRowData.Add(strMailType)

            arRowData.Add(Now)
            arRowData.Add(HttpContext.Current.Session("PropUserID"))
            arRowData.Add(0)
            arRowData.Add(CallNo)
            arRowData.Add(TaskNo)
            arRowData.Add(ActionNo)
            arRowData.Add(strToID)

            arRowData.Add(strCallType)
            arRowData.Add(strTaskType)
            arRowData.Add(strActionType)

            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBConnection = strConnection
            ' Table name
            SQL.DBTracing = False

            If SQL.Save("T040071", "mdlMain", "SaveMail", arColumnName, arRowData) = False Then
                stReturn.ErrorMessage = "Server is busy please try later..."
                stReturn.FunctionExecuted = False
                stReturn.ErrorCode = 1
            Else
                stReturn.ErrorMessage = "Records Saved successfully..."
                stReturn.FunctionExecuted = True
                stReturn.ErrorCode = 0
                stReturn.ExtraValue = intCallNo
            End If
            Return stReturn
        Catch ex As Exception
            stReturn.ErrorMessage = "Server is busy please try later..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 2
            CreateLog("WWSSave", "SaveMail-206", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
            Return stReturn
        End Try
    End Function
    Public Property PropFrom()
        Get
            Return mstrFrom
        End Get
        Set(ByVal Value)
            mstrFrom = Value
        End Set
    End Property
    Public Property PropWSSLink()
        Get
            Return mstrWSSLink
        End Get
        Set(ByVal Value)
            mstrWSSLink = Value
        End Set
    End Property

    Public Function GetCallType(ByVal CallNo As Integer, ByVal CompanyID As Integer) As String
        Try
            Dim strCallType As String
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            strCallType = SQL.Search("mdlMain", "GetCallType", "select CM_VC8_Call_Type from T040011 where CM_NU9_Call_No_PK=" & CallNo & " and CM_NU9_Comp_Id_FK=" & CompanyID)
            Return strCallType
        Catch ex As Exception
            CreateLog("mdlMain", "GetCallType", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    Public Function GetTaskType(ByVal TaskNo As Integer, ByVal CallNo As Integer, ByVal CompanyID As Integer) As String
        Try
            Dim strTaskType As String
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            strTaskType = SQL.Search("mdlMain", "GetTaskType", "select TM_VC8_task_type from T040021 where TM_NU9_Task_no_PK=" & TaskNo & " and TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Comp_ID_FK=" & CompanyID)
            Return strTaskType
        Catch ex As Exception
            CreateLog("mdlMain", "GetTaskType", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    Public Function GetActionType(ByVal ActionNo As Integer, ByVal TaskNo As Integer, ByVal CallNo As Integer, ByVal CompanyID As Integer) As String
        Try
            Dim strActionType As String
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            strActionType = SQL.Search("mdlMain", "GetActionType", "select AM_VC100_Action_type from T040031 where AM_NU9_Action_Number=" & ActionNo & " and AM_NU9_Task_Number=" & TaskNo & " and AM_NU9_Call_Number=" & CallNo & " and	AM_NU9_Comp_ID_FK=" & CompanyID)
            Return strActionType
        Catch ex As Exception
            CreateLog("mdlMain", "GetActionType", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    ''**************************************Encryption Start***************************************
    'This function is used to encrypt string data
    'Author:-Harpreet Singh, Sunil Rana
    'Date:- 
    'Modified Date:- ----
    Public Function IONEncrypt(ByVal strPassword As String) As String
        Try
            'get the byte of the password 
            Dim key As Byte() = Encoding.ASCII.GetBytes(strPassword)
            'declare the integer
            Dim int As Integer
            Dim intl As Integer = key.Length
            'multiply each byte by 2 
            For int = 0 To key.Length - 1
                Dim newbyte As Int32
                newbyte = key(int)
                newbyte = newbyte * 2
                If newbyte = 160 Then newbyte = 161
                If newbyte = 128 Then newbyte = 129
                'If newbyte = 146 Then newbyte = 201
                'If newbyte = 154 Then newbyte = 203
                'If newbyte = 156 Then newbyte = 205
                key(int) = newbyte
            Next
            'declare a new string for the password to return 
            Dim strcharnew As String
            'loop for converting the byte in char which will be new string 
            For inti As Integer = 0 To key.Length - 1
                Dim s As Char = Convert.ToChar(key(inti))
                strcharnew = strcharnew & s
            Next
            'return the ne string 
            Return strcharnew
        Catch ex As Exception
            CreateLog("mdlMain", "IONDecrypt", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
            Return "Not Done"
        End Try
    End Function
    'This function is used to decrypt string data
    'Author:-Harpreet Singh, Sunil Rana
    'Date:- 
    'Modified Date:- ----
    Public Function IONDecrypt(ByVal strPassword As String) As String
        Try
            'get the unicode  of the password which is in coded form 
            Dim key As Byte() = Encoding.Unicode.GetBytes(strPassword)

            Dim intlength As Integer = key.Length
            For int As Integer = 0 To key.Length - 1
                Dim oldbyt As Int32
                If key(int) = 129 Then
                    oldbyt = 64
                ElseIf key(int) = 161 Then
                    oldbyt = 80
                Else
                    oldbyt = key(int) / 2
                End If
                key(int) = oldbyt
            Next

            'for the original string 
            Dim strDec_password(key.Length - 1) As String
            Dim s As Char
            For inti As Integer = 0 To key.Length - 1
                s = Convert.ToChar(key(inti))
                strDec_password(inti) = s
            Next
            Dim strReturn As String
            Dim I As Integer
            For I = 0 To strDec_password.Length - 1
                If (I Mod 2) = 0 Then
                    strReturn = strReturn & strDec_password(I)
                End If
            Next
            Return (strReturn)
        Catch ex As Exception
            CreateLog("mdlMain", "IONDecrypt", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
            Return "NOT DONE"
        End Try
    End Function
    Public Function FillUDCDropDown(ByVal ddlCustom As DropDownList, ByVal intDefaultNo As Integer, ByVal intProductCode As Integer, ByVal strUDCType As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlMain", "FillUDCDropDown", "select Name as ID,Description,Company from UDC where ProductCode=" & intProductCode & " and UDCType='" & strUDCType & "'", SQL.CommandBehaviour.Default, blnStatus)
            Dim inti As Integer
            inti = 0
            ddlCustom.Items.Add("")
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(sqRDR(0))
                    inti = inti + 1
                    If inti >= intDefaultNo Then
                        ddlCustom.Items.Add("More")
                        sqRDR.Close()
                        Exit While
                    End If
                End While
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillUDCDropDown", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    Public Function FillNonUDCDropDown(ByVal ddlCustom As DropDownList, ByVal strSQL As String, Optional ByVal OptionalField As Boolean = False)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            If OptionalField = True Then
                ddlCustom.Items.Add("")
            End If
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillNonUDCDropDown", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        End Try
    End Function
    'fill company drop down 
    Public Function FillCompanyDdl(ByVal ddlCustom As DropDownList, ByVal strSQL As String)
        Dim sqRDR As SqlClient.SqlDataReader
        Dim blnStatus As Boolean
        Try
            sqRDR = SQL.Search("mdlmain", "FillNonUDCDropDown-1718", strSQL, SQL.CommandBehaviour.Default, blnStatus)
            ddlCustom.Items.Clear()
            ddlCustom.Items.Add("")
            If blnStatus = True Then
                While sqRDR.Read
                    ddlCustom.Items.Add(New ListItem(sqRDR(1), sqRDR(0)))
                End While
                sqRDR.Close()
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillCompanyDdl", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function
    Public Function GetStatus(ByVal CompanyID As Integer, ByVal CallNumber As Int64, ByVal StatusOf As StatusType, Optional ByVal TaskNumber As Integer = 0) As String
        Dim strSql As String
        Try
            Select Case StatusOf
                Case StatusType.CallStatus
                    strSql = "select  Top 1 SU_VC50_Status_Name from T040081 where SU_NU_Range_To>=" & _
                                " (select SU_NU9_Status_Code from T040081 Where SU_VC50_Status_Name=( " & _
                                " Select CN_VC20_Call_Status From T040011 Where CM_NU9_Call_No_PK=" & CallNumber & " and CM_NU9_Comp_Id_FK=" & CompanyID & " ) " & _
                                " and (SU_NU9_CompID=" & CompanyID & " or SU_NU9_CompID=0)) " & _
                                " order by SU_NU9_Status_Code asc "
                Case StatusType.TaskStatus
                    strSql = "select  Top 1 SU_VC50_Status_Name from T040081 where SU_NU_Range_To>=" & _
                               " (select SU_NU9_Status_Code from T040081 Where SU_VC50_Status_Name=( " & _
                               " Select TM_VC50_Deve_Status From T040021 Where TM_NU9_Task_No_PK =" & TaskNumber & " AND TM_NU9_Call_No_FK=" & CallNumber & " and TM_NU9_Comp_Id_FK=" & CompanyID & " ) " & _
                               " and (SU_NU9_CompID=" & CompanyID & " or SU_NU9_CompID=0)) " & _
                               " order by SU_NU9_Status_Code asc "
                Case StatusType.TemplateCallStatus
                    strSql = "select  Top 1 SU_VC50_Status_Name from T040081 where SU_NU_Range_To>=" & _
                               " (select SU_NU9_Status_Code from T040081 Where SU_VC50_Status_Name=( " & _
                               " Select TCM_VC20_Call_Status From T050021 Where TCM_NU9_Call_No_PK=" & CallNumber & " and TCM_NU9_CompId_FK=" & CompanyID & " ) " & _
                               " and (SU_NU9_CompID=" & CompanyID & "  or SU_NU9_CompID=0)) " & _
                               " order by SU_NU9_Status_Code asc "
                Case StatusType.TemplateTaskStatus
                    strSql = "select  Top 1 SU_VC50_Status_Name from T040081 where SU_NU_Range_To>=" & _
                               " (select SU_NU9_Status_Code from T040081 Where SU_VC50_Status_Name=( " & _
                               " Select TTM_VC50_Deve_Status From T050031 Where TTM_NU9_Task_No_PK =" & TaskNumber & " AND  TTM_NU9_Call_No_FK=" & CallNumber & " and TTM_NU9_Comp_Id_FK=" & CompanyID & " ) " & _
                               " and (SU_NU9_CompID=" & CompanyID & " or SU_NU9_CompID=0) ) " & _
                               " order by SU_NU9_Status_Code asc "
            End Select
            Return SQL.Search("Main", "GetStatus-Search", strSql)
        Catch ex As Exception
            CreateLog("GetStatus-1607", "Main Module", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
            Return "NOT FOUND"
        End Try
    End Function
    Public Function CheckCompulsorySettings() As String
        Dim sb As New StringBuilder
        Dim obj As HttpContext
        Dim flg As Boolean = False
        obj = HttpContext.Current
        sb.Append("<U><B>You have following INVALID BROWSER SETTINGS:</U></U> ")
        sb.Append("<BR>")
        sb.Append("<UL>")

        If obj.Request.Browser.Browser.IndexOf("IE") < 0 Then
            sb.Append("<LI>Invalid Browser (Please use Internet Explorer 6.x)")
            sb.Append("<BR>")
            flg = True
        End If
        If obj.Request.Browser.JavaScript() = False Then
            sb.Append("<LI>JavaScript is disabled")
            sb.Append("<BR>")
            flg = True
        End If
        If obj.Request.Browser.Cookies = False Then
            sb.Append("<LI>Cookies are disabled")
            sb.Append("<BR>")
            flg = True
        End If
        If obj.Request.Browser.Frames = False Then
            sb.Append("<LI>HTML Frames are disabled")
            sb.Append("<BR>")
            flg = True
        End If
        If flg = True Then
            sb.Append("</UL>")
            Return sb.ToString
        Else
            Return " "
        End If
    End Function
    'This function is used to show messages panels
    'Author:-Harpreet Singh
    'Date:- 
    'Modified Date:- ----
    Public Function ShowMsgPenel(ByVal cpnlErrorPanel As CustomControls.Web.CollapsiblePanel, ByVal lstError As ListBox, ByVal imgError As System.Web.UI.WebControls.Image, ByVal msgStatus As MSG)

        cpnlErrorPanel.State = CustomControls.Web.PanelState.Expanded
        cpnlErrorPanel.Visible = True
        lstError.Visible = True
        imgError.Visible = True

        lstError.Font.Size = FontUnit.Point(8)
        Dim strPath As String
        If HttpContext.Current.Request.ApplicationPath = "/" Then
            strPath = ""
        Else
            strPath = HttpContext.Current.Request.ApplicationPath
        End If
        If msgStatus = MSG.msgOK Then
            cpnlErrorPanel.Text = "Message..."
            lstError.ForeColor = Color.Black
            imgError.ImageUrl = strPath & "/Images/Pok.gif"
        ElseIf msgStatus = MSG.msgInfo Then
            cpnlErrorPanel.Text = "Message..."
            lstError.ForeColor = Color.Black
            imgError.ImageUrl = strPath & "/Images/info01.jpg"
        ElseIf msgStatus = MSG.msgWarning Then
            cpnlErrorPanel.Text = "Message..."
            lstError.ForeColor = Color.Black
            imgError.ImageUrl = strPath & "/Images/warning.gif"
        Else
            cpnlErrorPanel.Text = "Error Message..."
            lstError.ForeColor = Color.Red
            imgError.ImageUrl = strPath & "/Images/Alert.gif"
        End If
    End Function
    'checking search text is emplty or not
    Function ChechkValidityforSearch(ByVal arrtextBoxes As ArrayList) As Boolean
        Dim intI As Integer
        For intI = 0 To arrtextBoxes.Count - 1
            If arrtextBoxes(intI) <> "" Then
                Return True
            End If
        Next
        Return False
    End Function
    'built query for special characters
    Public Function GetSearchString(ByVal SearchString As String) As String
        Dim strHoldSearch As String
        Dim strReturnFilter As String

        For intI As Integer = 0 To SearchString.Length - 1
            strHoldSearch = SearchString.Substring(intI, 1)

            If strHoldSearch.Equals("'") Then
                strReturnFilter &= strHoldSearch.Replace("'", "''")
            ElseIf strHoldSearch.Equals("[") Then
                strReturnFilter &= strHoldSearch.Replace("[", "[[]")
            ElseIf strHoldSearch.Equals("]") Then
                strReturnFilter &= strHoldSearch.Replace("]", "[]]")
            ElseIf strHoldSearch.Equals("%") Then
                strReturnFilter &= strHoldSearch.Replace("%", "[%]")
            Else
                strReturnFilter &= strHoldSearch
            End If
        Next
        Return strReturnFilter
    End Function
    'This function is used to Logout from WSS.
    'This function for both Virtual directory and websites
    'Author:-Harpreet Singh
    'Date:- 
    'Modified Date:- ----
    Public Function LogoutWSS()
        Try
            FormsAuthentication.SignOut()
            HttpContext.Current.Session.Abandon()
            If HttpContext.Current.Request.ApplicationPath = "/" Then
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UserHostAddress & "/Login/Login.aspx?Logout=1", False)
            Else
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath & "/Login/Login.aspx?Logout=1", False)
            End If

        Catch ex As Exception
            CreateLog("mdlMain", "FillUDCDropDown", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function

    'Function for Encoding/Decoding HTML data for proper display
    'The objData parmeter has the Object datatype, you can pass string,dataset,datatable,dataview
    'to get the Encoded/Decoded output.
    'Author:-Harpreet Singh
    'Date:- 06/11/2006
    'Modified Date:- ----
    Public Function HTMLEncodeDecode(ByVal enuAction As Action, ByRef objData As Object, Optional ByVal DescriptionColumns As Hashtable = Nothing) As Object
        Try
            'This variable hold the input data and returns the output data
            Dim objType As Type
            'get the type of the input data.
            'depending upon the input data type encode/decode the input data and produce output data
            objType = objData.GetType
            Select Case objType.Name
                Case "String"
                    'Encode/Decode the String
                    Dim strTemp As String = ""
                    Select Case enuAction
                        Case Action.Encode
                            strTemp = HttpContext.Current.Server.HtmlEncode(CType(objData, String))
                        Case Action.Decode
                            strTemp = HttpContext.Current.Server.HtmlDecode(CType(objData, String))
                    End Select
                    objData = strTemp
                    Return objData
                Case "DataSet"
                    'Encode/Decode the DataSet
                    Dim dsTemp As DataSet
                    dsTemp = CType(objData, DataSet)
                    'loop through the dataset for all tables
                    For Each dtTemp As DataTable In dsTemp.Tables
                        'loop through the datatable for all row
                        For intI As Integer = 0 To dtTemp.Rows.Count - 1
                            'loop through the rows for all columns
                            For intJ As Integer = 0 To dtTemp.Columns.Count - 1
                                If IsNothing(DescriptionColumns) = False Then
                                    If DescriptionColumns.Count > 0 And DescriptionColumns(dtTemp.Columns(intJ).ColumnName) > 0 Then
                                        If DescriptionColumns.Contains(dtTemp.Columns(intJ).ColumnName) Then
                                            Dim strTempDesc() As String = CStr(dtTemp.Rows(intI).Item(dtTemp.Columns(intJ).ColumnName)).Split(" ")
                                            Dim strDesc As String = ""
                                            For intK As Integer = 0 To strTempDesc.Length - 1
                                                If strTempDesc(intK).Length >= DescriptionColumns(dtTemp.Columns(intJ).ColumnName) Then
                                                    For intW As Integer = 1 To (strTempDesc(intK).Length / DescriptionColumns(dtTemp.Columns(intJ).ColumnName)) + 1
                                                        If intW * DescriptionColumns(dtTemp.Columns(intJ).ColumnName) >= strTempDesc(intK).Length Then
                                                            strTempDesc(intK) = strTempDesc(intK).Insert(intW * DescriptionColumns(dtTemp.Columns(intJ).ColumnName) - 1, Space(1))
                                                        End If
                                                    Next
                                                End If
                                                strDesc &= strTempDesc(intK) & Space(1)
                                            Next
                                            dtTemp.Rows(intI).Item(dtTemp.Columns(intJ).ColumnName) = strDesc.Replace(",", ", ")
                                        End If
                                    End If
                                End If
                                If IsDBNull(dtTemp.Rows(intI).Item(intJ)) = False Then
                                    Select Case enuAction
                                        Case Action.Encode
                                            dtTemp.Rows(intI).Item(intJ) = HttpContext.Current.Server.HtmlEncode(dtTemp.Rows(intI).Item(intJ))
                                        Case Action.Decode
                                            dtTemp.Rows(intI).Item(intJ) = HttpContext.Current.Server.HtmlDecode(dtTemp.Rows(intI).Item(intJ))
                                    End Select
                                End If
                            Next
                        Next
                        'commit the changes to the datatable
                        dtTemp.AcceptChanges()
                    Next
                    'commit the changes to the dataset
                    dsTemp.AcceptChanges()
                    'copy the dataset to the object variable
                    objData = dsTemp
                    'return the dataset
                    Return objData
                Case "DataTable"
                    'Encode/Decode the DataTable
                    Dim dtTemp As DataTable
                    dtTemp = CType(objData, DataTable)
                    'loop through the datatable for all row
                    For intI As Integer = 0 To dtTemp.Rows.Count - 1
                        'loop through the rows for all columns
                        For intJ As Integer = 0 To dtTemp.Columns.Count - 1
                            If IsNothing(DescriptionColumns) = False Then
                                If DescriptionColumns.Count > 0 And DescriptionColumns(dtTemp.Columns(intJ).ColumnName) > 0 Then
                                    If DescriptionColumns.Contains(dtTemp.Columns(intJ).ColumnName) Then
                                        Dim strTempDesc() As String = CStr(dtTemp.Rows(intI).Item(dtTemp.Columns(intJ).ColumnName)).Split(" ")
                                        Dim strDesc As String = ""
                                        For intK As Integer = 0 To strTempDesc.Length - 1
                                            If strTempDesc(intK).Length >= DescriptionColumns(dtTemp.Columns(intJ).ColumnName) Then
                                                For intW As Integer = 1 To CInt(strTempDesc(intK).Length / DescriptionColumns(dtTemp.Columns(intJ).ColumnName)) + 1
                                                    If intW * DescriptionColumns(dtTemp.Columns(intJ).ColumnName) <= strTempDesc(intK).Length Then
                                                        strTempDesc(intK) = strTempDesc(intK).Insert(intW * DescriptionColumns(dtTemp.Columns(intJ).ColumnName) - 1, Space(1))
                                                    End If
                                                Next
                                            End If
                                            strDesc &= strTempDesc(intK) & Space(1)
                                        Next
                                        dtTemp.Rows(intI).Item(dtTemp.Columns(intJ).ColumnName) = strDesc.Replace(",", ", ")
                                    End If
                                End If
                            End If
                            If IsDBNull(dtTemp.Rows(intI).Item(intJ)) = False Then
                                Select Case enuAction
                                    Case Action.Encode
                                        dtTemp.Rows(intI).Item(intJ) = HttpContext.Current.Server.HtmlEncode(dtTemp.Rows(intI).Item(intJ))
                                    Case Action.Decode
                                        dtTemp.Rows(intI).Item(intJ) = HttpContext.Current.Server.HtmlDecode(dtTemp.Rows(intI).Item(intJ))
                                End Select
                            End If
                        Next
                    Next
                    'commit the changes to the datatable
                    dtTemp.AcceptChanges()
                    'copy the datatable to the object variable
                    objData = dtTemp
                    'return the datatable
                    Return objData
                Case "DataView"
                    'Encode/Decode the DataView
                    Dim dvTemp As DataView
                    dvTemp = CType(objData, DataView)
                    'loop through the datatable for all row
                    For intI As Integer = 0 To dvTemp.Table.Rows.Count - 1
                        'loop through the rows for all columns
                        For intJ As Integer = 0 To dvTemp.Table.Columns.Count - 1
                            If IsNothing(DescriptionColumns) = False Then
                                If DescriptionColumns.Count > 0 And DescriptionColumns(dvTemp.Table.Columns(intJ).ColumnName) > 0 Then
                                    If DescriptionColumns.Contains(dvTemp.Table.Columns(intJ).ColumnName) Then
                                        Dim strTempDesc() As String = CStr(dvTemp.Table.Rows(intI).Item(dvTemp.Table.Columns(intJ).ColumnName)).Split(" ")
                                        Dim strDesc As String = ""
                                        For intK As Integer = 0 To strTempDesc.Length - 1
                                            If strTempDesc(intK).Length >= DescriptionColumns(dvTemp.Table.Columns(intJ).ColumnName) Then
                                                For intW As Integer = 1 To CInt(strTempDesc(intK).Length / DescriptionColumns(dvTemp.Table.Columns(intJ).ColumnName)) + 1
                                                    If intW * DescriptionColumns(dvTemp.Table.Columns(intJ).ColumnName) <= strTempDesc(intK).Length Then
                                                        strTempDesc(intK) = strTempDesc(intK).Insert(intW * DescriptionColumns(dvTemp.Table.Columns(intJ).ColumnName) - 1, Space(1))
                                                    End If
                                                Next
                                            End If
                                            strDesc &= strTempDesc(intK) & Space(1)
                                        Next
                                        dvTemp.Table.Rows(intI).Item(dvTemp.Table.Columns(intJ).ColumnName) = strDesc.Replace(",", ", ")
                                    End If
                                End If
                            End If
                            If IsDBNull(dvTemp.Table.Rows(intI).Item(intJ)) = False Then
                                Select Case enuAction
                                    Case Action.Encode
                                        dvTemp.Table.Rows(intI).Item(intJ) = HttpContext.Current.Server.HtmlEncode(dvTemp.Table.Rows(intI).Item(intJ))
                                    Case Action.Decode
                                        dvTemp.Table.Rows(intI).Item(intJ) = HttpContext.Current.Server.HtmlDecode(dvTemp.Table.Rows(intI).Item(intJ))
                                End Select
                            End If
                        Next
                    Next
                    'commit the changes to the dataview
                    dvTemp.Table.AcceptChanges()
                    'copy the dataview to the object variable
                    objData = dvTemp
                    'return the dataview
                    Return objData
            End Select
        Catch ex As Exception
            CreateLog("mdlMain", "HTMLEncodeDecode", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try

    End Function
    'This function produces a new dataview from the existing 
    'dataview by applyng the Filter criteria
    'Author:-Harpreet Singh
    'Create Date:- 06/11/2006
    'Modified Date:- ----
    Public Function GetFilteredDataView(ByRef dvView As DataView, ByVal strFilter As String) As DataView
        Try
            Dim dtOld As New DataTable   'Datatable to hold original dataview
            Dim dtNew As New DataTable     'Datatable to hold new dataview
            Dim drOld() As DataRow    'Array of datarows to hold rows returned by select method of datatable
            dtOld = dvView.Table 'get the datatable from the dataview
            drOld = dtOld.Select(strFilter) 'apply the filter to the datatable and get array of rows
            Dim drNew As DataRow
            'Add the columns to the new datatable from the old datatable.
            For intI As Integer = 0 To dtOld.Columns.Count - 1
                dtNew.Columns.Add(dtOld.Columns(intI).ColumnName, dtOld.Columns(intI).DataType)
            Next
            'loop through the row array to fill the new datatable
            For Each drNew In drOld
                Dim drNew1 As DataRow = dtNew.NewRow
                'copy the original row to a new row that can be added to the new datatable.
                'because the original row is attached with the old data table and 
                'that cannot be added to the new datatable.
                For intJ As Integer = 0 To dtOld.Columns.Count - 1
                    drNew1.Item(intJ) = drNew.Item(intJ)
                Next
                'add the copied row to the new datatable.
                dtNew.Rows.Add(drNew1)
            Next
            'get the view from the new datatable.
            dvView = dtNew.DefaultView
            'return the dataview
            Return dvView
        Catch ex As Exception
            'create log in case of exception
            CreateLog("mdlMain", "GetFilteredDataView", LogType.Application, LogSubType.Exception, "", ex.Message, "", "", "")
        End Try
    End Function

    'This function converts the standard date to Julian format 
    'Author:-
    'Create Date:-
    'Modified Date:- ----

    Public Function DateToJulian(ByVal dtDate As Date) As String
        Try
            Dim strRetDate As String
            strRetDate = "1"
            strRetDate &= dtDate.Year.ToString.Substring(2, 2)
            If (dtDate.DayOfYear.ToString.Length = 2) Then
                strRetDate &= "0" & dtDate.DayOfYear
            ElseIf (dtDate.DayOfYear.ToString.Length = 1) Then
                strRetDate &= "00" & dtDate.DayOfYear
            Else
                strRetDate &= dtDate.DayOfYear
            End If
            Return strRetDate
        Catch ex As Exception
            CreateLog("mdlMain", "DateToJulian-266", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
    'This function converts the Julian date format to standard date format
    'Author:-
    'Create Date:-
    'Modified Date:- ----
    Public Function JulianToDate(ByVal dtInput As String) As Date
        Try
            Dim strJulian As String = dtInput
            Dim intCentury As Integer = CType(strJulian.Substring(0, 1), Integer) + 19
            Dim intYear As Integer = CType(strJulian.Substring(1, 2), Integer)
            Dim yr As Integer = CType(CStr(intCentury) & strJulian.Substring(1, 2), Integer)
            Dim intmonth = CInt(strJulian.Substring(3, 3))
            Dim dtNewDate As Date = "01/01/1900"
            dtNewDate = dtNewDate.AddYears(yr - 1900)
            dtNewDate = dtNewDate.AddDays(intmonth - 1)
            Return dtNewDate
        Catch ex As Exception
            CreateLog("mdlMain", "JulianToDate-282", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    'This function will validate the user id against its validity and enable/disable in T060011
    'Author:-Harpreet Singh
    'Create Date:-20/11/2006
    'Modified Date:- ----
    Public Function CheckUserValiditity(ByVal intABID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        'Dim blnValidate As Boolean
        'Dim strMSG As String
        Dim strSQL As String
        Dim intRows As Integer
        strSQL = "select * from T060011 where UM_VC4_Status_Code_FK='ENB' and UM_IN4_Address_No_FK=" & intABID
        If SQL.Search("", "", strSQL, intRows) = False Then
            stReturn.ErrorMessage = "The user profile of this user is disabled..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 1
            Return stReturn
        End If
        strSQL = "select * from T060011 where isnull(UM_DT8_To_date,'" & Now.Date & "')>='" & Now.Date & "' and UM_DT8_From_date<='" & Now.Date & "'  and UM_IN4_Address_No_FK=" & intABID

        If SQL.Search("", "", strSQL, intRows) = False Then
            stReturn.ErrorMessage = "The user profile of this user is expired..."
            stReturn.FunctionExecuted = False
            stReturn.ErrorCode = 1
            Return stReturn
        End If
        stReturn.FunctionExecuted = True
        stReturn.ErrorCode = 0
        Return stReturn
    End Function

    'This function will check whether a user has any pending task before we  can disable its profile in address book
    'Author:-Harpreet Singh
    'Create Date:-20/11/2006
    'Modified Date:- ----
    Public Function IsUserIDUsed(ByVal intUserID As Integer, ByVal strABType As String) As Boolean
        Try
            Dim blnFound As Boolean = False
            Dim strSQL As String
            Dim sqCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)

            Select Case strABType.ToUpper
                Case "COM"
                    strSQL = "select * from T060011 where UM_IN4_Company_AB_ID=" & intUserID
                Case Else
                    strSQL = "select Top 1 * from T040011 where CM_NU9_Call_Owner=" & intUserID & " and CN_VC20_Call_Status<>'CLOSED';select Top 1 * from T040021 where TM_VC8_Supp_Owner=" & intUserID & " and TM_VC50_Deve_status<>'CLOSED';select Top 1 * from T050021 where TCM_NU9_Call_Owner=" & intUserID & " and TCM_VC20_Call_Status<>'CLOSED';select Top 1 * from T050031 where TTM_VC8_Supp_Owner=" & intUserID & " and TTM_VC50_Deve_status<>'CLOSED'"
            End Select


            If sqCon.State <> ConnectionState.Open Then
                sqCon.Open()
            End If
            Dim dsABNum As New DataSet
            Dim sqADP As New SqlClient.SqlDataAdapter(strSQL, sqCon)
            sqADP.Fill(dsABNum)

            If dsABNum.Tables.Count > 0 Then
                For intI As Integer = 0 To dsABNum.Tables.Count - 1
                    If dsABNum.Tables(intI).Rows.Count > 0 Then
                        blnFound = True
                        Exit For
                    End If
                Next
            End If
            sqCon.Close()
            sqADP.Dispose()
            dsABNum.Dispose()
            Return blnFound
        Catch ex As Exception
            CreateLog("mdlMain", "IsUserIDUsed", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        Finally

        End Try
    End Function
    'This function will check whether a task is dependent on the other or not.
    'Author:-Harpreet Singh
    'Create Date:-21/11/2006
    'Modified Date:- ----

    Public Function CheckTaskDependency(ByVal intCompID As Integer, ByVal intCallNo As Integer, ByVal intTaskNo As Integer) As Boolean
        Try
            Dim strSQL As String
            Dim blnFlag As Boolean = False
            Dim introws As Integer
            strSQL = "select * from t040021 where tm_nu9_call_no_fk=" & intCallNo & " and tm_nu9_comp_id_fk=" & intCompID & " and tm_vc50_deve_status<>'CLOSED' and tm_nu9_task_no_pk=(select tm_nu9_dependency from t040021 where tm_nu9_call_no_fk=" & intCallNo & " and tm_nu9_comp_id_fk=" & intCompID & " and tm_nu9_task_no_pk=" & intTaskNo & " ) "
            blnFlag = SQL.Search("Call_Detail", "SaveAction-2930", strSQL, introws)
            Return blnFlag
        Catch ex As Exception
            CreateLog("mdlMain", "CheckTaskDependency", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try

    End Function

    'This function will check the validity of a SubCategory
    'Author:-Harpreet Singh
    'Create Date:-21/11/2006
    'Modified Date:- ----

    Public Function CheckProjectValidity(ByVal intCompID As Integer, ByVal intProjectID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strSQL As String
            Dim intRows As Integer
            strSQL = "select * from T210011 where PR_NU9_Comp_ID_FK=" & intCompID & " and PR_NU9_Project_ID_Pk=" & intProjectID & " and PR_DT8_Start_Date <=' " & Now.Date & "'"
            If SQL.Search("", "", strSQL, intRows) = False Then
                stReturn.ErrorMessage = "This SubCategory is not Started. Please change SubCategory's Start Date"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                Return stReturn
            End If
            strSQL = "select * from T210011 where PR_NU9_Comp_ID_FK=" & intCompID & " and PR_NU9_Project_ID_Pk=" & intProjectID & " and PR_DT8_Close_Date >=' " & Now.Date & "'"
            If SQL.Search("", "", strSQL, intRows) = False Then
                stReturn.ErrorMessage = "This SubCategory is expired. Please change SubCategory's Close Date"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                Return stReturn
            End If
            stReturn.ErrorCode = 0
            stReturn.FunctionExecuted = True
            Return stReturn
        Catch ex As Exception
            CreateLog("mdlMain", "CheckProjectValidity", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            stReturn.ErrorCode = 2
            stReturn.ErrorMessage = "Server is unable to process your request please try later"
            stReturn.FunctionExecuted = False
            Return stReturn
        End Try
    End Function

    'This function will dropdown using data given by hidden field
    'Author:-Harpreet Singh
    'Create Date:-27/11/2006
    'Modified Date:- ----
    Public Function FillAjaxDropDown(ByVal DDL As DropDownList, ByVal strData As String, ByVal strDDLClientName As String, Optional ByVal FirstDDLListItem As Object = vbNull)
        Try
            If strData <> "" Then
                DDL.Items.Clear()
                If TypeOf FirstDDLListItem Is ListItem Then
                    DDL.Items.Add(FirstDDLListItem)
                End If
                Dim arr() As String
                Dim arrID() As String
                Dim arrName() As String
                arr = strData.Split("~")
                If arr.Length > 0 Then
                    If arr(1).Length > 0 Then
                        arrID = arr(1).Remove(arr(1).Length - 1, 1).Split("^")
                        arrName = arr(0).Remove(arr(0).Length - 1, 1).Split("^")
                        If arrID.Length > 0 Then
                            For intI As Integer = 0 To arrID.Length - 1
                                DDL.Items.Add(New ListItem(arrName(intI), arrID(intI)))
                            Next
                        End If
                    End If
                End If
                DDL.SelectedValue = HttpContext.Current.Request.Form(strDDLClientName)
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "FillAjaxDropDown", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

    'This function will check the validity of an Agreement
    'Author:-Harpreet Singh
    'Create Date:-30/11/2006
    'Modified Date:- ----

    Public Function CheckAgreementValidity(ByVal intCompID As Integer, ByVal intAgreementID As Integer) As ReturnValue
        Dim stReturn As ReturnValue
        Try
            Dim strSQL As String
            Dim intRows As Integer
            strSQL = "select * from T080011 where AG_VC8_Cust_Name=" & intCompID & " and AG_NU8_ID_PK=" & intAgreementID & " and AG_DT_Valid_From <=' " & Now.Date & "'"
            If SQL.Search("", "", strSQL, intRows) = False Then
                stReturn.ErrorMessage = "This Agreement is not Started. Please change Agreement's Valid From Date"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                Return stReturn
            End If
            strSQL = "select * from T080011 where AG_VC8_Cust_Name=" & intCompID & " and AG_NU8_ID_PK=" & intAgreementID & " and AG_DT_Valid_To >=' " & Now.Date & "'"
            If SQL.Search("", "", strSQL, intRows) = False Then
                stReturn.ErrorMessage = "This Agreement is expired. Please change Agreement's Valid Up To Date"
                stReturn.ErrorCode = 1
                stReturn.FunctionExecuted = False
                Return stReturn
            End If
            stReturn.ErrorCode = 0
            stReturn.FunctionExecuted = True
            Return stReturn
        Catch ex As Exception
            CreateLog("mdlMain", "CheckAgreementValidity", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            stReturn.ErrorCode = 2
            stReturn.ErrorMessage = "Server is unable to process your request please try later"
            stReturn.FunctionExecuted = False
            Return stReturn
        End Try
    End Function


    Private Function Encrypt(ByVal Data As String) As String
        Dim shaM As SHA1Managed = New SHA1Managed
        System.Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        '// Getting the bytes of the encrypted data.//
        Dim bytEncrypt() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        '// Converting the byte into string.//
        Dim strEncrypt As String = System.Convert.ToBase64String(bytEncrypt)
        Encrypt = strEncrypt
    End Function

    Private Function Decrypt(ByVal Data As String) As String
        Dim bytData() As Byte = System.Convert.FromBase64String(Data)
        Dim strData As String = ASCIIEncoding.ASCII.GetString(bytData)
        Decrypt = strData
    End Function

    'This function will set the comment flag for Call/Task/Action in whole WSS
    'Author:-Harpreet Singh
    'Create Date:-12/12/2006
    'Modified Date:- ----
    Public Function SetCommentFlag(ByRef objData As Object, ByVal enuLevel As CommentLevel, ByVal CompanyID As Integer, ByVal CallNO As Integer, ByVal TaskNo As Integer, ByVal ActionNo As Integer) As Object
        Try
            'Set the connection string
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'This dataset will store all the comments. Later this DataSet will be used for filtering.
            Dim dsComment As New DataSet
            'This will store the table name for Comment table
            Dim strTableName As String
            'This will store the SQL for comment table
            Dim strSQLComment As String
            'Check if function is called for template or for all other cases
            If enuLevel = CommentLevel.TemplateTaskLevel Then
                'this is for task template
                strTableName = "T050061"
                strSQLComment = "select * from T050061"
            Else
                'else it is for all other call/task/action
                strTableName = "T040061"
                strSQLComment = "select * from T040061"
            End If
            'fill the comment dataset
            If SQL.Search(strTableName, "mdlMain", "SetCommentFlag", strSQLComment, dsComment, "", "") = True Then
                'used to store filter for comment dataset
                Dim strFilter As String
                'used to store Company column name in dataset
                Dim strCompColName As String
                'used to store call column name in dataset
                Dim strCallColName As String
                'used to store task column name in dataset
                Dim strTaskColName As String
                'used to store action column name in dataset
                Dim strActionColName As String
                'used to store comment flag column name in dataset
                Dim strCommentColName As String
                'used to store type of company(SCM/CCMP)
                Dim strCompType As String = HttpContext.Current.Session("PropCompanyType")
                'used ro store temporary dataview given by Filter function
                Dim dvTemp As New DataView
                'used to store comment original dataview
                Dim dvOld As New DataView
                'check if function is called for grid screens or edit screen
                'for grid screens type of objData will be dataview and
                'for edit sceens type of objData will be numeric
                If TypeOf objData Is Integer Or TypeOf objData Is Int16 Or TypeOf objData Is Int32 Or TypeOf objData Is Int64 Or TypeOf objData Is Short Or TypeOf objData Is Long Or TypeOf objData Is Double Then
                    'This part will be used for edit screens
                    Select Case enuLevel
                        'for call detail comment icon
                        Case CommentLevel.CallLevel
                            strFilter = "CM_NU9_CompId_Fk=" & Val(CompanyID) & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0"
                            'for Task edit screen comment icon
                        Case CommentLevel.TaskLevel
                            strFilter = "CM_NU9_CompId_Fk=" & Val(CompanyID) & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=" & Val(TaskNo) & " and CM_NU9_Action_Number=0"
                            'for action edit comment icon
                        Case CommentLevel.ActionLevel
                            strFilter = "CM_NU9_CompId_Fk=" & Val(CompanyID) & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=" & Val(TaskNo) & " and CM_NU9_Action_Number=" & Val(ActionNo)
                            'for template task edit comment icon
                        Case CommentLevel.TemplateTaskLevel
                            strFilter = "CM_NU9_CompId_Fk=" & Val(CompanyID) & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=" & Val(TaskNo) & " and CM_NU9_Action_Number=" & Val(ActionNo)
                    End Select
                    'Save the comment dataset in dvOld dataview
                    dvOld = dsComment.Tables(0).DefaultView
                    'assign dvOld to dvTemp
                    'dvTemp will be used duringgetting filtered dataview
                    dvTemp = dvOld
                    'check if it is a client company then add the External condition in filter
                    If UCase(strCompType) <> "SCM" Then
                        strFilter = strFilter & " and CM_VC50_IE='External'"
                    End If
                    'get the filtered dataview 
                    GetFilteredDataView(dvTemp, strFilter)
                    'If any record found that means there is atleast one comment
                    'else no comment exists
                    If dvTemp.Table.Rows.Count > 0 Then
                        'add the read condition to the filter
                        strFilter = "CM_CH1_Flag='1' and   " & strFilter
                        dvTemp = dvOld
                        'then again get the filtered dataview with new filter
                        GetFilteredDataView(dvTemp, strFilter)
                        'if any record found that mean atleast one comment is unread
                        If dvTemp.Table.Rows.Count > 0 Then
                            'set the flag as unread
                            objData = "2"
                        Else
                            'set the flag as read
                            objData = "1"
                        End If
                    Else
                        'set the flag as no comment
                        objData = "0"
                    End If
                    'return the comment flag
                    Return objData
                Else
                    'This part will be used for grid screens
                    'used to store dataview comming in the form of object from grids
                    Dim dvData As DataView
                    'assign the objData to dataview after converting
                    dvData = CType(objData, DataView)
                    'hashtable used to store all companies with name and id
                    Dim htCompInfo As New Hashtable
                    'dataset for storing comapny info
                    Dim dsCompInfo As New DataSet
                    'get all comapanies with name and id
                    If SQL.Search("T010011", "mdlMain", "SetCommentFlag", "select CI_NU8_Address_Number,CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type='COM'", dsCompInfo, "", "") = True Then
                        'fill the hash table with name and id info of company
                        For intI As Integer = 0 To dsCompInfo.Tables(0).Rows.Count - 1
                            htCompInfo.Add(dsCompInfo.Tables(0).Rows(intI).Item(1), dsCompInfo.Tables(0).Rows(intI).Item(0))
                        Next
                    End If
                    'loop through the grid dataview
                    'check for every record in grid
                    'calculate the comment flag
                    'modifiy the input dataview with the new comment flag
                    For intI As Integer = 0 To dvData.Table.Rows.Count - 1
                        'check for call/task/action/templatetask
                        Select Case enuLevel
                            'for call view grid
                            Case CommentLevel.CallLevel
                                'name cmpany col in data view
                                strCompColName = "CompID"
                                'name of call column in dataview
                                strCallColName = "CallNo"
                                'name of comment flag col in dataview
                                strCommentColName = "C"
                                'create the filter
                                strFilter = "CM_NU9_CompId_Fk=" & htCompInfo(dvData.Table.Rows(intI).Item(strCompColName)) & " and CM_NU9_Call_Number=" & Val(dvData.Table.Rows(intI).Item(strCallColName)) & " and CM_NU9_Task_Number=0 and CM_NU9_Action_Number=0"
                                'for task view grid
                            Case CommentLevel.AllTaskLevel
                                'name cmpany col in data view
                                strCompColName = "CompID"
                                'name of call column in dataview
                                strCallColName = "CallNo"
                                'name of task no col in dataview
                                strTaskColName = "TaskNo"
                                'name of comment flag col in dataview
                                strCommentColName = "C"
                                'create the filter
                                strFilter = "CM_NU9_CompId_Fk=" & htCompInfo(dvData.Table.Rows(intI).Item(strCompColName)) & " and CM_NU9_Call_Number=" & Val(dvData.Table.Rows(intI).Item(strCallColName)) & " and CM_NU9_Task_Number=" & Val(dvData.Table.Rows(intI).Item(strTaskColName)) & " and CM_NU9_Action_Number=0"
                                'for task view grid for particular company and call number
                            Case CommentLevel.TaskLevel
                                'check company session
                                If IsNothing(CompanyID) Then
                                    Exit Function
                                End If
                                'name of task no col
                                strTaskColName = "TM_NU9_Task_no_PK"
                                'name of comment flag col
                                strCommentColName = "Blank1"
                                'create the filter
                                strFilter = "CM_NU9_CompId_Fk=" & Val(CompanyID) & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=" & Val(dvData.Table.Rows(intI).Item(strTaskColName)) & " and CM_NU9_Action_Number=0"
                                'for action grid
                            Case CommentLevel.ActionLevel
                                'check company session
                                If IsNothing(CompanyID) Then
                                    Exit Function
                                End If
                                'name action no col
                                strActionColName = "AM_NU9_Action_Number"
                                'name of comment flag col
                                strCommentColName = "Blank1"
                                'create the filter
                                strFilter = "CM_NU9_CompId_Fk=" & Val(CompanyID) & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=" & Val(TaskNo) & " and CM_NU9_Action_Number=" & Val(dvData.Table.Rows(intI).Item(strActionColName))
                            Case CommentLevel.TemplateTaskLevel
                                'check company session
                                If IsNothing(CompanyID) Then
                                    Exit Function
                                End If
                                'name of task no col
                                strTaskColName = "TTM_NU9_Task_no_PK"
                                'name of comment flag col
                                strCommentColName = "Blank1"
                                'create the filter
                                strFilter = "CM_NU9_CompId_Fk=" & CompanyID & " and CM_NU9_Call_Number=" & Val(CallNO) & " and CM_NU9_Task_Number=" & Val(dvData.Table.Rows(intI).Item(strTaskColName)) & " and CM_NU9_Action_Number=0"
                        End Select
                        'Save the comment dataset in dvOld dataview
                        dvOld = dsComment.Tables(0).DefaultView
                        'assign dvOld to dvTemp
                        'dvTemp will be used duringgetting filtered dataview
                        dvTemp = dvOld
                        'check if it is a client company then add the External condition in filter
                        If UCase(strCompType) <> "SCM" Then
                            strFilter = strFilter & " and CM_VC50_IE='External'"
                        End If
                        'get the filtered dataview 
                        GetFilteredDataView(dvTemp, strFilter)
                        'If any record found that means there is atleast one comment
                        'else no comment exists
                        If dvTemp.Table.Rows.Count > 0 Then
                            'add the read condition to the filter
                            strFilter = "CM_CH1_Flag='1' and   " & strFilter
                            dvTemp = dvOld
                            'then again get the filtered dataview with new filter
                            GetFilteredDataView(dvTemp, strFilter)
                            'if any record found that mean atleast one comment is unread
                            If dvTemp.Table.Rows.Count > 0 Then
                                'set the flag as unread
                                dvData.Table.Rows(intI).Item(strCommentColName) = "2"
                            Else
                                'set the flag as read
                                dvData.Table.Rows(intI).Item(strCommentColName) = "1"
                            End If
                        Else
                            'set the flag as no comment
                            dvData.Table.Rows(intI).Item(strCommentColName) = "0"
                        End If
                    Next
                    'commit the changes done to the dataview
                    dvData.Table.AcceptChanges()
                    'return the modified dataview with actual comment flag value
                    Return dvData
                End If
            End If
        Catch ex As Exception
            'create the log of exception if any
            CreateLog("mdlMain", "SetCommentFlag", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Public Function SetControlFocus(ByVal Control As System.Web.UI.Control)
        Try
            Dim strScript As String
            strScript = "<script language='javascript'>document.getElementById('" & Control.ClientID & "').focus();</script>"
            Control.Page.RegisterStartupScript("FocusScript", strScript)
        Catch ex As Exception
            CreateLog("mdlMain", "SetControlFocus", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    'This function will check the attachment
    'Author:-Harpreet Singh
    'Create Date:-15/05/2007
    'Modified Date:- ----
    Public Function ChangeAttachmentToolTip(ByVal CompanyID As Integer, ByVal CallNo As Integer) As Boolean
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Dim intAttach As Integer = 0
            SQL.Search("", "", "(select CM_NU8_Attach_No from T040011 where CM_NU9_Call_No_PK=" & Val(CallNo) & " and CM_NU9_Comp_Id_FK=" & Val(CompanyID) & " and CM_NU8_Attach_No is not null)union(select TM_CH1_Attachment from T040021 where TM_NU9_Call_No_FK=" & Val(CallNo) & " and TM_NU9_Comp_ID_FK=" & Val(CompanyID) & " and TM_CH1_Attachment is not null)union(select AM_CH1_Attachment from T040031 where AM_NU9_Call_Number=" & Val(CallNo) & " and AM_NU9_Comp_ID_FK=" & Val(CompanyID) & " and AM_CH1_Attachment is not null)", intAttach, "")
            If intAttach > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "ChangeAttachmentToolTip", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
    Public Enum IsTime
        WithTime = 1
        DateOnly = 2
    End Enum
    'This function will set date format
    'Author:-Harpreet Singh
    'Create Date:-16/05/2007
    'Modified Date:- ----
    Public Function SetDateFormat(ByVal InputDateTime As DateTime, Optional ByVal EnIsTime As IsTime = IsTime.DateOnly) As String
        Try
            If IsDate(InputDateTime) Then
                Select Case EnIsTime
                    Case IsTime.DateOnly
                        Return Format(InputDateTime, HttpContext.Current.Session("PropDataFormat"))
                    Case IsTime.WithTime
                        Return Format(InputDateTime, HttpContext.Current.Session("PropDataTimeFormat"))
                End Select
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "SetDateFormat", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try

    End Function

    'This function will set date format in a data table
    'Author:-Harpreet Singh
    'Create Date:-16/05/2007
    'Modified Date:- ----

    Public Function SetDataTableDateFormat(ByRef InputDataTable As DataTable, ByVal DateColumns As Hashtable)
        Try
            If DateColumns.Count > 0 Then
                For intI As Integer = 0 To InputDataTable.Rows.Count - 1
                    For intJ As Integer = 0 To InputDataTable.Columns.Count - 1
                        If DateColumns.Contains(InputDataTable.Columns(intJ).ColumnName) Then
                            If IsDate(InputDataTable.Rows(intI).Item(intJ)) Then
                                Select Case CType(DateColumns.Item(InputDataTable.Columns(intJ).ColumnName), IsTime)
                                    Case IsTime.DateOnly
                                        InputDataTable.Rows(intI).Item(intJ) = Format(CDate(InputDataTable.Rows(intI).Item(intJ)), HttpContext.Current.Session("PropDataFormat"))
                                    Case IsTime.WithTime
                                        InputDataTable.Rows(intI).Item(intJ) = Format(CDate(InputDataTable.Rows(intI).Item(intJ)), HttpContext.Current.Session("PropDataTimeFormat"))
                                End Select
                            End If
                        End If
                    Next
                Next
                InputDataTable.AcceptChanges()
            End If
        Catch ex As Exception
            CreateLog("mdlMain", "SetDataTableDateFormat", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
    Public Enum EnumTaskOrder
        UpdateTask = 1
        DeleteTask = 2
    End Enum
    'This function will set TaskOrder of all task under a Call
    'Author:-Harpreet Singh
    'Create Date:-21/05/2007
    'Modified Date:- ----
    Public Function ChangeTaskOrder(ByVal enuChange As EnumTaskOrder, ByVal OldTaskOrder As Integer, ByVal CompanyID As Integer, ByVal CallNo As Integer, Optional ByVal NewTaskOrder As Integer = 0) As Boolean
        Try
            Dim strSQL As String
            Dim objCon As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
            Dim objADP As SqlClient.SqlDataAdapter
            Dim dsTaskOrder As New DataSet
            Select Case enuChange
                Case EnumTaskOrder.DeleteTask
                    strSQL = "select * from T040021  where TM_NU9_Comp_ID_FK=" & CompanyID & " and TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Task_Order>" & OldTaskOrder & " order by TM_NU9_Task_Order asc"
                    objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                    objADP.Fill(dsTaskOrder, "T040021")
                    Dim intC As Integer = OldTaskOrder
                    For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                        dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") = intC
                        intC += 1
                    Next

                Case EnumTaskOrder.UpdateTask
                    If NewTaskOrder > OldTaskOrder Then
                        strSQL = "select * from T040021 where TM_NU9_Comp_ID_FK=" & CompanyID & " and TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Task_Order>" & OldTaskOrder & " and TM_NU9_Task_Order<=" & NewTaskOrder & " order by TM_NU9_Task_Order asc"
                        objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                        objADP.Fill(dsTaskOrder, "T040021")
                        Dim intC As Integer = OldTaskOrder
                        For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                            dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") = intC 'dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") - 1
                            intC += 1
                        Next
                    ElseIf NewTaskOrder < OldTaskOrder Then
                        strSQL = "select * from T040021 where TM_NU9_Comp_ID_FK=" & CompanyID & " and TM_NU9_Call_No_FK=" & CallNo & " and TM_NU9_Task_Order>=" & NewTaskOrder & " and TM_NU9_Task_Order<" & OldTaskOrder & " order by TM_NU9_Task_Order asc"
                        objADP = New SqlClient.SqlDataAdapter(strSQL, objCon)
                        objADP.Fill(dsTaskOrder, "T040021")
                        Dim intC As Integer = NewTaskOrder + 1
                        For intI As Integer = 0 To dsTaskOrder.Tables(0).Rows.Count - 1
                            dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") = intC 'dsTaskOrder.Tables(0).Rows(intI).Item("TM_NU9_Task_Order") + 1
                            intC += 1
                        Next
                    End If
            End Select

            Dim DatasetChanges = dsTaskOrder.GetChanges
            If Not IsNothing(DatasetChanges) Then
                Dim objCMDBldr As New SqlClient.SqlCommandBuilder(objADP)
                objADP.Update(dsTaskOrder, "T040021")
                objCMDBldr.Dispose()
                objADP.Dispose()
            End If
            objCon.Dispose()
        Catch ex As Exception
            CreateLog("mdlMain", "ChangeTaskOrder-2342", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function

    Public Function GetCompanySubQuery() As String
        Try
            Return "(select UC_NU9_Comp_ID_FK CompID from T060041, T010011  where UC_NU9_Comp_ID_FK = CI_NU8_Address_Number AND CI_VC8_Status='ENA' and UC_NU9_User_ID_FK=" & Val(HttpContext.Current.Session("PropUserID")) & " and UC_BT1_Access=1 ) union (select 0 CompID)"
        Catch ex As Exception
            CreateLog("mdlMain", "GetCompanySubQuery-2395", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
    '--Ranvijay for AB
    ' -- get managername by passing managerid
    Public Function GetManagerName(ByVal ManagerID As Integer) As String
        Dim Value As String
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            Value = SQL.Search("", "", "select CI_VC36_Name from t010011 where CI_NU8_address_number=" & ManagerID)
        Catch ex As Exception
            CreateLog("mdlMain", "GetManagerName", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
        Return Value
    End Function
    '-- Ranvijay for userprofile
    '-- get udc description from udcname
    Public Function GetUDCDescription(ByVal UDCType As String, ByVal UDCName As String, Optional ByVal Company As Integer = 0) As String
        Dim UDCDesc As String = ""
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            ' -- search from default company as well as specified company. If both companies have value then get from sepecific company thats why using ordering and top 1.
            UDCDesc = SQL.Search("", "", "Select top 1 Description from UDC Where ProductCode=0 and (company=0 or Company=" & Company & ") And UDCType='" & UDCType & "' And Name='" & UDCName & "' Order By Company desc")
        Catch ex As Exception
            CreateLog("mdlMain", "GetUDCDescription", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
        Return UDCDesc
    End Function


    'Public Function ShowMsgPenelNew(ByVal pnlMsg As Panel, ByVal lstErrorList As ListBox, ByVal msgStatus As MSG)

    '    ' ShowMsgPenel(cpnlError, lstErrorList, ImgError, msgStatus)


    '    Dim strPath1 As String
    '    If HttpContext.Current.Request.ApplicationPath = "/" Then
    '        strPath1 = "../../"
    '    Else
    '        strPath1 = HttpContext.Current.Request.ApplicationPath
    '    End If

    '    Dim strPath As String = HttpContext.Current.Request.Url.Scheme & Uri.SchemeDelimiter & HttpContext.Current.Request.Url.Authority & strPath1
    '    Dim strHeaderColor As String
    '    Dim strMsgColor As String
    '    Dim strMessage As String
    '    Dim strSubject As String
    '    Dim strImagePath As String

    '    Select Case msgStatus
    '        Case mdlMain.MSG.msgOK
    '            strHeaderColor = "Green"
    '            strMsgColor = "Black"
    '            strSubject = "OK Message..."
    '            strImagePath = strPath & "/Images/Pok.gif"
    '        Case mdlMain.MSG.msgInfo
    '            strHeaderColor = "DarkBlue"
    '            strMsgColor = "Black"
    '            strSubject = "Info Message..."
    '            strImagePath = strPath & "/Images/info01.jpg"
    '        Case mdlMain.MSG.msgWarning
    '            strHeaderColor = "DarkBlue"
    '            strMsgColor = "Black"
    '            strSubject = "Warning Message..."
    '            strImagePath = strPath & "/Images/warning.gif"
    '        Case mdlMain.MSG.msgError
    '            strHeaderColor = "Red"
    '            strMsgColor = "Red"
    '            strSubject = "Error Message..."
    '            strImagePath = strPath & "/Images/Alert.gif"
    '    End Select

    '    'strImagePath = "WSS%205.1%20Web/Images/Images/Alert.gif"
    '    strSubject = "<font size=1 color=" & strHeaderColor & " face=verdana><b>" & strSubject & "</b></font>"
    '    strMessage = "<table align=left width=100% border=0 cellpadding=0 cellspacing=0>"
    '    '''''''''commented because images were not coming..''''''

    '    'strMessage &= "<tr valign=top><td valign=center rowspan=" & lstErrorList.Items.Count + 1 & " width=20px><img src=""" & HttpContext.Current.Server.MapPath(strImagePath) & """></td><td colspan=2></td></tr>"
    '    strMessage &= "<tr valign=top><td valign=top align=left rowspan=" & lstErrorList.Items.Count + 1 & " width=20px><img src=""" & strImagePath & """></td><td colspan=2></td></tr>"
    '    For intI As Integer = 0 To lstErrorList.Items.Count - 1
    '        strMessage &= "<tr valign=top><td width=20px align=left><font size=1 color=" & strMsgColor & " face=verdana><b>" & (intI + 1).ToString & ".</B></font></td><td align=left><font size=1 color=" & strMsgColor & " face=verdana><b>" & lstErrorList.Items(intI).Text & "</b></font></td></tr>"
    '    Next
    '    strMessage &= "</table>"

    '    Dim popMsg As New ION.Web.PopupWin
    '    popMsg.HideAfter = 14000
    '    popMsg.ShowAfter = 0

    '    popMsg.Visible = True
    '    popMsg.OffsetY = 0
    '    popMsg.OffsetX = 0
    '    popMsg.Width = Unit.Pixel(300)
    '    Select Case lstErrorList.Items.Count
    '        Case 1
    '            popMsg.Height = Unit.Pixel(60)
    '        Case 2
    '            popMsg.Height = Unit.Pixel(70)
    '        Case 3
    '            popMsg.Height = Unit.Pixel(80)
    '        Case 4
    '            popMsg.Height = Unit.Pixel(90)
    '        Case 5
    '            popMsg.Height = Unit.Pixel(100)
    '        Case 6
    '            popMsg.Height = Unit.Pixel(110)
    '        Case 7
    '            popMsg.Height = Unit.Pixel(120)
    '        Case 8
    '            popMsg.Height = Unit.Pixel(130)
    '        Case 9
    '            popMsg.Height = Unit.Pixel(140)
    '        Case Else
    '            popMsg.Height = Unit.Pixel(120)
    '    End Select

    '    popMsg.PopupSpeed = 10
    '    popMsg.DragDrop = True
    '    popMsg.DockMode = ION.Web.PopupDocking.BottomRight
    '    popMsg.ColorStyle = ION.Web.PopupColorStyle.Green
    '    popMsg.Title = strSubject
    '    popMsg.Message = strMessage
    '    popMsg.ShowLink = False
    '    pnlMsg.Controls.Clear()
    '    pnlMsg.Controls.Add(popMsg)
    'End Function
    Public Function ShowMsgPenelNew(ByVal pnlMsg As Panel, ByVal lstErrorList As ListBox, ByVal msgStatus As MSG)

        ' ShowMsgPenel(cpnlError, lstErrorList, ImgError, msgStatus)


        Dim strPath1 As String
        If HttpContext.Current.Request.ApplicationPath = "/" Then
            strPath1 = "../../"
        Else
            strPath1 = HttpContext.Current.Request.ApplicationPath
        End If

        Dim strPath As String = HttpContext.Current.Request.Url.Scheme & Uri.SchemeDelimiter & HttpContext.Current.Request.Url.Authority & strPath1
        Dim strHeaderColor As String
        Dim strMsgColor As String
        Dim strMessage As New StringBuilder
        Dim strSubject As String
        Dim strImagePath As String
        Dim radTooltipWindow As New Telerik.Web.UI.RadToolTip
        Select Case msgStatus
            Case mdlMain.MSG.msgOK
                strHeaderColor = "Green"
                strMsgColor = "Black"
                strSubject = "OK Message..."
                strImagePath = strPath & "/Images/Pok.gif"
            Case mdlMain.MSG.msgInfo
                strHeaderColor = "DarkBlue"
                strMsgColor = "Black"
                strSubject = "Info Message..."
                strImagePath = strPath & "/Images/info01.jpg"
                radTooltipWindow.Modal = True
                radTooltipWindow.HideEvent = Telerik.Web.UI.ToolTipHideEvent.ManualClose
            Case mdlMain.MSG.msgWarning
                strHeaderColor = "DarkBlue"
                strMsgColor = "Black"
                strSubject = "Warning Message..."
                strImagePath = strPath & "/Images/warning.gif"
                radTooltipWindow.Modal = True
                radTooltipWindow.HideEvent = Telerik.Web.UI.ToolTipHideEvent.ManualClose
            Case mdlMain.MSG.msgError
                strHeaderColor = "Red"
                strMsgColor = "Red"
                strSubject = "Error Message..."
                strImagePath = strPath & "/Images/Alert.gif"
                radTooltipWindow.Modal = True
                radTooltipWindow.HideEvent = Telerik.Web.UI.ToolTipHideEvent.ManualClose
        End Select

        'strImagePath = "WSS%205.1%20Web/Images/Images/Alert.gif"
        strSubject = "<font size=1 color=" & strHeaderColor & " face=verdana><b>" & strSubject & "</b></font>"
        strMessage.Append("<table align=left width=100% border=0 cellpadding=0 cellspacing=0 style=background-color:#C8D8EF>")
        '''''''''commented because images were not coming..''''''
        strMessage.Append("<tr valign=top  style=height:20px><td background=" & strPath & "/Images/top_nav_back.gif valign=middle align=left colspan=3>" & strSubject & "<td><tr>")
        strMessage.Append("<tr valign=top height=10px><td valign=top align=left colspan=3><td><tr>")

        'strMessage &= "<tr valign=top><td valign=center rowspan=" & lstErrorList.Items.Count + 1 & " width=20px><img src=""" & HttpContext.Current.Server.MapPath(strImagePath) & """></td><td colspan=2></td></tr>"
        strMessage.Append("<tr valign=top><td valign=top align=left rowspan=" & lstErrorList.Items.Count + 1 & " width=20px><img src=""" & strImagePath & """></td><td colspan=2></td></tr>")
        For intI As Integer = 0 To lstErrorList.Items.Count - 1
            strMessage.Append("<tr valign=top><td width=20px align=left><font size=1 color=" & strMsgColor & " face=verdana><b>" & (intI + 1).ToString & ".</B></font></td><td align=left><font size=1 color=" & strMsgColor & " face=verdana><b>" & lstErrorList.Items(intI).Text & "</b></font></td></tr>")
        Next
        strMessage.Append("<tr valign=top height=10px><td valign=top align=left colspan=3><td><tr>")
        strMessage.Append("</table>")

        radTooltipWindow.Width = Unit.Pixel(300)
        'radTooltipWindow.Title = strSubject
        radTooltipWindow.Text = strMessage.ToString()
        radTooltipWindow.ShowCallout = False
        radTooltipWindow.OffsetX = 0
        radTooltipWindow.OffsetY = 0
        radTooltipWindow.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.BrowserWindow
        radTooltipWindow.Position = Telerik.Web.UI.ToolTipPosition.BottomRight
        radTooltipWindow.Animation = Telerik.Web.UI.ToolTipAnimation.Slide
        radTooltipWindow.IsClientID = True
        radTooltipWindow.AutoCloseDelay = 7000
        'radTooltipWindow.BorderStyle = BorderStyle.Solid
        'radTooltipWindow.BorderColor = System.Drawing.ColorTranslator.FromHtml("#CEDFF1")
        'radTooltipWindow.BorderWidth = Unit.Pixel(6)
        radTooltipWindow.BackColor = System.Drawing.ColorTranslator.FromHtml("#83ADFF")


        'Dim popMsg As New ION.Web.PopupWin
        'popMsg.HideAfter = 14000
        'popMsg.ShowAfter = 0

        'popMsg.Visible = True
        'popMsg.OffsetY = 0
        'popMsg.OffsetX = 0
        'popMsg.Width = Unit.Pixel(300)
        'Select Case lstErrorList.Items.Count
        '    Case 1
        '        radTooltipWindow.Height = Unit.Pixel(60)
        '    Case 2
        '        radTooltipWindow.Height = Unit.Pixel(70)
        '    Case 3
        '        radTooltipWindow.Height = Unit.Pixel(80)
        '    Case 4
        '        radTooltipWindow.Height = Unit.Pixel(90)
        '    Case 5
        '        radTooltipWindow.Height = Unit.Pixel(100)
        '    Case 6
        '        radTooltipWindow.Height = Unit.Pixel(110)
        '    Case 7
        '        radTooltipWindow.Height = Unit.Pixel(120)
        '    Case 8
        '        radTooltipWindow.Height = Unit.Pixel(130)
        '    Case 9
        '        radTooltipWindow.Height = Unit.Pixel(140)
        '    Case Else
        '        radTooltipWindow.Height = Unit.Pixel(120)
        'End Select

        'popMsg.PopupSpeed = 10
        'popMsg.DragDrop = True
        'popMsg.DockMode = ION.Web.PopupDocking.BottomRight
        'popMsg.ColorStyle = ION.Web.PopupColorStyle.Green
        'popMsg.Title = strSubject
        'popMsg.Message = strMessage
        'popMsg.ShowLink = False
        pnlMsg.Controls.Clear()
        pnlMsg.Controls.Add(radTooltipWindow)
        radTooltipWindow.Show()
        Return True
    End Function
    Public Function ChkMembersInSubcategory(ByVal intTaskOwner As Integer, ByVal intSubcategory As Integer, ByVal intCompId As Integer) As Boolean
        Try

            Dim strSQL As String
            Dim intRows As Integer
            strSQL = "Select pm_nu9_project_member_id from T210012 where PM_NU9_Comp_ID_FK=" & intCompId & " and pm_nu9_project_id_fk =" & intSubcategory & " and pm_nu9_project_member_id=" & intTaskOwner & ""

            If SQL.Search("T210012", "ChkTaskOwnerInSubcategory-2625", strSQL, intRows) = False Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
        End Try

    End Function
    Public Function GetDefaultUserForTemplate(ByVal intSubCategory As Integer, ByVal intCompId As Integer) As Integer

        Try

            Dim intDefaultTaskOwnerID As Integer
            intDefaultTaskOwnerID = SQL.Search("ProjectMasterDetail", "SaveProjectInfo", "Select top 1  pm_nu9_project_member_id From T210012 Where  PM_NU9_Project_ID_Fk=" & intSubCategory & " And PM_NU9_Comp_ID_FK=" & intCompId)

            If intDefaultTaskOwnerID > 0 Then
                Return intDefaultTaskOwnerID
            Else
                Return 0
            End If

        Catch ex As Exception
        End Try

    End Function
#Region "Side Menu"
    Dim dsMnu As DataSet  '--ds
    Dim dvMnu As DataView ' -dvskill
    Dim dsSideMnu As DataSet '--dsSkill
    Dim dvSM As DataView '--dvscr
    Public Sub CreateSideMenu(ByVal trvMenu As TreeView)
        Try
            'Dim db As Database = DatabaseFactory.CreateDatabase()

            Dim sqlCommand As String = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID,OBM.OBM_VC50_Object_Name Name, ROD_VC50_Alias_Name as AName, OBM.OBM_IN4_Object_PID_FK as ObjectPID, OBM.OBM_VC200_URL as PageURL,  OBM_VC200_Image as ImageURL, OBM.OBM_VC4_Object_Type_FK ObjType, OBM_SI2_Order_By, ROD_CH1_Enable_Disable as ED,ROD.ROD_CH1_View_Hide as VH,  OBM.OBM_VC4_Object_Type_FK ObjectType,OBM.OBM_VC50_Grid_Name as GName  from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA  WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND  UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND  RA.RA_VC4_Status_Code = 'ENB' AND  RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND  ROM.ROM_VC50_Status_Code_FK = 'ENB' AND  (((OBM.OBM_VC4_Object_Type_FK ='MNU' and OBM.OBM_VC4_Object_Type_FK ='SCR' ) and (ROD.ROD_CH1_View_Hide = 'H' or ROD.ROD_CH1_Enable_Disable = 'D')) OR  ((OBM.OBM_VC4_Object_Type_FK ='MNU' or OBM.OBM_VC4_Object_Type_FK ='SCR' ) and (ROD.ROD_CH1_View_Hide <> 'H'))) and ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND  OBM.OBM_VC4_Status_Code_FK = 'ENB' AND  OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and  ROM_IN4_Role_ID_PK = " & HttpContext.Current.Session("PropRole") & "  order by OBM.OBM_SI2_Order_By"

            'Dim dbcmd As DbCommand = db.GetSqlStringCommand(sqlCommand)
            'ds = db.ExecuteDataSet(dbcmd)
            'ION.Data.SQL.GetConncetionString("")
            dsMnu = New DataSet
            ION.Data.SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            ION.Data.SQL.DBTracing = False
            ION.Data.SQL.Search("T070011", "", "", sqlCommand, dsMnu, "", "")
            CreateTreeMenu(trvMenu)
        Catch ex As Exception
            'HRMS_BLL.HRMSGeneric.LogError("MdlMain", "CreateSideMenu", ex)
        End Try

    End Sub
    Private Function CreateTreeMenu(ByVal trvmenu As TreeView) As Boolean
        Try
            trvmenu.Nodes.Clear()
            Dim trParentNode As TreeNode
            dsSideMnu = dsMnu
            dsMnu.Dispose()
            dvMnu = New DataView
            dvMnu = GetFilteredDataView1(dsSideMnu.Tables(0).DefaultView, " ObjectPID is null or ObjectPID =0", "OBM_SI2_Order_By")
            For intI As Integer = 0 To dvMnu.Table.Rows.Count - 1
                dvSM = New DataView
                dvSM = GetFilteredDataView1(dsSideMnu.Tables(0).DefaultView, " ObjectPID=" & dvMnu.Table.Rows(intI).Item("ObjectID"), "OBM_SI2_Order_By")
                trParentNode = New TreeNode
                trParentNode.Text = dvMnu.Table.Rows(intI).Item("Name")
                trParentNode.SelectAction = TreeNodeSelectAction.Expand
                'If Not dvMnu.Table.Rows(intI).Item("OBM_VC200_URL") Is DBNull.Value Then
                '    trParentNode.NavigateUrl = dvMnu.Table.Rows(intI).Item("OBM_VC200_URL")
                'End If

                'If Not dvMnu.Table.Rows(intI).Item("OBM_VC200_URL") Is DBNull.Value Then
                trParentNode.Value = dvMnu.Table.Rows(intI).Item("ObjectID")
                trParentNode.ToolTip = dvMnu.Table.Rows(intI).Item("Name")
                AddChild(trParentNode, dvMnu.Table.Rows(intI).Item("ObjectID"), dsSideMnu.Tables(0).DefaultView)
                trvmenu.Nodes.Add(trParentNode)
                'trvmenu.ExpandAll()
            Next
        Catch ex As Exception
            'HRMS_BLL.HRMSGeneric.LogError("MdlMain", "CreateTreeMenu", ex)
        End Try


    End Function

    Private Sub AddChild(ByRef trvParentNode As TreeNode, ByVal ParentID As Integer, ByVal dvChilds As DataView)
        Dim trvChildNode As TreeNode
        Dim dvLast As DataView
        Dim intL As Integer
        Try
            dvLast = GetFilteredDataView1(dvChilds, "ObjectPID=" & ParentID, "OBM_SI2_Order_By")
            For intL = 0 To dvLast.Table.Rows.Count - 1
                trvChildNode = New TreeNode
                trvChildNode.Text = dvLast.Table.Rows(intL).Item("Name")
                trvChildNode.Value = dvLast.Table.Rows(intL).Item("ObjectID")
                trvChildNode.ToolTip = dvLast.Table.Rows(intL).Item("Name")
                If Not dvLast.Table.Rows(intL).Item("PageURL") Is DBNull.Value Then
                    trvChildNode.NavigateUrl = dvLast.Table.Rows(intL).Item("PageURL") ' & "?scrID=" & dvLast.Table.Rows(intL).Item("OBM_IN4_Object_ID_PK")
                End If

                AddChild(trvChildNode, dvLast.Table.Rows(intL).Item("ObjectID"), dsSideMnu.Tables(0).DefaultView)
                trvParentNode.ChildNodes.Add(trvChildNode)
            Next
        Catch ex As Exception
            'HRMS_BLL.HRMSGeneric.LogError("MdlMain", "AddChild", ex)
        End Try
    End Sub

    Public Function GetFilteredDataView1(ByVal dvView As DataView, ByVal strFilter As String, ByVal strSort As String) As DataView
        Try
            Dim dtOld As New DataTable
            Dim dtNew As New DataTable
            Dim drOld() As DataRow
            dtOld = dvView.Table
            If String.IsNullOrEmpty(strFilter) Then
                If String.IsNullOrEmpty(strSort) Then
                    Return dvView
                End If
            End If
            If String.IsNullOrEmpty(strSort) Then
                If String.IsNullOrEmpty(strFilter) Then
                    Return dvView
                End If
            End If
            drOld = dtOld.Select(strFilter, strSort)
            Dim drNew As DataRow
            For intI As Integer = 0 To dtOld.Columns.Count - 1
                dtNew.Columns.Add(dtOld.Columns(intI).ColumnName, dtOld.Columns(intI).DataType)
            Next
            For Each drNew In drOld
                Dim drNew1 As DataRow = dtNew.NewRow
                For intJ As Integer = 0 To dtOld.Columns.Count - 1
                    drNew1.Item(intJ) = drNew.Item(intJ)
                Next
                dtNew.Rows.Add(drNew1)
            Next
            dvView = dtNew.DefaultView
            Return dvView
        Catch ex As Exception
            'HRMS_BLL.HRMSGeneric.LogError("MdlMain", "GetFilteredDataView", ex)
            Return Nothing
        End Try
    End Function
#End Region

End Module
