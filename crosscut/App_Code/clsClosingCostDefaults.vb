Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsClosingCostDefaults
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FieldName As String = ""
    Dim _IsFixed As Boolean = False
    Dim _FixedValue As Decimal = 0
    Dim _IsCalc As Boolean = False
    Dim _Formula As String = ""
    Dim _IsLookup As Boolean = False
    Dim _LookupFieldName As String = ""
    Dim _LookupTableName As String = ""
    Dim _LookupDataTypeID As Integer = 0
    Dim _Active As Boolean = False
    Dim _OPTIONAL As Boolean = False
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ClosingCostDefaults where FieldID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ClosingCostDefaults where FieldID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ClosingCostDefaults")
            If ds.Tables("t_ClosingCostDefaults").Rows.Count > 0 Then
                dr = ds.Tables("t_ClosingCostDefaults").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(Optional ByVal bShowInActive As Boolean = False) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select * from t_ClosingCostDefaults "
            sql += IIf(bShowInActive, "", "where active =1")
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("FieldName") Is System.DBNull.Value) Then _FieldName = dr("FieldName")
        If Not (dr("IsFixed") Is System.DBNull.Value) Then _IsFixed = dr("IsFixed")
        If Not (dr("FixedValue") Is System.DBNull.Value) Then _FixedValue = dr("FixedValue")
        If Not (dr("IsCalc") Is System.DBNull.Value) Then _IsCalc = dr("IsCalc")
        If Not (dr("Formula") Is System.DBNull.Value) Then _Formula = dr("Formula")
        If Not (dr("IsLookup") Is System.DBNull.Value) Then _IsLookup = dr("IsLookup")
        If Not (dr("LookupFieldName") Is System.DBNull.Value) Then _LookupFieldName = dr("LookupFieldName")
        If Not (dr("LookupTableName") Is System.DBNull.Value) Then _LookupTableName = dr("LookupTableName")
        If Not (dr("LookupDataTypeID") Is System.DBNull.Value) Then _LookupDataTypeID = dr("LookupDataTypeID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("OPTIONAL") Is System.DBNull.Value) Then _OPTIONAL = dr("OPTIONAL")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ClosingCostDefaults where FieldID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ClosingCostDefaults")
            If ds.Tables("t_ClosingCostDefaults").Rows.Count > 0 Then
                dr = ds.Tables("t_ClosingCostDefaults").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ClosingCostDefaultsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FieldName", SqlDbType.varchar, 0, "FieldName")
                da.InsertCommand.Parameters.Add("@IsFixed", SqlDbType.bit, 0, "IsFixed")
                da.InsertCommand.Parameters.Add("@FixedValue", SqlDbType.money, 0, "FixedValue")
                da.InsertCommand.Parameters.Add("@IsCalc", SqlDbType.bit, 0, "IsCalc")
                da.InsertCommand.Parameters.Add("@Formula", SqlDbType.varchar, 0, "Formula")
                da.InsertCommand.Parameters.Add("@IsLookup", SqlDbType.bit, 0, "IsLookup")
                da.InsertCommand.Parameters.Add("@LookupFieldName", SqlDbType.varchar, 0, "LookupFieldName")
                da.InsertCommand.Parameters.Add("@LookupTableName", SqlDbType.varchar, 0, "LookupTableName")
                da.InsertCommand.Parameters.Add("@LookupDataTypeID", SqlDbType.int, 0, "LookupDataTypeID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@OPTIONAL", SqlDbType.bit, 0, "OPTIONAL")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FieldID", SqlDbType.Int, 0, "FieldID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ClosingCostDefaults").NewRow
            End If
            Update_Field("FieldName", _FieldName, dr)
            Update_Field("IsFixed", _IsFixed, dr)
            Update_Field("FixedValue", _FixedValue, dr)
            Update_Field("IsCalc", _IsCalc, dr)
            Update_Field("Formula", _Formula, dr)
            Update_Field("IsLookup", _IsLookup, dr)
            Update_Field("LookupFieldName", _LookupFieldName, dr)
            Update_Field("LookupTableName", _LookupTableName, dr)
            Update_Field("LookupDataTypeID", _LookupDataTypeID, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("OPTIONAL", _OPTIONAL, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_ClosingCostDefaults").Rows.Count < 1 Then ds.Tables("t_ClosingCostDefaults").Rows.Add(dr)
            da.Update(ds, "t_ClosingCostDefaults")
            _ID = ds.Tables("t_ClosingCostDefaults").Rows(0).Item("FieldID")
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
            oEvents.KeyField = "FieldID"
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

    Public Property FieldName() As String
        Get
            Return _FieldName
        End Get
        Set(ByVal value As String)
            _FieldName = value
        End Set
    End Property

    Public Property IsFixed() As Boolean
        Get
            Return _IsFixed
        End Get
        Set(ByVal value As Boolean)
            _IsFixed = value
        End Set
    End Property

    Public Property FixedValue() As Decimal
        Get
            Return _FixedValue
        End Get
        Set(ByVal value As Decimal)
            _FixedValue = value
        End Set
    End Property

    Public Property IsCalc() As Boolean
        Get
            Return _IsCalc
        End Get
        Set(ByVal value As Boolean)
            _IsCalc = value
        End Set
    End Property

    Public Property Formula() As String
        Get
            Return _Formula
        End Get
        Set(ByVal value As String)
            _Formula = value
        End Set
    End Property

    Public Property IsLookup() As Boolean
        Get
            Return _IsLookup
        End Get
        Set(ByVal value As Boolean)
            _IsLookup = value
        End Set
    End Property

    Public Property LookupFieldName() As String
        Get
            Return _LookupFieldName
        End Get
        Set(ByVal value As String)
            _LookupFieldName = value
        End Set
    End Property

    Public Property LookupTableName() As String
        Get
            Return _LookupTableName
        End Get
        Set(ByVal value As String)
            _LookupTableName = value
        End Set
    End Property

    Public Property LookupDataTypeID() As Integer
        Get
            Return _LookupDataTypeID
        End Get
        Set(ByVal value As Integer)
            _LookupDataTypeID = value
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

    Public Property isOptional() As Boolean
        Get
            Return _OPTIONAL
        End Get
        Set(ByVal value As Boolean)
            _OPTIONAL = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property FieldID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
