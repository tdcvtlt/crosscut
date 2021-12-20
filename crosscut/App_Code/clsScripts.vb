Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsScripts
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ScriptName As String = ""
    Dim _Active As Boolean = False
    Dim _QueueID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection("data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        cm = New SqlCommand("Select * from t_Scripts where ScriptID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Scripts where ScriptID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Scripts")
            If ds.Tables("t_Scripts").Rows.Count > 0 Then
                dr = ds.Tables("t_Scripts").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ScriptName") Is System.DBNull.Value) Then _ScriptName = dr("ScriptName")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Scripts where ScriptID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Scripts")
            If ds.Tables("t_Scripts").Rows.Count > 0 Then
                dr = ds.Tables("t_Scripts").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ScriptsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ScriptName", SqlDbType.VarChar, 0, "ScriptName")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ScriptID", SqlDbType.Int, 0, "ScriptID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Scripts").NewRow
            End If
            Update_Field("ScriptName", _ScriptName, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_Scripts").Rows.Count < 1 Then ds.Tables("t_Scripts").Rows.Add(dr)
            da.Update(ds, "t_Scripts")
            _ID = ds.Tables("t_Scripts").Rows(0).Item("ScriptID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Public Function Get_Queue_List(ByVal QueueID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = "data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;"
        ds.SelectCommand = "select * from t_scripts where scriptid in (select scriptid from t_Script2Queue where queueid = " & QueueID & ")"
        Return ds

    End Function

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = "data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;"
        Try
            ds.SelectCommand = "select * from t_scripts"


        Catch ex As Exception

        End Try
        Return ds

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
            oEvents.KeyField = "ScriptID"
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

    Public Property ScriptName() As String
        Get
            Return _ScriptName
        End Get
        Set(ByVal value As String)
            _ScriptName = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property ScriptID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property QueueID() As Integer
        Get
            Return _QueueID
        End Get
        Set(ByVal value As Integer)
            _QueueID = value
        End Set
    End Property

End Class

