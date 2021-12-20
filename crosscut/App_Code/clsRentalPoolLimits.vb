Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRentalPoolLimits
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Year As Integer = 0
    Dim _UnitTypeID As Integer = 0
    Dim _RoomTypeID As Integer = 0
    Dim _CategoryID As Integer = 0
    Dim _Qty As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Dim dread2 As SqlDataReader
    Dim dread3 As SqlDataReader
    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RentalPoolLimits where RentalPoolID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RentalPoolLimits where RentalPoolID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RentalPoolLimits")
            If ds.Tables("t_RentalPoolLimits").Rows.Count > 0 Then
                dr = ds.Tables("t_RentalPoolLimits").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Year") Is System.DBNull.Value) Then _Year = dr("Year")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("RoomTypeID") Is System.DBNull.Value) Then _RoomTypeID = dr("RoomTypeID")
        If Not (dr("CategoryID") Is System.DBNull.Value) Then _CategoryID = dr("CategoryID")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RentalPoolLimits where RentalPoolID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RentalPoolLimits")
            If ds.Tables("t_RentalPoolLimits").Rows.Count > 0 Then
                dr = ds.Tables("t_RentalPoolLimits").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RentalPoolLimitsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Year", SqlDbType.int, 0, "Year")
                da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.int, 0, "UnitTypeID")
                da.InsertCommand.Parameters.Add("@RoomTypeID", SqlDbType.int, 0, "RoomTypeID")
                da.InsertCommand.Parameters.Add("@CategoryID", SqlDbType.int, 0, "CategoryID")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.int, 0, "Qty")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RentalPoolID", SqlDbType.Int, 0, "RentalPoolID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RentalPoolLimits").NewRow
            End If
            Update_Field("Year", _Year, dr)
            Update_Field("UnitTypeID", _UnitTypeID, dr)
            Update_Field("RoomTypeID", _RoomTypeID, dr)
            Update_Field("CategoryID", _CategoryID, dr)
            Update_Field("Qty", _Qty, dr)
            If ds.Tables("t_RentalPoolLimits").Rows.Count < 1 Then ds.Tables("t_RentalPoolLimits").Rows.Add(dr)
            da.Update(ds, "t_RentalPoolLimits")
            _ID = ds.Tables("t_RentalPoolLimits").Rows(0).Item("RentalPoolID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "RentalPoolID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function Get_Limit(ByVal inYear As Integer, ByVal uTypeID As Integer, ByVal rTypeID As Integer, ByVal category As Integer) As Integer
        Dim limit As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Qty from t_RentalPoolLimits where [year] = '" & inYear & "' and unittypeid = '" & uTypeID & "' and roomTypeID = '" & rTypeID & "' and categoryID = '" & category & "'"
            dread = cm.ExecuteReader()
            dread.Read()
            limit = dread("Qty")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return limit
    End Function
    Public Function Get_Current_Amt(ByVal usageID As Integer, ByVal inYear As Integer, ByVal utypeID As Integer, ByVal rTypeID As Integer, ByVal category As Integer) As Integer
        Dim limit As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(Distinct(u.UsageID)) As Usages from t_Usage u inner join t_Comboitems us on u.StatusID = us.ComboItemID where u.UsageID <> '" & usageID & "' and Year(inDate) = '" & inYear & "' and unittypeid = '" & utypeID & "' and roomTypeID = '" & rTypeID & "' and categoryID = '" & category & "' and us.ComboItem = 'Used'"
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                dread.Read()
                limit = dread("Usages")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return limit
    End Function
    Public Function Add_Pool_Year(ByVal pYear As Integer) As Boolean
        Dim bAdded As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            ds = New DataSet
            da = New SqlDataAdapter(cm)
            cm.CommandText = "Select c.ComboItemID from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.ComboName = 'UnitType'  and c.Active = 1 order by c.ComboItem asc"
            da.Fill(ds, "UnitType")
            For i = 0 To ds.Tables("UnitType").Rows.Count - 1
                cm.CommandText = "Select c.ComboItemID from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.ComboName = 'RoomType'  and c.Active = 1 order by Left(c.ComboItem, 1) asc"
                da.Fill(ds, "RoomType")
                For j = 0 To ds.Tables("RoomType").Rows.Count - 1
                    cm.CommandText = "Select c.ComboItemID from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.ComboName = 'UsageCategory'  and c.Active = 1 order by Left(c.ComboItem, 1) asc"
                    da.Fill(ds, "Category")
                    For k = 0 To ds.Tables("Category").Rows.Count - 1
                        cm.CommandText = "Insert into t_RentalPoolLimits(Year, UnitTYpeID, RoomTypeID, CategoryID, Qty) Values (" & pYear & "," & ds.Tables("UnitType").Rows(i).Item("ComboItemID") & "," & ds.Tables("RoomType").Rows(j).Item("ComboItemID") & "," & ds.Tables("Category").Rows(k).Item("ComboItemID") & ",0)"
                        cm.ExecuteNonQuery()
                    Next
                    ds.Tables("Category").Clear()
                Next
                ds.Tables("RoomType").Clear()
            Next
            da.Dispose()
        Catch ex As Exception
            _Err = ex.Message
            bAdded = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bAdded
    End Function
    Public Function List_Limits_By_Year(ByVal pYear As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select r.RentalPoolID As ID, ut.ComboItem as UnitType, rt.ComboItem as RoomType, cat.ComboItem as Category, r.Qty, (Select Case when Count(Distinct(u.UsageID)) is null then 0 else Count(Distinct(u.UsageID)) end from t_Usage u inner join t_Comboitems urt on u.TypeID = urt.ComboItemID inner join t_ComboItems ust on u.StatusID = ust.ComboItemID where Year(u.InDate) = '" & pYear & "' and ust.ComboItem = 'Used' and urt.ComboItem = 'Rental' and u.RoomTypeID = r.RoomTypeID and u.UnitTYpeID = r.UnitTypeID and u.CategoryID = r.CategoryID) as QtyUsed from t_RentalPoolLimits r inner join t_ComboItems rt on r.RoomTypeID = rt.ComboItemID inner join t_ComboItems ut on r.UnitTypeID = ut.ComboItemID inner join t_ComboItems cat on r.CategoryID = cat.ComboItemID where r.Year = " & pYear
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Limit_Years() As SQLDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select Distinct(Year) from t_RentalPoolLimits order by Year asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Year() As Integer
        Get
            Return _Year
        End Get
        Set(ByVal value As Integer)
            _Year = value
        End Set
    End Property

    Public Property UnitTypeID() As Integer
        Get
            Return _UnitTypeID
        End Get
        Set(ByVal value As Integer)
            _UnitTypeID = value
        End Set
    End Property

    Public Property RoomTypeID() As Integer
        Get
            Return _RoomTypeID
        End Get
        Set(ByVal value As Integer)
            _RoomTypeID = value
        End Set
    End Property

    Public Property CategoryID() As Integer
        Get
            Return _CategoryID
        End Get
        Set(ByVal value As Integer)
            _CategoryID = value
        End Set
    End Property

    Public Property Qty() As Integer
        Get
            Return _Qty
        End Get
        Set(ByVal value As Integer)
            _Qty = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property RentalPoolID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
