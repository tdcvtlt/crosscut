Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPurchaseRequestItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PurchaseRequestID As Integer = 0
    Dim _ItemNumber As String = ""
    Dim _Qty As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _Location As String = ""
    Dim _Purpose As String = ""
    Dim _Note As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PurchaseRequestItems where Item2RequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PurchaseRequestItems where Item2RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PurchaseRequestItems")
            If ds.Tables("t_PurchaseRequestItems").Rows.Count > 0 Then
                dr = ds.Tables("t_PurchaseRequestItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PurchaseRequestID") Is System.DBNull.Value) Then _PurchaseRequestID = dr("PurchaseRequestID")
        If Not (dr("ItemNumber") Is System.DBNull.Value) Then _ItemNumber = dr("ItemNumber")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("Location") Is System.DBNull.Value) Then _Location = dr("Location")
        If Not (dr("Purpose") Is System.DBNull.Value) Then _Purpose = dr("Purpose")
        If Not (dr("Note") Is System.DBNull.Value) Then _Note = dr("Note")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select * from t_PurchaseRequestItems where Item2RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PurchaseRequestItems")
            If ds.Tables("t_PurchaseRequestItems").Rows.Count > 0 Then
                dr = ds.Tables("t_PurchaseRequestItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PurchaseRequestItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PurchaseRequestID", SqlDbType.Int, 0, "PurchaseRequestID")
                da.InsertCommand.Parameters.Add("@ItemNumber", SqlDbType.VarChar, 0, "ItemNumber")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.Int, 0, "Qty")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.Money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@Location", SqlDbType.VarChar, 0, "Location")
                da.InsertCommand.Parameters.Add("@Purpose", SqlDbType.VarChar, 0, "Purpose")
                da.InsertCommand.Parameters.Add("@Note", SqlDbType.Text, 0, "Note")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Item2RequestID", SqlDbType.Int, 0, "Item2RequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PurchaseRequestItems").NewRow
            End If
            Update_Field("PurchaseRequestID", _PurchaseRequestID, dr)
            Update_Field("ItemNumber", _ItemNumber, dr)
            Update_Field("Qty", _Qty, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("Location", _Location, dr)
            Update_Field("Purpose", _Purpose, dr)
            Update_Field("Note", _Note, dr)
            If ds.Tables("t_PurchaseRequestItems").Rows.Count < 1 Then ds.Tables("t_PurchaseRequestItems").Rows.Add(dr)
            da.Update(ds, "t_PurchaseRequestItems")
            _ID = ds.Tables("t_PurchaseRequestItems").Rows(0).Item("Item2RequestID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
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
            oEvents.KeyField = "Item2RequestID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function List_PRItems(ByVal reqID As Integer) As DataTable
        Dim dt As New DataTable
        Dim row As DataRow
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select c.Item2RequestID, c.ItemNumber, c.ITEMDESC, c.Qty, c.Amount, c.Location, c.Purpose, d.Approved from (Select a.Item2RequestID, a.ItemNumber, b.ITEMDESC, a.Qty, a.Location, a.Purpose, a.Amount, a.PurchaseRequestID from t_PurchaseRequestItems a left outer join t_IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) where a.PurchaseRequestID = '" & reqID & "') c inner join t_PurchaseRequest d on c.PurchaseRequestid = d.PurchaseRequestid order by Item2requestid asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dt.Columns.Add("Item2RequestID")
                dt.Columns.Add("ItemNumber")
                dt.Columns.Add("Description")
                dt.Columns.Add("Qty")
                dt.Columns.Add("Amount")
                dt.Columns.Add("Location")
                dt.Columns.Add("Purpose")
                dt.Columns.Add("EditItem")
                dt.Columns.Add("Remove")
            End If
            Do While dread.Read()
                row = dt.NewRow
                row("Item2RequestID") = dread("Item2RequestID")
                row("ItemNumber") = dread("ItemNumber")
                row("Description") = dread("ITEMDESC")
                row("Qty") = dread("Qty")
                row("Amount") = FormatCurrency(dread("Amount"), 2)
                row("Location") = dread("Location")
                row("Purpose") = dread("Purpose")
                If dread("Approved") = "0" Then
                    row("EditItem") = "TRUE"
                    row("Remove") = "TRUE"
                Else
                    row("EditItem") = "FALSE"
                    row("Remove") = "FALSE"
                End If
                dt.Rows.Add(row)
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return dt
    End Function
    Public Function Get_ItemOrdered_Amount(ByVal item As String) As Integer
        Dim partsInstalled As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Select Case WHEN Sum(b.Qty) is null then 0 else Sum(b.Qty) end As QtyRequested from t_PurchaseRequest a inner join t_PurchaseRequestItems b on a.purchaserequestid = b.purchaserequestid where b.ItemNumber = '" & item & "' and (a.Approved = 0 or a.Approved = 1) and a.Received = 0"
            dread = cm.ExecuteReader
            dread.Read()
            partsInstalled = dread("Parts")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return partsInstalled
    End Function
    Public Function Remove_Item(ByVal item As String, ByVal PRID As Integer) As Boolean
        Dim removePart As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then
                cn.ConnectionString = Resources.Resource.cns
                cn.Open()
            End If
            cm.CommandText = "Delete from t_PurchaseRequestItems where PurchaseRequestID = " & PRID & " and ItemNumber = '" & item & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return removePart
    End Function
    Public Function Remove_PRItem(ByVal itemID As Integer) As Boolean
        Dim removePart As Boolean = True
        'Try
        If cn.State <> ConnectionState.Open Then
            cn.ConnectionString = Resources.Resource.cns
            cn.Open()
        End If
        cm.CommandText = "Delete from t_PurchaseRequestItems where Item2RequestID = " & itemID & ""
        cm.ExecuteNonQuery()
        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State = ConnectionState.Open Then cn.Close()
        'End Try
        Return removePart
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PurchaseRequestID() As Integer
        Get
            Return _PurchaseRequestID
        End Get
        Set(ByVal value As Integer)
            _PurchaseRequestID = value
        End Set
    End Property

    Public Property ItemNumber() As String
        Get
            Return _ItemNumber
        End Get
        Set(ByVal value As String)
            _ItemNumber = value
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

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return _Location
        End Get
        Set(ByVal value As String)
            _Location = value
        End Set
    End Property

    Public Property Purpose() As String
        Get
            Return _Purpose
        End Get
        Set(ByVal value As String)
            _Purpose = value
        End Set
    End Property

    Public Property Note() As String
        Get
            Return _Note
        End Get
        Set(ByVal value As String)
            _Note = value
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

    Public Property Item2RequestID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
