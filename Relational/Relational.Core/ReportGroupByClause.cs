using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relational.Core
{
     public class ReportGroupByClause
    {
         private string groupColumns;
         private string orderColumn;
         private IRelationalReport reportsession;
         private List<string> groupByCollection;
         public List<string> GroupByCollection
         {
             get { return groupByCollection; }
             set { groupByCollection = value; }
         }

         public ReportGroupByClause(IRelationalReport reportsession)
         {
             this.reportsession = reportsession;
             groupColumns = reportsession.reportDetails["ReportGroupBy"];
             orderColumn = reportsession.reportDetails["ReportSortColumns"];
             ParseReportGroupByMeta();
             //if (groupByCollection != null && groupByCollection.Count > 0 && !groupByCollection.Contains(orderColumn))
             //{
             //    groupByCollection.Insert(0, orderColumn.Trim('[', ']'));
             //}
            
         }
         public void SetSorting(string sortcolumn)
         {

                 ParseReportGroupByMeta();
                 //if (groupByCollection != null && groupByCollection.Count > 0 && !groupByCollection.Contains(sortcolumn))
                 //{
                 //    groupByCollection.Insert(0, sortcolumn.Trim('[', ']'));
                 //}
              
             
         }
         public string ToSql()
         {
             if (groupColumns!=null && groupColumns.Length>0 &&  groupByCollection != null && groupByCollection.Count>0)
             {
                 return " Group By " + "[" + String.Join("],[", groupByCollection) + "]";
             }
             else
             {
                 return "";
             }
         }

         private void ParseReportGroupByMeta()
         {
             groupByCollection = groupColumns.Trim('[', ']').Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToList();
         }

    }
}
