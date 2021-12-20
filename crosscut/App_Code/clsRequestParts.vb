Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRequestParts
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ItemNumber As String = ""
    Dim _RequestID As Integer = 0
    Dim _Qty As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _DateInstalled As String = ""
    Dim _InstalledById As Integer = 0
    Dim _PRID As Integer = 0
    Dim _GPImported As Boolean = False
    Dim _GPImportedDate As String = ""
    Dim _GPError As Boolean = False
    Dim _GPErrorDesc As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RequestParts where Part2RequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RequestParts where Part2RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RequestParts")
            If ds.Tables("t_RequestParts").Rows.Count > 0 Then
                dr = ds.Tables("t_RequestParts").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ItemNumber") Is System.DBNull.Value) Then _ItemNumber = dr("ItemNumber")
        If Not (dr("RequestID") Is System.DBNull.Value) Then _RequestID = dr("RequestID")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("DateInstalled") Is System.DBNull.Value) Then _DateInstalled = dr("DateInstalled")
        If Not (dr("InstalledById") Is System.DBNull.Value) Then _InstalledById = dr("InstalledById")
        If Not (dr("PRID") Is System.DBNull.Value) Then _PRID = dr("PRID")
        If Not (dr("GPImported") Is System.DBNull.Value) Then _GPImported = dr("GPImported")
        If Not (dr("GPImportedDate") Is System.DBNull.Value) Then _GPImportedDate = dr("GPImportedDate")
        If Not (dr("GPError") Is System.DBNull.Value) Then _GPError = dr("GPError")
        If Not (dr("GPErrorDesc") Is System.DBNull.Value) Then _GPErrorDesc = dr("GPErrorDesc")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select * from t_RequestParts where Part2RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RequestParts")
            If ds.Tables("t_RequestParts").Rows.Count > 0 Then
                dr = ds.Tables("t_RequestParts").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RequestPartsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ItemNumber", SqlDbType.VarChar, 0, "ItemNumber")
                da.InsertCommand.Parameters.Add("@RequestID", SqlDbType.Int, 0, "RequestID")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.Int, 0, "Qty")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@DateInstalled", SqlDbType.DateTime, 0, "DateInstalled")
                da.InsertCommand.Parameters.Add("@InstalledById", SqlDbType.Int, 0, "InstalledById")
                da.InsertCommand.Parameters.Add("@PRID", SqlDbType.Int, 0, "PRID")
                da.InsertCommand.Parameters.Add("@GPImported", SqlDbType.Bit, 0, "GPImported")
                da.InsertCommand.Parameters.Add("@GPImportedDate", SqlDbType.DateTime, 0, "GPImportedDate")
                da.InsertCommand.Parameters.Add("@GPError", SqlDbType.Bit, 0, "GPError")
                da.InsertCommand.Parameters.Add("@GPErrorDesc", SqlDbType.Text, 0, "GPErrorDesc")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Part2RequestID", SqlDbType.Int, 0, "Part2RequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RequestParts").NewRow
            End If
            Update_Field("ItemNumber", _ItemNumber, dr)
            Update_Field("RequestID", _RequestID, dr)
            Update_Field("Qty", _Qty, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("DateInstalled", _DateInstalled, dr)
            Update_Field("InstalledById", _InstalledById, dr)
            Update_Field("PRID", _PRID, dr)
            Update_Field("GPImported", _GPImported, dr)
            Update_Field("GPImportedDate", _GPImportedDate, dr)
            Update_Field("GPError", _GPError, dr)
            Update_Field("GPErrorDesc", _GPErrorDesc, dr)
            If ds.Tables("t_RequestParts").Rows.Count < 1 Then ds.Tables("t_RequestParts").Rows.Add(dr)
            da.Update(ds, "t_RequestParts")
            _ID = ds.Tables("t_RequestParts").Rows(0).Item("Part2RequestID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "Part2RequestID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Parts(ByVal requestID As Integer) As DataTable
        Dim dt As New DataTable
        Dim row As DataRow
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select c.Part2RequestID, c.ItemNumber, c.ITEMDESC, c.Qty, d.ComboItem as PartStatus, c.PRID from (Select a.Part2RequestID, a.ItemNumber, b.ITEMDESC, a.Qty, a.StatusID, a.PRID from t_RequestParts a inner join t_IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.RequestID = '" & requestID & "') c inner join t_ComboItems d on c.statusid = d.comboitemid order by part2requestid asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dt.Columns.Add("Part2RequestID")
                dt.Columns.Add("ItemNumber")
                dt.Columns.Add("Description")
                dt.Columns.Add("Qty")
                dt.Columns.Add("Status")
                dt.Columns.Add("PRID")
                dt.Columns.Add("Remove")
                dt.Columns.Add("Install")
            End If
            Do While dread.Read()
                row = dt.NewRow
                row("Part2RequestID") = dread("Part2RequestID")
                row("ItemNumber") = dread("ItemNumber")
                row("Description") = dread("ITEMDESC")
                row("Qty") = dread("Qty")
                row("Status") = dread("PartStatus")
                row("PRID") = dread("PRID")
                If dread("PartStatus") = "Assigned" Or dread("PartStatus") = "Requested" Then
                    row("Remove") = "TRUE"
                Else
                    row("Remove") = "FALSE"
                End If
                If dread("PartStatus") = "Assigned" Then
                    row("Install") = "TRUE"
                Else
                    row("Install") = "FALSE"
                End If
                dt.Rows.Add(row)
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function
    Public Function Get_MinOrder_Amt(ByVal item As String) As Integer
        Dim minOrderAmt As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when MNMMORDRQTY is null then 0 else MNMMORDRQTY end from t_IV00102 where RTrim(ITEMNMBR) = '" & RTrim(item) & "' and RTRIM(LOCNCODE) = 'WAREHOUSE'"
            dread = cm.ExecuteReader
            dread.Read()
            minOrderAmt = dread("MNMMORDRQTY")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return minOrderAmt
    End Function
    Public Function Validate_GP_Part(ByVal item As String) As Integer
        Dim parts As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Parts from t_IV00101 where RTrim(ITEMNMBR) = '" & RTrim(item) & "'"
            dread = cm.ExecuteReader
            dread.Read()
            parts = dread("Parts")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return parts
    End Function
    Public Function Get_ItemOrder_Amt(ByVal item As String) As Integer
        Dim itemOrderAmt As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when CURRCOST is null then 0 else CURRCOST end from t_IV00101 where RTrim(ITEMNMBR) = '" & RTrim(item) & "'"
            dread = cm.ExecuteReader
            dread.Read()
            itemOrderAmt = dread("CURRCOST")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return itemOrderAmt
    End Function
    Public Function Get_Installed_Count(ByVal item As String, ByVal gpImported As Integer) As Integer
        Dim partsInstalled As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Sum(rp.Qty) is Null then 0 Else Sum(rp.Qty) end as Parts from t_RequestParts rp inner join t_comboItems ps on rp.StatusID = ps.ComboItemID where rp.ItemNumber = '" & item & "' and rp.GPImported = " & gpImported & " and ps.ComboItem = 'Installed'"
            dread = cm.ExecuteReader
            dread.Read()
            partsInstalled = dread("Parts")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return partsInstalled
    End Function

    Public Function Get_GP_Count(ByVal item As String) As Integer
        Dim partsInstalled As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Sum(QTYONHND) is Null then 0 Else Sum(QTYONHND) end as Parts from t_IV00102 where RTrim(ITEMNMBR) = '" & RTrim(item) & "' and RTRIM(LOCNCODE) = 'WAREHOUSE'"
            dread = cm.ExecuteReader
            dread.Read()
            partsInstalled = dread("Parts")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return partsInstalled
    End Function

    Public Function Get_GP_Par(ByVal item As String) As Integer
        Dim partsInstalled As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Sum(SFTYSTCKQTY) is Null then 0 Else Sum(SFTYSTCKQTY) end as Parts from t_IV00102 where RTrim(ITEMNMBR) = '" & RTrim(item) & "' and RTRIM(LOCNCODE) = 'WAREHOUSE'"
            dread = cm.ExecuteReader
            dread.Read()
            partsInstalled = dread("Parts")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return partsInstalled
    End Function
    Public Function Get_Assigned_Count(ByVal item As String) As Integer
        Dim partsInstalled As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Sum(rp.Qty) is Null then 0 Else Sum(rp.Qty) end as Parts from t_RequestParts rp inner join t_comboItems ps on rp.StatusID = ps.ComboItemID where rp.ItemNumber = '" & item & "' and ps.ComboItem = 'Assigned'"
            dread = cm.ExecuteReader
            dread.Read()
            partsInstalled = dread("Parts")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return partsInstalled
    End Function
    Public Function Remove_Part(ByVal ID As Integer) As Boolean
        Dim removed As Boolean = True
        Dim opRequest As New clsPurchaseRequest
        Dim oRequestPart As New clsRequestParts
        Dim opRequestItems As New clsPurchaseRequestItems
        Dim oCombo As New clsComboItems
        oRequestPart.Part2RequestID = ID
        oRequestPart.Load()
        Dim availOnHand As Integer = 0
        Dim totalOrderAmt As Integer = 0
        Dim totalAvail As Integer = 0
        Try
            cn.ConnectionString = Resources.Resource.cns
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select g.* from (SELECT c.*, (SELECT COUNT(*) FROM t_RequestParts WHERE PRID = c.PurchaseRequestID AND requestID <> '" & oRequestPart.RequestID & "' AND itemNumber = '" & oRequestPart.ItemNumber & "' AND StatusID = (SELECT comboitemid FROM t_ComboItems ci inner join t_Combos c on ci.comboid = c.comboid WHERE c.comboname = 'PartStatus' AND comboitem = 'Requested')) AS OtherRequests, (SELECT COUNT(*) FROM t_RequestParts WHERE requestid = '" & oRequestPart.RequestID & "' AND itemnumber = '" & oRequestPart.ItemNumber & "' AND PRID = c.PurchaseRequestID AND StatusID = (SELECT cid.comboitemid FROM t_ComboItems cid inner join t_Combos cmb on cid.comboid = cmb.comboid WHERE cmb.comboname = 'PartStatus' AND comboitem = 'Requested')) AS SelfRequest FROM (SELECT DISTINCT (a.PurchaseRequestID), SUM(b.Qty) AS AmtRequested FROM t_PurchaseRequest a INNER JOIN t_PurchaseRequestItems b ON a.PurchaseRequestID = b.PurchaseRequestID WHERE b.ItemNumber = '" & oRequestPart.ItemNumber & "' AND a.Approved = '0' GROUP BY a.PurchaseRequestID) c) g where g.OtherRequests = 0 and (g.SelfRequest = 1 or g.SelfRequest = 0) Order By g.SelfRequest, g.PurchaseRequestID desc"
            dread = cm.ExecuteReader
            Do While dread.Read
                availOnHand = (oRequestPart.Get_GP_Count(oRequestPart.ItemNumber) - oRequestPart.Get_Installed_Count(oRequestPart.ItemNumber, 0)) - oRequestPart.Get_Assigned_Count(oRequestPart.ItemNumber)
                totalOrderAmt = opRequest.Get_Ordered_Count(oRequestPart.ItemNumber) - opRequest.Get_Requested_Count(oRequestPart.ItemNumber, oRequestPart.RequestID)
                If oCombo.Lookup_ComboItem(oRequestPart.StatusID) = "Requested" Then
                    totalOrderAmt = totalOrderAmt + oRequestPart.Qty
                ElseIf oCombo.Lookup_ComboItem(oRequestPart.StatusID) = "Assigned" Then
                    availOnHand = availOnHand + 5
                End If
                totalAvail = availOnHand + totalOrderAmt

                '****** Subtract Qty being ordered in this PR from the availonHand *******' 
                '****** if this value is greated than the par, we can delete the PR Line Item ******'
                If (totalAvail - dread("AmtRequested")) >= oRequestPart.Get_GP_Par(oRequestPart.ItemNumber) Then
                    opRequestItems.Remove_Item(oRequestPart.ItemNumber, dread("PurchaseRequestID"))
                    '**** If there are no other items on PR, Deny It
                    If opRequest.Get_TotalItem_Count(dread("PurchaseRequestID")) = 0 Then
                        opRequest.PurchaseRequestID = dread("PurchaseRequestID")
                        opRequest.Load()
                        opRequest.Approved = "-1"
                        opRequest.Save()
                    End If
                    '***** Cancel all other requested parts tied to this work order and the same PR
                    oRequestPart.Cancel_PR_Parts(dread("PurchaseRequestID"), oRequestPart.ItemNumber, oRequestPart.Part2RequestID, oRequestPart.RequestID)
                Else
                    opRequest.PurchaseRequestID = dread("PurchaseRequestID")
                    opRequest.Load()
                    opRequest.Approved = 1 '"totalAvail " & totalAvail & " - " & dread("AmtRequested")
                    'opRequest.Save()
                End If
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            removed = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return removed
    End Function
    Public Function Cancel_PR_Parts(ByVal PRID As Integer, ByVal item As String, ByVal ID As Integer, ByVal requestID As Integer) As Boolean
        Dim cxlParts As Boolean = True
        Dim oCombo As New clsComboItems
        Try
            cn.ConnectionString = Resources.Resource.cns
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_RequestParts set PRID = 0, statusid = " & oCombo.Lookup_ID("PartStatus", "Cancelled") & " where  PRID = '" & PRID & "' and itemNumber = '" & ItemNumber & "' and Part2RequestID <> '" & ID & "' and requestID = '" & requestID & "' and statusID = '" & oCombo.Lookup_ID("PartStatus", "Requested") & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        oCombo = Nothing
        Return cxlParts
    End Function

    Public Function Get_ReqAssigned_Parts(ByVal reqID As Integer) As Integer
        Dim parts As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else count(*) end as Parts from t_Requestparts p inner join t_ComboItems s on p.StatusID = s.ComboItemID where p.requestID = " & reqID & " and (s.ComboItem = 'Requested' or s.ComboItem = 'Assigned')"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                parts = dread("Parts")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return parts
    End Function

    Public Function Get_ReqInstalled_Parts(ByVal reqID As Integer) As Integer
        Dim parts As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else count(*) end as Parts from t_Requestparts p inner join t_ComboItems s on p.StatusID = s.ComboItemID where p.requestID = " & reqID & " and (s.ComboItem = 'Installed')"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                parts = dread("Parts")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return parts
    End Function

    Public Function Get_Parts_By_PR(ByVal PRID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ID")
        Dim dRow As DataRow
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select rp.Part2RequestID as ID from t_RequestParts rp inner join t_CombOitems rs on rp.StatusID = rs.ComboitemID where rp.PRID = " & PRID & " and rs.ComboItem = 'Requested'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read()
                    dRow = dt.NewRow
                    dRow("ID") = dread("ID")
                    dt.Rows.Add(dRow)
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property ItemNumber() As String
        Get
            Return _ItemNumber
        End Get
        Set(ByVal value As String)
            _ItemNumber = value
        End Set
    End Property

    Public Property RequestID() As Integer
        Get
            Return _RequestID
        End Get
        Set(ByVal value As Integer)
            _RequestID = value
        End Set
    End Property

    Public Property Qty() As Integer
        Get
            Return _Qty
        End Get
        Set(ByVal value As Integer)
            _Qty = value
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

    Public Property DateInstalled() As String
        Get
            Return _DateInstalled
        End Get
        Set(ByVal value As String)
            _DateInstalled = value
        End Set
    End Property

    Public Property InstalledById() As Integer
        Get
            Return _InstalledById
        End Get
        Set(ByVal value As Integer)
            _InstalledById = value
        End Set
    End Property

    Public Property PRID() As Integer
        Get
            Return _PRID
        End Get
        Set(ByVal value As Integer)
            _PRID = value
        End Set
    End Property

    Public Property GPImported() As Boolean
        Get
            Return _GPImported
        End Get
        Set(ByVal value As Boolean)
            _GPImported = value
        End Set
    End Property

    Public Property GPImportedDate() As String
        Get
            Return _GPImportedDate
        End Get
        Set(ByVal value As String)
            _GPImportedDate = value
        End Set
    End Property

    Public Property GPError() As Boolean
        Get
            Return _GPError
        End Get
        Set(ByVal value As Boolean)
            _GPError = value
        End Set
    End Property

    Public Property GPErrorDesc() As String
        Get
            Return _GPErrorDesc
        End Get
        Set(ByVal value As String)
            _GPErrorDesc = value
        End Set
    End Property

    Public Property Part2RequestID() As Integer
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

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
End Class
