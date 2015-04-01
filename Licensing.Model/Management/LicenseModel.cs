using System.ComponentModel.DataAnnotations;

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

    }
}
