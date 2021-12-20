Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsReDeeds
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ReDeededTo As Integer = 0
    Dim _ReDeededNewKCP As String = ""
    Dim _ReDeededFrom As Integer = 0
    Dim _ReDeededOldKCP As String = ""
    Dim _ReDeedDate As String = ""
    Dim _TransferFee As Decimal = 0
    Dim _ReDeedTypeID As Integer = 0
    Dim _DeceasedDate As String = ""
    Dim _Executor As String = ""
    Dim _Finalized As Boolean = False
    Dim _StartDate As String = ""
    Dim _FinalizedDate As String = ""
    Dim _CurrentStep As Integer = 0
    Dim _TitleTypeID As Integer = 0
    Dim _FirstOccupancy As Integer = 0
    Dim _DeedType As String = ""

    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ReDeeds where ReDeedID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ReDeeds where ReDeedID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ReDeeds")
            If ds.Tables("t_ReDeeds").Rows.Count > 0 Then
                dr = ds.Tables("t_ReDeeds").Rows(0)
                Set_Values()
                Set_KCPs()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Sub

    Private Sub Set_KCPs()
        If cn.State <> ConnectionState.Open Then cn.Open()
        If _ReDeededFrom > 0 Then

            cm.CommandText = "Select ContractNumber from t_Contract where ContractID = " & _ReDeededFrom
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                _ReDeededOldKCP = ds.Tables("0").Rows(0)("ContractNumber")
            End If
            ds.Tables("0").Clear()

        End If

        If _ReDeededTo > 0 Then

            cm.CommandText = "Select ContractNumber,OccupancyDate from t_Contract where ContractID = " & _ReDeededTo
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                _ReDeededNewKCP = ds.Tables("0").Rows(0)("ContractNumber")
                If Not (ds.Tables("0").Rows(0)("OccupancyDate") Is System.DBNull.Value) Then
                    _FirstOccupancy = Year(ds.Tables("0").Rows(0)("OccupancyDate"))
                End If
            End If
            ds.Tables("0").Clear()

            cm.CommandText = "Select TitleTypeID from t_Mortgage where contractid = " & _ReDeededTo
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                _TitleTypeID = ds.Tables("0").Rows(0)("TitleTypeID")
            End If
            ds.Tables("0").Clear()

            cm.CommandText = "Select Comboitem from t_comboitems where comboitemid = " & _ReDeedTypeID
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                _DeedType = ds.Tables("0").Rows(0)("Comboitem")
            End If
            ds.Tables("0").Clear()

        End If
        If cn.State <> ConnectionState.Closed Then cn.Close()
    End Sub

    Private Sub Set_Values()
        If Not (dr("ReDeededTo") Is System.DBNull.Value) Then _ReDeededTo = dr("ReDeededTo")
        If Not (dr("ReDeededFrom") Is System.DBNull.Value) Then _ReDeededFrom = dr("ReDeededFrom")
        If Not (dr("ReDeedDate") Is System.DBNull.Value) Then _ReDeedDate = dr("ReDeedDate")
        If Not (dr("TransferFee") Is System.DBNull.Value) Then _TransferFee = dr("TransferFee")
        If Not (dr("ReDeedTypeID") Is System.DBNull.Value) Then _ReDeedTypeID = dr("ReDeedTypeID")
        If Not (dr("DeceasedDate") Is System.DBNull.Value) Then _DeceasedDate = dr("DeceasedDate")
        If Not (dr("Executor") Is System.DBNull.Value) Then _Executor = dr("Executor")
        If Not (dr("Finalized") Is System.DBNull.Value) Then _Finalized = dr("Finalized")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("FinalizedDate") Is System.DBNull.Value) Then _FinalizedDate = dr("FinalizedDate")
        If Not (dr("CurrentStep") Is System.DBNull.Value) Then _CurrentStep = dr("CurrentStep")
    End Sub

    Public Function List_Pending() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select r.ReDeedID, c.contractnumber as KCP, c2.contractnumber as NewKCP, r.ReDeedDate,rt.comboitem as Type, r.StartDate from t_Redeeds r inner join t_Contract c on c.contractid = r.redeededfrom inner join t_Contract c2 on c2.contractid = r.redeededto inner join t_Comboitems rt on rt.comboitemid = r.redeedtypeid where Finalized <> 1 "
            Return ds
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List_Completed() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select r.ReDeedID, c.contractnumber as KCP, c2.contractnumber as NewKCP, r.ReDeedDate,rt.comboitem as Type, r.StartDate from t_Redeeds r inner join t_Contract c on c.contractid = r.redeededfrom inner join t_Contract c2 on c2.contractid = r.redeededto inner join t_Comboitems rt on rt.comboitemid = r.redeedtypeid where Finalized = 1 "
            Return ds
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function


    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ReDeeds where ReDeedID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ReDeeds")
            If ds.Tables("t_ReDeeds").Rows.Count > 0 Then
                dr = ds.Tables("t_ReDeeds").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReDeedsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ReDeededTo", SqlDbType.int, 0, "ReDeededTo")
                da.InsertCommand.Parameters.Add("@ReDeededFrom", SqlDbType.int, 0, "ReDeededFrom")
                da.InsertCommand.Parameters.Add("@ReDeedDate", SqlDbType.datetime, 0, "ReDeedDate")
                da.InsertCommand.Parameters.Add("@TransferFee", SqlDbType.money, 0, "TransferFee")
                da.InsertCommand.Parameters.Add("@ReDeedTypeID", SqlDbType.Int, 0, "ReDeedTypeID")
                da.InsertCommand.Parameters.Add("@DeceasedDate", SqlDbType.datetime, 0, "DeceasedDate")
                da.InsertCommand.Parameters.Add("@Executor", SqlDbType.varchar, 0, "Executor")
                da.InsertCommand.Parameters.Add("@Finalized", SqlDbType.bit, 0, "Finalized")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.datetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@FinalizedDate", SqlDbType.DateTime, 0, "FinalizedDate")
                da.InsertCommand.Parameters.Add("@CurrentStep", SqlDbType.Int, 0, "CurrentStep")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReDeedID", SqlDbType.Int, 0, "ReDeedID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ReDeeds").NewRow
            End If
            Update_Field("ReDeededTo", _ReDeededTo, dr)
            Update_Field("ReDeededFrom", _ReDeededFrom, dr)
            Update_Field("ReDeedDate", _ReDeedDate, dr)
            Update_Field("TransferFee", _TransferFee, dr)
            Update_Field("ReDeedTypeID", _ReDeedTypeID, dr)
            Update_Field("DeceasedDate", _DeceasedDate, dr)
            Update_Field("Executor", _Executor, dr)
            Update_Field("Finalized", _Finalized, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("FinalizedDate", _FinalizedDate, dr)
            Update_Field("CurrentStep", _CurrentStep, dr)
            If ds.Tables("t_ReDeeds").Rows.Count < 1 Then ds.Tables("t_ReDeeds").Rows.Add(dr)
            da.Update(ds, "t_ReDeeds")
            _ID = ds.Tables("t_ReDeeds").Rows(0).Item("ReDeedID")
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
            oEvents.KeyField = "ReDeedID"
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

#Region "Properties"

    Public ReadOnly Property DeedType As String
        Get
            Return _DeedType
        End Get
    End Property

    Public ReadOnly Property FirstOccupancyYear As Integer
        Get
            Return _FirstOccupancy
        End Get
    End Property

    Public ReadOnly Property TitleTypeID As Integer
        Get
            Return _TitleTypeID
        End Get
    End Property

    Public Property ReDeededNewKCP As String
        Get
            Return _ReDeededNewKCP
        End Get
        Set(ByVal value As String)
            _ReDeededNewKCP = value
        End Set
    End Property

    Public Property ReDeededOldKCP As String
        Get
            Return _ReDeededOldKCP
        End Get
        Set(ByVal value As String)
            _ReDeededOldKCP = value
        End Set
    End Property

    Public Property CurrentStep As Integer
        Get
            Return _CurrentStep
        End Get
        Set(ByVal value As Integer)
            _CurrentStep = value
        End Set
    End Property

    Public Property ReDeededTo() As Integer
        Get
            Return _ReDeededTo
        End Get
        Set(ByVal value As Integer)
            _ReDeededTo = value
        End Set
    End Property

    Public Property ReDeededFrom() As Integer
        Get
            Return _ReDeededFrom
        End Get
        Set(ByVal value As Integer)
            _ReDeededFrom = value
        End Set
    End Property

    Public Property ReDeedDate() As String
        Get
            Return _ReDeedDate
        End Get
        Set(ByVal value As String)
            _ReDeedDate = value
        End Set
    End Property

    Public Property TransferFee() As Decimal
        Get
            Return _TransferFee
        End Get
        Set(ByVal value As Decimal)
            _TransferFee = value
        End Set
    End Property

    Public Property ReDeedTypeID() As Integer
        Get
            Return _ReDeedTypeID
        End Get
        Set(ByVal value As Integer)
            _ReDeedTypeID = value
        End Set
    End Property

    Public Property DeceasedDate() As String
        Get
            Return _DeceasedDate
        End Get
        Set(ByVal value As String)
            _DeceasedDate = value
        End Set
    End Property

    Public Property Executor() As String
        Get
            Return _Executor
        End Get
        Set(ByVal value As String)
            _Executor = value
        End Set
    End Property

    Public Property Finalized() As Boolean
        Get
            Return _Finalized
        End Get
        Set(ByVal value As Boolean)
            _Finalized = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property FinalizedDate() As String
        Get
            Return _FinalizedDate
        End Get
        Set(ByVal value As String)
            _FinalizedDate = value
        End Set
    End Property

    Public Property ReDeedID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
#End Region
End Class
