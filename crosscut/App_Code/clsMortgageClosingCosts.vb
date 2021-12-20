Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsMortgageClosingCosts
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _MortgageID As Integer = 0
    Dim _FieldID As Integer = 0
    Dim _Amount As Decimal = 0
    Dim _DateCreated As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_MortgageClosingCosts where MortgageClosingCostID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_MortgageClosingCosts where MortgageClosingCostID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_MortgageClosingCosts")
            If ds.Tables("t_MortgageClosingCosts").Rows.Count > 0 Then
                dr = ds.Tables("t_MortgageClosingCosts").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(ByVal MortID As Integer) As SqlDataSource

        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "select mc.MortgageClosingCostID as ID, cc.FieldName, convert(varchar,mc.Amount) as Amount, cc.Optional from t_MortgageClosingCosts mc inner join t_ClosingCostDefaults cc on cc.fieldid = mc.fieldid where mortgageid = " & MortID & " order by cc.FieldName"
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("MortgageID") Is System.DBNull.Value) Then _MortgageID = dr("MortgageID")
        If Not (dr("FieldID") Is System.DBNull.Value) Then _FieldID = dr("FieldID")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
    End Sub

    Public Function Save() As Boolean
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_MortgageClosingCosts where MortgageClosingCostID = " & _ID
        da = New SqlDataAdapter(cm)
        Dim sqlCMBuild As New SqlCommandBuilder(da)
        ds = New DataSet
        da.Fill(ds, "t_MortgageClosingCosts")
        If ds.Tables("t_MortgageClosingCosts").Rows.Count > 0 Then
            dr = ds.Tables("t_MortgageClosingCosts").Rows(0)
        Else
            da.InsertCommand = New SqlCommand("dbo.sp_MortgageClosingCostsInsert", cn)
            da.InsertCommand.CommandType = CommandType.StoredProcedure
            da.InsertCommand.Parameters.Add("@MortgageID", SqlDbType.Int, 0, "MortgageID")
            da.InsertCommand.Parameters.Add("@FieldID", SqlDbType.Int, 0, "FieldID")
            da.InsertCommand.Parameters.Add("@Amount", SqlDbType.Money, 0, "Amount")
            da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
            Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@MortgageClosingCostID", SqlDbType.Int, 0, "MortgageClosingCostID")
            parameter.Direction = ParameterDirection.Output
            dr = ds.Tables("t_MortgageClosingCosts").NewRow
        End If
        Update_Field("MortgageID", _MortgageID, dr)
        Update_Field("FieldID", _FieldID, dr)
        Update_Field("Amount", _Amount, dr)
        Update_Field("DateCreated", _DateCreated, dr)
        If ds.Tables("t_MortgageClosingCosts").Rows.Count < 1 Then ds.Tables("t_MortgageClosingCosts").Rows.Add(dr)
        da.Update(ds, "t_MortgageClosingCosts")
        _ID = ds.Tables("t_MortgageClosingCosts").Rows(0).Item("MortgageClosingCostID")
        'Catch ex As Exception
        '_Err = ex.ToString
        'Return False
        '        Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return True
        'End Try
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
            oEvents.KeyField = "MortgageClosingCostID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Get_Total_CC(ByVal mortID As Integer) As Double
        Dim ccAmt As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(Amount) as Amt from t_MortgageClosingCosts where mortgageid = " & mortID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ccAmt = dread("Amt")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ccAmt
    End Function

    Public Function Get_Fee(ByVal fName As String, ByVal mortID As Integer) As Double
        Dim fee As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(Amount) as FeeAmount from t_MortgageClosingCosts a inner join t_ClosingCostDefaults b on a.fieldid = b.fieldid where fieldname = '" & fName & "' and mortgageid = '" & mortID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                fee = dread("FeeAmount")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return fee
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property MortgageID() As Integer
        Get
            Return _MortgageID
        End Get
        Set(ByVal value As Integer)
            _MortgageID = value
        End Set
    End Property

    Public Property FieldID() As Integer
        Get
            Return _FieldID
        End Get
        Set(ByVal value As Integer)
            _FieldID = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property MortgageClosingCostID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
