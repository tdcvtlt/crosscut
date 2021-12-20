Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProject2Room
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProjectID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Project2Room where ProjectRoomID = " & _ID, cn)
    End Sub

    Public Function List(ProjectID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select RoomNumber as Room, Address1 as Unit from t_Room a inner join t_Unit u on u.unitid = a.unitid inner join t_Project2Room p on p.roomid = a.roomid where p.projectid = " & ProjectID
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Project2Room where ProjectRoomID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Project2Room")
            If ds.Tables("t_Project2Room").Rows.Count > 0 Then
                dr = ds.Tables("t_Project2Room").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProjectID") Is System.DBNull.Value) Then _ProjectID = dr("ProjectID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Project2Room where ProjectRoomID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Project2Room")
            If ds.Tables("t_Project2Room").Rows.Count > 0 Then
                dr = ds.Tables("t_Project2Room").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Project2RoomInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProjectID", SqlDbType.int, 0, "ProjectID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ProjectRoomID", SqlDbType.Int, 0, "ProjectRoomID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Project2Room").NewRow
            End If
            Update_Field("ProjectID", _ProjectID, dr)
            Update_Field("RoomID", _RoomID, dr)
            If ds.Tables("t_Project2Room").Rows.Count < 1 Then ds.Tables("t_Project2Room").Rows.Add(dr)
            da.Update(ds, "t_Project2Room")
            _ID = ds.Tables("t_Project2Room").Rows(0).Item("ProjectRoomID")
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
            oEvents.KeyField = "ProjectRoomID"
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

    Public Property ProjectID() As Integer
        Get
            Return _ProjectID
        End Get
        Set(ByVal value As Integer)
            _ProjectID = value
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

    Public Property ProjectRoomID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
End Class
