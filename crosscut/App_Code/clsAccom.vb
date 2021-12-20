Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccom
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AccomName As String = ""
    Dim _AccomLocationID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Description As String = ""
    Dim _OnlineDescription As String = ""
    Dim _URL As String = ""
    Dim _Address As String = ""
    Dim _City As String = ""
    Dim _PostalCode As String = ""
    Dim _StateID As Integer = 0
    Dim _Directions As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Accom where AccomID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Accom where AccomID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Accom")
            If ds.Tables("t_Accom").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AccomName") Is System.DBNull.Value) Then _AccomName = dr("AccomName")
        If Not (dr("AccomLocationID") Is System.DBNull.Value) Then _AccomLocationID = dr("AccomLocationID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("OnlineDescription") Is System.DBNull.Value) Then _OnlineDescription = dr("OnlineDescription")
        If Not (dr("URL") Is System.DBNull.Value) Then _URL = dr("URL")
        If Not (dr("Address") Is System.DBNull.Value) Then _Address = dr("Address")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("PostalCode") Is System.DBNull.Value) Then _PostalCode = dr("PostalCode")
        If Not (dr("Directions") Is System.DBNull.Value) Then _Directions = dr("Directions")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Accom where AccomID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Accom")
            If ds.Tables("t_Accom").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_AccomInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AccomName", SqlDbType.varchar, 0, "AccomName")
                da.InsertCommand.Parameters.Add("@AccomLocationID", SqlDbType.int, 0, "AccomLocationID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@OnlineDescription", SqlDbType.varchar, 0, "OnlineDescription")
                da.InsertCommand.Parameters.Add("@URL", SqlDbType.varchar, 0, "URL")
                da.InsertCommand.Parameters.Add("@Address", SqlDbType.VarChar, 0, "Address")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.Int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@PostalCode", SqlDbType.VarChar, 0, "PostalCode")
                da.InsertCommand.Parameters.Add("@Directions", SqlDbType.Text, 0, "Directions")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.Int, 0, "AccomID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Accom").NewRow
            End If
            Update_Field("AccomName", _AccomName, dr)
            Update_Field("AccomLocationID", _AccomLocationID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("OnlineDescription", _OnlineDescription, dr)
            Update_Field("URL", _URL, dr)
            Update_Field("Address", _Address, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("Directions", _Directions, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_Accom").Rows.Count < 1 Then ds.Tables("t_Accom").Rows.Add(dr)
            da.Update(ds, "t_Accom")
            _ID = ds.Tables("t_Accom").Rows(0).Item("AccomID")
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
            oEvents.KeyField = "AccomID"
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

    Public Function Search_Accoms(ByVal accomName As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Dim sSQL As String = ""
        ds.ConnectionString = Resources.Resource.cns
        If accomName = "" Then
            sSQL = "Select Top 100 a.AccomID as ID, a.AccomName, al.ComboItem as Location from t_Accom a left outer join t_Comboitems al on a.AccomLocationID = al.ComboItemID order by AccomName asc"
        Else
            sSQL = "Select Top 100 a.AccomID as ID, a.AccomName, al.ComboItem as Location from t_Accom a left outer join t_Comboitems al on a.AccomLocationID = al.ComboItemID where a.accomname like '" & accomName & "%' order by accomname asc"
        End If
        ds.SelectCommand = sSQL
        Return ds
    End Function

    Public Function Get_Accommodations() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select AccomID, AccomName from t_Accom"
        Return ds
    End Function

    Public Function Accoms_By_Location(ByVal loc As Integer, ByVal accomID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select AccomID, AccomName from t_Accom where AccomID = " & accomID & " or (AccomLocationID = " & loc & " and Active = 1) order by accomname asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property AccomName() As String
        Get
            Return _AccomName
        End Get
        Set(ByVal value As String)
            _AccomName = value
        End Set
    End Property

    Public Property AccomLocationID() As Integer
        Get
            Return _AccomLocationID
        End Get
        Set(ByVal value As Integer)
            _AccomLocationID = value
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property OnlineDescription() As String
        Get
            Return _OnlineDescription
        End Get
        Set(ByVal value As String)
            _OnlineDescription = value
        End Set
    End Property

    Public Property URL() As String
        Get
            Return _URL
        End Get
        Set(ByVal value As String)
            _URL = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(ByVal value As String)
            _Address = value
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

    Public Property PostalCode() As String
        Get
            Return _PostalCode
        End Get
        Set(ByVal value As String)
            _PostalCode = value
        End Set
    End Property

    Public Property Directions() As String
        Get
            Return _Directions
        End Get
        Set(ByVal value As String)
            _Directions = value
        End Set
    End Property

    Public Property AccomID() As Integer
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(value As Boolean)
            _Active = value
        End Set
    End Property
End Class
