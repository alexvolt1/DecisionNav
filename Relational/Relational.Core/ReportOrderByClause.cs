using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSS.Logs;
using System.Threading.Tasks;
using System.Configuration;

namespace Relational.Core
{
    public class ReportOrderByClause
    {
        private string orderColumn;
        public string OrderColumn
        {
            get { return orderColumn; }
            set { orderColumn = value; }
        }
        private string orderDir;
        private string groupColumns;

        private List<string> orderColumnsCollection;

        private IRelationalReport reportsession;
        public ReportOrderByClause(IRelationalReport reportsession)
        {
            string ModuleLogging = System.Configuration.ConfigurationManager.AppSettings["debugging"];
            bool isDebugging = false;
            if (isDebugging)
            {
                bool.TryParse(ModuleLogging, out isDebugging);
                if (isDebugging)
                {
                    Log.AppendToFile = true;
                    Log.Console = true;
                    Log.LogPath = @"D:\Logs";
                    Log.FileName = "Relational ReportOrderByClause";
                }
            }
            this.reportsession = reportsession;
            orderColumn = reportsession.reportDetails["ReportSortColumns"];
            orderDir = reportsession.reportDetails["ReportSortDir"];
            groupColumns = reportsession.reportDetails["ReportGroupBy"];
           

            SQLField v = null;
            if (reportsession.ReportColumns.ExtraColumns != null)
            {
                v = reportsession.ReportColumns.ExtraColumns.Where(x => x.Name == orderColumn.Trim('[', ']')).FirstOrDefault();
            }
            if (v != null)
            {
                reportsession.sqlOrderByStringForPager = String.Format(" ORDER BY {0} {1} ", v.Expression, orderDir);
            }
            else
            {
                reportsession.sqlOrderByStringForPager = String.Format(" ORDER BY {0} {1} ", orderColumn, orderDir);
            }

            //reportsession.sqlOrderByString = "ORDER BY " + orderColumn + "  " + orderDir;
        }



        public string ToSQL()
        {
            orderColumnsCollection = orderColumn.Trim('[', ']').Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToList();

           


            List<string> cols = new List<string>();
            if (groupColumns != null && groupColumns.Length > 0)
            {
                foreach (string c in orderColumnsCollection)
                {
                    if (!groupColumns.Contains(c))
                    {
                        cols.Add(String.Format("SUM({0})", WrapTextNText(c)));
                    }
                    else
                    {
                        cols.Add(WrapTextNText(c,true));
                    }
                }
                string s = string.Join(",", cols);
                return String.Format(" ORDER BY {0} {1} ", s, orderDir);
            }
            else
            {
                //return String.Format(" ORDER BY {0} {1} ", "[" + String.Join("],[", orderColumnsCollection) + "]", orderDir);

                List<string> wr = new List<string>();
                foreach (string c in orderColumnsCollection)
                {
                    wr.Add(WrapTextNText(c,true));
                }
                return String.Format(" ORDER BY {0} {1} ", string.Join(",", wr), orderDir);
            }
        }
        private string WrapTextNText(string column)
        {
            return WrapTextNText(column, false);
        }

        private string WrapTextNText(string column,bool wrap)
        {   string c = CONVERTERS.SQLHelper.WrapSQLName(column);
        if (reportsession.sqlTableSchema != null) 
            {
                if (reportsession.sqlTableSchema.ContainsKey(column))
                {
                    string v = reportsession.sqlTableSchema[column];
                    if (v != null && (v == "text" || v == "ntext"))
                    {
                        return string.Format(" CAST({0} AS NVARCHAR(300)) ", c);
                    }
                }
                else
                {
                    Log.Write("SQLTABLESchema, cannot find " +  column + " key");
                }

            }

            return c;
        }

        internal string ToSQLFlat()
        {
            //if (groupColumns != null && groupColumns.Length > 0 && !groupColumns.Contains(orderColumn))
            //{
            //    return String.Format("ORDER BY {0} ", "[" + String.Join("],[", orderColumnsCollection) + "]");
            //}     
            return "";
        }

        public void SetSorting(IRelationalReport reportsession, string column, string direction)
        {
            orderColumn = "";
            orderDir = direction == "1" ? "ASC" : "DESC";
            SQLField v = null;
            if (reportsession.ReportColumns.ExtraColumns != null)
            {
                v = reportsession.ReportColumns.ExtraColumns.Where(x => x.Name == column).FirstOrDefault();
            }
            if (v != null)
            {
                orderColumn = String.Format("[{0}]",v.Expression);
            }
            else
            {
                orderColumn = String.Format("[{0}]", column);
            }
            //reportsession.sqlOrderByString = "ORDER BY " + orderColumn + "  " + orderDir;

           // orderColumn = String.Format("[{0}]",column);
           // orderDir = direction == "1" ? "ASC" : "DESC";
            //SQLField v = null;
            //if (reportsession.ReportColumns.ExtraColumns != null)
            //{
            //    v = reportsession.ReportColumns.ExtraColumns.Where(x => x.Name == orderColumn.Trim('[', ']')).FirstOrDefault();
            //}
            //if (v != null)
            //{
            //    reportsession.sqlOrderByStringForPager = String.Format(" ORDER BY {0} {1} ", v.Expression, orderDir);
            //}
            //else
            //{
            //    reportsession.sqlOrderByStringForPager = String.Format(" ORDER BY {0} {1} ", orderColumn, orderDir);
            //}

            //reportsession.sqlOrderByString = "ORDER BY " + orderColumn + "  " + orderDir;
        }


    }
}


           //string orderColumn = "";
           //string orderDir = "";

           //// Here will be the temp logic -----------------------------------------
           //if (reportsession.sortColumn != null && reportsession.sortColumn != "")
           //{
           //    orderColumn = reportsession.sortColumn;
           //}
           //else
           //{
           //    orderColumn = reportsession.orderColumn;
           //}
           //if (reportsession.sortDir != null && reportsession.sortDir != "")
           //{
           //    orderDir = reportsession.sortDir;
           //}
           //else
           //{
           //    orderDir = reportsession.orderDir;
           //}
           ////------------------------------------------------------------------------
           
           //SQLField v=null;
           //  if (reportsession.ReportColumns.ExtraColumns!=null)
           //  {
           //   v = reportsession.ReportColumns.ExtraColumns.Where(x => x.Name == orderColumn.Trim('[', ']')).FirstOrDefault();
           //  }
           //if (v != null)
           //  {
           //    reportsession.sqlOrderByStringForPager = String.Format(" ORDER BY {0} {1} ", v.Expression, orderDir);
           //  }
           // else
           //  {
           //    reportsession.sqlOrderByStringForPager = String.Format(" ORDER BY {0} {1} ", orderColumn, orderDir);
           //  }

           //  reportsession.sqlOrderByString = "ORDER BY " + orderColumn + "  " + orderDir;
