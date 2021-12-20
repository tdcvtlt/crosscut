Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsUnit2Unit
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _UnitID As Integer = 0
    Dim _Unit2ID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Unit2Unit where Unit2UnitID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Unit2Unit where Unit2UnitID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Unit2Unit")
            If ds.Tables("t_Unit2Unit").Rows.Count > 0 Then
                dr = ds.Tables("t_Unit2Unit").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("UnitID") Is System.DBNull.Value) Then _UnitID = dr("UnitID")
        If Not (dr("Unit2ID") Is System.DBNull.Value) Then _Unit2ID = dr("Unit2ID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Unit2Unit where Unit2UnitID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Unit2Unit")
            If ds.Tables("t_Unit2Unit").Rows.Count > 0 Then
                dr = ds.Tables("t_Unit2Unit").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Unit2UnitInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@UnitID", SqlDbType.int, 0, "UnitID")
                da.InsertCommand.Parameters.Add("@Unit2ID", SqlDbType.int, 0, "Unit2ID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Unit2UnitID", SqlDbType.Int, 0, "Unit2UnitID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Unit2Unit").NewRow
            End If
            Update_Field("UnitID", _UnitID, dr)
            Update_Field("Unit2ID", _Unit2ID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Unit2Unit").Rows.Count < 1 Then ds.Tables("t_Unit2Unit").Rows.Add(dr)
            da.Update(ds, "t_Unit2Unit")
            _ID = ds.Tables("t_Unit2Unit").Rows(0).Item("Unit2UnitID")
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
            oEvents.KeyField = "Unit2UnitID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_UnitTies(ByVal unitID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.COnnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ut.Unit2UnitID, u.Name as Unit from t_Unit2Unit ut inner join t_Unit u on ut.Unit2ID = u.UnitID where ut.UnitID = " & unitID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Remove_Tie(ByVal ID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_Unit2Unit where unit2unitid = " & ID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property UnitID() As Integer
        Get
            Return _UnitID
        End Get
        Set(ByVal value As Integer)
            _UnitID = value
        End Set
    End Property

    Public Property Unit2ID() As Integer
        Get
            Return _Unit2ID
        End Get
        Set(ByVal value As Integer)
            _Unit2ID = value
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

    Public Property Unit2UnitID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
