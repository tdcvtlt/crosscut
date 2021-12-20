Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web.UI.WebControls

Public Class clsFinancialTransactionCodes
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TransCodeID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _TransTypeID As Integer = 0
    Dim _MerchantAccountID As Integer = 0
    Dim _RoomCharge As Boolean = False
    Dim _OpenBalanceCheckIn As Boolean = False
    Dim _Description As String = ""
    Dim _Active As Boolean = False
    Dim _ListFilter As String = ""
    Dim _Taxable As Boolean = False

    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_FinTransCodes where FinTransID = " & _ID, cn)
    End Sub
    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_FinTransCodes where FinTransID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_FinTransCodes")
            If ds.Tables("t_FinTransCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_FinTransCodes").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select f.FinTransID as ID, tc.Comboitem as Code from t_FinTransCodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid inner join t_Comboitems tt on tt.comboitemid = f.transtypeid where f.active = 1 and tt.comboitem in  ('" & Replace(_ListFilter, ",", "','") & "') order by tc.comboitem"
        Return ds
    End Function

    Public Function List_MF() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select f.FinTransID as ID, tc.Comboitem as Code from t_FinTransCodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid inner join t_Comboitems tt on tt.comboitemid = f.transtypeid where f.active = 1 and tc.comboitem like 'mf%' order by tc.comboitem"
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("TransCodeID") Is System.DBNull.Value) Then _TransCodeID = dr("TransCodeID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("TransTypeID") Is System.DBNull.Value) Then _TransTypeID = dr("TransTypeID")
        If Not (dr("MerchantAccountID") Is System.DBNull.Value) Then _MerchantAccountID = dr("MerchantAccountID")
        If Not (dr("RoomCharge") Is System.DBNull.Value) Then _RoomCharge = dr("RoomCharge")
        If Not (dr("OpenBalanceCheckIn") Is System.DBNull.Value) Then _OpenBalanceCheckIn = dr("OpenBalanceCheckIn")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("Taxable") Is System.DBNull.Value) Then _Taxable = dr("Taxable")
    End Sub
    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_FinTransCodes where FinTransID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_FinTransCodes")
            If ds.Tables("t_FinTransCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_FinTransCodes").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_FinTransCodesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TransCodeID", SqlDbType.Int, 0, "TransCodeID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.Money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@TransTypeID", SqlDbType.Int, 0, "TransTypeID")
                da.InsertCommand.Parameters.Add("@MerchantAccountID", SqlDbType.Int, 0, "MerchantAccountID")
                da.InsertCommand.Parameters.Add("@RoomCharge", SqlDbType.Bit, 0, "RoomCharge")
                da.InsertCommand.Parameters.Add("@OpenBalanceCheckIn", SqlDbType.Bit, 0, "OpenBalanceCheckIn")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@Taxable", SqlDbType.Bit, 0, "Taxable")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FinTransID", SqlDbType.Int, 0, "FinTransID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_FinTransCodes").NewRow
            End If
            Update_Field("TransCodeID", _TransCodeID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("TransTypeID", _TransTypeID, dr)
            Update_Field("MerchantAccountID", _MerchantAccountID, dr)
            Update_Field("RoomCharge", _RoomCharge, dr)
            Update_Field("OpenBalanceCheckIn", _OpenBalanceCheckIn, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("Taxable", _Taxable, dr)
            If ds.Tables("t_FinTransCodes").Rows.Count < 1 Then ds.Tables("t_FinTransCodes").Rows.Add(dr)
            da.Update(ds, "t_FinTransCodes")
            _ID = ds.Tables("t_FinTransCodes").Rows(0).Item("FinTransID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function Find_Fin_Trans(ByRef strTransType As String, ByRef strTransCode As String) As Integer
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_FinTransCodes f inner join t_Comboitems tc on tc.comboitemid = f.transcodeid inner join t_Comboitems tt on tt.comboitemid = f.transtypeid where tt.comboitem = '" & strTransType & "' and tc.comboitem = '" & strTransCode & "'"
            Dim dread As SqlDataReader = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                Return dread("FinTransID")
            Else
                Return 0
            End If
            dread.Close()
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function List_Trans_Codes(ByVal transType As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If transType = "" Then
                ds.SelectCommand = "Select f.FintransID, tc.ComboItem as TransCode from t_FintransCodes f inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID where f.Active = '1' order by tc.ComboItem asc"
            Else
                ds.SelectCommand = "Select f.FintransID, tc.ComboItem as TransCode from t_FintransCodes f inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID inner join t_ComboItems tt on f.TransTypeID = tt.ComboItemID where tt.ComboItem = '" & transType & "' and f.Active = '1' order by tc.ComboItem asc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Trans_BY_Acct(ByVal acct As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select tc.CombOItem as TransCode, f.MerchantAccountID as AcctID from t_FInTransCodes f inner join t_CCMerchantAccount ma on f.MerchantAccountID = ma.AccountID inner join t_ComboItems tc on f.TransCodeID = tc.CombOItemID where ma.AccountName in (" & acct & ") and f.Active = '1' order by tc.ComboItem asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
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
            oEvents.KeyField = "FinTransID"
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
    Public Property TransCodeID() As Integer
        Get
            Return _TransCodeID
        End Get
        Set(ByVal value As Integer)
            _TransCodeID = value
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
    Public Property TransTypeID() As Integer
        Get
            Return _TransTypeID
        End Get
        Set(ByVal value As Integer)
            _TransTypeID = value
        End Set
    End Property
    Public Property MerchantAccountID() As Integer
        Get
            Return _MerchantAccountID
        End Get
        Set(ByVal value As Integer)
            _MerchantAccountID = value
        End Set
    End Property
    Public Property RoomCharge() As Boolean
        Get
            Return _RoomCharge
        End Get
        Set(ByVal value As Boolean)
            _RoomCharge = value
        End Set
    End Property
    Public Property OpenBalanceCheckIn() As Boolean
        Get
            Return _OpenBalanceCheckIn
        End Get
        Set(ByVal value As Boolean)
            _OpenBalanceCheckIn = value
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property Taxable() As Boolean
        Get
            Return _Taxable
        End Get
        Set(ByVal value As Boolean)
            _Taxable = value
        End Set
    End Property

    Public Property ListFilter As String
        Get
            Return _ListFilter
        End Get
        Set(ByVal value As String)
            _ListFilter = value
        End Set
    End Property

    Public Property FinTransID() As Integer
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
