Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsGiftCard
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Number As Integer = 0
    Dim _Balance As Decimal = 0
    Dim _Active As Boolean = False
    Dim _CreatedByID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_GiftCard where GiftCardID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            If _ID > 0 Then
                cm.CommandText = "Select * from t_GiftCard where GiftCardID = " & _ID
            ElseIf _Number > 0 Then
                cm.CommandText = "Select * from t_GiftCard where Number = " & _Number
            Else
                cm.CommandText = "Select * from t_GiftCard where Number = " & _Number & " or GiftCardID = " & _ID
            End If
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_GiftCard")
            If ds.Tables("t_GiftCard").Rows.Count > 0 Then
                dr = ds.Tables("t_GiftCard").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("GiftCardID") Is System.DBNull.Value) Then _ID = dr("GiftCardID")
        If Not (dr("Number") Is System.DBNull.Value) Then _Number = dr("Number")
        If Not (dr("Balance") Is System.DBNull.Value) Then _Balance = dr("Balance")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_GiftCard where GiftCardID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_GiftCard")
            If ds.Tables("t_GiftCard").Rows.Count > 0 Then
                dr = ds.Tables("t_GiftCard").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_GiftCardInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Number", SqlDbType.int, 0, "Number")
                da.InsertCommand.Parameters.Add("@Balance", SqlDbType.money, 0, "Balance")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@GiftCardID", SqlDbType.Int, 0, "GiftCardID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_GiftCard").NewRow
            End If
            Update_Field("Number", _Number, dr)
            Update_Field("Balance", _Balance, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            If ds.Tables("t_GiftCard").Rows.Count < 1 Then ds.Tables("t_GiftCard").Rows.Add(dr)
            da.Update(ds, "t_GiftCard")
            _ID = ds.Tables("t_GiftCard").Rows(0).Item("GiftCardID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "GiftCardID"
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

    Public Property Number() As Integer
        Get
            Return _Number
        End Get
        Set(ByVal value As Integer)
            _Number = value
        End Set
    End Property

    Public Property Balance() As Decimal
        Get
            Return _Balance
        End Get
        Set(ByVal value As Decimal)
            _Balance = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
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

    Public Property GiftCardID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
