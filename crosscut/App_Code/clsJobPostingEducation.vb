Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsJobPostingEducation
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ApplicantID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _School As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _Certification As String = ""
    Dim _DateReceived As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_JobPostingEducation where EducationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_JobPostingEducation where EducationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingEducation")
            If ds.Tables("t_JobPostingEducation").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingEducation").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ApplicantID") Is System.DBNull.Value) Then _ApplicantID = dr("ApplicantID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("School") Is System.DBNull.Value) Then _School = dr("School")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("Certification") Is System.DBNull.Value) Then _Certification = dr("Certification")
        If Not (dr("DateReceived") Is System.DBNull.Value) Then _DateReceived = dr("DateReceived")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_JobPostingEducation where EducationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingEducation")
            If ds.Tables("t_JobPostingEducation").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingEducation").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_JobPostingEducationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ApplicantID", SqlDbType.int, 0, "ApplicantID")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@School", SqlDbType.varchar, 0, "School")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.varchar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@Certification", SqlDbType.varchar, 0, "Certification")
                da.InsertCommand.Parameters.Add("@DateReceived", SqlDbType.date, 0, "DateReceived")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@EducationID", SqlDbType.Int, 0, "EducationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_JobPostingEducation").NewRow
            End If
            Update_Field("ApplicantID", _ApplicantID, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("School", _School, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("Certification", _Certification, dr)
            Update_Field("DateReceived", _DateReceived, dr)
            If ds.Tables("t_JobPostingEducation").Rows.Count < 1 Then ds.Tables("t_JobPostingEducation").Rows.Add(dr)
            da.Update(ds, "t_JobPostingEducation")
            _ID = ds.Tables("t_JobPostingEducation").Rows(0).Item("EducationID")
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
            oEvents.KeyField = "EducationID"
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

    Public Property School() As String
        Get
            Return _School
        End Get
        Set(ByVal value As String)
            _School = value
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

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property Certification() As String
        Get
            Return _Certification
        End Get
        Set(ByVal value As String)
            _Certification = value
        End Set
    End Property

    Public Property DateReceived() As String
        Get
            Return _DateReceived
        End Get
        Set(ByVal value As String)
            _DateReceived = value
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

    Public Property EducationID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
