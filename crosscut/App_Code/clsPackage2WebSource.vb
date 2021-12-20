Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackage2WebSource
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _WebSourceID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Package2WebSource where Package2WebSourceID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Package2WebSource where Package2WebSourceID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Package2WebSource")
            If ds.Tables("t_Package2WebSource").Rows.Count > 0 Then
                dr = ds.Tables("t_Package2WebSource").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("WebSourceID") Is System.DBNull.Value) Then _WebSourceID = dr("WebSourceID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Package2WebSource where Package2WebSourceID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Package2WebSource")
            If ds.Tables("t_Package2WebSource").Rows.Count > 0 Then
                dr = ds.Tables("t_Package2WebSource").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Package2WebSourceInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@WebSourceID", SqlDbType.int, 0, "WebSourceID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Package2WebSourceID", SqlDbType.Int, 0, "Package2WebSourceID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Package2WebSource").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("WebSourceID", _WebSourceID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_Package2WebSource").Rows.Count < 1 Then ds.Tables("t_Package2WebSource").Rows.Add(dr)
            da.Update(ds, "t_Package2WebSource")
            _ID = ds.Tables("t_Package2WebSource").Rows(0).Item("Package2WebSourceID")
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
            oEvents.KeyField = "Package2WebSourceID"
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

    Public Function Get_Pkg_WebSources(ByVal pkgID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select p.Package2WebSourceID AS ID, c.ComboItem as WebSource, p.Active from t_package2WebSource p inner join t_comboItems c on p.WebSourceID = c.ComboItemID where p.PackageID = " & pkgID
        Return ds
    End Function

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property WebSourceID() As Integer
        Get
            Return _WebSourceID
        End Get
        Set(ByVal value As Integer)
            _WebSourceID = value
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

    Public Property Package2WebSourceID() As Integer
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
