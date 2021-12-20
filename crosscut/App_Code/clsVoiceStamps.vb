Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsVoiceStamps
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _VSNumber As String = ""
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_VoiceStamps where VSID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_VoiceStamps where VSID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_VoiceStamps")
            If ds.Tables("t_VoiceStamps").Rows.Count > 0 Then
                dr = ds.Tables("t_VoiceStamps").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("VSNumber") Is System.DBNull.Value) Then _VSNumber = dr("VSNumber")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_VoiceStamps where VSID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_VoiceStamps")
            If ds.Tables("t_VoiceStamps").Rows.Count > 0 Then
                dr = ds.Tables("t_VoiceStamps").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_VoiceStampsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@VSNumber", SqlDbType.VarChar, 0, "VSNumber")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@VSID", SqlDbType.Int, 0, "VSID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_VoiceStamps").NewRow
            End If
            Update_Field("VSNumber", _VSNumber, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            If ds.Tables("t_VoiceStamps").Rows.Count < 1 Then ds.Tables("t_VoiceStamps").Rows.Add(dr)
            da.Update(ds, "t_VoiceStamps")
            _ID = ds.Tables("t_VoiceStamps").Rows(0).Item("VSID")
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
            oEvents.KeyField = "VSID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If _KeyValue > 0 Then
                ds.SelectCommand = "Select VSID, VSNumber from t_VoiceStamps where Keyfield='" & _KeyField & "' and Keyvalue = " & _KeyValue
            Else
                ds.SelectCommand = "Select VSID, VSNumber from t_VoiceStamps where 1=2"
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property VSNumber() As String
        Get
            Return _VSNumber
        End Get
        Set(ByVal value As String)
            _VSNumber = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
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

    Public Property VSID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
