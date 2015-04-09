using System.Collections.Generic;
using System.Linq;
using Licensing.Model.Shared;
using Licensing.Repository.Shared;

namespace Licensing.Repository.Catalog
{
    public class CatPeriodRepository : BaseRepository
    {
        public List<ItemModel> FindAll()
        {
            return DbConn.CatPeriod.Select(e => new ItemModel
            {
                Key = e.CatPeriodId,
                Name = e.Name
            }).ToList();
        }
    }
}
