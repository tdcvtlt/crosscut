Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCalendarEvents
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CalendarID As Integer = 0
    Dim _Date As String = ""
    Dim _Time As String = ""
    Dim _Description As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CalendarEvents where EventID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CalendarEvents where EventID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CalendarEvents")
            If ds.Tables("t_CalendarEvents").Rows.Count > 0 Then
                dr = ds.Tables("t_CalendarEvents").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("CalendarID") Is System.DBNull.Value) Then _CalendarID = dr("CalendarID")
        If Not (dr("Date") Is System.DBNull.Value) Then _Date = dr("Date")
        If Not (dr("Time") Is System.DBNull.Value) Then _Time = dr("Time")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CalendarEvents where EventID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CalendarEvents")
            If ds.Tables("t_CalendarEvents").Rows.Count > 0 Then
                dr = ds.Tables("t_CalendarEvents").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CalendarEventsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CalendarID", SqlDbType.int, 0, "CalendarID")
                da.InsertCommand.Parameters.Add("@Date", SqlDbType.smalldatetime, 0, "Date")
                da.InsertCommand.Parameters.Add("@Time", SqlDbType.varchar, 0, "Time")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@EventID", SqlDbType.Int, 0, "EventID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CalendarEvents").NewRow
            End If
            Update_Field("CalendarID", _CalendarID, dr)
            Update_Field("Date", _Date, dr)
            Update_Field("Time", _Time, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_CalendarEvents").Rows.Count < 1 Then ds.Tables("t_CalendarEvents").Rows.Add(dr)
            da.Update(ds, "t_CalendarEvents")
            _ID = ds.Tables("t_CalendarEvents").Rows(0).Item("EventID")
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
            oEvents.KeyField = "EventID"
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

    Public Property CalendarID() As Integer
        Get
            Return _CalendarID
        End Get
        Set(ByVal value As Integer)
            _CalendarID = value
        End Set
    End Property

    Public Property EventDate() As String
        Get
            Return _Date
        End Get
        Set(ByVal value As String)
            _Date = value
        End Set
    End Property

    Public Property Time() As String
        Get
            Return _Time
        End Get
        Set(ByVal value As String)
            _Time = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public Property EventID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
