﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LicenseCallCenterEntities : DbContext
    {
        public LicenseCallCenterEntities()
            : base("name=LicenseCallCenterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<CatPeriod> CatPeriod { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<LicensePeriod> LicensePeriod { get; set; }
        public virtual DbSet<ViewClientGrid> ViewClientGrid { get; set; }
        public virtual DbSet<ViewLicenseGrid> ViewLicenseGrid { get; set; }
        public virtual DbSet<ComputerClient> ComputerClient { get; set; }
        public virtual DbSet<ComputerServer> ComputerServer { get; set; }
        public virtual DbSet<License> License { get; set; }
    }
}
