using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Relational.Core.CONVERTERS;
using Relational.Core.Client;
using System.ServiceModel;
using System.Diagnostics;
using DSS.Logs;

namespace Relational.Core.DATA
{
    public class ReportData
    {
        #region VARIABLES
        private static string address = System.Configuration.ConfigurationManager.AppSettings["RelationalService"];
        // string ModuleLogging = System.Configuration.ConfigurationManager.AppSettings["debugging"];
        public static bool isDebugging = true;
        #endregion

        public ReportData()
        {
            // bool.TryParse(ModuleLogging, out isDebugging);
            // if (isDebugging)
            // {
            Log.AppendToFile = true;
            Log.Console = true;
            Log.LogPath = @"C:\Logs";
            Log.FileName = "Relational ReportData";
            // }
            //address = System.Configuration.ConfigurationManager.AppSettings["RelationalService"];
        }


        #region REPORT
        private string getConnect()
        {
            return "blah";
        }

        internal static DataTable GetAvailableReportsForDatasource(DWCredentials Connect)
        {
            string sql = " SELECT [QueryID] " +
                         " ,[ReportName] " +
                         " ,[ReportHasFilter] " +
                        // " ,[ReportDefaultColumns] " +
                        // " ,[ReportAllColumns] " +
                        // " ,[ReportSortColumns] " +
                        // " ,[ReportSortDir] " +
                        // " ,[ReportTableName] " +
                        // " ,[ReportVariance] " +
                        // " ,[ReportNumRecs] " +
                        // " ,[ReportWhereStr] " +
                        // " ,[ReportGroupBy] " +
                        // " ,[ReportChartType] " +
                        // " ,[ReportColWidths] " +
                        " ,[AlternateReport] " +
                        " FROM [LWStoredProcs2] ";

            using (DataSet ds = GetDataSet(sql, Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to get reports list from db ");
                else
                    return ds.Tables[0];
            }

        }
        internal static Dictionary<string, string> GetReportDetails(IRelationalReport rr)
        {
            Dictionary<string, string> reportDetails = new Dictionary<string, string>();
            string sql = "SELECT ReportNumRecs,ReportVariance,ReportAllColumns,ReportDefaultColumns,ReportTableName,ReportSortColumns,ReportSortDir," +
                         " ReportGroupBy, ReportChartType, ReportName" +
                         " ,replace(ReportWhereStr,'''''','''') as [ReportWhereStr]" +
                         " ,ReportHasFilter, ReportColWidths, AlternateReport" +
                         " FROM LWStoredProcs2 WHERE QueryID = " + int.Parse(rr.ReportId);

            using (DataSet ds = GetDataSet(sql, rr.Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to get Report Details from db ");

                int columns = ds.Tables[0].Columns.Count;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < columns; i++)
                    {
                        reportDetails.Add(ds.Tables[0].Columns[i].ToString(), row[i].ToString());
                    }
                }
            }

            rr.ReportName = reportDetails["ReportName"].ToString();
            rr.ReportColWidths = reportDetails["ReportColWidths"].ToString();
            rr.reportTableName = reportDetails["ReportTableName"];
            return reportDetails;
        }
        internal static Dictionary<string, string> GetReportCountAndTotals(IRelationalReport rr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("WITH result_set AS (");
            sb.AppendLine("SELECT");
            sb.AppendLine(rr.ReportColumns.ToSql());
            sb.AppendLine("FROM");
            sb.AppendLine(rr.reportTableName);
            sb.AppendLine(rr.ReportWhereClause.ToSql());
            sb.AppendLine(rr.ReportGroupByClause.ToSql());
            sb.AppendLine(") SELECT COUNT(1) as 'RecordsCount' FROM result_set ");
            using (DataSet ds = GetDataSet(sb.ToString(), rr.Connect))
            {
                rr.ReportRecordsCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }



            Dictionary<string, string> dict = new Dictionary<string, string>();
            using (DataSet ds = GetDataSet(rr.TotalsSQL, rr.Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to get Count  and Totals from db ");

                int columns = ds.Tables[0].Columns.Count;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < columns; i++)
                    {
                        dict.Add(ds.Tables[0].Columns[i].ToString(), row[i].ToString());
                    }
                }
            }
            // rr.ReportRecordsCount = Convert.ToInt32(dict["RecordCount"]);
            return dict;
        }
        internal static Dictionary<string, string> GetTableSchema(string sql, DWCredentials Connect)
        {
            Dictionary<string, string> sqlTableSchema = new Dictionary<string, string>();
            using (DataSet ds = GetDataSet(sql, Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to get Table schema from db ");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sqlTableSchema.Add(row[0].ToString(), row[1].ToString());
                }
            }
            return sqlTableSchema;
        }
        public static object GetDataSerialized(RelationalReport rr, int from, int to)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("WITH result_set AS (");
            sb.AppendLine("SELECT");
            sb.AppendLine(String.Format("ROW_NUMBER() OVER ({0}) AS [id],", rr.ReportOrderByClause.ToSQL()));
            sb.AppendLine(rr.ReportColumns.ToSql());
            sb.AppendLine("FROM");
            sb.AppendLine(rr.reportTableName);
            sb.AppendLine(rr.ReportWhereClause.ToSql());
            sb.AppendLine(rr.ReportGroupByClause.ToSql());
            sb.AppendLine(") SELECT * FROM result_set WHERE ");
            sb.AppendLine(String.Format("[id] BETWEEN {0} and {1}", from, from + to - 1));
            sb.AppendLine(rr.ReportOrderByClause.ToSQLFlat());
            using (DataSet ds = GetDataSet(sb.ToString(), rr.Connect))
            {
                //hack to add indent to a "Obligor" fields based on "DealType" for "Part-Syndi Report"
                //if (rr.ReportName == "Part-Syndi Report")
                //{
                //return Serialization.DataTableToJSONRelational_Extended(ds.Tables[0]);
                //}
                //else{
                //    return Serialization.DataTableToJSONRelational(ds.Tables[0]);
                //}

                return Serialization.DataTableToJSONRelational(ds.Tables[0]);
            }
        }
        public static object GetDataSerialized2(RelationalReport rr, int from, int to)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("WITH result_set AS (");
            sb.AppendLine("SELECT");
            sb.AppendLine(String.Format("ROW_NUMBER() OVER ({0}) AS [id],", rr.ReportOrderByClause.ToSQL()));
            sb.AppendLine(rr.ReportColumns.ToSql());
            sb.AppendLine("FROM");
            sb.AppendLine(rr.reportTableName);
            sb.AppendLine(rr.ReportWhereClause.ToSql());
            sb.AppendLine(rr.ReportGroupByClause.ToSql());
            sb.AppendLine(") SELECT * FROM result_set WHERE ");
            sb.AppendLine(String.Format("[id] BETWEEN {0} and {1}", from, from + to));
            sb.AppendLine(rr.ReportOrderByClause.ToSQLFlat());
            return sb.ToString();
        }

        #endregion


        #region FILTERS

        // Filter tree
        internal static List<FilterValueItem> GetReportFilterValues(IRelationalReport rr, LevelItemExtended item)
        {
            return GetReportFilterValues(rr, item, "", "");
        }
        internal static List<FilterValueItem> GetReportFilterValues(IRelationalReport rr, LevelItemExtended item, string memberSelect)
        {
            return GetReportFilterValues(rr, item, memberSelect, "");
        }
        internal static int GetCurrentMonth(IRelationalReport rr)
        {
            using (DataSet ds = GetDataSet("EXEC spGetCurrUpDownMonth", rr.Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to Current Month from db ");

                int i = 0;
                int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out i);
                return i;
            }

        }
        internal static List<FilterValueItem> GetReportFilterValues(IRelationalReport rr, LevelItemExtended item, string memberSelect, string prevFilter)
        {
            // List<string> values = new List<string>();
            List<FilterValueItem> values = new List<FilterValueItem>();
            string sql = "";
            if (item.listid == 0)
            {
                string currField = String.Format("[{0}]", item.Name);
                if (item.orderfield != null && item.orderfield != "")
                {
                    currField += String.Format(",[{0}]", item.orderfield);
                }
                List<string[]> parameters = new List<string[]>() { };
                parameters.Add(new[] { "@tableID", string.Format("[{0}]", item.SourceTable) });
                parameters.Add(new[] { "@memSel", memberSelect });
                parameters.Add(new[] { "@fieldList", currField });
                parameters.Add(new[] { "@prevFilter", prevFilter });

                sql = "spGetReportFilterDataFromTable";

                if (item.orderfield != null && item.orderfield != "")
                {
                    parameters.Add(new[] { "@order", item.orderfield });
                }
                DataSet ds = GetDataSet(sql, parameters, rr.Connect);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    values.Add(new FilterValueItem(row[0].ToString()));
                }
            }
            else if (item.fieldtype == 3 || item.fieldtype == 4)
            {

                DataSet ds = GetDataSet("spGetReportFilterDataFromList", new[] { "@LISTID", item.listid.ToString() }, rr.Connect);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    values.Add(new FilterValueItem(row[0].ToString()));
                }
            }
            else
            {
                List<string[]> parameters = new List<string[]>() { };
                parameters.Add(new[] { "@LISTID", item.listid.ToString() });
                parameters.Add(new[] { "@TABLEID", item.SourceTable });
                parameters.Add(new[] { "@memSel", memberSelect });
                parameters.Add(new[] { "@fieldList", "[" + item.Name + "]" });
                parameters.Add(new[] { "@prevFilter", prevFilter });


                DataSet ds = GetDataSet("spGetReportFilterDataRangeFromList", parameters, rr.Connect);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    FilterValueItem valueitem = new FilterValueItem();
                    valueitem.Name = row["Range"].ToString();
                    valueitem.Value1 = row["Value1"].ToString();
                    valueitem.Value2 = row["Value2"].ToString();

                    values.Add(new FilterValueItem(valueitem.Name, valueitem.Value1, valueitem.Value2));
                }

            }

            return values;

        }
        // Filter tree
        internal static void GetReportFilters(ReportFiltersCollection reportFiltersCollection, IRelationalReport reportsession)
        {
            using (DataSet ds = GetReportFilters(reportsession.ReportId, reportsession.Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to get Filters from db ");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ReportFilter f = new ReportFilter(row);
                    reportFiltersCollection.Filters.Add(f);
                }
            }
        }

        internal static DataSet GetReportFilterValues(string sql, List<string[]> parameters, DWCredentials Connect)
        {
            return GetDataSet(sql, parameters, Connect);
        }

        internal static DataSet GetReportFilterValues(string sql, string[] parameter, DWCredentials Connect)
        {
            return GetDataSet(sql, parameter, Connect);
        }

        internal static DataSet GetReportFilters(string ReportID, DWCredentials Connect)
        {
            return GetDataSet("spGetReportFilters", new[] { "@ReportID", ReportID }, Connect);
        }


        #endregion

        #region Custom Search

        internal static void GetKeywordSearchParams(IRelationalReport reportsession)
        {
            // string sql = "SELECT [SearchID],[TableName],[ColumnNameq] FROM [LW_CustomerSearch2]";
            string sql = "SELECT [SearchID],[TableName],[ColumnNameq] FROM [LW_CustomerSearch2] where [TableName]='" + reportsession.reportTableName + "'";
            using (DataSet ds = GetDataSet(sql, reportsession.Connect))
            {
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("Unable to get Search Results from db ");
                KeywordSearch ks = new KeywordSearch();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    ks.Add(new KeywordSearchItem((int)row["SearchID"], row["TableName"].ToString(), row["ColumnNameq"].ToString()));

                }
                reportsession.KeywordSearch = ks;
            }
        }

        internal static List<string> GetSearchResults(IRelationalReport reportsession, UserReport Report)
        {
            try
            {
                KeywordSearchItem item = reportsession.KeywordSearch.SearchParams.Where(x => x.ID == Report.Qid).FirstOrDefault();
                List<string> results = new List<string>();
                List<string[]> parameters = new List<string[]>() { };
                //parameters.Add(new[] { "@currSearchTable", item.TableName  });
                parameters.Add(new[] { "@currSearchTable", reportsession.reportTableName });
                parameters.Add(new[] { "@currSearchField", item.ColumnName });
                //parameters.Add(new[] { "@currSearchStr", Report.Keyword });
                parameters.Add(new[] { "@currSearchStr", Report.Keyword.Replace("'", "''").Replace(";", "") });
                using (DataSet ds = GetDataSet("spMemSelCustomer", parameters, reportsession.Connect))
                {
                    if (ds == null || ds.Tables.Count == 0)
                        throw new InvalidOperationException("Unable to get Search Results from db ");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        results.Add("");
                    }
                    else
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            results.Add(row[0].ToString());
                        }
                    }

                }
                return results;
            }
            catch (Exception ex)
            {
                Log.Write(ex.ToString());
                throw ex;
            }

        }
        #endregion


        #region MEMBERS




        internal static void GetHierarchies(List<ReportHierarchy> Hierarchies, IRelationalReport reportsession)
        {
            using (DataSet ds = GetHierTypes(reportsession.Connect))
            {
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    throw new InvalidOperationException("Unable to get HierTypes from db ");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ReportHierarchy hierarchy = new ReportHierarchy();
                    hierarchy.ID = int.Parse(row["Hierid"].ToString());
                    hierarchy.Name = row["HierName"].ToString();
                    hierarchy.SourceTable = row["SourceTable"].ToString();
                    Hierarchies.Add(hierarchy);
                }
            }

            foreach (ReportHierarchy h in Hierarchies)
            {
                using (DataSet ds = GetHierarchyMembers(h.ID, reportsession.Connect))
                {
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        throw new InvalidOperationException("Unable to get HierTypes from db ");
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ReportMember member = new ReportMember();
                        member.HierarhyId = row["HierID"].ToString();
                        member.Level = (int)row["Level"];
                        member.Name = row["Name"].ToString();
                        h.AddMember(member);
                    }
                }
            }
        }

        internal static DataSet GetHierTypes(DWCredentials Connect)
        {
            return GetDataSet("EXEC spGetHierarchyTypes", Connect);
        }

        internal static DataSet GetHierarchyMembers(int hierarchyId, DWCredentials Connect)
        {
            // return GetDataSet("SELECT * FROM LW_Hierarchy where name <> 'end' order by [level]", Connect);
            return GetDataSet("SELECT * FROM LW_Hierarchy where name <> 'end' and HierID = " + hierarchyId + " order by [level]", Connect);
        }




        internal static void GetMemberValues(ReportHierarchy currenthierarchy, int level, IRelationalReport reportsession)
        {
            if (reportsession == null || reportsession.ReportMembersCollection == null || reportsession.Hierarchies.ReportHierarchiesCollection == null)
            {
                Log.Write("Hierarchy " + currenthierarchy.Name + "is empty at Reportdata.cs:GetMembersValues");
                throw new InvalidOperationException("Hierarchy is empty");
            }
            if (currenthierarchy.Members == null || currenthierarchy.Members.Count < level + 1)
            {
                Log.Write("Members don't exist for " + currenthierarchy.Name + " at Reportdata.cs:GetMembersValues");
                throw new InvalidOperationException("Member doesn't exist");
            }

            int mlevel = currenthierarchy.Members[level].Level;
            ReportMember member = currenthierarchy.Members.Where(c => c.Level == mlevel).FirstOrDefault();
            if (member == null)
            {
                Log.Write("Unable to find Member [" + level + "] at Reportdata.cs:GetMembersValues");
                throw new InvalidOperationException("Unable to find Member [" + level + "]");
            }

            if (member.values != null) member.values.Clear();
            var s = member.Name;
            var h = currenthierarchy.SourceTable;
            string where = reportsession.ReportMembersCollection.ToSql();
            string sql = string.Format(
                "SELECT Distinct {0} FROM {1} {2} ORDER BY 1"

               , SQLHelper.WrapSQLName(s), SQLHelper.WrapSQLName(h), where == null || where == "" ? "" : " where " + where);
            // ,SQLHelper.WrapSQLName(s), SQLHelper.WrapSQLName("m_" + h), where == null || where == "" ? "" : " where " + where);




            using (DataSet ds = GetDataSet(sql, reportsession.Connect))
            {
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    throw new InvalidOperationException("Unable to get Member Values from db ");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ReportMemberValue membervalue = new ReportMemberValue();
                    membervalue.Text = row[0].ToString();
                    membervalue.Selected = false;
                    member.AddValue(membervalue);
                }
            }
        }



        //internal static void GetMemberValues(int hierarchyid, int level, IRelationalReport reportsession)
        //{
        //    if (reportsession == null || reportsession.ReportMembersCollection == null || reportsession.ReportMembersCollection.Hierarchies == null)
        //        throw new InvalidOperationException("Hierarchy is empty");
        //    if (reportsession.ReportMembersCollection.Hierarchies.Count < hierarchyid + 1)
        //        throw new InvalidOperationException("Hierarchy doesn't exist");
        //    if (reportsession.ReportMembersCollection.Hierarchies[hierarchyid].Members == null || reportsession.ReportMembersCollection.Hierarchies[hierarchyid].Members.Count < level + 1)
        //        throw new InvalidOperationException("Member doesn't exist");

        //   // ReportMember member = reportsession.ReportMembersCollection.Hierarchies[hierarchyid].Members.Where(c => c.Value.Level == level).FirstOrDefault().Value;
        //    ReportMember member = reportsession.ReportMembersCollection.Hierarchies[hierarchyid].Members.Where(c => c.Level == level).FirstOrDefault();
        //    if(member.values!=null)member.values.Clear();
        //    var s = member.Name;
        //    var h = reportsession.ReportMembersCollection.Hierarchies[hierarchyid].SourceTable ;
        //    string where  = reportsession.ReportMembersCollection.ToSql();
        //    string sql = string.Format(
        //        "SELECT Distinct {0} FROM {1} {2} ORDER BY 1"

        //       ,SQLHelper.WrapSQLName(s),SQLHelper.WrapSQLName(h), where==null || where == ""?"":" where " +  where);
        //       // ,SQLHelper.WrapSQLName(s), SQLHelper.WrapSQLName("m_" + h), where == null || where == "" ? "" : " where " + where);




        //    using (DataSet ds = GetDataSet(sql, reportsession.Connect))
        //    {
        //        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //            throw new InvalidOperationException("Unable to get Member Values from db ");
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            ReportMemberValue membervalue = new ReportMemberValue();
        //            membervalue.Text = row[0].ToString();
        //            membervalue.Selected = false;
        //            member.AddValue(membervalue);
        //        }
        //    }
        //}
        #endregion

        #region EXPORT
        internal static Byte[] GetDataForExport(RelationalReport rr)
        {


            Log.AppendToFile = true;
            Log.Console = true;
            Log.LogPath = @"D:\Logs";
            Log.FileName = "Relational Report Total Time";




            Stopwatch sw = new Stopwatch();
            sw.Start();
            Log.Write("Requesting Excel report [" + rr.Connect.TopicID + "] for [" + rr.Connect.UserName + "]");


            byte[] file = null;
            using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(address))
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
                try
                {
                    file = client.DownloadReport2(rr.Connect.BuildConnectObject(rr.ReportSQL));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            sw.Stop();
            Log.Write("Received Excel report [" + rr.Connect.TopicID + "] for [" + rr.Connect.UserName + "] in [" + sw.Elapsed.Hours + ":" + sw.Elapsed.Minutes + ":" + sw.Elapsed.Seconds + "]");
            return file;
        }
        #endregion

        #region DATA Calls




        internal static int TestConnection(DWCredentials Connect, string url)
        {
            using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(url))
            {
                return client.TestConnection(Connect.BuildConnectObject());
            }

        }
        private static DataSet GetReportDataPage(DWCredentials Connect, List<string[]> parameters, int from, int to)
        {
            using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(address))
            {
                List<string[]> listParameters = new List<string[]>() { };
                return client.GetReportDataPaged(parameters.ToArray(), from, to, Connect.BuildConnectObject());
            }
        }
        private static DataSet GetDataSet(string sql, DWCredentials Connect)
        {
            using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(address))
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
                return client.GetDataSetInternal1(sql, Connect.BuildConnectObject(sql));
            }
        }
        private static DataSet GetDataSet(string sql, string[] parameter, DWCredentials Connect)
        {
            string connect = Connect.BuildConnectObject(sql);
            // if (isDebugging)
            //{  
            Log.AppendToFile = true;
            Log.Console = true;
            Log.LogPath = @"D:\Logs";
            Log.FileName = "Relational ReportData";
            string p = parameter == null ? "null" : string.Join("|", parameter);
            string s = "Requesting Data from GetDataSetInternal2 --->" + "sql :[" + sql + "]" + "parameters:[" + p + "]" + "connect:[" + connect + "]";
            Log.Write(s);
            //}

            using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(address))
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
                return client.GetDataSetInternal2(sql, parameter, connect);
            }
        }
        private static DataSet GetDataSet(string sql, List<string[]> parameters, DWCredentials Connect)
        {
            using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(address))
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
                return client.GetDataSetInternal3(sql, parameters.ToArray(), Connect.BuildConnectObject(sql));
            }
        }








        //internal static int TestConnection(DWCredentials Connect, string url)
        //{
        //    using (Relational.Core.Client.RelationalProxy.RelationalServiceClient client = new Relational.Core.Client.RelationalClient().GetClient(url))
        //    {
        //        return client.TestConnection(Connect.BuildConnectObject());
        //    }
        //}
        //private static DataSet GetReportDataPage(DWCredentials Connect, List<string[]> parameters, int from, int to)
        //{
        //    using (RelationalProxy.RelationalServiceClient client = new RelationalProxy.RelationalServiceClient("BasicHttpBinding_IRelationalService",address))
        //    {
        //        List<string[]> listParameters = new List<string[]>() { };
        //        return client.GetReportDataPaged(parameters.ToArray(), from, to, Connect.BuildConnectObject());
        //    }
        //}
        //private static DataSet GetDataSet(string sql, DWCredentials Connect)
        //{
        //    using (RelationalProxy.RelationalServiceClient client = new RelationalProxy.RelationalServiceClient("BasicHttpBinding_IRelationalService", address))
        //    {
        //        client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
        //        return client.GetDataSetInternal1(sql, Connect.BuildConnectObject(sql));
        //    }
        //}
        //private static DataSet GetDataSet(string sql, string[] parameter, DWCredentials Connect)
        //{
        //    using (RelationalProxy.RelationalServiceClient client = new RelationalProxy.RelationalServiceClient("BasicHttpBinding_IRelationalService", address))
        //    {
        //        client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
        //        return client.GetDataSetInternal2(sql, parameter, Connect.BuildConnectObject(sql));
        //    }
        //}
        //private static DataSet GetDataSet(string sql, List<string[]> parameters, DWCredentials Connect)
        //{
        //    using (RelationalProxy.RelationalServiceClient client = new RelationalProxy.RelationalServiceClient("BasicHttpBinding_IRelationalService", address))
        //    {
        //        client.InnerChannel.OperationTimeout = new TimeSpan(00, 50, 00);
        //        return client.GetDataSetInternal3(sql, parameters.ToArray(), Connect.BuildConnectObject(sql));
        //    }
        //}
        #endregion




    }
}


