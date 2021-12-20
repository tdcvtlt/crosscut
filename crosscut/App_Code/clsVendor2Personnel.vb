Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsVendor2Personnel
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _VendorID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _Admin As Boolean = False
    Dim _Manager As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Vendor2Personnel where Vendor2PersonnelID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Vendor2Personnel where Vendor2PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Vendor2Personnel")
            If ds.Tables("t_Vendor2Personnel").Rows.Count > 0 Then
                dr = ds.Tables("t_Vendor2Personnel").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("Admin") Is System.DBNull.Value) Then _Admin = dr("Admin")
        If Not (dr("Manager") Is System.DBNull.Value) Then _Manager = dr("Manager")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Vendor2Personnel where Vendor2PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Vendor2Personnel")
            If ds.Tables("t_Vendor2Personnel").Rows.Count > 0 Then
                dr = ds.Tables("t_Vendor2Personnel").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Vendor2PersonnelInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.int, 0, "VendorID")
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@Admin", SqlDbType.bit, 0, "Admin")
                da.InsertCommand.Parameters.Add("@Manager", SqlDbType.bit, 0, "Manager")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Vendor2PersonnelID", SqlDbType.Int, 0, "Vendor2PersonnelID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Vendor2Personnel").NewRow
            End If
            Update_Field("VendorID", _VendorID, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("Admin", _Admin, dr)
            Update_Field("Manager", _Manager, dr)
            If ds.Tables("t_Vendor2Personnel").Rows.Count < 1 Then ds.Tables("t_Vendor2Personnel").Rows.Add(dr)
            da.Update(ds, "t_Vendor2Personnel")
            _ID = ds.Tables("t_Vendor2Personnel").Rows(0).Item("Vendor2PersonnelID")
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
            oEvents.KeyField = "Vendor2PersonnelID"
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

    Public Function List_Vendors(ByVal persID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select vp.Vendor2PersonnelID, v.Vendor, vp.Admin, vp.Manager from t_Vendor2Personnel vp inner join t_Vendor v on vp.VendorID = v.VendorID where vp.PersonnelID = " & persID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Vendors(ByVal persID As Integer) As String
        Dim vendors As String = "-1"
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select distinct(vendorid) from t_vendor2Personnel where personnelid = " & persID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If vendors = "-1" Then
                        vendors = dread("VendorID")
                    Else
                        vendors = vendors & "," & dread("VendorID")
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return vendors
    End Function

    Public Function List_Vendors_Add(ByVal ID As Integer, ByVal persID As Integer) As sqldatasource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If ID = 0 Then
                ds.SelectCommand = "Select Distinct(v.VendorID), v.Vendor from t_Vendor v left outer join t_Vendor2Personnel vp on vp.VendorID = v.VendorID where v.VendorID not in (Select vendorid from t_Vendor2Personnel where personnelid = " & persID & ") and v.Active = 1 order by v.Vendor asc"
            Else
                ds.SelectCommand = "Select VendorID, Vendor from t_Vendor where active = 1 order by vendor asc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Remove_Vendor(ByVal ID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_Vendor2Personnel where vendor2PersonnelID = " & ID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bRemoved = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function

    Public Function Get_Members(ByVal vendor As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select p.PersonnelID, p.UserName from t_Personnel p inner join t_Vendor2Personnel pd on p.PersonnelID = pd.PersonnelID inner join t_Vendor v on pd.VendorID = v.VendorID where v.Vendor = '" & vendor & "' and p.Active = '1' order by p.UserName asc"
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

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property Admin() As Boolean
        Get
            Return _Admin
        End Get
        Set(ByVal value As Boolean)
            _Admin = value
        End Set
    End Property

    Public Property Manager() As Boolean
        Get
            Return _Manager
        End Get
        Set(ByVal value As Boolean)
            _Manager = value
        End Set
    End Property

    Public Property Vendor2PersonnelID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
