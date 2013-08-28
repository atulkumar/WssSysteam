Imports Microsoft.VisualBasic
Namespace ION.BusinessUnit
    Public Class CommentViewBO
        Private _blnCommentFlag As Boolean
        Private _intPropUserID As Int32

        Public Property blnCommentFlag() As Boolean
            Get
                Return _blnCommentFlag
            End Get
            Set(ByVal value As Boolean)
                _blnCommentFlag = value
            End Set
        End Property

        Public Property intPropUserID() As Int32
            Get
                Return _intPropUserID
            End Get
            Set(ByVal value As Int32)
                _intPropUserID = value
            End Set
        End Property

    End Class
End Namespace
