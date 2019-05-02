using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models.NavBarModel
{
    public class NavBarModel
    {
        public IEnumerable<AvailWeek> AvailWeek { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
        public IEnumerable<Coupons> Coupons { get; set; }

        public string StatusMessage { get; set; }

        public string CurrentWeekName { get; set; }

    }
}
