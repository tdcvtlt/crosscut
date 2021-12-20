Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsJobPostingExperience
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ApplicantID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _Company As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _Phone As String = ""
    Dim _Title As String = ""
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _Duties As String = ""
    Dim _StartSalary As String = ""
    Dim _EndSalary As String = ""
    Dim _Supervisor As String = ""
    Dim _AllowContact As Boolean = False
    Dim _LeavingReason As String = ""
    Dim _BranchID As Integer = 0
    Dim _MilRank As String = ""
    Dim _ContactPerson As String = ""
    Dim _ServiceStartDate As String = ""
    Dim _ServiceEndDate As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_JobPostingExperience where ExperienceID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_JobPostingExperience where ExperienceID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingExperience")
            If ds.Tables("t_JobPostingExperience").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingExperience").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ApplicantID") Is System.DBNull.Value) Then _ApplicantID = dr("ApplicantID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("Company") Is System.DBNull.Value) Then _Company = dr("Company")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("Phone") Is System.DBNull.Value) Then _Phone = dr("Phone")
        If Not (dr("Title") Is System.DBNull.Value) Then _Title = dr("Title")
        _StartDate = dr("StartDate") & ""
        'If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        _EndDate = dr("EndDate") & ""
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("Duties") Is System.DBNull.Value) Then _Duties = dr("Duties")
        If Not (dr("StartSalary") Is System.DBNull.Value) Then _StartSalary = dr("StartSalary")
        If Not (dr("EndSalary") Is System.DBNull.Value) Then _EndSalary = dr("EndSalary")
        If Not (dr("Supervisor") Is System.DBNull.Value) Then _Supervisor = dr("Supervisor")
        If Not (dr("AllowContact") Is System.DBNull.Value) Then _AllowContact = dr("AllowContact")
        If Not (dr("LeavingReason") Is System.DBNull.Value) Then _LeavingReason = dr("LeavingReason")
        If Not (dr("BranchID") Is System.DBNull.Value) Then _BranchID = dr("BranchID")
        If Not (dr("MilRank") Is System.DBNull.Value) Then _MilRank = dr("MilRank")
        If Not (dr("ContactPerson") Is System.DBNull.Value) Then _ContactPerson = dr("ContactPerson")
        _ServiceStartDate = dr("ServiceStartDate") & ""
        'If Not (dr("ServiceStartDate") Is System.DBNull.Value) Then _ServiceStartDate = dr("ServiceStartDate")
        _ServiceEndDate = dr("ServiceEndDate") & ""
        'If Not (dr("ServiceEndDate") Is System.DBNull.Value) Then _ServiceEndDate = dr("ServiceEndDate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_JobPostingExperience where ExperienceID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingExperience")
            If ds.Tables("t_JobPostingExperience").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingExperience").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_JobPostingExperienceInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ApplicantID", SqlDbType.int, 0, "ApplicantID")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@Company", SqlDbType.varchar, 0, "Company")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.varchar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@Phone", SqlDbType.varchar, 0, "Phone")
                da.InsertCommand.Parameters.Add("@Title", SqlDbType.varchar, 0, "Title")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.date, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.date, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@Duties", SqlDbType.varchar, 0, "Duties")
                da.InsertCommand.Parameters.Add("@StartSalary", SqlDbType.varchar, 0, "StartSalary")
                da.InsertCommand.Parameters.Add("@EndSalary", SqlDbType.varchar, 0, "EndSalary")
                da.InsertCommand.Parameters.Add("@Supervisor", SqlDbType.varchar, 0, "Supervisor")
                da.InsertCommand.Parameters.Add("@AllowContact", SqlDbType.bit, 0, "AllowContact")
                da.InsertCommand.Parameters.Add("@LeavingReason", SqlDbType.varchar, 0, "LeavingReason")
                da.InsertCommand.Parameters.Add("@BranchID", SqlDbType.int, 0, "BranchID")
                da.InsertCommand.Parameters.Add("@MilRank", SqlDbType.VarChar, 0, "MilRank")
                da.InsertCommand.Parameters.Add("@ContactPerson", SqlDbType.varchar, 0, "ContactPerson")
                da.InsertCommand.Parameters.Add("@ServiceStartDate", SqlDbType.date, 0, "ServiceStartDate")
                da.InsertCommand.Parameters.Add("@ServiceEndDate", SqlDbType.date, 0, "ServiceEndDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ExperienceID", SqlDbType.Int, 0, "ExperienceID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_JobPostingExperience").NewRow
            End If
            Update_Field("ApplicantID", _ApplicantID, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("Company", _Company, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("Phone", _Phone, dr)
            Update_Field("Title", _Title, dr)
            If _StartDate = "" Then _StartDate = System.DBNull.Value.ToString
            Update_Field("StartDate", _StartDate, dr)
            If _EndDate = "" Then _EndDate = System.DBNull.Value.ToString
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("Duties", _Duties, dr)
            Update_Field("StartSalary", _StartSalary, dr)
            Update_Field("EndSalary", _EndSalary, dr)
            Update_Field("Supervisor", _Supervisor, dr)
            Update_Field("AllowContact", _AllowContact, dr)
            Update_Field("LeavingReason", _LeavingReason, dr)
            Update_Field("BranchID", _BranchID, dr)
            Update_Field("MilRank", _MilRank, dr)
            Update_Field("ContactPerson", _ContactPerson, dr)
            If _ServiceStartDate = "" Then _ServiceStartDate = System.DBNull.Value.ToString
            Update_Field("ServiceStartDate", _ServiceStartDate, dr)
            If _ServiceEndDate = "" Then _ServiceEndDate = System.DBNull.Value.ToString
            Update_Field("ServiceEndDate", _ServiceEndDate, dr)
            If ds.Tables("t_JobPostingExperience").Rows.Count < 1 Then ds.Tables("t_JobPostingExperience").Rows.Add(dr)
            da.Update(ds, "t_JobPostingExperience")
            _ID = ds.Tables("t_JobPostingExperience").Rows(0).Item("ExperienceID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField).ToString, "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ExperienceID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            If Right(Trim(sField), 4) = "Date" And sValue = "" Then
                drow(sField) = DBNull.Value
            Else
                drow(sField) = sValue
            End If
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property ApplicantID() As Integer
        Get
            Return _ApplicantID
        End Get
        Set(ByVal value As Integer)
            _ApplicantID = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property Company() As String
        Get
            Return _Company
        End Get
        Set(ByVal value As String)
            _Company = value
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

    Public Property Phone() As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
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

    Public Property Duties() As String
        Get
            Return _Duties
        End Get
        Set(ByVal value As String)
            _Duties = value
        End Set
    End Property

    Public Property StartSalary() As String
        Get
            Return _StartSalary
        End Get
        Set(ByVal value As String)
            _StartSalary = value
        End Set
    End Property

    Public Property EndSalary() As String
        Get
            Return _EndSalary
        End Get
        Set(ByVal value As String)
            _EndSalary = value
        End Set
    End Property

    Public Property Supervisor() As String
        Get
            Return _Supervisor
        End Get
        Set(ByVal value As String)
            _Supervisor = value
        End Set
    End Property

    Public Property AllowContact() As Boolean
        Get
            Return _AllowContact
        End Get
        Set(ByVal value As Boolean)
            _AllowContact = value
        End Set
    End Property

    Public Property LeavingReason() As String
        Get
            Return _LeavingReason
        End Get
        Set(ByVal value As String)
            _LeavingReason = value
        End Set
    End Property

    Public Property BranchID() As Integer
        Get
            Return _BranchID
        End Get
        Set(ByVal value As Integer)
            _BranchID = value
        End Set
    End Property

    Public Property MilRank() As String
        Get
            Return _MilRank
        End Get
        Set(ByVal value As String)
            _MilRank = value
        End Set
    End Property

    Public Property ContactPerson() As String
        Get
            Return _ContactPerson
        End Get
        Set(ByVal value As String)
            _ContactPerson = value
        End Set
    End Property

    Public Property ServiceStartDate() As String
        Get
            Return _ServiceStartDate
        End Get
        Set(ByVal value As String)
            _ServiceStartDate = value
        End Set
    End Property

    Public Property ServiceEndDate() As String
        Get
            Return _ServiceEndDate
        End Get
        Set(ByVal value As String)
            _ServiceEndDate = value
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

    Public Property ExperienceID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
