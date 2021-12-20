Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadFiles
    Dim _UserDBID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FileName As String = ""
    Dim _Status As String = ""
    Dim _Header As Boolean = False
    Dim _PhoneColumn As Integer = 0
    Dim _UserID As Integer = 0
    Dim _DateUploaded As String = ""
    Dim _DateCompleted As String = ""
    Dim _DateDownloaded As String = ""
    Dim _TemplateID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadFiles where FileID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadFiles where FileID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadFiles")
            If ds.Tables("t_LeadFiles").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadFiles").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LeadFiles order by fileid desc")
    End Function

    Private Sub Set_Values()
        If Not (dr("FileName") Is System.DBNull.Value) Then _FileName = dr("FileName")
        If Not (dr("Status") Is System.DBNull.Value) Then _Status = dr("Status")
        If Not (dr("Header") Is System.DBNull.Value) Then _Header = dr("Header")
        If Not (dr("PhoneColumn") Is System.DBNull.Value) Then _PhoneColumn = dr("PhoneColumn")
        If Not (dr("UserID") Is System.DBNull.Value) Then _UserID = dr("UserID")
        If Not (dr("DateUploaded") Is System.DBNull.Value) Then _DateUploaded = dr("DateUploaded")
        If Not (dr("DateCompleted") Is System.DBNull.Value) Then _DateCompleted = dr("DateCompleted")
        If Not (dr("DateDownloaded") Is System.DBNull.Value) Then _DateDownloaded = dr("DateDownloaded")
        If Not (dr("TemplateID") Is System.DBNull.Value) Then _TemplateID = dr("TemplateID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadFiles where FileID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadFiles")
            If ds.Tables("t_LeadFiles").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadFiles").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadFilesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FileName", SqlDbType.varchar, 0, "FileName")
                da.InsertCommand.Parameters.Add("@Status", SqlDbType.varchar, 0, "Status")
                da.InsertCommand.Parameters.Add("@Header", SqlDbType.bit, 0, "Header")
                da.InsertCommand.Parameters.Add("@PhoneColumn", SqlDbType.int, 0, "PhoneColumn")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.int, 0, "UserID")
                da.InsertCommand.Parameters.Add("@DateUploaded", SqlDbType.datetime, 0, "DateUploaded")
                da.InsertCommand.Parameters.Add("@DateCompleted", SqlDbType.datetime, 0, "DateCompleted")
                da.InsertCommand.Parameters.Add("@DateDownloaded", SqlDbType.datetime, 0, "DateDownloaded")
                da.InsertCommand.Parameters.Add("@TemplateID", SqlDbType.int, 0, "TemplateID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FileID", SqlDbType.Int, 0, "FileID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadFiles").NewRow
            End If
            Update_Field("FileName", _FileName, dr)
            Update_Field("Status", _Status, dr)
            Update_Field("Header", _Header, dr)
            Update_Field("PhoneColumn", _PhoneColumn, dr)
            Update_Field("UserID", _UserID, dr)
            Update_Field("DateUploaded", _DateUploaded, dr)
            Update_Field("DateCompleted", _DateCompleted, dr)
            Update_Field("DateDownloaded", _DateDownloaded, dr)
            Update_Field("TemplateID", _TemplateID, dr)
            If ds.Tables("t_LeadFiles").Rows.Count < 1 Then ds.Tables("t_LeadFiles").Rows.Add(dr)
            da.Update(ds, "t_LeadFiles")
            _ID = ds.Tables("t_LeadFiles").Rows(0).Item("FileID")
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
            oEvents.CreatedByID = _UserDBID
            oEvents.KeyField = "FileID"
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

    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property

    Public Property Header() As Boolean
        Get
            Return _Header
        End Get
        Set(ByVal value As Boolean)
            _Header = value
        End Set
    End Property

    Public Property PhoneColumn() As Integer
        Get
            Return _PhoneColumn
        End Get
        Set(ByVal value As Integer)
            _PhoneColumn = value
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

    Public Property UserDBID() As Integer
        Get
            Return _UserDBID
        End Get
        Set(ByVal value As Integer)
            _UserDBID = value
        End Set
    End Property

    Public Property DateUploaded() As String
        Get
            Return _DateUploaded
        End Get
        Set(ByVal value As String)
            _DateUploaded = value
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

    Public Property DateDownloaded() As String
        Get
            Return _DateDownloaded
        End Get
        Set(ByVal value As String)
            _DateDownloaded = value
        End Set
    End Property

    Public Property TemplateID() As Integer
        Get
            Return _TemplateID
        End Get
        Set(ByVal value As Integer)
            _TemplateID = value
        End Set
    End Property

    Public Property FileID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
