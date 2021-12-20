Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Math
Imports System

Public Class clsPersonnelPunch
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _TimeIn As String = ""
    Dim _TimeOut As String = ""
    Dim _DateIn As String = ""
    Dim _DateOut As String = ""
    Dim _InManual As Boolean = False
    Dim _InUserID As Integer = 0
    Dim _OutManual As Boolean = False
    Dim _OutUserID As Integer = 0
    Dim _InManDate As String = ""
    Dim _OutManDate As String = ""
    Dim _DepartmentID As Integer = 0
    Dim _InClockID As Integer = 0
    Dim _OutClockID As Integer = 0
    Dim _PunchTypeID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelPunch where PunchID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelPunch where PunchID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelPunch")
            If ds.Tables("t_PersonnelPunch").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelPunch").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("TimeIn") Is System.DBNull.Value) Then _TimeIn = dr("TimeIn")
        If Not (dr("TimeOut") Is System.DBNull.Value) Then _TimeOut = dr("TimeOut")
        If Not (dr("DateIn") Is System.DBNull.Value) Then _DateIn = dr("DateIn")
        If Not (dr("DateOut") Is System.DBNull.Value) Then _DateOut = dr("DateOut")
        If Not (dr("InManual") Is System.DBNull.Value) Then _InManual = dr("InManual")
        If Not (dr("InUserID") Is System.DBNull.Value) Then _InUserID = dr("InUserID")
        If Not (dr("OutManual") Is System.DBNull.Value) Then _OutManual = dr("OutManual")
        If Not (dr("OutUserID") Is System.DBNull.Value) Then _OutUserID = dr("OutUserID")
        If Not (dr("InManDate") Is System.DBNull.Value) Then _InManDate = dr("InManDate")
        If Not (dr("OutManDate") Is System.DBNull.Value) Then _OutManDate = dr("OutManDate")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("InClockID") Is System.DBNull.Value) Then _InClockID = dr("InClockID")
        If Not (dr("OutClockID") Is System.DBNull.Value) Then _OutClockID = dr("OutClockID")
        If Not (dr("PunchTypeID") Is System.DBNull.Value) Then _PunchTypeID = dr("PunchTypeID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelPunch where PunchID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelPunch")
            If ds.Tables("t_PersonnelPunch").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelPunch").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelPunchInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@TimeIn", SqlDbType.datetime, 0, "TimeIn")
                da.InsertCommand.Parameters.Add("@TimeOut", SqlDbType.datetime, 0, "TimeOut")
                da.InsertCommand.Parameters.Add("@DateIn", SqlDbType.datetime, 0, "DateIn")
                da.InsertCommand.Parameters.Add("@DateOut", SqlDbType.datetime, 0, "DateOut")
                da.InsertCommand.Parameters.Add("@InManual", SqlDbType.bit, 0, "InManual")
                da.InsertCommand.Parameters.Add("@InUserID", SqlDbType.int, 0, "InUserID")
                da.InsertCommand.Parameters.Add("@OutManual", SqlDbType.bit, 0, "OutManual")
                da.InsertCommand.Parameters.Add("@OutUserID", SqlDbType.int, 0, "OutUserID")
                da.InsertCommand.Parameters.Add("@InManDate", SqlDbType.datetime, 0, "InManDate")
                da.InsertCommand.Parameters.Add("@OutManDate", SqlDbType.datetime, 0, "OutManDate")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@InClockID", SqlDbType.int, 0, "InClockID")
                da.InsertCommand.Parameters.Add("@OutClockID", SqlDbType.int, 0, "OutClockID")
                da.InsertCommand.Parameters.Add("@PunchTypeID", SqlDbType.int, 0, "PunchTypeID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PunchID", SqlDbType.Int, 0, "PunchID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelPunch").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("TimeIn", _TimeIn, dr)
            Update_Field("TimeOut", _TimeOut, dr)
            Update_Field("DateIn", _DateIn, dr)
            Update_Field("DateOut", _DateOut, dr)
            Update_Field("InManual", _InManual, dr)
            Update_Field("InUserID", _InUserID, dr)
            Update_Field("OutManual", _OutManual, dr)
            Update_Field("OutUserID", _OutUserID, dr)
            Update_Field("InManDate", _InManDate, dr)
            Update_Field("OutManDate", _OutManDate, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("InClockID", _InClockID, dr)
            Update_Field("OutClockID", _OutClockID, dr)
            Update_Field("PunchTypeID", _PunchTypeID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_PersonnelPunch").Rows.Count < 1 Then ds.Tables("t_PersonnelPunch").Rows.Add(dr)
            da.Update(ds, "t_PersonnelPunch")
            _ID = ds.Tables("t_PersonnelPunch").Rows(0).Item("PunchID")
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
            oEvents.KeyField = "PunchID"
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

    Public Function Get_Last_Activity(ByVal persID As Integer) As String
        Dim activity As String = "Last TimeClock Activity: "
        Dim sTime(2) As String
        Try 
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 * from t_PersonnelPunch where personnelid = " & persID & " order by DateIn desc, TimeIn desc, punchID desc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If IsDBNull(dread("TimeIn")) Then
                    sTime = Split(dread("TimeIn"), " ")
                    activity = activity & "Punched Out On " & sTime(0) & " At " & sTime(1) & " " & sTime(2) & "."
                Else
                    Dim dtime As DateTime
                    dtime = dread("TimeIn")
                    dread.Close()
                    cm.CommandText = "Select Top 1 * from t_PersonnelPunch where personnelid = " & persID & " order by DateOut desc, timeout desc, punchid desc"
                    dread = cm.ExecuteReader
                    dread.Read()
                    If IsDBNull(dread("TimeOut")) Then
                        sTime = Split(dread("TimeIn"), " ")
                        activity = activity & "Punched Out On " & sTime(0) & " At " & sTime(1) & " " & sTime(2) & "."
                    Else
                        If DateTime.Compare(dread("TimeOut"), dtime) > 0 Then
                            sTime = Split(dread("TimeOut"), " ")
                            activity = activity & "Punched Out On " & sTime(0) & " At " & sTime(1) & " " & sTime(2) & "."
                        Else
                            sTime = Split(dtime, " ")
                            activity = activity & "Punched In On " & sTime(0) & " At " & sTime(1) & " " & sTime(2) & "."
                        End If
                    End If
                End If
            Else
                activity = activity & "N/A."
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return activity
    End Function

    Public Function Val_Punch_In_With_Date(ByVal persID As Integer, ByVal deptID As Integer, ByVal pDate As Date) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Punches from t_PersonnelPunch where timeout is null and datediff(hh, TimeIn, '" & pDate & "') < 2 and personnelid = " & persID & " and departmentid = " & deptID & ""
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Punches") > 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Val_Punch_In(ByVal persID As Integer, ByVal deptID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Punches from t_PersonnelPunch where timeout is null and datediff(hh, TimeIn, GetDate()) < 2 and personnelid = " & persID & " and departmentid = " & deptID & ""
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Punches") > 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Val_Punch_Out_With_Date(ByVal persID As Integer, ByVal deptID As Integer, ByVal pDate As Date) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 *, DateDiff(hh, TimeOUt, '" & pDate & "') As HoursDiff from t_PersonnelPunch where personnelid = '" & persID & "' and departmentid = '" & deptID & "' order by timeout desc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("HoursDiff") < 1 Then
                    bValid = False
                End If
            End If
            dread.Close()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Val_Punch_Out(ByVal persID As Integer, ByVal deptID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 *, DateDiff(hh, TimeOUt, GetDate()) As HoursDiff from t_PersonnelPunch where personnelid = '" & persID & "' and departmentid = '" & deptID & "' order by timeout desc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("HoursDiff") < 1 Then
                    bValid = False
                End If
            End If
            dread.Close()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Missed_Punch_Check(ByVal persID As Integer, ByVal deptID As Integer, ByVal inOut As String) As String
        Dim mp As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            'Replaced 5/7/2013
            'cm.CommandText = "Select * from t_PersonnelPunch a left outer join t_PersonnelMissedPunch b on a.punchid = b.punchid where a.TimeOut Is Null and a.departmentid = '" & deptID & "' and a.personnelid = '" & persID & "' and b.MissedPunchID is null order by timein asc"
            If inOut = "In" Then
                cm.CommandText = "Select * from t_personnelPunch where ((timein < '" & System.DateTime.Now & "' and timeout is null) or (timeout < '" & System.DateTime.Now & "' and timein is null)) and personnelid = " & persID & " and departmentid = " & deptID
            Else
                cm.CommandText = "Select * from t_personnelPunch where ((Timeout IS NULL and datediff(hh, TimeIn, GetDate()) > 12) or (timeout < '" & System.DateTime.Now & "' and timein is null)) and personnelid = " & persID & " and departmentid = " & deptID
            End If
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                dread.Read()
                If IsDBNull(dread("TimeOut")) Then
                    mp = "You Punched In On " & dread("TimeIn") & " and did not punch out. You need to submit a missed punch form before you can Punch " & inOut & ". This punch has not been registered."
                Else
                    mp = "You Punched Out On " & dread("TimeOUt") & " and did not punch in. You need to submit a missed punch form before you can Punch " & inOut & ". This punch has not been registered."
                End If
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return mp
    End Function

    Public Function Missed_Punch_Check_With_Date(ByVal persID As Integer, ByVal deptID As Integer, ByVal inOut As String, ByVal pDate As Date) As String
        Dim mp As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            'Replaced 5/7/2013
            'cm.CommandText = "Select * from t_PersonnelPunch a left outer join t_PersonnelMissedPunch b on a.punchid = b.punchid where a.TimeOut Is Null and a.departmentid = '" & deptID & "' and a.personnelid = '" & persID & "' and b.MissedPunchID is null order by timein asc"
            If inOut = "In" Then
                cm.CommandText = "Select * from t_personnelPunch where ((timein < '" & pDate & "' and timeout is null) or (timeout < '" & pDate & "' and timein is null)) and personnelid = " & persID & " and departmentid = " & deptID
            Else
                cm.CommandText = "Select * from t_personnelPunch where ((Timeout IS NULL and datediff(hh, TimeIn, '" & pDate & "') > 12) or (timeout < '" & pDate & "' and timein is null)) and personnelid = " & persID & " and departmentid = " & deptID
            End If
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                dread.Read()
                If IsDBNull(dread("TimeOut")) Then
                    mp = " Punched In On " & dread("TimeIn") & " and did not punch out. A missed punch form will need to be submitted before they can Punch " & inOut & ". This punch has not been registered."
                Else
                    mp = " Punched Out On " & dread("TimeOUt") & " and did not punch in. A missed punch form will need to be submitted before they can Punch " & inOut & ". This punch has not been registered."
                End If
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return mp
    End Function

    Public Function Get_Current_Punch(ByVal persID As Integer) As Integer
        Dim punchID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 * from t_PersonnelPunch where Timeout IS NULL and datediff(hh, TimeIn, GetDate()) < 12 and personnelid = " & persID & " and punchid not in (Select punchID from t_PersonnelMissedPunch where punchin = 0 and personnelid = " & persID & " and HRApproved = '0' and ManagerApproved = '0') order by punchid desc"
            dread = cm.ExecuteReader()
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

    Public Function Get_Current_Punch_With_Date(ByVal persID As Integer, ByVal pDate As Date) As Integer
        Dim punchID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 * from t_PersonnelPunch where Timeout IS NULL and datediff(hh, TimeIn, '" & pDate & "') < 12 and personnelid = " & persID & " and punchid not in (Select punchID from t_PersonnelMissedPunch where punchin = 0 and personnelid = " & persID & " and HRApproved = '0' and ManagerApproved = '0') order by punchid desc"
            dread = cm.ExecuteReader()
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

    Public Function Get_Punches(ByVal persID As Integer) As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim punches As String = "''"
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim appStatus As String = ""
        dt.Columns.Add("PunchID")
        dt.Columns.Add("DateIn")
        dt.Columns.Add("TimeIn")
        dt.Columns.Add("InManual")
        dt.Columns.Add("DateOut")
        dt.Columns.Add("TimeOut")
        dt.Columns.Add("OutManual")
        dt.Columns.Add("Department")
        dt.Columns.Add("PunchType")
        dt.Columns.Add("MissedPunchApproval")
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "SELECT a.PunchID, a.DateIn, Case when a.InManual = '1' then 'True' else 'False' end as InManual, a.TimeIn, a.DateOut, Case when a.OutManual = '1' then 'True' else 'False' end as OutManual, a.TimeOut, b.ComboItem AS Department, c.ComboItem AS PunchType FROM t_PersonnelPunch a LEFT OUTER JOIN t_ComboItems b ON a.DepartmentID = b.ComboItemID LEFT OUTER JOIN t_ComboItems c ON a.PunchTypeID = c.ComboItemID WHERE ((a.PersonnelID = " & persID & ") AND (a.TimeIn IS NULL)) OR ((a.PersonnelID = " & persID & ") AND (a.TimeOut IS NULL)) order by a.timein desc"
        '5/8/Addition to mark missed punch approvals green - MB
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        da.Fill(ds, "PunchGroup1")
        If ds.Tables("PunchGroup1").Rows.Count > 0 Then
            For i = 0 To ds.Tables("PunchGroup1").Rows.Count - 1
                appStatus = ""
                cm.CommandText = "Select PunchIn, ManagerApproved, HRApproved from t_PersonnelMissedPunch where punchID = " & ds.Tables("PunchGroup1").Rows(i).Item("PunchID")
                da.Fill(ds, "MissedGroup1")
                If ds.Tables("MissedGroup1").Rows.Count > 0 Then
                    For j = 0 To ds.Tables("MissedGroup1").Rows.Count - 1
                        If ds.Tables("MissedGroup1").Rows(j).Item("HRApproved") = "1" Or ds.Tables("MissedGroup1").Rows(j).Item("ManagerApproved") = "1" Then
                            If ds.Tables("MissedGroup1").Rows(j).Item("PunchIn") = True Then
                                If appStatus <> "" Then
                                    appStatus = "BothApproved"
                                Else
                                    appStatus = "InApproved"
                                End If
                            Else
                                If appStatus <> "" Then
                                    appStatus = "BothApproved"
                                Else
                                    appStatus = "OutApproved"
                                End If
                            End If
                        End If
                    Next
                End If
                ds.Tables("MissedGroup1").Clear()
                dr = dt.NewRow
                dr("PunchID") = ds.Tables("PunchGroup1").Rows(i).Item("PunchID")
                dr("DateIn") = ds.Tables("PunchGroup1").Rows(i).Item("DateIn")
                dr("TimeIn") = ds.Tables("PunchGroup1").Rows(i).Item("TimeIn")
                dr("InManual") = ds.Tables("PunchGroup1").Rows(i).Item("InManual")
                dr("DateOut") = ds.Tables("PunchGroup1").Rows(i).Item("DateOut")
                dr("TimeOut") = ds.Tables("PunchGroup1").Rows(i).Item("TimeOut")
                dr("OutManual") = ds.Tables("PunchGroup1").Rows(i).Item("OutManual")
                dr("Department") = ds.Tables("PunchGroup1").Rows(i).Item("Department")
                dr("PunchType") = ds.Tables("PunchGroup1").Rows(i).Item("PunchType")
                dr("MissedPunchApproval") = appStatus
                If punches = "''" Then
                    punches = "'" & ds.Tables("PunchGroup1").Rows(i).Item("PunchID") & "'"
                Else
                    punches = punches & ",'" & ds.Tables("PunchGroup1").Rows(i).Item("PunchID") & "'"
                End If
                dt.Rows.Add(dr)
            Next
        End If
        ds.Tables("PunchGroup1").Clear()

        cm.CommandText = "SELECT Top 50 a.PunchID, a.DateIn, Case when a.InManual = '1' then 'True' else 'False' end as Inmanual, a.TimeIn, a.DateOut, Case when a.OutManual = '1' then 'True' else 'False' end as OutManual, a.TimeOut, b.ComboItem AS Department, c.ComboItem AS PunchType FROM t_PersonnelPunch a LEFT OUTER JOIN t_ComboItems b ON a.DepartmentID = b.ComboItemID LEFT OUTER JOIN t_ComboItems c ON a.PunchTypeID = c.ComboItemID WHERE (a.PersonnelID = " & persID & ") AND (a.PunchID NOT IN (" & punches & ")) ORDER BY a.TimeIn DESC"
        da.Fill(ds, "PunchGroup1")
        If ds.Tables("PunchGroup1").Rows.Count > 0 Then
            For i = 0 To ds.Tables("PunchGroup1").Rows.Count - 1
                appStatus = ""
                cm.CommandText = "Select PunchIn, ManagerApproved, HRApproved from t_PersonnelMissedPunch where punchID = " & ds.Tables("PunchGroup1").Rows(i).Item("PunchID")
                da.Fill(ds, "MissedGroup1")
                If ds.Tables("MissedGroup1").Rows.Count > 0 Then
                    For j = 0 To ds.Tables("MissedGroup1").Rows.Count - 1
                        If ds.Tables("MissedGroup1").Rows(j).Item("HRApproved") = "1" Or ds.Tables("MissedGroup1").Rows(j).Item("ManagerApproved") = "1" Then
                            If ds.Tables("MissedGroup1").Rows(j).Item("PunchIn") = True Then
                                If appStatus <> "" Then
                                    appStatus = "BothApproved"
                                Else
                                    appStatus = "InApproved"
                                End If
                            Else
                                If appStatus <> "" Then
                                    appStatus = "BothApproved"
                                Else
                                    appStatus = "OutApproved"
                                End If
                            End If
                        End If
                    Next
                End If
                ds.Tables("MissedGroup1").Clear()
                dr = dt.NewRow
                dr("PunchID") = ds.Tables("PunchGroup1").Rows(i).Item("PunchID")
                dr("DateIn") = ds.Tables("PunchGroup1").Rows(i).Item("DateIn")
                dr("TimeIn") = ds.Tables("PunchGroup1").Rows(i).Item("TimeIn")
                dr("InManual") = ds.Tables("PunchGroup1").Rows(i).Item("InManual")
                dr("DateOut") = ds.Tables("PunchGroup1").Rows(i).Item("DateOut")
                dr("TimeOut") = ds.Tables("PunchGroup1").Rows(i).Item("TimeOut")
                dr("OutManual") = ds.Tables("PunchGroup1").Rows(i).Item("OutManual")
                dr("Department") = ds.Tables("PunchGroup1").Rows(i).Item("Department")
                dr("PunchType") = ds.Tables("PunchGroup1").Rows(i).Item("PunchType")
                dr("MissedPunchApproval") = appStatus
                dt.Rows.Add(dr)
            Next
        End If
        '** End 5/8/13 addition

        'Replace 5/8/13
        'dread = cm.ExecuteReader
        'If dread.HasRows Then
        '    Do While dread.Read
        '        dr = dt.NewRow
        '        dr("PunchID") = dread("PunchID")
        '        dr("DateIn") = dread("DateIn")
        '        dr("TimeIn") = dread("TimeIn")
        '        dr("InManual") = dread("InManual")
        '        dr("DateOut") = dread("DateOut")
        '        dr("TimeOut") = dread("TimeOut")
        '        dr("OutManual") = dread("OutManual")
        '        dr("Department") = dread("Department")
        '        dr("PunchType") = dread("PunchType")
        '        If punches = "''" Then
        '            punches = "'" & dread("PunchID") & "'"
        '        Else
        '            punches = punches & ",'" & dread("PunchID") & "'"
        '        End If
        '        dt.Rows.Add(dr)
        '    Loop
        'End If
        'dread.Close()

        'cm.CommandText = "SELECT Top 50 a.PunchID, a.DateIn, Case when a.InManual = '1' then 'True' else 'False' end as Inmanual, a.TimeIn, a.DateOut, Case when a.OutManual = '1' then 'True' else 'False' end as OutManual, a.TimeOut, b.ComboItem AS Department, c.ComboItem AS PunchType FROM t_PersonnelPunch a LEFT OUTER JOIN t_ComboItems b ON a.DepartmentID = b.ComboItemID LEFT OUTER JOIN t_ComboItems c ON a.PunchTypeID = c.ComboItemID WHERE (a.PersonnelID = " & persID & ") AND (a.PunchID NOT IN (" & punches & ")) ORDER BY a.TimeIn DESC"
        'dread = cm.ExecuteReader
        'If dread.HasRows Then
        '    Do While dread.Read
        '        dr = dt.NewRow
        '        dr("PunchID") = dread("PunchID")
        '        dr("DateIn") = dread("DateIn")
        '        dr("TimeIn") = dread("TimeIn")
        '        dr("InManual") = dread("InManual")
        '        dr("DateOut") = dread("DateOut")
        '        dr("TimeOut") = dread("TimeOut")
        '        dr("OutManual") = dread("OutManual")
        '        dr("Department") = dread("Department")
        '        dr("PunchType") = dread("PunchType")
        '        dt.Rows.Add(dr)
        '    Loop
        'End If
        'dread.Close()


        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return dt
    End Function

    Public Function Delete_Punch(ByVal punchID As Integer) As Boolean
        Dim bDeleted As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_PersonnelPunch where punchID = " & punchID & ""
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bDeleted = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bDeleted
    End Function
    Public Function get_Hours(ByVal sDate As Date, ByVal eDate As Date, ByVal persID As Integer) As String
        Dim first As Boolean = True
        Dim hours As String = ""
        Dim dept As String = ""
        Dim tempDate As Date
        Dim tempHours As Double = 0
        Dim pOTTotal As Double = 0
        Dim dOTTotal As Double = 0
        Dim cOTTotal As Double = 0
        Dim gOTTotal As Double = 0
        Dim dPALTotal As Double = 0
        Dim dJURYTotal As Double = 0
        Dim dBERTotal As Double = 0
        Dim dSSLBTotal As Double = 0
        Dim dWORKTotal As Double = 0
        Dim pPALTotal As Double = 0
        Dim pJURYTotal As Double = 0
        Dim pBERTotal As Double = 0
        Dim pSSLBTotal As Double = 0
        Dim pWORKTotal As Double = 0
        Dim cPALTotal As Double = 0
        Dim cJURYTotal As Double = 0
        Dim cBERTotal As Double = 0
        Dim cSSLBTotal As Double = 0
        Dim cWORKTotal As Double = 0
        Dim gPALTotal As Double = 0
        Dim gJURYTotal As Double = 0
        Dim gBERTotal As Double = 0
        Dim gSSLBTotal As Double = 0
        Dim gWORKTotal As Double = 0
        Dim pTotal(3) As Integer
        Dim dTotal(3) As Integer
        Dim cTotal(3) As Integer
        Dim gTotal(3) As Integer
        For i = 0 To 2
            gTotal(i) = 0
            cTotal(i) = 0
            pTotal(i) = 0
            dTotal(i) = 0
        Next
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandTimeout = 0
        cm.CommandText = "Select Round(DIFF1/60,1) as Diff1Rd, Round(DIFF2/60, 1) As Diff2Rd, * from ufn_PunchDetail('" & sDate & "', '" & eDate & "') where personnelid = " & persID & " order by company, department, lastname, firstname, Datein, Timein1"
        dread = cm.ExecuteReader
        If dread.HasRows Then
            hours = "<table width = '100%'>"
            Do While dread.Read()
                If first Then
                    dept = dread("Department")
                    tempDate = CDate(dread("DateIn"))
                    hours = hours & "<tr><th colspan = 8 align=left>" & dread("Department") & "</th></tr>"
                    hours = hours & "<tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Total</th></tr>"
                    first = False
                End If
                hours = hours & "<tr><td>" & DateDiff(DateInterval.Day, CDate(tempDate.ToShortDateString), CDate(CDate(dread("DateIn")).ToShortDateString)) & "</td></tr>"
                If DateDiff(DateInterval.Day, CDate(tempDate.ToShortDateString), CDate(CDate(dread("DateIn")).ToShortDateString)) = 7 Then
                    If tempHours > 40 Then
                        pOTTotal = Round(pOTTotal + (tempHours - 40), 1)
                        dOTTotal = Round(dOTTotal + (tempHours - 40), 1)
                        cOTTotal = Round(cOTTotal + (tempHours - 40), 1)
                        gOTTotal = Round(gOTTotal + (tempHours - 40), 1)
                    End If
                    tempHours = 0
                    tempDate = CDate(dread("DateIn"))
                End If
                'Write Totals
                If dept <> dread("Department") Then
                    If tempHours > 40 Then
                        pOTTotal = Round(pOTTotal + (tempHours - 40), 1)
                        dOTTotal = Round(dOTTotal + (tempHours - 40), 1)
                        cOTTotal = Round(cOTTotal + (tempHours - 40), 1)
                        gOTTotal = Round(gOTTotal + (tempHours - 40), 1)
                    End If
                    'Department Totals
                    hours = hours & "<tr><th colspan = 4  align=left>" & dept & " Department Totals</th>"
                    hours = hours & "</tr>"
                    hours = hours & "<tr><th colspan = 7></th>"
                    hours = hours & "<th colspan = 2 align = right>PAL:</th><th>" & dPALTotal & "</th></tr>"
                    hours = hours & "<tr><th colspan = 7></th>"
                    hours = hours & "<th colspan = 2 align = right>JURY DUTY:</th><th>" & dJURYTotal & "</th></tr>"
                    hours = hours & "<tr><th colspan = 7></th>"
                    hours = hours & "<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & dBERTotal & "</th></tr>"
                    hours = hours & "<tr><th colspan = 7></th>"
                    hours = hours & "<th colspan = 2 align = right>SSLB:</th><th>" & dSSLBTotal & "</th></tr>"
                    hours = hours & "<tr><th colspan = 7></th>"
                    hours = hours & "<th colspan = 2 align = right>WORKED:</th><th> " & dWORKTotal - dOTTotal & "</th></tr>"
                    hours = hours & "<tr><th colspan = 7></th>"
                    hours = hours & "<th colspan = 2 align = right>OVERTIME:</th><th> " & dOTTotal & "</th></tr>"
                    hours = hours & "<tr><td colspan = 8 align=left>&nbsp;</td></tr>"
                    For i = 0 To 2
                        dTotal(i) = 0
                    Next
                    dPALTotal = 0
                    dJURYTotal = 0
                    dBERTotal = 0
                    dSSLBTotal = 0
                    dWORKTotal = 0
                    dOTTotal = 0
                    hours = hours & "<tr><th colspan = 8 align=left>" & dread("Department") & "</th></tr>"
                    hours = hours & "<tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Time In</th><th>Time Out</th><th>Type</th><th>Sub Total</th><th>Total</th></tr>"
                    dept = dread("Department")
                    tempDate = CDate(dread("DateIn"))
                End If

                hours = hours & "<tr>"
                hours = hours & "<td>" & dread("DateIn") & "</td>"
                If Not (IsDBNull(dread("TimeIn1"))) Then
                    hours = hours & "<td align = center>" & Right(dread("TimeIn1"), Len(dread("TimeIn1").ToString) - InStr(dread("TimeIn1").ToString, " ")) & "</td>"
                Else
                    hours = hours & "<td align = center>N/A</td>"
                End If
                If dread("TimeOut1") & "" <> "" Then
                    hours = hours & "<td align = center>" & Right(dread("TimeOut1"), Len(dread("TimeOut1").ToString) - InStr(dread("TimeOut1").ToString, " ")) & "</td>"
                Else
                    hours = hours & "<td align = center>N/A</td>"
                End If
                If dread("PUnchType") & "" <> "" Then
                    hours = hours & "<td align = center>" & dread("PunchType") & "</td>"
                Else
                    hours = hours & "<td align = center>N/A</td>"
                End If
                If dread("Diff1") & "" <> "" Then
                    hours = hours & "<td align = center>" & dread("Diff1Rd") & "</td>"
                    pTotal(0) = pTotal(0) + dread("Diff1Rd")
                    dTotal(0) = dTotal(0) + dread("Diff1Rd")
                    cTotal(0) = cTotal(0) + dread("Diff1Rd")
                    gTotal(0) = gTotal(0) + dread("Diff1Rd")
                    Select Case CStr(dread("PunchType"))
                        Case "PAL"
                            dPALTotal += dread("Diff1Rd")
                            pPALTotal += dread("Diff1Rd")
                            cPALTotal += dread("Diff1Rd")
                            gPALTotal += dread("Diff1Rd")
                        Case "Jury Duty"
                            pJURYTotal += dread("Diff1Rd")
                            dJURYTotal += dread("Diff1Rd")
                            cJURYTotal += dread("Diff1Rd")
                            gJURYTotal += dread("Diff1Rd")
                        Case "Bereavement"
                            pBERTotal += dread("Diff1Rd")
                            dBERTotal += dread("Diff1Rd")
                            cBERTotal += dread("Diff1Rd")
                            gBERTotal += dread("Diff1Rd")
                        Case "SSLB"
                            pSSLBTotal += dread("Diff1Rd")
                            dSSLBTotal += dread("Diff1Rd")
                            cSSLBTotal += dread("Diff1Rd")
                            gSSLBTotal += dread("Diff1Rd")
                        Case Else
                            pWORKTotal += dread("Diff1Rd")
                            dWORKTotal += dread("Diff1Rd")
                            cWORKTotal += dread("Diff1Rd")
                            gWORKTotal += dread("Diff1Rd")
                            tempHours += dread("Diff1Rd")
                    End Select
                Else
                    hours = hours & "<td align = center>0</td>"
                End If
                If dread("TimeIn2") & "" <> "" Then
                    hours = hours & "<td align = center>" & Right(dread("TimeIn2"), Len(dread("TimeIn2").ToString) - InStr(dread("TimeIn2").ToString, " ")) & "</td>"
                Else
                    hours = hours & "<td align = center>N/A</td>"
                End If
                If dread("TimeOut2") & "" <> "" Then
                    hours = hours & "<td align = center>" & Right(dread("TimeOut2"), Len(dread("TimeOut2").ToString) - InStr(dread("TimeOut2").ToString, " ")) & "</td>"
                Else
                    hours = hours & "<td align = center>N/A</td>"
                End If
                If dread("PUnchType2") & "" <> "" Then
                    hours = hours & "<td align = center>" & dread("PunchType2") & "</td>"
                Else
                    hours = hours & "<td align = center>N/A</td>"
                End If
                If dread("Diff2") & "" <> "" Then
                    hours = hours & "<td align = center>" & dread("Diff2Rd") & "</td>"
                    pTotal(1) = pTotal(1) + dread("Diff2Rd")
                    dTotal(1) = dTotal(1) + dread("Diff2Rd")
                    cTotal(1) = cTotal(1) + dread("Diff2Rd")
                    gTotal(1) = gTotal(1) + dread("Diff2Rd")
                    Select Case CStr(dread("PunchType2"))
                        Case "PAL"
                            dPALTotal += dread("Diff2Rd")
                            pPALTotal += dread("Diff2Rd")
                            cPALTotal += dread("Diff2Rd")
                            gPALTotal += dread("Diff2Rd")
                        Case "Jury Duty"
                            pJURYTotal += dread("Diff2Rd")
                            dJURYTotal += dread("Diff2Rd")
                            cJURYTotal += dread("Diff2Rd")
                            gJURYTotal += dread("Diff2Rd")
                        Case "Bereavement"
                            pBERTotal += dread("Diff2Rd")
                            dBERTotal += dread("Diff2Rd")
                            cBERTotal += dread("Diff2Rd")
                            gBERTotal += dread("Diff2Rd")
                        Case "SSLB"
                            pSSLBTotal += dread("Diff2Rd")
                            dSSLBTotal += dread("Diff2Rd")
                            cSSLBTotal += dread("Diff2Rd")
                            gSSLBTotal += dread("Diff2Rd")
                        Case Else
                            pWORKTotal += dread("Diff2Rd")
                            dWORKTotal += dread("Diff2Rd")
                            cWORKTotal += dread("Diff2Rd")
                            gWORKTotal += dread("Diff2Rd")
                            tempHours += dread("Diff2Rd")
                    End Select
                Else
                    hours = hours & "<td align = center>0</td>"
                End If
                If dread("Total") & "" <> "" Then
                    hours = hours & "<td align = center>" & dread("Diff1Rd") + dread("Diff2Rd") & "</td>"
                    pTotal(2) += Round(dread("Total") / 60, 1)
                    dTotal(2) += Round(dread("Total") / 60, 1)
                    cTotal(2) += Round(dread("Total") / 60, 1)
                    gTotal(2) += Round(dread("Total") / 60, 1)
                Else
                    If dread("Diff1") & "" <> "" Then
                        hours = hours & "<td align = center>" & dread("Diff1Rd") & "</td>"
                        pTotal(2) += dread("Diff1Rd")
                        dTotal(2) += dread("Diff1Rd")
                        cTotal(2) += dread("Diff1Rd")
                        gTotal(2) += dread("Diff1Rd")
                    ElseIf dread("Diff2") & "" <> "" Then
                        hours = hours & "<td align = center>" & dread("Diff2Rd") & "</td>"
                        pTotal(2) += dread("Diff2Rd")
                        dTotal(2) += dread("Diff2Rd")
                        cTotal(2) += dread("Diff2Rd")
                        gTotal(2) += dread("Diff2Rd")
                    Else
                        hours = hours & "<td align = center>0</td>"
                    End If
                End If
                hours = hours & "</td>"
            Loop
            'Personnel Totals
            If tempHours > 40 Then
                pOTTotal += (tempHours - 40)
                gOTTotal = gOTTotal + (tempHours - 40)
                dOTTotal = dOTTotal + (tempHours - 40)
            End If

            'Department Totals
            hours = hours & "<tr><th colspan = 4  align=left>" & dept & " Department Totals</th>"
            hours = hours & "</tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>PAL:</th><th>" & dPALTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>JURY DUTY:</th><th>" & dJURYTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & dBERTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>SSLB:</th><th>" & dSSLBTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>WORKED:</th><th> " & dWORKTotal - dOTTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>OVERTIME:</th><th> " & dOTTotal & "</th></tr>"
            hours = hours & "<tr><td colspan = 8 align=left>&nbsp;</td></tr>"

            'Grand Totals
            hours = hours & "<tr><th colspan=8>&nbsp;</th></tr>"
            hours = hours & "<tr><th colspan=4 align=left>Grand Totals:</th>"
            hours = hours & "</tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>PAL:</th><th>" & gPALTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>JURY DUTY:</th><th>" & gJURYTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>BEREAVEMENT:</th><th>" & gBERTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>SSLB:</th><th>" & gSSLBTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>WORKED:</th><th> " & gWORKTotal - gOTTotal & "</th></tr>"
            hours = hours & "<tr><th colspan = 7></th>"
            hours = hours & "<th colspan = 2 align = right>OVERTIME:</th><th> " & gOTTotal & "</th></tr>"
            hours = hours & "</table>"
        Else
            hours = "No Records For This Date Range."
        End If
        dread.Close()
        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return hours
    End Function

    Public Function Wipe_Punch(ByVal punchID As Integer, ByVal punchIN As Boolean) As Boolean
        Dim bWiped As Boolean = False
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        If punchIN Then
            cm.CommandText = "Update t_PersonnelPunch set TimeIn = NULL, DateIn = NULL, InManDate = NULL where punchID = " & punchID
        Else
            cm.CommandText = "Update t_PersonnelPunch set TimeOut = NULL, DateOut = NULL, OutManDate = NULL where punchID = " & punchID
        End If
        cm.ExecuteNonQuery()
        bWiped = True
        ' Catch ex As Exception
        '_Err = ex.Message
        ' bWiped = False
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bWiped
    End Function

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property TimeIn() As String
        Get
            Return _TimeIn
        End Get
        Set(ByVal value As String)
            _TimeIn = value
        End Set
    End Property

    Public Property TimeOut() As String
        Get
            Return _TimeOut
        End Get
        Set(ByVal value As String)
            _TimeOut = value
        End Set
    End Property

    Public Property DateIn() As String
        Get
            Return _DateIn
        End Get
        Set(ByVal value As String)
            _DateIn = value
        End Set
    End Property

    Public Property DateOut() As String
        Get
            Return _DateOut
        End Get
        Set(ByVal value As String)
            _DateOut = value
        End Set
    End Property

    Public Property InManual() As Boolean
        Get
            Return _InManual
        End Get
        Set(ByVal value As Boolean)
            _InManual = value
        End Set
    End Property

    Public Property InUserID() As Integer
        Get
            Return _InUserID
        End Get
        Set(ByVal value As Integer)
            _InUserID = value
        End Set
    End Property

    Public Property OutManual() As Boolean
        Get
            Return _OutManual
        End Get
        Set(ByVal value As Boolean)
            _OutManual = value
        End Set
    End Property

    Public Property OutUserID() As Integer
        Get
            Return _OutUserID
        End Get
        Set(ByVal value As Integer)
            _OutUserID = value
        End Set
    End Property

    Public Property InManDate() As String
        Get
            Return _InManDate
        End Get
        Set(ByVal value As String)
            _InManDate = value
        End Set
    End Property

    Public Property OutManDate() As String
        Get
            Return _OutManDate
        End Get
        Set(ByVal value As String)
            _OutManDate = value
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

    Public Property InClockID() As Integer
        Get
            Return _InClockID
        End Get
        Set(ByVal value As Integer)
            _InClockID = value
        End Set
    End Property

    Public Property OutClockID() As Integer
        Get
            Return _OutClockID
        End Get
        Set(ByVal value As Integer)
            _OutClockID = value
        End Set
    End Property

    Public Property PunchTypeID() As Integer
        Get
            Return _PunchTypeID
        End Get
        Set(ByVal value As Integer)
            _PunchTypeID = value
        End Set
    End Property
    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property
    Public Property PunchID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property
End Class
