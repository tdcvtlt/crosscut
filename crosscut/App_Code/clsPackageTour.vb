Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageTour
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PackageReservationID As Integer = 0
    Dim _TourLocationID As Integer = 0
    Dim _CampaignID As Integer = 0
    Dim _TourTypeID As Integer = 0
    Dim _TourSubTypeID As Integer = 0
    Dim _TourSourceID As Integer = 0
    Dim _TourStatusID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageTour where PackageTourID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageTour where PackageTourID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageTour")
            If ds.Tables("t_PackageTour").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTour").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Sub Load(packageID As Int32, packageReservationID As Int32)
        Try
            cm.CommandText = String.Format("Select * from t_PackageTour where PackageID = {0} and PackageReservationID = {1}", packageID, packageReservationID)
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageTour")
            If ds.Tables("t_PackageTour").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTour").Rows(0)
                _ID = ds.Tables("t_PackageTour").Rows(0).Item("PackageTourID")
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("PackageReservationID") Is System.DBNull.Value) Then _PackageReservationID = dr("PackageReservationID")
        If Not (dr("TourLocationID") Is System.DBNull.Value) Then _TourLocationID = dr("TourLocationID")
        If Not (dr("CampaignID") Is System.DBNull.Value) Then _CampaignID = dr("CampaignID")
        If Not (dr("TourTypeID") Is System.DBNull.Value) Then _TourTypeID = dr("TourTypeID")
        If Not (dr("TourSubTypeID") Is System.DBNull.Value) Then _TourSubTypeID = dr("TourSubTypeID")
        If Not (dr("TourSourceID") Is System.DBNull.Value) Then _TourSourceID = dr("TourSourceID")
        If Not (dr("TourStatusID") Is System.DBNull.Value) Then _TourStatusID = dr("TourStatusID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageTour where PackageTourID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageTour")
            If ds.Tables("t_PackageTour").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTour").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageTourInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PackageReservationID", SqlDbType.int, 0, "PackageReservationID")
                da.InsertCommand.Parameters.Add("@TourLocationID", SqlDbType.int, 0, "TourLocationID")
                da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.int, 0, "CampaignID")
                da.InsertCommand.Parameters.Add("@TourTypeID", SqlDbType.int, 0, "TourTypeID")
                da.InsertCommand.Parameters.Add("@TourSubTypeID", SqlDbType.int, 0, "TourSubTypeID")
                da.InsertCommand.Parameters.Add("@TourSourceID", SqlDbType.int, 0, "TourSourceID")
                da.InsertCommand.Parameters.Add("@TourStatusID", SqlDbType.int, 0, "TourStatusID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageTourID", SqlDbType.Int, 0, "PackageTourID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageTour").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PackageReservationID", _PackageReservationID, dr)
            Update_Field("TourLocationID", _TourLocationID, dr)
            Update_Field("CampaignID", _CampaignID, dr)
            Update_Field("TourTypeID", _TourTypeID, dr)
            Update_Field("TourSubTypeID", _TourSubTypeID, dr)
            Update_Field("TourSourceID", _TourSourceID, dr)
            Update_Field("TourStatusID", _TourStatusID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_PackageTour").Rows.Count < 1 Then ds.Tables("t_PackageTour").Rows.Add(dr)
            da.Update(ds, "t_PackageTour")
            _ID = ds.Tables("t_PackageTour").Rows(0).Item("PackageTourID")
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
            oEvents.KeyField = "PackageTourID"
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

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property PackageReservationID() As Integer
        Get
            Return _PackageReservationID
        End Get
        Set(ByVal value As Integer)
            _PackageReservationID = value
        End Set
    End Property

    Public Property TourLocationID() As Integer
        Get
            Return _TourLocationID
        End Get
        Set(ByVal value As Integer)
            _TourLocationID = value
        End Set
    End Property

    Public Property CampaignID() As Integer
        Get
            Return _CampaignID
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
        End Set
    End Property

    Public Property TourTypeID() As Integer
        Get
            Return _TourTypeID
        End Get
        Set(ByVal value As Integer)
            _TourTypeID = value
        End Set
    End Property

    Public Property TourSubTypeID() As Integer
        Get
            Return _TourSubTypeID
        End Get
        Set(ByVal value As Integer)
            _TourSubTypeID = value
        End Set
    End Property

    Public Property TourSourceID() As Integer
        Get
            Return _TourSourceID
        End Get
        Set(ByVal value As Integer)
            _TourSourceID = value
        End Set
    End Property

    Public Property TourStatusID() As Integer
        Get
            Return _TourStatusID
        End Get
        Set(ByVal value As Integer)
            _TourStatusID = value
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

    Public Property PackageTourID() As Integer
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
End Class
