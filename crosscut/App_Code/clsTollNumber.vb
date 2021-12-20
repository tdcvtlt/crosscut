Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsTollNumbers
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TollNumber As String = ""
    Dim _Active As Boolean = False
    Dim _CarrierID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection("data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        cm = New SqlCommand("Select * from t_TollNumbers where TollID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_TollNumbers where TollID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_TollNumbers")
            If ds.Tables("t_TollNumbers").Rows.Count > 0 Then
                dr = ds.Tables("t_TollNumbers").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("TollNumber") Is System.DBNull.Value) Then _TollNumber = dr("TollNumber")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("CarrierID") Is System.DBNull.Value) Then _CarrierID = dr("CarrierID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_TollNumbers where TollID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_TollNumbers")
            If ds.Tables("t_TollNumbers").Rows.Count > 0 Then
                dr = ds.Tables("t_TollNumbers").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_TollNumbersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TollNumber", SqlDbType.varchar, 0, "TollNumber")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@CarrierID", SqlDbType.int, 0, "CarrierID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TollID", SqlDbType.Int, 0, "TollID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_TollNumbers").NewRow
            End If
            Update_Field("TollNumber", _TollNumber, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("CarrierID", _CarrierID, dr)
            If ds.Tables("t_TollNumbers").Rows.Count < 1 Then ds.Tables("t_TollNumbers").Rows.Add(dr)
            da.Update(ds, "t_TollNumbers")
            _ID = ds.Tables("t_TollNumbers").Rows(0).Item("TollID")
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
            oEvents.KeyField = "TollID"
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

    Public Property TollNumber() As String
        Get
            Return _TollNumber
        End Get
        Set(ByVal value As String)
            _TollNumber = value
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

    Public Property CarrierID() As Integer
        Get
            Return _CarrierID
        End Get
        Set(ByVal value As Integer)
            _CarrierID = value
        End Set
    End Property

    Public Property TollID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

