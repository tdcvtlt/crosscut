Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsTour
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Dim _TourID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _TourLocationID As Integer = 0
    Dim _TourDate As String = ""
    Dim _TourTime As Integer = 0
    Dim _ArrivalTime As String = ""
    Dim _StartTime As String = ""
    Dim _EndTime As String = ""
    Dim _ResultID As Integer = 0
    Dim _CampaignID As Integer = 0
    Dim _BookingDate As String = ""
    Dim _StatusDate As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _SourceID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _ReferrerID As Integer = 0
    Dim _PackageIssuedID As Integer = 0
    Dim _CheckedIn As Boolean = False
    Dim _DebitCard As Boolean = False
    Dim _ReservationID As Integer = 0
    Dim _UserID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Replace", cn)
    End Sub

    Public Sub Load()
        Try
            Dim sql As String = "Select * from t_Tour where tourid = " & _TourID
            cm.CommandText = sql
            cn.Open()
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                Fill_Values()
            End If
            dread.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Fill_Values()
        Try
            _TourID = dread("TourID")
            _ProspectID = IIf(dread("ProspectID") Is System.DBNull.Value, 0, dread("ProspectID"))
            _LocationID = IIf(dread("LocationID") Is System.DBNull.Value, 0, dread("LocationID"))
            _TourLocationID = IIf(dread("TourLocationID") Is System.DBNull.Value, 0, dread("TourLocationID"))
            _TourDate = dread("TourDate") & ""

            'Surround code in block to prevent DBNull {04/12}
            If dread("TourTime").Equals(DBNull.Value) = False Then
                _TourTime = dread("TourTime")
            End If

            _ArrivalTime = dread("ArrivalTime") & ""
            _StartTime = dread("StartTime") & ""
            _EndTime = dread("EndTime") & ""
            _ResultID = IIf(dread("ResultID") Is System.DBNull.Value, 0, dread("ResultID"))
            _CampaignID = IIf(dread("CampaignID") Is System.DBNull.Value, 0, dread("CampaignID"))
            _BookingDate = dread("BookingDate") & ""
            _StatusDate = dread("StatusDate") & ""
            _TypeID = IIf(dread("TypeID") Is System.DBNull.Value, 0, dread("TypeID"))
            _SubTypeID = IIf(dread("SubTypeID") Is System.DBNull.Value, 0, dread("SubTypeID"))
            _SourceID = IIf(dread("SourceID") Is System.DBNull.Value, 0, dread("SourceID"))
            _StatusID = IIf(dread("StatusID") Is System.DBNull.Value, 0, dread("StatusID"))
            _ReferrerID = IIf(dread("ReferrerID") Is System.DBNull.Value, 0, dread("ReferrerID"))
            _PackageIssuedID = IIf(dread("PackageIssuedID") Is System.DBNull.Value, 0, dread("PackageIssuedID"))
            _CheckedIn = IIf(dread("CheckedIn") Is System.DBNull.Value, 0, dread("CheckedIn"))
            _DebitCard = IIf(dread("DebitCard") Is System.DBNull.Value, 0, dread("DebitCard"))
            _ReservationID = IIf(dread("ReservationID") Is System.DBNull.Value, 0, dread("ReservationID"))
            _CRMSID = IIf(dread("CRMSID") Is System.DBNull.Value, 0, dread("CRMSID"))
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Get_Prospect_Name(ByVal iTourID As Integer) As String
        Dim sName As String = ""
        Dim oPros As New clsProspect
        oPros.Prospect_ID = get_Prospect_ID(iTourID)
        oPros.Load()
        sName = oPros.Last_Name & ", " & oPros.First_Name
        oPros = Nothing
        Return sName
    End Function


    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Tour where TourID = " & _TourID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Tour")
            If ds.Tables("t_Tour").Rows.Count > 0 Then
                dr = ds.Tables("t_Tour").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_TourInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.Int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@TourLocationID", SqlDbType.Int, 0, "TourLocationID")
                da.InsertCommand.Parameters.Add("@TourDate", SqlDbType.SmallDateTime, 0, "TourDate")
                da.InsertCommand.Parameters.Add("@TourTime", SqlDbType.Int, 0, "TourTime")
                da.InsertCommand.Parameters.Add("@ArrivalTime", SqlDbType.SmallDateTime, 0, "ArrivalTime")
                da.InsertCommand.Parameters.Add("@StartTime", SqlDbType.SmallDateTime, 0, "StartTime")
                da.InsertCommand.Parameters.Add("@EndTime", SqlDbType.SmallDateTime, 0, "EndTime")
                da.InsertCommand.Parameters.Add("@ResultID", SqlDbType.Int, 0, "ResultID")
                da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.Int, 0, "CampaignID")
                da.InsertCommand.Parameters.Add("@BookingDate", SqlDbType.SmallDateTime, 0, "BookingDate")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.SmallDateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@SourceID", SqlDbType.Int, 0, "SourceID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@ReferrerID", SqlDbType.Int, 0, "ReferrerID")
                da.InsertCommand.Parameters.Add("@PackageIssuedID", SqlDbType.Int, 0, "PackageIssuedID")
                da.InsertCommand.Parameters.Add("@CheckedIn", SqlDbType.Bit, 0, "CheckedIn")
                da.InsertCommand.Parameters.Add("@DebitCard", SqlDbType.Bit, 0, "DebitCard")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.Int, 0, "ReservationID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TourID", SqlDbType.Int, 0, "TourID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Tour").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("TourLocationID", _TourLocationID, dr)
            Update_Field("TourDate", _TourDate, dr)
            Update_Field("TourTime", _TourTime, dr)
            Update_Field("ArrivalTime", _ArrivalTime, dr)
            Update_Field("StartTime", _StartTime, dr)
            Update_Field("EndTime", _EndTime, dr)
            Update_Field("ResultID", _ResultID, dr)
            Update_Field("CampaignID", _CampaignID, dr)
            Update_Field("BookingDate", _BookingDate, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("SourceID", _SourceID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("ReferrerID", _ReferrerID, dr)
            Update_Field("PackageIssuedID", _PackageIssuedID, dr)
            Update_Field("CheckedIn", _CheckedIn, dr)
            Update_Field("DebitCard", _DebitCard, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("ReservationID", _ReservationID, dr)
            If ds.Tables("t_Tour").Rows.Count < 1 Then ds.Tables("t_Tour").Rows.Add(dr)
            da.Update(ds, "t_Tour")

            If _TourID = 0 Then
                Dim oEvents As New clsEvents
                oEvents.Create_Create_Event("TourID", ds.Tables("t_Tour").Rows(0).Item("TourID"), 0, _UserID, "")
                oEvents = Nothing
            End If
            _TourID = ds.Tables("t_Tour").Rows(0).Item("TourID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "TourID"
            oEvents.KeyValue = _TourID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function get_Tour_Status(ByVal tourID As Integer) As String
        Dim tStatus As String = ""
        Try
            cm.CommandText = "Select ts.ComboItem as TourStatus from t_Tour t left outer join t_ComboItems ts on t.StatusID = ts.ComboItemID where t.TourID = '" & tourID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            tStatus = dread.GetValue(0).ToString & ""
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return tStatus
    End Function

    Public Function List(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " TourID as ID,  TourDate as [Tour Date], tt.Description as [Tour Time], c.name as Campaign, ts.comboitem as Status From t_Tour t left outer join t_Campaign c on c.campaignid = t.campaignid left outer join t_Comboitems ts on ts.comboitemid = t.statusid left outer join t_Comboitems tt on t.TourTime = tt.ComboitemID "
            sql += IIf(sFilterField <> "", " where " & sFilterField & " = '" & sFilterValue & "' ", "")
            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function Get_Tour_By_Res(ByVal resID As Integer) As Integer
        Dim tourID As Integer = 0
        Try
            cm.CommandText = "Select TourID from t_Tour where Reservationid = " & resID
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            If dread.HasRows Then
                tourID = dread("TourID")
            End If
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return tourID
    End Function

    Public Function todays_Tours(Optional ByVal loc As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select c.Name As Campaign, t.TourID, p.LastName + ', ' + p.FirstName As Prospect, tt.ComboItem as TourTime from t_Tour t inner join t_Prospect p on t.prospectid = p.prospectid inner join t_Campaign c on t.campaignid = c.campaignid inner join t_ComboItems tl on t.TourLocationID = tl.ComboItemID inner join t_ComboItems ts on t.StatusID = ts.ComboItemID left outer join t_ComboItems tt on t.TourTime = tt.comboitemid where tl.ComboItem = '" & loc & "' and t.TourDate = '" & System.DateTime.Now.ToShortDateString & "' and t.CheckedIn = '0' and ts.ComboItem = 'Booked' order by p.LastName, p.FirstName"
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function get_VacationClub_Tours() As SQLDataSource
        Dim ds As New sqldataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select t.TourID, tt.ComboItem as TourTime, t.TourDate, p.Lastname + ', ' + p.Firstname as Prospect from t_Tour t inner join t_prospect p on t.prospectid = p.prospectid left outer join t_ComboItems tt on t.tourTime = tt.ComboItemID left outer join t_combOitems tl on t.TourLocationID = tl.ComboItemID where tl.ComboItem = 'VacationClub' and t.CheckedIn = '1' order by p.LastName, p.FirstName"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function get_Exit_Tours(Optional ByVal loc As String = "") As sqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            'Dim sql As String = "Select Distinct(t.tourid), p.LastName + ', ' + p.FirstName as Prospect, tt.comboItem as TourTime, r.FirstName + ' ' + r.LastName as SalesRep from t_Tour t inner join t_Prospect p on t.prospectid = p.ProspectID inner join t_ComboItems tt on t.TourTime = tt.ComboItemID left outer join t_ComboItems tl on t.TourLocationID = tl.ComboItemID left outer join t_PersonnelTrans pt on t.tourid = pt.KeyValue left outer join t_ComboItems ptl on pt.TitleID = ptl.ComboItemID left outer join t_Personnel r on pt.PersonnelID = r.PersonnelID where t.tourDate = '" & System.DateTime.Now.ToShortDateString & "' and (ptl.ComboItem is null or (ptl.ComboItem = 'Sales Executive' and pt.KeyField = 'TourID')) and t.CheckedIn = '1' and tl.ComboItem = '" & loc & "' order by p.LastName +', '+ p.FirstName, tt.comboitem, r.firstname + ' ' + r.lastname"
            Dim sql As String = "Select Distinct(t.tourid), p.LastName + ', ' + p.FirstName as Prospect, tt.comboItem as TourTime, (Select top 1 pr.Firstname + ' ' + pr.LastName from t_PersonnelTrans pt inner join t_ComboItems ptt on pt.TitleID = ptt.ComboItemID inner join t_Personnel pr on pt.PersonnelID = pr.PersonnelID where pt.Keyfield = 'TourID' and pt.Keyvalue = t.TourID and ptt.ComboItem = 'Sales Executive') as SalesRep from t_Tour t inner join t_Prospect p on t.prospectid = p.ProspectID inner join t_ComboItems tt on t.TourTime = tt.ComboItemID left outer join t_ComboItems tl on t.TourLocationID = tl.ComboItemID where t.tourDate = '" & System.DateTime.Now.ToShortDateString & "' and t.CheckedIn = '1' and tl.ComboItem = '" & loc & "' order by p.LastName +', '+ p.FirstName, tt.comboitem"
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function get_Outstanding_Tours(Optional ByVal loc As String = "") As sqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select t.tourid, p.LastName + ', ' + p.FirstName as Prospect, tt.comboItem as TourTime, r.FirstName + ' ' + r.LastName as SalesRep from t_Tour t inner join t_Prospect p on t.prospectid = p.ProspectID inner join t_ComboItems tt on t.TourTime = tt.ComboItemID left outer join t_ComboItems tl on t.TourLocationID = tl.ComboItemID left outer join t_PersonnelTrans pt on t.tourid = pt.KeyValue left outer join t_ComboItems ptl on pt.TitleID = ptl.ComboItemID left outer join t_Personnel r on pt.PersonnelID = r.PersonnelID where t.tourDate < '" & System.DateTime.Now.ToShortDateString & "' and (ptl.ComboItem is null or (ptl.ComboItem = 'Sales Executive' and pt.KeyField = 'TourID')) and t.CheckedIn = '1' and tl.ComboItem = '" & loc & "' order by p.lastname, p.FirstName"
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function get_Campaign(ByVal tourID As Integer) As String
        Dim campaign As String = ""
        Try
            cm.CommandText = "Select c.name from t_Tour t inner join t_Campaign c on t.campaignid = c.campaignid where t.tourid = '" & tourID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader
            dread.Read()
            campaign = dread(0).ToString
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return campaign
    End Function
    Public Function get_Prospect_ID(ByVal tourID As Integer) As Integer
        Dim prosID As Integer = 0
        Try
            cm.CommandText = "Select ProspectID from t_Tour where tourid = '" & tourID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader
            dread.Read()
            prosID = dread("ProspectID")
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return prosID
    End Function

    Public Function val_TourID(ByVal ID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Tours from t_Tour where tourid = '" & ID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Tours") > 0 Then
                bValid = True
            Else
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function


    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property

    Public Property ReservationID As Integer
        Get
            Return _ReservationID
        End Get
        Set(ByVal value As Integer)
            _ReservationID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
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

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
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

    Public Property TourLocationID() As Integer
        Get
            Return _TourLocationID
        End Get
        Set(ByVal value As Integer)
            _TourLocationID = value
        End Set
    End Property

    Public Property TourDate() As String
        Get
            Return _TourDate
        End Get
        Set(ByVal value As String)
            _TourDate = value
        End Set
    End Property

    Public Property TourTime() As Integer
        Get
            Return _TourTime
        End Get
        Set(ByVal value As Integer)
            _TourTime = value
        End Set
    End Property

    Public Property ArrivalTime() As String
        Get
            Return _ArrivalTime
        End Get
        Set(ByVal value As String)
            _ArrivalTime = value
        End Set
    End Property

    Public Property StartTime() As String
        Get
            Return _StartTime
        End Get
        Set(ByVal value As String)
            _StartTime = value
        End Set
    End Property

    Public Property EndTime() As String
        Get
            Return _EndTime
        End Get
        Set(ByVal value As String)
            _EndTime = value
        End Set
    End Property

    Public Property ResultID() As Integer
        Get
            Return _ResultID
        End Get
        Set(ByVal value As Integer)
            _ResultID = value
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

    Public Property BookingDate() As String
        Get
            Return _BookingDate
        End Get
        Set(ByVal value As String)
            _BookingDate = value
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

    Public Property SourceID() As Integer
        Get
            Return _SourceID
        End Get
        Set(ByVal value As Integer)
            _SourceID = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            If value <> _StatusID Then
                _StatusDate = System.DateTime.Now.ToShortDateString
            End If
            _StatusID = value
        End Set
    End Property

    Public Property ReferrerID() As Integer
        Get
            Return _ReferrerID
        End Get
        Set(ByVal value As Integer)
            _ReferrerID = value
        End Set
    End Property

    Public Property PackageIssuedID() As Integer
        Get
            Return _PackageIssuedID
        End Get
        Set(ByVal value As Integer)
            _PackageIssuedID = value
        End Set
    End Property

    Public Property CheckedIn() As Boolean
        Get
            Return _CheckedIn
        End Get
        Set(ByVal value As Boolean)
            _CheckedIn = value
        End Set
    End Property

    Public Property DebitCard() As Boolean
        Get
            Return _DebitCard
        End Get
        Set(ByVal value As Boolean)
            _DebitCard = value
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
End Class
