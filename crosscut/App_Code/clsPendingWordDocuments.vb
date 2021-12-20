Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPendingWordDocuments
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TemplateFile As String = ""
    Dim _SaveFile As String = ""
    Dim _Completed As Boolean = False
    Dim _DateCompleted As String = ""
    Dim _Ready As Boolean = False
    Dim _FileName As String = ""
    Dim _SessionID As String = ""
    Dim _Printer As String = ""
    Dim _DateRequested As String = ""
    Dim _ErrorFlag As Boolean = False
    Dim _ErrorDesc As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PendingWordDocuments where PendingWordDocumentID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PendingWordDocuments where PendingWordDocumentID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PendingWordDocuments")
            If ds.Tables("t_PendingWordDocuments").Rows.Count > 0 Then
                dr = ds.Tables("t_PendingWordDocuments").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("TemplateFile") Is System.DBNull.Value) Then _TemplateFile = dr("TemplateFile")
        If Not (dr("SaveFile") Is System.DBNull.Value) Then _SaveFile = dr("SaveFile")
        If Not (dr("Completed") Is System.DBNull.Value) Then _Completed = dr("Completed")
        If Not (dr("DateCompleted") Is System.DBNull.Value) Then _DateCompleted = dr("DateCompleted")
        If Not (dr("Ready") Is System.DBNull.Value) Then _Ready = dr("Ready")
        If Not (dr("FileName") Is System.DBNull.Value) Then _FileName = dr("FileName")
        If Not (dr("SessionID") Is System.DBNull.Value) Then _SessionID = dr("SessionID")
        If Not (dr("Printer") Is System.DBNull.Value) Then _Printer = dr("Printer")
        If Not (dr("DateRequested") Is System.DBNull.Value) Then _DateRequested = dr("DateRequested")
        If Not (dr("ErrorFlag") Is System.DBNull.Value) Then _ErrorFlag = dr("ErrorFlag")
        If Not (dr("ErrorDesc") Is System.DBNull.Value) Then _ErrorDesc = dr("ErrorDesc")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PendingWordDocuments where PendingWordDocumentID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PendingWordDocuments")
            If ds.Tables("t_PendingWordDocuments").Rows.Count > 0 Then
                dr = ds.Tables("t_PendingWordDocuments").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PendingWordDocumentsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TemplateFile", SqlDbType.VarChar, 255, "TemplateFile")
                da.InsertCommand.Parameters.Add("@SaveFile", SqlDbType.VarChar, 255, "SaveFile")
                da.InsertCommand.Parameters.Add("@Completed", SqlDbType.bit, 0, "Completed")
                da.InsertCommand.Parameters.Add("@DateCompleted", SqlDbType.datetime, 0, "DateCompleted")
                da.InsertCommand.Parameters.Add("@Ready", SqlDbType.bit, 0, "Ready")
                da.InsertCommand.Parameters.Add("@FileName", SqlDbType.VarChar, 255, "FileName")
                da.InsertCommand.Parameters.Add("@SessionID", SqlDbType.VarChar, 255, "SessionID")
                da.InsertCommand.Parameters.Add("@Printer", SqlDbType.VarChar, 255, "Printer")
                da.InsertCommand.Parameters.Add("@DateRequested", SqlDbType.datetime, 0, "DateRequested")
                da.InsertCommand.Parameters.Add("@ErrorFlag", SqlDbType.bit, 0, "ErrorFlag")
                da.InsertCommand.Parameters.Add("@ErrorDesc", SqlDbType.text, 0, "ErrorDesc")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PendingWordDocumentID", SqlDbType.Int, 0, "PendingWordDocumentID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PendingWordDocuments").NewRow
            End If
            Update_Field("TemplateFile", _TemplateFile, dr)
            Update_Field("SaveFile", _SaveFile, dr)
            Update_Field("Completed", _Completed, dr)
            Update_Field("DateCompleted", _DateCompleted, dr)
            Update_Field("Ready", _Ready, dr)
            Update_Field("FileName", _FileName, dr)
            Update_Field("SessionID", _SessionID, dr)
            Update_Field("Printer", _Printer, dr)
            Update_Field("DateRequested", _DateRequested, dr)
            Update_Field("ErrorFlag", _ErrorFlag, dr)
            Update_Field("ErrorDesc", _ErrorDesc, dr)
            If ds.Tables("t_PendingWordDocuments").Rows.Count < 1 Then ds.Tables("t_PendingWordDocuments").Rows.Add(dr)
            da.Update(ds, "t_PendingWordDocuments")
            _ID = ds.Tables("t_PendingWordDocuments").Rows(0).Item("PendingWordDocumentID")
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
            oEvents.KeyField = "PendingWordDocumentID"
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

    Public Property TemplateFile() As String
        Get
            Return _TemplateFile
        End Get
        Set(ByVal value As String)
            _TemplateFile = value
        End Set
    End Property

    Public Property SaveFile() As String
        Get
            Return _SaveFile
        End Get
        Set(ByVal value As String)
            _SaveFile = value
        End Set
    End Property

    Public Property Completed() As Boolean
        Get
            Return _Completed
        End Get
        Set(ByVal value As Boolean)
            _Completed = value
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

    Public Property Ready() As Boolean
        Get
            Return _Ready
        End Get
        Set(ByVal value As Boolean)
            _Ready = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Public Property SessionID() As String
        Get
            Return _SessionID
        End Get
        Set(ByVal value As String)
            _SessionID = value
        End Set
    End Property

    Public Property Printer() As String
        Get
            Return _Printer
        End Get
        Set(ByVal value As String)
            _Printer = value
        End Set
    End Property

    Public Property DateRequested() As String
        Get
            Return _DateRequested
        End Get
        Set(ByVal value As String)
            _DateRequested = value
        End Set
    End Property

    Public Property ErrorFlag() As Boolean
        Get
            Return _ErrorFlag
        End Get
        Set(ByVal value As Boolean)
            _ErrorFlag = value
        End Set
    End Property

    Public Property ErrorDesc() As String
        Get
            Return _ErrorDesc
        End Get
        Set(ByVal value As String)
            _ErrorDesc = value
        End Set
    End Property

    Public Property PendingWordDocumentID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
