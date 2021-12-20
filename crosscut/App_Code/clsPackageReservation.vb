Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageReservation
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _PromoNights As Integer = 0
    Dim _PromoRate As Decimal = 0
    Dim _SourceID As Integer = 0
    Dim _CreateTour As Boolean = False
    Dim _CRMSID As Integer = 0
    Dim _ResortCompanyID As Integer = 0
    Dim _TypeID As Int32
    Dim _AllowExtraNight As Boolean = False

    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageReservation where PackageReservationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageReservation where PackageReservationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservation")
            If ds.Tables("t_PackageReservation").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservation").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("PromoNights") Is System.DBNull.Value) Then _PromoNights = dr("PromoNights")
        If Not (dr("PromoRate") Is System.DBNull.Value) Then _PromoRate = dr("PromoRate")
        If Not (dr("SourceID") Is System.DBNull.Value) Then _SourceID = dr("SourceID")
        If Not (dr("CreateTour") Is System.DBNull.Value) Then _CreateTour = dr("CreateTour")
        If Not (dr("ResortCompanyID") Is System.DBNull.Value) Then _ResortCompanyID = dr("ResortCompanyID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("AllowExtraNight") Is System.DBNull.Value) Then _AllowExtraNight = dr("AllowExtraNight")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageReservation where PackageReservationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservation")
            If ds.Tables("t_PackageReservation").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservation").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageReservationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@PromoNights", SqlDbType.int, 0, "PromoNights")
                da.InsertCommand.Parameters.Add("@PromoRate", SqlDbType.money, 0, "PromoRate")
                da.InsertCommand.Parameters.Add("@CreateTour", SqlDbType.bit, 0, "CreateTour")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@SourceID", SqlDbType.Int, 0, "SourceID")
                da.InsertCommand.Parameters.Add("@ResortCompanyID", SqlDbType.Int, 0, "ResortCompanyID")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@AllowExtraNight", SqlDbType.Bit, 0, "AllowExtraNight")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageReservationID", SqlDbType.Int, 0, "PackageReservationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageReservation").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("PromoNights", _PromoNights, dr)
            Update_Field("PromoRate", _PromoRate, dr)
            Update_Field("CreateTour", _CreateTour, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("SourceID", _SourceID, dr)
            Update_Field("ResortCompanyID", _ResortCompanyID, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("AllowExtraNight", _AllowExtraNight, dr)
            If ds.Tables("t_PackageReservation").Rows.Count < 1 Then ds.Tables("t_PackageReservation").Rows.Add(dr)
            da.Update(ds, "t_PackageReservation")
            _ID = ds.Tables("t_PackageReservation").Rows(0).Item("PackageReservationID")
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
            oEvents.KeyField = "PackageReservationID"
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

    Public Function Find_Res_ID(ByVal pkgID As Integer) As Integer
        Dim pkgResID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select PackageReservationID from t_packageReservation where packageid = " & pkgID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                pkgResID = dread("PackageReservationID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return pkgResID
    End Function
    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
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

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
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

    Public Property CreateTour() As Boolean
        Get
            Return _CreateTour
        End Get
        Set(ByVal value As Boolean)
            _CreateTour = value
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

    Public Property PackageReservationID() As Integer
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

    Public Property SourceID() As Integer
        Get
            Return _SourceID
        End Get
        Set(ByVal value As Integer)
            _SourceID = value
        End Set
    End Property

    Public Property ResortCompanyID() As Integer
        Get
            Return _ResortCompanyID
        End Get
        Set(ByVal value As Integer)
            _ResortCompanyID = value
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

    Public Property AllowExtraNight() As Boolean
        Get
            Return _AllowExtraNight
        End Get
        Set(value As Boolean)
            _AllowExtraNight = value
        End Set
    End Property

End Class

