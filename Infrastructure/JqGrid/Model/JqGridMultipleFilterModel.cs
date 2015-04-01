using System.Collections.Generic;

namespace Infrastructure.JqGrid.Model
{
    public class JqGridMultipleFilterModel
    {
        public string groupOp { get; set; }
        public List<JqGridRulesModel> rules { get; set; }
    }
}

