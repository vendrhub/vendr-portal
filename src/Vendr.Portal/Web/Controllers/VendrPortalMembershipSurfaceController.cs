using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Vendr.Core.Web.Api;
using Vendr.Portal.Helpers;
using Vendr.Portal.Web.Dtos;
using Vendr.Portal.Web.Models;

namespace Vendr.Portal.Web.Controllers
{
    public class VendrPortalMembershipSurfaceController : SurfaceController
    {
        private readonly IVendrApi _vendrApi;

        public VendrPortalMembershipSurfaceController(IVendrApi vendrApi)
        {
            _vendrApi = vendrApi;
        }

        [HttpPost]
        public ActionResult Login(VendrPortalLoginDto model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            // Authenticate the member
            var username = Membership.GetUserNameByEmail(model.Email) ?? model.Email;
            if (!Membership.ValidateUser(username, model.Password))
            {
                TempData["VendrPortalErrorMessage"] = "Invalid email and/or password";
                return CurrentUmbracoPage();
            }

            // Set auth cookie
            FormsAuthentication.SetAuthCookie(username, model.RememberMe);

            // See if there is a return URL
            if (Url.IsLocalUrl(model.ReturnUrl) && model.ReturnUrl.Length > 1 && model.ReturnUrl.StartsWith("/")
                && !model.ReturnUrl.StartsWith("//") && !model.ReturnUrl.StartsWith("/\\"))
            {
                return Redirect(model.ReturnUrl);
            }

            // See if the raw URL is for a different page, and if it is, it means
            // this should be the protected page that redirected to the login so
            // redirect back to that page
            var rawUrl = Request.RawUrl;
            if (CurrentPage.Url != rawUrl)
            {
                return Redirect(rawUrl);
            }

            // If all else fails, redirect to the protected root pages first child
            var protectedRoot = CurrentPage.GetPortalProtectedRoot();
            return protectedRoot != null
                ? RedirectToUmbracoPage(protectedRoot.FirstChild())
                : RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToUmbracoPage(CurrentPage.GetPortalLoginPage());
        }

        [HttpPost]
        public ActionResult SendResetPasswordEmail(VendrPortalResetPasswordRequestDto model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var username = Membership.GetUserNameByEmail(model.Email) ?? model.Email;
            var member = Membership.GetUser(username, false);
            if (member == null)
            {
                // Even if it's an invalid member, just pretent it was successful as otherwise
                // it gives away that the email is valid which could then be used to brute force
                // the account
                TempData["VendrPortalStatus"] = "EmailSent";
                return RedirectToCurrentUmbracoPage();
            }

            // Create a simple time based code
            var key = $"VendrPortal.ResetPassword+{model.Email.ToLower().Trim().GenerateHash()}"; 
            var code = TimeBasedCodeHelper.GenerateCode();

            // Store the code in the key value store
            KeyValueStoreHelper.AddOrUpdate(key, code);

            // TODO: Create a reset password email template setting and get the value from there instead of the hard coded value below

            var emailModel = new ResetPasswordEmailModel { 
                Email = model.Email, 
                Code = code, 
                ResetPasswordUrl = Url.SurfaceAction<VendrPortalMembershipSurfaceController>("ResetPassword", new
                {
                    email = model.Email,
                    code
                })
            };
            //var emailTemplate = _vendrApi.GetEmailTemplate(CurrentPage.GetPortalStore().Id, "resetPassword");

            //try
            //{
            //    _vendrApi.SendEmail(emailTemplate, emailModel, model.Email, Thread.CurrentThread.CurrentCulture.Name);
            //} 
            //catch (Exception ex)
            //{
            //    _vendrApi.Log.Error<VendrPortalMembershipSurfaceController>(ex, "Error sending email {EmailAlias} to {EmailAddress}", emailTemplate.Alias, model.Email);
            //}

            TempData["VendrPortalResetPasswordUrl"] = emailModel.ResetPasswordUrl;

            TempData["VendrPortalStatus"] = "EmailSent";

            return RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
        [ActionName("ResetPassword")]
        public ActionResult ResetPasswordGet(VendrPortalResetPasswordBaseDto model)
        {
            // This isn't strictly necasary but I want to keep URL structures
            // for both registration and reset password links to be the same
            // and the registraion email needs to be a surface action, so we
            // force the reset password link to also be a surface action.

            TempData["email"] = model.Email;
            TempData["code"] = model.Code;

            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public ActionResult ResetPasswordPost(VendrPortalResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var username = Membership.GetUserNameByEmail(model.Email) ?? model.Email;
            var member = Membership.GetUser(username, false);
            if (member == null)
            {
                TempData["VendrPortalErrorMessage"] = "Unable to reset password";
                return RedirectToUmbracoPage(CurrentPage.GetPortalResetPasswordPage());
            }

            var key = $"VendrPortal.ResetPassword+{model.Email.ToLower().Trim().GenerateHash()}";
            var code = KeyValueStoreHelper.GetAndDelete(key);

            if (code == null || !TimeBasedCodeHelper.ValidateCode(model.Code))
            {
                TempData["VendrPortalErrorMessage"] = "Unable to reset password";
                return RedirectToUmbracoPage(CurrentPage.GetPortalResetPasswordPage());
            }

            // Change the members password
            var tmpPassword = member.ResetPassword();
            member.ChangePassword(tmpPassword, model.Password);

            // Set auth cookie
            FormsAuthentication.SetAuthCookie(username, false);

            // Redirect to protected portal root
            var protectedRoot = CurrentPage.GetPortalProtectedRoot();
            return protectedRoot != null
                ? RedirectToUmbracoPage(protectedRoot.FirstChild())
                : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult Register(VendrRegisterDto model)
        {
            // We don't use usernames, instead we use the email address to login
            // but given a username can't be changed, we don't just use the email
            // as the username, instead we set it to something random instead.
            var username = Guid.NewGuid().ToString("N");

            // Create the member (unapproved)
            var member = Membership.CreateUser(username, model.Password, model.Email, null, null, false, out MembershipCreateStatus createStatus);
            if (createStatus != MembershipCreateStatus.Success)
            {
                TempData["VendrPortalErrorMessage"] = "Unable to register member";

                return RedirectToCurrentUmbracoPage();
            }

            // Assign the member to the role
            Roles.AddUserToRole(username, "Vendr Portal Member");

            // Store a verification token in key value table
            var key = $"VendrPortal.Register+{model.Email.ToLower().Trim().GenerateHash()}";
            var code = Guid.NewGuid().ToString("N").ToUrlBase64();

            KeyValueStoreHelper.AddOrUpdate(key, code);

            // TODO: Raise event to allow adding extra URL params

            var emailModel = new RegistrationEmailModel
            {
                Email = model.Email,
                Code = code,
                ConfirmRegistrationUrl = Url.SurfaceAction<VendrPortalMembershipSurfaceController>("ConfirmRegistration", new
                {
                    email = model.Email,
                    code = code
                })
            };
            //var emailTemplate = _vendrApi.GetEmailTemplate(CurrentPage.GetPortalStore().Id, "confirmRegistration");

            //try
            //{
            //    _vendrApi.SendEmail(emailTemplate, emailModel, model.Email, Thread.CurrentThread.CurrentCulture.Name);
            //} 
            //catch (Exception ex)
            //{
            //    _vendrApi.Log.Error<VendrPortalMembershipSurfaceController>(ex, "Error sending email {EmailAlias} to {EmailAddress}", emailTemplate.Alias, model.Email);
            //}

            // TODO: Send confirm registration email
            TempData["VendrPortalStatus"] = "EmailSent";
            TempData["VendrPortalResetPasswordUrl"] = emailModel.ConfirmRegistrationUrl;

            return RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
        public ActionResult ConfirmRegistration(VendrConfirmRegistrationDto model)
        {
            var key = $"VendrPortal.Register+{model.Email.ToLower().Trim().GenerateHash()}";
            if (KeyValueStoreHelper.Get(key) != model.Code)
            {
                TempData["VendrPortalErrorMessage"] = "Unable to confirm registration";
                RedirectToUmbracoPage(CurrentPage.GetPortalLoginPage());
            }

            // Key was value so delete it so it can't be confirmed again
            KeyValueStoreHelper.Delete(key);

            var username = Membership.GetUserNameByEmail(model.Email) ?? model.Email;
            var member = Membership.GetUser(username, false);
            if (member == null)
            {
                TempData["VendrPortalErrorMessage"] = "Unable to confirm registration";
                RedirectToUmbracoPage(CurrentPage.GetPortalLoginPage());
            }

            // Approve the member
            member.IsApproved = true;
            Membership.UpdateUser(member);

            // TODO: Raise event to allow performing extra actions

            TempData["VendrPortalSuccessMessage"] = "Registration successful";

            // Redirect to the login page
            return RedirectToUmbracoPage(CurrentPage.GetPortalLoginPage());
        }
    }
}