Imports Microsoft.VisualBasic

Public Class clsPersonnal

   
    Private _firstName As String
    Private _lastName As String
    Private _prospectId As String

    Public Property FirstName As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Public Property LastName As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

    Public Property ProspectId As String
        Get
            Return _prospectId
        End Get
        Set(ByVal value As String)
            _prospectId = value
        End Set
    End Property


End Class
