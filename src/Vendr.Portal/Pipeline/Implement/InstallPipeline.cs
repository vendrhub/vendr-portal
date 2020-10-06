using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vendr.Portal.Pipeline.Implement.Tasks;

namespace Vendr.Portal.Pipeline.Implement
{
    internal class InstallPipeline : Pipeline<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipeline()
        {
            Tasks = input => input
                .Pipe(new CreateVendrPortalDataTypesTask())
                .Pipe(new CreateVendrPortalDocumentTypesTask())
                .Pipe(new CreateVendrPortalNodesTask());
        }
    }
}