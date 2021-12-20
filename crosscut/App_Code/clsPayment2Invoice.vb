Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPayment2Invoice
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _InvoiceID As Integer = 0
    Dim _PaymentID As Integer = 0
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
        cm = New SqlCommand("Select * from t_Payment2Invoice where Inv2PayID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Payment2Invoice where Inv2PayID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Payment2Invoice")
            If ds.Tables("t_Payment2Invoice").Rows.Count > 0 Then
                dr = ds.Tables("t_Payment2Invoice").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("InvoiceID") Is System.DBNull.Value) Then _InvoiceID = dr("InvoiceID")
        If Not (dr("PaymentID") Is System.DBNull.Value) Then _PaymentID = dr("PaymentID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Payment2Invoice where Inv2PayID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Payment2Invoice")
            If ds.Tables("t_Payment2Invoice").Rows.Count > 0 Then
                dr = ds.Tables("t_Payment2Invoice").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Payment2InvoiceInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@InvoiceID", SqlDbType.int, 0, "InvoiceID")
                da.InsertCommand.Parameters.Add("@PaymentID", SqlDbType.int, 0, "PaymentID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.bit, 0, "PosNeg")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Inv2PayID", SqlDbType.Int, 0, "Inv2PayID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Payment2Invoice").NewRow
            End If
            Update_Field("InvoiceID", _InvoiceID, dr)
            Update_Field("PaymentID", _PaymentID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            If ds.Tables("t_Payment2Invoice").Rows.Count < 1 Then ds.Tables("t_Payment2Invoice").Rows.Add(dr)
            da.Update(ds, "t_Payment2Invoice")
            _ID = ds.Tables("t_Payment2Invoice").Rows(0).Item("Inv2PayID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
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
            oEvents.KeyField = "Inv2PayID"
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

    Public Function Get_Invoice_ID(ByVal paymentID As Integer) As Integer
        Dim invID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select InvoiceID from t_Payment2Invoice where paymentid = " & paymentID & ""
            dread = cm.ExecuteReader
            dread.Read()
            invID = dread("InvoiceID")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return invID
    End Function

    Public Function Update_Pmt_Amt(ByVal paymentID As Integer, ByVal amt As Double) As Boolean
        Dim bUpdated As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_Payment2Invoice set amount = " & amt & " where paymentID = " & paymentID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bUpdated
    End Function
    Public Property InvoiceID() As Integer
        Get
            Return _InvoiceID
        End Get
        Set(ByVal value As Integer)
            _InvoiceID = value
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

    Public Property PosNeg() As Boolean
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Boolean)
            _PosNeg = value
        End Set
    End Property

    Public Property Inv2PayID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
