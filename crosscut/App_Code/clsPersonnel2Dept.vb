Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnel2Dept
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _isManager As Boolean = False
    Dim _clockIn As Boolean = False
    Dim _CompanyID As Integer = 0
    Dim _Active As Boolean = False
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
        cm = New SqlCommand("Select * from t_Personnel2Dept where Personnel2Dept = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Personnel2Dept where Personnel2Dept = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Personnel2Dept")
            If ds.Tables("t_Personnel2Dept").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel2Dept").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("isManager") Is System.DBNull.Value) Then _isManager = dr("isManager")
        If Not (dr("clockIn") Is System.DBNull.Value) Then _clockIn = dr("clockIn")
        If Not (dr("CompanyID") Is System.DBNull.Value) Then _CompanyID = dr("CompanyID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel2Dept where Personnel2Dept = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Personnel2Dept")
            If ds.Tables("t_Personnel2Dept").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel2Dept").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Personnel2DeptInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@isManager", SqlDbType.bit, 0, "isManager")
                da.InsertCommand.Parameters.Add("@clockIn", SqlDbType.bit, 0, "clockIn")
                da.InsertCommand.Parameters.Add("@CompanyID", SqlDbType.int, 0, "CompanyID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Personnel2Dept", SqlDbType.Int, 0, "Personnel2Dept")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Personnel2Dept").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("isManager", _isManager, dr)
            Update_Field("clockIn", _clockIn, dr)
            Update_Field("CompanyID", _CompanyID, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Personnel2Dept").Rows.Count < 1 Then ds.Tables("t_Personnel2Dept").Rows.Add(dr)
            da.Update(ds, "t_Personnel2Dept")
            _ID = ds.Tables("t_Personnel2Dept").Rows(0).Item("Personnel2Dept")
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
            oEvents.KeyField = "Personnel2Dept"
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

    Public Function List_Depts(ByVal PersID As Integer) As SQLDataSOurce
        Dim ds As New SQLDataSOurce
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.Personnel2Dept As ID, d.ComboItem as Department, p.IsManager, p.clockIn, c.Comboitem as Company, p.Active from t_Personnel2Dept p inner join t_ComboItems d on p.DepartmentID = d.ComboItemID inner join t_ComboItems c on p.CompanyID = c.ComboItemID where p.PersonnelID = " & PersID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Depts(ByVal persID As Integer) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.DepartmentID, d.ComboItem as Department from t_personnel2Dept p inner join t_ComboItems d on p.Departmentid = d.ComboItemID where p.PersonnelID = " & persID & " and p.Active = '1' and p.ClockIn = '1' order by d.ComboItem asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function


    '*** Created 2nd Function to list depts for workorders since they will need all members regardless of clockin
    '**** Didnt want to go back and edit all timeclock pages for this release
    '**** Next release add a clock In boolean to eliminate this function if youd like
    Public Function Get_My_Depts(ByVal persID As Integer) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.DepartmentID, d.ComboItem as Department from t_personnel2Dept p inner join t_ComboItems d on p.Departmentid = d.ComboItemID where p.PersonnelID = " & persID & " and p.Active = '1' order by d.ComboItem asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_My_Dept_Members(ByVal persID As Integer) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "SELECT DISTINCT p2.PersonnelID, p2.FirstName + ' ' + p2.LastName as Personnel FROM t_Personnel2Dept pd INNER JOIN t_Personnel p ON p.PersonnelID = pd.PersonnelID INNER JOIN t_Personnel2Dept pd2 ON pd.DepartmentID = pd2.DepartmentID INNER JOIN t_Personnel p2 ON p2.PersonnelID = pd2.PersonnelID WHERE (p.PersonnelID = " & persID & ") AND (pd2.Active = '1') AND (pd.Active = '1') ORDER BY Personnel"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function


    Public Function Get_MyMgr_Dept_Members(ByVal persID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select distinct p.PersonnelID, p.FirstName + ' ' + p.LastName as Personnel from t_Personnel2Dept pd inner join t_Personnel p on p.PersonnelID = pd.PersonnelID where pd.Active = '1' and p.Active = '1' and pd.DepartmentID in (Select DepartmentID from t_Personnel2Dept where Active = '1' and isManager = '1' and personnelid = " & persID & ") order by Personnel asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Get_Manager_Emails(ByVal deptID As Integer) As String()
        Dim mgrs(0) As String
        Dim i As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select p.Email from t_Personnel p inner join t_Personnel2Dept pd on p.Personnelid = pd.Personnelid where pd.DepartmentID = " & deptID & " and pd.Active = '1' and pd.IsManager = '1' and p.Active = '1'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If mgrs(UBound(mgrs)) <> "" Then
                        i = i + 1
                        ReDim Preserve mgrs(i)
                    End If
                    mgrs(UBound(mgrs)) = dread("Email")    
                Loop
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return mgrs
    End Function
    Public Function Get_My_Supervisors(ByVal persID As Integer) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "SELECT DISTINCT p2.PersonnelID, p2.FirstName + ' ' + p2.LastName as Personnel FROM t_Personnel2Dept pd INNER JOIN t_Personnel p ON p.PersonnelID = pd.PersonnelID INNER JOIN t_Personnel2Dept pd2 ON pd.DepartmentID = pd2.DepartmentID INNER JOIN t_Personnel p2 ON p2.PersonnelID = pd2.PersonnelID WHERE (p.PersonnelID = " & persID & ") AND (pd2.Active = '1') and pd2.IsManager = '1' AND (pd.Active = '1') and p2.Active = '1' ORDER BY Personnel"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Manager_Check(ByVal persID As Integer, ByVal deptID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Depts from t_Personnel2Dept where active = '1' and isManager = '1' and personnelid = " & persID & " and departmentid = " & deptID & ""
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Depts") = 0 Then
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

    Public Function Member_Of_WOParty(ByVal partyID As Integer, ByVal persID As Integer) As Boolean
        Dim bMember As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "SELECT COUNT(p.PersonnelID) AS Depts FROM t_Personnel p INNER JOIN t_Personnel2Dept pd ON p.PersonnelID = pd.PersonnelID INNER JOIN t_WODept2Party wp ON pd.DepartmentID = wp.DepartmentID WHERE (p.PersonnelID = " & persID & ") AND (wp.ResponsiblePartyID = " & partyID & ") AND (pd.Active = '1')"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Depts") = 0 Then
                bMember = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bMember
    End Function

    Public Function Get_Dept_Count(ByVal persID As Integer) As Integer
        Dim deptCount As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when count(*) is null then 0 else Count(*) end as Departments from t_Personnel2Dept where active = '1' and clockin = '1' and personnelid = " & persID & ""
            dread = cm.ExecuteReader()
            dread.Read()
            deptCount = dread("Departments")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return deptCount
    End Function

    'For Time Clock Check In when we know there is only one Department
    Public Function Get_Active_Dept(ByVal persID As Integer) As Integer
        Dim deptID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select DepartmentID from t_Personnel2Dept where active = '1' and clockin = '1' and personnelid = " & persID & ""
            dread = cm.ExecuteReader()
            dread.Read()
            deptID = dread("DepartmentID")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return deptID
    End Function


    Public Function Member_Of(ByVal persID As Integer, ByVal dept As String) As Boolean
        Dim bMember As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Dept from t_Personnel2Dept d inner join t_ComboItems c on d.Departmentid = c.ComboItemID where d.Active = '1' and c.ComboItem = '" & dept & "' and d.PersonnelID = " & persID & ""
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Dept") = 0 Then
                bMember = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bMember = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bMember
    End Function

    Public Function Get_Members(ByVal dept As String) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.PersonnelID, p.UserName from t_Personnel p inner join t_Personnel2Dept pd on p.PersonnelID = pd.PersonnelID inner join t_ComboItems d on pd.DepartmentID = d.ComboItemID where d.ComboItem = '" & dept & "' and pd.Active = '1' order by p.UserName asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_ClockIn_Members(ByVal dept As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.PersonnelID, p.FirstName + ' ' + p.LastName as Personnel, d.ComboItem as Department, pd.DepartmentID from t_Personnel p inner join t_Personnel2Dept pd on p.PersonnelID = pd.PersonnelID inner join t_ComboItems d on pd.DepartmentID = d.ComboItemID where d.ComboItem in (" & dept & ") and pd.Active = '1' and pd.ClockIn = '1' order by p.Firstname, p.Lastname asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Company(ByVal persID As Integer, ByVal deptID As Integer) As Integer
        Dim companyID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select COmpanyID from t_Personnel2Dept where personnelid = " & persID & " and departmentid = " & deptID & " and active = 1"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                companyID = dread("CompanyID")
            End If
            dread.Close()

        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return companyID
    End Function
    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
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

    Public Property isManager() As Boolean
        Get
            Return _isManager
        End Get
        Set(ByVal value As Boolean)
            _isManager = value
        End Set
    End Property

    Public Property clockIn() As Boolean
        Get
            Return _clockIn
        End Get
        Set(ByVal value As Boolean)
            _clockIn = value
        End Set
    End Property

    Public Property CompanyID() As Integer
        Get
            Return _CompanyID
        End Get
        Set(ByVal value As Integer)
            _CompanyID = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property Personnel2Dept() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Err As String
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
