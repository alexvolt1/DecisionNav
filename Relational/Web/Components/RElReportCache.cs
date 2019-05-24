using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Relational.Components
{
    public class RElReportCache
    {
        public static RelationalReport RelReportObject
        {
            get
            {
                if (HttpContext.Current.Cache["RelReportObject"] == null)
                {
                    RelReportObject = new RelationalReport();
                    return RelReportObject;
                }
                return (RelationalReport)HttpContext.Current.Cache["RelReportObject"];
            }
            set
            {
                HttpContext.Current.Cache.Insert("RelReportObject", value, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
        public static DataTable RelReportData
        {
            get
            {
                if (HttpContext.Current.Cache["RelReportData"] == null)
                {
                    return null;
                }
                return (DataTable)HttpContext.Current.Cache["RelReportData"];
            }
            set
            {
                HttpContext.Current.Cache.Insert("RelReportData", value, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
        public static DataTable RelReportMembers
        {
            get
            {
                if (HttpContext.Current.Cache["RelReportMembers"] == null)
                {
                    return null;
                }
                return (DataTable)HttpContext.Current.Cache["RelReportMembers"];
            }
            set
            {
                HttpContext.Current.Cache.Insert("RelReportMembers", value, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
    }
}