Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMTeamMembers
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _TeamLeader As Boolean = False
    Dim _TeamID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMTeamMembers where PMTeamMemberID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMTeamMembers where PMTeamMemberID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMTeamMembers")
            If ds.Tables("t_PMTeamMembers").Rows.Count > 0 Then
                dr = ds.Tables("t_PMTeamMembers").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("TeamLeader") Is System.DBNull.Value) Then _TeamLeader = dr("TeamLeader")
        If Not (dr("TeamID") Is System.DBNull.Value) Then _TeamID = dr("TeamID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMTeamMembers where PMTeamMemberID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMTeamMembers")
            If ds.Tables("t_PMTeamMembers").Rows.Count > 0 Then
                dr = ds.Tables("t_PMTeamMembers").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMTeamMembersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@TeamLeader", SqlDbType.bit, 0, "TeamLeader")
                da.InsertCommand.Parameters.Add("@TeamID", SqlDbType.Int, 0, "TeamID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PMTeamMemberID", SqlDbType.Int, 0, "PMTeamMemberID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMTeamMembers").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("TeamLeader", _TeamLeader, dr)
            Update_Field("TeamID", _TeamID, dr)
            If ds.Tables("t_PMTeamMembers").Rows.Count < 1 Then ds.Tables("t_PMTeamMembers").Rows.Add(dr)
            da.Update(ds, "t_PMTeamMembers")
            _ID = ds.Tables("t_PMTeamMembers").Rows(0).Item("PMTeamMemberID")
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
            oEvents.KeyField = "PMTeamMemberID"
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
    Public Function List_Group_Members(ByVal teamID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select pm.PMTeamMemberID as ID, p.FirstName + ' ' + p.Lastname as Personnel, pm.TeamLeader from t_PMTeamMembers pm inner join t_Personnel p on pm.PersonnelID = p.PersonnelID where pm.teamID = " & teamID & " order by p.Lastname"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Remove_Member(ByVal ID As Integer) As Boolean
        Dim bRemoved As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_PMTeamMembers where pmteammemberid = " & ID
            cm.ExecuteNonQuery()
            bRemoved = True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
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

    Public Property TeamID() As Integer
        Get
            Return _TeamID
        End Get
        Set(ByVal value As Integer)
            _TeamID = value
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

    Public Property TeamLeader() As Boolean
        Get
            Return _TeamLeader
        End Get
        Set(ByVal value As Boolean)
            _TeamLeader = value
        End Set
    End Property

    Public Property PMTeamMemberID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
