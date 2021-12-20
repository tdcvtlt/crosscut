Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsGlobalCalendar
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _Event As String = ""
    Dim _EventDate As String = ""
    Dim _EventTypeID As Integer = 0
    Dim _Completed As Boolean = False
    Dim _ProspectID As Integer = 0
    Dim _Keyfield As String = ""
    Dim _Keyvalue As Integer = 0
    Dim _ContactID As Integer = 0
    Dim _ContactMethodID As Integer = 0
    Dim _ContactResultID As Integer = 0
    Dim _ContactDate As String = ""
    Dim _Scheduled As Boolean = False
    Dim _Description As String = ""
    Dim _CompletedByID As Integer = 0
    Dim _DateCompleted As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_GlobalCalendar where CalendarEntryID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_GlobalCalendar where CalendarEntryID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_GlobalCalendar")
            If ds.Tables("t_GlobalCalendar").Rows.Count > 0 Then
                dr = ds.Tables("t_GlobalCalendar").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("Event") Is System.DBNull.Value) Then _Event = dr("Event")
        If Not (dr("EventDate") Is System.DBNull.Value) Then _EventDate = dr("EventDate")
        If Not (dr("EventTypeID") Is System.DBNull.Value) Then _EventTypeID = dr("EventTypeID")
        If Not (dr("Completed") Is System.DBNull.Value) Then _Completed = dr("Completed")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("Keyfield") Is System.DBNull.Value) Then _Keyfield = dr("Keyfield")
        If Not (dr("Keyvalue") Is System.DBNull.Value) Then _Keyvalue = dr("Keyvalue")
        If Not (dr("ContactID") Is System.DBNull.Value) Then _ContactID = dr("ContactID")
        If Not (dr("ContactMethodID") Is System.DBNull.Value) Then _ContactMethodID = dr("ContactMethodID")
        If Not (dr("ContactResultID") Is System.DBNull.Value) Then _ContactResultID = dr("ContactResultID")
        If Not (dr("ContactDate") Is System.DBNull.Value) Then _ContactDate = dr("ContactDate")
        If Not (dr("Scheduled") Is System.DBNull.Value) Then _Scheduled = dr("Scheduled")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("CompletedByID") Is System.DBNull.Value) Then _CompletedByID = dr("CompletedByID")
        If Not (dr("DateCompleted") Is System.DBNull.Value) Then _DateCompleted = dr("DateCompleted")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_GlobalCalendar where CalendarEntryID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_GlobalCalendar")
            If ds.Tables("t_GlobalCalendar").Rows.Count > 0 Then
                dr = ds.Tables("t_GlobalCalendar").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_GlobalCalendarInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@Event", SqlDbType.text, 0, "Event")
                da.InsertCommand.Parameters.Add("@EventDate", SqlDbType.smalldatetime, 0, "EventDate")
                da.InsertCommand.Parameters.Add("@EventTypeID", SqlDbType.int, 0, "EventTypeID")
                da.InsertCommand.Parameters.Add("@Completed", SqlDbType.bit, 0, "Completed")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@Keyfield", SqlDbType.varchar, 0, "Keyfield")
                da.InsertCommand.Parameters.Add("@Keyvalue", SqlDbType.int, 0, "Keyvalue")
                da.InsertCommand.Parameters.Add("@ContactID", SqlDbType.int, 0, "ContactID")
                da.InsertCommand.Parameters.Add("@ContactMethodID", SqlDbType.int, 0, "ContactMethodID")
                da.InsertCommand.Parameters.Add("@ContactResultID", SqlDbType.int, 0, "ContactResultID")
                da.InsertCommand.Parameters.Add("@ContactDate", SqlDbType.smalldatetime, 0, "ContactDate")
                da.InsertCommand.Parameters.Add("@Scheduled", SqlDbType.bit, 0, "Scheduled")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                da.InsertCommand.Parameters.Add("@CompletedByID", SqlDbType.int, 0, "CompletedByID")
                da.InsertCommand.Parameters.Add("@DateCompleted", SqlDbType.smalldatetime, 0, "DateCompleted")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CalendarEntryID", SqlDbType.Int, 0, "CalendarEntryID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_GlobalCalendar").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("Event", _Event, dr)
            Update_Field("EventDate", _EventDate, dr)
            Update_Field("EventTypeID", _EventTypeID, dr)
            Update_Field("Completed", _Completed, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("Keyfield", _Keyfield, dr)
            Update_Field("Keyvalue", _Keyvalue, dr)
            Update_Field("ContactID", _ContactID, dr)
            Update_Field("ContactMethodID", _ContactMethodID, dr)
            Update_Field("ContactResultID", _ContactResultID, dr)
            Update_Field("ContactDate", _ContactDate, dr)
            Update_Field("Scheduled", _Scheduled, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("CompletedByID", _CompletedByID, dr)
            Update_Field("DateCompleted", _DateCompleted, dr)
            If ds.Tables("t_GlobalCalendar").Rows.Count < 1 Then ds.Tables("t_GlobalCalendar").Rows.Add(dr)
            da.Update(ds, "t_GlobalCalendar")
            _ID = ds.Tables("t_GlobalCalendar").Rows(0).Item("CalendarEntryID")
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
            oEvents.KeyField = "CalendarEntryID"
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

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
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

    Public Property Event_Description() As String
        Get
            Return _Event
        End Get
        Set(ByVal value As String)
            _Event = value
        End Set
    End Property

    Public Property EventDate() As String
        Get
            Return _EventDate
        End Get
        Set(ByVal value As String)
            _EventDate = value
        End Set
    End Property

    Public Property EventTypeID() As Integer
        Get
            Return _EventTypeID
        End Get
        Set(ByVal value As Integer)
            _EventTypeID = value
        End Set
    End Property

    Public Property Completed() As Boolean
        Get
            Return _Completed
        End Get
        Set(ByVal value As Boolean)
            _Completed = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property Keyfield() As String
        Get
            Return _Keyfield
        End Get
        Set(ByVal value As String)
            _Keyfield = value
        End Set
    End Property

    Public Property Keyvalue() As Integer
        Get
            Return _Keyvalue
        End Get
        Set(ByVal value As Integer)
            _Keyvalue = value
        End Set
    End Property

    Public Property ContactID() As Integer
        Get
            Return _ContactID
        End Get
        Set(ByVal value As Integer)
            _ContactID = value
        End Set
    End Property

    Public Property ContactMethodID() As Integer
        Get
            Return _ContactMethodID
        End Get
        Set(ByVal value As Integer)
            _ContactMethodID = value
        End Set
    End Property

    Public Property ContactResultID() As Integer
        Get
            Return _ContactResultID
        End Get
        Set(ByVal value As Integer)
            _ContactResultID = value
        End Set
    End Property

    Public Property ContactDate() As String
        Get
            Return _ContactDate
        End Get
        Set(ByVal value As String)
            _ContactDate = value
        End Set
    End Property

    Public Property Scheduled() As Boolean
        Get
            Return _Scheduled
        End Get
        Set(ByVal value As Boolean)
            _Scheduled = value
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

    Public Property CompletedByID() As Integer
        Get
            Return _CompletedByID
        End Get
        Set(ByVal value As Integer)
            _CompletedByID = value
        End Set
    End Property

    Public Property DateCompleted() As String
        Get
            Return _DateCompleted
        End Get
        Set(ByVal value As String)
            _DateCompleted = value
        End Set
    End Property

    Public Property CalendarEntryID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
