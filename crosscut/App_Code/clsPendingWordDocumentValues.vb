Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPendingWordDocumentValues
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PendingWordDocumentID As Integer = 0
    Dim _BookMark As String = ""
    Dim _Value As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PendingWordDocumentValues where PendingWordDocumentValueID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PendingWordDocumentValues where PendingWordDocumentValueID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PendingWordDocumentValues")
            If ds.Tables("t_PendingWordDocumentValues").Rows.Count > 0 Then
                dr = ds.Tables("t_PendingWordDocumentValues").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PendingWordDocumentID") Is System.DBNull.Value) Then _PendingWordDocumentID = dr("PendingWordDocumentID")
        If Not (dr("BookMark") Is System.DBNull.Value) Then _BookMark = dr("BookMark")
        If Not (dr("Value") Is System.DBNull.Value) Then _Value = dr("Value")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PendingWordDocumentValues where PendingWordDocumentValueID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PendingWordDocumentValues")
            If ds.Tables("t_PendingWordDocumentValues").Rows.Count > 0 Then
                dr = ds.Tables("t_PendingWordDocumentValues").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PendingWordDocumentValuesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PendingWordDocumentID", SqlDbType.int, 0, "PendingWordDocumentID")
                da.InsertCommand.Parameters.Add("@BookMark", SqlDbType.varchar, 0, "BookMark")
                da.InsertCommand.Parameters.Add("@Value", SqlDbType.varchar, 0, "Value")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PendingWordDocumentValueID", SqlDbType.Int, 0, "PendingWordDocumentValueID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PendingWordDocumentValues").NewRow
            End If
            Update_Field("PendingWordDocumentID", _PendingWordDocumentID, dr)
            Update_Field("BookMark", _BookMark, dr)
            Update_Field("Value", _Value, dr)
            If ds.Tables("t_PendingWordDocumentValues").Rows.Count < 1 Then ds.Tables("t_PendingWordDocumentValues").Rows.Add(dr)
            da.Update(ds, "t_PendingWordDocumentValues")
            _ID = ds.Tables("t_PendingWordDocumentValues").Rows(0).Item("PendingWordDocumentValueID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "PendingWordDocumentValueID"
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

    Public Property PendingWordDocumentID() As Integer
        Get
            Return _PendingWordDocumentID
        End Get
        Set(ByVal value As Integer)
            _PendingWordDocumentID = value
        End Set
    End Property

    Public Property BookMark() As String
        Get
            Return _BookMark
        End Get
        Set(ByVal value As String)
            _BookMark = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Public Property PendingWordDocumentValueID() As Integer
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
