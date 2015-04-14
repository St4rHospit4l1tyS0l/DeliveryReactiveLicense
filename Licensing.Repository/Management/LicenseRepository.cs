using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Json;
using Licensing.Model.Communication;
using Licensing.Model.Management;
using Licensing.Repository.Database;
using Licensing.Repository.Shared;

namespace Licensing.Repository.Management
{
    public class LicenseRepository : BaseRepository, IClientRepository
    {
        public LicenseRepository()
            : base(new LicenseCallCenterEntities())
        {
        }

        public LicenseRepository(LicenseCallCenterEntities dbConn)
            : base(dbConn)
        {
        }

        public LicenseModel FindViewById(int id)
        {
            return DbConn.License.Where(e => e.LicenseId == id)
                .Select(e => new LicenseModel
            {
                LicenseId = e.LicenseId,
                ClientId = e.ClientId,
                IsActive = e.IsActive,
                IsObsolete = e.IsObsolete,
                Name = e.Name
            }).FirstOrDefault();
        }

        public string[] GetActivationCodeById(int id)
        {
            var firstOrDefault = DbConn.License.Where(e => e.LicenseId == id && e.IsObsolete == false)
                .Select(e => new List<string>{ e.ActivationCode, e.Client.FirstName + " " + e.Client.LastName, e.Client.Email}).FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.ToArray() : null;
        }

        public License GetLicenseIdByActivationCode(string activationCode)
        {
            return DbConn.License.FirstOrDefault(e => e.ActivationCode == activationCode && e.IsObsolete == false && e.IsActive);
        }

        public LicensePeriod GetLastPayedPeriod(int licenseId)
        {
            var now = DateTime.Now;
            return DbConn.LicensePeriod.Where(e => e.LicenseId == licenseId 
                && e.IsObsolete == false && e.IsPayed 
                && now >= e.StartDate && now <= e.EndDate)
                    .OrderByDescending(e => e.EndDate)
                    .FirstOrDefault();
        }

        public ComputerServer GetServerInfo(string hn, int licensePeriodId)
        {
            return DbConn.ComputerServer.FirstOrDefault(e => e.ServerName == hn && e.LicensePeriodId == licensePeriodId);
        }

        public int ServerCount(int licensePeriodId)
        {
            return DbConn.ComputerServer.Count(e => e.LicensePeriodId == licensePeriodId);
        }

        public decimal ClientCount(int licensePeriodId)
        {
            return DbConn.ComputerClient.Count(e => e.LicensePeriodId == licensePeriodId);
        }

        public void AddServerInfo(ConnectionInfoModel model, int licensePeriodId)
        {
            DbConn.ComputerServer.Add(new ComputerServer
            {
                HardwareId = model.Serialize(),
                IsObsolete = false,
                LicensePeriodId = licensePeriodId,
                ServerName = model.Hn,
                ActivationLog = (new List<HardwareInfoModel> {new HardwareInfoModel{Hk = model.Hk, Hn = model.Hn}}).Serialize()
            });

            DbConn.SaveChanges();
        }

        public void UpdateServerInfo(ComputerServer device, List<HardwareInfoModel> activationLog)
        {
            device.ActivationLog = activationLog.Serialize();
            DbConn.SaveChanges();
        }

        public ComputerClient GetClientInfo(string hn, int licensePeriodId)
        {
            return DbConn.ComputerClient.FirstOrDefault(e => e.ClientHost == hn && e.LicensePeriodId == licensePeriodId);
        }

        public void UpdateClientInfo(ComputerClient client, List<HardwareInfoModel> activationLog)
        {
            client.ActivationLog = activationLog.Serialize();
            DbConn.SaveChanges();
        }

        public void AddClientInfo(ConnectionInfoModel model, int licensePeriodId)
        {
            DbConn.ComputerServer.Add(new ComputerServer
            {
                HardwareId = model.Serialize(),
                IsObsolete = false,
                LicensePeriodId = licensePeriodId,
                ServerName = model.Hn,
                ActivationLog = (new List<HardwareInfoModel> { new HardwareInfoModel { Hk = model.Hk, Hn = model.Hn } }).Serialize()
            });
            DbConn.SaveChanges();
        }

        public void SaveActivationPeriod(LicensePeriod period)
        {
            period.TimestampActivation = DateTime.Now;
            DbConn.SaveChanges();
        }

    }
}
