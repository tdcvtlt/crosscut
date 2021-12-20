Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsScript2Queue
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ScriptID As Integer = 0
    Dim _QueueID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection("data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        cm = New SqlCommand("Select * from t_Script2Queue where Script2QueueID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Script2Queue where Script2QueueID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Script2Queue")
            If ds.Tables("t_Script2Queue").Rows.Count > 0 Then
                dr = ds.Tables("t_Script2Queue").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ScriptID") Is System.DBNull.Value) Then _ScriptID = dr("ScriptID")
        If Not (dr("QueueID") Is System.DBNull.Value) Then _QueueID = dr("QueueID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Script2Queue where Script2QueueID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Script2Queue")
            If ds.Tables("t_Script2Queue").Rows.Count > 0 Then
                dr = ds.Tables("t_Script2Queue").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Script2QueueInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ScriptID", SqlDbType.int, 0, "ScriptID")
                da.InsertCommand.Parameters.Add("@QueueID", SqlDbType.int, 0, "QueueID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Script2QueueID", SqlDbType.Int, 0, "Script2QueueID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Script2Queue").NewRow
            End If
            Update_Field("ScriptID", _ScriptID, dr)
            Update_Field("QueueID", _QueueID, dr)
            If ds.Tables("t_Script2Queue").Rows.Count < 1 Then ds.Tables("t_Script2Queue").Rows.Add(dr)
            da.Update(ds, "t_Script2Queue")
            _ID = ds.Tables("t_Script2Queue").Rows(0).Item("Script2QueueID")
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
            oEvents.KeyField = "Script2QueueID"
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

    Public Property ScriptID() As Integer
        Get
            Return _ScriptID
        End Get
        Set(ByVal value As Integer)
            _ScriptID = value
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

    Public ReadOnly Property Script2QueueID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class

