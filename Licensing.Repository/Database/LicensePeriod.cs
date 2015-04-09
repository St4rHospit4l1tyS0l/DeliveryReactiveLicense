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
    
    public partial class LicensePeriod
    {
        public LicensePeriod()
        {
            this.ComputerClient = new HashSet<ComputerClient>();
            this.ComputerServer = new HashSet<ComputerServer>();
        }
    
        public int LicensePeriodId { get; set; }
        public int LicenseId { get; set; }
        public System.DateTime StartDate { get; set; }
        public int CatPeriodId { get; set; }
        public System.DateTime EndDate { get; set; }
        public int ServerNumber { get; set; }
        public int ClientNumber { get; set; }
        public bool IsPayed { get; set; }
        public bool IsObsolete { get; set; }
        public Nullable<System.DateTime> TimestampActivation { get; set; }
        public System.DateTime InsDateTime { get; set; }
        public string InsUserId { get; set; }
        public Nullable<System.DateTime> DelDateTime { get; set; }
        public string DelUserId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual CatPeriod CatPeriod { get; set; }
        public virtual ICollection<ComputerClient> ComputerClient { get; set; }
        public virtual ICollection<ComputerServer> ComputerServer { get; set; }
        public virtual License License { get; set; }
    }
}
