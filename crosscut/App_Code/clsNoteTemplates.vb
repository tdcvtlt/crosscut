Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsNoteTemplates
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Note As String = ""
    Dim _UserDBID As Integer = 0
    Dim _Title As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_NoteTemplates where NoteTemplateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_NoteTemplates where NoteTemplateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_NoteTemplates")
            If ds.Tables("t_NoteTemplates").Rows.Count > 0 Then
                dr = ds.Tables("t_NoteTemplates").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(UserID As Integer, ActiveOnly As Boolean, ForDropDown As Boolean) As SqlDataSource
        Dim ds As New SqlDataSource(Resources.Resource.cns, If(ForDropDown, "select 0 as NoteTemplateID, 'Select' as Title union ", "") & "Select " & If(ForDropDown, "NoteTemplateID, Title", "*") & " from t_NoteTemplates where Active in (" & If(ActiveOnly, "1", "1,0") & ")  and UserID = " & UserID & " order by Title")
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("NoteTemplateID") Is System.DBNull.Value) Then _ID = dr("NoteTemplateID")
        If Not (dr("Note") Is System.DBNull.Value) Then _Note = dr("Note")
        If Not (dr("UserID") Is System.DBNull.Value) Then _UserDBID = dr("UserID")
        If Not (dr("Title") Is System.DBNull.Value) Then _Title = dr("Title")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_NoteTemplates where NoteTemplateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_NoteTemplates")
            If ds.Tables("t_NoteTemplates").Rows.Count > 0 Then
                dr = ds.Tables("t_NoteTemplates").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_NoteTemplatesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Note", SqlDbType.Text, 0, "Note")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.Int, 0, "UserID")
                da.InsertCommand.Parameters.Add("@Title", SqlDbType.VarChar, 0, "Title")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@NoteTemplateID", SqlDbType.Int, 0, "NoteTemplateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_NoteTemplates").NewRow
            End If
            Update_Field("Note", _Note, dr)
            Update_Field("UserID", _UserDBID, dr)
            Update_Field("Title", _Title, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_NoteTemplates").Rows.Count < 1 Then ds.Tables("t_NoteTemplates").Rows.Add(dr)
            da.Update(ds, "t_NoteTemplates")
            _ID = ds.Tables("t_NoteTemplates").Rows(0).Item("NoteTemplateID")
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
            oEvents.KeyField = "NoteTemplateID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Note() As String
        Get
            Return _Note
        End Get
        Set(ByVal value As String)
            _Note = value
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

    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
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

    Public Property NoteTemplateID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Error_Message As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
