using System.Web;
using System.Web.Security;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Vendr.Portal.Web.Composing
{
    public class VendrPortalComponent : IComponent
    {
        public void Initialize()
        {
            // Auto-redirect login page to protected root if already logged in
            //PublishedRequest.Prepared += (sender, args) =>
            //{
            //    var request = sender as PublishedRequest;
            //    if (!request.HasPublishedContent) return;

            //    var httpRequest = HttpContext.Current.Request;

            //    var content = request.PublishedContent;
            //    if (content.ContentType.Alias == VendrPortalConstants.ContentTypes.Aliases.LoginPage)
            //    {
            //        // TODO: Maybe check a portal role?
            //        if (httpRequest.IsAuthenticated && Membership.GetUser(false) != null)
            //        {
            //            var redirectNode = content.GetPortalProtectedRoot()?.FirstChild();
            //            if (redirectNode != null)
            //            {
            //                request.SetRedirect(redirectNode.Url);
            //            }
            //        }
            //    }
            //};

            // Sync email address to the member name as we aren't capturing this
            // and the username isn't very friendly
            MemberService.Saving += (sender, args) => {
                foreach (var member in args.SavedEntities)
                {
                    member.Name = member.Email;
                }
            };
        }

        public void Terminate()
        { }
    }
}