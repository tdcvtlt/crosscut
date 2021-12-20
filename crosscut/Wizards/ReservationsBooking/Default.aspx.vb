Imports System.Data.SqlClient
Imports System.Globalization

Public Class _Default1
    Inherits System.Web.UI.Page
    Public Property Prospect As WzProspect
    Private Panels As List(Of Panel) = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Panels = New List(Of Panel)(New Panel() {Welcome_Panel, ListPackagesForSales_Panel, ListPackagesOfferedByVendor_Panel, EditProspect_Panel,
                                    EditReservation_Panel, EditTour_Panel, SearchProspect_Panel, ListPendingReservations_Panel,
                                    AssignRooms_Panel, ReAssignRooms_Panel, EditHotel_Panel, ProcesPayment_Panel, ProcessRefund_Panel,
                                    EditNotes_Panel, Confirmation_Panel})
        Array.ForEach(Panels.ToArray(), Sub(p As Panel) p.Visible = False)

        If IsPostBack = False Then
            HiddenField_ViewStateID.Value = Guid.NewGuid.ToString
            Reset_Controls()
            Init_DropDownLists()
            TextBox_Search_CheckIn.Text = "12/13/2021"
            TextBox_Search_CheckOut.Text = "12/16/2021"

            Prospect = New WzProspect
            With Prospect
                .Prospect_Emails = New List(Of WzEmail)
                .Prospect_Phones = New List(Of WzPhone)
                .Prospect_Addresses = New List(Of WzAddress)
                .Prospect_Premiums = New List(Of WzPremium)
                .Prospect_Packages = New List(Of WzPackage)
            End With
            ViewState(HiddenField_ViewStateID.Value) = Prospect
            Welcome_Panel.Visible = True
        Else
            Prospect = DirectCast(ViewState(HiddenField_ViewStateID.Value), WzProspect)
            Dim l = New List(Of WizardStep)

            If RadioButton_Welcome_Option_1.Checked Then
                With Wizard1.WizardSteps.OfType(Of WizardStep)

                    l.Add(.Where(Function(x) x.ID = "ListPackagesForSales_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "SearchProspect_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "EditProspect_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "EditReservation_WizardStep").Single())

                    Try
                        If Prospect.Package_Is_Resort_Stayed = True Then
                            l.Add(.Where(Function(x) x.ID = "AssignRooms_WizardStep").Single())
                        Else
                            l.Add(.Where(Function(x) x.ID = "EditHotel_WizardStep").Single())
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Try
                        If Prospect.Package_Has_Tour Then
                            l.Add(.Where(Function(x) x.ID = "EditTour_WizardStep").Single())
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try

                    If Prospect.Prospect_Packages.Sum(Function(x) x.RateCost) > 0 Then
                        l.Add(.Where(Function(x) x.ID = "ProcesPayment_WizardStep").Single())
                    End If

                    l.Add(.Where(Function(x) x.ID = "EditNotes_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "Confirmation_WizardStep").Single())
                End With

                Button_SearchProspect_New.Visible = True

            ElseIf RadioButton_Welcome_Option_2.Checked Then
                With Wizard1.WizardSteps.OfType(Of WizardStep)
                    l.Add(.Where(Function(x) x.ID = "SearchProspect_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "ListPendingReservations_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "EditProspect_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "ListPackagesOfferedByVendor_WizardStep").Single())

                    l.Add(.Where(Function(x) x.ID = "EditNotes_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "Confirmation_WizardStep").Single())
                End With


            ElseIf RadioButton_Welcome_Option_3.Checked Then
                With Wizard1.WizardSteps.OfType(Of WizardStep)


                    l.Add(.Where(Function(x) x.ID = "EditNotes_WizardStep").Single())
                    l.Add(.Where(Function(x) x.ID = "Confirmation_WizardStep").Single())
                End With

            ElseIf RadioButton_Welcome_Option_4.Checked Then
            Else

            End If

            Array.ForEach(Wizard1.WizardSteps.OfType(Of WizardStep).ToArray(),
                           Sub(ws As WizardStep)
                               If Wizard1.WizardSteps.IndexOf(ws) > 0 Then
                                   Wizard1.WizardSteps.Remove(ws)
                               End If
                           End Sub)
            Array.ForEach(l.ToArray(), Sub(ws As WizardStep) Wizard1.WizardSteps.Add(ws))
        End If
    End Sub
    Private Iterator Function Enumerate_Controls_Recursive(ByVal parent As Control) As IEnumerable(Of Control)
        For Each child As Control In parent.Controls
            Yield child
            For Each descendant As Control In Enumerate_Controls_Recursive(child)
                Yield descendant
            Next
        Next
    End Function
    Private Sub Reset_Controls()
        For Each ctl As Control In Enumerate_Controls_Recursive(Page)
            If TypeOf ctl Is TextBox Then
                CType(ctl, TextBox).Text = String.Empty
            ElseIf TypeOf ctl Is DropDownList Then
                CType(ctl, DropDownList).ClearSelection()
                CType(ctl, DropDownList).SelectedIndex = 0
            ElseIf TypeOf ctl Is CheckBox Then
                CType(ctl, CheckBox).Checked = False
            End If
        Next
        GridView_Addresses.DataSource = New List(Of WzAddress)(New WzAddress() {New WzAddress With {.Address_AddressID = 0, .Address_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}})
        GridView_Addresses.DataBind()

        GridView_Emails.DataSource = New List(Of WzEmail)(New WzEmail() {New WzEmail With {.Email_EmailID = 0, .Email_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}})
        GridView_Emails.DataBind()

        GridView_Phones.DataSource = New List(Of WzPhone)(New WzPhone() {New WzPhone With {.Phone_PhoneID = 0, .Phone_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}})
        GridView_Phones.DataBind()
    End Sub
    Private Sub Init_DropDownLists()
        Dim ddl = New List(Of DropDownList)(New DropDownList() {
            DropDownList_MaritalStatusID,
            DropDownList_Phone_TypeID,
            DropDownList_StateID,
            DropDownList_CountryID,
            DropDownList_Address_TypeID,
            DropDownList_Premium_StatusID,
            DropDownList_TourLocationID,
            DropDownList_TourSubTypeID,
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
            DropDownList_UnitTypes,
            DropDownList_Hotel_ReservationLocationID,
            DropDownList_Hotel_GuestTypeID,
            DropDownList_Hotel_RoomTypeID
        })

        Array.ForEach(ddl.ToArray(), Sub(dd As DropDownList) dd.ClearSelection())
        Dim cb = New List(Of String)(New String() {"MaritalStatus", "Phone", "State", "Country", "AddressType", "PremiumStatus", "TourLocation", "TourSubType", "TourSource", "TourType", "TourStatus", "ReservationSource", "ReservationSubType", "ReservationType", "ReservationStatus", "ResortCompany",
                                     "ReservationLocation", "PackageType", "PackageSubType", "UnitType", "ReservationLocation", "AccGuestType", "AccomRoomType"})

        ddl.Zip(cb, Function(x As DropDownList, y As String)
                        x.AppendDataBoundItems = True
                        x.Items.Add(New ListItem("(All)", ""))
                        x.DataSource = CType(New clsComboItems().Load_ComboItems(y).Select(DataSourceSelectArguments.Empty), DataView).ToTable()
                        x.DataTextField = "ComboItem"
                        x.DataValueField = "ComboItemID"
                        x.DataBind()
                        Return x
                    End Function).ToList()

        DropDownList_Reservation_Adults.DataSource = Enumerable.Range(1, 6)
        DropDownList_Reservation_Children.DataSource = Enumerable.Range(0, 8)

        DropDownList_Reservation_Adults.DataBind()
        DropDownList_Reservation_Children.DataBind()

        With DropDownList_CampaignID
            .ClearSelection()
            .Items.Clear()
            .AppendDataBoundItems = True
            .Items.Add(New ListItem("(All)", ""))
            .DataTextField = "Name"
            .DataValueField = "CampaignID"
            .DataSource = CType(New clsCampaign().List().Select(DataSourceSelectArguments.Empty), DataView).ToTable
            .DataBind()
        End With


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
         & "end as Room " _
         & "from t_Package p inner join t_ComboItems s on p.UnitTypeID = s.ComboItemID where len(p.Bedrooms) > 0 or p.Bedrooms is not null order by Room;"

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

        With DropDownList_Hotel_AccommodationID
            .AppendDataBoundItems = True
            .Items.Add("(All)")
            .DataTextField = "AccomName"
            .DataValueField = "AccomID"
            .DataSource = New clsAccom().Accoms_By_Location(1007, 0)
            .DataBind()
        End With

        With DropDownList_Hotel_CheckInLocationID
            .DataTextField = "Location"
            .DataValueField = "ID"
            .DataSource = New clsAccom2CheckInLocation().CheckIn_Locations_By_Accom(290, 0)
            .DataBind()
        End With
    End Sub
    Private Sub Clear_TextBoxes(list As List(Of TextBox))
        For Each tb As TextBox In list
            tb.Text = String.Empty
        Next
    End Sub
    Private Sub Clear_DropDownlists(list As List(Of DropDownList))
        For Each dd As DropDownList In list
            dd.ClearSelection()
        Next
    End Sub
    Private Sub Clear_CheckBoxes(list As List(Of CheckBox))
        For Each cb As CheckBox In list
            cb.Checked = False
        Next
    End Sub
    Class Utility
        Public Shared Function Get_ComboItem_String(comboItemID As Int32) As String
            Return New clsComboItems().Lookup_ComboItem(comboItemID)
        End Function
        Public Shared Function Search_Text(searchText As String, searchCategory As String, Optional vendorID As Integer = 0) As DataTable
            Dim Filter As New TextBox
            Filter.Text = searchText
            If searchText = Nothing Then searchText = String.Empty

            Select Case LCase(searchCategory)
                Case "phone"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & Filter.Text & "%' order by ph.number")
                        Else
                            Return Search_Query("Select top 50 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where ph.number like '" & Filter.Text & "%' and vp.VendorID in (" & vendorID = 0 & ") order by ph.number")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where ph.number like '" & Filter.Text & "%' and vp.VendorID in (" & vendorID = 0 & ")")
                        End If
                    End If

                Case "address1"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by Address1")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by Address1")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.address1 like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.Address1 from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where a.address1 like '" & Filter.Text & "%' and vp.VendorID in (" & vendorID = 0 & ")")
                        End If
                    End If

                Case "city"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.City")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by a.City")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.City like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.City from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and a.City like '" & Filter.Text & "%'")
                        End If
                    End If
                Case "state"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid order by s.Comboitem")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by s.Comboitem")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid where s.comboitem like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, s.comboitem as State from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_Comboitems s on s.comboitemid = a.stateid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and s.comboitem like '" & Filter.Text & "%'")
                        End If
                    End If
                Case "email"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid order by e.Email")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by e.Email")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid where e.Email like '" & Filter.Text & "%'  order by e.Email")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, e.Email from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid left outer join t_ProspectEmail e on e.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and e.Email like '" & Filter.Text & "%'  order by e.Email")
                        End If
                    End If

                Case "name"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by Lastname, Firstname")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by Lastname, Firstname")
                        End If
                    Else
                        If InStr(Filter.Text, ",") > 0 Then
                            Dim sName(2) As String
                            sName = Filter.Text.Split(",")
                            If vendorID = 0 Then
                                Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                            Else
                                Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%'")
                            End If
                        Else
                            If vendorID = 0 Then
                                Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.lastname  like '" & Filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                            Else
                                Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.lastname  like '" & Filter.Text.Replace(New Char() {"'"}, "''") & "%'")
                            End If
                        End If

                    End If

                Case "id"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.Prospectid")
                        Else
                            Return Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by p.Prospectid")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.Prospectid like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.ProspectID,p.LastName, p.FirstName, ph.Number from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.Prospectid like '" & Filter.Text & "%'")
                        End If
                    End If

                Case "postalcode"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid order by a.PostalCode")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by a.PostalCode")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid where a.PostalCode like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid left outer join t_ProspectAddress a on a.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and a.PostalCode like '" & Filter.Text & "%'")
                        End If
                    End If

                Case "ssn"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SSN")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by p.SSN")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SSN like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.SSN like '" & Filter.Text & "%'")
                        End If
                    End If

                Case "spousessn"
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by p.SpouseSSN")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by p.SpouseSSN")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where p.SpouseSSN like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, p.SpouseSSN from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and p.SpouseSSN like '" & Filter.Text & "%'")
                        End If
                    End If

                Case "LeadID".ToLower()
                    If Filter.Text.Length > 0 Then
                        If Integer.TryParse(Filter.Text, 0) Then
                            With New clsLeads
                                .LeadID = Filter.Text
                                .Load()
                                If .ProspectID < 1 Then
                                    Return Search_Query(String.Format("select LastName, FirstName, Coalesce(ProspectID, 0), LeadID [ProspectID] from t_Leads where leadID={0}", Filter.Text))
                                Else
                                    Return Search_Query(String.Format("select top 1 p.LastName, p.FirstName, Coalesce(p.ProspectID, 0) ProspectID, l.LeadID  from t_Prospect p inner join t_Leads l on p.ProspectID = l.ProspectID where p.ProspectID = {0} order by l.LeadID desc;", .ProspectID))
                                End If
                            End With

                        End If
                    End If

                Case Else
                    If Filter.Text = "" Then
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid order by ph.number")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") order by ph.number")
                        End If
                    Else
                        If vendorID = 0 Then
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.Number like '" & Filter.Text & "%'")
                        Else
                            Return Search_Query("Select top 100 p.LastName, p.FirstName, p.ProspectID, ph.Number as Phone, a.PostalCode from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid inner join t_PackageIssued pi on p.ProspectID = pi.ProspectID inner join t_Package pk on pi.PackageID = pk.PackageID inner join t_Vendor2Package vp on pk.PackageID = vp.PackageID where vp.VendorID in (" & vendorID = 0 & ") and ph.Number like '" & Filter.Text & "%'")
                        End If
                    End If
            End Select
            Return New DataTable
        End Function
        Private Shared Function Search_Query(sql As String) As DataTable
            Dim dt = New DataTable
            Using cn = New SqlConnection(Resources.Resource.cns)
                Try
                    Using cm = New SqlCommand(sql, cn)
                        cn.Open()
                        dt.Load(cm.ExecuteReader())
                    End Using
                Catch ex As Exception
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End Using
            Return dt
        End Function
    End Class

    Protected Sub Button_Edit_Address_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_Address_Submit.Click
        Dim sid = Label_Edit_Address.Attributes("Address_SID")
        Dim Address As WzAddress = Nothing

        If sid.StartsWith("New_") Then
            Address = New WzAddress
            Address.Address_SID = Guid.NewGuid.ToString
            Prospect.Prospect_Addresses.Add(Address)
        Else
            Address = Prospect.Prospect_Addresses.Where(Function(x) x.Address_SID = sid).Single
        End If

        With Address
            If DropDownList_Address_TypeID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_StateID.SelectedItem.Value) = False Then
                    .Address_StateID = DropDownList_StateID.SelectedItem.Value
                End If
            End If
            If DropDownList_Address_TypeID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_Address_TypeID.SelectedItem.Value) = False Then
                    .Address_TypeID = DropDownList_Address_TypeID.SelectedItem.Value
                End If
            End If
            If DropDownList_CountryID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_CountryID.SelectedItem.Value) = False Then
                    .Address_CountryID = DropDownList_CountryID.SelectedItem.Value
                End If
            End If
            .Address_Address1 = TextBox_Address1.Text.Trim()
            .Address_Address2 = TextBox_Address2.Text.Trim()
            .Address_City = TextBox_City.Text.Trim()
            .Address_Region = TextBox_Region.Text.Trim()
            .Address_PostalCode = TextBox_PostalCode.Text.Trim()
            .Address_ActiveFlag = CheckBox_ActiveFlag.Checked
            .Address_ContractAddress = CheckBox_ContractAddress.Checked
        End With

        GridView_Addresses.DataSource = Prospect.Prospect_Addresses.ToList()
        GridView_Addresses.DataBind()
        Label_Edit_Address.Attributes.Clear()
        MultiView_Prospect.SetActiveView(View_Prospect_Main)

    End Sub
    Protected Sub Button_Edit_Email_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_Email_Submit.Click
        Dim Sid = Label_Edit_Email.Attributes("Email_SID")
        Dim Email As WzEmail = Nothing

        If Sid.StartsWith("New_") Then
            Email = New WzEmail
            Email.Email_SID = Guid.NewGuid.ToString
            Prospect.Prospect_Emails.Add(Email)
        Else
            Email = Prospect.Prospect_Emails.Where(Function(x) x.Email_SID = Sid).Single
        End If

        With Email
            .Email_Email = TextBox_Email_Address.Text.Trim()
            .Email_IsActive = CheckBox_Email_IsActive.Checked
            .Email_IsPrimary = CheckBox_Email_IsPrimary.Checked
        End With

        GridView_Emails.DataSource = Prospect.Prospect_Emails.ToList()
        GridView_Emails.DataBind()
        Label_Edit_Email.Attributes.Clear()

        MultiView_Prospect.SetActiveView(View_Prospect_Main)
    End Sub
    Protected Sub Button_Edit_Phone_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_Phone_Submit.Click
        Dim sid = Label_Edit_Phone.Attributes("Phone_SID")
        Dim Phone As WzPhone = Nothing

        If sid.StartsWith("New_") Then
            Phone = New WzPhone
            Phone.Phone_SID = Guid.NewGuid.ToString
            Prospect.Prospect_Phones.Add(Phone)
        Else
            Phone = Prospect.Prospect_Phones.Where(Function(x) x.Phone_SID = sid).Single
        End If

        With Phone
            If DropDownList_Phone_TypeID.SelectedValue IsNot Nothing Then
                If String.IsNullOrEmpty(DropDownList_Phone_TypeID.SelectedItem.Value) = False Then
                    .Phone_TypeID = DropDownList_Phone_TypeID.SelectedItem.Value
                End If
            End If

            .Phone_Extension = TextBox_Phone_Extension.Text.Trim
            .Phone_Number = TextBox_Phone_Number.Text.Trim
            .Phone_Active = CheckBox_Phone_Active.Checked
        End With

        GridView_Phones.DataSource = Prospect.Prospect_Phones.ToList()
        GridView_Phones.DataBind()
        Label_Edit_Phone.Attributes.Clear()
        MultiView_Prospect.SetActiveView(View_Prospect_Main)

    End Sub
    Protected Sub Button_Edit_Email_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_Email_Cancel.Click
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
    End Sub
    Protected Sub Button_Edit_Phone_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_Phone_Cancel.Click
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
    End Sub
    Protected Sub Button_Edit_Address_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_Address_Cancel.Click
        MultiView_Prospect.SetActiveView(View_Prospect_Main)
    End Sub
    Protected Sub Button_Edit_PremiumIssued_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Edit_PremiumIssued_Cancel.Click
        MultiView_Tour.SetActiveView(View_Tour)
    End Sub
    Protected Sub Button_Edit_PremiumIssued_Submit_Click(sender As Object, e As EventArgs) Handles Button_Edit_PremiumIssued_Submit.Click
        Dim pre = Prospect.Prospect_Premiums.Where(Function(x) x.Premium_SID = CType(sender, Button).Attributes("Premium_SID")).Single
        With pre
            .Premium_PremiumID = DropDownListList_Premiums.SelectedItem.Value
            .Premium_PremiumName = DropDownListList_Premiums.SelectedItem.Text
            .Premium_StatusID = DropDownList_Premium_StatusID.SelectedItem.Value
            .Premium_CostEA = TextBox_Premium_CostEA.Text
            .Premium_QtyAssigned = DropDownList_Premium_QtyAssigned.SelectedItem.Text
        End With

        With GridView_ListPremiums
            .DataSource = Prospect.Prospect_Premiums
            .DataBind()
        End With

        CType(sender, Button).Attributes.Clear()
        MultiView_Tour.SetActiveView(View_Tour)
    End Sub
    Protected Sub LinkButton_Address_Add_Click(sender As Object, e As EventArgs)
        Dim Address = New WzAddress With {.Address_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}
        Label_Edit_Address.Attributes.Add("Address_SID", Address.Address_SID)
        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Address1, TextBox_Address2, TextBox_City, TextBox_PostalCode, TextBox_Region})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_ActiveFlag, CheckBox_ContractAddress})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_CountryID, DropDownList_StateID, DropDownList_Address_TypeID})

        Clear_TextBoxes(txb)
        Clear_CheckBoxes(ckb)
        Clear_DropDownlists(ddl)

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Address)
    End Sub
    Protected Sub LinkButton_Phone_Add_Click(sender As Object, e As EventArgs)
        Dim Phone = New WzPhone With {.Phone_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}
        Label_Edit_Phone.Attributes.Add("Phone_SID", Phone.Phone_SID)
        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Phone_Number, TextBox_Phone_Extension})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Phone_Active})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_Phone_TypeID})

        Clear_TextBoxes(txb)
        Clear_CheckBoxes(ckb)
        Clear_DropDownlists(ddl)
        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Phone)
    End Sub
    Protected Sub LinkButton_Email_Add_Click(sender As Object, e As EventArgs)
        Dim Email = New WzEmail With {.Email_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}
        Label_Edit_Email.Attributes.Add("Email_SID", Email.Email_SID)
        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Email_Address})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Email_IsActive, CheckBox_Email_IsPrimary})

        Clear_TextBoxes(txb)
        Clear_CheckBoxes(ckb)
        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Email)
    End Sub
    Protected Sub LinkButton_Email_Edit_Click(sender As Object, e As EventArgs)
        Dim lk = CType(sender, LinkButton)
        Dim gvr = CType(lk.NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Email_Address})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Email_IsPrimary, CheckBox_Email_IsActive})

        If Prospect.Prospect_Emails.Count = 0 Then
            Prospect.Prospect_Email = New WzEmail With {.Email_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}
        Else
            Prospect.Prospect_Email = Prospect.Prospect_Emails.Where(Function(x) x.Email_SID = gv.DataKeys(gvr.RowIndex).Value).Single()
        End If

        Label_Edit_Email.Attributes.Add("Email_SID", Prospect.Prospect_Email.Email_SID)

        txb.ForEach(Sub(c As TextBox) c.DataBind())
        ckb.ForEach(Sub(c As CheckBox) c.DataBind())

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Email)

    End Sub
    Protected Sub LinkButton_Phone_Edit_Click(sender As Object, e As EventArgs)
        Dim lk = CType(sender, LinkButton)
        Dim gvr = CType(lk.NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Phone_Number, TextBox_Phone_Extension})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_Phone_Active})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_Phone_TypeID})

        ddl.ForEach(Sub(c As DropDownList) c.ClearSelection())

        If Prospect.Prospect_Phones.Count = 0 Then
            Prospect.Prospect_Phone = New WzPhone With {.Phone_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}
        Else
            Prospect.Prospect_Phone = Prospect.Prospect_Phones.Where(Function(x) x.Phone_SID = gv.DataKeys(gvr.RowIndex).Value).Single()
        End If

        Label_Edit_Phone.Attributes.Add("Phone_SID", Prospect.Prospect_Phone.Phone_SID)

        If DropDownList_Phone_TypeID.Items.FindByValue(Prospect.Prospect_Phone.Phone_TypeID) IsNot Nothing Then
            DropDownList_Phone_TypeID.Items.FindByValue(Prospect.Prospect_Phone.Phone_TypeID).Selected = True
        End If

        txb.ForEach(Sub(c As TextBox) c.DataBind())
        ddl.ForEach(Sub(c As DropDownList) c.DataBind())
        ckb.ForEach(Sub(c As CheckBox) c.DataBind())

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Phone)
    End Sub
    Protected Sub LinkButton_Address_Edit_Click(sender As Object, e As EventArgs)
        Dim lk = CType(sender, LinkButton)
        Dim gvr = CType(lk.NamingContainer, GridViewRow)
        Dim gv = CType(gvr.NamingContainer, GridView)

        Dim txb = New List(Of TextBox)(New TextBox() {TextBox_Address1, TextBox_Address2, TextBox_City, TextBox_PostalCode, TextBox_Region})
        Dim ckb = New List(Of CheckBox)(New CheckBox() {CheckBox_ActiveFlag, CheckBox_ContractAddress})
        Dim ddl = New List(Of DropDownList)(New DropDownList() {DropDownList_CountryID, DropDownList_StateID, DropDownList_Address_TypeID})
        Dim lbl = New List(Of Label)(New Label() {Label_Edit_Address})

        ddl.ForEach(Sub(c As DropDownList) c.ClearSelection())

        If Prospect.Prospect_Addresses.Count = 0 Then
            Prospect.Prospect_Address = New WzAddress With {.Address_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}
        Else
            Prospect.Prospect_Address = Prospect.Prospect_Addresses.Where(Function(x) x.Address_SID = gv.DataKeys(gvr.RowIndex).Value).Single()
        End If

        Label_Edit_Address.Attributes.Add("Address_SID", Prospect.Prospect_Address.Address_SID)

        If DropDownList_CountryID.Items.FindByValue(Prospect.Prospect_Address.Address_CountryID) IsNot Nothing Then
            DropDownList_CountryID.Items.FindByValue(Prospect.Prospect_Address.Address_CountryID).Selected = True
        End If
        If DropDownList_StateID.Items.FindByValue(Prospect.Prospect_Address.Address_StateID) IsNot Nothing Then
            DropDownList_StateID.Items.FindByValue(Prospect.Prospect_Address.Address_StateID).Selected = True
        End If
        If DropDownList_Address_TypeID.Items.FindByValue(Prospect.Prospect_Address.Address_TypeID) IsNot Nothing Then
            DropDownList_Address_TypeID.Items.FindByValue(Prospect.Prospect_Address.Address_TypeID).Selected = True
        End If

        txb.ForEach(Sub(c As TextBox) c.DataBind())
        ddl.ForEach(Sub(c As DropDownList) c.DataBind())
        ckb.ForEach(Sub(c As CheckBox) c.DataBind())
        lbl.ForEach(Sub(c As Label) c.DataBind())

        MultiView_Prospect.SetActiveView(View_Prospect_Edit_Address)
    End Sub
    Protected Sub Wizard1_SideBarButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.SideBarButtonClick

        If Wizard1.WizardSteps(e.NextStepIndex).ID.IndexOf("_") = 0 Then Return

        Dim prefix = Wizard1.WizardSteps(e.NextStepIndex).ID.Substring(0, Wizard1.WizardSteps(e.NextStepIndex).ID.IndexOf("_"))
        Dim panel As Panel = Panels.Where(Function(x) x.ID.Substring(0, x.ID.IndexOf("_")).Equals(prefix)).SingleOrDefault
        If panel IsNot Nothing Then
            panel.Visible = True
            Response.Write(panel.ID)
        End If
    End Sub
    Protected Sub Wizard1_ActiveStepChanged(sender As Object, e As EventArgs) Handles Wizard1.ActiveStepChanged
        If Panels Is Nothing Then Return

        Dim prefix = Wizard1.WizardSteps(Wizard1.ActiveStepIndex).ID.Substring(0, Wizard1.WizardSteps(Wizard1.ActiveStepIndex).ID.IndexOf("_"))
        Dim panel As Panel = Panels.Where(Function(x) x.ID.Substring(0, x.ID.IndexOf("_")).Equals(prefix)).SingleOrDefault
        If panel IsNot Nothing Then
            HiddenField_CurrentPanelID.Value = panel.ID
        End If

        If panel.ID = "AssignRooms_Panel" Then
            AssignRooms_Panel_Load()
        ElseIf panel.ID = "ListPendingReservations_Panel" Then
            ListPendingReservations_Panel_Load()
        End If
    End Sub
    Protected Sub Button_Save_Click(sender As Object, e As EventArgs) Handles Button_Save.Click
        Save_Db()
    End Sub
    Protected Sub Button_Search_Available_Packages_For_Purchasing_Click(sender As Object, e As EventArgs) Handles Button_Search_Available_Packages_For_Purchasing.Click

        If String.IsNullOrEmpty(TextBox_Search_CheckIn.Text) = False And String.IsNullOrEmpty(TextBox_Search_CheckOut.Text) = False Then
            Dim nights = DateTime.Parse(TextBox_Search_CheckOut.Text).Subtract(DateTime.Parse(TextBox_Search_CheckIn.Text)).Days
            Dim dt = New clsReservationWizard().Available_Packages(TextBox_Search_CheckIn.Text, nights, 1, "crms.kingscreekplantation.com")
            With GridView_ListPackagesForSales
                .DataSource = dt
                .DataBind()
            End With
        End If
    End Sub
    Private Sub _Default1_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            Prospect.Prospect_ProspectID = 9
            Load_Db()
        End If

        With Prospect
            If TextBox_Prospect_ProspectID.Text.Length > 0 Then
                .Prospect_ProspectID = TextBox_Prospect_ProspectID.Text
            End If
            .Prospect_FirstName = TextBox_Prospect_FirstName.Text.Trim
            .Prospect_LasttName = TextBox_Prospect_LastName.Text.Trim
            .Prospect_SpouseFirstName = TextBox_Spouse_FirstName.Text.Trim
            .Prospect_SpouseLastName = TextBox_SpouseLastName.Text.Trim
            If String.IsNullOrEmpty(DropDownList_MaritalStatusID.SelectedItem.Value) = False Then
                .Prospect_MaritalStatusID = DropDownList_MaritalStatusID.SelectedItem.Value
            End If

            If TextBox_ReservationID.Text.Length > 0 Then
                .Reservation_ReservationID = TextBox_ReservationID.Text
            End If

            .Reservation_ReservationNumber = TextBox_Reservation_Number.Text.Trim
            .Reservation_CheckInDate = TextBox_Reservation_CheckIn.Text
            .Reservation_CheckOutDate = TextBox_Reservation_CheckOut.Text
            .Reservation_DateBooked = TextBox_Reservation_DateBooked.Text
            .Reservation_StatusDate = TextBox_Reservation_StatusDate.Text
            .Reservation_LockInventory = CheckBox_Reservation_LockInventory.Checked

            If String.IsNullOrEmpty(DropDownList_Reservation_LocationID.SelectedItem.Value) = False Then
                .Reservation_Reservation_LocationID = DropDownList_Reservation_LocationID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_Resort_CompanyID.SelectedItem.Value) = False Then
                .Reservation_ResortCompanyID = DropDownList_Reservation_Resort_CompanyID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_StatusID.SelectedItem.Value) = False Then
                .Reservation_StatusID = DropDownList_Reservation_StatusID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_TypeID.SelectedItem.Value) = False Then
                .Reservation_TypeID = DropDownList_Reservation_TypeID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_SubTypeID.SelectedItem.Value) = False Then
                .Reservation_SubTypeID = DropDownList_Reservation_SubTypeID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_Adults.SelectedItem.Value) = False Then
                .Reservation_NumberAdults = DropDownList_Reservation_Adults.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_Children.SelectedItem.Value) = False Then
                .Reservation_NumberChildren = DropDownList_Reservation_Children.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_Reservation_SourceID.SelectedItem.Value) = False Then
                .Reservation_SourceID = DropDownList_Reservation_SourceID.SelectedItem.Value
            End If

            If TextBox_TourID.Text.Length > 0 Then
                .Tour_TourID = TextBox_TourID.Text
            End If

            .Tour_Date = TextBox_TourDate.Text
            .Tour_BookingDate = TextBox_BookingDate.Text

            If String.IsNullOrEmpty(DropDownList_CampaignID.SelectedItem.Value) = False Then
                .Tour_CampaignID = DropDownList_CampaignID.SelectedItem.Value
            End If

            If String.IsNullOrEmpty(DropDownList_TourTypeID.SelectedItem.Value) = False Then
                .Tour_TypeID = DropDownList_TourTypeID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_TourSubTypeID.SelectedItem.Value) = False Then
                .Tour_SubTypeID = DropDownList_TourSubTypeID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_TourSourceID.SelectedItem.Value) = False Then
                .Tour_SourceID = DropDownList_TourSourceID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_TourLocationID.SelectedItem.Value) = False Then
                .Tour_TourLocationID = DropDownList_TourLocationID.SelectedItem.Value
            End If
            If String.IsNullOrEmpty(DropDownList_TourTime.SelectedValue) = False Then
                If String.IsNullOrEmpty(DropDownList_TourTime.SelectedItem.Value) = False Then
                    .Tour_TourTime = DropDownList_TourTime.SelectedItem.Value
                End If
            End If
        End With

        Array.ForEach(Panels.ToArray(), Sub(p As Panel)
                                            If p.ID = HiddenField_CurrentPanelID.Value Then
                                                p.Visible = True
                                            End If
                                        End Sub)
    End Sub
    Private Sub Load_Db()

        For Each row As DataRow In New clsAddress() With {.ProspectID = Prospect.Prospect_ProspectID}.Get_Table().Rows
            With New clsAddress
                .AddressID = row("ID").ToString
                .Load()
                Dim address = New WzAddress
                address.Address_SID = Guid.NewGuid.ToString
                address.Address_AddressID = .AddressID
                address.Address_Address1 = .Address1
                address.Address_Address2 = .Address2
                address.Address_City = .City
                address.Address_StateID = .StateID
                address.Address_PostalCode = .PostalCode
                address.Address_Region = .Region
                address.Address_CountryID = .CountryID
                address.Address_TypeID = .TypeID
                address.Address_ActiveFlag = .ActiveFlag
                address.Address_ContractAddress = .ContractAddress

                Prospect.Prospect_Addresses.Add(address)
            End With
        Next
        For Each row As DataRow In New clsEmail() With {.ProspectID = Prospect.Prospect_ProspectID}.Get_Table().Rows
            With New clsEmail
                .EmailID = row("ID").ToString
                .Load()
                Dim email = New WzEmail
                email.Email_SID = Guid.NewGuid.ToString
                email.Email_EmailID = .EmailID
                email.Email_Email = .Email
                email.Email_IsActive = .IsActive
                email.Email_IsPrimary = .IsPrimary

                Prospect.Prospect_Emails.Add(email)
            End With
        Next
        For Each row As DataRow In New clsPhone() With {.ProspectID = Prospect.Prospect_ProspectID}.Get_Table().Rows
            With New clsPhone
                .PhoneID = row("ID").ToString
                .Load()
                Dim phone = New WzPhone
                phone.Phone_SID = Guid.NewGuid.ToString
                phone.Phone_PhoneID = .PhoneID
                phone.Phone_Number = .Number
                phone.Phone_Extension = .Extension
                phone.Phone_TypeID = .TypeID
                phone.Phone_Active = .Active

                Prospect.Prospect_Phones.Add(phone)
            End With
        Next

        With Prospect
            GridView_Addresses.DataSource = .Prospect_Addresses.ToList
            GridView_Emails.DataSource = .Prospect_Emails.ToList
            GridView_Phones.DataSource = .Prospect_Phones.ToList

            If .Prospect_Addresses.Count = 0 Then GridView_Addresses.DataSource = New List(Of WzAddress)(New WzAddress() {New WzAddress With {.Address_AddressID = 0, .Address_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}})
            If .Prospect_Emails.Count = 0 Then GridView_Emails.DataSource = New List(Of WzEmail)(New WzEmail() {New WzEmail With {.Email_EmailID = 0, .Email_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}})
            If .Prospect_Phones.Count = 0 Then GridView_Phones.DataSource = New List(Of WzPhone)(New WzPhone() {New WzPhone With {.Phone_PhoneID = 0, .Phone_SID = String.Format("New_{0}", Guid.NewGuid.ToString)}})

            GridView_Addresses.DataBind()
            GridView_Emails.DataBind()
            GridView_Phones.DataBind()
        End With

        With New clsProspect
            .Prospect_ID = Prospect.Prospect_ProspectID
            .Load()
            TextBox_Prospect_ProspectID.Text = Prospect.Prospect_ProspectID
            TextBox_Prospect_FirstName.Text = .First_Name
            TextBox_Prospect_LastName.Text = .Last_Name
            TextBox_Spouse_FirstName.Text = .SpouseFirstName
            TextBox_SpouseLastName.Text = .SpouseLastName
            If DropDownList_MaritalStatusID.Items.FindByValue(.MaritalStatusID) IsNot Nothing Then
                DropDownList_MaritalStatusID.Items.FindByValue(.MaritalStatusID).Selected = True
            End If
        End With

        Dim objTour = New clsTour
        Dim objReservation = New clsReservations

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand()
                Try
                    Dim sq = String.Format("select *,
                        prp.PaymentMethodID, prftc.FinTransCodeID, pr.TypeID 'InventoryTypeID', p.UnitTypeID, p.Description, p.Package
                        from t_Package p 
                        inner join t_PackageReservation pr on p.PackageID = pr.PackageID
                        inner join t_PackageTour pt on pt.PackageReservationID = pr.PackageReservationID
                        left join t_PackageReservationFinTransCode prftc on prftc.PackageReservationID = pr.PackageReservationID
                        left join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID
                        where p.Active = 1
                        and p.PackageID in (248)")

                    Dim dt = New DataTable
                    With cm
                        cn.Open()
                        .Connection = cn
                        .CommandText = sq
                        dt.Load(.ExecuteReader())
                    End With

                    If dt.Rows.Count = 1 Then
                        Dim dr = dt.Rows(0)

                        With New clsPackageTour
                            .PackageTourID = dr("PackageTourID").ToString
                            .Load()
                            objTour.BookingDate = DateTime.Now.ToShortDateString()
                            objTour.StatusID = .TourStatusID
                            objTour.TypeID = .TourTypeID
                            objTour.SubTypeID = .TourSubTypeID
                            objTour.SourceID = .TourSourceID
                            objTour.TourLocationID = .TourLocationID
                            objTour.CampaignID = .CampaignID
                        End With

                        With New clsPackageReservation
                            .PackageReservationID = dr("PackageReservationID").ToString
                            .Load()
                            objReservation.SourceID = .SourceID
                            objReservation.StatusID = .StatusID
                            objReservation.ResLocationID = .LocationID
                            objReservation.ResortCompanyID = .ResortCompanyID
                            objReservation.TypeID = .TypeID
                            objReservation.StatusDate = DateTime.Now.ToShortDateString
                            objReservation.DateBooked = DateTime.Now.ToShortDateString
                        End With

                        dt = Nothing
                        dt = New DataTable

                        With cm
                            .CommandText = String.Format("select * from t_PackageTourPremium where PackageID = 248 and PremiumStatusID is not null")
                            dt.Load(cm.ExecuteReader())
                        End With

                        For Each row As DataRow In dt.Rows
                            With New clsPackageTourPremium
                                .PackageTourPremiumID = row("PackageTourPremiumID").ToString
                                .Load()

                                Dim objPremium = New WzPremium
                                objPremium.Premium_SID = Guid.NewGuid.ToString
                                objPremium.Premium_PremiumID = .PremiumID
                                objPremium.Premium_CostEA = .CostEA
                                objPremium.Premium_QtyAssigned = .QtyAssigned
                                objPremium.Premium_StatusID = .PremiumStatusID
                                objPremium.Premium_Optional = .OptionalPrem
                                With New clsPremium
                                    .PremiumID = objPremium.Premium_PremiumID
                                    .Load()
                                    objPremium.Premium_PremiumName = .PremiumName
                                End With
                                Prospect.Prospect_Premiums.Add(objPremium)
                            End With
                        Next
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Using

        With objTour
            .TourID = Prospect.Tour_TourID
            '.Load()
            TextBox_TourID.Text = .TourID
            TextBox_TourDate.Text = .TourDate
            TextBox_BookingDate.Text = .BookingDate
            If DropDownList_TourStatusID.Items.FindByValue(.StatusID) IsNot Nothing Then
                DropDownList_TourStatusID.Items.FindByValue(.StatusID).Selected = True
            End If
            If DropDownList_TourTypeID.Items.FindByValue(.TypeID) IsNot Nothing Then
                DropDownList_TourTypeID.Items.FindByValue(.TypeID).Selected = True
            End If
            If DropDownList_TourSourceID.Items.FindByValue(.SourceID) IsNot Nothing Then
                DropDownList_TourSourceID.Items.FindByValue(.SourceID).Selected = True
            End If
            If DropDownList_TourTime.Items.FindByValue(.TourTime) IsNot Nothing Then
                DropDownList_TourTime.Items.FindByValue(.TourTime).Selected = True
            End If
            If DropDownList_TourLocationID.Items.FindByValue(.TourLocationID) IsNot Nothing Then
                DropDownList_TourLocationID.Items.FindByValue(.TourLocationID).Selected = True
            End If
            If DropDownList_TourSubTypeID.Items.FindByValue(.SubTypeID) IsNot Nothing Then
                DropDownList_TourSubTypeID.Items.FindByValue(.SubTypeID).Selected = True
            End If
            If DropDownList_CampaignID.Items.FindByValue(.CampaignID) IsNot Nothing Then
                DropDownList_CampaignID.Items.FindByValue(.CampaignID).Selected = True
            End If

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("Select pi.PremiumIssuedID from t_PremiumIssued pi where pi.KeyField = 'TourID' and pi.KeyValue = {0}", 567498), cn)
                    Try
                        cn.Open()
                        Dim dt = New DataTable
                        dt.Load(cm.ExecuteReader)

                        Prospect.Prospect_Premiums.Clear()

                        For Each dr As DataRow In dt.Rows
                            With New clsPremiumIssued
                                .PremiumIssuedID = dr("PremiumIssuedID").ToString
                                .Load()
                                Dim obj = New WzPremium
                                obj.Premium_SID = Guid.NewGuid.ToString
                                obj.Premium_PremiumIssuedID = .PremiumIssuedID
                                obj.Premium_PremiumID = .PremiumID
                                obj.Premium_CostEA = .CostEA
                                obj.Premium_CertificateNumber = .CertificateNumber
                                obj.Premium_QtyAssigned = .QtyAssigned
                                obj.Premium_StatusID = .StatusID
                                With New clsPremium
                                    .PremiumID = obj.Premium_PremiumID
                                    .Load()
                                    obj.Premium_PremiumName = .PremiumName
                                End With
                                Prospect.Prospect_Premiums.Add(obj)
                            End With
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End Using

            With GridView_ListPremiums
                .DataSource = Prospect.Prospect_Premiums
                .DataBind()
            End With
        End With

        With objReservation
            .ReservationID = Prospect.Reservation_ReservationID
            ' .Load()

            TextBox_ReservationID.Text = .ReservationID
            TextBox_Reservation_Number.Text = .ReservationNumber
            TextBox_Reservation_CheckIn.Text = .CheckInDate
            TextBox_Reservation_CheckOut.Text = .CheckOutDate
            TextBox_Reservation_DateBooked.Text = .DateBooked
            TextBox_Reservation_StatusDate.Text = .StatusDate
            CheckBox_Reservation_LockInventory.Checked = .LockInventory

            DropDownList_Reservation_Total_Nights.ClearSelection()
            DropDownList_Reservation_Total_Nights.Items.Clear()

            If String.IsNullOrEmpty(.CheckInDate) = False And String.IsNullOrEmpty(.CheckOutDate) = False Then
                DropDownList_Reservation_Total_Nights.DataSource = Enumerable.Range(DateTime.Parse(.CheckOutDate).Subtract(DateTime.Parse(.CheckInDate)).Days, 1)
                DropDownList_Reservation_Total_Nights.DataBind()
            End If

            If DropDownList_Reservation_LocationID.Items.FindByValue(.ResLocationID) IsNot Nothing Then
                DropDownList_Reservation_LocationID.Items.FindByValue(.ResLocationID).Selected = True
            End If
            If DropDownList_Reservation_Resort_CompanyID.Items.FindByValue(.ResortCompanyID) IsNot Nothing Then
                DropDownList_Reservation_Resort_CompanyID.Items.FindByValue(.ResortCompanyID).Selected = True
            End If
            If DropDownList_Reservation_StatusID.Items.FindByValue(.StatusID) IsNot Nothing Then
                DropDownList_Reservation_StatusID.Items.FindByValue(.StatusID).Selected = True
            End If
            If DropDownList_Reservation_TypeID.Items.FindByValue(.TypeID) IsNot Nothing Then
                DropDownList_Reservation_TypeID.Items.FindByValue(.TypeID).Selected = True
            End If
            If DropDownList_Reservation_SubTypeID.Items.FindByValue(.SubTypeID) IsNot Nothing Then
                DropDownList_Reservation_SubTypeID.Items.FindByValue(.SubTypeID).Selected = True
            End If
            If DropDownList_Reservation_Adults.Items.FindByValue(.NumberAdults) IsNot Nothing Then
                DropDownList_Reservation_Adults.Items.FindByValue(.NumberAdults).Selected = True
            End If
            If DropDownList_Reservation_Children.Items.FindByValue(.NumberChildren) IsNot Nothing Then
                DropDownList_Reservation_Children.Items.FindByValue(.NumberChildren).Selected = True
            End If
            If DropDownList_Reservation_SourceID.Items.FindByValue(.SourceID) IsNot Nothing Then
                DropDownList_Reservation_SourceID.Items.FindByValue(.SourceID).Selected = True
            End If
        End With

        Prospect.Reservation_CheckInDate = "11/2/2019"
        Prospect.Reservation_CheckOutDate = "11/6/2019"
        Prospect.Tour_CampaignID = 5597
        Prospect.Tour_TourLocationID = 16975

        With DropDownList_TourTime
            .ClearSelection()
            .Items.Clear()
        End With

        'Load Tour Waves
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand("select * from dbo.ufn_TourAvailability(@StartDate, @Days, @CampID, @LocID)", cn)
                Try
                    cn.Open()
                    With Prospect
                        cm.CommandType = CommandType.Text
                        cm.Parameters.AddWithValue("@StartDate", DateTime.Parse(.Reservation_CheckInDate).AddDays(1).ToShortDateString())
                        cm.Parameters.AddWithValue("@Days", DateTime.Parse(.Reservation_CheckOutDate).AddDays(-1).Subtract(DateTime.Parse(.Reservation_CheckInDate).AddDays(1)).Days)
                        cm.Parameters.AddWithValue("@CampID", .Tour_CampaignID)
                        cm.Parameters.AddWithValue("@LocID", .Tour_TourLocationID)
                    End With

                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())

                    If dt.Rows.Count > 0 Then
                        Dim k = 0
                        Dim dic As New Dictionary(Of String, String)
                        dic.Add("8:00 AM", "800")
                        dic.Add("8:30 AM", "830")
                        dic.Add("9:00 AM", "900")
                        dic.Add("9:30 AM", "930")
                        dic.Add("10:00 AM", "1000")
                        dic.Add("10:30 AM", "1030")
                        dic.Add("11:00 AM", "1100")
                        dic.Add("11:30 AM", "1130")
                        dic.Add("12:00 PM", "1200")
                        dic.Add("12:30 PM", "1230")
                        dic.Add("1:00 PM", "1300")
                        dic.Add("1:30 PM", "1330")
                        dic.Add("2:00 PM", "1400")
                        dic.Add("2:30 PM", "1430")
                        dic.Add("3:00 PM", "1500")
                        dic.Add("3:30 PM", "1530")
                        dic.Add("4:00 PM", "1600")
                        dic.Add("4:30 PM", "1630")
                        dic.Add("5:30 PM", "1730")
                        dic.Add("6:00 PM", "1800")
                        dic.Add("6:30 PM", "1830")
                        dic.Add("7:00 PM", "1900")
                        dic.Add("8:00 PM", "2000")
                        dic.Add("8:15 PM", "2015")

                        dt.Columns.Add("ComboItemID")
                        For Each dr As DataRow In dt.Rows
                            dr("TourDate") = DateTime.Parse(dr("TourDate")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                            If dr("TourTime").ToString = "12:00 AM" Then
                                dr("TourTime") = "12:00 PM"
                            End If
                            If dr("TourTime").ToString = "12:30 AM" Then
                                dr("TourTime") = "12:30 PM"
                            End If
                            Dim dic_val = dic(dr("TourTime").ToString)
                            dr("ComboItemID") = New clsComboItems().Lookup_ID("TourTime", dic_val)
                        Next

                        For Each gr In dt.AsEnumerable().GroupBy(Function(x) x.Field(Of DateTime)("TourDate").ToShortDateString())
                            Dim tour_times = New List(Of String)
                            For Each dr As DataRow In gr
                                tour_times.Add(String.Format("{0},{1}", dr("TourTime").ToString, dr("ComboItemID").ToString))
                            Next
                            With DropDownList_TourDate
                                .Attributes.Add(DateTime.Parse(gr.Key).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), String.Join(";", tour_times.ToArray()))
                            End With
                            If k = 0 Then
                                With DropDownList_TourTime
                                    .DataTextField = "TourTime"
                                    .DataValueField = "ComboItemID"
                                    .DataSource = dt.AsEnumerable().GroupBy(Function(x) x.Field(Of DateTime)("TourDate")).First().CopyToDataTable()
                                    .DataBind()
                                End With
                                k += 1
                            End If
                        Next

                        With DropDownList_TourDate
                            .DataSource = dt.AsEnumerable().Select(Function(x) DateTime.Parse(x("TourDate").ToString).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)).Distinct()
                            .DataBind()
                        End With
                    End If

                    'If dt.Rows.Count > 0 Then

                    '    Dim tour_dates = New DataTable
                    '    tour_dates.Columns.AddRange(New DataColumn() {New DataColumn("TourDate"), New DataColumn("TourTime")})

                    '    For Each gr In dt.AsEnumerable().GroupBy(Function(x) x.Field(Of DateTime)("TOURDATE").ToShortDateString())
                    '        Dim newRow = tour_dates.NewRow
                    '        For Each dr As DataRow In gr
                    '            newRow("TourDate") = DateTime.Parse(gr.Key).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                    '            newRow("TourTime") = New clsComboItems().Lookup_ID("TourTime", dr("TOURTIME").ToString)
                    '        Next
                    '        With DropDownList_TourDate
                    '            .Attributes.Add(DateTime.Parse(gr.Key).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), String.Join(";", tour_times.ToArray()))
                    '        End With
                    '        tour_dates.Rows.Add(newRow)

                    '        If k = 0 Then
                    '            With DropDownList_TourTime
                    '                .DataTextField = 0
                    '                .DataValueField = 0
                    '                .DataSource = tour_times
                    '                .DataBind()
                    '            End With
                    '            k += 1
                    '        End If
                    '    Next
                    '    With DropDownList_TourDate
                    '        .DataSource = tour_dates
                    '        .DataBind()
                    '    End With
                    'End If
                Catch ex As Exception
                    Dim er = ex.Message
                Finally
                    cn.Close()
                End Try
            End Using
        End Using


    End Sub
    Private Sub Save_Db()
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(String.Format("insert into t_event values('reservationid', {0}, null, null, null, null, '{1}', null, null)", New Random().Next(), DateTime.Now), cn)

                Try
                    With New clsProspect
                        .Prospect_ID = Prospect.Prospect_ProspectID
                        .Load()
                        .First_Name = Prospect.Prospect_FirstName
                        .Last_Name = Prospect.Prospect_LasttName
                        .MaritalStatusID = Prospect.Prospect_MaritalStatusID
                        .SpouseFirstName = Prospect.Prospect_SpouseFirstName
                        .SpouseLastName = Prospect.Prospect_SpouseLastName
                        .Save()
                    End With

                    For Each address As WzAddress In Prospect.Prospect_Addresses
                        With New clsAddress
                            .AddressID = address.Address_AddressID
                            .Load()
                            .ProspectID = Prospect.Prospect_ProspectID
                            .Address1 = address.Address_Address1
                            .Address2 = address.Address_Address2
                            .City = address.Address_City
                            .StateID = address.Address_StateID
                            .PostalCode = address.Address_PostalCode
                            .Region = address.Address_Region
                            .CountryID = address.Address_CountryID
                            .TypeID = address.Address_TypeID
                            .ActiveFlag = address.Address_ActiveFlag
                            .ContractAddress = address.Address_ContractAddress
                            .UserID = 0
                            .Save()
                        End With
                    Next
                    For Each email As WzEmail In Prospect.Prospect_Emails
                        With New clsEmail
                            .EmailID = email.Email_EmailID
                            .Load()
                            .ProspectID = Prospect.Prospect_ProspectID
                            .Email = email.Email_Email
                            .IsPrimary = email.Email_IsPrimary
                            .IsActive = email.Email_IsActive
                            .UserID = 0
                            .Save()
                        End With
                    Next
                    For Each phone As WzPhone In Prospect.Prospect_Phones
                        With New clsPhone
                            .PhoneID = phone.Phone_PhoneID
                            .Load()
                            .ProspectID = Prospect.Prospect_ProspectID
                            .Number = phone.Phone_Number
                            .Extension = phone.Phone_Extension
                            .TypeID = phone.Phone_TypeID
                            .Active = phone.Phone_Active
                            .UserID = 0
                            .Save()
                        End With
                    Next

                    Dim reservation = New clsReservations
                    Dim tour = New clsTour
                    Dim package_issued = New clsPackageIssued
                    Dim package_issued_2_package = New clsPackageIssued2Package
                    Dim package_issued_2_room = New clsPackageIssued2Room

                    With package_issued
                        .PackageIssuedID = 0
                        .Load()
                        .ProspectID = Prospect.Prospect_ProspectID
                        .PackageID = 0
                        .PurchaseDate = DateTime.Now
                        .StatusDate = DateTime.Now
                        .UserID = 0
                        .VendorID = 0
                        .ExpirationDate = DateTime.Now
                        .StatusID = 0
                        .Save()
                    End With

                    With package_issued_2_package
                        .PackageID = 0
                        .PackageIssuedID = package_issued.PackageIssuedID
                        .UserID = 0
                        .DateAdded = DateTime.Now
                        .DateRemoved = Nothing
                        .Save()
                    End With

                    With package_issued_2_room
                        .ID = 0
                        .Load()
                        .PkgIss2PkgID = package_issued_2_package.ID
                        .RoomID = 0
                        .Save()
                    End With

                    With tour
                        .TourID = Prospect.Tour_TourID
                        .Load()
                        .ProspectID = Prospect.Prospect_ProspectID
                        .TourDate = Prospect.Tour_Date
                        .CampaignID = Prospect.Tour_CampaignID
                        .TypeID = Prospect.Tour_TypeID
                        .SubTypeID = Prospect.Tour_SubTypeID
                        .SourceID = Prospect.Tour_SourceID
                        .StatusDate = DateTime.Now
                        .StatusID = Prospect.Tour_StatusID
                        .TourTime = Prospect.Tour_TourTime
                        .TourLocationID = Prospect.Tour_TourLocationID
                        .BookingDate = DateTime.Now
                        .PackageIssuedID = package_issued.PackageIssuedID
                        .ReservationID = reservation.ReservationID
                        .Save()
                    End With

                    With reservation
                        .ReservationID = Prospect.Reservation_ReservationID
                        .Load()
                        .ProspectID = Prospect.Prospect_ProspectID
                        .ReservationNumber = Prospect.Reservation_ReservationNumber
                        .ResLocationID = Prospect.Reservation_Reservation_LocationID
                        .CheckInDate = Prospect.Reservation_CheckInDate
                        .CheckOutDate = Prospect.Reservation_CheckOutDate

                        .DateBooked = Prospect.Reservation_DateBooked
                        .StatusDate = DateTime.Now
                        .LockInventory = Prospect.Reservation_LockInventory

                        .SourceID = Prospect.Reservation_SourceID
                        .NumberAdults = Prospect.Reservation_NumberAdults
                        .NumberChildren = Prospect.Reservation_NumberChildren
                        .TypeID = Prospect.Reservation_TypeID
                        .SubTypeID = Prospect.Reservation_SubTypeID
                        .ResortCompanyID = Prospect.Reservation_ResortCompanyID
                        .StatusID = Prospect.Reservation_StatusID
                        .PackageIssuedID = package_issued.PackageIssuedID
                        .TourID = tour.TourID
                        .Save()
                    End With

                    cn.Open()
                    cm.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try

            End Using
        End Using
    End Sub
    <Serializable>
    Class WzProspect
        Public Property Prospect_ProspectID As Int32
        Public Property Prospect_FirstName As String
        Public Property Prospect_LasttName As String
        Public Property Prospect_SpouseFirstName As String
        Public Property Prospect_SpouseLastName As String
        Public ReadOnly Property Prospect_FullName As String
            Get
                Return String.Format("{0} {1}", Prospect_FirstName, Prospect_LasttName)
            End Get
        End Property

        Public Property Prospect_MaritalStatusID As Int32
        Public Property Reservation_ReservationID As Int32
        Public Property Reservation_ReservationNumber As String
        Public Property Reservation_CheckInDate As String
        Public Property Reservation_CheckOutDate As String
        Public Property Reservation_DateBooked As String
        Public Property Reservation_TotalNights As Int32
        Public Property Reservation_LockInventory As Boolean
        Public Property Reservation_StatusDate As String
        Public Property Reservation_StatusID As Int32
        Public Property Reservation_TypeID As Int32
        Public Property Reservation_SubTypeID As Int32
        Public Property Reservation_SourceID As Int32
        Public Property Reservation_NumberAdults As Int32
        Public Property Reservation_NumberChildren As Int32
        Public Property Reservation_ResortCompanyID As Int32
        Public Property Reservation_Reservation_LocationID As Int32
        Public Property Tour_TourID As Int32
        Public Property Tour_StatusID As Int32
        Public Property Tour_CampaignID As Int32
        Public Property Tour_TypeID As Int32
        Public Property Tour_SourceID As Int32
        Public Property Tour_TourTime As Int32
        Public Property Tour_TourLocationID As Int32
        Public Property Tour_SubTypeID As Int32
        Public Property Tour_BookingDate As String
        Public Property Tour_Date As String

        Public Property Prospect_Email As WzEmail
        Public Property Prospect_Address As WzAddress
        Public Property Prospect_Phone As WzPhone
        Public Property Prospect_Premium As WzPremium
        Public Property Prospect_Addresses As List(Of WzAddress)
        Public Property Prospect_Emails As List(Of WzEmail)
        Public Property Prospect_Phones As List(Of WzPhone)
        Public Property Prospect_Premiums As List(Of WzPremium)
        Public Property Prospect_Packages As List(Of WzPackage)
        Public Property Prospect_Hotel As WzHotel
        Public ReadOnly Property Package_Is_Resort_Stayed As Boolean
            Get
                If Prospect_Packages.Count = 0 Then
                    Throw New Exception("Packages are empty!")
                End If
                With Prospect_Packages.First
                    If String.IsNullOrEmpty(.Bedrooms) And .AccomRoomTypeID > 0 And .AccomID > 0 Then
                        Return True
                    ElseIf .Bedrooms.Length > 0 And .AccomRoomTypeID = 0 And .AccomID > 0 Then
                        Return False
                    Else
                        Throw New Exception("Not enough details to make decisions!")
                    End If
                End With
            End Get
        End Property
        Public ReadOnly Property Package_Has_Tour As Boolean
            Get
                If Prospect_Packages.Count = 0 Then
                    Throw New Exception("Packages are empty!")
                End If
                If Prospect_Packages.Where(Function(x) New String() {"Tradeshow", "Tour Package", "Tour Promotion"}.Contains(x.PackageType)).Count > 0 Then
                    Return True
                End If
                If Prospect_Packages.Where(Function(x) New String() {"Rental", "Owner Getaway", "Owner Advantage"}.Contains(x.PackageType)).Count > 0 Then
                    Return False
                Else
                    Throw New Exception("Not enough details to make decisions!")
                End If
            End Get
        End Property
    End Class
    <Serializable>
    Class WzAddress
        Public Property Address_SID As String
        Public Property Address_AddressID As Int32
        Public Property Address_TypeID As Int32
        Public Property Address_CountryID As Int32
        Public Property Address_StateID As Int32
        Public ReadOnly Property Address_State As String
            Get
                Return Utility.Get_ComboItem_String(Address_StateID)
            End Get
        End Property
        Public Property Address_ActiveFlag As Boolean
        Public Property Address_ContractAddress As Boolean
        Public Property Address_Address1 As String
        Public Property Address_Address2 As String
        Public Property Address_City As String
        Public Property Address_PostalCode As String
        Public Property Address_Region As String

    End Class
    <Serializable>
    Class WzEmail
        Public Property Email_SID As String
        Public Property Email_EmailID As Int32
        Public Property Email_Email As String
        Public Property Email_IsPrimary As Boolean
        Public Property Email_IsActive As Boolean
    End Class
    <Serializable>
    Class WzPhone
        Public Property Phone_SID As String
        Public Property Phone_PhoneID As Int32
        Public Property Phone_Number As String
        Public Property Phone_Extension As String
        Public Property Phone_TypeID As Int32
        Public ReadOnly Property Phone_Type As String
            Get
                Return Utility.Get_ComboItem_String(Phone_TypeID)
            End Get
        End Property
        Public Property Phone_Active As Boolean
    End Class
    <Serializable>
    Class WzPremium
        Public Property Premium_SID As String
        Public Property Premium_PremiumIssuedID As Int32
        Public Property Premium_PremiumID As Int32
        Public Property Premium_PremiumName As String
        Public Property Premium_CertificateNumber As String
        Public Property Premium_CostEA As String
        Public Property Premium_QtyAssigned As Int32
        Public Property Premium_StatusID As Int32
        Public Property Premium_Optional As Boolean
        Public ReadOnly Property Premium_Status As String
            Get
                Return Utility.Get_ComboItem_String(Premium_StatusID)
            End Get
        End Property
    End Class
    <Serializable>
    Class WzFinTransCodeID
        Public Property FinTransCodeID As Int32
        Public Property Amount As Decimal
    End Class
    <Serializable>
    Class WzPaymentMethodID
        Public Property PaymentMethodID As Int32
        Public Property Amount As Decimal
    End Class
    <Serializable>
    Class WzPackage
        Public Property SID As String
        Public Property PackageID As Int32
        Public Property AccomID As Int32
        Public Property AccomRoomTypeID As Int32
        Public Property Description As String
        Public Property Package As String
        Public Property TypeID As Int32
        Public Property Bedrooms As String
        Public Property UnitTypeID As Int32
        Public Property InventoryTypeID As Int32
        Public ReadOnly Property InventoryType As String
            Get
                Return Utility.Get_ComboItem_String(InventoryTypeID)
            End Get
        End Property
        Public ReadOnly Property UnitType As String
            Get
                Return Utility.Get_ComboItem_String(UnitTypeID)
            End Get
        End Property
        Public ReadOnly Property PackageType As String
            Get
                Return Utility.Get_ComboItem_String(TypeID)
            End Get
        End Property
        Public Property RateCost As Decimal
        Public Property FinTransCodes As New List(Of WzFinTransCodeID)
        Public Property PaymentMethods As New List(Of WzPaymentMethodID)
        Public Property Rooms As New List(Of WzRoom)
    End Class
    <Serializable>
    Class WzRoom
        Public Property SID As String
        Public Property RoomID As Int32
        Public Property RoomNumber As String
        Public Property RoomType As String
        Public Property RoomSubType As String
        Public Property AllocationID As List(Of Int32)
    End Class
    <Serializable>
    Class WzHotel
        Public Property Hotel_ConfirmationNumber As String
        Public Property Hotel_CheckInDate As String
        Public Property Hotel_CheckOutDate As String
        Public Property Hotel_AccommodationID As Int32
        Public Property Hotel_ReservationLocationID As Int32
        Public Property Hotel_CheckInLocationID As Int32
        Public Property Hotel_GuestTypeID As Int32
        Public Property Hotel_RoomTypeID As Int32

    End Class
    Protected Sub CheckBox_ListPackagesForSales_Select_CheckedChanged(sender As Object, e As EventArgs)
        Dim package_id = GridView_ListPackagesForSales.DataKeys(CType(CType(sender, CheckBox).NamingContainer, GridViewRow).RowIndex).Value.ToString
        Dim qty = Convert.ToInt32(CType(CType(CType(sender, CheckBox).NamingContainer, GridViewRow).FindControl("DropDownList_ListPackagesForSales_Qty"), DropDownList).SelectedItem.Text)

        If CType(sender, CheckBox).Checked = False And Prospect.Prospect_Packages.Where(Function(x) x.PackageID = package_id).Count > 0 Then
            Prospect.Prospect_Packages.RemoveAll(Function(x) x.PackageID = package_id)
        Else
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("select prp.PaymentMethodID, prftc.FinTransCodeID, pr.TypeID 'InventoryTypeID', p.UnitTypeID, coalesce(p.Description, '') Description 
                    from t_Package p 
                    inner join t_PackageReservation pr on p.PackageID = pr.PackageID
                    left join t_PackageReservationFinTransCode prftc on prftc.PackageReservationID = pr.PackageReservationID
                    left join t_PackageReservationPayment prp on prp.PackageReservationFinTransID = prftc.PackageReservationFinTransCodeID
                    where p.Active = 1
                    and p.PackageID in ({0})", package_id), cn)
                    Try
                        cn.Open()
                        Dim dt As New DataTable
                        Dim pk = New WzPackage
                        dt.Load(cm.ExecuteReader())

                        For Each row As DataRow In dt.Rows
                            If row("FinTransCodeID").Equals(DBNull.Value) = False Then
                                pk.FinTransCodes.Add(New WzFinTransCodeID With {.FinTransCodeID = row("FinTransCodeID").ToString, .Amount = 0})
                            End If
                            If row("PaymentMethodID").Equals(DBNull.Value) = False Then
                                pk.PaymentMethods.Add(New WzPaymentMethodID With {.PaymentMethodID = row("PaymentMethodID").ToString, .Amount = 0})
                            End If
                            pk.UnitTypeID = row("UnitTypeID").ToString
                            pk.InventoryTypeID = row("InventoryTypeID").ToString
                            pk.Description = row("Description").ToString
                        Next
                        With Prospect
                            For i = 1 To qty
                                .Prospect_Packages.Add(New WzPackage With {
                                                    .PackageID = package_id,
                                                    .Description = pk.Description,
                                                    .FinTransCodes = pk.FinTransCodes,
                                                    .InventoryTypeID = pk.InventoryTypeID,
                                                    .PaymentMethods = pk.PaymentMethods,
                                                    .RateCost = pk.RateCost,
                                                    .Rooms = pk.Rooms,
                                                    .UnitTypeID = pk.UnitTypeID,
                                                    .SID = Guid.NewGuid.ToString
                                                })
                            Next
                        End With
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End Using
            End Using
        End If

    End Sub
    Protected Sub GridView_ListPackagesForSales_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView_ListPackagesForSales.RowDataBound
        Dim gv = CType(sender, GridView)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row_vw = CType(e.Row.DataItem, DataRowView)

            With CType(e.Row.FindControl("DropDownList_ListPackagesForSales_Qty"), DropDownList)
                .Items.Clear()
                .DataSource = IIf(row_vw("PackageType").ToString = "Rental", Enumerable.Range(1, Convert.ToInt32(row_vw("RoomsAvail").ToString)), Enumerable.Range(1, 1))
                .DataBind()
            End With
        End If
    End Sub
    Protected Sub DropDownList_ListPackagesForSales_Qty_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim chk = CType(CType(CType(sender, DropDownList).NamingContainer, GridViewRow).FindControl("CheckBox_ListPackagesForSales_Select"), CheckBox).Checked
        Dim qty = Convert.ToInt32(CType(sender, DropDownList).SelectedItem.Text)
        Dim package_id = GridView_ListPackagesForSales.DataKeys(CType(CType(sender, DropDownList).NamingContainer, GridViewRow).RowIndex).Value.ToString
        If chk Then
            With Prospect.Prospect_Packages
                If .Where(Function(x) x.PackageID = package_id).Count > qty Then
                    For i = .Where(Function(x) x.PackageID = package_id).Count To qty + 1 Step -1
                        .Where(Function(x) x.PackageID = package_id).ToList().RemoveAt(i - 1)
                    Next
                ElseIf .Where(Function(x) x.PackageID = package_id).Count < qty Then
                    For i = .Where(Function(x) x.PackageID = package_id).Count To qty - 1
                        Dim pk = .Where(Function(x) x.PackageID = package_id).First
                        .Add(New WzPackage With {.PackageID = pk.PackageID,
                             .Description = pk.Description,
                             .FinTransCodes = pk.FinTransCodes,
                             .InventoryTypeID = pk.InventoryTypeID,
                             .PaymentMethods = pk.PaymentMethods,
                             .RateCost = pk.RateCost,
                             .Rooms = pk.Rooms,
                             .UnitTypeID = pk.UnitTypeID,
                             .SID = Guid.NewGuid.ToString
                             })
                    Next
                End If
            End With
        End If

    End Sub
    Protected Sub Button_SearchProspect_Search_Click(sender As Object, e As EventArgs) Handles Button_SearchProspect_Search.Click

        With Gridview_SearchProspect_Result
            .DataSource = Nothing
            .DataBind()
            .DataSource = Utility.Search_Text(TextBox_SearchProspect_Text.Text, DropDownList_SearchProspect_Options.SelectedItem.Text, 0)
            .DataBind()
        End With
    End Sub
    Protected Sub Gridview_SearchProspect_Result_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gridview_SearchProspect_Result.SelectedIndexChanged
        Dim exit_func_early = False, is_tour_req = False, stop_advancing = False, sql = String.Empty

        With New clsDoNotSellList
            .ID = .Get_DNS_Entry(Prospect.Prospect_ProspectID)
            .Load()
            If .ID > 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), Guid.NewGuid().ToString(), String.Format("alert('Stop, prospect is on the Do Not Sell list.');"), True)
                Return
            End If
        End With

        'per work order 55542, stop moving forward if last toured within 1 year
        'this step runs if the CheckIn date comes from the search box
        'it will not if search page has not been reached, just displays the last tour ID and date if available via js Confirm()
        'and let user continue
        'If Array.Exists(New Int32() {0, 3, 4, 5, 6}, Function(j) j = StartupOptionID) Then
        '    sql = IIf(CheckIn.HasValue, String.Format("select top 1 t.tourid from t_Tour t inner join t_ComboItems ts on t.StatusID = ts.ComboItemID  inner join t_Reservations r on r.TourID = t.TourID where ts.ComboItem in ('showed') and t.TourDate >  DATEADD(yy, -1, DATEADD(d, -1, '{0}')) and t.ProspectID = {1} order by t.TourID desc;", DateTime.Parse(Prospect.Reservation_CheckInDate).ToShortDateString(), Prospect.Prospect_ProspectID),
        '        String.Format("select top 1 t.tourid, t.TourDate from t_Tour t inner join t_ComboItems ts on t.StatusID = ts.ComboItemID  inner join t_Reservations r on r.TourID = t.TourID where ts.ComboItem in ('showed') and t.ProspectID = {0} order by t.TourID desc;", Prospect.Prospect_ProspectID))

        '    If is_tour_req Then
        '        'per work order , stops from moving forward if one last toured within 1 year
        '        Using cn As New SqlConnection(Resources.Resource.cns)
        '            Using cm = New SqlCommand(String.Format("select top 1 t.tourid, t.TourDate from t_Tour t inner join t_ComboItems ts on t.StatusID = ts.ComboItemID  where ts.ComboItem in ('showed') and t.TourDate >  DATEADD(yy, -1, DATEADD(d, -1, '{0}')) and t.ProspectID = {1} order by t.TourID desc;", DateTime.Parse(Prospect.Reservation_CheckInDate).ToShortDateString(), Prospect.Prospect_ProspectID), cn)
        '                Try
        '                    cn.Open()
        '                    Dim dt = New DataTable
        '                    dt.Load(cm.ExecuteReader())
        '                    If dt.Rows.Count = 1 Then
        '                        stop_advancing = True
        '                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Alert", String.Format("alert('Stop, this prospect (tour ID#{0}) last toured on {1} would still be less than a year from {2}.');", dt.Rows(0)("TourID").ToString(), DateTime.Parse(dt.Rows(0)("TourDate").ToString()).ToShortDateString(), DateTime.Parse(Prospect.Reservation_CheckInDate).ToShortDateString()), True)
        '                        Return
        '                    End If
        '                Catch ex As Exception
        '                    cn.Close()
        '                    Throw ex
        '                End Try
        '            End Using
        '        End Using
        '    End If
        'Else
        '    Using cn As New SqlConnection(Resources.Resource.cns)
        '        Using cm = New SqlCommand(String.Format("select top 1 t.tourid, t.TourDate from t_Tour t inner join t_ComboItems ts on t.StatusID = ts.ComboItemID  where ts.ComboItem in ('showed') and t.ProspectID = {0} order by t.TourID desc;", Prospect.Prospect_ProspectID), cn)
        '            Try
        '                cn.Open()
        '                Dim dt = New DataTable
        '                dt.Load(cm.ExecuteReader())
        '                If dt.Rows.Count = 1 Then

        '                    HiddenFieldConfirm.Value = String.Format("{0},{1}", dt.Rows(0)(0).ToString(), DateTime.Parse(dt.Rows(0)(1).ToString()).ToShortDateString())
        '                    exit_func_early = True
        '                End If
        '            Catch ex As Exception
        '                cn.Close()
        '                Throw ex
        '            End Try
        '        End Using
        '    End Using
        'End If
    End Sub
    Protected Sub Button_AssignRooms_Result_Select_Click(sender As Object, e As EventArgs)
        Dim p = New WzPackage
        Dim dt2 = New DataTable
        Dim gvr = CType(CType(sender, Button).NamingContainer, GridViewRow)
        Dim pkg = Prospect.Prospect_Packages.Where(Function(x) x.SID = gvr.Attributes("Package_SID")).First

        If CType(sender, Button).Text = "Remove" Then
            pkg.Rooms.Clear()
            With GridView_AssignRooms_Result
                .DataSource = Prospect.Prospect_Packages
                .DataBind()
            End With
        Else
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("select * from ufn_RoomsAvailable ('4', 17264, '11/1/2019', '11/3/2019', 17623, (select ComboItem from t_ComboItems where ComboItemID = 17264),(select ComboItem from t_ComboItems where ComboItemID = 17623), 0) where AVAILABLE = 'available'"), cn)
                    Try
                        cn.Open()
                        Dim dt = New DataTable
                        dt2.Columns.AddRange(New DataColumn() {New DataColumn("Package_SID"), New DataColumn("Package"), New DataColumn("RoomID"), New DataColumn("RoomNumber"), New DataColumn("RoomType"), New DataColumn("RoomSubType")})
                        dt.Load(cm.ExecuteReader())
                        For Each row As DataRow In dt.Rows
                            Dim newRow = dt2.NewRow

                            newRow("Package_SID") = pkg.SID
                            newRow("Package") = pkg.Description
                            newRow("RoomID") = row("RoomID").ToString
                            newRow("RoomNumber") = row("RoomNumber").ToString
                            newRow("RoomType") = row("RoomType1").ToString
                            newRow("RoomSubType") = row("RoomSubType1").ToString

                            If row("RoomID2").Equals(DBNull.Value) = False Then
                                newRow("RoomID") = String.Format("{0},{1}", row("RoomID").ToString, row("RoomID2").ToString)
                                newRow("RoomNumber") = String.Format("{0},{1}", row("RoomNumber").ToString, row("RoomNumber2").ToString)
                                newRow("RoomType") = String.Format("{0},{1}", row("RoomType1").ToString, row("RoomType2").ToString)
                                newRow("RoomSubType") = String.Format("{0},{1}", row("RoomSubType1").ToString, row("RoomSubType2").ToString)
                            End If
                            If row("RoomID3").Equals(DBNull.Value) = False Then
                                newRow("RoomID") = String.Format("{0},{1},{2}", row("RoomID").ToString, row("RoomID2").ToString, row("RoomID3").ToString)
                                newRow("RoomNumber") = String.Format("{0},{1},{2}", row("RoomNumber").ToString, row("RoomNumber2").ToString, row("RoomNumber3").ToString)
                                newRow("RoomType") = String.Format("{0},{1},{2}", row("RoomType1").ToString, row("RoomType2").ToString, row("RoomType3").ToString)
                                newRow("RoomSubType") = String.Format("{0},{1},{2}", row("RoomSubType1").ToString, row("RoomSubType2").ToString, row("RoomSubType3").ToString)
                            End If

                            dt2.Rows.Add(newRow)
                        Next
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    End Try
                End Using
            End Using

            With GridView_AssignRooms_Select
                .DataSource = dt2
                .DataBind()
            End With

            MultiView_AssignRooms.SetActiveView(View_AssignRooms_Select)
        End If
    End Sub
    Private Sub AssignRooms_Panel_Load()

        If Prospect.Prospect_Packages.Count > 0 Then
            With GridView_AssignRooms_Result
                .DataSource = Prospect.Prospect_Packages
                .DataBind()
            End With
        End If
    End Sub
    Private Sub ListPendingReservations_Panel_Load()

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand()
                Try
                    Dim sq = String.Format("select distinct r.ReservationID [Reservation ID], r.ProspectID, g.PackageID, pi.PackageIssuedID, g.MinNights 'Nights', rs.ComboItem [Status], g.Description [Package Description], convert(varchar, r.CheckInDate, 101) [Check-In],  
                            convert(varchar, r.CheckOutDate, 101) [Check-Out], p.FirstName + ' ' + p.LastName [Prospect], a.AccomName [Accommodation] 
                            from t_Reservations r inner join t_Prospect p on r.ProspectID = p.ProspectID 
                            left join t_Tour t on t.tourid = r.tourid 
                            inner join t_ComboItems rs on r.StatusID = rs.ComboItemID 
                            inner join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID 
                            inner join t_Package g on g.PackageID = pi.PackageID 
                            inner join t_ComboItems pt on pt.ComboItemID = g.TypeID 
                            inner join t_Accom a on a.AccomID = g.AccomID 
                            where rs.ComboItem in ('OpenEnded') and pt.ComboItem in ('Tradeshow', 'Tour Package') 
                            and r.CheckInDate is null 
                            and r.CheckOutDate is null 
                            and DATALENGTH(g.Bedrooms) > 0 
                            and a.AccomName in ('KCP')
                            and p.ProspectID =  {0}", 7348951)

                    Dim dt = New DataTable
                    With cm
                        cn.Open()
                        .CommandText = sq
                        .Connection = cn
                        dt.Load(.ExecuteReader())
                    End With

                    With GridView_ListPendngReservations
                        If dt.Rows.Count = 0 Then
                            .EmptyDataText = String.Format("{0} has no pending reservation", 0)
                        Else
                            .DataSource = dt
                        End If

                        .DataBind()
                    End With


                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Using
    End Sub
    Protected Sub GridView_AssignRooms_Result_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView_AssignRooms_Result.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row = CType(e.Row.DataItem, WzPackage)

            If String.IsNullOrEmpty(row.Description) = False Then
                With CType(e.Row.FindControl("Label_AssignRooms_Result_Package"), Label)
                    .Text = row.Description.ToUpper()
                End With
            End If

            If row.Rooms IsNot Nothing Then
                With CType(e.Row.FindControl("Label_AssignRooms_Result_Room"), Label)
                    .Text = String.Join(", ", row.Rooms.Select(Function(x) x.RoomNumber).ToArray())
                End With

                With CType(e.Row.FindControl("Label_AssignRooms_Result_Size"), Label)
                    .Text = String.Join(", ", row.Rooms.Select(Function(x) x.RoomType).ToArray())
                End With
            End If

            CType(e.Row.FindControl("Button_AssignRooms_Result_Select"), Button).Text = IIf(CType(e.Row.FindControl("Label_AssignRooms_Result_Room"), Label).Text.Length > 0, "Remove", "Select")
            e.Row.Attributes.Add("Package_SID", row.SID)
        End If
    End Sub
    Protected Sub Button_AssignRooms_Select_Cancel_Click(sender As Object, e As EventArgs) Handles Button_AssignRooms_Select_Cancel.Click
        MultiView_AssignRooms.SetActiveView(View_AssignRooms_Result)
    End Sub
    'Select Button in gridview's row
    Protected Sub Button_AssignRooms_Select_Click(sender As Object, e As EventArgs)
        Dim gvr = CType(CType(sender, Button).NamingContainer, GridViewRow)
        Dim pkg = Prospect.Prospect_Packages.Where(Function(x) x.SID = gvr.Attributes("Package_SID")).Single
        pkg.Rooms.Clear()
        Dim l = New List(Of WzRoom)

        For i As Int32 = 0 To gvr.Attributes("RoomID").Split(",").Count() - 1
            l.Add(New WzRoom With {.RoomID = gvr.Attributes("RoomID").Split(",")(i), .RoomNumber = gvr.Attributes("RoomNumber").Split(",")(i), .RoomType = gvr.Attributes("RoomType").Split(",")(i), .RoomSubType = gvr.Attributes("RoomSubType").Split(",")(i)})
        Next
        Prospect.Reservation_CheckInDate = "11/1/2019"
        Prospect.Reservation_CheckOutDate = "11/4/2019"
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand
                Try
                    cn.Open()
                    cm.Connection = cn
                    cm.CommandText = String.Format("select AllocationID, RoomID from t_RoomAllocationMatrix where RoomID in ({0}) and DateAllocated between '{1}' and '{2}' and ReservationID < 1", String.Join(",", l.Select(Function(x) x.RoomID).ToArray()), DateTime.Parse(Prospect.Reservation_CheckInDate).ToShortDateString(), DateTime.Parse(Prospect.Reservation_CheckOutDate).AddDays(-1).ToShortDateString())
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())
                    For Each row As DataRow In dt.Rows
                        For Each r As WzRoom In l
                            If r.AllocationID Is Nothing Then r.AllocationID = New List(Of Integer)

                            If Int32.Parse(row("RoomID").ToString) = r.RoomID Then
                                r.AllocationID.Add(row("AllocationID").ToString)
                            End If
                        Next
                    Next
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Using
        pkg.Rooms = l
        With GridView_AssignRooms_Result
            .DataSource = Prospect.Prospect_Packages
            .DataBind()
        End With
        MultiView_AssignRooms.SetActiveView(View_AssignRooms_Result)
    End Sub
    Protected Sub GridView_AssignRooms_Select_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView_AssignRooms_Select.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row = CType(e.Row.DataItem, DataRowView)

            e.Row.Attributes.Add("Package_SID", row("Package_SID"))
            e.Row.Attributes.Add("RoomID", row("RoomID"))
            e.Row.Attributes.Add("RoomNumber", row("RoomNumber"))
            e.Row.Attributes.Add("RoomType", row("RoomType"))
            e.Row.Attributes.Add("RoomSubType", row("RoomSubType"))
        End If
    End Sub
    Protected Sub Button_Premium_Edit_Select_Click(sender As Object, e As EventArgs)
        Dim gvr = CType(CType(sender, Button).NamingContainer, GridViewRow)
        Dim sid = GridView_ListPremiums.DataKeys(CType(CType(sender, Button).NamingContainer, GridViewRow).RowIndex).Value.ToString
        Dim pre = Prospect.Prospect_Premiums.Where(Function(x) x.Premium_SID = sid).Single

        Button_Edit_PremiumIssued_Submit.Attributes.Add("Premium_SID", sid)

        With DropDownListList_Premiums
            If .Items.Count = 0 Then
                .Items.Clear()
                .DataTextField = "PremiumName"
                .DataValueField = "PremiumID"
                .DataSource = New clsPremium().List_Active()
                .DataBind()
            End If
            .ClearSelection()

            If .Items.FindByValue(pre.Premium_PremiumID) IsNot Nothing Then
                .Items.FindByValue(pre.Premium_PremiumID).Selected = True
            End If
        End With
        With DropDownList_Premium_StatusID
            .ClearSelection()

            If .Items.FindByValue(pre.Premium_StatusID) IsNot Nothing Then
                .Items.FindByValue(pre.Premium_StatusID).Selected = True
            End If
        End With

        With DropDownList_Premium_QtyAssigned
            If .Items.Count = 0 Then
                .DataSource = Enumerable.Range(1, 12)
                .DataBind()
            End If
            .ClearSelection()
            If .Items.FindByValue(pre.Premium_QtyAssigned) IsNot Nothing Then
                .Items.FindByValue(pre.Premium_QtyAssigned).Selected = True
            End If
        End With

        TextBox_Premium_Cert.Text = pre.Premium_CertificateNumber
        TextBox_Premium_CostEA.Text = pre.Premium_CostEA

        MultiView_Tour.SetActiveView(View_Premium_Edit)

    End Sub
    Protected Sub GridView_ListPendngReservations_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView_ListPendngReservations.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row = CType(e.Row.DataItem, DataRowView)
            e.Row.Attributes.Add("ReservationID", row("Reservation ID").ToString)
            e.Row.Attributes.Add("ProspectID", row("ProspectID").ToString)
            e.Row.Attributes.Add("PackageID", row("PackageID").ToString)
            e.Row.Attributes.Add("PackageIssuedID", row("PackageIssuedID").ToString)
        End If
    End Sub
    Protected Sub GridView_ListPendngReservations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_ListPendngReservations.SelectedIndexChanged
        'Dim chk = CType(CType(CType(sender, DropDownList).NamingContainer, GridViewRow).FindControl("CheckBox_ListPackagesForSales_Select"), CheckBox).Checked
        'Dim qty = Convert.ToInt32(CType(sender, DropDownList).SelectedItem.Text)
        'Dim package_id = GridView_ListPackagesForSales.DataKeys(CType(CType(sender, DropDownList).NamingContainer, GridViewRow).RowIndex).Value.ToString

        'Dim gvr = CType(CType(sender, Button).NamingContainer, GridViewRow)
        Dim gvr = GridView_ListPendngReservations.SelectedRow

        With Prospect
            .Reservation_ReservationID = gvr.Attributes("ReservationID")
            .Prospect_ProspectID = gvr.Attributes("ProspectID")
        End With
    End Sub

    Protected Sub LinkButton_SearchProspect_Select_Click(sender As Object, e As EventArgs)
        Dim prospect_id = CType(sender, LinkButton).Text

    End Sub
End Class
