using System;
using System.ComponentModel.DataAnnotations;

namespace Licensing.Model.Management
{
    public class ClientModel
    {

        public int ClientId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Company { get; set; }

        [Required]
        [MaxLength(1000)]
        [EmailAddress]
        public string Email { get; set; }
        
        public int CountryId { get; set; }
        
        public string CountryName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FullAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        public string FullName {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

    }
}
