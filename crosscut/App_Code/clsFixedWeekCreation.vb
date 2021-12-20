Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class clsFixedWeekCreation
    Public Function Create_Fixed_Week(sYear As String, UserID As Integer) As DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim ret As New DataTable
        Try
            ret.Columns.Add("KCP")
            ret.Columns.Add("ReservationID")
            ret.Columns.Add("Booked Date")

            cm.CommandText = "select i.ContractID,i.bd,i.SaleType, i.ContractNumber,r.RoomID, r.RoomNumber, si.Week, u.name, a.cst, case when a.CST = 'Legacy' then 'Owner' else 'Points' end as UsageType,'Owner' as UsageSubtype " & _
                        "	, 2016 as UsageYear, 'Used' as UsageStatus, i.SaleType as UnitType, i.BD as RoomType, 7 as [Days], " & _
                        "	CAST('12/31/15' as datetime) + (7* (si.Week-1)) + (case when rst.ComboItem = 'Thu' then 0 else case when rst.ComboItem = 'Fri' then 1 else case when rst.ComboItem = 'Sat' then 2 else 3 end end end) as StartInDateThurs  " & _
                        "	from ( " & _
                        "		select distinct f.frequency, case when f.frequency = 'Biennial' then YEAR(c.occupancydate) % 2  else 0 end as Avail, c.*, cs.ComboItem as CST " & _
                        "       from t_Contract c " & _
                        "           inner join t_ComboItems ct on ct.ComboItemID = c.WeekTypeID " & _
                        "			inner join t_ComboItems cs on cs.ComboItemID = c.SubTypeID " & _
                        "			inner join t_Frequency f on f.FrequencyID = c.FrequencyID " & _
                        "		where ct.ComboItem = 'Fixed' " & _
                        "			and c.ContractID in (select ContractID from t_SoldInventory) " & _
                        "	) a inner join v_ContractInventory as i on i.ContractID = a.contractid " & _
                        "	inner join t_SoldInventory s on s.ContractID = i.ContractID " & _
                        "	inner join t_SalesInventory si on si.SalesInventoryID = s.SalesInventoryID " & _
                        "	inner join t_Unit u on u.UnitID = si.UnitID " & _
                        "	inner join t_Room r on r.UnitID = u.UnitID " & _
                        "	inner join t_ComboItems rst on rst.ComboItemID = r.SubTypeID " & _
                        "where a.Avail = 0 and a.ContractID not in (select ContractID from t_Usage u where u.UsageYear = 2016) " & _
                        "order by a.contractid"
            da.Fill(ds, "Master")
            If ds.Tables("Master").Rows.Count > 0 Then
                Dim usageID, resID, conID As Integer
                For Each dr As DataRow In ds.Tables("Master").Rows
                    If conID <> dr("ContractID") Then
                        conID = dr("ContractID")
                        Dim oCon As New clsContract
                        oCon.ContractID = conID
                        oCon.Load()

                        'Create Usage
                        Dim oUsage As New clsUsage
                        With oUsage
                            .SoldInventoryID = 0
                            .ContractID = dr("ContractID")
                            .DateCreated = Date.Today
                            .AmountPromised = 0
                            .CategoryID = 0
                            .Days = 7
                            .InDate = dr("StartInDateThurs")
                            .OutDate = CDate(dr("StartInDateThurs")).AddDays(7)
                            .Points = 0
                            .RoomTypeID = (New clsComboItems).Lookup_ID("RoomType", dr("RoomType"))
                            .UnitTypeID = (New clsComboItems).Lookup_ID("UnitType", dr("UnitType"))
                            .StatusID = (New clsComboItems).Lookup_ID("UsageStatus", "Used")
                            .SubTypeID = (New clsComboItems).Lookup_ID("ReservationSubType", dr("UsageSubType"))
                            .TypeID = (New clsComboItems).Lookup_ID("ReservationType", dr("UsageType"))
                            .UsageYear = sYear
                            .UserID = UserID
                            .Save()
                            usageID = .UsageID
                        End With

                        'Create Reservation 
                        Dim oRes As New clsReservations
                        With oRes
                            .CheckInDate = oUsage.InDate
                            .CheckOutDate = oUsage.OutDate
                            .DateBooked = Date.Today
                            .LocationID = 1
                            .LockInventory = True
                            .NumberAdults = 2
                            .NumberChildren = 0
                            .PackageIssuedID = 0
                            .ProspectID = oCon.ProspectID
                            .ReservationNumber = ""
                            .ResLocationID = (New clsComboItems).Lookup_ID("ReservationLocation", "KCP")
                            .ResortCompanyID = (New clsComboItems).Lookup_ID("ResortCompany", "KCP")
                            .SourceID = 0
                            .StatusDate = Date.Today
                            .StatusID = (New clsComboItems).Lookup_ID("ReservationStatus", "Booked")
                            .SubTypeID = (New clsComboItems).Lookup_ID("ReservationSubType", dr("UsageSubType"))
                            .TourID = 0
                            .TypeID = (New clsComboItems).Lookup_ID("ReservationType", dr("UsageType"))
                            .UserID = UserID
                            .Save()
                            resID = .ReservationID
                        End With

                        'Add Note to Reservation

                        Dim oNote As New clsNotes
                        With oNote
                            .KeyField = "ReservationID"
                            .KeyValue = resID
                            .CreatedByID = UserID
                            .DateCreated = Date.Now
                            .Note = "DO NOT MOVE. FIXED WEEK OWNER."
                            .UserID = UserID
                            .Save()
                        End With
                        oNote = Nothing
                        oNote = New clsNotes
                        With oNote
                            .KeyField = "ProspectID"
                            .KeyValue = oCon.ProspectID
                            .CreatedByID = UserID
                            .DateCreated = Date.Now
                            .Note = "KCP " & dr("ContractNumber") & " Booked owners 2016 fixed week for " & dr("StartInDateThurs")
                            .UserID = UserID
                            .Save()
                        End With
                        oNote = Nothing
                        oNote = New clsNotes
                        With oNote
                            .KeyField = "ContractID"
                            .KeyValue = oCon.ContractID
                            .CreatedByID = UserID
                            .DateCreated = Date.Now
                            .Note = "KCP " & dr("ContractNumber") & " Booked owners 2016 fixed week for " & dr("StartInDateThurs")
                            .UserID = UserID
                            .Save()
                        End With

                        oNote = Nothing
                        oRes = Nothing
                        oUsage = Nothing
                        oCon = Nothing
                    End If

                    'Add room to usage 
                    Dim oUs As New clsUsage
                    oUs.Add_Room(usageID, dr("RoomID"), dr("StartInDateThurs"), CDate(dr("StartInDateThurs")).AddDays(6))
                    oUs = Nothing

                    'Add room to reservation
                    Dim oRe As New clsReservations
                    oRe.Add_Room(dr("RoomID"), resID, CDate(dr("StartInDateThurs")), CDate(dr("StartInDateThurs")).AddDays(6))
                    oRe = Nothing

                    'Check Allocation
                    Dim oRTM As New clsRoomAllocationMatrix
                    With oRTM
                        For i = 0 To 6
                            .AllocationID = .Get_Allocation_ID(CDate(dr("StartInDateThurs")).AddDays(i), dr("RoomID"))
                            .Load()
                            .TypeID = (New clsComboItems).Lookup_ID("ReservationType", dr("UsageType"))
                            .UserID = UserID
                            .Save()
                        Next
                    End With
                    oRTM = Nothing
                    Dim oDR As DataRow = ret.NewRow
                    oDR(0) = dr("ContractNumber")
                    oDR(1) = resID
                    oDR(2) = Date.Now.ToString
                    ret.Rows.Add(oDR)
                    oDR = Nothing
                Next

            End If

            
        Catch ex As Exception
            ret.Columns.Add("Error")
            ret.Rows.Add(ex.ToString)
        End Try

        Return ret
    End Function
    



End Class
