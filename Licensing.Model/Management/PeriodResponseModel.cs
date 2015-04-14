using System;

namespace Licensing.Model.Management
{
    public class PeriodResponseModel
    {
        public int LicensePeriodBeforeId { get; set; }
        public int LicensePeriodId { get; set; }
        public string InsDateTime { get; set; }
        public string InsUserName { get; set; }
    }
}
