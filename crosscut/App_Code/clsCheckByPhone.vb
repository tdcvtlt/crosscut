Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCheckByPhone
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DateEntered As String = ""
    Dim _EnteredByID As Integer = 0
    Dim _AccountLastName As String = ""
    Dim _AccountMiddleInit As String = ""
    Dim _AccountFirstName As String = ""
    Dim _ContractNumber As String = ""
    Dim _RoutingNumber As String = ""
    Dim _AccountNumber As String = ""
    Dim _CheckingFlag As Boolean = False
    Dim _SavingsFlag As Boolean = False
    Dim _Amount As Decimal = 0
    Dim _DateToRun As String = ""
    Dim _StatusID As Integer = 0
    Dim _DateCompleted As String = ""
    Dim _TransactionID As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CheckByPhone where CheckByPhoneID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CheckByPhone where CheckByPhoneID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CheckByPhone")
            If ds.Tables("t_CheckByPhone").Rows.Count > 0 Then
                dr = ds.Tables("t_CheckByPhone").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DateEntered") Is System.DBNull.Value) Then _DateEntered = dr("DateEntered")
        If Not (dr("EnteredByID") Is System.DBNull.Value) Then _EnteredByID = dr("EnteredByID")
        If Not (dr("AccountLastName") Is System.DBNull.Value) Then _AccountLastName = dr("AccountLastName")
        If Not (dr("AccountMiddleInit") Is System.DBNull.Value) Then _AccountMiddleInit = dr("AccountMiddleInit")
        If Not (dr("AccountFirstName") Is System.DBNull.Value) Then _AccountFirstName = dr("AccountFirstName")
        If Not (dr("ContractNumber") Is System.DBNull.Value) Then _ContractNumber = dr("ContractNumber")
        If Not (dr("RoutingNumber") Is System.DBNull.Value) Then _RoutingNumber = dr("RoutingNumber")
        If Not (dr("AccountNumber") Is System.DBNull.Value) Then _AccountNumber = dr("AccountNumber")
        If Not (dr("CheckingFlag") Is System.DBNull.Value) Then _CheckingFlag = dr("CheckingFlag")
        If Not (dr("SavingsFlag") Is System.DBNull.Value) Then _SavingsFlag = dr("SavingsFlag")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("DateToRun") Is System.DBNull.Value) Then _DateToRun = dr("DateToRun")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("DateCompleted") Is System.DBNull.Value) Then _DateCompleted = dr("DateCompleted")
        If Not (dr("TransactionID") Is System.DBNull.Value) Then _TransactionID = dr("TransactionID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CheckByPhone where CheckByPhoneID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CheckByPhone")
            If ds.Tables("t_CheckByPhone").Rows.Count > 0 Then
                dr = ds.Tables("t_CheckByPhone").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CheckByPhoneInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DateEntered", SqlDbType.datetime, 0, "DateEntered")
                da.InsertCommand.Parameters.Add("@EnteredByID", SqlDbType.int, 0, "EnteredByID")
                da.InsertCommand.Parameters.Add("@AccountLastName", SqlDbType.varchar, 0, "AccountLastName")
                da.InsertCommand.Parameters.Add("@AccountMiddleInit", SqlDbType.varchar, 0, "AccountMiddleInit")
                da.InsertCommand.Parameters.Add("@AccountFirstName", SqlDbType.VarChar, 0, "AccountFirstName")
                da.InsertCommand.Parameters.Add("@ContractNumber", SqlDbType.VarChar, 0, "ContractNumber")
                da.InsertCommand.Parameters.Add("@RoutingNumber", SqlDbType.varchar, 0, "RoutingNumber")
                da.InsertCommand.Parameters.Add("@AccountNumber", SqlDbType.varchar, 0, "AccountNumber")
                da.InsertCommand.Parameters.Add("@CheckingFlag", SqlDbType.bit, 0, "CheckingFlag")
                da.InsertCommand.Parameters.Add("@SavingsFlag", SqlDbType.bit, 0, "SavingsFlag")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@DateToRun", SqlDbType.varchar, 0, "DateToRun")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@DateCompleted", SqlDbType.varchar, 0, "DateCompleted")
                da.InsertCommand.Parameters.Add("@TransactionID", SqlDbType.char, 0, "TransactionID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CheckByPhoneID", SqlDbType.Int, 0, "CheckByPhoneID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CheckByPhone").NewRow
            End If
            Update_Field("DateEntered", _DateEntered, dr)
            Update_Field("EnteredByID", _EnteredByID, dr)
            Update_Field("AccountLastName", _AccountLastName, dr)
            Update_Field("AccountMiddleInit", _AccountMiddleInit, dr)
            Update_Field("AccountFirstName", _AccountFirstName, dr)
            Update_Field("ContractNumber", _ContractNumber, dr)
            Update_Field("RoutingNumber", _RoutingNumber, dr)
            Update_Field("AccountNumber", _AccountNumber, dr)
            Update_Field("CheckingFlag", _CheckingFlag, dr)
            Update_Field("SavingsFlag", _SavingsFlag, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("DateToRun", _DateToRun, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("DateCompleted", _DateCompleted, dr)
            Update_Field("TransactionID", _TransactionID, dr)
            If ds.Tables("t_CheckByPhone").Rows.Count < 1 Then ds.Tables("t_CheckByPhone").Rows.Add(dr)
            da.Update(ds, "t_CheckByPhone")
            _ID = ds.Tables("t_CheckByPhone").Rows(0).Item("CheckByPhoneID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "CheckByPhoneID"
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

    Public Property DateEntered() As String
        Get
            Return _DateEntered
        End Get
        Set(ByVal value As String)
            _DateEntered = value
        End Set
    End Property

    Public Property EnteredByID() As Integer
        Get
            Return _EnteredByID
        End Get
        Set(ByVal value As Integer)
            _EnteredByID = value
        End Set
    End Property

    Public Property AccountLastName() As String
        Get
            Return _AccountLastName
        End Get
        Set(ByVal value As String)
            _AccountLastName = value
        End Set
    End Property

    Public Property AccountMiddleInit() As String
        Get
            Return _AccountMiddleInit
        End Get
        Set(ByVal value As String)
            _AccountMiddleInit = value
        End Set
    End Property

    Public Property AccountFirstName() As String
        Get
            Return _AccountFirstName
        End Get
        Set(ByVal value As String)
            _AccountFirstName = value
        End Set
    End Property

    Public Property ContractNumber() As String
        Get
            Return _ContractNumber
        End Get
        Set(ByVal value As String)
            _ContractNumber = value
        End Set
    End Property

    Public Property RoutingNumber() As String
        Get
            Return _RoutingNumber
        End Get
        Set(ByVal value As String)
            _RoutingNumber = value
        End Set
    End Property

    Public Property AccountNumber() As String
        Get
            Return _AccountNumber
        End Get
        Set(ByVal value As String)
            _AccountNumber = value
        End Set
    End Property

    Public Property CheckingFlag() As Boolean
        Get
            Return _CheckingFlag
        End Get
        Set(ByVal value As Boolean)
            _CheckingFlag = value
        End Set
    End Property

    Public Property SavingsFlag() As Boolean
        Get
            Return _SavingsFlag
        End Get
        Set(ByVal value As Boolean)
            _SavingsFlag = value
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

    Public Property DateToRun() As String
        Get
            Return _DateToRun
        End Get
        Set(ByVal value As String)
            _DateToRun = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property DateCompleted() As String
        Get
            Return _DateCompleted
        End Get
        Set(ByVal value As String)
            _DateCompleted = value
        End Set
    End Property

    Public Property TransactionID() As String
        Get
            Return _TransactionID
        End Get
        Set(ByVal value As String)
            _TransactionID = value
        End Set
    End Property

    Public Property CheckByPhoneID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
