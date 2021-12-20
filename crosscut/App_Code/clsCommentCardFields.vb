Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCommentCardFields
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FieldName As String = ""
    Dim _Description As String = ""
    Dim _DateAdded As String = ""
    Dim _AddedByID As Integer = 0
    Dim _GeneratesWorkOrder As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CommentCardFields where FieldID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CommentCardFields where FieldID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CommentCardFields")
            If ds.Tables("t_CommentCardFields").Rows.Count > 0 Then
                dr = ds.Tables("t_CommentCardFields").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FieldName") Is System.DBNull.Value) Then _FieldName = dr("FieldName")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("AddedByID") Is System.DBNull.Value) Then _AddedByID = dr("AddedByID")
        If Not (dr("GeneratesWorkOrder") Is System.DBNull.Value) Then _GeneratesWorkOrder = dr("GeneratesWorkOrder")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CommentCardFields where FieldID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CommentCardFields")
            If ds.Tables("t_CommentCardFields").Rows.Count > 0 Then
                dr = ds.Tables("t_CommentCardFields").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CommentCardFieldsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FieldName", SqlDbType.varchar, 0, "FieldName")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.datetime, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@AddedByID", SqlDbType.int, 0, "AddedByID")
                da.InsertCommand.Parameters.Add("@GeneratesWorkOrder", SqlDbType.bit, 0, "GeneratesWorkOrder")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FieldID", SqlDbType.Int, 0, "FieldID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CommentCardFields").NewRow
            End If
            Update_Field("FieldName", _FieldName, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("AddedByID", _AddedByID, dr)
            Update_Field("GeneratesWorkOrder", _GeneratesWorkOrder, dr)
            If ds.Tables("t_CommentCardFields").Rows.Count < 1 Then ds.Tables("t_CommentCardFields").Rows.Add(dr)
            da.Update(ds, "t_CommentCardFields")
            _ID = ds.Tables("t_CommentCardFields").Rows(0).Item("FieldID")
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
            oEvents.KeyField = "FieldID"
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

    Public Property FieldName() As String
        Get
            Return _FieldName
        End Get
        Set(ByVal value As String)
            _FieldName = value
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

    Public Property DateAdded() As String
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property AddedByID() As Integer
        Get
            Return _AddedByID
        End Get
        Set(ByVal value As Integer)
            _AddedByID = value
        End Set
    End Property

    Public Property GeneratesWorkOrder() As Boolean
        Get
            Return _GeneratesWorkOrder
        End Get
        Set(ByVal value As Boolean)
            _GeneratesWorkOrder = value
        End Set
    End Property

    Public ReadOnly Property FieldID() As Integer
        Get
            Return _ID
        End Get
    End Property


    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
