Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageTourPremium
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageTourID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PremiumID As Integer = 0
    Dim _QtyAssigned As Integer = 0
    Dim _CostEA As Integer = 0
    Dim _Optional As Boolean = False    
    Dim _BundleID As Integer = 0
    Dim _PremiumStatusID As Int32 = 0

    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageTourPremium where PackageTourPremiumID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageTourPremium where PackageTourPremiumID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageTourPremium")
            If ds.Tables("t_PackageTourPremium").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTourPremium").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageTourID") Is System.DBNull.Value) Then _PackageTourID = dr("PackageTourID")
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("PremiumID") Is System.DBNull.Value) Then _PremiumID = dr("PremiumID")
        If Not (dr("QtyAssigned") Is System.DBNull.Value) Then _QtyAssigned = dr("QtyAssigned")
        If Not (dr("CostEA") Is System.DBNull.Value) Then _CostEA = dr("CostEA")
        If Not (dr("Optional") Is System.DBNull.Value) Then _Optional = dr("Optional")
        If Not (dr("BundleID") Is System.DBNull.Value) Then _BundleID = dr("BundleID")
        If Not (dr("PremiumStatusID") Is System.DBNull.Value) Then _PremiumStatusID = dr("PremiumStatusID")
    End Sub

    Public Function Delete() As Integer

        Dim r_affected = -1

        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = String.Format("delete from t_PackageTourPremium where PackageTourPremiumID = {0}", _ID)
            
            r_affected = cm.ExecuteNonQuery()
            Dim oEvents As New clsEvents
            With oEvents
                .EventType = "Delete"
                .FieldName = ""
                .OldValue = _ID
                .NewValue = ""
                .DateCreated = Date.Now
                .CreatedByID = _UserID
                .KeyField = "PackageTourPremiumID"
                .KeyValue = _ID
                oEvents.Create_Event()                
            End With         

        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message)
        Finally
            cn.Close()
        End Try

        Return r_affected
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageTourPremium where PackageTourPremiumID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageTourPremium")
            If ds.Tables("t_PackageTourPremium").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageTourPremium").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageTourPremiumInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageTourID", SqlDbType.int, 0, "PackageTourID")
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PremiumID", SqlDbType.int, 0, "PremiumID")
                da.InsertCommand.Parameters.Add("@QtyAssigned", SqlDbType.int, 0, "QtyAssigned")
                da.InsertCommand.Parameters.Add("@CostEA", SqlDbType.int, 0, "CostEA")
                da.InsertCommand.Parameters.Add("@Optional", SqlDbType.bit, 0, "Optional")
                da.InsertCommand.Parameters.Add("@BundleID", SqlDbType.Bit, 0, "BundleID")                
                da.InsertCommand.Parameters.Add("@PremiumStatusID", SqlDbType.Int, 4, "PremiumStatusID")

                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageTourPremiumID", SqlDbType.Int, 0, "PackageTourPremiumID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageTourPremium").NewRow
            End If
            Update_Field("PackageTourID", _PackageTourID, dr)
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PremiumID", _PremiumID, dr)
            Update_Field("QtyAssigned", _QtyAssigned, dr)
            Update_Field("CostEA", _CostEA, dr)
            Update_Field("Optional", _Optional, dr)
            Update_Field("BundleID", _BundleID, dr)            
            Update_Field("PremiumStatusID", _PremiumStatusID, dr)

            If ds.Tables("t_PackageTourPremium").Rows.Count < 1 Then ds.Tables("t_PackageTourPremium").Rows.Add(dr)
            da.Update(ds, "t_PackageTourPremium")
            _ID = ds.Tables("t_PackageTourPremium").Rows(0).Item("PackageTourPremiumID")
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
            oEvents.KeyField = "PackageTourPremiumID"
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

    Public Function Get_Package_Tour_Premiums(ByVal pkgTourID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select pt.PackageTourPremiumID as ID, pt.PremiumStatusID, p.PremiumName as Premium, pt.QtyAssigned as Qty, pt.CostEA, pb.Name as Bundle, pt.Optional from t_PackageTourPremium pt left outer join t_Premium p on pt.PremiumID = p.PremiumID left outer join t_PremiumBundles pb on pt.BundleID = pb.PremiumBundleID where pt.PackageTourID = " & pkgTourID
        Return ds
    End Function

    Public Function List_Package_Tour_Premiums(ByVal pkgTourID As Integer) As DataSet
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter("Select * from t_PackageTourPremium where packageTourID = " & pkgTourID, cn)
        da.Fill(ds, "Premiums")
        da.Dispose()
        Return ds
    End Function

    Public Property PackageTourID() As Integer
        Get
            Return _PackageTourID
        End Get
        Set(ByVal value As Integer)
            _PackageTourID = value
        End Set
    End Property

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
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

    Public Property QtyAssigned() As Integer
        Get
            Return _QtyAssigned
        End Get
        Set(ByVal value As Integer)
            _QtyAssigned = value
        End Set
    End Property

    Public Property CostEA() As Integer
        Get
            Return _CostEA
        End Get
        Set(ByVal value As Integer)
            _CostEA = value
        End Set
    End Property

    Public Property OptionalPrem() As Boolean
        Get
            Return _Optional
        End Get
        Set(ByVal value As Boolean)
            _Optional = value
        End Set
    End Property

    Public Property BundleID() As Integer
        Get
            Return _BundleID
        End Get
        Set(value As Integer)
            _BundleID = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

    Public Property PackageTourPremiumID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property

    Public Property PremiumStatusID As Int32
        Get
            Return _PremiumStatusID
        End Get
        Set(value As Int32)
            _PremiumStatusID = value
        End Set
    End Property
    
End Class
