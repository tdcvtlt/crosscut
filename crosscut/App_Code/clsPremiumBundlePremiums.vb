Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPremiumBundlePremiums
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PremiumBundleID As Integer = 0
    Dim _PremiumID As Integer = 0
    Dim _Qty As Integer = 0
    Dim _CostEA As Decimal = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PremiumBundlePremiums where Premium2BundleID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PremiumBundlePremiums where Premium2BundleID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PremiumBundlePremiums")
            If ds.Tables("t_PremiumBundlePremiums").Rows.Count > 0 Then
                dr = ds.Tables("t_PremiumBundlePremiums").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PremiumBundleID") Is System.DBNull.Value) Then _PremiumBundleID = dr("PremiumBundleID")
        If Not (dr("PremiumID") Is System.DBNull.Value) Then _PremiumID = dr("PremiumID")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
        If Not (dr("CostEA") Is System.DBNull.Value) Then _CostEA = dr("CostEA")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PremiumBundlePremiums where Premium2BundleID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PremiumBundlePremiums")
            If ds.Tables("t_PremiumBundlePremiums").Rows.Count > 0 Then
                dr = ds.Tables("t_PremiumBundlePremiums").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PremiumBundlePremiumsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PremiumBundleID", SqlDbType.int, 0, "PremiumBundleID")
                da.InsertCommand.Parameters.Add("@PremiumID", SqlDbType.int, 0, "PremiumID")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.int, 0, "Qty")
                da.InsertCommand.Parameters.Add("@CostEA", SqlDbType.Money, 0, "CostEA")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Premium2BundleID", SqlDbType.Int, 0, "Premium2BundleID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PremiumBundlePremiums").NewRow
            End If
            Update_Field("PremiumBundleID", _PremiumBundleID, dr)
            Update_Field("PremiumID", _PremiumID, dr)
            Update_Field("Qty", _Qty, dr)
            Update_Field("CostEA", _CostEA, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PremiumBundlePremiums").Rows.Count < 1 Then ds.Tables("t_PremiumBundlePremiums").Rows.Add(dr)
            da.Update(ds, "t_PremiumBundlePremiums")
            _ID = ds.Tables("t_PremiumBundlePremiums").Rows(0).Item("Premium2BundleID")
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
            oEvents.KeyField = "Premium2BundleID"
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

    Public Function get_Bundle_Premiums(ByVal bundleID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select pb.Premium2BundleID as ID, p.PremiumName, pb.Qty, pb.CostEA, pb.Active from t_PremiumBundlePremiums pb inner join t_Premium p on pb.PremiumID = p.PremiumID where pb.PremiumBundleID = " & bundleID
        Return ds
    End Function

    Public Function list_Premiums(ByVal premID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select PremiumID, PremiumName from t_Premium where active = 1 or premiumid in (Select premiumid from t_PremiumBundlePremiums where Premium2BundleID = " & premID & ")"
        Return ds
    End Function

    Public Function get_Active_Premiums(ByVal bundleID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("PremiumID")
        dt.Columns.Add("Qty")
        dt.Columns.Add("Cost")
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_PremiumBundlePremiums where PremiumBundleID = " & bundleID
        Dim dread As SqlDataReader
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Dim dRow As DataRow
            Do While dread.Read
                dRow = dt.NewRow()
                dRow("PremiumID") = dread("PremiumID")
                dRow("Qty") = dread("Qty")
                dRow("Cost") = dread("CostEA")
                dt.Rows.Add(dRow)
            Loop
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return dt
    End Function
    Public Property PremiumBundleID() As Integer
        Get
            Return _PremiumBundleID
        End Get
        Set(ByVal value As Integer)
            _PremiumBundleID = value
        End Set
    End Property

    Public Property PremiumID() As Integer
        Get
            Return _PremiumID
        End Get
        Set(ByVal value As Integer)
            _PremiumID = value
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

    Public Property CostEA() As Decimal
        Get
            Return _CostEA
        End Get
        Set(ByVal value As Decimal)
            _CostEA = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(value As Boolean)
            _Active = value
        End Set
    End Property
    Public Property Premium2BundleID() As Integer
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
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

End Class
