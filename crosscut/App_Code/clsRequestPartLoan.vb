Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRequestPartLoan
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ItemNumber As String = ""
    Dim _RequestID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _ReservationID As Integer = 0
    Dim _Qty As Integer = 0
    Dim _DateLoaned As String = ""
    Dim _DatePickedUp As String = ""
    Dim _StatusID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RequestPartLoan where RequestPartLoanID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RequestPartLoan where RequestPartLoanID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RequestPartLoan")
            If ds.Tables("t_RequestPartLoan").Rows.Count > 0 Then
                dr = ds.Tables("t_RequestPartLoan").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ItemNumber") Is System.DBNull.Value) Then _ItemNumber = dr("ItemNumber")
        If Not (dr("RequestID") Is System.DBNull.Value) Then _RequestID = dr("RequestID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("ReservationID") Is System.DBNull.Value) Then _ReservationID = dr("ReservationID")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
        If Not (dr("DateLoaned") Is System.DBNull.Value) Then _DateLoaned = dr("DateLoaned")
        If Not (dr("DatePickedUp") Is System.DBNull.Value) Then _DatePickedUp = dr("DatePickedUp")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RequestPartLoan where RequestPartLoanID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RequestPartLoan")
            If ds.Tables("t_RequestPartLoan").Rows.Count > 0 Then
                dr = ds.Tables("t_RequestPartLoan").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RequestPartLoanInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ItemNumber", SqlDbType.varchar, 0, "ItemNumber")
                da.InsertCommand.Parameters.Add("@RequestID", SqlDbType.int, 0, "RequestID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.int, 0, "ReservationID")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.int, 0, "Qty")
                da.InsertCommand.Parameters.Add("@DateLoaned", SqlDbType.datetime, 0, "DateLoaned")
                da.InsertCommand.Parameters.Add("@DatePickedUp", SqlDbType.datetime, 0, "DatePickedUp")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RequestPartLoanID", SqlDbType.Int, 0, "RequestPartLoanID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RequestPartLoan").NewRow
            End If
            Update_Field("ItemNumber", _ItemNumber, dr)
            Update_Field("RequestID", _RequestID, dr)
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("ReservationID", _ReservationID, dr)
            Update_Field("Qty", _Qty, dr)
            Update_Field("DateLoaned", _DateLoaned, dr)
            Update_Field("DatePickedUp", _DatePickedUp, dr)
            Update_Field("StatusID", _StatusID, dr)
            If ds.Tables("t_RequestPartLoan").Rows.Count < 1 Then ds.Tables("t_RequestPartLoan").Rows.Add(dr)
            da.Update(ds, "t_RequestPartLoan")
            _ID = ds.Tables("t_RequestPartLoan").Rows(0).Item("RequestPartLoanID")
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
            oEvents.KeyField = "RequestPartLoanID"
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
            cm.CommandText = "Select a.RequestPartLoanID, a.ItemNumber, b.ITEMDESC, a.Qty, a.ReservationID, rm.RoomNumber as MovedFrom, a.DatePickedUp, ps.ComboItem as PartStatus from t_RequestPartLoan a inner join t_IV00101 b on RTrim(a.ItemNumber) = RTrim(b.ITEMNMBR) left outer join t_Room rm on a.RoomID = rm.RoomID left outer join t_COmboItems ps on a.StatusID = ps.ComboitemID where a.RequestID = '" & requestID & "' order by requestpartloanid asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dt.Columns.Add("RequestPartLoanID")
                dt.Columns.Add("ItemNumber")
                dt.Columns.Add("Description")
                dt.Columns.Add("Qty")
                dt.Columns.Add("ReservationID")
                dt.Columns.Add("MovedFrom")
                dt.Columns.Add("DatePickedUp")
                dt.Columns.Add("Status")
                dt.Columns.Add("Return")
            End If
            Do While dread.Read()
                row = dt.NewRow
                row("RequestPartLoanID") = dread("RequestPartLoanID")
                row("ItemNumber") = dread("ItemNumber")
                row("Description") = dread("ITEMDESC")
                row("Qty") = dread("Qty")
                row("ReservationID") = dread("ReservationID")
                row("MovedFrom") = dread("MovedFrom")
                row("DatePickedUp") = dread("DatePickedUp")
                row("Status") = dread("PartStatus")
                If dread("PartStatus") = "OnLoan" Then
                    row("Return") = "TRUE"
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
    Public Function Get_Loaned_Count(ByVal item As String) As Integer
        Dim partsInstalled As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Sum(rp.Qty) is Null then 0 Else Sum(rp.Qty) end as Parts from t_RequestParts rp inner join t_comboItems ps on rp.StatusID = ps.ComboItemID where rp.ItemNumber = '" & item & "' and ps.ComboItem = 'OnLoan'"
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

    Public Function Get_ReqLoaned_Count(ByVal reqID As Integer) As Integer
        Dim parts As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Parts from t_RequestPartLoan r inner join t_ComboItems s on r.StatusID = s.ComboItemID where r.RequestID = " & reqID & " and s.ComboItem = 'OnLoan'"
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

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
        End Set
    End Property

    Public Property ReservationID() As Integer
        Get
            Return _ReservationID
        End Get
        Set(ByVal value As Integer)
            _ReservationID = value
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

    Public Property DateLoaned() As String
        Get
            Return _DateLoaned
        End Get
        Set(ByVal value As String)
            _DateLoaned = value
        End Set
    End Property

    Public Property DatePickedUp() As String
        Get
            Return _DatePickedUp
        End Get
        Set(ByVal value As String)
            _DatePickedUp = value
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

    Public Property RequestPartLoanID() As Integer
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
