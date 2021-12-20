Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackage
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Package As String = ""
    Dim _Cost As Decimal = 0
    Dim _ExpirationPeriod As Integer = 0
    Dim _PromptCost As Boolean = False
    Dim _Active As Boolean = False
    Dim _DateActive As String = ""
    Dim _DateInActive As String = ""
    Dim _MinimumCharge As Decimal = 0
    Dim _MaximumCharge As Decimal = 0
    Dim _DefaultCost As Decimal = 0
    Dim _Description As String = ""
    Dim _CRMSID As Integer = 0
    Dim _OptionalLocation As Boolean = False
    Dim _AccomRoomTypeID As Integer = 0
    Dim _AccomID As Integer = 0
    Dim _UnitTypeID As Integer = 0
    Dim _Bedrooms As String = ""
    Dim _MinNights As Integer = 0
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _DefaultInvoiceAmt As Decimal = 0
    Dim _MaxPremiumAmount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Package where PackageID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Package where PackageID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Package")
            If ds.Tables("t_Package").Rows.Count > 0 Then
                dr = ds.Tables("t_Package").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Package") Is System.DBNull.Value) Then _Package = dr("Package")
        If Not (dr("Cost") Is System.DBNull.Value) Then _Cost = dr("Cost")
        If Not (dr("ExpirationPeriod") Is System.DBNull.Value) Then _ExpirationPeriod = dr("ExpirationPeriod")
        If Not (dr("PromptCost") Is System.DBNull.Value) Then _PromptCost = dr("PromptCost")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("DateActive") Is System.DBNull.Value) Then _DateActive = dr("DateActive")
        If Not (dr("DateInActive") Is System.DBNull.Value) Then _DateInActive = dr("DateInActive")
        If Not (dr("MinimumCharge") Is System.DBNull.Value) Then _MinimumCharge = dr("MinimumCharge")
        If Not (dr("MaximumCharge") Is System.DBNull.Value) Then _MaximumCharge = dr("MaximumCharge")
        If Not (dr("DefaultCost") Is System.DBNull.Value) Then _DefaultCost = dr("DefaultCost")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("OptionalLocation") Is System.DBNull.Value) Then _OptionalLocation = dr("OptionalLocation")
        If Not (dr("AccomRoomTypeID") Is System.DBNull.Value) Then _AccomRoomTypeID = dr("AccomRoomTypeID")
        If Not (dr("AccomID") Is System.DBNull.Value) Then _AccomID = dr("AccomID")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("Bedrooms") Is System.DBNull.Value) Then _Bedrooms = dr("Bedrooms")
        If Not (dr("MinNights") Is System.DBNull.Value) Then _MinNights = dr("MinNights")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("DefaultInvoiceAmt") Is System.DBNull.Value) Then _DefaultInvoiceAmt = dr("DefaultInvoiceAmt")
        If Not (dr("MaxPremiumAmount") Is System.DBNull.Value) Then _MaxPremiumAmount = dr("MaxPremiumAmount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Package where PackageID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Package")
            If ds.Tables("t_Package").Rows.Count > 0 Then
                dr = ds.Tables("t_Package").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Package", SqlDbType.varchar, 0, "Package")
                da.InsertCommand.Parameters.Add("@Cost", SqlDbType.money, 0, "Cost")
                da.InsertCommand.Parameters.Add("@ExpirationPeriod", SqlDbType.int, 0, "ExpirationPeriod")
                da.InsertCommand.Parameters.Add("@PromptCost", SqlDbType.bit, 0, "PromptCost")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@DateActive", SqlDbType.datetime, 0, "DateActive")
                da.InsertCommand.Parameters.Add("@DateInActive", SqlDbType.datetime, 0, "DateInActive")
                da.InsertCommand.Parameters.Add("@MinimumCharge", SqlDbType.money, 0, "MinimumCharge")
                da.InsertCommand.Parameters.Add("@MaximumCharge", SqlDbType.money, 0, "MaximumCharge")
                da.InsertCommand.Parameters.Add("@DefaultCost", SqlDbType.money, 0, "DefaultCost")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@OptionalLocation", SqlDbType.bit, 0, "OptionalLocation")
                da.InsertCommand.Parameters.Add("@AccomRoomTypeID", SqlDbType.Int, 0, "AccomRoomTypeID")
                da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.Int, 0, "AccomID")
                da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.Int, 0, "UnitTypeID")
                da.InsertCommand.Parameters.Add("@Bedrooms", SqlDbType.VarChar, 0, "Bedrooms")
                da.InsertCommand.Parameters.Add("@MinNights", SqlDbType.Int, 0, "MinNights")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.DateTime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.DateTime, 0, "Enddate")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@DefaultInvoiceAmt", SqlDbType.Money, 0, "DefaultInvoiceAmt")
                da.InsertCommand.Parameters.Add("@MaxPremiumAmount", SqlDbType.Money, 0, "MaxPremiumAmount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.Int, 0, "PackageID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Package").NewRow
            End If
            Update_Field("Package", _Package, dr)
            Update_Field("Cost", _Cost, dr)
            Update_Field("ExpirationPeriod", _ExpirationPeriod, dr)
            Update_Field("PromptCost", _PromptCost, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("DateActive", _DateActive, dr)
            Update_Field("DateInActive", _DateInActive, dr)
            Update_Field("MinimumCharge", _MinimumCharge, dr)
            Update_Field("MaximumCharge", _MaximumCharge, dr)
            Update_Field("DefaultCost", _DefaultCost, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("OptionalLocation", _OptionalLocation, dr)
            Update_Field("AccomRoomTypeID", _AccomRoomTypeID, dr)
            Update_Field("AccomID", _AccomID, dr)
            Update_Field("UnitTypeID", _UnitTypeID, dr)
            Update_Field("Bedrooms", _Bedrooms, dr)
            Update_Field("MinNights", _MinNights, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("DefaultInvoiceAmt", _DefaultInvoiceAmt, dr)
            Update_Field("MaxPremiumAmount", _MaxPremiumAmount, dr)
            If ds.Tables("t_Package").Rows.Count < 1 Then ds.Tables("t_Package").Rows.Add(dr)
            da.Update(ds, "t_Package")
            _ID = ds.Tables("t_Package").Rows(0).Item("PackageID")
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
            oEvents.KeyField = "PackageID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Sub Fill_DD(ByVal dd As DropDownList, ByVal pkgID As Integer)
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select PackageID, Package from t_Package where active = 1 or packageID = " & pkgID & " order by Package "
        dd.DataSource = ds
        dd.DataValueField = "PackageID"
        dd.DataTextField = "Package"
        dd.DataBind()
        ds = Nothing
    End Sub

    Public Function Get_Pkg_Name(ByVal pkgID As Integer) As String
        Dim pkgName As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Package from t_Package where PackageID = " & pkgID
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                dread.Read()
                pkgName = dread("Package")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return pkgName
    End Function

    Public Function Validate_Vendor(ByVal pkgID As Integer, ByVal vendors As String) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Vendor2Package where packageid = " & pkgID & " and vendorid in (" & vendors & ")"
            dread = cm.ExecuteReader()
            bValid = dread.HasRows
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Validate_Vendor_By_Prospect(ByVal prosID As Integer, ByVal vendors As String) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageIssued pi inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on vp.PackageID = pk.PackageID where pi.Prospectid = " & prosID & " and vp.vendorid in (" & vendors & ")"
            dread = cm.ExecuteReader()
            bValid = dread.HasRows
            dread.Close()
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

    Public Property Package() As String
        Get
            Return _Package
        End Get
        Set(ByVal value As String)
            _Package = value
        End Set
    End Property

    Public Property Cost() As Decimal
        Get
            Return _Cost
        End Get
        Set(ByVal value As Decimal)
            _Cost = value
        End Set
    End Property

    Public Property ExpirationPeriod() As Integer
        Get
            Return _ExpirationPeriod
        End Get
        Set(ByVal value As Integer)
            _ExpirationPeriod = value
        End Set
    End Property

    Public Property PromptCost() As Boolean
        Get
            Return _PromptCost
        End Get
        Set(ByVal value As Boolean)
            _PromptCost = value
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

    Public Property DateActive() As String
        Get
            Return _DateActive
        End Get
        Set(ByVal value As String)
            _DateActive = value
        End Set
    End Property

    Public Property DateInActive() As String
        Get
            Return _DateInActive
        End Get
        Set(ByVal value As String)
            _DateInActive = value
        End Set
    End Property

    Public Property MinimumCharge() As Decimal
        Get
            Return _MinimumCharge
        End Get
        Set(ByVal value As Decimal)
            _MinimumCharge = value
        End Set
    End Property

    Public Property MaximumCharge() As Decimal
        Get
            Return _MaximumCharge
        End Get
        Set(ByVal value As Decimal)
            _MaximumCharge = value
        End Set
    End Property

    Public Property DefaultCost() As Decimal
        Get
            Return _DefaultCost
        End Get
        Set(ByVal value As Decimal)
            _DefaultCost = value
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

    Public Property OptionalLocation() As Boolean
        Get
            Return _OptionalLocation
        End Get
        Set(ByVal value As Boolean)
            _OptionalLocation = value
        End Set
    End Property

    Public Property UnitTypeID() As Integer
        Get
            Return _UnitTypeID
        End Get
        Set(ByVal value As Integer)
            _UnitTypeID = value
        End Set
    End Property

    Public Property AccomRoomTypeID() As Integer
        Get
            Return _AccomRoomTypeID
        End Get
        Set(ByVal value As Integer)
            _AccomRoomTypeID = value
        End Set
    End Property

    Public Property AccomID() As Integer
        Get
            Return _AccomID
        End Get
        Set(ByVal value As Integer)
            _AccomID = value
        End Set
    End Property

    Public Property PackageID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
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

    Public Property MinNights() As Integer
        Get
            Return _MinNights
        End Get
        Set(ByVal value As Integer)
            _MinNights = value
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

    Public Property Bedrooms() As String
        Get
            Return _Bedrooms
        End Get
        Set(ByVal value As String)
            _Bedrooms = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(value As Integer)
            _TypeID = value
        End Set
    End Property
    Public Property SubTypeID() As Integer
        Get
            Return _SubTypeID
        End Get
        Set(value As Integer)
            _SubTypeID = value
        End Set
    End Property
    Public Property DefaultInvoiceAmt() As Decimal
        Get
            Return _DefaultInvoiceAmt
        End Get
        Set(value As Decimal)
            _DefaultInvoiceAmt = value
        End Set
    End Property
    Public Property MaxPremiumAmount() As Decimal
        Get
            Return _MaxPremiumAmount
        End Get
        Set(value As Decimal)
            _MaxPremiumAmount = value
        End Set
    End Property
End Class
