Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAFC2CRMS
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AFCID As Integer = 0
    Dim _InvoiceID As Integer = 0
    Dim _PaymentID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_AFC2CRMS where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_AFC2CRMS where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_AFC2CRMS")
            If ds.Tables("t_AFC2CRMS").Rows.Count > 0 Then
                dr = ds.Tables("t_AFC2CRMS").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AFCID") Is System.DBNull.Value) Then _AFCID = dr("AFCID")
        If Not (dr("InvoiceID") Is System.DBNull.Value) Then _InvoiceID = dr("InvoiceID")
        If Not (dr("PaymentID") Is System.DBNull.Value) Then _PaymentID = dr("PaymentID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_AFC2CRMS where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_AFC2CRMS")
            If ds.Tables("t_AFC2CRMS").Rows.Count > 0 Then
                dr = ds.Tables("t_AFC2CRMS").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_AFC2CRMSInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AFCID", SqlDbType.int, 0, "AFCID")
                da.InsertCommand.Parameters.Add("@InvoiceID", SqlDbType.int, 0, "InvoiceID")
                da.InsertCommand.Parameters.Add("@PaymentID", SqlDbType.int, 0, "PaymentID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_AFC2CRMS").NewRow
            End If
            Update_Field("AFCID", _AFCID, dr)
            Update_Field("InvoiceID", _InvoiceID, dr)
            Update_Field("PaymentID", _PaymentID, dr)
            If ds.Tables("t_AFC2CRMS").Rows.Count < 1 Then ds.Tables("t_AFC2CRMS").Rows.Add(dr)
            da.Update(ds, "t_AFC2CRMS")
            _ID = ds.Tables("t_AFC2CRMS").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
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

    Public Property AFCID() As Integer
        Get
            Return _AFCID
        End Get
        Set(ByVal value As Integer)
            _AFCID = value
        End Set
    End Property

    Public Property InvoiceID() As Integer
        Get
            Return _InvoiceID
        End Get
        Set(ByVal value As Integer)
            _InvoiceID = value
        End Set
    End Property

    Public Property PaymentID() As Integer
        Get
            Return _PaymentID
        End Get
        Set(ByVal value As Integer)
            _PaymentID = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Err As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

End Class
