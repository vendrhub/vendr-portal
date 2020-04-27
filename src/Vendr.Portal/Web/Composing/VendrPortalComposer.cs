using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Vendr.Portal.Web.Routing;

namespace Vendr.Portal.Web.Composing
{
    public class VendrPortalComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            //composition.UrlProviders().Insert<VendrPortalProtectedUrlProvider>();

            composition.Components().Append<VendrPortalComponent>();
        }
    }
}