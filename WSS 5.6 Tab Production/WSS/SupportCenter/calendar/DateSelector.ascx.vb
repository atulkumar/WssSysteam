Public Class DateSelector
    Inherits System.Web.UI.UserControl


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_Date As System.Web.UI.WebControls.Label
    'Protected WithEvents txt_Date As System.Web.UI.WebControls.TextBox
    'Protected WithEvents imgCalendar As System.Web.UI.WebControls.Image
    'Protected WithEvents PnlCalender As System.Web.UI.WebControls.Panel

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private intLeftPos As Integer
    Private intTopPos As Integer
    Private UntWidth As System.Web.UI.WebControls.Unit
 

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        Dim strUserpos As String
        strUserpos = intLeftPos.ToString & " , " & intTopPos.ToString

        Dim scriptStr As String = _
                "javascript:return showDate(" & getClientID() & ")"

        imgCalendar.Attributes.Add("onclick", scriptStr)

    End Sub

    ' Get the id of the control rendered on client side
    ' Very essential for Javascript Calendar scripts to locate the textbox
    Public Function getClientID() As String
        Return txt_Date.ClientID()
    End Function
    'This property will set the CSS for textbox  if it is in Fast Entry
    Public WriteOnly Property FastEntry() As Boolean
        Set(ByVal Value As Boolean)
            If Value = True Then
                txt_Date.CssClass = "txtNoFocusFE"
            Else
                txt_Date.CssClass = "txtNoFocus"
            End If
        End Set
    End Property

 
    ' This propery sets/gets the calendar date
    Public Property CalendarDate() As String
        Get
            Return txt_Date.Text
        End Get
        Set(ByVal Value As String)
            txt_Date.Text = Value
        End Set
    End Property

    Public Property readOnlyDate() As Boolean
        Get
            Return imgCalendar.Visible
        End Get
        Set(ByVal Value As Boolean)
            imgCalendar.Visible = Value
        End Set
    End Property

    Public Property Editable() As Boolean
        Get
            Return txt_Date.ReadOnly
        End Get
        Set(ByVal Value As Boolean)
            txt_Date.ReadOnly = Not Value
        End Set
    End Property

    Public Property Width() As System.Web.UI.WebControls.Unit
        Get
            Return UntWidth
        End Get
        Set(ByVal Value As System.Web.UI.WebControls.Unit)
            Dim Unttmp As New Unit
            Dim UntTmpWidth As New Unit
            Dim cnvtUnt As New UnitConverter
            'UntTmpWidth = Value
            UntWidth = Value
            UntTmpWidth = Value

            Select Case Value.Type 'Value.Type
                Case UnitType.Pixel
                    'If Value.Value < 86 Then
                    '    UntTmpWidth = Unit.Point(86)
                    'End If
                    Unttmp = Unit.Pixel(UntTmpWidth.Value - imgCalendar.Width.Value)
                Case UnitType.Percentage
                    Unttmp = Unit.Percentage(Value.Value - 15)
                Case UnitType.Point
                    'If Value.Value < 65 Then
                    '    UntTmpWidth = Unit.Point(65)
                    'End If
                    imgCalendar.Width = Unit.Point((imgCalendar.Width.Value * 0.75))
                    UntTmpWidth = Unit.Pixel(Value.Value * 1.33)
                    Unttmp = Unit.Pixel(UntTmpWidth.Value - imgCalendar.Width.Value)
            End Select
            PnlCalender.Width = UntTmpWidth
            txt_Date.Width = Unttmp
        End Set
    End Property

    Public Property LeftPos() As Integer
        Get
            Return intLeftPos
        End Get
        Set(ByVal Value As Integer)
            intLeftPos = Value
        End Set
    End Property
    Public Property TopPos() As Integer
        Get
            Return intTopPos
        End Get
        Set(ByVal Value As Integer)
            intTopPos = Value
        End Set
    End Property

End Class
