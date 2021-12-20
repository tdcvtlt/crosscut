Imports Microsoft.VisualBasic

Public Class clsFinancialAmount

    Private _id As String
    Private _acct As String
    Private _invoice As String
    Private _transDate As String
    Private _amount As String
    Private _balance As String
    Private _ccApproval As String
    Private _dueDate As String
    Private _invoiceAmount As String
    Private _keyValue As String

    Public Property ID As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Property Acct As String
        Get
            Return _acct
        End Get
        Set(ByVal value As String)
            _acct = value
        End Set
    End Property


    Public Property Invoice As String
        Get
            Return _invoice
        End Get
        Set(ByVal value As String)
            _invoice = value
        End Set
    End Property

    Public Property TransDate As String
        Get
            Return _transDate
        End Get
        Set(ByVal value As String)
            _transDate = value
        End Set
    End Property

    Public Property Amount As String
        Get
            Return _amount
        End Get
        Set(ByVal value As String)
            _amount = value
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

    Public Property CCApproval As String
        Get
            Return _ccApproval
        End Get
        Set(ByVal value As String)
            _ccApproval = value
        End Set
    End Property

    Public Property DueDate As String
        Get
            Return _dueDate
        End Get
        Set(ByVal value As String)
            _dueDate = value
        End Set
    End Property

    Public Property InvoiceAmount As String
        Get
            Return _invoiceAmount
        End Get
        Set(ByVal value As String)
            _invoiceAmount = value
        End Set
    End Property

    Public Property KeyValue As String
        Get
            Return _keyValue
        End Get
        Set(ByVal value As String)
            _keyValue = value
        End Set
    End Property





End Class
