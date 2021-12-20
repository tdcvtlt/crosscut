Imports Microsoft.VisualBasic

Public Class IdName

    Dim _id As Integer = 0
    Dim _name As String = String.Empty

    Public Property ID As Integer
        Set(ByVal value As Integer)
            _id = value
        End Set
        Get
            Return _id
        End Get
    End Property

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
End Class
