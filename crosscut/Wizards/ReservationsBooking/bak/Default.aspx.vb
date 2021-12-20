Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports System.Web.UI

Partial Class Wizards_ReservationsBooking_Default
    Inherits System.Web.UI.Page

    Private cancelSearch As Boolean = True

    Public Property Prospect As WzImpProspect
    Public Property Email As WzEmail
    Public Property Address As WzAddress
    Public Property Phone As WzPhone
    Public Property Reservation As WzReservation
    Property Tour As WzTour
    Property PremiumIssued As WzPremiumIssued

    Private panels As List(Of Panel) = Nothing

    Delegate Sub Talk()
    Event evt As Talk

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'creates an array of existing Asp.net Panel controls
        panels = New List(Of Panel) _
            (New Panel() {Panel1, Panel2, Panel3, Panel4, Panel5, Panel6, Panel7,
            Panel8, Panel9, Panel10, Panel11, Panel12, Panel13, Panel14, Panel15})

        Array.ForEach(panels.ToArray(), Sub(p As Panel)
                                            'all panels are invisible
                                            p.Visible = False
                                            If panels.IndexOf(p) = 0 And IsPostBack = False Then
                                                'set panel 1 visible if not a postback
                                                p.Visible = True
                                            End If
                                        End Sub)

        If IsPostBack = False Then
            'appPath probably contains something like /crmsnet or /kcpcrms
            Dim appPath = Request.ApplicationPath
            'initialize the RadioButtonList_StartupOptions control, passing the vendor name if accessing from the site kcpcrms
            'or an empty string if accessing from crmsnet site
            Dim logonVendor = String.Empty

            If appPath.ToLower().IndexOf("crmsnet") < 1 And appPath.ToLower().IndexOf("kcpcrms") > 1 And Session("User") IsNot Nothing Then
                With New clsVendor
                    .VendorID = CType(Session("User"), User).ActiveVendor
                    .Load()
                    'stores logon vendor id in a hidden field for posterity
                    HiddenField_LogonVendorID.Value = .VendorID
                    logonVendor = .Vendor
                End With
            End If
            With RadioButtonList_StartupOptions
                .DataSource = StartupPackageChoices.ListChoices(logonVendor)
                .DataBind()
                'ensures the first option selected as a default
                .SelectedIndex = 0
            End With

            'why do I needs this hidden variable?
            'HiddenField_CurrentPanel.Value = Panel1.ID

            Init_DropDownLists()

            'Dim addr As IAddress = New WzImpAddress
            'Dim phone As IPhone = New WzImpPhone
            'Dim email As IEmail = New WzImpEmail
            'Dim p As IProspectID = New IImpProspectID With {.ProspectID = 8203336}
            'Prospect = New WzImpProspect(p, addr, phone, email)

            'Dim pr As IPackageReservation = New WzImpPackageReservation
            'pr.Load(1134)
            'Dim r As IReservation = New WzImpReservation()
            'Dim duration As IDuration = New WzImpDuration With {.StartDate = "10/5/2021", .Nights = 3}
            'Dim t As ITour = New WzImpTour
            'Reservation = r.Load(New WzImpDuration With {.StartDate = "10/5/2021", .Nights = 3}, pr, New WzImpTour)

            'Prospect.Reservation = Reservation

            'ViewState("Prospect") = Prospect


        Else
            'there are several panels representing each steps in each phase of the booking process,
            'depending on the phase and the package's type and its financial, 
            'those steps are either added or removed accordingly.
            Dim selectedOption = RadioButtonList_StartupOptions.SelectedItem.Text
            Dim wizardNav As IWzNavigate = New WzNavigation(New BookingPhase(selectedOption), 10, True)
            Dim wizardStepsIndex As List(Of Int32) = New List(Of Int32)

            For i = 0 To Wizard1.WizardSteps.Count - 1
                Dim isMatch = False
                For j = 0 To wizardNav.WizardSteps.Count - 1
                    If Wizard1.WizardSteps(i).Title = wizardNav.WizardSteps(j) Then
                        isMatch = True
                        Exit For
                    End If
                Next
                'if match, do not add to the array of integer
                If isMatch = False Then wizardStepsIndex.Add(i)
            Next
            'removes the wizard steps that not included in the array of strings in the WizardSteps function for the phase number
            For i = wizardStepsIndex.Count - 1 To 0 Step -1
                Wizard1.WizardSteps.RemoveAt(wizardStepsIndex(i))
            Next

        End If

        'If IsPostBack Then
        '    Prospect = ViewState("Prospect")


        '    Reservation = Prospect.Reservation

        '    Load_Data(Prospect)

        'End If

        'If IsPostBack = False Then


        '    RadioButtonList1.Items.Clear()

        '    For Each s As String In VendorSalePackages.ListPackages("Spinnaker")
        '        RadioButtonList1.Items.Add(New ListItem(s, s))
        '    Next
        'End If

        'If DropDownList_Search_Nights.Items.Count = 0 And Not RadioButtonList1.SelectedItem Is Nothing Then



        'End If








    End Sub


    Protected Sub Wizard1_ActiveStepChanged(sender As Object, e As System.EventArgs) Handles Wizard1.ActiveStepChanged
        Dim panel = CType(CType(Master.Master.FindControl("ContentPlaceHolder1").FindControl("ContentPlaceHolder2"), ContentPlaceHolder).FindControl("Panel" + (Wizard1.ActiveStepIndex + 1).ToString()), Panel)
        panel.Visible = True

        HiddenField_CurrentPanel.Value = panel.ID

        Response.Write(String.Format("<h2 style=color:red;>{0}</h2>", panel.ID))

        Dim data As IData = New ExcelData()

        If panel.ID = "Panel5" Then

            Dim gv As List(Of GridView) = New List(Of GridView)(New GridView() {GridView_Emails, GridView_Phones, GridView_Addresses})
            Dim vs As List(Of String) = New List(Of String)(New String() {"ViewState_Emails", "ViewState_Phones", "ViewState_Addresses"})

            Prospect.Addresses = data.GetAddresses
            Prospect.Phones = data.GetPhones
            Prospect.Emails = data.GetEmails

            For i As Int32 = 0 To vs.Count - 1
                Select Case i
                    Case 0
                        ViewState(vs(i)) = Prospect.Emails
                    Case 1
                        ViewState(vs(i)) = Prospect.Phones
                    Case 2
                        ViewState(vs(i)) = Prospect.Addresses
                    Case Else
                End Select
            Next
            gv.Zip(vs, Function(c As GridView, s As String)
                           c.DataSource = ViewState(s)
                           c.DataBind()
                           Return c
                       End Function).ToList()
        End If

    End Sub

    Private Sub Load_Data(prospect As WzImpProspect)
        With prospect
            TextBox_Prospect_ProspectID.Text = .ProspectID
            TextBox_Prospect_FirstName.Text = .FirstName
            TextBox_Prospect_LastName.Text = .LastName
            TextBox_SpouseLastName.Text = .Spouse.LastName
            TextBox_Spouse_FirstName.Text = .Spouse.FirstName

            If DropDownList_MaritalStatusID.Items.FindByValue(.MaritalStatusID) IsNot Nothing Then
                DropDownList_MaritalStatusID.Items.FindByValue(.MaritalStatusID).Selected = True
            End If

        End With

        With prospect.Reservation
            TextBox_ReservationID.Text = .ReservationID
            TextBox_Reservation_Number.Text = .ReservationNumber
            TextBox_Reservation_CheckIn.Text = .CheckIn
            TextBox_Reservation_CheckOut.Text = .CheckOut
            TextBox_Reservation_DateBooked.Text = .DateBooked
            TextBox_Reservation_StatusDate.Text = .StatusDate

            CheckBox_Reservation_LockInventory.Checked = .LockInventory

            If DropDownList_Reservation_StatusID.Items.FindByValue(.StatusID) IsNot Nothing Then
                DropDownList_Reservation_StatusID.Items.FindByValue(.StatusID).Selected = True
            End If
            If DropDownList_Reservation_SourceID.Items.FindByValue(.SourceID) IsNot Nothing Then
                DropDownList_Reservation_SourceID.Items.FindByValue(.SourceID).Selected = True
            End If

            If DropDownList_Reservation_Resort_CompanyID.Items.FindByValue(.ResortCompanyID) IsNot Nothing Then
                DropDownList_Reservation_Resort_CompanyID.Items.FindByValue(.ResortCompanyID).Selected = True
            End If
            If DropDownList_Reservation_LocationID.Items.FindByValue(.ResLocationID) IsNot Nothing Then
                DropDownList_Reservation_LocationID.Items.FindByValue(.ResLocationID).Selected = True
            End If

            If DropDownList_Reservation_Adults.Items.FindByValue(.Adults) IsNot Nothing Then
                DropDownList_Reservation_Adults.Items.FindByValue(.Adults).Selected = True
            End If
            If DropDownList_Reservation_Children.Items.FindByValue(.Children) IsNot Nothing Then
                DropDownList_Reservation_Children.Items.FindByValue(.Children).Selected = True
            End If

            If DropDownList_Reservation_Resort_CompanyID.Items.FindByValue(.ResortCompanyID) IsNot Nothing Then
                DropDownList_Reservation_Resort_CompanyID.Items.FindByValue(.ResortCompanyID).Selected = True
            End If
            If DropDownList_Reservation_TypeID.Items.FindByValue(.TypeID) IsNot Nothing Then
                DropDownList_Reservation_TypeID.Items.FindByValue(.TypeID).Selected = True
            End If
            If DropDownList_Reservation_SubTypeID.Items.FindByValue(.SubTypeID) IsNot Nothing Then
                DropDownList_Reservation_SubTypeID.Items.FindByValue(.SubTypeID).Selected = True
            End If
        End With

        With prospect.Reservation.Tour
            TextBox_TourID.Text = .TourID
            TextBox_TourDate.Text = .TourDate
            TextBox_BookingDate.Text = .BookingDate


            If DropDownList_TourStatusID.Items.FindByValue(.StatusID) IsNot Nothing Then
                DropDownList_TourStatusID.Items.FindByValue(.StatusID).Selected = True
            End If
            If DropDownList_CampaignID.Items.FindByValue(.CampaignID) IsNot Nothing Then
                DropDownList_CampaignID.Items.FindByValue(.CampaignID).Selected = True
            End If

            If DropDownList_TourSourceID.Items.FindByValue(.SourceID) IsNot Nothing Then
                DropDownList_TourSourceID.Items.FindByValue(.SourceID).Selected = True
            End If
            If DropDownList_TourTime.Items.FindByValue(.TourTimeID) IsNot Nothing Then
                DropDownList_TourTime.Items.FindByValue(.TourTimeID).Selected = True
            End If
            If DropDownList_TourLocationID.Items.FindByValue(.LocationID) IsNot Nothing Then
                DropDownList_TourLocationID.Items.FindByValue(.LocationID).Selected = True
            End If
            If DropDownList_TourTypeID.Items.FindByValue(.TypeID) IsNot Nothing Then
                DropDownList_TourTypeID.Items.FindByValue(.TypeID).Selected = True
            End If
            If DropDownList_TourSubTypeID.Items.FindByValue(.SubTypeID) IsNot Nothing Then
                DropDownList_TourSubTypeID.Items.FindByValue(.SubTypeID).Selected = True
            End If
        End With
    End Sub

    Private Class StartupPackageChoices

        Public Shared Function ListChoices(vendor As String) As List(Of String)
            Dim l As New List(Of String)
            'if no vendor supplied in the parameter, use the  means Club Explorer or VRC
            If vendor.Equals("Spinnaker") Then
                l.Add("Purchase A Tour Promotion, An Owner Advantage, An Owner Getaway Or Rental Packages")
                l.Add("Book A Tour Or A Tradeshow Package")
                l.Add("Schedule A Booked, Reset Or A Pending Reservation")
                l.Add("Purchase A Tour Package")

            ElseIf vendor.Length = 0 Then
                l.Add("Purchase A Tour Promotion, An Owner Advantage, An Owner Getaway Or Rental Packages")
                l.Add("Book A Tour Or A Tradeshow Package")
                l.Add("Schedule A Booked, Reset Or A Pending Reservation")

            ElseIf vendor.Equals("UP4ORCE") Or vendor.Equals("Global") Or vendor.Equals("FantaSea") Then
                l.Add("Purchase A 1-Night or A 2-Nights Tour Promotion Package")
                l.Add("Purchase A Tour Package")

            End If

            Return l
        End Function

    End Class
    Private Interface IDropDownNightsAvailable
        Function ListNights() As List(Of Integer)
    End Interface
    Private Class DropDownNightsAvailable
        Implements IDropDownNightsAvailable

        Private phaseSelected As String

        Public Sub New(phase As String)
            Me.phaseSelected = phase
        End Sub

        Public Function List() As List(Of Integer) Implements IDropDownNightsAvailable.ListNights

            If phaseSelected.Equals("Purchase A 1-Night or A 2-Nights Tour Promotion Package") Then
                Return Enumerable.Range(1, 2).ToList()
            ElseIf phaseSelected.Equals("Schedule A Booked, Reset Or A Pending Reservation") Then
                Return Enumerable.Range(2, 9).ToList()
            Else
                Return Enumerable.Range(2, 9).ToList()
            End If
        End Function
    End Class

    Private Interface IBookingPhase
        Function PhaseNumber() As Int32
    End Interface
    Private Class BookingPhase
        Implements IBookingPhase

        Private selectedPhase As String
        Public Sub New(phase As String)
            selectedPhase = phase
        End Sub
        Public Function PhaseNumber() As Integer Implements IBookingPhase.PhaseNumber
            If selectedPhase.Equals("Purchase A Tour Promotion, An Owner Advantage, An Owner Getaway Or Rental Packages") Or selectedPhase.Equals("Purchase A Tour Package") Or selectedPhase.Equals("Purchase A 1-Night or A 2-Nights Tour Promotion Package") Then
                Return 1
            ElseIf selectedPhase.Equals("Book A Tour Or A Tradeshow Package") Then
                Return 2
            ElseIf selectedPhase.Equals("Schedule A Booked, Reset Or A Pending Reservation") Then
                Return 3
            Else
                Return 0
            End If
        End Function
    End Class
    Private Interface IWzNavigate
        Function WizardSteps() As List(Of String)
    End Interface
    Private Class WzNavigation
        Implements IWzNavigate

        Private iBookingPhase As IBookingPhase
        Private balance As Decimal
        Private isResort As Boolean

        Public Sub New(iBookingPhase As IBookingPhase, balance As Decimal, isResort As Boolean)
            Me.iBookingPhase = iBookingPhase
            Me.balance = balance
            Me.isResort = isResort
        End Sub

        Public Function WizardSteps() As List(Of String) Implements IWzNavigate.WizardSteps
            Dim l = New List(Of String)

            l.Add("Welcome")

            If iBookingPhase.PhaseNumber().Equals(1) Then
                l.Add("Search Available Packages For Purchasing")
                l.Add("Search For Prospect")
                l.Add("Edit Prospect")
                l.Add("Edit Reservation")
                l.Add(IIf(isResort, "Allocate Rooms", "Edit Hotel"))
                l.Add("Edit Notes")

                If balance = 10 Then
                    l.Add("Confirmation")
                End If
            ElseIf iBookingPhase.PhaseNumber().Equals(2) Then


            End If
            Return l
        End Function
    End Class

    Protected Sub GridView_Search_Available_Packages_For_Purchasing_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView_Search_Available_Packages_For_Purchasing.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'insert the packageID as the attribute of the checkbox
            With CType(e.Row.Cells(0).FindControl("CheckBox7"), CheckBox)
                .Attributes.Add("PackageID", CType(e.Row.DataItem, DataRowView)("PackageID"))
            End With
            With CType(e.Row.Cells(4).FindControl("DropDownList1"), DropDownList)
                If CType(e.Row.DataItem, DataRowView)("TourRequired").ToString = 0 Then
                    'use RoomsAvail column to quickly insert integer to the drop down control
                    .DataSource = Enumerable.Range(1, Int32.Parse(CType(e.Row.DataItem, DataRowView)("RoomsAvail")))
                    .DataBind()
                Else
                    .Items.Clear()
                    .Items.Add("1")
                End If
            End With
        End If
    End Sub

    Protected Sub Button_Search_Available_Packages_For_Purchasing_Click(sender As Object, e As EventArgs) Handles Button_Search_Available_Packages_For_Purchasing.Click
        'cancelSearch is used to stop the ObjectDataSource from running its Selecting event at startup

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand
                Try
                    cn.Open()
                    cm.Connection = cn
                    cm.CommandText = "select * from Online order by PackageId"
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    dt.Columns.Add("UnitType")
                    dt.Columns.Add("PackageSubType")
                    dt.Columns.Add("MinCost")
                    dt.Columns.Add("MaxCost")

                    Dim l = New List(Of Int32)
                    Array.ForEach(dt.Rows.OfType(Of DataRow).ToArray(),
                        Sub(dr As DataRow)
                            l.Add(Int32.Parse(dr("PackageID").ToString))
                        End Sub)
                    cm.CommandText = String.Format("select p.PackageID, pst.ComboItem 'PackageSubType', ut.ComboItem 'UnitType' from t_Package p left join t_ComboItems pst on pst.ComboItemID = p.SubTypeID left join t_ComboItems ut on ut.ComboItemID = p.UnitTypeID where p.PackageID in ({0}) order by p.PackageID;", String.Join(",", l.ToArray()))
                    Dim dt2 = New DataTable
                    dt2.Load(cm.ExecuteReader())
                    For Each r As DataRow In dt.Rows
                        For Each dr As DataRow In dt2.Rows
                            If Int32.Parse(r("PackageID")) = Int32.Parse(dr("PackageID")) Then
                                r("UnitType") = dr("UnitType")
                                r("PackageSubType") = dr("PackageSubType").ToString
                                Dim rdn = New Random
                                r("MinCost") = rdn.Next(1, 200)
                                r("MaxCost") = rdn.Next(200, 532)
                            End If
                        Next
                    Next

                    Dim f As Func(Of DropDownList, List(Of String)) =
                        Function(ddl As DropDownList)
                            If ddl.SelectedItem.Text = "(All)" Then
                                Dim ll = ddl.Items.OfType(Of ListItem).Where(Function(x) x.Text <> "(All)").Select(Function(x) x.Text).ToList()
                                ll.Add(String.Empty)
                                Return ll
                            Else
                                Return New List(Of String)(New String() {ddl.SelectedItem.Text})
                            End If
                        End Function

                    Dim d = dt.Rows.OfType(Of DataRow).Where(Function(x) Array.IndexOf(f(DropDownList_RoomSizes).ToArray(), x("RoomType")) >= 0 And
                                                                 Array.IndexOf(f(DropDownList_PackageTypes).ToArray(), x("PackageType")) >= 0 And
                                                                 Array.IndexOf(f(DropDownList_PackageSubTypes).ToArray(), x("PackageSubType")) >= 0 And
                                                                 Array.IndexOf(f(DropDownList_UnitTypes).ToArray(), x("UnitType")) >= 0)
                    If d.Count() > 0 Then
                        GridView_Search_Available_Packages_For_Purchasing.DataSource = d.CopyToDataTable().Rows.OfType(Of DataRow).OrderBy(Function(x) x.Item("PackageType")).ThenBy(Function(x) x.Item("Description")).CopyToDataTable()
                        GridView_Search_Available_Packages_For_Purchasing.DataBind()
                    Else
                        GridView_Search_Available_Packages_For_Purchasing.DataSource = Nothing
                        GridView_Search_Available_Packages_For_Purchasing.DataBind()
                    End If


                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Using


        panels(Integer.Parse(Regex.Replace(HiddenField_CurrentPanel.Value, "[^\d]", "")) - 1).Visible = True
    End Sub







    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs)

        Dim gvr = CType(CType(sender, LinkButton).NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        HiddenFieldCurrentEditedEmailID.Value = gv.DataKeys(gvr.RowIndex).Value

    End Sub

    Private Sub a()
        Dim tb = New List(Of TextBox)(New TextBox() {
            TextBox_ReservationID,
            TextBox_Reservation_Number,
            TextBox_Reservation_CheckIn,
            TextBox_Reservation_CheckOut,
            TextBox_Reservation_DateBooked,
            TextBox_Reservation_StatusDate
        })
        Dim dl = New List(Of DropDownList)(New DropDownList() {
            DropDownList_Reservation_LocationID,
            DropDownList_Reservation_Resort_CompanyID,
            DropDownList_Reservation_SourceID,
            DropDownList_Reservation_StatusID,
            DropDownList_Reservation_TypeID,
            DropDownList_Reservation_SubTypeID,
            DropDownList_Reservation_Total_Nights
        })

        Dim cb = New List(Of CheckBox)(New CheckBox() {CheckBox_Reservation_LockInventory})
    End Sub


#Region "ObjectDataSource controls"

    Protected Sub ObjectDataSource_Prospect_Selecting(sender As Object, e As ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource_Prospect.Selecting
        '
        e.Cancel = IIf(Int32.Parse(e.InputParameters("ProspectID").ToString()) <> 0, True, False)





        'Dim id = tx.Text

    End Sub


#End Region


#Region "Private common functions"
    'helper function called by another function to recursively loop through all control types to reset the content
    Private Iterator Function EnumerateControlsRecursive(ByVal parent As Control) As IEnumerable(Of Control)
        For Each child As Control In parent.Controls
            Yield child

            For Each descendant As Control In EnumerateControlsRecursive(child)
                Yield descendant
            Next
        Next
    End Function
    Private Sub Reset_Controls_Default()
        For Each ctl As Control In EnumerateControlsRecursive(Page)
            If TypeOf ctl Is TextBox Then
                CType(ctl, TextBox).Text = String.Empty
            ElseIf TypeOf ctl Is DropDownList Then
                CType(ctl, DropDownList).ClearSelection()
                CType(ctl, DropDownList).SelectedIndex = 0
            ElseIf TypeOf ctl Is CheckBox Then
                CType(ctl, CheckBox).Checked = False
            End If
        Next
    End Sub
    Private Sub Init_DropDownLists()

        Dim ddl = New List(Of DropDownList)(New DropDownList() {
            DropDownList_MaritalStatusID,
            DropDownList_Phone_TypeID,
            DropDownList_StateID,
            DropDownList_CountryID,
            DropDownList_Address_TypeID,
            DropDownList_PremiumStatusID,
            DropDownList_TourLocationID,
            DropDownList_TourSubTypeID,
            DropDownList_TourTime,
            DropDownList_TourSourceID,
            DropDownList_TourTypeID,
            DropDownList_TourStatusID,
            DropDownList_Reservation_SourceID,
            DropDownList_Reservation_SubTypeID,
            DropDownList_Reservation_TypeID,
            DropDownList_Reservation_StatusID,
            DropDownList_Reservation_Resort_CompanyID,
            DropDownList_Reservation_LocationID,
            DropDownList_PackageTypes,
            DropDownList_PackageSubTypes,
            DropDownList_UnitTypes
        })

        Array.ForEach(ddl.ToArray(), Sub(dd As DropDownList) dd.ClearSelection())
        Dim cb = New List(Of String)(New String() {"MaritalStatus", "Phone", "State", "Country", "AddressType", "PremiumStatus", "TourLocation", "TourSubType", "TourTime", "TourSource", "TourType", "TourStatus", "ReservationSource", "ReservationSubType", "ReservationType", "ReservationStatus", "ResortCompany",
                                     "ReservationLocation", "PackageType", "PackageSubType", "UnitType"})

        ddl.Zip(cb, Function(x As DropDownList, y As String)
                        x.AppendDataBoundItems = True
                        x.Items.Add(New ListItem("(All)", ""))
                        x.DataSource = CType(New clsComboItems().Load_ComboItems(y).Select(DataSourceSelectArguments.Empty), DataView).ToTable()
                        x.DataTextField = "ComboItem"
                        x.DataValueField = "ComboItemID"
                        x.DataBind()
                        Return x
                    End Function).ToList()

    End Sub
    'all panels hidden and un-hide the current one tracked by the hidden control
    Private Sub Unhide_Current_Panel()
        Array.ForEach(panels.ToArray(), Sub(p As Panel)
                                            If p.ID = HiddenField_CurrentPanel.Value Then
                                                p.Visible = True
                                            End If
                                        End Sub)
    End Sub
    'do not use right now but may delete later
    Private Function GetComboItemAsString(comboItemID As Int32) As String
        Dim d As IData = New ExcelData
        Dim l = d.GetDataComboItem(String.Empty)
        Dim r = l.Where(Function(x) x.ComboItemID = comboItemID).SingleOrDefault()
        If r IsNot Nothing Then
            Return r.ComboItem
        Else
            Return String.Empty
        End If
    End Function
    Private Sub TextBoxes_Init(list As List(Of TextBox))
        For Each tb As TextBox In list
            tb.Text = String.Empty
        Next
    End Sub
    Private Sub DropDownlists_Init(list As List(Of DropDownList))
        For Each dd As DropDownList In list
            dd.ClearSelection()
        Next
    End Sub
    Private Sub CheckBoxes_Init(list As List(Of CheckBox))
        For Each cb As CheckBox In list
            cb.Checked = False
        Next
    End Sub

#End Region

    Protected Sub Button_Reset_Controls_Click(sender As Object, e As EventArgs) Handles Button_Reset_Controls.Click
        Reset_Controls_Default()

    End Sub

    Protected Sub LinkButton_Email_Edit_Click(sender As Object, e As EventArgs)
        Dim lk = CType(sender, LinkButton)
        Dim gvr = CType(lk.NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Email_Address})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Email_IsPrimary, CheckBox_Email_IsActive})

        Email = Prospect.Emails.Where(Function(x) x.SID = gv.DataKeys(gvr.RowIndex).Value).Single()
        Label_Edit_Email.Attributes.Add("SID", gv.DataKeys(gvr.RowIndex).Value)

        txb.ForEach(Sub(c As TextBox) c.DataBind())
        ckb.ForEach(Sub(c As CheckBox) c.DataBind())

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Email)
        Unhide_Current_Panel()
    End Sub
    Protected Sub LinkButton_Phone_Edit_Click(sender As Object, e As EventArgs)
        Dim lk = CType(sender, LinkButton)
        Dim gvr = CType(lk.NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Phone_Number, TextBox_Phone_Extension})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Phone_Active})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_Phone_TypeID})

        ddl.ForEach(Sub(c As DropDownList) c.ClearSelection())

        Phone = Prospect.Phones.Where(Function(x) x.SID = gv.DataKeys(gvr.RowIndex).Value).Single()
        Label_Edit_Phone.Attributes.Add("SID", gv.DataKeys(gvr.RowIndex).Value)

        If DropDownList_Phone_TypeID.Items.FindByValue(Phone.TypeID) IsNot Nothing Then
            DropDownList_Phone_TypeID.Items.FindByValue(Phone.TypeID).Selected = True
        End If

        txb.ForEach(Sub(c As TextBox) c.DataBind())
        ddl.ForEach(Sub(c As DropDownList) c.DataBind())
        ckb.ForEach(Sub(c As CheckBox) c.DataBind())

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Phone)
        Unhide_Current_Panel()
    End Sub
    Protected Sub LinkButton_Address_Edit_Click(sender As Object, e As EventArgs)
        Dim lk = CType(sender, LinkButton)
        Dim gvr = CType(lk.NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Address1, TextBox_Address2, TextBox_City, TextBox_PostalCode, TextBox_Regiion})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_ActiveFlag, CheckBox_ContractAddress})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_CountryID, DropDownList_StateID, DropDownList_Address_TypeID})
        Dim lbl = New List(Of Label)(New Label() {Label_Edit_Address})

        ddl.ForEach(Sub(c As DropDownList) c.ClearSelection())

        Address = Prospect.Addresses.Where(Function(x) x.SID = gv.DataKeys(gvr.RowIndex).Value).Single()
        Label_Edit_Address.Attributes.Add("SID", gv.DataKeys(gvr.RowIndex).Value)

        If DropDownList_CountryID.Items.FindByValue(Address.CountryID) IsNot Nothing Then
            DropDownList_CountryID.Items.FindByValue(Address.CountryID).Selected = True
        End If
        If DropDownList_StateID.Items.FindByValue(Address.StateID) IsNot Nothing Then
            DropDownList_StateID.Items.FindByValue(Address.StateID).Selected = True
        End If
        If DropDownList_Address_TypeID.Items.FindByValue(Address.TypeID) IsNot Nothing Then
            DropDownList_Address_TypeID.Items.FindByValue(Address.TypeID).Selected = True
        End If

        txb.ForEach(Sub(c As TextBox) c.DataBind())
        ddl.ForEach(Sub(c As DropDownList) c.DataBind())
        ckb.ForEach(Sub(c As CheckBox) c.DataBind())
        lbl.ForEach(Sub(c As Label) c.DataBind())

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Address)
        Unhide_Current_Panel()
    End Sub

    Protected Sub Button_Edit_Email_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_Email_Submit.Click
        Dim sid = Label_Edit_Email.Attributes("SID")
        Email = Prospect.Emails.Where(Function(x) x.SID = sid).SingleOrDefault()

        If Email Is Nothing Then
            Email = New WzEmail
            Prospect.Emails.Add(Email)
        End If

        With Email
            .Address = TextBox_Email_Address.Text.Trim()
            .IsActive = CheckBox_Email_IsActive.Checked
            .IsPrimary = CheckBox_Email_IsPrimary.Checked
        End With

        ViewState("Prospect") = Prospect
        GridView_Emails.DataSource = Prospect.Emails.ToList()
        GridView_Emails.DataBind()
        Label_Edit_Email.Attributes.Remove("SID")
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
        Unhide_Current_Panel()
    End Sub
    Protected Sub Button_Edit_Phone_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_Phone_Submit.Click
        Dim sid = Label_Edit_Phone.Attributes("SID")
        Phone = Prospect.Phones.Where(Function(x) x.SID = sid).SingleOrDefault()
        If Phone Is Nothing Then
            Phone = New WzPhone
            Prospect.Phones.Add(Phone)
        End If

        With Phone
            If DropDownList_Phone_TypeID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_Phone_TypeID.SelectedItem.Value) = False Then
                    .TypeID = DropDownList_Phone_TypeID.SelectedItem.Value
                End If
            End If
            .Type = GetComboItemAsString(.TypeID)
            .Extension = TextBox_Phone_Extension.Text.Trim
            .Number = TextBox_Phone_Number.Text.Trim
            .Active = CheckBox_Phone_Active.Checked
        End With
        ViewState("Prospect") = Prospect
        GridView_Phones.DataSource = Prospect.Phones.ToList()
        GridView_Phones.DataBind()
        Label_Edit_Phone.Attributes.Remove("SID")
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
        Unhide_Current_Panel()
    End Sub
    Protected Sub Button_Edit_Address_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_Address_Submit.Click
        Dim sid = Label_Edit_Address.Attributes("SID")
        Address = Prospect.Addresses.Where(Function(x) x.SID = sid).SingleOrDefault()
        If Address Is Nothing Then
            Address = New WzAddress
            Prospect.Addresses.Add(Address)
        End If
        With Address
            If DropDownList_Address_TypeID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_StateID.SelectedItem.Value) = False Then
                    .StateID = DropDownList_StateID.SelectedItem.Value
                End If
            End If
            If DropDownList_Address_TypeID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_Address_TypeID.SelectedItem.Value) = False Then
                    .TypeID = DropDownList_Address_TypeID.SelectedItem.Value
                End If
            End If
            If DropDownList_CountryID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_CountryID.SelectedItem.Value) = False Then
                    .CountryID = DropDownList_CountryID.SelectedItem.Value
                End If
            End If
            .Address1 = TextBox_Address1.Text.Trim()
            .Address2 = TextBox_Address2.Text.Trim()
            .City = TextBox_City.Text.Trim()
            .Region = TextBox_Regiion.Text.Trim()
            .PostalCode = TextBox_PostalCode.Text.Trim()
            .State = GetComboItemAsString(.StateID)
            .ActiveFlag = CheckBox_ActiveFlag.Checked
            .ContractAddress = CheckBox_ContractAddress.Checked
        End With

        ViewState("Prospect") = Prospect
        GridView_Addresses.DataSource = Prospect.Addresses.ToList()
        GridView_Addresses.DataBind()
        Label_Edit_Address.Attributes.Remove("SID")
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
        Unhide_Current_Panel()
    End Sub
    Protected Sub Button_Edit_Email_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_Email_Cancel.Click
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
        Unhide_Current_Panel()
    End Sub
    Protected Sub Button_Edit_Phone_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_Phone_Cancel.Click
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
        Unhide_Current_Panel()
    End Sub
    Protected Sub Button_Edit_Address_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_Address_Cancel.Click
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
        Unhide_Current_Panel()
    End Sub

    Protected Sub LinkButton_Address_Add_Click(sender As Object, e As EventArgs)
        Address = New WzAddress
        Label_Edit_Address.Attributes.Add("SID", Address.SID)
        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Address1, TextBox_Address2, TextBox_City, TextBox_PostalCode, TextBox_Regiion})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_ActiveFlag, CheckBox_ContractAddress})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_CountryID, DropDownList_StateID, DropDownList_Address_TypeID})
        TextBoxes_Init(txb)
        CheckBoxes_Init(ckb)
        DropDownlists_Init(ddl)
        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Address)
        Unhide_Current_Panel()
    End Sub
    Protected Sub LinkButton_Phone_Add_Click(sender As Object, e As EventArgs)
        Phone = New WzPhone
        Label_Edit_Phone.Attributes.Add("SID", Phone.SID)
        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Phone_Number, TextBox_Phone_Extension})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Phone_Active})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_Phone_TypeID})
        TextBoxes_Init(txb)
        CheckBoxes_Init(ckb)
        DropDownlists_Init(ddl)
        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Phone)
        Unhide_Current_Panel()
    End Sub
    Protected Sub LinkButton_Email_Add_Click(sender As Object, e As EventArgs)
        Email = New WzEmail
        Label_Edit_Email.Attributes.Add("SID", Email.SID)
        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Email_Address})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Email_IsActive, CheckBox_Email_IsPrimary})
        TextBoxes_Init(txb)
        CheckBoxes_Init(ckb)

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Email)
        Unhide_Current_Panel()
    End Sub

#Region "Supporting classes"

    Public Interface IData
        Function GetData() As List(Of WizardSalesPackage)
        Function GetDataComboItem(comboName As String) As List(Of WizardComboItem)
        Function GetComboItemAsString(comboItemID As Int32) As String
        Function GetEmails() As List(Of WzEmail)
        Function GetPhones() As List(Of WzPhone)
        Function GetAddresses() As List(Of WzAddress)
    End Interface
    Public Class ExcelData
        Implements IData

        Private cs As String = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0;", "c:\data\Book1.xlsx")

        Public Function GetData() As List(Of WizardSalesPackage) Implements IData.GetData
            Dim l As New List(Of WizardSalesPackage)

            Dim cn = New OleDbConnection(cs)
            cn.Open()

            Dim ad = New OleDbDataAdapter
            Dim cm = New OleDbCommand("SELECT * FROM [Sheet1$]", cn)
            Dim ds = New DataSet
            ad.SelectCommand = cm

            ad.Fill(ds, "xlxsData")

            For Each row As DataRow In ds.Tables(0).Rows
                l.Add(New WizardSalesPackage With {
                      .PackageID = row("PackageID"),
                      .AccomName = row("AccomName"),
                      .Description = row("Description"),
                      .TourRequired = row("TourRequired"),
                      .RoomType = row("RoomType"),
                      .RoomsAvail = row("RoomAvail"),
                      .PackageType = row("PackageType"),
                      .PackageSubType = row("PackageSubType"),
                      .MaxCost = 1.01})
            Next
            cn.Close()

            Return l
        End Function

        Private Function GetComboItems() As List(Of WizardComboItem)
            Dim l = New List(Of WizardComboItem)
            Dim err = String.Empty

            Using cn = New OleDbConnection(cs)
                Try
                    cn.Open()
                    Dim ad = New OleDbDataAdapter
                    Dim cm = New OleDbCommand("SELECT * FROM [ComboItem$]", cn)
                    Dim ds = New DataSet
                    ad.SelectCommand = cm

                    ad.Fill(ds, "ComboItem")

                    cm = New OleDbCommand("SELECT * FROM [Combo$]", cn)
                    ad.SelectCommand = cm
                    Dim tb = New DataTable("Combo")
                    tb.Load(cm.ExecuteReader())
                    ds.Tables.Add(tb)

                    ds.Tables(0).PrimaryKey = New DataColumn() {ds.Tables(0).Columns("ComboItemID")}
                    ds.Tables(1).PrimaryKey = New DataColumn() {ds.Tables(1).Columns("ComboID")}

                    ds.EnforceConstraints = False
                    Dim r = ds.Relations.Add(ds.Tables("Combo").Columns("ComboID"), ds.Tables("ComboItem").Columns("ComboID"))

                    For Each ci As DataRow In ds.Tables("ComboItem").Rows
                        Dim combo_name = String.Empty
                        Dim c = ds.Tables("Combo").Rows.OfType(Of DataRow).Where(Function(x) Int32.Parse(x("ComboID")) = Int32.Parse(ci("ComboID"))).SingleOrDefault()
                        If c IsNot Nothing Then
                            combo_name = c("ComboName").ToString()
                        End If
                        l.Add(New WizardComboItem With {.ComboItemID = ci("ComboItemID"), .ComboItem = ci("ComboItem"), .ComboID = ci("ComboID"), .ComboName = combo_name})
                    Next
                Catch ex As Exception
                    err = ex.Message
                Finally
                    cn.Close()
                End Try
            End Using
            HttpContext.Current.Session("Session_ComboItems") = l
            Return l
        End Function

        Public Function GetDataComboItem(comboName As String) As List(Of WizardComboItem) Implements IData.GetDataComboItem
            Dim l As List(Of WizardComboItem) = Nothing

            'HttpContext.Current.Session("Session_ComboItems") = Nothing

            If HttpContext.Current.Session("Session_ComboItems") IsNot Nothing Then
                l = CType(HttpContext.Current.Session("Session_ComboItems"), List(Of WizardComboItem))
            Else
                l = GetComboItems()
            End If
            Return l.Where(Function(x) x.ComboName.ToLower() = comboName.ToLower()).ToList()
        End Function

        Public Function GetEmails() As List(Of WzEmail) Implements IData.GetEmails
            Dim l = New List(Of WzEmail)

            l.Add(New WzEmail With {.EmailID = 16486825, .Address = "ihclaye@gmail.com", .IsActive = True, .IsPrimary = True})
            l.Add(New WzEmail With {.EmailID = 16488637, .Address = "iclaye@its.jnj.com", .IsActive = True, .IsPrimary = False})
            l.Add(New WzEmail With {.EmailID = 16488649, .Address = "dllhc88@gmail.com", .IsActive = True, .IsPrimary = False})
            Return l
        End Function

        Public Function GetPhones() As List(Of WzPhone) Implements IData.GetPhones
            Dim l = New List(Of WzPhone)
            Dim p = New WzPhone With {.PhoneID = 4395016, .Number = "7578713083", .Extension = "Peter Cell ", .TypeID = 9113, .Active = True}
            p.Type = GetComboItemAsString(p.TypeID)
            l.Add(p)

            p = New WzPhone With {.PhoneID = 12388475, .Number = "", .Extension = "", .TypeID = 9112, .Active = False}
            p.Type = GetComboItemAsString(p.TypeID)
            l.Add(p)

            p = New WzPhone With {.PhoneID = 12388476, .Number = "7578709240", .Extension = "Robin Cell", .TypeID = 34314, .Active = True}
            p.Type = GetComboItemAsString(p.TypeID)
            l.Add(p)

            p = New WzPhone With {.PhoneID = 12388478, .Number = "8046429800", .Extension = "Peter Work", .TypeID = 9114, .Active = True}
            p.Type = GetComboItemAsString(p.TypeID)
            l.Add(p)

            Return l
        End Function

        Public Function GetAddresses() As List(Of WzAddress) Implements IData.GetAddresses
            Dim l = New List(Of WzAddress)

            l.Add(New WzAddress With {
                .AddressID = 14424826,
                .Address1 = "102 Deal",
                .Address2 = "",
                .City = "Williamsburg",
                .StateID = 0,
                .PostalCode = "23188",
                .Region = "",
                .CountryID = 0,
                .TypeID = 17073,
                .State = GetComboItemAsString(.TypeID),
                .ActiveFlag = True,
                .ContractAddress = True
            })
            Return l
        End Function


        Public Function GetComboItemAsString(comboItemID As Int32) As String Implements IData.GetComboItemAsString
            Dim l As List(Of WizardComboItem) = Nothing

            If HttpContext.Current.Session("Session_ComboItems") IsNot Nothing Then
                l = CType(HttpContext.Current.Session("Session_ComboItems"), List(Of WizardComboItem))
            Else
                l = GetDataComboItem(String.Empty)
            End If

            Dim r = l.Where(Function(x) x.ComboItemID = comboItemID).SingleOrDefault()
            If r IsNot Nothing Then
                Return r.ComboItem
            Else
                Return String.Empty
            End If
        End Function
    End Class
    Public Class WizardComboItem
        Public Property ComboItemID As Int32
        Public Property ComboItem As String
        Public Property ComboID As Int32
        Public Property ComboName As String
    End Class

#Region "Prospect"
    <Serializable>
    Class WzImpProspect
        Implements IProspect

        Public Property Spouse As IPerson Implements IProspect.Spouse
        Public Property Addresses As List(Of WzAddress) Implements IProspect.Addresses
        Public Property Emails As List(Of WzEmail) Implements IProspect.Emails
        Public Property Phones As List(Of WzPhone) Implements IProspect.Phones
        Public Property FirstName As String Implements IPerson.FirstName
        Public Property LastName As String Implements IPerson.LastName
        Public Property MaritalStatusID As Integer Implements IMaritalStatusID.MaritalStatusID
        Public Property ProspectID As Integer Implements IProspectID.ProspectID

        Property Reservation As WzReservation
        Public ReadOnly Property FullName As String
            Get
                Return String.Format("{0} {1}", FirstName, LastName).ToUpper()
            End Get
        End Property

        Public Sub New(prospect As IProspectID, address As IAddress, phone As IPhone, email As IEmail)
            Me.ProspectID = prospect.ProspectID
            Addresses = New List(Of WzAddress)
            Phones = New List(Of WzPhone)
            Emails = New List(Of WzEmail)
            Spouse = New WzImpSpouse
            If prospect.ProspectID > 0 Then
                With New clsProspect
                    .Prospect_ID = prospect.ProspectID
                    .Load()
                    FirstName = .First_Name
                    LastName = .Last_Name
                    MaritalStatusID = .MaritalStatusID
                    Spouse.FirstName = .SpouseFirstName
                    Spouse.LastName = .SpouseLastName
                End With

                Addresses = address.Load(prospect)
                Phones = phone.Load(prospect)
                Emails = email.Load(prospect)
            End If
        End Sub
    End Class
    Class IImpProspectID
        Implements IProspectID
        Public Property ProspectID As Integer Implements IProspectID.ProspectID
    End Class
    Interface IProspectID
        Property ProspectID As Int32
    End Interface
    Interface IPerson
        Property FirstName As String
        Property LastName As String
    End Interface
    Interface IMaritalStatusID
        Property MaritalStatusID As Int32
    End Interface
    Interface IPrimaryOwner
        Inherits IPerson
        Inherits IMaritalStatusID
        Inherits IProspectID
    End Interface
    Interface IProspect
        Inherits IPrimaryOwner

        Property Spouse As IPerson
        Property Addresses As List(Of WzAddress)
        Property Emails As List(Of WzEmail)
        Property Phones As List(Of WzPhone)
    End Interface
    <Serializable>
    Class WzImpSpouse
        Implements IPerson

        Public Property FirstName As String Implements IPerson.FirstName
        Public Property LastName As String Implements IPerson.LastName

    End Class
#End Region
#Region "Prospect Emails"
    <Serializable>
    Public Class WzEmail
        Public Property SID As String = Guid.NewGuid.ToString()
        Public Property EmailID As Int32
        Public Property Address As String
        Public Property IsPrimary As Boolean
        Public Property IsActive As Boolean
    End Class
    Interface IEmail
        Function Load(prospect As IProspectID) As List(Of WzEmail)
    End Interface
    Class WzImpEmail
        Inherits ConnectionInfo
        Implements IEmail

        Private Function IEmail_Load(prospect As IProspectID) As List(Of WzEmail) Implements IEmail.Load
            Dim l As New List(Of WzEmail)
            Dim dt = CType(New clsEmail With {.ProspectID = prospect.ProspectID}.List().Select(DataSourceSelectArguments.Empty), DataView).ToTable
            For Each row As DataRow In dt.Rows
                l.Add(New WzEmail With {.IsPrimary = row("Primary").ToString, .Address = row("Email").ToString, .IsActive = row("Active").ToString, .EmailID = row("ID")})
            Next
            Return l
        End Function
    End Class
#End Region
#Region "Prospect Phones"
    <Serializable>
    Public Class WzPhone
        Public Property SID As String = Guid.NewGuid.ToString()
        Public Property PhoneID As Int32
        Public Property Number As String
        Public Property Extension As String
        Public Property TypeID As String
        Public Property Type As String
        Public Property Active As Boolean
    End Class
    Interface IPhone
        Function Load(prospect As IProspectID) As List(Of WzPhone)
    End Interface
    Class WzImpPhone
        Inherits ConnectionInfo
        Implements IPhone

        Public Function Load(prospect As IProspectID) As List(Of WzPhone) Implements IPhone.Load
            Dim l = New List(Of WzPhone)
            Dim dt = CType(New clsPhone With {.ProspectID = prospect.ProspectID}.List().Select(DataSourceSelectArguments.Empty), DataView).ToTable
            For Each row As DataRow In dt.Rows
                l.Add(New WzPhone With {.PhoneID = row("ID"), .Number = row("Number").ToString, .Extension = row("Extension").ToString, .Active = row("Active").ToString, .Type = row("Type").ToString, .TypeID = New clsComboItems().Lookup_ID("PhoneType", row("Type").ToString)})
            Next
            Return l
        End Function
    End Class
#End Region
#Region "Prospect Addresses"
    <Serializable>
    Class WzAddress
        Public Property SID As String = Guid.NewGuid.ToString()
        Public Property AddressID As Integer
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property City As String
        Public Property StateID As Integer
        Public Property State As String
        Public Property PostalCode As String
        Public Property Region As String
        Public Property TypeID As Integer
        Public Property Type As String
        Public Property CountryID As Integer
        Public Property ActiveFlag As Boolean
        Public Property ContractAddress As Boolean

    End Class
    Interface IAddress
        Function Load(prospect As IProspectID) As List(Of WzAddress)
    End Interface
    Class WzImpAddress
        Inherits ConnectionInfo
        Implements IAddress

        Public Function Load(prospect As IProspectID) As List(Of WzAddress) Implements IAddress.Load
            Dim l = New List(Of WzAddress)
            Dim dt = New DataTable
            dt = CType(New clsAddress() With {.ProspectID = prospect.ProspectID}.List().Select(DataSourceSelectArguments.Empty), DataView).ToTable

            For Each row As DataRow In dt.Rows
                Dim add = New clsAddress
                add.AddressID = row("ID")
                add.Load()

                l.Add(New WzAddress With {
                    .AddressID = row("ID"),
                    .Address1 = row("Address1").ToString,
                    .Address2 = row("Address2").ToString,
                    .City = row("City").ToString,
                    .State = row("State").ToString,
                    .StateID = New clsComboItems().Lookup_ID("State", row("State").ToString),
                    .PostalCode = row("Zip").ToString,
                    .CountryID = New clsComboItems().Lookup_ID("Country", row("Country").ToString),
                    .ActiveFlag = row("ActiveFlag").ToString,
                    .ContractAddress = add.ContractAddress,
                    .Region = add.Region.ToString,
                    .TypeID = add.TypeID,
                    .Type = New clsComboItems().Lookup_ComboItem(add.TypeID)
                })
            Next
            Return l
        End Function
    End Class
#End Region
#Region "Reservation"
    <Serializable()>
    Class WzReservation
        Property ReservationID As Int32
        Property ReservationNumber As String
        Property ResLocationID As Int32
        Property CheckIn As DateTime
        Property CheckOut As DateTime
        Property DateBooked As DateTime
        Property StatusDate As DateTime
        Property TypeID As Int32
        Property SubTypeID As Int32
        Property Adults As Int32
        Property Children As Int32
        Property LockInventory As Boolean
        Property StatusID As Int32
        Property ResortCompanyID As Int32
        Property SourceID As Int32
        ReadOnly Property TotalNights As Int32
            Get
                Return CheckOut.Subtract(CheckIn).Days
            End Get
        End Property

        Property Tour As WzTour

    End Class
    Interface IReservation
        Function Load(reservationID As Int32, tour As ITour) As WzReservation
        Function Load(duration As IDuration, packageReservation As IPackageReservation, tour As ITour) As WzReservation
    End Interface
    Class WzImpReservation
        Inherits ConnectionInfo
        Implements IReservation

        Private r As WzReservation = Nothing
        Public Function Load(reservationID As Integer, tour As ITour) As WzReservation Implements IReservation.Load
            r = New WzReservation
            Dim tour_id = 0
            With New clsReservations
                .ReservationID = reservationID
                .Load()
                r.ReservationID = .ReservationID
                r.ReservationNumber = .ReservationNumber
                r.ResLocationID = .ResLocationID
                r.ResortCompanyID = .ResortCompanyID
                r.StatusDate = .StatusDate
                r.StatusID = .StatusID
                r.TypeID = .TypeID
                r.SubTypeID = .SubTypeID
                r.CheckIn = .CheckInDate
                r.CheckOut = .CheckOutDate
                r.Adults = .NumberAdults
                r.Children = .NumberChildren
                r.DateBooked = .DateBooked
                r.LockInventory = .LockInventory
                r.SourceID = .SourceID
                tour_id = .TourID
            End With
            If tour_id > 0 Then
                Dim pi As IPremiumIssuedList = New WzImpPremiumIssuedList
                r.Tour = tour.Load(tour_id, pi)
            End If
            Return r
        End Function
        Public Function Load(duration As IDuration, packageReservation As IPackageReservation, tour As ITour) As WzReservation Implements IReservation.Load
            r = New WzReservation
            With r
                r.ReservationID = WzRandomID.Generate_Random_ID
                r.ResortCompanyID = packageReservation.ResortCompanyID
                r.ResLocationID = packageReservation.LocationID
                r.StatusID = packageReservation.StatusID
                r.SourceID = packageReservation.SourceID
                r.StatusDate = DateTime.Now.ToShortDateString
                r.TypeID = packageReservation.ResTypeID
                r.SubTypeID = 0
                r.Adults = 2
                r.DateBooked = DateTime.Now.ToShortDateString
                r.CheckIn = duration.StartDate.ToShortDateString
                r.CheckOut = duration.StartDate.AddDays(duration.Nights).ToShortDateString
            End With
            If packageReservation.PackageType = EnumPackageType.TourPackage Or packageReservation.PackageType = EnumPackageType.TourPromotion Or packageReservation.PackageType = EnumPackageType.Tradeshow Then
                Dim pi As IPremiumIssuedList = New WzImpPremiumIssuedList
                r.Tour = tour.Load(packageReservation, pi)
            End If
            Return r
        End Function
    End Class
    Interface IPackageReservation
        Property PackageID As Int32
        Property PackageReservationID As Int32
        Property BedRooms As String
        Property UnitTypeID As Int32
        ReadOnly Property UnitType As String
        Property InventoryTypeID As Int32
        ReadOnly Property InventoryType As String
        Property ResortCompanyID As Int32
        Property LocationID As Int32
        Property StatusID As Int32
        Property SourceID As Int32
        Property ResTypeID As Int32
        Property PackageType As EnumPackageType
        Property AccomID As Int32
        ReadOnly Property Accom As EnumAccom
        Sub Load(packageID As Int32)
    End Interface
    Class WzImpPackageReservation
        Inherits ConnectionInfo
        Implements IPackageReservation

        Public Property PackageID As Integer Implements IPackageReservation.PackageID
        Public Property PackageReservationID As Integer Implements IPackageReservation.PackageReservationID
        Public Property BedRooms As String Implements IPackageReservation.BedRooms
        Public Property UnitTypeID As Integer Implements IPackageReservation.UnitTypeID
        Public ReadOnly Property UnitType As String Implements IPackageReservation.UnitType
            Get
                Return IIf(UnitTypeID > 0, New clsComboItems().Lookup_ComboItem(UnitTypeID), String.Empty)
            End Get
        End Property
        Public Property InventoryTypeID As Integer Implements IPackageReservation.InventoryTypeID
        Public ReadOnly Property InventoryType As String Implements IPackageReservation.InventoryType
            Get
                Return IIf(InventoryTypeID > 0, New clsComboItems().Lookup_ComboItem(InventoryTypeID), String.Empty)
            End Get
        End Property
        Public Property ResortCompanyID As Integer Implements IPackageReservation.ResortCompanyID
        Public Property LocationID As Integer Implements IPackageReservation.LocationID
        Public Property StatusID As Integer Implements IPackageReservation.StatusID
        Public Property SourceID As Integer Implements IPackageReservation.SourceID
        Public Property ResTypeID As Integer Implements IPackageReservation.ResTypeID
        Public Property PackageType As EnumPackageType Implements IPackageReservation.PackageType
        Public Property AccomID As Integer Implements IPackageReservation.AccomID
        Public ReadOnly Property Accom As EnumAccom Implements IPackageReservation.Accom
            Get
                Return IIf(AccomID = 0 Or AccomID = 72, EnumAccom.Resort, EnumAccom.Hotel)
            End Get
        End Property

        Public Sub Load(packageID As Integer) Implements IPackageReservation.Load

            With New clsPackageReservation
                .PackageReservationID = New clsPackageReservation().Find_Res_ID(packageID)
                .Load()
                PackageReservationID = .PackageReservationID
                InventoryTypeID = .TypeID
                ResortCompanyID = .ResortCompanyID
                ResTypeID = .TypeID
                SourceID = .SourceID
                LocationID = .LocationID
                StatusID = .StatusID
            End With
            With New clsPackage
                .PackageID = packageID
                .Load()
                Me.PackageID = packageID
                BedRooms = .Bedrooms
                UnitTypeID = .UnitTypeID
                AccomID = .AccomID
                Select Case New clsComboItems().Lookup_ComboItem(.TypeID)
                    Case "Rental"
                        PackageType = EnumPackageType.Rental
                    Case "Owner Getaway"
                        PackageType = EnumPackageType.OwnerGetaway
                    Case "Owner Advantage"
                        PackageType = EnumPackageType.OwnerAdvantage
                    Case "Tour Package"
                        PackageType = EnumPackageType.TourPackage
                    Case "Tour Promotion"
                        PackageType = EnumPackageType.TourPromotion
                    Case "Tradeshow"
                        PackageType = EnumPackageType.Tradeshow
                End Select
            End With
        End Sub
    End Class
#End Region
#Region "Tour"
    <Serializable()>
    Class WzTour
        Property TourID As Int32
        Property CampaignID As Int32
        Property StatusID As Int32
        Property LocationID As Int32
        Property TypeID As Int32
        Property SubTypeID As Int32
        Property BookingDate As DateTime
        Property TourDate As DateTime
        Property SourceID As Int32
        Property TourTimeID As Int32
        Property PremiumIssuedList As WzPremiumIssuedList

    End Class
    Interface ITour
        Function Load(tourID As Int32, premiumIssuedList As IPremiumIssuedList) As WzTour
        Function Load(packageReservation As IPackageReservation, premiumIssuedList As IPremiumIssuedList) As WzTour
    End Interface
    Class WzImpTour
        Inherits ConnectionInfo
        Implements ITour

        Public Function Load(tourID As Integer, premiumIssuedList As IPremiumIssuedList) As WzTour Implements ITour.Load
            Dim tour = New WzTour
            With New clsTour
                .TourID = tourID
                .Load()
                tour.TourID = .TourID
                tour.LocationID = .TourLocationID
                tour.SourceID = .SourceID
                tour.CampaignID = .CampaignID
                tour.TypeID = .TypeID
                tour.SubTypeID = .SubTypeID
                tour.SourceID = .SourceID
                tour.TourDate = .TourDate
                tour.StatusID = .StatusID
                tour.BookingDate = .BookingDate
                tour.TourTimeID = .TourTime
            End With
            tour.PremiumIssuedList = premiumIssuedList.Load(tourID)
            Return tour
        End Function

        Public Function Load(packageReservation As IPackageReservation, premiumIssuedList As IPremiumIssuedList) As WzTour Implements ITour.Load
            Dim tour = New WzTour
            With New clsPackageTour
                .Load(packageReservation.PackageID, packageReservation.PackageReservationID)
                tour.CampaignID = .CampaignID
                tour.LocationID = .TourLocationID
                tour.StatusID = .TourSourceID
                tour.TypeID = .TourTypeID
                tour.SubTypeID = .TourSubTypeID
                tour.SourceID = .TourSourceID
                tour.TourTimeID = 0
            End With
            tour.PremiumIssuedList = premiumIssuedList.Load(packageReservation)
            Return tour
        End Function
    End Class
#End Region
#Region "Premiums"

    <Serializable()>
    Class WzPremiumIssued
        Property PremiumIssuedID As Int32
        Property PremiumID As Int32
        Property PremiumName As String
        Property CertificateNumber As String
        Property Premium_Cost As Decimal
        Property PremiumIssued_CostEA As Decimal
        Property QtyAssigned As Int32
        Property StatusID As Int32
        ReadOnly Property Status As String
            Get
                Return IIf(StatusID > 0, New clsComboItems().Lookup_ComboItem(StatusID), String.Empty)
            End Get
        End Property
        Property Is_Optional As Boolean
    End Class
    <Serializable>
    Class WzPremiumIssuedList
        Inherits List(Of WzPremiumIssued)

        Function Is_Above_MaxPremiumAmount(packageID As Int32) As Boolean
            Dim max_amt = 0D
            Return IIf(ToArray().ToList().Sum(Function(x) x.Premium_Cost) > max_amt, True, False)
        End Function
    End Class
    Interface IPremiumIssuedList
        Function Load(tourID As Int32) As WzPremiumIssuedList
        Function Load(packageReservation As IPackageReservation) As WzPremiumIssuedList
    End Interface
    Class WzImpPremiumIssuedList
        Inherits ConnectionInfo
        Implements IPremiumIssuedList

        Public Function Load(tourID As Integer) As WzPremiumIssuedList Implements IPremiumIssuedList.Load
            Dim l = New WzPremiumIssuedList
            Using cn As New SqlConnection(cs)
                Using cm = New SqlCommand()
                    Try
                        cn.Open()
                        cm.CommandText = String.Format("Select pi.PremiumIssuedID, p.PremiumID, p.PremiumName, p.Description, 0 [Optional], pi.QtyAssigned, pi.CostEA,  ps.ComboItem [Status], ps.ComboItemID [StatusID] from t_PremiumIssued pi left outer join t_Premium p on pi.PremiumID = p.PremiumID " _
                        & "left outer join t_ComboItems ps on pi.StatusID = ps.ComboItemID where pi.KeyField = 'TourID' and pi.KeyValue = {0} and ps.ComboItem <> 'Do Not Issue'", tourID)
                        cm.Connection = cn
                        Dim dt = New DataTable
                        dt.Load(cm.ExecuteReader())
                        For Each row As DataRow In dt.Rows
                            l.Add(New WzPremiumIssued With {
                                  .PremiumIssuedID = row("PremiumIssued"),
                                  .PremiumID = row("PremiumID").ToString,
                                  .PremiumName = row("PremiumName").ToString,
                                  .Is_Optional = row("Optional").ToString,
                                  .QtyAssigned = row("QtyAssigned").ToString,
                                  .PremiumIssued_CostEA = row("CostEA").ToString,
                                  .StatusID = row("StatusID").ToString})
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
            Return l
        End Function

        Public Function Load(packageReservation As IPackageReservation) As WzPremiumIssuedList Implements IPremiumIssuedList.Load
            Dim l = New WzPremiumIssuedList
            Using cn As New SqlConnection(cs)
                Using cm = New SqlCommand()
                    Try
                        cn.Open()
                        cm.CommandText = String.Format(
                       "select 0 PremiumIssuedID, p.PremiumID, p.PremiumName, p.Description, (select Optional from t_PackageTourPremium ptp where PackageTourID in (select PackageTourID from t_PackageTour where PackageReservationID in (select PackageReservationID from t_PackageReservation where PackageID = {0} and p.PremiumID =  ptp.PremiumID))) [Optional], " _
                       & "(select ptp.QtyAssigned from t_PackageTourPremium ptp where PackageTourID in (select PackageTourID from t_PackageTour where PackageReservationID in (select PackageReservationID from t_PackageReservation where PackageID = {0} and p.PremiumID =  ptp.PremiumID))) QtyAssigned, " _
                       & "(select ptp.CostEA from t_PackageTourPremium ptp where PackageTourID in (select PackageTourID from t_PackageTour where PackageReservationID in (select PackageReservationID from t_PackageReservation where PackageID = {0} and p.PremiumID =  ptp.PremiumID))) CostEA, " _
                       & "(select ci.ComboItem from t_PackageTourPremium ptp inner join t_ComboItems ci on ci.ComboItemID = ptp.PremiumStatusID where PackageTourID in (select PackageTourID from t_PackageTour where PackageReservationID in (select PackageReservationID from t_PackageReservation where PackageID = {0} and p.PremiumID =  ptp.PremiumID))) Status, " _
                       & "(select ci.ComboItemID from t_PackageTourPremium ptp inner join t_ComboItems ci on ci.ComboItemID = ptp.PremiumStatusID where PackageTourID in (select PackageTourID from t_PackageTour where PackageReservationID in (select PackageReservationID from t_PackageReservation where PackageID = {0} and p.PremiumID =  ptp.PremiumID))) StatusID " _
                       & "from t_Premium p where Active = 1 and PremiumID in (select PremiumID from t_PackageTourPremium where PackageTourID in (select PackageTourID from t_PackageTour where PackageReservationID in (select PackageReservationID from t_PackageReservation where PackageID = {0}))) order by p.PremiumName", packageReservation.PackageID)
                        cm.Connection = cn

                        Dim dt = New DataTable
                        dt.Load(cm.ExecuteReader())

                        For Each row As DataRow In dt.Rows
                            l.Add(New WzPremiumIssued With {
                                  .PremiumIssuedID = row("PremiumIssuedID"),
                                  .PremiumID = row("PremiumID").ToString,
                                  .PremiumName = row("PremiumName").ToString,
                                  .Is_Optional = row("Optional").ToString,
                                  .QtyAssigned = row("QtyAssigned").ToString,
                                  .PremiumIssued_CostEA = row("CostEA").ToString,
                                  .StatusID = row("StatusID").ToString})
                        Next

                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
            Return l
        End Function
    End Class

#End Region
#Region "Invoices"

#End Region
    Public Class WizardSalesPackage
        Public Property PackageID As Int32
        Public Property Description As String
        Public Property MaxCost As Decimal
        Public Property MinCost As Decimal
        Public Property AccomName As String
        Public Property TourRequired As Boolean
        Public Property RoomType As String
        Public Property RoomsAvail As Int32
        Public Property PackageType As String
        Public Property PackageSubType As String


    End Class
    Public Shared Function List_1(checkin As DateTime, nights As Int32, searchOption As Int32, webSource As String) As List(Of WizardSalesPackage)

        Dim l = New List(Of WizardSalesPackage)
        Dim d = New clsReservationWizard().Available_Packages(checkin.ToShortDateString(), nights, searchOption, webSource)

        For Each row As DataRow In d.Tables(0).Rows
            l.Add(New WizardSalesPackage With {
                  .PackageID = row("PackageID"),
                  .AccomName = row("AccomName"),
                  .Description = row("Description"),
                  .TourRequired = row("TourRequired"),
                  .RoomType = row("RoomType"),
                  .RoomsAvail = row("RoomsAvail"),
                  .PackageType = row("PackageType"),
                  .PackageSubType = "N/A",
                  .MaxCost = 1.01})
        Next
        Return l
    End Function
    Public Shared Function List(checkin As DateTime, nights As Int32, searchOption As Int32, webSource As String, Optional ddlPackageType As DropDownList = Nothing, Optional ddlPackageSubType As DropDownList = Nothing, Optional ddlUnitTypes As DropDownList = Nothing, Optional ddlRoomSizes As DropDownList = Nothing) As List(Of WizardSalesPackage)

        Dim l = New ExcelData().GetData().ToList()

        If ddlPackageType.SelectedItem.Text <> "All" Then
            l = l.Where(Function(x) x.PackageType = ddlPackageType.SelectedItem.Text).ToList()
        End If

        If ddlPackageSubType.SelectedItem.Text <> "All" Then
            If ddlPackageSubType.SelectedItem.Text = "Empty" Then
                l = l.Where(Function(x) String.IsNullOrEmpty(x.PackageSubType) Or x.PackageSubType = "NULL").ToList()
            Else
                l = l.Where(Function(x) x.PackageSubType.Trim() = ddlPackageSubType.SelectedItem.Text).ToList()
            End If
        End If

        If ddlUnitTypes.SelectedItem.Text <> "All" Then
            l = l.Where(Function(x) x.RoomType.IndexOf(ddlUnitTypes.SelectedItem.Text) > 0).ToList()
        End If

        If ddlRoomSizes.SelectedItem.Text <> "All" Then
            l = l.Where(Function(x) x.RoomType.Equals(ddlRoomSizes.SelectedItem.Text)).ToList()
        End If
        'Dim d = Available_Packages(checkin.ToShortDateString(), nights, searchOption, webSource)

        'For Each row As DataRow In d.Tables(0).Rows
        '    l.Add(New WizardSalesPackage With {
        '          .PackageID = row("PackageID"),
        '          .AccomName = row("AccomName"),
        '          .Description = row("Description"),
        '          .TourRequired = row("TourRequired"),
        '          .RoomType = row("RoomType"),
        '          .RoomsAvail = row("RoomsAvail"),
        '          .PackageType = row("PackageType"),
        '          .PackageSubType = "N/A",
        '          .MaxCost = 1.01})
        'Next
        Return l
    End Function

    Public Enum EnumPackageType
        OwnerGetaway
        OwnerAdvantage
        Rental
        Tradeshow
        TourPackage
        TourPromotion
    End Enum
    Public Enum EnumAccom
        Resort
        Hotel
    End Enum

    Public Interface IDuration
        Property StartDate As DateTime
        Property Nights As Int32
    End Interface


    Class WzImpDuration
        Implements IDuration

        Public Property StartDate As Date Implements IDuration.StartDate
        Public Property Nights As Integer Implements IDuration.Nights
    End Class
    MustInherit Class ConnectionInfo

        Protected cs As String = String.Format("data source=localhost\sqlexpress;initial catalog=CRMSNet;user=sa;persist security info=False;packet size=4096;Connection Timeout=120;")
        Protected cmd As SqlCommand
        Protected cn As SqlConnection
        Protected ada As SqlDataAdapter
        Protected ds As DataSet
        Protected dt As DataTable
    End Class

    Interface IPaymentMethodID
        ReadOnly Property PaymentMethodID As Int32
    End Interface
    Interface IDiscountPaymentMethodID
        ReadOnly Property DiscountPaymentMethodID As Int32
    End Interface
    Interface IDiscount
        Inherits IDiscountPaymentMethodID
        Property Discount_Amount As Decimal
    End Interface

    Interface IChargeBack
        Inherits IPaymentMethodID
        Property Chargeback_Amount As Decimal
    End Interface
    Interface IPromoNights
        ReadOnly Property PromoNights As Int32
    End Interface
    Interface IMinimumNights
        'for rental, owner getaway
        ReadOnly Property MinNights As Int32
    End Interface
    Interface IFinTransCode
        ReadOnly Property ONHOA_FinTransID As Int32
    End Interface
    Interface IInvoiceONHOA
        Inherits IFinTransCode
        Property ONHOA_Amount As Decimal
    End Interface
    Interface IResortFeeFinTransCode
        ReadOnly Property ResortFeeFinTransID As Int32
    End Interface
    Interface IInvoiceResortFee
        Inherits IResortFeeFinTransCode
        Property ResortFee_Amount As Decimal
    End Interface
    Interface IPackageType
        ReadOnly Property Type As EnumPackageType
    End Interface
    Public Interface IPackage
        Inherits IPackageType
        Inherits IFinTransCode
        Property PackageID As Int32
    End Interface


    Interface IPackageID
        Property PackageID As Int32
    End Interface
    Interface IPackageRental
        Inherits IInvoiceONHOA
        Inherits IPackageID
        Inherits IMinimumNights
    End Interface

    Interface IPackageOwnerGetaway
        Inherits IInvoiceONHOA
        Inherits IPackageID
        Inherits IMinimumNights
    End Interface
    Interface IPackageOwnerAdvantage
        Inherits IInvoiceONHOA
        Inherits IPackageID
        Inherits IMinimumNights
    End Interface
    'base interface for all packages with a tour
    Interface IPackageTour
        Inherits IPackageID
        Inherits IInvoiceONHOA
        Inherits IPromoNights
        Inherits IChargeBack
        Inherits ICalculation
    End Interface

    Interface IPackageTourPromotion
        Inherits IPackageTour
        Inherits IDiscount

        ReadOnly Property PromoRate As Decimal
    End Interface

    Interface IPackageTradeshow
        Inherits IPackageTour
        Inherits IInvoiceResortFee
    End Interface
    Interface IPackageTourPackage
        Inherits IPackageTour
        Inherits IDiscount
    End Interface
    Interface ICalculation
        Sub Calculate(duraction As IDuration)
    End Interface



    Class WzRateTable
        Inherits ConnectionInfo

        Overloads Function GetRateTableID(packageID As Int32) As Int32
            Dim rate_table_id = 0
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand
                    Try
                        cmd.CommandText = String.Format("select rt.RateTableID from t_Package p inner join t_Accom2Resort a2r on p.AccomID = a2r.AccomID and p.UnitTypeID = a2r.UnitTypeID and p.Bedrooms = a2r.BD inner join t_RateTable rt on rt.RateTableID = a2r.RateTableID where p.PackageID = {0}", packageID)
                        cmd.Connection = cn
                        cn.Open()
                        Dim o = cmd.ExecuteScalar
                        If o IsNot Nothing Then
                            rate_table_id = Convert.ToInt32(o)
                        End If
                    Catch ex As Exception
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return rate_table_id
        End Function
    End Class
    Class WzInvoiceRental
        Inherits WzRateTable
        Implements IPackageRental
        Implements ICalculation

        ReadOnly Property RateTableID As Int32
            Get
                Return GetRateTableID(PackageID)
            End Get
        End Property

        Public Property ONHOA_Amount As Decimal Implements IInvoiceONHOA.ONHOA_Amount
        Public ReadOnly Property ONHOA_FinTransID As Integer Implements IFinTransCode.ONHOA_FinTransID
            Get
                Return 32
            End Get
        End Property

        Public Property PackageID As Integer Implements IPackageID.PackageID

        Public ReadOnly Property MinNights As Integer Implements IMinimumNights.MinNights
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select MinNights from t_package where PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property

        Public Sub Calculate(duraction As IDuration) Implements ICalculation.Calculate
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand()
                    Try
                        cn.Open()
                        cmd.Connection = cn
                        'calculates the package's min nights at the Rental rate
                        cmd.CommandText = String.Format("select Coalesce(sum(RentalAmount), 0) Rate from t_RateTableRates where RateTableID in ({0}) and Date between '{1}' and '{2}'", RateTableID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(MinNights - 1).ToShortDateString())
                        ONHOA_Amount = cmd.ExecuteScalar()
                        'adds the minimum amount if guest stays beyond the min nights
                        If duraction.Nights > MinNights Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in ({0}) and Date between '{1}' and '{2}'", RateTableID, duraction.StartDate.AddDays(MinNights).ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount += cmd.ExecuteScalar()
                        ElseIf MinNights = 0 Then
                            cmd.CommandText = String.Format("select Coalesce(sum(RentalAmount), 0) Rate from t_RateTableRates where RateTableID in ({0}) and Date between '{1}' and '{2}'", RateTableID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount = cmd.ExecuteScalar()
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
        End Sub
    End Class
    Class WzInvoiceOwnerGetaway
        Inherits WzRateTable
        Implements IPackageOwnerGetaway
        Implements ICalculation

        Public Property ONHOA_Amount As Decimal Implements IInvoiceONHOA.ONHOA_Amount
        Public ReadOnly Property ONHOA_FinTransID As Integer Implements IFinTransCode.ONHOA_FinTransID
            Get
                Return 32
            End Get
        End Property
        Public Property PackageID As Integer Implements IPackageID.PackageID
        Public ReadOnly Property MinNights As Integer Implements IMinimumNights.MinNights
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select MinNights from t_package where PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property

        Public Sub Calculate(duraction As IDuration) Implements ICalculation.Calculate
            Dim sql = String.Format("select Coalesce(sum(OwnerAmount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(MinNights - 1).ToShortDateString())
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand(sql, cn)
                    Try
                        ONHOA_Amount = cmd.ExecuteScalar()
                        'reservation stays longer the min nights
                        If duraction.Nights > MinNights Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.AddDays(MinNights).ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount += cmd.ExecuteScalar()
                        ElseIf MinNights = 0 Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount = cmd.ExecuteScalar()
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
        End Sub

    End Class
    Class WzInvoiceOwnerAdvantage
        Inherits WzRateTable
        Implements IPackageRental
        Implements ICalculation

        Public Property ONHOA_Amount As Decimal Implements IInvoiceONHOA.ONHOA_Amount

        Public ReadOnly Property ONHOA_FinTransID As Integer Implements IFinTransCode.ONHOA_FinTransID
            Get
                Return 32
            End Get
        End Property

        Public Property PackageID As Integer Implements IPackageID.PackageID

        Public ReadOnly Property MinNights As Integer Implements IMinimumNights.MinNights
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select MinNights from t_package where PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property

        Public Sub Calculate(duraction As IDuration) Implements ICalculation.Calculate
            Dim sql = String.Format("select Coalesce(sum(OwnerAdvAmount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(MinNights - 1).ToShortDateString())
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand(sql, cn)
                    Try
                        ONHOA_Amount = cmd.ExecuteScalar()
                        'reservation stays longer the min nights
                        If duraction.Nights > MinNights Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.AddDays(MinNights).ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount += cmd.ExecuteScalar()
                        ElseIf MinNights = 0 Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount = cmd.ExecuteScalar()
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
        End Sub
    End Class
    Class WzInvoiceTourPromotion
        Inherits ConnectionInfo
        Implements IPackageTourPromotion

        Public Property PackageID As Integer Implements IPackageID.PackageID
        Public Property ONHOA_Amount As Decimal Implements IInvoiceONHOA.ONHOA_Amount
        Public ReadOnly Property ONHOA_FinTransID As Integer Implements IFinTransCode.ONHOA_FinTransID
            Get
                Return 32
            End Get
        End Property
        Public ReadOnly Property PromoNights As Integer Implements IPromoNights.PromoNights
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select p.PromoNights from t_package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID where pr.PackageReservationID = {0}", New clsPackageReservation().Find_Res_ID(PackageID))
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property
        Public Property Chargeback_Amount As Decimal Implements IChargeBack.Chargeback_Amount

        Public ReadOnly Property PaymentMethodID As Integer Implements IPaymentMethodID.PaymentMethodID
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select prp.PaymentMethodID from t_PackageReservationFinTransCode prftc inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID inner join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID inner join t_ComboItems pm on pm.ComboItemID = prp.PaymentMethodID where pm.ComboItem like 'Chargeback%' and pr.PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property
        Public Property Discount_Amount As Decimal Implements IDiscount.Discount_Amount
        Public ReadOnly Property DiscountPaymentMethodID As Integer Implements IDiscountPaymentMethodID.DiscountPaymentMethodID
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select prp.PaymentMethodID from t_PackageReservationFinTransCode prftc inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID inner join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID inner join t_ComboItems pm on pm.ComboItemID = prp.PaymentMethodID where pm.ComboItem = 'discount' and pr.PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property

        Public ReadOnly Property PromoRate As Decimal Implements IPackageTourPromotion.PromoRate
            Get
                Dim amt = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select pr.PromoRate from t_package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID where pr.PackageReservationID = {0}", New clsPackageReservation().Find_Res_ID(PackageID))
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                amt = Convert.ToDecimal(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return amt
            End Get
        End Property

        Public Sub Calculate(duraction As IDuration) Implements ICalculation.Calculate
            Dim sql = String.Format("select Coalesce(sum(TPromotionAmount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(PromoNights - 1).ToShortDateString())
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand(sql, cn)
                    Try
                        ONHOA_Amount = cmd.ExecuteScalar()
                        'reservation stays longer the promo nights
                        If duraction.Nights > PromoNights Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.AddDays(PromoNights).ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount += cmd.ExecuteScalar()
                        ElseIf PromoNights = 0 Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount = cmd.ExecuteScalar()
                        End If

                        cmd.CommandText = String.Format("select Coalesce(sum(TPromotionAmount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(PromoNights - 1).ToShortDateString())
                        Discount_Amount = ONHOA_Amount - PromoRate
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
        End Sub
    End Class
    Class WzInvoiceTradeshow
        Inherits WzRateTable
        Implements IPackageTradeshow

        Public Property PackageID As Integer Implements IPackageID.PackageID
        Public Property ONHOA_Amount As Decimal Implements IInvoiceONHOA.ONHOA_Amount
        Public ReadOnly Property PromoNights As Integer Implements IPromoNights.PromoNights
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select p.PromoNights from t_package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID where pr.PackageReservationID = {0}", New clsPackageReservation().Find_Res_ID(PackageID))
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property
        Public ReadOnly Property PaymentMethodID As Integer Implements IPaymentMethodID.PaymentMethodID
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select prp.PaymentMethodID from t_PackageReservationFinTransCode prftc inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID inner join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID inner join t_ComboItems pm on pm.ComboItemID = prp.PaymentMethodID where pm.ComboItem like 'Chargeback%' and pr.PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property
        Public ReadOnly Property ONHOA_FinTransID As Integer Implements IFinTransCode.ONHOA_FinTransID
            Get
                Return 32
            End Get
        End Property

        Public Property Chargeback_Amount As Decimal Implements IChargeBack.Chargeback_Amount

        Public ReadOnly Property ResortFeeFinTransID As Integer Implements IResortFeeFinTransCode.ResortFeeFinTransID
            Get
                Return 504
            End Get
        End Property

        Public Property ResortFee_Amount As Decimal Implements IInvoiceResortFee.ResortFee_Amount
            Get
                Dim amt = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select Coalesce(prftc.Amount, 0) from t_PackageReservation pr inner join t_PackageReservationFinTransCode prftc on pr.PackageReservationID = prftc.PackageReservationID inner join t_FinTransCodes ftc on ftc.FinTransID = prftc.FinTransCodeID inner join t_ComboItems tc on tc.ComboItemID = ftc.TransCodeID where tc.ComboItem = 'resort fee' and pr.PackageReservationID = {0}", New clsPackageReservation().Find_Res_ID(PackageID))
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                amt = Convert.ToDecimal(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return amt
            End Get
            Set(value As Decimal)
            End Set
        End Property

        Public Sub Calculate(duraction As IDuration) Implements ICalculation.Calculate
            Dim sql = String.Format("select Coalesce(sum(TSAmount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(PromoNights - 1).ToShortDateString())
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand(sql, cn)
                    Try
                        ONHOA_Amount = cmd.ExecuteScalar()
                        Chargeback_Amount = ONHOA_Amount

                        'reservation stays longer the promo nights
                        If duraction.Nights > PromoNights Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.AddDays(PromoNights).ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount += cmd.ExecuteScalar()
                        ElseIf PromoNights = 0 Then
                            String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(PromoNights - 1).ToShortDateString())
                            ONHOA_Amount = cmd.ExecuteScalar()
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
        End Sub
    End Class
    Class WzInvoiceTourPackage
        Inherits WzRateTable
        Implements IPackageTourPackage

        Public Property PackageID As Integer Implements IPackageID.PackageID
        Public Property ONHOA_Amount As Decimal Implements IInvoiceONHOA.ONHOA_Amount
        Public ReadOnly Property ONHOA_FinTransID As Integer Implements IFinTransCode.ONHOA_FinTransID
            Get
                Return 32
            End Get
        End Property

        Public ReadOnly Property PromoNights As Integer Implements IPromoNights.PromoNights
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select p.PromoNights from t_package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID where pr.PackageReservationID = {0}", New clsPackageReservation().Find_Res_ID(PackageID))
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property
        Public Property Chargeback_Amount As Decimal Implements IChargeBack.Chargeback_Amount
        Public ReadOnly Property PaymentMethodID As Integer Implements IPaymentMethodID.PaymentMethodID
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select prp.PaymentMethodID from t_PackageReservationFinTransCode prftc inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID inner join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID inner join t_ComboItems pm on pm.ComboItemID = prp.PaymentMethodID where pm.ComboItem like 'Chargeback%' and pr.PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property

        Public Property Discount_Amount As Decimal Implements IDiscount.Discount_Amount

        Public ReadOnly Property DiscountPaymentMethodID As Integer Implements IDiscountPaymentMethodID.DiscountPaymentMethodID
            Get
                Dim i = 0
                Using cn = New SqlConnection(cs)
                    Using cmd = New SqlCommand()
                        Try
                            cn.Open()
                            cmd.Connection = cn
                            cmd.CommandText = String.Format("select prp.PaymentMethodID from t_PackageReservationFinTransCode prftc inner join t_PackageReservation pr on pr.PackageReservationID = prftc.PackageReservationID inner join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID inner join t_ComboItems pm on pm.ComboItemID = prp.PaymentMethodID where pm.ComboItem = 'discount' and pr.PackageID = {0}", PackageID)
                            Dim o = cmd.ExecuteScalar()
                            If o IsNot Nothing Then
                                i = Convert.ToInt32(o)
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End Using
                End Using
                Return i
            End Get
        End Property

        Public Sub Calculate(duraction As IDuration) Implements ICalculation.Calculate
            Dim sql = String.Format("select Coalesce(sum(TPAmount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(PromoNights - 1).ToShortDateString())
            Using cn = New SqlConnection(cs)
                Using cmd = New SqlCommand(sql, cn)
                    Try
                        ONHOA_Amount = cmd.ExecuteScalar()
                        Chargeback_Amount = ONHOA_Amount

                        'reservation stays longer the promo nights
                        If duraction.Nights > PromoNights Then
                            cmd.CommandText = String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.AddDays(PromoNights).ToShortDateString(), duraction.StartDate.AddDays(duraction.Nights - 1).ToShortDateString())
                            ONHOA_Amount += cmd.ExecuteScalar()
                        ElseIf PromoNights = 0 Then
                            String.Format("select Coalesce(sum(Amount), 0) Rate from t_RateTableRates where RateTableID in (select rt.RateTableID from t_Package p inner join t_Accom2Resort ar on p.AccomID = ar.AccomID and p.UnitTypeID = ar.UnitTypeID and p.Bedrooms = ar.BD inner join t_RateTable rt on rt.RateTableID = ar.RateTableID where p.PackageID = {0}) and Date between '{1}' and '{2}'", PackageID, duraction.StartDate.ToShortDateString(), duraction.StartDate.AddDays(PromoNights - 1).ToShortDateString())
                            ONHOA_Amount = cmd.ExecuteScalar()
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
        End Sub
    End Class

    'for hotel accommodation
    Class WzAccommodation
        Inherits WzReservation

        Property AccommodationID As Int32
        Property LodgingID As Int32
        Property CheckInLocationID As Int32
        Property RoomTypeID As Int32
        Property AccomID As Int32
        Property GuestTypeID As Int32

    End Class
    Interface IAccommodationLoad
        Function Load(accommodationID As Int32) As WzAccommodation
    End Interface

    Class WzAllocation
        Property GUID As String
        Property AllocationID As Int32
        Property RoomID As Int32
        Property RoomNumber As String
        Property DateAllocated As DateTime
    End Class
    Class WzAllocationMatrix
        Inherits List(Of WzAllocation)

        ReadOnly Property Rooms As List(Of String)
            Get
                If ToArray().Count > 0 Then
                    Return ToArray().GroupBy(Function(x) x.RoomNumber).Select(Function(x) x.Key).ToList()
                Else
                    Return New List(Of String)()
                End If
            End Get
        End Property
        ReadOnly Property StartDate As DateTime?
            Get
                If ToArray().Count > 0 Then
                    Return ToArray().OrderBy(Function(x) x.DateAllocated).FirstOrDefault().DateAllocated
                Else
                    Return Nothing
                End If
            End Get
        End Property
        ReadOnly Property EndDate As DateTime?
            Get
                If ToArray().Count > 0 Then
                    Return ToArray().OrderByDescending(Function(x) x.DateAllocated).LastOrDefault().DateAllocated
                Else
                    Return Nothing
                End If
            End Get
        End Property
    End Class
    Interface IAllocationMatrixLoad
        Function Load(reservationID As Int32) As WzAllocationMatrix
        Function Load(packageID As Int32, iDuration As IDuration) As WzAllocationMatrix
    End Interface
    Class AllocationMatrixLoad
        Inherits ConnectionInfo
        Implements IAllocationMatrixLoad

        Public Function Load(reservationID As Integer) As WzAllocationMatrix Implements IAllocationMatrixLoad.Load
            Dim sql = String.Format("SELECT AllocationID, DateAllocated FROM t_RoomAllocationMatrix WHERE ReserevationID = {0}", reservationID)
            Dim l As New WzAllocationMatrix
            Using cn = New SqlConnection(cs)
                Using ad = New SqlDataAdapter(sql, cn)
                    Try
                        ad.Fill(dt)
                        For Each dr As DataRow In dt.Rows
                            l.Add(New WzAllocation With {
                                  .AllocationID = dr("AllocationID").ToString,
                                  .DateAllocated = DateTime.Parse(dr("DateAllocated").ToString())})
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
            Return l
        End Function

        Public Function Load(packageID As Int32, IDuration As IDuration) As WzAllocationMatrix Implements IAllocationMatrixLoad.Load
            Dim pr As IPackageReservation = New WzImpPackageReservation
            'Dim pr = r.Load(packageID)

            Dim dt = New DataTable
            Dim l As New WzAllocationMatrix
            Dim sql = String.Format("select * from ufn_RoomsAvailable('{0}', {1}, '{2}', {3}, {4}, '{5}', '{6}', 0) where AVAILABLE = 'available'",
                        pr.BedRooms, pr.UnitTypeID, IDuration.StartDate.ToShortDateString(), IDuration.Nights,
                pr.InventoryTypeID, pr.UnitType, pr.InventoryType)

            Using cn = New SqlConnection(cs)
                Using ad = New SqlDataAdapter(sql, cn)
                    Try
                        ad.Fill(dt)
                        For Each dr As DataRow In dt.Rows
                            Dim guid_id = Guid.NewGuid().ToString

                            If dr.Item("RoomID").Equals(DBNull.Value) = False Then
                                For i = 0 To IDuration.Nights - 1
                                    l.Add(New WzAllocation With {
                                        .GUID = guid_id,
                                        .RoomID = dr("RoomID").ToString(),
                                        .RoomNumber = dr("RoomNumber").ToString(),
                                        .DateAllocated = IDuration.StartDate.AddDays(i)})
                                Next
                            End If
                            If dr.Item("RoomID2").Equals(DBNull.Value) = False Then
                                For i = 0 To IDuration.Nights - 1
                                    l.Add(New WzAllocation With {
                                        .GUID = guid_id,
                                        .RoomID = dr("RoomID").ToString(),
                                        .RoomNumber = dr("RoomNumber2").ToString(),
                                        .DateAllocated = IDuration.StartDate.AddDays(i)})
                                Next
                            End If
                            If dr.Item("RoomID3").Equals(DBNull.Value) = False Then
                                For i = 0 To IDuration.Nights - 1
                                    l.Add(New WzAllocation With {
                                        .GUID = guid_id,
                                        .RoomID = dr("RoomID").ToString(),
                                        .RoomNumber = dr("RoomNumber3").ToString(),
                                        .DateAllocated = IDuration.StartDate.AddDays(i)})
                                Next
                            End If
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using
            Return l
        End Function
    End Class
    Interface ITourAvailability
        Function Check(IDuration As IDuration, campaignID As Int32, tourLocationID As Int32) As List(Of WzTourWave)
    End Interface
    Class WzTourWave
        ReadOnly Property ComboItemID As Int32
            Get
                If TourTime.ToLower().Trim().LastIndexOf("am") > 0 Then
                    Return New clsComboItems().Lookup_ID("TourTime", TourTime.ToLower().Replace(":", "").Replace("am", "").Trim())
                ElseIf TourTime.ToLower().Trim().LastIndexOf("pm") > 0 Then
                    Dim v = TourTime.ToLower().Replace(":", "").Replace("pm", "").Trim()
                    Return New clsComboItems().Lookup_ID("TourTime", Int32.Parse(v) + 1200)
                Else
                    Return 0
                End If
            End Get
        End Property
        Property TourDate As String
        Property TourTime As String
    End Class
    Class WzTourAvailability
        Inherits ConnectionInfo
        Implements ITourAvailability

        Public Function Check(IDuration As IDuration, campaignID As Integer, tourLocationID As Integer) As List(Of WzTourWave) Implements ITourAvailability.Check
            Dim dt = New DataTable
            Dim l = New List(Of WzTourWave)
            Using cn = New SqlConnection(cs)
                Using cm = New SqlCommand("select * from dbo.ufn_TourAvailability(@StartDate, @Days, @CampID, @LocID)", cn)
                    cm.CommandType = CommandType.Text
                    cm.Parameters.AddWithValue("@StartDate", IDuration.StartDate.AddDays(1).ToShortDateString())
                    cm.Parameters.AddWithValue("@Days", IIf(IDuration.Nights = 0, 0, IDuration.Nights - 2))
                    cm.Parameters.AddWithValue("@CampID", campaignID)
                    cm.Parameters.AddWithValue("@LocID", tourLocationID)
                    Try
                        cn.Open()
                        dt.Load(cm.ExecuteReader())
                        For Each dr As DataRow In dt.Rows
                            l.Add(New WzTourWave With {.TourDate = DateTime.Parse(dr("TourDate").ToString()).ToShortDateString(), .TourTime = DateTime.Parse(dr("TourTime").ToString()).ToShortDateString()})
                        Next
                    Catch ex As Exception
                        cn.Close()
                        Dim er = ex.Message
                    End Try
                End Using
            End Using
            Return l
        End Function
    End Class
    'used to assign to columns without primary key values yet.
    Class WzRandomID
        Public Shared ReadOnly Property Generate_Random_ID As Int32
            Get
                Return -New Random().Next(Int32.MaxValue - (New Random().Next()))
            End Get
        End Property
    End Class
#End Region


#Region "GUI Controls - Tour And Premiums"
    Protected Sub Button_Edit_PremiumIssued_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_PremiumIssued_Cancel.Click

    End Sub
    Protected Sub Button_Edit_PremiumIssued_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_PremiumIssued_Submit.Click

    End Sub

    Private Function IsWizardStepIncluded(panel As Object) As Boolean
        For Each ws As WizardStep In Wizard1.WizardSteps
            If Integer.Parse(Regex.Replace(ws.ID, "[^\d]", "")) = Integer.Parse(Regex.Replace(CType(panel, Panel).ID, "[^\d]", "")) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub Panel2_Load(sender As Object, e As EventArgs) Handles Panel2.Load
        'DropDownList_Search_Nights ASP.NET control to bind data only once after the Page.IsPostBack property is true.
        If DropDownList_Search_Nights.Items.Count = 0 And IsWizardStepIncluded(sender) And IsPostBack = True Then
            Dim nightsAvail As IDropDownNightsAvailable = New DropDownNightsAvailable(RadioButtonList_StartupOptions.SelectedItem.Text)

            DropDownList_Search_Nights.DataSource = nightsAvail.ListNights()
            DropDownList_Search_Nights.DataBind()

            Dim sql = "select distinct case " _
          & "when s.ComboItem = 'Estates' and Bedrooms = '1BD-DWN' then '1 Downstairs Bedroom Estates' " _
          & "when s.ComboItem = 'Estates' and Bedrooms = '1BD-UP' then '1 Upstairs Bedroom Estates' " _
          & "when s.ComboItem = 'Estates' and Bedrooms = '2' then '2 Bedroom Estates' " _
          & "when s.ComboItem = 'Estates' and Bedrooms = '3' then '3 Bedroom Estates' " _
          & "when s.ComboItem = 'Estates' and Bedrooms = '4' then '4 Bedroom Estates' " _
          & "when s.ComboItem = 'Cottage' and Bedrooms = '1' then '1 Bedroom Cottage' " _
          & "when s.ComboItem = 'Cottage' and Bedrooms = '2' then '2 Bedroom Cottage' " _
          & "when s.ComboItem = 'Cottage' and Bedrooms = '3' then '3 Bedroom Cottage' " _
          & "when s.ComboItem = 'Townes' and Bedrooms = '2' then '2 Bedroom Townes' " _
          & "when s.ComboItem = 'Townes' and Bedrooms = '4' then '4 Bedroom Townes' " _
          & "end as Room from t_Package p inner join t_ComboItems s on p.UnitTypeID = s.ComboItemID where len(p.Bedrooms) > 0 or p.Bedrooms is not null order by Room;"

            Using cn As New SqlConnection(Resources.Resource.cns)
                Using cm As New SqlCommand(sql, cn)
                    Try
                        cn.Open()
                        Dim d = New DataTable
                        d.Load(cm.ExecuteReader())
                        With DropDownList_RoomSizes
                            .AppendDataBoundItems = True
                            .Items.Add("(All)")
                            .DataTextField = "Room"
                            .DataValueField = "Room"
                            .DataSource = d
                            .DataBind()
                        End With
                    Catch ex As Exception
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
    End Sub

    Private Sub Panel7_Load(sender As Object, e As EventArgs) Handles Panel7.Load
        Dim pkID = 248
        HiddenField_LogonVendorID.Value = 34
        Dim sqlText = String.Format("select s.*, '' 'Resort-Hotel', '' 'Unit-Room' ,(select AccomName from t_Accom where AccomID = s.AccomID) AccomName, coalesce((select ComboItem from t_ComboItems where ComboItemID = s.UnitTypeID), '') 'UnitType', coalesce((select ComboItem from t_ComboItems where ComboItemID = s.AccomRoomTypeID), '') 'RoomType', coalesce(pt.PackageTourID, 0) PackageTourID, coalesce(pt.CampaignID, 0) CampaignID, coalesce(pt.TourLocationID, 0) TourLocationID, v.VendorID, v.Vendor from " _
            & "t_Vendor2Package v2p inner join t_Vendor v on v.VendorID = v2p.VendorID inner join " _
            & "(select p.PackageID, p.Description, pr.PackageReservationID, pr.CreateTour, p.AccomRoomTypeID, p.AccomID, p.UnitTypeID " _
            & "from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
            & "inner join (Select p1.PackageID, p1.Bedrooms, pr1.PromoNights, p1.TypeID from t_Package p1 " _
            & "inner join t_PackageReservation pr1 on p1.PackageID = pr1.PackageID where p1.PackageID = {0}) x " _
            & "on pr.PromoNights = x.PromoNights and Left(p.Bedrooms, 1) = Left(x.Bedrooms, 1) and x.TypeID = p.TypeID " _
            & "and CONVERT(datetime, CONVERT(varchar(10), p.EndDate, 101)) >= CONVERT(datetime, CONVERT(varchar(10), GETDATE(), 101)) and p.Active = 1) s " _
            & "on s.PackageID = v2p.PackageID left join t_PackageTour pt on pt.PackageReservationID = s.PackageReservationID " _
            & "and v.VendorID = {1}", pkID, HiddenField_LogonVendorID.Value)

        Using cn As New SqlConnection(Resources.Resource.cns)
            Using cm As New SqlCommand(sqlText, cn)
                Try
                    cn.Open()
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())


                    With GridView_Search_Available_Packages_For_Rebooking
                        .DataSource = dt
                        .DataBind()
                    End With
                Catch ex As Exception
                    cn.Close()
                End Try
            End Using
        End Using
    End Sub


#End Region




    Protected Sub GridView_Search_Available_Packages_For_Rebooking_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView_Search_Available_Packages_For_Rebooking.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr = CType(e.Row.DataItem, DataRowView).Row
            Dim cb = CType(e.Row.Cells(0).FindControl("RadioButton1"), CheckBox)
            For Each col As DataColumn In dr.Table.Columns
                cb.Attributes.Add(String.Format("data-{0}", col.ColumnName.Trim()), dr(col).ToString)
            Next

            'set the checkbox on the current package

        End If
    End Sub


End Class




