Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsReservationCheckIns
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ReservationID As Integer = 0
    Dim _FollowUpDate As String = ""
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ReservationCheckIns where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ReservationCheckIns where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ReservationCheckIns")
            If ds.Tables("t_ReservationCheckIns").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationCheckIns").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ReservationID") Is System.DBNull.Value) Then _ReservationID = dr("ReservationID")
        If Not (dr("FollowUpDate") Is System.DBNull.Value) Then _FollowUpDate = dr("FollowUpDate")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ReservationCheckIns where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ReservationCheckIns")
            If ds.Tables("t_ReservationCheckIns").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationCheckIns").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReservationCheckInsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ReservationID", SqlDbType.Int, 0, "ReservationID")
                da.InsertCommand.Parameters.Add("@FollowUpDate", SqlDbType.DateTime, 0, "FollowUpDate")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ReservationCheckIns").NewRow
            End If
            Update_Field("ReservationID", _ReservationID, dr)
            Update_Field("FollowUpDate", _FollowUpDate, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            If ds.Tables("t_ReservationCheckIns").Rows.Count < 1 Then ds.Tables("t_ReservationCheckIns").Rows.Add(dr)
            da.Update(ds, "t_ReservationCheckIns")
            _ID = ds.Tables("t_ReservationCheckIns").Rows(0).Item("ID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
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
            oEvents.KeyField = "ID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Fill_Table(ByVal sDate As Date, ByVal eDate As Date) As Boolean
        Dim bFill As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Insert into t_ReservationCheckIns (ReservationID, StatusID) " & _
                     "Select r.ReservationID, (Select c.ComboItemID from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.Comboname = 'ResCheckInReportStatus' and c.ComboItem = 'Not Contacted') as StatusID " & _
                     "From t_reservations r left outer join t_ReservationCheckIns rc on r.ReservationID = rc.ReservationID " & _
                     "inner join t_ComboItems rs on r.StatusID = rs.ComboItemID " & _
                     "where r.CheckInDate between '" & sDate & "' and '" & eDate & "' and rs.ComboItem = 'In-House' and rc.ReservationID is null"
            cm.ExecuteNonQuery()
            bFill = True
        Catch ex As Exception
            bFill = False
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bFill
    End Function

    Public Function Get_Reservations(ByVal sDate As Date, ByVal eDate As Date) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ID")
        dt.Columns.Add("ReservationID")
        dt.Columns.Add("StatusID")
        dt.Columns.Add("FollowUpDate")
        dt.Columns.Add("CheckInDate")
        dt.Columns.Add("CheckOutDate")
        dt.Columns.Add("FirstName")
        dt.Columns.Add("LastName")
        dt.Columns.Add("Rooms")
        dt.Columns.Add("Extensions")

        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select rc.ID, rc.ReservationID, rc.StatusID, rc.FollowUpDate, r.CheckInDate, r.CheckOutDate, p.FirstName, p.LastName " & _
                    "from t_ReservationCheckIns rc inner join t_Reservations r on rc.ReservationID = r.ReservationID " & _
                    "inner join t_ComboItems rcs on rc.StatusID = rcs.ComboItemID " & _
                    "inner join t_Prospect p on r.ProspectID = p.ProspectID " & _
                    "where rcs.ComboItem <> 'Complete' and ((r.CheckInDate between '" & sDate & "' and '" & eDate & "') or (rc.FollowUpDate between '" & sDate & "' and '" & eDate & "')) " & _
                    "order by r.CheckInDate asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    Dim drow As DataRow = dt.NewRow
                    drow("ID") = dread("ID")
                    drow("ReservationID") = dread("ReservationID")
                    drow("StatusID") = dread("StatusID")
                    drow("FollowUpDate") = dread("FollowUpdate")
                    drow("CheckInDate") = dread("CheckInDate")
                    drow("CheckOutDate") = dread("CheckOutDate")
                    drow("FirstName") = dread("FirstName")
                    drow("LastName") = dread("LastName")
                    Dim oRC As New clsReservationCheckIns
                    drow("Rooms") = oRC.Get_Res_rooms(dread("ReservationID"))
                    drow("Extensions") = oRC.Get_Res_Phones(dread("ReservationID"))
                    oRC = Nothing
                    dt.Rows.Add(drow)
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

    Public Function Get_Res_rooms(ByVal resID As Integer) As String
        Dim rooms As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct rm.RoomNumber, rm.RoomID from t_RoomAllocationmatrix rx inner join t_Room rm on rx.RoomID = rm.RoomID where rx.ReservationID = '" & resID & "' order by rm.RoomID, rm.RoomNumber"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If rooms = "" Then
                        rooms = dread("ROomNumber")
                    Else
                        rooms = rooms & " / " & dread("RoomNumber")
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return rooms
    End Function

    Public Function Get_Res_Phones(ByVal resID As Integer) As String
        Dim rooms As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct rm.Phone, rm.RoomID from t_RoomAllocationmatrix rx inner join t_Room rm on rx.RoomID = rm.RoomID where rx.ReservationID = '" & resID & "' order by rm.RoomID, rm.Phone"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If rooms = "" Then
                        rooms = dread("Phone")
                    Else
                        rooms = rooms & " / " & dread("Phone")
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return rooms
    End Function

    Public Property ReservationID() As Integer
        Get
            Return _ReservationID
        End Get
        Set(ByVal value As Integer)
            _ReservationID = value
        End Set
    End Property

    Public Property FollowUpDate() As String
        Get
            Return _FollowUpDate
        End Get
        Set(ByVal value As String)
            _FollowUpDate = value
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

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property ID() As Integer
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
    Public Property Err As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
