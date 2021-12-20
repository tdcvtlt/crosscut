Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCCTransApplyTo
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CCTransID As Integer = 0
    Dim _PaymentID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CCTransApplyTo where CCTransApplyToID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CCTransApplyTo where CCTransApplyToID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CCTransApplyTo")
            If ds.Tables("t_CCTransApplyTo").Rows.Count > 0 Then
                dr = ds.Tables("t_CCTransApplyTo").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("CCTransID") Is System.DBNull.Value) Then _CCTransID = dr("CCTransID")
        If Not (dr("PaymentID") Is System.DBNull.Value) Then _PaymentID = dr("PaymentID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CCTransApplyTo where CCTransApplyToID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CCTransApplyTo")
            If ds.Tables("t_CCTransApplyTo").Rows.Count > 0 Then
                dr = ds.Tables("t_CCTransApplyTo").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CCTransApplyToInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CCTransID", SqlDbType.int, 0, "CCTransID")
                da.InsertCommand.Parameters.Add("@PaymentID", SqlDbType.int, 0, "PaymentID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CCTransApplyToID", SqlDbType.Int, 0, "CCTransApplyToID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CCTransApplyTo").NewRow
            End If
            Update_Field("CCTransID", _CCTransID, dr)
            Update_Field("PaymentID", _PaymentID, dr)
            Update_Field("Amount", _Amount, dr)
            If ds.Tables("t_CCTransApplyTo").Rows.Count < 1 Then ds.Tables("t_CCTransApplyTo").Rows.Add(dr)
            da.Update(ds, "t_CCTransApplyTo")
            _ID = ds.Tables("t_CCTransApplyTo").Rows(0).Item("CCTransApplyToID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "CCTransApplyToID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Get_Applied_To(ByVal cctransID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ca.PaymentID, i.InvoiceID, tc.ComboItem as Invoice, ca.Amount, pm.Comboitem as PaymentMethod from t_CCTransApplyto ca inner join t_payments p on ca.paymentID = p.PaymentID left outer join t_comboitems pm on p.MethodID = pm.ComboitemID inner join t_payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FintransCodes f on i.FintransID = f.Fintransid left outer join t_comboItems tc on f.TransCodeID = tc.ComboItemID where ca.CCTransID = '" & cctransID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Create_Refund_Items(ByVal ccTransID As Integer, ByVal refundID As Integer) As Boolean
        Dim bProcessed As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            '**** Loop Through Refund Request Apply To Items *****'
            '**** Create Payments for Each Item with posNeg Flag adjusted *****'
            '**** Create CCTransApply To Items for each item
            Dim oPayment As New clsPayments
            Dim oPaymentB As New clsPayments
            Dim oPmt2Invoice As New clsPayment2Invoice
            Dim oCombo As New clsComboItems
            Dim oCCTransApply As New clsCCTransApplyTo
            Dim pmtID As Integer = 0
            cm.CommandText = "Select * from t_RefundRequestApplyTo where RefundRequestID = '" & refundID & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                oPayment.PaymentID = dread("PaymentID")
                oPayment.Load()
                oPaymentB.PaymentID = 0
                oPaymentB.Load()
                oPaymentB.MethodID = oCombo.Lookup_ID("PaymentMethod", oCombo.Lookup_ComboItem(oPayment.MethodID) & " - Refund")
                oPaymentB.Amount = dread("Amount")
                oPaymentB.PosNeg = False
                oPaymentB.TransDate = System.DateTime.Now
                oPaymentB.UserID = _UserID
                oPaymentB.Save()
                pmtID = oPaymentB.PaymentID
                oPmt2Invoice.Inv2PayID = 0
                oPmt2Invoice.Load()
                oPmt2Invoice.PaymentID = pmtID
                oPmt2Invoice.Amount = dread("Amount")
                oPmt2Invoice.InvoiceID = oPmt2Invoice.Get_Invoice_ID(dread("PaymentID"))
                oPmt2Invoice.Save()
                oCCTransApply.CCTransApplyToID = 0
                oCCTransApply.CCTransID = ccTransID
                oCCTransApply.PaymentID = pmtID
                oCCTransApply.Amount = dread("Amount")
                oCCTransApply.Save()
            Loop
            dread.Close()
            oPayment = Nothing
            oPaymentB = Nothing
            oCombo = Nothing
            oPmt2Invoice = Nothing
            oCCTransApply = Nothing
        Catch ex As Exception
            _Err = ex.Message
            bProcessed = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bProcessed
    End Function

    Public Function Create_Payment_Items(ByVal cctransID As Integer, ByVal ccID As Integer) As Boolean
        Dim bProcessed As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            '**** Loop Through CCTrans Apply To Items *****'
            '**** Create Payments for Each Item *****'
            '**** Update CCTransApply Record replacing the invoiceid with the created paymentid
            Dim oPayment As New clsPayments
            Dim oPmt2Invoice As New clsPayment2Invoice
            Dim oCombo As New clsComboItems
            Dim oCCTransApply As New clsCCTransApplyTo
            Dim pmtID As Integer = 0
            Dim oFinancials As New clsFinancials
            '***** Get Credit Card Type
            cm.CommandText = "Select * from t_CCTransApplyTo where CCTransID = '" & cctransID & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                oPayment.PaymentID = 0
                oPayment.Load()
                oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", oFinancials.Get_CCPaymentMethod(ccID))
                oPayment.Amount = dread("Amount")
                oPayment.PosNeg = True
                oPayment.TransDate = System.DateTime.Now
                oPayment.UserID = _UserID
                oPayment.Save()
                pmtID = oPayment.PaymentID
                oPmt2Invoice.Inv2PayID = 0
                oPmt2Invoice.Load()
                oPmt2Invoice.PaymentID = pmtID
                oPmt2Invoice.Amount = dread("Amount")
                oPmt2Invoice.InvoiceID = dread("PaymentID")
                oPmt2Invoice.PosNeg = True
                oPmt2Invoice.Save()
                oCCTransApply.CCTransApplyToID = dread("CCTransApplyToID")
                oCCTransApply.UserID = _UserID
                oCCTransApply.PaymentID = pmtID
                oCCTransApply.CCTransID = dread("CCTransID")
                oCCTransApply.Amount = dread("Amount")
                oCCTransApply.Save()
            Loop
            dread.Close()
            oPayment = Nothing
            oCombo = Nothing
            oFinancials = Nothing
            oPmt2Invoice = Nothing
            oCCTransApply = Nothing
        Catch ex As Exception
            _Err = ex.Message
            bProcessed = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bProcessed
    End Function

    Public Function List_ApplyTo_Items(ByVal CCTransID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select tc.ComboItem as Invoice, ra.Amount, i.transdate As Date from t_CCTransApplyTo ra inner join t_Invoices i on ra.PaymentID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID where ra.CCTransID = " & CCTransID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Payments(ByVal ID As Integer) As String
        Dim IDs As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select PaymentID from t_CCTransApplyTo where CCTransID = " & ID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If IDs = "" Then
                        IDs = dread("PaymentID")
                    Else
                        IDs = IDs & "," & dread("PaymentID")
                    End If
                Loop
            Else
                IDs = "IDs-" & ID
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return IDs
    End Function
    Public Function Is_CCPayment(ByVal paymentID As Integer) As Boolean
        Dim ccPayment As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CCTransApplyTo ca inner join t_CCTrans cc on ca.CCTransID = cc.CCTransID where ca.paymentid = " & paymentID & " and cc.Imported = 1"
            dread = cm.ExecuteReader
            ccPayment = dread.HasRows
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ccPayment
    End Function
    Public Property CCTransID() As Integer
        Get
            Return _CCTransID
        End Get
        Set(ByVal value As Integer)
            _CCTransID = value
        End Set
    End Property

    Public Property PaymentID() As Integer
        Get
            Return _PaymentID
        End Get
        Set(ByVal value As Integer)
            _PaymentID = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property
    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property CCTransApplyToID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
