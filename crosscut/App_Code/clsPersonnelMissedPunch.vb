Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnelMissedPunch
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _PunchID As Integer = 0
    Dim _PunchIn As Boolean = False
    Dim _PunchTime As String = ""
    Dim _ManagerApproved As String = ""
    Dim _ManagerID As Integer = 0
    Dim _HRApproved As String = ""
    Dim _HRID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _LinkedMissedPunchID As Integer = 0
    Dim _Reason As String = ""
    Dim _DateCreated As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelMissedPunch where MissedPunchID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelMissedPunch where MissedPunchID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelMissedPunch")
            If ds.Tables("t_PersonnelMissedPunch").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelMissedPunch").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("PunchID") Is System.DBNull.Value) Then _PunchID = dr("PunchID")
        If Not (dr("PunchIn") Is System.DBNull.Value) Then _PunchIn = dr("PunchIn")
        If Not (dr("PunchTime") Is System.DBNull.Value) Then _PunchTime = dr("PunchTime")
        If Not (dr("ManagerApproved") Is System.DBNull.Value) Then _ManagerApproved = dr("ManagerApproved")
        If Not (dr("ManagerID") Is System.DBNull.Value) Then _ManagerID = dr("ManagerID")
        If Not (dr("HRApproved") Is System.DBNull.Value) Then _HRApproved = dr("HRApproved")
        If Not (dr("HRID") Is System.DBNull.Value) Then _HRID = dr("HRID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("LinkedMissedPunchID") Is System.DBNull.Value) Then _LinkedMissedPunchID = dr("LinkedMissedPunchID")
        If Not (dr("Reason") Is System.DBNull.Value) Then _Reason = dr("Reason")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelMissedPunch where MissedPunchID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelMissedPunch")
            If ds.Tables("t_PersonnelMissedPunch").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelMissedPunch").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelMissedPunchInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@PunchID", SqlDbType.int, 0, "PunchID")
                da.InsertCommand.Parameters.Add("@PunchIn", SqlDbType.bit, 0, "PunchIn")
                da.InsertCommand.Parameters.Add("@PunchTime", SqlDbType.datetime, 0, "PunchTime")
                da.InsertCommand.Parameters.Add("@ManagerApproved", SqlDbType.varchar, 0, "ManagerApproved")
                da.InsertCommand.Parameters.Add("@ManagerID", SqlDbType.int, 0, "ManagerID")
                da.InsertCommand.Parameters.Add("@HRApproved", SqlDbType.varchar, 0, "HRApproved")
                da.InsertCommand.Parameters.Add("@HRID", SqlDbType.int, 0, "HRID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@LinkedMissedPunchID", SqlDbType.int, 0, "LinkedMissedPunchID")
                da.InsertCommand.Parameters.Add("@Reason", SqlDbType.varchar, 0, "Reason")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@MissedPunchID", SqlDbType.Int, 0, "MissedPunchID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelMissedPunch").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("PunchID", _PunchID, dr)
            Update_Field("PunchIn", _PunchIn, dr)
            Update_Field("PunchTime", _PunchTime, dr)
            Update_Field("ManagerApproved", _ManagerApproved, dr)
            Update_Field("ManagerID", _ManagerID, dr)
            Update_Field("HRApproved", _HRApproved, dr)
            Update_Field("HRID", _HRID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("LinkedMissedPunchID", _LinkedMissedPunchID, dr)
            Update_Field("Reason", _Reason, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            If ds.Tables("t_PersonnelMissedPunch").Rows.Count < 1 Then ds.Tables("t_PersonnelMissedPunch").Rows.Add(dr)
            da.Update(ds, "t_PersonnelMissedPunch")
            _ID = ds.Tables("t_PersonnelMissedPunch").Rows(0).Item("MissedPunchID")
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
            oEvents.KeyField = "MissedPunchID"
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

    Public Function Get_Missing_PunchID(ByVal persID As Integer, ByVal deptID As Integer, ByVal punchDate As DateTime, ByVal checkIn As Boolean) As Integer
        Dim punchID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If checkIn Then
                cm.CommandText = "Select PunchID from t_PersonnelPunch where personnelid = " & persID & " and departmentid = " & deptID & " and TimeIn is null and DateDiff(hh,'" & punchDate & "', TimeOut) <= 12 and DateDiff(hh,'" & punchDate & "', TimeOut) >= 0 and punchid not in (Select punchid from t_PersonnelMissedPunch where personnelid = " & persID & " and HRApproved = '0' and ManagerApproved = '0' and punchIn = 1)"
            Else
                cm.CommandText = "Select PunchID from t_PersonnelPunch where personnelid = " & persID & " and departmentid = " & deptID & " and TimeOut is null and DateDiff(hh,TimeIn,'" & punchDate & "') <= 12 and DateDiff(hh, TimeIn,'" & punchDate & "') >= 0 and punchid not in (Select punchid from t_PersonnelMissedPunch where personnelid = " & persID & " and HRApproved = '0' and ManagerApproved = '0' and punchIn = 0) order by punchid asc"
            End If
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                punchID = dread("PunchID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return punchID
    End Function

    Public Function Check_For_Submitted_Punch(ByVal punchID As Integer, ByVal punchIn As Boolean) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If punchIn Then
                cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Requests from t_PersonnelMissedPunch where punchID = " & punchID & " and ManagerApproved = '0' and HRApproved = '0' and PunchIn = 1"
            Else
                cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Requests from t_PersonnelMissedPunch where punchID = " & punchID & " and ManagerApproved = '0' and HRApproved = '0' and PunchIn = 0"
            End If
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Requests") > 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Get_Missed_Punches(ByVal mgrApproval As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select a.MissedPunchID as ID, p.LastName + ', ' + p.FirstName as Employee, d.ComboItem as Department, Case when a.PunchIn = '1' then 'Punch In' else 'Punch Out' end as PunchMissed, a.PunchTime as PunchTime, a.Reason from t_PersonnelMissedPunch a left outer join t_ComboItems d on a.DepartmentID = d.ComboItemID inner join t_Personnel p on a.PersonnelID = p.PersonnelID where a.HRApproved = '0' and a.ManagerApproved = '" & mgrApproval & "' order by d.comboitem"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Dept_Missed_Punches(ByVal persID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select a.MissedPunchID as ID, p.LastName + ', ' + p.FirstName as Employee, d.ComboItem as Department, Case when a.PunchIn = '1' then 'Punch In' else 'Punch Out' end as PunchMissed, a.PunchTime as PunchTime, a.Reason from t_PersonnelMissedPunch a inner join t_PErsonnel2Dept pd on a.Departmentid = pd.DepartmentID left outer join t_ComboItems d on a.DepartmentID = d.ComboItemID inner join t_Personnel p on a.PersonnelID = p.PersonnelID where a.HRApproved = '0' and a.ManagerApproved = '0' and pd.PersonnelID = " & persID & " and pd.Active = '1' and pd.IsManager = '1'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property PunchID() As Integer
        Get
            Return _PunchID
        End Get
        Set(ByVal value As Integer)
            _PunchID = value
        End Set
    End Property

    Public Property PunchIn() As Boolean
        Get
            Return _PunchIn
        End Get
        Set(ByVal value As Boolean)
            _PunchIn = value
        End Set
    End Property

    Public Property PunchTime() As String
        Get
            Return _PunchTime
        End Get
        Set(ByVal value As String)
            _PunchTime = value
        End Set
    End Property

    Public Property ManagerApproved() As String
        Get
            Return _ManagerApproved
        End Get
        Set(ByVal value As String)
            _ManagerApproved = value
        End Set
    End Property

    Public Property ManagerID() As Integer
        Get
            Return _ManagerID
        End Get
        Set(ByVal value As Integer)
            _ManagerID = value
        End Set
    End Property

    Public Property HRApproved() As String
        Get
            Return _HRApproved
        End Get
        Set(ByVal value As String)
            _HRApproved = value
        End Set
    End Property

    Public Property HRID() As Integer
        Get
            Return _HRID
        End Get
        Set(ByVal value As Integer)
            _HRID = value
        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Integer)
            _DepartmentID = value
        End Set
    End Property

    Public Property LinkedMissedPunchID() As Integer
        Get
            Return _LinkedMissedPunchID
        End Get
        Set(ByVal value As Integer)
            _LinkedMissedPunchID = value
        End Set
    End Property

    Public Property Reason() As String
        Get
            Return _Reason
        End Get
        Set(ByVal value As String)
            _Reason = value
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

    Public Property MissedPunchID() As Integer
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property
End Class
