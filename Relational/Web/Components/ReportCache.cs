using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Relational.Components
{
    public sealed class ReportCache
    {

        private static volatile ReportCache instance = null;
        private static object instanceLock = new object();
        public static ReportCache Instance
        {
            get
            {
                if (ReportCache.instance == null)
                {
                    lock (ReportCache.instanceLock)
                    {
                        if (ReportCache.instance == null)
                            ReportCache.instance = new ReportCache();
                    }
                }
                return ReportCache.instance;
            }
        }

        private ReportCache()
        {

        }
        public void AddReport(IRelationalReport report)
        {
            string key = report.CacheKey;
            if (report == null)
                throw new ArgumentNullException("Cannot add report to cache");
            HttpContext.Current.Cache.Insert(key, report, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
          
        }
        public IRelationalReport GetReport(string cachekey)
        {
            if (HttpContext.Current.Cache[cachekey] == null)
            {
                return null;
            }
            return (IRelationalReport)HttpContext.Current.Cache[cachekey];
          
        }

    }
}