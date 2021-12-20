Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccomCheckInLocations
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CheckInLocationID As Integer = 0
    Dim _Directions As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_AccomCheckInLocations where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_AccomCheckInLocations where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_AccomCheckInLocations")
            If ds.Tables("t_AccomCheckInLocations").Rows.Count > 0 Then
                dr = ds.Tables("t_AccomCheckInLocations").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("CheckInLocationID") Is System.DBNull.Value) Then _CheckInLocationID = dr("CheckInLocationID")
        If Not (dr("Directions") Is System.DBNull.Value) Then _Directions = dr("Directions")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_AccomCheckInLocations where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_AccomCheckInLocations")
            If ds.Tables("t_AccomCheckInLocations").Rows.Count > 0 Then
                dr = ds.Tables("t_AccomCheckInLocations").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_AccomCheckInLocationsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CheckInLocationID", SqlDbType.int, 0, "CheckInLocationID")
                da.InsertCommand.Parameters.Add("@Directions", SqlDbType.text, 0, "Directions")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_AccomCheckInLocations").NewRow
            End If
            Update_Field("CheckInLocationID", _CheckInLocationID, dr)
            Update_Field("Directions", _Directions, dr)
            If ds.Tables("t_AccomCheckInLocations").Rows.Count < 1 Then ds.Tables("t_AccomCheckInLocations").Rows.Add(dr)
            da.Update(ds, "t_AccomCheckInLocations")
            _ID = ds.Tables("t_AccomCheckInLocations").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
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

    Public Function List(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "" Then
                ds.SelectCommand = "Select a.ID, b.ComboItem as Location from t_AccomCheckInLocations a inner join t_ComboItems b on a.CheckInLocationID = b.ComboItemID order by b.ComboItem asc"
            Else
                ds.SelectCommand = "Select a.ID, b.ComboItem as Location from t_AccomCheckInLocations a inner join t_ComboItems b on a.CheckInLocationID = b.ComboItemID where b.ComboItem = " & filter & " order by b.ComboItem asc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Lookup_By_ID(ByVal ID As Integer) As Integer
        Dim locID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ID from t_AccomCheckInLocations where CheckInLocationID = " & ID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                locID = dread("ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return locID
    End Function

    Public Property CheckInLocationID() As Integer
        Get
            Return _CheckInLocationID
        End Get
        Set(ByVal value As Integer)
            _CheckInLocationID = value
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

    Public Property ID() As Integer
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
