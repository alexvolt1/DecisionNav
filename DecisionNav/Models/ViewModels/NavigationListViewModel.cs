﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models.ViewModels
{
    public class NavigationListViewModel
    {
        public string Id { get; set; }
        public string ViewId { get; set; }
        public string DefaultName { get; set; }
        public string ImageUrl { get; set; }
        public IList<NavigationListViewModel> Children { get; set; }
    }
}
