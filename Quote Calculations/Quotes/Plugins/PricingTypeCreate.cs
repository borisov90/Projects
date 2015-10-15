// <copyright file="PricingTypeCreate.cs" company="Microsoft">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Microsoft</author>
// <date>2/27/2015 12:22:14 PM</date>
// <summary>Implements the PricingTypeCreate Plugin.</summary>
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
    /// PricingTypeCreate Plugin.
    /// </summary>    
    public class PricingTypeCreate: Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PricingTypeCreate"/> class.
        /// </summary>
        public PricingTypeCreate()
            : base(typeof(PricingTypeCreate))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Create", "quotedetail", new Action<LocalPluginContext>(ExecutePricingTypeCreate)));

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
        protected void ExecutePricingTypeCreate(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // TODO: Implement your custom Plug-in business logic.
            IPluginExecutionContext context = localContext.PluginExecutionContext;
            IOrganizationService service = localContext.OrganizationService;

            Guid quoteDetailID = (Guid)((Entity)context.InputParameters["Target"]).Id;
            ColumnSet set = new ColumnSet();
            set.AllColumns = true;
            var quoteDetail = service.Retrieve("quotedetail", quoteDetailID, set);
            const decimal minimumValue = 0;
            const decimal minimumRatio = 1;

            if (context.Depth > 1)
            {
                return;
            }
            else
            {
                var priceperunit = (Money)quoteDetail["priceperunit"];
                quoteDetail["new_specificdiscountpercentage"] = minimumValue;
                quoteDetail["new_ratio"] = minimumRatio;
                quoteDetail["new_grossannualincome"] = new Money(minimumValue);
                quoteDetail["new_gaixratio"] = new Money(minimumValue);
                quoteDetail["new_recommendedvalue"] = priceperunit;
                //quoteDetail["new_fixedpriceplusratio"] = new Money(minimumValue);
               
                //This sets the current pricing type of the quote product, to the default pricing type of the quote
                var parentQuote = (EntityReference)quoteDetail["quoteid"];

                var quote = service.Retrieve(parentQuote.LogicalName, parentQuote.Id, new ColumnSet(true));

                var pricingType = (OptionSetValue)quote["new_pricingtype"];

                quoteDetail["new_pricingtype"] = new OptionSetValue(pricingType.Value);

                service.Update(quoteDetail);
            }
        }
    }
}