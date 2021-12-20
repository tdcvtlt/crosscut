Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPremiumIssued
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PremiumID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _DateIssued As String = ""
    Dim _IssuedByID As Integer = 0
    Dim _CostEA As Decimal = 0
    Dim _TotalCost As Decimal = 0
    Dim _CBCostEA As Decimal = 0
    Dim _CertificateNumber As String = ""
    Dim _QtyAssigned As Integer = 0
    Dim _QtyIssued As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PremiumIssued where PremiumIssuedID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PremiumIssued where PremiumIssuedID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PremiumIssued")
            If ds.Tables("t_PremiumIssued").Rows.Count > 0 Then
                dr = ds.Tables("t_PremiumIssued").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PremiumID") Is System.DBNull.Value) Then _PremiumID = dr("PremiumID")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateIssued") Is System.DBNull.Value) Then _DateIssued = dr("DateIssued")
        If Not (dr("IssuedByID") Is System.DBNull.Value) Then _IssuedByID = dr("IssuedByID")
        If Not (dr("CostEA") Is System.DBNull.Value) Then _CostEA = dr("CostEA")
        If Not (dr("TotalCost") Is System.DBNull.Value) Then _TotalCost = dr("TotalCost")
        If Not (dr("CBCostEA") Is System.DBNull.Value) Then _CBCostEA = dr("CBCostEA")
        If Not (dr("CertificateNumber") Is System.DBNull.Value) Then _CertificateNumber = dr("CertificateNumber")
        If Not (dr("QtyAssigned") Is System.DBNull.Value) Then _QtyAssigned = dr("QtyAssigned")
        If Not (dr("QtyIssued") Is System.DBNull.Value) Then _QtyIssued = dr("QtyIssued")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PremiumIssued where PremiumIssuedID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PremiumIssued")
            If ds.Tables("t_PremiumIssued").Rows.Count > 0 Then
                dr = ds.Tables("t_PremiumIssued").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PremiumIssuedInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PremiumID", SqlDbType.int, 0, "PremiumID")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateIssued", SqlDbType.datetime, 0, "DateIssued")
                da.InsertCommand.Parameters.Add("@IssuedByID", SqlDbType.int, 0, "IssuedByID")
                da.InsertCommand.Parameters.Add("@CostEA", SqlDbType.money, 0, "CostEA")
                da.InsertCommand.Parameters.Add("@TotalCost", SqlDbType.money, 0, "TotalCost")
                da.InsertCommand.Parameters.Add("@CBCostEA", SqlDbType.money, 0, "CBCostEA")
                da.InsertCommand.Parameters.Add("@CertificateNumber", SqlDbType.varchar, 0, "CertificateNumber")
                da.InsertCommand.Parameters.Add("@QtyAssigned", SqlDbType.int, 0, "QtyAssigned")
                da.InsertCommand.Parameters.Add("@QtyIssued", SqlDbType.int, 0, "QtyIssued")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PremiumIssuedID", SqlDbType.Int, 0, "PremiumIssuedID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PremiumIssued").NewRow
            End If
            Update_Field("PremiumID", _PremiumID, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            If _DateIssued <> "" Then Update_Field("DateIssued", _DateIssued, dr)
            Update_Field("IssuedByID", _IssuedByID, dr)
            Update_Field("CostEA", _CostEA, dr)
            Update_Field("TotalCost", _TotalCost, dr)
            Update_Field("CBCostEA", _CBCostEA, dr)
            Update_Field("CertificateNumber", _CertificateNumber, dr)
            Update_Field("QtyAssigned", _QtyAssigned, dr)
            Update_Field("QtyIssued", _QtyIssued, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_PremiumIssued").Rows.Count < 1 Then ds.Tables("t_PremiumIssued").Rows.Add(dr)
            da.Update(ds, "t_PremiumIssued")
            _ID = ds.Tables("t_PremiumIssued").Rows(0).Item("PremiumIssuedID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Public Function Get_Assigned_Premiums(ByVal TourID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select pi.PremiumIssuedID, p.PremiumName from t_PremiumIssued pi left outer join t_Premium p on pi.PremiumID = p.PremiumID left outer join t_ComboItems ps on pi.StatusID = ps.ComboItemID where pi.KeyField = 'TourID' and pi.KeyValue = '" & TourID & "'"
        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Function Get_Prepared_Premiums(ByVal TourID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select pi.PremiumIssuedID, p.PremiumName from t_PremiumIssued pi left outer join t_Premium p on pi.PremiumID = p.PremiumID left outer join t_ComboItems ps on pi.StatusID = ps.ComboItemID where pi.KeyField = 'TourID' and pi.KeyValue = '" & TourID & "' and ps.ComboItem = 'Prepared'"
        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Function Get_Gifts(ByVal tourID As Integer) As String
        Dim gifts As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select pt.ComboItem as pType, pi.QtyAssigned, pi.TotalCost, p.PremiumName from t_PremiumIssued pi inner join t_Premium p on pi.PremiumID = p.PremiumID inner join t_ComboItems pt on p.TypeID= pt.ComboItemID inner join t_ComboItems ps on pi.StatusID = ps.ComboItemID where pi.KeyField = 'TourID' and pi.KeyValue = " & tourID & " and ps.ComboItem in ('Not Issued')"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read()
                    If dread("pType") = "Certificate" Or dread("pType") = "Cash" Or dread("pType") = "Dinner" Then
                        If gifts = "" Then
                            gifts = gifts & dread("QtyAssigned") & " " & dread("PremiumName") & " " & FormatCurrency(dread("TotalCost"), 2)
                        Else
                            gifts = gifts & ", " & dread("QtyAssigned") & " " & dread("PremiumName") & " " & FormatCurrency(dread("TotalCost"), 2)
                        End If
                    Else
                        If gifts = "" Then
                            gifts = gifts & dread("qtyAssigned") & " " & dread("PremiumName")
                        Else
                            gifts = gifts & ", " & dread("qtyAssigned") & " " & dread("PremiumName")
                        End If
                    End If
                Loop
            Else
                gifts = "N/A"
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return gifts
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
            oEvents.KeyField = _KeyField
            oEvents.KeyValue = _KeyValue
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

    Public Property PremiumID() As Integer
        Get
            Return _PremiumID
        End Get
        Set(ByVal value As Integer)
            _PremiumID = value
        End Set
    End Property

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

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property DateIssued() As String
        Get
            Return _DateIssued
        End Get
        Set(ByVal value As String)
            _DateIssued = value
        End Set
    End Property

    Public Property IssuedByID() As Integer
        Get
            Return _IssuedByID
        End Get
        Set(ByVal value As Integer)
            _IssuedByID = value
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

    Public Property TotalCost() As Decimal
        Get
            Return _TotalCost
        End Get
        Set(ByVal value As Decimal)
            _TotalCost = value
        End Set
    End Property

    Public Property CBCostEA() As Decimal
        Get
            Return _CBCostEA
        End Get
        Set(ByVal value As Decimal)
            _CBCostEA = value
        End Set
    End Property

    Public Property CertificateNumber() As String
        Get
            Return _CertificateNumber
        End Get
        Set(ByVal value As String)
            _CertificateNumber = value
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

    Public Property QtyIssued() As Integer
        Get
            Return _QtyIssued
        End Get
        Set(ByVal value As Integer)
            _QtyIssued = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property PremiumIssuedID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
End Class
