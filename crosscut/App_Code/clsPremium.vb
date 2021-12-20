Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System


Public Class clsPremium
    Dim _ID As Integer = 0
    Dim _PremiumName As String = ""
    Dim _Description As String = ""
    Dim _Cost As Decimal = 0
    Dim _CBCost As Decimal = 0
    Dim _QtyOnHand As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _UserID As Integer = 0
    Dim _Active As Boolean = False

    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0

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

    Public Function List_Active() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = " select PremiumID, PremiumName from t_Premium where Active = '1' order by PremiumName asc"
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = " select * from t_Premium where premiumid = " & _ID
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Sub Save()
        Try
            Dim bUpdatePending As Boolean = False
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Premium where PremiumID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlcmdbuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Premium")
            If ds.Tables("Premium").Rows.Count > 0 Then
                dr = ds.Tables("Premium").Rows(0)
            Else
                dr = ds.Tables("Premium").NewRow
            End If

            Update_Field("Description", _Description, dr)
            If _ID > 0 Then
                If _Cost <> dr("Cost") Or _CBCost <> dr("CBCost") Then
                    bUpdatePending = True
                End If
            End If
            Update_Field("Cost", _Cost, dr)
            Update_Field("CBCost", _CBCost, dr)
            Update_Field("QtyOnHand", _QtyOnHand, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("PremiumName", _PremiumName, dr)
            If ds.Tables("Premium").Rows.Count < 1 Then ds.Tables("Premium").Rows.Add(dr)

            da.Update(ds, "Premium")

            If bUpdatePending Then
                cm.CommandText = "Update t_PremiumIssued set CostEa=" & _Cost & ", TotalCost = " & _Cost & " * QtyAssigned, CBCostEa=" & _CBCost & " where premiumid=" & _ID & " and statusid= (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where i.comboitem = 'Not Issued' and c.ComboName='PremiumStatus')"
                cm.ExecuteNonQuery()
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()

        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List_Issued() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If _KeyField <> "" Then
            ds.SelectCommand = "Select i.PremiumIssuedID as ID, p.PremiumName as " & IIf(_KeyField.ToLower() = "reservationid", "Gift", "Premium") & ", i.QtyAssigned, i.QtyIssued, i.CostEa, s.comboitem as Status from t_PremiumIssued i inner join t_Premium p on p.premiumid = i.premiumid left outer join t_Comboitems s on s.comboitemid = i.statusid where keyfield='" & _KeyField & "' and keyvalue = " & _KeyValue
        Else
            _Err = "KeyField Not Set"
        End If

        Return ds
    End Function
    Public Function List_TourWizard_Premiums(ByVal tourID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ID")
        dt.Columns.Add("Premium")
        dt.Columns.Add("PremiumID")
        dt.Columns.Add("Certificate")
        dt.Columns.Add("QtyAssigned")
        dt.Columns.Add("CostEA")
        dt.Columns.Add("TotalCost")
        dt.Columns.Add("StatusID")
        dt.Columns.Add("Status")
        dt.Columns.Add("Dirty")
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select i.PremiumIssuedID, p.PremiumID, i.CertificateNumber, p.PremiumName as Premiumname, i.QtyAssigned, i.CostEA, i.QtyAssigned * i.CostEA as TotalCost, i.StatusID, s.ComboItem as Status from t_PremiumIssued i inner join t_Premium p on i.premiumid = p.premiumid left outer join t_Comboitems s on s.comboitemid = i.statusid where keyfield = 'TourID' and keyvalue = '" & tourID & "'"
            dRead = cm.ExecuteReader
            While dRead.Read
                dr = dt.NewRow
                dr("ID") = dRead.Item("PremiumIssuedID")
                dr("Premium") = dRead.Item("PremiumName")
                dr("PremiumID") = dRead.Item("PremiumID")
                dr("Certificate") = dRead.Item("CertificateNumber")
                dr("QtyAssigned") = dRead.Item("QtyAssigned")
                dr("CostEA") = dRead.Item("CostEA")
                dr("TotalCost") = dRead.Item("TotalCost")
                dr("StatusID") = dRead("StatusID")
                dr("Status") = dRead.Item("Status")
                dr("Dirty") = False
                dt.Rows.Add(dr)
            End While
            dRead = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return dt
    End Function
    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Premium where PremiumID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Premium")
            If ds.Tables("Premium").Rows.Count > 0 Then
                dr = ds.Tables("Premium").Rows(0)
                Set_Values()
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        _ID = dr("PremiumID")
        _PremiumName = IIf(Not (dr("PremiumName") Is System.DBNull.Value), dr("PremiumName"), "")
        _Description = IIf(Not (dr("Description") Is System.DBNull.Value), dr("Description"), "")
        _Cost = IIf(Not (dr("Cost") Is System.DBNull.Value), dr("Cost"), 0)
        _CBCost = IIf(Not (dr("CBCost") Is System.DBNull.Value), dr("CBCost"), 0)
        _QtyOnHand = IIf(Not (dr("QtyOnHand") Is System.DBNull.Value), dr("QtyOnHand"), 0)
        _TypeID = IIf(Not (dr("TypeID") Is System.DBNull.Value), dr("TypeID"), 0)
        _LocationID = IIf(Not (dr("LocationID") Is System.DBNull.Value), dr("LocationID"), 0)
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
            oEvents.KeyField = "PremiumID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property PremiumID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property PremiumName() As String
        Get
            Return _PremiumName
        End Get
        Set(ByVal value As String)
            _PremiumName = value
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

    Public Property Cost() As Decimal
        Get
            Return _Cost
        End Get
        Set(ByVal value As Decimal)
            _Cost = value
        End Set
    End Property

    Public Property CBCost() As Decimal
        Get
            Return _CBCost
        End Get
        Set(ByVal value As Decimal)
            _CBCost = value
        End Set
    End Property

    Public Property QtyOnHand() As Decimal
        Get
            Return _QtyOnHand
        End Get
        Set(ByVal value As Decimal)
            _QtyOnHand = value
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
    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
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

