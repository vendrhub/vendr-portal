using System;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Vendr.Portal.Web.Routing
{
    public class VendrPortalProtectedUrlProvider : DefaultUrlProvider
    {
        public VendrPortalProtectedUrlProvider(IRequestHandlerSection requestSettings, ILogger logger, IGlobalSettings globalSettings, ISiteDomainHelper siteDomainHelper) 
            : base(requestSettings, logger, globalSettings, siteDomainHelper)
        { }

        public override UrlInfo GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlMode mode, string culture, Uri current)
        {
            var protectedRoot = content.Ancestor(VendrPortalConstants.ContentTypes.Aliases.ProtectedRoot);
            var isProtected = protectedRoot != null;

            return isProtected
                ? ConstructProtectedUrl(umbracoContext, content, protectedRoot, mode, culture, current) 
                : base.GetUrl(umbracoContext, content, mode, culture, current);
        }

        private UrlInfo ConstructProtectedUrl(UmbracoContext umbracoContext, IPublishedContent content, IPublishedContent protectedRoot, UrlMode mode, string culture, Uri current)
        {
            var protectedRootUrl = base.GetUrl(umbracoContext, protectedRoot, mode, culture, current).Text;
            var protectedRootParentUrl = protectedRootUrl.TrimEnd('/').TrimEnd(protectedRoot.UrlSegment);
            var contentUrl = base.GetUrl(umbracoContext, content, mode, culture, current);
            var newContentUrl = protectedRootParentUrl + contentUrl.Text.TrimStart(protectedRootUrl);
            return new UrlInfo(newContentUrl, true, culture);
        }
    }
}