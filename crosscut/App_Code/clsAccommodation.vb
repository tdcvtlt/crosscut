Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccommodation
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ReservationID As Integer = 0
    Dim _TourID As Integer = 0
    Dim _AccomLocationID As Integer = 0
    Dim _LodgingID As Integer = 0
    Dim _ConfirmationNumber As String = ""
    Dim _PromoNights As Integer = 0
    Dim _PromoRate As Decimal = 0
    Dim _AdditionalNights As Integer = 0
    Dim _AdditionalRate As Decimal = 0
    Dim _GuestTypeID As Integer = 0
    Dim _RoomTypeID As Integer = 0
    Dim _Smoking As Boolean = False
    Dim _ArrivalDate As String = ""
    Dim _DepartureDate As String = ""
    Dim _NumberAdults As Integer = 0
    Dim _NumberChildren As Integer = 0
    Dim _RoomCost As Decimal = 0
    Dim _AccomID As Integer = 0
    Dim _CheckInLocationID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Accommodation where AccommodationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Accommodation where AccommodationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Accommodation")
            If ds.Tables("t_Accommodation").Rows.Count > 0 Then
                dr = ds.Tables("t_Accommodation").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ReservationID") Is System.DBNull.Value) Then _ReservationID = dr("ReservationID")
        If Not (dr("TourID") Is System.DBNull.Value) Then _TourID = dr("TourID")
        If Not (dr("AccomLocationID") Is System.DBNull.Value) Then _AccomLocationID = dr("AccomLocationID")
        If Not (dr("LodgingID") Is System.DBNull.Value) Then _LodgingID = dr("LodgingID")
        If Not (dr("ConfirmationNumber") Is System.DBNull.Value) Then _ConfirmationNumber = dr("ConfirmationNumber")
        If Not (dr("PromoNights") Is System.DBNull.Value) Then _PromoNights = dr("PromoNights")
        If Not (dr("PromoRate") Is System.DBNull.Value) Then _PromoRate = dr("PromoRate")
        If Not (dr("AdditionalNights") Is System.DBNull.Value) Then _AdditionalNights = dr("AdditionalNights")
        If Not (dr("AdditionalRate") Is System.DBNull.Value) Then _AdditionalRate = dr("AdditionalRate")
        If Not (dr("GuestTypeID") Is System.DBNull.Value) Then _GuestTypeID = dr("GuestTypeID")
        If Not (dr("RoomTypeID") Is System.DBNull.Value) Then _RoomTypeID = dr("RoomTypeID")
        If Not (dr("Smoking") Is System.DBNull.Value) Then _Smoking = dr("Smoking")
        If Not (dr("ArrivalDate") Is System.DBNull.Value) Then _ArrivalDate = dr("ArrivalDate")
        If Not (dr("DepartureDate") Is System.DBNull.Value) Then _DepartureDate = dr("DepartureDate")
        If Not (dr("NumberAdults") Is System.DBNull.Value) Then _NumberAdults = dr("NumberAdults")
        If Not (dr("NumberChildren") Is System.DBNull.Value) Then _NumberChildren = dr("NumberChildren")
        If Not (dr("RoomCost") Is System.DBNull.Value) Then _RoomCost = dr("RoomCost")
        If Not (dr("AccomID") Is System.DBNull.Value) Then _AccomID = dr("AccomID")
        If Not (dr("CheckInLocationID") Is System.DBNull.Value) Then _CheckInLocationID = dr("CheckInLocationID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Accommodation where AccommodationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Accommodation")
            If ds.Tables("t_Accommodation").Rows.Count > 0 Then
                dr = ds.Tables("t_Accommodation").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_AccommodationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.int, 0, "ReservationID")
                da.InsertCommand.Parameters.Add("@TourID", SqlDbType.int, 0, "TourID")
                da.InsertCommand.Parameters.Add("@AccomLocationID", SqlDbType.int, 0, "AccomLocationID")
                da.InsertCommand.Parameters.Add("@LodgingID", SqlDbType.int, 0, "LodgingID")
                da.InsertCommand.Parameters.Add("@ConfirmationNumber", SqlDbType.varchar, 0, "ConfirmationNumber")
                da.InsertCommand.Parameters.Add("@PromoNights", SqlDbType.int, 0, "PromoNights")
                da.InsertCommand.Parameters.Add("@PromoRate", SqlDbType.money, 0, "PromoRate")
                da.InsertCommand.Parameters.Add("@AdditionalNights", SqlDbType.int, 0, "AdditionalNights")
                da.InsertCommand.Parameters.Add("@AdditionalRate", SqlDbType.money, 0, "AdditionalRate")
                da.InsertCommand.Parameters.Add("@GuestTypeID", SqlDbType.int, 0, "GuestTypeID")
                da.InsertCommand.Parameters.Add("@RoomTypeID", SqlDbType.int, 0, "RoomTypeID")
                da.InsertCommand.Parameters.Add("@Smoking", SqlDbType.bit, 0, "Smoking")
                da.InsertCommand.Parameters.Add("@ArrivalDate", SqlDbType.datetime, 0, "ArrivalDate")
                da.InsertCommand.Parameters.Add("@DepartureDate", SqlDbType.datetime, 0, "DepartureDate")
                da.InsertCommand.Parameters.Add("@NumberAdults", SqlDbType.int, 0, "NumberAdults")
                da.InsertCommand.Parameters.Add("@NumberChildren", SqlDbType.int, 0, "NumberChildren")
                da.InsertCommand.Parameters.Add("@RoomCost", SqlDbType.money, 0, "RoomCost")
                da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.Int, 0, "AccomID")
                da.InsertCommand.Parameters.Add("@CheckInLocationID", SqlDbType.Int, 0, "CheckInLocationID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AccommodationID", SqlDbType.Int, 0, "AccommodationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Accommodation").NewRow
            End If
            Update_Field("ReservationID", _ReservationID, dr)
            Update_Field("TourID", _TourID, dr)
            Update_Field("AccomLocationID", _AccomLocationID, dr)
            Update_Field("LodgingID", _LodgingID, dr)
            Update_Field("ConfirmationNumber", _ConfirmationNumber, dr)
            Update_Field("PromoNights", _PromoNights, dr)
            Update_Field("PromoRate", _PromoRate, dr)
            Update_Field("AdditionalNights", _AdditionalNights, dr)
            Update_Field("AdditionalRate", _AdditionalRate, dr)
            Update_Field("GuestTypeID", _GuestTypeID, dr)
            Update_Field("RoomTypeID", _RoomTypeID, dr)
            Update_Field("Smoking", _Smoking, dr)
            Update_Field("ArrivalDate", _ArrivalDate, dr)
            Update_Field("DepartureDate", _DepartureDate, dr)
            Update_Field("NumberAdults", _NumberAdults, dr)
            Update_Field("NumberChildren", _NumberChildren, dr)
            Update_Field("RoomCost", _RoomCost, dr)
            Update_Field("AccomID", _AccomID, dr)
            Update_Field("CheckInLocationID", _CheckInLocationID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Accommodation").Rows.Count < 1 Then ds.Tables("t_Accommodation").Rows.Add(dr)
            da.Update(ds, "t_Accommodation")
            _ID = ds.Tables("t_Accommodation").Rows(0).Item("AccommodationID")
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
            oEvents.KeyField = "AccommodationID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Accoms(ByVal sort As String, ByVal ID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If sort = "Tour" Then
                ds.SelectCommand = "Select a.AccommodationID, al.ComboItem as Accommodation, a.ArrivalDate, a.DepartureDate from t_Accommodation a left outer join t_ComboItems al on a.LodgingID = al.ComboItemID where a.TourID = '" & ID & "'"
            ElseIf sort = "Reservation" Then
                ds.SelectCommand = "Select a.AccommodationID, al.AccomName as Accommodation, a.ArrivalDate, a.DepartureDate from t_Accommodation a left outer join t_Accom al on a.AccomID = al.AccomID where a.ReservationID = '" & ID & "'"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Accom_Name(ByVal resID As Integer) As String
        Dim accomName As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select c.Description as Name from t_Accommodation a left outer join t_ComboItems c on a.LodgingID = c.ComboitemID where a.ReservationID = " & resID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                accomName = dread("Name")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return accomName
    End Function

    Public Function Get_Accom_Size(ByVal resID As Integer) As Integer
        Dim accomSize As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select RoomTypeID from t_Accommodation where reservationid = " & resID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                accomSize = dread("RoomTypeID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return accomSize
    End Function

    Public Function Get_Accom_By_Res(ByVal resID As Integer) As Integer
        Dim accomID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select AccommodationID from t_Accommodation where reservationid = " & resID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                accomID = dread("AccommodationID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return accomID
    End Function
    Public Function Accom_Count(ByVal resID As Integer) As Integer
        Dim aCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Accoms from t_Accommodation where reservationid = " & resID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                aCount = dread("Accoms")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return aCount
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property ReservationID() As Integer
        Get
            Return _ReservationID
        End Get
        Set(ByVal value As Integer)
            _ReservationID = value
        End Set
    End Property

    Public Property TourID() As Integer
        Get
            Return _TourID
        End Get
        Set(ByVal value As Integer)
            _TourID = value
        End Set
    End Property

    Public Property AccomLocationID() As Integer
        Get
            Return _AccomLocationID
        End Get
        Set(ByVal value As Integer)
            _AccomLocationID = value
        End Set
    End Property

    Public Property LodgingID() As Integer
        Get
            Return _LodgingID
        End Get
        Set(ByVal value As Integer)
            _LodgingID = value
        End Set
    End Property

    Public Property ConfirmationNumber() As String
        Get
            Return _ConfirmationNumber
        End Get
        Set(ByVal value As String)
            _ConfirmationNumber = value
        End Set
    End Property

    Public Property PromoNights() As Integer
        Get
            Return _PromoNights
        End Get
        Set(ByVal value As Integer)
            _PromoNights = value
        End Set
    End Property

    Public Property PromoRate() As Decimal
        Get
            Return _PromoRate
        End Get
        Set(ByVal value As Decimal)
            _PromoRate = value
        End Set
    End Property

    Public Property AdditionalNights() As Integer
        Get
            Return _AdditionalNights
        End Get
        Set(ByVal value As Integer)
            _AdditionalNights = value
        End Set
    End Property

    Public Property AdditionalRate() As Decimal
        Get
            Return _AdditionalRate
        End Get
        Set(ByVal value As Decimal)
            _AdditionalRate = value
        End Set
    End Property

    Public Property GuestTypeID() As Integer
        Get
            Return _GuestTypeID
        End Get
        Set(ByVal value As Integer)
            _GuestTypeID = value
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

    Public Property Smoking() As Boolean
        Get
            Return _Smoking
        End Get
        Set(ByVal value As Boolean)
            _Smoking = value
        End Set
    End Property

    Public Property ArrivalDate() As String
        Get
            Return _ArrivalDate
        End Get
        Set(ByVal value As String)
            _ArrivalDate = value
        End Set
    End Property

    Public Property DepartureDate() As String
        Get
            Return _DepartureDate
        End Get
        Set(ByVal value As String)
            _DepartureDate = value
        End Set
    End Property

    Public Property NumberAdults() As Integer
        Get
            Return _NumberAdults
        End Get
        Set(ByVal value As Integer)
            _NumberAdults = value
        End Set
    End Property

    Public Property NumberChildren() As Integer
        Get
            Return _NumberChildren
        End Get
        Set(ByVal value As Integer)
            _NumberChildren = value
        End Set
    End Property

    Public Property RoomCost() As Decimal
        Get
            Return _RoomCost
        End Get
        Set(ByVal value As Decimal)
            _RoomCost = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property AccommodationID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property AccomID() As Integer
        Get
            Return _AccomID
        End Get
        Set(ByVal value As Integer)
            _AccomID = value
        End Set
    End Property
    Public Property CheckInLocationID() As Integer
        Get
            Return _CheckInLocationID
        End Get
        Set(ByVal value As Integer)
            _CheckInLocationID = value
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
