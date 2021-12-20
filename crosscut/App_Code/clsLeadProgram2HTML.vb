Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadProgram2HTML
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LeadProgramID As Integer = 0
    Dim _FilePath As String = ""
    Dim _Active As Boolean = False
    Dim _isHTML As Boolean = False
    Dim _sideBar As Boolean = False
    Dim _terms As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadProgram2HTML where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadProgram2HTML where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram2HTML")
            If ds.Tables("t_LeadProgram2HTML").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram2HTML").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LeadProgramID") Is System.DBNull.Value) Then _LeadProgramID = dr("LeadProgramID")
        If Not (dr("FilePath") Is System.DBNull.Value) Then _FilePath = dr("FilePath")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("isHTML") Is System.DBNull.Value) Then _isHTML = dr("isHTML")
        If Not (dr("sidebar") Is System.DBNull.Value) Then _sideBar = dr("sidebar")
        If Not (dr("terms") Is System.DBNull.Value) Then _terms = dr("terms")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadProgram2HTML where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram2HTML")
            If ds.Tables("t_LeadProgram2HTML").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram2HTML").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadProgram2HTMLInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LeadProgramID", SqlDbType.int, 0, "LeadProgramID")
                da.InsertCommand.Parameters.Add("@FilePath", SqlDbType.varchar, 0, "FilePath")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@isHTML", SqlDbType.Bit, 0, "isHTML")
                da.InsertCommand.Parameters.Add("@Sidebar", SqlDbType.Bit, 0, "Sidebar")
                da.InsertCommand.Parameters.Add("@Terms", SqlDbType.Bit, 0, "terms")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadProgram2HTML").NewRow
            End If
            Update_Field("LeadProgramID", _LeadProgramID, dr)
            Update_Field("FilePath", _FilePath, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("isHTML", _isHTML, dr)
            Update_Field("sidebar", _sideBar, dr)
            Update_Field("terms", _terms, dr)
            If ds.Tables("t_LeadProgram2HTML").Rows.Count < 1 Then ds.Tables("t_LeadProgram2HTML").Rows.Add(dr)
            da.Update(ds, "t_LeadProgram2HTML")
            _ID = ds.Tables("t_LeadProgram2HTML").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
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

    Public Function get_Path(ByVal progID As Integer, ByVal isHTML As Integer, ByVal isSideBar As Integer, ByVal isTerms As Integer) As String
        Dim path As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select FilePath from t_LeadProgram2HTML where Active = 1 and LeadProgramID = " & progID & " and isHTML = " & isHTML & " and sidebar = " & isSideBar & " and terms = " & isTerms
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                path = dread("FilePath")
            End If
            dread.Close()
        Catch ex As Exception
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return path
    End Function

    Public Function List_URLS(ByVal LPID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ID, Case when SideBar = 1 then 'SideBar' when Terms = 1 then 'Terms' when isHTML = 1 then 'HTML' else 'JavaScript' end as Type, FilePath, Active from t_LeadProgram2HTML where LeadProgramID = " & LPID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property LeadProgramID() As Integer
        Get
            Return _LeadProgramID
        End Get
        Set(ByVal value As Integer)
            _LeadProgramID = value
        End Set
    End Property

    Public Property FilePath() As String
        Get
            Return _FilePath
        End Get
        Set(ByVal value As String)
            _FilePath = value
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

    Public Property isHTML() As Boolean
        Get
            Return _isHTML
        End Get
        Set(ByVal value As Boolean)
            _isHTML = value
        End Set
    End Property

    Public Property sidebar() As Boolean
        Get
            Return _sideBar
        End Get
        Set(ByVal value As Boolean)
            _sideBar = value
        End Set
    End Property

    Public Property terms() As Boolean
        Get
            Return _terms
        End Get
        Set(ByVal value As Boolean)
            _terms = value
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

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
