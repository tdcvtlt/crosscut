Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsTourWaveLimits
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CampaignTypeID As Integer
    Dim _WaveTimeID As Integer
    Dim _MaxLimit As Integer
    Dim _EffectiveStartDate As String = ""
    Dim _EffectiveEndDate As String = ""
    Dim _TourLocationID As Integer = 0
    Dim _Last_Modified As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_TourWaveLimits where TourWaveLimitsID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_TourWaveLimits where TourWaveLimitsID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_TourWaveLimits")
            If ds.Tables("t_TourWaveLimits").Rows.Count > 0 Then
                dr = ds.Tables("t_TourWaveLimits").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()

        If Not (dr("CampaignTypeID") Is System.DBNull.Value) Then _CampaignTypeID = dr("CampaignTypeID")
        If Not (dr("WaveTimeID") Is System.DBNull.Value) Then _WaveTimeID = dr("WaveTimeID")
        If Not (dr("MaxLimit") Is System.DBNull.Value) Then _MaxLimit = dr("MaxLimit")
        If Not (dr("EffectiveStartDate") Is System.DBNull.Value) Then _EffectiveStartDate = dr("EffectiveStartDate")
        If Not (dr("EffectiveEndDate") Is System.DBNull.Value) Then _EffectiveEndDate = dr("EffectiveEndDate")
        If Not (dr("TourLocationID") Is System.DBNull.Value) Then _TourLocationID = dr("TourLocationID")

    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_TourWaveLimits where TourWaveLimitsID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_TourWaveLimits")
            If ds.Tables("t_TourWaveLimits").Rows.Count > 0 Then
                dr = ds.Tables("t_TourWaveLimits").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_TourWaveLimitsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CampaignTypeID", SqlDbType.smallint, 0, "CampaignTypeID")
                da.InsertCommand.Parameters.Add("@WaveTimeID", SqlDbType.smallint, 0, "WaveTimeID")
                da.InsertCommand.Parameters.Add("@MaxLimit", SqlDbType.tinyint, 0, "MaxLimit")
                da.InsertCommand.Parameters.Add("@EffectiveStartDate", SqlDbType.smalldatetime, 0, "EffectiveStartDate")
                da.InsertCommand.Parameters.Add("@EffectiveEndDate", SqlDbType.smalldatetime, 0, "EffectiveEndDate")
                da.InsertCommand.Parameters.Add("@Last_Modified", SqlDbType.SmallDateTime, 0, "Last_Modified")
                da.InsertCommand.Parameters.Add("@TourLocationID", SqlDbType.Int, 4, "TourLocationID")

                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TourWaveLimitsID", SqlDbType.Int, 0, "TourWaveLimitsID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_TourWaveLimits").NewRow
            End If
            Update_Field("CampaignTypeID", _CampaignTypeID, dr)
            Update_Field("WaveTimeID", _WaveTimeID, dr)
            Update_Field("MaxLimit", _MaxLimit, dr)
            Update_Field("EffectiveStartDate", _EffectiveStartDate, dr)
            Update_Field("EffectiveEndDate", _EffectiveEndDate, dr)
            Update_Field("Last_Modified", DateTime.Now.ToLongTimeString(), dr)
            Update_Field("TourLocationID", _TourLocationID, dr)

            If ds.Tables("t_TourWaveLimits").Rows.Count < 1 Then ds.Tables("t_TourWaveLimits").Rows.Add(dr)
            da.Update(ds, "t_TourWaveLimits")
            _ID = ds.Tables("t_TourWaveLimits").Rows(0).Item("TourWaveLimitsID")
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
            oEvents.KeyField = "TourWaveLimitsID"
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

    Public Property CampaignTypeID() As Integer
        Get
            Return _CampaignTypeID
        End Get
        Set(ByVal value As Integer)
            _CampaignTypeID = value
        End Set
    End Property

    Public Property WaveTimeID() As Integer
        Get
            Return _WaveTimeID
        End Get
        Set(ByVal value As Integer)
            _WaveTimeID = value
        End Set
    End Property

    Public Property MaxLimit() As Integer
        Get
            Return _MaxLimit
        End Get
        Set(ByVal value As Integer)
            _MaxLimit = value
        End Set
    End Property

    Public Property EffectiveStartDate() As String
        Get
            Return _EffectiveStartDate
        End Get
        Set(ByVal value As String)
            _EffectiveStartDate = value
        End Set
    End Property

    Public Property EffectiveEndDate() As String
        Get
            Return _EffectiveEndDate
        End Get
        Set(ByVal value As String)
            _EffectiveEndDate = value
        End Set
    End Property

    Public Property Last_Modified() As String
        Get
            Return _Last_Modified
        End Get
        Set(ByVal value As String)
            _Last_Modified = value
        End Set
    End Property

    Public Property TourLocationID As Integer
        Get
            Return _TourLocationID
        End Get
        Set(value As Integer)
            _TourLocationID = value
        End Set
    End Property

    ''' <summary>
    ''' Table primary key
    ''' </summary>    
    Public Property TourWaveLimitsID() As Integer
        Set(ByVal value As Integer)
            _ID = value
        End Set
        Get
            Return _ID
        End Get
    End Property


    Public ReadOnly Property GetCampaignTypes As System.Data.DataTable
        Get

            Using ada = New SqlDataAdapter( _
                "select ct.ComboItem [campaign_type_name], ct.ComboItemID [ID]" & _
                "from t_Campaign c inner join t_ComboItems ct on c.TypeID = ct.ComboItemID " & _
                "where (c.TypeID > 0 Or c.TypeID Is Not null) " & _
                "group by ct.ComboItem, ct.ComboItemID", _
                cn)

                Dim dt = New DataTable()
                Try
                    ada.Fill(dt)

                Catch ex As Exception
                    Throw New Exception()
                End Try

                Return dt
            End Using
        End Get
    End Property


    ''' <summary>
    ''' Returns all active tour locations
    ''' </summary>
    Public Function GetLocations() As System.Data.DataTable

        Using ada = New SqlDataAdapter( _
                        "select comboItem, comboItemID from t_comboitems where comboid = 270 and active = 1 order by comboitem", cn)

            Dim dt = New DataTable()
            Try
                ada.Fill(dt)
            Catch ex As Exception
                Throw New Exception()
            End Try

            Return dt
        End Using

    End Function

    Public ReadOnly Property GetTourWaves As System.Data.DataTable
        Get

            Using ada = New SqlDataAdapter( _
                "select ComboItemID, Description from t_ComboItems " & _
                "where(ComboID = 846 And Active = 1) order by CONVERT(smallint, comboitem)", cn)

                Dim dt = New DataTable()
                Try
                    ada.Fill(dt)

                Catch ex As Exception
                    Throw New Exception()
                End Try

                Return dt
            End Using

        End Get
    End Property

    ''' <summary>
    ''' Retrieves past records stopped at 1 day prior today's date.
    ''' </summary>
    Public Function GetViewLimits(startDate As DateTime, tourLocationId As Integer) As System.Data.DataTable

        Dim addedum = "and t.statusid in (17020, 17011)"

        Using ada = New SqlDataAdapter( _
            String.Format("select (select comboitem from t_ComboItems where ComboItemID = twl.CampaignTypeID) [campaign_type_name], " & _
            "tw.Description [tour_wave],  convert(varchar, twl.EffectiveStartDate, 101) StartDate, convert(varchar, twl.EffectiveEndDate, 101) EndDate, " & _
            "(select count(c.typeid) from t_campaign c inner join t_tour t on c.campaignid = t.campaignid " & _
            "where c.typeid = twl.campaigntypeid and t.tourdate between twl.effectiveStartDate and " & _
            "twl.effectiveEndDate and t.tourTime = twl.WaveTimeID {2} and t.tourlocationid = {1}) [c], " & _
            "(select comboItem from t_comboItems where comboItemID = twl.tourLocationID) Location, *" & _
            "from t_TourWaveLimits twl inner join t_ComboItems tw on twl.WaveTimeID = ComboItemID " & _
            "where CONVERT(smalldatetime, CONVERT(varchar, twl.EffectiveStartDate, 101)) between '{0}' and " & _
            "convert(smalldatetime, getdate() - 1, 101) and twl.tourLocationID = {1} " & _
            "order by [campaign_type_name],  " & _
            "CONVERT(smalldatetime, CONVERT(varchar, twl.EffectiveStartDate, 101)), " & _
            "convert(smallint, tw.comboItem)", startDate, tourLocationId, addedum), cn)

            Dim dt = New DataTable()
            Try
                ada.Fill(dt)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' Records retrieved from particular Start Date and campaign type name
    ''' </summary>
    Public Function GetViewLimits(campaignId As Integer, startDate As DateTime, tourLocationId As Integer) As System.Data.DataTable

        Dim addedum = "and t.statusid in (16995)"

        Dim sql = String.Format("select (select comboitem from t_ComboItems where ComboItemID = twl.CampaignTypeID) [campaign_type_name], " & _
            "tw.Description [tour_wave],  convert(varchar, twl.EffectiveStartDate, 101) StartDate, convert(varchar, twl.EffectiveEndDate, 101) EndDate, " & _
            "(select count(c.typeid) from t_campaign c inner join t_tour t on c.campaignid = t.campaignid " & _
            "where c.typeid = twl.campaigntypeid and t.tourdate between twl.effectiveStartDate and " & _
            "twl.effectiveEndDate and t.tourTime = twl.WaveTimeID {2} and t.tourlocationid = {3}) [c], " & _
            "(select comboItem from t_comboItems where comboItemID = twl.tourLocationID) Location, *" & _
            "from t_TourWaveLimits twl inner join t_ComboItems tw on twl.WaveTimeID = ComboItemID " & _
            "where CONVERT(smalldatetime, CONVERT(varchar, twl.EffectiveStartDate, 101)) >= '{1}'  " & _
            "and twl.CampaignTypeID = {0} and twl.tourLocationID = {3} " & _
            "order by [campaign_type_name],   " & _
            "CONVERT(smalldatetime, CONVERT(varchar, twl.EffectiveStartDate, 101)), " & _
            "convert(smallint, tw.comboItem)", campaignId, startDate, addedum, tourLocationId)

        Using ada = New SqlDataAdapter(sql, cn)

            Dim dt = New DataTable()
            Try
                ada.Fill(dt)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return dt
        End Using
    End Function


    ''' <summary>
    ''' Check the existence for particular campaign type name, tour wave, start date and end date 
    ''' </summary>
    Public Function Exists(campaignTypeID As Integer, waveID As Integer, startDate As DateTime, endDate As DateTime, tourLocationId As Integer) As Boolean
        Using ada = New SqlDataAdapter(String.Format( _
                                       "select * from t_TourWaveLimits twl where effectiveStartDate = '{0}' " & _
                                       "and effectiveEndDate = '{1}' and twl.waveTimeID = {2} and twl.campaignTypeID = {3} and twl.tourLocationID = {4}", _
                                       startDate, endDate, waveID, campaignTypeID, tourLocationId), cn)

            Dim dt = New DataTable()
            Try

                ada.Fill(dt)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return IIf(dt.Rows.Count > 0, True, False)
        End Using
    End Function

    Public Function IsLimitReached(campaignTypeID As Integer, waveID As Integer, startDate As DateTime, endDate As DateTime, tourLocationId As Integer) As Integer
        Dim addedum = IIf(DateTime.Now.CompareTo(startDate) <= 0, "t.statusid in (select ci.ComboItemID from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName = 'tourstatus' and ci.ComboItem in ('booked'))", "t.statusid in (select ci.ComboItemID from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName = 'tourstatus' and ci.ComboItem in ('OnTour', 'Showed'))")
        Dim ret_val = 0
        Dim sql = String.Format("select (Select MaxLimit from  t_TourWaveLimits twl " & _
                    "where EffectiveStartDate = '{0}' and effectiveEndDate = '{1}' " & _
                    "and twl.WaveTimeID = {2} and twl.CampaignTypeID = {3} and twl.TourLocationID = {5} ) - " & _
                    "(select COUNT(*) from t_Tour t where CampaignID in (select CampaignID from t_Campaign where TypeID = {3}) " & _
                    "and t.TourDate = '{0}' and t.TourTime = {2} and {4} and t.tourlocationid = {5})", _
                    startDate, endDate, waveID, campaignTypeID, addedum, tourLocationId)

        Using ada = New SqlDataAdapter(sql, cn)
            Try
                cn.Open()
                Dim o = ada.SelectCommand.ExecuteScalar()
                If o.Equals(DBNull.Value) Then
                    ret_val = Integer.MinValue
                Else
                    ret_val = DirectCast(o, Integer)
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                cn.Close()
            End Try
        End Using
        Return ret_val
    End Function

    Public Function GetCampaignTypeID(campaignID As Integer) As Integer

        Dim sql = String.Format("select typeid from t_campaign where campaignid = {0}", campaignID)

        Using ada = New SqlDataAdapter(sql, cn)

            Try
                cn.Open()
                Dim o = ada.SelectCommand.ExecuteScalar()
                cn.Close()
                If o Is Nothing Then
                    Return 0
                Else
                    Return DirectCast(o, Integer)
                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Using
    End Function



    Public Function GetCampaignNames(type_id As Integer) As DataTable

        Dim sql = String.Format("select Name from t_Campaign c where TypeID = {0} and Active = 1 order by Name", type_id)

        Using ada = New SqlDataAdapter(sql, cn)

            Try
                Dim dt = New DataTable()

                ada.Fill(dt)
                Return dt

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Using
    End Function


    Public Function GetData(eff_date_start As Date, eff_date_end As Date, tour_location_id As Int32, wave_time_id As Int32, campaign_type_id As Int32) As DataTable
        Dim dt = New DataTable()
        Dim sqlText = String.Format("select TourWaveLimitsID [ID],  cpt.comboitem [Campaign Type], " & _
                        "wt.comboitem [Wave Time], tl.comboitem [Tour Location], MaxLimit [Max Limit], " & _
                        "convert(varchar(10), EffectiveStartDate, 101) [Start Date], convert(varchar(10), EffectiveEndDate, 101) [End Date] " & _
                        "from t_TourWaveLimits twl " & _
                        "inner join t_ComboItems cpt on cpt.comboitemid = twl.campaigntypeid " & _
                        "inner join t_ComboItems wt on wt.comboitemid = twl.WaveTimeID " & _
                        "inner join t_ComboItems tl on tl.comboitemid = twl.TourLocationID " & _
                        "where tourlocationid in ({0}) and campaigntypeid in ({1}) and WaveTimeID " & _
                        "in ({2}) and EffectiveStartDate between '{3}' and '{4}'", _
                        tour_location_id, _
                        campaign_type_id, _
                        wave_time_id, _
                        eff_date_start, _
                        eff_date_end)

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)
                Try

                    ad.Fill(dt)
                Catch ex As Exception
                    HttpContext.Current.Response.Write(String.Format("{0}", ex.Message))
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        Return dt
    End Function

    Public Function UpdateData(limit_max As Int16, tour_wave_limit_id() As String) As Int16
        Dim record_affected = 0

        Dim sqlText = String.Format("select * from t_tourwavelimits where TourWaveLimitsID in ({0})", String.Join(",", tour_wave_limit_id))

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)

                Try
                    Dim cb = New SqlCommandBuilder(ad)
                    Dim dt = New DataTable()
                    ad.Fill(dt)
                    For Each dr As DataRow In dt.Rows
                        dr("MaxLimit") = limit_max
                        dr("last_modified") = DateTime.Now
                    Next
                    ad.Update(dt)
                    record_affected = dt.Rows.Count
                Catch ex As Exception
                    HttpContext.Current.Response.Write(String.Format("{0}", ex.Message))
                End Try               
            End Using
        End Using
        Return record_affected
    End Function



End Class
