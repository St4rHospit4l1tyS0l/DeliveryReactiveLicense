using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Infrastructure.Extensions;
using Licensing.Model.Management;
using Licensing.Repository.Database;
using Licensing.Repository.Shared;

namespace Licensing.Repository.Management
{
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository()
            : base(new LicenseCallCenterEntities())
        {
        }

        public ClientRepository(LicenseCallCenterEntities dbConn)
            : base(dbConn)
        {
        }

        public ClientModel FindViewById(int id)
        {
            return DbConn.Client.Where(e => e.ClientId == id).Select(e => new ClientModel
            {
                CountryId = e.CountryId,
                Email = e.Email,
                FullAddress = e.FullAddress,
                Phone = e.Phone,
                ClientId = e.ClientId,
                Company = e.Company,
                FirstName = e.FirstName,
                LastName = e.LastName,
            }).FirstOrDefault();
        }

        public ExpandoObject GetClientInfoById(int clId)
        {
            return DbConn.Client.Where(e => e.ClientId == clId)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName, 
                    e.Email
                }).Single().ToExpando();
        }
    }
}
