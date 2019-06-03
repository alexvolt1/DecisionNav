using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models.ViewModels
{

    public class NavigationItem_ViewModel
    {
        public IEnumerable<NavigationItem_View> NavigationItem_View { get; set; }

    }
}
