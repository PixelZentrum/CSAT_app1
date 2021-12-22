using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365Plugin
{
    public class invoiceTest : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            try
            {
                
                // Obtain the execution context from the service provider
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                // The InputParameters collection contains all the data passed in the message request.
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    // Obtain the target entity from the input parameters.
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName == "invoice")
                    {
                        if (entity.Attributes.Contains("name") == true)
                        {

                            Entity updateentity = new Entity(entity.LogicalName);

                            updateentity.Id = entity.Id;

                            Random rd = new Random();
                            int rand_num = rd.Next(100, 200);

                            updateentity["description"] = rand_num.ToString();
                            service.Update(updateentity);
                        }
                        else
                        {
                            throw new InvalidPluginExecutionException("Error occured.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("an error occurred...\n");
                tracingService.Trace(ex.Message);
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
