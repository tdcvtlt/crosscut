Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPremium2FinTrans
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PremiumID As Integer = 0
    Dim _FinTransID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Premium2FinTrans where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Premium2FinTrans where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Premium2FinTrans")
            If ds.Tables("t_Premium2FinTrans").Rows.Count > 0 Then
                dr = ds.Tables("t_Premium2FinTrans").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function GetFinTransID(PremiumID As Integer) As Integer
        Dim ret As Integer = 0
        Try
            cm.CommandText = "Select fintransid from t_Premium2FinTrans where PremiumID = " & PremiumID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Premium2FinTrans")
            If ds.Tables("t_Premium2FinTrans").Rows.Count > 0 Then
                ret = ds.Tables("t_Premium2FinTrans").Rows(0)("fintransid")
            End If

        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ret
    End Function

    Private Sub Set_Values()
        If Not (dr("ID") Is System.DBNull.Value) Then _ID = dr("ID")
        If Not (dr("PremiumID") Is System.DBNull.Value) Then _PremiumID = dr("PremiumID")
        If Not (dr("FinTransID") Is System.DBNull.Value) Then _FinTransID = dr("FinTransID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Premium2FinTrans where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Premium2FinTrans")
            If ds.Tables("t_Premium2FinTrans").Rows.Count > 0 Then
                dr = ds.Tables("t_Premium2FinTrans").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Premium2FinTransInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PremiumID", SqlDbType.Int, 0, "PremiumID")
                da.InsertCommand.Parameters.Add("@FinTransID", SqlDbType.Int, 0, "FinTransID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Premium2FinTrans").NewRow
            End If
            Update_Field("PremiumID", _PremiumID, dr)
            Update_Field("FinTransID", _FinTransID, dr)
            If ds.Tables("t_Premium2FinTrans").Rows.Count < 1 Then ds.Tables("t_Premium2FinTrans").Rows.Add(dr)
            da.Update(ds, "t_Premium2FinTrans")
            _ID = ds.Tables("t_Premium2FinTrans").Rows(0).Item("ID")
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
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PremiumID() As Integer
        Get
            Return _PremiumID
        End Get
        Set(ByVal value As Integer)
            _PremiumID = value
        End Set
    End Property

    Public Property FinTransID() As Integer
        Get
            Return _FinTransID
        End Get
        Set(ByVal value As Integer)
            _FinTransID = value
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
