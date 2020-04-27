using Umbraco.Web;
using Umbraco.Core.Models.PublishedContent;
using Vendr.Core.Models;
using Vendr.Core;

namespace Vendr.Portal.Web
{
    public static class PublishedContentExtensions
    {
        public static StoreReadOnly GetPortalStore(this IPublishedContent content)
        {
            return content.Value<StoreReadOnly>(Constants.Properties.StorePropertyAlias, fallback: Fallback.ToAncestors);
        }

        public static IPublishedContent GetPortalRoot(this IPublishedContent content)
        {
            return content.AncestorOrSelf(VendrPortalConstants.ContentTypes.Aliases.Root);
        }

        public static IPublishedContent GetPortalProtectedRoot(this IPublishedContent content)
        {
            return content.GetPortalRoot().FirstChildOfType(VendrPortalConstants.ContentTypes.Aliases.ProtectedRoot);
        }

        public static IPublishedContent GetPortalLoginPage(this IPublishedContent content)
        {
            return content.GetPortalRoot().FirstChildOfType(VendrPortalConstants.ContentTypes.Aliases.LoginPage);
        }

        public static IPublishedContent GetPortalResetPasswordPage(this IPublishedContent content)
        {
            return content.GetPortalRoot().FirstChildOfType(VendrPortalConstants.ContentTypes.Aliases.ResetPasswordPage);
        }

        public static IPublishedContent GetPortalRegisterPage(this IPublishedContent content)
        {
            return content.GetPortalRoot().FirstChildOfType(VendrPortalConstants.ContentTypes.Aliases.RegisterPage);
        }
    }
}