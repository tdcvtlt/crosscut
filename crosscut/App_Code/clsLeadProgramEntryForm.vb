Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadProgramEntryForm
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Description As String = ""
    Dim _DateCreated As String = ""
    Dim _HTMLPath As String = ""
    Dim _TermsPath As String = ""
    Dim _SideBarPath As String = ""
    Dim _JSPath As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadProgramEntryForm where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadProgramEntryForm where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgramEntryForm")
            If ds.Tables("t_LeadProgramEntryForm").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgramEntryForm").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("HTMLPath") Is System.DBNull.Value) Then _HTMLPath = dr("HTMLPath")
        If Not (dr("TermsPath") Is System.DBNull.Value) Then _TermsPath = dr("TermsPath")
        If Not (dr("SideBarPath") Is System.DBNull.Value) Then _SideBarPath = dr("SideBarPath")
        If Not (dr("JSPath") Is System.DBNull.Value) Then _JSPath = dr("JSPath")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadProgramEntryForm where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgramEntryForm")
            If ds.Tables("t_LeadProgramEntryForm").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgramEntryForm").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadProgramEntryFormInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@HTMLPath", SqlDbType.varchar, 0, "HTMLPath")
                da.InsertCommand.Parameters.Add("@TermsPath", SqlDbType.varchar, 0, "TermsPath")
                da.InsertCommand.Parameters.Add("@SideBarPath", SqlDbType.varchar, 0, "SideBarPath")
                da.InsertCommand.Parameters.Add("@JSPath", SqlDbType.varchar, 0, "JSPath")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadProgramEntryForm").NewRow
            End If
            Update_Field("Description", _Description, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("HTMLPath", _HTMLPath, dr)
            Update_Field("TermsPath", _TermsPath, dr)
            Update_Field("SideBarPath", _SideBarPath, dr)
            Update_Field("JSPath", _JSPath, dr)
            If ds.Tables("t_LeadProgramEntryForm").Rows.Count < 1 Then ds.Tables("t_LeadProgramEntryForm").Rows.Add(dr)
            da.Update(ds, "t_LeadProgramEntryForm")
            _ID = ds.Tables("t_LeadProgramEntryForm").Rows(0).Item("ID")
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

    Public Function List_Forms() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ID, Description from t_LeadProgramEntryForm order by description"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public Property HTMLPath() As String
        Get
            Return _HTMLPath
        End Get
        Set(ByVal value As String)
            _HTMLPath = value
        End Set
    End Property

    Public Property TermsPath() As String
        Get
            Return _TermsPath
        End Get
        Set(ByVal value As String)
            _TermsPath = value
        End Set
    End Property

    Public Property SideBarPath() As String
        Get
            Return _SideBarPath
        End Get
        Set(ByVal value As String)
            _SideBarPath = value
        End Set
    End Property

    Public Property JSPath() As String
        Get
            Return _JSPath
        End Get
        Set(ByVal value As String)
            _JSPath = value
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

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
