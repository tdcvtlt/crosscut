Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Public Class clsUnit
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _Address1 As String = ""
    Dim _Address2 As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _Zip As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _StyleID As Integer = 0
    Dim _BudgetedPrice As Decimal = 0
    Dim _StatusID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _err As String = ""

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
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing
        dRead = Nothing
        MyBase.Finalize()
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try

        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Function List_For_Tie(ByVal unitID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.COnnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select UnitID, Name as Unit from t_Unit where unitid <> " & unitID
        Catch ex As Exception
            _err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Unit where UnitID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Unit")
            If ds.Tables("t_Unit").Rows.Count > 0 Then
                dr = ds.Tables("t_Unit").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_UnitInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.VarChar, 0, "Name")
                da.InsertCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 0, "Address1")
                da.InsertCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 0, "Address2")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.Int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@Zip", SqlDbType.VarChar, 0, "Zip")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@StyleID", SqlDbType.Int, 0, "StyleID")
                da.InsertCommand.Parameters.Add("@BudgetedPrice", SqlDbType.Money, 0, "BudgetedPrice")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@UnitID", SqlDbType.Int, 0, "UnitID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Unit").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("Address1", _Address1, dr)
            Update_Field("Address2", _Address2, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("Zip", _Zip, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("StyleID", _StyleID, dr)
            Update_Field("BudgetedPrice", _BudgetedPrice, dr)
            Update_Field("StatusID", _StatusID, dr)

            If ds.Tables("t_Unit").Rows.Count < 1 Then ds.Tables("t_Unit").Rows.Add(dr)
            da.Update(ds, "t_Unit")
            _ID = ds.Tables("t_Unit").Rows(0).Item("UnitID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try

    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Unit where UnitID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Unit")
            If ds.Tables("t_Unit").Rows.Count > 0 Then
                dr = ds.Tables("t_Unit").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("Address1") Is System.DBNull.Value) Then _Address1 = dr("Address1")
        If Not (dr("Address2") Is System.DBNull.Value) Then _Address2 = dr("Address2")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("Zip") Is System.DBNull.Value) Then _Zip = dr("Zip")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("StyleID") Is System.DBNull.Value) Then _StyleID = dr("StyleID")
        If Not (dr("BudgetedPrice") Is System.DBNull.Value) Then _BudgetedPrice = dr("BudgetedPrice")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
    End Sub

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            'oEvents.CreatedByID = _UserID
            oEvents.KeyField = "UnitID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If

    End Sub

    Public Function List_Units() As SQLDataSource
        Dim ds As New SqlDataSource
        Try
            ds.COnnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select UnitID, Name from t_Unit order by name asc"
        Catch ex As Exception
            _err = ex.Message
        End Try
        Return ds
    End Function

    Public Property UnitID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Public Property StateID() As Integer
        Get
            Return _StateID
        End Get
        Set(ByVal value As Integer)
            _StateID = value
        End Set
    End Property

    Public Property Zip() As String
        Get
            Return _Zip
        End Get
        Set(ByVal value As String)
            _Zip = value
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

    Public Property SubTypeID() As Integer
        Get
            Return _SubTypeID
        End Get
        Set(ByVal value As Integer)
            _SubTypeID = value
        End Set
    End Property

    Public Property StyleID As Integer
        Get
            Return _StyleID
        End Get
        Set(ByVal value As Integer)
            _StyleID = value
        End Set
    End Property

    Public Property BudgetedPrice As Decimal
        Get
            Return _BudgetedPrice
        End Get
        Set(ByVal value As Decimal)
            _BudgetedPrice = value
        End Set
    End Property

    Public Property StatusID As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

End Class
