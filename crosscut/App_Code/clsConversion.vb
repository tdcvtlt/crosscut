Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsConversion
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _ConversionNumber As String = ""
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _FundingInstitutionID As Integer = 0
    Dim _FundingNumber As String = ""
    Dim _FundingStatusID As Integer = 0
    Dim _FundingPoolID As Integer = 0
    Dim _TitleTypeID As Integer = 0
    Dim _LegalDescription As String = ""
    Dim _OrigSellingPrice As Decimal = 0
    Dim _InsurancePolicyNum As String = ""
    Dim _InsuranceCompany As String = ""
    Dim _SalesVolume As Decimal = 0
    Dim _SalesPrice As Decimal = 0
    Dim _CommissionVolume As Decimal = 0
    Dim _Discount As Decimal = 0
    Dim _Equity As Decimal = 0
    Dim _UserAmount1 As Decimal = 0
    Dim _CCTotal As Decimal = 0
    Dim _DPTotal As Decimal = 0
    Dim _Balance As Decimal = 0
    Dim _CCFinanced As Decimal = 0
    Dim _InterestTypeID As Integer = 0
    Dim _PaymentTypeID As Integer = 0
    Dim _PaymentFee As Decimal = 0
    Dim _UserAmount2 As Decimal = 0
    Dim _UserAmount3 As Decimal = 0
    Dim _TotalFinanced As Decimal = 0
    Dim _APR As Decimal = 0
    Dim _Terms As Integer = 0
    Dim _FrequencyID As Integer = 0
    Dim _OriginationDate As String = ""
    Dim _FirstPaymentDate As String = ""
    Dim _OneTimeFee As Decimal = 0
    Dim _NextPaymentDate As String = ""
    Dim _GracePeriod As Integer = 0
    Dim _LateFeeExempt As Boolean = False
    Dim _LateFeePolicy As String = ""
    Dim _CalcAPR As Decimal = 0
    Dim _AutoPay As Boolean = False
    Dim _CashOutDate As String = ""
    Dim _DownPaymentDueDate As String = ""
    Dim _DateCreated As String = ""
    Dim _Err As String = ""
    Dim _occupancyYear As Int16 = 0

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Conversion where ConversionID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Conversion where ConversionID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Conversion")
            If ds.Tables("t_Conversion").Rows.Count > 0 Then
                dr = ds.Tables("t_Conversion").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("ConversionNumber") Is System.DBNull.Value) Then _ConversionNumber = dr("ConversionNumber")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("FundingInstitutionID") Is System.DBNull.Value) Then _FundingInstitutionID = dr("FundingInstitutionID")
        If Not (dr("FundingNumber") Is System.DBNull.Value) Then _FundingNumber = dr("FundingNumber")
        If Not (dr("FundingStatusID") Is System.DBNull.Value) Then _FundingStatusID = dr("FundingStatusID")
        If Not (dr("FundingPoolID") Is System.DBNull.Value) Then _FundingPoolID = dr("FundingPoolID")
        If Not (dr("TitleTypeID") Is System.DBNull.Value) Then _TitleTypeID = dr("TitleTypeID")
        If Not (dr("LegalDescription") Is System.DBNull.Value) Then _LegalDescription = dr("LegalDescription")
        If Not (dr("OrigSellingPrice") Is System.DBNull.Value) Then _OrigSellingPrice = dr("OrigSellingPrice")
        If Not (dr("InsurancePolicyNum") Is System.DBNull.Value) Then _InsurancePolicyNum = dr("InsurancePolicyNum")
        If Not (dr("InsuranceCompany") Is System.DBNull.Value) Then _InsuranceCompany = dr("InsuranceCompany")
        If Not (dr("SalesVolume") Is System.DBNull.Value) Then _SalesVolume = dr("SalesVolume")
        If Not (dr("SalesPrice") Is System.DBNull.Value) Then _SalesPrice = dr("SalesPrice")
        If Not (dr("CommissionVolume") Is System.DBNull.Value) Then _CommissionVolume = dr("CommissionVolume")
        If Not (dr("Discount") Is System.DBNull.Value) Then _Discount = dr("Discount")
        If Not (dr("Equity") Is System.DBNull.Value) Then _Equity = dr("Equity")
        If Not (dr("UserAmount1") Is System.DBNull.Value) Then _UserAmount1 = dr("UserAmount1")
        If Not (dr("CCTotal") Is System.DBNull.Value) Then _CCTotal = dr("CCTotal")
        If Not (dr("DPTotal") Is System.DBNull.Value) Then _DPTotal = dr("DPTotal")
        If Not (dr("Balance") Is System.DBNull.Value) Then _Balance = dr("Balance")
        If Not (dr("CCFinanced") Is System.DBNull.Value) Then _CCFinanced = dr("CCFinanced")
        If Not (dr("InterestTypeID") Is System.DBNull.Value) Then _InterestTypeID = dr("InterestTypeID")
        If Not (dr("PaymentTypeID") Is System.DBNull.Value) Then _PaymentTypeID = dr("PaymentTypeID")
        If Not (dr("PaymentFee") Is System.DBNull.Value) Then _PaymentFee = dr("PaymentFee")
        If Not (dr("UserAmount2") Is System.DBNull.Value) Then _UserAmount2 = dr("UserAmount2")
        If Not (dr("UserAmount3") Is System.DBNull.Value) Then _UserAmount3 = dr("UserAmount3")
        If Not (dr("TotalFinanced") Is System.DBNull.Value) Then _TotalFinanced = dr("TotalFinanced")
        If Not (dr("APR") Is System.DBNull.Value) Then _APR = dr("APR")
        If Not (dr("Terms") Is System.DBNull.Value) Then _Terms = dr("Terms")
        If Not (dr("FrequencyID") Is System.DBNull.Value) Then _FrequencyID = dr("FrequencyID")
        If Not (dr("OriginationDate") Is System.DBNull.Value) Then _OriginationDate = dr("OriginationDate")
        If Not (dr("FirstPaymentDate") Is System.DBNull.Value) Then _FirstPaymentDate = dr("FirstPaymentDate")
        If Not (dr("OneTimeFee") Is System.DBNull.Value) Then _OneTimeFee = dr("OneTimeFee")
        If Not (dr("NextPaymentDate") Is System.DBNull.Value) Then _NextPaymentDate = dr("NextPaymentDate")
        If Not (dr("GracePeriod") Is System.DBNull.Value) Then _GracePeriod = dr("GracePeriod")
        If Not (dr("LateFeeExempt") Is System.DBNull.Value) Then _LateFeeExempt = dr("LateFeeExempt")
        If Not (dr("LateFeePolicy") Is System.DBNull.Value) Then _LateFeePolicy = dr("LateFeePolicy")
        If Not (dr("CalcAPR") Is System.DBNull.Value) Then _CalcAPR = dr("CalcAPR")
        If Not (dr("AutoPay") Is System.DBNull.Value) Then _AutoPay = dr("AutoPay")
        If Not (dr("CashOutDate") Is System.DBNull.Value) Then _CashOutDate = dr("CashOutDate")
        If Not (dr("DownPaymentDueDate") Is System.DBNull.Value) Then _DownPaymentDueDate = dr("DownPaymentDueDate")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("OccupancyYear") Is System.DBNull.Value) Then _occupancyYear = dr("OccupancyYear")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Conversion where ConversionID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Conversion")
            If ds.Tables("t_Conversion").Rows.Count > 0 Then
                dr = ds.Tables("t_Conversion").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ConversionInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@ConversionNumber", SqlDbType.varchar, 0, "ConversionNumber")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.datetime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@FundingInstitutionID", SqlDbType.int, 0, "FundingInstitutionID")
                da.InsertCommand.Parameters.Add("@FundingNumber", SqlDbType.varchar, 0, "FundingNumber")
                da.InsertCommand.Parameters.Add("@FundingStatusID", SqlDbType.int, 0, "FundingStatusID")
                da.InsertCommand.Parameters.Add("@FundingPoolID", SqlDbType.int, 0, "FundingPoolID")
                da.InsertCommand.Parameters.Add("@TitleTypeID", SqlDbType.int, 0, "TitleTypeID")
                da.InsertCommand.Parameters.Add("@LegalDescription", SqlDbType.ntext, 0, "LegalDescription")
                da.InsertCommand.Parameters.Add("@OrigSellingPrice", SqlDbType.money, 0, "OrigSellingPrice")
                da.InsertCommand.Parameters.Add("@InsurancePolicyNum", SqlDbType.varchar, 0, "InsurancePolicyNum")
                da.InsertCommand.Parameters.Add("@InsuranceCompany", SqlDbType.varchar, 0, "InsuranceCompany")
                da.InsertCommand.Parameters.Add("@SalesVolume", SqlDbType.money, 0, "SalesVolume")
                da.InsertCommand.Parameters.Add("@SalesPrice", SqlDbType.money, 0, "SalesPrice")
                da.InsertCommand.Parameters.Add("@CommissionVolume", SqlDbType.money, 0, "CommissionVolume")
                da.InsertCommand.Parameters.Add("@Discount", SqlDbType.money, 0, "Discount")
                da.InsertCommand.Parameters.Add("@Equity", SqlDbType.money, 0, "Equity")
                da.InsertCommand.Parameters.Add("@UserAmount1", SqlDbType.money, 0, "UserAmount1")
                da.InsertCommand.Parameters.Add("@CCTotal", SqlDbType.money, 0, "CCTotal")
                da.InsertCommand.Parameters.Add("@DPTotal", SqlDbType.money, 0, "DPTotal")
                da.InsertCommand.Parameters.Add("@Balance", SqlDbType.money, 0, "Balance")
                da.InsertCommand.Parameters.Add("@CCFinanced", SqlDbType.money, 0, "CCFinanced")
                da.InsertCommand.Parameters.Add("@InterestTypeID", SqlDbType.int, 0, "InterestTypeID")
                da.InsertCommand.Parameters.Add("@PaymentTypeID", SqlDbType.int, 0, "PaymentTypeID")
                da.InsertCommand.Parameters.Add("@PaymentFee", SqlDbType.money, 0, "PaymentFee")
                da.InsertCommand.Parameters.Add("@UserAmount2", SqlDbType.money, 0, "UserAmount2")
                da.InsertCommand.Parameters.Add("@UserAmount3", SqlDbType.money, 0, "UserAmount3")
                da.InsertCommand.Parameters.Add("@TotalFinanced", SqlDbType.money, 0, "TotalFinanced")
                da.InsertCommand.Parameters.Add("@APR", SqlDbType.float, 0, "APR")
                da.InsertCommand.Parameters.Add("@Terms", SqlDbType.int, 0, "Terms")
                da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.int, 0, "FrequencyID")
                da.InsertCommand.Parameters.Add("@OriginationDate", SqlDbType.smalldatetime, 0, "OriginationDate")
                da.InsertCommand.Parameters.Add("@FirstPaymentDate", SqlDbType.smalldatetime, 0, "FirstPaymentDate")
                da.InsertCommand.Parameters.Add("@OneTimeFee", SqlDbType.money, 0, "OneTimeFee")
                da.InsertCommand.Parameters.Add("@NextPaymentDate", SqlDbType.smalldatetime, 0, "NextPaymentDate")
                da.InsertCommand.Parameters.Add("@GracePeriod", SqlDbType.int, 0, "GracePeriod")
                da.InsertCommand.Parameters.Add("@LateFeeExempt", SqlDbType.bit, 0, "LateFeeExempt")
                da.InsertCommand.Parameters.Add("@LateFeePolicy", SqlDbType.ntext, 0, "LateFeePolicy")
                da.InsertCommand.Parameters.Add("@CalcAPR", SqlDbType.float, 0, "CalcAPR")
                da.InsertCommand.Parameters.Add("@AutoPay", SqlDbType.bit, 0, "AutoPay")
                da.InsertCommand.Parameters.Add("@CashOutDate", SqlDbType.datetime, 0, "CashOutDate")
                da.InsertCommand.Parameters.Add("@DownPaymentDueDate", SqlDbType.datetime, 0, "DownPaymentDueDate")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@OccupancyYear", SqlDbType.SmallInt, 2, "OccupancyYear")

                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ConversionID", SqlDbType.Int, 0, "ConversionID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Conversion").NewRow
            End If
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("ConversionNumber", _ConversionNumber, dr)
            'Add code to add/remove restrictor (DelinquentMortgage)
            Dim oCI As New clsComboItems
            Dim oRes As New clsUsageRestriction2Contract
            Dim restrictorID As Long = 0
            Dim tieID As Integer = 0
            oRes.UserID = _UserID
            If oCI.Lookup_ComboItem(_StatusID) = "Delinquent" And oCI.Lookup_ComboItem(IIf(Not (dr("StatusID") Is System.DBNull.Value), dr("StatusID"), 0)) <> "Delinquent" Then
                'Add the Restrictor if it is not there already
                restrictorID = Get_RestrictionID()
                If restrictorID > 0 Then
                    If Not (Restrictor_Exists(restrictorID, tieID)) Then
                        oRes.UsageRestriction2ContractID = 0
                        oRes.Load()
                        oRes.ContractID = _ContractID
                        oRes.UsageRestrictionID = restrictorID
                        oRes.DateCreated = Date.Now
                        oRes.PersonnelID = _UserID
                        oRes.Save()
                    End If
                End If
            ElseIf oCI.Lookup_ComboItem(IIf(Not (dr("StatusID") Is System.DBNull.Value), dr("StatusID"), 0)) = "Delinquent" And oCI.Lookup_ComboItem(_StatusID) <> "Delinquent" Then
                'Remove the Restrictor if it is there
                restrictorID = Get_RestrictionID()
                If restrictorID > 0 And Restrictor_Exists(restrictorID, tieID) Then
                    oRes.Remove_Restrictor(tieID)
                End If
            Else
                'Do nothing
            End If
            oRes = Nothing
            oCI = Nothing
            'End code addition
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("FundingInstitutionID", _FundingInstitutionID, dr)
            Update_Field("FundingNumber", _FundingNumber, dr)
            Update_Field("FundingStatusID", _FundingStatusID, dr)
            Update_Field("FundingPoolID", _FundingPoolID, dr)
            Update_Field("TitleTypeID", _TitleTypeID, dr)
            Update_Field("LegalDescription", _LegalDescription, dr)
            Update_Field("OrigSellingPrice", _OrigSellingPrice, dr)
            Update_Field("InsurancePolicyNum", _InsurancePolicyNum, dr)
            Update_Field("InsuranceCompany", _InsuranceCompany, dr)
            Update_Field("SalesVolume", _SalesVolume, dr)
            Update_Field("SalesPrice", _SalesPrice, dr)
            Update_Field("CommissionVolume", _CommissionVolume, dr)
            Update_Field("Discount", _Discount, dr)
            Update_Field("Equity", _Equity, dr)
            Update_Field("UserAmount1", _UserAmount1, dr)
            Update_Field("CCTotal", _CCTotal, dr)
            Update_Field("DPTotal", _DPTotal, dr)
            Update_Field("Balance", _Balance, dr)
            Update_Field("CCFinanced", _CCFinanced, dr)
            Update_Field("InterestTypeID", _InterestTypeID, dr)
            Update_Field("PaymentTypeID", _PaymentTypeID, dr)
            Update_Field("PaymentFee", _PaymentFee, dr)
            Update_Field("UserAmount2", _UserAmount2, dr)
            Update_Field("UserAmount3", _UserAmount3, dr)
            Update_Field("TotalFinanced", _TotalFinanced, dr)
            Update_Field("APR", _APR, dr)
            Update_Field("Terms", _Terms, dr)
            Update_Field("FrequencyID", _FrequencyID, dr)
            Update_Field("OriginationDate", _OriginationDate, dr)
            Update_Field("FirstPaymentDate", _FirstPaymentDate, dr)
            Update_Field("OneTimeFee", _OneTimeFee, dr)
            Update_Field("NextPaymentDate", _NextPaymentDate, dr)
            Update_Field("GracePeriod", _GracePeriod, dr)
            Update_Field("LateFeeExempt", _LateFeeExempt, dr)
            Update_Field("LateFeePolicy", _LateFeePolicy, dr)
            Update_Field("CalcAPR", _CalcAPR, dr)
            Update_Field("AutoPay", _AutoPay, dr)
            Update_Field("CashOutDate", _CashOutDate, dr)
            Update_Field("DownPaymentDueDate", _DownPaymentDueDate, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("OccupancyYear", _occupancyYear, dr)

            If ds.Tables("t_Conversion").Rows.Count < 1 Then ds.Tables("t_Conversion").Rows.Add(dr)
            da.Update(ds, "t_Conversion")
            _ID = ds.Tables("t_Conversion").Rows(0).Item("ConversionID")

            

            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Private Function Restrictor_Exists(ByVal id As Long, ByRef res2cont As Integer) As Boolean
        Dim ret As Boolean = False
        Dim oRes As New clsUsageRestriction2Contract
        Dim t As DataTable = CType(oRes.List(_ContractID).Select(New DataSourceSelectArguments()), DataView).ToTable
        If t.Rows.Count > 0 Then
            For i = 0 To t.Rows.Count - 1
                If t.Rows(i)("UsageRestrictionID") = id Then
                    res2cont = t.Rows(i)("UsageRestriction2ContractID")
                    ret = True
                    Exit For
                End If
            Next
        End If
        oRes = Nothing
        Return ret
    End Function

    Private Function Get_RestrictionID() As Integer
        Dim oUR As New clsUsageRestriction
        Dim ret As Integer = 0
        Dim t As DataTable = CType(oUR.Get_Restrictions.Select(New DataSourceSelectArguments()), DataView).ToTable
        If t.Rows.Count > 0 Then
            For Each r As DataRow In t.Rows
                If r("Name") & "" = "DelinquentMortgage" Then
                    ret = r("UsageRestrictionID")
                    Exit For
                End If
            Next
        End If

        oUR = Nothing
        Return ret
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
            oEvents.KeyField = "ConversionID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function PMT() As Double
        Dim ans As Double
        Dim Rate As Double = _APR / 1200
        Dim mType As Integer = 0
        Dim PV As Double = _TotalFinanced
        ans = FormatCurrency(IIf(_TotalFinanced > 0, Math.Round((Rate * (mType + PV * (1 + Rate) ^ _Terms)) / ((1 - Rate * mType) * (1 - (1 + Rate) ^ _Terms)), 2), 0), 2) * -1
        Return ans
    End Function
    Public Function List_Conversions(ByVal contractID As Integer) As SQLDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select c.Conversionid, c.Conversionnumber, cs.Comboitem as Status, c.salesvolume, c.salesprice from t_Conversion c left outer join t_ComboItems cs on cs.comboitemid = c.statusid where ContractID = '" & contractID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function get_Conversion_ID(ByVal conID As Integer) As Integer
        Dim convID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ConversionID from t_Conversion where contractID = " & conID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                convID = dread("ConversionID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return convID
    End Function

    Public Function val_ConversionID(ByVal ID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as Conversions from t_Conversion where Conversionid = '" & ID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Conversions") > 0 Then
                bValid = True
            Else
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
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

    Public Property ConversionNumber() As String
        Get
            Return _ConversionNumber
        End Get
        Set(ByVal value As String)
            _ConversionNumber = value
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

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property FundingInstitutionID() As Integer
        Get
            Return _FundingInstitutionID
        End Get
        Set(ByVal value As Integer)
            _FundingInstitutionID = value
        End Set
    End Property

    Public Property FundingNumber() As String
        Get
            Return _FundingNumber
        End Get
        Set(ByVal value As String)
            _FundingNumber = value
        End Set
    End Property

    Public Property FundingStatusID() As Integer
        Get
            Return _FundingStatusID
        End Get
        Set(ByVal value As Integer)
            _FundingStatusID = value
        End Set
    End Property

    Public Property FundingPoolID() As Integer
        Get
            Return _FundingPoolID
        End Get
        Set(ByVal value As Integer)
            _FundingPoolID = value
        End Set
    End Property

    Public Property TitleTypeID() As Integer
        Get
            Return _TitleTypeID
        End Get
        Set(ByVal value As Integer)
            _TitleTypeID = value
        End Set
    End Property

    Public Property LegalDescription() As String
        Get
            Return _LegalDescription
        End Get
        Set(ByVal value As String)
            _LegalDescription = value
        End Set
    End Property

    Public Property OrigSellingPrice() As Decimal
        Get
            Return _OrigSellingPrice
        End Get
        Set(ByVal value As Decimal)
            _OrigSellingPrice = value
        End Set
    End Property

    Public Property InsurancePolicyNum() As String
        Get
            Return _InsurancePolicyNum
        End Get
        Set(ByVal value As String)
            _InsurancePolicyNum = value
        End Set
    End Property

    Public Property InsuranceCompany() As String
        Get
            Return _InsuranceCompany
        End Get
        Set(ByVal value As String)
            _InsuranceCompany = value
        End Set
    End Property

    Public Property SalesVolume() As Decimal
        Get
            Return _SalesVolume
        End Get
        Set(ByVal value As Decimal)
            _SalesVolume = value
        End Set
    End Property

    Public Property SalesPrice() As Decimal
        Get
            Return _SalesPrice
        End Get
        Set(ByVal value As Decimal)
            _SalesPrice = value
        End Set
    End Property

    Public Property CommissionVolume() As Decimal
        Get
            Return _CommissionVolume
        End Get
        Set(ByVal value As Decimal)
            _CommissionVolume = value
        End Set
    End Property

    Public Property Discount() As Decimal
        Get
            Return _Discount
        End Get
        Set(ByVal value As Decimal)
            _Discount = value
        End Set
    End Property

    Public Property Equity() As Decimal
        Get
            Return _Equity
        End Get
        Set(ByVal value As Decimal)
            _Equity = value
        End Set
    End Property

    Public Property UserAmount1() As Decimal
        Get
            Return _UserAmount1
        End Get
        Set(ByVal value As Decimal)
            _UserAmount1 = value
        End Set
    End Property

    Public Property CCTotal() As Decimal
        Get
            Return _CCTotal
        End Get
        Set(ByVal value As Decimal)
            _CCTotal = value
        End Set
    End Property

    Public Property DPTotal() As Decimal
        Get
            Return _DPTotal
        End Get
        Set(ByVal value As Decimal)
            _DPTotal = value
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

    Public Property CCFinanced() As Decimal
        Get
            Return _CCFinanced
        End Get
        Set(ByVal value As Decimal)
            _CCFinanced = value
        End Set
    End Property

    Public Property InterestTypeID() As Integer
        Get
            Return _InterestTypeID
        End Get
        Set(ByVal value As Integer)
            _InterestTypeID = value
        End Set
    End Property

    Public Property PaymentTypeID() As Integer
        Get
            Return _PaymentTypeID
        End Get
        Set(ByVal value As Integer)
            _PaymentTypeID = value
        End Set
    End Property

    Public Property PaymentFee() As Decimal
        Get
            Return _PaymentFee
        End Get
        Set(ByVal value As Decimal)
            _PaymentFee = value
        End Set
    End Property

    Public Property UserAmount2() As Decimal
        Get
            Return _UserAmount2
        End Get
        Set(ByVal value As Decimal)
            _UserAmount2 = value
        End Set
    End Property

    Public Property UserAmount3() As Decimal
        Get
            Return _UserAmount3
        End Get
        Set(ByVal value As Decimal)
            _UserAmount3 = value
        End Set
    End Property

    Public Property TotalFinanced() As Decimal
        Get
            Return _TotalFinanced
        End Get
        Set(ByVal value As Decimal)
            _TotalFinanced = value
        End Set
    End Property

    Public Property APR() As Decimal
        Get
            Return _APR
        End Get
        Set(ByVal value As Decimal)
            _APR = value
        End Set
    End Property

    Public Property Terms() As Integer
        Get
            Return _Terms
        End Get
        Set(ByVal value As Integer)
            _Terms = value
        End Set
    End Property

    Public Property FrequencyID() As Integer
        Get
            Return _FrequencyID
        End Get
        Set(ByVal value As Integer)
            _FrequencyID = value
        End Set
    End Property

    Public Property OriginationDate() As String
        Get
            Return _OriginationDate
        End Get
        Set(ByVal value As String)
            _OriginationDate = value
        End Set
    End Property

    Public Property FirstPaymentDate() As String
        Get
            Return _FirstPaymentDate
        End Get
        Set(ByVal value As String)
            _FirstPaymentDate = value
        End Set
    End Property

    Public Property OneTimeFee() As Decimal
        Get
            Return _OneTimeFee
        End Get
        Set(ByVal value As Decimal)
            _OneTimeFee = value
        End Set
    End Property

    Public Property NextPaymentDate() As String
        Get
            Return _NextPaymentDate
        End Get
        Set(ByVal value As String)
            _NextPaymentDate = value
        End Set
    End Property

    Public Property GracePeriod() As Integer
        Get
            Return _GracePeriod
        End Get
        Set(ByVal value As Integer)
            _GracePeriod = value
        End Set
    End Property

    Public Property LateFeeExempt() As Boolean
        Get
            Return _LateFeeExempt
        End Get
        Set(ByVal value As Boolean)
            _LateFeeExempt = value
        End Set
    End Property

    Public Property LateFeePolicy() As String
        Get
            Return _LateFeePolicy
        End Get
        Set(ByVal value As String)
            _LateFeePolicy = value
        End Set
    End Property

    Public Property CalcAPR() As Decimal
        Get
            Return _CalcAPR
        End Get
        Set(ByVal value As Decimal)
            _CalcAPR = value
        End Set
    End Property

    Public Property AutoPay() As Boolean
        Get
            Return _AutoPay
        End Get
        Set(ByVal value As Boolean)
            _AutoPay = value
        End Set
    End Property

    Public Property CashOutDate() As String
        Get
            Return _CashOutDate
        End Get
        Set(ByVal value As String)
            _CashOutDate = value
        End Set
    End Property

    Public Property DownPaymentDueDate() As String
        Get
            Return _DownPaymentDueDate
        End Get
        Set(ByVal value As String)
            _DownPaymentDueDate = value
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
    Public Property ConversionID() As Integer
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
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property


    Public Property OccupancyYear As Integer
        Get
            Return _occupancyYear
        End Get
        Set(ByVal value As Integer)
            _occupancyYear = value
        End Set
    End Property
End Class
