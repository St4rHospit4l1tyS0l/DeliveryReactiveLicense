using System.Collections.Generic;
using System.Linq;
using Licensing.Model.Shared;
using Licensing.Repository.Shared;

namespace Licensing.Repository.Catalog
{
    public class CountryRepository : BaseRepository
    {
        public List<ItemModel> FindAll()
        {
            return DbConn.Country.Select(e => new ItemModel
            {
                Key = e.CountryId,
                Name = e.Name
            }).ToList();
        }
    }
}
