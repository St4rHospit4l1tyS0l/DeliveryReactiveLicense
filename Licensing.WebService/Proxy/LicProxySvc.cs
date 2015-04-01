using System;

namespace Licensing.WebService.Proxy
{
    public class LicProxySvc : ILicProxySvc
    {
        public string RequestActivation(string sRequest)
        {
            try
            {
                return String.Empty;

            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
