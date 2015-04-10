using System;
using DeliveryReactiveLicensing.Service.License;
using Licensing.Model.Communication;
using Infrastructure.Json;

namespace Licensing.WebService.Proxy
{
    public class LicProxySvc : ILicProxySvc
    {
        private readonly ILicenseService _serviceLicense = new LicenseService();

        public string RequestActivation(string sRequest)
        {
            try
            {                
                return _serviceLicense.RequestActivation(sRequest);
            }
            catch (Exception)
            {
                return new ResponseConnection
                {
                    IsSuccess = false,
                    Message = "El código de activación no es correcto, por favor revise que la información sea correcta"
                }.SerializeAndEncrypt();
            }
        }

    }
}
