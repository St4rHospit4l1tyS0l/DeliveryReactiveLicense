using System;
using System.Web;
using System.Web.Script.Serialization;
using Infrastructure.Connection;
using Licensing.Model.Communication;
using Licensing.Repository.Database;
using Licensing.Repository.Resources;
using log4net;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Licensing.Repository.Log
{
    public class SharedLogger
    {


        public static void LogError(Exception ex, params object[] arrVal)
        {
            try
            {
                dynamic username = GetUser();
                dynamic modelExcep = new ExceptionLog();

                modelExcep.MsgException = ex.Message;
                modelExcep.ExceptionLogUid = Guid.NewGuid();
                modelExcep.InnerException = GetInnerExceptions(ex);
                modelExcep.ParamsValues = GetSerializedValues(arrVal);
                modelExcep.StackTrace = ex.StackTrace;
                modelExcep.Timestamp = DateTime.Now;
                modelExcep.Username = username;
                SaveLogToDb(modelExcep);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void LogActivation(string activationInfo)
        {
            try
            {
                using (var dbConn = new LogLicenseEntities())
                {
                    dbConn.ActivationLog.Add(new ActivationLog
                    {
                        ActivationInfo = activationInfo + AccountConstants.SEPARATOR_INFO + IpAddress.GetIp()
                    });
                    dbConn.SaveChanges();
                }
            }
            catch (Exception)
            {
                SaveLogToFile(activationInfo);
            }
        }

        private static void SaveLogToDb(ExceptionLog modelExcep)
        {
            try
            {
                using (var dbConn = new LogLicenseEntities())
                {
                    dbConn.ExceptionLog.Add(modelExcep);
                    dbConn.SaveChanges();
                }
            }
            catch (Exception)
            {
                SaveLogToFile(modelExcep);
            }
        }

        private static void SaveLogToFile(ExceptionLog exceptionLog)
        {
            try
            {
                LogManager.GetLogger("ERROR_LOGGER").Fatal(GetSerializedValue(exceptionLog));
            }
            catch (Exception)
            {
                return;
            }
        }


        private static void SaveLogToFile(String activationInfo)
        {
            try
            {
                LogManager.GetLogger("INFO_ACTIVATION").Fatal(activationInfo);
            }
            catch (Exception)
            {
                return;
            }
        }

        private static string GetSerializedValue(ExceptionLog exceptionLog)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(exceptionLog);
            }
            catch (Exception)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetSerializedValue");
            }
        }


        private static string GetSerializedValues(object[] arrVal)
        {
            try
            {
                dynamic serializer = new JavaScriptSerializer();
                return serializer.Serialize(arrVal);
            }
            catch (Exception)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetSerializedValues");
            }
        }

        private static string GetInnerExceptions(Exception exception)
        {
            try
            {
                if (exception.InnerException == null)
                {
                    return "|EOE|";
                }
                return string.Format("|{0}|{1}", exception.InnerException.Message, GetInnerExceptions(exception.InnerException));
            }
            catch (Exception)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetInnerExceptions");
            }
        }

        private static string GetUser()
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.User.Identity.Name;

            }
            catch (Exception)
            {
                return ResShared.ERROR_NOUSER_FROMCONTEXT;
            }
        }
    }
}

