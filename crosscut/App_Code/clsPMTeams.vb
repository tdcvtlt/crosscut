Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMTeams
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMTeams where PMTeamID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMTeams where PMTeamID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMTeams")
            If ds.Tables("t_PMTeams").Rows.Count > 0 Then
                dr = ds.Tables("t_PMTeams").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMTeams where PMTeamID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMTeams")
            If ds.Tables("t_PMTeams").Rows.Count > 0 Then
                dr = ds.Tables("t_PMTeams").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMTeamsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PMTeamID", SqlDbType.Int, 0, "PMTeamID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMTeams").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PMTeams").Rows.Count < 1 Then ds.Tables("t_PMTeams").Rows.Add(dr)
            da.Update(ds, "t_PMTeams")
            _ID = ds.Tables("t_PMTeams").Rows(0).Item("PMTeamID")
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
            oEvents.KeyField = "PMTeamID"
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

    Public Function List_Teams(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If filter <> "" Then
            ds.SelectCommand = "Select * from t_PMTeams where name like '" & filter & "%'"
        Else
            ds.SelectCommand = "Select * from t_PMTeams order by name asc"
        End If
        Return ds
    End Function

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
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

    Public Property PMTeamID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property
End Class
