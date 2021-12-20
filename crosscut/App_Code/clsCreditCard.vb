Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCreditCard
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _Number As String = ""
    Dim _Expiration As String = ""
    Dim _String As String = ""
    Dim _Security As String = ""
    Dim _Address As String = ""
    Dim _NameOnCard As String = ""
    Dim _PostalCode As String = ""
    Dim _StateID As Integer = 0
    Dim _City As String = ""
    Dim _TypeID As Integer = 0
    Dim _ReadyToImport As Boolean = False
    Dim _ImportedID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _UpdateCard As Boolean = False
    Dim _Token As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CreditCard where CreditCardID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CreditCard where CreditCardID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CreditCard")
            If ds.Tables("t_CreditCard").Rows.Count > 0 Then
                dr = ds.Tables("t_CreditCard").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("Number") Is System.DBNull.Value) Then _Number = dr("Number")
        If Not (dr("Expiration") Is System.DBNull.Value) Then _Expiration = dr("Expiration")
        If Not (dr("String") Is System.DBNull.Value) Then _String = dr("String")
        If Not (dr("Security") Is System.DBNull.Value) Then _Security = dr("Security")
        If Not (dr("Address") Is System.DBNull.Value) Then _Address = dr("Address")
        If Not (dr("NameOnCard") Is System.DBNull.Value) Then _NameOnCard = dr("NameOnCard")
        If Not (dr("PostalCode") Is System.DBNull.Value) Then _PostalCode = dr("PostalCode")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("ReadyToImport") Is System.DBNull.Value) Then _ReadyToImport = dr("ReadyToImport")
        If Not (dr("ImportedID") Is System.DBNull.Value) Then _ImportedID = dr("ImportedID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("UpdateCard") Is System.DBNull.Value) Then _UpdateCard = dr("UpdateCard")
        If Not (dr("Token") Is System.DBNull.Value) Then _Token = dr("Token")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CreditCard where CreditCardID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CreditCard")
            If ds.Tables("t_CreditCard").Rows.Count > 0 Then
                dr = ds.Tables("t_CreditCard").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CreditCardInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@Number", SqlDbType.varchar, 0, "Number")
                da.InsertCommand.Parameters.Add("@Expiration", SqlDbType.varchar, 0, "Expiration")
                da.InsertCommand.Parameters.Add("@String", SqlDbType.varchar, 0, "String")
                da.InsertCommand.Parameters.Add("@Security", SqlDbType.varchar, 0, "Security")
                da.InsertCommand.Parameters.Add("@Address", SqlDbType.varchar, 0, "Address")
                da.InsertCommand.Parameters.Add("@NameOnCard", SqlDbType.varchar, 0, "NameOnCard")
                da.InsertCommand.Parameters.Add("@PostalCode", SqlDbType.varchar, 0, "PostalCode")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.varchar, 0, "City")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@ReadyToImport", SqlDbType.bit, 0, "ReadyToImport")
                da.InsertCommand.Parameters.Add("@ImportedID", SqlDbType.int, 0, "ImportedID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@UpdateCard", SqlDbType.Bit, 0, "UpdateCard")
                da.InsertCommand.Parameters.Add("@Token", SqlDbType.VarChar, 0, "Token")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CreditCardID", SqlDbType.Int, 0, "CreditCardID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CreditCard").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("Number", _Number, dr)
            Update_Field("Expiration", _Expiration, dr)
            Update_Field("String", _String, dr)
            Update_Field("Security", _Security, dr)
            Update_Field("Address", _Address, dr)
            Update_Field("NameOnCard", _NameOnCard, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("City", _City, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("ReadyToImport", _ReadyToImport, dr)
            Update_Field("ImportedID", _ImportedID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("UpdateCard", _UpdateCard, dr)
            Update_Field("Token", _Token, dr)
            If ds.Tables("t_CreditCard").Rows.Count < 1 Then ds.Tables("t_CreditCard").Rows.Add(dr)
            da.Update(ds, "t_CreditCard")
            _ID = ds.Tables("t_CreditCard").Rows(0).Item("CreditCardID")
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
            oEvents.KeyField = "CreditCardID"
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

    Public Function Get_Card_OnFile(ByVal prosID As Integer) As SQLDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select CreditCardID, Number, Expiration, Security, NameOnCard, Address, City, StateID, PostalCode,Token from t_creditCard where prospectid = " & prosID & " and token is not null and left(Token, 4) <> 'supt'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Cards_By_Prospect(ByVal prosID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select c.CreditCardID as ID, ct.ComboItem as Type, c.Number, Left(c.Expiration, 2) + '/' + Right(c.Expiration, 2) as Expiration from t_CreditCard c left outer join t_ComboItems ct on c.TypeID = ct.ComboItemID where c.token not like 'supt%' and c.token <> '' and c.ProspectID = " & prosID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Property Token As String
        Get
            Return _Token
        End Get
        Set(value As String)
            _Token = value
        End Set
    End Property
    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property Number() As String
        Get
            Return _Number
        End Get
        Set(ByVal value As String)
            _Number = value
        End Set
    End Property

    Public Property Expiration() As String
        Get
            Return _Expiration
        End Get
        Set(ByVal value As String)
            _Expiration = value
        End Set
    End Property

    Public Property CCString() As String
        Get
            Return _String
        End Get
        Set(ByVal value As String)
            _String = value
        End Set
    End Property

    Public Property Security() As String
        Get
            Return _Security
        End Get
        Set(ByVal value As String)
            _Security = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(ByVal value As String)
            _Address = value
        End Set
    End Property

    Public Property NameOnCard() As String
        Get
            Return _NameOnCard
        End Get
        Set(ByVal value As String)
            _NameOnCard = value
        End Set
    End Property

    Public Property PostalCode() As String
        Get
            Return _PostalCode
        End Get
        Set(ByVal value As String)
            _PostalCode = value
        End Set
    End Property

    Public Property StateID() As Integer
        Get
            Return _StateID
        End Get
        Set(ByVal value As Integer)
            _StateID = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property ReadyToImport() As Boolean
        Get
            Return _ReadyToImport
        End Get
        Set(ByVal value As Boolean)
            _ReadyToImport = value
        End Set
    End Property

    Public Property ImportedID() As Integer
        Get
            Return _ImportedID
        End Get
        Set(ByVal value As Integer)
            _ImportedID = value
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

    Public Property CreditCardID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
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

    Public Property UpdateCard() As Boolean
        Get
            Return _UpdateCard
        End Get
        Set(value As Boolean)
            _UpdateCard = value
        End Set
    End Property
End Class
