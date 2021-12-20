Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System
Imports System.Data

Public Module Structures
    Structure User
        Dim PersonnelID As Integer
        Dim UserName As String
        Dim FirstName As String
        Dim LastName As String
        Dim Groups() As String
        Dim VendorIDs() As String
        Dim ActiveVendor As Integer
        Dim VendorMenu As Integer
    End Structure

    Structure CoOwner
        Dim StructID As Integer
        Dim ContractID As Integer
        Dim oPros As clsProspect
        Dim oAdd As clsAddress
        Dim oPhone As clsPhone
        Dim oEmail As clsEmail
    End Structure

    


    Public Function CheckSecurity(ByVal sArea As String, ByVal sItem As String, Optional ByVal sGroups As String = "", Optional ByVal sSid As String = "", Optional ByVal lUserID As Long = 0, Optional ByRef sErr As String = "") As Boolean
        'Return True if permission is granted
        'Returns False if not granted and/or is missing
        'Dim oSec As New clssecurity
        'Return oSec.Check_Security(ByRef sArea As String, ByVal sGroups As String, ByVal sSID As String)


        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_SecurityItem2User u inner join t_SecurityItem i on i.ItemID = u.ItemID inner join t_Securitygroups g on g.GroupID = i.Groupid where g.name = '" & sArea & "' and  u.personnelid = " & lUserID & " and i.Item = '" & sItem & "'", cn)
        Dim DREAD As SqlDataReader
        Dim bReturn As Boolean = False
        'Try
        cn.Open()
        DREAD = cm.ExecuteReader
        bReturn = DREAD.HasRows
        DREAD.Close()
        cn.Close()

        'Catch ex As Exception
        'sErr = ex.ToString
        'Return False
        'Finally
        If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        DREAD = Nothing
        cm = Nothing
        cn = Nothing
        Return bReturn
        'End Try

    End Function

    Public Sub Send_Mail(ByVal emailto As String, ByVal emailFrom As String, ByVal subj As String, ByVal body As String, Optional ByVal isHMTL As Boolean = True)
        Try
            'Dim m As MailMessage = New MailMessage(emailFrom, emailto, subj, body)
            'Dim client2 As New SmtpClient("rs-ex-01")
            'm.IsBodyHtml = isHMTL
            ''client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis
            'client2.Send(m)
            If (subj.ToLower().Substring(0, 10) = "security r" Or subj.ToLower().Substring(0, 10) = "work order") Or emailFrom.ToLower() = "mis@kingscreekplantation.com" Or emailFrom.ToLower() = "misdept@kingscreekplantation.com" Or emailFrom.ToLower() = "administrator@kingscreekplantation.com" Then
                emailFrom = "mis_department@kingscreekplantation.com"
            End If
            Dim client As SmtpClient = New SmtpClient("smtp.office365.com", 587)
            client.EnableSsl = True
            client.Credentials = New System.Net.NetworkCredential("wmcopiers@kingscreekplantation.com", "Temp99")
            Dim from As MailAddress = New MailAddress(emailFrom, String.Empty, System.Text.Encoding.UTF8)
            Dim ato As MailAddress = New MailAddress(emailto)
            Dim message As MailMessage = New MailMessage(from, ato)
            message.Body = body
            message.IsBodyHtml = isHMTL
            message.BodyEncoding = System.Text.Encoding.UTF8
            message.Subject = subj
            message.SubjectEncoding = System.Text.Encoding.UTF8
            '// Set the method that is called back when the send operation ends.
            'client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            '// The userState can be any object that allows your callback 
            '// method to identify this send operation.
            '// For this example, I am passing the message itself
            client.Send(message)


        Catch ex As Exception

        End Try
        
    End Sub

    Public Sub Send_Mail_wAttachment(ByVal sfrom As String, ByVal sto As String, ByVal subject As String, ByVal body As String, ByVal Attachment As Attachment)
        'Dim mail As MailMessage = New MailMessage(sfrom, sto, subject, body)
        'mail.Attachments.Add(Attachment)
        'Dim smtp As SmtpClient = New SmtpClient("rs-ex-01")
        'smtp.Send(mail)
        'smtp = Nothing
        'mail = Nothing

        If (subject.ToLower().Substring(0, 10) = "security r" Or subject.ToLower().Substring(0, 10) = "work order") Or sfrom.ToLower() = "mis@kingscreekplantation.com" Or sfrom.ToLower() = "misdept@kingscreekplantation.com" Or sfrom.ToLower() = "administrator@kingscreekplantation.com" Then
            sfrom = "mis_department@kingscreekplantation.com"
        End If
        Dim client As SmtpClient = New SmtpClient("smtp.office365.com", 587)
        client.EnableSsl = True
        client.Credentials = New System.Net.NetworkCredential("wmcopiers@kingscreekplantation.com", "Temp99")
        Dim from As MailAddress = New MailAddress(sfrom, String.Empty, System.Text.Encoding.UTF8)
        Dim ato As MailAddress = New MailAddress(sto)
        Dim message As MailMessage = New MailMessage(from, ato)
        message.Body = body
        'message.IsBodyHtml = isHMTL
        message.BodyEncoding = System.Text.Encoding.UTF8
        message.Subject = subject
        message.Attachments.Add(Attachment)
        message.SubjectEncoding = System.Text.Encoding.UTF8
        '// Set the method that is called back when the send operation ends.
        'client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        '// The userState can be any object that allows your callback 
        '// method to identify this send operation.
        '// For this example, I am passing the message itself
        client.Send(message)
    End Sub

    Public Function Load_Contract_Data(ByVal conID As Integer) As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        dt.TableName = "ContractData"
        dt.Columns.Add("FirstName")
        dt.Columns.Add("LastName")
        dt.Columns.Add("MiddleInit")
        dt.Columns.Add("OwnerDisplay") 'Deed Owner Display
        dt.Columns.Add("Owner") '
        dt.Columns.Add("OwnerLetter") 'Owner Display for Letter
        dt.Columns.Add("ConDay")
        dt.Columns.Add("ConMonth")
        dt.Columns.Add("ConYear")
        dt.Columns.Add("ContractNumber")
        dt.Columns.Add("Unit")
        dt.Columns.Add("Week")
        dt.Columns.Add("SalePrice")
        dt.Columns.Add("TitleType")
        dt.Columns.Add("OccupancyYear")
        dt.Columns.Add("StateFee")
        dt.Columns.Add("LocalFee")
        dt.Columns.Add("AddtlFee")
        dt.Columns.Add("WeekType")
        dt.Columns.Add("WeekTypePA") 'Purchase Agreement
        dt.Columns.Add("Frequency")
        dt.Columns.Add("DownPayment")
        dt.Columns.Add("DownPaymentReceived")
        dt.Columns.Add("DownPaymentScheduled")
        dt.Columns.Add("OrigDate")
        dt.Columns.Add("CCFinanced")
        dt.Columns.Add("Ra")
        dt.Columns.Add("Fc")
        dt.Columns.Add("AmtFinanced")
        dt.Columns.Add("TP")
        dt.Columns.Add("FP")
        dt.Columns.Add("Terms")
        dt.Columns.Add("MPMT")
        dt.Columns.Add("FPMT")
        dt.Columns.Add("FPMTDay")
        dt.Columns.Add("BDUnit")
        dt.Columns.Add("Points")
        dt.Columns.Add("ContractDate")
        dt.Columns.Add("Years")
        dt.Columns.Add("MPMT-6")
        dt.Columns.Add("MPMT10")
        dt.Columns.Add("MPMT10-6")
        dt.Columns.Add("SVCFEE")
        dt.Columns.Add("CC")
        dt.Columns.Add("EXP")
        dt.Columns.Add("ATL+ATLB+PROC")
        dt.Columns.Add("ATL")
        dt.Columns.Add("ATLB")
        dt.Columns.Add("ATL+ATLB")
        dt.Columns.Add("RECDEED")
        dt.Columns.Add("RECDOT")
        dt.Columns.Add("RECDEED+RECDOT")
        dt.Columns.Add("CCLESSEXPLESSATLLESSREC")
        dt.Columns.Add("A+B+C")
        dt.Columns.Add("CCP")
        dt.Columns.Add("CCLESSCCP")
        dt.Columns.Add("CCF+DP")
        dt.Columns.Add("SPC")
        dt.Columns.Add("FIN+DP")
        dt.Columns.Add("TIP")
        dt.Columns.Add("SIZE")
        dt.Columns.Add("SPLIT")
        dt.Columns.Add("MF")
        dt.Columns.Add("INV")
        dt.Columns.Add("ADD1")
        dt.Columns.Add("ADD2")
        dt.Columns.Add("CLOSER")
        dt.Columns.Add("UGOptionSeason")
        dt.Columns.Add("UGoptionBD")
        dt.Columns.Add("UGoptionUnitType")
        dt.Columns.Add("UGOptionPrice")
        dt.Columns.Add("ClubExplore")
        dt.Columns.Add("Company")
        dt.Columns.Add("Date")
        dt.Columns.Add("Phone")
        dt.Columns.Add("OldOwnerDisplay")
        dt.Columns.Add("OldFrequency")
        dt.Columns.Add("OldUnit")
        dt.Columns.Add("OldWeek")
        dt.Columns.Add("OldOwner")
        dt.Columns.Add("Rb")
        dt.Columns.Add("CashOutDate")
        Dim dtSig As New DataTable
        dtSig.TableName = "Signatures"

        Dim dtOwners As New DataTable
        dtOwners.TableName = "Owners"

        Dim dtOldSig As New DataTable
        dtOldSig.TableName = "OldSignatures"

        Dim oPros As New clsProspect
        Dim oCon As New clsContract
        Dim oMort As New clsMortgage
        Dim oCombo As New clsComboItems
        Dim oFreq As New clsFrequency
        Dim oRecFeesDeed As New clsRecordingFeesDeed
        Dim oSI As New clsSoldInventory
        Dim oMortCC As New clsMortgageClosingCosts
        Dim oAdd As New clsAddress
        Dim oUField As New clsUserFields
        Dim oPhone As New clsPhone
        Dim oOldContract As New clsContractUpDownGrade
        Dim oldConID As Integer = oOldContract.Get_Old_ContractID(conID)
        oCon.ContractID = conID
        oCon.Load()
        oPros.Prospect_ID = oCon.ProspectID
        oPros.Load()
        oMort.MortgageID = oMort.Get_Mortgage_ID(oCon.ContractID)
        '        oMort.ContractID = oCon.ContractID
        oMort.Load()
        Dim drow As DataRow = dt.NewRow
        If oldConID > 0 Then
            Dim oOldCOn As New clsContract
            oOldCOn.ContractID = oldConID
            oOldCOn.Load
            drow("OldOwnerDisplay") = oCon.Get_Owner_Display(oldConID)
            oFreq.FrequencyID = oOldCOn.FrequencyID
            oFreq.Load()
            drow("OldFrequency") = oFreq.Frequency
            drow("OldUnit") = oCon.Get_Old_Unit_Display(oldConID)
            drow("OldWeek") = oCon.Get_Old_Week_Display(oldConID)
            drow("OldOwner") = oCon.Get_Owner(oldConID, False)
            oOldCOn = Nothing
        Else
            drow("OldOwnerDisplay") = ""
            drow("OldFrequency") = ""
            drow("OldUnit") = "N/A"
            drow("OldWeek") = "N/A"
            drow("OldOwner") = ""
        End If
        drow("FirstName") = oPros.First_Name
        drow("LastName") = oPros.Last_Name
        drow("MiddleInit") = oPros.MiddleInit
        drow("OwnerDisplay") = oCon.Get_Owner_Display(oCon.ContractID)
        drow("Owner") = oCon.Get_Owner(oCon.ContractID, False)
        drow("OwnerLetter") = oCon.Get_Owner(oCon.ContractID, True)
        Select Case Day(oCon.ContractDate)
            Case 1, 21, 31
                drow("ConDay") = Day(oCon.ContractDate) & "st"
            Case 2, 22
                drow("ConDay") = Day(oCon.ContractDate) & "nd"
            Case 3, 23
                drow("ConDay") = Day(oCon.ContractDate) & "rd"
            Case Else
                drow("ConDay") = Day(oCon.ContractDate) & "th"
        End Select
        drow("ConMonth") = MonthName(Month(oCon.ContractDate))
        drow("ConYear") = Year(oCon.ContractDate)
        drow("Unit") = oCon.Get_Unit_Display(oCon.ContractID)
        drow("Week") = oCon.Get_Week_Display(oCon.ContractID)
        drow("SalePrice") = FormatCurrency(oMort.SalesPrice, 2) '"$" & IIf(oMort.SalesPrice = 0, 0.00, CDbl(oMort.SalesPrice))
        If oMort.TitleTypeID = 0 Then
            drow("TitleType") = "Husband and Wife, As Tenants by the Entirety With Right of Survivorship as at Common Law"
        Else
            oCombo.ID = oMort.TitleTypeID
            oCombo.Load()
            drow("TitleType") = oCombo.Description
        End If
        drow("OccupancyYear") = Year(oCon.OccupancyDate)
        drow("StateFee") = FormatCurrency(oRecFeesDeed.Get_Fees("State039", oMort.SalesPrice), 2)
        drow("LocalFee") = FormatCurrency(oRecFeesDeed.Get_Fees("County213", oMort.SalesPrice), 2)
        drow("AddtlFee") = FormatCurrency(oRecFeesDeed.Get_Fees("DoC038", oMort.SalesPrice) + oRecFeesDeed.Get_Fees("Grantor220", oMort.SalesPrice), 2)
        If oCombo.Lookup_ComboItem(oCon.WeekTypeID) = "Float" Then
            drow("WeekType") = "____________"
            drow("WeekTypePA") = "Floating Week"
        Else
            drow("WeekType") = oCon.Get_CheckIn_Day(oCon.ContractID)
            drow("WeekTypePA") = "fixed week, " & oCon.Get_CheckIn_Day(oCon.ContractID) & " check-in"
        End If
        oFreq.FrequencyID = oCon.FrequencyID
        oFreq.Load()
        drow("Frequency") = oFreq.Frequency
        drow("DownPayment") = FormatCurrency(oMort.Get_DP_Total(oMort.MortgageID), 2)
        drow("DownPaymentReceived") = FormatCurrency(oMort.Get_DP_Received(oMort.MortgageID), 2)
        drow("DownPaymentScheduled") = FormatCurrency(oMort.Get_DP_Scheduled(oMort.MortgageID), 2)
        drow("OrigDate") = oMort.OrigDate
        drow("CCFinanced") = FormatCurrency(oMort.Get_CC_Financed_Amt(oMort.MortgageID), 2)
        drow("ContractNumber") = oCon.ContractNumber
        Dim pmtAmt As Double = 0
        If oMort.TotalFinanced > 0 Then
            pmtAmt = Pmt((oMort.APR / 100) / 12, oMort.Terms, oMort.TotalFinanced * -1)
            pmtAmt = Rate(oMort.Terms, (pmtAmt + 6) * -1, oMort.TotalFinanced) * 12 * 100
            drow("Ra") = Format(pmtAmt, "###.00") & "%"
        Else
            drow("Ra") = "N/A"
        End If
        Dim curFC As Double = 0
        If oMort.TotalFinanced > 0 Then
            For i = 1 To oMort.Terms
                curFC = curFC + (IPmt((oMort.APR / 1200), i, oMort.Terms, oMort.TotalFinanced) * -1)
            Next
            curFC = curFC + (oMort.Terms * 6)
        End If
        drow("Fc") = FormatCurrency(curFC, 2)
        drow("AmtFinanced") = FormatCurrency(oMort.TotalFinanced, 2)
        If oMort.TotalFinanced > 0 Then
            curFC = curFC + oMort.TotalFinanced
        End If
        drow("TP") = FormatCurrency(curFC, 2)
        If oMort.TotalFinanced > 0 Then
            curFC = curFC + oMort.DPTotal
        End If
        drow("FP") = FormatCurrency(curFC, 2)
        If oMort.TotalFinanced > 0 Then
            drow("Terms") = oMort.Terms
        Else
            drow("Terms") = "N/A"
        End If
        pmtAmt = 0
        If oMort.TotalFinanced > 0 Then
            pmtAmt = Pmt((0.169 / 12), oMort.Terms, oMort.TotalFinanced * -1)
        End If
        drow("Rb") = FormatCurrency(pmtAmt, 2)
        pmtAmt = 0
        If oMort.TotalFinanced > 0 Then
            pmtAmt = Pmt((oMort.APR / 100) / 12, oMort.Terms, oMort.TotalFinanced * -1) + 6
        End If
        drow("MPMT") = FormatCurrency(pmtAmt, 2)
        pmtAmt = 0
        If oMort.TotalFinanced > 0 Then
            pmtAmt = Pmt((oMort.APR / 100) / 12, oMort.Terms, oMort.TotalFinanced * -1)
        End If
        drow("MPMT-6") = FormatCurrency(pmtAmt, 2)
        pmtAmt = 0
        If oMort.TotalFinanced > 0 Then
            If oMort.Terms > 0 Then
                pmtAmt = Pmt((oMort.APR / 100) / 12, oMort.Terms, oMort.TotalFinanced * -1) + 6
            End If
        End If
        drow("MPMT10") = FormatCurrency(pmtAmt, 2)
        If oMort.TotalFinanced > 0 Then
            If oMort.Terms > 0 Then
                pmtAmt = Pmt((oMort.APR / 100) / 12, oMort.Terms, oMort.TotalFinanced * -1)
            End If
        End If
        drow("MPMT10-6") = FormatCurrency(pmtAmt, 2)
        If oMort.TotalFinanced > 0 Then
            drow("SVCFEE") = FormatCurrency(6, 2)
        Else
            drow("SVCFEE") = FormatCurrency(0, 2)
        End If
        If oMort.TotalFinanced > 0 Then
            Dim fPMT As String = ""
            Dim fPMTDay As String = ""
            fPMT = MonthName(Month(oMort.FirstPaymentDate))
            Select Case Day(oMort.FirstPaymentDate)
                Case 1, 21, 31
                    fPMTDay = Day(oMort.FirstPaymentDate) & "st"
                Case 2, 22
                    fPMTDay = Day(oMort.FirstPaymentDate) & "nd"
                Case 3, 23
                    fPMTDay = Day(oMort.FirstPaymentDate) & "rd"
                Case Else
                    fPMTDay = Day(oMort.FirstPaymentDate) & "th"
            End Select
            fPMT = fPMT & " " & fPMTDay
            fPMT = fPMT & ", " & Year(oMort.FirstPaymentDate)
            drow("FPMT") = fPMT
            drow("FPMTDay") = fPMTDay
        Else
            drow("FPMT") = "N/A"
            drow("FPMTDay") = "N/A"
        End If
        drow("BDUnit") = "<u>" & oSI.Get_UnitBD_Display(oCon.ContractID) & "</u>"
        drow("Points") = "<u>" & oSI.Get_Points_Value(oCon.ContractID) & "</u>"
        drow("ContractDate") = CDate(oCon.ContractDate).ToShortDateString
        drow("Years") = oMort.Terms / 12
        drow("CC") = FormatCurrency(oMort.Get_CC_Total(oMort.MortgageID), 2)
        drow("EXP") = FormatCurrency(oMortCC.Get_Fee("Equifax", oMort.MortgageID), 2)
        drow("ATL+ATLB+PROC") = FormatCurrency(oMort.Get_CC_Total(oMort.MortgageID) - oMortCC.Get_Fee("Equifax", oMort.MortgageID) - oMortCC.Get_Fee("Recording Fees Deed", oMort.MortgageID) - oMortCC.Get_Fee("RecordingFeesDOT", oMort.MortgageID), 2)
        drow("ATL") = FormatCurrency(oMortCC.Get_Fee("ConsumerFirstLender", oMort.MortgageID), 2)
        drow("ATLB") = FormatCurrency(oMortCC.Get_Fee("ConsumerFirstBorrower", oMort.MortgageID), 2)
        drow("ATL+ATLB") = FormatCurrency(oMortCC.Get_Fee("ConsumerFirstLender", oMort.MortgageID) + oMortCC.Get_Fee("ConsumerFirstBorrower", oMort.MortgageID), 2)
        drow("RECDEED") = FormatCurrency(oMortCC.Get_Fee("Recording Fees Deed", oMort.MortgageID), 2)
        drow("RECDOT") = FormatCurrency(oMortCC.Get_Fee("RecordingFeesDOT", oMort.MortgageID), 2)
        drow("RECDEED+RECDOT") = FormatCurrency(oMortCC.Get_Fee("Recording Fees Deed", oMort.MortgageID) + oMortCC.Get_Fee("RecordingFeesDOT", oMort.MortgageID), 2)
        drow("CCLESSEXPLESSATLLESSREC") = FormatCurrency(oMort.Get_CC_Total(oMort.MortgageID) - oMortCC.Get_Fee("Equifax", oMort.MortgageID) - oMortCC.Get_Fee("ConsumerFirstLender", oMort.MortgageID) - oMortCC.Get_Fee("ConsumerFirstBorrower", oMort.MortgageID) - oMortCC.Get_Fee("Recording Fees Deed", oMort.MortgageID) - oMortCC.Get_Fee("RecordingFeesDOT", oMort.MortgageID), 2)
        drow("A+B+C") = FormatCurrency(oMort.Get_CC_Total(oMort.MortgageID) - oMortCC.Get_Fee("Recording Fees Deed", oMort.MortgageID) - oMortCC.Get_Fee("RecordingFeesDOT", oMort.MortgageID), 2)
        drow("CCP") = FormatCurrency(oMort.CCTotal - oMort.CCFinanced, 2)
        drow("CCLESSCCP") = FormatCurrency(oMort.Get_CC_Total(oMort.MortgageID) - (oMort.CCTotal - oMort.CCFinanced), 2)
        drow("CCF+DP") = FormatCurrency(oMort.Get_DP_Total(oMort.MortgageID) + oMort.Get_CC_Total(oMort.MortgageID) - (oMort.CCTotal - oMort.CCFinanced), 2)
        drow("SPC") = FormatCurrency(oMort.SalesPrice + oMort.Get_CC_Total(oMort.MortgageID), 2)
        drow("FIN+DP") = FormatCurrency(oMort.TotalFinanced + oMort.Get_DP_Total(oMort.MortgageID), 2)
        drow("CLOSER") = oCon.Get_Closing_Officer(oCon.ContractID)
        drow("ClubExplore") = FormatCurrency(oCon.get_CE_Dues_Amt(oCon.ProspectID), 2)

        'If oCon.FrequencyID = 1 Then
        '    drow("ClubExplore") = FormatCurrency(179, 2)
        'ElseIf oCon.FrequencyID = 2 Then
        '    drow("ClubExplore") = FormatCurrency(132, 2)
        'ElseIf oCon.FrequencyID = 3 Then
        '    drow("ClubExplore") = FormatCurrency(117, 2)
        'Else
        '    drow("ClubExplore") = "N/A"
        'End If
        curFC = 0
        If oMort.TotalFinanced > 0 Then
            For i = 1 To oMort.Terms
                curFC = curFC + (IPmt((oMort.APR / 1200), i, oMort.Terms, oMort.TotalFinanced) * -1)
            Next
            curFC = curFC + (oMort.Terms * 6)
            curFC = curFC / oMort.TotalFinanced
        End If
        drow("TIP") = FormatPercent(curFC, 2)
        drow("SIZE") = oCon.Get_Unit_Size(oCon.ContractID)
        If oCon.SplitMF Then
            drow("SPLIT") = "Should we choose to split our Triennial usage over 2 to 3 years, we must pay our maintenance fee on a pro-rata basis (1/3 of the stated fee for each key of usage accessed per year)"
        Else
            drow("SPLIT") = ""
        End If
        drow("MF") = FormatCurrency(oCon.MaintenanceFeeAmount, 2)
        drow("INV") = oCon.Get_Inventory(oCon.ContractID)
        oAdd.AddressID = oAdd.Get_Contract_Address(oCon.ProspectID)
        oAdd.Load()
        drow("ADD1") = Trim(Replace(oAdd.Address1, Chr(13), ""))
        drow("ADD2") = oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode
        oUField.ID = oUField.Get_UserField_Value_ID(oUField.Get_UserFieldID(oUField.Get_GroupID("Contract"), "UpgradeOptionSeason"), oCon.ContractID)
        oUField.Load()
        If oUField.UFValue = "" Then
            drow("UGOptionSeason") = "N/A"
        Else
            drow("UGOptionSeason") = oUField.UFValue
        End If
        oUField.ID = oUField.Get_UserField_Value_ID(oUField.Get_UserFieldID(oUField.Get_GroupID("Contract"), "UpgradeOptionBD"), oCon.ContractID)
        oUField.Load()
        If oUField.UFValue = "" Then
            drow("UGOptionBD") = "N/A"
        Else
            drow("UGOptionBD") = oUField.UFValue
        End If
        oUField.ID = oUField.Get_UserField_Value_ID(oUField.Get_UserFieldID(oUField.Get_GroupID("Contract"), "UpgradeOptionUnitType"), oCon.ContractID)
        oUField.Load()
        If oUField.UFValue = "" Then
            drow("UGOptionUnitType") = "N/A"
        Else
            drow("UGOptionUnitType") = oUField.UFValue
        End If
        oUField.ID = oUField.Get_UserField_Value_ID(oUField.Get_UserFieldID(oUField.Get_GroupID("Contract"), "UpgradeOptionPrice"), oCon.ContractID)
        oUField.Load()
        If oUField.UFValue = "" Then
            drow("UGOptionPrice") = "N/A"
        Else
            drow("UGOptionPrice") = FormatCurrency(oUField.UFValue, 2)
        End If
        drow("Company") = oCon.CompanyName
        drow("Date") = MonthName(Month(System.DateTime.Now)) & " " & Day(System.DateTime.Now) & ", " & Year(System.DateTime.Now)
        drow("Phone") = oPhone.Get_Phone_Number(oCon.ProspectID)
        drow("CashOutDate") = CDate(oCon.ContractDate).AddDays(45).ToShortDateString
        dt.Rows.Add(drow)
        ds.Tables.Add(dt)

        dtSig = oCon.Get_Signatures(oCon.ContractID, False)
        ds.Tables.Add(dtSig)

        dtOwners = oCon.Get_Owners(oCon.ContractID)
        ds.Tables.Add(dtOwners)

        dtOldSig = oCon.Get_Signatures(oldConID, True)
        ds.Tables.Add(dtOldSig)

        oCon = Nothing
        oAdd = Nothing
        oPros = Nothing
        oCombo = Nothing
        oMort = Nothing
        oMortCC = Nothing
        oRecFeesDeed = Nothing
        oSI = Nothing
        oPhone = Nothing
        Return ds
    End Function

    Public Structure Batch_Records
        Dim PaySummaryID As Integer
        Dim BatchID As Integer
        Dim PersonnelID As Integer
        Dim HourlyRate As Decimal
        Dim Hours As Decimal
        Dim OTHours As Decimal
        Dim JuryHours As Decimal
        Dim PALHours As Decimal
        Dim SSLBHours As Decimal
        Dim BERHours As Decimal
        Dim HourlyPay As Decimal
        Dim SalaryPay As Decimal
        Dim TourPay As Decimal
        Dim TourSubTotal As Decimal
        Dim PremiumsTotal As Decimal
        Dim RefundableDeposits As Decimal
        Dim NonRefundableDeposits As Decimal
        Dim TourDeposits As Decimal
        Dim SalesPay As Decimal
        Dim ReserveDeposit As Decimal
        Dim ReserveWithdrawal As Decimal
        Dim ChargeBackAmount As Decimal
        Dim AdvancePayment As Decimal
        Dim Spiff As Decimal
        Dim SteadyPayRate As Decimal
        Dim SteadyPay As Decimal
        Dim DrawPayRate As Decimal
        Dim DrawPay As Decimal
        Dim DepartmentID As Integer
        Dim Department As String
        Dim CompanyID As Integer
        Dim Bonus As Decimal
        Dim CommissionAmount As Decimal
        Dim CancellationAmount As Decimal
        Dim PAL As Decimal
        Dim SSLB As Decimal
        Dim PosAdj As Decimal
        Dim NegAdj As Decimal
        Dim Posted As Boolean
        Dim TourCount As Integer
        Dim TourPayRate As Decimal
        Dim DeveloperSalesPay As Decimal
        Dim HoldCommissions As Decimal
        Dim ReleasedCommissions As Decimal
        Dim HoldCancellationsAmount As Decimal
        Dim ReleasedCancellationsAmount As Decimal
    End Structure

End Module
