Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSalesInventory2ContractHist
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _SalesInventoryID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _DateAdded As String = ""
    Dim _DateRemoved As String = ""
    Dim _Active As Boolean = False
    Dim _HideFromContract As Boolean = False
    Dim _FrequencyID As Integer = 0
    Dim _SeasonID As Integer = 0
    Dim _OccupancyYear As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SalesInventory2ContractHist where SalesInventoryContractHistID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If _ID = 0 Then
                cm.CommandText = "Select * from t_SalesInventory2ContractHist where ContractID = " & _ContractID & " and SalesinventoryId = " & _SalesInventoryID
            Else
                cm.CommandText = "Select * from t_SalesInventory2ContractHist where SalesInventoryContractHistID = " & _ID
            End If
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SalesInventory2ContractHist")
            If ds.Tables("t_SalesInventory2ContractHist").Rows.Count > 0 Then
                dr = ds.Tables("t_SalesInventory2ContractHist").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("SalesInventoryID") Is System.DBNull.Value) Then _SalesInventoryID = dr("SalesInventoryID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("DateRemoved") Is System.DBNull.Value) Then _DateRemoved = dr("DateRemoved")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("HideFromContract") Is System.DBNull.Value) Then _HideFromContract = dr("HideFromContract")
        If Not (dr("FrequencyID") Is System.DBNull.Value) Then _FrequencyID = dr("FrequencyID")
        If Not (dr("SeasonID") Is System.DBNull.Value) Then _SeasonID = dr("SeasonID")
        If Not (dr("OccupancyYear") Is System.DBNull.Value) Then _OccupancyYear = dr("OccupancyYear")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SalesInventory2ContractHist where SalesInventoryContractHistID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SalesInventory2ContractHist")
            If ds.Tables("t_SalesInventory2ContractHist").Rows.Count > 0 Then
                dr = ds.Tables("t_SalesInventory2ContractHist").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SalesInventory2ContractHistInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@SalesInventoryID", SqlDbType.int, 0, "SalesInventoryID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.smalldatetime, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@DateRemoved", SqlDbType.smalldatetime, 0, "DateRemoved")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@HideFromContract", SqlDbType.bit, 0, "HideFromContract")
                da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.int, 0, "FrequencyID")
                da.InsertCommand.Parameters.Add("@SeasonID", SqlDbType.int, 0, "SeasonID")
                da.InsertCommand.Parameters.Add("@OccupancyYear", SqlDbType.int, 0, "OccupancyYear")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SalesInventoryContractHistID", SqlDbType.Int, 0, "SalesInventoryContractHistID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SalesInventory2ContractHist").NewRow
            End If
            Update_Field("SalesInventoryID", _SalesInventoryID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("DateRemoved", _DateRemoved, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("HideFromContract", _HideFromContract, dr)
            Update_Field("FrequencyID", _FrequencyID, dr)
            Update_Field("SeasonID", _SeasonID, dr)
            Update_Field("OccupancyYear", _OccupancyYear, dr)
            If ds.Tables("t_SalesInventory2ContractHist").Rows.Count < 1 Then ds.Tables("t_SalesInventory2ContractHist").Rows.Add(dr)
            da.Update(ds, "t_SalesInventory2ContractHist")
            _ID = ds.Tables("t_SalesInventory2ContractHist").Rows(0).Item("SalesInventoryContractHistID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try

    End Function

    Public Function List(Optional ByVal iLimit As Integer = 0, Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " so.SalesInventoryContractHistID as ID,  u.Name, s.Week, so.OccupancyYear, f.Frequency, so.Active From t_Salesinventory2contracthist so inner join t_Salesinventory s on s.salesinventoryid = so.salesinventoryid inner join t_Unit u on u.unitid = s.unitid left outer join t_Frequency f on so.FrequencyID = f.FrequencyID"
            sql += " where so.contractid = '" & _ContractID & "' and Hidefromcontract <> 1"
            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
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
            oEvents.KeyField = "SalesInventoryContractHistID"
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

    Public Function Release_Inventory(ByVal ID As Integer, ByVal invID As Integer) As Boolean
        Dim bReleased As Integer = True
        Dim oInvHist As New clsSalesInventory2ContractHist
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SalesInventory2ContractHist where salesinventoryid = " & invID & " and contractID = " & ID & " and dateremoved is null"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    oInvHist.SalesInventoryContractHistID = dread("SalesInventoryContractHistID")
                    oInvHist.Load()
                    oInvHist.DateRemoved = System.DateTime.Now
                    oInvHist.Active = False
                    oInvHist.Save()
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bReleased
    End Function

    Public Function Add_Inventory(ByVal ID As Integer, ByVal invID As Integer) As Boolean
        Dim bAdded As Boolean = True
        Dim oContract As New clsContract
        Dim oInvHist As New clsSalesInventory2ContractHist
        oContract.ContractID = ID
        oContract.Load()
        oInvHist.SalesInventoryContractHistID = 0
        oInvHist.Load()
        oInvHist.ContractID = ID
        oInvHist.DateAdded = System.DateTime.Now
        oInvHist.HideFromContract = False
        oInvHist.SeasonID = oContract.SeasonID
        oInvHist.SalesInventoryID = invID
        oInvHist.FrequencyID = oContract.FrequencyID
        oInvHist.OccupancyYear = Year(oContract.OccupancyDate)
        oInvHist.Active = True
        oInvHist.Save()
        oContract = Nothing
        oInvHist = Nothing
        Return bAdded
    End Function

    Public Property SalesInventoryID() As Integer
        Get
            Return _SalesInventoryID
        End Get
        Set(ByVal value As Integer)
            _SalesInventoryID = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property DateAdded() As String
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property DateRemoved() As String
        Get
            Return _DateRemoved
        End Get
        Set(ByVal value As String)
            _DateRemoved = value
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

    Public Property HideFromContract() As Boolean
        Get
            Return _HideFromContract
        End Get
        Set(ByVal value As Boolean)
            _HideFromContract = value
        End Set
    End Property

    Public Property FrequencyID() As Integer
        Get
            Return _FrequencyID
        End Get
        Set(ByVal value As Integer)
            _FrequencyID = value
        End Set
    End Property

    Public Property SeasonID() As Integer
        Get
            Return _SeasonID
        End Get
        Set(ByVal value As Integer)
            _SeasonID = value
        End Set
    End Property

    Public Property OccupancyYear() As Integer
        Get
            Return _OccupancyYear
        End Get
        Set(ByVal value As Integer)
            _OccupancyYear = value
        End Set
    End Property

    Public Property SalesInventoryContractHistID() As Integer
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
End Class
