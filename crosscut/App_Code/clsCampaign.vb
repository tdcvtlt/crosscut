Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCampaign
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Name As String = ""
    Dim _Description As String = ""
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _MaxCostPerTour As Decimal = 0
    Dim _PromoNights As Integer = 0
    Dim _PromoRate As Decimal = 0
    Dim _PromoGuests As Integer = 0
    Dim _AdditionalNightRate As Decimal = 0
    Dim _AdditionalGuestRate As Decimal = 0
    Dim _DepartmentProgramID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _AccountID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Campaign where CampaignID = " & _ID, cn)
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select * from t_Campaign where active = 1 order by name")
        Return ds
    End Function
    Public Function Load_Lookup() As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select campaignid, name from t_Campaign where active = '1' order by name asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Lookup() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_Campaign where active = 1 or CampaignID = " & _ID & " order by Name"
        Return ds
    End Function
    Public Function Lookup_ID(ByVal camp As String) As Integer
        Dim campID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select CampaignID from t_Campaign where name = '" & camp & "'"
            dread = cm.ExecuteReader
            dread.Read()
            campID = dread("CampaignID")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return campID
    End Function
    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Campaign where CampaignID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Campaign")
            If ds.Tables("t_Campaign").Rows.Count > 0 Then
                dr = ds.Tables("t_Campaign").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("MaxCostPerTour") Is System.DBNull.Value) Then _MaxCostPerTour = dr("MaxCostPerTour")
        If Not (dr("PromoNights") Is System.DBNull.Value) Then _PromoNights = dr("PromoNights")
        If Not (dr("PromoRate") Is System.DBNull.Value) Then _PromoRate = dr("PromoRate")
        If Not (dr("PromoGuests") Is System.DBNull.Value) Then _PromoGuests = dr("PromoGuests")
        If Not (dr("AdditionalNightRate") Is System.DBNull.Value) Then _AdditionalNightRate = dr("AdditionalNightRate")
        If Not (dr("AdditionalGuestRate") Is System.DBNull.Value) Then _AdditionalGuestRate = dr("AdditionalGuestRate")
        If Not (dr("DepartmentProgramID") Is System.DBNull.Value) Then _DepartmentProgramID = dr("DepartmentProgramID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("AccountID") Is System.DBNull.Value) Then _AccountID = dr("AccountID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Campaign where CampaignID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Campaign")
            If ds.Tables("t_Campaign").Rows.Count > 0 Then
                dr = ds.Tables("t_Campaign").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CampaignInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.ntext, 0, "Description")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.smalldatetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.smalldatetime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@MaxCostPerTour", SqlDbType.money, 0, "MaxCostPerTour")
                da.InsertCommand.Parameters.Add("@PromoNights", SqlDbType.int, 0, "PromoNights")
                da.InsertCommand.Parameters.Add("@PromoRate", SqlDbType.money, 0, "PromoRate")
                da.InsertCommand.Parameters.Add("@PromoGuests", SqlDbType.int, 0, "PromoGuests")
                da.InsertCommand.Parameters.Add("@AdditionalNightRate", SqlDbType.money, 0, "AdditionalNightRate")
                da.InsertCommand.Parameters.Add("@AdditionalGuestRate", SqlDbType.money, 0, "AdditionalGuestRate")
                da.InsertCommand.Parameters.Add("@DepartmentProgramID", SqlDbType.int, 0, "DepartmentProgramID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@AccountID", SqlDbType.int, 0, "AccountID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.Int, 0, "CampaignID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Campaign").NewRow
            End If
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("Name", _Name, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("MaxCostPerTour", _MaxCostPerTour, dr)
            Update_Field("PromoNights", _PromoNights, dr)
            Update_Field("PromoRate", _PromoRate, dr)
            Update_Field("PromoGuests", _PromoGuests, dr)
            Update_Field("AdditionalNightRate", _AdditionalNightRate, dr)
            Update_Field("AdditionalGuestRate", _AdditionalGuestRate, dr)
            Update_Field("DepartmentProgramID", _DepartmentProgramID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("AccountID", _AccountID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_Campaign").Rows.Count < 1 Then ds.Tables("t_Campaign").Rows.Add(dr)
            da.Update(ds, "t_Campaign")
            _ID = ds.Tables("t_Campaign").Rows(0).Item("CampaignID")
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
            oEvents.KeyField = "CampaignID"
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

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
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
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property SubTypeID() As Integer
        Get
            Return _SubTypeID
        End Get
        Set(ByVal value As Integer)
            _SubTypeID = value
        End Set
    End Property

    Public Property MaxCostPerTour() As Decimal
        Get
            Return _MaxCostPerTour
        End Get
        Set(ByVal value As Decimal)
            _MaxCostPerTour = value
        End Set
    End Property

    Public Property PromoNights() As Integer
        Get
            Return _PromoNights
        End Get
        Set(ByVal value As Integer)
            _PromoNights = value
        End Set
    End Property

    Public Property PromoRate() As Decimal
        Get
            Return _PromoRate
        End Get
        Set(ByVal value As Decimal)
            _PromoRate = value
        End Set
    End Property

    Public Property PromoGuests() As Integer
        Get
            Return _PromoGuests
        End Get
        Set(ByVal value As Integer)
            _PromoGuests = value
        End Set
    End Property

    Public Property AdditionalNightRate() As Decimal
        Get
            Return _AdditionalNightRate
        End Get
        Set(ByVal value As Decimal)
            _AdditionalNightRate = value
        End Set
    End Property

    Public Property AdditionalGuestRate() As Decimal
        Get
            Return _AdditionalGuestRate
        End Get
        Set(ByVal value As Decimal)
            _AdditionalGuestRate = value
        End Set
    End Property

    Public Property DepartmentProgramID() As Integer
        Get
            Return _DepartmentProgramID
        End Get
        Set(ByVal value As Integer)
            _DepartmentProgramID = value
        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Integer)
            _DepartmentID = value
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property CampaignID() As Integer
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
End Class
