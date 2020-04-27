using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vendr.Portal
{
    public static class VendrPortalConstants
    {
        public static class ContentTypes
        {
            public static class Aliases
            {
                public const string Base = "vendrPortalBase";
                public const string Root = "vendrPortalRoot";
                public const string LoginPage = "vendrPortalLoginPage";
                public const string ResetPasswordPage = "vendrPortalResetPasswordPage";
                public const string RegisterPage = "vendrPortalRegisterPage";
                public const string ErrorPage = "vendrPortalErrorPage";
                public const string ProtectedRoot = "vendrPortalProtectedRoot";
            }

            //public static class Guids
            //{
            //    public const string BasePage = "55f4b88e-69b6-45a4-bc4f-d48f35f6b904";
            //    public static readonly Guid BasePageGuid = new Guid(BasePage);

            //    public const string CheckoutPage = "e5e809cf-f3e5-4bb8-b7bb-c67c8303c2f4";
            //    public static readonly Guid CheckoutPageGuid = new Guid(CheckoutPage);

            //    public const string CheckoutStepPage = "d9384576-e6a8-4ef2-8ca8-475ee6e7546d";
            //    public static readonly Guid CheckoutStepPageGuid = new Guid(CheckoutStepPage);
            //}
        }
    }
}