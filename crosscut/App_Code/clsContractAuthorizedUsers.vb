Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsContractAuthorizedUsers
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _Active As Boolean = False
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SQLDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ContractAuthorizedUsers where AuthorizedUserID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ContractAuthorizedUsers where AuthorizedUserID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ContractAuthorizedUsers")
            If ds.Tables("t_ContractAuthorizedUsers").Rows.Count > 0 Then
                dr = ds.Tables("t_ContractAuthorizedUsers").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractAuthorizedUsers where AuthorizedUserID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ContractAuthorizedUsers")
            If ds.Tables("t_ContractAuthorizedUsers").Rows.Count > 0 Then
                dr = ds.Tables("t_ContractAuthorizedUsers").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ContractAuthorizedUsersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.varchar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.varchar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AuthorizedUserID", SqlDbType.Int, 0, "AuthorizedUserID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ContractAuthorizedUsers").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            If ds.Tables("t_ContractAuthorizedUsers").Rows.Count < 1 Then ds.Tables("t_ContractAuthorizedUsers").Rows.Add(dr)
            da.Update(ds, "t_ContractAuthorizedUsers")
            _ID = ds.Tables("t_ContractAuthorizedUsers").Rows(0).Item("AuthorizedUserID")
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
            oEvents.KeyField = "AuthorizedUserID"
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

    Public Function List_AuthUsers(ByVal conID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select AuthorizedUserID as ID, Firstname, LastName from t_ContractAuthorizedUsers where contractid = " & conID & " and active = '1'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Remove_Auth_User(ByVal authUserID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_ContractAuthorizedUsers set Active = '0' where AuthorizedUserID = " & authUserID & ""
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bRemoved = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function
    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
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

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
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

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property AuthorizedUserID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
