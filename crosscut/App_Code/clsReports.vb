Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsReports
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Description As String = ""
    Dim _filename As String = ""
    Dim _filegroup As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Reports where ReportID = " & _ID, cn)
    End Sub

    Public Function List_Group(ByVal group As String) As SqlDataSource
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select reportid as ID, Name, Description as [Desc], filename as Path from t_Reports where active = 1 and filegroup = '" & group & "' order by Name")
        Return ds
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Reports where ReportID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Reports")
            If ds.Tables("t_Reports").Rows.Count > 0 Then
                dr = ds.Tables("t_Reports").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("filename") Is System.DBNull.Value) Then _filename = dr("filename")
        If Not (dr("filegroup") Is System.DBNull.Value) Then _filegroup = dr("filegroup")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Reports where ReportID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Reports")
            If ds.Tables("t_Reports").Rows.Count > 0 Then
                dr = ds.Tables("t_Reports").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReportsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@filename", SqlDbType.varchar, 0, "filename")
                da.InsertCommand.Parameters.Add("@filegroup", SqlDbType.varchar, 0, "filegroup")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReportID", SqlDbType.Int, 0, "ReportID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Reports").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("filename", _filename, dr)
            Update_Field("filegroup", _filegroup, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_Reports").Rows.Count < 1 Then ds.Tables("t_Reports").Rows.Add(dr)
            da.Update(ds, "t_Reports")
            _ID = ds.Tables("t_Reports").Rows(0).Item("ReportID")
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
            oEvents.KeyField = "ReportID"
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property filename() As String
        Get
            Return _filename
        End Get
        Set(ByVal value As String)
            _filename = value
        End Set
    End Property

    Public Property filegroup() As String
        Get
            Return _filegroup
        End Get
        Set(ByVal value As String)
            _filegroup = value
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

    Public ReadOnly Property ReportID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
