using System.Dynamic;
using System.Linq;
using Infrastructure.Extensions;
using Licensing.Repository.Database;
using Licensing.Repository.Shared;

namespace Licensing.Repository.Management
{
    public class PeriodRepository : BaseRepository, IClientRepository
    {
        public PeriodRepository()
            : base(new LicenseCallCenterEntities())
        {
        }

        public PeriodRepository(LicenseCallCenterEntities dbConn)
            : base(dbConn)
        {
        }


        public ExpandoObject GetLicenseInfoByIdAndClientId(int id, int clId)
        {
            return DbConn.License.Where(e => e.IsObsolete == false && e.LicenseId == id && e.ClientId == clId)
                .Select(e => new
                {
                    e.LicenseId,
                    e.ClientId,
                    e.Name
                }).Single().ToExpando();
        }
    }
}
