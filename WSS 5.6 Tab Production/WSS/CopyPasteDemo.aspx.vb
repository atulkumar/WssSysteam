Imports System.Xml
Imports System.Data

Partial Class CopyPasteDemo
    Inherits System.Web.UI.Page
    Private mintR As Integer
    Private mintC As Integer
    Private Shared dsOld As New DataSet
    Private ds As New DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        cpnlError.State = CustomControls.Web.PanelState.Collapsed
        cpnlError.Enabled = False
        mintR = Val(Request.Form("txthidden1"))
        mintC = Val(Request.Form("txthidden2"))
        btnpastedata.Attributes.Add("Onclick", "return PasteFromClipboard();")
        If Not IsPostBack Then
            dsOld.Clear()
            dsOld.Tables.Clear()
            Dim dt As New DataTable
            dsOld.Tables.Add(dt)
            dsOld.Tables(0).Columns.Add("Col0")
            dsOld.Tables(0).Columns.Add("Col1")
            dsOld.Tables(0).Columns.Add("Col2")
            dsOld.Tables(0).Columns.Add("Col3")
            dsOld.Tables(0).Columns.Add("Col4")
            dsOld.Tables(0).Columns.Add("Col5")
            Dim dr As DataRow
            For inti As Integer = 0 To 3
                dr = dsOld.Tables(0).NewRow
                dr.Item(0) = ""
                dsOld.Tables(0).Rows.Add(dr)
            Next
            GrdAddSerach.DataSource = dsOld.Tables(0)
            GrdAddSerach.DataBind()
        End If
        If IsPostBack = True Then
            If TextBox1.Text.Trim <> "" Then
                ShowData(TextBox1.Text.Trim)
            End If
        End If
        ' FormatGrid()
    End Sub
    Private Function FormatGrid()
        For inti As Integer = 0 To GrdAddSerach.Columns.Count - 1
            GrdAddSerach.Columns(inti).HeaderStyle.Width = Unit.Pixel(100)
        Next
    End Function

    Public Function ShowData(ByVal Data As Object) As Boolean

        Try

            Dim strdata As String = CStr(Data)
            Dim strD() As String

            Dim dt As New DataTable
            Dim dr1 As DataRow

            strdata = strdata.Replace(vbNewLine, "*")
            strdata = strdata.Replace(vbTab, "~")
            strD = strdata.Split("*")


            Dim blnColumnAdded As Boolean = False

            For inti As Integer = 0 To strD.Length - 1

                Dim strC() As String
                strC = strD(inti).Split("~")

                Dim intColumnLength As Integer = strC.Length

                If intColumnLength > 5 Then
                    ' Show error message that columns copied are more  then on the grid

                    Exit Function
                End If

                dr1 = dt.NewRow
                For intj As Integer = 0 To strC.Length - 1
                    If blnColumnAdded = False Then
                        dt.Columns.Add("Column" & intj)
                    End If
                    dr1(intj) = strC(intj).ToString
                Next
                blnColumnAdded = True
                dt.Rows.Add(dr1)
            Next
            Dim ds As New DataSet
            ds.Tables.Add(dt)

            If dsOld.Tables.Count > 0 Then
                Dim c As Integer = dsOld.Tables(0).Columns.Count
                If (mintC + ds.Tables(0).Columns.Count) > dsOld.Tables(0).Columns.Count Then
                    For intj As Integer = c To ((mintC + ds.Tables(0).Columns.Count) - dsOld.Tables(0).Columns.Count) - 1 + (c)
                        dsOld.Tables(0).Columns.Add("col" & intj)
                    Next
                End If

                Dim m As Integer = 0
                For inti As Int16 = mintR To (ds.Tables(0).Rows.Count + mintR) - 1
                    If inti > dsOld.Tables(0).Rows.Count - 1 Then
                        Dim dr As DataRow = dsOld.Tables(0).NewRow
                        dsOld.Tables(0).Rows.Add(dr)
                    End If
                    Dim n As Integer = 0

                    For intk As Integer = mintC To (ds.Tables(0).Columns.Count + mintC) - 1
                        dsOld.Tables(0).Rows(inti).Item(intk) = ds.Tables(0).Rows(m).Item(n)
                        n = n + 1

                    Next
                    m = m + 1
                Next
                Dim blnFlag As Boolean = False
                If (dsOld.Tables(0).Rows.Count - mintR) <= ds.Tables(0).Rows.Count Then
                    blnFlag = True
                End If

                If blnFlag = True Then
                    Dim drNew As DataRow = dsOld.Tables(0).NewRow
                    dsOld.Tables(0).Rows.Add(drNew)
                End If


                dsOld.AcceptChanges()
                GrdAddSerach.DataSource = dsOld.Tables(0)


                GrdAddSerach.DataBind()
            Else

                dsOld.Merge(ds)
                GrdAddSerach.DataSource = ds.Tables(0)

                GrdAddSerach.DataBind()
            End If

            System.IO.File.Delete("c:\Test\Test1.xml")

        Catch ex As Exception

        End Try

    End Function

    Private Sub GrdAddSerach_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddSerach.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            For inti As Integer = 0 To dsOld.Tables(0).Columns.Count - 1
                e.Item.Cells(inti).Attributes.Add("onclick", "xxyy(" & e.Item.ItemIndex & "," & inti & ",this)")

            Next


        End If
    End Sub
End Class
