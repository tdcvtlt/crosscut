Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRoom2MaintTech
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _RepID As Integer = 0
    Dim _ExpirationDate As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Room2MaintTech where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Room2MaintTech where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Room2MaintTech")
            If ds.Tables("t_Room2MaintTech").Rows.Count > 0 Then
                dr = ds.Tables("t_Room2MaintTech").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("RepID") Is System.DBNull.Value) Then _RepID = dr("RepID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Room2MaintTech where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Room2MaintTech")
            If ds.Tables("t_Room2MaintTech").Rows.Count > 0 Then
                dr = ds.Tables("t_Room2MaintTech").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Room2MaintTechInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.Int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@RepID", SqlDbType.Int, 0, "RepID")
                da.InsertCommand.Parameters.Add("@ExpirationDate", SqlDbType.DateTime, 0, "ExpirationDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Room2MaintTech").NewRow
            End If
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("RepID", _RepID, dr)
            If _ExpirationDate = "" Then
                Create_Event("ExpirationDate", _ExpirationDate, dr)
                dr("ExpirationDate") = System.DBNull.Value
            Else
                Update_Field("ExpirationDate", _ExpirationDate, dr)
            End If

            If ds.Tables("t_Room2MaintTech").Rows.Count < 1 Then ds.Tables("t_Room2MaintTech").Rows.Add(dr)
            da.Update(ds, "t_Room2MaintTech")
            _ID = ds.Tables("t_Room2MaintTech").Rows(0).Item("ID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Create_Event(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), CStr(drow(sField) & ""), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

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

    Public Function chk_Exist(ByVal repID As Integer, ByVal roomID As Integer) As Boolean
        Dim bFound As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Reps from t_Room2MaintTech where (expirationDate is null or expirationdate > getdate()-1) and repID = '" & repID & "' and roomid = '" & roomID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("Reps") > 0 Then
                    bFound = True
                End If
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bFound
    End Function

    Public Function List_Assigned_Rooms(ByVal techID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select rt.ID, rm.RoomNumber,rt.ExpirationDate from t_Room2MaintTech rt inner join t_Room rm on rt.RoomID = rm.RoomID where (rt.ExpirationDate is null or rt.ExpirationDate > getdate()-1) and  rt.RepID = " & techID
        Return ds
    End Function

    Public Function Delete_Tie(ByVal ID As Integer) As Boolean
        Dim bRem As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_Room2MaintTech where ID = " & ID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bRem = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRem
    End Function


    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
        End Set
    End Property

    Public Property ExpirationDate As String
        Get
            Return _ExpirationDate
        End Get
        Set(value As String)
            _ExpirationDate = value
        End Set
    End Property

    Public Property RepID() As Integer
        Get
            Return _RepID
        End Get
        Set(ByVal value As Integer)
            _RepID = value
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
End Class
