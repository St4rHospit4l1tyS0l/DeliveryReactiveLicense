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
    
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaims>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogins>();
            this.Client = new HashSet<Client>();
            this.Client1 = new HashSet<Client>();
            this.LicensePeriod = new HashSet<LicensePeriod>();
            this.LicensePeriod1 = new HashSet<LicensePeriod>();
            this.AspNetRoles = new HashSet<AspNetRoles>();
            this.License = new HashSet<License>();
            this.License1 = new HashSet<License>();
            this.License2 = new HashSet<License>();
        }
    
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<Client> Client { get; set; }
        public virtual ICollection<Client> Client1 { get; set; }
        public virtual ICollection<LicensePeriod> LicensePeriod { get; set; }
        public virtual ICollection<LicensePeriod> LicensePeriod1 { get; set; }
        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; }
        public virtual ICollection<License> License { get; set; }
        public virtual ICollection<License> License1 { get; set; }
        public virtual ICollection<License> License2 { get; set; }
    }
}
