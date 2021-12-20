Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System
Public Class clsReservations

    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _PackageIssuedID As Integer = 0
    Dim _ResLocationID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _SourceID As Integer = 0
    Dim _CheckInDate As String = ""
    Dim _CheckOutDate As String = ""
    Dim _ReservationNumber As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _DateBooked As String = ""
    Dim _LockInventory As Boolean = False
    Dim _NumberAdults As Integer = 0
    Dim _NumberChildren As Integer = 0
    Dim _TourID As Integer = 0
    Dim _ResortCompanyID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Reservations where ReservationID = " & _ID, cn)
    End Sub
    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Reservations where ReservationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Reservations")
            If ds.Tables("t_Reservations").Rows.Count > 0 Then
                dr = ds.Tables("t_Reservations").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub
    Private Sub Set_Values()
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("PackageIssuedID") Is System.DBNull.Value) Then _PackageIssuedID = dr("PackageIssuedID")
        If Not (dr("ResLocationID") Is System.DBNull.Value) Then _ResLocationID = dr("ResLocationID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("SourceID") Is System.DBNull.Value) Then _SourceID = dr("SourceID")
        If Not (dr("CheckInDate") Is System.DBNull.Value) Then _CheckInDate = dr("CheckInDate")
        If Not (dr("CheckOutDate") Is System.DBNull.Value) Then _CheckOutDate = dr("CheckOutDate")
        If Not (dr("ReservationNumber") Is System.DBNull.Value) Then _ReservationNumber = dr("ReservationNumber")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("DateBooked") Is System.DBNull.Value) Then _DateBooked = dr("DateBooked")
        If Not (dr("LockInventory") Is System.DBNull.Value) Then _LockInventory = dr("LockInventory")
        If Not (dr("NumberAdults") Is System.DBNull.Value) Then _NumberAdults = dr("NumberAdults")
        If Not (dr("NumberChildren") Is System.DBNull.Value) Then _NumberChildren = dr("NumberChildren")
        If Not (dr("TourID") Is System.DBNull.Value) Then _TourID = dr("TourID")
        If Not (dr("ResortCompanyID") Is System.DBNull.Value) Then _ResortCompanyID = dr("ResortCompanyID")
    End Sub
    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Reservations where ReservationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Reservations")
            If ds.Tables("t_Reservations").Rows.Count > 0 Then
                dr = ds.Tables("t_Reservations").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReservationsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@PackageIssuedID", SqlDbType.int, 0, "PackageIssuedID")
                da.InsertCommand.Parameters.Add("@ResLocationID", SqlDbType.int, 0, "ResLocationID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.datetime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@SourceID", SqlDbType.int, 0, "SourceID")
                da.InsertCommand.Parameters.Add("@CheckInDate", SqlDbType.datetime, 0, "CheckInDate")
                da.InsertCommand.Parameters.Add("@CheckOutDate", SqlDbType.datetime, 0, "CheckOutDate")
                da.InsertCommand.Parameters.Add("@ReservationNumber", SqlDbType.varchar, 0, "ReservationNumber")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@DateBooked", SqlDbType.datetime, 0, "DateBooked")
                da.InsertCommand.Parameters.Add("@LockInventory", SqlDbType.bit, 0, "LockInventory")
                da.InsertCommand.Parameters.Add("@NumberAdults", SqlDbType.int, 0, "NumberAdults")
                da.InsertCommand.Parameters.Add("@NumberChildren", SqlDbType.Int, 0, "NumberChildren")
                da.InsertCommand.Parameters.Add("@TourID", SqlDbType.Int, 0, "TourID")
                da.InsertCommand.Parameters.Add("@ResortCompanyID", SqlDbType.Int, 0, "ResortCompanyID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.Int, 0, "ReservationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Reservations").NewRow

            End If
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("PackageIssuedID", _PackageIssuedID, dr)
            Update_Field("ResLocationID", _ResLocationID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("SourceID", _SourceID, dr)
            If _CheckInDate = "" Then
                Create_Event("CheckInDate", _CheckInDate, dr)
                dr("CheckInDate") = System.DBNull.Value
            Else
                Update_Field("CheckInDate", _CheckInDate, dr)
            End If
            If _CheckOutDate = "" Then
                Create_Event("CheckOutDate", _CheckOutDate, dr)
                dr("CheckOutDate") = System.DBNull.Value
            Else
                Update_Field("CheckOutDate", _CheckOutDate, dr)
            End If

            Update_Field("ReservationNumber", _ReservationNumber, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("DateBooked", _DateBooked, dr)
            Update_Field("LockInventory", _LockInventory, dr)
            Update_Field("NumberAdults", _NumberAdults, dr)
            Update_Field("NumberChildren", _NumberChildren, dr)
            Update_Field("TourID", _TourID, dr)
            Update_Field("ResortCompanyID", _ResortCompanyID, dr)
            If ds.Tables("t_Reservations").Rows.Count < 1 Then ds.Tables("t_Reservations").Rows.Add(dr)
            da.Update(ds, "t_Reservations")
            If _ID = 0 Then
                Dim oEvents As New clsEvents
                oEvents.Create_Create_Event("ReservationID", ds.Tables("t_Reservations").Rows(0).Item("ReservationID"), 0, _UserID, "")
                oEvents = Nothing
            End If
            _ID = ds.Tables("t_Reservations").Rows(0).Item("ReservationID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Private Sub Create_Event(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), CStr(drow(sField) & ""), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ReservationID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), CStr(drow(sField) & ""), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ReservationID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub
    Public Function val_ResID(ByVal resID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Reservations from t_Reservations where reservationid = '" & resID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Reservations") > 0 Then
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
    Public Function Add_Spare_Room(ByVal rooms As String, ByVal resID As Integer, ByVal inDate As Date, ByVal outDate As Date, ByVal usageID As Integer) As Boolean
        Dim dRow As DataRow
        Dim trans1 As SqlTransaction
        Dim x As Integer
        Dim oCombo As New clsComboItems
        If cn.State <> ConnectionState.Open Then cn.Open()
        trans1 = cn.BeginTransaction()
        Try
            cm.CommandText = "Select * from t_RoomAllocationMatrix where roomid = '" & rooms & "' and dateallocated between '" & inDate & "' and '" & outDate & "'"
            cm.Transaction = trans1
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Rooms")
            If ds.Tables("Rooms").Rows.Count > 0 Then
                For x = 0 To ds.Tables("Rooms").Rows.Count - 1
                    dRow = ds.Tables("Rooms").Rows(x)
                    If (dRow("UsageID") <> 0 Or dRow("TypeID") <> oCombo.Lookup_ID("ReservationType", "Spare") Or dRow("ReservationID") > 0) Then
                        Throw New Exception("One or more rooms are unavailable. Please Requery and select another room.")
                    Else
                        dRow("ReservationID") = resID
                        dRow("UsageID") = usageID
                    End If
                Next
            End If
            Dim sc As New SqlCommandBuilder(da)
            da.Update(ds, "Rooms")
            trans1.Commit()
            Return True
        Catch ex As Exception
            trans1.Rollback()
            _Err = ex.Message
            Throw New Exception(ex.Message)
            Return False
        Finally
            cn.Close()
        End Try
    End Function
    Public Function Remove_Room(ByVal roomID As Integer, ByVal resID As Integer, ByVal inDate As Date, ByVal outDate As Date) As Boolean
        Dim x As Integer
        Dim dRow As DataRow
        Dim oPros As New clsProspect
        Dim oCombo As New clsComboItems
        Dim oRes As New clsReservations
        Dim prosName As String = ""
        Dim binHouse As Boolean = False
        Dim rmStatus As Integer = 0
        oRes.ReservationID = resID
        oRes.Load()
        If oCombo.Lookup_ComboItem(oRes.StatusID) = "In-House" Then
            binHouse = True
            rmStatus = oCombo.Lookup_ID("RoomStatus", "Dirty")
            'Get Prospect name for interface check-out
            oPros.Prospect_ID = oRes.ProspectID
            oPros.Load()
            prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
        End If
        oPros = Nothing
        oCombo = Nothing
        oRes = Nothing
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RoomAllocationMatrix where roomid = '" & roomID & "' and ReservationID = '" & resID & "' and dateallocated between '" & inDate & "' and '" & outDate & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet()
            da.Fill(ds, "RemoveRooms")
            If ds.Tables("RemoveRooms").Rows.Count > 0 Then
                For x = 0 To ds.Tables("RemoveRooms").Rows.Count - 1
                    dRow = ds.Tables("RemoveRooms").Rows(x)
                    dRow("ReservationID") = 0
                Next x
            End If
            Dim sc As New SqlCommandBuilder(da)
            da.Update(ds, "RemoveRooms")

            Dim oRoom As New clsRooms
            oRoom.RoomID = roomID
            oRoom.Load()
            If binHouse = True Then
                Interface_Procedure(roomID, prosName, "CHECKIN", oRoom.Phone)
                oRoom.StatusID = rmStatus
                oRoom.UserID = _UserID
                oRoom.Save()
            End If
            Dim oEvent As New clsEvents
            oEvent.KeyField = "ReservationID"
            oEvent.KeyValue = resID
            oEvent.EventType = "Remove"
            oEvent.NewValue = oRoom.RoomNumber
            oEvent.CreatedByID = _UserID
            oEvent.Create_Event()
            oEvent = Nothing
            oRoom = Nothing

            Return True
        Catch ex As Exception
            _Err = ex.Message
            Throw New Exception(ex.Message)
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function
    Public Function Add_Room(ByVal rooms As String, ByVal resID As Integer, ByVal inDate As Date, ByVal outDate As Date) As Boolean
        Dim roomString As String()
        Dim sAns As String = ""
        Dim dRow As DataRow
        Dim trans As SqlTransaction
        Dim oPros As New clsProspect
        Dim oCombo As New clsComboItems
        Dim oRes As New clsReservations
        Dim prosName As String = ""
        Dim binHouse As Boolean = False
        Dim rmStatus As Integer = 0
        oRes.ReservationID = resID
        oRes.Load()
        If oCombo.Lookup_ComboItem(oRes.StatusID) = "In-House" Then
            binHouse = True
            rmStatus = oCombo.Lookup_ID("RoomStatus", "Dirty")
            'Get Prospect name for interface check-in
            oPros.Prospect_ID = oRes.ProspectID
            oPros.Load()
            prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
        End If
        oPros = Nothing
        oCombo = Nothing
        oRes = Nothing
        If cn.State <> ConnectionState.Open Then cn.Open()
        trans = cn.BeginTransaction()
        Try
            roomString = rooms.Split("-")
            Dim i As Integer
            Dim x As Integer
            For i = 0 To UBound(roomString)
                cm.CommandText = "Select * from t_RoomAllocationmatrix where roomID = '" & roomString(i) & "' and dateAllocated between '" & inDate & "' and '" & outDate & "'"
                cm.Transaction = trans
                da = New SqlDataAdapter(cm)
                ds = New DataSet()
                da.Fill(ds, "Rooms")
                If ds.Tables("Rooms").Rows.Count > 0 Then
                    For x = 0 To ds.Tables("Rooms").Rows.Count - 1
                        dRow = ds.Tables("Rooms").Rows(x)
                        If ((dRow("UsageID") Is System.DBNull.Value Or dRow("UsageID") = 0) Or dRow("ReservationID") > 0) Then
                            Throw New Exception("One or more rooms are unavailable. Please requery and select another room.")
                        Else
                            dRow("ReservationID") = resID
                        End If
                    Next
                End If
                Dim sc As New SqlCommandBuilder(da)
                da.Update(ds, "Rooms")
                Dim oRoom As New clsRooms
                oRoom.RoomID = CInt(roomString(i))
                oRoom.Load()
                If binHouse = True Then
                    Interface_Procedure(CInt(roomString(i)), prosName, "CHECKIN", oRoom.Phone)
                    oRoom.StatusID = rmStatus
                    oRoom.UserID = _UserID
                    oRoom.Save()
                End If
                Dim oEvent As New clsEvents
                oEvent.KeyField = "ReservationID"
                oEvent.KeyValue = resID
                oEvent.EventType = "Add"
                oEvent.NewValue = oRoom.RoomNumber
                oEvent.CreatedByID = _UserID
                oEvent.Create_Event()
                oEvent = Nothing
                oRoom = Nothing
            Next
            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            _Err = ex.Message
            Throw New Exception(ex.Message)
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function
    Public Function Search(ByVal filter As String, ByVal filterOption As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If filterOption = "resID" Then
                If filter = "" Then
                    ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid left outer join t_Prospect p on r.prospectid = p.prospectid order by r.reservationid asc"
                Else
                    ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid left outer join t_Prospect p on r.prospectid = p.prospectid where r.reservationid like '" & filter & "%' order by r.reservationid asc"
                End If
            ElseIf filterOption = "resNumber" Then
                If filter = "" Then
                    ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid left outer join t_Prospect p on r.prospectid = p.prospectid order by r.reservationid asc"
                Else
                    ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid left outer join t_Prospect p on r.prospectid = p.prospectid where r.reservationnumber like '" & filter & "%' order by r.reservationid asc"
                End If
            Else
                Dim name As String() = Nothing
                If filter = "" Then
                    ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid left outer join t_Prospect p on r.prospectid = p.prospectid order by r.reservationid asc"
                Else
                    If InStr(filter, ",") > 0 Then
                        name = filter.Split(",")
                        ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid inner join t_Prospect p on r.prospectid = p.prospectid where p.LastName = '" & Replace(Trim(Left(filter, InStr(filter, ",") - 1)), "'", "''") & "' and p.firstname like '" & Replace(Trim(Right(filter, Len(filter) - InStr(filter, ","))), "'", "''") & "%' order by r.reservationid asc"
                        'cm.CommandText = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOUtDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid inner join t_Prospect p on r.prospectid = p.prospectid where p.LastName = '" & name(0) & "' and p.firstname like '" & name(1) & "%' order by r.reservationid asc"
                    Else
                        ds.SelectCommand = "Select Top 50 r.ReservationID, rs.ComboItem as Status, r.CheckInDate, r.CheckOutDate, rt.ComboItem as Type, p.LastName + ', ' + p.FirstName as Prospect, r.DateBooked from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID left outer join t_ComboItems rt on r.typeid = rt.comboitemid left outer join t_Prospect p on r.prospectid = p.prospectid where p.LastName like '" & Replace(filter, "'", "''") & "%' order by r.reservationid asc"
                    End If
                End If
            End If

        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function List(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " ReservationID as ID,  r.CheckInDate, r.CheckOutDate, rs.comboitem as Status, r.DateBooked, rt.ComboItem as Type From t_Reservations r left outer join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_ComboItems rt on r.TypeID = rt.ComboitemID "
            sql += IIf(sFilterField <> "", " where " & sFilterField.Replace("'", "''") & " = '" & sFilterValue & "' ", "")
            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Public Function List_Rooms(ByVal resID As Integer) As DataTable
        Dim dt As New DataTable
        Dim sDate As Date
        Dim tempDate As Date
        dt.Columns.Add("RoomID")
        dt.Columns.Add("RoomNumber")
        dt.Columns.Add("Type")
        dt.Columns.Add("In Date")
        dt.Columns.Add("Out Date")
        dt.Columns.Add("Removable")
        dt.Columns.Add("Swappable")

        Try
            cn = New SqlConnection(Resources.Resource.cns)
            cm = New SqlCommand("", cn)
            da = New SqlDataAdapter(cm)
            Dim dRow As DataRow
            ds = New DataSet
            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim oRes As New clsReservations
            Dim oCombo As New clsComboItems
            oRes.ReservationID = resID
            oRes.Load()
            cm.CommandText = "SELECT DISTINCT a.RoomID, b.RoomNumber, c.ComboItem AS Type FROM t_RoomAllocationMatrix a INNER JOIN t_Room b ON a.RoomID = b.RoomID LEFT OUTER JOIN t_ComboItems c ON b.TypeID = c.ComboItemID WHERE a.ReservationID = '" & resID & "'"
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                For i = 0 To ds.Tables("0").Rows.Count - 1
                    cm.CommandText = "Select DateAllocated from t_RoomAllocationMatrix where roomid = '" & ds.Tables("0").Rows(i).Item("RoomID") & "' and ReservationID = '" & resID & "' order by dateallocated"
                    da.Fill(ds, "1")
                    If ds.Tables("1").Rows.Count > 0 Then
                        'set the in and out date for the room
                        sDate = CDate(ds.Tables("1").Rows(0).Item("DateAllocated"))
                        tempDate = CDate(ds.Tables("1").Rows(0).Item("DateAllocated"))
                        'Loop through to find the out date
                        For j = 0 To ds.Tables("1").Rows.Count - 1
                            'If Not (tempDate.Equals(CDate(ds.Tables("1").Rows(j).Item("DateAllocated")))) Then                                 'DateTime.Compare(CDate(tempDate), CDate(ds.Tables("1").Rows(j).Item("DateAllocated"))) <> 0 Then
                            If Not (DateDiff(DateInterval.Day, CDate(tempDate), CDate(ds.Tables("1").Rows(j).Item("DateAllocated"))) = 0) Then
                                dRow = dt.NewRow
                                dRow("RoomID") = ds.Tables("0").Rows(i).Item("RoomID")
                                dRow("RoomNumber") = ds.Tables("0").Rows(i).Item("RoomNumber")
                                dRow("Type") = ds.Tables("0").Rows(i).Item("Type") & " "
                                dRow("In Date") = sDate.ToShortDateString
                                dRow("Out Date") = tempDate.ToShortDateString
                                If ((DateTime.Compare(System.DateTime.Now, sDate) >= 0 And DateTime.Compare(System.DateTime.Now, tempDate) <= 0) Or (DateTime.Compare(System.DateTime.Now, sDate) <= 0)) Then
                                    dRow("Removable") = "YES"
                                Else
                                    dRow("Removable") = "NO"
                                End If
                                If (DateTime.Compare(System.DateTime.Now, sDate) >= 0 And DateTime.Compare(System.DateTime.Now, tempDate) <= 0) Or (DateTime.Compare(System.DateTime.Now, tempDate) < 0 And oCombo.Lookup_ComboItem(oRes.StatusID) = "Booked") Then
                                    dRow("Swappable") = "YES"
                                Else
                                    dRow("Swappable") = "NO"
                                End If
                                dt.Rows.Add(dRow)

                                'Reset both dates
                                sDate = CDate(ds.Tables("1").Rows(j).Item("DateAllocated")) '.ToString
                                tempDate = CDate(ds.Tables("1").Rows(j).Item("DateAllocated")) '.ToString
                                tempDate = tempDate.AddDays(1)
                            Else
                                'Still allocated so check the next day
                                tempDate = tempDate.AddDays(1)
                            End If
                        Next
                        'Add the room that had sequencial dates since it would never have been added in the loop above
                        dRow = dt.NewRow
                        dRow("RoomID") = ds.Tables("0").Rows(i).Item("RoomID")
                        dRow("RoomNumber") = ds.Tables("0").Rows(i).Item("RoomNumber")
                        dRow("Type") = ds.Tables("0").Rows(i).Item("Type") & " "
                        dRow("In Date") = sDate.ToShortDateString
                        dRow("Out Date") = tempDate.ToShortDateString
                        If ((DateTime.Compare(System.DateTime.Now, sDate) >= 0 And DateTime.Compare(System.DateTime.Now, tempDate) < 0) Or (DateTime.Compare(System.DateTime.Now, sDate) <= 0)) Then
                            dRow("Removable") = "YES"
                        Else
                            dRow("Removable") = "NO"
                        End If
                        If (DateTime.Compare(System.DateTime.Now, sDate) >= 0 And DateTime.Compare(System.DateTime.Now, tempDate) < 0) Or (DateTime.Compare(System.DateTime.Now, tempDate) < 0 And oCombo.Lookup_ComboItem(oRes.StatusID) = "Booked") Then
                            dRow("Swappable") = "YES"
                        Else
                            dRow("Swappable") = "NO"
                        End If
                        dt.Rows.Add(dRow)
                    End If
                    'Clear the table of the previous query records
                    ds.Tables("1").Clear()
                Next
                'dRow = dt.NewRow
                'dRow("RoomID") = ds.Tables("0").Rows(i).Item("RoomID")
                'dRow("RoomNumber") = ds.Tables("0").Rows(i).Item("RoomNumber")
                'dRow("Type") = ds.Tables("0").Rows(i).Item("Type")
                'dRow("In Date") = sDate
                'dRow("Out Date") = tempDate
                'dt.Rows.Add(dRow)
            Else
                'dRow = dt.NewRow
                'dRow("RoomID") = "A"
                'dRow("RoomNumber") = "A"
                'dRow("Type") = "A"
                'dRow("In Date") = "A"
                'dRow("Out Date") = "A"
                'dt.Rows.Add(dRow)

            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            cm = Nothing
            da = Nothing
            cn = Nothing
            ds = Nothing
        End Try
        Return dt
    End Function
    Public Sub Interface_Procedure(ByVal roomID As Integer, ByVal prosName As String, ByVal action As String, ByVal phone As String)
        Try
            Dim oMsg As New clsRoomMessages
            oMsg.SiteID = 1
            oMsg.Action = action
            oMsg.GuestName = prosName
            oMsg.Flag = 0
            oMsg.Extension = phone
            oMsg.PBXDateIn = System.DateTime.Now
            oMsg.UserName = _UserID
            oMsg.RoomID = roomID
            oMsg.Save()
            oMsg = Nothing
        Catch ex As Exception
            _Err = ex.Message
        End Try
    End Sub
    Public Function Interface_CheckIn(ByVal resID As Integer, ByVal prosName As String) As Boolean
        Dim bCheckIn As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(a.RoomID), b.Phone from t_RoomAllocationMatrix a inner join t_Room b on a.roomid = b.roomid where a.reservationid = '" & resID & "' and a.dateallocated >= '" & System.DateTime.Now & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                Interface_Procedure(dread("RoomID"), prosName, "CHECKIN", dread("phone"))
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bCheckIn
    End Function
    Public Function Check_In(ByVal resID As Integer, ByVal prosName As String) As Boolean
        Dim bCheckIn As Boolean = True
        Try
            Dim rmStatusID As Integer
            Dim oCombo As New clsComboItems
            rmStatusID = oCombo.Lookup_ID("RoomStatus", "Occupied")
            oCombo = Nothing
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(a.RoomID), b.Phone from t_RoomAllocationMatrix a inner join t_Room b on a.roomid = b.roomid where a.reservationid = '" & resID & "' and a.dateallocated >= '" & System.DateTime.Now & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                Dim oRoom As New clsRooms
                oRoom.RoomID = dread("RoomID")
                oRoom.Load()
                oRoom.StatusID = rmStatusID
                oRoom.UserID = _UserID
                oRoom.Save()
                oRoom = Nothing
                Interface_Procedure(dread("RoomID"), prosName, "CHECKIN", dread("phone"))
            Loop
            dread.Close()
            Dim oEvent As New clsEvents
            oEvent.KeyField = "ReservationID"
            oEvent.KeyValue = resID
            oEvent.EventType = "Check In"
            oEvent.CreatedByID = _UserID
            oEvent.Create_Event()
            oEvent = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bCheckIn
    End Function
    Public Function Check_Out(ByVal resID As Integer, ByVal prosName As String, ByVal outDate As Date, ByVal bEarly As Boolean) As Boolean
        Dim bCheckOut As Boolean = True
        Try
            Dim rmStatusID As Integer
            Dim oCombo As New clsComboItems
            rmStatusID = oCombo.Lookup_ID("RoomStatus", "Dirty")
            oCombo = Nothing
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(a.RoomID), b.Phone from t_RoomAllocationMatrix a inner join t_Room b on a.roomid = b.roomid where a.reservationid = '" & resID & "' and a.dateallocated = '" & outDate.ToShortDateString & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                Dim oRoom As New clsRooms
                oRoom.RoomID = dread("RoomID")
                oRoom.Load()
                oRoom.UserID = _UserID
                oRoom.StatusID = rmStatusID
                oRoom.Save()
                oRoom = Nothing
                Interface_Procedure(dread("RoomID"), prosName, "CHECKOUT", dread("phone"))
            Loop
            dread.Close()
            If bEarly Then
                cm.CommandText = "Update t_RoomAllocationMatrix set ReservationID = '0' where reservationid = '" & resID & "' and DateAllocated >= '" & System.DateTime.Now.ToShortDateString & "'"
                cm.ExecuteNonQuery()
            End If
            Dim oEvent As New clsEvents
            oEvent.KeyField = "ReservationID"
            oEvent.KeyValue = resID
            oEvent.EventType = "Check Out"
            oEvent.CreatedByID = _UserID
            oEvent.Create_Event()
            oEvent = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bCheckOut
    End Function
    Public Function check_Swappable(ByVal roomID As Integer, ByVal inDate As Date, ByVal outDate As Date) As Boolean
        Dim bValid As Boolean = False
        Dim resID As Integer = 0
        Try
            cm.CommandText = "Select ReservationID from t_RoomAllocationMatrix where roomid = '" & roomID & "' and dateallocated between '" & inDate & "' and '" & outDate & "' order by reservationid desc"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            If dread.GetValue(0).ToString & "" <> "" Then
                resID = dread.GetValue(0)
            End If
            dread.Close()
            If resID <> 0 Then
                cm.CommandText = "Select * from t_RoomAllocationMatrix where reservationID = '" & resID & "' and ((roomid = (Select LockoutID from t_Room where roomid = '" & roomID & "')) OR (roomid in (Select roomid from t_Room where lockoutid = '" & roomID & "')) or (RoomID in (Select RoomID from t_Room where unitid in (Select unit2id from t_Unit2Unit where unitid = (Select unitid from t_Room where roomid = '" & roomID & "'))))) and roomid <> '" & roomID & "'"
                dread = cm.ExecuteReader()
                dread.Read()
                If dread.HasRows Then
                    bValid = False
                Else
                    bValid = True
                End If
            Else
                bValid = True
            End If
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function swap_Room(ByVal roomID As Integer, ByVal newRoomID As Integer, ByVal resID As Integer, ByVal inDate As Date, ByVal outDate As Date) As Boolean
        Dim bValid As Boolean = True
        Dim usageA As Integer
        Dim resA As Integer
        Dim typeA As Integer
        Dim res2 As String = ""
        Dim usages As String = ""
        Dim usagesB As String = ""
        Dim dRow As DataRow
        'Dim cm1 As SqlCommand
        Dim bInHouse As Boolean = False
        Dim rStatus As Integer = 0
        'Try
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim i As Integer
        cm.CommandText = "Select * from t_RoomAllocationMatrix where reservationID = '" & resID & "' and roomID = '" & roomID & "' and dateallocated between '" & inDate & "' and '" & outDate & "'"
        If cn.State <> ConnectionState.Open Then cn.Open()
        da.Fill(ds, "OldRoom")
        If ds.Tables("OldRoom").Rows.Count > 0 Then
            For i = 0 To ds.Tables("OldRoom").Rows.Count - 1
                dRow = ds.Tables("OldRoom").Rows(i)
                usageA = dRow("UsageID")
                resA = dRow("ReservationID")
                typeA = dRow("TypeID")
                If usages = "" Then
                    usages = usageA
                Else
                    If InStr(usages, CStr(usageA)) = 0 Then
                        usages = usages & "," & CStr(usageA)
                    End If
                End If
                cm.CommandText = "Select * from t_RoomAllocationMatrix where roomid = '" & newRoomID & "' and dateallocated = '" & dRow("DateAllocated") & "'"
                dread = cm.ExecuteReader()
                dread.Read()
                If dread("UsageID") <> 0 Then
                    If InStr(usages, dread("UsageID")) = 0 Then
                        If usagesB = "" Then
                            usagesB = dread("UsageID")
                        Else
                            usagesB = usagesB & "," & dread("UsageID")
                        End If
                    End If
                End If
                If dread("ReservationID") <> 0 Then
                    If res2 = "" Then
                        res2 = dread("ReservationID")
                    Else
                        If InStr(res2, dread("ReservationID")) = 0 Then
                            res2 = res2 & "," & dread("ReservationID")
                        End If
                    End If
                End If
                dRow("UsageID") = dread("UsageID")
                dRow("ReservationID") = dread("ReservationID")
                dRow("TypeID") = dread("TypeID")
                Dim oRMatrix As New clsRoomAllocationMatrix
                oRMatrix.AllocationID = dread("AllocationID")
                oRMatrix.UserID = _UserID
                oRMatrix.Load()
                oRMatrix.TypeID = typeA
                oRMatrix.ReservationID = resA
                oRMatrix.UsageID = usageA
                oRMatrix.Save()
                oRMatrix.AllocationID = dRow("AllocationID")
                oRMatrix.Load()
                oRMatrix.UsageID = dread("UsageiD")
                oRMatrix.ReservationID = dread("ReservationID")
                oRMatrix.TypeID = dread("TypeID")
                oRMatrix.Save()
                oRMatrix = Nothing
                dread.Close()

                'cm1 = New SqlCommand("Update t_RoomAllocationMatrix set usageid = '" & usageA & "', reservationid = '" & resA & "', typeID = '" & typeA & "' where dateAllocated = '" & dRow("DateAllocated") & "' and roomid = '" & newRoomID & "'", cn)
                'cm1.ExecuteNonQuery()
            Next
            'Create Usage Event for Moving Room
            Dim usageIDs() As String = usages.Split(",")
            Dim usageBIDs() As String = usagesB.Split(",")
            Dim resIDs() As String = res2.Split(",")
            Dim oRoom As New clsRooms
            Dim oEvent As New clsEvents
            If usages <> "" Then
                For i = 0 To UBound(usageIDs)
                    oEvent.KeyField = "UsageID"
                    oEvent.KeyValue = usageIDs(i)
                    oEvent.EventType = "Move"
                    oRoom.RoomID = roomID
                    oRoom.Load()
                    oEvent.OldValue = oRoom.RoomNumber
                    oRoom.RoomID = newRoomID
                    oRoom.Load()
                    oEvent.NewValue = oRoom.RoomNumber
                    oEvent.CreatedByID = _UserID
                    oEvent.FieldName = "Room"
                    oEvent.Create_Event()
                Next
            End If

            If usagesB <> "" Then
                For i = 0 To UBound(usageBIDs)
                    oEvent.KeyField = "UsageID"
                    oEvent.KeyValue = usageBIDs(i)
                    oEvent.EventType = "Move"
                    oRoom.RoomID = newRoomID
                    oRoom.Load()
                    oEvent.OldValue = oRoom.RoomNumber
                    oRoom.RoomID = roomID
                    oRoom.Load()
                    oEvent.NewValue = oRoom.RoomNumber
                    oEvent.CreatedByID = _UserID
                    oEvent.FieldName = "Room"
                    oEvent.Create_Event()
                Next
            End If

            If res2 <> "" Then
                For i = 0 To UBound(resIDs)
                    oEvent.KeyField = "ReservationID"
                    oEvent.KeyValue = resIDs(i)
                    oEvent.EventType = "Move"
                    oRoom.RoomID = newRoomID
                    oRoom.Load()
                    oEvent.OldValue = oRoom.RoomNumber
                    oRoom.RoomID = roomID
                    oRoom.Load()
                    oEvent.NewValue = oRoom.RoomNumber
                    oEvent.CreatedByID = _UserID
                    oEvent.FieldName = "Room"
                    oEvent.Create_Event()
                Next
            End If
            oEvent = Nothing
            oRoom = Nothing
        End If
        Dim sc As New SqlCommandBuilder(da)
        'da.Update(ds, "OldRoom")

        'Interface CheckIn for new Room, Interface CheckOut for old Room
        Dim oCombo As New clsComboItems
        Dim oRes As New clsReservations
        oRes.ReservationID = resID
        oRes.Load()
        If oCombo.Lookup_ComboItem(oRes.StatusID) = "In-House" Then
            bInHouse = True
            Dim oPros As New clsProspect
            oPros.Prospect_ID = oRes.ProspectID
            Dim oRoom As New clsRooms
            Dim phone As String
            Dim prosName As String = ""
            oRes.ReservationID = resID
            oRes.Load()
            oPros.Prospect_ID = oRes.ProspectID
            oRes = Nothing
            oPros.Load()
            prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
            oPros = Nothing
            oRoom.RoomID = newRoomID
            oRoom.Load()
            phone = oRoom.Phone
            oRoom.StatusID = oCombo.Lookup_ID("RoomStatus", "Occupied")
            oRoom.UserID = _UserID
            oRoom.Save()
            Interface_Procedure(newRoomID, prosName, "CHECKIN", phone)
            oRoom.RoomID = roomID
            oRoom.Load()
            phone = oRoom.Phone
            oRoom.StatusID = oCombo.Lookup_ID("RoomStatus", "Dirty")
            oRoom.UserID = _UserID
            oRoom.Save()
            Interface_Procedure(roomID, prosName, "CHECKOUT", phone)
            oRoom = Nothing
        End If

        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bValid
    End Function
    Public Function move_To_Spare(ByVal roomID As Integer, ByVal newRoomID As Integer, ByVal resID As Integer, ByVal inDate As Date, ByVal outDate As Date) As Boolean
        Dim bValid As Boolean = True
        Dim usageA As Integer
        Dim usages As String = ""
        Dim resA As Integer
        Dim typeA As Integer
        Dim dRow As DataRow
        Dim dRow2 As DataRow
        Dim trans As SqlTransaction
        Dim bInHouse As Boolean = False
        Dim prosName As String = ""
        Dim rmStatus As Integer = 0
        If cn.State <> ConnectionState.Open Then cn.Open()
        trans = cn.BeginTransaction()
        Try
            Dim oRes As New clsReservations
            Dim oCombo As New clsComboItems
            oRes.ReservationID = resID
            oRes.Load()
            If oCombo.Lookup_ComboItem(oRes.StatusID) = "In-House" Then
                bInHouse = True
                Dim oPros As New clsProspect
                oPros.Prospect_ID = oRes.ProspectID
                oPros.Load()
                prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
                oPros = Nothing
            End If
            oRes = Nothing
            Dim i As Integer
            cm.CommandText = "Select * from t_RoomAllocationMatrix where reservationID = '" & resID & "' and roomID = '" & roomID & "' and dateallocated between '" & inDate & "' and '" & outDate & "'"
            cm.Transaction = trans
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "OldRoom")
            If ds.Tables("OldRoom").Rows.Count > 0 Then
                For i = 0 To ds.Tables("OldRoom").Rows.Count - 1
                    dRow = ds.Tables("OldRoom").Rows(i)
                    usageA = dRow("UsageID")
                    resA = dRow("ReservationID")
                    typeA = dRow("TypeID")
                    If usages = "" Then
                        usages = usageA
                    Else
                        If InStr(usages, usageA) = 0 Then
                            usages = usages & "," & usageA
                        End If
                    End If
                    cm.CommandText = "Select * from t_RoomAllocationMatrix where roomid = '" & newRoomID & "' and dateallocated = '" & dRow("DateAllocated") & "'"
                    da.Fill(ds, "NewRoom")
                    dRow2 = ds.Tables("NewRoom").Rows(0)
                    If dRow2("UsageID") > 0 Or dRow2("ReservationID") <> 0 Then
                        Throw New Exception("Room Has Been Reserved or Assigned to Usage.")
                    Else
                        Dim oRMatrix As New clsRoomAllocationMatrix
                        oRMatrix.UserID = _UserID
                        oRMatrix.AllocationID = dRow("AllocationID")
                        oRMatrix.Load()
                        oRMatrix.UsageID = 0
                        oRMatrix.ReservationID = 0
                        oRMatrix.TypeID = oCombo.Lookup_ID("ReservationType", "Spare")
                        oRMatrix.Save()

                        'dRow("UsageID") = 0
                        'dRow("ReservationID") = 0
                        'dRow("TypeID") = oCombo.Lookup_ID("ReservationType", "Spare")
                        oRMatrix.AllocationID = dRow2("AllocationID")
                        oRMatrix.Load()
                        oRMatrix.UsageID = usageA
                        oRMatrix.ReservationID = resA
                        oRMatrix.TypeID = typeA
                        oRMatrix.Save()
                        'dRow2("UsageID") = usageA
                        'dRow2("ReservationID") = resA
                        'dRow2("TypeID") = typeA
                        oRMatrix = Nothing
                    End If
                    Dim sc2 As New SqlCommandBuilder(da)
                    'da.Update(ds, "NewRoom")
                    ds.Tables("NewRoom").Clear()
                Next

                'Create Usage Event for Moving Room
                If usages <> "" Then
                    Dim usageIDs() As String = usages.Split(",")
                    Dim oRoom As New clsRooms
                    Dim oEvent As New clsEvents
                    For i = 0 To UBound(usageIDs)
                        oEvent.KeyField = "UsageID"
                        oEvent.KeyValue = usageIDs(i)
                        oEvent.EventType = "Move"
                        oRoom.RoomID = roomID
                        oRoom.Load()
                        oEvent.OldValue = oRoom.RoomNumber
                        oRoom.RoomID = newRoomID
                        oRoom.Load()
                        oEvent.NewValue = oRoom.RoomNumber
                        oEvent.CreatedByID = _UserID
                        oEvent.FieldName = "Room"
                        oEvent.Create_Event()
                    Next
                    oRoom = Nothing
                    oEvent = Nothing
                End If
            End If
            cm.CommandText = "Select * from t_RoomAllocationMatrix where reservationID = '" & resID & "' and roomID = '" & roomID & "' and dateallocated between '" & inDate & "' and '" & outDate & "'"
            Dim sc As New SqlCommandBuilder(da)
            'da.Update(ds, "OldRoom")

            If bInHouse = True Then
                Dim oRoom As New clsRooms
                oRoom.RoomID = roomID
                oRoom.Load()
                Interface_Procedure(roomID, prosName, "CHECKOUT", oRoom.Phone)
                oRoom.StatusID = oCombo.Lookup_ID("RoomStatus", "Dirty")
                oRoom.UserID = _UserID
                oRoom.Save()
                oRoom.RoomID = newRoomID
                oRoom.Load()
                Interface_Procedure(roomID, prosName, "CHECKIN", oRoom.Phone)
                oRoom.StatusID = oCombo.Lookup_ID("RoomStatus", "Occupied")
                oRoom.UserID = _UserID
                oRoom.Save()
                oRoom = Nothing
            End If
            trans.Commit()
            bValid = True
            oCombo = Nothing
        Catch ex As Exception
            trans.Rollback()
            _Err = ex.Message
            Throw New Exception(ex.Message)
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function val_Res_Location(ByVal resID As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            cm.CommandText = "Select rl.ComboItem as ResLocation from t_Reservations r left outer join t_CombOItems rl on r.ResLocationID = rl.ComboItemID where r.ReservationID = '" & resID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            If dread.GetValue(0).ToString & "" = "KCP" Then
                bValid = True
            End If
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function val_Res_Amt(ByVal resID As Integer, ByVal minAmt As Long) As Boolean
        Dim bValid As Boolean = True
        Try
            Dim resType As String = Get_ResType(resID)
            If resType = "Marketing" Or resType = "Rental" Or resType = "Developer" Or resType = "Vendor" Then
                cm.CommandText = "SELECT Sum(i.Amount) As Amount FROM t_Invoices i INNER JOIN t_FinTransCodes f ON i.FinTransID = f.FinTransID INNER JOIN t_ComboItems tt ON f.TransTypeID = tt.ComboItemID WHERE (f.RoomCharge = '1') AND (tt.ComboItem = 'ReservationTrans') and i.KeyField = 'ReservationID' and i.KeyValue = '" & resID & "'"
                If cn.State <> ConnectionState.Open Then cn.Open()
                dread = cm.ExecuteReader()
                dread.Read()
                If dread.GetValue(0) Is System.DBNull.Value Then
                    bValid = False
                ElseIf CLng(dread.GetValue(0)) < minAmt Then
                    bValid = False
                End If
                dread = Nothing
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Get_Usages(ByVal resID As Integer) As String
        Dim usages As String = ""
        Try
            cm.CommandText = "Select Distinct(rmx.UsageID) As UsageID, u.ContractID, c.ContractNumber, p.FirstName + ' ' + p.LastName as Prospect from t_RoomAllocationMatrix rmx inner join t_Usage u on rmx.UsageID = u.UsageID inner join t_Contract c on u.ContractID = c.COntractID inner join t_Prospect p on c.ProspectID = p.ProspectID where rmx.ReservationID = '" & resID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            Do While dread.Read
                If usages = "" Then
                    usages = dread("UsageID") & "|" & dread("ContractID") & "|" & dread("ContractNumber") & "|" & dread("Prospect")
                Else
                    usages = usages & "|" & dread("UsageID") & "|" & dread("ContractID") & "|" & dread("ContractNumber") & "|" & dread("Prospect")
                End If
            Loop
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return usages
    End Function
    Public Function Get_ResType(ByVal resID As Integer) As String
        Dim resType As String = ""
        Try
            cm.CommandText = "Select rs.ComboItem as ResType from t_Reservations r left outer join t_ComboItems rs on r.TypeID = rs.ComboItemID where r.ReservationID = '" & resID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            resType = dread.GetValue(0).ToString & ""
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return resType
    End Function
    Public Function Get_Room_Count(ByVal resID As Integer) As Integer
        Dim rCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(Distinct(RoomID)) as Rooms from t_RoomAllocationmatrix where reservationID = '" & resID & "'"
            dread = cm.ExecuteReader()
            dread.Read()
            rCount = dread("Rooms")
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return rCount
    End Function
    Public Function val_CheckIn_Financials(ByVal resID As Integer) As Boolean
        Dim bProceed As Boolean = False
        Dim rbalance As Double = 0
        Dim ccTransID As String = ""
        Dim dRead2 As SqlDataReader
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            '***Check sum of invoices that are allowed to be checked in with a balance
            cm.CommandText = "Select Case when Sum(a.balance) is Null then 0 else Sum(a.Balance) end As Balance from ufn_Financials(0,'ReservationID','" & resID & "',0) a inner join t_Invoices i on a.ID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FintransiD where f.OpenBalanceCheckIn = 0"
            dread = cm.ExecuteReader()
            dread.Read()
            If dread("Balance") > 0 Then
                '****Check to see if there is a cleaning fee.
                dread.Close()
                cm.CommandText = "Select * from t_Invoices a inner join t_FinTransCodes f on a.Fintransid = f.fintransid inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID where tc.ComboItem = 'Cleaning Fee' and i.KeyField = '" & ReservationID & "' and i.KeyValue = '" & resID & "'"
                dread = cm.ExecuteReader
                dread.Read()
                If dread.HasRows Then
                    dread.Close()
                    cm.CommandText = "Select * from ufn_Financials(0,'ReservationID','" & resID & "') a inner join t_Invoices i on a.invoiceID = i.InvoiceID inner join t_FinTransCodes f on i.fintransid = f.fintransid where f.OpenBalanceCheckIn = 1"
                    dread = cm.ExecuteReader
                    dread.Read()
                    If dread.HasRows Then
                        dread.Close()
                        cm.CommandText = "Select a.Balance, a.ID from ufn_Financials(0,'ReservationID','" & resID & "') a inner join t_Invoices i on a.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.fintransid = f.fintransid where f.OpenBalanceCheckIn = 0 and a.Invoice <> 'Cleaning Fee'"
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            Do While dread.Read
                                rbalance = rbalance + dread("Balance")
                                cm.CommandText = "Select CCTransID from t_CCTrans a inner join t_ComboItems b on a.TransTypeID = b.ComboItemID where a.Approved = '0' and a.Imported = '0' and (b.ComboItem = 'Charge' or b.ComboItem = 'Force') and (a.applyto = '" & dread("InvoiceID") & "' or a.applyto like '%," & dread("InvoiceID") & ",%' or a.applyto like '" & dread("InvoiceID") & ",%' or a.applyto like '%" & dread("InvoiceID") & "')"
                                dRead2 = cm.ExecuteReader
                                Do While dRead2.Read
                                    If InStr(ccTransID, dRead2("CCTransID")) = 0 Then
                                        If ccTransID = "" Then
                                            ccTransID = "'" & dRead2("CCTransID") & "'"
                                        Else
                                            ccTransID = ccTransID & ",'" & dRead2("CCTransID") & "'"
                                        End If
                                    End If
                                Loop
                                dRead2.Close()
                            Loop
                            dread.Close()
                            If rbalance > 0 And ccTransID <> "" Then
                                cm.CommandText = "Select Case when Sum(Amount) is Null then 0 else sum(Amount) end as CCAmt from t_CCTrans where cctransid in (" & ccTransID & ")"
                                dread = cm.ExecuteReader
                                dread.Read()
                                If dread("CCAmt") > 0 Then
                                    If CDbl(dread("CCAmt")) >= CDbl(rbalance) Then
                                        bProceed = True
                                    End If
                                End If
                                dread.Close()
                            End If
                        Else
                            bProceed = True
                        End If
                    Else
                        dread.Close()
                        cm.CommandText = "Select * from ufn_Financials(0,'ReservationID','" & resID & "') a inner join t_Invoices i on a.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.fintransid where a.balance > 0 and f.OpenBalanceCheckIn = 0"
                        dread = cm.ExecuteReader
                        Do While dread.Read
                            rbalance = rbalance + dread("Balance")
                            cm.CommandText = "Select CCTransID from t_CCTrans a inner join t_ComboItems b on a.TransTypeID = b.ComboItemID where a.Approved = '0' and a.Imported = '0' and (b.ComboItem = 'Charge' or b.ComboItem = 'Force') and (a.applyto = '" & dread("InvoiceID") & "' or a.applyto like '%," & dread("InvoiceID") & ",%' or a.applyto like '" & dread("InvoiceID") & ",%' or a.applyto like '%" & dread("InvoiceID") & "')"
                            dRead2 = cm.ExecuteReader
                            Do While dRead2.Read
                                If InStr(ccTransID, dRead2("CCTransID")) = 0 Then
                                    If ccTransID = "" Then
                                        ccTransID = "'" & dRead2("CCTransID") & "'"
                                    Else
                                        ccTransID = ccTransID & ",'" & dRead2("CCTransID") & "'"
                                    End If
                                End If
                            Loop
                            dRead2.Close()
                        Loop
                        dread.Close()
                        If rbalance > 0 And ccTransID <> "" Then
                            cm.CommandText = "Select Case when Sum(Amount) is Null then 0 else sum(Amount) end as CCAmt from t_CCTrans where cctransid in (" & ccTransID & ")"
                            dread = cm.ExecuteReader
                            dread.Read()
                            If dread("CCAmt") > 0 Then
                                If CDbl(dread("CCAmt")) >= CDbl(rbalance) Then
                                    bProceed = True
                                End If
                            End If
                            dread.Close()
                        End If
                    End If
                Else
                    'No Cleaning Fee so pull all invoices 
                    dread.Close()
                    cm.CommandText = "Select * from ufn_Financials(0,'ReservationID','" & resID & "') a inner join t_Invoices i on a.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.fintransid where a.balance > 0 and f.OpenBalanceCheckIn = 0"
                    dread = cm.ExecuteReader
                    Do While dread.Read
                        rbalance = rbalance + dread("Balance")
                        cm.CommandText = "Select CCTransID from t_CCTrans a inner join t_ComboItems b on a.TransTypeID = b.ComboItemID where a.Approved = '0' and a.Imported = '0' and (b.ComboItem = 'Charge' or b.ComboItem = 'Force') and (a.applyto = '" & dread("InvoiceID") & "' or a.applyto like '%," & dread("InvoiceID") & ",%' or a.applyto like '" & dread("InvoiceID") & ",%' or a.applyto like '%" & dread("InvoiceID") & "')"
                        dRead2 = cm.ExecuteReader
                        Do While dRead2.Read
                            If InStr(ccTransID, dRead2("CCTransID")) = 0 Then
                                If ccTransID = "" Then
                                    ccTransID = "'" & dRead2("CCTransID") & "'"
                                Else
                                    ccTransID = ccTransID & ",'" & dRead2("CCTransID") & "'"
                                End If
                            End If
                        Loop
                        dRead2.Close()
                    Loop
                    dread.Close()
                    If rbalance > 0 And ccTransID <> "" Then
                        cm.CommandText = "Select Case when Sum(Amount) is Null then 0 else sum(Amount) end as CCAmt from t_CCTrans where cctransid in (" & ccTransID & ")"
                        dread = cm.ExecuteReader
                        dread.Read()
                        If dread("CCAmt") > 0 Then
                            If CDbl(dread("CCAmt")) >= CDbl(rbalance) Then
                                bProceed = True
                            End If
                        End If
                    End If
                End If
            Else
                dread.Close()
                dRead2 = Nothing
                bProceed = True
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return (bProceed)
    End Function
    Public Function Concierge_Rpt(ByVal sdate As Date, ByVal eDate As Date, ByVal opt As String) As DataTable
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt.Columns.Add("Guest Name")
        dt.Columns.Add("Owner (Y/N)")
        dt.Columns.Add("DoNotTour")
        dt.Columns.Add("ReservationID")
        dt.Columns.Add("D-Paper/Resort Finance")
        dt.Columns.Add("Source")
        dt.Columns.Add("Type")
        dt.Columns.Add("SubType")
        dt.Columns.Add("Campaign")
        dt.Columns.Add("Phone")
        dt.Columns.Add("Unit")
        dt.Columns.Add("CheckIn")
        dt.Columns.Add("CheckOut")
        dt.Columns.Add("Last Tour")
        dt.Columns.Add("Phone Numbers")
        dt.Columns.Add("Email")
        dt.Columns.Add("Address")
        dt.Columns.Add("Book")
        dt.Columns.Add("Extra Tour")
        dt.Columns.Add("HaveComments")
        dt.Columns.Add("DNS")
        dt.Columns.Add("DNC")
        dt.Columns.Add("Status")
        dt.Columns.Add("Comment1")
        dt.Columns.Add("Comment2")
        dt.Columns.Add("Comment3")
        dt.Columns.Add("Comment4")
        dt.Columns.Add("Comment5")
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim i As Integer = 0

        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If opt = "Booked" Then
                cm.CommandText = "Select r.ReservationID, (select top 1 Prospectid from v_DoNottourList where prospectid = r.ProspectID) as DoNotTour, (select top 1 case when soldinventoryid is null then 'N' else 'Y' end from t_Soldinventory s inner join t_Contract c on c.contractid = s.contractid where c.prospectid = r.prospectid) as Owner,  r.ProspectID, p.LastName + ', ' + p.FirstName as Guest, r.CheckInDate, r.CheckOutDate, rs.ComboItem as Source, rt.ComboItem as Type, rst.ComboItem as Subtype, camp.Name as Campaign, st.ComboItem as Status from t_Reservations r inner join t_Prospect p on r.ProspectID = p.ProspectID left outer join t_ComboItems rs on r.SourceID = rs.ComboItemID left outer join t_ComboItems rt on r.TypeID = rt.ComboItemID left outer join t_CombOitems rst on r.SubTypeID = rst.ComboItemID left outer join t_ComboItems st on r.StatusID = st.ComboItemID left outer join t_Tour t on t.ReservationID = r.ReservationID left outer join t_Campaign camp on t.CampaignID = camp.CampaignID inner join t_comboItems rl on r.ResLocationID = rl.CombOitemID where r.CheckInDate between '" & sdate & "' and '" & eDate & "' and st.ComboItem in ('Booked','In-House') and rl.ComboItem = 'KCP' order by p.lastname + ', ' + p.firstname"
            Else
                cm.CommandText = "Select r.ReservationID, (select top 1 Prospectid from v_DoNottourList where prospectid = r.ProspectID) as DoNotTour, (select top 1 case when soldinventoryid is null then 'N' else 'Y' end from t_Soldinventory s inner join t_Contract c on c.contractid = s.contractid where c.prospectid = r.prospectid) as Owner,  r.ProspectID, p.LastName + ', ' + p.FirstName as Guest, r.CheckInDate, r.CheckOutDate, rs.ComboItem as Source, rt.ComboItem as Type, rst.ComboItem as Subtype, camp.Name as Campaign, st.ComboItem as Status from t_Reservations r inner join t_Prospect p on r.ProspectID = p.ProspectID left outer join t_ComboItems rs on r.SourceID = rs.ComboItemID left outer join t_ComboItems rt on r.TypeID = rt.ComboItemID left outer join t_CombOitems rst on r.SubTypeID = rst.ComboItemID left outer join t_ComboItems st on r.StatusID = st.ComboItemID left outer join t_Tour t on t.ReservationID = r.ReservationID left outer join t_Campaign camp on t.CampaignID = camp.CampaignID inner join t_comboItems rl on r.ResLocationID = rl.CombOitemID where r.CheckInDate between '" & sdate & "' and '" & eDate & "' and st.ComboItem not in ('Booked','In-House') and rl.ComboItem = 'KCP' order by p.lastname + ', ' + p.firstname"
            End If
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                For i = 0 To ds.Tables("0").Rows.Count - 1
                    Dim bDel As Boolean = False
                    dRow = dt.NewRow
                    dRow("Guest Name") = ds.Tables("0").Rows(i).Item("Guest")
                    dRow("Owner (Y/N)") = ds.Tables("0").Rows(i).Item("Owner")
                    Dim oPros As New clsProspect
                    If oPros.Check_Owner(ds.Tables("0").Rows(i).Item("ProspectID")) Then
                        Dim oMort As New clsMortgage
                        bDel = oMort.Delinquent_Check(ds.Tables("0").Rows(i).Item("ProspectID"))
                        oMort = Nothing
                    End If
                    oPros = Nothing
                    dRow("ReservationID") = ds.Tables("0").Rows(i).Item("ReservationID")
                    dRow("Source") = ds.Tables("0").Rows(i).Item("Source")
                    dRow("Type") = ds.Tables("0").Rows(i).Item("Type")
                    dRow("SubType") = ds.Tables("0").Rows(i).Item("SubType")
                    dRow("Campaign") = ds.Tables("0").Rows(i).Item("Campaign")
                    dRow("CheckIn") = ds.Tables("0").Rows(i).Item("CheckInDate")
                    If Not IsDBNull(dRow("CheckIn")) Then
                        dRow("CheckIn") = FormatDateTime(dRow("CheckIn"), DateFormat.ShortDate)
                    End If
                    dRow("CheckOut") = ds.Tables("0").Rows(i).Item("CheckOutDate")
                    If Not IsDBNull(dRow("CheckOut")) Then
                        dRow("CheckOut") = FormatDateTime(dRow("CheckOut"), DateFormat.ShortDate)
                    End If
                    If Not (IsDBNull(ds.Tables("0").Rows(i).Item("DoNotTour"))) Then
                        dRow("DoNotTour") = "Do Not Tour"
                    Else
                        dRow("DoNotTour") = ""
                    End If

                    cm.CommandText = "Select c.ContractNumber, cs.ComboItem from t_Contract c left outer join t_Comboitems cs on c.StatusID = cs.ComboItemID where c.ProspectID = '" & ds.Tables("0").Rows(i).Item("ProspectID") & "' and (cs.ComboItem = 'Developer' or c.Contractnumber like '%X%')"
                    da.Fill(ds, "1")
                    If ds.Tables("1").Rows.Count > 0 Then
                        If InStr(ds.Tables("1").Rows(0).Item("ContractNumber"), "X") > 0 Then
                            dRow("D-Paper/Resort Finance") = "Resort Finance"
                        Else
                            dRow("D-Paper/Resort Finance") = "D-Paper"
                        End If
                    Else
                        dRow("D-Paper/Resort Finance") = ""
                    End If
                    ds.Tables("1").Clear()
                    cm.CommandText = "Select Distinct(r.RoomNumber), r.Phone from t_RoomAllocationMatrix rmx inner join t_Room r on rmx.RoomID = r.RoomID where rmx.ReservationID = '" & ds.Tables("0").Rows(i).Item("ReservationID") & "'"
                    da.Fill(ds, "2")
                    If ds.Tables("2").Rows.Count > 0 Then
                        dRow("Phone") = ds.Tables("2").Rows(0).Item("Phone")
                        dRow("Unit") = ds.Tables("2").Rows(0).Item("RoomNumber")
                    Else
                        dRow("Phone") = ""
                        dRow("Unit") = ""
                    End If
                    ds.Tables("2").Clear()
                    cm.CommandText = "Select Top 1 t.tourdate from t_Tour t inner join t_ComboItems ts on t.StatusID = ts.ComboItemID where t.prospectid = '" & ds.Tables("0").Rows(i).Item("ProspectID") & "' and (ts.ComboItem = 'Showed' or ts.ComboItem = 'Booked') order by t.tourdate desc"
                    da.Fill(ds, "3")
                    If ds.Tables("3").Rows.Count > 0 Then
                        If IsDBNull(ds.Tables("3").Rows(0).Item("TourDate")) Then
                            dRow("Last Tour") = "N/A"
                        Else
                            dRow("Last Tour") = ds.Tables("3").Rows(0).Item("TourDate")
                        End If
                    Else
                        dRow("Last Tour") = "N/A"
                    End If
                    ds.Tables("3").Clear()

                    cm.CommandText = "select *, (select top 1 prospectid from t_DoNotSellList l where l.prospectid in (select prospectid from t_ProspectPhone where number = pp.number and number is not null and number <> '')) as DNS,(select top 1 ps.comboitem from t_Comboitems ps inner join t_Prospect p on p.statusid=ps.comboitemid where p.prospectid=pp.prospectid and ps.comboitem = 'Do Not Call') as DNC from t_ProspectPhone pp  where pp.prospectid  = '" & ds.Tables("0").Rows(i).Item("ProspectID") & "' and pp.active = 1"
                    da.Fill(ds, "6")
                    Dim sPhoneNumbers As String = ""
                    Dim DNS As Boolean = False
                    Dim DNC As Boolean = False
                    If ds.Tables("6").Rows.Count > 0 Then
                        For z As Integer = 0 To ds.Tables("6").Rows.Count - 1
                            If Not IsDBNull(ds.Tables("6").Rows(z).Item("Number")) Then
                                sPhoneNumbers &= IIf(sPhoneNumbers = "", ds.Tables("6").Rows(z).Item("Number").ToString, ", " & ds.Tables("6").Rows(z).Item("Number").ToString)
                            Else
                                sPhoneNumbers = "N/A"
                            End If
                            If Not IsDBNull(ds.Tables("6").Rows(z).Item("DNS")) Then DNS = True
                            If Not IsDBNull(ds.Tables("6").Rows(z).Item("DNC")) Then DNC = True
                        Next

                    Else
                        sPhoneNumbers = "N/A"

                    End If
                    dRow("Phone Numbers") = sPhoneNumbers
                    dRow("DNS") = IIf(DNS, "Y", "N")
                    dRow("DNC") = IIf(DNC, "Y", "N")
                    dRow("Status") = ds.Tables("0").Rows(i).Item("Status")
                    ds.Tables("6").Clear()

                    Dim sEmail As String = ""
                    cm.CommandText = "select * from t_ProspectEmail where prospectid  = '" & ds.Tables("0").Rows(i).Item("ProspectID") & "' and isactive = 1"
                    da.Fill(ds, "6")
                    If ds.Tables("6").Rows.Count > 0 Then
                        For z As Integer = 0 To ds.Tables("6").Rows.Count - 1
                            If Not IsDBNull(ds.Tables("6").Rows(z).Item("Email")) Then
                                sEmail &= IIf(sEmail = "", ds.Tables("6").Rows(z).Item("Email"), ", " & ds.Tables("6").Rows(z).Item("Email"))
                            Else
                                sEmail = "N/A"
                            End If

                        Next

                    Else
                        sEmail = "N/A"

                    End If
                    dRow("email") = sEmail
                    ds.Tables("6").Clear()




                    Dim address As String = String.Empty
                    cm.CommandText = String.Format(
                        "select top 1 * from t_ProspectAddress pa inner join t_comboitems co on pa.stateid = co.comboitemid  where ActiveFlag = 1  " &
                        "and prospectid = {0} order by addressid desc", ds.Tables("0").Rows(i).Item("ProspectID"))
                    da.Fill(ds, "Address")

                    If ds.Tables("Address").Rows.Count > 0 Then

                        Dim address_table As DataTable = ds.Tables("Address")
                        dRow("Address") = String.Format("{0}   {1}, {2}, {3}",
                                                        address_table.Rows(0).Item("Address1"),
                                                        address_table.Rows(0).Item("City"),
                                                        address_table.Rows(0).Item("ComboItem"),
                                                        address_table.Rows(0).Item("PostalCode"))

                    End If

                    ds.Tables("Address").Clear()


                    cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end As Comments from t_Comments where KeyField = 'Reservation' and KeyValue = '" & ds.Tables("0").Rows(i).Item("ReservationID") & "'"
                    da.Fill(ds, "4")
                    If ds.Tables("4").Rows.Count > 0 Then
                        If ds.Tables("4").Rows(0).Item("Comments") > 0 Then
                            dRow("HaveComments") = "YES"
                        Else
                            dRow("HaveComments") = "NO"
                        End If
                    Else
                        dRow("HaveComments") = "NO"
                    End If
                    ds.Tables("4").Clear()
                    cm.CommandText = "Select top 5 Comment from t_Comments where keyfield = 'Reservation' and keyvalue = '" & ds.Tables("0").Rows(i).Item("ReservationID") & "'"
                    dread = cm.ExecuteReader
                    Dim xx As Integer = 1
                    If dread.HasRows Then
                        Do While dread.Read
                            dRow("Comment" & xx) = dread("Comment")
                            xx = xx + 1
                        Loop
                    End If
                    dread.Close()
                    If DNS = False And bDel = False Then
                        dt.Rows.Add(dRow)
                    End If
                Next
            End If
            da.Dispose()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function

    Public Function Check_Taxes(ByVal resID As Integer) As Boolean
        Dim bTaxed As Boolean = True
        '        Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Execute SP_RES_CHECK_OUT_TAXES " & resID & ""
        dread = cm.ExecuteReader
        dread.Read()
        If dread("Taxes") = 1 Then
            bTaxed = True
        Else
            bTaxed = False
        End If
        dread.Close()
        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bTaxed
    End Function

    Public Function Get_BD_Count(ByVal resID As Integer) As Integer
        Dim bdCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "SELECT SUM(CAST(LEFT(BDTYPE, 1) AS integer)) AS BD FROM (SELECT DISTINCT rmx.RoomID, rm.TypeID, rt.ComboItem AS BDTYPE FROM t_RoomAllocationMatrix rmx INNER JOIN t_Room rm ON rmx.RoomID = rm.RoomID INNER JOIN t_ComboItems rt ON rm.TypeID = rt.ComboItemID WHERE (rmx.ReservationID = " & resID & ")) xx"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                bdCount = dread("BD")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bdCount
    End Function

    Public Function Get_Total_Balance(ByVal resID As Integer) As Double
        Dim balance As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(Balance) as Balance from ufn_Financials(0,'ReservationID'," & resID & ",0)"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                balance = dread("Balance")
            Else
                balance = 0
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return balance
    End Function
    Public Function Get_Inv_Amount(ByVal resID As Integer) As Double
        Dim balance As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(Amount) as Amount from ufn_Financials(0,'ReservationID'," & resID & ",0)"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                balance = dread("Amount")
            Else
                balance = 0
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return balance
    End Function
    Public Function Get_Total_Payments(ByVal resID As Integer) As Double
        Dim payments As Double = 0
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim i As Integer = 0
        Dim j As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select p.* from t_Payments p inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID where i.KeyField = 'ReservationID' and i.KeyValue = " & resID & " and p.Adjustment = 0 and p.ApplyToID = 0"
            'cm.CommandText = "Select SUM(Amount + Adjustment) as Amt from v_payments where KeyField = 'ReservationID' and IsAdjustment = 0 and KeyValue = " & resID
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                For i = 0 To ds.Tables("0").Rows.Count - 1
                    If ds.Tables("0").Rows(i).Item("PosNeg") = False Then
                        payments = payments + (CDbl(ds.Tables("0").Rows(i).Item("Amount")) * -1)
                    Else
                        payments = payments + CDbl(ds.Tables("0").Rows(i).Item("Amount"))
                    End If
                    cm.CommandText = "Select * from t_Payments where ApplyToID = " & ds.Tables("0").Rows(i).Item("PaymentID")
                    da.Fill(ds, "1")
                    If ds.Tables("1").Rows.Count > 0 Then
                        For j = 0 To ds.Tables("1").Rows.Count - 1
                            If ds.Tables("1").Rows(j).Item("PosNeg") = False Then
                                payments = payments + (CDbl(ds.Tables("1").Rows(j).Item("Amount")) * -1)
                            Else
                                payments = payments + CDbl(ds.Tables("1").Rows(j).Item("Amount"))
                            End If
                        Next
                        ds.Tables("1").Clear()
                    End If
                Next
            End If
            da.Dispose()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return payments
    End Function
    Public Function Get_Total_Adjustments(ByVal resID As Integer) As Double
        Dim payments As Double = 0
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim i As Integer = 0
        Dim j As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select p.* from t_Payments p inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID where i.KeyField = 'ReservationID' and i.KeyValue = " & resID & " and p.Adjustment = 1"
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                For i = 0 To ds.Tables("0").Rows.Count - 1
                    If ds.Tables("0").Rows(i).Item("PosNeg") = False Then
                        payments = payments + (CDbl(ds.Tables("0").Rows(i).Item("Amount")) * -1)
                    Else
                        payments = payments + CDbl(ds.Tables("0").Rows(i).Item("Amount"))
                    End If
                    cm.CommandText = "Select * from t_Payments where ApplyToID = " & ds.Tables("0").Rows(i).Item("PaymentID")
                    da.Fill(ds, "1")
                    If ds.Tables("1").Rows.Count > 0 Then
                        For j = 0 To ds.Tables("1").Rows.Count - 1
                            If ds.Tables("1").Rows(j).Item("PosNeg") = False Then
                                payments = payments + (CDbl(ds.Tables("1").Rows(j).Item("Amount")) * -1)
                            Else
                                payments = payments + CDbl(ds.Tables("1").Rows(j).Item("Amount"))
                            End If
                        Next
                    End If
                Next
            End If
            da.Dispose()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return payments
    End Function

    Public Function Conf_Letter_Body(ByVal resID As Integer) As String
        Dim sAns As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select x.*, pa.Address1, pa.Address2, pa.City, st.ComboItem as State, pa.PostalCode from (SELECT distinct r.ReservationID, p.ProspectID, p.FirstName + ' ' + p.LastName as ProspectName, (Select Top 1 AddressID from t_ProspectAddress where prospectid = p.prospectid and activeflag = 1) as AddressID,  " &
                                    "r.CheckInDate, r.CheckOutDate, " &
                                    "( " &
                                    "select cast(sum(cast(left(ty.comboitem,1)as int))as varchar(2)) as RoomSize " &
                                    "from  t_Room rm " &
                                    "INNER JOIN t_ComboItems ty on ty.ComboItemID = rm.TypeID " &
                                    "where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " &
                                    ") + 'BD' as RoomSize, " &
                                    "( " &
                                    "select sum(MaxOccupancy) " &
                                    "from t_Room rm " &
                                    "where rm.roomid in (select distinct roomid from t_Roomallocationmatrix where reservationid = r.reservationid) " &
                                    ") as MaxOccupancy " &
                                    "FROM t_Prospect p " &
                                    "INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID " &
                                    "WHERE r.ReservationID in (" & resID & ")) x " &
                                "LEFT OUTER JOIN t_ProspectAddress pa on x.AddressID = pa.AddressID " &
                                "LEFT OUTER JOIN t_ComboItems st on pa.StateID = st.ComboITEMID"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                sAns = "<center><img src='https://secure.kingscreekplantation.com/secure/images/kcpoalogo.jpg'></center>"
                sAns = sAns & "<table colspan='360'>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>"
                sAns = sAns & "<font size='4'>"
                sAns = sAns & "<b>"
                sAns = sAns & System.DateTime.Now.ToShortDateString
                sAns = sAns & "</b></font></td></tr>"
                sAns = sAns & "<tr colspan='360'>"
                sAns = sAns & "<td>"
                sAns = sAns & "<font size='4'>"
                sAns = sAns & "<b>"
                sAns = sAns & dread("ProspectName")
                sAns = sAns & "</b>"
                sAns = sAns & "</font>"
                sAns = sAns & "</td>"
                'sAns = sAns & "<td colspan='150'>&nbsp;</td>"
                'sAns = sAns & "<td align='right'><b><font size='4'>" & System.DateTime.Now.ToShortDateString & "</font></b></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>"
                sAns = sAns & "<font size='3'><b>"
                sAns = sAns & dread("Address1")
                sAns = sAns & "</font></b>"
                sAns = sAns & "</td>"
                sAns = sAns & "</tr>"
                If dread("Address2") & "" <> "" Then
                    sAns = sAns & "<tr><td><font size='3'><b>" & dread("Address2") & "</b></font></td></tr>"
                End If
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>"
                sAns = sAns & "<font size='3'><b>"
                sAns = sAns & dread("City") & ",&nbsp;" & dread("state") & "&nbsp;" & dread("postalcode")
                sAns = sAns & "</font></b>"
                sAns = sAns & "</td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "</table>"
                sAns = sAns & "<center>"
                sAns = sAns & "<form id='form1'>"
                sAns = sAns & "<center><h3><b><u>Confirmation Number:</u> " & dread("ReservationID") & "</b></h3>"
                sAns = sAns & "<div style='border:thin solid black; width: 750; height:140px'>"
                sAns = sAns & "<div>"
                sAns = sAns & "<table height='136'>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td align='left'><b><u><font size='2'>Room Type:</font></u></b></td>"
                sAns = sAns & "<td><font size='2'><b> " & dread("RoomSize") & "</b></font></td>"
                sAns = sAns & "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>"
                sAns = sAns & "<td align='left'><font size='2'><b><u>Check-In Time:</u></b></font></td>"
                sAns = sAns & "<td><font size='2'><b>After 4:00 p.m.</font></b></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td align='left'><b><u><font size='2'>Check-In Date:</font></u></b></td>"
                sAns = sAns & "<td><font size='2'><b> " & dread("CheckInDate") & "</b></font></td>"
                sAns = sAns & "<td></td>"
                sAns = sAns & "<td align='left'><b><u><font size='2'>Check-Out Time:</font></u></b></td>"
                sAns = sAns & "<td><font size='2'><b>Before 10:00 a.m.</b></font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td align='left'><b><u><font size='2'>Check-Out Date:</font></u></b></td>"
                sAns = sAns & "<td><font size='2'><b> " & dread("CheckOutDate") & "</b></font></td>"
                sAns = sAns & "<td></td>"
                sAns = sAns & "<td align='left'><b><u><font size='2'>Maximum Occupancy:</font></u></b></td>"
                sAns = sAns & "<td><font size='2'><b>" & dread("MaxOccupancy") & "</b></font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "</table>"
                sAns = sAns & "</div>"
                sAns = sAns & "</div>"
                sAns = sAns & "<br>"
                sAns = sAns & "<table>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'><b>-YOU <u>MUST</u> PRESENT THIS CONFIRMATION LETTER AT TIME OF CHECK-IN TO CLAIM YOUR ROOM.</b></font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-We look forward to having you stay with us at King's Creek.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-Check-in at the Aquatic Center prior to 10:00p.m. Between 10:00p.m. and 8:00a.m. you may check-in at the Gate House.</b></font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-For your own protection, we cannot allow check-in for anyone other than the owner without your prior authorization. I.D. will be required upon check-in.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-A major credit card or debit card with a Visa or Mastercard logo must be provided at check-in for incidentals and a $75.00 hold will be placed on your card.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-Call Owner Services for special requests such as: pack and play, high-chairs, ADA accessible units, etc. Special requests are based on availability and are not guaranteed.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-King's Creek does not allow pets.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-KING'S CREEK UNITS ARE SMOKE FREE.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "</table>"
                sAns = sAns & "<center>"
                sAns = sAns & "<table>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'><u>PLEASE NOTE:</u></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-In the event you are unable to use the above dates you must notify Owner Services 30 days in advance.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-There will be a $40 charge for any name changes once the reservation has been booked.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-Should you need to change or cancel the dates of your reservation prior to check in, there will be a $50 charge.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>-No changes to check in date of reservation can be made if reservation is less than 30 days out. If booked dates cannot be used, you will forfeit your week.</font></td>"
                sAns = sAns & "</tr>"

                sAns = sAns & "<tr>"
                sAns = sAns & "<td>&nbsp;</td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>REMINDER; All owner obligations, including mortgage accounts, <b><u>must</u></b> be kept current in order to maintain your usage status.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "<tr>"
                sAns = sAns & "<td><font size='2'>Pursuant to the Public Offering Statement, failure to do so will result in cancellation of your reservation and/or exchange deposits.</font></td>"
                sAns = sAns & "</tr>"
                sAns = sAns & "</table>"
                sAns = sAns & "</center>"
                sAns = sAns & "<center>"
                sAns = sAns & "<p>"
                sAns = sAns & "<font size='2'>If you have any questions regarding this reservation or the facilities available at King's Creek, please do not hesitate to contact your Owner Services Team at 866-228-6796.</font>"
                sAns = sAns & "</p>"
                sAns = sAns & "</center>"
                sAns = sAns & "<p style=text-align:center;font-weight:bold>191 Cottage Cove Lane  Williamsburg, VA 23185</p>"
                sAns = sAns & "</form>"
                sAns = sAns & "</center>"
            Else
                sAns = "EMPTY"
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            sAns = "Error: " & ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return sAns
    End Function

    Public Function Get_Res_Contract(ByVal resID As Integer) As Integer
        Dim conID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "select c.contractid from t_prospect p inner join t_reservations r on r.prospectid = p.prospectid inner join t_roomallocationmatrix x on x.reservationid = r.reservationid inner join t_usage u on u.usageid = x.usageid inner join t_contract c on c.contractid = u.contractid where r.reservationid = " & resID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                conID = dread("ContractID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return conID
    End Function

    Public Function Get_ResID_ByPkg(ByVal pkgID As Integer) As Integer
        Dim resID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ReservationID from t_Reservations where packageissuedid = " & pkgID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                resID = dread("ReservationID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return resID
    End Function
    Public Function Get_ResUnit_Type(ByVal resID As Integer) As String
        Dim unitType As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select top 1 (ut.ComboItem) as UnitType from t_RoomAllocationmatrix rmx inner join t_Room r on rmx.RoomID = r.RoomID " &
                                "inner join t_Unit u on r.UnitID = u.UnitID inner join t_ComboItems ut on u.TypeID = ut.ComboItemID " &
                                "where rmx.ReservationID = '" & resID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                unitType = dread("UnitType")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return unitType
    End Function
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
    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
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
    Public Property ResLocationID() As Integer
        Get
            Return _ResLocationID
        End Get
        Set(ByVal value As Integer)
            _ResLocationID = value
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
    Public Property SourceID() As Integer
        Get
            Return _SourceID
        End Get
        Set(ByVal value As Integer)
            _SourceID = value
        End Set
    End Property
    Public Property CheckInDate() As String
        Get
            Return _CheckInDate
        End Get
        Set(ByVal value As String)
            _CheckInDate = value
        End Set
    End Property
    Public Property CheckOutDate() As String
        Get
            Return _CheckOutDate
        End Get
        Set(ByVal value As String)
            _CheckOutDate = value
        End Set
    End Property
    Public Property ReservationNumber() As String
        Get
            Return _ReservationNumber
        End Get
        Set(ByVal value As String)
            _ReservationNumber = value
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
    Public Property DateBooked() As String
        Get
            Return _DateBooked
        End Get
        Set(ByVal value As String)
            _DateBooked = value
        End Set
    End Property
    Public Property LockInventory() As Boolean
        Get
            Return _LockInventory
        End Get
        Set(ByVal value As Boolean)
            _LockInventory = value
        End Set
    End Property
    Public Property NumberAdults() As Integer
        Get
            Return _NumberAdults
        End Get
        Set(ByVal value As Integer)
            _NumberAdults = value
        End Set
    End Property
    Public Property NumberChildren() As Integer
        Get
            Return _NumberChildren
        End Get
        Set(ByVal value As Integer)
            _NumberChildren = value
        End Set
    End Property
    Public Property ReservationID() As Integer
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
    Public Property TourID As Integer
        Get
            Return _TourID
        End Get
        Set(ByVal value As Integer)
            _TourID = value
        End Set
    End Property

    Public Property ResortCompanyID As Integer
        Get
            Return _ResortCompanyID
        End Get
        Set(ByVal value As Integer)
            _ResortCompanyID = value
        End Set
    End Property
    Public ReadOnly Property Err() As String
        Get
            Return _Err
        End Get
    End Property
End Class

