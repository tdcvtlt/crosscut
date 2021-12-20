Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLGKioskLeads
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _Phone As String = ""
    Dim _Email As String = ""
    Dim _Zip As String = ""
    Dim _MaritalStatus As String = ""
    Dim _Age As String = ""
    Dim _Income As String = ""
    Dim _CreditCard As String = ""
    Dim _Agree As Boolean = False
    Dim _KioskID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LGKioskLeads where KioskLeadID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LGKioskLeads where KioskLeadID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskLeads")
            If ds.Tables("t_LGKioskLeads").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskLeads").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("Phone") Is System.DBNull.Value) Then _Phone = dr("Phone")
        If Not (dr("Email") Is System.DBNull.Value) Then _Email = dr("Email")
        If Not (dr("Zip") Is System.DBNull.Value) Then _Zip = dr("Zip")
        If Not (dr("MaritalStatus") Is System.DBNull.Value) Then _MaritalStatus = dr("MaritalStatus")
        If Not (dr("Age") Is System.DBNull.Value) Then _Age = dr("Age")
        If Not (dr("Income") Is System.DBNull.Value) Then _Income = dr("Income")
        If Not (dr("CreditCard") Is System.DBNull.Value) Then _CreditCard = dr("CreditCard")
        If Not (dr("Agree") Is System.DBNull.Value) Then _Agree = dr("Agree")
        If Not (dr("KioskID") Is System.DBNull.Value) Then _KioskID = dr("KioskID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LGKioskLeads where KioskLeadID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LGKioskLeads")
            If ds.Tables("t_LGKioskLeads").Rows.Count > 0 Then
                dr = ds.Tables("t_LGKioskLeads").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LGKioskLeadsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.varchar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.varchar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@Phone", SqlDbType.varchar, 0, "Phone")
                da.InsertCommand.Parameters.Add("@Email", SqlDbType.varchar, 0, "Email")
                da.InsertCommand.Parameters.Add("@Zip", SqlDbType.varchar, 0, "Zip")
                da.InsertCommand.Parameters.Add("@MaritalStatus", SqlDbType.varchar, 0, "MaritalStatus")
                da.InsertCommand.Parameters.Add("@Age", SqlDbType.varchar, 0, "Age")
                da.InsertCommand.Parameters.Add("@Income", SqlDbType.varchar, 0, "Income")
                da.InsertCommand.Parameters.Add("@CreditCard", SqlDbType.varchar, 0, "CreditCard")
                da.InsertCommand.Parameters.Add("@Agree", SqlDbType.bit, 0, "Agree")
                da.InsertCommand.Parameters.Add("@KioskID", SqlDbType.Int, 0, "KioskID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.Int, 0, "LocationID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@KioskLeadID", SqlDbType.Int, 0, "KioskLeadID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LGKioskLeads").NewRow
            End If
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("Phone", _Phone, dr)
            Update_Field("Email", _Email, dr)
            Update_Field("Zip", _Zip, dr)
            Update_Field("MaritalStatus", _MaritalStatus, dr)
            Update_Field("Age", _Age, dr)
            Update_Field("Income", _Income, dr)
            Update_Field("CreditCard", _CreditCard, dr)
            Update_Field("Agree", _Agree, dr)
            Update_Field("KioskID", _KioskID, dr)
            Update_Field("LocationID", _LocationID, dr)
            If ds.Tables("t_LGKioskLeads").Rows.Count < 1 Then ds.Tables("t_LGKioskLeads").Rows.Add(dr)
            da.Update(ds, "t_LGKioskLeads")
            _ID = ds.Tables("t_LGKioskLeads").Rows(0).Item("KioskLeadID")
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
            oEvents.KeyField = "KioskLeadID"
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

    Public Property Phone() As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

    Public Property Zip() As String
        Get
            Return _Zip
        End Get
        Set(ByVal value As String)
            _Zip = value
        End Set
    End Property

    Public Property MaritalStatus() As String
        Get
            Return _MaritalStatus
        End Get
        Set(ByVal value As String)
            _MaritalStatus = value
        End Set
    End Property

    Public Property Age() As String
        Get
            Return _Age
        End Get
        Set(ByVal value As String)
            _Age = value
        End Set
    End Property

    Public Property Income() As String
        Get
            Return _Income
        End Get
        Set(ByVal value As String)
            _Income = value
        End Set
    End Property

    Public Property CreditCard() As String
        Get
            Return _CreditCard
        End Get
        Set(ByVal value As String)
            _CreditCard = value
        End Set
    End Property

    Public Property Agree() As Boolean
        Get
            Return _Agree
        End Get
        Set(ByVal value As Boolean)
            _Agree = value
        End Set
    End Property

    Public Property KioskID() As Integer
        Get
            Return _KioskID
        End Get
        Set(ByVal value As Integer)
            _KioskID = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property KioskLeadID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
