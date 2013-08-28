Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Namespace IONGrid
    Public Class clsDataGridItemTemplate
        Implements ITemplate
        Private dtBind As DataTable
        Private strddlName As String
        Private strDataValueField As String
        Private strDataTextField As String

        Public Sub New(ByVal DDLName As String, ByVal DataValueField As String, ByVal DataTextField As String, ByVal DDLSource As DataTable)
            Me.dtBind = DDLSource
            Me.strDataValueField = DataValueField
            Me.strDataTextField = DataTextField
            Me.strddlName = DDLName
        End Sub

        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim ddl As DropDownList = New DropDownList
            AddHandler ddl.DataBinding, AddressOf ddl_DataBinding
            objContainer.Controls.Add(ddl)
        End Sub

        Private Sub ddl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim ddl As DropDownList = CType(sender, DropDownList)
            ddl.ID = strddlName
            ddl.DataSource = dtBind
            ddl.DataValueField = strDataValueField
            ddl.DataTextField = strDataTextField
        End Sub
    End Class

    Public Class CreateItemTemplatePushButton
        Implements ITemplate
        Private strColumnText As String
        Private strButtonName As String
        Private strCommandName As String
        Private Visibile As Boolean = True
        Private Enable As Boolean = True
        Public Sub New(ByVal ButtonName As String, ByVal ColText As String, ByVal CommandName As String)
            Me.strColumnText = ColText
            Me.strButtonName = ButtonName
            Me.strCommandName = CommandName
        End Sub
        Public Sub New(ByVal ButtonName As String, ByVal ColText As String, ByVal CommandName As String, ByVal Enabling As Boolean, ByVal Visibiling As Boolean)
            Me.strColumnText = ColText
            Me.strButtonName = ButtonName
            Me.Visibile = Visibiling
            Me.Enable = Enabling
            Me.strCommandName = CommandName
        End Sub
        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim btn As Button = New Button
            btn.CommandName = strCommandName
            AddHandler btn.DataBinding, AddressOf btn_DataBinding
            objContainer.Controls.Add(btn)
        End Sub
        Private Sub btn_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim btn As Button = CType(sender, Button)
            btn.ID = strButtonName
            btn.Text = strColumnText
            btn.Visible = Visibile
            btn.Enabled = Enable
            btn.CommandName = strCommandName
            btn.CausesValidation = False
        End Sub
    End Class
#Region "Item Template for Label"
    Public Class CreateItemTemplateLabel
        Implements ITemplate
        Private strColumnText As String
        Private strLabelName As String
        Public Sub New(ByVal ColText As String, ByVal LabelName As String)
            Me.strColumnText = ColText
            Me.strLabelName = LabelName
        End Sub
        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim lbl As Label = New Label
            lbl.ID = strLabelName
            AddHandler lbl.DataBinding, AddressOf lbl_DataBinding
            objContainer.Controls.Add(lbl)
        End Sub
        Private Sub lbl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim lbl As Label = CType(sender, Label)
            lbl.ID = strLabelName
            lbl.Text = strColumnText
        End Sub
    End Class
#End Region

#Region "Item Template for TextBox"
    Public Class CreateItemTemplateTextBox
        Implements ITemplate
        Private strColumnText As String
        Private strTextBoxName As String
        Private TextMode As Boolean
        Private MaxLength As Integer = 0
        'Private ReadOnly As Boolean = False

        Public Sub New(ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
        End Sub

        Public Sub New(ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean, ByVal MaxLength As Integer)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
            Me.MaxLength = MaxLength
        End Sub

        Public Sub New(ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean, ByVal MaxLength As Integer, ByVal ReadOnlyProperty As Boolean)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
            Me.MaxLength = MaxLength
            'Me.ReadOnly = ReadOnlyProperty
        End Sub

        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim txt As TextBox = New TextBox
            txt.ID = strTextBoxName
            txt.Width = System.Web.UI.WebControls.Unit.Percentage(100)
            txt.AutoPostBack = False

            AddHandler txt.DataBinding, AddressOf txt_DataBinding


            objContainer.Controls.Add(txt)
        End Sub

        Private Sub txt_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim txt As TextBox = CType(sender, TextBox)
            If TextMode = True Then
                txt.TextMode = TextBoxMode.MultiLine
            Else
                txt.TextMode = TextBoxMode.SingleLine
            End If
            txt.ID = strTextBoxName
            txt.MaxLength = MaxLength
            txt.ReadOnly = False
            txt.Text = strColumnText
        End Sub
    End Class
#End Region

#Region "Item Template for SubMit button"
    Public Class CreateItemTemplateSubmitButton
        Implements ITemplate
        Private strColumnText As String
        Private strButtonName As String
        'Private ReadOnly As Boolean = False

        Public Sub New(ByVal ColText As String, ByVal ButtonName As String)
            Me.strColumnText = ColText
            Me.strButtonName = ButtonName
        End Sub
        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim BTN As New Button
            BTN.Width = System.Web.UI.WebControls.Unit.Percentage(0)
            BTN.Height = System.Web.UI.WebControls.Unit.Percentage(0)
            BTN.Visible = False
            objContainer.Controls.Add(BTN)
        End Sub
    End Class
#End Region

#Region "Item Template for TextBox For Header"
    Public Class CreateItemTemplateTextBoxForHeader
        Implements ITemplate
        Private strColumnText As String
        Private strLabelText As String
        Private strTextBoxName As String
        Private TextMode As Boolean
        Private MaxLength As Integer = 0
        Private blnTextBox As Boolean
        Private UntTextBoxWidth As System.Web.UI.WebControls.Unit
        'Private ReadOnly As Boolean = False
        Private dataField As String
        Public Sub New(ByVal DataField As String, ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean, ByVal HeaderText As String, ByVal ShowTextBox As Boolean, Optional ByVal MaxLength As Int16 = -1)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
            Me.strLabelText = HeaderText
            Me.dataField = DataField
            Me.blnTextBox = ShowTextBox
            If MaxLength <> -1 Then
                Me.MaxLength = MaxLength
            End If
        End Sub
        Public Sub New(ByVal DataField As String, ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean, ByVal MaxLength As Int16, ByVal HeaderText As String, ByVal ShowTextBox As Boolean)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
            Me.MaxLength = MaxLength
            Me.strLabelText = HeaderText
            Me.blnTextBox = ShowTextBox
            Me.dataField = DataField
        End Sub
        Public Sub New(ByVal DataField As String, ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean, ByVal MaxLength As Int16, ByVal ReadOnlyProperty As Boolean, ByVal HeaderText As String, ByVal ShowTextBox As Boolean)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
            Me.MaxLength = MaxLength
            Me.blnTextBox = ShowTextBox
            Me.dataField = DataField
        End Sub
        Public Sub New(ByVal DataField As String, ByVal ColText As String, ByVal TextBoxName As String, ByVal TextArea As Boolean, ByVal HeaderText As String, ByVal ShowTextBox As Boolean, ByVal TextBoxWidth As System.Web.UI.WebControls.Unit, Optional ByVal MaxLength As Int16 = -1)
            Me.strColumnText = ColText
            Me.strTextBoxName = TextBoxName
            Me.TextMode = TextArea
            Me.strLabelText = HeaderText
            Me.blnTextBox = ShowTextBox
            Me.UntTextBoxWidth = TextBoxWidth
            If MaxLength <> -1 Then
                Me.MaxLength = MaxLength
            End If
            Me.dataField = DataField
        End Sub
        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim txt As TextBox = New TextBox
            Dim lbl As New LinkButton
            txt.ID = strTextBoxName
            txt.Width = System.Web.UI.WebControls.Unit.Percentage(100)
            txt.AutoPostBack = False
            AddHandler txt.DataBinding, AddressOf txt_DataBinding
            lbl.ID = "lbl" + strTextBoxName
            AddHandler lbl.DataBinding, AddressOf lbl_DataBinding
            If blnTextBox = True Then
                objContainer.Controls.Add(txt)
            End If
            objContainer.Controls.Add(lbl)
        End Sub
        Private Sub txt_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim txt As TextBox = CType(sender, TextBox)
            If TextMode = True Then
                txt.TextMode = TextBoxMode.MultiLine
            Else
                txt.TextMode = TextBoxMode.SingleLine
            End If
            txt.ID = strTextBoxName
            txt.MaxLength = MaxLength
            txt.ReadOnly = False
            txt.Text = strColumnText
            txt.CssClass = "SearchTxtBox"
            If UntTextBoxWidth.Value > 0 Then
                txt.Width = UntTextBoxWidth
            End If
        End Sub
        Public Event OnSort(ByVal sender As Object, ByVal e As CommandEventArgs)
        Private Sub lbl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim lbl As LinkButton = CType(sender, LinkButton)
            lbl.ID = "lbl" & strTextBoxName
            lbl.Text = strLabelText
            lbl.ForeColor = Color.Black
            If lbl.ID.ToUpper = "A" Or lbl.ID.ToUpper = "C" Or lbl.ID = "F".ToUpper Or lbl.ID.ToUpper = "lblBlank1_H".ToUpper Or lbl.ID.ToUpper = "lblBlank2_H.ID".ToUpper Or lbl.ID.ToUpper = "lblBlank3_H".ToUpper Then
            Else
                AddHandler lbl.Click, AddressOf SorGrid
            End If
        End Sub
        Protected Sub SorGrid(ByVal sender As Object, ByVal e As EventArgs)
            Dim lbl As System.Web.UI.WebControls.LinkButton = CType(sender, System.Web.UI.WebControls.LinkButton)
            lbl.ID = strTextBoxName
            If lbl.ID.ToUpper = "A" Or lbl.ID.ToUpper = "C" Or lbl.ID.ToUpper = "F" Or lbl.ID.ToUpper = "lblBlank1_H".ToUpper Or lbl.ID.ToUpper = "lblBlank2_H.ID".ToUpper Or lbl.ID.ToUpper = "lblBlank3_H".ToUpper Then
            Else
                Dim ee As CommandEventArgs = New CommandEventArgs("Sort", Me.dataField)
                RaiseEvent OnSort(sender, ee)
            End If
        End Sub
    End Class
#End Region

#Region "Item Template for Image"
    Public Class CreateItemTemplateImage
        Implements ITemplate
        Private strID As String
        Private strImageUrl As String
        Public Sub New(ByVal ImgID As String, ByVal ImageUrl As String)
            Me.strID = ImgID
            Me.strImageUrl = ImageUrl
        End Sub
        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim Img As New System.Web.UI.WebControls.Image
            Img.ID = strID
            AddHandler Img.DataBinding, AddressOf lbl_DataBinding
            objContainer.Controls.Add(Img)
        End Sub
        Private Sub lbl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim Img As System.Web.UI.WebControls.Image = CType(sender, System.Web.UI.WebControls.Image)
            Img.ID = strID
            Img.ImageUrl = strImageUrl
        End Sub
    End Class
#End Region

#Region "Item Template for CheckBox"
    Public Class CreateItemTemplateCheckBox
        Implements ITemplate
        Private ColumnValue As Integer
        Private strCheckBoxName As String
        Private PropReadOnly As Boolean = False


        Public Sub New(ByVal CheckBoxName As String)
            Me.strCheckBoxName = CheckBoxName
        End Sub
        Public Sub New(ByVal CheckBoxName As String, ByVal ColValue As Integer)
            Me.strCheckBoxName = CheckBoxName
            Me.ColumnValue = ColValue
        End Sub

        Public Sub New(ByVal ColValue As Integer, ByVal CheckBoxName As String, ByVal ReadOnlyProperty As Boolean)
            Me.ColumnValue = ColValue
            Me.strCheckBoxName = CheckBoxName
            Me.PropReadOnly = ReadOnlyProperty
        End Sub

        Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim chk As CheckBox = New CheckBox
            chk.ID = strCheckBoxName
            chk.Width = System.Web.UI.WebControls.Unit.Percentage(100)
            chk.AutoPostBack = False

            AddHandler chk.DataBinding, AddressOf chk_DataBinding


            objContainer.Controls.Add(chk)
        End Sub

        Private Sub chk_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim chk As CheckBox = CType(sender, CheckBox)

            chk.ID = strCheckBoxName
            chk.Checked = ColumnValue
        End Sub
    End Class
#End Region

#Region "Item Template for Calendar"
    Public Class CreateItemTemplateCalendar
        'Implements ITemplate
        'Private ColumnValue As String
        'Private strCalendarName As String
        'Private PropReadOnly As Boolean = False

        'Public Sub New(ByVal CalendarName As String)
        '    Me.strCalendarName = CalendarName
        'End Sub
        'Public Sub New(ByVal CalendarName As String, ByVal ColValue As String)
        '    Me.strCalendarName = CalendarName
        '    Me.ColumnValue = ColValue
        'End Sub

        'Public Sub New(ByVal ColValue As String, ByVal CalendarName As String, ByVal ReadOnlyProperty As Boolean)
        '    Me.ColumnValue = ColValue
        '    Me.strCalendarName = CalendarName
        '    Me.PropReadOnly = ReadOnlyProperty
        'End Sub

        'Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
        '    Dim MyCalendar As DateSelector = New DateSelector
        '    'MyCalendar.ID = strCalendarName

        '    AddHandler MyCalendar.DataBinding, AddressOf MyCalendar_DataBinding

        '    objContainer.Controls.Add(MyCalendar)
        'End Sub

        'Private Sub MyCalendar_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        '    Dim dt As DateSelector = CType(sender, DateSelector)

        '    dt.ID = strCalendarName
        '    dt.CalendarDate = ColumnValue
        '    dt.readOnlyDate = PropReadOnly
        '    dt.Width = System.Web.UI.WebControls.Unit.Percentage(100)
        'End Sub
    End Class
#End Region

#Region "Item Template for ComboBox"
    'Public Class CreateItemTemplateComboBox
    '    Implements ITemplate
    '    Private ColumnValue() As String
    '    Private strComboName As String
    '    Private PropReadOnly As Boolean = False

    '    Public Sub New(ByVal ComboName As String)
    '        Me.strComboName = ComboName
    '    End Sub
    '    Public Sub New(ByVal ComboName As String, ByVal ColValue() As String)
    '        Me.strComboName = ComboName
    '        Me.ColumnValue = ColValue
    '    End Sub

    '    Public Sub New(ByVal ColValue() As String, ByVal ComboName As String, ByVal ReadOnlyProperty As Boolean)
    '        Me.ColumnValue = ColValue
    '        Me.strComboName = ComboName
    '        Me.PropReadOnly = ReadOnlyProperty
    '    End Sub

    '    Public Sub InstantiateIn(ByVal objContainer As Control) Implements System.Web.UI.ITemplate.InstantiateIn
    '        Dim ddl As CustomDDL = New CustomDDL
    '        'ddl.ID = strComboName
    '        'ddl.AddAllItems(ColumnValue)
    '        AddHandler ddl.DataBinding, AddressOf ddl_DataBinding


    '        objContainer.Controls.Add(ddl)
    '    End Sub

    '    Private Sub ddl_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
    '        Dim ddl As New CustomDDL
    '        ddl = CType(sender, CustomDDL)
    '        ddl.Width = Unit.Pixel(100)
    '        ddl.ID = strComboName
    '        'ddl.ReadOnly = PropReadOnly
    '        'ddl.AddAllItems(ColumnValue)

    '    End Sub
    'End Class
#End Region

End Namespace