Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient


Public Class clsSalesInventory
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _UnitID As Integer = 0
    Dim _Week As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _SeasonID As Integer = 0
    Dim _WeekTypeID As Integer = 0
    Dim _BudgetedPrice As Decimal = 0
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _Hide As Boolean = False
    Dim _Force As Boolean = False
    Dim _ForcedFreqID As Integer = 0
    Dim _CRMSID As Integer = 0
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
        cm = New SqlCommand("Select * from t_SalesInventory where SalesInventoryID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SalesInventory where SalesInventoryID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SalesInventory")
            If ds.Tables("t_SalesInventory").Rows.Count > 0 Then
                dr = ds.Tables("t_SalesInventory").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("UnitID") Is System.DBNull.Value) Then _UnitID = dr("UnitID")
        If Not (dr("Week") Is System.DBNull.Value) Then _Week = dr("Week")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("SeasonID") Is System.DBNull.Value) Then _SeasonID = dr("SeasonID")
        If Not (dr("WeekTypeID") Is System.DBNull.Value) Then _WeekTypeID = dr("WeekTypeID")
        If Not (dr("BudgetedPrice") Is System.DBNull.Value) Then _BudgetedPrice = dr("BudgetedPrice")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("Hide") Is System.DBNull.Value) Then _Hide = dr("Hide")
        If Not (dr("Force") Is System.DBNull.Value) Then _Force = dr("Force")
        If Not (dr("ForcedFreqID") Is System.DBNull.Value) Then _ForcedFreqID = dr("ForcedFreqID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("Points") Is System.DBNull.Value) Then _Points = dr("Points")

    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SalesInventory where SalesInventoryID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SalesInventory")
            If ds.Tables("t_SalesInventory").Rows.Count > 0 Then
                dr = ds.Tables("t_SalesInventory").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SalesInventoryInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@UnitID", SqlDbType.int, 0, "UnitID")
                da.InsertCommand.Parameters.Add("@Week", SqlDbType.int, 0, "Week")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.datetime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@SeasonID", SqlDbType.int, 0, "SeasonID")
                da.InsertCommand.Parameters.Add("@WeekTypeID", SqlDbType.int, 0, "WeekTypeID")
                da.InsertCommand.Parameters.Add("@BudgetedPrice", SqlDbType.float, 0, "BudgetedPrice")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@Hide", SqlDbType.bit, 0, "Hide")
                da.InsertCommand.Parameters.Add("@Force", SqlDbType.bit, 0, "Force")
                da.InsertCommand.Parameters.Add("@ForcedFreqID", SqlDbType.int, 0, "ForcedFreqID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@Points", SqlDbType.Int, 0, "Points")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SalesInventoryID", SqlDbType.Int, 0, "SalesInventoryID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SalesInventory").NewRow
            End If
            Update_Field("UnitID", _UnitID, dr)
            Update_Field("Week", _Week, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("SeasonID", _SeasonID, dr)
            Update_Field("WeekTypeID", _WeekTypeID, dr)
            Update_Field("BudgetedPrice", _BudgetedPrice, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("Hide", _Hide, dr)
            Update_Field("Force", _Force, dr)
            Update_Field("ForcedFreqID", _ForcedFreqID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("Points", _Points, dr)
            If ds.Tables("t_SalesInventory").Rows.Count < 1 Then ds.Tables("t_SalesInventory").Rows.Add(dr)
            da.Update(ds, "t_SalesInventory")
            _ID = ds.Tables("t_SalesInventory").Rows(0).Item("SalesInventoryID")
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
            oEvents.KeyField = "SalesInventoryID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Get_Available_Inventory(ByVal iReqFreq As Integer, ByVal iReqOccYear As Integer, ByVal iSeasonID As Integer, ByVal sFilter As String) As SqlDataSource
        Dim sSQL As String = "select i.week,i.salesinventoryid,u.name from t_Salesinventory i left outer join (select * from t_ResaleInventory r where Available=0 and vendorid = 0) r on r.SalesInventoryID= i.SalesInventoryID inner join t_Unit u on u.unitid = i.unitid left outer join t_SoldInventory s on s.salesinventoryid = i.salesinventoryid where s.salesinventoryid is null and u.name like '" & sFilter & "%' and r.salesinventoryid is null and i.seasonid = " & iSeasonID & _
             "union " & _
             "select distinct i.week,i.salesinventoryid,u.name from t_Salesinventory i inner join t_Unit u on u.unitid = i.unitid inner join (select salesinventoryid, frequencyid, occupancyyear from t_SoldInventory union select salesinventoryid, frequencyid, occupancyyear from t_Resaleinventory where available=0 and vendorid = 0) s on s.salesinventoryid = i.salesinventoryid where not(i.salesinventoryid in (select salesinventoryid from (select salesinventoryid, frequencyid, occupancyyear from t_SoldInventory union select salesinventoryid, frequencyid, occupancyyear from t_Resaleinventory where available=0 and vendorid = 0) x where frequencyid = " & iReqFreq & " and ((" & iReqOccYear & ") -((occupancyyear))) % ((select [Interval] from t_Frequency where FrequencyID = " & iReqFreq & ")) = 0)) and s.frequencyid = " & iReqFreq & " and u.name like '" & sFilter & "%' and i.seasonid = " & iSeasonID & " and i.salesinventoryID not in (Select SalesInventoryID from t_SoldInventory where frequencyid <> " & iReqFreq & " union Select SalesInventoryID from t_ResaleInventory where frequencyid <> " & iReqFreq & " and available = 0 and vendorid = 0) order by u.name"
        Dim ds As New SqlDataSource(Resources.Resource.cns, sSQL)
        Return ds
        
    End Function

    Public Function Get_Table() As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select i.Week,i.SalesInventoryID, u.Name from t_Salesinventory i inner join t_Unit u on u.unitid = i.unitid where 1=2", cn)
        Dim dr As SqlDataReader

        Dim i As Integer
        Try
            cn.Open()
            dr = cm.ExecuteReader
            dr.Read()
            For i = 0 To dr.VisibleFieldCount - 1
                dt.Columns.Add(dr.GetName(i))
            Next
            dt.Columns.Add("Dirty")
            
            dr.Close()
            cn.Close()

        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try

        Return dt
    End Function

    Public Function Assign_Inventory(ByVal ContractID As Integer, ByVal SalesinventoryID As Integer) As Boolean
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_Soldinventory ", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim dr As DataRow
        'Try
        'fill the contract table
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_Contract where contractid = " & ContractID
        da.Fill(ds, "Contract")
        cm.CommandText = "Select * from t_Soldinventory where salesinventoryid=" & SalesinventoryID & " and abs(occupancyyear-" & Year(ds.Tables("Contract").Rows(0).Item("OccupancyDate")) & ") % (select interval from t_Frequency where frequencyid = " & ds.Tables("Contract").Rows(0).Item("FrequencyID") & ") = 0"
        da.Fill(ds, "Inventory")
        If ds.Tables("Inventory").Rows.Count > 0 Then
            _Err = "This inventory has already been assigned"
            Return False
        Else
            Dim cmdBuilder As New SqlCommandBuilder(da)
            dr = ds.Tables("Inventory").NewRow
            Dim si As New clsSalesInventory
            si.SalesInventoryID = SalesinventoryID
            si.Load()
            dr("Points") = si.Points
            dr("SalesinventoryID") = SalesinventoryID
            dr("FrequencyID") = ds.Tables("Contract").Rows(0).Item("FrequencyID")
            dr("ContractID") = ContractID
            dr("SalesPrice") = 0
            dr("OccupancyYear") = Year(ds.Tables("Contract").Rows(0).Item("OccupancyDate"))
            ds.Tables("Inventory").Rows.Add(dr)
            da.Update(ds, "Inventory")
            si = Nothing
            Dim oHist As New clsSalesInventory2ContractHist
            oHist.Active = True
            oHist.ContractID = ds.Tables("Contract").Rows(0)("ContractID")
            oHist.DateAdded = Date.Now
            oHist.FrequencyID = ds.Tables("Contract").Rows(0)("FrequencyID")
            oHist.OccupancyYear = Year(ds.Tables("Contract").Rows(0)("OccupancyDate"))
            oHist.SalesInventoryID = SalesinventoryID
            oHist.SeasonID = ds.Tables("Contract").Rows(0)("SeasonID")
            oHist.Save()
            oHist = Nothing
            Return True
        End If

        'Catch ex As Exception
        '_Err = ex.ToString
        'Return False
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        dr = Nothing
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
        'End Try
    End Function

    Public Function List_Inventory(ByVal UnitID As Integer) As SqlDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select si.SalesInventoryID as ID, si.Week, s.ComboItem as Season, si.BudgetedPrice, si.Points from t_SalesInventory si left outer join t_ComboItems s on si.SeasonID = s.ComboItemID where si.UnitID = " & UnitID & " order by week asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Item_Exists(ByVal unitID As Integer, ByVal week As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Items from t_SalesInventory where unitid = " & unitID & " and week = " & week
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("Items") > 0 Then
                    bValid = False
                End If
            Else
                bValid = False
            End If
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property UnitID() As Integer
        Get
            Return _UnitID
        End Get
        Set(ByVal value As Integer)
            _UnitID = value
        End Set
    End Property

    Public Property Week() As Integer
        Get
            Return _Week
        End Get
        Set(ByVal value As Integer)
            _Week = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
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

    Public Property WeekTypeID() As Integer
        Get
            Return _WeekTypeID
        End Get
        Set(ByVal value As Integer)
            _WeekTypeID = value
        End Set
    End Property

    Public Property BudgetedPrice() As Decimal
        Get
            Return _BudgetedPrice
        End Get
        Set(ByVal value As Decimal)
            _BudgetedPrice = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property SubTypeID() As Integer
        Get
            Return _SubTypeID
        End Get
        Set(ByVal value As Integer)
            _SubTypeID = value
        End Set
    End Property

    Public Property Hide() As Boolean
        Get
            Return _Hide
        End Get
        Set(ByVal value As Boolean)
            _Hide = value
        End Set
    End Property

    Public Property Force() As Boolean
        Get
            Return _Force
        End Get
        Set(ByVal value As Boolean)
            _Force = value
        End Set
    End Property

    Public Property ForcedFreqID() As Integer
        Get
            Return _ForcedFreqID
        End Get
        Set(ByVal value As Integer)
            _ForcedFreqID = value
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

    Public Property Points() As Integer
        Get
            Return _Points
        End Get
        Set(value As Integer)
            _Points = value
        End Set
    End Property

    Public Property SalesInventoryID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

End Class
