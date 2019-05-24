using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSS.Logs;
using System.Reflection;
using System.Configuration;
namespace Relational.Core
{
    public class ReportColumns
    {
        private List<string> columnsCollection;
        public List<string> ColumnsCollection
        {
            get { return columnsCollection; }
            set { columnsCollection = value; }
        }
        private List<string> groupByColumnsCollection;
        public List<string> GroupByColumnsCollection
        {
            get { return groupByColumnsCollection; }
            set { groupByColumnsCollection = value; }
        }
        public string Columns = "";
        public string SQLColumns = "";
        public string AllColumns = "";
        public string DefaultColumns = "";
       
        public List<SQLField> ExtraColumns;
        IRelationalReport reportsession;
        public ReportColumns(IRelationalReport rr)
        {
            
            string ModuleLogging = ConfigurationManager.AppSettings["ModuleLogging"];
            string InfoLogging = ConfigurationManager.AppSettings["InfoLogging"];
            bool isDebugging = false;
            bool isVerbose = false;

            bool.TryParse(ModuleLogging, out isDebugging);
            bool.TryParse(InfoLogging, out isVerbose);
          
            if (isDebugging)
            {
                Log.AppendToFile = true;
                Log.Console = true;
                Log.LogPath = @"D:\Logs";
                Log.FileName = "Relational Internal Service2";
             
                //Log.Write("Service called, debugging enabled", thisType);
            }


            if (rr.reportDetails == null)
            {
                throw new Exception("No Report Details available");
            }
            reportsession = rr;
            AllColumns = reportsession.reportDetails["ReportAllColumns"];
            DefaultColumns = reportsession.reportDetails["ReportDefaultColumns"];

            Columns = AllColumns;//
            columnsCollection = Columns.Trim('[', ']').Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToList();
           
            
        }
        public object BuildColumnsJson()
        {
            try
            {
               CONVERTERS.ToJSON.BuildColumnsJSON(reportsession);
               return reportsession.ColumnsJson;
            }
            catch (Exception ex)
            {
                Log.Write("Error Source: " + reportsession.ReportName, typeof(ReportColumns));
                Log.Write("Columns: " + reportsession.ReportColumns.Columns, typeof(ReportColumns));
                Log.Write(ex.ToString(), typeof(ReportColumns));
                throw ex;
            }

        }


        internal string[] ToArray()
        {
            if (reportsession.reportTableName.ToLower().Contains("updown") && ExtraColumns!=null && ExtraColumns.Count>0)
            {
                string cols  = this.Columns.Replace(",[Facility Exposure, End]", ",[Facility Exposure, End],[" + string.Join("],[", ExtraColumns.Select(f => f.Name).ToArray()) + "]");
                return cols.Trim('[', ']').Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            }
            else
            {
               return columnsCollection.ToArray();
            }
        }
        internal string ToSql()
        {
            string sql = "";
            if (reportsession.ReportGroupByClause.GroupByCollection != null && reportsession.ReportGroupByClause.GroupByCollection.Count > 0)
            {
                List<string> strings = new List<string>();
                groupByColumnsCollection = reportsession.ReportGroupByClause.GroupByCollection;
                foreach (string column in columnsCollection)
                {
                    if (groupByColumnsCollection.Contains(column))
                    {
                        strings.Add(CONVERTERS.SQLHelper.WrapSQLName(column));
                    }
                    else
                    {
                        strings.Add(String.Format("SUM([{0}]) AS [{0}]", column));
                    }
                }
                sql = String.Join(",", strings);
            }
            else
            {
                sql = "[" + String.Join("],[", columnsCollection) + "]";
            }


            if (reportsession.reportTableName.ToLower().Contains("updown"))
            {
               List<string> s = new List<string>();
                foreach (SQLField f in ExtraColumns)
                {
                    string c;
                    if (f.Expression != null)
                    {
                        c= "(" + f.Expression + ") as '" + f.Name + "'";
                    }
                    else
                    {
                        c = CONVERTERS.SQLHelper.WrapSQLName(f.Name);
                    }
                   
                    s.Add(c);
                }
                sql = sql.Replace(",[Facility Exposure, End]", ",[Facility Exposure, End]," + string.Join(",", s.ToArray()));
            }
            return sql;
        }



        //internal string ToSql()
        //{
        //string sql = "";
        //if (reportsession.ReportGroupByClause.GroupByCollection != null && reportsession.ReportGroupByClause.GroupByCollection.Count>0)
        //{

        //    List<string> strings = new List<string>();
        //    groupByColumnsCollection = reportsession.ReportGroupByClause.GroupByCollection;
        //    foreach (string column in columnsCollection)
        //    {
        //        if (groupByColumnsCollection.Contains(column))
        //        {
        //            strings.Add(CONVERTERS.SQLHelper.WrapSQLName(column));
        //        }
        //        else
        //        {
        //            strings.Add(String.Format("SUM([{0}]) AS [{0}]",column));
        //        }
        //    }
        //    sql = String.Join(",",strings);
        //}
        //else
        //{
        //  sql = "[" + String.Join("],[", columnsCollection) + "]";
        //}
        //return sql;
        //}

        internal string ToSqlString()
        {
            return "'" + String.Join("','", columnsCollection) + "'";
        }

        public void ApplyExtraFields(List<SQLField> fields )
        {
            ExtraColumns = fields;
        }







        internal bool ValidColumn(string col)
        {
          if(ColumnsCollection.Contains(col)){
              return true;
          }
          if(ExtraColumns.Where(x=>x.Name==col)!=null){
              return true;
          }
            return false;
        }
    }
}
