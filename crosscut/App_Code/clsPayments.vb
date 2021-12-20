Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPayments
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _MethodID As Integer = 0
    Dim _Adjustment As Boolean = False
    Dim _PosNeg As Boolean = False
    Dim _Amount As Decimal = 0
    Dim _ApplyToID As Integer = 0
    Dim _TransDate As String = ""
    Dim _Description As String = ""
    Dim _Reference As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Payments where PaymentID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Payments where PaymentID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Payments")
            If ds.Tables("t_Payments").Rows.Count > 0 Then
                dr = ds.Tables("t_Payments").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("MethodID") Is System.DBNull.Value) Then _MethodID = dr("MethodID")
        If Not (dr("Adjustment") Is System.DBNull.Value) Then _Adjustment = dr("Adjustment")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("ApplyToID") Is System.DBNull.Value) Then _ApplyToID = dr("ApplyToID")
        If Not (dr("UserID") Is System.DBNull.Value) Then _UserID = dr("UserID")
        If Not (dr("TransDate") Is System.DBNull.Value) Then _TransDate = dr("TransDate")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("Reference") Is System.DBNull.Value) Then _Reference = dr("Reference")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Payments where PaymentID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Payments")
            If ds.Tables("t_Payments").Rows.Count > 0 Then
                dr = ds.Tables("t_Payments").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PaymentsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@MethodID", SqlDbType.int, 0, "MethodID")
                da.InsertCommand.Parameters.Add("@Adjustment", SqlDbType.bit, 0, "Adjustment")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.bit, 0, "PosNeg")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@ApplyToID", SqlDbType.int, 0, "ApplyToID")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.int, 0, "UserID")
                da.InsertCommand.Parameters.Add("@TransDate", SqlDbType.DateTime, 0, "TransDate")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@Reference", SqlDbType.VarChar, 0, "Reference")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PaymentID", SqlDbType.Int, 0, "PaymentID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Payments").NewRow
            End If
            Update_Field("MethodID", _MethodID, dr)
            Update_Field("Adjustment", _Adjustment, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("ApplyToID", _ApplyToID, dr)
            Update_Field("UserID", _UserID, dr)
            Update_Field("TransDate", _TransDate, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("Reference", _Reference, dr)
            If ds.Tables("t_Payments").Rows.Count < 1 Then ds.Tables("t_Payments").Rows.Add(dr)
            da.Update(ds, "t_Payments")
            _ID = ds.Tables("t_Payments").Rows(0).Item("PaymentID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function List_Payments() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If _KeyField = "ProspectID" Then
            ds.SelectCommand = "select p.PaymentID as ID, m.comboitem as Method, p.TransDate, p.Amount from t_Payments p left outer join t_Comboitems m on m.comboitemid = p.methodid where p.paymentid in (select PaymentID from t_Payment2Invoice pi inner join t_Invoices i on i.invoiceid = pi.invoiceid where i.prospectid = " & _ProspectID & " and i.keyfield= '" & _KeyField & "' and (i.keyvalue = " & _KeyValue & " or i.keyvalue = " & _ProspectID & ")) order by PaymentID"
        Else
            ds.SelectCommand = "select p.PaymentID as ID, m.comboitem as Method, p.TransDate, p.Amount from t_Payments p left outer join t_Comboitems m on m.comboitemid = p.methodid where p.paymentid in (select PaymentID from t_Payment2Invoice pi inner join t_Invoices i on i.invoiceid = pi.invoiceid where i.prospectid = " & _ProspectID & " and i.keyfield= '" & _KeyField & "' and i.keyvalue = " & _KeyValue & ") order by PaymentID"
        End If
        Return ds
    End Function

    Public Function List_Checks() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "select p.PaymentID as ID, m.comboitem as Method, p.TransDate, p.Amount from t_Payments p left outer join t_Comboitems m on m.comboitemid = p.methodid where p.paymentid in (select PaymentID from t_Payment2Invoice pi inner join t_Invoices i on i.invoiceid = pi.invoiceid where i.prospectid = " & _ProspectID & " and i.keyfield= '" & _KeyField & "' and i.keyvalue = " & _KeyValue & ") and (m.ComboItem = 'Check' or m.ComboItem = 'ACH Payment' or m.ComboItem Like 'Concord%' or m.ComboItem = 'Aspen' or m.ComboItem = 'Meridian') order by PaymentID"
        Return ds
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
            oEvents.KeyField = "PaymentID"
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

    Public Function List_Cash_Payments(ByVal keyField As String, ByVal KeyValue As Integer) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If keyField = "ProspectID" Then
                ds.SelectCommand = "Select p.PaymentID, i.InvoiceID, tc.ComboItem as Invoice, p.TransDate, p.Amount from t_payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboitemID where pm.ComboItem = 'Cash' and i.KeyField = 'ContractID' and i.ProspectID = '" & KeyValue & "' and i.KeyValue > 0"
            Else
                ds.SelectCommand = "Select p.PaymentID, i.InvoiceID, tc.ComboItem as Invoice, p.TransDate, p.Amount from t_payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboitemID where pm.ComboItem = 'Cash' and i.KeyField = '" & keyField & "' and i.KeyValue = " & KeyValue & ""
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Check_Refund_Payments(ByVal keyField As String, ByVal KeyValue As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If keyField = "ProspectID" Then
                ds.SelectCommand = "Select p.PaymentID, i.InvoiceID, tc.ComboItem as Invoice, p.TransDate, p.Amount, f.MerchantAccountID, pm.ComboItem as Method from t_payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboitemID where pm.ComboItem <> 'Payment Move' and p.Adjustment = 0 and p.ApplyToID = 0 and ((i.KeyField = 'ProspectID' and i.Keyvalue = '" & KeyValue & "') or (i.Keyfield = 'ContractID' and i.ProspectID = '" & KeyValue & "' and i.KeyValue > 0)) and p.PosNeg = 1"
            Else
                ds.SelectCommand = "Select p.PaymentID, i.InvoiceID, tc.ComboItem as Invoice, p.TransDate, p.Amount, f.MerchantAccountID, pm.ComboItem as Method from t_payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboitemID where pm.ComboItem <> 'Payment Move' and p.Adjustment = 0 and p.ApplyToID = 0 and i.KeyField = '" & keyField & "' and i.KeyValue = " & KeyValue & " and p.posneg = 1"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Transferable_Payments(ByVal keyField As String, ByVal KeyValue As Integer) As SQLDataSource
        Dim ds As New sqldataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If keyField = "ProspectID" Then
                ds.SelectCommand = "Select p.PaymentID, pm.ComboItem as PaymentMethod, tc.ComboItem as Invoice, p.TransDate, p.Amount, u.UserName, i.InvoiceID from t_payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboitemID left outer join t_Personnel u on p.UserID = u.PersonnelID where p.Adjustment = '0' and p.PosNeg = '1' and i.KeyField = '" & keyField & "' and i.KeyValue = " & KeyValue & "" 'ContractID' and i.ProspectID = '" & KeyValue & "' and i.KeyValue > 0"
            Else
                ds.SelectCommand = "Select p.PaymentID, pm.ComboItem as PaymentMethod, tc.ComboItem as Invoice, p.TransDate, p.Amount, u.UserName, i.InvoiceID from t_payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.Fintransid = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboitemID left outer join t_Personnel u on p.UserID = u.PersonnelID where p.Adjustment = '0' and p.PosNeg = '1' and i.KeyField = '" & keyField & "' and i.KeyValue = " & KeyValue & ""
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Moved_Amount(ByVal paymentID As Integer) As Double
        Dim movedAmt As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Sum(p.Amount) is null then 0 else Sum(p.Amount) end as MovedAmt from t_Payments p inner join t_ComboItems pm on p.MethodID = pm.ComboItemID where p.PosNeg = '0' and p.ApplyToID = " & paymentID & " AND pm.Comboitem = 'Payment Move'"
            dread = cm.ExecuteReader
            dread.Read()
            movedAmt = dread("MovedAmt")
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return movedAmt
    End Function

    Public Function Print_Receipt(ByVal pmts As String, ByVal sched As String) As String
        Dim recpt As String = ""
        Dim payments() As String = pmts.Split(",")
        Dim oCon As New clsContract
        Dim oMort As New clsMortgage
        Dim oConv As New clsConversion
        Dim oPros As New clsProspect
        Dim oAddress As New clsAddress
        Dim oPayment As New clsPayments
        Dim keyField As String = ""
        Dim keyValue As String = ""
        Dim conNumber As String = ""
        Dim prosID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            'Tied to a Company?
            If sched = "" Then
                cm.CommandText = "Select Case when cct.Approved = -1 then 0 else cc.CreditCardID end as CreditCardID, p.Reference, p.TransDate, i.KeyField, i.KeyValue, i.ProspectID, pm.ComboItem as PaymentMethod, ma.AccountName, ma.MerchantNumber, cct.CCTransID, cct.DateImported, cct.ICVResponse, ccb.Batch, cc.CreditCardID, cc.Number, cc.Expiration, cc.NameOnCard, ct.Comboitem as CardType, cctt.ComboItem as TransType, cct.Amount as CCTransAmt, i.Reference as InvRef from t_Payments p inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FinTransID inner join t_CCMerchantAccount ma on f.MerchantAccountID = ma.AccountID left outer join t_ComboItems pm on p.methodid = pm.ComboItemID left outer join t_CCTransApplyTo cca on p.PaymentID = cca.PaymentID left outer join t_CCTrans cct on cca.CCTransID = cct.CCTransID left outer join t_CreditCard cc on cct.CreditCardID = cc.CreditCardID left outer join t_CCBatch ccb on cct.BatchID = ccb.BatchID left outer join t_ComboItems ct on cc.TypeID = ct.CombOitemID left outer join t_ComboItems cctt on cct.TransTypeID = cctt.ComboItemID where p.PaymentID = '" & payments(0) & "' and (cct.Imported is null or cct.Imported = 1 or cct.Approved = -1)"
            Else
                cm.CommandText = "Select i.KeyField, i.KeyValue, i.ProspectID, p.reference, p.SchedDate, p.CreditCardID, cc.Number, cc.NameOnCard, ma.MerchantNumber, cct.ComboItem As CardType, pm.ComboItem as PaymentMethod,'' as InvRef from t_PaymentsScheduled p inner join t_PaymentSched2Invoice pi on p.SchedPayID = pi.SchedPaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_ComboItems pm on p.MethodID = pm.ComboItemID inner join t_FinTransCodes f on i.FinTransID = f.FinTransID inner join t_CCMerchantAccount ma on f.MerchantAccountID = ma.AccountID left outer join t_CreditCard cc on p.CreditCardID = cc.CreditCardID left outer join t_ComboItems cct on cc.TypeID = cct.ComboItemID where p.SchedPayID = '" & payments(0) & "'"
            End If
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                recpt = "<table>"
                dread.Read()
                keyField = UCase(dread("KeyField"))
                keyValue = UCase(dread("KeyValue"))
                prosID = dread("ProspectID")
                If UCase(dread("KeyField")) = "CONTRACTID" Then
                    oCon.ContractID = dread("KeyValue")
                    oCon.Load()
                    oCon.ContractNumber = oCon.ContractNumber
                    If oCon.CompanyName & "" <> "" Then
                        recpt = recpt & "<tr><td>" & oCon.CompanyName & "</td></tr>"
                    End If
                ElseIf UCase(dread("KeyField")) = "MORTGAGEDP" Then
                    oMort.MortgageID = dread("KeyValue")
                    oMort.Load()
                    oCon.ContractID = oMort.ContractID
                    oCon.Load()
                    conNumber = oCon.ContractNumber
                    If oCon.CompanyName & "" <> "" Then
                        recpt = recpt & "<tr><td>" & oCon.CompanyName & "</td></tr>"
                    End If
                ElseIf UCase(dread("KeyField")) = "CONVERSIONDP" Then
                    oConv.ConversionID = keyValue
                    oConv.Load()
                    oCon.ContractID = oConv.ContractID
                    oCon.Load()
                    conNumber = oCon.ContractNumber
                    If oCon.CompanyName & "" <> "" Then
                        recpt = recpt & "<tr><td>" & oCon.CompanyName & "</td></tr>"
                    End If
                End If
                oPros.Prospect_ID = prosID
                oPros.Load()
                recpt = recpt & "<tr><td>" & oPros.First_Name & " " & oPros.Last_Name & "</td></tr>"
                recpt = recpt & oAddress.Get_Receipt_Address(oPros.Prospect_ID)
                recpt = recpt & "<tr><td>" & dread("InvRef") & "</td></tr>"
                recpt = recpt & "</table>"
                If dread("CreditCardID") & "" = "" Or dread("CreditCardID") & "" = "0" Then
                    If sched <> "" Then
                        recpt = recpt & "<br>Payment Will Be Received On: " & dread("SchedDate")
                    Else
                        recpt = recpt & "<br>" & dread("TransDate")
                    End If
                    If InStr(dread("PaymentMethod"), "Refund") <> 0 Then
                        recpt = recpt & "<br>Refund<br>"
                    Else
                        recpt = recpt & "<br>" & dread("Reference") & "<br>"
                    End If
                    recpt = recpt & "<table>"
                    If keyField = "MORTGAGEDP" Or keyField = "CONVERSIONDP" Or keyField = "CONTRACTID" Then
                        For i = 0 To UBound(payments)
                            recpt = recpt & oPayment.List_Rcpt_Items(payments(i), sched, conNumber)
                        Next i
                    Else
                        For i = 0 To UBound(payments)
                            recpt = recpt & oPayment.List_Rcpt_Items(payments(i), sched, conNumber)
                        Next i
                    End If
                    recpt = recpt & "</table>"
                Else
                    If sched <> "" Then
                        recpt = recpt & "<br>Payment Will Be Received On: " & dread("SchedDate") & "<br>"
                        recpt = recpt & "MER# :" & dread("MerchantNumber")
                        recpt = recpt & "<br>TER#: 0001<br>"
                        recpt = recpt & "S-A-L-E-S D-R-A-F-T<br>"
                        recpt = recpt & "REF: "
                        recpt = recpt & "<br>BATCH:<br>"
                        recpt = recpt & "<CD Type: " & dread("CardType") & "<br>"
                        recpt = recpt & "TR Type: CHARGE<br>"
                    Else
                        recpt = recpt & "<br>" & dread("DateImported") & "<br>"
                        recpt = recpt & "MER#: " & dread("MerchantNumber")
                        recpt = recpt & "<br>TER#: 0001<br>"
                        recpt = recpt & "S-A-L-E-S D-R-A-F-T<br>"
                        recpt = recpt & "REF: "
                        If Len(dread("ICVResponse")) <= 7 Then
                        Else
                            recpt = recpt & Right(dread("ICVResponse") & "", Len(dread("ICVresponse") & "") - 7)
                        End If
                        recpt = recpt & "<br>BATCH: " & dread("Batch") & "<br>"
                        recpt = recpt & "CD Type: " & dread("CardType") & "<br>"
                        recpt = recpt & "TR Type: " & dread("TransType") & "<br>"
                    End If
                    recpt = recpt & "<table>"
                    Dim skipLast As Boolean = False
                    If sched = "" Then
                        skipLast = True
                    End If
                    If keyField = "MORTGAGEID" Or keyField = "CONVERSIONDP" Or keyField = "CONTRACTID" Then
                        For i = 0 To UBound(payments)
                            recpt = recpt & oPayment.List_Rcpt_Items(payments(i), sched, conNumber, skipLast)
                        Next i
                    Else
                        For i = 0 To UBound(payments)
                            recpt = recpt & oPayment.List_Rcpt_Items(payments(i), sched, "", skipLast)
                        Next i
                    End If
                    If skipLast Then
                        recpt = recpt & "<tr><td>" & FormatCurrency(dread("CCTransAmt"), 2) & "</td></tr>"
                    End If
                    recpt = recpt & "</table><br>"
                    recpt = recpt & "ACCT: " & dread("Number") & "<br>"
                    recpt = recpt & "EXP: ****<br>"
                    If sched = "" Then
                        recpt = recpt & "AP: " & Right(Left(dread("ICVResponse"), 7), 6) & "<br>"
                    Else
                        recpt = recpt & "AP: <br>"
                    End If
                    recpt = recpt & "Name: " & dread("NameOnCard") & "<br>"
                    recpt = recpt & "CARDMEMBER ACKNOWLEDGES RECEIPT OF GOODS AND/OR SERVICES IN THE AMOUNT OF THE TOTAL SHOWN HEREON AND AGREES TO PERFORM THE OBLIGATIONS SET FORTH BY THE CARDMEMBER'S AGREEMENT WITH THE ISSUER <br>"
                    recpt = recpt & "<br><br>"
                    recpt = recpt & "Signature: ______________________________________<BR>"
                    recpt = recpt & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & dread("NameOnCard")
                End If
            Else
                recpt = recpt & "<tr><td>NO ROWS</td></tr>"
            End If
            dread.Close()
            oCon = Nothing
            oConv = Nothing
            oMort = Nothing
            oPros = Nothing
        Catch ex As Exception
            _Err = ex.Message
	    recpt = recpt & _Err
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return recpt & "</table>"
    End Function

    Public Function List_Rcpt_Items(ByVal paymentID As Integer, ByVal sched As String, Optional ByVal conNumber As String = "", Optional ByVal skipTotal As Boolean = False) As String
        Dim items As String = ""
        Dim amt As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If sched <> "" Then
                cm.CommandText = "Select p.Amount, tc.CombOitem as transcode from t_paymentsScheduled p inner join t_PaymentSched2Invoice pi on pi.SchedPaymentID = p.SchedPayID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.CombOitemID where p.SchedpayID = '" & paymentID & "'"
            Else
                cm.CommandText = "Select p.Amount, tc.CombOitem as transcode from t_payments p inner join t_Payment2Invoice pi on pi.PaymentID = p.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.CombOitemID where p.paymentID = '" & paymentID & "'"
            End If
            dread = cm.ExecuteReader()
            If dread.HasRows Then
                Do While dread.Read
                    If conNumber = "" Then
                        items = items & "<tr><td>" & dread("transcode") & "</td><td>" & FormatCurrency(dread("Amount"), 2) & "</td></tr>"
                    Else
                        items = items & "<tr><td>KCP #:" & conNumber & "</td><td>" & dread("transcode") & "</td><td>" & FormatCurrency(dread("Amount"), 2) & "</td></tr>"
                    End If
                    amt = amt + dread("Amount")
                Loop
                If Not (skipTotal) Then
                    items = items & "<tr><td>Total:</td><td>" & FormatCurrency(amt, 2) & "</td></tr>"
                End If
            End If
            dread.Close()
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return items
    End Function

    Public Function Get_Merchant_Acct(ByVal pmtid As Integer, ByVal sched As String) As String
        Dim mAcct As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If sched <> "" Then
                cm.CommandText = "Select ma.AccountName from t_PaymentsScheduled p inner join t_PaymentSched2Invoice pi on pi.PaymentID = p.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FintransID inner join t_CCMerchantAccount ma on f.MerchantAccountID = ma.AccountID where p.SchedPayID = '" & pmtid & "'"
            Else
                cm.CommandText = "Select ma.AccountName from t_Payments p inner join t_Payment2Invoice pi on pi.PaymentID = p.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FinTransID = f.FintransID inner join t_CCMerchantAccount ma on f.MerchantAccountID = ma.AccountID where p.PaymentID = '" & pmtid & "'"
            End If
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                mAcct = dread("Accountname")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return mAcct
    End Function

    Public Property ProspectID As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property KeyField As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property KeyValue As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Property MethodID() As Integer
        Get
            Return _MethodID
        End Get
        Set(ByVal value As Integer)
            _MethodID = value
        End Set
    End Property

    Public Property Adjustment() As Boolean
        Get
            Return _Adjustment
        End Get
        Set(ByVal value As Boolean)
            _Adjustment = value
        End Set
    End Property

    Public Property PosNeg() As Boolean
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Boolean)
            _PosNeg = value
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

    Public Property ApplyToID() As Integer
        Get
            Return _ApplyToID
        End Get
        Set(ByVal value As Integer)
            _ApplyToID = value
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

    Public Property TransDate() As String
        Get
            Return _TransDate
        End Get
        Set(ByVal value As String)
            _TransDate = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property
    Public Property Reference() As String
        Get
            Return _Reference
        End Get
        Set(ByVal value As String)
            _Reference = value
        End Set
    End Property
    Public Property PaymentID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
