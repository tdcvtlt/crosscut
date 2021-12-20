Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackagePremium
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageTourID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PremiumID As Integer = 0
    Dim _QtyAssigned As Integer = 0
    Dim _CostEA As Decimal = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackagePremium where PackagePremiumID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackagePremium where PackagePremiumID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackagePremium")
            If ds.Tables("t_PackagePremium").Rows.Count > 0 Then
                dr = ds.Tables("t_PackagePremium").Rows(0)
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
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackagePremium where PackagePremiumID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackagePremium")
            If ds.Tables("t_PackagePremium").Rows.Count > 0 Then
                dr = ds.Tables("t_PackagePremium").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackagePremiumInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageTourID", SqlDbType.int, 0, "PackageTourID")
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PremiumID", SqlDbType.int, 0, "PremiumID")
                da.InsertCommand.Parameters.Add("@QtyAssigned", SqlDbType.int, 0, "QtyAssigned")
                da.InsertCommand.Parameters.Add("@CostEA", SqlDbType.money, 0, "CostEA")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackagePremiumID", SqlDbType.Int, 0, "PackagePremiumID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackagePremium").NewRow
            End If
            Update_Field("PackageTourID", _PackageTourID, dr)
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PremiumID", _PremiumID, dr)
            Update_Field("QtyAssigned", _QtyAssigned, dr)
            Update_Field("CostEA", _CostEA, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_PackagePremium").Rows.Count < 1 Then ds.Tables("t_PackagePremium").Rows.Add(dr)
            da.Update(ds, "t_PackagePremium")
            _ID = ds.Tables("t_PackagePremium").Rows(0).Item("PackagePremiumID")
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
            oEvents.KeyField = "PackagePremiumID"
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

    Public Property CostEA() As Decimal
        Get
            Return _CostEA
        End Get
        Set(ByVal value As Decimal)
            _CostEA = value
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

    Public Property PackagePremiumID() As Integer
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
