Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCombinedOverviewAuto
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _OverviewDate As String = ""
    Dim _OverviewLocation As String = ""
    Dim _LocationID As Integer = 0
    Dim _LineToursAct As Integer = 0
    Dim _LineToursProj As Integer = 0
    Dim _LineSalesAct As Integer = 0
    Dim _LineSalesProj As Decimal = 0
    Dim _LineIntervalSales As Decimal = 0
    Dim _LineIntervalCan As Decimal = 0
    Dim _LineVolumeAct As Decimal = 0
    Dim _LineVolumeProj As Decimal = 0
    Dim _LineClosingProj As Decimal = 0
    Dim _IHToursAct As Integer = 0
    Dim _IHToursProj As Integer = 0
    Dim _IHSalesAct As Integer = 0
    Dim _IHSalesProj As Integer = 0
    Dim _IHIntervalsSales As Decimal = 0
    Dim _IHIntervalsCan As Decimal = 0
    Dim _IHVolumeAct As Decimal = 0
    Dim _IHVolumeProj As Decimal = 0
    Dim _IHUpgradesAct As Integer = 0
    Dim _IHUpgradesProj As Integer = 0
    Dim _IHUpgradeVolumeAct As Decimal = 0
    Dim _IHUpgradeVolumeProj As Decimal = 0
    Dim _CreatedByID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _LastUpdatedBy As Integer = 0
    Dim _DateUpdated As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CombinedOverviewAuto where CombinedOverViewID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CombinedOverviewAuto where CombinedOverViewID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CombinedOverviewAuto")
            If ds.Tables("t_CombinedOverviewAuto").Rows.Count > 0 Then
                dr = ds.Tables("t_CombinedOverviewAuto").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("OverviewDate") Is System.DBNull.Value) Then _OverviewDate = dr("OverviewDate")
        If Not (dr("OverviewLocation") Is System.DBNull.Value) Then _OverviewLocation = dr("OverviewLocation")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("LineToursAct") Is System.DBNull.Value) Then _LineToursAct = dr("LineToursAct")
        If Not (dr("LineToursProj") Is System.DBNull.Value) Then _LineToursProj = dr("LineToursProj")
        If Not (dr("LineSalesAct") Is System.DBNull.Value) Then _LineSalesAct = dr("LineSalesAct")
        If Not (dr("LineSalesProj") Is System.DBNull.Value) Then _LineSalesProj = dr("LineSalesProj")
        If Not (dr("LineIntervalSales") Is System.DBNull.Value) Then _LineIntervalSales = dr("LineIntervalSales")
        If Not (dr("LineIntervalCan") Is System.DBNull.Value) Then _LineIntervalCan = dr("LineIntervalCan")
        If Not (dr("LineVolumeAct") Is System.DBNull.Value) Then _LineVolumeAct = dr("LineVolumeAct")
        If Not (dr("LineVolumeProj") Is System.DBNull.Value) Then _LineVolumeProj = dr("LineVolumeProj")
        If Not (dr("LineClosingProj") Is System.DBNull.Value) Then _LineClosingProj = dr("LineClosingProj")
        If Not (dr("IHToursAct") Is System.DBNull.Value) Then _IHToursAct = dr("IHToursAct")
        If Not (dr("IHToursProj") Is System.DBNull.Value) Then _IHToursProj = dr("IHToursProj")
        If Not (dr("IHSalesAct") Is System.DBNull.Value) Then _IHSalesAct = dr("IHSalesAct")
        If Not (dr("IHSalesProj") Is System.DBNull.Value) Then _IHSalesProj = dr("IHSalesProj")
        If Not (dr("IHIntervalsSales") Is System.DBNull.Value) Then _IHIntervalsSales = dr("IHIntervalsSales")
        If Not (dr("IHIntervalsCan") Is System.DBNull.Value) Then _IHIntervalsCan = dr("IHIntervalsCan")
        If Not (dr("IHVolumeAct") Is System.DBNull.Value) Then _IHVolumeAct = dr("IHVolumeAct")
        If Not (dr("IHVolumeProj") Is System.DBNull.Value) Then _IHVolumeProj = dr("IHVolumeProj")
        If Not (dr("IHUpgradesAct") Is System.DBNull.Value) Then _IHUpgradesAct = dr("IHUpgradesAct")
        If Not (dr("IHUpgradesProj") Is System.DBNull.Value) Then _IHUpgradesProj = dr("IHUpgradesProj")
        If Not (dr("IHUpgradeVolumeAct") Is System.DBNull.Value) Then _IHUpgradeVolumeAct = dr("IHUpgradeVolumeAct")
        If Not (dr("IHUpgradeVolumeProj") Is System.DBNull.Value) Then _IHUpgradeVolumeProj = dr("IHUpgradeVolumeProj")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("LastUpdatedBy") Is System.DBNull.Value) Then _LastUpdatedBy = dr("LastUpdatedBy")
        If Not (dr("DateUpdated") Is System.DBNull.Value) Then _DateUpdated = dr("DateUpdated")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CombinedOverviewAuto where CombinedOverViewID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CombinedOverviewAuto")
            If ds.Tables("t_CombinedOverviewAuto").Rows.Count > 0 Then
                dr = ds.Tables("t_CombinedOverviewAuto").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CombinedOverviewAutoInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@OverviewDate", SqlDbType.datetime, 0, "OverviewDate")
                da.InsertCommand.Parameters.Add("@OverviewLocation", SqlDbType.varchar, 0, "OverviewLocation")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@LineToursAct", SqlDbType.int, 0, "LineToursAct")
                da.InsertCommand.Parameters.Add("@LineToursProj", SqlDbType.int, 0, "LineToursProj")
                da.InsertCommand.Parameters.Add("@LineSalesAct", SqlDbType.int, 0, "LineSalesAct")
                da.InsertCommand.Parameters.Add("@LineSalesProj", SqlDbType.float, 0, "LineSalesProj")
                da.InsertCommand.Parameters.Add("@LineIntervalSales", SqlDbType.money, 0, "LineIntervalSales")
                da.InsertCommand.Parameters.Add("@LineIntervalCan", SqlDbType.money, 0, "LineIntervalCan")
                da.InsertCommand.Parameters.Add("@LineVolumeAct", SqlDbType.money, 0, "LineVolumeAct")
                da.InsertCommand.Parameters.Add("@LineVolumeProj", SqlDbType.money, 0, "LineVolumeProj")
                da.InsertCommand.Parameters.Add("@LineClosingProj", SqlDbType.money, 0, "LineClosingProj")
                da.InsertCommand.Parameters.Add("@IHToursAct", SqlDbType.int, 0, "IHToursAct")
                da.InsertCommand.Parameters.Add("@IHToursProj", SqlDbType.int, 0, "IHToursProj")
                da.InsertCommand.Parameters.Add("@IHSalesAct", SqlDbType.int, 0, "IHSalesAct")
                da.InsertCommand.Parameters.Add("@IHSalesProj", SqlDbType.int, 0, "IHSalesProj")
                da.InsertCommand.Parameters.Add("@IHIntervalsSales", SqlDbType.money, 0, "IHIntervalsSales")
                da.InsertCommand.Parameters.Add("@IHIntervalsCan", SqlDbType.money, 0, "IHIntervalsCan")
                da.InsertCommand.Parameters.Add("@IHVolumeAct", SqlDbType.money, 0, "IHVolumeAct")
                da.InsertCommand.Parameters.Add("@IHVolumeProj", SqlDbType.money, 0, "IHVolumeProj")
                da.InsertCommand.Parameters.Add("@IHUpgradesAct", SqlDbType.int, 0, "IHUpgradesAct")
                da.InsertCommand.Parameters.Add("@IHUpgradesProj", SqlDbType.int, 0, "IHUpgradesProj")
                da.InsertCommand.Parameters.Add("@IHUpgradeVolumeAct", SqlDbType.money, 0, "IHUpgradeVolumeAct")
                da.InsertCommand.Parameters.Add("@IHUpgradeVolumeProj", SqlDbType.money, 0, "IHUpgradeVolumeProj")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@LastUpdatedBy", SqlDbType.int, 0, "LastUpdatedBy")
                da.InsertCommand.Parameters.Add("@DateUpdated", SqlDbType.datetime, 0, "DateUpdated")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CombinedOverViewID", SqlDbType.Int, 0, "CombinedOverViewID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CombinedOverviewAuto").NewRow
            End If
            Update_Field("OverviewDate", _OverviewDate, dr)
            Update_Field("OverviewLocation", _OverviewLocation, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("LineToursAct", _LineToursAct, dr)
            Update_Field("LineToursProj", _LineToursProj, dr)
            Update_Field("LineSalesAct", _LineSalesAct, dr)
            Update_Field("LineSalesProj", _LineSalesProj, dr)
            Update_Field("LineIntervalSales", _LineIntervalSales, dr)
            Update_Field("LineIntervalCan", _LineIntervalCan, dr)
            Update_Field("LineVolumeAct", _LineVolumeAct, dr)
            Update_Field("LineVolumeProj", _LineVolumeProj, dr)
            Update_Field("LineClosingProj", _LineClosingProj, dr)
            Update_Field("IHToursAct", _IHToursAct, dr)
            Update_Field("IHToursProj", _IHToursProj, dr)
            Update_Field("IHSalesAct", _IHSalesAct, dr)
            Update_Field("IHSalesProj", _IHSalesProj, dr)
            Update_Field("IHIntervalsSales", _IHIntervalsSales, dr)
            Update_Field("IHIntervalsCan", _IHIntervalsCan, dr)
            Update_Field("IHVolumeAct", _IHVolumeAct, dr)
            Update_Field("IHVolumeProj", _IHVolumeProj, dr)
            Update_Field("IHUpgradesAct", _IHUpgradesAct, dr)
            Update_Field("IHUpgradesProj", _IHUpgradesProj, dr)
            Update_Field("IHUpgradeVolumeAct", _IHUpgradeVolumeAct, dr)
            Update_Field("IHUpgradeVolumeProj", _IHUpgradeVolumeProj, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("LastUpdatedBy", _LastUpdatedBy, dr)
            Update_Field("DateUpdated", _DateUpdated, dr)
            If ds.Tables("t_CombinedOverviewAuto").Rows.Count < 1 Then ds.Tables("t_CombinedOverviewAuto").Rows.Add(dr)
            da.Update(ds, "t_CombinedOverviewAuto")
            _ID = ds.Tables("t_CombinedOverviewAuto").Rows(0).Item("CombinedOverViewID")
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
            oEvents.KeyField = "CombinedOverViewID"
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

    Public Function List(ByVal sDate As String, ByVal loc As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If sDate = "" Then
                ds.SelectCommand = "Select Top 50 CombinedOverViewID as ID, OverviewDate as OverView from t_CombinedOverviewAuto where overviewlocation = '" & loc & "' order by combinedoverviewid desc"
            Else
                ds.SelectCommand = "Select CombinedOverViewID as ID, OverviewDate as OverView from t_CombinedOverviewAuto where overviewdate = '" & sDate & "' and overviewlocation = '" & loc & "'"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_MTD_Total(ByVal sDate As String, ByVal loc As String, ByVal fieldName As String) As Double
        Dim MTDTotal As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Sum(" & fieldName & ") is null then 0 else sum(" & fieldName & ") end as MTDTotal from t_CombinedOverViewAuto where overviewlocation = '" & loc & "' and Month(OverViewDate) = Month('" & CDate(sDate) & "') and Year(OverViewDate) = Year('" & CDate(sDate) & "') and OverViewDate <= '" & CDate(sDate) & "'"
            dread = cm.ExecuteReader
            dread.Read()
            MTDTotal = dread("MTDTotal")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return MTDTotal
    End Function

    Public Function Get_YTD_Total(ByVal sDate As String, ByVal loc As String, ByVal fieldName As String) As Double
        Dim YTDTotal As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Sum(" & fieldName & ") is null then 0 else sum(" & fieldName & ") end as YTDTotal from t_CombinedOverViewAuto where overviewlocation = '" & loc & "' and Year(OverViewDate) = Year('" & CDate(sDate) & "') and OverViewDate <= '" & CDate(sDate) & "'"
            dread = cm.ExecuteReader
            dread.Read()
            YTDTotal = dread("YTDTotal")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return YTDTotal
    End Function

    Public Property OverviewDate() As String
        Get
            Return _OverviewDate
        End Get
        Set(ByVal value As String)
            _OverviewDate = value
        End Set
    End Property

    Public Property OverviewLocation() As String
        Get
            Return _OverviewLocation
        End Get
        Set(ByVal value As String)
            _OverviewLocation = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property LineToursAct() As Integer
        Get
            Return _LineToursAct
        End Get
        Set(ByVal value As Integer)
            _LineToursAct = value
        End Set
    End Property

    Public Property LineToursProj() As Integer
        Get
            Return _LineToursProj
        End Get
        Set(ByVal value As Integer)
            _LineToursProj = value
        End Set
    End Property

    Public Property LineSalesAct() As Integer
        Get
            Return _LineSalesAct
        End Get
        Set(ByVal value As Integer)
            _LineSalesAct = value
        End Set
    End Property

    Public Property LineSalesProj() As Decimal
        Get
            Return _LineSalesProj
        End Get
        Set(ByVal value As Decimal)
            _LineSalesProj = value
        End Set
    End Property

    Public Property LineIntervalSales() As Decimal
        Get
            Return _LineIntervalSales
        End Get
        Set(ByVal value As Decimal)
            _LineIntervalSales = value
        End Set
    End Property

    Public Property LineIntervalCan() As Decimal
        Get
            Return _LineIntervalCan
        End Get
        Set(ByVal value As Decimal)
            _LineIntervalCan = value
        End Set
    End Property

    Public Property LineVolumeAct() As Decimal
        Get
            Return _LineVolumeAct
        End Get
        Set(ByVal value As Decimal)
            _LineVolumeAct = value
        End Set
    End Property

    Public Property LineVolumeProj() As Decimal
        Get
            Return _LineVolumeProj
        End Get
        Set(ByVal value As Decimal)
            _LineVolumeProj = value
        End Set
    End Property

    Public Property LineClosingProj() As Decimal
        Get
            Return _LineClosingProj
        End Get
        Set(ByVal value As Decimal)
            _LineClosingProj = value
        End Set
    End Property

    Public Property IHToursAct() As Integer
        Get
            Return _IHToursAct
        End Get
        Set(ByVal value As Integer)
            _IHToursAct = value
        End Set
    End Property

    Public Property IHToursProj() As Integer
        Get
            Return _IHToursProj
        End Get
        Set(ByVal value As Integer)
            _IHToursProj = value
        End Set
    End Property

    Public Property IHSalesAct() As Integer
        Get
            Return _IHSalesAct
        End Get
        Set(ByVal value As Integer)
            _IHSalesAct = value
        End Set
    End Property

    Public Property IHSalesProj() As Integer
        Get
            Return _IHSalesProj
        End Get
        Set(ByVal value As Integer)
            _IHSalesProj = value
        End Set
    End Property

    Public Property IHIntervalsSales() As Decimal
        Get
            Return _IHIntervalsSales
        End Get
        Set(ByVal value As Decimal)
            _IHIntervalsSales = value
        End Set
    End Property

    Public Property IHIntervalsCan() As Decimal
        Get
            Return _IHIntervalsCan
        End Get
        Set(ByVal value As Decimal)
            _IHIntervalsCan = value
        End Set
    End Property

    Public Property IHVolumeAct() As Decimal
        Get
            Return _IHVolumeAct
        End Get
        Set(ByVal value As Decimal)
            _IHVolumeAct = value
        End Set
    End Property

    Public Property IHVolumeProj() As Decimal
        Get
            Return _IHVolumeProj
        End Get
        Set(ByVal value As Decimal)
            _IHVolumeProj = value
        End Set
    End Property

    Public Property IHUpgradesAct() As Integer
        Get
            Return _IHUpgradesAct
        End Get
        Set(ByVal value As Integer)
            _IHUpgradesAct = value
        End Set
    End Property

    Public Property IHUpgradesProj() As Integer
        Get
            Return _IHUpgradesProj
        End Get
        Set(ByVal value As Integer)
            _IHUpgradesProj = value
        End Set
    End Property

    Public Property IHUpgradeVolumeAct() As Decimal
        Get
            Return _IHUpgradeVolumeAct
        End Get
        Set(ByVal value As Decimal)
            _IHUpgradeVolumeAct = value
        End Set
    End Property

    Public Property IHUpgradeVolumeProj() As Decimal
        Get
            Return _IHUpgradeVolumeProj
        End Get
        Set(ByVal value As Decimal)
            _IHUpgradeVolumeProj = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property LastUpdatedBy() As Integer
        Get
            Return _LastUpdatedBy
        End Get
        Set(ByVal value As Integer)
            _LastUpdatedBy = value
        End Set
    End Property

    Public Property DateUpdated() As String
        Get
            Return _DateUpdated
        End Get
        Set(ByVal value As String)
            _DateUpdated = value
        End Set
    End Property

    Public Property CombinedOverViewID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

