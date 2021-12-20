Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsDID
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DIDNumber As String = ""
    Dim _CarrierID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection("data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        cm = New SqlCommand("Select * from t_DIDs where DID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_DIDs where DID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_DIDs")
            If ds.Tables("t_DIDs").Rows.Count > 0 Then
                dr = ds.Tables("t_DIDs").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DIDNumber") Is System.DBNull.Value) Then _DIDNumber = dr("DIDNumber")
        If Not (dr("CarrierID") Is System.DBNull.Value) Then _CarrierID = dr("CarrierID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DIDs where DID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_DIDs")
            If ds.Tables("t_DIDs").Rows.Count > 0 Then
                dr = ds.Tables("t_DIDs").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_DIDsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DIDNumber", SqlDbType.VarChar, 0, "DIDNumber")
                da.InsertCommand.Parameters.Add("@CarrierID", SqlDbType.Int, 0, "CarrierID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@DID", SqlDbType.Int, 0, "DID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_DIDs").NewRow
            End If
            Update_Field("DIDNumber", _DIDNumber, dr)
            Update_Field("CarrierID", _CarrierID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_DIDs").Rows.Count < 1 Then ds.Tables("t_DIDs").Rows.Add(dr)
            da.Update(ds, "t_DIDs")
            _ID = ds.Tables("t_DIDs").Rows(0).Item("DID")
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
            oEvents.KeyField = "DID"
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

    Public Property DIDNumber() As String
        Get
            Return _DIDNumber
        End Get
        Set(ByVal value As String)
            _DIDNumber = value
        End Set
    End Property

    Public Property CarrierID() As Integer
        Get
            Return _CarrierID
        End Get
        Set(ByVal value As Integer)
            _CarrierID = value
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

    Public ReadOnly Property DID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
