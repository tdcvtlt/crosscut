Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageTourPayment
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PackageTourFinTransID As Integer = 0
    Dim _PaymentMethodID As Integer = 0
    Dim _Adjustment As Boolean = False
    Dim _PosNeg As Boolean = False
    Dim _FixedAmount As Boolean = False
    Dim _Amount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageTourPayment where PackageTourPaymentID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageTourPayment where PackageTourPaymentID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageTourPayment")
            If ds.Tables("t_PackageTourPayment").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTourPayment").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("PackageTourFinTransID") Is System.DBNull.Value) Then _PackageTourFinTransID = dr("PackageTourFinTransID")
        If Not (dr("PaymentMethodID") Is System.DBNull.Value) Then _PaymentMethodID = dr("PaymentMethodID")
        If Not (dr("Adjustment") Is System.DBNull.Value) Then _Adjustment = dr("Adjustment")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
        If Not (dr("FixedAmount") Is System.DBNull.Value) Then _FixedAmount = dr("FixedAmount")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageTourPayment where PackageTourPaymentID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageTourPayment")
            If ds.Tables("t_PackageTourPayment").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTourPayment").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageTourPaymentInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PackageTourFinTransID", SqlDbType.int, 0, "PackageTourFinTransID")
                da.InsertCommand.Parameters.Add("@PaymentMethodID", SqlDbType.int, 0, "PaymentMethodID")
                da.InsertCommand.Parameters.Add("@Adjustment", SqlDbType.bit, 0, "Adjustment")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.bit, 0, "PosNeg")
                da.InsertCommand.Parameters.Add("@FixedAmount", SqlDbType.bit, 0, "FixedAmount")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageTourPaymentID", SqlDbType.Int, 0, "PackageTourPaymentID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageTourPayment").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PackageTourFinTransID", _PackageTourFinTransID, dr)
            Update_Field("PaymentMethodID", _PaymentMethodID, dr)
            Update_Field("Adjustment", _Adjustment, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            Update_Field("FixedAmount", _FixedAmount, dr)
            Update_Field("Amount", _Amount, dr)
            If ds.Tables("t_PackageTourPayment").Rows.Count < 1 Then ds.Tables("t_PackageTourPayment").Rows.Add(dr)
            da.Update(ds, "t_PackageTourPayment")
            _ID = ds.Tables("t_PackageTourPayment").Rows(0).Item("PackageTourPaymentID")
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
            oEvents.KeyField = "PackageTourPaymentID"
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

    Public Function List_Payments(ByVal pkgFinTransID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select p.PackageTourPaymentID as ID, pm.ComboItem as Method, p.Amount, p.Adjustment, p.PosNeg, p.FixedAmount from t_packagetourpayment p inner join t_Comboitems pm on p.PaymentMethodID = pm.ComboItemID where p.PackageTourFinTransID = " & pkgFinTransID
        Return ds
    End Function

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property PackageTourFinTransID() As Integer
        Get
            Return _PackageTourFinTransID
        End Get
        Set(ByVal value As Integer)
            _PackageTourFinTransID = value
        End Set
    End Property

    Public Property PaymentMethodID() As Integer
        Get
            Return _PaymentMethodID
        End Get
        Set(ByVal value As Integer)
            _PaymentMethodID = value
        End Set
    End Property

    Public Property Adjustment() As Boolean
        Get
            Return _Adjustment
        End Get
        Set(ByVal value As Boolean)
            _Adjustment = value
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

    Public Property FixedAmount() As Boolean
        Get
            Return _FixedAmount
        End Get
        Set(ByVal value As Boolean)
            _FixedAmount = value
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

    Public Property PackageTourPaymentID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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
    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

End Class
