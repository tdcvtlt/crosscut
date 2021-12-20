Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsMaintenanceFeeCodes
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Code As String = ""
    Dim _Phase As String = ""
    Dim _Size As Integer = 0
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_MaintenanceFeeCodes where MaintenanceFeeCodeID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_MaintenanceFeeCodes where MaintenanceFeeCodeID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_MaintenanceFeeCodes")
            If ds.Tables("t_MaintenanceFeeCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_MaintenanceFeeCodes").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Code") Is System.DBNull.Value) Then _Code = dr("Code")
        If Not (dr("Phase") Is System.DBNull.Value) Then _Phase = dr("Phase")
        If Not (dr("Size") Is System.DBNull.Value) Then _Size = dr("Size")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function List(Optional ByVal bIncludeBlank As Boolean = False) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If bIncludeBlank Then
                ds.SelectCommand = "Select 0 as MaintenanceFeeCodeID, '(Empty)' as Code, '' as Phase, 0 as Size union Select MaintenanceFeeCodeID, Code, Phase, Size from t_MaintenanceFeeCodes order by code"
            Else
                ds.SelectCommand = "Select * from t_MaintenanceFeeCodes order by code"
            End If

        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Search(ByVal filter As String, ByVal filterOption As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If filterOption = "maintenancefeecodeid" Then
                If filter = "" Then
                    ds.SelectCommand = "Select Top 50 * from t_MaintenanceFeeCodes order by Code asc"
                Else
                    ds.SelectCommand = "Select Top 50 * from t_MaintenanceFeeCodes where maintenancefeecodeid like '" & filter & "%' order maintenancefeecodeid asc"
                End If
            ElseIf filterOption = "code" Then
                If filter = "" Then
                    ds.SelectCommand = "Select Top 50 * from t_MaintenanceFeeCodes order by code asc"
                Else
                    ds.SelectCommand = "Select Top 50 * from t_MaintenanceFeeCodes where code like '" & filter & "%' order by code asc"
                End If
            Else
                ds.SelectCommand = "Select Top 50 * from t_MaintenanceFeeCodes order by Code asc"
            End If

        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_MaintenanceFeeCodes where MaintenanceFeeCodeID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_MaintenanceFeeCodes")
            If ds.Tables("t_MaintenanceFeeCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_MaintenanceFeeCodes").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_MaintenanceFeeCodesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Code", SqlDbType.VarChar, 0, "Code")
                da.InsertCommand.Parameters.Add("@Phase", SqlDbType.VarChar, 0, "Phase")
                da.InsertCommand.Parameters.Add("@Size", SqlDbType.Int, 0, "Size")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.Text, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@MaintenanceFeeCodeID", SqlDbType.Int, 0, "MaintenanceFeeCodeID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_MaintenanceFeeCodes").NewRow
            End If
            Update_Field("Code", _Code, dr)
            Update_Field("Phase", _Phase, dr)
            Update_Field("Size", _Size, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_MaintenanceFeeCodes").Rows.Count < 1 Then ds.Tables("t_MaintenanceFeeCodes").Rows.Add(dr)
            da.Update(ds, "t_MaintenanceFeeCodes")
            _ID = ds.Tables("t_MaintenanceFeeCodes").Rows(0).Item("MaintenanceFeeCodeID")
            ds = Nothing
            da = Nothing
            sqlCMBuild = Nothing
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            ds = Nothing
            da = Nothing
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "MaintenanceFeeCodeID"
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

    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(ByVal value As String)
            _Code = value
        End Set
    End Property

    Public Property Phase() As String
        Get
            Return _Phase
        End Get
        Set(ByVal value As String)
            _Phase = value
        End Set
    End Property

    Public Property Size() As Integer
        Get
            Return _Size
        End Get
        Set(ByVal value As Integer)
            _Size = value
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

    Public Property MaintenanceFeeCodeID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property
End Class
