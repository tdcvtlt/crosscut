Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAFCImports
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TransID As Integer = 0
    Dim _ApplyID As Integer = 0
    Dim _BillCodeName As String = ""
    Dim _TransType As String = ""
    Dim _TransDate As String = ""
    Dim _Amount As Decimal = 0
    Dim _PaymentAmount As Decimal? = Nothing
    Dim _TRXNO As String = ""
    Dim _ResortID As String = ""
    Dim _OwnerID As Integer = 0
    Dim _AccountNo As String = ""
    Dim _CreateDate As String = ""
    Dim _RefPaymentCodeName As String = ""
    Dim _IsAWriteOffPayment As Boolean = False
    Dim _BeginningDayAccountBalance As Decimal = 0
    Dim _EndDayAccountBalance As Decimal = 0
    Dim _Processed As String = ""
    Dim _ErrorDesc As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_AFCImports where IntegrationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_AFCImports where IntegrationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_AFCImports")
            If ds.Tables("t_AFCImports").Rows.Count > 0 Then
                dr = ds.Tables("t_AFCImports").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("TransID") Is System.DBNull.Value) Then _TransID = dr("TransID")
        If Not (dr("ApplyID") Is System.DBNull.Value) Then _ApplyID = dr("ApplyID")
        If Not (dr("BillCodeName") Is System.DBNull.Value) Then _BillCodeName = dr("BillCodeName")
        If Not (dr("TransType") Is System.DBNull.Value) Then _TransType = dr("TransType")
        If Not (dr("TransDate") Is System.DBNull.Value) Then _TransDate = dr("TransDate")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("PaymentAmount") Is System.DBNull.Value) Then _PaymentAmount = dr("PaymentAmount")
        If Not (dr("TRXNO") Is System.DBNull.Value) Then _TRXNO = dr("TRXNO")
        If Not (dr("ResortID") Is System.DBNull.Value) Then _ResortID = dr("ResortID")
        If Not (dr("OwnerID") Is System.DBNull.Value) Then _OwnerID = dr("OwnerID")
        If Not (dr("AccountNo") Is System.DBNull.Value) Then _AccountNo = dr("AccountNo")
        If Not (dr("CreateDate") Is System.DBNull.Value) Then _CreateDate = dr("CreateDate")
        If Not (dr("RefPaymentCodeName") Is System.DBNull.Value) Then _RefPaymentCodeName = dr("RefPaymentCodeName")
        If Not (dr("IsAWriteOffPayment") Is System.DBNull.Value) Then _IsAWriteOffPayment = dr("IsAWriteOffPayment")
        If Not (dr("BeginningDayAccountBalance") Is System.DBNull.Value) Then _BeginningDayAccountBalance = dr("BeginningDayAccountBalance")
        If Not (dr("EndDayAccountBalance") Is System.DBNull.Value) Then _EndDayAccountBalance = dr("EndDayAccountBalance")
        If Not (dr("Processed") Is System.DBNull.Value) Then _Processed = dr("Processed")
        If Not (dr("ErrorDesc") Is System.DBNull.Value) Then _ErrorDesc = dr("ErrorDesc")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_AFCImports where IntegrationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_AFCImports")
            If ds.Tables("t_AFCImports").Rows.Count > 0 Then
                dr = ds.Tables("t_AFCImports").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_AFCImportsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TransID", SqlDbType.int, 0, "TransID")
                da.InsertCommand.Parameters.Add("@ApplyID", SqlDbType.int, 0, "ApplyID")
                da.InsertCommand.Parameters.Add("@BillCodeName", SqlDbType.varchar, 0, "BillCodeName")
                da.InsertCommand.Parameters.Add("@TransType", SqlDbType.varchar, 0, "TransType")
                da.InsertCommand.Parameters.Add("@TransDate", SqlDbType.datetime, 0, "TransDate")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@PaymentAmount", SqlDbType.money, 0, "PaymentAmount")
                da.InsertCommand.Parameters.Add("@TRXNO", SqlDbType.varchar, 0, "TRXNO")
                da.InsertCommand.Parameters.Add("@ResortID", SqlDbType.VarChar, 0, "ResortID")
                da.InsertCommand.Parameters.Add("@OwnerID", SqlDbType.int, 0, "OwnerID")
                da.InsertCommand.Parameters.Add("@AccountNo", SqlDbType.varchar, 0, "AccountNo")
                da.InsertCommand.Parameters.Add("@CreateDate", SqlDbType.datetime, 0, "CreateDate")
                da.InsertCommand.Parameters.Add("@RefPaymentCodeName", SqlDbType.varchar, 0, "RefPaymentCodeName")
                da.InsertCommand.Parameters.Add("@IsAWriteOffPayment", SqlDbType.bit, 0, "IsAWriteOffPayment")
                da.InsertCommand.Parameters.Add("@BeginningDayAccountBalance", SqlDbType.Money, 0, "BeginningDayAccountBalance")
                da.InsertCommand.Parameters.Add("@EndDayAccountBalance", SqlDbType.money, 0, "EndDayAccountBalance")
                da.InsertCommand.Parameters.Add("@Processed", SqlDbType.varchar, 0, "Processed")
                da.InsertCommand.Parameters.Add("@ErrorDesc", SqlDbType.varchar, 0, "ErrorDesc")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@IntegrationID", SqlDbType.Int, 0, "IntegrationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_AFCImports").NewRow
            End If
            Update_Field("TransID", _TransID, dr)
            Update_Field("ApplyID", _ApplyID, dr)
            Update_Field("BillCodeName", _BillCodeName, dr)
            Update_Field("TransType", _TransType, dr)
            Update_Field("TransDate", _TransDate, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("PaymentAmount", _PaymentAmount, dr)
            Update_Field("TRXNO", _TRXNO, dr)
            Update_Field("ResortID", _ResortID, dr)
            Update_Field("OwnerID", _OwnerID, dr)
            Update_Field("AccountNo", _AccountNo, dr)
            Update_Field("CreateDate", _CreateDate, dr)
            Update_Field("RefPaymentCodeName", _RefPaymentCodeName, dr)
            Update_Field("IsAWriteOffPayment", _IsAWriteOffPayment, dr)
            Update_Field("BeginningDayAccountBalance", _BeginningDayAccountBalance, dr)
            Update_Field("EndDayAccountBalance", _EndDayAccountBalance, dr)
            Update_Field("Processed", _Processed, dr)
            Update_Field("ErrorDesc", _ErrorDesc, dr)
            If ds.Tables("t_AFCImports").Rows.Count < 1 Then ds.Tables("t_AFCImports").Rows.Add(dr)
            da.Update(ds, "t_AFCImports")
            _ID = ds.Tables("t_AFCImports").Rows(0).Item("IntegrationID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
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
            oEvents.KeyField = "IntegrationID"
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

    Public Property TransID() As Integer
        Get
            Return _TransID
        End Get
        Set(ByVal value As Integer)
            _TransID = value
        End Set
    End Property

    Public Property ApplyID() As Integer
        Get
            Return _ApplyID
        End Get
        Set(ByVal value As Integer)
            _ApplyID = value
        End Set
    End Property

    Public Property BillCodeName() As String
        Get
            Return _BillCodeName
        End Get
        Set(ByVal value As String)
            _BillCodeName = value
        End Set
    End Property

    Public Property TransType() As String
        Get
            Return _TransType
        End Get
        Set(ByVal value As String)
            _TransType = value
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

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Public Property PaymentAmount() As Decimal?
        Get
            Return _PaymentAmount
        End Get
        Set(ByVal value As Decimal?)
            _PaymentAmount = value
        End Set
    End Property

    Public Property TRXNO() As String
        Get
            Return _TRXNO
        End Get
        Set(ByVal value As String)
            _TRXNO = value
        End Set
    End Property

    Public Property ResortID() As String
        Get
            Return _ResortID
        End Get
        Set(ByVal value As String)
            _ResortID = value
        End Set
    End Property

    Public Property OwnerID() As Integer
        Get
            Return _OwnerID
        End Get
        Set(ByVal value As Integer)
            _OwnerID = value
        End Set
    End Property

    Public Property AccountNo() As String
        Get
            Return _AccountNo
        End Get
        Set(ByVal value As String)
            _AccountNo = value
        End Set
    End Property

    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal value As String)
            _CreateDate = value
        End Set
    End Property

    Public Property RefPaymentCodeName() As String
        Get
            Return _RefPaymentCodeName
        End Get
        Set(ByVal value As String)
            _RefPaymentCodeName = value
        End Set
    End Property

    Public Property IsAWriteOffPayment() As Boolean
        Get
            Return _IsAWriteOffPayment
        End Get
        Set(ByVal value As Boolean)
            _IsAWriteOffPayment = value
        End Set
    End Property

    Public Property BeginningDayAccountBalance() As Decimal
        Get
            Return _BeginningDayAccountBalance
        End Get
        Set(ByVal value As Decimal)
            _BeginningDayAccountBalance = value
        End Set
    End Property

    Public Property EndDayAccountBalance() As Decimal
        Get
            Return _EndDayAccountBalance
        End Get
        Set(ByVal value As Decimal)
            _EndDayAccountBalance = value
        End Set
    End Property

    Public Property Processed() As String
        Get
            Return _Processed
        End Get
        Set(ByVal value As String)
            _Processed = value
        End Set
    End Property

    Public Property ErrorDesc() As String
        Get
            Return _ErrorDesc
        End Get
        Set(ByVal value As String)
            _ErrorDesc = value
        End Set
    End Property

    Public ReadOnly Property IntegrationID() As Integer
        Get
            Return _ID
        End Get
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
