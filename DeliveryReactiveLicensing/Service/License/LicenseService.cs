using System;
using System.Text;
using Licensing.Repository.Management;

namespace DeliveryReactiveLicensing.Service.License
{
    public class LicenseService : ILicenseService
    {
        public string[] GetInfoForActivationCode(int id)
        {
            using (var licenseRepository = new ClientRepository())
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
