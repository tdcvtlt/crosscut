Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSSLBRequestDates
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _SSLBRequestID As Integer = 0
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _SSLBHours As Integer = 0
    Dim _UnpaidHours As Integer = 0
    Dim _Processed As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SSLBRequestDates where SSLBRequestDateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SSLBRequestDates where SSLBRequestDateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SSLBRequestDates")
            If ds.Tables("t_SSLBRequestDates").Rows.Count > 0 Then
                dr = ds.Tables("t_SSLBRequestDates").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("SSLBRequestID") Is System.DBNull.Value) Then _SSLBRequestID = dr("SSLBRequestID")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("SSLBHours") Is System.DBNull.Value) Then _SSLBHours = dr("SSLBHours")
        If Not (dr("UnpaidHours") Is System.DBNull.Value) Then _UnpaidHours = dr("UnpaidHours")
        If Not (dr("Processed") Is System.DBNull.Value) Then _Processed = dr("Processed")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SSLBRequestDates where SSLBRequestDateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SSLBRequestDates")
            If ds.Tables("t_SSLBRequestDates").Rows.Count > 0 Then
                dr = ds.Tables("t_SSLBRequestDates").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SSLBRequestDatesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@SSLBRequestID", SqlDbType.int, 0, "SSLBRequestID")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.datetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.datetime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@SSLBHours", SqlDbType.int, 0, "SSLBHours")
                da.InsertCommand.Parameters.Add("@UnpaidHours", SqlDbType.int, 0, "UnpaidHours")
                da.InsertCommand.Parameters.Add("@Processed", SqlDbType.bit, 0, "Processed")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SSLBRequestDateID", SqlDbType.Int, 0, "SSLBRequestDateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SSLBRequestDates").NewRow
            End If
            Update_Field("SSLBRequestID", _SSLBRequestID, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("SSLBHours", _SSLBHours, dr)
            Update_Field("UnpaidHours", _UnpaidHours, dr)
            Update_Field("Processed", _Processed, dr)
            If ds.Tables("t_SSLBRequestDates").Rows.Count < 1 Then ds.Tables("t_SSLBRequestDates").Rows.Add(dr)
            da.Update(ds, "t_SSLBRequestDates")
            _ID = ds.Tables("t_SSLBRequestDates").Rows(0).Item("SSLBRequestDateID")
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
            oEvents.KeyField = "SSLBRequestDateID"
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

    Public Function Get_SSLB_Dates(ByVal palID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select StartDate, EndDate, SSLBHours, UnpaidHours from t_SSLBRequestDates where SSLBRequestID = " & palID
        Return ds
    End Function

    Public Property SSLBRequestID() As Integer
        Get
            Return _SSLBRequestID
        End Get
        Set(ByVal value As Integer)
            _SSLBRequestID = value
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

    Public Property SSLBHours() As Integer
        Get
            Return _SSLBHours
        End Get
        Set(ByVal value As Integer)
            _SSLBHours = value
        End Set
    End Property

    Public Property UnpaidHours() As Integer
        Get
            Return _UnpaidHours
        End Get
        Set(ByVal value As Integer)
            _UnpaidHours = value
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

    Public Property SSLBRequestDateID() As Integer
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
