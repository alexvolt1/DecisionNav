using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models.ViewModels
{
    public class NavigationObjectsModel
    {
        public IList<NavigationItem_View> NavItemView { get; set; }

        public IList<NavigationList> NavList { get; set; }
    }
}
