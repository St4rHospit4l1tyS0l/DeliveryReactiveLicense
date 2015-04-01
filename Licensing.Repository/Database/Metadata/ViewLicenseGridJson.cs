using System;
using Infrastructure.Extensions;
using Licensing.Model.Management;

namespace Licensing.Repository.Database.Metadata
{
    public static class ViewLicenseGridJson
    {
        public static ViewLicenseGrid ModelEnt;
        public static string Key = ModelEnt.PropertyName(e => e.LicenseId);

        public static string Columns = String.Join(",", new[]
        {
            ModelEnt.PropertyName(e => e.LicenseId),
            ModelEnt.PropertyName(e => e.Name),
            ModelEnt.PropertyName(e => e.IsActive),
            ModelEnt.PropertyName(e => e.IsObsolete),
            ModelEnt.PropertyName(e => e.InsDateTime),
            ModelEnt.PropertyName(e => e.UserName)
        });

        public static LicenseModel DynamicToDto(dynamic data)
        {
            var model = new LicenseModel
            {
                LicenseId = data.LicenseId,
                ClientId = data.ClientId,
                IsActive = data.IsActive,
                IsObsolete = data.IsObsolete,
                Name = data.Name
            };

            return model;

        }
    }
}
