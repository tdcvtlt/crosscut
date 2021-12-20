Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadTemplateMapping
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LeadTemplateID As Integer = 0
    Dim _ColumnName As String = ""
    Dim _ColumnNumber As Integer = 0
    Dim _ConvertValue As Boolean = False
    Dim _Value As String = ""
    Dim _ConvertedValue As String = ""
    Dim _CheckDuplicates As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadTemplateMapping where LeadFieldMappingID = " & _ID, cn)
    End Sub

    Public Function List_All() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LeadTemplateMapping")
    End Function

    Public Function List(ByVal TemplateID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LeadTemplateMapping where LeadTemplateID = " & TemplateID)
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadTemplateMapping where LeadFieldMappingID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadTemplateMapping")
            If ds.Tables("t_LeadTemplateMapping").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadTemplateMapping").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LeadTemplateID") Is System.DBNull.Value) Then _LeadTemplateID = dr("LeadTemplateID")
        If Not (dr("ColumnName") Is System.DBNull.Value) Then _ColumnName = dr("ColumnName")
        If Not (dr("ColumnNumber") Is System.DBNull.Value) Then _ColumnNumber = dr("ColumnNumber")
        If Not (dr("ConvertValue") Is System.DBNull.Value) Then _ConvertValue = dr("ConvertValue")
        If Not (dr("Value") Is System.DBNull.Value) Then _Value = dr("Value")
        If Not (dr("ConvertedValue") Is System.DBNull.Value) Then _ConvertedValue = dr("ConvertedValue")
        If Not (dr("CheckDuplicates") Is System.DBNull.Value) Then _CheckDuplicates = dr("CheckDuplicates")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadTemplateMapping where LeadFieldMappingID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadTemplateMapping")
            If ds.Tables("t_LeadTemplateMapping").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadTemplateMapping").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadTemplateMappingInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LeadTemplateID", SqlDbType.int, 0, "LeadTemplateID")
                da.InsertCommand.Parameters.Add("@ColumnName", SqlDbType.varchar, 0, "ColumnName")
                da.InsertCommand.Parameters.Add("@ColumnNumber", SqlDbType.int, 0, "ColumnNumber")
                da.InsertCommand.Parameters.Add("@ConvertValue", SqlDbType.bit, 0, "ConvertValue")
                da.InsertCommand.Parameters.Add("@Value", SqlDbType.varchar, 0, "Value")
                da.InsertCommand.Parameters.Add("@ConvertedValue", SqlDbType.varchar, 0, "ConvertedValue")
                da.InsertCommand.Parameters.Add("@CheckDuplicates", SqlDbType.bit, 0, "CheckDuplicates")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LeadFieldMappingID", SqlDbType.Int, 0, "LeadFieldMappingID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadTemplateMapping").NewRow
            End If
            Update_Field("LeadTemplateID", _LeadTemplateID, dr)
            Update_Field("ColumnName", _ColumnName, dr)
            Update_Field("ColumnNumber", _ColumnNumber, dr)
            Update_Field("ConvertValue", _ConvertValue, dr)
            Update_Field("Value", _Value, dr)
            Update_Field("ConvertedValue", _ConvertedValue, dr)
            Update_Field("CheckDuplicates", _CheckDuplicates, dr)
            If ds.Tables("t_LeadTemplateMapping").Rows.Count < 1 Then ds.Tables("t_LeadTemplateMapping").Rows.Add(dr)
            da.Update(ds, "t_LeadTemplateMapping")
            _ID = ds.Tables("t_LeadTemplateMapping").Rows(0).Item("LeadFieldMappingID")
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
            oEvents.KeyField = "LeadFieldMappingID"
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

    Public Property LeadTemplateID() As Integer
        Get
            Return _LeadTemplateID
        End Get
        Set(ByVal value As Integer)
            _LeadTemplateID = value
        End Set
    End Property

    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal value As String)
            _ColumnName = value
        End Set
    End Property

    Public Property ColumnNumber() As Integer
        Get
            Return _ColumnNumber
        End Get
        Set(ByVal value As Integer)
            _ColumnNumber = value
        End Set
    End Property

    Public Property ConvertValue() As Boolean
        Get
            Return _ConvertValue
        End Get
        Set(ByVal value As Boolean)
            _ConvertValue = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Public Property ConvertedValue() As String
        Get
            Return _ConvertedValue
        End Get
        Set(ByVal value As String)
            _ConvertedValue = value
        End Set
    End Property

    Public Property CheckDuplicates() As Boolean
        Get
            Return _CheckDuplicates
        End Get
        Set(ByVal value As Boolean)
            _CheckDuplicates = value
        End Set
    End Property

    Public Property LeadFieldMappingID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
