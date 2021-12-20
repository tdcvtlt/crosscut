Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsVendor
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Vendor As String = ""
    Dim _Phone1 As String = ""
    Dim _Fax As String = ""
    Dim _Contact As String = ""
    Dim _Email As String = ""
    Dim _Address1 As String = ""
    Dim _Address2 As String = ""
    Dim _City As String = ""
    Dim _State As String = ""
    Dim _PostalCode As String = ""
    Dim _Description As String = ""
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Vendor where VendorID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Vendor where VendorID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Vendor")
            If ds.Tables("t_Vendor").Rows.Count > 0 Then
                dr = ds.Tables("t_Vendor").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Vendor") Is System.DBNull.Value) Then _Vendor = dr("Vendor")
        If Not (dr("Phone1") Is System.DBNull.Value) Then _Phone1 = dr("Phone1")
        If Not (dr("Fax") Is System.DBNull.Value) Then _Fax = dr("Fax")
        If Not (dr("Contact") Is System.DBNull.Value) Then _Contact = dr("Contact")
        If Not (dr("Email") Is System.DBNull.Value) Then _Email = dr("Email")
        If Not (dr("Address1") Is System.DBNull.Value) Then _Address1 = dr("Address1")
        If Not (dr("Address2") Is System.DBNull.Value) Then _Address2 = dr("Address2")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("State") Is System.DBNull.Value) Then _State = dr("State")
        If Not (dr("PostalCode") Is System.DBNull.Value) Then _PostalCode = dr("PostalCode")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function List_Vendors() As SQLDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select VendorID, Vendor from t_Vendor order by vendor asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Active_Vendors() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "select 0 as VendorID, ' ' as Vendor union Select VendorID, Vendor from t_Vendor where active = 1 order by vendor asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Vendor where VendorID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Vendor")
            If ds.Tables("t_Vendor").Rows.Count > 0 Then
                dr = ds.Tables("t_Vendor").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_VendorInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Vendor", SqlDbType.varchar, 0, "Vendor")
                da.InsertCommand.Parameters.Add("@Phone1", SqlDbType.varchar, 0, "Phone1")
                da.InsertCommand.Parameters.Add("@Fax", SqlDbType.varchar, 0, "Fax")
                da.InsertCommand.Parameters.Add("@Contact", SqlDbType.varchar, 0, "Contact")
                da.InsertCommand.Parameters.Add("@Email", SqlDbType.varchar, 0, "Email")
                da.InsertCommand.Parameters.Add("@Address1", SqlDbType.varchar, 0, "Address1")
                da.InsertCommand.Parameters.Add("@Address2", SqlDbType.varchar, 0, "Address2")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.varchar, 0, "City")
                da.InsertCommand.Parameters.Add("@State", SqlDbType.varchar, 0, "State")
                da.InsertCommand.Parameters.Add("@PostalCode", SqlDbType.varchar, 0, "PostalCode")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.Int, 0, "VendorID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Vendor").NewRow
            End If
            Update_Field("Vendor", _Vendor, dr)
            Update_Field("Phone1", _Phone1, dr)
            Update_Field("Fax", _Fax, dr)
            Update_Field("Contact", _Contact, dr)
            Update_Field("Email", _Email, dr)
            Update_Field("Address1", _Address1, dr)
            Update_Field("Address2", _Address2, dr)
            Update_Field("City", _City, dr)
            Update_Field("State", _State, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Vendor").Rows.Count < 1 Then ds.Tables("t_Vendor").Rows.Add(dr)
            da.Update(ds, "t_Vendor")
            _ID = ds.Tables("t_Vendor").Rows(0).Item("VendorID")
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
            oEvents.KeyField = "VendorID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Sales_Locations(ByVal vendor As String) As SQLDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select SalesLocationID, Location from t_VendorSalesLocations a inner join t_Vendor b on a.vendorid = b.vendorid where b.Vendor = '" & vendor & "' and a.Active = '1'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Vendor() As String
        Get
            Return _Vendor
        End Get
        Set(ByVal value As String)
            _Vendor = value
        End Set
    End Property

    Public Property Phone1() As String
        Get
            Return _Phone1
        End Get
        Set(ByVal value As String)
            _Phone1 = value
        End Set
    End Property

    Public Property Fax() As String
        Get
            Return _Fax
        End Get
        Set(ByVal value As String)
            _Fax = value
        End Set
    End Property

    Public Property Contact() As String
        Get
            Return _Contact
        End Get
        Set(ByVal value As String)
            _Contact = value
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

    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
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

    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public Property VendorID() As Integer
        Set(value As Integer)
            _ID = value
        End Set
        Get
            Return _ID
        End Get
    End Property
End Class
