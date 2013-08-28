<%@ WebHandler Language="VB" Class="Handler" %>

Imports System
Imports System.Web
Imports System.IO
Imports ION.Logging.EventLogging

Public Class Handler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
                     
            Dim strFileName As String = context.Request.QueryString("FileName")
            Dim strFilePath As String = context.Request.QueryString("FilePath")
            
            If File.Exists(strFilePath) Then
                Dim strmFile As Stream = File.OpenRead(strFilePath)
                Dim buffer(strmFile.Length) As Byte

                strmFile.Read(buffer, 0, CType(strmFile.Length, Int32))
                context.Response.ClearHeaders()
                context.Response.ClearContent()
                context.Response.ContentType = "application/octet-stream"
              
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName)
                context.Response.BinaryWrite(buffer)
                context.Response.Flush()
                context.Response.End()
            Else
                context.Response.Write("<script>alert('File Not Found');</script>")
                context.Response.Write("<script>window.close();</script>")
            End If
                 
        Catch ex As Exception
            CreateLog("Handler", "ProcessRequest-34", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message, "", "", "")
        Finally
        End Try
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class