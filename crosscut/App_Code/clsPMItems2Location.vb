Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPMItem2Location
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PMLocationID As Integer = 0
    Dim _PMItemID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PMItem2Location where PMItem2PMLocationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PMItem2Location where PMItem2PMLocationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PMItem2Location")
            If ds.Tables("t_PMItem2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_PMItem2Location").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PMLocationID") Is System.DBNull.Value) Then _PMLocationID = dr("PMLocationID")
        If Not (dr("PMItemID") Is System.DBNull.Value) Then _PMItemID = dr("PMItemID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PMItem2Location where PMItem2PMLocationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PMItem2Location")
            If ds.Tables("t_PMItem2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_PMItem2Location").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PMItem2LocationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PMLocationID", SqlDbType.int, 0, "PMLocationID")
                da.InsertCommand.Parameters.Add("@PMItemID", SqlDbType.int, 0, "PMItemID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PMItem2PMLocationID", SqlDbType.Int, 0, "PMItem2PMLocationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PMItem2Location").NewRow
            End If
            Update_Field("PMLocationID", _PMLocationID, dr)
            Update_Field("PMItemID", _PMItemID, dr)
            If ds.Tables("t_PMItem2Location").Rows.Count < 1 Then ds.Tables("t_PMItem2Location").Rows.Add(dr)
            da.Update(ds, "t_PMItem2Location")
            _ID = ds.Tables("t_PMItem2Location").Rows(0).Item("PMItem2PMLocationID")
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
            oEvents.KeyField = "PMItem2PMLocationID"
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

    Public Property PMLocationID() As Integer
        Get
            Return _PMLocationID
        End Get
        Set(ByVal value As Integer)
            _PMLocationID = value
        End Set
    End Property

    Public Property PMItemID() As Integer
        Get
            Return _PMItemID
        End Get
        Set(ByVal value As Integer)
            _PMItemID = value
        End Set
    End Property

    Public ReadOnly Property PMItem2PMLocationID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class

