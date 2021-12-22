using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace Dynamics365Plugin
{
    public class fieldDataCopier : CodeActivity

    {
        [RequiredArgument]
        [Input("Pass your data here...")]
        public InArgument<string> DataInput { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ITracingService tracingService = context.GetExtension<ITracingService>();
            tracingService.Trace("This will appear in the Trace Log");
            try
            {

                string input = DataInput.Get(context);
                IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
                // Use the context service to create an instance of IOrganizationService.             
                IOrganizationService service = serviceFactory.CreateOrganizationService(Guid.Parse("0B58A93E-B1E5-EB11-BACB-000D3A5ACB40"));
                test1(input, workflowContext, service);
                tracingService.Trace("Workflow executed successfully");
            }
            catch (Exception e)
            {
                throw new InvalidWorkflowException("The workflow failed with reason: " + e.Message);
            }
        }

        private static void test1(String input, IWorkflowContext workflowContext, IOrganizationService service)
        {
            Guid recordId = workflowContext.PrimaryEntityId;
            Entity updateentity = new Entity(workflowContext.PrimaryEntityName);
            updateentity.Id = recordId;
            updateentity["description"] = input;
            service.Update(updateentity);
        }
    }
}
