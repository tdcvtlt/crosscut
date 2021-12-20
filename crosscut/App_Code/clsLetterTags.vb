Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLetterTags
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Tag As String = ""
    Dim _TypeID As Integer = 0
    Dim _ViewID As Integer = 0
    Dim _FieldName As String = ""
    Dim _KeyFieldName As String = ""
    Dim _KeyFieldValue As String = ""
    Dim _TagStyleID As Integer = 0
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LetterTags where TagID = " & _ID, cn)
    End Sub

    Public Function List(ByVal TypeID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select Tag, Description from t_LetterTags where typeid = " & TypeID)
    End Function

    Public Function List_Setup(ByVal TypeID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LetterTags where typeid = " & TypeID)
    End Function


    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LetterTags where TagID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LetterTags")
            If ds.Tables("t_LetterTags").Rows.Count > 0 Then
                dr = ds.Tables("t_LetterTags").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Tag") Is System.DBNull.Value) Then _Tag = dr("Tag")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("ViewID") Is System.DBNull.Value) Then _ViewID = dr("ViewID")
        If Not (dr("FieldName") Is System.DBNull.Value) Then _FieldName = dr("FieldName")
        If Not (dr("KeyFieldName") Is System.DBNull.Value) Then _KeyFieldName = dr("KeyFieldName")
        If Not (dr("KeyFieldValue") Is System.DBNull.Value) Then _KeyFieldValue = dr("KeyFieldValue")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("TagStyleID") Is System.DBNull.Value) Then _TagStyleID = dr("TagStyleID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LetterTags where TagID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LetterTags")
            If ds.Tables("t_LetterTags").Rows.Count > 0 Then
                dr = ds.Tables("t_LetterTags").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LetterTagsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Tag", SqlDbType.VarChar, 0, "Tag")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@ViewID", SqlDbType.Int, 0, "ViewID")
                da.InsertCommand.Parameters.Add("@FieldName", SqlDbType.VarChar, 0, "FieldName")
                da.InsertCommand.Parameters.Add("@KeyFieldName", SqlDbType.VarChar, 0, "KeyFieldName")
                da.InsertCommand.Parameters.Add("@KeyFieldValue", SqlDbType.VarChar, 0, "KeyFieldValue")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.Text, 0, "Description")
                da.InsertCommand.Parameters.Add("@TagStyleID", SqlDbType.Int, 0, "TagStyleID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TagID", SqlDbType.Int, 0, "TagID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LetterTags").NewRow
            End If
            Update_Field("Tag", _Tag, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("ViewID", _ViewID, dr)
            Update_Field("FieldName", _FieldName, dr)
            Update_Field("KeyFieldName", _KeyFieldName, dr)
            Update_Field("KeyFieldValue", _KeyFieldValue, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("TagStyleID", _TagStyleID, dr)
            If ds.Tables("t_LetterTags").Rows.Count < 1 Then ds.Tables("t_LetterTags").Rows.Add(dr)
            da.Update(ds, "t_LetterTags")
            _ID = ds.Tables("t_LetterTags").Rows(0).Item("TagID")
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
            oEvents.KeyField = "TagID"
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

    Public Property Tag() As String
        Get
            Return _Tag
        End Get
        Set(ByVal value As String)
            _Tag = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property TagStyleID As Integer
        Get
            Return _TagStyleID
        End Get
        Set(ByVal value As Integer)
            _TagStyleID = value
        End Set
    End Property

    Public Property ViewID() As Integer
        Get
            Return _ViewID
        End Get
        Set(ByVal value As Integer)
            _ViewID = value
        End Set
    End Property

    Public Property FieldName() As String
        Get
            Return _FieldName
        End Get
        Set(ByVal value As String)
            _FieldName = value
        End Set
    End Property

    Public Property KeyFieldName() As String
        Get
            Return _KeyFieldName
        End Get
        Set(ByVal value As String)
            _KeyFieldName = value
        End Set
    End Property

    Public Property KeyFieldValue() As String
        Get
            Return _KeyFieldValue
        End Get
        Set(ByVal value As String)
            _KeyFieldValue = value
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

    Public Property TagID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
