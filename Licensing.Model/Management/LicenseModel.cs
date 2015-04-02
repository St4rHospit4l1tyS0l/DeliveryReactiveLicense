using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Models;

namespace Licensing.Model.Management
{
    public class LicenseModel
    {

        public int LicenseId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int ClientId { get; set; }

        public bool IsActive { get; set; }

        
        public bool IsObsolete { get; set; }

        public DateTime InsDateTime { get; set; }
        public string InsUserName { get; set; }

        public string IsActiveTx
        {
            get
            {
                return IsActive ? "SI" : "NO";
            }
        }
        public bool IsActiveIn {
            get
            {
                return String.IsNullOrWhiteSpace(IsActiveInTx) == false && IsActiveInTx.Trim().ToLower() == "on";
            }
        }
        
        public string IsActiveInTx { get; set; }

        public string InsDtTx
        {
            get
            {
                return InsDateTime.ToString(SharedConstants.DATE_TIME_FORMAT);
            }
        }
    }
}
