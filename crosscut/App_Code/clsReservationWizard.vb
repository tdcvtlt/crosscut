Imports System.Data.SqlClient
Imports System.Data
Imports System.Runtime.CompilerServices
Imports System.Linq
Imports Microsoft.VisualBasic
Imports System.Web.UI
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.IO

Public Class clsReservationWizard

    Public Function Available_Packages(ByVal sDate As Date, ByVal Nights As Integer, ByVal searchOption As Integer, ByVal webSource As String,
                                       ByVal vendorID As Integer) As DataSet
        Dim ds As New DataSet
        Dim oCombo As New clsComboItems
        Dim sourceID As Integer = 0
        Dim accomLocID As Integer = oCombo.Lookup_ID("ReservationLocation", "Williamsburg")
        sourceID = oCombo.Lookup_ID("WebSource", webSource)

        Dim cn As New SqlConnection(Resources.Resource.cns)

        Try

            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand("Exec onlineMarketingSiteTrackingVendors '" & sDate.ToShortDateString & "'," & Nights & "," & accomLocID & ",0,0," & searchOption & "," & sourceID & "," & vendorID, cn)

            cm.CommandTimeout = 0
            Dim da As New SqlDataAdapter(cm)
            Dim i As Integer = 0
            da.Fill(ds, "Packages")
            ds.Tables("Packages").Columns.Add("MinRate")
            ds.Tables("Packages").Columns.Add("MaxRate")
            ds.Tables("Packages").Columns.Add("PackageType")

            If ds.Tables("Packages").Rows.Count > 0 And 1 = 2 Then
                For i = 0 To ds.Tables("Packages").Rows.Count - 1
                    ds.Tables("Packages").Rows(i).Item("MinRate") = Get_Package_Rate(ds.Tables("Packages").Rows(i).Item("PackageID"), sDate, sDate.AddDays(Nights - 1), False)
                    ds.Tables("Packages").Rows(i).Item("MaxRate") = Get_Package_Rate(ds.Tables("Packages").Rows(i).Item("PackageID"), sDate, sDate.AddDays(Nights - 1), True)
                    With New clsPackage
                        .PackageID = ds.Tables("Packages").Rows(i).Item("PackageID")
                        .Load()
                        ds.Tables("Packages").Rows(i).Item("PackageType") = New clsComboItems().Lookup_ComboItem(.TypeID)
                    End With
                Next
            End If
            cm = Nothing
            da = Nothing
        Catch ex As Exception
            cn.Close()
            Throw ex
        Finally
            cn.Close()
        End Try

        Return ds
    End Function

    Public Function Available_Packages(ByVal sDate As Date, ByVal Nights As Integer, ByVal searchOption As Integer, ByVal webSource As String) As DataSet
        Dim ds As New DataSet
        Dim oCombo As New clsComboItems
        Dim sourceID As Integer = 0
        Dim accomLocID As Integer = oCombo.Lookup_ID("ReservationLocation", "Williamsburg")
        sourceID = oCombo.Lookup_ID("WebSource", webSource)
        'Try        
        Dim cn As New SqlConnection(Resources.Resource.cns)
        If cn.State <> ConnectionState.Open Then cn.Open()
        Dim cm As New SqlCommand("Exec onlinePackages '" & sDate.ToShortDateString & "'," & Nights & "," & accomLocID & ",0,0," & searchOption & "," & sourceID & "", cn)

        cm.CommandTimeout = 0
        Dim da As New SqlDataAdapter(cm)
        Dim i As Integer = 0
        da.Fill(ds, "Packages")
        ds.Tables("Packages").Columns.Add("MinRate")
        ds.Tables("Packages").Columns.Add("MaxRate")


        If ds.Tables("Packages").Rows.Count > 0 And 1 = 1 Then
            For i = 0 To ds.Tables("Packages").Rows.Count - 1
                ds.Tables("Packages").Rows(i).Item("MinRate") = Get_Package_Rate(ds.Tables("Packages").Rows(i).Item("PackageID"), sDate, sDate.AddDays(Nights - 1), False)
                ds.Tables("Packages").Rows(i).Item("MaxRate") = Get_Package_Rate(ds.Tables("Packages").Rows(i).Item("PackageID"), sDate, sDate.AddDays(Nights - 1), True)
                'cm.CommandText = "Select p.PackageID, ptp.PackageTourPremiumID as PremiumID, Case when ptp.PremiumID > 0 then convert(nvarchar(3), ptp.QtyAssigned) + ' ' + pr.Description else convert(nvarchar(3), ptp.QtyAssigned) + ' ' + pb.Description end as DescriptionQty, Case when ptp.PremiumID > 0 then pr.Description else pb.Description end as Description, ptp.QtyAssigned from t_Package p inner join t_packagetour pt on p.PackageID = pt.PackageID inner join t_PackageTourPremium ptp on pt.PackageTourID = ptp.PackageTourID left outer join t_premium pr on ptp.PremiumID = pr.PremiumID left outer join t_PremiumBundles pb on ptp.BundleID = pb.PremiumBundleID where p.PackageID = " & ds.Tables("Packages").Rows(i).Item("PackageID") & " and ptp.Optional = 1"
                ''cm.CommandText = "Select p.PackageID, ptp.PackageTourPremiumID as PremiumID, Case when ptp.PremiumID > 0 then pr.Description else pb.Description end as Description from t_Package p inner join t_packagetour pt on p.PackageID = pt.PackageID inner join t_PackageTourPremium ptp on pt.PackageTourID = ptp.PackageTourID left outer join t_premium pr on ptp.PremiumID = pr.PremiumID left outer join t_PremiumBundles pb on ptp.BundleID = pb.PremiumBundleID where p.PackageID = " & ds.Tables("Packages").Rows(i).Item("PackageID") & " and ptp.Optional = 1"
                'da.Fill(ds, "Premiums")
            Next
        End If



        If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        'Catch ex As Exception
        'Throw New Exception(ex.Message)
        'End Try

        Return ds
    End Function
    Private Function Get_Package_Cost(ByVal packageID As Integer, ByVal inDate As DateTime, ByVal outDate As DateTime, ByVal invType As String) As Double
        Dim cost As Double = 0
        Dim sSQL As String = ""
        Dim resortStay As Boolean = True
        Dim oPkg As New clsPackage
        oPkg.PackageID = packageID
        oPkg.Load()
        If oPkg.AccomRoomTypeID > 0 Then
            resortStay = False
        End If
        oPkg = Nothing
        If resortStay Then
            If invType = "Rental" Then
                sSQL = "Select Case when Sum(rtt.RentalAmount) is null then 0 else Sum(rtt.RentalAmount) end as Cost from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & outDate & "' and p.PackageID = " & packageID
            Else
                sSQL = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Cost from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & outDate & "' and p.PackageID = " & packageID
            End If

            'Not sure who added this but it doesnt make sense, it doesnt return the tru cost.
            'Commented out and went with original query 9/6/13 -MB
            'sSQL = "Select (select case when sum(case when rtt.cost is null then 0 else rtt.cost end) is null then 0 else sum(case when rtt.cost is null then 0 else rtt.cost end) end from t_Package p inner join t_Packagereservation pr on pr.packageid = p.packageid inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and dateadd(dd,case when pr.promonights is null then 0 else pr.promonights end,'" & inDate & "') and p.PackageID = " & packageID & ") + case when sum(case when rtt.Amount is null then 0 else rtt.Amount end) is null then 0 else sum(case when rtt.Amount is null then 0 else rtt.Amount end) end  as Cost from t_Package p  inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID inner join t_Packagereservation pr on pr.packageid = p.packageid where rtt.Date between dateadd(dd, case when pr.promonights is null then 0 else pr.promonights end,'" & inDate & "') and '" & outDate & "' and p.PackageID = " & packageID
        Else
            sSQL = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Cost from t_Package p inner join t_Accom2RoomType ar on p.AccomID = ar.AccomID and p.AccomRoomTypeID = ar.RoomTypeID inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & outDate & "' and p.PackageID = " & packageID
        End If
        Try
            Dim cn As New SqlConnection(Resources.Resource.cns)
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand(sSQL, cn)
            Dim dr As SqlDataReader
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                cost = dr("Cost")
            End If
            dr.Close()
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
        Catch ex As Exception

        End Try

        Return cost
    End Function
    Public Function Get_Package_Rate(ByVal packageID As Integer, ByVal inDate As DateTime, ByVal outdate As DateTime, ByVal max As Boolean) As Double
        Dim rate As Double = 0
        Dim sSQL As String = ""
        Dim resortStay As Boolean = True
        Dim nights As Integer = 0
        Dim newInDate As DateTime
        Dim oPkg As New clsPackage
        Dim oCombo As New clsComboItems
        Dim opkgRes As New clsPackageReservation
        Dim pkgType As String = ""
        Dim invType As String = ""
        oPkg.PackageID = packageID
        oPkg.Load()
        If oPkg.AccomRoomTypeID > 0 Then
            resortStay = False
        End If

        pkgType = oCombo.Lookup_ComboItem(oPkg.TypeID)
        opkgRes.PackageReservationID = opkgRes.Find_Res_ID(packageID)
        opkgRes.Load()
        invType = oCombo.Lookup_ComboItem(opkgRes.TypeID)
        oPkg = Nothing
        opkgRes = Nothing
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand("Select pr.PromoRate, pr.PromoNights from t_package p inner join t_packagereservation pr on p.packageID = pr.PackageID where p.packageid = " & packageID, cn)
            Dim dread As SqlDataReader
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                rate = dread("PromoRate")
                nights = dread("PromoNights")
            End If
            dread.Close()

            If pkgType = "Owner Getaway" And nights > 0 Then
                cm.CommandText = "Select Case when Sum(rtt.OwnerAmount) is null then 0 else Sum(rtt.OwnerAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & packageID
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    dread.Read()
                    rate = dread("Rate")
                End If
                dread.Close()
            ElseIf nights > 0 And ((invType = "Rental" And (pkgType = "Tradeshow" Or pkgType = "Tour Package")) Or (pkgType = "Tour Package" And invType = "Marketing")) Then
                cm.CommandText = "Select Case when Sum(rtt.TSAmount) is null then 0 else Sum(rtt.TSAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & packageID
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    dread.Read()
                    rate = dread("Rate")
                End If
                dread.Close()
            End If
            newInDate = inDate.AddDays(nights)
            If DateTime.Compare(newInDate, outdate) <= 0 Then
                If resortStay Then
                    If max Then
                        If pkgType = "Rental" Then
                            cm.CommandText = "Select Case when Sum(rtt.RentalAmount) is null then 0 else Sum(rtt.RentalAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & outdate & "' and p.PackageID = " & packageID
                        Else
                            cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & outdate & "' and p.PackageID = " & packageID
                        End If
                    Else
                        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & outdate & "' and p.PackageID = " & packageID
                    End If
                Else
                    If max Then
                        cm.CommandText = "Select Case when Sum(rtt.RentalAmount) is null then 0 else Sum(rtt.RentalAmount) end as Rate from t_Package p inner join t_Accom2RoomType ar on p.AccomID = ar.AccomID and p.AccomRoomTypeID = ar.RoomTypeID inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & outdate & "' and p.PackageID = " & packageID
                    Else
                        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2RoomType ar on p.AccomID = ar.AccomID and p.AccomRoomTypeID = ar.RoomTypeID inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & outdate & "' and p.PackageID = " & packageID
                    End If
                End If
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    dread.Read()
                    rate = rate + dread("Rate")
                End If
                dread.Close()
            End If
            If pkgType = "Tour Promotion" Then
                Dim bHoliday As Boolean = False
                Dim oRateHoliday As New clsRateTableHolidays
                'Holiday Fee Check
                newInDate = inDate
                Do While DateTime.Compare(newInDate, outdate) <= 0
                    If oRateHoliday.Check_Date(newInDate) > 0 Then
                        oRateHoliday.ID = oRateHoliday.Check_Date(newInDate)
                        oRateHoliday.Load()
                        rate = rate + oRateHoliday.HolidayRate
                        Exit Do
                    Else
                        newInDate = newInDate.AddDays(1)
                    End If
                Loop
                oRateHoliday = Nothing
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return rate
    End Function



    Public Function Get_Invoice_Amt(ByVal pkID As Integer, ByVal charge As Double, ByVal inDate As DateTime, ByVal nights As Integer) As Decimal
        Dim invAmt As Decimal = 0
        Dim oPkg As New clsPackage
        Dim oPkgRes As New clsPackageReservation
        Dim oCombo As New clsComboItems
        Dim invType As String = ""
        Dim pkgType As String = ""
        Dim numNights As Integer = 0
        Dim newIndate As Date

        oPkg.PackageID = pkID
        oPkg.Load()
        pkgType = oCombo.Lookup_ComboItem(oPkg.TypeID)

        'If pkgType = "Tour Promotion" Then

        '    oPkgRes.PackageReservationID = oPkgRes.Find_Res_ID(pkID)
        '    oPkgRes.Load()
        '    invType = oCombo.Lookup_ComboItem(oPkgRes.TypeID)

        '    Dim cn As New SqlConnection(Resources.Resource.cns)
        '    If cn.State <> ConnectionState.Open Then cn.Open()
        '    Dim cm As SqlCommand, dread As SqlDataReader

        '    If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then

        '        cm = New SqlCommand("Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkID, cn))

        '        dread = cm.ExecuteReader
        '        If dread.HasRows Then
        '            dread.Read()
        '            invAmt = invAmt + dread("Rate")
        '        End If
        '        dread.Close()
        '        If cn.State <> ConnectionState.Closed Then cn.Close()
        '    End If

        '    newIndate = inDate.AddDays(oPkgRes.PromoNights)


        '    Dim cm As New SqlCommand("Select Case when Sum(rtt.TSAmount) is null then 0 else Sum(rtt.TSAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0), cn)
        '    Dim dread As SqlDataReader
        '    dread = cm.ExecuteReader
        '    If dread.HasRows Then
        '        dread.Read()
        '        invAmt = dread("Rate")
        '    End If
        '    dread.Close()

        '    newIndate = inDate.AddDays(oPkgRes.PromoNights)
        '    If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
        '        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkID
        '        dread = cm.ExecuteReader
        '        If dread.HasRows Then
        '            dread.Read()
        '            invAmt = invAmt + dread("Rate")
        '        End If
        '        dread.Close()
        '    End If
        '    If cn.State <> ConnectionState.Closed Then cn.Close()

        'End If

        Return 0
    End Function

    Public Function Get_Invoice_Amt(ByVal pkgs() As Integer, ByVal charge As Double, ByVal inDate As DateTime, ByVal nights As Integer) As Decimal
        Dim invAmt As Decimal = 0
        Dim oPkg As New clsPackage
        Dim oPkgRes As New clsPackageReservation
        Dim oCombo As New clsComboItems
        Dim invType As String = ""
        Dim pkgType As String = ""
        Dim numNights As Integer = 0
        Dim newIndate As Date

        If pkgs.Length > 1 Then
            Dim cn As New SqlConnection(Resources.Resource.cns)
            If cn.State <> ConnectionState.Open Then cn.Open()
            For i = 0 To UBound(pkgs)
                oPkg.PackageID = pkgs(i)
                oPkg.Load()
                pkgType = oCombo.Lookup_ComboItem(oPkg.TypeID)
                oPkgRes.PackageReservationID = oPkgRes.Find_Res_ID(pkgs(i))
                oPkgRes.Load()
                invType = oCombo.Lookup_ComboItem(oPkgRes.TypeID)
                If pkgType = "Owner Getaway" Then
                    Dim cm As New SqlCommand("Select Case when Sum(rtt.OwnerAmount) is null then 0 else Sum(rtt.OwnerAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(i), cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt += dread("Rate")
                    End If
                    dread.Close()
                    newIndate = inDate.AddDays(oPkgRes.PromoNights)
                    If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(i)
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            invAmt += dread("Rate")
                        End If
                        dread.Close()
                    End If
                    cm = Nothing
                    dread = Nothing
                ElseIf pkgType = "Owner Advantage" Then
                    Dim cm As New SqlCommand("Select Case when Sum(rtt.OwnerAdvAmount) is null then 0 else Sum(rtt.OwnerAdvAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(i), cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt += dread("Rate")
                    End If
                    dread.Close()
                    'newIndate = inDate.AddDays(oPkgRes.PromoNights)
                    'If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                    '    cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(i)
                    '    dread = cm.ExecuteReader
                    '    If dread.HasRows Then
                    '        dread.Read()
                    '        invAmt += dread("Rate")
                    '    End If
                    '    dread.Close()
                    'End If
                    cm = Nothing
                    dread = Nothing
                ElseIf pkgType = "Rental" Then
                    Dim cm As New SqlCommand("Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(i), cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt += dread("Rate")
                    End If
                    dread.Close()
                    cm = Nothing
                    dread = Nothing
                ElseIf pkgType = "Tour Promotion" Then
                    If invType = "Marketing" Then

                        invAmt = charge
                        Exit For

                    ElseIf invType = "Rental" Then
                        Dim cm As New SqlCommand("Select Case when Sum(rtt.TSAmount) is null then 0 else Sum(rtt.TSAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(i), cn)
                        Dim dread As SqlDataReader
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            invAmt += dread("Rate")
                        End If
                        dread.Close()

                        newIndate = inDate.AddDays(oPkgRes.PromoNights)
                        If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                            cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(i)
                            dread = cm.ExecuteReader
                            If dread.HasRows Then
                                dread.Read()
                                invAmt += dread("Rate")
                            End If
                            dread.Close()
                        End If
                        cm = Nothing
                        dread = Nothing
                    Else
                    End If
                Else
                End If
            Next i
            If charge > invAmt Then
                invAmt = charge
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Else
            oPkg.PackageID = pkgs(0)
            oPkg.Load()
            pkgType = oCombo.Lookup_ComboItem(oPkg.TypeID)
            If pkgType = "Owner Getaway" Or pkgType = "Rental" Then
                invAmt = charge
            ElseIf pkgType = "Tour Promotion" Then
                oPkgRes.PackageReservationID = oPkgRes.Find_Res_ID(pkgs(0))
                oPkgRes.Load()
                invType = oCombo.Lookup_ComboItem(oPkgRes.TypeID)
                If invType = "Marketing" Then
                    If oPkg.DefaultInvoiceAmt > 0 And oPkgRes.PromoNights = nights And oPkg.DefaultInvoiceAmt > charge Then
                        invAmt = oPkg.DefaultInvoiceAmt
                    Else
                        invAmt = charge ' + nights
                    End If
                ElseIf invType = "Rental" Then
                    Dim cn As New SqlConnection(Resources.Resource.cns)
                    If cn.State <> ConnectionState.Open Then cn.Open()
                    Dim cm As New SqlCommand("Select Case when Sum(rtt.TSAmount) is null then 0 else Sum(rtt.TSAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0), cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt = dread("Rate")
                    End If
                    dread.Close()

                    newIndate = inDate.AddDays(oPkgRes.PromoNights)
                    If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(0)
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            invAmt = invAmt + dread("Rate")
                        End If
                        dread.Close()
                    End If
                    If cn.State <> ConnectionState.Closed Then cn.Close()
                Else
                    invAmt = charge
                End If
            ElseIf pkgType = "Tradeshow" Then
                oPkgRes.PackageReservationID = oPkgRes.Find_Res_ID(pkgs(0))
                oPkgRes.Load()
                invType = oCombo.Lookup_ComboItem(oPkgRes.TypeID)
                If invType = "Marketing" Then
                    If oPkgRes.PromoNights >= nights Then
                        invAmt = oPkg.DefaultInvoiceAmt
                    Else
                        invAmt = oPkg.DefaultInvoiceAmt
                        newIndate = inDate.AddDays(oPkgRes.PromoNights)
                        If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                            Dim cn As New SqlConnection(Resources.Resource.cns)
                            If cn.State <> ConnectionState.Open Then cn.Open()
                            Dim cm As New SqlCommand("Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(0), cn)
                            Dim dread As SqlDataReader
                            dread = cm.ExecuteReader
                            If dread.HasRows Then
                                dread.Read()
                                invAmt = invAmt + dread("Rate")
                            End If
                            dread.Close()
                            If cn.State <> ConnectionState.Closed Then cn.Close()
                        End If
                    End If
                ElseIf invType = "Rental" Then
                    Dim cn As New SqlConnection(Resources.Resource.cns)
                    If cn.State <> ConnectionState.Open Then cn.Open()
                    Dim cm As New SqlCommand("Select Case when Sum(rtt.TSAmount) is null then 0 else Sum(rtt.TSAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0), cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt = dread("Rate")
                    End If
                    dread.Close()

                    newIndate = inDate.AddDays(oPkgRes.PromoNights)
                    If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(0)
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            invAmt = invAmt + dread("Rate")
                        End If
                        dread.Close()
                    End If
                    If cn.State <> ConnectionState.Closed Then cn.Close()
                End If
            ElseIf pkgType = "Tour Package" Then
                oPkgRes.PackageReservationID = oPkgRes.Find_Res_ID(pkgs(0))
                oPkgRes.Load()
                If nights <= oPkgRes.PromoNights Then
                    Dim cn As New SqlConnection(Resources.Resource.cns)
                    If cn.State <> ConnectionState.Open Then cn.Open()
                    Dim cm As New SqlCommand("Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0), cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt = dread("Rate")
                    End If
                    dread.Close()
                    If cn.State <> ConnectionState.Closed Then cn.Close()
                Else
                    Dim oResExNight As New clsPackageReservation2CheckInDay
                    Dim cn As New SqlConnection(Resources.Resource.cns)
                    Dim cmText As String = ""
                    If cn.State <> ConnectionState.Open Then cn.Open()
                    If oPkgRes.AllowExtraNight And oResExNight.Validate_CheckInDay(oPkgRes.PackageReservationID, CInt(inDate.DayOfWeek)) Then
                        cmText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights) & "' and p.PackageID = " & pkgs(0)
                        newIndate = inDate.AddDays(oPkgRes.PromoNights + 1)
                    Else
                        cmText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                        newIndate = inDate.AddDays(oPkgRes.PromoNights)
                    End If

                    Dim cm As New SqlCommand(cmText, cn)
                    Dim dread As SqlDataReader
                    dread = cm.ExecuteReader
                    If dread.HasRows Then
                        dread.Read()
                        invAmt = dread("Rate")
                    End If
                    dread.Close()

                    If DateTime.Compare(newIndate, inDate.AddDays(nights)) < 0 Then
                        cm.CommandText = "Select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newIndate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(0)
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            invAmt = invAmt + dread("Rate")
                        End If
                        dread.Close()
                    End If
                    If cn.State <> ConnectionState.Closed Then cn.Close()
                End If

                'added 1/29/2021
            ElseIf pkgType = "Owner Advantage" And nights > 0 Then
                Dim cn As New SqlConnection(Resources.Resource.cns)
                If cn.State <> ConnectionState.Open Then cn.Open()
                Dim cm As New SqlCommand("Select Case when Sum(rtt.OwnerAdvAmount) is null then 0 else Sum(rtt.OwnerAdvAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(nights - 1) & "' and p.PackageID = " & pkgs(0), cn)
                Dim dread As SqlDataReader
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    dread.Read()
                    invAmt = dread("Rate")
                End If
                dread.Close()
            Else
                invAmt = charge
            End If
        End If
        Return invAmt
    End Function
    Public Function Get_Discounts(ByVal pkgs() As Integer, ByVal max As Double, ByVal charge As Double, Optional ByVal pkgResFinTransID As Integer = 0, Optional ByVal nights As Integer = 0, Optional ByVal inDate As DateTime = Nothing) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("MethodID")
        dt.Columns.Add("Amount")
        Dim adjAmt As Double = max - charge
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand()
            cm.Connection = cn
            Dim dread As SqlDataReader
            Dim i As Integer = 0
            Dim packages As String = ""
            For i = 0 To UBound(pkgs)
                If packages = "" Then
                    packages = "'" & pkgs(i) & "'"
                Else
                    packages &= ",'" & pkgs(i) & "'"
                End If
            Next

            If max > 0 Or charge > 0 Then
                'Dim cm As New SqlCommand("Select Distinct(pp.PaymentMethodID) as MethodID from t_package p inner join t_packagereservation pr on p.packageID = pr.PackageID inner join t_PackageReservationFinTransCode pf on pr.PackageReservationID = pf.PackageReservationID inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where p.packageid in (" & packages & ")", cn)
                cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_package p inner join t_packagereservation pr on p.packageID = pr.PackageID inner join t_PackageReservationFinTransCode pf on pr.PackageReservationID = pf.PackageReservationID inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where p.packageid in (" & packages & ")"
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    Do While dread.Read
                        Dim dr As DataRow = dt.NewRow
                        dr("MethodID") = dread("MethodID")
                        dr("Amount") = 0
                        dt.Rows.Add(dr)
                    Loop
                End If
                dread.Close()
                For i = 0 To dt.Rows.Count - 1
                    dt.Rows(i).Item("Amount") = adjAmt / dt.Rows.Count
                Next
            Else
                Dim oPkg As New clsPackage
                Dim oPkgRes As New clsPackageReservation
                Dim oPkgResNight As New clsPackageReservation2CheckInDay
                Dim oCombo As New clsComboItems
                Dim pkgType As String = ""
                Dim resType As String = ""
                Dim cbAMt As Double = 0
                oPkg.PackageID = pkgs(0)
                oPkg.Load()
                oPkgRes.PackageReservationID = oPkgRes.Find_Res_ID(pkgs(0))
                oPkgRes.Load()
                pkgType = oCombo.Lookup_ComboItem(oPkg.TypeID)
                resType = oCombo.Lookup_ComboItem(oPkgRes.TypeID)
                If pkgType = "Tradeshow" Then
                    If resType = "Marketing" Then
                        cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            Dim dr As DataRow = dt.NewRow
                            dr("MethodID") = dread("MethodID")
                            dr("Amount") = oPkg.DefaultInvoiceAmt
                            dt.Rows.Add(dr)
                        End If
                        dread.Close()
                    Else
                        cm.CommandText = "Select Case when Sum(rtt.TSAmount) is null then 0 else Sum(rtt.TSAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                        dread = cm.ExecuteReader
                        dread.Read()
                        cbAMt = dread("Rate")
                        dread.Close()
                        cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            Dim dr As DataRow = dt.NewRow
                            dr("MethodID") = dread("MethodID")
                            dr("Amount") = cbAMt
                            dt.Rows.Add(dr)
                        End If
                        dread.Close()
                    End If
                ElseIf pkgType = "Tour Package" Then
                    If nights <= oPkgRes.PromoNights Then
                        If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                            cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and rt.Name = '1 Bedroom Cottage'"
                        Else
                            cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                        End If
                        dread = cm.ExecuteReader
                        dread.Read()
                        cbAMt = dread("Rate")
                        dread.Close()
                        cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                        dread = cm.ExecuteReader
                        If dread.HasRows Then
                            dread.Read()
                            Dim dr As DataRow = dt.NewRow
                            dr("MethodID") = dread("MethodID")
                            dr("Amount") = cbAMt
                            dt.Rows.Add(dr)
                        End If
                        dread.Close()
                        If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                            cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                            dread = cm.ExecuteReader
                            If dread.HasRows Then
                                dread.Read()
                                Dim dr As DataRow = dt.NewRow
                                dr("MethodID") = oCombo.Lookup_ID("PaymentMethod", "Discount")
                                dr("Amount") = dread("Rate") - cbAMt
                                dt.Rows.Add(dr)
                            End If
                            dread.Close()
                        End If
                    Else
                        If oPkgRes.AllowExtraNight And oPkgResNight.Validate_CheckInDay(oPkgRes.PackageReservationID, CInt(inDate.DayOfWeek)) Then
                            If oPkgRes.PromoNights = 2 Then
                                If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and rt.Name = '1 Bedroom Cottage'"
                                Else
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights) & "' and p.PackageID = " & pkgs(0)
                                End If
                                dread = cm.ExecuteReader
                                dread.Read()
                                cbAMt = dread("Rate")
                                dread.Close()
                                cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                                dread = cm.ExecuteReader
                                If dread.HasRows Then
                                    dread.Read()
                                    Dim dr As DataRow = dt.NewRow
                                    dr("MethodID") = dread("MethodID")
                                    dr("Amount") = cbAMt
                                    dt.Rows.Add(dr)
                                End If
                                dread.Close()
                                If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                                    dread = cm.ExecuteReader
                                    If dread.HasRows Then
                                        dread.Read()
                                        Dim dr As DataRow = dt.NewRow
                                        dr("MethodID") = oCombo.Lookup_ID("PaymentMethod", "Discount")
                                        dr("Amount") = dread("Rate") - cbAMt
                                        dt.Rows.Add(dr)
                                    End If
                                    dread.Close()
                                End If
                            Else
                                Dim dr As DataRow
                                If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and rt.Name = '1 Bedroom Cottage'"
                                Else
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                                End If
                                dread = cm.ExecuteReader
                                dread.Read()
                                cbAMt = dread("Rate")
                                dread.Close()
                                cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                                dread = cm.ExecuteReader
                                If dread.HasRows Then
                                    dread.Read()
                                    dr = dt.NewRow
                                    dr("MethodID") = dread("MethodID")
                                    dr("Amount") = cbAMt
                                    dt.Rows.Add(dr)
                                End If
                                dread.Close()
                                Dim newInDate As DateTime
                                newInDate = inDate.AddDays(oPkgRes.PromoNights)
                                If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & newInDate & "' and p.PackageID = " & pkgs(0)
                                Else
                                    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & newInDate & "' and p.PackageID = " & pkgs(0)
                                End If
                                dread = cm.ExecuteReader
                                dread.Read()
                                dr = dt.NewRow
                                dr("MethodID") = oCombo.Lookup_ID("PaymentMethod", "Discount")
                                If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                    dr("Amount") = dread("Rate") - cbAMt
                                Else
                                    dr("Amount") = dread("Rate")
                                End If
                                dt.Rows.Add(dr)
                                dread.Close()
                            End If
                        Else
                            If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and rt.Name = '1 Bedroom Cottage'"
                            Else
                                cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                            End If
                            dread = cm.ExecuteReader
                            dread.Read()
                            cbAMt = dread("Rate")
                            dread.Close()
                            cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                            dread = cm.ExecuteReader
                            If dread.HasRows Then
                                dread.Read()
                                Dim dr As DataRow = dt.NewRow
                                dr("MethodID") = dread("MethodID")
                                dr("Amount") = cbAMt
                                dt.Rows.Add(dr)
                            End If
                            dread.Close()
                            If oPkg.Package = "Cali-W 2Bdrm" Or oPkg.Package = "Czar-W 2Bdrm" Or oPkg.Package = "Daytona-W 2Bdrm" Or oPkg.Package = "Flo-W 2Bdrm" Or oPkg.Package = "Orl-W 2Bdrm" Or oPkg.Package = "Cali-Golf 2Bdrm" Or oPkg.Package = "Czar-Golf 2Bdrm" Or oPkg.Package = "Daytona-Golf 2Bdrm" Or oPkg.Package = "Flo-Golf 2Bdrm" Or oPkg.Package = "Orl-Golf 2Bdrm" Or oPkg.Package = "Czar500 2Bdrm" Or oPkg.Package = "Czar2 2Bdrm" Or oPkg.Package = "Czar-Home 2Bdrm" Then
                                cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                                dread = cm.ExecuteReader
                                If dread.HasRows Then
                                    dread.Read()
                                    Dim dr As DataRow = dt.NewRow
                                    dr("MethodID") = oCombo.Lookup_ID("PaymentMethod", "Discount")
                                    dr("Amount") = dread("Rate") - cbAMt
                                    dt.Rows.Add(dr)
                                End If
                                dread.Close()
                            End If
                        End If
                    End If
                    'Updated 6/10 to account for 5 person package
                    'If nights <= oPkgRes.PromoNights Then
                    '    cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                    '    dread = cm.ExecuteReader
                    '    dread.Read()
                    '    cbAMt = dread("Rate")
                    '    dread.Close()
                    '    cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                    '    dread = cm.ExecuteReader
                    '    If dread.HasRows Then
                    '        dread.Read()
                    '        Dim dr As DataRow = dt.NewRow
                    '        dr("MethodID") = dread("MethodID")
                    '        dr("Amount") = cbAMt
                    '        dt.Rows.Add(dr)
                    '    End If
                    '    dread.Close()
                    'Else
                    '    If oPkgRes.AllowExtraNight And oPkgResNight.Validate_CheckInDay(oPkgRes.PackageReservationID, inDate.DayOfWeek) Then
                    '        If oPkgRes.PromoNights = 2 Then
                    '            cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights) & "' and p.PackageID = " & pkgs(0)
                    '            dread = cm.ExecuteReader
                    '            dread.Read()
                    '            cbAMt = dread("Rate")
                    '            dread.Close()
                    '            cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                    '            dread = cm.ExecuteReader
                    '            If dread.HasRows Then
                    '                dread.Read()
                    '                Dim dr As DataRow = dt.NewRow
                    '                dr("MethodID") = dread("MethodID")
                    '                dr("Amount") = cbAMt
                    '                dt.Rows.Add(dr)
                    '            End If
                    '            dread.Close()
                    '        Else
                    '            Dim dr As DataRow
                    '            cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                    '            dread = cm.ExecuteReader
                    '            dread.Read()
                    '            cbAMt = dread("Rate")
                    '            dread.Close()
                    '            cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                    '            dread = cm.ExecuteReader
                    '            If dread.HasRows Then
                    '                dread.Read()
                    '                dr = dt.NewRow
                    '                dr("MethodID") = dread("MethodID")
                    '                dr("Amount") = cbAMt
                    '                dt.Rows.Add(dr)
                    '            End If
                    '            dread.Close()
                    '            Dim newInDate As DateTime
                    '            newInDate = inDate.AddDays(oPkgRes.PromoNights)
                    '            cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & newInDate & "' and '" & newInDate & "' and p.PackageID = " & pkgs(0)
                    '            dread = cm.ExecuteReader
                    '            dread.Read()
                    '            dr = dt.NewRow
                    '            dr("MethodID") = oCombo.Lookup_ID("PaymentMethod", "Discount")
                    '            dr("Amount") = dread("Rate")
                    '            dt.Rows.Add(dr)
                    '            dread.Close()
                    '        End If
                    '    Else
                    '        cm.CommandText = "Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on ar.RateTableID = rt.RateTableID inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '" & inDate & "' and '" & inDate.AddDays(oPkgRes.PromoNights - 1) & "' and p.PackageID = " & pkgs(0)
                    '        dread = cm.ExecuteReader
                    '        dread.Read()
                    '        cbAMt = dread("Rate")
                    '        dread.Close()
                    '        cm.CommandText = "Select Distinct(pp.PaymentMethodID) as MethodID from t_PackageReservationFinTransCode pf inner join t_packageReservationPayment pp on pf.PackageReservationFinTransCodeID = pp.PackageReservationFinTransID where pp.PackageReservationFinTransID = " & pkgResFinTransID
                    '        dread = cm.ExecuteReader
                    '        If dread.HasRows Then
                    '            dread.Read()
                    '            Dim dr As DataRow = dt.NewRow
                    '            dr("MethodID") = dread("MethodID")
                    '            dr("Amount") = cbAMt
                    '            dt.Rows.Add(dr)
                    '        End If
                    '        dread.Close()
                    '    End If
                    'End If
                End If
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try



        Return dt
    End Function


#Region "Wizard"

    Public Enum EnumScenario
        One
        Two
        Three
        Four
        Ten
        Eleven
        Five
    End Enum

    Public Class Reservation
        Inherits clsReservations

        Public Sub New()
        End Sub

        Public Tour As New Tour
        Public Accommodation As New clsAccommodation

        Public Overloads Sub Load(reservation_id As Int32, scenario As EnumScenario, Optional accom_name As String = "KCP")
            ReservationID = reservation_id

            Accommodation.AccommodationID = -2

            If scenario = EnumScenario.Three Then
                If accom_name <> "KCP" Then
                    Using cn = New SqlConnection(Resources.Resource.cns)
                        Using cm = New SqlCommand()
                            Try
                                cn.Open()
                                cm.Connection = cn
                                cm.CommandText = String.Format("select top 1 AccommodationID from t_Accommodation where ReservationID = {0} ", ReservationID)
                                Dim accommodation_id = Int32.Parse(cm.ExecuteScalar())

                                Accommodation.AccommodationID = accommodation_id
                                Accommodation.Load()

                            Catch ex As Exception
                                cn.Close()
                                Throw ex
                            Finally
                                cn.Close()
                            End Try
                        End Using
                    End Using
                End If
            End If
            MyBase.Load()
        End Sub

        Public Overloads Sub Save(scenario As EnumScenario, user_id As Int32, packages As List(Of Wizard_Package), packages_price As Decimal, Optional accom_name As String = "KCP")

            DateBooked = DateTime.Now
            StatusID = New clsComboItems().Lookup_ID("ReservationStatus", "Booked")
            StatusDate = DateTime.Now
            UserID = user_id
            Dim package_id = 0, package_issued_id = 0

            With New clsAccommodation

                Dim has_acc = False

                'AccomRoomTypeID > 0 means its accomID is other than KCP
                'this reservation has a hotel accommodation
                If Accommodation.RoomTypeID > 0 Then
                    Accommodation.AccommodationID = 0
                    has_acc = True
                ElseIf .Get_Accom_By_Res(ReservationID) > 0 Then
                    Accommodation.AccommodationID = .Get_Accom_By_Res(ReservationID)
                    has_acc = True
                End If

                If has_acc Then
                    .Load()
                    If Me.Accommodation.AccommodationID < 1 Then
                        .TourID = Me.TourID
                    End If
                    .UserID = user_id
                    .ArrivalDate = Me.CheckInDate
                    .DepartureDate = Me.CheckOutDate
                    .NumberAdults = Me.NumberAdults
                    .NumberChildren = Me.NumberChildren
                    .ConfirmationNumber = Me.ReservationNumber
                    .RoomCost = packages_price
                    .PromoNights = DateTime.Parse(.DepartureDate).Subtract(DateTime.Parse(.ArrivalDate)).Days
                    .AccomID = Me.Accommodation.AccomID
                    .RoomTypeID = Me.Accommodation.RoomTypeID
                    .GuestTypeID = Me.Accommodation.GuestTypeID
                    .ReservationID = Me.ReservationID
                    .CheckInLocationID = Me.Accommodation.CheckInLocationID

                    .Save()
                End If
            End With

            MyBase.Save()

            If scenario = EnumScenario.Two Or scenario = EnumScenario.Three Or scenario = EnumScenario.Eleven Then

                package_id = packages.First().Package.PackageID

                With New clsPackageIssued
                    .PackageIssuedID = PackageIssuedID
                    .Load()
                    .PackageID = package_id
                    .UserID = user_id
                    .StatusDate = DateTime.Now
                    .StatusID = New clsComboItems().Lookup_ID("PackageStatus", "OpenEnded")
                    .VendorID = New Base_Package().Get_Package_VendorID(package_id)
                    .Save()
                End With

                MyBase.Save()

            End If

            If scenario = EnumScenario.One Or scenario = EnumScenario.Ten Then
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using cm = New SqlCommand()
                        Try
                            package_id = packages.First().PackageID
                            cn.Open()
                            cm.Connection = cn
                            cm.CommandText = String.Format("Insert Into t_PackageIssued (LocationID, PackageID, ProspectID, Cost, StatusID, PurchaseDate, StatusDate, VendorID) values(0, {0}, {1}, {2}, {3}, '{4}', '{5}', {6});select SCOPE_IDENTITY() ",
                                                               package_id,
                                                               ProspectID,
                                                               packages_price,
                                                               New clsComboItems().Lookup_ID("PackageStatus", "OpenEnded"),
                                                               DateTime.Now,
                                                               DateTime.Now,
                                                               New Base_Package().Get_Package_VendorID(package_id))
                            package_issued_id = cm.ExecuteScalar()
                            MyBase.PackageIssuedID = package_issued_id
                            MyBase.Save()

                            cm.CommandText = String.Format("Insert Into t_Event (KeyField, KeyValue, Type, DateCreated, CreatedByID, FieldName, NewValue) values ('PackageIssuedID', {0}, 'Create', getdate(), {1}, 'PackageIssuedID', {0});", package_issued_id, user_id)
                            cm.CommandText += String.Format("Insert Into t_Event (KeyField, KeyValue, Type, DateCreated, CreatedByID, FieldName, NewValue) values ('ReservationID', {0}, 'Create', getdate(), {1}, 'ReservationID', {0});", MyBase.ReservationID, user_id)
                            cm.ExecuteNonQuery()

                        Catch ex As Exception
                            cn.Close()
                            Throw ex
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
            End If
        End Sub

    End Class

    Public Class Prospect
        Inherits clsProspect

        Public Sub New()
        End Sub

        Public Overloads Sub Load(prospect_id As Int32)
            Emails.Clear()
            Phones.Clear()
            Addresses.Clear()
            Me.Prospect_ID = prospect_id
            MyBase.Load()

            Dim e = New clsEmail()
            e.ProspectID = prospect_id
            For Each dr As DataRow In e.Get_Table().Rows
                Dim x As New clsEmail()
                x.EmailID = dr("ID").ToString()
                x.Load()
                Me.Emails.Add(x)
            Next

            Dim p = New clsPhone
            p.ProspectID = prospect_id
            For Each dr As DataRow In p.Get_Table().Rows
                Dim x As New clsPhone()
                x.PhoneID = dr("ID").ToString()
                x.Load()
                Me.Phones.Add(x)
            Next

            Dim a = New clsAddress
            a.ProspectID = prospect_id
            For Each dr As DataRow In a.Get_Table().Rows
                Dim x As New clsAddress()
                x.AddressID = dr("ID").ToString()
                x.Load()
                Me.Addresses.Add(x)
            Next
        End Sub

        Public Overloads Sub Save(user_id As Int32)
            MyBase.Save()
            For Each e As clsEmail In Me.Emails
                With New clsEmail
                    .EmailID = e.EmailID
                    .Load()
                    .ProspectID = MyBase.Prospect_ID
                    .Email = e.Email
                    .IsActive = e.IsActive
                    .IsPrimary = e.IsPrimary
                    .UserID = e.UserID
                    .Save()
                End With
            Next
            For Each p As clsPhone In Me.Phones
                With New clsPhone
                    .PhoneID = p.PhoneID
                    .Load()
                    .ProspectID = MyBase.Prospect_ID
                    .UserID = user_id
                    .Number = p.Number
                    .TypeID = p.TypeID
                    .Active = p.Active
                    .Extension = p.Extension
                    .Save()
                End With
            Next
            For Each a As clsAddress In Me.Addresses
                With New clsAddress
                    .AddressID = a.AddressID
                    .Load()
                    .Address1 = a.Address1
                    .Address2 = a.Address2
                    .ActiveFlag = a.ActiveFlag
                    .City = a.City
                    .CountryID = a.CountryID
                    .ContractAddress = a.ContractAddress
                    .ProspectID = MyBase.Prospect_ID
                    .PostalCode = a.PostalCode
                    .Region = a.Region
                    .TypeID = a.TypeID
                    .StateID = a.StateID
                    .UserID = user_id
                    .Save()
                End With
            Next
        End Sub

        Public Emails As New List(Of clsEmail)
        Public Phones As New List(Of clsPhone)
        Public Addresses As New List(Of clsAddress)

    End Class

    Public Class Tour
        Inherits clsTour

        Public Sub New()
        End Sub

        Public Overloads Sub Load(scenario As EnumScenario)
            MyBase.Load()
        End Sub

        Public Premiums_Max_Cost As Decimal
        Public Premiums As New List(Of PremiumIssued)

        Public Overloads Sub Save(scenario As EnumScenario, user_id As Int32)
            UserID = user_id
            MyBase.Save()

            If scenario = EnumScenario.Ten Or scenario = EnumScenario.One Then
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using cm = New SqlCommand()
                        Try
                            cn.Open()
                            cm.Connection = cn
                            cm.CommandText = String.Format("Insert Into t_Event (KeyField, KeyValue, Type, DateCreated, CreatedByID, FieldName, NewValue) values ('TourID', {0}, 'Create', getdate(), {1}, 'TourID', {0});", MyBase.TourID, user_id)
                            cm.ExecuteNonQuery()

                        Catch ex As Exception
                            cn.Close()
                            Throw ex
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
            End If

            For Each pe In Me.Premiums
                If pe.PremiumIssuedID < 1 Then
                    pe.PremiumIssuedID = 0
                    pe.Load()

                    pe.DateCreated = DateTime.Now
                    pe.CreatedByID = user_id
                    pe.UserID = user_id
                    pe.KeyField = "TourID"
                    pe.KeyValue = MyBase.TourID
                End If

                If scenario = EnumScenario.One Then
                    If pe.QtyAssigned > 0 Then
                        pe.Save()
                    End If
                Else
                    pe.Save()
                End If
            Next
        End Sub
    End Class

    Public Class PremiumIssued
        Inherits clsPremiumIssued

        Public Sub New()
        End Sub

        Public Function [Optional](packageTourID As Int32, packageID As Int32, premiumID As Int32) As Boolean
            Dim re = False

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("Select top 1 Optional from t_PackageTourPremium where PackageTourID = {0} And PackageID = {1} And PremiumID = {2}", packageTourID, packageID, premiumID), cn)
                    Try
                        cn.Open()

                        Dim v = cm.ExecuteScalar()
                        If Not v Is Nothing Then
                            re = CType(v, Boolean)
                        End If

                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return re
        End Function

        Public ReadOnly Property PremiumIssuedStatus As String
            Get
                Return New clsComboItems().Lookup_ComboItem(StatusID)
            End Get
        End Property

        Public ReadOnly Property PremiumName As String
            Get
                Dim n = String.Empty
                With New clsPremium()
                    .PremiumID = PremiumID
                    .Load()
                    n = .PremiumName
                End With
                Return n
            End Get
        End Property
    End Class


    Public Class Helper
        Public Shared Int32_Randomized As Int32 = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9))
        Public Shared LOGIN_VENDORID As Int32 = -1
        Public Shared WEB As String
        Public Shared Host As String
    End Class

    Public Class Wizard

        Public Sub New()
        End Sub

        Public GUID_TIMESTAMP As String = String.Empty
        Public Inventories_Available As New List(Of List(Of Wizard_Room))
        Public RadioButtonChoice As Int32 = 0
        Public Packages_Price As Decimal
        Public Packages As New List(Of Wizard_Package)

        Public Reservation As New Reservation
        Public Prospect As New Prospect
        Public Tour As New Tour
        Public Accom_Name As String = "KCP"
        Public Lead_ID As Int32
        Public Up4Orce_Fee As Decimal = "29.95"

        Public Sub Save_Hotel()
            Dim in_Synch = New InvoiceSynch
            Dim invoice_financials As New Package_Generate_Financials()
            Dim package_tour_package As New TourPackage_Package
            Dim invoice_tour_package As New TourPackage_Invoice
            Dim package_base As New Base_Package

            Me.Prospect.UserID = USER_ID
            Me.Prospect.Save(USER_ID)

            Dim prospect_id = Prospect.Prospect_ID
            Dim reservation_id = 0
            Dim package_id = 0
            Dim base_invoice_amt = 0
            Dim chargeback_amt = 0

            Dim checkin_date = DateTime.Parse(Reservation.CheckInDate)
            Dim checkout_date = DateTime.Parse(Reservation.CheckOutDate)
            Dim nights_stay = checkout_date.Subtract(checkin_date).Days
            package_id = Packages.First().Package.PackageID
            reservation_id = Reservation.ReservationID

            Tour.Save(Scenario, USER_ID)
            Reservation.Save(Scenario, USER_ID, Packages, Packages_Price, package_base.Get_Accom_Name(package_id))

            If Scenario = EnumScenario.Two Then

                invoice_tour_package = package_tour_package.Calculate_OnHOA_Hotel(nights_stay, checkin_date, package_id)
                package_tour_package.Calculate_Chargeback_Hotel(package_id, invoice_tour_package)

                invoice_financials.Create_Invoice_Hotel(0, package_tour_package.Get_Chargeback_MethodID(package_id), reservation_id, prospect_id, USER_ID, invoice_tour_package.Base_Amount, invoice_tour_package.Chargeback_Amount, "Two")

            ElseIf Scenario = EnumScenario.Three Then

                invoice_tour_package = package_tour_package.Calculate_OnHOA_Hotel(nights_stay, checkin_date, package_id)
                package_tour_package.Calculate_Chargeback_Hotel(package_id, invoice_tour_package)

                base_invoice_amt = invoice_tour_package.Base_Amount
                chargeback_amt = invoice_tour_package.Chargeback_Amount

                in_Synch.Run_sp_RW_Synch_Invoice(reservation_id, package_id, base_invoice_amt, chargeback_amt, 0, 0, USER_ID)
            End If
        End Sub
        Public Sub Save_All()
            Dim invoice_financials As New Package_Generate_Financials()
            Dim package_base As New Base_Package
            Dim package_allocation As New Allocation_Package

            Dim checkin_date = DateTime.Parse(Reservation.CheckInDate)
            Dim checkout_date = DateTime.Parse(Reservation.CheckOutDate)
            Dim prospect_id = Prospect.Prospect_ID
            Dim reservation_id = 0
            Dim package_id = 0
            Dim package_issued_id = 0
            Dim package_type = String.Empty
            Dim nights_stay = checkout_date.Subtract(checkin_date).Days


            Dim invoice_tour_promotion As New TourPromotion_Invoice
            Dim invoice_tour_package As New TourPackage_Invoice
            Dim invoice_tradeshow As New Tradeshow_Invoice
            Dim invoice_owner_getaway As New OwnerGetaway_Invoice
            Dim invoice_rentals As New List(Of Rental_Invoice)

            Dim package_tour_promotion As New TourPromotion_Package
            Dim package_owner_getaway As New OwnerGetaway_Package
            Dim package_tour_package As New TourPackage_Package
            Dim package_tradeshow As New Tradeshow_Package
            Dim package_rental As New Rental_Package

            Try
                Me.Prospect.UserID = USER_ID
                Me.Prospect.Save(USER_ID)

                prospect_id = Prospect.Prospect_ID

                If Scenario = EnumScenario.One Then

                    Reservation.ProspectID = Prospect.Prospect_ID

                    If Packages.Count = Packages.Where(Function(x) package_base.Get_Package_Type(x.PackageID) = "Rental" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Getaway" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Advantage").Count Then

                    Else
                        Tour.ProspectID = Prospect.Prospect_ID


                        Tour.Save(Scenario, USER_ID)
                        Reservation.TourID = Tour.TourID
                    End If

                    Reservation.Save(Scenario, USER_ID, Packages, Packages_Price)
                    reservation_id = Reservation.ReservationID
                    package_issued_id = Reservation.PackageIssuedID

                    If Packages.Count = Packages.Where(Function(x) package_base.Get_Package_Type(x.PackageID) = "Rental" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Getaway" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Advantage").Count Then

                    Else
                        Tour.PackageIssuedID = package_issued_id
                        Tour.ReservationID = Reservation.ReservationID
                        Tour.Save(Scenario, USER_ID)
                    End If



                    For Each p In Packages
                        For Each r In p.RoomID_List
                            package_allocation.Insert_Rooms_Allocation(reservation_id, checkin_date, nights_stay, r.Room_ID)
                        Next
                    Next

                    For Each p In Packages
                        package_allocation.Insert_PackageIssued_Related(package_issued_id, p.PackageID, p.RoomID_List.Select(Function(x) x.Room_ID).ToArray())
                    Next

                    For Each p In Packages
                        package_type = package_base.Get_Package_Type(p.PackageID)

                        If package_type = "Owner Getaway" Then
                            invoice_owner_getaway = package_owner_getaway.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)
                        ElseIf package_type = "Tour Promotion" Then
                            invoice_tour_promotion = package_tour_promotion.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)
                        ElseIf package_type = "Rental" Then
                            invoice_rentals.Add(package_rental.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID))
                        ElseIf package_type = "Tour Package" Then
                            invoice_tour_package = package_tour_package.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)
                            invoice_tour_package = package_tour_package.Calculate_Discount(nights_stay, checkin_date.ToShortDateString(), p.PackageID, invoice_tour_package)

                            invoice_financials.Create_Invoice_TourPackage(0, package_base.Get_Chargeback_MethodID(p.PackageID), reservation_id, prospect_id, USER_ID,
                                                                                  (invoice_tour_package.Base_Amount + invoice_tour_package.Additional_Amount), invoice_tour_package.Chargeback_Amount,
                                                                                    invoice_tour_package.Discount_Amount, "One")

                        ElseIf package_type = "Owner Advantage" Then
                            With New InvoiceOwnerAdvantage With { _
                                .PackageID = p.PackageID, _
                                .CheckIn = checkin_date.ToShortDateString(), _
                                .Nights = nights_stay, _
                                .ProspectID = prospect_id, _
                                .Reference = "Wizard Generated Invoice", _
                                .ReservationID = reservation_id, _
                                .Description = "Spinnaker's Owner Advantage package", _
                                .UserID = USER_ID}

                                .Calculate()
                            End With
                        End If
                    Next

                    If invoice_tour_promotion.Base_Amount > 0 Then

                        invoice_financials.Create_Invoice_TourPromotion(0, reservation_id, prospect_id, USER_ID,
                                (invoice_tour_promotion.Base_Amount) +
                                (invoice_rentals.Sum(Function(x) x.Base_Amount)),
                                invoice_tour_promotion.Discount_Amount, "One")

                    ElseIf invoice_owner_getaway.Base_Amount > 0 Then

                        invoice_financials.Create_Invoice_OwerGetaway(0, reservation_id, prospect_id, USER_ID,
                                (invoice_owner_getaway.Base_Amount + invoice_owner_getaway.Additional_Amount) +
                                (invoice_rentals.Sum(Function(x) x.Base_Amount)), "One")

                    ElseIf invoice_rentals.Sum(Function(x) x.Base_Amount) > 0 Then
                        Dim amount_diff = IIf(invoice_rentals.Sum(Function(x) x.Base_Amount) > Packages_Price, Packages_Price, invoice_rentals.Sum(Function(x) x.Base_Amount))
                        invoice_financials.Create_Invoice_Rental(0, reservation_id, prospect_id, USER_ID, amount_diff, "One")
                    End If


                ElseIf Scenario = EnumScenario.Two Then

                    Tour.Save(Scenario, USER_ID)
                    Reservation.Save(Scenario, USER_ID, Packages, Packages_Price)
                    package_id = Packages.First().Package.PackageID
                    reservation_id = Reservation.ReservationID
                    package_issued_id = Reservation.PackageIssuedID

                    '-- added 3/3/2020 next 2 lines of code
                    package_allocation.Delete_PackageIssued_Related(package_issued_id)
                    package_allocation.Reset_Rooms_Allocation(reservation_id)

                    For Each p In Packages
                        For Each r In p.Package.RoomID_List
                            package_allocation.Insert_Rooms_Allocation(reservation_id, checkin_date, nights_stay, r.Room_ID)
                        Next
                    Next

                    For Each p In Packages
                        package_allocation.Insert_PackageIssued_Related(package_issued_id, package_id, p.RoomID_List.Select(Function(x) x.Room_ID).ToArray())
                    Next

                    If package_base.Get_Package_Type(package_id) = "Tradeshow" Then

                        invoice_tradeshow = package_tradeshow.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                        invoice_tradeshow = package_tradeshow.Calculate_Chargeback(nights_stay, checkin_date.ToShortDateString(), package_id, invoice_tradeshow)
                        invoice_tradeshow = package_tradeshow.Calculate_ResortFee(package_id, invoice_tradeshow)

                        invoice_financials.Create_Invoice_Tradeshow(0, invoice_tradeshow.Chargeback_MethodID, package_tradeshow.Get_ResortFee_MethodID(package_id),
                                    reservation_id, prospect_id, USER_ID, (invoice_tradeshow.Base_Amount + invoice_tradeshow.Additional_Amount), invoice_tradeshow.ResortFee_Amount,
                                    invoice_tradeshow.Chargeback_Amount, "Two")

                    ElseIf package_base.Get_Package_Type(package_id) = "Tour Package" Then

                        invoice_tour_package = package_tour_package.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                        invoice_tour_package = package_tour_package.Calculate_Discount(nights_stay, checkin_date.ToShortDateString(), package_id, invoice_tour_package)

                        invoice_financials.Create_Invoice_TourPackage(0, package_base.Get_Chargeback_MethodID(package_id), reservation_id, prospect_id, USER_ID,
                                                                              (invoice_tour_package.Base_Amount + invoice_tour_package.Additional_Amount), invoice_tour_package.Chargeback_Amount,
                                                                                invoice_tour_package.Discount_Amount, "Two")
                    End If


                ElseIf Scenario = EnumScenario.Three Then

                    package_id = Packages.First().Package.PackageID

                    If package_base.Get_Package_Type(package_id) <> "Rental" Then
                        Tour.Save(Scenario, USER_ID)
                    End If
                    Reservation.TypeID = package_base.Get_Package_Reservation_Type_ID(package_id)
                    Reservation.Save(Scenario, USER_ID, Packages, Packages_Price)
                    reservation_id = Reservation.ReservationID
                    package_issued_id = Reservation.PackageIssuedID

                    package_allocation.Delete_PackageIssued_Related(package_issued_id)
                    package_allocation.Reset_Rooms_Allocation(reservation_id)

                    For Each p In Packages
                        For Each r In p.Package.RoomID_List
                            package_allocation.Insert_Rooms_Allocation(reservation_id, checkin_date, nights_stay, r.Room_ID)
                        Next
                    Next

                    For Each p In Packages
                        package_allocation.Insert_PackageIssued_Related(package_issued_id, p.Package.PackageID, p.Package.RoomID_List.Select(Function(x) x.Room_ID).ToArray())
                    Next

                    Dim base_invoice_amt = 0
                    Dim add_invoice_amt = 0
                    Dim resort_fee_amt = 0
                    Dim discount_amt = 0
                    Dim chargeback_amt = 0

                    Dim in_Synch = New InvoiceSynch

                    For Each p In Packages

                        package_type = package_base.Get_Package_Type(p.Package.PackageID)
                        package_id = p.Package.PackageID
                        '--Tradeshow, Tour Package, Tour Promotion, Rental, Owner Getaway

                        If package_type = "Owner Getaway" Then

                            invoice_owner_getaway = package_owner_getaway.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)

                        ElseIf package_type = "Tour Promotion" Then

                            invoice_tour_promotion = package_tour_promotion.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)

                        ElseIf package_base.Get_Package_Type(package_id) = "Tradeshow" Then

                            invoice_tradeshow = package_tradeshow.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                            invoice_tradeshow = package_tradeshow.Calculate_Chargeback(nights_stay, checkin_date.ToShortDateString(), package_id, invoice_tradeshow)
                            invoice_tradeshow = package_tradeshow.Calculate_ResortFee(package_id, invoice_tradeshow)

                        ElseIf package_base.Get_Package_Type(package_id) = "Tour Package" Then

                            invoice_tour_package = package_tour_package.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                            invoice_tour_package = package_tour_package.Calculate_Discount(nights_stay, checkin_date.ToShortDateString(), package_id, invoice_tour_package)


                        ElseIf package_base.Get_Package_Type(package_id) = "Rental" Then

                            invoice_rentals.Add(package_rental.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id))
                        End If
                    Next

                    base_invoice_amt = (invoice_owner_getaway.Base_Amount + invoice_owner_getaway.Additional_Amount) +
                        (invoice_tour_promotion.Base_Amount) +
                         (invoice_tradeshow.Base_Amount + invoice_tradeshow.Additional_Amount) +
                         (invoice_tour_package.Base_Amount + invoice_tour_package.Additional_Amount) +
                         (invoice_rentals.Sum(Function(x) x.Base_Amount))


                    discount_amt = invoice_tour_promotion.Discount_Amount + invoice_tour_package.Discount_Amount
                    chargeback_amt = invoice_tradeshow.Chargeback_Amount + invoice_tour_package.Chargeback_Amount
                    resort_fee_amt = invoice_tradeshow.ResortFee_Amount

                    in_Synch.Run_sp_RW_Synch_Invoice(reservation_id, package_id, base_invoice_amt, chargeback_amt, discount_amt, resort_fee_amt, USER_ID)

                ElseIf Scenario = EnumScenario.Eleven Then

                    Reservation.ProspectID = Prospect.Prospect_ID
                    Tour.Save(Scenario, USER_ID)
                    Reservation.Save(Scenario, USER_ID, Packages, Packages_Price)
                    reservation_id = Reservation.ReservationID
                    package_issued_id = Reservation.PackageIssuedID

                    For Each p In Packages
                        package_allocation.Insert_And_Reset_Room_Allocations(reservation_id,
                                                                             checkin_date,
                                                                             nights_stay,
                                                                             p.Package.RoomID_List.Select(Function(x) x.Room_ID).ToArray())
                    Next

                    package_allocation.Delete_PackageIssued_Related(package_issued_id)

                    For Each p In Packages
                        package_allocation.Insert_PackageIssued_Related(package_issued_id, p.PackageID, p.Package.RoomID_List.Select(Function(x) x.Room_ID).ToArray())
                    Next

                    For Each p In Packages
                        package_type = package_base.Get_Package_Type(p.Package.PackageID)

                        If package_type = "Tour Promotion" Then
                            invoice_tour_promotion = package_tour_promotion.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)
                        End If
                    Next

                    If Up4Orce_Fee = "29.95" Then
                        '' 30 - Reschedule Fee-HOA
                        invoice_financials.Create_Invoice_TourPromotion(30, reservation_id, prospect_id, USER_ID,
                                29.95, 0, "Eleven")
                    End If

                    Dim in_Synch = New InvoiceSynch
                    Dim base_invoice_amt = invoice_tour_promotion.Base_Amount

                    package_id = Packages.First().Package.PackageID
                    in_Synch.Run_sp_RW_Synch_Invoice(reservation_id, package_id, base_invoice_amt, 0, invoice_tour_promotion.Discount_Amount, 0, USER_ID)

                ElseIf Scenario = EnumScenario.Ten Then
                    Reservation.ProspectID = Prospect.Prospect_ID

                    If Packages.Count = Packages.Where(Function(x) package_base.Get_Package_Type(x.PackageID) = "Rental" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Getaway").Count Then

                    Else
                        Tour.ProspectID = Prospect.Prospect_ID
                        Tour.Save(Scenario, USER_ID)
                        Reservation.TourID = Tour.TourID
                    End If

                    Reservation.Save(Scenario, USER_ID, Packages, Packages_Price)
                    reservation_id = Reservation.ReservationID
                    package_issued_id = Reservation.PackageIssuedID

                    With New clsLeads
                        .LeadID = Me.Lead_ID
                        .Load()
                        '.ProspectID = prospect_id
                        .Save()
                    End With

                    If Packages.Count = Packages.Where(Function(x) package_base.Get_Package_Type(x.PackageID) = "Rental" _
                                                                Or package_base.Get_Package_Type(x.PackageID) = "Owner Getaway").Count Then

                    Else
                        Tour.PackageIssuedID = package_issued_id
                        Tour.ReservationID = Reservation.ReservationID
                        Tour.Save(Scenario, USER_ID)
                    End If

                    For Each p In Packages
                        For Each r In p.RoomID_List
                            package_allocation.Insert_Rooms_Allocation(reservation_id, checkin_date, nights_stay, r.Room_ID)
                        Next
                    Next

                    For Each p In Packages
                        package_allocation.Insert_PackageIssued_Related(package_issued_id, p.PackageID, p.RoomID_List.Select(Function(x) x.Room_ID).ToArray())
                    Next

                    For Each p In Packages
                        package_type = package_base.Get_Package_Type(p.PackageID)

                        If package_type = "Owner Getaway" Then
                            invoice_owner_getaway = package_owner_getaway.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)
                        ElseIf package_type = "Tour Promotion" Then
                            invoice_tour_promotion = package_tour_promotion.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID)
                        ElseIf package_type = "Rental" Then
                            invoice_rentals.Add(package_rental.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), p.PackageID))
                        End If
                    Next

                    If invoice_tour_promotion.Base_Amount > 0 Then

                        invoice_financials.Create_Invoice_TourPromotion(0, reservation_id, prospect_id, USER_ID,
                                (invoice_tour_promotion.Base_Amount) +
                                (invoice_rentals.Sum(Function(x) x.Base_Amount)),
                                invoice_tour_promotion.Discount_Amount, "Ten")

                    ElseIf invoice_owner_getaway.Base_Amount > 0 Then

                        invoice_financials.Create_Invoice_OwerGetaway(0, reservation_id, prospect_id, USER_ID,
                                (invoice_owner_getaway.Base_Amount + invoice_owner_getaway.Additional_Amount) +
                                (invoice_rentals.Sum(Function(x) x.Base_Amount)), "Ten")

                    Else
                        Dim amount_diff = IIf(invoice_rentals.Sum(Function(x) x.Base_Amount) > Packages_Price, Packages_Price, invoice_rentals.Sum(Function(x) x.Base_Amount))
                        invoice_financials.Create_Invoice_Rental(0, reservation_id, prospect_id, USER_ID, amount_diff, "Ten")
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Sub Load_All()
            Dim package_issued_id = 0
            Dim package_allocation = New Allocation_Package

            If Scenario = EnumScenario.One Then
                If Prospect.Prospect_ID > 0 Then
                    Prospect.Load(Me.Prospect.Prospect_ID)
                End If

            ElseIf Scenario = EnumScenario.Two Then
                Reservation.Load(Reservation.ReservationID, Scenario)
                Tour.TourID = New clsTour().Get_Tour_By_Res(Reservation.ReservationID)
                Tour.Load(Scenario)
                Prospect.Load(Reservation.ProspectID)
                package_issued_id = Reservation.PackageIssuedID
                Packages.Clear()
                With New clsPackageIssued
                    .PackageIssuedID = Reservation.PackageIssuedID
                    .Load()
                    Dim w_package As New Wizard_Package
                    w_package.PackageID = .PackageID
                    w_package.GUID = Guid.NewGuid.ToString
                    Packages.Add(w_package)
                End With

            ElseIf Scenario = EnumScenario.Three Then

                Reservation.Load(Reservation.ReservationID, Scenario, Accom_Name)
                Tour.TourID = New clsTour().Get_Tour_By_Res(Reservation.ReservationID)
                Tour.Load(Scenario)
                Prospect.Load(Reservation.ProspectID)
                package_issued_id = Reservation.PackageIssuedID
                With New clsPackageIssued
                    .PackageIssuedID = package_issued_id
                    .Load()
                    Accom_Name = New Base_Package().Get_Accom_Name(.PackageID)

                    If Accom_Name <> "KCP" Then
                        Dim w_package As New Wizard_Package
                        w_package.PackageID = .PackageID
                        w_package.GUID = Guid.NewGuid.ToString
                        Packages.Add(w_package)
                    Else
                        Packages.Clear()
                        Packages = package_allocation.List_PackageIssued_2_Packages(package_issued_id)
                    End If
                End With
                Reservation.Load(Reservation.ReservationID, Scenario, Accom_Name)

            ElseIf Scenario = EnumScenario.Ten Then
                If Prospect.Prospect_ID > 0 Then
                    Prospect.Addresses.Clear()
                    Prospect.Emails.Clear()
                    Prospect.Phones.Clear()
                    Tour.Premiums.Clear()
                    Prospect.Load(Me.Prospect.Prospect_ID)
                End If

                If Me.Prospect.Prospect_ID < 1 Then
                    With New clsLeads
                        .LeadID = Me.Lead_ID
                        .Load()

                        Prospect.First_Name = .FirstName
                        Prospect.Last_Name = .LastName
                        Prospect.MaritalStatusID = New clsComboItems().Lookup_ID("MaritalStatus", .MaritalStatus)

                        Prospect.Addresses.Clear()
                        Prospect.Emails.Clear()
                        Prospect.Phones.Clear()
                        Tour.Premiums.Clear()

                        If .EmailAddress.Length > 0 Then
                            Dim email_addr = .EmailAddress.Trim()
                            Me.Prospect.Emails.Add(
                                New clsEmail With {
                                    .EmailID = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9)),
                                    .Email = email_addr,
                                    .IsActive = True
                                })
                        End If

                        If .PhoneNumber.Length > 0 Then
                            Dim phone_num = .PhoneNumber.Trim()
                            Me.Prospect.Phones.Add(
                                New clsPhone With {
                                    .PhoneID = -Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(9)),
                                    .Number = phone_num,
                                    .Active = True
                                }
                            )
                        End If
                    End With
                End If

            ElseIf Scenario = EnumScenario.Eleven Then
                Prospect.Addresses.Clear()
                Prospect.Emails.Clear()
                Prospect.Phones.Clear()
                Tour.Premiums.Clear()
                Packages.Clear()

                Reservation.Load(Reservation.ReservationID, Scenario, Accom_Name)
                Tour.TourID = New clsTour().Get_Tour_By_Res(Reservation.ReservationID)
                Tour.Load(Scenario)
                Prospect.Load(Reservation.ProspectID)
                package_issued_id = Reservation.PackageIssuedID

                With New clsPackageIssued
                    .PackageIssuedID = package_issued_id
                    .Load()
                    Accom_Name = New Base_Package().Get_Accom_Name(.PackageID)

                    Packages = package_allocation.List_PackageIssued_2_Packages(package_issued_id)
                End With
            End If
        End Sub

        Public Scenario As EnumScenario
        Public USER_ID As Int32 = -1
        Public Search_CheckIn_Date As DateTime? = Nothing
        Public Search_CheckOut_date As DateTime? = Nothing
    End Class


    Public Class Wizard_Package

        Public PackageID As Int32
        Public GUID As String = String.Empty
        Public RoomID_List As New List(Of Wizard_Room)
        Public Package As Wizard_Package

    End Class

    Public Class Wizard_Room
        Public Room_ID As Int32
        Public Room_Number As String
        Public Room_Type As String
        Public Room_Sub_Type_1a As String
        Public Room_Sub_Type_1 As String
        Public Wizard_Room_GUID As String
    End Class

    Public Class Page

        Public Sub New()
        End Sub
        Public Name As String
        Public Visible As Boolean = True
    End Class

#End Region


#Region "Custom Exceptions"
    Public Class Exception_Tour_Waves_Not_Available
        Inherits Exception

        Private tour_date_start As DateTime, tour_date_end As DateTime

        Public Sub New(tour_date_start As DateTime, tour_date_end As DateTime)
            Me.tour_date_start = tour_date_start
            Me.tour_date_end = tour_date_end
        End Sub
        Public Overrides ReadOnly Property Message As String
            Get
                Return String.Format("No tour waves available between {0} and {1}",
                                    tour_date_start.ToShortDateString(), tour_date_end.ToShortDateString())
            End Get
        End Property
    End Class

    Public Class Exception_Allocation_Room_Search_Not_Exist
        Inherits Exception

        Private checkin_date As DateTime
        Private nights_stay As Int32

        Public Sub New(checkin_date As DateTime, nights_stay As Int32)
            Me.checkin_date = checkin_date
            Me.nights_stay = nights_stay
        End Sub
        Public Overrides ReadOnly Property Message As String
            Get
                Return String.Format("No rooms are available between {0} and {1}", checkin_date.ToShortDateString(), checkin_date.AddDays(nights_stay).ToShortDateString())
            End Get
        End Property
    End Class
    Public Class Exception_Package_Is_Not_Found_In_PackageIssued2Package_Table
        Inherits Exception

        Private package_issued_id As Int32

        Public Sub New(package_issued_id As Int32)
            Me.package_issued_id = package_issued_id
        End Sub

        Public Overrides ReadOnly Property Message As String
            Get
                Return String.Format("Package ID {0} does not have its assocations set in the related tables.", Me.package_issued_id)
            End Get
        End Property
    End Class
    Public Class Exception_Packages_Available_None
        Inherits Exception

        Private checkin_date As DateTime
        Private nights_stay As Int32

        Public Sub New(checkin_date As DateTime, nights_stay As Int32)
            Me.checkin_date = checkin_date
            Me.nights_stay = nights_stay
        End Sub
        Public Overrides ReadOnly Property Message As String
            Get
                Return String.Format("No packages available between {0} and {1}", checkin_date.ToShortDateString(),
                                             checkin_date.AddDays(nights_stay).ToShortDateString())
            End Get
        End Property
    End Class

    Public Class Exception_Allocated_Rooms_No_Longer_Available
        Inherits Exception

        Dim checkin_date As DateTime, checkout_date As DateTime

        Public Sub New(checkin_date As DateTime, checkout_date As DateTime)
            Me.checkin_date = checkin_date
            Me.checkout_date = checkout_date
        End Sub

        Public Overrides ReadOnly Property Message As String
            Get
                Return String.Format("Room(s) are no longer available during {0} and {1}. Either choose different rooms or go back and select another check-in date!", checkin_date.ToShortDateString(), checkout_date.ToShortDateString)
            End Get
        End Property
    End Class

#End Region


    Public Class InvoiceSynch
        Public Sub Run_sp_RW_Synch_Invoice(reservationID As Int32, packageID As Int32, onHoaAmt As Decimal, chargebackAmt As Decimal, discountAmt As Decimal, resortFee As Decimal, userID As Int32)
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand("sp_RW_Synch_Invoice", cn)
                    Try
                        cm.CommandType = CommandType.StoredProcedure
                        cm.Parameters.Add("@RESERVATION_ID", SqlDbType.Int).Value = reservationID
                        cm.Parameters.Add("@PACKAGE_ID", SqlDbType.Int).Value = packageID
                        cm.Parameters.Add("@ONHOA_AMOUNT", SqlDbType.SmallMoney).Value = onHoaAmt
                        cm.Parameters.Add("@CHARGE_BACK_AMOUNT", SqlDbType.SmallMoney).Value = chargebackAmt
                        cm.Parameters.Add("@DISCOUNT_AMOUNT", SqlDbType.SmallMoney).Value = discountAmt
                        cm.Parameters.Add("@RESORT_FEE_AMOUNT", SqlDbType.SmallMoney).Value = resortFee
                        cm.Parameters.Add("@USER_ID", SqlDbType.Int).Value = userID
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub
    End Class
    Public Class Package_Xtra

        Public Shared Inventories_Available As New List(Of List(Of Wizard_Room))

        Public Function Get_Tours_Available(checkin_date As Date, campaign_id As Int32, tour_location_id As Int32) As DataTable
            Dim dt = New DataTable
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand("select * from dbo.ufn_TourAvailability(@StartDate, @Days, @CampID, @LocID)", cn)

                    cm.CommandType = CommandType.Text
                    cm.Parameters.AddWithValue("@StartDate", checkin_date.AddDays(1).ToShortDateString())
                    cm.Parameters.AddWithValue("@Days", 0)
                    cm.Parameters.AddWithValue("@CampID", campaign_id)
                    cm.Parameters.AddWithValue("@LocID", tour_location_id)
                    Try
                        cn.Open()
                        Dim rd = cm.ExecuteReader()
                        If rd.HasRows Then
                            dt.Load(rd)
                        End If
                    Catch ex As Exception
                        Dim er = ex.Message
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            If dt.Rows.Count = 0 Then
                Throw New Exception_Tour_Waves_Not_Available(checkin_date.AddDays(1), checkin_date.AddDays(1))
            End If
            Return dt
        End Function

        Public Function Get_Tours_Available(checkin_date As Date, nights_stay As Int32, campaign_id As Int32, tour_location_id As Int32) As DataTable
            Dim dt = New DataTable
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand("select * from dbo.ufn_TourAvailability(@StartDate, @Days, @CampID, @LocID)", cn)
                    cm.CommandType = CommandType.Text
                    cm.Parameters.AddWithValue("@StartDate", checkin_date.AddDays(1).ToShortDateString())
                    cm.Parameters.AddWithValue("@Days", nights_stay - 2)
                    cm.Parameters.AddWithValue("@CampID", campaign_id)
                    cm.Parameters.AddWithValue("@LocID", tour_location_id)
                    Try
                        cn.Open()
                        Dim rd = cm.ExecuteReader()
                        If rd.HasRows Then
                            dt.Load(rd)
                        End If
                    Catch ex As Exception
                        Dim er = ex.Message
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            If dt.Rows.Count = 0 Then
                Throw New Exception_Tour_Waves_Not_Available(checkin_date.AddDays(1), checkin_date.AddDays(nights_stay - 2))
            End If
            Return dt
        End Function
        Public Function Get_Rooms_Avalailable(bed_room As String, room_unit_type_id As Integer, reservation_type_id As Integer, checkin_date As Date?, nights_stay As Short, Optional up_dwn As String = "") As List(Of List(Of Wizard_Room))

            If New clsComboItems().Lookup_ComboItem(room_unit_type_id) = "Estates" And Int32.Parse(bed_room.Substring(0, 1)) = 1 Then
                bed_room = String.Format("{0}bd-{1}", bed_room.Substring(0, 1), up_dwn)
            Else
                bed_room = Int32.Parse(bed_room.Substring(0, 1)).ToString()
            End If

            Dim sql = String.Format("select * from ufn_RoomsAvailable ('{0}', {1}, '{2}', '{3}', {4}, '{5}', '{6}', 0) where AVAILABLE = 'available'",
                bed_room, room_unit_type_id, checkin_date.Value.ToShortDateString(),
                checkin_date.Value.AddDays(nights_stay - 1).ToShortDateString(),
                reservation_type_id,
                New clsComboItems().Lookup_ComboItem(room_unit_type_id),
                New clsComboItems().Lookup_ComboItem(reservation_type_id))

            Dim l = New List(Of List(Of Wizard_Room))
            Dim dt = New DataTable()

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using ad = New SqlDataAdapter(sql, cn)
                    Try
                        ad.Fill(dt)
                        For Each dr As DataRow In dt.Rows
                            Dim m = New List(Of Wizard_Room)

                            Dim r_w_guid = Guid.NewGuid.ToString()
                            m.Add(New Wizard_Room With {.Room_ID = Convert.ToInt32(dr.Item("RoomID").ToString()),
                                  .Room_Number = dr("RoomNumber").ToString(), .Room_Type = dr("RoomType1").ToString(),
                                  .Room_Sub_Type_1 = dr("RoomSubType1").ToString(),
                                  .Room_Sub_Type_1a = dr("RoomSubType1a").ToString(),
                                  .Wizard_Room_GUID = r_w_guid})

                            If dr.Item("roomID2").Equals(DBNull.Value) = False Then
                                m.Add(New Wizard_Room With {.Room_ID = Convert.ToInt32(dr.Item("RoomID2").ToString()),
                                    .Room_Number = dr("RoomNumber2").ToString(), .Room_Type = dr("RoomType2").ToString(),
                                    .Room_Sub_Type_1 = dr("RoomSubType2").ToString(),
                                    .Wizard_Room_GUID = r_w_guid})
                            End If

                            If dr.Item("roomID3").Equals(DBNull.Value) = False Then
                                m.Add(New Wizard_Room With {.Room_ID = Convert.ToInt32(dr.Item("RoomID3").ToString()),
                                    .Room_Number = dr("RoomNumber3").ToString(), .Room_Type = dr("RoomType3").ToString(),
                                    .Room_Sub_Type_1 = dr("RoomSubType3").ToString(),
                                    .Wizard_Room_GUID = r_w_guid})
                            End If

                            l.Add(m)
                        Next

                    Catch ex As Exception
                        cn.Close()
                        ad.Dispose()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            If l.Count = 0 Then
                Throw New Exception_Allocation_Room_Search_Not_Exist(checkin_date, nights_stay)
            Else
                Inventories_Available.Clear()
                Inventories_Available = l
            End If
            Return l
        End Function

        Public Function Ensure_Rooms_Still_Available(rooms_id As List(Of Int32), checkin_date As Date?, nights_stay As Short) As Boolean
            Dim sql = String.Format("select count(*) from t_RoomAllocationMatrix where DateAllocated between '{0}' and '{1}' and RoomID in ({2}) and ReservationID < 1", checkin_date.Value.ToShortDateString(), checkin_date.Value.AddDays(nights_stay - 1), IIf(rooms_id.Count = 0, 0, String.Join(",", rooms_id.ToArray())))

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()

                        Dim sum = rooms_id.Count * nights_stay
                        Dim count = Convert.ToInt32(cm.ExecuteScalar())

                        If sum <> count And count > 0 Then
                            Throw New Exception_Allocated_Rooms_No_Longer_Available(checkin_date, checkin_date.Value.AddDays(nights_stay))
                        End If
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return True
        End Function
    End Class
    Public Class Package_Generate_Financials

        Public Sub Create_Invoice_TourPackage(fin_trans_id As Int32, method_id As Int32, reservation_id As Int32, prospect_id As Int32, user_id As Int32, invoice_amt As Decimal, chargeback_amt As Decimal, discount_amt As Decimal, wizard_phase As String)
            Dim invoice_id = 0
            Dim payment_id = 0

            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 32, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = "Wizard Generated Invoice"
                .Description = String.Format("Tour Package - Phase {0}", wizard_phase)
                .Amount = invoice_amt
                .PaymentMethodID = 0
                .Save()
                invoice_id = .InvoiceID
            End With

            With New clsPayments
                .PaymentID = 0
                .Load()
                .MethodID = method_id
                .Adjustment = 1
                .PosNeg = 1
                .Amount = chargeback_amt
                .ApplyToID = 0
                .UserID = user_id
                .ProspectID = prospect_id
                .TransDate = DateTime.Now
                .Reference = "Wizard Generated Payment"
                .Description = String.Format("Tour Package Chargeback - Phase {0}", wizard_phase)
                .Save()
                payment_id = .PaymentID
            End With
            With New clsPayment2Invoice
                .Inv2PayID = 0
                .Load()
                .InvoiceID = invoice_id
                .PaymentID = payment_id
                .Amount = chargeback_amt
                .PosNeg = 1
                .Save()
            End With

            If discount_amt > 0 Then
                With New clsPayments
                    .PaymentID = 0
                    .Load()
                    .MethodID = 18081
                    .Adjustment = 1
                    .PosNeg = 1
                    .Amount = discount_amt
                    .ApplyToID = 0
                    .UserID = user_id
                    .ProspectID = prospect_id
                    .TransDate = DateTime.Now
                    .Reference = "Wizard Generated Payment"
                    .Description = String.Format("Tour Package Discount - Phase {0}", wizard_phase)
                    .Save()
                    payment_id = .PaymentID
                End With
                With New clsPayment2Invoice
                    .Inv2PayID = 0
                    .Load()
                    .InvoiceID = invoice_id
                    .PaymentID = payment_id
                    .Amount = discount_amt
                    .PosNeg = 1
                    .Save()
                End With
            End If
        End Sub
        Public Sub Create_Invoice_OwerGetaway(fin_trans_id As Int32, reservation_id As Int32, prospect_id As Int32, user_id As Int32, invoice_amt As Decimal, wizard_phase As String)
            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 32, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = "Wizard Generated Invoice"
                .Description = String.Format("Owner Getaway - Phase {0}", wizard_phase)
                .Amount = invoice_amt
                .PaymentMethodID = 0
                .Save()
            End With
        End Sub
        Public Sub Create_Invoice_TourPromotion(fin_trans_id As Int32, reservation_id As Int32, prospect_id As Int32, user_id As Int32, invoice_amt As Decimal, discount_amt As Decimal, wizard_phase As String)
            Dim invoice_id = 0
            Dim payment_id = 0

            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 32, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = "Wizard Generated Invoice"
                .Description = String.Format("Tour Promotion - Phase {0}", wizard_phase)
                .Amount = invoice_amt
                .PaymentMethodID = 0
                .Save()
                invoice_id = .InvoiceID
            End With

            If discount_amt > 0 Then
                With New clsPayments
                    .PaymentID = 0
                    .Load()
                    .MethodID = 18081
                    .Adjustment = 1
                    .PosNeg = 1
                    .Amount = discount_amt
                    .ApplyToID = 0
                    .UserID = user_id
                    .ProspectID = prospect_id
                    .TransDate = DateTime.Now
                    .Reference = "Wizard Generated Payment"
                    .Description = String.Format("Tour Promotion Discount - Phase {0}", wizard_phase)
                    .Save()
                    payment_id = .PaymentID
                End With
                With New clsPayment2Invoice
                    .Inv2PayID = 0
                    .Load()
                    .InvoiceID = invoice_id
                    .PaymentID = payment_id
                    .Amount = discount_amt
                    .PosNeg = 1
                    .Save()
                End With
            End If

        End Sub
        Public Sub Create_Invoice_Tradeshow(fin_trans_id As Int32, method_id As Int32, resort_fee_method_id As Int32, reservation_id As Int32, prospect_id As Int32, user_id As Int32, invoice_amt As Decimal, resort_fee_amt As Decimal, chargeback_amt As Decimal, wizard_phase As String)
            Dim invoice_id = 0
            Dim payment_id = 0

            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 32, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = "Wizard Generated Invoice"
                .Description = String.Format("Tradeshow Package - Phase {0}", wizard_phase)
                .Amount = invoice_amt
                .PaymentMethodID = 0
                .Save()
                invoice_id = .InvoiceID
            End With

            If chargeback_amt > 0 Then
                With New clsPayments
                    .PaymentID = 0
                    .Load()
                    .MethodID = method_id
                    .Adjustment = 1
                    .PosNeg = 1
                    .Amount = chargeback_amt
                    .ApplyToID = 0
                    .UserID = user_id
                    .ProspectID = prospect_id
                    .TransDate = DateTime.Now
                    .Reference = "Wizard Generated Payment"
                    .Description = String.Format("Tour Package Discount - Phase {0}", wizard_phase)
                    .Save()
                    payment_id = .PaymentID
                End With
                With New clsPayment2Invoice
                    .Inv2PayID = 0
                    .Load()
                    .InvoiceID = invoice_id
                    .PaymentID = payment_id
                    .Amount = chargeback_amt
                    .PosNeg = 1
                    .Save()
                End With
            End If

            If resort_fee_amt > 0 Then
                With New clsInvoices
                    .InvoiceID = 0
                    .Load()
                    .FinTransID = resort_fee_method_id
                    .ProspectID = prospect_id
                    .KeyField = "RESERVATIONID"
                    .KeyValue = reservation_id
                    .TransDate = DateTime.Now
                    .DueDate = DateTime.Now.AddMonths(1)
                    .UserID = user_id
                    .Adjustment = 0
                    .PosNeg = 0
                    .ApplyToID = 0
                    .Reference = "Wizard Generated Invoice"
                    .Description = String.Format("Tradeshow Package - Phase {0}", wizard_phase)
                    .Amount = resort_fee_amt
                    .PaymentMethodID = 0
                    .Save()
                End With
            End If
        End Sub
        Public Sub Create_Invoice_Rental(fin_trans_id As Int32, reservation_id As Int32, prospect_id As Int32, user_id As Int32, invoice_amt As Decimal, wizard_phase As String)
            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 32, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = "Wizard Generated Invoice"
                .Description = String.Format("Rental - Phase {0}", wizard_phase)
                .Amount = invoice_amt
                .PaymentMethodID = 0
                .Save()
            End With
        End Sub
        Public Sub Create_Invoice_Hotel(fin_trans_id As Int32, method_id As Int32, reservation_id As Int32, prospect_id As Int32, user_id As Int32, invoice_amt As Decimal, chargeback_amt As Decimal, wizard_phase As String)
            Dim invoice_id = 0
            Dim payment_id = 0

            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 489, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = "Wizard Generated Invoice"
                .Description = String.Format("Hotel - Phase {0}", wizard_phase)
                .Amount = invoice_amt
                .PaymentMethodID = 0
                .Save()
                invoice_id = .InvoiceID
            End With
            With New clsPayments
                .PaymentID = 0
                .Load()
                .MethodID = method_id
                .Adjustment = 1
                .PosNeg = 1
                .Amount = chargeback_amt
                .ApplyToID = 0
                .UserID = user_id
                .ProspectID = prospect_id
                .TransDate = DateTime.Now
                .Reference = "Wizard Generated Payment"
                .Description = String.Format("Hotel Chargeback - Phase {0}", wizard_phase)
                .Save()
                payment_id = .PaymentID
            End With
            With New clsPayment2Invoice
                .Inv2PayID = 0
                .Load()
                .InvoiceID = invoice_id
                .PaymentID = payment_id
                .Amount = chargeback_amt
                .PosNeg = 1
                .Save()
            End With
        End Sub

    End Class
    Class Base_Package

        ''' <summary>
        '''             String.Format("Data Source=sql1;Initial Catalog=CRMSNet;Persist Security Info=True;User ID=asp;Password=aspnet")
        ''' </summary>


        Public Function Get_RateTable_ID(ByVal package_id As Integer) As Integer
            Dim rate_table_id As Integer = 0
            Dim sql As String = String.Format("select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = " &
                "ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        rate_table_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return rate_table_id
        End Function

        ''' <summary>
        ''' Looks up the t_PackageReservation table for the TypeID
        ''' </summary>
        ''' <param name="package_id"></param>
        ''' <returns></returns>
        Public Function Get_Package_Reservation_Type_ID(package_id As Int32) As Int32
            Dim package_reservation_type_id As Integer = 0
            Dim sql As String = String.Format("select pr.TypeID from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID where p.PackageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        package_reservation_type_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return package_reservation_type_id
        End Function

        ''' <summary>
        ''' Looks up the t_Package table for the UnitTypeID
        ''' </summary>
        ''' <param name="package_id"></param>
        ''' <returns></returns>
        Public Function Get_Package_Unit_Type_ID(ByVal package_id As Integer) As Integer
            Dim unit_type_id As Integer = 0
            Dim sql As String = String.Format("select UnitTypeID from t_Package where PackageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        unit_type_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return unit_type_id
        End Function
        Public Function Get_Package_Campaign_ID(ByVal package_id As Integer) As Integer
            Dim campaign_id As Integer = 0
            Dim sql As String = String.Format("select pt.CampaignID from t_Package p inner join t_PackageReservation pr on pr.PackageID = p.PackageID " _
                & "inner join t_PackageTour pt on pt.PackageReservationID = pr.PackageReservationID where p.PackageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        campaign_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return campaign_id
        End Function
        Public Function Get_Package_Tour_Location_ID(ByVal package_id As Integer) As Integer
            Dim tour_location_id As Integer = 0
            Dim sql As String = String.Format("select pt.TourLocationID from t_Package p inner join t_PackageReservation pr on pr.PackageID = p.PackageID " _
                & "inner join t_PackageTour pt on pt.PackageReservationID = pr.PackageReservationID where p.PackageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        tour_location_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return tour_location_id
        End Function
        Public Function Get_Accom_Name(package_id As Int32) As String
            Dim sql = String.Format("select COALESCE(a.AccomName, '') from t_Package p inner join t_Accom a on a.AccomID = p.AccomID where p.PackageID = {0}", package_id)
            Dim accom_name = String.Empty
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        accom_name = cm.ExecuteScalar().ToString()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return accom_name
        End Function

        Public Function Get_Package_Name(ByVal package_id As Integer) As String
            Dim package_name As String = String.Empty
            Dim sql As String = String.Format("select Package from t_package where packageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        package_name = Convert.ToString(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return package_name
        End Function
        Public Function Get_Package_Type(package_id As Int32) As String
            Dim package_type As String = String.Empty
            Dim sql As String = String.Format("select pt.ComboItem from t_Package p inner join t_ComboItems pt on p.TypeID = pt.ComboItemID where p.PackageID = {0};", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        package_type = Convert.ToString(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return package_type
        End Function

        Public Function Get_Promo_Nights(ByVal package_id As Integer) As Integer
            Dim nights As Integer = 0
            Dim sql As String = String.Format("select pr.PromoNights from t_Package p inner join t_PackageReservation pr on pr.PackageID = p.PackageID where p.PackageID = {0};", package_id)
            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        nights = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return nights
        End Function
        Public Function Get_Package_Description(package_id As Int32) As String
            Dim description As String = String.Empty
            Dim sql As String = String.Format("select COALESCE(p.Description, '') from t_Package p where p.PackageID = {0};", package_id)
            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        description = Convert.ToString(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return description
        End Function
        Public Function Get_Package_PackageID(reservation_id As Int32) As Int32
            Dim package_id = 0
            Dim sql As String = String.Format("select pi.PackageID from t_Reservations r inner join t_PackageIssued pi on pi.PackageIssuedID = r.PackageIssuedID where r.ReservationID = {0};", reservation_id)
            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        package_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return package_id
        End Function
        Public Function Get_Package_MinNights(package_id As Int32) As Int32
            Dim sql = String.Format("select p.MinNights from t_Package p where p.PackageID = {0}", package_id)
            Dim min_nights = 0
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        min_nights = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return min_nights
        End Function
        Public Function Get_Package_EndDate(package_id As Int32) As DateTime?
            Dim sql = String.Format("select p.EndDate from t_Package p where p.PackageID = {0}", package_id)
            Dim end_date As DateTime?
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        Dim ed = cm.ExecuteScalar()
                        If ed.Equals(DBNull.Value) = False Then end_date = Convert.ToDateTime(ed)
                    Catch ex As Exception
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return end_date
        End Function
        Public Function Get_Chargeback_MethodID(ByVal package_id As Integer) As Integer
            Dim method_id As Integer = 0
            Dim sql As String = String.Format("select distinct prp.PaymentMethodID as MethodID from t_PackageReservationFinTransCode prftc inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID " &
                "inner join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID where pr.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        method_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return method_id
        End Function
        Public Function Get_Package_Extra_Night_Allowed(package_id As Integer, checkin_date As Date) As Boolean
            Dim tf = False
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("Select Count(*) from t_PackageReservation2CheckInDay pc where PackageReservationID in (select pr.PackageReservationID from t_Package p " &
                                                        "inner join t_PackageReservation pr on p.PackageID = pr.PackageID where p.PackageID = {0}) and (DATEPART(dw, '{1}')  - 1 in (pc.CheckInDay))", package_id, checkin_date.ToShortDateString()), cn)
                    Try
                        cn.Open()
                        tf = IIf(Convert.ToInt32(cm.ExecuteScalar()) = 1, True, False)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return tf
        End Function
        Public Function Get_DiscountID() As Integer
            Dim discount_id As Integer = 0
            Dim sql As String = String.Format("select ci.ComboItemID from t_ComboItems ci inner join t_Combos c on c.ComboID = ci.ComboID where c.ComboName = 'PaymentMethod' and ci.ComboItem = 'Discount';")

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        discount_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return discount_id
        End Function
        Public Function Get_Package_VendorID(package_id As Int32) As Int32
            Dim package_vendor_id = 0
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("Select vendorID from t_Vendor2Package group by  PackageID, VendorID having PackageID={0}", package_id), cn)
                    Try
                        cn.Open()
                        package_vendor_id = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return package_vendor_id
        End Function
        Public Function Get_Package_Vendor_By_VendorID(package_id As Int32) As String
            Dim vendor = String.Empty
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("Select Vendor from t_Vendor Where VendorID = {0}", Get_Package_VendorID(package_id)), cn)
                    Try
                        cn.Open()
                        vendor = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return vendor
        End Function
        Public Function Get_Package_Bedroom(package_id As Int32) As String
            Dim bedroom = String.Empty
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sql = String.Format("select coalesce(Bedrooms, '') BedRooms from t_Package where PackageID = {0}", package_id)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        bedroom = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cm.Dispose()
                        cn.Close()
                    End Try
                End Using
            End Using
            Return bedroom
        End Function
    End Class
    Class OwnerGetaway_Package
        Inherits Base_Package

        Public Function Calculate_OnHOA(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer) As OwnerGetaway_Invoice
            Dim rate_table_id As Integer = Get_RateTable_ID(package_id)
            Dim promo_nights As Integer = Get_Promo_Nights(package_id)
            Dim i As OwnerGetaway_Invoice = New OwnerGetaway_Invoice
            Dim sql = String.Format("select Case when Sum(rtt.OwnerAmount) is null then 0 else Sum(rtt.OwnerAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and rtt.DATE between '{1}' and '{2}'", rate_table_id, checkin_date.ToShortDateString(), checkin_date.AddDays(promo_nights - 1).ToShortDateString())

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        i.Base_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            If DateTime.Compare(checkin_date.AddDays(promo_nights), checkin_date.AddDays((total_nights - 1))) <= 0 Then
                sql = String.Format("select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and rtt.DATE between '{1}' and '{2}'", rate_table_id, checkin_date.AddDays(promo_nights).ToShortDateString, checkin_date.AddDays((total_nights - 1)).ToShortDateString)

                Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                    Using cm As SqlCommand = New SqlCommand(sql, cn)
                        Try
                            cn.Open()
                            i.Additional_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                        Catch e As Exception
                            cn.Close()
                            Throw e
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
            End If

            Return i
        End Function
    End Class
    Class OwnerGetaway_Invoice

        Private base_amt As Decimal
        Private add_amt As Decimal

        Public Property Base_Amount As Decimal
            Get
                Return base_amt
            End Get
            Set(value As Decimal)
                base_amt = value
            End Set
        End Property

        Public Property Additional_Amount As Decimal
            Get
                Return add_amt
            End Get
            Set(value As Decimal)
                add_amt = value
            End Set
        End Property
    End Class
    Class Tradeshow_Package
        Inherits Base_Package

        Public Function Calculate_OnHOA(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer) As Tradeshow_Invoice
            Dim rate_table_id As Integer = Get_RateTable_ID(package_id)
            Dim promo_nights As Integer = Get_Promo_Nights(package_id)
            Dim i As Tradeshow_Invoice = New Tradeshow_Invoice

            i.Base_Amount = Me.Get_DefaultInvoice_Amount(package_id)
            If DateTime.Compare(checkin_date.AddDays(promo_nights), checkin_date.AddDays((total_nights - 1))) < 0 Then
                Dim sql As String = String.Format("select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and '{2}'", rate_table_id, checkin_date.AddDays(promo_nights).ToShortDateString, checkin_date.AddDays((total_nights - 1)).ToShortDateString)

                Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                    Using cm As SqlCommand = New SqlCommand(sql, cn)
                        Try
                            cn.Open()
                            i.Additional_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                        Catch e As Exception
                            cn.Close()
                            Throw e
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
            End If

            Return i
        End Function

        Public Function Calculate_Chargeback(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer, Optional ByVal i As Tradeshow_Invoice = Nothing) As Tradeshow_Invoice
            i.Chargeback_Amount = Me.Get_DefaultInvoice_Amount(package_id)
            i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
            Return i
        End Function

        Public Function Calculate_ResortFee(ByVal package_id As Integer, Optional ByVal i As Tradeshow_Invoice = Nothing) As Tradeshow_Invoice
            Dim sql As String = String.Format("select top 1 isnull(prftc.Amount, 0) Amount from t_FinTransCodes ftc inner join t_ComboItems ft on ftc.TransCodeID = ft.ComboItemID inner join t_PackageReservationFinTransCode prftc " &
                "on prftc.FinTransCodeID = ftc.FinTransID inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID where ft.ComboItem = 'resort fee' and pr.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        i.ResortFee_MethodID = Me.Get_ResortFee_MethodID(package_id)
                        i.ResortFee_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return i
        End Function

        Public Function Get_ResortFee_MethodID(ByVal package_id As Integer) As Integer
            Dim fin_trans_id As Integer = 0
            Dim sql As String = String.Format("select top 1 ftc.FinTransID from t_FinTransCodes ftc inner join t_ComboItems ft on ftc.TransCodeID = ft.ComboItemID inner join t_PackageReservationFinTransCode prftc " &
                "on prftc.FinTransCodeID = ftc.FinTransID inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID where ft.ComboItem = 'resort fee' and pr.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        fin_trans_id = Convert.ToInt32(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return fin_trans_id
        End Function

        Private Function Get_Inventory_Type(ByVal package_id As Integer) As String
            Dim inventory_type As String = String.Empty
            Dim sql As String = String.Format("select prt.ComboItem from t_Package p inner join t_ComboItems pt on pt.ComboItemID = p.TypeID inner join t_PackageReservation pr on pr.PackageID = p.PackageID inner join t_ComboItems prt on prt.ComboItemID = pr.TypeID where pt.ComboItem = 'tradeshow' and p.Active = 1 and p.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        inventory_type = Convert.ToString(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return inventory_type
        End Function

        Private Function Get_DefaultInvoice_Amount(ByVal package_id As Integer) As Decimal
            Dim def_inv_amt As Decimal = 0

            Dim sql As String = String.Format("select p.DefaultInvoiceAmt from t_Package p inner join t_ComboItems pt on pt.ComboItemID = p.TypeID " &
                "inner join t_PackageReservation pr on pr.PackageID = p.PackageID inner join t_ComboItems prt on prt.ComboItemID = pr.TypeID where pt.ComboItem = 'tradeshow' and p.Active = 1 and p.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        def_inv_amt = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return def_inv_amt
        End Function

    End Class
    Class Tradeshow_Invoice

        Private chargeback_amt As Decimal
        Private chargeback_id As Int32
        Private resortfee_amt As Decimal
        Private resortfee_id As Int32
        Private base_amt As Decimal
        Private add_amt As Decimal

        Public Property Additional_Amount As Decimal
            Get
                Return add_amt
            End Get
            Set(value As Decimal)
                add_amt = value
            End Set
        End Property

        Public Property Base_Amount As Decimal
            Get
                Return base_amt
            End Get
            Set(value As Decimal)
                base_amt = value
            End Set
        End Property

        Public Property Chargeback_Amount As Decimal
            Get
                Return chargeback_amt
            End Get
            Set(value As Decimal)
                chargeback_amt = value
            End Set
        End Property

        Public Property Chargeback_MethodID As Integer
            Get
                Return chargeback_id
            End Get
            Set(value As Integer)
                chargeback_id = value
            End Set
        End Property

        Public Property ResortFee_Amount As Decimal
            Get
                Return resortfee_amt
            End Get
            Set(value As Decimal)
                resortfee_amt = value
            End Set
        End Property

        Public Property ResortFee_MethodID As Integer
            Get
                Return resortfee_id
            End Get
            Set(value As Int32)
                resortfee_id = value
            End Set
        End Property
    End Class
    Class TourPromotion_Package
        Inherits Base_Package

        Public Function Calculate_OnHOA(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer) As TourPromotion_Invoice
            Dim rate_table_id As Integer = Get_RateTable_ID(package_id)
            Dim promo_nights As Integer = Get_Promo_Nights(package_id)
            Dim stay_nights() As DateTime = {}
            Dim i As TourPromotion_Invoice = New TourPromotion_Invoice

            For j As Int32 = 0 To total_nights - 1
                Array.Resize(stay_nights, stay_nights.Length + 1)
                stay_nights(stay_nights.Length - 1) = checkin_date.AddDays(j)
            Next


            If DateTime.Compare(checkin_date.AddDays(promo_nights), checkin_date.AddDays((total_nights - 1))) <= 0 Then
                Dim sql As String = String.Format("select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and '{2}'", rate_table_id, checkin_date.AddDays(promo_nights).ToShortDateString, checkin_date.AddDays((total_nights - 1)).ToShortDateString)

                Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                    Using cm As SqlCommand = New SqlCommand(sql, cn)
                        Try
                            cn.Open()
                            i.Additional_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                        Catch e As Exception
                            cn.Close()
                            Throw e
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
            End If
            i.Holiday_Amount = Get_Holiday_Rate(stay_nights, i)

            Dim def_amt = Get_DefaultInvoice_Amount(package_id)
            Dim pro_rate = Get_PromoRate_Amount(package_id)

            ' did not add nights beyond min nights stay
            If i.Additional_Amount = 0 Then
                i.Base_Amount = def_amt
                i.Discount_Amount = def_amt - (pro_rate + i.Holiday_Amount)
            Else
                If def_amt < (pro_rate + i.Additional_Amount) Then
                    i.Discount_Amount = 0
                    i.Base_Amount = pro_rate + i.Additional_Amount + i.Holiday_Amount
                Else
                    i.Discount_Amount = def_amt - (pro_rate + i.Additional_Amount)
                    i.Base_Amount = pro_rate + i.Additional_Amount + i.Holiday_Amount
                End If
            End If

            Return i
        End Function

        Private Function Get_Holiday_Rate(stay_nights() As DateTime, Optional ByVal i As TourPromotion_Invoice = Nothing) As Decimal
            Dim sql As String = String.Format("select coalesce(SUM(HolidayRate), 0) from t_RateTableHolidays where HolidayDate between '{0}' and '{1}';", stay_nights(0).ToShortDateString(), stay_nights(stay_nights.Length - 1).ToShortDateString())

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        i.Holiday_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return i.Holiday_Amount
        End Function
        Private Function Get_DefaultInvoice_Amount(ByVal package_id As Integer) As Decimal
            Dim def_inv_amt As Decimal = 0

            Dim sql As String = String.Format("select p.DefaultInvoiceAmt from t_Package p inner join t_ComboItems pt on pt.ComboItemID = p.TypeID " &
                "inner join t_PackageReservation pr on pr.PackageID = p.PackageID inner join t_ComboItems prt on prt.ComboItemID = pr.TypeID where pt.ComboItem = 'tour promotion' and p.Active = 1 and p.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        def_inv_amt = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return def_inv_amt
        End Function

        Private Function Get_PromoRate_Amount(ByVal package_id As Integer) As Decimal
            Dim pro_rate_amt As Decimal = 0
            Dim sql As String = String.Format("select IsNull(pr.PromoRate, 0) from t_Package p inner join t_ComboItems pt on pt.ComboItemID = p.TypeID inner join t_PackageReservation pr on pr.PackageID = p.PackageID inner join t_ComboItems prt on prt.ComboItemID = pr.TypeID " &
                                              "where pt.ComboItem = 'tour promotion' and p.Active = 1 and p.PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        pro_rate_amt = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return pro_rate_amt
        End Function
    End Class
    Class TourPromotion_Invoice

        Private discount_amt As Decimal
        Private holiday_amt As Decimal
        Private base_amt As Decimal
        Private add_amt As Decimal

        Public Property Base_Amount As Decimal
            Get
                Return base_amt
            End Get
            Set(value As Decimal)
                base_amt = value
            End Set
        End Property

        Public Property Additional_Amount As Decimal
            Get
                Return add_amt
            End Get
            Set(value As Decimal)
                add_amt = value
            End Set
        End Property

        Public Property Holiday_Amount As Decimal
            Get
                Return holiday_amt
            End Get
            Set(value As Decimal)
                holiday_amt = value
            End Set
        End Property

        Public Property Discount_Amount As Decimal
            Get
                Return discount_amt
            End Get
            Set(value As Decimal)
                discount_amt = value
            End Set
        End Property
    End Class
    Class TourPackage_Package
        Inherits Base_Package


        Public Function Calculate_OnHOA_Hotel(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer) As TourPackage_Invoice
            Dim i As TourPackage_Invoice = New TourPackage_Invoice
            Dim package_nights As Integer = Get_Promo_Nights(package_id)
            Dim extra_night As Boolean = Me.Get_Extra_Night(package_id, checkin_date)
            Dim package_night_end = checkin_date.AddDays(package_nights - 1 + (IIf(extra_night, 1, 0)))
            package_night_end = checkin_date.AddDays(total_nights - 1)
            Dim hotel_name = String.Empty

            Dim sql = String.Format("select acc.AccomName from t_Package p inner join t_Accom acc on acc.AccomID = p.AccomID where PackageID = {0}", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        hotel_name = Convert.ToString(cm.ExecuteScalar)
                        sql = String.Format("select Case when Sum(rtt.Amount) Is null Then 0 Else Sum(rtt.Amount) End As Rate from t_RateTableRates rtt inner join t_RateTable rt On rt.RateTableID = rtt.RateTableID where rt.Name = '{0}' and DATE between '{1}' and '{2}' ", hotel_name, checkin_date.ToShortDateString, package_night_end.ToShortDateString)
                        cm.CommandText = sql
                        i.Base_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return i
        End Function

        Public Function Calculate_Chargeback_Hotel(ByVal package_id As Integer, i As TourPackage_Invoice) As TourPackage_Invoice
            i.Chargeback_Amount = i.Base_Amount
            i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
            Return i
        End Function
        Public Function Calculate_OnHOA(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer) As TourPackage_Invoice
            Dim i As TourPackage_Invoice = New TourPackage_Invoice
            Dim package_nights As Integer = Get_Promo_Nights(package_id)
            Dim extra_night As Boolean = Me.Get_Extra_Night(package_id, checkin_date)
            Dim rate_table_id As Integer = Get_RateTable_ID(package_id)
            Dim package_night_end = checkin_date.AddDays(package_nights - 1 + (IIf(extra_night, 1, 0)))
            Dim addititional_night_start = checkin_date.AddDays(package_nights + (IIf(extra_night, 1, 0)))
            Dim additional_night_end = checkin_date.AddDays((total_nights - 1))


            Dim sql As String = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and '{2}' ", rate_table_id, checkin_date.ToShortDateString, package_night_end.ToShortDateString)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        i.Base_Amount = Convert.ToDecimal(cm.ExecuteScalar)

                        If total_nights > (package_nights + (IIf(extra_night, 1, 0))) Then
                            sql = String.Format("select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and '{2}'", rate_table_id, addititional_night_start.ToShortDateString, additional_night_end.ToShortDateString)

                            cm.CommandText = sql
                            i.Additional_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                        End If

                        '2/10/2021 added below 2 lines since amounts are wrong
                        Dim rw As New clsReservationWizard()
                        i.Base_Amount = rw.Get_Invoice_Amt(New Int32() {package_id}, 0, checkin_date, total_nights)
                        i.Additional_Amount = 0

                        rw = Nothing

                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return i
        End Function

        Public Function Calculate_Discount(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer, Optional ByVal i As TourPackage_Invoice = Nothing) As TourPackage_Invoice
            Dim promo_nights As Integer = Get_Promo_Nights(package_id)
            Dim rate_table_id As Integer = Get_RateTable_ID(package_id)
            Dim extra_night As Boolean = Me.Get_Extra_Night(package_id, checkin_date)
            Dim package_name As String = Get_Package_Name(package_id)
            Dim is_2br As Boolean = Me.Is_Tour_Package_2BR(package_id)
            Dim sql As String = String.Empty

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(String.Empty, cn)
                    Try
                        cn.Open()
                        If promo_nights = total_nights Then
                            If is_2br Then
                                sql = String.Format("Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = rtt.RateTableID where rtt.Date between '{0}' and '{1}' and rt.Name = '" &
                                    "1 Bedroom Cottage'", checkin_date.ToShortDateString, checkin_date.AddDays((promo_nights - 1)).ToShortDateString)
                                cm.CommandText = sql
                                i.Chargeback_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                                i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
                            Else
                                sql = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and " &
                                    " '{2}' ", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays((promo_nights - 1)).ToShortDateString)
                                cm.CommandText = sql
                                i.Chargeback_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                                i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
                            End If

                            If is_2br Then
                                i.Discount_Amount = i.Base_Amount - i.Chargeback_Amount
                            End If

                        Else
                            ' promo_nights == 2, plus an extra night
                            If promo_nights = 2 Then
                                If is_2br Then
                                    sql = String.Format("Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = " &
                                    "rtt.RateTableID where rtt.Date between '{0}' and '{1}' and rt.Name = '1 Bedroom Cottage' ", checkin_date.ToShortDateString, checkin_date.AddDays(promo_nights).ToShortDateString)
                                    cm.CommandText = sql
                                    i.Chargeback_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                                    i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
                                Else
                                    If extra_night Then
                                        sql = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and " &
                                       " '{2}' ", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays(promo_nights).ToShortDateString)
                                    Else
                                        sql = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and " &
                                       " '{2}' ", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays(promo_nights - 1).ToShortDateString)
                                    End If

                                    cm.CommandText = sql
                                    i.Chargeback_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                                    i.Discount_Amount = 0
                                    i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
                                End If

                                If is_2br Then
                                    sql = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and " &
                                        " '{2}' ", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays(promo_nights - 1).ToShortDateString)
                                    cm.CommandText = sql
                                    i.Discount_Amount = Convert.ToDecimal(cm.ExecuteScalar) - i.Chargeback_Amount
                                    i.Discount_Amount = i.Base_Amount - i.Chargeback_Amount
                                End If

                            End If

                            ' promo_nights == 3, plus an extra night 
                            ' rate for 2Br based on 1Br for the 3 nights
                            If promo_nights = 3 Then
                                If is_2br Then
                                    sql = String.Format("Select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTable rt inner join t_RateTableRates rtt on rt.RateTableID = " &
                                    " rtt.RateTableID where rtt.Date between '{0}' and '{1}' and rt.Name = '1 Bedroom Cottage' ", checkin_date.ToShortDateString, checkin_date.AddDays((promo_nights - 1)).ToShortDateString)
                                    cm.CommandText = sql
                                    i.Chargeback_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                                    i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
                                Else
                                    sql = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and " &
                                        " '{2}' ", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays((promo_nights - 1)).ToShortDateString)
                                    cm.CommandText = sql
                                    i.Chargeback_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                                    i.Chargeback_MethodID = Get_Chargeback_MethodID(package_id)
                                End If

                                sql = String.Format("select Case when Sum(rtt.TPAmount) is null then 0 else Sum(rtt.TPAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and " &
                                        " '{2}' ", rate_table_id, checkin_date.AddDays(promo_nights).ToShortDateString(), checkin_date.AddDays(promo_nights).ToShortDateString)
                                cm.CommandText = sql
                                i.Discount_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                            End If
                        End If

                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return i
        End Function

        Private Function Is_Tour_Package_2BR(ByVal package_id As Integer) As Boolean
            Dim is_2br As Boolean = False
            Dim sql As String = String.Format("select COUNT(*) from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID inner join t_ComboItems pt on pt.ComboItemID = p.TypeID where p.PackageID = {0} and pt.ComboItem in ('tour package') and LEFT(p.Bedrooms, 1) = 2", package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        is_2br = IIf(Convert.ToInt32(cm.ExecuteScalar()) = 0, False, True)
                    Catch e As Exception
                        cn.Close()
                        Throw e
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return is_2br
        End Function

        Private Function Get_Extra_Night(ByVal package_id As Integer, ByVal checkin_date As DateTime) As Boolean
            Dim extra_night As Boolean = False
            Dim sql As String = String.Format("select COUNT(*) from t_Package p inner join t_PackageReservation pr on pr.PackageID = p.PackageID inner join t_PackageReservation2CheckInDay pr2cd on pr.PackageReservationID = pr2cd.PackageReservationID where p.PackageID = {0} and pr2cd.CheckInDay in (select DATEPART(dw, '{1}') - 1);", package_id, checkin_date.ToShortDateString)
            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        extra_night = IIf(Convert.ToInt32(cm.ExecuteScalar()) = 0, False, True)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return extra_night
        End Function
    End Class
    Public Class TourPackage_Invoice

        Private chargeback_amt As Decimal
        Private discount_amt As Decimal
        Private chargeback_id As Int32
        Private base_amt As Decimal
        Private add_amt As Decimal

        Public Property Additional_Amount As Decimal
            Get
                Return add_amt
            End Get
            Set(value As Decimal)
                add_amt = value
            End Set
        End Property

        Public Property Base_Amount As Decimal
            Get
                Return base_amt
            End Get
            Set(value As Decimal)
                base_amt = value
            End Set
        End Property

        Public Property Chargeback_Amount As Decimal
            Get
                Return chargeback_amt
            End Get
            Set(value As Decimal)
                chargeback_amt = value
            End Set
        End Property

        Public Property Discount_Amount As Decimal
            Get
                Return discount_amt
            End Get
            Set(value As Decimal)
                discount_amt = value
            End Set
        End Property

        Public Property Chargeback_MethodID As Integer
            Get
                Return chargeback_id
            End Get
            Set(value As Integer)
                chargeback_id = value
            End Set
        End Property
    End Class
    Class Rental_Package
        Inherits Base_Package

        Public Function Calculate_OnHOA(ByVal total_nights As Integer, ByVal checkin_date As DateTime, ByVal package_id As Integer) As Rental_Invoice
            Dim i As Rental_Invoice = New Rental_Invoice
            Dim rate_table_id As Integer = Get_RateTable_ID(package_id)

            Dim sql As String = String.Format("select Case when Sum(rtt.RentalAmount) is null then 0 else Sum(rtt.RentalAmount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and '{2}'", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays(total_nights - 1).ToShortDateString)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        i.Base_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                        cm.CommandText = String.Format("select Case when Sum(rtt.Amount) is null then 0 else Sum(rtt.Amount) end as Rate from t_RateTableRates rtt where RateTableID = {0} and DATE between '{1}' and '{2}'", rate_table_id, checkin_date.ToShortDateString, checkin_date.AddDays(total_nights - 1).ToShortDateString)
                        i.Min_Amount = Convert.ToDecimal(cm.ExecuteScalar)
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return i
        End Function
    End Class
    Public Class Rental_Invoice
        Private base_amt As Decimal
        Private min_amt As Decimal

        Public Property Base_Amount As Decimal
            Get
                Return base_amt
            End Get
            Set(value As Decimal)
                base_amt = value
            End Set
        End Property

        Public Property Min_Amount As Decimal
            Get
                Return min_amt
            End Get
            Set(value As Decimal)
                min_amt = value
            End Set
        End Property
    End Class
    Public Class Allocation_Package

        Private sql As String = String.Empty

        Public Sub Reset_Rooms_Allocation(reservation_id As Int32)
            sql = String.Format("UPDATE t_RoomAllocationMatrix SET ReservationID = 0 WHERE ReservationID = {0};", reservation_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        Public Function List_PackageIssued_2_Packages(package_issued_id As Int32) As List(Of Wizard_Package)
            Dim l = New List(Of Wizard_Package)

            sql = String.Format("select pi2p.PackageID, pi2r.RoomID from t_PackageIssued2Package pi2p left join t_PackageIssued2Room pi2r on pi2p.ID = pi2r.PkgIss2PkgID " &
                                "inner join t_Package p on p.PackageID = pi2p.PackageID inner join t_ComboItems pt on pt.ComboItemID = p.TypeID " &
                                "where pt.ComboItem in ('Tour Promotion', 'Tradeshow', 'Tour Package') and pi2p.PackageIssuedID = {0} " &
                                "Union All " &
                                "select pi2p.PackageID, pi2r.RoomID from t_PackageIssued2Package pi2p left join t_PackageIssued2Room pi2r on pi2p.ID = pi2r.PkgIss2PkgID " &
                                "inner join t_Package p on p.PackageID = pi2p.PackageID inner join t_ComboItems pt on pt.ComboItemID = p.TypeID " &
                                "where pt.ComboItem in ('Owner Getaway') and pi2p.PackageIssuedID = {0} " &
                                "Union All " &
                                "select pi2p.PackageID, pi2r.RoomID from t_PackageIssued2Package pi2p left join t_PackageIssued2Room pi2r on pi2p.ID = pi2r.PkgIss2PkgID " &
                                "inner join t_Package p on p.PackageID = pi2p.PackageID inner join t_ComboItems pt on pt.ComboItemID = p.TypeID " &
                                "where pt.ComboItem in ('Rental') " &
                                "and pi2p.PackageIssuedID = {0};", package_issued_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        Dim dt = New DataTable
                        dt.Load(cm.ExecuteReader())

                        For Each gr In dt.AsEnumerable().GroupBy(Function(x) x.Field(Of Int32)("PackageID"))
                            Dim wiz_package = New Wizard_Package With {.PackageID = gr.Key, .GUID = Guid.NewGuid.ToString()}
                            wiz_package.Package = New Wizard_Package With {.PackageID = gr.Key, .GUID = Guid.NewGuid.ToString()}
                            l.Add(wiz_package)
                            For Each dr As DataRow In gr
                                If dr("RoomID").Equals(DBNull.Value) = False Then
                                    wiz_package.RoomID_List.Add(New Wizard_Room With {.Room_ID = dr("RoomID").ToString()})
                                    wiz_package.Package.RoomID_List.Add(New Wizard_Room With {.Room_ID = dr("RoomID").ToString()})
                                End If
                            Next
                        Next
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            Return l
        End Function
        Public Sub Insert_Rooms_Allocation(reservation_id As Int32, checkin_date As DateTime, stay_nights As Int32, room_id As Int32)
            sql = String.Format("Update t_RoomAllocationMatrix Set ReservationID = {0} where AllocationID In ( " &
                                    "select AllocationID from t_RoomAllocationMatrix where RoomID = {1} and ReservationID = 0 and " &
                                    "DateAllocated between '{2}' and '{3}')", reservation_id, room_id, checkin_date.ToShortDateString(),
                                                                             checkin_date.AddDays(stay_nights - 1).ToShortDateString())

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub
        Public Sub Insert_And_Reset_Room_Allocations(reservation_id As Int32, checkin_date As DateTime, stay_nights As Int32, rooms_id() As Int32)
            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(String.Empty, cn)
                    Try
                        cn.Open()
                        sql = String.Format("Update t_RoomAllocationMatrix Set ReservationID = 0 Where ReservationID = {0} And ReservationID > 0;", reservation_id)
                        cm.CommandText = sql
                        cm.ExecuteNonQuery()
                        For Each room_id As Int32 In rooms_id
                            sql = String.Format("Update t_RoomAllocationMatrix " _
                                                & "Set ReservationID = {0} " _
                                                & "Where RoomID = {1} " _
                                                & "And DateAllocated between '{2}' and '{3}' " _
                                                & "And ReservationID = 0", reservation_id, room_id,
                                                                           checkin_date.ToShortDateString(),
                                                                           checkin_date.AddDays(stay_nights - 1).ToShortDateString())
                            cm.CommandText = sql
                            cm.ExecuteNonQuery()
                        Next
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub
        Public Sub Delete_PackageIssued_Related(package_issued_id As Int32)

            sql = String.Format("delete from t_PackageIssued2Room where PkgIss2PkgID in (select ID from t_PackageIssued2Package where PackageIssuedID = {0}); " &
                        "delete from t_PackageIssued2Package where PackageIssuedID = {0}", package_issued_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        Public Sub Insert_PackageIssued_Related(package_issued_id As Int32, package_id As Int32, rooms_id() As Int32)
            sql = String.Format("Insert Into t_PackageIssued2Package Select {0}, {1}, GETDATE(), NULL;", package_issued_id, package_id)

            Using cn As SqlConnection = New SqlConnection(Resources.Resource.cns)
                Using cm As SqlCommand = New SqlCommand()
                    Try
                        If rooms_id.Length > 0 Then
                            sql += String.Format("Insert Into t_PackageIssued2Room ")
                            For i = 0 To rooms_id.Length - 1
                                sql += String.Format("select @@IDENTITY, {0} ", rooms_id(i))
                                If i < rooms_id.Length - 1 Then
                                    sql += "union all "
                                End If
                            Next
                        End If
                        cm.CommandText = sql
                        cm.Connection = cn
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

        End Sub
    End Class



    Public Class InvoiceOwnerAdvantage
        Inherits Payment

        Private cs As String = Resources.Resource.cns
        Private rw As New clsReservationWizard

        Public Sub New()
        End Sub

        Public Sub Calculate()

            Dim dt = New DataTable
            Dim sql = String.Format("select ftc.FinTransID from t_FinTransCodes ftc inner join t_ComboItems ft on ftc.TransCodeID = ft.ComboItemID inner join t_PackageReservationFinTransCode prftc on prftc.FinTransCodeID = ftc.FinTransID inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID where pr.PackageID = {0}", PackageID)

            Using cn = New SqlConnection(cs)
                Using cm = New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        FinTransID = cm.ExecuteScalar()
                    Catch ex As Exception
                        cn.Close()
                        Throw ex
                    End Try
                End Using
            End Using

            Dim amount = rw.Get_Invoice_Amt(New Int32() {PackageID}, 0, CheckIn, Nights)

            'ONHOA invoice
            Create(FinTransID, ProspectID, ReservationID, amount, UserID, Reference, Description)
        End Sub

    End Class

    Public MustInherit Class Invoice

        Protected Function Create(fin_trans_id As Int32, prospect_id As Int32, reservation_id As Int32, amount As Decimal, user_id As Int32, reference As String, descriptiont As String) As Int32
            Dim invoiceID As Int32 = 0

            With New clsInvoices
                .InvoiceID = 0
                .Load()
                .FinTransID = IIf(fin_trans_id = 0, 32, fin_trans_id)
                .ProspectID = prospect_id
                .KeyField = "RESERVATIONID"
                .KeyValue = reservation_id
                .TransDate = DateTime.Now
                .DueDate = DateTime.Now.AddMonths(1)
                .UserID = user_id
                .Adjustment = 0
                .PosNeg = 0
                .ApplyToID = 0
                .Reference = reference
                .Description = descriptiont
                .Amount = amount
                .PaymentMethodID = 0
                .Save()
                invoiceID = .InvoiceID
            End With

            Return invoiceID
        End Function

    End Class

    Public Class Payment
        Inherits Invoice

        Public Property FinTransID As Int32
        Public Property ProspectID As Int32
        Public Property ReservationID As Int32
        Public Property Amount As Decimal
        Public Property UserID As Int32
        Public Property Reference As String
        Public Property Description As String
        Public Property CheckIn As DateTime
        Public Property Nights As Int32
        Public Property PackageID As Int32

        Public Property Payments As List(Of Payment)

        Private paymentMethodID As Int32, paymentAmount As Int32, paymentDescription As String

        Public Sub New()
            Payments = New List(Of Payment)
        End Sub

        Public Sub New(methodID As Int32, amount As Decimal, description As String)
            paymentMethodID = methodID
            paymentAmount = amount
            paymentDescription = description
        End Sub

        Public Overloads Sub Create()

            Dim invoice_id = Create(FinTransID, ProspectID, ReservationID, Amount, UserID, Reference, Description)

            For Each p As Payment In Payments
                Dim payment_id = 0

                With New clsPayments
                    .PaymentID = 0
                    .Load()
                    .MethodID = p.paymentMethodID
                    .Adjustment = 1
                    .PosNeg = 1
                    .Amount = p.paymentAmount
                    .ApplyToID = 0
                    .UserID = UserID
                    .ProspectID = ProspectID
                    .TransDate = DateTime.Now
                    .Reference = "Wizard Generated Payment"
                    .Description = p.paymentDescription
                    .Save()
                    payment_id = .PaymentID
                End With

                With New clsPayment2Invoice
                    .Inv2PayID = 0
                    .Load()
                    .InvoiceID = invoice_id
                    .PaymentID = payment_id
                    .Amount = p.paymentAmount
                    .PosNeg = 1
                    .Save()
                End With
            Next
        End Sub

    End Class
End Class