Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProspectContact
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _ContactMethodID As Integer = 0
    Dim _ContactResultID As Integer = 0
    Dim _ContactDate As String = ""
    Dim _Scheduled As Boolean = False
    Dim _PersonnelID As Integer = 0
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ProspectContact where Pros2ContactID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ProspectContact where Pros2ContactID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ProspectContact")
            If ds.Tables("t_ProspectContact").Rows.Count > 0 Then
                dr = ds.Tables("t_ProspectContact").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("ContactMethodID") Is System.DBNull.Value) Then _ContactMethodID = dr("ContactMethodID")
        If Not (dr("ContactResultID") Is System.DBNull.Value) Then _ContactResultID = dr("ContactResultID")
        If Not (dr("ContactDate") Is System.DBNull.Value) Then _ContactDate = dr("ContactDate")
        If Not (dr("Scheduled") Is System.DBNull.Value) Then _Scheduled = dr("Scheduled")
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProspectContact where Pros2ContactID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ProspectContact")
            If ds.Tables("t_ProspectContact").Rows.Count > 0 Then
                dr = ds.Tables("t_ProspectContact").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ProspectContactInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@ContactMethodID", SqlDbType.int, 0, "ContactMethodID")
                da.InsertCommand.Parameters.Add("@ContactResultID", SqlDbType.int, 0, "ContactResultID")
                da.InsertCommand.Parameters.Add("@ContactDate", SqlDbType.datetime, 0, "ContactDate")
                da.InsertCommand.Parameters.Add("@Scheduled", SqlDbType.bit, 0, "Scheduled")
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Pros2ContactID", SqlDbType.Int, 0, "Pros2ContactID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ProspectContact").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ContactMethodID", _ContactMethodID, dr)
            Update_Field("ContactResultID", _ContactResultID, dr)
            Update_Field("ContactDate", _ContactDate, dr)
            Update_Field("Scheduled", _Scheduled, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_ProspectContact").Rows.Count < 1 Then ds.Tables("t_ProspectContact").Rows.Add(dr)
            da.Update(ds, "t_ProspectContact")
            _ID = ds.Tables("t_ProspectContact").Rows(0).Item("Pros2ContactID")
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
            oEvents.KeyField = "Pros2ContactID"
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

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property ContactMethodID() As Integer
        Get
            Return _ContactMethodID
        End Get
        Set(ByVal value As Integer)
            _ContactMethodID = value
        End Set
    End Property

    Public Property ContactResultID() As Integer
        Get
            Return _ContactResultID
        End Get
        Set(ByVal value As Integer)
            _ContactResultID = value
        End Set
    End Property

    Public Property ContactDate() As String
        Get
            Return _ContactDate
        End Get
        Set(ByVal value As String)
            _ContactDate = value
        End Set
    End Property

    Public Property Scheduled() As Boolean
        Get
            Return _Scheduled
        End Get
        Set(ByVal value As Boolean)
            _Scheduled = value
        End Set
    End Property

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
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

    Public Property Pros2ContactID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
