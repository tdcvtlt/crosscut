Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLF2CF
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LFInvoiceID As Integer = 0
    Dim _CFInvoiceID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LF2CF where LF2CFID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LF2CF where LF2CFID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LF2CF")
            If ds.Tables("t_LF2CF").Rows.Count > 0 Then
                dr = ds.Tables("t_LF2CF").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LFInvoiceID") Is System.DBNull.Value) Then _LFInvoiceID = dr("LFInvoiceID")
        If Not (dr("CFInvoiceID") Is System.DBNull.Value) Then _CFInvoiceID = dr("CFInvoiceID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LF2CF where LF2CFID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LF2CF")
            If ds.Tables("t_LF2CF").Rows.Count > 0 Then
                dr = ds.Tables("t_LF2CF").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LF2CFInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LFInvoiceID", SqlDbType.int, 0, "LFInvoiceID")
                da.InsertCommand.Parameters.Add("@CFInvoiceID", SqlDbType.int, 0, "CFInvoiceID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LF2CFID", SqlDbType.Int, 0, "LF2CFID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LF2CF").NewRow
            End If
            Update_Field("LFInvoiceID", _LFInvoiceID, dr)
            Update_Field("CFInvoiceID", _CFInvoiceID, dr)
            If ds.Tables("t_LF2CF").Rows.Count < 1 Then ds.Tables("t_LF2CF").Rows.Add(dr)
            da.Update(ds, "t_LF2CF")
            _ID = ds.Tables("t_LF2CF").Rows(0).Item("LF2CFID")
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
            oEvents.KeyField = "LF2CFID"
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

    Public Property LFInvoiceID() As Integer
        Get
            Return _LFInvoiceID
        End Get
        Set(ByVal value As Integer)
            _LFInvoiceID = value
        End Set
    End Property

    Public Property CFInvoiceID() As Integer
        Get
            Return _CFInvoiceID
        End Get
        Set(ByVal value As Integer)
            _CFInvoiceID = value
        End Set
    End Property

    Public Property LF2CFID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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

    Public Property Err As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

End Class
