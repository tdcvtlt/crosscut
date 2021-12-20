Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient


Public Class clsComboItems
    Dim UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ComboID As Integer = 0
    Dim _Comboitem As String = ""
    Dim _Active As Boolean = False
    Dim _Description As String = ""
    Dim _LocationID As Integer = 0
    Dim _Err As String = ""

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As DataRow
    Dim ds As DataSet
    Dim dRead As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("", cn)
    End Sub

    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Comboitems where Comboitemid = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Comboitems")
            If ds.Tables("Comboitems").Rows.Count > 0 Then
                dr = ds.Tables("Comboitems").Rows(0)
                Set_Values()
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Lookup_ID(ByVal comboname As String, ByVal comboitem As String) As Integer
        Dim iRet As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Comboitemid from t_Comboitems i inner join t_Combos c on c.comboid=i.comboid where c.comboname = '" & comboname & "' and i.comboitem = '" & comboitem & "'"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                iRet = IIf(dRead(0) & "" <> "", dRead(0), 0)
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try

        Return iRet
    End Function
    Public Function Lookup_ComboItem(ByVal comboitemID As Integer) As String
        Dim iRet As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Comboitem from t_Comboitems where comboitemid = '" & comboitemID & "'"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                iRet = IIf(dRead(0) & "" <> "", dRead(0), 0)
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return iRet
    End Function

    Public Function Load_ComboItems(ByVal comboName As String) As SqlDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ComboItemID, ComboItem from t_ComboItems c inner join t_Combos b on c.CombOID = b.ComboID where b.CombOname = '" & comboName & "' and c.Active = '1' order by c.ComboItem asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select * from t_Comboitems where ComboitemID = " & _ID
            ds.ConnectionString = Resources.resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List_All() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select * from t_Comboitems where Active = 1"
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Sub Set_Values()
        _ID = dr("Comboitemid")
        _ComboID = IIf(Not (dr("ComboID") Is System.DBNull.Value), dr("ComboID"), 0)
        _Comboitem = IIf(Not (dr("Comboitem") Is System.DBNull.Value), dr("Comboitem"), "")
        _Active = IIf(Not (dr("Active") Is System.DBNull.Value), dr("Active"), False)
        _Description = IIf(Not (dr("Description") Is System.DBNull.Value), dr("Description"), "")
        _LocationID = IIf(Not (dr("LocationID") Is System.DBNull.Value), dr("LocationID"), 0)
    End Sub

    Public Sub Save()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Comboitems where ComboitemID = " & _ID
            da = New SqlDataAdapter(cm)

            Dim sqlcmdbuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Comboitems")

            If ds.Tables("Comboitems").Rows.Count > 0 Then
                dr = ds.Tables("Comboitems").Rows(0)
            Else
                dr = ds.Tables("Comboitems").NewRow
            End If

            dr("ComboItem") = _Comboitem
            dr("ComboID") = _ComboID
            dr("Active") = _Active
            dr("Description") = _Description
            dr("LocationID") = _LocationID

            If ds.Tables("Comboitems").Rows.Count < 1 Then ds.Tables("Comboitems").Rows.Add(dr)

            da.Update(ds, "Comboitems")

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        da = Nothing
        dr = Nothing
        ds = Nothing
        dRead = Nothing
        MyBase.Finalize()
    End Sub

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property ComboID() As Integer
        Get
            Return _ComboID
        End Get
        Set(ByVal value As Integer)
            _ComboID = value
        End Set
    End Property

    Public Property Comboitem As String
        Get
            Return _Comboitem
        End Get
        Set(ByVal value As String)
            _Comboitem = value
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
