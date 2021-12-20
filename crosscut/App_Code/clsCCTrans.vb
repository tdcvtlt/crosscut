Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCCTrans
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TransTypeID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _AccountID As Integer = 0
    Dim _Imported As Boolean = False
    Dim _ICVResponse As String = ""
    Dim _DateImported As String = ""
    Dim _Approved As Integer = 0
    Dim _ApprovedBy As String = ""
    Dim _DateApproved As String = ""
    Dim _ApplyTo As String = ""
    Dim _CreditCardID As Integer = 0
    Dim _ClientIP As String = ""
    Dim _Applied As Boolean = False
    Dim _DateCreated As String = ""
    Dim _BatchID As Integer = 0
    Dim _PreAuthID As Integer = 0
    Dim _CreatedByID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Token As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CCTrans where CCTransID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CCTrans where CCTransID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CCTrans")
            If ds.Tables("t_CCTrans").Rows.Count > 0 Then
                dr = ds.Tables("t_CCTrans").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("TransTypeID") Is System.DBNull.Value) Then _TransTypeID = dr("TransTypeID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("AccountID") Is System.DBNull.Value) Then _AccountID = dr("AccountID")
        If Not (dr("Imported") Is System.DBNull.Value) Then _Imported = dr("Imported")
        If Not (dr("ICVResponse") Is System.DBNull.Value) Then _ICVResponse = dr("ICVResponse")
        If Not (dr("DateImported") Is System.DBNull.Value) Then _DateImported = dr("DateImported")
        If Not (dr("Approved") Is System.DBNull.Value) Then _Approved = dr("Approved")
        If Not (dr("ApprovedBy") Is System.DBNull.Value) Then _ApprovedBy = dr("ApprovedBy")
        If Not (dr("DateApproved") Is System.DBNull.Value) Then _DateApproved = dr("DateApproved")
        If Not (dr("ApplyTo") Is System.DBNull.Value) Then _ApplyTo = dr("ApplyTo")
        If Not (dr("CreditCardID") Is System.DBNull.Value) Then _CreditCardID = dr("CreditCardID")
        If Not (dr("ClientIP") Is System.DBNull.Value) Then _ClientIP = dr("ClientIP")
        If Not (dr("Applied") Is System.DBNull.Value) Then _Applied = dr("Applied")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("BatchID") Is System.DBNull.Value) Then _BatchID = dr("BatchID")
        If Not (dr("PreAuthID") Is System.DBNull.Value) Then _PreAuthID = dr("PreAuthID")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("Token") Is System.DBNull.Value) Then _Token = dr("Token")
    End Sub

    Public Function Save() As Boolean
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_CCTrans where CCTransID = " & _ID
        da = New SqlDataAdapter(cm)
        Dim sqlCMBuild As New SqlCommandBuilder(da)
        ds = New DataSet
        da.Fill(ds, "t_CCTrans")
        If ds.Tables("t_CCTrans").Rows.Count > 0 Then
            dr = ds.Tables("t_CCTrans").Rows(0)
        Else
            da.InsertCommand = New SqlCommand("dbo.sp_CCTransInsert", cn)
            da.InsertCommand.CommandType = CommandType.StoredProcedure
            da.InsertCommand.Parameters.Add("@TransTypeID", SqlDbType.Int, 0, "TransTypeID")
            da.InsertCommand.Parameters.Add("@Amount", SqlDbType.Money, 0, "Amount")
            da.InsertCommand.Parameters.Add("@AccountID", SqlDbType.Int, 0, "AccountID")
            da.InsertCommand.Parameters.Add("@Imported", SqlDbType.Bit, 0, "Imported")
            da.InsertCommand.Parameters.Add("@ICVResponse", SqlDbType.VarChar, 0, "ICVResponse")
            da.InsertCommand.Parameters.Add("@DateImported", SqlDbType.DateTime, 0, "DateImported")
            da.InsertCommand.Parameters.Add("@Approved", SqlDbType.Int, 0, "Approved")
            da.InsertCommand.Parameters.Add("@ApprovedBy", SqlDbType.VarChar, 0, "ApprovedBy")
            da.InsertCommand.Parameters.Add("@DateApproved", SqlDbType.DateTime, 0, "DateApproved")
            da.InsertCommand.Parameters.Add("@ApplyTo", SqlDbType.VarChar, 0, "ApplyTo")
            da.InsertCommand.Parameters.Add("@CreditCardID", SqlDbType.Int, 0, "CreditCardID")
            da.InsertCommand.Parameters.Add("@ClientIP", SqlDbType.VarChar, 0, "ClientIP")
            da.InsertCommand.Parameters.Add("@Applied", SqlDbType.Bit, 0, "Applied")
            da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
            da.InsertCommand.Parameters.Add("@BatchID", SqlDbType.Int, 0, "BatchID")
            da.InsertCommand.Parameters.Add("@PreAuthID", SqlDbType.Int, 0, "PreAuthID")
            da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.Int, 0, "CreatedByID")
            da.InsertCommand.Parameters.Add("@Token", SqlDbType.VarChar, 0, "Token")
            'da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
            Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CCTransID", SqlDbType.Int, 0, "CCTransID")
            parameter.Direction = ParameterDirection.Output
            dr = ds.Tables("t_CCTrans").NewRow
        End If
        Update_Field("TransTypeID", _TransTypeID, dr)
        Update_Field("Amount", _Amount, dr)
        Update_Field("AccountID", _AccountID, dr)
        Update_Field("Imported", _Imported, dr)
        Update_Field("ICVResponse", _ICVResponse, dr)
        Update_Field("DateImported", _DateImported, dr)
        Update_Field("Approved", _Approved, dr)
        Update_Field("ApprovedBy", _ApprovedBy, dr)
        Update_Field("DateApproved", _DateApproved, dr)
        Update_Field("ApplyTo", _ApplyTo, dr)
        Update_Field("CreditCardID", _CreditCardID, dr)
        Update_Field("ClientIP", _ClientIP, dr)
        Update_Field("Applied", _Applied, dr)
        Update_Field("DateCreated", _DateCreated, dr)
        Update_Field("BatchID", _BatchID, dr)
        Update_Field("PreAuthID", _PreAuthID, dr)
        Update_Field("CreatedByID", _CreatedByID, dr)
        Update_Field("Token", _Token, dr)
        'Update_Field("CRMSID", _CRMSID, dr)
        If ds.Tables("t_CCTrans").Rows.Count < 1 Then ds.Tables("t_CCTrans").Rows.Add(dr)
        da.Update(ds, "t_CCTrans")
        _ID = ds.Tables("t_CCTrans").Rows(0).Item("CCTransID")
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return True
        'Catch ex As Exception
        '_Err = ex.ToString
        'Return False
        'End Try
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
            oEvents.KeyField = "CCTransID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_CC_Trans_For_Refund(ByVal keyField As String, ByVal keyValue As Integer) As SQLDataSource
        Dim ds As New sqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            'If keyField = "ProspectID" Then
            'ds.SelectCommand = "Select Distinct(c.CCTransID), cc.Number, cc.Expiration, c.Amount, c.DateImported, Case when ma.RefRequest = '0' Then 'False' else 'True' end as RefRequest from t_CCTrans c inner join t_ComboItems tt on c.TransTypeID = tt.ComboItemID inner join t_CCMerchantAccount ma on c.AccountID = ma.AccountID inner join t_CreditCard cc on c.CreditCardID = cc.CreditCardID inner join t_CCTransApplyTo cca on c.CCTransID = cca.CCTransID inner join t_Payments p on cca.paymentID = p.PaymentID inner join t_Payment2Invoice pi on pi.PaymentID = p.paymentID inner join t_Invoices i on pi.InvoiceID = i.invoiceID where i.KeyField = 'ContractID' and i.Keyvalue > '0' and i.ProspectID = '" & keyValue & "' and (tt.ComboItem = 'Force' or tt.ComboItem = 'Charge') and c.Imported = 1 and c.ICVResponse Not like 'N%' order by c.CCTransID asc"
            'Else
            ds.SelectCommand = "Select Distinct(c.CCTransID), cc.Number, cc.Expiration, c.Amount, c.DateImported, Case when ma.RefRequest = '0' Then 'False' else 'True' end as RefRequest from t_CCTrans c inner join t_ComboItems tt on c.TransTypeID = tt.COmboItemID inner join t_CCMerchantAccount ma on c.AccountID = ma.AccountID inner join t_CreditCard cc on c.CreditCardID = cc.CreditCardID inner join t_CCTransApplyTo cca on c.CCTransID = cca.CCTransID inner join t_Payments p on cca.paymentID = p.PaymentID inner join t_Payment2Invoice pi on pi.PaymentID = p.paymentID inner join t_Invoices i on pi.InvoiceID = i.invoiceID where i.KeyField = '" & keyField & "' and i.Keyvalue = '" & keyValue & "' and (tt.ComboItem = 'Force' or tt.COmboItem = 'Charge') and c.Imported = 1 and c.ICVResponse Not like 'N%' and c.token is not null and c.token <> '' order by c.CCTransID asc"
            'End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Pending_Requests(ByVal acctID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(r.CCTransID) As ID, p.LastName + ', ' + p.FirstName as Prospect, Case when i.KeyField = 'ContractID' then 'KCP#: ' + c.ContractNumber else i.KeyField + ': ' + Cast(i.KeyValue as varchar) end as Account, r.Amount, r.DateCreated, cc.Number, cc.Expiration, tt.ComboItem as TransType, rb.FirstName + ' ' + rb.Lastname as RequestedBy from t_CCTrans r inner join t_CreditCard cc on r.CreditCardID = cc.CreditCardID inner join t_ComboItems tt on r.TransTypeID = tt.ComboItemID inner join t_CCTransApplyTo ra on r.CCTransID = ra.CCTransID inner join t_Invoices i on ra.PaymentID = i.InvoiceID inner join t_prospect p on i.prospectid = p.prospectid left outer join t_Contract c on i.KeyValue = c.ContractID left outer join t_Personnel rb on r.CreatedByID = rb.PersonnelID where r.AccountID = " & acctID & " and approved = 0 order by r.DateCreated asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function CC_Lookup(ByVal num As String, ByVal exp As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If exp = "" Then
                ds.SelectCommand = "Select Top 100 c.CCTransID, cc.Number, cc.Expiration, cc.NameOnCard, c.Amount, c.DateImported, cct.ComboItem as TransType, ma.Accountname, c.ICVResponse, (Select Top 1 i.KeyField + ' ' + Cast(i.KeyValue as varchar) as ApplyTo from t_CCTransApplyTo a inner join t_Payments p on a.PaymentID = p.PaymentID inner join t_Payment2Invoice pi on pi.PaymentID = p.PaymentID inner join t_invoices i on pi.InvoiceID = i.InvoiceID where a.CCTransID = c.CCTransID) as ApplyTo from t_CCtrans c inner join t_CreditCard cc on c.CreditCardID = cc.CreditCardID inner join t_ComboItems cct on c.TransTypeID = cct.ComboItemID inner join t_CCMerchantAccount ma on c.AccountID = ma.AccountID where cc.number like '%" & num & "' order by c.DateImported desc"
            Else
                ds.SelectCommand = "Select c.CCTransID, cc.Number, cc.Expiration, cc.NameOnCard, c.Amount, c.DateImported, cct.ComboItem as TransType, ma.Accountname, c.ICVResponse, (Select Top 1 i.KeyField + ' ' + Cast(i.KeyValue as varchar) as ApplyTo from t_CCTransApplyTo a inner join t_Payments p on a.PaymentID = p.PaymentID inner join t_Payment2Invoice pi on pi.PaymentID = p.PaymentID inner join t_invoices i on pi.InvoiceID = i.InvoiceID where a.CCTransID = c.CCTransID) as ApplyTo from t_CCtrans c inner join t_CreditCard cc on c.CreditCardID = cc.CreditCardID inner join t_ComboItems cct on c.TransTypeID = cct.ComboItemID inner join t_CCMerchantAccount ma on c.AccountID = ma.AccountID where cc.number like '%" & num & "' and cc.Expiration = '" & exp & "' order by c.DateImported desc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Decline_Card(ByVal ccTransID As Integer) As Boolean
        Dim ret As Boolean = False
        Try

            cm.CommandText = "select a.*, c.CreatedByID from t_CCTransApplyto a inner join t_CCTrans c on c.cctransid = a.cctransid where a.cctransid = '" & ccTransID & "'"
            da.Fill(ds, "Pay")
            For Each row As DataRow In ds.Tables("Pay").Rows
                Dim oPay As New clsPayments
                oPay.Adjustment = 0
                oPay.Amount = row("Amount")
                oPay.MethodID = (New clsComboItems).Lookup_ID("PaymentMethod", "Card Declined")
                oPay.ApplyToID = row("PaymentID")
                oPay.UserID = row("CreatedByID")
                oPay.TransDate = Date.Now
                oPay.Reference = "Card Declined"
                oPay.Description = "Card Declined"
                oPay.Save()
                oPay = Nothing
                GC.Collect()
            Next

            ret = True
        Catch ex As Exception

        End Try
        Return ret
    End Function

    Public Function Find_PA_By_ResID(ByVal resID As Integer, ByVal amt As Double, ByVal acctID As Integer) As Integer
        Dim ccTransID As Integer = 0
        Dim oRes As New clsReservations
        oRes.ReservationID = resID
        oRes.Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select cct.CCTransID from t_CCTrans cct inner join t_ComboItems tt on cct.TransTypeID = tt.ComboItemID inner join t_CreditCard cc on cct.CreditCardID = cc.CreditCardID " & _
                                "where tt.ComboItem = 'PreAuth' and cct.DateImported between '" & oRes.CheckInDate & "' and '" & CDate(oRes.CheckInDate).AddDays(1) & "' and Amount = '" & amt & "' " & _
                                "and cct.ICVResponse not like 'N%' and cct.Imported = 1 and cct.AccountID = '" & acctID & "' and cc.ProspectID = " & oRes.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ccTransID = dread("CCTransID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        oRes = Nothing
        Return ccTransID
    End Function

    Public Function Check_PA(ByVal paID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Forces from t_CCTrans where PreAuthID = '" & paID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Forces") > 0 Then
                bValid = False
            End If
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function


    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Token As String
        Get
            Return _Token
        End Get
        Set(value As String)
            _Token = value
        End Set
    End Property

    Public Property TransTypeID() As Integer
        Get
            Return _TransTypeID
        End Get
        Set(ByVal value As Integer)
            _TransTypeID = value
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

    Public Property AccountID() As Integer
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Integer)
            _AccountID = value
        End Set
    End Property

    Public Property Imported() As Boolean
        Get
            Return _Imported
        End Get
        Set(ByVal value As Boolean)
            _Imported = value
        End Set
    End Property

    Public Property ICVResponse() As String
        Get
            Return _ICVResponse
        End Get
        Set(ByVal value As String)
            _ICVResponse = value
        End Set
    End Property

    Public Property DateImported() As String
        Get
            Return _DateImported
        End Get
        Set(ByVal value As String)
            _DateImported = value
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

    Public Property ApprovedBy() As String
        Get
            Return _ApprovedBy
        End Get
        Set(ByVal value As String)
            _ApprovedBy = value
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

    Public Property ApplyTo() As String
        Get
            Return _ApplyTo
        End Get
        Set(ByVal value As String)
            _ApplyTo = value
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

    Public Property ClientIP() As String
        Get
            Return _ClientIP
        End Get
        Set(ByVal value As String)
            _ClientIP = value
        End Set
    End Property

    Public Property Applied() As Boolean
        Get
            Return _Applied
        End Get
        Set(ByVal value As Boolean)
            _Applied = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property BatchID() As Integer
        Get
            Return _BatchID
        End Get
        Set(ByVal value As Integer)
            _BatchID = value
        End Set
    End Property

    Public Property PreAuthID() As Integer
        Get
            Return _PreAuthID
        End Get
        Set(ByVal value As Integer)
            _PreAuthID = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property CCTransID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Error_Message As String
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
End Class
