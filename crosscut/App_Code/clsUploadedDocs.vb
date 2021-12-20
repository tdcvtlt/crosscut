Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsUploadedDocs
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Path As String = ""
    Dim _UploadedByID As Integer = 0
    Dim _DateUploaded As String = ""
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _ContentText As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_UploadedDocs where FileID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_UploadedDocs where FileID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_UploadedDocs")
            If ds.Tables("t_UploadedDocs").Rows.Count > 0 Then
                dr = ds.Tables("t_UploadedDocs").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Path") Is System.DBNull.Value) Then _Path = dr("Path")
        If Not (dr("UploadedByID") Is System.DBNull.Value) Then _UploadedByID = dr("UploadedByID")
        If Not (dr("DateUploaded") Is System.DBNull.Value) Then _DateUploaded = dr("DateUploaded")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
        If Not (dr("ContentText") Is System.DBNull.Value) Then _ContentText = dr("ContentText")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_UploadedDocs where FileID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_UploadedDocs")
            If ds.Tables("t_UploadedDocs").Rows.Count > 0 Then
                dr = ds.Tables("t_UploadedDocs").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_UploadedDocsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Path", SqlDbType.varchar, 0, "Path")
                da.InsertCommand.Parameters.Add("@UploadedByID", SqlDbType.int, 0, "UploadedByID")
                da.InsertCommand.Parameters.Add("@DateUploaded", SqlDbType.datetime, 0, "DateUploaded")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.Int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@ContentText", SqlDbType.Text, 0, "ContentText")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FileID", SqlDbType.Int, 0, "FileID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_UploadedDocs").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Path", _Path, dr)
            Update_Field("UploadedByID", _UploadedByID, dr)
            Update_Field("DateUploaded", _DateUploaded, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("ContentText", _ContentText, dr)
            If ds.Tables("t_UploadedDocs").Rows.Count < 1 Then ds.Tables("t_UploadedDocs").Rows.Add(dr)
            da.Update(ds, "t_UploadedDocs")
            _ID = ds.Tables("t_UploadedDocs").Rows(0).Item("FileID")
            If cn.State <> ConnectionState.Closed then cn.Close
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

    Public Function Copy_Docs(ByVal oKF As String, ByVal oKV As Integer, ByVal nKF As String, ByVal nKV As Integer) As Boolean
        Dim bCopy As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Insert into t_UploadedDocs (Name, Path, UploadedByID, DateUploaded, KeyField, KeyValue) Select Name, Path, " & _UserID & " as UploadedByID, '" & System.DateTime.Now & "' as DateUploaded, '" & nKF & "' as KeyField, " & nKV & " as KeyValue from t_UploadedDocs where keyfield = '" & oKF & "' and keyvalue = " & oKV
            cm.ExecuteNonQuery()
            If cn.State <> ConnectionState.Closed Then cn.Close()
            bCopy = True
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return bCopy
    End Function
#Region "Properties"
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
        End Set
    End Property

    Public Property UploadedByID() As Integer
        Get
            Return _UploadedByID
        End Get
        Set(ByVal value As Integer)
            _UploadedByID = value
        End Set
    End Property

    Public Property ContentText As String
        Get
            Return _ContentText
        End Get
        Set(ByVal value As String)
            _ContentText = value
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

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
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

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
#End Region
End Class
