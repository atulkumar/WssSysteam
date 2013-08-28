Imports ION.Data
Imports ION.Logging.EventLogging
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Partial Class Help_ViewReleaseHistory
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'To Add Meta Tag i.e Redirect Page to Login Page after Session Timeout in Page Header
        '###########################
        Dim meta As HtmlMeta
        meta = New HtmlMeta()
        meta.HttpEquiv = "Refresh"
        meta.Content = Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";URL=../Login/Login.aspx"
        Me.Header.Controls.Add(meta)
        '###########################
        BINDGrid()
    End Sub
    Private Function BINDGrid() As Boolean

        SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        SQL.DBTracing = False
        Dim dsTemp As New DataSet
        Dim dt As New DataTable
        Dim sqstr As String
        Dim HTML As String = ""
        HTML = HTML
        sqstr = " select * from T070062"
        Try
            If SQL.Search("T070062 ", "dataobjentry", "BINDGrid", sqstr, dsTemp, "", "") = True Then

                dt = dsTemp.Tables(0)

                HTML = HTML & "<BR><br><table width=""600px"" style=""border-collapse: collapse"" border=""0"" bordercolor=""#e0e0e0"">"

            Else
                Exit Function
            End If


            'sqRDR reader read data from t130022 and fill Textboxes
            For inti As Integer = 0 To dt.Rows.Count - 1
                Dim strModule As String = dt.Rows(inti).Item("RE_VC50_ModName")
                Dim strSub As String = dt.Rows(inti).Item("RE_VC250_Subject").ToString
                Dim strDesc As String = dt.Rows(inti).Item("RE_VC8000_Desc").ToString
                Dim strType As String = dt.Rows(inti).Item("RE_VC50_Type").ToString
                Dim strDate As String = dt.Rows(inti).Item("RE_VC30_Date").ToString

                HTML = HTML & "<tr>"
                HTML = HTML & "<td>"
                HTML = HTML & "<table width=""600px"" bgcolor=""white"" style=""border-collapse: collapse"" border=""1"" bordercolor=""#Red"">"

                HTML = HTML & "<tr>"
                HTML = HTML & "<td colspan=""2"" height=""14"" align=""Center"" bgcolor=""#e0e0e0"">"
                HTML = HTML & "<font color=#000000 size=2 face=""Verdana""><b>"
                HTML = HTML & strType
                HTML = HTML & "</b></font></td>"
                HTML = HTML & "</tr>"

                HTML = HTML & "<tr>"
                HTML = HTML & "<td width=""200px"" bgcolor=""#e0e0e0"">"
                HTML = HTML & "<font size=2 face=""Verdana"" >"
                HTML = HTML & "<b>Module </b></font>"
                HTML = HTML & "</td>"
                HTML = HTML & "<td width=""400px"">"
                HTML = HTML & "<font size=2 face=""Verdana"">"
                HTML = HTML & strModule
                HTML = HTML & "</font></td>"
                HTML = HTML & "</tr>"

                HTML = HTML & "<tr>"
                HTML = HTML & "<td width=""200px"" bgcolor=""#e0e0e0"">"
                HTML = HTML & "<font size=2 face=""Verdana"" >"
                HTML = HTML & "<b>Release Date </b></font>"
                HTML = HTML & "</td>"
                HTML = HTML & "<td width=""400px"">"
                HTML = HTML & "<font size=2 face=""Verdana"">"
                HTML = HTML & strDate
                HTML = HTML & "</font></td>"
                HTML = HTML & "</tr>"


                HTML = HTML & "<tr>"
                HTML = HTML & "<td width=""200px"" bgcolor=""#e0e0e0"">"
                HTML = HTML & "<font size=2 face=""Verdana"" >"
                HTML = HTML & "<b>Subject </b></font>"
                HTML = HTML & "</td>"
                HTML = HTML & "<td width=""400px"">"
                HTML = HTML & "<font size=2 face=""Verdana"">"
                HTML = HTML & strSub
                HTML = HTML & "</font></td>"
                HTML = HTML & "</tr>"

                HTML = HTML & "<tr>"
                HTML = HTML & "<td width=""200px"" bgcolor=""#e0e0e0"">"
                HTML = HTML & "<font size=2 face=""Verdana"" >"
                HTML = HTML & "<b>Description </b></font>"
                HTML = HTML & "</td>"
                HTML = HTML & "<td width=""400px"">"
                HTML = HTML & "<font size=2 face=""Verdana"">"
                HTML = HTML & strDesc
                HTML = HTML & "</font></td>"
                HTML = HTML & "</tr>"

                HTML = HTML & "</td>"
                HTML = HTML & "</tr>"
                HTML = HTML & "</table>"

                HTML = HTML & "<tr>"
                HTML = HTML & "<td    colspan=""2""  height=""30"" >"
                HTML = HTML & " <font color=gray face=verdana size=2>"
                HTML = HTML & "</font></td>"
                HTML = HTML & "</tr>"

            Next

            HTML = HTML & "</table>"
            Label1.Text = HTML

        Catch ex As Exception
            CreateLog("BasicMonitorEdit", "filltextbox-97", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
        End Try
    End Function

 
End Class
