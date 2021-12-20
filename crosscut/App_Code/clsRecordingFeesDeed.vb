Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRecordingFeesDeed
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PurchasePrice As Decimal = 0
    Dim _Deed301 As Decimal = 0
    Dim _ProcFee036 As Decimal = 0
    Dim _VSLF145 As Decimal = 0
    Dim _VOF035 As Decimal = 0
    Dim _Transfer212 As Decimal = 0
    Dim _Tech106 As Decimal = 0
    Dim _State039 As Decimal = 0
    Dim _County213 As Decimal = 0
    Dim _DoC038 As Decimal = 0
    Dim _Grantor220 As Decimal = 0
    Dim _MortgageAmount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RecordingFeesDeed where RecordingFeeID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RecordingFeesDeed where RecordingFeeID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RecordingFeesDeed")
            If ds.Tables("t_RecordingFeesDeed").Rows.Count > 0 Then
                dr = ds.Tables("t_RecordingFeesDeed").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PurchasePrice") Is System.DBNull.Value) Then _PurchasePrice = dr("PurchasePrice")
        If Not (dr("Deed301") Is System.DBNull.Value) Then _Deed301 = dr("Deed301")
        If Not (dr("ProcFee036") Is System.DBNull.Value) Then _ProcFee036 = dr("ProcFee036")
        If Not (dr("VSLF145") Is System.DBNull.Value) Then _VSLF145 = dr("VSLF145")
        If Not (dr("VOF035") Is System.DBNull.Value) Then _VOF035 = dr("VOF035")
        If Not (dr("Transfer212") Is System.DBNull.Value) Then _Transfer212 = dr("Transfer212")
        If Not (dr("Tech106") Is System.DBNull.Value) Then _Tech106 = dr("Tech106")
        If Not (dr("State039") Is System.DBNull.Value) Then _State039 = dr("State039")
        If Not (dr("County213") Is System.DBNull.Value) Then _County213 = dr("County213")
        If Not (dr("DoC038") Is System.DBNull.Value) Then _DoC038 = dr("DoC038")
        If Not (dr("Grantor220") Is System.DBNull.Value) Then _Grantor220 = dr("Grantor220")
        If Not (dr("MortgageAmount") Is System.DBNull.Value) Then _MortgageAmount = dr("MortgageAmount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RecordingFeesDeed where RecordingFeeID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RecordingFeesDeed")
            If ds.Tables("t_RecordingFeesDeed").Rows.Count > 0 Then
                dr = ds.Tables("t_RecordingFeesDeed").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RecordingFeesDeedInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PurchasePrice", SqlDbType.money, 0, "PurchasePrice")
                da.InsertCommand.Parameters.Add("@Deed301", SqlDbType.money, 0, "Deed301")
                da.InsertCommand.Parameters.Add("@ProcFee036", SqlDbType.money, 0, "ProcFee036")
                da.InsertCommand.Parameters.Add("@VSLF145", SqlDbType.money, 0, "VSLF145")
                da.InsertCommand.Parameters.Add("@VOF035", SqlDbType.money, 0, "VOF035")
                da.InsertCommand.Parameters.Add("@Transfer212", SqlDbType.money, 0, "Transfer212")
                da.InsertCommand.Parameters.Add("@Tech106", SqlDbType.money, 0, "Tech106")
                da.InsertCommand.Parameters.Add("@State039", SqlDbType.money, 0, "State039")
                da.InsertCommand.Parameters.Add("@County213", SqlDbType.money, 0, "County213")
                da.InsertCommand.Parameters.Add("@DoC038", SqlDbType.money, 0, "DoC038")
                da.InsertCommand.Parameters.Add("@Grantor220", SqlDbType.money, 0, "Grantor220")
                da.InsertCommand.Parameters.Add("@MortgageAmount", SqlDbType.money, 0, "MortgageAmount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RecordingFeeID", SqlDbType.Int, 0, "RecordingFeeID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RecordingFeesDeed").NewRow
            End If
            Update_Field("PurchasePrice", _PurchasePrice, dr)
            Update_Field("Deed301", _Deed301, dr)
            Update_Field("ProcFee036", _ProcFee036, dr)
            Update_Field("VSLF145", _VSLF145, dr)
            Update_Field("VOF035", _VOF035, dr)
            Update_Field("Transfer212", _Transfer212, dr)
            Update_Field("Tech106", _Tech106, dr)
            Update_Field("State039", _State039, dr)
            Update_Field("County213", _County213, dr)
            Update_Field("DoC038", _DoC038, dr)
            Update_Field("Grantor220", _Grantor220, dr)
            Update_Field("MortgageAmount", _MortgageAmount, dr)
            If ds.Tables("t_RecordingFeesDeed").Rows.Count < 1 Then ds.Tables("t_RecordingFeesDeed").Rows.Add(dr)
            da.Update(ds, "t_RecordingFeesDeed")
            _ID = ds.Tables("t_RecordingFeesDeed").Rows(0).Item("RecordingFeeID")
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
            oEvents.KeyField = "RecordingFeeID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Get_Fees(ByVal fName As String, ByVal SP As Double) As Double
        Dim fees As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select top 1 " & fName & " as Fee from t_RecordingFeesDeed where mortgageamount >= '" & SP & "' order by mortgageamount"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                fees = dread("Fee")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return fees
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PurchasePrice() As Decimal
        Get
            Return _PurchasePrice
        End Get
        Set(ByVal value As Decimal)
            _PurchasePrice = value
        End Set
    End Property

    Public Property Deed301() As Decimal
        Get
            Return _Deed301
        End Get
        Set(ByVal value As Decimal)
            _Deed301 = value
        End Set
    End Property

    Public Property ProcFee036() As Decimal
        Get
            Return _ProcFee036
        End Get
        Set(ByVal value As Decimal)
            _ProcFee036 = value
        End Set
    End Property

    Public Property VSLF145() As Decimal
        Get
            Return _VSLF145
        End Get
        Set(ByVal value As Decimal)
            _VSLF145 = value
        End Set
    End Property

    Public Property VOF035() As Decimal
        Get
            Return _VOF035
        End Get
        Set(ByVal value As Decimal)
            _VOF035 = value
        End Set
    End Property

    Public Property Transfer212() As Decimal
        Get
            Return _Transfer212
        End Get
        Set(ByVal value As Decimal)
            _Transfer212 = value
        End Set
    End Property

    Public Property Tech106() As Decimal
        Get
            Return _Tech106
        End Get
        Set(ByVal value As Decimal)
            _Tech106 = value
        End Set
    End Property

    Public Property State039() As Decimal
        Get
            Return _State039
        End Get
        Set(ByVal value As Decimal)
            _State039 = value
        End Set
    End Property

    Public Property County213() As Decimal
        Get
            Return _County213
        End Get
        Set(ByVal value As Decimal)
            _County213 = value
        End Set
    End Property

    Public Property DoC038() As Decimal
        Get
            Return _DoC038
        End Get
        Set(ByVal value As Decimal)
            _DoC038 = value
        End Set
    End Property

    Public Property Grantor220() As Decimal
        Get
            Return _Grantor220
        End Get
        Set(ByVal value As Decimal)
            _Grantor220 = value
        End Set
    End Property

    Public Property MortgageAmount() As Decimal
        Get
            Return _MortgageAmount
        End Get
        Set(ByVal value As Decimal)
            _MortgageAmount = value
        End Set
    End Property

    Public Property RecordingFeeID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
