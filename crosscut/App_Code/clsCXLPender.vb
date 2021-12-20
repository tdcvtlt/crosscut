Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCXLPender



    ''' <summary>
    ''' Get amount paid todate, contract no. etc. whose status changed from pender to cancelled pender within date range
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <remarks></remarks>
    Public Shared Function GetCXLPenderAmountToDate(ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of clsPersonnalFinanancialCxlPender)

        Dim cxlPenderList As List(Of clsPersonnalFinanancialCxlPender) = GetCXLPenderInDates(startDate, endDate)

        If cxlPenderList Is Nothing Then Return Nothing

        'If cxlPenderList.Count = 0 Then Return Nothing


        Dim del As ConvertToStringDelimited = New ConvertToStringDelimited(AddressOf ToCommaDelimited)
        Dim separated = del.Invoke(cxlPenderList.Select(Function(x) x.ContractID.ToString()).ToArray())

        Dim sqlText As String = String.Format("select * from t_mortgage where contractid in ({0})", separated)

        Dim conx As String = "data source=RS-SQL-01;initial catalog=CRMSNET;user=asp;password=aspnet;" & _
                     "persist security info=False;packet size=4096;"






        'cnn = New SqlConnection(Resources.Resource.cns)
        cnn = New SqlConnection(conx)
        cmd = New SqlCommand(sqlText, cnn)

        cnn.Open()

        Dim rdr As SqlDataReader = cmd.ExecuteReader()
        Dim ListOfFinancials As List(Of clsFinancialAmount) = New List(Of clsFinancialAmount)()

        Dim GetFinancial As Func(Of String, List(Of clsFinancialAmount)) = AddressOf GetFinancialByMortgageID

        While rdr.Read()

            Dim re = GetFinancial.Invoke(rdr.Item("MORTGAGEID").ToString())

            If Not re Is Nothing Then
                cxlPenderList.First(Function(x) x.ContractID.Equals(rdr.Item("CONTRACTID").ToString())).Financials = re
                cxlPenderList.First(Function(x) x.ContractID.Equals(rdr.Item("CONTRACTID").ToString())).Balance = _
                    Decimal.Round((re.Sum(Function(x) Decimal.Parse(x.Balance)) - _
                     (re.Sum(Function(x) Decimal.Parse(x.InvoiceAmount)))), 2, MidpointRounding.AwayFromZero)

            End If

        End While

        cnn.Close()

        Return cxlPenderList
    End Function

    Private Shared cnn As SqlConnection
    Private Shared cmd As SqlCommand
    Private Shared dt As DataTable


    ''' <summary>
    ''' Get Event objects for contract statuses from Pender to CXL-Pender that changed over a date range.
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCXLPenderInDates(ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of clsPersonnalFinanancialCxlPender)

        Dim sqlText As String = String.Format("select * from t_event where fieldname = 'statusid' " & _
                "and keyfield = 'contractid' " & _
                "and oldvalue = 'Pender' and newvalue = 'CXL-Pender' " & _
                "and datecreated between '{0}' and '{1}'", startDate, endDate)

        cnn = New SqlConnection(Resources.Resource.cns)
        cmd = New SqlCommand(sqlText, cnn)
        cnn.Open()

        Dim rdr As SqlDataReader = cmd.ExecuteReader()

        If (rdr.HasRows = True) Then

            Dim events As List(Of clsEvents) = New List(Of clsEvents)

            While rdr.Read()

                events.Add( _
                     New clsEvents With _
                     { _
                         .EventID = rdr.Item("EventID").ToString(), _
                         .KeyField = rdr.Item("KeyField").ToString(), _
                         .KeyValue = rdr.Item("KeyValue").ToString(), _
                         .EventType = rdr.Item("Type").ToString(), _
                         .OldValue = rdr.Item("OldValue").ToString(), _
                         .NewValue = rdr.Item("NewValue").ToString(), _
                         .DateCreated = rdr.Item("DateCreated").ToString() _
                     } _
                 )
            End While

            rdr.Close()
            cnn.Close()

            Dim del As ConvertToStringDelimited = New ConvertToStringDelimited(AddressOf ToCommaDelimited)
            Dim contractID = del.Invoke(events.Select(Function(x) x.KeyValue.ToString()).ToArray())


            HttpContext.Current.Response.Write(contractID)
            Return Nothing

            Dim aliasId As String = New LookupComboItemIdByContractStatus(AddressOf LookupIdByContractStatus).Invoke("CXL-PENDER")
            sqlText = String.Format("select CONTRACTID, a.PROSPECTID, CONTRACTNUMBER, LastName, FirstName from t_contract a inner join t_Prospect b " & _
                                    "on a.prospectid = b.prospectid where contractid in ({0}) and a.statusid = {1}", contractID, aliasId)


            HttpContext.Current.Response.Write(sqlText)
            Return Nothing

            cmd = New SqlCommand(sqlText, cnn)
            cnn.Open()
            rdr = cmd.ExecuteReader()

            If (rdr.HasRows = True) Then
                Dim cxlPenderList As List(Of clsPersonnalFinanancialCxlPender) = New List(Of clsPersonnalFinanancialCxlPender)()
                While rdr.Read()

                    Dim evt = events.FirstOrDefault(Function(x) x.KeyValue.Equals(rdr.Item("CONTRACTID").ToString()))
                    cxlPenderList.Add(New clsPersonnalFinanancialCxlPender With _
                        { _
                            .ContractID = rdr.Item("CONTRACTID"), _
                            .ContractNumber = rdr.Item("CONTRACTNUMBER"), _
                            .FirstName = rdr.Item("FIRSTNAME"), _
                            .LastName = rdr.Item("LASTNAME"), _
                            .ProspectId = rdr.Item("PROSPECTID") _
                        })
                End While

                cnn.Close()

                HttpContext.Current.Response.Write(cxlPenderList.Count.ToString())
                Return cxlPenderList

            Else
                rdr.Close()
                cnn.Close()
                Return Nothing
            End If
        Else
            rdr.Close()
            cnn.Close()
            Return Nothing
        End If
    End Function

    Private Delegate Function ConvertToStringDelimited(ByVal num() As String) As String
    Private Delegate Function LookupComboItemIdByContractStatus(ByVal text As String) As String


    Private Shared Function ToCommaDelimited(ByVal num() As String) As String

        Dim tmp As String = String.Empty

        For Each x As String In num

            Dim pos As Integer = Array.IndexOf(num, x)

            If pos < num.GetLength(0) - 1 Then
                tmp += String.Format("{0},", x)
            Else
                tmp += x
            End If
        Next

        Return tmp
    End Function

    Private Shared Function LookupIdByContractStatus(ByVal text As String) As String

        Dim sqlText As String = String.Format("select ComboItemID from t_combos a inner join t_comboitems b " & _
                                "on a.comboid = b.comboid where comboname = 'contractstatus' " & _
                                "and comboitem = '{0}' ", text)

        cnn.Close()
        cnn = Nothing
        cnn = New SqlConnection(Resources.Resource.cns)
        cmd = New SqlCommand(sqlText, cnn)
        cnn.Open()
        Dim comboItemId As String = cmd.ExecuteScalar().ToString()

        cnn.Close()
        Return comboItemId
    End Function


    Private Shared Function GetFinancialByMortgageID(ByVal mortgageId As String) As List(Of clsFinancialAmount)

        Dim conx As String = "data source=RS-SQL-01;initial catalog=CRMSNET;user=asp;password=aspnet;" & _
                            "persist security info=False;packet size=4096; "

        Dim sqlText As String = String.Format("select * from dbo.ufn_financials(0, 'MORTGAGEDP', {0}, 0) WHERE INVOICE LIKE 'DOWN PAYMENT EXIT%'", mortgageId)


        Dim conn As SqlConnection = New SqlConnection(conx)
        Dim cmdn As SqlCommand = New SqlCommand(sqlText, conn)

        conn.Open()
        Dim rdrn As SqlDataReader = cmdn.ExecuteReader()

        Dim financialInfo As List(Of clsFinancialAmount) = New List(Of clsFinancialAmount)()

        If rdrn.HasRows = True Then

            While rdrn.Read()

                Dim financial As clsFinancialAmount = _
                    New clsFinancialAmount With _
                    { _
                        .ID = IIf(rdrn.Item("ID").Equals(DBNull.Value), "0", rdrn.Item("ID")), _
                        .Acct = IIf(rdrn.Item("Acct").Equals(DBNull.Value), "0", rdrn.Item("Acct")), _
                        .Invoice = IIf(rdrn.Item("Invoice").Equals(DBNull.Value), String.Empty, rdrn.Item("Invoice")), _
                        .TransDate = IIf(rdrn.Item("TransDate").Equals(DBNull.Value), DateTime.MaxValue.ToString(), rdrn.Item("TransDate")), _
                        .Amount = IIf(rdrn.Item("Amount").Equals(DBNull.Value), "0", rdrn.Item("Amount")), _
                        .Balance = IIf(rdrn.Item("Balance").Equals(DBNull.Value), "0", rdrn.Item("Balance")), _
                        .CCApproval = IIf(rdrn.Item("CCApproval").Equals(DBNull.Value), "0", rdrn.Item("CCApproval")), _
                        .DueDate = IIf(rdrn.Item("DueDate").Equals(DBNull.Value), DateTime.MaxValue.ToString(), rdrn.Item("DueDate")), _
                        .InvoiceAmount = IIf(rdrn.Item("InvoiceAmount").Equals(DBNull.Value), "0", rdrn.Item("InvoiceAmount")), _
                        .KeyValue = IIf(rdrn.Item("KeyValue").Equals(DBNull.Value), "0", rdrn.Item("KeyValue")) _
                    }

                financialInfo.Add(financial)
            End While

            'HttpContext.Current.Response.Write(String.Format("<br/><b>{0}</b>", financial.ID))
            rdrn.Close()
            conn.Close()

            Return financialInfo
        Else

            rdrn.Close()
            conn.Close()

            Return Nothing
        End If
    End Function

End Class
