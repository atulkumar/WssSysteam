Imports ION.Net
Imports ION.Data
Imports System.Web.Security
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System.Data


Partial Class MonitoringCenter_CreateDomain
    Inherits System.Web.UI.Page
    Public intSeqID As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Write("<meta http-equiv=""refresh"" content=""" & Str(Val(HttpContext.Current.Session.Timeout) * 60) & ";Login.aspx"" />")
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        txtCSS(Me.Page)

        btnClick.Attributes.Add("onclick", "SaveEdit('Save');")

        Dim txthiddenImage As String = Request.Form("txthiddenImage")
        If Not IsPostBack Then
            mstrqID = Request.QueryString("ID")
            If (Session("PropCompanyType") = "SCM") Then
                PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM'", ddCompanyName, "N")
            Else
                PopulateDropDownLists("select CI_NU8_Address_Number, CI_VC36_Name from T010011 where CI_VC8_Address_Book_Type = 'COM' and CI_NU8_Address_Number = " & Val(Session("PropCompanyType")), ddCompanyName, "N")
            End If
        End If

        If txthiddenImage <> "" Then

            Select Case txthiddenImage

                Case "Close"
                    Response.Redirect("DomainInfo.aspx")
                Case "Ok"
                    If SaveJobInfo() = True Then
                        Response.Redirect("DomainInfo.aspx")
                    End If
                Case "Save"
                    SaveJobInfo()
                Case "Logout"
                    LogoutWSS()
            End Select

        End If
        If mstrqID <> "-1" Then
            Dim sqrdr As SqlClient.SqlDataReader
            Dim blnStatus As Boolean
            Try
                sqrdr = SQL.Search("CreateDomain", "Load", "select DM_IN4_DID,  DM_IN4_Company_ID_FK, DM_VC150_DomainName, DM_VC150_UserID, DM_VC150_Password, DM_CH1_IsEnable from  T170011 where DM_IN4_DID = " & mstrqID, SQL.CommandBehaviour.SingleRow, blnStatus, "")
                If blnStatus = True Then
                    While sqrdr.Read
                        ddCompanyName.SelectedValue = sqrdr("DM_IN4_Company_ID_FK")
                        txtDomainName.Text = sqrdr("DM_VC150_DomainName")
                        txtUserID.Text = sqrdr("DM_VC150_UserID")
                        ddDomainStatus.SelectedValue = sqrdr("DM_CH1_IsEnable")
                        txtPassword.Text = sqrdr("DM_VC150_Password")
                    End While
                    sqrdr.Close()
                End If
            Catch ex As Exception
                lstError.Items.Clear()
                lstError.Items.Add("Server is busy please try later...")
                ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                'cpnlError.Visible = True
                'cpnlError.Text = "Error Message"
                'ImgError.ImageUrl = "../images/error_image.gif"
                'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)

                CreateLog("CreateDomain", "Load-101", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            End Try
        End If

    End Sub

    Private Function SaveJobInfo() As Boolean
        Try

            If ValidateJobInfo() = True Then
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' SQL.DBTable = "T170011"
                SQL.DBTracing = False

                arColumnName.Add("DM_IN4_Company_ID_FK")
                arColumnName.Add("DM_VC150_DomainName")
                arColumnName.Add("DM_VC150_UserID")
                arColumnName.Add("DM_VC150_Password")
                arColumnName.Add("DM_CH1_IsEnable")

                arColumnName.Add("DM_IN4_Last_Modified_By")
                arColumnName.Add("DM_DT8_Last_Modified_Date")
                arColumnName.Add("DM_VC10_Modified_By_System_Code")

                If mstrqID = "-1" Then
                    arColumnName.Add("DM_SI2_Inserted_By")
                    arColumnName.Add("DM_DT8_Inserted_Date")
                    arColumnName.Add("DM_VC10_Inserted_By_System_Code")
                End If

                arRowData.Add(ddCompanyName.SelectedValue.Trim)
                arRowData.Add(txtDomainName.Text.Trim)
                arRowData.Add(txtUserID.Text.Trim)
                arRowData.Add(txtPassword.Text.Trim)
                arRowData.Add(ddDomainStatus.SelectedValue.Trim)

                arRowData.Add(Session("PropUserID"))
                arRowData.Add(Now.ToShortDateString)
                arRowData.Add(GetIP())

                If mstrqID = "-1" Then
                    arRowData.Add(Session("PropUserID"))
                    arRowData.Add(Now.ToShortDateString)
                    arRowData.Add(GetIP())
                End If

                If mstrqID = "-1" Then
                    If SQL.Save("T170011", "CreateDomain", "load", arColumnName, arRowData) = True Then
                        mstrqID = intSeqID
                        lstError.Items.Clear()
                        cpnlError.Visible = True
                        'cpnlError.Text = "Message"
                        lstError.Items.Add("Record saved successfully...")
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        'ImgError.ImageUrl = "../images/Pok.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Server is busy please try later...")
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Error Message"
                        'ImgError.ImageUrl = "../images/error_image.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        Return False
                    End If
                Else
                    If SQL.Update("T170011", "CreateDomain", "SaveJobInfo", "select * from T170011 where DM_IN4_DID=" & mstrqID & "", arColumnName, arRowData) = True Then
                        lstError.Items.Clear()
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Message"
                        lstError.Items.Add("Record updated successfully...")
                        'ImgError.ImageUrl = "../images/Pok.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgOK)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgOK)
                        Return True
                    Else
                        lstError.Items.Clear()
                        lstError.Items.Add("Server is busy please try later...")
                        'cpnlError.Visible = True
                        'cpnlError.Text = "Error Message"
                        'ImgError.ImageUrl = "../images/error_image.gif"
                        'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
                        ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgError)
                        Return False
                    End If
                End If
            End If
        Catch ex As Exception
            lstError.Items.Clear()
            lstError.Items.Add("Server is busy please try later...")
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/error_image.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            CreateLog("CreateDomain", "SaveJobInfo-184", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
            Return False
        End Try

    End Function

    Private Function ValidateJobInfo() As Boolean
        Dim shFlag As Short
        shFlag = 0
        lstError.Items.Clear()

        If txtDomainName.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Domain Name cannot be empty...")
        End If
        If txtUserID.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("UserID cannot be empty...")
        End If
        If txtPassword.Text.Equals("") Then
            shFlag = 1
            lstError.Items.Add("Password cannot be empty...")
        End If
        If shFlag = 1 Then
            'cpnlError.Visible = True
            'cpnlError.Text = "Error Message"
            'ImgError.ImageUrl = "../images/warning.gif"
            'MessagePanelListStyle(lstError, mdlMain.MSG.msgError)
            ShowMsgPenel(cpnlError, lstError, ImgError, mdlMain.MSG.msgInfo)
            Return False
        Else
            Return True
        End If

        lstError.Items.Clear()

    End Function

    Private Function PopulateDropDownLists(ByVal sqlQuery As String, ByRef ddData As DropDownList, ByVal isOptional As Char)

        Dim dtDDData As DataTable
        Dim sqlCon As New SqlConnection
        Dim sqlda As SqlDataAdapter
        Try
            dtDDData = New DataTable
            sqlCon = New SqlConnection(SQL.DBConnection)
            sqlda = New SqlDataAdapter(sqlQuery.Trim, sqlCon)
            sqlCon.Open()
            sqlda.Fill(dtDDData)
            ddData.DataSource = dtDDData
            ddData.DataTextField = dtDDData.Columns(1).ColumnName
            ddData.DataValueField = dtDDData.Columns(0).ColumnName
            If (isOptional = "Y") Then
                Dim row As DataRow
                row = dtDDData.NewRow
                row(0) = "0"
                row(1) = "Optional"
                dtDDData.Rows.InsertAt(row, 0)
                ddData.SelectedValue = "0"
            End If
            ddData.DataBind()
        Catch ex As Exception
            CreateLog("CreateDomain", "PopulateDropDownLists-486", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA")
        Finally
            sqlCon.Close()
        End Try

    End Function

End Class
