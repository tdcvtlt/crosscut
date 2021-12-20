Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsFinancials
    Dim oFTC As clsFinancialTransactionCodes
    Dim oFTC2Tax As clsFinTransCode2Tax
    Dim oInvoices As clsInvoices
    Dim oPay As clsPayments
    Dim oPay2Inv As clsPayment2Invoice
    Dim oSched As clsPaymentsScheduled
    Dim oSched2Inv As clsPaymentSched2Invoice
    Dim oTax As clsTaxCodes
    Dim _KeyField As String
    Dim _KeyValue As String
    Dim _ProspectID As Integer
    Dim _Err As String = ""

    Public Sub New()
        oFTC = New clsFinancialTransactionCodes
        oFTC2Tax = New clsFinTransCode2Tax
        oInvoices = New clsInvoices
        oPay = New clsPayments
        oPay2Inv = New clsPayment2Invoice
        oSched = New clsPaymentsScheduled
        oSched2Inv = New clsPaymentSched2Invoice
        oTax = New clsTaxCodes
    End Sub

    Public Function Request_Refund() As Integer
        Return 0
    End Function
    Private Function Get_CCTYPEID(ByVal ccNum As String) As Integer
        Dim typeID As Integer = 0
        Dim oCombo As New clsComboItems
        Select Case CLng(Left(ccNum, 1))
            Case 1
                If CLng(Left(ccNum, 4)) = 1800 Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "JCB")
                Else
                    typeID = 0
                End If
            Case 2
                If CLng(Left(ccNum, 4)) = 2131 Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "JCB")
                Else
                    typeID = 0
                End If
            Case 3
                If CLng(Left(ccNum, 3)) > 299 And CLng(Left(ccNum, 3)) < 306 Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "Diners Club")
                ElseIf CLng(Left(ccNum, 2)) = 36 Or CLng(Left(ccNum, 2)) = 38 Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "Diners Club")
                ElseIf CLng(Left(ccNum, 2)) = 34 Or CLng(Left(ccNum, 2)) = 37 Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "AMEX")
                Else
                    typeID = oCombo.Lookup_ID("CreditCardType", "JCB")
                End If
            Case 4
                typeID = oCombo.Lookup_ID("CreditCardType", "Visa")
            Case 5
                If CLng(Left(ccNum, 2)) > 50 Or CLng(Left(ccNum, 2)) < 56 Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "MasterCard")
                Else
                    typeID = 0
                End If
            Case 6
                If CLng(Left(ccNum, 4)) = 6011 Or CLng(Left(ccNum, 2)) = 65 Or (CLng(Left(ccNum, 3)) > 643 And CLng(Left(ccNum, 3)) < 650) Or (CLng(Left(ccNum, 6)) > 622125 And CLng(Left(ccNum, 6)) < 622926) Then
                    typeID = oCombo.Lookup_ID("CreditCardType", "Discover")
                Else
                    typeID = 0
                End If
            Case Else
                typeID = 0
        End Select
        Return typeID
    End Function
    Public Function Get_CCPaymentMethod(ByVal ID As Integer) As String
        Dim pMethod As String = ""
        Dim cn As New SqlConnection(Resources.Resource.cns)
        cn.Open()
        Dim cm As New SqlCommand("Select Case when c.ComboItem is Null then '' else c.Comboitem end as CCType from t_CreditCard cc left outer join t_ComboItems c on cc.TypeID = c.ComboItemID where cc.CreditCardID = '" & ID & "'", cn)
        Dim dread As SqlDataReader

        Try
            dread = cm.ExecuteReader
            dread.Read()
            pMethod = dread("CCType")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            cn.Close()
            cn = Nothing
            cm = Nothing
            dread = Nothing
        End Try
        Return pMethod
    End Function
    Public Function Get_CreditCard(ByVal number As String, ByVal exp As String, ByVal cvv As String, ByVal address As String, ByVal city As String, ByVal state As Integer, ByVal zip As String, ByVal name As String, ByVal swipe As String, ByVal prosID As Integer, ByVal token As String, Optional ByVal type As String = "") As Long
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_CreditCard where number = '" & number & "' and Expiration = '" & exp & "' and token='" & token & "'", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim lRetVal As Long
        Dim sqlCm As New SqlCommandBuilder(da)
        Try
            da.Fill(ds, "CC")
            If ds.Tables("CC").Rows.Count > 0 Then
                If zip <> "" Then
                    dr = ds.Tables("CC").Rows(0)
                    dr("PostalCode") = zip
                    da.Update(ds, "CC")
                End If
                lRetVal = ds.Tables("CC").Rows(0).Item("CreditCardID")
            Else
                dr = ds.Tables("CC").NewRow
                dr("ProspectID") = prosID
                dr("TypeID") = (New clsComboItems).Lookup_ID("CreditCardType", type) 'Get_CCTYPEID(Trim(number))
                dr("Expiration") = exp
                dr("Number") = Trim(number)
                dr("Security") = Trim(cvv)
                dr("String") = swipe
                dr("Address") = Trim(address)
                dr("City") = city
                dr("StateID") = state 'IIf(IsNumeric(state), state, 0)
                dr("PostalCode") = IIf(zip <> "", zip, 0)
                dr("NameOnCard") = Trim(name)
                dr("ReadyToImport") = 0
                dr("ImportedID") = 0
                dr("Token") = token
                ds.Tables("CC").Rows.Add(dr)
                da.Update(ds, "CC")
                ds.Clear()
                da.Fill(ds, "CC")
                If ds.Tables("CC").Rows.Count > 0 Then
                    lRetVal = ds.Tables("CC").Rows(0).Item("CreditCardID")
                Else
                    lRetVal = 0
                End If
            End If
        Catch ex As Exception
            lRetVal = 0
        Finally
            cn = Nothing
            cm = Nothing
            da = Nothing
            ds = Nothing
            dr = Nothing
            sqlCm = Nothing
        End Try

        Return lRetVal
    End Function

    Public Function Process_Payment() As Integer
        Return 0
    End Function

    Public Function Request_Status() As String
        Return ""
    End Function

    Public Function Prospect_Financials() As SqlDataSource
        oInvoices.ProspectID = _ProspectID
        oInvoices.KeyField = _KeyField
        oInvoices.KeyValue = _KeyValue
        Return oInvoices.List_Invoices()
    End Function

    Public Function Prospect_Sub_Financials(ByVal iInvoiceID As Integer) As SqlDataSource
        oInvoices.ProspectID = _ProspectID
        oInvoices.KeyField = _KeyField
        oInvoices.KeyValue = _KeyValue
        Return oInvoices.List_Child_Invoices(iInvoiceID)
    End Function

    Public Function Reservation_Financials() As SqlDataSource
        Return Prospect_Financials()
    End Function

    Public Function Reservation_Sub_Financials(ByVal iInvoiceID As Integer) As SqlDataSource
        Return Prospect_Sub_Financials(iInvoiceID)
    End Function

    Public Function Get_Vendor_Check_Payout_Invoices(ByVal inDate As Date, ByVal outDate As Date, ByVal invoice As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select i.InvoiceID, r.ReservationID, r.CheckIndate, r.CheckOutDate, ia.Balance, p.FirstName, p.LastName, rs.Comboitem as Status from ufn_Invoice_Aging ('" & System.DateTime.Now.ToShortDateString & "','" & invoice & "','ReservationID') ia inner join t_Invoices i on ia.ID = i.InvoiceID inner join t_Reservations r on i.KeyValue = r.ReservationID inner join t_Prospect p on r.ProspectID = p.ProspectID left outer join t_ComboItems rs on r.StatusID = rs.CombOitemID where i.KeyField = 'ReservationID' and r.CheckInDate between '" & inDate & "' and '" & outDate & "' order by r.ReservationID asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Pending_Payments() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select PaymentType, Amount, CreatedDate, Exception from v_PendingPayments where " & IIf(KeyValue = 0, "ProspectID", KeyField) & " = " & IIf(KeyValue = 0, ProspectID, KeyValue)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function


    Public Function Get_Balance(ByVal KeyField As String, ByVal KeyValue As Integer, ByVal prosID As Integer) As Double
        Dim balance As Double = 0
        Try
            Dim cn As New SqlConnection(Resources.Resource.cns)
            cn.Open()
            Dim cm As New SqlCommand("", cn)
            'Dim cm As New SqlCommand("Select Case when Sum(Balance) is Null then 0 else Sum(Balance) end as Balance from v_Invoices where keyfield = '" & KeyField & "' and keyvalue = " & KeyValue, cn)
            If KeyField = "ProspectID" Then
                cm.CommandText = "Select Case when Sum(Balance) is Null then 0 else Sum(Balance) end as Balance from v_Invoices where (keyfield = 'ProspectID' or keyField = 'ContractID') and prospectid = " & prosID
            Else
                cm.CommandText = "Select Case when Sum(Balance) is Null then 0 else Sum(Balance) end as Balance from v_Invoices where keyfield = '" & KeyField & "' and keyvalue = " & KeyValue
            End If
            Dim dread As SqlDataReader
            dread = cm.ExecuteReader
            dread.Read()
            If dread.HasRows Then
                balance = dread("Balance")
            End If
            dread.Close()
            cn.Close()
            cn = Nothing
            cm = Nothing
            dread = Nothing
        Finally
        End Try
        Return balance
    End Function
    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property
    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

    Public Property KeyValue As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property ProspectID As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property
End Class
