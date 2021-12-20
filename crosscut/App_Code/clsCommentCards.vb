Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCommentCards
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _CardDate As String = ""
    Dim _DateEntered As String = ""
    Dim _EnteredByID As Integer = 0
    Dim _FrontDesk As Integer = 0
    Dim _HouseKeeping As Integer = 0
    Dim _ResortAmmentities As Integer = 0
    Dim _Maintenance As Integer = 0
    Dim _Hospitality As Integer = 0
    Dim _UnitQuality As Integer = 0
    Dim _SPA As Integer = 0
    Dim _OverAllEval As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CommentCards where CardID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CommentCards where CardID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CommentCards")
            If ds.Tables("t_CommentCards").Rows.Count > 0 Then
                dr = ds.Tables("t_CommentCards").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("CardDate") Is System.DBNull.Value) Then _CardDate = dr("CardDate")
        If Not (dr("DateEntered") Is System.DBNull.Value) Then _DateEntered = dr("DateEntered")
        If Not (dr("EnteredByID") Is System.DBNull.Value) Then _EnteredByID = dr("EnteredByID")
        If Not (dr("FrontDesk") Is System.DBNull.Value) Then _FrontDesk = dr("FrontDesk")
        If Not (dr("HouseKeeping") Is System.DBNull.Value) Then _HouseKeeping = dr("HouseKeeping")
        If Not (dr("ResortAmmentities") Is System.DBNull.Value) Then _ResortAmmentities = dr("ResortAmmentities")
        If Not (dr("Maintenance") Is System.DBNull.Value) Then _Maintenance = dr("Maintenance")
        If Not (dr("Hospitality") Is System.DBNull.Value) Then _Hospitality = dr("Hospitality")
        If Not (dr("UnitQuality") Is System.DBNull.Value) Then _UnitQuality = dr("UnitQuality")
        If Not (dr("SPA") Is System.DBNull.Value) Then _SPA = dr("SPA")
        If Not (dr("OverAllEval") Is System.DBNull.Value) Then _OverAllEval = dr("OverAllEval")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CommentCards where CardID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CommentCards")
            If ds.Tables("t_CommentCards").Rows.Count > 0 Then
                dr = ds.Tables("t_CommentCards").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CommentCardsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@CardDate", SqlDbType.datetime, 0, "CardDate")
                da.InsertCommand.Parameters.Add("@DateEntered", SqlDbType.datetime, 0, "DateEntered")
                da.InsertCommand.Parameters.Add("@EnteredByID", SqlDbType.int, 0, "EnteredByID")
                da.InsertCommand.Parameters.Add("@FrontDesk", SqlDbType.int, 0, "FrontDesk")
                da.InsertCommand.Parameters.Add("@HouseKeeping", SqlDbType.int, 0, "HouseKeeping")
                da.InsertCommand.Parameters.Add("@ResortAmmentities", SqlDbType.int, 0, "ResortAmmentities")
                da.InsertCommand.Parameters.Add("@Maintenance", SqlDbType.int, 0, "Maintenance")
                da.InsertCommand.Parameters.Add("@Hospitality", SqlDbType.int, 0, "Hospitality")
                da.InsertCommand.Parameters.Add("@UnitQuality", SqlDbType.int, 0, "UnitQuality")
                da.InsertCommand.Parameters.Add("@SPA", SqlDbType.int, 0, "SPA")
                da.InsertCommand.Parameters.Add("@OverAllEval", SqlDbType.int, 0, "OverAllEval")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CardID", SqlDbType.Int, 0, "CardID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CommentCards").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("CardDate", _CardDate, dr)
            Update_Field("DateEntered", _DateEntered, dr)
            Update_Field("EnteredByID", _EnteredByID, dr)
            Update_Field("FrontDesk", _FrontDesk, dr)
            Update_Field("HouseKeeping", _HouseKeeping, dr)
            Update_Field("ResortAmmentities", _ResortAmmentities, dr)
            Update_Field("Maintenance", _Maintenance, dr)
            Update_Field("Hospitality", _Hospitality, dr)
            Update_Field("UnitQuality", _UnitQuality, dr)
            Update_Field("SPA", _SPA, dr)
            Update_Field("OverAllEval", _OverAllEval, dr)
            If ds.Tables("t_CommentCards").Rows.Count < 1 Then ds.Tables("t_CommentCards").Rows.Add(dr)
            da.Update(ds, "t_CommentCards")
            _ID = ds.Tables("t_CommentCards").Rows(0).Item("CardID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        End Try

        Return False

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
            oEvents.KeyField = "CardID"
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

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
        End Set
    End Property

    Public Property CardDate() As String
        Get
            Return _CardDate
        End Get
        Set(ByVal value As String)
            _CardDate = value
        End Set
    End Property

    Public Property DateEntered() As String
        Get
            Return _DateEntered
        End Get
        Set(ByVal value As String)
            _DateEntered = value
        End Set
    End Property

    Public Property EnteredByID() As Integer
        Get
            Return _EnteredByID
        End Get
        Set(ByVal value As Integer)
            _EnteredByID = value
        End Set
    End Property

    Public Property FrontDesk() As Integer
        Get
            Return _FrontDesk
        End Get
        Set(ByVal value As Integer)
            _FrontDesk = value
        End Set
    End Property

    Public Property HouseKeeping() As Integer
        Get
            Return _HouseKeeping
        End Get
        Set(ByVal value As Integer)
            _HouseKeeping = value
        End Set
    End Property

    Public Property ResortAmmentities() As Integer
        Get
            Return _ResortAmmentities
        End Get
        Set(ByVal value As Integer)
            _ResortAmmentities = value
        End Set
    End Property

    Public Property Maintenance() As Integer
        Get
            Return _Maintenance
        End Get
        Set(ByVal value As Integer)
            _Maintenance = value
        End Set
    End Property

    Public Property Hospitality() As Integer
        Get
            Return _Hospitality
        End Get
        Set(ByVal value As Integer)
            _Hospitality = value
        End Set
    End Property

    Public Property UnitQuality() As Integer
        Get
            Return _UnitQuality
        End Get
        Set(ByVal value As Integer)
            _UnitQuality = value
        End Set
    End Property

    Public Property SPA() As Integer
        Get
            Return _SPA
        End Get
        Set(ByVal value As Integer)
            _SPA = value
        End Set
    End Property

    Public Property OverAllEval() As Integer
        Get
            Return _OverAllEval
        End Get
        Set(ByVal value As Integer)
            _OverAllEval = value
        End Set
    End Property

    Public ReadOnly Property CardID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class

