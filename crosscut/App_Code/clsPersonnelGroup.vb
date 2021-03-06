Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnelGroup
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _GroupName As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelGroup where PersonnelGroupID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelGroup where PersonnelGroupID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelGroup")
            If ds.Tables("t_PersonnelGroup").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelGroup").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("GroupName") Is System.DBNull.Value) Then _GroupName = dr("GroupName")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelGroup where PersonnelGroupID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelGroup")
            If ds.Tables("t_PersonnelGroup").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelGroup").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelGroupInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@GroupName", SqlDbType.VarChar, 50, "GroupName")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PersonnelGroupID", SqlDbType.Int, 0, "PersonnelGroupID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelGroup").NewRow
            End If
            Update_Field("GroupName", _GroupName, dr)
            If ds.Tables("t_PersonnelGroup").Rows.Count < 1 Then ds.Tables("t_PersonnelGroup").Rows.Add(dr)
            da.Update(ds, "t_PersonnelGroup")
            _ID = ds.Tables("t_PersonnelGroup").Rows(0).Item("PersonnelGroupID")
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
            oEvents.KeyField = "PersonnelGroupID"
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

    Public Function List(ByVal filter As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "" Then
                ds.SelectCommand = "Select PersonnelGroupID as ID, Groupname from t_PersonnelGroup order by groupname asc"
            Else
                ds.SelectCommand = "Select PersonnelGroupID as ID, GroupName from t_PersonnelGroup where groupname = '" & filter & "'"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property GroupName() As String
        Get
            Return _GroupName
        End Get
        Set(ByVal value As String)
            _GroupName = value
        End Set
    End Property

    Public Property PersonnelGroupID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
