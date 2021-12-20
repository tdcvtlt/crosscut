Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRefundRequestApplyTo
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RefundRequestID As Integer = 0
    Dim _PaymentID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RefundRequestApplyTo where RefundApplyID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RefundRequestApplyTo where RefundApplyID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RefundRequestApplyTo")
            If ds.Tables("t_RefundRequestApplyTo").Rows.Count > 0 Then
                dr = ds.Tables("t_RefundRequestApplyTo").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RefundRequestID") Is System.DBNull.Value) Then _RefundRequestID = dr("RefundRequestID")
        If Not (dr("PaymentID") Is System.DBNull.Value) Then _PaymentID = dr("PaymentID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RefundRequestApplyTo where RefundApplyID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RefundRequestApplyTo")
            If ds.Tables("t_RefundRequestApplyTo").Rows.Count > 0 Then
                dr = ds.Tables("t_RefundRequestApplyTo").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RefundRequestApplyToInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RefundRequestID", SqlDbType.int, 0, "RefundRequestID")
                da.InsertCommand.Parameters.Add("@PaymentID", SqlDbType.int, 0, "PaymentID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RefundApplyID", SqlDbType.Int, 0, "RefundApplyID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RefundRequestApplyTo").NewRow
            End If
            Update_Field("RefundRequestID", _RefundRequestID, dr)
            Update_Field("PaymentID", _PaymentID, dr)
            Update_Field("Amount", _Amount, dr)
            If ds.Tables("t_RefundRequestApplyTo").Rows.Count < 1 Then ds.Tables("t_RefundRequestApplyTo").Rows.Add(dr)
            da.Update(ds, "t_RefundRequestApplyTo")
            _ID = ds.Tables("t_RefundRequestApplyTo").Rows(0).Item("RefundApplyID")
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
            oEvents.KeyField = "RefundApplyID"
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

    Public Function List_ApplyTo_Items(ByVal refundID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select pm.ComboItem As Payment, ra.Amount, p.transdate As Date, tc.ComboItem as Invoice from t_RefundRequestApplyTo ra inner join t_Payments p on ra.PaymentID = p.PaymentID inner join t_Payment2Invoice pi on pi.PaymentID = p.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID inner join t_COmboItems pm on p.MethodID = pm.ComboItemID where ra.RefundRequestID = " & refundID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Ref_Amount_Per_Payment(ByVal paymentID As Integer) As Double
        Dim refAmt As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            'cm.CommandText = "Select Sum(ra.Amount) as RefAmt from t_RefundRequestApplyTo ra inner join t_RefundRequest rr on ra.RefundRequestID = rr.RefundRequestID where rr.Approved = 1 and ra.PaymentID = " & paymentID
            '***** Updated to fix for declined refunds 3/14/13 RH *****'
            'cm.CommandText = "Select Sum(ra.Amount) as RefAmt from t_RefundRequestApplyTo ra inner join t_RefundRequest rr on ra.RefundRequestID = rr.RefundRequestID left outer join t_CCTrans cc on cc.creditcardid = rr.creditcardid left outer join t_Comboitems tt on tt.comboitemid= cc.transtypeid where rr.Approved = 1 and (cc.cctransid is null or (tt.comboitem in ('Refund') and cc.icvresponse not like 'n%')) and ra.PaymentID =" & paymentID
            '***** Updated to fix for declined refunds 3/22/13 MB *****'
            '***** Added CCTransID link to RefundRequest to check icvresponse of that payment
            cm.CommandText = "Select Sum(ra.Amount) as RefAmt from t_RefundRequestApplyTo ra inner join t_RefundRequest rr on ra.RefundRequestID = rr.RefundRequestID left outer join t_CCTrans cc on rr.CCTransID = cc.CCTransID where ra.PaymentID = " & paymentID & " and (cc.ICVResponse is Null or cc.ICVResponse not like 'N%') and rr.Approved <> -1"

            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                refAmt = dread("RefAmt")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return refAmt
    End Function

    Public Property RefundRequestID() As Integer
        Get
            Return _RefundRequestID
        End Get
        Set(ByVal value As Integer)
            _RefundRequestID = value
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

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Public Property RefundApplyID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
