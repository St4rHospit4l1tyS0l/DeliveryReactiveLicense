using System.Collections.Generic;
using System.Linq;
using Licensing.Model.Management;
using Licensing.Repository.Database;
using Licensing.Repository.Shared;

namespace Licensing.Repository.Management
{
    public class LicenseRepository : BaseRepository, IClientRepository
    {
        public LicenseRepository()
            : base(new LicenseCallCenterEntities())
        {
        }

        public LicenseRepository(LicenseCallCenterEntities dbConn)
            : base(dbConn)
        {
        }

        public LicenseModel FindViewById(int id)
        {
            return DbConn.License.Where(e => e.LicenseId == id)
                .Select(e => new LicenseModel
            {
                LicenseId = e.LicenseId,
                ClientId = e.ClientId,
                IsActive = e.IsActive,
                IsObsolete = e.IsObsolete,
                Name = e.Name
            }).FirstOrDefault();
        }

        public string[] GetActivationCodeById(int id)
        {
            var firstOrDefault = DbConn.License.Where(e => e.LicenseId == id && e.IsObsolete == false)
                .Select(e => new List<string>{ e.ActivationCode, e.Client.FirstName + " " + e.Client.LastName, e.Client.Email}).FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.ToArray() : null;
        }
    }
}
