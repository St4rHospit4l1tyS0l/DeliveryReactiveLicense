namespace DeliveryReactiveLicensing.Service.License
{
    public interface ILicenseService
    {
        string[] GetInfoForActivationCode(int id);
        string GenerateBody(string[] message);
        string RequestActivation(string sRequest);
    }
}