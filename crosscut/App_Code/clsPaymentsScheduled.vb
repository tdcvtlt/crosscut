Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPaymentsScheduled
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _MethodID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _PosNeg As Boolean = False
    Dim _ApplyToID As Integer = 0
    Dim _TransDate As String = ""
    Dim _SchedDate As String = ""
    Dim _CreatedDate As String = ""
    Dim _ProcessedDate As String = ""
    Dim _Successful As Boolean = False
    Dim _Processed As Boolean = False
    Dim _Cancelled As Boolean = False
    Dim _CreditCardID As Integer = 0
    Dim _Reference As String = ""
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PaymentsScheduled where SchedPayID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PaymentsScheduled where SchedPayID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PaymentsScheduled")
            If ds.Tables("t_PaymentsScheduled").Rows.Count > 0 Then
                dr = ds.Tables("t_PaymentsScheduled").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("MethodID") Is System.DBNull.Value) Then _MethodID = dr("MethodID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
        If Not (dr("ApplyToID") Is System.DBNull.Value) Then _ApplyToID = dr("ApplyToID")
        If Not (dr("UserID") Is System.DBNull.Value) Then _UserID = dr("UserID")
        If Not (dr("TransDate") Is System.DBNull.Value) Then _TransDate = dr("TransDate")
        If Not (dr("SchedDate") Is System.DBNull.Value) Then _SchedDate = dr("SchedDate")
        If Not (dr("CreatedDate") Is System.DBNull.Value) Then _CreatedDate = dr("CreatedDate")
        If Not (dr("ProcessedDate") Is System.DBNull.Value) Then _ProcessedDate = dr("ProcessedDate")
        If Not (dr("Successful") Is System.DBNull.Value) Then _Successful = dr("Successful")
        If Not (dr("Processed") Is System.DBNull.Value) Then _Processed = dr("Processed")
        If Not (dr("Cancelled") Is System.DBNull.Value) Then _Cancelled = dr("Cancelled")
        If Not (dr("Reference") Is System.DBNull.Value) Then _Reference = dr("Reference")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("CreditCardID") Is System.DBNull.Value) Then _CreditCardID = dr("CreditCardID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PaymentsScheduled where SchedPayID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PaymentsScheduled")
            If ds.Tables("t_PaymentsScheduled").Rows.Count > 0 Then
                dr = ds.Tables("t_PaymentsScheduled").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PaymentsScheduledInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@MethodID", SqlDbType.int, 0, "MethodID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.bit, 0, "PosNeg")
                da.InsertCommand.Parameters.Add("@ApplyToID", SqlDbType.int, 0, "ApplyToID")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.int, 0, "UserID")
                da.InsertCommand.Parameters.Add("@TransDate", SqlDbType.datetime, 0, "TransDate")
                da.InsertCommand.Parameters.Add("@SchedDate", SqlDbType.datetime, 0, "SchedDate")
                da.InsertCommand.Parameters.Add("@CreatedDate", SqlDbType.datetime, 0, "CreatedDate")
                da.InsertCommand.Parameters.Add("@ProcessedDate", SqlDbType.datetime, 0, "ProcessedDate")
                da.InsertCommand.Parameters.Add("@Successful", SqlDbType.bit, 0, "Successful")
                da.InsertCommand.Parameters.Add("@Processed", SqlDbType.Bit, 0, "Processed")
                da.InsertCommand.Parameters.Add("@Reference", SqlDbType.VarChar, 0, "Reference")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@Cancelled", SqlDbType.Bit, 0, "Cancelled")
                da.InsertCommand.Parameters.Add("@CreditCardID", SqlDbType.Int, 0, "CreditCardID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SchedPayID", SqlDbType.Int, 0, "SchedPayID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PaymentsScheduled").NewRow
            End If
            Update_Field("MethodID", _MethodID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            Update_Field("ApplyToID", _ApplyToID, dr)
            Update_Field("UserID", _UserID, dr)
            Update_Field("TransDate", _TransDate, dr)
            Update_Field("SchedDate", _SchedDate, dr)
            Update_Field("CreatedDate", _CreatedDate, dr)
            Update_Field("ProcessedDate", _ProcessedDate, dr)
            Update_Field("Successful", _Successful, dr)
            Update_Field("Processed", _Processed, dr)
            Update_Field("Cancelled", _Cancelled, dr)
            Update_Field("CreditCardID", _CreditCardID, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Reference", _Reference, dr)
            If ds.Tables("t_PaymentsScheduled").Rows.Count < 1 Then ds.Tables("t_PaymentsScheduled").Rows.Add(dr)
            da.Update(ds, "t_PaymentsScheduled")
            _ID = ds.Tables("t_PaymentsScheduled").Rows(0).Item("SchedPayID")
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
            oEvents.KeyField = "SchedPayID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Scheduled_Payments(ByVal KeyField As String, ByVal KeyValue As Integer) As SQLDataSource
        Dim ds As New sqldataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand() = "SELECT p.SchedPayID AS ID, pm.ComboItem AS Payment, p.Amount, p.SchedDate, tc.ComboItem AS Invoice FROM t_PaymentsScheduled p INNER JOIN t_PaymentSched2Invoice sp ON p.SchedPayID = sp.SchedPaymentID INNER JOIN t_Invoices i ON sp.InvoiceID = i.InvoiceID INNER JOIN t_ComboItems pm ON p.MethodID = pm.ComboItemID INNER JOIN t_FinTransCodes f ON i.FinTransID = f.FinTransID INNER JOIN t_ComboItems tc ON f.TransCodeID = tc.ComboItemID where i.KeyField = '" & KeyField & "' and i.Keyvalue = '" & KeyValue & "' and p.Cancelled = '0' and p.Processed = '0' order by p.SchedDate asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property MethodID() As Integer
        Get
            Return _MethodID
        End Get
        Set(ByVal value As Integer)
            _MethodID = value
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

    Public Property ApplyToID() As Integer
        Get
            Return _ApplyToID
        End Get
        Set(ByVal value As Integer)
            _ApplyToID = value
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

    Public Property TransDate() As String
        Get
            Return _TransDate
        End Get
        Set(ByVal value As String)
            _TransDate = value
        End Set
    End Property

    Public Property SchedDate() As String
        Get
            Return _SchedDate
        End Get
        Set(ByVal value As String)
            _SchedDate = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return _CreatedDate
        End Get
        Set(ByVal value As String)
            _CreatedDate = value
        End Set
    End Property

    Public Property ProcessedDate() As String
        Get
            Return _ProcessedDate
        End Get
        Set(ByVal value As String)
            _ProcessedDate = value
        End Set
    End Property

    Public Property Successful() As Boolean
        Get
            Return _Successful
        End Get
        Set(ByVal value As Boolean)
            _Successful = value
        End Set
    End Property

    Public Property Processed() As Boolean
        Get
            Return _Processed
        End Get
        Set(ByVal value As Boolean)
            _Processed = value
        End Set
    End Property

    Public Property Cancelled() As Boolean
        Get
            Return _Cancelled
        End Get
        Set(ByVal value As Boolean)
            _Cancelled = value
        End Set
    End Property

    Public Property SchedPayID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property CreditCardID() As Integer
        Get
            Return _CreditCardID
        End Get
        Set(ByVal value As Integer)
            _CreditCardID = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _Description
        End Get
        Set(value As String)
            _Description = value
        End Set
    End Property

    Public Property Reference As String
        Get
            Return _Reference
        End Get
        Set(value As String)
            _Reference = value
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
End Class
