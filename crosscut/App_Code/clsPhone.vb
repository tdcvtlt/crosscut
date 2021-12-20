Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System


Public Class clsPhone
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _Number As String = ""
    Dim _Extension As String = ""
    Dim _TypeID As Integer = 0
    Dim _Active As Boolean = False
    Dim _UserID As Integer = 0
    Dim _Err As String = ""

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dRead As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("", cn)
    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing
        dRead = Nothing
        MyBase.Finalize()
    End Sub
    Public Function Get_Table() As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select p.PhoneID as ID,p.Active as Active, Number, Extension, TypeID, type.comboitem as Type from t_ProspectPhone p left outer join t_comboitems type on type.comboitemid= p.typeid where prospectid = " & _ProspectID, cn)
        Dim dr As SqlDataReader
        Dim row As DataRow
        Dim i As Integer
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                For i = 0 To dr.VisibleFieldCount - 1
                    dt.Columns.Add(dr.GetName(i))
                Next
                dt.Columns.Add("Dirty")
                row = dt.NewRow
                For i = 0 To dr.VisibleFieldCount - 1
                    row.Item(i) = dr.Item(i)
                Next
                row("Dirty") = False
                dt.Rows.Add(row)
                While dr.Read
                    row = dt.NewRow
                    For i = 0 To dr.VisibleFieldCount - 1
                        row.Item(i) = dr.Item(i)
                    Next
                    row("Dirty") = False
                    dt.Rows.Add(row)
                End While
            End If
            dr.Close()
            cn.Close()

        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try

        Return dt
    End Function

    Public Function Lookup_ProspectID(ByVal strNumber As String) As Integer
        Dim ret As Integer = 0
        cm.CommandText = "Select prospectid from t_ProspectPhone where number = '" & strNumber & "' order by active desc, prospectid desc"
        Try
            cn.Open()
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                ret = dRead("prospectid")
            End If
            dRead.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If Not (dRead.IsClosed) Then dRead.Close()
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Function Lookup_PhoneID(ByVal ProsID As Integer, ByVal strNumber As String) As Integer
        Dim ret As Integer = 0
        cm.CommandText = "Select phoneid from t_ProspectPhone where number = '" & strNumber & "' and prospectid = " & ProsID & " order by active desc, prospectid desc"
        Try
            cn.Open()
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                ret = dRead("phoneid")
            End If
            dRead.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If Not (dRead.IsClosed) Then dRead.Close()
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Function Lookup_By_Number() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.Number, p.PhoneID as ID, pr.ProspectID, ps.comboitem as Status, (select top 1 datecreated from t_Event where keyfield = 'Prospectid' and keyvalue = p.prospectid and fieldname = 'StatusID' and NewValue = 'Do Not Call' order by datecreated desc) as DateAdded, (select top 1 datecreated from t_Event where keyfield = 'Prospectid' and keyvalue = p.prospectid and fieldname = 'Number' and oldvalue = '' order by datecreated desc) as DateCreated from t_ProspectPhone p inner join t_Prospect pr on pr.prospectid = p.prospectid left outer join t_Comboitems ps on ps.comboitemid = pr.statusid where p.number = '" & _Number & "' " & _
                "union " & _
                "select '" & _Number & "' as Number, 0 as ID, 0 as ProspectID, 'FED Do Not Call' as Status, '' as DateAdded, '' as DateCreated from t_FedDNC2 where areacode = '" & Left(_Number, 3) & "' and number = '" & Right(_Number, Len(_Number) - 3) & "' " & _
                "union " & _
                "select '" & _Number & "' as Number, 0 as ID, 0 as ProspectID, 'State Do Not Call' as Status, case when dateadded is null then '' else dateadded end as DateAdded, case when dateadded is null then '' else dateadded end as DateCreated from t_StateDNC where number = '" & _Number & "'"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select p.PhoneID as ID, p.Number, p.Extension, t.comboitem as Type, p.Active from t_ProspectPhone p left outer join t_Comboitems t on t.comboitemid = p.typeid where p.prospectid = " & _ProspectID & " order by p.active desc"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Get_Primary_Phone(ByVal ProsID As Integer) As Integer
        Dim ret As Integer = 0
        cm.CommandText = "Select PhoneID from t_ProspectPhone where prospectid = '" & ProsID & "' order by active desc, phoneid desc"
        Try
            cn.Open()
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                ret = dRead("PhoneID")
            End If
            dRead.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If Not (IsNothing(dRead)) Then
                If Not (dRead.IsClosed) Then dRead.Close()
            End If
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Sub Save()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProspectPhone where PhoneID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlcmdbuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Phone")
            If ds.Tables("Phone").Rows.Count > 0 Then
                dr = ds.Tables("Phone").Rows(0)
            Else
                dr = ds.Tables("Phone").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("Number", _Number, dr)
            Update_Field("Extension", _Extension, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("Active", _Active, dr)

            If ds.Tables("Phone").Rows.Count < 1 Then ds.Tables("Phone").Rows.Add(dr)

            da.Update(ds, "Phone")

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProspectPhone where PhoneID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Phone")
            If ds.Tables("Phone").Rows.Count > 0 Then
                dr = ds.Tables("Phone").Rows(0)
                Set_Values()
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        _ID = dr("PhoneID")
        _ProspectID = IIf(Not (dr("ProspectID") Is System.DBNull.Value), dr("ProspectID"), 0)
        _Number = IIf(Not (dr("Number") Is System.DBNull.Value), dr("Number"), "")
        _Extension = IIf(Not (dr("Extension") Is System.DBNull.Value), dr("Extension"), 0)
        _TypeID = IIf(Not (dr("TypeID") Is System.DBNull.Value), dr("TypeID"), 0)
        _Active = IIf(Not (dr("Active") Is System.DBNull.Value), dr("Active"), False)
    End Sub

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ProspectID"
            oEvents.KeyValue = _ProspectID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Public Function Get_Phone_Number(ByVal prosID As Integer) As String
        Dim phone As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select top 1 Number from t_ProspectPhone where prospectID = '" & prosID & "' order by phoneid desc, Active desc"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                phone = dRead("Number")
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return phone
    End Function

    Public Function Verify_Phone_Type(ByVal PhoneID As Integer, ByVal prosID As Integer, ByVal typeID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Phones from t_ProspectPhone where PhoneID <> " & PhoneID & " and prospectID = " & prosID & " and TypeID = " & typeID & " and active = 1"
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("Phones") > 0 Then
                bValid = False
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Property PhoneID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property Number() As String
        Get
            Return _Number
        End Get
        Set(ByVal value As String)
            _Number = value
        End Set
    End Property

    Public Property Extension() As String
        Get
            Return _Extension
        End Get
        Set(ByVal value As String)
            _Extension = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property
End Class
