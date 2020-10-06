using Vendr.Core.Models;

namespace Vendr.Portal.Pipeline.Implement
{
    public class InstallPipelineContext
    {
        public int SiteRootNodeId { get; set; }

        public StoreReadOnly Store { get; set; }
    }
}