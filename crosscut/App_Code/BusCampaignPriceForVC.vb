Imports System.Reflection

Partial Public Class BusCampaignPriceForVC
    Private Shared _4K As String = "338"
    Private Shared _Prestige As String = "338"
    Private Shared _Dealz As String = "350"
    Private Shared _SaveOn As String = "350"
    Private Shared _IMS As String = "350"
    Private Shared _RVI As String = "350"
    Private Shared _EMKO As String = "350"
    Private Shared _VTE As String = "375"
    Private Shared _Premier2 As String = "375"
    Private Shared _GMG As String = "375"
    Private Shared _MBA As String = "325"
    Private Shared _DST As String = "175"

    Public Sub New()
    End Sub

    Public Shared ReadOnly Property Dealz() As String
        Get
            Return _Dealz
        End Get
    End Property

    Public Shared ReadOnly Property DST() As String
        Get
            Return _DST
        End Get
    End Property

    Public Shared ReadOnly Property EMKO() As String
        Get
            Return _EMKO
        End Get
    End Property

    Public Shared ReadOnly Property FourK() As String
        Get
            Return _4K
        End Get
    End Property

    Public Shared ReadOnly Property GMG() As String
        Get
            Return _GMG
        End Get
    End Property

    Public Shared ReadOnly Property IMS() As String
        Get
            Return _IMS
        End Get
    End Property

    Public Shared ReadOnly Property MBA() As String
        Get
            Return _MBA
        End Get
    End Property

    Public Shared ReadOnly Property Premier2() As String
        Get
            Return _Premier2
        End Get
    End Property

    Public Shared ReadOnly Property Prestige() As String
        Get
            Return _Prestige
        End Get
    End Property

    Public Shared ReadOnly Property RVI() As String
        Get
            Return _RVI
        End Get
    End Property

    Public Shared ReadOnly Property SaveOn() As String
        Get
            Return _SaveOn
        End Get
    End Property

    Public Shared ReadOnly Property VTE() As String
        Get
            Return _VTE
        End Get
    End Property



    Public Shared Function GetCampaignPrice(ByVal campaign As String) As String
        Dim this As BusCampaignPriceForVC = New BusCampaignPriceForVC()
        Dim name As PropertyInfo = this.GetType().GetProperty(campaign)

        If name Is Nothing Then
            Return "375"
        Else
            Return DirectCast(name.GetValue(this, Nothing), String)
        End If
    End Function

End Class
