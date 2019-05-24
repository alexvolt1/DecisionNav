using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Core
{
  public class RelationalReport:IRelationalReport
    {
        public string CacheKey { get; set; }

        public string ReportName { get; set; }
        public string ReportColWidths { get; set; }
        public string ReportId { get; set; }
        public string OriginalReportId { get; set; }
        public string AlternateReport { get; set; }
        public string ReportGroupBy { get; set; }
        public Dictionary<string, string> reportDetails { get; set; }
        public ReportNumRecs NumRecs { get; set; }
        public ReportColumns  ReportColumns { get; set; }
        public KeywordSearch KeywordSearch { get; set; }
        public string SQLDataSource { get; set; }
        
       

        public ReportFiltersCollection ReportFiltersCollection { get; set; }
        public ReportMembersCollection ReportMembersCollection { get; set; }
        public ReportWhereClause ReportWhereClause { get; set; }
        public ReportGroupByClause ReportGroupByClause { get; set; }
        public ReportOrderByClause ReportOrderByClause { get; set; }
        public Dictionary<string, string> sqlTableSchema { get; set; }
        public ReportVariance Variance { get; set; }
        public DWCredentials Connect { get; set; }
        public ReportHierarchies Hierarchies { get; set; }

        public string ColumnsJson { get; set; }
       
        public string ReportSQL { get; set; }
        public string TotalsSQL { get; set; }
        public string CountSQL { get; set; }
        public string ReportTableSCHEMASQL { get; set; }
        public int ReportRecordsCount { get; set; }
        public string WhereClause { get; set; }

        public string sqlOrderByString { get; set; }
        public string sqlOrderByStringForPager { get; set; }
        public string orderColumn { get; set; }
        public string orderDir { get; set; }
        public string sortDir { get; set; }
        public string sortColumn { get; set; }
        public string reportTableName { get; set; }
        public string sqlfieldsString { get; set; }
        public string sqlWhereString { get; set; }
       // public string sqlgroupbyString { get; set; }



        public string TopicID { get; set; }
        public string ReportDescription { get; set; }

        public RelationalReport()
        {
            this.Connect = new DWCredentials();
        }

    }
    
}
