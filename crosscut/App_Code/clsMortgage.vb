Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports Microsoft.VisualBasic.Financial

Public Class clsMortgage
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _Number As String = ""
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _TitleTypeID As Integer = 0
    Dim _OrigSellingPrice As Decimal = 0
    Dim _InsurancePolicyID As Integer = 0
    Dim _SalesVolume As Decimal = 0
    Dim _SalesPrice As Decimal = 0
    Dim _CommVolume As Decimal = 0
    Dim _Discount As Decimal = 0
    Dim _UserAmount1 As Decimal = 0
    Dim _UserAmount2 As Decimal = 0
    Dim _UserAmount3 As Decimal = 0
    Dim _TotalFinanced As Decimal = 0
    Dim _CCTotal As Decimal = 0
    Dim _CCFinanced As Decimal = 0
    Dim _DPTotal As Decimal = 0
    Dim _DPBalance As Decimal = 0
    Dim _InterestTypeID As Integer = 0
    Dim _PaymentTypeID As Integer = 0
    Dim _PaymentFee As Decimal = 0
    Dim _APR As Decimal = 0
    Dim _Terms As Integer = 0
    Dim _FrequencyID As Integer = 0
    Dim _OrigDate As String = ""
    Dim _FirstPaymentDate As String = ""
    Dim _OneTimeFee As Decimal = 0
    Dim _GracePeriod As Integer = 0
    Dim _CalcAPR As Decimal = 0
    Dim _AutoPay As Boolean
    Dim _CashOutDate As String = ""
    Dim _DPDueDate As String = ""
    Dim _DPInvoiceID As Integer = 0
    Dim _CCInvoiceID As Integer = 0
    Dim _IgnoreStatusDate As Boolean = False

    Dim _Err As String = ""


    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Mortgage where MortgageID = " & _ID, cn)
        da = New SqlDataAdapter(cm)
    End Sub

    Public Function Get_Owner_Link() As String
        Dim sRet As String = ""
        If _ID > 0 Then
            cm.CommandText = "Select p.ProspectID, p.Lastname + ', ' + p.firstname as Name from t_Prospect p inner join t_Contract c on c.prospectid = p.prospectid inner join t_Mortgage m on m.contractid = c.contractid where m.mortgageid = " & _ID
            da.Fill(ds, "0")
            If ds.Tables("0").Rows.Count > 0 Then
                sRet = ds.Tables("0").Rows(0)("ProspectID") & "||" & ds.Tables("0").Rows(0)("Name")
            End If
            ds.Tables("0").Clear()
        End If

        Return sRet
    End Function
    Public Sub Load()
        Try
            If _ContractID > 0 And _ID > 0 Then
                cm.CommandText = "Select * from t_Mortgage where MortgageID = " & _ID & " or ContractID=" & _ContractID
            ElseIf _ContractID > 0 Then
                cm.CommandText = "Select * from t_Mortgage where MortgageID = " & _ID & " or ContractID=" & _ContractID
            Else
                cm.CommandText = "Select * from t_Mortgage where MortgageID = " & _ID
            End If
            ds = New DataSet
            da.Fill(ds, "Mortgage")

            If ds.Tables("Mortgage").Rows.Count > 0 Then
                dr = ds.Tables("Mortgage").Rows(0)
                Set_Values()
            End If
            _Err = ds.Tables("Mortgage").Rows.Count
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Query(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " m.MortgageID as ID, ms.comboitem as Status, m.SalesVolume, m.SalesPrice, m.Number as EquiantID from t_Mortgage m left outer join t_Comboitems ms on ms.comboitemid = m.statusid "
            If sFilterField <> "" Then
                If sFilterField = "ContractNumber" Then
                    If InStr(sFilterValue, ",") > 0 And InStr(sFilterValue, ", ") < 1 Then sFilterValue = Replace(sFilterValue, ",", ", ")
                    sql += " where m.contractid in (select contractid from t_Contract where contractnumber like '" & sFilterValue & "%') "
                Else
                    sql += " where " & sFilterField & " like '" & sFilterValue & "' "
                End If
            End If

            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " m.MortgageID as ID, ms.comboitem as Status, m.SalesVolume, m.SalesPrice, m.Number as EquiantID from t_Mortgage m left outer join t_Comboitems ms on ms.comboitemid = m.statusid "
            sql += IIf(sFilterField <> "", " where " & sFilterField & " = '" & sFilterValue & "' ", "")
            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
            Catch ex As Exception
                _Err = ex.ToString
            End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("MortgageID") Is System.DBNull.Value) Then _ID = dr("MortgageID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("Number") Is System.DBNull.Value) Then _Number = dr("Number")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("TitleTypeID") Is System.DBNull.Value) Then _TitleTypeID = dr("TitleTypeID")
        If Not (dr("OrigSellingPrice") Is System.DBNull.Value) Then _OrigSellingPrice = dr("OrigSellingPrice")
        If Not (dr("InsurancePolicyID") Is System.DBNull.Value) Then _InsurancePolicyID = dr("InsurancePolicyID")
        If Not (dr("SalesVolume") Is System.DBNull.Value) Then _SalesVolume = dr("SalesVolume")
        If Not (dr("SalesPrice") Is System.DBNull.Value) Then _SalesPrice = dr("SalesPrice")
        If Not (dr("CommVolume") Is System.DBNull.Value) Then _CommVolume = dr("CommVolume")
        If Not (dr("Discount") Is System.DBNull.Value) Then _Discount = dr("Discount")
        If Not (dr("UserAmount1") Is System.DBNull.Value) Then _UserAmount1 = dr("UserAmount1")
        If Not (dr("UserAmount2") Is System.DBNull.Value) Then _UserAmount2 = dr("UserAmount2")
        If Not (dr("UserAmount3") Is System.DBNull.Value) Then _UserAmount3 = dr("UserAmount3")
        If Not (dr("TotalFinanced") Is System.DBNull.Value) Then _TotalFinanced = dr("TotalFinanced")
        If Not (dr("CCTotal") Is System.DBNull.Value) Then _CCTotal = dr("CCTotal")
        If Not (dr("CCFinanced") Is System.DBNull.Value) Then _CCFinanced = dr("CCFinanced")
        If Not (dr("DPTotal") Is System.DBNull.Value) Then _DPTotal = dr("DPTotal")
        If Not (dr("DPBalance") Is System.DBNull.Value) Then _DPBalance = dr("DPBalance")
        If Not (dr("InterestTypeID") Is System.DBNull.Value) Then _InterestTypeID = dr("InterestTypeID")
        If Not (dr("PaymentTypeID") Is System.DBNull.Value) Then _PaymentTypeID = dr("PaymentTypeID")
        If Not (dr("PaymentFee") Is System.DBNull.Value) Then _PaymentFee = dr("PaymentFee")
        If Not (dr("APR") Is System.DBNull.Value) Then _APR = dr("APR")
        If Not (dr("Terms") Is System.DBNull.Value) Then _Terms = dr("Terms")
        If Not (dr("FrequencyID") Is System.DBNull.Value) Then _FrequencyID = dr("FrequencyID")
        If Not (dr("OrigDate") Is System.DBNull.Value) Then _OrigDate = dr("OrigDate")
        If Not (dr("FirstPaymentDate") Is System.DBNull.Value) Then _FirstPaymentDate = dr("FirstPaymentDate")
        If Not (dr("OneTimeFee") Is System.DBNull.Value) Then _OneTimeFee = dr("OneTimeFee")
        If Not (dr("GracePeriod") Is System.DBNull.Value) Then _GracePeriod = dr("GracePeriod")
        If Not (dr("CalcAPR") Is System.DBNull.Value) Then _CalcAPR = dr("CalcAPR")
        If Not (dr("AutoPay") Is System.DBNull.Value) Then _AutoPay = dr("AutoPay")
        If Not (dr("CashOutDate") Is System.DBNull.Value) Then _CashOutDate = dr("CashOutDate")
        If Not (dr("DPDueDate") Is System.DBNull.Value) Then _DPDueDate = dr("DPDueDate")
        If Not (dr("DPInvoiceID") Is System.DBNull.Value) Then _DPInvoiceID = dr("DPInvoiceID")
        If Not (dr("CCInvoiceID") Is System.DBNull.Value) Then _CCInvoiceID = dr("CCInvoiceID")
    End Sub

    Public Function Save() As Boolean
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_Mortgage where mortgageid = " & _ID
        da = New SqlDataAdapter(cm)
        Dim sqlCMBuild As New SqlCommandBuilder(da)
        ds = New DataSet
        da.Fill(ds, "Mort")
        If ds.Tables("Mort").Rows.Count > 0 Then
            dr = ds.Tables("Mort").Rows(0)
        Else
            da.InsertCommand = New SqlCommand("dbo.sp_MortgageInsert", cn)
            da.InsertCommand.CommandType = CommandType.StoredProcedure
            da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.Int, 0, "ContractID")
            da.InsertCommand.Parameters.Add("@Number", SqlDbType.VarChar, 50, "Number")
            da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
            da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.SmallDateTime, 0, "StatusDate")
            da.InsertCommand.Parameters.Add("@TitleTypeID", SqlDbType.Int, 0, "TitleTypeID")
            da.InsertCommand.Parameters.Add("@OrigSellingPrice", SqlDbType.Money, 0, "OrigSellingPrice")
            da.InsertCommand.Parameters.Add("@InsurancePolicyID", SqlDbType.Int, 0, "InsurancePolicyID")
            da.InsertCommand.Parameters.Add("@SalesVolume", SqlDbType.Money, 0, "SalesVolume")
            da.InsertCommand.Parameters.Add("@SalesPrice", SqlDbType.Money, 0, "SalesPrice")
            da.InsertCommand.Parameters.Add("@CommVolume", SqlDbType.Money, 0, "CommVolume")
            da.InsertCommand.Parameters.Add("@Discount", SqlDbType.Money, 0, "Discount")
            da.InsertCommand.Parameters.Add("@UserAmount1", SqlDbType.Money, 0, "UserAmount1")
            da.InsertCommand.Parameters.Add("@UserAmount2", SqlDbType.Money, 0, "UserAmount2")
            da.InsertCommand.Parameters.Add("@UserAmount3", SqlDbType.Money, 0, "UserAmount3")
            da.InsertCommand.Parameters.Add("@TotalFinanced", SqlDbType.Money, 0, "TotalFinanced")
            da.InsertCommand.Parameters.Add("@CCTotal", SqlDbType.Money, 0, "CCTotal")
            da.InsertCommand.Parameters.Add("@CCFinanced", SqlDbType.Money, 0, "CCFinanced")
            da.InsertCommand.Parameters.Add("@DPTotal", SqlDbType.Money, 0, "DPTotal")
            da.InsertCommand.Parameters.Add("@DPBalance", SqlDbType.Money, 0, "DPBalance")
            da.InsertCommand.Parameters.Add("@InterestTypeID", SqlDbType.Int, 0, "InterestTypeID")
            da.InsertCommand.Parameters.Add("@PaymentTypeID", SqlDbType.Int, 0, "PaymentTypeID")
            da.InsertCommand.Parameters.Add("@PaymentFee", SqlDbType.Money, 0, "PaymentFee")
            da.InsertCommand.Parameters.Add("@APR", SqlDbType.Float, 0, "APR")
            da.InsertCommand.Parameters.Add("@Terms", SqlDbType.Int, 0, "Terms")
            da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.Int, 0, "FrequencyID")
            da.InsertCommand.Parameters.Add("@OrigDate", SqlDbType.SmallDateTime, 0, "OrigDate")
            da.InsertCommand.Parameters.Add("@FirstPaymentDate", SqlDbType.SmallDateTime, 0, "FirstPaymentDate")
            da.InsertCommand.Parameters.Add("@OneTimeFee", SqlDbType.Money, 0, "OneTimeFee")
            da.InsertCommand.Parameters.Add("@GracePeriod", SqlDbType.Int, 0, "GracePeriod")
            da.InsertCommand.Parameters.Add("@CalcAPR", SqlDbType.Float, 0, "CalcAPR")
            da.InsertCommand.Parameters.Add("@AutoPay", SqlDbType.Bit, 0, "AutoPay")
            da.InsertCommand.Parameters.Add("@CashOutDate", SqlDbType.SmallDateTime, 0, "CashOutDate")
            da.InsertCommand.Parameters.Add("@DPDueDate", SqlDbType.SmallDateTime, 0, "DPDueDate")
            da.InsertCommand.Parameters.Add("@DPInvoiceID", SqlDbType.Int, 0, "DPInvoiceID")
            da.InsertCommand.Parameters.Add("@CCInvoiceID", SqlDbType.Int, 0, "CCInvoiceID")
            Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@MortgageID", SqlDbType.Int, 0, "MortgageID")
            parameter.Direction = ParameterDirection.Output

            dr = ds.Tables("Mort").NewRow
        End If
        Update_Field("ContractID", _ContractID, dr)
        Update_Field("Number", _Number, dr)
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
        If _StatusDate = "" Then _StatusDate = System.DBNull.Value.ToString
        Update_Field("StatusDate", _StatusDate, dr)
        Update_Field("TitleTypeID", _TitleTypeID, dr)
        Update_Field("OrigSellingPrice", _OrigSellingPrice, dr)
        Update_Field("InsurancePolicyID", _InsurancePolicyID, dr)
        Update_Field("SalesVolume", _SalesVolume, dr)
        Update_Field("SalesPrice", _SalesPrice, dr)
        Update_Field("CommVolume", _CommVolume, dr)
        Update_Field("Discount", _Discount, dr)
        Update_Field("UserAmount1", _UserAmount1, dr)
        Update_Field("UserAmount2", _UserAmount2, dr)
        Update_Field("UserAmount3", _UserAmount3, dr)
        Update_Field("TotalFinanced", _TotalFinanced, dr)
        Update_Field("CCTotal", _CCTotal, dr)
        Update_Field("CCFinanced", _CCFinanced, dr)
        Update_Field("DPTotal", _DPTotal, dr)
        Update_Field("DPBalance", _DPBalance, dr)
        Update_Field("InterestTypeID", _InterestTypeID, dr)
        Update_Field("PaymentTypeID", _PaymentTypeID, dr)
        Update_Field("PaymentFee", _PaymentFee, dr)
        Update_Field("APR", _APR, dr)
        Update_Field("Terms", _Terms, dr)
        Update_Field("FrequencyID", _FrequencyID, dr)
        If _OrigDate = "" Then _OrigDate = System.DBNull.Value.ToString
        Update_Field("OrigDate", _OrigDate, dr)
        If _FirstPaymentDate = "" Then _FirstPaymentDate = System.DBNull.Value.ToString
        Update_Field("FirstPaymentDate", _FirstPaymentDate, dr)
        Update_Field("OneTimeFee", _OneTimeFee, dr)
        Update_Field("GracePeriod", _GracePeriod, dr)
        Update_Field("CalcAPR", _CalcAPR, dr)
        Update_Field("AutoPay", _AutoPay, dr)
        If _CashOutDate = "" Then _CashOutDate = System.DBNull.Value.ToString
        Update_Field("CashOutDate", _CashOutDate, dr)
        If _DPDueDate = "" Then _DPDueDate = System.DBNull.Value.ToString
        Update_Field("DPDueDate", _DPDueDate, dr)
        Update_Field("DPInvoiceID", _DPInvoiceID, dr)
        Update_Field("CCInvoiceID", _CCInvoiceID, dr)

        If ds.Tables("Mort").Rows.Count < 1 Then ds.Tables("Mort").Rows.Add(dr)
        da.Update(ds, "Mort")
        _ID = ds.Tables("Mort").Rows(0).Item("MortgageID")
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return True

        'Catch ex As Exception
        '_Err = ex.ToString
        'Return False
        'Finally

        'End Try
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
            oEvents.KeyField = "MortgageID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Private Sub ReCalc_All()

        If _OrigDate = "" Then    _OrigDate = DateTime.Today
        If _FirstPaymentDate = "" Then _FirstPaymentDate = DateTime.Today
        If _Terms = 0 Then _Terms = 84
        
        '      If CStr(request("FinCC")) = "1" Then
        '          If request("CCFinanced") = "" Then
        '              CCFinanced = 0
        '          Else
        '              CCFinanced = CDbl(request("CCFinanced"))
        '          End If
        '      Else
        '          CCFinanced = 0
        '      End If
        If _CCFinanced > 0 Then
            _TotalFinanced = Replace(Replace(FormatCurrency(_SalesPrice + _CCFinanced - _DPTotal, 2), "$", ""), ",", "")
        Else
            _TotalFinanced = Replace(Replace(FormatCurrency(_SalesPrice - _DPTotal, 2), "$", ""), ",", "")
        End If

        '      Update_CC_Defaults(SalesPrice, request("MortgageID"))
        '      PaymentProcessingFee = replace(replace(formatcurrency(PaymentFee), "$", ""), ",", "")
        '      ActualAmount = replace(replace(formatcurrency(CDbl(LoanPaymentAmount) + CDbl(PaymentProcessingFee)), "$", ""), ",", "")
        '      TotalPayments = replace(replace(formatcurrency(CDbl(LoanPaymentAmount) * Terms), "$", ""), ",", "")
        '      TotalFinanceCharges = replace(replace(formatcurrency(CDbl(TotalPayments) - CDbl(TotalFinanced)), "$", ""), ",", "")
        '      TotalFees = replace(replace(formatcurrency((CDbl(PaymentProcessingFee) * Terms) + CDbl(OneTimeFee)), "$", ""), ",", "")
        '      PaidToday = replace(replace(formatcurrency(DPTotal), "$", ""), ",", "")
        '      TotalCost = replace(replace(formatcurrency(CDbl(TotalFinanceCharges) + CDbl(TotalFinanced) + CDbl(TotalFees) + CDbl(PaidToday)), "$", ""), ",", "")
        '      response.write(TotalFinanced & "|" & LoanPaymentAmount & "|" & PaymentProcessingFee & "|" & ActualAmount & "|" & TotalFinanced & "|" & TotalFinanceCharges & "|" & TotalPayments & "|" & TotalFees & "|" & PaidToday & "|" & TotalCost)
    End Sub

    Public Function PMT() As Double
        Dim ans As Double
        Dim Rate As Double = _APR / 1200
        Dim mType As Integer = 0
        Dim PV As Double = _TotalFinanced
        ans = FormatCurrency(IIf(_TotalFinanced > 0, Math.Round((Rate * (mType + PV * (1 + Rate) ^ _Terms)) / ((1 - Rate * mType) * (1 - (1 + Rate) ^ _Terms)), 2), 0), 2) * -1
        Return ans
    End Function

    Public Function Calc_Amortization() As DataTable
        Dim i As Integer
        Dim tbl As DataTable = New DataTable
        Dim LastBalance As Double = _TotalFinanced
        Dim mtotalinterest As Double = 0
        Dim mtotalprinciple As Double = 0
        Dim minterestpayment As Double = 0
        Dim adjustedamount As Double = 0
        Dim Rate As Double = _APR / 1200
        Dim iCols As Integer = 6
        Dim Payment As Double = PMT()
        Dim tr As DataRow
        tbl.Columns.Add("Payment Number")
        tbl.Columns.Add("Beginning Balance")
        tbl.Columns.Add("Payment")
        tbl.Columns.Add("Proc. Fee")
        tbl.Columns.Add("Interest")
        tbl.Columns.Add("Principle")
        tbl.Columns.Add("Ending Balance")

        For i = 1 To _Terms
            minterestpayment = FormatCurrency(LastBalance * Rate)
            If LastBalance - (Payment - minterestpayment) < 1 Then
                adjustedamount = LastBalance - (Payment - minterestpayment)
                minterestpayment = minterestpayment - adjustedamount
            End If
            mtotalinterest += minterestpayment
            mtotalprinciple = (mtotalprinciple + (Payment - (minterestpayment)))
            tr = tbl.NewRow
            tr("Payment Number") = i
            tr("Beginning Balance") = Replace(FormatCurrency(LastBalance, 2), "$", "")
            tr("Payment") = Replace(FormatCurrency(Payment, 2), "$", "")
            tr("Proc. Fee") = Replace(FormatCurrency(_PaymentFee, 2), "$", "")
            tr("Interest") = Replace(FormatCurrency(minterestpayment, 2), "$", "")
            tr("Principle") = Replace(FormatCurrency(Payment - minterestpayment, 2), "$", "")
            LastBalance = LastBalance - Payment + minterestpayment
            If LastBalance < 0.0 Then
                LastBalance = 0
            Else
                If Mid(CStr(LastBalance), InStr(CStr(LastBalance), ".") + 3, 1) = "5" Then
                    LastBalance = Math.Round(LastBalance, 2) + 0.01
                Else
                    LastBalance = Math.Round(LastBalance, 2)
                End If
            End If
            tr("Ending Balance") = Replace(FormatCurrency(LastBalance, 2), "$", "")
            tbl.Rows.Add(tr)
        Next
        tr = tbl.NewRow
        tr("Payment Number") = 0
        tr("Beginning Balance") = 0
        tr("Payment") = Replace(FormatCurrency(Payment * _Terms, 2), "$", "")
        tr("Proc. Fee") = Replace(FormatCurrency(_PaymentFee * _Terms, 2), "$", "")
        tr("Interest") = Replace(FormatCurrency(mtotalinterest, 2), "$", "")
        tr("Principle") = Replace(FormatCurrency(mtotalprinciple, 2), "$", "")
        tbl.Rows.Add(tr)
        If adjustedamount <> 0 Then
            tr = tbl.NewRow
            tr("Payment Number") = "ADJ:"
            tr("Beginning Balance") = Replace(FormatCurrency(adjustedamount, 2), "$", "")
            tr("Payment") = 0
            tr("Proc. Fee") = 0
            tr("Interest") = 0
            tr("Principle") = 0
            tbl.Rows.Add(tr)
        End If

        Return tbl

    End Function

    Public Function IPMT(ByVal Rate As Double, ByVal Paymentnumber As Integer, ByVal PV As Double, ByVal payment As Double, ByVal totalinterest As Double, ByVal totalprinciple As Double) As Double
        Dim balance As Double = PV
        Dim interestpayment As Double = 0

        For x = 1 To Paymentnumber
            interestpayment = balance * Rate 'round((balance*Rate),2)

            balance = Math.Round(balance - payment + interestpayment, 2)
        Next
        totalprinciple = Math.Round(totalprinciple + payment - interestpayment, 2)
        totalinterest = totalinterest + interestpayment
        Return interestpayment
    End Function


    Public Function Create_CC_Defaults(ByVal MortID As Integer, ByVal SP As Double, ByVal DPTotal As Double) As Double
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        Dim tmpSP As Double = 0
        Dim CCTotal As Double = 0
        cm.CommandText = "Select * from t_ClosingCostDefaults where active = 1"
        da.Fill(ds, "0")

        If ds.Tables("0").Rows.Count > 0 Then
            For i = 0 To ds.Tables("0").Rows.Count - 1
                Dim oCCDefaults As New clsMortgageClosingCosts

                With ds.Tables("0").Rows(i)
                    oCCDefaults.MortgageID = MortID
                    oCCDefaults.FieldID = .Item("FieldID")
                    If .Item("IsFixed") Then
                        If .Item("Optional") Then
                            oCCDefaults.Amount = .Item("FixedValue")
                        Else
                            oCCDefaults.Amount = .Item("FixedValue")
                        End If
                    ElseIf .Item("isCalc") Then
                        oCCDefaults.Amount = 0
                    ElseIf .Item("isLookUp") Then
                        If InStr(UCase(.Item("LookupTableName")), "DOT") > 0 Then
                            tmpSP = (SP + Get_Fixed_Default_Total(MortID) + Get_Lookup_Value("v_RecordingFeesDeed", "", SP)) - DPTotal + Get_Lookup_Value(.Item("LookupTableName"), "", tmpSP) ' * 0.9
                        Else
                            tmpSP = SP
                        End If
                        oCCDefaults.Amount = Get_Lookup_Value(.Item("LookupTableName"), "", tmpSP)
                    Else
                        oCCDefaults.Amount = 0
                    End If
                    oCCDefaults.DateCreated = Date.Now
                    oCCDefaults.Save()
                    CCTotal += oCCDefaults.Amount
                End With
                oCCDefaults = Nothing
            Next
        End If
        ds.Tables("0").Clear()

        cm.CommandText = "Select sum(amount) as Total from t_MortgageClosingCosts where mortgageid=" & MortID
        'da.SelectCommand = cm
        da.Fill(ds, "0")
        Return ds.Tables("0").Rows(0)("Total")
        ds.Tables("0").Clear()
        Return True

        'Catch ex As Exception
        '_Err = ex.ToString
        'Return -1
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try

    End Function

    Public Function Update_CC_Defaults(ByVal SP As Double, ByVal MortID As Integer, ByVal DPTotal As Double) As Double
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        Dim DPA As Double = 0
        Dim tmpSP As Double = 0
        Dim CCTotal As Double = 0
        cm.CommandText = "Select d.*, m.MortgageClosingCostID from t_ClosingCostDefaults d inner join t_MortgageClosingCosts m on m.fieldid=d.fieldid where m.mortgageid = " & MortID
        da.Fill(ds, "0")
        If ds.Tables("0").Rows.Count > 0 Then
            For i = 0 To ds.Tables("0").Rows.Count - 1
                With ds.Tables("0").Rows(i)
                    Dim oMortCC As New clsMortgageClosingCosts
                    oMortCC.MortgageClosingCostID = .Item("MortgageClosingCostID")
                    oMortCC.Load()
                    If .Item("IsFixed") Then
                        If .Item("Optional") Then
                            oMortCC.Amount = oMortCC.Amount
                        Else
                            oMortCC.Amount = .Item("FixedValue")
                        End If
                    ElseIf .Item("isCalc") Then
                        oMortCC.Amount = 0
                    ElseIf .Item("isLookUp") Then
                        If InStr(UCase(.Item("LookupTableName")), "DOT") > 0 Then
                            If DPTotal <> SP Then
                                tmpSP = (SP + Get_Fixed_Default_Total(MortID) + Get_Lookup_Value("v_RecordingFeesDeed", "", SP)) - DPTotal
                                Dim chkSP As Double = 0
                                Do While chkSP <> tmpSP
                                    chkSP = Math.Round(tmpSP, 2)
                                    tmpSP = (SP + Get_Fixed_Default_Total(MortID) + Get_Lookup_Value("v_RecordingFeesDeed", "", SP)) - DPTotal + Get_Lookup_Value(.Item("LookupTableName"), "", tmpSP) '* 0.9
                                    tmpSP = Math.Round(tmpSP, 2)
                                Loop
                            Else
                                tmpSP = 0
                            End If
                        Else
                            tmpSP = SP
                        End If
                        oMortCC.Amount = Get_Lookup_Value(.Item("LookupTableName"), "", tmpSP)
                    Else
                        oMortCC.Amount = 0
                    End If
                    oMortCC.Save()

                    oMortCC = Nothing
                End With
            Next
        End If

        ds.Tables("0").Clear()
        cm.CommandText = "Select sum(amount) as Total from t_MortgageClosingCosts where mortgageid=" & MortID
        da.Fill(ds, "0")
        Return ds.Tables("0").Rows(0)("Total")
        ds.Tables("0").Clear()

        'Return True
        'Catch ex As Exception
        '_Err = ex.ToString
        'Return -1
        'Return False
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try

    End Function

    Function Get_Fixed_Default_Total(ByVal MortID As Integer) As Double
        cm.CommandText = "Select sum(amount) as amount from t_MortgageClosingCosts m inner join t_ClosingCostDefaults c on c.fieldid = m.fieldid where mortgageid = " & MortID & " and isfixed = 1 and (m.amount > 0 or optional is null or optional = 0)"
        'da.SelectCommand = cm
        da.Fill(ds, "Total")
        Dim dAns As Double = 0
        If ds.Tables("Total").Rows.Count > 0 Then
            If ds.Tables("Total").Rows(0)("Amount") Is System.DBNull.Value Then
                dAns = 0
            Else
                dAns = ds.Tables("Total").Rows(0)("Amount")
            End If
        End If
        ds.Tables("Total").Clear()
        'da.Dispose()
        Return dAns
    End Function

    Function Get_Lookup_Value(ByVal TableName As String, ByVal DataType As String, ByVal CompareValue As Double) As Double
        Dim dAns As Double = 0
        cm.CommandText = "Select * from " & TableName & " order by Lookup"
        'da.SelectCommand = cm
        da.Fill(ds, "Lookup")
        Dim i As Integer = 0
        For i = 0 To ds.Tables("Lookup").Rows.Count - 1
            dr = ds.Tables("Lookup").Rows(i)
            If CompareValue < CDbl(dr("Lookup")) Then
                If i > 0 Then
                    dr = ds.Tables("Lookup").Rows(i - 1)
                    If CompareValue > CDbl(dr("Lookup")) Then
                        dr = ds.Tables("Lookup").Rows(i)
                    End If
                    dAns = dr("LookupValue")
                End If
                Exit For
            End If
        Next
        ds.Tables("Lookup").Clear()
        'da.Dispose()
        Return dAns

    End Function

    Function Update_Total_Financed() As Double
        Dim dAns As Double = 0
        _TotalFinanced = _SalesPrice + _CCFinanced - _DPTotal

        Return _TotalFinanced
    End Function

    Function Get_Mortgage_ID(ByVal conID As Integer) As Integer
        Dim mortID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select mortgageid from t_mortgage where contractid = '" & conID & "'"
            dread = cm.ExecuteReader()
            dread.Read()
            mortID = dread("MortgageID")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return mortID
    End Function

    Function Delinquent_Check(ByVal prosID As Integer) As Boolean
        Dim bDel As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Mortgages from t_Mortgage m inner join t_Contract c on m.ContractID = c.ContractID inner join t_ComboItems ms on m.StatusID = ms.ComboItemID where ms.ComboItem = 'Delinquent' and c.ProspectID = " & prosID
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Mortgages") > 0 Then
                bDel = True
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bDel
    End Function

    Public Function Get_DP_Total(ByVal mortID As Integer) As Double
        Dim dpTotal As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select i.Amount as DPTotal from v_Invoices i inner join t_mortgage m on i.ID = m.DPInvoiceID where m.Mortgageid = '" & mortID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                dpTotal = dread("DPTotal")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dpTotal
    End Function

    Public Function Get_CC_Total(ByVal mortID As Integer) As Double
        Dim ccTotal As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select i.Amount as CCTotal from v_Invoices i inner join t_mortgage m on i.ID = m.CCInvoiceID where m.Mortgageid = '" & mortID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ccTotal = dread("CCTotal")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ccTotal
    End Function

    Public Function Get_DP_Received(ByVal mortID As Integer) As Double
        Dim dpReceived As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(Amount + Adjustment) * -1 as DPReceived from v_Payments where Invoice like 'Down Payment%' and KeyField = 'MortgageDP' and KeyValue = '" & mortID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                dpReceived = dread("DPReceived")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dpReceived
    End Function

    Public Function Get_DP_Scheduled(ByVal mortID As Integer) As Double
        Dim dpScheduled As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(p.Amount) as DPScheduled from t_PaymentsScheduled p inner join t_PaymentSched2Invoice pi on p.SchedPayID = pi.SchedPaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.COmboItemID where Upper(i.KeyField) = 'MORTGAGEDP' and i.KeyValue = '" & mortID & "' and tc.ComboItem LIKE '%Down Payment%' and p.Processed = '0' and p.Cancelled = '0'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                dpScheduled = dread("DPScheduled")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dpScheduled
    End Function

    Public Function Get_CC_Financed_Amt(ByVal mortID As Integer) As Double
        Dim CCFinanced As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select i.balance as CCFinanced from t_Mortgage m inner join v_Invoices i on i.id = m.ccinvoiceid where m.MortgageID = '" & mortID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                CCFinanced = dread("CCFinanced")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return CCFinanced
    End Function

#Region "Properties"

    Public Property DPInvoiceID As Integer
        Get
            Return _DPInvoiceID
        End Get
        Set(ByVal value As Integer)
            _DPInvoiceID = value
        End Set
    End Property

    Public Property CCInvoiceID As Integer
        Get
            Return _CCInvoiceID
        End Get
        Set(ByVal value As Integer)
            _CCInvoiceID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
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

    Public Property Number() As String
        Get
            Return _Number
        End Get
        Set(ByVal value As String)
            _Number = Left(value, 50)
        End Set
    End Property

    Public Property IgnoreStatusDate As Boolean
        Get
            Return _IgnoreStatusDate
        End Get
        Set(value As Boolean)
            _IgnoreStatusDate = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            If value <> _StatusID And Not (_IgnoreStatusDate) Then
                _StatusDate = System.DateTime.Now.ToShortDateString
            End If
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

    Public Property TitleTypeID() As Integer
        Get
            Return _TitleTypeID
        End Get
        Set(ByVal value As Integer)
            _TitleTypeID = value
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

    Public Property InsurancePolicyID() As Integer
        Get
            Return _InsurancePolicyID
        End Get
        Set(ByVal value As Integer)
            _InsurancePolicyID = value
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

    Public Property CommVolume() As Decimal
        Get
            Return _CommVolume
        End Get
        Set(ByVal value As Decimal)
            _CommVolume = value
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

    Public Property UserAmount1() As Decimal
        Get
            Return _UserAmount1
        End Get
        Set(ByVal value As Decimal)
            _UserAmount1 = value
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

    Public Property CCTotal() As Decimal
        Get
            Return _CCTotal
        End Get
        Set(ByVal value As Decimal)
            _CCTotal = value
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

    Public Property DPTotal() As Decimal
        Get
            Return _DPTotal
        End Get
        Set(ByVal value As Decimal)
            _DPTotal = value
        End Set
    End Property

    Public Property DPBalance() As Decimal
        Get
            Return _DPBalance
        End Get
        Set(ByVal value As Decimal)
            _DPBalance = value
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

    Public Property OrigDate() As String
        Get
            Return _OrigDate
        End Get
        Set(ByVal value As String)
            _OrigDate = value
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

    Public Property GracePeriod() As Integer
        Get
            Return _GracePeriod
        End Get
        Set(ByVal value As Integer)
            _GracePeriod = value
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

    Public Property DPDueDate() As String
        Get
            Return _DPDueDate
        End Get
        Set(ByVal value As String)
            _DPDueDate = value
        End Set
    End Property

    Public Property MortgageID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

#End Region
End Class
