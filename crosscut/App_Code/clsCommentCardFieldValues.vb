Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCommentCardFieldValues
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FieldID As Integer = 0
    Dim _CardID As Integer = 0
    Dim _MaintReq As Boolean = False
    Dim _ValueName As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CommentCardFieldValues where ValueID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CommentCardFieldValues where ValueID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CommentCardFieldValues")
            If ds.Tables("t_CommentCardFieldValues").Rows.Count > 0 Then
                dr = ds.Tables("t_CommentCardFieldValues").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FieldID") Is System.DBNull.Value) Then _FieldID = dr("FieldID")
        If Not (dr("CardID") Is System.DBNull.Value) Then _CardID = dr("CardID")
        If Not (dr("MaintReq") Is System.DBNull.Value) Then _MaintReq = dr("MaintReq")
        If Not (dr("ValueName") Is System.DBNull.Value) Then _ValueName = dr("ValueName")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CommentCardFieldValues where ValueID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CommentCardFieldValues")
            If ds.Tables("t_CommentCardFieldValues").Rows.Count > 0 Then
                dr = ds.Tables("t_CommentCardFieldValues").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CommentCardFieldValuesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FieldID", SqlDbType.int, 0, "FieldID")
                da.InsertCommand.Parameters.Add("@CardID", SqlDbType.int, 0, "CardID")
                da.InsertCommand.Parameters.Add("@MaintReq", SqlDbType.bit, 0, "MaintReq")
                da.InsertCommand.Parameters.Add("@ValueName", SqlDbType.varchar, 0, "ValueName")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ValueID", SqlDbType.Int, 0, "ValueID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CommentCardFieldValues").NewRow
            End If
            Update_Field("FieldID", _FieldID, dr)
            Update_Field("CardID", _CardID, dr)
            Update_Field("MaintReq", _MaintReq, dr)
            Update_Field("ValueName", _ValueName, dr)
            If ds.Tables("t_CommentCardFieldValues").Rows.Count < 1 Then ds.Tables("t_CommentCardFieldValues").Rows.Add(dr)
            da.Update(ds, "t_CommentCardFieldValues")
            _ID = ds.Tables("t_CommentCardFieldValues").Rows(0).Item("ValueID")
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
            oEvents.KeyField = "ValueID"
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

    Public Property FieldID() As Integer
        Get
            Return _FieldID
        End Get
        Set(ByVal value As Integer)
            _FieldID = value
        End Set
    End Property

    Public Property CardID() As Integer
        Get
            Return _CardID
        End Get
        Set(ByVal value As Integer)
            _CardID = value
        End Set
    End Property

    Public Property MaintReq() As Boolean
        Get
            Return _MaintReq
        End Get
        Set(ByVal value As Boolean)
            _MaintReq = value
        End Set
    End Property

    Public Property ValueName() As String
        Get
            Return _ValueName
        End Get
        Set(ByVal value As String)
            _ValueName = value
        End Set
    End Property

    Public ReadOnly Property ValueID() As Integer
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
