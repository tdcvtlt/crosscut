Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRefundRequest
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _Approved As Integer = 0
    Dim _ApprovedByID As Integer = 0
    Dim _DateApproved As String = ""
    Dim _RequestedByID As Integer = 0
    Dim _DateRequested As String = ""
    Dim _AccountID As Integer = 0
    Dim _CreditCardID As Integer = 0
    Dim _Reason As String = ""
    Dim _CCTransID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RefundRequest where RefundRequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RefundRequest where RefundRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RefundRequest")
            If ds.Tables("t_RefundRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_RefundRequest").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("Approved") Is System.DBNull.Value) Then _Approved = dr("Approved")
        If Not (dr("ApprovedByID") Is System.DBNull.Value) Then _ApprovedByID = dr("ApprovedByID")
        If Not (dr("DateApproved") Is System.DBNull.Value) Then _DateApproved = dr("DateApproved")
        If Not (dr("RequestedByID") Is System.DBNull.Value) Then _RequestedByID = dr("RequestedByID")
        If Not (dr("DateRequested") Is System.DBNull.Value) Then _DateRequested = dr("DateRequested")
        If Not (dr("AccountID") Is System.DBNull.Value) Then _AccountID = dr("AccountID")
        If Not (dr("CreditCardID") Is System.DBNull.Value) Then _CreditCardID = dr("CreditCardID")
        If Not (dr("Reason") Is System.DBNull.Value) Then _Reason = dr("Reason")
        If Not (dr("CCTransID") Is System.DBNull.Value) Then _CCTransID = dr("CCTransID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RefundRequest where RefundRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RefundRequest")
            If ds.Tables("t_RefundRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_RefundRequest").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RefundRequestInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@Approved", SqlDbType.Int, 0, "Approved")
                da.InsertCommand.Parameters.Add("@ApprovedByID", SqlDbType.int, 0, "ApprovedByID")
                da.InsertCommand.Parameters.Add("@DateApproved", SqlDbType.datetime, 0, "DateApproved")
                da.InsertCommand.Parameters.Add("@RequestedByID", SqlDbType.int, 0, "RequestedByID")
                da.InsertCommand.Parameters.Add("@DateRequested", SqlDbType.datetime, 0, "DateRequested")
                da.InsertCommand.Parameters.Add("@AccountID", SqlDbType.int, 0, "AccountID")
                da.InsertCommand.Parameters.Add("@CreditCardID", SqlDbType.Int, 0, "CreditCardID")
                da.InsertCommand.Parameters.Add("@Reason", SqlDbType.VarChar, 0, "Reason")
                da.InsertCommand.Parameters.Add("@CCTransID", SqlDbType.Int, 0, "CCTransID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RefundRequestID", SqlDbType.Int, 0, "RefundRequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RefundRequest").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("Approved", _Approved, dr)
            Update_Field("ApprovedByID", _ApprovedByID, dr)
            Update_Field("DateApproved", _DateApproved, dr)
            Update_Field("RequestedByID", _RequestedByID, dr)
            Update_Field("DateRequested", _DateRequested, dr)
            Update_Field("AccountID", _AccountID, dr)
            Update_Field("CreditCardID", _CreditCardID, dr)
            Update_Field("Reason", _Reason, dr)
            Update_Field("CCTransID", _CCTransID, dr)
            If ds.Tables("t_RefundRequest").Rows.Count < 1 Then ds.Tables("t_RefundRequest").Rows.Add(dr)
            da.Update(ds, "t_RefundRequest")
            _ID = ds.Tables("t_RefundRequest").Rows(0).Item("RefundRequestID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "RefundRequestID"
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

    Public Function Get_Pending_Requests(ByVal acctID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(r.RefundRequestID) As ID, p.LastName + ', ' + p.FirstName as Prospect, Case when i.KeyField = 'ContractID' then 'KCP#: ' + c.ContractNumber else i.KeyField + ': ' + Cast(i.KeyValue as varchar) end as Account, r.Amount, r.DateRequested, rb.FirstName + ' ' + rb.Lastname as RequestedBy, r.Reason from t_RefundRequest r inner join t_Prospect p on r.ProspectID = p.ProspectID inner join t_RefundRequestApplyTo ra on r.RefundRequestID = ra.RefundRequestID inner join t_Payments pmt on ra.PaymentID = pmt.PaymentID inner join t_Payment2Invoice pi on pmt.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID left outer join t_Contract c on i.KeyValue = c.ContractID left outer join t_Personnel rb on r.RequestedByID = rb.PersonnelID where r.AccountID = " & acctID & " and approved = 0 order by r.DateRequested asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
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

    Public Property Approved() As Integer
        Get
            Return _Approved
        End Get
        Set(ByVal value As Integer)
            _Approved = value
        End Set
    End Property

    Public Property ApprovedByID() As Integer
        Get
            Return _ApprovedByID
        End Get
        Set(ByVal value As Integer)
            _ApprovedByID = value
        End Set
    End Property

    Public Property DateApproved() As String
        Get
            Return _DateApproved
        End Get
        Set(ByVal value As String)
            _DateApproved = value
        End Set
    End Property

    Public Property RequestedByID() As Integer
        Get
            Return _RequestedByID
        End Get
        Set(ByVal value As Integer)
            _RequestedByID = value
        End Set
    End Property

    Public Property DateRequested() As String
        Get
            Return _DateRequested
        End Get
        Set(ByVal value As String)
            _DateRequested = value
        End Set
    End Property

    Public Property AccountID() As Integer
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Integer)
            _AccountID = value
        End Set
    End Property

    Public Property CreditCardID() As Integer
        Get
            Return _CreditCardID
        End Get
        Set(ByVal value As Integer)
            _CreditCardID = value
        End Set
    End Property

    Public Property RefundRequestID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
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

    Public Property Reason() As String
        Get
            Return _Reason
        End Get
        Set(ByVal value As String)
            _Reason = value
        End Set
    End Property


    Public Property CCTransID() As Integer
        Get
            Return _CCTransID
        End Get
        Set(ByVal value As Integer)
            _CCTransID = value
        End Set
    End Property
End Class
