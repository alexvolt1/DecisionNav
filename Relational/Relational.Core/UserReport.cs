using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Core
{
    public class UserReport : Relational.Core.IUserReport
    {
        public string TopicId { get; set; }
        public int ReportId { get; set; }
        public bool UseAlternateReport { get; set; }
        public List<Memsel> SelectedMembers { get; set; }
        public List<Memsel> SelectedFilters { get; set; }
        public List<Memsel> SelectedSearchStrings { get; set; }
        public int Level { get; set; }
        public string Keyword { get; set; }
        public int Qid { get; set; }
        
    }
}
