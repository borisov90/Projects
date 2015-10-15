// <copyright file="PostQuoteUpdate.cs" company="Microsoft">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Microsoft</author>
// <date>3/13/2015 4:12:51 PM</date>
// <summary>Implements the PostQuoteUpdate Plugin.</summary>
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
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// PostQuoteUpdate Plugin.
    /// Fires when the following attributes are updated:
    /// new_createinvoice
    /// </summary>    
    public class PostQuoteUpdate : Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostQuoteUpdate"/> class.
        /// </summary>
        public PostQuoteUpdate()
            : base(typeof(PostQuoteUpdate))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Update", "quote", new Action<LocalPluginContext>(ExecutePostQuoteUpdate)));

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
        protected void ExecutePostQuoteUpdate(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // TODO: Implement your custom Plug-in business logic.

            //IPluginExecutionContext context = localContext.PluginExecutionContext;
            //IOrganizationService service = localContext.OrganizationService;
            //Guid quoteProductID = (Guid)((Entity)context.InputParameters["Target"]).Id;
            //var serviceContext = new OrganizationServiceContext(service);
            //ColumnSet set = new ColumnSet();
            //set.AllColumns = true;
            //var quote = service.Retrieve("quote", quoteProductID, set);

            //if (context.Depth > 5)
            //{
            //    return;
            //}
            //else
            //{
            //    var salesorderType = (OptionSetValue)quote["new_createinvoice"];

            //    if (salesorderType.Value == 1)
            //    {
            //        var salesorder = new Entity("salesorder");
            //        salesorder["name"] = quote["name"] + " Invoice Order for the Prepayment";
            //        service.Create(salesorder);
            //    }
            //    else if(salesorderType.Value == 2)
            //    {
            //        var salesorder = new Entity("salesorder");
            //        salesorder["name"] = quote["name"] + " Invoice Order";
            //        service.Create(salesorder);
            //    }
               
            //}
        }
    }
}