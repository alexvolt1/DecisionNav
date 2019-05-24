using System;
using Relational.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace RelationalTests
{
    [TestClass]
    public class RelationalTests
    {
        [TestMethod]
        public void GetFilters()
        {
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 101,
            };
            RelationalReport rr = (RelationalReport)GetReport("101");
            ReportFactory.ApplyUserSelection(rr, userreport);
            object o = rr.ReportFiltersCollection.ToJson();
        }


        [TestMethod]
        public void TestExtraFields11()
        {

            RelationalReport rr = (RelationalReport)GetReport2("11");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 11,
            };
            //rr.ReportFiltersCollection.isDebugging = true;
            //rr.ReportFiltersCollection.isVerbose = true;
            ReportFactory.ApplyUserSelection(rr, userreport);
            object b = Relational.Core.DATA.ReportData.GetDataSerialized(rr, 1, 10);
           
        }
        [TestMethod]
        public void TestExtraFields12()
        {

            RelationalReport rr = (RelationalReport)GetReport2("12");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 12,
            };
            //rr.ReportFiltersCollection.isDebugging = true;
            //rr.ReportFiltersCollection.isVerbose = true;
            ReportFactory.ApplyUserSelection(rr, userreport);
            object b = Relational.Core.DATA.ReportData.GetDataSerialized(rr, 1, 10);

        }



        [TestMethod]
        public void TestWhereClause()
        {

            RelationalReport rr = (RelationalReport)GetReport2("19");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 19,
            };
            ReportFactory.ApplyUserSelection(rr, userreport);
            object o = rr.ReportFiltersCollection.ToJson();
        }



        [TestMethod]
        public void TestAlternateOrderByandGroupBy7()
        {
            RelationalReport rr = (RelationalReport)GetReport2("7");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 7,
            };
            ReportFactory.ApplyUserSelection(rr, userreport);

            object a = Relational.Core.DATA.ReportData.GetDataSerialized2(rr, 1, 10);
            string a1 = (string)a;


            userreport.UseAlternateReport = true;
            ReportFactory.ApplyUserSelection(rr, userreport);

            object b = Relational.Core.DATA.ReportData.GetDataSerialized2(rr, 1, 10);
            string b1 = (string)b;
        }




        [TestMethod]
        public void TestAlternateOrderByandGroupBy107()
        {
            RelationalReport rr = (RelationalReport)GetReport2("107");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 107,
            };
            ReportFactory.ApplyUserSelection(rr, userreport);

            string o;
            string g;

            //Default
            o = rr.ReportOrderByClause.ToSQL();
            g = rr.ReportGroupByClause.ToSql();
            Assert.AreEqual(" ORDER BY [Facility Exposure, End] DESC ", o);
            Assert.AreEqual(" Group By [Facility Exposure, End],[Relationship Number],[Accrual Date]", g);

            ////Sort on default column
            //rr.ReportOrderByClause.SetSorting(rr, "Facility Exposure, End", "0");
            //o = rr.ReportOrderByClause.ToSQL();
            //g = rr.ReportGroupByClause.ToSql();
            //Assert.AreEqual(" ORDER BY [Facility Exposure, End] DESC ", o);
            //Assert.AreEqual(" Group By [Relationship Number],[Accrual Date]", g);

            ////Sort by other column ASC
            //rr.ReportOrderByClause.SetSorting(rr, "Change in Facility Exposure", "1");
            //rr.ReportGroupByClause.SetSorting("Change in Facility Exposure");
            //o = rr.ReportOrderByClause.ToSQL();
            //g = rr.ReportGroupByClause.ToSql();
            //// Assert.AreEqual(" ORDER BY [Change in Facility Exposure] ASC, [Relationship Number] ASC ", o);
            //Assert.AreEqual(" Group By [Change in Facility Exposure],[Relationship Number],[Accrual Date]", g);

            //// ////Order by other column DESC
            //rr.ReportOrderByClause.SetSorting(rr, "Change in Facility Exposure", "0");
            //rr.ReportGroupByClause.SetSorting("Change in Facility Exposure");
            //o = rr.ReportOrderByClause.ToSQL();
            //g = rr.ReportGroupByClause.ToSql();
            //// Assert.AreEqual(" ORDER BY [Change in Facility Exposure] DESC, [Relationship Number] ASC ", o);
            //Assert.AreEqual(" Group By [Change in Facility Exposure],[Relationship Number],[Accrual Date]", g);

            ////Back to default column
            //rr.ReportOrderByClause.SetSorting(rr, "Relationship Number", "1");
            //rr.ReportGroupByClause.SetSorting("Relationship Number");
            //o = rr.ReportOrderByClause.ToSQL();
            //g = rr.ReportGroupByClause.ToSql();
            //Assert.AreEqual(" ORDER BY [Relationship Number] ASC ", o);
            //Assert.AreEqual(" Group By [Relationship Number],[Accrual Date]", g);


        }

        [TestMethod]
        public void TestAlternateOrderByandGroupBy()
        {
            RelationalReport rr = (RelationalReport)GetReport2("104");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 104,
            };
            ReportFactory.ApplyUserSelection(rr, userreport);

            string o;
            string g;

            //Default
            o = rr.ReportOrderByClause.ToSQL();
            g = rr.ReportGroupByClause.ToSql();
            Assert.AreEqual(" ORDER BY [Relationship Number] ASC ", o);
            Assert.AreEqual(" Group By [Relationship Number],[Accrual Date]", g);

            //Sort on default column
            rr.ReportOrderByClause.SetSorting(rr, "Relationship Number", "0");
            o = rr.ReportOrderByClause.ToSQL();
            g = rr.ReportGroupByClause.ToSql();
            Assert.AreEqual(" ORDER BY [Relationship Number] DESC ", o);
            Assert.AreEqual(" Group By [Relationship Number],[Accrual Date]", g);

            //Sort by other column ASC
            rr.ReportOrderByClause.SetSorting(rr, "Change in Facility Exposure", "1");
            rr.ReportGroupByClause.SetSorting("Change in Facility Exposure");
            o = rr.ReportOrderByClause.ToSQL();
            g = rr.ReportGroupByClause.ToSql();
            // Assert.AreEqual(" ORDER BY [Change in Facility Exposure] ASC, [Relationship Number] ASC ", o);
            Assert.AreEqual(" Group By [Change in Facility Exposure],[Relationship Number],[Accrual Date]", g);

           // ////Order by other column DESC
            rr.ReportOrderByClause.SetSorting(rr, "Change in Facility Exposure", "0");
            rr.ReportGroupByClause.SetSorting("Change in Facility Exposure");
            o = rr.ReportOrderByClause.ToSQL();
            g = rr.ReportGroupByClause.ToSql();
            // Assert.AreEqual(" ORDER BY [Change in Facility Exposure] DESC, [Relationship Number] ASC ", o);
            Assert.AreEqual(" Group By [Change in Facility Exposure],[Relationship Number],[Accrual Date]", g);

            //Back to default column
            rr.ReportOrderByClause.SetSorting(rr, "Relationship Number", "1");
            rr.ReportGroupByClause.SetSorting("Relationship Number");
            o = rr.ReportOrderByClause.ToSQL();
            g = rr.ReportGroupByClause.ToSql();
            Assert.AreEqual(" ORDER BY [Relationship Number] ASC ", o);
            Assert.AreEqual(" Group By [Relationship Number],[Accrual Date]", g);
            

        }



        [TestMethod]
        public void TestOrderBy12()
        {

            RelationalReport rr = (RelationalReport)GetReport2("12");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 12,

            };
            ReportFactory.ApplyUserSelection(rr, userreport);

            rr.ReportOrderByClause.SetSorting(rr, String.Format("[{0}]", "Obligor Name"), "ASC");
            string o = rr.ReportOrderByClause.ToSQL();
            Assert.AreEqual(" ORDER BY [Obligor Name] ASC ", o);

            rr.ReportOrderByClause.SetSorting(rr, String.Format("[{0}]", "Obligor Name"), "DESC");
            o = rr.ReportOrderByClause.ToSQL();
            Assert.AreEqual(" ORDER BY [Obligor Name] DESC ", o);

        }

        [TestMethod]
        public void TestOrderBy()
        {

            RelationalReport rr = (RelationalReport)GetReport2("4");
            UserReport userreport = new UserReport
            {
                Level = -1,
                ReportId = 4,
                 
            };
            ReportFactory.ApplyUserSelection(rr, userreport);

            rr.ReportOrderByClause.SetSorting(rr, String.Format("[{0}]", "Obligor Name"),"ASC");
            string o = rr.ReportOrderByClause.ToSQL();
            Assert.AreEqual(" ORDER BY [Obligor Name] ASC ",o);

            rr.ReportOrderByClause.SetSorting(rr, String.Format("[{0}]", "Obligor Name"), "DESC");
            o = rr.ReportOrderByClause.ToSQL();
            Assert.AreEqual(" ORDER BY [Obligor Name] DESC ", o);
           
        }


        private static IRelationalReport GetReport2(string id)
        {
            string cachekey = "U000123" + "@@@" + id;
            RelationalReport rr = (RelationalReport)ReportCache.Instance.GetReport(cachekey);
            if (rr == null)
            {
                rr = new RelationalReport();
                rr.ReportId = id;
                rr.CacheKey = cachekey;
                rr.SQLDataSource = "not used";
                rr.Connect.UserName = "rryvkin_fa";
                rr.Connect.Password = "Kirova2017";
                rr.Connect.ClamedUserName = "U000123";
                rr.Connect.DATAConnectionString = @"Data Source=dssdb14\dev2014;Initial Catalog=RAF_even;Integrated Security=SSPI;Connect Timeout=240";
                rr.Connect.ApplicationId = "RAF_LW_Rel";
                rr.Connect.UserGroup = "RAF";
                ReportFactory factory = new ReportFactory();
                factory.GetReportDetails(rr);
                if (rr == null)
                {
                    throw new Exception("Report " + id + "does not exist");
                }
                ReportCache.Instance.AddReport(rr);

            }
            return rr;
        }

        private static IRelationalReport GetReport(string id)
        {
            string cachekey = "U000123" + "@@@" + id;
            RelationalReport rr = (RelationalReport)ReportCache.Instance.GetReport(cachekey);
            if (rr == null)
            {
                rr = new RelationalReport();
                rr.ReportId = id;
                rr.CacheKey = cachekey;
                rr.SQLDataSource = "not used";
                rr.Connect.UserName = "rryvkin_fa";
                rr.Connect.Password = "Kirova2017";
                rr.Connect.ClamedUserName = "U000123";
                rr.Connect.DATAConnectionString = @"Data Source=dssdb14\dev2014;Initial Catalog=RAF PROD OA DW;Integrated Security=SSPI;Connect Timeout=240";
                rr.Connect.ApplicationId = "PROD_OA";
                rr.Connect.UserGroup = "RAF";
                ReportFactory factory = new ReportFactory();
                factory.GetReportDetails(rr);
                if (rr == null)
                {
                    throw new Exception("Report " + id + "does not exist");
                }
                ReportCache.Instance.AddReport(rr);

            }
            return rr;
        }


        public string ObjectToString(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public object StringToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
