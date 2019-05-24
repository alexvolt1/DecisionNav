using System;
using System.Collections.Generic;
//using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Relational.Core
{
    public sealed class ReportCache
    {
        private static volatile ReportCache instance = null;
        private static object instanceLock = new object();
        //private ConcurrentDictionary<string, RelationalReport> loadedReports = new ConcurrentDictionary<string, RelationalReport>();
        private Dictionary<string, RelationalReport> loadedReports = new Dictionary<string, RelationalReport>();
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


        public void AddReport(IRelationalReport report)
        {
            if (report == null)
                throw new ArgumentNullException("Cannot add report to cache");
            string key = report.CacheKey;
            this.loadedReports[key] = (RelationalReport)report;
        }
        public IRelationalReport GetReport(string cachekey)
        {
            RelationalReport rr = new RelationalReport();
            this.loadedReports.TryGetValue(cachekey, out rr);
            return rr;
        }

    }
}
