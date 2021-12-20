<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Wizards/ReservationsBooking/MasterPage.master" CodeBehind="Default.aspx.vb" Inherits="crosscut._Default1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h1>Welcome!</h1>
    <asp:Label ID="Label23" runat="server" Text="<%# Prospect.Prospect_FirstName  %>"></asp:Label>

    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
    <asp:HiddenField ID="HiddenField_LogonVendorID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPersonnelID" runat="server" />
    <asp:HiddenField ID="HiddenFieldProspectID" runat="server" />
    <asp:HiddenField ID="HiddenFieldReservationID" runat="server" />
    <asp:HiddenField ID="HiddenFieldCurrentEditedEmailID" runat="server" />
    <asp:HiddenField ID="HiddenFieldCurrentEditedPhoneID" runat="server" />
    <asp:HiddenField ID="HiddenFieldCurrentEditedAddressID" runat="server" />

    <asp:HiddenField ID="HiddenField_CurrentPanelID" runat="server" />
    <asp:HiddenField ID="HiddenField_ViewStateID" runat="server" />

    <asp:ObjectDataSource ID="ObjectDataSource_Reservation" runat="server" SelectMethod="ListReservations" TypeName="clsReservationWizard"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource_Prospect" runat="server" SelectMethod="GetProspect" TypeName="clsReservationWizard+ProspectDB">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="prospectID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:Button ID="Button_Reset_Controls" runat="server" Text="Button" />


    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0">
        <WizardSteps>
            <asp:WizardStep ID="Welcome_WizardStep" runat="server" Title="Welcome">
            </asp:WizardStep>
            <asp:WizardStep ID="ListPackagesForSales_WizardStep" runat="server" Title="List Packages For Sales">
            </asp:WizardStep>
            <asp:WizardStep ID="ListPackagesOfferedByVendor_WizardStep" runat="server" Title="List Packages Offered By Vendor">
            </asp:WizardStep>
            <asp:WizardStep ID="SearchProspect_WizardStep" runat="server" Title="Search Prospect">
            </asp:WizardStep>
            <asp:WizardStep ID="ListPendingReservations_WizardStep" runat="server" Title="List Pending Reservations">
            </asp:WizardStep>
            <asp:WizardStep ID="EditProspect_WizardStep" runat="server" Title="Edit Prospect">
            </asp:WizardStep>
            <asp:WizardStep ID="EditTour_WizardStep" runat="server" Title="Edit Tour">
            </asp:WizardStep>
            <asp:WizardStep ID="EditReservation_WizardStep" runat="server" Title="Edit Reservation">
            </asp:WizardStep>
            <asp:WizardStep ID="AssignRooms_WizardStep" runat="server" Title="Assign Rooms">
            </asp:WizardStep>
            <asp:WizardStep ID="ReAssignRooms_WizardStep" runat="server" Title="Re-Assign Rooms">
            </asp:WizardStep>
            <asp:WizardStep ID="EditHotel_WizardStep" runat="server" Title="Edit Hotel">
            </asp:WizardStep>
            <asp:WizardStep ID="ProcesPayment_WizardStep" runat="server" Title="Process Payment">
            </asp:WizardStep>
            <asp:WizardStep ID="ProcessRefund_WizardStep" runat="server" Title="Process Refund">
            </asp:WizardStep>
            <asp:WizardStep ID="EditNotes_WizardStep" runat="server" Title="Edit Notes">
            </asp:WizardStep>
            <asp:WizardStep ID="Confirmation_WizardStep" runat="server" Title="Confirmation">
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
    <asp:Panel ID="Welcome_Panel" runat="server">
        <h1>Panel 1</h1>
        <p>
            <asp:Label ID="Label66" runat="server" AssociatedControlID="RadioButton_Welcome_Option_1">
                <asp:RadioButton ID="RadioButton_Welcome_Option_1" GroupName="Well" runat="server" />
                <span>Purchase A Tour Promotion Or An Owner Getaway Or Rental Packages</span>
            </asp:Label>
        </p>
         <p>
            <asp:Label ID="Label67" runat="server" AssociatedControlID="RadioButton_Welcome_Option_2">
                <asp:RadioButton ID="RadioButton_Welcome_Option_2" GroupName="Well" runat="server" />
                <span>Book A Package Tour Or A Tradeshow Package</span>
            </asp:Label>
        </p>
         <p>
            <asp:Label ID="Label68" runat="server" AssociatedControlID="RadioButton_Welcome_Option_3">
                <asp:RadioButton ID="RadioButton_Welcome_Option_3" GroupName="Well" runat="server" />
                <span>Schedule A Booked, Reset Or Pending Package</span>
            </asp:Label>
        </p>
         <p>
            <asp:Label ID="Label69" runat="server" AssociatedControlID="RadioButton_Welcome_Option_4">
                <asp:RadioButton ID="RadioButton_Welcome_Option_4" GroupName="Well" runat="server" />
                <span>Purchase A 1-Night Or A 2-Nights Tour Promotion Package</span>
            </asp:Label>
        </p>
       <%-- <asp:RadioButtonList ID="RadioButtonList_Welcome_StartupOptions" runat="server">
            <asp:ListItem Selected="True">Purchase A Tour Promotion Or An Owner Getaway Or Rental Packages</asp:ListItem>
            <asp:ListItem>Book A Package Tour Or A Tradeshow Package</asp:ListItem>
            <asp:ListItem>Purchase A 1-Night Or A 2-Nights Tour Promotion Package</asp:ListItem>
            <asp:ListItem>Schedule A Booked, Reset Or Pending Package</asp:ListItem>
        </asp:RadioButtonList>--%>

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
    <asp:Panel ID="ListPackagesForSales_Panel" runat="server">
        <h2>Panel 2</h2>

        <asp:DropDownList ID="DropDownList_PackageTypes" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList_PackageSubTypes" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList_UnitTypes" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList_RoomSizes" runat="server">
        </asp:DropDownList>


        <hr />
        <asp:Label ID="Label2" runat="server" Text="Check-In"></asp:Label>
        <asp:TextBox ID="TextBox_Search_CheckIn" runat="server"></asp:TextBox>

        <asp:Label ID="Label3" runat="server" Text="Check-Out"></asp:Label>
        <asp:TextBox ID="TextBox_Search_CheckOut" runat="server"></asp:TextBox>

        <asp:Label ID="Label4" runat="server" Text="Nights"></asp:Label>
        <asp:DropDownList ID="DropDownList_Search_Nights" runat="server">
        </asp:DropDownList>


        <asp:Button ID="Button_Search_Available_Packages_For_Purchasing" runat="server" Text="Search..." />
        <asp:Label ID="Label63" runat="server" Text="Minimum Price"></asp:Label>
        <asp:TextBox ID="TextBox_Search_Available_Packages_CostMin" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="Label64" runat="server" Text="Maximum Price"></asp:Label>
        <asp:TextBox ID="TextBox_Search_Available_Packages_CostMax" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="Label62" runat="server" Text="Total Price"></asp:Label>
        <asp:TextBox ID="TextBox_Search_Available_Packages_Price_Total" runat="server"></asp:TextBox>

        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView_ListPackagesForSales"
                    runat="server"
                    AutoGenerateColumns="False" EmptyDataText="No data" DataKeyNames="PackageID">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox_ListPackagesForSales_Select" runat="server" CssClass="Package-Search-CheckBox" AutoPostBack="True" OnCheckedChanged="CheckBox_ListPackagesForSales_Select_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" HeaderText="Package" SortExpression="Description" />
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList_ListPackagesForSales_Qty" runat="server" Width="95%" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_ListPackagesForSales_Qty_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PackageType" HeaderText="Package Type" SortExpression="PackageType" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>
    <asp:Panel ID="ListPackagesOfferedByVendor_Panel" runat="server" BorderStyle="Dotted">
        <h3>Panel 3</h3>


        <asp:TextBox ID="TextBox_ListPackagesOfferedByVendor_CheckIn" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBoxListPackagesOfferedByVendor_CheckOut" runat="server"></asp:TextBox>
        <asp:DropDownList ID="DropDownList_ListPackagesOfferedByVendor_Nights" runat="server"></asp:DropDownList>
        <asp:Button ID="ButtonListPackagesOfferedByVendor_Search_Submit" runat="server" Text="Search" />
    </asp:Panel>
    <asp:Panel ID="EditProspect_Panel" runat="server">
        <asp:MultiView ID="MultiView_Prospect" runat="server" ActiveViewIndex="0">
            <asp:View ID="View_Prospect_Main" runat="server">
                <h1>Edit Prospect</h1>

                <asp:Label ID="Label5" runat="server" Text="Prospect ID"></asp:Label>
                <asp:TextBox ID="TextBox_Prospect_ProspectID" runat="server" Text="<%# Prospect.Prospect_ProspectID %>"></asp:TextBox>

                <asp:Label ID="Label6" runat="server" Text="First Name"></asp:Label>
                <asp:TextBox ID="TextBox_Prospect_FirstName" runat="server" Text="<%# Prospect.Prospect_FirstName %>"></asp:TextBox>

                <asp:Label ID="Label7" runat="server" Text="Last Name"></asp:Label>
                <asp:TextBox ID="TextBox_Prospect_LastName" runat="server" Text="<%# Prospect.Prospect_LasttName %>"></asp:TextBox>

                <asp:Label ID="Label8" runat="server" Text="Spouse First Name"></asp:Label>
                <asp:TextBox ID="TextBox_Spouse_FirstName" runat="server" Text="<%# Prospect.Prospect_SpouseFirstName%>"></asp:TextBox>

                <asp:Label ID="Label9" runat="server" Text="Spouse Last Name"></asp:Label>
                <asp:TextBox ID="TextBox_SpouseLastName" runat="server" Text="<%# Prospect.Prospect_SpouseLastName %>"></asp:TextBox>

                <asp:Label ID="Label10" runat="server" Text="Marital Status"></asp:Label>
                <asp:DropDownList ID="DropDownList_MaritalStatusID" runat="server"></asp:DropDownList>

                <asp:GridView ID="GridView_Emails" runat="server" DataKeyNames="Email_SID" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton_Email_Add" runat="server" OnClick="LinkButton_Email_Add_Click">New Email</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Email_Edit" runat="server" OnClick="LinkButton_Email_Edit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Email_Email" HeaderText="Email" SortExpression="Address" />
                        <asp:TemplateField HeaderText="Primary">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox4" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Email_IsPrimary") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox5" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Email_IsActive") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="GridView_Phones" runat="server" AutoGenerateColumns="False" DataKeyNames="Phone_SID">
                    <Columns>
                        <asp:TemplateField>

                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton_Phone_Add" runat="server" OnClick="LinkButton_Phone_Add_Click">New Phone</asp:LinkButton>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Phone_Edit" runat="server" OnClick="LinkButton_Phone_Edit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Phone_Number" HeaderText="Number" SortExpression="Number" />
                        <asp:BoundField DataField="Phone_Extension" HeaderText="Extension" SortExpression="Extension" />
                        <asp:BoundField DataField="Phone_Type" HeaderText="Type" SortExpression="Type" />
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox6" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Phone_Active") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="GridView_Addresses" runat="server" DataKeyNames="Address_SID" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton_Address_Add" runat="server" OnClick="LinkButton_Address_Add_Click">New Address</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Address_Edit" runat="server" OnClick="LinkButton_Address_Edit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Address_Address1" HeaderText="Address 1" />
                        <asp:BoundField DataField="Address_Address2" HeaderText="Address 2" />
                        <asp:BoundField DataField="Address_City" HeaderText="City" />
                        <asp:BoundField DataField="Address_PostalCode" HeaderText="Postal Code" />
                        <asp:BoundField DataField="Address_State" HeaderText="State" SortExpression="State" />
                        <asp:CheckBoxField DataField="Address_ActiveFlag" HeaderText="Active" ReadOnly="True" SortExpression="Active" />
                        <asp:CheckBoxField DataField="Address_ContractAddress" HeaderText="Contract Address" SortExpression="ContractAddress" />
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View ID="View_Prospect_Edit_Email" runat="server">

                <asp:Label ID="Label17" runat="server" Text="Edit Edit"></asp:Label>
                <asp:Label ID="Label_Edit_Email" runat="server" Text="<%# Prospect.Prospect_FullName %>"></asp:Label>

                <asp:Label ID="Label1" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="TextBox_Email_Address" runat="server" Text="<%# Prospect.Prospect_Email.Email_Email %>"></asp:TextBox>

                <asp:Label ID="Label14" runat="server" Text="Is Primary"></asp:Label>
                <asp:CheckBox ID="CheckBox_Email_IsPrimary" runat="server" Checked="<%# Prospect.Prospect_Email.Email_IsPrimary %>" />

                <asp:Label ID="Label15" runat="server" Text="Is Active"></asp:Label>
                <asp:CheckBox ID="CheckBox_Email_IsActive" runat="server" Checked="<%# Prospect.Prospect_Email.Email_IsActive %>" />

                <asp:Button ID="Button_Edit_Email_Submit" runat="server" Text="Submit" />
                <asp:Button ID="Button_Edit_Email_Cancel" runat="server" Text="Cancel" />
            </asp:View>
            <asp:View ID="View_Prospect_Edit_Phone" runat="server">
                <asp:Label ID="Label16" runat="server" Text="Edit Phone"></asp:Label>
                <asp:Label ID="Label_Edit_Phone" runat="server" Text="<%# Prospect.Prospect_FullName %>"></asp:Label>


                <asp:Label ID="Label19" runat="server" Text="Number"></asp:Label>
                <asp:TextBox ID="TextBox_Phone_Number" Text="<%# Prospect.Prospect_Phone.Phone_Number %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label20" runat="server" Text="Extension"></asp:Label>
                <asp:TextBox ID="TextBox_Phone_Extension" Text="<%# Prospect.Prospect_Phone.Phone_Extension %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label21" runat="server" Text="Type"></asp:Label>
                <asp:DropDownList ID="DropDownList_Phone_TypeID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label22" runat="server" Text="Active"></asp:Label>
                <asp:CheckBox ID="CheckBox_Phone_Active" Checked="<%# Prospect.Prospect_Phone.Phone_Active %>" runat="server" />

                <asp:Button ID="Button_Edit_Phone_Cancel" runat="server" Text="Cancel" />
                <asp:Button ID="Button_Edit_Phone_Submit" runat="server" Text="Submit" />
            </asp:View>
            <asp:View ID="View_Prospect_Edit_Address" runat="server">
                <asp:Label ID="Label24" runat="server" Text="Edit Address"></asp:Label>
                <asp:Label ID="Label_Edit_Address" runat="server" Text="<%# Prospect.Prospect_FullName %>"></asp:Label>
                <asp:Label ID="Label25" runat="server" Text="Address 1"></asp:Label>
                <asp:TextBox ID="TextBox_Address1" Text="<%# Prospect.Prospect_Address.Address_Address1 %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label26" runat="server" Text="Address 2"></asp:Label>
                <asp:TextBox ID="TextBox_Address2" Text="<%# Prospect.Prospect_Address.Address_Address2 %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label27" runat="server" Text="City"></asp:Label>
                <asp:TextBox ID="TextBox_City" Text="<%# Prospect.Prospect_Address.Address_City %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label28" runat="server" Text="State"></asp:Label>
                <asp:DropDownList ID="DropDownList_StateID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label29" runat="server" Text="Postal Code"></asp:Label>
                <asp:TextBox ID="TextBox_PostalCode" Text="<%# Prospect.Prospect_Address.Address_PostalCode %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label30" runat="server" Text="Region"></asp:Label>
                <asp:TextBox ID="TextBox_Region" Text="<%# Prospect.Prospect_Address.Address_Region %>" runat="server"></asp:TextBox>
                <asp:Label ID="Label31" runat="server" Text="Country"></asp:Label>
                <asp:DropDownList ID="DropDownList_CountryID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label32" runat="server" Text="Type"></asp:Label>
                <asp:DropDownList ID="DropDownList_Address_TypeID" runat="server"></asp:DropDownList>
                <asp:Label ID="Label33" runat="server" Text="Active"></asp:Label>
                <asp:CheckBox ID="CheckBox_ActiveFlag" Checked="<%# Prospect.Prospect_Address.Address_ActiveFlag %>" runat="server" />
                <asp:Label ID="Label34" runat="server" Text="Contract Address"></asp:Label>
                <asp:CheckBox ID="CheckBox_ContractAddress" Checked="<%# Prospect.Prospect_Address.Address_ContractAddress %>" runat="server" />

                <asp:Button ID="Button_Edit_Address_Cancel" runat="server" Text="Cancel" />
                <asp:Button ID="Button_Edit_Address_Submit" runat="server" Text="Submit" />
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <asp:Panel ID="EditReservation_Panel" runat="server">
        <h2>Panel 5</h2>
        <h1>Edit Reservation</h1>
        <asp:Label ID="Label13" runat="server" Text="Reservation ID"></asp:Label>
        <asp:TextBox ID="TextBox_ReservationID" runat="server" Text="<%# Prospect.Reservation_ReservationID %>"></asp:TextBox>
        <asp:Label ID="Label18" runat="server" Text="Reservation Number"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_Number" runat="server" Text="<%# Prospect.Reservation_ReservationNumber %>"></asp:TextBox>
        <asp:Label ID="Label35" runat="server" Text="Location"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_LocationID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label36" runat="server" Text="Check In"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_CheckIn" runat="server" Text="<%# Prospect.Reservation_CheckInDate %>"></asp:TextBox>
        <asp:Label ID="Label37" runat="server" Text="Check Out"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_CheckOut" runat="server" Text="<%# Prospect.Reservation_CheckOutDate %>"></asp:TextBox>
        <asp:Label ID="Label38" runat="server" Text="Total Nights"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_Total_Nights" runat="server"></asp:DropDownList>
        <asp:Label ID="Label40" runat="server" Text="Date Booked"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_DateBooked" runat="server" Text="<%# Prospect.Reservation_DateBooked %>"></asp:TextBox>
        <asp:Label ID="Label39" runat="server" Text="Resoert Company"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_Resort_CompanyID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label41" runat="server" Text="Lock Inventory"></asp:Label>
        <asp:CheckBox ID="CheckBox_Reservation_LockInventory" Checked="<%# Prospect.Reservation_LockInventory %>" runat="server" />
        <asp:Label ID="Label42" runat="server" Text="Status"></asp:Label>
        <asp:DropDownList ID="DropDownList_Reservation_StatusID" runat="server"></asp:DropDownList>
        <asp:Label ID="Label48" runat="server" Text="Status Date"></asp:Label>
        <asp:TextBox ID="TextBox_Reservation_StatusDate" runat="server" Text="<%# Prospect.Reservation_StatusDate %>"></asp:TextBox>
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
    <asp:Panel ID="EditTour_Panel" runat="server">
        <h2>Panel 6</h2>
        <asp:MultiView runat="server" ID="MultiView_Tour" ActiveViewIndex="0">
            <asp:View runat="server" ID="View_Tour">
                <asp:Label runat="server" Text="Tour ID"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_TourID" Text="<%# Prospect.Tour_TourID %>"></asp:TextBox>

                <asp:Label ID="Label11" runat="server" Text="Tour Date"></asp:Label>
                <asp:TextBox runat="server" ID="TextBox_TourDate" CssClass="form-control" Text="<%# Prospect.Tour_Date %>"></asp:TextBox>

                <asp:Label ID="Label12" runat="server" Text="Tour Status"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourStatusID"></asp:DropDownList>

                <asp:Label ID="Label49" runat="server" Text="Campaign"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_CampaignID"></asp:DropDownList>

                <asp:Label ID="Label50" runat="server" Text="Tour Type"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourTypeID"></asp:DropDownList>

                <asp:Label ID="Label51" runat="server" Text="Tour Source"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourSourceID"></asp:DropDownList>

                <asp:Label ID="Label65" runat="server" Text="Tour Date"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourDate"></asp:DropDownList>


                <asp:Label ID="Label52" runat="server" Text="Tour Time"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourTime"></asp:DropDownList>

                <asp:Label ID="Label53" runat="server" Text="Tour Location"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourLocationID"></asp:DropDownList>

                <asp:Label ID="Label54" runat="server" Text="Tour Sub-Type"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="DropDownList_TourSubTypeID"></asp:DropDownList>

                <asp:Label ID="Label55" runat="server" Text="Booking Date"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_BookingDate" Text="<%# Prospect.Tour_BookingDate %>"></asp:TextBox>

                <asp:GridView runat="server" ID="GridView_ListPremiums" AutoGenerateColumns="False" DataKeyNames="Premium_SID">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="Button_Premium_Edit_Select" runat="server" OnClick="Button_Premium_Edit_Select_Click" Text="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Premium_PremiumName" HeaderText="Premium Name" SortExpression="PremiumName" />
                        <asp:BoundField DataField="Premium_CostEA" HeaderText="Cost" SortExpression="Cost" />
                        <asp:BoundField DataField="Premium_Status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="Premium_QtyAssigned" HeaderText="Quantity" SortExpression="QtyAssigned" />
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View runat="server" ID="View_Premium_Edit">

                <asp:Label ID="Label56" runat="server" CssClass="control-label" Text="Premium Name"></asp:Label>
                <asp:DropDownList runat="server" ID="DropDownListList_Premiums"></asp:DropDownList>
                <asp:Label ID="Label57" runat="server" CssClass="control-label" Text="Certificate Number"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_Premium_Cert" Text="<%# Prospect.Prospect_Premium.Premium_CertificateNumber %>" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label58" runat="server" CssClass="control-label" Text="Premium Cost"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_Premium_Cost" Text='<%# Prospect.Prospect_Premium.Premium_CostEA %>' ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label59" runat="server" CssClass="control-label" Text="Chargeback Cost"></asp:Label>
                <asp:TextBox runat="server" CssClass="form-control" ID="TextBox_Premium_CostEA" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label60" runat="server" CssClass="control-label" Text="Quantity"></asp:Label>
                <asp:DropDownList runat="server" ID="DropDownList_Premium_QtyAssigned"></asp:DropDownList>
                <asp:Label ID="Label61" runat="server" CssClass="control-label" Text="Status"></asp:Label>
                <asp:DropDownList runat="server" ID="DropDownList_Premium_StatusID"></asp:DropDownList>

                <asp:Button runat="server" ID="Button_Edit_PremiumIssued_Cancel" Text="Cancel" />
                <asp:Button runat="server" ID="Button_Edit_PremiumIssued_Submit" Text="Submit" />
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <asp:Panel ID="SearchProspect_Panel" runat="server">
        <asp:TextBox runat="server" ID="TextBox_SearchProspect_Text"></asp:TextBox>
        <asp:DropDownList runat="server" ID="DropDownList_SearchProspect_Options">
            <asp:ListItem>Address</asp:ListItem>
            <asp:ListItem>City</asp:ListItem>
            <asp:ListItem>Email</asp:ListItem>
            <asp:ListItem>ID</asp:ListItem>
            <asp:ListItem></asp:ListItem>
            <asp:ListItem Selected="True">Phone</asp:ListItem>
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>Postal Code</asp:ListItem>
            <asp:ListItem>Spouse SSN</asp:ListItem>
            <asp:ListItem>SSN</asp:ListItem>
            <asp:ListItem>State</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" Text="Search..." ID="Button_SearchProspect_Search"></asp:Button>
        <asp:Button runat="server" Text="New Prospect" ID="Button_SearchProspect_New" Visible="False"></asp:Button>

        <asp:GridView runat="server" ID="Gridview_SearchProspect_Result">
            <Columns>
                <asp:TemplateField HeaderText="Select" SortExpression="ProspectID">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton_SearchProspect_Select" Text='<%# Eval("ProspectID") %>' runat="server" OnClick="LinkButton_SearchProspect_Select_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="ListPendingReservations_Panel" runat="server">
        <asp:GridView ID="GridView_ListPendngReservations" runat="server" DataKeyNames="Reservation ID" AutoGenerateColumns="False">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Select" />
                <asp:BoundField DataField="Reservation ID" HeaderText="Reservation ID" SortExpression="Reservation ID" />
                <asp:BoundField DataField="Prospect" HeaderText="Prospect" SortExpression="Prospect" />
                <asp:BoundField DataField="Package Description" HeaderText="Package Description" SortExpression="Package Description" />
                <asp:BoundField DataField="Check-IN" HeaderText="Date Check In" SortExpression="Check-IN" />
                <asp:BoundField DataField="Check-OUT" HeaderText="Date Check Out" SortExpression="Check-OUT" />
                <asp:BoundField DataField="Nights" HeaderText="Nights" SortExpression="Nights" />
                <asp:BoundField DataField="Accommodation" HeaderText="Accommodation" SortExpression="Accommodation" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="AssignRooms_Panel" runat="server">
        <h2>Rooms Selection</h2>
        <asp:MultiView ID="MultiView_AssignRooms" runat="server" ActiveViewIndex="0">
            <asp:View ID="View_AssignRooms_Result" runat="server">
                <asp:GridView ID="GridView_AssignRooms_Result" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:Button ID="Button_AssignRooms_Result_Select" runat="server" OnClick="Button_AssignRooms_Result_Select_Click" Text="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Package">
                            <ItemTemplate>
                                <asp:Label ID="Label_AssignRooms_Result_Package" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room">
                            <ItemTemplate>
                                <asp:Label ID="Label_AssignRooms_Result_Room" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size">
                            <ItemTemplate>
                                <asp:Label ID="Label_AssignRooms_Result_Size" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View ID="View_AssignRooms_Select" runat="server">
                <asp:GridView ID="GridView_AssignRooms_Select" runat="server">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="Button_AssignRooms_Select" runat="server" Text="Select" OnClick="Button_AssignRooms_Select_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Package" HeaderText="Package" SortExpression="Package" />
                        <asp:BoundField DataField="RoomNumber" HeaderText="Room Numbers" SortExpression="RoomNumber" />
                        <asp:BoundField DataField="RoomType" HeaderText="Room Type" SortExpression="RoomType" />
                    </Columns>
                </asp:GridView>
                <asp:Button ID="Button_AssignRooms_Select_Cancel" runat="server" Text="Cancel" />
            </asp:View>
        </asp:MultiView>

    </asp:Panel>
    <asp:Panel ID="ReAssignRooms_Panel" runat="server"></asp:Panel>
    <asp:Panel ID="EditHotel_Panel" runat="server">

        <asp:TextBox ID="TextBox_Hotel_ConfirmationNumber" runat="server"></asp:TextBox>
        <asp:DropDownList ID="DropDownList_Hotel_ReservationLocationID" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="DropDownList_Hotel_AccommodationID" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="DropDownList_Hotel_CheckInLocationID" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="DropDownList_Hotel_GuestTypeID" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="DropDownList_Hotel_RoomTypeID" runat="server"></asp:DropDownList>
        <asp:TextBox ID="TextBox_Hotel_ArrivalDate" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox_Hotel_DepartureDate" runat="server"></asp:TextBox>
    </asp:Panel>
    <asp:Panel ID="ProcesPayment_Panel" runat="server"></asp:Panel>
    <asp:Panel ID="ProcessRefund_Panel" runat="server"></asp:Panel>
    <asp:Panel ID="EditNotes_Panel" runat="server"></asp:Panel>
    <asp:Panel ID="Confirmation_Panel" runat="server">
        <asp:Button ID="Button_Save" runat="server" Text="Button" />
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Head2" runat="server">



    <script type="text/javascript">

        $(function () {
            $('#<%= DropDownList_TourDate.ClientID %>').on('change', function (e) {
                // retrieve tour waves stored as comma separated string in this control's custom attributes into an array
                var dates = $('#<%= DropDownList_TourDate.ClientID %>').attr(this.value.replace(/[\//]/g, '-')).split(';');

                $('#<%= DropDownList_TourTime.ClientID %>').empty();
                // append them to the tour wave drop down list
                for (var i = 0; i < dates.length; i++) {
                    $('<option/>').val(dates[i].split(',')[1]).html(dates[i].split(',')[0]).appendTo($('#<%= DropDownList_TourTime.ClientID %>'));
                }
            });
        });

    </script>
</asp:Content>
