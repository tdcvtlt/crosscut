Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsQueues
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _CreatedDate As String = ""
    Dim _RequestedByID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection("data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        cm = New SqlCommand("Select * from t_Queues where QueueID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Queues where QueueID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Queues")
            If ds.Tables("t_Queues").Rows.Count > 0 Then
                dr = ds.Tables("t_Queues").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("CreatedDate") Is System.DBNull.Value) Then _CreatedDate = dr("CreatedDate")
        If Not (dr("RequestedByID") Is System.DBNull.Value) Then _RequestedByID = dr("RequestedByID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Queues where QueueID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Queues")
            If ds.Tables("t_Queues").Rows.Count > 0 Then
                dr = ds.Tables("t_Queues").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_QueuesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@CreatedDate", SqlDbType.datetime, 0, "CreatedDate")
                da.InsertCommand.Parameters.Add("@RequestedByID", SqlDbType.int, 0, "RequestedByID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@QueueID", SqlDbType.Int, 0, "QueueID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Queues").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("CreatedDate", _CreatedDate, dr)
            Update_Field("RequestedByID", _RequestedByID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            If ds.Tables("t_Queues").Rows.Count < 1 Then ds.Tables("t_Queues").Rows.Add(dr)
            da.Update(ds, "t_Queues")
            _ID = ds.Tables("t_Queues").Rows(0).Item("QueueID")
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
            oEvents.KeyField = "QueueID"
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

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return _CreatedDate
        End Get
        Set(ByVal value As String)
            _CreatedDate = value
        End Set
    End Property

    Public Property RequestedByID() As Integer
        Get
            Return _RequestedByID
        End Get
        Set(ByVal value As Integer)
            _RequestedByID = value
        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Integer)
            _DepartmentID = value
        End Set
    End Property

    Public Property QueueID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

