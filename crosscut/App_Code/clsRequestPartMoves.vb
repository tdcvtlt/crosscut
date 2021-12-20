Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRequestPartMoves
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RequestID As Integer = 0
    Dim _ItemNumber As String = ""
    Dim _Qty As Integer = 0
    Dim _RoomFrom As Integer = 0
    Dim _RoomTo As Integer = 0
    Dim _MovedBy As Integer = 0
    Dim _DateMoved As String = ""
    Dim _StatusID As Integer = 0
    Dim _GPImported As Boolean = False
    Dim _GPImportedDate As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RequestPartMoves where RequestPartMoveID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RequestPartMoves where RequestPartMoveID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RequestPartMoves")
            If ds.Tables("t_RequestPartMoves").Rows.Count > 0 Then
                dr = ds.Tables("t_RequestPartMoves").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RequestID") Is System.DBNull.Value) Then _RequestID = dr("RequestID")
        If Not (dr("ItemNumber") Is System.DBNull.Value) Then _ItemNumber = dr("ItemNumber")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
        If Not (dr("RoomFrom") Is System.DBNull.Value) Then _RoomFrom = dr("RoomFrom")
        If Not (dr("RoomTo") Is System.DBNull.Value) Then _RoomTo = dr("RoomTo")
        If Not (dr("MovedBy") Is System.DBNull.Value) Then _MovedBy = dr("MovedBy")
        If Not (dr("DateMoved") Is System.DBNull.Value) Then _DateMoved = dr("DateMoved")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("GPImported") Is System.DBNull.Value) Then _GPImported = dr("GPImported")
        If Not (dr("GPImportedDate") Is System.DBNull.Value) Then _GPImportedDate = dr("GPImportedDate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RequestPartMoves where RequestPartMoveID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RequestPartMoves")
            If ds.Tables("t_RequestPartMoves").Rows.Count > 0 Then
                dr = ds.Tables("t_RequestPartMoves").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RequestPartMovesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RequestID", SqlDbType.int, 0, "RequestID")
                da.InsertCommand.Parameters.Add("@ItemNumber", SqlDbType.varchar, 0, "ItemNumber")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.int, 0, "Qty")
                da.InsertCommand.Parameters.Add("@RoomFrom", SqlDbType.int, 0, "RoomFrom")
                da.InsertCommand.Parameters.Add("@RoomTo", SqlDbType.int, 0, "RoomTo")
                da.InsertCommand.Parameters.Add("@MovedBy", SqlDbType.int, 0, "MovedBy")
                da.InsertCommand.Parameters.Add("@DateMoved", SqlDbType.datetime, 0, "DateMoved")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@GPImported", SqlDbType.bit, 0, "GPImported")
                da.InsertCommand.Parameters.Add("@GPImportedDate", SqlDbType.datetime, 0, "GPImportedDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RequestPartMoveID", SqlDbType.Int, 0, "RequestPartMoveID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RequestPartMoves").NewRow
            End If
            Update_Field("RequestID", _RequestID, dr)
            Update_Field("ItemNumber", _ItemNumber, dr)
            Update_Field("Qty", _Qty, dr)
            Update_Field("RoomFrom", _RoomFrom, dr)
            Update_Field("RoomTo", _RoomTo, dr)
            Update_Field("MovedBy", _MovedBy, dr)
            Update_Field("DateMoved", _DateMoved, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("GPImported", _GPImported, dr)
            Update_Field("GPImportedDate", _GPImportedDate, dr)
            If ds.Tables("t_RequestPartMoves").Rows.Count < 1 Then ds.Tables("t_RequestPartMoves").Rows.Add(dr)
            da.Update(ds, "t_RequestPartMoves")
            _ID = ds.Tables("t_RequestPartMoves").Rows(0).Item("RequestPartMoveID")
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
            oEvents.KeyField = "RequestPartMoveID"
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
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select a.RequestPartMoveID, a.ItemNumber, b.ITEMDESC, a.Qty, rm.RoomNumber as MovedFrom, ps.ComboItem as PartStatus from t_RequestPartMoves a inner join t_IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) left outer join t_Room rm on a.RoomFrom = rm.RoomID left outer join t_COmboItems ps on a.StatusID = ps.ComboitemID where a.RequestID = '" & requestID & "' order by requestpartmoveid asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dt.Columns.Add("RequestPartMoveID")
                dt.Columns.Add("ItemNumber")
                dt.Columns.Add("Description")
                dt.Columns.Add("Qty")
                dt.Columns.Add("MovedFrom")
                dt.Columns.Add("Status")
                dt.Columns.Add("Remove")
                dt.Columns.Add("Install")
            End If
            Do While dread.Read()
                row = dt.NewRow
                row("RequestPartMoveID") = dread("RequestPartMoveID")
                row("ItemNumber") = dread("ItemNumber")
                row("Description") = dread("ITEMDESC")
                row("Qty") = dread("Qty")
                row("MovedFrom") = dread("MovedFrom")
                row("Status") = dread("PartStatus")
                If dread("PartStatus") = "Assigned" Or dread("PartStatus") = "Requested" Then
                    row("Remove") = "TRUE"
                End If
                If dread("PartStatus") = "Assigned" Then
                    row("Install") = "TRUE"
                End If
                dt.Rows.Add(row)
            Loop
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function

    Public Function Get_Requested_Count(ByVal reqID As Integer) As Integer
        Dim parts As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else count(*) end as Parts from t_RequestPartMoves r inner join t_ComboItems s on r.StatusID = s.ComboItemID where requestID = " & reqID & " and s.ComboItem = 'Requested'"
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

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property RequestID() As Integer
        Get
            Return _RequestID
        End Get
        Set(ByVal value As Integer)
            _RequestID = value
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

    Public Property RoomFrom() As Integer
        Get
            Return _RoomFrom
        End Get
        Set(ByVal value As Integer)
            _RoomFrom = value
        End Set
    End Property

    Public Property RoomTo() As Integer
        Get
            Return _RoomTo
        End Get
        Set(ByVal value As Integer)
            _RoomTo = value
        End Set
    End Property

    Public Property MovedBy() As Integer
        Get
            Return _MovedBy
        End Get
        Set(ByVal value As Integer)
            _MovedBy = value
        End Set
    End Property

    Public Property DateMoved() As String
        Get
            Return _DateMoved
        End Get
        Set(ByVal value As String)
            _DateMoved = value
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
    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

    Public Property RequestPartMoveID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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
