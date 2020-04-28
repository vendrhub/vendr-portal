using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Vendr.Portal.Web.Composing
{
    public class VendrPortalComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<VendrPortalComponent>();
        }
    }
}