using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Infrastructure.Extensions;
using Infrastructure.Models;
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

        public dynamic GetPeriodsByLicenseId(int id)
        {
            var lstPeriods = DbConn.LicensePeriod.Where(e => e.LicenseId == id && e.IsObsolete == false)
                .Select(e => new
                {
                    e.LicensePeriodId,
                    e.LicenseId,
                    e.StartDate,
                    e.CatPeriodId,
                    e.EndDate,
                    e.ServerNumber,
                    e.ClientNumber,
                    e.IsPayed,
                    e.TimestampActivation,
                    e.InsDateTime,
                    InsUserName = e.AspNetUsers.UserName
                }).ToList();


            return lstPeriods.Select(e => new
            {
                e.LicensePeriodId,
                e.LicenseId,
                DateStartTx = e.StartDate.ToString(SharedConstants.DATE_FORMAT_A),
                e.CatPeriodId,
                DateEndTx = e.EndDate.ToString(SharedConstants.DATE_FORMAT_A),
                e.ServerNumber,
                e.ClientNumber,
                e.IsPayed,
                Activation = e.TimestampActivation.HasValue ? e.TimestampActivation.Value.ToString(SharedConstants.DATE_FORMAT_A) : SharedConstants.NOT_APPLICABLE,
                InsDateTime = e.InsDateTime.ToString(SharedConstants.DATE_FORMAT_A),
                e.InsUserName
            }).ToList();
        }
    }
}
