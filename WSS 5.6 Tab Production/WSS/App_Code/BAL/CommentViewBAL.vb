Imports Microsoft.VisualBasic
Imports System.Data
Imports ION.Logging.EventLogging
Imports ION.DataLayer
Imports ION.BusinessUnit
Namespace ION.BusinessLogic

    Public Class CommentViewBAL
        Public Function GetComments(ByVal objCommentsViewBU As CommentViewBO) As DataSet
            'Get the Comments 
            Dim dsComments As DataSet = Nothing
            'created the CommentsViewDAL object and call the method to get the comments
            Dim objCommentsViewDAL As CommentViewDAL = New CommentViewDAL
            dsComments = objCommentsViewDAL.GetCommentsView(objCommentsViewBU)
            objCommentsViewDAL = Nothing

            If (dsComments.Tables(0).Rows.Count > 0) Then
                Dim htDateCols As New Hashtable
                htDateCols.Add("CommentDate", 1)
                SetDataTableDateFormat(dsComments.Tables(0), htDateCols)
            End If
            Return dsComments
        End Function
    End Class
End Namespace
