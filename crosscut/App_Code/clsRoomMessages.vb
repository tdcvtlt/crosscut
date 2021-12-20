Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRoomMessages
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _SiteID As Integer = 0
    Dim _Action As String = ""
    Dim _GuestName As String = ""
    Dim _Flag As Boolean = False
    Dim _Extension As String = ""
    Dim _PBXDateIn As String = ""
    Dim _PBXTimeIn As String = ""
    Dim _PBXStatus As String = ""
    Dim _UserName As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RoomMessages where RoomMessageID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RoomMessages where RoomMessageID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RoomMessages")
            If ds.Tables("t_RoomMessages").Rows.Count > 0 Then
                dr = ds.Tables("t_RoomMessages").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("SiteID") Is System.DBNull.Value) Then _SiteID = dr("SiteID")
        If Not (dr("Action") Is System.DBNull.Value) Then _Action = dr("Action")
        If Not (dr("GuestName") Is System.DBNull.Value) Then _GuestName = dr("GuestName")
        If Not (dr("Flag") Is System.DBNull.Value) Then _Flag = dr("Flag")
        If Not (dr("Extension") Is System.DBNull.Value) Then _Extension = dr("Extension")
        If Not (dr("PBXDateIn") Is System.DBNull.Value) Then _PBXDateIn = dr("PBXDateIn")
        If Not (dr("PBXTimeIn") Is System.DBNull.Value) Then _PBXTimeIn = dr("PBXTimeIn")
        If Not (dr("PBXStatus") Is System.DBNull.Value) Then _PBXStatus = dr("PBXStatus")
        If Not (dr("UserName") Is System.DBNull.Value) Then _UserName = dr("UserName")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RoomMessages where RoomMessageID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RoomMessages")
            If ds.Tables("t_RoomMessages").Rows.Count > 0 Then
                dr = ds.Tables("t_RoomMessages").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RoomMessagesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@SiteID", SqlDbType.int, 0, "SiteID")
                da.InsertCommand.Parameters.Add("@Action", SqlDbType.varchar, 0, "Action")
                da.InsertCommand.Parameters.Add("@GuestName", SqlDbType.varchar, 0, "GuestName")
                da.InsertCommand.Parameters.Add("@Flag", SqlDbType.bit, 0, "Flag")
                da.InsertCommand.Parameters.Add("@Extension", SqlDbType.varchar, 0, "Extension")
                da.InsertCommand.Parameters.Add("@PBXDateIn", SqlDbType.datetime, 0, "PBXDateIn")
                da.InsertCommand.Parameters.Add("@PBXTimeIn", SqlDbType.datetime, 0, "PBXTimeIn")
                da.InsertCommand.Parameters.Add("@PBXStatus", SqlDbType.varchar, 0, "PBXStatus")
                da.InsertCommand.Parameters.Add("@UserName", SqlDbType.int, 0, "UserName")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RoomMessageID", SqlDbType.Int, 0, "RoomMessageID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RoomMessages").NewRow
            End If
            Update_Field("SiteID", _SiteID, dr)
            Update_Field("Action", _Action, dr)
            Update_Field("GuestName", _GuestName, dr)
            Update_Field("Flag", _Flag, dr)
            Update_Field("Extension", _Extension, dr)
            Update_Field("PBXDateIn", _PBXDateIn, dr)
            Update_Field("PBXTimeIn", _PBXTimeIn, dr)
            Update_Field("PBXStatus", _PBXStatus, dr)
            Update_Field("UserName", _UserName, dr)
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_RoomMessages").Rows.Count < 1 Then ds.Tables("t_RoomMessages").Rows.Add(dr)
            da.Update(ds, "t_RoomMessages")
            _ID = ds.Tables("t_RoomMessages").Rows(0).Item("RoomMessageID")
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
            oEvents.KeyField = "RoomMessageID"
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

    Public Property SiteID() As Integer
        Get
            Return _SiteID
        End Get
        Set(ByVal value As Integer)
            _SiteID = value
        End Set
    End Property

    Public Property Action() As String
        Get
            Return _Action
        End Get
        Set(ByVal value As String)
            _Action = value
        End Set
    End Property

    Public Property GuestName() As String
        Get
            Return _GuestName
        End Get
        Set(ByVal value As String)
            _GuestName = value
        End Set
    End Property

    Public Property Flag() As Boolean
        Get
            Return _Flag
        End Get
        Set(ByVal value As Boolean)
            _Flag = value
        End Set
    End Property

    Public Property Extension() As String
        Get
            Return _Extension
        End Get
        Set(ByVal value As String)
            _Extension = value
        End Set
    End Property

    Public Property PBXDateIn() As String
        Get
            Return _PBXDateIn
        End Get
        Set(ByVal value As String)
            _PBXDateIn = value
        End Set
    End Property

    Public Property PBXTimeIn() As String
        Get
            Return _PBXTimeIn
        End Get
        Set(ByVal value As String)
            _PBXTimeIn = value
        End Set
    End Property

    Public Property PBXStatus() As String
        Get
            Return _PBXStatus
        End Get
        Set(ByVal value As String)
            _PBXStatus = value
        End Set
    End Property

    Public Property UserName() As Integer
        Get
            Return _UserName
        End Get
        Set(ByVal value As Integer)
            _UserName = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public ReadOnly Property RoomMessageID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
