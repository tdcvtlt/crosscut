Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadProgram2Location
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LeadProgramID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Version As Double = 0
    Dim _Active As Boolean = False
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadProgram2Location where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadProgram2Location where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram2Location")
            If ds.Tables("t_LeadProgram2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram2Location").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LeadProgramID") Is System.DBNull.Value) Then _LeadProgramID = dr("LeadProgramID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("Version") Is System.DBNull.Value) Then _Version = dr("Version")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadProgram2Location where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram2Location")
            If ds.Tables("t_LeadProgram2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram2Location").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadProgram2LocationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LeadProgramID", SqlDbType.int, 0, "LeadProgramID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@Version", SqlDbType.Float, 0, "Version")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.DateTime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.DateTime, 0, "EndDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadProgram2Location").NewRow
            End If
            Update_Field("LeadProgramID", _LeadProgramID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("Version", _Version, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            If ds.Tables("t_LeadProgram2Location").Rows.Count < 1 Then ds.Tables("t_LeadProgram2Location").Rows.Add(dr)
            da.Update(ds, "t_LeadProgram2Location")
            _ID = ds.Tables("t_LeadProgram2Location").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
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

    Public Function get_Active_Location(ByVal progID As Integer) As Integer
        Dim locID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ID from t_LeadProgram2Location where LeadProgramID = " & progID & " and Active = 1"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                locID = dread("ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return locID
    End Function

    Public Function List_Locations(ByVal LPID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select l.ID, c.ComboItem as Location, l.Version, l.StartDate, l.EndDate, l.Active from t_LeadProgram2Location l inner join t_ComboItems c on l.LocationID = c.ComboItemID where l.LeadProgramID = " & LPID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property LeadProgramID() As Integer
        Get
            Return _LeadProgramID
        End Get
        Set(ByVal value As Integer)
            _LeadProgramID = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property Version() As Double
        Get
            Return _Version
        End Get
        Set(ByVal value As Double)
            _Version = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
