Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageReservationFinTransCode
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PackageReservationID As Integer = 0
    Dim _FinTransID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _UseFormula As Boolean = False
    Dim _Formula As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageReservationFinTransCode where PackageReservationFinTransCodeID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageReservationFinTransCode where PackageReservationFinTransCodeID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservationFinTransCode")
            If ds.Tables("t_PackageReservationFinTransCode").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservationFinTransCode").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Sub Load(packageID As Int32, packageReservationID As Int32)
        Try
            cm.CommandText = String.Format("Select * from t_PackageReservationFinTransCode where PackageID={0} and PackageReservationID={1}", _
                                           packageID, packageReservationID)
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservationFinTransCode")
            If ds.Tables("t_PackageReservationFinTransCode").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservationFinTransCode").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(packageID As Int32, packageReservationID As Int32) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = String.Format("Select * from t_PackageReservationFinTransCode where PackageID={0} and PackageReservationID={1}", packageID, packageReservationID)
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try

        Return ds
    End Function
    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("PackageReservationID") Is System.DBNull.Value) Then _PackageReservationID = dr("PackageReservationID")
        If Not (dr("FinTransCodeID") Is System.DBNull.Value) Then _FinTransID = dr("FinTransCodeID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("UseFormula") Is System.DBNull.Value) Then _UseFormula = dr("UseFormula")
        If Not (dr("Formula") Is System.DBNull.Value) Then _Formula = dr("Formula")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageReservationFinTransCode where PackageReservationFinTransCodeID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservationFinTransCode")
            If ds.Tables("t_PackageReservationFinTransCode").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservationFinTransCode").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageReservationFinTransCodeInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PackageReservationID", SqlDbType.int, 0, "PackageReservationID")
                da.InsertCommand.Parameters.Add("@FinTransCodeID", SqlDbType.Int, 0, "FinTransCodeID")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@UseFormula", SqlDbType.bit, 0, "UseFormula")
                da.InsertCommand.Parameters.Add("@Formula", SqlDbType.varchar, 0, "Formula")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageReservationFinTransCodeID", SqlDbType.Int, 0, "PackageReservationFinTransCodeID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageReservationFinTransCode").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PackageReservationID", _PackageReservationID, dr)
            Update_Field("FinTransCodeID", _FinTransID, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("UseFormula", _UseFormula, dr)
            Update_Field("Formula", _Formula, dr)
            If ds.Tables("t_PackageReservationFinTransCode").Rows.Count < 1 Then ds.Tables("t_PackageReservationFinTransCode").Rows.Add(dr)
            da.Update(ds, "t_PackageReservationFinTransCode")
            _ID = ds.Tables("t_PackageReservationFinTransCode").Rows(0).Item("PackageReservationFinTransCodeID")
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
            oEvents.KeyField = "PackageReservationFinTransCodeID"
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

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property PackageReservationID() As Integer
        Get
            Return _PackageReservationID
        End Get
        Set(ByVal value As Integer)
            _PackageReservationID = value
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

    Public Property UseFormula() As Boolean
        Get
            Return _UseFormula
        End Get
        Set(ByVal value As Boolean)
            _UseFormula = value
        End Set
    End Property

    Public Property Formula() As String
        Get
            Return _Formula
        End Get
        Set(ByVal value As String)
            _Formula = value
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

    Public Property PackageReservationFinTransCodeID() As Integer
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
End Class
