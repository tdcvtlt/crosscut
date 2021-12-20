Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSecurityGroups
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SecurityGroups where GroupID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SecurityGroups where GroupID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SecurityGroups")
            If ds.Tables("t_SecurityGroups").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityGroups").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SecurityGroups where GroupID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SecurityGroups")
            If ds.Tables("t_SecurityGroups").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityGroups").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SecurityGroupsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@GroupID", SqlDbType.Int, 0, "GroupID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SecurityGroups").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_SecurityGroups").Rows.Count < 1 Then ds.Tables("t_SecurityGroups").Rows.Add(dr)
            da.Update(ds, "t_SecurityGroups")
            _ID = ds.Tables("t_SecurityGroups").Rows(0).Item("GroupID")
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
            oEvents.KeyField = "GroupID"
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

    Public Function List() As SQLDataSOurce
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select GroupID, Name from t_SecurityGroups order by Name asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Group_Exists(ByVal name As String) As Boolean
        Dim bExists As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Name from t_SecurityGroups where name = '" & name & "'"
            dread = cm.ExecuteReader
            bExists = dread.HasRows()
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bExists = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bExists
    End Function
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
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

    Public Property GroupID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
