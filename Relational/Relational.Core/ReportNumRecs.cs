using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Core
{
    
    public class ReportNumRecs
    {
        public string numRecs = "";
        public ReportNumRecs(IRelationalReport rr)
        {
            if (rr.reportDetails == null)
                throw new Exception("No Report Details available");
           this.numRecs = rr.reportDetails["ReportNumRecs"];
        }
        public string ToSql()
        {
            if (this.numRecs != "")
            {
                return "TOP " + this.numRecs + " ";
            }
            return "";
        }
    }
}
