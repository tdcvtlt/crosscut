Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccom2CheckInLocation
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AccomID As Integer = 0
    Dim _CheckInLocationID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Accom2CheckInLocation where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Accom2CheckInLocation where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Accom2CheckInLocation")
            If ds.Tables("t_Accom2CheckInLocation").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom2CheckInLocation").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AccomID") Is System.DBNull.Value) Then _AccomID = dr("AccomID")
        If Not (dr("CheckInLocationID") Is System.DBNull.Value) Then _CheckInLocationID = dr("CheckInLocationID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Accom2CheckInLocation where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Accom2CheckInLocation")
            If ds.Tables("t_Accom2CheckInLocation").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom2CheckInLocation").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Accom2CheckInLocationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.int, 0, "AccomID")
                da.InsertCommand.Parameters.Add("@CheckInLocationID", SqlDbType.int, 0, "CheckInLocationID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Accom2CheckInLocation").NewRow
            End If
            Update_Field("AccomID", _AccomID, dr)
            Update_Field("CheckInLocationID", _CheckInLocationID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_Accom2CheckInLocation").Rows.Count < 1 Then ds.Tables("t_Accom2CheckInLocation").Rows.Add(dr)
            da.Update(ds, "t_Accom2CheckInLocation")
            _ID = ds.Tables("t_Accom2CheckInLocation").Rows(0).Item("ID")
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

    Public Function Get_CheckIn_Locations(ByVal accomID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select a.ID, l.ComboItem as Location, a.Active from t_Accom2CheckInLocation a inner join t_CombOitems l on a.CheckInLocationID = l.ComboItemID where a.AccomID = " & accomID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function CheckIn_Locations_By_Accom(ByVal accomID As Integer, ByVal locID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(a.CheckInlocationID) as ID, l.ComboItem as Location from t_Accom2CheckInLocation a inner join t_CombOitems l on a.CheckInLocationID = l.ComboItemID where (a.AccomID = " & accomID & " and a.Active = 1) or l.ComboItemID = " & locID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property AccomID() As Integer
        Get
            Return _AccomID
        End Get
        Set(ByVal value As Integer)
            _AccomID = value
        End Set
    End Property

    Public Property CheckInLocationID() As Integer
        Get
            Return _CheckInLocationID
        End Get
        Set(ByVal value As Integer)
            _CheckInLocationID = value
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
