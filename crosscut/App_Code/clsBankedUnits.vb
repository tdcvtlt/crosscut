Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.IO

Public Class clsBankedUnits
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _UnitTypeID As Integer = 0
    Dim _UnitSize As String = ""
    Dim _SeasonID As Integer = 0
    Dim _ExchangeID As Integer = 0
    Dim _MembershipNumber As String = ""
    Dim _StatusID As Integer = 0
    Dim _UsageYear As Integer = 0
    Dim _DateDeposited As String = ""
    Dim _DepositYear As Integer = 0
    Dim _WeekDeposited As Integer = 0
    Dim _NoteID As Integer = 0
    Dim _CreatedByID As Integer = 0
    Dim _ConfirmationNumber As String = ""
    Dim _FrequencyID As Integer = 0
    Dim _DepositedByID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _CRMSID As Integer = 0
    Dim _UsageID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_BankedUnits where DepositID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_BankedUnits where DepositID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_BankedUnits")
            If ds.Tables("t_BankedUnits").Rows.Count > 0 Then
                dr = ds.Tables("t_BankedUnits").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("UnitSize") Is System.DBNull.Value) Then _UnitSize = dr("UnitSize")
        If Not (dr("SeasonID") Is System.DBNull.Value) Then _SeasonID = dr("SeasonID")
        If Not (dr("ExchangeID") Is System.DBNull.Value) Then _ExchangeID = dr("ExchangeID")
        If Not (dr("MembershipNumber") Is System.DBNull.Value) Then _MembershipNumber = dr("MembershipNumber")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("UsageYear") Is System.DBNull.Value) Then _UsageYear = dr("UsageYear")
        If Not (dr("DateDeposited") Is System.DBNull.Value) Then _DateDeposited = dr("DateDeposited")
        If Not (dr("DepositYear") Is System.DBNull.Value) Then _DepositYear = dr("DepositYear")
        If Not (dr("WeekDeposited") Is System.DBNull.Value) Then _WeekDeposited = dr("WeekDeposited")
        If Not (dr("NoteID") Is System.DBNull.Value) Then _NoteID = dr("NoteID")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("ConfirmationNumber") Is System.DBNull.Value) Then _ConfirmationNumber = dr("ConfirmationNumber")
        If Not (dr("FrequencyID") Is System.DBNull.Value) Then _FrequencyID = dr("FrequencyID")
        If Not (dr("DepositedByID") Is System.DBNull.Value) Then _DepositedByID = dr("DepositedByID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("UsageID") Is System.DBNull.Value) Then _UsageID = dr("UsageID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_BankedUnits where DepositID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_BankedUnits")
            If ds.Tables("t_BankedUnits").Rows.Count > 0 Then
                dr = ds.Tables("t_BankedUnits").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_BankedUnitsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.Int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.Int, 0, "UnitTypeID")
                da.InsertCommand.Parameters.Add("@UnitSize", SqlDbType.VarChar, 0, "UnitSize")
                da.InsertCommand.Parameters.Add("@SeasonID", SqlDbType.Int, 0, "SeasonID")
                da.InsertCommand.Parameters.Add("@ExchangeID", SqlDbType.Int, 0, "ExchangeID")
                da.InsertCommand.Parameters.Add("@MembershipNumber", SqlDbType.VarChar, 0, "MembershipNumber")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@UsageYear", SqlDbType.Int, 0, "UsageYear")
                da.InsertCommand.Parameters.Add("@DateDeposited", SqlDbType.DateTime, 0, "DateDeposited")
                da.InsertCommand.Parameters.Add("@DepositYear", SqlDbType.Int, 0, "DepositYear")
                da.InsertCommand.Parameters.Add("@WeekDeposited", SqlDbType.Int, 0, "WeekDeposited")
                da.InsertCommand.Parameters.Add("@NoteID", SqlDbType.Int, 0, "NoteID")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.Int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@ConfirmationNumber", SqlDbType.VarChar, 0, "ConfirmationNumber")
                da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.Int, 0, "FrequencyID")
                da.InsertCommand.Parameters.Add("@DepositedByID", SqlDbType.Int, 0, "DepositedByID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@UsageID", SqlDbType.Int, 0, "UsageID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@DepositID", SqlDbType.Int, 0, "DepositID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_BankedUnits").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("UnitTypeID", _UnitTypeID, dr)
            Update_Field("UnitSize", _UnitSize, dr)
            Update_Field("SeasonID", _SeasonID, dr)
            Update_Field("ExchangeID", _ExchangeID, dr)
            Update_Field("MembershipNumber", _MembershipNumber, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("UsageYear", _UsageYear, dr)
            Update_Field("DateDeposited", _DateDeposited, dr)
            Update_Field("DepositYear", _DepositYear, dr)
            Update_Field("WeekDeposited", _WeekDeposited, dr)
            Update_Field("NoteID", _NoteID, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("ConfirmationNumber", _ConfirmationNumber, dr)
            Update_Field("FrequencyID", _FrequencyID, dr)
            Update_Field("DepositedByID", _DepositedByID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("UsageID", _UsageID, dr)
            If ds.Tables("t_BankedUnits").Rows.Count < 1 Then ds.Tables("t_BankedUnits").Rows.Add(dr)
            da.Update(ds, "t_BankedUnits")
            _ID = ds.Tables("t_BankedUnits").Rows(0).Item("DepositID")
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
            oEvents.KeyField = "DepositID"
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

    Public Function List_BankedUnits(ByVal user As String, ByVal exchComp As String, ByVal status As String, ByVal sDate As Date, ByVal eDate As Date, ByVal conNum As String, ByVal dateSort As Boolean) As SqlDataSource


        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If dateSort = True Then

                'Dim tmp As String = "Select d.DepositID, d.ProspectID, d.ContractID, p.LastName + ', ' + p.FirstName as Owner, c.ContractNumber, ds.ComboItem as Status, d.StatusDate, cb.UserName as CreatedBy, db.UserName as DepositedBy, d.UsageYear, d.DepositYear, ut.ComboItem as UnitType, d.MemberShipNumber, d.DateDeposited, d.UnitSize, rm.RoomNumber, s.ComboItem as Usage, f.Frequency as Frequency, ec.ComboItem as ExchangeCompany from t_bankedUnits d inner join t_prospect p on d.ProspectiD = p.ProspectiD inner join t_Contract c on d.ContractID = c.ContractID left outer join t_ComboItems ds on d.StatusID = ds.ComboItemID left outer join t_CombOItems ut on d.UnitTypeID = ut.ComboItemID left outer join t_ComboItems s on d.SeasonID = s.ComboItemID left outer join t_CombOItems ec on d.ExchangeID = ec.ComboitemID left outer join t_Frequency f on d.FrequencyID = f.FrequencyID left outer join t_Personnel cb on d.CreatedByID = cb.PersonnelID left outer join t_Personnel db on d.DepositedByID = db.PersonnelID left outer join t_Bank2Room br on d.DepositID = br.DepositID left outer join t_Room rm on br.RoomID = rm.RoomID where d.ExchangeID in (" & exchComp & ") and d.CreatedByID in (" & user & ") and d.StatusID in (" & status & ") and d.DateCreated between '" & sDate & "' and '" & eDate.AddDays(1) & "' and c.ContractNumber like '" & conNum & "%'"

                'HttpContext.Current.Response.Write("<br/><hr/>" & tmp)


                ds.SelectCommand = "Select d.DepositID, d.ProspectID, d.ContractID, p.LastName + ', ' + p.FirstName as Owner, c.ContractNumber, ds.ComboItem as Status, d.StatusDate, cb.UserName as CreatedBy, db.UserName as DepositedBy, d.UsageYear as YearUsed, d.DepositYear, ut.ComboItem as UnitType, d.MemberShipNumber, d.UsageID, d.UnitSize, s.ComboItem as Usage, f.Frequency as Frequency, ec.ComboItem as ExchangeCompany, d.DateCreated, cst.comboitem as SubType  from t_bankedUnits d inner join t_prospect p on d.ProspectiD = p.ProspectiD inner join t_Contract c on d.ContractID = c.ContractID left outer join t_Comboitems cst on cst.comboitemid=c.subtypeid left outer join t_ComboItems ds on d.StatusID = ds.ComboItemID left outer join t_CombOItems ut on d.UnitTypeID = ut.ComboItemID left outer join t_ComboItems s on d.SeasonID = s.ComboItemID left outer join t_CombOItems ec on d.ExchangeID = ec.ComboitemID left outer join t_Frequency f on d.FrequencyID = f.FrequencyID left outer join t_Personnel cb on d.CreatedByID = cb.PersonnelID left outer join t_Personnel db on d.DepositedByID = db.PersonnelID where d.ExchangeID in (" & exchComp & ") and d.CreatedByID in (" & user & ") and d.StatusID in (" & status & ") and d.DateCreated between '" & sDate & "' and '" & eDate.AddDays(1) & "' and c.ContractNumber like '" & conNum & "%'"
                'ds.SelectCommand = "Select d.DepositID, d.ProspectID, d.ContractID, p.LastName + ', ' + p.FirstName as Owner, c.ContractNumber, ds.ComboItem as Status, d.StatusDate, cb.UserName as CreatedBy, db.UserName as DepositedBy, d.UsageYear as YearUsed, d.DepositYear, ut.ComboItem as UnitType, d.MemberShipNumber, d.DateDeposited, d.UnitSize, rm.RoomNumber, s.ComboItem as Usage, f.Frequency as Frequency, ec.ComboItem as ExchangeCompany, d.DateCreated, cst.comboitem as SubType  from t_bankedUnits d inner join t_prospect p on d.ProspectiD = p.ProspectiD inner join t_Contract c on d.ContractID = c.ContractID left outer join t_Comboitems cst on cst.comboitemid=c.subtypeid left outer join t_ComboItems ds on d.StatusID = ds.ComboItemID left outer join t_CombOItems ut on d.UnitTypeID = ut.ComboItemID left outer join t_ComboItems s on d.SeasonID = s.ComboItemID left outer join t_CombOItems ec on d.ExchangeID = ec.ComboitemID left outer join t_Frequency f on d.FrequencyID = f.FrequencyID left outer join t_Personnel cb on d.CreatedByID = cb.PersonnelID left outer join t_Personnel db on d.DepositedByID = db.PersonnelID left outer join t_Bank2Room br on d.DepositID = br.DepositID left outer join t_Room rm on br.RoomID = rm.RoomID where d.ExchangeID in (" & exchComp & ") and d.CreatedByID in (" & user & ") and d.StatusID in (" & status & ") and d.DateCreated between '" & sDate & "' and '" & eDate.AddDays(1) & "' and c.ContractNumber like '" & conNum & "%'"

            Else
                ds.SelectCommand = "Select d.DepositID, d.ProspectID, d.ContractID, p.LastName + ', ' + p.FirstName as Owner, c.ContractNumber, ds.ComboItem as Status, d.StatusDate, cb.UserName as CreatedBy, db.UserName as DepositedBy, d.UsageYear as YearUsed, d.DepositYear, ut.ComboItem as UnitType, d.MemberShipNumber, d.UsageID, d.UnitSize, s.ComboItem as Usage, f.Frequency as Frequency, ec.ComboItem as ExchangeCompany, d.DateCreated, cst.comboitem as SubType  from t_bankedUnits d inner join t_prospect p on d.ProspectiD = p.ProspectiD inner join t_Contract c on d.ContractID = c.ContractID left outer join t_Comboitems cst on cst.comboitemid=c.subtypeid left outer join t_ComboItems ds on d.StatusID = ds.ComboItemID left outer join t_CombOItems ut on d.UnitTypeID = ut.ComboItemID left outer join t_ComboItems s on d.SeasonID = s.ComboItemID left outer join t_CombOItems ec on d.ExchangeID = ec.ComboitemID left outer join t_Frequency f on d.FrequencyID = f.FrequencyID left outer join t_Personnel cb on d.CreatedByID = cb.PersonnelID left outer join t_Personnel db on d.DepositedByID = db.PersonnelID where d.ExchangeID in (" & exchComp & ") and d.CreatedByID in (" & user & ") and d.StatusID in (" & status & ") and d.StatusDate between '" & sDate & "' and '" & eDate.AddDays(1) & "' and c.ContractNumber like '" & conNum & "%'"
                'ds.SelectCommand = "Select d.DepositID, d.ProspectID, d.ContractID, p.LastName + ', ' + p.FirstName as Owner, c.ContractNumber, ds.ComboItem as Status, d.StatusDate, cb.UserName as CreatedBy, db.UserName as DepositedBy, d.UsageYear as YearUsed, d.DepositYear, ut.ComboItem as UnitType, d.MemberShipNumber, d.DateDeposited, d.UnitSize, rm.RoomNumber, s.ComboItem as Usage, f.Frequency as Frequency, ec.ComboItem as ExchangeCompany, d.DateCreated, cst.comboitem as SubType  from t_bankedUnits d inner join t_prospect p on d.ProspectiD = p.ProspectiD inner join t_Contract c on d.ContractID = c.ContractID left outer join t_Comboitems cst on cst.comboitemid=c.subtypeid left outer join t_ComboItems ds on d.StatusID = ds.ComboItemID left outer join t_CombOItems ut on d.UnitTypeID = ut.ComboItemID left outer join t_ComboItems s on d.SeasonID = s.ComboItemID left outer join t_CombOItems ec on d.ExchangeID = ec.ComboitemID left outer join t_Frequency f on d.FrequencyID = f.FrequencyID left outer join t_Personnel cb on d.CreatedByID = cb.PersonnelID left outer join t_Personnel db on d.DepositedByID = db.PersonnelID left outer join t_Bank2Room br on d.DepositID = br.DepositID left outer join t_Room rm on br.RoomID = rm.RoomID where d.ExchangeID in (" & exchComp & ") and d.CreatedByID in (" & user & ") and d.StatusID in (" & status & ") and d.StatusDate between '" & sDate & "' and '" & eDate.AddDays(1) & "' and c.ContractNumber like '" & conNum & "%'"

            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Rooms(ByVal bd As String, ByVal uType As String) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("RoomID")
        dt.Columns.Add("Room")
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
            If ((uType = "Cottage" Or uType = "Estates") And bd = "3BD") Or (uType = "Townes" And bd = "4BD") Then
                cm.CommandText = "Select r.RoomID as RoomID1, r.RoomNumber as Room1, lo.RoomID as RoomID2, lo.RoomNumber as Room2, '' as RoomID3, '' as Room3 " & _
                                    "from t_Room r inner join t_Room lo on r.LockoutID = lo.RoomID " & _
                                    "inner join t_Unit u on r.UnitID = u.UnitID " & _
                                    "inner join t_ComboItems ut on u.TypeID = ut.ComboItemID where ut.ComboItem = '" & uType & "' " & _
                                    "order by charindex(r.RoomNumber, '-') asc, r.RoomNumber asc"
            ElseIf uType = "Estates" And bd = "4BD" Then
                cm.CommandText = "Select r.RoomID as RoomID1, r.RoomNumber as Room1,lo.RoomID as RoomID2, lo.RoomNumber as Room2, r3.RoomID as RoomID3, r3.RoomNumber as Room3 " & _
                                    "from t_Room r inner join t_Room lo on r.LockoutID = lo.RoomID " & _
                                    "inner join t_Unit u on r.UnitID = u.UnitID " & _
                                    "inner join t_ComboItems ut on u.TypeID = ut.ComboItemID " & _
                                    "inner join t_Unit2Unit u2 on r.UnitID = u2.UnitID and u2.Unit2ID <> lo.UnitID " & _
                                    "inner join t_Room r3 on u2.Unit2ID = r3.UnitID " & _
                                    "where ut.ComboItem = '" & uType & "' " & _
                                    "order by CHARINDEX(r.RoomNumber, '-') asc, r.RoomNumber asc"
            Else
                cm.CommandText = "Select r.RoomID as RoomID1, r.RoomNumber as Room1, '' as RoomID2, '' as Room2, '' as RoomID3, '' as Room3 " & _
                                    "From t_Room r inner join t_Unit u on r.UnitID = u.UnitID " & _
                                    "inner join t_ComboItems rt on r.TypeID = rt.ComboItemID " & _
                                    "inner join t_ComboItems ut on u.TypeID = ut.ComboItemID " & _
                                    "where rt.ComboItem = '" & bd & "' and ut.CombOitem = '" & uType & "' " & _
                                    "order by charindex(r.RoomNumber,'-') asc, r.RoomNumber"
            End If
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    Dim dRow As DataRow = dt.NewRow
                If dread("RoomID2").ToString = "" Then
                    dRow("RoomID") = dread("RoomID1")
                    dRow("Room") = dread("Room1")
                ElseIf dread("RoomID3").ToString = "" Then
                    dRow("RoomID") = dread("RoomID1") & "|" & dread("RoomID2")
                    dRow("Room") = dread("Room1") & "/" & dread("Room2")
                Else
                    dRow("RoomID") = dread("RoomID1") & "|" & dread("RoomID2") & "|" & dread("RoomID3")
                        dRow("Room") = dread("Room1") & "/" & dread("Room2") & "/" & dread("Room3")
                    End If
                    dt.Rows.Add(dRow)
                Loop
            End If
            dread.Close()
            'Catch ex As Exception
            '    _Err = ex.Message
            'Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            'End Try
            Return dt
    End Function

    Public Function Validate_Room(ByVal roomID As Integer, ByVal ciDate As Date, ByVal uType As String) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Rooms " & _
                                "from t_RoomAllocationmatrix rmx inner join t_ComboItems rt on rmx.TypeID = rt.ComboItemID " & _
                                "where rmx.RoomID = '" & roomID & "' and dateallocated between '" & ciDate & "' and '" & ciDate.AddDays(6) & "' and rt.ComboItem = '" & uType & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Rooms") = 7 Then
                bValid = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Validate_Room_UsageID(ByVal roomID As Integer, ByVal ciDate As Date) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Rooms " & _
                                "from t_RoomAllocationmatrix rmx inner join t_ComboItems rt on rmx.TypeID = rt.ComboItemID " & _
                                "where rmx.RoomID = '" & roomID & "' and dateallocated between '" & ciDate & "' and '" & ciDate.AddDays(6) & "' and usageID = 0"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Rooms") = 7 Then
                bValid = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
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

    Public Property UnitTypeID() As Integer
        Get
            Return _UnitTypeID
        End Get
        Set(ByVal value As Integer)
            _UnitTypeID = value
        End Set
    End Property

    Public Property UnitSize() As String
        Get
            Return _UnitSize
        End Get
        Set(ByVal value As String)
            _UnitSize = value
        End Set
    End Property

    Public Property SeasonID() As Integer
        Get
            Return _SeasonID
        End Get
        Set(ByVal value As Integer)
            _SeasonID = value
        End Set
    End Property

    Public Property ExchangeID() As Integer
        Get
            Return _ExchangeID
        End Get
        Set(ByVal value As Integer)
            _ExchangeID = value
        End Set
    End Property

    Public Property MembershipNumber() As String
        Get
            Return _MembershipNumber
        End Get
        Set(ByVal value As String)
            _MembershipNumber = value
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

    Public Property UsageYear() As Integer
        Get
            Return _UsageYear
        End Get
        Set(ByVal value As Integer)
            _UsageYear = value
        End Set
    End Property

    Public Property DateDeposited() As String
        Get
            Return _DateDeposited
        End Get
        Set(ByVal value As String)
            _DateDeposited = value
        End Set
    End Property

    Public Property DepositYear() As Integer
        Get
            Return _DepositYear
        End Get
        Set(ByVal value As Integer)
            _DepositYear = value
        End Set
    End Property

    Public Property WeekDeposited() As Integer
        Get
            Return _WeekDeposited
        End Get
        Set(ByVal value As Integer)
            _WeekDeposited = value
        End Set
    End Property

    Public Property NoteID() As Integer
        Get
            Return _NoteID
        End Get
        Set(ByVal value As Integer)
            _NoteID = value
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

    Public Property ConfirmationNumber() As String
        Get
            Return _ConfirmationNumber
        End Get
        Set(ByVal value As String)
            _ConfirmationNumber = value
        End Set
    End Property

    Public Property FrequencyID() As Integer
        Get
            Return _FrequencyID
        End Get
        Set(ByVal value As Integer)
            _FrequencyID = value
        End Set
    End Property

    Public Property DepositedByID() As Integer
        Get
            Return _DepositedByID
        End Get
        Set(ByVal value As Integer)
            _DepositedByID = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property DepositID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property UsageID() As Integer
        Get
            Return _UsageID
        End Get
        Set(value As Integer)
            _UsageID = value
        End Set
    End Property
End Class
