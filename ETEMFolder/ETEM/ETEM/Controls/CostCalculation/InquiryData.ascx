<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquiryData.ascx.cs" Inherits="ETEM.Controls.CostCalculation.InquiryData" %>

<%@ Register Src="~/Controls/Common/SMCDropDownList.ascx" TagName="SMCDropDownList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Common/SMCAutoCompleteTextBox.ascx" TagName="SMCAutoComplete" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Common/SMCTextArea.ascx" TagName="SMCTextArea" TagPrefix="uc3" %>
<%@ Register Src="../Common/SMCCalendar.ascx" TagName="SMCCalendar" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/CostCalculation/ProfilesListCtrlChooseOffer.ascx" TagName="ucProfilesListCtrlChooseOffer" TagPrefix="uc5" %>

<asp:Panel ID="pnlUserMainData" runat="server">
    <div class="ResultContext">
        <asp:Label ID="lbResultContext" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="span4">
                <p>
                    <asp:Label ID="lbInquiryNumber" runat="server" Text="Inquiry No" ></asp:Label>
                </p>
                <asp:TextBox ID="tbxInquiryNumber" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbOfferDate" runat="server" Text="Date" CssClass="required"></asp:Label>
                </p>
                <uc4:SMCCalendar ID="tbxOfferDate" runat="server" CssClassTextBox="mandatory" />
            </div>
            <div class="span4">
                <p>
                    <asp:Label ID="lbCustomer" runat="server" Text="Customer" CssClass="required"></asp:Label>
                </p>

                <asp:TextBox ID="tbxCustomer" runat="server" CssClass="span4 mandatory"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span12 bold tabBlue">
                Die dimensions & Price
            </div>
        </div>
        <div class="sectionBorder">
            <div class="row">
                <div class="span8">
                    <p>
                        <asp:Label ID="lbProfileSetting" runat="server" Text="Profile"  CssClass="required"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxProfileSettingName" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>

                    <asp:HiddenField ID="hdnIdProfileSetting" runat="server" />
                    <asp:Button ID="btnChoose" runat="server" Text="Choose Profile" CssClass="btn modalWindow mandatory" OnClick="btnChoose_Click" />
                    <asp:Button ID="btnShowHideGridView" runat="server" Text="Show Validation" CssClass="btn mandatory" OnClick="btnShowHideGridView_Click" />
                </div>
                <div class="span4">
                    <p>
                        <asp:Label ID="Label12" runat="server" Text="Number of cavities" ></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlNumberOfCavities" runat="server" DropDownEnabled="false" KeyTypeIntCode="NumberOfCavities" ShowButton="false" CssClassDropDown="mandatory" />

                </div>
            </div>

            <div class="row" runat="server" id="divGridValidation">
                <div class="span8">
                    <asp:GridView ID="gvSettingValidation" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" ShowHeader="False" CssClass="MainGrid" BorderWidth="0px" ForeColor="Red" Font-Size="Smaller">
                        <Columns>
                            <asp:BoundField DataField="ValidationRequirement" SortExpression="ValidationRequirement" ItemStyle-Width="400px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lbEmptyDataText" runat="server" Text='<%# GetCaption("GridView_EmptyDataRow") %>' ItemStyle-Width="400px" />
                        </EmptyDataTemplate>
                        <PagerStyle CssClass="cssPager" />
                    </asp:GridView>
                </div>
            </div>

            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbValueForA" runat="server" Text="A (mm) - Length"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxValueForA" runat="server"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbValueForB" runat="server" Text="B (mm) - Width" ></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxValueForB" runat="server"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbValueForC" runat="server" Text="C (mm)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxValueForC" runat="server"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbValueForD" runat="server" Text="D (mm)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxValueForD" runat="server"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbValueForS" runat="server" Text="s min (mm) - Tickness"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxValueForS" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="Label32" runat="server" Text="Inner section perimeter" CssClass="required"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPin" runat="server" CssClass="span4" ></asp:TextBox>
                </div>
                 <div class="span2">
                    <p>
                        <asp:Label ID="Label33" runat="server" Text="Outer section perimeter" CssClass="required"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPout" runat="server" CssClass="span4" ></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDiameter" runat="server" Text="Ø (mm)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDiameter" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDieDiameter" runat="server" Text="Ø die (mm)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDieDiameter" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDieDimensions" runat="server" Text="Die dimensions (mm)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxDieDimensions" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbPress" runat="server" Text="Press"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxPress" runat="server" CssClass="span4" Enabled="false" Visible="false"></asp:TextBox>
                    <uc1:SMCDropDownList ID="ddlPress" runat="server" DataSourceType="PRESS" KeyTypeIntCode="CostCenter" ShowButton="false" CssClassDropDown="mandatory"  DropDownEnabled="false" />
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbAging" runat="server" Text="Aging" ></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlAging" runat="server" KeyTypeIntCode="YES_NO" ShowButton="false" CssClassDropDown="mandatory" DefaultValueByName="YES" DropDownEnabled="false" />

                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label30" runat="server" Text="Hardness" CssClass="required"></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlHardness" runat="server" KeyTypeIntCode="Hardness" ShowButton="false" CssClassDropDown="mandatory"  />

                </div>
            </div>


            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbDieprice" runat="server" Text="Die price (EUR)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDiePrice" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbVendor" runat="server" Text="Vendor"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxVendor" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span6">
                    <p>
                        <asp:Label ID="lbPricelist" runat="server" Text="Price list"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxPriceListStr" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>

                </div>

            </div>

        </div>
        <div class="row">
            <div class="span12 bold tabBlue">
                Offer parameters
            </div>
        </div>
        <div class="sectionBorder">
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbLengthOfFinalPC" runat="server" Text="Length of final PC (mm)" CssClass="required"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxLengthOfFinalPC" runat="server" CssClass="span4"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbWeightPerMeter" runat="server" Text="Weight per meter (gr/m)" CssClass="required"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxWeightPerMeter" runat="server" CssClass="span4"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbWeightPerPC" runat="server" Text="Weight per PC (kg)"  ></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxWeightPerPC" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label14" runat="server" Text="Pcs for the whole project"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPcsForTheWholeProject" runat="server" CssClass="span4"></asp:TextBox>
                </div>


                <div class="span2">
                    <p>
                        <asp:Label ID="Label7" runat="server" Text="Tonnage (kg)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxTonnage_KG" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>

                <div class="span2">
                    <p>
                        <asp:Label ID="Label15" runat="server" Text="Tonnage (ton)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxTonnage" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>

            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="Label9" runat="server" Text="Days of credit" ></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxDaysOfCredit" runat="server" CssClass="span4 mandatory"></asp:TextBox>
                </div>

                <div class="span2">
                    <p>
                        <asp:Label ID="Label11" runat="server" Text="Gross margin (%)" ></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxGrossMargin" runat="server" CssClass="span4 mandatory"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label31" runat="server" Text="Standard packaging" ></asp:Label>
                    </p>

                    <uc1:SMCDropDownList ID="ddlStandardPackaging" runat="server" KeyTypeIntCode="YES_NO" ShowButton="false" CssClassDropDown="mandatory" DefaultValueByName="YES"  AddingDefaultValue="false"/>
                </div>
                <div class="span2" >
                    <p>
                        <asp:Label ID="Label4" runat="server" Text="Savings Rate (%)" CssClass="required" Visible="false"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxSavingsRate" runat="server" CssClass="span4" Visible="false"></asp:TextBox>
                </div>
                <div class="span2" runat="server" id="divInterest" visible="false">
                    <p>
                        <asp:Label ID="Label10" runat="server" Text="Interest (%)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxInterest" runat="server" CssClass="span4"></asp:TextBox>
                </div>

            </div>
        </div>
        <div class="row">
            <div class="span12">
            </div>
        </div>


        <div class="row">
            <div class="span12 bold tabBlue">
                Material price
            </div>
        </div>
        <div class="sectionBorder">
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbLME" runat="server" Text="LME (EUR/ton)" ></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxLME" runat="server" CssClass="span2 mandatory"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbPREMIUM" runat="server" Text="PREMIUM (EUR/ton)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxPREMIUM" runat="server" CssClass="span2 mandatory"></asp:TextBox>
                </div>


                <div class="span4">
                    <p>
                        <asp:Label ID="Label16" runat="server" Text="Price list"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxMaterialPriceList" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbMaterial" runat="server" Text="Aluminium (EUR/ton)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxMaterial" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                    <asp:HiddenField ID="hdnIdMaterialPriceList" runat="server" />
                </div>

                <div class="span2">
                    <p>
                        <asp:Label ID="Label25" runat="server" Text="Aluminium (EUR)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxMaterial_EUR" runat="server" CssClass="span2 yellow-colored" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="span12 bold tabBlue">
                Manufacturing
            </div>
        </div>
        <div class="sectionBorder">
            <div class="row">
                <div class="span8">
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label8" runat="server" Text="Extrusion (EUR/ton)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxExtrusion_EUR_TON" runat="server" CssClass="span2 " Enabled="false"></asp:TextBox>
                </div>

                <div class="span2">
                    <p>
                        <asp:Label ID="Label27" runat="server" Text="Extrusion (EUR)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxExtrusion_EUR" runat="server" CssClass="span2 yellow-colored"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span8">
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label26" runat="server" Text="Packaging  (EUR/ton)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPackaging_EUR_TON" runat="server" CssClass="span2 " Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label28" runat="server" Text="Packaging  (EUR)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxPackaging_EUR" runat="server" CssClass="span2 yellow-colored"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="span12 bold tabBlue">
                Transportation Cost & Commission
            </div>
        </div>
        <div class="sectionBorder">
            <div class="row">
                <div class="span8">
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label18" runat="server" Text="Transportation (EUR/ton)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxTransportationCost" runat="server" CssClass="span2"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label19" runat="server" Text="Transportation (EUR)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxTransportation_EUR" runat="server" CssClass="span2 yellow-colored" Enabled="false"> </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="lbCommision" runat="server" Text="Commision" ></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlCommision" runat="server" KeyTypeIntCode="Commision" ShowButton="false" CssClassDropDown="span2 mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlCommision_SelectedIndexChanged" />

                </div>


                <div class="span2">
                    <p>
                        <asp:Label ID="lbAgent" runat="server" Text="Agent" ></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlAgent" runat="server" KeyTypeIntCode="Agent" ShowButton="false" CssClassDropDown="span2 mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlCalculationCommission_SelectedIndexChanged"/>

                </div>

                <div class="span4">
                    <p>
                        <asp:Label ID="Label13" runat="server" Text="Calculation of commission" ></asp:Label>
                    </p>
                    <uc1:SMCDropDownList ID="ddlCalculationCommission" runat="server" KeyTypeIntCode="CalculationCommission" ShowButton="false" CssClassDropDown="mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlCalculationCommission_SelectedIndexChanged" />

                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="lbCommission" runat="server" Text="Commission (%)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxCommission" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label17" runat="server" Text="Commission (EUR)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxCommission_EUR" runat="server" CssClass="span2 yellow-colored" Enabled="false"></asp:TextBox>
                </div>

            </div>
        </div>


        <div class="row">
            <div class="span12 bold tabBlue">
                SGA's and financial expenses 
            </div>
        </div>
        <div class="sectionBorder">
            <div class="row">
                <div class="span2">
                    <p>
                        <asp:Label ID="Label20" runat="server" Text="Administration (EUR/ton)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxAdministrationExpenses" runat="server" CssClass="span2"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label21" runat="server" Text="Sales (EUR/ton)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxSalesExpenses" runat="server" CssClass="span2"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label22" runat="server" Text="Fin. fixed (EUR/ton)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxFinancialFixedExpenses" runat="server" CssClass="span2"></asp:TextBox>
                </div>
                <div class="span2">
                    <p>
                        <asp:Label ID="Label23" runat="server" Text="Fin. variable (EUR/ton)"></asp:Label>
                    </p>
                    <asp:TextBox ID="tbxFinancialVariableExpenses" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                </div>

                <div class="span2">
                    <p>
                        <asp:Label ID="Label5" runat="server" Text="SGA's and fin. (EUR/ton)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxSGAsAndFin_EUR_TON" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                </div>

                <div class="span2">
                    <p>
                        <asp:Label ID="Label24" runat="server" Text="SGA's and fin. (EUR)"></asp:Label>
                    </p>

                    <asp:TextBox ID="tbxSGAsAndFin_EUR" runat="server" CssClass="span2 yellow-colored" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="span12">
            </div>
        </div>
        <div class="row">
            <div class="span2">
                <p>
                    <asp:Label ID="Label6" runat="server" Text="Target price (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxTargetPrice" runat="server" CssClass="span2"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span10">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Total sales price (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxTotalSalesPrice_EUR_TON" runat="server" CssClass="span2 yellow-colored" Enabled="false"></asp:TextBox>
            </div>
            <div class="span2">
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Total sales price (EUR)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxTotalSalesPrice_EUR" runat="server" CssClass="span2 yellow-colored" Enabled="false"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="span10">
                <p>
                    <asp:Label ID="Label3" runat="server" Text="Offered sales price to customer (EUR/ton)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxActualTotalSalesPriceToCustomer" runat="server" CssClass="span2"></asp:TextBox>
            </div>
            <div class="span2">
                <p>
                    <asp:Label ID="Label29" runat="server" Text="Offered sales price to customer (EUR)"></asp:Label>
                </p>

                <asp:TextBox ID="tbxActualTotalSalesPriceToCustomerAll" runat="server" CssClass="span2 yellow-colored" Enabled="false"></asp:TextBox>
            </div>

        </div>





    </div>

    <asp:HiddenField ID="hdnParentControlID" runat="server" />
    <asp:HiddenField ID="hdnRowMasterKey" runat="server" />
</asp:Panel>

<uc5:ucProfilesListCtrlChooseOffer ID="ucProfilesListCtrlChooseOffer" runat="server" Visible="false" />
