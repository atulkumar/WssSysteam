#Region "Refered Assemblies"

Imports ION.Logging.EventLogging
Imports ION.Net
Imports ION.Data
Imports System.IO
Imports ION.Logging
Imports System.Web.Security
Imports System.Data.SqlClient

#End Region

Partial Class Reports_MonitorIndex
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        'Security Block
        Dim intId As Integer
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            intId = 530
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intId) = False Then
                Response.Redirect("../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intId)
        End If
        'End of Security Block
        Try
            Dim txthiddenImage = Request.Form("txthiddenImage")
            If Session("companyID") = Nothing Then
                ImgbtnDskRpt.Visible = False
                lnkDskRpt.Visible = False
            End If


            If txthiddenImage = "Logout" Then
                LogoutWSS()
            End If
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try

    End Sub

    Public Sub ClearVariables()
        HttpContext.Current.Session("PropCompanyID") = -1
        HttpContext.Current.Session("PropRole") = ""
        HttpContext.Current.Session("PropUserName") = ""
        HttpContext.Current.Session("PropUserID") = 0
        HttpContext.Current.Session("PropCallNumber") = 0
        HttpContext.Current.Session("PropTaskNumber") = 0
    End Sub
End Class
