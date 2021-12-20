Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsReservationWaitlist
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _UnitTypeID As Integer = 0
    Dim _BedRooms As Integer = 0
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _Active As Boolean = False
    Dim _SeasonID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ReservationWaitlist where WaitListID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ReservationWaitlist where WaitListID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ReservationWaitlist")
            If ds.Tables("t_ReservationWaitlist").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationWaitlist").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("BedRooms") Is System.DBNull.Value) Then _BedRooms = dr("BedRooms")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("SeasonID") Is System.DBNull.Value) Then _SeasonID = dr("SeasonID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ReservationWaitlist where WaitListID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ReservationWaitlist")
            If ds.Tables("t_ReservationWaitlist").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationWaitlist").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReservationWaitlistInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.int, 0, "UnitTypeID")
                da.InsertCommand.Parameters.Add("@BedRooms", SqlDbType.int, 0, "BedRooms")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.datetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.datetime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.Int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@SeasonID", SqlDbType.int, 0, "SeasonID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@WaitListID", SqlDbType.Int, 0, "WaitListID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ReservationWaitlist").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("UnitTypeID", _UnitTypeID, dr)
            Update_Field("BedRooms", _BedRooms, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("SeasonID", _SeasonID, dr)
            If ds.Tables("t_ReservationWaitlist").Rows.Count < 1 Then ds.Tables("t_ReservationWaitlist").Rows.Add(dr)
            da.Update(ds, "t_ReservationWaitlist")
            _ID = ds.Tables("t_ReservationWaitlist").Rows(0).Item("WaitListID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "WaitListID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Get_Waitlist(ByVal sDate As Date, ByVal eDate As Date, ByVal unitType As String, ByVal bd As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select rw.WaitlistID, rw.DateCreated, p.Firstname + ' ' + p.LastName as Owner, c.ContractNumber, rw.StartDate, rw.EndDate, ut.ComboItem as UnitType, rw.BedRooms as BR, rws.ComboItem as ReqSeason, cs.CombOItem as ContractSeason, pe.UserName from t_ReservationWaitList rw inner join t_Contract c on rw.ContractiD = c.ContractID inner join t_Prospect p on rw.ProspectID = p.ProspectID inner join t_ComboItems ut on rw.unitTypeID = ut.ComboitemID left outer join t_ComboItems cs on c.SeasonID = cs.ComboitemID inner join t_CombOItems rws on rw.SeasonID = rws.CombOItemID inner join t_Personnel pe on rw.CreatedByID = pe.PersonnelID where rw.StartDate Between '" & sDate & "' and '" & eDate & "' and rw.UnitTypeID in (" & unitType & ") and rw.Bedrooms in (" & bd & ") and rw.active = '1'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Con_Filter(ByVal filter As String, ByVal filterText As String) As SQLDataSOurce
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "Phone" Then
                ds.SelectCommand = "Select Distinct TOP 50 c.ProspectID, p.Firstname, p.LastName from t_Contract c inner join t_prospect p on c.prospectid = p.prospectid inner join t_ProspectPhone pp on p.ProspectID = pp.ProspectID where pp.Number = '" & filterText & "'"
            ElseIf filter = "Name" Then
                If InStr(filterText, ",") > 0 Then
                    Dim strName() As String
                    strName = filterText.Split(",")
                    If strName(1) <> "" Then
                        ds.SelectCommand = "Select Distinct TOP 50 c.ProspectID, p.Firstname, p.LastName from t_Contract c inner join t_prospect p on c.prospectid = p.prospectid where p.Lastname = '" & strName(0) & "' and p.FirstName LIKE '" & Trim(strName(1)) & "%'"
                    Else
                        ds.SelectCommand = "Select Distinct TOP 50 c.ProspectID, p.Firstname, p.LastName from t_Contract c inner join t_prospect p on c.prospectid = p.prospectid where p.Lastname Like '" & strName(0) & "%'"
                    End If
                Else
                    ds.SelectCommand = "Select Distinct TOP 50 c.ProspectID, p.Firstname, p.LastName from t_Contract c inner join t_prospect p on c.prospectid = p.prospectid where p.LastName LIKE '" & filterText & "%'"
                End If
            Else
                ds.SelectCommand = "Select Distinct TOP 50 c.ProspectID, p.Firstname, p.LastName from t_Contract c inner join t_prospect p on c.prospectid = p.prospectid where c.ContractNumber = '" & filterText & "'"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
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

    Public Property BedRooms() As Integer
        Get
            Return _BedRooms
        End Get
        Set(ByVal value As Integer)
            _BedRooms = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
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

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property SeasonID() As Integer
        Get
            Return _SeasonID
        End Get
        Set(ByVal value As Integer)
            _SeasonID = value
        End Set
    End Property

    Public Property WaitListID() As Integer
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
