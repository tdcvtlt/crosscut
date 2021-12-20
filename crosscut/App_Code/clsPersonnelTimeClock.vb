Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnelTimeClock
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _IPAddress As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelTimeClock where TimeClockID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelTimeClock where TimeClockID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelTimeClock")
            If ds.Tables("t_PersonnelTimeClock").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelTimeClock").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("IPAddress") Is System.DBNull.Value) Then _IPAddress = dr("IPAddress")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelTimeClock where TimeClockID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelTimeClock")
            If ds.Tables("t_PersonnelTimeClock").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelTimeClock").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelTimeClockInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@IPAddress", SqlDbType.varchar, 0, "IPAddress")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TimeClockID", SqlDbType.Int, 0, "TimeClockID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelTimeClock").NewRow
            End If
            Update_Field("IPAddress", _IPAddress, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PersonnelTimeClock").Rows.Count < 1 Then ds.Tables("t_PersonnelTimeClock").Rows.Add(dr)
            da.Update(ds, "t_PersonnelTimeClock")
            _ID = ds.Tables("t_PersonnelTimeClock").Rows(0).Item("TimeClockID")
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
            oEvents.KeyField = "TimeClockID"
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

    Public Function Get_Clock_ID(ByVal ipaddr As String) As Integer
        Dim clockID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select TimeClockID from t_PersonnelTimeClock where IPAddress = '" & ipaddr & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                clockID = dread("TimeClockID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return clockID
    End Function

    Public Function Validate_Clock(ByVal ipaddr As String) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as TC from t_PersonnelTimeClock where IPAddress = '" & ipaddr & "' and active = '1'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("TC") = 0 Then
                    bValid = False
                End If
            Else
                bValid = False
            End If
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Property IPAddress() As String
        Get
            Return _IPAddress
        End Get
        Set(ByVal value As String)
            _IPAddress = value
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

    Public Property TimeClockID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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
