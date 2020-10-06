using System;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Composing;

namespace Vendr.Portal.Pipeline.Implement.Tasks
{
    internal class CreateVendrPortalDocumentTypesTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            // Setup variables
            IContentType existing;

            int portalBaseContentTypeId;
            int checkoutStepPageContentTypeId;

            // Setup lazy data types
            var textstringDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.TextstringGuid));
            var textareaDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.TextareaGuid));
            var booleanDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.CheckboxGuid));
            var contentPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.ContentPickerGuid));
            var mediaPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.MediaPickerGuid));
            var themeColorPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(VendrPortalConstants.DataTypes.Guids.ThemeColorPickerGuid));
            var stepPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(VendrPortalConstants.DataTypes.Guids.StepPickerGuid));

            // Portal Base
            existing = Current.Services.ContentTypeService.Get(VendrPortalConstants.ContentTypes.Guids.BaseGuid);

            if (existing == null)
            {
                var contentType = new ContentType(-1)
                {
                    Key = VendrPortalConstants.ContentTypes.Guids.BaseGuid,
                    Alias = VendrPortalConstants.ContentTypes.Aliases.Base,
                    Name = "[Vendr Portal] Base"
                };

                Current.Services.ContentTypeService.Save(contentType);

                portalBaseContentTypeId = contentType.Id;
            }
            else
            {
                portalBaseContentTypeId = existing.Id;
            }

            // Portal Order History Page
            // Portal Account Settings Page

            // Portal Login Page
            // Portal Reset Password Page
            // Portal Register Page
            // Portal Error Page
            // Portal Protected Root

            // Portal Root
            existing = Current.Services.ContentTypeService.Get(VendrPortalConstants.ContentTypes.Guids.RootGuid);

            if (existing == null)
            {
                var contentType = new ContentType(portalBaseContentTypeId)
                {
                    Key = VendrPortalConstants.ContentTypes.Guids.RootGuid,
                    Alias = VendrPortalConstants.ContentTypes.Aliases.Root,
                    Name = "[Vendr Portal] Root",
                    Icon = "icon-globe-alt",
                    AllowedContentTypes = new[]{
                        new ContentTypeSort(checkoutStepPageContentTypeId, 1)
                    },
                    PropertyGroups = new PropertyGroupCollection(new[]{
                        new PropertyGroup(new PropertyTypeCollection(true, new[]{
                            new PropertyType(mediaPickerDataType.Value) {
                                Alias = "vendrStoreLogo",
                                Name = "Store Logo",
                                Description = "A logo image for the store to appear at the top of the checkout screens and order emails.",
                                SortOrder = 10
                            },
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrStoreAddress",
                                Name = "Store Address",
                                Description = "The address of the web store to appear in the footer of order emails.",
                                SortOrder = 20
                            },
                            new PropertyType(themeColorPickerDataType.Value) {
                                Alias = "vendrThemeColor",
                                Name = "Theme Color",
                                Description = "The theme color to use for colored elements of the checkout pages.",
                                SortOrder = 30
                            },
                            new PropertyType(booleanDataType.Value) {
                                Alias = "vendrCollectShippingInfo",
                                Name = "Collect Shipping Info",
                                Description = "Select whether to collect shipping information or not. Not necessary if you are only dealing with digital downloads.",
                                SortOrder = 40
                            },
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrOrderLinePropertyAliases",
                                Name = "Order Line Property Aliases",
                                Description = "Comma separated list of order line property aliases to display in the order summary.",
                                SortOrder = 50
                            },
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrPortalBackPage",
                                Name = "Portal Back Page",
                                Description = "The page to go back to when backing out of the checkout flow.",
                                SortOrder = 60
                            },
                            new PropertyType(contentPickerDataType.Value) {
                                Alias = "vendrTermsAndConditionsPage",
                                Name = "Terms and Conditions Page",
                                Description = "The page on the site containing the terms and conditions.",
                                SortOrder = 70
                            },
                            new PropertyType(contentPickerDataType.Value) {
                                Alias = "vendrPrivacyPolicyPage",
                                Name = "Privacy Policy Page",
                                Description = "The page on the site containing the privacy policy.",
                                SortOrder = 80
                            },
                            new PropertyType(booleanDataType.Value) {
                                Alias = "umbracoNaviHide",
                                Name = "Hide from Navigation",
                                Description = "Hide the checkout page from the sites main navigation.",
                                SortOrder = 90
                            }
                        })) {
                            Name = "Settings",
                            SortOrder =100
                        }
                    })
                };

                Current.Services.ContentTypeService.Save(contentType);

                checkoutStepPageContentTypeId = contentType.Id;
            }
            else
            {
                checkoutStepPageContentTypeId = existing.Id;
            }

            // Portal Page
            //existing = Current.Services.ContentTypeService.Get(VendrPortalConstants.ContentTypes.Guids.PortalPageGuid);

            //if (existing == null)
            //{
            //    var contentType = new ContentType(portalBaseContentTypeId)
            //    {
            //        Key = VendrPortalConstants.ContentTypes.Guids.PortalPageGuid,
            //        Alias = VendrPortalConstants.ContentTypes.Aliases.PortalPage,
            //        Name = "[Vendr Portal] Portal Page",
            //        Icon = "icon-cash-register color-green",
            //        AllowedContentTypes = new[]{
            //            new ContentTypeSort(checkoutStepPageContentTypeId, 1)
            //        },
            //        PropertyGroups = new PropertyGroupCollection(new[]{
            //            new PropertyGroup(new PropertyTypeCollection(true, new[]{
            //                new PropertyType(mediaPickerDataType.Value) {
            //                    Alias = "vendrStoreLogo",
            //                    Name = "Store Logo",
            //                    Description = "A logo image for the store to appear at the top of the checkout screens and order emails.",
            //                    SortOrder = 10
            //                },
            //                new PropertyType(textstringDataType.Value) {
            //                    Alias = "vendrStoreAddress",
            //                    Name = "Store Address",
            //                    Description = "The address of the web store to appear in the footer of order emails.",
            //                    SortOrder = 20
            //                },
            //                new PropertyType(themeColorPickerDataType.Value) {
            //                    Alias = "vendrThemeColor",
            //                    Name = "Theme Color",
            //                    Description = "The theme color to use for colored elements of the checkout pages.",
            //                    SortOrder = 30
            //                },
            //                new PropertyType(booleanDataType.Value) {
            //                    Alias = "vendrCollectShippingInfo",
            //                    Name = "Collect Shipping Info",
            //                    Description = "Select whether to collect shipping information or not. Not necessary if you are only dealing with digital downloads.",
            //                    SortOrder = 40
            //                },
            //                new PropertyType(textstringDataType.Value) {
            //                    Alias = "vendrOrderLinePropertyAliases",
            //                    Name = "Order Line Property Aliases",
            //                    Description = "Comma separated list of order line property aliases to display in the order summary.",
            //                    SortOrder = 50
            //                },
            //                new PropertyType(textstringDataType.Value) {
            //                    Alias = "vendrPortalBackPage",
            //                    Name = "Portal Back Page",
            //                    Description = "The page to go back to when backing out of the checkout flow.",
            //                    SortOrder = 60
            //                },
            //                new PropertyType(contentPickerDataType.Value) {
            //                    Alias = "vendrTermsAndConditionsPage",
            //                    Name = "Terms and Conditions Page",
            //                    Description = "The page on the site containing the terms and conditions.",
            //                    SortOrder = 70
            //                },
            //                new PropertyType(contentPickerDataType.Value) {
            //                    Alias = "vendrPrivacyPolicyPage",
            //                    Name = "Privacy Policy Page",
            //                    Description = "The page on the site containing the privacy policy.",
            //                    SortOrder = 80
            //                },
            //                new PropertyType(booleanDataType.Value) {
            //                    Alias = "umbracoNaviHide",
            //                    Name = "Hide from Navigation",
            //                    Description = "Hide the checkout page from the sites main navigation.",
            //                    SortOrder = 90
            //                }
            //            })) {
            //                Name = "Settings",
            //                SortOrder = 50
            //            }
            //        })
            //    };

            //    Current.Services.ContentTypeService.Save(contentType);
            //}

            // Continue the pipeline
            return ctx;
        }
    }
}