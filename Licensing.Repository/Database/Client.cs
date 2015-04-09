//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Licensing.Repository.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        public Client()
        {
            this.License = new HashSet<License>();
        }
    
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullAddress { get; set; }
        public int CountryId { get; set; }
        public System.DateTime InsDateTime { get; set; }
        public string InsUserId { get; set; }
        public Nullable<System.DateTime> UpdDateTime { get; set; }
        public string UpdUserId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<License> License { get; set; }
    }
}
