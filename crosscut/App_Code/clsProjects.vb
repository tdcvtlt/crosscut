Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProjects
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _DateCreated As String = ""
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Projects where ProjectID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Projects where ProjectID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Projects")
            If ds.Tables("t_Projects").Rows.Count > 0 Then
                dr = ds.Tables("t_Projects").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Get_Progress(ProjectID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "exec sp_ProjectProgress " & ProjectID
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Query(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            Dim sName(2) As String
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " ProjectID as ID, Name, ps.comboitem as Status from t_Projects p left outer join t_Comboitems ps on ps.comboitemid = p.statusid  "
            If sFilterField <> "" Then
                If sFilterField = "Name" Then
                    sql += " where p.Name like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' "
                ElseIf sFilterField = "ProjectID" Then
                    sql += " where p.ProjectID like '" & sFilterValue & "%' "
                
                Else
                    sql += " where " & sFilterField & " like '" & sFilterValue & "%' "
                End If
            End If

            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Projects where ProjectID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Projects")
            If ds.Tables("t_Projects").Rows.Count > 0 Then
                dr = ds.Tables("t_Projects").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ProjectsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.datetime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ProjectID", SqlDbType.Int, 0, "ProjectID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Projects").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            If ds.Tables("t_Projects").Rows.Count < 1 Then ds.Tables("t_Projects").Rows.Add(dr)
            da.Update(ds, "t_Projects")
            _ID = ds.Tables("t_Projects").Rows(0).Item("ProjectID")
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
            oEvents.KeyField = "ProjectID"
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
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property ProjectID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
