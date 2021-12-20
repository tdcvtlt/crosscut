Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRoomAllocationMatrix
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _DateAllocated As String = ""
    Dim _ReservationID As Integer = 0
    Dim _UsageID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RoomAllocationMatrix where AllocationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RoomAllocationMatrix where AllocationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RoomAllocationMatrix")
            If ds.Tables("t_RoomAllocationMatrix").Rows.Count > 0 Then
                dr = ds.Tables("t_RoomAllocationMatrix").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Allocation_Exists(ByVal sDate As String, ByVal RoomID As Integer) As Boolean
        Dim bRet As Boolean = False
        Try
            cm.CommandText = "Select * from t_RoomAllocationMatrix where RoomID = " & RoomID & " and DateAllocated = '" & sDate & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "RM")
            If ds.Tables("RM").Rows.Count > 0 Then bRet = True
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            bRet = False
            _Err = ex.ToString
        End Try
        Return bRet
    End Function

    Private Sub Set_Values()
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("DateAllocated") Is System.DBNull.Value) Then _DateAllocated = dr("DateAllocated")
        If Not (dr("ReservationID") Is System.DBNull.Value) Then _ReservationID = dr("ReservationID")
        If Not (dr("UsageID") Is System.DBNull.Value) Then _UsageID = dr("UsageID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RoomAllocationMatrix where AllocationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RoomAllocationMatrix")
            If ds.Tables("t_RoomAllocationMatrix").Rows.Count > 0 Then
                dr = ds.Tables("t_RoomAllocationMatrix").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RoomAllocationMatrixInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@DateAllocated", SqlDbType.smalldatetime, 0, "DateAllocated")
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.int, 0, "ReservationID")
                da.InsertCommand.Parameters.Add("@UsageID", SqlDbType.int, 0, "UsageID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AllocationID", SqlDbType.Int, 0, "AllocationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RoomAllocationMatrix").NewRow
            End If
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("DateAllocated", _DateAllocated, dr)
            Update_Field("ReservationID", _ReservationID, dr)
            Update_Field("UsageID", _UsageID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_RoomAllocationMatrix").Rows.Count < 1 Then ds.Tables("t_RoomAllocationMatrix").Rows.Add(dr)
            da.Update(ds, "t_RoomAllocationMatrix")
            _ID = ds.Tables("t_RoomAllocationMatrix").Rows(0).Item("AllocationID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function Get_Allocation_ID(ByVal sDate As String, ByVal RoomID As Integer) As Integer
        Dim bRet As Integer = 0
        Try
            cm.CommandText = "Select * from t_RoomAllocationMatrix where RoomID = " & RoomID & " and DateAllocated = '" & sDate & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "RM")
            If ds.Tables("RM").Rows.Count > 0 Then bRet = ds.Tables("RM").Rows(0)("AllocationID")
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            bRet = False
            _Err = ex.ToString
        End Try
        Return bRet
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
            oEvents.KeyField = "AllocationID"
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

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property DateAllocated() As String
        Get
            Return _DateAllocated
        End Get
        Set(ByVal value As String)
            _DateAllocated = value
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

    Public Property UsageID() As Integer
        Get
            Return _UsageID
        End Get
        Set(ByVal value As Integer)
            _UsageID = value
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

    Public Property AllocationID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property
End Class
