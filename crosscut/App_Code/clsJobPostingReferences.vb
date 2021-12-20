Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsJobPostingReferences
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ApplicantID As Integer = 0
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _PhoneNumber As String = ""
    Dim _YearsKnown As Integer = 0
    Dim _Active As Boolean = False
    Dim _Occupation As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_JobPostingReferences where ReferenceID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_JobPostingReferences where ReferenceID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingReferences")
            If ds.Tables("t_JobPostingReferences").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingReferences").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ApplicantID") Is System.DBNull.Value) Then _ApplicantID = dr("ApplicantID")
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("PhoneNumber") Is System.DBNull.Value) Then _PhoneNumber = dr("PhoneNumber")
        If Not (dr("YearsKnown") Is System.DBNull.Value) Then _YearsKnown = dr("YearsKnown")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("Occupation") Is System.DBNull.Value) Then _Occupation = dr("Occupation")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_JobPostingReferences where ReferenceID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_JobPostingReferences")
            If ds.Tables("t_JobPostingReferences").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPostingReferences").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_JobPostingReferencesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ApplicantID", SqlDbType.int, 0, "ApplicantID")
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.varchar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.varchar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.varchar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.varchar, 0, "PhoneNumber")
                da.InsertCommand.Parameters.Add("@YearsKnown", SqlDbType.int, 0, "YearsKnown")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@Occupation", SqlDbType.VarChar, 0, "Occupation")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReferenceID", SqlDbType.Int, 0, "ReferenceID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_JobPostingReferences").NewRow
            End If
            Update_Field("ApplicantID", _ApplicantID, dr)
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("PhoneNumber", _PhoneNumber, dr)
            Update_Field("YearsKnown", _YearsKnown, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("Occupation", _Occupation, dr)
            If ds.Tables("t_JobPostingReferences").Rows.Count < 1 Then ds.Tables("t_JobPostingReferences").Rows.Add(dr)
            da.Update(ds, "t_JobPostingReferences")
            _ID = ds.Tables("t_JobPostingReferences").Rows(0).Item("ReferenceID")
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
            oEvents.KeyField = "ReferenceID"
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

    Public Property PhoneNumber() As String
        Get
            Return _PhoneNumber
        End Get
        Set(ByVal value As String)
            _PhoneNumber = value
        End Set
    End Property

    Public Property YearsKnown() As Integer
        Get
            Return _YearsKnown
        End Get
        Set(ByVal value As Integer)
            _YearsKnown = value
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

    Public Property Occupation() As String
        Get
            Return _Occupation
        End Get
        Set(value As String)
            _Occupation = value
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

    Public Property ReferenceID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
