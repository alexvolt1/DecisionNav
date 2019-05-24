using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Core
{
   public  interface IFilterValueItem
    {
       string Name { get; set; }

    }
   public class FilterValueItem : IFilterValueItem
    {


        public string Name { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public bool isSelected { get; set;} 
        public FilterValueItem() { }
        public FilterValueItem(string Range, string Value1, string Value2)
        {
            Name = Range;
            this.Value1 = Value1;
            this.Value2 = Value2;
        }

        public FilterValueItem(string Name)
        {
            this.Name = Name;
        }
    }
}
