using System;
using System.Collections.Generic;

namespace Vendr.Portal
{
    public static class VendrPortalConstants
    {
        public static IDictionary<string, string> ColorMap = new Dictionary<string, string>
        {
            { "000000", "black" },
            { "f56565", "red-500" },
            { "ed8936", "orange-500" },
            { "ecc94b", "yellow-500" },
            { "48bb78", "green-500" },
            { "38b2ac", "teal-500" },
            { "4299e1", "blue-500" },
            { "667eea", "indigo-500" },
            { "9f7aea", "purple-500" },
            { "ed64a6", "pink-500" }
        };

        public static class DataTypes
        {
            public static class Guids
            {
                public const string StorePicker = "8aa96369-dde6-4213-aba6-470f9ec1c098c";
                public static readonly Guid StorePickerGuid = new Guid(StorePicker);

                public const string ThemeColorPicker = "e180aa34-eb1b-4db7-bcf8-8dfdfc3ad651";
                public static readonly Guid ThemeColorPickerGuid = new Guid(ThemeColorPicker);

                public const string EmailTemplatePicker = "b18e88ff-6448-4e75-b16e-48acad14ee2c";
                public static readonly Guid EmailTemplatePickerGuid = new Guid(EmailTemplatePicker);
            }
        }

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

            public static class Guids
            {
                public const string Base = "b34516e1-f8ec-4f56-bd2c-78aec56353d9";
                public static readonly Guid BaseGuid = new Guid(Base);

                public const string Root = "04aae650-b110-4012-87c4-edb17c7a89ea";
                public static readonly Guid RootGuid = new Guid(Root);

                public const string LoginPage = "250dfcac-2b04-41c9-a3df-ec99ef3152a0";
                public static readonly Guid LoginPageGuid = new Guid(LoginPage);

                public const string ResetPasswordPage = "09efd58f-9a8d-4911-897c-ff62dd8ab856";
                public static readonly Guid ResetPasswordPageGuid = new Guid(ResetPasswordPage);

                public const string RegisterPage = "6e144883-0265-4aec-944e-0a0ecb97b45e";
                public static readonly Guid RegisterPageGuid = new Guid(RegisterPage);

                public const string ErrorPage = "1ef50c8b-c8ff-4a29-b03e-d5b035dbe0a3";
                public static readonly Guid ErrorPageGuid = new Guid(ErrorPage);

                public const string ProtectedRoot = "3462cc96-9360-4b19-9d8d-13ed0b63beed";
                public static readonly Guid ProtectedRootGuid = new Guid(ProtectedRoot);
            }
        }
    }
}