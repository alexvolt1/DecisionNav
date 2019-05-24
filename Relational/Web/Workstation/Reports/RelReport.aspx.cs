using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using Relational.Core;
using DSS.Logs;
using Infor.PM.Decision.DecisionCore.Components;
namespace Relational.Workstation.Reports
{
    public partial class RelReport : System.Web.UI.Page
    {
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
            ReportId = rr.ReportId;
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
            RelationalReport rr = (RelationalReport)GetReport(Report.ReportId.ToString());
            ReportFactory.ApplyUserSelection(rr, Report);
            ReportCache.Instance.AddReport(rr);
            return rr.ColumnsJson;
        }

        [WebMethod]
        public static object GetData(int id, int from, int to, string searchstring)
        {
            RelationalReport rr = (RelationalReport)GetReport(id.ToString());
            return ReportFactory.GetData(rr, from, to);
        }

        [WebMethod]
        public static object Sort(int id, string col, string dir)
        {
            RelationalReport rr = (RelationalReport)GetReport(id.ToString());
            ReportFactory.Sort(rr, col, dir);
            return ReportFactory.GetDataSerialized(rr, 1, 100);
        }

        [WebMethod]
        public static object GetMembers(int id)
        {
            RelationalReport rr = (RelationalReport)GetReport(id.ToString());
            return rr.ReportMembersCollection.ToJson();
        }

        [WebMethod]
        public static object GetLevel(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.ReportId.ToString());
            ReportFactory.ApplyUserSelection(rr, Report);
            return rr.ReportMembersCollection.ToJson(Report);
        }

        [WebMethod]
        public static object GetFilters(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.ReportId.ToString());
            return rr.ReportFiltersCollection.ToJson();
        }

        [WebMethod]
        public static object GetFilterValues(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.ReportId.ToString());
            ReportFactory.ApplyUserSelection(rr, Report);
            return rr.ReportFiltersCollection.ToJson(Report);
        }

        [WebMethod]
        public static object GetSearchResults(Relational.Core.UserReport Report)
        {
            RelationalReport rr = (RelationalReport)GetReport(Report.ReportId.ToString());
            return   ReportFactory.GetSearchResults(rr, Report);
         
        }

        #endregion

        private static IRelationalReport GetReport(string id)
        {
          
            string cachekey = DeciwebProperties.DeciwebSession.Current.UserName + "@@@" + id;
            RelationalReport rr = (RelationalReport)ReportCache.Instance.GetReport(cachekey);

            if (rr == null)
            {
                //NavigationItem topic = Deciweb.Components.NavigationData.Instance.GetTopicById(id);
                System.Net.NetworkCredential creds = Infor.PM.Decision.DecisionCore.Server.CurrentUser.GetDatabaseCredential("");
                rr = new Core.RelationalReport();
               // rr.TopicID = topic.ID;
                rr.ReportId = id;
                rr.CacheKey = cachekey;
                rr.SQLDataSource = "-na-";
                rr.Connect.UserName = creds.UserName;
                rr.Connect.Password = creds.Password;
                rr.Connect.ClamedUserName = creds.UserName;
                rr.Connect.DATAConnectionString = @"Data Source=dssdb14\dev2014;Initial Catalog=RAF PROD OA DW;Integrated Security=SSPI;Connect Timeout=240";
                rr.Connect.ApplicationId = "PROD_OA";
                rr.Connect.UserGroup = "RAF";

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