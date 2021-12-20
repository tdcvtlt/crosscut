Imports Microsoft.VisualBasic

Public Class clsRegisterRawItem

    Private _rawItemID As String
    Private _supc As String
    Private _description As String
    Private _brandId As String
    Private _measurementUnitId As String

    Public Property RawItemID As String
        Get
            Return _rawItemID
        End Get
        Set(ByVal value As String)
            _rawItemID = value
        End Set
    End Property

    Public Property SUPC As String
        Get
            Return _supc
        End Get
        Set(ByVal value As String)
            _supc = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Public Property BrandID As String
        Get
            Return _brandId
        End Get
        Set(ByVal value As String)
            _brandId = value
        End Set
    End Property

    Public Property MeasurementUnitID As String
        Get
            Return _measurementUnitId
        End Get
        Set(ByVal value As String)
            _measurementUnitId = value
        End Set
    End Property

    Public ReadOnly Property DateTimeStamp As String
        Get
            Return DateTime.Now.ToString()
        End Get
    End Property
End Class
