Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Public Class clsTaxCodes
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CodeID As Integer = 0
    Dim _Percentage As Decimal = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_TaxCodes where TaxID = " & _ID, cn)
    End Sub
    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_TaxCodes where TaxID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_TaxCodes")
            If ds.Tables("t_TaxCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_TaxCodes").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub
    Private Sub Set_Values()
        If Not (dr("CodeID") Is System.DBNull.Value) Then _CodeID = dr("CodeID")
        If Not (dr("Percentage") Is System.DBNull.Value) Then _Percentage = dr("Percentage")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub
    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_TaxCodes where TaxID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_TaxCodes")
            If ds.Tables("t_TaxCodes").Rows.Count > 0 Then
                dr = ds.Tables("t_TaxCodes").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_TaxCodesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CodeID", SqlDbType.int, 0, "CodeID")
                da.InsertCommand.Parameters.Add("@Percentage", SqlDbType.money, 0, "Percentage")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TaxID", SqlDbType.Int, 0, "TaxID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_TaxCodes").NewRow
            End If
            Update_Field("CodeID", _CodeID, dr)
            Update_Field("Percentage", _Percentage, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_TaxCodes").Rows.Count < 1 Then ds.Tables("t_TaxCodes").Rows.Add(dr)
            da.Update(ds, "t_TaxCodes")
            _ID = ds.Tables("t_TaxCodes").Rows(0).Item("TaxID")
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
            oEvents.KeyField = "TaxID"
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
    Public Property CodeID() As Integer
        Get
            Return _CodeID
        End Get
        Set(ByVal value As Integer)
            _CodeID = value
        End Set
    End Property
    Public Property Percentage() As Decimal
        Get
            Return _Percentage
        End Get
        Set(ByVal value As Decimal)
            _Percentage = value
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
    Public ReadOnly Property TaxID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
