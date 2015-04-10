using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Json;
using Licensing.Model.Communication;
using Licensing.Repository.Database;
using Licensing.Repository.Log;
using Licensing.Repository.Management;

namespace DeliveryReactiveLicensing.Service.License
{
    public class LicenseService : ILicenseService
    {
        public string[] GetInfoForActivationCode(int id)
        {
            using (var licenseRepository = new LicenseRepository())
            {
                var result = licenseRepository.GetActivationCodeById(id);
                return CreateActivactionCodeMsg(result);
            }
        }

        public string GenerateBody(string[] message)
        {
            var body = string.Format("<h3>Estimado {0}:</h3><br>Gracias por adquirir la licencia del sistema Advanced Delivery Management. Se anexa el código de activación para que comience a usar el sistema.<br><br>Código de activación:<br><strong>{1}</strong><br><br><hr><span style=\"font-family: Sans; font-size: 10px;\">La información contenida en este correo electrónico y cualquier documento adjunto está destinada sólo para el uso de la persona o entidad a quien va dirigida y puede contener información privilegiada, confidencial y de acceso restringido. Si usted no es el destinatario de este correo, o si este correo se ha dirigido a usted debido a un error, por favor avise inmediatamente al remitente por correo electrónico y luego borre este mensaje, así como los archivos adjuntos. Cualquier divulgación, distribución, reproducción parcial o completa, uso o cualquier otra acción relativa a este correo queda estrictamente prohibido si usted no es el destinatario. Gracias de antemano.<br></span><hr>",
                message[2], message[0]);
            return body;
        }

        public string RequestActivation(string sRequest)
        {
            var deviceConnModel = sRequest.DeserializeAndDecrypt<DeviceConnModel>();
            var actCodeModel = deviceConnModel.ActivationCode.DeserializeAndDecrypt<ActivationCodeModel>();
            var deviceConnResponse = new DeviceConnModel();

            SharedLogger.LogActivation(sRequest);

            using (var repository = new LicenseRepository())
            {
                var license = repository.GetLicenseIdByActivationCode(actCodeModel.ActivationCode);

                if (license == null)
                {
                    return new ResponseConnection
                    {
                        IsSuccess = false,
                        Message = "El código de activación no es correcto, por favor revise que la información sea correcta"
                    }.SerializeAndEncrypt();
                }

                var period = repository.GetLastPayedPeriod(license.LicenseId);
                if (period == null)
                {
                    return new ResponseConnection
                    {
                        IsSuccess = false,
                        Message = "No tiene una licencia activa para esta fecha"
                    }.SerializeAndEncrypt();
                }

                if(period.TimestampActivation == null)
                    repository.SaveActivationPeriod(period);

                foreach (var server in deviceConnModel.LstServers)
                {
                    string device;
                    try
                    {
                        device = ProcessServerDevice(server, repository, period);
                        if (String.IsNullOrWhiteSpace(device))
                            continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    deviceConnResponse.LstServers.Add(device);
                }

                foreach (var client in deviceConnModel.LstClients)
                {
                    string device;
                    try
                    {
                        device = ProcessClientDevice(client, repository, period);
                        if (String.IsNullOrWhiteSpace(device))
                            continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    deviceConnResponse.LstClients.Add(device);
                }
            }

            return new ResponseConnection
            {
                IsSuccess = true,
                Data = deviceConnResponse.SerializeAndEncrypt()

            }.SerializeAndEncrypt();
        }

        private string ProcessServerDevice(string server, LicenseRepository repository, LicensePeriod period)
        {
            var model = server.DeserializeAndDecrypt<ConnectionInfoModel>();
            var device = repository.GetServerInfo(model.Hn, period.LicensePeriodId);
            return device == null ? AddDeviceIfValid(model, period, repository, true) : UpdateDeviceIfValid(device, null, model, period, repository, true);
        }

        private string ProcessClientDevice(string client, LicenseRepository repository, LicensePeriod period)
        {
            var model = client.DeserializeAndDecrypt<ConnectionInfoModel>();
            var device = repository.GetClientInfo(model.Hn, period.LicensePeriodId);
            return device == null ? AddDeviceIfValid(model, period, repository, false) : UpdateDeviceIfValid(null, device, model, period, repository, false);
        }

        private string UpdateDeviceIfValid(ComputerServer server, ComputerClient client, 
            ConnectionInfoModel model, LicensePeriod period, LicenseRepository repository, bool bIsServer)
        {
            var lstHardware = bIsServer ? 
                server.ActivationLog.Deserialize<List<HardwareInfoModel>>() : client.ActivationLog.Deserialize<List<HardwareInfoModel>>();

            model.Code = AccountConstants.CODE_VALID;
            model.Et = period.EndDate;
            model.Iv = true;
            model.St = period.StartDate;

            if (lstHardware.Any(e => e.Hn == model.Hn && e.Hk == model.Hk))
                return model.SerializeAndEncrypt();

            if (lstHardware.Count > AccountConstants.MAX_CHANGE_HARDWAREINFO)
                return string.Empty;

            lstHardware.Add(new HardwareInfoModel { Hk = model.Hk, Hn = model.Hn });

            if (bIsServer)
                repository.UpdateServerInfo(server, lstHardware);
            else
                repository.UpdateClientInfo(client, lstHardware);
            
            return model.SerializeAndEncrypt();
        }

        private string AddDeviceIfValid(ConnectionInfoModel model, LicensePeriod period, LicenseRepository repository, bool bIsServer)
        {
            if (period.ServerNumber - repository.ServerCount(period.LicensePeriodId) <= 0)
                return null;

            model.Code = AccountConstants.CODE_VALID;
            model.Et = period.EndDate;
            model.Iv = true;
            model.St = period.StartDate;

            if (bIsServer)
                repository.AddServerInfo(model, period.LicensePeriodId);
            else
                repository.AddClientInfo(model, period.LicensePeriodId);

            return model.SerializeAndEncrypt();
        }


        private string[] CreateActivactionCodeMsg(string[] result)
        {
            var sb = new StringBuilder();

            if (result == null || result.Length != 3 || String.IsNullOrWhiteSpace(result[0]) ||
                String.IsNullOrWhiteSpace(result[1]))
            {
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("==============================================================================<br/>");
                sb.AppendLine("No existe código de activación:<br/>");
                sb.AppendLine("Revise la licencia, ya que al parecer está obsoleta<br/>");
                sb.AppendLine("==============================================================================<br/>");
                result = new[] { sb.ToString(), "NoActivationCode.txt", String.Empty, String.Empty };
            }
            else
            {
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("==============================================================================<br/>");
                sb.AppendLine("Empresa/Cliente:<br/>");
                sb.AppendLine(result[1]);
                sb.AppendLine("<br/>==============================================================================<br/>");
                sb.AppendLine("Código de activación:<br/>");
                sb.AppendLine(result[0]);
                sb.AppendLine("<br/>==============================================================================<br/>");
                sb.AppendLine("Por favor ingregse la información en el módulo de activación de su sistema<br/>");
                sb.AppendLine("==============================================================================<br/>");
                result[0] = sb.ToString();
                result = new[] { result[0], String.Format("CódigoActivación-{0}.txt", result[1]), result[1], result[2] };
            }

            return result;
        }
    }
}
