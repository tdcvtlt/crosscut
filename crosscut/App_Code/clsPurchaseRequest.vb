Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPurchaseRequest
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Approved As String = ""
    Dim _POID As String = ""
    Dim _Received As Boolean = False
    Dim _VendorID As String = ""
    Dim _VendorDescription As String = ""
    Dim _IsManual As Integer = 0
    Dim _DateCreated As String = ""
    Dim _CreatedById As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PurchaseRequest where PurchaseRequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PurchaseRequest where PurchaseRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PurchaseRequest")
            If ds.Tables("t_PurchaseRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_PurchaseRequest").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Approved") Is System.DBNull.Value) Then _Approved = dr("Approved")
        If Not (dr("POID") Is System.DBNull.Value) Then _POID = dr("POID")
        If Not (dr("Received") Is System.DBNull.Value) Then _Received = dr("Received")
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
        If Not (dr("VendorDescription") Is System.DBNull.Value) Then _VendorDescription = dr("VendorDescription")
        If Not (dr("IsManual") Is System.DBNull.Value) Then _IsManual = dr("IsManual")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedById") Is System.DBNull.Value) Then _CreatedById = dr("CreatedById")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select * from t_PurchaseRequest where PurchaseRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PurchaseRequest")
            If ds.Tables("t_PurchaseRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_PurchaseRequest").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PurchaseRequestInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Approved", SqlDbType.varchar, 0, "Approved")
                da.InsertCommand.Parameters.Add("@POID", SqlDbType.varchar, 0, "POID")
                da.InsertCommand.Parameters.Add("@Received", SqlDbType.bit, 0, "Received")
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.varchar, 0, "VendorID")
                da.InsertCommand.Parameters.Add("@VendorDescription", SqlDbType.varchar, 0, "VendorDescription")
                da.InsertCommand.Parameters.Add("@IsManual", SqlDbType.int, 0, "IsManual")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedById", SqlDbType.int, 0, "CreatedById")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PurchaseRequestID", SqlDbType.Int, 0, "PurchaseRequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PurchaseRequest").NewRow
            End If
            Update_Field("Approved", _Approved, dr)
            Update_Field("POID", _POID, dr)
            Update_Field("Received", _Received, dr)
            Update_Field("VendorID", _VendorID, dr)
            Update_Field("VendorDescription", _VendorDescription, dr)
            Update_Field("IsManual", _IsManual, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedById", _CreatedById, dr)
            If ds.Tables("t_PurchaseRequest").Rows.Count < 1 Then ds.Tables("t_PurchaseRequest").Rows.Add(dr)
            da.Update(ds, "t_PurchaseRequest")
            _ID = ds.Tables("t_PurchaseRequest").Rows(0).Item("PurchaseRequestID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Function
    Public Function PR_Part_Search(ByVal item As String, ByVal qty As Integer, ByVal onHand As Integer, ByVal requestID As Integer) As Integer
        Dim totalQty As Integer = qty
        Dim qtyLeft As Integer = qty - IIf(onHand > -1, onHand, 0)
        Dim totalQtyAssigned As Integer = 0
        Dim totalQtyRequested As Integer = 0
        Dim qtyUnassigned As Integer = 0
        Dim opRequest As New clsPurchaseRequest
        Dim opRequestItems As New clsPurchaseRequestItems
        Dim opartRequest As New clsRequestParts
        Dim oCombo As New clsComboItems
        totalQtyAssigned = qtyLeft
        Dim PRID As Integer = 0
        Dim minOrderAmt As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select a.PurchaseRequestID, Case when b.Qty is null then 0 else b.Qty end As QtyRequested, (Select case when Sum(Qty) is null then 0 else Sum(Qty) end from t_RequestParts rp inner join t_ComboItems ps on rp.StatusID = ps.ComboItemID where rp.ItemNumber = '" & item & "' and rp.PRID = a.PurchaseRequestID and ps.ComboItem in ('Assigned','Requested')) as QtyAssigned from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.purchaserequestid = b.purchaserequestid where b.ItemNumber = '" & item & "' and (Approved = 0 or Approved = 1) and Received = 0"
            dread = cm.ExecuteReader
            '***** Get a total count being ordered, and a total count that are actually requested *****'
            Do While dread.Read
                totalQtyRequested = totalQtyRequested + dread("QtyRequested")
                totalQtyAssigned = totalQtyAssigned + dread("QtyAssigned")
                If qtyLeft > 0 Then
                    If CDbl(dread("QtyRequested")) > CDbl(dread("QtyAssigned")) Then
                        qtyUnassigned = dread("QtyRequested") - dread("QtyAssigned")
                        opartRequest.Part2RequestID = 0
                        opartRequest.Load()
                        opartRequest.RequestID = requestID
                        opartRequest.ItemNumber = item
                        opartRequest.PRID = dread("PurchaseRequestID")
                        opartRequest.StatusID = oCombo.Lookup_ID("PartStatus", "Requested")
                        If qtyUnassigned >= qtyLeft Then
                            opartRequest.Qty = qtyLeft
                            totalQty = totalQty - qtyLeft
                            qtyLeft = 0
                        Else
                            opartRequest.Qty = qtyUnassigned
                            qtyLeft = qtyLeft - qtyUnassigned
                            totalQty = totalQty - qtyUnassigned
                        End If
                        opartRequest.Save()
                    End If
                End If
            Loop
            dread.Close()
            '**** If there is still a qty that needs to be ordered create a PR, ****'
            '**** but we have to look at min order amount first if min order amt > qtyleft ****'
            '**** we order the min order amt, else order the qty left *****'
            If qtyLeft > 0 Then
                minOrderAmt = opartRequest.Get_MinOrder_Amt(item)
                PRID = Get_Already_Ordered(item)
                If PRID = 0 Then
                    '***** Create PR *****'
                    opRequest.PurchaseRequestID = 0
                    opRequest.Load()
                    opRequest.Approved = 0
                    opRequest.POID = 0
                    opRequest.Save()
                    PRID = opRequest.PurchaseRequestID
                End If
                '**** Create Purchase RequestItem ******'
                opRequestItems.Item2RequestID = 0
                opRequestItems.Load()
                opRequestItems.PurchaseRequestID = PRID
                opRequestItems.ItemNumber = item
                opRequestItems.Amount = opartRequest.Get_ItemOrder_Amt(item)
                If minOrderAmt >= qtyLeft Then
                    opRequestItems.Qty = minOrderAmt
                    totalQtyRequested = totalQtyRequested + minOrderAmt
                Else
                    opRequestItems.Qty = qtyLeft
                    totalQtyRequested = totalQtyRequested + qtyLeft
                End If
                opRequestItems.Save()
                '*****Assign Parts to Request
                opartRequest.Part2RequestID = 0
                opartRequest.Load()
                opartRequest.RequestID = requestID
                opartRequest.ItemNumber = item
                opartRequest.PRID = PRID
                opartRequest.StatusID = oCombo.Lookup_ID("PartStatus", "Requested")
                opartRequest.Qty = qtyLeft
                opartRequest.Save()
                totalQty = totalQty - qtyLeft
            End If

            '******** Check to see if we fall under par ******'
            Dim totalLeftOver As Integer = totalQtyRequested - totalQtyAssigned
            Dim itemPar As Integer = opartRequest.Get_GP_Par(item)
            Dim underParAmt As Integer = 0
            If totalLeftOver < itemPar Then
                underParAmt = itemPar - totalLeftOver
                minOrderAmt = opartRequest.Get_MinOrder_Amt(item)
                PRID = Get_Already_Ordered(item)
                If PRID = 0 Then
                    opRequest.PurchaseRequestID = 0
                    opRequest.Load()
                    opRequest.Approved = 0
                    opRequest.POID = 0
                    opRequest.Save()
                    PRID = opRequest.PurchaseRequestID
                End If
                '**** Create Purchase RequestItem ******'
                opRequestItems.Item2RequestID = 0
                opRequestItems.Load()
                opRequestItems.PurchaseRequestID = PRID
                opRequestItems.ItemNumber = item
                opRequestItems.Amount = opartRequest.Get_ItemOrder_Amt(item)
                If underParAmt > minOrderAmt Then
                    opRequestItems.Qty = underParAmt
                Else
                    opRequestItems.Qty = minOrderAmt
                End If
                opRequestItems.Note = "AmtReq: " & totalQtyRequested & " AmtAssign: " & totalQtyAssigned & " LeftOver: " & totalLeftOver & " UnderParAmt: " & underParAmt & " MinOrderAmt: " & minOrderAmt
                opRequestItems.Save()
            End If
            '******** End Par Check *********'

            opRequest = Nothing
            opRequestItems = Nothing
            opartRequest = Nothing
            oCombo = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return totalQty
    End Function
    Private Function Get_Already_Ordered(ByVal item As String) As Integer
        Dim PRID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Top 1 pr.PurchaseRequestID from t_PurchaseRequest pr inner join t_PurchaseRequestITems pri on pr.PurchaseRequestID = pri.PurchaseRequestID where pri.ItemNumber = '" & item & "' and pr.Approved = 0 order by PurchaseRequestID desc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                PRID = dread("PurchaseRequestID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return PRID
    End Function
    Public Function List(ByVal status As String, Optional ByVal ID As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If status = "ALL" Then
                If ID = "" Then
                    ds.SelectCommand = "Select a.PurchaseRequestID As ID, (Select Case When Sum(Amount) is null then 0 else Sum(Amount) end from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount, a.Approved As Status from t_PurchaseRequest a order by PurchaseRequestID asc"
                Else
                    ds.SelectCommand = "Select a.PurchaseRequestID As ID, (Select Case When Sum(Amount) is null then 0 else Sum(Amount) end from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount, a.Approved As Status from t_PurchaseRequest a where PurchaseRequestID like '" & ID & "%' order by PurchaseRequestID asc"
                End If
            Else
                If ID = "" Then
                    ds.SelectCommand = "Select a.PurchaseRequestID As ID, (Select Case When Sum(Amount) is null then 0 else Sum(Amount) end from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount, a.Approved As Status from t_PurchaseRequest a where Approved = '" & status & "' order by PurchaseRequestID asc"
                Else
                    ds.SelectCommand = "Select a.PurchaseRequestID As ID, (Select Case When Sum(Amount) is null then 0 else Sum(Amount) end from t_PurchaseRequestItems where purchaseRequestID = a.PurchaseRequestID) As Amount, a.Approved As Status from t_PurchaseRequest a where Approved = '" & status & "' and PurchaseRequestID like '" & ID & "%' order by PurchaseRequestID asc"
                End If
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Get_Requested_Count_PR(ByVal itemNumber As String, ByVal PRID As Integer) As Integer
        Dim reqCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Sum(Qty) is null then 0 else Sum(Qty) end As QtyRequested from t_RequestParts where PRID = '" & PRID & "' and itemNumber = '" & itemNumber & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                reqCount = reqCount + dread("QtyRequested")
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return reqCount
    End Function
    Public Function Get_Requested_Count(ByVal itemNumber As String, ByVal requestID As Integer) As Integer
        Dim reqCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select a.PurchaseRequestID, (Select Case when Sum(Qty) is null then 0 else Sum(Qty) end from t_RequestParts where PRID = a.PurchaseRequestID and itemNumber = '" & itemNumber & "' and requestid <> '" & requestID & "') As QtyRequested from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.PurchaseRequestID = b.PurchaseRequestID where a.Approved = '0' and b.ItemNumber = '" & itemNumber & "' group By a.purchaseRequestID"
            dread = cm.ExecuteReader
            Do While dread.Read
                reqCount = reqCount + dread("QtyRequested")
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return reqCount
    End Function
    Public Function Get_Ordered_Count_PR(ByVal itemNumber As String, ByVal PRID As Integer, ByVal PRItemID As Integer) As Integer
        Dim reqCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Sum(b.Qty) is null then 0 else Sum(b.Qty) end As QtyOrdered from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.PurchaseRequestID = b.PurchaseRequestID where a.PurchaseRequestID = '" & PRID & "' and b.Item2RequestID <> '" & PRItemID & "' and b.ItemNumber = '" & itemNumber & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                reqCount = reqCount + dread("QtyOrdered")
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return reqCount
    End Function
    Public Function Get_Ordered_Count(ByVal itemNumber As String) As Integer
        Dim reqCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select a.PurchaseRequestID, Case when Sum(b.Qty) is null then 0 else Sum(b.Qty) end as QtyOrdered from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.PurchaseRequestID = b.PurchaseRequestID where a.Approved = '0' and b.ItemNumber = '" & itemNumber & "' group By a.purchaseRequestID"
            dread = cm.ExecuteReader
            Do While dread.Read
                reqCount = reqCount + dread("QtyOrdered")
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return reqCount
    End Function
    Public Function List_Vendors(ByVal filter As String) As SQLDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select Top 100 VENDORID, VENDNAME from t_PM00200 where VENDNAME LIKE '" & filter & "%' order by VENDNAME"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
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
            oEvents.KeyField = "PurchaseRequestID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Get_TotalItem_Count(ByVal reqID As Integer) As Integer
        Dim itemCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Items from t_PurchaseRequestItems where purchaseRequestID = '" & reqID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            itemCount = dread("Items")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return itemCount
    End Function
    Public Function Cancel_PR_Parts(ByVal reqID As Integer) As Boolean
        Dim bSuccess As Boolean = True
        Dim oCombo As New clsComboItems
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Update t_RequestParts set statusID = '" & oCombo.Lookup_ID("PartStatus", "Cancelled") & "', PRID = 0 where PRID = '" & reqID & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bSuccess = False
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bSuccess
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Approved() As String
        Get
            Return _Approved
        End Get
        Set(ByVal value As String)
            _Approved = value
        End Set
    End Property

    Public Property POID() As String
        Get
            Return _POID
        End Get
        Set(ByVal value As String)
            _POID = value
        End Set
    End Property

    Public Property Received() As Boolean
        Get
            Return _Received
        End Get
        Set(ByVal value As Boolean)
            _Received = value
        End Set
    End Property

    Public Property VendorID() As String
        Get
            Return _VendorID
        End Get
        Set(ByVal value As String)
            _VendorID = value
        End Set
    End Property

    Public Property VendorDescription() As String
        Get
            Return _VendorDescription
        End Get
        Set(ByVal value As String)
            _VendorDescription = value
        End Set
    End Property

    Public Property IsManual() As Integer
        Get
            Return _IsManual
        End Get
        Set(ByVal value As Integer)
            _IsManual = value
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

    Public Property CreatedById() As Integer
        Get
            Return _CreatedById
        End Get
        Set(ByVal value As Integer)
            _CreatedById = value
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
    Public Property PurchaseRequestID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
