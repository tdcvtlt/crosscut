Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _Item2TrackID As Integer = 0
    Dim _DateAdded As String = String.Empty
    Dim _DateRemoved As String = String.Empty
    Dim _DateScheduleCreated As String = ""
    Dim _Description As String = String.Empty
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMItems where PMItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMItems where PMItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMItems")
            If ds.Tables("t_PMItems").Rows.Count > 0 Then
                dr = ds.Tables("t_PMItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
        If Not (dr("Item2TrackID") Is System.DBNull.Value) Then _Item2TrackID = dr("Item2TrackID")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("DateRemoved") Is System.DBNull.Value) Then _DateRemoved = dr("DateRemoved")
        If Not (dr("DateScheduleCreated") Is System.DBNull.Value) Then _DateScheduleCreated = dr("DateScheduleCreated")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMItems where PMItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMItems")
            If ds.Tables("t_PMItems").Rows.Count > 0 Then
                dr = ds.Tables("t_PMItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@Item2TrackID", SqlDbType.int, 0, "Item2TrackID")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.date, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@DateRemoved", SqlDbType.date, 0, "DateRemoved")
                da.InsertCommand.Parameters.Add("@DateScheduleCreated", SqlDbType.datetime, 0, "DateScheduleCreated")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.nchar, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PMItemID", SqlDbType.Int, 0, "PMItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMItems").NewRow
            End If
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("Item2TrackID", _Item2TrackID, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("DateRemoved", _DateRemoved, dr)
            Update_Field("DateScheduleCreated", _DateScheduleCreated, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_PMItems").Rows.Count < 1 Then ds.Tables("t_PMItems").Rows.Add(dr)
            da.Update(ds, "t_PMItems")
            _ID = ds.Tables("t_PMItems").Rows(0).Item("PMItemID")
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
            oEvents.KeyField = "PMItemID"
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

    Public Property KeyField() As String
        Get
            Return _KeyField.Trim()
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property Item2TrackID() As Integer
        Get
            Return _Item2TrackID
        End Get
        Set(ByVal value As Integer)
            _Item2TrackID = value
        End Set
    End Property

    Public Property DateAdded() As String
        Get
            Return _DateAdded.Trim()
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property DateRemoved() As String
        Get
            Return _DateRemoved.Trim()
        End Get
        Set(ByVal value As String)
            _DateRemoved = value
        End Set
    End Property

    Public Property DateScheduleCreated() As String
        Get
            Return _DateScheduleCreated.Trim()
        End Get
        Set(ByVal value As String)
            _DateScheduleCreated = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return IIf(_Description Is Nothing, "", _Description.Trim())
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property PMItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
