Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web.UI.WebControls

Public Class clsPersonnel
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _UserName As String = ""
    Dim _ADSID As String = ""
    Dim _Password As String = ""
    Dim _Active As Boolean = False
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _MiddleInit As String = ""
    Dim _ActAsTO As Boolean = False
    Dim _CRMSID As Integer = 0
    Dim _HireDate As String = ""
    Dim _TermDate As String = ""
    Dim _Address As String = ""
    Dim _City As String = ""
    Dim _State As Integer = 0
    Dim _Zip As String = ""
    Dim _StatusID As Integer = 0
    Dim _Email As String = ""
    Dim _PagerEmail As String = ""
    Dim _CellEmail As String = ""
    Dim _PrimaryContactMethod As String = ""
    Dim _SecondaryContactMethod As String = ""
    Dim _ReserveLimit As Decimal = 0
    Dim _SSN As String = ""
    Dim _CellPhone As String = ""
    Dim _HomePhone As String = ""
    Dim _Fax As String = ""
    Dim _BirthDate As String = ""
    Dim _DriversLicense As String = ""
    Dim _SpouseName As String = ""
    Dim _SalesRotorTypeID As Integer = 0
    Dim _ADPID As String = ""
    Dim _ReportsToID As Integer = 0
    Dim _PasswordExpirationDate As String = ""
    Dim _PasswordForceChange As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Personnel where PersonnelID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Personnel where PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Personnel")
            If ds.Tables("t_Personnel").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("UserName") Is System.DBNull.Value) Then _UserName = dr("UserName")
        If Not (dr("ADSID") Is System.DBNull.Value) Then _ADSID = dr("ADSID")
        If Not (dr("Password") Is System.DBNull.Value) Then _Password = dr("Password")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("MiddleInit") Is System.DBNull.Value) Then _MiddleInit = dr("MiddleInit")
        If Not (dr("ActAsTO") Is System.DBNull.Value) Then _ActAsTO = dr("ActAsTO")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("HireDate") Is System.DBNull.Value) Then _HireDate = dr("HireDate")
        If Not (dr("TermDate") Is System.DBNull.Value) Then _TermDate = dr("TermDate")
        If Not (dr("Address") Is System.DBNull.Value) Then _Address = dr("Address")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("State") Is System.DBNull.Value) Then _State = dr("State")
        If Not (dr("Zip") Is System.DBNull.Value) Then _Zip = dr("Zip")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("Email") Is System.DBNull.Value) Then _Email = dr("Email")
        If Not (dr("PagerEmail") Is System.DBNull.Value) Then _PagerEmail = dr("PagerEmail")
        If Not (dr("CellEmail") Is System.DBNull.Value) Then _CellEmail = dr("CellEmail")
        If Not (dr("PrimaryContactMethod") Is System.DBNull.Value) Then _PrimaryContactMethod = dr("PrimaryContactMethod")
        If Not (dr("SecondaryContactMethod") Is System.DBNull.Value) Then _SecondaryContactMethod = dr("SecondaryContactMethod")
        If Not (dr("ReserveLimit") Is System.DBNull.Value) Then _ReserveLimit = dr("ReserveLimit")
        If Not (dr("SSN") Is System.DBNull.Value) Then _SSN = dr("SSN")
        If Not (dr("CellPhone") Is System.DBNull.Value) Then _CellPhone = dr("CellPhone")
        If Not (dr("HomePhone") Is System.DBNull.Value) Then _HomePhone = dr("HomePhone")
        If Not (dr("Fax") Is System.DBNull.Value) Then _Fax = dr("Fax")
        If Not (dr("BirthDate") Is System.DBNull.Value) Then _BirthDate = dr("BirthDate")
        If Not (dr("DriversLicense") Is System.DBNull.Value) Then _DriversLicense = dr("DriversLicense")
        If Not (dr("SpouseName") Is System.DBNull.Value) Then _SpouseName = dr("SpouseName")
        If Not (dr("SalesRotorTypeID") Is System.DBNull.Value) Then _SalesRotorTypeID = dr("SalesRotorTypeID")
        If Not (dr("ReportsToID") Is System.DBNull.Value) Then _ReportsToID = dr("ReportsToID")
        If Not (dr("ADPID") Is System.DBNull.Value) Then _ADPID = dr("ADPID")
        If Not (dr("PasswordExpirationDate") Is System.DBNull.Value) Then _PasswordExpirationDate = dr("PasswordExpirationDate")
        If Not (dr("PasswordForceChange") Is System.DBNull.Value) Then _PasswordForceChange = dr("PasswordForceChange")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel where PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Personnel")
            If ds.Tables("t_Personnel").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 0, "UserName")
                da.InsertCommand.Parameters.Add("@ADSID", SqlDbType.VarChar, 0, "ADSID")
                da.InsertCommand.Parameters.Add("@Password", SqlDbType.VarChar, 0, "Password")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@MiddleInit", SqlDbType.Char, 0, "MiddleInit")
                da.InsertCommand.Parameters.Add("@ActAsTO", SqlDbType.Bit, 0, "ActAsTO")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@HireDate", SqlDbType.DateTime, 0, "HireDate")
                da.InsertCommand.Parameters.Add("@TermDate", SqlDbType.DateTime, 0, "TermDate")
                da.InsertCommand.Parameters.Add("@Address", SqlDbType.VarChar, 0, "Address")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 0, "City")
                da.InsertCommand.Parameters.Add("@State", SqlDbType.Int, 0, "State")
                da.InsertCommand.Parameters.Add("@Zip", SqlDbType.VarChar, 0, "Zip")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@Email", SqlDbType.VarChar, 0, "Email")
                da.InsertCommand.Parameters.Add("@PagerEmail", SqlDbType.VarChar, 0, "PagerEmail")
                da.InsertCommand.Parameters.Add("@CellEmail", SqlDbType.VarChar, 0, "CellEmail")
                da.InsertCommand.Parameters.Add("@PrimaryContactMethod", SqlDbType.VarChar, 0, "PrimaryContactMethod")
                da.InsertCommand.Parameters.Add("@SecondaryContactMethod", SqlDbType.VarChar, 0, "SecondaryContactMethod")
                da.InsertCommand.Parameters.Add("@ReserveLimit", SqlDbType.Money, 0, "ReserveLimit")
                da.InsertCommand.Parameters.Add("@SSN", SqlDbType.VarChar, 0, "SSN")
                da.InsertCommand.Parameters.Add("@CellPhone", SqlDbType.VarChar, 0, "CellPhone")
                da.InsertCommand.Parameters.Add("@HomePhone", SqlDbType.VarChar, 0, "HomePhone")
                da.InsertCommand.Parameters.Add("@Fax", SqlDbType.VarChar, 0, "Fax")
                da.InsertCommand.Parameters.Add("@BirthDate", SqlDbType.DateTime, 0, "BirthDate")
                da.InsertCommand.Parameters.Add("@DriversLicense", SqlDbType.VarChar, 0, "DriversLicense")
                da.InsertCommand.Parameters.Add("@SpouseName", SqlDbType.VarChar, 0, "SpouseName")
                da.InsertCommand.Parameters.Add("@SalesRotorTypeID", SqlDbType.Int, 0, "SalesRotorTypeID")
                da.InsertCommand.Parameters.Add("@ADPID", SqlDbType.VarChar, 0, "ADPID")
                da.InsertCommand.Parameters.Add("@ReportsToID", SqlDbType.Int, 0, "ReportsToID")
                da.InsertCommand.Parameters.Add("@PasswordExpirationDate", SqlDbType.DateTime, 0, "PasswordExpirationDate")
                da.InsertCommand.Parameters.Add("@PasswordForceChange", SqlDbType.Bit, 0, "PasswordForceChange")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.Int, 0, "PersonnelID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Personnel").NewRow
            End If
            Update_Field("UserName", _UserName, dr)
            Update_Field("ADSID", _ADSID, dr)
            Update_Field("Password", _Password, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("MiddleInit", _MiddleInit, dr)
            Update_Field("ActAsTO", _ActAsTO, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("HireDate", _HireDate, dr)
            Update_Field("TermDate", _TermDate, dr)
            Update_Field("Address", _Address, dr)
            Update_Field("City", _City, dr)
            Update_Field("State", _State, dr)
            Update_Field("Zip", _Zip, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("Email", _Email, dr)
            Update_Field("PagerEmail", _PagerEmail, dr)
            Update_Field("CellEmail", _CellEmail, dr)
            Update_Field("PrimaryContactMethod", _PrimaryContactMethod, dr)
            Update_Field("SecondaryContactMethod", _SecondaryContactMethod, dr)
            Update_Field("ReserveLimit", _ReserveLimit, dr)
            Update_Field("SSN", _SSN, dr)
            Update_Field("CellPhone", _CellPhone, dr)
            Update_Field("HomePhone", _HomePhone, dr)
            Update_Field("Fax", _Fax, dr)
            Update_Field("BirthDate", _BirthDate, dr)
            Update_Field("DriversLicense", _DriversLicense, dr)
            Update_Field("SpouseName", _SpouseName, dr)
            Update_Field("SalesRotorTypeID", _SalesRotorTypeID, dr)
            Update_Field("ADPID", _ADPID, dr)
            Update_Field("ReportsToID", _ReportsToID, dr)
            Update_Field("PasswordExpirationDate", _PasswordExpirationDate, dr)
            Update_Field("PasswordForceChange", _PasswordForceChange, dr)
            If ds.Tables("t_Personnel").Rows.Count < 1 Then ds.Tables("t_Personnel").Rows.Add(dr)
            da.Update(ds, "t_Personnel")
            If _ID = 0 Then
                Dim oEvents As New clsEvents
                oEvents.Create_Create_Event("PersonnelID", ds.Tables("t_Personnel").Rows(0).Item("PersonnelID"), 0, _UserID, "")
                oEvents = Nothing
            End If
            _ID = ds.Tables("t_Personnel").Rows(0).Item("PersonnelID")
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
            oEvents.KeyField = "PersonnelID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Active_Accounts() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select personnelid as ID, lastname + ', ' + firstname as Name from t_Personnel where adsid <> '' and adsid is not null order by lastname, firstname"
        Catch ex As Exception
            _Err = ex.ToString
        End Try

        Return ds
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Sub Lookup_User(ByVal PersonnelID As Integer)
        _ID = PersonnelID
        Load()
    End Sub


    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.PersonnelID as ID, p.LastName + ', ' + p.FirstName as Name from t_Personnel p inner join t_CombOItems ps on p.StatusID = ps.ComboItemID where ps.CombOitem like 'Active%' order by LastName, Firstname"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List_Active_Flag() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.PersonnelID as ID, p.LastName + ', ' + p.FirstName as Name from t_Personnel p where p.active = 1 order by LastName, Firstname"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List_Sales_Reps(ByVal loc As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If loc = "Woodbridge" Then
                ds.SelectCommand = "SELECT DISTINCT p.PersonnelID, p.LastName + ', ' + p.FirstName AS Personnel FROM t_Personnel p LEFT OUTER JOIN t_Personnel2Dept pd ON pd.PersonnelID = p.PersonnelID LEFT OUTER JOIN t_ComboItems d ON pd.DepartmentID = d.ComboItemID WHERE (pd.Active = '1') AND ((d.ComboItem = 'Offsite Sales')) OR (p.FirstName = 'House') or (p.FirstName = 'Podium') ORDER BY personnel"
            Else
                ds.SelectCommand = "SELECT DISTINCT p.PersonnelID, p.LastName + ', ' + p.FirstName AS Personnel FROM t_Personnel p LEFT OUTER JOIN t_Personnel2Dept pd ON pd.PersonnelID = p.PersonnelID LEFT OUTER JOIN t_ComboItems d ON pd.DepartmentID = d.ComboItemID WHERE (pd.Active = '1') AND ((d.ComboItem = 'In-House') OR (d.ComboItem = 'Dayline 1')) OR (p.FirstName = 'House') or (p.Firstname = 'Podium') ORDER BY personnel"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_TOs() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select Distinct p.PersonnelID, p.LastName + ', ' + p.FirstName as Personnel from t_Personnel p inner join t_CombOitems ps on p.StatusID = ps.CombOitemID where ps.Comboitem = 'Active' and p.actasto = '1'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Reps_For_Department(ByVal sDept As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select b.Lastname + ', ' + b.firstname as Name,b.FirstName, b.LastName, a.PersonnelID from t_Personnel2Dept a inner join t_Personnel b on a.personnelid = b.personnelid where a.departmentid = (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'Department' and comboitem = '" & sDept & "') and a.active = '1' order by b.lastname asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Exit_Reps() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select Distinct p.PersonnelID, p.LastName + ', ' + p.FirstName as Personnel From t_Personnel p inner join t_Personnel2Teams pt on p.PersonnelID = pt.PersonnelID inner join t_combOitems ps on p.StatusID = ps.ComboItemID left outer join t_ComboItems c on pt.TeamID = c.ComboItemID where c.ComboItem = 'Exit' and ps.ComboItem = 'Active' order by personnel"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_OPC_Reps() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "SELECT DISTINCT p.PersonnelID, p.LastName + ', ' + p.FirstName AS Personnel FROM t_Personnel p LEFT OUTER JOIN t_Personnel2Dept pd ON pd.PersonnelID = p.PersonnelID LEFT OUTER JOIN t_ComboItems d ON pd.DepartmentID = d.ComboItemID WHERE (pd.Active = '1') AND (d.ComboItem LIKE 'OPC%') ORDER BY personnel"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Get_Personnel_By_Dept(ByVal dept As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "SELECT DISTINCT p.PersonnelID, p.LastName + ', ' + p.FirstName AS Personnel FROM t_Personnel p LEFT OUTER JOIN t_Personnel2Dept pd ON pd.PersonnelID = p.PersonnelID LEFT OUTER JOIN t_ComboItems d ON pd.DepartmentID = d.ComboItemID WHERE (pd.Active = '1') AND (d.ComboItem = '" & dept & "') ORDER BY personnel"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Lookup_By_Name(ByVal fName As String, ByVal lName As String) As Integer
        Dim persID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select PersonnelID from t_Personnel where Firstname = '" & fName.Replace(New Char() {"'"}, "''") & "' and LastName = '" & lName.Replace(New Char() {"'"}, "''") & "'"
            dread = cm.ExecuteReader
            dread.Read()
            persID = dread("PersonnelID")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return persID
    End Function
    Public Function Lookup_By_FullName(ByVal fName As String, ByVal lName As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.PersonnelID as ID, p.FirstName, p.LastName, ps.ComboItem as Status from t_Personnel p left outer join t_comboItems ps on p.StatusID = ps.ComboitemID where p.Firstname = '" & fName.Replace(New Char() {"'"}, "''") & "' and p.LastName = '" & lName.Replace(New Char() {"'"}, "''") & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Lookup_By_ID(ByVal persID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.PersonnelID as ID, p.FirstName, p.LastName, ps.ComboItem as Status from t_Personnel p left outer join t_comboItems ps on p.StatusID = ps.ComboitemID where p.PersonnelID = '" & persID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function get_PersID(ByVal userName As String) As Integer
        Dim persID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel where UserName = '" & userName & "'"
            dread = cm.ExecuteReader
            dread.Read()
            persID = dread("PersonnelID")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return persID
    End Function

    Public Function List_Managers() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "select * from (Select 0 as Personnelid, '' as Name union select p.personnelid, p.LastName + ', ' + p.FirstName as Name from t_Personnel p inner join t_Personnel2dept d on d.personnelid = p.personnelid inner join t_comboItems ps on p.StatusID = ps.ComboItemID where isManager = 1 and d.active = 1 and ps.ComboItem = 'Active') a order by name"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function chk_Mgr_Status(ByVal personnelID As Integer) As Boolean
        Dim bMgr As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel2Dept where personnelid = '" & personnelID & "' and isManager = 1 and active = 1"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                bMgr = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bMgr
    End Function
    Public Function Validate_PersID(ByVal personnelID As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel where personnelID = " & personnelID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                bValid = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    ''' <summary>
    ''' Get Personnel ID, given ADSID as the parameter
    ''' </summary> 
    Public Function Get_PersonnelID_By_SID(sid As String) As Integer

        Dim id = 0
        Try

            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "select PersonnelID from t_personnel where ADSID = '" & sid & "'"

            id = DirectCast(cm.ExecuteScalar(), Integer)

        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try

        Return id
    End Function


    ''' <summary>
    ''' Get Department ID, given id of the person and company as the parameters
    ''' </summary>
    Public Function Get_DepartmentID_By_PersonnelID(pers_id As Integer, comp_id As Integer) As Integer

        Dim id = 0
        Try

            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "select DepartmentID from t_personnel2dept where personnelid = " & pers_id & " and companyid = " & comp_id

            id = DirectCast(cm.ExecuteScalar(), Integer)

        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try

        Return id
    End Function

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Public Property ADSID() As String
        Get
            Return _ADSID
        End Get
        Set(ByVal value As String)
            _ADSID = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Public Property PasswordForceChange As Boolean
        Get
            Return _PasswordForceChange
        End Get
        Set(ByVal value As Boolean)
            _PasswordForceChange = value
        End Set
    End Property

    Public Property PasswordExpirationDate As String
        Get
            Return _PasswordExpirationDate
        End Get
        Set(ByVal value As String)
            _PasswordExpirationDate = value
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

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    Public Property ReportsToID As Integer
        Get
            Return _ReportsToID
        End Get
        Set(ByVal value As Integer)
            _ReportsToID = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    Public Property MiddleInit() As String
        Get
            Return _MiddleInit
        End Get
        Set(ByVal value As String)
            _MiddleInit = value
        End Set
    End Property

    Public Property ActAsTO() As Boolean
        Get
            Return _ActAsTO
        End Get
        Set(ByVal value As Boolean)
            _ActAsTO = value
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

    Public Property HireDate() As String
        Get
            Return _HireDate
        End Get
        Set(ByVal value As String)
            _HireDate = value
        End Set
    End Property

    Public Property TermDate() As String
        Get
            Return _TermDate
        End Get
        Set(ByVal value As String)
            _TermDate = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(ByVal value As String)
            _Address = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Public Property State() As Integer
        Get
            Return _State
        End Get
        Set(ByVal value As Integer)
            _State = value
        End Set
    End Property

    Public Property Zip() As String
        Get
            Return _Zip
        End Get
        Set(ByVal value As String)
            _Zip = value
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

    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

    Public Property PagerEmail() As String
        Get
            Return _PagerEmail
        End Get
        Set(ByVal value As String)
            _PagerEmail = value
        End Set
    End Property

    Public Property CellEmail() As String
        Get
            Return _CellEmail
        End Get
        Set(ByVal value As String)
            _CellEmail = value
        End Set
    End Property

    Public Property PrimaryContactMethod() As String
        Get
            Return _PrimaryContactMethod
        End Get
        Set(ByVal value As String)
            _PrimaryContactMethod = value
        End Set
    End Property

    Public Property SecondaryContactMethod() As String
        Get
            Return _SecondaryContactMethod
        End Get
        Set(ByVal value As String)
            _SecondaryContactMethod = value
        End Set
    End Property

    Public Property ReserveLimit() As Decimal
        Get
            Return _ReserveLimit
        End Get
        Set(ByVal value As Decimal)
            _ReserveLimit = value
        End Set
    End Property

    Public Property SSN() As String
        Get
            Return _SSN
        End Get
        Set(ByVal value As String)
            _SSN = value
        End Set
    End Property

    Public Property CellPhone() As String
        Get
            Return _CellPhone
        End Get
        Set(ByVal value As String)
            _CellPhone = value
        End Set
    End Property

    Public Property HomePhone() As String
        Get
            Return _HomePhone
        End Get
        Set(ByVal value As String)
            _HomePhone = value
        End Set
    End Property

    Public Property Fax() As String
        Get
            Return _Fax
        End Get
        Set(ByVal value As String)
            _Fax = value
        End Set
    End Property

    Public Property BirthDate() As String
        Get
            Return _BirthDate
        End Get
        Set(ByVal value As String)
            _BirthDate = value
        End Set
    End Property

    Public Property DriversLicense() As String
        Get
            Return _DriversLicense
        End Get
        Set(ByVal value As String)
            _DriversLicense = value
        End Set
    End Property

    Public Property SpouseName() As String
        Get
            Return _SpouseName
        End Get
        Set(ByVal value As String)
            _SpouseName = value
        End Set
    End Property

    Public Property SalesRotorTypeID() As Integer
        Get
            Return _SalesRotorTypeID
        End Get
        Set(ByVal value As Integer)
            _SalesRotorTypeID = value
        End Set
    End Property

    Public Property ADPID() As String
        Get
            Return _ADPID
        End Get
        Set(ByVal value As String)
            _ADPID = value
        End Set
    End Property

    Public Property PersonnelID() As Integer
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
