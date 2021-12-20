<%@ Page Title="" Language="VB" MasterPageFile="~/Wizards/ReservationsBooking/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="crosscut.Wizards_ReservationsBooking_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head2" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.Package-Search-CheckBox').click(function (e) {
                var cb = $(this).find(':checkbox(:first)');                
                var rows = $('#<%= GridView_Search_Available_Packages_For_Purchasing.ClientID %>').find('tr:not(:first)');                
                var min = 0;
                var max = 0;
                
                if ($(cb).prop('checked')) {
                    var tx = $(cb).parents('tr:first').find('td:eq(5)').text();                    
                    if (tx == 'Owner Getaway' || tx == 'Tour Promotion' || tx == 'Tour Package' || tx == 'Tradeshow' || tx == 'Owner Advantage') {
                        // checked == true and if this instance is an Owner Getaway or Tour Promotion, or packages with a tour, 
                        // loop through them and disable them  
                        $.each(rows, function (index, row) {                            
                            if ($(row).find('td:eq(5)').text() == 'Owner Getaway' || $(row).find('td:eq(5)').text() == 'Tour Promotion' || $(row).find('td:eq(5)').text() == 'Tour Package' || $(row).find('td:eq(5)').text() == 'Tradeshow' || $(row).find('td:eq(5)').text() == 'Owner Advantage') {
                                $(row).find(':checkbox(:first)').prop('disabled', true);                               
                            }                           
                        });                        
                        $(cb).prop('disabled', false);
                    }
                    $.each(rows, function (index, row) {
                        // keeps running totals for min cost and max cost
                        var qty = $(row).find('select').val();
                        if ($(row).find(':checkbox(:first)').prop('checked') && $(row).find(':checkbox(:first)').prop('disabled') == false) {
                            min += $(row).find('td:eq(2)').text().length == 0 ? 0 : parseFloat($(row).find('td:eq(2)').text()) * parseInt(qty);
                            max += $(row).find('td:eq(3)').text().length == 0 ? 0 : parseFloat($(row).find('td:eq(3)').text()) * parseInt(qty);
                        }
                    });
                } else { // checked == false

                    var tx = $(cb).parents('tr:first').find('td:eq(5)').text();
                    if (tx == 'Owner Getaway' || tx == 'Tour Promotion' || tx == 'Tour Package' || tx == 'Tradeshow' || tx == 'Owner Advantage') {
                        $.each(rows, function (index, row) {
                            // un-check all the checkboxes, except Rental packages
                            var tx = $(row).find('td:eq(5)').text();
                            if (tx == 'Owner Getaway' || tx == 'Tour Promotion' || tx == 'Tour Package' || tx == 'Tradeshow' || tx == 'Owner Advantage') {
                                $(row).find(':checkbox(:first)').prop('disabled', false);                                                           
                            }
                        });
                    }
                    // keeps running totals for min cost and max cost
                    min = parseFloat($('#<%= TextBox_Search_Available_Packages_CostMin.ClientID %>').val());
                    max = parseFloat($('#<%= TextBox_Search_Available_Packages_CostMax.ClientID %>').val());

                    var qty = $(cb).parents('tr:first').find('select').val();
                    min -= parseFloat($(cb).parents('tr:first').find('td:eq(2)').text()) * parseInt(qty);
                    max -= parseFloat($(cb).parents('tr:first').find('td:eq(3)').text()) * parseInt(qty);;                    
                }
                // updates the totals in the textboxes
                $('#<%= TextBox_Search_Available_Packages_CostMin.ClientID %>').val(min);
                $('#<%= TextBox_Search_Available_Packages_CostMax.ClientID %>').val(max);
                $('#<%= TextBox_Search_Available_Packages_Price_Total.ClientID %>').val(max);
            });

            $('.Package-Rebooking-CheckBox').click(function (e) {
                var cb = $(this).find('input[type=radio]');                
                var rows = $('#<%= GridView_Search_Available_Packages_For_Rebooking.ClientID %>').find('tr:not(:first)');                
                $.each(rows, function (index, row) {
                    // uncheck all boxes
                    $(row).find('input[type=radio]').prop('checked', false);
                });
                $(cb).prop('checked', true);
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <h1>Welcome!</h1>
    <asp:Label ID="Label23" runat="server" Text="<%# Prospect.FirstName %>"></asp:Label>


    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
    <asp:HiddenField ID="HiddenField_LogonVendorID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPersonnelID" runat="server" />
    <asp:HiddenField ID="HiddenFieldProspectID" runat="server" />
    <asp:HiddenField ID="HiddenFieldReservationID" runat="server" />
    <asp:HiddenField ID="HiddenFieldCurrentEditedEmailID" runat="server" />
    <asp:HiddenField ID="HiddenFieldCurrentEditedPhoneID" runat="server" />
    <asp:HiddenField ID="HiddenFieldCurrentEditedAddressID" runat="server" />

    <asp:HiddenField ID="HiddenField_CurrentPanel" runat="server" />

    <asp:ObjectDataSource ID="ObjectDataSource_Reservation" runat="server" SelectMethod="ListReservations" TypeName="clsReservationWizard"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource_Prospect" runat="server" SelectMethod="GetProspect" TypeName="clsReservationWizard+ProspectDB">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="prospectID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ObjectDataSource_RoomsAvail" runat="server" SelectMethod="List_DDL_Rooms_Avail" TypeName="clsReservationWizard"></asp:ObjectDataSource>
   
    <asp:ObjectDataSource ID="ObjectDataSource_EmailSource" runat="server" TypeName="clsReservationWizard" SelectMethod="ListEmails"></asp:ObjectDataSource>


    <asp:Button ID="Button_Reset_Controls" runat="server" Text="Button" />

    <asp:Wizard ID="Wizard1" runat="server">
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Welcome">
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Search Available Packages For Purchasing">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep15" runat="server" Title="Search Available Packages Offered By The Vendor">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep3" runat="server" Title="Search For Prospect">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep14" runat="server" Title="List Pending Reservations">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep4" runat="server" Title="Edit Prospect">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep5" runat="server" Title="Edit Tour">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep6" runat="server" Title="Edit Reservation">
                <h2>I am still learning...</h2>
                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep9" runat="server" Title="Allocate Rooms">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep10" runat="server" Title="Re-Assign Rooms">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep11" runat="server" Title="Edit Hotel">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep7" runat="server" Title="Process Payment">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep8" runat="server" Title="Process Refund">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep12" runat="server" Title="Edit Notes">
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep13" runat="server" Title="Confirmation">
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <h1>Panel 1</h1>
        <asp:RadioButtonList ID="RadioButtonList_StartupOptions" runat="server">
        </asp:RadioButtonList>


        <asp:GridView ID="GridView_Search_Available_Packages_For_Rebooking" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="RadioButton1" runat="server" CssClass="Package-Rebooking-CheckBox" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PackageID" HeaderText="Package ID" SortExpression="PackageID" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                <FooterStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Visible="false">
        <h2>Panel 2</h2>

        <asp:Label ID="Label2" runat="server" Text="Check-IN"></asp:Label>
        <asp:TextBox ID="TextBox_Search_CheckIn" runat="server"></asp:TextBox>

        <asp:Label ID="Label3" runat="server" Text="Check-OUT"></asp:Label>
        <asp:TextBox ID="TextBox_Search_CheckOut" runat="server"></asp:TextBox>

        <asp:Label ID="Label4" runat="server" Text="Nights"></asp:Label>
        <asp:DropDownList ID="DropDownList_Search_Nights" runat="server">
        </asp:DropDownList>

        <asp:DropDownList ID="DropDownList_PackageTypes" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList_PackageSubTypes" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList_UnitTypes" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList_RoomSizes" runat="server">
        </asp:DropDownList>

        <asp:Button ID="Button_Search_Available_Packages_For_Purchasing" runat="server" Text="Search..." />
         <asp:Label ID="Label63" runat="server" Text="Minimum Price"></asp:Label>
        <asp:TextBox ID="TextBox_Search_Available_Packages_CostMin" runat="server" ReadOnly="True"></asp:TextBox>
         <asp:Label ID="Label64" runat="server" Text="Maximum Price"></asp:Label>
        <asp:TextBox ID="TextBox_Search_Available_Packages_CostMax" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="Label62" runat="server" Text="Total Price"></asp:Label>
        <asp:TextBox ID="TextBox_Search_Available_Packages_Price_Total" runat="server"></asp:TextBox>


        <asp:GridView ID="GridView_Search_Available_Packages_For_Purchasing" 
            runat="server" 
            AutoGenerateColumns="False" EmptyDataText="No data">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox7" runat="server" CssClass="Package-Search-CheckBox" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="Package" SortExpression="Description" />
                <asp:BoundField DataField="MinCost" HeaderText="Mnimum Cost" SortExpression="MinCost" />
                <asp:BoundField DataField="MaxCost" HeaderText="Maximum Cost" SortExpression="MaxCost" />
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" Width="95%">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PackageType" HeaderText="Package Type" SortExpression="PackageType" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" BorderStyle="Dotted" Visible="false">
        <h3>Panel 3</h3>
        <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Visible="false">
        <asp:MultiView ID="MultiView_Prospect" runat="server" ActiveViewIndex="0">
            <asp:View ID="View_Prospect_Main" runat="server">
                <h1>Edit Prospect</h1>

                <asp:Label ID="Label5" runat="server" Text="Prospect ID"></asp:Label>
                <asp:TextBox ID="TextBox_Prospect_ProspectID" runat="server" Text="<%# Prospect.ProspectID %>"></asp:TextBox>

                <asp:Label ID="Label6" runat="server" Text="First Name"></asp:Label>
                <asp:TextBox ID="TextBox_Prospect_FirstName" runat="server" Text="<%# Prospect.FirstName %>"></asp:TextBox>

                <asp:Label ID="Label7" runat="server" Text="Last Name"></asp:Label>
                <asp:TextBox ID="TextBox_Prospect_LastName" runat="server" Text="<%# Prospect.LastName %>"></asp:TextBox>

                <asp:Label ID="Label8" runat="server" Text="Spouse First Name"></asp:Label>
                <asp:TextBox ID="TextBox_Spouse_FirstName" runat="server" Text="<%# Prospect.Spouse.FirstName %>"></asp:TextBox>

                <asp:Label ID="Label9" runat="server" Text="Spouse Last Name"></asp:Label>
                <asp:TextBox ID="TextBox_SpouseLastName" runat="server" Text="<%# Prospect.Spouse.LastName %>"></asp:TextBox>

                <asp:Label ID="Label10" runat="server" Text="Marital Status"></asp:Label>
                <asp:DropDownList ID="DropDownList_MaritalStatusID" runat="server"></asp:DropDownList>

                <asp:GridView ID="GridView_Emails" runat="server" DataKeyNames="SID" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton_Email_Add" runat="server" OnClick="LinkButton_Email_Add_Click">New Email</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Email_Edit" runat="server" OnClick="LinkButton_Email_Edit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Address" HeaderText="Email" SortExpression="Address" />
                        <asp:TemplateField HeaderText="Primary">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox4" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsPrimary") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox5" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsActive") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="GridView_Phones" runat="server" AutoGenerateColumns="False" DataKeyNames="SID">
                    <Columns>
                        <asp:TemplateField>

                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton_Phone_Add" runat="server" OnClick="LinkButton_Phone_Add_Click">New Phone</asp:LinkButton>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Phone_Edit" runat="server" OnClick="LinkButton_Phone_Edit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Number" HeaderText="Number" SortExpression="Number" />
                        <asp:BoundField DataField="Extension" HeaderText="Extension" SortExpression="Extension" />
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox6" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Active") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="GridView_Addresses" runat="server" DataKeyNames="SID" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton_Address_Add" runat="server" OnClick="LinkButton_Address_Add_Click">New Address</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Address_Edit" runat="server" OnClick="LinkButton_Address_Edit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Address1" HeaderText="Address 1" />
                        <asp:BoundField DataField="Address2" HeaderText="Address 2" />
                        <asp:BoundField DataField="City" HeaderText="City" />
                        <asp:BoundField DataField="PostalCode" HeaderText="Postal Code" />
                        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                        <asp:CheckBoxField DataField="ActiveFlag" HeaderText="Active" ReadOnly="True" SortExpression="Active" />
                        <asp:CheckBoxField DataField="ContractAddress" HeaderText="Contract Address" SortExpression="ContractAddress" />
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View ID="View_Prospect_Edit_Email" runat="server">

                <asp:Label ID="Label17" runat="server" Text="Edit Edit"></asp:Label>
                <asp:Label ID="Label_Edit_Email" runat="server" Text="<%# Prospect.FullName %>"></asp:Label>

                <asp:Label ID="Label1" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="TextBox_Email_Address" runat="server" Text="<%# Email.Address %>"></asp:TextBox>

                <asp:Label ID="Label14" runat="server" Text="Is Primary"></asp:Label>
                <asp:CheckBox ID="CheckBox_Email_IsPrimary" runat="server" Checked="<%# Email.IsPrimary %>" />

                <asp:Label ID="Label15" runat="server" Text="Is Active"></asp:Label>
                <asp:CheckBox ID="CheckBox_Email_IsActive" runat="server" Checked="<%# Email.IsActive %>" />

                <asp:Button ID="Button_Edit_Email_Submit" runat="server" Text="Submit" />
                <asp:Button ID="Button_Edit_Email_Cancel" runat="server" Text="Cancel" />
            </asp:View>
            <asp:View ID="View_Prospect_Edit_Phone" runat="server">
                <asp:Label ID="Label16" runat="server" Text="Edit Phone"></asp:Label>
                <asp:Label ID="Label_Edit_Phone" runat="server" Text="<%# Prospect.FullName %>"></asp:Label>


                <asp:Label ID="Label19" runat="server" Text="Number"></asp:Label>
                <asp:TextBox ID="TextBox_Phone_Number" Text="<%# Phone.Number %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label20" runat="server" Text="Extension"></asp:Label>
                <asp:TextBox ID="TextBox_Phone_Extension" Text="<%# Phone.Extension.Trim %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label21" runat="server" Text="Type"></asp:Label>
                <asp:DropDownList ID="DropDownList_Phone_TypeID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label22" runat="server" Text="Active"></asp:Label>
                <asp:CheckBox ID="CheckBox_Phone_Active" Checked="<%# Phone.Active %>" runat="server" />

                <asp:Button ID="Button_Edit_Phone_Cancel" runat="server" Text="Cancel" />
                <asp:Button ID="Button_Edit_Phone_Submit" runat="server" Text="Submit" />
            </asp:View>
            <asp:View ID="View_Prospect_Edit_Address" runat="server">
                <asp:Label ID="Label24" runat="server" Text="Edit Address"></asp:Label>
                <asp:Label ID="Label_Edit_Address" runat="server" Text="<%# Prospect.FullName %>"></asp:Label>
                <asp:Label ID="Label25" runat="server" Text="Address 1"></asp:Label>
                <asp:TextBox ID="TextBox_Address1" Text="<%# Address.Address1 %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label26" runat="server" Text="Address 2"></asp:Label>
                <asp:TextBox ID="TextBox_Address2" Text="<%# Address.Address2 %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label27" runat="server" Text="City"></asp:Label>
                <asp:TextBox ID="TextBox_City" Text="<%# Address.City %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label28" runat="server" Text="State"></asp:Label>
                <asp:DropDownList ID="DropDownList_StateID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label29" runat="server" Text="Postal Code"></asp:Label>
                <asp:TextBox ID="TextBox_PostalCode" Text="<%# Address.PostalCode %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label30" runat="server" Text="Region"></asp:Label>
                <asp:TextBox ID="TextBox_Regiion" Text="<%# Address.Region %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label31" runat="server" Text="Country"></asp:Label>
                <asp:DropDownList ID="DropDownList_CountryID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label32" runat="server" Text="Type"></asp:Label>
                <asp:DropDownList ID="DropDownList_Address_TypeID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label33" runat="server" Text="Active"></asp:Label>
                <asp:CheckBox ID="CheckBox_ActiveFlag" Checked="<%# Address.ActiveFlag %>" runat="server" />
                <asp:Label ID="Label34" runat="server" Text="Contract Address"></asp:Label>
                <asp:CheckBox ID="CheckBox_ContractAddress" Checked="<%# Address.ContractAddress %>" runat="server" />

                <asp:Button ID="Button_Edit_Address_Cancel" runat="server" Text="Cancel" />
                <asp:Button ID="Button_Edit_Address_Submit" runat="server" Text="Submit" />
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Visible="false">
        <h2>Panel 5</h2>
        <h1>Edit Reservation</h1>
        <asp:Label ID="Label13" runat="server" Text="Reservation ID"></asp:Label>
        <asp:TextBox ID="TextBox_ReservationID" runat="server" Text="<%# Reservation.ReservationID %>"></asp:TextBox>
        <asp:Label ID="Label18" runat="server" Text="Reservation Number"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_Number" runat="server" Text="<%# Reservation.ReservationNumber %>"></asp:TextBox>
        <asp:Label ID="Label35" runat="server" Text="Location"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_LocationID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label36" runat="server" Text="Check In"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_CheckIn" runat="server" Text="<%# Reservation.CheckIn.ToShortDateString() %>"></asp:TextBox>
        <asp:Label ID="Label37" runat="server" Text="Check Out"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_CheckOut" runat="server" Text="<%# Reservation.CheckOut.ToShortDateString() %>"></asp:TextBox>
        <asp:Label ID="Label38" runat="server" Text="Total Nights"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_Total_Nights" runat="server"></asp:DropDownList>
        <asp:Label ID="Label40" runat="server" Text="Date Booked"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_DateBooked" runat="server" Text="<%# Reservation.DateBooked.ToShortDateString() %>"></asp:TextBox>
        <asp:Label ID="Label39" runat="server" Text="Resoert Company"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_Resort_CompanyID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label41" runat="server" Text="Lock Inventory"></asp:Label>
        <asp:CheckBox ID="CheckBox_Reservation_LockInventory" Checked="<%# Reservation.LockInventory %>" runat="server" />
        <asp:Label ID="Label42" runat="server" Text="Status"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_StatusID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label48" runat="server" Text="Status Date"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_StatusDate" runat="server" Text="<%# Reservation.StatusDate.ToShortDateString() %>"></asp:TextBox>
        <asp:Label ID="Label43" runat="server" Text="Type"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_TypeID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label44" runat="server" Text="Sub Type"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_SubTypeID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label45" runat="server" Text="Adults"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_Adults" runat="server"></asp:DropDownList>
        <asp:Label ID="Label46" runat="server" Text="Children"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_Children" runat="server"></asp:DropDownList>
        <asp:Label ID="Label47" runat="server" Text="Source"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_SourceID" runat="server"></asp:DropDownList>




    </asp:Panel>
    <asp:Panel ID="Panel6" runat="server" Visible="false">
        <h2>Panel 6</h2>
        <asp:MultiView runat="server" ID="MultiView_Tour" ActiveViewIndex="0">
            <asp:View runat="server" ID="View_Tour">
                <asp:Label runat="server" Text="Tour ID"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_TourID" Text="<%# Tour.TourID %>"></asp:TextBox>

                <asp:Label ID="Label11" runat="server" Text="Tour Date"></asp:Label>
                <asp:TextBox runat="server" ID="TextBox_TourDate" CssClass="form-control" Text="<%# Tour.TourDate.ToShortDateString() %>"></asp:TextBox>

                <asp:Label ID="Label12" runat="server" Text="Tour Status"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourStatusID"></asp:DropDownList>

                <asp:Label ID="Label49" runat="server" Text="Campaign"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_CampaignID"></asp:DropDownList>

                <asp:Label ID="Label50" runat="server" Text="Tour Type"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourTypeID"></asp:DropDownList>

                <asp:Label ID="Label51" runat="server" Text="Tour Source"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourSourceID"></asp:DropDownList>

                <asp:Label ID="Label52" runat="server" Text="Tour Time"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourTime"></asp:DropDownList>

                <asp:Label ID="Label53" runat="server" Text="Tour Location"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourLocationID"></asp:DropDownList>

                <asp:Label ID="Label54" runat="server" Text="Tour Sub-Type"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourSubTypeID"></asp:DropDownList>

                <asp:Label ID="Label55" runat="server" Text="Booking Date"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_BookingDate" Text="<%# Tour.BookingDate.ToShortDateString() %>"></asp:TextBox>

                <asp:GridView runat="server" ID="GridView_ListPremiums" AutoGenerateColumns="False" AutoGenerateSelectButton="True">
                    <Columns>                        
                        <asp:BoundField DataField="PremiumName" HeaderText="Premium Name" SortExpression="PremiumName" />
                        <asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="QtyAssigned" HeaderText="Quantity" SortExpression="QtyAssigned" />
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View runat="server" ID="View_Premium_Edit">

                <asp:Label ID="Label56" runat="server" CssClass="control-label" Text="Premium Name"></asp:Label>
                <asp:DropDownList runat="server" ID="DropDownListList_Premiums"></asp:DropDownList>
                <asp:Label ID="Label57" runat="server" CssClass="control-label" Text="Certificate Number"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_Cert" Text="<%# PremiumIssued.CertificateNumber.Trim() %>" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label58" runat="server" CssClass="control-label" Text="Premium Cost"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_Cost" Text='<%# PremiumIssued.Premium_Cost.ToString("N2") %>' ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label59" runat="server" CssClass="control-label" Text="Chargeback Cost"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_CostEA" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label60" runat="server" CssClass="control-label" Text="Quantity"></asp:Label>
                <asp:DropDownList runat="server" ID="DropDownList_Quantities"></asp:DropDownList>
                <asp:Label ID="Label61" runat="server" CssClass="control-label" Text="Status"></asp:Label>
                <asp:DropDownList runat="server" ID="DropDownList_PremiumStatusID"></asp:DropDownList>

                <asp:Button runat="server" ID="Button_Edit_PremiumIssued_Cancel" Text="Cancel" />
                <asp:Button runat="server" ID="Button_Edit_PremiumIssued_Submit" Text="Submit" />
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <asp:Panel ID="Panel7" runat="server" Visible="false">

        
    </asp:Panel>
    <asp:Panel ID="Panel8" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel9" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel10" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel11" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel12" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel13" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel14" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel15" runat="server" Visible="false"></asp:Panel>



    <script type="text/javascript">
        $(function () {            

            $('#<%= TextBox_Search_CheckIn.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,

                onSelect: function () {
                    var days = $('#<%= DropDownList_Search_Nights.ClientID %>').val();
                    var date = new Date($(this).val());
                    $('#<%= TextBox_Search_CheckOut.ClientID %>').val(moment(
                        new Date(date.getFullYear(),
                            date.getMonth(),
                            parseInt(date.getDate()) + parseInt(days))).format('L'));
                }
            });
            //
            $('#<%= DropDownList_Search_Nights.ClientID %>').on('change', function (e) {
                if ($('#<%= TextBox_Search_CheckIn.ClientID %>').val().length > 0) {
                    var days = $('#<%= DropDownList_Search_Nights.ClientID %>').val();
                    var date = new Date($('#<%= TextBox_Search_CheckIn.ClientID %>').val());
                    $('#<%= TextBox_Search_CheckOut.ClientID %>').val(moment(new Date(date.getFullYear(), date.getMonth(), parseInt(date.getDate()) + parseInt(days))).format('L'));
                }
            });

            $('#<%= TextBox_Search_CheckOut.ClientID %>').on('click', function () {
                $('#<%= DropDownList_Search_Nights.ClientID %>').focus();
            });
        })
    </script>
</asp:Content>
