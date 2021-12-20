Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Sql


Public Class clsFrequency
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Frequency As String = ""
    Dim _Interval As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Frequency where FrequencyID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Frequency where FrequencyID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Frequency")
            If ds.Tables("t_Frequency").Rows.Count > 0 Then
                dr = ds.Tables("t_Frequency").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Frequency") Is System.DBNull.Value) Then _Frequency = dr("Frequency")
        If Not (dr("Interval") Is System.DBNull.Value) Then _Interval = dr("Interval")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Frequency where FrequencyID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Frequency")
            If ds.Tables("t_Frequency").Rows.Count > 0 Then
                dr = ds.Tables("t_Frequency").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_FrequencyInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Frequency", SqlDbType.varchar, 0, "Frequency")
                da.InsertCommand.Parameters.Add("@Interval", SqlDbType.int, 0, "Interval")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.Int, 0, "FrequencyID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Frequency").NewRow
            End If
            Update_Field("Frequency", _Frequency, dr)
            Update_Field("Interval", _Interval, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Frequency").Rows.Count < 1 Then ds.Tables("t_Frequency").Rows.Add(dr)
            da.Update(ds, "t_Frequency")
            _ID = ds.Tables("t_Frequency").Rows(0).Item("FrequencyID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "FrequencyID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List_Frequencies() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select FrequencyID, Frequency from t_Frequency"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Frequency() As String
        Get
            Return _Frequency
        End Get
        Set(ByVal value As String)
            _Frequency = value
        End Set
    End Property

    Public Property Interval() As Integer
        Get
            Return _Interval
        End Get
        Set(ByVal value As Integer)
            _Interval = value
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

    Public Property FrequencyID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

End Class
