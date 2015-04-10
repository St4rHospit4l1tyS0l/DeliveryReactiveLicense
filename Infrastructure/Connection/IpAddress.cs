using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Infrastructure.Connection
{
    public static class IpAddress
    {
        public static string GetIp()
        {
            try
            {
                var context = OperationContext.Current;
                var prop = context.IncomingMessageProperties;
                var endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                if (endpoint != null)
                {
                    return endpoint.Address;
                }
                return "ND";

            }
            catch (Exception)
            {
                return "ND";
            }
        }
    }
}
