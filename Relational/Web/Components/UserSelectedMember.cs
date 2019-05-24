using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Relational.Components
{
    public class UserSelectedMember
    {
        public string parent { get; set; }
        public string value
        {
            get;
            set;
        }
        public string hid { get; set; }
        public string level { get; set; }
    }

    public class Memsel
    {
        public string parent { get; set; }
        public string value { get; set; }
        public string hid { get; set; }
        public string level { get; set; }
        public string fieldtype { get; set; }

        private string _rmin;
        public string rmin
        {
            get
            {
                if (_rmin == null || _rmin == "")
                {
                    return "0";
                }
                return _rmin;
            }
            set
            {
                _rmin = value;
            }
        }
       
        private string _rmax;
        public string rmax {
            get
            {
                if (_rmax == null || _rmax == "")
                {
                    return "0";
                }
                return _rmax;
            } 
            set
            {
                _rmax = value;
            }
        }
        public bool leaf { get; set; }
    }

    public class FilterDTO
    {
        public int id { get; set;}
        public List<string> ps { get; set; }
        public string pn { get; set; }
        public int hid { get; set; }
        public int level { get; set; }
       
        public List<Memsel> memsel { get; set; }
        public List<Memsel> prevFilters { get; set; }
    }

 
}