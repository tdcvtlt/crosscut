Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPaymentMethod2Adjustment
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PaymentMethodID As Integer = 0
    Dim _Permissions As Boolean = False
    Dim _InvoiceAdj As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PaymentMethod2Adjustment where PaymentMethod2AdjustmentID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PaymentMethod2Adjustment where PaymentMethod2AdjustmentID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PaymentMethod2Adjustment")
            If ds.Tables("t_PaymentMethod2Adjustment").Rows.Count > 0 Then
                dr = ds.Tables("t_PaymentMethod2Adjustment").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PaymentMethodID") Is System.DBNull.Value) Then _PaymentMethodID = dr("PaymentMethodID")
        If Not (dr("Permissions") Is System.DBNull.Value) Then _Permissions = dr("Permissions")
        If Not (dr("InvoiceAdj") Is System.DBNull.Value) Then _InvoiceAdj = dr("InvoiceAdj")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PaymentMethod2Adjustment where PaymentMethod2AdjustmentID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PaymentMethod2Adjustment")
            If ds.Tables("t_PaymentMethod2Adjustment").Rows.Count > 0 Then
                dr = ds.Tables("t_PaymentMethod2Adjustment").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PaymentMethod2AdjustmentInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PaymentMethodID", SqlDbType.int, 0, "PaymentMethodID")
                da.InsertCommand.Parameters.Add("@Permissions", SqlDbType.bit, 0, "Permissions")
                da.InsertCommand.Parameters.Add("@InvoiceAdj", SqlDbType.bit, 0, "InvoiceAdj")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PaymentMethod2AdjustmentID", SqlDbType.Int, 0, "PaymentMethod2AdjustmentID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PaymentMethod2Adjustment").NewRow
            End If
            Update_Field("PaymentMethodID", _PaymentMethodID, dr)
            Update_Field("Permissions", _Permissions, dr)
            Update_Field("InvoiceAdj", _InvoiceAdj, dr)
            If ds.Tables("t_PaymentMethod2Adjustment").Rows.Count < 1 Then ds.Tables("t_PaymentMethod2Adjustment").Rows.Add(dr)
            da.Update(ds, "t_PaymentMethod2Adjustment")
            _ID = ds.Tables("t_PaymentMethod2Adjustment").Rows(0).Item("PaymentMethod2AdjustmentID")
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
            oEvents.KeyField = "PaymentMethod2AdjustmentID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Sort_Adjustments(ByVal invFlag As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.PaymentMethodID as ID, c.ComboItem as Adjustment from t_PaymentMethod2Adjustment p inner join t_ComboItems c on p.PaymentMethodID = c.ComboItemID where p.InvoiceAdj = '" & invFlag & "' and c.Active = '1' order by c.ComboItem asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Require_Permissions(ByVal adj As Integer, ByVal invAdj As Integer) As Boolean
        Dim bPermissions As Boolean = True
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select Permissions from t_PaymentMethod2Adjustment where paymentmethodid = " & adj & " and InvoiceAdj = " & invAdj
        dread = cm.ExecuteReader()
        dread.Read()
        If dread("Permissions") = 1 Or dread("Permissions") = True Then
            bPermissions = True
        Else
            bPermissions = False
        End If
        dread.Close()
        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bPermissions
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PaymentMethodID() As Integer
        Get
            Return _PaymentMethodID
        End Get
        Set(ByVal value As Integer)
            _PaymentMethodID = value
        End Set
    End Property

    Public Property Permissions() As Boolean
        Get
            Return _Permissions
        End Get
        Set(ByVal value As Boolean)
            _Permissions = value
        End Set
    End Property

    Public Property InvoiceAdj() As Boolean
        Get
            Return _InvoiceAdj
        End Get
        Set(ByVal value As Boolean)
            _InvoiceAdj = value
        End Set
    End Property

    Public Property PaymentMethod2AdjustmentID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
