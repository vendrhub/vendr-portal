using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace Vendr.Portal.Pipeline.Implement.Tasks
{
    internal class CreateVendrPortalDataTypesTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            // Store Picker
            if (Current.Services.DataTypeService.GetDataType(VendrPortalConstants.DataTypes.Guids.StorePickerGuid) == null)
            {
                if (Current.PropertyEditors.TryGet("Vendr.StorePicker", out IDataEditor editor))
                {
                    var dataType = new DataType(editor)
                    {
                        Key = VendrPortalConstants.DataTypes.Guids.StorePickerGuid,
                        Name = "[Vendr Portal] Store Picker",
                        DatabaseType = ValueStorageType.Nvarchar
                    };

                    Current.Services.DataTypeService.Save(dataType);
                }
            }

            // Email Template Picker
            if (Current.Services.DataTypeService.GetDataType(VendrPortalConstants.DataTypes.Guids.EmailTemplatePickerGuid) == null)
            {
                if (Current.PropertyEditors.TryGet("Vendr.StoreEntityPicker", out IDataEditor editor))
                {
                    var dataType = new DataType(editor)
                    {
                        Key = VendrPortalConstants.DataTypes.Guids.EmailTemplatePickerGuid,
                        Name = "[Vendr Portal] Email Template Picker",
                        DatabaseType = ValueStorageType.Nvarchar,
                        Configuration = new Dictionary<string, object>
                        {
                            { "entityType", "EmailTemplate" }
                        }
                    };

                    Current.Services.DataTypeService.Save(dataType);
                }
            }

            // Theme Color Picker
            if (Current.Services.DataTypeService.GetDataType(VendrPortalConstants.DataTypes.Guids.ThemeColorPickerGuid) == null)
            {
                if (Current.PropertyEditors.TryGet(Constants.PropertyEditors.Aliases.ColorPicker, out IDataEditor editor))
                {
                    var dataType = new DataType(editor)
                    {
                        Key = VendrPortalConstants.DataTypes.Guids.ThemeColorPickerGuid,
                        Name = "[Vendr Portal] Theme Color Picker",
                        DatabaseType = ValueStorageType.Nvarchar,
                        Configuration = new ColorPickerConfiguration
                        {
                            Items = VendrPortalConstants.ColorMap.Select((kvp, idx) => new ValueListConfiguration.ValueListItem
                            {
                                Id = idx,
                                Value = "{\"value\":\"" + kvp.Key + "\", \"label\":\"" + kvp.Value + "\"}"
                            }).ToList(),
                            UseLabel = false
                        }
                    };

                    Current.Services.DataTypeService.Save(dataType);
                }
            }

            // Continue the pipeline
            return ctx;
        }
    }
}