using System;
using Infrastructure.Extensions;
using Licensing.Model.Management;

namespace Licensing.Repository.Database.Metadata
{
    public static class ViewClientGridJson
    {
        public static ViewClientGrid ModelEnt;
        public static string Key = ModelEnt.PropertyName(e => e.ClientId);

        public static string Columns = String.Join(",", new[]
        {
            ModelEnt.PropertyName(e => e.ClientId),
            ModelEnt.PropertyName(e => e.FirstName),
            ModelEnt.PropertyName(e => e.LastName),
            ModelEnt.PropertyName(e => e.Company),
            ModelEnt.PropertyName(e => e.Email),
            ModelEnt.PropertyName(e => e.CountryName),
            ModelEnt.PropertyName(e => e.FullAddress),
            ModelEnt.PropertyName(e => e.Phone)
        });


        public static Client ToDataModel(this ClientModel model)
        {
            var data = new Client
            {
                ClientId = model.ClientId,
                CountryId = model.CountryId,
                Email = model.Email,
                FullAddress = model.FullAddress,
                Phone = model.Phone,
                Company = model.Company,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            return data;
        }

        public static ClientModel ToDto(Client data)
        {
            var model = new ClientModel
            {
                ClientId = data.ClientId,
                Email = data.Email,
                Phone = data.Phone,
                Company = data.Company,
                CountryId = data.CountryId,
                FirstName = data.FirstName,
                LastName = data.LastName,
                FullAddress = data.FullAddress
            };

            return model;

        }
        /*
        public static ClientModel ToDto(LicenseDto data)
        {
            var model = new ClientModel
            {
                ClientNumbers = data.ClientNumber,
                CompanyClient = data.CompanyUser,
                CountryId = data.CountryId ?? EntityConstants.NULL_VALUE,
                Email = data.Email,
                EndDateTx = data.EndDate.ToString(SharedConstants.DATE_FORMAT),
                Address = data.FullAddress,
                IsActive = data.IsActive ? "true" : "false",
                LicenseId = data.LicenseId,
                Phone = data.Phone,
                ServerNumbers = data.ServerNumber,
                StartDateTx = data.StartDate.ToString(SharedConstants.DATE_FORMAT)
            };

            return model;

        }
        */
        public static ClientModel DynamicToDto(dynamic data)
        {
            var model = new ClientModel
            {
                ClientId = data.ClientId,
                Email = data.Email,
                Phone = data.Phone,
                Company = data.Company,
                CountryName = data.CountryName,
                FirstName = data.FirstName,
                LastName = data.LastName,
                FullAddress = data.FullAddress
            };

            return model;

        }

        public static Client UpdateModel(this ClientModel model, Client oldModel, string userId)
        {
            oldModel.CountryId = model.CountryId;
            oldModel.Email = model.Email;
            oldModel.Company = model.Company;
            oldModel.FirstName = model.FirstName;
            oldModel.LastName = model.LastName;
            oldModel.Phone = model.Phone;
            oldModel.FullAddress = model.FullAddress;
            oldModel.UpdDateTime = DateTime.Now;
            oldModel.UpdUserId = userId;
            return oldModel;
        }

    }
}
