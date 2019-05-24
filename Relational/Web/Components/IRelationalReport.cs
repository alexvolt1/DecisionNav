using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Components
{
    [Serializable]
   public class IRelationalReport
    {
        public string CacheKey { get; set; } 
        public System.Data.DataSet Members { get; set; }
        public string ReportName { get; set; }
        public string ReportColWidths { get; set; }
        public string ReportId { get; set; }
        public Dictionary<string, string> reportDetails { get; set; }
        public string Columns { get; set; }
        public string WhereClause { get; set; }


    }


    [Serializable]
    public class RelationalReport : IRelationalReport 
    {
        public System.Data.DataSet Data { get; set; }
        public Dictionary<string, decimal> ReportTotals { get; set; }
        public bool SubtotalAllowed { get; set; }
        public string SerializedData { get; set; }
        public System.Data.DataSet Filters { get; set; }
        public System.Data.DataSet FilterValues { get; set; }
    }
}
