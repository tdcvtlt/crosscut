Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsGiftCard2Location
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _GiftCardID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_GiftCard2Location where GiftCard2LocID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_GiftCard2Location where GiftCard2LocID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_GiftCard2Location")
            If ds.Tables("t_GiftCard2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_GiftCard2Location").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("GiftCardID") Is System.DBNull.Value) Then _GiftCardID = dr("GiftCardID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_GiftCard2Location where GiftCard2LocID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_GiftCard2Location")
            If ds.Tables("t_GiftCard2Location").Rows.Count > 0 Then
                dr = ds.Tables("t_GiftCard2Location").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_GiftCard2LocationInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@GiftCardID", SqlDbType.int, 0, "GiftCardID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@GiftCard2LocID", SqlDbType.Int, 0, "GiftCard2LocID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_GiftCard2Location").NewRow
            End If
            Update_Field("GiftCardID", _GiftCardID, dr)
            Update_Field("LocationID", _LocationID, dr)
            If ds.Tables("t_GiftCard2Location").Rows.Count < 1 Then ds.Tables("t_GiftCard2Location").Rows.Add(dr)
            da.Update(ds, "t_GiftCard2Location")
            _ID = ds.Tables("t_GiftCard2Location").Rows(0).Item("GiftCard2LocID")

            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function Clear_Locations(ByVal ID As Integer) As Boolean
        Dim bRemove As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_GiftCard2Location where GiftCardID = " & ID
            cm.ExecuteNonQuery()
            bRemove = True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemove
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
            oEvents.KeyField = "GiftCard2LocID"
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

    Public Property GiftCardID() As Integer
        Get
            Return _GiftCardID
        End Get
        Set(ByVal value As Integer)
            _GiftCardID = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property GiftCard2LocID() As Integer
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
        Set(value As Integer)
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
