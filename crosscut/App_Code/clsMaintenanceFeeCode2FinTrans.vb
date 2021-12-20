Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsMaintenanceFeeCode2FinTrans
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _MaintenanceFeeCodeID As Integer = 0
    Dim _FinTransID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_MaintenanceFeeCode2FinTrans where MaintenanceFeeCode2FinTransID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_MaintenanceFeeCode2FinTrans where MaintenanceFeeCode2FinTransID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_MaintenanceFeeCode2FinTrans")
            If ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows.Count > 0 Then
                dr = ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("MaintenanceFeeCodeID") Is System.DBNull.Value) Then _MaintenanceFeeCodeID = dr("MaintenanceFeeCodeID")
        If Not (dr("FinTransID") Is System.DBNull.Value) Then _FinTransID = dr("FinTransID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
    End Sub

    Public Function List(Optional ByVal sFilter As String = "", Optional ByVal sFilterValue As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If sFilter = "" Or sFilterValue = "" Then
                ds.SelectCommand = "Select Maintenancefeecode2fintransid, mc.Code, tc.comboitem as TransactionCode, mfc.Amount from t_MaintenanceFeeCode2FinTrans mfc inner join t_Maintenancefeecodes mc on mc.maintenancefeecodeid = mfc.maintenancefeecodeid inner join t_FintransCodes f on f.fintransid = mfc.fintransid inner join t_Comboitems tc on tc.comboitemid = f.transcodeid "
            Else
                ds.SelectCommand = "Select Maintenancefeecode2fintransid, mc.code, tc.comboitem as TransactionCode, mfc.Amount from t_MaintenanceFeeCode2FinTrans mfc inner join t_Maintenancefeecodes mc on mc.maintenancefeecodeid = mfc.maintenancefeecodeid inner join t_FintransCodes f on f.fintransid = mfc.fintransid inner join t_Comboitems tc on tc.comboitemid = f.transcodeid  where " & sFilter & " like '" & sFilterValue & "' order by " & sFilter
            End If


        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_MaintenanceFeeCode2FinTrans where MaintenanceFeeCode2FinTransID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_MaintenanceFeeCode2FinTrans")
            If ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows.Count > 0 Then
                dr = ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_MaintenanceFeeCode2FinTransInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@MaintenanceFeeCodeID", SqlDbType.int, 0, "MaintenanceFeeCodeID")
                da.InsertCommand.Parameters.Add("@FinTransID", SqlDbType.int, 0, "FinTransID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@MaintenanceFeeCode2FinTransID", SqlDbType.Int, 0, "MaintenanceFeeCode2FinTransID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_MaintenanceFeeCode2FinTrans").NewRow
            End If
            Update_Field("MaintenanceFeeCodeID", _MaintenanceFeeCodeID, dr)
            Update_Field("FinTransID", _FinTransID, dr)
            Update_Field("Amount", _Amount, dr)
            If ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows.Count < 1 Then ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows.Add(dr)
            da.Update(ds, "t_MaintenanceFeeCode2FinTrans")
            _ID = ds.Tables("t_MaintenanceFeeCode2FinTrans").Rows(0).Item("MaintenanceFeeCode2FinTransID")
            da = Nothing
            ds = Nothing
            sqlCMBuild = Nothing
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            da = Nothing
            ds = Nothing
            If cn.State <> ConnectionState.Closed Then cn.Close()

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
            oEvents.KeyField = "MaintenanceFeeCode2FinTransID"
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

    Public Property MaintenanceFeeCodeID() As Integer
        Get
            Return _MaintenanceFeeCodeID
        End Get
        Set(ByVal value As Integer)
            _MaintenanceFeeCodeID = value
        End Set
    End Property

    Public Property FinTransID() As Integer
        Get
            Return _FinTransID
        End Get
        Set(ByVal value As Integer)
            _FinTransID = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Public Property MaintenanceFeeCode2FinTransID() As Integer
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

