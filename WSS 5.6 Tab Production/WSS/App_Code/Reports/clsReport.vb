
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class clsReport

    Private Shared crtableLogoninfos As New TableLogOnInfos
    Private Shared crtableLogoninfo As New TableLogOnInfo
    Private Shared crConnectionInfo As New ConnectionInfo
    Private Shared crTables As Tables
    Private Shared crTable As Table
    Private Shared crSections As Sections
    Private Shared crReportObjects As ReportObjects
    Private Shared crReportObject As ReportObject
    Private Shared crSubreportObject As SubreportObject
    Private Shared crSubreportDocument As ReportDocument
    Private Shared TableCounter
    Private Shared crDatabase As Database
    Private Shared connectioninfo As connectioninfo


    '*******************************************************************
    '' Author        :      Atul Sharama
    '' Create Date   :      18 March 2006
    '' Purpose       :      This function return the login information for the report documents 
    '                       Input: Report Document 
    '                       OutPut: Report Document with Login Information from the web.config file 
    ''*******************************************************************


    Public Shared Function LogonInformation(ByVal rptDocument As ReportDocument)
        Dim strConnection As String
        Dim strCollection(50) As String
        Dim strCollection2(50) As String
        Dim strCollection3(50) As String
        Dim strSample As String
        'Dim strSplitted As String
        Dim i, j As Integer
        strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        strCollection = Split(strConnection, ";")
        i = 0
        j = 0

        For i = 0 To 3
            strCollection2 = Split(strCollection(i), "=")
            ' strCollection3(i) = strCollection2(0)
            strCollection3(i) = strCollection2(1)
            strSample = strCollection3(i)
        Next i
        Try
            With crConnectionInfo
                .ServerName = strCollection3(0) '"Ion15"
                .DatabaseName = strCollection3(1)
                .UserID = strCollection3(2)
                .Password = strCollection3(3)
            End With
            crTables = rptDocument.Database.Tables
            'Loop through each table in the report and apply the 
            'LogonInfo information 
            For Each crTable In crTables
                crtableLogoninfo = crTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                crTable.ApplyLogOnInfo(crtableLogoninfo)
            Next
            'Check if SUbreport Exists and Apply logon information to all subreports
            ' set the sections object to the current report's section 
            crSections = rptDocument.ReportDefinition.Sections
            'crSections = crReportDocument.ReportDefinition.Sections; 
            ' loop through all the sections to find all the report objects 
            Dim crSection As Section
            For Each crSection In crSections
                crReportObjects = crSection.ReportObjects
                'loop through all the report objects in there to find all subreports 
                For Each crReportObject In crReportObjects
                    If crReportObject.Kind = ReportObjectKind.SubreportObject Then
                        crSubreportObject = CType(crReportObject, SubreportObject)
                        'open the subreport object and logon as for the general report 
                        crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName)
                        crDatabase = crSubreportDocument.Database
                        crTables = rptDocument.Database.Tables
                        For Each crTable In crTables
                            crtableLogoninfo = crTable.LogOnInfo
                            crtableLogoninfo.ConnectionInfo = crConnectionInfo
                            crTable.ApplyLogOnInfo(crtableLogoninfo)
                        Next
                    End If
                Next
            Next
        Catch ex As Exception
            Dim str As String = ex.Message.ToString
        End Try
    End Function
End Class
