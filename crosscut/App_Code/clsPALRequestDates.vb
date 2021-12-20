Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPALRequestDates
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PALRequestID As Integer = 0
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _PALHours As Decimal = 0
    Dim _SSLBHours As Decimal = 0
    Dim _UnpaidHours As Decimal = 0
    Dim _Processed As Boolean = False
    Dim _Reason As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PALRequestDates where PALRequestDateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PALRequestDates where PALRequestDateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PALRequestDates")
            If ds.Tables("t_PALRequestDates").Rows.Count > 0 Then
                dr = ds.Tables("t_PALRequestDates").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PALRequestID") Is System.DBNull.Value) Then _PALRequestID = dr("PALRequestID")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("PALHours") Is System.DBNull.Value) Then _PALHours = dr("PALHours")
        If Not (dr("SSLBHours") Is System.DBNull.Value) Then _SSLBHours = dr("SSLBHours")
        If Not (dr("UnpaidHours") Is System.DBNull.Value) Then _UnpaidHours = dr("UnpaidHours")
        If Not (dr("Processed") Is System.DBNull.Value) Then _Processed = dr("Processed")
        If Not (dr("Reason") Is System.DBNull.Value) Then _Processed = dr("Reason")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PALRequestDates where PALRequestDateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PALRequestDates")
            If ds.Tables("t_PALRequestDates").Rows.Count > 0 Then
                dr = ds.Tables("t_PALRequestDates").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PALRequestDatesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PALRequestID", SqlDbType.int, 0, "PALRequestID")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.datetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.datetime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@PALHours", SqlDbType.Money, 0, "PALHours")
                da.InsertCommand.Parameters.Add("@SSLBHours", SqlDbType.Money, 0, "SSLBHours")
                da.InsertCommand.Parameters.Add("@UnpaidHours", SqlDbType.Money, 0, "UnpaidHours")
                da.InsertCommand.Parameters.Add("@Processed", SqlDbType.Bit, 0, "Processed")
                da.InsertCommand.Parameters.Add("@Reason", SqlDbType.VarChar, 150, "Reason")

                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PALRequestDateID", SqlDbType.Int, 0, "PALRequestDateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PALRequestDates").NewRow
            End If
            Update_Field("PALRequestID", _PALRequestID, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("PALHours", _PALHours, dr)
            Update_Field("SSLBHours", _SSLBHours, dr)
            Update_Field("UnpaidHours", _UnpaidHours, dr)
            Update_Field("Processed", _Processed, dr)
            Update_Field("Reason", _Reason, dr)
            If ds.Tables("t_PALRequestDates").Rows.Count < 1 Then ds.Tables("t_PALRequestDates").Rows.Add(dr)
            da.Update(ds, "t_PALRequestDates")
            _ID = ds.Tables("t_PALRequestDates").Rows(0).Item("PALRequestDateID")
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
            oEvents.KeyField = "PALRequestDateID"
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

    Public Function Get_PAL_Dates(ByVal palID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select StartDate, EndDate, PALHours, UnpaidHours, Reason from t_PALRequestDates where PALRequestID = " & palID
        Return ds
    End Function

    Public Property PALRequestID() As Integer
        Get
            Return _PALRequestID
        End Get
        Set(ByVal value As Integer)
            _PALRequestID = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
        End Set
    End Property

    Public Property PALHours() As Decimal
        Get
            Return _PALHours
        End Get
        Set(ByVal value As Decimal)
            _PALHours = value
        End Set
    End Property

    Public Property SSLBHours() As Decimal
        Get
            Return _SSLBHours
        End Get
        Set(ByVal value As Decimal)
            _SSLBHours = value
        End Set
    End Property

    Public Property UnpaidHours() As Decimal
        Get
            Return _UnpaidHours
        End Get
        Set(ByVal value As Decimal)
            _UnpaidHours = value
        End Set
    End Property

    Public Property PALRequestDateID() As Decimal
        Get
            Return _ID
        End Get
        Set(ByVal value As Decimal)
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
    Public Property Processed() As Boolean
        Get
            Return _Processed
        End Get
        Set(ByVal value As Boolean)
            _Processed = value
        End Set
    End Property

    Public Property Reason() As String
        Get
            Return _Reason
        End Get
        Set(ByVal value As String)
            _Reason = value
        End Set
    End Property
End Class
