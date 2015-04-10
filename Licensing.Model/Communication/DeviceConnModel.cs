using System.Collections.Generic;

namespace Licensing.Model.Communication
{
    public class DeviceConnModel
    {
        public List<string> LstClients { get; set; }
        public List<string> LstServers { get; set; }
        public string ActivationCode { get; set; }

        public DeviceConnModel()
        {
            LstClients = new List<string>();
            LstServers = new List<string>();
        }
    }
}
