Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsOnlineInventoryQuickCheck
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()

        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("", cn)
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Run_Report(ByVal sDate As Date, ByVal eDate As Date, ByVal opt As String, ByVal nights As Integer, ByVal uType As Integer, ByVal unitType As Integer, ByVal rmType As String, ByVal rmSubType As Integer) As String
        Dim rpt As String = ""
        Dim aUsageType(1) As String
        Dim aUnitType(1) As String
        Dim aRoomType(1) As String
        Dim aUsageTypeName(1) As String
        Dim aUnitTypeName(1) As String
        Dim aRoomTypeName(1) As String
        Dim i As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()

        'FILL ARRAYS
        If uType = 0 Then
            cm.CommandText = "Select c.ComboItemID, c.ComboItem from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.ComboName = 'ReservationType' and c.Active = 1 order by c.comboitem"
        Else
            cm.CommandText = "Select * from t_ComboItems where comboitemid = " & uType
        End If
        dread = cm.ExecuteReader
        i = 0
        Do While dread.Read
            If i > UBound(aUsageType) Then
                ReDim Preserve aUsageType(i)
                ReDim Preserve aUsageTypeName(i)
            End If
            aUsageType(i) = dread("ComboItemID")
            aUsageTypeName(i) = dread("ComboItem")
            i = i + 1
        Loop
        dread.Close()
        If unitType = 0 Then
            cm.CommandText = "Select c.ComboItemID, c.ComboItem from t_ComboItems c inner join t_Combos co on c.ComboID = co.COmboID where co.ComboName = 'UnitType' and c.Active = 1 order by c.comboitem"
        Else
            cm.CommandText = "Select * from t_ComboItems where comboitemid = " & unitType
        End If
        dread = cm.ExecuteReader
        i = 0
        Do While dread.Read
            If i > UBound(aUnitType) Then
                ReDim Preserve aUnitType(i)
                ReDim Preserve aUnitTypeName(i)
            End If
            aUnitType(i) = dread("ComboItemID")
            aUnitTypeName(i) = dread("ComboItem")
            i = i + 1
        Loop
        dread.Close()
        i = 0
        If rmType = "0" Then
            ReDim aRoomType(5)
            ReDim aRoomTypeName(5)
            cm.CommandText = "Select c.comboitem from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.ComboName = 'RoomType' and c.Active = 1 and c.ComboItem not like '2%'"
            dread = cm.ExecuteReader
            Do While dread.Read
                aRoomType(i) = dread("ComboItem")
                aRoomTypeName(i) = dread("ComboItem")
                i = i + 1
            Loop
            dread.Close()
            aRoomType(i) = "2"
            aRoomTypeName(i) = "2"
            i = i + 1
            aRoomType(i) = "3"
            aRoomTypeName(i) = "3"
            i = i + 1
            aRoomType(i) = "4"
            aRoomTypeName(i) = "4"
        Else
            aRoomType(0) = rmType
            aRoomTypeName(0) = rmType
        End If

        Dim d As Date = sDate
        Do While d <= CDate(eDate)
            For i = 0 To UBound(aUsageType)
                If aUsageType(i) <> "" Then
                    For x = 0 To UBound(aUnitType)
                        If aUnitType(x) <> "" Then
                            For y = 0 To UBound(aRoomType)
                                If aRoomType(y) <> "" Then
                                    If (aUnitTypeName(x) = "Townes" And (aRoomType(y) = "1" Or aRoomType(y) = "3" Or aRoomType(y) = "1BD-UP" Or aRoomType(y) = "1BD-DWN")) Or (aUnitTypeName(x) = "Cottage" And (aRoomType(y) = "1BD-UP" Or aRoomType(y) = "1BD-DWN")) Or (aUnitTypeName(x) = "Estates" And aRoomType(y) = "1") Then
                                    Else
                                        rpt = rpt & Get_And_Display(aUsageType(i), aUnitType(x), aRoomType(y), aUsageTypeName(i), aUnitTypeName(x), aRoomTypeName(y), d, nights, opt, rmSubType)
                                    End If

                                End If
                            Next
                        End If
                    Next
                End If
            Next
            d = d.AddDays(1)
        Loop
        'Catch ex As Exception

        '        Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return rpt
    End Function

    Protected Function Get_And_Display(ByVal usageid As String, ByVal unitid As String, ByVal roomid As String, ByVal usage As String, ByVal unit As String, ByVal room As String, ByVal startdate As Date, ByVal nights As Integer, ByVal opt As String, ByVal sType As String) As String
        Dim sAns As String = ""
        Dim subType As String = ""
        Dim rType As String = ""
        'Try
        If room = "1BD-DWN" Or room = "1BD-UP" Then
            rType = room
        Else
            rType = Left(room, 1)
        End If
        If sType = "0" Then
            subType = ""
        Else
            subType = sType
        End If

        If opt = "res" Then
            If subType = "" Then
                cm.CommandText = "select a.RoomNumber as [Room 1], a.RoomNumber2 as [2], a.RoomNumber3 as [3], st.comboitem as [Check-In Day], us.comboitem as [Style] from ufn_RoomsAvailable ('" & rType & "', '" & unitid & "','" & startdate & "','" & startdate.AddDays(nights - 1) & "','" & usageid & "', '" & unit & "', '" & usage & "', 0) a inner join t_Room r on r.roomid = a.roomid  left outer join t_Comboitems st on st.comboitemid = r.subtypeid inner join t_unit u on u.unitid = r.unitid left outer join t_comboitems us on us.comboitemid = u.styleid where available = 'available' order by st.comboitem"
            Else
                cm.CommandText = "select a.RoomNumber as [Room 1], a.RoomNumber2 as [2], a.RoomNumber3 as [3], st.comboitem as [Check-In Day], us.comboitem as [Style] from ufn_RoomsAvailable ('" & rType & "', '" & unitid & "','" & startdate & "','" & startdate.AddDays(nights - 1) & "','" & usageid & "', '" & unit & "', '" & usage & "', 0) a inner join t_Room r on r.roomid = a.roomid  left outer join t_Comboitems st on st.comboitemid = r.subtypeid inner join t_unit u on u.unitid = r.unitid left outer join t_comboitems us on us.comboitemid = u.styleid where available = 'available' and (st.comboitem is null or st.comboitemid = '" & subType & "') order by st.comboitem"
            End If
        Else
            If subType = "" Then
                cm.CommandText = "select a.RoomNumber as [Room 1], a.RoomNumber2 as [2], a.RoomNumber3 as [3], st.comboitem as [Check-In Day], us.comboitem as [Style] from ufn_UsageFreeRoomsAvailable ('" & rType & "', '" & unitid & "','" & startdate & "','" & startdate.AddDays(nights - 1) & "','" & usageid & "', '" & unit & "', '" & usage & "', 0)  a inner join t_Room r on r.roomid = a.roomid left outer join t_Comboitems st on st.comboitemid = r.subtypeid inner join t_unit u on u.unitid = r.unitid left outer join t_comboitems us on us.comboitemid = u.styleid where available = 'available' order by st.comboitem"
            Else
                cm.CommandText = "select a.RoomNumber as [Room 1], a.RoomNumber2 as [2], a.RoomNumber3 as [3], st.comboitem as [Check-In Day], us.comboitem as [Style] from ufn_UsageFreeRoomsAvailable ('" & rType & "', '" & unitid & "','" & startdate & "','" & startdate.AddDays(nights - 1) & "','" & usageid & "', '" & unit & "', '" & usage & "', 0)  a inner join t_Room r on r.roomid = a.roomid left outer join t_Comboitems st on st.comboitemid = r.subtypeid inner join t_unit u on u.unitid = r.unitid left outer join t_comboitems us on us.comboitemid = u.styleid where available = 'available' and (st.comboitem is null or st.comboitemid = '" & subType & "') order by st.comboitem"
            End If
        End If
        dread = cm.ExecuteReader
        If dread.HasRows Then
            If rType = "1BD-DWN" Or rType = "1BD-UP" Then
                sAns = sAns & "<div style='border:thin solid black;width:650px;'><b>" & usage & " - " & unit & " - " & room & "'s Available from " & startdate & " to " & startdate.AddDays(nights) & "</b>"
            Else
                sAns = sAns & "<div style='border:thin solid black;width:650px;'><b>" & usage & " - " & unit & " - " & room & "BD's Available from " & startdate & " to " & startdate.AddDays(nights) & "</b>"
            End If
            sAns = sAns & "<table border = 1 style='border-collapse:collapse;'><tr>"
            sAns = sAns & "<th>Room 1</th><th>2</th><th>3</th><th>Check-In Day</th><th>Style</th><th>Usage Type</th><th>Category</th></tr>"

            Dim cnn As New SqlConnection(Resources.Resource.cns)
            cnn.Open()

            Do While dread.Read
                sAns = sAns & "<tr>"
                sAns = sAns & "<td align = center>" & dread("Room 1") & "</td>"
                sAns = sAns & "<td align = center>" & dread("2") & "</td>"
                sAns = sAns & "<td align = center>" & dread("3") & "</td>"
                sAns = sAns & "<td align = center>" & dread("Check-In Day") & "</td>"
                sAns = sAns & "<td align = center>" & dread("Style") & "</td>"

                Dim sql As String = String.Format("select c.ComboItem from t_ComboItems c inner join t_Usage u on c.ComboItemID = u.TypeID where u.UsageID = {0}", usageid)

                Dim cmd As New SqlCommand(sql, cnn)
                Dim usage_type As String = DirectCast(cmd.ExecuteScalar(), String)

                sql = String.Format("select c.ComboItem from t_ComboItems c inner join t_Usage u on c.ComboItemID = u.CategoryID where u.UsageID = {0}", usageid)
                cmd = New SqlCommand(sql, cnn)

                Dim category As String = DirectCast(cmd.ExecuteScalar(), String)

                sAns = sAns & "<td align = center>" & usage_type & "</td>"
                sAns = sAns & "<td align = center>" & category & "</td>"

                sAns = sAns & "</tr>"
            Loop

            cnn.Close()
            sAns = sAns & "</table></div><br>"
        Else
        End If
        dread.Close()
        'Catch ex As Exception
        '_Err = ex.message
        'End Try
        Return sAns
    End Function
End Class
