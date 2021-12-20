Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadTemplateMappingItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LeadFieldMappingID As Integer = 0
    Dim _LookupValue As String = ""
    Dim _ConvertedValue As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadTemplateMappingItems where MapItemID = " & _ID, cn)
    End Sub

    Public Function List(id As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LeadTemplateMappingItems where LeadFieldMappingID = " & id)
    End Function

    Public Function Remove(id As Integer) As Boolean
        Dim ret As Boolean = False
        Try
            cn.Open()
            cm.CommandText = "Delete from t_LeadTemplateMappingItems where MapItemID = " & id
            cm.ExecuteNonQuery()
            cn.Close()
            ret = True
        Catch ex As Exception
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadTemplateMappingItems where MapItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadTemplateMappingItems")
            If ds.Tables("t_LeadTemplateMappingItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadTemplateMappingItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LeadFieldMappingID") Is System.DBNull.Value) Then _LeadFieldMappingID = dr("LeadFieldMappingID")
        If Not (dr("LookupValue") Is System.DBNull.Value) Then _LookupValue = dr("LookupValue")
        If Not (dr("ConvertedValue") Is System.DBNull.Value) Then _ConvertedValue = dr("ConvertedValue")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadTemplateMappingItems where MapItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadTemplateMappingItems")
            If ds.Tables("t_LeadTemplateMappingItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadTemplateMappingItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadTemplateMappingItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LeadFieldMappingID", SqlDbType.int, 0, "LeadFieldMappingID")
                da.InsertCommand.Parameters.Add("@LookupValue", SqlDbType.varchar, 0, "LookupValue")
                da.InsertCommand.Parameters.Add("@ConvertedValue", SqlDbType.varchar, 0, "ConvertedValue")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@MapItemID", SqlDbType.Int, 0, "MapItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadTemplateMappingItems").NewRow
            End If
            Update_Field("LeadFieldMappingID", _LeadFieldMappingID, dr)
            Update_Field("LookupValue", _LookupValue, dr)
            Update_Field("ConvertedValue", _ConvertedValue, dr)
            If ds.Tables("t_LeadTemplateMappingItems").Rows.Count < 1 Then ds.Tables("t_LeadTemplateMappingItems").Rows.Add(dr)
            da.Update(ds, "t_LeadTemplateMappingItems")
            _ID = ds.Tables("t_LeadTemplateMappingItems").Rows(0).Item("MapItemID")
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
            oEvents.KeyField = "MapItemID"
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

    Public Property LeadFieldMappingID() As Integer
        Get
            Return _LeadFieldMappingID
        End Get
        Set(ByVal value As Integer)
            _LeadFieldMappingID = value
        End Set
    End Property

    Public Property LookupValue() As String
        Get
            Return _LookupValue
        End Get
        Set(ByVal value As String)
            _LookupValue = value
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

    Public Property MapItemID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
End Class
