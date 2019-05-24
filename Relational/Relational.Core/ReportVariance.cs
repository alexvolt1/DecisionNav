using System;

namespace Relational.Core
{
   public class ReportVariance
    {
     
       public string variance = "";
       IRelationalReport rr;
       public ReportVariance(IRelationalReport rr)
       {
           this.rr = rr;
        if (rr.reportDetails == null)
        throw new Exception("No Report Details available");
        variance = rr.reportDetails["ReportVariance"].Trim();
       // orderColumn = rr.ReportOrderByClause.OrderColumn.Trim();
       }
       public string ToSql()
       {
           string varianceSQL = "";
           string ordercolumn = rr.ReportOrderByClause.OrderColumn.Trim();
           if (variance != "" && ordercolumn != "")
              {
               int v = 0;
               if (int.TryParse(variance, out v))
               {
                   if (v > 0)
                   {
                       varianceSQL = ordercolumn + " > " + variance + " ";
                   }
                   if (v < 0)
                   {
                       varianceSQL = ordercolumn + " < " + variance + " ";
                   }
               }
               else
               {
                   varianceSQL = ordercolumn + " is not null ";
               }
           }
              return varianceSQL;
       }
    }
}
