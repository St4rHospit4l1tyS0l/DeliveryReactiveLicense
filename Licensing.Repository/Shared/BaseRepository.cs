using System;
using Licensing.Repository.Database;
using Licensing.Repository.Log;

namespace Licensing.Repository.Shared
{
    public abstract class BaseRepository : IDisposable
    {
        public LicenseCallCenterEntities DbConn;

        protected BaseRepository()
        {
            DbConn = new LicenseCallCenterEntities();
        }

        protected BaseRepository(LicenseCallCenterEntities dbConn)
        {
            DbConn = dbConn;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposing)
                    return;

                if (DbConn != null)
                    DbConn.Dispose();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
            finally
            {
                DbConn = null;
            }
        }

    }
}
