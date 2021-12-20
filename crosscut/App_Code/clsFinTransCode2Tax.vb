Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Public Class clsFinTransCode2Tax
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FinTransCodeID As Integer = 0
    Dim _TaxID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_FinTransCode2Tax where FinTrans2TaxID = " & _ID, cn)
    End Sub
    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_FinTransCode2Tax where FinTrans2TaxID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_FinTransCode2Tax")
            If ds.Tables("t_FinTransCode2Tax").Rows.Count > 0 Then
                dr = ds.Tables("t_FinTransCode2Tax").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub
    Private Sub Set_Values()
        If Not (dr("FinTransCodeID") Is System.DBNull.Value) Then _FinTransCodeID = dr("FinTransCodeID")
        If Not (dr("TaxID") Is System.DBNull.Value) Then _TaxID = dr("TaxID")
    End Sub
    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_FinTransCode2Tax where FinTrans2TaxID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_FinTransCode2Tax")
            If ds.Tables("t_FinTransCode2Tax").Rows.Count > 0 Then
                dr = ds.Tables("t_FinTransCode2Tax").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_FinTransCode2TaxInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FinTransCodeID", SqlDbType.int, 0, "FinTransCodeID")
                da.InsertCommand.Parameters.Add("@TaxID", SqlDbType.int, 0, "TaxID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FinTrans2TaxID", SqlDbType.Int, 0, "FinTrans2TaxID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_FinTransCode2Tax").NewRow
            End If
            Update_Field("FinTransCodeID", _FinTransCodeID, dr)
            Update_Field("TaxID", _TaxID, dr)
            If ds.Tables("t_FinTransCode2Tax").Rows.Count < 1 Then ds.Tables("t_FinTransCode2Tax").Rows.Add(dr)
            da.Update(ds, "t_FinTransCode2Tax")
            _ID = ds.Tables("t_FinTransCode2Tax").Rows(0).Item("FinTrans2TaxID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "FinTrans2TaxID"
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
    Public Property FinTransCodeID() As Integer
        Get
            Return _FinTransCodeID
        End Get
        Set(ByVal value As Integer)
            _FinTransCodeID = value
        End Set
    End Property
    Public Property TaxID() As Integer
        Get
            Return _TaxID
        End Get
        Set(ByVal value As Integer)
            _TaxID = value
        End Set
    End Property
    Public ReadOnly Property FinTrans2TaxID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
