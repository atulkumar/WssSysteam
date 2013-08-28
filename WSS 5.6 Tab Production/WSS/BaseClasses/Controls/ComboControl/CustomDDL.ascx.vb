'************************************************************************************************************
' Page                 : - DropDown Control
' Purpose              : -Custom dropdown control with more option.  
' Date					Author						Modification Date					Description
' 27/04/06			Harpreet					16/11/20006					Created
'
' Notes: 
' Code:
'************************************************************************************************************

Imports ION.Data
Imports System.Web.Security
Imports System.Data
Imports ION.Logging.EventLogging

Partial Class CustomDDL
    Inherits System.Web.UI.UserControl

#Region "-------- Control Variables-------"

    '    Private arList As New ArrayList

    Private Name As String
    'Private strPopUpURL As String = "../../Search/Common/PopSearch1.aspx"
    Private strPopUpName As String
    Private strPopUpWidth As String
    Private strPopUpHeight As String
    Private strQuery As String
    Private blnUDC As Boolean = True
    Private blnMandatory As Boolean
    Protected WithEvents txtHIDPopupWidth As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtHIDPopupHeight As System.Web.UI.HtmlControls.HtmlInputHidden
    Private blnAutoPostBack As Boolean = False

    'Private blnEnabled As Boolean
    Public Enum DDLType
        FastEntry = 1
        NonFastEntry = 2
    End Enum

#End Region

#Region "-----------Page laod-------------"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            If txtHIDUDC.Value = "1" Then
                blnUDC = True
            Else
                blnUDC = False
            End If
            If blnAutoPostBack = False Then

                If blnUDC = False Then
                    DDL.Attributes.Add("OnChange", "SelectDDL('" & Me.ClientID & "','" & CreatePopupURL() & "', '" & Me.ClientID & "', '" & strPopUpWidth & "', '" & strPopUpHeight & "',0,0);")
                Else
                    DDL.Attributes.Add("OnChange", "SelectDDL('" & Me.ClientID & "','" & CreatePopupURL() & "', '" & Me.ClientID & "', '" & strPopUpWidth & "', '" & strPopUpHeight & "',1,0);")
                End If

            Else
                If blnUDC = False Then
                    DDL.Attributes.Add("OnChange", "SelectDDL('" & Me.ClientID & "','" & CreatePopupURL() & "', '" & Me.ClientID & "', '" & strPopUpWidth & "', '" & strPopUpHeight & "',0,1);")
                Else
                    DDL.Attributes.Add("OnChange", "SelectDDL('" & Me.ClientID & "','" & CreatePopupURL() & "', '" & Me.ClientID & "', '" & strPopUpWidth & "', '" & strPopUpHeight & "',1,1);")
                End If

            End If
            If IsPostBack Then
                CDDLSetItem()
            End If
            '  DDL.DataSource = arList
            '  DDL.DataBind()
        Catch ex As Exception
            CreateLog("CDDL", "Load" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        End Try
    End Sub
#End Region

#Region "-----------Properties------------"

    Public Property CDDLAutopostback() As Boolean
        Get
            'Return DDL.AutoPostBack
            Return blnAutoPostBack
        End Get
        Set(ByVal Value As Boolean)
            blnAutoPostBack = Value
            'DDL.AutoPostBack = Value
        End Set
    End Property

    Public WriteOnly Property CDDLType() As DDLType
        Set(ByVal Value As DDLType)
            If Value = DDLType.FastEntry Then
                DDL.CssClass = "txtNoFocusFE"
            Else
                DDL.CssClass = "txtNoFocus"
            End If
        End Set
    End Property

    Public ReadOnly Property CDDLGetCount() As Integer
        Get
            Return GetCount()
        End Get
    End Property

    Public WriteOnly Property CDDLUDC() As Boolean
        Set(ByVal Value As Boolean)
            If Value = True Then
                txtHIDUDC.Value = "1"
            Else
                txtHIDUDC.Value = "0"
            End If
            blnUDC = Value
        End Set
    End Property

    Public WriteOnly Property CDDLPopUpURL()
        Set(ByVal Value)
            '  strPopUpURL = Value
        End Set
    End Property

    Public WriteOnly Property CDDLPopUpWidth()
        Set(ByVal Value)
            strPopUpWidth = Value
        End Set
    End Property

    Public WriteOnly Property CDDLPopUpHeight()
        Set(ByVal Value)
            strPopUpHeight = Value
        End Set
    End Property

    Public WriteOnly Property CDDLSetWidth()
        Set(ByVal Value)
            DDL.Width = Unit.Pixel(Value)
        End Set
    End Property

    Public WriteOnly Property [Width]() As System.Web.UI.WebControls.Unit
        Set(ByVal Value As System.Web.UI.WebControls.Unit)
            DDL.Width = Value
        End Set
    End Property

    Public ReadOnly Property CDDLGetValue()
        Get
            Return txtHID.Value
        End Get
    End Property

    Public ReadOnly Property CDDLGetValueName()
        Get
            Return txtHIDName.Value
        End Get
    End Property

    Public Property CDDLQuery()
        Get
            Return strQuery
        End Get
        Set(ByVal Value)
            strQuery = Value
            strQuery = strQuery.Replace("'", """")
            txtHIDQuery.Value = HttpContext.Current.Server.UrlEncode(strQuery)
        End Set
    End Property

    Public Property [Enabled]() As Boolean
        Get
            Return DDL.Enabled
        End Get
        Set(ByVal Value As Boolean)
            DDL.Enabled = Value
        End Set
    End Property

    Public WriteOnly Property CDDLMandatoryField() As Boolean
        Set(ByVal Value As Boolean)
            blnMandatory = Value
        End Set
    End Property
#End Region

#Region "-----------Functions-------------"

    Public Sub CDDLAddSingleItem(ByVal Value As String, Optional ByVal UDC As Boolean = True, Optional ByVal Text As String = "")
        Try

            Dim item As ListItem
            Dim blnflag As Boolean
            blnflag = True
            For Each item In DDL.Items
                If item.Text = Text And item.Value = Value Then
                    blnflag = False
                End If
            Next
            If blnflag = True Then
                DDL.Items.Add(New ListItem(IIf(UDC, Value, Text), Value))

                txtHID.Value = Value
                If UDC = True Then
                    txtHIDName.Value = Value
                Else
                    txtHIDName.Value = Text
                End If
                DDL.SelectedIndex = 0
            End If
        Catch ex As Exception
            CreateLog("CDDL", "CDDLAddSingleItem" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        End Try
    End Sub

    Public Function CDDLFillDropDown(Optional ByVal intDefaultCount As Integer = 10, Optional ByVal UDC As Boolean = True, Optional ByVal strSelectedText As String = "", Optional ByVal strSelectedValue As String = "")
        Dim sqRDR As SqlClient.SqlDataReader
        Try

            Dim blnFlag As Boolean

            Dim blnStatus As Boolean
            blnFlag = False
            DDL.Items.Clear()
            'Fill blank only if field is not mandatory
            If blnMandatory = False Then
                'DDL.Items.Add(New ListItem("[Optional]", "")) 
                '' DDL.Items.Add(New ListItem("", ""))
            Else
                'DDL.Items.Add(New ListItem("[Mandatory]", ""))
            End If
            DDL.Items.Add(New ListItem("", ""))

            sqRDR = SQL.Search("CustomDDL", "CDDLFillDropDown-" & Me.ID, strQuery.Replace("""", "'"), SQL.CommandBehaviour.Default, blnStatus)

            If blnStatus = True Then
                Dim inti As Integer
                Dim strText As String
                Dim strValue As String

                inti = 0
                While sqRDR.Read
                    If sqRDR.FieldCount > 1 Then
                        strText = sqRDR(1)
                        strValue = sqRDR(0)
                    Else
                        strText = sqRDR(0)
                        strValue = sqRDR(0)
                    End If
                    If UDC = True Then
                        strText = strValue
                    End If
                    DDL.Items.Add(New ListItem(strText, strValue))
                    inti = inti + 1
                    If UDC = True Then
                        strSelectedText = strSelectedValue
                    End If
                    If strSelectedText <> "" And strSelectedValue <> "" Then
                        If CType(strText, String).ToUpper = strSelectedText.ToUpper And CType(strValue, String).ToUpper = strSelectedValue.ToUpper Then
                            txtHID.Value = strValue
                            txtHIDName.Value = strText
                            DDL.SelectedIndex = inti
                            blnFlag = True
                        End If
                    End If
                    If inti >= intDefaultCount Then
                        If strSelectedText <> "" And strSelectedValue <> "" Then
                            If blnFlag = False Then
                                DDL.Items(0).Text = strSelectedText
                                DDL.Items(0).Value = strSelectedValue
                                txtHID.Value = strSelectedValue
                                txtHIDName.Value = strSelectedText
                            End If
                        End If
                        If sqRDR.Read = True Then
                            DDL.Items.Add(New ListItem("More...", "More.."))
                            DDL.Items.Add(New ListItem("", ""))
                        End If
                        sqRDR.Close()
                        Exit While
                    End If
                End While
                sqRDR.Close()
            End If
            'If blnMandatory = True And DDL.Items.Count > 0 Then
            '    txtHID.Value = DDL.Items(0).Value
            '    txtHIDName.Value = DDL.Items(0).Text
            'End If

            '      DDL.SelectedIndex = 0
        Catch ex As Exception
            CreateLog("CustomDDL", "CDDLFillDropDown-" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        End Try

    End Function
    Public Function CDDLSetItem()
        Try
            Dim list As New ListItem
            list = DDL.Items.FindByValue(txtHID.Value)
            If IsNothing(list) = False Then
                '   If list.Value = "" Then
                '    DDL.SelectedIndex = 0
                'Else
                DDL.SelectedValue = list.Value
                'DDL.Items(0).Value = "0"
                'DDL.Items(0).Text = ""
                'End If
            Else
                If DDL.Items.Count > 0 Then
                    DDL.Items(0).Value = txtHID.Value
                    DDL.Items(0).Text = txtHIDName.Value
                    DDL.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            CreateLog("CDDL", "CDDLSetItem-" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        End Try
    End Function

    Public Function CDDLSetSelectedItem(ByVal Value As String, Optional ByVal UDC As Boolean = True, Optional ByVal Text As String = "")
        Try
            Dim list As ListItem
            If IsDBNull(Value) Then
                Value = "0"
            End If
            If IsDBNull(Text) Then
                Text = ""
            End If

            list = DDL.Items.FindByValue(Value)
            If IsNothing(list) = False Then
                DDL.SelectedValue = list.Value
            Else
                If DDL.Items.Count = 0 Then
                    DDL.Items.Add(New ListItem("", 0))
                End If
                DDL.Items(0).Value = Value
                If UDC = True Then
                    DDL.Items(0).Text = Value
                    txtHIDName.Value = Value
                Else
                    DDL.Items(0).Text = Text
                    txtHIDName.Value = Text
                End If
                DDL.SelectedIndex = 0
            End If
            txtHID.Value = DDL.SelectedItem.Value
            txtHIDName.Value = DDL.SelectedItem.Text
        Catch ex As Exception
            CreateLog("CDDL", "CDDLSetSelectedItem-" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        End Try
    End Function
    Public Sub CDDLSetBlank()
        Try

            If blnMandatory = True Then
                DDL.Items(0).Text = ""
            Else
                DDL.Items(0).Text = ""
            End If
            DDL.Items(0).Value = 0
            DDL.SelectedIndex = 0
            txtHID.Value = ""
            txtHIDName.Value = ""
        Catch ex As Exception
            CreateLog("CDDL", "CDDLSetBlank-" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        End Try
    End Sub

    Private Function GetCount() As Integer
        Try
            Dim intCountRows
            intCountRows = 0
            Dim sqRDR As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            sqRDR = SQL.Search("CustomDDL", "GetCount-340", strQuery.Replace("""", "'"), SQL.CommandBehaviour.Default, blnStatus)
            If blnStatus = True Then
                While sqRDR.Read
                    intCountRows = intCountRows + 1
                End While
                sqRDR.Close()
            End If
            Return intCountRows
        Catch ex As Exception
            CreateLog("CDDL", "GetCount-" & Me.ID, LogType.Application, LogSubType.Exception, "", ex.ToString, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
            Return 0
        End Try
    End Function

    Private Function CreatePopupURL() As String
        Dim strPath As String = Me.Request.Path
        'CreateLog(HttpContext.Current.Request.UserHostAddress, "CreatePopupURL", LogType.Application, LogSubType.Exception, "", strPath, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        Dim intCount As Integer = 0
        For intI As Integer = 0 To strPath.Length - 1
            If strPath.Substring(intI, 1).Equals("/") Then
                intCount = intCount + 1
            End If
        Next
        intCount = intCount - 2
        If HttpContext.Current.Request.ApplicationPath = "/" Then
            intCount = intCount + 1
        End If
        Dim strURL As String
        For intC As Integer = 0 To intCount - 1
            strURL &= "../"
        Next
        strURL &= "Search/Common/PopSearch1.aspx"
        ' CreateLog(HttpContext.Current.Request.ApplicationPath, "CreatePopupURL", LogType.Application, LogSubType.Exception, "", strURL, HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.Name)
        Return strURL
    End Function
#End Region

End Class
