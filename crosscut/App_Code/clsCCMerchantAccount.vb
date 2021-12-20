Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCCMerchantAccount
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AccountName As String = ""
    Dim _Description As String = ""
    Dim _SettlementFolder As String = ""
    Dim _Active As Boolean = False
    Dim _AccountTypeID As Integer = 0
    Dim _PreAuthAttempts As Integer = 0
    Dim _AccountNumber As String = ""
    Dim _CCApproval As Boolean = False
    Dim _RefRequest As Boolean = False
    Dim _PublicToken As String = ""
    Dim _PrivateToken As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CCMerchantAccount where AccountID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CCMerchantAccount where AccountID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CCMerchantAccount")
            If ds.Tables("t_CCMerchantAccount").Rows.Count > 0 Then
                dr = ds.Tables("t_CCMerchantAccount").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AccountID") Is System.DBNull.Value) Then _ID = dr("AccountID")
        If Not (dr("AccountName") Is System.DBNull.Value) Then _AccountName = dr("AccountName")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("SettlementFolder") Is System.DBNull.Value) Then _SettlementFolder = dr("SettlementFolder")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("AccountTypeID") Is System.DBNull.Value) Then _AccountTypeID = dr("AccountTypeID")
        If Not (dr("PreAuthAttempts") Is System.DBNull.Value) Then _PreAuthAttempts = dr("PreAuthAttempts")
        If Not (dr("AccountNumber") Is System.DBNull.Value) Then _AccountNumber = dr("AccountNumber")
        If Not (dr("CCApproval") Is System.DBNull.Value) Then _CCApproval = dr("CCApproval")
        If Not (dr("RefRequest") Is System.DBNull.Value) Then _RefRequest = dr("RefRequest")
        If Not (dr("PublicToken") Is System.DBNull.Value) Then _PublicToken = dr("PublicToken")
        If Not (dr("PrivateToken") Is System.DBNull.Value) Then _PrivateToken = dr("PrivateToken")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CCMerchantAccount where AccountID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CCMerchantAccount")
            If ds.Tables("t_CCMerchantAccount").Rows.Count > 0 Then
                dr = ds.Tables("t_CCMerchantAccount").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CCMerchantAccountInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AccountName", SqlDbType.varchar, 0, "AccountName")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@SettlementFolder", SqlDbType.VarChar, 0, "SettlementFolder")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@AccountTypeID", SqlDbType.int, 0, "AccountTypeID")
                da.InsertCommand.Parameters.Add("@PreAuthAttempts", SqlDbType.int, 0, "PreAuthAttempts")
                da.InsertCommand.Parameters.Add("@AccountNumber", SqlDbType.varchar, 0, "AccountNumber")
                da.InsertCommand.Parameters.Add("@CCApproval", SqlDbType.bit, 0, "CCApproval")
                da.InsertCommand.Parameters.Add("@RefRequest", SqlDbType.Bit, 0, "RefRequest")
                da.InsertCommand.Parameters.Add("@PublicToken", SqlDbType.VarChar, 0, "PublicToken")
                da.InsertCommand.Parameters.Add("@PrivateToken", SqlDbType.VarChar, 0, "PrivateToken")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AccountID", SqlDbType.Int, 0, "AccountID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CCMerchantAccount").NewRow
            End If
            Update_Field("AccountName", _AccountName, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("SettlementFolder", _SettlementFolder, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("AccountTypeID", _AccountTypeID, dr)
            Update_Field("PreAuthAttempts", _PreAuthAttempts, dr)
            Update_Field("AccountNumber", _AccountNumber, dr)
            Update_Field("CCApproval", _CCApproval, dr)
            Update_Field("RefRequest", _RefRequest, dr)
            Update_Field("PublicToken", _PublicToken, dr)
            Update_Field("PrivateToken", _PrivateToken, dr)
            If ds.Tables("t_CCMerchantAccount").Rows.Count < 1 Then ds.Tables("t_CCMerchantAccount").Rows.Add(dr)
            da.Update(ds, "t_CCMerchantAccount")
            _ID = ds.Tables("t_CCMerchantAccount").Rows(0).Item("AccountID")
            Return True
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "AccountID"
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

    Public Function List_Accounts() As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select AccountID, Description, PublicToken from t_CCMerchantAccount order by accountid"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Lookup_By_AcctName(ByVal acctName As String) As Integer
        Dim acctID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select AccountID from t_CCMerchantAccount where accountname = '" & acctName & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                acctID = dread("AccountID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return acctID
    End Function

    Public Property PublicToken As String
        Get
            Return _PublicToken
        End Get
        Set(value As String)
            _PublicToken = value
        End Set
    End Property

    Public Property PrivateToken As String 
        Get
            Return _PrivateToken
        End Get
        Set(value As String)
            _PrivateToken = value
        End Set
    End Property

    Public Property AccountName() As String
        Get
            Return _AccountName
        End Get
        Set(ByVal value As String)
            _AccountName = value
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

    Public Property SettlementFolder() As String
        Get
            Return _SettlementFolder
        End Get
        Set(ByVal value As String)
            _SettlementFolder = value
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

    Public Property AccountTypeID() As Integer
        Get
            Return _AccountTypeID
        End Get
        Set(ByVal value As Integer)
            _AccountTypeID = value
        End Set
    End Property

    Public Property PreAuthAttempts() As Integer
        Get
            Return _PreAuthAttempts
        End Get
        Set(ByVal value As Integer)
            _PreAuthAttempts = value
        End Set
    End Property

    Public Property AccountNumber() As String
        Get
            Return _AccountNumber
        End Get
        Set(ByVal value As String)
            _AccountNumber = value
        End Set
    End Property

    Public Property CCApproval() As Boolean
        Get
            Return _CCApproval
        End Get
        Set(ByVal value As Boolean)
            _CCApproval = value
        End Set
    End Property

    Public Property RefRequest() As Boolean
        Get
            Return _RefRequest
        End Get
        Set(ByVal value As Boolean)
            _RefRequest = value
        End Set
    End Property

    Public Property AccountID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
