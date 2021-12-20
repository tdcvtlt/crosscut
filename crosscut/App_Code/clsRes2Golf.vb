Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web.UI.WebControls

Public Class clsRes2Golf
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ReservationID As Integer = 0
    Dim _GolfID As Integer = 0
    Dim _Guests As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _CreatedByID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _StatusDate As String = ""
    Dim _GolfDate As String = ""
    Dim _TeeTime As String = ""
    Dim _ContactNumber As String = ""
    Dim _CancelContact As String = ""
    Dim _BookingContact As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Res2Golf where Res2GolfID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Res2Golf where Res2GolfID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Res2Golf")
            If ds.Tables("t_Res2Golf").Rows.Count > 0 Then
                dr = ds.Tables("t_Res2Golf").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ReservationID") Is System.DBNull.Value) Then _ReservationID = dr("ReservationID")
        If Not (dr("GolfID") Is System.DBNull.Value) Then _GolfID = dr("GolfID")
        If Not (dr("Guests") Is System.DBNull.Value) Then _Guests = dr("Guests")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("GolfDate") Is System.DBNull.Value) Then _GolfDate = dr("GolfDate")
        If Not (dr("TeeTime") Is System.DBNull.Value) Then _TeeTime = dr("TeeTime")
        If Not (dr("ContactNumber") Is System.DBNull.Value) Then _ContactNumber = dr("ContactNumber")
        If Not (dr("CancelContact") Is System.DBNull.Value) Then _CancelContact = dr("CancelContact")
        If Not (dr("BookingContact") Is System.DBNull.Value) Then _BookingContact = dr("BookingContact")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Res2Golf where Res2GolfID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Res2Golf")
            If ds.Tables("t_Res2Golf").Rows.Count > 0 Then
                dr = ds.Tables("t_Res2Golf").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Res2GolfInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.Int, 0, "ReservationID")
                da.InsertCommand.Parameters.Add("@GolfID", SqlDbType.Int, 0, "GolfID")
                da.InsertCommand.Parameters.Add("@Guests", SqlDbType.Int, 0, "Guests")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.Int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@GolfDate", SqlDbType.Date, 0, "GolfDate")
                da.InsertCommand.Parameters.Add("@TeeTime", SqlDbType.VarChar, 0, "TeeTime")
                da.InsertCommand.Parameters.Add("@ContactNumber", SqlDbType.VarChar, 0, "ContactNumber")
                da.InsertCommand.Parameters.Add("@CancelContact", SqlDbType.VarChar, 0, "CancelContact")
                da.InsertCommand.Parameters.Add("@BookingContact", SqlDbType.VarChar, 0, "BookingContact")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Res2GolfID", SqlDbType.Int, 0, "Res2GolfID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Res2Golf").NewRow
            End If
            Update_Field("ReservationID", _ReservationID, dr)
            Update_Field("GolfID", _GolfID, dr)
            Update_Field("Guests", _Guests, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("GolfDate", _GolfDate, dr)
            Update_Field("TeeTime", _TeeTime, dr)
            Update_Field("ContactNumber", _ContactNumber, dr)
            Update_Field("CancelContact", _CancelContact, dr)
            Update_Field("BookingContact", _BookingContact, dr)
            If ds.Tables("t_Res2Golf").Rows.Count < 1 Then ds.Tables("t_Res2Golf").Rows.Add(dr)
            da.Update(ds, "t_Res2Golf")
            _ID = ds.Tables("t_Res2Golf").Rows(0).Item("Res2GolfID")
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
            oEvents.KeyField = "Res2GolfID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Get_Tee_Times(ByVal resID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select rg.Res2GolfID as ID, g.Course, rg.GolfDate as Date, rg.TeeTime, rg.Guests, rgs.ComboItem as Status from t_Res2Golf rg inner join t_GolfCourse g on rg.GOlfID = g.GolfID inner join t_ComboItems rgs on rg.StatusID = rgs.ComboItemID where rg.ReservationID = '" & resID & "'"
        Return ds
    End Function

    Public Property ReservationID() As Integer
        Get
            Return _ReservationID
        End Get
        Set(ByVal value As Integer)
            _ReservationID = value
        End Set
    End Property

    Public Property GolfID() As Integer
        Get
            Return _GolfID
        End Get
        Set(ByVal value As Integer)
            _GolfID = value
        End Set
    End Property

    Public Property Guests() As Integer
        Get
            Return _Guests
        End Get
        Set(ByVal value As Integer)
            _Guests = value
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

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property GolfDate() As String
        Get
            Return _GolfDate
        End Get
        Set(ByVal value As String)
            _GolfDate = value
        End Set
    End Property

    Public Property TeeTime() As String
        Get
            Return _TeeTime
        End Get
        Set(ByVal value As String)
            _TeeTime = value
        End Set
    End Property

    Public Property ContactNumber() As String
        Get
            Return _ContactNumber
        End Get
        Set(ByVal value As String)
            _ContactNumber = value
        End Set
    End Property

    Public Property CancelContact() As String
        Get
            Return _CancelContact
        End Get
        Set(value As String)
            _CancelContact = value
        End Set
    End Property

    Public Property BookingContact() As String
        Get
            Return _BookingContact
        End Get
        Set(value As String)
            _BookingContact = value
        End Set
    End Property

    Public Property Res2GolfID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
