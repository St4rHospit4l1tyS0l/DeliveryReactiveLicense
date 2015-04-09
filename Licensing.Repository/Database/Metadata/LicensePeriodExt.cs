using Licensing.Model.Management;

namespace Licensing.Repository.Database.Metadata
{
    public static class LicensePeriodExt
    {
        public static LicensePeriod ToModel(this PeriodModel model, int licenseId)
        {
            return new LicensePeriod
            {
                CatPeriodId = model.CatPeriodId,
                ClientNumber = model.ClientNumber,
                EndDate = model.DateEnd,
                LicenseId = licenseId,
                ServerNumber = model.ServerNumber,
                StartDate = model.DateStart
            };
        }
    }
}
