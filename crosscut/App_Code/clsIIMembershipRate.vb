Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsIIMembershipRate
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _II_Rate As Decimal
    Dim _II_Membership As Decimal
    Dim _II_Payback As Decimal
    Dim _Frequency As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_IIMembershipRate where IIMemberRateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_IIMembershipRate where IIMemberRateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_IIMembershipRate")
            If ds.Tables("t_IIMembershipRate").Rows.Count > 0 Then
                dr = ds.Tables("t_IIMembershipRate").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("II_Rate") Is System.DBNull.Value) Then _II_Rate = dr("II_Rate")
        If Not (dr("II_Membership") Is System.DBNull.Value) Then _II_Membership = dr("II_Membership")
        If Not (dr("II_Payback") Is System.DBNull.Value) Then _II_Payback = dr("II_Payback")
        If Not (dr("Frequency") Is System.DBNull.Value) Then _Frequency = dr("Frequency")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_IIMembershipRate where IIMemberRateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_IIMembershipRate")
            If ds.Tables("t_IIMembershipRate").Rows.Count > 0 Then
                dr = ds.Tables("t_IIMembershipRate").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_IIMembershipRateInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@II_Rate", SqlDbType.smallmoney, 0, "II_Rate")
                da.InsertCommand.Parameters.Add("@II_Membership", SqlDbType.smallmoney, 0, "II_Membership")
                da.InsertCommand.Parameters.Add("@II_Payback", SqlDbType.smallmoney, 0, "II_Payback")
                da.InsertCommand.Parameters.Add("@Frequency", SqlDbType.int, 0, "Frequency")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@IIMemberRateID", SqlDbType.Int, 0, "IIMemberRateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_IIMembershipRate").NewRow
            End If
            Update_Field("II_Rate", _II_Rate, dr)
            Update_Field("II_Membership", _II_Membership, dr)
            Update_Field("II_Payback", _II_Payback, dr)
            Update_Field("Frequency", _Frequency, dr)
            If ds.Tables("t_IIMembershipRate").Rows.Count < 1 Then ds.Tables("t_IIMembershipRate").Rows.Add(dr)
            da.Update(ds, "t_IIMembershipRate")
            _ID = ds.Tables("t_IIMembershipRate").Rows(0).Item("IIMemberRateID")
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
            oEvents.KeyField = "IIMemberRateID"
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

    Public Property II_Rate() As Decimal
        Get
            Return _II_Rate
        End Get
        Set(ByVal value As Decimal)
            _II_Rate = value
        End Set
    End Property

    Public Property II_Membership() As Decimal
        Get
            Return _II_Membership
        End Get
        Set(ByVal value As Decimal)
            _II_Membership = value
        End Set
    End Property

    Public Property II_Payback() As Decimal
        Get
            Return _II_Payback
        End Get
        Set(ByVal value As Decimal)
            _II_Payback = value
        End Set
    End Property

    Public Property Frequency() As Integer
        Get
            Return _Frequency
        End Get
        Set(ByVal value As Integer)
            _Frequency = value
        End Set
    End Property

    Public Property IIMemberRateID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
