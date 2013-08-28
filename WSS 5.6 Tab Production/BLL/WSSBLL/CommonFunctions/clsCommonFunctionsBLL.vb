#Region "Purpose"
' "CREATION"
' CREATED BY: [Tarun Pahuja]
' CREATED ON: [10/08/09]
' PURPOSE   : [This Class is used fill The dropdowns used in WSS..this class will have the common functions used to fill the Dropdowns]
' UPDATED BY: []
' UPDATED ON: [ ]
' PURPOSE   : [ ]
' TABLES    : [ ]
#End Region

#Region "NameSpace"
Imports ION.Common.DAL
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
#End Region

Public Class clsCommonFunctionsBLL
    Private objCommonFunctionDAL As WSSDAL.clsCommonFunctionsDAL

    Sub New(ByVal ConnectionString As String, ByVal Provider As String)
        objCommonFunctionDAL = New WSSDAL.clsCommonFunctionsDAL(ConnectionString, Provider)
    End Sub
#Region "Functions"
    ''' <summary>
    ''' This function will return the dataset to bind the Call Type RadDropdown
    ''' to fill call type DDL, one parameter is required.. i.e. Session("PropCAComp")
    ''' </summary>
    ''' <returns>dataset</returns>
    ''' <remarks></remarks>
    Public Function fillRadCallTypeDDL(ByVal strCompID As String) As DataSet
        Dim dsCallTypeDDL As New DataSet
        Try
            dsCallTypeDDL = objCommonFunctionDAL.fillRadCallTypeDDL(strCompID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-fillRadCallTypeDDL-36", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsCallTypeDDL
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskType(ByVal CompanyID As Integer) As DataSet
        Dim dsTaskTypeDDL As New DataSet
        Try
            dsTaskTypeDDL = objCommonFunctionDAL.FillRadTaskType(CompanyID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadTaskType-53", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskTypeDDL
    End Function
    Public Function FillRadCallOwnerDefault(ByVal CallNo As Integer) As String
        Dim strCallOwner As String = String.Empty
        Try
            strCallOwner = objCommonFunctionDAL.FillRadCallOwnerDefault(CallNo)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadCallOwner-53", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return strCallOwner
    End Function
    Public Function FilltxtMailIdDefault(ByVal CallNo As Integer, ByVal TaskNo As Integer) As DataSet
        Dim strTaskOwner As New DataSet
        Try
            strTaskOwner = objCommonFunctionDAL.FillMailIdDefault(CallNo, TaskNo)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FilltxtMailIdDefault-53", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return strTaskOwner
    End Function

    ''' <summary>
    ''' This function will return the dataset to bind the Requested By User RadDropdown
    ''' to fill call type DDL, three parameters are required.. i.e. Session("PropCAComp"),
    ''' </summary>
    ''' <returns>dataset</returns>
    ''' <remarks></remarks>
    Public Function fillRadRequestedByDDL(ByVal strCompID As String) As DataSet
        Dim dsRequestedByDDL As New DataSet
        Try
            dsRequestedByDDL = objCommonFunctionDAL.fillRadRequestedByDDL(strCompID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-fillRadRequestedByDDL-68", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsRequestedByDDL
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <param name="ProjectID"></param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskOwner(ByVal CompanyID As Integer, ByVal ProjectID As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = objCommonFunctionDAL.FillRadTaskOwner(CompanyID, ProjectID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadTaskOwner-79", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskPriority(ByVal CompanyID As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = objCommonFunctionDAL.FillRadTaskPriority(CompanyID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadTaskPriority-97", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Function FillRadStatus(ByVal CompanyID As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = objCommonFunctionDAL.FillRadStatus(CompanyID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadStatus-117", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Function FillRadCallPriority(ByVal CompanyID As Integer) As DataSet
        Dim dsCallPriority As New DataSet
        Try
            dsCallPriority = objCommonFunctionDAL.FillRadCallPriority(CompanyID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadCallPriority-127", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsCallPriority
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Function FillRadProject(ByVal CompanyID As Integer) As DataSet
        Dim dsProject As New DataSet
        Try
            dsProject = objCommonFunctionDAL.FillRadProjects(CompanyID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadProject-137", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsProject
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fillRadCommentType(ByVal CompanyID As Integer, ByVal CallNo As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = objCommonFunctionDAL.fillRadCommentType(CompanyID, CallNo)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadCommentType-157", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompanyID"></param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Function FillRadTaskOwnerForTaskForward(ByVal ProjectID As Integer, ByVal CompanyID As Integer, ByVal TaskOwnerID As Integer) As DataSet
        Dim dsTaskOwner As New DataSet
        Try
            dsTaskOwner = objCommonFunctionDAL.FillRadTaskOwnerForTaskForward(ProjectID, CompanyID, TaskOwnerID)
        Catch ex As Exception
            CreateLog("WSS", "clsCommonFunctionsBLL-FillRadCommentType-157", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
        End Try
        Return dsTaskOwner
    End Function

#End Region
End Class
