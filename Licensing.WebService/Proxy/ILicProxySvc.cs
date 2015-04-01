using System.ServiceModel;

namespace Licensing.WebService.Proxy
{
    [ServiceContract]
    public interface ILicProxySvc
    {
        [OperationContract]
        string RequestActivation(string sRequest);
    }
}
