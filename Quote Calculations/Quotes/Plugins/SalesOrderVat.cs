// <copyright file="SalesOrderVat.cs" company="Microsoft">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Microsoft</author>
// <date>2/24/2015 12:26:54 PM</date>
// <summary>Implements the SalesOrderVat Plugin.</summary>
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
// </auto-generated>
namespace Quotes.Plugins
{
    using System;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Crm.Sdk.Messages;

    /// <summary>
    /// SalesOrderVat Plugin.
    /// Fires when the following attributes are updated:
    /// totallineitemamount,new_vat
    /// </summary>    
    public class SalesOrderVat: Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderVat"/> class.
        /// </summary>
        public SalesOrderVat()
            : base(typeof(SalesOrderVat))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Update", "salesorder", new Action<LocalPluginContext>(ExecuteSalesOrderVat)));

            // Note : you can register for more events here if this plugin is not specific to an individual entity and message combination.
            // You may also need to update your RegisterFile.crmregister plug-in registration file to reflect any change.
        }

        /// <summary>
        /// Executes the plug-in.
        /// </summary>
        /// <param name="localContext">The <see cref="LocalPluginContext"/> which contains the
        /// <see cref="IPluginExecutionContext"/>,
        /// <see cref="IOrganizationService"/>
        /// and <see cref="ITracingService"/>
        /// </param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics CRM caches plug-in instances.
        /// The plug-in's Execute method should be written to be stateless as the constructor
        /// is not called for every invocation of the plug-in. Also, multiple system threads
        /// could execute the plug-in at the same time. All per invocation state information
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        protected void ExecuteSalesOrderVat(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // TODO: Implement your custom Plug-in business logic.

            IPluginExecutionContext context = localContext.PluginExecutionContext;
            IOrganizationService service = localContext.OrganizationService;
            Guid orderID = (Guid)((Entity)context.InputParameters["Target"]).Id;
            ColumnSet set = new ColumnSet();
            set.AllColumns = true;
            var order = service.Retrieve("salesorder", orderID, set);


            if (context.Depth > 5)
            {
                return;
            }
            else
            {
                var totalamount = (Money)order["totallineitemamount"];
                var discount = (Money)order["totaldiscountamount"];

                var VAT = (OptionSetValue)order["new_vat"];
                var tax = totalamount.Value * VAT.Value / 100;

                ConditionExpression condition = new ConditionExpression();
                condition.AttributeName = "salesorderid";
                condition.Operator = ConditionOperator.Equal;
                condition.Values.Add(orderID);

                FilterExpression filter = new FilterExpression();
                filter.AddCondition(condition);

                QueryExpression query = new QueryExpression();
                query.EntityName = "salesorderdetail";
                query.ColumnSet = new ColumnSet(true);
                query.Criteria = filter;

                EntityCollection orderdetails = service.RetrieveMultiple(query);

                foreach (var detail in orderdetails.Entities)
                {

                    bool isLocked = (bool)detail.Attributes["salesorderispricelocked"];

                    if (isLocked)
                    {
                        //It is really important to unlock both the Invoice and the Order!
                        UnlockSalesOrderPricingRequest unlockRequest = new UnlockSalesOrderPricingRequest();
                        unlockRequest.SalesOrderId = ((EntityReference)detail.Attributes["salesorderid"]).Id;
                        UnlockSalesOrderPricingResponse unlockRequestResponse = (UnlockSalesOrderPricingResponse)service.Execute(unlockRequest);
                    }

                    var quantity = (decimal)detail["quantity"];
                    var priceperunit = (Money)detail["priceperunit"];
                    var teamleader = (OptionSetValue)detail["new_tldiscount"];

                    //Then I calculate the manual discount and baseamount, for the further calculations
                    //detail.Attributes["manualdiscountamount"] = new Money((priceperunit.Value * teamleader.Value / 100) * quantity);
                    var manualdiscountamount = (Money)detail.Attributes["manualdiscountamount"];
                    //detail.Attributes["baseamount"] = new Money(priceperunit.Value * quantity);
                    var baseamount = (Money)detail["baseamount"];

                    //finally I calculate the tax
                    detail["new_vat"] = new OptionSetValue(VAT.Value);
                    var taxDetail = (baseamount.Value - manualdiscountamount.Value) * VAT.Value / 100;
                    detail.Attributes["tax"] = new Money(taxDetail); //tax

                    service.Update(detail);
                }

                order["new_totalamountincludingvat"] = new Money((totalamount.Value - discount.Value) + tax);

                service.Update(order);
            }
        }
    }
}
