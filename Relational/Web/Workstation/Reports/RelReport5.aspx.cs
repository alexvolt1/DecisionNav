using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Relational.Core;
using DSS.Logs;

namespace Relational.Workstation.Reports
{
    public partial class RelReport5 : System.Web.UI.Page
    {

        public string TopicId { get; set; }
        public string ReportId { get; set; }
        public static string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public bool isDebugging
        {
            get
            {
                return true;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.AppendToFile = true;
            Log.Console = true;
            Log.LogPath = @"D:\Logs";
            Log.FileName = "Relational Report2";


            string param = Request.QueryString["a"];
            if (param == null || param == "")
            {
                Response.Write("Incorrect param");
                Response.End();
            }

            if (String.IsNullOrEmpty(DeciwebProperties.DeciwebSession.Current.UserName))
            {
                Response.Redirect("../../Login.aspx");
            }
            //     We will load this report to a cache here        //
            try
            {
                RelationalReport rr = (RelationalReport)GetReport(param);
                ReportName = rr.ReportName;
                ReportDescription = rr.ReportDescription;
                TopicId = rr.TopicID;

            }
            catch (Exception ex)
            {
                Log.Write(ex.ToString());
                if (isDebugging)
                {
                    Response.Write(ex.ToString());
                }
                else
                {
                    Response.Write("Unable to load this report");
                }

                Response.End();
            }
        }


        #region Web Methods
        [WebMethod]
        public static object GetMetaData(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.TopicId);
            ReportFactory.ApplyUserSelection(rr, Report);
            ReportCache.Instance.AddReport(rr);
            return rr.ReportColumns.BuildColumnsJson();
        }

        [WebMethod]
        public static object GetData(string id, int from, int to, string searchstring)
        {
            RelationalReport rr = (RelationalReport)GetReport(id);
            return ReportFactory.GetData(rr, from, to);
        }

        [WebMethod]
        public static object Sort(string id, string col, string dir, string buffer)
        {
            RelationalReport rr = (RelationalReport)GetReport(id);
            ReportFactory.Sort(rr, col, dir);
            //return ReportFactory.GetDataSerialized(rr, 1, 100);
            int.TryParse(buffer, out int ibuffer);
            return ReportFactory.GetDataSerialized(rr, 1, ibuffer);
        }

        [WebMethod]
        public static object GetHierarchies(string id)
        {
            RelationalReport rr = (RelationalReport)GetReport(id);
            return rr.Hierarchies.ReportHierarchiesCollection;
        }

        [WebMethod]
        public static object SetHierarchies(string id, int hid)
        {
            RelationalReport rr = (RelationalReport)GetReport(id);
            rr.Hierarchies.SetCurrent(hid);
            return rr.ReportMembersCollection.ToJson();
        }

        [WebMethod]
        public static object GetMembers(string id)
        {
            RelationalReport rr = (RelationalReport)GetReport(id);
            return rr.ReportMembersCollection.ToJson();
        }

        [WebMethod]
        public static object GetLevel(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.TopicId);
            ReportFactory.ApplyUserSelection(rr, Report);
            return rr.ReportMembersCollection.ToJson(Report);
        }

        [WebMethod]
        public static object GetFilters(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.TopicId);
            return rr.ReportFiltersCollection.ToJson();
        }

        [WebMethod]
        public static object GetFilterValues(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.TopicId);
            ReportFactory.ApplyUserSelection(rr, Report);
            return rr.ReportFiltersCollection.ToJson(Report);
        }

        [WebMethod]
        public static object GetSearchResults(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.TopicId);
            return ReportFactory.GetSearchResults(rr, Report);

        }

        #endregion

        private static IRelationalReport GetReport(string id)
        {
            string cachekey = DeciwebProperties.DeciwebSession.Current.UserName + "@@@" + id;
            RelationalReport rr = (RelationalReport)ReportCache.Instance.GetReport(cachekey);
            if (rr == null)
            {
                rr = new Core.RelationalReport
                {
                    TopicID = id,
                    ReportId = id,
                    ReportDescription = "Report",
                    CacheKey = cachekey,
                    SQLDataSource = "-na-",
                };
                rr.Connect.UserName = "demo";
                rr.Connect.Password = "Demopw01";
                rr.Connect.ClamedUserName = "Demo";
                rr.Connect.DATAConnectionString = @"Data Source=AVOLT10L\AVOLT10L;Initial Catalog=BBT_Core_Even;Integrated Security=SSPI;Connect Timeout=240";
                rr.Connect.ApplicationId = "BBT_LW";
                rr.Connect.UserGroup = "BBT";

                Relational.Core.ReportFactory factory = new Core.ReportFactory();
                factory.GetReportDetails(rr);
                if (rr == null)
                {
                    throw new Exception("Report " + id + "does not exist");
                }


                ReportCache.Instance.AddReport(rr);
            }
            return rr;
        }
    }
}