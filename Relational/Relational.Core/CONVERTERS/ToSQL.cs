using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Relational.Core.CONVERTERS
{
   internal class ToSQL
   {


#region DATA SELECT CONVERTERS
       internal static void BuildDataSQL(IRelationalReport reportsession)
       {
           BuildDataSQL(reportsession, null);
       }
       internal static void BuildDataSQL(IRelationalReport reportsession, IUserReport userreport)
       {
           if (userreport != null)
           {
               reportsession.ReportMembersCollection.ParseMembers(userreport.SelectedMembers);
               reportsession.ReportFiltersCollection.ParseFilters(userreport);
               reportsession.ReportMembersCollection.SearchStringsToSQL(userreport.SelectedSearchStrings);
           }

           string where = reportsession.ReportWhereClause.ToSql();
           string columns = reportsession.ReportColumns.ToSql();
           string sqlfromString = "FROM " + reportsession.reportTableName + " ";


#region DATA SQL TEMPLATE
           string sqltemplate = "SELECT "
               + columns
               + sqlfromString
               + where
               + reportsession.ReportGroupByClause.ToSql()
               + reportsession.ReportOrderByClause.ToSQL();
               reportsession.ReportSQL = sqltemplate;
               
                
#endregion

#region      COUNT AND TOTALS
               reportsession.ReportTableSCHEMASQL = " SELECT COLUMN_NAME,  DATA_TYPE" +
                                 " FROM INFORMATION_SCHEMA.COLUMNS " +
                                 " WHERE TABLE_NAME = N'" + reportsession.reportTableName + "' " +
                                 " and COLUMN_NAME in(" + reportsession.ReportColumns.ToSqlString() + ")";
               reportsession.sqlTableSchema = DATA.ReportData.GetTableSchema(reportsession.ReportTableSCHEMASQL, reportsession.Connect);
               List<string> decimalcolumns = (from a in reportsession.sqlTableSchema where a.Value == "decimal" select a.Key).ToList();


               reportsession.TotalsSQL = "select COUNT(1) as RecordCount "
                        + RendertotalSQLString(decimalcolumns)
                        + " from " + reportsession.reportTableName
                        + reportsession.ReportWhereClause.ToSql();
#endregion
       }

      



  

       #endregion


     #region     Generic CONVERTERS

        

       private static string RendertotalSQLString(List<string> decimalcolumns)
       {
           List<string> result = new List<string>();
           foreach (string s in decimalcolumns)
           {
               result.Add(String.Format(" SUM([{0}]) as '[{0}]'", s));
           }
           if (result.Count > 0)
           {
               return "," +  string.Join(",", result.ToArray());
           }
           return "";
       }
       #endregion











   }

}
