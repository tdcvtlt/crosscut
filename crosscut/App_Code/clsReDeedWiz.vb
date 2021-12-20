Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Sql

Public Class clsReDeedWiz

    Dim _Err As String = ""

    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property

    Public Function Check_Contract(ByVal strContractNumber As String, ByRef iID As Integer) As Boolean
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        cm.CommandText = "Select a.*, b.ComboItem from t_Contract a inner join t_ComboItems b on a.statusid = b.comboitemid where a.contractnumber = '" & strContractNumber & "'"
        da.Fill(ds, "0")

        If ds.Tables("0").Rows.Count = 0 Then
            _Err = "Can't Find That Contract.<br />Please enter a different Contract Number"
        Else
            iID = ds.Tables("0").Rows(0)("ContractID")
            If ds.Tables("0").Rows(0)("Comboitem") & "" = "Active" Or ds.Tables("0").Rows(0)("Comboitem") & "" = "Suspense" Or ds.Tables("0").Rows(0)("Comboitem") & "" = "ReDeed" Then
                cm.CommandText = "Select * from t_Mortgage where contractid = '" & ds.Tables("0").Rows(0)("ContractID") & "'"
                da.Fill(ds, "1")
                If ds.Tables("1").Rows.Count > 0 Then
                    '***** Check to See if Contract Has Restrictions *******'
                    '***** If the restriction is ForeClosurePending, ReverterPending, BankruptcyPending, or PaymentScheduledtoAvoidGarnisment or PaymentScheduledtoAvoidJudgement
                    cm.CommandText = "Select a.Name, a.StartDate, a.EndDate from t_UsageRestriction2Contract b inner join t_UsageRestriction a on a.usagerestrictionid = b.UsageRestrictionID where b.contractid = '" & ds.Tables("0").Rows(0)("ContractID") & "' and (a.EndDate is Null or a.EndDate > '" & Date.Today & "')"
                    da.Fill(ds, "2")
                    If ds.Tables("2").Rows.Count > 0 Then
                        _Err = "There is A Usage Restriction on this Contract that must be Removed Before ReDeeding."
                    Else
                        _Err = ""
                    End If
                Else
                    _Err = "Please Insert a Mortgage Before ReDeeding this Contract."
                End If
            Else
                _Err = "This Contract Can Not Be ReDeeded."
            End If
        End If
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing

        Return IIf(_Err = "", True, False)

    End Function


    Public Function Validate_ContractNumber(ByVal strContractNumber As String) As Boolean
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        cm.CommandText = "Select * from t_Contract where contractnumber = '" & strContractNumber & "'"
        da.Fill(ds, "0")
        If ds.Tables("0").Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
    End Function

    Public Function Save_Step2(ByVal iID As Integer, ByVal iOldKCPID As Integer, ByVal sNewKCP As String, ByVal iOccYear As Integer, ByVal iMortTitleType As Integer, ByVal sDeedType As String, ByVal UserID As Integer, ByVal ContractDate As String, ByVal Executor As String, ByVal TransferFee As Double, ByVal DeceasedDate As String) As Integer
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet



        '****** Get ComboItemID for ReDeed and Cancelled
        cm.CommandText = "Select comboitemid from t_ComboItems i inner join t_Combos com on com.comboid = i.comboid where comboitem = 'Pending ReDeed' and comboname = 'ContractStatus'"
        da.Fill(ds, "0")
        Dim reDeedID As Integer = ds.Tables("0").Rows(0)("Comboitemid")
        ds.Tables("0").Clear()
        cm.CommandText = "Select comboitemid from t_ComboItems i inner join t_Combos com on com.comboid = i.comboid where comboitem = 'Canceled' and comboname = 'ContractStatus'"
        da.Fill(ds, "0")
        Dim cxlID As Integer = ds.Tables("0").Rows(0)("ComboItemID")
        ds.Tables("0").Clear()

        '***** Get ComboItemID from SaleTypeID

        cm.CommandText = "Select b.ComboItem from t_Contract a inner join t_ComboItems b on a.saletypeid = b.comboitemid where a.ContractID = " & iOldKCPID
        da.Fill(ds, "0")
        Dim saletypeid As Integer = 0
        If ds.Tables("0").Rows.Count = 0 Then
            saletypeid = 0
        Else
            If Left(ds.Tables("0").Rows(0)("ComboItem"), 4) = "Cott" Then
                cm.CommandText = "Select comboitemid from t_ComboItems i inner join t_Combos com on com.comboid = i.comboid where comboname = 'contractsaletype' and comboitem = 'Cottage Rewrite'"
                da.Fill(ds, "1")
                saletypeid = ds.Tables("1").Rows(0)("comboitemid")
                ds.Tables("1").Clear()
            Else
                cm.CommandText = "Select comboitemid from t_ComboItems i inner join t_Combos com on com.comboid = i.comboid where comboname = 'contractsaletype' and left(comboitem, 4) = '" & Left(ds.Tables("0").Rows(0)("ComboItem"), 4) & "' and right(rtrim(comboitem), 7) = 'ReWrite'"
                da.Fill(ds, "1")
                If ds.Tables("1").Rows.Count = 0 Then
                    saletypeid = 0
                Else
                    saletypeid = ds.Tables("1").Rows(0)("ComboItemID")
                End If
                ds.Tables("1").Clear()
            End If
        End If
        ds.Tables("0").Clear()

        '****** Need to Create New Contract and Mortgage record to match the records of the redeeded contractnumber
        '****** Also need to update the status of the old contract to cancelled and the new contract to ReDeed
        Dim oldContract As New clsContract
        oldContract.ContractID = iOldKCPID
        oldContract.Load()
        Dim oCI As New clsComboItems
        Dim oContract As New clsContract
        oContract.UserID = UserID
        oContract.ContractNumber = sNewKCP
        oContract.ProspectID = 0
        oContract.LocationID = oldContract.LocationID
        oContract.ContractDate = ContractDate 'oldContract.ContractDate
        'oContract.StatusDat = sContractDate
        oContract.OccupancyDate = CDate("1/1/" & iOccYear)
        oContract.SaleTypeID = saletypeid
        oContract.SaleSubTypeID = oldContract.SaleSubTypeID
        oContract.StatusID = reDeedID
        oContract.TourID = oldContract.TourID
        oContract.FrequencyID = oldContract.FrequencyID
        oContract.WeekTypeID = oldContract.WeekTypeID
        oContract.SeasonID = oldContract.SeasonID
        'seasonID = rs.Fields("SeasonID")
        oContract.CampaignID = oldContract.CampaignID
        oContract.SaleSubTypeID = oldContract.SaleSubTypeID
        oContract.SubTypeID = oldContract.SubTypeID
        oContract.BillingCodeID = oldContract.BillingCodeID
        oContract.MaintenanceFeeAmount = oldContract.MaintenanceFeeAmount
        oContract.TypeID = oldContract.TypeID
        'rs2.Fields("CompanyName") = rs.Fields("CompanyName")
        oContract.SubStatusID = oCI.Lookup_ID("ContractSubStatus", "ReDeed")
        oContract.SplitMF = oldContract.SplitMF
        oContract.Save()

        Dim contID As Integer = oContract.ContractID
        Dim oldStatusID As Integer = oldContract.StatusID
        'oldContract.StatusID = cxlID
        'oldContract.UserID = UserID
        'oldContract.StatusDate = Date
        'oldContract.Save()

        Dim oldMort As New clsMortgage
        Dim newMort As New clsMortgage

        oldMort.ContractID = iOldKCPID
        oldMort.Load()

        newMort.ContractID = contID
        'ProspectID") = 0
        'LocationID") = rs.Fields("LocationID")
        newMort.TitleTypeID = iMortTitleType
        newMort.InterestTypeID = oldMort.InterestTypeID
        newMort.SalesVolume = oldMort.SalesVolume
        newMort.SalesPrice = oldMort.SalesPrice
        newMort.CommVolume = oldMort.CommVolume
        newMort.APR = oldMort.APR
        newMort.Terms = oldMort.Terms
        newMort.CashOutDate = oldMort.CashOutDate
        newMort.DPDueDate = oldMort.DPDueDate
        newMort.StatusID = oCI.Lookup_ID("MortgageStatus", "N/A")
        newMort.Save()
        Dim mortID As Integer = newMort.MortgageID


        '****** Need to Copy Personnel Trans from Old Contract to New Contract *******'
        cm.CommandText = "Select * from t_PersonnelTrans where keyfield = 'contractid' and keyvalue = '" & iOldKCPID & "'"
        da.Fill(ds, "0")
        For i = 0 To ds.Tables("0").Rows.Count - 1
            Dim oPT As New clsPersonnelTrans
            oPT.Created_By_ID = UserID
            oPT.PersonnelID = ds.Tables("0").Rows(i)("PersonnelID")
            'oPT.LocationID = ds.Tables("0").Rows(i)("LocationID")
            oPT.KeyField = "ContractID"
            oPT.KeyValue = contID

            If (ds.Tables("0").Rows(i)("TitleID").Equals(DBNull.Value) = False) Then
                oPT.TitleID = ds.Tables("0").Rows(i)("TitleID")
            End If

            oPT.Percentage = ds.Tables("0").Rows(i)("Percentage")
            oPT.Date_Created = Date.Now
            'oPT.Active = 1
            oPT.Save()
            oPT = Nothing
        Next
        ds.Tables("0").Clear()

        

        '******** Create entry into ReDeeds table *********
        ''******** Get ReDeed Type ********'
        cm.CommandText = "Select comboitemid from t_ComboItems i inner join t_Combos com on com.comboid = i.comboid where comboitem = '" & sDeedType & "'"
        da.Fill(ds, "0")
        Dim DeedType As Integer = ds.Tables("0").Rows(0)("ComboItemID")
        ds.Tables("0").Clear()

        Dim oReDeed As New clsReDeeds
        oReDeed.ReDeedID = iID
        oReDeed.Load()
        oReDeed.ReDeededTo = contID
        oReDeed.ReDeededFrom = iOldKCPID
        oReDeed.ReDeedDate = ContractDate
        oReDeed.TransferFee = TransferFee
        oReDeed.ReDeedTypeID = DeedType
        If DeceasedDate <> "" Then
            oReDeed.DeceasedDate = DeceasedDate
        End If
        oReDeed.Executor = Executor
        oReDeed.Save()
        oReDeed = Nothing

        oldMort = Nothing
        newMort = Nothing
        oldContract = Nothing
        oContract = Nothing
        oCI = Nothing
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        Return contID
    End Function

    Public Sub Save_Step3()
        '***** Release Inventory from Old Contract and Assign it to New Contract ******'
        'Dim cn As New SqlConnection(Resources.Resource.cns)
        'Dim cm As New SqlCommand("", cn)
        'Dim da As New SqlDataAdapter(cm)
        'Dim ds As New DataSet
        'Dim oHist As New clsSalesInventory2ContractHist
        'Dim oSold As New clsSoldInventory
        'Dim oNew As New clsSalesInventory

        'cm.CommandText = "Select * from t_SoldInventory where contractid = '" & iOldKCPID & "'"
        'da.Fill(ds, "0")
        'For i = 0 To ds.Tables("0").Rows.Count - 1
        '    '**** Mark As Released ****'
        '    oHist.ContractID = iOldKCPID
        '    oHist.SalesInventoryID = ds.Tables("0").Rows(i)("SalesInventoryID")
        '    oHist.Load()
        '    oHist.DateRemoved = Date.Now
        '    oHist.Active = False
        '    oHist.Save()


        '    oSold.SoldInventoryID = ds.Tables("0").Rows(i)("SoldInventoryID")
        '    oSold.Load()
        '    oSold.Delete()
        '    oNew.Assign_Inventory(contID, oSold.SalesInventoryID)

        'Next
        'ds.Tables("0").Clear()

        '

        ''******* Insert Transfer Fee Account Item into new Contract Number
        'cm.CommandText = "Select finTransCodeID from t_FinTransCodes where TransCodeID in (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'TransCode' and comboitem = 'TransferFee') and transtypeid in (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'transcodetype' and comboitem = 'contracttrans')"
        'da.Fill(ds, "0")
        'Dim fintransid As Integer = ds.Tables("0").Rows(0)("FinTransCodeID")
        'ds.Tables("0").Clear()
        'Dim oInv As New clsInvoices
        'oInv.KeyField = "ContractID"
        'oInv.KeyValue = contID
        'oInv.ProspectID = 0
        'oInv.Amount = TransferFee
        'oInv.TransDate = Date.Now
        'oInv.FinTransID = fintransid
        ''rs.Fields("Reference") = "Transfer Fee"
        'oInv.UserID = UserID
        'oInv.DueDate = DateAdd(DateInterval.Day, 10, Date.Today)
        'oInv.Save()
        'oInv = Nothing

        '********************************************************************************************************* OLD STEP 3

        '******* If new prospect create new *******'
        'If request("ProspectID") = 0 Or request("ProspectID") = "" Then
        '    rs.Open("Select * from t_Prospect where 1=2", cn, 3, 3)
        '    rs.AddNew()
        '    rs.Fields("FirstName") = request("firstname")
        '    rs.Fields("LastName") = request("lastname")
        '    rs.Fields("MiddleInit") = request("middleinit")
        '    rs.Fields("SSN") = request("ssn")
        '    rs.Fields("Address1") = request("address1")
        '    rs.Fields("City") = request("city")
        '    rs.Fields("StateOrProvince") = request("stateorprovince")
        '    rs.Fields("PostalCode") = request("postalcode")
        '    rs.Fields("WorkPhone") = request("workphone")
        '    rs.Fields("Homephone") = request("homephone")
        '    rs.Fields("WorkExtension") = request("WorkExtension")
        '    rs.Fields("EmailName") = request("EmailName")
        '    rs.Fields("SpouseName") = request("spousename")
        '    rs.Fields("SpouseLastName") = request("spouselastname")
        '    rs.Fields("SpouseSSN") = request("SpouseSSN")
        '    rs.Fields("OwnerFlag") = 1
        '    rs.Fields("Country/Region") = request("[Country/Region]")
        '    rs.Updatebatch()
        '    rs.MoveFirst()
        '    prosID = rs.Fields("ProspectID")
        '    rs.Close()
        '    Create_Event("ProspectID", prosID, "", "", "Create", "")
        '    '******* Exisitng Owner so Update Information *****'
        'Else
        '    prosID = request("ProspectID")
        '    rs.Open("Select FirstName, LastName, MiddleInit, SSN, Address1, City, StateOrProvince, Homephone, PostalCode, WorkPhone, WorkExtension, EmailName, SpouseName, SpouseLastName, SpouseSSN from t_Prospect where prospectid = '" & request("ProspectID") & "'", cn, 3, 3)
        '    For i = 0 To rs.Fields.count - 1
        '        If CStr(rs.Fields(i).value & "") <> CStr(request(rs.Fields(i).name)) Then
        '            rs.fields(i) = request(rs.fields(i).name)
        '            If rs.fields(i).name = "StateOrProvince" Then
        '                Create_Event("ProspectID", prosID, Get_Lookup(rs.fields(i).value & ""), Get_Lookup(request(rs.fields(i).name)), "Change", rs.fields(i).name)
        '            Else
        '                Create_Event("ProspectID", prosID, rs.fields(i).value & "", request(rs.fields(i).name), "Change", rs.fields(i).name)
        '            End If
        '        End If
        '    Next
        '    rs.UPdatebatch()
        '    rs.Close()
        'End If

        ''******** Assign new prospectID to Contract and Mortgage and AccountItems*******'
        'rs.Open("Select * from t_Contract where contractid = '" & request("ContractID") & "'", cn, 3, 3)
        'rs.Fields("ProspectID") = prosID
        'rs.UpdateBatch()
        'rs.Close()

        'rs.OPen("Select * from t_Mortgage where mortgageid = '" & request("MortgageID") & "'", cn, 3, 3)
        'rs.Fields("ProspectID") = prosID
        'rs.UpdateBatch()
        'rs.Close()

        ''******* If Spouse as co owner checked then update t_ContractCoOwners table
        'If request("SpouseCoOwns") = "1" Then
        '    rs.open("Select * from t_ContractCoOwners where prospectid = '" & prosid & "' and contractid = '" & request("ContractID") & "'", cn, 3, 3)
        '    If rs.eof And rs.bof Then
        '        rs.addnew()
        '        rs.fields("ProspectID").value = prosid
        '        rs.fields("ContractID").value = request("ContractID")
        '        rs.update()
        '    End If
        '    rs.close()
        'End If

        ''****** If OwnerType is Company or Trust then fill in field in Contract table ********'
        'If request("ContractOwnerType") = "trust" Or request("ContractOwnerType") = "companyname" Then
        '    rs.open("Select * from t_Contract where ContractID = '" & request("ContractID") & "'", cn, 3, 3)
        '    If request("ContractOwnerType") = "trust" Then
        '        rs.fields("Trust").value = 1
        '        rs.fields("TrustName").value = request("ContractOwnerTypeValue")
        '    Else
        '        rs.Fields("companyname") = request("ContractOwnerTypeValue")
        '    End If
        '    rs.update()
        '    rs.close()
        'End If

        ''**** Update AccountItems *******'
        'rs.Open("Select * from t_AccountItems where contractid = '" & request("ContractID") & "'", cn, 3, 3)
        'Do While Not rs.EOF
        '    rs.Fields("ProspectID") = prosID
        '    rs.UpdateBatch()
        '    rs.MoveNext()
        'Loop
        'rs.Close()

        'response.write("OK|" & prosID)
        'response.end()
    End Sub

    Public Sub Save_Step4()
        '******** transfer any items marked for transfer *******'
        '******** Invoices require prospectid and contractid to be replaced ********'
        '******** Payments require contractid to be replaced ************'

        'If request("xferitems") = "" Then
        '    '**** NO Items to transfer
        'Else
        '    '***** Get prospectid from new contract *******'
        '    rs.Open("Select prospectid from t_Contract where contractid = '" & request("newContract") & "'", cn, 3, 3)
        '    prosID = rs.Fields("ProspectID")
        '    rs.Close()

        '    aItems = Split(request("xferItems"), "|")
        '    For i = 0 To UBound(aItems)
        '        '******* UPdate invoices ******'
        '        rs.Open("Select * from t_AccountItems where accountitemid = '" & aItems(i) & "'", cn, 3, 3)
        '        rs.Fields("ProspectID") = prosID
        '        rs.Fields("ContractID") = request("newContract")
        '        rs.UpdateBatch()
        '        rs.Close()
        '        '****** Update Payments ***********'
        '        rs.Open("Select * from t_AccountItems where accountitemid in (Select accountitemid from t_AccountItemsApplied where appliedtoItemID = '" & aItems(i) & "')", cn, 3, 3)
        '        Do While Not rs.EOF
        '            rs.Fields("ContractID") = request("newContract")
        '            rs.UpdateBatch()
        '            rs.MoveNext()
        '        Loop
        '        rs.Close()
        '    Next
        'End If
        'response.write("OK|" & prosID)
    End Sub


End Class
