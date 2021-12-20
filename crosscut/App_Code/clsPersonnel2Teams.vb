Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnel2Teams
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TeamID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Personnel2Teams where Pers2TeamID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Personnel2Teams where Pers2TeamID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Personnel2Teams")
            If ds.Tables("t_Personnel2Teams").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel2Teams").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("TeamID") Is System.DBNull.Value) Then _TeamID = dr("TeamID")
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel2Teams where Pers2TeamID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Personnel2Teams")
            If ds.Tables("t_Personnel2Teams").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel2Teams").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Personnel2TeamsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TeamID", SqlDbType.int, 0, "TeamID")
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Pers2TeamID", SqlDbType.Int, 0, "Pers2TeamID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Personnel2Teams").NewRow
            End If
            Update_Field("TeamID", _TeamID, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            If ds.Tables("t_Personnel2Teams").Rows.Count < 1 Then ds.Tables("t_Personnel2Teams").Rows.Add(dr)
            da.Update(ds, "t_Personnel2Teams")
            _ID = ds.Tables("t_Personnel2Teams").Rows(0).Item("Pers2TeamID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "Pers2TeamID"
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

    Public Function List_Teams(ByVal persID As Integer) As SQLDataSource
        Dim ds As New sqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.Pers2TeamID as ID, t.ComboItem as SalesTeam from t_Personnel2Teams p inner join t_ComboItems t on p.TeamID = t.ComboItemID where p.PersonnelID = " & persID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Remove_Team(ByVal ID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete From t_Personnel2Teams where Pers2TeamID = " & ID & ""
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bRemoved = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function

    Public Property TeamID() As Integer
        Get
            Return _TeamID
        End Get
        Set(ByVal value As Integer)
            _TeamID = value
        End Set
    End Property

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property Pers2TeamID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
