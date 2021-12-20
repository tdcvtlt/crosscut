Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPaymentSched2Invoice
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _InvoiceID As Integer = 0
    Dim _SchedPaymentID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _PosNeg As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PaymentSched2Invoice where SchedPay2InvoiceID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PaymentSched2Invoice where SchedPay2InvoiceID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PaymentSched2Invoice")
            If ds.Tables("t_PaymentSched2Invoice").Rows.Count > 0 Then
                dr = ds.Tables("t_PaymentSched2Invoice").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("InvoiceID") Is System.DBNull.Value) Then _InvoiceID = dr("InvoiceID")
        If Not (dr("SchedPaymentID") Is System.DBNull.Value) Then _SchedPaymentID = dr("SchedPaymentID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PaymentSched2Invoice where SchedPay2InvoiceID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PaymentSched2Invoice")
            If ds.Tables("t_PaymentSched2Invoice").Rows.Count > 0 Then
                dr = ds.Tables("t_PaymentSched2Invoice").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PaymentSched2InvoiceInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@InvoiceID", SqlDbType.int, 0, "InvoiceID")
                da.InsertCommand.Parameters.Add("@SchedPaymentID", SqlDbType.int, 0, "SchedPaymentID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.bit, 0, "PosNeg")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SchedPay2InvoiceID", SqlDbType.Int, 0, "SchedPay2InvoiceID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PaymentSched2Invoice").NewRow
            End If
            Update_Field("InvoiceID", _InvoiceID, dr)
            Update_Field("SchedPaymentID", _SchedPaymentID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            If ds.Tables("t_PaymentSched2Invoice").Rows.Count < 1 Then ds.Tables("t_PaymentSched2Invoice").Rows.Add(dr)
            da.Update(ds, "t_PaymentSched2Invoice")
            _ID = ds.Tables("t_PaymentSched2Invoice").Rows(0).Item("SchedPay2InvoiceID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "SchedPay2InvoiceID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Get_Invoice_ID(ByVal paymentID As Integer) As Integer
        Dim invoiceID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select InvoiceID from t_PaymentSched2Invoice where SchedPaymentID = " & paymentID
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                dread.Read()
                invoiceID = dread("InvoiceID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return invoiceID
    End Function

    Public Function Update_Pmt_Amt(ByVal paymentID As Integer, ByVal amt As Double) As Boolean
        Dim bUpdated As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_PaymentSched2Invoice set amount = " & amt & " where SchedpaymentID = " & paymentID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bUpdated
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property InvoiceID() As Integer
        Get
            Return _InvoiceID
        End Get
        Set(ByVal value As Integer)
            _InvoiceID = value
        End Set
    End Property

    Public Property SchedPaymentID() As Integer
        Get
            Return _SchedPaymentID
        End Get
        Set(ByVal value As Integer)
            _SchedPaymentID = value
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

    Public Property PosNeg() As Boolean
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Boolean)
            _PosNeg = value
        End Set
    End Property

    Public ReadOnly Property SchedPay2InvoiceID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
