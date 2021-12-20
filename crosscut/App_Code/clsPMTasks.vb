Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMTasks
    Dim _UserID As Integer = 0
    Dim _ID As Int32 = 0
    Dim _Name As String = String.Empty
    Dim _Description As String = String.Empty
    Dim _Item2TrackID As Integer = 0
    Dim _Category As String = String.Empty
    Dim _IssueID As Integer = 0
    Dim _Interval As Integer = 0
    Dim _Active As Boolean = False
    Dim _DateAdded As String = String.Empty
    Dim _TeamID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMTasks where TaskID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMTasks where TaskID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMTasks")
            If ds.Tables("t_PMTasks").Rows.Count > 0 Then
                dr = ds.Tables("t_PMTasks").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Item2TrackID") Is System.DBNull.Value) Then _Item2TrackID = dr("Item2TrackID")
        If Not (dr("Category") Is System.DBNull.Value) Then _Category = dr("Category")
        If Not (dr("IssueID") Is System.DBNull.Value) Then _IssueID = dr("IssueID")
        If Not (dr("Interval") Is System.DBNull.Value) Then _Interval = dr("Interval")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("TeamID") Is System.DBNull.Value) Then _TeamID = dr("TeamID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMTasks where TaskID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMTasks")
            If ds.Tables("t_PMTasks").Rows.Count > 0 Then
                dr = ds.Tables("t_PMTasks").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMTasksInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.nchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.nchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@Item2TrackID", SqlDbType.int, 0, "Item2TrackID")
                da.InsertCommand.Parameters.Add("@Category", SqlDbType.nchar, 0, "Category")
                da.InsertCommand.Parameters.Add("@IssueID", SqlDbType.int, 0, "IssueID")
                da.InsertCommand.Parameters.Add("@Interval", SqlDbType.int, 0, "Interval")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.Date, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@TeamID", SqlDbType.Int, 0, "TeamID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TaskID", SqlDbType.Int, 0, "TaskID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMTasks").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Item2TrackID", _Item2TrackID, dr)
            Update_Field("Category", _Category, dr)
            Update_Field("IssueID", _IssueID, dr)
            Update_Field("Interval", _Interval, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("TeamID", _TeamID, dr)
            If ds.Tables("t_PMTasks").Rows.Count < 1 Then ds.Tables("t_PMTasks").Rows.Add(dr)
            da.Update(ds, "t_PMTasks")
            _ID = ds.Tables("t_PMTasks").Rows(0).Item("TaskID")
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
            oEvents.KeyField = "TaskID"
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
            Return _Name.Trim()
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description.Trim()
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public Property Category() As String
        Get
            Return _Category.Trim()
        End Get
        Set(ByVal value As String)
            _Category = value
        End Set
    End Property

    Public Property IssueID() As Integer
        Get
            Return _IssueID
        End Get
        Set(ByVal value As Integer)
            _IssueID = value
        End Set
    End Property

    Public Property Interval() As Integer
        Get
            Return _Interval
        End Get
        Set(ByVal value As Integer)
            _Interval = value
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

    Public Property DateAdded() As String
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property TaskID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property TeamID() As Integer
        Get
            Return _TeamID
        End Get
        Set(ByVal value As Integer)
            _TeamID = value
        End Set
    End Property
End Class
