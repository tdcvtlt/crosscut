Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsReservationLetter2Location
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ResLetterID As Integer = 0
    Dim _ResLocationID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ReservationLetter2Location where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ReservationLetter2Location where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ReservationLetter2Location")
            If ds.Tables("t_ReservationLetter2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationLetter2Location").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ResLetterID") Is System.DBNull.Value) Then _ResLetterID = dr("ResLetterID")
        If Not (dr("ResLocationID") Is System.DBNull.Value) Then _ResLocationID = dr("ResLocationID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ReservationLetter2Location where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ReservationLetter2Location")
            If ds.Tables("t_ReservationLetter2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationLetter2Location").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReservationLetter2LocationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ResLetterID", SqlDbType.int, 0, "ResLetterID")
                da.InsertCommand.Parameters.Add("@ResLocationID", SqlDbType.int, 0, "ResLocationID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ReservationLetter2Location").NewRow
            End If
            Update_Field("ResLetterID", _ResLetterID, dr)
            Update_Field("ResLocationID", _ResLocationID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_ReservationLetter2Location").Rows.Count < 1 Then ds.Tables("t_ReservationLetter2Location").Rows.Add(dr)
            da.Update(ds, "t_ReservationLetter2Location")
            _ID = ds.Tables("t_ReservationLetter2Location").Rows(0).Item("ID")
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

    Public Function List_Locations(ByVal letterID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select a.ID, b.ComboItem as Location, a.Active from t_ReservationLetter2Location a inner join t_ComboItems b on a.ResLocationID = b.ComboItemID where a.ResLetterID = " & letterID & " order by b.ComboItem asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property ResLetterID() As Integer
        Get
            Return _ResLetterID
        End Get
        Set(ByVal value As Integer)
            _ResLetterID = value
        End Set
    End Property

    Public Property ResLocationID() As Integer
        Get
            Return _ResLocationID
        End Get
        Set(ByVal value As Integer)
            _ResLocationID = value
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
