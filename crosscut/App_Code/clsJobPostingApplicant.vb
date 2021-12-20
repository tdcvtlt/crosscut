Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsJobPostingApplicant
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _MiddleName As String = ""
    Dim _NickName As String = ""
    Dim _Suffix As String = ""
    Dim _EmailAddress As String = ""
    Dim _Address As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _PostalCode As String = ""
    Dim _AddressYears As Integer = 0
    Dim _AddressMonths As Integer = 0
    Dim _SSN As Integer = 0
    Dim _HomePhone As String = ""
    Dim _MobilePhone As String = ""
    Dim _DOB As String = ""
    Dim _UserName As String = ""
    Dim _Password As String = ""
    Dim _Esig As String = ""
    Dim _SecQuestion1 As Integer = 0
    Dim _SecAnswer1 As String = ""
    Dim _SecQuestion2 As Integer = 0
    Dim _SecAnswer2 As String = ""
    Dim _SecQuestion3 As Integer = 0
    Dim _SecAnswer3 As String = ""
    Dim _Felony As Boolean = False
    Dim _FelonyDesc As String = ""
    Dim _Misdemeanor As Boolean = False
    Dim _MisdemeanorDesc As String = ""
    Dim _BackGroundAuthorize As Boolean = False
    Dim _PreviousName As Boolean = False
    Dim _PreviousFirst As String = ""
    Dim _PreviousLast As String = ""
    Dim _PreviousMiddle As String = ""
    Dim _PreviousAddress As String = ""
    Dim _PreviousCity As String = ""
    Dim _PreviousStateID As Integer = 0
    Dim _PreviousPostalCode As String = ""
    Dim _PreviousAddYears As Integer = 0
    Dim _PreviousAddMonths As Integer = 0
    Dim _PhysicalLimitations As Boolean = False
    Dim _PhysicalLimitationsDesc As String = ""
    Dim _BirthMonth As Integer = 0
    Dim _BirthDay As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_JobPostingApplicant where ApplicantID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_JobPostingApplicant where ApplicantID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingApplicant")
            If ds.Tables("t_JobPostingApplicant").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingApplicant").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("MiddleName") Is System.DBNull.Value) Then _MiddleName = dr("MiddleName")
        If Not (dr("NickName") Is System.DBNull.Value) Then _NickName = dr("NickName")
        If Not (dr("Suffix") Is System.DBNull.Value) Then _Suffix = dr("Suffix")
        If Not (dr("EmailAddress") Is System.DBNull.Value) Then _EmailAddress = dr("EmailAddress")
        If Not (dr("Address") Is System.DBNull.Value) Then _Address = dr("Address")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("PostalCode") Is System.DBNull.Value) Then _PostalCode = dr("PostalCode")
        If Not (dr("AddressYears") Is System.DBNull.Value) Then _AddressYears = dr("AddressYears")
        If Not (dr("AddressMonths") Is System.DBNull.Value) Then _AddressMonths = dr("AddressMonths")
        If Not (dr("SSN") Is System.DBNull.Value) Then _SSN = dr("SSN")
        If Not (dr("HomePhone") Is System.DBNull.Value) Then _HomePhone = dr("HomePhone")
        If Not (dr("MobilePhone") Is System.DBNull.Value) Then _MobilePhone = dr("MobilePhone")
        If Not (dr("DOB") Is System.DBNull.Value) Then _DOB = dr("DOB")
        If Not (dr("UserName") Is System.DBNull.Value) Then _UserName = dr("UserName")
        If Not (dr("Password") Is System.DBNull.Value) Then _Password = dr("Password")
        If Not (dr("ESig") Is System.DBNull.Value) Then _Esig = dr("Esig")
        If Not (dr("SecQuestion1") Is System.DBNull.Value) Then _SecQuestion1 = dr("SecQuestion1")
        If Not (dr("SecAnswer1") Is System.DBNull.Value) Then _SecAnswer1 = dr("SecAnswer1")
        If Not (dr("SecQuestion2") Is System.DBNull.Value) Then _SecQuestion2 = dr("SecQuestion2")
        If Not (dr("SecAnswer2") Is System.DBNull.Value) Then _SecAnswer2 = dr("SecAnswer2")
        If Not (dr("SecQuestion3") Is System.DBNull.Value) Then _SecQuestion3 = dr("SecQuestion3")
        If Not (dr("SecAnswer3") Is System.DBNull.Value) Then _SecAnswer3 = dr("SecAnswer3")
        If Not (dr("Felony") Is System.DBNull.Value) Then _Felony = dr("Felony")
        If Not (dr("FelonyDesc") Is System.DBNull.Value) Then _FelonyDesc = dr("FelonyDesc")
        If Not (dr("Misdemeanor") Is System.DBNull.Value) Then _Misdemeanor = dr("Misdemeanor")
        If Not (dr("MisdemeanorDesc") Is System.DBNull.Value) Then _MisdemeanorDesc = dr("MisdemeanorDesc")
        If Not (dr("BackGroundAuthorize") Is System.DBNull.Value) Then _BackGroundAuthorize = dr("BackGroundAuthorize")
        If Not (dr("PreviousName") Is System.DBNull.Value) Then _PreviousName = dr("PreviousName")
        If Not (dr("PreviousFirst") Is System.DBNull.Value) Then _PreviousFirst = dr("PreviousFirst")
        If Not (dr("PreviousLast") Is System.DBNull.Value) Then _PreviousLast = dr("PreviousLast")
        If Not (dr("PreviousMiddle") Is System.DBNull.Value) Then _PreviousMiddle = dr("PreviousMiddle")
        If Not (dr("PreviousAddress") Is System.DBNull.Value) Then _PreviousAddress = dr("PreviousAddress")
        If Not (dr("PreviousCity") Is System.DBNull.Value) Then _PreviousCity = dr("PreviousCity")
        If Not (dr("PreviousStateID") Is System.DBNull.Value) Then _PreviousStateID = dr("PreviousStateID")
        If Not (dr("PreviousPostalCode") Is System.DBNull.Value) Then _PreviousPostalCode = dr("PreviousPostalCode")
        If Not (dr("PreviousAddYears") Is System.DBNull.Value) Then _PreviousAddYears = dr("PreviousAddYears")
        If Not (dr("PreviousAddMonths") Is System.DBNull.Value) Then _PreviousAddMonths = dr("PreviousAddMonths")
        If Not (dr("PhysicalLimitations") Is System.DBNull.Value) Then _PhysicalLimitations = dr("PhysicalLimitations")
        If Not (dr("PhysicalLimitationsDesc") Is System.DBNull.Value) Then _PhysicalLimitationsDesc = dr("PhysicalLimitationsDesc")
        If Not (dr("BirthMonth") Is System.DBNull.Value) Then _BirthMonth = dr("BirthMonth")
        If Not (dr("BirthDay") Is System.DBNull.Value) Then _BirthDay = dr("BirthDay")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_JobPostingApplicant where ApplicantID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingApplicant")
            If ds.Tables("t_JobPostingApplicant").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingApplicant").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_JobPostingApplicantInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.varchar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.varchar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@MiddleName", SqlDbType.varchar, 0, "MiddleName")
                da.InsertCommand.Parameters.Add("@NickName", SqlDbType.VarChar, 0, "NickName")
                da.InsertCommand.Parameters.Add("@Suffix", SqlDbType.VarChar, 0, "Suffix")
                da.InsertCommand.Parameters.Add("@EmailAddress", SqlDbType.varchar, 0, "EmailAddress")
                da.InsertCommand.Parameters.Add("@Address", SqlDbType.varchar, 0, "Address")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.varchar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@PostalCode", SqlDbType.varchar, 0, "PostalCode")
                da.InsertCommand.Parameters.Add("@AddressYears", SqlDbType.int, 0, "AddressYears")
                da.InsertCommand.Parameters.Add("@AddressMonths", SqlDbType.int, 0, "AddressMonths")
                da.InsertCommand.Parameters.Add("@SSN", SqlDbType.int, 0, "SSN")
                da.InsertCommand.Parameters.Add("@HomePhone", SqlDbType.varchar, 0, "HomePhone")
                da.InsertCommand.Parameters.Add("@MobilePhone", SqlDbType.varchar, 0, "MobilePhone")
                da.InsertCommand.Parameters.Add("@DOB", SqlDbType.varchar, 0, "DOB")
                da.InsertCommand.Parameters.Add("@UserName", SqlDbType.varchar, 0, "UserName")
                da.InsertCommand.Parameters.Add("@Password", SqlDbType.VarChar, 0, "Password")
                da.InsertCommand.Parameters.Add("@ESig", SqlDbType.VarChar, 0, "Esig")
                da.InsertCommand.Parameters.Add("@SecQuestion1", SqlDbType.int, 0, "SecQuestion1")
                da.InsertCommand.Parameters.Add("@SecAnswer1", SqlDbType.varchar, 0, "SecAnswer1")
                da.InsertCommand.Parameters.Add("@SecQuestion2", SqlDbType.int, 0, "SecQuestion2")
                da.InsertCommand.Parameters.Add("@SecAnswer2", SqlDbType.varchar, 0, "SecAnswer2")
                da.InsertCommand.Parameters.Add("@SecQuestion3", SqlDbType.Int, 0, "SecQuestion3")
                da.InsertCommand.Parameters.Add("@SecAnswer3", SqlDbType.VarChar, 0, "SecAnswer3")
                da.InsertCommand.Parameters.Add("@Felony", SqlDbType.Bit, 0, "Felony")
                da.InsertCommand.Parameters.Add("@FelonyDesc", SqlDbType.varchar, 0, "FelonyDesc")
                da.InsertCommand.Parameters.Add("@Misdemeanor", SqlDbType.bit, 0, "Misdemeanor")
                da.InsertCommand.Parameters.Add("@MisdemeanorDesc", SqlDbType.varchar, 0, "MisdemeanorDesc")
                da.InsertCommand.Parameters.Add("@BackGroundAuthorize", SqlDbType.bit, 0, "BackGroundAuthorize")
                da.InsertCommand.Parameters.Add("@PreviousName", SqlDbType.bit, 0, "PreviousName")
                da.InsertCommand.Parameters.Add("@PreviousFirst", SqlDbType.varchar, 0, "PreviousFirst")
                da.InsertCommand.Parameters.Add("@PreviousLast", SqlDbType.varchar, 0, "PreviousLast")
                da.InsertCommand.Parameters.Add("@PreviousMiddle", SqlDbType.varchar, 0, "PreviousMiddle")
                da.InsertCommand.Parameters.Add("@PreviousAddress", SqlDbType.varchar, 0, "PreviousAddress")
                da.InsertCommand.Parameters.Add("@PreviousCity", SqlDbType.varchar, 0, "PreviousCity")
                da.InsertCommand.Parameters.Add("@PreviousStateID", SqlDbType.int, 0, "PreviousStateID")
                da.InsertCommand.Parameters.Add("@PreviousPostalCode", SqlDbType.varchar, 0, "PreviousPostalCode")
                da.InsertCommand.Parameters.Add("@PreviousAddYears", SqlDbType.int, 0, "PreviousAddYears")
                da.InsertCommand.Parameters.Add("@PreviousAddMonths", SqlDbType.int, 0, "PreviousAddMonths")
                da.InsertCommand.Parameters.Add("@PhysicalLimitations", SqlDbType.bit, 0, "PhysicalLimitations")
                da.InsertCommand.Parameters.Add("@PhysicalLimitationsDesc", SqlDbType.VarChar, 0, "PhysicalLimitationsDesc")
                da.InsertCommand.Parameters.Add("@BirthMonth", SqlDbType.Int, 0, "BirthMonth")
                da.InsertCommand.Parameters.Add("@BirthDay", SqlDbType.Int, 0, "BirthDay")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ApplicantID", SqlDbType.Int, 0, "ApplicantID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_JobPostingApplicant").NewRow
            End If
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("MiddleName", _MiddleName, dr)
            Update_Field("NickName", _NickName, dr)
            Update_Field("Suffix", _Suffix, dr)
            Update_Field("EmailAddress", _EmailAddress, dr)
            Update_Field("Address", _Address, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("AddressYears", _AddressYears, dr)
            Update_Field("AddressMonths", _AddressMonths, dr)
            Update_Field("SSN", _SSN, dr)
            Update_Field("HomePhone", _HomePhone, dr)
            Update_Field("MobilePhone", _MobilePhone, dr)
            Update_Field("DOB", _DOB, dr)
            Update_Field("UserName", _UserName, dr)
            Update_Field("Password", _Password, dr)
            Update_Field("ESig", _Esig, dr)
            Update_Field("SecQuestion1", _SecQuestion1, dr)
            Update_Field("SecAnswer1", _SecAnswer1, dr)
            Update_Field("SecQuestion2", _SecQuestion2, dr)
            Update_Field("SecAnswer2", _SecAnswer2, dr)
            Update_Field("SecQuestion3", _SecQuestion3, dr)
            Update_Field("SecAnswer3", _SecAnswer3, dr)
            Update_Field("Felony", _Felony, dr)
            Update_Field("FelonyDesc", _FelonyDesc, dr)
            Update_Field("Misdemeanor", _Misdemeanor, dr)
            Update_Field("MisdemeanorDesc", _MisdemeanorDesc, dr)
            Update_Field("BackGroundAuthorize", _BackGroundAuthorize, dr)
            Update_Field("PreviousName", _PreviousName, dr)
            Update_Field("PreviousFirst", _PreviousFirst, dr)
            Update_Field("PreviousLast", _PreviousLast, dr)
            Update_Field("PreviousMiddle", _PreviousMiddle, dr)
            Update_Field("PreviousAddress", _PreviousAddress, dr)
            Update_Field("PreviousCity", _PreviousCity, dr)
            Update_Field("PreviousStateID", _PreviousStateID, dr)
            Update_Field("PreviousPostalCode", _PreviousPostalCode, dr)
            Update_Field("PreviousAddYears", _PreviousAddYears, dr)
            Update_Field("PreviousAddMonths", _PreviousAddMonths, dr)
            Update_Field("PhysicalLimitations", _PhysicalLimitations, dr)
            Update_Field("PhysicalLimitationsDesc", _PhysicalLimitationsDesc, dr)
            Update_Field("BirthMonth", _BirthMonth, dr)
            Update_Field("BirthDay", _BirthDay, dr)
            If ds.Tables("t_JobPostingApplicant").Rows.Count < 1 Then ds.Tables("t_JobPostingApplicant").Rows.Add(dr)
            da.Update(ds, "t_JobPostingApplicant")
            _ID = ds.Tables("t_JobPostingApplicant").Rows(0).Item("ApplicantID")
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
            oEvents.KeyField = "ApplicantID"
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

    Public Function Check_UserName(ByVal uName As String) As Boolean

        Dim bFound As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Users from t_JobPostingAPplicant where username = '" & uName & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Users") > 0 Then
                bFound = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bFound
    End Function

    Public Function Validate_Esig(ByVal esig As String, ByVal userID As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Users from t_JobPostingApplicant where ESig = '" & esig & "' and ApplicantID = " & userID
            dread = cm.ExecuteReader
            dread.Read()
            If dread("users") > 0 Then
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

    Public Function Validate_Sec_Answer(ByVal userID As String, ByVal questionID As Integer, ByVal answer As String) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Answers from t_JobPostingApplicant where username = '" & userID & "' and ((SecQuestion1 = " & questionID & " and secAnswer1 = '" & answer & "') or (SecQuestion2 = " & questionID & " and secAnswer2 = '" & answer & "') or (SecQuestion3 = " & questionID & " and secAnswer3 = '" & answer & "'))"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Answers") > 0 Then
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

    Public Function Get_Applicant_ID(ByVal uName As String) As Integer
        Dim appID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ApplicantID from t_JobPostingApplicant where username = '" & uName & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                appID = dread("ApplicantID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return appID
    End Function

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
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

    Public Property MiddleName() As String
        Get
            Return _MiddleName
        End Get
        Set(ByVal value As String)
            _MiddleName = value
        End Set
    End Property

    Public Property NickName() As String
        Get
            Return _NickName
        End Get
        Set(ByVal value As String)
            _NickName = value
        End Set
    End Property

    Public Property Suffix() As String
        Get
            Return _Suffix
        End Get
        Set(ByVal value As String)
            _Suffix = value
        End Set
    End Property

    Public Property EmailAddress() As String
        Get
            Return _EmailAddress
        End Get
        Set(ByVal value As String)
            _EmailAddress = value
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

    Public Property StateID() As Integer
        Get
            Return _StateID
        End Get
        Set(ByVal value As Integer)
            _StateID = value
        End Set
    End Property

    Public Property PostalCode() As String
        Get
            Return _PostalCode
        End Get
        Set(ByVal value As String)
            _PostalCode = value
        End Set
    End Property

    Public Property AddressYears() As Integer
        Get
            Return _AddressYears
        End Get
        Set(ByVal value As Integer)
            _AddressYears = value
        End Set
    End Property

    Public Property AddressMonths() As Integer
        Get
            Return _AddressMonths
        End Get
        Set(ByVal value As Integer)
            _AddressMonths = value
        End Set
    End Property

    Public Property SSN() As Integer
        Get
            Return _SSN
        End Get
        Set(ByVal value As Integer)
            _SSN = value
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

    Public Property MobilePhone() As String
        Get
            Return _MobilePhone
        End Get
        Set(ByVal value As String)
            _MobilePhone = value
        End Set
    End Property

    Public Property DOB() As String
        Get
            Return _DOB
        End Get
        Set(ByVal value As String)
            _DOB = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
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

    Public Property ESig() As String
        Get
            Return _Esig
        End Get
        Set(ByVal value As String)
            _Esig = value
        End Set
    End Property

    Public Property SecQuestion1() As Integer
        Get
            Return _SecQuestion1
        End Get
        Set(ByVal value As Integer)
            _SecQuestion1 = value
        End Set
    End Property

    Public Property SecAnswer1() As String
        Get
            Return _SecAnswer1
        End Get
        Set(ByVal value As String)
            _SecAnswer1 = value
        End Set
    End Property

    Public Property SecQuestion2() As Integer
        Get
            Return _SecQuestion2
        End Get
        Set(ByVal value As Integer)
            _SecQuestion2 = value
        End Set
    End Property

    Public Property SecAnswer2() As String
        Get
            Return _SecAnswer2
        End Get
        Set(ByVal value As String)
            _SecAnswer2 = value
        End Set
    End Property

    Public Property SecQuestion3() As Integer
        Get
            Return _SecQuestion3
        End Get
        Set(ByVal value As Integer)
            _SecQuestion3 = value
        End Set
    End Property

    Public Property SecAnswer3() As String
        Get
            Return _SecAnswer3
        End Get
        Set(ByVal value As String)
            _SecAnswer3 = value
        End Set
    End Property

    Public Property Felony() As Boolean
        Get
            Return _Felony
        End Get
        Set(ByVal value As Boolean)
            _Felony = value
        End Set
    End Property

    Public Property FelonyDesc() As String
        Get
            Return _FelonyDesc
        End Get
        Set(ByVal value As String)
            _FelonyDesc = value
        End Set
    End Property

    Public Property Misdemeanor() As Boolean
        Get
            Return _Misdemeanor
        End Get
        Set(ByVal value As Boolean)
            _Misdemeanor = value
        End Set
    End Property

    Public Property MisdemeanorDesc() As String
        Get
            Return _MisdemeanorDesc
        End Get
        Set(ByVal value As String)
            _MisdemeanorDesc = value
        End Set
    End Property

    Public Property BackGroundAuthorize() As Boolean
        Get
            Return _BackGroundAuthorize
        End Get
        Set(ByVal value As Boolean)
            _BackGroundAuthorize = value
        End Set
    End Property

    Public Property PreviousName() As Boolean
        Get
            Return _PreviousName
        End Get
        Set(ByVal value As Boolean)
            _PreviousName = value
        End Set
    End Property

    Public Property PreviousFirst() As String
        Get
            Return _PreviousFirst
        End Get
        Set(ByVal value As String)
            _PreviousFirst = value
        End Set
    End Property

    Public Property PreviousLast() As String
        Get
            Return _PreviousLast
        End Get
        Set(ByVal value As String)
            _PreviousLast = value
        End Set
    End Property

    Public Property PreviousMiddle() As String
        Get
            Return _PreviousMiddle
        End Get
        Set(ByVal value As String)
            _PreviousMiddle = value
        End Set
    End Property

    Public Property PreviousAddress() As String
        Get
            Return _PreviousAddress
        End Get
        Set(ByVal value As String)
            _PreviousAddress = value
        End Set
    End Property

    Public Property PreviousCity() As String
        Get
            Return _PreviousCity
        End Get
        Set(ByVal value As String)
            _PreviousCity = value
        End Set
    End Property

    Public Property PreviousStateID() As Integer
        Get
            Return _PreviousStateID
        End Get
        Set(ByVal value As Integer)
            _PreviousStateID = value
        End Set
    End Property

    Public Property PreviousPostalCode() As String
        Get
            Return _PreviousPostalCode
        End Get
        Set(ByVal value As String)
            _PreviousPostalCode = value
        End Set
    End Property

    Public Property PreviousAddYears() As Integer
        Get
            Return _PreviousAddYears
        End Get
        Set(ByVal value As Integer)
            _PreviousAddYears = value
        End Set
    End Property

    Public Property PreviousAddMonths() As Integer
        Get
            Return _PreviousAddMonths
        End Get
        Set(ByVal value As Integer)
            _PreviousAddMonths = value
        End Set
    End Property

    Public Property PhysicalLimitations() As Boolean
        Get
            Return _PhysicalLimitations
        End Get
        Set(ByVal value As Boolean)
            _PhysicalLimitations = value
        End Set
    End Property

    Public Property PhysicalLimitationsDesc() As String
        Get
            Return _PhysicalLimitationsDesc
        End Get
        Set(ByVal value As String)
            _PhysicalLimitationsDesc = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

    Public Property BirthMonth() As Integer
        Get
            Return _BirthMonth
        End Get
        Set(ByVal value As Integer)
            _BirthMonth = value
        End Set
    End Property

    Public Property BirthDay() As Integer
        Get
            Return _BirthDay
        End Get
        Set(ByVal value As Integer)
            _BirthDay = value
        End Set
    End Property

    Public Property ApplicantID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
