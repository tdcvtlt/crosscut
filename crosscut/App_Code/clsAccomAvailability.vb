Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccomAvailability
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AccomID As Integer = 0
    Dim _RoomTypeID As Integer = 0
    Dim _DateAvail As Integer = 0
    Dim _ReservationID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_AccomAvailability where AccomAvailabilityID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_AccomAvailability where AccomAvailabilityID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_AccomAvailability")
            If ds.Tables("t_AccomAvailability").Rows.Count > 0 Then
                dr = ds.Tables("t_AccomAvailability").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AccomID") Is System.DBNull.Value) Then _AccomID = dr("AccomID")
        If Not (dr("RoomTypeID") Is System.DBNull.Value) Then _RoomTypeID = dr("RoomTypeID")
        If Not (dr("DateAvail") Is System.DBNull.Value) Then _DateAvail = dr("DateAvail")
        If Not (dr("ReservationID") Is System.DBNull.Value) Then _ReservationID = dr("ReservationID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_AccomAvailability where AccomAvailabilityID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_AccomAvailability")
            If ds.Tables("t_AccomAvailability").Rows.Count > 0 Then
                dr = ds.Tables("t_AccomAvailability").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_AccomAvailabilityInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.int, 0, "AccomID")
                da.InsertCommand.Parameters.Add("@RoomTypeID", SqlDbType.int, 0, "RoomTypeID")
                da.InsertCommand.Parameters.Add("@DateAvail", SqlDbType.int, 0, "DateAvail")
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.int, 0, "ReservationID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AccomAvailabilityID", SqlDbType.Int, 0, "AccomAvailabilityID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_AccomAvailability").NewRow
            End If
            Update_Field("AccomID", _AccomID, dr)
            Update_Field("RoomTypeID", _RoomTypeID, dr)
            Update_Field("DateAvail", _DateAvail, dr)
            Update_Field("ReservationID", _ReservationID, dr)
            If ds.Tables("t_AccomAvailability").Rows.Count < 1 Then ds.Tables("t_AccomAvailability").Rows.Add(dr)
            da.Update(ds, "t_AccomAvailability")
            _ID = ds.Tables("t_AccomAvailability").Rows(0).Item("AccomAvailabilityID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "AccomAvailabilityID"
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

    Public Property AccomID() As Integer
        Get
            Return _AccomID
        End Get
        Set(ByVal value As Integer)
            _AccomID = value
        End Set
    End Property

    Public Property RoomTypeID() As Integer
        Get
            Return _RoomTypeID
        End Get
        Set(ByVal value As Integer)
            _RoomTypeID = value
        End Set
    End Property

    Public Property DateAvail() As Integer
        Get
            Return _DateAvail
        End Get
        Set(ByVal value As Integer)
            _DateAvail = value
        End Set
    End Property

    Public Property ReservationID() As Integer
        Get
            Return _ReservationID
        End Get
        Set(ByVal value As Integer)
            _ReservationID = value
        End Set
    End Property

    Public Property AccomAvailabilityID() As Integer
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
