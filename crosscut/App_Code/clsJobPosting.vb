Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsJobPosting
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Title As String = ""
    Dim _Summary As String = ""
    Dim _Description As String = ""
    Dim _Active As Boolean = False
    Dim _CompanyID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _WebSiteID As Integer = 0
    Dim _Positions As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_JobPosting where JobID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_JobPosting where JobID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_JobPosting")
            If ds.Tables("t_JobPosting").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPosting").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "select j.jobID, j.Title, j.Summary, j.Description, j.DateCreated, j.CompanyID, j.DepartmentID, " & _
                                                "c.comboitem Company, d.comboitem Department " & _
                                                "from t_jobPosting j " & _
                                                "left join t_comboitems c on j.companyid = c.comboitemid " & _
                                                "left join t_comboitems d on j.departmentid = d.comboitemid"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("Title") Is System.DBNull.Value) Then _Title = dr("Title")
        If Not (dr("Summary") Is System.DBNull.Value) Then _Summary = dr("Summary")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("CompanyID") Is System.DBNull.Value) Then _CompanyID = dr("CompanyID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("WebSiteID") Is System.DBNull.Value) Then _WebSiteID = dr("WebSiteID")
        If Not (dr("Positions") Is System.DBNull.Value) Then _Positions = dr("Positions")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_JobPosting where JobID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_JobPosting")
            If ds.Tables("t_JobPosting").Rows.Count > 0 Then
                dr = ds.Tables("t_JobPosting").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_JobPostingInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Title", SqlDbType.varchar, 0, "Title")
                da.InsertCommand.Parameters.Add("@Summary", SqlDbType.VarChar, 1000, "Summary")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@CompanyID", SqlDbType.int, 0, "CompanyID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.Int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@WebSiteID", SqlDbType.Int, 0, "WebSiteID")
                da.InsertCommand.Parameters.Add("@Positions", SqlDbType.Int, 0, "Positions")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@JobID", SqlDbType.Int, 0, "JobID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_JobPosting").NewRow
            End If
            Update_Field("Title", _Title, dr)
            Update_Field("Summary", _Summary, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("CompanyID", _CompanyID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("WebSiteID", _WebSiteID, dr)
            Update_Field("Positions", _Positions, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("TypeID", _TypeID, dr)
            If _StatusDate <> "" Then
                If Not (dr("StatusDate") Is System.DBNull.Value) Then
                    If _StatusDate <> CDate(CStr(dr("StatusDate"))).ToShortDateString Then
                        Update_Field("StatusDate", _StatusDate, dr)
                    End If
                Else
                    Update_Field("StatusDate", _StatusDate, dr)
                End If
            Else
                Update_Field("StatusDate", _StatusDate, dr)
            End If
            If ds.Tables("t_JobPosting").Rows.Count < 1 Then ds.Tables("t_JobPosting").Rows.Add(dr)
            da.Update(ds, "t_JobPosting")
            _ID = ds.Tables("t_JobPosting").Rows(0).Item("JobID")
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
            oEvents.KeyField = "JobID"
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

    Public Function List_Job_Postings(ByVal active As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If active = -1 Then
            ds.SelectCommand = "Select a.JobID, a.Title, c.ComboItem as Status, a.StatusDate, a.Active from t_JobPosting a left outer join t_Comboitems c on a.StatusID = c.CombOitemID order by a.Title asc"
        Else
            ds.SelectCommand = "Select a.JobID, a.Title, c.Comboitem as Status, a.StatusDate, a.Active from t_JobPosting a left outer join t_ComboItems c on a.StatusID = c.ComboitemID where a.active = " & active & " order by a.Title asc"
        End If
        Return ds
    End Function

    Public Function List_Jobs(Optional ByVal jobID As Integer = 0) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If jobID <> 0 Then
            ds.SelectCommand = "Select JobID, Title from t_JobPosting where active = 1 or jobID = " & jobID & " order by Title asc"
        Else
            ds.SelectCommand = "Select JobID, Title from t_JobPosting where active = 1 order by Title asc"
        End If
        Return ds
    End Function
    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property

    Public Property Summary() As String
        Get
            Return _Summary
        End Get
        Set(ByVal value As String)
            _Summary = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public Property CompanyID() As Integer
        Get
            Return _CompanyID
        End Get
        Set(ByVal value As Integer)
            _CompanyID = value
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

    Public Property DepartmentID() As Integer
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Integer)
            _DepartmentID = value
        End Set
    End Property

    Public Property WebSiteID() As Integer
        Get
            Return _WebSiteID
        End Get
        Set(ByVal value As Integer)
            _WebSiteID = value
        End Set
    End Property

    Public Property Positions() As Integer
        Get
            Return _Positions
        End Get
        Set(ByVal value As Integer)
            _Positions = value
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

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property
    Public Property JobID() As Integer
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
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
