Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient


Public Class clsRooms
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RoomNumber As String = ""
    Dim _UnitID As Integer = 0
    Dim _LockOutID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _MaintenanceStatusID As Integer = 0
    Dim _MaintenanceStatusDate As String = ""
    Dim _HouseKeepingStatusID As Integer = 0
    Dim _HouseKeepingStatusDate As String = ""
    Dim _Phone As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _MaxOccupancy As Integer = 0
    Dim _UnitNumber As String = ""
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dRead As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Room where RoomID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Room where RoomID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Room")
            If ds.Tables("t_Room").Rows.Count > 0 Then
                dr = ds.Tables("t_Room").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RoomNumber") Is System.DBNull.Value) Then _RoomNumber = dr("RoomNumber")
        If Not (dr("UnitID") Is System.DBNull.Value) Then _UnitID = dr("UnitID")
        If Not (dr("LockOutID") Is System.DBNull.Value) Then _LockOutID = dr("LockOutID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("MaintenanceStatusID") Is System.DBNull.Value) Then _MaintenanceStatusID = dr("MaintenanceStatusID")
        If Not (dr("MaintenanceStatusDate") Is System.DBNull.Value) Then _MaintenanceStatusDate = dr("MaintenanceStatusDate")
        If Not (dr("HouseKeepingStatusID") Is System.DBNull.Value) Then _HouseKeepingStatusID = dr("HouseKeepingStatusID")
        If Not (dr("HouseKeepingStatusDate") Is System.DBNull.Value) Then _HouseKeepingStatusDate = dr("HouseKeepingStatusDate")
        If Not (dr("Phone") Is System.DBNull.Value) Then _Phone = dr("Phone")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("MaxOccupancy") Is System.DBNull.Value) Then _MaxOccupancy = dr("MaxOccupancy")
        'If Not (dr("UnitNumber") Is System.DBNull.Value) Then _UnitNumber = dr("UnitNumber")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Room where RoomID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Room")
            If ds.Tables("t_Room").Rows.Count > 0 Then
                dr = ds.Tables("t_Room").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RoomInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RoomNumber", SqlDbType.varchar, 0, "RoomNumber")
                da.InsertCommand.Parameters.Add("@UnitID", SqlDbType.int, 0, "UnitID")
                da.InsertCommand.Parameters.Add("@LockOutID", SqlDbType.int, 0, "LockOutID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.smalldatetime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@MaintenanceStatusID", SqlDbType.int, 0, "MaintenanceStatusID")
                da.InsertCommand.Parameters.Add("@MaintenanceStatusDate", SqlDbType.smalldatetime, 0, "MaintenanceStatusDate")
                da.InsertCommand.Parameters.Add("@HouseKeepingStatusID", SqlDbType.int, 0, "HouseKeepingStatusID")
                da.InsertCommand.Parameters.Add("@HouseKeepingStatusDate", SqlDbType.smalldatetime, 0, "HouseKeepingStatusDate")
                da.InsertCommand.Parameters.Add("@Phone", SqlDbType.varchar, 0, "Phone")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@MaxOccupancy", SqlDbType.int, 0, "MaxOccupancy")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.Int, 0, "RoomID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Room").NewRow
            End If
            Update_Field("RoomNumber", _RoomNumber, dr)
            Update_Field("UnitID", _UnitID, dr)
            Update_Field("LockOutID", _LockOutID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("MaintenanceStatusID", _MaintenanceStatusID, dr)
            Update_Field("MaintenanceStatusDate", _MaintenanceStatusDate, dr)
            Update_Field("HouseKeepingStatusID", _HouseKeepingStatusID, dr)
            Update_Field("HouseKeepingStatusDate", _HouseKeepingStatusDate, dr)
            Update_Field("Phone", _Phone, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("MaxOccupancy", _MaxOccupancy, dr)
            If ds.Tables("t_Room").Rows.Count < 1 Then ds.Tables("t_Room").Rows.Add(dr)
            da.Update(ds, "t_Room")
            _ID = ds.Tables("t_Room").Rows(0).Item("RoomID")
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
            oEvents.KeyField = "RoomID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function List(ByVal sField As String, ByVal ID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "select RoomID as ID, RoomNumber, Phone, Where Roomid = " & _ID
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function List_Rooms() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select RoomID, RoomNumber from t_Room order by CharIndex('-',RoomNumber) asc, RoomNumber asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Lockouts(ByVal roomID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "select RoomID, RoomNumber from t_room where (unitid = (select unitid from t_room where roomid = '" & roomID & "')) and (roomid <> '" & roomID & "') UNION SELECT RoomID, RoomNumber FROM t_Room WHERE (UnitID IN (SELECT unit2id FROM t_Unit2Unit WHERE unitID = (SELECT UnitID FROM t_Room WHERE roomid = '" & roomID & "'))) AND (RoomID <> '" & roomID & "')"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Unit_Rooms(ByVal sField As String, ByVal ID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select RoomID as ID, RoomNumber, Phone from t_Room where UnitID = " & _UnitID
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function search_Swappables(ByVal resID As Integer, ByVal roomID As Integer, ByVal inDate As Date, ByVal outDate As Date, ByVal status As String) As sqldatasource
        Dim uTypeID As Integer = 0
        Dim rTypeID As Integer = 0
        Dim rsTypeID As Integer = 0
        Dim ds As New sqldatasource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim oRoom As New clsRooms
            Dim oRes As New clsReservations
            oRes.ReservationID = resID
            oRes.Load()
            oRoom.RoomID = roomID
            oRoom.Load()
            rTypeID = oRoom.TypeID
            rsTypeID = oRoom.SubTypeID
            oRoom = Nothing
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ut.ComboItemID from t_Room r inner join t_Unit u on r.unitid = u.unitid left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID where r.RoomID = '" & roomID & "'"
            dRead = cm.ExecuteReader()
            dRead.Read()
            uTypeID = dRead.GetValue(0).ToString & ""
            dRead = Nothing
            If status = "Booked" And DateTime.Compare(System.DateTime.Now, CDate(oRes.CheckInDate)) < 0 Then
                ds.SelectCommand = "Select RoomID, RoomNumber, RoomType, RoomSubType, UnitType, UnitStyle, RoomStatus from (Select Distinct(rmx.RoomID), rm.RoomNumber, rt.ComboItem as RoomType, rst.ComboItem as RoomSubType, ut.ComboItem as UnitType, ust.ComboItem as UnitStyle, rms.ComboItem as RoomStatus, Count(rmx.AllocationID) As Nights from t_RoomAllocationMatrix rmx left outer join t_Reservations r on rmx.ReservationID = r.ReservationID left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rmt on rmx.TypeID = rmt.ComboItemID inner join t_Room rm on rmx.RoomID = rm.RoomID left outer join t_ComboItems rt on rm.TypeID = rt.ComboItemID left outer join t_ComboItems rst on rm.SubTypeID = rst.ComboItemID left outer join t_ComboItems rms on rm.StatusID = rms.ComboItemID left outer join t_Unit u on rm.UnitID = u.UnitID left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID left outer join t_ComboItems ust on u.StyleID = ust.ComboItemID where ((rmx.ReservationID = 0 or rmx.ReservationID = '" & resID & "') OR (r.CheckInDate = '" & inDate & "' and r.CheckOutDate = '" & outDate & "' and r.LockInventory = '0' and r.ReservationID <> '" & resID & "' and rs.ComboItem = 'Booked')) and rmx.dateallocated between '" & inDate & "' and '" & outDate.AddDays(-1) & "' and rmt.CombOitem <> 'Spare' and rmx.RoomID <> '" & roomID & "' and rt.ComboItemID = '" & rTypeID & "' and ut.ComboItemID = '" & uTypeID & "' and rst.ComboItemID = '" & rsTypeID & "' Group By rmx.RoomID, rm.RoomNumber, rt.ComboItem, rst.ComboItem, ut.ComboItem, ust.ComboItem, rms.ComboItem) xx where Nights = DateDiff(d, '" & inDate & "', '" & outDate & "') order by Charindex('-',roomnumber), roomnumber"
            Else
                ds.SelectCommand = "Select RoomID, RoomNumber, RoomType, RoomSubType, UnitType, UnitStyle, RoomStatus from (Select Distinct(rmx.RoomID), rm.RoomNumber, rt.ComboItem as RoomType, rst.ComboItem as RoomSubType, ut.ComboItem as UnitType, ust.ComboItem as UnitStyle, rms.ComboItem as RoomStatus, Count(rmx.AllocationID) As Nights from t_RoomAllocationMatrix rmx left outer join t_Reservations r on rmx.ReservationID = r.ReservationID left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rmt on rmx.TypeID = rmt.ComboItemID inner join t_Room rm on rmx.RoomID = rm.RoomID left outer join t_ComboItems rt on rm.TypeID = rt.ComboItemID left outer join t_ComboItems rst on rm.SubTypeID = rst.ComboItemID left outer join t_ComboItems rms on rm.StatusID = rms.ComboItemID left outer join t_Unit u on rm.UnitID = u.UnitID left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID left outer join t_ComboItems ust on u.StyleID = ust.ComboItemID where ((rmx.ReservationID = 0 or rmx.ReservationID = '" & resID & "') OR (r.CheckInDate = '" & inDate & "' and r.CheckOutDate = '" & outDate & "' and r.LockInventory = '0' and r.ReservationID <> '" & resID & "' and rs.ComboItem = 'Booked')) and rmx.dateallocated between '" & inDate & "' and '" & outDate.AddDays(-1) & "' and rmt.CombOitem <> 'Spare' and rmx.RoomID <> '" & roomID & "' and rt.ComboItemID = '" & rTypeID & "' and ut.ComboItemID = '" & uTypeID & "' Group By rmx.RoomID, rm.RoomNumber, rt.ComboItem, rst.ComboItem, ut.ComboItem, ust.ComboItem, rms.ComboItem) xx where Nights = DateDiff(d, '" & inDate & "', '" & outDate & "') order by Charindex('-',roomnumber), roomnumber"
            End If
            oRes = Nothing
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ds
    End Function
    Public Function search_Spares(ByVal inDate As Date, ByVal outDate As Date) As SqlDataSource
        Dim ds As New sqldatasource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select xx.RoomID, xx.RoomNumber, xx.RoomType, xx.UnitType from (Select Distinct rm.RoomID, Count(rm.AllocationID) as Nights, r.RoomNumber, rmt.ComboItem as RoomType, ut.ComboItem as UnitType from t_ROomAllocationMatrix rm left outer join t_ComboItems rt on rm.TypeID = rt.ComboItemID left outer join t_Room r on rm.RoomID = r.RoomID left outer join t_ComboItems rmt on r.TypeID = rmt.CombOItemID left outer join t_ComboItems rst on r.SubTypeID = rst.ComboItemID left outer join t_Unit u on r.UnitID = u.UnitID left outer join t_ComboItems ut on u.TypeID = ut.ComboitemID where rt.ComboItem = 'Spare' and rm.ReservationID = '0' and rm.DateAllocated between '" & inDate & "' and '" & outDate.AddDays(-1) & "' Group By rm.RoomID, r.RoomNumber, rmt.ComboItem, ut.ComboItem) xx where xx.Nights = DateDiff(d,'" & inDate & "','" & outDate & "') order by CharIndex('-',xx.RoomNumber), xx.RoomNumber"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function search_Swappable_Spares(ByVal roomID As Integer, ByVal inDate As Date, ByVal outDate As Date) As SQLDataSource
        Dim rTypeID As Integer
        Dim ds As New sqldatasource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim oRoom As New clsRooms
            Dim oCombo As New clsComboItems
            oRoom.RoomID = roomID
            oRoom.Load()
            rTypeID = oRoom.TypeID
            oRoom = Nothing
            If cn.State <> ConnectionState.Open Then cn.Open()
            ds.SelectCommand = "Select RoomID, RoomNumber, RoomType, RoomSubType, UnitType, UnitStyle, RoomStatus from (Select Distinct(rmx.RoomID), rm.RoomNumber, rt.ComboItem as RoomType, rst.ComboItem as RoomSubType, ut.ComboItem as UnitType, ust.ComboItem as UnitStyle, rms.ComboItem as RoomStatus, Count(rmx.AllocationID) As Nights from t_RoomAllocationMatrix rmx left outer join t_Reservations r on rmx.ReservationID = r.ReservationID left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rmt on rmx.TypeID = rmt.ComboItemID inner join t_Room rm on rmx.RoomID = rm.RoomID left outer join t_ComboItems rt on rm.TypeID = rt.ComboItemID left outer join t_ComboItems rst on rm.SubTypeID = rst.ComboItemID left outer join t_Unit u on rm.UnitID = u.UnitID left outer join t_ComboItems rms on rm.StatusID = rms.ComboItemID left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID left outer join t_ComboItems ust on u.StyleID = ust.ComboItemID where ((rmx.ReservationID = 0 and rmx.UsageID = 0)) and rmx.dateallocated between '" & inDate & "' and '" & outDate.AddDays(-1) & "' and rmt.CombOitem = 'Spare' and Left(rt.ComboItem, 3) = '" & Left(oCombo.Lookup_ComboItem(rTypeID), 3) & "' Group By rmx.RoomID, rm.RoomNumber, rt.ComboItem, rst.ComboItem, ut.ComboItem, ust.ComboItem, rms.CombOitem) xx where Nights = DateDiff(d, '" & inDate & "', '" & outDate & "') order by Charindex('-',roomnumber), roomnumber"
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ds
    End Function
    Public Function Validate_Spare_Allocation_Res(ByVal roomID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        _Err = ""
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(rmx.ReservationID), p.LastName + ', ' + p.FirstName as Prospect, r.CheckInDate, r.CheckOutDate, rt.ComboItem as ResType, rst.ComboItem as ResSubType from t_RoomAllocationMatrix rmx inner join t_Reservations r on  rmx.ReservationID = r.ReservationID inner join t_prospect p on r.ProspectID = p.ProspectiD left outer join t_ComboItems rt on r.TypeID = rt.ComboItemID left outer join t_ComboItems rst on r.SubTypeID = rst.CombOitemID where rmx.roomID = '" & roomID & "' and rmx.DateAllocated Between '" & startDate & "' and '" & endDate & "' and rmx.ReservationID > '0'"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                bValid = False
                Do While dRead.Read
                    If _Err = "" Then
                        _Err = dRead("ReservationID") & " - " & dRead("Prospect") & "  " & dRead("CheckInDate") & " - " & dRead("CheckOutDate") & "   " & dRead("ResType") & "/" & dRead("ResSubType")
                    Else
                        _Err = _Err & "<br>" & dRead("ReservationID") & " - " & dRead("Prospect") & "  " & dRead("CheckInDate") & " - " & dRead("CheckOutDate") & "   " & dRead("ResType") & "/" & dRead("ResSubType")
                    End If
                Loop
            End If
            dRead = Nothing
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Validate_Spare_Allocation_Usage(ByVal roomID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        _Err = ""
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(rmx.UsageID), p.LastName + ', ' + p.FirstName as Prospect, c.ContractNumber, u.InDate, u.OutDate, ut.ComboItem as UsageType from t_RoomAllocationMatrix rmx inner join t_usage u on rmx.UsageID = u.UsageID inner join t_Contract c on u.ContractID = c.ContractID inner join t_prospect p on c.ProspectID = p.ProspectID left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID where rmx.roomID = '" & roomID & "' and rmx.DateAllocated Between '" & startDate & "' and '" & endDate & "' and rmx.UsageiD > '0'"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                bValid = False
                Do While dRead.Read
                    If _Err = "" Then
                        _Err = dRead("Prospect") & "  " & dRead("ContractNumber") & "  " & dRead("InDate") & " - " & dRead("OutDate") & " " & dRead("UsageType")
                    Else
                        _Err = _Err & "<br>" & dRead("Prospect") & "  " & dRead("ContractNumber") & "  " & dRead("InDate") & " - " & dRead("OutDate") & " " & dRead("UsageType")
                    End If
                Loop
            End If
            dRead = Nothing
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Validate_Spare_Allocation_Service(ByVal roomID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        _Err = ""
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(AllocationID) is Null then 0 Else Count(AllocationID) end as Reservations from t_RoomAllocationMatrix where roomID = '" & roomID & "' and DateAllocated Between '" & startDate & "' and '" & endDate & "' and ReservationID = '-1'"
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("Reservations") > 0 Then
                bValid = False
            End If
            dRead = Nothing
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Validate_Type_Allocation(ByVal roomID As Integer, ByVal typeID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal userID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim oCombo As New clsComboItems
            If CheckSecurity("Rooms", "AllocateTo" & oCombo.Lookup_ComboItem(typeID), , , userID) Then
                cm.CommandText = "Select Distinct(TypeID) as TypeID from t_RoomAllocationMatrix where dateallocated between '" & startDate & "' and '" & endDate & "' and roomid = " & roomID
                dRead = cm.ExecuteReader()
                If dRead.HasRows Then
                    Do While dRead.Read
                        If dRead("TypeID") = 0 Then
                            If Not (CheckSecurity("Rooms", "AllocateNew", , , userID)) Then
                                bValid = False
                                Exit Do
                            End If
                        ElseIf Not (CheckSecurity("Rooms", "AllocateFrom" & oCombo.Lookup_ComboItem(dRead("TypeID")), , , userID)) Then
                            bValid = False
                            Exit Do
                        End If
                    Loop
                Else
                    If Not (CheckSecurity("Rooms", "AllocateNew", , , userID)) Then
                        bValid = False
                    End If
                End If
            Else
                bValid = False
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Allocate(ByVal roomID As Integer, ByVal typeID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        _Err = ""
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_RoomAllocationMatrix set TypeID = '" & typeID & "' where RoomID = '" & roomID & "' and dateAllocated between '" & startDate & "' and '" & endDate & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Validate_Out_Of_Service(ByVal roomID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        _Err = ""
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(rmx.ReservationID), p.LastName + ', ' + p.FirstName as Prospect, r.CheckInDate, r.CheckOutDate, rt.ComboItem as ResType, rst.ComboItem as ResSubType from t_RoomAllocationMatrix rmx inner join t_Reservations r on  rmx.ReservationID = r.ReservationID left outer join t_prospect p on r.ProspectID = p.ProspectiD left outer join t_ComboItems rt on r.TypeID = rt.ComboItemID left outer join t_ComboItems rst on r.SubTypeID = rst.CombOitemID where rmx.roomID = '" & roomID & "' and rmx.DateAllocated Between '" & startDate & "' and '" & endDate & "' and rmx.ReservationID > '0'"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                bValid = False
                Do While dRead.Read
                    If _Err = "" Then
                        _Err = dRead("ReservationID") & " - " & dRead("Prospect") & "  " & dRead("CheckInDate") & " - " & dRead("CheckOutDate") & "   " & dRead("ResType") & "/" & dRead("ResSubType")
                    Else
                        _Err = _Err & "<br>" & dRead("ReservationID") & " - " & dRead("Prospect") & "  " & dRead("CheckInDate") & " - " & dRead("CheckOutDate") & "   " & dRead("ResType") & "/" & dRead("ResSubType")
                    End If
                Loop
            End If
            dRead = Nothing
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Out_Of_Service(ByVal roomID As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal sType As String, ByVal reason As String, ByVal reasonID As Integer) As Boolean
        _Err = ""
        Dim bProceed As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If sType = "Out" Then
                cm.CommandText = "Update t_RoomAllocationMatrix set ReservationID = '-1' where roomid = '" & roomID & "' and dateallocated between '" & startDate & "' and '" & endDate & "'"
            Else
                cm.CommandText = "Update t_RoomAllocationMatrix set ReservationID = '0' where roomid = '" & roomID & "' and dateallocated between '" & startDate & "' and '" & endDate & "' and reservationid = '-1'"
            End If
            cm.ExecuteNonQuery()
            Dim oRoomOffline As New clsRoomsOffline
            oRoomOffline.ID = 0
            oRoomOffline.Load()
            oRoomOffline.RoomID = roomID
            oRoomOffline.StartDate = startDate
            oRoomOffline.Enddate = endDate
            oRoomOffline.Reason = reason
            oRoomOffline.ReasonID = reasonID
            oRoomOffline.CreatedByID = _UserID
            oRoomOffline.DateCreated = System.DateTime.Now
            If sType = "Out" Then
                oRoomOffline.OutOfService = True
            Else
                oRoomOffline.OutOfService = False
            End If
            oRoomOffline.Save()
            oRoomOffline = Nothing
            Dim oEvent As New clsEvents
            oEvent.KeyField = "RoomID"
            oEvent.KeyValue = roomID
            oEvent.DateCreated = System.DateTime.Now
            oEvent.CreatedByID = _UserID
            oEvent.EventType = "Change"
            If sType = "Out" Then
                oEvent.OldValue = "In Service"
                oEvent.NewValue = "Out Of Service"
            Else
                oEvent.OldValue = "Out Of Service"
                oEvent.NewValue = "In Service"
            End If
            oEvent.Create_Event()
            oEvent = Nothing
        Catch ex As Exception
            bProceed = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bProceed
    End Function

    Public Function Room_Usage(ByVal roomID As Integer, ByVal sDate As Date, ByVal eDate As Date) As SqlDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select mt.ComboItem as Type, Case when r.ReservationID is null then 0 else r.ReservationID end as ReservationID, Case when c.ContractID is null then 0 else c.ContractID end as contractid, m.DateAllocated, c.ContractNumber, u.UsageID from t_RoomAllocationmatrix m left outer join t_ComboItems mt on mt.ComboItemID = m.TypeID left outer join t_Usage u on u.usageid = m.usageid left outer join t_Contract c on c.ContractID = u.ContractID left outer join t_Reservations r on r.ReservationID= m.ReservationID where dateallocated between '" & sDate & "' and '" & eDate & "' and roomid = " & roomID & " order by dateallocated"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Check_InHouse_Res(ByVal roomID As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Res from t_RoomAllocationMatrix rmx inner join t_reservations r on rmx.ReservationID = r.ReservationID inner join t_ComboItems rs on r.StatusID = rs.ComboItemID where dateallocated = Cast(getDate() as date) and rs.ComboItem = 'In-House' and roomid = " & roomID
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("Res") > 0 Then
                bValid = True
            End If
            dRead.Close()
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

    Public Property RoomNumber() As String
        Get
            Return _RoomNumber
        End Get
        Set(ByVal value As String)
            _RoomNumber = value
        End Set
    End Property

    Public Property UnitID() As Integer
        Get
            Return _UnitID
        End Get
        Set(ByVal value As Integer)
            _UnitID = value
        End Set
    End Property

    Public Property LockOutID() As Integer
        Get
            Return _LockOutID
        End Get
        Set(ByVal value As Integer)
            _LockOutID = value
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
            If value <> _StatusID Then
                _StatusDate = System.DateTime.Now.ToShortDateString
            End If
            _StatusID = value
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

    Public Property MaintenanceStatusID() As Integer
        Get
            Return _MaintenanceStatusID
        End Get
        Set(ByVal value As Integer)
            If value <> _MaintenanceStatusID Then
                _MaintenanceStatusDate = System.DateTime.Now.ToShortDateString
            End If
            _MaintenanceStatusID = value
        End Set
    End Property

    Public Property MaintenanceStatusDate() As String
        Get
            Return _MaintenanceStatusDate
        End Get
        Set(ByVal value As String)
            _MaintenanceStatusDate = value
        End Set
    End Property

    Public Property HouseKeepingStatusID() As Integer
        Get
            Return _HouseKeepingStatusID
        End Get
        Set(ByVal value As Integer)
            _HouseKeepingStatusID = value
        End Set
    End Property

    Public Property HouseKeepingStatusDate() As String
        Get
            Return _HouseKeepingStatusDate
        End Get
        Set(ByVal value As String)
            _HouseKeepingStatusDate = value
        End Set
    End Property

    Public Property Phone() As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = value
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

    Public Property MaxOccupancy() As Integer
        Get
            Return _MaxOccupancy
        End Get
        Set(ByVal value As Integer)
            _MaxOccupancy = value
        End Set
    End Property

    Public Property RoomID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UnitNumber() As String
        Get
            Return _UnitNumber
        End Get
        Set(ByVal value As String)
            _UnitNumber = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property
    Public ReadOnly Property Err() As String
        Get
            Return _Err
        End Get
    End Property
End Class
