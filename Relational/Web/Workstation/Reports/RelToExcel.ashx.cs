using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ClosedXML.Excel;
using System.Data;
using Relational.Core;
namespace Relational.Workstation.Reports
{
    /// <summary>
    /// Summary description for RelToExcel
    /// </summary>
    public class RelToExcel : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            byte[] file = null;
            string id = context.Request.QueryString["id"];
            string cachekey = DeciwebProperties.DeciwebSession.Current.UserName + "@@@" + id;
            RelationalReport rr = (RelationalReport)ReportCache.Instance.GetReport(cachekey);
            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("content-disposition", "attachment;filename=" + rr.ReportName + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ssZ") + ".xlsx");
            if (context.Request.HttpMethod == "GET")
            {
                file = ReportFactory.GetDataForExport(rr);
                context.Response.BinaryWrite(file);
            }

            context.Response.Flush();
            context.Response.End();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}