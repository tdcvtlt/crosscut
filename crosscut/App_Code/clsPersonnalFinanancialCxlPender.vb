Imports Microsoft.VisualBasic

Public Class clsPersonnalFinanancialCxlPender
    Inherits clsPersonnal


    Private _financials As List(Of clsFinancialAmount)
    Private _contractId As String
    Private _contractNumber As String
    Private _balance As String

    Public Property Financials As List(Of clsFinancialAmount)
        Get
            Return _financials
        End Get
        Set(ByVal value As List(Of clsFinancialAmount))
            _financials = value
        End Set
    End Property

    Public Property ContractID As String
        Get
            Return _contractId
        End Get
        Set(ByVal value As String)
            _contractId = value
        End Set
    End Property


    Public Property ContractNumber As String
        Get
            Return _contractNumber
        End Get
        Set(ByVal value As String)
            _contractNumber = value
        End Set
    End Property


    Public Property Balance As String
        Get
            Return _balance
        End Get
        Set(ByVal value As String)
            _balance = value
        End Set
    End Property

End Class
