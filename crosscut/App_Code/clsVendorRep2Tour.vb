Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsVendorRep2Tour
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RepID As Integer = 0
    Dim _TourID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _UserName As String = ""
    Dim _SolID As Integer = 0
    'Dim _UserID As Integer = 0
    Dim _SaleLocID As Integer = 0
    Dim _ProsAccomID As Integer = 0
    Dim _PkgPrice As Double = 0.0
    Dim _Paid As Boolean = False
    Dim _DatePaid As String = ""
    Dim _CB As Boolean = False
    Dim _DateCB As String = ""
    Dim _AmountPaid As Double = 0.0
    Dim _AmountCB As Double = 0.0
    Dim _Payable As Boolean = False
    Dim _Voicestamp As String = ""
    Dim _Quinella As Boolean = False
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select *, UserID as SolID from t_VendorRep2Tour where RepTourID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select *, UserID as SolID from t_VendorRep2Tour where RepTourID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_VendorRep2Tour")
            If ds.Tables("t_VendorRep2Tour").Rows.Count > 0 Then
                dr = ds.Tables("t_VendorRep2Tour").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RepID") Is System.DBNull.Value) Then _RepID = dr("RepID")
        If Not (dr("TourID") Is System.DBNull.Value) Then _TourID = dr("TourID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("UserName") Is System.DBNull.Value) Then _UserName = dr("UserName")
        If Not (dr("SolID") Is System.DBNull.Value) Then _SolID = dr("SolID")
        If Not (dr("SaleLocID") Is System.DBNull.Value) Then _SaleLocID = dr("SaleLocID")
        If Not (dr("ProsAccomID") Is System.DBNull.Value) Then _ProsAccomID = dr("ProsAccomID")
        If Not (dr("PkgPrice") Is System.DBNull.Value) Then _PkgPrice = dr("PkgPrice")
        If Not (dr("Paid") Is System.DBNull.Value) Then _Paid = dr("Paid")
        If Not (dr("DatePaid") Is System.DBNull.Value) Then _DatePaid = dr("DatePaid")
        If Not (dr("CB") Is System.DBNull.Value) Then _CB = dr("CB")
        If Not (dr("DateCB") Is System.DBNull.Value) Then _DateCB = dr("DateCB")
        If Not (dr("AmountPaid") Is System.DBNull.Value) Then _AmountPaid = dr("AmountPaid")
        If Not (dr("AmountCB") Is System.DBNull.Value) Then _AmountCB = dr("AmountCB")
        If Not (dr("Payable") Is System.DBNull.Value) Then _Payable = dr("Payable")
        If Not (dr("Voicestamp") Is System.DBNull.Value) Then _Voicestamp = dr("Voicestamp")
        If Not (dr("Quinella") Is System.DBNull.Value) Then _Quinella = dr("Quinella")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select *, UserID as SolID from t_VendorRep2Tour where RepTourID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_VendorRep2Tour")
            If ds.Tables("t_VendorRep2Tour").Rows.Count > 0 Then
                dr = ds.Tables("t_VendorRep2Tour").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_VendorRep2TourInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RepID", SqlDbType.Int, 0, "RepID")
                da.InsertCommand.Parameters.Add("@TourID", SqlDbType.Int, 0, "TourID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 0, "UserName")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.Int, 0, "SolID")
                da.InsertCommand.Parameters.Add("@SaleLocID", SqlDbType.Int, 0, "SaleLocID")
                da.InsertCommand.Parameters.Add("@ProsAccomID", SqlDbType.Int, 0, "ProsAccomID")
                da.InsertCommand.Parameters.Add("@PkgPrice", SqlDbType.Float, 0, "PkgPrice")
                da.InsertCommand.Parameters.Add("@Paid", SqlDbType.Bit, 0, "Paid")
                da.InsertCommand.Parameters.Add("@DatePaid", SqlDbType.DateTime, 0, "DatePaid")
                da.InsertCommand.Parameters.Add("@CB", SqlDbType.Bit, 0, "CB")
                da.InsertCommand.Parameters.Add("@DateCB", SqlDbType.DateTime, 0, "DateCB")
                da.InsertCommand.Parameters.Add("@AmountPaid", SqlDbType.Float, 0, "AmountPaid")
                da.InsertCommand.Parameters.Add("@AmountCB", SqlDbType.Float, 0, "AmountCB")
                da.InsertCommand.Parameters.Add("@Payable", SqlDbType.Bit, 0, "Payable")
                da.InsertCommand.Parameters.Add("@Voicestamp", SqlDbType.VarChar, 0, "Voicestamp")
                da.InsertCommand.Parameters.Add("@Quinella", SqlDbType.Bit, 0, "Quinella")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RepTourID", SqlDbType.Int, 0, "RepTourID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_VendorRep2Tour").NewRow
            End If
            Update_Field("RepID", _RepID, dr)
            Update_Field("TourID", _TourID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("UserName", _UserName, dr)
            Update_Field("UserID", _SolID, dr)
            Update_Field("SaleLocID", _SaleLocID, dr)
            Update_Field("ProsAccomID", _ProsAccomID, dr)
            Update_Field("PkgPrice", _PkgPrice, dr)
            Update_Field("Paid", _Paid, dr)
            Update_Field("DatePaid", _DatePaid, dr)
            Update_Field("CB", _CB, dr)
            Update_Field("DateCB", _DateCB, dr)
            Update_Field("AmountPaid", _AmountPaid, dr)
            Update_Field("AmountCB", _AmountCB, dr)
            Update_Field("Payable", _Payable, dr)
            Update_Field("Voicestamp", _Voicestamp, dr)
            Update_Field("Quinella", _Quinella, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_VendorRep2Tour").Rows.Count < 1 Then ds.Tables("t_VendorRep2Tour").Rows.Add(dr)
            da.Update(ds, "t_VendorRep2Tour")
            _ID = ds.Tables("t_VendorRep2Tour").Rows(0).Item("RepTourID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            Return False
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
            oEvents.KeyField = "RepTourID"
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

    Public Function List_Display(ByVal tourID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select RepTourID as ID, vs.Location, p.FirstName + ' ' + p.Lastname as Solicitor from t_VendorRep2Tour vt left outer join t_VendorSalesLocations vs on vt.SalelocID = vs.SalesLocationID left outer join t_Personnel p on vt.UserID = p.PersonnelID where vt.TourID = '" & tourID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Locations(ByVal tourid As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(ID), Location from " & _
                               "(Select Distinct(vsl.SalesLocationID) as ID, vsl.Location from t_Tour t inner join t_VendorCampaigns vc on t.CampaignID = vc.CRMSCampID inner join t_VendorSalesLocations vsl on vsl.VendorID = vc.VendorID where t.TourID = '" & tourid & "' and vsl.Active = 1 " & _
                               "UNION " & _
                               "Select Distinct(vsl.SalesLocationID) as ID, vsl.Location from t_Tour t inner join t_packageIssued pi on t.PackageIssuedID = pi.PackageIssuedID inner join t_Vendor2Package vp on pi.PackageID = vp.PackageID inner join t_VendorSalesLocations vsl on vp.VendorID = vsl.VendorID where t.TourID = '" & tourid & "' and vsl.Active = 1 " & _
                                ") x order by location"
            '            ds.SelectCommand = "Select vsl.SalesLocationID as ID, vsl.Location from t_Tour t inner join t_VendorCampaigns vc on t.CampaignID = vc.CRMSCampID inner join t_VendorSalesLocations vsl on vsl.VendorID = vc.VendorID where t.TourID = '" & tourid & "' and vsl.Active = 1 order by vsl.Location asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Reps(ByVal tourid As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(PersonnelID), Rep from " & _
                                "(Select Distinct(p.PersonnelID), p.FirstName + ' ' + p.LastName as Rep from t_Tour t inner join t_VendorCampaigns vc on t.CampaignID = vc.CRMSCampID inner join t_Vendor2Personnel vp on vc.VendorID = vp.VendorID inner join t_Personnel p on vp.PersonnelID = p.PersonnelID where t.TourID = '" & tourid & "' " & _
                                "UNION " & _
                                "Select Distinct(p.PersonnelID), p.FirstName + ' ' + p.LastName as Rep from t_Tour t inner join t_PackageIssued pi on pi.PackageIssuedID = t.PackageIssuedID inner join t_Vendor2Package vpk on pi.PackageID = vpk.PackageID inner join t_Vendor2Personnel vp on vpk.VendorID = vp.VendorID inner join t_Personnel p on vp.PersonnelID = p.PersonnelID where t.TourID = '" & tourid & "' " & _
                                ") x order by Rep"
            'ds.SelectCommand = "Select p.PersonnelID, p.FirstName + ' ' + p.LastName as Rep from t_Tour t inner join t_VendorCampaigns vc on t.CampaignID = vc.CRMSCampID inner join t_Vendor2Personnel vp on vc.VendorID = vp.VendorID inner join t_Personnel p on vp.PersonnelID = p.PersonnelID where t.TourID = '" & tourid & "' order by p.FirstName asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function


    Public Property RepID() As Integer
        Get
            Return _RepID
        End Get
        Set(ByVal value As Integer)
            _RepID = value
        End Set
    End Property

    Public Property TourID() As Integer
        Get
            Return _TourID
        End Get
        Set(ByVal value As Integer)
            _TourID = value
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

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property
    Public Property SolID() As Integer
        Get
            Return _SolID
        End Get
        Set(ByVal value As Integer)
            _SolID = value
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

    Public Property SaleLocID() As Integer
        Get
            Return _SaleLocID
        End Get
        Set(ByVal value As Integer)
            _SaleLocID = value
        End Set
    End Property

    Public Property ProsAccomID() As Integer
        Get
            Return _ProsAccomID
        End Get
        Set(ByVal value As Integer)
            _ProsAccomID = value
        End Set
    End Property

    Public Property PkgPrice() As Double
        Get
            Return _PkgPrice
        End Get
        Set(ByVal value As Double)
            _PkgPrice = value
        End Set
    End Property

    Public Property Paid() As Boolean
        Get
            Return _Paid
        End Get
        Set(ByVal value As Boolean)
            _Paid = value
        End Set
    End Property

    Public Property DatePaid() As String
        Get
            Return _DatePaid
        End Get
        Set(ByVal value As String)
            _DatePaid = value
        End Set
    End Property

    Public Property CB() As Boolean
        Get
            Return _CB
        End Get
        Set(ByVal value As Boolean)
            _CB = value
        End Set
    End Property

    Public Property DateCB() As String
        Get
            Return _DateCB
        End Get
        Set(ByVal value As String)
            _DateCB = value
        End Set
    End Property

    Public Property AmountPaid() As Double
        Get
            Return _AmountPaid
        End Get
        Set(ByVal value As Double)
            _AmountPaid = value
        End Set
    End Property

    Public Property AmountCB() As Double
        Get
            Return _AmountCB
        End Get
        Set(ByVal value As Double)
            _AmountCB = value
        End Set
    End Property

    Public Property Payable() As Boolean
        Get
            Return _Payable
        End Get
        Set(ByVal value As Boolean)
            _Payable = value
        End Set
    End Property

    Public Property Voicestamp() As String
        Get
            Return _Voicestamp
        End Get
        Set(ByVal value As String)
            _Voicestamp = value
        End Set
    End Property

    Public Property Quinella() As Boolean
        Get
            Return _Quinella
        End Get
        Set(ByVal value As Boolean)
            _Quinella = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

    Public Property RepTourID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
