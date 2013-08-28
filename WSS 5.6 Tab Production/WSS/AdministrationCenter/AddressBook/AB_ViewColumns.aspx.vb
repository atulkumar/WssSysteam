'*******************************************************************
' Page                 : - Ab_View_Columns
' Purpose              : - It will show the different views.User can select any view, according to which                                information will be shown.
' Tables used          : -  T070011, T070031, T070042, T060011, T060022, T030212,T030201
' Date					Author	        Sachin					Modification Date					Description
' 21/03/06											   -------------------					Created
'
' Notes: 
' Code:
'*******************************************************************
Imports System
Imports ION.Data
Imports ION.Logging
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Data.SqlClient
Imports ION.Logging.EventLogging
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.SessionState
Partial Class AdministrationCenter_AddressBook_AB_ViewColumns
    Inherits System.Web.UI.Page

#Region "global variables"
    Dim mdvtable As New DataView
    Dim mstrTable As String
    Private Shared arddlADValue As New ArrayList
    Private Shared artxtBoxValue As New ArrayList
    Private Shared arddlFAValue As New ArrayList
    Private intID As Int16 'it will store screen ID
    Private marWhereCondition As New ArrayList
    Private Shared artxtBoxFaValue As New ArrayList

    ' ***********Filter*****************
    Structure GridFilter
        Dim ColumnName As String
        Dim AD As String
        Dim SO As String
        Dim FA As String
        Dim Value As String
    End Structure
    Private Shared MyViewState() As GridFilter
    ' ***********End Filter*****************

#End Region

#Region "Page Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '463 call view
        '464 Task View
        '2212 Task Heirarchy
        '502 to Do List
        '536 Work View/Historic View
        '799 Call View Simple
        '229 Address Book
        '40 Project
        '1024 Call +
        mstrTable = Request.QueryString("TBLName")
        If Not IsPostBack Then
            lstError.Attributes.Add("Onclick", "CopyTOClipBoard('" & lstError.ClientID & "');")
            imgSave.Attributes.Add("Onclick", "return SaveEdit('Save');")
            imgReset.Attributes.Add("Onclick", "return SaveEdit('Reset');")
            imgDelete.Attributes.Add("Onclick", "return SaveEdit('Delete');")
            imgOk.Attributes.Add("Onclick", "return SaveEdit('Ok');")
            imgClose.Attributes.Add("Onclick", "return SaveEdit('Close');")
            imgEdit.Attributes.Add("OnClick", "SaveEdit('Update');") 'Addining javascript function on click
        End If
        intID = Request.QueryString("ScrID")   'storing screen's ID
        cpnlErrorPanel.Enabled = False
        cpnlErrorPanel.State = CustomControls.Web.PanelState.Collapsed
        lstError.Items.Clear()

        Dim txthiddenImage As String = Request.Form("txthiddenImage")

        If txthiddenImage <> "" Then
            Try
                Select Case txthiddenImage

                    Case "Ok"
                        'Security Block
                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlErrorPanel.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If
                        'End of Security Block
                        If SaveView() = True Then
                            Response.Write("<script>window.close();</script>")
                        Else
                            Exit Sub
                        End If

                    Case "Save"

                        If imgSave.Enabled = False Or imgSave.Visible = False Then
                            cpnlErrorPanel.Visible = True
                            lstError.Items.Add("You don't have access rights to Save record...")
                            Exit Sub
                        End If

                        SaveView()
                    Case "Delete"
                        Call DeleteView()
                    Case "Update"
                        Call UpdateView()
                End Select
            Catch ex As Exception
                CreateLog("AB_ViewColumn", "Load-121", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            End Try
        End If
        Call txtCSS(Me.Page)
        If Not IsPostBack Then
            Dim str As String
            str = HttpContext.Current.Session("PropRootDir")
            Dim obj As New clsSecurityCache
            If obj.ScreenAccess(intID) = False Then
                Response.Redirect("../../frm_NoAccess.aspx")
            End If
            obj.ControlSecurity(Me.Page, intID)
            'End of Security Block

            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1
            lstError.Items.Clear()
            cpnlErrorPanel.Visible = False
        End If
        If Not IsPostBack Then
            FillDefaultColumns()
            FillViewName()
            RemoveUserItemsFromDefault()
        End If

    End Sub
#End Region

#Region "FillDefaultColumns"
    '*******************************************************************
    ' Function             :-  FillDefaultColumns
    ' Purpose              :- Function will fatch value from data base and filled the default list view 
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/09/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub FillDefaultColumns()

        Dim mstrSourceId As String = "WebForm1.asp"
        Dim mstrAppId As String = "AppDemo"
        Dim myItem As ListItem

        Try

            lstdefaultcolumn.Items.Clear()
            Dim dsdataset As DataSet = New DataSet
            Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'change made after implement security
            '*********************************************************************************************
            Dim strSQL As String = "Select OBM.OBM_IN4_Object_ID_PK as ObjectID,ROD.ROD_VC50_Alias_Name ColName," _
                                    & " OBM.OBM_VC200_URL as ColValue,OBM.OBM_VC200_Descr as ColWidth  from " _
                                    & " T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                                    & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                                    & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =" & intID & " And " _
                                    & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                                    & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                                    & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                                    & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
                                    & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                                    & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & HttpContext.Current.Session("PropRole") & " AND " _
                                    & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK " _
                                    & " order by OBM.OBM_SI2_Order_By"

            '***************************************************************************************************
            SQL.DBConnection = strConnection
            SQL.DBTracing = False
            If SQL.Search("T030212", "AB_ViewColumns", "FillDefaultColumns-192", strSQL, dsdataset, "sachin", "Prashar") = True Then
                'fill dataset and put enteries to the list box
                For inti As Integer = 0 To dsdataset.Tables(0).Rows.Count - 1
                    myItem = New ListItem
                    myItem.Text = dsdataset.Tables(0).Rows(inti).Item("ColName")
                    myItem.Value = dsdataset.Tables(0).Rows(inti).Item("ColValue")
                    If myItem.Text = "C" Or myItem.Text = "A" Or myItem.Text = "F" Or myItem.Text = "AssignByID" Or myItem.Text = "SuppOwnID" Or myItem.Text = "CallReqByID" Or myItem.Text = "CallEntByID" Or myItem.Text = "CoordinatorID" Then
                    Else
                        lstdefaultcolumn.Items.Add(myItem)
                    End If
                Next
            End If
            'Added mandatory field in user list according to screens ID wise
            'Date 1-1-2007 modified by sachin
            '**********************************************************************
            If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                lstusercolumn.Items.Add(New ListItem("CallNo", "TM_NU9_Call_No_FK"))
                lstusercolumn.Items.Add(New ListItem("TaskNo", "TM_NU9_Task_no_PK"))
                lstusercolumn.Items.Add(New ListItem("CompID", "TM_NU9_Comp_ID_FK"))
            ElseIf intID = 463 Or intID = 799 Or intID = 1024 Then
                lstusercolumn.Items.Add(New ListItem("CallNo", "CM_NU9_Call_No_PK"))
                lstusercolumn.Items.Add(New ListItem("CompID", "CM_NU9_Comp_Id_FK"))
                lstusercolumn.Items.Add(New ListItem("SuppComp", "CM_NU9_CustID_FK"))
            ElseIf intID = 40 Then
                lstusercolumn.Items.Add(New ListItem("SubCategory", "PR_NU9_Project_ID_Pk"))
                lstusercolumn.Items.Add(New ListItem("SubCategoryName", "PR_VC20_Name"))
                lstusercolumn.Items.Add(New ListItem("SubCategoryComp", "PR_NU9_Comp_ID_FK"))
            ElseIf intID = 229 Then
                lstusercolumn.Items.Add(New ListItem("AddNo", "CI_NU8_Address_Number"))
            End If
            '***********************************************************************************
            FillOrderingGrd()
        Catch ex As Exception
            Dim intExceptionNumber As String
            '\\find the unique number for the exception
            intExceptionNumber = ex.TargetSite.Attributes
            '\\Ion.Logs called
            CreateLog("AB_ViewColumn", "FillDefaultColumns-208", EventLogging.LogType.Application, EventLogging.LogSubType.Exception, intExceptionNumber, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
#End Region

#Region " Button Events to move data between two ListView"

    Private Sub btnaddall1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnaddall1.Click
        AddRemoveAll(lstdefaultcolumn, lstusercolumn)
        FillOrderingGrd()
    End Sub
    Private Sub btnColmoveup1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnColmoveup1.Click
        MoveUp(lstusercolumn)
        FillOrderingGrd()
    End Sub
    '*******************************************************************
    ' Function             :-  MoveDown
    ' Purpose              :- Function will move data one step downwards in list 
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/09/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub MoveDown(ByVal lstBox As ListBox)

        Try

            Dim iIndex As Integer
            Dim iCount As Integer
            Dim iOffset As Integer
            Dim iInsertAt As Integer
            Dim iIndexSelectedMarker As Integer = -1
            Dim lItemData As String
            Dim lItemval As String
            iCount = lstBox.Items.Count
            iIndex = iCount - 1
            iOffset = 1

            While iIndex >= 0
                If lstBox.SelectedIndex >= 0 Then
                    lItemData = lstBox.SelectedItem.Text.ToString
                    lItemval = lstBox.SelectedItem.Value.ToString
                    iIndexSelectedMarker = lstBox.SelectedIndex
                    If Not (-1 = iIndexSelectedMarker) Then
                        Dim iIndex2 As Integer = 0
                        While iIndex2 < iCount - 1
                            If lItemval = lstBox.Items(iIndex2).Value.ToString Then
                                lstBox.Items.RemoveAt(iIndex2)

                                iInsertAt = Microsoft.VisualBasic.IIf((iIndex2 + iOffset) < 0, 0, iIndex2 + iOffset)
                                Dim li As ListItem = New ListItem(lItemData, lItemval)
                                lstBox.Items.Insert(iInsertAt, li)

                                Exit While
                            End If
                            System.Threading.Interlocked.Increment(iIndex2)
                        End While
                    End If
                End If
                iIndex = iIndex - 1
            End While
            If iIndexSelectedMarker = lstBox.Items.Count - 1 Then
                lstBox.SelectedIndex = iIndexSelectedMarker
            Else
                lstBox.SelectedIndex = iIndexSelectedMarker + 1
            End If
            '***********Rvs Start************
            Call ChngArrSequence()
            '***********End************
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "MoveDown-265", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    '*******************************************************************
    ' Function             :-  MoveUp
    ' Purpose              :- Function will move data Upwards in list 
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/09/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub MoveUp(ByVal lstBox As ListBox)
        Dim iIndex As Integer
        Dim iCount As Integer
        Dim iOffset As Integer
        Dim iInsertAt As Integer
        Dim iIndexSelectedMarker As Integer = -1
        Dim lItemData As String
        Dim lItemval As String
        Try
            iCount = lstBox.Items.Count
            iIndex = 0
            iOffset = -1
            While iIndex < iCount
                If lstBox.SelectedIndex > 0 Then
                    lItemval = lstBox.SelectedItem.Value.ToString
                    lItemData = lstBox.SelectedItem.Text.ToString
                    iIndexSelectedMarker = lstBox.SelectedIndex
                    If Not (-1 = iIndexSelectedMarker) Then
                        Dim iIndex2 As Integer = 0

                        While iIndex2 < iCount
                            If lItemval = lstBox.Items(iIndex2).Value.ToString Then
                                lstBox.Items.RemoveAt(iIndex2)
                                iInsertAt = Microsoft.VisualBasic.IIf((iIndex2 + iOffset) < 0, 0, iIndex2 + iOffset)
                                Dim li As ListItem = New ListItem(lItemData, lItemval)
                                lstBox.Items.Insert(iInsertAt, li)

                                ' break 
                            End If
                            System.Threading.Interlocked.Increment(iIndex2)
                        End While

                    End If
                Else
                    If -1 = iIndexSelectedMarker Then
                        iIndexSelectedMarker = iIndex
                        ' break 
                    End If
                End If
                iIndex = iIndex + 1
            End While
            If iIndexSelectedMarker = 0 Then
                lstBox.SelectedIndex = iIndexSelectedMarker
            Else

                lstBox.SelectedIndex = iIndexSelectedMarker - 1
            End If
            '***********Rvs Start************
            Call ChngArrSequence()
            '***********End************
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "MoveUp-315", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Sub btnColmovedwn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnColmovedwn1.Click

        MoveDown(lstusercolumn)
        FillOrderingGrd()
    End Sub
    Private Sub btnremoveall1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnremoveall1.Click
        AddRemoveAll(lstusercolumn, lstdefaultcolumn)
        FillOrderingGrd()
    End Sub
    '*******************************************************************
    ' Function             :-  AddRemoveAll
    ' Purpose              :- Function will remove the data from  list control
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/09/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub AddRemoveAll(ByVal aSource As ListBox, ByVal aTarget As ListBox)

        Try

            Dim licCollection As New ListItemCollection
            Dim intCount As Integer = 0
            While intCount < aSource.Items.Count
                licCollection.Add(aSource.Items(intCount))
                System.Math.Min(System.Threading.Interlocked.Increment(intCount), intCount - 1)
            End While
            '****************************************
            Dim intCount1 As Integer = 0
            Dim inti As Integer = 0
            Dim intListCount As Integer = lstusercolumn.Items.Count
            Dim rslt As String
            Dim intRemoveItems As Integer

            While inti <= intListCount - 1
                If lstusercolumn.Items.Count > 0 Then
                    For intRemoveItems = 0 To lstdefaultcolumn.Items.Count - 1
                        If lstusercolumn.Items(inti).Value.Equals(lstdefaultcolumn.Items(intRemoveItems).Value) Then
                            rslt = "Yes"
                            Exit For
                        Else
                            rslt = "No"
                        End If
                    Next
                    If rslt = "Yes" Then
                        licCollection.Remove(lstdefaultcolumn.Items(intRemoveItems))
                        System.Math.Min(System.Threading.Interlocked.Increment(intCount1), intCount1 - 1)
                    End If
                End If
                System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
            End While
            '*******************************************
            intCount1 = 0

            While intCount1 < licCollection.Count
                aSource.Items.Remove(licCollection(intCount1))
                aTarget.Items.Add(licCollection(intCount1))
                System.Math.Min(System.Threading.Interlocked.Increment(intCount1), intCount1 - 1)
            End While
            'For Each item As ListItem In aSource.Items
            '    aTarget.Items.Add(item)
            'Next
            'aSource.Items.Clear()
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "AddRemoveAll-337", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub

    Private Sub btnremoveitem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnremoveitem1.Click
        AddRemoveItem(lstusercolumn, lstdefaultcolumn, False)
        '***********Rvs Start************
        Call ChngArrSequence()
        '***********End************
        FillOrderingGrd()
    End Sub

    Private Sub AddRemoveItem(ByVal aSource As ListBox, ByVal aTarget As ListBox, ByVal Add As Boolean)
        Dim licCollection As ListItemCollection
        Try
            licCollection = New ListItemCollection
            Dim intCount As Integer = 0

            If Add = True Then
                If lstusercolumn.Items.Count = 0 Then
                    While intCount < aSource.Items.Count
                        If aSource.Items(intCount).Selected = True Then
                            licCollection.Add(aSource.Items(intCount))
                        End If
                        System.Math.Min(System.Threading.Interlocked.Increment(intCount), intCount - 1)
                    End While

                    Dim intCount1 As Integer = 0
                    While intCount1 < licCollection.Count
                        aSource.Items.Remove(licCollection(intCount1))
                        aTarget.Items.Add(licCollection(intCount1))
                        System.Math.Min(System.Threading.Interlocked.Increment(intCount1), intCount1 - 1)
                    End While
                Else

                    Dim FlgFound As Short
                    While intCount < aSource.Items.Count
                        For inti As Integer = 0 To lstusercolumn.Items.Count - 1
                            ' If Not lstdefaultcolumn.SelectedItem.Equals(lstusercolumn.Items(inti)) Then
                            If Not lstdefaultcolumn.SelectedValue.Equals(lstusercolumn.Items(inti).Value) Then
                                'if data not found in user selected columns list
                                FlgFound = 1
                            Else
                                ' if data field already in user list
                                'FlgFound = 2
                                Exit While
                            End If
                        Next

                        If FlgFound = 1 Then
                            If aSource.Items(intCount).Selected = True Then
                                licCollection.Add(aSource.Items(intCount))
                                Exit While
                            End If
                        End If

                        System.Math.Min(System.Threading.Interlocked.Increment(intCount), intCount - 1)
                    End While

                    Dim intCount1 As Integer = 0

                    While intCount1 < licCollection.Count
                        aSource.Items.Remove(licCollection(intCount1))
                        aTarget.Items.Add(licCollection(intCount1))
                        System.Math.Min(System.Threading.Interlocked.Increment(intCount1), intCount1 - 1)
                    End While
                End If

            Else

                'check mandatory field
                'Date  02-01-2007 modified by sachin prashar
                '*****************************************************************************************************
                If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                    If lstusercolumn.SelectedItem.Text.ToUpper = "CallNo".ToUpper Or lstusercolumn.SelectedItem.Text.ToUpper = "TaskNo".ToUpper Or lstusercolumn.SelectedItem.Text.ToUpper = "CompID".ToUpper Then

                        lstError.Items.Clear()
                        lstError.Items.Add("Mandatory Fields should be in user List...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                ElseIf intID = 463 Or intID = 799 Or intID = 1024 Then
                    If lstusercolumn.SelectedItem.Text.ToUpper = "CallNo".ToUpper Or lstusercolumn.SelectedItem.Text.ToUpper = "CompID".ToUpper Or lstusercolumn.SelectedItem.Text.ToUpper = "SuppComp".ToUpper Then

                        lstError.Items.Clear()
                        lstError.Items.Add("Mandatory Fields should be in user List...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                ElseIf intID = 229 Then
                    If lstusercolumn.SelectedItem.Text.ToUpper = "AddNo".ToUpper Then

                        lstError.Items.Clear()
                        lstError.Items.Add("Mandatory Fields should be in user List...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                        Exit Sub
                    End If
                ElseIf intID = 40 Then
                    If lstusercolumn.SelectedItem.Text.ToUpper = "SubCategory".ToUpper Or lstusercolumn.SelectedItem.Text.ToUpper = "SubCategoryName".ToUpper Or lstusercolumn.SelectedItem.Text.ToUpper = "SubCategoryComp".ToUpper Then

                        lstError.Items.Clear()
                        lstError.Items.Add("Mandatory Fields should be in user List...")
                        ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)

                        Exit Sub
                    End If
                End If
                '*****************************************************************************************************

                While intCount < aSource.Items.Count
                    If aSource.Items(intCount).Selected = True Then
                        licCollection.Add(aSource.Items(intCount))
                    End If
                    System.Math.Min(System.Threading.Interlocked.Increment(intCount), intCount - 1)
                End While

                '****************************************
                Dim intCount1 As Integer = 0
                Dim inti As Integer = 0
                Dim sts As String = "No"

                While inti < lstdefaultcolumn.Items.Count
                    If lstusercolumn.Items.Count > 0 Then
                        If IsNothing(lstusercolumn.SelectedItem) = False Then
                            '  If lstusercolumn.SelectedItem.Equals(lstdefaultcolumn.Items(inti)) Then
                            If lstusercolumn.SelectedValue.Equals(lstdefaultcolumn.Items(inti).Value) Then
                                lstusercolumn.Items.Remove(licCollection(0))
                                licCollection.Remove(lstdefaultcolumn.Items(inti))
                                sts = "Yes"
                                System.Math.Min(System.Threading.Interlocked.Increment(intCount1), intCount1 - 1)
                            End If
                        Else
                            Exit While
                        End If
                    End If
                    System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                End While
                '*******************************************
                intCount1 = 0
                If sts = "No" Then
                    While intCount1 < licCollection.Count
                        aSource.Items.Remove(licCollection(intCount1))
                        aTarget.Items.Add(licCollection(intCount1))
                        System.Math.Min(System.Threading.Interlocked.Increment(intCount1), intCount1 - 1)
                    End While
                End If
            End If
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "AddRemoveItem-426", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        Finally
            licCollection = Nothing
        End Try
    End Sub

    Private Sub btnadditem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnadditem1.Click
        AddRemoveItem(lstdefaultcolumn, lstusercolumn, True)
        '***********Rvs Start************
        Call ChngArrSequence()
        '***********End************
        FillOrderingGrd()
    End Sub
#End Region

#Region "SAve VIEW Data"

    '*******************************************************************
    ' Function             :-  SaveView
    ' Purpose              :- Function will save the data in database
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/09/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Function SaveView() As Boolean
        Dim shError As Short
        lstError.Items.Clear()
        'Security Block
        If imgSave.Enabled = False Or imgSave.Visible = False Then
            cpnlErrorPanel.Visible = True
            lstError.Items.Add("You don't have access rights to Save record...")
            ' Return False
            Exit Function
        End If
        'End of Security Block
        Try
            'fill value for AD SO fields
            '***************
            GetADSOValue()
            '***************
            'display error if view name blank
            '***********************
            If txtViewName.Text.Equals("") Then
                lstError.Items.Add("View Name cannot be blank...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            '******************************
            'display error message if view primery key not add in view Addno
            '**********************************
            Dim lstItem As ListItem

            Select Case intID
                Case "229"
                    lstItem = lstusercolumn.Items.FindByText("AddNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include AddNo... ")
                        shError = 1
                    End If
                Case "463" Or "799" Or "1024"
                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("SuppComp")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include SuppComp...")
                        shError = 1
                    End If
                Case "464"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If
                Case "2212"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If
                Case "502"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If
                Case "536"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If
                Case "40"
                    lstItem = lstusercolumn.Items.FindByText("SubCategory")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include SubCategory...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("SubCategoryName")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include SubCategoryName...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("SubCategoryComp")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include SubCategoryComp...")
                        shError = 1
                    End If

                Case Else
                    lstItem = lstusercolumn.Items(0)
            End Select

            If shError = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            '***********************************
            'display error message if no columns selected for view
            '****************************************
            If lstusercolumn.Items.Count <= 0 Then
                lstError.Items.Add("Select column first..")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            '****************************************
            'Display error message if view already exist
            '**************************************
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            Dim strCheck As String = SQL.Search("AB_ViewColumns", "SaveView-603", "select UV_VC50_View_Name from T030201 where UV_VC50_View_Name='" & txtViewName.Text.Trim.Replace("'", "''") & "' and UV_IN4_Role_ID=" & Val(Session("PropRole")) & " and UV_VC50_tbl_Name='" & intID & "' and UV_NU9_Comp_ID=" & Val(Session("PropCompanyID")) & "")

            If Not IsNothing(strCheck) Then
                lstError.Items.Add("View Name already exist in the database...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Function
            End If
            '**************************************
        Catch ex As Exception
            lstError.Items.Add("Error Occured While Save...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("AB_ViewColumn", "SaveView-582", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
            Exit Function
        End Try

        Dim intViewID As Integer

        Try
            lstError.Items.Clear()
            Dim sqdrCol As SqlDataReader
            Dim blnReturn As Boolean
            Dim strSearch As String = String.Empty
            For intColumns As Integer = 0 To lstusercolumn.Items.Count - 1
                strSearch &= " select  OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_Descr as ColWidth from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                               & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                               & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =" & intID & "  And " _
                               & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                               & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                               & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                               & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
                               & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                               & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & Val(HttpContext.Current.Session("PropRole")) & " AND " _
                               & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=" & intID & " and obm_vc4_object_type_fk='VIW')  and OBM.OBM_VC200_URL='" & lstusercolumn.Items(intColumns).Value & "' Union all "
            Next
            strSearch = strSearch.Remove(Len(strSearch) - 10, 9)
            ' get the Col_value from the table which will be used for storing when a  a new user view is created
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T030212"
            SQL.DBTracing = False
            sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-669", strSearch, SQL.CommandBehaviour.CloseConnection, blnReturn)
            Dim arView As New ArrayList
            Dim arColWidth As New ArrayList
            ' if no record is returned then exit sub
            If blnReturn = False Then
                Exit Function
            Else
                ' get the col_value in a arraylist
                While sqdrCol.Read
                    arView.Add(sqdrCol.Item("OBM_VC200_URL"))
                    arColWidth.Add(sqdrCol.Item("ColWidth"))
                End While
                ' close the reader
                sqdrCol.Close()
                sqdrCol = SQL.Search("AB_ViewColumns", "SaveView-686", "select isnull(max(UV_IN4_View_ID),0) as UV_IN4_View_ID from T030201", SQL.CommandBehaviour.CloseConnection, blnReturn)
                If blnReturn = False Then
                    Exit Function
                Else
                    While sqdrCol.Read
                        intViewID = sqdrCol.Item("UV_IN4_View_ID")
                        intViewID += 1
                    End While
                    sqdrCol.Close()
                End If
                Dim arColumnName As New ArrayList
                Dim arRowData As New ArrayList
                'save data in userview name "T030201"
                arColumnName.Add("UV_VC50_View_Name")
                arColumnName.Add("UV_IN4_View_ID")
                arColumnName.Add("UV_VC50_tbl_Name")
                arColumnName.Add("UV_IN4_Role_ID")
                arColumnName.Add("UV_NU9_Comp_ID")
                arColumnName.Add("UV_NU9_User_ID") 'Added new field to store user id with view records


                arRowData.Add(txtViewName.Text.Trim)
                arRowData.Add(intViewID)
                arRowData.Add(intID)
                arRowData.Add(Session("PropRole"))
                arRowData.Add(Session("PropCompanyID"))
                arRowData.Add(Session("PropUserID"))

                Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' save the view name
                SQL.DBConnection = strConnection
                ' SQL.DBTable = "T030201"
                SQL.DBTracing = False

                If SQL.Save("T030201", "AB_ViewColumns", "SaveView-720", arColumnName, arRowData) = True Then
                    ' get the view ID from the table               
                    ' arraylist for columns and records
                    Dim arCol As New ArrayList
                    Dim arRow As New ArrayList
                    arCol.Clear()
                    arRow.Clear()

                    Dim Multi As SQL.AddMultipleRows
                    SQL.DBConnection = strConnection
                    ' SQL.DBTable = "T030212"
                    SQL.DBTracing = False
                    ' define column name
                    arCol.Add("UV_IN4_ID")
                    arCol.Add("UV_IN4_View_ID")
                    arCol.Add("UV_VC50_COL_Name")
                    arCol.Add("UV_VC50_COL_Value")
                    arCol.Add("UV_SI2_Order_By")
                    arCol.Add("UV_VC10_Col_width")
                    arCol.Add("UV_VC50_tbl_Name")
                    arCol.Add("UV_VC5_AD")
                    arCol.Add("UV_NU9_SO")
                    arCol.Add("UV_VC5_FA")
                    arCol.Add("UV_VC20_Value")
                    Dim intSelectedColumns As Integer
                    ' add multiple rows in the dataset
                    For intSelectedColumns = 0 To lstusercolumn.Items.Count - 1
                        'If shFlag = 1 Then
                        'arRow.Add(intSelectedColumns + 2)
                        'Else
                        'End If
                        arRow.Add(intSelectedColumns + 1)
                        arRow.Add(intViewID)
                        arRow.Add(lstusercolumn.Items(intSelectedColumns).Text)
                        arRow.Add(arView.Item(intSelectedColumns))
                        arRow.Add(1)
                        arRow.Add(arColWidth.Item(intSelectedColumns))
                        arRow.Add(intID)
                        arRow.Add(arddlADValue.Item(intSelectedColumns))
                        If artxtBoxValue.Item(intSelectedColumns) = "" Then
                            arRow.Add(0)
                        Else
                            arRow.Add(artxtBoxValue.Item(intSelectedColumns))
                        End If
                        arRow.Add(arddlFAValue.Item(intSelectedColumns))
                        If arddlFAValue.Item(intSelectedColumns) = "" Then
                            arRow.Add("")
                        Else
                            ' arRow.Add(artxtBoxFaValue.Item(intSelectedColumns))
                            arRow.Add(IIf(IsNumeric(artxtBoxFaValue.Item(intSelectedColumns)), artxtBoxFaValue.Item(intSelectedColumns), "'" & artxtBoxFaValue.Item(intSelectedColumns) & "'"))
                        End If
                        'arRow.Add(IIf(IsNumeric(artxtBoxFaValue.Item(intSelectedColumns)), artxtBoxFaValue.Item(intSelectedColumns), "'" & artxtBoxFaValue.Item(intSelectedColumns) & "'"))
                        Multi.Add("T030212", arCol, arRow)
                    Next
                    Dim IntIncrementID As Integer
                    IntIncrementID = intSelectedColumns
                    If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                        IntIncrementID = IntIncrementID + 1
                        'add comment column
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("SuppOwnID")
                        arRow.Add("TM_VC8_Supp_Owner")

                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")

                        Multi.Add("T030212", arCol, arRow)
                        '******************************************
                        'add comment column
                        IntIncrementID = IntIncrementID + 1
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("AssignByID")
                        arRow.Add("TM_NU9_Assign_by")

                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")

                        Multi.Add("T030212", arCol, arRow)
                        '******************************************
                    End If

                    If intID = 464 Then
                        'add comment column for adding call ciolumns in task view
                        IntIncrementID = IntIncrementID + 1
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("CallReqByID")
                        arRow.Add("CM_NU9_Call_Owner")

                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")

                        Multi.Add("T030212", arCol, arRow)

                        'add comment column
                        IntIncrementID = IntIncrementID + 1
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("CallEntByID")
                        arRow.Add("CM_VC100_By_Whom")

                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")
                        Multi.Add("T030212", arCol, arRow)

                        'add comment column
                        IntIncrementID = IntIncrementID + 1
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("CoordinatorID")
                        arRow.Add("CM_NU9_Coordinator")

                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")
                        Multi.Add("T030212", arCol, arRow)

                    End If

                    If intID = 464 Or intID = 502 Or intID = 463 Or intID = 799 Or intID = 536 Or intID = 1024 Or intID = 2212 Then
                        'forattachment columns and comment
                        '***********************************************
                        'add comment column
                        IntIncrementID = IntIncrementID + 1
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("C")
                        If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                            arRow.Add("TM_CH1_Comment")
                        Else
                            arRow.Add("CM_CH1_Comment") '' view for call view screen
                        End If
                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")

                        Multi.Add("T030212", arCol, arRow)
                        '******************************************
                        'add attachment column
                        IntIncrementID = IntIncrementID + 1

                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("A")
                        If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                            arRow.Add("TM_CH1_Attachment")
                        Else
                            arRow.Add("CM_NU8_Attach_No")
                        End If
                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")

                        Multi.Add("T030212", arCol, arRow)
                        '*********************************************************
                    End If
                    If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                        'for add form column
                        'add attachment column
                        IntIncrementID = IntIncrementID + 1
                        arRow.Add(IntIncrementID)
                        arRow.Add(intViewID)
                        arRow.Add("F")
                        If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                            arRow.Add("TM_CH1_Forms")
                        Else

                        End If
                        arRow.Add(1)
                        arRow.Add(10)
                        arRow.Add(intID)
                        arRow.Add("UnSorted")
                        arRow.Add(0)
                        arRow.Add("")
                        arRow.Add("")
                        Multi.Add("T030212", arCol, arRow)
                    End If
                    ' save row in the table detail view for that view
                    Multi.Save()
                    'fill data after save new view 
                    '***********************************************************
                    FillViewName()
                    '***********************************************************
                    arddlADValue.Clear()
                    artxtBoxValue.Clear()
                    lstError.Items.Clear()
                    lstError.Items.Add("Records Saved successfully...")
                    ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                    Return True
                End If

            End If

        Catch ex As Exception
            SQL.Delete("AB_ViewColumns", "SaveView-804", "Delete from T030201 where UV_IN4_View_ID=" & intViewID & " and UV_VC50_View_Name='" & txtViewName.Text.Trim & "' and UV_VC50_tbl_Name='" & intID & "'", SQL.Transaction.Serializable)
            lstError.Items.Add("Error Occured While Save...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            CreateLog("AB_ViewColumn", "SaveView-760", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
            Return False
        End Try

    End Function
#End Region

#Region "FillOrderingGrd"
    Private Sub FillOrderingGrd()

        Dim myDataTable As DataTable = New DataTable("ColTable")
        Dim myDataColumn1 As DataColumn
        Dim myDataColumn2 As DataColumn
        Dim myDataColumn3 As DataColumn
        Dim myDataColumn4 As DataColumn
        Dim myDataColumn5 As DataColumn
        Dim myDataRow As DataRow

        Try
            ' Create new DataColumn, set DataType, ColumnName and add to DataTable. 
            myDataColumn1 = New DataColumn
            myDataColumn1.DataType = System.Type.GetType("System.String")
            myDataColumn1.ColumnName = "UV_VC50_COL_Name"
            myDataColumn1.Caption = "ColumnName"

            ' Create new DataColumn, set DataType, ColumnName and add to DataTable. 
            myDataColumn2 = New DataColumn
            myDataColumn2.DataType = System.Type.GetType("System.String")
            myDataColumn2.ColumnName = "UV_VC5_AD"
            myDataColumn2.Caption = "A/D"


            ' Create new DataColumn, set DataType, ColumnName and add to DataTable. 
            myDataColumn3 = New DataColumn
            myDataColumn3.DataType = System.Type.GetType("System.String")
            myDataColumn3.ColumnName = "UV_NU9_SO"
            myDataColumn3.Caption = "SO"

            ' Create new DataColumn, set DataType, ColumnName and add to DataTable. 
            myDataColumn4 = New DataColumn
            myDataColumn4.DataType = System.Type.GetType("System.String")
            myDataColumn4.ColumnName = "UV_VC5_FA"
            myDataColumn4.Caption = "FA"

            ' Create new DataColumn, set DataType, ColumnName and add to DataTable. 
            myDataColumn5 = New DataColumn
            myDataColumn5.DataType = System.Type.GetType("System.String")
            myDataColumn5.ColumnName = "UV_VC20_Value"
            myDataColumn5.Caption = "Value"

            ' Add the Column to the DataColumnCollection.
            myDataTable.Columns.Add(myDataColumn1)
            myDataTable.Columns.Add(myDataColumn2)
            myDataTable.Columns.Add(myDataColumn3)
            myDataTable.Columns.Add(myDataColumn4)
            myDataTable.Columns.Add(myDataColumn5)
            Dim i As Integer
            i = 0
            While i < lstusercolumn.Items.Count
                ReDim Preserve MyViewState(lstusercolumn.Items.Count)
                myDataRow = myDataTable.NewRow()
                If lstusercolumn.Items(i).Text = "default" Or MyViewState.GetUpperBound(0) <= 0 Then
                    myDataRow("UV_VC50_COL_Name") = lstusercolumn.Items(i).Text
                    myDataRow("UV_VC5_AD") = "UnSorted"
                    myDataRow("UV_VC5_FA") = ""

                Else
                    myDataRow("UV_VC50_COL_Name") = IIf(IsNothing(MyViewState(i).ColumnName), lstusercolumn.Items(i).Text, MyViewState(i).ColumnName)
                    myDataRow("UV_VC5_AD") = IIf(IsNothing(MyViewState(i).AD), "UnSorted", MyViewState(i).AD)
                    myDataRow("UV_VC5_FA") = IIf(IsNothing(MyViewState(i).FA), "", MyViewState(i).FA)
                    myDataRow("UV_NU9_SO") = IIf(IsNothing(MyViewState(i).SO), "", MyViewState(i).SO)
                    myDataRow("UV_VC20_VALUE") = IIf(IsNothing(MyViewState(i).Value), "", MyViewState(i).Value)
                End If
                myDataTable.Rows.Add(myDataRow)
                i = i + 1
            End While
            GrdView.DataSource = myDataTable
            GrdView.DataBind()
            txtCountHID.Value = lstusercolumn.Items.Count
            AddValidation()
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "FillOrderingGrd-1111", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        FillOrderingGrd()
    End Sub

    Private Sub GetADSOValue()

        Dim ViewGrdItem As DataGridItem
        arddlADValue.Clear()
        artxtBoxValue.Clear()
        arddlFAValue.Clear()
        artxtBoxFaValue.Clear()
        arddlADValue.Clear()
        artxtBoxValue.Clear()
        arddlADValue.Clear()
        arddlFAValue.Clear()
        artxtBoxFaValue.Clear()

        For Each ViewGrdItem In GrdView.Items

            Dim myddlAD As New DropDownList
            myddlAD = CType(ViewGrdItem.Cells(1).Controls(1), DropDownList)

            Dim mytxtboxSOValue As New TextBox
            mytxtboxSOValue = CType(ViewGrdItem.Cells(2).Controls(1), TextBox)
            'get value for FA and value columns

            Dim myddlFA As New DropDownList
            myddlFA = CType(ViewGrdItem.Cells(3).Controls(1), DropDownList)

            Dim mytxtboxFAvalue As New TextBox
            mytxtboxFAvalue = CType(ViewGrdItem.Cells(4).Controls(1), TextBox)

            If myddlAD.SelectedItem.Text <> "" Then
                arddlADValue.Add(myddlAD.SelectedItem.Text)
            Else
                arddlADValue.Add("")
            End If

            If mytxtboxSOValue.Text <> "" Then
                artxtBoxValue.Add(mytxtboxSOValue.Text)
            Else
                artxtBoxValue.Add("")
            End If

            'get value for FA and value columns
            If myddlFA.SelectedItem.Text <> "" Then
                arddlFAValue.Add(myddlFA.SelectedItem.Text)
            Else
                arddlFAValue.Add("")
            End If

            If mytxtboxFAvalue.Text <> "" Then
                artxtBoxFaValue.Add(mytxtboxFAvalue.Text)
            Else
                artxtBoxFaValue.Add("")
            End If
        Next
    End Sub
    Private Sub AddValidation()
        Dim ViewGrdItem As DataGridItem

        For Each ViewGrdItem In GrdView.Items
            Dim myddlAD As DropDownList = CType(ViewGrdItem.Cells(1).Controls(1), DropDownList)
            Dim mytextbox As TextBox = CType(ViewGrdItem.Cells(2).Controls(1), TextBox)
            mytextbox.Attributes.Add("onkeyup", "ChkUniqueNumber();")
            mytextbox.Attributes.Add("onkeypress", "NumericOnly();")
        Next
    End Sub
    '*******************************************************************
    ' Function             :-  FillViewName
    ' Purpose              :- Function will fatch view names from data base and filled the dropdown list
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/09/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub FillViewName()

        Dim blnView As Boolean
        Dim sqrdView As SqlDataReader
        ddlview.Items.Clear()
        Try
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            If intID = 502 Then
                sqrdView = SQL.Search("AB_ViewColumns", "FillViewName-981", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='" & intID & "' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " and (UV_NU9_User_ID=" & Session("PropUserID") & " or UV_NU9_User_ID is null )  order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
            Else
                sqrdView = SQL.Search("AB_ViewColumns", "FillViewName-981", "select UV_VC50_View_Name, UV_IN4_View_ID from T030201 where uv_vc50_tbl_name='" & intID & "' and UV_IN4_Role_ID=" & Session("PropRole") & " and UV_NU9_Comp_ID=" & Session("PropCompanyID") & " order by uv_in4_view_id", SQL.CommandBehaviour.CloseConnection, blnView)
            End If
            If blnView = True Then
                ddlview.DataSource = sqrdView
                ddlview.DataTextField = "UV_VC50_View_Name"
                ddlview.DataValueField = "UV_IN4_View_ID"
                ddlview.DataBind()
                sqrdView.Close()
            Else
            End If
            ddlview.Items.Add("Default")
            ddlview.Items(ddlview.Items.Count - 1).Value = 0
            ddlview.SelectedIndex = ddlview.Items.Count - 1

        Catch ex As Exception
            CreateLog("AB_ViewColumns", "FillViewName-946", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", False)
        End Try
    End Sub
    Private Sub ddlview_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlview.SelectedIndexChanged

        cpnlErrorPanel.Visible = False
        txtViewName.Text = ""
        Dim dsView As New DataSet
        Dim myItem As ListItem

        Try
            FillDefaultColumns()
            ClearMyViewState()
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            SQL.DBTracing = False
            SQL.Search("T030212", "AB_ViewColumns", "DDLBiew-SelectedIndexChanged-1022", "select * from T030212 where uv_vc50_tbl_name='" & intID & "' and UV_IN4_View_ID=" & ddlview.SelectedValue & "and UV_VC50_COL_Name not in('A','C','F','AssignByID','SuppOwnID','CallReqByID','CallEntByID','CoordinatorID')  order by UV_IN4_ID", dsView, "", "")
            For inti As Integer = 0 To dsView.Tables("T030212").Rows.Count - 1
                dsView.Tables("T030212").Rows(inti).Item("UV_VC20_Value") = Replace(dsView.Tables("T030212").Rows(inti).Item("UV_VC20_Value"), "'", "")
                If dsView.Tables("T030212").Rows(inti).Item("UV_NU9_SO") = 0 Then
                    dsView.Tables("T030212").Rows(inti).Item("UV_NU9_SO") = Convert.DBNull
                End If
            Next
            dsView.AcceptChanges()
            'dsView.AcceptChanges()
            GrdView.DataSource = dsView.Tables("T030212")
            GrdView.DataBind()
            txtCountHID.Value = dsView.Tables("T030212").Rows.Count()
            'fill dataset and put enteries to the list box
            lstusercolumn.Items.Clear()
            ReDim MyViewState(dsView.Tables(0).Rows.Count - 1)
            For inti As Integer = 0 To dsView.Tables(0).Rows.Count - 1
                myItem = New ListItem
                myItem.Text = dsView.Tables(0).Rows(inti).Item("UV_VC50_COL_Name") 'fill alias name
                myItem.Value = dsView.Tables(0).Rows(inti).Item("UV_VC50_COL_Value") 'fill database columns name

                If myItem.Text = "C" Or myItem.Text = "A" Or myItem.Text = "F" Or myItem.Text = "AssignByID" Or myItem.Text = "SuppOwnID" Or myItem.Text = "CallReqByID" Or myItem.Text = "CallEntByID" Or myItem.Text = "CoordinatorID" Then
                Else
                    lstusercolumn.Items.Add(myItem)
                    MyViewState(inti).ColumnName = dsView.Tables(0).Rows(inti).Item("UV_VC50_COL_Name")
                    MyViewState(inti).AD = dsView.Tables(0).Rows(inti).Item("UV_VC5_AD")
                    MyViewState(inti).FA = IIf(IsDBNull(dsView.Tables(0).Rows(inti).Item("UV_VC5_FA")), "", dsView.Tables(0).Rows(inti).Item("UV_VC5_FA"))
                    MyViewState(inti).SO = IIf(IsDBNull(dsView.Tables(0).Rows(inti).Item("UV_NU9_SO")), "", dsView.Tables(0).Rows(inti).Item("UV_NU9_SO"))
                    MyViewState(inti).Value = IIf(IsDBNull(dsView.Tables(0).Rows(inti).Item("UV_VC20_Value")), "", dsView.Tables(0).Rows(inti).Item("UV_VC20_Value"))
                End If
            Next
            RemoveUserItemsFromDefault()
        Catch ex As Exception
            CreateLog("AB-ViewColumns", "SelectedIndexChanged-989", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "ddlView", )
        End Try
    End Sub
    Private Sub GrdView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdView.ItemDataBound

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            CType(e.Item.Cells(3).FindControl("ddlFA"), DropDownList).Attributes.Add("onchange", "javascript:validateFAValue();")
            CType(e.Item.Cells(2).Controls(1), TextBox).Attributes.Add("onkeyup", "javascript:ChkUniqueNumber();")
            CType(e.Item.Cells(2).Controls(1), TextBox).Attributes.Add("onkeypress", "javascript:NumericOnly();")
            CType(e.Item.Cells(4).Controls(1), TextBox).Attributes.Add("onclick", "javascript:validateFAValue();")
        End If

    End Sub
    '*******************************************************************
    ' Function             :-  DeleteView
    ' Purpose              :- Function will delete the views data from related tables
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/07/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub DeleteView()
        lstError.Items.Clear()
        If ddlview.SelectedItem.Text.Equals("Default") Then
            lstError.Items.Add("Default View cannot be deleted by any one...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
            Exit Sub
        End If

        If ddlview.SelectedItem.Text.Equals("") Then
            lstError.Items.Add("Please select view for delete...")
            ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
            Exit Sub
        End If

        Try

            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            If SQL.Delete("AB_ViewColumns", "DeleteView-1300", "delete from  T030213 where UV_IN4_View_ID='" & ddlview.SelectedValue & "' and UV_VC50_Screen_ID='" & intID & "'", SQL.Transaction.Serializable) = True Then
                SQL.Delete("AB_ViewColumns", "DeleteView-1301", "delete from T030212 where UV_IN4_View_ID='" & ddlview.SelectedValue & "'  and UV_VC50_tbl_Name='" & intID & "'", SQL.Transaction.Serializable)
                SQL.Delete("AB_ViewColumns", "Delete-View", "delete from T030201 where UV_IN4_VIEW_ID=" & ddlview.SelectedValue & " and UV_VC50_tbl_Name ='" & intID & "'", SQL.Transaction.Serializable)

                lstError.Items.Clear()
                lstError.Items.Add("View Deleted successfully... ")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                'to fill to do list session and set default value in sessions
                If intID = "502" Then
                    If Session("TDLViewValue") = ddlview.SelectedValue Then
                        Session("TDLViewName") = "Default"
                        Session("TDLViewValue") = 0
                    End If
                End If
                'to fill Call View session and set default value in sessions
                If intID = "463" Then
                    If Session("CallViewValue") = ddlview.SelectedValue Then
                        Session("CallViewName") = "Default"
                        Session("CallViewValue") = 0
                    End If
                End If
                If intID = "799" Then
                    If Session("CallViewSimpleValue") = ddlview.SelectedValue Then
                        Session("CallViewsimpleName") = "Default"
                        Session("CallViewSimpleValue") = 0
                    End If
                End If
                If intID = "1024" Then
                    If Session("CallViewSimpleValue") = ddlview.SelectedValue Then
                        Session("CallViewsimpleName") = "Default"
                        Session("CallViewSimpleValue") = 0
                    End If
                End If

                'to fill Task view session and set default value in sessions
                If intID = "464" Then
                    If Session("TaskViewValue") = ddlview.SelectedValue Then
                        Session("TaskViewName") = "Default"
                        Session("TaskViewValue") = 0
                    End If
                End If
                'to fill Historic view's session and set default value in sessions
                If intID = "536" Then
                    If Session("HistoricViewValue") = ddlview.SelectedValue Then
                        Session("HistoricViewName") = "Default"
                        Session("HistoricViewValue") = 0
                    End If
                End If
                If intID = "2212" Then
                    If Session("TaskViewValue") = ddlview.SelectedValue Then
                        Session("TaskViewName") = "Default"
                        Session("TaskViewValue") = 0
                    End If
                End If
                'fill data after save new view 
                '*****************************
                FillViewName()
                lstusercolumn.Items.Clear()
                FillOrderingGrd()
                '*******************************
            Else
                lstError.Items.Clear()
                lstError.Items.Add("Error occured while view delete...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgError)
            End If
            Session("Flag") = 1
        Catch ex As Exception
            CreateLog("AB_ViewColumns", "deleteView-1051", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    '*******************************************************************
    ' Function             :-  UpdateView
    ' Purpose              :- Function will Upadte the views data from related tables
    '    							
    ' Date					Author						Modification Date					Description
    ' 13/07/06			sachin prashar			-------------------					Created
    '
    ' Notes:
    ' Code: 
    '*******************************************************************
    Private Sub UpdateView()
        Try

            'Display error message
            '**********************************************************
            lstError.Items.Clear()
            If ddlview.SelectedItem.Text.Equals("Default") Then
                lstError.Items.Clear()
                lstError.Items.Add("Default View cannot be Updated by any one...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgWarning)
                Exit Sub
            End If

            If ddlview.SelectedItem.Text.Equals("") Then
                lstError.Items.Clear()
                lstError.Items.Add("Please select view for Update...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

            If lstusercolumn.Items.Count <= 0 Then
                lstError.Items.Add("Select column for View...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If

            Dim lstItem As ListItem
            Dim shError As Short

            Select Case intID
                Case "229"
                    lstItem = lstusercolumn.Items.FindByText("AddNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include AddNo... ")
                        shError = 1
                    End If
                Case "463" Or "799" Or "1024"
                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If
                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("SuppComp")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include SuppComp..")
                        shError = 1
                    End If

                Case "464"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If

                Case "2212"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If


                Case "502"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If

                Case "536"
                    lstItem = lstusercolumn.Items.FindByText("TaskNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Task No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CallNo")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include Call No...")
                        shError = 1
                    End If

                    lstItem = lstusercolumn.Items.FindByText("CompID")
                    If IsNothing(lstItem) = True Then
                        lstError.Items.Add("View must include CompID...")
                        shError = 1
                    End If
                Case Else
                    lstItem = lstusercolumn.Items(0)
            End Select
            If shError = 1 Then
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgInfo)
                Exit Sub
            End If
            '************************************************************
            'fill value for AD SO fields
            '**********************************************
            GetADSOValue()
            '**********************************************
            Dim sqdrCol As SqlDataReader
            Dim blnReturn As Boolean
            Dim strSearch As String = String.Empty
            ' Create sql query to get the Col_Value for the columns selected
            '****************************************************************
            For intColumns As Integer = 0 To lstusercolumn.Items.Count - 1
                strSearch &= " select  OBM.OBM_VC200_URL,ROD.ROD_VC50_ALIAS_NAME,OBM.OBM_VC200_Descr as ColWidth from  T070011 OBM,T070031 ROM,T070042 ROD,T060011 UM,T060022 RA " _
                   & " WHERE UM.UM_VC50_UserID ='" & HttpContext.Current.Session("PropUserName") & "' AND " _
                   & " UM.UM_IN4_Address_No_FK = RA.RA_IN4_AB_ID_FK AND OBM.OBM_IN4_Object_PID_FK =" & intID & "  And " _
                   & " RA.RA_DT8_Valid_UpTo >='" & Now.Date & "' AND RA.RA_VC4_Status_Code = 'ENB' AND " _
                   & " RA.RA_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                   & " ROM.ROM_DT8_End_Date >= '" & Now.Date & "' AND ROM.ROM_VC50_Status_Code_FK = 'ENB' AND " _
                   & " OBM.OBM_VC4_Object_Type_FK ='VIW'  and ROD.ROD_CH1_View_Hide <> 'H' and" _
                   & " ROD.ROD_IN4_Role_ID_FK = ROM.ROM_IN4_Role_ID_PK AND " _
                   & " OBM.OBM_VC4_Status_Code_FK = 'ENB' AND rod.ROD_IN4_Role_ID_FK =" & Val(HttpContext.Current.Session("PropRole")) & " AND " _
                   & " OBM.OBM_IN4_Object_ID_PK = ROD.ROD_IN4_Object_ID_FK and rod.rod_in4_object_id_fk in(select obm_in4_object_id_pk from t070011 where obm_in4_object_pid_fk=" & intID & " and obm_vc4_object_type_fk='VIW')  and OBM.OBM_VC200_URL='" & lstusercolumn.Items(intColumns).Value & "' Union all "
            Next
            strSearch = strSearch.Remove(Len(strSearch) - 10, 9)

            ' get the Col_value from the table which will be used for storing when a  a new user view is created
            SQL.DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
            'SQL.DBTable = "T030212"
            SQL.DBTracing = False
            sqdrCol = SQL.Search("AB_ViewColumns", "UpdateView-1205", strSearch, SQL.CommandBehaviour.CloseConnection, blnReturn)
            Dim arView As New ArrayList
            Dim arColWidth As New ArrayList
            ' if no record is returned then exit sub
            If blnReturn = False Then
                Exit Sub
            Else
                ' get the col_value in a arraylist
                While sqdrCol.Read
                    arView.Add(sqdrCol.Item("OBM_VC200_URL"))
                    arColWidth.Add(sqdrCol.Item("ColWidth"))
                End While
                ' close the reader
                sqdrCol.Close()
            End If
            '**************************************************************
            If SQL.Delete("AB_ViewColumns", "UpdateView-1229", "delete from T030212 where UV_IN4_View_ID='" & ddlview.SelectedValue & "'  and UV_VC50_tbl_Name='" & intID & "'", SQL.Transaction.Serializable) = True Then

                Dim strConnection As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
                ' arraylist for columns and records
                Dim arCol As New ArrayList
                Dim arRow As New ArrayList

                Dim Multi As SQL.AddMultipleRows
                SQL.DBConnection = strConnection
                SQL.DBTracing = False

                ' define column name
                '***************************************************
                arCol.Add("UV_IN4_ID")
                arCol.Add("UV_IN4_View_ID")
                arCol.Add("UV_VC50_COL_Name")
                arCol.Add("UV_VC50_COL_Value")
                arCol.Add("UV_SI2_Order_By")
                arCol.Add("UV_VC10_Col_width")
                arCol.Add("UV_VC50_tbl_Name")
                arCol.Add("UV_VC5_AD")
                arCol.Add("UV_NU9_SO")
                arCol.Add("UV_VC5_FA")
                arCol.Add("UV_VC20_Value")
                '************************************************************
                ' add multiple rows in the dataset
                '************************************************************
                Dim intSelectedColumns As Integer
                For intSelectedColumns = 0 To lstusercolumn.Items.Count - 1
                    arRow.Add(intSelectedColumns + 1)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add(lstusercolumn.Items(intSelectedColumns).Text)
                    arRow.Add(arView.Item(intSelectedColumns))
                    arRow.Add(1)
                    arRow.Add(arColWidth.Item(intSelectedColumns))
                    arRow.Add(intID)
                    arRow.Add(arddlADValue.Item(intSelectedColumns))
                    If artxtBoxValue.Item(intSelectedColumns) = "" Then
                        arRow.Add(0)
                    Else
                        arRow.Add(artxtBoxValue.Item(intSelectedColumns))
                    End If
                    arRow.Add(arddlFAValue.Item(intSelectedColumns))
                    'if factor is clear then clear the factor textbox value
                    '*****************************************
                    If arddlFAValue.Item(intSelectedColumns) = "" Then
                        arRow.Add("")
                    Else
                        arRow.Add(IIf(IsNumeric(artxtBoxFaValue.Item(intSelectedColumns)), artxtBoxFaValue.Item(intSelectedColumns), "'" & artxtBoxFaValue.Item(intSelectedColumns) & "'"))
                    End If
                    '************************************************
                    Multi.Add("T030212", arCol, arRow)
                Next
                '**************************************************
                Dim IntIncrementID As Integer
                IntIncrementID = intSelectedColumns
                If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                    IntIncrementID = IntIncrementID + 1

                    'add comment column
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("SuppOwnID")
                    arRow.Add("TM_VC8_Supp_Owner")

                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")

                    Multi.Add("T030212", arCol, arRow)
                    '******************************************
                    'add comment column
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("AssignByID")
                    arRow.Add("TM_NU9_Assign_by")

                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")
                    Multi.Add("T030212", arCol, arRow)
                    '******************************************
                End If

                If intID = 464 Then
                    'add comment column for adding call ciolumns in task view
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("CallReqByID")
                    arRow.Add("CM_NU9_Call_Owner")

                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")

                    Multi.Add("T030212", arCol, arRow)

                    'add comment column
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("CallEntByID")
                    arRow.Add("CM_VC100_By_Whom")

                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")
                    Multi.Add("T030212", arCol, arRow)


                    'add comment column
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("CoordinatorID")
                    arRow.Add("CM_NU9_Coordinator")

                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")
                    Multi.Add("T030212", arCol, arRow)

                End If

                If intID = 464 Or intID = 502 Or intID = 463 Or intID = 799 Or intID = 536 Or intID = 1024 Or intID = 2212 Then
                    'for attachment columns and comment and form columns
                    '***********************************************
                    'add comment column
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("C")
                    If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                        arRow.Add("TM_CH1_Comment")
                    Else
                        arRow.Add("CM_CH1_Comment")
                    End If
                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")
                    Multi.Add("T030212", arCol, arRow)
                    '******************************************
                    'add attachment column
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("A")
                    If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                        arRow.Add("TM_CH1_Attachment")
                    Else
                        arRow.Add("CM_NU8_Attach_No")
                    End If
                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")
                    Multi.Add("T030212", arCol, arRow)
                End If
                If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                    'for add form column
                    IntIncrementID = IntIncrementID + 1
                    arRow.Add(IntIncrementID)
                    arRow.Add(ddlview.SelectedValue)
                    arRow.Add("F")
                    If intID = 464 Or intID = 502 Or intID = 536 Or intID = 2212 Then
                        arRow.Add("TM_CH1_Forms")
                    Else

                    End If
                    arRow.Add(1)
                    arRow.Add(10)
                    arRow.Add(intID)
                    arRow.Add("UnSorted")
                    arRow.Add(0)
                    arRow.Add("")
                    arRow.Add("")
                    Multi.Add("T030212", arCol, arRow)
                End If
                ' save row in the table detail view for that view
                Multi.Save()
                'Clear shared array
                '******************************
                arddlADValue.Clear()
                artxtBoxValue.Clear()
                arddlADValue.Clear()
                arddlFAValue.Clear()
                artxtBoxFaValue.Clear()
                '****************************
                lstError.Items.Clear()
                lstError.Items.Add("View Records Updated successfully...")
                ShowMsgPenelNew(pnlMsg, lstError, mdlMain.MSG.msgOK)
                'fill data after save new view 
                '*****************************
                ' FillViewName()
                ' lstusercolumn.Items.Clear()
                'GetADSOValue()
                GrdView.Columns.Clear()
                '*******************************
            End If
        Catch ex As Exception
            CreateLog("AB_ViewColumns", "updateView-1219", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Private Sub ChngArrSequence()
        Try
            Dim TempViewState() As GridFilter
            Dim intI As Integer
            Dim intFoundIndex As Integer
            FillArrayFromGrid()
            TempViewState = MyViewState.Clone()

            For intI = 0 To lstusercolumn.Items.Count - 1
                MyViewState(intI).ColumnName = lstusercolumn.Items(intI).Text
                intFoundIndex = SearchArray(lstusercolumn.Items(intI).Text, TempViewState)
                If intFoundIndex > -1 Then
                    MyViewState(intI).AD = TempViewState(intFoundIndex).AD
                    MyViewState(intI).FA = TempViewState(intFoundIndex).FA
                    MyViewState(intI).SO = TempViewState(intFoundIndex).SO
                    MyViewState(intI).Value = TempViewState(intFoundIndex).Value
                End If
            Next

        Catch ex As Exception
            CreateLog("AB_ViewColumns", "ChngArrSequnce-1367", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Sub
    Private Function SearchArray(ByVal SearchValue As String, ByVal SArray() As GridFilter) As Integer
        Dim intIndexFound As Integer
        Try
            For intIndexFound = 0 To SArray.GetUpperBound(0)
                If SArray(intIndexFound).ColumnName = SearchValue Then
                    Return intIndexFound
                End If
            Next
            Return -1
        Catch ex As Exception
            CreateLog("AB_ViewColumns", "SearchArray-1384", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"))
        End Try
    End Function
    Private Sub FillArrayFromGrid()
        Dim i As Integer
        Try
            Call GetADSOValue()
            For i = 0 To MyViewState.GetUpperBound(0) - 1
                MyViewState(i).AD = arddlADValue.Item(i)
                MyViewState(i).SO = artxtBoxValue.Item(i)
                MyViewState(i).FA = arddlFAValue.Item(i)
                MyViewState(i).Value = artxtBoxFaValue.Item(i)
            Next
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "FillArrayFromGrid-1873", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    Protected Overrides Sub Finalize()
        If IsNothing(MyViewState) = False Then
            'MyViewState.Clear(MyViewState, 0, MyViewState.Length)
            Array.Clear(MyViewState, 0, MyViewState.Length)
        End If
        MyBase.Finalize()
    End Sub
    Private Sub ClearMyViewState()
        Dim i As Integer
        Try
            If IsNothing(MyViewState) = False Then
                For i = 0 To MyViewState.Length - 1
                    MyViewState(i).Value = ""
                    MyViewState(i).AD = ""
                    MyViewState(i).FA = ""
                    MyViewState(i).SO = ""
                Next
            End If
        Catch ex As Exception
            CreateLog("AB_ViewColumn", "ClearMyViewState-1961", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString, HttpContext.Current.Session("PropUserID"), HttpContext.Current.Session("PropUserName"), "NA", )
        End Try
    End Sub
    Private Sub RemoveUserItemsFromDefault()
        If (lstusercolumn.Items.Count > 0) Then
            For intRemoveItems = 0 To lstusercolumn.Items.Count - 1
                If (lstdefaultcolumn.Items.Contains(lstusercolumn.Items(intRemoveItems)) = True) Then
                    lstdefaultcolumn.Items.Remove(lstusercolumn.Items(intRemoveItems))
                End If
            Next
        End If
    End Sub

End Class
