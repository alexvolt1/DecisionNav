using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Relational.Core
{
   public interface IRelationalReport
    {
         string CacheKey { get; set; }

         string ReportName { get; set; }
         string ReportId { get; set; }
         string OriginalReportId { get; set; }
         string AlternateReport { get; set; }
         Dictionary<string, string> reportDetails {get; set;}
         ReportNumRecs NumRecs { get; set; }
         ReportColumns ReportColumns { get; set; }
         KeywordSearch KeywordSearch { get; set; }
         string ReportColWidths { get; set; }
         ReportVariance Variance { get; set; }
         ReportFiltersCollection ReportFiltersCollection { get; set; }
         ReportMembersCollection ReportMembersCollection { get; set; }
         ReportWhereClause ReportWhereClause { get; set; }
         ReportGroupByClause ReportGroupByClause { get; set; }
         ReportOrderByClause ReportOrderByClause { get; set; }
         Dictionary<string, string> sqlTableSchema { get; set; }
         DWCredentials Connect { get; set; }
         ReportHierarchies Hierarchies { get; set; }


         string ReportSQL { get; set; }
         string TotalsSQL { get; set; }
         string CountSQL { get; set; }
         string ReportTableSCHEMASQL { get; set; }
         int ReportRecordsCount { get; set; }
         




         string SQLDataSource { get; set; }
         
         string ColumnsJson { get; set; }
         string sqlOrderByString { get; set; }
         string sqlOrderByStringForPager { get; set; }
         string reportTableName { get; set; }
         string sqlfieldsString { get; set; }
         string sqlWhereString { get; set; }
        // string sqlgroupbyString { get; set; }
         string orderColumn { get; set; }
         string orderDir { get; set; }
         string ReportGroupBy { get; set; }

         string sortDir { get; set; }

         string sortColumn { get; set; }
         string ReportDescription { get; set; }
    }
}
