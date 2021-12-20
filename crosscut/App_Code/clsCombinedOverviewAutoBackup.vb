Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCombinedOverviewAutoBackup
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CombinedOverviewID As Integer = 0
    Dim _TourID As Integer = 0
    Dim _TourStatusID As Integer = 0
    Dim _TourTypeID As Integer = 0
    Dim _TourSubTypeID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _ContractStatusID As Integer = 0
    Dim _ContractBillingCodeID As Integer = 0
    Dim _ContractFrequencyID As Integer = 0
    Dim _MortgageSalesVolume As Decimal = 0
    Dim _ContractDate As String = ""
    Dim _ContractStatusDate As String = ""
    Dim _Fieldname As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CombinedOverviewAutoBackup where CombinedOverviewBackupID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CombinedOverviewAutoBackup where CombinedOverviewBackupID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CombinedOverviewAutoBackup")
            If ds.Tables("t_CombinedOverviewAutoBackup").Rows.Count > 0 Then
                dr = ds.Tables("t_CombinedOverviewAutoBackup").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("CombinedOverviewID") Is System.DBNull.Value) Then _CombinedOverviewID = dr("CombinedOverviewID")
        If Not (dr("TourID") Is System.DBNull.Value) Then _TourID = dr("TourID")
        If Not (dr("TourStatusID") Is System.DBNull.Value) Then _TourStatusID = dr("TourStatusID")
        If Not (dr("TourTypeID") Is System.DBNull.Value) Then _TourTypeID = dr("TourTypeID")
        If Not (dr("TourSubTypeID") Is System.DBNull.Value) Then _TourSubTypeID = dr("TourSubTypeID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("ContractStatusID") Is System.DBNull.Value) Then _ContractStatusID = dr("ContractStatusID")
        If Not (dr("ContractBillingCodeID") Is System.DBNull.Value) Then _ContractBillingCodeID = dr("ContractBillingCodeID")
        If Not (dr("ContractFrequencyID") Is System.DBNull.Value) Then _ContractFrequencyID = dr("ContractFrequencyID")
        If Not (dr("MortgageSalesVolume") Is System.DBNull.Value) Then _MortgageSalesVolume = dr("MortgageSalesVolume")
        If Not (dr("ContractDate") Is System.DBNull.Value) Then _ContractDate = dr("ContractDate")
        If Not (dr("ContractStatusDate") Is System.DBNull.Value) Then _ContractStatusDate = dr("ContractStatusDate")
        If Not (dr("Fieldname") Is System.DBNull.Value) Then _Fieldname = dr("Fieldname")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CombinedOverviewAutoBackup where CombinedOverviewBackupID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CombinedOverviewAutoBackup")
            If ds.Tables("t_CombinedOverviewAutoBackup").Rows.Count > 0 Then
                dr = ds.Tables("t_CombinedOverviewAutoBackup").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CombinedOverviewAutoBackupInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CombinedOverviewID", SqlDbType.int, 0, "CombinedOverviewID")
                da.InsertCommand.Parameters.Add("@TourID", SqlDbType.int, 0, "TourID")
                da.InsertCommand.Parameters.Add("@TourStatusID", SqlDbType.int, 0, "TourStatusID")
                da.InsertCommand.Parameters.Add("@TourTypeID", SqlDbType.int, 0, "TourTypeID")
                da.InsertCommand.Parameters.Add("@TourSubTypeID", SqlDbType.int, 0, "TourSubTypeID")
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@ContractStatusID", SqlDbType.int, 0, "ContractStatusID")
                da.InsertCommand.Parameters.Add("@ContractBillingCodeID", SqlDbType.int, 0, "ContractBillingCodeID")
                da.InsertCommand.Parameters.Add("@ContractFrequencyID", SqlDbType.int, 0, "ContractFrequencyID")
                da.InsertCommand.Parameters.Add("@MortgageSalesVolume", SqlDbType.money, 0, "MortgageSalesVolume")
                da.InsertCommand.Parameters.Add("@ContractDate", SqlDbType.datetime, 0, "ContractDate")
                da.InsertCommand.Parameters.Add("@ContractStatusDate", SqlDbType.datetime, 0, "ContractStatusDate")
                da.InsertCommand.Parameters.Add("@Fieldname", SqlDbType.nchar, 0, "Fieldname")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CombinedOverviewBackupID", SqlDbType.Int, 0, "CombinedOverviewBackupID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CombinedOverviewAutoBackup").NewRow
            End If
            Update_Field("CombinedOverviewID", _CombinedOverviewID, dr)
            Update_Field("TourID", _TourID, dr)
            Update_Field("TourStatusID", _TourStatusID, dr)
            Update_Field("TourTypeID", _TourTypeID, dr)
            Update_Field("TourSubTypeID", _TourSubTypeID, dr)
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("ContractStatusID", _ContractStatusID, dr)
            Update_Field("ContractBillingCodeID", _ContractBillingCodeID, dr)
            Update_Field("ContractFrequencyID", _ContractFrequencyID, dr)
            Update_Field("MortgageSalesVolume", _MortgageSalesVolume, dr)
            Update_Field("ContractDate", _ContractDate, dr)
            Update_Field("ContractStatusDate", _ContractStatusDate, dr)
            Update_Field("Fieldname", _Fieldname, dr)
            If ds.Tables("t_CombinedOverviewAutoBackup").Rows.Count < 1 Then ds.Tables("t_CombinedOverviewAutoBackup").Rows.Add(dr)
            da.Update(ds, "t_CombinedOverviewAutoBackup")
            _ID = ds.Tables("t_CombinedOverviewAutoBackup").Rows(0).Item("CombinedOverviewBackupID")
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
            oEvents.KeyField = "CombinedOverviewBackupID"
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

    Public Function List_Tour_Backup(ByVal overViewID As Integer, ByVal fieldname As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select TourID,ts.comboitem as TourStatus, tt.comboitem as TourType, tst.comboitem as TourSubType from t_CombinedOverviewAutoBackup o left outer join t_Comboitems ts on ts.comboitemid = o.tourstatusid left outer join t_comboitems tt on tt.comboitemid = o.tourtypeid left outer join t_comboitems tst on tst.comboitemid = o.toursubtypeid where CombinedOverviewID = '" & overviewID & "' and fieldname = '" & fieldname & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Contract_Backup(ByVal overViewID As Integer, ByVal fieldname As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select c.ContractNumber as KCP#,  cs.comboitem as Status, bc.comboitem as BillingCode, f.Frequency, o.MortgageSalesVolume as SalesVolume, c.ContractDate,c.StatusDate from t_CombinedOverviewAutoBackup o left outer join t_Comboitems cs on cs.comboitemid = o.contractstatusid left outer join t_comboitems bc on bc.comboitemid = o.contractbillingcodeid left outer join t_Frequency f on f.frequencyid = o.contractfrequencyid left outer join t_Mortgage m on m.contractid = o.contractid inner join t_Contract c on c.contractid = o.contractid where o.CombinedOverviewID = '" & overViewID & "' and o.fieldname = '" & fieldname & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function


    Public Property CombinedOverviewID() As Integer
        Get
            Return _CombinedOverviewID
        End Get
        Set(ByVal value As Integer)
            _CombinedOverviewID = value
        End Set
    End Property

    Public Property TourID() As Integer
        Get
            Return _TourID
        End Get
        Set(ByVal value As Integer)
            _TourID = value
        End Set
    End Property

    Public Property TourStatusID() As Integer
        Get
            Return _TourStatusID
        End Get
        Set(ByVal value As Integer)
            _TourStatusID = value
        End Set
    End Property

    Public Property TourTypeID() As Integer
        Get
            Return _TourTypeID
        End Get
        Set(ByVal value As Integer)
            _TourTypeID = value
        End Set
    End Property

    Public Property TourSubTypeID() As Integer
        Get
            Return _TourSubTypeID
        End Get
        Set(ByVal value As Integer)
            _TourSubTypeID = value
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

    Public Property ContractStatusID() As Integer
        Get
            Return _ContractStatusID
        End Get
        Set(ByVal value As Integer)
            _ContractStatusID = value
        End Set
    End Property

    Public Property ContractBillingCodeID() As Integer
        Get
            Return _ContractBillingCodeID
        End Get
        Set(ByVal value As Integer)
            _ContractBillingCodeID = value
        End Set
    End Property

    Public Property ContractFrequencyID() As Integer
        Get
            Return _ContractFrequencyID
        End Get
        Set(ByVal value As Integer)
            _ContractFrequencyID = value
        End Set
    End Property

    Public Property MortgageSalesVolume() As Decimal
        Get
            Return _MortgageSalesVolume
        End Get
        Set(ByVal value As Decimal)
            _MortgageSalesVolume = value
        End Set
    End Property

    Public Property ContractDate() As String
        Get
            Return _ContractDate
        End Get
        Set(ByVal value As String)
            _ContractDate = value
        End Set
    End Property

    Public Property ContractStatusDate() As String
        Get
            Return _ContractStatusDate
        End Get
        Set(ByVal value As String)
            _ContractStatusDate = value
        End Set
    End Property

    Public Property Fieldname() As String
        Get
            Return _Fieldname
        End Get
        Set(ByVal value As String)
            _Fieldname = value
        End Set
    End Property

    Public Property CombinedOverviewBackupID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

