Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageReservation2CheckInDay
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageReservationID As Integer = 0
    Dim _CheckInDay As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageReservation2CheckInDay where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageReservation2CheckInDay where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservation2CheckInDay")
            If ds.Tables("t_PackageReservation2CheckInDay").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservation2CheckInDay").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageReservationID") Is System.DBNull.Value) Then _PackageReservationID = dr("PackageReservationID")
        If Not (dr("CheckInDay") Is System.DBNull.Value) Then _CheckInDay = dr("CheckInDay")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageReservation2CheckInDay where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservation2CheckInDay")
            If ds.Tables("t_PackageReservation2CheckInDay").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservation2CheckInDay").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageReservation2CheckInDayInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageReservationID", SqlDbType.int, 0, "PackageReservationID")
                da.InsertCommand.Parameters.Add("@CheckInDay", SqlDbType.int, 0, "CheckInDay")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageReservation2CheckInDay").NewRow
            End If
            Update_Field("PackageReservationID", _PackageReservationID, dr)
            Update_Field("CheckInDay", _CheckInDay, dr)
            If ds.Tables("t_PackageReservation2CheckInDay").Rows.Count < 1 Then ds.Tables("t_PackageReservation2CheckInDay").Rows.Add(dr)
            da.Update(ds, "t_PackageReservation2CheckInDay")
            _ID = ds.Tables("t_PackageReservation2CheckInDay").Rows(0).Item("ID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Public Function List_CheckIn_Days(ByVal ID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ID, Case when CheckInDay = 0 then 'Sunday' when CheckInDay = 1 then 'Monday' when CheckInDay = 2 then 'Tuesday' when CheckInDay = 3 then 'Wednesday' when CheckInday = 4 then 'Thursday' when CheckInDay = 5 then 'Friday' else 'Saturday' end as CheckInDay from t_PackageReservation2CheckInDay where PackageReservationID = " & ID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Delete_Record(ByVal ID As Integer) As Boolean
        Dim bDeleted As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_PackageReservation2CheckInDay where ID = " & ID
            cm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bDeleted
    End Function

    Public Function Validate_CheckInDay(ByVal pkgResID As Integer, ByVal chkInDay As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_packageReservation2CheckInday where PackageReservationID = " & pkgResID & " and CheckInDay = " & chkInDay
            dread = cm.ExecuteReader
            If dread.HasRows Then
                bValid = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
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

    Public Property PackageReservationID() As Integer
        Get
            Return _PackageReservationID
        End Get
        Set(ByVal value As Integer)
            _PackageReservationID = value
        End Set
    End Property

    Public Property CheckInDay() As Integer
        Get
            Return _CheckInDay
        End Get
        Set(ByVal value As Integer)
            _CheckInDay = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property
End Class
