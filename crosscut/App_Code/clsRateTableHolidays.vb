Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRateTableHolidays
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _HolidayDate As String = ""
    Dim _HolidayRate As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RateTableHolidays where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RateTableHolidays where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RateTableHolidays")
            If ds.Tables("t_RateTableHolidays").Rows.Count > 0 Then
                dr = ds.Tables("t_RateTableHolidays").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("HolidayDate") Is System.DBNull.Value) Then _HolidayDate = dr("HolidayDate")
        If Not (dr("HolidayRate") Is System.DBNull.Value) Then _HolidayRate = dr("HolidayRate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RateTableHolidays where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RateTableHolidays")
            If ds.Tables("t_RateTableHolidays").Rows.Count > 0 Then
                dr = ds.Tables("t_RateTableHolidays").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RateTableHolidaysInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@HolidayDate", SqlDbType.date, 0, "HolidayDate")
                da.InsertCommand.Parameters.Add("@HolidayRate", SqlDbType.Money, 0, "HolidayRate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RateTableHolidays").NewRow
            End If
            Update_Field("HolidayDate", _HolidayDate, dr)
            Update_Field("HolidayRate", _HolidayRate, dr)
            If ds.Tables("t_RateTableHolidays").Rows.Count < 1 Then ds.Tables("t_RateTableHolidays").Rows.Add(dr)
            da.Update(ds, "t_RateTableHolidays")
            _ID = ds.Tables("t_RateTableHolidays").Rows(0).Item("ID")
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

    Public Function List_Holiday_Rates() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select * from t_RateTableHolidays where holidaydate >= '" & System.DateTime.Now.ToShortDateString & "'"
        Return ds
    End Function

    Public Function Date_Exists(ByVal sDate As Date) As Boolean
        Dim bFound As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RateTableHolidays where holidayDate = '" & sDate.ToShortDateString & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                bFound = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bFound
    End Function

    Public Function Check_Date(ByVal sDate As Date) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ID from t_RateTableHolidays where holidayDate = '" & sDate.ToShortDateString & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ID = dread("ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Function Delete_Record(ByVal ID As Integer) As Boolean
        Dim bDeleted As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_RateTableHolidays where ID = " & ID
            cm.ExecuteNonQuery()
            bDeleted = True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bDeleted
    End Function

    Public Property HolidayDate() As String
        Get
            Return _HolidayDate
        End Get
        Set(ByVal value As String)
            _HolidayDate = value
        End Set
    End Property

    Public Property HolidayRate() As Decimal
        Get
            Return _HolidayRate
        End Get
        Set(ByVal value As Decimal)
            _HolidayRate = value
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
End Class
