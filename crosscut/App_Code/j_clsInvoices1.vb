Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Public Class clsInvoices11
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FinTransID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _Adjustment As Boolean = False
    Dim _PosNeg As Boolean = False
    Dim _ApplyToID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _Reference As String = ""
    Dim _Description As String = ""
    Dim _TransDate As String = ""
    Dim _DueDate As String = ""
    Dim _Err As String = ""
    Dim _PaymentMethodID As Integer = 0
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Invoices where InvoiceID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Invoices where InvoiceID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Invoices")
            If ds.Tables("t_Invoices").Rows.Count > 0 Then
                dr = ds.Tables("t_Invoices").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FinTransID") Is System.DBNull.Value) Then _FinTransID = dr("FinTransID")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
        If Not (dr("Adjustment") Is System.DBNull.Value) Then _Adjustment = dr("Adjustment")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
        If Not (dr("ApplyToID") Is System.DBNull.Value) Then _ApplyToID = dr("ApplyToID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Reference") Is System.DBNull.Value) Then _Reference = dr("Reference")
        If Not (dr("UserID") Is System.DBNull.Value) Then _UserID = dr("UserID")
        If Not (dr("TransDate") Is System.DBNull.Value) Then _TransDate = dr("TransDate")
        If Not (dr("DueDate") Is System.DBNull.Value) Then _DueDate = dr("DueDate")
        If Not (dr("PaymentMethodID") Is System.DBNull.Value) Then _PaymentMethodID = dr("PaymentMethodID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Invoices where InvoiceID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Invoices")
            If ds.Tables("t_Invoices").Rows.Count > 0 Then
                dr = ds.Tables("t_Invoices").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_InvoicesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FinTransID", SqlDbType.Int, 0, "FinTransID")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@Adjustment", SqlDbType.bit, 0, "Adjustment")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.bit, 0, "PosNeg")
                da.InsertCommand.Parameters.Add("@ApplyToID", SqlDbType.int, 0, "ApplyToID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.int, 0, "UserID")
                da.InsertCommand.Parameters.Add("@TransDate", SqlDbType.datetime, 0, "TransDate")
                da.InsertCommand.Parameters.Add("@Reference", SqlDbType.VarChar, 0, "Reference")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@DueDate", SqlDbType.DateTime, 0, "DueDate")
                da.InsertCommand.Parameters.Add("@PaymentMethodID", SqlDbType.Int, 0, "PaymentMethodID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@InvoiceID", SqlDbType.Int, 0, "InvoiceID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Invoices").NewRow
            End If
            Update_Field("FinTransID", _FinTransID, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("Adjustment", _Adjustment, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            Update_Field("ApplyToID", _ApplyToID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("UserID", _UserID, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Reference", _Reference, dr)
            Update_Field("TransDate", _TransDate, dr)
            Update_Field("DueDate", _DueDate, dr)
            Update_Field("PaymentMethodID", _PaymentMethodID, dr)
            If ds.Tables("t_Invoices").Rows.Count < 1 Then ds.Tables("t_Invoices").Rows.Add(dr)
            da.Update(ds, "t_Invoices")
            _ID = ds.Tables("t_Invoices").Rows(0).Item("InvoiceID")
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
            oEvents.KeyField = "InvoiceID"
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

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Property FinTransID() As Integer
        Get
            Return _FinTransID
        End Get
        Set(ByVal value As Integer)
            _FinTransID = value
        End Set
    End Property

    Public Function List_Invoices_With_Balance() As SQLDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        'ds.SelectCommand = "select a.*, c.MaintenanceFeeStatusID MaintenanceFee from ufn_financials(" & _ProspectID & ", '" & _KeyField & "'," & _KeyValue & ",0)  a left join t_contract c on c.contractid = a.keyvalue where balance > 0 order by transdate asc"

        Dim sql = "select a.*, case when (invoice in ('late fee', 'admin fee', 'banking fee', 'interest', 'legalfee') and c.MaintenanceFeeStatusID is not null) or (left(invoice, 2) = 'mf' and c.MaintenanceFeeStatusID is not null) then 'AFC' end AFC from ufn_financials(" & _ProspectID & ", '" & _KeyField & "'," & _KeyValue & ",0)  a left join t_contract c on c.contractid = a.keyvalue where balance > 0 order by transdate asc"
        ds.SelectCommand = Sql
        Return ds
    End Function

    Public Function List_Invoices() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "select * from ufn_financials(" & _ProspectID & ", '" & _KeyField & "'," & _KeyValue & ",0) order by transdate desc"
        Return ds
    End Function

    Public Function List_Invoices_Oldest_First() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "select * from ufn_financials(" & _ProspectID & ", '" & _KeyField & "'," & _KeyValue & ",0) order by transdate asc"
        Return ds
    End Function

    Public Function List_Child_Invoices(ByVal iInvoiceID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "select * from ufn_financials(" & _ProspectID & ", '" & _KeyField & "'," & _KeyValue & "," & iInvoiceID & ") order by transdate desc"
        
        Return ds
    End Function

    Public Function Find_Existing_Invoice(ByVal KeyField As String, ByVal KeyValue As Integer, ByVal TransCode As String) As Integer
        Dim iRet As Integer = 0
        cn.Open()
        cm.CommandText = "Select * from t_Invoices i inner join t_FinTranscodes f on f.fintransid = i.fintransid inner join t_Comboitems tc on tc.comboitemid = f.transcodeid where tc.comboitem = '" & TransCode & "' and i.keyfield = '" & KeyField & "' and i.keyvalue='" & KeyValue & "'"
        dread = cm.ExecuteReader
        If dread.HasRows Then
            dread.Read()
            iRet = dread.Item("InvoiceID")
        End If
        dread.Close()
        cn.Close()
        Return iRet
    End Function


    Public Function List_Invoice_Adjustments() As SQLDataSource
        Dim ds As New SQLDataSOurce
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select c.ComboItemID as ID, c.ComboItem as Adjustment from t_Combos cc inner join t_Comboitems c on cc.ComboID = c.ComboID where cc.COmboName = 'InvoiceAdjustments' and c.Active = '1' order by c.comboitem asc"
        Return ds
    End Function

    Public Function List_Transfer_Invoices(ByVal keyField As String, ByVal KeyValue As Integer, ByVal extraRow As Boolean) As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add("ID")
        dt.Columns.Add("Invoice")
        dt.Columns.Add("TransDate")
        dt.Columns.Add("Amount")
        dt.Columns.Add("Balance")

        If extraRow Then
            dr = dt.NewRow
            dr("ID") = 0
            dr("Invoice") = "Create Same Invoice"
            dr("TransDate") = ""
            dr("Amount") = ""
            dr("Balance") = ""
            dt.Rows.Add(dr)
        End If
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If keyField = "MortgageID" Then
                cm.CommandText = "select ID, Invoice, TransDate, Amount, Balance from ufn_financials(0, 'MortgageDP'," & KeyValue & ",0) UNION select ID, Invoice, TransDate, Amount, Balance from ufn_financials(0, 'MortgageCC', " & KeyValue & ",0)"
            ElseIf keyField = "ConversionID" Then
                cm.CommandText = "select ID, Invoice, TransDate, Amount, Balance from ufn_financials(0, 'ConversionDP'," & KeyValue & ",0) UNION select ID, Invoice, TransDate, Amount, Balance from ufn_financials(0, 'ConversionCC'," & KeyValue & ",0)"
            Else
                cm.CommandText = "select ID, Invoice, TransDate, Amount, Balance from ufn_financials(0, '" & keyField & "', " & KeyValue & ",0)"
            End If
            dread = cm.ExecuteReader
            Do While dread.Read
                dr = dt.NewRow
                dr("ID") = dread("ID")
                dr("Invoice") = dread("Invoice")
                dr("TransDate") = dread("TransDate")
                dr("Amount") = dread("Amount")
                dr("Balance") = dread("Balance")
                dt.Rows.Add(dr)
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function
#Region "Properties"
    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property
    Public Property Adjustment() As Boolean
        Get
            Return _Adjustment
        End Get
        Set(ByVal value As Boolean)
            _Adjustment = value
        End Set
    End Property

    Public Property PosNeg() As Boolean
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Boolean)
            _PosNeg = value
        End Set
    End Property

    Public Property ApplyToID() As Integer
        Get
            Return _ApplyToID
        End Get
        Set(ByVal value As Integer)
            _ApplyToID = value
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
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property TransDate() As String
        Get
            Return _TransDate
        End Get
        Set(ByVal value As String)
            _TransDate = value
        End Set
    End Property
    Public Property DueDate() As String
        Get
            Return _DueDate
        End Get
        Set(ByVal value As String)
            _DueDate = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property
    Public Property Reference() As String
        Get
            Return _Reference
        End Get
        Set(ByVal value As String)
            _Reference = value
        End Set
    End Property

    Public Property PaymentMethodID() As Integer
        Get
            Return _PaymentMethodID
        End Get
        Set(ByVal value As Integer)
            _PaymentMethodID = value
        End Set
    End Property
    Public Property InvoiceID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
#End Region
End Class
