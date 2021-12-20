Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadTemplates
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _NumberColumns As Integer = 0
    Dim _PhoneColumn As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadTemplates where LeadTemplateID = " & _ID, cn)
    End Sub

    Public Function List(ByVal bIncludeBlank As Boolean) As SqlDataSource
        If bIncludeBlank Then
            Return New SqlDataSource(Resources.Resource.cns, "Select 0 as LeadTemplateID, 'None' as Name union select leadtemplateid, name from t_LeadTemplates order by Name")
        Else
            Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LeadTemplates order by Name")
        End If
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadTemplates where LeadTemplateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadTemplates")
            If ds.Tables("t_LeadTemplates").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadTemplates").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("NumberColumns") Is System.DBNull.Value) Then _NumberColumns = dr("NumberColumns")
        If Not (dr("PhoneColumn") Is System.DBNull.Value) Then _PhoneColumn = dr("PhoneColumn")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadTemplates where LeadTemplateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadTemplates")
            If ds.Tables("t_LeadTemplates").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadTemplates").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadTemplatesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@NumberColumns", SqlDbType.int, 0, "NumberColumns")
                da.InsertCommand.Parameters.Add("@PhoneColumn", SqlDbType.int, 0, "PhoneColumn")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LeadTemplateID", SqlDbType.Int, 0, "LeadTemplateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadTemplates").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("NumberColumns", _NumberColumns, dr)
            Update_Field("PhoneColumn", _PhoneColumn, dr)
            If ds.Tables("t_LeadTemplates").Rows.Count < 1 Then ds.Tables("t_LeadTemplates").Rows.Add(dr)
            da.Update(ds, "t_LeadTemplates")
            _ID = ds.Tables("t_LeadTemplates").Rows(0).Item("LeadTemplateID")
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
            oEvents.KeyField = "LeadTemplateID"
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

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property NumberColumns() As Integer
        Get
            Return _NumberColumns
        End Get
        Set(ByVal value As Integer)
            _NumberColumns = value
        End Set
    End Property

    Public Property PhoneColumn() As Integer
        Get
            Return _PhoneColumn
        End Get
        Set(ByVal value As Integer)
            _PhoneColumn = value
        End Set
    End Property

    Public Property LeadTemplateID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
