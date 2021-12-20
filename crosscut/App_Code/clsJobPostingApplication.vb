Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsJobPostingApplication
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ApplicantID As Integer = 0
    Dim _JobID As Integer = 0
    Dim _ApplicationStep As Integer = 0
    Dim _Completed As Integer = 0
    Dim _ReferralSourceID As Integer = 0
    Dim _PostingSourceID As Integer = 0
    Dim _ReferrerID As Integer = 0
    Dim _Referrer As String = ""
    Dim _ReferrerPrevPosition As String = ""
    Dim _ReferrerPrevStartDate As String = ""
    Dim _ReferrerPrevEndDate As String = ""
    Dim _ReferralSourceOther As String = ""
    Dim _StartDate As String = ""
    Dim _HoursPerWeek As Integer = 0
    Dim _WorkWeekends As Boolean = False
    Dim _WorkHolidays As Boolean = False
    Dim _WorkOvertime As Boolean = False
    Dim _DesiredPayTypeID As Integer = 0
    Dim _DesiredPay As Decimal = 0
    Dim _PositionInterest As String = ""
    Dim _BestCandidate As String = ""
    Dim _ChangeJobReason As String = ""
    Dim _TimeShareFamiliar As Boolean = False
    Dim _ThreeWordDesc As String = ""
    Dim _CoWorkerDescription As String = ""
    Dim _DeptSuccess As String = ""
    Dim _OvercomeFailureExample As String = ""
    Dim _FriendsWork As Boolean = False
    Dim _FriendsDesc As String = ""
    Dim _DateStarted As String = ""
    Dim _DateCompleted As String = ""
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
        cm = New SqlCommand("Select * from t_JobPostingApplication where ApplicationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_JobPostingApplication where ApplicationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingApplication")
            If ds.Tables("t_JobPostingApplication").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingApplication").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ApplicantID") Is System.DBNull.Value) Then _ApplicantID = dr("ApplicantID")
        If Not (dr("JobID") Is System.DBNull.Value) Then _JobID = dr("JobID")
        If Not (dr("ApplicationStep") Is System.DBNull.Value) Then _ApplicationStep = dr("ApplicationStep")
        If Not (dr("Completed") Is System.DBNull.Value) Then _Completed = dr("Completed")
        If Not (dr("ReferralSourceID") Is System.DBNull.Value) Then _ReferralSourceID = dr("ReferralSourceID")
        If Not (dr("PostingSourceID") Is System.DBNull.Value) Then _PostingSourceID = dr("PostingSourceID")
        If Not (dr("ReferrerID") Is System.DBNull.Value) Then _ReferrerID = dr("ReferrerID")
        If Not (dr("Referrer") Is System.DBNull.Value) Then _Referrer = dr("Referrer")
        If Not (dr("ReferrerPrevPosition") Is System.DBNull.Value) Then _ReferrerPrevPosition = dr("ReferrerPrevPosition")
        _ReferrerPrevStartDate = dr("ReferrerPrevStartDate") & ""
        'If Not (dr("ReferrerPrevStartDate") Is System.DBNull.Value) Then _ReferrerPrevStartDate = dr("ReferrerPrevStartDate")
        _ReferrerPrevEndDate = dr("ReferrerPrevEndDate") & ""
        'If Not (dr("ReferrerPrevEndDate") Is System.DBNull.Value) Then _ReferrerPrevEndDate = dr("ReferrerPrevEndDate")
        If Not (dr("ReferralSourceOther") Is System.DBNull.Value) Then _ReferralSourceOther = dr("ReferralSourceOther")
        _StartDate = dr("StartDate") & ""
        'If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("HoursPerWeek") Is System.DBNull.Value) Then _HoursPerWeek = dr("HoursPerWeek")
        If Not (dr("WorkWeekends") Is System.DBNull.Value) Then _WorkWeekends = dr("WorkWeekends")
        If Not (dr("WorkHolidays") Is System.DBNull.Value) Then _WorkHolidays = dr("WorkHolidays")
        If Not (dr("WorkOvertime") Is System.DBNull.Value) Then _WorkOvertime = dr("WorkOvertime")
        If Not (dr("DesiredPayTypeID") Is System.DBNull.Value) Then _DesiredPayTypeID = dr("DesiredPayTypeID")
        If Not (dr("DesiredPay") Is System.DBNull.Value) Then _DesiredPay = dr("DesiredPay")
        If Not (dr("PositionInterest") Is System.DBNull.Value) Then _PositionInterest = dr("PositionInterest")
        If Not (dr("BestCandidate") Is System.DBNull.Value) Then _BestCandidate = dr("BestCandidate")
        If Not (dr("ChangeJobReason") Is System.DBNull.Value) Then _ChangeJobReason = dr("ChangeJobReason")
        If Not (dr("TimeShareFamiliar") Is System.DBNull.Value) Then _TimeShareFamiliar = dr("TimeShareFamiliar")
        If Not (dr("ThreeWordDesc") Is System.DBNull.Value) Then _ThreeWordDesc = dr("ThreeWordDesc")
        If Not (dr("CoWorkerDescription") Is System.DBNull.Value) Then _CoWorkerDescription = dr("CoWorkerDescription")
        If Not (dr("DeptSuccess") Is System.DBNull.Value) Then _DeptSuccess = dr("DeptSuccess")
        If Not (dr("OvercomeFailureExample") Is System.DBNull.Value) Then _OvercomeFailureExample = dr("OvercomeFailureExample")
        If Not (dr("FriendsWork") Is System.DBNull.Value) Then _FriendsWork = dr("FriendsWork")
        If Not (dr("FriendsDesc") Is System.DBNull.Value) Then _FriendsDesc = dr("FriendsDesc")
        If Not (dr("DateCompleted") Is System.DBNull.Value) Then _DateCompleted = dr("DateCompleted")
        If Not (dr("DateStarted") Is System.DBNull.Value) Then _DateStarted = dr("DateStarted")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_JobPostingApplication where ApplicationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingApplication")
            If ds.Tables("t_JobPostingApplication").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingApplication").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_JobPostingApplicationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ApplicantID", SqlDbType.int, 0, "ApplicantID")
                da.InsertCommand.Parameters.Add("@JobID", SqlDbType.int, 0, "JobID")
                da.InsertCommand.Parameters.Add("@ApplicationStep", SqlDbType.Int, 0, "ApplicationStep")
                da.InsertCommand.Parameters.Add("@Completed", SqlDbType.Int, 0, "Completed")
                da.InsertCommand.Parameters.Add("@ReferralSourceID", SqlDbType.int, 0, "ReferralSourceID")
                da.InsertCommand.Parameters.Add("@PostingSourceID", SqlDbType.int, 0, "PostingSourceID")
                da.InsertCommand.Parameters.Add("@ReferrerID", SqlDbType.int, 0, "ReferrerID")
                da.InsertCommand.Parameters.Add("@Referrer", SqlDbType.varchar, 0, "Referrer")
                da.InsertCommand.Parameters.Add("@ReferrerPrevPosition", SqlDbType.varchar, 0, "ReferrerPrevPosition")
                da.InsertCommand.Parameters.Add("@ReferrerPrevStartDate", SqlDbType.Date, 0, "ReferrerPrevStartDate")
                da.InsertCommand.Parameters.Add("@ReferrerPrevEndDate", SqlDbType.Date, 0, "ReferrerPrevEndDate")
                da.InsertCommand.Parameters.Add("@ReferralSourceOther", SqlDbType.varchar, 0, "ReferralSourceOther")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.Date, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@HoursPerWeek", SqlDbType.int, 0, "HoursPerWeek")
                da.InsertCommand.Parameters.Add("@WorkWeekends", SqlDbType.bit, 0, "WorkWeekends")
                da.InsertCommand.Parameters.Add("@WorkHolidays", SqlDbType.bit, 0, "WorkHolidays")
                da.InsertCommand.Parameters.Add("@WorkOvertime", SqlDbType.bit, 0, "WorkOvertime")
                da.InsertCommand.Parameters.Add("@DesiredPayTypeID", SqlDbType.int, 0, "DesiredPayTypeID")
                da.InsertCommand.Parameters.Add("@DesiredPay", SqlDbType.money, 0, "DesiredPay")
                da.InsertCommand.Parameters.Add("@PositionInterest", SqlDbType.varchar, 0, "PositionInterest")
                da.InsertCommand.Parameters.Add("@BestCandidate", SqlDbType.varchar, 0, "BestCandidate")
                da.InsertCommand.Parameters.Add("@ChangeJobReason", SqlDbType.varchar, 0, "ChangeJobReason")
                da.InsertCommand.Parameters.Add("@TimeShareFamiliar", SqlDbType.bit, 0, "TimeShareFamiliar")
                da.InsertCommand.Parameters.Add("@ThreeWordDesc", SqlDbType.varchar, 0, "ThreeWordDesc")
                da.InsertCommand.Parameters.Add("@CoWorkerDescription", SqlDbType.varchar, 0, "CoWorkerDescription")
                da.InsertCommand.Parameters.Add("@DeptSuccess", SqlDbType.varchar, 0, "DeptSuccess")
                da.InsertCommand.Parameters.Add("@OvercomeFailureExample", SqlDbType.varchar, 0, "OvercomeFailureExample")
                da.InsertCommand.Parameters.Add("@FriendsWork", SqlDbType.Bit, 0, "FriendsWork")
                da.InsertCommand.Parameters.Add("@FriendsDesc", SqlDbType.VarChar, 0, "FriendsDesc")
                da.InsertCommand.Parameters.Add("@DateCompleted", SqlDbType.DateTime, 0, "DateCompleted")
                da.InsertCommand.Parameters.Add("@DateStarted", SqlDbType.DateTime, 0, "DateStarted")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ApplicationID", SqlDbType.Int, 0, "ApplicationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_JobPostingApplication").NewRow
            End If
            Update_Field("ApplicantID", _ApplicantID, dr)
            Update_Field("JobID", _JobID, dr)
            Update_Field("ApplicationStep", _ApplicationStep, dr)
            Update_Field("Completed", _Completed, dr)
            Update_Field("ReferralSourceID", _ReferralSourceID, dr)
            Update_Field("PostingSourceID", _PostingSourceID, dr)
            Update_Field("ReferrerID", _ReferrerID, dr)
            Update_Field("Referrer", _Referrer, dr)
            Update_Field("ReferrerPrevPosition", _ReferrerPrevPosition, dr)
            If _ReferrerPrevStartDate = "" Then _ReferrerPrevStartDate = System.DBNull.Value.ToString
            Update_Field("ReferrerPrevStartDate", _ReferrerPrevStartDate, dr)
            If _ReferrerPrevEndDate = "" Then _ReferrerPrevEndDate = System.DBNull.Value.ToString
            Update_Field("ReferrerPrevEndDate", _ReferrerPrevEndDate, dr)
            Update_Field("ReferralSourceOther", _ReferralSourceOther, dr)
            If _StartDate = "" Then _StartDate = System.DBNull.Value.ToString
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("HoursPerWeek", _HoursPerWeek, dr)
            Update_Field("WorkWeekends", _WorkWeekends, dr)
            Update_Field("WorkHolidays", _WorkHolidays, dr)
            Update_Field("WorkOvertime", _WorkOvertime, dr)
            Update_Field("DesiredPayTypeID", _DesiredPayTypeID, dr)
            Update_Field("DesiredPay", _DesiredPay, dr)
            Update_Field("PositionInterest", _PositionInterest, dr)
            Update_Field("BestCandidate", _BestCandidate, dr)
            Update_Field("ChangeJobReason", _ChangeJobReason, dr)
            Update_Field("TimeShareFamiliar", _TimeShareFamiliar, dr)
            Update_Field("ThreeWordDesc", _ThreeWordDesc, dr)
            Update_Field("CoWorkerDescription", _CoWorkerDescription, dr)
            Update_Field("DeptSuccess", _DeptSuccess, dr)
            Update_Field("OvercomeFailureExample", _OvercomeFailureExample, dr)
            Update_Field("FriendsWork", _FriendsWork, dr)
            Update_Field("FriendsDesc", _FriendsDesc, dr)
            Update_Field("DateCompleted", _DateCompleted, dr)
            Update_Field("DateStarted", _DateStarted, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            If ds.Tables("t_JobPostingApplication").Rows.Count < 1 Then ds.Tables("t_JobPostingApplication").Rows.Add(dr)
            da.Update(ds, "t_JobPostingApplication")
            _ID = ds.Tables("t_JobPostingApplication").Rows(0).Item("ApplicationID")
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
            oEvents.KeyField = "ApplicationID"
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

    Public Function List_Apps_By_Job(ByVal jobID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select a.ApplicationiD, u.FirstName + ' ' + u.Lastname as Applicant, s.ComboItem as Status, a.StatusDate from t_JobPostingApplication a inner join t_JobPostingApplicant u on a.ApplicantID = u.ApplicantID left outer join t_ComboItems s on a.StatusID = s.ComboItemID where a.JobID = " & jobID & " and a.Completed = 1 order by a.StatusDate desc"
        Return ds
    End Function

    Public Function List_Apps_By_User(ByVal ID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select a.ApplicationID as ID, c.Title, b.ComboItem as Status from t_JobPostingApplication a left outer join t_COmboItems b on a.StatusID = b.ComboItemID inner join t_JobPosting c on a.JobID = c.JobID where a.ApplicantID = " & ID & " and a.Completed = 1"
        Return ds
    End Function

    Public Property ApplicantID() As Integer
        Get
            Return _ApplicantID
        End Get
        Set(ByVal value As Integer)
            _ApplicantID = value
        End Set
    End Property

    Public Property JobID() As Integer
        Get
            Return _JobID
        End Get
        Set(ByVal value As Integer)
            _JobID = value
        End Set
    End Property

    Public Property AppicationStep() As Integer
        Get
            Return _ApplicationStep
        End Get
        Set(ByVal value As Integer)
            _ApplicationStep = value
        End Set
    End Property

    Public Property Completed() As Integer
        Get
            Return _Completed
        End Get
        Set(ByVal value As Integer)
            _Completed = value
        End Set
    End Property

    Public Property ReferralSourceID() As Integer
        Get
            Return _ReferralSourceID
        End Get
        Set(ByVal value As Integer)
            _ReferralSourceID = value
        End Set
    End Property

    Public Property PostingSourceID() As Integer
        Get
            Return _PostingSourceID
        End Get
        Set(ByVal value As Integer)
            _PostingSourceID = value
        End Set
    End Property

    Public Property ReferrerID() As Integer
        Get
            Return _ReferrerID
        End Get
        Set(ByVal value As Integer)
            _ReferrerID = value
        End Set
    End Property

    Public Property Referrer() As String
        Get
            Return _Referrer
        End Get
        Set(ByVal value As String)
            _Referrer = value
        End Set
    End Property

    Public Property ReferrerPrevPosition() As String
        Get
            Return _ReferrerPrevPosition
        End Get
        Set(ByVal value As String)
            _ReferrerPrevPosition = value
        End Set
    End Property

    Public Property ReferrerPrevStartDate() As String
        Get
            Return _ReferrerPrevStartDate
        End Get
        Set(ByVal value As String)
            _ReferrerPrevStartDate = value
        End Set
    End Property

    Public Property ReferrerPrevEndDate() As String
        Get
            Return _ReferrerPrevEndDate
        End Get
        Set(ByVal value As String)
            _ReferrerPrevEndDate = value
        End Set
    End Property

    Public Property ReferralSourceOther() As String
        Get
            Return _ReferralSourceOther
        End Get
        Set(ByVal value As String)
            _ReferralSourceOther = value
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

    Public Property HoursPerWeek() As Integer
        Get
            Return _HoursPerWeek
        End Get
        Set(ByVal value As Integer)
            _HoursPerWeek = value
        End Set
    End Property

    Public Property WorkWeekends() As Boolean
        Get
            Return _WorkWeekends
        End Get
        Set(ByVal value As Boolean)
            _WorkWeekends = value
        End Set
    End Property

    Public Property WorkHolidays() As Boolean
        Get
            Return _WorkHolidays
        End Get
        Set(ByVal value As Boolean)
            _WorkHolidays = value
        End Set
    End Property

    Public Property WorkOvertime() As Boolean
        Get
            Return _WorkOvertime
        End Get
        Set(ByVal value As Boolean)
            _WorkOvertime = value
        End Set
    End Property

    Public Property DesiredPayTypeID() As Integer
        Get
            Return _DesiredPayTypeID
        End Get
        Set(ByVal value As Integer)
            _DesiredPayTypeID = value
        End Set
    End Property

    Public Property DesiredPay() As Decimal
        Get
            Return _DesiredPay
        End Get
        Set(ByVal value As Decimal)
            _DesiredPay = value
        End Set
    End Property

    Public Property PositionInterest() As String
        Get
            Return _PositionInterest
        End Get
        Set(ByVal value As String)
            _PositionInterest = value
        End Set
    End Property

    Public Property BestCandidate() As String
        Get
            Return _BestCandidate
        End Get
        Set(ByVal value As String)
            _BestCandidate = value
        End Set
    End Property

    Public Property ChangeJobReason() As String
        Get
            Return _ChangeJobReason
        End Get
        Set(ByVal value As String)
            _ChangeJobReason = value
        End Set
    End Property

    Public Property TimeShareFamiliar() As Boolean
        Get
            Return _TimeShareFamiliar
        End Get
        Set(ByVal value As Boolean)
            _TimeShareFamiliar = value
        End Set
    End Property

    Public Property ThreeWordDesc() As String
        Get
            Return _ThreeWordDesc
        End Get
        Set(ByVal value As String)
            _ThreeWordDesc = value
        End Set
    End Property

    Public Property CoWorkerDescription() As String
        Get
            Return _CoWorkerDescription
        End Get
        Set(ByVal value As String)
            _CoWorkerDescription = value
        End Set
    End Property

    Public Property DeptSuccess() As String
        Get
            Return _DeptSuccess
        End Get
        Set(ByVal value As String)
            _DeptSuccess = value
        End Set
    End Property

    Public Property OvercomeFailureExample() As String
        Get
            Return _OvercomeFailureExample
        End Get
        Set(ByVal value As String)
            _OvercomeFailureExample = value
        End Set
    End Property

    Public Property FriendsWork() As Boolean
        Get
            Return _FriendsWork
        End Get
        Set(ByVal value As Boolean)
            _FriendsWork = value
        End Set
    End Property
    Public Property FriendsDesc() As String
        Get
            Return _FriendsDesc
        End Get
        Set(ByVal value As String)
            _FriendsDesc = value
        End Set
    End Property
    Public Property DateCompleted() As String
        Get
            Return _DateCompleted
        End Get
        Set(ByVal value As String)
            _DateCompleted = value
        End Set
    End Property
    Public Property DateStarted() As String
        Get
            Return _DateStarted
        End Get
        Set(ByVal value As String)
            _DateStarted = value
        End Set
    End Property
    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            If value <> _StatusID Then
                _StatusDate = System.DateTime.Now.ToShortDateString
            End If
            _StatusID = value
        End Set
    End Property
    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
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
    Public Property ApplicationID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
