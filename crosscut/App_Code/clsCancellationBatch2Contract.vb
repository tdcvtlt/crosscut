Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCancellationBatch2Contract
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _BatchID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _DateAdded As String = ""
    Dim _AddedByID As Integer = 0
    Dim _DateRemoved As String = ""
    Dim _RemovedByID As Integer = 0
    Dim _NextStep As Integer = 0
    Dim _NextStepDate As String = ""
    Dim _PreviousStep As Integer = 0
    Dim _PreviousStepDate As String = ""
    Dim _CurrentBalance As Decimal = 0
    Dim _LenderCode As String = ""
    Dim _DaysPastDueInitial As Integer = 0
    Dim _PaymentsMade As Integer = 0
    Dim _LastPayment As String = ""
    Dim _InterestPaidThruDate As String = ""
    Dim _FirstNotice As String = ""
    Dim _SecondNotice As String = ""
    Dim _ContractStatus As Integer = 0
    Dim _ContractSubStatus As Integer = 0
    Dim _MortgageStatus As Integer = 0
    Dim _MortgageSubStatus As Integer = 0
    Dim _ReasonRemoved As Integer = 0
    Dim _MaintenanceFeeStatus As Integer = 0
    Dim _PayOffAmount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CancellationBatch2Contract where Batch2ContractID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CancellationBatch2Contract where Batch2ContractID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CancellationBatch2Contract")
            If ds.Tables("t_CancellationBatch2Contract").Rows.Count > 0 Then
                dr = ds.Tables("t_CancellationBatch2Contract").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("BatchID") Is System.DBNull.Value) Then _BatchID = dr("BatchID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("AddedByID") Is System.DBNull.Value) Then _AddedByID = dr("AddedByID")
        If Not (dr("DateRemoved") Is System.DBNull.Value) Then _DateRemoved = dr("DateRemoved")
        If Not (dr("RemovedByID") Is System.DBNull.Value) Then _RemovedByID = dr("RemovedByID")
        If Not (dr("NextStep") Is System.DBNull.Value) Then _NextStep = dr("NextStep")
        If Not (dr("NextStepDate") Is System.DBNull.Value) Then _NextStepDate = dr("NextStepDate")
        If Not (dr("PreviousStep") Is System.DBNull.Value) Then _PreviousStep = dr("PreviousStep")
        If Not (dr("PreviousStepDate") Is System.DBNull.Value) Then _PreviousStepDate = dr("PreviousStepDate")
        If Not (dr("CurrentBalance") Is System.DBNull.Value) Then _CurrentBalance = dr("CurrentBalance")
        If Not (dr("LenderCode") Is System.DBNull.Value) Then _LenderCode = dr("LenderCode")
        If Not (dr("DaysPastDueInitial") Is System.DBNull.Value) Then _DaysPastDueInitial = dr("DaysPastDueInitial")
        If Not (dr("PaymentsMade") Is System.DBNull.Value) Then _PaymentsMade = dr("PaymentsMade")
        If Not (dr("LastPayment") Is System.DBNull.Value) Then _LastPayment = dr("LastPayment")
        If Not (dr("InterestPaidThruDate") Is System.DBNull.Value) Then _InterestPaidThruDate = dr("InterestPaidThruDate")
        If Not (dr("FirstNotice") Is System.DBNull.Value) Then _FirstNotice = dr("FirstNotice")
        If Not (dr("SecondNotice") Is System.DBNull.Value) Then _SecondNotice = dr("SecondNotice")
        If Not (dr("ContractStatus") Is System.DBNull.Value) Then _ContractStatus = dr("ContractStatus")
        If Not (dr("ContractSubStatus") Is System.DBNull.Value) Then _ContractSubStatus = dr("ContractSubStatus")
        If Not (dr("MortgageStatus") Is System.DBNull.Value) Then _MortgageStatus = dr("MortgageStatus")
        If Not (dr("MortgageSubStatus") Is System.DBNull.Value) Then _MortgageSubStatus = dr("MortgageSubStatus")
        If Not (dr("ReasonRemoved") Is System.DBNull.Value) Then _ReasonRemoved = dr("ReasonRemoved")
        If Not (dr("MaintenanceFeeStatus") Is System.DBNull.Value) Then _MaintenanceFeeStatus = dr("MaintenanceFeeStatus")
        If Not (dr("PayOffAmount") Is System.DBNull.Value) Then _PayOffAmount = dr("PayOffAmount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CancellationBatch2Contract where Batch2ContractID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CancellationBatch2Contract")
            If ds.Tables("t_CancellationBatch2Contract").Rows.Count > 0 Then
                dr = ds.Tables("t_CancellationBatch2Contract").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CancellationBatch2ContractInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@BatchID", SqlDbType.Int, 0, "BatchID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.Int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.DateTime, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@AddedByID", SqlDbType.Int, 0, "AddedByID")
                da.InsertCommand.Parameters.Add("@DateRemoved", SqlDbType.DateTime, 0, "DateRemoved")
                da.InsertCommand.Parameters.Add("@RemovedByID", SqlDbType.Int, 0, "RemovedByID")
                da.InsertCommand.Parameters.Add("@NextStep", SqlDbType.Int, 0, "NextStep")
                da.InsertCommand.Parameters.Add("@NextStepDate", SqlDbType.DateTime, 0, "NextStepDate")
                da.InsertCommand.Parameters.Add("@PreviousStep", SqlDbType.Int, 0, "PreviousStep")
                da.InsertCommand.Parameters.Add("@PreviousStepDate", SqlDbType.DateTime, 0, "PreviousStepDate")
                da.InsertCommand.Parameters.Add("@CurrentBalance", SqlDbType.Money, 0, "CurrentBalance")
                da.InsertCommand.Parameters.Add("@LenderCode", SqlDbType.VarChar, 0, "LenderCode")
                da.InsertCommand.Parameters.Add("@DaysPastDueInitial", SqlDbType.Int, 0, "DaysPastDueInitial")
                da.InsertCommand.Parameters.Add("@PaymentsMade", SqlDbType.Int, 0, "PaymentsMade")
                da.InsertCommand.Parameters.Add("@LastPayment", SqlDbType.DateTime, 0, "LastPayment")
                da.InsertCommand.Parameters.Add("@InterestPaidThruDate", SqlDbType.DateTime, 0, "InterestPaidThruDate")
                da.InsertCommand.Parameters.Add("@FirstNotice", SqlDbType.DateTime, 0, "FirstNotice")
                da.InsertCommand.Parameters.Add("@SecondNotice", SqlDbType.DateTime, 0, "SecondNotice")
                da.InsertCommand.Parameters.Add("@ContractStatus", SqlDbType.Int, 0, "ContractStatus")
                da.InsertCommand.Parameters.Add("@ContractSubStatus", SqlDbType.Int, 0, "ContractSubStatus")
                da.InsertCommand.Parameters.Add("@MortgageStatus", SqlDbType.Int, 0, "MortgageStatus")
                da.InsertCommand.Parameters.Add("@MortgageSubStatus", SqlDbType.Int, 0, "MortgageSubStatus")
                da.InsertCommand.Parameters.Add("@ReasonRemoved", SqlDbType.Int, 0, "ReasonRemoved")
                da.InsertCommand.Parameters.Add("@MaintenanceFeeStatus", SqlDbType.Int, 0, "MaintenanceFeeStatus")
                da.InsertCommand.Parameters.Add("@PayOffAmount", SqlDbType.Money, 0, "PayOffAmount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Batch2ContractID", SqlDbType.Int, 0, "Batch2ContractID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CancellationBatch2Contract").NewRow
            End If
            Update_Field("BatchID", _BatchID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("AddedByID", _AddedByID, dr)
            Update_Field("DateRemoved", _DateRemoved, dr)
            Update_Field("RemovedByID", _RemovedByID, dr)
            Update_Field("NextStep", _NextStep, dr)
            Update_Field("NextStepDate", _NextStepDate, dr)
            Update_Field("PreviousStep", _PreviousStep, dr)
            Update_Field("PreviousStepDate", _PreviousStepDate, dr)
            Update_Field("CurrentBalance", _CurrentBalance, dr)
            Update_Field("LenderCode", _LenderCode, dr)
            Update_Field("DaysPastDueInitial", _DaysPastDueInitial, dr)
            Update_Field("PaymentsMade", _PaymentsMade, dr)
            Update_Field("LastPayment", _LastPayment, dr)
            Update_Field("InterestPaidThruDate", _InterestPaidThruDate, dr)
            Update_Field("FirstNotice", _FirstNotice, dr)
            Update_Field("SecondNotice", _SecondNotice, dr)
            Update_Field("ContractStatus", _ContractStatus, dr)
            Update_Field("ContractSubStatus", _ContractSubStatus, dr)
            Update_Field("MortgageStatus", _MortgageStatus, dr)
            Update_Field("MortgageSubStatus", _MortgageSubStatus, dr)
            Update_Field("ReasonRemoved", _ReasonRemoved, dr)
            Update_Field("MaintenanceFeeStatus", _MaintenanceFeeStatus, dr)
            Update_Field("PayOffAmount", _PayOffAmount, dr)
            If ds.Tables("t_CancellationBatch2Contract").Rows.Count < 1 Then ds.Tables("t_CancellationBatch2Contract").Rows.Add(dr)
            da.Update(ds, "t_CancellationBatch2Contract")
            _ID = ds.Tables("t_CancellationBatch2Contract").Rows(0).Item("Batch2ContractID")
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
            oEvents.KeyField = "Batch2ContractID"
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

    Public Property PayOffAmount As Decimal
        Get
            Return _PayOffAmount
        End Get
        Set(value As Decimal)
            _PayOffAmount = value
        End Set
    End Property

    Public Property BatchID() As Integer
        Get
            Return _BatchID
        End Get
        Set(ByVal value As Integer)
            _BatchID = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property DateAdded() As String
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property AddedByID() As Integer
        Get
            Return _AddedByID
        End Get
        Set(ByVal value As Integer)
            _AddedByID = value
        End Set
    End Property

    Public Property DateRemoved() As String
        Get
            Return _DateRemoved
        End Get
        Set(ByVal value As String)
            _DateRemoved = value
        End Set
    End Property

    Public Property RemovedByID() As Integer
        Get
            Return _RemovedByID
        End Get
        Set(ByVal value As Integer)
            _RemovedByID = value
        End Set
    End Property

    Public Property NextStep() As Integer
        Get
            Return _NextStep
        End Get
        Set(ByVal value As Integer)
            _NextStep = value
        End Set
    End Property

    Public Property NextStepDate() As String
        Get
            Return _NextStepDate
        End Get
        Set(ByVal value As String)
            _NextStepDate = value
        End Set
    End Property

    Public Property PreviousStep() As Integer
        Get
            Return _PreviousStep
        End Get
        Set(ByVal value As Integer)
            _PreviousStep = value
        End Set
    End Property

    Public Property PreviousStepDate() As String
        Get
            Return _PreviousStepDate
        End Get
        Set(ByVal value As String)
            _PreviousStepDate = value
        End Set
    End Property

    Public Property CurrentBalance() As Decimal
        Get
            Return _CurrentBalance
        End Get
        Set(ByVal value As Decimal)
            _CurrentBalance = value
        End Set
    End Property

    Public Property LenderCode() As String
        Get
            Return _LenderCode
        End Get
        Set(ByVal value As String)
            _LenderCode = value
        End Set
    End Property

    Public Property DaysPastDueInitial() As Integer
        Get
            Return _DaysPastDueInitial
        End Get
        Set(ByVal value As Integer)
            _DaysPastDueInitial = value
        End Set
    End Property

    Public Property PaymentsMade() As Integer
        Get
            Return _PaymentsMade
        End Get
        Set(ByVal value As Integer)
            _PaymentsMade = value
        End Set
    End Property

    Public Property LastPayment() As String
        Get
            Return _LastPayment
        End Get
        Set(ByVal value As String)
            _LastPayment = value
        End Set
    End Property

    Public Property InterestPaidThruDate() As String
        Get
            Return _InterestPaidThruDate
        End Get
        Set(ByVal value As String)
            _InterestPaidThruDate = value
        End Set
    End Property

    Public Property FirstNotice() As String
        Get
            Return _FirstNotice
        End Get
        Set(ByVal value As String)
            _FirstNotice = value
        End Set
    End Property

    Public Property SecondNotice() As String
        Get
            Return _SecondNotice
        End Get
        Set(ByVal value As String)
            _SecondNotice = value
        End Set
    End Property

    Public Property ContractStatus() As Integer
        Get
            Return _ContractStatus
        End Get
        Set(ByVal value As Integer)
            _ContractStatus = value
        End Set
    End Property

    Public Property ContractSubStatus() As Integer
        Get
            Return _ContractSubStatus
        End Get
        Set(ByVal value As Integer)
            _ContractSubStatus = value
        End Set
    End Property

    Public Property MortgageStatus() As Integer
        Get
            Return _MortgageStatus
        End Get
        Set(ByVal value As Integer)
            _MortgageStatus = value
        End Set
    End Property

    Public Property MortgageSubStatus() As Integer
        Get
            Return _MortgageSubStatus
        End Get
        Set(ByVal value As Integer)
            _MortgageSubStatus = value
        End Set
    End Property

    Public Property ReasonRemoved() As Integer
        Get
            Return _ReasonRemoved
        End Get
        Set(ByVal value As Integer)
            _ReasonRemoved = value
        End Set
    End Property

    Public Property Batch2ContractID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
    Public Property MaintenanceFeeStatus As Integer
        Get
            Return _MaintenanceFeeStatus
        End Get
        Set(value As Integer)
            _MaintenanceFeeStatus = value
        End Set
    End Property
End Class
