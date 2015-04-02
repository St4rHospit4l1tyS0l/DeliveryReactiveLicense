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
            ModelEnt.PropertyName(e => e.ClientId),
            ModelEnt.PropertyName(e => e.Name),
            ModelEnt.PropertyName(e => e.IsActive),
            ModelEnt.PropertyName(e => e.IsObsolete),
            ModelEnt.PropertyName(e => e.InsDateTime),
            ModelEnt.PropertyName(e => e.InsUserName)
        });

        public static LicenseModel DynamicToDto(dynamic data)
        {
            var model = new LicenseModel
            {
                LicenseId = data.LicenseId,
                ClientId = data.ClientId,
                IsActive = data.IsActive,
                IsObsolete = data.IsObsolete,
                Name = data.Name,
                InsDateTime = data.InsDateTime,
                InsUserName = data.InsUserName
            };

            return model;
        }


        public static License ToDataModel(this LicenseModel model)
        {
            var data = new License
            {
                LicenseId = model.LicenseId,
                ClientId = model.ClientId,
                IsActive = model.IsActiveIn,
                Name = model.Name,
            };

            return data;
        }

        public static License UpdateModel(this LicenseModel model, License oldModel, string userId)
        {
            oldModel.Name = model.Name;
            oldModel.IsActive = model.IsActiveIn;
            oldModel.UpdDateTime = DateTime.Now;
            oldModel.UpdUserId = userId;
            return oldModel;
        }


    }
}
