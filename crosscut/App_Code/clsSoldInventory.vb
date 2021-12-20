Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSoldInventory
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _SalesInventoryID As Integer = 0
    Dim _FrequencyID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _SalesPrice As Decimal = 0
    Dim _OccupancyYear As Integer = 0
    Dim _Points As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SoldInventory where SoldInventoryID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            If _ID = 0 Then
                cm.CommandText = "Select * from t_Soldinventory where Contractid = " & _ContractID & " and SalesinventoryID = " & _SalesInventoryID
            Else
                cm.CommandText = "Select * from t_SoldInventory where SoldInventoryID = " & _ID
            End If

            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SoldInventory")
            If ds.Tables("t_SoldInventory").Rows.Count > 0 Then
                dr = ds.Tables("t_SoldInventory").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(Optional ByVal iLimit As Integer = 0, Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " so.SoldInventoryID as ID,  u.Name, s.Week, so.OccupancyYear From t_Soldinventory so inner join t_Salesinventory s on s.salesinventoryid = so.salesinventoryid inner join t_Unit u on u.unitid = s.unitid"
            sql += " where so.contractid = '" & _ContractID & "' "
            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("SalesInventoryID") Is System.DBNull.Value) Then _SalesInventoryID = dr("SalesInventoryID")
        If Not (dr("FrequencyID") Is System.DBNull.Value) Then _FrequencyID = dr("FrequencyID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("SalesPrice") Is System.DBNull.Value) Then _SalesPrice = dr("SalesPrice")
        If Not (dr("OccupancyYear") Is System.DBNull.Value) Then _OccupancyYear = dr("OccupancyYear")
        If Not (dr("Points") Is System.DBNull.Value) Then _Points = dr("Points")
    End Sub

    Public Function Delete() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_Soldinventory where contractid = " & _ContractID & " and salesinventoryid = " & _SalesInventoryID
            cm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SoldInventory where SoldInventoryID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SoldInventory")
            If ds.Tables("t_SoldInventory").Rows.Count > 0 Then
                dr = ds.Tables("t_SoldInventory").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SoldInventoryInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@SalesInventoryID", SqlDbType.int, 0, "SalesInventoryID")
                da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.int, 0, "FrequencyID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@SalesPrice", SqlDbType.money, 0, "SalesPrice")
                da.InsertCommand.Parameters.Add("@OccupancyYear", SqlDbType.Int, 0, "OccupancyYear")
                da.InsertCommand.Parameters.Add("@Points", SqlDbType.Int, 0, "Points")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SoldInventoryID", SqlDbType.Int, 0, "SoldInventoryID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SoldInventory").NewRow
            End If
            Update_Field("SalesInventoryID", _SalesInventoryID, dr)
            Update_Field("FrequencyID", _FrequencyID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("SalesPrice", _SalesPrice, dr)
            Update_Field("OccupancyYear", _OccupancyYear, dr)
            Update_Field("Points", _Points, dr)
            If ds.Tables("t_SoldInventory").Rows.Count < 1 Then ds.Tables("t_SoldInventory").Rows.Add(dr)
            da.Update(ds, "t_SoldInventory")
            _ID = ds.Tables("t_SoldInventory").Rows(0).Item("SoldInventoryID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "SoldInventoryID"
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

    Public Function Move_Inventory(ByVal oldID As Integer, ByVal newID As Integer) As Boolean
        Dim bMoved As Boolean = True
        Dim oSoldInv As New clsSoldInventory
        Dim oSalesInv As New clsSalesInventory2ContractHist
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SoldInventory where contractid = " & oldID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    oSoldInv.SoldInventoryID = dread("SoldInventoryID")
                    oSoldInv.Load()
                    oSoldInv.ContractID = newID
                    oSoldInv.Save()
                    oSalesInv.Release_Inventory(oldID, dread("SalesInventoryID"))
                    oSalesInv.Add_Inventory(newID, dread("SalesInventoryID"))
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bMoved
    End Function

    Public Function Get_Points_Value(ByVal conID As Integer) As Integer
        Dim points As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Sum(Points) is Null then 0 else sum(points) end as PointsValue from t_SoldInventory where contractid = '" & conID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                points = dread("PointsValue")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return points
    End Function

    Public Function Get_UnitBD_Display(ByVal conid As Integer) As String
        Dim display As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(ut.ComboItem) as UnitType,(Select Sum(Cast(Left(unst.ComboItem, 1) as Integer)) as BD from t_SOldInventory sdi inner join t_Salesinventory sli on sdi.SalesInventoryID = sli.SalesInventoryID inner join t_unit un on sli.UnitID = un.UnitID inner join t_ComboItems unt on un.TypeID = unt.ComboItemID inner join t_Comboitems unst on un.SubTypeID = unst.ComboitemID where sdi.ContractID = '" & conid & "' and unt.ComboItem = ut.ComboItem) as BD from t_SoldInventory sdi inner join t_SalesInventory si on sdi.SalesInventoryID = si.SalesInventoryID inner join t_Unit u on si.UnitID = u.UnitID inner join t_ComboItems ut on u.TypeID = ut.ComboItemID inner join t_ComboItems ust on u.SubTypeID = ust.ComboItemID where sdi.ContractID = '" & conid & "' Group By ut.ComboItem, ust.ComboItem "
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If display = "" Then
                        display = dread("UnitType") & " " & dread("BD") & " BD"
                    Else
                        display = display & Chr(10) & dread("UnitTYpe") & " " & dread("BD") & " BD"
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Property SalesInventoryID() As Integer
        Get
            Return _SalesInventoryID
        End Get
        Set(ByVal value As Integer)
            _SalesInventoryID = value
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

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property SalesPrice() As Decimal
        Get
            Return _SalesPrice
        End Get
        Set(ByVal value As Decimal)
            _SalesPrice = value
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

    Public Property SoldInventoryID() As Integer
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

    Public Property Points() As Integer
        Get
            Return _Points
        End Get
        Set(value As Integer)
            _Points = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
