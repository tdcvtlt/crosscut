Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsBank2Room
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DepositID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Bank2Room where Deposit2RoomID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Bank2Room where Deposit2RoomID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Bank2Room")
            If ds.Tables("t_Bank2Room").Rows.Count > 0 Then
                dr = ds.Tables("t_Bank2Room").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DepositID") Is System.DBNull.Value) Then _DepositID = dr("DepositID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Bank2Room where Deposit2RoomID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Bank2Room")
            If ds.Tables("t_Bank2Room").Rows.Count > 0 Then
                dr = ds.Tables("t_Bank2Room").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Bank2RoomInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DepositID", SqlDbType.int, 0, "DepositID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Deposit2RoomID", SqlDbType.Int, 0, "Deposit2RoomID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Bank2Room").NewRow
            End If
            Update_Field("DepositID", _DepositID, dr)
            Update_Field("RoomID", _RoomID, dr)
            If ds.Tables("t_Bank2Room").Rows.Count < 1 Then ds.Tables("t_Bank2Room").Rows.Add(dr)
            da.Update(ds, "t_Bank2Room")
            _ID = ds.Tables("t_Bank2Room").Rows(0).Item("Deposit2RoomID")
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
            oEvents.KeyField = "Deposit2RoomID"
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

    Public Function List_Room(ByVal depositID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select rm.RoomID, rm.RoomNumber from t_Bank2Room b inner join t_Room rm on b.RoomID = rm.ROomID where b.DepositID = " & depositID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Validate(ByVal depositID As Integer, ByVal roomID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Rooms from t_Bank2Room where depositid = " & depositID & " and roomid = " & roomID & ""
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Rooms") = 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Clean_UP(ByVal depositID As Integer, ByVal rooms As String) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_Bank2Room where depositid = " & depositID & " and roomid not in (" & rooms & ")"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Property DepositID() As Integer
        Get
            Return _DepositID
        End Get
        Set(ByVal value As Integer)
            _DepositID = value
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

    Public Property Deposit2RoomID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Err() As Integer
        Get
            Return _Err
        End Get
        Set(ByVal value As Integer)
            _Err = value
        End Set
    End Property
End Class
