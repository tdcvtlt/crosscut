Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsComments
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _Comment As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _CreatedBy As String = ""
    Dim _CreatedDate As String = ""
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Comments where CommentID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select c.*, p.username from t_Comments c left outer join t_Personnel p on p.personnelid = c.createdbyid where CommentID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Comments")
            If ds.Tables("t_Comments").Rows.Count > 0 Then
                dr = ds.Tables("t_Comments").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
        If Not (dr("Comment") Is System.DBNull.Value) Then _Comment = dr("Comment")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("CreatedDate") Is System.DBNull.Value) Then _CreatedDate = dr("CreatedDate")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("username") Is System.DBNull.Value) Then _CreatedBy = dr("UserName")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Comments where CommentID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Comments")
            If ds.Tables("t_Comments").Rows.Count > 0 Then
                dr = ds.Tables("t_Comments").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CommentsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@Comment", SqlDbType.text, 0, "Comment")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@CreatedDate", SqlDbType.datetime, 0, "CreatedDate")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CommentID", SqlDbType.Int, 0, "CommentID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Comments").NewRow
            End If
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("Comment", _Comment, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("CreatedDate", _CreatedDate, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Comments").Rows.Count < 1 Then ds.Tables("t_Comments").Rows.Add(dr)
            da.Update(ds, "t_Comments")
            _ID = ds.Tables("t_Comments").Rows(0).Item("CommentID")
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
            oEvents.KeyField = "CommentID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Comments(ByVal KeyField As String, ByVal KeyValue As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.UserName, c.Comment, c.CreatedDate as Date from t_Comments c inner join t_Personnel p on c.CreatedByID = p.PersonnelID where c.keyField = '" & KeyField & "' and c.keyValue = '" & KeyValue & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function


    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

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

    Public Property Comment() As String
        Get
            Return _Comment
        End Get
        Set(ByVal value As String)
            _Comment = value
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

    Public Property CreatedDate() As String
        Get
            Return _CreatedDate
        End Get
        Set(ByVal value As String)
            _CreatedDate = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property CommentID() As Integer
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

    Public ReadOnly Property CreatedBy As String
        Get
            Return _CreatedBy
        End Get
    End Property
End Class
