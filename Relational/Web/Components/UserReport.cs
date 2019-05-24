using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Relational.Components
{
    public class UserReport
    {
        public int ReportId { get; set; }
        public List<Memsel> SelectedMembers { get; set; }
        public List<Memsel> SelectedFilters { get; set; }
       
    }
}