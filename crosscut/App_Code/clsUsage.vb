Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsUsage
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _CategoryID As Integer = 0
    Dim _AmountPromised As Decimal = 0
    Dim _UsageYear As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _UnitTypeID As Integer = 0
    Dim _RoomTypeID As Integer = 0
    Dim _Days As Integer = 0
    Dim _Points As Integer = 0
    Dim _SoldInventoryID As Integer = 0
    Dim _InDate As String = ""
    Dim _OutDate As String = ""
    Dim _StatusID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim dread As SqlDataReader
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Usage where UsageID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Usage where UsageID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Usage")
            If ds.Tables("t_Usage").Rows.Count > 0 Then
                dr = ds.Tables("t_Usage").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("UsageID") Is System.DBNull.Value) Then _ID = dr("UsageID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("CategoryID") Is System.DBNull.Value) Then _CategoryID = dr("CategoryID")
        If Not (dr("AmountPromised") Is System.DBNull.Value) Then _AmountPromised = dr("AmountPromised")
        If Not (dr("UsageYear") Is System.DBNull.Value) Then _UsageYear = dr("UsageYear")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("RoomTypeID") Is System.DBNull.Value) Then _RoomTypeID = dr("RoomTypeID")
        If Not (dr("Days") Is System.DBNull.Value) Then _Days = dr("Days")
        If Not (dr("Points") Is System.DBNull.Value) Then _Points = dr("Points")
        If Not (dr("SoldInventoryID") Is System.DBNull.Value) Then _SoldInventoryID = dr("SoldInventoryID")
        If Not (dr("InDate") Is System.DBNull.Value) Then _InDate = dr("InDate")
        If Not (dr("OutDate") Is System.DBNull.Value) Then _OutDate = dr("OutDate")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
    End Sub

    Public Function Save() As Boolean
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_Usage where UsageID = " & _ID
        da = New SqlDataAdapter(cm)
        Dim sqlCMBuild As New SqlCommandBuilder(da)
        ds = New DataSet
        da.Fill(ds, "t_Usage")
        If ds.Tables("t_Usage").Rows.Count > 0 Then
            dr = ds.Tables("t_Usage").Rows(0)
        Else
            da.InsertCommand = New SqlCommand("dbo.sp_UsageInsert", cn)
            da.InsertCommand.CommandType = CommandType.StoredProcedure
            da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
            da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
            da.InsertCommand.Parameters.Add("@CategoryID", SqlDbType.Int, 0, "CategoryID")
            da.InsertCommand.Parameters.Add("@AmountPromised", SqlDbType.Money, 0, "AmountPromised")
            da.InsertCommand.Parameters.Add("@UsageYear", SqlDbType.Int, 0, "UsageYear")
            da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.Int, 0, "ContractID")
            da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.Int, 0, "UnitTypeID")
            da.InsertCommand.Parameters.Add("@RoomTypeID", SqlDbType.Int, 0, "RoomTypeID")
            da.InsertCommand.Parameters.Add("@Days", SqlDbType.Int, 0, "Days")
            da.InsertCommand.Parameters.Add("@Points", SqlDbType.Int, 0, "Points")
            da.InsertCommand.Parameters.Add("@SoldInventoryID", SqlDbType.Int, 0, "SoldInventoryID")
            da.InsertCommand.Parameters.Add("@InDate", SqlDbType.DateTime, 0, "InDate")
            da.InsertCommand.Parameters.Add("@OutDate", SqlDbType.DateTime, 0, "OutDate")
            da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
            da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
            Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@UsageID", SqlDbType.Int, 0, "UsageID")
            parameter.Direction = ParameterDirection.Output
            dr = ds.Tables("t_Usage").NewRow
        End If
        Update_Field("TypeID", _TypeID, dr)
        Update_Field("SubTypeID", _SubTypeID, dr)
        Update_Field("CategoryID", _CategoryID, dr)
        Update_Field("AmountPromised", _AmountPromised, dr)
        Update_Field("UsageYear", _UsageYear, dr)
        Update_Field("ContractID", _ContractID, dr)
        Update_Field("UnitTypeID", _UnitTypeID, dr)
        Update_Field("RoomTypeID", _RoomTypeID, dr)
        Update_Field("Days", _Days, dr)
        Update_Field("Points", _Points, dr)
        Update_Field("SoldInventoryID", _SoldInventoryID, dr)
        Update_Field("InDate", _InDate, dr)
        Update_Field("OutDate", _OutDate, dr)
        Update_Field("DateCreated", _DateCreated, dr)
        Update_Field("StatusID", _StatusID, dr)
        If ds.Tables("t_Usage").Rows.Count < 1 Then ds.Tables("t_Usage").Rows.Add(dr)
        da.Update(ds, "t_Usage")
        _ID = ds.Tables("t_Usage").Rows(0).Item("UsageID")
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return True
        'Catch ex As Exception
        '   _Err = ex.ToString
        '  Return False
        'Finally

        'End Try
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
            oEvents.KeyField = "UsageID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function get_Usage_ID(ByVal filter As String, ByVal filterText As String, ByVal usageYear As Integer, ByVal resType As String, ByVal inDate As Date, ByVal outDate As Date, ByVal ProspectID As Integer) As DataTable
        Dim dt As DataTable
        dt = New DataTable
        Dim oUsage As New clsUsage
        dt.Columns.Add("RoomID")
        dt.Columns.Add("RoomNumber")
        dt.Columns.Add("RoomType")
        dt.Columns.Add("RoomSubType")
        dt.Columns.Add("Category")
        Try
            cn = New SqlConnection(Resources.Resource.cns)
            cm = New SqlCommand("", cn)
            da = New SqlDataAdapter(cm)
            Dim dRow As DataRow
            ds = New DataSet
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Select Case filter
                Case "OwnerName"
                    If resType = "Exchange" Or resType = "Points" Or resType = "NALJR" Then
                        If InStr(filterText, ",") > 0 Then
                            cm.CommandText = "Select u.UsageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID where u.UsageYear = '" & usageYear & "' and lastname like '" & Replace(Trim(Left(filterText, InStr(filterText, ",") - 1)), "'", "''") & "%' and firstname like '" & Replace(Trim(Right(filterText, Len(filterText) - InStr(filterText, ","))), "'", "''") & "%' and ut.ComboItem = '" & resType & "' and u.InDate = '" & inDate & "' and u.OutDate = '" & outDate & "'"
                        Else
                            cm.CommandText = "Select u.UsageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID where u.UsageYear = '" & usageYear & "' and lastname like '" & Replace(Trim(filterText), "'", "''") & "%' and ut.ComboItem = '" & resType & "' and u.InDate = '" & inDate & "' and u.OutDate = '" & outDate & "'"
                        End If
                    Else
                        If InStr(filterText, ",") > 0 Then
                            cm.CommandText = "Select u.usageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID where u.UsageYear = '" & usageYear & "' and lastname like '" & Replace(Trim(Left(filterText, InStr(filterText, ",") - 1)), "'", "''") & "%' and firstname like '" & Replace(Trim(Right(filterText, Len(filterText) - InStr(filterText, ","))), "'", "''") & "%' and ut.ComboItem = '" & resType & "'"
                        Else
                            cm.CommandText = "Select u.usageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID where u.UsageYear = '" & usageYear & "' and lastname like '" & Replace(Trim(filterText), "'", "''") & "%' and ut.ComboItem = '" & resType & "'"
                        End If
                    End If
                Case "KCPNumber"
                    'cm.CommandText = "Select Distinct(rm.RoomID) from t_usage u inner join t_RoomAllocationMatrix rm on u.UsageID = rm.UsageID left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid where c.contractnumber = '" & filterText & "' and u.usageYear = '" & usageYear & "' and ut.comboitem = '" & resType & "'"
                    cm.CommandText = "Select u.UsageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid where c.contractnumber = '" & filterText & "' and u.usageYear = '" & usageYear & "' and ut.comboitem = '" & resType & "'"
                Case "IINumber"
                    cm.CommandText = "Select u.UsageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_UF_Value ufv on ufv.keyvalue = p.prospectid left outer join t_UFields uf on ufv.UFID = uf.UFID where uf.UFName = 'II Membership Number' and u.UsageYear = '" & usageYear & "' and ut.ComboItem = '" & resType & "' and ufv.UFValue =  '" & filterText & "'"
                Case "RCINumber"
                    cm.CommandText = "Select u.usageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_UF_Value ufv on ufv.keyvalue = p.prospectid left outer join t_UFields uf on ufv.UFID = uf.UFID where uf.UFName = 'RCI Membership Number' and u.UsageYear = '" & usageYear & "' and ut.ComboItem = '" & resType & "' and ufv.UFValue =  '" & filterText & "'"
                Case "ICENumber"
                    cm.CommandText = "Select u.usageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_UF_Value ufv on ufv.keyvalue = p.prospectid left outer join t_UFields uf on ufv.UFID = uf.UFID where uf.UFName = 'ICE Membership Number' and u.UsageYear = '" & usageYear & "' and ut.ComboItem = '" & resType & "' and ufv.UFValue =  '" & filterText & "'"
                Case "RCIPoints"
                    cm.CommandText = "Select u.UsageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid left outer join t_Prospect p on c.prospectid = p.prospectid left outer join t_UF_Value ufv on ufv.keyvalue = p.prospectid left outer join t_UFields uf on ufv.UFID = uf.UFID where uf.UFName = 'RCI Points Membership Number' and u.UsageYear = '" & usageYear & "' and ut.ComboItem = '" & resType & "' and ufv.UFValue =  '" & filterText & "'"
                Case "Year"
                    cm.CommandText = "Select u.UsageID from t_usage u left outer join t_Contract c on u.contractid = c.contractid left outer join t_ComboItems ut on u.typeid = ut.comboitemid where c.prospectid = '" & ProspectID & "' and u.UsageYear = '" & usageYear & "' and ut.ComboItem = '" & resType & "'"
            End Select
            da.Fill(ds, "usage")
            If ds.Tables("usage").Rows.Count > 0 Then
                For i = 0 To ds.Tables("usage").Rows.Count - 1
                    cm.CommandText = "Select Distinct(RoomID) As RoomID from t_RoomAllocationMatrix where usageID = '" & ds.Tables("usage").Rows(i).Item("UsageID") & "'"

                    da.Fill(ds, "rooms")
                    If ds.Tables("rooms").Rows.Count > 0 Then
                        For j = 0 To ds.Tables("rooms").Rows.Count - 1
                            cm.CommandText = "Select * from ufn_UsageRooms (" & ds.Tables("rooms").Rows(j).Item("RoomID") & ", '" & inDate & "', '" & outDate.AddDays(-1) & "', " & ds.Tables("usage").Rows(i).Item("UsageID") & ")"
                            da.Fill(ds, "availability")
                            If ds.Tables("availability").Rows.Count > 0 Then
                                For k = 0 To ds.Tables("availability").Rows.Count - 1
                                    If ds.Tables("availability").Rows(k).Item("available") = "available" And Not (ds.Tables("availability").Rows(k).Item("available") Is System.DBNull.Value) Then
                                        dRow = dt.NewRow
                                        With ds.Tables("availability").Rows(k)
                                            dRow("RoomID") = .Item("RoomID")
                                            dRow("RoomNumber") = .Item("RoomNumber")
                                            dRow("RoomType") = .Item("RoomType")
                                            dRow("RoomSubType") = .Item("RoomSubType")
                                            If resType = "Rental" Then
                                                dRow("Category") = oUsage.Get_Categories(.Item("RoomID"), CDate(inDate), CDate(outDate).AddDays(-1))
                                            Else
                                                dRow("Category") = "N/A"
                                            End If
                                        End With
                                        dt.Rows.Add(dRow)
                                    End If
                                Next
                            End If
                            ds.Tables("availability").Clear()
                        Next
                    End If
                    ds.Tables("rooms").Clear()
                Next i
            End If
            Return dt
        Catch ex As Exception
            _Err = ex.ToString
            Return dt
        Finally
            cm = Nothing
            da = Nothing
            cn = Nothing
            ds = Nothing
            oUsage = Nothing
        End Try
    End Function
    Public Function List(ByVal contractID As Integer) As SQLDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Top 50 u.UsageID, ut.ComboItem as Type, ust.ComboItem as SubType, urt.CombOitem as RoomType, u.UsageYear, u.Days, u.InDate, u.OutDate, us.ComboItem as Status from t_Usage u left outer join t_ComboItems ut on u.TypeID = ut.ComboItemID left outer join t_ComboItems ust on u.SubTypeID = ust.ComboItemID left outer join t_ComboItems urt on u.RoomTypeID = urt.ComboItemID left outer join t_ComboItems us on u.StatusID = us.ComboItemID where u.ContractID = '" & contractID & "' Order by u.UsageYear desc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Search_Usages(ByVal usageID As String, ByVal unitTypeID As Integer, ByVal usageTypeID As Integer, ByVal uSubTypeID As Integer, ByVal rmType As String, ByVal sDate As String, ByVal eDate As String, ByVal conNum As String) As DataTable

        Dim dt As New DataTable
        dt.Columns.Add("UsageID")
        dt.Columns.Add("ReservationID")
        dt.Columns.Add("Prospect")
        dt.Columns.Add("ContractNumber")
        dt.Columns.Add("UsageYear")
        dt.Columns.Add("UnitType")
        dt.Columns.Add("RoomType")
        dt.Columns.Add("Room(s)")
        dt.Columns.Add("UsageSubType")
        dt.Columns.Add("InDate")
        dt.Columns.Add("Days")
        dt.Columns.Add("UsageStatus")
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim sSQL As String = ""
            sSQL = "Select Distinct Top 1000 (u.UsageID), rmx.ReservationID, p.FirstName + ' ' + p.LastName as Prospect, c.ContractNumber, u.UsageYear, ut.Comboitem as UnitType, rmt.ComboItem as RoomType, ust.ComboItem as UsageSubType, u.InDate, u.Days, us.ComboItem as UsageStatus from t_Usage u left outer join t_Contract c on u.ContractID = c.ContractID inner join t_Prospect p on c.ProspectID = p.ProspectID left outer join t_RoomAllocationMatrix rmx on u.UsageID = rmx.UsageID left outer join t_ComboItems ut on u.UnitTypeID = ut.CombOitemID left outer join t_ComboItems rmt on u.RoomTypeID = rmt.ComboitemID left outer join t_ComboItems ust on u.SubTypeID = ust.ComboItemID left outer join t_Comboitems us on u.StatusID = us.ComboItemID"
            If usageID <> "" Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and u.usageID = " & CInt(usageID) & ""
                Else
                    sSQL = sSQL & " where u.UsageID = " & CInt(usageID) & ""
                End If
            End If
            If unitTypeID > 0 Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and u.UnitTypeID = " & unitTypeID & ""
                Else
                    sSQL = sSQL & " where u.UnitTypeID = " & unitTypeID & ""
                End If
            End If
            If usageTypeID > 0 Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and u.TypeID = " & usageTypeID & ""
                Else
                    sSQL = sSQL & " where u.TypeID = " & usageTypeID & ""
                End If
            End If
            If uSubTypeID > 0 Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and u.SubTypeID = " & uSubTypeID & ""
                Else
                    sSQL = sSQL & " where u.SubTypeID = " & uSubTypeID & ""
                End If
            End If
            If rmType <> "''" Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and u.RoomTypeID In (" & rmType & ")"
                Else
                    sSQL = sSQL & " where u.RoomTypeID In (" & rmType & ")"
                End If
            End If
            If sDate <> "" Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and u.InDate between '" & sDate & "' and '" & eDate & "'"
                Else
                    sSQL = sSQL & " where u.InDate between '" & sDate & "' and '" & eDate & "'"
                End If
            End If
            If conNum <> "" Then
                If InStr(sSQL, "where") <> 0 Then
                    sSQL = sSQL & " and c.ContractNumber = '" & conNum & "'"
                Else
                    sSQL = sSQL & " where c.ContractNumber = '" & conNum & "'"
                End If
            End If
            sSQL &= " Order by u.InDate, u.UsageID "

            cm.CommandText = sSQL
            Dim da As New SqlDataAdapter(cm)
            Dim ds As New DataSet
            da.Fill(ds, "Usages")

            If ds.Tables("Usages").Rows.Count > 0 Then
                Dim dr As DataRow
                For i = 0 To ds.Tables("Usages").Rows.Count - 1
                    dr = dt.NewRow
                    dr("UsageID") = ds.Tables("Usages").Rows(i).Item("UsageID")
                    dr("ReservationID") = ds.Tables("Usages").Rows(i).Item("ReservationID")
                    dr("Prospect") = ds.Tables("Usages").Rows(i).Item("Prospect")
                    dr("ContractNumber") = ds.Tables("Usages").Rows(i).Item("ContractNumber")
                    dr("UsageYear") = ds.Tables("Usages").Rows(i).Item("UsageYear")
                    dr("UnitType") = ds.Tables("Usages").Rows(i).Item("UnitType")
                    dr("RoomType") = ds.Tables("Usages").Rows(i).Item("RoomType")
                    dr("UsageSubType") = ds.Tables("Usages").Rows(i).Item("UsageSubtype")
                    dr("InDate") = ds.Tables("Usages").Rows(i).Item("InDate")
                    dr("Days") = ds.Tables("Usages").Rows(i).Item("Days")
                    dr("UsageStatus") = ds.Tables("Usages").Rows(i).Item("UsageStatus")

                    cm.CommandText = "Select Distinct(rm.RoomNumber) from t_RoomAllocationMatrix rmx inner join t_Room rm on rmx.RoomID = rm.RoomID where rmx.UsageID = " & ds.Tables("Usages").Rows(i).Item("UsageID")
                    da.Fill(ds, "Rooms")
                    If ds.Tables("Rooms").Rows.Count > 0 Then
                        For j = 0 To ds.Tables("Rooms").Rows.Count - 1
                            If j = 0 Then
                                dr("Room(s)") = ds.Tables("Rooms").Rows(j).Item("RoomNumber")
                            Else
                                dr("Room(s)") = dr("Room(s)") & "/" & ds.Tables("Rooms").Rows(j).Item("RoomNumber")
                            End If
                        Next
                    Else
                        dr("Room(s)") = ""
                    End If
                    ds.Tables("Rooms").Clear()
                    dt.Rows.Add(dr)
                Next
            End If

            da.Dispose()
            da = Nothing
            If cn.State <> ConnectionState.Closed Then cn.Close()


            'Replace 3/28/2013 - MB
            'Dim ds As New SqlDataSource
            'Dim sSQL As String
            'Try
            '    ds.ConnectionString = Resources.Resource.cns
            '    sSQL = "Select Distinct Top 1000 (u.UsageID), rmx.ReservationID, p.FirstName + ' ' + p.LastName as Prospect, c.ContractNumber, u.UsageYear, ut.Comboitem as UnitType, rmt.ComboItem as RoomType, rm.RoomNumber, ust.ComboItem as UsageSubType, u.InDate, u.Days, us.ComboItem as UsageStatus from t_Usage u left outer join t_Contract c on u.ContractID = c.ContractID inner join t_Prospect p on c.ProspectID = p.ProspectID left outer join t_RoomAllocationMatrix rmx on u.UsageID = rmx.UsageID left outer join t_ComboItems ut on u.UnitTypeID = ut.CombOitemID left outer join t_ComboItems rmt on u.RoomTypeID = rmt.ComboitemID left outer join t_Room rm on rmx.RoomID = rm.RoomID left outer join t_ComboItems ust on u.SubTypeID = ust.ComboItemID left outer join t_Comboitems us on u.StatusID = us.ComboItemID"
            '    If usageID <> "" Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and u.usageID = " & CInt(usageID) & ""
            '        Else
            '            sSQL = sSQL & " where u.UsageID = " & CInt(usageID) & ""
            '        End If
            '    End If
            '    If unitTypeID > 0 Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and u.UnitTypeID = " & unitTypeID & ""
            '        Else
            '            sSQL = sSQL & " where u.UnitTypeID = " & unitTypeID & ""
            '        End If
            '    End If
            '    If usageTypeID > 0 Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and u.TypeID = " & usageTypeID & ""
            '        Else
            '            sSQL = sSQL & " where u.TypeID = " & usageTypeID & ""
            '        End If
            '    End If
            '    If uSubTypeID > 0 Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and u.SubTypeID = " & uSubTypeID & ""
            '        Else
            '            sSQL = sSQL & " where u.SubTypeID = " & uSubTypeID & ""
            '        End If
            '    End If
            '    If rmType <> "''" Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and u.RoomTypeID In (" & rmType & ")"
            '        Else
            '            sSQL = sSQL & " where u.RoomTypeID In (" & rmType & ")"
            '        End If
            '    End If
            '    If sDate <> "" Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and u.InDate between '" & sDate & "' and '" & eDate & "'"
            '        Else
            '            sSQL = sSQL & " where u.InDate between '" & sDate & "' and '" & eDate & "'"
            '        End If
            '    End If
            '    If conNum <> "" Then
            '        If InStr(sSQL, "where") <> 0 Then
            '            sSQL = sSQL & " and c.ContractNumber = '" & conNum & "'"
            '        Else
            '            sSQL = sSQL & " where c.ContractNumber = '" & conNum & "'"
            '        End If
            '    End If
            '    sSQL &= " Order by u.InDate, u.UsageID "
            '    ds.SelectCommand = sSQL
        Catch ex As Exception
            _Err = ex.Message
        End Try
        '        Return ds
        Return dt
    End Function
    Public Function List_Rooms(ByVal usageID As Integer, ByVal outDate As Date) As DataTable
        Dim dt As New DataTable
        Dim drow As DataRow
        dt.Columns.Add("RoomID")
        dt.Columns.Add("RoomNumber")
        dt.Columns.Add("Style")
        dt.Columns.Add("Removable")
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(rmx.RoomID), rm.RoomNumber, us.ComboItem as Style from t_Roomallocationmatrix rmx inner join t_Room rm on rmx.roomid = rm.roomid inner join t_Unit u on rm.Unitid = u.unitid left outer join t_COmboItems us on u.StyleID = us.ComboItemID where rmx.UsageID = '" & usageID & "'"
            dread = cm.ExecuteReader()
            Do While dread.Read
                drow = dt.NewRow
                drow.Item("RoomID") = dread("RoomID") & ""
                drow.Item("RoomNumber") = dread("RoomNumber") & ""
                drow.Item("Style") = dread("Style") & ""
                If DateTime.Compare(System.DateTime.Now, outDate) >= 0 Then
                    drow.Item("Removable") = "NO"
                Else
                    drow.Item("Removable") = "YES"
                End If
                dt.Rows.Add(drow)
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return dt
    End Function
    Public Function List_Usage_Reservations(ByVal usageID As Integer, ByVal roomID As Integer, ByVal inDateCheck As Boolean, ByVal inDate As Date) As String
        Dim reservations As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If inDateCheck Then
                cm.CommandText = "Select Distinct(ReservationID) As ReservationID from t_RoomAllocationMatrix where usageid = '" & usageID & "' and roomID = '" & roomID & "' and reservationID > 0 and dateAllocated >= '" & CDate(inDate).ToShortDateString & "'"
            Else
                cm.CommandText = "Select Distinct(ReservationID) As ReservationID from t_RoomAllocationMatrix where usageid = '" & usageID & "' and roomID = '" & roomID & "' and reservationID > 0"
            End If
            dread = cm.ExecuteReader()
            Do While dread.Read
                If reservations = "" Then
                    reservations = reservations & dread("ReservationID")
                Else
                    reservations = reservations & "," & dread("ReservationID")
                End If
            Loop
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return reservations
    End Function
    Public Function Search_Rooms(ByVal inDate As Date, ByVal outDate As Date, ByVal resType As Integer, ByVal rmType As Integer, ByVal uType As Integer) As SqlDataSource
        Dim roomType As String = ""
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Dim oCombo As New clsComboItems
        roomType = oCombo.Lookup_ComboItem(rmType)
        If roomType <> "1BD-DWN" And roomType <> "1BD-UP" Then
            roomType = Left(roomType, 1)
        End If

        Try
            'ds.SelectCommand = "Select xx.RoomID, xx.RoomNumber, xx.RoomType, xx.SubType, xx.Style from (Select Distinct(rmx.RoomID), Count(rmx.AllocationID) as Nights, rm.RoomNumber, rmt.ComboItem as Roomtype, rst.CombOitem as SubType, us.ComboItem as Style from t_RoomAllocationMatrix rmx inner join t_Room rm on rmx.RoomID = rm.RoomID inner join t_Unit u on rm.UnitID = u.UnitID inner join t_ComboItems rmt on rm.TypeID = rmt.CombOitemID inner join t_ComboItems ut on u.TypeID = ut.ComboItemID inner join t_ComboItems rst on rm.SubTypeID = rst.CombOItemID left outer join t_ComboItems us on u.StyleID = us.ComboItemID where rmx.DateAllocated between '" & inDate & "' and '" & CDate(outDate.AddDays(-1)) & "' and rmx.TypeID = '" & resType & "' and rmt.ComboItemID = '" & rmType & "' and u.TypeID = '" & UnitTypeID & "' and rmx.UsageID = 0 and rmx.ReservationID = 0 group by rmx.RoomID, rm.RoomNumber, rmt.ComboItem, rst.CombOitem, us.ComboItem) xx where xx.Nights = " & DateDiff("d", CDate(inDate), CDate(outDate)) & ""
            ds.SelectCommand = "Select Case when RoomID2 is null then cast(RoomID AS varchar) when roomid2 is not null and roomid3 is null then Cast(roomID as varchar) + '|' + Cast(roomID2 as varchar) else cast(roomID as varchar) + '|' + cast(RoomID2 as varchar) + '|' + cast(roomID3 as varchar) end as ID, Case when roomnumber2 is null then RoomNumber when roomnumber2 is not null and roomnumber3 IS null then RoomNumber + '|' + RoomNumber2 else RoomNumber + '|' + RoomNumber2 + '|' + RoomNumber3 end as [Room(s)], Case when roomtype2 is null then roomType when roomtype2 is not null and roomtype3 is null then roomtype + '|' + roomtype2 else roomtype + '|' + roomtype2 + '|' + roomtype3 end as [RoomType(s)], case when roomsubtype2 IS null then RoomSubType when roomsubtype2 is not null and roomsubtype3 is null then RoomSubType + '|' + RoomSubType2 else RoomSubType + '|' + RoomSubType2 + '|' + RoomSubType3 end as [RoomSubType(s)], UnitStyle from ufn_UsageFreeRoomsAvailable('" & roomType & "'," & UnitTypeID & ",'" & inDate & "','" & CDate(outDate.AddDays(-1)) & "'," & resType & ",'" & oCombo.Lookup_ComboItem(UnitTypeID) & "','" & oCombo.Lookup_ComboItem(resType) & "',0) where available = 'available' order by available"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        oCombo = Nothing
        Return ds
    End Function
    Public Function Add_Room(ByVal usageID As Integer, ByVal roomID As Integer, ByVal uInDate As Date, ByVal uOutDate As Date) As Boolean
        Dim dRow As DataRow
        Dim trans As SqlTransaction
        If cn.State <> ConnectionState.Open Then cn.Open()
        trans = cn.BeginTransaction()
        Try
            cm.CommandText = "Select * from t_RoomAllocationmatrix where roomID = '" & roomID & "' and dateAllocated between '" & uInDate & "' and '" & uOutDate & "'"
            cm.Transaction = trans
            da = New SqlDataAdapter(cm)
            ds = New DataSet()
            da.Fill(ds, "Rooms")
            If ds.Tables("Rooms").Rows.Count > 0 Then
                Dim x As Integer
                Dim oCombo As New clsComboItems
                Dim oRooms As New clsRooms
                oRooms.RoomID = roomID
                oRooms.Load()
                If oCombo.Lookup_ComboItem(oRooms.SubTypeID).ToString.ToUpper <> Left(CDate(uInDate).DayOfWeek.ToString, 3).ToUpper And Not (CheckSecurity("Usage", "AddDiffCheckInDayRoom",,, _UserID)) Then
                    Throw New Exception("The Check-In Day of the Usage Does Not Match the Check-In Day of the Room.")
                End If
                For x = 0 To ds.Tables("Rooms").Rows.Count - 1
                    dRow = ds.Tables("Rooms").Rows(x)
                    If (dRow("UsageID") > 0) Then
                        Throw New Exception("One or more rooms are unavailable. Please requery and select another room.")
                    Else
                        dRow("UsageID") = usageID
                    End If
                Next
                oCombo = Nothing
                oRooms = Nothing
            End If
            Dim sc As New SqlCommandBuilder(da)
            da.Update(ds, "Rooms")
            Dim oRoom As New clsRooms
            oRoom.RoomID = roomID
            oRoom.Load()
            Dim oEvent As New clsEvents
            oEvent.KeyField = "UsageID"
            oEvent.KeyValue = usageID
            oEvent.EventType = "Add"
            oEvent.NewValue = oRoom.RoomNumber
            oEvent.CreatedByID = _UserID
            oEvent.Create_Event()
            oEvent = Nothing
            oRoom = Nothing
            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            _Err = ex.Message
            'Throw New Exception(ex.Message)
            Return False
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Function
    Public Function Remove_Room(ByVal roomID As Integer, ByVal usageID As Integer, ByVal uInDate As Date, ByVal uOutDate As Date) As Boolean
        Try
            Dim dRow As DataRow
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RoomAllocationMatrix where roomid = '" & roomID & "' and UsageID = '" & usageID & "' and dateallocated between '" & uInDate & "' and '" & uOutDate & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet()
            da.Fill(ds, "Rooms")
            If ds.Tables("Rooms").Rows.Count > 0 Then
                Dim x As Integer
                For x = 0 To ds.Tables("Rooms").Rows.Count - 1
                    dRow = ds.Tables("Rooms").Rows(x)
                    dRow("UsageID") = 0
                Next x
            End If
            Dim sc As New SqlCommandBuilder(da)
            da.Update(ds, "Rooms")
            Dim oRoom As New clsRooms
            oRoom.RoomID = roomID
            oRoom.Load()
            Dim oEvent As New clsEvents
            oEvent.KeyField = "UsageID"
            oEvent.KeyValue = usageID
            oEvent.EventType = "Remove"
            oEvent.NewValue = oRoom.RoomNumber
            oEvent.CreatedByID = _UserID
            oEvent.Create_Event()
            oEvent = Nothing
            oRoom = Nothing
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.Message
            Return False
        End Try
    End Function
    Public Function List_Contracts(ByVal field As String, ByVal ID As Integer) As SqlDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If field = "Contract" Then
                ds.SelectCommand = "Select Distinct(c.ContractID), c.ContractNumber from t_Contract uc inner join t_Contract c on uc.ProspectID = c.ProspectID where uc.ContractID = '" & ID & "' order by c.contractnumber"
            ElseIf field = "Usage" Then
                ds.SelectCommand = "Select Distinct(c.ContractID), c.ContractNumber from t_Usage u inner join t_Contract uc on u.contractid = uc.contractid inner join t_Contract c on uc.ProspectID = c.ProspectID where u.UsageID = '" & ID & "' order by c.contractnumber"
            Else
                ds.SelectCommand = "Select Distinct(c.ContractID), c.ContractNumber from t_Contract c where c.ProspectID = '" & ID & "' order by c.contractnumber"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Room_Count(ByVal usageID As Integer, ByVal dateChk As Boolean, ByVal outDate As Date) As Boolean
        Dim bRooms As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If dateChk Then
                cm.CommandText = "Select Count(Distinct(RoomID)) as Rooms from t_RoomAllocationMatrix where usageID = '" & usageID & "' and dateAllocated >= '" & outDate & "' and ReservationID > 0"
            Else
                cm.CommandText = "Select Count(Distinct(RoomID)) as Rooms from t_RoomAllocationMatrix where usageID = '" & usageID & "'"
            End If
            dread = cm.ExecuteReader()
            dread.Read()
            If dread("Rooms") > 0 Then
                bRooms = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bRooms
    End Function
    Public Function Get_Owner(ByVal sort As String, ByVal ID As Integer) As String
        Dim owner As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If sort = "contract" Then
                cm.CommandText = "Select p.LastName + ', ' + p.FirstName as Prospect from t_Contract c inner join t_Prospect p on c.prospectid = p.prospectid where c.contractid = '" & ID & "'"
            ElseIf sort = "usage" Then
                cm.CommandText = "Select p.LastName + ', ' + p.Firstname as Prospect from t_Usage u inner join t_Contract c on u.contractid = c.contractid inner join t_Prospect p on c.prospectid = p.prospectid where u.usageid = '" & ID & "'"
            End If
            dread = cm.ExecuteReader
            dread.Read()
            owner = dread("Prospect")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return owner
    End Function
    Public Function Get_Usage_Reservations(ByVal usageID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(rmx.ReservationID) As ResID, r.CheckInDate, r.CheckOutDate, p.FirstName + ' ' + p.LastName As Prospect from t_RoomAllocationmatrix rmx inner join t_Reservations r on rmx.ReservationID = r.ReservationID inner join t_Prospect p on r.ProspectID = p.ProspectID where rmx.UsageiD = '" & usageID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Get_Categories(ByVal roomID As Integer, ByVal inDate As Date, ByVal outDate As Date) As String
        Dim cat As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(ComboItem) As Category from (Select b.CategoryID from t_RoomAllocationMatrix a inner join t_Usage b on a.usageid = b.usageid where a.roomid = '" & roomID & "' and DateAllocated between '" & inDate & "' and '" & outDate & "') c left outer join t_ComboItems d on c.CategoryID = d.ComboItemID"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If Not (IsDBNull(dread("Category"))) Then
                        If cat = "" Then
                            cat = "Category " & dread("Category")
                        Else
                            cat = cat & "$Category " & dread("Category")
                        End If
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return cat
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Add_BulkBank_Room(ByVal usageA As Integer, ByVal usageB As Integer, ByVal usageC As Integer, ByVal sDate As Date, ByVal eDate As Date, ByVal BD As String, ByVal unitType As String, ByVal unitTypeID As Integer, ByVal usageTypeID As Integer, ByVal RoomSubTypeID As Integer) As Boolean
        Dim bAdded As Boolean = False
        Dim bAvail As Boolean = True
        Dim allocatedNumber As Integer = 0
        Dim oCombo As New clsComboItems
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim i As Integer = 0
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        If BD = "3" Or (BD = "4" And unitType = "Townes") Then
            cm.CommandText = "Select a.RoomID as Room1, b.RoomID as Room2 from t_Room a inner join t_Room b on a.LockoutID = b.RoomID inner join t_Unit c on a.UnitID = c.UnitID where a.TypeID = " & oCombo.Lookup_ID("RoomType", "2BD") & " and a.SubTypeID = " & RoomSubTypeID & " and c.TypeID = " & unitTypeID & ""
            allocatedNumber = 14
        ElseIf BD = "4" And unitType = "Estates" Then
            cm.CommandText = "Select a.RoomID as Room1, b.RoomID as Room2, c.RoomID as Room3 from t_Room a inner join t_Room b on a.LockoutID = b.RoomID inner join t_Unit d on a.UnitID = d.UnitID inner join t_Unit2Unit e on d.UnitID = e.UnitID and b.UnitID <> e.Unit2ID inner join t_Room c on e.Unit2ID = c.UnitID where a.TypeID = " & oCombo.Lookup_ID("RoomType", "2BD") & " and a.SubTypeID = " & RoomSubTypeID & " and d.TypeID = " & unitTypeID & ""
            allocatedNumber = 21
        Else
            Dim rmTypeID As Integer = 0
            If BD = "1" Then
                rmTypeID = oCombo.Lookup_ID("RoomType", "1BD")
            ElseIf BD = "2" Then
                rmTypeID = oCombo.Lookup_ID("RoomType", "2BD")
            Else
                rmTypeID = oCombo.Lookup_ID("RoomType", BD)
            End If
            cm.CommandText = "Select a.RoomID As Room1 from t_Room a inner join t_Unit b on a.UnitID = b.UnitID where a.TypeID = " & rmTypeID & " and a.SubTypeID = " & RoomSubTypeID & " and b.TypeID = " & unitTypeID & ""
            allocatedNumber = 7
        End If
        da.Fill(ds, "0")
        If ds.Tables("0").Rows.Count > 0 Then
            For i = 0 To ds.Tables("0").Rows.Count - 1
                If BD = "3" Or (BD = "4" And unitType = "Townes") Then
                    cm.CommandText = "Select Case when count(*) is null then 0 else count(*) end as Avail from t_RoomAllocationmatrix where (roomid = '" & ds.Tables("0").Rows(i).Item("Room1") & "' or roomid = '" & ds.Tables("0").Rows(i).Item("Room2") & "') And DateAllocated between '" & sDate & "' and '" & eDate & "' and TypeID = " & usageTypeID & " and UsageID = 0"
                ElseIf BD = "4" And unitType = "Estates" Then
                    cm.CommandText = "Select Case when count(*) is null then 0 else count(*) end as Avail from t_RoomAllocationmatrix where (roomid = '" & ds.Tables("0").Rows(i).Item("Room1") & "' or roomid = '" & ds.Tables("0").Rows(i).Item("Room2") & "' or roomid = '" & ds.Tables("0").Rows(i).Item("Room3") & "') And DateAllocated between '" & sDate & "' and '" & eDate & "' and TypeID = " & usageTypeID & " and UsageID = 0"
                Else
                    cm.CommandText = "Select Case when count(*) is null then 0 else count(*) end as Avail from t_RoomAllocationmatrix where (roomid = '" & ds.Tables("0").Rows(i).Item("Room1") & "') And DateAllocated between '" & sDate & "' and '" & eDate & "' and TypeID = " & usageTypeID & " and UsageID = 0"
                End If
                da.Fill(ds, "1")
                If ds.Tables("1").Rows.Count > 0 Then
                    If ds.Tables("1").Rows(0).Item("Avail") <> allocatedNumber Then
                        bAvail = False
                    Else
                        bAvail = True
                    End If
                Else
                    bAdded = False
                End If
                ds.Tables("1").Clear()
                If bAvail Then
                    bAdded = True
                    cm.CommandText = "Update t_RoomAllocationMatrix set UsageID = " & usageA & " where roomid = '" & ds.Tables("0").Rows(i).Item("Room1") & "' and dateallocated between '" & sDate & "' and '" & eDate & "'"
                    cm.ExecuteNonQuery()
                    If BD = "3" Or (BD = "4" And unitType = "Townes") Then
                        cm.CommandText = "Update t_RoomAllocationMatrix set UsageID = " & usageA & " where roomid = '" & ds.Tables("0").Rows(i).Item("Room2") & "' and dateallocated between '" & sDate & "' and '" & eDate & "'"
                        cm.ExecuteNonQuery()
                    ElseIf BD = "4" And unitType = "Estates" Then
                        cm.CommandText = "Update t_RoomAllocationMatrix set UsageID = " & usageA & " where roomid = '" & ds.Tables("0").Rows(i).Item("Room2") & "' and dateallocated between '" & sDate & "' and '" & eDate & "'"
                        cm.ExecuteNonQuery()
                        cm.CommandText = "Update t_RoomAllocationMatrix set UsageID = " & usageA & " where roomid = '" & ds.Tables("0").Rows(i).Item("Room3") & "' and dateallocated between '" & sDate & "' and '" & eDate & "'"
                        cm.ExecuteNonQuery()
                    End If
                    Exit For
                End If
            Next
        Else
            bAdded = False
        End If
        da.Dispose()
        'Catch ex As Exception
        '_Err = ex.Message
        'bAdded = False
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bAdded
    End Function
    Public Function Print_Conf_Letter(ByVal usageID As Integer) As String
        Dim letter As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim oUsage As New clsUsage
            Dim oCombo As New clsComboItems
            Dim ufFilter As String = ""
            oUsage.UsageID = usageID
            oUsage.Load()
            If oCombo.Lookup_ComboItem(oUsage.SubTypeID) = "II" Then
                ufFilter = "II Membership Number"
            ElseIf oCombo.Lookup_ComboItem(oUsage.SubTypeID) = "RCI" Then
                ufFilter = "RCI Membership Number"
            Else
                ufFilter = "ICE Membership Number"
            End If
            cm.CommandText = "Select xx.*, pa.Address1, pa.Address2, pa.City, pa.PostalCode, st.ComboItem as State from (select p.FirstName + ' ' + p.LastName as ProspectName, (Select Top 1 AddressID from t_ProspectAddress where prospectID = p.ProspectID and ActiveFlag = 1 and ContractAddress = 1) as AddressID, ufv.UFValue, u.UsageID, rt.ComboItem as RoomType, u.DateCreated " & _
                                   "from t_Prospect p " & _
                                   "left outer join (select UFValue, KeyValue from t_UF_Value uv " & _
                                    "left outer join t_UFields uf on uf.UFID = uv.UFID " & _
                                    "where uf.UFName = '" & ufFilter & "') as ufv on ufv.KeyValue = p.ProspectID " & _
                                   "left outer join t_Contract c on c.ProspectID = p.ProspectID " & _
                                   "left outer join t_Usage u on u.ContractID = c.ContractID " & _
                                   "left outer join t_ComboItems rt on rt.ComboItemID = u.RoomTypeID " & _
                                   "left outer join t_ComboItems ust on ust.ComboItemID = u.SubTypeID " & _
                                   "where u.UsageID in (" & usageID & ")) xx " & _
                                   "left outer join t_ProspectAddress pa on xx.AddressID = pa.AddressID " & _
                                   "left outer join t_ComboItems st on pa.StateID = st.ComboItemID"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                letter = "<center><img src = 'https://secure.kingscreekplantation.com/crmsnet/images/kcp_logo.bmp' /></center>"
                letter = letter & "<br>"
                letter = letter & "<br>"
                letter = letter & "<table colspan='360'>"
                letter = letter & "<tr colspan='360'>"
                letter = letter & "<td>"
                letter = letter & "<font size='5'>"
                letter = letter & "<b>"
                letter = letter & dread("ProspectName")
                letter = letter & "</b>"
                letter = letter & "</font>"
                letter = letter & "</td>"
                letter = letter & "<td colspan='150'>&nbsp;</td>"
                letter = letter & "<td align='right'><b>" & System.DateTime.Now.ToShortDateString & "</b></td>"
                letter = letter & "</tr>"
                letter = letter & "<tr>"
                letter = letter & "<td>"
                letter = letter & "<font size='4'><b>"
                letter = letter & dread("Address1")
                letter = letter & "</font></b>"
                letter = letter & "</td>"
                letter = letter & "</tr>"
                letter = letter & "<tr>"
                letter = letter & "<td>"
                letter = letter & "<font size='4'><b>"
                letter = letter & dread("Address2")
                letter = letter & "</font></b>"
                letter = letter & "</td>"
                letter = letter & "</tr>"
                letter = letter & "<tr>"
                letter = letter & "<td>"
                letter = letter & "<font size='4'><b>"
                letter = letter & dread("City") & ",&nbsp;" & dread("state") & "&nbsp;" & dread("postalcode")
                letter = letter & "</font></b>"
                letter = letter & "</td>"
                letter = letter & "</tr>"
                letter = letter & "</table>"
                letter = letter & "<br><br>"
                If ufFilter = "ICE Membership Number" Then
                    letter = letter & "<center>"
                    letter = letter & "<table>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b>ICE Deposit Confirmation: (for KCP purposes):</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("UsageID") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b>ICE Exchange Number:</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("UFValue") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b>Room Type:</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("RoomType") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "</table>"
                    letter = letter & "</center>"
                    letter = letter & "<p>"
                    letter = letter & "<font size='3'>"
                    letter = letter & "<b>The unit information above has been deposited in your name with ICE and may be used for exchange purposes only.</b><br><br>"
                    letter = letter & "Call ICE at 1-888-320-4234 when you are ready for them to start a search for you.<br><br>"
                    letter = letter & "You have two(2) years from the start date of your deposited week to travel.<br><br>"
                    letter = letter & "Remember... the sooner you make your vacation exchange request, the more opportunity you will have to find a satisfactory match.<br><br>"
                    letter = letter & "REMINDER; All owner obligations, including mortgage accounts, <b><u>must</u></b> be kept current in order to maintain your usage status.<br><br>"
                    letter = letter & "Pursuant to the Public Offering Statement, failure to do so will result in cancellation of your reservation, rental and/or exchange deposits.<br><br><br>"
                    letter = letter & "<b>If you have any questions regarding this aboe ICE deposit, please contact your Owner Services Team at 1-866-228-6796.</b>"
                    letter = letter & "</font>"
                    letter = letter & "</p>"
                ElseIf ufFilter = "RCI Membership Number" Then
                    letter = letter & "<center>"
                    letter = letter & "<table>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b><u>RCI Deposit Confirmation: (for KCP purposes):</u></b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("UsageID") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b><u>RCI Exchange Number:</u></b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("UFValue") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b><u>Room Type:</u></b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("RoomType") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "</table>"
                    letter = letter & "</center>"
                    letter = letter & "<br><br>"
                    letter = letter & "<p>"
                    letter = letter & "<font size='3'>"
                    letter = letter & "<b>The unit information above has been deposited in your name with RCI and may be used for exchange purposes only.</b><br><br>"
                    letter = letter & "Call RCI at 1-800-221-6400 when you are ready for them to start a search for you.<br><br>"
                    letter = letter & "You have two(2) years from the start date of your deposited week to travel.<br><br>"
                    letter = letter & "Remember... the sooner you make your vacation exchange request, the more opportunity you will have to find a satisfactory match.<br><br>"
                    letter = letter & "REMINDER; All owner obligations, including mortgage accounts, <b><u>must</u></b> be kept current in order to maintain your usage status.<br><br>"
                    letter = letter & "Pursuant to the Public Offering Statement, failure to do so will result in cancellation of your reservation, rental and/or exchange deposits.<br><br><br>"
                    letter = letter & "<b>If you have any questions regarding the above RCI deposit, please contact your Owner Services Team at 1-866-228-6796</b>"
                    letter = letter & "</font>"
                    letter = letter & "</p>"
                Else
                    letter = letter & "<center>"
                    letter = letter & "<table>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b>II Deposit Confirmation: (for KCP purposes):</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("UsageID") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b>II Exchange Number:</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("UFValue") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='4'><b>Room Type:</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "<tr>"
                    letter = letter & "<td align='center'><font size='3'><b>" & dread("RoomType") & "</b></font></td>"
                    letter = letter & "</tr>"
                    letter = letter & "</table>"
                    letter = letter & "</center>"
                    letter = letter & "<br><br>"
                    letter = letter & "<p>"
                    letter = letter & "<b>The unit information has been deposited in your name with II and may be used for exchange purposes only.</b><br><br>"
                    letter = letter & "Call II at 1-877-ONTO KCP(1-877-668-6527) when you are ready for them to start a search for you.<br><br>"
                    letter = letter & "You have two(2) years before and after the start date of your deposited week to travel.<br><br>"
                    letter = letter & "Remember... the sooner you make your vacation exchange request, the more opportunity you will have to find a satisfactory match.<br><br>"
                    letter = letter & "REMINDER; All owner obligations, including mortgage accounts, <b><u>must</u></b> be kept current in order to maintain your usage status.<br><br>"
                    letter = letter & "Pursuant to the Public Offering Statement, failure to do so will result in cancellation of your reservation, rental and/or exchange deposits.<br><br><br>"
                    letter = letter & "<b>If you have any questions regarding your II deposit, please contact your Owner Services Team at 1-866-228-6796.</b>"
                    letter = letter & "</p>"
                End If
            Else
                letter = "There are no records for this ID"
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            letter = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return letter
    End Function

    Public Function Usage_BD_Count(ByVal roomID As Integer, ByVal inDate As DateTime, ByVal outDate As DateTime) As Integer
        Dim bd As Integer = 0
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select top 1 LEFT(rt.ComboItem, 1) as MAXBD from t_RoomAllocationMatrix rm inner join t_Usage u on rm.UsageID = u.UsageID inner join t_ComboItems rt on u.RoomTypeID = rt.ComboItemID where rm.DateAllocated between '" & inDate & "' and '" & outDate & "' and rm.RoomID = " & roomID & " order by rt.ComboItem desc"
        dread = cm.ExecuteReader
        If dread.HasRows Then
            dread.Read()
            bd = dread("MAXBD")
        End If
        dread.Close()
        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bd
    End Function

    Public Function Find_UsageID_By_Contract_Year(ByVal contractid As Integer, ByVal year As Integer) As Integer
        Dim ret As Integer = 0
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "select * from t_Usage where typeid = " & (New clsComboItems).Lookup_ID("ReservationType", "PointTracking") & " and contractid = " & contractid & " and usageyear=" & year
        dread = cm.ExecuteReader
        If dread.HasRows Then
            dread.Read()
            ret = dread("UsageID")
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return ret
    End Function


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

    Public Property CategoryID() As Integer
        Get
            Return _CategoryID
        End Get
        Set(ByVal value As Integer)
            _CategoryID = value
        End Set
    End Property

    Public Property AmountPromised() As Decimal
        Get
            Return _AmountPromised
        End Get
        Set(ByVal value As Decimal)
            _AmountPromised = value
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

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
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

    Public Property RoomTypeID() As Integer
        Get
            Return _RoomTypeID
        End Get
        Set(ByVal value As Integer)
            _RoomTypeID = value
        End Set
    End Property

    Public Property Days() As Integer
        Get
            Return _Days
        End Get
        Set(ByVal value As Integer)
            _Days = value
        End Set
    End Property

    Public Property Points() As Integer
        Get
            Return _Points
        End Get
        Set(ByVal value As Integer)
            _Points = value
        End Set
    End Property

    Public Property SoldInventoryID() As Integer
        Get
            Return _SoldInventoryID
        End Get
        Set(ByVal value As Integer)
            _SoldInventoryID = value
        End Set
    End Property

    Public Property InDate() As String
        Get
            Return _InDate
        End Get
        Set(ByVal value As String)
            _InDate = value
        End Set
    End Property

    Public Property OutDate() As String
        Get
            Return _OutDate
        End Get
        Set(ByVal value As String)
            _OutDate = value
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

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
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
End Class
