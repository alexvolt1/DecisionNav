using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models.BankVars
{
    public class Bank
    {
        [Key]
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string BankAlias { get; set; }
        public string DBServer { get; set; }
        public bool RMTools { get; set; }
        public bool HAT { get; set; }
        public bool Lockout_RMTools { get; set; }
        public char CustSearch { get; set; }
        public char ReportList { get; set; }
        public bool Lockout_RMWeekly { get; set; }
        public bool Lockout_RMMonthly { get; set; }
        public string DBServerDataNetwork { get; set; }
        public bool BatchProcessed { get; set; }
        public bool OnDemand { get; set; }

    }

}
