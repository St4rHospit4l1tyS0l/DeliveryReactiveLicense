using System;
using System.Globalization;
using Infrastructure.Models;

namespace Licensing.Model.Management
{
    public class PeriodModel
    {
        public int LicensePeriodId { get; set; }
        public string DateStartTx { get; set; }
        public string DateEndTx { get; set; }
        public int ServerNumber { get; set; }
        public int ClientNumber { get; set; }
        public string Activation { get; set; }
        public string InsDateTime { get; set; }
        public string InsUserName { get; set; }
        public int CatPeriodId { get; set; }
        public DateTime DateEnd
        {
            get
            {
                return DateTime.ParseExact(DateEndTx, SharedConstants.DATE_FORMAT_A, CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
            }
        }

        public DateTime DateStart
        {
            get
            {
                return DateTime.ParseExact(DateStartTx, SharedConstants.DATE_FORMAT_A, CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
            }
        }
    }
}