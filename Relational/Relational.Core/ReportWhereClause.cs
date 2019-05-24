using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Core
{
    public class ReportWhereClause
    {
        IRelationalReport rr;
        public string WhereClause = "";
        public ReportWhereClause(IRelationalReport reportsession)
        {
            rr = reportsession;
            if (rr.reportDetails == null)
                throw new Exception("No Report Details available");
                WhereClause = rr.reportDetails["ReportWhereStr"];
        }

        public string ToSql()
        {
            string sql = "";
            string variance = "";
            string memSQL = "";
            string filtSQL = "";
            string searchsql = "";
            variance = rr.Variance.ToSql();
            memSQL = rr.ReportMembersCollection.ToSql();
            filtSQL = rr.ReportFiltersCollection.ToSql();
            searchsql =rr.ReportMembersCollection.SearchStrings;
            //if (searchsql != "")
            //{
            //    string[] s = new string[] { searchsql, filtSQL }; //VARIANCE IS DISABLED for release 0
            //    //string[] s = new string[] {variance, searchsql, filtSQL};
            //    sql = string.Join(" AND ", s.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            //}
            //else
            //{
            //    string[] s = new string[] {WhereClause, memSQL, filtSQL }; //VARIANCE IS DISABLED for release 0
            //    //string[] s = new string[] {variance, memSQL, filtSQL};
            //    sql = string.Join(" AND ", s.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            //}
     
                string[] s = new string[] {searchsql, WhereClause, memSQL, filtSQL }; //VARIANCE IS DISABLED for release 0
                //string[] s = new string[] {variance, memSQL, filtSQL};
                sql = string.Join(" AND ", s.Where(x => !string.IsNullOrEmpty(x)).ToArray());
            
            return  sql == "" ? "" : " WHERE " + sql;
        }
    }
}
