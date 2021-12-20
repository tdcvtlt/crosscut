Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsOwnerAccounts
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _UserName As String = ""
    Dim _password As String = ""
    Dim _DateCreated As String = ""
    Dim _emailname As String = ""
    Dim _Confirmed As Boolean = False
    Dim _ConfirmationSessionID As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_OwnerAccounts where OwnerAccountID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_OwnerAccounts where OwnerAccountID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_OwnerAccounts")
            If ds.Tables("t_OwnerAccounts").Rows.Count > 0 Then
                dr = ds.Tables("t_OwnerAccounts").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("UserName") Is System.DBNull.Value) Then _UserName = dr("UserName")
        If Not (dr("password") Is System.DBNull.Value) Then _password = dr("password")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("emailname") Is System.DBNull.Value) Then _emailname = dr("emailname")
        If Not (dr("Confirmed") Is System.DBNull.Value) Then _Confirmed = dr("Confirmed")
        If Not (dr("ConfirmationSessionID") Is System.DBNull.Value) Then _ConfirmationSessionID = dr("ConfirmationSessionID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_OwnerAccounts where OwnerAccountID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_OwnerAccounts")
            If ds.Tables("t_OwnerAccounts").Rows.Count > 0 Then
                dr = ds.Tables("t_OwnerAccounts").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_OwnerAccountsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@UserName", SqlDbType.varchar, 0, "UserName")
                da.InsertCommand.Parameters.Add("@password", SqlDbType.varchar, 0, "password")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@emailname", SqlDbType.varchar, 0, "emailname")
                da.InsertCommand.Parameters.Add("@Confirmed", SqlDbType.bit, 0, "Confirmed")
                da.InsertCommand.Parameters.Add("@ConfirmationSessionID", SqlDbType.varchar, 0, "ConfirmationSessionID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@OwnerAccountID", SqlDbType.Int, 0, "OwnerAccountID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_OwnerAccounts").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("UserName", _UserName, dr)
            Update_Field("password", _password, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("emailname", _emailname, dr)
            Update_Field("Confirmed", _Confirmed, dr)
            Update_Field("ConfirmationSessionID", _ConfirmationSessionID, dr)
            If ds.Tables("t_OwnerAccounts").Rows.Count < 1 Then ds.Tables("t_OwnerAccounts").Rows.Add(dr)
            da.Update(ds, "t_OwnerAccounts")
            _ID = ds.Tables("t_OwnerAccounts").Rows(0).Item("OwnerAccountID")
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
            oEvents.KeyField = "OwnerAccountID"
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

    Public Function Lookup_Acct(ByVal prosID As Integer) As Integer
        Dim acctID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select OwnerAccountID from t_OwnerAccounts where ProspectID = " & prosID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                acctID = dread("OwnerAccountID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return acctID
    End Function


    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Public Property password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
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

    Public Property emailname() As String
        Get
            Return _emailname
        End Get
        Set(ByVal value As String)
            _emailname = value
        End Set
    End Property

    Public Property Confirmed() As Boolean
        Get
            Return _Confirmed
        End Get
        Set(ByVal value As Boolean)
            _Confirmed = value
        End Set
    End Property

    Public Property ConfirmationSessionID() As String
        Get
            Return _ConfirmationSessionID
        End Get
        Set(ByVal value As String)
            _ConfirmationSessionID = value
        End Set
    End Property

    Public Property OwnerAccountID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
