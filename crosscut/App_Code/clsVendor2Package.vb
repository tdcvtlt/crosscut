Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsVendor2Package
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _VendorID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Display As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Vendor2Package where Vendor2PackageID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Vendor2Package where Vendor2PackageID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Vendor2Package")
            If ds.Tables("t_Vendor2Package").Rows.Count > 0 Then
                dr = ds.Tables("t_Vendor2Package").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("Display") Is System.DBNull.Value) Then _Display = dr("Display")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Vendor2Package where Vendor2PackageID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Vendor2Package")
            If ds.Tables("t_Vendor2Package").Rows.Count > 0 Then
                dr = ds.Tables("t_Vendor2Package").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Vendor2PackageInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.int, 0, "VendorID")
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@Display", SqlDbType.Bit, 0, "Display")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Vendor2PackageID", SqlDbType.Int, 0, "Vendor2PackageID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Vendor2Package").NewRow
            End If
            Update_Field("VendorID", _VendorID, dr)
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("Display", _Display, dr)
            If ds.Tables("t_Vendor2Package").Rows.Count < 1 Then ds.Tables("t_Vendor2Package").Rows.Add(dr)
            da.Update(ds, "t_Vendor2Package")
            _ID = ds.Tables("t_Vendor2Package").Rows(0).Item("Vendor2PackageID")
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
            oEvents.KeyField = "Vendor2PackageID"
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

    Public Function List_Vendors_By_Package(ByVal pkgID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.Vendor2PackageID as ID, v.Vendor, p.Display from t_Vendor2Package p inner join t_Vendor v on p.VendorID = v.VendorID where p.PackageID = " & pkgID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property VendorID() As Integer
        Get
            Return _VendorID
        End Get
        Set(ByVal value As Integer)
            _VendorID = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property Vendor2PackageID() As Integer
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

    Public Property Display() As Boolean
        Get
            Return _Display
        End Get
        Set(value As Boolean)
            _Display = value
        End Set
    End Property
End Class
