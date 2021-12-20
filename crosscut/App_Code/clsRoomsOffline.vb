Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRoomsOffline
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _StartDate As String = ""
    Dim _Enddate As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _ReasonID As Integer = 0
    Dim _Reason As String = ""
    Dim _DateCreated As String = ""
    Dim _OutOfService As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RoomsOffline where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RoomsOffline where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RoomsOffline")
            If ds.Tables("t_RoomsOffline").Rows.Count > 0 Then
                dr = ds.Tables("t_RoomsOffline").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("Enddate") Is System.DBNull.Value) Then _Enddate = dr("Enddate")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("ReasonID") Is System.DBNull.Value) Then _ReasonID = dr("ReasonID")
        If Not (dr("Reason") Is System.DBNull.Value) Then _Reason = dr("Reason")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("OutOfService") Is System.DBNull.Value) Then _OutOfService = dr("OutOfService")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RoomsOffline where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RoomsOffline")
            If ds.Tables("t_RoomsOffline").Rows.Count > 0 Then
                dr = ds.Tables("t_RoomsOffline").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RoomsOfflineInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.Int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.DateTime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@Enddate", SqlDbType.DateTime, 0, "Enddate")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.Int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@ReasonID", SqlDbType.Int, 0, "ReasonID")
                da.InsertCommand.Parameters.Add("@Reason", SqlDbType.VarChar, 0, "Reason")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@OutOfService", SqlDbType.Bit, 0, "OutOfService")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RoomsOffline").NewRow
            End If
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("Enddate", _Enddate, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("Reason", _Reason, dr)
            Update_Field("ReasonID", _ReasonID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("OutOfService", _OutOfService, dr)
            If ds.Tables("t_RoomsOffline").Rows.Count < 1 Then ds.Tables("t_RoomsOffline").Rows.Add(dr)
            da.Update(ds, "t_RoomsOffline")
            _ID = ds.Tables("t_RoomsOffline").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
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

    Public Property Enddate() As String
        Get
            Return _Enddate
        End Get
        Set(ByVal value As String)
            _Enddate = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property ReasonID() As Integer
        Get
            Return _ReasonID
        End Get
        Set(value As Integer)
            _ReasonID = value
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

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property OutOfService() As Boolean
        Get
            Return _OutOfService
        End Get
        Set(ByVal value As Boolean)
            _OutOfService = value
        End Set
    End Property

    Public Property ID() As Integer
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
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class

